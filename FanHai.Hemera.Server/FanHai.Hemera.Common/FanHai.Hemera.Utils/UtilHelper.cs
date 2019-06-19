using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.IO;
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Utils
{
    public class UtilHelper
    {
        /// <summary>
        /// 生成新的GUID。
        /// </summary>
        /// <param name="i">GUID的后缀。</param>
        /// <returns>GUID字符串。</returns>
        public static string GenerateNewKey(int i)
        {
            return System.Guid.NewGuid() + "-" + i.ToString("000");
        }

        public static DataTable ConvertDataRowToDataTable(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return null;
            }

            DataTable tbData = new DataTable();
            tbData.Columns.Add("name");
            tbData.Columns.Add("value");
            string strKey = "", strValue = "";
            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                strKey = dataRow.Table.Columns[i].ColumnName;
                strValue = dataRow[strKey].ToString();
                DataRow dRow = tbData.NewRow();
                dRow[0] = strKey;
                dRow[1] = strValue;
                tbData.Rows.Add(dRow);
            }
            return tbData;
        }




        public static string BuilderWhereConditionString(string strFieldName, string[] strFieldValues)
        {
            string strReturn = "";
            string strTemp = " AND " + strFieldName + " IN (";
            if (strFieldValues.Length > 0)
            {
                for (int i = 0; i < strFieldValues.Length; i++)
                {
                    strTemp = strTemp + "'" + strFieldValues[i].Trim().PreventSQLInjection() + "',";
                }
                strTemp = strTemp.Substring(0, strTemp.Length - 1) + ")";
                strReturn = strTemp;
            }
            return strReturn;
        }


        /// <summary>
        /// Create Specify Columns Data Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        /// Owner:Andy Gao 2011-05-11 13:05:26
        public static DataTable CreateSpecifyColumnsDataTable(string tableName, params string[] columnNames)
        {
            DataTable dataTable = new DataTable(tableName);

            foreach (String columnName in columnNames)
            {
                dataTable.Columns.Add(columnName);
            }

            return dataTable;
        }

        private static readonly object objectLock = new object();

        /// <summary>
        /// Save DataSet Data To Specified Xml File
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// Owner:Andy Gao 2011-06-27 14:28:53
        public static void SaveDataSetData(DataSet ds, string directoryName, string fileName)
        {
            try
            {
                string dirPath = string.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, directoryName);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string filePath = string.Format("{0}{1}.XML", dirPath, fileName);

                lock (objectLock)
                {
                    if (ds != null)
                    {
                        ds.WriteXml(filePath);
                    }
                    else
                    {
                        File.AppendText(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("SaveDataSetData Error:" + ex.Message);
            }
        }

        public const string TABLE_RETURN_PARAM = "paraTable";
        /// <summary>
        /// 检查数据记录是否过期。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="tableName">数据库表名称。</param>
        /// <param name="conditions">查询条件。</param>
        /// <param name="oldEditTime">旧的编辑时间。</param>
        /// <returns>true表示过期，false表示没有过期。</returns>
        public static bool CheckRecordExpired(Database db, string tableName,
                                              List<KeyValuePair<string, string>> conditions, string oldEditTime)
        {
            string sql = "SELECT EDIT_TIME FROM " + tableName + " WHERE 1=1";
            if (conditions.Count > 0)
            {
                foreach (KeyValuePair<string, string> keyValue in conditions)
                {
                    sql += " AND " + keyValue.Key + "='" + keyValue.Value + "'";
                }

                try
                {
                    using (IDataReader read = db.ExecuteReader(CommandType.Text, sql))
                    {
                        if (read.Read())
                        {
                            string newEditTime = Convert.ToString(read["EDIT_TIME"]);
                            if (newEditTime != null && newEditTime != string.Empty && oldEditTime != null && oldEditTime != string.Empty)
                            {
                                if (Convert.ToDateTime(oldEditTime) != Convert.ToDateTime(newEditTime))
                                    return true;
                            }
                        }
                        read.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取数据库的当前日期时间。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <returns>数据库的当前日期时间。</returns>
        public static DateTime GetSysdate(Database db)
        {
            string sql = @"SELECT GETDATE()";
            object o = db.ExecuteScalar(CommandType.Text, sql);
            return Convert.ToDateTime(o);
        }
        /// <summary>
        /// 根据时间获取日排班主键。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="time">指定时间字符串 yyyy-MM-dd HH:mm:ss。</param>
        /// <returns>日排班主键。</returns>
        public static string GetShiftKey(Database db, string time)
        {
            string shiftName = string.Empty;
            string sql =string.Format(@"SELECT A.DKEY
                                        FROM CAL_SCHEDULE_DAY A
                                        WHERE '{0}' BETWEEN A.STARTTIME AND A.ENDTIME", 
                                        time.PreventSQLInjection());
            object o = db.ExecuteScalar(CommandType.Text, sql);
            if (o != null && o != DBNull.Value)
            {
                shiftName = o.ToString();
            }
            else
            {
                throw new Exception("请维护排班计划");
            }
            return shiftName;
        }
        /// <summary>
        /// 根据指定日期时间获取班别值。
        /// </summary>
        /// <param name="db">数据库对象。</param>
        /// <param name="time">指定时间字符串 yyyy-MM-dd HH:mm:ss。</param>
        /// <returns>当前班别值。</returns>
        public static string GetShiftValue(Database db, string time)
        {
            string shiftValue = string.Empty;
            string sql =string.Format(@"SELECT A.SHIFT_VALUE
                                        FROM CAL_SCHEDULE_DAY A
                                        WHERE '{0}' BETWEEN A.STARTTIME AND A.ENDTIME",
                                        time.PreventSQLInjection());
            object o = db.ExecuteScalar(CommandType.Text, sql);
            if (o != null && o != DBNull.Value)
            {
                shiftValue = o.ToString();
            }
            else
            {
                throw new Exception("请维护排班计划");
            }
            return shiftValue;
        }
        /// <summary>
        /// 获取年月日。年（2码）月（1码）日（1码）。
        /// </summary>
        /// <returns>
        /// 年（2码）月（1码）日（1码）
        /// </returns>
        public static string GetYYMD()
        {
            string strYYMd = null;
            string strDate = DateTime.Now.ToString("yyMMdd");
            string strYear = strDate.Substring(0, 2);
            string strMonth = strDate.Substring(2, 2);
            string strDay = strDate.Substring(4, 2);
            strYYMd = strYear;
            if (strMonth.Equals("10"))
            {
                strYYMd = strYYMd + "A";
            }
            else if (strMonth.Equals("11"))
            {
                strYYMd = strYYMd + "B";
            }
            else if (strMonth.Equals("12"))
            {
                strYYMd = strYYMd + "C";
            }
            else                //等于本身了
            {
                strYYMd = strYYMd + strMonth.Substring(1, 1);
            }
            string strAllDay = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";//截取第几码
            strYYMd = strYYMd + strAllDay.Substring(int.Parse(strDay) - 1, 1);
            return strYYMd;
        }
    }
}
 
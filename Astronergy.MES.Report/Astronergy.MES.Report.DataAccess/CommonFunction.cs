using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Astronergy.MES.Report.DataAccess
{
    /// <summary>
    /// 为SQL扩展<see cref="String"/>方法。
    /// </summary>
    public static class StringForSQLExtend
    {
        /// <summary>
        /// 防止拼凑SQL字符串的参数值进行SQL注入攻击。
        /// </summary>
        /// <param name="val">用于拼凑SQL字符串的参数值字符串。</param>
        /// <returns>过滤或替换过SQL关键字的字符串。</returns>
        public static string PreventSQLInjection(this string val)
        {
            if (string.IsNullOrEmpty(val)) return val;
            return val.Replace("'", "''");
        }
    }
    /// <summary>
    /// 通用类。
    /// </summary>
    public sealed class CommonFunction
    {
        private static Database _db = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 根据指定车间名称和产品类型获取工序及其排序信息
        /// </summary>
        /// <param name="roomName">车间名称。</param>
        /// <param name="partType">产品类型。</param>
        /// <returns>包含工序及其排序信息的数据集。</returns>
        public static DataSet GetOperations(string roomName, string partType)
        {
            const string storeProcedureName = "SP_QRY_OPERATION_BY_ROOMNAME";
            DataSet ds = null;
            using (DbConnection con = _db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                _db.AddInParameter(cmd, "p_roomName", DbType.String, roomName);
                _db.AddInParameter(cmd, "p_partType", DbType.String, partType);
                ds=_db.ExecuteDataSet(cmd);
            }
            return ds;
        }
        /// <summary>
        /// 获得车间数据。
        /// </summary>
        public static DataTable GetFactoryWorkPlace()
        {
            string sql = @"SELECT LOCATION_KEY,LOCATION_NAME
                           FROM FMM_LOCATION 
                           WHERE LOCATION_LEVEL=5
                           ORDER BY LOCATION_NAME";
            return _db.ExecuteDataSet(CommandType.Text,sql).Tables[0];
        }
        /// <summary>
        /// 获得产品ID号数据。
        /// </summary>
        public static DataTable GetProId()
        {
            string sql = @"SELECT PRODUCT_CODE 
                          FROM POR_PRODUCT
                          WHERE ISFLAG=1 
                          ORDER BY PRODUCT_CODE ASC ";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 获取产品料号数据。
        /// </summary>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public static DataTable GetPartNumber()
        {
            string sql = @"SELECT PART_ID PART_NUMBER
                           FROM POR_PART
                           WHERE PART_STATUS=1 
                           ORDER BY PART_ID";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取产品料号数据。
        /// </summary>
        /// <param name="orderNumber">工单号,多个工单号使用逗号(,)分隔。</param>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public static DataTable GetPartNumber(string orderNumber)
        {
            string sql = string.Format(@"SELECT ISNULL(b.PART_NUMBER,a.PART_NUMBER) PART_NUMBER
                                        FROM POR_WORK_ORDER a
                                        INNER JOIN dbo.SplitStringToTable('{0}') e ON e.VAL=a.ORDER_NUMBER
                                        LEFT JOIN POR_WO_PRD b ON b.WORK_ORDER_KEY=a.WORK_ORDER_KEY
                                        WHERE b.IS_USED='Y'",
                                        orderNumber.PreventSQLInjection());
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        public static DataTable GetWorkOrder()
        {
            string sql = @"select t.WORK_ORDER_NO from POR_LOT t  group by t.WORK_ORDER_NO";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获得产品型号数据。
        /// </summary>
        public static DataTable GetProductModel()
        {
            string sql = @"SELECT DISTINCT PROMODEL_NAME
                        FROM dbo.BASE_PRODUCTMODEL
                        WHERE ISFLAG=1";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 获取线别
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLine()
        {
            string sql = @"select distinct LINE_NAME
                        FROM FMM_PRODUCTION_LINE";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获得客户类别。
        /// </summary>
        public static DataTable GetCustomerType()
        {
            string sql = @"SELECT DISTINCT CUSTOMER_TYPE,
                                  CASE WHEN CUSTOMER_TYPE='常规' THEN 0 ELSE 1 END ORDER_SEQ
                          FROM V_WORK_ORDER_ATTR
                          ORDER BY ORDER_SEQ,CUSTOMER_TYPE";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 获得设备数据。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEquipments(string roomKey)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"SELECT DISTINCT EQUIPMENT_CODE,EQUIPMENT_KEY 
                           FROM EMS_EQUIPMENTS a
                           LEFT JOIN V_LOCATION l ON a.LOCATION_KEY=l.AREA_KEY");
             if (!string.IsNullOrEmpty(roomKey))
            {
                sbSql.AppendFormat(" WHERE l.ROOM_KEY='{0}'", roomKey.PreventSQLInjection());
            }
             sbSql.Append(" ORDER BY EQUIPMENT_CODE");
             return _db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获得存在在制品的工单数据。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLotWorkOrderNumber(string roomKey)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"SELECT distinct WORK_ORDER_NO
                           FROM POR_LOT
                           WHERE STATUS<2
                           AND CREATE_TIME>=DATEADD(D,-30,GETDATE())");
            if (!string.IsNullOrEmpty(roomKey))
            {
                sbSql.AppendFormat(" AND FACTORYROOM_KEY='{0}'", roomKey.PreventSQLInjection());
            }
            sbSql.Append(" ORDER BY WORK_ORDER_NO");
            return _db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
        }

        public static DataTable GetLotWorkProId(string roomKey)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"select distinct t.PRO_ID from POR_LOT t 
                            where t.STATUS<'2' ");
            if (!string.IsNullOrEmpty(roomKey))
            {
                sbSql.AppendFormat(" AND FACTORYROOM_KEY='{0}'", roomKey.PreventSQLInjection());
            }
            sbSql.Append(" ORDER BY PRO_ID ");
            return _db.ExecuteDataSet(CommandType.Text, sbSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取当期数据库时间。
        /// </summary>
        /// <returns>数据库时间。</returns>
        public static DateTime GetCurrentDateTime()
        {
            string sql = @"SELECT SYSDATETIME()";
            return Convert.ToDateTime(_db.ExecuteScalar(CommandType.Text, sql));
        }
        /// <summary>
        /// 获得班别信息(行政排班)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetShift()
        {
            string sql = @"SELECT T.ITEM_ORDER,
                            MAX( case T.ATTRIBUTE_NAME when 'CODE' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as SHIFT_VALUE
                            FROM CRM_ATTRIBUTE           T,
                            BASE_ATTRIBUTE          T1,
                            BASE_ATTRIBUTE_CATEGORY T2
                            WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY
                            AND T1.CATEGORY_KEY = T2.CATEGORY_KEY
                            AND UPPER(T2.CATEGORY_NAME) = 'BASIC_SHIFT'
                            GROUP BY T.ITEM_ORDER  ";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 获得白班，晚班信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCAL_SHIFT()
        {
            string sql = @"SELECT  [SHIFT_KEY],[SHIFT_NAME],[START_TIME],[END_TIME],[START_DAY_OF_SHIFT]
                            ,[CYCLE_DAYS],[SHIFT_TYPE],[OVER_DAY],[DESCRIPTIONS],[CREATOR],[CREATE_TIME],[CREATE_TIMEZONE]
                            ,[EDITOR],[EDIT_TIME],[EDIT_TIMEZONE]
                            FROM [CAL_SHIFT]";
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 获得车间所有区域
        /// </summary>
        /// <param name="workplaceKey"></param>
        /// <returns></returns>
        /// Owner Genchille.yang 2012-04-12 09:33:56
        public static DataTable GetFactoryWorkPlaceAreas(string workplaceKey)
        {
            string sql = string.Format(@"SELECT DISTINCT A.EQUIPMENT_KEY, A.EQUIPMENT_NAME,A.EQUIPMENT_CODE
                                          FROM EMS_EQUIPMENTS A
                                         WHERE A.LOCATION_KEY IN
                                               (SELECT T.LOCATION_KEY
                                                  FROM FMM_LOCATION T, FMM_LOCATION_RET T1
                                                 WHERE T.LOCATION_KEY = T1.LOCATION_KEY
                                                   AND T1.LOCATION_LEVEL = 9
                                                   AND T1.PARENT_LOC_KEY = '{0}')
                                        ", workplaceKey);
            return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 根据车间，工序获得的开始结束时间
        /// </summary>
        /// <param name="location">车间ID/车间名称</param>
        /// <param name="operation">工序ID/工序名称</param>
        /// <param name="errMsg">错误信息反馈</param>
        /// <returns>list[工序的开始时间，工序结束时间]</returns>
        /// Owner Genchille.yang 2013-05-13 09:33:56
        public static string[] GetOptShiftDate(string location, string operation,  out string errMsg)
        {
            return GetOptShiftDate(location, operation, string.Empty,out errMsg, string.Empty);
        }
        /// <summary>
        /// 根据车间，工序获得的开始结束时间
        /// </summary>
        /// <param name="location">车间ID/车间名称</param>
        /// <param name="operation">工序ID/工序名称</param>
        /// <param name="shift">班别ID/班别名称</param>
        /// <param name="errMsg">错误信息反馈</param>
        /// <returns>list[工序的开始时间，工序结束时间]</returns>
        /// Owner Genchille.yang 2013-05-13 09:33:56
        public static string[] GetOptShiftDate(string location, string operation,  string shift,out string errMsg)
        {
            return GetOptShiftDate(location, operation, shift, out errMsg, string.Empty);
        }
        /// <summary>
        /// 根据车间，工序获得的开始结束日期+时间
        /// </summary>
        /// <param name="location">车间ID/车间名称</param>
        /// <param name="operation">工序ID/工序名称</param>
        /// <param name="errMsg">错误信息反馈</param>
        /// <param name="s_date">日期，表示哪一天的日期</param>
        /// <returns>list[工序的开始时间，工序结束时间]</returns>
        /// Owner Genchille.yang 2013-05-13 09:33:56
        public static string[] GetOptShiftDate(string location, string operation, out string errMsg, string s_date)
        {
            return GetOptShiftDate(location, operation, string.Empty, out errMsg, s_date);
        }
        /// <summary>
        /// 根据车间，工序，班别获取开始时间和结束时间。
        /// </summary>
        /// <param name="location">车间ID/车间名称</param>
        /// <param name="operation">工序ID/工序名称</param>
        /// <param name="shift">班别ID/班别名称</param>
        /// <param name="s_date">日期，表示哪一天的日期</param>
        /// <param name="errMsg">错误信息反馈</param>
        /// <returns>list[日期的开始时间，日期的结束时间]</returns>
        /// Owner Genchille.yang 2013-05-13 09:33:56
        public static string[] GetOptShiftDate(string location, string operation, string shift,out string errMsg,string s_date)
        {
            errMsg = string.Empty;
            string[] lst = new string[2];
            string startdatetime = string.Empty, endatetime = string.Empty;
            //如果车间为空，返回08:00到第二天08:00的日期
            if (string.IsNullOrEmpty(location))
            {
                if (string.IsNullOrEmpty(s_date))
                    s_date = GetCurrentDateTime().ToString("yyyy-MM-dd");

                if (string.IsNullOrEmpty(shift) || shift.ToUpper().Equals("ALL"))
                {
                    startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + "08:00:00";
                    endatetime = Convert.ToDateTime(s_date).AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
                }
                else
                {
                    if (shift.Equals("白班"))
                    {
                        startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + "08:00:00";
                        endatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + "20:00:00";
                    }
                    else
                    {
                        startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + "20:00:00";
                        endatetime = Convert.ToDateTime(s_date).AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
                    }
                }

                lst[0] = startdatetime;
                lst[1] = endatetime;
                return lst;
            }

            try
            {
                string sqlCommand = string.Format(@"SELECT [START_TIME],[END_TIME],[OVER_DAY]
                                                FROM [BASE_OPT_SETTING] t where t.isflag=1 ");
                if (!string.IsNullOrEmpty(location))
                    sqlCommand += string.Format(@" and (t.LOCATION_KEY='{0}' or t.LOCATION_NAME='{0}')", location.PreventSQLInjection());
                if (!string.IsNullOrEmpty(operation))
                    sqlCommand += string.Format(@" and (t.OPERATION_KEY='{0}' or t.OPERATION_NAME='{0}')", operation.PreventSQLInjection());
                if (!string.IsNullOrEmpty(shift))
                    sqlCommand += string.Format(@" and (t.SHIFT_KEY='{0}' or t.SHIFT_NAME=N'{0}')", shift.PreventSQLInjection());
                DataTable dtDate = _db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
             
                if (dtDate.Rows.Count > 0)
                {
                    foreach (DataRow drDate in dtDate.Rows)
                    {
                        string overday = Convert.ToString(drDate["OVER_DAY"]);
                        //返回日期+时间
                        #region
                        if (string.IsNullOrEmpty(s_date))
                            s_date = GetCurrentDateTime().ToString("yyyy-MM-dd");


                        //按班别获取-不跨天
                        if (!string.IsNullOrEmpty(shift))
                        {
                            if (bool.Parse(overday))
                            {
                                startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["START_TIME"]) + ":00";
                                endatetime = Convert.ToDateTime(s_date).AddDays(1).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["END_TIME"]) + ":00";
                            }
                            if (!bool.Parse(overday))
                            {
                                startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["START_TIME"]) + ":00";
                                endatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["END_TIME"]) + ":00";
                            }
                        }
                        //不按班别获取-跨天
                        else
                        {
                            if (bool.Parse(overday))
                            {
                                endatetime = Convert.ToDateTime(s_date).AddDays(1).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["END_TIME"]) + ":00";
                            }
                            if (!bool.Parse(overday))
                            {
                                startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + Convert.ToString(drDate["START_TIME"]) + ":00";
                            }
                        }                      
                        #endregion

                    }
                    if (string.IsNullOrEmpty(startdatetime) || string.IsNullOrEmpty(endatetime))
                    {
                        startdatetime = GetCurrentDateTime().ToString("yyyy-MM-dd ") + "08:00:00";
                        endatetime = GetCurrentDateTime().AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(s_date))
                        s_date = GetCurrentDateTime().ToString("yyyy-MM-dd");

                    startdatetime = Convert.ToDateTime(s_date).ToString("yyyy-MM-dd ") + "08:00:00";
                    endatetime = Convert.ToDateTime(s_date).AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            lst[0] = startdatetime;
            lst[1] = endatetime;

            return lst;
        }
    }
}

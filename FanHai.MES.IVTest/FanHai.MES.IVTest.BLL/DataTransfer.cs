using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.Common;

namespace FanHai.MES.IVTest.BLL
{
    /// <summary>
    /// 字符串扩展类。
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 防止SQL注入。
        /// </summary>
        /// <param name="obj">字符串。</param>
        /// <returns>处理过的字符串。</returns>
        public static string PreventSQLInjection(this string obj)
        {
            if (obj == null) return obj;
            return obj.Replace("'","''");
        }
    }
    /// <summary>
    /// IV测试设备类型。
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 设备类型1 
        /// </summary>
        SunData=1,
        /// <summary>
        /// 设备类型2 
        /// </summary>
        Results=2
    }
    /// <summary>
    /// Access数据转到SQL Server数据库中。
    /// </summary>
    public class DataTransfer
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="accessConnString">Access数据库连接字符串。</param>
        /// <param name="sqlServerConnString">SqlServer数据库连接字符串。</param>
        public DataTransfer(string accessConnString, string sqlServerConnString)
        {
            this.AccessConnectionString = accessConnString;
            this.SqlServerConnectionString = sqlServerConnString;
        }
        /// <summary>
        /// Access数据库连接字符串。
        /// </summary>
        private string AccessConnectionString
        {
            get;
            set;
        }
        /// <summary>
        /// SqlServer数据库连接字符串。
        /// </summary>
        public string SqlServerConnectionString
        {
            get;
            set;
        }
        /// <summary>
        /// 上一次执行转置的数据数量。
        /// </summary>
        public int TransferCount
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取数据文件路径。先按照式化字符串中设置的格式寻找，如果没有匹配则返回文件夹中最新的文件。
        /// </summary>
        /// <param name="path">数据文件所在文件夹路径。</param>
        /// <param name="format">数据文件名称的格式化字符串。</param>
        /// <returns>
        /// 数据文件路径。
        /// </returns>
        public static string GetFullFile(string path,string format)
        {
            string strFileFullName = string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                return strFileFullName;
            }
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            FileSystemInfo[] fileinfo = TheFolder.GetFileSystemInfos("*.mdb");
            DateTime dtLastWrite = DateTime.MinValue;
            FileInfo fiLastWrite = null;
            DateTime dtFileName = DateTime.Now;
            //根据分选测试机早上8点钟才会更换测试数据文件的属性，增加下面的判断
            if (DateTime.Now >= Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00") 
                && DateTime.Now < Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 08:00:00"))
            {
                dtFileName = dtFileName.AddDays(-1);
            }
            string fileName = string.Format(format, dtFileName);
            //获取自定义的文件路径。
            if (string.IsNullOrEmpty(strFileFullName))
            {
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is FileInfo && i.Name == fileName)
                    {
                        strFileFullName = i.FullName;
                        break;
                    }
                }
            }
            //获取最新的文件。
            if (string.IsNullOrEmpty(strFileFullName))
            {
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is FileInfo && i.LastWriteTime > dtLastWrite)
                    {
                        dtLastWrite = i.LastWriteTime;
                        fiLastWrite = i as FileInfo;
                    }
                }
                if (fiLastWrite != null)
                {
                    strFileFullName = fiLastWrite.FullName;
                }
            }
            return strFileFullName;
        }
        /// <summary>
        /// 将数据从Access数据库转移到SqlServer数据库。
        /// </summary>
        /// <param name="deviceName">设备代码名称。</param>
        /// <param name="deviceType">设备类型。</param>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true:转置成功。false：转置失败。</returns>
        public bool AccessToSqlServer(string deviceName,DeviceType deviceType,string lotNumber)
        {
            SqlConnection sqlCon = null;
            SqlCommand sqlCmd = null;
            OleDbConnection oleCon = null;
            OleDbCommand oleCmd = null;
            bool bAllSuccess = true;
            DateTime dtMaxTestTime = new DateTime(1900,01,01);
            try
            {
                //创建SQL Server 和 Access的连接对象。
                sqlCon = new SqlConnection(this.SqlServerConnectionString);
                sqlCon.Open();
                sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandType = CommandType.Text;
                oleCon = new OleDbConnection(this.AccessConnectionString);
                oleCon.Open();
                oleCmd = oleCon.CreateCommand();
                oleCmd.CommandType = CommandType.Text;
                //根据设备代码名称和批次号 获取SQL Server数据库中最大的测试时间值。
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(@"SELECT MAX(TTIME) FROM WIP_IV_TEST WHERE DEVICENUM='{0}'",
                                    deviceName.PreventSQLInjection());
                if (!string.IsNullOrEmpty(lotNumber))
                {
                    sbSql.AppendFormat(" AND LOT_NUM='{0}'", lotNumber.PreventSQLInjection());
                }
                sqlCmd.CommandText = sbSql.ToString();
                object maxTestTime = sqlCmd.ExecuteScalar();
                if (maxTestTime != null && maxTestTime != DBNull.Value)
                {
                    dtMaxTestTime = Convert.ToDateTime(maxTestTime);
                }
                sbSql.Remove(0,sbSql.Length);
                string sql = GetQueryTestDataSql(deviceType);
                sbSql.AppendFormat("SELECT a.* FROM ({0}) a WHERE a.TTIME>'{1}'", 
                                    sql, 
                                    dtMaxTestTime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(lotNumber))
                {
                    sbSql.AppendFormat(" AND a.LOT_NUM='{0}'", lotNumber.PreventSQLInjection());
                }
                //从Access数据库获取>开始日期和开始时间的数据。
                oleCmd.CommandType = CommandType.Text;
                oleCmd.CommandText = sbSql.ToString();
                IDataReader reader = oleCmd.ExecuteReader();
                this.TransferCount = 0;
                //遍历查询到的所有数据。
                while (reader.Read())
                {
                    string lotNum = Convert.ToString(reader["LOT_NUM"]);
                    string testTime = Convert.ToString(reader["TTIME"]);
                    SqlTransaction dbTrans = sqlCon.BeginTransaction();
                    try
                    {
                        sqlCmd.Transaction = dbTrans;
                        //判断SQL Server是否lotNum+testTime+设备号相同的记录。如果有则代表数据已经读取过。
                        sqlCmd.CommandText = string.Format(@"SELECT COUNT(*) FROM WIP_IV_TEST 
                                                            WHERE DEVICENUM='{0}' AND LOT_NUM='{1}' AND TTIME='{2}'",
                                                            deviceName.PreventSQLInjection(),
                                                            lotNum.PreventSQLInjection(),
                                                            testTime.PreventSQLInjection());

                        int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                        //如果存在RowID标识的数据。则继续执行下一个数据
                        if (count > 0)
                        {
                            continue;
                        }
                        InsertDataToSqlServer(sqlCmd, reader, deviceName);
                        dbTrans.Commit();
                        this.TransferCount++;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        bAllSuccess = false;
                        EventLog.WriteEntry("FanHai.MES.IVTest", string.Format("{0}-{1}-{2}-{3}", deviceName, lotNum, testTime, ex.Message), EventLogEntryType.Error);
                        continue;
                    }
                    finally
                    {
                        if (dbTrans != null)
                        {
                            dbTrans.Dispose();
                            dbTrans = null;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                bAllSuccess = false;
                EventLog.WriteEntry("FanHai.MES.IVTest", ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                if (sqlCon != null)
                {
                    sqlCon.Close();
                    sqlCon.Dispose();
                    sqlCon = null;
                }
                if (oleCon != null)
                {
                    oleCon.Close();
                    oleCon.Dispose();
                    oleCon = null;
                }
            }
            return bAllSuccess;
        }
        /// <summary>
        /// 获取查询测试数据的SQL字符串。
        /// </summary>
        /// <param name="type">设备类型。</param>
        /// <returns>查询测试数据的SQL字符串。</returns>
        private string GetQueryTestDataSql(DeviceType type)
        {
            //--TTIME 测试时间
            //--LOT_NUM 批次号
            //--FF 填充因子
            //--ISC 测试短路电流
            //--EFF 组件转换效率
            //--RSH 串联电阻
            //--RS 并联电阻
            //--VOC 测试开路电压
            //--IPM 测试最大电流
            //--PM 测试最大功率
            //--AMBIENTTEMP 测度温度
            //--SENSORTEMP 环境温度
            //--INTENSITY 光强
            string sql = string.Empty;
            if (type == DeviceType.SunData)
            {
                sql = @"SELECT Format(SunData.[DateTime],'yyyy-MM-dd HH:mm:ss') AS TTIME,
	                        SunData.[ID] AS LOT_NUM,
	                        SunData.[FF] AS FF,
	                        SunData.[Isc] AS ISC,
	                        SunData.[Eff] AS EFF,
	                        SunData.[Rsh] AS RSH,
	                        SunData.[Rs] AS RS,
	                        SunData.[Voc] AS VOC,
	                        SunData.[Imax] AS IPM,
	                        SunData.[Vmax] AS VPM,
	                        SunData.[Pmax] AS PM,
	                        SunData.[Temp] AS AMBIENTTEMP,
	                        SunData.[EnvTemp] AS SENSORTEMP, 
	                        SunData.[Sun] AS INTENSITY
                        FROM SunData";
            }
            else
            {
                sql = @"SELECT  Format(Results.Test_Date,'yyyy-MM-dd ')+Format(Results.Test_Time,'HH:mm:ss') AS TTIME,
	                        Results.ID AS LOT_NUM,
	                        Results.FF AS FF,
	                        Results.Isc AS ISC,
	                        Results.ModEff AS EFF,
	                        Results.Rsh AS RSH,
	                        Results.Rs AS RS,
	                        Results.Voc AS VOC,
	                        Results.Ipm AS IPM,
	                        Results.Vpm AS VPM,
	                        Results.Pmax AS PM,
	                        Results.TMod AS AMBIENTTEMP,
	                        Results.TRef1 AS SENSORTEMP,
	                        Results.CIrr AS INTENSITY
                        FROM Results";
            }
            return sql;
        }
        /// <summary>
        /// 更新测试数据标志。
        /// </summary>
        /// <param name="oleCmd">SQL命令对象。</param>
        /// <param name="reader">测试数据记录。</param>
        /// <param name="type">设备类型。</param>
        private void UpdateTestDataFlag(OleDbCommand oleCmd, IDataReader reader, DeviceType type)
        {
            string lotNum = Convert.ToString(reader["LOT_NUM"]);
            string testTime = Convert.ToString(reader["TTIME"]);
            //设备类型是Results型的,更新标志为TITLE
            if (type == DeviceType.Results)
            {
                oleCmd.CommandText = string.Format(@"UPDATE Results SET TITLE='Y' 
                                             WHERE ID='{0}'
                                             AND Format(Results.Test_Date,'yyyy-MM-dd ')+Format(Results.Test_Time,'HH:mm:ss')='{1}'",
                                             lotNum.PreventSQLInjection(),
                                             testTime.PreventSQLInjection());
                oleCmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 向SQL Server中插入数据。
        /// </summary>
        /// <param name="sqlCmd">SQL命令对象。</param>
        /// <param name="reader">测试数据记录。</param>
        private void InsertDataToSqlServer(SqlCommand sqlCmd, IDataReader reader, string deviceName)
        {
            string lotNum = Convert.ToString(reader["LOT_NUM"]);//组件序列号
            string ttime = Convert.ToString(reader["TTIME"]);//测试时间
            string tdate = ttime.Length > 10 ? ttime.Substring(0, 10) : ttime;
            //根据设备代码+组件序列号获取最大ID
            sqlCmd.CommandText = string.Format(@"SELECT ISNULL(MAX(L_ID),0)+1 FROM WIP_IV_TEST 
                                                 WHERE DEVICENUM='{0}' AND LOT_NUM='{1}'",
                                                deviceName.PreventSQLInjection(),
                                                lotNum.PreventSQLInjection());
            object maxLId = Convert.ToInt32(sqlCmd.ExecuteScalar());
            //根据批次号获取电池片效率
            sqlCmd.CommandText = string.Format(@"SELECT TOP 1 EFFICIENCY FROM POR_LOT WHERE LOT_NUMBER='{0}'",
                                                lotNum.PreventSQLInjection());
            string celleff = Convert.ToString(sqlCmd.ExecuteScalar());
            //更新之前的测试数据为非默认数据
            sqlCmd.CommandText = string.Format(@"UPDATE WIP_IV_TEST SET VC_DEFAULT=0 WHERE LOT_NUM='{0}'",
                                                lotNum.PreventSQLInjection());
            sqlCmd.ExecuteNonQuery();
            //-----------------------------------
            //只有组件在当前站才可以改变测试值为默认值。
            //需要实现
            //-----------------------------------
            //如果序列号以JZ开头，代表是校准版，则更新设备校准版记录。
            if (lotNum.StartsWith("JZ"))
            {
                //更新设备校准板信息。
                sqlCmd.CommandText = string.Format(@"UPDATE WIP_CALIBRATION_INFO
                                                    SET CALIBRATION_NO='{1}',CALIBRATION_TIME=GETDATE()
                                                    WHERE MACHINE_NO='{0}'",
                                                    deviceName.PreventSQLInjection(),
                                                    lotNum.PreventSQLInjection());
                int result=sqlCmd.ExecuteNonQuery();
                //如果更新记录数<1,则插入一笔设备校准板记录
                if(result<1){
                    sqlCmd.CommandText = string.Format(@"INSERT INTO WIP_CALIBRATION_INFO(MACHINE_NO,CALIBRATION_NO,CALIBRATION_TIME)
                                                    VALUES('{0}','{1}',GETDATE())",
                                                    deviceName.PreventSQLInjection(),
                                                    lotNum.PreventSQLInjection());
                    sqlCmd.ExecuteNonQuery();
                }
            }
            //获取校准板信息。
            sqlCmd.CommandText = string.Format("SELECT TOP 1 CALIBRATION_NO FROM WIP_CALIBRATION_INFO WHERE MACHINE_NO='{0}' ORDER BY CALIBRATION_TIME DESC",
                                               deviceName.PreventSQLInjection());
            string calibrationNo = Convert.ToString(sqlCmd.ExecuteScalar());
            //新增测试数据
            object ff = reader["FF"];//FF 填充因子
            object isc = reader["ISC"];//ISC 测试短路电流
            object eff = reader["EFF"];//--EFF 组件转换效率
            object rsh = reader["RSH"];//--RSH 串联电阻
            object rs = reader["RS"];//--RS 并联电阻
            object voc = reader["VOC"];//--VOC 测试开路电压
            object ipm = reader["IPM"];// --IPM 测试最大电流
            object vpm = reader["VPM"];//--VPM 测试最大电压
            object pm = reader["PM"];//--PM 测试最大功率
            object ambienttemp = reader["AMBIENTTEMP"];//--AMBIENTTEMP 测度温度
            object sensortemp= reader["SENSORTEMP"];//--SENSORTEMP 环境温度
            object intensity = reader["INTENSITY"];//--INTENSITY 光强
            string ivTestKey=Guid.NewGuid().ToString();
            //向SQL Server中插入对应的数据。
            sqlCmd.CommandText = string.Format(@"INSERT INTO WIP_IV_TEST
                                                (IV_TEST_KEY,T_DATE,TTIME,LOT_NUM,FF,ISC,EFF,RSH,RS,VOC,IPM,VPM,PM,AMBIENTTEMP,SENSORTEMP,INTENSITY,DEVICENUM,L_ID,VC_DEFAULT,CALIBRATION_NO,VC_CELLEFF) 
                                                 VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',1,'{18}','{19}')",
                                                 ivTestKey,
                                                 tdate,
                                                 ttime,
                                                 lotNum.PreventSQLInjection(),
                                                 ff,
                                                 isc,
                                                 eff,
                                                 rsh,
                                                 rs,
                                                 voc,
                                                 ipm,
                                                 vpm,
                                                 pm,
                                                 ambienttemp,
                                                 sensortemp,
                                                 intensity,
                                                 deviceName.PreventSQLInjection(),
                                                 maxLId,
                                                 calibrationNo.PreventSQLInjection(),
                                                 celleff.PreventSQLInjection());

            sqlCmd.ExecuteNonQuery();
        }
    }
}

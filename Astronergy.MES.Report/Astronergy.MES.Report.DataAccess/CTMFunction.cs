using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Astronergy.MES.Report.DataAccess
{
    public sealed class CTMFunction:BaseDBAccess
    {
        private static Database db = DatabaseFactory.CreateDatabase();

        public static DataSet GetFactoryWorkPlace()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT LOCATION_KEY,LOCATION_NAME";
            sql += " FROM FMM_LOCATION";
            sql += " WHERE LOCATION_LEVEL='5'";
            sql += " ORDER BY LOCATION_NAME";
            dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
            return dsReturn;
        }

        public static DataSet GetProID()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT PRODUCT_CODE";
            sql += " FROM POR_PRODUCT";
            sql += " WHERE ISFLAG='1'";
            sql += " ORDER BY PRODUCT_CODE ASC";
            dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
            return dsReturn;
        }

        public static DataSet GetWO()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT ORDER_NUMBER";
            sql += " FROM POR_WORK_ORDER";
            sql += " ORDER BY ORDER_NUMBER DESC";
            dsReturn = db.ExecuteDataSet(CommandType.Text,sql);
            return dsReturn;
        }

        public static DataSet GetCTMReportData(string sFactory, string sProID, string sWO, string sStartSN, string sEndSN, string sStartDate, string sEndDate, string sDeviceNo, string sDefault)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            sql = "SELECT ROW_NUMBER()OVER(ORDER BY M.LOT_NUM) AS 'NO',M.*,N.START_TIMESTAMP,N.EQUIPMENT_CODE";
            sql += " FROM (SELECT CONVERT(VARCHAR(10),A.T_DATE,120) AS 'T_DATE'";
            sql += ",CONVERT(VARCHAR(10),A.TTIME,108) AS 'TTIME',A.LOT_NUM,B.PRO_ID,CAST(A.AMBIENTTEMP AS DECIMAL(8,2)) AS 'AMBIENTTEMP'";
            sql += ",CAST(A.INTENSITY AS DECIMAL(8,2)) AS 'INTENSITY',CAST(A.FF*100 AS DECIMAL(8,2)) AS 'FF',CAST(A.EFF AS DECIMAL(8,2)) AS 'EFF'";
            sql += ",CAST(A.PM AS DECIMAL(8,2)) AS 'PM',CAST(A.ISC AS DECIMAL(8,2)) AS 'ISC',CAST(A.IPM AS DECIMAL(8,2)) AS 'IPM'";
            sql += ",CAST(A.VOC AS DECIMAL(8,2)) AS 'VOC',CAST(A.VPM AS DECIMAL(8,2)) AS 'VPM',A.DEVICENUM,A.VC_PSIGN,A.DT_PRINTDT,A.P_NUM";
            sql += ",A.VC_DEFAULT,CAST(A.SENSORTEMP AS DECIMAL(8,2)) AS 'SENSORTEMP',A.VC_CUSTCODE,A.C_USERID,CAST(A.COEF_PMAX AS DECIMAL(8,2)) AS 'COEF_PMAX'";
            sql += ",CAST(A.COEF_ISC AS DECIMAL(8,2)) AS 'COEF_ISC',CAST(A.COEF_VOC AS DECIMAL(8,2)) AS 'COEF_VOC',CAST(A.COEF_IMAX AS DECIMAL(8,2)) AS 'COEF_IMAX'";
            sql += ",CAST(A.COEF_VMAX AS DECIMAL(8,2)) AS 'COEF_VMAX',CAST(A.COEF_FF AS DECIMAL(8,2)) AS 'COEF_FF',A.VC_CELLEFF";
            sql += ",CAST(A.DEC_CTM AS DECIMAL(8,2)) AS 'DEC_CTM',A.VC_WORKORDER,CAST(A.RS AS DECIMAL(8,2)) AS 'RS',CAST(A.RSH AS DECIMAL(8,2)) AS 'RSH'";
            sql += ",A.CALIBRATION_NO,B.SI_LOT";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '单焊长焊带批次号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN1'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '单焊短焊带批次号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN2'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '串焊短焊带批次号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN3'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '助焊剂批次号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN4'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '汇流条批号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN5'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '玻璃供应商' THEN C.PARAM_VALUE ELSE '' END) AS 'PN6'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN 'EVA上供应商' THEN C.PARAM_VALUE ELSE '' END) AS 'PN7'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN 'EVA下供应商' THEN C.PARAM_VALUE ELSE '' END) AS 'PN8'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '背板供应商' THEN C.PARAM_VALUE ELSE '' END) AS 'PN9'";
            sql += ",MAX(CASE C.PARAM_NAME WHEN '接线盒型号' THEN C.PARAM_VALUE ELSE '' END) AS 'PN10'";
            sql += " FROM WIP_IV_TEST A,POR_LOT B,WIP_PARAM C,WIP_TRANSACTION D";
            sql += " WHERE A.LOT_NUM=B.LOT_NUMBER AND B.LOT_KEY=C.LOT_KEY AND C.TRANSACTION_KEY=D.TRANSACTION_KEY";
            sql += " AND B.DELETED_TERM_FLAG!='2' AND D.UNDO_FLAG='0'";
            if (!string.IsNullOrEmpty(sFactory))
            {
                sql += " AND B.FACTORYROOM_KEY='" + sFactory + "'";
            }
            if (!string.IsNullOrEmpty(sProID))
            {
                sql += " AND B.PRO_ID='" + sProID + "'";
            }
            if (!string.IsNullOrEmpty(sWO))
            {
                sql += " AND B.WORK_ORDER_NO='" + sWO + "'";
            }
            if (!string.IsNullOrEmpty(sDefault))
            {
                sql += " AND A.VC_DEFAULT='1'";
            }
            if (!string.IsNullOrEmpty(sStartSN))
            {
                sql += " AND A.LOT_NUM>='" + sStartSN + "'";
            }
            if (!string.IsNullOrEmpty(sEndSN))
            {
                sql += " AND A.LOT_NUM<='" + sEndSN + "'";
            }
            if (!string.IsNullOrEmpty(sStartDate))
            {
                //sql += " AND A.T_DATE>='" + sStartDate + "'";
                sql += " AND A.T_DATE>=CONVERT(DATETIME,'" + sStartDate + "')";
            }
            if (!string.IsNullOrEmpty(sEndDate))
            {
                //sql += " AND A.T_DATE<='" + sEndDate + "'";
                sql += " AND A.T_DATE<=CONVERT(DATETIME,'" + sEndDate + "')";
            }
            if (!string.IsNullOrEmpty(sDeviceNo))
            {
                sql += " AND A.DEVICENUM='" + sDeviceNo + "'";
            }
            sql += " GROUP BY A.T_DATE,A.TTIME,A.LOT_NUM,B.PRO_ID,A.AMBIENTTEMP,A.INTENSITY,A.FF,A.EFF,A.PM,A.ISC,A.IPM,A.VOC,A.VPM";
            sql += ",A.DEVICENUM,A.VC_PSIGN,A.DT_PRINTDT,A.P_NUM,A.VC_DEFAULT,A.SENSORTEMP,A.VC_CUSTCODE,A.C_USERID,A.COEF_PMAX,A.COEF_ISC,A.COEF_VOC";
            sql += ",A.COEF_IMAX,A.COEF_VMAX,A.COEF_FF,A.VC_CELLEFF,A.DEC_CTM,A.VC_WORKORDER,A.RS,A.RSH,A.CALIBRATION_NO,B.SI_LOT) M";
            sql += " LEFT JOIN ";
            sql += "(SELECT T.LOT_NUM,T.START_TIMESTAMP,T.EQUIPMENT_CODE";
            sql += " FROM (SELECT A.LOT_NUM,C.START_TIMESTAMP,D.EQUIPMENT_CODE,B.LOT_KEY";
            sql += " FROM WIP_IV_TEST A,POR_LOT B,EMS_LOT_EQUIPMENT C,EMS_EQUIPMENTS D";
            sql += " WHERE A.LOT_NUM=B.LOT_NUMBER AND B.LOT_KEY=C.LOT_KEY AND C.EQUIPMENT_KEY=D.EQUIPMENT_KEY";
            sql += " AND C.STEP_KEY IN (SELECT E.ROUTE_STEP_KEY  FROM POR_ROUTE_STEP E WHERE E.ROUTE_STEP_NAME='层压')";
            //sql += " AND C.START_TIMESTAMP IN (SELECT MAX(F.START_TIMESTAMP) FROM EMS_LOT_EQUIPMENT F WHERE F.LOT_KEY=B.LOT_KEY";
            //sql += " AND F.STEP_KEY IN (SELECT G.ROUTE_STEP_KEY  FROM POR_ROUTE_STEP G WHERE G.ROUTE_STEP_NAME='层压'))";
            sql += " AND B.DELETED_TERM_FLAG!='2'";
            if (!string.IsNullOrEmpty(sFactory))
            {
                sql += " AND B.FACTORYROOM_KEY='" + sFactory + "'";
            }
            if (!string.IsNullOrEmpty(sProID))
            {
                sql += " AND B.PRO_ID='" + sProID + "'";
            }
            if (!string.IsNullOrEmpty(sWO))
            {
                sql += " AND B.WORK_ORDER_NO='" + sWO + "'";
            }
            if (!string.IsNullOrEmpty(sStartSN))
            {
                sql += " AND A.LOT_NUM>='" + sStartSN + "'";
            }
            if (!string.IsNullOrEmpty(sEndSN))
            {
                sql += " AND A.LOT_NUM<='" + sEndSN + "'";
            }
            if (!string.IsNullOrEmpty(sStartDate))
            {
                //sql += " AND A.T_DATE>='" + sStartDate + "'";
                sql += " AND A.T_DATE>=CONVERT(DATETIME,'" + sStartDate + "')";
            }
            if (!string.IsNullOrEmpty(sEndDate))
            {
                //sql += " AND A.T_DATE<='" + sEndDate + "'";
                sql += " AND A.T_DATE<=CONVERT(DATETIME,'" + sEndDate + "')";
            }
            if (!string.IsNullOrEmpty(sDeviceNo))
            {
                sql += " AND A.DEVICENUM='" + sDeviceNo + "'";
            }
            if (!string.IsNullOrEmpty(sDefault))
            {
                sql += " AND A.VC_DEFAULT='1'";
            }
            sql += ") T,";
            sql += "(SELECT MAX(F.START_TIMESTAMP) START_TIMESTAMP,F.LOT_KEY FROM EMS_LOT_EQUIPMENT F";
            sql += " WHERE  F.STEP_KEY IN(SELECT G.ROUTE_STEP_KEY  FROM POR_ROUTE_STEP G WHERE G.ROUTE_STEP_NAME='层压')";
            sql += " GROUP BY F.LOT_KEY) T1";
            sql += " WHERE T.LOT_KEY=T1.LOT_KEY AND T.START_TIMESTAMP=T1.START_TIMESTAMP";
            sql += ") N ON M.LOT_NUM=N.LOT_NUM";   
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);  
            return dsReturn;
        }

        /// <summary>
        /// 通过查询条件获取对应的测试信息  Modified by yongbing.yang 20131023 修改语句到存储过程
        /// 
        /// </summary>
        /// <param name="sFactory">工厂主键key</param>
        /// <param name="sProID"产品ID></param>
        /// <param name="sWO">工单号码</param>
        /// <param name="partNumber">产品料号</param>
        /// <param name="sStartSN">其实序列号</param>
        /// <param name="sEndSN">结束序列号</param>
        /// <param name="sStartDate">开始时间</param>
        /// <param name="sEndDate">结束时间</param>
        /// <param name="sDeviceNo">设备编号</param>
        /// <param name="sDefault">是否为有效数据：1有效数据，0无效数据</param>
        /// <returns></returns>
        public  DataSet GetCTMListData(string sFactory, 
                                       string sProID, 
                                       string sWO, 
                                       string partNumber,
                                       string sStartSN, 
                                       string sEndSN, 
                                       string sStartDate, 
                                       string sEndDate,
                                       string sDeviceNo, 
                                       string sDefault)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_LIST";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "p_start_time", DbType.String, sStartDate);
                this._db.AddInParameter(cmd, "p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "p_start_lotNo", DbType.String, sStartSN);
                this._db.AddInParameter(cmd, "p_end_lotNo", DbType.String, sEndSN);
                this._db.AddInParameter(cmd, "p_devicenum", DbType.String, sDeviceNo);
                this._db.AddInParameter(cmd, "p_defeat", DbType.String, sDefault);
                this._db.AddInParameter(cmd, "p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }

        }
    }
}

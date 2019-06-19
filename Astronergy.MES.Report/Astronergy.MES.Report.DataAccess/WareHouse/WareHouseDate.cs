using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Web;

namespace Astronergy.MES.Report.DataAccess.WareHouse
{
    public sealed class WareHouseDate : BaseDBAccess
    {
        private static Database db = DatabaseFactory.CreateDatabase();

        public static DataSet GetGrade()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT T.COLUMN_CODE,T.COLUMN_NAME,T.COLUMN_NAME_DESC";
            sql += " FROM (SELECT T.ITEM_ORDER";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name'";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Index' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Index'";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_type' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_type'";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_code' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_code'";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'Column_Name_Desc' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'Column_Name_Desc'";
            sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
            sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
            sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_TestRule_PowerSet'";
            sql += " GROUP BY T.ITEM_ORDER) T";
            sql += " WHERE T.Column_type='ProductGrade' AND T.COLUMN_CODE!='Grade_LOWER'";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetWareHouseData(string sFactory, string sStartDate, string sEndDate)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            sql = "SELECT M.GRADE,M.WH_DTAE,COUNT(M.LOT_NUM) AS 'WH_QTY',SUM(M.COEF_PMAX) AS 'WH_POWER',SUM(M.PM) AS 'WH_PM'";
            sql += " FROM (";
            sql += "SELECT CASE WHEN A.TIME_STAMP>=CONVERT(DATETIME,CONVERT(CHAR(10),A.TIME_STAMP,120)+' 08:00:00') THEN CONVERT(CHAR(10),A.TIME_STAMP,120)";
            sql += " ELSE CONVERT(CHAR(10),DATEADD(DAY,-1,A.TIME_STAMP),120) END AS 'WH_DTAE'";
            sql += ",B.PRO_LEVEL AS 'GRADE',C.LOT_NUM,C.COEF_PMAX,C.PM";
            sql += " FROM WIP_TRANSACTION A,POR_LOT B,WIP_IV_TEST C";
            sql += " WHERE A.PIECE_KEY=B.LOT_KEY AND B.LOT_NUMBER=C.LOT_NUM";
            sql += " AND B.DELETED_TERM_FLAG!='2' AND C.VC_DEFAULT='1'";
            sql += " AND A.STEP_NAME='入库' AND A.ACTIVITY='TRACKOUT'";
            if (!string.IsNullOrEmpty(sFactory))
            {
                sql += " AND B.FACTORYROOM_KEY='" + sFactory + "'";
            }
            if (!string.IsNullOrEmpty(sStartDate))
            {
                sql += " AND A.TIME_STAMP>='" + sStartDate + "'";
            }
            if (!string.IsNullOrEmpty(sEndDate))
            {
                sql += " AND A.TIME_STAMP<='" + sEndDate + "'";
            }
            sql += ") M";
            sql += " GROUP BY M.GRADE,M.WH_DTAE";
            sql += " ORDER BY M.GRADE ASC";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFactoryWorkPlace()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT LOCATION_KEY,LOCATION_NAME";
            sql += " FROM FMM_LOCATION";
            sql += " WHERE LOCATION_LEVEL='5'";
            sql += " ORDER BY LOCATION_NAME";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetWOData(string sFactoryName,string sStartDate,string sEndDate)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "select WORK_ORDER_KEY,ORDER_NUMBER,FACTORY_NAME";
            sql += " from POR_WORK_ORDER";
            sql += " where 1=1";
            if (!string.IsNullOrEmpty(sFactoryName))
            {
                sql += " and FACTORY_NAME='" + sFactoryName + "'";
            }
            if (!string.IsNullOrEmpty(sStartDate))
            {
                sql += " and CREATE_TIME >='" + sStartDate + "'";
            }
            if (!string.IsNullOrEmpty(sEndDate))
            {
                sql += " and CREATE_TIME <='" + sEndDate + "'";
            }
            sql += " order by CREATE_TIME desc";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetPowerLever(string sFactory, string sStartDate, string sEndDate)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "select distinct PMAXSTAB";
            sql += " from BASE_POWERSET where ISFLAG='1'";
            sql += " order by PMAXSTAB asc";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetWOQuantity(string sWO)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "select ISNULL(SUM(QUANTITY_ORDERED),0) AS 'QUANTITY_ORDERED'";
            sql += " from POR_WORK_ORDER";
            sql += " where 1=1";
            if (!string.IsNullOrEmpty(sWO))
            {
                sql += " and ORDER_NUMBER in (" + sWO + ")";
            }
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public  DataSet GetWareHouseDataNew(string sFactory, string sStartDate, string sEndDate,
                                            string sWO, string sPowerLevel, string partNumber)
        {
            DataSet dsReturn = new DataSet();
            const string storeProcedureName = "SP_QRY_WAREHOUSE_DATA";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 360;
                this._db.AddInParameter(cmd, "p_sFactory", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "p_sStartDate", DbType.String, sStartDate);
                this._db.AddInParameter(cmd, "p_sEndDate", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "p_sWO", DbType.String, sWO);
                this._db.AddInParameter(cmd, "p_sPowerLevel", DbType.String, sPowerLevel);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                dsReturn = this._db.ExecuteDataSet(cmd);
            }
            return dsReturn;
        }

        public DataSet GetWareHouseDataNewDtl(string sFactory, string sStartDate, string sEndDate, 
                                              string sWO, string sPowerLevel, string sGrade, 
                                              string sDate, string sType,string partNumber)
        {
            DataSet dsReturn = new DataSet();
            const string storeProcedureName = "SP_QRY_WAREHOUSE_DATA_DTL";
            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                cmd.CommandTimeout = 360;
                this._db.AddInParameter(cmd, "p_sFactory", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "p_sStartDate", DbType.String, sStartDate);
                this._db.AddInParameter(cmd, "p_sEndDate", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "p_sWO", DbType.String, sWO);
                this._db.AddInParameter(cmd, "p_sPowerLevel", DbType.String, sPowerLevel);
                this._db.AddInParameter(cmd, "p_stype", DbType.String, sType);
                this._db.AddInParameter(cmd, "p_sdate", DbType.String, sDate);
                this._db.AddInParameter(cmd, "p_sgard", DbType.String, sGrade);
                this._db.AddInParameter(cmd, "p_partNumber", DbType.String, partNumber);
                dsReturn = this._db.ExecuteDataSet(cmd);
            }
            return dsReturn;
        }
    }
}

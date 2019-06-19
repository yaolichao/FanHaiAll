using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Astronergy.MES.Report.DataAccess
{
    public sealed class CTMReportFunction:BaseDBAccess
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
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public DataSet GetCellEffDate(string sFactory,
            string sModleCode, 
            string sPowerType, 
            string sWO, 
            string sProID, 
            string sDeviceNum, 
            string sStartDte, 
            string sEndDate, 
            string sGlass, 
            string sEva, 
            string sSILot, 
            string sSingle, 
            string sBusBar, 
            string sJunctionbox,
            string partNumber)
        {

            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetCellEffDate");
                this._db.AddInParameter(cmd, "@p_power_type", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public DataSet GetPowerLevelDate(string sFactory,
                                        string sModleCode, 
                                        string sPowerType, 
                                        string sWO, 
                                        string sProID, 
                                        string sDeviceNum, 
                                        string sStartDte, 
                                        string sEndDate, 
                                        string sGlass, 
                                        string sEva, 
                                        string sSILot, 
                                        string sSingle, 
                                        string sBusBar, 
                                        string sJunctionbox,
                                        string partNumber)
        {

            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            if (sPowerType == "A")
            {
                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                    this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                    this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetPowerLevelDate");
                    this._db.AddInParameter(cmd, "@p_power_type", DbType.String, "A");
                    this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                    this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                    this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                    this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                    this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                    this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                    this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                    this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                    this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                    this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                    this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                    this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                    return this._db.ExecuteDataSet(cmd);
                }
            }
            if (sPowerType == "B")
            {
                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                    this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                    this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetPowerLevelDate");
                    this._db.AddInParameter(cmd, "@p_power_type", DbType.String, "B");
                    this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                    this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                    this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                    this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                    this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                    this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                    this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                    this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                    this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                    this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                    this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                    this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                    return this._db.ExecuteDataSet(cmd);
                }
            }
            return new DataSet();
        }

        public  DataSet GetPowerDate( string sFactory,
                                      string sModleCode, 
                                      string sPowerType, 
                                      string sWO, 
                                      string sProID, 
                                      string sDeviceNum, 
                                      string sStartDte, 
                                      string sEndDate, 
                                      string sGlass, 
                                      string sEva, 
                                      string sSILot, 
                                      string sSingle, 
                                      string sBusBar, 
                                      string sJunctionbox,
                                      string partNumber)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            if (sPowerType == "A")
            {  
                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                    this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                    this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetPowerDate");
                    this._db.AddInParameter(cmd, "@p_power_type", DbType.String, "A");
                    this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                    this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                    this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                    this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                    this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                    this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                    this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                    this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                    this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                    this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                    this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                    this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                    return this._db.ExecuteDataSet(cmd);
                }                
            }
            if (sPowerType == "B")
            {
                using (DbConnection con = this._db.CreateConnection())
                {
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storeProcedureName;
                    this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                    this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                    this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetPowerDate");
                    this._db.AddInParameter(cmd, "@p_power_type", DbType.String, "B");
                    this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                    this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                    this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                    this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                    this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                    this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                    this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                    this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                    this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                    this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                    this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                    this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                    return this._db.ExecuteDataSet(cmd);
                } 
            }
            return new DataSet();
        }

        public DataSet GetCreateLot(string sFactory,
                                    string sModleCode, 
                                    string sPowerType, 
                                    string sWO, 
                                    string sProID, 
                                    string sDeviceNum, 
                                    string sStartDte, 
                                    string sEndDate,
                                    string partNumber)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetCreateLot");
                this._db.AddInParameter(cmd, "@p_power_type", DbType.String, sPowerType);
                this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_eva", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_glass", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                this._db.AddInParameter(cmd, "@p_single", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_busbar", DbType.String, "");
                this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public DataSet GetIVDate( string sFactory,
                                  string sModleCode, 
                                  string sPowerType, 
                                  string sWO, 
                                  string sProID, 
                                  string sDeviceNum, 
                                  string sStartDte, 
                                  string sEndDate, 
                                  string sGlass, 
                                  string sEva, 
                                  string sSILot, 
                                  string sSingle, 
                                  string sBusBar, 
                                  string sJunctionbox,
                                  string partNumber)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetIVDate");
                this._db.AddInParameter(cmd, "@p_power_type", DbType.String, sPowerType);
                this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public DataSet GetCTMData( string sFactory,
                                          string sModleCode, 
                                          string sPowerType, 
                                          string sWO, 
                                          string sProID, 
                                          string sDeviceNum, 
                                          string sStartDte, 
                                          string sEndDate, 
                                          string sGlass, 
                                          string sEva, 
                                          string sSILot, 
                                          string sSingle, 
                                          string sBusBar, 
                                          string sJunctionbox,
                                          string partNumber)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetCTMData");
                this._db.AddInParameter(cmd, "@p_power_type", DbType.String, sPowerType);
                this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }

        public static DataSet GetWO(string sFactory, string sStartDte, string sEndDate)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            sql = "SELECT DISTINCT WORK_ORDER_NO";
            //sql += " FROM CTM_LIST";
            sql += " FROM RPT_CTM_LIST";
            sql += " WHERE ISNULL(WORK_ORDER_NO,'')!=''";
            if (sFactory != "ALL")
            {
                sql += " AND FACTORYROOM_KEY='" + sFactory + "'";
            }
            if (sStartDte != "" && sEndDate != "")
            {
                sql += " AND TTIME BETWEEN CONVERT(DATETIME,'" + sStartDte + "') AND CONVERT(DATETIME,'" + sEndDate + "')";
            }
            sql += " ORDER BY WORK_ORDER_NO";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetPID(string sFactory, string sStartDte, string sEndDate)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            sql = "SELECT DISTINCT PRO_ID";
            //sql += " FROM CTM_LIST";
            sql += " FROM RPT_CTM_LIST";
            sql += " WHERE ISNULL(WORK_ORDER_NO,'')!=''";
            if (sFactory != "ALL")
            {
                sql += " AND FACTORYROOM_KEY='" + sFactory + "'";
            }
            if (sStartDte != "" && sEndDate != "")
            {
                sql += " AND TTIME BETWEEN CONVERT(DATETIME,'" + sStartDte + "') AND CONVERT(DATETIME,'" + sEndDate + "')";
            }
            sql += " ORDER BY PRO_ID";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetProMode(string sProModeName)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "select PROMODEL_KEY,PROMODEL_NAME";
            sql += " from BASE_PRODUCTMODEL where ISFLAG='1'";
            if (!string.IsNullOrEmpty(sProModeName))
            {
                sql += " and PROMODEL_NAME='" + sProModeName + "'";
            }
            sql += " order by PROMODEL_NAME asc";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public DataSet GetCTMDataDay( string sFactory,
                                      string sModleCode,
                                      string sPowerType,
                                      string sWO,
                                      string sProID,
                                      string sDeviceNum,
                                      string sStartDte,
                                      string sEndDate,
                                      string sGlass,
                                      string sEva,
                                      string sSILot,
                                      string sSingle,
                                      string sBusBar, 
                                      string sJunctionbox,
                                      string partNumber)
        {
            const string storeProcedureName = "SP_QRY_MODULES_CTM_RPT";

            using (DbConnection con = this._db.CreateConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storeProcedureName;
                this._db.AddInParameter(cmd, "@p_start_time", DbType.String, sStartDte);
                this._db.AddInParameter(cmd, "@p_end_time", DbType.String, sEndDate);
                this._db.AddInParameter(cmd, "@p_type", DbType.String, "GetCTMDataDay");
                this._db.AddInParameter(cmd, "@p_power_type", DbType.String, sPowerType);
                this._db.AddInParameter(cmd, "@p_roomkey", DbType.String, sFactory);
                this._db.AddInParameter(cmd, "@p_devicenum", DbType.String, sDeviceNum);
                this._db.AddInParameter(cmd, "@p_proId", DbType.String, sProID);
                this._db.AddInParameter(cmd, "@p_workorder", DbType.String, sWO);
                this._db.AddInParameter(cmd, "@p_si_supplier", DbType.String, sSILot);
                this._db.AddInParameter(cmd, "@p_junctionbox", DbType.String, sJunctionbox);
                this._db.AddInParameter(cmd, "@p_eva", DbType.String, sEva);
                this._db.AddInParameter(cmd, "@p_glass", DbType.String, sGlass);
                this._db.AddInParameter(cmd, "@p_promodule", DbType.String, sModleCode);
                this._db.AddInParameter(cmd, "@p_single", DbType.String, sSingle);
                this._db.AddInParameter(cmd, "@p_busbar", DbType.String, sBusBar);
                this._db.AddInParameter(cmd, "@p_partNumber", DbType.String, partNumber);
                return this._db.ExecuteDataSet(cmd);
            }
        }
    }
}

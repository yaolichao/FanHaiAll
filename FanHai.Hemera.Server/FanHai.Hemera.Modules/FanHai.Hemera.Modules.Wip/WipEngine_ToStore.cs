using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Share.Constants;
using System.Data.Common;
using System.Collections;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Modules.WipJob;
using SolarViewer.Hemera.Modules.EDC;
using SolarViewer.Hemera.Utils.DatabaseHelper;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {

        public DataSet TransferLotToStore(DataSet dataset)
        {
            //dataset.WriteXml(@"d:\TransferLotToStore.xml");

            System.DateTime startTime = System.DateTime.Now;

            DataSet dsReturn = new DataSet();
            DbConnection dbconn = null;
            DbTransaction dbtran = null;
            string sql = "", storeKey = "", storeName = "", lotNumber = "";
            string strEditor = "", strEditTimeZone = "", strEditTime = "";
            string lotKey = "", stepKey = "";
            if (dataset != null)
            {
                if (dataset.Tables.Contains(WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME))
                {
                    WST_STORE_MAT_FIELDS tableFields = new WST_STORE_MAT_FIELDS();
                    DataTable dataTable = dataset.Tables[WST_STORE_MAT_FIELDS.DATABASE_TABLE_NAME];
                    storeName = dataTable.Rows[0][WST_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                    lotNumber = dataTable.Rows[0][WST_STORE_MAT_FIELDS.FIELD_ITEM_NO].ToString();
                    strEditor = dataTable.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDITOR].ToString();
                    strEditTimeZone = dataTable.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                    strEditTime = dataTable.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDIT_TIME].ToString();
                    lotKey = dataTable.Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                    stepKey = dataTable.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                    try
                    {
                        #region CheckRecordExpired
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNumber);
                        List<KeyValuePair<string, string>> listCondition = new List<KeyValuePair<string, string>>();
                        listCondition.Add(kvp);
                        if (UtilHelper.CheckRecordExpired(db, POR_LOT_FIELDS.DATABASE_TABLE_NAME, listCondition, strEditTime))
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP);
                            return dsReturn;
                        }
                        #endregion

                        sql = @"SELECT STORE_KEY FROM WST_STORE WHERE STORE_NAME='" + storeName + "'";
                        IDataReader dataReader = db.ExecuteReader(CommandType.Text, sql);
                        if (dataReader.Read())
                        {
                            storeKey = dataReader["STORE_KEY"].ToString();
                            using (dbconn = db.CreateConnection())
                            {
                                dbconn.Open();
                                //Create Transaction  
                                dbtran = dbconn.BeginTransaction();
                                try
                                {
                                    #region insert record into store_mat
                                    dataTable.Rows[0][WST_STORE_MAT_FIELDS.FIELD_EDIT_TIME] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    sql = DatabaseTable.BuildInsertSqlStatement(tableFields, dataTable, 0,
                                                                new Dictionary<string, string>() 
                                                        {                                                             
                                                            {WST_STORE_MAT_FIELDS.FIELD_STORE_KEY,storeKey},
                                                            {WST_STORE_MAT_FIELDS.FIELD_ITEM_TYPE,"Lot"},
                                                            {WST_STORE_MAT_FIELDS.FIELD_OBJECT_STATUS,"1"},                                                            
                                                        },
                                                                new List<string>()
                                                        {
                                                           WST_STORE_FIELDS.FIELD_STORE_NAME,
                                                           POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY,
                                                           POR_LOT_FIELDS.FIELD_LOT_KEY
                                                        });
                                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                                    #endregion

                                    #region 更新批次信息
                                    sql = @"UPDATE POR_LOT SET STATE_FLAG=11," +
                                            "EDITOR='" + strEditor + "'," +
                                            "EDIT_TIME=SYSDATE," +
                                            "EDIT_TIMEZONE='" + strEditTimeZone + "' " +
                                            "WHERE LOT_NUMBER='" + lotNumber + "'";
                                    db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                                    #endregion

                                    #region update ems_lot_equipment
                                    DataSet resDs = new DataSet();
                                    string sqlString = @"SELECT B.EQUIPMENT_KEY,A.EQUIPMENT_NAME,A.EQUIPMENT_STATE_KEY
                                     FROM EMS_EQUIPMENTS A,EMS_LOT_EQUIPMENT B
                                     WHERE A.EQUIPMENT_KEY = B.EQUIPMENT_KEY 
                                     AND B.STEP_KEY = '" + stepKey + "' " +
                                        "AND B.LOT_KEY='" + lotKey + "' " +
                                        "AND B.END_TIMESTAMP IS NULL";
                                    db.LoadDataSet(CommandType.Text, sqlString, resDs, new string[] { EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME });
                                    if (resDs != null && resDs.Tables.Count > 0)
                                    {
                                        if (resDs.Tables[0].Rows.Count > 0)
                                        {
                                            string equipmentKey = resDs.Tables[0].Rows[0]["EQUIPMENT_KEY"].ToString();
                                            WipManagement.TrackOutForEquipment(lotKey, stepKey, equipmentKey, strEditor, dbtran);
                                        }
                                    }
                                    #endregion

                                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                                    dbtran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                                    //Rollback Transaction
                                    dbtran.Rollback();
                                    LogService.LogError("TransferLotToStore Error: " + ex.Message);
                                }
                                finally
                                {
                                    dbconn.Close();
                                }
                            }
                        }
                        else
                        {
                            SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:SolarViewer.Hemera.Modules.FMM.StoreEngine.StoreIsNotExist}");
                            return dsReturn;
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    }
                }
            }

            System.DateTime endTime = System.DateTime.Now;
            LogService.LogInfo("TransferLotToStore Time: " + (endTime - startTime).TotalMilliseconds.ToString());

            return dsReturn;
        }
    }
}

using System.Text;
using System.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Modules.Wip
{
    public class LotNumPrintEngine : AbstractEngine, ILotNumPrintEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotNumPrintEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public override void Initialize()
        {

        }
        #region ILotNumPrintEngine 成员

        public DataSet GetNotPrintLotNumInf(string facKey, string equipmentKey, string lineKey)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsNotPrint = new DataSet();
            DataSet dsPrint = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT LOT_NUMBER,WORK_ORDER_NO,CREATE_TIME,CREATOR FROM POR_LOT
                                                   WHERE 
                                                        FACTORYROOM_KEY = '{0}'
                                                        AND EQUIPMENT_KEY = '{1}'
                                                        AND LINE_KEY = '{2}'
                                                        AND PRINT_STATUS = 0 
                                                        AND LOT_TYPE = 'N'
                                                        AND STATUS != 2
                                                        ORDER BY CREATE_TIME,LOT_NUMBER ASC", 
                                                        facKey, equipmentKey, lineKey);

                dsNotPrint = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                dsNotPrint.Tables[0].TableName = "NOT_PRINT";

                sqlCommon = string.Format(@" SELECT LOT_NUMBER,WORK_ORDER_NO,B.CREATE_TIME,B.CREATOR,B.PRINTER,B.PRINT_TIME FROM POR_LOT B INNER JOIN  dbo.POR_ROUTE_STEP A
                                                         ON A.ROUTE_ROUTE_VER_KEY = B.CUR_ROUTE_VER_KEY
                                                         AND A.ROUTE_STEP_KEY = B.CUR_STEP_VER_KEY
                                                         WHERE ROUTE_STEP_NAME = '单串焊'
                                                         AND FACTORYROOM_KEY = '{0}'
                                                         AND EQUIPMENT_KEY = '{1}'
                                                         AND LINE_KEY = '{2}'
                                                         AND PRINT_STATUS = 1 
                                                         AND LOT_TYPE = 'N'
                                                         AND STATUS != 2
                                                         ORDER BY PRINT_TIME DESC", facKey, equipmentKey, lineKey);

                dsPrint = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                dsPrint.Tables[0].TableName = "PRINT";

                dsReturn.Merge(dsNotPrint.Tables["NOT_PRINT"]);
                dsReturn.Merge(dsPrint.Tables["PRINT"]);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetNotPrintLotNumInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion


        #region ILotNumPrintEngine 成员

        /// <summary>
        /// 获取打印的信息
        /// </summary>
        /// <param name="facKey"></param>
        /// <param name="equipmentKey"></param>
        /// <param name="lineKey"></param>
        /// <returns></returns>
        public DataSet GetPrintInf(string lotNumber,string dateStart,string dateEnd,string printerNo )
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sqlCommon = new StringBuilder();
            try
            {
                sqlCommon.Append("SELECT LOT_NUMBER,WORK_ORDER,CONVERT(VARCHAR(20),PRINT_TIME,120) PRINT_TIME,PRINTER,MAC,IS_REPRINT FROM POR_LOT_PRINT ");
                if (lotNumber == null && dateStart == null && dateEnd == null && printerNo==null)
                {

                    sqlCommon .Append(" WHERE 1>2 ");
                }
                else
                {
                    sqlCommon.Append(" WHERE 1=1");
                    if (!string.IsNullOrEmpty(lotNumber))
                    {
                        sqlCommon.AppendFormat(" AND LOT_NUMBER='{0}'", lotNumber);
                    }

                    if (!string.IsNullOrEmpty(dateStart))
                    {
                        sqlCommon.AppendFormat(" AND PRINT_TIME>='{0}'", DateTime.Parse(dateStart).ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(dateEnd))
                    {
                        sqlCommon.AppendFormat(" AND PRINT_TIME<'{0}'", DateTime.Parse(dateEnd).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(printerNo))
                    {
                        sqlCommon.AppendFormat(" AND PRINT='{0}'", printerNo);
                    }
                   
                   
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon.ToString());
               

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrintInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
        #region ILotNumPrintEngine 成员


        public DataSet CheckAndUpdateLotInf(string lotNumber,string printer,string facKey,string equipmentKey,string lineKey)
        {
            DataSet dsReturn = new DataSet();
            string message = string.Empty;
            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    //组织查询SQL，并执行查询。
                    string strSql = string.Format(@"SELECT COUNT(*) FROM POR_LOT 
                                                        WHERE LOT_NUMBER = '{0}' AND PRINT_STATUS = 0
                                                            AND FACTORYROOM_KEY = '{1}'
                                                            AND EQUIPMENT_KEY = '{2}'
                                                            AND LINE_KEY = '{3}'", lotNumber,facKey,equipmentKey,lineKey);
                    int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                    //如果有查找到对应的记录，返回true。表示该工单号对应的工单存在。
                    if (count > 0)
                    {
                        //不更新，另写方法
//                        string sqlCommon = string.Format(@"UPDATE POR_LOT SET PRINT_STATUS = 1,PRINTER = '{1}',PRINT_TIME = GETDATE()
//                                                                WHERE LOT_NUMBER = '{0}'", lotNumber,printer);
//                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommon);
                        
                    }
                    else //否则，返回false，表示该工单号对应的工单不存在。
                    {
                        message = "批次不存在,已经被打印或者已经被转至其他单串焊机台";
                    }
                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, message);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("CheckAndUpdateLotInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
        #region ILotNumPrintEngine 成员
        public bool UpdateLotInf(string lotNumber, string printer)
        {
            //bool flag;
            string message = string.Empty;
            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    //DbTransaction dbTran = dbConn.BeginTransaction();
                    
                       
                    string sqlCommon = string.Format(@"UPDATE POR_LOT SET PRINT_STATUS = 1,PRINTER = '{1}',PRINT_TIME = GETDATE()
                                                            WHERE LOT_NUMBER = '{0}'", lotNumber,printer);


                    if (db.ExecuteNonQuery( CommandType.Text, sqlCommon) < 1)
                    {
                        message = "批次不存在";
                        LogService.LogError("UpdateLotInf Error: " + message);
                        return false;
                    }
                    else
                    {
                        //Commit Transaction
                        //dbTran.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("UpdateLotInf Error: " + ex.Message);
                return false;
            }
            
        }
        #endregion

        #region ILotNumPrintEngine 成员
        public bool save_Print(string lotNumber, string printer,string mac,char is_RePrint)
        {
            //bool flag;
            string message = string.Empty;
            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    //DbTransaction dbTran = dbConn.BeginTransaction();


                    string sqlGetLot_Key = string.Format("SELECT LOT_KEY,WORK_ORDER_NO FROM POR_LOT WHERE LOT_NUMBER='{0}'", lotNumber);
                    DataSet dsLotMessage = db.ExecuteDataSet(CommandType.Text, sqlGetLot_Key);

                    string lot_key = dsLotMessage.Tables[0].Rows[0]["LOT_KEY"].ToString();
                    string work_order = dsLotMessage.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();



                    string sqlPrintMessage = string.Format(@"INSERT INTO POR_LOT_PRINT(LOT_KEY,LOT_NUMBER,WORK_ORDER,PRINTER,MAC,IS_REPRINT) 
                                                                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}') ",
                                                                                        lot_key,
                                                                                        lotNumber,
                                                                                        work_order,
                                                                                        printer,
                                                                                        mac,
                                                                                        is_RePrint
                                                                                        );
                    db.ExecuteNonQuery(CommandType.Text, sqlPrintMessage);
                    //Commit Transaction
                    //dbTran.Commit();
                    return true;

                }
            }
            catch (Exception ex)
            {
                LogService.LogError("save_Print Error: " + ex.Message);
                return false;
            }

        }
        #endregion


        #region ILotNumPrintEngine 成员


        public DataSet GetIdByOrderNumber(string orderNum)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT PRINT_CODE FROM dbo.POR_WO_PRD_PRINTRULE
                                                            WHERE WORK_ORDER_NUMBER = '{0}'", orderNum);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetIdByOrderNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotNumPrintEngine 成员


        public DataSet GetPrintIdByLotNumber(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            string message = string.Empty;
            try
            {
                
                string strSql = string.Format(@"SELECT COUNT(*) FROM POR_LOT 
                                                    WHERE LOT_NUMBER = '{0}' AND PRINT_STATUS = 1", lotNumber);
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                strSql = string.Format(@"SELECT COUNT(*) FROM POR_LOT 
                                                    WHERE LOT_NUMBER = '{0}'", lotNumber);
                int count01 = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count01 <= 0)
                {
                    message = "批次不存在！";
                }
                if (count01 > 0 && count <= 0)
                {
                    message = "批次存在但是尚未被打印，无法补打,请到对应的单串焊机台打印！";
                }
                if (count > 0 && count01 > 0 )
                {

                    string sqlCommon = string.Format(@"SELECT PRINT_CODE FROM dbo.POR_WO_PRD_PRINTRULE A
                                                        INNER JOIN POR_LOT B
                                                       ON A.WORK_ORDER_NUMBER = B.WORK_ORDER_NO
                                                       WHERE B.LOT_NUMBER = '{0}'	", lotNumber);

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, message);
                
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrintIdByLotNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotNumPrintEngine 成员

        /// <summary>
        /// //根据批次号获取工序设备线别信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        public DataSet GetEquipmentByLotNumber(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            string message = string.Empty;
            try
            {
            
                string strSql = string.Format(@"SELECT COUNT(*) FROM POR_LOT 
                                                    WHERE LOT_NUMBER = '{0}' AND PRINT_STATUS = 0", lotNumber);
                int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                strSql = string.Format(@"SELECT COUNT(*) FROM POR_LOT 
                                                    WHERE LOT_NUMBER = '{0}'", lotNumber);
                int count01 = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
                if (count01 <= 0)
                {
                    message = "批次不存在！";
                }
                if (count01 > 0 && count <= 0)
                {
                    message = "批次存在但是已经被打印，请撤销重新创建到需要创建的设备上！";
                }
                if (count > 0 && count01 > 0)
                {

                    string sqlCommon = string.Format(@"SELECT OPERATION_NAME,B.EQUIPMENT_CODE,C.LINE_NAME FROM POR_LOT A
                                                                INNER JOIN dbo.EMS_EQUIPMENTS B ON A.EQUIPMENT_KEY = B.EQUIPMENT_KEY
                                                                INNER JOIN dbo.FMM_PRODUCTION_LINE C ON A.LINE_KEY = C.PRODUCTION_LINE_KEY
                                                                WHERE LOT_NUMBER = '{0}'", lotNumber);

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                }
                
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, message);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipmentByLotNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region ILotNumPrintEngine 成员

        /// <summary>
        /// 更新数据信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <param name="operations">工序名称</param>
        /// <param name="equipmentkey">设备主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns></returns>
        public DataSet UpdatePorLot(string lotNumber, string operations, string equipmentkey, string lineKey)
        {
            DataSet dsReturn = new DataSet();
            string message = string.Empty;
            try
            {
                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    string sqlCommon = string.Format(@"UPDATE POR_LOT SET OPERATION_NAME = '{0}',EQUIPMENT_KEY = '{1}',LINE_KEY='{2}'
                                                                WHERE LOT_NUMBER = '{3}'",operations,equipmentkey,lineKey,lotNumber);


                    db.ExecuteNonQuery(CommandType.Text, sqlCommon);
                    //Commit Transaction
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, message);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdatePorLot Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}

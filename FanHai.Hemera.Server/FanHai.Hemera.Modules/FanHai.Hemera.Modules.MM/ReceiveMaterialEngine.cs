using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Interface;
using System.Data.Common;

using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Data.OracleClient;
using FanHai.Hemera.Modules.FMM;

namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 来料接收数据的操作类。
    /// </summary>
    public class ReceiveMaterialEngine : AbstractEngine, IReceiveMaterialEngine
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReceiveMaterialEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 获取物料明细信息。
        /// </summary>
        /// <param name="sapOperation">SAP工作中心名称，多个工作中心使用逗号分割。C1,C2...</param>
        /// <param name="storeNameList">线上仓名称，多个线上仓使用逗号分割。store1,store2..</param>
        /// <returns>
        /// 包含物料明细信息的数据集对象。
        /// 【W.SAP_ISSURE_KEY,ROWNUM,W.MBLNR, W.MATNR,W.CHARG, W.MATXT, W.AUFNR,W.ERFME,W.ERFMG, W.LLIEF,B.WORK_CENTER】
        /// </returns>
        public DataSet GetMaterialDetail(string sapOperation, string storeNameList)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sSapOperation = UtilHelper.BuilderWhereConditionString("B.WORK_CENTER", sapOperation.Split(','));
                string sStoreNameList = UtilHelper.BuilderWhereConditionString(" D.STORE_NAME", storeNameList.Split(','));

                string sql = string.Format(@"SELECT TOP 10 ROW_NUMBER() OVER(ORDER BY W.CHARG) ROWNUM,W.SAP_ISSURE_KEY,W.MBLNR,W.MATNR,W.CHARG,W.MATXT,W.AUFNR,W.ERFME,W.ERFMG,W.LLIEF,B.WORK_CENTER
                                            FROM WST_SAP_ISSURE W
                                            JOIN POR_WORK_ORDER A ON A.ORDER_NUMBER=W.AUFNR
                                            JOIN POR_WORK_ORDER_BOM B ON W.MATNR = B.MATERIAL_CODE AND W.AUFNR = B.ORDER_NUMBER
                                            WHERE W.ISRECEIVED = '0'
                                            AND A.FACTORY_NAME IN (SELECT DISTINCT E.FACTORY_NAME
                                                                   FROM WST_STORE D
                                                                   LEFT JOIN V_LOCATION E ON D.LOCATION_KEY = E.ROOM_KEY
                                                                   WHERE 1=1 {1})", 
                                            sSapOperation, 
                                            sStoreNameList);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetMaterialDetail Error:{0}",ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
       
        /// <summary>
        /// 通过线上仓名称获取工厂名称。
        /// </summary>
        /// <param name="store">线上仓名称。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        public DataSet GetFactoryByStore(string store)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand =string.Format(@"SELECT DISTINCT b.PARENT_NAME
                                                FROM WST_STORE d
                                                LEFT JOIN V_LOCATION_RET a ON d.LOCATION_KEY=a.LOCATION_KEY
                                                LEFT JOIN V_LOCATION_RET b ON a.PARENT_KEY=b.LOCATION_KEY
                                                WHERE a.LOCATION_LEVEL=5 AND D.STORE_NAME='{0}'",
                                                store.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetFactoryByStore Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过工单号获取工单下发的工厂名称。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        public DataSet GetFactoryByOrderNumber(string orderNumber)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format("SELECT A.FACTORY_NAME FROM POR_WORK_ORDER A WHERE A.ORDER_NUMBER='{0}'", 
                                                   orderNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetFactoryByOrderNumber Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据线上仓名称获取工序名称。
        /// </summary>
        /// <param name="lineStore">线上仓名称。</param>
        /// <returns>包含工序名称的数据集。[OPERATION_NAME,STORE_NAME]</returns>
        public DataSet GetOperationByLineStore(string lineStore)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format("SELECT OPERATION_NAME,STORE_NAME FROM WST_STORE WHERE STORE_NAME='{0}'", lineStore.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetOperationByLineStore Error:{0}", ex.Message));
            }
            return dsReturn;
        }
       
        /// <summary>
        /// 通过工序和车间主键获取线上仓。
        /// </summary>
        /// <param name="operation">工序名称,</param>
        /// <param name="roomKey">车间主键,</param>
        /// <param name="stores">拥有权限的线上仓名称，使用逗号分隔store1,store2...。</param>
        /// <returns>包含线上仓的数据集对象。</returns>
        public DataSet GetStores(string operation, string roomKey, string stores)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string condition = UtilHelper.BuilderWhereConditionString("STORE_NAME", stores.Split(','));
                string sql = string.Format(@"SELECT STORE_KEY,STORE_NAME 
                                            FROM WST_STORE 
                                            WHERE OPERATION_NAME='{0}' 
                                            AND OBJECT_STATUS=1 
                                            AND LOCATION_KEY='{1}'
                                            {2}", 
                                            operation.PreventSQLInjection(), 
                                            roomKey.PreventSQLInjection(), 
                                            condition);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetStores Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过工序获取线上仓。
        /// </summary>
        /// <param name="operation">工序名称,</param>
        /// <param name="stores">拥有权限的线上仓名称。使用逗号分隔,store1,store2...。</param>
        /// <returns>包含线上仓的数据集对象。</returns>
        public DataSet GetStoreByOperation(string operation, string stores)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string condition = UtilHelper.BuilderWhereConditionString("STORE_NAME", stores.Split(','));
                string sql = string.Format("SELECT  STORE_NAME FROM WST_STORE WHERE OPERATION_NAME='{0}' {1}", 
                                            operation.PreventSQLInjection(), condition);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetStoreByOperation Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过工单号和物料料号获取线上仓。
        /// </summary>
        /// <param name="workOrder">工单号,</param>
        /// <param name="materialCode">物料编码,</param>
        /// <param name="stores">拥有权限的线上仓名称,使用逗号分隔,store1,store2...。</param>
        ///  <returns>包含线上仓的数据集对象。</returns>
        public DataSet GetStoreByMaterialCode(string workOrder, string materialCode, string stores)
        {
            const string CONST_GET_SAP_OPERATIONS_SQL = "SELECT DISTINCT WORK_CENTER FROM POR_WORK_ORDER_BOM WHERE  ORDER_NUMBER='{0}' AND MATERIAL_CODE='{1}'";
            DataSet dsReturn = new DataSet();
            try
            {
                //根据工单号和物料编码获取工作中心（SAP工序）
                DataSet dsSAPWorkCenter = db.ExecuteDataSet(CommandType.Text, string.Format(CONST_GET_SAP_OPERATIONS_SQL,
                                                                                            workOrder.PreventSQLInjection(), 
                                                                                            materialCode.PreventSQLInjection()));
                string strSAPWorkCenter = string.Empty;
                foreach (DataRow dr in dsSAPWorkCenter.Tables[0].Rows)
                {
                    strSAPWorkCenter += "'" + dr["WORK_CENTER"].ToString().PreventSQLInjection() + "',";
                }
                strSAPWorkCenter = strSAPWorkCenter.TrimEnd(',');

                //通过WORK_CENTER获取MES工序
                DataSet dsSAPMES = CrmAttributeEngine.GetDistinctColumnsData(db, "WORK_CENTER,OPERATION_NAME", "SAP_MES_OPERATIONS");
                DataTable dt = dsSAPMES.Tables[0];
                DataRow[] drs = dt.Select("WORK_CENTER in (" + strSAPWorkCenter + ")");
                string strOpearions = string.Empty;
                foreach (DataRow dr in drs)
                {
                    strOpearions += "'" + dr["OPERATION_NAME"].ToString().PreventSQLInjection() + "',";
                }
                strOpearions = strOpearions.TrimEnd(',');
                //通过MES工序和拥有权限的线上仓名称获取线上仓名称。
                string sql = string.Format("SELECT  STORE_NAME FROM WST_STORE WHERE OPERATION_NAME IN ({0}) AND OBJECT_STATUS=1", strOpearions);
                sql += UtilHelper.BuilderWhereConditionString("STORE_NAME", stores.Split(','));
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError(string.Format("GetStoreByMaterialCode Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取电池片物料信息。
        /// </summary>
        /// <returns>
        /// 包含电池片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        public DataSet GetMaterials()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT 
                              FROM POR_MATERIAL
                              WHERE MATERIAL_CODE > '200'
                              ORDER BY MATERIAL_CODE ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterials Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取硅片物料信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>
        /// 包含硅片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        public DataSet GetMaterials(string orderNumber)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT a.MATERIAL_KEY,a.MATERIAL_NAME,a.MATERIAL_CODE,a.UNIT 
                                            FROM POR_MATERIAL a
                                            WHERE EXISTS (SELECT b.MATERIAL_CODE 
                                                        FROM POR_WORK_ORDER_BOM b
                                                        WHERE b.ORDER_NUMBER='{0}'
                                                        AND b.MATERIAL_CODE=a.MATERIAL_CODE
                                                        AND b.REQ_QTY>0)
                                            AND MATERIAL_CODE > '200'
                                            ORDER BY MATERIAL_CODE ",
                                            orderNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterials Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取供应商信息。
        /// </summary>
        /// <returns>
        /// 包含供应商信息的数据集。
        /// 【CODE,NAME,NICKNAME】
        /// </returns>
        public DataSet GetSuppliers()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT CODE,NAME,NICKNAME FROM BASE_SUPPLIER ORDER BY CODE";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetSuppliers Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format("SELECT DISTINCT PRODUCT_KEY,PRODUCT_CODE,PRODUCT_NAME FROM POR_PRODUCT ORDER BY PRODUCT_CODE");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetProdId Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。</returns>
        public DataSet GetEfficiency()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT DISTINCT EFFICIENCY_NAME,LEFFICIENCY,UEFFICIENCY FROM BASE_EFFICIENCY
                                                    WHERE USED=1
                                                    ORDER BY LEFFICIENCY DESC,UEFFICIENCY DESC");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetEfficiency Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取领料项目（批次）号对应的信息。
        /// </summary>
        /// <returns>包含领料项目（批次）号信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialLotInfo(string val)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT TOP 1 a.CHARG,a.AUFNR,a.MATNR,a.MATXT,a.LLIEF,a.LICHA,a.PRO_ID,a.EFFICIENCY,a.GRADE,a.SUPPLIER_CODE,b.MATERIAL_KEY
                                                    FROM dbo.WST_SAP_ISSURE a
                                                    LEFT JOIN POR_MATERIAL b ON a.MATNR=b.MATERIAL_CODE
                                                    WHERE CHARG='{0}'",val.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetEfficiency Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 手工输入并保存接收的物料信息。
        /// </summary>
        /// <param name="htParams">包含输入数据的哈希表对象。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet ManualSaveReceiveMaterial(Hashtable htParams)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                string factoryRoomKey = Convert.ToString(htParams["FACTORYROOM_KEY"]);
                string operationName = Convert.ToString(htParams["OPERATION_NAME"]);
                string storeKey = Convert.ToString(htParams["STORE_KEY"]);


                string materialLot = Convert.ToString(htParams["MATERIAL_LOT"]);
                string orderNumber = Convert.ToString(htParams["ORDER_NUMBER"]);
                string proId = Convert.ToString(htParams["PRO_ID"]);
                string efficiency = Convert.ToString(htParams["EFFICIENCY"]);
                string grade = Convert.ToString(htParams["GRADE"]);
                string supplierName = Convert.ToString(htParams["SUPPLIER_NAME"]);
                string supplierCode = Convert.ToString(htParams["SUPPLIER_CODE"]);
                string materialKey = Convert.ToString(htParams["MATERIAL_KEY"]);
                string materialCode = Convert.ToString(htParams["MATERIAL_CODE"]);
                string materialDescription = Convert.ToString(htParams["MATERIAL_DESCRIPTION"]);
                string issueQty = Convert.ToString(htParams["ISSUE_QTY"]);
                string memo = Convert.ToString(htParams["MEMO"]);
                string issueNo = Convert.ToString(htParams["ISSUE_NO"]);
                string supplierLot = Convert.ToString(htParams["SUPPLIER_LOT"]);
                string issueStore = Convert.ToString(htParams["ISSUE_STORE"]);
                string unit = Convert.ToString(htParams["UNIT"]);
                string userName = Convert.ToString(htParams["USER_NAME"]);
                string timeZone = Convert.ToString(htParams["TIME_ZONE"]);
                string shiftName = Convert.ToString(htParams["SHIFT_NAME"]);
                string oem = Convert.ToString(htParams["OEM"]);
                bool IsUpdateOldReceiveMaterialInfo = Convert.ToBoolean(htParams["IsUpdateOldReceiveMaterialInfo"]);
                //--根据线上仓主键和物料主键查询记录
                string sql = string.Format(@"SELECT TOP 1 STORE_MATERIAL_KEY FROM WST_STORE_MATERIAL WHERE STORE_KEY='{0}' AND MATERIAL_KEY='{1}'",
                                            storeKey.PreventSQLInjection(), materialKey.PreventSQLInjection());
                string storeMaterialKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                //--不存在记录数
                if (string.IsNullOrEmpty(storeMaterialKey))
                {
                    storeMaterialKey = UtilHelper.GenerateNewKey(0);
                    sql = string.Format(@"INSERT INTO WST_STORE_MATERIAL(STORE_MATERIAL_KEY,STORE_KEY,MATERIAL_KEY)
                                          VALUES('{0}','{1}','{2}')", 
                                        storeMaterialKey.PreventSQLInjection(), 
                                        storeKey.PreventSQLInjection(), 
                                        materialKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                //--根据物料批号和线上仓物料主键查询记录
                sql = string.Format(@"SELECT TOP 1 STORE_MATERIAL_DETAIL_KEY FROM WST_STORE_MATERIAL_DETAIL 
                                      WHERE MATERIAL_LOT='{0}' AND STORE_MATERIAL_KEY='{1}'",
                                     materialLot.PreventSQLInjection(), storeMaterialKey.PreventSQLInjection());
                string storeMaterialDetailKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                //--不存在明细主键
                if (string.IsNullOrEmpty(storeMaterialDetailKey))
                {
                    storeMaterialDetailKey = UtilHelper.GenerateNewKey(0);
                    sql = string.Format(@"INSERT INTO WST_STORE_MATERIAL_DETAIL(STORE_MATERIAL_DETAIL_KEY,STORE_MATERIAL_KEY,MATERIAL_LOT,RECEIVE_QTY,CURRENT_QTY,MATERIAL_SUPPLIER)
                                      VALUES('{0}','{1}','{2}','{3}','{3}','{4}')",
                                    storeMaterialDetailKey.PreventSQLInjection(),
                                    storeMaterialKey.PreventSQLInjection(),
                                    materialLot.PreventSQLInjection(),
                                    issueQty.PreventSQLInjection(), 
                                    supplierName.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                else
                {
                    //--存在明细主键
                    sql = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL SET RECEIVE_QTY=RECEIVE_QTY+{0},CURRENT_QTY=CURRENT_QTY+{0},MATERIAL_SUPPLIER='{1}'
                                        WHERE STORE_MATERIAL_DETAIL_KEY='{2}'",
                                        issueQty.PreventSQLInjection(), 
                                        supplierName.PreventSQLInjection(), 
                                        storeMaterialDetailKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }
                //插入来料接收记录。
                string issue_key = UtilHelper.GenerateNewKey(0);
                //根据配料单号查询最大的ITMNO
                sql = string.Format(@"SELECT MAX(ITMNO) FROM WST_SAP_ISSURE WHERE MBLNR='{0}'", issueNo.PreventSQLInjection());
                string sItmNo = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                int itemNo = 0;
                if (sItmNo == null || !int.TryParse(sItmNo, out itemNo))
                {
                    itemNo = 1;
                }
                const string SQL = @"INSERT INTO WST_SAP_ISSURE
                                     (  
                                        SAP_ISSURE_KEY,MBLNR,WERKS,ITMNO,BWART,CDATE,ODATE,IDATE,AUFNR,MATNR,MATXT,CHARG,BWTAR,LGORT,BDMNG,ENMNG,
                                        ERFMG,ERFME,LLIEF,LICHA,ISRECEIVED,SHIFT_NAME,RECEIVE_TIME,OPERATOR,CREATOR,CREATE_TIME,CREATE_TIMEZONE,
                                        EDITOR,EDIT_TIME,EDIT_TIMEZONE,STORE_MATERIAL_DETAIL_KEY,OEM,PRO_ID,EFFICIENCY,GRADE,SUPPLIER_CODE,MEMO
                                     )
                                     VALUES
                                    (
                                        '{0}','{1}','SH01','{2}','000',
                                        CONVERT(VARCHAR,GETDATE(),112)+REPLACE(CONVERT(VARCHAR,GETDATE(),108),':',''),
                                        CONVERT(VARCHAR,GETDATE(),112)+REPLACE(CONVERT(VARCHAR,GETDATE(),108),':',''),
                                        CONVERT(VARCHAR,GETDATE(),112)+REPLACE(CONVERT(VARCHAR,GETDATE(),108),':',''),
                                        '{3}', '{4}','{5}','{6}',' ','{7}','{8}','{8}','{8}','{9}','{10}','{11}',1,'{12}',
                                        GETDATE(),'{13}','{13}', GETDATE(),'{14}','{13}', GETDATE(),'{14}','{15}','{16}',
                                        '{17}','{18}','{19}','{20}','{21}'
                                    )";

                sql = string.Format(SQL, issue_key.PreventSQLInjection(), 
                                         issueNo.PreventSQLInjection(), 
                                         itemNo.ToString("00"),
                                         orderNumber.PreventSQLInjection(), 
                                         materialCode.PreventSQLInjection(),
                                         materialDescription.PreventSQLInjection(),
                                         materialLot.PreventSQLInjection(), 
                                         issueStore.PreventSQLInjection(), 
                                         issueQty.PreventSQLInjection(),
                                         unit.PreventSQLInjection(), 
                                         supplierName.PreventSQLInjection(), 
                                         supplierLot.PreventSQLInjection(),
                                         shiftName.PreventSQLInjection(), 
                                         userName.PreventSQLInjection(), 
                                         timeZone.PreventSQLInjection(),
                                         storeMaterialDetailKey.PreventSQLInjection(), 
                                         oem.PreventSQLInjection(),
                                         proId.PreventSQLInjection(),
                                         efficiency.PreventSQLInjection(),
                                         grade.PreventSQLInjection(),
                                         supplierCode.PreventSQLInjection(),
                                         memo.PreventSQLInjection());
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                //更新之前的领料项目信息。
                if (IsUpdateOldReceiveMaterialInfo)
                {
                    //更新领料项目号对应的物料主键。
                    sql = string.Format(@"UPDATE a
                                        SET a.MATERIAL_KEY='{1}'
                                        FROM WST_STORE_MATERIAL a
                                        LEFT JOIN WST_STORE_MATERIAL_DETAIL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                        WHERE b.MATERIAL_LOT='{0}'",
                                        materialLot.PreventSQLInjection(),
                                        materialKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                    sql = string.Format(@"UPDATE b
                                        SET b.MATERIAL_SUPPLIER='{1}'
                                        FROM WST_STORE_MATERIAL_DETAIL b
                                        WHERE b.MATERIAL_LOT='{0}'",
                                        materialLot.PreventSQLInjection(),
                                        supplierName.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);

                    //更新领料项目号对应的信息。
                    sql = string.Format(@"UPDATE WST_SAP_ISSURE 
                                        SET AUFNR='{0}',MATNR='{1}',MATXT='{2}',LLIEF='{3}',PRO_ID='{4}',EFFICIENCY='{5}',GRADE='{6}',SUPPLIER_CODE='{7}'
                                        WHERE CHARG='{8}'",
                                        orderNumber.PreventSQLInjection(),
                                        materialCode.PreventSQLInjection(),
                                        materialDescription.PreventSQLInjection(),
                                        supplierName.PreventSQLInjection(),
                                        proId.PreventSQLInjection(),
                                        efficiency.PreventSQLInjection(),
                                        grade.PreventSQLInjection(),
                                        supplierCode.PreventSQLInjection(),
                                        materialLot.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                }

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("ManualSaveReceiveMaterial Error:{0}", ex.Message));
            }
            finally
            {
                dbCon.Close();
                dbTrans.Dispose();
                dbCon.Dispose();
            }
            return dsReturn;
        }
        /// <summary>
        /// 将选择的物料信息接收到线上仓。
        /// </summary>
        /// <param name="dtParams">包含物料信息的数据集对象。物料信息，员工号，时区</param>
        /// <returns>包含操作结果的数据集对象。</returns>
        public DataSet ReceiveLineMaterial(DataTable dtParams)
        {
            string materialKey = string.Empty;
            string storeKey = string.Empty;
            string storeMaterialKey = string.Empty;
            string storeMaterialDetailKey = string.Empty;
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            if (null != dtParams)
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                //Create Transaction
                try
                {
                    for (int i = 0; i < dtParams.Rows.Count; i++)
                    {
                        string materialCode = dtParams.Rows[i]["MATNR"].ToString().Trim();
                        string onlineWarehouse = dtParams.Rows[i]["OnlineWarehouse"].ToString().Trim();
                        //先判断物料在线上仓是否存在 通过Store 、Material、StoreMaterial 关联通过物料号+线上仓名称
                        string sql = string.Format(@"SELECT A.STORE_MATERIAL_KEY
                                                    FROM WST_STORE_MATERIAL A
                                                    INNER JOIN WST_STORE B ON A.STORE_KEY = B.STORE_KEY
                                                    INNER JOIN POR_MATERIAL C ON A.MATERIAL_KEY = C.MATERIAL_KEY 
                                                    WHERE C.MATERIAL_CODE='{0}' AND B.STORE_NAME='{1}'",
                                                    materialCode.PreventSQLInjection(),
                                                    onlineWarehouse.PreventSQLInjection());
                        dsReturn = db.ExecuteDataSet(dbTran, CommandType.Text, sql);
                        if (dsReturn.Tables[0].Rows.Count > 0)
                        {
                            storeMaterialKey = dsReturn.Tables[0].Rows[0]["STORE_MATERIAL_KEY"].ToString().Trim();
                        }
                        else
                        {
                            //获取库位对应的主键
                            string sqlStore = string.Format("SELECT A.STORE_KEY FROM WST_STORE A WHERE A.STORE_NAME='{0}'",
                                                             onlineWarehouse.PreventSQLInjection());
                            DataTable store = db.ExecuteDataSet(dbTran, CommandType.Text, sqlStore).Tables[0];
                            storeKey = store.Rows[0]["STORE_KEY"].ToString().Trim();

                            //物料信息判断
                            string sqlMaterial = string.Format("SELECT A.MATERIAL_KEY FROM POR_MATERIAL A WHERE A.MATERIAL_CODE='{0}'",
                                                                materialCode.PreventSQLInjection());
                            DataTable material = db.ExecuteDataSet(dbTran, CommandType.Text, sqlMaterial).Tables[0];
                            //通过返回信息来判断物料信息是否存在
                            if (material.Rows.Count > 0)
                            {
                                //获取物料的主键信息
                                materialKey = material.Rows[0]["MATERIAL_KEY"].ToString().Trim();
                            }
                            else
                            {
                                materialKey = UtilHelper.GenerateNewKey(0);
                                //插入新增物料的信息
                                string sqlMaterialInsert = string.Format(@"INSERT INTO POR_MATERIAL(MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,MATERIAL_VERSION,UNIT,MATERIAL_SPEC)
                                                                          VALUES ('{0}','{1}','{2}','1','{3}','{4}')",
                                                                          materialKey.PreventSQLInjection(),
                                                                          dtParams.Rows[i]["MATXT"].ToString().Trim().PreventSQLInjection(),
                                                                          dtParams.Rows[i]["MATNR"].ToString().Trim().PreventSQLInjection(),
                                                                          dtParams.Rows[i]["ERFME"].ToString().Trim().PreventSQLInjection(),
                                                                          dtParams.Rows[i]["MATXT"].ToString().Trim().PreventSQLInjection());
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlMaterialInsert);
                            }

                            //插入库位物料关联
                            storeMaterialKey = UtilHelper.GenerateNewKey(0);
                            string sqlSM = string.Format(@"INSERT INTO WST_STORE_MATERIAL(STORE_MATERIAL_KEY,STORE_KEY,MATERIAL_KEY) 
                                                           VALUES ('{0}','{1}','{2}')",
                                                           storeMaterialKey.PreventSQLInjection(),
                                                           storeKey.PreventSQLInjection(),
                                                           materialKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlSM);
                        }

                        string sqlSMD = string.Format(@"SELECT W.STORE_MATERIAL_DETAIL_KEY 
                                                       FROM  WST_STORE_MATERIAL_DETAIL W 
                                                       WHERE W.STORE_MATERIAL_KEY='{0}' AND W.MATERIAL_LOT='{1}'",
                                                       storeMaterialKey.PreventSQLInjection(),
                                                       dtParams.Rows[i]["CHARG"].ToString().Trim().PreventSQLInjection());
                        dsReturn = db.ExecuteDataSet(dbTran, CommandType.Text, sqlSMD);

                        if (dsReturn.Tables[0].Rows.Count > 0)
                        {
                            storeMaterialDetailKey = dsReturn.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"].ToString().Trim();
                            //更新该物料在对应库位的详细信息
                            string sqlUMSD = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                                            SET RECEIVE_QTY=RECEIVE_QTY+{0},CURRENT_QTY=CURRENT_QTY+{0}
                                                            WHERE STORE_MATERIAL_DETAIL_KEY='{1}'",
                                                            dtParams.Rows[i]["ERFMG"].ToString().Trim().PreventSQLInjection(),
                                                            storeMaterialDetailKey.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlUMSD);
                        }
                        else
                        {
                            storeMaterialDetailKey = System.Guid.NewGuid().ToString();
                            //插入物料在对应库位的详细信息
                            string sqlISMD = string.Format(@"INSERT INTO WST_STORE_MATERIAL_DETAIL (STORE_MATERIAL_DETAIL_KEY,STORE_MATERIAL_KEY ,MATERIAL_LOT,MATERIAL_SUPPLIER,RECEIVE_QTY,CURRENT_QTY)
                                                            VALUES ('{0}','{1}','{2}','{3}',{4},{4})",
                                                            storeMaterialDetailKey.PreventSQLInjection(),
                                                            storeMaterialKey.PreventSQLInjection(),
                                                            dtParams.Rows[i]["CHARG"].ToString().Trim().PreventSQLInjection(),
                                                            dtParams.Rows[i]["LLIEF"].ToString().Trim().PreventSQLInjection(),
                                                            dtParams.Rows[i]["ERFMG"].ToString().Trim().PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlISMD);
                        }
                        //更新
                        string sqlWSI = string.Format(@"UPDATE WST_SAP_ISSURE 
                                                        SET ISRECEIVED='1',RECEIVE_TIME=GETDATE(),SHIFT_NAME='{0}',OPERATOR='{1}',EDITOR='{1}',
                                                        EDIT_TIMEZONE='{2}',STORE_MATERIAL_DETAIL_KEY='{3}'
                                                        WHERE SAP_ISSURE_KEY='{4}'",
                                                        dtParams.Rows[i]["SHIFT_NAME"].ToString().Trim().PreventSQLInjection(),
                                                        dtParams.Rows[i]["OPERATOR"].ToString().Trim().PreventSQLInjection(),
                                                        dtParams.Rows[i]["EDIT_TIMEZONE"].ToString().Trim().PreventSQLInjection(),
                                                        storeMaterialDetailKey.PreventSQLInjection(),
                                                        dtParams.Rows[i]["SAP_ISSURE_KEY"].ToString().Trim().PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlWSI);
                    }
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    dbTran.Rollback();
                    LogService.LogError(string.Format("ReceiveLineMaterial Error:{0}", ex.Message));
                }
                finally
                {
                    dbTran = null;
                    dbConn.Close();
                    dbConn = null;
                }
            }
            return dsReturn;
        }
      
        /// <summary>
        /// 获取物料领用记录。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件的数据表对象。
        /// --------------------------------------
        /// {DO}{STORE_NAME}{OPERATION_NAME}{OPERATOR}{CHARG}{LLIEF}{MATNR}{MBLNR}{RECEIVE_TIME_START}{RECEIVE_TIME_END}
        /// --------------------------------------
        /// DO=Query
        /// ---------------------------------------
        /// DO=其他, 线上仓名称使用逗号分隔,store1,store1...
        /// --------------------------------------
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含物料领用详细信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialHistory(DataTable dtParams, ref PagingQueryConfig pconfig)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT  W.SAP_ISSURE_KEY,
                                       W.MBLNR ,
                                       W.CHARG ,
                                       W.AUFNR ,
                                       W.PRO_ID,
                                       W.GRADE,
                                       W.EFFICIENCY,
                                       W.LLIEF ,
                                       W.SUPPLIER_CODE,
                                       W.ERFMG ,
                                       W.MATNR ,
                                       W.MATXT ,
                                       W.ERFME ,
                                       B.OPERATION_NAME ,
                                       B.STORE_NAME ,
                                       F.LOCATION_NAME,
                                       W.RECEIVE_TIME ,
                                       W.SHIFT_NAME ,
                                       W.OPERATOR,
                                       W.MEMO
                                 FROM WST_STORE_MATERIAL A
                                 INNER JOIN WST_STORE B ON A.STORE_KEY = B.STORE_KEY
                                 INNER JOIN POR_MATERIAL C ON A.MATERIAL_KEY = C.MATERIAL_KEY
                                 INNER JOIN WST_STORE_MATERIAL_DETAIL D ON A.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                 INNER JOIN WST_SAP_ISSURE W ON D.STORE_MATERIAL_DETAIL_KEY = W.STORE_MATERIAL_DETAIL_KEY
                                 INNER JOIN FMM_LOCATION F ON B.LOCATION_KEY = F.LOCATION_KEY 
                                 WHERE B.OBJECT_STATUS=1 ";
                //判断是查询还是LOAD
                string doName = Convert.ToString(dtParams.Rows[0]["DO"]).Trim();
                if (doName == "Query")
                {
                    //领料项目号不为空
                    string materialLot = Convert.ToString(dtParams.Rows[0]["CHARG"]).Trim();
                    if (!string.IsNullOrEmpty(materialLot))
                    {
                        sql += string.Format(" AND W.CHARG LIKE '{0}%'", materialLot.PreventSQLInjection());
                    }
                    //工单号
                    string orderNo = Convert.ToString(dtParams.Rows[0]["AUFNR"]).Trim();
                    if (!string.IsNullOrEmpty(orderNo))
                    {
                        sql += string.Format(" AND W.AUFNR ='{0}'", orderNo.PreventSQLInjection());
                    }
                    //产品号
                    string proId = Convert.ToString(dtParams.Rows[0]["PRO_ID"]).Trim();
                    if (!string.IsNullOrEmpty(proId))
                    {
                        sql += string.Format(" AND W.PRO_ID ='{0}'", proId.PreventSQLInjection());
                    }
                    //转换效率
                    string efficiency = Convert.ToString(dtParams.Rows[0]["EFFICIENCY"]).Trim();
                    if (!string.IsNullOrEmpty(efficiency))
                    {
                        sql += string.Format(" AND W.EFFICIENCY ='{0}'", efficiency.PreventSQLInjection());
                    }
                    //等级
                    string grade = Convert.ToString(dtParams.Rows[0]["GRADE"]).Trim();
                    if (!string.IsNullOrEmpty(grade))
                    {
                        sql += string.Format(" AND W.GRADE ='{0}'", grade.PreventSQLInjection());
                    }
                    //供应商名称
                    string supplierName = Convert.ToString(dtParams.Rows[0]["LLIEF"]).Trim();
                    if (!string.IsNullOrEmpty(grade))
                    {
                        sql += string.Format(" AND W.LLIEF ='{0}'", supplierName.PreventSQLInjection());
                    }
                    //线上仓名称
                    string storeName = Convert.ToString(dtParams.Rows[0]["STORE_NAME"]).Trim();
                    if (!string.IsNullOrEmpty(grade))
                    {
                        sql += string.Format(" AND B.STORE_NAME ='{0}'", storeName.PreventSQLInjection());
                    }
                    //最早发料时间不为空
                    string receiveStartTime = Convert.ToString(dtParams.Rows[0]["RECEIVE_TIME_START"]).Trim();
                    if (!string.IsNullOrEmpty(receiveStartTime))
                    {
                        sql += string.Format(" AND W.RECEIVE_TIME >='{0}'", receiveStartTime.PreventSQLInjection());
                    }
                    //最晚发料时间不为空
                    string receiveEndTime = Convert.ToString(dtParams.Rows[0]["RECEIVE_TIME_END"]).Trim() + " 23:59:59";
                    if (!string.IsNullOrEmpty(receiveEndTime))
                    {
                        sql += string.Format(" AND W.RECEIVE_TIME <= '{0}'", receiveEndTime.PreventSQLInjection());
                    }
                }
                else
                {
                    string storeName = Convert.ToString(dtParams.Rows[0]["STORE_NAME"]).Trim();
                    if (!string.IsNullOrEmpty(storeName))
                    {
                        string sStoreName = UtilHelper.BuilderWhereConditionString("B.STORE_NAME", storeName.Split(','));
                        sql += sStoreName;
                    }
                }
                if (pconfig==null)
                {
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sql, pconfig.PageNo, pconfig.PageSize, out pages,
                        out records, db, dsReturn, "STORE_MATERIAL");
                    pconfig.Pages = pages;
                    pconfig.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetReceiveMaterialHistory Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        /// <summary>
        /// 为工单BOM添加自备料。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="materialCode">物料编码。</param>
        /// <param name="materialDescription">物料描述。</param>
        /// <returns>包含操作结果的数据集对象。</returns>
        public DataSet CreateWOBomOwnMaterial(string orderNumber, string materialCode, string materialDescription)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DbCommand cmd=db.GetStoredProcCommand("SP_CreateWOBomOwnMaterial");
                db.AddInParameter(cmd, "@orderNumber", DbType.String, orderNumber);
                db.AddInParameter(cmd, "@materialCode", DbType.String, materialCode);
                db.AddInParameter(cmd, "@description", DbType.String, materialDescription);
                db.ExecuteNonQuery(cmd);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError(string.Format("CreateWOBomOwnMaterial Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
    }
}

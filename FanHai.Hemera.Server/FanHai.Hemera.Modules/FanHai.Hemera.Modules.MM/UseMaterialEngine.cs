using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Collections;



namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 耗用物料的数据操作类。
    /// </summary>
    public class UseMaterialEngine : AbstractEngine, IUseMaterialEngine
    {
        private Database db = null;//数据库对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UseMaterialEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 根据线边仓名称获取车间名称和主键。
        /// </summary>
        /// <param name="STORES">用户拥有权限的线边仓名称,使用逗号分隔:store1,store2...</param>
        /// <returns>包含车间名称和车间主键的数据集对象。</returns>
        public DataSet GetWorkShopInfo(string stores)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = "";
                //线边仓名称长度>0,即方法参数传入了线边仓名称。
                if (stores.Length > 0)
                {
                    sql = @"SELECT DISTINCT V.PARENT_KEY,V.PARENT_NAME 
                            FROM V_LOCATION_RET V,WST_STORE S 
                            WHERE S.LOCATION_KEY = V.LOCATION_KEY";
                    string sqlCondition = UtilHelper.BuilderWhereConditionString("STORE_NAME", stores.Split(','));
                    sql += sqlCondition;
                }
                else//如果没有传入线边仓名称。
                {
                    sql = @"SELECT DISTINCT V.PARENT_KEY,V.PARENT_NAME 
                            FROM V_LOCATION_RET V,WST_STORE S 
                            WHERE S.LOCATION_KEY = V.LOCATION_KEY";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkShopInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工序名称和车间名称获取设备信息。
        /// </summary>
        /// <param name="operationname">工序名称。</param>
        /// <param name="cmbfactoryroom">车间名称。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        public DataSet GetEquipmentInfo(string operationname, string cmbfactoryroom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT D.EQUIPMENT_KEY,D.EQUIPMENT_NAME 
                               FROM EMS_OPERATION_EQUIPMENT E,EMS_EQUIPMENTS D,POR_ROUTE_OPERATION_VER F,V_LOCATION_RET V 
                               WHERE E.EQUIPMENT_KEY = D.EQUIPMENT_KEY 
                               AND F.ROUTE_OPERATION_VER_KEY = E.OPERATION_KEY 
                               AND D.LOCATION_KEY = V.LOCATION_KEY";
                //传入的工序名称不为空
                if (!string.IsNullOrEmpty(operationname))
                {
                    sql += string.Format(" AND F.ROUTE_OPERATION_NAME = '{0}'", operationname.PreventSQLInjection());
                }
                //传入车间名称不为空
                if (!string.IsNullOrEmpty(cmbfactoryroom))
                {
                    sql += string.Format(" AND V.PARENT_NAME='{0}'", cmbfactoryroom.PreventSQLInjection());
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipmentInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取物料耗用信息。
        /// </summary>
        /// <param name="operations">工序名称,使用逗号分隔:op1,op2...</param>
        /// <param name="stores">线上仓名称，使用逗号分隔:store1,store2...</param>
        /// <returns>包含物料耗用信息的数据集对象。</returns>
        public DataSet GetMaterialUsed(string operations, string stores)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY A.MATERIAL_LOT) AS ROWNUMBER,
                                        A.MATERIAL_USED_DETAIL_KEY,A.MATERIAL_USED_KEY,A.MATERIAL_LOT,A.OPERATION_KEY,A.ROUTE_OPERATION_NAME,
                                        A.LOCATION_KEY,A.LOCATION_NAME,A.EQUIPMENT_KEY,A.EQUIPMENT_NAME,A.SHIFT_NAME,A.USED_TIME,A.OPERATOR,A.MATNR,A.MATXT,A.LLIEF,
                                        A.USED_QTY,A.ERFME,A.STORE_NAME,A.STIR_TIME,A.PRINT_QTY,A.STATUS
                                 FROM 
                                        (SELECT DISTINCT W.MATERIAL_USED_DETAIL_KEY,U.MATERIAL_USED_KEY,D.MATERIAL_LOT,U.OPERATION_KEY,V.ROUTE_OPERATION_NAME,
                                                        F.LOCATION_KEY,F.LOCATION_NAME,E.EQUIPMENT_KEY,E.EQUIPMENT_NAME,U.SHIFT_NAME,U.USED_TIME,U.OPERATOR,I.MATNR,
                                                        I.MATXT,I.LLIEF,W.USED_QTY,I.ERFME,S.STORE_NAME,W.STIR_TIME,W.PRINT_QTY,W.STATUS
                                        FROM
                                            FMM_LOCATION  F,
                                            WST_STORE S,
                                            WST_STORE_MATERIAL M,
                                            WST_STORE_MATERIAL_DETAIL D,
                                            WST_SAP_ISSURE I,
                                            WST_MATERIAL_USED_DETAIL W,
                                            WST_MATERIAL_USED U,
                                            POR_ROUTE_OPERATION_VER V,
                                            EMS_EQUIPMENTS E 
                                        WHERE F.LOCATION_KEY = S.LOCATION_KEY
                                        AND S.STORE_KEY = M.STORE_KEY
                                        AND M.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                        AND D.STORE_MATERIAL_DETAIL_KEY = I.STORE_MATERIAL_DETAIL_KEY
                                        AND D.STORE_MATERIAL_DETAIL_KEY = W.STORE_MATERIAL_DETAIL_KEY
                                        AND W.MATERIAL_USED_KEY = U.MATERIAL_USED_KEY
                                        AND U.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                        AND U.EQUIPMENT_KEY = E.EQUIPMENT_KEY AND STATUS!=0 ";


                string sqlConditionStore = UtilHelper.BuilderWhereConditionString("S.STORE_NAME", stores.Split(','));
                sql += sqlConditionStore;
                string sqlConditionStore1 = UtilHelper.BuilderWhereConditionString("V.ROUTE_OPERATION_NAME", operations.Split(','));
                sql += sqlConditionStore1;
                sql += ") A";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaterialUsed Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取材料耗用详细信息。
        /// </summary>
        /// <param name="_materialLot">物料批号。</param>
        /// <param name="_gongXuName">工序名称。</param>
        /// <param name="_wuLiaoNumber">物料料号。</param>
        /// <param name="_factoryRoomName">车间名称。</param>
        /// <param name="_wuLiaoMiaoShu">物料描述。</param>
        /// <param name="_equipmentName">设备名称。</param>
        /// <param name="_gongYingShang">供应商名称。</param>
        /// <param name="_banCi">班次。</param>
        /// <param name="_lineCang">线上仓名称。</param>
        /// <param name="_jobNumber">工号。</param>
        /// <param name="_startTime">耗用时间。</param>
        /// <param name="_endTime">耗用时间。</param>
        /// <param name="_stores">线上仓名称。使用逗号分隔：store1,store2...</param>
        /// <param name="_operations">工序名称。使用逗号分隔：op1,op2...</param>
        /// <returns></returns>
        public DataSet GetStoreMaterialDetail(string _materialLot, string _gongXuName, string _wuLiaoNumber,
            string _factoryRoomName, string _wuLiaoMiaoShu, string _equipmentName, string _gongYingShang,
            string _banCi, string _lineCang, string _jobNumber,
            DateTime _startTime, DateTime _endTime, 
            string _stores, string _operations)
        {
            string sqlCondition = "";
            string sqlConditionStore = "";
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY A.MATERIAL_LOT) AS ROWNUMBER,
                                    A.MATERIAL_LOT,A.MATNR,A.MATXT,A.USED_QTY,A.ERFME,A.LLIEF,A.STORE_NAME,A.ROUTE_OPERATION_NAME,
                                    A.LOCATION_NAME,A.EQUIPMENT_NAME,A.SHIFT_NAME,A.USED_TIME,A.OPERATOR,A.STIR_TIME,A.PRINT_QTY
                                FROM 
                                (
                                    SELECT DISTINCT D.MATERIAL_LOT,I.MATNR,I.MATXT,W.USED_QTY,I.ERFME,I.LLIEF,S.STORE_NAME,V.ROUTE_OPERATION_NAME,
                                            F.LOCATION_NAME,E.EQUIPMENT_NAME,U.SHIFT_NAME,U.USED_TIME,U.OPERATOR,W.STIR_TIME,W.PRINT_QTY
                                    FROM FMM_LOCATION  F,
                                        WST_STORE S,
                                        WST_STORE_MATERIAL M,
                                        WST_STORE_MATERIAL_DETAIL D,
                                        WST_SAP_ISSURE I,
                                        WST_MATERIAL_USED_DETAIL W,
                                        WST_MATERIAL_USED U,
                                        POR_ROUTE_OPERATION_VER V,
                                        EMS_EQUIPMENTS E 
                                    WHERE F.LOCATION_KEY = S.LOCATION_KEY
                                    AND S.STORE_KEY = M.STORE_KEY
                                    AND M.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                    AND D.STORE_MATERIAL_DETAIL_KEY = I.STORE_MATERIAL_DETAIL_KEY
                                    AND D.STORE_MATERIAL_DETAIL_KEY = W.STORE_MATERIAL_DETAIL_KEY
                                    AND W.MATERIAL_USED_KEY = U.MATERIAL_USED_KEY
                                    AND U.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                    AND U.EQUIPMENT_KEY = E.EQUIPMENT_KEY ";
                if (!string.IsNullOrEmpty(_materialLot))
                {
                    sql += string.Format(" AND D.MATERIAL_LOT LIKE '{0}%'", _materialLot.PreventSQLInjection());
                }
                if (string.IsNullOrEmpty(_gongXuName))
                {
                    sqlCondition = UtilHelper.BuilderWhereConditionString("ROUTE_OPERATION_NAME", _operations.Split(','));
                    sql += sqlCondition;
                }
                else
                {
                    sql += string.Format(" AND V.ROUTE_OPERATION_NAME='{0}'", _gongXuName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_wuLiaoNumber))
                {
                    sql += string.Format(" AND I.MATNR LIKE '{0}%'", _wuLiaoNumber.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_factoryRoomName))
                {
                    sql += string.Format(" AND F.LOCATION_NAME='{0}'", _factoryRoomName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_wuLiaoMiaoShu))
                {
                    sql += string.Format(" AND I.MATXT LIKE '{0}%'", _wuLiaoMiaoShu.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_equipmentName))
                {
                    sql += string.Format(" AND E.EQUIPMENT_NAME='{0}'", _equipmentName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_gongYingShang))
                {
                    sql += string.Format(" AND I.LLIEF LIKE '{0}%'", _gongYingShang.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_banCi))
                {
                    sql += string.Format(" AND U.SHIFT_NAME='{0}'", _banCi.PreventSQLInjection());
                }
                if (string.IsNullOrEmpty(_lineCang))
                {
                    sqlConditionStore = UtilHelper.BuilderWhereConditionString("STORE_NAME", _stores.Split(','));
                    sql += sqlConditionStore;
                }
                else
                {
                    sql += string.Format(" AND S.STORE_NAME='{0}'", _lineCang.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_jobNumber))
                {
                    sql += string.Format(" AND U.OPERATOR LIKE '{0}%'", _jobNumber.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_startTime.ToString()))
                {
                    sql += string.Format(" AND U.USED_TIME>='{0}'", _startTime);
                }
                if (!string.IsNullOrEmpty(_endTime.ToString()))
                {
                    sql += string.Format(" AND U.USED_TIME<='{0}'", _endTime);
                }

                sql +=") A";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetStoreMaterialDetail Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过工序车间和物料批号获取物料信息。
        /// </summary>
        /// <param name="materialLot">物料批号</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="cmbFactoryRoom">工厂车间</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        public DataSet GetMaterialByLotOpFa(string materialLot, string operationName, string cmbFactoryRoom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT D.MATERIAL_LOT,D.MATERIAL_LOT,I.MATNR,I.MATXT,I.LLIEF,D.CURRENT_QTY,I.ERFME,S.STORE_NAME,
                                    D.STORE_MATERIAL_DETAIL_KEY
                               FROM 
                                    FMM_LOCATION  F,
                                    WST_STORE S,
                                    WST_STORE_MATERIAL M,
                                    WST_STORE_MATERIAL_DETAIL D,
                                    WST_SAP_ISSURE I
                               WHERE  
                                    F.LOCATION_KEY = S.LOCATION_KEY
                                    AND S.STORE_KEY = M.STORE_KEY
                                    AND M.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                    AND D.STORE_MATERIAL_DETAIL_KEY = I.STORE_MATERIAL_DETAIL_KEY
                                    AND D.CURRENT_QTY<>0 
                                    AND MATNR NOT LIKE '100%'";
                if (!string.IsNullOrEmpty(materialLot))
                {
                    sql += string.Format(" AND D.MATERIAL_LOT = '{0}'", materialLot.PreventSQLInjection());
                }
                //传入的工序名称不为空
                if (!string.IsNullOrEmpty(operationName))
                {
                    sql += string.Format(" AND S.OPERATION_NAME = '{0}'", operationName.PreventSQLInjection());
                }
                //传入车间名称不为空
                if (!string.IsNullOrEmpty(cmbFactoryRoom))
                {
                    sql += string.Format(" AND F.LOCATION_NAME ='{0}'", cmbFactoryRoom.PreventSQLInjection());
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaterialByLotOpFa Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据耗用主键获取物料耗用信息
        /// </summary>
        /// <param name="materialUsedDetalKey">耗用主键</param>
        /// <returns>包含物料耗用信息的数据集对象。</returns>
        public DataSet GetMaterialDetailByKey(string materialUsedDetalKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT 1 AS ROWNUMBER ,U.MATERIAL_USED_KEY,W.MATERIAL_USED_DETAIL_KEY,D.MATERIAL_LOT,U.OPERATION_KEY,V.ROUTE_OPERATION_NAME,F.LOCATION_KEY,
                                    F.LOCATION_NAME,E.EQUIPMENT_KEY,E.EQUIPMENT_NAME,U.SHIFT_NAME,U.USED_TIME,U.OPERATOR,I.MATNR,I.MATXT,I.LLIEF,W.USED_QTY AS CURRENT_QTY,
                                    I.ERFME,S.STORE_NAME,W.STIR_TIME,W.PRINT_QTY,W.STATUS
                               FROM
                                    FMM_LOCATION  F,
                                    WST_STORE S,
                                    WST_STORE_MATERIAL M,
                                    WST_STORE_MATERIAL_DETAIL D,
                                    WST_SAP_ISSURE I,
                                    WST_MATERIAL_USED_DETAIL W,
                                    WST_MATERIAL_USED U,
                                    POR_ROUTE_OPERATION_VER V,
                                    EMS_EQUIPMENTS E
                                WHERE 
                                    F.LOCATION_KEY = S.LOCATION_KEY
                                    AND S.STORE_KEY = M.STORE_KEY
                                    AND M.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                    AND D.STORE_MATERIAL_DETAIL_KEY = I.STORE_MATERIAL_DETAIL_KEY
                                    AND D.STORE_MATERIAL_DETAIL_KEY = W.STORE_MATERIAL_DETAIL_KEY
                                    AND W.MATERIAL_USED_KEY = U.MATERIAL_USED_KEY
                                    AND U.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                    AND U.EQUIPMENT_KEY = E.EQUIPMENT_KEY";
                //传入车间名称不为空
                sql += string.Format(" AND W.MATERIAL_USED_DETAIL_KEY ='{0}'", materialUsedDetalKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaterialDetailByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过批次物料批号和物料耗用主键获取未结账耗用明细。
        /// </summary>
        /// <param name="materialLot">物料批次号。</param>
        /// <param name="materitalUseKey">耗用主键。</param>
        /// <returns>包含物料耗用明细的数据集对象。</returns>
        public DataSet GetMaterialDetailByMaterialLot(string materialLot, string materitalUseKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT * 
                                            FROM WST_MATERIAL_USED_DETAIL 
                                            WHERE MATERIAL_LOT = '{0}' 
                                            AND STATUS=1 
                                            AND MATERIAL_USED_KEY = '{1}'", 
                                           materialLot.PreventSQLInjection(),
                                           materitalUseKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMaterialDetailByMaterialLot Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新物料耗用及其明细记录。
        /// </summary>
        /// <param name="materialLot">物料批号。</param>
        /// <param name="materialUsedKey">物料耗用主键。</param>
        /// <param name="materialUsedDetailKey">物料耗用明细主键</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="equipmentKey">设备名称。</param>
        /// <param name="usedTime">耗用时间。2012-09-12 13:00:00</param>
        /// <param name="stirTime">搅拌时间。2012-09-12 13:00:00</param>
        /// <param name="printQty">印刷数量。</param>
        /// <param name="dtParams">
        /// 包含更新信息的数据表对象。
        /// --------------------------------------------
        /// {EDITOR}
        /// {EDIT_TIMEZONE}
        /// --------------------------------------------
        /// </param>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public bool UpDateMaterialUsedAndDetail(string materialLot, string materialUsedKey, string materialUsedDetailKey, string operationName,
            string equipmentKey, string usedTime, string stirTime, string printQty,DataTable dtParams)
        {
            bool isFinish = false;
            List<string> sqlList = new List<string>();
            string detailSql = string.Empty;
            string operationname = string.Empty;
            string usedSql = string.Empty;
            DataSet ds = new DataSet();
            DbConnection dbConn = db.CreateConnection();
            dbConn.Open();
            DbTransaction dbTrans = dbConn.BeginTransaction();
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
            try
            {
                //更新材料耗用明细列表
                detailSql = string.Format(@"UPDATE WST_MATERIAL_USED_DETAIL 
                                            SET STIR_TIME ='{0}',PRINT_QTY = {1}
                                            WHERE MATERIAL_LOT = '{2}' AND  MATERIAL_USED_DETAIL_KEY ='{3}'",
                                            stirTime.PreventSQLInjection(),
                                            printQty.PreventSQLInjection(),
                                            materialLot.PreventSQLInjection(),
                                            materialUsedDetailKey.PreventSQLInjection());
                sqlList.Add(detailSql);
                //更新材料耗用表
                //根据工序名称获取工序主键。
                string sql = string.Format(@"SELECT ROUTE_OPERATION_VER_KEY 
                                            FROM POR_ROUTE_OPERATION_VER
                                            WHERE ROUTE_OPERATION_NAME = '{0}'",
                                            operationName.PreventSQLInjection());
                ds = db.ExecuteDataSet(CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count != 1)
                {
                    return false;
                }
                string operationKey = Convert.ToString(ds.Tables[0].Rows[0][0]);
                string editor = Convert.ToString(htParams["EDITOR"]);
                string editTimeZone = Convert.ToString(htParams["EDIT_TIMEZONE"]);
                usedSql = string.Format(@"UPDATE WST_MATERIAL_USED
                                          SET OPERATION_KEY = '{0}',EQUIPMENT_KEY = '{1}',USED_TIME= '{2}',
                                              EDITOR= '{3}',EDIT_TIMEZONE= '{4}',EDIT_TIME = GETDATE()
                                          WHERE MATERIAL_USED_KEY = '{5}'",
                                          operationKey.PreventSQLInjection(),
                                          equipmentKey.PreventSQLInjection(),
                                          usedTime.PreventSQLInjection(),
                                          editor.PreventSQLInjection(),
                                          editTimeZone.PreventSQLInjection(),
                                          materialUsedKey.PreventSQLInjection());
                sqlList.Add(usedSql);
                int reInt = 0;
                foreach (string sql1 in sqlList)
                {
                    reInt = db.ExecuteNonQuery(CommandType.Text, sql1);
                }
                dbTrans.Commit();
                isFinish = true;
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpDateMaterialUsedAndDetail Error: " + ex.Message);
                isFinish = false;
            }
            finally
            {
                dbTrans = null;
                dbConn.Close();
                dbConn = null;
            }
            return isFinish;
        }
        /// <summary>
        /// 删除物料耗用。
        /// </summary>
        /// <param name="materialLot">物料批号。</param>
        /// <param name="materialUsedKey">物料耗用主键。</param>
        /// <param name="materialUsedDetailKey">物料耗用明细主键。</param>
        /// <returns>true：成功。false：失败。</returns>
        public bool DeleteMaterital(string materialLot, string materialUsedKey, string materialUsedDetailKey)
        {
            bool isFinish = false;
            string countSql = string.Empty;
            List<string> sqlList = new List<string>();
            string storeSql = string.Empty;
            string operationname = string.Empty;
            string usedSql = string.Empty;
            string materialStore = string.Empty;
            string qtySql = string.Empty;
            string deleteDetilSql = string.Empty;
            string deleteUsedSql = string.Empty;
            decimal sumQty = 0;
            DataSet ds = new DataSet();
            DataSet dsUsed = new DataSet();
            DataSet dsMaterialStore = new DataSet();
            DataSet dsQty = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                

                //获取耗用数量,因为还要修改线上仓物料表中的数量
                usedSql = string.Format("SELECT USED_QTY FROM WST_MATERIAL_USED_DETAIL WHERE MATERIAL_USED_DETAIL_KEY ='{0}'",
                                        materialUsedDetailKey.PreventSQLInjection());
                dsUsed = db.ExecuteDataSet(dbTrans, CommandType.Text, usedSql);
                //获取物料所在仓库的明细主键。
                materialStore = string.Format(@"SELECT STORE_MATERIAL_DETAIL_KEY 
                                                FROM WST_MATERIAL_USED_DETAIL 
                                                WHERE MATERIAL_LOT ='{0}'
                                                AND MATERIAL_USED_KEY='{1}'",
                                                materialLot.PreventSQLInjection(),
                                                materialUsedKey.PreventSQLInjection());
                dsMaterialStore = db.ExecuteDataSet(dbTrans, CommandType.Text, materialStore);
                if (dsMaterialStore != null && dsMaterialStore.Tables.Count > 0 && dsMaterialStore.Tables[0].Rows.Count > 0)
                {
                    string storeMaterialDetailKey = Convert.ToString(dsMaterialStore.Tables[0].Rows[0][0]);
                    //获取数量因为修改线上仓物料表中的数量时得先取出数据然后相加得到数量
                    qtySql = string.Format("SELECT CURRENT_QTY FROM WST_STORE_MATERIAL_DETAIL WHERE STORE_MATERIAL_DETAIL_KEY='{0}'",
                                            storeMaterialDetailKey.PreventSQLInjection());
                    dsQty = db.ExecuteDataSet(CommandType.Text, qtySql);
                    sumQty = Convert.ToDecimal(dsUsed.Tables[0].Rows[0][0].ToString()) + Convert.ToDecimal(dsQty.Tables[0].Rows[0][0].ToString());
                    //modi  by chao.pang 来料接收程序修改引发的只记录材料批次号，不扣减数量
                    //更新线上仓物料数量
                    //storeSql = " UPDATE WST_STORE_MATERIAL_DETAIL " +
                    //           " SET  CURRENT_QTY= " + sumQty + "" +
                    //           " WHERE STORE_MATERIAL_DETAIL_KEY ='" + storeMaterialDetailKey.PreventSQLInjection() + "'";
                    //sqlList.Add(storeSql);
                }
                //从耗用明细表中删除该主键。
                deleteDetilSql =string.Format(@"DELETE FROM WST_MATERIAL_USED_DETAIL 
                                                WHERE  MATERIAL_LOT= '{0}' AND MATERIAL_USED_DETAIL_KEY='{1}'" ,
                                                materialLot.PreventSQLInjection(),
                                                materialUsedDetailKey.PreventSQLInjection());
                sqlList.Add(deleteDetilSql);

                //获取明细表中有该物料耗用表主键的条数
                countSql = string.Format("SELECT COUNT(*) FROM WST_MATERIAL_USED_DETAIL WHERE MATERIAL_USED_KEY = '{0}'",
                                        materialUsedKey.PreventSQLInjection());
                int count = Convert.ToInt32(db.ExecuteScalar(dbTrans, CommandType.Text, countSql));
                //判断如果主键条数为1，表示只有唯一这么一条记录，则需要在物料耗用表中删除该条记录。
                if (count == 1)
                {
                    deleteUsedSql = "DELETE FROM WST_MATERIAL_USED WHERE MATERIAL_USED_KEY='" + materialUsedKey.PreventSQLInjection() + "'";
                    sqlList.Add(deleteUsedSql);
                }

                foreach (string sql1 in sqlList)
                {
                    db.ExecuteNonQuery(dbTrans,CommandType.Text, sql1);
                }
                dbTrans.Commit();
                isFinish = true;
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("DeleteMaterital Error: " + ex.Message);
                isFinish = false;
            }
            return isFinish;
        }
        /// <summary>
        /// 新增物料耗用。
        /// </summary>
        /// <param name="dsParams">包含物料耗用记录的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertMaterial(DataSet dsParams)
        {
            const string CONST_GET_CURRENT_QTY_SQL = @" SELECT ISNULL(CURRENT_QTY,0) AS CURRENT_QTY
                                                        FROM WST_STORE_MATERIAL_DETAIL 
                                                        WHERE STORE_MATERIAL_DETAIL_KEY='{0}'";

            const string CONST_INSERT_1 = @" INSERT INTO WST_MATERIAL_USED_DETAIL
                                                (MATERIAL_USED_DETAIL_KEY, MATERIAL_USED_KEY, STORE_MATERIAL_DETAIL_KEY,
                                                MATERIAL_LOT, STATUS, USED_QTY, STIR_TIME,PRINT_QTY)
                                            VALUES('{0}','{1}','{2}','{3}',1,{4},'{5}',{6})";
            const string CONST_INSERT_2 = @" INSERT INTO WST_MATERIAL_USED 
                                                (MATERIAL_USED_KEY,OPERATION_KEY,EQUIPMENT_KEY,SHIFT_NAME,
                                                 OPERATOR,USED_TIME,DESCRIPTION,CREATE_TIME,CREATOR,CREATE_TIMEZONE,
                                                 EDITOR,EDIT_TIME,EDIT_TIMEZONE)
                                             VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE(),'{7}','{8}','{9}',GETDATE(),'{10}')";

            string sqlInsert = string.Empty;

            DataSet dsReturn = new DataSet();
            IList<string> sqlCommandList = new List<string>();

            DataTable dtDetail = dsParams.Tables["MATERIAL_USED_DATAIL"];
            DataTable dtUsed=dsParams.Tables["MATERIAL_USED"];
            DataTable dtHash=dsParams.Tables["HASH"];
            Hashtable hsTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);
                
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                //根据工序名称获取工序主键。
                string operationName = Convert.ToString(dtUsed.Rows[0][0]);
                string sql = string.Format(@"SELECT ROUTE_OPERATION_VER_KEY 
                                            FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'",
                                            operationName.PreventSQLInjection());
                string strOperationKey = Convert.ToString(db.ExecuteScalar(dbTrans, CommandType.Text, sql));
                //生成插入耗用记录的SQL
                string key = UtilHelper.GenerateNewKey(0);
                string strInsertInToUsed = string.Format(CONST_INSERT_2,
                    key.PreventSQLInjection(),                                        //主键
                    strOperationKey.PreventSQLInjection(),                            //工序主键 
                    dtUsed.Rows[0][1].ToString().PreventSQLInjection(),               //设备主键（EMS_EQUIPMENTS)
                    dtUsed.Rows[0][2].ToString().PreventSQLInjection(),               //班次值
                    dtUsed.Rows[0][4].ToString().PreventSQLInjection(),               //操作人
                    dtUsed.Rows[0][3].ToString().PreventSQLInjection(),               //耗用时间
                    dtUsed.Rows[0][5].ToString().PreventSQLInjection(),               //描述   
                    hsTable["CREATOR"].ToString().PreventSQLInjection(),              //创建人
                    hsTable["CREATE_TIMEZONE"].ToString().PreventSQLInjection(),      //创建时区
                    hsTable["EDITOR"].ToString().PreventSQLInjection(),               //编辑人
                    hsTable["EDIT_TIMEZONE"].ToString().PreventSQLInjection()         //编辑时区
                    );
                sqlCommandList.Add(strInsertInToUsed);
                //生成插入耗用明细记录的SQL
                foreach (DataRow dr in dtDetail.Rows)
                {
                    string strStoreMaterialKey = dr["STORE_MATERIAL_DETAIL_KEY"].ToString();            //线上仓物料主键
                    string strStoreMaterialLot = dr["MATERIAL_LOT"].ToString();                         //线上仓物料批号
                    double dUsedQty = Convert.ToDouble(dr["CURRENT_QTY"]);                              //耗用数量
                    string dtStirTime = dr["STIR_TIME"].ToString();
                    string strPrintQty = dr["PRINT_QTY"].ToString();
                    double dPrintQty = 0;
                    if (!double.TryParse(strPrintQty, out dPrintQty)) dPrintQty = 0;

                    //根据线上仓物料主键获取物料批对应的当前数量
                    string strGetCurrentQtySql = string.Format(CONST_GET_CURRENT_QTY_SQL, strStoreMaterialKey.PreventSQLInjection());
                    object objCurrentQty = db.ExecuteScalar(CommandType.Text, strGetCurrentQtySql);
                    double dCurrentQty = 0;
                    if (objCurrentQty != null) dCurrentQty = Convert.ToDouble(objCurrentQty);

                    if (dCurrentQty < dUsedQty)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "物料:" + strStoreMaterialLot + "的当前数量小于耗用数量，不能保存。");
                        return dsReturn;
                    }
                    //生成向物料耗用明细表插入记录的SQL语句
                    string strMaterialUsedDetailKey = System.Guid.NewGuid().ToString();
                    string sqlDetail = string.Format(CONST_INSERT_1,
                                                    strMaterialUsedDetailKey,                    //主键
                                                    key,
                                                    strStoreMaterialKey,
                                                    strStoreMaterialLot,
                                                    dUsedQty,
                                                    dtStirTime,
                                                    dPrintQty
                                                    );
                    sqlCommandList.Add(sqlDetail);

                    //modi  by chao.pang 来料接收程序修改引发的只记录材料批次号，不扣减数量
                    //更新线上仓物料数量
                    //string strUpdateStoreSql = " UPDATE WST_STORE_MATERIAL_DETAIL " +
                    //           " SET  CURRENT_QTY= " + Convert.ToString(dCurrentQty - dUsedQty) + "" +
                    //           " WHERE STORE_MATERIAL_DETAIL_KEY ='" + strStoreMaterialKey.PreventSQLInjection() + "'";
                    //sqlCommandList.Add(strUpdateStoreSql);
                }
                foreach (string sql1 in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql1);
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("InsertMaterial Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            finally
            {
                dbTrans = null;
                dbCon.Close();
                dbCon = null;
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取物料耗用的所有信息。
        /// </summary>
        /// <returns>包含物料耗用信息的数据集对象。</returns>
        public DataSet GetAllMaterialUsed()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.MATERIAL_LOT) AS ROWNUMBER,
                                    A.MATERIAL_LOT,A.MATNR,A.MATXT,A.USED_QTY,A.ERFME,A.LLIEF,A.STORE_NAME,A.ROUTE_OPERATION_NAME,
                                    A.LOCATION_NAME,A.EQUIPMENT_NAME,A.SHIFT_NAME,A.USED_TIME,A.OPERATOR,A.STIR_TIME,A.PRINT_QTY
                                FROM 
                                (
                                SELECT DISTINCT D.MATERIAL_LOT,I.MATNR,I.MATXT,W.USED_QTY,
                                    I.ERFME,I.LLIEF,S.STORE_NAME,V.ROUTE_OPERATION_NAME,
                                    F.LOCATION_NAME,E.EQUIPMENT_NAME,U.SHIFT_NAME,U.USED_TIME,
                                    U.OPERATOR,W.STIR_TIME,W.PRINT_QTY
                                FROM
                                    FMM_LOCATION  F,
                                    WST_STORE S,
                                    WST_STORE_MATERIAL M,
                                    WST_STORE_MATERIAL_DETAIL D,
                                    WST_SAP_ISSURE I,
                                    WST_MATERIAL_USED_DETAIL W,
                                    WST_MATERIAL_USED U,
                                    POR_ROUTE_OPERATION_VER V,
                                    EMS_EQUIPMENTS E 
                                WHERE F.LOCATION_KEY = S.LOCATION_KEY
                                AND S.STORE_KEY = M.STORE_KEY
                                AND M.STORE_MATERIAL_KEY = D.STORE_MATERIAL_KEY
                                AND D.STORE_MATERIAL_DETAIL_KEY = I.STORE_MATERIAL_DETAIL_KEY
                                AND D.STORE_MATERIAL_DETAIL_KEY = W.STORE_MATERIAL_DETAIL_KEY
                                AND W.MATERIAL_USED_KEY = U.MATERIAL_USED_KEY
                                AND U.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                AND U.EQUIPMENT_KEY = E.EQUIPMENT_KEY) A";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAllMaterialUsed Error: " + ex.Message);
            }
            return dsReturn;

        }
    }
}

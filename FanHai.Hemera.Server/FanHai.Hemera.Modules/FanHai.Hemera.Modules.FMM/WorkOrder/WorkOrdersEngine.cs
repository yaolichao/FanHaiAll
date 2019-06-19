using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.FMM
{
    /// <summary>
    /// 工单数据操作管理类。
    /// </summary>
    public class WorkOrdersEngine : AbstractEngine, IWorkOrdersEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WorkOrdersEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 根据工单主键获取工单信息。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <returns>包含工单信息的数据集对象。</returns>
        public DataSet GetWorkOrderByKey(string workOrderKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(workOrderKey))
                {
                    string sql = @" SELECT A.* FROM POR_WORK_ORDER A WHERE WORK_ORDER_KEY='" + workOrderKey.PreventSQLInjection() + "'";

                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                    sql = @"SELECT A.*,B.DATA_TYPE AS DATA_TYPE 
                            FROM POR_WORK_ORDER_ATTR A 
                            LEFT JOIN BASE_ATTRIBUTE B ON A.ATTRIBUTE_KEY=B.ATTRIBUTE_KEY 
                            WHERE A.ISFLAG=1 AND A.WORK_ORDER_KEY = '" + workOrderKey.PreventSQLInjection() + "'";
                    DataTable udaDataTable = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                    udaDataTable.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                    //ADD UDA TABLE TO DATASET
                    dsReturn.Merge(udaDataTable, true, MissingSchemaAction.Add);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "工单主键为空。");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderByKey Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号获取工单主键。
        /// </summary>
        /// <param name="workOrder">工单号。</param>
        /// <returns>包含工单主键信息的数据集。</returns>
        public DataSet GetWorkOrderKeyByOrder(string workOrder)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(workOrder))
                {
                    string sql = string.Format(@"SELECT y.WORK_ORDER_KEY FROM POR_WORK_ORDER y WHERE y.ORDER_NUMBER = '{0}'",
                                         workOrder.PreventSQLInjection());
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderKeyByOrder Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据车间名称获取工单号。
        /// </summary>
        /// <param name="factoryroom">工厂车间名称。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。【B.PARENT_KEY（工厂主键）,B.PARENT_NAME（工厂名称）,C.ORDER_NUMBER（工单号）】
        /// </returns>
        public DataSet GetWorkOrderByFactoryRoom(string factoryroom)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT DISTINCT B.PARENT_KEY,B.PARENT_NAME,C.ORDER_NUMBER
                                                FROM V_LOCATION_RET A
                                                LEFT JOIN V_LOCATION_RET B ON B.LOCATION_KEY=A.PARENT_KEY
                                                LEFT JOIN POR_WORK_ORDER C ON C.FACTORY_NAME = B.PARENT_NAME
                                                WHERE A.LOCATION_LEVEL = 5 AND C.ORDER_STATE ='REL' AND A.LOCATION_NAME ='{0}'",
                                                factoryroom.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlString);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetWorkOrderByFactoryRoom Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据车间主键获取工单号。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。
        /// 【ORDER_NUMBER（工单号）,产品ID号，成品编码，成品主键】
        /// </returns>
        public DataSet GetWorkOrderByFactoryRoomKey(string roomKey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT a.WORK_ORDER_KEY,a.ORDER_NUMBER,a.PRO_ID,a.PART_NUMBER,b.PART_KEY
                                                FROM POR_WORK_ORDER a
                                                LEFT JOIN POR_PART b ON a.PART_NUMBER=b.PART_ID
                                                WHERE EXISTS (SELECT b.FACTORY_KEY 
                                                            FROM   V_LOCATION b 
                                                            WHERE  b.FACTORY_NAME=a.FACTORY_NAME
                                                            AND    b.ROOM_KEY='{0}')
                                                AND a.ORDER_STATE ='REL'
                                                UNION ALL
                                                SELECT a.WORK_ORDER_KEY,a.ORDER_NUMBER,a.PRO_ID,a.PART_NUMBER,b.PART_KEY
                                                FROM POR_WORK_ORDER a
                                                LEFT JOIN POR_PART b ON a.PART_NUMBER=b.PART_ID
                                                WHERE EXISTS (SELECT b.FACTORY_KEY 
                                                            FROM   V_LOCATION b 
                                                            WHERE  b.FACTORY_NAME=a.FACTORY_NAME
                                                            AND    b.ROOM_KEY='{0}')
                                                AND a.ORDER_STATE ='TECO'
                                                AND a.CREATE_TIME>='{1}'",
                                                roomKey.PreventSQLInjection(),
                                                DateTime.Now.ToString("yyyy-MM-01"));
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlString);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetWorkOrderByFactoryRoomKey Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号获取成品料号信息。
        /// </summary>
        /// <param name="orderNum">工单号。</param>
        /// <returns>包含成品数据的数据集[PART_NAME, PART_DESC,PART_TYPE]</returns>
        public DataSet GetPartBytWorkOrder(string orderNum)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@"SELECT B.PART_NAME, B.PART_DESC,B.PART_TYPE
                                                FROM POR_WORK_ORDER A, POR_PART B
                                                WHERE A.PART_NUMBER = B.PART_NAME
                                                AND A.ORDER_NUMBER ='{0}'",
                                                orderNum.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlString);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetPart Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 工单清单。
        /// </summary>
        /// <param name="pconfig">分页设置对象。</param>
        /// <returns>包含工单数据的数据集。</returns>
        public DataSet GetWorkOrderInfoList(ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT ORDER_NUMBER AS WORKORDERNO, ORDER_STATE AS STATUS,DESCRIPTIONS AS WORKORDERDES, QUANTITY_ORDERED AS QTY,
                                    START_TIME AS STARTDATE, FINISH_TIME AS ENDDATE, PART_NUMBER AS PARTID,
                                    ORDER_TYPE AS WORKORDERTYPE, STOCK_LOCATION AS STORE,
                                    FACTORY_NAME FACTORY
                              FROM POR_WORK_ORDER";
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sql, pconfig.PageNo, pconfig.PageSize, out pages,
                    out records, db, dsReturn, "POR_WORK_ORDER");
                pconfig.Pages = pages;
                pconfig.Records = records;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderInfoList Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据条件获取工单数据。
        /// </summary>
        /// <param name="strFactory">工厂名称。</param>
        /// <param name="strWorkOrderNo">工单号。左匹配。</param>
        /// <param name="strPart">成品料号。左匹配。</param>
        /// <param name="strType">成品类型。</param>
        /// <param name="strStore">入库库位。</param>
        /// <param name="strStatus">状态。</param>
        /// <param name="pconfig">分页设置对象。</param>
        /// <returns>包含工单数据的数据集。</returns>
        public DataSet GetWorkOrderByCondition(string strFactory, string strWorkOrderNo,
                                               string strPart, string strType,
                                               string strStore, string strStatus,
                                               ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            string strCondition = string.Empty;
            //工厂
            if (strFactory != string.Empty)
            {
                strCondition = " FACTORY_NAME='" + strFactory.PreventSQLInjection() + "' AND ";
            }
            //工单
            if (strWorkOrderNo != string.Empty)
            {
                strCondition = " ORDER_NUMBER LIKE '" + strWorkOrderNo.PreventSQLInjection() + "%' AND ";
            }
            //成品料号
            if (strPart != string.Empty)
            {
                strCondition = " PART_NUMBER LIKE '" + strPart.PreventSQLInjection() + "%' AND ";
            }
            if (strType != string.Empty)
            {
                strCondition = " ORDER_TYPE='" + strType.PreventSQLInjection() + "' AND ";
            }
            //入库库位
            if (strStore != string.Empty)
            {
                strCondition = " STOCK_LOCATION LIKE '" + strStore.PreventSQLInjection() + "%' AND ";
            }
            if (strStatus != string.Empty)
            {
                strCondition = " ORDER_STATE='" + strStatus.PreventSQLInjection() + "' AND ";
            }
            try
            {
                string sqlString = string.Format(@"SELECT ORDER_NUMBER AS WORKORDERNO, ORDER_STATE AS STATUS,
                                                       DESCRIPTIONS AS WORKORDERDES, QUANTITY_ORDERED AS QTY,
                                                       START_TIME AS STARTDATE, FINISH_TIME AS ENDDATE, PART_NUMBER AS PARTID,
                                                       ORDER_TYPE AS WORKORDERTYPE, STOCK_LOCATION AS STORE,
                                                       FACTORY_NAME FACTORY
                                                FROM POR_WORK_ORDER  
                                                WHERE {0} 1=1 ", strCondition);
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sqlString, pconfig.PageNo, pconfig.PageSize, out pages,
                    out records, db, dsReturn, "POR_WORK_ORDER", "WORKORDERNO");
                pconfig.Pages = pages;
                pconfig.Records = records;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetWorkOrderByCondition Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取指定工步采集参数对应的工单参数设定数据。用于比对在采集时输入的数据是否符合工单参数设定。
        /// </summary>
        /// <param name="workorderKey">工单主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dcType">数据采集时刻。0:进站时采集 1：出站时采集</param>
        /// <returns>包含工单参数设定数据的数据集对象。</returns>
        public DataSet GetWorkOrderParam(string workorderKey, string stepKey, int dcType)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sql = string.Format(@"SELECT WORK_ORDER_KEY,ATTRIBUTE_KEY,ATTRIBUTE_NAME,ATTRIBUTE_VALUE,ATTRIBUTE_TYPE
                                            FROM POR_WORK_ORDER_ATTR W
                                            WHERE EXISTS (SELECT 1
		                                                  FROM POR_ROUTE_STEP_PARAM A
		                                                  WHERE A.ROUTE_STEP_KEY = '{0}'
		                                                  AND A.IS_DELETED=0
		                                                  AND A.DC_TYPE={1}
		                                                  AND A.PARAM_KEY=W.ATTRIBUTE_KEY)
                                            AND W.WORK_ORDER_KEY='{2}'
                                            AND W.ISFLAG=1
                                            AND W.ATTRIBUTE_TYPE=1",//1:表示工单物料控制数据。 0：表示工单自定义属性数据
                                            stepKey.PreventSQLInjection(),
                                            dcType,
                                            workorderKey.PreventSQLInjection());


                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderParam Error: " + ex.Message);
            }
            return dsReturn;
        }
        //--------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获得工单与Pro_id信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetWorkOrderByNoOrProid(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"select T.WORK_ORDER_KEY, T.ORDER_NUMBER,T1.PRODUCT_CODE,T.START_TIME CREATE_TIME
                                from POR_WORK_ORDER T LEFT JOIN POR_PRODUCT T1 ON T.PRO_ID=T1.PRODUCT_KEY
                                 WHERE  1=1 ";
                if (hstable.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                    sqlCommon += string.Format(" AND " + POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + " like '{0}%'", hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER].ToString());
                if (hstable.ContainsKey(POR_PRODUCT.FIELDS_PRODUCT_CODE))
                    sqlCommon += string.Format(" AND " + POR_PRODUCT.FIELDS_PRODUCT_CODE + " like '{0}%'", hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString());

                DataTable dtReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtReturn.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtReturn, true, MissingSchemaAction.Add);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderByNoOrProid Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单获得产品ID号，属性设置，及控制参数信息
        /// </summary>
        /// <param name="workorderkey"></param>
        /// <returns></returns>
        public DataSet GetWorkOrderAndAttrParamByKey(string workorderkey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"select T.WORK_ORDER_KEY, T.ORDER_NUMBER,T1.PRODUCT_CODE,T.CREATE_TIME,T1.PRODUCT_KEY PRO_ID,T.COMMENTS 
                                from POR_WORK_ORDER T LEFT JOIN POR_PRODUCT T1 ON T.PRO_ID=T1.PRODUCT_KEY
                                 WHERE T.WORK_ORDER_KEY='{0}' ", workorderkey);

                DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWorkOrder.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtWorkOrder, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"select T.WORK_ORDER_KEY,T.ATTRIBUTE_KEY,T.ATTRIBUTE_NAME,T.ATTRIBUTE_VALUE,T.ATTRIBUTE_TYPE,
                                                   T.EDITOR,T.EDIT_TIME,T.EDIT_TIMEZONE,t.WORK_ORDER_ATTR_KEY
                                            from POR_WORK_ORDER_ATTR T 
                                            LEFT JOIN BASE_ATTRIBUTE T1 ON T.ATTRIBUTE_KEY=T1.ATTRIBUTE_KEY
                                            where T.ATTRIBUTE_TYPE='0' AND T.WORK_ORDER_KEY='{0}' and t.ISFLAG=1
                                            union all
                                            SELECT T.WORK_ORDER_KEY,T.ATTRIBUTE_KEY,T.ATTRIBUTE_NAME,T.ATTRIBUTE_VALUE,T.ATTRIBUTE_TYPE,
                                                   T.EDITOR,T.EDIT_TIME,T.EDIT_TIMEZONE,t.WORK_ORDER_ATTR_KEY
                                            FROM POR_WORK_ORDER_ATTR T 
                                            LEFT JOIN BASE_PARAMETER T1 ON T.ATTRIBUTE_KEY=T1.PARAM_KEY
                                            WHERE T.ATTRIBUTE_TYPE='1' AND T1.PARAM_CATEGORY='5' AND T.WORK_ORDER_KEY='{0}' and t.ISFLAG=1", workorderkey);

                DataTable dtAttribute = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtAttribute.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtAttribute, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderAndAttrParamByKey Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获得所有工单信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllWorkOrderData()
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@" select t.WORK_ORDER_KEY,t.ORDER_NUMBER,t.DESCRIPTIONS,t.ORDER_STATE,t.ORDER_PRIORITY,
                                                     t.QUANTITY_ORDERED,t.START_TIME,t.FINISHED_TIME,t.PART_NUMBER,t.PART_REVISION,t.CREATOR,t.CREATE_TIME,
                                                     t.QUANTITY_LEFT,t.MODULE,t.REVENUE_TYPE,t.LINE_NAME,t.ORDER_TYPE,t.FACTORY_NAME,t.PRO_ID,
                                                     t1.PRODUCT_CODE
                                                      from POR_WORK_ORDER t left join POR_PRODUCT t1 on t.PRO_ID=t1.PRODUCT_KEY 
                                                     order by t.START_TIME desc");

                DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWorkOrder.TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtWorkOrder, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAllWorkOrderData Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取产品ID号数据。
        /// </summary>
        /// <returns>包含产品ID号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT PRODUCT_KEY,PRODUCT_CODE,PRODUCT_NAME
                                                    FROM POR_PRODUCT
                                                    WHERE ISFLAG=1
                                                    ORDER BY PRODUCT_CODE");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("WorkOrdersEngine GetProdId Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取产品料号数据。
        /// </summary>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public DataSet GetPartNumber()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT PART_KEY,PART_ID,PART_DESC
                                                    FROM POR_PART");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("WorkOrdersEngine GetPartNumber Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号获取工单信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单信息和工单物料的数据集对象。</returns>
        public DataSet GetWorkorderInfo(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT WORK_ORDER_KEY,ORDER_NUMBER,ORDER_STATE,ORDER_PRIORITY,DESCRIPTIONS,COMMENTS,ENTERED_TIME,PROMISED_TIME,
                                                    FINISHED_TIME,SHIPPED_TIME,CLOSED_TIME,QUANTITY_ORDERED,QUANTITY_IN_PROGRESS,QUANTITY_SCRAPPED,QUANTITY_FINISHED,
                                                    QUANTITY_SHIPPED,QUANTITY_CLOSED,START_TIME,FINISH_TIME,PLANNED_ROUTE_EP_VER_KEY,PLANNED_START_TIME,PLANNED_FINISH_TIME,
                                                    SCHEDULE_START_TIME,SCHEDULE_FINISH_TIME,PART_NUMBER,PART_REVISION,ORDER_CLOSE_TYPE,CREATOR,CREATE_TIME,CREATE_TIMEZONE,
                                                    EDITOR,EDIT_TIME,EDIT_TIMEZONE,NEXT_SEQ,QUANTITY_LEFT,TYPE,MODULE,SUPPLIER,REVENUE_TYPE,PART_KEY,REC_ORDER_NUMBER,
                                                    QUANTITY_REWORK,LINE_NAME,QUANTITY_LOSS,CLOSED_EDITOR,ORDER_TYPE,STOCK_LOCATION,FACTORY_NAME,PRO_ID
                                            FROM POR_WORK_ORDER
                                            WHERE ORDER_NUMBER='{0}'",
                                            orderNumber.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dsReturn.Tables[0].TableName = POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME;

                sql = string.Format(@"SELECT ORDER_NUMBER,SEQ_NO,ITEM_NO,MATERIAL_CODE,DESCRIPTION,REQ_QTY,MATERIAL_UNIT,WORK_CENTER,STORE_LOC,MATKL
                                    FROM POR_WORK_ORDER_BOM
                                    WHERE ORDER_NUMBER='{0}'",
                                    orderNumber.PreventSQLInjection());
                DataTable dtOrderBom = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtOrderBom.TableName = POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtOrderBom, true, MissingSchemaAction.Add);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkorderInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取物料编码数据。
        /// </summary>
        /// <returns>包含物料编码数据的数据集对象。</returns>
        public DataSet GetMaterialCode()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT MATERIAL_CODE,MATERIAL_NAME,UNIT 
                                                    FROM POR_MATERIAL");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("WorkOrdersEngine GetMaterialCode Error:{0}", ex.Message));
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存工单数据。
        /// </summary>
        /// <param name="dsParam">包含工单信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet Save(DataSet dsParam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (!dsParam.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME)
                    || dsParam.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count <= 0)
                {
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "数据集中不包含工单数据，请确认。");
                }
                DataTable dtOrder = dsParam.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtOrderBom = null;
                if (dsParam.Tables.Contains(POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME))
                {
                    dtOrderBom = dsParam.Tables[POR_WORK_ORDER_BOM_FIELDS.DATABASE_TABLE_NAME];
                }

                using (DbConnection dbConn = db.CreateConnection())
                {
                    dbConn.Open();
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    try
                    {
                        POR_WORK_ORDER_FIELDS orderFields = new POR_WORK_ORDER_FIELDS();
                        DataRow drOrder = dtOrder.Rows[0];
                        string orderNumber = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
                        string workOrderKey = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]);
                        string partNumber = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER]);
                        string partDescription = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_DESCRIPTIONS]);
                        string creator = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_CREATOR]);
                        string timezone = Convert.ToString(drOrder[POR_WORK_ORDER_FIELDS.FIELD_CREATE_TIMEZONE]);
                        string sql = string.Empty;
                        //查询料号是否存在，如果不存在，则增加产品料号数据
                        sql = string.Format(@"IF NOT EXISTS(SELECT 1 FROM POR_PART WHERE PART_ID='{0}')
                                            BEGIN
                                                INSERT INTO POR_PART(PART_KEY,PART_ID,PART_VERSION,PART_DESC,
                                                                    CREATOR,CREATE_TIME,CREATE_TIMEZONE,
                                                                    EDITOR,EDIT_TIME,EDIT_TIMEZONE,
                                                                    PART_NAME,PART_STATUS)
                                                VALUES(CONVERT(VARCHAR(64),NEWID()),'{0}',1,'{1}',
                                                       '{2}',GETDATE(),'{3}',
                                                       '{2}',GETDATE(),'{3}',
                                                       '{0}',1)
                                            END",
                                            partNumber.PreventSQLInjection(),
                                            partDescription.PreventSQLInjection(),
                                            creator.PreventSQLInjection(),
                                            timezone.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        //新增工单数据
                        if (string.IsNullOrEmpty(workOrderKey))
                        {
                            drOrder[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY] = CommonUtils.GenerateNewKey(0);
                            sql = DatabaseTable.BuildInsertSqlStatement(orderFields, drOrder, null);
                        }
                        else
                        {
                            //更新工单数据
                            Hashtable htOrder = CommonUtils.ConvertRowToHashtable(drOrder);
                            WhereConditions where = new WhereConditions(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                            sql = DatabaseTable.BuildUpdateSqlStatement(orderFields, htOrder, where);
                        }
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);


                        POR_WORK_ORDER_BOM_FIELDS orderBomFields = new POR_WORK_ORDER_BOM_FIELDS();
                        //新增或者更新工单BOM数据。
                        foreach (DataRow drOrderBom in dtOrderBom.Rows)
                        {
                            string bomOrderNumber = Convert.ToString(drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER]);
                            string seqNo = Convert.ToString(drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_SEQ_NO]);
                            string materialCode = Convert.ToString(drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_CODE]);
                            string materialDescription = Convert.ToString(drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_DESCRIPTION]);
                            string materialUnit = Convert.ToString(drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_MATERIAL_UNIT]);
                            //查询物料编码是否存在，如果不存在，则增加物料编码数据
                            sql = string.Format(@"IF NOT EXISTS(SELECT 1 FROM POR_MATERIAL WHERE MATERIAL_CODE='{0}')
                                                BEGIN
                                                    INSERT INTO POR_MATERIAL
                                                    (MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,MATERIAL_VERSION,MATERIAL_SPEC,UNIT,
                                                     CREATOR,CREATE_TIME,CREATE_TIMEZONE,
                                                     EDITOR,EDIT_TIME,EDIT_TIMEZONE,STATUS)
                                                    VALUES(CONVERT(VARCHAR(64),NEWID()),'{1}','{0}',1,'{1}','{2}',
                                                       '{3}',GETDATE(),'{4}',
                                                       '{3}',GETDATE(),'{4}',1)
                                                END",
                                                materialCode.PreventSQLInjection(),
                                                materialDescription.PreventSQLInjection(),
                                                materialUnit.PreventSQLInjection(),
                                                creator.PreventSQLInjection(),
                                                timezone.PreventSQLInjection());
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            //新增工单BOM数据
                            if (string.IsNullOrEmpty(bomOrderNumber))
                            {
                                drOrderBom[POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER] = orderNumber;
                                sql = DatabaseTable.BuildInsertSqlStatement(orderBomFields, drOrderBom, null);
                            }
                            else
                            {
                                //更新工单BOM数据。
                                Hashtable htOrderBom = CommonUtils.ConvertRowToHashtable(drOrderBom);
                                Conditions cons = new Conditions();
                                cons.Add(DatabaseLogicOperator.And, POR_WORK_ORDER_BOM_FIELDS.FIELD_ORDER_NUMBER, DatabaseCompareOperator.Equal, bomOrderNumber);
                                cons.Add(DatabaseLogicOperator.And, POR_WORK_ORDER_BOM_FIELDS.FIELD_SEQ_NO, DatabaseCompareOperator.Equal, seqNo);
                                sql = DatabaseTable.BuildUpdateSqlStatement(orderBomFields, htOrderBom, cons);
                            }
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        dbConn.Close();
                        dbTran = null;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("WorkOrdersEngine Save Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsWorkAttrParam"></param>
        /// <returns></returns>
        public DataSet SaveWorkOrderAttrParam(DataSet dsWorkAttrParam)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtWorkOrder_Update = null, dtWorkOrder_Insert = null;
            DataTable dtWorkAttr_Update = null, dtWorkAttr_Insert = null;

            List<string> sqlCommandList = new List<string>();
            if (dsWorkAttrParam.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORINSERT))
                dtWorkOrder_Insert = dsWorkAttrParam.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORINSERT];
            if (dsWorkAttrParam.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORUPDATE))
                dtWorkOrder_Update = dsWorkAttrParam.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_FORUPDATE];

            if (dsWorkAttrParam.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT))
                dtWorkAttr_Insert = dsWorkAttrParam.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT];
            if (dsWorkAttrParam.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE))
                dtWorkAttr_Update = dsWorkAttrParam.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE];

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                POR_WORK_ORDER_FIELDS workOrder = new POR_WORK_ORDER_FIELDS();
                POR_WORK_ORDER_ATTR_FIELDS workAttr = new POR_WORK_ORDER_ATTR_FIELDS();
                try
                {
                    if (dtWorkOrder_Insert != null && dtWorkOrder_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrder_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrder, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrder_Update != null && dtWorkOrder_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrder_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            WhereConditions wc = new WhereConditions(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY, hashTable[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY].ToString());
                            hashTable.Remove(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY);

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(workOrder, hashTable, wc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    if (dtWorkAttr_Insert != null && dtWorkAttr_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkAttr_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workAttr, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkAttr_Update != null && dtWorkAttr_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkAttr_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            List<WhereConditions> lwc = new List<WhereConditions>();
                            WhereConditions wc01 = new WhereConditions(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY, hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY].ToString());
                            lwc.Add(wc01);

                            sqlCommand = string.Format(@"insert into [POR_WORK_ORDER_ATTR]
                                                        ( [WORK_ORDER_KEY],[ATTRIBUTE_KEY],[ATTRIBUTE_NAME],[ATTRIBUTE_VALUE]
                                                        ,[ATTRIBUTE_TYPE],[EDITOR],[EDIT_TIME],[EDIT_TIMEZONE]
                                                        ,[WORK_ORDER_ATTR_KEY],[PRO_ID],[ISFLAG])
                                                        SELECT [WORK_ORDER_KEY],[ATTRIBUTE_KEY],[ATTRIBUTE_NAME],[ATTRIBUTE_VALUE]
                                                        ,[ATTRIBUTE_TYPE],'{0}',[EDIT_TIME],[EDIT_TIMEZONE]
                                                        ,NEWID(),[PRO_ID],0
                                                        FROM [POR_WORK_ORDER_ATTR]
                                                        where [WORK_ORDER_ATTR_KEY]='{1}'"
                                                                , Convert.ToString(hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR]),
                                                                Convert.ToString(hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY]));
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            hashTable.Remove(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY);
                            hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(workAttr, hashTable, lwc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveWorkOrderAttrParam Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }
        /// <summary>
        /// 删除工单属性设置
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet DelWorkAttrDataBy2Key(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommandList = new List<string>();

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                POR_WORK_ORDER_ATTR_FIELDS workAttr = new POR_WORK_ORDER_ATTR_FIELDS();
                Conditions conditions = new Conditions();
                try
                {
                    //conditions.Add(DatabaseLogicOperator.And, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY, DatabaseCompareOperator.Equal, hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString());
                    //conditions.Add(DatabaseLogicOperator.And, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY, DatabaseCompareOperator.Equal, hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_KEY].ToString());
                    conditions.Add(DatabaseLogicOperator.And, POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY, DatabaseCompareOperator.Equal, hstable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY].ToString());
                    if (!hstable.Contains(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG))
                        hstable.Add(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ISFLAG, 0);
                    sqlCommand = DatabaseTable.BuildUpdateSqlStatement(workAttr, hstable, conditions);

                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                    //Commit Transaction
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("DelWorkAttrDataBy2Key Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }
        /// <summary>
        /// 根据工单属性名称，获取工单属性相应的属性信息
        /// </summary>
        /// <param name="workorder">工单号/工单key</param>
        /// <param name="attributeName">工单设置属性名称</param>
        /// <returns>返回工单设置相应的属性信息</returns>
        public DataSet GetWorkOrderAttributeValue(Hashtable hsparams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = @"select t1.* from POR_WORK_ORDER t inner join POR_WORK_ORDER_ATTR t1 on t.WORK_ORDER_KEY=t1.WORK_ORDER_KEY where t1.ISFLAG=1  ";
                if (hsparams.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY))
                    sqlCommon += string.Format(@" and t.WORK_ORDER_KEY='{0}'", Convert.ToString(hsparams[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]));
                if (hsparams.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                    sqlCommon += string.Format(@" and t.ORDER_NUMBER='{0}'", Convert.ToString(hsparams[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]));
                if (hsparams.ContainsKey(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME))
                    sqlCommon += string.Format(@" and t1.ATTRIBUTE_NAME='{0}'", Convert.ToString(hsparams[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME]));

                DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWorkOrder.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtWorkOrder, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderAttributeValue Error: " + ex.Message);
            }

            return dsReturn;
        }

        public DataSet GetViewForWorkOrder(Hashtable hsparams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = @"select * from V_WORK_ORDER_ATTR t where 1=1  ";
                if (hsparams.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY))
                    sqlCommon += string.Format(@" and t.WORK_ORDER_KEY='{0}'", Convert.ToString(hsparams[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY]));
                if (hsparams.ContainsKey(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER))
                    sqlCommon += string.Format(@" and t.ORDER_NUMBER='{0}'", Convert.ToString(hsparams[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]));
                if (hsparams.ContainsKey("Customer"))
                    sqlCommon += string.Format(@" and t.Customer='{0}'", Convert.ToString(hsparams["Customer"]));

                DataTable dtWorkOrder = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWorkOrder.TableName = "V_WORK_ORDER_ATTR";
                dsReturn.Merge(dtWorkOrder, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetViewForWorkOrder Error: " + ex.Message);
            }

            return dsReturn;
        }


        /// <summary>
        /// 通过工单号获取对应的工单属性信息
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns></returns>
        public DataSet GetWorkOrderAttrParamByOrderNumber(string workorderNumber)
        {

            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"select T.WORK_ORDER_KEY,T.ATTRIBUTE_KEY,T.ATTRIBUTE_NAME,T.ATTRIBUTE_VALUE,T.ATTRIBUTE_TYPE,
                                                    T.EDITOR,T.EDIT_TIME,T.EDIT_TIMEZONE,t.WORK_ORDER_ATTR_KEY
                                                    from POR_WORK_ORDER_ATTR T 
                                                    LEFT JOIN BASE_ATTRIBUTE T1 ON T.ATTRIBUTE_KEY=T1.ATTRIBUTE_KEY
                                                    where T.ATTRIBUTE_TYPE='0' AND T.WORK_ORDER_KEY=(SELECT WORK_ORDER_KEY FROM POR_WORK_ORDER WHERE ORDER_NUMBER = '{0}') and t.ISFLAG=1
                                                    union all
                                                    SELECT T.WORK_ORDER_KEY,T.ATTRIBUTE_KEY,T.ATTRIBUTE_NAME,T.ATTRIBUTE_VALUE,T.ATTRIBUTE_TYPE,
                                                    T.EDITOR,T.EDIT_TIME,T.EDIT_TIMEZONE,t.WORK_ORDER_ATTR_KEY
                                                    FROM POR_WORK_ORDER_ATTR T 
                                                    LEFT JOIN BASE_PARAMETER T1 ON T.ATTRIBUTE_KEY=T1.PARAM_KEY
                                                    WHERE T.ATTRIBUTE_TYPE='1' AND T1.PARAM_CATEGORY='5' 
                                                    AND T.WORK_ORDER_KEY=(SELECT WORK_ORDER_KEY FROM POR_WORK_ORDER WHERE ORDER_NUMBER = '{0}') and t.ISFLAG=1", workorderNumber);

                DataTable dtAttribute = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtAttribute.TableName = POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtAttribute, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderAndAttrParamByKey Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单主键获取工单产品信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>工单对应的工单产品的表集信息</returns>
        public DataSet GetWorkOrderProByOrderKey(string workOrderKey)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;

            try
            {
                sqlCommon = string.Format(@"SELECT *,
		                                            (SELECT TOP 1 B.PART_DESC FROM POR_PART B
			                                            WHERE B.PART_STATUS = 1
			                                            AND B.PART_ID = A.PART_NUMBER
		                                            ) PART_DESC
                                            FROM POR_WO_PRD A
                                            WHERE A.IS_USED = 'Y' 
                                            AND A.WORK_ORDER_KEY = '{0}' 
                                            ORDER BY A.ITEM_NO ASC ", workOrderKey);

                DataTable dtWOPro = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOPro.TableName = "POR_WO_PRD";
                dsReturn.Merge(dtWOPro, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT *,DECAY_KEY AS 'DECAY_NEWKEY' FROM POR_WO_PRD_DECAY WHERE IS_USED = 'Y' AND WORK_ORDER_KEY='{0}'", workOrderKey);

                DataTable dtWOProDecay = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProDecay.TableName = "POR_WO_PRD_DECAY";
                dsReturn.Merge(dtWOProDecay, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_LEVEL WHERE IS_USED = 'Y' AND WORK_ORDER_KEY ='{0}'", workOrderKey);

                DataTable dtWOProLevel = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProLevel.TableName = "POR_WO_PRD_LEVEL";
                dsReturn.Merge(dtWOProLevel, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_PRINTSET  WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOProPrintSet = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProPrintSet.TableName = "POR_WO_PRD_PRINTSET";
                dsReturn.Merge(dtWOProPrintSet, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_PS WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOProPS = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProPS.TableName = "POR_WO_PRD_PS";
                dsReturn.Merge(dtWOProPS, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_PS_CLR WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOProPSCLR = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProPSCLR.TableName = "POR_WO_PRD_PS_CLR";
                dsReturn.Merge(dtWOProPSCLR, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_PS_SUB WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOProPSSUB = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOProPSSUB.TableName = "POR_WO_PRD_PS_SUB";
                dsReturn.Merge(dtWOProPSSUB, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_OEM WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOOEMInfo = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOOEMInfo.TableName = "POR_WO_OEM";
                dsReturn.Merge(dtWOOEMInfo, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_ROUTE WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWORoute = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWORoute.TableName = "POR_WO_ROUTE";
                dsReturn.Merge(dtWORoute, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_LINE WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtWOLine = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtWOLine.TableName = "POR_WO_LINE";
                dsReturn.Merge(dtWOLine, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_UPLOWRULE WHERE  WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtUPLOWRule = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtUPLOWRule.TableName = "POR_WO_PRD_UPLOWRULE";
                dsReturn.Merge(dtUPLOWRule, true, MissingSchemaAction.Add);

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_PRD_POWERSHOW WHERE IS_USED = 'Y' AND WORK_ORDER_KEY = '{0}'", workOrderKey);

                DataTable dtPowerShow = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];//fyb
                dtPowerShow.TableName = "POR_WO_PRD_POWERSHOW";
                dsReturn.Merge(dtPowerShow, true, MissingSchemaAction.Add);

                //add by wubaofeng EL图片规则查询
                sqlCommon = string.Format(@"SELECT * FROM POR_WO_ELTESTRULE WHERE WORK_ORDER_KEY = '{0}'", workOrderKey);
                DataTable dtElPicRule = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtElPicRule.TableName = "POR_WO_ELTESTRULE";
                dsReturn.Merge(dtElPicRule, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderProByOrderKey Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单主料号获取对应的料号清单
        /// </summary>
        /// <param name="mainPartNumber">主产品料号</param>
        /// <returns>主料号清单对应的料号的集合</returns>
        public DataSet GetPartNumberByMainPartNumber(string mainPartNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT B.MAIN_PART_NUMBER PART_ID,A.PART_DESC,
		                                                    ISNULL(B.MAIN_PART_NUMBER,A.PART_ID) MAIN_PART_NUMBER,
		                                                    ISNULL(B.PART_NUMBER,A.PART_ID) PART_NUMBER,
		                                                    ISNULL(B.ITEM_NO,1) ITEM_NO,
		                                                    ISNULL(B.MIN_POWER,0) MIN_POWER,
		                                                    ISNULL(B.MAX_POWER,99999) MAX_POWER,
		                                                    B.GRADES
                                                    FROM POR_PART A
                                                    LEFT JOIN POR_PART_BYPRODUCT B ON A.PART_ID =B.PART_NUMBER AND B.IS_USED = 'Y'
                                                    WHERE b.MAIN_PART_NUMBER = '{0}'
                                                    AND LEN(ISNULL(B.PART_NUMBER,A.PART_ID))>0
                                                    ORDER BY ITEM_NO ASC", mainPartNumber);

                DataTable dtPartNumberArray = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtPartNumberArray.TableName = "POR_PART_BYPRODUCT";
                dsReturn.Merge(dtPartNumberArray, true, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPartNumberByMainPartNumber Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 对工单对应的产品信息进行保存
        /// </summary>
        /// <param name="workOrderProInfo">工单对应的产品信息的集合</param>
        /// <returns>操作结果</returns>
        public DataSet SaveWorkOrderProInfo(DataSet workOrderProInfo)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtProduct_Insert = null, dtProduct_Update = null, dtProduct_Delete = null;
            DataTable dtProPS_Insert = null, dtProPS_Update = null, dtProPS_Delete = null;
            DataTable dtProPShow_Insert = null, dtProPShow_Update = null, dtProPShow_Delete = null;//fyb
            DataTable dtProPSColor_Insert = null, dtProPSColor_Update = null, dtProPSColor_Delete = null;
            DataTable dtProPSSub_Insert = null, dtProPSSub_Update = null, dtProPSSub_Delete = null;
            DataTable dtProLevel_Insert = null, dtProLevel_Update = null, dtProLevel_Delete = null;
            DataTable dtProDecay_Insert = null, dtProDecay_Update = null, dtProDecay_Delete = null;
            DataTable dtProPrintSet_Insert = null, dtProPrintSet_Update = null, dtProPrintSet_Delete = null;
            DataTable dtWorkOrderOEM_Insert = null, dtWorkOrderOEM_Update = null, dtWorkOrderOEM_Delete = null;
            DataTable dtWorkOrderRoute_Insert = null, dtWorkOrderRoute_Update = null, dtWorkOrderRoute_Delete = null;
            DataTable dtWorkOrderLine_Insert = null, dtWorkOrderLine_Update = null, dtWorkOrderLine_Delete = null;
            //DataTable dtWorkOrderUPLOWRule_Insert = null, dtWorkOrderUPLOWRule_Update = null, dtWorkOrderUPLOWRule_Delete = null;
            DataTable dtWorkOrderUpLowRule = null, dtWorkOrderUpLowRuleKEY = null;
            DataTable dtWorkAttr_Update = null, dtWorkAttr_Insert = null;
            DataTable dtPorWorkCtm = null, dtPorWorkCtmWorkKey = null; DataTable dtWoPrintRule = null; DataTable dtWoPrintRuleDetail = null;
            DataTable dtPackPrint_Insert = null, dtPackPrint_Update = null;
            List<string> sqlCommandList = new List<string>();
            DataTable dtElPicRule = null;
            DataTable dtWoPrint = null;
            #region //获取更新的信息
            //产品的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_INSERT"))
                dtProduct_Insert = workOrderProInfo.Tables["POR_WO_PRD_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_UPDATE"))
                dtProduct_Update = workOrderProInfo.Tables["POR_WO_PRD_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_DELETE"))
                dtProduct_Delete = workOrderProInfo.Tables["POR_WO_PRD_DELETE"];

            //功率分档的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_INSERT"))
                dtProPS_Insert = workOrderProInfo.Tables["POR_WO_PRD_PS_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_UPDATE"))
                dtProPS_Update = workOrderProInfo.Tables["POR_WO_PRD_PS_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_DELETE"))
                dtProPS_Delete = workOrderProInfo.Tables["POR_WO_PRD_PS_DELETE"];

            //功率铭牌清单的更新信息  add by chao.pang 20170929   yibin.fei
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_POWERSHOW_INSERT"))
                dtProPShow_Insert = workOrderProInfo.Tables["POR_WO_PRD_POWERSHOW_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_POWERSHOW_UPDATE"))
                dtProPShow_Update = workOrderProInfo.Tables["POR_WO_PRD_POWERSHOW_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_POWERSHOW_DELETE"))
                dtProPShow_Delete = workOrderProInfo.Tables["POR_WO_PRD_POWERSHOW_DELETE"];

            //功率分档花色的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_CLR_INSERT"))
                dtProPSColor_Insert = workOrderProInfo.Tables["POR_WO_PRD_PS_CLR_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_CLR_UPDATE"))
                dtProPSColor_Update = workOrderProInfo.Tables["POR_WO_PRD_PS_CLR_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_CLR_DELETE"))
                dtProPSColor_Delete = workOrderProInfo.Tables["POR_WO_PRD_PS_CLR_DELETE"];

            //功率分档子分档的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_SUB_INSERT"))
                dtProPSSub_Insert = workOrderProInfo.Tables["POR_WO_PRD_PS_SUB_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_SUB_UPDATE"))
                dtProPSSub_Update = workOrderProInfo.Tables["POR_WO_PRD_PS_SUB_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PS_SUB_DELETE"))
                dtProPSSub_Delete = workOrderProInfo.Tables["POR_WO_PRD_PS_SUB_DELETE"];

            //产品等级的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_LEVEL_INSERT"))
                dtProLevel_Insert = workOrderProInfo.Tables["POR_WO_PRD_LEVEL_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_LEVEL_UPDATE"))
                dtProLevel_Update = workOrderProInfo.Tables["POR_WO_PRD_LEVEL_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_LEVEL_DELETE"))
                dtProLevel_Delete = workOrderProInfo.Tables["POR_WO_PRD_LEVEL_DELETE"];

            //衰减信息的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_DECAY_INSERT"))
                dtProDecay_Insert = workOrderProInfo.Tables["POR_WO_PRD_DECAY_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_DECAY_UPDATE"))
                dtProDecay_Update = workOrderProInfo.Tables["POR_WO_PRD_DECAY_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_DECAY_DELETE"))
                dtProDecay_Delete = workOrderProInfo.Tables["POR_WO_PRD_DECAY_DELETE"];

            //标签打印设置的更新信息
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PRINTSET_INSERT"))
                dtProPrintSet_Insert = workOrderProInfo.Tables["POR_WO_PRD_PRINTSET_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PRINTSET_UPDATE"))
                dtProPrintSet_Update = workOrderProInfo.Tables["POR_WO_PRD_PRINTSET_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PRINTSET_DELETE"))
                dtProPrintSet_Delete = workOrderProInfo.Tables["POR_WO_PRD_PRINTSET_DELETE"];

            //OEM信息的更新
            if (workOrderProInfo.Tables.Contains("POR_WO_OEM_INSERT"))
                dtWorkOrderOEM_Insert = workOrderProInfo.Tables["POR_WO_OEM_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_OEM_UPDATE"))
                dtWorkOrderOEM_Update = workOrderProInfo.Tables["POR_WO_OEM_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_OEM_DELETE"))
                dtWorkOrderOEM_Delete = workOrderProInfo.Tables["POR_WO_OEM_DELETE"];

            //工艺流程信息的更新
            if (workOrderProInfo.Tables.Contains("POR_WO_ROUTE_INSERT"))
                dtWorkOrderRoute_Insert = workOrderProInfo.Tables["POR_WO_ROUTE_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_ROUTE_UPDATE"))
                dtWorkOrderRoute_Update = workOrderProInfo.Tables["POR_WO_ROUTE_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_ROUTE_DELETE"))
                dtWorkOrderRoute_Delete = workOrderProInfo.Tables["POR_WO_ROUTE_DELETE"];

            //工单线别
            if (workOrderProInfo.Tables.Contains("POR_WO_LINE_INSERT"))
                dtWorkOrderLine_Insert = workOrderProInfo.Tables["POR_WO_LINE_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_LINE_UPDATE"))
                dtWorkOrderLine_Update = workOrderProInfo.Tables["POR_WO_LINE_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_LINE_DELETE"))
                dtWorkOrderLine_Delete = workOrderProInfo.Tables["POR_WO_LINE_DELETE"];

            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_UPLOWRULE"))
                dtWorkOrderUpLowRule = workOrderProInfo.Tables["POR_WO_PRD_UPLOWRULE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_UPLOWRULE_KEY"))
                dtWorkOrderUpLowRuleKEY = workOrderProInfo.Tables["POR_WO_PRD_UPLOWRULE_KEY"];

            if (workOrderProInfo.Tables.Contains("POR_WO_CTM"))
                dtPorWorkCtm = workOrderProInfo.Tables["POR_WO_CTM"];
            if (workOrderProInfo.Tables.Contains("PRO_WO"))
                dtPorWorkCtmWorkKey = workOrderProInfo.Tables["PRO_WO"];

            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PRINTRULE_DETAIL"))
                dtWoPrintRuleDetail = workOrderProInfo.Tables["POR_WO_PRD_PRINTRULE_DETAIL"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRD_PRINTRULE"))
                dtWoPrintRule = workOrderProInfo.Tables["POR_WO_PRD_PRINTRULE"];

            if (workOrderProInfo.Tables.Contains("POR_WO_ELTESTRULE"))
                dtElPicRule = workOrderProInfo.Tables["POR_WO_ELTESTRULE"];
            #endregion

            if (workOrderProInfo.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT))
                dtWorkAttr_Insert = workOrderProInfo.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORINSERT];
            if (workOrderProInfo.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE))
                dtWorkAttr_Update = workOrderProInfo.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_FORUPDATE];

            if (workOrderProInfo.Tables.Contains("POR_WO_PLASH_AUTOPRINT_INSERT"))
                dtPackPrint_Insert = workOrderProInfo.Tables["POR_WO_PLASH_AUTOPRINT_INSERT"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PLASH_AUTOPRINT_UPDATE"))
                dtPackPrint_Update = workOrderProInfo.Tables["POR_WO_PLASH_AUTOPRINT_UPDATE"];
            if (workOrderProInfo.Tables.Contains("POR_WO_PRINT"))
            {
                dtWoPrint = workOrderProInfo.Tables["POR_WO_PRINT"];
            }

            using (DbConnection dbConn = db.CreateConnection())
            {
                //Open Connection
                dbConn.Open();
                //Create Transaction
                DbTransaction dbTran = dbConn.BeginTransaction();
                string sqlCommand = string.Empty;
                POR_WO_PRD workOrderPro = new POR_WO_PRD();
                POR_WO_PRD_DECAY workOrderProDecay = new POR_WO_PRD_DECAY();
                POR_WO_PRD_LEVEL workOrderProLevel = new POR_WO_PRD_LEVEL();
                POR_WO_PRD_PRINTSET workOrderProPrint = new POR_WO_PRD_PRINTSET();
                POR_WO_PRD_PS workOrderProPowerSet = new POR_WO_PRD_PS();
                POR_WO_PRD_PSHOW workOrderProPowerShow = new POR_WO_PRD_PSHOW(); //fyb
                POR_WO_PRD_PS_CLR workOrderProPSColor = new POR_WO_PRD_PS_CLR();
                POR_WO_PRD_PS_SUB workOrderProPSSub = new POR_WO_PRD_PS_SUB();
                POR_WO_OEM workOrderOEM = new POR_WO_OEM();
                POR_WO_ROUTE workOrderRoute = new POR_WO_ROUTE();
                POR_WO_LINE workOrderLine = new POR_WO_LINE();

                POR_WORK_ORDER_FIELDS workOrder = new POR_WORK_ORDER_FIELDS();
                POR_WORK_ORDER_ATTR_FIELDS workAttr = new POR_WORK_ORDER_ATTR_FIELDS();

                POR_WO_FLASH_AUTOPRINT workFlashAutoPrint = new POR_WO_FLASH_AUTOPRINT();
                try
                {
                    #region //产品信息保存
                    if (dtProduct_Insert != null && dtProduct_Insert.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtProduct_Insert.Rows.Count; i++)
                        {
                            string isMain = dtProduct_Insert.Rows[i]["IS_MAIN"].ToString();
                            if (isMain == "Y")
                            {
                                sqlCommand = string.Format(@"UPDATE {0}
                                                             SET PRO_ID = '{1}'
                                                             WHERE WORK_ORDER_KEY = '{2}'",
                                                                                            workOrder.TABLE_NAME,
                                                                                            dtProduct_Insert.Rows[i]["PRODUCT_KEY"],
                                                                                            dtProduct_Insert.Rows[i]["WORK_ORDER_KEY"]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                break;
                            }
                        }
                        foreach (DataRow dr in dtProduct_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM POR_WO_PRD
                                                         WHERE WORK_ORDER_KEY = '{0}'
                                                         AND PART_NUMBER = '{1}'", dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderPro, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProduct_Update != null && dtProduct_Update.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtProduct_Update.Rows.Count; i++)
                        {
                            string isMain = dtProduct_Update.Rows[i]["IS_MAIN"].ToString();
                            if (isMain == "Y")
                            {
                                sqlCommand = string.Format(@"UPDATE {0}
                                                             SET PRO_ID = '{1}'
                                                             WHERE WORK_ORDER_KEY = '{2}'",
                                                                                            workOrder.TABLE_NAME,
                                                                                            dtProduct_Update.Rows[i]["PRODUCT_KEY"],
                                                                                            dtProduct_Update.Rows[i]["WORK_ORDER_KEY"]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                                break;
                            }
                        }
                        foreach (DataRow dr in dtProduct_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',EDIT_TIME = SYSDATETIME(),IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND VERSION_NO = '{4}'", workOrderPro.TABLE_NAME,
                                                                                  dr["EDITOR"],
                                                                                  dr["WORK_ORDER_KEY"],
                                                                                  dr["PART_NUMBER"],
                                                                                  dr["VERSION_NO"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderPro, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProduct_Delete != null && dtProduct_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProduct_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',EDIT_TIME = SYSDATETIME(),IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND VERSION_NO = '{4}'", workOrderPro.TABLE_NAME,
                                                                                  dr["EDITOR"],
                                                                                  dr["WORK_ORDER_KEY"],
                                                                                  dr["PART_NUMBER"],
                                                                                  dr["VERSION_NO"]);


                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //功率分档信息保存
                    if (dtProPS_Insert != null && dtProPS_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPS_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND PART_NUMBER = '{2}'
                                                         AND POWERSET_KEY = '{3}'", workOrderProPowerSet.TABLE_NAME,
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPowerSet, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPS_Update != null && dtProPS_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPS_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProPowerSet.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["VERSION_NO"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPowerSet, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPS_Delete != null && dtProPS_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPS_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProPowerSet.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //功率分档花色信息保存
                    if (dtProPSColor_Insert != null && dtProPSColor_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSColor_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND PART_NUMBER = '{2}'
                                                         AND POWERSET_KEY = '{3}'
                                                         AND COLOR_CODE = '{4}' ", workOrderProPSColor.TABLE_NAME,
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["COLOR_CODE"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPSColor, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPSColor_Update != null && dtProPSColor_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSColor_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND COLOR_CODE = '{5}'
                                                         AND VERSION_NO = '{6}'", workOrderProPSColor.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["COLOR_CODE"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPSColor, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPSColor_Delete != null && dtProPSColor_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSColor_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND COLOR_CODE = '{5}'
                                                         AND VERSION_NO = '{6}'", workOrderProPSColor.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["COLOR_CODE"],
                                                                                   dr["VERSION_NO"]);


                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //功率分档子分档信息保存
                    if (dtProPSSub_Insert != null && dtProPSSub_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSSub_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND PART_NUMBER = '{2}'
                                                         AND POWERSET_KEY = '{3}'
                                                         AND PS_SUB_CODE = '{4}'", workOrderProPSSub.TABLE_NAME,
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["PS_SUB_CODE"]);

                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPSSub, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPSSub_Update != null && dtProPSSub_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSSub_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND PS_SUB_CODE = '{5}'
                                                         AND VERSION_NO = '{6}'", workOrderProPSSub.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["PS_SUB_CODE"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPSSub, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPSSub_Delete != null && dtProPSSub_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPSSub_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND POWERSET_KEY ='{4}'
                                                         AND PS_SUB_CODE = '{5}'
                                                         AND VERSION_NO = '{6}'", workOrderProPSSub.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["POWERSET_KEY"],
                                                                                   dr["PS_SUB_CODE"],
                                                                                   dr["VERSION_NO"]);


                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //产品等级信息保存
                    if (dtProLevel_Insert != null && dtProLevel_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProLevel_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND PART_NUMBER = '{2}'
                                                         AND GRADE = '{3}'", workOrderProLevel.TABLE_NAME,
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["GRADE"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProLevel, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProLevel_Update != null && dtProLevel_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProLevel_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND GRADE ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProLevel.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["GRADE"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProLevel, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProLevel_Delete != null && dtProLevel_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProLevel_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND GRADE ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProLevel.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["GRADE"],
                                                                                   dr["VERSION_NO"]);


                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //衰减信息保存
                    if (dtProDecay_Insert != null && dtProDecay_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProDecay_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProDecay, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProDecay_Update != null && dtProDecay_Update.Rows.Count > 0)
                    {
                        DataTable dtProDecay_Update_Copy = dtProDecay_Update.Copy();
                        dtProDecay_Update.Columns.Remove("DECAY_NEWKEY");
                        foreach (DataRow dr in dtProDecay_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND DECAY_KEY ='{4}'", workOrderProDecay.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["DECAY_KEY"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            for (int i = 0; i < dtProDecay_Update_Copy.Rows.Count; i++)
                            {
                                if (dr["DECAY_KEY"].ToString() == dtProDecay_Update_Copy.Rows[i]["DECAY_KEY"].ToString())
                                {
                                    dr["DECAY_KEY"] = dtProDecay_Update_Copy.Rows[i]["DECAY_NEWKEY"];
                                    break;
                                }
                            }


                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProDecay, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProDecay_Delete != null && dtProDecay_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProDecay_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND DECAY_KEY ='{4}'", workOrderProDecay.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["DECAY_KEY"]);


                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //标签打印设置信息保存
                    if (dtProPrintSet_Insert != null && dtProPrintSet_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPrintSet_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPrint, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPrintSet_Update != null && dtProPrintSet_Update.Rows.Count > 0)
                    {
                        DataTable dtProPrintSet_Update_Copy = dtProPrintSet_Update.Copy();
                        dtProPrintSet_Update.Columns.Remove("PRINTSET_NEWKEY");
                        foreach (DataRow dr in dtProPrintSet_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND PRINTSET_KEY ='{4}'", workOrderProPrint.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["PRINTSET_KEY"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            for (int i = 0; i < dtProPrintSet_Update_Copy.Rows.Count; i++)
                            {
                                if (dr["PRINTSET_KEY"].ToString() == dtProPrintSet_Update_Copy.Rows[i]["PRINTSET_KEY"].ToString())
                                {
                                    dr["PRINTSET_KEY"] = dtProPrintSet_Update_Copy.Rows[i]["PRINTSET_NEWKEY"];
                                    break;
                                }
                            }

                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPrint, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPrintSet_Delete != null && dtProPrintSet_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPrintSet_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND PRINTSET_KEY ='{4}'", workOrderProPrint.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["PRINTSET_KEY"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //工单OEM信息保存
                    if (dtWorkOrderOEM_Insert != null && dtWorkOrderOEM_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderOEM_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT CONVERT(INT, VERSION_NO) AS VERSION_NO FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'", workOrderOEM.TABLE_NAME,
                                                                                        dr["WORK_ORDER_KEY"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该工单对应的OEM的信息是否存在且被删除过，若存在在原有的基础上对版本号进行+1
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }
                            
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderOEM, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderOEM_Update != null && dtWorkOrderOEM_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderOEM_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", workOrderOEM.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["VERSION_NO"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderOEM, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderOEM_Delete != null && dtWorkOrderOEM_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderOEM_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", workOrderOEM.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //工单工艺流程信息保存
                    if (dtWorkOrderRoute_Insert != null && dtWorkOrderRoute_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderRoute_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'", workOrderRoute.TABLE_NAME,
                                                                                        dr["WORK_ORDER_KEY"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该工单对应的OEM的信息是否存在且被删除过，若存在在原有的基础上对版本号进行+1
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderRoute, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderRoute_Update != null && dtWorkOrderRoute_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderRoute_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", workOrderRoute.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["VERSION_NO"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);


                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderRoute, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderRoute_Delete != null && dtWorkOrderRoute_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderRoute_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}'
                                                         AND VERSION_NO = '{3}'", workOrderRoute.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //工单线别绑定
                    if (dtWorkOrderLine_Insert != null && dtWorkOrderLine_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderLine_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND LINE_KEY = '{2}'", workOrderLine.TABLE_NAME,
                                                                                        dr["WORK_ORDER_KEY"],
                                                                                        dr["LINE_KEY"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该工单对应的OEM的信息是否存在且被删除过，若存在在原有的基础上对版本号进行+1
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                                dr["WORK_ORDER_LINE_KEY"] = dsInsertReturn.Tables[0].Rows[0]["WORK_ORDER_LINE_KEY"];
                            }
                            else
                            {
                                dr["VERSION_NO"] = 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderLine, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderLine_Update != null && dtWorkOrderLine_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderLine_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_LINE_KEY = '{2}' ", workOrderLine.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_LINE_KEY"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);


                            sqlCommand = string.Format(@"SELECT * FROM {0}
                                                         WHERE WORK_ORDER_KEY = '{1}'
                                                         AND LINE_KEY = '{2}'", workOrderLine.TABLE_NAME,
                                                                                        dr["WORK_ORDER_KEY"],
                                                                                        dr["LINE_KEY"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该工单对应的OEM的信息是否存在且被删除过，若存在在原有的基础上对版本号进行+1
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                                dr["WORK_ORDER_LINE_KEY"] = dsInsertReturn.Tables[0].Rows[0]["WORK_ORDER_LINE_KEY"];
                            }
                            else
                            {
                                dr["VERSION_NO"] = 1;
                            }

                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderLine, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkOrderLine_Delete != null && dtWorkOrderLine_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderLine_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_LINE_KEY = '{2}'
                                                         ", workOrderLine.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_LINE_KEY"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //工单功率上下线管控
                    sqlCommand = string.Format(@"DELETE  FROM {0}
                                                 WHERE WORK_ORDER_KEY = '{1}'", "POR_WO_PRD_UPLOWRULE",
                                                                                dtWorkOrderUpLowRuleKEY.Rows[0]["WORK_ORDER_KEY"].ToString());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    if (dtWorkOrderUpLowRule != null && dtWorkOrderUpLowRule.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkOrderUpLowRule.Rows)
                        {
                            sqlCommand = string.Format(@"INSERT INTO {0}(UPLOW_RULE_KEY,UPLOW_RULE_UPLINE,UPLOW_RULE_LOWLINE,
                                                                            WORK_ORDER_KEY,WORK_ORDER,CREATOR,CREATER_TIME,EDITOR,EDITOR_TIME)
                                                                VALUES('{1}','{2}','{3}','{4}','{5}','{6}',SYSDATETIME(),'{7}',SYSDATETIME())",
                                                                              "POR_WO_PRD_UPLOWRULE",
                                                                              UtilHelper.GenerateNewKey(0),
                                                                              dr["UPLOW_RULE_UPLINE"],
                                                                              dr["UPLOW_RULE_LOWLINE"],
                                                                              dr["WORK_ORDER_KEY"],
                                                                              dr["WORK_ORDER"],
                                                                              dr["CREATOR"],
                                                                              dr["EDITOR"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    #endregion

                    #region //CTM上下线管控
                    sqlCommand = string.Format(@"UPDATE dbo.POR_WO_PRD_CTM SET
                                            IS_USED = 0 WHERE WORK_ORDER_KEY = '{0}'",
                                                                                dtPorWorkCtmWorkKey.Rows[0][0].ToString());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    if (dtPorWorkCtm != null && dtPorWorkCtm.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPorWorkCtm.Rows)
                        {
                            sqlCommand = string.Format(@"INSERT INTO dbo.POR_WO_PRD_CTM(WORK_ORDER_KEY,POR_WO_PRD_CTM_KEY,PRO_KEY,EFF_UP,EFF_LOW,CTM_UP,CTM_LOW)
                                                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                              dr["WORK_ORDER_KEY"],
                                                                              UtilHelper.GenerateNewKey(0),
                                                                              dr["PRO_KEY"],
                                                                              dr["EFF_UP"],
                                                                              dr["EFF_LOW"],
                                                                              dr["CTM_UP"],
                                                                              dr["CTM_LOW"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    #endregion

                    #region //打印规则设定
                    sqlCommand = string.Format(@"DELETE FROM dbo.POR_WO_PRD_PRINTRULE_DETAIL WHERE WORK_ORDER_KEY = '{0}'",
                                                                                dtPorWorkCtmWorkKey.Rows[0][0].ToString());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    if (dtWoPrintRuleDetail != null && dtWoPrintRuleDetail.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWoPrintRuleDetail.Rows)
                        {
                            sqlCommand = string.Format(@"INSERT INTO dbo.POR_WO_PRD_PRINTRULE_DETAIL(PRINT_RULE_KEY,
                                                                        WORK_ORDER_KEY,
                                                                        PRINT_COLUM_CODE,
                                                                        PRINT_COLUM_NAME,
                                                                        PRINT_COLUM_VALUE,
                                                                        PRINT_CODE)
                                                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                                              UtilHelper.GenerateNewKey(0),
                                                                              dtPorWorkCtmWorkKey.Rows[0][0].ToString(),
                                                                              dr["PRINT_COLUM_CODE"],
                                                                              dr["PRINT_COLUM_NAME"],
                                                                              dr["PRINT_VALUE"],
                                                                              dr["PRINT_CODE"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }

                    if ((dtWoPrintRule == null || dtWoPrintRule.Rows.Count <= 0) && (dtWoPrintRuleDetail != null && dtWoPrintRuleDetail.Rows.Count > 0))
                    { }
                    else
                    {
                        sqlCommand = string.Format(@"DELETE FROM dbo.POR_WO_PRD_PRINTRULE WHERE WORK_ORDER_KEY = '{0}'",
                                                                                    dtPorWorkCtmWorkKey.Rows[0][0].ToString());

                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                        if (dtWoPrintRule != null && dtWoPrintRule.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtWoPrintRule.Rows)
                            {
                                sqlCommand = string.Format(@"INSERT INTO dbo.POR_WO_PRD_PRINTRULE(PRINT_KEY,
                                                                            WORK_ORDER_KEY,
                                                                            WORK_ORDER_NUMBER,
                                                                            PRINT_CODE,
                                                                            PRINT_NAME,
                                                                            PRINT_DESC,
                                                                            PRINT_RESOUCE,
                                                                            CREATOR,
                                                                            CEEATER_TIME)
                                                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',GETDATE())",
                                                                                  UtilHelper.GenerateNewKey(0),
                                                                                  dr["WORK_ORDER_KEY"],
                                                                                  dr["WORK_ORDER_NUMBER"],
                                                                                  dr["PRINT_CODE"],
                                                                                  dr["PRINT_NAME"],
                                                                                  dr["PRINT_DESC"],
                                                                                  dr["PRINT_RESOUCE"],
                                                                                  dr["CREATOR"]);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                    }

                    #endregion
                    #region //功率铭牌清单体现信息保存
                    if (dtProPShow_Insert != null && dtProPShow_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPShow_Insert.Rows)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM POR_WO_PRD_POWERSHOW
                                                         WHERE WORK_ORDER_KEY = '{0}'
                                                         AND PART_NUMBER = '{1}'
                                                         AND RULE_CODE = '{2}'",
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["RULE_CODE"]);
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            //判断该产品对应的信息是否存在且被删除过
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                dr["VERSION_NO"] = Convert.ToInt32(dsInsertReturn.Tables[0].Compute("max(VERSION_NO)", "")) + 1;
                            }

                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPowerShow, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPShow_Update != null && dtProPShow_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPShow_Update.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND RULE_CODE ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProPowerShow.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["RULE_CODE"],
                                                                                   dr["VERSION_NO"]);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            dr["VERSION_NO"] = Convert.ToInt32(dr["VERSION_NO"]) + 1;
                            dr["CREATOR"] = dr["EDITOR"];
                            dr["EDIT_TIME"] = DBNull.Value;
                            dr["CREATE_TIME"] = DBNull.Value;


                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workOrderProPowerShow, hashTable, null);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtProPShow_Delete != null && dtProPShow_Delete.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtProPShow_Delete.Rows)
                        {

                            sqlCommand = string.Format(@"UPDATE {0} 
                                                         SET EDITOR = '{1}',
                                                         EDIT_TIME = SYSDATETIME(),
                                                         IS_USED = 'N'
                                                         WHERE WORK_ORDER_KEY = '{2}' 
                                                         AND PART_NUMBER = '{3}'
                                                         AND RULE_CODE ='{4}'
                                                         AND VERSION_NO = '{5}'", workOrderProPowerShow.TABLE_NAME,
                                                                                   dr["EDITOR"],
                                                                                   dr["WORK_ORDER_KEY"],
                                                                                   dr["PART_NUMBER"],
                                                                                   dr["RULE_CODE"],
                                                                                   dr["VERSION_NO"]);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region
                    if (dtWorkAttr_Insert != null && dtWorkAttr_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkAttr_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workAttr, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtWorkAttr_Update != null && dtWorkAttr_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkAttr_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            List<WhereConditions> lwc = new List<WhereConditions>();
                            WhereConditions wc01 = new WhereConditions(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY, hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY].ToString());
                            lwc.Add(wc01);

                            sqlCommand = string.Format(@"insert into [POR_WORK_ORDER_ATTR]
                                                                            ( [WORK_ORDER_KEY],[ATTRIBUTE_KEY],[ATTRIBUTE_NAME],[ATTRIBUTE_VALUE]
                                                                            ,[ATTRIBUTE_TYPE],[EDITOR],[EDIT_TIME],[EDIT_TIMEZONE]
                                                                            ,[WORK_ORDER_ATTR_KEY],[PRO_ID],[ISFLAG])
                                                                            SELECT [WORK_ORDER_KEY],[ATTRIBUTE_KEY],[ATTRIBUTE_NAME],[ATTRIBUTE_VALUE]
                                                                            ,[ATTRIBUTE_TYPE],'{0}',[EDIT_TIME],[EDIT_TIMEZONE]
                                                                            ,NEWID(),[PRO_ID],0
                                                                            FROM [POR_WORK_ORDER_ATTR]
                                                                            where [WORK_ORDER_ATTR_KEY]='{1}'"
                                                                , Convert.ToString(hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDITOR]),
                                                                Convert.ToString(hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_WORK_ORDER_ATTR_KEY]));
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            hashTable.Remove(POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY);
                            hashTable[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_EDIT_TIME] = string.Empty;

                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(workAttr, hashTable, lwc);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region //包装清单自动打印设定
                    if (dtPackPrint_Insert != null && dtPackPrint_Insert.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPackPrint_Insert.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            sqlCommand = DatabaseTable.BuildInsertSqlStatement(workFlashAutoPrint, hashTable, null);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    if (dtPackPrint_Update != null && dtPackPrint_Update.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPackPrint_Update.Rows)
                        {
                            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                            List<WhereConditions> lwc = new List<WhereConditions>();
                            WhereConditions wc01 = new WhereConditions(POR_WO_FLASH_AUTOPRINT.FIELDS_WORK_ORDER_KEY, hashTable[POR_WO_FLASH_AUTOPRINT.FIELDS_WORK_ORDER_KEY].ToString());
                            WhereConditions wc02 = new WhereConditions(POR_WO_FLASH_AUTOPRINT.FIELDS_PART_NUMBER, hashTable[POR_WO_FLASH_AUTOPRINT.FIELDS_PART_NUMBER].ToString());
                            lwc.Add(wc01);
                            lwc.Add(wc02);
                            hashTable.Remove(POR_WO_FLASH_AUTOPRINT.FIELDS_WORK_ORDER_KEY);
                            hashTable.Remove(POR_WO_FLASH_AUTOPRINT.FIELDS_PART_NUMBER);
                            sqlCommand = DatabaseTable.BuildUpdateSqlStatement(workFlashAutoPrint, hashTable, lwc);
                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        }
                    }
                    #endregion

                    #region EL 图片规则保存
                    if (dtElPicRule != null && dtElPicRule.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtElPicRule.Rows.Count; i++)
                        {
                            sqlCommand = string.Format(@"SELECT * FROM POR_WO_ELTESTRULE WHERE WORK_ORDER_KEY = '{0}' and rule_type='{1}' ", dtElPicRule.Rows[i]["WORK_ORDER_KEY"].ToString(), dtElPicRule.Rows[i]["RULE_TYPE"].ToString());
                            DataSet dsInsertReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                            if (dsInsertReturn.Tables != null && dsInsertReturn.Tables[0].Rows.Count > 0)
                            {
                                sqlCommand = string.Format(@"UPDATE POR_WO_ELTESTRULE
                                                         SET ELTESTRULE = '{1}',EDIT_USER_ID = '{2}',EDIT_TIME = getdate() WHERE WORK_ORDER_KEY = '{0}' and RULE_TYPE='{3}' ",
                                                                                      dtElPicRule.Rows[0]["WORK_ORDER_KEY"].ToString(),
                                                                                      dtElPicRule.Rows[0]["ELTESTRULE"].ToString(),
                                                                                      dtElPicRule.Rows[0]["EDIT_USER_ID"].ToString(),
                                                                                      dtElPicRule.Rows[0]["RULE_TYPE"].ToString());
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                            else
                            {
                                String por_wo_eltestrule_key = System.Guid.NewGuid().ToString();
                                sqlCommand = string.Format(@"insert into [POR_WO_ELTESTRULE]
                                                                            ([POR_WO_ELTESTRULE_KEY],[WORK_ORDER_KEY],[ELTESTRULE],[CREATE_USER_ID],[CREATE_TIME],[EDIT_USER_ID],[EDIT_TIME],[RULE_TYPE])
                                                                values('{0}','{1}','{2}','{3}',getdate(),'{4}',getdate(),'{5}')"
                                                                   , por_wo_eltestrule_key
                                                                   , dtElPicRule.Rows[i]["WORK_ORDER_KEY"].ToString()
                                                                   , dtElPicRule.Rows[i]["ELTESTRULE"].ToString()
                                                                   , dtElPicRule.Rows[i]["CREATE_USER_ID"].ToString()
                                                                   , dtElPicRule.Rows[i]["EDIT_USER_ID"].ToString()
                                                                   , dtElPicRule.Rows[i]["RULE_TYPE"].ToString());
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                    }
                    #endregion
                    #region 打印信息
                    if (dtWoPrint != null)
                    {
                        sqlCommand = @"SELECT GETDATE()";
                        string time = Convert.ToDateTime(db.ExecuteScalar(CommandType.Text, sqlCommand)).ToString("yyyy-MM-dd");

                        int year = int.Parse(time.Substring(2, 2)) + 5;
                        int mouth = int.Parse(time.Substring(5, 2)) + 5;
                        int day = int.Parse(time.Substring(8, 2)) + 5;

                        sqlCommand = string.Format("DELETE FROM POR_WO_PRINT WHERE ORDER_NUMBER='{0}'", dtWoPrint.Rows[0]["ORDER_NUMBER"]);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                        sqlCommand = string.Format(@"INSERT INTO POR_WO_PRINT(ORDER_NUMBER,
                                                                                F001,
                                                                                F002,
                                                                                F003,
                                                                                F004,
                                                                                F005,
                                                                                F006,
                                                                                F007,
                                                                                F008,
                                                                                F101,
                                                                                F102) 
                                                     VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',GETDATE())",
                                                     dtWoPrint.Rows[0]["ORDER_NUMBER"],
                                                     dtWoPrint.Rows[0]["F001"],
                                                     dtWoPrint.Rows[0]["F002"],
                                                     dtWoPrint.Rows[0]["F003"],
                                                     dtWoPrint.Rows[0]["F004"],
                                                     dtWoPrint.Rows[0]["F005"],
                                                     year + "" + mouth + "" + day + "",
                                                     dtWoPrint.Rows[0]["F007"],
                                                     dtWoPrint.Rows[0]["F008"],
                                                     dtWoPrint.Rows[0]["F101"]);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    }

                    #endregion
                    dbTran.Commit();

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    LogService.LogError("SaveWorkOrderAttrParam Error: " + ex.Message);
                }
                finally
                {
                    //Close Connection
                    dbConn.Close();
                }
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过分档代码获取对应的分档规则
        /// </summary>
        /// <param name="powerSetCode">分档代码</param>
        /// <returns>分档代码对应的功率分档的集合</returns>
        public DataSet GetPowerSetByPowerSetCode(string powerSetCode, string partMinPower, string partMaxPower)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommonEnd = string.Empty;

                if (!string.IsNullOrEmpty(partMinPower))
                {
                    sqlCommonEnd += string.Format(" AND B.P_MIN >= '{0}' ", partMinPower);
                }

                if (!string.IsNullOrEmpty(partMaxPower))
                {
                    sqlCommonEnd += string.Format(" AND B.P_MAX <= '{0}' ", partMaxPower);
                }



                string sqlCommon = string.Format(@"SELECT * FROM  BASE_POWERSET B
                                                   WHERE B.ISFLAG = 1 
                                                   AND B.PS_CODE = '{0}' ", powerSetCode);

                sqlCommon += sqlCommonEnd + " ORDER BY B.PS_SEQ ASC";

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtPowerSet.TableName = "PowerSet";
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);
                if (dtPowerSet.Rows.Count > 0)
                {
                    sqlCommon = string.Format(@"SELECT A.* FROM BASE_POWERSET_DETAIL A
                                                INNER JOIN BASE_POWERSET B ON A.POWERSET_KEY = B.POWERSET_KEY
                                                WHERE A.ISFLAG = 1 
                                                AND B.ISFLAG = 1
                                                AND B.PS_CODE = '{0}'", powerSetCode);

                    sqlCommon += sqlCommonEnd + " ORDER BY A.POWERSET_KEY,A.PS_DTL_SUBCODE ASC";

                    DataTable dtPowerSetSub = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                    dtPowerSetSub.TableName = "PowerSetSub";
                    dsReturn.Merge(dtPowerSetSub, true, MissingSchemaAction.Add);

                    sqlCommon = string.Format(@"SELECT A.* FROM BASE_POWERSET_COLORATCNO A
                                                INNER JOIN BASE_POWERSET B ON A.POWERSET_KEY = B.POWERSET_KEY
                                                WHERE A.ISFLAG = 1 
                                                AND B.ISFLAG = 1
                                                AND B.PS_CODE = '{0}'", powerSetCode);

                    sqlCommon += sqlCommonEnd + " ORDER BY A.POWERSET_KEY ,A.COLOR_CODE ASC";

                    DataTable dtPowerSetColor = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                    dtPowerSetColor.TableName = "PowerSetColor";
                    dsReturn.Merge(dtPowerSetColor, true, MissingSchemaAction.Add);
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerSetByPowerSetCode Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单号或者序列号获取对应工单的OEM信息
        /// </summary>
        /// <param name="orderNumber">托盘对应的工单号</param>
        /// <param name="lotNumber">组件序列号</param>
        /// <returns>工单对应的OEM信息</returns>
        public DataSet GetWorkOrderOEMByOrderNumberOrLotNumber(string orderNumber, string lotNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                if (!string.IsNullOrEmpty(orderNumber))
                {
                    sqlCommon = string.Format(@"SELECT * FROM POR_WO_OEM
                                                   WHERE IS_USED = 'Y'
                                                   AND ORDER_NUMBER = '{0}'", orderNumber);
                }

                if (!string.IsNullOrEmpty(lotNumber))
                {
                    sqlCommon = string.Format(@"SELECT E.* FROM POR_LOT A
                                                LEFT JOIN POR_WORK_ORDER B ON A.WORK_ORDER_NO = B.ORDER_NUMBER 
                                                LEFT JOIN POR_WO_OEM E ON B.WORK_ORDER_KEY = E.WORK_ORDER_KEY AND E.IS_USED = 'Y'
                                                WHERE A.LOT_NUMBER = '{0}';", lotNumber);
                }

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtPowerSet.TableName = "POR_WO_OEM";
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderOEMByOrderNumberOrLotNumber Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单号获取接线盒信息的集合
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>接线盒信息的集合</returns>
        public DataSet GetWOJunctionBox(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"SELECT  ORDER_NUMBER,MATERIAL_CODE,B.JUNCTION_BOX
                                            FROM POR_WORK_ORDER_BOM A 
                                                 INNER JOIN POR_WO_MATERIAL_JUNCTION_BOX B ON A.MATERIAL_CODE = B.MATERIALCODE
                                            WHERE ORDER_NUMBER='{0}'
                                            AND A.REQ_QTY > 0", workorderNumber);

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtPowerSet.TableName = "POR_WO_MATERIAL_JUNCTION_BOX";
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWOJunctionBox Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单号获取工单对应的工艺流程信息
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>工单对应的工艺流程信息</returns>
        public DataSet GetWorkOrderRouteInfo(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"SELECT * FROM POR_WO_ROUTE WHERE WORK_ORDER_NUMBER = '{0}' AND IS_USED = 'Y'", workorderNumber);

                DataTable dtPowerSet = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtPowerSet.TableName = "POR_WO_ROUTE";
                dsReturn.Merge(dtPowerSet, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderRouteInfo Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工单获取OEM客户信息
        /// </summary>
        /// <param name="workorderNumber">工单信息</param>
        /// <returns>工单对应的OEM信息</returns>
        public DataSet GetWorkOrderOEMCustomer(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"SELECT CUSROMER FROM POR_WO_OEM
                                            WHERE ORDER_NUMBER = '{0}'
                                            AND IS_USED = 'Y'", workorderNumber);

                DataTable dtOEMCustomer = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtOEMCustomer.TableName = "POR_WO_OEM_CUSTOMER";
                dsReturn.Merge(dtOEMCustomer, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkOrderOEMCustomer Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取工厂线别
        /// </summary>
        /// <returns>获取工厂线别的数据集合</returns>
        public DataSet GetFatoryLine()
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"SELECT a.LINE_NAME AS LINE_NAME,
	                                               a.PRODUCTION_LINE_KEY AS LINE_KEY,
	                                               a.LINE_CODE AS LINE_CODE,
	                                               c.ROOM_NAME AS FACTORY_NAME,
	                                               c.ROOM_KEY AS FACTORY_KEY
                                            FROM FMM_PRODUCTION_LINE a
                                            INNER JOIN FMM_LOCATION_LINE b ON a.PRODUCTION_LINE_KEY=b.LINE_KEY
                                            INNER JOIN V_LOCATION c ON b.LOCATION_KEY=c.AREA_KEY
                                            ORDER BY FACTORY_NAME,LINE_CODE DESC");

                DataTable dtOEMCustomer = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];
                dtOEMCustomer.TableName = "FACTORY_LINE";
                dsReturn.Merge(dtOEMCustomer, true, MissingSchemaAction.Add);


                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFatoryLine Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取绑定的工厂线别是否正确
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns>True:绑定线别正确、或未绑定线别。False：绑定线别但是所选线别不在绑定范围</returns>
        public bool CheckWorkOrderLineBind(string LotNumber, string lineKey)
        {
            bool isBindTrueLine = true;

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"DECLARE @workOrderKey VARCHAR(64);

                                            SELECT @workOrderKey = WORK_ORDER_KEY FROM POR_LOT
                                            WHERE LOT_NUMBER = '{0}'

                                            IF EXISTS(SELECT 1 FROM POR_WO_LINE
		                                              WHERE WORK_ORDER_KEY = @workOrderKey
		                                              AND IS_USED = 'Y')
                                            BEGIN
	                                            IF EXISTS( SELECT 1 FROM POR_WO_LINE
		                                              WHERE WORK_ORDER_KEY = @workOrderKey
		                                              AND LINE_KEY = '{1}'
		                                              AND IS_USED = 'Y')
	                                            BEGIN
		                                            SELECT 1 AS VALUE
	                                            END
	                                            ELSE
	                                            BEGIN
		                                            SELECT 0 AS VALUE
	                                            END
                                            		  
                                            END
                                            ELSE
                                            BEGIN
	                                            SELECT 1 AS VALUE
                                            END
                                            ", LotNumber, lineKey);

                DataTable dtReturnValue = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];

                if (dtReturnValue.Rows[0][0].ToString() == "0")
                {
                    isBindTrueLine = false;
                }

            }
            catch (Exception ex)
            {
                LogService.LogError("CheckWorkOrderLineBind Error: " + ex.Message);
            }

            return isBindTrueLine;
        }
        /// <summary>
        /// 更新工单产品设置清单打印设置 2018.4.16
        /// </summary>
        /// <param name="WorkOrderNumber"></param>
        /// <returns></returns>
        public bool isUpDataPrint(string WorkOrderNumber)
        {
            bool isUpData = false;

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@"DELETE FROM POR_WO_FLASH_AUTOPRINT WHERE WORK_ORDER_KEY={0}
                                            ", WorkOrderNumber.PreventSQLInjection());
                DataSet dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                //DataTable dtReturnValue = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];


                isUpData = true;


            }
            catch (Exception ex)
            {
                LogService.LogError("isUpDataPrint Error: " + ex.Message);
            }

            return isUpData;
        }

        #region MyRegion
        public DataSet GetElPicRuleData(string workOrderKey)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsElrule = new DataSet();
            try
            {
                string sqlWoElRule = string.Format(@"SELECT POR_WO_ELTESTRULE_KEY,ELTESTRULE
                                                               FROM POR_WO_ELTESTRULE
                                                               WHERE WORK_ORDER_KEY = '{0}' ", workOrderKey);
                dsElrule = db.ExecuteDataSet(CommandType.Text, sqlWoElRule);
                dsElrule.Tables[0].TableName = "POR_WO_ELTESTRULE";
                dsReturn.Merge(dsElrule.Tables[0]);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetElPicRuleData Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion


        /// <summary>
        /// 通过LableID获取对应的打印类型
        /// </summary>
        /// <param name="lableID">LableID</param>
        /// <returns>该LableID对应的打印类型</returns>
        public string GetLablePrinterType(string lableID)
        {
            string printerType = string.Empty;

            try
            {
                //语句查询条件
                string sqlCommon = string.Empty;

                sqlCommon = string.Format(@" IF EXISTS(SELECT 1 FROM BASE_PRINTLABEL
		                                              WHERE LABEL_ID = '{0}'
		                                              AND IS_USED = 'Y')
                                            BEGIN
	                                            SELECT PRINTER_TYPE FROM BASE_PRINTLABEL 
	                                            WHERE LABEL_ID = '{0}'
												AND IS_USED = 'Y'                                           		  
                                            END
                                            ELSE
                                            BEGIN
	                                            SELECT 'Not Exists' AS PRINTER_TYPE
                                            END
                                            ", lableID);

                DataTable dtReturnValue = db.ExecuteDataSet(CommandType.Text, sqlCommon).Tables[0];

                printerType = dtReturnValue.Rows[0]["PRINTER_TYPE"].ToString();

            }
            catch (Exception ex)
            {
                LogService.LogError("GetLablePrinterType Error: " + ex.Message);
            }

            return printerType;
        }

        #region 获取EL 规则
        public DataSet GetElPicRuleData(string workOrderKey, string ruletype)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsElrule = new DataSet();
            try
            {
                string sqlWoElRule = string.Format(@"SELECT POR_WO_ELTESTRULE_KEY,ELTESTRULE
                                                               FROM POR_WO_ELTESTRULE
                                                               WHERE WORK_ORDER_KEY = '{0}' and  rule_type = '{1}' ", workOrderKey, ruletype);
                dsElrule = db.ExecuteDataSet(CommandType.Text, sqlWoElRule);
                dsElrule.Tables[0].TableName = "POR_WO_ELTESTRULE";
                dsReturn.Merge(dsElrule.Tables[0]);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetElPicRuleData Error: " + ex.Message);
            }
            return dsReturn;
        }
        #endregion


        #region IWorkOrdersEngine 成员


        /// <summary>
        /// 获取工单功率卡控上下限
        /// </summary>
        /// <param name="koRkNumber"></param>
        /// <returns></returns>
        public DataSet GetUpLowRule(string workOrderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlPo = string.Format(@"SELECT * FROM POR_WO_PRD_UPLOWRULE WHERE WORK_ORDER = '{0}'", workOrderNumber);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlPo);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetUpLowRule Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWorkOrdersEngine 成员

        /// <summary>
        /// 获取ctm上下限信息值
        /// </summary>
        /// <param name="proKey"></param>
        /// <returns></returns>
        public DataSet GetCtmInf(string proKey, string workOrder)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsProCtm = new DataSet();
            DataSet dsPorWoCtm = new DataSet();
            try
            {
                string sqlPo = string.Format(@"SELECT A.EFF_UP,A.EFF_LOW,A.CTM_UP,A.CTM_LOW,b.PRODUCT_KEY AS PRO_KEY,'{1}' AS WORK_ORDER_KEY  FROM dbo.BASE_PRODUCTMODEL_CTM A
                                                    INNER JOIN dbo.BASE_PRODUCTMODEL C ON A.PROMODEL_KEY = C.PROMODEL_KEY
                                                    INNER JOIN dbo.POR_PRODUCT B ON C.PROMODEL_NAME = B.PROMODEL_NAME
                                                    WHERE B.PRODUCT_KEY = '{0}'
                                                    AND A.ISFLAG = 1 AND B.ISFLAG = 1 AND C.ISFLAG = 1", proKey, workOrder);
                dsProCtm = db.ExecuteDataSet(CommandType.Text, sqlPo);
                dsProCtm.Tables[0].TableName = "BASE_PRODUCTMODEL_CTM";
                dsProCtm.Tables[0].Columns.Add("ischecked", typeof(bool));

                string sqlPorWoCTM = string.Format(@"SELECT EFF_UP,EFF_LOW,CTM_UP,CTM_LOW,PRO_KEY,WORK_ORDER_KEY FROM dbo.POR_WO_PRD_CTM
                                                            WHERE WORK_ORDER_KEY = '{0}' AND IS_USED = 1", workOrder);
                dsPorWoCtm = db.ExecuteDataSet(CommandType.Text, sqlPorWoCTM);
                dsPorWoCtm.Tables[0].TableName = "POR_WO_PRD_CTM";
                dsPorWoCtm.Tables[0].Columns.Add("ischecked", typeof(bool));
                dsReturn.Merge(dsProCtm.Tables[0]);
                dsReturn.Merge(dsPorWoCtm.Tables[0]);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCtmInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWorkOrdersEngine 成员

        /// <summary>
        /// 通过工单号获取打印规则内容
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>数据集</returns>
        public DataSet GetPrintRuleData(string workOrderKey)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsPrintrule = new DataSet();
            DataSet dsPrintruleDetail = new DataSet();
            try
            {
                string sqlWoPrintRuleDetail = string.Format(@"SELECT PRINT_CODE,PRINT_COLUM_CODE,
                                                               PRINT_COLUM_NAME,
                                                               PRINT_COLUM_VALUE AS PRINT_VALUE 
                                                               FROM POR_WO_PRD_PRINTRULE_DETAIL
                                                               WHERE WORK_ORDER_KEY = '{0}' ", workOrderKey);
                dsPrintruleDetail = db.ExecuteDataSet(CommandType.Text, sqlWoPrintRuleDetail);
                dsPrintruleDetail.Tables[0].TableName = "PRO_WO_PRD_PRINTRULE_DETAIL";

                string sqlWoPrintRule = string.Format(@"SELECT PRINT_CODE,PRINT_NAME,PRINT_DESC
                                                               FROM POR_WO_PRD_PRINTRULE
                                                               WHERE WORK_ORDER_KEY = '{0}' ", workOrderKey);
                dsPrintrule = db.ExecuteDataSet(CommandType.Text, sqlWoPrintRule);
                dsPrintrule.Tables[0].TableName = "PRO_WO_PRD_PRINTRULE";

                dsReturn.Merge(dsPrintruleDetail.Tables[0]);
                dsReturn.Merge(dsPrintrule.Tables[0]);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPrintRuleData Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWorkOrdersEngine 成员


        public DataSet GetProductModel()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlWoPrintRule = string.Format(@"SELECT [PRODUCT_MODEL_CODE]
                                                              ,[PRODUCT_MODEL_NAME]
                                                          FROM [dbo].[BASE_PRODUCT_MODEL]
                                                          ORDER BY PRODUCT_MODEL_CODE ASC  ");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlWoPrintRule);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetProductModel Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region //获取包装清单打印设置的信息
        public DataSet GetFlashAutoPrintData(string workOrderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT *     
                                                    FROM [dbo].[POR_WO_FLASH_AUTOPRINT]
                                                 WHERE WORK_ORDER_KEY='{0}'"
                                                    , workOrderNumber
                                                     );
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFlashAutoPrintData Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetWoPrint(string workOrderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT *     
                                                    FROM [dbo].[POR_WO_PRINT]
                                                 WHERE ORDER_NUMBER='{0}'"
                                                    , workOrderNumber
                                                     );
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFlashAutoPrintData Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}
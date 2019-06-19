using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.Wip
{
    /// <summary>
    /// 用于进行批次创建的类，实现了批次创建接口（<see cref="ILotCreateEngine"/>）。
    /// </summary>
    public class LotCreateEngine : AbstractEngine, ILotCreateEngine
    {
        private Database db;                             //用于数据库访问的变量。
        private readonly object objlock = new object();
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotCreateEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize() { }
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
                string sqlCommand = string.Format("SELECT DISTINCT PRODUCT_CODE,PRODUCT_NAME FROM POR_PRODUCT ORDER BY PRODUCT_CODE");
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
        /// 获取产品ID号对应的电池片数量。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <returns>电池片数量。</returns>
        public double GetCellNumber(string proId)
        {
            double cellNumber = 60;
            try
            {
                string sqlCommand = string.Format(@"SELECT ISNULL(b.CELL_NUM,60)
                                                    FROM POR_PRODUCT a
                                                    LEFT JOIN BASE_PRODUCTMODEL b ON a.PROMODEL_NAME=b.PROMODEL_NAME
                                                    WHERE a.PRODUCT_CODE='{0}'",
                                                    proId.PreventSQLInjection());
                object objCellNumber = db.ExecuteScalar(CommandType.Text, sqlCommand);
                if (objCellNumber != DBNull.Value
                    && objCellNumber != null)
                {
                    cellNumber = Convert.ToDouble(objCellNumber);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(string.Format("GetCellNumber Error:{0}-{1}", proId, ex.Message));
            }
            return cellNumber;
        }

        /// <summary>
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="orderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialInfo(string roomKey, string operationName, string orderNo, string proId)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(@"SELECT DISTINCT a.MATERIAL_LOT,d.AUFNR,d.MATNR,d.PRO_ID,d.EFFICIENCY,a.RECEIVE_QTY,a.CURRENT_QTY,d.LLIEF SUPPLIER_NAME,a.STORE_MATERIAL_DETAIL_KEY
                                    FROM WST_STORE_MATERIAL_DETAIL a
                                    LEFT JOIN WST_STORE_MATERIAL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                    LEFT JOIN WST_STORE c ON b.STORE_KEY=c.STORE_KEY
                                    LEFT JOIN WST_SAP_ISSURE d ON a.STORE_MATERIAL_DETAIL_KEY=d.STORE_MATERIAL_DETAIL_KEY
                                    WHERE a.CURRENT_QTY>0");
                if (!string.IsNullOrEmpty(roomKey))
                {
                    sqlBuilder.AppendFormat(" AND c.LOCATION_KEY='{0}'", roomKey.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(operationName))
                {
                    sqlBuilder.AppendFormat(" AND c.OPERATION_NAME='{0}'", operationName.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(orderNo))
                {
                    sqlBuilder.AppendFormat(" AND d.AUFNR='{0}'", orderNo.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(proId))
                {
                    sqlBuilder.AppendFormat(" AND d.PRO_ID='{0}'", proId.PreventSQLInjection());
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlBuilder.ToString());
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetReceiveMaterialInfo Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="val">原材料存储明细主键。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialInfo(string val)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendFormat(@"SELECT DISTINCT a.MATERIAL_LOT,d.AUFNR,d.MATNR,d.PRO_ID,d.EFFICIENCY,a.RECEIVE_QTY,a.CURRENT_QTY,d.LLIEF SUPPLIER_NAME,d.SUPPLIER_CODE
                                    FROM WST_STORE_MATERIAL_DETAIL a
                                    LEFT JOIN WST_STORE_MATERIAL b ON a.STORE_MATERIAL_KEY=b.STORE_MATERIAL_KEY
                                    LEFT JOIN WST_STORE c ON b.STORE_KEY=c.STORE_KEY
                                    LEFT JOIN WST_SAP_ISSURE d ON a.STORE_MATERIAL_DETAIL_KEY=d.STORE_MATERIAL_DETAIL_KEY
                                    WHERE a.STORE_MATERIAL_DETAIL_KEY='{0}'", val.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlBuilder.ToString());
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetReceiveMaterialInfo Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
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
                                                ORDER BY ORDER_NUMBER",
                                                roomKey.PreventSQLInjection());
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
        /// 创建批次。
        /// </summary>
        /// <param name="dsParams">包含创建批次所需信息的数据集对象。</param>
        /// <returns>包含创建批次结果的数据集对象。</returns>
        public DataSet CeateLot(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbMsg = new StringBuilder();
            try
            {
                DataTable dtMainData = dsParams.Tables[0];      //存放批次主数据
                DataTable dtAddtionData = dsParams.Tables[1];   //存放附加数据
                DataTable dtNewLot = dsParams.Tables[2];        //存在批次号和硅片供应商
                Hashtable htMainData = CommonUtils.ConvertToHashtable(dtMainData);
                Hashtable htAddtionData = CommonUtils.ConvertToHashtable(dtAddtionData);
                string workOrderNo = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                string workOrderKey = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                string storeMaterialDetailKey = Convert.ToString(htAddtionData["STORE_MATERIAL_DETAIL_KEY"]);
                string materialLotNo = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
                string locationKey = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                string createOperationName = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
                double materialQtyPerLot = Convert.ToDouble(htMainData[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
                //double materialSumQty = dtNewLot.Rows.Count * materialQtyPerLot;
                //获取工单指定的组件默认等级。
                string sql = @"SELECT b.GRADE_CODE
                            FROM POR_WORK_ORDER_ATTR a
                            INNER JOIN V_ProductGrade b ON b.GRADE_NAME=a.ATTRIBUTE_VALUE
                            WHERE a.ATTRIBUTE_NAME='DefaultGrade'
                            AND a.ISFLAG=1
                            AND a.WORK_ORDER_KEY=@workOrderKey";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@workOrderKey", DbType.String, workOrderKey);
                object defaultGradeCode = db.ExecuteScalar(cmd);
                if (defaultGradeCode != DBNull.Value && defaultGradeCode != null)
                {
                    if (htMainData.Contains(POR_LOT_FIELDS.FIELD_PRO_LEVEL))
                    {
                        htMainData.Add(POR_LOT_FIELDS.FIELD_PRO_LEVEL, defaultGradeCode);
                    }
                    else
                    {
                        htMainData[POR_LOT_FIELDS.FIELD_PRO_LEVEL] = defaultGradeCode;
                    }
                }
                int creatingCount = 0;
                int createdCount = 0;
                //移除dtNew中的无效列
                dtNewLot.Columns.Remove("CHECK_SMALL_PACK_NUMBER");
                //增加批次。
                foreach (DataRow drLot in dtNewLot.Rows)
                {
                    creatingCount++;
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        //批次转换为大写字母
                        string lotNumber = Convert.ToString(drLot["LOT_NUMBER"]).ToUpper();
                        sql = string.Format(@"SELECT COUNT(1) FROM POR_LOT WHERE LOT_NUMBER='{0}'",
                                    lotNumber.PreventSQLInjection());
                        int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                        if (count > 0)
                        {
                            sbMsg.AppendFormat("{1}-【{0}】在数据库中已存在。\r\n", lotNumber, creatingCount);
                            continue;
                        }
                        string siSupplierLot = Convert.ToString(drLot["SI_SUPPLIER_LOT"]);
                        string smallPackNumber = Convert.ToString(drLot["SMALL_PACK_NUMBER"]); //小包条码 add by yongbing.yang 20130808
                        string lotType = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
                        //判断批次号在数据库中是否存在，如果存在返回。
                        //获取原材料数量，判断原材料数量是否满足创批需要的原材料数量。
                        sql = string.Format(@"SELECT ISNULL(SUM(a.CURRENT_QTY),0)
                                           FROM WST_STORE_MATERIAL_DETAIL a 
                                           WHERE a.STORE_MATERIAL_DETAIL_KEY='{0}'",
                                           storeMaterialDetailKey.PreventSQLInjection());
                        double currentMaterialSumQty = Convert.ToDouble(db.ExecuteScalar(CommandType.Text, sql));
                        double diffQty = currentMaterialSumQty - materialQtyPerLot;
                        if (diffQty < 0)
                        {
                            sbMsg.AppendFormat("因原材料数量不足，创建失败。\r\n", createdCount);
                            break;
                        }
                        //更新原材料数量
                        sql = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                            SET CURRENT_QTY={0} 
                                            WHERE STORE_MATERIAL_DETAIL_KEY='{1}'",
                                            diffQty,
                                            storeMaterialDetailKey);
                        db.ExecuteNonQuery(CommandType.Text, sql);
                        //查询批次是工单的第几个批次。
                        sql = string.Format(@"SELECT NEXT_SEQ FROM POR_WORK_ORDER WHERE ORDER_NUMBER='{0}'",
                                    workOrderNo.PreventSQLInjection());
                        int nextSeq = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                        //设置批次号、硅片供应商和批次在工单中的索引号
                        htMainData[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = lotNumber;
                        htMainData[POR_LOT_FIELDS.FIELD_SI_LOT] = siSupplierLot;
                        htMainData[POR_LOT_FIELDS.FIELD_SMALL_PACK_NUMBER] = smallPackNumber;   //小包条码 add by yongbing.yang 20130808
                        htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ] = nextSeq;
                        DataSet dsCreateLotParams = new DataSet();
                        DataTable dtCreateLotMaindata = CommonUtils.ParseToDataTable(htMainData);
                        dtCreateLotMaindata.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                        dsCreateLotParams.Tables.Add(dtCreateLotMaindata);
                        dsCreateLotParams.Tables.Add(dtAddtionData.Copy());
                        //创建批次
                        LotManagement.CreateLot(db, dsCreateLotParams, drLot);
                        //非组件补片批次更新工单剩余数量内。
                        if (lotType != "L")
                        {
                            //更新工单剩余数量
                            sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                                  SET QUANTITY_LEFT=QUANTITY_LEFT-1,NEXT_SEQ=NEXT_SEQ+1
                                                  WHERE ORDER_NUMBER='{0}'",
                                                  workOrderNo.PreventSQLInjection());
                            db.ExecuteNonQuery(CommandType.Text, sql);
                        }
                        createdCount++;
                        ts.Complete();
                    }
                }
                if (sbMsg.Length > 0)
                {
                    sbMsg.AppendFormat("新增{0}块组件。", createdCount);
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                LogService.LogError("CeateLot Error: " + ex.Message);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据产品ID获取产品类型。
        /// </summary>
        /// <param name="sProductCode">产品ID。</param>
        /// <returns>
        /// 产品类型的数据集合。
        /// </returns>
        public DataSet GetProductModeByPID(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            string sql;

            try
            {
                sql = "select a.*";
                sql += " from BASE_PRODUCTMODEL a,POR_PRODUCT b";
                sql += " where a.PROMODEL_NAME=b.PROMODEL_NAME";
                sql += " and a.ISFLAG='1' and b.ISFLAG='1'";
                if (sProductCode != "")
                {
                    sql += " and b.PRODUCT_CODE='" + sProductCode + "'";
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetProductModeByPID Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetOrderNoType(string OrderNo)
        {
            DataSet dsReturn = new DataSet();
            string sql;

            try
            {
                sql = string.Format(@"SELECT A.ORDER_NUMBER,B.ATTRIBUTE_VALUE,C.LABELTYPE
                                      FROM POR_WORK_ORDER A 
                                      LEFT JOIN POR_WORK_ORDER_ATTR B ON A.WORK_ORDER_KEY=B.WORK_ORDER_KEY AND B.ISFLAG='1'
                                      LEFT JOIN POR_WO_PRD C ON C.WORK_ORDER_KEY = A.WORK_ORDER_KEY AND C.PART_NUMBER = A.PART_NUMBER AND C.IS_USED = 'Y'
                                      WHERE  A.ORDER_NUMBER='{0}' AND B.ATTRIBUTE_NAME='Customer'",
                                      OrderNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                //如果没有设置客户属性，则默认为常规客户。
                if (dsReturn.Tables[0].Rows.Count == 0)
                {
                    dsReturn.Tables[0].Rows.Add(OrderNo, "常规");
                }
                else if (dsReturn.Tables[0].Rows.Count > 0) //判断设定的工单属性客户信息（常规和Conergy TUV 认证的话 进行序列号包含工单信息的检查）
                {
                    string orderType = Convert.ToString(dsReturn.Tables[0].Rows[0]["ATTRIBUTE_VALUE"]);
                    string proLableType = Convert.ToString(dsReturn.Tables[0].Rows[0]["LABELTYPE"]);

                    bool check = false;       //判断是否需要进行序列号包含工单信息的检查

                    //常规工单、Conergy工单 TUV 认证部分  需要进行序列号包含工单的检查

                    //检查是否为常规工单
                    if (!string.IsNullOrEmpty(orderType) && orderType.Contains("常规"))
                    {
                        check = true;
                    }

                    //检查是否为 Conergy TUV认证
                    if (!check & !string.IsNullOrEmpty(orderType) && orderType.ToLower().Contains("conergy") && proLableType.Contains("TUV"))
                    {
                        check = true;
                    }

                    //如果不需要进行序列号包含工单的检查清除行信息。
                    if (!check)
                    {
                        dsReturn.Tables[0].Rows.Clear();
                        dsReturn.Tables[0].AcceptChanges();
                    }
                }

                //新增一个空表，用于兼容原有的程序，防止客户端BUG。
                dsReturn.Tables.Add(new DataTable());
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetOrderNoType Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据小包条码获取电池片段的信息 
        /// </summary>
        /// <param name="Small_Pack_Number">小包条码</param>
        /// <returns>电池片信息的集合</returns>
        public DataSet GetCellInformation(string Small_Pack_Number)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlString = string.Format(@" SELECT SUPPLIER,FACTORY_NAME,LINE,'('+ EFFICIENCY_NAME+')' EFFICIENCY_NAME,MO,
		                                            PART_ID,'('+ convert(varchar(10),CDATE,120)+')' CDATE,GRADE,COLOR,
		                                            CHECKER,BATERYTYPE,SEGMENTEDLOCATION,SMALL_PACK_NUMBER,CREATE_DATE
                                                    FROM dbo.WIP_SMALL_PARK_BARCODE WHERE  SMALL_PACK_NUMBER = '{0}'",
                                                Small_Pack_Number);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlString);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetCellInformation Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取产品BOM清单
        /// </summary>
        /// <param name="orderNo">工单</param>
        /// <returns></returns>
        public DataSet GetWorkOrderBom(string orderNo)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"select  t.ORDER_NUMBER,t.MATERIAL_CODE,t.DESCRIPTION,t.REQ_QTY,t.MATERIAL_UNIT 
                                                    from POR_WORK_ORDER_BOM t 
                                                    where t. ORDER_NUMBER='{0}'", orderNo.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("WorkOrdersEngine GetWorkOrderBom Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }

        #region ILotCreateEngine 成员

        /// <summary>
        /// 获取外购电池片供应商信息
        /// </summary>
        /// <param name="smallPackNumber">电池片条码</param>
        /// <returns>数据集</returns>
        public DataSet GetOutCellSupplier(string smallPackNumber)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT SUPPLIER_NAME FROM dbo.BASE_OUTCELL_SUPPLIER
                                                        WHERE SUPPLIER_CODE = '{0}'", smallPackNumber);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogService.LogError(string.Format("GetOutCellSupplier Error:{0}", ex.Message));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            return dsReturn;
        }

        #endregion

        #region ILotCreateEngine 成员

        /// <summary>
        /// 传入工单主键获取工单中的规则信息创建除流水号之外的组件序列号
        /// </summary>
        /// <param name="orderKey">工单主键</param>
        /// <returns>主键序列号的数据集</returns>
        public DataSet CreateLotNumByRule(string orderKey, int count)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            DataSet dsPrintRuleDetail = new DataSet();
            DataTable dtLotNum = new DataTable();
            string printCode = string.Empty;
            string sqlPrintRuleDetail = string.Format(@"SELECT WORK_ORDER_KEY,PRINT_RULE_KEY,PRINT_CODE,PRINT_COLUM_CODE,
                                                              PRINT_COLUM_NAME,PRINT_COLUM_VALUE 
                                                           FROM dbo.POR_WO_PRD_PRINTRULE_DETAIL
                                                              WHERE WORK_ORDER_KEY = '{0}'", orderKey);
            dsPrintRuleDetail = db.ExecuteDataSet(CommandType.Text, sqlPrintRuleDetail);    //根据工单主键获取工单产品属性设置的打印信息、
            if (dsPrintRuleDetail.Tables.Count <= 0 || dsPrintRuleDetail.Tables[0].Rows.Count <= 0)
            {
                msg = "工单未设置打印打印规则信息";
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                return dsReturn;
            }

            dtLotNum = CreateLotNumByRuleCode(dsPrintRuleDetail, count);

            if (dtLotNum.TableName == "ERROR_MSG")
            {
                msg = Convert.ToString(dtLotNum.Rows[0]["Error_Msg"]);
            }

            dsReturn.Merge(dtLotNum);
            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            return dsReturn;
        }

        #endregion
        private DataTable CreateLotNumByRuleCode(DataSet dsPrintRuleDetail, int count)
        {
            DataTable dtLotnum = null;
            DataTable dtMaxLotNum = new DataTable();
            string lotNum = string.Empty;
            string orderNum = string.Empty;
            string printCode = dsPrintRuleDetail.Tables[0].Rows[0]["PRINT_CODE"].ToString().ToUpper().Trim();
            #region
            if (printCode.Equals("4") || printCode.Equals("8"))
            {
                string sqlorderNum = string.Format(@"SELECT ORDER_NUMBER FROM POR_WORK_ORDER
                                                WHERE WORK_ORDER_KEY = '{0}'
                                                AND ORDER_STATE = 'REL'", dsPrintRuleDetail.Tables[0].Rows[0]["WORK_ORDER_KEY"].ToString().Trim());
                orderNum = db.ExecuteDataSet(CommandType.Text, sqlorderNum).Tables[0].Rows[0]["ORDER_NUMBER"].ToString();
                string sql = @"SELECT GETDATE()";
                string time = Convert.ToDateTime(db.ExecuteScalar(CommandType.Text, sql)).ToString("yyyy-MM-dd");
                DataRow[] arrayDR = dsPrintRuleDetail.Tables[0].Select("PRINT_COLUM_CODE = 'Product_Model'");
                string productModel = arrayDR[0]["PRINT_COLUM_VALUE"].ToString();

                #region//判断产品型号是否为空 为空：返回异常信息
                if (string.IsNullOrEmpty(productModel))
                {
                    dtLotnum = new DataTable();
                    dtLotnum.TableName = "ERROR_MSG";
                    dtLotnum.Columns.Add("Error_Msg");
                    DataRow dr = dtLotnum.NewRow();
                    dr["Error_Msg"] = "产品类型为空，请联系工艺确认！";
                    dtLotnum.Rows.Add(dr);

                    return dtLotnum;
                }
                else
                {
                    dtLotnum = new DataTable();
                    dtLotnum.TableName = "LOT_NUMBER_LIST";
                    dtLotnum.Columns.Add("LOT_NUMBER");
                }
                #endregion

                int year = int.Parse(time.Substring(2, 2)) + 18;
                int mouth = int.Parse(time.Substring(5, 2)) + 18;
                orderNum = orderNum.Substring(0, 2) + orderNum.Substring(orderNum.Length - 3, 3);
                lotNum = orderNum + year.ToString() + mouth.ToString() + productModel;
                string sqlMaxlotNum = string.Format(@"SELECT ISNULL(MAX(LOT_NUMBER),100000) LOT_NUMBER FROM POR_LOT
                                                                WHERE LOT_NUMBER LIKE '{0}%'",
                                                                lotNum);
                dtMaxLotNum = db.ExecuteDataSet(CommandType.Text, sqlMaxlotNum).Tables[0];
                string Lastfivenum = dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Substring(dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Length - 5, 5);
                for (int i = 1; i <= count; i++)
                {
                    DataRow dr = dtLotnum.NewRow();
                    dr["LOT_NUMBER"] = lotNum + (Convert.ToInt32(Lastfivenum) + i).ToString("00000");
                    dtLotnum.Rows.Add(dr);
                }

            }
            #endregion
            #region
            else if (printCode.Equals("5"))
            {
                string sqlorderNum = string.Format(@"SELECT ORDER_NUMBER FROM POR_WORK_ORDER
                                                WHERE WORK_ORDER_KEY = '{0}'
                                                AND ORDER_STATE = 'REL'", dsPrintRuleDetail.Tables[0].Rows[0]["WORK_ORDER_KEY"].ToString().Trim());
                orderNum = db.ExecuteDataSet(CommandType.Text, sqlorderNum).Tables[0].Rows[0]["ORDER_NUMBER"].ToString();
                string sql = @"SELECT GETDATE()";
                string time = Convert.ToDateTime(db.ExecuteScalar(CommandType.Text, sql)).ToString("yyyy-MM-dd");

                dtLotnum = new DataTable();
                dtLotnum.TableName = "LOT_NUMBER_LIST";
                dtLotnum.Columns.Add("LOT_NUMBER");

                string mouth = time.Substring(5, 2);
                string day = time.Substring(8, 2);
                lotNum = "0115002HH265P" + mouth.ToString() + day.ToString();
                string sqlMaxlotNum = string.Format(@"SELECT ISNULL(MAX(LOT_NUMBER),120000) LOT_NUMBER FROM POR_LOT
                                                                WHERE LOT_NUMBER LIKE '{0}%'",
                                                                lotNum);
                dtMaxLotNum = db.ExecuteDataSet(CommandType.Text, sqlMaxlotNum).Tables[0];
                string Lastfivenum = dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Substring(dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Length - 5, 5);
                for (int i = 1; i <= count; i++)
                {
                    DataRow dr = dtLotnum.NewRow();
                    dr["LOT_NUMBER"] = lotNum + (Convert.ToInt32(Lastfivenum) + i).ToString("00000");
                    dtLotnum.Rows.Add(dr);
                }

            }
            #endregion

            #region
            else if (printCode.Equals("6"))
            {
                string type = string.Empty;
                string cellCounts = string.Empty;
                string sqlorderNum = string.Format(@" SELECT ORDER_NUMBER,PRO_ID
                                                ,b.PROMODEL_NAME
                                                 FROM POR_WORK_ORDER a
                                                 ,POR_PRODUCT b
                                                WHERE WORK_ORDER_KEY = '{0}'
                                                and a.PRO_ID=b.PRODUCT_KEY
                                                AND ORDER_STATE = 'REL'", dsPrintRuleDetail.Tables[0].Rows[0]["WORK_ORDER_KEY"].ToString().Trim());
                DataSet dsOrderNumAndProdId = db.ExecuteDataSet(CommandType.Text, sqlorderNum);
                orderNum = dsOrderNumAndProdId.Tables[0].Rows[0]["ORDER_NUMBER"].ToString();
                string cellType = dsOrderNumAndProdId.Tables[0].Rows[0]["PROMODEL_NAME"].ToString();
                if (cellType.Contains("P"))
                {
                    type = "P";
                }
                else if (cellType.Contains("M"))
                {
                    type = "M";
                }

                if (cellType.Contains("6610"))
                {
                    cellCounts = "60";
                }
                else if (cellType.Contains("6612"))
                {
                    cellCounts = "72";
                }
                string sql = @"SELECT GETDATE()";
                string time = Convert.ToDateTime(db.ExecuteScalar(CommandType.Text, sql)).ToString("yyyy-MM-dd");

                dtLotnum = new DataTable();
                dtLotnum.TableName = "LOT_NUMBER_LIST";
                dtLotnum.Columns.Add("LOT_NUMBER");

                string year = time.Substring(2, 2);
                string mouth = time.Substring(5, 2);
                string day = time.Substring(8, 2);
                lotNum = year.ToString() + mouth.ToString() + type.ToString() + "62" + cellCounts.ToString() + "CT";
                string sqlMaxlotNum = string.Format(@"SELECT ISNULL(MAX(LOT_NUMBER),100000) LOT_NUMBER FROM POR_LOT
                                                                WHERE LOT_NUMBER LIKE '{0}%'",
                                                                lotNum);
                dtMaxLotNum = db.ExecuteDataSet(CommandType.Text, sqlMaxlotNum).Tables[0];
                string Lastfivenum = dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Substring(dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Length - 5, 5);
                for (int i = 1; i <= count; i++)
                {
                    DataRow dr = dtLotnum.NewRow();
                    dr["LOT_NUMBER"] = lotNum + (Convert.ToInt32(Lastfivenum) + i).ToString("00000");
                    dtLotnum.Rows.Add(dr);
                }

            }
            #endregion
            else if (printCode.Equals("1"))
            {
                dtLotnum = new DataTable();
                dtLotnum.TableName = "LOT_NUMBER_LIST";
                dtLotnum.Columns.Add("LOT_NUMBER");
                string sqlorderNum = string.Format(@"SELECT ORDER_NUMBER FROM POR_WORK_ORDER
                                                WHERE WORK_ORDER_KEY = '{0}'
                                                AND ORDER_STATE = 'REL'", dsPrintRuleDetail.Tables[0].Rows[0]["WORK_ORDER_KEY"].ToString().Trim());
                orderNum = db.ExecuteDataSet(CommandType.Text, sqlorderNum).Tables[0].Rows[0]["ORDER_NUMBER"].ToString();
                string sql = string.Format(@"SELECT *     
                                                 FROM [dbo].[POR_WO_PRINT]
                                                 WHERE ORDER_NUMBER='{0}'"
                                                , orderNum
                                                 );
                DataTable dtWoPrint = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                sql = @"SELECT GETDATE()";
                string time = Convert.ToDateTime(db.ExecuteScalar(CommandType.Text, sql)).ToString("yyyy-MM-dd");

                int year = int.Parse(time.Substring(2, 2)) + 5;
                int mouth = int.Parse(time.Substring(5, 2)) + 5;
                int day = int.Parse(time.Substring(8, 2)) + 5;

                lotNum = dtWoPrint.Rows[0]["f001"].ToString() + dtWoPrint.Rows[0]["f002"] + dtWoPrint.Rows[0]["f003"] + dtWoPrint.Rows[0]["f004"] + dtWoPrint.Rows[0]["f005"] +
                    dtWoPrint.Rows[0]["f006"] + dtWoPrint.Rows[0]["f007"] + dtWoPrint.Rows[0]["f008"];
                string sqlMaxlotNum = string.Format(@"SELECT ISNULL(MAX(LOT_NUMBER),100000) LOT_NUMBER FROM POR_LOT
                                                                WHERE LOT_NUMBER LIKE '{0}%'",
                                                             lotNum);
                dtMaxLotNum = db.ExecuteDataSet(CommandType.Text, sqlMaxlotNum).Tables[0];
                string Lastfivenum = dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Substring(dtMaxLotNum.Rows[0]["LOT_NUMBER"].ToString().Length - 5, 5);
                for (int i = 1; i <= count; i++)
                {
                    DataRow dr = dtLotnum.NewRow();
                    dr["LOT_NUMBER"] = lotNum + (Convert.ToInt32(Lastfivenum) + i).ToString("00000");
                    dtLotnum.Rows.Add(dr);
                }
            }
            return dtLotnum;
        }

        #region ILotCreateEngine 成员

        /// <summary>
        /// 批次创建。自动生成组件序列号，设定单串焊线别设备
        /// </summary>
        /// <param name="dsParams">创批的数据集信息</param>
        /// <returns>包含创建批次结果的数据集对象</returns>
        public DataSet CeateNewLot(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sbMsg = new StringBuilder();
            try
            {
                DataTable dtMainData = dsParams.Tables[0];      //存放批次主数据
                DataTable dtAddtionData = dsParams.Tables[1];   //存放附加数据
                DataTable dtNewLot = dsParams.Tables[2];        //存在批次号和硅片供应商
                Hashtable htMainData = CommonUtils.ConvertToHashtable(dtMainData);
                Hashtable htAddtionData = CommonUtils.ConvertToHashtable(dtAddtionData);
                string workOrderNo = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                string workOrderKey = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                string storeMaterialDetailKey = Convert.ToString(htAddtionData["STORE_MATERIAL_DETAIL_KEY"]);
                string materialLotNo = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_MATERIAL_LOT]);
                string locationKey = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                string createOperationName = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
                double materialQtyPerLot = Convert.ToDouble(htMainData[POR_LOT_FIELDS.FIELD_QUANTITY_INITIAL]);
                //double materialSumQty = dtNewLot.Rows.Count * materialQtyPerLot;
                //获取工单指定的组件默认等级。
                string sql = @"SELECT b.GRADE_CODE
                            FROM POR_WORK_ORDER_ATTR a
                            INNER JOIN V_ProductGrade b ON b.GRADE_NAME=a.ATTRIBUTE_VALUE
                            WHERE a.ATTRIBUTE_NAME='DefaultGrade'
                            AND a.ISFLAG=1
                            AND a.WORK_ORDER_KEY=@workOrderKey";
                DbCommand cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "@workOrderKey", DbType.String, workOrderKey);
                object defaultGradeCode = db.ExecuteScalar(cmd);
                if (defaultGradeCode != DBNull.Value && defaultGradeCode != null)
                {
                    if (htMainData.Contains(POR_LOT_FIELDS.FIELD_PRO_LEVEL))
                    {
                        htMainData.Add(POR_LOT_FIELDS.FIELD_PRO_LEVEL, defaultGradeCode);
                    }
                    else
                    {
                        htMainData[POR_LOT_FIELDS.FIELD_PRO_LEVEL] = defaultGradeCode;
                    }
                }
                int creatingCount = 0;
                int createdCount = 0;
                //移除dtNew中的无效列
                dtNewLot.Columns.Remove("CHECK_SMALL_PACK_NUMBER");
                //增加批次。
                foreach (DataRow drLot in dtNewLot.Rows)
                {
                    creatingCount++;
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        //批次转换为大写字母
                        string lotNumber = Convert.ToString(drLot["LOT_NUMBER"]).ToUpper();
                        sql = string.Format(@"SELECT COUNT(1) FROM POR_LOT WHERE LOT_NUMBER='{0}'",
                                    lotNumber.PreventSQLInjection());
                        int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                        if (count > 0)
                        {
                            sbMsg.AppendFormat("{1}-【{0}】在数据库中已存在。\r\n", lotNumber, creatingCount);
                            continue;
                        }
                        string siSupplierLot = Convert.ToString(drLot["SI_SUPPLIER_LOT"]);
                        string smallPackNumber = Convert.ToString(drLot["SMALL_PACK_NUMBER"]); //小包条码 add by yongbing.yang 20130808
                        string lotType = Convert.ToString(htMainData[POR_LOT_FIELDS.FIELD_LOT_TYPE]);
                        //判断批次号在数据库中是否存在，如果存在返回。
                        //获取原材料数量，判断原材料数量是否满足创批需要的原材料数量。
                        sql = string.Format(@"SELECT ISNULL(SUM(a.CURRENT_QTY),0)
                                           FROM WST_STORE_MATERIAL_DETAIL a 
                                           WHERE a.STORE_MATERIAL_DETAIL_KEY='{0}'",
                                           storeMaterialDetailKey.PreventSQLInjection());
                        double currentMaterialSumQty = Convert.ToDouble(db.ExecuteScalar(CommandType.Text, sql));
                        double diffQty = currentMaterialSumQty - materialQtyPerLot;
                        if (diffQty < 0)
                        {
                            sbMsg.AppendFormat("因原材料数量不足，创建失败。\r\n", createdCount);
                            break;
                        }
                        //更新原材料数量
                        sql = string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                            SET CURRENT_QTY={0} 
                                            WHERE STORE_MATERIAL_DETAIL_KEY='{1}'",
                                            diffQty,
                                            storeMaterialDetailKey);
                        db.ExecuteNonQuery(CommandType.Text, sql);
                        //查询批次是工单的第几个批次。
                        sql = string.Format(@"SELECT NEXT_SEQ FROM POR_WORK_ORDER WHERE ORDER_NUMBER='{0}'",
                                    workOrderNo.PreventSQLInjection());
                        int nextSeq = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                        //设置批次号、硅片供应商和批次在工单中的索引号
                        htMainData[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = lotNumber;
                        htMainData[POR_LOT_FIELDS.FIELD_SI_LOT] = siSupplierLot;
                        htMainData[POR_LOT_FIELDS.FIELD_SMALL_PACK_NUMBER] = smallPackNumber;   //小包条码 add by yongbing.yang 20130808
                        htMainData[POR_LOT_FIELDS.FIELD_WORK_ORDER_SEQ] = nextSeq;
                        DataSet dsCreateLotParams = new DataSet();
                        DataTable dtCreateLotMaindata = CommonUtils.ParseToDataTable(htMainData);
                        dtCreateLotMaindata.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                        dsCreateLotParams.Tables.Add(dtCreateLotMaindata);
                        dsCreateLotParams.Tables.Add(dtAddtionData.Copy());
                        //创建批次
                        LotManagement.CreateNewLot(db, dsCreateLotParams, drLot);
                        //非组件补片批次更新工单剩余数量内。
                        if (lotType != "L")
                        {
                            //更新工单剩余数量
                            sql = string.Format(@"UPDATE POR_WORK_ORDER 
                                                  SET QUANTITY_LEFT=QUANTITY_LEFT-1,NEXT_SEQ=NEXT_SEQ+1
                                                  WHERE ORDER_NUMBER='{0}'",
                                                  workOrderNo.PreventSQLInjection());
                            db.ExecuteNonQuery(CommandType.Text, sql);
                        }
                        createdCount++;
                        ts.Complete();
                    }
                }
                if (sbMsg.Length > 0)
                {
                    sbMsg.AppendFormat("新增{0}块组件。", createdCount);
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                LogService.LogError("CeateLot Error: " + ex.Message);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        #endregion




        public DataSet GetAnNeng(string ORDER_NUMBER)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"select ATTRIBUTE_VALUE 
                            from POR_WORK_ORDER_ATTR A,POR_WORK_ORDER B 
                            where ISFLAG='1' 
                            and A.WORK_ORDER_KEY=B.WORK_ORDER_KEY 
                            AND B.ORDER_NUMBER='{0}' 
                             and  ATTRIBUTE_NAME ='PalletAddLetter'", ORDER_NUMBER);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("LotOperationEngine.GetAnNeng Error: " + ex.Message);
            }
            return dsReturn;


        }
    }
}

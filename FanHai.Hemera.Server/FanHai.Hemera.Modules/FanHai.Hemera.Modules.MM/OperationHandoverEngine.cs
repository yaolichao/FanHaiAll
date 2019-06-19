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
using FanHai.Hemera.Share.Interface.WarehouseManagement;
namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 工序交接班数据的操作类。
    /// </summary>
    public class OperationHandoverEngine : AbstractEngine, IOperationHandoverEngine
    {
        private Database db = null; //数据库对象。
        public static string key1 = "";
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationHandoverEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 根据用户拥有权限的工厂车间名称和工序名称获取对应的工序交接班的记录
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="stores"></param>
        /// <returns></returns>
        public DataSet GetOperationHandoverBySAndF(string operations, DataTable dt)
        {
            DataSet dsReturn = new DataSet();
            string sqlCondition = "";
            string sqlCondition1 = "";
            string allFacRoom = string.Empty;
            try
            {
                string facRoom = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {//将工厂车间名称用，号隔开
                    facRoom += dt.Rows[i][1].ToString() + ",";
                }
                if (facRoom.Length > 0)
                {
                    allFacRoom = facRoom.Substring(0, facRoom.Length - 1);
                }

                string sql = "";

                if (allFacRoom.Length > 0)
                {
                    sql = @"SELECT W.OPERATION_HANDOVER_KEY,W.LOCATION_KEY,L.LOCATION_NAME,W.OPERATION_KEY,V.ROUTE_OPERATION_NAME,W.SEND_SHIFT_VALUE,W.RECEIVE_SHIFT_VALUE,W.HANDOVER_DATE,
                                (CASE W.STATUS WHEN 0 THEN '未交班' WHEN 1 THEN '已交班' WHEN 2 THEN '已接班' END) AS STATUS,W.SEND_OPERATOR,W.RECEIVE_OPERATOR
                                FROM
                                WST_OPERATION_HANDOVER W,
                                FMM_LOCATION L,
                                POR_ROUTE_OPERATION_VER V
                                WHERE W.LOCATION_KEY = L.LOCATION_KEY 
                                AND W.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY";
                    sqlCondition = UtilHelper.BuilderWhereConditionString("L.LOCATION_NAME", allFacRoom.Split(','));
                    sql += sqlCondition;
                    sqlCondition1 = UtilHelper.BuilderWhereConditionString("V.ROUTE_OPERATION_NAME", operations.Split(','));
                    sql += sqlCondition1;
                    sql += " ORDER BY W.HANDOVER_DATE DESC";
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                }

            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperationHandoverBySAndF Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 查询界面返回参数
        /// </summary>
        /// <param name="_lupFactoryRoom">工厂车间</param>
        /// <param name="_cmbGongXuName">工序名称</param>
        /// <param name="_lupJiaoBanShife">交接班次</param>
        /// <param name="_lupJieBanShife">接班班次</param>
        /// <param name="_timJiaoBanStart">交接时间起</param>
        /// <param name="_timJiaoBanEnd">交接时间末</param>
        /// <param name="_lupZhuangTai">状态</param>
        /// <param name="operations">用户拥有权限的工序</param>
        /// <param name="dt">工厂车间</param>
        /// <returns>返回查询结果</returns>
        public DataSet GetOperationHandoverByReturn(string _lupFactoryRoom, string _cmbGongXuName, string _lupJiaoBanShife,
            string _lupJieBanShife, string _timJiaoBanStart, string _timJiaoBanEnd, string _lupZhuangTai, string operations, DataTable dt)
        {
            string sqlCondition = "";
            string sqlCondition1 = "";
            string allFacRoom = string.Empty;

            DataSet dsReturn = new DataSet();
            try
            {
                string facRoom = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {//将工厂车间名称用，号隔开
                    facRoom += dt.Rows[i][1].ToString() + ",";
                }
                if (facRoom.Length > 0)
                {
                    allFacRoom = facRoom.Substring(0, facRoom.Length - 1);
                }


                string sql = @"SELECT W.OPERATION_HANDOVER_KEY,W.LOCATION_KEY,L.LOCATION_NAME,W.OPERATION_KEY,V.ROUTE_OPERATION_NAME,W.SEND_SHIFT_VALUE,W.RECEIVE_SHIFT_VALUE,W.HANDOVER_DATE,
                                (CASE W.STATUS WHEN 0 THEN '未交班' WHEN 1 THEN '已交班' WHEN 2 THEN '已接班' END)AS STATUS,W.SEND_OPERATOR,W.RECEIVE_OPERATOR
                                FROM
                                WST_OPERATION_HANDOVER W LEFT JOIN
                                FMM_LOCATION L ON W.LOCATION_KEY = L.LOCATION_KEY 
                                LEFT JOIN 
                                POR_ROUTE_OPERATION_VER V ON W.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                WHERE 1=1";
                if (_lupJiaoBanShife != "")
                {
                    sql += string.Format(" AND W.SEND_SHIFT_VALUE='{0}'", _lupJiaoBanShife);
                }
                if (_lupJieBanShife != "")
                {
                    sql += string.Format(" AND W.RECEIVE_SHIFT_VALUE='{0}'", _lupJieBanShife);
                }
                if (_lupZhuangTai != "")
                {
                    if (_lupZhuangTai == "未交班")
                    {
                        sql += string.Format(" AND W.STATUS=0");
                    }
                    if (_lupZhuangTai == "已交班")
                    {
                        sql += string.Format(" AND W.STATUS=1");
                    }
                    if (_lupZhuangTai == "已接班")
                    {
                        sql += string.Format(" AND W.STATUS=2");
                    }
                }
                else
                {
                    sql += string.Format(" AND W.STATUS!=0");
                }
                if (_timJiaoBanStart != "")
                {
                    string dtTimeStart = Convert.ToDateTime(_timJiaoBanStart).ToShortDateString();
                    sql += string.Format(" AND W.HANDOVER_DATE>='{0}')", dtTimeStart);
                }
                if (_timJiaoBanEnd != "")
                {
                    string dtTimeEnd = Convert.ToDateTime(_timJiaoBanEnd).ToShortDateString();
                    sql += string.Format(" AND W.HANDOVER_DATE<='{0}'", dtTimeEnd);
                }
                if (_lupFactoryRoom == "")
                {
                    sqlCondition = UtilHelper.BuilderWhereConditionString("L.LOCATION_NAME", allFacRoom.Split(','));
                    sql += sqlCondition;
                }
                else
                {
                    sql += string.Format(" AND L.LOCATION_NAME='{0}'", _lupFactoryRoom);
                }
                if (_cmbGongXuName == "")
                {
                    sqlCondition1 = UtilHelper.BuilderWhereConditionString("V.ROUTE_OPERATION_NAME", operations.Split(','));
                    sql += sqlCondition1;
                }
                else
                {
                    sql += string.Format(" AND V.ROUTE_OPERATION_NAME='{0}'", _cmbGongXuName);
                }
                sql += " ORDER BY W.HANDOVER_DATE DESC";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetOperationHandoverByReturn Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据当前班次和当前日期获取上一班次和上一班的交班日期。根据上一班次、工厂车间、工序名称、上一班的交班日期获取上一班的交班记录
        /// </summary>
        /// <param name="_lupShift">当前班次</param>
        /// <param name="_lupGongXu">工序名称</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        /// <returns></returns>
        public DataSet GetShangBanShift(string _lupShift, string _lupGongXu, string _lupFacRoomKey)
        {
            DataSet dsShangBanBanCiDay = new DataSet();
            DataSet dsReturn = new DataSet();
            string allFacRoom = string.Empty;
            try
            {
                string sql = "";
                string sql1 = "";
                string sql2 = "";

                sql = string.Format("SELECT SHIFT_VALUE,DAY FROM (SELECT TOP 1 * FROM CAL_SCHEDULE_DAY WHERE GETDATE()>ENDTIME AND SHIFT_VALUE!='{0}' ORDER BY ENDTIME DESC) T", _lupShift);
                dsShangBanBanCiDay = db.ExecuteDataSet(CommandType.Text, sql);

                sql1 = string.Format("SELECT ROUTE_OPERATION_VER_KEY FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'", _lupGongXu);
                string strOperationKey = db.ExecuteScalar(CommandType.Text, sql1).ToString().Trim();

                sql2 = string.Format("SELECT STATUS FROM WST_OPERATION_HANDOVER WHERE LOCATION_KEY = '{0}' AND OPERATION_KEY = '{1}' AND SEND_SHIFT_VALUE = '{2}' AND HANDOVER_DATE = '{3}'", _lupFacRoomKey, strOperationKey, dsShangBanBanCiDay.Tables[0].Rows[0][0].ToString(), dsShangBanBanCiDay.Tables[0].Rows[0][1].ToString());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql2);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShangBanShift Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// //如果上一班次的工序交接班记录不存在，继续根据工厂车间、工序名称判断是否存在工序交接班记录
        /// </summary>
        /// <param name="_lupGongXu">工序</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        /// <returns></returns>
        public DataSet GetShangBan(string _lupGongXu, string _lupFacRoomKey)
        {
            DataSet dsShangBanBanCiDay = new DataSet();
            DataSet dsReturn = new DataSet();
            string allFacRoom = string.Empty;
            try
            {

                string sql1 = "";
                string sql2 = "";

                sql1 = string.Format("SELECT ROUTE_OPERATION_VER_KEY FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'", _lupGongXu);
                string strOperationKey = db.ExecuteScalar(CommandType.Text, sql1).ToString();

                sql2 = string.Format("SELECT * FROM WST_OPERATION_HANDOVER WHERE LOCATION_KEY = '{0}' AND OPERATION_KEY = '{1}'", strOperationKey, _lupFacRoomKey);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql2);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShangBan Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 通过工序交接主键获取物料信息和在制品信息  MODI BY CHAO.PANG
        /// </summary>
        /// <param name="key">工序交接班主键</param>
        /// <returns>物料信息表在制品信息表</returns>
        public DataSet GetWipAndMatByKey(string key)
        {
            DataSet dsGetWipByKey = new DataSet();
            DataSet dsGetMatByKey = new DataSet();
            DataSet dsReturn = new DataSet();
            string allFacRoom = string.Empty;
            try
            {

                string sql1 = "";
                string sql2 = "";

                sql1 = string.Format(@" SELECT W.OPERATION_HANDOVER_DETAIL_KEY,W.OPERATION_HANDOVER_KEY,M.STORE_MATERIAL_KEY,P.MATERIAL_NAME,P.MATERIAL_CODE,
                                        P.UNIT,S.STORE_NAME,W.INIT_QTY,W.IN_QTY_1,W.OUT_QTY_1,W.OUT_QTY_2,W.END_QTY
                                        FROM 
                                        WST_OPERATION_HANDOVER_MAT W
                                        LEFT JOIN WST_STORE_MATERIAL M ON
                                        W. STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                        LEFT JOIN  WST_STORE S ON 
                                        M.STORE_KEY = S.STORE_KEY
                                        LEFT JOIN POR_MATERIAL P ON 
                                        M.MATERIAL_KEY = P.MATERIAL_KEY 
                                        WHERE W.OPERATION_HANDOVER_KEY = '{0}'", key);
                dsGetMatByKey = db.ExecuteDataSet(CommandType.Text, sql1);

                sql2 = string.Format(@"SELECT 
                                        W.OPERATION_HANDOVER_KEY,W.OPERATION_HANDOVER_DETAIL_KEY,O.WORK_ORDER_KEY,O.ORDER_NUMBER,P.PART_NAME,P.PART_DESC,(P.PART_MODULE||P.PART_TYPE  ) AS PART_TYPEMODULE,
                                        W.INIT_QTY,W.IN_QTY_1,W.OUT_QTY_1,W.OUT_QTY_2,W.END_QTY
                                        FROM WST_OPERATION_HANDOVER_WIP W 
                                        LEFT JOIN POR_WORK_ORDER O 
                                        ON W.ORDER_NUMBER = O.ORDER_NUMBER
                                        LEFT JOIN POR_PART P ON
                                        O.PART_KEY = P.PART_KEY
                                        WHERE W.OPERATION_HANDOVER_KEY = '{0}'", key);
                dsGetWipByKey = db.ExecuteDataSet(CommandType.Text, sql2);
                dsGetMatByKey.Tables[0].TableName = "MatByKey";
                dsReturn.Merge(dsGetMatByKey.Tables[0]);

                dsGetWipByKey.Tables[0].TableName = "WipByKey";
                dsReturn.Merge(dsGetWipByKey.Tables[0]);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWipAndMatByKey Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取工序交接记录
        /// </summary>
        /// <param name="shift">当前班别</param>
        /// <param name="operation">工序名称</param>
        /// <param name="factRoom">工厂车间主键</param>
        /// <returns></returns>
        public DataSet GetDangQianShiftHandover(string shift, string operation, string factRoom)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsDangQianShiftHandover = new DataSet();
            try
            {

                string sql1 = "";
                string sql2 = "";
                string sql3 = "";

                sql1 = string.Format(@"SELECT TOP 1 DAY FROM 
                                        (SELECT * FROM CAL_SCHEDULE_DAY WHERE GETDATE()<ENDTIME AND SHIFT_VALUE='{0}' ORDER BY ENDTIME ASC) T", 
                                      shift);
                DataSet strDay = db.ExecuteDataSet(CommandType.Text, sql1);

                sql2 = string.Format("SELECT ROUTE_OPERATION_VER_KEY FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'", operation);
                string strOperationKey = db.ExecuteScalar(CommandType.Text, sql2).ToString().Trim();


                sql3 = string.Format(@"SELECT W.OPERATION_HANDOVER_KEY,L.LOCATION_NAME,V.ROUTE_OPERATION_NAME,W.SEND_SHIFT_VALUE,W.RECEIVE_SHIFT_VALUE,W.HANDOVER_DATE,
                                        (CASE W.STATUS WHEN 0 THEN '未交班' WHEN 1 THEN '已交班' WHEN 2 THEN '已接班' END) AS STATUS,W.SEND_OPERATOR,W.RECEIVE_OPERATOR
                                        FROM
                                        WST_OPERATION_HANDOVER W LEFT JOIN 
                                        FMM_LOCATION L ON W.LOCATION_KEY = L.LOCATION_KEY 
                                        LEFT JOIN POR_ROUTE_OPERATION_VER V ON
                                        W.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY
                                        WHERE 1=1 AND W.LOCATION_KEY = '{0}' AND W.OPERATION_KEY = '{1}' 
                                        AND W.SEND_SHIFT_VALUE = '{2}' AND W.HANDOVER_DATE ='{3}'", factRoom, strOperationKey, shift, strDay.Tables[0].Rows[0][0].ToString().Trim());
                dsDangQianShiftHandover = db.ExecuteDataSet(CommandType.Text, sql3);

                strDay.Tables[0].TableName = "DAY";
                dsReturn.Merge(strDay.Tables[0]);

                dsDangQianShiftHandover.Tables[0].TableName = "ShiftHandover";
                dsReturn.Merge(dsDangQianShiftHandover.Tables[0]);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShangBanShift Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 没有记录在工序交接班中插入一条记录
        /// </summary>
        /// <param name="dsSetIn"></param>
        /// <returns></returns>
        public DataSet InsertHandOver(DataSet dsSetIn)
        {
            const string CONST_INSERT_1 = @" INSERT INTO WST_OPERATION_HANDOVER
                                            (OPERATION_HANDOVER_KEY,LOCATION_KEY ,OPERATION_KEY ,
                                            SEND_SHIFT_VALUE, HANDOVER_DATE,STATUS ,SEND_OPERATOR ,CREATE_TIME,CREATE_TIMEZONE,CREATOR)
                                            VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}',GETDATE(),'{7}','{8}')";
            string sqlInsert = string.Empty;

            DataSet dsReturn = new DataSet();
            DataTable dtHash = dsSetIn.Tables["HASH"];
            Hashtable hsTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

            string sql2 = string.Format("SELECT ROUTE_OPERATION_VER_KEY FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'", hsTable["OPERATIONNAME"].ToString());
            string strOperationKey = db.ExecuteScalar(CommandType.Text, sql2).ToString();


            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try 
            {
                string key = System.Guid.NewGuid().ToString();
                string strInsertInToHandover = string.Format(CONST_INSERT_1,
                    key,                                                //主键
                    hsTable["LOCATIOMKEY"].ToString(),                  //工厂车间主键 
                    strOperationKey,                                    //工序主键
                    hsTable["SENDSHIFTVALUE"].ToString(),               //交班班次
                    hsTable["DAY"].ToString(),                          //交班日期
                    hsTable["STATUS"].ToString(),                       //状态
                    hsTable["SENDOPERATOR"].ToString(),                 //交班人员   
                    hsTable["CREATE_TIMEZONE"].ToString(),              //创建时区
                    hsTable["CREATOR"].ToString()                       //创建人
                    );

                db.ExecuteNonQuery(CommandType.Text, strInsertInToHandover);

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("InsertHandOver Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        ///通过根据工序和工厂车间获取所有线上仓中的物料信息（WST_STORE,WST_STORE_MATERIAL)插入到WST_OPERATION_HANDOVER_MAT中
        ///（数量全部设置为0）。根据工序和工厂车间获取所有工单的在制品信息（POR_LOT,POR_WORK_ORDER,WIP_TRANSACTION)插入到
        ///WST_OPERATION_HANDOVER_WIP中（数量全部设置为0）
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        /// <param name="actRoom">工厂车间</param>
        /// <param name="operation">工序名称</param>
        public DataSet InsertHandOverMatAndWip(string handOverKey, string actRoom, string operation)
        {
            DataSet dsReturn = new DataSet(); 

            //根据工序和工厂车间获取所有线上仓中的物料信息（WST_STORE,WST_STORE_MATERIAL)插入到WST_OPERATION_HANDOVER_MAT中（数量全部设置为0）
            const string CONST_INSERT_1 = @" INSERT INTO WST_OPERATION_HANDOVER_MAT
                                             (OPERATION_HANDOVER_DETAIL_KEY,OPERATION_HANDOVER_KEY,STORE_MATERIAL_KEY,
                                              INIT_QTY,IN_QTY_1,OUT_QTY_1,OUT_QTY_2,END_QTY)
                                             VALUES('{0}','{1}','{2}',0,0,0,0,0)";
            //根据工序和工厂车间获取所有工单的在制品信息（POR_LOT,POR_WORK_ORDER,WIP_TRANSACTION)插入到WST_OPERATION_HANDOVER_WIP中（数量全部设置为0）
            const string CONST_INSERT_2 = @" INSERT INTO WST_OPERATION_HANDOVER_WIP
                                             (OPERATION_HANDOVER_DETAIL_KEY,OPERATION_HANDOVER_KEY,ORDER_NUMBER,
                                              INIT_QTY,IN_QTY_1,OUT_QTY_1,OUT_QTY_2,OUT_QTY_3,END_QTY)
                                             VALUES('{0}','{1}','{2}',0,0,0,0,0,0)";
            //查询线上仓物料表的主键为插入做准备
            string sql2 = string.Format(@"SELECT M.STORE_MATERIAL_KEY FROM
                                            WST_STORE S,WST_STORE_MATERIAL M
                                            WHERE
                                            S.STORE_KEY = M.STORE_KEY 
                                            AND
                                            S.OPERATION_NAME ='{0}' AND S.LOCATION_KEY = '{1}'", operation, actRoom);
            DataSet dsStoreMnaterialKey = db.ExecuteDataSet(CommandType.Text, sql2);


            string sqlFacName = string.Format("SELECT LOCATION_NAME FROM FMM_LOCATION WHERE LOCATION_KEY = '{0}'", actRoom);
            string strFacName = db.ExecuteScalar(CommandType.Text, sqlFacName).ToString();

            string sql3 = string.Format(@"SELECT  O.ORDER_NUMBER FROM 
                                            POR_ROUTE_STEP S,
                                            POR_LOT L,
                                            POR_WORK_ORDER O,
                                            WIP_TRANSACTION T
                                            WHERE 
                                            S.ROUTE_STEP_KEY = L.CUR_STEP_VER_KEY
                                            AND L.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            AND T.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            AND T.PIECE_KEY = L.LOT_KEY
                                            AND S.ROUTE_OPERATION_NAME = '{0}'
                                            AND O.FACTORY_NAME ='{1}'", operation, strFacName);
            DataSet dsOrderNumber = db.ExecuteDataSet(CommandType.Text, sql3);


            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                //循环线上仓物料表的主键插入到工序交接班物料明细表中
                for(int i=0;i<dsStoreMnaterialKey.Tables[0].Rows.Count;i++)
                {
                     key1 = System.Guid.NewGuid().ToString();
                     string strInsertInToHandover = string.Format(CONST_INSERT_1,
                        key1,                                                //主键
                        handOverKey,                                         //工序交接班主键
                        dsStoreMnaterialKey.Tables[0].Rows[i][0]            //线上仓物料主键
                        );

                     db.ExecuteNonQuery(CommandType.Text, strInsertInToHandover);
                }

                //循环在制品表的主键插入到工序交接班在制品明细表中
                for (int i = 0; i < dsOrderNumber.Tables[0].Rows.Count; i++)
                {
                    key1 = System.Guid.NewGuid().ToString();
                    string strInsertInToHandover = string.Format(CONST_INSERT_2,
                       key1,                                                //主键
                       handOverKey,                                         //工序交接班主键
                       dsOrderNumber.Tables[0].Rows[i][0]                  //工单号
                       );

                    db.ExecuteNonQuery(CommandType.Text, strInsertInToHandover);
                }

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("InsertHandOver Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }


        /// <summary>
        /// 通过工序交接班的主键然后获取WIP和物料的数量更新到表中
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        public DataSet UpdateHandOverMatAndWip(string handOverKey)
        {

            DataSet dsReturn = new DataSet();

            //更新退料数量
            const string CONST_UPDATE_TUILIAO = @"UPDATE WST_OPERATION_HANDOVER_MAT SET
                                                    OUT_QTY_1={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";
            //更新来料数量
            const string CONST_UPDATE_LAILIAO = @"UPDATE WST_OPERATION_HANDOVER_MAT SET
                                                    IN_QTY_1={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新耗用数量
            const string CONST_UPDATE_HAOYONG = @"UPDATE WST_OPERATION_HANDOVER_MAT SET
                                                    OUT_QTY_2={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新进站数量
            const string CONST_UPDATE_JINZHAN = @"UPDATE WST_OPERATION_HANDOVER_WIP SET
                                                    QUANTITY_OUT={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新报废数量
            const string CONST_UPDATE_BAOFEI = @"UPDATE WST_OPERATION_HANDOVER_WIP SET
                                                    QUANTITY_OUT={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新返工数量
            const string CONST_UPDATE_FANGONG = @"UPDATE WST_OPERATION_HANDOVER_WIP SET
                                                    QUANTITY_OUT={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新出站数量
            const string CONST_UPDATE_CHUZHAN = @"UPDATE WST_OPERATION_HANDOVER_WIP SET
                                                    QUANTITY_OUT={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";


            string sql1 = string.Format(@"SELECT H.OPERATION_HANDOVER_DETAIL_KEY,SUM(MENGE)
                                            FROM
                                            WST_OPERATION_HANDOVER_MAT H 
                                            LEFT JOIN 
                                            WST_STORE_MATERIAL M ON H.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN 
                                            WST_STORE_MATERIAL_DETAIL D ON D.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN
                                            WST_TL_ZMMLPO Z ON Z.STORE_MATERIAL_DETAIL_KEY =D.STORE_MATERIAL_DETAIL_KEY
                                            WHERE H.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY H.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsTuiLiao = db.ExecuteDataSet(CommandType.Text, sql1);

            string sql2 = string.Format(@"SELECT H.OPERATION_HANDOVER_DETAIL_KEY,SUM(ERFMG)
                                            FROM
                                            WST_OPERATION_HANDOVER_MAT H
                                            LEFT JOIN 
                                            WST_STORE_MATERIAL M ON H.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN 
                                            WST_STORE_MATERIAL_DETAIL D ON D.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN
                                            WST_SAP_ISSURE I ON I.STORE_MATERIAL_DETAIL_KEY =D.STORE_MATERIAL_DETAIL_KEY
                                            WHERE H.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY H.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsLaiLiao = db.ExecuteDataSet(CommandType.Text, sql2);

            string sql3 = string.Format(@"SELECT H.OPERATION_HANDOVER_DETAIL_KEY,SUM(USED_QTY)
                                            FROM
                                            WST_OPERATION_HANDOVER_MAT H 
                                            LEFT JOIN
                                            WST_STORE_MATERIAL M ON H.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN
                                            WST_STORE_MATERIAL_DETAIL D ON D.STORE_MATERIAL_KEY = M.STORE_MATERIAL_KEY
                                            LEFT JOIN 
                                            WST_MATERIAL_USED_DETAIL U ON U.STORE_MATERIAL_DETAIL_KEY =D.STORE_MATERIAL_DETAIL_KEY
                                            WHERE H.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY H.OPERATION_HANDOVER_DETAIL_KEY ", handOverKey);
            DataSet dsHaoYong = db.ExecuteDataSet(CommandType.Text, sql3);

            string sql4 = string.Format(@"SELECT W.OPERATION_HANDOVER_DETAIL_KEY,SUM(QUANTITY_OUT) FROM 
                                            WST_OPERATION_HANDOVER_WIP W LEFT JOIN
                                            POR_WORK_ORDER O ON W.ORDER_NUMBER = O.ORDER_NUMBER
                                            LEFT JOIN 
                                            WIP_TRANSACTION T ON T.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            LEFT JOIN 
                                            POR_LOT L ON T.PIECE_KEY = L.LOT_KEY AND L.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            WHERE T.UNDO_FLAG = 0
                                            AND T.ACTIVITY = 'TRACKIN'
                                            AND W.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY W.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsJinZhan = db.ExecuteDataSet(CommandType.Text, sql4);

            string sql5 = string.Format(@"SELECT W.OPERATION_HANDOVER_DETAIL_KEY,SUM(QUANTITY_OUT) FROM 
                                            WST_OPERATION_HANDOVER_WIP W LEFT JOIN
                                            POR_WORK_ORDER O ON W.ORDER_NUMBER = O.ORDER_NUMBER
                                            LEFT JOIN 
                                            WIP_TRANSACTION T ON T.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            LEFT JOIN 
                                            POR_LOT L ON T.PIECE_KEY = L.LOT_KEY AND L.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            WHERE T.UNDO_FLAG = 0
                                            AND T.ACTIVITY = 'TRACKOUT'
                                            AND W.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY W.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsChuZhan = db.ExecuteDataSet(CommandType.Text, sql5);

            string sql6 = string.Format(@"SELECT W.OPERATION_HANDOVER_DETAIL_KEY,SUM(QUANTITY_OUT) FROM 
                                            WST_OPERATION_HANDOVER_WIP W LEFT JOIN
                                            POR_WORK_ORDER O ON W.ORDER_NUMBER = O.ORDER_NUMBER
                                            LEFT JOIN 
                                            WIP_TRANSACTION T ON T.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            LEFT JOIN 
                                            POR_LOT L ON T.PIECE_KEY = L.LOT_KEY AND L.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            WHERE T.UNDO_FLAG = 0
                                            AND T.ACTIVITY = 'SETLOSSBONUS'
                                            AND W.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY W.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsBaoFei = db.ExecuteDataSet(CommandType.Text, sql6);

            string sql7 = string.Format(@"SELECT W.OPERATION_HANDOVER_DETAIL_KEY,SUM(QUANTITY_OUT) FROM 
                                            WST_OPERATION_HANDOVER_WIP W LEFT JOIN
                                            POR_WORK_ORDER O ON W.ORDER_NUMBER = O.ORDER_NUMBER
                                            LEFT JOIN 
                                            WIP_TRANSACTION T ON T.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            LEFT JOIN 
                                            POR_LOT L ON T.PIECE_KEY = L.LOT_KEY AND L.WORK_ORDER_KEY = O.WORK_ORDER_KEY
                                            WHERE T.UNDO_FLAG = 0
                                            AND T.ACTIVITY = 'REWORK'
                                            AND W.OPERATION_HANDOVER_KEY = '{0}'
                                            GROUP BY W.OPERATION_HANDOVER_DETAIL_KEY", handOverKey);
            DataSet dsFanGong = db.ExecuteDataSet(CommandType.Text, sql7);


            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                //循环更新退料数量
                for (int i = 0; i < dsTuiLiao.Tables[0].Rows.Count; i++)
                {
                    string strUpdateMatTuiLiao = string.Format(CONST_UPDATE_TUILIAO,
                       dsTuiLiao.Tables[0].Rows[i][1],           //退料数量
                       dsTuiLiao.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateMatTuiLiao);
                }

                //循环更新来料数量
                for (int i = 0; i < dsLaiLiao.Tables[0].Rows.Count; i++)
                {
                    string strUpdateMatLaiLiao = string.Format(CONST_UPDATE_LAILIAO,
                       dsLaiLiao.Tables[0].Rows[i][1],           //来料数量
                       dsLaiLiao.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateMatLaiLiao);
                }

                //循环更新耗用数量
                for (int i = 0; i < dsHaoYong.Tables[0].Rows.Count; i++)
                {
                    string strUpdateMatHaoYong = string.Format(CONST_UPDATE_HAOYONG,
                       dsHaoYong.Tables[0].Rows[i][1],           //耗用数量
                       dsHaoYong.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateMatHaoYong);
                }

                //循环更新进站数量
                for (int i = 0; i < dsJinZhan.Tables[0].Rows.Count; i++)
                {
                    string strUpdateWipJinZhan = string.Format(CONST_UPDATE_JINZHAN,
                       dsJinZhan.Tables[0].Rows[i][1],           //进站数量
                       dsJinZhan.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateWipJinZhan);
                }

                //循环更新报废数量
                for (int i = 0; i < dsBaoFei.Tables[0].Rows.Count; i++)
                {
                    string strUpdateWipBaoFei = string.Format(CONST_UPDATE_BAOFEI,
                       dsBaoFei.Tables[0].Rows[i][1],           //报废数量
                       dsBaoFei.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateWipBaoFei);
                }

                //循环更新返工数量
                for (int i = 0; i < dsFanGong.Tables[0].Rows.Count; i++)
                {
                    string strUpdateWipFanGong = string.Format(CONST_UPDATE_FANGONG,
                       dsFanGong.Tables[0].Rows[i][1],           //返工数量
                       dsFanGong.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateWipFanGong);
                }

                //循环更新出站数量
                for (int i = 0; i < dsChuZhan.Tables[0].Rows.Count; i++)
                {
                    string strUpdateWipChuZhan = string.Format(CONST_UPDATE_CHUZHAN,
                       dsChuZhan.Tables[0].Rows[i][1],           //出站数量
                       dsChuZhan.Tables[0].Rows[i][0]            //线上仓物料主键
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateWipChuZhan);
                }

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpdateHandOverMatAndWip Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 通过工序交接班的主键更新期末数据
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        public DataSet UpdateHandOverMatAndWipQiMoShuLiang(string handOverKey)
        {
            DataSet dsReturn = new DataSet();

            //更新mat期末数量
            const string CONST_UPDATE_MATQIMO = @"UPDATE WST_OPERATION_HANDOVER_MAT SET
                                                    END_QTY={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            //更新wip期末数量
            const string CONST_UPDATE_WIPQIMO = @"UPDATE WST_OPERATION_HANDOVER_WIP SET
                                                    END_QTY={0} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{1}'";

            string sql1 = string.Format(@"SELECT OPERATION_HANDOVER_DETAIL_KEY,INIT_QTY,IN_QTY_1,OUT_QTY_1,OUT_QTY_2
                                            FROM
                                            WST_OPERATION_HANDOVER_MAT
                                            WHERE 
                                            OPERATION_HANDOVER_KEY = '{0}'", handOverKey);
            DataSet dsOperationHandoverMatKey = db.ExecuteDataSet(CommandType.Text, sql1);


            string sql2 = string.Format(@"SELECT OPERATION_HANDOVER_DETAIL_KEY,INIT_QTY,IN_QTY_1,OUT_QTY_1,OUT_QTY_2,OUT_QTY_3
                                            FROM
                                            WST_OPERATION_HANDOVER_WIP
                                            WHERE 
                                            OPERATION_HANDOVER_KEY = '{0}'", handOverKey);
            DataSet dsOperationHandoverWipKey = db.ExecuteDataSet(CommandType.Text, sql2);

            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                //循环更新数量
                for (int i = 0; i < dsOperationHandoverMatKey.Tables[0].Rows.Count; i++)
                {
                    string strUpdateQiMoShuLiang = string.Format(CONST_UPDATE_WIPQIMO,
                       Convert.ToDouble(dsOperationHandoverMatKey.Tables[0].Rows[i][1].ToString()) + Convert.ToDouble(dsOperationHandoverMatKey.Tables[0].Rows[i][2].ToString()) - Convert.ToDouble(dsOperationHandoverMatKey.Tables[0].Rows[i][3].ToString()) - Convert.ToDouble(dsOperationHandoverMatKey.Tables[0].Rows[i][4].ToString()),
                       dsOperationHandoverMatKey.Tables[0].Rows[i][0]        
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateQiMoShuLiang);
                }

                //循环更新数量
                for (int i = 0; i < dsOperationHandoverWipKey.Tables[0].Rows.Count; i++)
                {
                    string strUpdateQiMoShuLiang = string.Format(CONST_UPDATE_MATQIMO,
                       Convert.ToDouble(dsOperationHandoverWipKey.Tables[0].Rows[i][1].ToString()) + Convert.ToDouble(dsOperationHandoverWipKey.Tables[0].Rows[i][2].ToString()) - Convert.ToDouble(dsOperationHandoverWipKey.Tables[0].Rows[i][3].ToString()) - Convert.ToDouble(dsOperationHandoverWipKey.Tables[0].Rows[i][4].ToString()) - Convert.ToDouble(dsOperationHandoverWipKey.Tables[0].Rows[i][5].ToString()),
                       dsOperationHandoverWipKey.Tables[0].Rows[i][0]
                       );

                    db.ExecuteNonQuery(CommandType.Text, strUpdateQiMoShuLiang);
                }


                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpdateHandOverMatAndWipQiMoShuLiang Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 获取上一班的交接记录
        /// </summary>
        /// <param name="shift">当前班次</param>
        /// <param name="operation">工序名称</param>
        /// <param name="factRoom">工厂主键</param>
        /// <returns></returns>
        public DataSet GetShangYiBanHandOver(string shift, string operation, string factRoom)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsDangQianShiftHandover = new DataSet();
            try
            {

                string sql1 = "";
                string sql2 = "";
                string sql3 = "";

                sql1 = string.Format(@"SELECT TOP 1 SHIFT_VALUE,DAY FROM 
                                        (SELECT * FROM CAL_SCHEDULE_DAY WHERE GETDATE()>ENDTIME AND SHIFT_VALUE!='{0}' 
                                        ORDER BY ENDTIME DESC) T", shift);
                DataSet strDay = db.ExecuteDataSet(CommandType.Text, sql1);

                sql2 = string.Format("SELECT ROUTE_OPERATION_VER_KEY FROM POR_ROUTE_OPERATION_VER WHERE ROUTE_OPERATION_NAME = '{0}'", operation);
                string strOperationKey = db.ExecuteScalar(CommandType.Text, sql2).ToString().Trim();


                sql3 = string.Format(@"SELECT W.OPERATION_HANDOVER_KEY,L.LOCATION_NAME,V.ROUTE_OPERATION_NAME,W.SEND_SHIFT_VALUE,W.RECEIVE_SHIFT_VALUE,W.HANDOVER_DATE,
                                        (CASE W.STATUS WHEN 0 THEN '未交班' WHEN 1 THEN '已交班' WHEN 2 THEN '已接班' END) AS STATUS,W.SEND_OPERATOR,W.RECEIVE_OPERATOR
                                        FROM
                                        WST_OPERATION_HANDOVER W,
                                        FMM_LOCATION L,
                                        POR_ROUTE_OPERATION_VER V
                                        WHERE W.LOCATION_KEY = L.LOCATION_KEY 
                                        AND W.OPERATION_KEY = V.ROUTE_OPERATION_VER_KEY AND W.LOCATION_KEY = '{0}' AND W.OPERATION_KEY = '{1}' 
                                        AND W.SEND_SHIFT_VALUE = '{2}' AND W.HANDOVER_DATE = '{3}'", factRoom, strOperationKey, strDay.Tables[0].Rows[0][0].ToString().Trim(), strDay.Tables[0].Rows[0][1].ToString().Trim());
                dsDangQianShiftHandover = db.ExecuteDataSet(CommandType.Text, sql3);

                strDay.Tables[0].TableName = "DAY";
                dsReturn.Merge(strDay.Tables[0]);

                dsDangQianShiftHandover.Tables[0].TableName = "ShiftHandover";
                dsReturn.Merge(dsDangQianShiftHandover.Tables[0]);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetShangYiBanHandOver Error: " + ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 交班后更新工序交接班内容和MAT内容
        /// </summary>
        /// <param name="dsSetIn">Hash表和界面MAT数据信息</param>
        public DataSet UpdateWipMatHandOverBySaveJiaoban(DataSet dsSetIn)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtMat=dsSetIn.Tables["WST_OPERATION_HANDOVER_MAT"];
            DataTable dtHash=dsSetIn.Tables["HASH"];
            Hashtable hsTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

            //更新工序交接班记录 修改状态和编辑人时间和时区
            const string CONST_UPDATE_JIAOJIEBAN = @"UPDATE WST_OPERATION_HANDOVER SET
                                                    STATUS=1,RECEIVE_SHIFT_VALUE='{0}',EDIT_TIME= GETDATE(),EDIT_TIMEZONE='{1}',EDITOR='{2}' 
                                                    WHERE OPERATION_HANDOVER_KEY = '{3}'";

            //更新mat物料期末数量和耗用
            const string CONST_UPDATE_MATQIMO = @"UPDATE WST_OPERATION_HANDOVER_MAT SET
                                                    OUT_QTY_2={0},END_QTY={1} WHERE OPERATION_HANDOVER_DETAIL_KEY = '{2}'";

            string sqlDangQianShift = @"SELECT TOP 1 SHIFT_VALUE FROM CAL_SCHEDULE_DAY D
                                        WHERE  GETDATE()<ENDTIME 
                                        ORDER BY ENDTIME DESC";
            DataSet dsDangQianShift = db.ExecuteDataSet(CommandType.Text, sqlDangQianShift);

            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                string strUpdateJiaoJieBan = string.Format(CONST_UPDATE_JIAOJIEBAN,dsDangQianShift.Tables[0].Rows[0][0].ToString(),hsTable["EDIT_TIMEZONE"].ToString(),hsTable["EDITOR"].ToString(),hsTable["OPERATION_HANDOVER_KEY"].ToString());
                db.ExecuteNonQuery(CommandType.Text, strUpdateJiaoJieBan);
                //循环更新数量
                for (int i = 0; i < dtMat.Rows.Count; i++)
                {
                    string strUpdateMat = string.Format(CONST_UPDATE_MATQIMO,
                       Convert.ToDouble(dtMat.Rows[i][10].ToString()) , Convert.ToDouble(dtMat.Rows[i][11].ToString()),
                       dtMat.Rows[i][0].ToString().Trim()
                       );
                    db.ExecuteNonQuery(CommandType.Text, strUpdateMat);
                }

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpdateWipMatHandOverBySaveJiaoban Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }


        /// <summary>
        /// 交班后更新工序交接班内容
        /// </summary>
        /// <param name="dsSetIn1"></param>
        public DataSet UpdateHandOver(DataSet dsSetIn)
        {
            DataSet dsReturn = new DataSet();
            DataTable dtHash = dsSetIn.Tables["HASH1"];
            Hashtable hsTable =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

            //更新工序交接班记录 修改状态和编辑人时间和时区
            const string CONST_UPDATE_JIAOJIEBAN = @"UPDATE WST_OPERATION_HANDOVER SET
                                                    STATUS=2,EDIT_TIME= GETDATE(),EDIT_TIMEZONE='{0}',EDITOR='{1}',
                                                    RECEIVE_SHIFT_VALUE='{2}',RECEIVE_OPERATOR='{3}'
                                                    WHERE OPERATION_HANDOVER_KEY = '{4}'";
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                string strUpdateJiaoJieBan = string.Format(CONST_UPDATE_JIAOJIEBAN, hsTable["EDIT_TIMEZONE"].ToString(),
                    hsTable["EDITOR"].ToString(),
                    hsTable["SHIFT"].ToString(),
                    hsTable["Receiveoperator"].ToString(), 
                    hsTable["OPERATION_HANDOVER_KEY"].ToString());
                db.ExecuteNonQuery(CommandType.Text, strUpdateJiaoJieBan);

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("UpdateHandOver Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }


        /// <summary>
        /// 根据上一工序交接班主键获取上一工序交接班的期末数量插入到新生成的数据中的期初数量
        /// </summary>
        /// <param name="handDangqianOverKey">当前工序交接班主键</param>
        /// <param name="handOverKey">上一工序交接班主键</param>
        public DataSet InsertMatWipQiChu(string handDangqianOverKey, string handOverKey)
        {
            DataSet dsReturn = new DataSet();
            //插入数据到mat中
            const string CONST_INSERT_1 = @" INSERT INTO WST_OPERATION_HANDOVER_MAT W
                                             (OPERATION_HANDOVER_DETAIL_KEY,OPERATION_HANDOVER_KEY,STORE_MATERIAL_KEY,
                                              W.INIT_QTY,W.IN_QTY_1,W.OUT_QTY_1,W.OUT_QTY_2,W.END_QTY)
                                             VALUES('{0}','{1}','{2}',{3},0,0,0,0)";
            //插入数据到wip中
            const string CONST_INSERT_2 = @" INSERT INTO WST_OPERATION_HANDOVER_WIP
                                             (OPERATION_HANDOVER_DETAIL_KEY,OPERATION_HANDOVER_KEY,ORDER_NUMBER,
                                              INIT_QTY,IN_QTY_1,OUT_QTY_1,OUT_QTY_2,OUT_QTY_3,END_QTY)
                                             VALUES('{0}','{1}','{2}',{3},0,0,0,0,0)";

            //查询上一工序的mat数据位插入做准备
            string sql2 = string.Format(@"SELECT STORE_MATERIAL_KEY,END_QTY FROM
                                            WST_OPERATION_HANDOVER_MAT
                                            WHERE
                                            OPERATION_HANDOVER_KEY = '{0}'", handOverKey);
            DataSet dsStoreMnaterialKey = db.ExecuteDataSet(CommandType.Text, sql2);


            //查询上一工序的mat数据位插入做准备
            string sql3 = string.Format(@"SELECT ORDER_NUMBER,END_QTY FROM 
                                            WST_OPERATION_HANDOVER_WIP
                                            WHERE 
                                            OPERATION_HANDOVER_KEY='{0}'", handOverKey);
            DataSet dsOrderNumber = db.ExecuteDataSet(CommandType.Text, sql3);


            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();   //创建事物
            try
            {
                //循环线上仓物料表的主键插入到工序交接班物料明细表中
                for(int i=0;i<dsStoreMnaterialKey.Tables[0].Rows.Count;i++)
                {
                     key1 = System.Guid.NewGuid().ToString();
                     string strInsertInToHandover = string.Format(CONST_INSERT_1,
                        key1,                                                //主键
                        handDangqianOverKey,                                //工序交接班主键
                        dsStoreMnaterialKey.Tables[0].Rows[i][0],            //线上仓物料主键
                        dsStoreMnaterialKey.Tables[0].Rows[i][1]            //INIT_QTY期初等于上一班期末
                        );

                     db.ExecuteNonQuery(CommandType.Text, strInsertInToHandover);
                }

                //循环在制品表的主键插入到工序交接班在制品明细表中
                for (int i = 0; i < dsOrderNumber.Tables[0].Rows.Count; i++)
                {
                    key1 = System.Guid.NewGuid().ToString();
                    string strInsertInToHandover = string.Format(CONST_INSERT_2,
                       key1,                                                //主键
                       handDangqianOverKey,                                         //工序交接班主键
                       dsOrderNumber.Tables[0].Rows[i][0],            //工单号
                       dsOrderNumber.Tables[0].Rows[i][1]            //INIT_QTY期初等于上一班期末
                       );

                    db.ExecuteNonQuery(CommandType.Text, strInsertInToHandover);
                }

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("InsertMatWipQiChu Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }

            return dsReturn;
        }


        public DataSet GetFacKeyByFacName(string facName)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                string sql1 = "";
                sql1 = string.Format(@"SELECT LOCATION_KEY FROM FMM_LOCATION WHERE LOCATION_NAME = '{0}'", facName);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql1);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFacKeyByFacName Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using System.Data;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.Interface;
using System.Data.Common;
using FanHai.Hemera.Utils.StaticFuncs;

namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 原材料领料退料数据查询类。
    /// </summary>
    public class MaterialReqOrReturnEngine : AbstractEngine, IMaterialReqOrReturnEngine
    {
        private Database db = null;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public MaterialReqOrReturnEngine()
        {
            db = DatabaseFactory.CreateDatabase();//实例化对象
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
        }

        #region 获取除电池片外的原材料信息
        public DataSet GetMaterials(string orderNum)
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
                                            AND MATERIAL_CODE NOT LIKE '200%' 
                                            AND MATERIAL_CODE NOT LIKE '201%'
                                            ORDER BY MATERIAL_CODE ",
                                            orderNum.PreventSQLInjection());
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
        #endregion

        #region 根据领料单号查询领料信息
        /// <summary>
        /// 根据领料单号查询领料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>领料单信息抬头表和明细表的信息</returns>
        public DataSet GetMatRequisitionInfByNum(string _numForSelect)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            DataTable dtKO = new DataTable();
            DataTable dtPO = new DataTable();
            string sql = string.Empty;
            try
            {
                sql = string.Format(@"SELECT * FROM dbo.WST_STORE_MATERIAL_REQUISITION
                                               WHERE MBLNR = '{0}' AND STATUSTORESTECT = 1",
                                               _numForSelect.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dtKO = dsReturn.Tables[0];
                dtKO.TableName = "WST_STORE_MATERIAL_REQUISITION";
                sql = string.Format(@"SELECT MBLNR_DETAILKEY,MBLNRKEY,MBLNR,MATNR,MATXT,QTY,ERFME,LLIEF,
                                                CREATOR,CREATE_TIME,EDITOR,EDIT_TIME,MEMO,AUFNR,Row_number() OVER (order by CREATE_TIME) AS ROWNUMBER 
                                                FROM dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                                    WHERE MBLNR = '{0}' AND STATUS = 1",
                                               _numForSelect.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dtPO = dsReturn.Tables[0];
                dtPO.TableName = "WST_STORE_MATERIAL_REQUISITION_DETAIL";
                dsReturn.Merge(dtKO);
                dsReturn.Merge(dtPO);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMatRequisitionInfByNum Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        #endregion

        #region 创建领料单，新建保存
        public DataSet CreateRequistionKoPo(DataSet dsIn)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsCheck = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION
                                        (MBLNRKEY,MBLNR,
                                        FACTORYKEY,FACTORYNAME,PROCESS,STORE_KEY,STORE_NAME,
                                        AUFNR,CREATOR,CREATE_TIME,STATUS,STATUSTORESTECT) 
                                        VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',GETDATE(),'',1) ";

                string sqlPo = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        (MBLNR_DETAILKEY,MBLNRKEY,MBLNR,
                                        MATNR,MATXT,QTY,ERFME,LLIEF,
                                        CREATOR,CREATE_TIME,MEMO,STATUS,
                                        AUFNR)
                                        VALUES
                                        ('{0}','{1}','{2}','{3}',
                                        '{4}',{5},'{6}',
                                        '{7}','{8}',GETDATE(),'{9}', '1','{10}') ";
                string sqlUpdate = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_QTY = ISNULL(PICKING_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) + {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";
                string sqlInstert = @"INSERT INTO 
                                            dbo.WST_STORE_MATERIAL_DETAIL_QTY
                                            (DETAIL_QTY_KEY,WORK_ORDER_NUMBER,MATERIAL,MBLNR,
                                                PICKING_QTY,PICKING_BACK_QTY,SEND_QTY,SEND_BACK_QTY,SUMQTY,LLIEF,UNIT)
                                            VALUES('{0}','{1}','{2}','{3}',{4},0,0,0,{4},'{5}','{6}')";

                DataTable dtKo = dsIn.Tables["WST_STORE_MATERIAL_REQUISITION"];
                DataTable dtPo = dsIn.Tables["WST_STORE_MATERIAL_REQUISITION_DETAIL"];

                string MBLNRKEY = UtilHelper.GenerateNewKey(0);
                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                //插入抬头表信息
                string strInsertInToKo = string.Format(sqlKo,
                                                       MBLNRKEY,                                            //主键
                                                       dtKo.Rows[0]["MBLNR"].ToString(),
                                                       dtKo.Rows[0]["FACTORYKEY"].ToString(),
                                                       dtKo.Rows[0]["FACTORYNAME"].ToString(),
                                                       dtKo.Rows[0]["PROCESS"].ToString(),
                                                       dtKo.Rows[0]["STORE_KEY"].ToString(),
                                                       dtKo.Rows[0]["STORE_NAME"].ToString(),
                                                       dtKo.Rows[0]["AUFNR"].ToString(),
                                                       dtKo.Rows[0]["CREATOR"].ToString()
                  );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsertInToKo);

                foreach (DataRow dr in dtPo.Rows)
                {
                    string MBLNR_DETAILKEY = UtilHelper.GenerateNewKey(0);
                    string MBLNR = dtKo.Rows[0]["MBLNR"].ToString();
                    string MBLNRKEY01 = MBLNRKEY;
                    //int ITMNO = Convert.ToInt32(dr["ROWNUMBER"].ToString().Trim().PreventSQLInjection());     //行号
                    string MATXT = dr["MATXT"].ToString().Trim().PreventSQLInjection();
                    string MATNR = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                    decimal QTY = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                    string ERFME = dr["ERFME"].ToString().Trim().PreventSQLInjection();
                    string LLIEF = dr["LLIEF"].ToString().Trim().PreventSQLInjection();
                    string CREATOR = dr["CREATOR"].ToString().Trim().PreventSQLInjection(); 
                    string MEMO = dr["MEMO"].ToString().Trim().PreventSQLInjection();
                    string AUFNR = dr["AUFNR"].ToString().Trim().PreventSQLInjection();
                    string sqlDetail = string.Format(sqlPo,
                                                        MBLNR_DETAILKEY,             //主键
                                                        MBLNRKEY01,
                                                        MBLNR,
                                                        MATNR,
                                                        MATXT,
                                                        QTY,
                                                        ERFME,
                                                        LLIEF,
                                                        CREATOR,
                                                        MEMO,
                                                        AUFNR
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                    ///判定统计领料数量是否有对应信息
                    string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", AUFNR, MATNR, MBLNR);
                    dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                    if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                    {
                        //更新已有的领料数据以及总数
                        string sqlUpt = string.Format(sqlUpdate,
                                                        QTY, AUFNR, MATNR, MBLNR
                                                        );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                    }
                    else
                    {
                        //插入新的领料数据
                        string sqlInst = string.Format(sqlInstert,
                                                        UtilHelper.GenerateNewKey(0), AUFNR, MATNR, MBLNR, QTY, LLIEF, ERFME
                                                        );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlInst);
                    }
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("CreateRequistionKoPo Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 判定是否存在存在根据领料单号
        public DataSet GetCountByNumToCheck(string number,int flag)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_REQUISITION WHERE MBLNR='{0}' AND STATUSTORESTECT = {1}", number,flag);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetCountByNumToCheck Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        #endregion

        #region 更新领料单信息
        public DataSet UpdateRequistionKoPo(DataSet dsSave,string editor,string mblnr)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsCheck = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                #region 定义SQL执行语句
                string sqlKoUpdate = @" UPDATE WST_STORE_MATERIAL_REQUISITION SET EDITOR = '{0}',EDIT_TIME = GETDATE()
                                        WHERE STATUSTORESTECT = 1 AND MBLNR = '{1}'";

                string sqlPoInsert = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        (MBLNR_DETAILKEY,MBLNRKEY,MBLNR,
                                        MATNR,MATXT,QTY,ERFME,LLIEF,
                                        CREATOR,CREATE_TIME,MEMO,STATUS,
                                        AUFNR)
                                        VALUES
                                        ('{0}','{1}','{2}','{3}',
                                        '{4}',{5},'{6}','{7}','{8}',
                                        GETDATE(),'{9}', '1','{10}') ";

                string sqlPoUpdate = @" UPDATE dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        SET STATUS = 0 , EDITOR = '{3}' ,EDIT_TIME = GETDATE()
                                        WHERE MBLNR = '{0}' AND MATNR = '{1}' AND AUFNR = '{2}'
                                        AND STATUS = 1";

                string sqlUpdateQTYInst = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_QTY = ISNULL(PICKING_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) + {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";

                string sqlInstertQTY = @"INSERT INTO 
                                            dbo.WST_STORE_MATERIAL_DETAIL_QTY
                                            (DETAIL_QTY_KEY,WORK_ORDER_NUMBER,MATERIAL,MBLNR,
                                                PICKING_QTY,PICKING_BACK_QTY,SEND_QTY,SEND_BACK_QTY,SUMQTY,LLIEF,UNIT)
                                            VALUES('{0}','{1}','{2}','{3}',{4},0,0,0,{4},'{5}','{6}')";

                string sqlUpdateQTYDel = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_QTY =  ISNULL(PICKING_QTY,0) - {0},SUMQTY =  ISNULL(SUMQTY,0) - {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";
                #endregion
                #region 绑定数据表
                DataTable dtInsert = null;
                DataTable dtDelete = null;
                if (dsSave.Tables.Contains("INSERT"))
                    dtInsert = dsSave.Tables["INSERT"];
                if (dsSave.Tables.Contains("DELETE"))
                    dtDelete = dsSave.Tables["DELETE"];
                #endregion
               
                string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT,MBLNRKEY FROM WST_STORE_MATERIAL_REQUISITION
                                                            WHERE MBLNR = '{0}' AND STATUSTORESTECT = 1
                                                            GROUP BY MBLNRKEY", mblnr);
                dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                if (dsCheck.Tables[0].Rows.Count > 0)
                { 
                    string mbKey = dsCheck.Tables[0].Rows[0]["MBLNRKEY"].ToString();
                    #region 修改抬头表信息修改人和修改时间
                    string strUpdateInKo = string.Format(sqlKoUpdate,
                                                           editor,
                                                           mblnr
                      );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdateInKo);
                    #endregion

                    #region 有删除的修改时需要做的SQL操作
                    if (dtDelete != null && dtDelete.Rows.Count > 0)
                    {
                        //1.修改从表中的行记录状态为 0 不可用
                        //2.修改数据，领料修改时若有同工单  同料号  同批次的则减去删除的量
                        foreach (DataRow dr in dtDelete.Rows)
                        {
                            string _mblnr = mblnr;
                            string _matnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                            decimal _qty = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                            string _aufnr = dr["AUFNR"].ToString().Trim().PreventSQLInjection();

                            //修改从表状态
                            string sqlDetail = string.Format(sqlPoUpdate,
                                                             _mblnr, _matnr, _aufnr, editor
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                            //修改领料数量和总数
                            string _sqlUpdateQty = string.Format(sqlUpdateQTYDel,
                                                             _qty, _aufnr, _matnr, _mblnr
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, _sqlUpdateQty);

                        }
                    }
                    #endregion

                    #region 有新增的修改时需要做的SQL操作
                    if (dtInsert != null && dtInsert.Rows.Count > 0)
                    {
                        //1.插入新的数据到从表中
                        //2.1修改数据，领料修改时若有同工单  同料号  同批次的则加上新增的数量信息
                        //2.2修改数据，领料修改时若没有同工单  同料号  同批次的则新增该数量信息行
                        foreach (DataRow dr in dtInsert.Rows)
                        {
                            string _detailKey = UtilHelper.GenerateNewKey(0);
                            string _mblnr = mblnr;
                            string _mblnrkey = mbKey;
                            string _matnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                            string _matxt = dr["MATXT"].ToString().Trim().PreventSQLInjection();
                            decimal _qty = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                            string _erfme = dr["ERFME"].ToString().Trim().PreventSQLInjection();
                            string _llief = dr["LLIEF"].ToString().Trim().PreventSQLInjection();
                            string creator = dr["CREATOR"].ToString().Trim().PreventSQLInjection();
                            string _memo = dr["MEMO"].ToString().Trim().PreventSQLInjection();
                            string _aufnr = dr["AUFNR"].ToString().Trim().PreventSQLInjection();

                            string sqlDetail = string.Format(sqlPoInsert,
                                                                _detailKey,             //主键
                                                                _mblnrkey,
                                                                _mblnr,
                                                                _matnr,
                                                                _matxt,
                                                                _qty,
                                                                _erfme,
                                                                _llief,
                                                                creator,
                                                                _memo,
                                                                _aufnr
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                            ///判定统计领料数量是否有对应信息
                            string sqlCheckQty = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", _aufnr, _matnr, _mblnr);
                            dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheckQty);
                            if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                            {
                                //更新已有的领料数据以及总数
                                string sqlUpt = string.Format(sqlUpdateQTYInst,
                                                                _qty, _aufnr, _matnr, _mblnr
                                                                );
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                            }
                            else
                            {
                                string sqlInst = string.Format(sqlInstertQTY,
                                                                UtilHelper.GenerateNewKey(0), _aufnr, _matnr, _mblnr, _qty, _llief, _erfme
                                                                );
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlInst);
                            }
                        }

                    }
                    #endregion
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateRequistionKoPo Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 根据领料单号修改领料状态
        /// <summary>根据领料单号修改领料状态
        /// 根据领料单号修改领料状态
        /// </summary>
        /// <param name="mblnr">领料单号</param>
        public DataSet UpdateStatus(string mblnr,string editor)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                string sqlUpdate = @"UPDATE dbo.WST_STORE_MATERIAL_REQUISITION
                                                    SET STATUS = 'T' ,PASS_PEOPLE = '{0}' ,PASS_TIME = GETDATE()
                                                    WHERE MBLNR = '{1}'
                                                    AND STATUSTORESTECT = 1";

                string sql = string.Format(sqlUpdate,
                                                 editor,mblnr
                                                 );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateStatus Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 删除领料单
        /// <summary>删除领料单
        /// 删除领料单
        /// </summary>
        /// <param name="_num">领料单号</param>
        /// <param name="name">修改人</param>
        /// <returns></returns>
        public DataSet DeleteNum(string _num, string name)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsPo = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                #region 定义SQL执行语句
                string sqlKoUpdate = @" UPDATE WST_STORE_MATERIAL_REQUISITION SET EDITOR = '{0}',EDIT_TIME = GETDATE(),STATUS = 'D'
                                        WHERE STATUSTORESTECT = 1 AND MBLNR = '{1}'";

                string sqlPoUpdate = @" UPDATE dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        SET STATUS = 0 , EDITOR = '{0}' ,EDIT_TIME = GETDATE()
                                        WHERE MBLNR = '{1}'
                                        AND STATUS = 1";

                string sqlUpdateQTYDel = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_QTY = ISNULL(PICKING_QTY,0) - {0},SUMQTY = ISNULL(SUMQTY,0) - {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";
                #endregion
                string strGetPo = string.Format(@"SELECT A.WORK_ORDER_NUMBER,A.MBLNR,A.MATERIAL,B.QTY FROM dbo.WST_STORE_MATERIAL_DETAIL_QTY A
                                                         INNER JOIN  dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL B ON
                                                             A.MATERIAL = B.MATNR AND A.WORK_ORDER_NUMBER = B.AUFNR AND A.MBLNR = B.MBLNR
                                                         WHERE B.STATUS = 1 AND B.MBLNR = '{0}'", _num);
                dsPo = db.ExecuteDataSet(CommandType.Text, strGetPo);
                //1.插入新的数据到从表中
                //2.1修改数据，领料修改时若有同工单  同料号  同批次的则加上新增的数量信息
                //2.2修改数据，领料修改时若没有同工单  同料号  同批次的则新增该数量信息行
                foreach (DataRow dr in dsPo.Tables[0].Rows)
                {
                    string _matnr = dr["MATERIAL"].ToString().Trim().PreventSQLInjection();
                    string _aufnr = dr["WORK_ORDER_NUMBER"].ToString().Trim().PreventSQLInjection();
                    decimal _qty = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                    string sqlDetail = string.Format(sqlUpdateQTYDel,
                                                        _qty,_aufnr, _matnr, _num
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);
                }
                string strUpdateInKo = string.Format(sqlKoUpdate,
                                                           name,
                                                           _num
                      );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdateInKo);

                string strPoUpdate = string.Format(sqlPoUpdate,
                                                           name,
                                                           _num
                      );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strPoUpdate);

                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("DeleteNum Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region 根据退料单号查询领料信息
        /// <summary>
        /// 根据退料单号查询退料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>退料单信息抬头表和明细表的信息</returns>
        public DataSet GetMatRequisitionInfByNumTui(string _numForSelect)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            DataTable dtKO = new DataTable();
            DataTable dtPO = new DataTable();
            string sql = string.Empty;
            try
            {
                sql = string.Format(@"SELECT * FROM dbo.WST_STORE_MATERIAL_REQUISITION
                                               WHERE MBLNR = '{0}' AND STATUSTORESTECT = 0",
                                               _numForSelect.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dtKO = dsReturn.Tables[0];
                dtKO.TableName = "WST_STORE_MATERIAL_REQUISITION";
                sql = string.Format(@"SELECT MBLNR_DETAILKEY,MBLNRKEY,MBLNR,MATNR,MATXT,QTY,ERFME,LLIEF,
                                                CREATOR,CREATE_TIME,EDITOR,EDIT_TIME,MEMO,AUFNR,BACK_MBLNR,Row_number() OVER (order by CREATE_TIME) AS ROWNUMBER 
                                                FROM dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                                    WHERE MBLNR = '{0}' AND STATUS = 1",
                                               _numForSelect.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                dtPO = dsReturn.Tables[0];
                dtPO.TableName = "WST_STORE_MATERIAL_REQUISITION_DETAIL";
                dsReturn.Merge(dtKO);
                dsReturn.Merge(dtPO);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMatRequisitionInfByNumTui Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        #endregion

        #region 创建退料单，新建保存
        public DataSet CreateRequistionKoPoTui(DataSet dsIn)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsCheck = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION
                                        (MBLNRKEY,MBLNR,
                                        FACTORYKEY,FACTORYNAME,PROCESS,STORE_KEY,STORE_NAME,
                                        AUFNR,CREATOR,CREATE_TIME,STATUS,STATUSTORESTECT,PASS_PEOPLE ,PASS_TIME) 
                                        VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',GETDATE(),'T',0,'{9}',GETDATE()) ";

                string sqlPo = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        (MBLNR_DETAILKEY,MBLNRKEY,MBLNR,
                                        MATNR,MATXT,QTY,ERFME,LLIEF,
                                        CREATOR,CREATE_TIME,MEMO,STATUS,
                                        AUFNR,BACK_MBLNR)
                                        VALUES
                                        ('{0}','{1}','{2}','{3}',
                                        '{4}',{5},'{6}','{7}','{8}',GETDATE(),'{9}', '1','{10}','{11}') ";
                string sqlUpdate = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_BACK_QTY = ISNULL(PICKING_BACK_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) - {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";

                DataTable dtKo = dsIn.Tables["WST_STORE_MATERIAL_REQUISITION"];
                DataTable dtPo = dsIn.Tables["WST_STORE_MATERIAL_REQUISITION_DETAIL"];

                string MBLNRKEY = UtilHelper.GenerateNewKey(0);
                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                //插入抬头表信息
                string strInsertInToKo = string.Format(sqlKo,
                                                       MBLNRKEY,                                            //主键
                                                       dtKo.Rows[0]["MBLNR"].ToString(),
                                                       dtKo.Rows[0]["FACTORYKEY"].ToString(),
                                                       dtKo.Rows[0]["FACTORYNAME"].ToString(),
                                                       dtKo.Rows[0]["PROCESS"].ToString(),
                                                       dtKo.Rows[0]["STORE_KEY"].ToString(),
                                                       dtKo.Rows[0]["STORE_NAME"].ToString(),
                                                       dtKo.Rows[0]["AUFNR"].ToString(),
                                                       dtKo.Rows[0]["CREATOR"].ToString(),
                                                       dtKo.Rows[0]["CREATOR"].ToString()
                  );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsertInToKo);

                foreach (DataRow dr in dtPo.Rows)
                {
                    string MBLNR_DETAILKEY = UtilHelper.GenerateNewKey(0);
                    string MBLNR = dtKo.Rows[0]["MBLNR"].ToString();
                    string MBLNRKEY01 = MBLNRKEY;
                    string MATXT = dr["MATXT"].ToString().Trim().PreventSQLInjection();
                    string MATNR = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                    decimal QTY = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                    string ERFME = dr["ERFME"].ToString().Trim().PreventSQLInjection();
                    string LLIEF = dr["LLIEF"].ToString().Trim().PreventSQLInjection();
                    string CREATOR = dr["CREATOR"].ToString().Trim().PreventSQLInjection();
                    string MEMO = dr["MEMO"].ToString().Trim().PreventSQLInjection();
                    string AUFNR = dr["AUFNR"].ToString().Trim().PreventSQLInjection();
                    string BACKMBLNR = dr["BACK_MBLNR"].ToString().Trim().PreventSQLInjection();
                    string sqlDetail = string.Format(sqlPo,
                                                        MBLNR_DETAILKEY,             //主键
                                                        MBLNRKEY01,
                                                        MBLNR,
                                                        MATNR,
                                                        MATXT,
                                                        QTY,
                                                        ERFME,
                                                        LLIEF,
                                                        CREATOR,
                                                        MEMO,
                                                        AUFNR,
                                                        BACKMBLNR
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                    ///判定统计退料数量是否有对应信息
                    string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", AUFNR, MATNR, BACKMBLNR);
                    dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                    if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                    {
                        //更新已有的退料数据以及总数
                        string sqlUpt = string.Format(sqlUpdate,
                                                        QTY, AUFNR, MATNR, BACKMBLNR
                                                        );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                    }
                    else
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WST_STORE_MATERIAL_DETAIL_QTY中不存在数据信息");
                        return retDS;
                    }
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("CreateRequistionKoPoTui Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 更新退料单信息
        public DataSet UpdateRequistionKoPoTui(DataSet dsSave, string editor, string mblnr)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsCheck = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                #region 定义SQL执行语句
                string sqlKoUpdate = @" UPDATE WST_STORE_MATERIAL_REQUISITION SET EDITOR = '{0}',EDIT_TIME = GETDATE()
                                        WHERE STATUSTORESTECT = 0 AND MBLNR = '{1}'";

                string sqlPoInsert = @" INSERT INTO dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        (MBLNR_DETAILKEY,MBLNRKEY,MBLNR,
                                        MATNR,MATXT,QTY,ERFME,LLIEF,
                                        CREATOR,CREATE_TIME,MEMO,STATUS,
                                        AUFNR,BACK_MBLNR)
                                        VALUES
                                        ('{0}','{1}','{2}','{3}',
                                        '{4}',{5},'{6}','{7}','{8}',
                                        GETDATE(),'{9}', '1','{10}','{11}') ";

                string sqlPoUpdate = @" UPDATE dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL
                                        SET STATUS = 0 , EDITOR = '{3}' ,EDIT_TIME = GETDATE()
                                        WHERE MBLNR = '{0}' AND MATNR = '{1}' AND AUFNR = '{2}'
                                        AND STATUS = 1";

                string sqlUpdateQTYInst = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_BACK_QTY = ISNULL(PICKING_BACK_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) - {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";

                string sqlUpdateQTYDel = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET PICKING_BACK_QTY = ISNULL(PICKING_BACK_QTY,0) - {0},SUMQTY = ISNULL(SUMQTY,0) + {0}
                                            WHERE
                                                WORK_ORDER_NUMBER='{1}'
                                                AND MATERIAL='{2}' 
                                                AND MBLNR='{3}'";
                #endregion
                #region 绑定数据表
                DataTable dtInsert = null;
                DataTable dtDelete = null;
                if (dsSave.Tables.Contains("INSERT"))
                    dtInsert = dsSave.Tables["INSERT"];
                if (dsSave.Tables.Contains("DELETE"))
                    dtDelete = dsSave.Tables["DELETE"];
                #endregion

                string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT,MBLNRKEY FROM WST_STORE_MATERIAL_REQUISITION
                                                            WHERE MBLNR = '{0}' AND STATUSTORESTECT = 0
                                                            GROUP BY MBLNRKEY", mblnr);
                dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                if (dsCheck.Tables[0].Rows.Count > 0)
                {
                    string mbKey = dsCheck.Tables[0].Rows[0]["MBLNRKEY"].ToString();
                    #region 修改抬头表信息修改人和修改时间
                    string strUpdateInKo = string.Format(sqlKoUpdate,
                                                           editor,
                                                           mblnr
                      );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdateInKo);

                    #region 有删除的修改时需要做的SQL操作
                    if (dtDelete != null && dtDelete.Rows.Count > 0)
                    {
                        //1.修改从表中的行记录状态为 0 不可用
                        //2.修改数据，退料修改时若有同工单  同料号  同批次的则加上删除的量
                        foreach (DataRow dr in dtDelete.Rows)
                        {
                            string _mblnr = mblnr;
                            string _matnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                            decimal _qty = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                            string _aufnr = dr["AUFNR"].ToString().Trim().PreventSQLInjection();

                            //修改从表状态
                            string sqlDetail = string.Format(sqlPoUpdate,
                                                             _mblnr, _matnr, _aufnr, editor
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                            //修改退料数量和总数
                            string _sqlUpdateQty = string.Format(sqlUpdateQTYDel,
                                                             _qty, _aufnr, _matnr, _mblnr
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, _sqlUpdateQty);

                        }
                    }
                    #endregion

                    #endregion
                    #region 有新增的修改时需要做的SQL操作
                    if (dtInsert != null && dtInsert.Rows.Count > 0)
                    {
                        //1.插入新的数据到从表中
                        //2.1修改数据，退料修改时若有同工单  同料号  同批次的则加上新增的数量信息
                        //2.2修改数据，退料修改时若没有同工单  同料号  同批次的则新增该数量信息行
                        foreach (DataRow dr in dtInsert.Rows)
                        {
                            string _detailKey = UtilHelper.GenerateNewKey(0);
                            string _mblnr = mblnr;
                            string _mblnrkey = mbKey;
                            string _matnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();
                            string _matxt = dr["MATXT"].ToString().Trim().PreventSQLInjection();
                            decimal _qty = Convert.ToDecimal(dr["QTY"].ToString().Trim().PreventSQLInjection());
                            string _erfme = dr["ERFME"].ToString().Trim().PreventSQLInjection();
                            string _llief = dr["LLIEF"].ToString().Trim().PreventSQLInjection();
                            string creator = dr["CREATOR"].ToString().Trim().PreventSQLInjection();
                            string _memo = dr["MEMO"].ToString().Trim().PreventSQLInjection();
                            string _aufnr = dr["AUFNR"].ToString().Trim().PreventSQLInjection();
                            string _backMblnr = dr["BACK_MBLNR"].ToString().Trim().PreventSQLInjection();
                            string sqlDetail = string.Format(sqlPoInsert,
                                                                _detailKey,             //主键
                                                                _mblnrkey,
                                                                _mblnr,
                                                                _matnr,
                                                                _matxt,
                                                                _qty,
                                                                _erfme,
                                                                _llief,
                                                                creator,
                                                                _memo,
                                                                _aufnr,
                                                                _backMblnr
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                            ///判定统计退料数量是否有对应信息
                            string sqlCheckQty = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", _aufnr, _matnr, _backMblnr);
                            dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheckQty);
                            if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                            {
                                //更新已有的退料数据以及总数
                                string sqlUpt = string.Format(sqlUpdateQTYInst,
                                                                _qty, _aufnr, _matnr, _backMblnr
                                                                );
                                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                            }
                            else
                            {
                                dbTrans.Rollback();
                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WST_STORE_MATERIAL_DETAIL_QTY中不存在数据信息");
                                return retDS;
                            }
                        }

                    }
                    #endregion
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateRequistionKoPoTui Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 根据退料单号修改退料状态为过账
        /// <summary>根据退料单号修改退料状态为过账
        /// 根据退料单号修改退料状态
        /// </summary>
        /// <param name="mblnr">退料单号</param>
        public DataSet UpdateStatusTui(string mblnr, string editor)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                string sqlUpdate = @"UPDATE dbo.WST_STORE_MATERIAL_REQUISITION
                                                    SET STATUS = 'T' ,PASS_PEOPLE = '{0}' ,PASS_TIME = GETDATE()
                                                    WHERE MBLNR = '{1}'
                                                    AND STATUSTORESTECT = 0";

                string sql = string.Format(sqlUpdate,
                                                 editor, mblnr
                                                 );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateStatusTui Error: " + ex.Message);
            }
            return retDS;
        }
        #endregion

        #region 获取除电池片外的已经领过料的原材料信息
        /// <summary>
        /// 根据工单,物料代码,批次号 获取已经退料的信息
        /// </summary>
        /// <param name="workorder">工单号</param>
        /// <param name="mat">物料号</param>
        /// <param name="charg">批次号</param>
        /// <returns></returns>
        public DataSet GetMaterialstTui(string orderNum, string mat, string num)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT WORK_ORDER_NUMBER,MATERIAL AS MATERIAL_CODE,PICKING_QTY,PICKING_BACK_QTY,SEND_QTY,A.MBLNR,
                                      SEND_BACK_QTY,SUMQTY,LLIEF,A.UNIT,B.MATERIAL_NAME,B.MATERIAL_KEY
                               FROM WST_STORE_MATERIAL_DETAIL_QTY A 
                               INNER JOIN POR_MATERIAL B ON A.MATERIAL = B.MATERIAL_CODE
                               INNER JOIN WST_STORE_MATERIAL_REQUISITION C ON A.MBLNR = C.MBLNR AND C.STATUS = 'T'
                                      WHERE 1=1 AND SUMQTY > 0";
                if (!string.IsNullOrEmpty(orderNum))
                {
                    sql += string.Format("AND WORK_ORDER_NUMBER = '{0}'", orderNum);
                }
                if (!string.IsNullOrEmpty(mat))
                {
                    sql += string.Format("AND MATERIAL = '{0}'", mat);
                }
                if (!string.IsNullOrEmpty(num))
                {
                    sql += string.Format("AND MBLNR = '{0}'", num);
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterialstTui Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        #endregion

        #region IMaterialReqOrReturnEngine 成员
        public DataSet GetMaterialstTui(string orderNumber)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT DISTINCT WORK_ORDER_NUMBER,MATERIAL AS MATERIAL_CODE,B.MATERIAL_NAME
                                                   FROM WST_STORE_MATERIAL_DETAIL_QTY A 
                                                   INNER JOIN POR_MATERIAL B ON A.MATERIAL = B.MATERIAL_CODE
                                                   INNER JOIN WST_STORE_MATERIAL_REQUISITION C ON A.MBLNR = C.MBLNR AND C.STATUS = 'T'
                                                        WHERE 1=1 AND SUMQTY > 0 AND WORK_ORDER_NUMBER = '{0}'
                                                        ", orderNumber);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterialstTui Error:{0}", ex.Message));
            }
            return dsReturn;
        }
        #endregion

        #region IMaterialReqOrReturnEngine 成员


        public DataSet GetMaterialInf(DataTable paramTable, ref PagingQueryConfig config)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
//                string sql = @"SELECT * FROM dbo.WST_STORE_MATERIAL_DETAIL_QTY
//                                        WHERE 
//                                        1=1";

                string sql = @"SELECT A.*,B.FACTORYNAME FROM dbo.WST_STORE_MATERIAL_DETAIL_QTY A
                                LEFT JOIN dbo.WST_STORE_MATERIAL_REQUISITION B ON A.MBLNR = B.MBLNR
                                WHERE 
                                1=1";
            
                //工单号
                string workOrder = Convert.ToString(paramTable.Rows[0]["WORK_ORDER_NUMBER"]).Trim();
                if (!string.IsNullOrEmpty(workOrder))
                {
                    sql += string.Format(" AND A.WORK_ORDER_NUMBER LIKE '{0}%'", workOrder.PreventSQLInjection());
                }
                //料号
                string material = Convert.ToString(paramTable.Rows[0]["MATERIAL"]).Trim();
                if (!string.IsNullOrEmpty(material))
                {
                    sql += string.Format(" AND A.MATERIAL ='{0}'", material.PreventSQLInjection());
                }
                //单号
                string mblnr = Convert.ToString(paramTable.Rows[0]["MBLNR"]).Trim();
                if (!string.IsNullOrEmpty(mblnr))
                {
                    sql += string.Format(" AND A.MBLNR ='{0}'", mblnr.PreventSQLInjection());
                }
                //工厂车间
                string fac = Convert.ToString(paramTable.Rows[0]["FACTORY"]).Trim();
                if (!string.IsNullOrEmpty(fac))
                {
                    sql += string.Format(" AND B.FACTORYNAME ='{0}'", fac.PreventSQLInjection());
                }
                //状态
                bool status = Convert.ToBoolean(paramTable.Rows[0]["STATUS"].ToString());
                if (status == true)
                {
                    sql += " AND A.SUMQTY > 0";
                }

                if (config == null)
                {
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sql, config.PageNo, config.PageSize, out pages,
                        out records, db, dsReturn, "MBLNR");
                    config.Pages = pages;
                    config.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterialInf Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion

        #region IMaterialReqOrReturnEngine 成员


        public DataSet GetMaterialRequisitionList(DataTable paramTable, ref PagingQueryConfig config)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT   CASE B.STATUSTORESTECT WHEN 1 THEN '领料记录' ELSE '退料记录' END AS STATUSTORESTECT,A.MBLNR,
                                        A.MATNR,A.MATXT,A.QTY,A.ERFME,A.LLIEF,A.AUFNR,A.MEMO,A.CREATOR,B.CREATE_TIME,
                                        B.FACTORYNAME,B.PROCESS,B.STORE_NAME,B.STATUS,B.PASS_PEOPLE,B.PASS_TIME
                                        FROM dbo.WST_STORE_MATERIAL_REQUISITION_DETAIL A LEFT JOIN 
                                        dbo.WST_STORE_MATERIAL_REQUISITION B ON A.MBLNRKEY = B.MBLNRKEY
                                        WHERE A.STATUS = 1
                                        ";

                //工单号
                string workOrder = Convert.ToString(paramTable.Rows[0]["AUFNR"]).Trim();
                if (!string.IsNullOrEmpty(workOrder))
                {
                    sql += string.Format(" AND A.AUFNR LIKE '{0}%'", workOrder.PreventSQLInjection());
                }
                //料号
                string material = Convert.ToString(paramTable.Rows[0]["MATNR"]).Trim();
                if (!string.IsNullOrEmpty(material))
                {
                    sql += string.Format(" AND A.MATNR ='{0}'", material.PreventSQLInjection());
                }
                //单号
                string mblnr = Convert.ToString(paramTable.Rows[0]["MBLNR"]).Trim();
                if (!string.IsNullOrEmpty(mblnr))
                {
                    sql += string.Format(" AND A.MBLNR ='{0}'", mblnr.PreventSQLInjection());
                }
                //工厂
                string facName = Convert.ToString(paramTable.Rows[0]["FACTORYNAME"]).Trim();
                if (!string.IsNullOrEmpty(facName))
                {
                    sql += string.Format(" AND B.FACTORYNAME = '{0}'", facName.PreventSQLInjection());
                }
                //状态
                string status = Convert.ToString(paramTable.Rows[0]["STATUS"]).Trim();
                if (!status.Equals("ALL"))
                {
                    sql += string.Format(" AND B.STATUS ={0}", status.PreventSQLInjection());
                }
                //类型
                string statusToRestect = Convert.ToString(paramTable.Rows[0]["STATUSTORESTECT"]).Trim();
                if (!string.IsNullOrEmpty(statusToRestect))
                {
                    sql += string.Format(" AND STATUSTORESTECT ={0}", statusToRestect.PreventSQLInjection());
                }
                //起始时间
                string startTime = Convert.ToString(paramTable.Rows[0]["CREATE_TIME_START"]).Trim();
                if (!string.IsNullOrEmpty(startTime))
                {
                    sql += string.Format(" AND B.CREATE_TIME >='{0}'", startTime.PreventSQLInjection());
                }
                //结束时间
                string endTime = Convert.ToString(paramTable.Rows[0]["CREATE_TIME_END"]).Trim();
                if (!string.IsNullOrEmpty(endTime))
                {
                    sql += string.Format(" AND B.CREATE_TIME <='{0}'", endTime.PreventSQLInjection());
                }
                if (config == null)
                {
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sql, config.PageNo, config.PageSize, out pages,
                        out records, db, dsReturn, "STATUSTORESTECT");
                    config.Pages = pages;
                    config.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterialRequisitionList Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion

        #region IMaterialReqOrReturnEngine 成员


        public DataSet GetEquMaterialInf(DataTable paramTable, ref PagingQueryConfig config)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT FACTORY_NAME,OPERATION_NAME,EQUIPMENT_NAME,LING_NAME,PARAMETER,
                                        MATERIAL_CODE,SUPPLIER_CODE,SENDING_QTY,SENDING_BACK_QTY,SENDING_UNIT,
                                        SUM_QTY,ORDER_NUMBER,SUPPLIER,USED_QTY
                                        FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                      WHERE SUM_QTY > 0";

                string facName = Convert.ToString(paramTable.Rows[0]["FACTORY_NAME"]).Trim();
                if (!string.IsNullOrEmpty(facName))
                {
                    sql += string.Format(" AND FACTORY_NAME = '{0}'", facName.PreventSQLInjection());
                }
                string operationName = Convert.ToString(paramTable.Rows[0]["OPERATION_NAME"]).Trim();
                if (!string.IsNullOrEmpty(operationName))
                {
                    sql += string.Format(" AND OPERATION_NAME ='{0}'", operationName.PreventSQLInjection());
                }
                string equName = Convert.ToString(paramTable.Rows[0]["EQUIPMENT_NAME"]).Trim();
                if (!string.IsNullOrEmpty(equName))
                {
                    sql += string.Format(" AND EQUIPMENT_NAME ='{0}'", equName.PreventSQLInjection());
                }
                string orderNum = Convert.ToString(paramTable.Rows[0]["ORDER_NUMBER"]).Trim();
                if (!string.IsNullOrEmpty(orderNum))
                {
                    sql += string.Format(" AND ORDER_NUMBER = '{0}'", orderNum.PreventSQLInjection());
                }
                string matCode = Convert.ToString(paramTable.Rows[0]["MATERIAL_CODE"]).Trim();
                if (!string.IsNullOrEmpty(matCode))
                {
                    sql += string.Format(" AND MATERIAL_CODE ='{0}'", matCode.PreventSQLInjection());
                }
                string supplierCode = Convert.ToString(paramTable.Rows[0]["SUPPLIER_CODE"]).Trim();
                if (!string.IsNullOrEmpty(supplierCode))
                {
                    sql += string.Format(" AND SUPPLIER_CODE ='{0}'", supplierCode.PreventSQLInjection());
                }
                if (config == null)
                {
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sql, config.PageNo, config.PageSize, out pages,
                        out records, db, dsReturn, "SUM_QTY");
                    config.Pages = pages;
                    config.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetEquMaterialInf Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion

        #region IMaterialReqOrReturnEngine 成员


        public DataSet GetMaterialSendingList(DataTable paramTable, ref PagingQueryConfig config)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT   FACTORY_NAME,OPERATION_NAME,EQUIPMENT_NAME,
                                        LING_NAME,PARAMETER,MATERIAL_CODE,MATERIAL_DESC,
                                        MBLNR,SUPPLIER,SUPPLIER_CODE,SENDING_QTY,
                                        SENDING_UNIT,CONRTAST_QTY,CONRTAST_UNIT,
                                        CASE STATUS WHEN 1 THEN '发料记录' ELSE '退料记录' END AS SENDINGSTATUS,
                                        CREATOR,CREATE_TIME,MEMO,WORK_ORDER FROM dbo.WST_STORE_MATERIAL_SENDING
                                        WHERE 1 = 1
                                        ";

                string facName = Convert.ToString(paramTable.Rows[0]["FACTORY_NAME"]).Trim();
                if (!string.IsNullOrEmpty(facName))
                {
                    sql += string.Format(" AND FACTORY_NAME = '{0}'", facName.PreventSQLInjection());
                }
                string operationName = Convert.ToString(paramTable.Rows[0]["OPERATION_NAME"]).Trim();
                if (!string.IsNullOrEmpty(operationName))
                {
                    sql += string.Format(" AND OPERATION_NAME ='{0}'", operationName.PreventSQLInjection());
                }
                string equName = Convert.ToString(paramTable.Rows[0]["EQUIPMENT_NAME"]).Trim();
                if (!string.IsNullOrEmpty(equName))
                {
                    sql += string.Format(" AND EQUIPMENT_NAME ='{0}'", equName.PreventSQLInjection());
                }
                string orderNum = Convert.ToString(paramTable.Rows[0]["WORK_ORDER"]).Trim();
                if (!string.IsNullOrEmpty(orderNum))
                {
                    sql += string.Format(" AND WORK_ORDER = '{0}'", orderNum.PreventSQLInjection());
                }
                string matCode = Convert.ToString(paramTable.Rows[0]["MATERIAL_CODE"]).Trim();
                if (!string.IsNullOrEmpty(matCode))
                {
                    sql += string.Format(" AND MATERIAL_CODE ='{0}'", matCode.PreventSQLInjection());
                }
                string parameter = Convert.ToString(paramTable.Rows[0]["PARAMETER"]).Trim();
                if (!string.IsNullOrEmpty(parameter))
                {
                    sql += string.Format(" AND PARAMETER ='{0}'", parameter.PreventSQLInjection());
                }
                string mblnr = Convert.ToString(paramTable.Rows[0]["MBLNR"]).Trim();
                if (!string.IsNullOrEmpty(mblnr))
                {
                    sql += string.Format(" AND MBLNR ='{0}'", mblnr.PreventSQLInjection());
                }
                string status = Convert.ToString(paramTable.Rows[0]["STATUS"]).Trim();
                if (!string.IsNullOrEmpty(status))
                {
                    sql += string.Format(" AND STATUS ='{0}'", status.PreventSQLInjection());
                }
                //起始时间
                string startTime = Convert.ToString(paramTable.Rows[0]["CREATE_TIME_START"]).Trim();
                if (!string.IsNullOrEmpty(startTime))
                {
                    sql += string.Format(" AND CREATE_TIME >='{0}'", startTime.PreventSQLInjection());
                }
                //结束时间
                string endTime = Convert.ToString(paramTable.Rows[0]["CREATE_TIME_END"]).Trim();
                if (!string.IsNullOrEmpty(endTime))
                {
                    sql += string.Format(" AND CREATE_TIME <='{0}'", endTime.PreventSQLInjection());
                }

                if (config == null)
                {
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                }
                else
                {
                    int pages = 0;
                    int records = 0;
                    AllCommonFunctions.CommonPagingData(sql, config.PageNo, config.PageSize, out pages,
                        out records, db, dsReturn, "STATUS");
                    config.Pages = pages;
                    config.Records = records;
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMaterialSendingList Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion
    }

}

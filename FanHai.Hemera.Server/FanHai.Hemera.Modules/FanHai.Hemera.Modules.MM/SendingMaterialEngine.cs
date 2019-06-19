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

namespace FanHai.Hemera.Modules.MM
{
    /// <summary>
    /// 原材料领料退料数据查询类。
    /// </summary>
    public class SendingMaterialEngine : AbstractEngine, ISendingMaterialEngine
    {
        private Database db = null;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SendingMaterialEngine()
        {
            db = DatabaseFactory.CreateDatabase();//实例化对象
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
        }
        #region ISendingMaterialEngine 成员

        public DataSet GetParameters()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT * FROM WST_STORE_MATERIAL_BUCKLE_CONTROL WHERE STATUS = 1";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetParameters Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion

        #region ISendingMaterialEngine 成员

        /// <summary>
        /// 插入数据到发料退料记录表，同时修改设备虚拟仓信息
        /// </summary>
        /// <param name="dtInf">数据信息表</param>
        /// <param name="flag">标志 1 发料 0 退料</param>
        /// <returns></returns>
        public DataSet InsertNewInf(DataTable dtInf)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                #region 插入数据到发料退料记录表中
                string sqlKo = @"INSERT INTO dbo.WST_STORE_MATERIAL_SENDING
                                        (SEND_MAT_KEY,SENDING_NUMBER,FACTORY_NAME,
                                        FACTORY_KEY,OPERATION_NAME,EQUIPMENT_NAME,
                                        EQUIPMENT_KEY,LING_NAME,LINE_KEY,
                                        PARAMETER,PARAMETER_KEY,MATERIAL_CODE,
                                        MATERIAL_DESC,MBLNR,SUPPLIER,
                                        SUPPLIER_CODE,SENDING_QTY,SENDING_UNIT,
                                        CONRTAST_QTY,CONRTAST_UNIT,STATUS,
                                        CREATOR,CREATE_TIME,MEMO,
                                        WORK_ORDER) 
                                VALUES
                                        ('{0}','{1}','{2}',
                                        '{3}','{4}','{5}',
                                        '{6}','{7}','{8}',
                                        '{9}','{10}','{11}',
                                        '{12}','{13}','{14}',
                                        '{15}',{16},'{17}',
                                        {18},'{19}',1,
                                        '{20}',GETDATE(),'{21}',
                                        '{22}')";
                #endregion
                #region
//                string sqlUpdate01 = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET 
//                                                SEND_QTY = ISNULL(SEND_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) - {0}
//                                            WHERE
//                                                WORK_ORDER_NUMBER='{1}'
//                                                AND MATERIAL='{2}' 
//                                                AND MBLNR='{3}'"; 
                string sqlUpdate01 = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET 
                                                                SEND_QTY = ISNULL(SEND_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) - {0}
                                                            WHERE
                                                                MATERIAL='{1}' 
                                                                AND MBLNR='{2}'"; 
                #endregion

                #region 查看领料信息数据表中是否存在物料的记录
//                string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
//                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", dtInf.Rows[0]["WORK_ORDER"].ToString(), dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString());
                string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE 
                                                                 MATERIAL='{0}' AND MBLNR='{1}'", dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString());
                DataSet dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                {
                    //更新已有的退料数据以及总数
                    string sqlUpt = string.Format(sqlUpdate01,
                                                    dtInf.Rows[0]["CONRTAST_QTY"].ToString(),  dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString()
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                }
                else
                {
                    dbTrans.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WST_STORE_MATERIAL_DETAIL_QTY中不存在数据信息");
                    return retDS;
                }
                #endregion
                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                if (dtInf != null && dtInf.Rows.Count > 0)
                {
                    //插入抬头表信息
                    string strInsert = string.Format(sqlKo,
                                                           UtilHelper.GenerateNewKey(0),                                            //主键
                                                           dtInf.Rows[0]["SENDING_NUMBER"].ToString(),
                                                           dtInf.Rows[0]["FACTORY_NAME"].ToString(),
                                                           dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                           dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_NAME"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                           dtInf.Rows[0]["LING_NAME"].ToString(),
                                                           dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                           dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                           dtInf.Rows[0]["MATERIAL_DESC"].ToString(),
                                                           dtInf.Rows[0]["MBLNR"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                           dtInf.Rows[0]["SENDING_QTY"].ToString(),
                                                           dtInf.Rows[0]["SENDING_UNIT"].ToString(),
                                                           dtInf.Rows[0]["CONRTAST_QTY"].ToString(),
                                                           dtInf.Rows[0]["CONRTAST_UNIT"].ToString(),
                                                           dtInf.Rows[0]["CREATOR"].ToString(),
                                                           dtInf.Rows[0]["MEMO"].ToString(),
                                                           dtInf.Rows[0]["WORK_ORDER"].ToString()
                      );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsert);

                    string insertOrUpdate = string.Format(@"DECLARE @FACKEY VARCHAR(64),
		                                                            @EQUIPMENTKEY VARCHAR(64),
		                                                            @OPERATIONNAME VARCHAR(64),
		                                                            @LINEKEY VARCHAR(64),
		                                                            @PARAMETERKEY VARCHAR(64),
		                                                            @MATERIALCODE VARCHAR(64),
		                                                            @SUPPLIERCODE VARCHAR(64),
		                                                            @ORDERNUMBER VARCHAR(64),
		                                                            @SENDINGQTY numeric(10, 3),
		                                                            @EDITOR VARCHAR(64),
		                                                            @MATEQUKEY VARCHAR(64),
		                                                            @SENDINGNUMBER VARCHAR(64),
		                                                            @FACTORYNAME VARCHAR(64),
		                                                            @EQUIPMENTNAME VARCHAR(64),
		                                                            @LINGNAME VARCHAR(64),
		                                                            @PARAMETER VARCHAR(64),
		                                                            @SENDINGUNIT VARCHAR(64),
                                                                    @SUPPLIER VARCHAR(64)
 		                                                            SET @FACKEY = '{0}'
		                                                            SET @EQUIPMENTKEY = '{1}'
		                                                            SET @OPERATIONNAME = '{2}'
		                                                            SET @LINEKEY = '{3}'
		                                                            SET @PARAMETERKEY = '{4}'
		                                                            SET @MATERIALCODE = '{5}'
		                                                            SET @SUPPLIERCODE = '{6}'
		                                                            SET @ORDERNUMBER = '{7}'
		                                                            SET @SENDINGQTY = {8}
		                                                            SET @SENDINGUNIT = '{9}'
		                                                            SET @EDITOR = '{10}'
		                                                            SET @MATEQUKEY = '{11}'
		                                                            SET @SENDINGNUMBER ='{12}'
		                                                            SET @FACTORYNAME = '{13}'
		                                                            SET @EQUIPMENTNAME = '{14}' 
		                                                            SET @LINGNAME = '{15}'
		                                                            SET @PARAMETER = '{16}'
                                                                    SET @SUPPLIER = '{17}'
                                                            IF EXISTS(SELECT  1  FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                                                                 WHERE FACTORY_KEY = @FACKEY
                                                                                    AND EQUIPMENT_KEY = @EQUIPMENTKEY
                                                                                    AND OPERATION_NAME = @OPERATIONNAME
                                                                                    AND LINE_KEY = @LINEKEY
                                                                                    AND PARAMETER_KEY = @PARAMETERKEY
                                                                                    AND MATERIAL_CODE = @MATERIALCODE
                                                                                    AND SUPPLIER_CODE = @SUPPLIERCODE
                                                                                    AND ORDER_NUMBER = @ORDERNUMBER
                                                                                    AND STATUS = 1)
                                                            BEGIN 
                                                            UPDATE dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE SET
                                                                    SENDING_QTY = ISNULL(SENDING_QTY,0) + @SENDINGQTY,
                                                                    SUM_QTY = ISNULL(SUM_QTY,0) + @SENDINGQTY,
                                                                    EDITOR = @EDITOR,
                                                                    EDITER_TIME = GETDATE()
                                                            WHERE FACTORY_KEY = @FACKEY
                                                                    AND EQUIPMENT_KEY = @EQUIPMENTKEY
                                                                    AND OPERATION_NAME = @OPERATIONNAME
                                                                    AND LINE_KEY = @LINEKEY
                                                                    AND PARAMETER_KEY = @PARAMETERKEY
                                                                    AND MATERIAL_CODE = @MATERIALCODE
                                                                    AND SUPPLIER_CODE = @SUPPLIERCODE
                                                                    AND ORDER_NUMBER = @ORDERNUMBER
                                                                    AND STATUS = 1
                                                            END
                                                            ELSE
                                                            BEGIN
                                                            INSERT INTO dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE(
                                                                    MAT_EQU_KEY,SENDING_NUMBER,FACTORY_NAME,FACTORY_KEY,
                                                                    OPERATION_NAME,EQUIPMENT_NAME,EQUIPMENT_KEY,LING_NAME,
                                                                    LINE_KEY,PARAMETER,PARAMETER_KEY,MATERIAL_CODE,
                                                                    SUPPLIER_CODE,SENDING_QTY,SENDING_UNIT,CREATOR,
                                                                    CREATE_TIME,SUM_QTY,ORDER_NUMBER,SUPPLIER)
                                                            VALUES(@MATEQUKEY,@SENDINGNUMBER,@FACTORYNAME,@FACKEY,
                                                                    @OPERATIONNAME,@EQUIPMENTNAME,@EQUIPMENTKEY,@LINGNAME,
                                                                    @LINEKEY,@PARAMETER,@PARAMETERKEY,@MATERIALCODE,
                                                                    @SUPPLIERCODE,@SENDINGQTY,@SENDINGUNIT,@EDITOR,
                                                                    GETDATE(),@SENDINGQTY,@ORDERNUMBER,@SUPPLIER)
                                                            END
                                                            ",
                                                           dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                           dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                           dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                           dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                           dtInf.Rows[0]["WORK_ORDER"].ToString(),
                                                           dtInf.Rows[0]["SENDING_QTY"].ToString(),
                                                           dtInf.Rows[0]["SENDING_UNIT"].ToString(),
                                                           dtInf.Rows[0]["CREATOR"].ToString(),
                                                           UtilHelper.GenerateNewKey(0), 
                                                           dtInf.Rows[0]["SENDING_NUMBER"].ToString(),
                                                           dtInf.Rows[0]["FACTORY_NAME"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_NAME"].ToString(),
                                                           dtInf.Rows[0]["LING_NAME"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER"].ToString()
                                                           );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, insertOrUpdate);

                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "保存清单信息为空！");
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("InsertNewInf Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region ISendingMaterialEngine 成员
        /// <summary>
        /// 插入数据到发料退料记录表，同时修改设备虚拟仓信息
        /// </summary>
        /// <param name="dtInf">数据信息表</param>
        /// <param name="flag">标志 1 发料 0 退料</param>
        /// <returns></returns>
        public DataSet UpdateParameterInf(DataTable dtInf)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @"INSERT INTO dbo.WST_STORE_MATERIAL_SENDING
                                        (SEND_MAT_KEY,SENDING_NUMBER,FACTORY_NAME,
                                        FACTORY_KEY,OPERATION_NAME,EQUIPMENT_NAME,
                                        EQUIPMENT_KEY,LING_NAME,LINE_KEY,
                                        PARAMETER,PARAMETER_KEY,MATERIAL_CODE,
                                        MATERIAL_DESC,MBLNR,SUPPLIER,
                                        SUPPLIER_CODE,SENDING_QTY,SENDING_UNIT,
                                        CONRTAST_QTY,CONRTAST_UNIT,STATUS,
                                        CREATOR,CREATE_TIME,MEMO,
                                        WORK_ORDER) 
                                VALUES
                                        ('{0}','{1}','{2}',
                                        '{3}','{4}','{5}',
                                        '{6}','{7}','{8}',
                                        '{9}','{10}','{11}',
                                        '{12}','{13}','{14}',
                                        '{15}',{16},'{17}',
                                        {18},'{19}',0,
                                        '{20}',GETDATE(),'{21}',
                                        '{22}')";

                #region 修改线上仓数据信息
//                string sqlUpdate01 = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET 
//                                                SEND_BACK_QTY = ISNULL(SEND_BACK_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) + {0}
//                                            WHERE
//                                                WORK_ORDER_NUMBER='{1}'
//                                                AND MATERIAL='{2}' 
//                                                AND MBLNR='{3}'";
                string sqlUpdate01 = @"UPDATE WST_STORE_MATERIAL_DETAIL_QTY SET 
                                                SEND_BACK_QTY = ISNULL(SEND_BACK_QTY,0) + {0},SUMQTY = ISNULL(SUMQTY,0) + {0}
                                            WHERE
                                                MATERIAL='{1}' 
                                                AND MBLNR='{2}'";
                #endregion

                #endregion
                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                if (dtInf != null && dtInf.Rows.Count > 0)
                {
                    #region 查看领料信息数据表中是否存在物料的记录
//                    string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE WORK_ORDER_NUMBER='{0}'
//                                                                AND MATERIAL='{1}' AND MBLNR='{2}'", dtInf.Rows[0]["WORK_ORDER"].ToString(), dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString());
                    string sqlCheck = string.Format(@"SELECT COUNT(1) AS COUNT FROM WST_STORE_MATERIAL_DETAIL_QTY WHERE 
                                                                MATERIAL='{0}' AND MBLNR='{1}'", dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString());
                    DataSet dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                    if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                    {
                        //更新已有的退料数据以及总数
                        string sqlUpt = string.Format(sqlUpdate01,
                                                        dtInf.Rows[0]["CONRTAST_QTY"].ToString(), dtInf.Rows[0]["MATERIAL_CODE"].ToString(), dtInf.Rows[0]["MBLNR"].ToString()
                                                        );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                    }
                    else
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WST_STORE_MATERIAL_DETAIL_QTY中不存在数据信息");
                        return retDS;
                    }
                    #endregion
                    #region
                    sqlCheck = string.Format(@"  SELECT SUM_QTY FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                             WHERE FACTORY_KEY = '{0}'
                                                AND EQUIPMENT_KEY = '{1}'
                                                AND OPERATION_NAME = '{2}'
                                                AND LINE_KEY = '{3}'
                                                AND PARAMETER_KEY = '{4}'
                                                AND MATERIAL_CODE = '{5}'
                                                AND SUPPLIER_CODE = '{6}'
                                                AND ORDER_NUMBER = '{7}'
                                                AND STATUS = 1",
                                                    dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                    dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                    dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                    dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                    dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                    dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                    dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                    dtInf.Rows[0]["WORK_ORDER"].ToString());
                    dsCheck = db.ExecuteDataSet(CommandType.Text, sqlCheck);
                    if (dsCheck.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dsCheck.Tables[0].Rows[0]["SUM_QTY"].ToString()) > Convert.ToDecimal(dtInf.Rows[0]["SENDING_QTY"].ToString()))
                        {//判定剩余数量是否大于本次退料数量
                            string sqlUpt = string.Format(@"UPDATE dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE SET
                                                                SENDING_BACK_QTY= ISNULL(SENDING_BACK_QTY,0) + {0},
                                                                SUM_QTY = ISNULL(SUM_QTY,0) - {0},
                                                                EDITOR = '{9}',
                                                                EDITER_TIME = GETDATE()
                                                        WHERE FACTORY_KEY = '{1}'
                                                                AND EQUIPMENT_KEY = '{2}'
                                                                AND OPERATION_NAME = '{3}'
                                                                AND LINE_KEY = '{4}'
                                                                AND PARAMETER_KEY = '{5}'
                                                                AND MATERIAL_CODE = '{6}'
                                                                AND SUPPLIER_CODE = '{7}'
                                                                AND ORDER_NUMBER = '{8}'
                                                                AND STATUS = 1",
                                                            dtInf.Rows[0]["SENDING_QTY"].ToString(),
                                                            dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                            dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                            dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                            dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                            dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                            dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                            dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                            dtInf.Rows[0]["WORK_ORDER"].ToString(),
                                                            dtInf.Rows[0]["CREATOR"].ToString()
                                                            );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                        }
                        else if (Convert.ToDecimal(dsCheck.Tables[0].Rows[0]["SUM_QTY"].ToString()) == Convert.ToDecimal(dtInf.Rows[0]["SENDING_QTY"].ToString()))
                        {
                            string sqlUpt = string.Format(@"UPDATE dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE SET
                                                                SENDING_BACK_QTY= ISNULL(SENDING_BACK_QTY,0) + {0},
                                                                SUM_QTY = ISNULL(SUM_QTY,0) - {0},
                                                                EDITOR = '{9}',
                                                                EDITER_TIME = GETDATE(),
                                                                STATUS = 0
                                                        WHERE FACTORY_KEY = '{1}'
                                                                AND EQUIPMENT_KEY = '{2}'
                                                                AND OPERATION_NAME = '{3}'
                                                                AND LINE_KEY = '{4}'
                                                                AND PARAMETER_KEY = '{5}'
                                                                AND MATERIAL_CODE = '{6}'
                                                                AND SUPPLIER_CODE = '{7}'
                                                                AND ORDER_NUMBER = '{8}'
                                                                AND STATUS = 1",
                                                                   dtInf.Rows[0]["SENDING_QTY"].ToString(),
                                                                   dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                                   dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                                   dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                                   dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                                   dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                                   dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                                   dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                                   dtInf.Rows[0]["WORK_ORDER"].ToString(),
                                                                   dtInf.Rows[0]["CREATOR"].ToString()
                                                                   );
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpt);
                        }
                        else
                        {
                            dbTrans.Rollback();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "WST_STORE_MATERIAL_EQUIPMENT_STORE中不存在数据信息,请先发料！");
                            return retDS;
                        }
                    }
                    else
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "此车间设备没有该工单的物料可退！");
                        return retDS;
                    }
                    //插入抬头表信息
                    string strInsert = string.Format(sqlKo,
                                                           UtilHelper.GenerateNewKey(0),                                            //主键
                                                           dtInf.Rows[0]["SENDING_NUMBER"].ToString(),
                                                           dtInf.Rows[0]["FACTORY_NAME"].ToString(),
                                                           dtInf.Rows[0]["FACTORY_KEY"].ToString(),
                                                           dtInf.Rows[0]["OPERATION_NAME"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_NAME"].ToString(),
                                                           dtInf.Rows[0]["EQUIPMENT_KEY"].ToString(),
                                                           dtInf.Rows[0]["LING_NAME"].ToString(),
                                                           dtInf.Rows[0]["LINE_KEY"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER"].ToString(),
                                                           dtInf.Rows[0]["PARAMETER_KEY"].ToString(),
                                                           dtInf.Rows[0]["MATERIAL_CODE"].ToString(),
                                                           dtInf.Rows[0]["MATERIAL_DESC"].ToString(),
                                                           dtInf.Rows[0]["MBLNR"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER"].ToString(),
                                                           dtInf.Rows[0]["SUPPLIER_CODE"].ToString(),
                                                           dtInf.Rows[0]["SENDING_QTY"].ToString(),
                                                           dtInf.Rows[0]["SENDING_UNIT"].ToString(),
                                                           dtInf.Rows[0]["CONRTAST_QTY"].ToString(),
                                                           dtInf.Rows[0]["CONRTAST_UNIT"].ToString(),
                                                           dtInf.Rows[0]["CREATOR"].ToString(),
                                                           dtInf.Rows[0]["MEMO"].ToString(),
                                                           dtInf.Rows[0]["WORK_ORDER"].ToString()
                      );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsert);
                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "保存清单信息为空！");
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateParameterInf Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region ISendingMaterialEngine 成员


        public DataSet GetMatEquipmentStore(string facKey, string equipmentKey, string operationName, 
                                                string lineKey, string parameterKey, 
                                                    string matCode, string orderNumber,string _type)
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Empty;
                if (_type.Equals("BindWorkOrder"))
                {
                    sql = @"SELECT DISTINCT ORDER_NUMBER FROM  dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                WHERE STATUS = 1";
                    if (!string.IsNullOrEmpty(facKey))
                    {
                        sql += string.Format(@" AND FACTORY_KEY = '{0}' ", facKey);
                    }
                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sql += string.Format(@" AND EQUIPMENT_KEY = '{0}' ", equipmentKey);
                    }
                    if (!string.IsNullOrEmpty(operationName))
                    {
                        sql += string.Format(@" AND OPERATION_NAME = '{0}' ", operationName);
                    }
                    if (!string.IsNullOrEmpty(lineKey))
                    {
                        sql += string.Format(@" AND LINE_KEY ='{0}' ", lineKey);
                    }
                    if (!string.IsNullOrEmpty(parameterKey))
                    {
                        sql += string.Format(@" AND PARAMETER_KEY = '{0}' ", parameterKey);
                    }
                }
                if (_type.Equals("BindMatCode"))
                {
                    sql = @"SELECT DISTINCT A.MATERIAL_CODE,B.DESCRIPTION FROM dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE A
                                    LEFT JOIN POR_WORK_ORDER_BOM B ON A.ORDER_NUMBER = B.ORDER_NUMBER AND A.MATERIAL_CODE = B.MATERIAL_CODE
                                    WHERE STATUS = 1";
                    if (!string.IsNullOrEmpty(facKey))
                    {
                        sql += string.Format(@" AND FACTORY_KEY = '{0}' ", facKey);
                    }
                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sql += string.Format(@" AND EQUIPMENT_KEY = '{0}' ", equipmentKey);
                    }
                    if (!string.IsNullOrEmpty(operationName))
                    {
                        sql += string.Format(@" AND OPERATION_NAME = '{0}' ", operationName);
                    }
                    if (!string.IsNullOrEmpty(lineKey))
                    {
                        sql += string.Format(@" AND LINE_KEY ='{0}' ", lineKey);
                    }
                    if (!string.IsNullOrEmpty(parameterKey))
                    {
                        sql += string.Format(@" AND PARAMETER_KEY = '{0}' ", parameterKey);
                    }
                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        sql += string.Format(@" AND A.ORDER_NUMBER = '{0}' ", orderNumber);
                    }
                }
                if (_type.Equals("BindSupplierCode"))
                {
                    sql = @"SELECT  SUPPLIER_CODE,SUM_QTY,SUPPLIER FROM  dbo.WST_STORE_MATERIAL_EQUIPMENT_STORE
                                WHERE STATUS = 1";
                    if (!string.IsNullOrEmpty(facKey))
                    {
                        sql += string.Format(@" AND FACTORY_KEY = '{0}' ", facKey);
                    }
                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sql += string.Format(@" AND EQUIPMENT_KEY = '{0}' ", equipmentKey);
                    }
                    if (!string.IsNullOrEmpty(operationName))
                    {
                        sql += string.Format(@" AND OPERATION_NAME = '{0}' ", operationName);
                    }
                    if (!string.IsNullOrEmpty(lineKey))
                    {
                        sql += string.Format(@" AND LINE_KEY ='{0}' ", lineKey);
                    }
                    if (!string.IsNullOrEmpty(parameterKey))
                    {
                        sql += string.Format(@" AND PARAMETER_KEY = '{0}' ", parameterKey);
                    }
                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        sql += string.Format(@" AND ORDER_NUMBER = '{0}' ", orderNumber);
                    }
                    if (!string.IsNullOrEmpty(matCode))
                    {
                        sql += string.Format(@" AND MATERIAL_CODE = '{0}' ", matCode);
                    }
                }
                if (_type.Equals("BindMblnr"))
                {
                    sql = @"SELECT DISTINCT MBLNR FROM  dbo.WST_STORE_MATERIAL_SENDING
                                WHERE STATUS = 1";
                    if (!string.IsNullOrEmpty(facKey))
                    {
                        sql += string.Format(@" AND FACTORY_KEY = '{0}' ", facKey);
                    }
                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sql += string.Format(@" AND EQUIPMENT_KEY = '{0}' ", equipmentKey);
                    }
                    if (!string.IsNullOrEmpty(operationName))
                    {
                        sql += string.Format(@" AND OPERATION_NAME = '{0}' ", operationName);
                    }
                    if (!string.IsNullOrEmpty(lineKey))
                    {
                        sql += string.Format(@" AND LINE_KEY ='{0}' ", lineKey);
                    }
                    if (!string.IsNullOrEmpty(parameterKey))
                    {
                        sql += string.Format(@" AND PARAMETER_KEY = '{0}' ", parameterKey);
                    }
                    if (!string.IsNullOrEmpty(orderNumber))
                    {
                        sql += string.Format(@" AND WORK_ORDER = '{0}' ", orderNumber);
                    }
                    if (!string.IsNullOrEmpty(matCode))
                    {
                        sql += string.Format(@" AND MATERIAL_CODE = '{0}' ", matCode);
                    }
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, msg);
                LogService.LogError(string.Format("GetMatEquipmentStore Error:{0}", ex.Message));
            }
            return dsReturn;
        }

        #endregion
    }

}

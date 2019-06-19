using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils;


using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface.EquipmentManagement;

using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Data.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备面板数据管理类。
    /// </summary>
    public class EquipmentLayoutEngine : AbstractEngine
    {
        private Database db;//数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentLayoutEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}

        /// <summary>
        /// 新增设备布局。
        /// </summary>
        /// <param name="dsParams">包含设备布局数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。
        /// ExtendedProperties：OUTPUT_MESSAGE
        /// </returns>
        public DataSet InsertEquipmentLayout(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            DbTransaction dbTrans = null;
            if (dsParams != null && dsParams.Tables.Count > 0 && dsParams.Tables.Contains(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtParams = dsParams.Tables[EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME];
                try
                {
                    dbCon.Open();
                    DbCommand dbCmd = dbCon.CreateCommand();
                    dbTrans = dbCon.BeginTransaction();
                    dbCmd.Transaction = dbTrans;
                    string layoutKey = string.Empty;
                    string sql = string.Empty;
                    if (dtParams.Rows.Count > 0)
                    {
                        #region 写入主表信息
                        byte[] data = (byte[])dtParams.Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_PIC];
                        layoutKey = dtParams.Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();


                        //执行插入语句，插入一个空对象empty_blob()   
                        sql = string.Format(@"INSERT INTO EMS_LAYOUT_MAIN(LAYOUT_KEY,LAYOUT_NAME,LAYOUT_DESC,LAYOUT_PIC,
                                              STATE_FLAG,CREATE_TIME,CREATOR) 
                                              VALUES('{0}','{1}','{2}',@t,'0',GETDATE(),'{3}')",
                                              layoutKey.PreventSQLInjection(),
                                              Convert.ToString(dtParams.Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME]).PreventSQLInjection(),
                                              Convert.ToString(dtParams.Rows[0][EMS_LAYOUT_MAIN_FIELDS.LAYOUT_DESC]).PreventSQLInjection(),
                                              Convert.ToString(dtParams.Rows[0][EMS_LAYOUT_MAIN_FIELDS.CREATOR]).PreventSQLInjection());

                        dbCmd.CommandText = sql;
                        db.AddInParameter(dbCmd, "t", DbType.Binary, data);

                        dbCmd.ExecuteNonQuery();
                        dbCmd.Parameters.Clear();
                        #endregion

                        #region 写入明细数据
                        if (dsParams.Tables.Contains(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME))
                        {
                            EMS_LAYOUT_DETAIL_FIELDS detailField = new EMS_LAYOUT_DETAIL_FIELDS();
                            DataTable detailTable = dsParams.Tables[EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME];
                            for (int i = 0; i < detailTable.Rows.Count; i++)
                            {
                                string sqlCommand = DatabaseTable.BuildInsertSqlStatement(detailField, detailTable, i);
                                dbCmd.CommandText = sqlCommand;
                                dbCmd.ExecuteNonQuery();
                            }
                        }
                        #endregion

                        dbTrans.Commit();

                        dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                        dsReturn.ExtendedProperties.Add(PARAMETERS.INPUT_KEY, layoutKey);
                    }
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("InsertEquipmentLayout Error: " + ex.Message);
                }
                finally
                {
                    dbTrans.Dispose();
                    if (dbCon.State == ConnectionState.Open)
                        dbCon.Close();
                    dbCon.Dispose();
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }
            return dsReturn;
        }

        /// <summary>
        /// 更新设备布局。
        /// </summary>
        /// <param name="dsParams">包含设备布局数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。
        /// ExtendedProperties：OUTPUT_MESSAGE
        /// </returns>
        public DataSet UpdateEquipmentLayout(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (dsParams != null && dsParams.Tables.Count > 0 && dsParams.Tables.Contains(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME))
            {
                DbConnection dbConn = null;
                DbTransaction dbTran = null;
                try
                {
                    dbConn = db.CreateConnection();
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    string layoutkey = dsParams.ExtendedProperties[EMS_LAYOUT_DETAIL_FIELDS.LAYOUT_KEY].ToString();
                    string editor = dsParams.ExtendedProperties[EMS_LAYOUT_DETAIL_FIELDS.EDITOR].ToString();

                    DataTable detailTable = dsParams.Tables[EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME];
                    EMS_LAYOUT_DETAIL_FIELDS detailField = new EMS_LAYOUT_DETAIL_FIELDS();

                    string equipmentkey = string.Empty;                   
                    //-----------------------------------------记录删除的数据---------------------------------
                    string s = string.Format(@"SELECT T.* FROM EMS_LAYOUT_DETAIL T WHERE T.LAYOUT_KEY = '{0}' AND FLAG=0 ", 
                                               layoutkey.PreventSQLInjection());
                    DataTable dt = db.ExecuteDataSet(CommandType.Text, s).Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        equipmentkey = dr[EMS_LAYOUT_DETAIL_FIELDS.EQUIPMENT_KEY].ToString();

                        DataRow[] drs = detailTable.Select(string.Format("EQUIPMENT_KEY='{0}'", equipmentkey));

                        string s02 = string.Format(@"UPDATE EMS_LAYOUT_DETAIL 
                                                     SET FLAG = 1, EDITOR = '{2}', EDIT_TIME = GETDATE()
                                                     WHERE LAYOUT_KEY = '{0}' AND EQUIPMENT_KEY = '{1}'", 
                                                     layoutkey.PreventSQLInjection(),
                                                     equipmentkey.PreventSQLInjection(),
                                                     editor.PreventSQLInjection());

                        string s03 = string.Format(@"DELETE FROM EMS_LAYOUT_DETAIL WHERE LAYOUT_KEY = '{0}' AND EQUIPMENT_KEY = '{1}'",
                                                    layoutkey.PreventSQLInjection(), equipmentkey.PreventSQLInjection());

                        if (drs != null && drs.Length > 0)
                            db.ExecuteNonQuery(dbTran, CommandType.Text, s03);
                        else
                            db.ExecuteNonQuery(dbTran, CommandType.Text, s02);
                    }
                    //插入新记录
                    for (int i = 0; i < detailTable.Rows.Count; i++)
                    {
                        string sqlCommand = DatabaseTable.BuildInsertSqlStatement(detailField, detailTable, i);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    }
                    dbTran.Commit();
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTran != null)
                    {
                        dbTran.Rollback();
                    }
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("UpdateEquipmentLayout Error:" + ex.Message);
                }
                finally
                {
                    dbTran.Dispose();
                    if (dbConn.State == ConnectionState.Open)
                        dbConn.Close();
                    dbConn.Dispose();
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据为空");
            }
            return dsReturn;
        }

        /// <summary>
        /// 查询设备布局。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件数据的数据集对象。
        /// -----------------------------------------
        /// ExtendedProperties {LAYOUT_NAME}
        /// -----------------------------------------
        /// </param>
        /// <returns>包含设备布局的数据集对象。
        /// ExtendedProperties：OUTPUT_MESSAGE
        /// </returns>
        /// </returns>
        public DataSet SearchEquipmentLayout(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME))
            {
                string layoutName = Convert.ToString(dsParams.ExtendedProperties[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME]);
                try
                {
                    string sql = @"SELECT T.LAYOUT_KEY,T.LAYOUT_NAME,T.LAYOUT_DESC FROM EMS_LAYOUT_MAIN T WHERE T.STATE_FLAG=0";
                    if (layoutName.Length > 0)
                    {
                        sql += " AND T.LAYOUT_NAME='" + layoutName.PreventSQLInjection() + "'";
                    }
                    dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                    dsReturn.Tables[0].TableName = EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("SearchEquipmentLayout Error:" + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "传入数据为空");
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取设备布局明细数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件数据的数据集对象。
        /// -----------------------------------------
        /// ExtendedProperties {LAYOUT_KEY}
        /// -----------------------------------------
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。
        /// ExtendedProperties：OUTPUT_MESSAGE
        /// </returns>
        public DataSet GetEquipmentLayout(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY))
            {
                try
                {
                    string layoutKey = Convert.ToString(dsParams.ExtendedProperties[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY]);
                    string sql = string.Format(@"SELECT T.LAYOUT_PIC FROM EMS_LAYOUT_MAIN T WHERE T.LAYOUT_KEY = '{0}'", layoutKey);
                    DataTable mainTable = new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable(EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME);
                    sql = string.Format(@"SELECT T.*,A.PARENT_EQUIPMENT_KEY,A.ISMULTICHAMBER,A.CHAMBER_TOTAL,
                                            A.CHAMBER_INDEX,B.EQUIPMENT_STATE_KEY,B.EQUIPMENT_STATE_NAME
                                          FROM EMS_LAYOUT_DETAIL T
                                          LEFT JOIN EMS_EQUIPMENTS A ON T.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                          INNER JOIN EMS_EQUIPMENT_STATES B ON A.EQUIPMENT_STATE_KEY = B.EQUIPMENT_STATE_KEY
                                          WHERE T.FLAG=0
                                          AND LAYOUT_KEY = '{0}'", 
                                          layoutKey.PreventSQLInjection());
                    DataTable detailTable = new DataView(db.ExecuteDataSet(CommandType.Text, sql).Tables[0]).ToTable(EMS_LAYOUT_DETAIL_FIELDS.DATABASE_TABLE_NAME);

                    dsReturn.Tables.Add(mainTable);
                    dsReturn.Tables.Add(detailTable);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    LogService.LogError("GetEquipmentLayout Error:" + ex.Message);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "传入数据为空");
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除设备布局。
        /// </summary>
        /// <param name="dsParams">
        /// 包含设备布局主键的数据集对象。
        /// ----------------------------------------
        /// ExtendedProperties：LAYOUT_KEY
        /// ----------------------------------------
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。
        /// ExtendedProperties：OUTPUT_MESSAGE
        /// </returns>
        public DataSet DeleteEquipmentLayout(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY))
            {
                try
                {
                    string layoutKey = dsParams.ExtendedProperties[EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY].ToString();
                    string sql = "UPDATE EMS_LAYOUT_MAIN A  SET A.STATE_FLAG=1 WHERE A.LAYOUT_KEY='" + layoutKey.PreventSQLInjection() + "'";
                    db.ExecuteNonQuery(CommandType.Text, sql);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("DeleteEquipmentLayout Error:" + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "传入数据为空");
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得单个设备的记录——事件历史，加工历史
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ---------------------------------------------------
        /// ExtendedProperties：
        /// {EQUIPMENT_KEY}
        /// {CREATE_TIME}
        /// {EDIT_TIME}
        /// ---------------------------------------------------
        /// </param>
        /// <returns>
        /// 包含单个设备的记录——事件历史，加工历史的数据集对象。
        /// --------------------------------------------------
        /// ExtendedProperties：{OUTPUT_MESSAGE}
        /// --------------------------------------------------
        /// </returns>
        /// Owner by genchille.yang 2012-03-25 13:51:13
        public DataSet GetSingleLayoutEventDoWorkHistory(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            List<string> sqlCommand = new List<string>();
            string sql01 = string.Empty, sql02 = string.Empty;
            DbConnection conn = db.CreateConnection();
            conn.Open();
            try
            {
                if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY)
                    && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.CREATE_TIME)
                    && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.EDIT_TIME))
                {
                    string equipmentKey = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString();
                    string startDate = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.CREATE_TIME].ToString();
                    string endDate = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.EDIT_TIME].ToString();
                    //获得事件历史
                    sql01 = string.Format(@"SELECT T.*, CONVERT(VARCHAR,T.EDIT_TIME,120) as  End_date,
                                               CONVERT(VARCHAR,T.CREATE_TIME,120) as  Start_date,
                                               T1.EQUIPMENT_STATE_NAME,
                                               T1.EQUIPMENT_STATE_TYPE,
                                               T2.EQUIPMENT_NAME,
                                               DATEDIFF(hh,T.CREATE_TIME,ISNULL(T.EDIT_TIME,GETDATE())) CONTINUEHOURS
                                            FROM EMS_STATE_EVENT T, EMS_EQUIPMENT_STATES T1, EMS_EQUIPMENTS T2
                                            WHERE T.EQUIPMENT_FROM_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                            AND T.ISCURRENT > 0
                                            AND T.EQUIPMENT_KEY = T2.EQUIPMENT_KEY
                                            AND T.EQUIPMENT_KEY = '{0}'
                                            AND T.CREATE_TIME BETWEEN '{1}' AND '{2}'", 
                                            equipmentKey.PreventSQLInjection(),
                                            startDate.PreventSQLInjection(),
                                            endDate.PreventSQLInjection());
                    //获得加工历史
                    sql02 = string.Format(@"SELECT T.PIECE_KEY,T.LOT_NUMBER, --批次号
	                                               T.PART_NAME, --产品
	                                               SUM(CASE T.ACTIVITY WHEN 'TRACKIN' THEN T.QUANTITY ELSE 0 END) QUANTITY_IN, --送入数
                                                   MAX(CASE T.ACTIVITY WHEN 'TRACKIN' THEN T.EDIT_TIME END) IN_EDIT_TIME, --送入时间
                                                   MAX(CASE T.ACTIVITY WHEN 'TRACKIN' THEN T.OPERATOR END) MOVE_IN_OPERATOR, --送入操作者
                                                   SUM(CASE T.ACTIVITY WHEN 'TRACKOUT' THEN T.QUANTITY ELSE 0 END) QUANTITY_OUT, --送出数
                                                   MAX(CASE T.ACTIVITY WHEN 'TRACKOUT' THEN T.EDIT_TIME END) OUT_EDIT_TIME, --送出时间
                                                   MAX(CASE T.ACTIVITY WHEN 'TRACKOUT' THEN T.OPERATOR END) MOVE_OUT_OPERATOR, --送出操作者
                                                   SUM(CASE T.ACTIVITY WHEN 'SETLOSSBONUS' THEN T.QUANTITY ELSE 0 END) LOSS_QUANTITY_OUT --送出数
                                            FROM 
                                            (
	                                              SELECT A.OPERATOR, --操作人
                                                       A.PIECE_KEY,
                                                       A.EQUIPMENT_KEY,
                                                       A1.LOT_NUMBER, --批次号
                                                       A2.PART_NAME, --产品               
                                                       A.ACTIVITY, --类别
                                                       A.EDIT_TIME, --作业时间
                                                       SUM(ISNULL(A.QUANTITY_IN, 0)) QUANTITY --送入数
                                                  FROM WIP_TRANSACTION A, POR_LOT A1, POR_PART A2
                                                  WHERE UPPER(A.ACTIVITY) = 'TRACKIN' --送入数
                                                  AND A.PIECE_KEY = A1.LOT_KEY
                                                  AND A1.PART_VER_KEY = A2.PART_KEY
                                                  AND A.EQUIPMENT_KEY = '{0}'
                                                  AND A.TIME_STAMP BETWEEN '{1}' AND '{2}'
                                                  GROUP BY A.OPERATOR, --操作人
                                                          A.PIECE_KEY,
                                                          A.EQUIPMENT_KEY,
                                                          A1.LOT_NUMBER, --批次号
                                                          A2.PART_NAME, --产品               
                                                          A.ACTIVITY, --类别
                                                          A.EDIT_TIME --作业时间
                                                 UNION ALL
                                                 SELECT B.OPERATOR,
                                                       B.PIECE_KEY,
                                                       B.EQUIPMENT_KEY,
                                                       B1.LOT_NUMBER,
                                                       B2.PART_NAME,
                                                       B.ACTIVITY,
                                                       B.EDIT_TIME,
                                                       SUM(ISNULL(B.QUANTITY_OUT, 0)) QUANTITY
                                                  FROM WIP_TRANSACTION B, POR_LOT B1, POR_PART B2
                                                  WHERE UPPER(B.ACTIVITY) = 'TRACKOUT' --送出数
                                                  AND B.PIECE_KEY = B1.LOT_KEY
                                                  AND B1.PART_VER_KEY = B2.PART_KEY
                                                  AND B.EQUIPMENT_KEY = '{0}'
                                                  AND B.TIME_STAMP BETWEEN '{1}' AND '{2}'
                                                  GROUP BY B.OPERATOR,
                                                          B.PIECE_KEY,
                                                          B.EQUIPMENT_KEY,
                                                          B1.LOT_NUMBER,
                                                          B2.PART_NAME,
                                                          B.ACTIVITY,
                                                          B.EDIT_TIME
                                                 UNION ALL
                                                 SELECT C.OPERATOR,
                                                       C.PIECE_KEY,
                                                       C.EQUIPMENT_KEY,
                                                       C1.LOT_NUMBER,
                                                       C2.PART_NAME,
                                                       C.ACTIVITY,
                                                       C.EDIT_TIME,
                                                       SUM(ISNULL(C.QUANTITY_OUT, 0)) QUANTITY
                                                  FROM WIP_TRANSACTION C, POR_LOT C1, POR_PART C2
                                                  WHERE UPPER(C.ACTIVITY) = 'SETLOSSBONUS' --报废数
                                                  AND C.PIECE_KEY = C1.LOT_KEY
                                                  AND C1.PART_VER_KEY = C2.PART_KEY
                                                  AND C.EQUIPMENT_KEY = '{0}'
                                                  AND C.TIME_STAMP BETWEEN '{1}' AND '{2}'
                                                 GROUP BY C.OPERATOR,
                                                          C.PIECE_KEY,
                                                          C.EQUIPMENT_KEY,
                                                          C1.LOT_NUMBER,
                                                          C2.PART_NAME,
                                                          C.ACTIVITY,
                                                          C.EDIT_TIME
                                            ) T
                                            GROUP BY T.PIECE_KEY, T.LOT_NUMBER, T.PART_NAME", 
                                            equipmentKey.PreventSQLInjection(),
                                            startDate.PreventSQLInjection(),
                                            endDate.PreventSQLInjection());

                    DataSet ds01 = db.ExecuteDataSet(CommandType.Text, sql01);
                    DataTable dtStateEvents = ds01.Tables[0];
                    dtStateEvents.TableName = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME;

                    DataSet ds02 = db.ExecuteDataSet(CommandType.Text, sql02);
                    DataTable dtDoWorkHistory = ds02.Tables[0];
                    dtDoWorkHistory.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;

                    dsReturn.Tables.Add(dtStateEvents.Copy());
                    dsReturn.Tables.Add(dtDoWorkHistory.Copy());
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("GetSingleLayoutEventDoWorkHistory Error:" + ex.Message);
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得当前设备的详细信息——加工批次数，电池片数，
        /// 产品分布情况，最后操作人，事件备注等信息
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ---------------------------------------------------
        /// ExtendedProperties：
        /// {EQUIPMENT_KEY}
        /// {CREATE_TIME}
        /// {EDIT_TIME}
        /// --------------------------------------------------- 
        /// </param>
        /// <returns>
        /// 包含前设备的详细信息——加工批次数，电池片数，
        /// 产品分布情况，最后操作人，事件备注等信息的数据集对象。
        /// --------------------------------------------------
        /// ExtendedProperties：{OUTPUT_MESSAGE}
        /// --------------------------------------------------
        /// </returns>
        /// Modify by genchille.yang 2012-08-20 16:56:21
        public DataSet GetCurrentEquWorkList(DataSet dsParams)
        { 
            DataSet dsReturn=new DataSet();
            if (dsParams != null && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY)
                && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.CREATE_TIME)
                && dsParams.ExtendedProperties.ContainsKey(EMS_STATE_EVENT_FIELDS.EDIT_TIME))
            {
                string equipmentKey = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.EQUIPMENT_KEY].ToString();       
                string startDate = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.CREATE_TIME].ToString();
                string endDate = dsParams.ExtendedProperties[EMS_STATE_EVENT_FIELDS.EDIT_TIME].ToString();

                try
                {
                    string s = string.Format(@"SELECT T.OPERATION_KEY
                                               FROM EMS_OPERATION_EQUIPMENT T
                                               WHERE T.EQUIPMENT_KEY = '{0}'", 
                                               equipmentKey.PreventSQLInjection());
                    string _operationkey = Convert.ToString(db.ExecuteScalar(CommandType.Text, s));

                    //设备的基本信息
                    string s00 = string.Format(@"SELECT E0.EQUIPMENT_KEY,E0.EQUIPMENT_NAME,E0.EQUIPMENT_CODE,E0.EQUIPMENT_ASSETSNO,E2.EQUIPMENT_STATE_NAME,E2.DESCRIPTION,
                                                       E4.DESCRIPTION REMARK,E4.EDITOR,E4.EDIT_TIME,E0.EQUIPMENT_WPH,E0.EQUIPMENT_AV_TIME,E0.EQUIPMENT_TRACT_TIME,E9.USERNAME
                                                FROM EMS_EQUIPMENTS E0,EMS_OPERATION_EQUIPMENT E1,EMS_EQUIPMENT_STATES E2, EMS_STATE_EVENT E4,RBAC_USER E9
                                                WHERE E0.EQUIPMENT_KEY = E1.EQUIPMENT_KEY AND E0.EQUIPMENT_STATE_KEY = E2.EQUIPMENT_STATE_KEY
                                                AND E4.EQUIPMENT_KEY = E0.EQUIPMENT_KEY AND E0.EQUIPMENT_KEY = '{0}'
                                                AND E9.BADGE = E4.EDITOR 
                                                AND E4.ISCURRENT =(SELECT MAX(A0.ISCURRENT) - 1
				                                                   FROM EMS_STATE_EVENT A0
				                                                   WHERE A0.EQUIPMENT_KEY = E0.EQUIPMENT_KEY)", 
                                                equipmentKey.PreventSQLInjection());
                    DataTable dt00 = db.ExecuteDataSet(CommandType.Text, s00).Tables[0];
                    dt00.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Merge(dt00, false, MissingSchemaAction.Add);
                    //获取当前运行批次
                    string s01 = string.Format(@"SELECT T1.EQUIPMENT_KEY, --运行的批次数，片数
                                                   COUNT(T.LOT_KEY) RUN_LOC_COUNT,
                                                   SUM(T.QUANTITY) RUN_CELL_NUMBER,T.PRO_ID AS TYPE
                                                FROM POR_LOT T, EMS_LOT_EQUIPMENT T1
                                                WHERE T.LOT_KEY = T1.LOT_KEY
                                                AND T1.END_TIMESTAMP IS NULL
                                                AND T1.EQUIPMENT_KEY = '{0}'
                                                GROUP BY T1.EQUIPMENT_KEY,T.PRO_ID", 
                                                equipmentKey.PreventSQLInjection());
                    DataTable dt01 = db.ExecuteDataSet(CommandType.Text, s01).Tables[0];
                    dt01.TableName = "RUN_LOT";
                    dsReturn.Merge(dt01, false, MissingSchemaAction.Add);                   
                    //获取当前暂停批次
                    string s02 = string.Format(@"SELECT T3.ROUTE_OPERATION_VER_KEY, --暂停的批次数，片数
	                                                   COUNT(T2.LOT_KEY) HOLD_LOC_COUNT,
	                                                   SUM(T2.QUANTITY) HOLD_CELL_NUMBER,T2.PRO_ID AS TYPE
                                                FROM POR_LOT T2,POR_ROUTE_STEP T3,
                                                   (SELECT A.PIECE_KEY
                                                    FROM WIP_TRANSACTION A
                                                    WHERE A.ACTIVITY = 'HOLD'
                                                    AND A.EDIT_TIME BETWEEN '{0}' AND '{1}'
                                                    GROUP BY A.PIECE_KEY) T4
                                                WHERE T2.CUR_STEP_VER_KEY = T3.ROUTE_STEP_KEY
                                                AND T2.LOT_KEY = T4.PIECE_KEY
                                                AND T2.STATUS = 1 --已激活
                                                AND T3.ROUTE_OPERATION_VER_KEY = '{2}'
                                                GROUP BY T3.ROUTE_OPERATION_VER_KEY,T2.PRO_ID", 
                                                startDate.PreventSQLInjection(),
                                                endDate.PreventSQLInjection(),
                                                _operationkey.PreventSQLInjection());
                    DataTable dt02 = db.ExecuteDataSet(CommandType.Text, s02).Tables[0];
                    dt02.TableName = "HOLD_LOT";
                    dsReturn.Merge(dt02, false, MissingSchemaAction.Add);
                    
                    //等待的批次数
                    string s03 = string.Format(@"SELECT T6.ROUTE_OPERATION_VER_KEY,
                                                   COUNT(T5.LOT_KEY) WAIT_LOC_COUNT, --等待的批次数，片数
                                                   SUM(T5.QUANTITY) WAIT_CELL_NUMBER,T5.PRO_ID AS TYPE
                                                FROM POR_LOT T5, POR_ROUTE_STEP T6
                                                WHERE T5.CUR_STEP_VER_KEY = T6.ROUTE_STEP_KEY
                                                AND T5.STATUS = 1 --已激活
                                                AND T5.STATE_FLAG = 0 -- 'QAITINGFORTRACKIN'
                                                AND T5.START_WAIT_TIME BETWEEN '{0}' AND '{1}'
                                                AND T6.ROUTE_OPERATION_VER_KEY = '{2}'
                                                AND EXISTS (SELECT COUNT(0) FROM EMS_EQUIPMENTS A, FMM_LOCATION_RET B
			                                                WHERE A.LOCATION_KEY = B.LOCATION_KEY
		                                                    AND B.LOCATION_LEVEL = 9
		                                                    AND B.PARENT_LOC_LEVEL = 5
		                                                    AND B.PARENT_LOC_KEY = T5.FACTORYROOM_KEY
		                                                    AND A.EQUIPMENT_KEY = '{3}')
                                                GROUP BY T6.ROUTE_OPERATION_VER_KEY,T5.PRO_ID", 
                                                startDate.PreventSQLInjection(),
                                                endDate.PreventSQLInjection(),
                                                _operationkey.PreventSQLInjection(),
                                                equipmentKey.PreventSQLInjection());
                    DataTable dt03 = db.ExecuteDataSet(CommandType.Text, s03).Tables[0];
                    dt03.TableName = "WAITING_LOT";
                    dsReturn.Merge(dt03, false, MissingSchemaAction.Add);
                    
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    LogService.LogError("GetCurrentEquWorkList Error:" + ex.Message);
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                }
            }
            return dsReturn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface.EquipmentManagement;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.EMS
{
    /// <summary>
    /// 设备数据管理类。
    /// </summary>
    public class Equipments : AbstractEngine, IEquipments
    {
        private Database db;//数据库操作对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Equipments()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize(){}
        /// <summary>
        /// 获取设备信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="equipmentName">设备名称。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">输出参数，总页数。</param>
        /// <param name="records">输出参数，总记录数。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        public DataSet GetEquipments(DataSet dsParams, string equipmentName, int pageNo, int pageSize,
                                     out int pages, out int records)
        {
            DataSet dsReturn = new DataSet();

            pages = 0;
            records = 0;

            try
            {
                EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

                #region 需要查询的数据表字段。

                List<string> interestColumns = new List<string>();

                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);             //设备主键
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);            //设备名称
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_ASSETSNO);        //资产编号 
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_DESCRIPTION);               //描述
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TYPE);            //设备类型
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE);            //设备编码

                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_MODE);            //设备型号
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MINQUANTITY);               //设备最小加工量
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_MAXQUANTITY);               //设备最大加工量
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_STATE_KEY);       //设备状态主键
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY);//设备转变状态规律
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY);       //设备组主键
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY);              //设备所属区域
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY);      //父设备主键
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER);                 //是否腔体
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISBATCH);                   //是否支持批处理
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER);            //是否是多腔体设备
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);             //腔体总数
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_INDEX);             //腔体编号               
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_AV_TIME);         //设备Av_Time值 
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_TRACT_TIME);      //设备Tract_Time值
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_WPH);             //设备WPH值
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_REAL_KEY);        //虚拟设备对应的物理设备
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CREATOR);                   //创建者
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIMEZONE_KEY);       
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME);               //创建时间
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR);                    //编辑者
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME);

                #endregion

                #region 查询条件。

                Conditions conditions = new Conditions();

                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];
                    if (inputParamDataTable != null && inputParamDataTable.Columns.Contains(PARAMETERS_INPUT.FIELD_KEY))
                    {
                        //行循环
                        foreach (DataRow row in inputParamDataTable.Rows)
                        {
                            object key = row[PARAMETERS_INPUT.FIELD_KEY];
                            if (key == null || key == DBNull.Value)
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, DatabaseCompareOperator.Null, string.Empty);
                            }
                            else
                            {
                                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, DatabaseCompareOperator.Equal, key.ToString());
                            }
                        }
                    }
                }

                #endregion

                if (!string.IsNullOrEmpty(equipmentName))
                {
                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, DatabaseCompareOperator.Like, string.Format("{0}%", equipmentName));
                }
                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentsFields, interestColumns, conditions);

                if (pageNo > 0 && pageSize > 0)
                {
                    //分页查询。
                    AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, dsReturn, EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);
                }
                else
                {
                    db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipments Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取设备数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备数据的数据集对象。</returns>
        public DataSet GetEquipments(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            if (dsParams != null &&
                dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY) &&
                dsParams.ExtendedProperties.ContainsKey(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY))
            {
                string equipmentLocationKey = dsParams.ExtendedProperties[EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY].ToString();
                string equipmentGroupKey = dsParams.ExtendedProperties[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].ToString();

                try
                {
                    EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

                    List<string> interestColumns = new List<string>();

                    interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);
                    interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);

                    Conditions conditions = new Conditions();

                    if (!string.IsNullOrEmpty(equipmentGroupKey))
                    {
                        conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, DatabaseCompareOperator.Equal, equipmentGroupKey);
                    }

                    if (!string.IsNullOrEmpty(equipmentLocationKey))
                    {
                        conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, DatabaseCompareOperator.Equal, equipmentLocationKey);
                    }

                    string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentsFields, interestColumns, conditions);

                    db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });

                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                }
                catch (Exception ex)
                {
                    dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);

                    LogService.LogError("GetEquipment Error: " + ex.Message);
                }
            }
            else
            {
                dsReturn.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取所有设备数据。
        /// </summary>
        /// <param name="equipmentName">设备名称，左右模糊匹配。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">输出参数，总页数。</param>
        /// <param name="records">输出参数，总记录数。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        public DataSet GetAllChildEquipments(string equipmentName, int pageNo, int pageSize, 
                                             out int pages, out int records)
        {
            DataSet resDS = new DataSet();

            pages = 0;
            records = 0;

            try
            {
                string sqlString = @"SELECT E.*
                                     FROM EMS_EQUIPMENTS E
                                     WHERE E.EQUIPMENT_KEY NOT IN (   SELECT PARENT_EQUIPMENT_KEY
                                                                      FROM EMS_EQUIPMENTS
                                                                      WHERE PARENT_EQUIPMENT_KEY IS NOT NULL
                                                                   )"; 
                if (!string.IsNullOrEmpty(equipmentName))
                {
                    sqlString += string.Format(" AND E.EQUIPMENT_NAME LIKE '%{0}%'", equipmentName);
                }
                if (pageNo > 0 && pageSize > 0)
                {
                    AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);
                }
                else
                {
                    db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });
                }

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("GetAllChildEquipments Error: " + ex.Message);
            }
            return resDS;
        }
        /// <summary>
        /// 获取可以包含子设备的设备数据。
        /// </summary>
        /// <returns>包含父设备数据的数据集对象。</returns>
        public DataSet GetAllParentEquipments()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

                #region 数据字段
                List<string> interestColumns = new List<string>();
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY);
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                interestColumns.Add(EMS_EQUIPMENTS_FIELDS.FIELD_CHAMBER_TOTAL);
                #endregion

                Conditions conditions = new Conditions();

                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_ISCHAMBER, DatabaseCompareOperator.Equal, "0");
                conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_ISMULTICHAMBER, DatabaseCompareOperator.Equal, "1");

                string sqlString = DatabaseTable.BuildQuerySqlStatement(equipmentsFields, interestColumns, conditions);

                db.LoadDataSet(CommandType.Text, sqlString, dsReturn, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAllParentEquipments Error: " + ex.Message);
            }

            return dsReturn;
        }
        /// <summary>
        /// 获取可以包含子设备的设备数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备数据的数据集对象。</returns>
        public DataSet GetParentEquipments(DataSet dsParams)
        {
            DataSet resDS = new DataSet();
            int pages = 0;
            int records = 0;
            string queryValue = string.Empty;
            string condition = string.Empty;

            if (dsParams != null &&
                dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                dsParams.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE)&&
                dsParams.ExtendedProperties.ContainsKey(PROPERTY_FIELDS.USER_NAME)&&
                dsParams.ExtendedProperties.ContainsKey("FLAG"))
            {
                queryValue = dsParams.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                int pageNo = Convert.ToInt32(dsParams.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(dsParams.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);
                string userName=dsParams.ExtendedProperties[PROPERTY_FIELDS.USER_NAME].ToString();
                string flag = dsParams.ExtendedProperties["FLAG"].ToString();

                try
                {
                    string sqlString = string.Empty;
                   if (flag.Trim().Equals("Q"))
                        condition = "IN";
                    else
                        condition = "NOT IN";

                   sqlString = string.Format(@"  SELECT E.*, A.EQUIPMENT_STATE_NAME
                                               FROM EMS_EQUIPMENTS E, EMS_EQUIPMENT_STATES A
                                              WHERE E.ISCHAMBER = 0
                                                AND E.EQUIPMENT_KEY {0}
                                                    (SELECT D.EQUIPMENT_KEY
                                                       FROM EMS_LAYOUT_DETAIL D, EMS_LAYOUT_MAIN M
                                                      WHERE D.LAYOUT_KEY = M.LAYOUT_KEY
                                                        AND M.STATE_FLAG = 0
                                                        AND D.PIC_TYPE = 'E'
                                                        AND D.FLAG=0
                                                        AND D.EQUIPMENT_KEY IS NOT NULL)
                                                AND E.EQUIPMENT_STATE_KEY = A.EQUIPMENT_STATE_KEY
                                                AND E.EQUIPMENT_KEY IN
                                                    (SELECT A0.EQUIPMENT_KEY
                                                       FROM EMS_EQUIPMENTS      A0,
                                                            FMM_LOCATION_LINE   A1,
                                                            FMM_PRODUCTION_LINE A2
                                                      WHERE A0.LOCATION_KEY = A1.LOCATION_KEY
                                                        AND A1.LINE_KEY = A2.PRODUCTION_LINE_KEY
                                                        AND A2.LINE_NAME IN (SELECT DISTINCT T2.LINE_NAME
                                                                               FROM RBAC_USER           T,
                                                                                    RBAC_USER_IN_ROLE   T1,
                                                                                    RBAC_ROLE_OWN_LINES T2
                                                                              WHERE T.USER_KEY = T1.USER_KEY
                                                                                AND T1.ROLE_KEY = T2.ROLE_KEY
                                                                                AND T.BADGE = '{1}'))", condition, userName);

                    if (!string.IsNullOrEmpty(queryValue))
                    {
                        sqlString += string.Format(" AND UPPER(E.EQUIPMENT_CODE) LIKE '{0}%'", queryValue);
                    }
                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("GetParentEquipments Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }
            return resDS;
        }
        /// <summary>
        /// 获取不包含子设备的设备数据。
        /// </summary>
        /// <param name="equipmentName">设备名称，左右模糊匹配。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">输出参数，总页数。</param>
        /// <param name="records">输出参数，总记录数。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        public DataSet GetChildEquipments(DataSet reqDS)
        {
            DataSet resDS = new DataSet();
             int pages = 0;
            int records = 0;

            if (reqDS != null &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGENO) &&
                reqDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_PAGESIZE))
            {
                string equipmentName = reqDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString();
                int pageNo = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGENO]);
                int pageSize = Convert.ToInt32(reqDS.ExtendedProperties[PARAMETERS.INPUT_PAGESIZE]);

                try
                {
                    string sqlString = @"SELECT E.*,A.EQUIPMENT_NAME AS PARENT_EQUIPMENT_NAME
                                        FROM EMS_EQUIPMENTS E,EMS_EQUIPMENTS A
                                        WHERE E.ISCHAMBER = 1
                                        AND E.EQUIPMENT_KEY NOT IN (SELECT D.EQUIPMENT_KEY
                                                                    FROM EMS_LAYOUT_DETAIL D, EMS_LAYOUT_MAIN M
                                                                    WHERE D.LAYOUT_KEY = M.LAYOUT_KEY
                                                                    AND M.STATE_FLAG = 0
                                                                    AND D.PIC_TYPE = 'C'
                                                                    AND D.EQUIPMENT_KEY IS NOT NULL)
                                        AND E.PARENT_EQUIPMENT_KEY=A.EQUIPMENT_KEY";
                    if (!string.IsNullOrEmpty(equipmentName))
                    {
                        sqlString += string.Format(" AND E.EQUIPMENT_NAME LIKE '%{0}%'", equipmentName);
                    }
                    if (pageNo > 0 && pageSize > 0)
                    {
                        AllCommonFunctions.CommonPagingData(sqlString, pageNo, pageSize, out pages, out records, db, resDS, EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME);
                    }
                    else
                    {
                        db.LoadDataSet(CommandType.Text, sqlString, resDS, new string[] { EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME });
                    }

                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, string.Empty);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_PAGES, pages);
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_RECORDS, records);
                }
                catch (Exception ex)
                {
                    resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, ex.Message);
                    LogService.LogError("GetChildEquipments Error: " + ex.Message);
                }
            }
            else
            {
                resDS.ExtendedProperties.Add(PARAMETERS.OUTPUT_MESSAGE, "提交数据不存在,请重新提交!");
            }
            return resDS;
        }
        /// <summary>
        /// 获取初始设备改变状态。
        /// </summary>
        /// <returns>包含设备改变状态的数据集对象。</returns>
        public DataSet GetInitEquipmentChangeState()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();

                    string s = @"SELECT T.*,T1.EQUIPMENT_CHANGE_STATE_KEY,T1.EQUIPMENT_FROM_STATE_KEY,T1.EQUIPMENT_TO_STATE_KEY
                                FROM EMS_EQUIPMENT_STATES T, EMS_EQUIPMENT_CHANGE_STATES T1
                                WHERE T.EQUIPMENT_STATE_KEY = T1.EQUIPMENT_FROM_STATE_KEY
                                AND UPPER(T.EQUIPMENT_STATE_NAME) = 'INIT' ";
                    DataTable dt = db.ExecuteDataSet(CommandType.Text, s).Tables[0];
                    dt.TableName = EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME;
                    dsReturn.Tables.Add(dt.Copy());
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetInitEquipmentChangeState Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 新增设备。
        /// </summary>
        /// <param name="dsParams">包含设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet InsertEquipments(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME) && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable equipmentsDataTable = dsParams.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];
                    DataTable dtSaveEvent = dsParams.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME];
                    DataTable dtUpdateEvent = dsParams.Tables[EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME_UPDATE];
                    //dtUpdateEvent.TableName = EMS_STATE_EVENT_FIELDS.DATABASE_TABLE_NAME;

                    EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();
                    EMS_STATE_EVENT_FIELDS equipmentStateEvent = new EMS_STATE_EVENT_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildInsertSqlStatements(equipmentsFields, equipmentsDataTable);
                    sqlStringList.Add(DatabaseTable.BuildInsertSqlStatements(equipmentStateEvent, dtSaveEvent)[0]);
                    sqlStringList.Add(DatabaseTable.BuildInsertSqlStatements(equipmentStateEvent, dtUpdateEvent)[0]);

                    string equipmentKey = string.Empty;
                    string equipmentName = string.Empty;
                    string sqlString = string.Empty;
                    string createTime = string.Empty;
                    string equipmentChangeStateKey = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentsDataTable.Rows.Count > 0)
                    {
                        equipmentKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                        equipmentName = equipmentsDataTable.Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString();

                        equipmentChangeStateKey = equipmentsDataTable.Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CHANGE_STATE_KEY].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection dbCon = db.CreateConnection())
                    {
                        dbCon.Open();

                        using (DbTransaction dbTrans = dbCon.BeginTransaction())
                        {
                            try
                            {
                                #region Check Equipment Name

                                string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentsFields, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, equipmentName, dbTrans);

                                if (!string.IsNullOrEmpty(returnData))
                                {
                                    throw new Exception("${res:FanHai.Hemera.Modules.EMS.Equipments.M0001}");
                                }

                                #endregion

                                if (db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlString) > 0)
                                {
                                    createTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentsFields, 
                                                                                            EMS_EQUIPMENTS_FIELDS.FIELD_CREATE_TIME, 
                                                                                            EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                                                                            equipmentKey, 
                                                                                            dbTrans);

                                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlStringList[1]);
                                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlStringList[2]);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }
                               
                                dbTrans.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("InsertEquipments Error: " + ex.Message);
                                dbTrans.Rollback();
                                throw ex;
                            }
                            finally
                            {
                                dbCon.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty, createTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("InsertEquipments Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 更新设备数据。
        /// </summary>
        /// <param name="dsParams">包含设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateEquipments(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME) && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable equipmentsDataTable = dsParams.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();

                    foreach (DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();

                        Condition keyCondition;

                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();

                    List<string> sqlStringList = DatabaseTable.BuildUpdateSqlStatements(equipmentsFields, equipmentsDataTable, conditionsList);

                    string equipmentKey = string.Empty;
                    string equipmentCode = string.Empty;
                    string sqlString = string.Empty;
                    string editTime = string.Empty;

                    if (sqlStringList.Count > 0 && inputParamDataTable.Rows.Count > 0 && equipmentsDataTable.Rows.Count > 0)
                    {
                        equipmentKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                        equipmentCode = equipmentsDataTable.Rows[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE].ToString();

                        sqlString = sqlStringList[0];
                    }

                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                #region Check Equipment Name

                                if (!string.IsNullOrEmpty(equipmentCode))
                                {
                                    Conditions conditions = new Conditions();

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE, DatabaseCompareOperator.Equal, equipmentCode);

                                    conditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, DatabaseCompareOperator.NotEqual, equipmentKey);

                                    string returnData = AllCommonFunctions.GetSpecifyTableColumnData(equipmentsFields, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, conditions, transaction);

                                    if (!string.IsNullOrEmpty(returnData))
                                    {
                                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.Equipments.M0001}");
                                    }
                                }

                                #endregion

                                if (db.ExecuteNonQuery(transaction, CommandType.Text, sqlString) > 0)
                                {
                                    editTime = AllCommonFunctions.GetSpecifyTableColumnData(equipmentsFields, EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                        equipmentKey, transaction);
                                }
                                else
                                {
                                    throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                                }

                                transaction.Commit();
                            }
                            catch(Exception ex)
                            {
                                LogService.LogError("UpdateEquipments Error: " + ex.Message);
                                transaction.Rollback();

                                throw;
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty, editTime);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("UpdateEquipments Error: " + ex.Message);
            }

            return dsReturn;
        }

        /// <summary>
        /// 删除设备数据。
        /// </summary>
        /// <param name="dsParams">包含删除条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteEquipments(DataSet dsParams)
        {
            DataSet resDS = new DataSet();

            try
            {
                if (dsParams != null && dsParams.Tables.Contains(PARAMETERS_INPUT.DATABASE_TABLE_NAME))
                {
                    DataTable inputParamDataTable = dsParams.Tables[PARAMETERS_INPUT.DATABASE_TABLE_NAME];

                    #region Build Conditions List

                    List<Conditions> conditionsList = new List<Conditions>();

                    foreach (DataRow row in inputParamDataTable.Rows)
                    {
                        Conditions conditions = new Conditions();

                        Condition keyCondition;

                        if (row[PARAMETERS_INPUT.FIELD_KEY] != null && row[PARAMETERS_INPUT.FIELD_KEY] != DBNull.Value)
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_KEY].ToString());
                        }
                        else
                        {
                            keyCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(keyCondition);

                        Condition editorCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDITOR] != null && row[PARAMETERS_INPUT.FIELD_EDITOR] != DBNull.Value)
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDITOR].ToString());
                        }
                        else
                        {
                            editorCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDITOR,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editorCondition);

                        Condition editTimeCondition;

                        if (row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != null && row[PARAMETERS_INPUT.FIELD_EDIT_TIME] != DBNull.Value)
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Equal, row[PARAMETERS_INPUT.FIELD_EDIT_TIME].ToString());
                        }
                        else
                        {
                            editTimeCondition = new Condition(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_EDIT_TIME,
                                 DatabaseCompareOperator.Null, string.Empty);
                        }

                        conditions.Add(editTimeCondition);

                        conditionsList.Add(conditions);
                    }

                    #endregion

                    EMS_EQUIPMENTS_FIELDS equipmentsFields = new EMS_EQUIPMENTS_FIELDS();
                    //数据库删除操作
                    List<string> sqlStringList = DatabaseTable.BuildDeleteSqlStatements(equipmentsFields, conditionsList);

                    string sqlString = string.Empty;

                    if (sqlStringList.Count > 0)
                    {
                        sqlString = sqlStringList[0];
                    }

                    #region Check Is Parent Equipment

                    string equipmentKey = inputParamDataTable.Rows[0][PARAMETERS_INPUT.FIELD_KEY].ToString();

                    Conditions checkConditions = new Conditions();

                    checkConditions.Add(DatabaseLogicOperator.And, EMS_EQUIPMENTS_FIELDS.FIELD_PARENT_EQUIPMENT_KEY, DatabaseCompareOperator.Equal, equipmentKey);

                    List<string> interestColumns = new List<string>() { EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY };
                    //查询语句 查询已被删除的数据判定是否被删除
                    string checkSqlString = DatabaseTable.BuildQuerySqlStatement(equipmentsFields, interestColumns, checkConditions);
                    //执行查询命令
                    object scalar = db.ExecuteScalar(CommandType.Text, checkSqlString);

                    if (scalar != null && scalar != DBNull.Value)
                    {//结果不为空或有数据库默认返回值
                        throw new Exception("${res:FanHai.Hemera.Modules.EMS.Equipments.M0002}");
                    }

                    #endregion

                    if (db.ExecuteNonQuery(CommandType.Text, sqlString) > 0)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, string.Empty);
                    }
                    else
                    {
                        throw new Exception("${res:FanHai.Hemera.Module.Common.M0002}");
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, "${res:FanHai.Hemera.Module.Common.M0001}");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(resDS, ex.Message);
                LogService.LogError("DeleteEquipments Error: " + ex.Message);
            }

            return resDS;
        }
        /// <summary>
        /// 获取设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码，左匹配模糊查询。</param>
        /// <param name="equipmentName">设备名称，左匹配模糊查询。</param>
        /// <param name="equipmentType">设备类型。如果为空，则查询所有类型的设备。</param>
        /// <param name="pconfig">数据分页查询的配置对象。</param>
        /// <returns>包含设备数据信息的数据集对象。</returns>
        public DataSet GetEquipments(string equipmentCode, string equipmentName, string equipmentType, ref PagingQueryConfig pconfig)
        {
            string sql = "SELECT a.* FROM EMS_EQUIPMENTS a WHERE 1=1 ";
            if (!string.IsNullOrEmpty(equipmentCode))
            {
                sql += string.Format(" AND a.EQUIPMENT_CODE LIKE '{0}{1}'",equipmentCode.PreventSQLInjection(),"%");
            }
            if (!string.IsNullOrEmpty(equipmentName))
            {
                sql += string.Format(" AND a.EQUIPMENT_NAME LIKE '{0}{1}'",equipmentName.PreventSQLInjection(),"%");
            }
            if (!string.IsNullOrEmpty(equipmentType))
            {
                sql += string.Format(" AND a.EQUIPMENT_TYPE = '{0}'",equipmentType.PreventSQLInjection());
            }
            DataSet dsReturn = new DataSet();
            try
            {
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sql, pconfig.PageNo, pconfig.PageSize, out pages,
                    out records, db, dsReturn, EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME,"EQUIPMENT_CODE");
                pconfig.Pages = pages;
                pconfig.Records = records;
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipments Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工厂车间和工序名称查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName)
        {
            const string CONST_GET_EQUIPMENT_SQL = @"SELECT a.EQUIPMENT_KEY,
                                                            CASE WHEN a.EQUIPMENT_REAL_KEY=' ' THEN a.EQUIPMENT_KEY
                                                                 WHEN a.EQUIPMENT_REAL_KEY IS NULL THEN a.EQUIPMENT_KEY
                                                                 ELSE a.EQUIPMENT_REAL_KEY
                                                            END AS EQUIPMENT_REAL_KEY,a.EQUIPMENT_NAME,a.EQUIPMENT_CODE
                                                     FROM EMS_EQUIPMENTS a
                                                     LEFT JOIN EMS_OPERATION_EQUIPMENT b ON a.EQUIPMENT_KEY=b.EQUIPMENT_KEY
                                                     LEFT JOIN POR_ROUTE_OPERATION_VER c ON b.OPERATION_KEY=c.ROUTE_OPERATION_VER_KEY
                                                     LEFT JOIN V_LOCATION d              ON a.LOCATION_KEY=d.AREA_KEY
                                                     WHERE d.ROOM_KEY='{0}' AND c.ROUTE_OPERATION_NAME='{1}'";
            DataSet ds=new DataSet();
            try
            {
                ds = db.ExecuteDataSet(CommandType.Text,
                                       string.Format(CONST_GET_EQUIPMENT_SQL, factoryRoomKey.PreventSQLInjection(), operationName.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, ex.Message);
                LogService.LogError("GetEquipments Error: " + ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 根据工厂车间和工序名称查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName,string lineKey)
        {
            const string CONST_GET_EQUIPMENT_SQL = @"SELECT a.EQUIPMENT_KEY,
                                                            CASE WHEN a.EQUIPMENT_REAL_KEY=' ' THEN a.EQUIPMENT_KEY
                                                                 WHEN a.EQUIPMENT_REAL_KEY IS NULL THEN a.EQUIPMENT_KEY
                                                                 ELSE a.EQUIPMENT_REAL_KEY
                                                            END AS EQUIPMENT_REAL_KEY,
                                                            a.EQUIPMENT_NAME,a.EQUIPMENT_CODE,f.LINE_NAME,f.PRODUCTION_LINE_KEY AS LINE_KEY,
                                                            g.EQUIPMENT_STATE_NAME
                                                    FROM EMS_EQUIPMENTS a
                                                    LEFT JOIN EMS_OPERATION_EQUIPMENT b ON a.EQUIPMENT_KEY=b.EQUIPMENT_KEY
                                                    LEFT JOIN POR_ROUTE_OPERATION_VER c ON b.OPERATION_KEY=c.ROUTE_OPERATION_VER_KEY
                                                    LEFT JOIN V_LOCATION d              ON a.LOCATION_KEY=d.AREA_KEY
                                                    LEFT JOIN FMM_LOCATION_LINE e       ON d.AREA_KEY=e.LOCATION_KEY
                                                    LEFT JOIN FMM_PRODUCTION_LINE f     ON e.LINE_KEY=f.PRODUCTION_LINE_KEY
                                                    LEFT JOIN EMS_EQUIPMENT_STATES g     ON a.EQUIPMENT_STATE_KEY=g.EQUIPMENT_STATE_KEY
                                                    WHERE d.ROOM_KEY='{0}' AND c.ROUTE_OPERATION_NAME='{1}' {2}
                                                    ORDER BY a.EQUIPMENT_CODE";
            DataSet ds = new DataSet();
            try
            {
                string condition=string.Empty;
                if (!string.IsNullOrEmpty(lineKey))
                {
                    condition = string.Format("AND e.LINE_KEY='{0}' ", lineKey.PreventSQLInjection());
                }
                ds = db.ExecuteDataSet(CommandType.Text,
                                       string.Format(CONST_GET_EQUIPMENT_SQL, factoryRoomKey.PreventSQLInjection(), 
                                                                              operationName.PreventSQLInjection(),
                                                                              condition));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, ex.Message);
                LogService.LogError("GetEquipments Error: " + ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 根据设备编码查询设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码。</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// </returns>
        public DataSet GetEquipments(string equipmentCode)
        {
            string sql = string.Format(@"SELECT a.*,c.ROUTE_OPERATION_NAME,c.ROUTE_OPERATION_VER_KEY,e.PRODUCTION_LINE_KEY,e.LINE_CODE,e.LINE_NAME
                                        FROM EMS_EQUIPMENTS a
                                        INNER JOIN EMS_OPERATION_EQUIPMENT b ON b.EQUIPMENT_KEY=a.EQUIPMENT_KEY
                                        INNER JOIN POR_ROUTE_OPERATION_VER c ON c.ROUTE_OPERATION_VER_KEY=b.OPERATION_KEY
                                        INNER JOIN FMM_LOCATION_LINE d ON d.LOCATION_KEY=a.LOCATION_KEY
                                        INNER JOIN FMM_PRODUCTION_LINE e ON e.PRODUCTION_LINE_KEY=d.LINE_KEY
                                        WHERE a.EQUIPMENT_CODE='{0}'",
                                        equipmentCode.PreventSQLInjection());
            DataSet dsReturn = new DataSet();
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipments(string equipmentCode) Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetEquipments()
        {
            string sql = string.Format(@"SELECT a.*,c.ROUTE_OPERATION_NAME,c.ROUTE_OPERATION_VER_KEY,e.PRODUCTION_LINE_KEY,e.LINE_CODE,e.LINE_NAME
                                        FROM EMS_EQUIPMENTS a
                                        INNER JOIN EMS_OPERATION_EQUIPMENT b ON b.EQUIPMENT_KEY=a.EQUIPMENT_KEY
                                        INNER JOIN POR_ROUTE_OPERATION_VER c ON c.ROUTE_OPERATION_VER_KEY=b.OPERATION_KEY
                                        INNER JOIN FMM_LOCATION_LINE d ON d.LOCATION_KEY=a.LOCATION_KEY
                                        INNER JOIN FMM_PRODUCTION_LINE e ON e.PRODUCTION_LINE_KEY=d.LINE_KEY
                                        WHERE a.EQUIPMENT_CODE like '%AIV%'");
            DataSet dsReturn = new DataSet();
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEquipments() Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工厂车间、工序名称和线别主键查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineNames">包含线别名称的数组。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE，LINE_NAME，LINE_KEY,EQUIPMENT_STATE_NAME]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName, string[] lineNames)
        {
            const string CONST_GET_EQUIPMENT_SQL = @"SELECT a.EQUIPMENT_KEY,
                                                           CASE WHEN a.EQUIPMENT_REAL_KEY=' ' THEN a.EQUIPMENT_KEY
                                                                WHEN a.EQUIPMENT_REAL_KEY IS NULL THEN a.EQUIPMENT_KEY
                                                                ELSE a.EQUIPMENT_REAL_KEY
                                                            END AS EQUIPMENT_REAL_KEY,
                                                            a.EQUIPMENT_NAME,a.EQUIPMENT_CODE,f.LINE_NAME,f.PRODUCTION_LINE_KEY AS LINE_KEY,
                                                            g.EQUIPMENT_STATE_NAME
                                                    FROM EMS_EQUIPMENTS a
                                                    LEFT JOIN EMS_OPERATION_EQUIPMENT b ON a.EQUIPMENT_KEY=b.EQUIPMENT_KEY
                                                    LEFT JOIN POR_ROUTE_OPERATION_VER c ON b.OPERATION_KEY=c.ROUTE_OPERATION_VER_KEY
                                                    LEFT JOIN V_LOCATION d              ON a.LOCATION_KEY=d.AREA_KEY
                                                    LEFT JOIN FMM_LOCATION_LINE e       ON d.AREA_KEY=e.LOCATION_KEY
                                                    LEFT JOIN FMM_PRODUCTION_LINE f     ON e.LINE_KEY=f.PRODUCTION_LINE_KEY
                                                    LEFT JOIN EMS_EQUIPMENT_STATES g     ON a.EQUIPMENT_STATE_KEY=g.EQUIPMENT_STATE_KEY
                                                    WHERE d.ROOM_KEY='{0}' AND c.ROUTE_OPERATION_NAME='{1}' {2}
                                                    ORDER BY a.EQUIPMENT_CODE";
            DataSet ds = new DataSet();
            try
            {
                string condition = string.Empty;
                if (lineNames!=null &&　lineNames.Length > 0)
                {
                    condition = UtilHelper.BuilderWhereConditionString("f.LINE_NAME", lineNames);
                }
                ds = db.ExecuteDataSet(CommandType.Text,
                                       string.Format(CONST_GET_EQUIPMENT_SQL, factoryRoomKey.PreventSQLInjection(), 
                                                                              operationName.PreventSQLInjection(), 
                                                                              condition));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, ex.Message);
                LogService.LogError("GetEquipments Error: " + ex.Message);
            }
            return ds;
        }
        /// <summary>
        /// 根据设备主键获取设备的状态信息。
        /// </summary>
        /// <param name="equipmentKey">设备主键</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// [EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME,EQUIPMENT_STATE_TYPE,Equipment_STATE_CATEGORY,DESCRIPTION]
        /// </returns>
        public DataSet GetEquipmentState(string equipmentKey)
        {
            const string CONST_GET_EQUIPMENT_SQL = @"SELECT b.EQUIPMENT_STATE_KEY,b.EQUIPMENT_STATE_NAME,b.EQUIPMENT_STATE_TYPE,b.Equipment_STATE_CATEGORY,b.DESCRIPTION
                                                    FROM EMS_EQUIPMENTS a
                                                    LEFT JOIN EMS_EQUIPMENT_STATES b ON a.EQUIPMENT_STATE_KEY=b.EQUIPMENT_STATE_KEY
                                                    WHERE a.EQUIPMENT_KEY='{0}'";
            DataSet ds = new DataSet();
            try
            {
                ds = db.ExecuteDataSet(CommandType.Text,
                                       string.Format(CONST_GET_EQUIPMENT_SQL, equipmentKey.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(ds, ex.Message);
                LogService.LogError("GetEquipmentState Error: " + ex.Message);
            }
            return ds;
        }      
        
    }
}

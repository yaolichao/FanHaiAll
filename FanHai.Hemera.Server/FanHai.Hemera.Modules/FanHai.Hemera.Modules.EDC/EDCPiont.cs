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

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Share.Interface;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 抽检点数据管理类。
    /// </summary>
    public class EDCPiont : AbstractEngine, IEDCPiont
    {
        //数据库操作对象。
        private Database db;
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EDCPiont()
        {
            //ceate db
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 查询抽检点设置数据。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件的数据表对象。包含键值对的数据表（TOPRODUCT 可选,OPERATION_NAME 可选），可以为NULL。
        /// </param>
        /// <returns>
        /// 包含抽检点设置数据的数据集。
        /// [ROW_KEY（分组中最小的ROW_KEY），TOPRODUCT,PART_TYPE,OPERATION_NAME,POINT_STATUS,
        /// POINT_STATE_DESCRIPTION,EQUIPMENT_NAME(用逗号分隔开),EQUIPMENT_KEY(用逗号分隔开),
        /// ACTION_NAME,EDC_NAME,SP_NAME,GROUP_KEY(标识分组的键),GROUP_NAME(抽检点设置名称)]
        /// </returns>
        public DataSet SearchEdcPoint(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = 
                            @"SELECT t.ROW_KEY,t.TOPRODUCT,t.PART_TYPE,t.OPERATION_NAME,ISNULL(t.point_status,0) AS POINT_STATUS,
                               (SELECT ROUTE_OPERATION_VER_KEY 
	                            FROM POR_ROUTE_OPERATION_VER 
	                            WHERE ROUTE_OPERATION_NAME=t.OPERATION_NAME AND OPERATION_STATUS=1
	                            AND OPERATION_VERSION=(SELECT MAX(OPERATION_VERSION) 
						                               FROM POR_ROUTE_OPERATION_VER
						                               WHERE ROUTE_OPERATION_NAME=t.OPERATION_NAME)) OPERATION_KEY,                 
	                            CASE WHEN ISNULL(t.point_status,0)=0 THEN '未激活' 
	                               WHEN ISNULL(t.point_status,0)=1 THEN '已激活'
	                               WHEN ISNULL(t.point_status,0)=2 THEN '存档'
	                            END AS POINT_STATE_DESCRIPTION,
                              g.EQUIPMENT_NAME,g.EQUIPMENT_KEY,
                              t.ACTION_NAME,
                              (SELECT em.EDC_NAME FROM EDC_MAIN em WHERE t.EDC_KEY = em.EDC_KEY) AS EDC_NAME,
                              (SELECT es.SP_NAME FROM EDC_SP es WHERE t.SP_KEY = es.SP_KEY) AS SP_NAME,
                              t.GROUP_KEY,
                              t.GROUP_NAME,
                              r.ROUTE_NAME,
                              t.MUST_INPUT_FIELD,
                              t.EDIT_DESC
                            FROM EDC_POINT t
                            INNER JOIN
                              (
                                SELECT a.GROUP_KEY,
                                     dbo.ConcatEdcPointEquipmentName(a.GROUP_KEY) EQUIPMENT_NAME,
                                     dbo.ConcatEdcPointEquipmentKey(a.GROUP_KEY) EQUIPMENT_KEY,
                                     MIN(ROW_KEY) ROW_KEY
                                FROM EDC_POINT a
                                GROUP BY a.GROUP_KEY
                              ) g ON t.ROW_KEY=g.ROW_KEY AND t.GROUP_KEY=g.GROUP_KEY
                            LEFT JOIN POR_ROUTE_ROUTE_VER r ON r.ROUTE_ROUTE_VER_KEY=t.ROUTE_VER_KEY
                            WHERE t.POINT_STATUS<2 ";
                if (dtParams != null)
                {
                    Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.ContainsKey(EDC_POINT_FIELDS.FIELD_TOPRODUCT))
                    {
                        string toProduct = Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_TOPRODUCT]);
                        sqlCommand += " AND t.TOPRODUCT LIKE '%" + toProduct.PreventSQLInjection() + "%'";
                    }
                    if (htParams.ContainsKey(EDC_POINT_FIELDS.FIELD_OPERATION_NAME))
                    {
                        string operatioName = Convert.ToString(htParams[EDC_POINT_FIELDS.FIELD_OPERATION_NAME]);
                        sqlCommand += " AND t.OPERATION_NAME LIKE '%" + operatioName.PreventSQLInjection() + "%'";
                    }
                }
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchEdcPoint Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 创建抽检点设置数据。
        /// </summary>
        /// <param name="dataSet">
        /// 包含抽检点设置数据的数据集对象。包含一个键值对的数据表。
        /// </param>
        /// <returns>包含EDC名称对应参数数据和执行结果数据的数据集。</returns>
        public DataSet CreateEdcPoint(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    List<string> sqlCommandList = new List<string>();
                    DataTable dataTable = dataset.Tables[0];
                    Hashtable hashData =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string toProduct=Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_TOPRODUCT]);
                    string partType=Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_PART_TYPE]);
                    string toproduct=Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_TOPRODUCT]);
                    string actionName = Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_ACTION_NAME]);
                    string edcKey = Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_EDC_KEY]);
                    string operationName = Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_OPERATION_NAME]);
                    string routeKey = Convert.ToString(hashData[EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY]);
                    //获取EDC名称对应的参数数据集合。
                    string sqlCommand = string.Format(@"SELECT T.*,'' AS PARAM_INDEX,'' AS PARAM_TYPE
                                                        FROM BASE_PARAMETER T 
                                                        WHERE EXISTS(SELECT * FROM EDC_MAIN_PARAM EM 
                                                                     WHERE EM.EDC_KEY = '{0}' 
                                                                     AND EM.PARAM_KEY=T.PARAM_KEY)", edcKey.PreventSQLInjection());
                    DataSet dsEDCPoint = db.ExecuteDataSet(dbTran,CommandType.Text, sqlCommand);

                    //判断记录是否存在。
                    sqlCommand = string.Format(@"SELECT COUNT(*) FROM EDC_POINT
                                                WHERE POINT_STATUS = '1'
                                                AND ACTION_NAME='{0}'
                                                AND EDC_KEY='{1}'
                                                AND OPERATION_NAME='{2}'", 
                                                actionName.PreventSQLInjection(), 
                                                edcKey.PreventSQLInjection(), 
                                                operationName.PreventSQLInjection());
                    //工艺流程。
                    if (!string.IsNullOrEmpty(routeKey))
                    {
                        sqlCommand += string.Format(" AND ROUTE_VER_KEY='{0}'", routeKey.PreventSQLInjection());
                    }
                    else
                    {
                        sqlCommand += " AND ROUTE_VER_KEY IS NULL";
                    }
                    //产品号。
                    if (!string.IsNullOrEmpty(toProduct))
                    {
                        sqlCommand += " AND TOPRODUCT='" + toProduct.PreventSQLInjection() + "'";
                    }
                    else
                    {
                        sqlCommand += " AND TOPRODUCT IS NULL";
                    }
                    //成品类型
                    if (!string.IsNullOrEmpty(partType))
                    {
                        sqlCommand += " AND PART_TYPE='" + partType.PreventSQLInjection() + "'";
                    }
                    else
                    {
                        sqlCommand += " AND PART_TYPE IS NULL";
                    }

                    if (hashData.ContainsKey(EDC_POINT_FIELDS.FIELD_SP_KEY))
                    {
                        string spKey = Convert.ToString(hashData.ContainsKey(EDC_POINT_FIELDS.FIELD_SP_KEY));
                        sqlCommand += " AND SP_KEY = '" + spKey.PreventSQLInjection() + "' ";
                    }

                    string[] equipmentKeys = null;
                    string[] equipmentTexts = null;
                    //包含设备主键。
                    if (hashData.ContainsKey(EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY))
                    {
                        string equipmentKey = hashData[EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                        string equipmentText = hashData[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME].ToString(); //设备描述。
                        equipmentKeys = equipmentKey.Split(',');
                        equipmentTexts = equipmentText.Split(',');
                    }

                    int length = 0;
                    string edcpointKey = string.Empty;
                    while (true)
                    {
                        hashData.Remove(EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY);
                        hashData.Remove(EDC_POINT_FIELDS.FIELD_ROW_KEY);
                        hashData.Remove(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME);
                        hashData.Remove(EDC_POINT_FIELDS.FIELD_EDIT_TIME);

                        string equipmentText = string.Empty;
                        string equipmentKey=string.Empty;
                        //设备主键数组不为空，并且循环控制参数<设备长度。
                        if (equipmentKeys != null && length < equipmentKeys.Length)
                        {
                            equipmentKey= equipmentKeys[length].Trim();
                            sqlCommand += " AND EQUIPMENT_KEY = '" +equipmentKey.PreventSQLInjection()+ "' ";
                            equipmentText = string.Format("设备[{0}]", equipmentTexts[length].Trim());
                            hashData.Add(EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                            length++;
                        }
                        else
                        {
                            sqlCommand += " AND EQUIPMENT_KEY IS NULL";
                        }
                        //判断抽检点记录是否存在
                        int rowcount = Convert.ToInt32(db.ExecuteScalar(dbTran,CommandType.Text, sqlCommand));
                        if (rowcount > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 0, equipmentText+"抽检点记录已经存在。");
                            return dsReturn;
                        }
                       
                        //新增记录。
                        edcpointKey = UtilHelper.GenerateNewKey(0);
                        hashData.Add(EDC_POINT_FIELDS.FIELD_ROW_KEY, edcpointKey);
                        hashData.Add(EDC_POINT_FIELDS.FIELD_EDIT_TIME, null);
                        EDC_POINT_FIELDS porLotFields = new EDC_POINT_FIELDS();
                        string sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, hashData, null);
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        
                        for (int i = 0;dsEDCPoint != null && i < dsEDCPoint.Tables[0].Rows.Count; i++)
                        {
                            Hashtable mainDataHashTable = new Hashtable();
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_EDC_POINT_ROWKEY, edcpointKey);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_TARGET, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_TARGET]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_ROW_KEY, UtilHelper.GenerateNewKey(0));
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL]);

                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY]);
                            mainDataHashTable.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX, dsEDCPoint.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX]);
      
                            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                            mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                            DataSet dsMainDataHashTable = new DataSet();
                            dsMainDataHashTable.Tables.Add(mainDataTable);
                            CreateEdcPointParam(db, dbTran, dsMainDataHashTable);
                        }

                        //设备主键数组为空,或者循环控制参数>=设备数组长度。
                        if (equipmentKeys == null || length >= equipmentKeys.Length)
                        {
                            break;
                        }
                    }
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 1, edcpointKey);
                }
                catch (Exception ex)
                {
                    LogService.LogError("CreateEdcPoint Error: " + ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, 2, ex.Message);
                }
                finally
                {
                    dbTran = null;
                    //Close Connection
                    dbConn.Close();
                }
            }
            return dsReturn;
        }
        /// <summary>
        /// 创建抽检点参数数据。
        /// </summary>
        /// <param name="db">数据库操作对象。</param>
        /// <param name="dbtran">事务操作。</param>
        /// <param name="dsParams">包含抽检点参数数据的数据集对象。</param>
        private void CreateEdcPointParam(Database db, DbTransaction dbtran, DataSet dsParams)
        {
            String sql = "";
            List<string> sqlCommandList = new List<string>();

            DataTable dtParams = dsParams.Tables[0];
            Hashtable htParams =FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);

            //initialize tablefields
            EDC_POINT_PARAMS_FIELDS porLotFields = new EDC_POINT_PARAMS_FIELDS();

            //get sql
            sql = DatabaseTable.BuildInsertSqlStatement(porLotFields, htParams, null);
            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
        }
        /// <summary>
        /// 根据抽检点设置的主键获取抽检点参数数据集合。
        /// </summary>
        /// <param name="pointRowKey">抽检点设置主键。</param>
        /// <returns>包含抽检点参数数据的数据集。</returns>
        public DataSet GetEdcPiontParams(string pointRowKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT * FROM EDC_POINT_PARAMS T WHERE T.EDC_POINT_ROWKEY='" + pointRowKey.PreventSQLInjection() + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEdcPiontParams Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 分页查询抽检点设置的修改历史。
        /// </summary>
        /// <param name="pointRowKey">抽检点设置主键。</param>
        /// <returns>包含抽检点设置的修改历史的数据集对象。</returns>
        public DataSet GetEdcPiontParamsTrans(string pointRowKey, ref PagingQueryConfig cofig)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //传进来的：第几页和每页大小
                int iPageNo = cofig.PageNo;
                int iPageSize = cofig.PageSize;
                //捞取所有的
                string sqlCommand =@"SELECT ROW_NUMBER() OVER(ORDER BY T2.CREATE_TIME) XUHAO, T2.* FROM 
                                    (
                                        SELECT   CONVERT(VARCHAR(30),B.CREATE_TIME, 120) CREATE_TIME, 
		                                         A.INSERT_TIMESTAMP,A.EQUIPMENT_NAME,B.OPERATION_NAME,B.ACTION_NAME,
		                                         B.PART_TYPE,B.EDIT_DESC,C.SP_NAME,F.EDC_NAME
                                        FROM 
                                        (
	                                         (SELECT   INSERT_TIMESTAMP, 
				                                        MIN (ROW_KEY) ROW_KEY,
				                                        dbo.ConcatEdcPointTransEquipmentName(INSERT_TIMESTAMP) EQUIPMENT_NAME
	                                           FROM EDC_POINT_TRANS D
	                                           WHERE GROUP_KEY = (SELECT TOP 1 T.GROUP_KEY
						                                          FROM EDC_POINT_TRANS T
						                                          WHERE T.ROW_KEY ='{0}')
	                                           GROUP BY INSERT_TIMESTAMP) A
	                                         LEFT JOIN EDC_POINT_TRANS B ON A.INSERT_TIMESTAMP = B.INSERT_TIMESTAMP AND A.ROW_KEY = B.ROW_KEY
	                                         LEFT JOIN EDC_SP C ON C.SP_KEY = B.SP_KEY
	                                         LEFT JOIN EDC_MAIN F ON F.EDC_KEY=B.EDC_KEY
                                        )
                                    ) T2 ";
                //总笔数和总页数
                int iRecords = 0;
                int iPages = 0;
                AllCommonFunctions.CommonPagingData(sqlCommand, iPageNo, iPageSize, out iPages, out iRecords, db, dsReturn, "parent", "CREATE_TIME");
                cofig.Records = iRecords;
                cofig.Pages = iPages;

                sqlCommand =string.Format(@"SELECT * 
                                            FROM EDC_POINT_PARAMS_TRANS T 
                                            LEFT JOIN (SELECT   ITEM_ORDER,
                                                                 MAX(CASE WHEN ATTRIBUTE_NAME='NAME' THEN ATTRIBUTE_VALUE END) NAME,
                                                                 MAX(CASE WHEN ATTRIBUTE_NAME='CODE' THEN ATTRIBUTE_VALUE END) CODE
                                                       FROM CRM_ATTRIBUTE
                                                       WHERE ATTRIBUTE_KEY IN (
								                                               SELECT ATTRIBUTE_KEY
								                                               FROM BASE_ATTRIBUTE
								                                               WHERE CATEGORY_KEY IN (SELECT CATEGORY_KEY
													                                                  FROM BASE_ATTRIBUTE_CATEGORY
                                                                                                      WHERE CATEGORY_NAME ='EDC_PARAM_TYPE')
                                                                               )
                                                        GROUP BY ITEM_ORDER) T2 ON T.PARAM_TYPE=T2.CODE 
                                            WHERE T.EDC_POINT_ROWKEY='{0}'",
                                            pointRowKey.PreventSQLInjection());
                DataTable dt2 = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];
                DataTable dt22 = dt2.Copy();
                dt22.TableName = "child";
                dsReturn.Tables.Add(dt22);
                dsReturn.Relations.Add("Relation", 
                                        dsReturn.Tables["parent"].Columns["INSERT_TIMESTAMP"], 
                                        dsReturn.Tables["child"].Columns["INSERT_TIMESTAMP"],
                                        false);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetEdcPiontParamsTrans Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除抽检点数据。
        /// </summary>
        /// <param name="groupKey">标识抽检点数据的组键。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet DeleteEDCPoint(string groupKey)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    string sqlCommand = string.Empty;
                    sqlCommand = string.Format(@"DELETE a FROM EDC_POINT_PARAMS a
                                                WHERE EXISTS(SELECT * FROM EDC_POINT b
                                                             WHERE b.GROUP_KEY='{0}'
                                                             AND b.ROW_KEY=a.EDC_POINT_ROWKEY)",groupKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    sqlCommand = string.Format(@"DELETE FROM EDC_POINT
                                                 WHERE GROUP_KEY='{0}'", groupKey.PreventSQLInjection());
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    LogService.LogError("DeleteEDCPoint Error: " + ex.Message);
                    //Rollback Transaction
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
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
        /// 更新抽检点参数数据。
        /// </summary>
        /// <param name="dsEdcPointParam">包含抽检点参数数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet UpdateEDCPointParams(DataSet dsEdcPointParam)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    const string INSERT_EDC_POINT= @"INSERT INTO EDC_POINT
                                                    (ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,
                                                    EDC_KEY,EDC_VERSION,SP_KEY,SP_VERSION,POINT_STATUS,PART_TYPE,GROUP_KEY,GROUP_NAME,EDITOR,EDIT_TIME,EDIT_DESC,ROUTE_VER_KEY,STEP_KEY,MUST_INPUT_FIELD)
                                                    SELECT TOP 1 '{1}',TOPRODUCT,OPERATION_NAME,'{2}',ACTION_NAME,
                                                    EDC_KEY,EDC_VERSION,SP_KEY,SP_VERSION,POINT_STATUS,PART_TYPE,GROUP_KEY,GROUP_NAME,EDITOR,EDIT_TIME,EDIT_DESC,ROUTE_VER_KEY,STEP_KEY,MUST_INPUT_FIELD
                                                    FROM EDC_POINT 
                                                    WHERE GROUP_KEY='{0}'";
                    const string INSERT_EDC_POINT_PARAMS = @"INSERT INTO EDC_POINT_PARAMS
                                                                (ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,UPPER_SPEC,UPPER_CONTROL,
                                                                TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,PARAM_COUNT,PARAM_KEY,PARAM_INDEX,PARAM_TYPE,PARAM_FORMULA,
                                                                ALLOW_MIN_VALUE,ALLOW_MAX_VALUE)
                                                            SELECT NEWID(),'{1}',EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,UPPER_SPEC,UPPER_CONTROL,
                                                                TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,PARAM_COUNT,PARAM_KEY,PARAM_INDEX,PARAM_TYPE,PARAM_FORMULA,
                                                                ALLOW_MIN_VALUE,ALLOW_MAX_VALUE
                                                                FROM EDC_POINT_PARAMS
                                                                WHERE EDC_POINT_ROWKEY=(SELECT MIN(ROW_KEY) FROM EDC_POINT WHERE GROUP_KEY='{0}')";
                    const string DELETE_EDC_POINT_PARAMS=@"DELETE FROM EDC_POINT_PARAMS
                                                           WHERE EDC_POINT_ROWKEY IN (SELECT ROW_KEY FROM EDC_POINT 
                                                                                      WHERE GROUP_KEY='{0}' {1})";
                    //AND EQUIPMENT_KEY IS NULL
                    const string DELETE_EDC_POINT = @"DELETE FROM EDC_POINT WHERE GROUP_KEY='{0}' {1}";
                    //Open Connection
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();
                    string sql = string.Empty;
                    int count = 0;
                    string groupKey = dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_GROUP_KEY].ToString();
                    string groupName = Convert.ToString(dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_GROUP_NAME]);
                    string newEquipmentKey = dsEdcPointParam.ExtendedProperties["NEW_EQUIPMENT_KEY"].ToString();
                    string editor=Convert.ToString(dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_EDITOR]);
                    string desc = Convert.ToString(dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_EDIT_DESC]);
                    string oldEquipmentKey = dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY].ToString();
                    int field = Convert.ToInt32(dsEdcPointParam.ExtendedProperties[EDC_POINT_FIELDS.FIELD_MUST_INPUT_FIELD]);
  
                    if (newEquipmentKey != oldEquipmentKey) //设备有变动。
                    {
                        string[] newEquipmentKeys = newEquipmentKey.Split(',');
                        string[] oldEquipmentKeys = oldEquipmentKey.Split(',');
                        string[] deleteEquipmentKeys= oldEquipmentKeys.Except<string>(newEquipmentKeys).ToArray();
                        string[] addEquipmentKeys = newEquipmentKeys.Except<string>(oldEquipmentKeys).ToArray();
                        //根据设备添加抽检点数据。
                        for (int i = 0; i < addEquipmentKeys.Length; i++)
                        {
                            string edcPointRowKey=UtilHelper.GenerateNewKey(0);
                            string equipmentKey=addEquipmentKeys[i];
                            sql = string.Format(INSERT_EDC_POINT_PARAMS, groupKey.PreventSQLInjection(), edcPointRowKey.PreventSQLInjection());
                            count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                            sql = string.Format(INSERT_EDC_POINT, groupKey.PreventSQLInjection(), edcPointRowKey.PreventSQLInjection(), equipmentKey.PreventSQLInjection());
                            count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                        //根据设备删除抽检点数据。
                        for (int i = 0; i < deleteEquipmentKeys.Length; i++)
                        {
                            string equipmentKey = deleteEquipmentKeys[i];
                            string sqlCondition = string.Format("AND EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                            //如果设备为空。
                            if(string.IsNullOrEmpty(equipmentKey)){
                                sqlCondition="AND EQUIPMENT_KEY IS NULL";
                            }                           

                            sql = string.Format(DELETE_EDC_POINT_PARAMS, groupKey, sqlCondition);
                            count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);                 

                            sql = string.Format(DELETE_EDC_POINT, groupKey, sqlCondition);
                            count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        }
                    }
                    //添加修改历史
                    string sqlCommand = @"SELECT SYSDATETIME() SHIJIANCHUO";
                    DataSet  ds = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                    string StrTimeStamp = Convert.ToString(ds.Tables[0].Rows[0][0]);

                    const string INSERT_HTEDC_POINT_SQL = @"INSERT INTO EDC_POINT_TRANS
                                                   (CREATE_TIME,ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,
                                                    EDC_KEY,EDC_VERSION,SP_KEY,SP_VERSION,POINT_STATUS,PART_TYPE,GROUP_KEY,EDITOR,EDIT_TIME,EDIT_DESC,INSERT_TIMESTAMP,MUST_INPUT_FIELD,ROUTE_VER_KEY,STEP_KEY)
                                                    SELECT GETDATE(),ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,
                                                    EDC_KEY,EDC_VERSION,SP_KEY,SP_VERSION,POINT_STATUS,PART_TYPE,GROUP_KEY,EDITOR,EDIT_TIME,EDIT_DESC,'{1}',MUST_INPUT_FIELD,ROUTE_VER_KEY,STEP_KEY
                                                    FROM EDC_POINT 
                                                    WHERE GROUP_KEY='{0}' ";
                    sql = string.Format(INSERT_HTEDC_POINT_SQL, 
                                        groupKey.PreventSQLInjection(), 
                                        StrTimeStamp.PreventSQLInjection());
                    count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //更新抽检点数据
                    const string UPDATE_EDC_POINT_SQL = @"UPDATE EDC_POINT 
                                                          SET EDITOR='{1}',EDIT_TIME=GETDATE(),EDIT_DESC='{2}',MUST_INPUT_FIELD={3},GROUP_NAME='{4}' 
                                                          WHERE GROUP_KEY='{0}'";
                    sql = string.Format(UPDATE_EDC_POINT_SQL, 
                                        groupKey.PreventSQLInjection(), 
                                        editor.PreventSQLInjection(), 
                                        desc.PreventSQLInjection(),
                                        field, 
                                        groupName.PreventSQLInjection());
                    count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    const string UPDATE_EDC_POINT_PARAMS_SQL = @"UPDATE a
                                                                SET a.UPPER_BOUNDARY='{2}',a.UPPER_SPEC='{3}',a.UPPER_CONTROL='{4}',a.TARGET='{5}',
                                                                    a.LOWER_CONTROL='{6}',a.LOWER_SPEC='{7}',a.LOWER_BOUNDARY='{8}',a.PARAM_COUNT='{9}',
                                                                    a.PARAM_INDEX='{10}',a.PARAM_TYPE='{11}',a.PARAM_FORMULA='{12}',
                                                                    a.ALLOW_MIN_VALUE='{13}',a.ALLOW_MAX_VALUE='{14}'
                                                                FROM EDC_POINT_PARAMS a
                                                                WHERE EXISTS(SELECT * FROM EDC_POINT b
                                                                             WHERE b.GROUP_KEY='{0}'
                                                                             AND b.ROW_KEY=a.EDC_POINT_ROWKEY)
                                                                AND a.PARAM_KEY='{1}'";                    
                    
                    for (int i = 0; i < dsEdcPointParam.Tables[0].Rows.Count; i++)
                    {
                        string rowKey = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_EDC_POINT_ROWKEY]);
                        string lowerBoundary = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]);
                        string lowControl = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL]);
                        string lowerSpec = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC]);
                        string paramCount = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT]);
                        string paramName = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME]);
                        string upperSpec = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC]);
                        string target = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_TARGET]);
                        string upperBoundary = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]);
                        string upperControl = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL]);
                        string paramType = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE]);
                        string paramKey = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY]);
                        string paramIndex = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX]);
                        string formula = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_FORMULA]);
                        string allowMinValue = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MIN_VALUE]);
                        string allowMaxValue = Convert.ToString(dsEdcPointParam.Tables[0].Rows[i][EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MAX_VALUE]);
                        sql = string.Format(UPDATE_EDC_POINT_PARAMS_SQL,
                                            groupKey.PreventSQLInjection(),
                                            paramKey.PreventSQLInjection(),
                                            upperBoundary.PreventSQLInjection(),
                                            upperSpec.PreventSQLInjection(),
                                            upperControl.PreventSQLInjection(),
                                            target.PreventSQLInjection(),
                                            lowControl.PreventSQLInjection(),
                                            lowerSpec.PreventSQLInjection(),
                                            lowerBoundary.PreventSQLInjection(),
                                            paramCount.PreventSQLInjection(),
                                            paramIndex.PreventSQLInjection(),
                                            paramType.PreventSQLInjection(),
                                            formula.PreventSQLInjection(),
                                            allowMinValue.PreventSQLInjection(),
                                            allowMaxValue.PreventSQLInjection());
                        count=db.ExecuteNonQuery(dbTran,CommandType.Text, sql);
                    }
                    //插入抽检点的历史数据
                    const string INSERT_HTEDC_POINT_PARAMS = @"INSERT INTO EDC_POINT_PARAMS_TRANS
                                                                   (CREATE_TIME,ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,UPPER_SPEC,UPPER_CONTROL,
                                                                    TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,PARAM_COUNT,PARAM_KEY,PARAM_INDEX,PARAM_TYPE,PARAM_FORMULA,
                                                                    ALLOW_MIN_VALUE,ALLOW_MAX_VALUE,INSERT_TIMESTAMP)
                                                               SELECT GETDATE(),ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,UPPER_SPEC,UPPER_CONTROL,
                                                                    TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,PARAM_COUNT,PARAM_KEY,PARAM_INDEX,PARAM_TYPE,PARAM_FORMULA,
                                                                    ALLOW_MIN_VALUE,ALLOW_MAX_VALUE,'{1}'
                                                               FROM EDC_POINT_PARAMS
                                                               WHERE EDC_POINT_ROWKEY in (SELECT ROW_KEY FROM EDC_POINT WHERE GROUP_KEY='{0}')";
                    sql = string.Format(INSERT_HTEDC_POINT_PARAMS,
                                        groupKey.PreventSQLInjection(), 
                                        StrTimeStamp.PreventSQLInjection());
                    count = db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                    dbTran.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    LogService.LogError("UpdateEDCPointParams Error: " + ex.Message);
                    dbTran.Rollback();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
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
        /// 更新抽检点数据的状态。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <param name="pointStatus">新的抽检点设置状态。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet UpdateEDCPointStatus(string groupKey, string pointState)
        {
            DataSet dsReturn = new DataSet();
            DbTransaction dbTran = null;
            using (DbConnection dbConn = db.CreateConnection())
            {
                try
                {
                    dbConn.Open();
                    dbTran = dbConn.BeginTransaction();

                    if (!string.IsNullOrEmpty(groupKey) && !string.IsNullOrEmpty(pointState))
                    {
                        string sql = string.Format(@"UPDATE EDC_POINT SET POINT_STATUS = '{0}' WHERE GROUP_KEY = '{1}'",
                                                    pointState.PreventSQLInjection(), 
                                                    groupKey.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        dbTran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    LogService.LogError("UpdateEDCPointStatus Error: " + ex.Message);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                }
                finally
                {
                    dbConn.Close();
                }
                return dsReturn;
            }
        }
        /// <summary>
        /// 查找正在使用的相同类型的抽检点数据是否存在。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <returns>true：存在。false：不存在。</returns>
        public bool FindExistUsedEDCPoint(string groupKey)
        {
            try
            {
                string equipKey = string.Empty;
                string sql = string.Format(@"SELECT P.* FROM EDC_POINT P WHERE P.GROUP_KEY= '{0}'",groupKey.PreventSQLInjection());
                DataSet ds = db.ExecuteDataSet(CommandType.Text, sql);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string rowKey=Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_ROW_KEY]);
                    string toProduct = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_TOPRODUCT]);
                    string partType = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_PART_TYPE]);
                    string operationName = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_OPERATION_NAME]);
                    string actionName = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_ACTION_NAME]);
                    string equipmentKey = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY]);
                    string edcKey = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_EDC_KEY]);
                    string routeKey = Convert.ToString(ds.Tables[0].Rows[i][EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY]);
                    sql = string.Format(@"SELECT COUNT(P.ROW_KEY)
                                          FROM EDC_POINT P
                                          WHERE P.POINT_STATUS = '1' 
                                          AND P.ROW_KEY <> '{0}'
                                          AND P.GROUP_KEY<>'{1}'", rowKey.PreventSQLInjection(), groupKey.PreventSQLInjection());
                    //工艺流程。
                    if (!string.IsNullOrEmpty(routeKey))
                    {
                        sql += string.Format(" AND ROUTE_VER_KEY='{0}'", routeKey.PreventSQLInjection());
                    }
                    else
                    {
                        sql += " AND ROUTE_VER_KEY IS NULL";
                    }
                    //产品号。
                    if (!string.IsNullOrEmpty(toProduct))
                    {
                        sql += string.Format(" AND P.TOPRODUCT='{0}'", toProduct.PreventSQLInjection());
                    }
                    else
                    {
                        sql += " AND P.TOPRODUCT IS NULL";
                    }
                    //成品类型
                    if (!string.IsNullOrEmpty(partType))
                    {
                        sql += string.Format(" AND P.PART_TYPE='{0}'", partType.PreventSQLInjection());
                    }
                    else
                    {
                        sql += " AND P.PART_TYPE IS NULL";
                    }
                    //设备主键。
                    if (!string.IsNullOrEmpty(equipmentKey))
                    {
                        sql += string.Format(" AND P.EQUIPMENT_KEY='{0}'", equipmentKey.PreventSQLInjection());
                    }
                    else
                    {
                        sql += " AND P.EQUIPMENT_KEY IS NULL";
                    }
                    //如果是离线操作，需要增加数据采集主键的判断。
                    if (actionName == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE)
                    {
                        sql += string.Format(" AND P.EDC_KEY='{0}'", edcKey.PreventSQLInjection());
                    }
                    sql += string.Format(" AND P.ACTION_NAME='{0}'", actionName.PreventSQLInjection());
                    sql += string.Format(" AND P.OPERATION_NAME='{0}'", operationName.PreventSQLInjection());

                    int count=Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql));
                    if (count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("FindExistedUsedEDCPoint Error: " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="productNo">产品号。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="equipmentKey">设备主键。如果为空，则查询设备主键为NULL的数据。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME,SP_NAME,SP_DESCRIPTIONS】
        /// </returns>
        public DataSet GetEDCPoint(string productNo, string operationName, string equipmentKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = @"SELECT DISTINCT a.ROW_KEY,a.TOPRODUCT,a.OPERATION_NAME,a.EQUIPMENT_KEY,a.ACTION_NAME,
                            a.SP_KEY,a.EDC_KEY,b.EDC_NAME,b.STATUS,a.PART_TYPE,c.EQUIPMENT_NAME,d.SP_NAME,d.DESCRIPTIONS SP_DESCRIPTIONS,
                            a.MUST_INPUT_FIELD,a.ROUTE_VER_KEY,r.ROUTE_NAME
                        FROM EDC_POINT a
                        LEFT JOIN EDC_MAIN b ON a.EDC_KEY=b.EDC_KEY
                        LEFT JOIN EMS_EQUIPMENTS c ON a.EQUIPMENT_KEY=c.EQUIPMENT_KEY
                        LEFT JOIN EDC_SP d ON a.SP_KEY=d.SP_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER r ON a.ROUTE_VER_KEY=r.ROUTE_ROUTE_VER_KEY
                        WHERE a.POINT_STATUS=1";
            if (!string.IsNullOrEmpty(productNo))
            {
                sql += " AND TOPRODUCT='" + productNo.PreventSQLInjection() + "'";
            }
            if (!string.IsNullOrEmpty(operationName))
            {
                sql += " AND OPERATION_NAME='" + operationName.PreventSQLInjection() + "'";
            }
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                sql += " AND EQUIPMENT_KEY='" + equipmentKey.PreventSQLInjection() + "'";
            }
            else
            {
                sql += " AND EQUIPMENT_KEY IS NULL";
            }
            sql += " ORDER BY r.ROUTE_NAME DESC,b.EDC_NAME,a.ACTION_NAME";
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetEDCPoint Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        public DataSet GetEDCPoint(string factoryRoomKey, string operationName)
        {
            return this.GetEDCPoint(factoryRoomKey, operationName, string.Empty, string.Empty);
        }
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="factoryRoomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="partType">成品类型。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME，SP_NAME，SP_DESCRIPTIONS】
        /// </returns>
        public DataSet GetEDCPoint(string factoryRoomKey, string operationName, string partType, string equipmentKey)
        {
            DataSet dsReturn = new DataSet();
            string sql = @"SELECT DISTINCT a.ROW_KEY,a.TOPRODUCT,a.OPERATION_NAME,a.EQUIPMENT_KEY,a.ACTION_NAME,
                        a.SP_KEY,a.EDC_KEY,b.EDC_NAME,b.STATUS,a.PART_TYPE,c.EQUIPMENT_NAME,d.SP_NAME,d.DESCRIPTIONS SP_DESCRIPTIONS,
                        a.MUST_INPUT_FIELD,a.ROUTE_VER_KEY,r.ROUTE_NAME
                        FROM EDC_POINT a
                        LEFT JOIN EDC_MAIN b ON a.EDC_KEY=b.EDC_KEY
                        LEFT JOIN EMS_EQUIPMENTS c ON a.EQUIPMENT_KEY=c.EQUIPMENT_KEY
                        LEFT JOIN EDC_SP d ON a.SP_KEY=d.SP_KEY
                        LEFT JOIN POR_ROUTE_ROUTE_VER r ON a.ROUTE_VER_KEY=r.ROUTE_ROUTE_VER_KEY
                        WHERE a.POINT_STATUS=1";

            if (!string.IsNullOrEmpty(factoryRoomKey))
            {
                sql += string.Format(@" AND (a.EQUIPMENT_KEY IS NULL 
                                             OR EXISTS (SELECT e.EQUIPMENT_NAME,e.EQUIPMENT_CODE,l.ROOM_NAME,l.ROOM_KEY 
                                                        FROM EMS_EQUIPMENTS e 
                                                        LEFT JOIN v_location l ON e.LOCATION_KEY=l.AREA_KEY 
                                                        WHERE l.ROOM_KEY='{0}' 
                                                        AND e.EQUIPMENT_KEY=a.EQUIPMENT_KEY))", factoryRoomKey.PreventSQLInjection());
            }
            if (!string.IsNullOrEmpty(operationName))
            {
                sql += " AND a.OPERATION_NAME='" + operationName.PreventSQLInjection() + "'";
            }
            if (!string.IsNullOrEmpty(partType))
            {
                sql += string.Format(" AND a.PART_TYPE='{0}'",partType.PreventSQLInjection());
            }
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                sql += string.Format(" AND (a.EQUIPMENT_KEY='{0}' OR a.EQUIPMENT_KEY IS NULL)", equipmentKey.PreventSQLInjection());
            }
            sql += " ORDER BY r.ROUTE_NAME DESC,b.EDC_NAME,a.ACTION_NAME";
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("GetEDCPoint Error: " + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
    }
}

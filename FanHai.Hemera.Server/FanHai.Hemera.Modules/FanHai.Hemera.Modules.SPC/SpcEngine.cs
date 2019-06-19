using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace FanHai.Hemera.Modules.SPC
{
    public class SpcEngine : AbstractEngine, ISpcEngine
    {
        private Database db;
        private Database dbSPC;
        private const string SPC_DATABASE = "SpcDatabase";
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SpcEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            dbSPC = DatabaseFactory.CreateDatabase(SPC_DATABASE);
        }

        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }

        /// <summary>
        /// 保存抽检界面数据保存时，spc相应操作。
        /// </summary>
        /// <param name="dataset">包含SPC数据的数据集对象。</param>
        /// <returns>true：保存成功。false:保存失败。</returns>
        public DataSet SaveParamData(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                {
                    string orderNumber = dataset.ExtendedProperties[SPC_PARAM_DATA_FIELDS.ORDER_NUMBER].ToString();//工单
                    string edcInskey = dataset.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDC_INS_KEY].ToString();
                    string shiftValue = dataset.ExtendedProperties[SPC_PARAM_DATA_FIELDS.SHIFT_VALUE].ToString();
                    string creator = dataset.ExtendedProperties[SPC_PARAM_DATA_FIELDS.CREATOR].ToString();//操作者
                    DataTable paramValueTable = dataset.Tables[EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME];
                    if (paramValueTable.Rows.Count > 0)
                    {
                        #region 将抽检数据保存置spc表中
                        DbConnection dbconn = null;
                        DbTransaction dbtran = null;
                        try
                        {
                            //取数据采集实例数据。
                            string edcMainInsSql = string.Format(@"SELECT a.EQUIPMENT_KEY,a.MATERIAL_LOT,a.STEP_NAME,a.SUPPLIER,a.PART_NO,
                                                                       a.LOCATION_KEY,a.PART_TYPE,a.LOT_NUMBER,b.ROUTE_VER_KEY,a.CREATE_TIME
                                                                FROM EDC_MAIN_INS a
                                                                LEFT JOIN EDC_POINT b ON a.EDC_POINT_KEY=b.ROW_KEY
                                                                WHERE a.EDC_INS_KEY='{0}'", edcInskey);
                            IDataReader reader = db.ExecuteReader(CommandType.Text, edcMainInsSql);
                            string equipmentKey = string.Empty;
                            string materialLot = string.Empty;
                            string stepName = string.Empty;
                            string supplier = string.Empty;
                            string partNo = string.Empty;
                            string partType = string.Empty;
                            string locationKey = string.Empty;
                            string lotNo = string.Empty;
                            string routeKey = string.Empty;
                            DateTime createTime = DateTime.Now;
                            if (reader.Read())
                            {
                                equipmentKey = Convert.ToString(reader["EQUIPMENT_KEY"]);
                                materialLot = Convert.ToString(reader["MATERIAL_LOT"]);
                                supplier = Convert.ToString(reader["SUPPLIER"]);
                                stepName = Convert.ToString(reader["STEP_NAME"]);
                                partNo = Convert.ToString(reader["PART_NO"]);
                                locationKey = Convert.ToString(reader["LOCATION_KEY"]);
                                partType = Convert.ToString(reader["PART_TYPE"]);
                                lotNo = Convert.ToString(reader["LOT_NUMBER"]);
                                routeKey = Convert.ToString(reader["ROUTE_VER_KEY"]);
                                createTime = Convert.ToDateTime(reader["CREATE_TIME"]);
                            }

                            dbconn = db.CreateConnection();
                            //Open Connection
                            dbconn.Open();
                            //Create Transaction  
                            dbtran = dbconn.BeginTransaction();
                            foreach (DataRow row in paramValueTable.Rows)
                            {
                                string paramKey = row[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString();
                                string sql = string.Format(@"INSERT INTO SPC_PARAM_DATA
                                                                (PARAM_KEY,EDC_INS_KEY,PARAM_VALUE,VALID_FLAG,DELETED_FLAG,CREATE_TIME,COL_KEY,EDIT_FLAG,ORDER_NUMBER,
                                                                 EQUIPMENT_KEY,SHIFT_VALUE,CREATOR,MATERIAL_LOT,STEP_NAME,SUPPLIER,PART_NO,LOCATION_KEY,PART_TYPE,LOT_NUMBER,ROUTE_KEY) 
                                                             VALUES
                                                                 ('{0}','{1}','{2}','{3}','{4}',{5},'{6}',{7},'{8}',
                                                                  '{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                                             paramKey,
                                                             row[EDC_COLLECTION_DATA_FIELDS.FIELD_EDC_INS_KEY],
                                                             row[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE],
                                                             row[EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG],
                                                             0,
                                                             "to_date('" + createTime.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd HH24:mi:ss')",
                                                             UtilHelper.GenerateNewKey(0),
                                                             0,
                                                             orderNumber,
                                                             equipmentKey,
                                                             shiftValue,
                                                             creator,
                                                             materialLot,
                                                             stepName,
                                                             supplier,
                                                             partNo,
                                                             locationKey,
                                                             partType,
                                                             lotNo,
                                                             routeKey
                                                            );
                                db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            }
                            dbtran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            dbtran.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            dbconn.Close();
                        }
                        #endregion
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "input data is null");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC Save value of Parameter error:" + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 数据采集界面方块电阻数据清空时，spc相应删除操作
        /// </summary>
        /// <returns></returns>
        public DataSet DeleteRParamData(string edcInskey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (edcInskey.Length > 0)
                {
                    string sql = string.Format(@"update spc_param_data a
                                              set a.deleted_flag = 1
                                                 where a.param_key in
                                                       (select  distinct t.param_key
                                                          from edc_collection_data t, base_parameter b
                                                         where t.param_key = b.param_key
                                                           and t.edc_ins_key = '{0}'
                                                           and b.device_type = 'R' and t.deleted_flag=0)
                                                   and a.edc_ins_key='{0}'
                                                   and a.deleted_flag=0", edcInskey);
                    db.ExecuteNonQuery(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "input paramter is empty");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("spc DeleteRParamData error" + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 撤销操作（UndoOUTEDC）时，Spc相应操作
        /// </summary>
        /// <param name="transactionKey"></param>
        /// <returns></returns>
        public DataSet DeleteParamData(string transactionKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (transactionKey.Length > 0)
                {
                    string sql = string.Format(@"UPDATE SPC_PARAM_DATA T  
                                      SET T.DELETED_FLAG = 1  
                                      WHERE T.EDC_INS_KEY = (SELECT W.EDC_INS_KEY
                                                              FROM WIP_TRANSACTION W
                                                              WHERE W.TRANSACTION_KEY = '{0}')", transactionKey);
                    db.ExecuteNonQuery(CommandType.Text, sql);
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "input parameter is empty");
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("spc DeleteParamData error:" + ex.Message);
            }
            return dsReturn;
        }

        #region Spc Other Function
        /// <summary>
        /// SPC参数，SPC空值图数据源
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Modify by genchille.yang 2012-05-2 15:27:19 
        public DataSet SearchParamValue(DataTable dataTable)
        {
            //定义返回的数据源
            DataSet dsReturn = new DataSet();
            try
            {
                string orderNumber = string.Empty;

                string pro_type = string.Empty;

                //dataTable 数据查询条件，如果为空则不能找到查询的数据，不能为null。
                if (dataTable != null)
                {
                    #region 查询参数值
                    //把dataTable还原为HashTable 便于查询条件使用
                    Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string scontrol = string.Format(@"SELECT t.*,t1.route_operation_name FROM SPC_CONTROL_PLAN t, por_route_operation_ver t1
                                                     WHERE t.step_key=t1.route_operation_ver_key
                                                     AND t.controlplanid='{0}'", hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID]);
                    DataTable dtControlPlan = db.ExecuteDataSet(CommandType.Text, scontrol).Tables[0];


                    dtControlPlan.TableName = "SPC_CONTROL_PLAN";//Q.003
                    dsReturn.Tables.Add(dtControlPlan.Copy());//Q.003

                    DataRow drControl = dtControlPlan.Rows[0];

                    string aSql = string.Empty;
                    string mSql = string.Empty;
                    string parakey = string.Empty, tSql = string.Empty, s_material = string.Empty, s_suppler = string.Empty;


                    //物料条件
                    const string materialSql = @" SELECT DISTINCT T1.MATERIAL_LOT FROM ({0}) T1  WHERE isnull(T1.MATERIAL_LOT, '0') <> '0'";
                    //供应商条件
                    const string supplerSql = @" SELECT DISTINCT T1.SUPPLIER FROM ({0}) T1 WHERE isnull(T1.SUPPLIER, '0') <> '0'";
                    //单点极差
                    const string mrSql = @"SELECT * FROM ( SELECT ROUND(T.PARAM_VALUE, 4) PARAM_VALUE,
                                           T.COL_KEY,
                                           T.EDIT_FLAG,
                                           T.EDIT_REASON,
                                           T.EDC_INS_KEY,
                                           TO_CHAR(T.CREATE_TIME, 'MM/dd hh24:mi:ss') CREATE_TIME,
                                           --T.CREATE_TIME CREATE_TIME,
                                           T.SUPPLIER,
                                           T.LOT_NUMBER,
                                           T.EQUIPMENT_KEY,
                                           T.MATERIAL_LOT,
                                           T.SHIFT_VALUE,
                                           T.ROUTE_KEY ROUTE_ROUTE_VER_KEY
                                      FROM SPC_PARAM_DATA T
                                     WHERE T.DELETED_FLAG = 0
                                       AND T.PARAM_KEY = '{0}'";
                    //其他分组
                    const string eSql = @" SELECT T0.EDC_INS_KEY , 
                                           T0.CREATE_TIME,
                                           T0.PARAM_KEY, 
                                           T0.SUPPLIER,
                                         T0.MATERIAL_LOT,
                                         T0.LOT_NUMBER ,T0.LOCATION_KEY,T0.EQUIPMENT_KEY,T0.SHIFT_VALUE,T0.ROUTE_ROUTE_VER_KEY FROM (
                                        SELECT T.EDC_INS_KEY,
                                         TO_CHAR(T.CREATE_TIME,'MM/dd hh24:mi:ss') CREATE_TIME,
                                         T.PARAM_KEY,
                                         T.SUPPLIER,
                                         T.MATERIAL_LOT,
                                         T.LOT_NUMBER,
                                         T.LOCATION_KEY,
                                         T.EQUIPMENT_KEY,
                                         T.SHIFT_VALUE,
                                         T.ROUTE_KEY ROUTE_ROUTE_VER_KEY
                                        FROM SPC_PARAM_DATA T
                                        WHERE T.DELETED_FLAG = 0
                                        AND T.PARAM_KEY = '{0}'";
                    //原始数据
                    const string eSql01 = @" SELECT ROUND(A.PARAM_VALUE, 4) PARAM_VALUE,
                                           A.COL_KEY,
                                           A.EDIT_FLAG,
                                           A.EDIT_REASON,
                                           A.EDC_INS_KEY,
                                           TO_CHAR(A.CREATE_TIME,'MM/dd hh24:mi:ss') CREATE_TIME                                         
                                      FROM SPC_PARAM_DATA A 
                                       WHERE A.DELETED_FLAG = 0 AND EXISTS (SELECT B.EDC_INS_KEY FROM ({0}) B WHERE B.EDC_INS_KEY=A.EDC_INS_KEY AND B.PARAM_KEY=A.PARAM_KEY AND TO_CHAR(A.CREATE_TIME,'MM/dd hh24:mi:ss')=B.CREATE_TIME)   
                                ORDER BY A.CREATE_TIME ASC ";

                    #region 查询条件
                    string controlType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
                    parakey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
                    //工厂车间
                    tSql += string.Format(" AND T.LOCATION_KEY='{0}'", drControl[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString());
                    //工序名称
                    tSql += string.Format(" AND  T.STEP_NAME = '{0}'", drControl["route_operation_name"].ToString());
                    //产品类型
                    tSql += string.Format(" AND T.PART_TYPE='{0}'", drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString());
                    //获得返回的产品类型
                    pro_type = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
                    //物料批次/供应商批次
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.MATERIAL_LOT))
                        tSql += string.Format("   AND T.MATERIAL_LOT='{0}'", hashTable[SPC_PARAM_DATA_FIELDS.MATERIAL_LOT]);
                    //供应商
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SUPPLIER))
                        tSql += string.Format("   AND T.SUPPLIER LIKE '{0}'", hashTable[SPC_PARAM_DATA_FIELDS.SUPPLIER]);
                    //班别
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SHIFT_VALUE))
                        tSql += string.Format("   AND T.SHIFT_VALUE='{0}'", hashTable[SPC_PARAM_DATA_FIELDS.SHIFT_VALUE]);
                    //设备
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY))
                        tSql += string.Format(" AND T.EQUIPMENT_KEY IN ({0})", hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY]);
                    #endregion
                    #region 单点移动极差
                    if (controlType.ToUpper() == "XBAR-MR")
                        tSql = string.Format(mrSql + tSql, parakey);//参数                  
                    #endregion
                    #region s-r-xbar
                    else
                        tSql = string.Format(eSql + tSql, parakey);//参数 
                    #endregion
                    #region mr
                    if (controlType.ToUpper() == "XBAR-MR")
                    {
                        if (hashTable.ContainsKey("MODE_CODE"))
                        {
                            int code = Convert.ToInt32(hashTable["MODE_CODE"]);
                            //string value = hashTable["MODE_VALUE"].ToString();
                            //按时间查询
                            if (code == 1)
                            {
                                string _startDate = hashTable["MODE_VALUE_STARTDATE"].ToString();
                                string _endDate = hashTable["MODE_VALUE_ENDDATE"].ToString();
                                tSql += string.Format(@"  AND T.CREATE_TIME BETWEEN TO_DATE('{0}','yyyy-MM-DD hh24:mi:ss') 
                                                        AND TO_DATE('{1}','yyyy-MM-DD hh24:mi:ss')) T0 ORDER BY T0.CREATE_TIME ASC", _startDate, _endDate);

                            }
                            //按点查询
                            if (code == 0)
                            {
                                int points = Convert.ToInt32(hashTable["MODE_VALUE_POINTS"]);

                                tSql += string.Format(" ORDER BY T.CREATE_TIME DESC ) T0 WHERE ROWNUM<={0} ORDER BY T0.CREATE_TIME ASC ", points);
                            }

                        }
                        s_material = string.Format(materialSql, tSql);
                        s_suppler = string.Format(supplerSql, tSql);

                        DataTable dtSPC = db.ExecuteDataSet(CommandType.Text, tSql).Tables[0];
                        dtSPC.TableName = "DATA_SPC";
                        dsReturn.Merge(dtSPC, false, MissingSchemaAction.Add);

                        DataTable dtMaterial = db.ExecuteDataSet(CommandType.Text, s_material).Tables[0];
                        dtMaterial.TableName = "Material";
                        DataTable dtSupplier = db.ExecuteDataSet(CommandType.Text, s_suppler).Tables[0];
                        dtSupplier.TableName = "Supplier";

                        dsReturn.Merge(dtMaterial, false, MissingSchemaAction.Add);
                        dsReturn.Merge(dtSupplier, false, MissingSchemaAction.Add);
                    }
                    #endregion
                    #region s-r-xbar
                    else
                    {
                        tSql += @" GROUP BY T.EDC_INS_KEY,
                                   T.CREATE_TIME,
                                   T.PARAM_KEY,
                                   T.SUPPLIER,
                                   T.MATERIAL_LOT,
                                   T.LOT_NUMBER,
                                   T.LOCATION_KEY,
                                   T.EQUIPMENT_KEY,
                                   T.SHIFT_VALUE,
                                   T.ROUTE_KEY ORDER BY T.CREATE_TIME DESC) T0 ";
                        if (hashTable.ContainsKey("MODE_CODE"))
                        {
                            int code = Convert.ToInt32(hashTable["MODE_CODE"]);
                            if (code == 1)//按时间查询
                            {
                                string _startDate = hashTable["MODE_VALUE_STARTDATE"].ToString();
                                string _endDate = hashTable["MODE_VALUE_ENDDATE"].ToString();

                                tSql = tSql + string.Format(@" WHERE T0.CREATE_TIME BETWEEN TO_CHAR(TO_DATE('{0}', 'yyyy-MM-DD hh24:mi:ss'),
                                                               'MM/dd hh24:mi:ss') AND
                                                                TO_CHAR(TO_DATE('{1}', 'yyyy-MM-DD hh24:mi:ss'),
                                                               'MM/dd hh24:mi:ss') ORDER BY T0.CREATE_TIME ASC ", _startDate, _endDate);
                            }

                            if (code == 0)//按点查询
                            {
                                int points = Convert.ToInt32(hashTable["MODE_VALUE_POINTS"]);
                                tSql += string.Format(" WHERE ROWNUM<={0} ORDER BY T0.CREATE_TIME ASC ", points);
                            }

                        }
                        s_material = string.Format(materialSql, tSql);
                        s_suppler = string.Format(supplerSql, tSql);

                        DataTable dtGroup = db.ExecuteDataSet(CommandType.Text, tSql).Tables[0];
                        dtGroup.TableName = "DATA_GROUP";

                        aSql = string.Format(eSql01, tSql);

                        DataTable dtSPC = db.ExecuteDataSet(CommandType.Text, aSql).Tables[0];
                        dtSPC.TableName = "DATA_SPC";

                        dsReturn.Merge(dtSPC, false, MissingSchemaAction.Add);
                        dsReturn.Merge(dtGroup, false, MissingSchemaAction.Add);

                        DataTable dtMaterial = new DataTable("Material");
                        dtMaterial.Columns.Add("MATERIAL_LOT");
                        DataTable dtSupplier = new DataTable("Supplier");
                        dtSupplier.Columns.Add("SUPPLIER");
                        foreach (DataRow dr in dtGroup.Rows)
                        {
                            DataRow[] rms = dtMaterial.Select(string.Format("MATERIAL_LOT='{0}'", dr["MATERIAL_LOT"].ToString()));
                            DataRow[] rss = dtSupplier.Select(string.Format("SUPPLIER='{0}'", dr["SUPPLIER"].ToString()));
                            if (rms.Length < 1 && !dr["MATERIAL_LOT"].ToString().Trim().Equals(string.Empty))
                            {
                                DataRow drm = dtMaterial.NewRow();
                                drm["MATERIAL_LOT"] = dr["MATERIAL_LOT"];
                                dtMaterial.Rows.Add(drm);
                            }
                            if (rss.Length < 1 && !dr["SUPPLIER"].ToString().Trim().Equals(string.Empty))
                            {
                                DataRow drs = dtSupplier.NewRow();
                                drs["SUPPLIER"] = dr["SUPPLIER"];
                                dtSupplier.Rows.Add(drs);
                            }
                        }
                        dsReturn.Merge(dtMaterial, false, MissingSchemaAction.Add);
                        dsReturn.Merge(dtSupplier, false, MissingSchemaAction.Add);
                    }

                    #endregion

                    #endregion

                    #region 查询参数规格上下线
                    if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                    {

                        #region
                        //                        //根据参数ID，产品型号，工序类型——确定参数规格的上下线
                        string sql = string.Format(@" SELECT MAX(A.UPPER_SPEC) UPPER_SPEC,
                                                       MAX(A.LOWER_SPEC) LOWER_SPEC,
                                                       MAX(A.TARGET) TARGET,
                                                       MAX(A.UPPER_BOUNDARY) UPPER_BOUNDARY,
                                                       MAX(A.LOWER_BOUNDARY) LOWER_BOUNDARY,
                                                       '' EQUIPMENT_KEY,
                                                       A.PARAM_NAME,
                                                       T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                 GROUP BY A.PARAM_NAME, T.ROUTE_VER_KEY
                                                UNION ALL
                                                SELECT A.UPPER_SPEC,
                                                       A.LOWER_SPEC,
                                                       A.TARGET,
                                                       A.UPPER_BOUNDARY,
                                                       A.LOWER_BOUNDARY,
                                                       T.EQUIPMENT_KEY,
                                                       A.PARAM_NAME,
                                                       T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                   AND isnull(T.EQUIPMENT_KEY, '0') <> '0'
                                                 GROUP BY A.UPPER_SPEC,
                                                          A.LOWER_SPEC,
                                                          A.TARGET,
                                                          A.UPPER_BOUNDARY,
                                                          A.LOWER_BOUNDARY,
                                                          A.PARAM_NAME,
                                                          T.EQUIPMENT_KEY,
                                                          A.PARAM_NAME,
                                                          T.ROUTE_VER_KEY",
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString(),
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString(),
                                    drControl["route_operation_name"].ToString());

                        #endregion

                        DataTable table = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                        table.TableName = EDC_POINT_PARAMS_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(table, false, MissingSchemaAction.Add);
                        dsReturn.ExtendedProperties.Add("C_TITLE", pro_type);

                        #region 设备 没用
                        //// 设备
                        //if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY))
                        //{
                        //    if (hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY].ToString().Contains(","))
                        //    {
                        //        string key = Convert.ToString(hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY].ToString()).Replace(" ", "");
                        //        key = key.Replace(",", "','");
                        //        tSql += string.Format("   AND T.EQUIPMENT_KEY in ('{0}') ", key);
                        //    }
                        //    else
                        //    {
                        //        tSql += string.Format("   AND T.EQUIPMENT_KEY = '{0}' ", hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY]);
                        //    }
                        //}

                        //if (table != null && table.Rows.Count > 0)
                        //{
                        //    string c_title = pro_type;
                        //    dsReturn.ExtendedProperties.Add("C_TITLE", pro_type);
                        //    dsReturn.ExtendedProperties.Add("USL", table.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY]);//参数规格上限
                        //    dsReturn.ExtendedProperties.Add("LSL", table.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY]);//规格下限                            

                        //    dsReturn.ExtendedProperties.Add("UCL", table.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC]);//参数规格上限
                        //    dsReturn.ExtendedProperties.Add("SL", table.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_TARGET]);//规格中心线
                        //    dsReturn.ExtendedProperties.Add("LCL", table.Rows[0][EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC]);//规格下限                            
                        //}
                        #endregion
                    }
                    #endregion
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC SearchParamValue error:" + ex.Message);
            }
            return dsReturn;
        }

        //Q.003
        /// <summary>
        /// 获得原始点数据
        /// </summary>
        /// <param name="strControlCode">管控代码</param>
        /// <param name="strCol_Key">COL_KEY 键值集合</param>
        /// <returns></returns>
        /// Modify genchille.yang 2012-07-04 10:38:19
        public DataSet GetTableData(string strControlCode, string pointkeys)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                #region sql
                string sql = string.Format(@" SELECT AA.CONTROLCODE, AA.LOCATION_NAME, AA.STEP_NAME, AA.PART_TYPE, AA.LOT_NUMBER,
                                               AA.EQUIPMENT_NAME,AA.PARAM_NAME,AA.PARAM_VALUE,AA.SHIFT_VALUE_DATE,
                                               AA.SHIFT_VALUE, AA.CREATE_TIME, AA.CREATOR, AA.MATERIAL_LOT,
                                               AA.SUPPLIER,BB.PECVD_EQUIPMENT_NAME,CC.TUIHUO_EQUIPMENT_NAME,DD.KUOSAN_EQUIPMENT_NAME
                                          FROM (
                                               --获得采集信息
                                                (SELECT {1} AS CONTROLCODE,D.LOCATION_NAME,
                                                        A.STEP_NAME,A.PART_TYPE,
                                                        B.LOT_NUMBER, C.EQUIPMENT_NAME,
                                                        E.PARAM_NAME,A.PARAM_VALUE,
                                                        '班别=' || TO_CHAR(A.CREATE_TIME, 'yyyy/mm/dd') || ' ' ||
                                                        A.SHIFT_VALUE || '班' AS SHIFT_VALUE_DATE,
                                                        A.SHIFT_VALUE,A.CREATE_TIME,
                                                        A.CREATOR,A.MATERIAL_LOT,A.SUPPLIER,B.LOT_KEY
                                                   FROM SPC_PARAM_DATA A
                                                   INNER JOIN SPC_GROUP_POINTS A1
                                                   ON A.PARAM_KEY=A1.PARAM_KEY AND A.EDC_INS_KEY=A1.EDC_INS_KEY AND A.CREATE_TIME=A1.CREATE_TIME
                                                   AND A.LOCATION_KEY=A1.LOCATION_KEY
                                                   LEFT JOIN EDC_MAIN_INS B
                                                     ON A.EDC_INS_KEY = B.EDC_INS_KEY
                                                   LEFT JOIN EMS_EQUIPMENTS C
                                                     ON A.EQUIPMENT_KEY = C.EQUIPMENT_KEY
                                                   LEFT JOIN FMM_LOCATION D
                                                     ON A.LOCATION_KEY = D.LOCATION_KEY
                                                   LEFT JOIN BASE_PARAMETER E
                                                     ON E.PARAM_KEY = A.PARAM_KEY
                                                  WHERE 1 = 1
                                                    AND ({0})) AA
                                               --获得PECVD设备
                                                LEFT JOIN
                                                (SELECT D.EQUIPMENT_NAME PECVD_EQUIPMENT_NAME, A.PIECE_KEY
                                                   FROM WIP_TRANSACTION A
                                                   LEFT JOIN POR_ROUTE_STEP B
                                                     ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                   LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                     ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                   LEFT JOIN EMS_EQUIPMENTS D
                                                     ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                  WHERE C.ROUTE_OPERATION_NAME = 'PECVD'
                                                    AND A.ACTIVITY = 'TRACKOUT') BB ON AA.LOT_KEY = BB.PIECE_KEY
                                               --获得退火设备                                        
                                                LEFT JOIN
                                                (SELECT D.EQUIPMENT_NAME TUIHUO_EQUIPMENT_NAME, A.PIECE_KEY
                                                   FROM WIP_TRANSACTION A
                                                   LEFT JOIN POR_ROUTE_STEP B
                                                     ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                   LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                     ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                   LEFT JOIN EMS_EQUIPMENTS D
                                                     ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                  WHERE C.ROUTE_OPERATION_NAME = '退火'
                                                    AND A.ACTIVITY = 'TRACKOUT') CC ON AA.LOT_KEY = CC.PIECE_KEY
                                               --获得扩散设备                    
                                                LEFT JOIN
                                                (SELECT D.EQUIPMENT_NAME KUOSAN_EQUIPMENT_NAME, A.PIECE_KEY
                                                   FROM WIP_TRANSACTION A
                                                   LEFT JOIN POR_ROUTE_STEP B
                                                     ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                   LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                     ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                   LEFT JOIN EMS_EQUIPMENTS D
                                                     ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                  WHERE C.ROUTE_OPERATION_NAME = '扩散'
                                                    AND A.ACTIVITY = 'TRACKOUT') DD ON AA.LOT_KEY = DD.PIECE_KEY)
                                         ORDER BY AA.CREATE_TIME ASC
                                        ", pointkeys, strControlCode);
                #endregion

                //dsReturn = dbSPC.ExecuteDataSet(CommandType.Text, sql);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC GetTableData error:" + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得均值信息
        /// </summary>
        /// <param name="strControlCode">管控计划代码</param>
        /// <param name="strCol_Key">COL_KEY键值集合</param>
        /// <returns></returns>
        /// Modify genchille.yang 2012-07-04 10:25:16
        public DataSet GetTableAvData(string strControlCode, string pointkeys)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                #region sql
                string sql = string.Format(@"SELECT AA.CONTROLCODE,AA.LOCATION_NAME,AA.STEP_NAME,AA.PART_TYPE,AA.LOT_NUMBER,AA.EQUIPMENT_NAME,
                                             AA.PARAM_NAME,ROUND(AA.V_VALUE,3) AV_VALUE,AA.SHIFT_VALUE_DATE,AA.SHIFT_VALUE,
                                             AA.CREATE_TIME, AA.CREATOR,AA.MATERIAL_LOT,AA.SUPPLIER,BB.PECVD_EQUIPMENT_NAME,CC.TUIHUO_EQUIPMENT_NAME,
                                             DD.KUOSAN_EQUIPMENT_NAME,AA.EDC_INS_KEY
                                        FROM (
                                              (SELECT {1} AS CONTROLCODE,D.LOCATION_NAME,A1.STEP_NAME,A1.PART_TYPE,B.LOT_NUMBER,
                                                      C.EQUIPMENT_NAME,E.PARAM_NAME,
                                                      A1.V_VALUE,'班别=' || TO_CHAR(A1.CREATE_TIME, 'yyyy/mm/dd') || ' ' ||
                                                      A1.SHIFT_VALUE || '班' AS SHIFT_VALUE_DATE,
                                                      A1.SHIFT_VALUE,A1.CREATE_TIME,'' AS CREATOR,A1.MATERIAL_LOT,
                                                      A1.SUPPLIER, B.LOT_KEY,A1.EDC_INS_KEY
                                                 FROM spc_group_points A1       
                                                 LEFT JOIN EDC_MAIN_INS B
                                                   ON A1.EDC_INS_KEY = B.EDC_INS_KEY
                                                 LEFT JOIN EMS_EQUIPMENTS C
                                                   ON A1.EQUIPMENT_KEY = C.EQUIPMENT_KEY
                                                 LEFT JOIN FMM_LOCATION D
                                                   ON A1.LOCATION_KEY = D.LOCATION_KEY
                                                 LEFT JOIN BASE_PARAMETER E
                                                   ON E.PARAM_KEY = A1.PARAM_KEY
                                                WHERE 1 = 1
                                                  AND ({0})) AA
                                             --获得PECVD设备
                                              LEFT JOIN
                                              (SELECT D.EQUIPMENT_NAME PECVD_EQUIPMENT_NAME, A.PIECE_KEY
                                                 FROM WIP_TRANSACTION A
                                                 LEFT JOIN POR_ROUTE_STEP B
                                                   ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                 LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                   ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                 LEFT JOIN EMS_EQUIPMENTS D
                                                   ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                WHERE C.ROUTE_OPERATION_NAME = 'PECVD'
                                                  AND A.ACTIVITY = 'TRACKOUT') BB ON AA.LOT_KEY = BB.PIECE_KEY
                                             --获得退火设备
                                              LEFT JOIN
                                              (SELECT D.EQUIPMENT_NAME TUIHUO_EQUIPMENT_NAME, A.PIECE_KEY
                                                 FROM WIP_TRANSACTION A
                                                 LEFT JOIN POR_ROUTE_STEP B
                                                   ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                 LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                   ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                 LEFT JOIN EMS_EQUIPMENTS D
                                                   ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                WHERE C.ROUTE_OPERATION_NAME = '退火'
                                                  AND A.ACTIVITY = 'TRACKOUT') CC ON AA.LOT_KEY = CC.PIECE_KEY
                                             --获得扩散设备
                                              LEFT JOIN
                                              (SELECT D.EQUIPMENT_NAME KUOSAN_EQUIPMENT_NAME, A.PIECE_KEY
                                                 FROM WIP_TRANSACTION A
                                                 LEFT JOIN POR_ROUTE_STEP B
                                                   ON A.STEP_KEY = B.ROUTE_STEP_KEY
                                                 LEFT JOIN POR_ROUTE_OPERATION_VER C
                                                   ON C.ROUTE_OPERATION_VER_KEY = B.ROUTE_OPERATION_VER_KEY
                                                 LEFT JOIN EMS_EQUIPMENTS D
                                                   ON D.EQUIPMENT_KEY = A.EQUIPMENT_KEY
                                                WHERE C.ROUTE_OPERATION_NAME = '扩散'
                                                  AND A.ACTIVITY = 'TRACKOUT') DD ON AA.LOT_KEY = DD.PIECE_KEY)
                                        ORDER BY AA.CREATE_TIME ASC
                                    ", pointkeys, strControlCode);
                #endregion

                //dsReturn = dbSPC.ExecuteDataSet(CommandType.Text, sql);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC GetTableData error:" + ex.Message);
            }
            return dsReturn;
        }

        private string KK(string strtemp)
        {
            string strreturn = "'" + strtemp + "'";
            return strreturn;
        }

        //public DataSet GetSpcMaterialSuppliers()


        public DataSet SearchPChartData(DataTable dataTable)
        {
            DataSet dsReturn = new DataSet();
            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
            try
            {
                string sql = @"SELECT                       
               isnull(SUM(CASE
                         WHEN A.QTY_TYPE = 'LT' THEN
                          A.QTY_OUT
                         ELSE
                          0
                       END),
                   0) AS QTY_TOTAL,
               isnull(SUM(CASE
                         WHEN A.QTY_TYPE = 'BF' THEN
                          A.QTY_OUT
                         ELSE
                          0
                       END),
                   0) AS QTY_SCRAP              
                FROM v_Wip_Online_Qty A 
                where 1=1";
                //if (hashTable.ContainsKey(Constant.START_TIME))
                //{
                //    sql += " and to_date(A.day,'yyyy-mm-dd')>=to_date('" + hashTable[Constant.START_TIME].ToString() + "','yyyy-mm-dd')";
                //}
                //if (hashTable.ContainsKey(Constant.END_TIME))
                //{
                //    sql += " and to_date(A.day,'yyyy-mm-dd')<=to_date('" + hashTable[Constant.END_TIME].ToString() + "','yyyy-mm-dd')";
                //}               
                if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.ORDER_NUMBER))
                {
                    sql += " and A.ORDER_NUMBER='" + hashTable[SPC_PARAM_DATA_FIELDS.ORDER_NUMBER] + "'";
                }
                if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SHIFT_VALUE))
                {
                    sql += " and A.SHIFT_VALUE='" + hashTable[SPC_PARAM_DATA_FIELDS.SHIFT_VALUE] + "'";
                }

                //sql += " group by a.ROUTE_STEP_NAME,a.route_step_seq order by a.route_step_seq";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsReturn;
        }


        /// <summary>
        /// 剔除图中异常点
        /// </summary>
        /// <param name="dataSet">原始数据主键</param>
        /// <returns></returns>
        public DataSet DeletePoints(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string editor = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDITOR].ToString();
                string reason = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDIT_REASON].ToString();
                string poingkey = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.POINT_KEY].ToString();
                bool isMr = Convert.ToBoolean(dataSet.ExtendedProperties["isMr"]);

                string strDelete = @"UPDATE spc_group_points T
                                            SET T.DELETED_REASON=:DELETED_REASON,T.DELETE_TIME=SYSDATE,T.DELETER=:DELETER,T.DELETED_FLAG=1
                                            WHERE T.POINT_KEY=:POINT_KEY";
                //DbCommand dbCmd = dbSPC.GetSqlStringCommand(strDelete);
                DbCommand dbCmd = db.GetSqlStringCommand(strDelete);
                db.AddInParameter(dbCmd, "DELETED_REASON", DbType.String, reason);
                db.AddInParameter(dbCmd, "DELETER", DbType.String, editor);
                db.AddInParameter(dbCmd, "POINT_KEY", DbType.String, poingkey);
                db.ExecuteNonQuery(dbCmd);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("剔除图中异常点出错：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }


        /// <summary>
        /// 修改图中的异常点
        /// </summary>
        /// <param name="dataSet">原始数据主键</param>
        /// <returns></returns>
        public DataSet ModifyPoints(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string editor = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDITOR].ToString();
                string reason = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDIT_REASON].ToString();
                string pointkey = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.POINT_KEY].ToString();
                string editFlag = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDIT_FLAG].ToString();
                DbTransaction dbTrans = null;
                DbConnection dbCon = null;
                try
                {
                    string strPoint = string.Format(@"UPDATE spc_group_points t SET T.EDIT_FLAG=:EDIT_FLAG
                                                          WHERE T.POINT_KEY=:POINT_KEY", editFlag, pointkey);
                    string strPointEdit = string.Format(@"INSERT INTO SPC_GROUP_POINTS_EDIT T
                                                            (POINT_DTL_KEY,POINT_KEY,EDIT_REASON,EDITOR)
                                                            VALUES(SYS_GUID(),:POINT_KEY,:EDIT_REASON,:EDITOR)", pointkey, reason, editor);
                    //dbCon = dbSPC.CreateConnection();
                    dbCon = db.CreateConnection();
                    dbCon.Open();
                    dbTrans = dbCon.BeginTransaction();
                    //DbCommand dbCmdUpdate = dbSPC.GetSqlStringCommand(strPoint);
                    DbCommand dbCmdUpdate = db.GetSqlStringCommand(strPoint);
                    //dbSPC.AddInParameter(dbCmdUpdate, "POINT_KEY", DbType.String, pointkey);
                    //dbSPC.AddInParameter(dbCmdUpdate, "EDIT_FLAG", DbType.String, editFlag);
                    //dbSPC.ExecuteNonQuery(dbCmdUpdate, dbTrans);
                    db.AddInParameter(dbCmdUpdate, "POINT_KEY", DbType.String, pointkey);
                    db.AddInParameter(dbCmdUpdate, "EDIT_FLAG", DbType.String, editFlag);
                    db.ExecuteNonQuery(dbCmdUpdate, dbTrans);
                    dbCmdUpdate = null;

                    //DbCommand dbCmdInsert = dbSPC.GetSqlStringCommand(strPointEdit);
                    //dbSPC.AddInParameter(dbCmdInsert, "POINT_KEY", DbType.String, pointkey);
                    //dbSPC.AddInParameter(dbCmdInsert, "EDIT_REASON", DbType.String, reason);
                    //dbSPC.AddInParameter(dbCmdInsert, "EDITOR", DbType.String, editor);
                    //dbSPC.ExecuteNonQuery(dbCmdInsert, dbTrans);
                    DbCommand dbCmdInsert = db.GetSqlStringCommand(strPointEdit);
                    db.AddInParameter(dbCmdInsert, "POINT_KEY", DbType.String, pointkey);
                    db.AddInParameter(dbCmdInsert, "EDIT_REASON", DbType.String, reason);
                    db.AddInParameter(dbCmdInsert, "EDITOR", DbType.String, editor);
                    db.ExecuteNonQuery(dbCmdInsert, dbTrans);
                    dbCmdInsert = null;

                    dbTrans.Commit();
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
                catch (Exception ex)
                {
                    if (dbTrans != null)
                    {
                        dbTrans.Rollback();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (dbCon != null)
                    {
                        dbCon.Close();
                    }
                    dbTrans = null;
                    dbCon = null;
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("修正图中异常点出错：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        public DataSet ModifyPointsForMr(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Contains(SPC_PARAM_DATA_FIELDS.DATABASE_TABLE_NAME))
                {
                    string editor = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDITOR].ToString();
                    string reason = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDIT_REASON].ToString();
                    string colKey = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.COL_KEY].ToString();
                    string editFlag = dataSet.ExtendedProperties[SPC_PARAM_DATA_FIELDS.EDIT_FLAG].ToString();
                    DbConnection dbconn = null;
                    DbTransaction dbtran = null;
                    try
                    {
                        dbconn = db.CreateConnection();
                        //Open Connection
                        dbconn.Open();
                        //Create Transaction  
                        dbtran = dbconn.BeginTransaction();

                        foreach (DataRow row in dataSet.Tables[SPC_PARAM_DATA_FIELDS.DATABASE_TABLE_NAME].Rows)
                        {
                            string sql = string.Format(@"UPDATE SPC_PARAM_DATA A
                                                           SET A.EDIT_FLAG   = {4},
                                                               A.EDITOR      = '{0}',
                                                               A.EDIT_REASON = '{1}',
                                                               A.EDIT_TIME   = {2}
                                                         WHERE A.COL_KEY = '{3}'",
                                                          editor,
                                                          reason,
                                                          "TO_DATE(TO_CHAR(SYSDATE,'yyyy-mm-dd hh24:mi:ss'),'yyyy-MM-DD hh24:mi:ss')",
                                                          row[SPC_PARAM_DATA_FIELDS.COL_KEY].ToString(),
                                                          editFlag);
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                        }

                        dbtran.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        dbtran.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        dbconn.Close();
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "no input data");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("修正图中异常点出错：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 编辑点信息
        /// </summary>
        /// <param name="edccolkey"></param>
        /// <returns></returns>
        /// owner genchille.yang 2012-07-29 10:53:19
        public DataSet GetEditInformation(string pointkey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = string.Format(@"SELECT TO_CHAR(T.EDIT_TIME,'yyyy/MM/dd hh24:mi:ss') EDIT_TIME,T.EDITOR,T.EDIT_REASON FROM SPC_GROUP_POINTS_EDIT T
                                            WHERE T.POINT_KEY='{0}'
                                            ORDER BY T.EDIT_TIME DESC", pointkey);

                //DataTable dtEditInformation = dbSPC.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                DataTable dtEditInformation = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dtEditInformation.TableName = SPC_GROUP_POINT_EDIT_FIELDS.DATABASE_TABLE_NAME;
                dsReturn.Merge(dtEditInformation, false, MissingSchemaAction.Add);

                DataTable dtPoint = GetPointInformation(pointkey).Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];
                dsReturn.Merge(dtPoint, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("获得编辑信息 GetEdditInformation：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得采集点信息
        /// </summary>
        /// <param name="dataSet">原始数据主键</param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-05-08 13:59:56
        public DataSet GetPointDetailInformation(string pointkey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string pointKey = pointkey, location_name = string.Empty, param_name = string.Empty, param_key = string.Empty, edc_ins_key = string.Empty, location_key = string.Empty, create_date = string.Empty;
                string sql01 = string.Format(@" SELECT T1.PARAM_KEY,T1.EDC_INS_KEY,T1.LOCATION_KEY,T1.CREATE_TIME,
                                                   T2.LOCATION_NAME,
                                                   T3.PARAM_NAME
                                              FROM SPC_GROUP_POINTS T1,
                                                   FMM_LOCATION     T2,
                                                   BASE_PARAMETER   T3,
                                                   EMS_EQUIPMENTS   T4
                                             WHERE T1.LOCATION_KEY = T2.LOCATION_KEY
                                               AND T1.PARAM_KEY = T3.PARAM_KEY
                                               AND T1.EQUIPMENT_KEY = T4.EQUIPMENT_KEY
                                               AND T1.POINT_KEY = '{0}'", pointKey);
                //DataTable dt01 = dbSPC.ExecuteDataSet(CommandType.Text, sql01).Tables[0];
                DataTable dt01 = db.ExecuteDataSet(CommandType.Text, sql01).Tables[0];
                DataRow dr01 = dt01.Rows[0];
                location_name = dr01[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME].ToString();
                param_name = dr01[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString();
                param_key = dr01[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY].ToString();
                edc_ins_key = dr01[EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY].ToString();
                location_key = dr01[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY].ToString();
                create_date = dr01[SPC_PARAM_DATA_FIELDS.CREATE_TIME].ToString();
                //--------------------------------------------------------------------------------
                string sql02 = string.Format(@"SELECT ROWNUM,'{0}' LOCATION_NAME,T.STEP_NAME,
                                                   T.LOT_NUMBER,'{1}' PARAM_NAME,ROUND(T.PARAM_VALUE, 5) PARAM_VALUE,
                                                   TO_CHAR(T.CREATE_TIME, 'yyyy/MM/dd hh24:mi:ss') CREATE_TIME,
                                                   T.ORDER_NUMBER,T.SHIFT_VALUE,T.PART_NO,T.PART_TYPE, T.MATERIAL_LOT,T.SUPPLIER
                                              FROM SPC_PARAM_DATA   T
                                             WHERE T.PARAM_KEY = '{2}'
                                               AND T.EDC_INS_KEY ='{3}'
                                               AND T.LOCATION_KEY = '{4}'
                                               AND T.CREATE_TIME = TO_DATE('{5}','yyyy/MM/dd hh24:mi:ss')",
                                                    location_name, param_name, param_key, edc_ins_key, location_key, create_date);

                //DataTable dt = dbSPC.ExecuteDataSet(CommandType.Text, sql02).Tables[0];
                DataTable dt = db.ExecuteDataSet(CommandType.Text, sql02).Tables[0];
                dt.TableName = SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS;
                dsReturn.Merge(dt, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("获得采集信息 GetPointDetailInformation：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得采集点信息
        /// </summary>
        /// <param name="dataSet">原始数据主键</param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-05-08 13:59:56
        public DataSet GetPointInformation(string pointkey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string pointKey = pointkey;
                string sql = string.Empty;

                sql = string.Format(@"SELECT C.Part_no,C.Part_type,C.Shift_value,C.Supplier,C.Material_lot,C.Order_number,
                                     D.Location_name,A.Lot_number,A.QUANTITY QTY,A.Creator,TO_CHAR(A.Create_time,'yyyy/MM/dd hh24:mi:ss') Create_time,
                                     CASE A.REWORK_FLAG WHEN 0 THEN '正常批' WHEN 1 THEN '返工批' ELSE '' END Rework_flag,
                                     E.Equipment_name
                                FROM POR_LOT A, EDC_MAIN_INS B,spc_group_points C,FMM_LOCATION D,EMS_EQUIPMENTS E
                                WHERE B.LOT_NUMBER=A.LOT_NUMBER(+)
                                AND B.EDC_INS_KEY = C.EDC_INS_KEY
                                AND C.LOCATION_KEY = D.LOCATION_KEY
                                AND C.EQUIPMENT_KEY = E.EQUIPMENT_KEY
                                AND C.POINT_KEY='{0}'", pointKey);
                DataTable dt = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                //DataTable dt = dbSPC.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                dt.TableName = SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS;
                dsReturn.Merge(dt, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                LogService.LogError("获得采集信息 GetPointInformation：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region DataSet GetSPControlData()获得管控计划页面基本绑定信息
        public DataSet GetSPControlData()
        {
            DataSet dsParams = new DataSet();
            try
            {
                string[] arrSql = new string[5];
                //车间
                arrSql[0] = @"SELECT DISTINCT T.LOCATION_KEY, T.LOCATION_NAME
                              FROM FMM_LOCATION T, FMM_LOCATION_RET T5
                             WHERE T5.LOCATION_LEVEL = 9
                               AND T.LOCATION_KEY = T5.PARENT_LOC_KEY";
                //工序信息
                arrSql[1] = @"SELECT T.ROUTE_OPERATION_VER_KEY, T.ROUTE_OPERATION_NAME
                              FROM POR_ROUTE_OPERATION_VER T
                             WHERE T.OPERATION_STATUS = 1 --表示激活
                               AND isnull(T.ROUTE_OPERATION_NAME, '0') <> '0'";
                //采样参数
                arrSql[2] = @"  SELECT DISTINCT T.PARAM_KEY,
                              T.PARAM_NAME,
                              T1.OPERATION_NAME,
                              T1.PART_TYPE,
                              MAX(T.UPPER_BOUNDARY) UPPER_BOUNDARY,
                              MAX(T.UPPER_SPEC) UPPER_SPEC,
                              MAX(T.UPPER_CONTROL) UPPER_CONTROL,
                              MAX(T.LOWER_CONTROL) LOWER_CONTROL,
                              MAX(T.LOWER_SPEC) LOWER_SPEC,
                              MAX(T.LOWER_BOUNDARY) LOWER_BOUNDARY,
                              MAX(T.TARGET) TARGET
                FROM EDC_POINT_PARAMS T, EDC_POINT T1
               WHERE T.EDC_POINT_ROWKEY = T1.ROW_KEY
                 AND T1.POINT_STATUS = 1
                 AND isnull(T.PARAM_KEY, '0') <> '0'
               GROUP BY T.PARAM_KEY, T.PARAM_NAME, T1.OPERATION_NAME, T1.PART_TYPE";
                //设备
                arrSql[3] = @"SELECT T1.OPERATION_KEY,
                               T2.PARENT_LOC_KEY AS LOCATION_KEY, --车间
                               T0.EQUIPMENT_KEY,
                               T0.EQUIPMENT_NAME,
                               T0.EQUIPMENT_CODE
                          FROM EMS_EQUIPMENTS T0, EMS_OPERATION_EQUIPMENT T1, FMM_LOCATION_RET T2
                         WHERE T0.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                           AND T0.LOCATION_KEY = T2.LOCATION_KEY
                           AND T2.LOCATION_LEVEL = 9
                           AND T2.PARENT_LOC_LEVEL = 5";
                //产品类型
                arrSql[4] = @"SELECT T.ITEM_ORDER,
                                           MAX(CASE T.ATTRIBUTE_NAME WHEN 'CODE' THEN T.ATTRIBUTE_VALUE END) AS TYPECODE,
                                           MAX(CASE T.ATTRIBUTE_NAME WHEN 'NAME' THEN T.ATTRIBUTE_VALUE END) AS TYPENAME
                                      FROM CRM_ATTRIBUTE T, BASE_ATTRIBUTE T1, BASE_ATTRIBUTE_CATEGORY T2
                                     WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY
                                       AND T1.CATEGORY_KEY = T2.CATEGORY_KEY
                                       AND UPPER(T2.CATEGORY_NAME) = 'LOT_TYPE'
                                     GROUP BY T.ITEM_ORDER
                                     ORDER BY T.ITEM_ORDER ASC";
                for (int i = 0; i < arrSql.Length; i++)
                {
                    DataTable dtTable = db.ExecuteDataSet(CommandType.Text, arrSql[i]).Tables[0];
                    switch (i)
                    {
                        case 0:
                            dtTable.TableName = FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME;
                            break;
                        //case 1:
                        //    dtTable.TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
                        //    break;
                        case 1:
                            dtTable.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                            break;
                        case 2:
                            dtTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                            break;
                        //case 3:
                        //    dtTable.TableName = "FMM_LOCATION_AREAS";
                        //    break;
                        case 3:
                            dtTable.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                            break;
                        case 4:
                            dtTable.TableName = "PARTYPE"; //产品类型
                            break;
                    }

                    dsParams.Merge(dtTable, false, MissingSchemaAction.Add);
                }
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsParams, ex.Message);
                LogService.LogError("GetSPControlData error:" + ex.Message);
            }
            return dsParams;
        }

        public DataSet GetSPControlGridData()
        {
            DataSet dsSpcControlGrid = new DataSet();
            try
            {

                string strSql = @"SELECT DISTINCT T1.*,
                                T2.LOCATION_NAME,
                                T3.STEP_TYPE,
                                T4.PARAM_NAME,
                                T4.UPPER_BOUNDARY,
                                T4.UPPER_SPEC,
                                T4.UPPER_CONTROL,
                                T4.LOWER_CONTROL,
                                T4.LOWER_SPEC,
                                T4.LOWER_BOUNDARY,
                                T4.TARGET,
                                CASE T1.STATES
                                      WHEN '1' THEN
                                       '激活' 
                                      WHEN '0' THEN
                                       '未激活' 
                                      WHEN '2' THEN
                                       '存档'
                                      ELSE ''
                                    END STATENAME
                  FROM SPC_CONTROL_PLAN T1,
                       FMM_LOCATION     T2,
                       POR_ROUTE_STEP   T3,
                       (
                       SELECT A.OPERATION_NAME,
                              A.PART_TYPE,
                              B.PARAM_KEY,
                              B.PARAM_NAME,
                              MAX(B.UPPER_BOUNDARY) UPPER_BOUNDARY,
                              MAX(B.UPPER_SPEC) UPPER_SPEC,
                              MAX(B.UPPER_CONTROL) UPPER_CONTROL,
                              MAX(B.LOWER_CONTROL) LOWER_CONTROL,
                              MAX(B.LOWER_SPEC) LOWER_SPEC,
                              MAX(B.LOWER_BOUNDARY) LOWER_BOUNDARY,
                              MAX(B.TARGET) TARGET
                         FROM EDC_POINT A, EDC_POINT_PARAMS B
                        WHERE A.ROW_KEY = B.EDC_POINT_ROWKEY
                          AND A.POINT_STATUS = 1
                        GROUP BY A.OPERATION_NAME, A.PART_TYPE, B.PARAM_KEY, B.PARAM_NAME
                       ) T4
                 WHERE T1.WERKS = T2.LOCATION_KEY
                   AND T1.STEP_KEY = T3.ROUTE_OPERATION_VER_KEY
                   AND T1.PARAMENTID = T4.PARAM_KEY
                   AND T3.ROUTE_STEP_NAME=T4.OPERATION_NAME
                   AND T1.PRODUCTCODE=T4.PART_TYPE
                   AND T1.STATES <> '3' 
                   ORDER BY T1.CONTROLCODE ASC";

                DataTable dtTable = db.ExecuteDataSet(CommandType.Text, strSql).Tables[0];
                dtTable.TableName = SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME;
                dsSpcControlGrid.Merge(dtTable, false, MissingSchemaAction.Add);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsSpcControlGrid, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsSpcControlGrid, ex.Message);
                LogService.LogError("GetSPControlGridData error:" + ex.Message);
            }
            return dsSpcControlGrid;
        }
        #endregion  

        public DataSet SaveSpcControlPlan(DataSet dsSpcControl)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            string sqlCommand = "";

            try
            {
                //设定新增的管控代码---有数字组成，根据车间+工序组成的管控代码
                if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME))
                {
                    string _string = string.Empty;
                    DataTable dtNew = dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME];
                    DataRow dr = dtNew.Rows[0];
                    sqlCommand = string.Format(@" SELECT count(T.CONTROLCODE)
                                                   FROM SPC_CONTROL_PLAN T
                                                  WHERE T.WERKS = '{0}'
                                                    AND T.STEP_KEY = '{1}'
                                                    AND T.PRODUCTCODE='{2}'",
                                                     dr[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString(),
                                                     dr[SPC_CONTROL_PLAN_FIELD.FIELD_STEP_KEY].ToString(),
                                                     dr[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString());
                    string sqlCommand01 = @"SELECT MAX(count(T.CONTROLCODE,0)) FROM SPC_CONTROL_PLAN T";
                    int i = 0;
                    try
                    {
                        i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand)) + 1;
                    }
                    catch (Exception ex)
                    {
                        LogService.LogError("SaveSpcControlPlan Get MAX(isnull(CONTROLCODE,0)) Error: " + ex.Message);
                        i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand01)) + 1;
                    }
                    dtNew.Rows[0][SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE] = i.ToString();

                    if (dataDs.ExtendedProperties.ContainsKey("CONTROLCODE"))
                        dataDs.ExtendedProperties["CONTROLCODE"] = i.ToString();
                    else
                        dataDs.ExtendedProperties.Add("CONTROLCODE", i.ToString());

                }
                //判断修改的管控代码是否有重复项
                #region
                if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE))
                {
                    string _code = string.Empty, _key = string.Empty;
                    DataTable dtModify = dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE];

                    foreach (DataRow dr in dtModify.Rows)
                    {
                        _code = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString();
                        _key = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString();

                        sqlCommand = string.Format(@"SELECT COUNT(0)
                                                      FROM SPC_CONTROL_PLAN T
                                                     WHERE T.CONTROLCODE = '{0}'
                                                       AND T.CONTROLPLANID <> '{1}'", _code, _key);
                        int i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                        if (i > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "修改的管控代码有重复!");
                            return dataDs;
                        }
                    }
                }
                #endregion

                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    try
                    {
                        SPC_CONTROL_PLAN_FIELD spcControlPlan = new SPC_CONTROL_PLAN_FIELD();
                        //Insert
                        if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME))
                        {
                            foreach (DataRow dr in dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME].Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(spcControlPlan, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //Update
                        if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE))
                        {
                            foreach (DataRow dr in dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE].Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                                WhereConditions wc = new WhereConditions(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID, hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString());
                                hashTable.Remove(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID);

                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(spcControlPlan, hashTable, wc);

                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }

                        //Commit Transaction
                        dbTran.Commit();

                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                    }
                    catch (Exception ex)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                        //Rollback Transaction
                        dbTran.Rollback();
                        LogService.LogError("SaveSpcControlPlan Error: " + ex.Message);
                    }
                    finally
                    {
                        //Close Connection
                        dbConn.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("Insert Abnormal Error: " + ex.Message);
            }

            return dataDs;
        }

        #region 新增/修改 SPC控制计划资料
        public DataSet SaveSpcControlPlan_Old(DataSet dsSpcControl)
        {
            //get dynamic dataset constructor
            DataSet dataDs = AllCommonFunctions.GetDynamicTwoColumnsDataSet();
            //define sql 
            string sqlCommand = "";

            try
            {
                if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME))
                {
                    string _string = string.Empty;
                    DataTable dtNew = dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME];
                    foreach (DataRow dr in dtNew.Rows)
                    {
                        _string += "'" + dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString() + "',";
                    }

                    if (!string.IsNullOrEmpty(_string))
                    {
                        _string = _string.TrimEnd(',');
                        sqlCommand = string.Format(@"SELECT COUNT(0) FROM SPC_CONTROL_PLAN T WHERE T.CONTROLCODE IN ({0})", _string);
                        int i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                        if (i > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "新增管控代码重复!");
                            return dataDs;
                        }
                    }
                }

                if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE))
                {
                    string _code = string.Empty, _key = string.Empty;
                    DataTable dtModify = dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE];

                    foreach (DataRow dr in dtModify.Rows)
                    {
                        _code = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString();
                        _key = dr[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString();

                        sqlCommand = string.Format(@"SELECT COUNT(0)
                                                      FROM SPC_CONTROL_PLAN T
                                                     WHERE T.CONTROLCODE = '{0}'
                                                       AND T.CONTROLPLANID <> '{1}'", _code, _key);
                        int i = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sqlCommand));
                        if (i > 0)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "修改的管控代码有重复!");
                            return dataDs;
                        }
                    }
                }

                using (DbConnection dbConn = db.CreateConnection())
                {
                    //Open Connection
                    dbConn.Open();
                    //Create Transaction
                    DbTransaction dbTran = dbConn.BeginTransaction();
                    try
                    {
                        SPC_CONTROL_PLAN_FIELD spcControlPlan = new SPC_CONTROL_PLAN_FIELD();
                        //Insert
                        if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME))
                        {
                            foreach (DataRow dr in dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME].Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);
                                sqlCommand = DatabaseTable.BuildInsertSqlStatement(spcControlPlan, hashTable, null);
                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }
                        //Update
                        if (dsSpcControl.Tables.Contains(SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE))
                        {
                            foreach (DataRow dr in dsSpcControl.Tables[SPC_CONTROL_PLAN_FIELD.DATABASE_TABLE_NAME_FORUPDATE].Rows)
                            {
                                Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertRowToHashtable(dr);

                                WhereConditions wc = new WhereConditions(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID, hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString());
                                hashTable.Remove(SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID);

                                sqlCommand = DatabaseTable.BuildUpdateSqlStatement(spcControlPlan, hashTable, wc);

                                db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);
                            }
                        }

                        //Commit Transaction
                        dbTran.Commit();

                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                    }
                    catch (Exception ex)
                    {
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                        //Rollback Transaction
                        dbTran.Rollback();
                        LogService.LogError("SaveSpcControlPlan Error: " + ex.Message);
                    }
                    finally
                    {
                        //Close Connection
                        dbConn.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("Insert Abnormal Error: " + ex.Message);
            }

            return dataDs;
        }
        #endregion

        #region 删除或更新资料
        public DataSet UpdateControlPlan(List<string> arrlst)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (arrlst.Count > 0)
                {
                    using (DbConnection dbconn = db.CreateConnection())
                    {
                        dbconn.Open();

                        DbTransaction dbtran = dbconn.BeginTransaction();
                        try
                        {

                            string sql = string.Format(@"UPDATE SPC_CONTROL_PLAN
                                                       SET STATES = '{0}', EDITOR = '{1}', EDIT_TIME = TO_DATE('{2}', 'yyyy-MM-dd hh24:mi:ss')
                                                     WHERE CONTROLPLANID ='{3}' ", arrlst[1], arrlst[2], DateTime.Now.ToShortDateString(), arrlst[0]);
                            db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                            dbtran.Commit();
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            dbtran.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
                else
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "no input data");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError("SPC管控规则操作错误：" + ex.Message);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
            }
            return dsReturn;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationKey"></param>
        /// <param name="paramKey"></param>
        /// <param name="stepName"></param>
        /// <param name="partType"></param>
        /// <param name="quertType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="points"></param>
        /// <param name="a_avg"></param>
        /// <returns></returns>
        public double GetPSigmaByProc(string locationKey, string paramKey, string stepName, string partType, string startDate, string endDate, double v_avg, string equipment_keys, string supplier)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string procedureName = "SP_GET_SPC_AVG";
            //DbCommand dbCmd = dbSPC.GetStoredProcCommand(procedureName);
            //dbSPC.AddInParameter(dbCmd, "V_LOCATION_KEY", DbType.String, locationKey);
            //dbSPC.AddInParameter(dbCmd, "V_PARAM_KEY", DbType.String, paramKey);
            //dbSPC.AddInParameter(dbCmd, "V_STEP_NAME", DbType.String, stepName);
            //dbSPC.AddInParameter(dbCmd, "V_PART_TYPE", DbType.String, partType);
            //dbSPC.AddInParameter(dbCmd, "V_START_DATE", DbType.String, startDate);
            //dbSPC.AddInParameter(dbCmd, "END_DATE", DbType.String, endDate);
            //dbSPC.AddInParameter(dbCmd, "V_AVG", DbType.Double, v_avg);
            //dbSPC.AddOutParameter(dbCmd, "OUTSQL", DbType.String, 50);
            //dbSPC.AddOutParameter(dbCmd, "OUTERRMSG", DbType.String, 255);
            //dbSPC.ExecuteNonQuery(dbCmd);
            //object pv = dbSPC.GetParameterValue(dbCmd, "OUTSQL");
            DbCommand dbCmd = db.GetStoredProcCommand(procedureName);
            db.AddInParameter(dbCmd, "V_LOCATION_KEY", DbType.String, locationKey);
            db.AddInParameter(dbCmd, "V_PARAM_KEY", DbType.String, paramKey);
            db.AddInParameter(dbCmd, "V_STEP_NAME", DbType.String, stepName);
            db.AddInParameter(dbCmd, "V_PART_TYPE", DbType.String, partType);
            db.AddInParameter(dbCmd, "V_START_DATE", DbType.String, startDate);
            db.AddInParameter(dbCmd, "END_DATE", DbType.String, endDate);
            db.AddInParameter(dbCmd, "V_AVG", DbType.Double, v_avg);
            db.AddOutParameter(dbCmd, "OUTSQL", DbType.String, 50);
            db.AddOutParameter(dbCmd, "OUTERRMSG", DbType.String, 255);
            db.ExecuteNonQuery(dbCmd);
            object pv = db.GetParameterValue(dbCmd, "OUTSQL");
            return Convert.ToDouble(pv);
        }

        /// <summary>
        /// SPC参数，SPC空值图数据源
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Modify by genchille.yang 2012-05-2 15:27:19 
        public DataSet SearchParamValueForXbar(DataTable dataTable)
        {
            //定义返回的数据源
            DataSet dsReturn = new DataSet();
            try
            {
                double p_sigma = 0;
                string orderNumber = string.Empty;
                string pro_type = string.Empty;
                string s_tmp = string.Empty, s_exep = string.Empty, s_exes = string.Empty;
                //定义查询参数
                string locationKey = string.Empty, paramKey = string.Empty, partType = string.Empty, equipmentKey = string.Empty, stepName = string.Empty;
                string startDate = string.Empty, endDate = string.Empty;
                int points = 0;
                int code = 2;

                //dataTable 数据查询条件，如果为空则不能找到查询的数据，不能为null。
                if (dataTable != null)
                {
                    #region 查询参数值
                    Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    DataTable dtControlPlan = GetControlPlanData(hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID].ToString());
                    DataRow drControl = dtControlPlan.Rows[0];

                    string controlType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
                    //------------------------------------------------------------------------------------------------------------
                    locationKey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString();
                    paramKey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
                    stepName = drControl[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString();
                    partType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY))
                        equipmentKey = hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY].ToString();
                    //--------------------------------------------------------------------------------------------------------------

                    #region
                    //read point data
                    //const string sql_points = @"SELECT T.POINT_KEY,T.EDC_INS_KEY,TO_CHAR(T.CREATE_TIME,'MM/dd hh24:mi:ss') CREATE_TIME,T.EQUIPMENT_KEY,
                    //                             T.SHIFT_VALUE,isnull(T.ROUTE_KEY,'0') ROUTE_KEY,T.SUPPLIER,T.MATERIAL_LOT,ROUND(T.V_VALUE,5) V_VALUE,ROUND(T.S_VALUE,5) S_VALUE,ROUND(T.R_VALUE,5) R_VALUE,
                    //                             T.P_COUNT,ROUND(T.A_VALUE,5) A_VALUE,T.EDIT_FLAG,T.DELETED_FLAG,T.LOT_NUMBER,T.CREATE_TIME CREATETIME
                    //                        FROM SPC_GROUP_POINTS T
                    //                        WHERE T.DELETED_FLAG = 0
                    //                         AND T.LOCATION_KEY = '{0}'
                    //                         AND T.PARAM_KEY = '{1}'
                    //                         AND T.STEP_NAME = '{3}'
                    //                         AND T.PART_TYPE = '{2}'";
                    const string sql_points = @"SELECT T.EDC_INS_KEY,format(T.CREATE_TIME,'MM/dd hh24:mi:ss') CREATE_TIME,T.EQUIPMENT_KEY,
                                                 T.SHIFT_VALUE,isnull(T.ROUTE_KEY,'0') ROUTE_KEY,T.SUPPLIER,T.MATERIAL_LOT,ROUND(T.V_VALUE,5) V_VALUE,ROUND(T.S_VALUE,5) S_VALUE,ROUND(T.R_VALUE,5) R_VALUE,
                                                 T.P_COUNT,ROUND(T.A_VALUE,5) A_VALUE,T.EDIT_FLAG,T.DELETED_FLAG,T.CREATE_TIME CREATETIME
                                            FROM SPC_GROUP_POINTS T
                                            WHERE T.DELETED_FLAG = 0
                                             AND T.LOCATION_KEY = '{0}'
                                             AND T.PARAM_KEY = '{1}'
                                             AND T.STEP_NAME = '{3}'
                                             AND T.PART_TYPE = '{2}'";
                    //read standard data
                    const string sql_vsr = @"SELECT ROUND(AVG(T.V_VALUE),5) V_AVG,ROUND(AVG(T.R_VALUE),5) R_AVG,ROUND(AVG(T.S_VALUE),5) S_AVG,isnull(SUM(T.P_COUNT),0) P_DTL_COUNT,
                                             isnull(COUNT(0),0) P_A_COUNT,MIN(ROUND(T.V_VALUE, 5)) MIN_V_VALUE,MAX(ROUND(T.V_VALUE, 5)) MAX_V_VALUE,MIN(ROUND(T.S_VALUE,5)) MIN_S_VALUE,
                                             MAX(ROUND(T.S_VALUE,5)) MAX_S_VALUE,MIN(ROUND(T.R_VALUE,5)) MIN_R_VALUE,MAX(ROUND(T.R_VALUE,5)) MAX_R_VALUE
                                      FROM SPC_GROUP_POINTS T
                                     WHERE T.LOCATION_KEY = '{0}'
                                       AND T.PARAM_KEY = '{1}'
                                       AND T.STEP_NAME = '{3}'
                                       AND T.PART_TYPE = '{2}'";
                    //get query condition
                    #region
                    if (hashTable.ContainsKey("MODE_CODE"))
                    {
                        code = Convert.ToInt32(hashTable["MODE_CODE"]);
                        #region
                        if (code == 0)
                        {
                            points = Convert.ToInt32(hashTable["MODE_VALUE_POINTS"]);

                            s_tmp = string.Format(@" SELECT MAX(A.CREATE_TIME) MAX_DATE,MIN(A.CREATE_TIME) MIN_DATE FROM (
                                                    SELECT T.CREATE_TIME,ROW_NUMBER() OVER(ORDER BY T.CREATE_TIME DESC) RN
                                                      FROM SPC_GROUP_POINTS T
                                                     WHERE  T.LOCATION_KEY = '{0}'
                                                       AND T.PARAM_KEY = '{1}'
                                                       AND T.STEP_NAME = '{4}'
                                                       AND T.PART_TYPE = '{2}'
                                                       AND T.DELETED_FLAG = 0  
                                                      --ORDER BY T.CREATE_TIME DESC
                                                    ) A WHERE A.RN<={3} ", locationKey, paramKey, partType, points.ToString(), stepName);


                            //DataTable dtForDate = dbSPC.ExecuteDataSet(CommandType.Text, s_tmp).Tables[0];
                            DataTable dtForDate = db.ExecuteDataSet(CommandType.Text, s_tmp).Tables[0];
                            startDate = dtForDate.Rows[0]["MIN_DATE"].ToString();
                            endDate = dtForDate.Rows[0]["MAX_DATE"].ToString();
                        }
                        #endregion

                        #region
                        if (code == 1)
                        {
                            startDate = hashTable["MODE_VALUE_STARTDATE"].ToString();
                            endDate = hashTable["MODE_VALUE_ENDDATE"].ToString();
                        }
                        #endregion

                    }
                    #endregion
                    if (!string.IsNullOrEmpty(endDate.Trim()))
                        s_tmp = string.Format(@" AND T.CREATE_TIME BETWEEN '{0}' AND '{1}' 
                                               ", startDate, endDate);
                    else
                        s_tmp = string.Format(@" AND T.CREATE_TIME BETWEEN '{0}' AND GETDATE() 
                                                    ", startDate);

                    s_exep = string.Format(sql_points + s_tmp, locationKey, paramKey, partType, stepName);
                    //DataTable dtPoints =dbSPC.ExecuteDataSet(CommandType.Text,s_exep).Tables[0];
                    DataTable dtPoints = db.ExecuteDataSet(CommandType.Text, s_exep).Tables[0];
                    dtPoints.TableName = SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS;
                    dsReturn.Merge(dtPoints, false, MissingSchemaAction.Add);

                    s_exes = string.Format(sql_vsr + s_tmp, locationKey, paramKey, partType, stepName);
                    //DataTable dtcletc = dbSPC.ExecuteDataSet(CommandType.Text, s_exes).Tables[0];
                    DataTable dtcletc = db.ExecuteDataSet(CommandType.Text, s_exes).Tables[0];
                    dtcletc.TableName = SPC_PARAM_DATA_FIELDS.DB_FOR_STANDARD;
                    dsReturn.Merge(dtcletc, false, MissingSchemaAction.Add);

                    double v_avg = Convert.ToDouble(dtcletc.Rows[0]["V_AVG"].ToString());
                    //calculate estimate deviation    
                    p_sigma = GetPSigmaByProc(locationKey, paramKey, stepName, partType, startDate, endDate, v_avg, string.Empty, string.Empty);
                    dsReturn.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.P_SIGMA, p_sigma);

                    #endregion

                    #endregion

                    #region 查询参数规格上下线
                    if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                    {

                        #region
                        //根据参数ID，产品型号，工序类型——确定参数规格的上下线
                        string sql = string.Format(@" SELECT MAX(A.UPPER_SPEC) UPPER_SPEC,
                                                       MAX(A.LOWER_SPEC) LOWER_SPEC,
                                                       MAX(A.TARGET) TARGET,
                                                       MAX(A.UPPER_BOUNDARY) UPPER_BOUNDARY,
                                                       MAX(A.LOWER_BOUNDARY) LOWER_BOUNDARY,
                                                       '' EQUIPMENT_KEY,A.PARAM_NAME,T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                 GROUP BY A.PARAM_NAME, T.ROUTE_VER_KEY
                                                UNION ALL
                                                SELECT A.UPPER_SPEC, A.LOWER_SPEC,A.TARGET,A.UPPER_BOUNDARY,A.LOWER_BOUNDARY,
                                                       T.EQUIPMENT_KEY, A.PARAM_NAME,T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                   AND isnull(T.EQUIPMENT_KEY, '0') <> '0'
                                                 GROUP BY A.UPPER_SPEC,A.LOWER_SPEC,A.TARGET, A.UPPER_BOUNDARY, A.LOWER_BOUNDARY,A.PARAM_NAME,
                                                          T.EQUIPMENT_KEY,A.PARAM_NAME, T.ROUTE_VER_KEY",
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString(),
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString(),
                                    drControl["route_operation_name"].ToString());

                        #endregion

                        DataTable table = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                        table.TableName = EDC_POINT_PARAMS_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(table, false, MissingSchemaAction.Add);
                        dsReturn.ExtendedProperties.Add("C_TITLE", pro_type);


                    }
                    #endregion
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC SearchParamValue error:" + ex.Message);
            }
            return dsReturn;
        }
        private DataTable GetControlPlanData(string conid)
        {
            string scontrol = string.Format(@" SELECT T.*, T1.ROUTE_OPERATION_NAME
                                               FROM SPC_CONTROL_PLAN T, POR_ROUTE_OPERATION_VER T1
                                               WHERE T.STEP_KEY = T1.ROUTE_OPERATION_VER_KEY
                                               AND( T.CONTROLPLANID = '{0}'  OR T.CONTROLCODE='{0}')", conid);

            DataTable dtControlPlan = db.ExecuteDataSet(CommandType.Text, scontrol).Tables[0];
            return dtControlPlan;
        }
        public DataSet GetStandardAndPSigma(DataTable dataTable, string avg)
        {
            DataSet dsReturn = new DataSet();
            //定义查询参数
            string locationKey = string.Empty, paramKey = string.Empty, partType = string.Empty, equipmentKey = string.Empty, stepName = string.Empty, equipment_keys = string.Empty, supplier = string.Empty;
            string startDate = string.Empty, endDate = string.Empty;

            #region 查询参数值
            Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
            DataTable dtControlPlan = GetControlPlanData(hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLCODE].ToString());
            DataRow drControl = dtControlPlan.Rows[0];
            #endregion
            string controlType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
            //------------------------------------------------------------------------------------------------------------
            locationKey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString();
            paramKey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
            stepName = drControl[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString();
            partType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
            //--------------------------------------------------------------------------------------------------------------

            //read standard data
            const string sql_vsr = @"SELECT ROUND(AVG(T.V_VALUE),5) V_AVG,ROUND(AVG(T.R_VALUE),5) R_AVG,ROUND(AVG(T.S_VALUE),5) S_AVG,isnull(SUM(T.P_COUNT),0) P_DTL_COUNT,
                                             isnull(COUNT(0),0) P_A_COUNT,MIN(ROUND(T.V_VALUE, 5)) MIN_V_VALUE,MAX(ROUND(T.V_VALUE, 5)) MAX_V_VALUE,MIN(ROUND(T.S_VALUE,5)) MIN_S_VALUE,
                                             MAX(ROUND(T.S_VALUE,5)) MAX_S_VALUE,MIN(ROUND(T.R_VALUE,5)) MIN_R_VALUE,MAX(ROUND(T.R_VALUE,5)) MAX_R_VALUE
                                      FROM SPC_GROUP_POINTS T
                                      WHERE {0} ";

            string where = string.Empty;
            where = string.Format(" T.LOCATION_KEY='{0}'", locationKey);
            where += string.Format(" AND T.PARAM_KEY='{0}'", paramKey);
            where += string.Format(" AND T.STEP_NAME='{0}'", stepName);
            where += string.Format(" AND T.PART_TYPE='{0}'", partType);

            if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY))
            {
                where += string.Format(" AND T.EQUIPMENT_KEY IN ({0})", hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY].ToString());
                equipment_keys = hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY].ToString();
            }
            if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SUPPLIER))
            {
                where += string.Format(" AND T.SUPPLIER='{0}'", hashTable[SPC_PARAM_DATA_FIELDS.SUPPLIER].ToString());
                supplier = hashTable[SPC_PARAM_DATA_FIELDS.SUPPLIER].ToString();
            }
            if (hashTable.ContainsKey("MAX_DATE"))
                endDate = hashTable["MAX_DATE"].ToString();
            if (hashTable.ContainsKey("MIN_DATE"))
                startDate = hashTable["MIN_DATE"].ToString();

            where += string.Format(" AND T.CREATE_TIME BETWEEN TO_DATE('{0}','yyyy/MM/dd hh24:mi:ss') AND TO_DATE('{1}','yyyy/MM/dd hh24:mi:ss')", startDate, endDate);

            string s_exes = string.Format(sql_vsr, where);
            //DataTable dtcletc = dbSPC.ExecuteDataSet(CommandType.Text, s_exes).Tables[0];
            DataTable dtcletc = db.ExecuteDataSet(CommandType.Text, s_exes).Tables[0];
            dtcletc.Rows[0]["V_AVG"] = avg;
            dtcletc.AcceptChanges();
            dtcletc.TableName = SPC_PARAM_DATA_FIELDS.DB_FOR_STANDARD;
            dsReturn.Merge(dtcletc, false, MissingSchemaAction.Add);
            //decimal v_avg = Convert.ToDecimal(dtcletc.Rows[0]["V_AVG"].ToString());
            decimal v_avg = Convert.ToDecimal(avg);

            //calculate estimate deviation    
            //double p_sigma = GetPSigmaByProc(locationKey, paramKey, stepName, partType, startDate, endDate, v_avg, equipment_keys, supplier);

            string sql = string.Format(@"  SELECT A.PARAM_VALUE
                                          FROM SPC_PARAM_DATA A
                                         WHERE A.LOCATION_KEY = '{0}'
                                           AND A.PARAM_KEY = '{1}'
                                           AND A.STEP_NAME = '{2}'
                                           AND A.PART_TYPE = '{3}'
                                           AND A.CREATE_TIME BETWEEN
                                               TO_DATE('{4}', 'yyyy/MM/dd hh24:mi:ss') AND
                                               TO_DATE('{5}', 'yyyy/MM/dd hh24:mi:ss')",
                                         locationKey, paramKey, stepName, partType, startDate, endDate);
            where = string.Empty;
            if (!string.IsNullOrEmpty(equipment_keys))
                where += string.Format(" AND EQUIPMENT_KEY IN ({0})", equipment_keys);
            if (!string.IsNullOrEmpty(supplier))
                where += string.Format(" AND SUPPLIER='{0}'", supplier);

            //DataTable dtSigma = dbSPC.ExecuteDataSet(CommandType.Text, sql + where).Tables[0];
            DataTable dtSigma = db.ExecuteDataSet(CommandType.Text, sql + where).Tables[0];
            double p_sigma = CalculateSigma(v_avg, dtSigma);

            dsReturn.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.P_SIGMA, p_sigma);

            return dsReturn;

        }
        #region 

        /// <summary>
        /// calculate 
        /// </summary>
        /// <param name="xAvg"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-08-16 16:29:29
        private double CalculateSigma(decimal xAvg, DataTable table)
        {
            double Sigma = 0;
            decimal tempValue = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                decimal value = Convert.ToDecimal(table.Rows[i][SPC_PARAM_DATA_FIELDS.PARAM_VALUE]);
                tempValue += (value - xAvg) * (value - xAvg);
            }
            Sigma = Math.Sqrt(Convert.ToDouble(tempValue / (table.Rows.Count)));
            return Math.Round(Sigma, 5);
        }
        #endregion

        /// <summary>
        /// SPC参数，SPC空值图数据源
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// Modify by genchille.yang 2012-05-2 15:27:19 
        public DataSet SearchParamValueForMr(DataTable dataTable)
        {
            //定义返回的数据源
            DataSet dsReturn = new DataSet();
            try
            {
                string orderNumber = string.Empty;
                string pro_type = string.Empty;

                if (dataTable != null)
                {
                    #region 查询参数值
                    //把dataTable还原为HashTable 便于查询条件使用
                    Hashtable hashTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dataTable);
                    string scontrol = string.Format(@"SELECT t.*,t1.route_operation_name FROM SPC_CONTROL_PLAN t, por_route_operation_ver t1
                                                     WHERE t.step_key=t1.route_operation_ver_key
                                                     AND t.controlplanid='{0}'", hashTable[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLPLANID]);
                    DataTable dtControlPlan = db.ExecuteDataSet(CommandType.Text, scontrol).Tables[0];


                    dtControlPlan.TableName = "SPC_CONTROL_PLAN";//Q.003
                    dsReturn.Tables.Add(dtControlPlan.Copy());//Q.003

                    DataRow drControl = dtControlPlan.Rows[0];

                    string aSql = string.Empty;
                    string mSql = string.Empty;
                    string parakey = string.Empty, tSql = string.Empty, s_material = string.Empty, s_suppler = string.Empty;


                    //物料条件
                    const string materialSql = @" SELECT DISTINCT T1.MATERIAL_LOT FROM ({0}) T1  WHERE isnull(T1.MATERIAL_LOT, '0') <> '0'";
                    //供应商条件
                    const string supplerSql = @" SELECT DISTINCT T1.SUPPLIER FROM ({0}) T1 WHERE isnull(T1.SUPPLIER, '0') <> '0'";
                    //单点极差
                    const string mrSql = @"SELECT * FROM ( SELECT ROUND(T.PARAM_VALUE, 4) PARAM_VALUE,
                                           T.COL_KEY,
                                           T.EDIT_FLAG,
                                           T.EDIT_REASON,
                                           T.EDC_INS_KEY,
                                           TO_CHAR(T.CREATE_TIME, 'MM/dd hh24:mi:ss') CREATE_TIME,
                                           --T.CREATE_TIME CREATE_TIME,
                                           T.SUPPLIER,
                                           T.LOT_NUMBER,
                                           T.EQUIPMENT_KEY,
                                           T.MATERIAL_LOT,
                                           T.SHIFT_VALUE,
                                           T.ROUTE_KEY ROUTE_ROUTE_VER_KEY
                                      FROM SPC_PARAM_DATA T
                                     WHERE T.DELETED_FLAG = 0
                                       AND T.PARAM_KEY = '{0}'";

                    #region 查询条件
                    string controlType = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_CONTROLTYPE].ToString();
                    parakey = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString();
                    //工厂车间
                    tSql += string.Format(" AND T.LOCATION_KEY='{0}'", drControl[SPC_CONTROL_PLAN_FIELD.FIELD_WERKS].ToString());
                    //工序名称
                    tSql += string.Format(" AND  T.STEP_NAME = '{0}'", drControl["route_operation_name"].ToString());
                    //产品类型
                    tSql += string.Format(" AND T.PART_TYPE='{0}'", drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString());
                    //获得返回的产品类型
                    pro_type = drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString();
                    //物料批次/供应商批次
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.MATERIAL_LOT))
                        tSql += string.Format("   AND T.MATERIAL_LOT='{0}'", hashTable[SPC_PARAM_DATA_FIELDS.MATERIAL_LOT]);
                    //供应商
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SUPPLIER))
                        tSql += string.Format("   AND T.SUPPLIER LIKE '{0}'", hashTable[SPC_PARAM_DATA_FIELDS.SUPPLIER]);
                    //班别
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.SHIFT_VALUE))
                        tSql += string.Format("   AND T.SHIFT_VALUE='{0}'", hashTable[SPC_PARAM_DATA_FIELDS.SHIFT_VALUE]);
                    //设备
                    if (hashTable.ContainsKey(SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY))
                        tSql += string.Format(" AND T.EQUIPMENT_KEY IN ({0})", hashTable[SPC_PARAM_DATA_FIELDS.EQUIPMENT_KEY]);
                    #endregion

                    tSql = string.Format(mrSql + tSql, parakey);//参数                  

                    if (controlType.ToUpper() == "XBAR-MR")
                    {
                        if (hashTable.ContainsKey("MODE_CODE"))
                        {
                            int code = Convert.ToInt32(hashTable["MODE_CODE"]);
                            //string value = hashTable["MODE_VALUE"].ToString();
                            //按时间查询
                            if (code == 1)
                            {
                                string _startDate = hashTable["MODE_VALUE_STARTDATE"].ToString();
                                string _endDate = hashTable["MODE_VALUE_ENDDATE"].ToString();
                                tSql += string.Format(@"  AND T.CREATE_TIME BETWEEN TO_DATE('{0}','yyyy-MM-DD hh24:mi:ss') 
                                                        AND TO_DATE('{1}','yyyy-MM-DD hh24:mi:ss')) T0 ORDER BY T0.CREATE_TIME ASC", _startDate, _endDate);

                            }
                            //按点查询
                            if (code == 0)
                            {
                                int points = Convert.ToInt32(hashTable["MODE_VALUE_POINTS"]);

                                tSql += string.Format(" ORDER BY T.CREATE_TIME DESC ) T0 WHERE ROWNUM<={0} ORDER BY T0.CREATE_TIME ASC ", points);
                            }

                        }
                        s_material = string.Format(materialSql, tSql);
                        s_suppler = string.Format(supplerSql, tSql);

                        DataTable dtSPC = db.ExecuteDataSet(CommandType.Text, tSql).Tables[0];
                        dtSPC.TableName = "DATA_SPC";
                        dsReturn.Merge(dtSPC, false, MissingSchemaAction.Add);

                        DataTable dtMaterial = db.ExecuteDataSet(CommandType.Text, s_material).Tables[0];
                        dtMaterial.TableName = "Material";
                        DataTable dtSupplier = db.ExecuteDataSet(CommandType.Text, s_suppler).Tables[0];
                        dtSupplier.TableName = "Supplier";

                        dsReturn.Merge(dtMaterial, false, MissingSchemaAction.Add);
                        dsReturn.Merge(dtSupplier, false, MissingSchemaAction.Add);
                    }
                    #endregion

                    #region 查询参数规格上下线
                    if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                    {

                        #region
                        //                        //根据参数ID，产品型号，工序类型——确定参数规格的上下线
                        string sql = string.Format(@" SELECT MAX(A.UPPER_SPEC) UPPER_SPEC,
                                                       MAX(A.LOWER_SPEC) LOWER_SPEC,
                                                       MAX(A.TARGET) TARGET,
                                                       MAX(A.UPPER_BOUNDARY) UPPER_BOUNDARY,
                                                       MAX(A.LOWER_BOUNDARY) LOWER_BOUNDARY,
                                                       '' EQUIPMENT_KEY,
                                                       A.PARAM_NAME,
                                                       T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                 GROUP BY A.PARAM_NAME, T.ROUTE_VER_KEY
                                                UNION ALL
                                                SELECT A.UPPER_SPEC,
                                                       A.LOWER_SPEC,
                                                       A.TARGET,
                                                       A.UPPER_BOUNDARY,
                                                       A.LOWER_BOUNDARY,
                                                       T.EQUIPMENT_KEY,
                                                       A.PARAM_NAME,
                                                       T.ROUTE_VER_KEY
                                                  FROM EDC_POINT T, EDC_POINT_PARAMS A
                                                 WHERE T.ROW_KEY = A.EDC_POINT_ROWKEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND T.PART_TYPE = '{1}'
                                                   AND T.OPERATION_NAME LIKE '{2}%'
                                                   AND isnull(T.EQUIPMENT_KEY, '0') <> '0'
                                                 GROUP BY A.UPPER_SPEC,
                                                          A.LOWER_SPEC,
                                                          A.TARGET,
                                                          A.UPPER_BOUNDARY,
                                                          A.LOWER_BOUNDARY,
                                                          A.PARAM_NAME,
                                                          T.EQUIPMENT_KEY,
                                                          A.PARAM_NAME,
                                                          T.ROUTE_VER_KEY",
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PARAMENTID].ToString(),
                                     drControl[SPC_CONTROL_PLAN_FIELD.FIELD_PRODUCTCODE].ToString(),
                                    drControl["route_operation_name"].ToString());

                        #endregion

                        DataTable table = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
                        table.TableName = EDC_POINT_PARAMS_FIELDS.DATABASE_TABLE_NAME;
                        dsReturn.Merge(table, false, MissingSchemaAction.Add);
                        dsReturn.ExtendedProperties.Add("C_TITLE", pro_type);

                    }
                    #endregion
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SPC SearchParamValue error:" + ex.Message);
            }
            return dsReturn;
        }
    }
}

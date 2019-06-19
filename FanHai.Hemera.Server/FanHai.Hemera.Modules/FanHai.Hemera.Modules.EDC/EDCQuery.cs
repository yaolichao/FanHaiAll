using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Collections;

namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 数据采集查询类，用于导出采集得到的数据值。
    /// </summary>
    public class EDCQuery : AbstractEngine, IEDCQuery
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
        public EDCQuery()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 查询抽检点数据。
        /// </summary>
        /// <returns>包含抽检点数据的数据集对象。</returns>
        public DataSet SearchEdcPoint()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT T.ROW_KEY, T.PART_TYPE, T.OPERATION_NAME,
	                            (
	                             SELECT ROUTE_OPERATION_VER_KEY
	                             FROM POR_ROUTE_OPERATION_VER
	                             WHERE ROUTE_OPERATION_NAME = T.OPERATION_NAME
	                             AND OPERATION_STATUS = 1
	                             AND OPERATION_VERSION =(SELECT MAX (OPERATION_VERSION)
							                             FROM POR_ROUTE_OPERATION_VER
							                             WHERE ROUTE_OPERATION_NAME = T.OPERATION_NAME)
	                            ) OPERATION_KEY,
	                            ISNULL(T.POINT_STATUS, 0) AS POINT_STATUS,
	                            CASE WHEN ISNULL(T.POINT_STATUS, 0)=0 THEN '未激活'
		                             WHEN ISNULL(T.POINT_STATUS, 0)=1 THEN '已激活'
		                             WHEN ISNULL(T.POINT_STATUS, 0)=2 THEN '存档'
                                END AS POINT_STATE_DESCRIPTION,
	                            T.ACTION_NAME, 
	                            (SELECT EM.EDC_NAME
                                FROM EDC_MAIN EM
                                WHERE T.EDC_KEY = EM.EDC_KEY) AS EDC_NAME,
                                T.EDC_KEY,
	                            (SELECT ES.SP_NAME
	                            FROM EDC_SP ES
	                            WHERE T.SP_KEY = ES.SP_KEY) AS SP_NAME, 
	                            T.SP_KEY, 
	                            T.GROUP_KEY,
	                            (SELECT ROUTE_NAME
	                            FROM POR_ROUTE_ROUTE_VER PR
	                            WHERE T.ROUTE_VER_KEY = PR.ROUTE_ROUTE_VER_KEY) ROUTE_NAME,
	                            T.ROUTE_VER_KEY, 
	                            G.EQUIPMENT_NAME, 
	                            G.EQUIPMENT_KEY, 
	                            T.EDIT_DESC
                            FROM EDC_POINT T
                            INNER JOIN (SELECT  GROUP_KEY,
				                            dbo.ConcatEdcPointEquipmentName(GROUP_KEY) EQUIPMENT_NAME,
				                            dbo.ConcatEdcPointEquipmentKey(GROUP_KEY) EQUIPMENT_KEY,
				                            MIN (ROW_KEY) ROW_KEY
			                            FROM EDC_POINT A
			                            GROUP BY GROUP_KEY) G ON T.ROW_KEY = G.ROW_KEY AND T.GROUP_KEY = G.GROUP_KEY                                       
                            WHERE T.POINT_STATUS = 1
                            ORDER BY OPERATION_NAME ,PART_TYPE,EDC_NAME";
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
        /// 根据车间名称查询设备。
        /// </summary>
        /// <param name="strFactoryRoom">车间名称。</param>
        /// <returns>包含设备名称的数据集对象。</returns>
        public DataSet SearchEMS(string strFactoryRoom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Format(@"SELECT EQUIPMENT_KEY,EQUIPMENT_NAME 
                                                    FROM EMS_EQUIPMENTS
                                                    WHERE LOCATION_KEY IN (SELECT AREA_KEY
                                                                          FROM V_LOCATION
                                                                          WHERE ROOM_NAME = '{0}')",
                                                    strFactoryRoom.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchEMS Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据条件查询设备。
        /// </summary>
        /// <param name="strFactoryRoom">车间名称。</param>
        /// <param name="groupKey">抽检点分组。</param>
        /// <param name="equipmentKey">设备主键，使用逗号分隔：设备1,设备2...。</param>
        /// <returns>包含设备名称的数据集对象。</returns>
        public DataSet SearchEMS(string strFactoryRoom, string groupKey, string equipmentKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                string sql=string.Empty;
                if (equipmentKey.Equals(string.Empty))//捞取当前车间这个站点的设备
                {
                    sql = @"SELECT EQUIPMENT_KEY, EQUIPMENT_NAME
                            FROM EMS_EQUIPMENTS
                            WHERE LOCATION_KEY IN (SELECT AREA_KEY
					                               FROM V_LOCATION
					                               WHERE ROOM_NAME = '{0}')
                            AND EQUIPMENT_KEY IN (
					                              SELECT EQUIPMENT_KEY
					                              FROM EMS_OPERATION_EQUIPMENT
					                              WHERE OPERATION_KEY =(SELECT (SELECT TOP 1 ROUTE_OPERATION_VER_KEY 
												                                FROM POR_ROUTE_OPERATION_VER T2
												                                WHERE T1.OPERATION_NAME =T2.ROUTE_OPERATION_NAME) OPERATION_KEY
										                                FROM EDC_POINT T1
										                                WHERE GROUP_KEY ='{1}')
					                             )";  
                    sqlCommand =string.Format(sql,strFactoryRoom.PreventSQLInjection(), 
                                                  groupKey.PreventSQLInjection());

                }
                else //根据车间和equipmentKey捞取
                {
                    string[] strEquipmentKey= equipmentKey.Split(',');
                    string strcon = string.Empty;
                    for (int i = 0; i < strEquipmentKey.Length; i++)
                    {
                        strcon = strcon + "'" + strEquipmentKey[i].PreventSQLInjection() + "',";
                    }
                    strcon = strcon.Substring(0, strcon.Length-1);
                    sql = @"SELECT EQUIPMENT_KEY, EQUIPMENT_NAME
                            FROM EMS_EQUIPMENTS
                            WHERE LOCATION_KEY IN (SELECT AREA_KEY
                                                   FROM V_LOCATION
                                                   WHERE ROOM_NAME = '{0}')
                            AND EQUIPMENT_KEY IN ({1})";
                    sqlCommand = string.Format(sql, strFactoryRoom.PreventSQLInjection(), strcon);
                }

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchEMS Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        ///  查询采集参数。
        /// </summary>
        /// <returns>包含采集参数的数据集对象。</returns>
        public DataSet SearchParam()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT PARAM_KEY,PARAM_NAME FROM BASE_PARAMETER";
                dsReturn = db.ExecuteDataSet(CommandType.Text,sqlCommand);
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
        /// 根据采集分组主键获取基础参数查询采集参数数据,
        /// </summary>
        /// <param name="edcKey">采集分组主键。</param>
        /// <returns>包含采集参数数据的数据集对象。</returns>
        public DataSet SearchParam( string edcKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT PARAM_KEY, PARAM_NAME
                               FROM BASE_PARAMETER
                               WHERE PARAM_KEY IN (SELECT PARAM_KEY
                                                   FROM EDC_MAIN_PARAM
                                                   WHERE EDC_KEY ='{0}')";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sqlCommand,edcKey.PreventSQLInjection()));
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
        /// 根据条件查询采集得到的数据。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------
        /// {ROW_KEY}
        /// {START_DATE}
        /// {END_DATE}
        /// {EQUIPMENT_NAME}
        /// {PARAM_NAME}
        /// ------------------------------
        /// </param>
        /// <returns>包含采集得到的数据的数据集对象。</returns>
        public DataSet EDCValueQuery(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            string rowKey=string.Empty;
            string startDate=string.Empty;
            string endDate=string.Empty;
            try
            {
                string sqlCommand = string.Empty;
                sqlCommand = @"SELECT  TT1.EDIT_TIME EDIT_TIME,TT1.STEP_NAME,TT3.EQUIPMENT_NAME,TT2.PARAM_NAME, TT1.SP_UNIT_SEQ,TT1.PARAM_VALUE                           
                            FROM 
                            (	
                              SELECT T1.*, T2.STEP_NAME, T2.EDC_NAME, T2.EQUIPMENT_KEY
                              FROM EDC_COLLECTION_DATA T1
                              JOIN (SELECT *
	                                FROM EDC_MAIN_INS
	                                WHERE '{0}' < EDIT_TIME AND EDIT_TIME< '{1}'
	                                AND EDC_POINT_KEY IN (
						                                  SELECT ROW_KEY 
						                                  FROM EDC_POINT  
						                                  WHERE GROUP_KEY=(SELECT GROUP_KEY FROM EDC_POINT WHERE ROW_KEY='{2}')
						                                  )
	                                ) T2 ON T1.EDC_INS_KEY = T2.EDC_INS_KEY
                            ) TT1
                            JOIN BASE_PARAMETER TT2 ON TT1.PARAM_KEY = TT2.PARAM_KEY
                            JOIN EMS_EQUIPMENTS TT3 ON TT3.EQUIPMENT_KEY = TT1.EQUIPMENT_KEY
                            WHERE 1=1 ";
                if (dtParams != null)
                {
                    Hashtable htParams = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtParams);
                    if (htParams.Contains("ROW_KEY"))
                    {
                        rowKey = Convert.ToString(htParams["ROW_KEY"]);
                    }
                    if (htParams.ContainsKey("START_DATE"))
                    {
                        startDate = Convert.ToString(htParams["START_DATE"]);
                    }
                    if (htParams.ContainsKey("END_DATE"))
                    {
                        endDate = Convert.ToString(htParams["END_DATE"]);
                    }
                    if (htParams.ContainsKey("EQUIPMENT_NAME"))
                    {
                        string equipmentName = Convert.ToString(htParams["EQUIPMENT_NAME"]);
                        sqlCommand = sqlCommand + "AND TT3.EQUIPMENT_NAME='" + equipmentName.PreventSQLInjection() + "'  ";
                    }
                    if (htParams.ContainsKey("PARAM_NAME"))
                    {
                        string paramName = Convert.ToString(htParams["PARAM_NAME"]);
                        sqlCommand = sqlCommand + "AND TT2.PARAM_NAME='" + paramName.PreventSQLInjection() + "'  ";
                    }
                }
                sqlCommand=sqlCommand+"  ORDER BY TT1.EDIT_TIME DESC, TT1.EDC_INS_KEY, TT1.PARAM_KEY, TT1.SP_UNIT_SEQ ";
                sqlCommand=string.Format(sqlCommand,
                                        startDate.PreventSQLInjection(),
                                        endDate.PreventSQLInjection(),
                                        rowKey.PreventSQLInjection());
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommand);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("EDCValueQuery Error: " + ex.Message);
            }
            return dsReturn;
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
///EquipmentAndOEE 的摘要说明
/// </summary>
public class EquipmentAndOEE
{
    private static Database _db = DatabaseFactory.CreateDatabase();

    public EquipmentAndOEE()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// OEE 报表数据，包括主表和明细表资料
    /// </summary>
    /// <param name="StartDate">时间区间-开始日期</param>
    /// <param name="EndDate">时间区间-结束日期</param>
    /// <param name="equipmentKey">设备号（单个设备ID/多个设备ID，之间用逗号分割）</param>
    /// <param name="locationkey">车间</param>
    /// <returns>设备OEE主表,明细表集合</returns>
    public DataSet GetOEE_MainDetailbak(string StartDate, string EndDate, string equipmentKey, string locationkey)
    {
        DataSet dsReturn = new DataSet();
        string sql = string.Empty;

        string strFilt = " 1=1 ";
        if (!string.IsNullOrEmpty(locationkey))
            strFilt += string.Format(" AND A.LOCATION_KEY='{0}'", locationkey);
        if (!string.IsNullOrEmpty(equipmentKey))
            strFilt += string.Format(" AND A.EQUIPMENT_KEY='{0}'", equipmentKey);
        if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            strFilt += string.Format(@" AND A.OPE_DATE BETWEEN '{0}' AND '{1}'", StartDate, EndDate);

        sql = string.Format(@"SELECT ROWNUM ,tb.* FROM (
                            SELECT T1.EQUIPMENT_NAME, 
                                   T1.DESCRIPTION,
                                   T.EQUIPMENT_KEY,
                                   T.WRKALLCELLS2 WRKALLCELLS, 
                                   ROUND(T.WRKALLCELLS /
                                   ((TO_DATE('{1}', 'yyyy-MM-dd hh24:mi:ss') -
                                   TO_DATE('{0}', 'yyyy-MM-dd hh24:mi:ss')) * 24),4) WPH_REL, --WPH实际(片)
                                   T1.EQUIPMENT_WPH WPH_TARGET, --WPH目标(片)
                                   TO_CHAR(ROUND(T.WRKALLCELLS / ((TO_DATE('{1}',
                                                                           'yyyy-MM-dd hh24:mi:ss') -
                                                 TO_DATE('{0}',
                                                                           'yyyy-MM-dd hh24:mi:ss')) * 24 *
                                                 T1.EQUIPMENT_WPH),
                                                 4) * 100) || '%' EFFI, 
                                   ROUND((T.V3 / T.V1) *
                                         (T.WRKALLCELLS * T1.EQUIPMENT_TRACT_TIME / T.V2) *
                                         (1 - T.WRKALLNGCELLS / T.WRKALLCELLS),
                                         2) EQUIPMENT_AVTIME_TARGET, --AvTime设备目标
                                   ROUND((T.V2 / T.V1) *
                                         (T.WRKALLCELLS * T1.EQUIPMENT_TRACT_TIME / T.V2) *
                                         (1 - T.WRKALLNGCELLS / T.WRKALLCELLS),
                                         2) WRK_OEE, --生产OEE
                                   T1.EQUIPMENT_AV_TIME AVTIME_TARGET,
                                   TO_CHAR(ROUND(T.RUN / T.ALL_TIME, 4) * 100) || '%' P_RUN, --RUN状态的百分比
                                   TO_CHAR(ROUND(T.LOST / T.ALL_TIME, 4) * 100) || '%' P_LOST, --LOST状态的百分比
                                   TO_CHAR(ROUND(T.TEST / T.ALL_TIME, 4) * 100) || '%' P_TEST, --LOST状态的百分比
                                   TO_CHAR(ROUND(T.OTHER / T.ALL_TIME, 4) * 100) || '%' P_OTHER, --OTHER状态的百分比
                                   TO_CHAR(ROUND(T.DOWN / T.ALL_TIME, 4) * 100) || '%' P_DOWN, --DOWN状态的百分比
                                   TO_CHAR(ROUND(T.PM / T.ALL_TIME, 4) * 100) || '%' P_PM, --PM状态的百分比
                                   TO_CHAR(ROUND(T.MON / T.ALL_TIME, 4) * 100) || '%' P_MON, --MON状态的百分比
                                   TO_CHAR(ROUND(T.T_DOWN / T.ALL_TIME, 4) * 100) || '%' P_T_DOWN, --T_DOWN状态的百分比      
                                   ROUND(T.DOWN * 60 / T.STATE_CHG_TIMES, 2) MTTR, --MTTR 以分钟为单位
                                   ROUND(T.V2 / T.STATE_CHG_TIMES, 2) MTBF --MTBF以小时为单位
                              FROM (SELECT B.EQUIPMENT_KEY,
                                           CASE
                                             WHEN B.WRKALLCELLS = 0 THEN
                                              1
                                             ELSE
                                              B.WRKALLCELLS
                                           END WRKALLCELLS,
                                           B.WRKALLCELLS  WRKALLCELLS2,
                                           B.WRKALLNGCELLS,
                                           CASE
                                             WHEN B.STATE_CHG_TIMES = 0 THEN
                                              1
                                             ELSE
                                              B.STATE_CHG_TIMES
                                           END STATE_CHG_TIMES,
                                           B.RUN,
                                           B.LOST,
                                           B.DOWN,
                                           B.PM,
                                           B.TEST,
                                           B.OTHER,
                                           B.MON,
                                           B.T_DOWN,
                                           CASE
                                             WHEN (B.RUN + B.LOST + B.DOWN + B.PM + B.TEST + B.MON +
                                                  B.T_DOWN) = 0 THEN
                                              1
                                             ELSE
                                              B.RUN + B.LOST + B.DOWN + B.PM + B.TEST + B.MON + B.T_DOWN
                                           END V1,
                                           CASE
                                             WHEN (B.LOST + B.RUN + B.TEST) = 0 THEN
                                              1
                                             ELSE
                                              B.LOST + B.RUN + B.TEST
                                           END V2,
                                           CASE
                                             WHEN (B.RUN + B.LOST + B.TEST + B.MON + B.T_DOWN) = 0 THEN
                                              1
                                             ELSE
                                              B.RUN + B.LOST + B.TEST + B.MON + B.T_DOWN
                                           END V3,
                                           CASE
                                             WHEN (B.RUN + B.LOST + B.DOWN + B.PM + B.TEST + B.OTHER +
                                                  B.MON + B.T_DOWN) = 0 THEN
                                              1
                                             ELSE
                                              B.RUN + B.LOST + B.DOWN + B.PM + B.TEST + B.OTHER + B.MON +
                                              B.T_DOWN
                                           END ALL_TIME
                                      FROM (SELECT A.EQUIPMENT_KEY,
                                                   ISNULL(SUM(A.WRKALLCELLS), 0) WRKALLCELLS,
                                                   ISNULL(SUM(A.WRKALLNGCELLS), 0) WRKALLNGCELLS,
                                                   ISNULL(SUM(A.STATE_CHG_TIMES), 0) STATE_CHG_TIMES,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'RUN', A.STATE_INTERVAL)),
                                                       0) RUN,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'LOST', A.STATE_INTERVAL)),
                                                       0) LOST,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'DOWN', A.STATE_INTERVAL)),
                                                       0) DOWN,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'PM', A.STATE_INTERVAL)),
                                                       0) PM,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'TEST', A.STATE_INTERVAL)),
                                                       0) TEST,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'OTHER', A.STATE_INTERVAL)),
                                                       0) OTHER,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE, 'MON', A.STATE_INTERVAL)),
                                                       0) MON,
                                                   ISNULL(SUM(DECODE(A.STATE_TYPE,
                                                                  'T_DOWN',
                                                                  A.STATE_INTERVAL)),
                                                       0) T_DOWN
                                              FROM RPT_EMS_OEE A
                                             WHERE {2}
                                             GROUP BY A.EQUIPMENT_KEY) B) T,
                                   EMS_EQUIPMENTS T1
                             WHERE T.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                               AND ISNULL(T1.EQUIPMENT_AV_TIME, 0) > 0
                               AND ISNULL(T1.EQUIPMENT_WPH, 0) > 0
                               AND ISNULL(T1.EQUIPMENT_TRACT_TIME, 0) > 0
                            ORDER BY   T1.EQUIPMENT_NAME 
                            ) tb", StartDate, EndDate, strFilt);

        DataTable dt01 = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        DataTable dtMain = dt01.Copy();
        dtMain.TableName = "dtMain";
        dsReturn.Tables.Add(dtMain);

        sql = string.Format(@"SELECT ROWNUM, B.EQUIPMENT_KEY, TO_CHAR(ROUND((B.RUN/B.ALLTIME)*100,2))||'%' RUN,
                          TO_CHAR(ROUND((B.LOST/B.ALLTIME)*100,2))||'%' LOST,
                          TO_CHAR(ROUND((B.T_MD/B.ALLTIME)*100,2))||'%' T_MD,
                          TO_CHAR(ROUND((B.W_MF/B.ALLTIME)*100,2))||'%' W_MF,
                          TO_CHAR(ROUND((B.DOWN/B.ALLTIME)*100,2))||'%' DOWN,
                          TO_CHAR(ROUND((B.CIMD/B.ALLTIME)*100,2))||'%' CIMD,
                          TO_CHAR(ROUND((B.W_EN/B.ALLTIME)*100,2))||'%' W_EN,
                          TO_CHAR(ROUND((B.FACD/B.ALLTIME)*100,2))||'%' FACD,
                          TO_CHAR(ROUND((B.PM/B.ALLTIME)*100,2))||'%' PM,
                          TO_CHAR(ROUND((B.SETUP/B.ALLTIME)*100,2))||'%' SETUP,
                          TO_CHAR(ROUND((B.TEST/B.ALLTIME)*100,2))||'%' TEST,
                          TO_CHAR(ROUND((B.OFF/B.ALLTIME)*100,2))||'%' OFF,
                          TO_CHAR(ROUND((B.P_DOWN/B.ALLTIME)*100,2))||'%' P_DOWN,
                          TO_CHAR(ROUND((B.MON/B.ALLTIME)*100,2))||'%' MON,
                          TO_CHAR(ROUND((B.T_DOWN/B.ALLTIME)*100,2))||'%' T_DOWN
                           FROM (                                     
                          SELECT T.EQUIPMENT_KEY, T.RUN,T.LOST,T.T_MD,T.W_MF,T.DOWN,T.CIMD,T.W_EN,T.FACD,T.PM,T.SETUP,T.TEST,T.OFF,T.P_DOWN,T.MON,T.T_DOWN,
                         CASE WHEN (T.RUN+T.LOST+T.T_MD+T.W_MF+T.DOWN+T.CIMD+T.W_EN+T.FACD+T.PM+T.SETUP+T.TEST+T.OFF+T.P_DOWN+T.MON+T.T_DOWN)=0 
                         THEN 1 ELSE
                           T.RUN+T.LOST+T.T_MD+T.W_MF+T.DOWN+T.CIMD+T.W_EN+T.FACD+T.PM+T.SETUP+T.TEST+T.OFF+T.P_DOWN+T.MON+T.T_DOWN
                           END  
                          ALLTIME
                           FROM (        
                          SELECT A.equipment_key,
                          ISNULL(SUM(DECODE(A.state_name,'RUN',A.STATE_INTERVAL)),0) RUN,
                          ISNULL(SUM(DECODE(A.state_name,'LOST',A.STATE_INTERVAL)),0) LOST,
                          ISNULL(SUM(DECODE(A.state_name,'T_MD',A.STATE_INTERVAL)),0) T_MD,
                          ISNULL(SUM(DECODE(A.state_name,'W_MF',A.STATE_INTERVAL)),0) W_MF,
                          ISNULL(SUM(DECODE(A.state_name,'DOWN',A.STATE_INTERVAL)),0) DOWN,
                          ISNULL(SUM(DECODE(A.state_name,'CIMD',A.STATE_INTERVAL)),0) CIMD,
                          ISNULL(SUM(DECODE(A.state_name,'W_EN',A.STATE_INTERVAL)),0) W_EN,
                          ISNULL(SUM(DECODE(A.state_name,'FACD',A.STATE_INTERVAL)),0) FACD,
                          ISNULL(SUM(DECODE(A.state_name,'PM',A.STATE_INTERVAL)),0) PM,
                          ISNULL(SUM(DECODE(A.state_name,'SETUP',A.STATE_INTERVAL)),0) SETUP,
                          ISNULL(SUM(DECODE(A.state_name,'TEST',A.STATE_INTERVAL)),0) TEST,
                          ISNULL(SUM(DECODE(A.state_name,'OFF',A.STATE_INTERVAL)),0) OFF,
                          ISNULL(SUM(DECODE(A.state_name,'P_DOWN',A.STATE_INTERVAL)),0) P_DOWN,
                          ISNULL(SUM(DECODE(A.state_name,'MON',A.STATE_INTERVAL)),0) MON,
                          ISNULL(SUM(DECODE(A.state_name,'T_DOWN',A.STATE_INTERVAL)),0) T_DOWN
                           FROM rpt_ems_oee A
                          WHERE  {0}
                          GROUP BY A.EQUIPMENT_KEY            
                          ) T                                    
                          )B ORDER BY B.EQUIPMENT_KEY ASC ", strFilt);

        DataTable dt02 = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        DataTable dtDetail = dt02.Copy();
        dtDetail.TableName = "dtDetail";
        dsReturn.Tables.Add(dtDetail);
        //DataRelation drelation = new DataRelation("MAST_DETAIL", dtMain.Columns["EQUIPMENT_KEY"], dtDetail.Columns["EQUIPMENT_KEY"]);
        // dsReturn.Relations.Add(drelation);
        //dsReturn = CommFunction.ExecuteDataSet(strArr, mesConn);
        return dsReturn;
    }

    /// <summary>
    /// OEE 报表数据，包括主表和明细表资料
    /// </summary>
    /// <param name="StartDate">时间区间-开始日期</param>
    /// <param name="EndDate">时间区间-结束日期</param>
    /// <param name="equipmentKey">设备号（单个设备ID/多个设备ID，之间用逗号分割）</param>
    /// <param name="locationkey">车间</param>
    /// <returns>设备OEE主表,明细表集合</returns>
    /// Owner by genchille.yang 2012-04-11 13:20:11
    /// Modify by genchille.yang 2012-07-05 14:04:56
    public DataTable GetOEE_MainDetail(string StartDate, string EndDate, string equipmentKey, string locationkey)
    {
        int v_allQty = 0, v_scrapQty = 0, v_defectQty = 0, v_splitQty = 0;
        //获得设备数据
        #region
        string f = " 1=1";
        if (!string.IsNullOrEmpty(equipmentKey.Trim()))
            f += string.Format(" AND T1.EQUIPMENT_KEY='{0}'", equipmentKey);
        //乔永明修改20120925 注释
        string sql = string.Format(@" SELECT ROW_NUMBER() over(order by EQUIPMENT_NO) as  ROWNUM,
                                        EQUIPMENT_KEY,EQUIPMENT_NAME,DESCRIPTION,WPH_TARGET,AVTIME_TARGET,EQUIPMENT_TRACT_TIME,EQUIPMENT_NO FROM (
                                        SELECT DISTINCT T1.EQUIPMENT_KEY, T1.EQUIPMENT_NAME,
                                            T1.DESCRIPTION,
                                            T1.EQUIPMENT_WPH WPH_TARGET,
                                            T1.EQUIPMENT_AV_TIME AVTIME_TARGET,
                                            T1.EQUIPMENT_TRACT_TIME,
                                            T4.ROUTE_STEP_SEQ,T1.EQUIPMENT_NO
                                        FROM EMS_EQUIPMENTS T1 INNER JOIN FMM_LOCATION_RET T2 on  T1.LOCATION_KEY = T2.LOCATION_KEY
                                        INNER JOIN  EMS_OPERATION_EQUIPMENT T3 ON T1.EQUIPMENT_KEY = T3.EQUIPMENT_KEY
                                        INNER JOIN
                                          (SELECT A.ROUTE_OPERATION_VER_KEY,MAX(A.ROUTE_STEP_SEQ) ROUTE_STEP_SEQ FROM  POR_ROUTE_STEP A 
                                          GROUP BY A.ROUTE_OPERATION_VER_KEY )T4 
                                          ON T3.OPERATION_KEY = T4.ROUTE_OPERATION_VER_KEY
                                        WHERE {1}
                                        AND T2.LOCATION_LEVEL = 9
                                        AND T2.PARENT_LOC_LEVEL = 5
                                        AND T1.EQUIPMENT_WPH > 0
                                        AND T1.EQUIPMENT_AV_TIME > 0
                                        AND T1.EQUIPMENT_TRACT_TIME > 0
                                        AND T2.PARENT_LOC_KEY = '{0}'
                                        GROUP BY T1.EQUIPMENT_KEY,
                                        T1.EQUIPMENT_NAME,T1.DESCRIPTION,T1.EQUIPMENT_WPH ,T1.EQUIPMENT_AV_TIME ,
                                        T1.EQUIPMENT_TRACT_TIME, T4.ROUTE_STEP_SEQ,T1.EQUIPMENT_NO ) T ORDER BY EQUIPMENT_NO, ROUTE_STEP_SEQ,DESCRIPTION 
                                        ", locationkey, f);
        #endregion
        DataTable dtEquipment02 = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        DataTable dtEquipment = dtEquipment02.Clone();
        DataRow[] drs = dtEquipment02.Select(string.Format("EQUIPMENT_KEY<>'null'"), "EQUIPMENT_NO");
        int i = 1;
        foreach (DataRow dr in drs)
        {
            DataRow[] drs01 = dtEquipment.Select(string.Format("EQUIPMENT_KEY='{0}'", dr["EQUIPMENT_KEY"].ToString()));
            if (drs01 == null || drs01.Length == 0)
            {
                dr["ROWNUM"] = i;
                dtEquipment.ImportRow(dr);
                i++;
            }
        }
        dtEquipment.AcceptChanges();

        //获得设备的加工量
        #region
        string f0 = " 1=1 ";
        if (!string.IsNullOrEmpty(equipmentKey))
            f0 += string.Format(" AND T.EQUIPMENT_KEY='{0}'", equipmentKey);

        string sqlallqty = string.Format(@"  SELECT SUM(T.QUANTITY) QUANTITY1,COUNT(*) AS QUANTITY,T.EQUIPMENT_KEY
                                             FROM EMS_LOT_EQUIPMENT T, EMS_EQUIPMENTS T1, FMM_LOCATION_RET T2
                                            WHERE {3} AND T.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                                              AND T1.LOCATION_KEY = T2.LOCATION_KEY
                                              AND T2.LOCATION_LEVEL = 9
                                              AND T2.PARENT_LOC_LEVEL = 5
                                              ----AND TO_CHAR(T.START_TIMESTAMP, 'yyyy-MM-dd hh24:mi:ss') BETWEEN
                                                  AND T.START_TIMESTAMP BETWEEN 
                                                  '{0}' AND '{1}'
                                              AND T2.PARENT_LOC_KEY = '{2}'
                                            GROUP BY T.EQUIPMENT_KEY
                                    ", StartDate, EndDate, locationkey, f0);
        #endregion
        DataTable dt_allQty = _db.ExecuteDataSet(CommandType.Text, sqlallqty).Tables[0];

        //获得设备的不良数量+碎片数量
        #region
        string f1 = "1=1 ";
        if (!string.IsNullOrEmpty(equipmentKey))
            f1 += string.Format(@" AND T3.EQUIPMENT_KEY='{0}'", equipmentKey);

        string sqlscrapQty = string.Format(@" SELECT ISNULL(SUM(A.QUANTITY), 0) QUANTITY, A.EQUIPMENT_KEY
                                      FROM (SELECT SUM(T.DEFECT_QUANTITY) QUANTITY, T3.EQUIPMENT_KEY
                                              FROM WIP_DEFECT       T,
                                                  ----- WIP_TRANSACTION@LNKTOREL  T3,
                                                   WIP_TRANSACTION  T3,
                                                   EMS_EQUIPMENTS   T1,
                                                   FMM_LOCATION_RET T2
                                             WHERE {3} AND T.TRANSACTION_KEY = T3.TRANSACTION_KEY
                                               AND T3.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                                               AND T1.LOCATION_KEY = T2.LOCATION_KEY
                                               AND T2.LOCATION_LEVEL = 9
                                               AND T2.PARENT_LOC_LEVEL = 5
                                               AND T2.PARENT_LOC_KEY = '{2}'
                                              ---- AND TO_CHAR(T.EDIT_TIME, 'yyyy-MM-dd hh24:mi:ss') BETWEEN '{0}' AND
                                                 AND T.EDIT_TIME BETWEEN '{0}' AND
                                                   '{1}'
                                             GROUP BY T3.EQUIPMENT_KEY
                                            UNION ALL
                                            SELECT SUM(T.SCRAP_QUANTITY) QUANTITY, T3.EQUIPMENT_KEY
                                              FROM WIP_SCRAP        T,
                                                   ---WIP_TRANSACTION@LNKTOREL  T3,
                                                   WIP_TRANSACTION  T3,
                                                   EMS_EQUIPMENTS   T1,
                                                   FMM_LOCATION_RET T2
                                             WHERE {3} AND T.TRANSACTION_KEY = T3.TRANSACTION_KEY
                                               AND T3.EQUIPMENT_KEY = T1.EQUIPMENT_KEY
                                               AND T1.LOCATION_KEY = T2.LOCATION_KEY
                                               AND T2.LOCATION_LEVEL = 9
                                               AND T2.PARENT_LOC_LEVEL = 5
                                               AND T2.PARENT_LOC_KEY = '{2}'
                                               ----AND TO_CHAR(T.EDIT_TIME, 'yyyy-MM-dd hh24:mi:ss') BETWEEN '{0}' AND
                                                   AND T.EDIT_TIME BETWEEN '{0}' AND
                                                   '{1}'
                                             GROUP BY T3.EQUIPMENT_KEY) A
                                            GROUP BY EQUIPMENT_KEY
                                                ", StartDate, EndDate, locationkey, f1);
        #endregion
        DataTable dt_scrapdefectQty = _db.ExecuteDataSet(CommandType.Text, sqlscrapQty).Tables[0];


        //获得设备的工作时间，各个状态的总和
        #region
        string f3 = "1=1 ";
        if (!string.IsNullOrEmpty(equipmentKey))
            f3 += string.Format(@" AND T.EQUIPMENT_KEY='{0}'", equipmentKey);

        string strAlltimes = string.Format(@"
                                            DECLARE @start_Date datetime;
                                            DECLARE @end_Date datetime;

                                            SET @start_Date = '{0}';
                                            SET @end_Date = '{1}';


                                            IF(@end_Date>SYSDATETIME())
                                            BEGIN
	                                            SET @end_Date = SYSDATETIME();
                                            END

                                            SELECT T1.EQUIPMENT_STATE_NAME,
                                                   T1.EQUIPMENT_STATE_TYPE,
                                                   T.EQUIPMENT_KEY,
                                                   ROUND(  
		                                               SUM(CASE
			                                              WHEN ( t.edit_time IS NULL OR t.edit_time >@end_Date )  AND @start_Date < t.create_time AND t.create_time <@end_Date
			                                              THEN   datediff(SECOND,t.create_time,@end_Date)
			                                              WHEN ( t.edit_time IS NULL OR t.edit_time >@end_Date) AND t.create_time <@start_Date
			                                              THEN   datediff(SECOND,@start_Date,@end_Date)
			                                              WHEN @start_Date < t.edit_time  AND t.edit_time <@end_Date  AND @start_Date < t.create_time AND t.create_time < @end_Date
			                                              THEN datediff(SECOND, t.create_time,t.edit_time )
			                                              WHEN @start_Date < t.edit_time  AND t.edit_time <@end_Date AND t.create_time <@start_Date
			                                              THEN  datediff(SECOND,@start_Date,t.edit_time)
			                                              ELSE 0
			                                            END
		                                               ),4)
                                                   AS  SECONDS,
                                                   ROUND(datediff(SECOND,@start_Date,@end_Date),4) AS ALLTIME
                                              FROM EMS_STATE_EVENT      T,
                                                   EMS_EQUIPMENT_STATES T1,
                                                   EMS_EQUIPMENTS       T2,
                                                   FMM_LOCATION_RET     T3
                                             WHERE {3} 
                                               AND T.EQUIPMENT_FROM_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                               AND T.EQUIPMENT_KEY = T2.EQUIPMENT_KEY
                                               AND T2.LOCATION_KEY = T3.LOCATION_KEY
                                               AND T3.LOCATION_LEVEL = 9
                                               AND T3.PARENT_LOC_KEY = '{2}'
                                               AND T3.PARENT_LOC_LEVEL = 5
                                               AND T.CREATE_TIME <= @end_Date
                                               AND (T.EDIT_TIME IS NULL OR T.EDIT_TIME >=@start_Date)
                                               AND T.ISCURRENT > 1
                                             GROUP BY T1.EQUIPMENT_STATE_NAME, T1.EQUIPMENT_STATE_TYPE, T.EQUIPMENT_KEY", StartDate, EndDate, locationkey, f3);
        #endregion
        DataTable dtAllTime = _db.ExecuteDataSet(CommandType.Text, strAlltimes).Tables[0];

        //获得总故障时间+故障次数
        #region
        string f4 = "1=1 ";
        if (!string.IsNullOrEmpty(equipmentKey))
            f4 += string.Format(@" AND T.EQUIPMENT_KEY='{0}'", equipmentKey);

        string strMttr_Times = string.Format(@" DECLARE @start_Date datetime;
                                                DECLARE @end_Date datetime;

                                                SET @start_Date = '{0}';
                                                SET @end_Date = '{1}';

                                                SELECT T1.EQUIPMENT_STATE_NAME,
                                                T.EQUIPMENT_KEY,
                                                ROUND(SUM(DATEDIFF(SECOND, T.CREATE_TIME,ISNULL(T.EDIT_TIME, GETDATE()))),4)AS SECONDS,
                                                COUNT(0) TIMES
                                                FROM  EMS_STATE_EVENT      T,
                                                EMS_EQUIPMENT_STATES T1,
                                                EMS_EQUIPMENTS       T2,
                                                FMM_LOCATION_RET     T3
                                                WHERE {3}  
                                                AND T.EQUIPMENT_FROM_STATE_KEY = T1.EQUIPMENT_STATE_KEY
                                                AND T.EQUIPMENT_KEY = T2.EQUIPMENT_KEY
                                                AND T2.LOCATION_KEY = T3.LOCATION_KEY
                                                AND T3.LOCATION_LEVEL = 9
                                                AND T3.PARENT_LOC_KEY = '{2}'
                                                AND T3.PARENT_LOC_LEVEL = 5
                                                AND T.CREATE_TIME BETWEEN @start_Date AND @end_Date
                                                AND T1.EQUIPMENT_STATE_NAME IN ('DOWN', 'CIMD', 'FACD','PM')
                                                GROUP BY T1.EQUIPMENT_STATE_NAME, T.EQUIPMENT_KEY", StartDate, EndDate, locationkey, f4);
        #endregion
        DataTable dtmttrmtbfTimes = _db.ExecuteDataSet(CommandType.Text, strMttr_Times).Tables[0];

        dtEquipment.Columns.Add("WRKALLCELLS");//加工量
        dtEquipment.Columns.Add("WPH_REL");//WPH实际
        dtEquipment.Columns.Add("EFFI");//WPH实际
        dtEquipment.Columns.Add("EQUIPMENT_AVTIME_TARGET");//设备目标Uptime
        dtEquipment.Columns.Add("WRK_OEE");//生产Uptime
        dtEquipment.Columns.Add("AVTIME_TARGET2");//AVTIME 目标
        dtEquipment.Columns.Add("P_RUN");//RUN%
        dtEquipment.Columns.Add("P_LOST");//LOST%

        dtEquipment.Columns.Add("P_TEST");//TEST%
        dtEquipment.Columns.Add("P_OTHER");//OTHER%
        dtEquipment.Columns.Add("P_DOWN");//DOWN%

        dtEquipment.Columns.Add("SUB_DOWN");
        dtEquipment.Columns.Add("SUB_CIMD");
        dtEquipment.Columns.Add("SUB_W_EN");
        dtEquipment.Columns.Add("SUB_FACD");
        dtEquipment.Columns.Add("SUB_PM");

        dtEquipment.Columns.Add("P_MON");//MON%
        dtEquipment.Columns.Add("P_T_DOWN");//T_DOWN%

        dtEquipment.Columns.Add("MTBF");//MTBF%
        dtEquipment.Columns.Add("MTTR");//MTTR%

        //总的时间，以秒为单位
        double total = Convert.ToDouble(dtAllTime.Rows[0]["ALLTIME"]);

        foreach (DataRow drEquipment in dtEquipment.Rows)
        {
            //获得设备
            string equipment_key = drEquipment["EQUIPMENT_KEY"].ToString();
            DataRow[] drsQty = dt_allQty.Select(string.Format(@"EQUIPMENT_KEY='{0}'", equipment_key));
            double input = 0;
            if (drsQty.Length > 0)
                input = Convert.ToDouble(drsQty[0]["QUANTITY"] == null ? "0" : drsQty[0]["QUANTITY"].ToString());

            //设备的目标运行时间
            drEquipment["AVTIME_TARGET2"] = (Convert.ToDouble(drEquipment["AVTIME_TARGET"].ToString()) * 100).ToString() + "%";

            double p1 = input * Convert.ToDouble(drEquipment["EQUIPMENT_TRACT_TIME"].ToString());

            double run = 0, lost = 0, test = 0;
            //获得设备的RUN时间
            DataRow[] dralltime01 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "RUN", equipment_key));
            foreach (DataRow dr in dralltime01)
                run += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的LOST时间
            DataRow[] dralltime02 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "LOST", equipment_key));
            foreach (DataRow dr in dralltime02)
                lost += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的TEST时间
            DataRow[] dralltime03 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "TEST", equipment_key));
            foreach (DataRow dr in dralltime03)
                test += Convert.ToDouble(dr["SECONDS"].ToString());

            double p2 = (run + lost + test);//以秒为单位
            //计算得到PE
            double pe = Math.Round(p1 / (p2 == 0 ? 1 : p2), 2);

            //不良数量总和
            int scrapdefect = 0;
            DataRow[] drs02 = dt_scrapdefectQty.Select(string.Format(@"EQUIPMENT_KEY='{0}'", equipment_key));
            foreach (DataRow dr in drs02)
                scrapdefect = Convert.ToInt32(dr["QUANTITY"].ToString());

            //计算得到QE
            double qe = Math.Round(((input - scrapdefect) / (input == 0 ? 1 : input)), 2);

            //AE的计算公式是 Total时间-Other时间-（down：DOWN_PM_MON+T_DOWN）/(Total-other)
            double a1 = 0, a2 = 0;
            double down = 0, pm = 0, mon = 0, t_down = 0, other = 0;
            //获得设备的DOWN时间
            DataRow[] dralltime04 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "DOWN", equipment_key));
            foreach (DataRow dr in dralltime04)
                down += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的PM时间
            DataRow[] dralltime05 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "PM", equipment_key));
            foreach (DataRow dr in dralltime05)
                pm += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的MON时间
            DataRow[] dralltime06 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "MON", equipment_key));
            foreach (DataRow dr in dralltime06)
                mon += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的T_DOWN时间
            DataRow[] dralltime07 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "T_DOWN", equipment_key));
            foreach (DataRow dr in dralltime07)
                t_down += Convert.ToDouble(dr["SECONDS"].ToString());
            //获得设备的OTHER时间
            DataRow[] dralltime08 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_TYPE='{0}' AND EQUIPMENT_KEY='{1}'", "OTHER", equipment_key));
            foreach (DataRow dr in dralltime08)
                other += Convert.ToDouble(dr["SECONDS"].ToString());

            double sub_down = 0, sub_cimd = 0, sub_w_en = 0, sub_facd = 0, sub_pm =0;
            DataRow[] dralltime0401 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_NAME='{0}' AND EQUIPMENT_KEY='{1}'", "DOWN", equipment_key));
            foreach (DataRow dr in dralltime0401)
                sub_down += Convert.ToDouble(dr["SECONDS"].ToString());
            DataRow[] dralltime0402 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_NAME='{0}' AND EQUIPMENT_KEY='{1}'", "CIMD", equipment_key));
            foreach (DataRow dr in dralltime0402)
                sub_cimd += Convert.ToDouble(dr["SECONDS"].ToString());
            DataRow[] dralltime0403 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_NAME='{0}' AND EQUIPMENT_KEY='{1}'", "W_EN", equipment_key));
            foreach (DataRow dr in dralltime0403)
                sub_w_en += Convert.ToDouble(dr["SECONDS"].ToString());
            DataRow[] dralltime0404 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_NAME='{0}' AND EQUIPMENT_KEY='{1}'", "FACD", equipment_key));
            foreach (DataRow dr in dralltime0404)
                sub_facd += Convert.ToDouble(dr["SECONDS"].ToString());
            DataRow[] dralltime0405 = dtAllTime.Select(string.Format(@"EQUIPMENT_STATE_NAME='{0}' AND EQUIPMENT_KEY='{1}'", "PM", equipment_key));
            foreach (DataRow dr in dralltime05)
                sub_pm += Convert.ToDouble(dr["SECONDS"].ToString());
            //double dn = down == 0 ? 1 : down;
            string p_sub_down = (sub_down / total).ToString("P");
            string p_sub_cimd = (sub_cimd / total).ToString("P");
            string p_sub_w_en = (sub_w_en / total).ToString("P");
            string p_sub_facd = (sub_facd / total).ToString("P");
            string p_sub_pm = (sub_pm / total).ToString("P");


            drEquipment["SUB_DOWN"] = p_sub_down;
            drEquipment["SUB_CIMD"] = p_sub_cimd;
            drEquipment["SUB_W_EN"] = p_sub_w_en;
            drEquipment["SUB_FACD"] = p_sub_facd;
            drEquipment["SUB_PM"] = p_sub_pm;

            a1 = run + lost + test;
            //a2 = run + lost + test + down + pm + mon + t_down;
            a2 = total - other;

            double ae = Math.Round((a1 / (a2 == 0 ? 1 : a2)), 2);

            double e1 = 0;
            e1 = run + lost + test + mon + t_down;

            //计算EE
            double ee = Math.Round((e1 / (a2 == 0 ? 1 : a2)), 3);

            //设备Uptime
            //drEquipment["EQUIPMENT_AVTIME_TARGET"] = Math.Round(ee * pe * qe * 100, 2).ToString() + "%";
            drEquipment["EQUIPMENT_AVTIME_TARGET"] = Math.Round(ee * 100, 2).ToString() + "%";
            //生产Uptime
            drEquipment["WRK_OEE"] = Math.Round(ae * pe * qe * 100, 2).ToString() + "%";

            total = total == 0 ? 1 : total;
            string p_run = Math.Round((run / total) * 100, 2).ToString() + "%";
            if (lost > total)
                lost = total;

            string p_lost = Math.Round((lost / total) * 100, 2).ToString() + "%";
            string p_test = Math.Round((test / total) * 100, 2).ToString() + "%";
            string p_other = Math.Round((other / total) * 100, 2).ToString() + "%";
            string p_down = Math.Round((down / total) * 100, 2).ToString() + "%";
            string p_mon = Math.Round((mon / total) * 100, 2).ToString() + "%";
            string p_t_down = Math.Round((t_down / total) * 100, 2).ToString() + "%";
            drEquipment["P_RUN"] = p_run;
            drEquipment["P_LOST"] = p_lost;
            drEquipment["P_TEST"] = p_test;
            drEquipment["P_OTHER"] = p_other;
            drEquipment["P_DOWN"] = p_down;            
            drEquipment["P_MON"] = p_mon;
            drEquipment["P_T_DOWN"] = p_t_down;

            //计算run%+lost%+test%
            double rlt = Math.Round((run + lost + test) / total, 4);
            //添加设备加工量
            DataRow[] drs01 = dt_allQty.Select(string.Format(@"EQUIPMENT_KEY='{0}'", equipment_key));
            if (drs01 != null && drs01.Length > 0)
            {

                input = Convert.ToInt32(drs01[0]["QUANTITY"].ToString());
                drEquipment["WRKALLCELLS"] = input;
                //添加WPH实际(片)
                drEquipment["WPH_REL"] = Math.Round((Convert.ToDouble(drs01[0]["QUANTITY"].ToString()) / ((total/24) * rlt)), 2).ToString();
                //目标片数，设备表自带

                //添加EFFI
                drEquipment["EFFI"] = (Math.Round(Convert.ToDouble(drEquipment["WPH_REL"].ToString()) / Convert.ToDouble(drEquipment["WPH_TARGET"].ToString()), 2) * 100).ToString() + "%";
            }
            else
            {
                drEquipment["WPH_REL"] = 0;
                drEquipment["EFFI"] = "0%";
            }

            double m1 = 0, m2 = 0;

            DataRow[] drs03 = dtmttrmtbfTimes.Select(string.Format(@"EQUIPMENT_KEY='{0}'", equipment_key));
            foreach (DataRow dr in drs03)
            {
                m1 += Convert.ToDouble(dr["SECONDS"].ToString());
                m2 += Convert.ToDouble(dr["TIMES"].ToString());
            }
            double mttr = Math.Round(((m1/60)) / (m2 == 0 ? 1 : m2), 2);
            drEquipment["MTTR"] = mttr;

            double mtbf = Math.Round(run / (m2 == 0 ? 1 : m2), 2);
            drEquipment["MTBF"] = mtbf;
        }

        return dtEquipment;
    }



    /// <summary>
    /// 获得车间所有区域
    /// </summary>
    /// <param name="workplaceKey"></param>
    /// <returns></returns>
    /// Owner Genchille.yang 2012-04-12 09:33:56
    public DataTable GetFactoryWorkPlaceAreas(string workplaceKey)
    {
        string sql = string.Format(@"SELECT DISTINCT A.EQUIPMENT_KEY, A.EQUIPMENT_NAME,A.EQUIPMENT_CODE
                                          FROM EMS_EQUIPMENTS A
                                         WHERE A.LOCATION_KEY IN
                                               (SELECT T.LOCATION_KEY
                                                  FROM FMM_LOCATION T, FMM_LOCATION_RET T1
                                                 WHERE T.LOCATION_KEY = T1.LOCATION_KEY
                                                   AND T1.LOCATION_LEVEL = 9
                                                   AND T1.PARENT_LOC_KEY = '{0}')                                        
                                           AND A.EQUIPMENT_WPH > 0
                                           AND A.EQUIPMENT_AV_TIME > 0
                                           AND A.EQUIPMENT_TRACT_TIME > 0
                                        ", workplaceKey);
        return _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
    }
}

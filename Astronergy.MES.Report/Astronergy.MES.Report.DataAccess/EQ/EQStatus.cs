using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// 设备状态报表以及设备状态明细报表
/// </summary>
public class EQStatus
{
    private static Database _db = DatabaseFactory.CreateDatabase();

    public EQStatus()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factoryKey">工厂Key</param>
    /// <param name="eqStatusKeys">设备状态key值字符串,格式为:'key1','key2',... </param>
    /// <param name="strTodayStartTime">当天开始日期</param>
    /// <param name="strTodayEndTime">当天结束日期</param>
    /// <returns></returns>
    public DataTable GetEqStatus(string factoryKey, string eqStatusKeys, string strTodayStartTime, string strTodayEndTime)
    {
        string strSql = string.Format(@"SELECT E.EQUIPMENT_KEY,T.LOCATION_NAME,E.EQUIPMENT_CODE,E.Description,ES.EQUIPMENT_STATE_NAME ||'(' || ES.DESCRIPTION ||')' AS EQUIPMENT_STATE_NAME,EV.CREATE_TIME,to_char(sysdate-EV.CREATE_TIME,'FM9999999999990.0')*24 HOURS
          ,CASE WHEN (QTY IS NULL) THEN 0 ELSE QTY END  AS MOVE_QTY
          ,CASE WHEN (H_QTY IS NULL) THEN 0 ELSE H_QTY END  AS HOLD_QTY
          ,CASE WHEN (C_QTY IS NULL) THEN 0 ELSE C_QTY END  AS C_QTY
          ,CASE WHEN (W_QTY IS NULL) THEN 0 ELSE W_QTY END  AS WAIT_QTY
          FROM FMM_LOCATION T
         INNER JOIN FMM_LOCATION_RET T1 ON (T.LOCATION_KEY = T1.LOCATION_KEY AND T1.LOCATION_LEVEL = 9)
         INNER JOIN EMS_EQUIPMENTS E ON E.LOCATION_KEY=T.LOCATION_KEY
         INNER JOIN EMS_EQUIPMENT_STATES ES ON ES.EQUIPMENT_STATE_KEY=E.EQUIPMENT_STATE_KEY
         INNER JOIN EMS_STATE_EVENT EV ON (EV.EQUIPMENT_KEY=E.EQUIPMENT_KEY AND EV.EQUIPMENT_FROM_STATE_KEY=ES.EQUIPMENT_STATE_KEY AND (EV.EQUIPMENT_TO_STATE_KEY IS NULL OR EV.EQUIPMENT_TO_STATE_KEY=''))
         LEFT JOIN 
         (
          --当天的move量
          SELECT EQUIPMENT_KEY,SUM(QUANTITY_IN+QUANTITY_OUT) AS QTY FROM wip_transaction
          WHERE ACTIVITY ='TRACKIN' AND ACTIVITY ='TRACKOUT'
          --AND (EDIT_TIME>=to_date('{2}','yyyy-mm-dd hh24:mi:ss') AND EDIT_TIME<=to_date('{3}','yyyy-mm-dd hh24:mi:ss'))  --当日即时move量：从当日早上8点开始到现在
          GROUP BY EQUIPMENT_KEY      
         ) MOVE_QTY ON MOVE_QTY.EQUIPMENT_KEY=E.EQUIPMENT_KEY
         
         LEFT JOIN 
         (
             --设备当前暂停片数 
            SELECT EQUIPMENT_KEY,SUM(QUANTITY_IN) AS H_QTY FROM wip_transaction
            WHERE ACTIVITY ='HOLD' 
            GROUP BY EQUIPMENT_KEY 
         ) H ON H.EQUIPMENT_KEY=E.EQUIPMENT_KEY
         
         LEFT JOIN 
         (
              --当前设备的运行片数
           SELECT EQUIPMENT_KEY,SUM(QUANTITY) AS C_QTY
           FROM EMS_LOT_EQUIPMENT
           GROUP BY EQUIPMENT_KEY
         ) CU ON CU.EQUIPMENT_KEY=E.EQUIPMENT_KEY
         
         LEFT JOIN 
         (
            --设备当前等待片数
            SELECT OE.EQUIPMENT_KEY,SUM(L.QUANTITY) AS W_QTY
            FROM POR_LOT L
            INNER JOIN POR_ROUTE_STEP S ON S.ROUTE_STEP_KEY=L.CUR_STEP_VER_KEY
            INNER JOIN POR_ROUTE_OPERATION_VER  R0 ON R0.ROUTE_OPERATION_VER_KEY=S.ROUTE_OPERATION_VER_KEY
            INNER JOIN  EMS_OPERATION_EQUIPMENT OE ON OE.OPERATION_KEY=R0.ROUTE_OPERATION_VER_KEY
            WHERE L.DELETED_TERM_FLAG!=2
            GROUP BY OE.EQUIPMENT_KEY
         ) WAIT ON WAIT.EQUIPMENT_KEY=E.EQUIPMENT_KEY
         
         WHERE T1.PARENT_LOC_KEY = '{0}'
         AND E.EQUIPMENT_STATE_KEY IN ({1})
         ORDER BY T.LOCATION_NAME,E.EQUIPMENT_CODE", factoryKey, eqStatusKeys, strTodayStartTime, strTodayStartTime);
        return _db.ExecuteDataSet(CommandType.Text, strSql).Tables[0];
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="eqKey">设备 key</param>
    /// <param name="eqStatus">设备状态key值字符串,格式为:'key1','key2',...</param>
    /// <param name="types">固定值：HOLD_QTY、C_QTY、WAIT_QTY、TOTAL（对应主表中的表头字段）</param>
    /// <returns></returns>
    public DataTable GetEqWipDetail(string eqKey, string eqStatus, string types)
    {
        string strSql = string.Empty;

        switch (types.ToUpper())
        {
            case "HOLD_QTY": //设备当前暂停片数
                strSql = string.Format(@"SELECT DISTINCT L.LOT_NUMBER,P.PART_ID,L.PROMISED_TIME,L.QUANTITY,L.MATERIAL_CODE,L.SUPPLIER_NAME
                              ,CASE L.STATE_FLAG  WHEN 0 THEN 'WaitingForTrackIn' WHEN 4 THEN 'WaitingForEDC' WHEN 5 THEN 'InEDC' WHEN 9 THEN 'WaitingForTrackout' 
                              WHEN 10 THEN 'Finished' WHEN 11 THEN 'ToStore'  END AS STATES
                             ,to_char(sysdate-L.EDIT_TIME,'FM9999999999990.0')*24 HOURS
                             ,S.ROUTE_OPERATION_NAME
                            FROM POR_LOT L
                            INNER JOIN POR_PART P ON (P.PART_KEY=L.PART_VER_KEY)
                            INNER JOIN POR_ROUTE_STEP S ON (S.ROUTE_STEP_KEY=L.CUR_STEP_VER_KEY)
                            INNER JOIN EMS_LOT_EQUIPMENT EL ON (EL.LOT_KEY=L.LOT_KEY)
                            INNER JOIN EMS_EQUIPMENTS E ON (E.EQUIPMENT_KEY=EL.EQUIPMENT_KEY)
                            INNER JOIN POR_ROUTE_OPERATION_VER RO ON (RO.ROUTE_OPERATION_VER_KEY=S.ROUTE_OPERATION_VER_KEY)
                            INNER JOIN WIP_TRANSACTION WIP ON (WIP.PIECE_KEY=L.LOT_KEY AND WIP.ACTIVITY ='HOLD') --暂停状态的玻璃
                            WHERE E.EQUIPMENT_STATE_KEY IN ({0}) AND 
                            RO.ROUTE_OPERATION_VER_KEY IN
                            (
                              SELECT OPERATION_KEY FROM EMS_OPERATION_EQUIPMENT OE
                              WHERE OE.EQUIPMENT_KEY='{1}'
                            )", eqStatus, eqKey);
                break;
            case "C_QTY":  //设备当前运行片数
                strSql = string.Format(@"SELECT DISTINCT L.LOT_NUMBER,P.PART_ID,L.PROMISED_TIME,EL.QUANTITY,L.MATERIAL_CODE,L.SUPPLIER_NAME
                              ,CASE L.STATE_FLAG  WHEN 0 THEN 'WaitingForTrackIn' WHEN 4 THEN 'WaitingForEDC' WHEN 5 THEN 'InEDC' WHEN 9 THEN 'WaitingForTrackout' 
                              WHEN 10 THEN 'Finished' WHEN 11 THEN 'ToStore'  END AS STATES
                             ,to_char(sysdate-L.EDIT_TIME,'FM9999999999990.0')*24 HOURS
                             ,S.ROUTE_OPERATION_NAME
                            FROM POR_LOT L
                            INNER JOIN POR_PART P ON (P.PART_KEY=L.PART_VER_KEY)
                            INNER JOIN POR_ROUTE_STEP S ON (S.ROUTE_STEP_KEY=L.CUR_STEP_VER_KEY)
                            INNER JOIN EMS_LOT_EQUIPMENT EL ON (EL.LOT_KEY=L.LOT_KEY)
                            INNER JOIN EMS_EQUIPMENTS E ON (E.EQUIPMENT_KEY=EL.EQUIPMENT_KEY)
                            INNER JOIN POR_ROUTE_OPERATION_VER RO ON (RO.ROUTE_OPERATION_VER_KEY=S.ROUTE_OPERATION_VER_KEY)
                            WHERE E.EQUIPMENT_STATE_KEY IN ({0}) AND 
                            EL.EQUIPMENT_KEY ='{1}'  
                            --RO.ROUTE_OPERATION_VER_KEY IN
                            --(
                             -- SELECT OPERATION_KEY FROM EMS_OPERATION_EQUIPMENT OE
                             -- WHERE OE.EQUIPMENT_KEY='{1}')
                            ", eqStatus, eqKey);
                break;
            case "WAIT_QTY":   //设备当前等待片数
                strSql = string.Format(@"SELECT DISTINCT L.LOT_NUMBER,P.PART_ID,L.PROMISED_TIME,L.QUANTITY,L.MATERIAL_CODE,L.SUPPLIER_NAME
                              ,CASE L.STATE_FLAG  WHEN 0 THEN 'WaitingForTrackIn' WHEN 4 THEN 'WaitingForEDC' WHEN 5 THEN 'InEDC' WHEN 9 THEN 'WaitingForTrackout' 
                              WHEN 10 THEN 'Finished' WHEN 11 THEN 'ToStore'  END AS STATES
                             ,to_char(sysdate-L.EDIT_TIME,'FM9999999999990.0')*24 HOURS
                             ,S.ROUTE_OPERATION_NAME
                            FROM POR_LOT L
                            INNER JOIN POR_PART P ON (P.PART_KEY=L.PART_VER_KEY)
                            INNER JOIN POR_ROUTE_STEP S ON (S.ROUTE_STEP_KEY=L.CUR_STEP_VER_KEY)
                            INNER JOIN EMS_LOT_EQUIPMENT EL ON (EL.LOT_KEY=L.LOT_KEY)
                            INNER JOIN EMS_EQUIPMENTS E ON (E.EQUIPMENT_KEY=EL.EQUIPMENT_KEY)                            
                            INNER JOIN POR_ROUTE_OPERATION_VER RO ON (RO.ROUTE_OPERATION_VER_KEY=S.ROUTE_OPERATION_VER_KEY)                  
                            WHERE L.DELETED_TERM_FLAG!=2 
                            AND E.EQUIPMENT_STATE_KEY IN ({0}) 
                            AND 
                            RO.ROUTE_OPERATION_VER_KEY IN
                            (
                              SELECT OPERATION_KEY FROM EMS_OPERATION_EQUIPMENT OE
                              WHERE OE.EQUIPMENT_KEY='{1}'
                            )", eqStatus, eqKey);
                break;
            case "TOTAL"://以上三种之和
                strSql = string.Format(@"SELECT DISTINCT L.LOT_NUMBER,P.PART_ID,L.PROMISED_TIME,L.QUANTITY,L.MATERIAL_CODE,L.SUPPLIER_NAME
                              ,CASE L.STATE_FLAG  WHEN 0 THEN 'WaitingForTrackIn' WHEN 4 THEN 'WaitingForEDC' WHEN 5 THEN 'InEDC' WHEN 9 THEN 'WaitingForTrackout' 
                              WHEN 10 THEN 'Finished' WHEN 11 THEN 'ToStore'  END AS STATES
                             ,to_char(sysdate-L.EDIT_TIME,'FM9999999999990.0')*24 HOURS
                             ,S.ROUTE_OPERATION_NAME
                            FROM POR_LOT L
                            INNER JOIN POR_PART P ON (P.PART_KEY=L.PART_VER_KEY)
                            INNER JOIN POR_ROUTE_STEP S ON (S.ROUTE_STEP_KEY=L.CUR_STEP_VER_KEY)
                            INNER JOIN EMS_LOT_EQUIPMENT EL ON (EL.LOT_KEY=L.LOT_KEY)
                            INNER JOIN EMS_EQUIPMENTS E ON (E.EQUIPMENT_KEY=EL.EQUIPMENT_KEY)
                            INNER JOIN POR_ROUTE_OPERATION_VER RO ON (RO.ROUTE_OPERATION_VER_KEY=S.ROUTE_OPERATION_VER_KEY)
                            WHERE E.EQUIPMENT_STATE_KEY IN ({0}) AND RO.ROUTE_OPERATION_VER_KEY IN
                              (
                                  SELECT OPERATION_KEY FROM EMS_OPERATION_EQUIPMENT OE
                                                          WHERE OE.EQUIPMENT_KEY='{1}'
                              )", eqStatus, eqKey);
                break;
            default:
                break;
        }

        if (!string.IsNullOrEmpty(strSql))
        {
            return _db.ExecuteDataSet(CommandType.Text, strSql).Tables[0];
        }
        return null;
    }

}

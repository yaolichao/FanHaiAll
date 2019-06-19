using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// 设备批次加工历史报表
/// </summary>
public class EQLotHistory
{
    private static Database _db = DatabaseFactory.CreateDatabase();

    public EQLotHistory()
    {

    }

    public DataTable GetEqLot(string factoryKey, string eqKey, string startDate, string endDate)
    {
        string strSql = string.Format(@"SELECT LOT_NUMBER,PART_NAME,EQUIPMENT_CODE,ROUTE_STEP_NAME,DESCRIPTIONS,IN_TIME,
                            (SELECT USERNAME FROM RBAC_USER WHERE IN_EDITOR=BADGE) IN_EDITOR,
                            IN_QTY,
                            OUT_TIME,
                            (SELECT USERNAME FROM RBAC_USER WHERE OUT_EDITOR=BADGE) OUT_EDITOR,
                            OUT_QTY
                             FROM (
                            SELECT LOT.LOT_NUMBER ,P.PART_NAME,EQU.EQUIPMENT_CODE,R.ROUTE_STEP_NAME,R.DESCRIPTIONS
                            ,MAX(DECODE(TRX.ACTIVITY,'TRACKIN',TRX.EDIT_TIME)) AS IN_TIME
                            ,MAX(DECODE(TRX.ACTIVITY,'TRACKIN',TRX.EDITOR)) AS IN_EDITOR
                            ,SUM(DECODE(TRX.ACTIVITY,'TRACKIN',TRX.QUANTITY_IN)) AS IN_QTY
                            ,MAX(DECODE(TRX.ACTIVITY,'TRACKOUT',TRX.EDIT_TIME)) AS OUT_TIME
                            ,MAX(DECODE(TRX.ACTIVITY,'TRACKOUT',TRX.EDITOR)) AS OUT_EDITOR
                            ,SUM(DECODE(TRX.ACTIVITY,'TRACKOUT',TRX.QUANTITY_OUT))  AS OUT_QTY 
                            FROM WIP_TRANSACTION  TRX
                            INNER JOIN  POR_LOT LOT ON TRX.PIECE_KEY=LOT.LOT_KEY
                            INNER JOIN POR_PART P ON P.PART_KEY=LOT.PART_VER_KEY
                            INNER JOIN POR_ROUTE_STEP R ON R.ROUTE_STEP_KEY=TRX.STEP_KEY
                            INNER JOIN EMS_EQUIPMENTS EQU  ON EQU.EQUIPMENT_KEY = TRX.EQUIPMENT_KEY 
                            WHERE (TRX.ACTIVITY='TRACKIN' OR TRX.ACTIVITY='TRACKOUT') AND LOT.FACTORYROOM_KEY='{0}' 
                                   AND TRX.EDIT_TIME>=TO_DATE('{1}','YYYY-MM-DD HH24:MI:SS') AND TRX.EDIT_TIME<=TO_DATE('{2}','YYYY-MM-DD HH24:MI:SS')                          
                        ", factoryKey, startDate, endDate);

        if (!string.IsNullOrEmpty(eqKey) && !eqKey.Equals("ALL"))
        {
            strSql += string.Format(" AND TRX.EQUIPMENT_KEY='{0}'", eqKey);
        }

        strSql += " GROUP BY LOT.LOT_NUMBER ,P.PART_NAME,R.ROUTE_STEP_NAME,R.DESCRIPTIONS,TRX.EDITOR,EQU.EQUIPMENT_CODE)";

        return _db.ExecuteDataSet(CommandType.Text, strSql).Tables[0];
    }

}

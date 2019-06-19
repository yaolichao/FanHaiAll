using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// 设备运行历史报表
/// </summary>
public class EQRunHistory
{
    private static Database _db = DatabaseFactory.CreateDatabase();

    public EQRunHistory()
    {

    }

    public DataTable GetEQRunHisory(string eqKey, string startDate, string endDate)
    {
        string sql = string.Format(@"SELECT EQ.EQUIPMENT_CODE,EQ.Description,
                                    (SELECT EQUIPMENT_STATE_NAME ||'(' ||S.DESCRIPTION ||')' FROM EMS_EQUIPMENT_STATES  S WHERE S.EQUIPMENT_STATE_KEY=E.EQUIPMENT_FROM_STATE_KEY) AS FROM_STATE_NAME,
                                    (SELECT EQUIPMENT_STATE_NAME ||'(' ||S.DESCRIPTION ||')' FROM EMS_EQUIPMENT_STATES  S WHERE S.EQUIPMENT_STATE_KEY=E.EQUIPMENT_TO_STATE_KEY) AS TO_STATE_NAME,
                                    E.CREATE_TIME,E.EDIT_TIME,(SELECT USERNAME FROM RBAC_USER WHERE BADGE=E.CREATOR) CREATOR,E.DESCRIPTION AS REMARK
                                    FROM EMS_STATE_EVENT E
                                    INNER JOIN EMS_EQUIPMENTS EQ ON EQ.EQUIPMENT_KEY=E.EQUIPMENT_KEY
                                    WHERE E.CREATE_TIME>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and E.CREATE_TIME<=to_date('{1}','yyyy-mm-dd hh24:mi:ss')", startDate, endDate);

        if (!eqKey.ToUpper().Equals("ALL") && !string.IsNullOrEmpty(eqKey))
        {
            sql += string.Format(@" AND EQ.EQUIPMENT_KEY ='{0}' ", eqKey);
        }

        DataTable dt = _db.ExecuteDataSet(CommandType.Text, sql).Tables[0];
        return dt;     
    }
}

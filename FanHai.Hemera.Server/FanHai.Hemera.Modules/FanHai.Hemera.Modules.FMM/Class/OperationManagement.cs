using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Utils.StaticFuncs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SolarViewer.Hemera.Utils.DatabaseHelper;



namespace SolarViewer.Hemera.Modules.FMM
{
    public class OperationManagement
    {
        public static DataSet OperationUpdate(Database db, DbTransaction dbtran, DataSet dataSet)
        {
            DataSet retDS = new DataSet();
            
            if (null != dataSet)
            {
                List<string> sqlCommandList = new List<string>();
                if (dataSet.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildUpdateSqlStatements(ref sqlCommandList, new POR_ROUTE_OPERATION_VER_FIELDS(),
                        dataSet.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME],
                        new Dictionary<string, string>() { 
                                                            {POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                                                            {POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIMEZONE, "CN-ZH"}
                                                         },
                        new List<string>() { POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY },
                        POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY);
                }
                if (dataSet.Tables.Contains(POR_ROUT_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildSqlStatementsForUDAs(ref sqlCommandList, new POR_ROUT_OPERATION_ATTR_FIELDS(), dataSet.Tables[POR_ROUT_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME], POR_ROUT_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY);
                }

                if (sqlCommandList.Count > 0)
                {
                    foreach (string sql in sqlCommandList)
                    {
                        db.ExecuteNonQuery(dbtran, CommandType.Text, sql);
                    }
                }
            }
            else
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, "The input parameter is wrong for opration update");
            }
            return retDS;
        }

    }
}

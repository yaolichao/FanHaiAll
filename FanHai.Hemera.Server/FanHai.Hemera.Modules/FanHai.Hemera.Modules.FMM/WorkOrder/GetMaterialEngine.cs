using System;
using System.Collections.Generic;
using System.Text;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Data.Common;

namespace FanHai.Hemera.Modules.FMM
{

    /// <summary>
    /// 物料数据相关数据操作类。
    /// </summary>
    /// <remarks>
    /// 包含添加、查询、更新、删除物料，检查工单是否存在等方法。
    /// </remarks>
    public class GetMaterialEngine : AbstractEngine,IGetMaterialsEngine
    {
        private Database db = null;                               //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetMaterialEngine()
        {
            db = DatabaseFactory.CreateDatabase();  //创建数据库对象。
        }
        /// <summary>
        /// 通过工单号检查工单是否存在。
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>
        /// true或者false,ture表示存在，false表示不存在。
        /// </returns>
        public bool CheckWorkOrderExistedViaOrderNum(string workOrderNo)
        {
            //如果工单号为空或null
            if (string.IsNullOrEmpty(workOrderNo))
            {
                return false;
            }
            //组织查询SQL，并执行查询。
            string strSql = "SELECT COUNT(*) FROM POR_WORK_ORDER WHERE ORDER_NUMBER ='" + workOrderNo.PreventSQLInjection() + "'";
            int count = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
            //如果有查找到对应的记录，返回true。表示该工单号对应的工单存在。
            if (count > 0)
            {
                return true;
            }
            else //否则，返回false，表示该工单号对应的工单不存在。
            {
                return false;
            }

        }
        /// <summary>
        /// 根据工单号和配料单号检查工单是否存在。
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="recNumber">配料单号</param>
        /// <returns>
        /// true或者false,ture表示存在，false表示不存在。
        /// </returns>
        public bool CheckWorkOrderExisted(string workOrderNo,string recNumber)
        {
            if (string.IsNullOrEmpty(workOrderNo) && string.IsNullOrEmpty(recNumber))
            {
                return false;
            }
            string strSql = "SELECT COUNT(*) FROM POR_WORK_ORDER_REC WHERE 1 = 1";
            if (!string.IsNullOrEmpty(workOrderNo))
            {
                strSql += " AND WORK_ORDER_NUMBER ='" + workOrderNo.PreventSQLInjection() + "'";
            }
            if (!string.IsNullOrEmpty(recNumber))
            {
                strSql += " AND REC_ORDER_NUMBER ='" + recNumber.PreventSQLInjection() + "'";
            }
            int count= Convert.ToInt32(db.ExecuteScalar(CommandType.Text, strSql));
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 向工单表中插入工单信息，同时向配料单表中插入配料信息
        /// </summary>
        /// <param name="dsParams">POR_WORK_ORDER和POR_WORK_ORDER_REC数据表和数据</param>
        /// <returns>返回含有操作信息的结果集</returns>
        public DataSet AddMaterialInformation(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommand = string.Empty;
            POR_WORK_ORDER_REC_FIELDS MaterialsFields = new POR_WORK_ORDER_REC_FIELDS();
            if (dsParams != null)
            {
                //生成向工单表插入数据的SQL
                List<string> sqlCommandList = new List<string>();
                if (dsParams.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DatabaseTable.BuildInsertSqlStatements(ref sqlCommandList,
                                                      new POR_WORK_ORDER_FIELDS(),
                                                      dsParams.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME],
                                                      new Dictionary<string, string>() { },
                                                      new List<string>());
                }

                //生成向配料单表插入数据的SQL
                if (dsParams.Tables.Contains(POR_WORK_ORDER_REC_FIELDS.DATABASE_TABLE_NAME))
                {
                    for(int i =0 ; i < dsParams.Tables[POR_WORK_ORDER_REC_FIELDS.DATABASE_TABLE_NAME].Rows.Count;i++)
                    {
                       sqlCommand =DatabaseTable.BuildInsertSqlStatement(MaterialsFields,dsParams.Tables[POR_WORK_ORDER_REC_FIELDS.DATABASE_TABLE_NAME],
                                                            i,new Dictionary<string,string>{},new List<string>());
                       sqlCommandList.Add(sqlCommand);       
                    }                           
                }
                if (sqlCommandList.Count > 0)
                {
                    DbConnection dbCon = db.CreateConnection();
                    dbCon.Open();
                    DbTransaction dbTrans = dbCon.BeginTransaction();
                    try
                    {
                        foreach (string sql in sqlCommandList)
                        {
                            db.ExecuteNonQuery(dbTrans, CommandType.Text, sql);
                        }
                        dbTrans.Commit();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                        LogService.LogError("MaterialsInsert Error: " + ex.Message);
                    }
                    finally
                    {
                        dbTrans = null;
                        dbCon.Close();
                        dbCon = null;
                    }
                }
            }
            else
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "No Work Order Tables in input paremter.");
            }

            return dsReturn;
        }
        /// <summary>
        /// 查询物料信息。
        /// </summary>
        /// <param name="workOrderNumber">工单号。</param>
        /// <param name="recNumber">配料单号。</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        public DataSet QueryMaterialInformation(string workOrderNumber, string recNumber)
        {
            DataSet dsReturn = new DataSet();

            if (string.IsNullOrEmpty(workOrderNumber) && string.IsNullOrEmpty(recNumber))
            {
                return dsReturn;
            }
            string strSql = @"SELECT A.WORK_ORDER_KEY,A.ORDER_NUMBER,A.ORDER_STATE,
                                A.ORDER_PRIORITY,A.DESCRIPTIONS,A.QUANTITY_ORDERED,
                                A.QUANTITY_IN_PROGRESS,A.QUANTITY_SCRAPPED,A.PART_NUMBER,
                                A.ORDER_CLOSE_TYPE,A.CREATOR,A.NEXT_SEQ,A.QUANTITY_LEFT,
                                A.REVENUE_TYPE,A.REC_ORDER_NUMBER,B.*
                            FROM POR_WORK_ORDER_REC B,POR_WORK_ORDER A
                            WHERE A.ORDER_NUMBER=B.WORK_ORDER_NUMBER";
            if (!string.IsNullOrEmpty(workOrderNumber))
            {
                strSql += " AND A.ORDER_NUMBER ='" + workOrderNumber.PreventSQLInjection() + "'";
            }
            if (!string.IsNullOrEmpty(recNumber))
            {
                strSql += " AND B.REC_ORDER_NUMBER ='" + recNumber.PreventSQLInjection() + "'";
            }
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, strSql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("Query Material Information Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 更新物料信息。
        /// </summary>
        /// <param name="dsParams">包含物料信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet UpdateMaterialInformation(DataSet dsParams)
        {
            throw new NotImplementedException();
            //DataSet dsReturn = new DataSet();
            //return dsReturn;
        }
        /// <summary>
        /// 删除物料信息。
        /// </summary>
        /// <param name="dsParams">包含物料信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet DeleteMaterialInformation(DataSet dsParams)
        {
            throw new NotImplementedException();
            //DataSet dsReturn = new DataSet();
            //return dsReturn;
        }
    }
}

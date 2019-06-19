using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Modules.FMM
{
    public class ByProductPartEngine :AbstractEngine, IByProductPartEngine 
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ByProductPartEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public override void Initialize(){}



        #region IByProductPartEngine 成员

        public DataSet GetByFourParameters(string partId, string partType, string partModule, string partClass, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sqlCommon = new StringBuilder();
            try
            {
                sqlCommon.Append(@"SELECT PART_ID,PART_DESC,PART_TYPE,PART_MODULE,PART_CLASS,
	                                           (SELECT COUNT(1)
		                                        FROM  POR_PART_BYPRODUCT b
		                                        WHERE b.MAIN_PART_NUMBER=A.PART_ID
		                                        AND b.IS_USED='Y') AS TOTOL
                                        FROM dbo.POR_PART A WHERE 1=1 AND A.PART_STATUS = 1");
                if (!string.IsNullOrEmpty(partId))
                {
                    sqlCommon.AppendFormat(" AND PART_ID LIKE '{0}%'", partId);
                }
                if (!string.IsNullOrEmpty(partType))
                {
                    sqlCommon.AppendFormat(" AND PART_TYPE LIKE '{0}%'", partType);
                }
                if (!string.IsNullOrEmpty(partModule))
                {
                    sqlCommon.AppendFormat(" AND PART_MODULE LIKE '{0}%'", partModule);
                }
                if (!string.IsNullOrEmpty(partClass))
                {
                    sqlCommon.AppendFormat(" AND PART_CLASS LIKE '{0}%'", partClass);
                }
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sqlCommon.ToString(),
                                                    pconfig.PageNo,
                                                    pconfig.PageSize,
                                                    out pages,
                                                    out records,
                                                    db,
                                                    dsReturn,
                                                    "POR_PART",
                                                    "PART_ID ASC");
                pconfig.Pages = pages;
                pconfig.Records = records;

                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetByFourParameters Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IByProductPartEngine 成员


        public DataSet GetByPartId(string partid)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            string sqlCommon = string.Empty;
            string sqlCommon1 = string.Empty;
            try
            {
                sqlCommon = string.Format(@"SELECT ITEM_NO,MAIN_PART_NUMBER,PART_NUMBER,A.VERSION_NO,B.PART_MODULE,
                                                MIN_POWER,MAX_POWER,GRADES,STORAGE_LOCATION,
                                                A.CREATOR,A.CREATE_TIME,A.EDITOR,A.EDIT_TIME,B.PART_DESC
                                            FROM dbo.POR_PART_BYPRODUCT A
                                            LEFT JOIN POR_PART B ON A.PART_NUMBER = B.PART_ID AND  B.PART_STATUS = 1
                                            WHERE MAIN_PART_NUMBER = '{0}' AND A.IS_USED = 'Y'
                                            ORDER BY ITEM_NO ASC", 
                                            partid);
                ds1 = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                ds1.Tables[0].TableName = "POR_PART_BYPRODUCT";

                sqlCommon1 = string.Format(@"SELECT 
                                                PART_ID,PART_MODULE,PART_DESC
                                                FROM dbo.POR_PART                                                
                                                WHERE PART_STATUS = 1
                                                AND PART_ID = '{0}'
                                                ", partid);
                ds2 = db.ExecuteDataSet(CommandType.Text, sqlCommon1);
                ds2.Tables[0].TableName = "POR_PART";
                dsReturn.Merge(ds1.Tables[0]);
                dsReturn.Merge(ds2.Tables[0]);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetByPartId Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IByProductPartEngine 成员


        public DataSet CheckPart(string partId)
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;
            try
            {
                sqlCommon = string.Format(@"SELECT PART_ID,PART_DESC,PART_MODULE FROM dbo.POR_PART
                                                WHERE PART_STATUS = 1 AND PART_ID = '{0}'", partId);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("CheckPart Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IByProductPartEngine 成员


        public DataSet GetByPartId()
        {
            DataSet dsReturn = new DataSet();
            string sqlCommon = string.Empty;
            try
            {
                sqlCommon = string.Format(@"SELECT 
                                                PART_ID AS PART_NUMBER,PART_MODULE,PART_DESC
                                                FROM dbo.POR_PART                                                
                                                WHERE PART_STATUS = 1");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetByPartId Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IByProductPartEngine 成员


        public DataSet GetInfFromPorPartAndProductPart(string name ,string partNum, DataTable dtGvlist)
        {
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            DataSet dsReturn = new DataSet();
            DataSet dsCheck = new DataSet();
            IList<string> sqlCommandList = new List<string>();
            try
            {
                string sqlInsert01 = @" INSERT INTO POR_PART_BYPRODUCT(
                                                MAIN_PART_NUMBER,PART_NUMBER,VERSION_NO,ITEM_NO,
                                                MIN_POWER,MAX_POWER,GRADES,STORAGE_LOCATION,
                                                IS_USED,CREATE_TIME,CREATOR
                                                )
                                                VALUES(
                                                '{0}','{1}',{2},{3},
                                                {4},{5},'{6}','{7}',
                                                'Y',GETDATE(),'{8}')";
                string sqlUpdate = @"UPDATE dbo.POR_PART_BYPRODUCT SET
                                        EDITOR = '{0}',EDIT_TIME = GETDATE(),IS_USED='N'
                                        WHERE MAIN_PART_NUMBER = '{1}' AND IS_USED = 'Y'";

                //查询产品料号是否存在对应的联副产品
                string partInf = string.Format(@"SELECT PART_NUMBER FROM dbo.POR_PART_BYPRODUCT WHERE MAIN_PART_NUMBER = '{0}'", partNum);
                dsCheck = db.ExecuteDataSet(CommandType.Text, partInf);

                if (dsCheck.Tables[0].Rows.Count < 1)
                {
                    //没有则直接插入新增的联副产品信息
                    for (int i = 0; i < dtGvlist.Rows.Count; i++)
                    {
                        string part_number = dtGvlist.Rows[i]["PART_NUMBER"].ToString().Trim();
                        int rownumber = Convert.ToInt32(dtGvlist.Rows[i]["ITEM_NO"].ToString().Trim());
                        string grades = dtGvlist.Rows[i]["GRADES"].ToString().Trim();
                        string storage = dtGvlist.Rows[i]["STORAGE_LOCATION"].ToString().Trim();
                        string sqlinsert = string.Format(sqlInsert01,
                            partNum,
                            part_number,
                            1,
                            rownumber,
                            string.IsNullOrEmpty(dtGvlist.Rows[i]["MIN_POWER"].ToString().Trim()) ? "NULL" : dtGvlist.Rows[i]["MIN_POWER"],
                            string.IsNullOrEmpty(dtGvlist.Rows[i]["MAX_POWER"].ToString().Trim()) ? "NULL" : dtGvlist.Rows[i]["MAX_POWER"],
                            grades,
                            storage,
                            name
                            );
                        sqlCommandList.Add(sqlinsert);
                    }       
                }
                else
                {
                    //有则将原有的联副产品IS_USED改为N,修改人和修改时间更新
                    string sqlUpdate01 = string.Format(sqlUpdate,
                                                        name,
                                                        partNum
                                                     );
                    sqlCommandList.Add(sqlUpdate01);

                    string partProduct = string.Format(@"SELECT ITEM_NO,MAIN_PART_NUMBER,PART_NUMBER,VERSION_NO = VERSION_NO+1
                                                            FROM dbo.POR_PART_BYPRODUCT 
                                                            WHERE  MAIN_PART_NUMBER = '{0}' ORDER BY ITEM_NO ASC", partNum);
                    DataSet dspartInf = db.ExecuteDataSet(CommandType.Text, partProduct);
                    DataTable dtpartInf = dspartInf.Tables[0];
                    DataTable gv = dtGvlist;
                    for (int i = 0; i < dtGvlist.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dtGvlist.Rows[i]["VERSION_NO"].ToString().Trim()))
                            dtGvlist.Rows[i]["VERSION_NO"] = "1";
                    }

                    //插入最新的绑定信息
                    for(int j =0;j<dtpartInf.Rows.Count;j++)
                    {
                        for (int i = 0; i < dtGvlist.Rows.Count; i++)
                        {
                            if (dtpartInf.Rows[j]["ITEM_NO"].ToString().Trim() == dtGvlist.Rows[i]["ITEM_NO"].ToString().Trim() ||
                                dtpartInf.Rows[j]["PART_NUMBER"].ToString().Trim() == dtGvlist.Rows[i]["PART_NUMBER"].ToString().Trim())
                            {
                                if (Convert.ToInt32(dtGvlist.Rows[i]["VERSION_NO"].ToString().Trim()) < Convert.ToInt32(dtpartInf.Rows[j]["VERSION_NO"].ToString().Trim()))
                                    dtGvlist.Rows[i]["VERSION_NO"] = dtpartInf.Rows[j]["VERSION_NO"].ToString().Trim();
                            }
                            //else if (dtGvlist.Rows[i]["ITEM_NO"].ToString().Trim() == "")
                            //{
                            //    dtGvlist.Rows[i]["VERSION_NO"] = "1";
                            //}
                        }
                    }
                          
                     
                    for (int i = 0; i < dtGvlist.Rows.Count; i++)
                    {
                        string part_number = dtGvlist.Rows[i]["PART_NUMBER"].ToString().Trim();
                        int rownumber = Convert.ToInt32(dtGvlist.Rows[i]["ITEM_NO"].ToString().Trim());
                        string grades = dtGvlist.Rows[i]["GRADES"].ToString().Trim();
                        string storage = dtGvlist.Rows[i]["STORAGE_LOCATION"].ToString().Trim();
                        int veno = Convert.ToInt32(dtGvlist.Rows[i]["VERSION_NO"].ToString().Trim());
                        string sqlinsert = string.Format(sqlInsert01,
                            partNum,
                            part_number,
                            veno,
                            rownumber,
                            string.IsNullOrEmpty(dtGvlist.Rows[i]["MIN_POWER"].ToString().Trim()) ? "NULL" : dtGvlist.Rows[i]["MIN_POWER"],
                            string.IsNullOrEmpty(dtGvlist.Rows[i]["MAX_POWER"].ToString().Trim()) ? "NULL" : dtGvlist.Rows[i]["MAX_POWER"],
                            grades,
                            storage,
                            name
                            );
                    
                        sqlCommandList.Add(sqlinsert);
                    }       
                }
                foreach (string sql1 in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql1);
                }
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetInfFromPorPartAndProductPart Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IByProductPartEngine 成员


        public DataSet Delete(string partId)
        {
            //get dynamic dataset constructor
            DataSet dataDs = new DataSet();
            //define sql 
            string sqlCommand = string.Empty;

            try
            {
                if (partId != string.Empty)
                {
                    using (DbConnection dbConn = db.CreateConnection())
                    {
                        //Open Connection
                        dbConn.Open();
                        //Create Transaction
                        DbTransaction dbTran = dbConn.BeginTransaction();
                        try
                        {
                            sqlCommand = string.Format("UPDATE dbo.POR_PART_BYPRODUCT SET IS_USED = 'N' WHERE MAIN_PART_NUMBER = '{0}'", partId);

                            db.ExecuteNonQuery(dbTran, CommandType.Text, sqlCommand);

                            //Commit Transaction
                            dbTran.Commit();

                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, "");
                        }
                        catch (Exception ex)
                        {
                            FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                            //Rollback Transaction
                            dbTran.Rollback();
                            LogService.LogError("Delete Error: " + ex.Message);
                        }
                        finally
                        {
                            //Close Connection
                            dbConn.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dataDs, ex.Message);
                LogService.LogError("Delete Error: " + ex.Message);
            }

            return dataDs;
        }

        #endregion
    }
}

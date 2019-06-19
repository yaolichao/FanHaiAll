using System.Text;
using System.Data;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using FanHai.Hemera.Utils.DatabaseHelper;

namespace FanHai.Hemera.Modules.WARK
{
    public class GroupArkEngine : AbstractEngine, IGroupArkEngine
    {
         /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        private Database db2 = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public GroupArkEngine()
        {
            db = DatabaseFactory.CreateDatabase();
            db2 = DatabaseFactory.CreateDatabase("SQLServerAwms");
        }

        #region IGroupArkEngine 成员

        public DataSet GetPalletData(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"SELECT PALLET_NO,WORKNUMBER,PRO_ID,GRADE,SAP_NO,LOT_COLOR,POWER_LEVEL,AVG_POWER,TOTLE_POWER,LOT_NUMBER_QTY,CS_DATA_GROUP,ARK_FLAG
                                    FROM WIP_CONSIGNMENT WHERE ISFLAG='1'";
                if (!string.IsNullOrEmpty(palletNo))
                {
                    sqlCommon += string.Format(" AND  PALLET_NO = '{0}'", palletNo);
                }
              
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletData Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        public override void Initialize()
        {
            
        }

        #region IGroupArkEngine 成员


        public DataSet GetArkNumber(string arkCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = @"SELECT CONTAINER_KEY,CONTAINER_CODE
                                    FROM AWMS_CONTAINER WHERE STATUS = '0'";

                if (!string.IsNullOrEmpty(arkCode))
                {
                    sqlCommon += string.Format(" AND CONTAINER_CODE='{0}'", arkCode);
                }
                dsReturn = db2.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetArkNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IGroupArkEngine 成员


        public DataSet GetContainerDetailInf(string key)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon =string.Format(@"SELECT PALLET_NO
                                    FROM AWMS_CONTAINER_DETAIL WHERE STATUS = '1' AND CONTAINER_KEY = '{0}'",key);
                dsReturn = db2.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetContainerDetailInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        public DataSet GetWipConInf(DataSet ds)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt = ds.Tables[0];
                string strPlus = string.Empty;
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    strPlus += "'"+dt.Rows[i][0].ToString().Trim()+"',";
                }
                if (string.IsNullOrEmpty(strPlus))
                {
                    strPlus = "'',";
                }
                string str = strPlus.Substring(0,strPlus.Length-1);
                string sqlCommon = string.Format(@"SELECT PALLET_NO,WORKNUMBER,PRO_ID,GRADE,SAP_NO,LOT_COLOR,POWER_LEVEL,AVG_POWER,TOTLE_POWER,LOT_NUMBER_QTY,CS_DATA_GROUP
                                    FROM WIP_CONSIGNMENT WHERE PALLET_NO IN({0})", str);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWipConInf Error: " + ex.Message);
            }
            return dsReturn; 
        }

        #endregion

        #region IGroupArkEngine 成员

        //ds为柜明细表中取值的托信息的数据集，dt为前台gridview中的数据
        public DataSet UpdateArkInf(DataSet ds, DataTable dt,int flag)
        {
            DataSet retDS = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            string sql = string.Empty;
            try
            {
                #region
                //0.判定柜是是新组柜还是已经存在的柜
                //  无托信息柜组柜
                //1.新增信息到柜抬头表
                //2.新增组柜信息到明细表
                //3.修改包装表中托状态为已组柜1
                if (flag == 1)
                {
                    string key = UtilHelper.GenerateNewKey(0);
                    DataTable dtHash = ds.Tables["HASH"];
                    Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);
                    string sql01 = string.Format(@"INSERT INTO AWMS_CONTAINER(CONTAINER_KEY,CONTAINER_CODE,STATUS) VALUES('{0}','{1}','0')",
                                                key,
                                                hsTable["CONTAINER_CODE"].ToString()
                                                );
                    db2.ExecuteNonQuery(CommandType.Text, sql01);

                    if (dt != null)
                    {
                        string sql02 = string.Format(@"SELECT CONTAINER_KEY,CONTAINER_CODE FROM AWMS_CONTAINER WHERE STATUS = '0' AND CONTAINER_CODE ='{0}'", hsTable["CONTAINER_CODE"].ToString());
                        DataSet ds02 = db2.ExecuteDataSet(CommandType.Text, sql02);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sql03 = string.Format(@"INSERT INTO  AWMS_CONTAINER_DETAIL(CONTAINER_KEY,CONTAINER_CODE,PALLET_NO,CREATOR,CDATE,STATUS)
                                                        VALUES('{0}','{1}','{2}','{3}',GETDATE(),'1')",
                                                                ds02.Tables[0].Rows[0]["CONTAINER_KEY"].ToString(),
                                                                ds02.Tables[0].Rows[0]["CONTAINER_CODE"].ToString(),
                                                                dt.Rows[i]["PALLET_NO"],
                                                                hsTable["CREATOR"].ToString()
                                                                );
                            db2.ExecuteNonQuery(CommandType.Text, sql03);
                        }

                        DataTable dt04 = dt;
                        string strPlus = string.Empty;
                        for (int i = 0; i < dt04.Rows.Count; i++)
                        {
                            strPlus += "'" + dt04.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                        }
                        if (string.IsNullOrEmpty(strPlus))
                        {
                            strPlus = "'',";
                        }
                        string str = strPlus.Substring(0, strPlus.Length - 1);
                        string sql04 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '1' WHERE PALLET_NO IN({0})",
                                                            str
                                                            );
                        db.ExecuteNonQuery(CommandType.Text, sql04);
                        dbTrans.Commit();

                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);

                    }

                }
                #endregion

                //  有柜组柜
                //  明细表中没有托信息则不执行1  2两步
                //1.删除明细表中该柜主键的所有托信息
                //2.修改包装表中对应的托状态为未组柜0
                //3.新增组柜托信息到明细表
                //4.修改包装表中对应托状态为已组柜1
                else if (flag == 0)
                {
                    string key = UtilHelper.GenerateNewKey(0);
                    DataTable dtHash = ds.Tables["HASH"];
                    Hashtable hsTable = FanHai.Hemera.Share.Common.CommonUtils.ConvertToHashtable(dtHash);

                    #region
                    if (dt != null)
                    {
                        string sql01 = string.Format(@"SELECT CONTAINER_KEY,CONTAINER_CODE FROM AWMS_CONTAINER WHERE STATUS = '0' AND CONTAINER_CODE ='{0}'"
                                                        , hsTable["CONTAINER_CODE"].ToString());
                        DataSet ds01 = db2.ExecuteDataSet(CommandType.Text, sql01);
                        if (ds01 != null)
                        {
                            if (ds01.Tables[0].Rows.Count == 1)
                            {
                                string sql02 = string.Format(@"SELECT PALLET_NO FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                , ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                                DataSet ds02 = db2.ExecuteDataSet(CommandType.Text, sql02);


                                string sql03 = string.Format(@"DELETE FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                , ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                                db2.ExecuteNonQuery(CommandType.Text, sql03);

                                string strPlus04 = string.Empty;
                                DataTable dt04 = ds02.Tables[0];
                                for (int i = 0; i < dt04.Rows.Count; i++)
                                {
                                    strPlus04 += "'" + dt04.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                                }
                                if (string.IsNullOrEmpty(strPlus04))
                                {
                                    strPlus04 = "'',";
                                }
                                string str04 = strPlus04.Substring(0, strPlus04.Length - 1);

                                string sql04 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '0' WHERE PALLET_NO IN({0})"
                                                            , str04);
                                db.ExecuteNonQuery(CommandType.Text, sql04);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string sql05 = string.Format(@"INSERT INTO  AWMS_CONTAINER_DETAIL(CONTAINER_KEY,CONTAINER_CODE,PALLET_NO,CREATOR,CDATE,STATUS)
                                                                VALUES('{0}','{1}','{2}','{3}',GETDATE(),'1')",
                                                                        ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString(),
                                                                        ds01.Tables[0].Rows[0]["CONTAINER_CODE"].ToString(),
                                                                        dt.Rows[i]["PALLET_NO"],
                                                                        hsTable["CREATOR"].ToString()
                                                                        );
                                    db2.ExecuteNonQuery(CommandType.Text, sql05);
                                }

                                DataTable dt06 = dt;
                                string strPlus06 = string.Empty;
                                for (int i = 0; i < dt06.Rows.Count; i++)
                                {
                                    strPlus06 += "'" + dt06.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                                }
                                if (string.IsNullOrEmpty(strPlus06))
                                {
                                    strPlus06 = "'',";
                                }
                                string str06 = strPlus06.Substring(0, strPlus06.Length - 1);
                                string sql06 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '1' WHERE PALLET_NO IN({0})",
                                                                    str06
                                                                    );
                                db.ExecuteNonQuery(CommandType.Text, sql06);

                            }
                            else
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string sql05 = string.Format(@"INSERT INTO  AWMS_CONTAINER_DETAIL(CONTAINER_KEY,CONTAINER_CODE,PALLET_NO,CREATOR,CDATE,STATUS)
                                                                VALUES('{0}','{1}','{2}','{3}',GETDATE(),'1')",
                                                                        ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString(),
                                                                        ds01.Tables[0].Rows[0]["CONTAINER_CODE"].ToString(),
                                                                        dt.Rows[i]["PALLET_NO"],
                                                                        hsTable["CREATOR"].ToString()
                                                                        );
                                    db2.ExecuteNonQuery( CommandType.Text, sql05);
                                }

                                DataTable dt06 = dt;
                                string strPlus06 = string.Empty;
                                for (int i = 0; i < dt06.Rows.Count; i++)
                                {
                                    strPlus06 += "'" + dt06.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                                }
                                if (string.IsNullOrEmpty(strPlus06))
                                {
                                    strPlus06 = "'',";
                                }
                                string str06 = strPlus06.Substring(0, strPlus06.Length - 1);
                                string sql06 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '1' WHERE PALLET_NO IN({0})",
                                                                    str06
                                                                    );
                                db.ExecuteNonQuery(CommandType.Text, sql06);
                            }

                        }              
                        dbTrans.Commit();

                        FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                    }
                    #endregion
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (dt == null)
                    {
                        string sql01 = string.Format(@"SELECT CONTAINER_KEY,CONTAINER_CODE FROM AWMS_CONTAINER WHERE STATUS = '0' AND CONTAINER_CODE ='{0}'"
                                                        , hsTable["CONTAINER_CODE"].ToString());
                        DataSet ds01 = db2.ExecuteDataSet(CommandType.Text, sql01);
                        if (ds01 != null)
                        {
                            if (ds01.Tables[0].Rows.Count == 1)
                            {
                                string sql02 = string.Format(@"SELECT PALLET_NO FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                , ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                                DataSet ds02 = db2.ExecuteDataSet(CommandType.Text, sql02);


                                string sql03 = string.Format(@"DELETE FROM AWMS_CONTAINER_DETAIL WHERE CONTAINER_KEY = '{0}'"
                                                                , ds01.Tables[0].Rows[0]["CONTAINER_KEY"].ToString());
                                db2.ExecuteNonQuery(CommandType.Text, sql03);

                                string strPlus04 = string.Empty;
                                DataTable dt04 = ds02.Tables[0];
                                for (int i = 0; i < dt04.Rows.Count; i++)
                                {
                                    strPlus04 += "'" + dt04.Rows[i]["PALLET_NO"].ToString().Trim() + "',";
                                }
                                if (string.IsNullOrEmpty(strPlus04))
                                {
                                    strPlus04 = "'',";
                                }
                                string str04 = strPlus04.Substring(0, strPlus04.Length - 1);

                                string sql04 = string.Format(@"UPDATE WIP_CONSIGNMENT SET ARK_FLAG = '0' WHERE PALLET_NO IN({0})"
                                                            , str04);
                                db.ExecuteNonQuery(CommandType.Text, sql04);

                                dbTrans.Commit();

                                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("UpdateArkInf Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region IGroupArkEngine 成员

        //有无组柜信息查询
        public DataSet QueryInf(string _arkCode, string _status, string _palletNo)
        {

            DataSet dsReturn = new DataSet();
            string _arkFlag = string.Empty;
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                sBuilder.Append(@"SELECT B.CONTAINER_CODE,A.PALLET_NO,A.SAP_NO,A.WORKNUMBER,B.CREATOR,B.CDATE,CASE WHEN A.ARK_FLAG ='1' THEN '已组柜' ELSE '未组柜' END AS ARK_FLAG
                                        from dbo.WIP_CONSIGNMENT A
                                        left join AWMS.dbo.AWMS_CONTAINER_DETAIL B
                                        on A.PALLET_NO = B.PALLET_NO
                                        WHERE A.ISFLAG = 1 AND A.CS_DATA_GROUP = '3'");

                if (!string.IsNullOrEmpty(_arkCode))
                {
                    sBuilder.AppendFormat(" AND B.CONTAINER_CODE = '{0}'", _arkCode.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_status))
                {
                    if (_status == "已组柜")
                    {
                        _arkFlag = "1";
                    }
                    else if (_status == "未组柜")
                    {
                        _arkFlag = "0";
                    }
                    sBuilder.AppendFormat(" AND A.ARK_FLAG  = '{0}'", _arkFlag.PreventSQLInjection());
                }
                if (!string.IsNullOrEmpty(_palletNo))
                {
                    string palletNo = _palletNo;
                    if (!string.IsNullOrEmpty(palletNo))
                    {
                        string palletNos = UtilHelper.BuilderWhereConditionString("A.PALLET_NO", palletNo.Split(new char[] { ',', '\n', '#' }));
                        sBuilder.AppendFormat(palletNos);
                    }
                }
                sBuilder.Append(" ORDER BY A.ARK_FLAG DESC");
                dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("QueryInf Error: " + ex.Message);
            }
            return dsReturn; 
        }

        #endregion

        #region IGroupArkEngine 成员


        public DataSet QueryInfHaveArked(string _teEntryNo, string _lciStatus)
        {
            DataSet dsReturn = new DataSet();
            string _arkFlag = string.Empty;
            try
            {
//                string sql01 = @"SELECT ZMBLNR,WERKS,[STATUS],ZMMTYP,AUFNR,VBELN_OEM,DEPT,CREATOR,CDATE FROM AWMS.dbo.AWMS_WH_ENTRY
//                                    WHERE [STATUS] = '正常'";
                string sql01 = @"SELECT ZMBLNR,WERKS,[STATUS],ZMMTYP,AUFNR,VBELN_OEM,DEPT,CREATOR,CDATE FROM AWMS.dbo.AWMS_WH_ENTRY
                                    WHERE [STATUS] = '正常' and ZMBLNR in(
                                    SELECT a.ZMBLNR FROM AWMS.dbo.AWMS_WH_ENTRY a
                                    inner join AWMS.dbo.AWMS_WH_ENTRY_DETAIL  b on a.zmblnr=b.zmblnr
                                    WHERE a.[STATUS] = '正常' and b.ITEMSTATUS = '正常') ";
                if (!string.IsNullOrEmpty(_teEntryNo))
                {
                    sql01 += string.Format(@" AND ZMBLNR = '{0}'", _teEntryNo);
                } 
                DataSet ds01 = db2.ExecuteDataSet(CommandType.Text, sql01);
                DataTable dt01 = ds01.Tables[0];
                dt01.TableName = "AWMS_WH_ENTRY";
                dsReturn.Merge(dt01);

//                string sql02 = @"SELECT B.CONTAINER_CODE AS 柜号,A.PALLET_NO AS 托号,A.SAP_NO AS 料号,A.WORKNUMBER AS 工单号,B.CREATOR AS 创建人,B.CDATE AS 创建时间,CASE WHEN A.ARK_FLAG ='1' THEN '已组柜' ELSE '未组柜' END AS 状态,A.LOT_NUMBER_QTY AS 数量
//                                    from dbo.WIP_CONSIGNMENT A
//                                    left join AWMS.dbo.AWMS_CONTAINER_DETAIL B
//                                    on A.PALLET_NO = B.PALLET_NO
//                                    WHERE A.ISFLAG = 1 AND A.CS_DATA_GROUP = '3'
//                                   ";
                string sql02 = @"SELECT B.CONTAINER_CODE AS 柜号,A.PALLET_NO AS 托号,A.SAP_NO AS 料号,A.WORKNUMBER AS 工单号,B.CREATOR AS 创建人,B.CDATE AS 创建时间,CASE WHEN A.ARK_FLAG ='1' THEN '已组柜' ELSE '未组柜' END AS 状态,A.LOT_NUMBER_QTY AS 数量
                                    from dbo.WIP_CONSIGNMENT A
                                    left join AWMS.dbo.AWMS_CONTAINER_DETAIL B
                                    on A.PALLET_NO = B.PALLET_NO
                                    LEFT JOIN AWMS.dbo.AWMS_WH_ENTRY_DETAIL C
                                    ON A.PALLET_NO = C.XP004
                                    WHERE A.ISFLAG = 1 AND A.CS_DATA_GROUP = '3' AND C.ITEMSTATUS = '正常'
                                    ";
                if (!string.IsNullOrEmpty(_teEntryNo))
                {
                    sql02 += string.Format(@" AND A.PALLET_NO IN (SELECT PALLET_NO FROM dbo.WIP_CONSIGNMENT WHERE CONSIGNMENT_KEY IN (
                                    SELECT KEYNO FROM AWMS.dbo.AWMS_WH_ENTRY_DETAIL WHERE ZMBLNR IN ('{0}'))) ", _teEntryNo);
                }
                if (!string.IsNullOrEmpty(_lciStatus))
                {
                    if (_lciStatus == "已组柜")
                    {
                        _arkFlag = "1";
                    }
                    else if (_lciStatus == "未组柜")
                    {
                        _arkFlag = "0";
                    }
                    sql02 += string.Format(@" AND A.ARK_FLAG  = '{0}'", _lciStatus);
                }
                DataSet ds02 = db2.ExecuteDataSet(CommandType.Text, sql02);
                DataTable dt02 = ds02.Tables[0];
                dt02.TableName = "ARK_PALLET";
                dsReturn.Merge(dt02);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("QueryInfHaveArked Error: " + ex.Message);
            }
            return dsReturn; 
        }

        #endregion
    }
}

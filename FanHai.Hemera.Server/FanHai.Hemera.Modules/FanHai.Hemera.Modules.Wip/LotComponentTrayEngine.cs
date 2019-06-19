using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;


namespace FanHai.Hemera.Modules.Wip
{
    public partial class LotComponentTrayEngine : AbstractEngine, ILotComponentTrayEngine
    {
        private Database db = null; //数据库操作对象。
        private Database _dbRead = null;

        public LotComponentTrayEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        #region ILotComponentTrayEngine 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertComponentTray(LotCustomerModel model)
        {
            int count = 0;
            bool isBool = false;

            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbTrans = dbconn.BeginTransaction();

                try
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("INSERT INTO dbo.POR_COMPONENT_TRAY_LIST ");
                    sbSql.Append(" ( TrayName ,TrayValue ,LotNumber ,LineKey ,LineName ,PackageNumber ,Number ,Color ,PsKey ,SubPowerlevel ,WorkOrderNo ,PatrNumber ,GradeName ,CreateTime,VirtualCustomerNumber,IsFlip,IsPack)");
                    sbSql.Append(" VALUES ");
                    sbSql.Append("(@TrayName ,@TrayValue ,@LotNumber ,@LineKey ,@LineName ,@PackageNumber ,@Number ,@Color ,@PsKey ,@SubPowerlevel ,@WorkOrderNo ,@PatrNumber ,@GradeName ,@CreateTime,@VirtualCustomerNumber,@IsFlip,@IsPack)");

                    SqlParameter[] parameters = {
					                new SqlParameter("@TrayName", SqlDbType.VarChar,50),
					                new SqlParameter("@TrayValue", SqlDbType.VarChar,50),
					                new SqlParameter("@LotNumber", SqlDbType.VarChar,50),
					                new SqlParameter("@LineKey", SqlDbType.VarChar,50),
					                new SqlParameter("@LineName", SqlDbType.VarChar,50),
					                new SqlParameter("@PackageNumber", SqlDbType.VarChar,50),
					                new SqlParameter("@Number", SqlDbType.VarChar,50),
					                new SqlParameter("@Color", SqlDbType.VarChar,50),
					                new SqlParameter("@PsKey", SqlDbType.VarChar,50),
					                new SqlParameter("@SubPowerlevel", SqlDbType.VarChar,50),
                                    new SqlParameter("@WorkOrderNo", SqlDbType.VarChar,50),
					                new SqlParameter("@PatrNumber", SqlDbType.VarChar,50),
					                new SqlParameter("@GradeName", SqlDbType.VarChar,50),
					                new SqlParameter("@CreateTime", SqlDbType.VarChar,50),
                                    new SqlParameter("@VirtualCustomerNumber", SqlDbType.VarChar,50),
                                    new SqlParameter("@IsFlip", SqlDbType.VarChar,50),
                                    new SqlParameter("@IsPack", SqlDbType.VarChar,50)};
                    parameters[0].Value = model.TrayText;
                    parameters[1].Value = model.TrayValue;
                    parameters[2].Value = model.LotNumber;
                    parameters[3].Value = model.LineKey;
                    parameters[4].Value = model.LineName;
                    parameters[5].Value = model.PackageNumber;
                    parameters[6].Value = model.Number;
                    parameters[7].Value = model.Color;
                    parameters[8].Value = model.PsKey;
                    parameters[9].Value = model.SubPowerlevel;
                    parameters[10].Value = model.WorkOrderNo;
                    parameters[11].Value = model.PatrNumber;
                    parameters[12].Value = model.GradeName;
                    parameters[13].Value = DateTime.Now.ToString();
                    parameters[14].Value = model.VirtualCustomerNumber;
                    parameters[15].Value = model.IsFlip;
                    parameters[16].Value = model.IsPack;
                    DbCommand com = db.GetSqlStringCommand(sbSql.ToString()); //初始化command
                    for (int i = 0; i < parameters.Length; i++)    //参数填充
                    {
                        db.AddInParameter(com, parameters[i].ParameterName, parameters[i].DbType, parameters[i].Value);
                    }

                    count = db.ExecuteNonQuery(com); //执行

                    if (count > 0)
                    {
                        isBool = true;
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw ex;
                }
                finally
                {
                    dbTrans.Dispose();
                    dbconn.Close();
                    dbconn.Dispose();
                }
            }

            return isBool;
        }

        public DataSet SelectComponentTray(string trayValue)
        {
            DataSet dsReturn = new DataSet();
            int intReturn = 0;

            using (DbConnection dbconn = db.CreateConnection())
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("SELECT TOP 50 * FROM dbo.POR_COMPONENT_TRAY_LIST ");
                    strSql.Append("  WHERE TrayValue =@trayValue ");
                    strSql.Append(" ORDER BY CreateTime DESC");

                    SqlParameter[] parameters = { new SqlParameter("@trayValue", SqlDbType.NVarChar, 20) };
                    parameters[0].Value = trayValue;

                    DbCommand com = db.GetSqlStringCommand(strSql.ToString());

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        db.AddInParameter(com, parameters[i].ParameterName, parameters[i].DbType, parameters[i].Value);
                    }

                    dsReturn = db.ExecuteDataSet(com);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetInfoForLotHistory Error: " + ex.Message);
                }
                finally
                {
                    dbconn.Close();
                }
            }

            return dsReturn;
        }

        public DataSet SelectComponentTrayLine(string trayValue,string linekey)
        {
            DataSet dsReturn = new DataSet();
            int intReturn = 0;

            using (DbConnection dbconn = db.CreateConnection())
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("SELECT TOP 50 * FROM dbo.POR_COMPONENT_TRAY_LIST ");
                    strSql.Append("  WHERE TrayValue =@trayValue ");
                    strSql.Append("  AND LineKey =@linekey ");
                    strSql.Append(" ORDER BY CreateTime DESC");

                    SqlParameter[] parameters = { 
                                                    new SqlParameter("@trayValue", SqlDbType.NVarChar, 20),
                                                    new SqlParameter("@linekey", SqlDbType.NVarChar, 40)
                                                };
                    parameters[0].Value = trayValue;
                    parameters[1].Value = linekey;

                    DbCommand com = db.GetSqlStringCommand(strSql.ToString());

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        db.AddInParameter(com, parameters[i].ParameterName, parameters[i].DbType, parameters[i].Value);
                    }

                    dsReturn = db.ExecuteDataSet(com);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("GetInfoForLotHistory Error: " + ex.Message);
                }
                finally
                {
                    dbconn.Close();
                }
            }

            return dsReturn;
        }


        public bool UpdateOpcValue(string strAddress, string strValue,string strDateTime)
        {
            int count = 0;
            bool isBool = false;

            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbTrans = dbconn.BeginTransaction();

                try
                {
                    StringBuilder sbSql = new StringBuilder();

                    sbSql.Append("UPDATE dbo.POR_OPC_ADDRESS ");
                    sbSql.Append("SET Value=@Value,[type]=@type ");
                    sbSql.Append(" WHERE [Address] = @Address");

                    Random r = new Random();

                    SqlParameter[] parameters = {
					                new SqlParameter("@Value", SqlDbType.VarChar,50),
					                new SqlParameter("@Address", SqlDbType.VarChar,50),
                                    new SqlParameter("@type", SqlDbType.VarChar,64) };

                    parameters[0].Value = strValue;
                    parameters[1].Value = strAddress;
                    parameters[2].Value = r.Next(999999);

                    DbCommand com = db.GetSqlStringCommand(sbSql.ToString()); //初始化command
                    for (int i = 0; i < parameters.Length; i++)    //参数填充
                    {
                        db.AddInParameter(com, parameters[i].ParameterName, parameters[i].DbType, parameters[i].Value);
                    }

                    count = db.ExecuteNonQuery(com); //执行

                    if (count > 0)
                    {
                        isBool = true;
                    }

                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    throw ex;
                }
                finally
                {
                    dbTrans.Dispose();
                    dbconn.Close();
                    dbconn.Dispose();
                }
            }

            return isBool;
        }


        public int CreateTrayNouber()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 自动生成单据编号（传单据类型）
        /// </summary>
        /// <param name="strTxt"> 单据类型</param>
        /// <param name="isAdd"> 是否在数据库记录中加 1</param>
        /// <returns> 单据编号或 null</returns>
        public string GetShgCod(string strTxt, bool isAdd)
        {
            string billCode = string.Empty;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SP_US_GetShgCod");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 32));
                //----------------接收返回参数 BEGIN-------
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@billCode";
                parameter.SqlDbType = SqlDbType.VarChar;
                parameter.Size = 64;
                parameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parameter);
                //-----------------END---------------------
                cmd.Parameters.Add(new SqlParameter("@add", SqlDbType.Int, 4));
                cmd.Parameters.Add(new SqlParameter("@trans", SqlDbType.Int, 4));

                if (isAdd)
                    cmd.Parameters["@add"].Value = 1;
                else
                    cmd.Parameters["@add"].Value = 0;
                cmd.Parameters["@name"].Value = strTxt;
                cmd.Parameters["@trans"].Value = 0;

                db.ExecuteNonQuery(cmd);

                billCode = cmd.Parameters["@billCode"].Value.ToString();
            }
            catch (Exception ex)
            {
                LogService.LogError("LotOperationEngine.GetWOProductPowersetData Error: " + ex.Message);
            }

            return billCode;
        }

        #endregion

        
    }
}

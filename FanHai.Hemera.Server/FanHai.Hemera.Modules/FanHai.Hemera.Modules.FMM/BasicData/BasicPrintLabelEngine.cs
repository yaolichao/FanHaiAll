using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils;
using System.Data.Common;
using System.Collections;
using FanHai.Hemera.Share.Common;
using System.Transactions;

namespace FanHai.Hemera.Modules.FMM
{  
    /// <summary>
    /// 标签或者铭牌数据操作类
    /// </summary>
    public class BasicPrintLabelEngine : AbstractEngine, IBasicPrintLabelEngine
    {
        private Database _db = null; //数据库操作对象。
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BasicPrintLabelEngine()
        {
            this._db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 获取标签或铭牌数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetPrintLabelData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT LABEL_ID,VERSION_NO,LABEL_NAME,DATA_TYPE,PRINTER_TYPE,PRODUCT_MODEL,
                                      CERTIFICATE_TYPE,POWERSET_TYPE,CUSTCHECK_TYPE,IS_VALID,IS_USED,
                                      CREATE_TIME,CREATOR,EDIT_TIME,EDITOR
                               FROM BASE_PRINTLABEL 
                               WHERE IS_USED='Y'";
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetPrintLabelData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取标签或铭牌明细数据。
        /// </summary>
        /// <returns>包含标签或铭牌明细数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetPrintLabelDetailData(string labelId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT LABEL_ID,VERSION_NO,LABEL_NAME,DATA_TYPE,PRINTER_TYPE,PRODUCT_MODEL,
                                                  CERTIFICATE_TYPE,POWERSET_TYPE,CUSTCHECK_TYPE,IS_VALID,IS_USED,
                                                  CREATE_TIME,CREATOR,EDIT_TIME,EDITOR
                                           FROM BASE_PRINTLABEL 
                                           WHERE LABEL_ID='{0}'
                                           ORDER BY VERSION_NO", labelId.PreventSQLInjection());
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetPrintLabelDetailData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存标签或铭牌数据。
        /// </summary>
        /// <param name="dsParams">包含标签或铭牌数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.SavePrintLabelData(DataSet dsParams)
        {

            DataSet dsReturn = new DataSet();
            StringBuilder sbMessage = new StringBuilder();
            try
            {
                if(dsParams!=null && dsParams.Tables.Count>0 && dsParams.Tables[0].Rows.Count>0)
                {
                    DataTable dtData=dsParams.Tables[0];
                    using (TransactionScope tsCope = new TransactionScope())
                    {
                        
                        foreach (DataRow dr in dtData.Rows)
                        {
                            if (dr.RowState != DataRowState.Modified && dr.RowState != DataRowState.Added)
                            {
                                continue;
                            }

                            //检查记录是否过期。防止重复修改。
                            string checkSql = @"SELECT EDIT_TIME FROM BASE_PRINTLABEL 
                                                WHERE LABEL_ID=@labelId 
                                                AND VERSION_NO=@versionNo";
                            DbCommand checkCmd = this._db.GetSqlStringCommand(checkSql);
                            this._db.AddInParameter(checkCmd, "@labelId", DbType.String, dr["LABEL_ID"]);
                            this._db.AddInParameter(checkCmd, "@versionNo", DbType.Int32, dr["VERSION_NO"]);
                            object objTime = this._db.ExecuteScalar(checkCmd);

                            if (objTime != null && DBNull.Value!=objTime)
                            {
                                if (dr.RowState == DataRowState.Added)
                                {
                                    sbMessage.AppendFormat("ID：{0},版本号：{1} 已存在。\r\n", dr["LABEL_ID"], dr["VERSION_NO"]);
                                    break;
                                }
                                else
                                {
                                    //数据已过期。
                                    if (Convert.ToString(objTime) != Convert.ToString(dr["EDIT_TIME"]))
                                    {
                                        sbMessage.AppendFormat("ID：{0},数据过期。\r\n", dr["LABEL_ID"]);
                                        break;
                                    }
                                }
                            }

                            int versionNo = 1;
                            string isValid = "Y";

                            if(dr.RowState == DataRowState.Modified)
                            {
                                //更新对应产品ID和版本号数据不可用。
                                string updateSql = @"UPDATE BASE_PRINTLABEL SET IS_USED='N',EDIT_TIME=GETDATE(),EDITOR=@editor
                                                     WHERE LABEL_ID=@labelId AND VERSION_NO=@versionNo";
                                DbCommand updateCmd = this._db.GetSqlStringCommand(updateSql);
                                this._db.AddInParameter(updateCmd, "@labelId", DbType.String, dr["LABEL_ID"]);
                                this._db.AddInParameter(updateCmd, "@versionNo", DbType.Int32, dr["VERSION_NO"]);
                                this._db.AddInParameter(updateCmd, "@editor", DbType.String, dr["EDITOR"]);
                                this._db.ExecuteNonQuery(updateCmd);

                                versionNo = Convert.ToInt32(dr["VERSION_NO"])+1;
                                isValid = Convert.ToString(dr["IS_VALID"]);
                            }

                            string insertSql = @"INSERT INTO BASE_PRINTLABEL(LABEL_ID,VERSION_NO,LABEL_NAME,DATA_TYPE,PRINTER_TYPE,PRODUCT_MODEL,
                                                           CERTIFICATE_TYPE,POWERSET_TYPE,CUSTCHECK_TYPE,IS_VALID,IS_USED,CREATE_TIME,CREATOR,EDIT_TIME,EDITOR)
                                                 VALUES(@labelId,@versionNo,@labelName,@dataType,@printerType,@productModel,
                                                           @certificateType,@powersetType,@custCheckType,@isValid,'Y',GETDATE(),@creator,GETDATE(),@editor)";
                            DbCommand cmd = this._db.GetSqlStringCommand(insertSql);
                            this._db.AddInParameter(cmd, "@labelId", DbType.String, dr["LABEL_ID"]);
                            this._db.AddInParameter(cmd, "@versionNo", DbType.Int32, versionNo);
                            this._db.AddInParameter(cmd, "@labelName", DbType.String, dr["LABEL_NAME"]);
                            this._db.AddInParameter(cmd, "@isValid", DbType.String, isValid);
                            this._db.AddInParameter(cmd, "@dataType", DbType.String, dr["DATA_TYPE"]);
                            this._db.AddInParameter(cmd, "@printerType", DbType.String, dr["PRINTER_TYPE"]);
                            this._db.AddInParameter(cmd, "@productModel", DbType.String, dr["PRODUCT_MODEL"]);
                            this._db.AddInParameter(cmd, "@certificateType", DbType.String, dr["CERTIFICATE_TYPE"]);
                            this._db.AddInParameter(cmd, "@powersetType", DbType.String, dr["POWERSET_TYPE"]);
                            this._db.AddInParameter(cmd, "@custCheckType", DbType.String, dr["CUSTCHECK_TYPE"]);
                            this._db.AddInParameter(cmd, "@creator", DbType.String, dr["CREATOR"]);
                            this._db.AddInParameter(cmd, "@editor", DbType.String, dr["EDITOR"]);
                            this._db.ExecuteNonQuery(cmd);
                        }
                        if (sbMessage.Length<=0)
                        {
                            tsCope.Complete();
                        }
                    }
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMessage.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.SavePrintLabelData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 是否允许设置标签或数据为无效数据。
        /// </summary>
        /// <param name="labelId">标签或铭牌ID</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.IsAllowInvalid(string labelId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql =string.Format(@"SELECT DISTINCT a.PRODUCT_CODE
                            FROM POR_PRODUCT a
                            INNER JOIN BASE_TESTRULE b ON a.PRO_TEST_RULE=b.TESTRULE_CODE AND b.ISFLAG=1
                            INNER JOIN BASE_TESTRULE_DECAY c ON c.TESTRULE_KEY=b.TESTRULE_KEY AND c.ISFLAG=1
                            INNER JOIN BASE_TESTRULE_PRINTSET d ON d.DECAY_KEY=c.DECAY_KEY AND d.ISFLAG=1
                            WHERE a.ISFLAG=1
                            AND d.VIEW_NAME='{0}'",labelId.PreventSQLInjection());
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                StringBuilder sbMsg = new StringBuilder();
                if (dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                {
                    sbMsg.AppendFormat("产品：");
                    for (int i = 0; i < dsReturn.Tables[0].Rows.Count;i++ )
                    {
                        if (i == 10)
                        {
                            sbMsg.AppendFormat("等{0}个产品ID", dsReturn.Tables[0].Rows.Count);
                            break;
                        }
                        DataRow dr = dsReturn.Tables[0].Rows[i];
                        sbMsg.AppendFormat("{0} ", dr["PRODUCT_CODE"]);
                    }
                    sbMsg.AppendFormat("设置了此ID，请确认。");
                }
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, sbMsg.ToString());
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.IsAllowInvalid Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取产品型号数据。
        /// </summary>
        /// <returns>包含产品型号数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetProductModelData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT PROMODEL_NAME
                            FROM BASE_PRODUCTMODEL
                            WHERE ISFLAG=1
                            ORDER BY PROMODEL_NAME";
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetProductModelData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取认证类型数据。
        /// </summary>
        /// <returns>包含认证类型数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetCertificateTypeData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT CERTIFICATION
                               FROM POR_PRODUCT
                               WHERE CERTIFICATION IS NOT NULL
                               ORDER BY CERTIFICATION";
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetCertificateTypeData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取分档方式数据。
        /// </summary>
        /// <returns>包含分档方式数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetPowersetTypeData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT TOLERANCE
                            FROM POR_PRODUCT
                            WHERE TOLERANCE IS NOT NULL
                            ORDER BY TOLERANCE";
                dsReturn = this._db.ExecuteDataSet(CommandType.Text, sql);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetPowersetTypeData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取标签或铭牌数据类型数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据类型数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetLabelDataTypeData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CODE", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Rows.Add("L", "L:功率标签");
                dt.Rows.Add("P", "P:组件铭牌");
                dsReturn.Tables.Add(dt);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetLabelDataTypeData Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取打印机类型数据。
        /// </summary>
        /// <returns>包含打印机类型数据的数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetPrinterTypeData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CODE", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Rows.Add("0", "A:立象打印机");
                dt.Rows.Add("1", "Z:斑马打印机");
                dt.Rows.Add("2", "ZN:斑马网络打印机");
                dsReturn.Tables.Add(dt);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetPrinterTypeData Error: " + ex.Message);
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取铭牌检验类型数据。
        /// </summary>
        /// <returns>包含获取铭牌检验类型数据集对象。</returns>
        DataSet IBasicPrintLabelEngine.GetCustCheckTypeData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CODE", typeof(string));
                dt.Columns.Add("NAME", typeof(string));
                dt.Rows.Add("", "");
                dt.Rows.Add("0", "0:序列号");
                dt.Rows.Add("1", "1:常规");
                dt.Rows.Add("2", "2:Conergy");
                dsReturn.Tables.Add(dt);
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("BasicPrintLabelEngine.GetPrinterCheckTypeData Error: " + ex.Message);
            }
            return dsReturn;
        }

    }
}


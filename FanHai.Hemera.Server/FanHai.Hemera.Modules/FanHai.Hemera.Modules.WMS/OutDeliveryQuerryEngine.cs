using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Share.Interface;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils.StaticFuncs;

namespace FanHai.Hemera.Modules.WMS
{
    public class OutDeliveryQuerryEngine : AbstractEngine, IOutDeliveryQuerryEngine
    {
        private Database db;                              //用于数据库访问的变量   
        //private Database db1;                             //用于数据库访问的变量   
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OutDeliveryQuerryEngine()
        {
            db = DatabaseFactory.CreateDatabase("SQLServerAwms");
            //db1 = DatabaseFactory.CreateDatabase("SQLServer");
        }
        
        #region IOutDeliveryQuerryEngine 成员

        public DataSet OutDeliveryQuerry(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.Append(@"select a.*,
                                ID,POSNR,b.STATUS as STATUS2,b.ERDAT as ERDAT2,PLANT,STORAGELOCATION,CPBH,XHGG,
                                OBDQTY,PICKINGQTY,QCQTY,POSTQTY,UNIT,VSTEL,REF_NO,REF_ITEM,LAST_CNG_DATE,
                                LAST_CNG_BY,QCDATE,QC_PERSON,BATCHNO,QC_NO,QC_ITEM,QC_RESULT,Cabinet_NO,
                                Cabinet_KEY,DemandNO,DemardPOS,DemardSch,PalletNO
                                from AWMS_OUTB_HEADER a,AWMS_OUTB_ITEM b where a.OUTBANDNO=b.OUTBANDNO and a.VBELN=b.VBELN ");

            if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
            {
                DataTable dtParams = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                Hashtable htParams = CommonUtils.ConvertToHashtable(dtParams);

                if (htParams.ContainsKey("OUTBANDNO"))
                {
                    string OUTBANDNO = Convert.ToString(htParams["OUTBANDNO"]);
                    sBuilder.AppendFormat(" AND a.OUTBANDNO = '{0}'", OUTBANDNO.PreventSQLInjection());
                }

                if (htParams.ContainsKey("VBELN"))
                {
                    string VBELN = Convert.ToString(htParams["VBELN"]);
                    sBuilder.AppendFormat(" AND a.VBELN = '{0}'", VBELN.PreventSQLInjection());
                }

                if (htParams.ContainsKey("CREATED_BY"))
                {
                    string CREATED_BY = Convert.ToString(htParams["CREATED_BY"]);
                    sBuilder.AppendFormat(" AND CREATED_BY = '{0}'", CREATED_BY.PreventSQLInjection());
                }

                if (htParams.ContainsKey("SALESTO"))
                {
                    string SALESTO = Convert.ToString(htParams["SALESTO"]);
                    sBuilder.AppendFormat(" AND SALESTO LIKE '{0}%'", SALESTO.PreventSQLInjection());
                }

                if (htParams.ContainsKey("XHGG"))
                {
                    string XHGG = Convert.ToString(htParams["XHGG"]);
                    sBuilder.AppendFormat(" AND (XHGG LIKE '{0}%' or CPBH LIKE '{0}%')", XHGG.PreventSQLInjection());
                }

                if (htParams.ContainsKey("PLANT"))
                {
                    string PLANT = Convert.ToString(htParams["PLANT"]);
                    sBuilder.AppendFormat(" AND PLANT= '{0}'", PLANT.PreventSQLInjection());
                }

                if (htParams.ContainsKey("STATUS"))
                {
                    string STATUS = Convert.ToString(htParams["STATUS"]);
                    sBuilder.AppendFormat(" AND b.STATUS= '{0}'", STATUS.PreventSQLInjection());
                }

                if (htParams.ContainsKey("BATCHNO"))
                {
                    string BATCHNO = Convert.ToString(htParams["BATCHNO"]);
                    sBuilder.AppendFormat(" AND BATCHNO= '{0}'", BATCHNO.PreventSQLInjection());
                }

                if (htParams.ContainsKey("Cabinet_NO"))
                {
                    string Cabinet_NO = Convert.ToString(htParams["Cabinet_NO"]);
                    sBuilder.AppendFormat(" AND Cabinet_NO= '{0}'", Cabinet_NO.PreventSQLInjection());
                }

                if (htParams.ContainsKey("CI"))
                {
                    string CI = Convert.ToString(htParams["CI"]);
                    sBuilder.AppendFormat(" AND CI= '{0}'", CI.PreventSQLInjection());
                }

                if (htParams.ContainsKey("ShipmentNo"))
                {
                    string ShipmentNo = Convert.ToString(htParams["ShipmentNo"]);
                    sBuilder.AppendFormat(" AND ShipmentNo like '{0}%'", ShipmentNo.PreventSQLInjection());
                }

                if (htParams.ContainsKey("LFDAT1"))
                {
                    string LFDAT1 = Convert.ToString(htParams["LFDAT1"]);
                    sBuilder.AppendFormat(" AND LFDAT >= '{0}'", LFDAT1.PreventSQLInjection());
                }

                if (htParams.ContainsKey("LFDAT2"))
                {
                    string LFDAT2 = Convert.ToString(htParams["LFDAT2"]);
                    sBuilder.AppendFormat(" AND LFDAT <= '{0}'", LFDAT2.PreventSQLInjection());
                }

                if (htParams.ContainsKey("QCDAT1"))
                {
                    string QCDAT1 = Convert.ToString(htParams["QCDAT1"]);
                    sBuilder.AppendFormat(" AND QCDAT >= '{0}'", QCDAT1.PreventSQLInjection());
                }

                if (htParams.ContainsKey("QCDAT2"))
                {
                    string QCDAT2 = Convert.ToString(htParams["QCDAT2"]);
                    sBuilder.AppendFormat(" AND QCDAT <= '{0}'", QCDAT2.PreventSQLInjection());
                }
                sBuilder.Append(@" order by ID");
            }
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
                if (dsReturn.Tables[0].Rows.Count == 0)
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "没有查询到相关的外向交货数据！");
                else
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("OutDeliveryQuerry Error: " + ex.Message);
            }
            return dsReturn;
         
        }

        public DataSet GetQCItem(string VBELN, string POSNR)
        {
            DataSet dsReturn = new DataSet();

            string sql = String.Format(@"select * from AWMS_OUTB_QC where VBELN =  '{0}' and POSNR = '{1}'
                                            ORDER BY ID",
                                            VBELN.PreventSQLInjection(), 
                                            POSNR.PreventSQLInjection ());
            try
            {
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                if (dsReturn.Tables[0].Rows.Count == 0)
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, "没有查询到相关的质检明细数据！");
                else
                    ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("OutDeliveryQuerry Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using System.Data;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.DatabaseHelper;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;


namespace FanHai.Hemera.Modules.WMS
{
    class PickOperationEngine : AbstractEngine, IPickOperationEngine
    {
         private Database db;                             //用于数据库访问的变量。
        /// <summary>
        /// 构造函数。
        /// </summary>
         public PickOperationEngine()
        {
            db = DatabaseFactory.CreateDatabase("SQLServerAwms");//SQLServerHis
        }        
         public override void Initialize() { }

         #region PickCtrl Member

         public string GetOutBNO(string Prefix, string tabName, string field)
         {
             string OutBNO = "";
             string tmpstr = "";

             StringBuilder sbSql = new StringBuilder();
             sbSql.AppendFormat(@" select convert(varchar,getdate(),112) as RQ ");
             string rq = (string)db.ExecuteScalar(CommandType.Text, sbSql.ToString()); //获取系统日期
             Prefix = Prefix + rq;
             sbSql = new StringBuilder();
             sbSql.AppendFormat(@" select  max(" + field + ") from " + tabName + " WHERE " + field + " like '" + Prefix + "%'");
             object oj = db.ExecuteScalar(CommandType.Text, sbSql.ToString());

             if (Convert.IsDBNull(oj))
             {
                 OutBNO = "";

             }
             else
             {
                 OutBNO = Convert.ToString(oj);
             }
             if (string.IsNullOrEmpty(OutBNO))
             {
                 OutBNO = Prefix + "0001";
             }
             else
             {
                 tmpstr = OutBNO.Substring(OutBNO.Length - 4, 4); //取出右边4位
                 tmpstr = (int.Parse(tmpstr) + 1).ToString("0000"); //单号加1
                 OutBNO = OutBNO.Substring(0, OutBNO.Length - 4) + tmpstr; //组合单号
             }
             return OutBNO;

         }
         public string SavePickHeaderData(DataTable dt_VBAK, string no)
         {
             try
             {
                 StringBuilder hdSql = new StringBuilder(); //写入出库抬头信息
                 hdSql.AppendFormat(@"INSERT INTO AWMS_OUTB_HEADER 
                                       (OUTBANDNO, VBELN, STATUS, ERDAT,
                                        CREATED_BY, SALESTO, SHIPTO, 
                                        SALESTO_NAME, SHIPTO_NAME,
                                        CI, ShipmentType, ShipmentNO 
                                        ) 
                                        VALUES
                                        (
                                        '{0}',
                                        '{1}',
                                        '{2}',
                                        getdate(),
                                        '{3}',                                    
                                        '{4}',
                                        '{5}',
                                        '{6}',
                                        '{7}',
                                        '{8}',
                                        '{9}',
                                        '{10}'

                                         )                                   
                                        ", no,
                                      "",
                                      "暂存",
                                      dt_VBAK.Rows[0]["BNAME"].ToString(),
                                      dt_VBAK.Rows[0]["KUNNR"].ToString(),
                                      dt_VBAK.Rows[0]["KNKLI"].ToString(),
                                      dt_VBAK.Rows[0]["KUNNR"].ToString(),
                                      dt_VBAK.Rows[0]["KNKLI"].ToString(),
                                      dt_VBAK.Rows[0]["CI"].ToString(),
                                      dt_VBAK.Rows[0]["ShipmentType"].ToString(),
                                      dt_VBAK.Rows[0]["ShipmentNO"].ToString()
                                      );

                 db.ExecuteNonQuery(CommandType.Text, hdSql.ToString());
             }
             catch (Exception ex)
             {
                 LogService.LogError("Save OutBand Develiery Error: " + ex.Message);
                 throw ex;
             }
             return "true";
         }
         public string SavePickItemData(DataSet ds, string sapvbeln,string outno)
         {
             DataTable dt = ds.Tables["RT_VBAP"];
             DataTable Dsave = new DataTable();
             Dsave.Columns.Add("ID", typeof(int));
             Dsave.Columns.Add("OUTBANDNO", typeof(string));
             Dsave.Columns.Add("VBELN", typeof(string));
             Dsave.Columns.Add("POSNR", typeof(string));
             Dsave.Columns.Add("STATUS", typeof(string));
             Dsave.Columns.Add("ERDAT", typeof(DateTime)); 
             Dsave.Columns.Add("PLANT", typeof(string));
             Dsave.Columns.Add("STORAGELOCATION", typeof(string));
             Dsave.Columns.Add("CPBH", typeof(string));
             Dsave.Columns.Add("XHGG", typeof(string));
             Dsave.Columns.Add("OBDQTY", typeof(string));
             Dsave.Columns.Add("PICKINGQTY", typeof(int));
             Dsave.Columns.Add("QCQTY", typeof(int));
             Dsave.Columns.Add("POSTQTY", typeof(int));
             Dsave.Columns.Add("UNIT", typeof(string));
             Dsave.Columns.Add("VSTEL", typeof(string));
             Dsave.Columns.Add("REF_NO", typeof(string));
             Dsave.Columns.Add("REF_ITEM", typeof(string));
             Dsave.Columns.Add("LAST_CNG_DATE", typeof(DateTime));
             Dsave.Columns.Add("LAST_CNG_BY", typeof(string));
             Dsave.Columns.Add("QCDATE", typeof(DateTime));
             Dsave.Columns.Add("QC_PERSON", typeof(string)); 
             Dsave.Columns.Add("BATCHNO", typeof(string));
             Dsave.Columns.Add("QC_NO", typeof(string));
             Dsave.Columns.Add("QC_ITEM", typeof(string));
             Dsave.Columns.Add("QC_RESULT", typeof(string));
             Dsave.Columns.Add("Cabinet_NO", typeof(string));
             Dsave.Columns.Add("Cabinet_KEY", typeof(string));
             Dsave.Columns.Add("DemandNO", typeof(string));
             Dsave.Columns.Add("DemardPOS", typeof(string));
             Dsave.Columns.Add("DemardSch", typeof(string)); 
             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 DataRow dr = Dsave.NewRow();
                 dr["OUTBANDNO"] = outno;
                 dr["VBELN"] = sapvbeln;
                 dr["POSNR"] = (i + 1).ToString();
                 dr["STATUS"] = "暂存";
                 dr["ERDAT"] = DateTime.Now;
                 dr["PLANT"] = dt.Rows[i]["WERKS"].ToString();
                 dr["STORAGELOCATION"] = dt.Rows[i]["LGORT"].ToString();
                 dr["CPBH"] = dt.Rows[i]["MATNR"].ToString();
                 dr["XHGG"] = dt.Rows[i]["ARKTX"].ToString();
                 dr["OBDQTY"] = Math.Floor(float.Parse(dt.Rows[i]["OutBQty"].ToString()));
                 dr["PICKINGQTY"] = Math.Floor(float.Parse(dt.Rows[i]["OutBQty"].ToString()));
                 dr["QCQTY"] = 0;
                 dr["POSTQTY"] =0 ;
                 dr["UNIT"] = dt.Rows[i]["VRKME"].ToString();
                 dr["VSTEL"] = dt.Rows[i]["VSTEL"].ToString();
                 dr["REF_NO"] = dt.Rows[i]["VBELN"].ToString();
                 dr["REF_ITEM"] = dt.Rows[i]["POSNR"].ToString();
                 dr["LAST_CNG_DATE"] = DateTime.Now;
                 dr["LAST_CNG_BY"] = dt.Rows[i]["ERNAM"].ToString();
                 dr["BATCHNO"] = dt.Rows[i]["CHARG"].ToString() ;
                 dr["Cabinet_NO"] = dt.Rows[i]["CONTAINER_CODE"].ToString();
                 dr["Cabinet_KEY"] = dt.Rows[i]["CONTAINER_KEY"].ToString();
                 dr["DemandNO"] = dt.Rows[i]["AUFNR"].ToString();
                 dr["DemardPOS"] = dt.Rows[i]["VGPOS"].ToString();
                 dr["DemardSch"] = dt.Rows[i]["GRKOR"].ToString();
                 Dsave.Rows.Add(dr);
             }
             SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(db.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
             sqlbulkcopy.DestinationTableName = "AWMS_OUTB_ITEM";//数据库中的表名
             //sqlbulkcopy.ColumnMappings.Add("OUTBANDNO", "OUTBANDNO");
             //sqlbulkcopy.ColumnMappings.Add("VBELN", "VBELN");
             //sqlbulkcopy.ColumnMappings.Add("POSNR", "POSNR");
             //sqlbulkcopy.ColumnMappings.Add("STATUS", "STATUS");
             //sqlbulkcopy.ColumnMappings.Add("ERDAT", "ERDAT");
             //sqlbulkcopy.ColumnMappings.Add("PLANT", "PLANT");
             //sqlbulkcopy.ColumnMappings.Add("STORAGELOCATION", "STORAGELOCATION");
             //sqlbulkcopy.ColumnMappings.Add("CPBH", "CPBH");
             //sqlbulkcopy.ColumnMappings.Add("XHGG", "XHGG");
             //sqlbulkcopy.ColumnMappings.Add("OBDQTY", "OUTBAOBDQTYNDNO");
             //sqlbulkcopy.ColumnMappings.Add("PICKINGQTY", "PICKINGQTY");
             //sqlbulkcopy.ColumnMappings.Add("QCQTY", "QCQTY");
             //sqlbulkcopy.ColumnMappings.Add("POSTQTY", "POSTQTY");
             //sqlbulkcopy.ColumnMappings.Add("UNIT", "UNIT");
             //sqlbulkcopy.ColumnMappings.Add("VSTEL", "VSTEL");
             //sqlbulkcopy.ColumnMappings.Add("REF_NO", "REF_NO");
             //sqlbulkcopy.ColumnMappings.Add("REF_ITEM", "REF_ITEM");
             //sqlbulkcopy.ColumnMappings.Add("LAST_CNG_DATE", "LAST_CNG_DATE");
             //sqlbulkcopy.ColumnMappings.Add("LAST_CNG_BY", "LAST_CNG_BY");
             //sqlbulkcopy.ColumnMappings.Add("BATCHNO", "BATCHNO");
             ////sqlbulkcopy.ColumnMappings.Add("Cabinet_NO", "Cabinet_NO");
             //sqlbulkcopy.ColumnMappings.Add("Cabinet_KEY", "Cabinet_KEY");
             //sqlbulkcopy.ColumnMappings.Add("DemandNO", "DemandNO");
             //sqlbulkcopy.ColumnMappings.Add("DemardPOS", "DemardPOS");
             //sqlbulkcopy.ColumnMappings.Add("DemardSch", "DemardSch");
             sqlbulkcopy.WriteToServer(Dsave);

             return "true";
         }

         public string SavePickData(DataSet ds, string NO)
         {

             string OUTBNO = GetOutBNO(NO, "AWMS_OUTB_HEADER", "OUTBANDNO");
             try
             {
                 DataTable dt = ds.Tables["RT_VBAP"]; 
                 SavePickHeaderData(ds.Tables["RT_VBAK"],OUTBNO);
                 SavePickItemData(ds, NO, OUTBNO);


                 StringBuilder UPSql = new StringBuilder();
                 UPSql.AppendFormat(@" UPDATE AWMS_OUTB_ITEM SET STATUS = '保存' WHERE OUTBANDNO = '{0}'
                                      UPDATE AWMS_OUTB_HEADER SET STATUS = '保存' WHERE OUTBANDNO = '{1}'   
                                    ", OUTBNO.PreventSQLInjection(), OUTBNO.PreventSQLInjection()); //保存成功，修改状态
                 db.ExecuteNonQuery(CommandType.Text, UPSql.ToString());
                 return OUTBNO;
             }
             catch (Exception ex)
             {
                 LogService.LogError("Save OutBand Develiery Error: " + ex.Message);
                 throw ex;
             }
         }

         public DataSet GetCabinetData(string _CabinetNo,out string msg)
         {
             DataSet dsReturn = new DataSet();
             try
             {

                 StringBuilder sbSql = new StringBuilder();
                 sbSql.AppendFormat(@"
                                    SELECT  
                                    AWMS_CONTAINER_DETAIL.CONTAINER_CODE AS CONTAINER_CODE,
                                    AWMS_CONTAINER_DETAIL.CONTAINER_KEY AS CONTAINER_KEY,
                                    AWMS_CONTAINER.STATUS AS STATUS,
                                    AWMS_WH_ENTRY_DETAIL.MATNR AS MATNR,
                                    AWMS_WH_ENTRY_DETAIL.MENGE AS MENGE,
                                    AWMS_WH_ENTRY_DETAIL.CHARG AS CHARG,
                                    AWMS_WH_ENTRY_DETAIL.XP001 AS XP001,
                                    AWMS_WH_ENTRY_DETAIL.XP002 AS XP002 ,
                                    AWMS_WH_ENTRY_DETAIL.XP003 AS XP003,
                                    AWMS_WH_ENTRY_DETAIL.XP004 AS XP004,
                                    AWMS_WH_ENTRY_DETAIL.XP005 AS XP005,
                                    AWMS_WH_ENTRY_DETAIL.XP006 AS XP006,
                                    AWMS_WH_ENTRY_DETAIL.XP007 AS XP007,
                                    AWMS_WH_ENTRY_DETAIL.XP008 AS XP008,
                                    AWMS_WH_ENTRY_DETAIL.XP009 AS XP009,
                                    AWMS_WH_ENTRY_DETAIL.XP010 AS XP010,
                                    'N' as IsUsed
                                    FROM AWMS_CONTAINER
                                    INNER JOIN AWMS_CONTAINER_DETAIL
                                    ON AWMS_CONTAINER.CONTAINER_KEY = AWMS_CONTAINER_DETAIL.CONTAINER_KEY 
                                    INNER JOIN AWMS_WH_ENTRY_DETAIL 
                                    ON AWMS_CONTAINER_DETAIL.PALLET_NO = AWMS_WH_ENTRY_DETAIL.XP004
                                    INNER JOIN  AWMS_WH_ENTRY
                                    ON AWMS_WH_ENTRY.ZMBLNR = AWMS_WH_ENTRY_DETAIL.ZMBLNR
                                    WHERE  AWMS_WH_ENTRY.STATUS='正常'
	                                    AND AWMS_WH_ENTRY_DETAIL.ITEMSTATUS='正常'
	                                    AND AWMS_CONTAINER.CONTAINER_CODE ='{0}'                                        
                                    ",
                                     _CabinetNo.PreventSQLInjection()
                                     
                                     );
                 //执行查询。
                 dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());                 
                 dsReturn.Tables[0].TableName = "CabinetData";
                 if (dsReturn.Tables[0].Rows.Count<=0 )
                 {
                     msg = "找不到对应的柜号！";
                     return null;
                 }
                 if (dsReturn.Tables[0].Rows[0]["STATUS"].ToString() != "0" )
                 {
                     msg = "此柜号已刷！";
                     return null;  
                 }
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
             catch (Exception ex)
             {
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("GetCabinetData Error: " + ex.Message);
             }
             msg = "";
             return dsReturn;
         }


         public bool DeleteOutBand(string _orderNO, int flag)
         {
             try
             {
                 StringBuilder sbSql = new StringBuilder();
                 string str = "";
                 switch (flag)
                 {
                     case 0:
                         str = " OUTBANDNO = '" + _orderNO+"'"; break;
                     case 1:
                         str = " VBELN = '" + _orderNO + "'"; break;
                 }
                 sbSql.AppendFormat(@" UPDATE AWMS_OUTB_HEADER SET STATUS='删除' WHERE " + @str);
                 sbSql.AppendFormat(@" UPDATE AWMS_OUTB_ITEM   SET STATUS='删除' WHERE " + @str);
                 sbSql.AppendFormat(@" UPDATE AWMS_CONTAINER  SET STATUS= 0 FROM  AWMS_OUTB_ITEM WHERE  "+ @str+@" AND 
                                                          AWMS_CONTAINER.CONTAINER_KEY = AWMS_OUTB_ITEM.Cabinet_KEY ");
                 db.ExecuteNonQuery(CommandType.Text, sbSql.ToString());
                 
             }
             catch (Exception ex)
             {
                 LogService.LogError("Delete OutBand NO fail: " + ex.Message);
                 throw ex;
             }
             return true;

         }


         public DataSet GetCRRelation()
         {
             DataSet dsReturn = new DataSet();
             try
             {

                 StringBuilder sbSql = new StringBuilder();
                 sbSql.AppendFormat(@" SELECT * FROM AWMS_ATINN");
                 dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                 dsReturn.Tables[0].TableName = "AWMS_ATINN";
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
             catch (Exception ex)
             {
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("GetCRRelation Error: " + ex.Message);
             }
             return dsReturn;

         }

         public DataSet ImportPickdata(string _outbandno)
         {

             DataSet dsReturn = new DataSet();
             try
             {

                 StringBuilder sbSql = new StringBuilder();
                 sbSql.AppendFormat(@"
                                    SELECT 
                                    REF_NO AS DOCUMENT_NUMB ,                                    
                                    REF_ITEM AS DOCUMENT_ITEM,
                                    POSNR,
                                    PLANT, 
                                    STORAGELOCATION AS STGE_LOC,                                    
                                    VSTEL AS LOADINGGRP,
                                    OBDQTY AS QUANTITY_SALES_UOM,
                                    CONVERT(VARCHAR,GETDATE(),112) AS DATE_USAGE,
                                    CONVERT(VARCHAR,GETDATE(),112) AS DATE,
                                    CONVERT(VARCHAR,GETDATE(),112) AS GOODS_ISSUE_DATE,
                                    CONVERT(VARCHAR,GETDATE(),112) AS LOADING_DATE,
                                    CONVERT(VARCHAR,GETDATE(),112) AS DELIVERY_DATE,
                                    CONVERT(VARCHAR,GETDATE(),108) AS TIME,
                                    UNIT AS SALES_UNIT,
                                    BATCHNO  AS BATCH 
                                    FROM AWMS_OUTB_ITEM WITH(NOLOCK)
                                    WHERE STATUS='保存'
                                    AND OUTBANDNO = '{0}'
                                    
                                    SELECT 
                                    * 
                                    FROM AWMS_OUTB_HEADER 
                                    WHERE STATUS='保存'
                                    AND OUTBANDNO = '{1}'
                                    
                                    SELECT TOP 1 *  FROM AWMS_CONTAINER WITH(NOLOCK) WHERE CONTAINER_KEY IN 
                                    ( SELECT DISTINCT Cabinet_KEY FROM AWMS_OUTB_ITEM WITH(NOLOCK)   WHERE OUTBANDNO = '{2}' AND STATUS='保存' )
                                    AND STATUS <> 0 
                                    ",
                                     _outbandno.PreventSQLInjection(),
                                     _outbandno.PreventSQLInjection(),
                                     _outbandno.PreventSQLInjection());
                 //执行查询。
                 dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                 dsReturn.Tables[0].TableName = "REQUEST";
                 dsReturn.Tables[1].TableName = "HEADER";
                 dsReturn.Tables[2].TableName = "CK_CONTAINER";

                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
             catch (Exception ex)
             {
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("ImportPickdata Error: " + ex.Message);
             }
             return dsReturn;

         }
         public void UpdateSapVbeln(string sapVbeln, string OutBNO)
         {

             StringBuilder UPSql = new StringBuilder();
             UPSql.AppendFormat(@" UPDATE AWMS_OUTB_ITEM SET VBELN = '{0}',STATUS='已确认'  WHERE OUTBANDNO = '{1}'
                                  UPDATE AWMS_OUTB_HEADER SET VBELN = '{2}',STATUS='已确认'  WHERE OUTBANDNO = '{3}'
                                  UPDATE AWMS_CONTAINER  SET STATUS= 1 FROM  AWMS_OUTB_ITEM WHERE  OUTBANDNO = '{4}' AND 
                                                          AWMS_CONTAINER.CONTAINER_KEY = AWMS_OUTB_ITEM.Cabinet_KEY
                                    ",
                                      sapVbeln.PreventSQLInjection(),
                                      OutBNO.PreventSQLInjection(),
                                      sapVbeln.PreventSQLInjection(),
                                      OutBNO.PreventSQLInjection(),
                                      OutBNO.PreventSQLInjection()
                                      ); //保存成功，修改状态
             db.ExecuteNonQuery(CommandType.Text, UPSql.ToString());
         }
         #endregion


         #region PickModify Member
         public DataSet GetPickedInfo(string _outbandno, string _Sapvbeln)
         {
             DataSet dsReturn = new DataSet();
             string _str = "";
             if (string.IsNullOrEmpty(_Sapvbeln) != true)
             {
                 _str += " and MXB.VBELN = '" + _Sapvbeln + "'";
             }
             if (string.IsNullOrEmpty(_outbandno) != true)
             {
                 _str += " and MXB.OUTBANDNO = '" + _outbandno + "'";
             }
             try
             {
                 StringBuilder sbSql = new StringBuilder();
                 sbSql.AppendFormat(@" SELECT  
                SALESTO,SHIPTO,SALESTO_NAME,SHIPTO_NAME,
                CI,ShipmentType,ShipmentNO,
                MXB.* 
                FROM AWMS_OUTB_HEADER HZB WITH (NOLOCK)
                INNER JOIN AWMS_OUTB_ITEM MXB WITH (NOLOCK)
                ON HZB.OUTBANDNO = MXB.OUTBANDNO
                WHERE MXB.STATUS='已确认' AND  HZB.STATUS='已确认'
                " + @_str);
                 dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                 dsReturn.Tables[0].TableName = "OUTB_TAB";
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
             catch (Exception ex)
             {
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("GetPickedInfo Error: " + ex.Message);
             }
             return dsReturn;
         }

         public DataSet GetCarInfo(string _ono)
         {
             if (string.IsNullOrEmpty(_ono)==true)
             {
                 return null;
             }

             DataSet dsReturn = new DataSet();
             try
             {
                 StringBuilder sbSql = new StringBuilder();

                 sbSql.AppendFormat(@" SELECT CI,ShipmentType,ShipmentNO FROM AWMS_OUTB_HEADER WHERE OUTBANDNO = '" + _ono + "'");
                 dsReturn = db.ExecuteDataSet(CommandType.Text, sbSql.ToString());
                 dsReturn.Tables[0].TableName = "CarInfo";
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
             }
             catch (Exception ex)
             {
                 ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                 LogService.LogError("ImportPickdata Error: " + ex.Message);
             }
             return dsReturn;

         }

        public bool UpateCarInfo(string _ci,string _Stype,string _sno,string _ono,string _vno)
        {
            string _str = "";
            if (string.IsNullOrEmpty(_ono) != true)
            {
                _str += " and OUTBANDNO = '" + _ono + "'";
            }
            if (string.IsNullOrEmpty(_vno) != true)
            {
                _str += " and VBELN  = '" + _vno + "'";
            }
            try
             {
                StringBuilder sbSql = new StringBuilder();
                                            
                                            
                sbSql.AppendFormat(@" UPDATE AWMS_OUTB_HEADER SET CI	= '{0}',
                                    ShipmentType = '{1}',
                                    ShipmentNO  = '{2}'
                                    WHERE OUTBANDNO = '" + _ono + "'",
                                    _ci,
                                    _Stype,
                                    _sno
                                    );
                db.ExecuteNonQuery(CommandType.Text, sbSql.ToString());            
            }
             catch (Exception ex)
             {                
                 LogService.LogError("UpateCarInfo Error: " + ex.Message);
                 throw ex;
                 return false;                     
             }
            return true;

        }
        
         #endregion
    }
}




//                 for (int i = 0; i < dt.Rows.Count; i++)
//                 {
//                     StringBuilder sbSql = new StringBuilder(); //写入出库信息
//                     sbSql.AppendFormat(@"INSERT INTO AWMS_OUTB_ITEM 
//                                        ( OUTBANDNO,
//                                            VBELN,
//                                            POSNR,
//                                            STATUS,
//                                            ERDAT, 
//                                            PLANT,
//                                            STORAGELOCATION,
//                                            CPBH,
//                                            XHGG,
//                                            OBDQTY,
//                                            PICKINGQTY,
//                                            QCQTY,
//                                            POSTQTY,
//                                            UNIT,
//                                            VSTEL, 
//                                            REF_NO,
//                                            REF_ITEM,
//                                            LAST_CNG_DATE,
//                                            LAST_CNG_BY,
//                                            BATCHNO,
//                                            Cabinet_NO,
//                                            Cabinet_KEY,
//                                            DemandNO,
//                                            DemardPOS,
//                                            DemardSch
//                                            )   
//                                        VALUES
//                                        (
//                                            '{0}',
//                                            '{1}',
//                                            '{2}',
//                                            '{3}',
//                                            getdate(),
//                                            '{4}',
//                                            '{5}',
//                                            '{6}',
//                                            '{7}',
//                                            '{8}',
//                                            '{9}',
//                                            '{10}',
//                                            '{11}',
//                                            '{12}',
//                                            '{13}',
//                                            '{14}',
//                                            '{15}',                                          
//                                            getdate(),
//                                            '{16}',                                           
//                                            '{17}',
//                                            '{18}',                                         
//                                            '{19}',
//                                            '{20}',
//                                            '{21}',                                         
//                                            '{22}'
//                                            )
//                                        ",
//                                             OUTBNO,
//                                             NO,
//                                             (i + 1).ToString(),
//                                             "暂存",
//                                             dt.Rows[i]["WERKS"].ToString(),
//                                             dt.Rows[i]["LGORT"].ToString(),
//                                             dt.Rows[i]["MATNR"].ToString(),
//                                             dt.Rows[i]["ARKTX"].ToString(),
//                                             Math.Floor(float.Parse(dt.Rows[i]["OutBQty"].ToString())),
//                                             Math.Floor(float.Parse(dt.Rows[i]["OutBQty"].ToString())),
//                                             0,
//                                             0,
//                                             dt.Rows[i]["VRKME"].ToString(),
//                                             dt.Rows[i]["VSTEL"].ToString(),
//                                             dt.Rows[i]["VBELN"].ToString(),
//                                             dt.Rows[i]["POSNR"].ToString(),
//                                             dt.Rows[i]["ERNAM"].ToString(),
//                                             dt.Rows[i]["CHARG"].ToString(),
//                                             dt.Rows[i]["CONTAINER_CODE"].ToString(),
//                                             dt.Rows[i]["CONTAINER_KEY"].ToString(),
//                                             dt.Rows[i]["AUFNR"].ToString(),
//                                             dt.Rows[i]["VGPOS"].ToString(),
//                                             dt.Rows[i]["GRKOR"].ToString()
//                                         );
//                     db.ExecuteNonQuery(CommandType.Text, sbSql.ToString());

//                 }
//                 //执行查询。
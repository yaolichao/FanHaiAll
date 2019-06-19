using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using FanHai.Hemera.Utils.DatabaseHelper;
using System.Data.Common;
using SAP.Middleware.Connector;
using System.Web;
using System.Data.SqlClient;
using FanHai.Hemera.Share.Interface.RFC;

namespace Astronergy.Modules.WMS
{
    public class OutboundOperationEngine : AbstractEngine,IOutboundOperationEngine
    {
        private Database db; //用于数据库访问的变量。
        private Database db2;
        //构造函数
        public OutboundOperationEngine()
        {
            db2 = DatabaseFactory.CreateDatabase();//默认数据库链接
            db = DatabaseFactory.CreateDatabase("SQLServerAwms");
        }
        // 初始化方法。
        public override void Initialize()
        {
           
        }
        #region IOutboundOperationEngine 成员
        //实现接口
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            return Query(dsParams, ref config, true);
        }

        #endregion
       
        private DataSet Query(DataSet dsParams,ref PagingQueryConfig pConfig,bool ispaging )
        {
            DataSet dsReturn = new DataSet();
            StringBuilder sBuilder = new StringBuilder();
            try
            {
                ////出库校验
                //if (status == "CKJY")
                //{
                    sBuilder.Append(@"select OUTBANDNO,VBELN,POSNR,CPBH,XHGG,OBDQTY,QC_RESULT,BATCHNO,STATUS,QC_PERSON, 
                                             case PKG_MAT when 0 then '合格' when 1 then '不合格' else ''end as PKG_MAT,
                                             case BILL_BRND  when 0 then '合格' when 1 then '不合格'else '' end as BILL_BRND ,
                                             case CANTR  when 0 then '合格' when 1 then '不合格' else '' end as CANTR ,
                                             case F_OUTB_CUSTM  when 0 then '合格' when 1 then '不合格'else '' end as F_OUTB_CUSTM ,
                                             case EL  when 0 then '合格' when 1 then '不合格'else '' end as EL ,  
                                             case DATA_FORMT when 0 then '合格' when 1 then '不合格'else '' end as DATA_FORMT,
                                             case LIST_ABSENCE  when 0 then '合格' when 1 then '不合格'else '' end as LIST_ABSENCE ,
                                             case LiST_ERR  when 0 then '合格' when 1 then '不合格'else '' end as LiST_ERR ,
                                             case CELL  when 0 then '合格' when 1 then '不合格'else '' end as CELL ,
                                             case MOD_ERR  when 0 then '合格' when 1 then '不合格'else '' end as MOD_ERR , 
                                             case QLVL_ERR when 0 then '合格' when 1 then '不合格'else '' end as QLVL_ERR,
                                             case FRAME  when 0 then '合格' when 1 then '不合格'else '' end as FRAME ,
                                             case BRND_PARM_ERR  when 0 then '合格' when 1 then '不合格'else '' end as BRND_PARM_ERR ,
                                             case CONT_LOCK_BRK  when 0 then '合格' when 1 then '不合格'else '' end as CONT_LOCK_BRK ,
                                             case CUSTM_CK  when 0 then '合格' when 1 then '不合格'else '' end as CUSTM_CK       
                                       from (select OUTBANDNO,a.VBELN,a.POSNR,CPBH,XHGG,OBDQTY,a.QC_RESULT,a.BATCHNO,a.STATUS,a.QC_PERSON, 
                                                    PKG_MAT,BILL_BRND ,CANTR ,F_OUTB_CUSTM ,EL ,DATA_FORMT, LIST_ABSENCE , LiST_ERR , CELL ,
                                                    MOD_ERR ,QLVL_ERR,FRAME ,BRND_PARM_ERR ,CONT_LOCK_BRK ,CUSTM_CK                                    
                                               from AWMS_OUTB_ITEM a left join AWMS_OUTB_QC b on a.VBELN=b.VBELN and a.POSNR=b.POSNR
                                              where a.STATUS in('已确认') and b.STATUS is null
                                       union select distinct OUTBANDNO,a.VBELN,a.POSNR,CPBH,XHGG,OBDQTY,a.QC_RESULT,a.BATCHNO,a.STATUS,a.QC_PERSON, 
                                                    PKG_MAT=2,BILL_BRND=2 ,CANTR=2 ,F_OUTB_CUSTM= 2,EL=2 ,DATA_FORMT=2, 
                                                    LIST_ABSENCE=2 ,LiST_ERR=2 , CELL=2 ,MOD_ERR=2 , QLVL_ERR=2,FRAME=2 ,
                                                    BRND_PARM_ERR=2 ,CONT_LOCK_BRK=2 ,CUSTM_CK=2                                    
                                               from AWMS_OUTB_ITEM a left join AWMS_OUTB_QC b on a.VBELN=b.VBELN and a.POSNR=b.POSNR
                                              where a.STATUS in('已确认') and b.STATUS ='删除'      
                                       union select OUTBANDNO,a.VBELN,a.POSNR,CPBH,XHGG,OBDQTY,a.QC_RESULT,a.BATCHNO,a.STATUS,a.QC_PERSON, 
                                                    PKG_MAT,BILL_BRND ,CANTR ,F_OUTB_CUSTM ,EL ,DATA_FORMT, LIST_ABSENCE , LiST_ERR , CELL ,
                                                    MOD_ERR ,QLVL_ERR,FRAME ,BRND_PARM_ERR ,CONT_LOCK_BRK ,CUSTM_CK                                    
                                               from AWMS_OUTB_ITEM a left join AWMS_OUTB_QC b on a.VBELN=b.VBELN and a.POSNR=b.POSNR
                                              where a.STATUS in('已过账','已检验') and b.STATUS='正常') a  where 1=1
                                ");
                //}
//                //出库管理
//                else if (status == "CKGL")
//                {
//                    sBuilder.Append(@"select a.OUTBANDNO,a.VBELN,a.POSNR,a.status,CPBH,XHGG,QCQTY,OBDQTY,QC_RESULT,BATCHNO,
//                                             c.PALLET_NO from AWMS_OUTB_ITEM a,AWMS_CONTAINER_DETAIL b ,[dbo].WIP_CONSIGNMENT c
//                                       where a.Cabinet_NO=b.CONTAINER_CODE and a.Cabinet_KEY=b.CONTAINER_KEY and a.CPBH=c.SAP_NO
//                                             and b.PALLET_NO=c.PALLET_NO and a.STATUS in('已确认','已过账','已检验')
//                                    ");
//                }
               
                if (dsParams != null && dsParams.Tables.Contains(TRANS_TABLES.TABLE_MAIN_DATA))
                {
                    DataTable dtParam = dsParams.Tables[TRANS_TABLES.TABLE_MAIN_DATA];
                    Hashtable hsParam = CommonUtils.ConvertToHashtable(dtParam);
                    if (hsParam.ContainsKey(AWMS_OUTB_ITEM_FIELDS.Field_VBELN))
                    {
                        string vbelnNo = Convert.ToString(hsParam[AWMS_OUTB_ITEM_FIELDS.Field_VBELN]);
                        sBuilder.AppendFormat(" and a.VBELN like '{0}%'", vbelnNo.PreventSQLInjection());
                       
                    }
                    if (hsParam.ContainsKey(AWMS_OUTB_ITEM_FIELDS.Field_OUTBANDNO))
                    {
                        string outboundNo = Convert.ToString(hsParam[AWMS_OUTB_ITEM_FIELDS.Field_OUTBANDNO]);
                        sBuilder.AppendFormat(" and a.OUTBANDNO like '{0}%'", outboundNo.PreventSQLInjection());  
                    }
                    if (hsParam.ContainsKey(AWMS_CONTAINER_DETAIL_FIELDS.Field_PALLET_NO))
                    {
                        string outboundNo = Convert.ToString(hsParam[AWMS_CONTAINER_DETAIL_FIELDS.Field_PALLET_NO]);
                        sBuilder.AppendFormat(" and c.PALLET_NO like '{0}%'", outboundNo.PreventSQLInjection());
                    }
                }
                int pages = 0;
                int records = 0;
                AllCommonFunctions.CommonPagingData(sBuilder.ToString(),
                                                    pConfig.PageNo,
                                                    pConfig.PageSize,
                                                    out pages,
                                                    out records,
                                                    db,
                                                    dsReturn,
                                                    POR_LOT_FIELDS.DATABASE_TABLE_NAME,
                                                    "");
                pConfig.Pages = pages;
                pConfig.Records = records;

                ReturnMessageUtils.AddServerReturnMessage(dsReturn,string.Empty);
            }

            catch (Exception ex)
            { 
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("OutboundOperationEngine Query Error:" + ex.Message);
            }
            finally
            { }
            return dsReturn;
        }

        public DataSet getOutboudInfo(string Outboudno, string VebelNO, string ShipmentNO, string SType)
        {
            StringBuilder sBuilder = new StringBuilder();
            DataSet dsReturn = new DataSet();
            try
            {
                string S_status = string.Empty;

                sBuilder.AppendFormat(@"SELECT ERDAT ,CREATED_BY,SALESTO,SHIPTO,SALESTO_NAME,SHIPTO_NAME,VBELN,OUTBANDNO,STATUS
                                            ,CI,ShipmentNo,LFDAT,QCDAT,KODAT,CREATED_BY, case ShipmentType when '0' then '陆运' when '1' then '海运' when '2' then '空运' end as ShipmentType
                                             FROM AWMS_OUTB_HEADER
                                            WHERE STATUS in('已确认','已检验','已过账')"
                                          );
//                //出库管理
//                else if (status == "CKGL")
//                {
//                    sBuilder.AppendFormat(@"SELECT ERDAT ,CREATED_BY,SALESTO,SHIPTO,SALESTO_NAME,SHIPTO_NAME,VBELN,OUTBANDNO
//                                            ,CI, ShipmentType,ShipmentNo,status FROM AWMS_OUTB_HEADER
//                                            WHERE STATUS in('已确认','已过账','已检验')"
//                                         );
//                }
                if (Outboudno!= "")
                {
                    sBuilder.AppendFormat(" and OUTBANDNO like '{0}%'", Outboudno.PreventSQLInjection());
                }
                if (VebelNO != "")
                {
                    sBuilder.AppendFormat(" and VBELN like '%{0}%'", VebelNO.PreventSQLInjection());
                }
                if (ShipmentNO != "")
                {
                    sBuilder.AppendFormat(" and ShipmentNO like '{0}%'", ShipmentNO.PreventSQLInjection());
                }
                if (SType != "")
                {
                    sBuilder.AppendFormat(" and ShipmentType = '{0}'", SType.PreventSQLInjection());
                }
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());
                
            }
            catch (Exception ex)
            {
                LogService.LogError("getOutboudInfo Error: " + ex.Message);
                throw ex;
            }
            return dsReturn;
        }

        public DataSet getOutboudItem(string Outboudno, string VebelNO, string ShipmentNO, string SType)
        {
            StringBuilder sBuilder = new StringBuilder();
            DataSet dsReturn = new DataSet();
            try
            {
                string S_status = string.Empty;
                //出库校验

                sBuilder.AppendFormat(@"SELECT CPBH,XHGG,OBDQTY,UNIT,VSTEL,QC_RESULT,PalletNO,Cabinet_NO,a.STATUS
                                          FROM AWMS_OUTB_ITEM a,AWMS_OUTB_HEADER b 
                                         WHERE a.STATUS in('已确认','已检验','已过账') and b.STATUS in('已确认','已检验','已过账')
                                               and a.OUTBANDNO=b.OUTBANDNO and a.VBELN=b.VBELN "
                                      );
     
                if (Outboudno != "")
                {
                    sBuilder.AppendFormat(" and a.OUTBANDNO like '{0}%'", Outboudno.PreventSQLInjection());
                }
                if (VebelNO != "")
                {
                    sBuilder.AppendFormat(" and a.VBELN like '%{0}%'", VebelNO.PreventSQLInjection());
                }
                if (ShipmentNO != "")
                {
                    sBuilder.AppendFormat(" and ShipmentNO like '{0}%'", ShipmentNO.PreventSQLInjection());
                }
                if (SType != "")
                {
                    sBuilder.AppendFormat(" and ShipmentType = '{0}'", SType.PreventSQLInjection());
                }
                //执行查询。
                dsReturn = db.ExecuteDataSet(CommandType.Text, sBuilder.ToString());

            }
            catch (Exception ex)
            {
                LogService.LogError("getOutboudItem Error: " + ex.Message);
                throw ex;
            }
            return dsReturn;
        }
       
        public string SetQcResult(DataSet dsParams, string Outboudno, string VebelNO,string QC_PERSON,string IsEdit,out bool Result)
        {
            string strReturn = "传入的参数错误。";
            DataSet dsReturn = new DataSet();
            string QcNo = string.Empty;
            Result = true;
            if (null == dsParams || !dsParams.Tables.Contains(AWMS_OUTB_QC_FIELDS.TABLE_NAME_OUTB_QC))
            {
                ReturnMessageUtils.AddServerReturnMessage(dsReturn, "传入的参数错误。");
                Result = false;
                return strReturn;
            }
            DbConnection dbConn = null;
            DbTransaction dbTran = null;

            try
            {
                dbConn = db.CreateConnection();               
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();

                DataTable dtQC = dsParams.Tables[AWMS_OUTB_QC_FIELDS.TABLE_NAME_OUTB_QC];
                //// 检查是否存在相同的检验单号
                //var lnq = from item in dtQC.AsEnumerable()
                //          group item by item[AWMS_OUTB_QC_FIELDS.Field_OUTBANDNO] into g
                //          where g.Count()> 1
                //          select g.Count();
                //if (lnq.Count() >0)
                //{
                //    dbTran.Rollback();
                //    ReturnMessageUtils.AddServerReturnMessage(dsReturn,"");
                //}
                //AWMS_OUTB_QC_FIELDS Qc_Fileds = new AWMS_OUTB_QC_FIELDS();
              
                //更新AWMS_OUTB_HEADER质检日期。
                string sql = string.Format(@"UPDATE AWMS_OUTB_HEADER 
                                                 SET QCDAT=GETDATE(),status='{2}'
                                                 WHERE OUTBANDNO='{0}' and VBELN='{1}'",
                                             Outboudno.PreventSQLInjection(),
                                             VebelNO.PreventSQLInjection(),
                                             "已检验");
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                //生成检验单号
                GetBill gb=new GetBill();
                QcNo =gb.GetBillNo("JY", "AWMS_OUTB_QC", "QC_NO", db);

                
                //循环更新外向交货单明细表，保存出库检验信息。
                int item = 1;
                foreach (DataRow dr in dtQC.Rows)
                {
                    string POSNR = Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_POSNR]);
                    string QC_RESULT = Convert.ToString(dr[AWMS_OUTB_ITEM_FIELDS.Field_QC_RESULT]);
                                       

                    //保存出库检验信息
                    int PKG_MAT = 0;
                    int BILL_BRND = 0;
                    int CANTR = 0;
                    int F_OUTB_CUSTM = 0;
                    int EL = 0;
                    int DATA_FORMT = 0;
                    int LIST_ABSENCE = 0;
                    int LiST_ERR = 0;
                    int CELL = 0;
                    int MOD_ERR = 0;
                    int QLVL_ERR = 0;
                    int FRAME = 0;
                    int BRND_PARM_ERR = 0;
                    int CONT_LOCK_BRK = 0;
                    int CUSTM_CK = 0;


                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_PKG_MAT))
                    {
                        PKG_MAT=checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_PKG_MAT]));                       
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_BILL_BRND))
                    {
                        BILL_BRND=checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_BILL_BRND]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_CANTR))
                    {
                        CANTR=checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_CANTR]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_F_OUTB_CUSTM))
                    {
                        F_OUTB_CUSTM=checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_F_OUTB_CUSTM]));
                     }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_EL))
                    {
                        EL = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_EL]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_DATA_FORMT))
                    {
                        DATA_FORMT = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_DATA_FORMT]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_LIST_ABSENCE))
                    {
                        LIST_ABSENCE = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_LIST_ABSENCE]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_LiST_ERR))
                    {
                        LiST_ERR = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_LiST_ERR]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_CELL))
                    {
                        CELL = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_CELL]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_MOD_ERR))
                    {
                        MOD_ERR = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_MOD_ERR]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_QLVL_ERR))
                    {
                        QLVL_ERR = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_QLVL_ERR]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_FRAME))
                    {
                        FRAME = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_FRAME]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_BRND_PARM_ERR))
                    {
                        BRND_PARM_ERR = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_BRND_PARM_ERR]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_CONT_LOCK_BRK))
                    {
                        CONT_LOCK_BRK = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_CONT_LOCK_BRK]));
                    }
                    if (!dr.IsNull(AWMS_OUTB_QC_FIELDS.Field_CUSTM_CK))
                    {
                        CUSTM_CK = checkQc(Convert.ToString(dr[AWMS_OUTB_QC_FIELDS.Field_CUSTM_CK]));
                    }
                    if (QC_RESULT == "")
                    {
                        QC_RESULT = "合格";
                        if (!((PKG_MAT == 0) && (BILL_BRND == 0) && (CANTR == 0) && (F_OUTB_CUSTM == 0) && (EL == 0) &&
                            (DATA_FORMT == 0) && (LIST_ABSENCE == 0) && (LiST_ERR == 0) && (CELL == 0) && (MOD_ERR == 0) &&
                             (QLVL_ERR == 0) && (FRAME == 0) && (BRND_PARM_ERR == 0) && (CONT_LOCK_BRK == 0) && (CUSTM_CK == 0)))
                        {
                            QC_RESULT = "不合格";
                        }
                    }
                    if (QC_RESULT == "不合格")
                    {
                        Result = false;
                    }
                    string BATCHNO = Convert.ToString(dr[AWMS_OUTB_ITEM_FIELDS.Field_BATCHNO]);

                    sql = string.Format(@"UPDATE AWMS_OUTB_ITEM 
                                                 SET QC_NO='{3}',QC_ITEM='{4}',QC_RESULT='{5}',QC_PERSON='{6}',status='已检验',
                                                    LAST_CNG_DATE=GETDATE(),LAST_CNG_BY='{7}',QCDATE =GETDATE(),QCQTY=OBDQTY
                                                 WHERE OUTBANDNO='{0}' and VBELN='{1}'and POSNR='{2}'",
                                                Outboudno.PreventSQLInjection(),
                                                 VebelNO.PreventSQLInjection(),
                                                 POSNR.PreventSQLInjection(),
                                                 QcNo.PreventSQLInjection(),
                                                 Convert.ToString(item).PreventSQLInjection(),
                                                 QC_RESULT,
                                                 QC_PERSON.PreventSQLInjection(),
                                                 QC_PERSON.PreventSQLInjection()
                                                 );
                    db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    //新增检验信息
                    if (IsEdit == "N")
                    {
                        string CurrentTime= DateTime.Now.ToString();
                        string remark = "出货单号" + Outboudno + "已被" + QC_PERSON +"在"+CurrentTime+ "检验";
                        sql = string.Format(@"insert into AWMS_OUTB_QC(QC_NO, QC_ITEM, VBELN, POSNR, PKG_MAT, BILL_BRND, CANTR, 
                                                                   F_OUTB_CUSTM, EL, DATA_FORMT, LIST_ABSENCE, LiST_ERR,CELL,
                                                                   MOD_ERR, QLVL_ERR, FRAME, BRND_PARM_ERR, CONT_LOCK_BRK, 
                                                                   CUSTM_CK, QC_PERSON, BATCHNO,status)
                                                             values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',
                                                                    '{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'
                                                                    ,'{19}','{20}','正常')
                                           ",
                                                 QcNo.PreventSQLInjection(),
                                                 Convert.ToString(item).PreventSQLInjection(),
                                                 VebelNO.PreventSQLInjection(),
                                                 POSNR.PreventSQLInjection(),
                                                 Convert.ToString(PKG_MAT).PreventSQLInjection(),
                                                 Convert.ToString(BILL_BRND).PreventSQLInjection(),
                                                 Convert.ToString(CANTR).PreventSQLInjection(),
                                                 Convert.ToString(F_OUTB_CUSTM).PreventSQLInjection(),
                                                 Convert.ToString(EL).PreventSQLInjection(),
                                                 Convert.ToString(DATA_FORMT).PreventSQLInjection(),
                                                 Convert.ToString(LIST_ABSENCE).PreventSQLInjection(),
                                                 Convert.ToString(LiST_ERR).PreventSQLInjection(),
                                                 Convert.ToString(CELL).PreventSQLInjection(),
                                                 Convert.ToString(MOD_ERR).PreventSQLInjection(),
                                                 Convert.ToString(QLVL_ERR).PreventSQLInjection(),
                                                 Convert.ToString(FRAME).PreventSQLInjection(),
                                                 Convert.ToString(BRND_PARM_ERR).PreventSQLInjection(),
                                                 Convert.ToString(CONT_LOCK_BRK).PreventSQLInjection(),
                                                 Convert.ToString(CUSTM_CK).PreventSQLInjection(),
                                                 QC_PERSON.PreventSQLInjection(),
                                                 Convert.ToString(BATCHNO).PreventSQLInjection()
                                                 );
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);

                        //更新AWMS_OUTB_ITEM检验单单号、检验单行号、检验结果、检验人、最后修改时间、最后修改人、检验时间
                        sql = string.Format(@" insert into AWMS_RECORD_HIS(Bill_no, Bill_item, Bill_type, Bill_status,CreatedBy,
                                                                       CreateDate,Pre_no, Remark) 
                                                             values('{0}','{1}','检验','正常','{2}',GETDATE(),'{3}','{4}')",                                                
                                                 QcNo.PreventSQLInjection(),
                                                 Convert.ToString(item).PreventSQLInjection(),
                                                 QC_PERSON.PreventSQLInjection(),
                                                 Outboudno.PreventSQLInjection(),
                                                 remark.PreventSQLInjection());
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                    //修改检验信息
                    else if (IsEdit == "Y")
                    {//QC_NO, QC_ITEM, VBELN, POSNR,
                        string CurrentTime = DateTime.Now.ToString();
                        string remark = "检验结果已被" + QC_PERSON +"在"+CurrentTime+ "修改";
                        sql = string.Format(@"update AWMS_OUTB_QC set PKG_MAT='{0}', BILL_BRND='{1}', CANTR='{2}',F_OUTB_CUSTM='{3}', 
                                                                      EL='{4}', DATA_FORMT='{5}', LIST_ABSENCE='{6}', LiST_ERR='{18}',CELL='{7}',
                                                                      MOD_ERR='{8}', QLVL_ERR='{9}', FRAME='{10}', BRND_PARM_ERR='{11}', CONT_LOCK_BRK='{12}', 
                                                                      CUSTM_CK='{13}', QC_PERSON='{14}' from AWMS_OUTB_QC a ,AWMS_OUTB_ITEM b
                                                                where a.QC_NO=b.QC_NO and a.QC_ITEM=b.QC_ITEM and a.status='正常' and b.OUTBANDNO='{15}'
                                                                      and b.VBELN='{16}'and b.POSNR='{17}'",
                                                 Convert.ToString(PKG_MAT).PreventSQLInjection(),
                                                 Convert.ToString(BILL_BRND).PreventSQLInjection(),
                                                 Convert.ToString(CANTR).PreventSQLInjection(),
                                                 Convert.ToString(F_OUTB_CUSTM).PreventSQLInjection(),
                                                 Convert.ToString(EL).PreventSQLInjection(),
                                                 Convert.ToString(DATA_FORMT).PreventSQLInjection(),
                                                 Convert.ToString(LIST_ABSENCE).PreventSQLInjection(),                                               
                                                 Convert.ToString(CELL).PreventSQLInjection(),
                                                 Convert.ToString(MOD_ERR).PreventSQLInjection(),
                                                 Convert.ToString(QLVL_ERR).PreventSQLInjection(),
                                                 Convert.ToString(FRAME).PreventSQLInjection(),
                                                 Convert.ToString(BRND_PARM_ERR).PreventSQLInjection(),
                                                 Convert.ToString(CONT_LOCK_BRK).PreventSQLInjection(),
                                                 Convert.ToString(CUSTM_CK).PreventSQLInjection(),
                                                 QC_PERSON.PreventSQLInjection(),
                                                 Outboudno.PreventSQLInjection(),
                                                 VebelNO.PreventSQLInjection(),
                                                 POSNR.PreventSQLInjection(),
                                                 Convert.ToString(LiST_ERR).PreventSQLInjection()
                                                 );
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                        
                        sql = string.Format(@"update AWMS_RECORD_HIS set Memo3='{2}'from AWMS_RECORD_HIS a,AWMS_OUTB_ITEM b 
                                              where a.bill_no=b.QC_NO and a.bill_ITEM=b.QC_ITEM and b.OUTBANDNO='{0}' and b.VBELN='{1}'",                                        
                                                 Outboudno.PreventSQLInjection(),
                                                 VebelNO.PreventSQLInjection(),
                                                 remark.PreventSQLInjection()
                                                 );
                        db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                    }
                        item = item + 1;                
                }
        
                dbTran.Commit();
                strReturn="出库检验信息保存成功!";
                //ReturnMessageUtils.AddServerReturnMessage(dsReturn, string.Empty);
            }
            catch (Exception ex)
            {
                //ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                strReturn = "出库检验信息保存失败!";
                LogService.LogError("SetQcResult Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally 
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return strReturn;
        }
        /// <summary>
        /// 删除出库检验结果
        /// </summary>
        /// <param name="OutboundNo"></param>
        /// <param name="VebelNO"></param>
        /// <returns></returns>
        public void DeleteQcResult(string OutboundNo, string VebelNO,string Del_Person)
        {
            DbConnection dbConn = null;
            DbTransaction dbTran = null;

            try
            {
                string Memo ="该检验单已被"+Del_Person+"删除";
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                string sql = string.Format(@"update AWMS_OUTB_QC set status='删除' from AWMS_OUTB_QC a,AWMS_OUTB_ITEM b 
                                              where a.QC_NO=b.QC_NO and a.QC_ITEM=b.QC_ITEM and b.OUTBANDNO='{0}' and b.VBELN='{1}'
                                             update AWMS_RECORD_HIS set Memo2='{3}'from AWMS_RECORD_HIS a,AWMS_OUTB_ITEM b 
                                              where a.bill_no=b.QC_NO and a.bill_ITEM=b.QC_ITEM and b.OUTBANDNO='{0}' and b.VBELN='{1}'
                                             update AWMS_OUTB_ITEM set status='已确认',LAST_CNG_DATE=getdate(),LAST_CNG_BY='{2}',QC_NO='',QC_ITEM=0,QC_RESULT=''
                                              where OUTBANDNO='{0}' and VBELN='{1}'
                                             update AWMS_OUTB_HEADER set status='已确认'
                                              where OUTBANDNO='{0}' and VBELN='{1}'
                                            ",
                                             OutboundNo.PreventSQLInjection(),
                                             VebelNO.PreventSQLInjection(),
                                             Del_Person.PreventSQLInjection(),
                                             Memo.PreventSQLInjection());
                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();

            }
            catch (Exception ex)
            {
                //ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
               
                LogService.LogError("SetQcResult Error: " + ex.Message);
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }

        }
         
        /// <summary>
        /// 过账同步SAP
        /// </summary>
        /// <param name="VebelNO"></param>
        /// <param name="Qcno"></param>
        /// <param name="msg"></param>

        //private void OUTB_DELIVERY_CONFIRM(string VebelNO, string Qcno, out string msg, DbConnection dbConn, DbTransaction dbTran)
        //{
        //IDestinationConfiguration ID = new SapRemoteConnect();
        //    RfcDestinationManager.RegisterDestinationConfiguration(ID);
        //    RfcDestination prd = RfcDestinationManager.GetDestination("ASRS_TEST");
        //    //RfcDestinationManager.UnregisterDestinationConfiguration(ID);           

        //    ////进行外向交货过帐            
        //    IRfcFunction function = prd.Repository.CreateFunction("BAPI_OUTB_DELIVERY_CONFIRM_DEC");
        //    //赋值

        //    IRfcStructure H_DATA = function.GetStructure("HEADER_DATA");//抬头信息
        //    IRfcStructure H_CONTROL = function.GetStructure("HEADER_CONTROL");//抬头控制信息

        //    //抬头部分赋值
        //    H_DATA.SetValue("DELIV_NUMB", VebelNO);//交货
        //    function.SetValue("HEADER_DATA", H_DATA);

        //    H_CONTROL.SetValue("DELIV_NUMB", VebelNO);//交货
        //    H_CONTROL.SetValue("POST_GI_FLG", "X");//自动过帐货物移动
        //    H_CONTROL.SetValue("DELIV_DATE_FLG", "X");//确认交货日期
        //    H_CONTROL.SetValue("GROSS_WT_FLG", "X");//毛重的确认
        //    function.SetValue("HEADER_CONTROL", H_CONTROL);
        //    function.SetValue("DELIVERY", VebelNO);

        //    IRfcTable returnTable = function.GetTable("RETURN");
        //    RfcSessionManager.BeginContext(prd);
        //    function.Invoke(prd);//提交调用BAPI 


        //    string Mes = string.Empty;
        //    if (returnTable.RowCount > 0)
        //    {
        //        Mes = "外向交货单过账失败:";
        //        for (int i = 0; i < returnTable.RowCount; i++)
        //        {
        //            //失败的产品信息

        //            string type = returnTable.ElementAt(i).GetString("TYPE");
        //            string id = returnTable.ElementAt(i).GetString("id");
        //            int number = returnTable.ElementAt(i).GetInt("NUMBER");
        //            string message = returnTable.ElementAt(i).GetString("MESSAGE");
        //            string system = returnTable.ElementAt(i).GetString("SYSTEM");
        //            string REF_EXT1 = returnTable.ElementAt(i).GetString("MESSAGE_V1");
        //            string REF_EXT2 = returnTable.ElementAt(i).GetString("MESSAGE_V2");
        //            string REF_EXT3 = returnTable.ElementAt(i).GetString("MESSAGE_V3");

        //            Mes = Mes + type + message;
                    
        //        }
        //        UpdateTable(Mes, VebelNO, Qcno, 1, dbConn, dbTran);
        //    }
        //    else
        //    {
        //        prd.Repository.ClearFunctionMetadata();
        //        RfcFunctionMetadata BAPI_COMMIT = prd.Repository.GetFunctionMetadata("BAPI_TRANSACTION_COMMIT");
        //        IRfcFunction function1 = null;
        //        function1 = BAPI_COMMIT.CreateFunction();
        //        function1.SetValue("WAIT", "X");
        //        function1.Invoke(prd);

        //        Mes = "外向交货单过账成功！";
        //        UpdateTable(Mes, VebelNO, Qcno, 1, dbConn, dbTran);
        //    }

        //    RfcSessionManager.EndContext(prd);
        //    prd = null;
        //    msg = Mes;
        //}

        /// <summary>
        ///保存后更新相应表信息 
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="VebelNO"></param>
        /// <param name="Qcno"></param>
        /// <param name="Flag"></param>
        public string UpdateTable(string Msg, string VebelNO, string Outboudno, int Flag)
        {
            string strReturn=string.Empty;
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try 
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                if (Flag == 1)//过账成功
                {
                    DataSet dsretrun = new DataSet();

                    //更新单据状态
                    string SqlStr = string.Format(@"UPDATE AWMS_OUTB_HEADER SET status='已过账'where VBELN='{0}'
                                                UPDATE AWMS_OUTB_ITEM SET status='已过账' where VBELN='{1}'
                                                UPDATE AWMS_CONTAINER SET status='2' from AWMS_CONTAINER a,AWMS_OUTB_ITEM b 
                                                    where a.CONTAINER_CODE=b.Cabinet_NO and a.CONTAINER_KEY=b.Cabinet_KEY
                                                    and b.VBELN='{2}'
                                                ",
                                                    VebelNO.PreventSQLInjection(),
                                                    VebelNO.PreventSQLInjection(),
                                                    VebelNO.PreventSQLInjection()
                                                    );
                    db.ExecuteNonQuery(dbTran, CommandType.Text, SqlStr);
                    //取托盘号
                    SqlStr = string.Format(@"select distinct PALLET_NO from AWMS_CONTAINER_DETAIL a,AWMS_OUTB_ITEM b 
                                                    where a.CONTAINER_CODE=b.Cabinet_NO and a.CONTAINER_KEY=b.Cabinet_KEY
                                                    and b.VBELN='{0}'",
                                                    VebelNO.PreventSQLInjection());

                    dsretrun = db.ExecuteDataSet(dbTran, CommandType.Text, SqlStr);
                    string palletNo = string.Empty;
                    if (dsretrun.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsretrun.Tables[0].Rows)
                        {
                            palletNo = palletNo + "'" + dsretrun.Tables[0].Rows[0]["PALLET_NO"].ToString() + "',";
                        }
                        palletNo = palletNo.Remove(palletNo.LastIndexOf(","), 1);

                    }
                    //更新表WIP_CONSIGNMENT中托盘状态
                    if (!(palletNo=="")) 
                    {
                        SqlStr = string.Format(@"update [dbo].WIP_CONSIGNMENT set CS_DATA_GROUP='4' where PALLET_NO in(" + palletNo + ")");//0：包装中；1：包装；2：入库检；3:已入库；4：已出货
                    }
                    

                    db.ExecuteNonQuery(dbTran, CommandType.Text, SqlStr);
                    //strReturn = "外向交货单过账成功！";
                }
                else
                {
                    //strReturn = "外向交货单过账失败！";
                }

                string sql = string.Format(@"UPDATE AWMS_RECORD_HIS 
                                                 SET Memo1='{0}' 
                                                 WHERE pre_no='{1}'",
                                             Msg.PreventSQLInjection(),
                                             Outboudno.PreventSQLInjection());

                db.ExecuteNonQuery(dbTran, CommandType.Text, sql);
                dbTran.Commit();
               
            }
            catch (Exception ex)
            {
                LogService.LogError("UpdateTable Error: " + ex.Message);
                
                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {
                
                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return strReturn;
        }

        //根据合格或者不合格来确定检验各个字段的保存值
        private int checkQc(string qcresult)
        {
            int Rretrun=0;
            if (qcresult == "不合格")//0：合格 1：不合格
            { 
                Rretrun = 1;
            }
            return Rretrun;
        }

        public DataSet UpdateConteinerNo(string OutboundNo,string VebelNO ,string containerNo)
        {
            DataSet dsReturn = new DataSet();
            DbConnection dbConn = null;
            DbTransaction dbTran = null;
            try
            {
                dbConn = db.CreateConnection();
                dbConn.Open();
                dbTran = dbConn.BeginTransaction();
                //更新单据状态
                string SqlStr = string.Format(@"UPDATE AWMS_OUTB_HEADER SET ShipmentNO='{2}'
                                                    where OUTBANDNO='{0}' and VBELN='{1}'
                                                ",
                                                OutboundNo.PreventSQLInjection(),
                                                VebelNO.PreventSQLInjection(),
                                                containerNo.PreventSQLInjection()
                                              
                                                );
                db.ExecuteNonQuery(dbTran, CommandType.Text, SqlStr);
                dbTran.Commit();
            }
            catch (Exception ex)
            {
                LogService.LogError("UpdateConteinerNo Error: " + ex.Message);

                if (dbTran != null)
                {
                    dbTran.Rollback();
                }
            }
            finally
            {

                if (dbConn != null)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbTran = null;
            }
            return dsReturn;
        }
    }
}

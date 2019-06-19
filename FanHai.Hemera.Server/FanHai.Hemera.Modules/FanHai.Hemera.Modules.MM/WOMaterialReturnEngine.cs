using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface.WarehouseManagement;
using System.Data;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.DatabaseHelper;
using FanHai.Hemera.Utils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Modules.MM
{
   /// <summary>
   /// 工单退料数据的操作类。
   /// </summary>
    public class WOMaterialReturnEngine : AbstractEngine,IWOMaterialReturnEngine
    {
        private Database db = null; //数据库操作对象。

        /// <summary>
        /// 构造函数
        /// </summary>
        public WOMaterialReturnEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        /// <summary>
        /// 初始化。
        /// </summary>
        public override void Initialize()
        {
        }
        /// <summary>
        /// 获取当前时间的班别。
        /// </summary>
        /// <returns>包含当前时间班别数据的数据集对象。</returns>
        public DataSet GetCurrentShift()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT SHIFT_VALUE AS SHIFT 
                               FROM CAL_SCHEDULE_DAY 
                               WHERE STARTTIME<GETDATE() AND GETDATE() < ENDTIME";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCurrentShift Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据物料批号获取物料信息。
        /// </summary>
        /// <param name="MatLot">物料批号。</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        public DataSet GetMatLotInfo(string MatLot)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT A.MATERIAL_LOT AS MATLOT, D.MATERIAL_CODE AS MATCODE, D.MATERIAL_NAME AS MATDES, 
                                   E.AUFNR WORKORDERNO ,D.UNIT UNIT , A.CURRENT_QTY AS CURRENTQTY, 
                                   '0' AS RETURNQTY, A.MATERIAL_SUPPLIER SUPPLIER, C.OPERATION_NAME OPERATION,
                                   C.STORE_NAME STORE,F.LOCATION_NAME  FACROOM
                              FROM WST_STORE_MATERIAL_DETAIL A 
                              LEFT JOIN WST_STORE_MATERIAL B     ON A.STORE_MATERIAL_KEY = B.STORE_MATERIAL_KEY
                              LEFT JOIN WST_STORE C              ON B.STORE_KEY = C.STORE_KEY
                              LEFT JOIN POR_MATERIAL D           ON B.MATERIAL_KEY = D.MATERIAL_KEY
                              LEFT JOIN (SELECT DISTINCT STORE_MATERIAL_DETAIL_KEY,AUFNR 
                                         FROM WST_SAP_ISSURE ) E ON A.STORE_MATERIAL_DETAIL_KEY = E.STORE_MATERIAL_DETAIL_KEY
                              LEFT JOIN FMM_LOCATION  F          ON C.LOCATION_KEY=F.LOCATION_KEY
                              WHERE A.MATERIAL_LOT='{0}' AND A.CURRENT_QTY>0";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, MatLot.PreventSQLInjection()));              
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMatLotInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据车间名称获取工厂名称。
        /// </summary>
        /// <param name="strFacRoom">车间名称。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        public DataSet GetFacRoomtoFac(string strFacRoom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT DISTINCT FACTORY_NAME  FACTORY 
                               FROM  V_LOCATION 
                               WHERE ROOM_NAME ='{0}'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, strFacRoom.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetFacRoomtoFac Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据前置码获取退料单最大流水号。
        /// </summary>
        /// <param name="strPrex">退料单前置码。</param>
        /// <returns>包含最大流水号的数据集对象。</returns>
        public  DataSet GetRetMatNo(string strPrex)
        {
            
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT SERIALNO 
                               FROM 
                               (
                                    SELECT MAX(SUBSTRING(ZMBLNR,6,10)) SERIALNO FROM WST_TL_ZMMLKO
                                    WHERE ZMBLNR LIKE '{0}%'
                               ) T
                              WHERE SERIALNO IS NOT NULL";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, strPrex.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRetMatNo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存退料单数据。
        /// </summary>
        /// <param name="strRetMatNo">退料单号。</param>
        /// <param name="strRetMatDate">退料单日期。</param>
        /// <param name="strShift">班别名称。</param>
        /// <param name="strOperator">操作人。</param>
        /// <param name="strRetMatReason">退料原因。</param>
        /// <param name="dtMatLotList">包含退料物料数据的数据表对象。</param>
        /// <returns>true:保存成功。false：保存失败。</returns>
        public bool Save(string strRetMatNo, string strRetMatDate, string strShift, string strOperator,
            string strRetMatReason, DataTable dtMatLotList)
        {
            bool bltmp = false;
            DbConnection dbCon =db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                //1.表示退料单的数据表 WST_TL_ZMMLKO
                DbCommand dbCom = dbCon.CreateCommand();
                dbCom.CommandText = Save1(strRetMatNo, strRetMatDate, strShift, strOperator, strRetMatReason, dtMatLotList);
                dbCom.Transaction = dbTrans;
                dbCom.ExecuteNonQuery();

                //2.表示退料的详细信息，一批一批记录
                for(int i=0;i<dtMatLotList.Rows.Count;i++)
                {
                    dbCom.CommandText = Save2(strRetMatNo, strRetMatDate, strShift, strOperator, strRetMatReason, dtMatLotList.Rows[i]);
                    dbCom.ExecuteNonQuery();
                }

                //3.更新数量
                for (int i = 0; i < dtMatLotList.Rows.Count; i++)
                {
                    dbCom.CommandText = Save3(strRetMatNo, strRetMatDate, strShift, strOperator, strRetMatReason, dtMatLotList.Rows[i]);
                    dbCom.ExecuteNonQuery();
                }

                //4.调用RFC
                DataSet dsOut=new DataSet();
                DataSet dsIn = Save4(strRetMatNo, strRetMatDate, strShift, strOperator, strRetMatReason, dtMatLotList);
                AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_RET", dsIn, out dsOut);
                if (int.Parse(dsOut.Tables[0].Rows[0]["NUMBER"].ToString()) == 0) //MESSAGE
                {
                    dbTrans.Commit();
                    bltmp = true;
                }
                else
                {
                    dbTrans.Rollback();
                    bltmp = false;
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("WOMaterialReturnEngine Save Error: " + ex.Message);
            }
            finally
            {
                dbCon.Close();
            }
            return bltmp;
        }

        //插入退料表的SQL
        private string Save1(string strRetMatNo, string strRetMatDate, string strShift, string strOperator,
            string strRetMatReason, DataTable dtMatLotList)
        {
            string sql=null;
            string strSendMatNo=GetSendMatNO(dtMatLotList.Rows[0]["MATLOT"].ToString());//原来的发料单，需要捞取MES的发料表
            string strFactory = GetERPFac(strRetMatNo.Substring(0,1));//一种方法是到表中取值，另外是通过第一个lot到发料表去找工厂，目前用的第一种
            sql = string.Format(@"INSERT INTO WST_TL_ZMMLKO
                                      (ZMBLNR,WERKS, DEPT,REASON,ZMMLTYP,BLDAT, BWART, POST, MBLNR,MJAHR,LVORM, REF_EBELN,ERDAT,    ERNAM, LAEDA, AENAM, BUDAT, SHIFT_VALUE)
                                VALUES('{0}', '{1}', '',  '{2}', '',     '{3}', '262', '',   '',   '',   '',    '{4}',    GETDATE(), '{5}','',    '{5}', '',    '{6}')",
                                strRetMatNo.PreventSQLInjection(),
                                strFactory.PreventSQLInjection(),
                                strRetMatReason.PreventSQLInjection(),
                                strRetMatDate.PreventSQLInjection(),
                                strSendMatNo.PreventSQLInjection(),
                                strOperator.PreventSQLInjection(),
                                strShift.PreventSQLInjection());
           return sql;
        }
        
        //插入退料明细表
        private string  Save2(string strRetMatNo, string strRetMatDate, string strShift, string strOperator,
            string  strRetMatReason,DataRow  dtMatLotRow)
       {
           string sql = null;
           string strSTORE_MATERIAL_DETAIL_KEY = GetSTORE_MATERIAL_DETAIL_KEY(dtMatLotRow["MATLOT"].ToString());//根据lot在明细表捞取
           string strLGORT = GetLGORT(dtMatLotRow["MATLOT"].ToString());//根据lot在发料表捞取发料的时候在ERP的库位
           string strSendMatNo=GetSendMatNO(dtMatLotRow["MATLOT"].ToString());
           sql =string.Format(@"INSERT INTO WST_TL_ZMMLPO
                                    (ZMBLNR, AUFNR, CHARG, STORE_MATERIAL_DETAIL_KEY,MATNR, IN_MENGE,MENGE, LGORT, MEINS, ZTLSTA, PURPOSE, REF_EBELN, BARCODE,CREATE_TIME) 
                              VALUES('{0}',  '{1}' ,'{2}', '{3}'  ,                  '{4}', '{5}',   '{6}', '{7}', '{8}', '',     '',      '{9}',     '{10}', GETDATE())",
                            strRetMatNo.PreventSQLInjection(),
                            dtMatLotRow["WORKORDERNO"].ToString().PreventSQLInjection(),
                            dtMatLotRow["MATLOT"].ToString().PreventSQLInjection(),
                            strSTORE_MATERIAL_DETAIL_KEY.PreventSQLInjection(),
                            dtMatLotRow["MATCODE"].ToString().PreventSQLInjection(),
                            dtMatLotRow["RETURNQTY"].ToString().PreventSQLInjection(),
                            dtMatLotRow["RETURNQTY"].ToString().PreventSQLInjection(),
                            strLGORT.PreventSQLInjection(),
                            dtMatLotRow["UNIT"].ToString().PreventSQLInjection(),
                            strSendMatNo.PreventSQLInjection());
           return sql;
        }
        //更新lot当前数量
        private string Save3(string strRetMatNo, string strRetMatDate, string strShift, string strOperator, string strRetMatReason, DataRow dtMatLotRow)
       {
           string sql = null;
           string sql2 = string.Empty;
           DataSet ds = new DataSet();
           sql2 = @"SELECT *  
                   FROM WST_STORE_MATERIAL_DETAIL A 
                   LEFT JOIN WST_STORE_MATERIAL B  ON A.STORE_MATERIAL_KEY = B.STORE_MATERIAL_KEY
                   LEFT JOIN WST_STORE C ON C.STORE_KEY=B.STORE_KEY
                   LEFT JOIN POR_MATERIAL D ON B.MATERIAL_KEY=D.MATERIAL_KEY     
                   WHERE MATERIAL_LOT ='{0}' AND C.STORE_NAME ='{1}' AND D.MATERIAL_CODE='{2}'";
           ds = db.ExecuteDataSet(CommandType.Text, String.Format(sql2,
                                                                dtMatLotRow["MATLOT"].ToString().PreventSQLInjection(),
                                                                dtMatLotRow["STORE"].ToString().PreventSQLInjection(),
                                                                 dtMatLotRow["MATCODE"].ToString().PreventSQLInjection()));
           string storeMaterialDetailKey = ds.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"].ToString();
           float fReturnQty=float.Parse(dtMatLotRow["RETURNQTY"].ToString());
           sql =string.Format(@"UPDATE WST_STORE_MATERIAL_DETAIL 
                                SET CURRENT_QTY =CURRENT_QTY - {0}
                                WHERE STORE_MATERIAL_DETAIL_KEY = '{1}' ", 
                                fReturnQty,
                                storeMaterialDetailKey.PreventSQLInjection());
           return sql;
       }
        //组退给ERP的table
        private DataSet  Save4(string strRetMatNo, string strRetMatDate, string strShift, string strOperator, 
            string strRetMatReason, DataTable dtMatLotList)
        {
           DataSet dsReturn = new DataSet();

           DataTable dtReturn = new DataTable();
           dtReturn.TableName = "ZPP_RETURN_M";
           dtReturn.Columns.Add("MBLNR");
           dtReturn.Columns.Add("ITMNO");
           dtReturn.Columns.Add("BWART");
           dtReturn.Columns.Add("BDATE");
           dtReturn.Columns.Add("WERKS");

           dtReturn.Columns.Add("AUFNR");
           dtReturn.Columns.Add("MATNR");
           dtReturn.Columns.Add("CHARG");
           dtReturn.Columns.Add("LGORT");
           dtReturn.Columns.Add("ERFMG");

           dtReturn.Columns.Add("ERFME");
           dtReturn.Columns.Add("ZSOUCE");
           //入库库位，那个厂退那个库位
           string strLGORT = GetERPReturnStore(strRetMatNo.Substring(0, 1));
           //数据源
           string strDataSource = GetMESDataSource(strRetMatNo.Substring(0, 1));
           for (int i = 0; i < dtMatLotList.Rows.Count; i++)
           {
               DataRow drReturn = dtReturn.NewRow();
               drReturn["MBLNR"] = strRetMatNo;
               drReturn["ITMNO"] = i+1;
               drReturn["BWART"] = "262";
               drReturn["BDATE"] = strRetMatDate;
               drReturn["WERKS"] = "HZ01";//问胡和peter  工厂

               drReturn["AUFNR"] = dtMatLotList.Rows[i]["WORKORDERNO"];
               drReturn["MATNR"] = dtMatLotList.Rows[i]["MATCODE"];
               drReturn["CHARG"] = dtMatLotList.Rows[i]["MATLOT"];
               drReturn["LGORT"] = strLGORT;//入库库位
               drReturn["ERFMG"] = dtMatLotList.Rows[i]["RETURNQTY"];

               drReturn["ERFME"] = dtMatLotList.Rows[i]["UNIT"];
               drReturn["ZSOUCE"] = strDataSource; //数据源怎么转

               dtReturn.Rows.Add(drReturn);
           }
           dsReturn.Tables.Add(dtReturn);
           return dsReturn;
       }
        //通过属性类型，和其中一个属性的值，捞取另一个属性的值
        private string GetAttributeValue(string strCateName, string strAttriName1, string strAttriName2, string strAttriValue2)
        {
            string strAttriValue1 = null;
            DataSet dsReturn = new DataSet();
            try
            {
                //第一个参数 属性类型
                //第二个参数 要找的属性名
                //第三个参数 属性类型
                //第四个参数 已知的属性名
                //第五个参数 已知属性名的属性值
                string sql = @"SELECT ATTRIBUTE_VALUE
                                  FROM CRM_ATTRIBUTE
                                 WHERE ATTRIBUTE_KEY IN (
                                          SELECT ATTRIBUTE_KEY
                                            FROM BASE_ATTRIBUTE
                                           WHERE CATEGORY_KEY IN (SELECT CATEGORY_KEY
                                                                    FROM BASE_ATTRIBUTE_CATEGORY
                                                                   WHERE CATEGORY_NAME = '{0}') 
                                             AND ATTRIBUTE_NAME = '{1}')
                                   AND ITEM_ORDER IN ( 
                                          SELECT ITEM_ORDER
                                            FROM CRM_ATTRIBUTE
                                           WHERE ATTRIBUTE_KEY IN (
                                                    SELECT ATTRIBUTE_KEY
                                                      FROM BASE_ATTRIBUTE
                                                     WHERE CATEGORY_KEY IN (
                                                                        SELECT CATEGORY_KEY
                                                                          FROM BASE_ATTRIBUTE_CATEGORY
                                                                         WHERE CATEGORY_NAME =
                                                                                              '{2}')
                                                       AND ATTRIBUTE_NAME = '{3}')
                                             AND ATTRIBUTE_VALUE = '{4}')";

                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strCateName.PreventSQLInjection(), 
                                                                            strAttriName1.PreventSQLInjection(), 
                                                                            strCateName.PreventSQLInjection(), 
                                                                            strAttriName2.PreventSQLInjection(), 
                                                                            strAttriValue2.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetAttributeValue Error: " + ex.Message);
            }
            strAttriValue1 = dsReturn.Tables[0].Rows[0]["ATTRIBUTE_VALUE"].ToString();
            return strAttriValue1;
        }
        /// <summary>
        /// 根据ERP线别获取ERP工厂
        /// </summary>
        /// <param name="strFacCode"></param>
        /// <returns></returns>
        private string GetERPFac( string strFacCode)
        {
            string strERPFac = null;
            strERPFac = GetAttributeValue(BASEDATA_CATEGORY_NAME.MEScontrastERP, "ERPFACTORY", "ERPLINE", strFacCode);
            return strERPFac;
        }
        /// <summary>
        /// 根据第一批获取原来的发料单
        /// </summary>
        /// <param name="strMatLot"></param>
        /// <returns></returns>
        private string GetSendMatNO( string strMatLot)
        {
            string strSendMatNo = null;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT MBLNR SENDMATNO FROM WST_SAP_ISSURE WHERE CHARG ='{0}'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strMatLot.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSendMatNO Error: " + ex.Message);
            }
            strSendMatNo = dsReturn.Tables[0].Rows[0]["SENDMATNO"].ToString();
            return strSendMatNo;
        }
        /// <summary>
        /// 根据每批从明细表中获取明细表的key
        /// </summary>
        /// <param name="strMatLot"></param>
        /// <returns></returns>
        private string GetSTORE_MATERIAL_DETAIL_KEY(string strMatLot)
        {
            string strSTORE_MATERIAL_DETAIL_KEY = null;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT STORE_MATERIAL_DETAIL_KEY FROM WST_STORE_MATERIAL_DETAIL WHERE MATERIAL_LOT ='{0}'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strMatLot.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetSTORE_MATERIAL_DETAIL_KEY Error: " + ex.Message);
            }
            strSTORE_MATERIAL_DETAIL_KEY = dsReturn.Tables[0].Rows[0]["STORE_MATERIAL_DETAIL_KEY"].ToString();
            return strSTORE_MATERIAL_DETAIL_KEY;
        }

        //根据lot在发料表捞取库位
        private string GetLGORT(string strMatLot)
        {
            string strLGORT = null;
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT LGORT  FROM WST_SAP_ISSURE WHERE CHARG ='{0}'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, String.Format(sql, strMatLot.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetLGORT Error: " + ex.Message);
            }
            strLGORT = dsReturn.Tables[0].Rows[0]["LGORT"].ToString();
            return strLGORT;
        }

        /// <summary>
        /// 根据ERP线别取得ERP的入库库位
        /// </summary>
        /// <param name="strFacCode"></param>
        /// <returns></returns>
        private string GetERPReturnStore(string strFacCode)
        {
            string strERPReturnStore = null;
            strERPReturnStore = GetAttributeValue(BASEDATA_CATEGORY_NAME.MEScontrastERP, "ERPRETURNSTORE", "ERPLINE", strFacCode);
            return strERPReturnStore;
        }
        /// <summary>
        /// 根据ERP线别取得MES数据源
        /// </summary>
        /// <param name="strFacCode"></param>
        /// <returns></returns>
        private string GetMESDataSource(string strFacCode)
        {
            string strMESDataSource = null;
            strMESDataSource = GetAttributeValue(BASEDATA_CATEGORY_NAME.MEScontrastERP, "MESDATASOURCE", "ERPLINE", strFacCode);
            return strMESDataSource;
        }
        /// <summary>
        /// 根据线上仓名称和工序名称获取所有退料信息。
        /// </summary>
        /// <param name="strStore">线上仓名称，使用逗号分隔:store1,store2....</param>
        /// <param name="strOperation">工序名称，使用逗号分隔:op1,op2.....</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        public DataSet GetRetMatInfo(string strStore,string strOperation)
        {
            DataSet dsReturn = new DataSet();//定义一个对象
            string[] strArrStore = strStore.Split(',');
            string strCondition=null;
            for (int i = 0; i < strArrStore.Length; i++)
            {
                strCondition = strCondition + "'" + strArrStore[i].PreventSQLInjection() + "',";
            }
            strCondition = strCondition.Substring(0, strCondition.Length - 1);

            string[] strArrOperation = strOperation.Split(',');
            string strCondition2 = null;
            for (int i = 0; i < strArrOperation.Length; i++)
            {
                strCondition2 = strCondition2+ "'" + strArrOperation[i].PreventSQLInjection() + "',";
            }
            strCondition2 = strCondition2.Substring(0, strCondition2.Length - 1);


            try
            {
                string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ZMBLNR,A.CHARG,A.MATNR) INDEX1,
                                    A.ZMBLNR RETURNNO ,A.CHARG MATLOT ,A.MATNR MATCODE,F.MATERIAL_NAME MATDES,A.MEINS UNIT,A.IN_MENGE RETURNQTY,
                                    A.AUFNR WORKORDERNO ,C.MATERIAL_SUPPLIER SUPPLIER,E.OPERATION_NAME  OPERATION,E.STORE_NAME  STORE ,G.LOCATION_NAME FACROOM,
                                    A.CREATE_TIME RETURNDATE,B.SHIFT_VALUE SHIFT,B.ERNAM OPERATOR
                                FROM WST_TL_ZMMLPO A
                                LEFT JOIN WST_TL_ZMMLKO B ON A.ZMBLNR=B.ZMBLNR
                                LEFT JOIN WST_STORE_MATERIAL_DETAIL C ON A.STORE_MATERIAL_DETAIL_KEY=C.STORE_MATERIAL_DETAIL_KEY
                                LEFT JOIN WST_STORE_MATERIAL D ON C.STORE_MATERIAL_KEY=D.STORE_MATERIAL_KEY
                                LEFT JOIN WST_STORE E ON D.STORE_KEY =E.STORE_KEY
                                LEFT JOIN POR_MATERIAL F ON D.MATERIAL_KEY=F.MATERIAL_KEY
                                LEFT JOIN FMM_LOCATION G  ON E.LOCATION_KEY=G.LOCATION_KEY
                                WHERE E.STORE_NAME IN ({0}) AND  E.OPERATION_NAME IN ( {1} )";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, strCondition, strCondition2));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRetMatInfo Error: " + ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据查询条件获取工单退料信息。
        /// </summary>
        /// <param name="strMatLot">物料批号。</param>
        /// <param name="strMatCode">物料料号。</param>
        /// <param name="strMatDes">物料描述。</param>
        /// <param name="strOperation">工序名称。</param>
        /// <param name="strStore">线上仓名称。</param>
        /// <param name="strFacRoom">车间名称。</param>
        /// <param name="strSupplier">供应商名称。</param>
        /// <param name="strShift">班别名。</param>
        /// <param name="strOperator">操作人。</param>
        /// <param name="strFromRetDate">退料日期（开始）</param>
        /// <param name="strToRetDate">退料日期（结束）</param>
        /// <param name="strRetMatNo">退料单号。</param>
        /// <param name="strOperationALL">工序名称，使用逗号分隔:op1,op2.....</param>
        /// <param name="strStoreALL">线上仓名称，使用逗号分隔:store1,store2....</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        public DataSet GetWoRetMatInof(string strMatLot, string strMatCode, string strMatDes, string strOperation,
            string strStore, string strFacRoom, string strSupplier, string strShift, 
            string strOperator, string strFromRetDate, string strToRetDate, string strRetMatNo,
            string strOperationALL, string strStoreALL)
        {
            //1、根据物料批号（左匹配模糊查询），物料编码（左匹配模糊查询），物料描述（左匹配模糊查询），工序名称，线上仓名称，工厂车间，设备名称，
            //供应商（左匹配模糊查询），班次，员工号（左匹配模糊查询），退料单号
            //退料日期区间从WST_TL_ZMMLKO，WST_TL_ZMMLPO，WST_STORE_MATERIAL_DETAIL，WST_STORE_MATERIAL分页获取数据（每页默认为20行记录）。											
            //2、若工序名称为空以PROPERTY_FIELDS.OPERATIONS的值作为工序名称的查询条件，若线上仓名称为空以PROPERTY_FIELDS.STORES的值作为线上仓名称的查询条件，
            db = DatabaseFactory.CreateDatabase();//实例化对象
            DataSet dsReturn = new DataSet();//定义一个对象
            string strCondition ="1=1";
            //批号
            if (strMatLot.Trim().Length != 0)
            {
                strCondition = strCondition + " AND MATLOT LIKE '" + strMatLot.Trim().PreventSQLInjection() + "%' ";
            }
            //编码
            if (strMatCode.Trim().Length != 0)
            {
                strCondition = strCondition + " AND MATCODE LIKE '" + strMatLot.Trim().PreventSQLInjection() + "%'  ";
            }          
            //物料描述
            if (strMatDes.Trim().Length != 0)
            {
                strCondition = strCondition + " AND MATDES LIKE '" + strMatLot.Trim().PreventSQLInjection() + "%' ";
            }
           
            //工序
            if (strOperation.Trim().Length != 0)
            {
                strCondition = strCondition + " AND OPERATION='" + strOperation.Trim().PreventSQLInjection() + "'";
            }
            else
            {
                string[] strArrOperation = strOperationALL.Split(',');//strOperationALL
                strCondition = strCondition + " AND OPERATION IN (";
                for (int i = 0; i < strArrOperation.Length; i++)
                {
                    strCondition = strCondition + "'" + strArrOperation[i].PreventSQLInjection() + "',";
                }
                strCondition = strCondition.Substring(0, strCondition.Length - 1);
                strCondition = strCondition + ") ";
            }
            //仓库
            if (strStore.Trim().Length != 0)
            {
                strCondition = strCondition + " AND STORE='" + strOperation.Trim().PreventSQLInjection() + "'";
            }
            else
            {
                string[] strArrStore = strStoreALL.Split(',');//strOperationALL
                strCondition = strCondition + " AND STORE IN (";
                for (int i = 0; i < strArrStore.Length; i++)
                {
                    strCondition = strCondition + "'" + strArrStore[i].PreventSQLInjection() + "',";
                }
                strCondition = strCondition.Substring(0, strCondition.Length - 1);
                strCondition = strCondition + ") ";
            }
            //工厂车间
            if (strFacRoom.Trim().Length != 0)
            {
                strCondition = strCondition + " AND FACROOM='" + strFacRoom.Trim().PreventSQLInjection() + "'";
            }
            //供应商s
            if (strSupplier.Trim().Length != 0)
            {
                strCondition = strCondition + " AND SUPPLIER LIKE  '" + strSupplier.Trim().PreventSQLInjection() + "%'";
            }
            // string strToRetDate, string strRetMatNo, string strOperationALL, string strStoreALL)

            //班次
            if (strShift.Trim().Length != 0)
            {
                strCondition = strCondition + " AND SHIFT='" + strShift.Trim().PreventSQLInjection() + "'";
            }
            //员工
            if (strOperator.Trim().Length != 0)
            {
                strCondition = strCondition + " AND OPERATION LIKE  '" + strOperator.Trim().PreventSQLInjection() + "%'";
            }
            try
            {
                string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ZMBLNR,A.CHARG,A.MATNR) INDEX1,
                                    A.ZMBLNR RETURNNO ,A.CHARG MATLOT ,A.MATNR MATCODE,F.MATERIAL_NAME MATDES,A.MEINS UNIT,A.IN_MENGE RETURNQTY,
                                    A.AUFNR WORKORDERNO ,C.MATERIAL_SUPPLIER SUPPLIER,E.OPERATION_NAME  OPERATION,E.STORE_NAME  STORE,G.LOCATION_NAME FACROOM,
                                    A.CREATE_TIME RETURNDATE,B.SHIFT_VALUE SHIFT,B.ERNAM OPERATOR
                                FROM WST_TL_ZMMLPO A
                                LEFT JOIN WST_TL_ZMMLKO B ON A.ZMBLNR=B.ZMBLNR
                                LEFT JOIN WST_STORE_MATERIAL_DETAIL C ON A.STORE_MATERIAL_DETAIL_KEY=C.STORE_MATERIAL_DETAIL_KEY
                                LEFT JOIN WST_STORE_MATERIAL D ON C.STORE_MATERIAL_KEY=D.STORE_MATERIAL_KEY
                                LEFT JOIN WST_STORE E ON D.STORE_KEY =E.STORE_KEY
                                LEFT JOIN POR_MATERIAL F ON D.MATERIAL_KEY=F.MATERIAL_KEY
                                LEFT JOIN FMM_LOCATION G  ON E.LOCATION_KEY=G.LOCATION_KEY 
                                WHERE  {0} ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, strCondition));
            }
            catch (Exception ex)
            {
                LogService.LogError("GetWoRetMatInof Error: " + ex.Message);
            }
            return dsReturn;
        }
         //根据退料单得到退料信息,这里需要修改让秀出来的好看一点，是第一个画面的  修改以后
        /// <summary>
        /// 根据退料单得到退料信息。
        /// </summary>
        /// <param name="strRetMatList">退料单号。</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        public DataSet GetRetMatInfo1(string strRetMatList)
        {
            db = DatabaseFactory.CreateDatabase();//实例化对象
            DataSet dsReturn = new DataSet();//定义一个对象
            try
            {
                string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY A.ZMBLNR,A.CHARG,A.MATNR) INDEX1,
                                    A.ZMBLNR RETURNNO ,A.CHARG MATLOT ,A.MATNR MATCODE,F.MATERIAL_NAME MATDES,A.MEINS UNIT,A.IN_MENGE RETURNQTY,
                                    A.AUFNR WORKORDERNO ,C.MATERIAL_SUPPLIER SUPPLIER,E.OPERATION_NAME  OPERATION,E.STORE_NAME  STORE ,G.LOCATION_NAME FACROOM,
                                    A.CREATE_TIME RETURNDATE,B.SHIFT_VALUE SHIFT,B.ERNAM OPERATOR
                                FROM WST_TL_ZMMLPO A
                                LEFT JOIN WST_TL_ZMMLKO B ON A.ZMBLNR=B.ZMBLNR
                                LEFT JOIN WST_STORE_MATERIAL_DETAIL C ON A.STORE_MATERIAL_DETAIL_KEY=C.STORE_MATERIAL_DETAIL_KEY
                                LEFT JOIN WST_STORE_MATERIAL D ON C.STORE_MATERIAL_KEY=D.STORE_MATERIAL_KEY
                                LEFT JOIN WST_STORE E ON D.STORE_KEY =E.STORE_KEY
                                LEFT JOIN POR_MATERIAL F ON D.MATERIAL_KEY=F.MATERIAL_KEY
                                LEFT JOIN FMM_LOCATION G  ON E.LOCATION_KEY=G.LOCATION_KEY
                                WHERE A.ZMBLNR = '{0}' ";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql, strRetMatList.PreventSQLInjection()));

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRetMatInfo Error: " + ex.Message);
            }
            return dsReturn;
        }

        //删除
        /// <summary>
        /// 获取SAP中的退料单是否删除。
        /// </summary>
        /// <param name="dsIMPORT">包含退料单信息的数据集对象。</param>
        /// <returns>true:退料单在SAP中已删除。false:退料单未删除。</returns>
        public bool DeleteMat(DataSet dsIMPORT)
        {
            bool bltmp = false;
            DataSet dsOut = new DataSet();
            try
            { 
                //调用RFC
                AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_RET", dsIMPORT, out dsOut);
            }
            catch (Exception ex)
            {
                LogService.LogError("WOMaterialReturnEngine Save Error: " + ex.Message);
            }
            finally
            {
            }

            if (Convert.ToString(dsOut.Tables[0].Rows[0]["LVORM"])=="D")//判断条件
            { 
                bltmp = true;
            }
            return bltmp;
        }

        //删除2 如果SAP中退料单已删除，则从WST_TL_ZMMLKO，WST_TL_ZMMLPO删除退料单号对应的退料单记录,更新明细
        /// <summary>
        /// 删除退料单。
        /// </summary>
        /// <param name="strReturnNo">退料单号。</param>
        /// <returns>true：删除成功。false：删除失败。</returns>
        public bool DeleteMat2(string strReturnNo)
        {
            DataTable dtMatLotList = new DataTable();//捞取这个退料单上的批次和数量
            dtMatLotList = GetRetMatNoInfo(strReturnNo);


            bool bltmp = false;
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                DbCommand dbCom = dbCon.CreateCommand();
                dbCom.Transaction = dbTrans;
                //1.删除WST_TL_ZMMLKO
                dbCom.CommandText = Delete1(strReturnNo);
                dbCom.ExecuteNonQuery();
                //2.删除WST_TL_ZMMLPO
                dbCom.CommandText = Delete2(strReturnNo);
                dbCom.ExecuteNonQuery();
                //3.更新数量
                for (int i = 0; i < dtMatLotList.Rows.Count; i++)
                {
                    dbCom.CommandText = Delete3(dtMatLotList.Rows[0]);
                    dbCom.ExecuteNonQuery();
                }
                
                dbTrans.Commit();
                bltmp = true;

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                LogService.LogError("WOMaterialReturnEngine Save Error: " + ex.Message);
                bltmp = false;

            }
            finally
            {
                dbCon.Close();
            }

            return bltmp;
        }

        //取得退料单的物料批号
        private DataTable GetRetMatNoInfo(string strReturnNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sql = @"SELECT CHARG MATLOT,MENGE RETURNQTY FROM WST_TL_ZMMLPO WHERE ZMBLNR='{0}'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, string.Format(sql,strReturnNo.PreventSQLInjection()));
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRetMatNoInfo Error: " + ex.Message);
            }
            return dsReturn.Tables[0];
        }
        //删除退料单表
        private string Delete1(string strReturnNo)
        {
            string sql = "DELETE FROM WST_TL_ZMMLKO WHERE ZMBLNR='" + strReturnNo.PreventSQLInjection() + "'";
            return sql;
        }
        //删除退料单里边的具体批
        private string Delete2(string strReturnNo)
        {
            string sql = "DELETE FROM WST_TL_ZMMLPO WHERE ZMBLNR='" + strReturnNo.PreventSQLInjection() + "'";
            return sql;
        }
        //更新物料明细的数量
        private string Delete3(DataRow dr)
        {
            float  itemp=float.Parse(dr["RETURNQTY"].ToString());
            string matLot=dr["MATLOT"].ToString();
            string sql =string.Format(@"UPDATE  WST_STORE_MATERIAL_DETAIL 
                                        SET RECEIVE_QTY=RECEIVE_QTY +{0},CURRENT_QTY=CURRENT_QTY + {0}
                                        WHERE MATERIAL_LOT='{1}'",
                                        itemp,
                                        matLot.PreventSQLInjection());
            return sql;
        }
    }


}

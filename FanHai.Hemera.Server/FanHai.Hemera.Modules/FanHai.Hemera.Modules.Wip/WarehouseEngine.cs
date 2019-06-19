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

namespace FanHai.Hemera.Modules.Wip
{
    public class WarehouseEngine : AbstractEngine, IWarehouseEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public WarehouseEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public override void Initialize()
        {

        }

        #region IWarehouseEngine 成员

        public DataSet SearchPallet(string palletNo)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT PALLET_NO,SAP_NO,CS_DATA_GROUP,WORKNUMBER,LOT_NUMBER_QTY,INWH_QTY FROM WIP_CONSIGNMENT 
                                                    WHERE 
                                                    ISFLAG = '1' 
                                                    AND PALLET_NO ='{0}'", palletNo);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchPallet Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion


        #region IWarehouseEngine 成员


        public DataSet SearchPelletInfToList(string PalletNo, string work_order, string part_number)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                #region
                //                string sqlCommon = string.Format(@"SELECT   A.WORK_NUMBER,
                //                                                            C.LINE_NAME,
                //                                                            A.PRO_LEVEL AS XP001,
                //                                                            A.CONSIGNMENT_KEY,
                //                                                            convert(char(20),SUM(H.COEF_PMAX)) AS XP005,
                //                                                            A.POWER_LEVEL AS XP006,
                //                                                            B.PALLET_NO AS XP004,
                //                                                            convert(decimal(8,3),COUNT(1)) AS MENGE, 
                //                                                            A.PART_NUMBER AS MATNR,
                //                                                            C.PART_NUMBER,
                //                                                            C.REVENUE_TYPE AS XP002,
                //                                                            E.PART_DESC AS MAKTX,
                //                                                            REPLACE(REPLACE(REPLACE(D.TOLERANCE,'~','/'),'-+','/+'),'-0','0') AS TOLERANCE,
                //                                                            F.STORAGE_LOCATION AS LGORT,'【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MIN,'0'),'.'))+','+
                //                                                            CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MAX,'0'),'.'))+'】' AS XP011
                //                                                    FROM dbo.WIP_CONSIGNMENT_DETAIL A 
                //                                                    INNER JOIN dbo.WIP_IV_TEST H ON H.LOT_NUM = A.LOT_NUMBER AND H.VC_DEFAULT = '1'
                //                                                    INNER JOIN dbo.WIP_CONSIGNMENT B ON B.CONSIGNMENT_KEY = A.CONSIGNMENT_KEY 
                //			                                                    AND B.ISFLAG = 1 AND B.CS_DATA_GROUP > 1
                //                                                    INNER JOIN dbo.POR_WORK_ORDER C ON A.WORK_NUMBER = C.ORDER_NUMBER AND C.ORDER_STATE IN ('TECO','REL')
                //                                                    LEFT JOIN dbo.POR_WO_PRD D ON D.WORK_ORDER_KEY  = C.WORK_ORDER_KEY
                //                                                                AND A.PART_NUMBER = D.PART_NUMBER
                //                                                                AND D.IS_USED='Y'             
                //                                                    LEFT JOIN POR_PART E ON A.PART_NUMBER = E.PART_ID AND E.PART_STATUS = 1 
                //                                                    LEFT JOIN dbo.POR_PART_BYPRODUCT F ON A.PART_NUMBER = F.PART_NUMBER 
                //                                                                AND F.MAIN_PART_NUMBER = C.PART_NUMBER 
                //                                                                AND F.IS_USED = 'Y'
                //                                                    LEFT JOIN POR_WO_PRD_PS G ON G.WORK_ORDER_KEY = C.WORK_ORDER_KEY 
                //                                                                AND G.PART_NUMBER = A.PART_NUMBER
                //                                                                AND G.PMAXSTAB=A.POWER_LEVEL
                //                                                                AND G.IS_USED='Y'
                //                                                    WHERE A.WORK_NUMBER = '{0}' AND B.PALLET_NO = '{1}'
                //                                                    GROUP BY A.WORK_NUMBER,C.LINE_NAME,A.PRO_LEVEL,A.CONSIGNMENT_KEY,A.POWER_LEVEL,B.PALLET_NO,A.PART_NUMBER,
                //                                                    C.PART_NUMBER,C.REVENUE_TYPE,E.PART_DESC,F.STORAGE_LOCATION,D.TOLERANCE,G.P_MIN,G.P_MAX",  work_order,PalletNo);
                //                string sqlCommon = string.Format(@"SELECT   A.WORK_NUMBER,
                //                                                            C.LINE_NAME,
                //                                                            A.PRO_LEVEL AS XP001,
                //                                                            A.CONSIGNMENT_KEY,
                //                                                            convert(char(20),SUM(H.COEF_PMAX)) AS XP005,
                //                                                            A.POWER_LEVEL AS XP006,
                //                                                            B.PALLET_NO AS XP004,
                //                                                            convert(decimal(8,3),COUNT(1)) AS MENGE, 
                //                                                            A.PART_NUMBER AS MATNR,
                //                                                            C.PART_NUMBER,
                //                                                            C.REVENUE_TYPE AS XP002,
                //                                                            E.PART_DESC AS MAKTX,
                //                                                            REPLACE(REPLACE(REPLACE(D.TOLERANCE,'~','/'),'-+','/+'),'-0','0') AS TOLERANCE,
                //                                                            F.STORAGE_LOCATION AS LGORT,
                //                                                            A.COLOR AS XP009,
                //                                                            (CASE WHEN I.PS_SUB_CODE IS NOT NULL THEN '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(I.P_DTL_MIN,'0'),'.'))+','+
                //                                                                                                                CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(I.P_DTL_MAX,'0'),'.'))+'】'
                //			                                                      ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MIN,'0'),'.'))+','+
                //                                                                                                                CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MAX,'0'),'.'))+'】' END) AS XP011
                //                                                    FROM dbo.WIP_CONSIGNMENT_DETAIL A 
                //                                                    INNER JOIN dbo.WIP_IV_TEST H ON H.LOT_NUM = A.LOT_NUMBER AND H.VC_DEFAULT = '1'
                //                                                    INNER JOIN dbo.WIP_CONSIGNMENT B ON B.CONSIGNMENT_KEY = A.CONSIGNMENT_KEY 
                //                                                                AND B.ISFLAG = 1 AND B.CS_DATA_GROUP > 1
                //                                                    INNER JOIN dbo.POR_WORK_ORDER C ON A.WORK_NUMBER = C.ORDER_NUMBER AND C.ORDER_STATE IN ('TECO','REL')
                //                                                    LEFT JOIN dbo.POR_WO_PRD D ON D.WORK_ORDER_KEY  = C.WORK_ORDER_KEY
                //                                                                AND A.PART_NUMBER = D.PART_NUMBER
                //                                                                AND D.IS_USED='Y'             
                //                                                    LEFT JOIN POR_PART E ON A.PART_NUMBER = E.PART_ID AND E.PART_STATUS = 1 
                //                                                    LEFT JOIN dbo.POR_PART_BYPRODUCT F ON A.PART_NUMBER = F.PART_NUMBER 
                //                                                                AND F.MAIN_PART_NUMBER = C.PART_NUMBER 
                //                                                                AND F.IS_USED = 'Y'
                //                                                    LEFT JOIN POR_WO_PRD_PS G ON G.WORK_ORDER_KEY = C.WORK_ORDER_KEY 
                //                                                                AND G.PART_NUMBER = A.PART_NUMBER
                //                                                                AND G.PMAXSTAB=A.POWER_LEVEL
                //                                                                AND G.IS_USED='Y'
                //                                                    LEFT JOIN dbo.POR_WO_PRD_PS_SUB I ON G.WORK_ORDER_KEY = I.WORK_ORDER_KEY
                //			                                                    AND G.PART_NUMBER = I.PART_NUMBER 
                //			                                                    AND G.POWERSET_KEY = I.POWERSET_KEY
                //			                                                    AND H.I_PKID=I.PS_SUB_CODE
                //			                                                    AND I.IS_USED='Y'
                //                                                    WHERE A.WORK_NUMBER = '{0}' AND B.PALLET_NO = '{1}'
                //                                                    GROUP BY A.WORK_NUMBER,C.LINE_NAME,A.PRO_LEVEL,A.CONSIGNMENT_KEY,A.POWER_LEVEL,B.PALLET_NO,A.PART_NUMBER,A.COLOR,
                //                                                    C.PART_NUMBER,C.REVENUE_TYPE,E.PART_DESC,F.STORAGE_LOCATION,D.TOLERANCE,G.P_MIN,G.P_MAX,I.P_DTL_MIN,I.P_DTL_MAX,I.PS_SUB_CODE", work_order, PalletNo);
                
                //C.DESCRIPTIONS AS MAKTX,
                #endregion
                string sqlCommon = string.Format(@"SELECT 
                                                            C.LINE_NAME,
                                                            A.PRO_LEVEL AS XP001,
                                                            A.CONSIGNMENT_KEY,
                                                            convert(char(20),SUM(H.COEF_PMAX)) AS XP005,
                                                            A.POWER_LEVEL AS XP006,
                                                            B.PALLET_NO AS XP004,
                                                            convert(decimal(8,3),COUNT(1)) AS MENGE, 
                                                            A.PART_NUMBER AS MATNR,
                                                            C.REVENUE_TYPE AS XP002,
                                                            E.PART_DESC AS MAKTX,
                                                            REPLACE(REPLACE(REPLACE(D.TOLERANCE,'~','/'),'-+','/+'),'-0','0') AS TOLERANCE,
                                                            F.STORAGE_LOCATION AS LGORT,
                                                            A.COLOR AS XP009,
                                                            (CASE WHEN G.SUB_PS_WAY = '电流' AND ISNULL(I.PS_SUB_CODE,0)>0 THEN 
	                                                        SUBSTRING(POWERLEVEL,CHARINDEX('-',POWERLEVEL)+1,LEN(POWERLEVEL)-CHARINDEX('-',POWERLEVEL))
	                                                        ELSE '否' END) AS DL,
                                                             (CASE WHEN G.SUB_PS_WAY = '功率' AND ISNULL(I.PS_SUB_CODE,0)>0 THEN 
                                                                            '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(I.P_DTL_MIN,'0'),'.'))+'，'+
                                                                                                                                CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(I.P_DTL_MAX,'0'),'.'))+'】'
                                                                             ELSE '【'+ CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MIN,'0'),'.'))+'，'+
                                                                                                                                CONVERT(VARCHAR(20),dbo.TRIMSTR(dbo.TRIMSTR(G.P_MAX,'0'),'.'))+'】' END) AS XP011
                                                    FROM dbo.WIP_CONSIGNMENT_DETAIL A 
                                                    INNER JOIN dbo.WIP_IV_TEST H ON H.LOT_NUM = A.LOT_NUMBER AND H.VC_DEFAULT = '1'
                                                    INNER JOIN dbo.WIP_CONSIGNMENT B ON B.CONSIGNMENT_KEY = A.CONSIGNMENT_KEY 
                                                                AND B.ISFLAG = 1 AND B.CS_DATA_GROUP > 1
                                                    INNER JOIN dbo.POR_WORK_ORDER C ON A.WORK_NUMBER = C.ORDER_NUMBER AND C.ORDER_STATE IN ('TECO','REL')
                                                    LEFT JOIN dbo.POR_WO_PRD D ON D.WORK_ORDER_KEY  = C.WORK_ORDER_KEY
                                                                AND A.PART_NUMBER = D.PART_NUMBER
                                                                AND D.IS_USED='Y'             
                                                    LEFT JOIN POR_PART E ON A.PART_NUMBER = E.PART_ID AND E.PART_STATUS = 1 
                                                    LEFT JOIN dbo.POR_PART_BYPRODUCT F ON A.PART_NUMBER = F.PART_NUMBER 
                                                                AND F.MAIN_PART_NUMBER = C.PART_NUMBER 
                                                                AND F.IS_USED = 'Y'
                                                    LEFT JOIN POR_WO_PRD_PS G ON G.WORK_ORDER_KEY = C.WORK_ORDER_KEY 
                                                                AND G.PART_NUMBER = A.PART_NUMBER
                                                                AND G.PMAXSTAB=A.POWER_LEVEL
                                                                AND G.PS_SEQ = H.I_IDE
                                                                AND G.IS_USED='Y'
                                                    LEFT JOIN dbo.POR_WO_PRD_PS_SUB I ON G.WORK_ORDER_KEY = I.WORK_ORDER_KEY
                                                                AND G.PART_NUMBER = I.PART_NUMBER 
                                                                AND G.POWERSET_KEY = I.POWERSET_KEY
                                                                AND H.I_PKID=I.PS_SUB_CODE
                                                                AND I.IS_USED='Y'
                                                    WHERE  B.PALLET_NO = '{0}' ", PalletNo);
                if (!string.IsNullOrEmpty(work_order))
                {
                    sqlCommon += string.Format(" AND A.WORK_NUMBER = '{0}'", work_order);
                }
                if (!string.IsNullOrEmpty(part_number))
                {
                    sqlCommon += string.Format(" AND A.PART_NUMBER = '{0}'", part_number);
                }
                sqlCommon += @" GROUP BY C.LINE_NAME,A.PRO_LEVEL,A.CONSIGNMENT_KEY,A.POWER_LEVEL,B.PALLET_NO,A.PART_NUMBER,A.COLOR,
                                    C.REVENUE_TYPE,F.STORAGE_LOCATION,D.TOLERANCE,G.P_MIN,G.P_MAX,I.P_DTL_MIN,I.P_DTL_MAX,I.PS_SUB_CODE,G.SUB_PS_WAY,I.POWERLEVEL,E.PART_DESC";

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("SearchPelletInfToList Error: " + ex.Message);
            }
            return dsReturn;

        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet CreateRkKoPo(DataSet dsIn)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsInRfc = new DataSet();
            DataTable dtInRfc = new DataTable();
            dtInRfc.Columns.Add("LFSNR");//入库单号
            dtInRfc.Columns.Add("WERKS");//工厂号
            dtInRfc.Columns.Add("ITMNO");//序号 
            dtInRfc.Columns.Add("BWART");//物料移动类型
            dtInRfc.Columns.Add("CDATE");//创建时间
            dtInRfc.Columns.Add("AUFNR");//工单号
            dtInRfc.Columns.Add("MATNR");//工单料号
            dtInRfc.Columns.Add("MATNR1");//实际入库产品编码
            dtInRfc.Columns.Add("LGORT");//入库仓
            dtInRfc.Columns.Add("CHARG");//批次
            dtInRfc.Columns.Add("LMNGA");//数量
            dtInRfc.Columns.Add("XP001");//质量等级
            dtInRfc.Columns.Add("XP002");//保税手册号
            dtInRfc.Columns.Add("XP003");//电流分档
            dtInRfc.Columns.Add("XP004");//托盘号
            dtInRfc.Columns.Add("XP005");//实际功率
            dtInRfc.Columns.Add("XP006");//标称功率
            dtInRfc.Columns.Add("XP007");//分档方式
            dtInRfc.Columns.Add("XP008");//红外等级下限 
            dtInRfc.Columns.Add("XP009");//衰减因子 
            dtInRfc.Columns.Add("XP010");//FF下限% 
            dtInRfc.Columns.Add("XP011");//晶硅功率范围    
            dtInRfc.TableName = "ZPP_WORKED";
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlKo = @" INSERT INTO dbo.WIP_RK_ZXBKO
                                    (ZMBLNRKEY,ZMBLNR,WERKS,AUFNR,
                                    LVORM,DEPT,CDATE,CREATOR,
                                    WORK_ORDER_MATNR,MAKTX,MEMO1,LINE) 
                                    VALUES
                                    ('{0}','{1}','{2}','{3}',0,'{4}',GETDATE(),'{5}','{6}','{7}','{8}','{9}') ";

                string sqlPo = @" INSERT INTO dbo.WIP_RK_ZXBPO
                                    (ZMBLNRDETILKEY,ZMBLNR,ZEILE,ITEMSTATUS,
                                    BWART,MATNR,MENGE,KEYNO,REMARK,
                                    XP001,XP002,XP003,XP004,
                                    XP005,XP006,XP007,XP008,
                                    XP009,XP010,XP011,
                                    CREATE_TIME,LGORT)
                                    VALUES
                                    ('{0}','{1}',{2},0,
                                    '{3}','{4}',{5},'{6}',
                                    '{7}','{8}','{9}','{10}',
                                    '{11}', '{12}','{13}','{14}',
                                    '{15}', '{16}','{17}','{18}',
                                    GETDATE(),'{19}') ";
                ///传入参数 入库单号和行号
                ///修改参数  批次号
                /// 根据RFC调用传回的信息更新入库明细表批次号
                string sqlUpdatePiCiBuyoutSet = @"UPDATE dbo.WIP_RK_ZXBPO SET CHARG = '{0}' WHERE ZMBLNR ='{1}' AND ZEILE = '{2}'";
                ///传入参数托号
                ///修改状态为3 已入库
                /// 更新包装表中托状态
                //string sqlUpdateConsignmentStatus = @"UPDATE dbo.WIP_CONSIGNMENT SET CS_DATA_GROUP = '3',INWH_QTY = '{0}' WHERE PALLET_NO = '{1}' AND ISFLAG = 1";  //状态不卡控
                string sqlUpdateConsignmentStatus = @"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY = '{0}' WHERE PALLET_NO = '{1}' AND ISFLAG = 1";
                string sqlUpdateConsignmentStatus01 = @"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY = '{0}' WHERE PALLET_NO = '{1}' AND ISFLAG = 1";
                ///sql定义end---------------------------------------------------------------------------------------------------------------------------

                ///数据处理start-------------------------------------------------------------------------------------------------------------------
                DataTable dtKo = dsIn.Tables["WIP_RK_KO"];
                DataTable dtPo = dsIn.Tables["WIP_RK_PO"];
                string ZMBLNRKEY = UtilHelper.GenerateNewKey(0);
                //产品标识(1)＋线别(2) + 年(2) + 月(1) + 日(2) + 流水码(4)产品标识：薄膜→P；杭州晶硅电池→C；晶硅组件→J；大写字母
                string YYDD = GetYYMD();
                string ZMBLNR1 = "J" + dtKo.Rows[0]["LINE"].ToString().Trim() + YYDD;
                string sqlMax = string.Format(@"SELECT max(substring(ZMBLNR,9,4)) as maxLotNumber from dbo.WIP_RK_ZXBKO where ZMBLNR like '{0}%'", ZMBLNR1);
                DataSet dsReturn = db.ExecuteDataSet(CommandType.Text, sqlMax);
                if (dsReturn.Tables[0].Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dsReturn.Tables[0].Rows[0]["maxLotNumber"].ToString()))
                    {
                        ZMBLNR1 += "5001";
                    }
                    else
                    {
                        int maxlotnumber = int.Parse(dsReturn.Tables[0].Rows[0]["maxLotNumber"].ToString());
                        if (maxlotnumber < 9999)
                            ZMBLNR1 += (maxlotnumber + 1).ToString("0000");
                    }
                }
                //调用sap获取入库单状态 主要是为了判定入库单是否存在如果存在了就要把流水号加一位,
                //持续判定,知道不存在时再使用生存的入库单号做入库
                string werks = dtKo.Rows[0]["WERKS"].ToString().Trim().PreventSQLInjection();
                //递归函数
                string ZMBLNR = RfcSapRkStatus(ZMBLNR1, werks);


                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                string year = currentTime.Year.ToString("00");
                string Month = currentTime.Month.ToString("00");
                string Day = currentTime.Day.ToString("00");
                string Hour = currentTime.Hour.ToString("00");
                string Minute = currentTime.Minute.ToString("00");
                string Second = currentTime.Minute.ToString("00");
                string Cdate = year + Month + Day + Hour + Minute + Second;
                ///数据处理end---------------------------------------------------------------------------------------------------------------------------


                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                //插入抬头表信息
                string strInsertInToKo = string.Format(sqlKo,
                  ZMBLNRKEY.PreventSQLInjection(),                                            //主键
                  ZMBLNR,                                                                     //入库单号 
                  dtKo.Rows[0]["WERKS"].ToString().Trim().PreventSQLInjection(),              //工厂
                  dtKo.Rows[0]["WORK_ORDER"].ToString().Trim().PreventSQLInjection(),         //工单号
                  dtKo.Rows[0]["DEPT"].ToString().Trim().PreventSQLInjection(),               //部门
                  dtKo.Rows[0]["CREATOR"].ToString().Trim().PreventSQLInjection(),            //工单料号
                  dtKo.Rows[0]["WORK_ORDER_MATNR"].ToString().Trim().PreventSQLInjection(),   //描述   
                  dtKo.Rows[0]["MAKTX"].ToString().Trim().PreventSQLInjection(),              //物料描述
                  dtKo.Rows[0]["MEM01"].ToString().Trim().PreventSQLInjection(),              //备注
                  dtKo.Rows[0]["LINE"].ToString().Trim().PreventSQLInjection()                //线别                  
                  );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, strInsertInToKo);
                //生成插入入库明细记录的SQL
                foreach (DataRow dr in dtPo.Rows)
                {
                    string keyPo = UtilHelper.GenerateNewKey(0);                       //入库单明细主键
                    string strKoInPo = ZMBLNR;                                         //入库单号
                    int strRowNumber = Convert.ToInt32(dr["ROWNUMBER"].ToString().Trim().PreventSQLInjection());     //行号
                    string strBwart = dr["BWART"].ToString().Trim().PreventSQLInjection();                           //移动类型
                    string strMatnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();                           //物料号
                    decimal strMenge = Convert.ToDecimal(dr["MENGE"].ToString().Trim().PreventSQLInjection());        //数量       
                    string keyPalletN0 = dr["CONSIGNMENT_KEY"].ToString().Trim().PreventSQLInjection();              //包装表主键
                    string strRemark = dr["REMARK"].ToString().Trim().PreventSQLInjection();                         //备注
                    string lgort = dr["LGORT"].ToString().Trim().PreventSQLInjection();                         //库位


                    //生成向入库明细表插入记录的SQL语句
                    string sqlDetail = string.Format(sqlPo,
                                                    keyPo,             //主键
                                                    strKoInPo,         //入库单号
                                                    strRowNumber,      //行号
                                                    strBwart,          //移动类型
                                                    strMatnr,          //物料号
                                                    strMenge,          //数量
                                                    keyPalletN0,       //托号主键
                                                    strRemark,         //备注
                                                    dr["XP001"].ToString().Trim().PreventSQLInjection(),    //质量等级
                                                    dr["XP002"].ToString().Trim().PreventSQLInjection(),    //保税手册号
                                                    dr["DL"].ToString().Trim().PreventSQLInjection(),       //电流分档
                                                    dr["XP004"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP005"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP006"].ToString().Trim().PreventSQLInjection(),
                                                    dr["TOLERANCE"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP008"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP009"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP010"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP011"].ToString().Trim().PreventSQLInjection(),
                                                    lgort
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                    //修改包装表状态为3已入库 判定入库数量加上已经入库数量和托数量对比是否一致
                    //一致则修改入库状态和数量 不一致则不修改入库状态 只改入库数量
                    string strGetQTY = string.Format(@"SELECT LOT_NUMBER_QTY,INWH_QTY FROM dbo.WIP_CONSIGNMENT WHERE PALLET_NO = '{0}' 
                                                            AND ISFLAG = 1 ",
                                                            dr["XP004"].ToString().Trim().PreventSQLInjection());
                    DataSet dsGetQTY = db.ExecuteDataSet(dbTrans, CommandType.Text, strGetQTY);
                    int sum = Convert.ToInt32(dsGetQTY.Tables[0].Rows[0]["INWH_QTY"].ToString().Trim()) + Convert.ToInt32(strMenge);
                    int lotNumberQTY = Convert.ToInt32(dsGetQTY.Tables[0].Rows[0]["LOT_NUMBER_QTY"].ToString().Trim());
                    if (sum == lotNumberQTY)
                    {
                        string strUpdate = string.Format(sqlUpdateConsignmentStatus, sum, dr["XP004"].ToString().Trim().PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                        //sqlCommandList.Add(strUpdate);
                    }
                    else
                    {
                        string strUpdate = string.Format(sqlUpdateConsignmentStatus01, sum, dr["XP004"].ToString().Trim().PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                        //sqlCommandList.Add(strUpdate);
                    }

                    ///表名 ZPP_WORKED
                    /// 将数据填写到dataset中然后传入到sap中
                    DataRow drInRfc = dtInRfc.NewRow();
                    drInRfc["LFSNR"] = strKoInPo;
                    drInRfc["WERKS"] = dtKo.Rows[0]["WERKS"].ToString().Trim();
                    drInRfc["ITMNO"] = strRowNumber;
                    drInRfc["BWART"] = strBwart;
                    drInRfc["CDATE"] = Cdate;
                    drInRfc["AUFNR"] = dtKo.Rows[0]["WORK_ORDER"].ToString().Trim();
                    drInRfc["MATNR"] = dtKo.Rows[0]["WORK_ORDER_MATNR"].ToString().Trim();
                    drInRfc["MATNR1"] = strMatnr;
                    if (string.IsNullOrEmpty(lgort))
                    {
                        drInRfc["LGORT"] = null;
                    }
                    else
                    {
                        drInRfc["LGORT"] = lgort;
                    }
                    drInRfc["CHARG"] = null;
                    drInRfc["LMNGA"] = strMenge;
                    drInRfc["XP001"] = dr["XP001"].ToString().Trim();
                    drInRfc["XP002"] = dr["XP002"].ToString().Trim();
                    drInRfc["XP003"] = dr["DL"].ToString().Trim();
                    drInRfc["XP004"] = dr["XP004"].ToString().Trim();
                    decimal xp005 = Convert.ToDecimal(dr["XP005"].ToString().Trim());
                    drInRfc["XP005"] = Math.Round(xp005, 2).ToString();
                    drInRfc["XP006"] = dr["XP006"].ToString().Trim();
                    if (string.IsNullOrEmpty(dr["TOLERANCE"].ToString().Trim()))
                    {
                        drInRfc["XP007"] = "无";
                    }
                    else
                    {
                        drInRfc["XP007"] = dr["TOLERANCE"].ToString().Trim();
                    }
                    drInRfc["XP008"] = dr["XP008"].ToString().Trim();
                    drInRfc["XP009"] = dr["XP009"].ToString().Trim();
                    drInRfc["XP010"] = dr["XP010"].ToString().Trim();
                    drInRfc["XP011"] = dr["XP011"].ToString().Trim();
                    dtInRfc.Rows.Add(drInRfc);
                }
                dsInRfc.Merge(dtInRfc);
                //调用RFC-----------------------------------------------------------------------------------------------------
                DataSet dsOut = new DataSet();
                ////调用RFC
                try
                {
                    AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_REV", dsInRfc, out dsOut);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsIn, ex.Message);
                    LogService.LogError("同步SAP生产入库单异常: " + ex.Message);
                }

                //------------------------------------------------------------------------------------------------------------
                //更新批次
                if (dsOut != null || dsOut.Tables["ZPP_WORKED"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsOut.Tables["ZPP_WORKED"].Rows)
                    {
                        string sqlUpdate = string.Format(sqlUpdatePiCiBuyoutSet,
                                                    dr["CHARG"].ToString().Trim(),
                                                    dr["LFSNR"].ToString().Trim().PreventSQLInjection(),
                                                    dr["ITMNO"].ToString().Trim().PreventSQLInjection()
                                                    );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpdate);
                    }
                }
                //查询信息数据返回到界面

                //foreach (string sql1 in sqlCommandList)
                //{
                //    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql1);
                //}
                ///sql执行end-------------------------------------------------------------------------------------------------------------------

                DataTable dtRkNum = new DataTable();
                dtRkNum.Columns.Add("ZMBLNR");
                DataRow drRk = dtRkNum.NewRow();
                drRk["ZMBLNR"] = ZMBLNR;
                dtRkNum.Rows.Add(drRk);
                dtRkNum.TableName = "RETURN_ZMBLNR";
                retDS.Merge(dtRkNum);

                DataTable dtout01 = dsOut.Tables["ZPP_WORKED"];
                DataTable dtout02 = dsOut.Tables["RETURN"];

                retDS.Merge(dtout01);
                retDS.Merge(dtout02);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                if (dsOut.Tables["RETURN"].Rows[0]["NUMBER"].ToString() == "-1")
                {
                    dbTrans.Rollback();
                    return retDS;
                }
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("CreateRkKoPo Error: " + ex.Message);
            }
            return retDS;
        }

        public static string GetYYMD()
        {
            string strYYMd = null;
            string strDate = DateTime.Now.ToString("yyMMdd");
            string strYear = strDate.Substring(0, 2);
            string strMonth = strDate.Substring(2, 2);
            string strDay = strDate.Substring(4, 2);
            strYYMd = strYear;
            if (strMonth.Equals("10"))
            {
                strYYMd = strYYMd + "A";
            }
            else if (strMonth.Equals("11"))
            {
                strYYMd = strYYMd + "B";
            }
            else if (strMonth.Equals("12"))
            {
                strYYMd = strYYMd + "C";
            }
            else                //等于本身了
            {
                strYYMd = strYYMd + strMonth.Substring(1, 1);
            }
            string strAllDay = strDate.Substring(4, 2);
            strYYMd = strYYMd + strAllDay;
            return strYYMd;
        }
        #endregion
        //获取移动类型
        #region IWarehouseEngine 成员


        public DataSet GetBwart(string p)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT ORDER_NUMBER FROM dbo.POR_WORK_ORDER WHERE PART_NUMBER = '{0}'
                                                    AND ORDER_STATE IN ('TECO','REL')", p);

                DataSet ds = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                DataTable dt = new DataTable();
                dt.Columns.Add("BWART");
                DataRow dr = dt.NewRow();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string BWART = "101";
                    dr["BWART"] = BWART;
                    dt.Rows.Add(dr);
                }
                else
                {
                    string BWART = "531";
                    dr["BWART"] = BWART;
                    dt.Rows.Add(dr);
                }
                dt.TableName = "BWART";
                dsReturn.Merge(dt);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetBwart Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
        //查询功能
        #region IWarehouseEngine 成员


        public DataSet GetRkKoInformation(string rkNum, string Werks, string WorkOrder, string Status, string StartTime, string EndTime)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"	SELECT ZMBLNRKEY,Row_number() OVER (order by CDATE)as ROWNUMBER,
                                                    ZMBLNR,WERKS,AUFNR,RKSTATUS,CDATE,CREATOR,WORK_ORDER_MATNR,MAKTX,MEMO1,LINE
                                                     FROM dbo.WIP_RK_ZXBKO WHERE LVORM = 0 ");
                if (!string.IsNullOrEmpty(rkNum))
                    sqlCommon += @"AND ZMBLNR LIKE '" + rkNum + "%'";
                if (!string.IsNullOrEmpty(Werks))
                    sqlCommon += string.Format(@"AND WERKS = '{0}'", Werks);
                if (!string.IsNullOrEmpty(WorkOrder))
                    sqlCommon += @"AND AUFNR LIKE '" + WorkOrder + "%'";
                if (!string.IsNullOrEmpty(Status))
                {
                    if (Status == "已创建")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = ''");
                    }
                    if (Status == "未审批(W)")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = 'W'");
                    }
                    if (Status == "审批通过(A)")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = 'A'");
                    }
                    if (Status == "拒绝(R)")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = 'R'");
                    }
                    if (Status == "已过账(T)")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = 'T'");
                    }
                    if (Status == "已删除(D)")
                    {
                        sqlCommon += string.Format(@"AND RKSTATUS = 'D'");
                    }
                }
                if (!string.IsNullOrEmpty(StartTime))
                    sqlCommon += @"AND CDATE >= '" + StartTime + "'";
                if (!string.IsNullOrEmpty(EndTime))
                    sqlCommon += @"AND CDATE <= '" + EndTime + "'";
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetRkKoInformation Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet GetKoPoByRkNumber(string rknum)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsKo = new DataSet();
            DataSet dsPo = new DataSet();
            try
            {
                string sqlKo = string.Format(@"SELECT 
                                                    ZMBLNR,WERKS,AUFNR,RKSTATUS,DEPT,
                                                    CDATE,CREATOR,WORK_ORDER_MATNR,MAKTX,MEMO1,LINE
                                                    FROM dbo.WIP_RK_ZXBKO WHERE ZMBLNR ='{0}'", rknum);
                dsKo = db.ExecuteDataSet(CommandType.Text, sqlKo);
                DataTable dtKo = dsKo.Tables[0];
                dtKo.TableName = "WIP_RK_KO";

//                string sqlPo = string.Format(@"SELECT 
//                                                    A.ZMBLNR,ZEILE AS ROWNUMBER,BWART,MATNR,MENGE,CHARG,REMARK,KEYNO AS CONSIGNMENT_KEY,K.AUFNR AS WORKNUMBER,
//                                                    XP001,XP002,XP003 AS DL,XP004,
//                                                    XP005,XP006,XP007 AS TOLERANCE,XP008,
//                                                    XP009,XP010,XP011,A.CREATE_TIME,B.PART_DESC AS MAKTX,LGORT,XP011
//                                                    FROM dbo.WIP_RK_ZXBPO A
//                                                    LEFT JOIN dbo.POR_PART B
//                                                    ON A.MATNR = B.PART_ID AND B.PART_STATUS = 1
//                                                    LEFT JOIN dbo.WIP_RK_ZXBKO K ON K.ZMBLNR = A.ZMBLNR AND K.LVORM = 0
//                                                    WHERE A.ZMBLNR ='{0}' AND ITEMSTATUS = 0", rknum);
                string sqlPo = string.Format(@"SELECT 
                                                    A.ZMBLNR,ZEILE AS ROWNUMBER,BWART,MATNR,MENGE,CHARG,A.REMARK,KEYNO AS CONSIGNMENT_KEY,K.AUFNR AS WORKNUMBER,
                                                    XP001,XP002,XP003 AS DL,XP004,
                                                    XP005,XP006,XP007 AS TOLERANCE,XP008,
                                                    XP009,XP010,XP011,A.CREATE_TIME,C.PART_DESC AS MAKTX,LGORT,XP011
                                                    FROM dbo.WIP_RK_ZXBPO A
                                                    LEFT JOIN dbo.WIP_RK_ZXBKO K ON K.ZMBLNR = A.ZMBLNR AND K.LVORM = 0
                                                    LEFT JOIN dbo.POR_PART C ON A.MATNR = C.PART_ID 
                                                    WHERE A.ZMBLNR ='{0}' AND ITEMSTATUS = 0", rknum);
                
                dsPo = db.ExecuteDataSet(CommandType.Text, sqlPo);

                DataTable dtPo = dsPo.Tables[0];
                dtPo.TableName = "WIP_RK_PO";

                dsReturn.Merge(dtKo);
                dsReturn.Merge(dtPo);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetKoPoByRkNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
        //删除入库单修改状态
        #region IWarehouseEngine 成员


        public DataSet RfcToDeleteRk(string rkNum, string werks)
        {
            IList<string> sqlCommandList = new List<string>();
            DataSet dsReturn = new DataSet();
            DataSet dsOut = new DataSet();
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ////调用RFC
                try
                {
                    DataSet dsInRfc = new DataSet();
                    dsInRfc.ExtendedProperties.Add("I_WERKS", werks);
                    dsInRfc.ExtendedProperties.Add("I_ZMBLNR", rkNum);

                    AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_STS", dsInRfc, out dsOut);

                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                    LogService.LogError("获取入库单状态异常: " + ex.Message);
                }
                string okOrNot = dsOut.ExtendedProperties["E_SUBRC"].ToString();
                string e_MSG = dsOut.ExtendedProperties["E_MSG"].ToString();
                string e_STAT = dsOut.ExtendedProperties["E_STAT"].ToString();

                if (e_STAT == "D")
                {
                    string sqlCommand2 = string.Format(@"UPDATE dbo.WIP_RK_ZXBKO SET RKSTATUS = '{0}',LVORM = 1 WHERE ZMBLNR = '{1}'", e_STAT, rkNum);
                    sqlCommandList.Add(sqlCommand2);
                    string sqlCommand1 = string.Format(@"UPDATE dbo.WIP_RK_ZXBPO SET ITEMSTATUS = 1 WHERE ZMBLNR ='{0}'", rkNum);
                    sqlCommandList.Add(sqlCommand1);
                    //                    string sqlCommand2 = string.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET CS_DATA_GROUP = 2 
                    //                                                            WHERE PALLET_NO IN 
                    //                                                            (SELECT XP004 FROM dbo.WIP_RK_ZXBPO WHERE ZMBLNR ='{0}' AND ITEMSTATUS = 0)",
                    //                                                             rkNum);
                    //                    sqlCommandList.Add(sqlCommand2);   //状态不卡控
                    string sqlGetMENGE = string.Format(@" SELECT ZMBLNR,XP004,MENGE FROM dbo.WIP_RK_ZXBPO A
                                                             INNER JOIN dbo.WIP_CONSIGNMENT B ON A.XP004 = B.PALLET_NO AND ISFLAG = 1 WHERE ITEMSTATUS = 0
                                                             AND ZMBLNR = '{0}'", rkNum);
                    DataSet dsGetMENGE = db.ExecuteDataSet(CommandType.Text, sqlGetMENGE);
                    foreach (DataRow dr in dsGetMENGE.Tables[0].Rows)
                    {
                        //                        string sqlUpdateCsdatagroup = string.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET CS_DATA_GROUP = '2',INWH_QTY =INWH_QTY - {0} 
                        //                                            WHERE PALLET_NO ='{1}' AND ISFLAG = 1
                        //                                            ", Convert.ToInt32(dr["MENGE"]), dr["XP004"]);  //状态不卡控
                        string sqlUpdateCsdatagroup = string.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY =INWH_QTY - {0} 
                                            WHERE PALLET_NO ='{1}' AND ISFLAG = 1
                                            ", Convert.ToInt32(dr["MENGE"]), dr["XP004"]);
                        sqlCommandList.Add(sqlUpdateCsdatagroup);
                    }
                }
                else
                {
                    string sqlCommand = string.Format(@"UPDATE dbo.WIP_RK_ZXBKO SET RKSTATUS = '{0}' WHERE ZMBLNR = '{1}'", e_STAT, rkNum);
                    sqlCommandList.Add(sqlCommand);
                }
                foreach (string sql1 in sqlCommandList)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql1);
                }
                DataTable dtRk = new DataTable();
                dtRk.Columns.Add("E_SUBRC");
                dtRk.Columns.Add("E_MSG");
                dtRk.Columns.Add("E_STAT");
                DataRow drRk = dtRk.NewRow();
                drRk["E_SUBRC"] = okOrNot;
                drRk["E_MSG"] = e_MSG;
                drRk["E_STAT"] = e_STAT;
                dtRk.Rows.Add(drRk);
                dtRk.TableName = "STATUS";
                dsReturn.Merge(dtRk);
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("RfcToDeleteRk Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        //查询绑定数据
        #region IWarehouseEngine 成员


        public DataSet GetKoPoByRkNumberForPrint(string koRkNumber)
        {
            DataSet dsReturn = new DataSet();
            DataSet dsKo = new DataSet();
            DataSet dsPo = new DataSet();
            try
            {
                string sqlKo = string.Format(@"SELECT 
                                                        ZMBLNR,AUFNR,DEPT,CDATE
                                                        FROM dbo.WIP_RK_ZXBKO WHERE ZMBLNR ='{0}'", koRkNumber);
                dsKo = db.ExecuteDataSet(CommandType.Text, sqlKo);
                DataTable dtKo = dsKo.Tables[0];
                dtKo.TableName = "WIP_RK_KO";

                string sqlPo = string.Format(@"SELECT 
                                                    A.ZMBLNR,A.MATNR,F.PART_DESC AS PART_DESC,A.CHARG,A.XP006 + ' W' AS XP006,
                                                    A.XP001,A.XP002,A.XP003,A.XP004,A.XP009,A.XP011,
                                                    CONVERT(VARCHAR(20),Convert(decimal(30,3),XP005)) + ' W' AS XP005,E.JUNCTION_BOX,A.XP007,A.MENGE
                                                    FROM dbo.WIP_RK_ZXBPO A
                                                    INNER JOIN dbo.WIP_RK_ZXBKO B ON A.ZMBLNR = B.ZMBLNR AND B.LVORM = 0
                                                    INNER JOIN dbo.POR_WORK_ORDER C ON B.AUFNR = C.ORDER_NUMBER
                                                    INNER JOIN dbo.WIP_CONSIGNMENT D ON A.XP004 = D.PALLET_NO AND D.ISFLAG=1
                                                    LEFT JOIN dbo.POR_WO_PRD E ON
                                                            E.WORK_ORDER_KEY = C.WORK_ORDER_KEY  AND 
                                                            D.SAP_NO = E.PART_NUMBER 
                                                            AND E.IS_USED='Y' 
                                                    LEFT JOIN POR_PART F ON D.SAP_NO = F.PART_ID  AND F.PART_STATUS = 1
                                                    WHERE  A.ZMBLNR = '{0}' AND ITEMSTATUS = 0
                                                    ORDER BY XP004 ASC", koRkNumber);
                dsPo = db.ExecuteDataSet(CommandType.Text, sqlPo);
                DataTable dtPo = dsPo.Tables[0];
                dtPo.TableName = "WIP_RK_PO";

                dsReturn.Merge(dtKo);
                dsReturn.Merge(dsPo);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetKoPoByRkNumberForPrint Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet InsertPrintInf(string number, string name)
        {
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlInsert = @" INSERT INTO dbo.WIP_RK_PRINT
                                    (PRINTKEY,RK_KO_KEY,PRINTER,PRINTTIME) 
                                    VALUES
                                    ('{0}','{1}','{2}',GETDATE())";
                string key = UtilHelper.GenerateNewKey(0);
                string sqlDetail = string.Format(sqlInsert,
                                                   key,
                                                   number,
                                                   name
                                                   );
                db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);
                dbTrans.Commit();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("InsertPrintInf Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion


        #region IWarehouseEngine 成员


        public DataSet EditRkKoPo(DataSet dsIn)
        {
            IList<string> sqlCommandList = new List<string>();
            IList<string> sqlCommandList1 = new List<string>();
            DataSet retDS = new DataSet();
            DataSet dsInRfc = new DataSet();
            DataTable dtInRfc = new DataTable();
            dtInRfc.Columns.Add("LFSNR");//入库单号
            dtInRfc.Columns.Add("WERKS");//工厂号
            dtInRfc.Columns.Add("ITMNO");//序号 
            dtInRfc.Columns.Add("BWART");//物料移动类型
            dtInRfc.Columns.Add("CDATE");//创建时间
            dtInRfc.Columns.Add("AUFNR");//工单号
            dtInRfc.Columns.Add("MATNR");//工单料号
            dtInRfc.Columns.Add("MATNR1");//实际入库产品编码
            dtInRfc.Columns.Add("LGORT");//入库仓
            dtInRfc.Columns.Add("CHARG");//批次
            dtInRfc.Columns.Add("LMNGA");//数量
            dtInRfc.Columns.Add("XP001");//质量等级
            dtInRfc.Columns.Add("XP002");//保税手册号
            dtInRfc.Columns.Add("XP003");//电流分档
            dtInRfc.Columns.Add("XP004");//托盘号
            dtInRfc.Columns.Add("XP005");//实际功率
            dtInRfc.Columns.Add("XP006");//标称功率
            dtInRfc.Columns.Add("XP007");//分档方式
            dtInRfc.Columns.Add("XP008");//红外等级下限 
            dtInRfc.Columns.Add("XP009");//衰减因子 
            dtInRfc.Columns.Add("XP010");//FF下限% 
            dtInRfc.Columns.Add("XP011");//晶硅功率范围    
            dtInRfc.TableName = "ZPP_WORKED";
            DbConnection dbCon = db.CreateConnection();
            dbCon.Open();
            DbTransaction dbTrans = dbCon.BeginTransaction();
            try
            {
                ///sql定义start--------------------------------------------------------------------------------------------------------------
                string sqlPo = @" INSERT INTO dbo.WIP_RK_ZXBPO
                                    (ZMBLNRDETILKEY,ZMBLNR,ZEILE,ITEMSTATUS,
                                    BWART,MATNR,MENGE,KEYNO,REMARK,
                                    XP001,XP002,XP003,XP004,
                                    XP005,XP006,XP007,XP008,
                                    XP009,XP010,XP011,
                                    CREATE_TIME,LGORT)
                                    VALUES
                                    ('{0}','{1}',{2},0,
                                    '{3}','{4}',{5},'{6}',
                                    '{7}','{8}','{9}','{10}',
                                    '{11}', '{12}','{13}','{14}',
                                    '{15}', '{16}','{17}','{18}',
                                    GETDATE(),'{19}')";
                ///传入参数 入库单号和行号
                ///修改参数  批次号
                /// 根据RFC调用传回的信息更新入库明细表批次号
                string sqlUpdatePiCiBuyoutSet = @"UPDATE dbo.WIP_RK_ZXBPO SET CHARG = '{0}' WHERE ZMBLNR ='{1}' AND ZEILE = '{2}'";
                ///传入参数托号
                ///修改状态为3 已入库
                /// 更新包装表中托状态
                string sqlUpdateConsignmentStatus = @"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY = '{0}' WHERE PALLET_NO = '{1}' AND ISFLAG = 1";  //状态不卡控
                string sqlUpdateConsignmentStatus01 = @"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY = '{0}' WHERE PALLET_NO = '{1}' AND ISFLAG = 1";
                ///sql定义end---------------------------------------------------------------------------------------------------------------------------

                ///数据处理start-------------------------------------------------------------------------------------------------------------------
                DataTable dtKo = dsIn.Tables["WIP_RK_KO"];
                DataTable dtPo = dsIn.Tables["WIP_RK_PO"];
                string ZMBLNR = dtKo.Rows[0]["ZMBLNR"].ToString().Trim();
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                string year = currentTime.Year.ToString("00");
                string Month = currentTime.Month.ToString("00");
                string Day = currentTime.Day.ToString("00");
                string Hour = currentTime.Hour.ToString("00");
                string Minute = currentTime.Minute.ToString("00");
                string Second = currentTime.Minute.ToString("00");
                string Cdate = year + Month + Day + Hour + Minute + Second;

                ///数据处理end---------------------------------------------------------------------------------------------------------------------------


                ///sql执行start-------------------------------------------------------------------------------------------------------------------
                string sqlGetMENGE = string.Format(@" SELECT ZMBLNR,XP004,MENGE FROM dbo.WIP_RK_ZXBPO A
                                                             INNER JOIN dbo.WIP_CONSIGNMENT B ON A.XP004 = B.PALLET_NO AND ISFLAG = 1 WHERE ITEMSTATUS = 0
                                                             AND ZMBLNR = '{0}'", ZMBLNR);
                DataSet dsGetMENGE = db.ExecuteDataSet(dbTrans, CommandType.Text, sqlGetMENGE);
                foreach (DataRow dr in dsGetMENGE.Tables[0].Rows)
                {
                    //                    string sqlUpdateCsdatagroup = string.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET CS_DATA_GROUP = '2',INWH_QTY =INWH_QTY - {0} 
                    //                                            WHERE PALLET_NO ='{1}' AND ISFLAG = 1    //状态卡控
                    string sqlUpdateCsdatagroup = string.Format(@"UPDATE dbo.WIP_CONSIGNMENT SET INWH_QTY =INWH_QTY - {0} 
                                            WHERE PALLET_NO ='{1}' AND ISFLAG = 1
                                            ", Convert.ToInt32(dr["MENGE"]), dr["XP004"]);
                    sqlCommandList1.Add(sqlUpdateCsdatagroup);
                }

                string sqlUpItemstatus = string.Format(@"UPDATE dbo.WIP_RK_ZXBPO SET ITEMSTATUS = 1 WHERE ZMBLNR = '{0}'", ZMBLNR);
                sqlCommandList1.Add(sqlUpItemstatus);
                foreach (string sql2 in sqlCommandList1)
                {
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql2);
                }

                //生成插入入库明细记录的SQL
                foreach (DataRow dr in dtPo.Rows)
                {
                    string keyPo = UtilHelper.GenerateNewKey(0);                       //入库单明细主键
                    string strKoInPo = ZMBLNR;                                         //入库单号
                    int strRowNumber = Convert.ToInt32(dr["ROWNUMBER"].ToString().Trim().PreventSQLInjection());     //行号
                    string strBwart = dr["BWART"].ToString().Trim().PreventSQLInjection();                           //移动类型
                    string strMatnr = dr["MATNR"].ToString().Trim().PreventSQLInjection();                           //物料号
                    decimal strMenge = Convert.ToDecimal(dr["MENGE"].ToString().Trim().PreventSQLInjection());        //数量       
                    string keyPalletN0 = dr["CONSIGNMENT_KEY"].ToString().Trim().PreventSQLInjection();              //包装表主键
                    string strRemark = dr["REMARK"].ToString().Trim().PreventSQLInjection();                         //备注
                    string lgort = dr["LGORT"].ToString().Trim().PreventSQLInjection();                         //库位
                    //生成向入库明细表插入记录的SQL语句
                    string sqlDetail = string.Format(sqlPo,
                                                    keyPo,             //主键
                                                    strKoInPo,         //入库单号
                                                    strRowNumber,      //行号
                                                    strBwart,          //移动类型
                                                    strMatnr,          //物料号
                                                    strMenge,          //数量
                                                    keyPalletN0,       //托号
                                                    strRemark,         //备注
                                                    dr["XP001"].ToString().Trim().PreventSQLInjection(),    //质量等级
                                                    dr["XP002"].ToString().Trim().PreventSQLInjection(),    //保税手册号
                                                    dr["DL"].ToString().Trim().PreventSQLInjection(),       //电流分档
                                                    dr["XP004"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP005"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP006"].ToString().Trim().PreventSQLInjection(),
                                                    dr["TOLERANCE"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP008"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP009"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP010"].ToString().Trim().PreventSQLInjection(),
                                                    dr["XP011"].ToString().Trim().PreventSQLInjection(),
                                                    lgort
                                                    );
                    db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlDetail);

                    //修改包装表状态为 判定入库数量加上已经入库数量和托数量对比是否一致
                    //一致则修改入库状态和数量 不一致则不修改入库状态 只改入库数量
                    string strGetQTY = string.Format(@"SELECT LOT_NUMBER_QTY,INWH_QTY FROM dbo.WIP_CONSIGNMENT WHERE PALLET_NO = '{0}' 
                                                            AND ISFLAG = 1 ",
                                                            dr["XP004"].ToString().Trim().PreventSQLInjection());
                    DataSet dsGetQTY = db.ExecuteDataSet(dbTrans, CommandType.Text, strGetQTY);
                    int sum = Convert.ToInt32(dsGetQTY.Tables[0].Rows[0]["INWH_QTY"].ToString().Trim()) + Convert.ToInt32(strMenge);
                    int lotNumberQTY = Convert.ToInt32(dsGetQTY.Tables[0].Rows[0]["LOT_NUMBER_QTY"].ToString().Trim());
                    if (sum == lotNumberQTY)
                    {
                        string strUpdate = string.Format(sqlUpdateConsignmentStatus, sum, dr["XP004"].ToString().Trim().PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                        //sqlCommandList.Add(strUpdate);
                    }
                    else
                    {
                        string strUpdate = string.Format(sqlUpdateConsignmentStatus01, sum, dr["XP004"].ToString().Trim().PreventSQLInjection());
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, strUpdate);
                        //sqlCommandList.Add(strUpdate);
                    }

                    ///表名 ZPP_WORKED
                    /// 将数据填写到dataset中然后传入到sap中
                    DataRow drInRfc = dtInRfc.NewRow();
                    drInRfc["LFSNR"] = strKoInPo;
                    drInRfc["WERKS"] = dtKo.Rows[0]["WERKS"].ToString().Trim();
                    drInRfc["ITMNO"] = strRowNumber;
                    drInRfc["BWART"] = strBwart;
                    drInRfc["CDATE"] = Cdate;
                    drInRfc["AUFNR"] = dtKo.Rows[0]["WORK_ORDER"].ToString().Trim();
                    drInRfc["MATNR"] = dtKo.Rows[0]["WORK_ORDER_MATNR"].ToString().Trim();
                    drInRfc["MATNR1"] = strMatnr;
                    if (string.IsNullOrEmpty(lgort))
                    {
                        drInRfc["LGORT"] = null;
                    }
                    else
                    {
                        drInRfc["LGORT"] = lgort;
                    }
                    if (string.IsNullOrEmpty(dr["CHARG"].ToString().Trim()))
                    {
                        drInRfc["CHARG"] = null;
                    }
                    else
                    {
                        drInRfc["CHARG"] = dr["CHARG"].ToString().Trim();
                    }
                    drInRfc["LMNGA"] = strMenge;
                    drInRfc["XP001"] = dr["XP001"].ToString().Trim();
                    drInRfc["XP002"] = dr["XP002"].ToString().Trim();
                    drInRfc["XP003"] = dr["DL"].ToString().Trim();
                    drInRfc["XP004"] = dr["XP004"].ToString().Trim();
                    decimal xp005 = Convert.ToDecimal(dr["XP005"].ToString().Trim());
                    drInRfc["XP005"] = Math.Round(xp005, 2).ToString();
                    drInRfc["XP006"] = dr["XP006"].ToString().Trim();
                    if (string.IsNullOrEmpty(dr["TOLERANCE"].ToString().Trim()))
                    {
                        drInRfc["XP007"] = "无";
                    }
                    else
                    {
                        drInRfc["XP007"] = dr["TOLERANCE"].ToString().Trim();
                    }
                    drInRfc["XP008"] = dr["XP008"].ToString().Trim();
                    drInRfc["XP009"] = dr["XP009"].ToString().Trim();
                    drInRfc["XP010"] = dr["XP010"].ToString().Trim();
                    drInRfc["XP011"] = dr["XP011"].ToString().Trim();
                    dtInRfc.Rows.Add(drInRfc);
                }
                dsInRfc.Merge(dtInRfc);
                //调用RFC-----------------------------------------------------------------------------------------------------
                DataSet dsOut = new DataSet();
                ////调用RFC
                try
                {
                    AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_REV", dsInRfc, out dsOut);
                }
                catch (Exception ex)
                {
                    FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsIn, ex.Message);
                    LogService.LogError("同步SAP生产入库单异常: " + ex.Message);
                }

                //------------------------------------------------------------------------------------------------------------
                //更新批次
                if (dsOut != null || dsOut.Tables["ZPP_WORKED"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsOut.Tables["ZPP_WORKED"].Rows)
                    {
                        string sqlUpdate = string.Format(sqlUpdatePiCiBuyoutSet,
                                                    dr["CHARG"].ToString().Trim(),
                                                    dr["LFSNR"].ToString().Trim().PreventSQLInjection(),
                                                    dr["ITMNO"].ToString().Trim().PreventSQLInjection()
                                                    );
                        db.ExecuteNonQuery(dbTrans, CommandType.Text, sqlUpdate);
                    }
                }
                //查询信息数据返回到界面

                //foreach (string sql1 in sqlCommandList)
                //{
                //    db.ExecuteNonQuery(dbTrans, CommandType.Text, sql1);
                //}
                ///sql执行end-------------------------------------------------------------------------------------------------------------------

                DataTable dtRkNum = new DataTable();
                dtRkNum.Columns.Add("ZMBLNR");
                DataRow drRk = dtRkNum.NewRow();
                drRk["ZMBLNR"] = ZMBLNR;
                dtRkNum.Rows.Add(drRk);
                dtRkNum.TableName = "RETURN_ZMBLNR";
                retDS.Merge(dtRkNum);

                DataTable dtout01 = dsOut.Tables["ZPP_WORKED"];
                DataTable dtout02 = dsOut.Tables["RETURN"];

                retDS.Merge(dtout01);
                retDS.Merge(dtout02);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, string.Empty);
                if (dsOut.Tables["RETURN"].Rows[0]["NUMBER"].ToString() == "-1")
                {
                    dbTrans.Rollback();
                    return retDS;
                }
                dbTrans.Commit();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(retDS, ex.Message);
                LogService.LogError("EditRkKoPo Error: " + ex.Message);
            }
            return retDS;
        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet GetPo(string rkNum, string pallet_no)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon01 = string.Format(@"SELECT * FROM dbo.WIP_RK_ZXBPO WHERE ZMBLNR = '{0}' 
                                                        AND XP004 = '{1}' AND ITEMSTATUS = 0", rkNum, pallet_no);
                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon01);

                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPo Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWarehouseEngine 成员

        //获取全部工单
        public DataSet GetWorkNumber()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT DISTINCT ORDER_NUMBER FROM dbo.POR_WORK_ORDER
                                                        WHERE  ORDER_STATE IN ('TECO','REL')");

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetWorkNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet GetPorWorkOrderInfByWorkNo(string workNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT A.ORDER_NUMBER,A.PART_NUMBER,A.LINE_NAME,A.DESCRIPTIONS AS PART_DESC
                                                        FROM 
                                                        dbo.POR_WORK_ORDER A
                                                        LEFT JOIN POR_PART B ON B.PART_ID = A.PART_NUMBER AND B.PART_STATUS = 1
                                                        WHERE A.ORDER_STATE IN ('TECO','REL') AND A.ORDER_NUMBER = '{0}'", workNumber);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPorWorkOrderInfByWorkNo Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region IWarehouseEngine 成员


        public DataSet GetPalletWorkorder(string paNo, string work_order)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                string sqlCommon = string.Format(@"SELECT DISTINCT WORK_ORDER_NO 
                                                        FROM 
                                                        dbo.POR_LOT WHERE PALLET_NO = '{0}' 
                                                        AND WORK_ORDER_NO = '{1}'  AND STATUS < 2", paNo, work_order);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPalletWorkorder Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
        public string RfcSapRkStatus(string ZMBLNR, string werks)
        {
            DataSet dsOut = new DataSet();
            string zmb = ZMBLNR.Substring(0, 8);
            ////调用RFC
            try
            {

                DataSet dsInRfc = new DataSet();
                dsInRfc.ExtendedProperties.Add("I_WERKS", werks);
                dsInRfc.ExtendedProperties.Add("I_ZMBLNR", ZMBLNR);

                AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_STS", dsInRfc, out dsOut);

            }
            catch (Exception ex)
            {

            }
            string okOrNot = dsOut.ExtendedProperties["E_SUBRC"].ToString();  //0 表示入库单存在  -1 表示入库单不存在
            string e_MSG = dsOut.ExtendedProperties["E_MSG"].ToString();      //订单不存在提示   订单不存在  HZ01  J..单号
            string e_STAT = dsOut.ExtendedProperties["E_STAT"].ToString();    //存在有状态 不存在为空
            if (okOrNot == "0")
            {
                ZMBLNR = ZMBLNR.Substring(8, 4);
                int maxlotnumber = int.Parse(ZMBLNR);
                if (maxlotnumber < 9999)
                    ZMBLNR = (maxlotnumber + 1).ToString("0000");
                return RfcSapRkStatus(zmb + ZMBLNR, werks);
            }
            return ZMBLNR;
        }

        #region IWarehouseEngine 成员


        public string GetBonded(string bonded)
        {
            DataSet dsOut = new DataSet();
            ////调用RFC
            try
            {
                DataSet dsInRfc = new DataSet();
                dsInRfc.ExtendedProperties.Add("I_ATNAM", "SAP_KRR_USERDEFINE1");
                dsInRfc.ExtendedProperties.Add("I_ATWRT", bonded);

                AllCommonFunctions.SAPRemoteFunctionCall("Z_RFC_CHARC_GET_TEXT", dsInRfc, out dsOut);

            }
            catch (Exception ex)
            {

            }
            string bon = dsOut.ExtendedProperties["E_TEXT"].ToString();  //0 表示入库单存在  -1 表示入库单不存在
            return bon;
        }

        #endregion
    }
}

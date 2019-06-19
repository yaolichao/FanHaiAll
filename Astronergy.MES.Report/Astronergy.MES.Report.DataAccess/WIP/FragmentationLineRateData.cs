using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Astronergy.MES.Report.DataAccess
{
    public sealed class FragmentationLineRateData
    {
        private static Database db = DatabaseFactory.CreateDatabase();

        public static DataSet GetFragmentationDate(string sFactoryroomName, string sStartDate, string sEndDate, string sType, string sShiftName, string sDetailItem)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            string sShiftPoint = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sql = @"SELECT ISNULL(SUM(B.PATCH_QUANTITY),0) AS 'ERROR_QTY'
                    FROM WIP_PATCH B
                    INNER JOIN WIP_TRANSACTION A ON B.PATCHED_TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE A.UNDO_FLAG=0";
            if (sType == "ALL")
            {
                sql += " AND B.REASON_CODE_CLASS IN ('19','21')";
            }
            else
            {
                sql += " AND B.REASON_CODE_CLASS='" + sType + "'";
            }

            if (sDetailItem != "")
            {
                sql += " AND B.REASON_CODE_NAME LIKE '%" + sDetailItem + "%'";
            }

            if (sFactoryroomName != "ALL")
            {
                sql += " AND C.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }

            if (sShiftName == "A")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sShiftPoint + "')";
            }
            else if (sShiftName == "B")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sShiftPoint + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            else
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            sql += " AND A.ACTIVITY='PATCHED'";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetMoveInOutQty(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sMoveType, string sStepName)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            string sDay = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd");
            DataSet dsDay = GetDateTime(sDay, sShiftName, sFactoryroomName);
            if (dsDay.Tables[0].Rows.Count > 0)
            {
                sStartDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["START_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                sEndDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["END_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            sql = @"SELECT ISNULL(SUM(A.QUANTITY_IN),0) AS 'MOV_QTY'
                    FROM WIP_TRANSACTION A
                    INNER JOIN WIP_LOT B ON B.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sFactoryroomName != "ALL")
            {
                sql += " AND B.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
            sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            sql += " AND A.ACTIVITY='" + sMoveType + "'";
            sql += " AND A.STEP_NAME='" + sStepName + "'";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetShifName()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT T.ITEM_ORDER";
            sql += ",MAX( case T.ATTRIBUTE_NAME when 'CODE' THEN T.ATTRIBUTE_VALUE ELSE ''  end ) as 'SHIFT_NAME'";
            sql += " FROM CRM_ATTRIBUTE  T,BASE_ATTRIBUTE  T1,BASE_ATTRIBUTE_CATEGORY T2";
            sql += " WHERE T.ATTRIBUTE_KEY = T1.ATTRIBUTE_KEY AND T1.CATEGORY_KEY = T2.CATEGORY_KEY";
            sql += " AND UPPER(T2.CATEGORY_NAME) = 'Basic_Shift'";
            sql += " GROUP BY T.ITEM_ORDER";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetSupplier(string sCode, string sName, String sNickname)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT CODE,NAME";
            sql += " FROM BASE_SUPPLIER";
            sql += " WHERE 1=1";
            if (sCode != "")
            {
                sql += " AND CODE='" + sCode + "'";
            }
            if (sName != "")
            {
                sql += " AND NAME='" + sName + "'";
            }
            if (sNickname != "")
            {
                sql += " AND NICKNAME='" + sNickname + "'";
            }
            sql += " ORDER BY CODE ASC";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFragmentationBySupplier(string sFactoryroomName, string sStartDate, string sEndDate, string sType, string sShiftName, string sSupplier)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            string sShiftPoint = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sql = @"SELECT ISNULL(SUM(B.PATCH_QUANTITY),0) AS 'ERROR_QTY'
                    FROM WIP_PATCH B
                    INNER JOIN WIP_TRANSACTION A ON B.PATCHED_TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sType == "ALL")
            {
                sql += " AND B.REASON_CODE_CLASS IN ('19','21')";
            }
            else
            {
                sql += " AND B.REASON_CODE_CLASS='" + sType + "'";
            }
            if (sSupplier != "")
            {
                sql += " AND C.SUPPLIER_NAME='" + sSupplier + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND C.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.ACTIVITY='PATCHED'";

            if (sShiftName == "A")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sShiftPoint + "')";
            }
            else if (sShiftName == "B")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sShiftPoint + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            else
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetMoveInOutQtyBySupplier(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sMoveType, string sStepName, string sSupplier)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            string sDay = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd");
            DataSet dsDay = GetDateTime(sDay, sShiftName, sFactoryroomName);
            if (dsDay.Tables[0].Rows.Count > 0)
            {
                sStartDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["START_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                sEndDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["END_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            sql = @"SELECT ISNULL(SUM(A.QUANTITY_IN),0) AS 'MOV_QTY' 
                    FROM WIP_TRANSACTION A
                    INNER JOIN WIP_LOT B ON B.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sSupplier != "")
            {
                sql += " AND B.SUPPLIER_NAME='" + sSupplier + "'";
            }
            if (sMoveType != "")
            {
                sql += " AND A.ACTIVITY='" + sMoveType + "'";
            }
            if (sStepName != "")
            {
                sql += " AND A.STEP_NAME='" + sStepName + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND B.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
            sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetStepName()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT ROUTE_OPERATION_NAME";
            sql += " FROM POR_ROUTE_OPERATION_VER";
            sql += " ORDER BY SORT_SEQ ASC";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFragmentationByStep(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sStepName)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            string sShiftPoint = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sql = @"SELECT ISNULL(SUM(B.PATCH_QUANTITY),0) AS 'ERROR_QTY'
                    FROM WIP_PATCH B
                    INNER JOIN WIP_TRANSACTION A ON B.PATCHED_TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sStepName != "")
            {
                sql += " AND B.STEP_NAME='" + sStepName + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND C.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.ACTIVITY='PATCHED'";
            if (sShiftName == "A")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sShiftPoint + "')";
            }
            else if (sShiftName == "B")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sShiftPoint + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            else
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetMoveInOutQtyByStep(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sMoveType, string sStepName)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            string sDay = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd");
            DataSet dsDay = GetDateTime(sDay, sShiftName, sFactoryroomName);
            if (dsDay.Tables[0].Rows.Count > 0)
            {
                sStartDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["START_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                sEndDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["END_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            sql = @"SELECT ISNULL(SUM(A.QUANTITY_OUT),0) AS 'MOV_QTY' 
                    FROM WIP_TRANSACTION A
                    INNER JOIN WIP_LOT B ON B.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sMoveType != "")
            {
                sql += " AND A.ACTIVITY='" + sMoveType + "'";
            }
            if (sStepName != "")
            {
                sql += " AND A.STEP_NAME='" + sStepName + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND B.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
            sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFragmentationByFlow(string sFactoryroomName, string sStartDate, string sEndDate, string sType, string sShiftName, string sFlow)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            string sShiftPoint = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sql = @"SELECT ISNULL(SUM(B.PATCH_QUANTITY),0) AS 'ERROR_QTY'
                    FROM WIP_PATCH B
                    INNER JOIN WIP_TRANSACTION A ON B.PATCHED_TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sType == "ALL")
            {
                sql += " AND B.REASON_CODE_CLASS IN ('19','21')";
            }
            else
            {
                sql += " AND B.REASON_CODE_CLASS='" + sType + "'";
            }
            if (sFlow != "")
            {
                sql += " AND SUBSTRING(C.SI_LOT,CHARINDEX('-',C.SI_LOT)+2,1)='" + sFlow + "'";
            }
            sql += " AND C.SUPPLIER_NAME='正泰'";
            if (sFactoryroomName != "ALL")
            {
                sql += " AND C.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.ACTIVITY='PATCHED'";
            if (sShiftName == "A")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sShiftPoint + "')";
            }
            else if (sShiftName == "B")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sShiftPoint + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            else
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetMoveInOutQtyByFlow(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sMoveType, string sStepName, string sFlow)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            string sDay = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd");
            DataSet dsDay = GetDateTime(sDay, sShiftName, sFactoryroomName);
            if (dsDay.Tables[0].Rows.Count > 0)
            {
                sStartDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["START_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                sEndDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["END_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            sql = @"SELECT ISNULL(SUM(A.QUANTITY_IN),0) AS 'MOV_QTY'
                    FROM WIP_TRANSACTION A
                    INNER JOIN WIP_LOT B ON B.TRANSACTION_KEY=A.TRANSACTION_KEY
                    WHERE  A.UNDO_FLAG=0";
            if (sFlow != "")
            {
                sql += " AND SUBSTRING(B.SI_LOT,CHARINDEX('-',B.SI_LOT)+2,1)='" + sFlow + "'";
            }
            sql += " AND B.SUPPLIER_NAME='正泰'";
            //if (sShiftName != "ALL")
            //{
            //    sql += " AND A.SHIFT_NAME='" + sShiftName + "'";
            //}
            if (sMoveType != "")
            {
                sql += " AND A.ACTIVITY='" + sMoveType + "'";
            }
            if (sStepName != "")
            {
                sql += " AND A.STEP_NAME='" + sStepName + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND  B.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
            sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetProMode()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT DISTINCT PROMODEL_NAME";
            sql += " FROM BASE_PRODUCTMODEL";
            sql += " WHERE ISFLAG='1'";
            sql += " ORDER BY PROMODEL_NAME ASC";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFragmentationByMode(string sFactoryroomName, string sStartDate, string sEndDate, string sType, string sShiftName, string sMode)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();

            string sShiftPoint = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sql = @"SELECT ISNULL(SUM(B.PATCH_QUANTITY),0) AS 'ERROR_QTY'
                    FROM WIP_PATCH B
                    INNER JOIN WIP_TRANSACTION A ON B.PATCHED_TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN POR_PRODUCT D ON D.PRODUCT_CODE=C.PRO_ID
                    WHERE  A.UNDO_FLAG=0";
            if (sType == "ALL")
            {
                sql += " AND B.REASON_CODE_CLASS IN ('19','21')";
            }
            else
            {
                sql += " AND B.REASON_CODE_CLASS='" + sType + "'";
            }
            if (sMode != "")
            {
                sql += " AND D.PROMODEL_NAME='" + sMode + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND C.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.ACTIVITY='PATCHED'";
            if (sShiftName == "A")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sShiftPoint + "')";
            }
            else if (sShiftName == "B")
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sShiftPoint + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }
            else
            {
                sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
                sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";
            }

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetMoveInOutQtyByMode(string sFactoryroomName, string sStartDate, string sEndDate, string sShiftName, string sMoveType, string sStepName, string sMode)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();


            string sDay = DateTime.Parse(sStartDate).ToString("yyyy-MM-dd");
            DataSet dsDay = GetDateTime(sDay, sShiftName, sFactoryroomName);
            if (dsDay.Tables[0].Rows.Count > 0)
            {
                sStartDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["START_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                sEndDate = DateTime.Parse(dsDay.Tables[0].Rows[0]["END_TIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }

            sql = @"SELECT ISNULL(SUM(A.QUANTITY_IN),0) AS 'MOV_QTY'
                    FROM WIP_TRANSACTION A
                    INNER JOIN WIP_LOT B ON B.TRANSACTION_KEY=A.TRANSACTION_KEY
                    INNER JOIN POR_PRODUCT C ON C.PRODUCT_CODE=B.PRO_ID
                    WHERE A.UNDO_FLAG=0";
            if (sMode != "")
            {
                sql += " AND C.PROMODEL_NAME='" + sMode + "'";
            }
            //if (sShiftName != "ALL")
            //{
            //    sql += " AND A.SHIFT_NAME='" + sShiftName + "'";
            //}
            if (sMoveType != "")
            {
                sql += " AND A.ACTIVITY='" + sMoveType + "'";
            }
            if (sStepName != "")
            {
                sql += " AND A.STEP_NAME='" + sStepName + "'";
            }
            if (sFactoryroomName != "ALL")
            {
                sql += " AND B.FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            sql += " AND A.TIME_STAMP>=CONVERT(DATETIME,'" + sStartDate + "')";
            sql += " AND A.TIME_STAMP<CONVERT(DATETIME,'" + sEndDate + "')";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetLinePatchData(string sFactoryroomName,string slineName, string sStartDate, string sEndDate, string sShiftName, string sPatchItem)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = @"SELECT FACTORYROOM_NAME,PATCH_DATE,SHIFT_NAME,PATCH_ITEM,SUM(PATCH_QTY) AS 'PATCH_QTY',SUM(TOT_QTY) AS 'TOT_QTY'
                    FROM RPT_PATCH_DATA_TEST
                    WHERE 1=1";
            if (sFactoryroomName != "ALL")
            {
                sql += " AND FACTORYROOM_NAME='" + sFactoryroomName + "'";
            }
            if (slineName != "")
            {
                sql += string.Format(" AND LINE_NAME IN ({0})",slineName);
            }

            if (sShiftName != "ALL")
            {
                sql += " AND SHIFT_NAME='" + sShiftName + "'";
            }
            if (sPatchItem != "")
            {
                sql += " AND PATCH_ITEM='" + sPatchItem + "'";
            }
            sql += " AND PATCH_DATE>='" + sStartDate + "'";
            sql += " AND PATCH_DATE<='" + sEndDate + "'";
            sql += " GROUP BY FACTORYROOM_NAME,PATCH_DATE,SHIFT_NAME,PATCH_ITEM";

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        public static DataSet GetFactoryWorkPlace()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT LOCATION_KEY,LOCATION_NAME";
            sql += " FROM FMM_LOCATION";
            sql += " WHERE LOCATION_LEVEL='5'";
            sql += " ORDER BY LOCATION_NAME";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }
        public static DataSet GetLineName()
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            sql = "SELECT LINE_NAME,LINE_CODE FROM FMM_PRODUCTION_LINE";
            sql += " WHERE LINE_CODE LIKE 'L%'";
            sql += " ORDER BY LINE_NAME";
            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);
            return dsReturn;
        }

        /// <summary>
        /// 获得不良数据明细信息
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sName"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataSet QueryPatchLineDataDtl(string sType, string sName, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            DataSet dsReturn = new DataSet();
            if (locationKey.ToUpper().Trim().Equals("ALL"))
                locationKey = string.Empty;
            if (lineName.ToUpper().Trim().Equals("ALL"))
                lineName = string.Empty;
            if (shiftName.ToUpper().Trim().Equals("ALL"))
                shiftName = string.Empty;

            //A:(焊前+焊后)总碎片率,B:焊前碎片率,C:焊后碎片率
            if (sType.Trim().ToUpper().Equals("A") || sType.Trim().ToUpper().Equals("B") || sType.Trim().ToUpper().Equals("C"))
            {
                dsReturn = GetPatchLineDataForABC(sType, locationKey, lineName, shiftName, startDate, endDate);
            }
            //D:红外不良碎片率
            if (sType.Trim().ToUpper().Equals("D"))
            {
                dsReturn = GetPatchLineDataForD(sType, locationKey, lineName, shiftName, startDate, endDate);
            }
            //E:硅片供应商碎片率
            if (sType.Trim().ToUpper().Equals("E"))
            {
                dsReturn = GetPatchLineDataForE(sType, sName, locationKey, lineName, shiftName, startDate, endDate);
            }
            //F:各工序碎片率
            if (sType.Trim().ToUpper().Equals("F"))
            {
                dsReturn = GetPatchLineDataForF(sType, sName, locationKey, lineName, shiftName, startDate, endDate);
            }
            //G:杭州各栋别碎片率 
            if (sType.Trim().ToUpper().Equals("G"))
            {
                dsReturn = GetPatchLineDataForG(sType, sName, locationKey, lineName, shiftName, startDate, endDate);
            }
            //H:各型号碎片率 
            if (sType.Trim().ToUpper().Equals("H"))
            {
                dsReturn = GetPatchLineDataForH(sType, sName, locationKey, lineName, shiftName, startDate, endDate);
            }

            return dsReturn;
        }

        /// <summary>
        /// 各型号碎片率
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sName"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForH(string sType, string sName, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            string opt = string.Empty;
            DataSet dsReturn = new DataSet(); ;

            opt = sName.Substring(1, 5);

            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @"SELECT '''' + C.LOT_NUMBER AS 'LOT_NUMBER',B.REASON_CODE_NAME,B.PATCH_QUANTITY,B.RESPONSIBLE_PERSON,
            			            C.PRO_ID,C.WORK_ORDER_NO,C.PALLET_NO,C.PALLET_TIME,A.OPR_LINE,ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) AS 'LINE_NAME',
            			            C.EFFICIENCY,SUBSTRING(C.MATERIAL_LOT,CHARINDEX('-',C.MATERIAL_LOT)-1,1) AS '质量等级',
                                    C.SUPPLIER_NAME,C.SI_LOT
            	              FROM WIP_PATCH B 
                              INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                              INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                              INNER JOIN POR_PRODUCT D ON C.PRO_ID=D.PRODUCT_CODE
            	              WHERE B.REASON_CODE_CLASS IN ('19','21')	        
            	              AND D.ISFLAG=1
            	              AND A.ACTIVITY='PATCHED'
                              AND A.UNDO_FLAG=0";

            sqlPatchCount = @"SELECT B.REASON_CODE_NAME,SUM(B.PATCH_QUANTITY) AS 'PATCH_TOT'
                              FROM WIP_PATCH B 
                              INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                              INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
                              INNER JOIN POR_PRODUCT D ON C.PRO_ID=D.PRODUCT_CODE
            	              WHERE B.REASON_CODE_CLASS IN ('19','21')	        
            	              AND D.ISFLAG=1
            	              AND A.ACTIVITY='PATCHED'
                              AND A.UNDO_FLAG=0";

            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" and C.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) IN ({0})", lineName);
            if (!string.IsNullOrEmpty(opt))
                sqlCommand += string.Format(@"  AND D.PROMODEL_NAME='{0}'", opt);

            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY B.REASON_CODE_NAME
	                            ORDER BY SUM(B.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        /// <summary>
        /// 各栋别碎片率
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sName"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForG(string sType, string sName, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            string opt = string.Empty;
            DataSet dsReturn = new DataSet();

            int istart = sName.IndexOf('(', 0);
            opt = sName.Substring(istart, 1);


            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @"  SELECT  '''' + C.LOT_NUMBER AS 'LOT_NUMBER',B.REASON_CODE_NAME,B.PATCH_QUANTITY,B.RESPONSIBLE_PERSON,
            			                C.PRO_ID,C.WORK_ORDER_NO,C.PALLET_NO,C.PALLET_TIME,A.OPR_LINE,ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) AS 'LINE_NAME',
            			                C.EFFICIENCY,SUBSTRING(C.MATERIAL_LOT,CHARINDEX('-',C.MATERIAL_LOT)-1,1) AS '质量等级',
                                        C.SUPPLIER_NAME,C.SI_LOT
            	                  FROM WIP_PATCH B
                                  INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                                  INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
            	                  WHERE B.REASON_CODE_CLASS IN ('19','21')	       
            	                  AND C.SUPPLIER_NAME='正泰'	                 
            	                  AND A.ACTIVITY='PATCHED'
                                  AND A.UNDO_FLAG=0";

            sqlPatchCount = @"SELECT B.REASON_CODE_NAME,SUM(B.PATCH_QUANTITY) AS 'PATCH_TOT'
                              FROM WIP_PATCH B
                                  INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                                  INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
            	                  WHERE B.REASON_CODE_CLASS IN ('19','21')	       
            	                  AND C.SUPPLIER_NAME='正泰'	                 
            	                  AND A.ACTIVITY='PATCHED'
                                  AND A.UNDO_FLAG=0";

            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" and C.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) IN ({0})", lineName);
            if (!string.IsNullOrEmpty(opt))
                sqlCommand += string.Format(@" AND SUBSTRING(C.SI_LOT,CHARINDEX('-',C.SI_LOT)+2,1)='{0}'", opt);

            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY B.REASON_CODE_NAME
	                            ORDER BY SUM(B.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        /// <summary>
        /// 各工序碎片率
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sName"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForF(string sType, string sName, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {

            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            string opt = string.Empty;
            DataSet dsReturn = new DataSet();

            int iend = sName.IndexOf(')', 0);
            opt = sName.Substring(1, iend - 1);


            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @"  SELECT  '''' + C.LOT_NUMBER AS 'LOT_NUMBER',B.REASON_CODE_NAME,B.PATCH_QUANTITY,B.RESPONSIBLE_PERSON,
            			                C.PRO_ID,C.WORK_ORDER_NO,C.PALLET_NO,C.PALLET_TIME,A.OPR_LINE,ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) AS 'LINE_NAME',
            			                C.EFFICIENCY,SUBSTRING(C.MATERIAL_LOT,CHARINDEX('-',C.MATERIAL_LOT)-1,1) AS '质量等级',
                                        C.SUPPLIER_NAME,C.SI_LOT
            	                  FROM WIP_PATCH B
                                  INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                                  INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
            	                  WHERE B.REASON_CODE_CLASS IN ('19','21')	              
            	                  AND A.ACTIVITY='PATCHED'
                                  AND A.UNDO_FLAG=0";

            sqlPatchCount = @"SELECT B.REASON_CODE_NAME,SUM(B.PATCH_QUANTITY) AS 'PATCH_TOT'
                              FROM WIP_PATCH B
                                  INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY
                                  INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
            	                  WHERE B.REASON_CODE_CLASS IN ('19','21')	              
            	                  AND A.ACTIVITY='PATCHED'
                                  AND A.UNDO_FLAG=0";

            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" and C.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) IN ({0})", lineName);
            if (!string.IsNullOrEmpty(opt))
                sqlCommand += string.Format(@" AND B.STEP_NAME='{0}'", opt);

            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" and A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" and A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY B.REASON_CODE_NAME
	                            ORDER BY SUM(B.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        /// <summary>
        /// 硅片供应商碎片率
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForE(string sType, string sName, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            string supply = string.Empty;
            DataSet dsReturn = new DataSet();

            int iend = sName.IndexOf(')', 0);
            supply = sName.Substring(1, iend - 1);


            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @"  SELECT  '''' + C.LOT_NUMBER AS 'LOT_NUMBER',B.REASON_CODE_NAME,B.PATCH_QUANTITY,B.RESPONSIBLE_PERSON,
			                C.PRO_ID,C.WORK_ORDER_NO,C.PALLET_NO,C.PALLET_TIME,ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) AS 'LINE_NAME',
			                C.EFFICIENCY,SUBSTRING(C.MATERIAL_LOT,CHARINDEX('-',C.MATERIAL_LOT)-1,1) AS '质量等级',
                            C.SUPPLIER_NAME,C.SI_LOT
	                  FROM  WIP_PATCH B
                      INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY 
                      INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
	                  WHERE B.REASON_CODE_CLASS IN ('19','21')	            
	                  AND A.ACTIVITY='PATCHED'
                      AND A.UNDO_FLAG=0";

            sqlPatchCount = @"SELECT B.REASON_CODE_NAME,SUM(B.PATCH_QUANTITY) AS 'PATCH_TOT'
                              FROM  WIP_PATCH B
                              INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY 
                              INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY
	                          WHERE B.REASON_CODE_CLASS IN ('19','21')	            
	                          AND A.ACTIVITY='PATCHED'
                              AND A.UNDO_FLAG=0";

            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" and C.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) IN ({0})", lineName);
            if (!string.IsNullOrEmpty(supply))
                sqlCommand += string.Format(@" AND C.SUPPLIER_NAME='{0}'", supply);

            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY B.REASON_CODE_NAME
	                            ORDER BY SUM(B.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        /// <summary>
        /// 红外不良碎片率
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="locationKey"></param>
        /// <param name="shiftName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForD(string sType, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            DataSet dsReturn = new DataSet();

            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @" SELECT  '''' + C.LOT_NUMBER AS 'LOT_NUMBER',B.REASON_CODE_NAME,B.PATCH_QUANTITY,B.RESPONSIBLE_PERSON,
                            C.PRO_ID,C.WORK_ORDER_NO,C.PALLET_NO,C.PALLET_TIME,A.OPR_LINE,ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) AS 'LINE_NAME',
                            C.EFFICIENCY,SUBSTRING(C.MATERIAL_LOT,CHARINDEX('-',C.MATERIAL_LOT)-1,1) AS '质量等级',
                            C.SUPPLIER_NAME,C.SI_LOT
                              FROM WIP_PATCH B
                              INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY 
                              INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY 
                              WHERE B.REASON_CODE_CLASS='21'
                              AND B.REASON_CODE_NAME LIKE '%红外不良%' 
                              AND A.ACTIVITY='PATCHED'
                              AND A.UNDO_FLAG=0";
            sqlPatchCount = @"SELECT B.REASON_CODE_NAME,SUM(B.PATCH_QUANTITY) AS 'PATCH_TOT'
                              FROM WIP_PATCH B
                              INNER JOIN WIP_TRANSACTION A ON A.TRANSACTION_KEY=B.PATCHED_TRANSACTION_KEY 
                              INNER JOIN WIP_LOT C ON C.TRANSACTION_KEY=A.TRANSACTION_KEY 
                              WHERE B.REASON_CODE_CLASS='21'
                              AND B.REASON_CODE_NAME LIKE '%红外不良%' 
                              AND A.ACTIVITY='PATCHED'
                              AND A.UNDO_FLAG=0";


            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" AND C.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(A.OPR_LINE,C.LOT_LINE_CODE) IN ({0})", lineName);
            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" AND A.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND A.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY B.REASON_CODE_NAME
	                            ORDER BY SUM(B.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        /// <summary>
        /// 获得焊前焊后碎片不良信息数据
        /// </summary>
        /// <param name="sType">不良信息类别</param>
        /// <param name="locationKey">车间</param>
        /// <param name="shiftName">班别</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        private static DataSet GetPatchLineDataForABC(string sType, string locationKey, string lineName, string shiftName, string startDate, string endDate)
        {
            string sqlCommand = string.Empty;
            string sqlPatchData = string.Empty;
            string sqlPatchCount = string.Empty;

            DataSet dsReturn = new DataSet();

            string sShiftPoint = DateTime.Parse(startDate).ToString("yyyy-MM-dd") + " 21:00:00";
            sqlPatchData = @"select '''' + t2.LOT_NUMBER AS 'LOT_NUMBER',t.REASON_CODE_NAME,t.PATCH_QUANTITY,t.RESPONSIBLE_PERSON,
                                        t2.PRO_ID,t2.WORK_ORDER_NO,t2.PALLET_NO,t2.PALLET_TIME,ISNULL(t1.OPR_LINE,t2.LOT_LINE_CODE) AS 'LINE_NAME',
                                        t2.EFFICIENCY,SUBSTRING(t2.MATERIAL_LOT,CHARINDEX('-',t2.MATERIAL_LOT)-1,1) AS '质量等级',
                                        t2.SUPPLIER_NAME,t2.SI_LOT
                                        from WIP_PATCH t
                                        INNER JOIN WIP_TRANSACTION t1 ON t.PATCHED_TRANSACTION_KEY=t1.TRANSACTION_KEY
                                        INNER JOIN WIP_LOT t2 ON t2.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                        WHERE t1.ACTIVITY='PATCHED'
                                        AND t1.UNDO_FLAG=0";

            sqlPatchCount = @"select t.REASON_CODE_NAME,SUM(t.PATCH_QUANTITY) AS 'PATCH_TOT'
                                        from WIP_PATCH t
                                        INNER JOIN WIP_TRANSACTION t1 ON t.PATCHED_TRANSACTION_KEY=t1.TRANSACTION_KEY
                                        INNER JOIN WIP_LOT t2 ON t2.TRANSACTION_KEY=t1.TRANSACTION_KEY
                                        WHERE t1.ACTIVITY='PATCHED'
                                        AND t1.UNDO_FLAG=0";

            if (!string.IsNullOrEmpty(locationKey))
                sqlCommand += string.Format(@" AND t2.FACTORYROOM_NAME='{0}'", locationKey.PreventSQLInjection());
            if (!string.IsNullOrEmpty(lineName))
                sqlCommand += string.Format(@" AND ISNULL(t1.OPR_LINE,t2.LOT_LINE_CODE) IN ({0})", lineName);
            if (sType.Trim().ToUpper().Equals("A"))
                sqlCommand += string.Format(@" AND t.REASON_CODE_CLASS in ('19','21')");
            if (sType.Trim().ToUpper().Equals("B"))
                sqlCommand += string.Format(@" AND t.REASON_CODE_CLASS = '19'");
            if (sType.Trim().ToUpper().Equals("C"))
                sqlCommand += string.Format(@" AND t.REASON_CODE_CLASS = '21'");


            if (shiftName.ToUpper() == "A")
            {
                sqlCommand += string.Format(@" AND t1.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND t1.TIME_STAMP <=CONVERT(datetime,'{0}')", sShiftPoint);
            }
            else if (shiftName.ToUpper() == "B")
            {
                sqlCommand += string.Format(@" AND t1.TIME_STAMP >=CONVERT(datetime,'{0}')", sShiftPoint);
                sqlCommand += string.Format(@" AND t1.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }
            else
            {
                sqlCommand += string.Format(@" AND t1.TIME_STAMP >=CONVERT(datetime,'{0}')", startDate);
                sqlCommand += string.Format(@" AND t1.TIME_STAMP <=CONVERT(datetime,'{0}')", endDate);
            }

            sqlPatchData += sqlCommand;

            sqlPatchCount += sqlCommand;

            sqlPatchCount += @"	GROUP BY t.REASON_CODE_NAME
	                            ORDER BY SUM(t.PATCH_QUANTITY) DESC";

            DataTable dtPatcheData = db.ExecuteDataSet(CommandType.Text, sqlPatchData).Tables[0];
            dtPatcheData.TableName = "PATCH_DATA";
            dsReturn.Merge(dtPatcheData);

            DataTable dtPatcheCount = db.ExecuteDataSet(CommandType.Text, sqlPatchCount).Tables[0];
            dtPatcheCount.TableName = "PATCH_COUNT";
            dsReturn.Merge(dtPatcheCount);

            return dsReturn;
        }

        public static DataSet GetDateTime(string sDay, string sShiftName, string sLocationName)
        {
            string sql = string.Empty;
            DataSet dsReturn = new DataSet();
            if (sShiftName == "ALL")
            {
                sql = "select k.LOCATION_NAME,k.OPERATION_NAME,MIN(k.START_TIME) AS 'START_TIME',MAX(k.END_TIME) AS 'END_TIME'";
                sql += " from (select LOCATION_NAME,OPERATION_NAME,case when SHIFT_NAME='白班' then 'A' ELSE 'B' END AS 'SHIFT_NAME'";
                sql += ",CONVERT(DATETIME,('" + sDay + "' + ' ' + START_TIME + ':00')) as 'START_TIME'";
                sql += ",DATEADD(DAY,CONVERT(INT,OVER_DAY),CONVERT(DATETIME,('" + sDay + "' + ' ' + END_TIME + ':00'))) as 'END_TIME'";
                sql += " from BASE_OPT_SETTING where ISFLAG=1";
                sql += " and OPERATION_NAME='单串焊') k";
                sql += " where 1=1";
                if (sLocationName != "ALL")
                {
                    sql += " and k.LOCATION_NAME='" + sLocationName + "'";
                }
                sql += " group by k.LOCATION_NAME,k.OPERATION_NAME";
                sql += " order by k.OPERATION_NAME";
            }
            else
            {
                sql = "select t.* from ";
                sql += " (select LOCATION_NAME,OPERATION_NAME,case when SHIFT_NAME='白班' then 'A' ELSE 'B' END AS 'SHIFT_NAME'";
                sql += ",CONVERT(DATETIME,('" + sDay + "' + ' ' + START_TIME + ':00')) as 'START_TIME'";
                sql += ",DATEADD(DAY,CONVERT(INT,OVER_DAY),CONVERT(DATETIME,('" + sDay + "' + ' ' + END_TIME + ':00'))) as 'END_TIME'";
                sql += " from BASE_OPT_SETTING where ISFLAG=1";
                sql += " and OPERATION_NAME='单串焊') t";
                sql += " where t.SHIFT_NAME='" + sShiftName + "'";
                if (sLocationName != "ALL")
                {
                    sql += " and t.LOCATION_NAME='" + sLocationName + "'";
                }
                sql += " order by t.LOCATION_NAME,t.SHIFT_NAME";
            }

            dsReturn = db.ExecuteDataSet(CommandType.Text, sql);

            return dsReturn;
        }

    }
}

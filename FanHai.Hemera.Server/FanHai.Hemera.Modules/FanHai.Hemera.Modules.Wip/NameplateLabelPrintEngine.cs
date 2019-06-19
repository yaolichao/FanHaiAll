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
    public class NameplateLabelPrintEngine : AbstractEngine, INameplateLabelPrintEngine
    {
        /// <summary>
        /// 数据库操作对象。
        /// </summary>
        private Database db = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public NameplateLabelPrintEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public override void Initialize()
        {

        }

        #region INameplateLabelPrintEngine 成员

        public DataSet GetMatrialByWorkOrderNumber(string orderNumber)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT A.PART_NUMBER,A.PRODUCT_CODE 
                                                        FROM dbo.POR_WO_PRD A
                                                        INNER JOIN POR_WORK_ORDER B ON
                                                        A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                        WHERE A.IS_USED = 'Y'
                                                        AND B.ORDER_NUMBER = '{0}'", orderNumber);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMatrialByWorkOrderNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region INameplateLabelPrintEngine 成员


        public DataSet GetPowerByWOnumberAndPartID(string _orderNumner, string _partName)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT PMAXSTAB FROM dbo.POR_WO_PRD_PS A 
                                                            INNER JOIN POR_WORK_ORDER B 
                                                            ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                            where A.PART_NUMBER = '{0}'
                                                            AND B.ORDER_NUMBER = '{1}'
                                                            AND IS_USED = 'Y'
                                                            AND B.ORDER_STATE = 'REL'", _partName,_orderNumner);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerByWOnumberAndPartID Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region INameplateLabelPrintEngine 成员


        public DataSet GetInfByWOnumberAndPartIDAndPower(string _orderNumner, string _partName, string _power)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT PMAXSTAB,ISCSTAB,VOCSTAB,IMPPSTAB,VMPPSTAB,FUSE,PS_CODE,POWER_DIFFERENCE FROM dbo.POR_WO_PRD_PS A 
                                                            INNER JOIN POR_WORK_ORDER B 
                                                            ON A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                            where A.PART_NUMBER = '{0}'
                                                            AND B.ORDER_NUMBER = '{1}'
                                                            AND IS_USED = 'Y'
                                                            AND B.ORDER_STATE = 'REL' AND PMAXSTAB = '{2}'", _partName, _orderNumner, _power);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetPowerByWOnumberAndPartID Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region INameplateLabelPrintEngine 成员


        public DataSet GetInfByWOnumberAndPartID(string _orderNumner, string _partName)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT PROMODEL_NAME,LABELTYPE,TOLERANCE
                                                            FROM dbo.POR_WO_PRD A
                                                            INNER JOIN POR_WORK_ORDER B ON
                                                            A.WORK_ORDER_KEY = B.WORK_ORDER_KEY
                                                            WHERE A.IS_USED = 'Y'
                                                            AND B.ORDER_NUMBER = '{0}'
                                                            AND A.PART_NUMBER = '{1}'", _orderNumner, _partName);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetMatrialByWorkOrderNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion

        #region INameplateLabelPrintEngine 成员


        public DataSet GetCellTypeByWorkOrderNumber(string workOrder)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                string sqlCommon = string.Format(@"SELECT CELL_TYPE FROM POR_WORK_ORDER
                                                            WHERE ORDER_NUMBER = '{0}'", workOrder);

                dsReturn = db.ExecuteDataSet(CommandType.Text, sqlCommon);
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, "");
            }
            catch (Exception ex)
            {
                FanHai.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                LogService.LogError("GetCellTypeByWorkOrderNumber Error: " + ex.Message);
            }
            return dsReturn;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    public class ExchangeWoEntity
    {
        #region Private variable definitions
        private string _errorMsg = string.Empty;
        #endregion

        #region Properties
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        #endregion

        public DataSet GetExchangeWoData(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            pages = 0;
            records = 0;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIExchangeWoEngine().GetExchangeWoData(reqDS, pageNo, pageSize, out  pages, out  records, hstable);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据查询条件获取返工单批次数据。
        /// </summary>
        /// <param name="hstable">查询条件。</param>
        /// <returns>包好返工单数据数据集对象。</returns>
        public DataSet GetExchangeByFilter(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIExchangeWoEngine().GetExchangeByFilter(hstable);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        public string CompareWorkOrderType(Hashtable hstable)
        {

            string msg = string.Empty;
            string oldWorkNo = Convert.ToString(hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER+"2"]);
            string newWorkNo = Convert.ToString(hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
            string col01="Order_Type", col02="Work_Order_No";
            string[] l_s = new string[] {col01,col02 };
            string category = BASEDATA_CATEGORY_NAME.Uda_work_type;
            DataTable dtWorkType = BaseData.Get(l_s, category);

            DataRow[] drsOldWorkNo = dtWorkType.Select(string.Format(col02 + "='{0}'", oldWorkNo.Substring(0,2)));
            if (drsOldWorkNo != null && drsOldWorkNo.Length > 0)
            {
                string oldType = Convert.ToString(drsOldWorkNo[0][col01]);

                DataRow[] drsNewWorkNo = dtWorkType.Select(string.Format(col02 + "='{0}'", newWorkNo.Substring(0, 2)));
                if (drsNewWorkNo != null && drsNewWorkNo.Length > 0)
                {
                    string newType = Convert.ToString(drsNewWorkNo[0][col01]);
                     if (oldType.Equals("R") && newType.Equals("P"))
                    {
                        msg = string.Format(@"旧工单【{0}】为【返工工单】不能转到【量产工单】，请确认!", oldWorkNo);
                    }
                }
                else
                {
                    msg = string.Format(@"新工单【{0}】未设定工单类型【量产工单/返工工单】，不能转工单!", newWorkNo);
                }
            }
            else
            {
                msg = string.Format(@"旧工单【{0}】未设定工单类型【量产工单/返工工单】，不能转工单!", oldWorkNo);
            }
            return msg;
        }

        public DataSet SaveExchangeWo(DataSet dsSave)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIExchangeWoEngine().SaveExchangeWo(dsSave);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
    }
}

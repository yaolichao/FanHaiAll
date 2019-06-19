using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using System.Data;

namespace FanHai.Hemera.Utils.Entities
{
    public class OutDeliveryQuerryEngine
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        /// <summary>
        /// 查询外向交货单明细记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含外向交货单明细记录信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IOutDeliveryQuerryEngine>().OutDeliveryQuerry(dsParams);
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
        //
        /// <summary>
        /// 查询质检明细记录。
        /// </summary>
        /// <param name="id">外向交货明细ID。</param>
        /// <returns>对应外向交货ID质检明细记录信息的数据集对象。</returns>
        public DataSet GetQCItem(string VBELN, string POSNR)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IOutDeliveryQuerryEngine>().GetQCItem(VBELN, POSNR);
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
        //

    }
}

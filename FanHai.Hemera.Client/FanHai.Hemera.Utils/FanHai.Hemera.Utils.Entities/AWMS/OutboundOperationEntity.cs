using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class OutboundOperationEntity
    {
        private string _errorMsg = string.Empty;
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        /// <summary>
        /// 查询外向交货单出库记录
        /// </summary>
        /// <param name="dsParam"></param>
        /// <param name="PQConf"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataSet Query(DataSet dsParam, ref PagingQueryConfig PQConf)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IOutboundOperationEngine>().Query(dsParam, ref PQConf);
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
        /// 根据外向交货单号或者出库单号查询外向交货单抬头相关数据
        /// </summary>
        /// <param name="OutboundNo"></param>
        /// <param name="VebelNO"></param>
        /// <returns></returns>
        public DataSet getOutboudInfo(string OutboundNo, string VebelNO, string ShipmentNO, string SType)
        { 
            DataSet strReturn=null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strReturn = serverFactory.Get<IOutboundOperationEngine>().getOutboudInfo(OutboundNo, VebelNO, ShipmentNO, SType);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return strReturn;
        }

        /// <summary>
        /// 根据外向交货单号或者出库单号查询明细
        /// </summary>
        /// <param name="OutboundNo"></param>
        /// <param name="VebelNO"></param>
        /// <returns></returns>
        public DataSet getOutboudItem(string OutboundNo, string VebelNO, string ShipmentNO, string SType)
        {
            DataSet strReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strReturn = serverFactory.Get<IOutboundOperationEngine>().getOutboudItem(OutboundNo, VebelNO, ShipmentNO, SType);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return strReturn;
        }
        /// <summary>
        /// 保存检验结果
        /// </summary>
        /// <param name="dsParams"></param>
        /// <param name="OutboundNo"></param>
        /// <param name="VebelNO"></param>
        /// <param name="QC_PERSON"></param>
        /// <returns></returns>
        public string SetQcResult(DataSet dsParams, string OutboundNo, string VebelNO, string QC_PERSON, string IsEdit, out bool result)
        {
            string strReturn = string.Empty;
            //try
            //{
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strReturn = serverFactory.Get<IOutboundOperationEngine>().SetQcResult(dsParams, OutboundNo, VebelNO, QC_PERSON,IsEdit,out result);
                //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            //}
            //catch ()
            //{
               // _errorMsg = ex.Message;
            //}
            //finally
            //{
                CallRemotingService.UnregisterChannel();
            //}
            return strReturn;
        }
        /// <summary>
        /// 删除检验结果
        /// </summary>
        /// <param name="OutboundNo"></param>
        /// <param name="VebelNO"></param>
        /// <returns></returns>
        public void  DeleteQcResult(string OutboundNo, string VebelNO,string Del_Person)
        {
            try
            {
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            serverFactory.Get<IOutboundOperationEngine>().DeleteQcResult(OutboundNo, VebelNO,Del_Person);
            //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
             _errorMsg = ex.Message;
            }
            finally
            {
            CallRemotingService.UnregisterChannel();
            }
            
        }
        /// <summary>
        /// 更新表字段
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="VebelNO"></param>
        /// <param name="Outboudno"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public string UpdateTable(string Msg, string VebelNO, string Outboudno, int Flag)
        {
            string strReturn = string.Empty;
            //try
            //{
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strReturn = serverFactory.Get<IOutboundOperationEngine>().UpdateTable(Msg, VebelNO, Outboudno, Flag);
                //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            //}
            //catch ()
            //{
                //_errorMsg = ex.Message;
            //}
            //finally
            //{
                CallRemotingService.UnregisterChannel();
            //}
            return strReturn;
        }
        /// <summary>
        /// 更新柜号等信息
        /// </summary>
        /// <param name="OutboundNo"></param>
        /// <param name="containerNo"></param>
        /// <param name="ciNo"></param>
        /// <param name="shipmentType"></param>
        /// <returns></returns>
        public bool UpdateConteinerNo(string OutboundNo, string VebelNO, string containerNo)
        {
            bool bReturn = false;
            DataSet dsRerurn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsRerurn = serverFactory.Get<IOutboundOperationEngine>().UpdateConteinerNo(OutboundNo,VebelNO, containerNo);
                 _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                 //执行失败。
                 if (_errorMsg == string.Empty)
                     bReturn = true;
               
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                 return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }

    }
}

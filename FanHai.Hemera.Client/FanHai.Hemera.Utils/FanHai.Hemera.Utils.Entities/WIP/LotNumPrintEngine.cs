using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class LotNumPrintEngine
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
        /// 获取未打印的组件序列号
        /// </summary>
        /// <param name="facKey">车间主键</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns>未打印的组件序列号数据集</returns>
        public DataSet GetNotPrintLotNumInf(string facKey, string equipmentKey, string lineKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().GetNotPrintLotNumInf(facKey, equipmentKey, lineKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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
        /// 获取序列号打印的详细信息
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="printerNo"></param>
        /// <returns></returns>
        public DataSet GetPrintInf(string lotNumber, string dateStart, string dateEnd, string printerNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().GetPrintInf( lotNumber, dateStart, dateEnd,printerNo );
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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
        /// 确认批次是否存在且更新信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <param name="printer">批次号</param>
        /// <returns>是否更新成功</returns>
        public bool CheckAndUpdateLotInf(string lotNumber,string printer,string facKey,string equipmentKey,string lineKey)
        {
            bool _return = false;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().CheckAndUpdateLotInf(lotNumber, printer,facKey,equipmentKey,lineKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            if (string.IsNullOrEmpty(_errorMsg))
                _return = true;
            return _return;
        }
        /// <summary>
        /// 更新打印信息
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <param name="printer"></param>
        /// <param name="facKey"></param>
        /// <param name="equipmentKey"></param>
        /// <param name="lineKey"></param>
        /// <returns></returns>
        public bool UpdateLotInf(string lotNumber, string printer)
        {
            bool _return = false;
            //DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    _return = serverFactory.CreateILotNumPrintEngine().UpdateLotInf(lotNumber, printer);
                    //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
           
            return _return;
        }

        public bool save_Print(string lotNumber, string printer, string mac, char is_RePrint)
        {
            bool _return = false;
            //DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    _return = serverFactory.CreateILotNumPrintEngine().save_Print(lotNumber, printer,mac,is_RePrint);
                    //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return _return;
        }
        /// <summary>
        /// 根据工单号获取打印代码
        /// </summary>
        /// <param name="orderNum">工单号</param>
        /// <returns></returns>
        public DataSet GetIdByOrderNumber(string orderNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().GetIdByOrderNumber(orderNum);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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
        /// 根据组件序列号获取打印代码
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        public DataSet GetPrintIdByLotNumber(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().GetPrintIdByLotNumber(lotNumber);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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
        /// //根据批次号获取工序设备线别信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        public DataSet GetEquipmentByLotNumber(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().GetEquipmentByLotNumber(lotNumber);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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
        /// 更新数据信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <param name="operations">工序名称</param>
        /// <param name="equipmentkey">设备主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns></returns>
        public DataSet UpdatePorLot(string lotNumber, string operations,string equipmentkey,string lineKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILotNumPrintEngine().UpdatePorLot(lotNumber, operations, equipmentkey, lineKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
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

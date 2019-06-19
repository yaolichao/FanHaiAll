using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 标签或铭牌操作相关的实体类。
    /// </summary>
    public class PrintLabelEntity
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
        /// 获取标签或铭牌数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据的数据集对象。</returns>
        public DataSet GetPrintLabelData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetPrintLabelData();
                _errorMsg=ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取标签或铭牌明细数据。
        /// </summary>
        /// <returns>包含标签或铭牌明细数据的数据集对象。</returns>
        public DataSet GetPrintLabelDetailData(string labelId)
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetPrintLabelDetailData(labelId);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
         /// <summary>
        /// 是否允许设置标签或数据为无效数据。
        /// </summary>
        /// <param name="labelId">标签或铭牌ID</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public bool IsAllowInvalid(string labelId)
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().IsAllowInvalid(labelId);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return false;
                }
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
            return true;
        }
        /// <summary>
        /// 获取产品型号数据。
        /// </summary>
        /// <returns>包含产品型号数据的数据集对象。</returns>
        public DataSet GetProductModelData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetProductModelData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取认证类型数据。
        /// </summary>
        /// <returns>包含认证类型数据的数据集对象。</returns>
        public DataSet GetCertificateTypeData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetCertificateTypeData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取分档方式数据。
        /// </summary>
        /// <returns>包含分档方式数据的数据集对象。</returns>
        public DataSet GetPowersetTypeData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetPowersetTypeData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取标签或铭牌数据类型数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据类型数据的数据集对象。</returns>
        public  DataSet GetLabelDataTypeData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetLabelDataTypeData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取打印机类型数据。
        /// </summary>
        /// <returns>包含打印机类型数据的数据集对象。</returns>
        public DataSet GetPrinterTypeData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetPrinterTypeData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取铭牌检验类型数据。
        /// </summary>
        /// <returns>包含获取铭牌检验类型数据集对象。</returns>
        public DataSet GetCustCheckTypeData()
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().GetCustCheckTypeData();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return null;
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 保存标签或铭牌数据。
        /// </summary>
        /// <param name="dsParams">包含标签或铭牌数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public void SavePrintLabelData(DataSet dsParams)
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.Get<IBasicPrintLabelEngine>().SavePrintLabelData(dsParams);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
    }
}

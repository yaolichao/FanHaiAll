using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using System.Data;
using FanHai.Hemera.Share.Common;
using System.Collections;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 产品认证维护的实体类。
    /// </summary>
    public class BasicCertificationEntity
    {
        private string _errorMsg = "";
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        /// <summary>
        /// 获取认证数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetCertification(string certificationType, DateTime certificationDate)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IBasicCertificationEngine>().GetCertification(certificationType, certificationDate);
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
        /// 获取有效的认证数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetValidCertification()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IBasicCertificationEngine>().GetValidCertification();
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
        /// 获取有效的认证类型
        /// </summary>
        /// <returns></returns>
        public DataSet GetValidCertificationType()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IBasicCertificationEngine>().GetValidCertificationType();
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
        /// 保存
        /// </summary>
        /// <param name="dsChange"></param>
        /// <returns></returns>
        public DataSet SaveCertification(DataSet dsChange)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IBasicCertificationEngine>().SaveCertification(dsChange);
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

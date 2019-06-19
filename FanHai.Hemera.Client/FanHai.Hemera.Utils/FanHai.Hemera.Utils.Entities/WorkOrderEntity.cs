//----------------------------------------------------------------------------------
// Copyright (c) Chint
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间                   说明
// ---------------------------------------------------------------------------------
// 张振东               2013-06-21                 新增工单操作的实体类。
// =================================================================================
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
    /// 工单操作的实体类。
    /// </summary>
    public class WorkOrderEntity
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
        /// 获取产品ID号数据。
        /// </summary>
        /// <returns>包含产品ID号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetProdId();
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
        /// 获取产品料号数据。
        /// </summary>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        public DataSet GetPartNumber()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetPartNumber();
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
        /// 根据工单号获取工单信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单信息的数据集对象。</returns>
        public DataSet GetWorkorderInfo(string orderNumber)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetWorkorderInfo(orderNumber);
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
        /// 保存工单数据。
        /// </summary>
        /// <param name="dsParam">包含工单信息的数据集对象。</param>
        public void Save(DataSet dsParam)
        {
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().Save(dsParam);
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
        }
        /// <summary>
        /// 获取物料编码数据。
        /// </summary>
        /// <returns>包含物料编码数据的数据集对象。</returns>
        public DataSet GetMaterialCode()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetMaterialCode();
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
        /// 通过产品id获取ctm值
        /// </summary>
        /// <param name="proKey">产品id主键</param>
        /// <returns>ctm上下限管控信息</returns>
        public DataSet GetCtmInf(string proKey,string workOrder)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetCtmInf(proKey, workOrder);
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
        /// 通过工单号获取打印规则内容
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>数据集</returns>
        public DataSet GetPrintRuleData(string workOrderKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetPrintRuleData(workOrderKey);
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
        /// 获取产品类型打印编码
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>数据集</returns>
        public DataSet GetProductModel()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetProductModel();
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
        /// 通过工单号取EL 图片规则
        /// </summary>
        /// <param name="workOrderKey"></param>
        /// <returns></returns>
        public DataSet GetElPicRuleData(string workOrderKey,string ruleType)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetElPicRuleData(workOrderKey, ruleType);
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
        /// 通过工单号获取包装清单自动打印规则内容
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>数据集</returns>
        public DataSet GetFlashAutoPrintData(string workOrderNumber)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IWorkOrdersEngine>().GetFlashAutoPrintData(workOrderNumber);
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

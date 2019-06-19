//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-10-22            新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 查询工艺流程的实体类。
    /// </summary>
    public class RouteQueryEntity
    {
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            private set;
            get;
        }
        /// <summary>
        /// 获取已激活的工艺流程数据。
        /// </summary>
        /// <returns>包含已激活的工艺流程数据的数据集对象。
        /// [ROUTE_ROUTE_VER_KEY,ROUTE_NAME,DESCRIPTION]
        /// </returns>
        public DataSet GetActivedRouteData()
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIRouteEngine().GetActivedRouteData();
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 根据工艺流程主键获取工步数据。
        /// </summary>
        /// <returns>
        /// 包含工步数据的数据集对象。
        /// [ROUTE_STEP_KEY,ROUTE_STEP_NAME,ROUTE_STEP_SEQ]
        /// </returns>
        public DataSet GetRouteStepDataByRouteKey(string routeKey)
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIRouteEngine().GetRouteStepDataByRouteKey(routeKey);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 根据工步主键获取工步数据。
        /// </summary>
        /// <returns>
        /// 包含工步数据的数据集对象。
        /// </returns>
        public DataSet GetRouteStepDataByStepKey(string stepKey)
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIRouteEngine().GetStepDataByKey(stepKey);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
         /// <summary>
        /// 根据工步主键获取工步数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dctype">数据采集时刻。</param>
        /// <returns>包含工步数据的数据集对象。</returns>
        public DataSet GetStepBaseDataAndParamDataByKey(string stepKey,OperationParamDCType dctype)
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIRouteEngine().GetStepBaseDataAndParamDataByKey(stepKey, (int)dctype);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 根据工步主键获取工步自定义数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步自定义数据的数据集对象。</returns>
        public DataSet GetStepUda(string stepKey)
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIRouteEngine().GetStepUda(stepKey);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                return dsReturn;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取工步指定的自定义属性值。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="attributeName">自定义属性名。</param>
        /// <returns>自定义属性名对应的属性值。</returns>
        public string GetStepUdaValue(string stepKey, string attributeName)
        {
            try
            {
                this.ErrorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                return factor.CreateIRouteEngine().GetStepUdaValue(stepKey,attributeName);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return null;
        }
        /// <summary>
        /// 获取已激活且不重复的工序名称。
        /// </summary>
        /// <returns>
        /// 包含工序名称的数据集对象。
        /// [ROUTE_OPERATION_NAME]
        /// </returns>
        public DataSet GetDistinctOperationNameList()
        {
            string msg = string.Empty;
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIOperationEngine().GetDistinctOperationNameList();
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
                this.ErrorMsg = msg;
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据主键获取工序记录。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        public DataSet GetOperationByKey(string operationKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.ErrorMsg = string.Empty;
                string msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIOperationEngine().GetOperationByKey(operationKey);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    this.ErrorMsg = msg;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序自定义属性值。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <param name="attrName">自定义属性名称,如果为空则查询所有自定义属性数据。</param>
        /// <returns>自定义属性值。</returns>
        public string GetMaxVersionOperationAttrValue(string operationName, string attrName)
        {
            string attrValue=string.Empty;
            DataSet dsReturn = this.GetMaxVersionOperationAttrInfo(operationName, attrName);
            if (string.IsNullOrEmpty(this.ErrorMsg)
               && dsReturn.Tables.Count > 0
               && dsReturn.Tables[0].Rows.Count > 0)
            {
                attrValue = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE]);
            }
            return attrValue;
        }
        /// <summary>
        /// 获取版本号最大的工序自定义属性数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <param name="attrName">自定义属性名称,如果为空则查询所有自定义属性数据。</param>
        /// <returns>包含工序自定义数据的数据集对象。</returns>
        public DataSet GetMaxVersionOperationAttrInfo(string operationName, string attrName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.ErrorMsg = string.Empty;
                string msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIOperationEngine().GetMaxVersionOperationAttrInfo(operationName,attrName);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    this.ErrorMsg = msg;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取版本号最大的工序基础数据和参数数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序参数数据的数据集对象。</returns>
        public DataSet GetOperationBaseAndParamInfo(string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this.ErrorMsg = string.Empty;
                string msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIOperationEngine().GetOperationBaseAndParamInfo(operationName);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    this.ErrorMsg = msg;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工厂车间名称和产品类型获取工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程信息的数据集。</returns>
        public DataSet GetProcessPlan(string factoryName, string productType, bool isRework)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IEnterpriseEngine>().GetProcessPlan(factoryName, productType, isRework);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 根据工厂车间名称和产品类型获取首工序的工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程首工序信息的数据集。</returns>
        public DataSet GetProcessPlanFirstOperation(string factoryName, string productType, bool isRework)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<IEnterpriseEngine>().GetProcessPlanFirstOperation(factoryName, productType, isRework);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取指定工步的下一个工步。
        /// </summary>
        /// <returns>包含下一个工步数据的数据集对象。</returns>
        /// <param name="enterpriseKey">工艺流程组KEY</param>
        /// <param name="routeKey">工艺路线KEY</param>
        /// <param name="stepKey">工步KEY</param>
        /// <returns>工艺流程组名称,主键；工艺流程名称,主键，工步名称,主键</returns>
        public DataSet GetEnterpriseNextRouteAndStep(string enterpriseKey,string routeKey,string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEnterpriseEngine().GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                this.ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
    }
}

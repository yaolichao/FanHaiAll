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
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 批次创建实体类。
    /// </summary>
    public class LotCreateEntity
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
        /// 获取电池片终检录入信息 add by yongbing.yang 20130806
        /// </summary>
        /// <param name="Small_Pack_Number">小包条码</param>
        /// <returns></returns>
        public DataSet GetCellInformation(string Small_Pack_Number)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetCellInformation(Small_Pack_Number);
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
        /// 获取产品BOM清单
        /// </summary>
        /// <param name="orderNo">工单</param>
        /// <returns></returns>
        public DataSet GetWorkOrderBom(string orderNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetWorkOrderBom(orderNo);
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
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetProdId();
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
        /// 获取产品ID号对应的电池片数量。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <returns>电池片数量。</returns>
        public double GetCellNumber(string proId)
        {
            IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
            return iServerObjFactory.Get<ILotCreateEngine>().GetCellNumber(proId);
        }
        /// <summary>
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="orderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialInfo(string roomKey,string operationName,string orderNo,string proId)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetReceiveMaterialInfo(roomKey,operationName,orderNo,proId);
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
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="val">原材料线上仓存储明细主键。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialInfo(string val)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetReceiveMaterialInfo(val);
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
        /// 根据车间主键获取工单号。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。
        /// 【工单主键,ORDER_NUMBER（工单号）,产品ID号，成品编码，成品主键】
        /// </returns>
        public DataSet GetWorkOrderByFactoryRoomKey(string roomKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetWorkOrderByFactoryRoomKey(roomKey);
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
        /// 创建批次。
        /// </summary>
        /// <param name="dsParams">包含创建批次所需信息的数据集对象。</param>
        /// <returns>包含创建批次结果的数据集对象。</returns>
        public DataSet CeateLot(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().CeateLot(dsParams);
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
        /// 根据产品ID获取产品类型。
        /// </summary>
        /// <param name="sProductCode">产品ID。</param>
        /// <returns>
        /// 产品类型的数据集合。
        /// </returns>
        public DataSet GetProductModeByPID(string sProductCode)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetProductModeByPID(sProductCode);
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

        public DataSet GetOrderNoType(string OrderNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetOrderNoType(OrderNo);
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

        public DataSet GetOutCellSupplier(string smallPackNumber)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().GetOutCellSupplier(smallPackNumber);
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
        /// 传入工单主键获取工单中的规则信息创建除流水号之外的组件序列号
        /// </summary>
        /// <param name="orderKey">工单主键</param>
        /// <returns>主键序列号的数据集</returns>
        public DataSet CreateLotNumByRule(string orderKey,int count)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().CreateLotNumByRule(orderKey,count);
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
        /// 批次创建。自动生成组件序列号，设定单串焊线别设备
        /// </summary>
        /// <param name="dsParams">创批的数据集信息</param>
        /// <returns>包含创建批次结果的数据集对象</returns>
        public DataSet CeateNewLot(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotCreateEngine>().CeateNewLot(dsParams);
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

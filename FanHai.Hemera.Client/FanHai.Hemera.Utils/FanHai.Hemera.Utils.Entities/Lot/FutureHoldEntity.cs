//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-07-10            新建
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
    /// 批次预设暂停操作相关的实体类。
    /// </summary>
    public class FutureHoldEntity
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
        /// 根据主键获取预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        public DataSet Get(string key) {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotFutureHoldEngine().Get(key);
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
        /// 批量新增预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待预设暂停批次的数据集。</param>
        public void Insert(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotFutureHoldEngine().Insert(dsParams);
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
        /// 更新预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待更新的预设暂停批次的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public void Update(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotFutureHoldEngine().Update(dsParams);
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
        /// 根据主键删除预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <param name="deletor">删除人。</param>
        /// <param name="key">预设暂停主键。</param>
        public void Delete(string key, string deletor)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotFutureHoldEngine().Delete(key, deletor);
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
        /// 分页查询预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="config">数据分页配置对象。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotFutureHoldEngine().Query(dsParams,ref config);
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

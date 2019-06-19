//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-12-15            新建
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
    /// IV测试数据转置类。
    /// </summary>
    public class IVTestDataTransferEntity
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
        /// 根据设备代码获取最大的IV测试时间。
        /// </summary>
        /// <param name="deviceNo">设备代码。</param>
        /// <returns>指定设备最大的IV测试时间。</returns>
        public DateTime GetMaxIVTestTime(string deviceNo)
        {
            DateTime dtMaxTestTime = new DateTime(1900, 01, 01);
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dtMaxTestTime= iServerObjFactory.Get<IIVTestDataTransferEngine>().GetMaxIVTestTime(deviceNo);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dtMaxTestTime;
        }

        /// <summary>
        /// 新增IV测试数据。
        /// </summary>
        /// <param name="dsParams">包含IV测试数据的数据集对象。</param>
        /// <returns>新增IV测试数据的个数。</returns>
        public int InsertIVTestData(DataSet dsParams)
        {
            int count = 0;
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                count = iServerObjFactory.Get<IIVTestDataTransferEngine>().InsertIVTestData(dsParams);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return count;
        }
    }
}

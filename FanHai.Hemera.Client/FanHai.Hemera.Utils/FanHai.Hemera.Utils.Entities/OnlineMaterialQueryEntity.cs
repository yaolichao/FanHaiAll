//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  乔永明
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 乔永明               2012-02-16             新建 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface.WarehouseManagement;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 在线物料查询的实体类。
    /// </summary>
    public class OnlineMaterialQueryEntity:EntityObject
    {
        private string _errorMsg = "";
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
        }
        /// <summary>
        /// 查询在线物料信息。
        /// </summary>
        /// <param name="model">
        /// 包含查询条件对象。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含在线物料信息的数据集。</returns>
        public DataSet Query(OnlineMaterialQueryModel model, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIOnlineMaterialQueryEngine().Query(model, ref pconfig);
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
        /// 查询在线物料明细信息。
        /// </summary>
        /// <param name="model">
        /// 包含查询条件对象。
        /// </param>
        /// <param name="storeMaterialKey">
        /// 在线物料主键
        /// </param>
        /// <returns>包含在线物料信息的数据集。</returns>
        public DataSet QueryDetail(OnlineMaterialQueryModel model,string storeMaterialKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIOnlineMaterialQueryEngine().QueryDetail(model, storeMaterialKey);
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

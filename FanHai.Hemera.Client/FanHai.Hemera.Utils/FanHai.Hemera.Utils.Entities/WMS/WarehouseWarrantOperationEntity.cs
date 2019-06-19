//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// zhangjf              2013-09-18            新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using System.Data;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 入库单管理操作类
    /// </summary>
    public class WarehouseWarrantOperationEntity
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
        /// 获取特性名称
        /// </summary>
        /// <param name="ZMMTYP"></param>
        /// <returns></returns>
        public DataSet GetATNAM(String ZMMTYP)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IWarehouseWarrantOperationEngine>().GetATNAM(ZMMTYP);
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
        /// 返回托盘明细数据
        /// </summary>
        /// <param name="PALNO"></param>
        /// <returns></returns>
        public DataSet GetWorkItems(String PALNO)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IWarehouseWarrantOperationEngine>().GetWorkItems(PALNO);
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
        /// 保存入库单
        /// </summary>
        /// <param name="dsHead"></param>
        /// <param name="dsItem"></param>
        /// <returns></returns>
        public int SaveWarehouseWarrant(DataTable dtHead, DataTable dtItem, out string returnInfo)
        {
            int returnVal = 0;
            returnInfo = String.Empty;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                returnVal = serverFactory.Get<IWarehouseWarrantOperationEngine>().SaveWarehouseWarrant(dtHead, dtItem, out returnInfo);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return returnVal; 
        }

        /// <summary>
        /// 查询入库单记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IWarehouseWarrantOperationEngine>().QueryWarehouseWarrant(dsParams, ref config);
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

        public DataSet QueryWarehouseWarrantItems(string ZMBLNR)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IWarehouseWarrantOperationEngine>().QueryWarehouseWarrantItems(ZMBLNR);
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

        public DataSet QueryWarehouseWarrantHead(string ZMBLNR, string ISSYN, bool isQueryForSyn)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IWarehouseWarrantOperationEngine>().QueryWarehouseWarrantHead(ZMBLNR, ISSYN, isQueryForSyn);
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
        /// 入库单增加删除标记
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        public bool AddLvorm(string ZMBLNR)
        {
            bool isExists = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                isExists = serverFactory.Get<IWarehouseWarrantOperationEngine>().AddLvorm(ZMBLNR);
            }
            catch (Exception ex)
            {
                return isExists;
            }
            return isExists;
        }

        /// <summary>
        /// 同步SAP
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <param name="SYNMAN"></param>
        /// <param name="returnStr"></param>
        /// <returns></returns>
        public bool SynSAP(string ZMBLNR, string SYNMAN, out string returnStr)
        {
            returnStr = String.Empty;
            bool isSuccess = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                isSuccess = serverFactory.Get<IWarehouseWarrantOperationEngine>().SynSAP(ZMBLNR, SYNMAN, out returnStr);
            }
            catch (Exception ex)
            {
                returnStr = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return isSuccess; 
        }

    }
}

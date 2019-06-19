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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 批次查询实体类。
    /// </summary>
    public class LotQueryEntity
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
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig pconfig)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SearchLotList(dsParams, ref pconfig);
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
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        public DataSet Query(DataSet dsParams, ref PagingConfig pconfig)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SearchLotList(dsParams,ref pconfig);
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
        /// 根据批次号查询批次信息。
        /// </summary>
        /// <param name="lotNo">批次号或批次主键。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        public DataSet GetLotInfo(string lotNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetLotInfo(lotNo);
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
        /// 为批次号打印获取批次信息。
        /// </summary>
        /// <param name="dtParams">包含查询条件的数据表。
        /// 键值对数据集。键值<see cref="POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER"/>,
        /// <see cref="POR_LOT_FIELDS.FIELD_IS_PRINT"/>,<see cref="POR_LOT_FIELDS.FIELD_IS_REWORKED"/>
        /// </param>
        /// <returns>包含批次信息的数据集。
        /// [LOT_KEY,LOT_NUMBER,IS_PRINT,REWORK_FLAG,QUANTITY,ORDER_NUMBER]
        /// </returns>
        public DataSet GetLotNumberForPrint(DataTable dtParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverobj = CallRemotingService.GetRemoteObject();
                dsReturn = serverobj.CreateILotEngine().GetLotNumberForPrint(dtParams);
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                this._errorMsg = msg;
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
        /// 根据批次号查询未删除、未完成、未终结的批次数据。
        /// </summary>
        /// <param name="lotNo">左右匹配模糊查询。</param>
        /// <param name="lotNo">工厂车间主键。</param>
        /// <returns>包含批次数据的数据集对象。</returns>
        public DataSet QueryUsingLotData(string lotNo, string roomKey)
        {
            Hashtable htParams = new Hashtable();
            if (!string.IsNullOrEmpty(lotNo))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNo);
            }
            htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, "0");
            htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "N");
            if (!string.IsNullOrEmpty(roomKey))
            {
                htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, roomKey);
            }
            DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            DataSet dsParams = new DataSet();
            dsParams.Merge(dtParams, false, MissingSchemaAction.Add);

            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SearchLotList(dsParams);
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
        /// 根据批次主键获取批次的历史操作记录
        /// </summary>
        /// <param name="lotKEY">批次主键。</param>
        /// <returns>
        /// 查询得到的包含批次操作信息的数据集对象。
        /// </returns>
        public DataSet GetInfoForLotHistory(string lotKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetInfoForLotHistory(lotKey);
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
        /// 根据批次主键获取批次报废不良信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次报废不良信息的数据集。
        /// [LOT_NUMBER,ROUTE_OPERATION_NAME,SCRAP_QUANTITY,DEFECT_QUANTITY]
        /// </returns>
        public DataSet GetScrapAndDefectQty(string lotKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetScrapAndDefectQty(lotKey);
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
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        public DataSet Query(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SearchLotList(dsParams);
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
        /// 根据批次主键获取批次工序参数信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次工序参数信息的数据集。
        /// </returns>
        public DataSet GetParamInfo(string lotKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetParamInfo(lotKey);
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
            return dsReturn;
        }

        /// <summary>
        /// 根据厂别获取组件录入卡控信息
        /// </summary>
        /// <param name="sFactoryName">厂别。</param>
        /// <returns>
        /// 根据厂别获取组件录入卡控信息
        /// </returns>
        public DataSet GetCheckbarcodeInputType(string sFactoryName)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetCheckbarcodeInputType(sFactoryName);
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
            return dsReturn;
        }

        /// <summary>
        /// 根据组件序列号获取组件对应的铭牌检查类型
        /// </summary>
        /// <param name="LotNo">组件序列号</param>
        /// <returns>组件对应的检查类型的数据集</returns>
        public DataSet GetLotCustCheckType(string lotNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetLotCustCheckType(lotNo);
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
            return dsReturn;
        }

        /// <summary>
        /// 根据托盘号获取批次信息。
        /// </summary>
        /// <param name="pallet_no">托盘号</param>
        /// <returns>包含批次信息的数据集</returns>
        public DataSet GetLotInfoByPallet_No(string pallet_no)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetLotInfoByPallet_No(pallet_no);
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
            return dsReturn;
        }
    }
}

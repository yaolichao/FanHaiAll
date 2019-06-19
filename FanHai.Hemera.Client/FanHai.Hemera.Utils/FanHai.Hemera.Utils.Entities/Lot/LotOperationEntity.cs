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
    /// 批次操作实体类。
    /// </summary>
    public class LotOperationEntity
    {
        public DataSet dsEquipments; 
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        public LotOperationEntity()
        {

        }

        public LotOperationEntity(DataSet ds)
        {
            dsEquipments = ds;
        }
        /// <summary>
        /// 获取可选问题工序。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        public DataSet GetTroubleStepInfo(string lotKey, string operations)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetTroubleStepInfo(lotKey,  operations);
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
        /// 获取电池片回收的可选问题工序。
        /// </summary>
        /// <param name="lotKey">回收电池片的批次主键。。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        public DataSet GetRecoveredTroubleStepInfo(string lotKey, string operations)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetRecoveredTroubleStepInfo(lotKey, operations);
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
        /// 根据原因代码类别获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        public DataSet GetReasonCode(string categoryKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetReasonCode(categoryKey);
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
        /// 根据原因分组主键获取原因分类。
        /// </summary>
        /// <param name="categoryKey">原因分组主键。</param>
        /// <returns>包含原因分类的数据集对象。</returns>
        public DataSet GetReasonCodeClass(string categoryKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetReasonCodeClass(categoryKey);
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
        /// 根据原因代码类别获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码类别主键。</param>
        /// <param name="codeClass">原因代码分类。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        public DataSet GetReasonCode(string categoryKey,string codeClass)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetReasonCode(categoryKey, codeClass);
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
        /// 获取默认的超时原因代码数据。
        /// </summary>
        /// <returns>
        /// 包含超时原因代码数据的数据集。
        /// </returns>
        public DataSet GetDelayReasonCode()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIReasonCodeEngine().GetReasonCodeForDelay(string.Empty);
                this._errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 获取默认的退料原因代码数据。
        /// </summary>
        /// <returns>
        /// 包含退料原因代码数据的数据集。
        /// </returns>
        public DataSet GetReturnMaterialReasonCode()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIReasonCodeEngine().GetReasonCodeForReturnMaterial(string.Empty);
                this._errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 获取默认的回收原因代码数据。
        /// </summary>
        /// <returns>
        /// 包含回收原因代码数据的数据集。
        /// </returns>
        public DataSet GetRecoverReasonCode()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIReasonCodeEngine().GetReasonCodeForRecover(string.Empty);
                this._errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 根据工步主键获取退料原因代码。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含退料原因代码数据的数据集对象。</returns>
        public DataSet GetReturnMaterialReasonCode(string stepKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetReturnMaterialReasonCode(stepKey);
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
        /// 获取暂停原因代码类别。
        /// </summary>
        /// <returns>包含暂停原因代码类别的数据集对象。</returns>
        public DataSet GetHoldReasonCodeCategory() 
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetHoldReasonCodeCategory();
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
        /// 获取待补片的批次信息。
        /// </summary>
        /// <param name="workorderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="efficiency">转换效率。</param>
        /// <returns>包含待补片批次信息的数据集对象。</returns>
        public DataSet GetPatchedLotNumber(string workorderNo, string proId, string efficiency)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPatchedLotNumber(workorderNo,proId,efficiency);
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
        /// 获取待回收的批次信息（包含电池片报废和被电池片补片的批次数据）。
        /// </summary>
        /// <param name="recoverdLotKey">回收批次主键。</param>
        /// <param name="operationKey">问题工序主键。</param>
        /// <returns>包含待回收批次信息的数据集对象。</returns>
        public DataSet GetBeRecoverdLotNumber(string recoverdLotKey, string operationKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                dsReturn = serverFactory.Get<ILotOperationEngine>().GetBeRecoverdLotNumber(recoverdLotKey, operationKey);
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
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.Get<ILotOperationEngine>().GetProdId();
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
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。</returns>
        public DataSet GetEfficiency()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetEfficiency();
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
        /// 获取批次暂停信息。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次暂停信息的数据集对象。</returns>
        public DataSet GetLotHoldInfo(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetLotHoldInfo(lotNumber);
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
        /// 根据批次主键获取批次进站是否超时。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// true表示超时，false表示没有超时。
        /// </returns>
        public bool GetLotTrackInIsDelay(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                bool isDelay = serverFactory.Get<ILotOperationEngine>().GetLotTrackInIsDelay(lotKey);
                return isDelay;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 根据批次主键获取批次出站的时间控制数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次时间控制数据的数据集对象。数据集中包含两个数据表对象。
        /// 有一个名称为TrackOutStatus的数据表对象，数据表的列名为
        /// TimeStatusFlag（时间状态，0：加工时间没有满足最小加工时间。
        /// 1:加工时间满足最小时间，没有超过报警时间。
        /// 2:加工时间超过报警时间，没有超过最大加工时间。
        /// 3:加工时间超过最大加工时间。） 
        /// TimeControlBaseSubMin（基础加工时间-最小加工时间）。
        /// </returns>
        public DataSet GetTrackOutTimeControlStatus(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetTrackOutTimeControlStatus(lotKey);
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
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        public DataSet GetPalletInfo(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPalletInfo(palletNo);
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
        /// 获取托盘上的批次信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含批次信息的数据表。</returns>
        public DataSet GetPalletLotInfo(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPalletLotInfo(palletNo);
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
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        public bool CheckCalibrationCycle(string lotNumber,string proId, string deviceCode)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckCalibrationCycle(lotNumber,proId, deviceCode);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        public bool CheckCalibrationCycle(string proId, string deviceCode)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckCalibrationCycle(proId,deviceCode);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        public bool CheckFixCycle(string lotNumber,string lineKey, string proId, DateTime waitTrackInTime)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckFixCycle(lotNumber,lineKey, proId, waitTrackInTime);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 检查是否超过恒温时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过恒温周期。false:没有超过恒温周期。</returns>
        public bool CheckConstantTemperatureCycle(string lotNumber, string lineKey, string proId, DateTime waitTrackInTime)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckConstantTemperatureCycle(lotNumber, lineKey, proId, waitTrackInTime);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        public bool CheckFixCycle(string lineKey, string proId, DateTime waitTrackInTime)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckFixCycle(lineKey, proId, waitTrackInTime);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 工单在过站时检查花色是否必须输入。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>true：需要检查花色。false：不用检查花色。</returns>
        public bool IsCheckColorByWorkOrder(string workOrderNo)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().IsCheckColorByWorkOrder(workOrderNo);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 检查IV测试数据是否存在。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="trackInTime">进站时间。</param>
        /// <returns>false:不存在。true：存在。</returns>
        public bool CheckIVTestData(string lotNumber, DateTime trackInTime)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckIVTestData(lotNumber, trackInTime);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
        /// <summary>
        /// 获取组件有效的IV测试数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>组件测试有效的测试时间。</returns>
        public DateTime GetLotValidIVTestTime(string lotNumber)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().GetLotValidIVTestTime(lotNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return DateTime.MinValue;
        }
        /// <summary>
        /// 获取批次最后一笔操作记录。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次最后一笔操作记录的数据集对象。</returns>
        public DataSet GetLotLastestActivity(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetLotLastestActivity(lotNumber);
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
        /// （电池片/组件）报废操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <param name="dsParams">包含报废信息的数据集对象。</param>
        public void LotScrap(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotScrap(dsParams);
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
        /// （电池片/组件）不良操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <param name="dsParams">包含不良信息的数据集对象。</param>
        public void LotDefect(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotDefect(dsParams);
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
        /// 电池片补片操作。
        /// </summary>
        /// <param name="dsParams">包含补片信息的数据集对象。</param>
        public void LotPatch(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotPatch(dsParams);
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
        /// 电池片回收操作。用于撤销电池片报废和电池片补片。
        /// </summary>
        /// <param name="dsParams">包含回收信息的数据集对象。</param>
        public void LotRecovered(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotRecovered(dsParams);
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
        /// 调整批次操作。
        /// </summary>
        /// <param name="dsParams">包含批次调整信息的数据集对象。</param>
        public void LotAdjust(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotAdjust(dsParams);
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
        /// 暂停批次操作。
        /// </summary>
        /// <param name="dsParams">包含批次暂停信息的数据集对象。</param>
        public void LotHold(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotHold(dsParams);
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
        /// 批次线别调整操作。
        /// </summary>
        /// <param name="dsParams">包含批次线别调整信息的数据集对象。</param>
        public void LotExchangeLine(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotExchangeLine(dsParams);
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
        /// 释放批次操作。
        /// </summary>
        /// <param name="dsParams">包含批次释放信息的数据集对象。</param>
        public void LotRelease(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotRelease(dsParams);
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
        /// 返工/返修批次操作。
        /// </summary>
        /// <param name="dsParams">包含批次（返工/返修）信息的数据集对象。</param>
        public void LotRework(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotRework(dsParams);
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
        /// 退料操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL"/>。
        /// </remarks>
        /// <param name="dsParams">包含退料信息的数据集对象。</param>
        public void LotReturnMaterial(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotReturnMaterial(dsParams);
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
        /// 获取指定工步主键进站后是否返回主界面。
        /// </summary>
        /// <param name="stepKey">指定工步主键。</param>
        /// <returns>进站后是否返回主界面。默认为true返回主界面。</returns>
        public bool GetTrackInIsReturnMainView(string stepKey)
        {
            bool isReturnMainview = true;
            try
            {
                const string TRACKIN_RETURN_MAINVIEW = "TrackInReturnMainView";
                _errorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                string obj=factor.CreateIRouteEngine().GetStepUdaValue(stepKey, TRACKIN_RETURN_MAINVIEW);
                if (!bool.TryParse(obj, out isReturnMainview))
                {
                    isReturnMainview = true;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                LoggingService.Error(this.ErrorMsg, ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return isReturnMainview;
        }
        /// <summary>
        /// 批次批量进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次进站信息的数据集对象。</param>
        /// <returns>0:进站成功。1:记录过期。2:需要抽检。-1:进站失败。</returns>
        public int LotBatchTrackIn(IList<DataSet> lstTrackInData)
        {
            int code = -1;
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotBatchTrackIn(lstTrackInData);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return code;
        }

        /// <summary>
        /// 批次进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次进站信息的数据集对象。</param>
        /// <returns>0:进站成功。1:记录过期。2:需要抽检。-1:进站失败。</returns>
        public int LotTrackIn(DataSet dsParams)
        {
            int code = -1;
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotTrackIn(dsParams);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn,ref code);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return code;
        }
        /// <summary>
        /// 批次批量出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// 上料参数信息，flag是否设定自动上料
        /// </remarks>
        /// <param name="dsParams">包含批次出站信息的数据集对象。</param>
        public void LotBatchTrackOut(IList<DataSet> lstTrackInData,DataTable dtStepParam,int flag)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotBatchTrackOut(lstTrackInData, dtStepParam,flag);
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
        /// 批次出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次出站信息的数据集对象。</param>
        public void LotTrackOut(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotTrackOut(dsParams);
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
        /// 批次结束操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次结束信息的数据集对象。</param>
        public void LotTerminal(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotTerminal(dsParams);
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
        /// 托盘进站作业。
        /// </summary>
        /// <param name="dsParams">包含托盘入库信息的数据集对象。</param>
        public void PalletTrackIn(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().PalletTrackIn(dsParams);
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
        /// 托盘出站作业。
        /// </summary>
        /// <param name="dsParams">包含托盘入库信息的数据集对象。</param>
        public void PalletTrackOut(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().PalletTrackOut(dsParams);
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
        /// 托盘入库作业。
        /// </summary>
        /// <param name="dsParams">包含托盘入库信息的数据集对象。</param>
        public void PalletToWarehouse(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().PalletToWarehouse(dsParams);
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
        /// 批次撤销操作。
        /// </summary>
        /// <param name="dsParams">包含撤销操作信息的数据集对象。</param>
        /// <returns>
        /// 包含结果数据的数据集对象。
        /// </returns>
        public void LotUndo(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotUndo(dsParams);
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
        /// 打印标签后自动进站或出站。
        /// </summary>
        /// <param name="lotNumber">组件序列号。</param>
        /// <param name="sDeviceNum">设备代码。</param>
        /// <returns>true：成功。false：失败。</returns>
        public bool LotTrackInAndLotTrackOut(string lotNumber, string equipmentCode)
        {
           
            //根据设备编码获取设备及其所在工序信息。
            //EquipmentEntity equipmentEntity = new EquipmentEntity();
            //DataSet dsEquipments = equipmentEntity.GetEquipments(equipmentCode);
            //if (!string.IsNullOrEmpty(equipmentEntity.ErrorMsg))
            //{
            //    this._errorMsg = equipmentEntity.ErrorMsg;
            //    return true;
            //}
            DataRow[] drEquipments = dsEquipments.Tables[0].Select(string.Format("EQUIPMENT_CODE='{0}'", equipmentCode));
            if (null == dsEquipments || dsEquipments.Tables.Count <= 0 || dsEquipments.Tables[0].Rows.Count <= 0)
            {
                this._errorMsg = string.Format("设备编码{0}对应的设备不存在。",equipmentCode);
                return true;
            }
            //根据批次序列号获取批次信息。
            LotQueryEntity queryEntity = new LotQueryEntity();
            DataSet dsLotInfo = queryEntity.GetLotInfo(lotNumber);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                this._errorMsg = queryEntity.ErrorMsg;
                return true;
            }
            if (null == dsLotInfo || dsLotInfo.Tables.Count <= 0 || dsLotInfo.Tables[0].Rows.Count <= 0)
            {
                this._errorMsg = string.Format("序列号{0}对应的批次数据不存在。", lotNumber);
                return true;
            }

            DataRow drLotInfo = dsLotInfo.Tables[0].Rows[0];

            string partNumber = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PART_NUMBER]);
            string workOrderNo = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
            double qty = Convert.ToDouble(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
            double leftQty = qty;

            string workOrderKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
            string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
            string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
            string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
            string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);

            string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
            string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
            string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
            string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
            
            int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            //判断设备所在工序名称和批次当前所在工序的名称相同。
            DataRow[] drCompareEquipments = dsEquipments.Tables[0]
                                                 .Select(string.Format("{0}='{1}'", "ROUTE_OPERATION_NAME", stepName));
            //设备所在工序名称和批次当前所在工序的名称不相同。
            if (drCompareEquipments.Length <= 0)
            {
                this._errorMsg = "设备所在工序名称和批次当前所在工序的名称不相同。";
                return false;
            }
            string curEquipmentKey = Convert.ToString(drEquipments[0][EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
            string curLineKey = Convert.ToString(drEquipments[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY]);
            string curLineName = Convert.ToString(drEquipments[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME]);
            string operationKey = Convert.ToString(drEquipments[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            //获取班别。
            Shift shift = new Shift();
            string shiftName = shift.GetCurrShiftName();
            string shiftKey = string.Empty;
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            //判断批次状态
            //如果是等待进站（STATE_FLAG== LotStateFlag.WaitintForTrackIn）
            if (stateFlag == 0)
            {
                DataSet dsParams = new DataSet();
                //组织操作数据。
                #region 组织操作数据。
                Hashtable htTransaction = new Hashtable();
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, userName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, curLineKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, curLineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, curEquipmentKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, userName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtTransaction);
                #endregion
                //组织其他附加参数数据
                #region 组织其他附加参数数据
                Hashtable htMaindata = new Hashtable();
                htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
                htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, 0);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNumber);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, workOrderNo);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
                DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                dsParams.Tables.Add(dtParams);
                #endregion
                //进站操作
                int code = this.LotTrackIn(dsParams);
                //如果进站失败，记录错误消息
                if (!string.IsNullOrEmpty(this.ErrorMsg))
                {
                    return false;
                }
                #region //重新获取批次信息
                //根据批次序列号获取批次信息。
                dsLotInfo = queryEntity.GetLotInfo(lotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    this._errorMsg = queryEntity.ErrorMsg;
                    return false;
                }
                if (null == dsLotInfo || dsLotInfo.Tables.Count <= 0 || dsLotInfo.Tables[0].Rows.Count <= 0)
                {
                    this._errorMsg = string.Format("序列号{0}对应的数据不存在。", lotNumber);
                    return false;
                }
                drLotInfo = dsLotInfo.Tables[0].Rows[0];
                #endregion

                stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
            }
            //判断批次状态，如果是等待进站（STATE_FLAG== LotStateFlag.WaitintForTrackOut）
            if (stateFlag == 9)
            {
                DataSet dsParams = new DataSet();

                #region 获取下一个工步数据。
                string toEnterpriseKey = enterpriseKey;
                string toRouteKey = routeKey;
                string toStepKey = stepKey;
                string toEnterpriseName = enterpriseName;
                string toRouteName = routeName;
                string toStepName = stepName;
                //获取下一个工步数据。
                RouteQueryEntity routeQueryEntity = new RouteQueryEntity();
                DataSet dsRouteNextStep = routeQueryEntity.GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                if (!string.IsNullOrEmpty(routeQueryEntity.ErrorMsg))
                {
                    this._errorMsg = routeQueryEntity.ErrorMsg;
                    return false;
                }
                if (null != dsRouteNextStep
                    && dsRouteNextStep.Tables.Count > 0
                    && dsRouteNextStep.Tables[0].Rows.Count > 0)
                {
                    DataRow drRouteNextStep = dsRouteNextStep.Tables[0].Rows[0];
                    toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                    toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                    toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                    toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                    toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                    toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                }

                Hashtable htStepTransaction = new Hashtable();
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, timezone);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, userName);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtStepTransaction);
                #endregion
                //组织操作数据。
                #region 操作数据
                Hashtable htTransaction = new Hashtable();
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, userName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, curLineKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, curLineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, curEquipmentKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, curLineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, userName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtTransaction);
                #endregion
                //组织其他附加参数数据
                #region 其他附加参数数据
                Hashtable htMaindata = new Hashtable();
                htMaindata.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
                htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, 0);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, lotNumber);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, workOrderNo);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
                htMaindata.Add(ROUTE_OPERATION_ATTRIBUTE.IsShowSetNewRoute, false);
                DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                dsParams.Tables.Add(dtParams);
                #endregion
                //出站操作
                this.LotTrackOut(dsParams);
                //如果出站失败，记录错误消息
                if (!string.IsNullOrEmpty(this.ErrorMsg))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 包装作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotPackage(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotPackage(dsParams);
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
        /// 根据托盘号获取包装数据。
        /// </summary>
        /// <returns></returns>
        public DataSet GetPackageData(string packageNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPackageData(packageNo);
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
        /// 根据批次号获取包装数据。
        /// </summary>
        /// <returns></returns>
        public DataSet GetPackageDataByLotNo(string val)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPackageDataByLotNo(val);
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
        /// 获取包装批次数据。
        /// </summary>
        /// <param name="htParams">
        /// 允许参数使用的键值。
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_SIDECODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY"/>
        /// </param>
        /// <returns>包含包装批次数据的数据集对象。</returns>
        public DataSet GetPackageLotInfo(Hashtable htParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPackageLotInfo(htParams);
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
        /// 根据批次号获取满包数量。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>满包数量。</returns>
        public int GetPackageFullQty(string lotNumber)
        {
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().GetPackageFullQty(lotNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return 0;
        }

        /// <summary>
        /// 获取包装批次对应产品的平均功率控制数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含包装批次对应产品的平均功率控制数据的数据集对象。</returns>
        public DataSet GetPackageAvgPowerRangeData(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPackageAvgPowerRangeData(lotNumber);
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
        /// 根据批次号获取包装混包规则数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含混包规则数据的数据集对象。</returns>
        public DataSet GetPackageMixRule(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPackageMixRule(lotNumber);
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
        /// 获取批次功率分档和子分档数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次功率分档和子分档数据的数据集对象。</returns>
        public DataSet GetLotPowersetData(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetLotPowersetData(lotNumber);
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
        /// 根据等级代码获取等级名称。
        /// </summary>
        /// <param name="gradeCode">等级代码。</param>
        /// <returns>等级名称。</returns>
        public string GetGradeName(string proLevel)
        {
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().GetGradeName(proLevel);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return proLevel;
        }

        /// <summary>
        ///  根据工单号判断是否为安能单 yibin.fei 2017.11.23
        /// </summary>
        /// <param name="order_Number"></param>
        /// <returns></returns>
        public DataSet GetAnNeng (string  order_Number)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotCreateEngine>().GetAnNeng(order_Number);
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
        /// 根据分档主键获取分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="psKey">分档主键。</param>
        /// <returns>包含分档数据的数据集对象。</returns>
        public DataSet GetWOProductPowersetData(string orderNumber, string partNumber, string psKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetWOProductPowersetData(orderNumber, partNumber, psKey);
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
        /// 批次拆包作业。
        /// </summary>
        /// <param name="dsParams">包含包装数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet LotUnpack(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotUnpack(dsParams);
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
        /// 根据工序名称获取工序主键。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>工序主键。</returns>
        public string GetOperationKey(string operationName)
        {
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IOperationEngine>().GetMaxVersionOperationBaseInfo(operationName);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

                if (string.IsNullOrEmpty(_errorMsg)
                    && dsReturn.Tables[0].Rows.Count > 0)
                {
                    string operationKey = Convert.ToString(dsReturn.Tables[0].Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
                    return operationKey;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return string.Empty;
        }
        /// <summary>
        /// 批次入库检验作业。
        /// </summary>
        /// <param name="dsParams">包含包装检验数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet LotToWarehouseCheck(DataSet dsParam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotToWarehouseCheck(dsParam);
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
        /// 检查流程卡。
        /// </summary>
        /// <returns>true:成功;false:失败。</returns>
        public bool CheckProcessCard(string lotNumber)
        {
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CheckProcessCard(lotNumber);
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
        }
        /// <summary>
        /// 获取工单产品数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单及其产品数据的数据集对象。</returns>
        public DataSet GetWoProductData(string orderNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetWoProductData(orderNo);
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
        /// 比对等级是否符合工单和产品的要求。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>true：符合;false:不符合。</returns>
        public bool CompareExchangeGrade(string newOrderNumber, string newPartNumber, string grade)
        {
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().CompareExchangeGrade(newOrderNumber, newPartNumber,grade);
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
        }
        /// <summary>
        /// 根据托盘号和批次号获取返工数据。
        /// </summary>
        /// <remarks>
        /// 只输入托盘号，根据托盘号获取返工明细数据。
        /// 只输入批次号，根据批次号获取返工明细数据。
        /// 同时输入托盘号和批次号，获取返工明细数据。
        /// </remarks>
        /// <returns>包含返工明细数据的数据集对象。</returns>
        public DataSet GetExchangeData(string packageNo, string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetExchangeData(packageNo,lotNo);
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
        /// 获取组件对应的工艺流程中组件所在工序的属性信息
        /// </summary>
        /// <param name="lotNo">批次号</param>
        /// <returns>工序对应的属性信息</returns>
        public DataSet GetLotRouteAttrData(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetLotRouteAttrData(lotNo);
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
        /// 根据工单号、产品料号和功率获取对应的产品衰减系数。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="pm">功率。</param>
        /// <returns>包含工单产品衰减系数数据的数据集对象。</returns>
        public DataSet GetDecayCoefficient(string orderNumber, string partNumber, decimal pm)
        {
            IVTestDataEntity _testDataEntity = new IVTestDataEntity();
            DataSet dsReturn=_testDataEntity.GetDecayCoefficient(orderNumber, partNumber, pm);
            this._errorMsg = _testDataEntity.ErrorMsg;
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号、产品料号和衰减后功率获取对应的分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="lotNo">组件批次号。</param>
        /// <param name="coefPM">衰减后功率。</param>
        /// <returns>包含对应分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetData(string newOrderNumber, string newPartNumber, string lotNumber, decimal coefPM)
        {
            IVTestDataEntity _testDataEntity = new IVTestDataEntity();
            DataSet dsReturn = _testDataEntity.GetWOPowerSetData(newOrderNumber, newPartNumber, lotNumber, coefPM);
            this._errorMsg = _testDataEntity.ErrorMsg;
            return dsReturn;
        }
        /// <summary>
        /// 返工单作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotExchange(DataSet dsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotExchange(dsParams);
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
        /// 根据工单主键、产品料号、分档主键和衰减后数据获取对应的子分档数据。
        /// </summary>
        /// <param name="workOrderKy">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <param name="val">根据子分档规则设置不同的值，如果是功率子分档设置为衰减后功率，如果是电流子分档设置为衰减后电流。</param>
        /// <returns>包含对应子分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetDetailData(string workOrderKy, string partNumber, string powersetKey, decimal val)
        {
            IVTestDataEntity _testDataEntity = new IVTestDataEntity();
            DataSet dsReturn = _testDataEntity.GetWOPowerSetDetailData(workOrderKy, partNumber, powersetKey, val);
            this._errorMsg = _testDataEntity.ErrorMsg;
            return dsReturn;
        }
        /// <summary>
        /// 入库检验返回到上一工序作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        public DataSet LotToWarehouseCheckReject(DataSet dsParam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().LotToWarehouseCheckReject(dsParam);
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
        /// 是否检查组件的IV测试数据。
        /// </summary>
        /// <param name="lotNumber">组件序列号。</param>
        /// <returns>true：检查。false：不检查。</returns>
        public bool IsCheckIVTestData(string lotNumber)
        {
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().IsCheckIVTestData(lotNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }
        /// <summary>
        /// 获取工单检验允许的等级代码。符合的等级在终检将不需要输入不良和备注。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>工单检验允许的等级代码。多个等级使用逗号（,）分隔。</returns>
        public string GetWOCheckAllowGrade(string lotNumber,out string gradeName)
        {
            gradeName = string.Empty;
            _errorMsg = string.Empty;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().GetWOCheckAllowGrade(lotNumber,out gradeName);
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
        /// 自动生成新的托盘号。
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>数据集对象</returns>
        public DataSet NewPalletNo(string lotNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().NewPalletNo(lotNum);
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
        /// 自动生成托盘号
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        public string GetNewPalletNum(string lotNum)
        {
            string palletNo = string.Empty;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                palletNo= serverFactory.Get<ILotOperationEngine>().GetNewPalletNum(lotNum);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return palletNo;
        }
        /// <summary>
        /// 判断托盘号是否已存在。yibin.fei 2018.03.13
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        public bool SurePalletNo(string PalletNo)
        {
           
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().SurePalletNo(PalletNo);
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
        }
        /// <summary>
        /// 获取托盘状态。
        /// </summary>
        /// <param name="palletNoForCheck">托盘号</param>
        /// <returns>数据集对象</returns>
        public DataSet GetPalletStatus(string palletNoForCheck)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetPalletStatus(palletNoForCheck);
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
        /// 判定是否为SE工单
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>true false</returns>
        public bool GetOutLotForSe(string lotNum)
        {
            try
            {
                _errorMsg = string.Empty;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<ILotOperationEngine>().GetOutLotForSe(lotNum);
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

        }
        /// <summary>
        /// SE 客户自动生产托号规则
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns></returns>
        public DataSet NewSEPalletNo(string lotNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().NewSEPalletNo(lotNum);
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
        /// 根据工单主键，物料代码
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料代码</param>
        /// <returns>数据集</returns>
        public DataSet GetWorkOrderBomByMat(string workOrderKey,string mat)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetWorkOrderBomByMat(workOrderKey,mat);
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

        public DataSet GetStepParams(string stepKey, string roomKey, string equipmentKey, string operationName, string workorder, OperationParamDCType dcType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetStepParams(stepKey, roomKey, equipmentKey, operationName, workorder, (int)dcType);
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
        /// 根据参数名称获取参数组信息
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <returns></returns>
        public DataSet GetParamerTeamName(string paramName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetParamerTeamName(paramName);
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
        /// 根据工单主键和mat物料类型获取替代料信息和工单bom信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料类型</param>
        /// <returns></returns>
        public DataSet GetWorkOrderBomByWorkKeyAndMat(string workOrderKey, string mat)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetWorkOrderBomByWorkKeyAndMat(workOrderKey, mat);
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
        /// 根据工单主键和物料类型获取特殊物料数据
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料类型</param>
        /// <returns></returns>
        public DataSet GetSpecailBomByWorkKeyAndMat(string workOrderKey, string mat)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<ILotOperationEngine>().GetSpecailBomByWorkKeyAndMat(workOrderKey, mat);
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
        /// 更新POR_LOT表新工单号字段
        /// </summary>
        /// <param name="newWorkOrder"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        public bool UpdatePor_Lot(string newWorkOrder, string Pallet_No)
        {
            bool bReturn = false;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                bReturn = serverFactory.Get<ILotOperationEngine>().UpdatePor_Lot(newWorkOrder, Pallet_No);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }

        /// <summary>
        /// 更新Wip_consignment表新工单号字段
        /// </summary>
        /// <param name="INWHORDER"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        public bool UpdateWip_consignment(string INWHORDER, string Pallet_No)
        {
            bool bReturn = false;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                bReturn = serverFactory.Get<ILotOperationEngine>().UpdateWip_consignment(INWHORDER, Pallet_No);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }
    }
}

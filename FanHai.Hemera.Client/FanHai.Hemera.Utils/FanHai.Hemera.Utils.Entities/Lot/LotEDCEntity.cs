using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;
namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 批次数据采集的实体类。
    /// </summary>
    public class LotEDCEntity
    {
        private string _lotKey = "";
        private string _lotNumber = "";
        private string _enterpriseKey = "";
        private string _stepKey = "";
        private string _routeKey = "";
        private string _workOrderKey = "";
        private string _partKey = "";
        private string _edcKey = "";
        private string _samplingKey = "";
        private string _edcInsKey = "";
        private string _edcName = "";
        private string _spCount = "";
        private string _errorMsg = "";
        private string _spUnits = "";
        private string _operator = "";
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return this._errorMsg; }
        }
        public string SpUnits
        {
            get { return this._spUnits; }
            set { this._spUnits = value; }
        }
        public string SpCount
        {
            get { return this._spCount; }
            set { this._spCount = value; }
        }
        /// <summary>
        /// 管控数据组名称。
        /// </summary>
        public string EdcName
        {
            get { return this._edcName; }
            set { this._edcName = value; }
        }
        /// <summary>
        /// 批次主键。
        /// </summary>
        public string LotKey
        {
            get { return this._lotKey; }
            set { this._lotKey = value; }
        }
        /// <summary>
        /// 工单主键。
        /// </summary>
        public string WorkOrderKey
        {
            get { return this._workOrderKey; }
            set { this._workOrderKey = value; }
        }
        /// <summary>
        /// 批次号。
        /// </summary>
        public string LotNumber
        {
            get { return this._lotNumber; }
            set { this._lotNumber = value; }
        }
        /// <summary>
        /// 工步主键。
        /// </summary>
        public string StepKey
        {
            get { return this._stepKey; }
            set { this._stepKey = value; }
        }
        /// <summary>
        /// 流程主键。
        /// </summary>
        public string RouteKey
        {
            get { return this._routeKey; }
            set { this._routeKey = value; }
        }
        /// <summary>
        /// 流程组主键。
        /// </summary>
        public string EnterpriseKey
        {
            get { return this._enterpriseKey; }
            set { this._enterpriseKey = value; }
        }
        /// <summary>
        /// 成品主键。
        /// </summary>
        public string PartKey
        {
            get { return this._partKey; }
            set { this._partKey = value; }
        }
        /// <summary>
        /// 管控数据组主键。
        /// </summary>
        public string EdcKey
        {
            get { return this._edcKey; }
            set { this._edcKey = value; }
        }
        /// <summary>
        /// 抽样规则主键。
        /// </summary>
        public string SamplingKey
        {
            get { return this._samplingKey; }
            set { this._samplingKey = value; }
        }
        /// <summary>
        /// 数据采集实例主键。
        /// </summary>
        public string EdcInsKey
        {
            get { return this._edcInsKey; }
            set { this._edcInsKey = value; }
        }
        /// <summary>
        /// 操作人。
        /// </summary>
        public string Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }
        /// <summary>
        /// 根据抽样规则主键获取抽样规则信息。需要先设置属性<see cref="SamplingKey"/>的值。
        /// </summary>
        /// <returns>包含抽样规则信息的数据集。 </returns>
        public DataSet GetSamplingInfo()
        {
            DataSet dsReturn = new DataSet();
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(EDC_SP_FIELDS.FIELD_SP_KEY,_samplingKey);
            mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().GetSamplingInfo(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return dsReturn;
        }
        /// <summary>
        /// 根据管控数据组主键获取需要管控项信息。需要先设置属性<see cref="EdcKey"/>的值。
        /// </summary>
        /// <returns>
        /// 包含管控项数据的数据集。
        /// 【A.PARAM_KEY,A.PARAM_NAME,A.MANDATORY,A.ISDERIVED,A.DATA_TYPE,A.DEVICE_TYPE】
        /// </returns>
        public DataSet GetParams()
        {
            DataSet dsReturn = new DataSet();
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY,_edcKey);
            mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = TRANS_TABLES.TABLE_PARAM;
            dataSet.Tables.Add(mainDataTable);
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().GetParams(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return dsReturn;
        }
        /// <summary>
        /// 保存数据采集到得数据。
        /// </summary>
        /// <param name="dataset">包含到采集到的明细数据的数据集。</param>
        public void SaveEDCCollectionData(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().SaveEDCCollectionData(dataset);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        }
         /// <summary>
        /// 保存离线采集到的数据。
        /// </summary>
        /// <param name="dataset">包含管控项数据值的数据集。</param>
        public void SaveOfflineEDCData(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEDCEngine().SaveOfflineEDCData(dataset);
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
        /// 保存在线采集到的数据。
        /// </summary>
        /// <param name="dataset">包含管控项数据值的数据集。</param>
        /// <param name="otherParamCount">参数采集数的个数。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">数据采集实例主键。</param>
        /// <param name="isHold">是否HOLD批次。true：HOLD批次，false：不HOLD批次。</param>
        /// <param name="holdComment">HOLD原因，只有<see cref="isHold"/>=true时，才有意义。</param>
        public void SaveOnlineEDCData(DataSet dataset, int paramCount, string lotKey, string edcInsKey,
            bool isHold,string holdComment)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                if (isHold == true)
                {
                    DataTable mainDataTable = new DataTable();
                    Hashtable mainDataHashTable = new Hashtable();
                    Shift shift = new Shift();
                    string shiftName = shift.GetCurrShiftName();
                    string shiftKey = string.Empty;// shift.IsShiftValueExists(shiftName);
                    string operComputerName = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
                    mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, lotKey);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY, CommonUtils.GenerateNewKey(0));
                    mainDataHashTable.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_KEY, string.Empty);
                    mainDataHashTable.Add(WIP_HOLD_RELEASE_FIELDS.FIELD_REASON_CODE_NAME, string.Empty);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, holdComment);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));                   
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, _operator);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                    mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, operComputerName);
                    mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                    mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                    dataset.Tables.Add(mainDataTable);
                }

                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dataset.ExtendedProperties.Add(EDC_MAIN_INS_FIELDS.FIELD_EDITOR, _operator);
                    dsReturn = serverFactory.CreateIEDCEngine().SaveOnlineEDCData(dataset, paramCount, lotKey, edcInsKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        }
        /// <summary>
        /// 根据批次主键和数据采集主键获取采集的数据集合。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">数据采集主键。</param>
        /// <returns>
        /// 包含批次已采集的数据集合的数据集对象。
        /// [SP_UNIT_SEQ,PARAM_KEY,EDC_INS_KEY,PARAM_VALUE,COL_KEY,VALID_FLAG,
        /// FAILED_RULES,SP_SAMP_SEQ,EDITOR,EDIT_TIME,EDIT_TIMEZONE,DELETED_FLAG]
        /// </returns>
        public DataSet GetEDCCollectionData(string lotKey,string edcInsKey)
        {
            DataSet dsReturn = new DataSet();
            if (edcInsKey != "")
            {
                try
                {
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    if (null != serverFactory)
                    {
                        dsReturn = serverFactory.CreateIEDCEngine().GetEDCCollectionData(lotKey,edcInsKey);
                        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            }          

            return dsReturn;
        }
        /// <summary>
        /// 根据批次主键获取批次采集的数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>包含批次采集数据的数据集。
        /// 【EDC_KEY,SP_COUNT,SP_UNITS,EDC_INS_KEY】
        /// </returns>
        public DataSet GetEDCMainIN(string lotKey)
        {
            DataSet dsReturn = new DataSet();
            if (lotKey != "")
            {
                try
                {
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    if (null != serverFactory)
                    {
                        dsReturn = serverFactory.CreateIEDCEngine().GetEDCMainIN(lotKey);
                        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            }
            return dsReturn;
        }
        /// <summary>
        /// 保存数据采集实例数据到数据库中。
        /// </summary>
        public void SaveEDCMainIN()
        {
            DataSet dsReturn = new DataSet();
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY, _edcInsKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY, _edcKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_NAME, _edcName);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_SP_KEY, _samplingKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_ENTERPRISE_KEY, _enterpriseKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_KEY, _lotKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER, _lotNumber);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_KEY, _partKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_ROUTE_KEY, _routeKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_SP_COUNT, _spCount);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_SP_UNITS, _spUnits);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_KEY, _stepKey);
            mainDataHashTable.Add(EDC_MAIN_INS_FIELDS.FIELD_WORK_ORDER_KEY, _workOrderKey);
            mainDataHashTable.Add(COMMON_FIELDS.FIELD_COMMON_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().SaveEDCMainIN(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        }
        /// <summary>
        /// 查询离线数据采集数据。
        /// </summary>
        /// <param name="dtParams">数据采集实例。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含数据采集数据的数据集对象。</returns>
        public DataSet QueryEDCData(DataTable dtParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIEDCEngine().QueryEDCData(dtParams, ref config);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return dsReturn;
        }
    }
}

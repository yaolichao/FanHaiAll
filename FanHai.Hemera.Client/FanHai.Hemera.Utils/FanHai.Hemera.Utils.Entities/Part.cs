using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    public class Part : EntityObject
    {
        //add by chao.pang 2013-12-04 start
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
        //add by chao.pang 2013-12-04 end

        #region Constructor
        public Part() : base(EntityStatus.InActive)
        {
            _partKey = CommonUtils.GenerateNewKey(0);
        }
        public Part(string partKey) : base(EntityStatus.InActive)
        {
            // TODO: Init Part based on key by searching database
            if (partKey.Length > 0)
            {
                GetPartByKey(partKey);
                IsInitializeFinished = true;
            }
        }
        public Part(string partKey, string partNumber, string partVersion, string unit, string descriptions, 
                    string effectivityStart, string effectivityEnd, string status, string instructionsKey, string instructionsName)
            :base((EntityStatus)Convert.ToInt32(status))
        {
            _partKey = partKey;
            _partName = partNumber;
            _partVersion = partVersion;
            //_unit = unit;
            _descriptions = descriptions;
            _effectivityStart = effectivityStart;
            _effectivityEnd = effectivityEnd;
            //_instructionsKey = instructionsKey;
            //_instructionsName = instructionsName;
        }
        #endregion

        #region Public Functions
        #region Actions
        #region override function
        public override bool UpdateStatus()
        {
            if (partUpdate())
                return true;
            else
                return false;
        }
        public override bool Insert()
        {
            bool bResult = false;
            DataSet dataSet = new DataSet();
            List<string> fields = new List<string>(){ 
                                                        POR_PART_FIELDS.FIELD_PART_KEY,
                                                        POR_PART_FIELDS.FIELD_PART_ID,
                                                        POR_PART_FIELDS.FIELD_PART_NAME,
                                                        POR_PART_FIELDS.FIELD_PART_VERSION,                                                       
                                                        POR_PART_FIELDS.FIELD_PART_DESC,
                                                        POR_PART_FIELDS.FIELD_EFFECTIVITY_START,
                                                        POR_PART_FIELDS.FIELD_EFFECTIVITY_END,                                                       
                                                        POR_PART_FIELDS.FIELD_CREATOR,
                                                        POR_PART_FIELDS.FIELD_EDITOR,
                                                        POR_PART_FIELDS.FIELD_PART_TYPE,
                                                        POR_PART_FIELDS.FIELD_PART_MODULE,
                                                        POR_PART_FIELDS.FIELD_CUR_ENTERPRISE_VER_KEY,
                                                        POR_PART_FIELDS.FIELD_CUR_ROUTE_VER_KEY,
                                                        POR_PART_FIELDS.FIELD_CUR_STEP_VER_KEY,
                                                        POR_PART_FIELDS.FIELD_PART_STATUS,
                                                        POR_PART_FIELDS.FIELD_PART_CLASS
                                                    };
            DataTable dtPart=FanHai.Hemera.Utils.Common.Utils
                                        .CreateDataTableWithColumns(POR_PART_FIELDS.DATABASE_TABLE_NAME, fields);

            //为插入做准备 
            object[] partValues = new object[] 
                                                  {
                                                      _partKey, 
                                                      _partName,
                                                      _partName,
                                                      _partVersion,
                                                      _descriptions,
                                                      _effectivityStart,
                                                      _effectivityEnd,
                                                      PropertyService.Get(PROPERTY_FIELDS.USER_NAME),
                                                      PropertyService.Get(PROPERTY_FIELDS.USER_NAME),
                                                      _type,
                                                      _module,
                                                      _enterpriseKey,
                                                      _routeKey,
                                                      _stepKey,
                                                      Convert.ToInt32(_partStatus).ToString(),
                                                      _partClass
                                                  };
            dtPart.Rows.Add(partValues);
            dataSet.Tables.Add(dtPart);

            // Part UDAs
            if (_uda.UserDefinedAttrList.Count > 0)
            {
                DataTable dtPartUDAs = DataTableHelper.CreateDataTableForUDA(POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME, POR_PART_ATTR_FIELDS.FIELDS_PART_KEY);
                _uda.ParseInsertDataToDataTable(ref dtPartUDAs);
                if (dtPartUDAs.Rows.Count > 0)
                {
                    dataSet.Tables.Add(dtPartUDAs);
                }
            }
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIPartEngine().PartInsert(dataSet);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    if (returnMessage.Length < 1)
                    {
                        foreach (UserDefinedAttr uda in _uda.UserDefinedAttrList)
                        {
                            uda.OperationAction = OperationAction.Update;
                        }
                        this.ResetDirtyList();
                        bResult = true;
                    }
                    else
                    {
                        MessageService.ShowError(returnMessage);
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        public override bool Update()
        {
            if (partUpdate())
                return true;
            else
                return false;
        }     
        /// <summary>
        /// 删除成品
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            bool bResult = false;
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIPartEngine().PartDelete(_partKey);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    if (returnMessage.Length < 1)
                    {
                        bResult = true;
                    }
                    else
                    {
                        MessageService.ShowError(returnMessage);
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || _uda.IsDirty);
            }
        }
        #endregion
        private bool partUpdate()
        {
            bool bResult = false;
            DataSet dataSet = new DataSet();
            if (IsDirty)
            {
                if (DirtyList.Count > 0)
                {

                    DataTable partTable = DataTableHelper.CreateDataTableForUpdateBasicData(POR_PART_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _partKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                     };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref partTable, rowData);
                    }
                    if (partTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(partTable);
                    }
                }
                if (_uda.IsDirty)
                {
                    DataTable partUdaTable = DataTableHelper.CreateDataTableForUDA
                      (POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME, POR_PART_ATTR_FIELDS.FIELDS_PART_KEY);
                    _uda.ParseUpdateDataToDataTable(ref partUdaTable);
                    dataSet.Tables.Add(partUdaTable);
                }
                try
                {
                    DataSet dsReturn = null;
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    dsReturn = serverFactory.CreateIPartEngine().PartUpdate(dataSet);
                    string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (returnMsg != string.Empty)
                    {
                        MessageService.ShowError(returnMsg);                       
                    }
                    else
                    {
                        foreach (UserDefinedAttr uda in _uda.UserDefinedAttrList)
                        {
                            uda.OperationAction = OperationAction.Update;
                        }
                        this.ResetDirtyList();
                        bResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);                   
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }
            else
            {
                MessageService.ShowMessage
                 ("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
               
            }
            return bResult;
        }
        public void GetPartByKey(string PartKey)
        {
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPartEngine().GetPartByKey(PartKey);
                string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (returnMsg != string.Empty)
                {
                    MessageService.ShowError(returnMsg);
                }
                else
                {
                    SetPartProperty(dsReturn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        private void SetPartProperty(DataSet dsPart)
        {
            try
            {
                if (dsPart.Tables.Contains(POR_PART_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable partTable = dsPart.Tables[POR_PART_FIELDS.DATABASE_TABLE_NAME];
                    if (partTable.Rows.Count > 0)
                    {
                        PartKey = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_KEY].ToString();
                        PartID = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_ID].ToString();
                        PartName = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_NAME].ToString();
                        PartVersion = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_VERSION].ToString();
                        Module = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_MODULE].ToString();
                        Type = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_TYPE].ToString();
                        EnterpriseName = partTable.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME].ToString();
                        EnterpriseVersion = partTable.Rows[0][POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION].ToString();
                        EnterpriseKey = partTable.Rows[0][POR_PART_FIELDS.FIELD_CUR_ENTERPRISE_VER_KEY].ToString();
                        RouteKey = partTable.Rows[0][POR_PART_FIELDS.FIELD_CUR_ROUTE_VER_KEY].ToString();
                        RouteName = partTable.Rows[0][POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString();
                        StepKey = partTable.Rows[0][POR_PART_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString();
                        StepName = partTable.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
                        Status = (EntityStatus)Convert.ToInt32(partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_STATUS]);
                        Descriptions = partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_DESC].ToString();
                        EffectivityStart =partTable.Rows[0][POR_PART_FIELDS.FIELD_EFFECTIVITY_START].ToString();
                        EffectivityEnd = partTable.Rows[0][POR_PART_FIELDS.FIELD_EFFECTIVITY_END].ToString();
                        Editor=partTable.Rows[0][POR_PART_FIELDS.FIELD_EDITOR].ToString();
                        EditTimeZone=partTable.Rows[0][POR_PART_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                        PartClass= partTable.Rows[0][POR_PART_FIELDS.FIELD_PART_CLASS].ToString();
                    }
                }
                if (dsPart.Tables.Contains(POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable partUdaTable = dsPart.Tables[POR_PART_ATTR_FIELDS.DATABASE_TABLE_NAME];
                    if (partUdaTable.Rows.Count > 0)
                    {
                        SetPartUda(partUdaTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        private void SetPartUda(DataTable partUdaTable)
        {
            foreach (DataRow dataRow in partUdaTable.Rows)
            {
                string linkedPartKey = dataRow[partUdaTable.Columns[POR_PART_FIELDS.FIELD_PART_KEY]].ToString();
                string attributeKey = dataRow[partUdaTable.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY]].ToString();
                string attributeName = dataRow[partUdaTable.Columns[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME]].ToString();
                string attributeValue = dataRow[partUdaTable.Columns[POR_PART_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]].ToString();
                UserDefinedAttr uda = new UserDefinedAttr(linkedPartKey, attributeKey, attributeName, attributeValue, "");
                uda.DataType = dataRow[partUdaTable.Columns["DATA_TYPE"]].ToString();
                uda.OperationAction = OperationAction.Update;                
                _uda.UserDefinedAttrAdd(uda);                
            }
        }
        #endregion //Actions
        #region Properties
        public string PartKey
        {
            get
            {
                return _partKey;
            }
            set
            {
                _partKey = value;
            }
        }
        public string PartName
        {
            get
            {
                return _partName;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_NAME, value);
                _partName = value;
            }
        }

        public string PartID
        {
            get
            {
                return _partId;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_ID, value);
                _partId = value;
            }
        }
        public string PartVersion
        {
            get
            {
                return _partVersion;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_VERSION,value);
                _partVersion = value;
            }
        }
        //public string Unit
        //{
        //    get
        //    {
        //        return _unit;
        //    }
        //    set
        //    {
        //        _unit = value;
        //    }
        //}
        public string Descriptions
        {
            get
            {
                return _descriptions;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_DESC,value);
                _descriptions = value;
            }
        }
        public string EffectivityStart
        {
            get
            {
                return _effectivityStart;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_EFFECTIVITY_START,value);
                _effectivityStart = value;
            }
        }
        public string EffectivityEnd
        {
            get
            {
                return _effectivityEnd;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_EFFECTIVITY_END,value);
                _effectivityEnd = value;
            }
        }
        //public string InstructionsKey
        //{
        //    get
        //    {
        //        return _instructionsKey;
        //    }
        //    set
        //    {
        //        _instructionsKey = value;
        //    }
        //}
        //public string InstructionsName
        //{
        //    get
        //    {
        //        return _instructionsName;
        //    }
        //    set
        //    {
        //        _instructionsName = value;
        //    }
        //}
        public UserDefinedAttrs UserDefinedAttrs
        {
            get
            {
                return _uda;
            }
            set
            {
                _uda = value;
            }
        }
        public string EnterpriseKey
        {
            get
            {
                return _enterpriseKey;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_CUR_ENTERPRISE_VER_KEY,value);
                _enterpriseKey = value;
            }
        }
        public string RouteKey
        {
            get
            {
                return _routeKey;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_CUR_ROUTE_VER_KEY, value);
                _routeKey = value;
            }
        }
        public string StepKey
        {
            get
            {
                return _stepKey;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_CUR_STEP_VER_KEY,value);
                _stepKey = value;
            }
        }
        public string EnterpriseName
        {
            get
            {
                return _enterpriseName;
            }
            set
            {
                _enterpriseName = value;
            }
        }
        public string RouteName
        {
            get
            {
                return _routeName;
            }
            set
            {
                _routeName = value;
            }
        }
        public string StepName
        {
            get
            {
                return _stepName;
            }
            set
            {
                _stepName = value;
            }
        }
        public string EnterpriseVersion
        {
            get
            {
                return _enterpriseVersion;
            }
            set
            {
                _enterpriseVersion = value;
            }
        }
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_TYPE,value);
                _type = value;
            }
        }
        public string Module
        {
            get
            {
                return _module;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_MODULE,value);
                _module = value;
            }
        }

        public string PartClass
        {
            get
            {
                return _partClass;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_CLASS, value);
                _partClass = value;
            }
        }

        public override EntityStatus Status
        {
            get
            {
                return _partStatus;
            }
            set
            {
                ValidateDirtyList(POR_PART_FIELDS.FIELD_PART_STATUS, Convert.ToInt32(value).ToString());
                _partStatus = value;
            }
        }


        #endregion // Properties
        #endregion //Public Functions

       

        #region Private Variables Definitions
        // Part basic data
        private string _partKey = "";
        private string _partId = "";
        private string _partName = "";
        private string _partVersion = "1";
        //private string _unit = "";
        private string _descriptions = "";
        //private string _effectivityStart = DateTime.Now.ToString();
        //private string _effectivityEnd = "9999.12.31 23:59:59";
        private string _effectivityStart = "";
        private string _effectivityEnd = "";
        private string _enterpriseKey = "";
        private string _enterpriseVersion = "";
        private string _routeKey = "";
        private string _stepKey = "";
        private string _enterpriseName = "";
        private string _routeName="";
        private string _stepName = "";
        private string _type = "";
        private string _module = "";
        //vicky add
        private string _partClass = "";
        private EntityStatus _partStatus = EntityStatus.InActive;
        // SOP parts
       // private string _instructionsKey = "";
       // private string _instructionsName = "";

        //
        UserDefinedAttrs _uda = new UserDefinedAttrs(POR_PART_ATTR_FIELDS.FIELDS_PART_KEY);
        #endregion


        //add by chao.pang 2013-12-04 start
        public DataSet GetPartType()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIPartEngine().GetPartType();
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
        //add by chao.pang 2013-12-04 end
    }
}

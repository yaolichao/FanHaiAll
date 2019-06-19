/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using System.Data;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Utils.Entities
{
    public class Param:EntityObject
    {

        public Param()
        {
            _paramKey =  CommonUtils.GenerateNewKey(0);
        }

        public Param(string paramKey)
        {
            _paramKey = paramKey;

            if (paramKey.Length > 0)
            {
                GetParamByKey(paramKey);
                this.IsInitializeFinished = true;
            }
        }

        #region Send insert data to server
        /// <summary>
        /// Send insert data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable paramTable = DataTableHelper.CreateDataTableForInsertParam();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY, _paramKey},
                                                        {BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, _paramName},
                                                        {BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS, _paramDescription},
                                                        {BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY, _paramCategory},
                                                        {BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE, _paramDataType},
                                                        {BASE_PARAMETER_FIELDS.FIELD_DEFAULT_UOM, _paramDefaultUom},
                                                        {BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY, _paramUpperBoundary},
                                                        {BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC, _paramUpperSpec},
                                                        {BASE_PARAMETER_FIELDS.FIELD_TARGET, _paramTarget},
                                                        {BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY,_paramLowerBoundary},
                                                        {BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC, _paramLowerSpec},
                                                        {BASE_PARAMETER_FIELDS.FIELD_MANDATORY,_paramMandatory},
                                                        {BASE_PARAMETER_FIELDS.FIELD_STATUS, Convert.ToInt32(_paramStatus).ToString()},
                                                        {BASE_PARAMETER_FIELDS.FIELD_ISDERIVED, _isDerived},
                                                        {BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE,_calculateType},
                                                        {COMMON_FIELDS.FIELD_COMMON_CREATOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                        {BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE,_deviceType}
                                                    };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref paramTable, dataRow);
            dataSet.Tables.Add(paramTable);

            if (_isDerived == "1")
            {
                if (derivaParamTable.Rows.Count > 0)
                {
                    dataSet.Tables.Add(derivaParamTable); 
                }
            }

            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIParamEngine().ParamInsert(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        this.ResetDirtyList();
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
                dataSet.Tables.Clear();
            }

            return true;
        }
        #endregion

        #region Send update data to server
        /// <summary>
        /// Send update data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();
                if (this.DirtyList.Count > 0)
                {
                    DataTable paramTable = DataTableHelper.CreateDataTableForUpdateBasicData(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);

                    List<string> possibleDirtyFields = new List<string>(){ BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY,
                                                                           BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_DEFAULT_UOM,
                                                                           BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY,
                                                                           BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_TARGET, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY,
                                                                           BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_MANDATORY,
                                                                           BASE_PARAMETER_FIELDS.FIELD_STATUS, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_ISDERIVED, 
                                                                           BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE,
                                                                           BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE,
                                                                           COMMON_FIELDS.FIELD_COMMON_EDITOR};

                    string newValue = string.Empty;
                    foreach (string field in possibleDirtyFields)
                    {
                        if (this.DirtyList.ContainsKey(field))
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _paramKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };

                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref paramTable, rowData);
                        }
                    }

                    if (paramTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(paramTable);
                    }
                }

                if (_isDerived == "1")
                {
                    if (derivaParamTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(derivaParamTable);
                    }
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIParamEngine().ParamUpdate(dataSet);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            this.ResetDirtyList();
                            MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex);
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                    dataSet.Tables.Clear();
                }
            }
            else
            {
                MessageService.ShowMessage
                     ("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
            }

            return true;
        }
        #endregion

        #region Send delete data to server
        /// <summary>
        /// Send delete data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Delete()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIParamEngine().DeleteParam(_paramKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return true;
        }
        #endregion

        #region Get param via key
        /// <summary>
        /// Get param via key
        /// </summary>
        public void GetParamByKey(string paramKey)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIParamEngine().GetParamByKey(paramKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        SetParamProperties(dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME]);
                        SetDerivaParamProperties(dsReturn.Tables[BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME]);
                        dsReturn.Tables.Remove(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);
                        dsReturn.Tables.Remove(BASE_PARAMETER_DERIVTION_FIELDS.DATABASE_TABLE_NAME);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        #endregion

        #region Set param properties
        /// <summary>
        /// Set param properties
        /// </summary>
        private void SetParamProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count>0 && 1 == dt.Rows.Count)
                {
                    ParamName = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString();
                    ParamDescription = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    ParamCategory = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY].ToString();
                    ParamDataType = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE].ToString();
                    ParamDefaultUom = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_DEFAULT_UOM].ToString();
                    ParamUpperBoundary = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY].ToString();
                    ParamUpperSpec = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC].ToString();
                    ParamTarget = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_TARGET].ToString();
                    ParamLowerBoundary = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY].ToString();
                    ParamLowerSpec = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC].ToString();
                    ParamMandatory = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_MANDATORY].ToString();
                    DeviceType = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE].ToString();
                    Status = (EntityStatus)Convert.ToInt32(dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_STATUS]);
                    IsDerived = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_ISDERIVED].ToString();
                    CalculateType = dt.Rows[0][BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE].ToString();

                    Editor = dt.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDITOR].ToString();
                    EditTime = dt.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDIT_TIME].ToString();
                    EditTimeZone = dt.Rows[0][COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE].ToString();

                    OperationAction = OperationAction.Update;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        #endregion

        #region Set derivation param properties
        /// <summary>
        /// Set derivation param properties
        /// </summary>
        private void SetDerivaParamProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    derivaParamTable = dt;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        #endregion

        #region Validate param name exist or not
        /// <summary>
        /// Validate param name exist or not
        /// </summary>
        public bool ParamNameValidate()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIParamEngine().GetDistinctParamName();
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        foreach (DataRow dataRow in dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME].Rows)
                        {
                            if (_paramName == dataRow[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString())
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return true;
        }
        #endregion

        #region Update param status
        /// <summary>
        /// Update param status
        /// </summary>
        public override bool UpdateStatus()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();

                if (this.DirtyList.Count > 0)
                {
                    DataTable paramTable =
                        DataTableHelper.CreateDataTableForUpdateBasicData(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME);

                    List<string> possibleDirtyFields = new List<string>() { BASE_PARAMETER_FIELDS.FIELD_STATUS, 
                                                                            COMMON_FIELDS.FIELD_COMMON_EDITOR };

                    string newValue = string.Empty;
                    foreach (string field in possibleDirtyFields)
                    {
                        if (this.DirtyList.ContainsKey(field))
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _paramKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref paramTable, rowData);
                        }
                    }
                    if (paramTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(paramTable);
                    }
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIParamEngine().ParamUpdate(dataSet);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            this.ResetDirtyList();
                            MessageService.ShowMessage("${res:Global.UpdateStatusMessage}", "${res:Global.SystemInfo}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex);
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }

            return true;
        }
        #endregion

        #region Properties

        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || IsDerived == "1");
            }
        }

        public string ParamKey
        {
            get { return _paramKey; }
            set { _paramKey = value; }
        }

        public string ParamName
        {
            get { return _paramName; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, value);
                _paramName = value; 
            }
        }

        public string ParamCategory
        {
            get { return _paramCategory; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY, value);
                _paramCategory = value; 
            }
        }

        public string ParamDataType
        {
            get { return _paramDataType; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE, value);
                _paramDataType = value; 
            }
        }

        public string ParamDescription
        {
            get { return _paramDescription; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_DESCRIPTIONS, value);
                _paramDescription = value; 
            }
        }

        public string ParamDefaultValue
        {
            get { return _paramDefaultValue; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_DEFAULT_VALUE, value);
                _paramDefaultValue = value; 
            }
        }

        public string ParamDefaultUom
        {
            get { return _paramDefaultUom; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_DEFAULT_UOM, value);
                _paramDefaultUom = value; 
            }
        }

        public string ParamUpperBoundary
        {
            get { return _paramUpperBoundary; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY, value);
                _paramUpperBoundary = value; 
            }
        }

        public string ParamUpperSpec
        {
            get { return _paramUpperSpec; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC, value);
                _paramUpperSpec = value; 
            }
        }

        public string ParamUpperControl
        {
            get { return _paramUpperControl; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_UPPER_CONTROL, value);
                _paramUpperControl = value; 
            }
        }

        public string ParamTarget
        {
            get { return _paramTarget; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_TARGET, value);
                _paramTarget = value; 
            }
        }

        public string ParamLowerControl
        {
            get { return _paramLowerControl; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_LOWER_CONTROL, value);
                _paramLowerControl = value; 
            }
        }

        public string ParamLowerSpec
        {
            get { return _paramLowerSpec; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC, value);
                _paramLowerSpec = value; 
            }
        }

        public string ParamLowerBoundary
        {
            get { return _paramLowerBoundary; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY, value);
                _paramLowerBoundary = value; 
            }
        }

        public string ParamMandatory
        {
            get { return _paramMandatory; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_MANDATORY, value);
                _paramMandatory = value; 
            }
        }

        public string ParamSiteNumber
        {
            get { return _paramSiteNumber; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_SITE_NUMBER, value);
                _paramSiteNumber = value; 
            }
        }

        public override EntityStatus Status
        {
            get { return _paramStatus; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_STATUS, Convert.ToInt32(value).ToString());
                _paramStatus = value; 
            }
        }

        public string IsDerived
        {
            get { return _isDerived; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_ISDERIVED, value);
                _isDerived = value; 
            }
        }

        public string CalculateType
        {
            get { return _calculateType; }
            set 
            {
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_CALCULATE_TYPE, value);
                _calculateType = value; 
            }
        }
        public string DeviceType
        {
            get { return _deviceType; }
            set 
            {
                _deviceType = value;
                ValidateDirtyList(BASE_PARAMETER_FIELDS.FIELD_DEVICE_TYPE, value);
            }
        }


        public DataTable DerivaParamTable
        {
            get { return derivaParamTable; }
            set { derivaParamTable = value; }
        }


        public OperationAction OperationAction
        {
            get
            {
                return _operationAction;
            }
            set
            {
                _operationAction = value;
            }
        }

        public string ErrorMsg
        {
            get { return _msg; }
        }
        #endregion
        #region Private variable definition
        //Private variable definition
        private string _paramKey = string.Empty;
        private string _paramName = string.Empty;     
        private string _paramCategory = string.Empty;     
        private string _paramDataType = string.Empty;     
        private string _paramDescription = string.Empty;        
        private string _paramDefaultValue = string.Empty;       
        private string _paramDefaultUom = string.Empty;    
        private string _paramUpperBoundary = string.Empty;     
        private string _paramUpperSpec = string.Empty;       
        private string _paramUpperControl = string.Empty;
        private string _paramTarget = string.Empty;
        private string _paramLowerControl = string.Empty;
        private string _paramLowerSpec = string.Empty;
        private string _paramLowerBoundary = string.Empty;
        private string _paramMandatory = string.Empty;
        private string _paramSiteNumber = string.Empty;

        private string _isDerived = "0";
        private string _calculateType = string.Empty;
        private string _deviceType = string.Empty;

        private string _msg = string.Empty;

        private DataTable derivaParamTable = null;

        private EntityStatus _paramStatus = EntityStatus.InActive;
        private OperationAction _operationAction = OperationAction.None;
        #endregion


        public DataSet GetBaseParamsByCategory()
        {
            DataSet dsReturn = null;
            try
            {
                _msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIParamEngine().GetBaseParamsByCategory();
                    _msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _msg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
    }
}

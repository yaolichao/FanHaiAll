using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
     public class UdaEntity :EntityObject
    {
        #region Private variable definition
        private string _objectKey = "";
        private string _objectName = "";
        private string _categoryKey = "";
        private string _line_code = "";
        private string _description = "";
        private string _linkedToTable = "";
        //private bool _isLineTableUpdate = false;
        
        private string _errorMsg="";

        private UserDefinedAttrsEx _UDAs = new UserDefinedAttrsEx(BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY, FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME);

        #endregion

        #region property
        public string ObjectKey
        {
            get { return _objectKey; }
            set { _objectKey = value; }
        }


        public string ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }

        public string CategoryKey
        {
            get { return _categoryKey; }
            set { _categoryKey = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
    

        public string LineCode
        {
            get { return _line_code; }
            set { _line_code = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
 
        }

        public string LinkedToTable
        {
            get { return _linkedToTable; }
            set { _linkedToTable = value; }

        }

        public UserDefinedAttrsEx UserDefinedAttrsEx
        {
            get
            {
                return _UDAs;
            }
            set
            {
                _UDAs = value;
            }
        }

        #endregion

        #region Constructor
        public UdaEntity()
        {
            _objectKey =  CommonUtils.GenerateNewKey(0);
        }
        public UdaEntity(string objectName)
        {
            if (objectName.Length > 0)
            {
                GetObjectConfByName(objectName);
                IsInitializeFinished = true;
            }
        }
        #endregion

        #region override Function
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || _UDAs.IsDirty);
            }
        }

        public override bool Insert()
        {
            bool bResult = false;
            DataSet dataSet = new DataSet();

            DataTable dtAttributeConf = CreateDataTableForInsert();
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                   {FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,_objectKey},
                                                   {FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,_objectName.ToUpper()},
                                                   {FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE,_line_code},
                                                   {FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS,_description},
                                                   {FMM_PRODUCTION_LINE_FIELDS.FIELD_EDITOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dtAttributeConf, dataRow);
            if (dtAttributeConf.Rows.Count > 0)
            {
                dataSet.Tables.Add(dtAttributeConf);
            }
            
            //  UDAs
            if (_UDAs.UserDefinedAttrList.Count > 0)
            {
                DataTable dtUDAs = DataTableHelper.CreateDataTableForUDAEx(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME, BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY,BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE);
                _UDAs.ParseInsertDataToDataTable(ref dtUDAs);

                if (dtUDAs.Rows.Count > 0)
                {
                    dataSet.Tables.Add(dtUDAs);
                }
            }
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    if (dataSet.Tables.Count > 0)
                    {
                        DataSet retDS = factor.CreateIUdaCommonControlEx().AddLineAttributeValue(dataSet);
                        string strMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                        if (strMsg.Length < 1)
                        {
                            foreach (UserDefinedAttrEx uda in _UDAs.UserDefinedAttrList)
                            {
                                uda.OperationAction = OperationAction.Update;
                            }
                            this.ResetDirtyList();
                            bResult = true;
                        }
                        else
                        {
                            MessageService.ShowError(strMsg);
                        }
                    }
                    else
                    {
                        MessageService.ShowWarning("No dataTable in input parameter");
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }

       
        public override bool Update()
        {
            if (UdaObjectUpdate())
                return true;
            else
                return false;
        }

        public override bool Delete()
        {
            bool bResult = false;
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIUdaCommonControlEx().DeleteLineTypeAttribute(_objectKey);
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
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        public override bool UpdateStatus()
        {
            //if (WorkOrderUpdate())
            //    return true;
            //else
                return false;
        }
        #endregion


        private bool UdaObjectUpdate()
        {
            bool bReturn = false;
            DataSet dataSet = new DataSet();
            if (IsDirty)
            {
                if (DirtyList.Count > 0)
                {

                    DataTable entityTable = DataTableHelper.CreateDataTableForUpdateBasicData(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _objectKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                     };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref entityTable, rowData);
                    }
                    if (entityTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(entityTable);
                    }
                }
                if (_UDAs.IsDirty)
                {
                    DataTable dtUDAs = DataTableHelper.CreateDataTableForUDAEx(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME, BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY, BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_TYPE);
                    _UDAs.ParseUpdateDataToDataTable(ref dtUDAs);
                    dataSet.Tables.Add(dtUDAs);
                }
                try
                {
                    DataSet dsReturn = null;
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    dsReturn = serverFactory.CreateIUdaCommonControlEx().UpdateLineTypeAttribute(dataSet);
                    string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (returnMsg.Length < 1)
                    {
                        foreach (UserDefinedAttrEx uda in _UDAs.UserDefinedAttrList)
                        {
                            uda.OperationAction = OperationAction.Update;
                        }
                        this.ResetDirtyList();
                        bReturn = true;
                    }
                    else
                    {
                        MessageService.ShowError(returnMsg);
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
                //MessageService.ShowMessage
                // ("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
                MessageService.ShowMessage("自定义属性没有更新!");
            }
            return bReturn;
        }

    

        #region Get Config Information Data

        public void GetObjectConfByName(string objectName)
        {
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIUdaCommonControlEx().GetLineTypeByName(objectName);
                string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (returnMsg != string.Empty)
                {
                    MessageService.ShowError(returnMsg);
                }
                else
                {
                    SetAttributeDataToProperty(dsReturn);
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

        private void SetAttributeDataToProperty(DataSet dataSet)
        {
            try
            {
                if (dataSet.Tables.Contains(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable objTable = dataSet.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
                    if (objTable.Rows.Count > 0)
                    {
                        _objectKey = objTable.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY].ToString();
                        _objectName = objTable.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
                        _line_code = objTable.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE].ToString();
                        _description = objTable.Rows[0][FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    }
                }
                if (dataSet.Tables.Contains(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable objUdaTable = dataSet.Tables[BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME];
                    if (objUdaTable.Rows.Count > 0)
                    {
                        SetObjUda(objUdaTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        private void SetObjUda(DataTable objUdaTable)
        {
            foreach (DataRow dataRow in objUdaTable.Rows)
            {
                string linkedObjKey = dataRow[objUdaTable.Columns[BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_OBJECT_KEY]].ToString();
                string attributeKey = dataRow[objUdaTable.Columns[BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_ATTRIBUTE_KEY]].ToString();
                string attributeName = dataRow[objUdaTable.Columns[BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_ATTRIBUTE_NAME]].ToString();
                string attributeValue = dataRow[objUdaTable.Columns[BASE_ATTRIBUTE_VALUE_FIELDS.FIELD_ATTRIBUTE_VALUE]].ToString();
                UserDefinedAttrEx uda = new UserDefinedAttrEx(linkedObjKey, attributeKey, attributeName, attributeValue, "");
                uda.DataType = dataRow[objUdaTable.Columns["DATA_TYPE"]].ToString();
                uda.OperationAction = OperationAction.Update;
                _UDAs.UserDefinedAttrAdd(uda);
            }
        }


        #endregion



        #region Others

        private DataTable CreateDataTableForInsert()
        {
            List<string> fields = new List<string>() 
                                                    { FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY,
                                                      FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME,
                                                      FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE,
                                                      FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS,
                                                      FMM_PRODUCTION_LINE_FIELDS.FIELD_EDITOR };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        #endregion



        public DataSet InitObjectType()
        {
            DataSet dsObjectType = new DataSet();
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            dsObjectType = serverFactory.CreateIUdaCommonControlEx().GetUdaObjectType();
            return dsObjectType;
        }


        public DataSet InitObjectNameByType(string objectType)
        {
            DataSet dsObjectName = new DataSet();
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            dsObjectName = serverFactory.CreateIUdaCommonControlEx().GetUdaObjectNameList(objectType);
            return dsObjectName;
        }
    }
}

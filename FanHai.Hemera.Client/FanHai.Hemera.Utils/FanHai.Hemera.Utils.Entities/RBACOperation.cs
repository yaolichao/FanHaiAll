/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Utils.Entities
{
    public class RBACOperation:EntityObject
    {
        #region define attribute
        private string _operationKey = "";
        private string _operationName = "";
        private string _GroupKey = "";
        private string _displayName = "";
        private string _remark = "";        
        private string _operationCode = "";        
        private string _errorMsg = "";        
        #endregion

        #region Properties       
        public string OperationKey
        {
            get { return _operationKey; }
            set { _operationKey = value; }
        }
        public string OperationCode
        {
            get
            {
                return this._operationCode;
            }
            set
            {
                this._operationCode = value;
                ValidateDirtyList(RBAC_OPERATION_FIELDS.FIELD_OPERATION_CODE, value);
            }
        }
        public string GroupKey
        {
            get
            {
                return _GroupKey;
            }
            set
            {
                _GroupKey = value;
                ValidateDirtyList(RBAC_OPERATION_FIELDS.FIELD_OPERATION_GROUP_KEY, value);
            }
        }
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
                ValidateDirtyList(RBAC_OPERATION_FIELDS.FIELD_REMARK, value);
            }
        }
        public string OperationName
        {
            get
            {
                return _operationName;
            }
            set
            {
                _operationName = value;
                ValidateDirtyList(RBAC_OPERATION_FIELDS.FIELD_OPERATION_NAME, value);
            }
        }
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                ValidateDirtyList(RBAC_OPERATION_FIELDS.FIELD_DISPLAY_NAME, value);
            }
        }     
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        #endregion       

        #region Action
        public void SaveOperation(bool bNew, DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            if (bNew)
            {
                ds = dataset;
            }
            else
            {
                if (IsDirty)
                {
                    if (DirtyList.Count>0)
                    {
                        DataTable operationTable = DataTableHelper.CreateDataTableForUpdateBasicData(RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME);

                        foreach (string Key in DirtyList.Keys)
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _operationKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE,DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE,DirtyList[Key].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref operationTable, rowData);
                        }
                        if (operationTable.Rows.Count > 0)
                        {
                            ds.Tables.Add(operationTable);
                        }
                    }
                }
            }
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    if (bNew)
                    {
                        dsReturn = serverFactory.CreateRBACOperationEngine().AddOperation(ds);
                    }
                    else
                    {
                        dsReturn = serverFactory.CreateRBACOperationEngine().UpdateOperation(ds);
                    }
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

        public void DeleteOperation()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateRBACOperationEngine().DeleteOperation(_operationKey);
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
        public DataSet GetOperation()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateRBACOperationEngine().GetOperation(_GroupKey);
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
        #endregion
    }
}

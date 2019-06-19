/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections;
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
    public class Resource:EntityObject
    {
        #region define attribute
        private string _resourceKey = "";
        private string _resourceGroupKey = "";
        private string _resourceCode = "";
        private string _resourceName = "";
        private string _descriptions = "";      
        private string _remark = "";       
        private string _errorMsg = "";      
        #endregion

        #region Properties       
        public string ResourceKey
        {
            get
            {
                return this._resourceKey;
            }
            set
            {
                this._resourceKey = value;
            }
        }
        public string ResourceGroupKey
        {
            get
            {
                return this._resourceGroupKey;
            }
            set
            {
                this._resourceGroupKey = value;
                ValidateDirtyList(RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY, value);
            }
        }
        public string ResourceCode
        {
            get
            {
                return this._resourceCode;
            }
            set
            {
                this._resourceCode = value;
                ValidateDirtyList(RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE, value);
            }
        }
        public string ResourceName
        {
            get
            {
                return this._resourceName;
            }
            set
            {
                this._resourceName = value;
                ValidateDirtyList(RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_NAME, value);
            }
        }
        public string Descriptions
        {
            get
            {
                return _descriptions;
            }
            set
            {
                _descriptions = value;
                ValidateDirtyList(RBAC_RESOURCE_FIELDS.FIELD_DESCRIPTIONS, value);
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
                ValidateDirtyList(RBAC_RESOURCE_FIELDS.FIELD_REMARK, value);
            }
        }
       
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        #endregion       

        #region Action
        public void SaveResource(bool bNew, DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            if (bNew)
            {
                ds = dataSet;
            }
            else
            {
                if (IsDirty)
                {
                    if (this.DirtyList.Count>0)
                    {
                        if (DirtyList.ContainsKey(RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE))
                        {
                            DirtyItem dItem = new DirtyItem();
                            dItem.FieldOriginalValue = "";
                            dItem.FieldNewValue = _resourceGroupKey;
                            DirtyList.Add(RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY,dItem);
                        }
                        DataTable resourceTable = DataTableHelper.CreateDataTableForUpdateBasicData(RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME);

                        foreach (string Key in DirtyList.Keys)
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _resourceKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}                                                        
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref resourceTable, rowData);
                        }
                        if (resourceTable.Rows.Count > 0)
                        {
                            ds.Tables.Add(resourceTable);
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
                        dsReturn = serverFactory.CreateIResourceEngine().AddResource(ds);
                    }
                    else
                    {
                        dsReturn = serverFactory.CreateIResourceEngine().UpdateResource(ds);
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

        public void DeleteResource()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIResourceEngine().DeleteResource(_resourceKey);
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
        public DataSet GetResource()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIResourceEngine().GetResource(_resourceGroupKey);
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

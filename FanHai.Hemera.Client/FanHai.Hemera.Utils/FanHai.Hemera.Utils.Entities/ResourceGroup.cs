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

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Utils.Entities
{
    public class ResourceGroup:EntityObject
    {
        #region define attribute
        private string _GroupKey = "";
        private string _GroupName = "";
        private string _resourceGroupCode = "";
        private string _descriptions = "";
        private string _remark = "";      
        private string _errorMsg = "";       
        #endregion

        #region Properties       
        public string Remark
        {
            get { return _remark; }
            set 
            {
                _remark = value;
                ValidateDirtyList(RBAC_RESOURCE_GROUP_FIELDS.FIELD_REMARK,value);
            }
        }
        public string Descriptions
        {
            get { return _descriptions; }
            set 
            {
                _descriptions = value;
                ValidateDirtyList(RBAC_RESOURCE_GROUP_FIELDS.FIELD_DESCRIPTIONS,value);
            }
        }
        public string GroupKey
        {
            get { return _GroupKey; }
            set { _GroupKey = value; }
        }
        public string GroupName
        {
            get 
            {
                return _GroupName;
            }
            set
            { 
                _GroupName = value;
                ValidateDirtyList(RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME,value);
            }
        }
        public string ResourceGroupCode
        {
            get
            {
                return this._resourceGroupCode;
            }
            set
            {
                this._resourceGroupCode = value;
                ValidateDirtyList(RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_CODE,value);
            }
        }       
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        #endregion      

        #region Action
        public void AddResourceGroup(bool bNew,DataSet dataset)
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
                        DataTable resourceGroupTable = DataTableHelper.CreateDataTableForUpdateBasicData(RBAC_RESOURCE_GROUP_FIELDS.DATABASE_TABLE_NAME);

                        foreach (string Key in DirtyList.Keys)
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _GroupKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE,DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE,DirtyList[Key].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref resourceGroupTable, rowData);
                        }
                        if (resourceGroupTable.Rows.Count > 0)
                        {
                            ds.Tables.Add(resourceGroupTable);
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
                        dsReturn = serverFactory.CreateIResourceGroupEngine().AddResourceGroup(ds);
                    }
                    else
                    {
                        dsReturn = serverFactory.CreateIResourceGroupEngine().UpdateResourceGroup(ds);
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
        /// <summary>
        /// 获取资源组数据。
        /// </summary>
        /// <returns></returns>
        public DataSet GetResourceGroup()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIResourceGroupEngine().GetResourceGroup(_GroupKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }
        public void DeleteResourceGroup()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIResourceGroupEngine().DeleteResourceGroup(_GroupKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
        }
        #endregion
    }
}

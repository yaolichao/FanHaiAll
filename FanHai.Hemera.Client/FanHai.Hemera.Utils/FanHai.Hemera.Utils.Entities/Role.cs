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
    public class Role:EntityObject
    {
        #region define attribute
        private string _roleKey = "";
        private string _roleName = "";
        private string _description = "";
        private string _remark = "";        
        private string _errorMsg = "";             
        #endregion

        #region Properties        
        public string RoleKey
        {
            get 
            {
                return this._roleKey;
            }
            set
            {
                this._roleKey = value;
            }
        }

        public string RoleName
        {
            get
            {
                return this._roleName;
            }
            set
            {
                this._roleName = value;
                ValidateDirtyList(RBAC_ROLE_FIELDS.FIELD_ROLE_NAME, value);
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
                ValidateDirtyList(RBAC_ROLE_FIELDS.FIELD_DESCRIPTIONS,value);
            }
        }

        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
                ValidateDirtyList(RBAC_ROLE_FIELDS.FIELD_REMARK,value);
            }
        }      
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }
        #endregion

        #region override ToString
        public override string ToString()
        {

            return this._roleName;

        }
        #endregion

        #region Construction
        public Role()
        {

        }
        public Role(string roleKey)
        {
            this._roleKey = roleKey;
        }
        #endregion       

        #region Action
        public DataSet GetRoleInfo()
        {
            DataSet dsReturn = new DataSet();           
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIRoleEngine().GetRoleInfo(_roleKey);
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

        public DataSet GetUsersOfRole()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserRoleEngine().GetUsersOfRole(_roleKey);
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

        public void SaveRole(bool isNew)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            Hashtable hashTable = new Hashtable();
            DataTable dataTable = new DataTable();
            if (isNew)
            {
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_ROLE_KEY, _roleKey);
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_ROLE_NAME,_roleName);
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_DESCRIPTIONS,_description);
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_REMARK,_remark);
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_CREATOR,Creator);
                hashTable.Add(RBAC_ROLE_FIELDS.FIELD_CREATE_TIMEZONE,CreateTimeZone);              
                dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataTable.TableName = RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME;
                ds.Tables.Add(dataTable);
            }
            else
            {
                //update
                if (IsDirty)
                {                    
                    if (DirtyList.Count>0)
                    {
                        foreach (string key in DirtyList.Keys)
                        {
                            hashTable.Add(key, DirtyList[key].FieldNewValue);
                        }
                        hashTable.Add(RBAC_ROLE_FIELDS.FIELD_ROLE_KEY, _roleKey);                        
                        dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                        dataTable.TableName = RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME;
                        ds.Tables.Add(dataTable);
                    }
                }
            }
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    if (isNew)
                    {
                        dsReturn = serverFactory.CreateIRoleEngine().AddRole(ds);
                    }
                    else
                    {
                        dsReturn = serverFactory.CreateIRoleEngine().UpdateRole(ds);
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

        public void DeleteRole()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIRoleEngine().DeleteRole(_roleKey);
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
        #endregion
    }
}

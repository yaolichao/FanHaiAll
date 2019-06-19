/*
<FileInfo>
  <Author>Rayna.Liu, FanHai Hemera</Author>
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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    public class User:EntityObject
    {
        #region define attribute
        private string _userKey="";
        private string _badge="";
        private string _userName="";
        private string _password="";
        private string _email="";
        private string _officePhone="";
        private string _mobilePhone="";       
        private string _isLockedOut="0"; //verify wheter the current user is locked
        private string _isApproved="0";  //verify wheter the current user is approved
        private string _isActive="1";    //verify wheter the user is online        
        private string _lastLoginIP="";     //the last login IP            
        private string _remark="";       
        private string _errorMsg = "";
        #endregion

        #region Properties       
        public string UserKey
        {
            get 
            { 
                return this._userKey;
            }
            set
            {
                this._userKey = value;
            }
        }
        public string ErrorMsg
        {
            get { return this._errorMsg; }
            set { }
        }       

        /// <summary>
        /// Get and set the badge of user
        /// </summary>
        public string Badge
        {
            get 
            {
                return this._badge;
            }
            set
            {
               
                this._badge = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_BADGE, value);
                
            }
        }

        /// <summary>
        /// Get and set the name of user
        /// </summary>
        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
               
                this._userName = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_USERNAME, value);
                
            }
        }

        /// <summary>
        /// Get and set the password of user
        /// </summary>
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
               
                this._password = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_PASSWORD, value);
                
            }
        }

        /// <summary>
        /// Get and set the email of user
        /// </summary>
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                
                this._email = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_EMAIL, value);
               
            }
        }

        /// <summary>
        /// Get and set the telephone of user
        /// </summary>
        public string OfficePhone
        {
            get
            {
                return this._officePhone;
            }
            set
            {
               
                this._officePhone = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_OFFICE_PHONE, value);
                
            }
        }

        /// <summary>
        /// Get and set the mobilephone of user
        /// </summary>
        public string MobilePhone
        {
            get
            {
                return this._mobilePhone;
            }
            set
            {
                
                this._mobilePhone = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_MOBILE_PHONE, value);
                
            }
        }                

        /// <summary>
        /// Gets a value that indicates whether the user has been locked
        /// </summary>
        public string IsLockedOut
        {
            get
            {
                return this._isLockedOut;
            }
            set
            {
                this._isLockedOut = value;
            }

        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        public string IsApproved
        {
            get
            {
                return this._isApproved;
            }
            set
            {
                this._isApproved = value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has online
        /// </summary>
        public string IsActive
        {
            get
            {
                return this._isActive;
            }
            set
            {
                this._isActive = value;
            }              
        }       

        /// <summary>
        /// get and set the ip of a user last login
        /// </summary>
        public string LastLoginIP
        {
            get
            {
                return this._lastLoginIP;
            }
            set
            {
                this._lastLoginIP = value;
            }
        }
       

        /// <summary>
        /// get and set remark information about user
        /// </summary>
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {                
                this._remark = value;
                ValidateDirtyList(RBAC_USER_FIELDS.FIELD_REMARK, value);
            }
        }
        #endregion

        #region override ToString
        public override string ToString()
        {
            return this._userName;
        }
        #endregion      
       
        #region construct function
        public User()
        {

        }
        
        #endregion

        #region Action
        public DataSet GetUserInfo()
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            DataTable dataTable = new DataTable();
            Hashtable hashTable = new Hashtable();
            hashTable.Add(RBAC_USER_FIELDS.FIELD_USER_KEY,_userKey);
            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            dataTable.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
            ds.Tables.Add(dataTable);
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserEngine().GetUserInfo(ds);
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

        public DataSet SearchUserInfo()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserEngine().SearchUser(_userName);
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

        public DataSet GetRolesOfUser()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserRoleEngine().GetRolesOfUser(_userKey);
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

        public DataSet GetRoleNotBelongToUser()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserRoleEngine().GetRolesNotBelongToUser(_userKey);
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

        public void SaveUserRole(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserRoleEngine().SaveUserRole(dataset);
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

        public void DeleteUser()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIUserEngine().DeleteUser(_userKey);
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

        public void SaveUser(bool isNew)
        {
            DataSet dsReturn = new DataSet();
            DataSet ds = new DataSet();
            Hashtable hashTable = new Hashtable();
            DataTable dataTable = new DataTable();
            if (isNew)
            {
                hashTable.Add(RBAC_USER_FIELDS.FIELD_USER_KEY, _userKey);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_USERNAME, _userName);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_PASSWORD, _password);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_BADGE, _badge);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_OFFICE_PHONE, _officePhone);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_MOBILE_PHONE, _mobilePhone);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_REMARK, _remark);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_EMAIL, _email);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_CREATOR,Creator);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_CREATE_TIMEZONE, CreateTimeZone);
                dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataTable.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
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
                            hashTable.Add(key,DirtyList[key].FieldNewValue);
                        }
                        hashTable.Add(RBAC_USER_FIELDS.FIELD_USER_KEY,_userKey);
                        dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                        dataTable.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
                        ds.Tables.Add(dataTable);                        
                    }
                }
            }
            
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    if(isNew)
                    {
                        dsReturn = serverFactory.CreateIUserEngine().AddUser(ds);
                    }                    
                    else
                    {
                        dsReturn=serverFactory.CreateIUserEngine().UpdateUser(ds);
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

        public string GetPrivilegeOfUser()
        {
            string privileges = "";
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    DataSet dsReturn = new DataSet();
                    dsReturn = serverFactory.CreateIUserEngine().GetPrivilegeOfUser(_userKey);
                    if (dsReturn.Tables.Count > 0)
                    {
                        for (int i = 0; i < dsReturn.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                privileges = dsReturn.Tables[0].Rows[i]["PRIVILEGE_CODE"].ToString();
                            }
                            else
                            {
                                privileges = privileges + "," + dsReturn.Tables[0].Rows[i]["PRIVILEGE_CODE"].ToString();
                            }
                        }
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
            return privileges;
        }
        /// <summary>
        /// 获取用户拥有权限的线别名称信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 使用,分割的线别名称字符串。
        /// </returns>
        public string GetLineOfUser(string userKey, string roomName)
        {
            string lines = "";
            try
            {
                IServerObjFactory serverObjFactory = CallRemotingService.GetRemoteObject();
                if (null != serverObjFactory)
                {
                    DataSet dsLines = new DataSet();
                    dsLines = serverObjFactory.CreateIUserEngine().GetLineOfUser(userKey, roomName);
                    if (dsLines.Tables.Count > 0)
                    {
                        if (dsLines.Tables.Contains(RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dataTable=dsLines.Tables[RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME];
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    lines = dataTable.Rows[i][RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME].ToString();
                                }
                                else
                                {
                                    lines = lines + "," + dataTable.Rows[i][RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME].ToString();
                                }
                            }
                        }
                    }
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
                }
            }
            catch(Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return lines;
        }

        public DataSet  GetLineDataSetOfUser()
        {
            DataSet dsLines = new DataSet();
            try
            {
                IServerObjFactory serverObjFactory = CallRemotingService.GetRemoteObject();
                if (null != serverObjFactory)
                {
                    dsLines = serverObjFactory.CreateIUserEngine().GetLineOfUser(_userKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsLines);
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
            return dsLines;
        }
        /// <summary>
        /// 获取用户拥有权限的工序名称。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 使用,分割的工序名称字符串。
        /// </returns>
        public string GetOperationOfUser(string userKey)
        {
            string operations = "";
            try
            {
                IServerObjFactory serverObjFactory = CallRemotingService.GetRemoteObject();
                if (null != serverObjFactory)
                {
                    DataSet dsOperation = new DataSet();
                    dsOperation = serverObjFactory.CreateIUserEngine().GetOperationOfUser(userKey);
                    if (dsOperation.Tables.Count > 0)
                    {
                        if (dsOperation.Tables.Contains(RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dataTable = dsOperation.Tables[RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME];
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    operations = dataTable.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString();
                                }
                                else
                                {
                                    operations = operations + "," + dataTable.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString();
                                }
                            }
                        }
                    }
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsOperation);
                }
            }
            catch(Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return operations;
        }
        /// <summary>
        /// 获取用户拥有权限的线上仓信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 使用,分割的线上仓名称字符串。
        /// </returns>
        public string GetStoreOfUser(string userKey,string roomName)
        {
            string stores = "";
            try
            {
                IServerObjFactory serverObjFactory = CallRemotingService.GetRemoteObject();
                if (null != serverObjFactory)
                {
                    DataSet dsStore = new DataSet();
                    dsStore = serverObjFactory.CreateIUserEngine().GetStoreOfUser(userKey,roomName);
                    if (dsStore.Tables.Count > 0)
                    {
                        if (dsStore.Tables.Contains(RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME))
                        {
                            DataTable dataTable = dsStore.Tables[RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME];
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    stores = dataTable.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                                }
                                else
                                {
                                    stores = stores + "," + dataTable.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                                }
                            }
                        }
                    }
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsStore);
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
            return stores;
        }

        public bool ChangePassword(DataTable table)
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = obj.CreateIUserEngine().ChangePassword(table);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length < 1)
                {
                    return true;
                }
                else
                {
                    MessageService.ShowMessage(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }

        public bool CheckOperator(string badge)
        {
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                DataSet ds = obj.CreateIUserEngine().CheckOperator(badge);
                if (ds != null && ds.ExtendedProperties.ContainsKey(PARAMETERS.OUTPUT_MESSAGE))
                {
                    _errorMsg = ds.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE].ToString();
                }
                if (_errorMsg.Length == 0)
                {
                    return true;
                }
                else
                {
                    MessageService.ShowError("验证员工号出错！" + _errorMsg);
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
            return false;
        }

        #endregion

        /// <summary>
        /// 记录用户登录信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="site">站点。</param>
        /// <param name="language">语言。</param>
        /// <param name="version">系统版本号。</param>
        public void LogUserLoginInfo(string userKey, string site, string language, string version)
        {
            string preLoginLogKey = PropertyService.Get(PROPERTY_FIELDS.LOGIN_LOG_KEY);

            DataTable loginLogDataTable = RBAC_LOGIN_LOG_FIELDS.CreateDataTable();

            DataRow dataRow = loginLogDataTable.NewRow();

            string loginLogKey =  CommonUtils.GenerateNewKey(0);

            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_LOGIN_LOG_KEY] = loginLogKey;
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_USER_KEY] = userKey;
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_SITE] = site;
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_LANGUAGE] = language;
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_COMPUTER_NAME] = Environment.MachineName.ToString();
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_COMPUTER_IP] = FanHai.Hemera.Utils.Common.Utils.GetLocationIPAddress();
            dataRow[RBAC_LOGIN_LOG_FIELDS.FIELD_VERSION] = version;

            loginLogDataTable.Rows.Add(dataRow);

            loginLogDataTable.AcceptChanges();

            DataSet reqDS = new DataSet();

            reqDS.Tables.Add(loginLogDataTable);

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                IUserEngine userEngine =null;
                if (serverFactory != null){
                    userEngine=serverFactory.CreateIUserEngine();
                }
                if (userEngine!=null && !string.IsNullOrEmpty(preLoginLogKey))
                {
                    userEngine.LogUserLogoutInfo(preLoginLogKey);
                }

                if (userEngine != null && userEngine.LogUserLoginInfo(reqDS))
                {
                    PropertyService.Set(PROPERTY_FIELDS.LOGIN_LOG_KEY, loginLogKey);
                }
            }
            catch
            {
                //TODO: Ignore Generant Exception.
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 记录用户登出信息。
        /// </summary>
        public void LogUserLogoutInfo()
        {
            string loginLogKey = PropertyService.Get(PROPERTY_FIELDS.LOGIN_LOG_KEY);

            if (!string.IsNullOrEmpty(loginLogKey))
            {
                try
                {
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    if (serverFactory != null)
                    {
                        serverFactory.CreateIUserEngine().LogUserLogoutInfo(loginLogKey);
                    }
                }
                catch
                {
                    //TODO: Ignore Generant Exception.
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }
        }

        public bool CheckLastPackageOperator()
        {
            bool blBack = false;
            string badge = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            if (!string.IsNullOrEmpty(badge))
            {
                try
                {
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    DataSet dsReturn = serverFactory.CreateIUserEngine().CheckLastPackageOperator(badge);
                    if (dsReturn.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME)) 
                    {
                        DataTable dtUser = dsReturn.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                        if (dtUser.Rows.Count > 0)
                        {
                            blBack = true;
                        }
                    }
                }
                catch
                {
                    //TODO: Ignore Generant Exception.
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }
            return blBack;           
        }
    }
}

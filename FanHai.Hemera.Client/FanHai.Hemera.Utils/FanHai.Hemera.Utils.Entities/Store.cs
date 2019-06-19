//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭                 2012-02-10            添加注释 
// =================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Utils.Entities
{
    public class Store:EntityObject
    {       

        #region Properties 
        private string _storeName = "";
        private string _storeKey = "";
        private string _storeType = "";
        private string _objectStatus = "";
        private string _locationKey = "";
        private string _description = "";
        private string _errorMsg = "";
        private string _typeName = "";
        private string _operationName = "";
        private string _requestFlag = "";
        #endregion
       
        #region define Attribue
        public string StoreKey
        {
            get { return _storeKey; }
            set { _storeKey = value; }
        }
        public string StoreName
        {
            get 
            { 
                return this._storeName;
            }
            set 
            { 
                this._storeName = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_STORE_NAME,value);
            }
        }
        public string StoreType
        {
            get 
            { 
                return _storeType;
            }
            set 
            { 
                _storeType = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_STORE_TYPE,value);
            }
        }
        public string TypeName
        {
            get 
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
                //ValidateDirtyList(WST_STORE_FIELDS.FIELD_TYPE_NAME, value);
            }
        }
        public string ObjectStatus
        {
            get
            {
                return this._objectStatus;
            }
            set
            {
                this._objectStatus = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_OBJECT_STATUS,value);
            }
        }
        public string LocationKey
        {
            get
            {
                return this._locationKey;
            }
            set
            {
                this._locationKey = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_LOCATION_KEY,value);
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
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_DESCRIPTION,value);
            }
        }

        public string OperationName
        {
            get { return this._operationName; }
            set
            {
                this._operationName = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_OPERATION_NAME, value);
            }
        }
        public string RequestFlag
        {
            get { return this._requestFlag; }
            set
            {
                this._requestFlag = value;
                ValidateDirtyList(WST_STORE_FIELDS.FIELD_REQUEST_FLAG, value);
            }
        }
        public string ErrorMsg
        {
            get { return this._errorMsg; }
            set { this._errorMsg = value; }
        }
        #endregion

        #region Action
        #region Insert
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            DataTable storeTable = DataTableHelper.CreateDataTableForInsertStore();
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                     {WST_STORE_FIELDS.FIELD_CREATOR,Creator},
                                                     {WST_STORE_FIELDS.FIELD_CREATE_TIMEZONE,CreateTimeZone},
                                                     {WST_STORE_FIELDS.FIELD_DESCRIPTION,_description},
                                                     {WST_STORE_FIELDS.FIELD_LOCATION_KEY,_locationKey},
                                                     {WST_STORE_FIELDS.FIELD_OBJECT_STATUS,_objectStatus},
                                                     {WST_STORE_FIELDS.FIELD_STORE_KEY,_storeKey},
                                                     {WST_STORE_FIELDS.FIELD_STORE_NAME,_storeName},
                                                     {WST_STORE_FIELDS.FIELD_STORE_TYPE,_storeType},
                                                     {WST_STORE_FIELDS.FIELD_TYPE_NAME,_typeName},
                                                     {WST_STORE_FIELDS.FIELD_OPERATION_NAME,_operationName},
                                                     {WST_STORE_FIELDS.FIELD_REQUEST_FLAG,_requestFlag},
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref storeTable, dataRow);
            dataSet.Tables.Add(storeTable);
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIStoreEngine().AddStore(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg != "")
                    {
                        MessageService.ShowError(_errorMsg);
                        return false;
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

        #region Update
        public override bool Update()
        {
            //if (IsDirty)
            //{
                DataSet dataSet = new DataSet();
                if (this.DirtyList.Count > 1)
                {
                    DataTable storeTable = DataTableHelper.CreateDataTableForUpdateBasicData(WST_STORE_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _storeKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref storeTable, rowData);
                    }
                    if (storeTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(storeTable);
                    }
                    try
                    {
                        DataSet dsReturn = null;
                        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                        if (null != serverFactory)
                        {
                            dsReturn = serverFactory.CreateIStoreEngine().UpdateStore(dataSet);
                            _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                            if (_errorMsg != "")
                            {
                                if (_errorMsg == COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP)
                                {
                                    //提示“数据已经被修改，请刷新后再操作！”信息
                                    MessageService.ShowWarning("${res:Global.RecordExpired}");
                                }
                                else
                                {
                                    MessageService.ShowError(_errorMsg);
                                }
                                return false;
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
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}", "${res:Global.SystemInfo}");
                }
            //}
            //else
            //{
            //    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}", "${res:Global.SystemInfo}");
            //}
            return true;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除方法
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            try
            {
                DataSet dsReturn = null;
                //调用远程对象
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //远程对象调用成功
                if (null != serverFactory)
                {
                    //调用远程对象的删除线边仓数据的方法，根据主键删除线边仓数据
                    dsReturn = serverFactory.CreateIStoreEngine().DeleteStore(_storeKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg != "")
                    {
                        MessageService.ShowError(_errorMsg);
                        return false;
                    }
                    else
                    {
                        this.ClearData();
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

        #region GetStore
        /// <summary>
        /// 根据线边仓主键，查出线边仓信息和区域名称
        /// </summary>
        /// <returns></returns>
        public DataSet GetStore()
        {
            DataSet dsReturn = new DataSet();           
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //调用远程对象的取得线边仓信息的方法
                    dsReturn = serverFactory.CreateIStoreEngine().GetStore(_storeKey);
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
        #endregion

    }
}

/*
<FileInfo>
  <Author>Hao.Zhang, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities;


namespace FanHai.Hemera.Utils.Entities
{
    public class LocationRet : SEntity
    {
        #region Construction
        public LocationRet()
        {

        }
        #endregion

        #region Private variable definition
        private string _parentLocationKey = "";           //父区域key  
        private string _parentLocationLevel = "";          //父区域类型
        private string _locationKey = "";           //区域key  
        private string _locationLevel = "";          //区域类型
        private string _errorMsg = "";

        public Dictionary<string, string> _dirtyList = new Dictionary<string, string>();
        #endregion

        #region Properties
        public string LocationKey
        {
            get { return _locationKey; }
            set { _locationKey = value; }
        }
        public string LocationLevel
        {
            get { return _locationLevel; }
            set { _locationLevel = value; }
        }
        public string ParentLocationKey
        {
            get { return _parentLocationKey; }
            set { _parentLocationKey = value; }
        }
        public string ParentLocationLevel
        {
            get { return _parentLocationLevel; }
            set { _parentLocationLevel = value; }
        }
        public string ErrorMsg
        {
            get { return this._errorMsg; }
            set { this._errorMsg = value; }
        }

        #endregion

        #region Actions

        //#region GetMaxVerRoute
        //public DataSet GetMaxVerRoute()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dsFrom = new DataSet();
        //    DataTable dataTable = new DataTable();
        //    Hashtable hashTable = new Hashtable();
        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            if (_routName == "")
        //            {
        //                dsReturn = serverFactory.CreateIRouteEngine().GetMaxVerRoute(null);
        //            }
        //            else
        //            {
        //                hashTable.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, _routName);
        //                dataTable = Utils.ConvertHashtableToDataTable(hashTable);
        //                dsFrom.Tables.Add(dataTable);
        //                dsReturn = serverFactory.CreateIRouteEngine().GetMaxVerRoute(dsFrom);
        //            }
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}
        //#endregion

        //#region GetEnterpriseInfoByName
        //public DataSet GetEnterpriseInfoByName()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dataSet = new DataSet();
        //    Hashtable RouteHashTable = new Hashtable();
        //    DataTable RouteTable = new DataTable();

        //    RouteHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, _enterpriseName);
        //    RouteTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(RouteHashTable);
        //    dataSet.Tables.Add(RouteTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {

        //            if (_enterpriseName != "")
        //            {
        //                dsReturn = serverFactory.CreateIEnterpriseEngine().GetEnterpriseInfo(dataSet); 
        //            }
        //            else
        //            {
        //                dsReturn = serverFactory.CreateIEnterpriseEngine().SearchEnterpriseInfo(null);
        //            }
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}
        //#endregion

        //#region GetEnterpriseInfo
        //public DataSet GetEnterpriseInfoByKey()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dataSet = new DataSet();
        //    Hashtable RouteHashTable = new Hashtable();
        //    DataTable RouteTable = new DataTable();

        //    RouteHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    RouteTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(RouteHashTable);
        //    dataSet.Tables.Add(RouteTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateIEnterpriseEngine().GetEnterprise(dataSet);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}
        //#endregion

        //#region update status of enterprise
        ///// <summary>
        ///// control status function
        ///// </summary>        
        //public override bool UpdateStatus()
        //{
        //    //get status of operation and put them into hashtable                    
        //    Hashtable enterpriseHashTable = new Hashtable();
        //    DataSet dataSet = new DataSet();

        //    // Add "Enterprise Data" 
        //    enterpriseHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    enterpriseHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS, this.Status);

        //    DataTable enterpriseDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(enterpriseHashTable);
        //    dataSet.Tables.Add(enterpriseDataTable);

        //    try
        //    {
        //        string msg = string.Empty;
        //        DataSet dsReturn = null;
        //        IServerObjFactory factor = CallRemotingService.GetRemoteObject();
        //        if (null != factor)
        //        {
        //            dsReturn = factor.CreateIEnterpriseEngine().UpdateEnterpriseStatus(dataSet);
        //            msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //            if (msg != string.Empty)
        //            {
        //                MessageService.ShowError(msg);
        //                return false;
        //            }
        //            else
        //            {
        //                MessageService.ShowMessage("设置状态成功!");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageService.ShowError(ex);
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return true;
        //}

        //#endregion

        //#region save basic enterprise informatin
        //public DataSet SaveNewEnterprise()
        //{
        //    DataSet DSReceveServer = new DataSet();
        //    Hashtable OperationHashTable = new Hashtable();
        //    DataSet dataSet = new DataSet();
        //    DataTable OperationTable = new DataTable();

        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, _enterpriseName);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION, _description);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS, base.Status);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_CREATE_TIME, base.CreateTime);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION, EnterpriseVersion);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_CREATOR, base.Creator);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_CREATE_TIMEZONE, base.CreateTimeZone);
        //    OperationTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(OperationHashTable);
        //    _dsPassValue.Tables.Add(OperationTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            DSReceveServer = serverFactory.CreateIEnterpriseEngine().AddEnterprise(_dsPassValue);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(DSReceveServer);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return DSReceveServer;
        //}
        //#endregion

        //#region AddEnterpriseVer
        //public DataSet AddEnterpriseVer()
        //{
        //    DataSet DSReceveServer = new DataSet();
        //    DataSet DSEnterpriseRouteReceve = new DataSet();
        //    Hashtable OperationHashTable = new Hashtable();
        //    DataSet dataSet = new DataSet();
        //    DataTable OperationTable = new DataTable();

        //    OperationHashTable.Add(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY, _routEnterpriseVerKey);
        //    OperationHashTable.Add(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ROUTE_VER_KEY, _routRouteVerKey);
        //    OperationHashTable.Add(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ, _routSeq);

        //    OperationTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(OperationHashTable);
        //    dataSet.Tables.Add(OperationTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            DSReceveServer = serverFactory.CreateIEnterpriseEngine().AddEnterpriseVer(dataSet);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(DSReceveServer);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return DSReceveServer;

        //}
        //#endregion

        //#region GetEnterpriseRouteInfo
        //public DataSet GetEnterpriseRouteInfo()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dataSet = new DataSet();
        //    Hashtable RouteHashTable = new Hashtable();
        //    DataTable RouteTable = new DataTable();

        //    RouteHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    RouteTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(RouteHashTable);
        //    dataSet.Tables.Add(RouteTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateIEnterpriseEngine().GetEnterpriseRouteInfo(dataSet);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}

        //#endregion

        //#region DeleteEnterprise
        ///// <summary>
        ///// DeleteEnterprise
        ///// </summary>
        //public DataSet DeleteEnterprise()
        //{
        //    Hashtable mainDataHashTable = new Hashtable();
        //    DataSet dataSet = new DataSet();
        //    DataSet dsReturn = new DataSet();
        //    if (_enterpriseKey != "")
        //    {
        //        mainDataHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //        DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
        //        mainDataTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
        //        dataSet.Tables.Add(mainDataTable);

        //        //Call Remoting Service
        //        try
        //        {
        //            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

        //            if (null != serverFactory)
        //            {
        //                dsReturn = serverFactory.CreateIEnterpriseEngine().DeleteEnterprise(dataSet);
        //                ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorMsg = ex.Message;
        //        }
        //        finally
        //        {
        //            CallRemotingService.UnregisterChannel();
        //        }
        //    }

        //    return dsReturn;
        //}
        //#endregion

        //#region dirtyList
        ///// <summary>
        ///// ValidateDirtyList
        ///// </summary>
        ///// <param name="key">column key</param>
        ///// <param name="newValue">new value</param>
        //private void ValidateDirtyList(string key, string newValue)
        //{
        //    if (_dirtyList.ContainsKey(key))
        //    {
        //        if (newValue == _dirtyList[key])
        //        {
        //            _dirtyList.Remove(key);
        //        }
        //        else
        //        {
        //            _dirtyList[key] = newValue;
        //        }
        //    }
        //    else
        //    {
        //        //string orginalValue = string.Empty;
        //        //switch (key)
        //        //{
        //        //    case POR_LOT_FIELDS.FIELD_SHIFT_NAME:
        //        //        orginalValue = _shiftName;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY:
        //        //        orginalValue = _curProductionLineKey;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_LINE_NAME:
        //        //        orginalValue = _lineName;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_PRIORITY:
        //        //        orginalValue = _priority;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY:
        //        //        orginalValue = _routeEnterpriseVerKey;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY:
        //        //        orginalValue = _curRouteVerKey;
        //        //        break;
        //        //    case POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY:
        //        //        orginalValue = _curStepVerKey;
        //        //        break;
        //        //    default:
        //        //        break;
        //        //}
        //        _dirtyList.Add(key, newValue);
        //    }
        //}
        //#endregion

        //#region GetEnterpriseRoute
        //public DataSet GetEnterpriseRoute()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dataSet = new DataSet();
        //    Hashtable RouteHashTable = new Hashtable();
        //    DataTable RouteTable = new DataTable();

        //    RouteHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    RouteTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(RouteHashTable);
        //    dataSet.Tables.Add(RouteTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateIEnterpriseEngine().GetEnterpriseRoute(dataSet);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}
        //#endregion

        //#region SearchRouteStep
        //public DataSet SearchRouteStep()
        //{
        //    DataSet dsReturn = new DataSet();
        //    DataSet dataSet = new DataSet();
        //    Hashtable RouteHashTable = new Hashtable();
        //    DataTable RouteTable = new DataTable();

        //    RouteHashTable.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, _routKey);
        //    RouteTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(RouteHashTable);
        //    dataSet.Tables.Add(RouteTable);

        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateIStepEngine().SearchRouteStep(dataSet);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;
        //}

        //#endregion
        //#region UpdateEnterprise
        //public DataSet UpdateEnterprise()
        //{
        //    DataSet dataSet = new DataSet();
        //    DataTable OperationTable = new DataTable();
        //    Hashtable OperationHashTable = new Hashtable();
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY, _enterpriseKey);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, _enterpriseName);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION, _description);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS, _enterpriseStatus);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION, EnterpriseVersion);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDITOR, base.Editor);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME, base.EditTime);
        //    OperationHashTable.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE, base.EditTimeZone);
        //    OperationTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(OperationHashTable);
        //    _dsPassValue.Tables.Add(OperationTable);

        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateIEnterpriseEngine().UpdateEnterprise(_dsPassValue);
        //            ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }

        //    return dsReturn;

        //}
        //#endregion

        #endregion


    }
}
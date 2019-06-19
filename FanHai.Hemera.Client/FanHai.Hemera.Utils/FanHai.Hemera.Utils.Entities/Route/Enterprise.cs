
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
    public class EnterpriseEntity:EntityObject
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public EnterpriseEntity()
        {
            _enterpriseKey =  CommonUtils.GenerateNewKey(0);
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        public EnterpriseEntity(string enterpriseKey)
        {
            _enterpriseKey = enterpriseKey;

            if (enterpriseKey.Length > 0)
            {
                GetEnterpriseByKey(enterpriseKey);
                this.IsInitializeFinished = true;
            }
        }
        /// <summary>
        /// Send insert data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable enterpriseTable = DataTableHelper.CreateDataTableForInsertEnterprise();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY,_enterpriseKey},
                                                        {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME,_enterpriseName},
                                                        {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION,_enterpriseDescription},
                                                        {POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS,
                                                            Convert.ToInt32(_enterpriseStatus).ToString()},
                                                        {COMMON_FIELDS.FIELD_COMMON_CREATOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                        {COMMON_FIELDS.FIELD_COMMON_EDITOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                    };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref enterpriseTable, dataRow);
            dataSet.Tables.Add(enterpriseTable);

            if (enterpriseList.Count > 0)
            {
                DataTable relationTable = DataTableHelper.CreateDataTableForInsertEpAndRtRelation();

                foreach (Enterprises enterprises in enterpriseList)
                {
                    enterprises.EnterpriseKey = _enterpriseKey;
                    enterprises.ParseInsertAndDeleteDataToDataTable(ref relationTable);
                }

                if (relationTable.Rows.Count > 0)
                {
                    dataSet.Tables.Add(relationTable);
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
                    dsReturn = factor.CreateIEnterpriseEngine().EnterpriseInsert(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        this.EnterpriseVersion = msg;
                        this.ResetDirtyList(); 
                        foreach (Enterprises enterprises in enterpriseList)
                        {
                            enterprises.IsInitializeFinished = true;
                        }
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
        /// <summary>
        /// Send update data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();
                if (DirtyList.Count > 1)
                {
                    DataTable enterpriseTable = 
                        DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME);

                    List<string> possibleDirtyFields = new List<string>(){ POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDITOR,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE };

                    string newValue = string.Empty;
                    foreach (string field in possibleDirtyFields)
                    {
                        if (this.DirtyList.ContainsKey(field))
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _enterpriseKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref enterpriseTable, rowData);
                        }
                    }
                    if (enterpriseTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(enterpriseTable);
                    }
                }

                if (enterpriseList.Count > 0)
                {
                    DataTable relationTable = DataTableHelper.CreateDataTableForInsertEpAndRtRelation();
                    DataTable relationUpdateTable = 
                        DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME + "_UPDATE");

                    ParseUpdateDataToDataTable(ref relationTable, ref relationUpdateTable);

                    if (relationTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(relationTable);
                    }
                    if (relationUpdateTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(relationUpdateTable);
                    }
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIEnterpriseEngine().EnterpriseUpdate(dataSet);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            this.ResetDirtyList();
                            foreach (Enterprises enterprises in enterpriseList)
                            {
                                enterprises.ResetDirtyList(); 
                            }
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
            }
            else
            {
                MessageService.ShowMessage
                    ("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
            }

            return true;
        }
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
                    dsReturn = factor.CreateIEnterpriseEngine().EnterpriseDelete(_enterpriseKey);
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
        /// <summary>
        /// Get enterprise via key
        /// </summary>
        /// <param name="enterpriseKey">enterpriseKey</param>
        public void GetEnterpriseByKey(string enterpriseKey)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIEnterpriseEngine().GetEnterpriseByKey(enterpriseKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        SetEnterpriseProperties(dsReturn.Tables[POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME]);
                        if (dsReturn.Tables[POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                        {
                            SetEpAndRtRelation(dsReturn.Tables[POR_ROUTE_EP_VER_R_VER_FIELDS.DATABASE_TABLE_NAME]);
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
        }
        /// <summary>
        /// Set enterprise properties
        /// </summary>
        /// <param name="dt">DataTable</param>
        private void SetEnterpriseProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0 && 1 == dt.Rows.Count)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        EnterpriseKey = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY].ToString();
                        EnterpriseName = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME].ToString();
                        EnterpriseDescription = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION].ToString();
                        EnterpriseVersion = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_VERSION].ToString();

                        Status = (EntityStatus)Convert.ToInt32(dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS]);
                        Editor = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDITOR].ToString();
                        EditTime = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME].ToString();
                        EditTimeZone = dataRow[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// Set enterprise and route relation
        /// </summary>
        /// <param name="dtAttribute">datatable for enterprise and route relation</param>
        private void SetEpAndRtRelation(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        Enterprises enterprises = new Enterprises();
                        enterprises.OperationAction = OperationAction.Update;
                        enterprises.RouteSqenceKey = dataRow[POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY].ToString();
                        enterprises.EnterpriseKey = dataRow[POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY].ToString();
                        enterprises.RouteKey = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
                        enterprises.RouteName = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString();
                        enterprises.RouteSeqence = dataRow[POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ].ToString();
                        enterprises.RouteDescription = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION].ToString();
                        enterprises.IsInitializeFinished = true;
                        enterpriseList.Add(enterprises);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// control status function
        /// </summary>        
        public override bool UpdateStatus()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();

                if (this.DirtyList.Count > 0)
                {
                    DataTable enterpriseTable = 
                        DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_ENTERPRISE_VER_FIELDS.DATABASE_TABLE_NAME);

                    List<string> possibleDirtyFields = new List<string>(){ POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDITOR,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIME,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_EDIT_TIMEZONE,
                                                                           POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS };

                    string newValue = string.Empty;
                    foreach (string field in possibleDirtyFields)
                    {
                        if (this.DirtyList.ContainsKey(field))
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _enterpriseKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref enterpriseTable, rowData);
                        }
                    }
                    if (enterpriseTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(enterpriseTable);
                    }
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIEnterpriseEngine().EnterpriseUpdate(dataSet);
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
        /// <summary>
        /// Get max version route
        /// </summary>
        public void GetMaxVerRoute(ref DataTable dataTable, DataSet ds)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIRouteEngine().GetMaxVerRoute(ds);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        dataTable = dsReturn.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME];
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
        /// <summary>
        /// Fill update data to table
        /// </summary>
        public void ParseUpdateDataToDataTable(ref DataTable relationTable, ref DataTable updateTable)
        {
            if (null == relationTable || null == updateTable || !IsDirty)
            {
                return;
            }
            foreach (Enterprises enterprises in enterpriseList)
            {
                if (OperationAction.None == enterprises.OperationAction || 
                    OperationAction.Update == enterprises.OperationAction)
                {
                    continue;
                }
                enterprises.EnterpriseKey = _enterpriseKey;
                if (OperationAction.Modified == enterprises.OperationAction)
                {
                    enterprises.ParseUpdateDataToDataTable(ref updateTable);
                }
                else
                {
                    enterprises.ParseInsertAndDeleteDataToDataTable(ref relationTable);
                }
            }
        }
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || enterpriseList.Count > 0);
            }
        }

        public string EnterpriseKey
        {
            get { return _enterpriseKey; }
            set { _enterpriseKey = value; }
        }

        public override EntityStatus Status
        {
            get 
            { 
                return _enterpriseStatus; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_STATUS, Convert.ToInt32(value).ToString());
                _enterpriseStatus = value; 
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
                ValidateDirtyList(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME, value);
                _enterpriseName = value; 
            }
        }

        public string EnterpriseDescription
        {
            get 
            { 
                return _enterpriseDescription;
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_DESCRIPTION, value);
                _enterpriseDescription = value; 
            }
        }

        public string EnterpriseVersion
        {
            get { return _enterpriseVersion; }
            set { _enterpriseVersion = value; }
        }

        public string EnterpriseFromVersion
        {
            get { return _enterpriseFromVersion; }
            set { _enterpriseFromVersion = value; }
        }

        public List<Enterprises> EnterpriseList
        {
            get { return enterpriseList; }
            set { enterpriseList = value; }
        }
        private string _enterpriseKey = string.Empty;         
        private string _enterpriseName = string.Empty;        
        private string _enterpriseVersion = string.Empty;     
        private string _enterpriseDescription = string.Empty; 
        private string _enterpriseFromVersion = string.Empty;

        private EntityStatus _enterpriseStatus = EntityStatus.InActive;               

        private List<Enterprises> enterpriseList = new List<Enterprises>();


    }

    public class Enterprises:SEntity
    {
        /// <summary>
        ///  No param construct
        /// </summary>
        public Enterprises()
        {
        }

        /// <summary>
        ///  One Route type param construct
        /// </summary>
        public Enterprises(DataRow dataRow)
        {
            _routeSqenceKey =  CommonUtils.GenerateNewKey(0);
            _routeKey = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
            _routeName = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString();
            _routeDescription = dataRow[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION].ToString();
        }
        /// <summary>
        /// Fill insert and delete data to table
        /// </summary>
        public void ParseInsertAndDeleteDataToDataTable(ref DataTable relationTable)
        {
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)_operationAction)},
                                                        {POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQUENCE_KEY,_routeSqenceKey},
                                                        {POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY,_enterpriseKey},
                                                        {POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ROUTE_VER_KEY,_routeKey},
                                                        {POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ,_routeSeqence}
                                                    };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref relationTable, dataRow);
        }
        /// <summary>
        /// Fill update data to table
        /// </summary>
        public void ParseUpdateDataToDataTable(ref DataTable updateTable)
        {
            if (null == updateTable || !IsDirty)
            {
                return;
            }

            List<string> possibleDirtyFields = new List<string>(){ POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY,
                                                                   POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ROUTE_VER_KEY,
                                                                   POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ };

            string newValue = string.Empty;
            foreach (string field in possibleDirtyFields)
            {
                if (this.DirtyList.ContainsKey(field))
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _routeSqenceKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref updateTable, rowData);
                }
            }
        }
        public string RouteSqenceKey
        {
            get { return _routeSqenceKey; }
            set { _routeSqenceKey = value; }
        }

        public string EnterpriseKey
        {
            get 
            { 
                return _enterpriseKey; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ENTERPRISE_VER_KEY, value);
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
                ValidateDirtyList(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_ROUTE_VER_KEY, value);
                _routeKey = value;
            }
        }

        public string RouteSeqence
        {
            get
            {
                return _routeSeqence;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_EP_VER_R_VER_FIELDS.FIELDS_ROUTE_SEQ, value);
                _routeSeqence = value;
            }
        }


        public string RouteName
        {
            get { return _routeName; }
            set { _routeName = value; }
        }

        public string RouteDescription
        {
            get { return _routeDescription; }
            set { _routeDescription = value; }
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
        private string _routeSqenceKey = string.Empty;
        private string _enterpriseKey = string.Empty;
        private string _routeKey = string.Empty;
        private string _routeName = string.Empty;
        private string _routeSeqence = string.Empty;
        private string _routeDescription = string.Empty;

        private OperationAction _operationAction = OperationAction.None;

    }
}




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

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 工艺流程实体类。
    /// </summary>
    public class RouteEntity:EntityObject
    {
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        private string _routeVerKey = string.Empty;
        /// <summary>
        /// 工艺流程名称。
        /// </summary>
        private string _routeName = string.Empty;
        /// <summary>
        /// 工艺流程描述。
        /// </summary>
        private string _routeDescription = string.Empty;
        /// <summary>
        /// 工艺流程版本。
        /// </summary>
        private string _routeVersion = string.Empty;
        /// <summary>
        /// 工艺流程有效期开始时间。
        /// </summary>
        private string _routeEffectivityStart = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 工艺流程有效期结束时间。
        /// </summary>
        private string _routeEffectivityEnd = "9999-12-31 23:59:59";
        /// <summary>
        /// 工艺流程状态。
        /// </summary>
        private EntityStatus _routeStatus = EntityStatus.InActive;
        /// <summary>
        /// 工艺流程中的工步列表。
        /// </summary>
        private List<StepEntity> stepList = new List<StepEntity>();
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get;
            private set;
        }
        /// <summary>
        /// 记录是否有修改。
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || stepList.Count > 0);
            }
        }
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public string RouteVerKey
        {
            get { return _routeVerKey; }
            set { _routeVerKey = value; }
        }
        /// <summary>
        /// 工艺流程名称。
        /// </summary>
        public string RouteName
        {
            get
            {
                return _routeName;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, value);
                _routeName = value;
            }
        }
        /// <summary>
        /// 工艺流程有效期开始时间。
        /// </summary>
        public string RouteEffectivityStart
        {
            get
            {
                return _routeEffectivityStart;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_START, value);
                _routeEffectivityStart = value;
            }
        }
        /// <summary>
        /// 工艺流程有效期结束时间。
        /// </summary>
        public string RouteEffectivityEnd
        {
            get
            {
                return _routeEffectivityEnd;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_END, value);
                _routeEffectivityEnd = value;
            }
        }
        /// <summary>
        /// 工艺流程描述。
        /// </summary>
        public string RouteDescription
        {
            get
            {
                return _routeDescription;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION, value);
                _routeDescription = value;
            }
        }
        /// <summary>
        /// 工艺流程版本号。
        /// </summary>
        public string RouteVersion
        {
            get
            {
                return _routeVersion;
            }
            set
            {
                _routeVersion = value;
            }
        }
        /// <summary>
        /// 状态。
        /// </summary>
        public override EntityStatus Status
        {
            get
            {
                return _routeStatus;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_STATUS, Convert.ToInt32(value).ToString());
                _routeStatus = value;
            }
        }
        /// <summary>
        /// 工艺流程中包含的工步列表。
        /// </summary>
        public List<StepEntity> StepList
        {
            get { return stepList; }
            set { stepList = value; }
        }
 
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RouteEntity()
        {
            _routeVerKey =  CommonUtils.GenerateNewKey(0);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RouteEntity(string routeVerKey)
        {
            _routeVerKey = routeVerKey;
            if (routeVerKey.Length > 0)
            {
                GetRouteByKey(routeVerKey);
                this.IsInitializeFinished = true;
            }
        }
        /// <summary>
        /// 新增工艺流程。
        /// </summary>
        /// <returns>true：新增成功。false：新增失败。</returns>
        public override bool Insert()
        {
            DataSet dsParams = new DataSet();
            //组织工艺流程数据。
            DataTable dtRoute = CommonUtils.CreateDataTable(new POR_ROUTE_ROUTE_VER_FIELDS());
            DataRow drRoute = dtRoute.NewRow();
            dtRoute.Rows.Add(drRoute);
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY] = _routeVerKey;
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME] = _routeName;
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_START] = _routeEffectivityStart;
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_END] = _routeEffectivityEnd;
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION] = _routeDescription;
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_STATUS] = Convert.ToInt32(_routeStatus);
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_CREATE_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            drRoute[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);            
            dsParams.Tables.Add(dtRoute);
            //组织工步数据。
            if (StepList.Count > 0)
            {
                DataTable dtStep = CommonUtils.CreateDataTable(new POR_ROUTE_STEP_FIELDS());
                dtStep.Columns.Add(COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION);
                DataTable dtStepUda = DataTableHelper.CreateDataTableForUDA(POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME,                                                                                                      POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY);
                DataTable dtStepParam = CommonUtils.CreateDataTable(new POR_ROUTE_STEP_PARAM_FIELDS());
                foreach (StepEntity step in stepList)
                {
                    step.RouteVerKey = _routeVerKey;
                    step.ParseInsertAndDeleteDataToDataTable(ref dtStep, ref dtStepUda, ref dtStepParam);
                }
                if (dtStep.Rows.Count > 0)
                {
                    dsParams.Tables.Add(dtStep);
                    //组织工步自定义属性数据。
                    if (dtStepUda.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStepUda);
                    }
                    //组织工步参数数据。
                    if (dtStepParam.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStepParam);
                    }
                }
            }
            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIRouteEngine().RouteInsert(dsParams);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                if (code == -1)
                {
                    MessageService.ShowError(msg);
                    return false;
                }
                else
                {
                    this.RouteVersion = msg;
                    this.ResetDirtyList(); 
                    foreach (StepEntity step in stepList)
                    {
                        step.Params.AcceptChanges();
                        step.IsInitializeFinished =true;
                    }
                    MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
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
        /// 更新工艺流程。
        /// </summary>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dsParams = new DataSet();
                //组织工艺流程数据。
                if (this.DirtyList.Count > 1)
                {
                    DataTable routeTable = DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _routeVerKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref routeTable, rowData);
                    }
                    if (routeTable.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(routeTable);
                    }
                }
                //组织工步数据。
                if (StepList.Count > 0)
                {
                    //DataTable dtStep = DataTableHelper.CreateDataTableForInsertStep();
                    DataTable dtStep = CommonUtils.CreateDataTable(new POR_ROUTE_STEP_FIELDS());
                    dtStep.Columns.Add(COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION);
                    DataTable dtStepUpdate = DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE");
                    DataTable dtStepUda = DataTableHelper.CreateDataTableForUDA(POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME, 
                                                                                   POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY);
                    DataTable dtStepParam = CommonUtils.CreateDataTable(new POR_ROUTE_STEP_PARAM_FIELDS());

                    ParseUpdateDataToDataTable(ref dtStep, ref dtStepUpdate, ref dtStepUda,ref dtStepParam);
                    //组织工步数据。
                    if (dtStep.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStep);
                    }
                    //组织工步自定义属性数据。
                    if (dtStepUpdate.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStepUpdate);
                    }
                    //组织工步自定义属性数据。
                    if (dtStepUda.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStepUda);
                    }
                    //组织工步参数数据。
                    if (dtStepParam.Rows.Count > 0)
                    {
                        dsParams.Tables.Add(dtStepParam);
                    }
                }
                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    dsReturn = factor.CreateIRouteEngine().RouteUpdate(dsParams);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        this.ResetDirtyList();
                        foreach (StepEntity step in stepList)
                        {
                            foreach (DataRow dr in step.Params.Rows)
                            {
                                int isDeleted = Convert.ToInt32(dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED]);
                                if (isDeleted == 1)
                                {
                                    dr.Delete();
                                }
                            }
                            step.Params.AcceptChanges();
                            step.ResetDirtyList();
                        }
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
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
        /// 填充更新（包含修改、新增、删除数据）数据到数据表。
        /// </summary>
        /// <param name="stepTable">包含新增或删除工步数据的数据表对象。</param>
        /// <param name="stepUpdateTable">包含更新工步数据的数据表对象。</param>
        /// <param name="stepUDAs">包含工步自定义属性数据的数据表对象</param>
        /// <param name="stepUDAs">包含工步参数数据的数据表对象</param>
        private void ParseUpdateDataToDataTable(ref DataTable stepTable,
                                               ref DataTable stepUpdateTable,
                                               ref DataTable stepUDAs,
                                               ref DataTable stepParam)
        {
            if (null == stepTable || null == stepUDAs || !IsDirty)
            {
                return;
            }
            foreach (StepEntity step in stepList)
            {
                if (OperationAction.None == step.OperationAction || OperationAction.Update == step.OperationAction)
                {
                    continue;
                }
                if (OperationAction.Modified == step.OperationAction)
                {
                    step.ParseUpdateDataToDataTable(ref stepUpdateTable, ref stepUDAs,ref stepParam);
                }
                else
                {
                    step.RouteVerKey = _routeVerKey;
                    step.ParseInsertAndDeleteDataToDataTable(ref stepTable, ref stepUDAs, ref stepParam);
                }
            }
        }
        /// <summary>
        /// 更新工艺流程状态。
        /// </summary>        
        public override bool UpdateStatus()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();
                // Prepare Operation
                if (this.DirtyList.Count > 0)
                {
                    DataTable routeTable = DataTableHelper.CreateDataTableForUpdateBasicData(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME);

                    List<string> possibleDirtyFields = new List<string>()
                                              {POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDITOR,
                                               POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIME,
                                               POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIMEZONE,
                                               POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_STATUS};

                    string newValue = string.Empty;
                    foreach (string field in possibleDirtyFields)
                    {
                        if (this.DirtyList.ContainsKey(field))
                        {
                            Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _routeVerKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, field},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[field].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[field].FieldNewValue}
                                                    };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref routeTable, rowData);
                        }
                    }
                    if (routeTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(routeTable);
                    }
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIRouteEngine().RouteUpdate(dataSet);
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
        /// 删除工艺流程。
        /// </summary>
        /// <returns>true：删除成功。false：删除失败。</returns>
        public override bool Delete()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIRouteEngine().RouteDelete(_routeVerKey);
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
        /// 根据主键获取工艺流程信息。
        /// </summary>
        /// <param name="routeKey">工艺主键。</param>
        public void GetRouteByKey(string routeKey)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIRouteEngine().GetRouteByKey(routeKey);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    SetRouteProperties(dsReturn.Tables[POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME]);                        
                    if (dsReturn.Tables.Count > 1)
                    {
                        SetRouteStep(dsReturn);
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
        /// 设置工艺流程属性。
        /// </summary>
        /// <param name="dt">包含工艺流程信息的数据表对象。</param>
        private void SetRouteProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0 && 1 == dt.Rows.Count)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RouteVerKey = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
                        RouteName = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME].ToString();
                        RouteEffectivityStart = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_START].ToString();
                        RouteEffectivityEnd = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EFFECTIVITY_END].ToString();
                        RouteDescription = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_DESCRIPTION].ToString();
                        RouteVersion = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_VERSION].ToString();

                        Status = (EntityStatus)Convert.ToInt32(dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_STATUS]);
                        Editor = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDITOR].ToString();
                        EditTime = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIME].ToString();
                        EditTimeZone = dr[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// 设置工艺流程中的工步信息。
        /// </summary>
        /// <param name="ds">包含工步数据的数据集对象。</param>
        internal void SetRouteStep(DataSet ds)
        {
            DataTable dtStep = ds.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtStepUda = ds.Tables[POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtStepParam= ds.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME];
            try
            {
                foreach (DataRow drStep in dtStep.Rows)
                {
                    StepEntity step = new StepEntity();
                    step.OperationAction = OperationAction.Update;
                    step.StepKey = drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY].ToString();
                    step.RouteVerKey = drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
                    step.StepName = drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME].ToString();
                    step.OsDuration = drStep[POR_ROUTE_STEP_FIELDS.FIELD_DURATION].ToString();

                    step.OsDescription = drStep[POR_ROUTE_STEP_FIELDS.FIELD_DESCRIPTIONS].ToString();

                    step.StepSeqence = drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_SEQ].ToString();
                    step.RouteVerKey = drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY].ToString();
                    step.Editor = drStep[POR_ROUTE_STEP_FIELDS.FIELD_EDITOR].ToString();
                    step.EditTime = drStep[POR_ROUTE_STEP_FIELDS.FIELD_EDIT_TIME].ToString();
                    step.EditTimeZone = drStep[POR_ROUTE_STEP_FIELDS.FIELD_EDIT_TIMEZONE].ToString();

                    step.ScrapCodesKey = drStep[POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY].ToString();
                    step.ScrapCodesName = Convert.ToString(drStep["SCRAP_REASON_CODE_CATEGORY_NAME"]);
                    step.DefectCodesKey = drStep[POR_ROUTE_STEP_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY].ToString();
                    step.DefectCodesName = Convert.ToString(drStep["DEFECT_REASON_CODE_CATEGORY_NAME"]);

                    step.ParamCountPerRow = Convert.ToInt32(drStep[POR_ROUTE_STEP_FIELDS.FIELD_PARAM_COUNT_PER_ROW]);
                    step.ParamOrderType = (OperationParamOrderType)Convert.ToInt32(drStep[POR_ROUTE_STEP_FIELDS.FIELD_PARAM_ORDER_TYPE]);
                    //工序自定义数据。
                    if (dtStepUda!=null && dtStepUda.Rows.Count > 0)
                    {
                        DataRow[] dataRows = dtStepUda.Select(POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY + " = '" + step.StepKey + "'");
                        foreach (DataRow row in dataRows)
                        {
                            step.SetStepUDAs(row);
                        }
                    }
                    //工序参数。
                    if (dtStepParam != null)
                    {
                        step.Params = dtStepParam.Clone();
                        DataRow[] dataRows = dtStepParam.Select(POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_KEY + " = '" + step.StepKey + "'");
                        foreach (DataRow row in dataRows)
                        {
                            step.Params.ImportRow(row);
                        }
                        step.Params.AcceptChanges();
                    }
                    step.IsInitializeFinished = true;
                    stepList.Add(step);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// 获取工艺流程关联的线别信息。
        /// </summary>
        /// <returns>包含工艺流程和线别关联信息的数据表对象。</returns>
        public DataTable GetRouteLineRelation()
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                if (_routeVerKey.Length > 1)
                {
                    hashTable.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, _routeVerKey);
                }

                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataSet.Tables.Add(dataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRouteEngine().SearchRouteLine(dataSet);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                return dsReturn.Tables["POR_ROUTE_LINE"];
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return null;
        }
        /// <summary>
        /// 删除工艺流程关联的线别信息。
        /// </summary>
        /// <param name="strRouteKey">工艺流程主键。</param>
        /// <param name="strLineKey">线别主键。</param>
        /// <returns>true:删除成功。false：删除失败。</returns>
        public bool DeleteRouteLineRelation(string strRouteKey, string strLineKey)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                if (strRouteKey.Length > 1)
                {
                    hashTable.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY, strRouteKey);
                    hashTable.Add("PRODUCTION_LINE_KEY", strLineKey);
                }

                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataSet.Tables.Add(dataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIRouteEngine().DeleteRouteLine(dataSet);
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
                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return true;
        }
        

        /// <summary>
        /// 获取最大版本号的工序信息。
        /// </summary>
        /// <param name="list">包含工序实体类的列表。</param>
        /// <param name="dsQueryParams">包含查询条件的数据集对象。</param>
        public void GetMaxVerOperation(ref List<OperationEntity> list, DataSet dsQueryParams)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIOperationEngine().GetMaxVerOperation(dsQueryParams);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    AddOperationDataToList(dsReturn, ref list);
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
        /// 添加工序数据到列表中。
        /// </summary>
        /// <param name="dt">包含工序数据的数据集对象。</param>
        /// <param name="list">包含工序数据的列表。</param>
        private void AddOperationDataToList(DataSet ds, ref List<OperationEntity> list)
        {
            DataTable dtOperation = ds.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtOperationUda = ds.Tables[POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME];
            DataTable dtOperationParams = ds.Tables[POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME];
            list.Clear();
            try
            {
                foreach (DataRow dr in dtOperation.Rows)
                {
                    OperationEntity operation = new OperationEntity();
                    operation.OperationVerKey = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY].ToString();
                    operation.OperationName = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString();
                    operation.OperationVersion = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION].ToString();
                    operation.OsDuration = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION].ToString();
                    operation.OsDescription = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    operation.Status = (EntityStatus)Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS]);
                    operation.Editor = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDITOR].ToString();
                    operation.EditTime = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIME].ToString();

                    operation.ScrapCodesKey = 
                        dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY].ToString();
                    operation.ScrapCodesName = Convert.ToString(dr["SCRAP_REASON_CODE_CATEGORY_NAME"]);

                    operation.DefectCodesKey = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY].ToString();
                    operation.DefectCodesName = Convert.ToString(dr["DEFECT_REASON_CODE_CATEGORY_NAME"]);

                    operation.ParamCountPerRow = Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_COUNT_PER_ROW]);
                    operation.ParamOrderType = (OperationParamOrderType)Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_ORDER_TYPE]);
                    //如果存在自定义数据。
                    if (dtOperationUda!=null)
                    {
                        DataRow[] dataRows = dtOperationUda.Select
                            (POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY + " = '" + operation.OperationVerKey + "'");
                        foreach (DataRow row in dataRows)
                        {
                            operation.SetOperationUDAs(row);                              
                        }  
                    }
                    //如果存在工序参数。
                    if (dtOperationParams!=null)
                    {
                        DataRow[] dataRows = dtOperationParams.Select
                            (POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_OPERATION_VER_KEY + " = '" + operation.OperationVerKey + "'");
                        foreach (DataRow row in dataRows)
                        {
                            operation.Params.ImportRow(row);
                        }
                    }
                    list.Add(operation);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        
    }
}

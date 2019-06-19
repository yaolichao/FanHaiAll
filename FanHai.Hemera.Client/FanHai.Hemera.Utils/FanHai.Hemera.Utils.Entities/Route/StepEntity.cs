//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-5             修改
// =================================================================================
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 工步实体类。
    /// </summary>
    public class StepEntity : Osbase
    {
        /// <summary>
        /// 工步主键。
        /// </summary>
        private string _stepKey = string.Empty;
        /// <summary>
        /// 工步名称。
        /// </summary>
        private string _stepName = string.Empty;
        /// <summary>
        /// 工步类型。
        /// </summary>
        private string _stepType = string.Empty;
        /// <summary>
        /// 序号。
        /// </summary>
        private string _stepSeqence = string.Empty;
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        private string _routeVerKey = string.Empty;
        /// <summary>
        /// 工步版本。
        /// </summary>
        private string _stepVersion = string.Empty;
        /// <summary>
        /// 自定义属性。
        /// </summary>
        private UserDefinedAttrs _stepUDAs = new UserDefinedAttrs(POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY);
        /// <summary>
        /// 工步记录是否有修改。
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                int paramsChangedCount = 0;
                DataTable dt = this._params.GetChanges();
                if (dt != null)
                {
                    paramsChangedCount += dt.Rows.Count;
                }
                return (this.DirtyList.Count > 0 || _stepUDAs.IsDirty || paramsChangedCount > 0);
            }
        }
        /// <summary>
        /// 工步主键。
        /// </summary>
        public string StepKey
        {
            get { return _stepKey; }
            set { _stepKey = value; }
        }
        /// <summary>
        /// 工步名称。
        /// </summary>
        public string StepName
        {
            get
            {
                return _stepName;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME, value);
                _stepName = value;
            }
        }
        /// <summary>
        /// 工步类型。
        /// </summary>
        public string StepType
        {
            get
            {
                return _stepType;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_STEP_FIELDS.FIELD_STEP_TYPE, value);
                _stepType = value;
            }
        }
        /// <summary>
        /// 序号。
        /// </summary>
        public string StepSeqence
        {
            get
            {
                return _stepSeqence;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_SEQ, value);
                _stepSeqence = value;
            }
        }
        /// <summary>
        /// 版本号。
        /// </summary>
        public string StepVersion
        {
            get { return _stepVersion; }
            set { _stepVersion = value; }
        }
        /// <summary>
        /// 工艺流程主键。
        /// </summary>
        public string RouteVerKey
        {
            get
            {
                return _routeVerKey;
            }
            set
            {
                _routeVerKey = value;
            }
        }
        /// <summary>
        /// 自定义属性。
        /// </summary>
        public UserDefinedAttrs UserDefinedAttrs
        {
            get
            {
                return _stepUDAs;
            }
            set
            {
                _stepUDAs = value;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StepEntity()
        {

        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public StepEntity(OperationEntity operation)
        {
            _stepKey =  FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            _stepName = operation.OperationName;
            _stepVersion = operation.OperationVersion;
            _operationVerKey = operation.OperationVerKey;
            _osDuration = operation.OsDuration;
            _osDescription = operation.OsDescription;
            _scrapCodesKey = operation.ScrapCodesKey;
            _scrapCodesName = operation.ScrapCodesName;
            _defectCodesKey = operation.DefectCodesKey;
            _defectCodesName = operation.DefectCodesName;
            _paramOrderType = operation.ParamOrderType;
            _paramCountPerRow = operation.ParamCountPerRow;

            //_stepUDAs = operation.UserDefinedAttrs;
            _stepUDAs.UserDefinedAttrChangeKey(_stepKey);
            _stepUDAs.LinkedToItemColumnName = POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY;

            this.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
 
            POR_ROUTE_STEP_PARAM_FIELDS fileds = new POR_ROUTE_STEP_PARAM_FIELDS();
            this._params = CommonUtils.CreateDataTable(fileds);
            foreach (DataRow dr in operation.Params.Rows)
            {
                DataRow drStepParams = this._params.NewRow();
                this._params.Rows.Add(drStepParams);
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_FROM] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DATA_FROM];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_TYPE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DATA_TYPE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DC_TYPE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_DC_TYPE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = this.EditTimeZone;
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDITOR] = this.Editor;
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_DELETED] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_MUSTINPUT] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_MUSTINPUT];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_READONLY] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_MUSTINPUT];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_INDEX];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_KEY] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_KEY];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_NAME] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_PARAM_NAME];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_KEY] = this._stepKey;
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_ROUTE_STEP_PARAM_KEY] = CommonUtils.GenerateNewKey(0);
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_FAILED_MSG] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_VALIDATE_FAILED_MSG];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_FAILED_RULE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_VALIDATE_FAILED_RULE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_RULE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_VALIDATE_RULE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_CALCULATE_RULE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_CALCULATE_RULE];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_FIELD_LENGTH] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_FIELD_LENGTH];
                drStepParams[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_MAT_RULE] = dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_MAT_RULE];
            }

        }
        /// <summary>
        /// 更新工步数据。
        /// </summary>
        /// <returns>true:更新成功。false：更新失败。</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dsParams = new DataSet();

                DataTable dtStepUpdate = DataTableHelper.CreateDataTableForUpdateBasicData
                    (POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME + "_UPDATE");
                DataTable dtStepUda = DataTableHelper.CreateDataTableForUDA
                    (POR_ROUTE_STEP_ATTR_FIELDS.DATABASE_TABLE_NAME, POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY);
                DataTable dtStepParam = CommonUtils.CreateDataTable(new POR_ROUTE_STEP_PARAM_FIELDS());
                ParseUpdateDataToDataTable(ref dtStepUpdate, ref dtStepUda, ref dtStepParam);
                //组织工步数据
                if (dtStepUpdate.Rows.Count > 0)
                {
                    dsParams.Tables.Add(dtStepUpdate);
                }
                //组织工步自定义属性数据。
                if (dtStepUda.Rows.Count > 0)
                {
                    DealUdaTable(dtStepUda,"I");
                    dsParams.Tables.Add(dtStepUda);
                }
                //组织工步参数数据。
                if (dtStepParam.Rows.Count > 0)
                {
                    dsParams.Tables.Add(dtStepParam);
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
                        foreach (DataRow dr in this.Params.Rows)
                        {
                            int isDeleted = Convert.ToInt32(dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED]);
                            if (isDeleted == 1)
                            {
                                dr.Delete();
                            }
                        }
                        this.Params.AcceptChanges();
                        this.ResetDirtyList();
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
        /// 设置工步的自定义属性。
        /// </summary>
        /// <param name="dtAttribute">包含自定义属性数据的数据行。</param>
        internal void SetStepUDAs(DataRow dataRow)
        {
            try
            {
                string linkedToItemKey = dataRow[POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ROUTE_STEP_KEY].ToString();
                string attributeKey = dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString();
                string attributeName = dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString();
                string attributeValue = string.Empty;
                if (attributeName == COMMON_NAMES.LINKED_ITEM_EDC)
                {
                    attributeValue = ConvertEdcKeyOrName(dataRow[POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString(), "O");
                }
                else
                {
                    attributeValue = dataRow[POR_ROUTE_STEP_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                }
                UserDefinedAttr uda = new UserDefinedAttr(linkedToItemKey, attributeKey, attributeName, attributeValue, "");
                uda.DataType = dataRow[BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE].ToString();
                uda.OperationAction = OperationAction.Update;
                _stepUDAs.UserDefinedAttrAdd(uda);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// 填充要新增或删除的数据到数据表。
        /// </summary>
        /// <param name="stepUpdateTable">包含新增或删除工步数据的数据表。</param>
        /// <param name="stepUDAs">包含工步自定义属性新增或删除数据的数据表。</param>
        /// <param name="stepParams">包含工步参数更新数据的数据表。</param>
        internal void ParseInsertAndDeleteDataToDataTable(ref DataTable stepTable, ref DataTable stepUDAs,ref DataTable stepParams)
        {
            DataRow drStep=stepTable.NewRow();
            stepTable.Rows.Add(drStep);
            drStep[COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION]=Convert.ToInt32(_operationAction);
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY] = _stepKey;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME] = _stepName;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_STEP_VERSION] = _stepVersion;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_DURATION] = _osDuration;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_DESCRIPTIONS] = _osDescription;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY] = _operationVerKey;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_NAME] = _operationName;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY] = _routeVerKey;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_SEQ] = _stepSeqence;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_PARAM_ORDER_TYPE] = (int)_paramOrderType;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_PARAM_COUNT_PER_ROW] = _paramCountPerRow;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY] = _scrapCodesKey;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY] = _defectCodesKey;
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_CREATOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_CREATE_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            drStep[POR_ROUTE_STEP_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            _stepUDAs.ParseInsertDataToDataTable(ref stepUDAs);

            if (this.Params != null)
            {
                DataTable dtStepParamChange = this.Params.GetChanges();
                if (dtStepParamChange != null)
                {
                    stepParams.Merge(dtStepParamChange, false, MissingSchemaAction.Ignore);
                }
                foreach (DataRow dr in stepParams.Rows)
                {
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
            }
        }
        /// <summary>
        /// 填充要更新的数据到数据表。
        /// </summary>
        /// <param name="stepUpdateTable">包含工步更新数据的数据表。</param>
        /// <param name="stepUDAs">包含工步自定义属性更新数据的数据表。</param>
        /// <param name="stepParams">包含工步参数更新数据的数据表。</param>
        internal void ParseUpdateDataToDataTable(ref DataTable stepUpdateTable, ref DataTable stepUDAs, ref DataTable stepParams)
        {
            if (null == stepUpdateTable || null == stepUDAs || !IsDirty)
            {
                return;
            }

            foreach (string Key in this.DirtyList.Keys)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _stepKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref stepUpdateTable, rowData);
            }
            //组织工步自定义属性。
            if (_stepUDAs.IsDirty)
            {
                _stepUDAs.ParseUpdateDataToDataTable(ref stepUDAs);
            }
            //组织工步参数。
            if (this.Params != null)
            {
                DataTable dtStepParamChange = this.Params.GetChanges();
                if (dtStepParamChange != null)
                {
                    stepParams.Merge(dtStepParamChange, false, MissingSchemaAction.Ignore);
                }
                foreach (DataRow dr in stepParams.Rows)
                {
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    dr[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                }
            }
        }
    }
}

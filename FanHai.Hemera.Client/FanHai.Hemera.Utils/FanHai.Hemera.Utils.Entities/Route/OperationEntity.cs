//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-5             修改
// =================================================================================
#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 工序实体类。
    /// </summary>
    public class OperationEntity : Osbase 
    {
        private EntityStatus _operationStatus = EntityStatus.InActive;
        private UserDefinedAttrs _operationUDAs = new UserDefinedAttrs(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY);
        /// <summary>
        /// 排序序号。
        /// </summary>
        protected decimal _sortSequence= 900;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get;
            private set;
        }
        /// <summary>
        /// 工序记录是否有修改。
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                int paramsChangedCount = 0;
                DataTable dt=this._params.GetChanges();
                if(dt!=null){
                    paramsChangedCount += dt.Rows.Count;
                }
                return (this.DirtyList.Count > 0 || _operationUDAs.IsDirty || paramsChangedCount>0);
            }
        }
        /// <summary>
        /// 工序状态。
        /// </summary>
        public override EntityStatus Status
        {
            get
            {
                return _operationStatus;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS, Convert.ToInt32(value).ToString());
                _operationStatus = value;
            }
        }
        /// <summary>
        /// 工序版本号。
        /// </summary>
        public string OperationVersion
        {
            get
            {
                return _operationVersion;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION, value);
                _operationVersion = value;
            }
        }
        /// <summary>
        /// 工序排序序号。
        /// </summary>
        public decimal SortSequence
        {
            get
            {
                return _sortSequence;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SORT_SEQ, Convert.ToString(value));
                _sortSequence = value;
            }
        }
        /// <summary>
        /// 用户自定义属性。
        /// </summary>
        public UserDefinedAttrs UserDefinedAttrs
        {
            get
            {
                return _operationUDAs;
            }
            set
            {
                _operationUDAs = value;
            }
        }
        
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationEntity()
        {
            _operationVerKey =  CommonUtils.GenerateNewKey(0);
            POR_ROUTE_OPERATION_PARAM_FIELDS paramsFields = new POR_ROUTE_OPERATION_PARAM_FIELDS();
            this._params = CommonUtils.CreateDataTable(paramsFields);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OperationEntity(string operationKey)
        {
            _operationVerKey = operationKey;
            POR_ROUTE_OPERATION_PARAM_FIELDS paramsFields = new POR_ROUTE_OPERATION_PARAM_FIELDS();
            this._params = CommonUtils.CreateDataTable(paramsFields);
            if (operationKey.Length > 0)
            {
                GetOperationByKey(operationKey);
                this.IsInitializeFinished = true;
            }
        }
        /// <summary>
        /// 新增工序记录。
        /// </summary>
        /// <returns>true：新增成功。false：新增失败。</returns>
        public override bool Insert()
        {
            DataSet dsParams = new DataSet();
            POR_ROUTE_OPERATION_VER_FIELDS fields = new POR_ROUTE_OPERATION_VER_FIELDS();
            DataTable dtOperation = CommonUtils.CreateDataTable(fields);
            //组织工序数据。
            DataRow drOperation = dtOperation.NewRow();
            dtOperation.Rows.Add(drOperation);
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]=_operationVerKey;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME] = _operationName;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION] = _osDuration;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SORT_SEQ] = _sortSequence;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS] = Convert.ToInt32(_operationStatus);
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS] = _osDescription;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_ORDER_TYPE] = Convert.ToInt32(_paramOrderType);
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_COUNT_PER_ROW] = _paramCountPerRow;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY] = _scrapCodesKey;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY] = _defectCodesKey;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_CREATOR] = this.Creator;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDITOR] = this.Editor;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIMEZONE] = this.EditTimeZone;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_CREATE_TIMEZONE] = this.CreateTimeZone;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_CREATE_TIME] = DBNull.Value;
            drOperation[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
            dsParams.Tables.Add(dtOperation);
            //组织自定义属性数据。
            DataTable operationUdaTable = DataTableHelper.CreateDataTableForUDA(POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME,
                                                                                POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY);
            _operationUDAs.ParseInsertDataToDataTable(ref operationUdaTable);
            if (operationUdaTable.Rows.Count > 0)
            {
                //如果有LINED_EDC类型的，则将参数名称转换为参数主键存储到数据库中。
                DealUdaTable(operationUdaTable, "I");
                dsParams.Tables.Add(operationUdaTable);
            }
            //组织工序参数数据。
            DataTable dtOperationParams = this._params.GetChanges();
            if (dtOperationParams != null)
            {
                dtOperationParams.TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;
                foreach (DataRow dr in dtOperationParams.Rows)
                {
                    dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDITOR] = this.Editor;
                    dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                    dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = this.EditTimeZone;
                }
                dsParams.Tables.Add(dtOperationParams);
            }
            try
            {
                int code = 0;
                string msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIOperationEngine().OperationInsert(dsParams);
                msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                if (code == -1)
                {
                    this.ErrorMsg = msg;
                    MessageService.ShowError(msg);
                    return false;
                }
                else
                {
                    this.OperationVersion = msg;
                    foreach (UserDefinedAttr uda in _operationUDAs.UserDefinedAttrList)
                    {
                        uda.OperationAction = OperationAction.Update;
                    }
                    this._params.AcceptChanges();
                    this.ResetDirtyList();
                    MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }
        /// <summary>
        /// 更新工序记录。
        /// </summary>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dsParams = new DataSet();
                Hashtable htCommon = new Hashtable();
                htCommon.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIMEZONE, this.EditTimeZone);
                htCommon.Add(COMMON_FIELDS.FIELD_COMMON_EDITOR, this.Editor);
                htCommon.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, this.EditTime);
                DataTable dtCommon = CommonUtils.ParseToDataTable(htCommon);
                dtCommon.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                dsParams.Tables.Add(dtCommon);
                //组织工序数据
                if (this.DirtyList.Count > 0)
                {
                    Hashtable htOpeartion = new Hashtable();
                    htOpeartion.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, _operationVerKey);
                    foreach (string Key in this.DirtyList.Keys)
                    {
                        htOpeartion.Add(Key, this.DirtyList[Key].FieldNewValue);
                    }
                    DataTable dtOperation = CommonUtils.ParseToDataTable(htOpeartion);
                    dtOperation.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                    dsParams.Tables.Add(dtOperation);
                }
                //组织自定义属性。
                if (_operationUDAs.IsDirty)
                {
                    DataTable operationUdaTable = DataTableHelper.CreateDataTableForUDA
                        (POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME, POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY);
                    _operationUDAs.ParseUpdateDataToDataTable(ref operationUdaTable);
                    DealUdaTable(operationUdaTable, "I");
                    dsParams.Tables.Add(operationUdaTable);
                }
                //组织工序参数数据表。
                DataTable dtOperationParams = this._params.GetChanges();
                if (dtOperationParams != null)
                {
                    dtOperationParams.TableName = POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME;
                    foreach (DataRow dr in dtOperationParams.Rows)
                    {
                        dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDITOR] = this.Editor;
                        dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                        dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = this.EditTimeZone;
                    }
                    dsParams.Tables.Add(dtOperationParams);
                }
                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIOperationEngine().OperationUpdate(dsParams);
                        msg =ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            this.ErrorMsg = msg;
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            foreach (UserDefinedAttr uda in _operationUDAs.UserDefinedAttrList)
                            {
                                uda.OperationAction = OperationAction.Update;
                            }
                            foreach (DataRow dr in this._params.Rows)
                            {
                                int isDeleted=Convert.ToInt32(dr[POR_ROUTE_OPERATION_PARAM_FIELDS.FIELD_IS_DELETED]);
                                if (isDeleted == 1)
                                {
                                    dr.Delete();
                                }
                            }
                            this._params.AcceptChanges();
                            this.ResetDirtyList();
                            MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ErrorMsg = ex.Message;
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
        /// 更新工序状态。
        /// </summary>        
        public override bool UpdateStatus()
        {
            return Update();
        }
        /// <summary>
        /// 删除工序记录。
        /// </summary>
        /// <returns>true：删除成功。false：删除失败。</returns>
        public override bool Delete()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIOperationEngine().OperationDelete(_operationVerKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        this.ErrorMsg = msg;
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
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return true;
        }
        /// <summary>
        /// 根据主键获取工序记录。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        private void GetOperationByKey(string operationKey)
        {
            try
            {
                string msg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIOperationEngine().GetOperationByKey(operationKey);
                msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    this.ErrorMsg = msg;
                    MessageService.ShowError(msg);
                }
                else
                {
                    SetOperationProperties(dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME]);
                    
                    this._params = dsReturn.Tables[POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME];
                    if (null != dsReturn.Tables[POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME])
                    {
                        SetOperationUDAs(dsReturn.Tables[POR_ROUTE_OPERATION_ATTR_FIELDS.DATABASE_TABLE_NAME]);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 设置工序属性。
        /// </summary>
        /// <param name="dt">包含工序属性数据的数据表。</param>
        private void SetOperationProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0 && 1 == dt.Rows.Count)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OperationName = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString();
                        OperationVersion = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_VERSION].ToString();
                        OsDuration = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION].ToString();
                        
                        OsDescription = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS].ToString();
                        Status = (EntityStatus)Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_OPERATION_STATUS]);
                        SortSequence = Convert.ToDecimal(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SORT_SEQ]);
                        Editor = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDITOR].ToString();
                        EditTime = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIME].ToString();
                        EditTimeZone = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_EDIT_TIMEZONE].ToString();

                        ScrapCodesKey = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY].ToString();
                        ScrapCodesName = Convert.ToString(dr["SCRAP_REASON_CODE_CATEGORY_NAME"]);
                        DefectCodesKey = dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY].ToString();
                        DefectCodesName =Convert.ToString(dr["DEFECT_REASON_CODE_CATEGORY_NAME"]);

                        this.ParamCountPerRow=Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_COUNT_PER_ROW]);
                        this.ParamOrderType=(OperationParamOrderType)Convert.ToInt32(dr[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_ORDER_TYPE]);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// 设置工序自定义属性。
        /// </summary>
        /// <param name="dtAttribute">包含工序自定义属性数据的数据表。</param>
        internal void SetOperationUDAs(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string linkedToItemKey = dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY].ToString();
                        string attributeKey = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString();
                        string attributeName = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString();
                        string attributeValue = string.Empty;
                        if (attributeName == COMMON_NAMES.LINKED_ITEM_EDC)
                        {
                            attributeValue = ConvertEdcKeyOrName(dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString(), "O");
                        }
                        else
                        {
                            attributeValue = dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                        }
                        UserDefinedAttr uda = new UserDefinedAttr(linkedToItemKey, attributeKey, attributeName, attributeValue, "");
                        uda.DataType = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE].ToString();
                        uda.OperationAction = OperationAction.Update;
                        _operationUDAs.UserDefinedAttrAdd(uda);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
        }
        /// <summary>
        /// 设置工序自定义属性。
        /// </summary>
        /// <param name="dr">包含工序自定义属性数据的数据行。</param>
        internal void SetOperationUDAs(DataRow dr)
        {
            try
            {
                string linkedToItemKey = dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_OPERATION_VER_KEY].ToString();
                string attributeKey = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_KEY].ToString();
                string attributeName = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString();
                string attributeValue = dr[POR_ROUTE_OPERATION_ATTR_FIELDS.FIELD_ATTRIBUTE_VALUE].ToString();
                UserDefinedAttr uda = new UserDefinedAttr(linkedToItemKey, attributeKey, attributeName, attributeValue, "");
                uda.DataType = dr[BASE_ATTRIBUTE_FIELDS.FIELDS_DATA_TYPE].ToString();
                uda.OperationAction = OperationAction.Update;
                _operationUDAs.UserDefinedAttrAdd(uda);

            }
            catch (Exception ex)
            {
                this.ErrorMsg = ex.Message;
                MessageService.ShowError(ex);
            }
        }
       
       
    }
}

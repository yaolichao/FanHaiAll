//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-20            修改
// =================================================================================
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
    /// <summary>
    /// 原因代码实体类。
    /// </summary>
    public class ReasonCode : EntityObject
    {
        /// <summary>
        /// 主键。
        /// </summary>
        private string _codeKey = string.Empty;
        /// <summary>
        /// 名称。
        /// </summary>
        private string _codeName = string.Empty;
        /// <summary>
        /// 类型。
        /// </summary>
        private string _codeType = string.Empty;
        /// <summary>
        /// 分类。
        /// </summary>
        private string _codeClass = string.Empty;
        /// <summary>
        /// 描述。
        /// </summary>
        private string _codeDescriptions = string.Empty;
        /// <summary>
        /// 主键。
        /// </summary>
        public string CodeKey
        {
            get { return _codeKey; }
            set
            {
                _codeKey = value;
                ValidateDirtyList(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY, value);
            }
        }
        /// <summary>
        /// 名称。
        /// </summary>
        public string CodeName
        {
            get { return _codeName; }
            set
            {
                _codeName = value;
                ValidateDirtyList(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME, value);
            }
        }
        /// <summary>
        /// 类型。
        /// </summary>
        public string CodeType
        {
            get { return _codeType; }
            set
            {
                _codeType = value;
                ValidateDirtyList(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE, value);
            }
        }

        /// <summary>
        /// 分类。
        /// </summary>
        public string CodeClass
        {
            get { return _codeClass; }
            set
            {
                _codeClass = value;
                ValidateDirtyList(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS, value);
            }
        }
        /// <summary>
        /// 描述。
        /// </summary>
        public string CodeDescriptions
        {
            get { return _codeDescriptions; }
            set
            {
                _codeDescriptions = value;
                ValidateDirtyList(FMM_REASON_CODE_FIELDS.FIELD_DESCRIPTIONS, value);
            }
        }
        /// <summary>
        /// 数据是否有修改。
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0);
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReasonCode() { }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="codeKey">原因代码主键。</param>
        public ReasonCode(string codeKey)
        {
            _codeKey = codeKey;
        }
        /// <summary>
        /// 新增原因代码记录。
        /// </summary>
        /// <returns>true：成功；否则失败。</returns>
        public override bool Insert()
        {
            DataSet dsParams = new DataSet();
            
            DataTable dtCode = CommonUtils.CreateDataTable(new FMM_REASON_CODE_FIELDS());
            DataRow drCode = dtCode.NewRow();
            dtCode.Rows.Add(drCode);
            drCode[FMM_REASON_CODE_FIELDS.FIELD_DESCRIPTIONS] = this._codeDescriptions;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_EDIT_TIMEZONE] = this.EditTimeZone;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_EDITOR] = this.Editor;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS] = this._codeClass;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY] = this._codeKey;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME] = this._codeName;
            drCode[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE] = this._codeType;
            dsParams.Tables.Add(dtCode);
            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIReasonCodeEngine().ReasonCodeInsert(dsParams);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                if (code == -1)
                {
                    MessageService.ShowError(msg);
                    return false;
                }
                this.ResetDirtyList();
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
        /// 更新原因代码记录。
        /// </summary>
        /// <returns>true：成功；否则失败。</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dsParams = new DataSet();

                DataTable dtUpdate = DataTableHelper.CreateDataTableForUpdateBasicData(FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME);

                foreach (string Key in this.DirtyList.Keys)
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _codeKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dtUpdate, rowData);
                }
                if (dtUpdate.Rows.Count > 0)
                {
                    dsParams.Tables.Add(dtUpdate);
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    dsReturn = factor.CreateIReasonCodeEngine().ReasonCodeUpdate(dsParams);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    this.ResetDirtyList();
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
                MessageService.ShowMessage("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
            }
            return true;
        }
        /// <summary>
        /// 删除原因代码。
        /// </summary>
        /// <returns>true：成功；否则失败。</returns>
        public override bool Delete()
        {
            if (_codeKey != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        DataSet dsReturn = factor.CreateIReasonCodeEngine().DeleteReasonCode(_codeKey);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");  //系统提示删除成功
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }

            }
            return true;
        }
        /// <summary>
        /// 获取原因代码。
        /// </summary>
        /// <param name="paramTable">包含查询参数的数据表对象。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        public DataTable GetReasonCode(DataTable paramTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIReasonCodeEngine().GetReasonCode(paramTable);
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                    return null;
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
            return dsReturn.Tables[0];
        }
        /// <summary>
        /// 原因代码名称验证。
        /// </summary>
        /// <returns>true：通过；否则失败。</returns>
        public bool ReasonCodeNameValidate()
        {
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = factor.CreateIReasonCodeEngine().GetReasonCode(this._codeType, this._codeName,this._codeClass);
                string msg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(msg))
                {
                    MessageService.ShowError(msg);
                    return false;
                }
                if (dsReturn != null 
                    && dsReturn.Tables.Count > 0 
                    && dsReturn.Tables[0].Rows.Count > 0)
                {
                    return false;
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
    }
}

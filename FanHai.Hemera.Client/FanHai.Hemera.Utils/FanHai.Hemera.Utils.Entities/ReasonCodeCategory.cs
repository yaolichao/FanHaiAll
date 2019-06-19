/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// chao.pang           2012-02-28           添加注释 
// =================================================================================
#region using 
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
#endregion


namespace FanHai.Hemera.Utils.Entities
{
    public class ReasonCodeCategoryEntity : EntityObject
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
        /// Send insert data to server  新增代码组
        /// </summary>
        /// <returns>bool</returns>
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeCategoryTable = DataTableHelper.CreateDataTableForInsertCodeCategory();
            //为新增数据做准备
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY, _codeCategoryKey},
                                                        {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME, _codeCategoryName},
                                                        {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_DESCRIPTIONS, _codeCategoryDescriptions},
                                                        {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE, _codeCategoryType},
                                                        {FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                    };

            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref codeCategoryTable, dataRow);
            //将数据表添加到数据集中 
            dataSet.Tables.Add(codeCategoryTable);

            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    //调用新增方法返回数据集
                    dsReturn = factor.CreateIReasonCodeEngine().ReasonCodeCategoryInsert(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }

                    this.ResetDirtyList();
                    //系统提示保存成功 
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
        /// Send update data to server
        /// </summary>
        /// <returns>bool</returns>
        public override bool Update()
        {
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();

                DataTable codeCategoryTable = DataTableHelper.CreateDataTableForUpdateBasicData(FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME);

                foreach (string Key in this.DirtyList.Keys)
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _codeCategoryKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref codeCategoryTable, rowData);
                }

                if (codeCategoryTable.Rows.Count > 0)
                {
                    dataSet.Tables.Add(codeCategoryTable);
                }

                try
                {
                    string msg = string.Empty;
                    DataSet dsReturn = null;
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        dsReturn = factor.CreateIReasonCodeEngine().ReasonCodeCategoryUpdate(dataSet);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }

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
                MessageService.ShowMessage("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
            }

            return true;
        }
        //代码组管理删除操作 modi by chao.pang
        public override bool Delete()
        {
            if (_codeCategoryKey != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        DataSet dsReturn = factor.CreateIReasonCodeEngine().DeleteReasonCodeCategory(_codeCategoryKey);
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
        /// 获取原因代码类别。
        /// </summary>
        /// <param name="dtParam">
        /// 包含查询参数的数据集对象。REASON_CODE_CATEGORY_NAME和REASON_CODE_CATEGORY_TYPE。
        /// </param>
        /// <returns>包含原因代码类别信息的数据集对象。</returns>
        public DataTable GetReasonCodeCategory(DataTable dtParam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIReasonCodeEngine().GetReasonCodeCategory(dtParam);
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    _errorMsg = msg;
                    return null;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return null;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn.Tables[0];
        }

        /// <summary>
        /// 为<see cref="GetReasonCodeCategory"/>使用的查询参数创建数据表结构。
        /// </summary>
        /// <returns>
        /// 包含<see cref="GetReasonCodeCategory"/>方法使用的查询参数的数据表对象。
        /// “REASON_CODE_CATEGORY_NAME”和“REASON_CODE_CATEGORY_TYPE”。
        /// </returns>
        public DataTable GetReasonCodeCategoryParamTable(string name, string type)
        {
            Hashtable ht = new Hashtable();
            ht.Add(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME, name);
            ht.Add(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE, type);
            return FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(ht);
        }
        /// <summary>
        /// Validate reason code category name exist or not
        /// </summary>
        public bool CodeCategoryNameValidate()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIReasonCodeEngine().GetDistinctReasonCodeCategoryName();
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        foreach (DataRow dataRow in dsReturn.Tables[0].Rows)
                        {
                            if (_codeCategoryName == dataRow[FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME].ToString())
                                return false;
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

            return true;
        }

        public ReasonCodeCategoryEntity()
        {
        }


        public ReasonCodeCategoryEntity(string codeCategoryKey)
        {
            _codeCategoryKey = codeCategoryKey;
        }

        public string CodeCategoryKey
        {
            get { return _codeCategoryKey; }
            set
            {
                _codeCategoryKey = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY, value);
            }
        }
        public string CodeCategoryName
        {
            get { return _codeCategoryName; }
            set
            {
                _codeCategoryName = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME, value);
            }
        }

        public string CodeCategoryDescriptions
        {
            get { return _codeCategoryDescriptions; }
            set
            {
                _codeCategoryDescriptions = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_DESCRIPTIONS, value);
            }
        }

        public string CodeCategoryEditor
        {
            get { return _codeCategoryEditor; }
            set
            {
                _codeCategoryEditor = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDITOR, value);
            }
        }

        public string CodeCategoryEditTime
        {
            get { return _codeCategoryEditTime; }
            set
            {
                _codeCategoryEditTime = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_EDIT_TIME, value);
            }
        }

        public string CodeCategoryEditTimeZoneKey
        {
            get { return _codeCategoryEditTimeZoneKey; }
            set
            {
                _codeCategoryEditTimeZoneKey = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY, value);
            }
        }

        public string CodeCategoryType
        {
            get { return _codeCategoryType; }
            set
            {
                _codeCategoryType = value;
                ValidateDirtyList(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_TYPE, value);
            }
        }
        //Private variable definition

        private string _codeCategoryKey = string.Empty;
        private string _codeCategoryName = string.Empty;
        private string _codeCategoryDescriptions = string.Empty;
        private string _codeCategoryEditor = string.Empty;
        private string _codeCategoryEditTime = string.Empty;
        private string _codeCategoryEditTimeZoneKey = string.Empty;
        private string _codeCategoryType = string.Empty;
    }
}

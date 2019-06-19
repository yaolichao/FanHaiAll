//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-01-29            添加注释
// =================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示原因代码的对话框。
    /// </summary>
    public partial class ReasonCodeDialog : BaseDialog
    {
        private string categoryKey = string.Empty;
        private string categoryType = string.Empty;

        DataTable categoryInsertTable = CreateCategoryTable();
        DataTable categoryDeleteTable = CreateCategoryTable();

        public static DataTable CreateCategoryTable()
        {
            List<string> fields = new List<string>() { 
                                                        FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY,
                                                        FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY
                                                     };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(FMM_REASON_R_CATEGORY_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="codeCategory"></param>
        public ReasonCodeDialog(ReasonCodeCategoryEntity codeCategory)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ReasonCodeDialog.Title}"))
        {
            InitializeComponent();
            this.categoryKey = codeCategory.CodeCategoryKey;
            this.categoryType = codeCategory.CodeCategoryType;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReasonCodeDialog_Load(object sender, EventArgs e)
        {
            LoadResourceFileToUI();
            ListCategoryBind();
            ListCodeBind();
            BindReasonCodeClass();
        }
        /// <summary>
        /// 绑定原因代码分类数据到下拉列表数据
        /// </summary>
        public void BindReasonCodeClass()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            DataTable dtReturn = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);
            if (dtReturn != null)
            {
                dtReturn.DefaultView.Sort = "CODE ASC";
                dtReturn.Rows.InsertAt(dtReturn.NewRow(), 0);
                this.lueReasonCodeClass.Properties.DataSource = dtReturn;
                this.lueReasonCodeClass.Properties.ValueMember = "CODE";
                this.lueReasonCodeClass.Properties.DisplayMember = "NAME";
            }
        }
        /// <summary>
        /// 载入UI资源。
        /// </summary>
        private void LoadResourceFileToUI()
        {
            //this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
            //this.btnColse.Text = StringParser.Parse("${res:Global.CloseButtonText}");
        }
        /// <summary>
        /// 绑定已有的原因代码数据。
        /// </summary>
        public void ListCategoryBind()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            //远程调用 
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            if (serverFactory != null)
            {
                dsReturn = serverFactory.CreateIReasonCodeEngine().GetReasonCategory(categoryKey);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    lbxCategory.DataSource = dsReturn.Tables[FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME];
                }
            }
            CallRemotingService.UnregisterChannel();
        }
        /// <summary>
        /// 绑定原因代码。
        /// </summary>
        public void ListCodeBind()
        {
            string msg = string.Empty;
            DataSet dsReturn = new DataSet();
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            if (serverFactory != null)
            {
                dsReturn = serverFactory.CreateIReasonCodeEngine().GetReasonCodeNotExistCategory(categoryKey, categoryType);
                msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg != string.Empty)
                {
                    MessageService.ShowError(msg);
                }
                else
                {
                    lbxCode.DataSource = dsReturn.Tables[FMM_REASON_CODE_FIELDS.DATABASE_TABLE_NAME]; 
                }
            }
            CallRemotingService.UnregisterChannel();
        }
        /// <summary>
        /// 原因代码分类值改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueReasonCodeClass_EditValueChanged(object sender, EventArgs e)
        {
            string rcClass=Convert.ToString(this.lueReasonCodeClass.EditValue);
            DataTable dt = this.lbxCode.DataSource as DataTable;
            if (dt != null)
            {
                if (!string.IsNullOrEmpty(rcClass))
                {
                    dt.DefaultView.RowFilter = string.Format("{0}='{1}'", FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS, rcClass);
                }
                else
                {
                    dt.DefaultView.RowFilter = string.Empty;
                }
            }
        }
        /// <summary>
        /// 关闭窗体。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColse_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 左移按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveToLeft_Click(object sender, EventArgs e)
        {
            if(this.lbxCode.SelectedItems.Count<=0)
            {//提示至少选择一条数据
                MessageService.ShowWarning("${res:Global.SelectedOneRecMessage}");
            }
            else
            {
                foreach (DataRowView item in this.lbxCode.SelectedItems)
                {
                    DataTable dtCategory = (DataTable)this.lbxCategory.DataSource;
                    dtCategory.ImportRow(item.Row);

                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY, categoryKey},
                                                                {FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY,item.Row.ItemArray[0].ToString()}
                                                            };

                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref categoryInsertTable, rowData);

                    for (int i = 0; i < categoryDeleteTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < categoryInsertTable.Rows.Count; j++)
                        {
                            if (categoryDeleteTable.Rows[i][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString() == categoryInsertTable.Rows[j][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString())
                            {
                                categoryInsertTable.Rows.Remove(categoryInsertTable.Rows[j]);
                            }
                        }

                        if (categoryDeleteTable.Rows[i][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString() == item.Row.ItemArray[0].ToString())
                        {
                            categoryDeleteTable.Rows.Remove(categoryDeleteTable.Rows[i]);
                        }
                    }
                }

                DataTable dtCode = (DataTable)this.lbxCode.DataSource;
                SelectedItemClear(lbxCode, dtCode);
            }
        }
        /// <summary>
        /// 右移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveToRight_Click(object sender, EventArgs e)
        {
            if (this.lbxCategory.SelectedItems.Count <= 0)
            {
                MessageService.ShowWarning("${res:Global.SelectedOneRecMessage}");
            }
            else
            {
                foreach (DataRowView item in this.lbxCategory.SelectedItems)
                {
                    DataTable dtCode = (DataTable)this.lbxCode.DataSource;
                    dtCode.ImportRow(item.Row);

                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {FMM_REASON_R_CATEGORY_FIELDS.FIELD_CATEGORY_KEY, categoryKey},
                                                                {FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY,item.Row.ItemArray[0].ToString()}
                                                            };

                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref categoryDeleteTable, rowData);

                    for (int i = 0; i < categoryInsertTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < categoryDeleteTable.Rows.Count; j++)
                        {
                            if (categoryInsertTable.Rows[i][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString() == categoryDeleteTable.Rows[j][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString())
                            {
                                categoryDeleteTable.Rows.Remove(categoryDeleteTable.Rows[j]);
                            }
                        }

                        if (categoryInsertTable.Rows[i][FMM_REASON_R_CATEGORY_FIELDS.FIELD_REASON_CODE_KEY].ToString() == item.Row.ItemArray[0].ToString())
                        {
                            categoryInsertTable.Rows.Remove(categoryInsertTable.Rows[i]);
                        }
                    }
                }

                DataTable dtCategory = (DataTable)this.lbxCategory.DataSource;
                SelectedItemClear(lbxCategory, dtCategory);
            }
        }

        private void SelectedItemClear(ListBox lstBx, DataTable bindTable)
        {
            int[] selectedIndexs = new int[lstBx.SelectedItems.Count];
            for (int i = lstBx.Items.Count - 1, j = 0; i >= 0; i--)
            {
                if (lstBx.GetSelected(i))
                {
                    selectedIndexs[j++] = i;
                }
            }
            for (int i = 0; i < selectedIndexs.Length; i++)
            {
                bindTable.Rows.Remove(((DataRowView)lstBx.Items[selectedIndexs[i]]).Row);
            }
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {//提示确定要保存吗
                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (serverFactory != null)
                {
                    if (categoryInsertTable.Rows.Count > 0)
                    {//存在数据
                        DataSet dsInsertData = new DataSet();
                        dsInsertData.Tables.Add(categoryInsertTable);
                        //添加操作 
                        dsReturn = serverFactory.CreateIReasonCodeEngine().AddReasonCategory(dsInsertData);

                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                        }
                    }

                    if (categoryDeleteTable.Rows.Count > 0)
                    {//categoryDeleteTable存在数据
                        DataSet dsDeleteData = new DataSet();
                        dsDeleteData.Tables.Add(categoryDeleteTable);
                        //删除操作
                        dsReturn = serverFactory.CreateIReasonCodeEngine().DeleteReasonCategory(dsDeleteData);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                        }
                    }

                    if (msg == string.Empty)
                    {
                        //保存成功
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                    }

                }

                CallRemotingService.UnregisterChannel();
            }
        }
    }
}

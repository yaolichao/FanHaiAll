using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    /// <summary>
    /// 标签铭牌设置控件。
    /// </summary>
    public partial class PrintLabelCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 是否允许新增标签或铭牌数据。
        /// </summary>
        bool _isAllowNew = true;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public PrintLabelCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            GridViewHelper.SetGridView(gvResults);
            GridViewHelper.SetGridView(gvPrintLabelDataDetail);
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");//保存

            //lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.lbl.0001}");//标签铭牌设置
            gcolName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0001}");//名称
            gcolVersion.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0002}");//版本号
            gcolDataType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0003}");//类型
            gcolPrinterType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0004}");//打印机类型
            gcolIsValid.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0005}");//是否有效
            gcolProductModel.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0006}");//产品型号
            gcolCertificateType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0007}");//认证类型
            gcolPowersetType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0008}");//分档方式
            gcolCustCheck_Type.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.GridControl.0009}");//检验方式

        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="isAllowNew">是否允许新增。</param>
        public PrintLabelCtrl(string title, bool isAllowNew)
            : this()
        {
            this._isAllowNew = isAllowNew;
            //this.lblApplicationTitle.Text = title;
            this.gcResults.EmbeddedNavigator.Buttons.Append.Visible = isAllowNew;
            this.gcResults.EmbeddedNavigator.Buttons.Remove.Visible = isAllowNew;
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasicPrintLabelCtrl_Load(object sender, EventArgs e)
        {
            BindPrinterTypeData();
            BindProductModelData();
            BindLabelDataTypeData();
            BindCertificateTypeData();
            BindPowersetTypeData();
            BindLabelData();
            BindCustCheckTypeData();
        }
        /// <summary>
        /// 绑定标签或铭牌数据。
        /// </summary>
        private void BindLabelData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet dsLabelData = entity.GetPrintLabelData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.gcResults.DataSource = dsLabelData.Tables[0];
        }


        private void BindProductModelData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetProductModelData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.ricmbProductModel.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                this.ricmbProductModel.Items.Add(dr[0]);
            }

        }

        private void BindCertificateTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetCertificateTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.ricmbCertificateType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                this.ricmbCertificateType.Items.Add(dr[0]);
            }
        }

        private void BindPowersetTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetPowersetTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.ricmbPowersetType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                this.ricmbPowersetType.Items.Add(dr[0]);
            }
        }

        private void BindLabelDataTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetLabelDataTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.rilueDataType.DataSource = ds.Tables[0];
            this.rilueDataType.ValueMember = "CODE";
            this.rilueDataType.DisplayMember = "NAME";
            this.rilueDataType.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME"));
        }

        private void BindPrinterTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetPrinterTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.riluePrinterType.DataSource = ds.Tables[0];
            this.riluePrinterType.ValueMember = "CODE";
            this.riluePrinterType.DisplayMember = "NAME";
            this.riluePrinterType.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME"));
        }

        private void BindCustCheckTypeData()
        {
            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet ds = entity.GetCustCheckTypeData();
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.rilueCustCheckType.DataSource = ds.Tables[0];
            this.rilueCustCheckType.ValueMember = "CODE";
            this.rilueCustCheckType.DisplayMember = "NAME";
            this.rilueCustCheckType.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME"));
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.gvResults.State == GridState.Editing
                && this.gvResults.IsEditorFocused
                && this.gvResults.EditingValueModified)
            {
                this.gvResults.SetFocusedRowCellValue(this.gvResults.FocusedColumn, this.gvResults.EditingValue);
            }
            this.gvResults.UpdateCurrentRow();

            int rowIndex = this.gvResults.FocusedRowHandle;
            DataTable dt = this.gcResults.DataSource as DataTable;
            DataTable dtChanges = dt.GetChanges();
            if (dtChanges == null || dtChanges.Rows.Count <= 0)
            {
                this.tsbSave.Enabled = false;
                return;
            }
            var items = dtChanges.AsEnumerable();

            var ids = from item in items
                      where string.IsNullOrEmpty(Convert.ToString(item["LABEL_ID"]))
                      select item;
            foreach (var item in ids)
            {
                //MessageService.ShowMessage("ID 不能为空", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.gvResults.FocusedColumn = this.gcolId;
                this.gvResults.FocusedRowHandle = this.gvResults.GetRowHandle(dt.Rows.IndexOf(item));
                this.gvResults.ShowEditor();
                return;
            }

            var names = from item in items
                        where string.IsNullOrEmpty(Convert.ToString(item["LABEL_NAME"]))
                        select item;
            foreach (var item in names)
            {
                //MessageService.ShowMessage("名称 不能为空" ,"提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.gvResults.FocusedColumn = this.gcolName;
                this.gvResults.FocusedRowHandle = this.gvResults.GetRowHandle(dt.Rows.IndexOf(item));
                this.gvResults.ShowEditor();
                return;
            }

            var dataTypes = from item in items
                            where string.IsNullOrEmpty(Convert.ToString(item["DATA_TYPE"]))
                            select item;
            foreach (var item in dataTypes)
            {
                //MessageService.ShowMessage(string.Format("类型 不能为空"), "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.gvResults.FocusedColumn = this.gcolDataType;
                this.gvResults.FocusedRowHandle = this.gvResults.GetRowHandle(dt.Rows.IndexOf(item));
                this.gvResults.ShowEditor();
                return;
            }

            var printerTypes = from item in items
                               where string.IsNullOrEmpty(Convert.ToString(item["PRINTER_TYPE"]))
                               select item;
            foreach (var item in printerTypes)
            {
                //MessageService.ShowMessage(string.Format("打印机类型 不能为空"), "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.gvResults.FocusedColumn = this.gcolPrinterType;
                this.gvResults.FocusedRowHandle = this.gvResults.GetRowHandle(dt.Rows.IndexOf(item));
                this.gvResults.ShowEditor();
                return;
            }

            DataSet dsParams = new DataSet();
            DataTable dtParams = dtChanges;
            foreach (DataRow dr in items)
            {
                dr["CREATOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                dr["EDITOR"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            }
            dsParams.Tables.Add(dtParams);
            PrintLabelEntity entity = new PrintLabelEntity();
            entity.SavePrintLabelData(dsParams);

            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            BindLabelData();
            this.gvResults.FocusedRowHandle = rowIndex;
        }
        /// <summary>
        /// 关闭当前窗口。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }

        private void gvResults_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            DataRow dr = this.gvResults.GetDataRow(e.RowHandle);
            string labelId = Convert.ToString(dr["LABEL_ID"]);

            PrintLabelEntity entity = new PrintLabelEntity();
            DataSet dsLabelDetailData = entity.GetPrintLabelDetailData(labelId);
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            e.ChildList = dsLabelDetailData.Tables[0].DefaultView;
        }

        private void gvResults_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = false;
        }

        private void gvResults_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvResults_MasterRowGetRelationDisplayCaption(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            //e.RelationName = "修改历史";
            e.RelationName = StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0006}");
        }

        private void gvResults_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "MasterDetail";
        }
        private void gvResults_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            ColumnView view = sender as ColumnView;
            DataRow dr = view.GetDataRow(e.RowHandle);
            dr["VERSION_NO"] = 1;
            dr["IS_VALID"] = "Y";
        }

        private void gcResults_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                this.gvResults.FocusedColumn = this.gcolId;
                foreach (GridColumn column in this.gvResults.Columns)
                {
                    column.OptionsColumn.ReadOnly = false;
                }
            }
            else if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                int rowIndex = this.gvResults.FocusedRowHandle;
                if (rowIndex < 0)
                {
                    e.Handled = true;
                }
                DataRow dr = this.gvResults.GetDataRow(rowIndex);
                if (dr.RowState != DataRowState.Added)
                {
                    e.Handled = true;
                }
            }
        }

        private void gvResults_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int rowIndex = e.FocusedRowHandle;
            if (rowIndex < 0) return;
            SetGridViewFocusedCellReadonly(rowIndex);

            DataRow dr = this.gvResults.GetDataRow(rowIndex);
            if (dr != null && dr.RowState == DataRowState.Added)
            {
                this.gcResults.EmbeddedNavigator.Buttons.Remove.Visible = true;
            }
            else
            {
                this.gcResults.EmbeddedNavigator.Buttons.Remove.Visible = false;
            }
        }

        private void gvResults_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            int rowIndex = this.gvResults.FocusedRowHandle;
            if (rowIndex < 0) return;
            SetGridViewFocusedCellReadonly(rowIndex);
        }

        void SetGridViewFocusedCellReadonly(int rowIndex)
        {
            DataRow dr = this.gvResults.GetDataRow(rowIndex);
            if (dr.RowState == DataRowState.Added)
            {
                this.gvResults.FocusedColumn.OptionsColumn.ReadOnly = false;
            }
            else
            {
                if (this.gvResults.FocusedColumn == this.gcolIsValid
                    || this.gvResults.FocusedColumn == this.gcolCertificateType
                    || this.gvResults.FocusedColumn == this.gcolProductModel
                    || this.gvResults.FocusedColumn == this.gcolPowersetType)
                {
                    this.gvResults.FocusedColumn.OptionsColumn.ReadOnly = false;
                }
                else
                {
                    if (this.gvResults.FocusedColumn == this.gcolId
                        || this.gvResults.FocusedColumn == this.gcolVersion)
                    {
                        this.gvResults.FocusedColumn.OptionsColumn.ReadOnly = true;
                    }
                    else
                    {
                        this.gvResults.FocusedColumn.OptionsColumn.ReadOnly = !this._isAllowNew;
                    }
                }
            }
        }

        private void gvResults_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string val = Convert.ToString(e.Value);
            DataTable dt = this.gcResults.DataSource as DataTable;
            DataRow dr = this.gvResults.GetDataRow(e.RowHandle);
            if (dr == null) return;
            if (e.Column == this.gcolId)
            {
                if (dt != null && !string.IsNullOrEmpty(val))
                {
                    DataRow[] drs = dt.Select(string.Format("LABEL_ID='{0}'", val));
                    if (drs.Length > 0)
                    {
                        //MessageService.ShowMessage(string.Format("ID:{0} 已存在，请确认。", val), "提示");
                        MessageService.ShowMessage(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.PrintLabelCtrl.msg.0005}"),val), StringParser.Parse("${res:Global.SystemInfo}"));
                        dr[e.Column.FieldName] = string.Empty;
                        this.gvResults.FocusedColumn = e.Column;
                    }
                }
            }
            else if (e.Column == this.gcolIsValid)
            {
                if (val == "N")
                {
                    string labelId = Convert.ToString(dr["LABEL_ID"]);
                    PrintLabelEntity entity = new PrintLabelEntity();
                    if (!entity.IsAllowInvalid(labelId))
                    {
                        MessageService.ShowMessage(entity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
                        this.gvResults.FocusedColumn = e.Column;
                        this.gvResults.FocusedRowHandle = e.RowHandle;
                        dr[e.Column.FieldName] = "Y";
                    }
                }
            }
            if (dr.RowState == DataRowState.Modified)
            {
                bool rejectChanges = true;
                foreach (DataColumn col in dt.Columns)
                {
                    string original = Convert.ToString(dr[col, DataRowVersion.Original]);
                    string current = Convert.ToString(dr[col, DataRowVersion.Default]);
                    if (current != original)
                    {
                        rejectChanges = false;
                        break;
                    }
                }
                if (rejectChanges)
                {
                    dr.RejectChanges();
                }
            }
        }

        private void gvResults_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            DataRow dr = this.gvResults.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (dr.RowState == DataRowState.Modified)
                {
                    e.Appearance.BackColor = Color.GreenYellow;
                    e.Appearance.ForeColor = Color.White;
                }
            }
        }
        //添加筛选条件行
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

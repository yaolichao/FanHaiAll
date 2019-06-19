namespace FanHai.Hemera.Addins.BasicData.Gui
{
    partial class PrintLabelCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvPrintLabelDataDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolDetailLabelId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailLabelName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailVersionNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailIsValid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailProductModel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailCertificateType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailPowersetType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailCustCheck_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailCreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailCreator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDetailEditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcResults = new DevExpress.XtraGrid.GridControl();
            this.gvResults = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolVersion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueDataType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gcolPrinterType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riluePrinterType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gcolIsValid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.richkIsValid = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gcolProductModel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ricmbProductModel = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gcolCertificateType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ricmbCertificateType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gcolPowersetType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ricmbPowersetType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gcolCustCheck_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueCustCheckType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrintLabelDataDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riluePrinterType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richkIsValid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbProductModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbCertificateType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbPowersetType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueCustCheckType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(799, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(614, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-14 16:40:55";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(308, 23);
            this.lblMenu.Text = "打印管理->打印管理->标签铭牌设置";
            // 
            // gvPrintLabelDataDetail
            // 
            this.gvPrintLabelDataDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolDetailLabelId,
            this.gcolDetailLabelName,
            this.gcolDetailVersionNo,
            this.gcolDetailIsValid,
            this.gcolDetailProductModel,
            this.gcolDetailCertificateType,
            this.gcolDetailPowersetType,
            this.gcolDetailCustCheck_Type,
            this.gcolDetailCreateTime,
            this.gcolDetailCreator,
            this.gcolDetailEditTime,
            this.gcolDetailEditor});
            this.gvPrintLabelDataDetail.GridControl = this.gcResults;
            this.gvPrintLabelDataDetail.Name = "gvPrintLabelDataDetail";
            this.gvPrintLabelDataDetail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPrintLabelDataDetail.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvPrintLabelDataDetail.OptionsBehavior.ReadOnly = true;
            this.gvPrintLabelDataDetail.OptionsView.ShowGroupPanel = false;
            this.gvPrintLabelDataDetail.OptionsView.ShowIndicator = false;
            // 
            // gcolDetailLabelId
            // 
            this.gcolDetailLabelId.Caption = "ID";
            this.gcolDetailLabelId.FieldName = "LABEL_ID";
            this.gcolDetailLabelId.Name = "gcolDetailLabelId";
            this.gcolDetailLabelId.Visible = true;
            this.gcolDetailLabelId.VisibleIndex = 0;
            // 
            // gcolDetailLabelName
            // 
            this.gcolDetailLabelName.Caption = "名称";
            this.gcolDetailLabelName.FieldName = "LABEL_NAME";
            this.gcolDetailLabelName.Name = "gcolDetailLabelName";
            this.gcolDetailLabelName.Visible = true;
            this.gcolDetailLabelName.VisibleIndex = 1;
            // 
            // gcolDetailVersionNo
            // 
            this.gcolDetailVersionNo.Caption = "版本号";
            this.gcolDetailVersionNo.FieldName = "VERSION_NO";
            this.gcolDetailVersionNo.Name = "gcolDetailVersionNo";
            this.gcolDetailVersionNo.Visible = true;
            this.gcolDetailVersionNo.VisibleIndex = 2;
            // 
            // gcolDetailIsValid
            // 
            this.gcolDetailIsValid.Caption = "是否有效";
            this.gcolDetailIsValid.FieldName = "IS_VALID";
            this.gcolDetailIsValid.Name = "gcolDetailIsValid";
            this.gcolDetailIsValid.Visible = true;
            this.gcolDetailIsValid.VisibleIndex = 3;
            // 
            // gcolDetailProductModel
            // 
            this.gcolDetailProductModel.Caption = "产品型号";
            this.gcolDetailProductModel.FieldName = "PRODUCT_MODEL";
            this.gcolDetailProductModel.Name = "gcolDetailProductModel";
            this.gcolDetailProductModel.Visible = true;
            this.gcolDetailProductModel.VisibleIndex = 4;
            // 
            // gcolDetailCertificateType
            // 
            this.gcolDetailCertificateType.Caption = "认证类型";
            this.gcolDetailCertificateType.FieldName = "CERTIFICATE_TYPE";
            this.gcolDetailCertificateType.Name = "gcolDetailCertificateType";
            this.gcolDetailCertificateType.Visible = true;
            this.gcolDetailCertificateType.VisibleIndex = 5;
            // 
            // gcolDetailPowersetType
            // 
            this.gcolDetailPowersetType.Caption = "分档方式";
            this.gcolDetailPowersetType.FieldName = "POWERSET_TYPE";
            this.gcolDetailPowersetType.Name = "gcolDetailPowersetType";
            this.gcolDetailPowersetType.Visible = true;
            this.gcolDetailPowersetType.VisibleIndex = 7;
            // 
            // gcolDetailCustCheck_Type
            // 
            this.gcolDetailCustCheck_Type.Caption = "检验方式";
            this.gcolDetailCustCheck_Type.FieldName = "CUSTCHECK_TYPE";
            this.gcolDetailCustCheck_Type.Name = "gcolDetailCustCheck_Type";
            this.gcolDetailCustCheck_Type.Visible = true;
            this.gcolDetailCustCheck_Type.VisibleIndex = 6;
            // 
            // gcolDetailCreateTime
            // 
            this.gcolDetailCreateTime.Caption = "创建时间";
            this.gcolDetailCreateTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolDetailCreateTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolDetailCreateTime.FieldName = "CREATE_TIME";
            this.gcolDetailCreateTime.Name = "gcolDetailCreateTime";
            this.gcolDetailCreateTime.Visible = true;
            this.gcolDetailCreateTime.VisibleIndex = 8;
            // 
            // gcolDetailCreator
            // 
            this.gcolDetailCreator.Caption = "创建人";
            this.gcolDetailCreator.FieldName = "CREATOR";
            this.gcolDetailCreator.Name = "gcolDetailCreator";
            this.gcolDetailCreator.Visible = true;
            this.gcolDetailCreator.VisibleIndex = 9;
            // 
            // gcolDetailEditTime
            // 
            this.gcolDetailEditTime.Caption = "编辑时间";
            this.gcolDetailEditTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolDetailEditTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolDetailEditTime.FieldName = "EDIT_TIME";
            this.gcolDetailEditTime.Name = "gcolDetailEditTime";
            this.gcolDetailEditTime.Visible = true;
            this.gcolDetailEditTime.VisibleIndex = 10;
            // 
            // gcolDetailEditor
            // 
            this.gcolDetailEditor.Caption = "编辑人";
            this.gcolDetailEditor.FieldName = "EDITOR";
            this.gcolDetailEditor.Name = "gcolDetailEditor";
            this.gcolDetailEditor.Visible = true;
            this.gcolDetailEditor.VisibleIndex = 11;
            // 
            // gcResults
            // 
            this.gcResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcResults.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.White;
            this.gcResults.EmbeddedNavigator.Appearance.ForeColor = System.Drawing.Color.Black;
            this.gcResults.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcResults.EmbeddedNavigator.Appearance.Options.UseForeColor = true;
            this.gcResults.EmbeddedNavigator.Buttons.Append.Hint = "添加";
            this.gcResults.EmbeddedNavigator.Buttons.CancelEdit.Hint = "取消编辑";
            this.gcResults.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.Edit.Hint = "编辑";
            this.gcResults.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.EndEdit.Hint = "完成编辑";
            this.gcResults.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.First.Hint = "第一条";
            this.gcResults.EmbeddedNavigator.Buttons.Last.Hint = "最后一条";
            this.gcResults.EmbeddedNavigator.Buttons.Next.Hint = "下一条";
            this.gcResults.EmbeddedNavigator.Buttons.Prev.Hint = "上一条";
            this.gcResults.EmbeddedNavigator.Buttons.Remove.Hint = "删除";
            this.gcResults.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcResults.EmbeddedNavigator.TextStringFormat = " {0} / {1}";
            this.gcResults.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcResults_EmbeddedNavigator_ButtonClick);
            gridLevelNode1.LevelTemplate = this.gvPrintLabelDataDetail;
            gridLevelNode1.RelationName = "MasterDetail";
            this.gcResults.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcResults.Location = new System.Drawing.Point(1, 47);
            this.gcResults.MainView = this.gvResults;
            this.gcResults.Name = "gcResults";
            this.gcResults.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.richkIsValid,
            this.ricmbProductModel,
            this.ricmbCertificateType,
            this.ricmbPowersetType,
            this.rilueDataType,
            this.riluePrinterType,
            this.rilueCustCheckType});
            this.gcResults.Size = new System.Drawing.Size(799, 341);
            this.gcResults.TabIndex = 5;
            this.gcResults.UseEmbeddedNavigator = true;
            this.gcResults.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResults,
            this.gvPrintLabelDataDetail});
            // 
            // gvResults
            // 
            this.gvResults.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolId,
            this.gcolName,
            this.gcolVersion,
            this.gcolDataType,
            this.gcolPrinterType,
            this.gcolIsValid,
            this.gcolProductModel,
            this.gcolCertificateType,
            this.gcolPowersetType,
            this.gcolCustCheck_Type});
            this.gvResults.GridControl = this.gcResults;
            this.gvResults.Name = "gvResults";
            this.gvResults.OptionsView.EnableAppearanceEvenRow = true;
            this.gvResults.OptionsView.ShowGroupPanel = false;
            // 
            // gcolId
            // 
            this.gcolId.Caption = "ID";
            this.gcolId.FieldName = "LABEL_ID";
            this.gcolId.Name = "gcolId";
            this.gcolId.OptionsColumn.FixedWidth = true;
            this.gcolId.Visible = true;
            this.gcolId.VisibleIndex = 0;
            this.gcolId.Width = 100;
            // 
            // gcolName
            // 
            this.gcolName.Caption = "名称";
            this.gcolName.FieldName = "LABEL_NAME";
            this.gcolName.MinWidth = 100;
            this.gcolName.Name = "gcolName";
            this.gcolName.Visible = true;
            this.gcolName.VisibleIndex = 1;
            this.gcolName.Width = 100;
            // 
            // gcolVersion
            // 
            this.gcolVersion.Caption = "版本号";
            this.gcolVersion.FieldName = "VERSION_NO";
            this.gcolVersion.Name = "gcolVersion";
            this.gcolVersion.OptionsColumn.FixedWidth = true;
            this.gcolVersion.Visible = true;
            this.gcolVersion.VisibleIndex = 2;
            this.gcolVersion.Width = 70;
            // 
            // gcolDataType
            // 
            this.gcolDataType.Caption = "类型";
            this.gcolDataType.ColumnEdit = this.rilueDataType;
            this.gcolDataType.FieldName = "DATA_TYPE";
            this.gcolDataType.Name = "gcolDataType";
            this.gcolDataType.OptionsColumn.FixedWidth = true;
            this.gcolDataType.Visible = true;
            this.gcolDataType.VisibleIndex = 3;
            this.gcolDataType.Width = 100;
            // 
            // rilueDataType
            // 
            this.rilueDataType.AutoHeight = false;
            this.rilueDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueDataType.Name = "rilueDataType";
            this.rilueDataType.NullText = "";
            // 
            // gcolPrinterType
            // 
            this.gcolPrinterType.Caption = "打印机类型";
            this.gcolPrinterType.ColumnEdit = this.riluePrinterType;
            this.gcolPrinterType.FieldName = "PRINTER_TYPE";
            this.gcolPrinterType.Name = "gcolPrinterType";
            this.gcolPrinterType.OptionsColumn.FixedWidth = true;
            this.gcolPrinterType.Visible = true;
            this.gcolPrinterType.VisibleIndex = 4;
            this.gcolPrinterType.Width = 100;
            // 
            // riluePrinterType
            // 
            this.riluePrinterType.AutoHeight = false;
            this.riluePrinterType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riluePrinterType.Name = "riluePrinterType";
            this.riluePrinterType.NullText = "";
            // 
            // gcolIsValid
            // 
            this.gcolIsValid.Caption = "是否有效";
            this.gcolIsValid.ColumnEdit = this.richkIsValid;
            this.gcolIsValid.FieldName = "IS_VALID";
            this.gcolIsValid.Name = "gcolIsValid";
            this.gcolIsValid.OptionsColumn.FixedWidth = true;
            this.gcolIsValid.Visible = true;
            this.gcolIsValid.VisibleIndex = 5;
            this.gcolIsValid.Width = 100;
            // 
            // richkIsValid
            // 
            this.richkIsValid.AutoHeight = false;
            this.richkIsValid.Name = "richkIsValid";
            this.richkIsValid.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.richkIsValid.ValueChecked = "Y";
            this.richkIsValid.ValueUnchecked = "N";
            // 
            // gcolProductModel
            // 
            this.gcolProductModel.Caption = "产品型号";
            this.gcolProductModel.ColumnEdit = this.ricmbProductModel;
            this.gcolProductModel.FieldName = "PRODUCT_MODEL";
            this.gcolProductModel.Name = "gcolProductModel";
            this.gcolProductModel.OptionsColumn.FixedWidth = true;
            this.gcolProductModel.Visible = true;
            this.gcolProductModel.VisibleIndex = 6;
            this.gcolProductModel.Width = 100;
            // 
            // ricmbProductModel
            // 
            this.ricmbProductModel.AutoHeight = false;
            this.ricmbProductModel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ricmbProductModel.Name = "ricmbProductModel";
            // 
            // gcolCertificateType
            // 
            this.gcolCertificateType.Caption = "认证类型";
            this.gcolCertificateType.ColumnEdit = this.ricmbCertificateType;
            this.gcolCertificateType.FieldName = "CERTIFICATE_TYPE";
            this.gcolCertificateType.Name = "gcolCertificateType";
            this.gcolCertificateType.OptionsColumn.FixedWidth = true;
            this.gcolCertificateType.Visible = true;
            this.gcolCertificateType.VisibleIndex = 7;
            this.gcolCertificateType.Width = 100;
            // 
            // ricmbCertificateType
            // 
            this.ricmbCertificateType.AutoHeight = false;
            this.ricmbCertificateType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ricmbCertificateType.Name = "ricmbCertificateType";
            // 
            // gcolPowersetType
            // 
            this.gcolPowersetType.Caption = "分档方式";
            this.gcolPowersetType.ColumnEdit = this.ricmbPowersetType;
            this.gcolPowersetType.FieldName = "POWERSET_TYPE";
            this.gcolPowersetType.Name = "gcolPowersetType";
            this.gcolPowersetType.OptionsColumn.FixedWidth = true;
            this.gcolPowersetType.Visible = true;
            this.gcolPowersetType.VisibleIndex = 8;
            this.gcolPowersetType.Width = 100;
            // 
            // ricmbPowersetType
            // 
            this.ricmbPowersetType.AutoHeight = false;
            this.ricmbPowersetType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ricmbPowersetType.Name = "ricmbPowersetType";
            // 
            // gcolCustCheck_Type
            // 
            this.gcolCustCheck_Type.Caption = "检验方式";
            this.gcolCustCheck_Type.ColumnEdit = this.rilueCustCheckType;
            this.gcolCustCheck_Type.FieldName = "CUSTCHECK_TYPE";
            this.gcolCustCheck_Type.Name = "gcolCustCheck_Type";
            this.gcolCustCheck_Type.Visible = true;
            this.gcolCustCheck_Type.VisibleIndex = 9;
            // 
            // rilueCustCheckType
            // 
            this.rilueCustCheckType.AutoHeight = false;
            this.rilueCustCheckType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueCustCheckType.Name = "rilueCustCheckType";
            this.rilueCustCheckType.NullText = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tsbSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(1, 388);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(799, 39);
            this.panelControl1.TabIndex = 3;
            // 
            // tsbSave
            // 
            this.tsbSave.Location = new System.Drawing.Point(-1, 12);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(82, 23);
            this.tsbSave.TabIndex = 0;
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // PrintLabelCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcResults);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PrintLabelCtrl";
            this.Size = new System.Drawing.Size(801, 427);
            this.Load += new System.EventHandler(this.BasicPrintLabelCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            this.Controls.SetChildIndex(this.gcResults, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPrintLabelDataDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riluePrinterType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richkIsValid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbProductModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbCertificateType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricmbPowersetType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueCustCheckType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraGrid.GridControl gcResults;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPrintLabelDataDetail;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailLabelId;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailLabelName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailVersionNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailIsValid;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailProductModel;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailCertificateType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailPowersetType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailCustCheck_Type;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailCreateTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailCreator;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDetailEditor;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResults;
        private DevExpress.XtraGrid.Columns.GridColumn gcolId;
        private DevExpress.XtraGrid.Columns.GridColumn gcolName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolVersion;
        private DevExpress.XtraGrid.Columns.GridColumn gcolDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueDataType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolPrinterType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit riluePrinterType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolIsValid;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit richkIsValid;
        private DevExpress.XtraGrid.Columns.GridColumn gcolProductModel;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricmbProductModel;
        private DevExpress.XtraGrid.Columns.GridColumn gcolCertificateType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricmbCertificateType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolPowersetType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricmbPowersetType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolCustCheck_Type;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueCustCheckType;
    }
}

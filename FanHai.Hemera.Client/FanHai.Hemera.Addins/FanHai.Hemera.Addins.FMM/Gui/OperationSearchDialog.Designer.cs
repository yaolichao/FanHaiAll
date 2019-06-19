namespace FanHai.Hemera.Addins.FMM
{
    partial class OperationSearchDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lcContent = new DevExpress.XtraLayout.LayoutControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.grdCtrlOperation = new DevExpress.XtraGrid.GridControl();
            this.gridViewOperation = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_operationKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Version = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_IsReWork = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkRwk = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn_Creator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtOperationName = new DevExpress.XtraEditors.TextEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgResult = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRwk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(571, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 26);
            this.btnQuery.StyleController = this.lcContent;
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lcContent
            // 
            this.lcContent.AllowCustomization = false;
            this.lcContent.Controls.Add(this.btnConfirm);
            this.lcContent.Controls.Add(this.grdCtrlOperation);
            this.lcContent.Controls.Add(this.txtOperationName);
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.btnQuery);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Name = "lcContent";
            this.lcContent.Root = this.layoutControlGroup1;
            this.lcContent.Size = new System.Drawing.Size(656, 381);
            this.lcContent.TabIndex = 1;
            this.lcContent.Text = "layoutControl1";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(490, 345);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(76, 26);
            this.btnConfirm.StyleController = this.lcContent;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // grdCtrlOperation
            // 
            this.grdCtrlOperation.Location = new System.Drawing.Point(10, 50);
            this.grdCtrlOperation.MainView = this.gridViewOperation;
            this.grdCtrlOperation.Name = "grdCtrlOperation";
            this.grdCtrlOperation.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkRwk});
            this.grdCtrlOperation.Size = new System.Drawing.Size(636, 281);
            this.grdCtrlOperation.TabIndex = 10;
            this.grdCtrlOperation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOperation});
            // 
            // gridViewOperation
            // 
            this.gridViewOperation.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_operationKey,
            this.gridColumn_Name,
            this.gridColumn_Description,
            this.gridColumn_Version,
            this.gridColumn_IsReWork,
            this.gridColumn_Creator,
            this.gridColumn_CreateTime});
            this.gridViewOperation.GridControl = this.grdCtrlOperation;
            this.gridViewOperation.Name = "gridViewOperation";
            this.gridViewOperation.OptionsBehavior.Editable = false;
            this.gridViewOperation.OptionsView.ShowGroupPanel = false;
            this.gridViewOperation.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridViewOperation_CustomDrawRowIndicator);
            this.gridViewOperation.DoubleClick += new System.EventHandler(this.gridViewOperation_DoubleClick);
            // 
            // gridColumn_operationKey
            // 
            this.gridColumn_operationKey.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_operationKey.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_operationKey.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_operationKey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_operationKey.Caption = "ROUTE_OPERATION_VER_KEY";
            this.gridColumn_operationKey.FieldName = "ROUTE_OPERATION_VER_KEY";
            this.gridColumn_operationKey.Name = "gridColumn_operationKey";
            // 
            // gridColumn_Name
            // 
            this.gridColumn_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Name.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_Name.Caption = "名称";
            this.gridColumn_Name.FieldName = "ROUTE_OPERATION_NAME";
            this.gridColumn_Name.Name = "gridColumn_Name";
            this.gridColumn_Name.Visible = true;
            this.gridColumn_Name.VisibleIndex = 0;
            // 
            // gridColumn_Description
            // 
            this.gridColumn_Description.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Description.Caption = "描述";
            this.gridColumn_Description.FieldName = "DESCRIPTIONS";
            this.gridColumn_Description.Name = "gridColumn_Description";
            this.gridColumn_Description.Visible = true;
            this.gridColumn_Description.VisibleIndex = 1;
            // 
            // gridColumn_Version
            // 
            this.gridColumn_Version.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Version.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Version.Caption = "版本";
            this.gridColumn_Version.FieldName = "OPERATION_VERSION";
            this.gridColumn_Version.Name = "gridColumn_Version";
            this.gridColumn_Version.Visible = true;
            this.gridColumn_Version.VisibleIndex = 2;
            // 
            // gridColumn_IsReWork
            // 
            this.gridColumn_IsReWork.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_IsReWork.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_IsReWork.Caption = "是否返工";
            this.gridColumn_IsReWork.ColumnEdit = this.chkRwk;
            this.gridColumn_IsReWork.FieldName = "IS_REWORKABLE";
            this.gridColumn_IsReWork.Name = "gridColumn_IsReWork";
            this.gridColumn_IsReWork.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn_IsReWork.Visible = true;
            this.gridColumn_IsReWork.VisibleIndex = 5;
            // 
            // chkRwk
            // 
            this.chkRwk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.chkRwk.DisplayValueChecked = "1";
            this.chkRwk.DisplayValueGrayed = "-1";
            this.chkRwk.DisplayValueUnchecked = "0";
            this.chkRwk.Name = "chkRwk";
            this.chkRwk.ValueChecked = "1";
            this.chkRwk.ValueGrayed = "";
            this.chkRwk.ValueUnchecked = "0";
            // 
            // gridColumn_Creator
            // 
            this.gridColumn_Creator.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Creator.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Creator.Caption = "操作人";
            this.gridColumn_Creator.FieldName = "CREATOR";
            this.gridColumn_Creator.Name = "gridColumn_Creator";
            this.gridColumn_Creator.Visible = true;
            this.gridColumn_Creator.VisibleIndex = 3;
            // 
            // gridColumn_CreateTime
            // 
            this.gridColumn_CreateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CreateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CreateTime.Caption = "操作时间";
            this.gridColumn_CreateTime.FieldName = "CREATE_TIME";
            this.gridColumn_CreateTime.Name = "gridColumn_CreateTime";
            this.gridColumn_CreateTime.Visible = true;
            this.gridColumn_CreateTime.VisibleIndex = 4;
            // 
            // txtOperationName
            // 
            this.txtOperationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOperationName.Location = new System.Drawing.Point(37, 10);
            this.txtOperationName.Name = "txtOperationName";
            this.txtOperationName.Size = new System.Drawing.Size(530, 20);
            this.txtOperationName.StyleController = this.lcContent;
            this.txtOperationName.TabIndex = 0;
            this.txtOperationName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOperationName_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(570, 345);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 26);
            this.btnCancel.StyleController = this.lcContent;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgQuery,
            this.lcgResult,
            this.lcgButtons});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup1.Size = new System.Drawing.Size(656, 381);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lcgQuery
            // 
            this.lcgQuery.CustomizationFormText = "查询条件";
            this.lcgQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciName,
            this.lciQuery});
            this.lcgQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgQuery.Name = "lcgQuery";
            this.lcgQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgQuery.Size = new System.Drawing.Size(650, 40);
            this.lcgQuery.Text = "查询条件";
            this.lcgQuery.TextVisible = false;
            // 
            // lciName
            // 
            this.lciName.Control = this.txtOperationName;
            this.lciName.CustomizationFormText = "名称";
            this.lciName.Location = new System.Drawing.Point(0, 0);
            this.lciName.Name = "lciName";
            this.lciName.Size = new System.Drawing.Size(561, 30);
            this.lciName.Text = "名称";
            this.lciName.TextSize = new System.Drawing.Size(24, 14);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(561, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(80, 30);
            this.lciQuery.MinSize = new System.Drawing.Size(70, 30);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(79, 30);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // lcgResult
            // 
            this.lcgResult.CustomizationFormText = "结果";
            this.lcgResult.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciResult});
            this.lcgResult.Location = new System.Drawing.Point(0, 40);
            this.lcgResult.Name = "lcgResult";
            this.lcgResult.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgResult.Size = new System.Drawing.Size(650, 295);
            this.lcgResult.Text = "结果";
            this.lcgResult.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.grdCtrlOperation;
            this.lciResult.CustomizationFormText = "结果";
            this.lciResult.Location = new System.Drawing.Point(0, 0);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(640, 285);
            this.lciResult.Text = "结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "按钮";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciClose,
            this.lciConfirm,
            this.emptySpaceItem1});
            this.lcgButtons.Location = new System.Drawing.Point(0, 335);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgButtons.Size = new System.Drawing.Size(650, 40);
            this.lcgButtons.Text = "按钮";
            this.lcgButtons.TextVisible = false;
            // 
            // lciClose
            // 
            this.lciClose.Control = this.btnCancel;
            this.lciClose.CustomizationFormText = "关闭";
            this.lciClose.Location = new System.Drawing.Point(560, 0);
            this.lciClose.MaxSize = new System.Drawing.Size(80, 30);
            this.lciClose.MinSize = new System.Drawing.Size(80, 30);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(80, 30);
            this.lciClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciClose.Text = "关闭";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextVisible = false;
            // 
            // lciConfirm
            // 
            this.lciConfirm.Control = this.btnConfirm;
            this.lciConfirm.CustomizationFormText = "确定";
            this.lciConfirm.Location = new System.Drawing.Point(480, 0);
            this.lciConfirm.MaxSize = new System.Drawing.Size(80, 30);
            this.lciConfirm.MinSize = new System.Drawing.Size(80, 30);
            this.lciConfirm.Name = "lciConfirm";
            this.lciConfirm.Size = new System.Drawing.Size(80, 30);
            this.lciConfirm.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciConfirm.Text = "确定";
            this.lciConfirm.TextSize = new System.Drawing.Size(0, 0);
            this.lciConfirm.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(480, 30);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // OperationSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 381);
            this.Controls.Add(this.lcContent);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OperationSearchDialog";
            this.Load += new System.EventHandler(this.OperationSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRwk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtOperationName;
        private DevExpress.XtraGrid.GridControl grdCtrlOperation;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOperation;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_operationKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Description;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Version;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_IsReWork;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkRwk;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Creator;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CreateTime;
        private DevExpress.XtraLayout.LayoutControl lcContent;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciName;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraLayout.LayoutControlGroup lcgResult;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraLayout.LayoutControlItem lciConfirm;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}
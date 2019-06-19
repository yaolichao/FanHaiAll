namespace FanHai.Hemera.Utils.Dialogs
{
    partial class PalletQueryHelpDialog
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
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.pgnQueryResult = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.gcResult = new DevExpress.XtraGrid.GridControl();
            this.gvResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gclRowNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclPalletNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclCsDataGroup = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclWorkorderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclSapNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclPowerLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclGrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.respostior_Grade = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.tePalletNo = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciPalletNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPageNavigation = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.respostior_Grade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPageNavigation)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.AllowCustomization = false;
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.gcResult);
            this.Content.Controls.Add(this.btnQuery);
            this.Content.Controls.Add(this.tePalletNo);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgRoot;
            this.Content.Size = new System.Drawing.Size(650, 301);
            this.Content.TabIndex = 0;
            // 
            // pgnQueryResult
            // 
            this.pgnQueryResult.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.pgnQueryResult.Appearance.Options.UseBackColor = true;
            this.pgnQueryResult.Location = new System.Drawing.Point(4, 267);
            this.pgnQueryResult.Margin = new System.Windows.Forms.Padding(0);
            this.pgnQueryResult.Name = "pgnQueryResult";
            this.pgnQueryResult.PageNo = 1;
            this.pgnQueryResult.Pages = 0;
            this.pgnQueryResult.PageSize = 200;
            this.pgnQueryResult.Records = 0;
            this.pgnQueryResult.Size = new System.Drawing.Size(642, 30);
            this.pgnQueryResult.TabIndex = 64;
            this.pgnQueryResult.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.pgnQueryResult_DataPaging);
            // 
            // gcResult
            // 
            this.gcResult.Location = new System.Drawing.Point(4, 30);
            this.gcResult.MainView = this.gvResult;
            this.gcResult.Name = "gcResult";
            this.gcResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.respostior_Grade});
            this.gcResult.Size = new System.Drawing.Size(642, 233);
            this.gcResult.TabIndex = 6;
            this.gcResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResult});
            // 
            // gvResult
            // 
            this.gvResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gclRowNum,
            this.gclPalletNo,
            this.gclCsDataGroup,
            this.gclWorkorderNo,
            this.gclSapNo,
            this.gclPowerLevel,
            this.gclGrade});
            this.gvResult.GridControl = this.gcResult;
            this.gvResult.Name = "gvResult";
            this.gvResult.OptionsBehavior.Editable = false;
            this.gvResult.OptionsBehavior.ReadOnly = true;
            this.gvResult.OptionsView.ShowGroupPanel = false;
            this.gvResult.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvResult_CustomDrawCell);
            this.gvResult.DoubleClick += new System.EventHandler(this.gvResult_DoubleClick);
            // 
            // gclRowNum
            // 
            this.gclRowNum.Caption = "序号";
            this.gclRowNum.FieldName = "ROW_NUM";
            this.gclRowNum.Name = "gclRowNum";
            this.gclRowNum.OptionsColumn.AllowEdit = false;
            this.gclRowNum.OptionsColumn.FixedWidth = true;
            this.gclRowNum.OptionsColumn.ReadOnly = true;
            this.gclRowNum.Visible = true;
            this.gclRowNum.VisibleIndex = 0;
            this.gclRowNum.Width = 50;
            // 
            // gclPalletNo
            // 
            this.gclPalletNo.Caption = "托盘号";
            this.gclPalletNo.FieldName = "VIRTUAL_PALLET_NO";
            this.gclPalletNo.Name = "gclPalletNo";
            this.gclPalletNo.Visible = true;
            this.gclPalletNo.VisibleIndex = 1;
            this.gclPalletNo.Width = 70;
            // 
            // gclCsDataGroup
            // 
            this.gclCsDataGroup.Caption = "状态";
            this.gclCsDataGroup.FieldName = "CS_DATA_GROUP";
            this.gclCsDataGroup.Name = "gclCsDataGroup";
            this.gclCsDataGroup.Visible = true;
            this.gclCsDataGroup.VisibleIndex = 2;
            this.gclCsDataGroup.Width = 67;
            // 
            // gclWorkorderNo
            // 
            this.gclWorkorderNo.Caption = "工单号";
            this.gclWorkorderNo.FieldName = "WORKNUMBER";
            this.gclWorkorderNo.Name = "gclWorkorderNo";
            this.gclWorkorderNo.Visible = true;
            this.gclWorkorderNo.VisibleIndex = 3;
            this.gclWorkorderNo.Width = 70;
            // 
            // gclSapNo
            // 
            this.gclSapNo.Caption = "成品料号";
            this.gclSapNo.FieldName = "SAP_NO";
            this.gclSapNo.Name = "gclSapNo";
            this.gclSapNo.Visible = true;
            this.gclSapNo.VisibleIndex = 4;
            this.gclSapNo.Width = 70;
            // 
            // gclPowerLevel
            // 
            this.gclPowerLevel.Caption = "功率档位";
            this.gclPowerLevel.FieldName = "POWER_LEVEL";
            this.gclPowerLevel.Name = "gclPowerLevel";
            this.gclPowerLevel.OptionsColumn.FixedWidth = true;
            this.gclPowerLevel.Visible = true;
            this.gclPowerLevel.VisibleIndex = 5;
            this.gclPowerLevel.Width = 85;
            // 
            // gclGrade
            // 
            this.gclGrade.Caption = "托等级";
            this.gclGrade.ColumnEdit = this.respostior_Grade;
            this.gclGrade.FieldName = "GRADE";
            this.gclGrade.Name = "gclGrade";
            this.gclGrade.Visible = true;
            this.gclGrade.VisibleIndex = 6;
            this.gclGrade.Width = 66;
            // 
            // respostior_Grade
            // 
            this.respostior_Grade.AutoHeight = false;
            this.respostior_Grade.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.respostior_Grade.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Column_Name", "等级")});
            this.respostior_Grade.Name = "respostior_Grade";
            this.respostior_Grade.NullText = "";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(569, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(77, 22);
            this.btnQuery.StyleController = this.Content;
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tePalletNo
            // 
            this.tePalletNo.Location = new System.Drawing.Point(43, 4);
            this.tePalletNo.Name = "tePalletNo";
            this.tePalletNo.Size = new System.Drawing.Size(522, 20);
            this.tePalletNo.StyleController = this.Content;
            this.tePalletNo.TabIndex = 4;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciPalletNo,
            this.lciQuery,
            this.lciResult,
            this.lciPageNavigation});
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgRoot.Size = new System.Drawing.Size(650, 301);
            this.lcgRoot.TextVisible = false;
            // 
            // lciPalletNo
            // 
            this.lciPalletNo.Control = this.tePalletNo;
            this.lciPalletNo.CustomizationFormText = "托盘号";
            this.lciPalletNo.Location = new System.Drawing.Point(0, 0);
            this.lciPalletNo.Name = "lciPalletNo";
            this.lciPalletNo.Size = new System.Drawing.Size(565, 26);
            this.lciPalletNo.Text = "托盘号";
            this.lciPalletNo.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(565, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(81, 26);
            this.lciQuery.MinSize = new System.Drawing.Size(81, 26);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(81, 26);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcResult;
            this.lciResult.CustomizationFormText = "查询结果";
            this.lciResult.Location = new System.Drawing.Point(0, 26);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(646, 237);
            this.lciResult.Text = "查询结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lciPageNavigation
            // 
            this.lciPageNavigation.Control = this.pgnQueryResult;
            this.lciPageNavigation.CustomizationFormText = "分页";
            this.lciPageNavigation.Location = new System.Drawing.Point(0, 263);
            this.lciPageNavigation.MaxSize = new System.Drawing.Size(0, 34);
            this.lciPageNavigation.MinSize = new System.Drawing.Size(104, 34);
            this.lciPageNavigation.Name = "lciPageNavigation";
            this.lciPageNavigation.Size = new System.Drawing.Size(646, 34);
            this.lciPageNavigation.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPageNavigation.Text = "分页";
            this.lciPageNavigation.TextSize = new System.Drawing.Size(0, 0);
            this.lciPageNavigation.TextVisible = false;
            // 
            // PalletQueryHelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(650, 301);
            this.ControlBox = false;
            this.Controls.Add(this.Content);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PalletQueryHelpDialog";
            this.Deactivate += new System.EventHandler(this.PalletQueryHelpDialog_Deactivate);
            this.Load += new System.EventHandler(this.PalletQueryHelpDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.respostior_Grade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPageNavigation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit tePalletNo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciPalletNo;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraGrid.GridControl gcResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResult;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private FanHai.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPageNavigation;
        private DevExpress.XtraGrid.Columns.GridColumn gclRowNum;
        private DevExpress.XtraGrid.Columns.GridColumn gclPalletNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclWorkorderNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclSapNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclPowerLevel;
        private DevExpress.XtraGrid.Columns.GridColumn gclGrade;
        private DevExpress.XtraGrid.Columns.GridColumn gclCsDataGroup;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit respostior_Grade;

    }
}
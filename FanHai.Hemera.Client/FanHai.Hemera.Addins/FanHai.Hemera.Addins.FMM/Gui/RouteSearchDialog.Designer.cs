namespace FanHai.Hemera.Addins.FMM
{
    partial class RouteSearchDialog
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.gcResult = new DevExpress.XtraGrid.GridControl();
            this.gvResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Version = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Creator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgResult = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefixButtons = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(695, 11);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(87, 35);
            this.btnQuery.StyleController = this.lcContent;
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lcContent
            // 
            this.lcContent.AllowCustomization = false;
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.btnQuery);
            this.lcContent.Controls.Add(this.btnConfirm);
            this.lcContent.Controls.Add(this.gcResult);
            this.lcContent.Controls.Add(this.txtName);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcContent.Name = "lcContent";
            this.lcContent.Root = this.lcgRoot;
            this.lcContent.Size = new System.Drawing.Size(791, 504);
            this.lcContent.TabIndex = 1;
            this.lcContent.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(695, 458);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 35);
            this.btnCancel.StyleController = this.lcContent;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(604, 458);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 35);
            this.btnConfirm.StyleController = this.lcContent;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // gcResult
            // 
            this.gcResult.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcResult.Location = new System.Drawing.Point(9, 62);
            this.gcResult.MainView = this.gvResult;
            this.gcResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcResult.Name = "gcResult";
            this.gcResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcResult.Size = new System.Drawing.Size(773, 380);
            this.gcResult.TabIndex = 11;
            this.gcResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResult});
            // 
            // gvResult
            // 
            this.gvResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_Key,
            this.gridColumn_Name,
            this.gridColumn_Description,
            this.gridColumn_Version,
            this.gridColumn_Creator,
            this.gridColumn_CreateTime});
            this.gvResult.DetailHeight = 450;
            this.gvResult.FixedLineWidth = 3;
            this.gvResult.GridControl = this.gcResult;
            this.gvResult.Name = "gvResult";
            this.gvResult.OptionsBehavior.Editable = false;
            this.gvResult.OptionsView.ShowGroupPanel = false;
            this.gvResult.DoubleClick += new System.EventHandler(this.gvResult_DoubleClick);
            // 
            // gridColumn_Key
            // 
            this.gridColumn_Key.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Key.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Key.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Key.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Key.Caption = "KEY";
            this.gridColumn_Key.FieldName = "ROUTE_ROUTE_VER_KEY";
            this.gridColumn_Key.MinWidth = 23;
            this.gridColumn_Key.Name = "gridColumn_Key";
            this.gridColumn_Key.Width = 86;
            // 
            // gridColumn_Name
            // 
            this.gridColumn_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Name.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_Name.Caption = "名称";
            this.gridColumn_Name.FieldName = "ROUTE_NAME";
            this.gridColumn_Name.MinWidth = 23;
            this.gridColumn_Name.Name = "gridColumn_Name";
            this.gridColumn_Name.Visible = true;
            this.gridColumn_Name.VisibleIndex = 0;
            this.gridColumn_Name.Width = 86;
            // 
            // gridColumn_Description
            // 
            this.gridColumn_Description.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Description.Caption = "描述";
            this.gridColumn_Description.FieldName = "DESCRIPTION";
            this.gridColumn_Description.MinWidth = 23;
            this.gridColumn_Description.Name = "gridColumn_Description";
            this.gridColumn_Description.Visible = true;
            this.gridColumn_Description.VisibleIndex = 1;
            this.gridColumn_Description.Width = 86;
            // 
            // gridColumn_Version
            // 
            this.gridColumn_Version.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Version.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Version.Caption = "版本";
            this.gridColumn_Version.FieldName = "ROUTE_VERSION";
            this.gridColumn_Version.MinWidth = 23;
            this.gridColumn_Version.Name = "gridColumn_Version";
            this.gridColumn_Version.Visible = true;
            this.gridColumn_Version.VisibleIndex = 2;
            this.gridColumn_Version.Width = 86;
            // 
            // gridColumn_Creator
            // 
            this.gridColumn_Creator.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Creator.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Creator.Caption = "操作人";
            this.gridColumn_Creator.FieldName = "CREATOR";
            this.gridColumn_Creator.MinWidth = 23;
            this.gridColumn_Creator.Name = "gridColumn_Creator";
            this.gridColumn_Creator.Visible = true;
            this.gridColumn_Creator.VisibleIndex = 3;
            this.gridColumn_Creator.Width = 86;
            // 
            // gridColumn_CreateTime
            // 
            this.gridColumn_CreateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CreateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CreateTime.Caption = "操作时间";
            this.gridColumn_CreateTime.FieldName = "CREATE_TIME";
            this.gridColumn_CreateTime.MinWidth = 23;
            this.gridColumn_CreateTime.Name = "gridColumn_CreateTime";
            this.gridColumn_CreateTime.Visible = true;
            this.gridColumn_CreateTime.VisibleIndex = 4;
            this.gridColumn_CreateTime.Width = 86;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.DisplayValueChecked = "1";
            this.repositoryItemCheckEdit1.DisplayValueGrayed = "1";
            this.repositoryItemCheckEdit1.DisplayValueUnchecked = "0";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(42, 11);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(649, 24);
            this.txtName.StyleController = this.lcContent;
            this.txtName.TabIndex = 0;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgQuery,
            this.lcgResult,
            this.lcgButtons});
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgRoot.Size = new System.Drawing.Size(791, 504);
            this.lcgRoot.TextVisible = false;
            // 
            // lcgQuery
            // 
            this.lcgQuery.CustomizationFormText = "查询";
            this.lcgQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciName,
            this.lciQuery});
            this.lcgQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgQuery.Name = "lcgQuery";
            this.lcgQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgQuery.Size = new System.Drawing.Size(787, 51);
            this.lcgQuery.Text = "查询";
            this.lcgQuery.TextVisible = false;
            // 
            // lciName
            // 
            this.lciName.Control = this.txtName;
            this.lciName.CustomizationFormText = "名称";
            this.lciName.Location = new System.Drawing.Point(0, 0);
            this.lciName.Name = "lciName";
            this.lciName.Size = new System.Drawing.Size(686, 39);
            this.lciName.Text = "名称";
            this.lciName.TextSize = new System.Drawing.Size(30, 18);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(686, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(91, 39);
            this.lciQuery.MinSize = new System.Drawing.Size(91, 39);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(91, 39);
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
            this.lcgResult.Location = new System.Drawing.Point(0, 51);
            this.lcgResult.Name = "lcgResult";
            this.lcgResult.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgResult.Size = new System.Drawing.Size(787, 396);
            this.lcgResult.Text = "结果";
            this.lcgResult.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcResult;
            this.lciResult.CustomizationFormText = "结果";
            this.lciResult.Location = new System.Drawing.Point(0, 0);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(777, 384);
            this.lciResult.Text = "结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "按钮";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciConfirm,
            this.lciClose,
            this.esiPrefixButtons});
            this.lcgButtons.Location = new System.Drawing.Point(0, 447);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgButtons.Size = new System.Drawing.Size(787, 51);
            this.lcgButtons.Text = "按钮";
            this.lcgButtons.TextVisible = false;
            // 
            // lciConfirm
            // 
            this.lciConfirm.Control = this.btnConfirm;
            this.lciConfirm.CustomizationFormText = "确定";
            this.lciConfirm.Location = new System.Drawing.Point(595, 0);
            this.lciConfirm.MaxSize = new System.Drawing.Size(91, 39);
            this.lciConfirm.MinSize = new System.Drawing.Size(91, 39);
            this.lciConfirm.Name = "lciConfirm";
            this.lciConfirm.Size = new System.Drawing.Size(91, 39);
            this.lciConfirm.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciConfirm.Text = "确定";
            this.lciConfirm.TextSize = new System.Drawing.Size(0, 0);
            this.lciConfirm.TextVisible = false;
            // 
            // lciClose
            // 
            this.lciClose.Control = this.btnCancel;
            this.lciClose.CustomizationFormText = "关闭";
            this.lciClose.Location = new System.Drawing.Point(686, 0);
            this.lciClose.MaxSize = new System.Drawing.Size(91, 39);
            this.lciClose.MinSize = new System.Drawing.Size(91, 39);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(91, 39);
            this.lciClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciClose.Text = "关闭";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextVisible = false;
            // 
            // esiPrefixButtons
            // 
            this.esiPrefixButtons.AllowHotTrack = false;
            this.esiPrefixButtons.CustomizationFormText = "esiPrefixButtons";
            this.esiPrefixButtons.Location = new System.Drawing.Point(0, 0);
            this.esiPrefixButtons.Name = "esiPrefixButtons";
            this.esiPrefixButtons.Size = new System.Drawing.Size(595, 39);
            this.esiPrefixButtons.TextSize = new System.Drawing.Size(0, 0);
            // 
            // RouteSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 504);
            this.Controls.Add(this.lcContent);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "RouteSearchDialog";
            this.Load += new System.EventHandler(this.RouteSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraGrid.GridControl gcResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Key;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Description;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Version;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Creator;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CreateTime;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraLayout.LayoutControl lcContent;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciName;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private DevExpress.XtraLayout.LayoutControlItem lciConfirm;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQuery;
        private DevExpress.XtraLayout.LayoutControlGroup lcgResult;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefixButtons;

    }
}
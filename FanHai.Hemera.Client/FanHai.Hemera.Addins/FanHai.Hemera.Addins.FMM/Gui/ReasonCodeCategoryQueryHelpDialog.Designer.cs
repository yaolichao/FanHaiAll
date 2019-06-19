namespace FanHai.Hemera.Addins.FMM
{
    partial class ReasonCodeCategoryQueryHelpDialog
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
            this.txName = new DevExpress.XtraEditors.TextEdit();
            this.gcResult = new DevExpress.XtraGrid.GridControl();
            this.gvResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gclName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btSearch = new DevExpress.XtraEditors.SimpleButton();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.AllowCustomizationMenu = false;
            this.Content.Controls.Add(this.txName);
            this.Content.Controls.Add(this.gcResult);
            this.Content.Controls.Add(this.btSearch);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgRoot;
            this.Content.Size = new System.Drawing.Size(426, 212);
            this.Content.TabIndex = 4;
            // 
            // txName
            // 
            this.txName.Location = new System.Drawing.Point(35, 7);
            this.txName.Name = "txName";
            this.txName.Size = new System.Drawing.Size(314, 21);
            this.txName.StyleController = this.Content;
            this.txName.TabIndex = 1;
            // 
            // gcResult
            // 
            this.gcResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcResult.Location = new System.Drawing.Point(7, 33);
            this.gcResult.MainView = this.gvResult;
            this.gcResult.Name = "gcResult";
            this.gcResult.Size = new System.Drawing.Size(412, 172);
            this.gcResult.TabIndex = 2;
            this.gcResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResult});
            // 
            // gvResult
            // 
            this.gvResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gclName,
            this.gclDesc,
            this.gclKey});
            this.gvResult.GridControl = this.gcResult;
            this.gvResult.Name = "gvResult";
            this.gvResult.OptionsBehavior.ReadOnly = true;
            this.gvResult.OptionsView.ShowGroupPanel = false;
            this.gvResult.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvResult_ShowingEditor);
            this.gvResult.DoubleClick += new System.EventHandler(this.gvResult_DoubleClick);
            // 
            // gclName
            // 
            this.gclName.AppearanceHeader.Options.UseTextOptions = true;
            this.gclName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gclName.Caption = "名称";
            this.gclName.FieldName = "REASON_CODE_CATEGORY_NAME";
            this.gclName.Name = "gclName";
            this.gclName.Visible = true;
            this.gclName.VisibleIndex = 0;
            this.gclName.Width = 97;
            // 
            // gclDesc
            // 
            this.gclDesc.AppearanceHeader.Options.UseTextOptions = true;
            this.gclDesc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gclDesc.Caption = "描述";
            this.gclDesc.FieldName = "DESCRIPTIONS";
            this.gclDesc.Name = "gclDesc";
            this.gclDesc.Visible = true;
            this.gclDesc.VisibleIndex = 1;
            this.gclDesc.Width = 120;
            // 
            // gclKey
            // 
            this.gclKey.Caption = "主键";
            this.gclKey.FieldName = "REASON_CODE_CATEGORY_KEY";
            this.gclKey.Name = "gclKey";
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSearch.Location = new System.Drawing.Point(353, 7);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(66, 22);
            this.btSearch.StyleController = this.Content;
            this.btSearch.TabIndex = 3;
            this.btSearch.Text = "查询";
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "Root";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciName,
            this.lciQuery,
            this.lciResult});
            this.lcgRoot.Location = new System.Drawing.Point(0, 0);
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.lcgRoot.Size = new System.Drawing.Size(426, 212);
            this.lcgRoot.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Text = "Root";
            this.lcgRoot.TextVisible = false;
            // 
            // lciName
            // 
            this.lciName.Control = this.txName;
            this.lciName.CustomizationFormText = "名称";
            this.lciName.Location = new System.Drawing.Point(0, 0);
            this.lciName.Name = "lciName";
            this.lciName.Size = new System.Drawing.Size(346, 26);
            this.lciName.Text = "名称";
            this.lciName.TextSize = new System.Drawing.Size(24, 14);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btSearch;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(346, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(70, 0);
            this.lciQuery.MinSize = new System.Drawing.Size(70, 26);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(70, 26);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextToControlDistance = 0;
            this.lciQuery.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcResult;
            this.lciResult.CustomizationFormText = "结果";
            this.lciResult.Location = new System.Drawing.Point(0, 26);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(416, 176);
            this.lciResult.Text = "结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextToControlDistance = 0;
            this.lciResult.TextVisible = false;
            // 
            // ReasonCodeCategoryQueryHelpDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 212);
            this.ControlBox = false;
            this.Controls.Add(this.Content);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.LookAndFeel.SkinName = "Coffee";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReasonCodeCategoryQueryHelpDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Deactivate += new System.EventHandler(this.ReasonCodeQueryHelpDialog_Deactivate);
            this.Load += new System.EventHandler(this.ReasonCodeQueryHelpDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txName;
        private DevExpress.XtraEditors.SimpleButton btSearch;
        private DevExpress.XtraGrid.GridControl gcResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResult;
        private DevExpress.XtraGrid.Columns.GridColumn gclName;
        private DevExpress.XtraGrid.Columns.GridColumn gclDesc;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciName;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private DevExpress.XtraGrid.Columns.GridColumn gclKey;

    }
}
namespace FanHai.Hemera.Addins.EDC
{
    partial class ParamSearchDialog
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
            this.gridColumn_paramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_paramKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gvParamList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_paramDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcParamList = new DevExpress.XtraGrid.GridControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lcContent = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.txtParamName = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgResult = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefix = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.gvParamList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtParamName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefix)).BeginInit();
            this.SuspendLayout();
            // 
            // gridColumn_paramName
            // 
            this.gridColumn_paramName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_paramName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_paramName.Caption = "名称";
            this.gridColumn_paramName.FieldName = "PARAM_NAME";
            this.gridColumn_paramName.Name = "gridColumn_paramName";
            this.gridColumn_paramName.Visible = true;
            this.gridColumn_paramName.VisibleIndex = 0;
            // 
            // gridColumn_paramKey
            // 
            this.gridColumn_paramKey.Caption = "PARAM_KEY";
            this.gridColumn_paramKey.FieldName = "PARAM_KEY";
            this.gridColumn_paramKey.Name = "gridColumn_paramKey";
            // 
            // gvParamList
            // 
            this.gvParamList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_paramKey,
            this.gridColumn_paramName,
            this.gridColumn_paramDescription});
            this.gvParamList.GridControl = this.gcParamList;
            this.gvParamList.Name = "gvParamList";
            this.gvParamList.OptionsBehavior.Editable = false;
            this.gvParamList.OptionsView.ShowGroupPanel = false;
            this.gvParamList.DoubleClick += new System.EventHandler(this.gvParamList_DoubleClick);
            // 
            // gridColumn_paramDescription
            // 
            this.gridColumn_paramDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_paramDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_paramDescription.Caption = "描述";
            this.gridColumn_paramDescription.FieldName = "DESCRIPTIONS";
            this.gridColumn_paramDescription.Name = "gridColumn_paramDescription";
            this.gridColumn_paramDescription.Visible = true;
            this.gridColumn_paramDescription.VisibleIndex = 1;
            // 
            // gcParamList
            // 
            this.gcParamList.Location = new System.Drawing.Point(10, 52);
            this.gcParamList.MainView = this.gvParamList;
            this.gcParamList.Name = "gcParamList";
            this.gcParamList.Size = new System.Drawing.Size(674, 300);
            this.gcParamList.TabIndex = 9;
            this.gcParamList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvParamList});
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(608, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(76, 26);
            this.btnQuery.StyleController = this.lcContent;
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lcContent
            // 
            this.lcContent.AllowCustomization = false;
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.btnConfirm);
            this.lcContent.Controls.Add(this.btnQuery);
            this.lcContent.Controls.Add(this.gcParamList);
            this.lcContent.Controls.Add(this.txtParamName);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Name = "lcContent";
            this.lcContent.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(750, 161, 650, 400);
            this.lcContent.Root = this.lcgRoot;
            this.lcContent.Size = new System.Drawing.Size(694, 404);
            this.lcContent.TabIndex = 11;
            this.lcContent.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(608, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 26);
            this.btnCancel.StyleController = this.lcContent;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(528, 368);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(76, 26);
            this.btnConfirm.StyleController = this.lcContent;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // txtParamName
            // 
            this.txtParamName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtParamName.Location = new System.Drawing.Point(37, 10);
            this.txtParamName.Name = "txtParamName";
            this.txtParamName.Size = new System.Drawing.Size(567, 20);
            this.txtParamName.StyleController = this.lcContent;
            this.txtParamName.TabIndex = 0;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "Root";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgQuery,
            this.lcgResult,
            this.lcgButtons});
            this.lcgRoot.Name = "Root";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgRoot.Size = new System.Drawing.Size(694, 404);
            this.lcgRoot.Text = "Root";
            this.lcgRoot.TextVisible = false;
            // 
            // lcgQuery
            // 
            this.lcgQuery.CustomizationFormText = "查询条件";
            this.lcgQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciName,
            this.lciQuery});
            this.lcgQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgQuery.Name = "lcgQuery";
            this.lcgQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.lcgQuery.Size = new System.Drawing.Size(690, 42);
            this.lcgQuery.Text = "查询条件";
            this.lcgQuery.TextVisible = false;
            // 
            // lciName
            // 
            this.lciName.Control = this.txtParamName;
            this.lciName.CustomizationFormText = "名称";
            this.lciName.Location = new System.Drawing.Point(0, 0);
            this.lciName.Name = "lciName";
            this.lciName.Size = new System.Drawing.Size(598, 30);
            this.lciName.Text = "名称";
            this.lciName.TextSize = new System.Drawing.Size(24, 14);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(598, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(80, 30);
            this.lciQuery.MinSize = new System.Drawing.Size(80, 30);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(80, 30);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // lcgResult
            // 
            this.lcgResult.CustomizationFormText = "查询结果";
            this.lcgResult.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciResult});
            this.lcgResult.Location = new System.Drawing.Point(0, 42);
            this.lcgResult.Name = "lcgResult";
            this.lcgResult.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.lcgResult.Size = new System.Drawing.Size(690, 316);
            this.lcgResult.Text = "查询结果";
            this.lcgResult.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcParamList;
            this.lciResult.CustomizationFormText = "查询结果";
            this.lciResult.Location = new System.Drawing.Point(0, 0);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(678, 304);
            this.lciResult.Text = "查询结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "按钮";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciConfirm,
            this.lciClose,
            this.esiPrefix});
            this.lcgButtons.Location = new System.Drawing.Point(0, 358);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.lcgButtons.Size = new System.Drawing.Size(690, 42);
            this.lcgButtons.Text = "按钮";
            this.lcgButtons.TextVisible = false;
            // 
            // lciConfirm
            // 
            this.lciConfirm.Control = this.btnConfirm;
            this.lciConfirm.CustomizationFormText = "确定";
            this.lciConfirm.Location = new System.Drawing.Point(518, 0);
            this.lciConfirm.MaxSize = new System.Drawing.Size(80, 30);
            this.lciConfirm.MinSize = new System.Drawing.Size(80, 30);
            this.lciConfirm.Name = "lciConfirm";
            this.lciConfirm.Size = new System.Drawing.Size(80, 30);
            this.lciConfirm.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciConfirm.Text = "确定";
            this.lciConfirm.TextSize = new System.Drawing.Size(0, 0);
            this.lciConfirm.TextVisible = false;
            // 
            // lciClose
            // 
            this.lciClose.Control = this.btnCancel;
            this.lciClose.CustomizationFormText = "关闭";
            this.lciClose.Location = new System.Drawing.Point(598, 0);
            this.lciClose.MaxSize = new System.Drawing.Size(80, 30);
            this.lciClose.MinSize = new System.Drawing.Size(80, 30);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(80, 30);
            this.lciClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciClose.Text = "关闭";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextVisible = false;
            // 
            // esiPrefix
            // 
            this.esiPrefix.AllowHotTrack = false;
            this.esiPrefix.CustomizationFormText = "esiPrefix";
            this.esiPrefix.Location = new System.Drawing.Point(0, 0);
            this.esiPrefix.Name = "esiPrefix";
            this.esiPrefix.Size = new System.Drawing.Size(518, 30);
            this.esiPrefix.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ParamSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 404);
            this.Controls.Add(this.lcContent);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "ParamSearchDialog";
            this.Load += new System.EventHandler(this.ParamSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvParamList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtParamName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefix)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramKey;
        private DevExpress.XtraGrid.Views.Grid.GridView gvParamList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_paramDescription;
        private DevExpress.XtraGrid.GridControl gcParamList;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtParamName;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
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
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefix;
    }
}
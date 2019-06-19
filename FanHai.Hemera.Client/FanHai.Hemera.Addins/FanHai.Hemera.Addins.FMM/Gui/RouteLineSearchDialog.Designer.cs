namespace FanHai.Hemera.Addins.FMM
{
    partial class RouteLineSearchDialog
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
            this.gcLine = new DevExpress.XtraGrid.GridControl();
            this.gvLine = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_LineKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LineCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Descriptions = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lcContent = new DevExpress.XtraLayout.LayoutControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.txtLine = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefixButton = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgResult = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.gcLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).BeginInit();
            this.SuspendLayout();
            // 
            // gcLine
            // 
            this.gcLine.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcLine.Location = new System.Drawing.Point(7, 59);
            this.gcLine.MainView = this.gvLine;
            this.gcLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLine.Name = "gcLine";
            this.gcLine.Size = new System.Drawing.Size(777, 386);
            this.gcLine.TabIndex = 3;
            this.gcLine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLine});
            this.gcLine.DoubleClick += new System.EventHandler(this.gcLine_DoubleClick);
            // 
            // gvLine
            // 
            this.gvLine.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_LineKey,
            this.gridColumn_LineCode,
            this.gridColumn_LineName,
            this.gridColumn_Descriptions});
            this.gvLine.DetailHeight = 450;
            this.gvLine.FixedLineWidth = 3;
            this.gvLine.GridControl = this.gcLine;
            this.gvLine.Name = "gvLine";
            this.gvLine.OptionsView.ShowGroupPanel = false;
            this.gvLine.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvLine_ShowingEditor);
            // 
            // gridColumn_LineKey
            // 
            this.gridColumn_LineKey.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LineKey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LineKey.Caption = "LineKey";
            this.gridColumn_LineKey.FieldName = "PRODUCTION_LINE_KEY";
            this.gridColumn_LineKey.MinWidth = 23;
            this.gridColumn_LineKey.Name = "gridColumn_LineKey";
            this.gridColumn_LineKey.Width = 23;
            // 
            // gridColumn_LineCode
            // 
            this.gridColumn_LineCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LineCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LineCode.Caption = "线别代码";
            this.gridColumn_LineCode.FieldName = "LINE_CODE";
            this.gridColumn_LineCode.MinWidth = 23;
            this.gridColumn_LineCode.Name = "gridColumn_LineCode";
            this.gridColumn_LineCode.Visible = true;
            this.gridColumn_LineCode.VisibleIndex = 0;
            this.gridColumn_LineCode.Width = 203;
            // 
            // gridColumn_LineName
            // 
            this.gridColumn_LineName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LineName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LineName.Caption = "线别名称";
            this.gridColumn_LineName.FieldName = "LINE_NAME";
            this.gridColumn_LineName.MinWidth = 23;
            this.gridColumn_LineName.Name = "gridColumn_LineName";
            this.gridColumn_LineName.Visible = true;
            this.gridColumn_LineName.VisibleIndex = 1;
            this.gridColumn_LineName.Width = 210;
            // 
            // gridColumn_Descriptions
            // 
            this.gridColumn_Descriptions.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Descriptions.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Descriptions.Caption = "线别描述";
            this.gridColumn_Descriptions.FieldName = "DESCRIPTIONS";
            this.gridColumn_Descriptions.MinWidth = 23;
            this.gridColumn_Descriptions.Name = "gridColumn_Descriptions";
            this.gridColumn_Descriptions.Visible = true;
            this.gridColumn_Descriptions.VisibleIndex = 2;
            this.gridColumn_Descriptions.Width = 283;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(697, 8);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(87, 35);
            this.btnQuery.StyleController = this.lcContent;
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // lcContent
            // 
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.btnQuery);
            this.lcContent.Controls.Add(this.btnConfirm);
            this.lcContent.Controls.Add(this.txtLine);
            this.lcContent.Controls.Add(this.gcLine);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcContent.Name = "lcContent";
            this.lcContent.Root = this.lcgRoot;
            this.lcContent.Size = new System.Drawing.Size(791, 504);
            this.lcContent.TabIndex = 5;
            this.lcContent.Text = "layoutControl1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(697, 461);
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
            this.btnConfirm.Location = new System.Drawing.Point(606, 461);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 35);
            this.btnConfirm.StyleController = this.lcContent;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtLine
            // 
            this.txtLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLine.Location = new System.Drawing.Point(40, 8);
            this.txtLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(653, 24);
            this.txtLine.StyleController = this.lcContent;
            this.txtLine.TabIndex = 1;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "Root";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgButtons,
            this.lcgResult,
            this.lcgQuery});
            this.lcgRoot.Name = "Root";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Size = new System.Drawing.Size(791, 504);
            this.lcgRoot.TextVisible = false;
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "按钮";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciConfirm,
            this.lciClose,
            this.esiPrefixButton});
            this.lcgButtons.Location = new System.Drawing.Point(0, 453);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgButtons.Size = new System.Drawing.Size(791, 51);
            this.lcgButtons.Text = "按钮";
            this.lcgButtons.TextVisible = false;
            // 
            // lciConfirm
            // 
            this.lciConfirm.Control = this.btnConfirm;
            this.lciConfirm.CustomizationFormText = "确定";
            this.lciConfirm.Location = new System.Drawing.Point(599, 0);
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
            this.lciClose.Location = new System.Drawing.Point(690, 0);
            this.lciClose.MaxSize = new System.Drawing.Size(91, 39);
            this.lciClose.MinSize = new System.Drawing.Size(91, 39);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(91, 39);
            this.lciClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciClose.Text = "关闭";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextVisible = false;
            // 
            // esiPrefixButton
            // 
            this.esiPrefixButton.AllowHotTrack = false;
            this.esiPrefixButton.CustomizationFormText = "esiPrefixButton";
            this.esiPrefixButton.Location = new System.Drawing.Point(0, 0);
            this.esiPrefixButton.Name = "esiPrefixButton";
            this.esiPrefixButton.Size = new System.Drawing.Size(599, 39);
            this.esiPrefixButton.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgResult
            // 
            this.lcgResult.CustomizationFormText = "结果";
            this.lcgResult.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciResult});
            this.lcgResult.Location = new System.Drawing.Point(0, 51);
            this.lcgResult.Name = "lcgResult";
            this.lcgResult.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgResult.Size = new System.Drawing.Size(791, 402);
            this.lcgResult.Text = "结果";
            this.lcgResult.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcLine;
            this.lciResult.CustomizationFormText = "结果";
            this.lciResult.Location = new System.Drawing.Point(0, 0);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(781, 390);
            this.lciResult.Text = "结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lcgQuery
            // 
            this.lcgQuery.CustomizationFormText = "查询";
            this.lcgQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciQuery,
            this.lciName});
            this.lcgQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgQuery.Name = "lcgQuery";
            this.lcgQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgQuery.Size = new System.Drawing.Size(791, 51);
            this.lcgQuery.Text = "查询";
            this.lcgQuery.TextVisible = false;
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(690, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(91, 39);
            this.lciQuery.MinSize = new System.Drawing.Size(91, 39);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(91, 39);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // lciName
            // 
            this.lciName.Control = this.txtLine;
            this.lciName.CustomizationFormText = "名称";
            this.lciName.Location = new System.Drawing.Point(0, 0);
            this.lciName.Name = "lciName";
            this.lciName.Size = new System.Drawing.Size(690, 39);
            this.lciName.Text = "名称";
            this.lciName.TextSize = new System.Drawing.Size(30, 18);
            // 
            // RouteLineSearchDialog
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
            this.Name = "RouteLineSearchDialog";
            this.Load += new System.EventHandler(this.RouteLineSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcLine;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLine;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LineKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LineName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Descriptions;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtLine;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LineCode;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraLayout.LayoutControl lcContent;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciName;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private DevExpress.XtraLayout.LayoutControlItem lciConfirm;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.LayoutControlGroup lcgResult;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQuery;
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefixButton;

    }
}
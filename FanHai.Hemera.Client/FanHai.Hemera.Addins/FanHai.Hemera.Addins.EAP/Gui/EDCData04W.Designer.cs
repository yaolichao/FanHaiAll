namespace FanHai.Hemera.Addins.EAP
{
    partial class EDCData04W
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
            this.chkIsAutoGetData.Checked = false;
            if (tReadResistanceData != null && !mreReadResistanceData.WaitOne(1000))
            {
                tReadResistanceData.Abort();
            }
            tReadResistanceData = null;
            if (balanceReader != null)
            {
                balanceReader.Close();
                balanceReader = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.gcParam = new DevExpress.XtraGrid.GridControl();
            this.gvParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.paramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upper_Boundary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upper_spec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.target = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lower_spec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lower_boundary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcAllowMinValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcAllowMaxValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLotInfo = new DevExpress.XtraEditors.GroupControl();
            this.pnlIsAutoGetData = new DevExpress.XtraEditors.PanelControl();
            this.lblIsAutoGetData = new DevExpress.XtraEditors.LabelControl();
            this.chkIsAutoGetData = new DevExpress.XtraEditors.CheckEdit();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btSubmit = new System.Windows.Forms.ToolStripButton();
            this.btnGetRflectance = new System.Windows.Forms.ToolStripButton();
            this.btnGetRefraction = new System.Windows.Forms.ToolStripButton();
            this.btnGetThickness = new System.Windows.Forms.ToolStripButton();
            this.btnGetResistance = new System.Windows.Forms.ToolStripButton();
            this.btClose = new System.Windows.Forms.ToolStripButton();
            this.ContentMain = new System.Windows.Forms.TableLayoutPanel();
            this.pcParams = new DevExpress.XtraEditors.XtraScrollableControl();
            this.PanelTitleMain = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).BeginInit();
            this.gcLotInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlIsAutoGetData)).BeginInit();
            this.pnlIsAutoGetData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAutoGetData.Properties)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.ContentMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitleMain)).BeginInit();
            this.PanelTitleMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(1009, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(789, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-03-10 17:11:21";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(96, 29);
            this.lblMenu.Text = "采集数据";
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.ColumnCount = 2;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.14925F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.85075F));
            this.tableLayoutPanelContent.Controls.Add(this.gcParam, 1, 0);
            this.tableLayoutPanelContent.Controls.Add(this.gcLotInfo, 0, 0);
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelContent.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.RowCount = 1;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(1005, 162);
            this.tableLayoutPanelContent.TabIndex = 2;
            // 
            // gcParam
            // 
            this.gcParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcParam.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcParam.Location = new System.Drawing.Point(507, 4);
            this.gcParam.MainView = this.gvParam;
            this.gcParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcParam.Name = "gcParam";
            this.gcParam.Size = new System.Drawing.Size(495, 154);
            this.gcParam.TabIndex = 0;
            this.gcParam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvParam});
            // 
            // gvParam
            // 
            this.gvParam.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.paramName,
            this.upper_Boundary,
            this.upper_spec,
            this.target,
            this.lower_spec,
            this.lower_boundary,
            this.gcAllowMinValue,
            this.gcAllowMaxValue});
            this.gvParam.DetailHeight = 450;
            this.gvParam.FixedLineWidth = 3;
            this.gvParam.GridControl = this.gcParam;
            this.gvParam.Name = "gvParam";
            this.gvParam.OptionsView.ShowGroupPanel = false;
            // 
            // paramName
            // 
            this.paramName.Caption = "参数名称";
            this.paramName.FieldName = "PARAM_NAME";
            this.paramName.MinWidth = 23;
            this.paramName.Name = "paramName";
            this.paramName.OptionsColumn.ReadOnly = true;
            this.paramName.Visible = true;
            this.paramName.VisibleIndex = 0;
            this.paramName.Width = 114;
            // 
            // upper_Boundary
            // 
            this.upper_Boundary.Caption = "上规格线(USL)";
            this.upper_Boundary.FieldName = "UPPER_BOUNDARY";
            this.upper_Boundary.MinWidth = 23;
            this.upper_Boundary.Name = "upper_Boundary";
            this.upper_Boundary.OptionsColumn.ReadOnly = true;
            this.upper_Boundary.Visible = true;
            this.upper_Boundary.VisibleIndex = 1;
            this.upper_Boundary.Width = 82;
            // 
            // upper_spec
            // 
            this.upper_spec.Caption = "上控制线(UCL)";
            this.upper_spec.FieldName = "UPPER_SPEC";
            this.upper_spec.MinWidth = 23;
            this.upper_spec.Name = "upper_spec";
            this.upper_spec.OptionsColumn.ReadOnly = true;
            this.upper_spec.Visible = true;
            this.upper_spec.VisibleIndex = 2;
            this.upper_spec.Width = 82;
            // 
            // target
            // 
            this.target.Caption = "目标值";
            this.target.FieldName = "TARGET";
            this.target.MinWidth = 23;
            this.target.Name = "target";
            this.target.OptionsColumn.ReadOnly = true;
            this.target.Visible = true;
            this.target.VisibleIndex = 3;
            this.target.Width = 82;
            // 
            // lower_spec
            // 
            this.lower_spec.Caption = "下控制线(LCL)";
            this.lower_spec.FieldName = "LOWER_SPEC";
            this.lower_spec.MinWidth = 23;
            this.lower_spec.Name = "lower_spec";
            this.lower_spec.OptionsColumn.ReadOnly = true;
            this.lower_spec.Visible = true;
            this.lower_spec.VisibleIndex = 4;
            this.lower_spec.Width = 82;
            // 
            // lower_boundary
            // 
            this.lower_boundary.Caption = "下规格线(LSL)";
            this.lower_boundary.FieldName = "LOWER_BOUNDARY";
            this.lower_boundary.MinWidth = 23;
            this.lower_boundary.Name = "lower_boundary";
            this.lower_boundary.OptionsColumn.ReadOnly = true;
            this.lower_boundary.Visible = true;
            this.lower_boundary.VisibleIndex = 5;
            this.lower_boundary.Width = 89;
            // 
            // gcAllowMinValue
            // 
            this.gcAllowMinValue.Caption = "可输最小值";
            this.gcAllowMinValue.FieldName = "ALLOW_MIN_VALUE";
            this.gcAllowMinValue.MinWidth = 23;
            this.gcAllowMinValue.Name = "gcAllowMinValue";
            this.gcAllowMinValue.OptionsColumn.ReadOnly = true;
            this.gcAllowMinValue.Visible = true;
            this.gcAllowMinValue.VisibleIndex = 6;
            this.gcAllowMinValue.Width = 86;
            // 
            // gcAllowMaxValue
            // 
            this.gcAllowMaxValue.Caption = "可输最大值";
            this.gcAllowMaxValue.FieldName = "ALLOW_MAX_VALUE";
            this.gcAllowMaxValue.MinWidth = 23;
            this.gcAllowMaxValue.Name = "gcAllowMaxValue";
            this.gcAllowMaxValue.OptionsColumn.ReadOnly = true;
            this.gcAllowMaxValue.Visible = true;
            this.gcAllowMaxValue.VisibleIndex = 7;
            this.gcAllowMaxValue.Width = 86;
            // 
            // gcLotInfo
            // 
            this.gcLotInfo.Controls.Add(this.pnlIsAutoGetData);
            this.gcLotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLotInfo.Location = new System.Drawing.Point(3, 4);
            this.gcLotInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcLotInfo.Name = "gcLotInfo";
            this.gcLotInfo.Size = new System.Drawing.Size(498, 154);
            this.gcLotInfo.TabIndex = 0;
            this.gcLotInfo.Text = "批次基本信息";
            // 
            // pnlIsAutoGetData
            // 
            this.pnlIsAutoGetData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlIsAutoGetData.Controls.Add(this.lblIsAutoGetData);
            this.pnlIsAutoGetData.Controls.Add(this.chkIsAutoGetData);
            this.pnlIsAutoGetData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlIsAutoGetData.Location = new System.Drawing.Point(2, 110);
            this.pnlIsAutoGetData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlIsAutoGetData.Name = "pnlIsAutoGetData";
            this.pnlIsAutoGetData.Size = new System.Drawing.Size(494, 42);
            this.pnlIsAutoGetData.TabIndex = 2;
            // 
            // lblIsAutoGetData
            // 
            this.lblIsAutoGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIsAutoGetData.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblIsAutoGetData.Appearance.Options.UseForeColor = true;
            this.lblIsAutoGetData.Location = new System.Drawing.Point(205, 12);
            this.lblIsAutoGetData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblIsAutoGetData.Name = "lblIsAutoGetData";
            this.lblIsAutoGetData.Size = new System.Drawing.Size(0, 18);
            this.lblIsAutoGetData.TabIndex = 2;
            // 
            // chkIsAutoGetData
            // 
            this.chkIsAutoGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIsAutoGetData.Location = new System.Drawing.Point(3, 4);
            this.chkIsAutoGetData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkIsAutoGetData.Name = "chkIsAutoGetData";
            this.chkIsAutoGetData.Properties.Caption = "是否自动获取数据？";
            this.chkIsAutoGetData.Size = new System.Drawing.Size(189, 22);
            this.chkIsAutoGetData.TabIndex = 1;
            this.chkIsAutoGetData.CheckedChanged += new System.EventHandler(this.chkIsAutoGetData_CheckedChanged);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.ContentMain, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 60);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1009, 488);
            this.tableLayoutPanelMain.TabIndex = 8;
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackgroundImage = global::FanHai.Hemera.Addins.EAP.Properties.Resources.toolstrip_bk;
            this.toolStripMain.Font = new System.Drawing.Font("Arial", 10F);
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSubmit,
            this.btnGetRflectance,
            this.btnGetRefraction,
            this.btnGetThickness,
            this.btnGetResistance,
            this.btClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1009, 27);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            this.toolStripMain.Visible = false;
            // 
            // btSubmit
            // 
            this.btSubmit.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.save_accept;
            this.btSubmit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSubmit.Name = "btSubmit";
            this.btSubmit.Size = new System.Drawing.Size(69, 24);
            this.btSubmit.Text = "保存";
            this.btSubmit.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // btnGetRflectance
            // 
            this.btnGetRflectance.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btnGetRflectance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetRflectance.Name = "btnGetRflectance";
            this.btnGetRflectance.Size = new System.Drawing.Size(159, 24);
            this.btnGetRflectance.Text = "获取反射率数据";
            this.btnGetRflectance.Click += new System.EventHandler(this.btnGetRflectance_Click);
            // 
            // btnGetRefraction
            // 
            this.btnGetRefraction.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btnGetRefraction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetRefraction.Name = "btnGetRefraction";
            this.btnGetRefraction.Size = new System.Drawing.Size(159, 24);
            this.btnGetRefraction.Text = "获取折射率数据";
            this.btnGetRefraction.Click += new System.EventHandler(this.btnGetRefraction_Click);
            // 
            // btnGetThickness
            // 
            this.btnGetThickness.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btnGetThickness.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetThickness.Name = "btnGetThickness";
            this.btnGetThickness.Size = new System.Drawing.Size(141, 24);
            this.btnGetThickness.Text = "获取膜厚数据";
            this.btnGetThickness.Click += new System.EventHandler(this.btnGetThickness_Click);
            // 
            // btnGetResistance
            // 
            this.btnGetResistance.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btnGetResistance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGetResistance.Name = "btnGetResistance";
            this.btnGetResistance.Size = new System.Drawing.Size(141, 24);
            this.btnGetResistance.Text = "获取方阻数据";
            this.btnGetResistance.Click += new System.EventHandler(this.btnGetResistance_Click);
            // 
            // btClose
            // 
            this.btClose.Image = global::FanHai.Hemera.Addins.EAP.Properties.Resources.cancel;
            this.btClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(69, 24);
            this.btClose.Text = "关闭";
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // ContentMain
            // 
            this.ContentMain.ColumnCount = 1;
            this.ContentMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentMain.Controls.Add(this.pcParams, 0, 1);
            this.ContentMain.Controls.Add(this.tableLayoutPanelContent, 0, 0);
            this.ContentMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentMain.Location = new System.Drawing.Point(0, 0);
            this.ContentMain.Margin = new System.Windows.Forms.Padding(0);
            this.ContentMain.Name = "ContentMain";
            this.ContentMain.RowCount = 2;
            this.ContentMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.ContentMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentMain.Size = new System.Drawing.Size(1009, 302);
            this.ContentMain.TabIndex = 3;
            // 
            // pcParams
            // 
            this.pcParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcParams.Location = new System.Drawing.Point(3, 219);
            this.pcParams.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pcParams.Name = "pcParams";
            this.pcParams.Size = new System.Drawing.Size(1003, 79);
            this.pcParams.TabIndex = 3;
            // 
            // PanelTitleMain
            // 
            this.PanelTitleMain.Controls.Add(this.simpleButton5);
            this.PanelTitleMain.Controls.Add(this.simpleButton4);
            this.PanelTitleMain.Controls.Add(this.simpleButton3);
            this.PanelTitleMain.Controls.Add(this.simpleButton6);
            this.PanelTitleMain.Controls.Add(this.btnSave);
            this.PanelTitleMain.Controls.Add(this.simpleButton1);
            this.PanelTitleMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelTitleMain.Location = new System.Drawing.Point(1, 548);
            this.PanelTitleMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelTitleMain.Name = "PanelTitleMain";
            this.PanelTitleMain.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.PanelTitleMain.Size = new System.Drawing.Size(1009, 48);
            this.PanelTitleMain.TabIndex = 1;
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(12, 9);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(125, 29);
            this.simpleButton5.TabIndex = 0;
            this.simpleButton5.Text = "获取反射率数据";
            this.simpleButton5.Click += new System.EventHandler(this.btnGetRflectance_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(154, 9);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(118, 29);
            this.simpleButton4.TabIndex = 0;
            this.simpleButton4.Text = "获取折射率数据";
            this.simpleButton4.Click += new System.EventHandler(this.btnGetRefraction_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(289, 9);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(117, 29);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "获取膜厚数据";
            this.simpleButton3.Click += new System.EventHandler(this.btnGetThickness_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(637, 9);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(74, 29);
            this.simpleButton6.TabIndex = 0;
            this.simpleButton6.Text = "关闭";
            this.simpleButton6.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(546, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 29);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(423, 9);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(106, 29);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "获取方阻数据";
            this.simpleButton1.Click += new System.EventHandler(this.btnGetResistance_Click);
            // 
            // EDCData04W
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.PanelTitleMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "EDCData04W";
            this.Size = new System.Drawing.Size(1011, 596);
            this.Load += new System.EventHandler(this.EDCData04W_Load);
            this.Controls.SetChildIndex(this.PanelTitleMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).EndInit();
            this.gcLotInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlIsAutoGetData)).EndInit();
            this.pnlIsAutoGetData.ResumeLayout(false);
            this.pnlIsAutoGetData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsAutoGetData.Properties)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ContentMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitleMain)).EndInit();
            this.PanelTitleMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private DevExpress.XtraEditors.GroupControl gcLotInfo;
        private DevExpress.XtraGrid.GridControl gcParam;
        private DevExpress.XtraGrid.Views.Grid.GridView gvParam;
        private DevExpress.XtraGrid.Columns.GridColumn paramName;
        private DevExpress.XtraGrid.Columns.GridColumn upper_Boundary;
        private DevExpress.XtraGrid.Columns.GridColumn upper_spec;
        private DevExpress.XtraGrid.Columns.GridColumn target;
        private DevExpress.XtraGrid.Columns.GridColumn lower_spec;
        private DevExpress.XtraGrid.Columns.GridColumn lower_boundary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btSubmit;
        private System.Windows.Forms.ToolStripButton btClose;
        private System.Windows.Forms.ToolStripButton btnGetRflectance;
        private System.Windows.Forms.ToolStripButton btnGetRefraction;
        private System.Windows.Forms.ToolStripButton btnGetThickness;
        private System.Windows.Forms.ToolStripButton btnGetResistance;
        private System.Windows.Forms.TableLayoutPanel ContentMain;
        private DevExpress.XtraEditors.XtraScrollableControl pcParams;
        private DevExpress.XtraEditors.CheckEdit chkIsAutoGetData;
        private DevExpress.XtraEditors.PanelControl pnlIsAutoGetData;
        private DevExpress.XtraEditors.LabelControl lblIsAutoGetData;
        private DevExpress.XtraGrid.Columns.GridColumn gcAllowMinValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcAllowMaxValue;
        private DevExpress.XtraEditors.PanelControl PanelTitleMain;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
    }
}

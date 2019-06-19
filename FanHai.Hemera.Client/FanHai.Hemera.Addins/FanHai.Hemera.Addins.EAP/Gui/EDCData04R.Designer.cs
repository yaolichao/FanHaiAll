namespace SolarViewer.Hemera.Addins.EAP
{
    partial class EDCData04R
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gcLotInfo = new DevExpress.XtraEditors.GroupControl();
            this.gcParamInfo = new DevExpress.XtraEditors.GroupControl();
            this.gcParam = new DevExpress.XtraGrid.GridControl();
            this.gvParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.paramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upper_Boundary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upper_spec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.target = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lower_spec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lower_boundary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pcParams = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btSubmit = new System.Windows.Forms.ToolStripButton();
            this.btGetData = new System.Windows.Forms.ToolStripButton();
            this.btWData = new System.Windows.Forms.ToolStripButton();
            this.btDeleteData = new System.Windows.Forms.ToolStripButton();
            this.btClose = new System.Windows.Forms.ToolStripButton();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamInfo)).BeginInit();
            this.gcParamInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcParams)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(9, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(88, 24);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "数据采集";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.87511F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.12489F));
            this.tableLayoutPanel2.Controls.Add(this.gcLotInfo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gcParamInfo, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 73);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1161, 390);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // gcLotInfo
            // 
            this.gcLotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLotInfo.Location = new System.Drawing.Point(3, 3);
            this.gcLotInfo.Name = "gcLotInfo";
            this.gcLotInfo.Size = new System.Drawing.Size(515, 384);
            this.gcLotInfo.TabIndex = 0;
            this.gcLotInfo.Text = "批次基本信息";
            // 
            // gcParamInfo
            // 
            this.gcParamInfo.Controls.Add(this.gcParam);
            this.gcParamInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcParamInfo.Location = new System.Drawing.Point(524, 3);
            this.gcParamInfo.Name = "gcParamInfo";
            this.gcParamInfo.Size = new System.Drawing.Size(634, 384);
            this.gcParamInfo.TabIndex = 1;
            this.gcParamInfo.Text = "参数基本信息";
            // 
            // gcParam
            // 
            this.gcParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcParam.Location = new System.Drawing.Point(2, 23);
            this.gcParam.MainView = this.gvParam;
            this.gcParam.Name = "gcParam";
            this.gcParam.Size = new System.Drawing.Size(630, 359);
            this.gcParam.TabIndex = 1;
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
            this.lower_boundary});
            this.gvParam.GridControl = this.gcParam;
            this.gvParam.Name = "gvParam";
            this.gvParam.OptionsView.ShowGroupPanel = false;
            // 
            // paramName
            // 
            this.paramName.Caption = "参数名称";
            this.paramName.FieldName = "PARAM_NAME";
            this.paramName.Name = "paramName";
            this.paramName.OptionsColumn.ReadOnly = true;
            this.paramName.Visible = true;
            this.paramName.VisibleIndex = 0;
            this.paramName.Width = 100;
            // 
            // upper_Boundary
            // 
            this.upper_Boundary.Caption = "上线";
            this.upper_Boundary.FieldName = "UPPER_BOUNDARY";
            this.upper_Boundary.Name = "upper_Boundary";
            this.upper_Boundary.OptionsColumn.ReadOnly = true;
            this.upper_Boundary.Visible = true;
            this.upper_Boundary.VisibleIndex = 1;
            this.upper_Boundary.Width = 72;
            // 
            // upper_spec
            // 
            this.upper_spec.Caption = "期望上线";
            this.upper_spec.FieldName = "UPPER_SPEC";
            this.upper_spec.Name = "upper_spec";
            this.upper_spec.OptionsColumn.ReadOnly = true;
            this.upper_spec.Visible = true;
            this.upper_spec.VisibleIndex = 2;
            this.upper_spec.Width = 72;
            // 
            // target
            // 
            this.target.Caption = "目标值";
            this.target.FieldName = "TARGET";
            this.target.Name = "target";
            this.target.OptionsColumn.ReadOnly = true;
            this.target.Visible = true;
            this.target.VisibleIndex = 3;
            this.target.Width = 72;
            // 
            // lower_spec
            // 
            this.lower_spec.Caption = "期望下线";
            this.lower_spec.FieldName = "LOWER_SPEC";
            this.lower_spec.Name = "lower_spec";
            this.lower_spec.OptionsColumn.ReadOnly = true;
            this.lower_spec.Visible = true;
            this.lower_spec.VisibleIndex = 4;
            this.lower_spec.Width = 72;
            // 
            // lower_boundary
            // 
            this.lower_boundary.Caption = "下线";
            this.lower_boundary.FieldName = "LOWER_BOUNDARY";
            this.lower_boundary.Name = "lower_boundary";
            this.lower_boundary.OptionsColumn.ReadOnly = true;
            this.lower_boundary.Visible = true;
            this.lower_boundary.VisibleIndex = 5;
            this.lower_boundary.Width = 78;
            // 
            // pcParams
            // 
            this.pcParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcParams.Location = new System.Drawing.Point(3, 469);
            this.pcParams.Name = "pcParams";
            this.pcParams.Size = new System.Drawing.Size(1161, 391);
            this.pcParams.TabIndex = 3;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.pcParams, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1167, 863);
            this.tableLayoutPanelMain.TabIndex = 118;
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackgroundImage = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.toolstrip_bk;
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSubmit,
            this.btGetData,
            this.btWData,
            this.btDeleteData,
            this.btClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1167, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // btSubmit
            // 
            this.btSubmit.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.save_accept;
            this.btSubmit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSubmit.Name = "btSubmit";
            this.btSubmit.Size = new System.Drawing.Size(52, 22);
            this.btSubmit.Text = "提交";
            this.btSubmit.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // btGetData
            // 
            this.btGetData.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btGetData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(76, 22);
            this.btGetData.Text = "获取数据";
            this.btGetData.Click += new System.EventHandler(this.btGetData_Click);
            // 
            // btWData
            // 
            this.btWData.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.document_go;
            this.btWData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btWData.Name = "btWData";
            this.btWData.Size = new System.Drawing.Size(76, 22);
            this.btWData.Text = "硅片称重";
            this.btWData.Click += new System.EventHandler(this.btWData_Click);
            // 
            // btDeleteData
            // 
            this.btDeleteData.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.list_remove2;
            this.btDeleteData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btDeleteData.Name = "btDeleteData";
            this.btDeleteData.Size = new System.Drawing.Size(76, 22);
            this.btDeleteData.Text = "清空数据";
            this.btDeleteData.Click += new System.EventHandler(this.btDeleteData_Click);
            // 
            // btClose
            // 
            this.btClose.Image = global::SolarViewer.Hemera.Addins.EAP.Properties.Resources.cancel;
            this.btClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(52, 22);
            this.btClose.Text = "关闭";
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.labelControl1);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(6);
            this.PanelTitle.Size = new System.Drawing.Size(1161, 39);
            this.PanelTitle.TabIndex = 1;
            // 
            // EDCData04R
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "EDCData04R";
            this.Size = new System.Drawing.Size(1167, 863);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamInfo)).EndInit();
            this.gcParamInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcParams)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.GroupControl gcLotInfo;
        private DevExpress.XtraEditors.GroupControl gcParamInfo;
        private DevExpress.XtraEditors.PanelControl pcParams;
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
        private System.Windows.Forms.ToolStripButton btGetData;
        private System.Windows.Forms.ToolStripButton btWData;
        private System.Windows.Forms.ToolStripButton btDeleteData;
        private System.Windows.Forms.ToolStripButton btClose;
        private DevExpress.XtraEditors.PanelControl PanelTitle;

    }
}

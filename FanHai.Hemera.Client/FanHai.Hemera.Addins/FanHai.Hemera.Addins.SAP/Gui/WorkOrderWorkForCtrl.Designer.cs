namespace FanHai.Hemera.Addins.SAP
{
    partial class WorkOrderWorkForCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderWorkForCtrl));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbWorkOrderWorkFor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.Content = new System.Windows.Forms.TableLayoutPanel();
            this.grpQueryArea = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.lpShift = new DevExpress.XtraEditors.LookUpEdit();
            this.lpWordNumber = new DevExpress.XtraEditors.LookUpEdit();
            this.lpFacName = new DevExpress.XtraEditors.LookUpEdit();
            this.lpWordOrderPeople = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblFacName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblWordNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblWordOrderPeople = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.grpWordOrderQingDan = new DevExpress.XtraEditors.GroupControl();
            this.grdCrtlCode = new DevExpress.XtraGrid.GridControl();
            this.gvWordOrderList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdAckTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdAufnr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdAplfl = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdGmnga = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdXmnga = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdReportor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdShift = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdConend = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdWerks = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdRoomkey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lpCodeType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.paginationWorkOrderWorkFors = new FanHai.Hemera.Utils.Controls.PaginationControl();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpQueryArea)).BeginInit();
            this.grpQueryArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lpShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpWordNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpFacName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpWordOrderPeople.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFacName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWordNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWordOrderPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWordOrderQingDan)).BeginInit();
            this.grpWordOrderQingDan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCrtlCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWordOrderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpCodeType)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(917, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(697, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-05-15 11:05:55";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(917, 912);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 31);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(6);
            this.PanelTitle.Size = new System.Drawing.Size(911, 50);
            this.PanelTitle.TabIndex = 7;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApplicationTitle.Appearance.Options.UseFont = true;
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationTitle.Location = new System.Drawing.Point(8, 8);
            this.lblApplicationTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(108, 27);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "工单报工";
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.toolStripMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolStripMain.BackgroundImage")));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbWorkOrderWorkFor,
            this.toolStripSeparator1,
            this.tsbClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(917, 27);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tsbWorkOrderWorkFor
            // 
            this.tsbWorkOrderWorkFor.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.submit;
            this.tsbWorkOrderWorkFor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWorkOrderWorkFor.Name = "tsbWorkOrderWorkFor";
            this.tsbWorkOrderWorkFor.Size = new System.Drawing.Size(93, 24);
            this.tsbWorkOrderWorkFor.Text = "工单报工";
            this.tsbWorkOrderWorkFor.Click += new System.EventHandler(this.tsbWorkOrderWorkFor_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.cancel;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(63, 24);
            this.tsbClose.Text = "关闭";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // Content
            // 
            this.Content.ColumnCount = 1;
            this.Content.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Content.Controls.Add(this.grpQueryArea, 0, 0);
            this.Content.Controls.Add(this.grpWordOrderQingDan, 0, 1);
            this.Content.Controls.Add(this.paginationWorkOrderWorkFors, 0, 2);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 85);
            this.Content.Margin = new System.Windows.Forms.Padding(0);
            this.Content.Name = "Content";
            this.Content.RowCount = 3;
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.47369F));
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.52632F));
            this.Content.Size = new System.Drawing.Size(917, 827);
            this.Content.TabIndex = 5;
            // 
            // grpQueryArea
            // 
            this.grpQueryArea.Controls.Add(this.layoutControl2);
            this.grpQueryArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpQueryArea.Location = new System.Drawing.Point(3, 4);
            this.grpQueryArea.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpQueryArea.Name = "grpQueryArea";
            this.grpQueryArea.Size = new System.Drawing.Size(911, 146);
            this.grpQueryArea.TabIndex = 4;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.lpShift);
            this.layoutControl2.Controls.Add(this.lpWordNumber);
            this.layoutControl2.Controls.Add(this.lpFacName);
            this.layoutControl2.Controls.Add(this.lpWordOrderPeople);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(2, 28);
            this.layoutControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(907, 116);
            this.layoutControl2.TabIndex = 15;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // lpShift
            // 
            this.lpShift.Enabled = false;
            this.lpShift.Location = new System.Drawing.Point(541, 36);
            this.lpShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lpShift.Name = "lpShift";
            this.lpShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lpShift.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "CODE")});
            this.lpShift.Properties.NullText = "";
            this.lpShift.Size = new System.Drawing.Size(358, 24);
            this.lpShift.StyleController = this.layoutControl2;
            this.lpShift.TabIndex = 16;
            // 
            // lpWordNumber
            // 
            this.lpWordNumber.Location = new System.Drawing.Point(541, 8);
            this.lpWordNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lpWordNumber.Name = "lpWordNumber";
            this.lpWordNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lpWordNumber.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ORDER_NUMBER", "工单号", 5, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.lpWordNumber.Properties.NullText = "";
            this.lpWordNumber.Size = new System.Drawing.Size(358, 24);
            this.lpWordNumber.StyleController = this.layoutControl2;
            this.lpWordNumber.TabIndex = 14;
            // 
            // lpFacName
            // 
            this.lpFacName.Location = new System.Drawing.Point(86, 8);
            this.lpFacName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lpFacName.Name = "lpFacName";
            this.lpFacName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lpFacName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", "名称")});
            this.lpFacName.Properties.NullText = "";
            this.lpFacName.Size = new System.Drawing.Size(373, 24);
            this.lpFacName.StyleController = this.layoutControl2;
            this.lpFacName.TabIndex = 11;
            this.lpFacName.TabStop = false;
            this.lpFacName.TextChanged += new System.EventHandler(this.lpFacName_TextChanged);
            // 
            // lpWordOrderPeople
            // 
            this.lpWordOrderPeople.EditValue = "";
            this.lpWordOrderPeople.Enabled = false;
            this.lpWordOrderPeople.Location = new System.Drawing.Point(86, 36);
            this.lpWordOrderPeople.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lpWordOrderPeople.Name = "lpWordOrderPeople";
            this.lpWordOrderPeople.Properties.NullText = "[EditValue is null]";
            this.lpWordOrderPeople.Size = new System.Drawing.Size(373, 24);
            this.lpWordOrderPeople.StyleController = this.layoutControl2;
            this.lpWordOrderPeople.TabIndex = 15;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblFacName,
            this.lblWordNumber,
            this.lblWordOrderPeople,
            this.layoutControlItem2});
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlGroup2.Size = new System.Drawing.Size(907, 116);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // lblFacName
            // 
            this.lblFacName.Control = this.lpFacName;
            this.lblFacName.CustomizationFormText = "工厂车间：";
            this.lblFacName.Location = new System.Drawing.Point(0, 0);
            this.lblFacName.Name = "lblFacName";
            this.lblFacName.Size = new System.Drawing.Size(455, 28);
            this.lblFacName.Text = "工厂车间：";
            this.lblFacName.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lblWordNumber
            // 
            this.lblWordNumber.Control = this.lpWordNumber;
            this.lblWordNumber.CustomizationFormText = "工单号：";
            this.lblWordNumber.Location = new System.Drawing.Point(455, 0);
            this.lblWordNumber.Name = "lblWordNumber";
            this.lblWordNumber.Size = new System.Drawing.Size(440, 28);
            this.lblWordNumber.Text = "工单号：";
            this.lblWordNumber.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lblWordOrderPeople
            // 
            this.lblWordOrderPeople.Control = this.lpWordOrderPeople;
            this.lblWordOrderPeople.CustomizationFormText = "layoutControlItem1";
            this.lblWordOrderPeople.Location = new System.Drawing.Point(0, 28);
            this.lblWordOrderPeople.Name = "lblWordOrderPeople";
            this.lblWordOrderPeople.Size = new System.Drawing.Size(455, 76);
            this.lblWordOrderPeople.Text = "报工人：";
            this.lblWordOrderPeople.TextSize = new System.Drawing.Size(75, 18);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lpShift;
            this.layoutControlItem2.CustomizationFormText = "班次：";
            this.layoutControlItem2.Location = new System.Drawing.Point(455, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(440, 76);
            this.layoutControlItem2.Text = "班次：";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(75, 18);
            // 
            // grpWordOrderQingDan
            // 
            this.grpWordOrderQingDan.Controls.Add(this.grdCrtlCode);
            this.grpWordOrderQingDan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpWordOrderQingDan.Location = new System.Drawing.Point(3, 158);
            this.grpWordOrderQingDan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpWordOrderQingDan.Name = "grpWordOrderQingDan";
            this.grpWordOrderQingDan.Size = new System.Drawing.Size(911, 594);
            this.grpWordOrderQingDan.TabIndex = 5;
            this.grpWordOrderQingDan.Text = "工单报工清单";
            // 
            // grdCrtlCode
            // 
            this.grdCrtlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCrtlCode.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdCrtlCode.Location = new System.Drawing.Point(2, 28);
            this.grdCrtlCode.LookAndFeel.SkinName = "Coffee";
            this.grdCrtlCode.MainView = this.gvWordOrderList;
            this.grdCrtlCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdCrtlCode.Name = "grdCrtlCode";
            this.grdCrtlCode.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lpCodeType});
            this.grdCrtlCode.Size = new System.Drawing.Size(907, 564);
            this.grdCrtlCode.TabIndex = 4;
            this.grdCrtlCode.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWordOrderList});
            // 
            // gvWordOrderList
            // 
            this.gvWordOrderList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grdNumber,
            this.grdAckTime,
            this.grdAufnr,
            this.grdAplfl,
            this.grdGmnga,
            this.grdXmnga,
            this.grdReportor,
            this.grdShift,
            this.grdConend,
            this.grdWerks,
            this.grdRoomkey});
            this.gvWordOrderList.DetailHeight = 450;
            this.gvWordOrderList.FixedLineWidth = 3;
            this.gvWordOrderList.GridControl = this.grdCrtlCode;
            this.gvWordOrderList.Name = "gvWordOrderList";
            this.gvWordOrderList.OptionsBehavior.Editable = false;
            this.gvWordOrderList.OptionsView.ShowGroupPanel = false;
            this.gvWordOrderList.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvWordOrderList_CustomDrawCell);
            // 
            // grdNumber
            // 
            this.grdNumber.Caption = "序号";
            this.grdNumber.FieldName = "ROWNUM";
            this.grdNumber.MinWidth = 23;
            this.grdNumber.Name = "grdNumber";
            this.grdNumber.Visible = true;
            this.grdNumber.VisibleIndex = 0;
            this.grdNumber.Width = 86;
            // 
            // grdAckTime
            // 
            this.grdAckTime.Caption = "报工时间";
            this.grdAckTime.FieldName = "ACKDATETIME";
            this.grdAckTime.MinWidth = 23;
            this.grdAckTime.Name = "grdAckTime";
            this.grdAckTime.Visible = true;
            this.grdAckTime.VisibleIndex = 1;
            this.grdAckTime.Width = 86;
            // 
            // grdAufnr
            // 
            this.grdAufnr.Caption = "工单号";
            this.grdAufnr.FieldName = "AUFNR";
            this.grdAufnr.MinWidth = 23;
            this.grdAufnr.Name = "grdAufnr";
            this.grdAufnr.Visible = true;
            this.grdAufnr.VisibleIndex = 2;
            this.grdAufnr.Width = 86;
            // 
            // grdAplfl
            // 
            this.grdAplfl.Caption = "工序号";
            this.grdAplfl.FieldName = "APLFL";
            this.grdAplfl.MinWidth = 23;
            this.grdAplfl.Name = "grdAplfl";
            this.grdAplfl.Visible = true;
            this.grdAplfl.VisibleIndex = 3;
            this.grdAplfl.Width = 86;
            // 
            // grdGmnga
            // 
            this.grdGmnga.Caption = "合格品数量";
            this.grdGmnga.FieldName = "GMNGA";
            this.grdGmnga.MinWidth = 23;
            this.grdGmnga.Name = "grdGmnga";
            this.grdGmnga.Visible = true;
            this.grdGmnga.VisibleIndex = 4;
            this.grdGmnga.Width = 86;
            // 
            // grdXmnga
            // 
            this.grdXmnga.Caption = "报废数量";
            this.grdXmnga.FieldName = "XMNGA";
            this.grdXmnga.MinWidth = 23;
            this.grdXmnga.Name = "grdXmnga";
            this.grdXmnga.Visible = true;
            this.grdXmnga.VisibleIndex = 5;
            this.grdXmnga.Width = 86;
            // 
            // grdReportor
            // 
            this.grdReportor.Caption = "报工人";
            this.grdReportor.FieldName = "REPORTOR";
            this.grdReportor.MinWidth = 23;
            this.grdReportor.Name = "grdReportor";
            this.grdReportor.Visible = true;
            this.grdReportor.VisibleIndex = 6;
            this.grdReportor.Width = 86;
            // 
            // grdShift
            // 
            this.grdShift.Caption = "报工班次";
            this.grdShift.FieldName = "SHIFT_NAME";
            this.grdShift.MinWidth = 23;
            this.grdShift.Name = "grdShift";
            this.grdShift.Visible = true;
            this.grdShift.VisibleIndex = 7;
            this.grdShift.Width = 86;
            // 
            // grdConend
            // 
            this.grdConend.Caption = "最终确认";
            this.grdConend.FieldName = "CON_END";
            this.grdConend.MinWidth = 23;
            this.grdConend.Name = "grdConend";
            this.grdConend.Visible = true;
            this.grdConend.VisibleIndex = 8;
            this.grdConend.Width = 86;
            // 
            // grdWerks
            // 
            this.grdWerks.Caption = "工厂代码";
            this.grdWerks.FieldName = "WERKS";
            this.grdWerks.MinWidth = 23;
            this.grdWerks.Name = "grdWerks";
            this.grdWerks.Visible = true;
            this.grdWerks.VisibleIndex = 9;
            this.grdWerks.Width = 86;
            // 
            // grdRoomkey
            // 
            this.grdRoomkey.Caption = "工厂车间";
            this.grdRoomkey.FieldName = "LOCATION_NAME";
            this.grdRoomkey.MinWidth = 23;
            this.grdRoomkey.Name = "grdRoomkey";
            this.grdRoomkey.Visible = true;
            this.grdRoomkey.VisibleIndex = 10;
            this.grdRoomkey.Width = 86;
            // 
            // lpCodeType
            // 
            this.lpCodeType.AutoHeight = false;
            this.lpCodeType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lpCodeType.Name = "lpCodeType";
            this.lpCodeType.NullText = "";
            // 
            // paginationWorkOrderWorkFors
            // 
            this.paginationWorkOrderWorkFors.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.paginationWorkOrderWorkFors.Appearance.Options.UseBackColor = true;
            this.paginationWorkOrderWorkFors.AutoSize = true;
            this.paginationWorkOrderWorkFors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paginationWorkOrderWorkFors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paginationWorkOrderWorkFors.Location = new System.Drawing.Point(3, 761);
            this.paginationWorkOrderWorkFors.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.paginationWorkOrderWorkFors.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.paginationWorkOrderWorkFors.Name = "paginationWorkOrderWorkFors";
            this.paginationWorkOrderWorkFors.PageNo = 1;
            this.paginationWorkOrderWorkFors.Pages = 0;
            this.paginationWorkOrderWorkFors.PageSize = 20;
            this.paginationWorkOrderWorkFors.Records = 0;
            this.paginationWorkOrderWorkFors.Size = new System.Drawing.Size(911, 61);
            this.paginationWorkOrderWorkFors.TabIndex = 6;
            this.paginationWorkOrderWorkFors.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.paginationWorkOrderWorkFors_DataPaging);
            // 
            // WorkOrderWorkForCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "WorkOrderWorkForCtrl";
            this.Size = new System.Drawing.Size(919, 912);
            this.Load += new System.EventHandler(this.WorkOrderWorkForCtrl_Load);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.Content.ResumeLayout(false);
            this.Content.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpQueryArea)).EndInit();
            this.grpQueryArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lpShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpWordNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpFacName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpWordOrderPeople.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFacName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWordNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWordOrderPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpWordOrderQingDan)).EndInit();
            this.grpWordOrderQingDan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCrtlCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWordOrderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpCodeType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tsbWorkOrderWorkFor;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraEditors.GroupControl grpQueryArea;
        private DevExpress.XtraEditors.LookUpEdit lpWordNumber;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem lblFacName;
        private DevExpress.XtraLayout.LayoutControlItem lblWordNumber;
        private System.Windows.Forms.TableLayoutPanel Content;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private DevExpress.XtraEditors.LookUpEdit lpFacName;
        private DevExpress.XtraLayout.LayoutControlItem lblWordOrderPeople;
        private DevExpress.XtraEditors.LookUpEdit lpShift;
        private DevExpress.XtraEditors.TextEdit lpWordOrderPeople;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.GroupControl grpWordOrderQingDan;
        private DevExpress.XtraGrid.GridControl grdCrtlCode;
        private DevExpress.XtraGrid.Views.Grid.GridView gvWordOrderList;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lpCodeType;
        private DevExpress.XtraGrid.Columns.GridColumn grdAckTime;
        private DevExpress.XtraGrid.Columns.GridColumn grdAufnr;
        private DevExpress.XtraGrid.Columns.GridColumn grdAplfl;
        private DevExpress.XtraGrid.Columns.GridColumn grdGmnga;
        private DevExpress.XtraGrid.Columns.GridColumn grdXmnga;
        private DevExpress.XtraGrid.Columns.GridColumn grdReportor;
        private DevExpress.XtraGrid.Columns.GridColumn grdShift;
        private DevExpress.XtraGrid.Columns.GridColumn grdConend;
        private DevExpress.XtraGrid.Columns.GridColumn grdWerks;
        private DevExpress.XtraGrid.Columns.GridColumn grdRoomkey;
        private DevExpress.XtraGrid.Columns.GridColumn grdNumber;
        private FanHai.Hemera.Utils.Controls.PaginationControl paginationWorkOrderWorkFors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

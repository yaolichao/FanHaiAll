namespace FanHai.Hemera.Addins.WMS
{
    partial class ShipManagementCtrl
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
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tspSave = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.teCINumber = new DevExpress.XtraEditors.TextEdit();
            this.pgnQueryResult = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gclPalletNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclWorkorderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclSAPNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclPowerLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclShipmentNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclContainerNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclShipmentType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclShipmentDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclShipmentOperator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclArtNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclGustCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lueShipmentType = new DevExpress.XtraEditors.LookUpEdit();
            this.teContainerNo = new DevExpress.XtraEditors.TextEdit();
            this.teShipmentNo = new DevExpress.XtraEditors.TextEdit();
            this.tePalletNo = new DevExpress.XtraEditors.MemoEdit();
            this.lcgTopMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciList = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgInpupt = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciPalletNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnRemove = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciShipmentNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciContainerNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCINumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciShipmentType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPagination = new DevExpress.XtraLayout.LayoutControlItem();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblDevice = new DevExpress.XtraLayout.LayoutControlItem();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teCINumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShipmentType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teContainerNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teShipmentNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShipmentNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciContainerNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCINumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShipmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPagination)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDevice)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApplicationTitle.Appearance.Options.UseFont = true;
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationTitle.Location = new System.Drawing.Point(8, 8);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(84, 21);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "出货查询";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tspSave});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(740, 25);
            this.toolStripMain.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.cancel;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(49, 22);
            this.tsbClose.Text = "关闭";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tspSave
            // 
            this.tspSave.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.save_accept;
            this.tspSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspSave.Name = "tspSave";
            this.tspSave.Size = new System.Drawing.Size(49, 22);
            this.tspSave.Text = "保存";
            this.tspSave.Click += new System.EventHandler(this.tspSave_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(740, 430);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // Content
            // 
            this.Content.AllowCustomizationMenu = false;
            this.Content.Controls.Add(this.teCINumber);
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.gcList);
            this.Content.Controls.Add(this.btnQuery);
            this.Content.Controls.Add(this.lueShipmentType);
            this.Content.Controls.Add(this.teContainerNo);
            this.Content.Controls.Add(this.teShipmentNo);
            this.Content.Controls.Add(this.tePalletNo);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 74);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgTopMain;
            this.Content.Size = new System.Drawing.Size(734, 353);
            this.Content.TabIndex = 0;
            this.Content.Text = "layoutControl1";
            // 
            // teCINumber
            // 
            this.teCINumber.Location = new System.Drawing.Point(406, 27);
            this.teCINumber.Name = "teCINumber";
            this.teCINumber.Properties.MaxLength = 30;
            this.teCINumber.Size = new System.Drawing.Size(133, 21);
            this.teCINumber.StyleController = this.Content;
            this.teCINumber.TabIndex = 42;
            // 
            // pgnQueryResult
            // 
            this.pgnQueryResult.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.pgnQueryResult.Appearance.Options.UseBackColor = true;
            this.pgnQueryResult.Location = new System.Drawing.Point(2, 320);
            this.pgnQueryResult.Margin = new System.Windows.Forms.Padding(0);
            this.pgnQueryResult.Name = "pgnQueryResult";
            this.pgnQueryResult.PageNo = 1;
            this.pgnQueryResult.Pages = 0;
            this.pgnQueryResult.PageSize = 200;
            this.pgnQueryResult.Records = 0;
            this.pgnQueryResult.Size = new System.Drawing.Size(730, 31);
            this.pgnQueryResult.TabIndex = 64;
            this.pgnQueryResult.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.pgnQueryResult_DataPaging);
            // 
            // gcList
            // 
            this.gcList.Location = new System.Drawing.Point(2, 116);
            this.gcList.MainView = this.gvList;
            this.gcList.Name = "gcList";
            this.gcList.Size = new System.Drawing.Size(730, 200);
            this.gcList.TabIndex = 12;
            this.gcList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvList});
            // 
            // gvList
            // 
            this.gvList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gclPalletNo,
            this.gclQuantity,
            this.gclWorkorderNo,
            this.gclSAPNo,
            this.gclPowerLevel,
            this.gclShipmentNo,
            this.gclContainerNo,
            this.gclShipmentType,
            this.gclShipmentDate,
            this.gclShipmentOperator,
            this.gclArtNo,
            this.gclGustCheck,
            this.gclFlag});
            this.gvList.GridControl = this.gcList;
            this.gvList.Name = "gvList";
            this.gvList.OptionsCustomization.AllowColumnMoving = false;
            this.gvList.OptionsCustomization.AllowFilter = false;
            this.gvList.OptionsView.ShowGroupPanel = false;
            this.gvList.OptionsView.ShowIndicator = false;
            this.gvList.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvList_CellValueChanging);
            this.gvList.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvList_ShowingEditor);
            // 
            // gclPalletNo
            // 
            this.gclPalletNo.Caption = "托盘号";
            this.gclPalletNo.FieldName = "VIRTUAL_PALLET_NO";
            this.gclPalletNo.Name = "gclPalletNo";
            this.gclPalletNo.OptionsColumn.ReadOnly = true;
            this.gclPalletNo.Visible = true;
            this.gclPalletNo.VisibleIndex = 0;
            // 
            // gclQuantity
            // 
            this.gclQuantity.Caption = "组件数量";
            this.gclQuantity.FieldName = "LOT_NUMBER_QTY";
            this.gclQuantity.Name = "gclQuantity";
            this.gclQuantity.OptionsColumn.ReadOnly = true;
            this.gclQuantity.SummaryItem.FieldName = "QUANTITY";
            this.gclQuantity.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.gclQuantity.Visible = true;
            this.gclQuantity.VisibleIndex = 1;
            // 
            // gclWorkorderNo
            // 
            this.gclWorkorderNo.Caption = "工单号";
            this.gclWorkorderNo.FieldName = "WORKNUMBER";
            this.gclWorkorderNo.Name = "gclWorkorderNo";
            this.gclWorkorderNo.OptionsColumn.ReadOnly = true;
            this.gclWorkorderNo.Visible = true;
            this.gclWorkorderNo.VisibleIndex = 2;
            // 
            // gclSAPNo
            // 
            this.gclSAPNo.Caption = "SAP料号";
            this.gclSAPNo.FieldName = "SAP_NO";
            this.gclSAPNo.Name = "gclSAPNo";
            this.gclSAPNo.OptionsColumn.ReadOnly = true;
            this.gclSAPNo.Visible = true;
            this.gclSAPNo.VisibleIndex = 3;
            // 
            // gclPowerLevel
            // 
            this.gclPowerLevel.Caption = "功率档位";
            this.gclPowerLevel.FieldName = "POWER_LEVEL";
            this.gclPowerLevel.Name = "gclPowerLevel";
            this.gclPowerLevel.OptionsColumn.ReadOnly = true;
            this.gclPowerLevel.Visible = true;
            this.gclPowerLevel.VisibleIndex = 4;
            // 
            // gclShipmentNo
            // 
            this.gclShipmentNo.Caption = "出货单号";
            this.gclShipmentNo.FieldName = "SHIPMENT_NO";
            this.gclShipmentNo.Name = "gclShipmentNo";
            this.gclShipmentNo.OptionsColumn.ReadOnly = true;
            this.gclShipmentNo.Visible = true;
            this.gclShipmentNo.VisibleIndex = 5;
            // 
            // gclContainerNo
            // 
            this.gclContainerNo.Caption = "货柜号";
            this.gclContainerNo.FieldName = "CONTAINER_NO";
            this.gclContainerNo.Name = "gclContainerNo";
            this.gclContainerNo.Visible = true;
            this.gclContainerNo.VisibleIndex = 6;
            // 
            // gclShipmentType
            // 
            this.gclShipmentType.Caption = "运输类型";
            this.gclShipmentType.FieldName = "SHIPMENT_TYPE";
            this.gclShipmentType.Name = "gclShipmentType";
            this.gclShipmentType.OptionsColumn.ReadOnly = true;
            this.gclShipmentType.Visible = true;
            this.gclShipmentType.VisibleIndex = 7;
            // 
            // gclShipmentDate
            // 
            this.gclShipmentDate.Caption = "出货日期";
            this.gclShipmentDate.FieldName = "SHIPMENT_DATE";
            this.gclShipmentDate.Name = "gclShipmentDate";
            this.gclShipmentDate.OptionsColumn.ReadOnly = true;
            this.gclShipmentDate.Visible = true;
            this.gclShipmentDate.VisibleIndex = 8;
            // 
            // gclShipmentOperator
            // 
            this.gclShipmentOperator.Caption = "出货人";
            this.gclShipmentOperator.FieldName = "SHIPMENT_OPERATOR";
            this.gclShipmentOperator.Name = "gclShipmentOperator";
            this.gclShipmentOperator.OptionsColumn.ReadOnly = true;
            this.gclShipmentOperator.Visible = true;
            this.gclShipmentOperator.VisibleIndex = 9;
            // 
            // gclArtNo
            // 
            this.gclArtNo.Caption = "Art.No.";
            this.gclArtNo.FieldName = "ARTICNO";
            this.gclArtNo.Name = "gclArtNo";
            this.gclArtNo.OptionsColumn.ReadOnly = true;
            this.gclArtNo.Visible = true;
            this.gclArtNo.VisibleIndex = 10;
            // 
            // gclGustCheck
            // 
            this.gclGustCheck.Caption = "是否客检";
            this.gclGustCheck.FieldName = "CUSTCHECK";
            this.gclGustCheck.Name = "gclGustCheck";
            this.gclGustCheck.OptionsColumn.ReadOnly = true;
            this.gclGustCheck.Visible = true;
            this.gclGustCheck.VisibleIndex = 11;
            // 
            // gclFlag
            // 
            this.gclFlag.Caption = "标志位";
            this.gclFlag.FieldName = "HFLAG";
            this.gclFlag.Name = "gclFlag";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(632, 52);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(96, 26);
            this.btnQuery.StyleController = this.Content;
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lueShipmentType
            // 
            this.lueShipmentType.Location = new System.Drawing.Point(595, 27);
            this.lueShipmentType.Name = "lueShipmentType";
            this.lueShipmentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueShipmentType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", " "),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "CODE")});
            this.lueShipmentType.Properties.NullText = "";
            this.lueShipmentType.Size = new System.Drawing.Size(133, 21);
            this.lueShipmentType.StyleController = this.Content;
            this.lueShipmentType.TabIndex = 6;
            // 
            // teContainerNo
            // 
            this.teContainerNo.Location = new System.Drawing.Point(237, 27);
            this.teContainerNo.Name = "teContainerNo";
            this.teContainerNo.Size = new System.Drawing.Size(113, 21);
            this.teContainerNo.StyleController = this.Content;
            this.teContainerNo.TabIndex = 5;
            // 
            // teShipmentNo
            // 
            this.teShipmentNo.Location = new System.Drawing.Point(58, 27);
            this.teShipmentNo.Name = "teShipmentNo";
            this.teShipmentNo.Size = new System.Drawing.Size(123, 21);
            this.teShipmentNo.StyleController = this.Content;
            this.teShipmentNo.TabIndex = 4;
            // 
            // tePalletNo
            // 
            this.tePalletNo.Location = new System.Drawing.Point(58, 52);
            this.tePalletNo.Name = "tePalletNo";
            this.tePalletNo.Size = new System.Drawing.Size(570, 56);
            this.tePalletNo.StyleController = this.Content;
            this.tePalletNo.TabIndex = 9;
            this.tePalletNo.TabStop = false;
            // 
            // lcgTopMain
            // 
            this.lcgTopMain.CustomizationFormText = " ";
            this.lcgTopMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgTopMain.GroupBordersVisible = false;
            this.lcgTopMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciList,
            this.lcgInpupt,
            this.lciPagination});
            this.lcgTopMain.Location = new System.Drawing.Point(0, 0);
            this.lcgTopMain.Name = "Root";
            this.lcgTopMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTopMain.Size = new System.Drawing.Size(734, 353);
            this.lcgTopMain.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTopMain.Text = "Root";
            this.lcgTopMain.TextVisible = false;
            // 
            // lciList
            // 
            this.lciList.Control = this.gcList;
            this.lciList.CustomizationFormText = "列表";
            this.lciList.Location = new System.Drawing.Point(0, 114);
            this.lciList.Name = "lciList";
            this.lciList.Size = new System.Drawing.Size(734, 204);
            this.lciList.Text = "列表";
            this.lciList.TextSize = new System.Drawing.Size(0, 0);
            this.lciList.TextToControlDistance = 0;
            this.lciList.TextVisible = false;
            // 
            // lcgInpupt
            // 
            this.lcgInpupt.CustomizationFormText = " ";
            this.lcgInpupt.ExpandButtonVisible = true;
            this.lcgInpupt.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciPalletNo,
            this.lciBtnRemove,
            this.lciShipmentNo,
            this.lciContainerNo,
            this.lciCINumber,
            this.lciShipmentType});
            this.lcgInpupt.Location = new System.Drawing.Point(0, 0);
            this.lcgInpupt.Name = "lcgInpupt";
            this.lcgInpupt.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgInpupt.Size = new System.Drawing.Size(734, 114);
            this.lcgInpupt.Text = " ";
            // 
            // lciPalletNo
            // 
            this.lciPalletNo.Control = this.tePalletNo;
            this.lciPalletNo.CustomizationFormText = "托盘号";
            this.lciPalletNo.Location = new System.Drawing.Point(0, 25);
            this.lciPalletNo.MinSize = new System.Drawing.Size(66, 60);
            this.lciPalletNo.Name = "lciPalletNo";
            this.lciPalletNo.Size = new System.Drawing.Size(626, 60);
            this.lciPalletNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPalletNo.Text = "托盘号";
            this.lciPalletNo.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lciBtnRemove
            // 
            this.lciBtnRemove.Control = this.btnQuery;
            this.lciBtnRemove.CustomizationFormText = "移除";
            this.lciBtnRemove.Location = new System.Drawing.Point(626, 25);
            this.lciBtnRemove.MaxSize = new System.Drawing.Size(100, 30);
            this.lciBtnRemove.MinSize = new System.Drawing.Size(80, 30);
            this.lciBtnRemove.Name = "lciBtnRemove";
            this.lciBtnRemove.Size = new System.Drawing.Size(100, 60);
            this.lciBtnRemove.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnRemove.Text = "移除";
            this.lciBtnRemove.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnRemove.TextToControlDistance = 0;
            this.lciBtnRemove.TextVisible = false;
            // 
            // lciShipmentNo
            // 
            this.lciShipmentNo.Control = this.teShipmentNo;
            this.lciShipmentNo.CustomizationFormText = "出货单号";
            this.lciShipmentNo.Location = new System.Drawing.Point(0, 0);
            this.lciShipmentNo.Name = "lciShipmentNo";
            this.lciShipmentNo.Size = new System.Drawing.Size(179, 25);
            this.lciShipmentNo.Text = "出货单号";
            this.lciShipmentNo.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lciContainerNo
            // 
            this.lciContainerNo.Control = this.teContainerNo;
            this.lciContainerNo.CustomizationFormText = "货柜号";
            this.lciContainerNo.Location = new System.Drawing.Point(179, 0);
            this.lciContainerNo.Name = "lciContainerNo";
            this.lciContainerNo.Size = new System.Drawing.Size(169, 25);
            this.lciContainerNo.Text = "货柜号";
            this.lciContainerNo.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lciCINumber
            // 
            this.lciCINumber.Control = this.teCINumber;
            this.lciCINumber.CustomizationFormText = "CI号";
            this.lciCINumber.Location = new System.Drawing.Point(348, 0);
            this.lciCINumber.Name = "lciCINumber";
            this.lciCINumber.Size = new System.Drawing.Size(189, 25);
            this.lciCINumber.Text = "CI号";
            this.lciCINumber.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lciShipmentType
            // 
            this.lciShipmentType.Control = this.lueShipmentType;
            this.lciShipmentType.CustomizationFormText = "运输类型";
            this.lciShipmentType.Location = new System.Drawing.Point(537, 0);
            this.lciShipmentType.Name = "lciShipmentType";
            this.lciShipmentType.Size = new System.Drawing.Size(189, 25);
            this.lciShipmentType.Text = "运输类型";
            this.lciShipmentType.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lciPagination
            // 
            this.lciPagination.Control = this.pgnQueryResult;
            this.lciPagination.CustomizationFormText = "分页";
            this.lciPagination.Location = new System.Drawing.Point(0, 318);
            this.lciPagination.MaxSize = new System.Drawing.Size(0, 35);
            this.lciPagination.MinSize = new System.Drawing.Size(1, 35);
            this.lciPagination.Name = "lciPagination";
            this.lciPagination.Size = new System.Drawing.Size(734, 35);
            this.lciPagination.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPagination.Text = "分页";
            this.lciPagination.TextSize = new System.Drawing.Size(0, 0);
            this.lciPagination.TextToControlDistance = 0;
            this.lciPagination.TextVisible = false;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(5);
            this.PanelTitle.Size = new System.Drawing.Size(734, 40);
            this.PanelTitle.TabIndex = 0;
            // 
            // lblDevice
            // 
            this.lblDevice.CustomizationFormText = "关联设备：";
            this.lblDevice.Location = new System.Drawing.Point(0, 146);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(621, 25);
            this.lblDevice.Text = "关联设备：";
            this.lblDevice.TextSize = new System.Drawing.Size(60, 14);
            this.lblDevice.TextToControlDistance = 5;
            // 
            // ShipManagementCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "ShipManagementCtrl";
            this.Size = new System.Drawing.Size(740, 430);
            this.Load += new System.EventHandler(this.PalletShipmentQueryCtrl_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teCINumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueShipmentType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teContainerNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teShipmentNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShipmentNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciContainerNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCINumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciShipmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPagination)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDevice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTopMain;
        private DevExpress.XtraLayout.LayoutControlItem lblDevice;
        private DevExpress.XtraEditors.LookUpEdit lueShipmentType;
        private DevExpress.XtraEditors.TextEdit teContainerNo;
        private DevExpress.XtraEditors.TextEdit teShipmentNo;
        private DevExpress.XtraLayout.LayoutControlItem lciShipmentNo;
        private DevExpress.XtraLayout.LayoutControlItem lciContainerNo;
        private DevExpress.XtraLayout.LayoutControlItem lciShipmentType;
        private DevExpress.XtraLayout.LayoutControlItem lciPalletNo;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvList;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnRemove;
        private DevExpress.XtraLayout.LayoutControlItem lciList;
        private DevExpress.XtraLayout.LayoutControlGroup lcgInpupt;
        private DevExpress.XtraGrid.Columns.GridColumn gclShipmentNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclContainerNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclPalletNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclShipmentType;
        private DevExpress.XtraGrid.Columns.GridColumn gclQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gclWorkorderNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclSAPNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclPowerLevel;
        private DevExpress.XtraGrid.Columns.GridColumn gclShipmentDate;
        private DevExpress.XtraGrid.Columns.GridColumn gclShipmentOperator;
        private FanHai.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPagination;
        private DevExpress.XtraEditors.TextEdit teCINumber;
        private DevExpress.XtraLayout.LayoutControlItem lciCINumber;
        private DevExpress.XtraEditors.MemoEdit tePalletNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclArtNo;
        private System.Windows.Forms.ToolStripButton tspSave;
        private DevExpress.XtraGrid.Columns.GridColumn gclGustCheck;
        private DevExpress.XtraGrid.Columns.GridColumn gclFlag;
    }
}

namespace FanHai.Hemera.Addins.SAP
{
    partial class ReceiveMaterialCtrl
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
            this.tsbRefresMaterial = new System.Windows.Forms.ToolStripButton();
            this.tsbReceiveMaterial = new System.Windows.Forms.ToolStripButton();
            this.tsbReceiveList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraEditors.GroupControl();
            this.lcTop = new DevExpress.XtraLayout.LayoutControl();
            this.lueShiftName = new DevExpress.XtraEditors.LookUpEdit();
            this.gcMaterialList = new DevExpress.XtraEditors.GroupControl();
            this.gdcData = new DevExpress.XtraGrid.GridControl();
            this.gdvMaterialDefault = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_SAP_ISSURE_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CHECK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ricChkSelect = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn_ROWNUM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MBLNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CHARG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MATNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MATXT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_AUFNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ERFME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ERFMG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LLIEF = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Gdc_LineStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Param_LineStore = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.Gdc_Operation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Param_Operation = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.txtOperatorNumber = new DevExpress.XtraEditors.TextEdit();
            this.lcgTopMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lbOperatorNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lpShiftName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciMaterialDetail = new DevExpress.XtraLayout.LayoutControlItem();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblDevice = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcTop)).BeginInit();
            this.lcTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueShiftName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMaterialList)).BeginInit();
            this.gcMaterialList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvMaterialDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricChkSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_LineStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Operation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperatorNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbOperatorNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpShiftName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDevice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(1008, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(788, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-03-13 14:00:58";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
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
            this.lblApplicationTitle.Text = "来料接收";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresMaterial,
            this.tsbReceiveMaterial,
            this.tsbReceiveList,
            this.toolStripSeparator1,
            this.tsbClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1009, 27);
            this.toolStripMain.TabIndex = 0;
            // 
            // tsbRefresMaterial
            // 
            this.tsbRefresMaterial.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.arrow_refresh;
            this.tsbRefresMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresMaterial.Name = "tsbRefresMaterial";
            this.tsbRefresMaterial.Size = new System.Drawing.Size(123, 24);
            this.tsbRefresMaterial.Text = "刷新来料数据";
            this.tsbRefresMaterial.Click += new System.EventHandler(this.tsbRefresMaterial_Click);
            // 
            // tsbReceiveMaterial
            // 
            this.tsbReceiveMaterial.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.submit;
            this.tsbReceiveMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReceiveMaterial.Name = "tsbReceiveMaterial";
            this.tsbReceiveMaterial.Size = new System.Drawing.Size(93, 24);
            this.tsbReceiveMaterial.Text = "来料接收";
            this.tsbReceiveMaterial.Click += new System.EventHandler(this.tsbReceiveMaterial_Click);
            // 
            // tsbReceiveList
            // 
            this.tsbReceiveList.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.document_add;
            this.tsbReceiveList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReceiveList.Name = "tsbReceiveList";
            this.tsbReceiveList.Size = new System.Drawing.Size(123, 24);
            this.tsbReceiveList.Text = "来料接收清单";
            this.tsbReceiveList.Click += new System.EventHandler(this.tsbReceiveList_Click);
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
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1008, 765);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // Content
            // 
            this.Content.Controls.Add(this.lcTop);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 90);
            this.Content.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(1003, 671);
            this.Content.TabIndex = 0;
            // 
            // lcTop
            // 
            this.lcTop.Controls.Add(this.lueShiftName);
            this.lcTop.Controls.Add(this.gcMaterialList);
            this.lcTop.Controls.Add(this.txtOperatorNumber);
            this.lcTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcTop.Location = new System.Drawing.Point(2, 28);
            this.lcTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcTop.Name = "lcTop";
            this.lcTop.Root = this.lcgTopMain;
            this.lcTop.Size = new System.Drawing.Size(999, 641);
            this.lcTop.TabIndex = 0;
            this.lcTop.Text = "layoutControl1";
            // 
            // lueShiftName
            // 
            this.lueShiftName.Enabled = false;
            this.lueShiftName.Location = new System.Drawing.Point(52, 5);
            this.lueShiftName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueShiftName.Name = "lueShiftName";
            this.lueShiftName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueShiftName.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "班别")});
            this.lueShiftName.Properties.NullText = "";
            this.lueShiftName.Size = new System.Drawing.Size(475, 24);
            this.lueShiftName.StyleController = this.lcTop;
            this.lueShiftName.TabIndex = 62;
            // 
            // gcMaterialList
            // 
            this.gcMaterialList.Controls.Add(this.gdcData);
            this.gcMaterialList.Location = new System.Drawing.Point(4, 33);
            this.gcMaterialList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcMaterialList.Name = "gcMaterialList";
            this.gcMaterialList.Size = new System.Drawing.Size(991, 603);
            this.gcMaterialList.TabIndex = 1;
            this.gcMaterialList.Text = "物料明细";
            // 
            // gdcData
            // 
            this.gdcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdcData.Location = new System.Drawing.Point(2, 28);
            this.gdcData.MainView = this.gdvMaterialDefault;
            this.gdcData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdcData.Name = "gdcData";
            this.gdcData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Param_Operation,
            this.Param_LineStore,
            this.repositoryItemCheckedComboBoxEdit1,
            this.ricChkSelect});
            this.gdcData.Size = new System.Drawing.Size(987, 573);
            this.gdcData.TabIndex = 16;
            this.gdcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvMaterialDefault});
            // 
            // gdvMaterialDefault
            // 
            this.gdvMaterialDefault.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_SAP_ISSURE_KEY,
            this.gridColumn_CHECK,
            this.gridColumn_ROWNUM,
            this.gridColumn_MBLNR,
            this.gridColumn_CHARG,
            this.gridColumn_MATNR,
            this.gridColumn_MATXT,
            this.gridColumn_AUFNR,
            this.gridColumn_ERFME,
            this.gridColumn_ERFMG,
            this.gridColumn_LLIEF,
            this.Gdc_LineStore,
            this.Gdc_Operation});
            this.gdvMaterialDefault.DetailHeight = 450;
            this.gdvMaterialDefault.FixedLineWidth = 3;
            this.gdvMaterialDefault.GridControl = this.gdcData;
            this.gdvMaterialDefault.Name = "gdvMaterialDefault";
            this.gdvMaterialDefault.OptionsCustomization.AllowSort = false;
            this.gdvMaterialDefault.OptionsView.ShowGroupPanel = false;
            this.gdvMaterialDefault.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gdvMaterialDefault_CustomDrawCell);
            this.gdvMaterialDefault.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gdvMaterialDefault_CustomRowCellEditForEditing);
            this.gdvMaterialDefault.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gdvMaterialDefault_CellValueChanging);
            // 
            // gridColumn_SAP_ISSURE_KEY
            // 
            this.gridColumn_SAP_ISSURE_KEY.Caption = "物料Key";
            this.gridColumn_SAP_ISSURE_KEY.FieldName = "W.SAP_ISSURE_KEY";
            this.gridColumn_SAP_ISSURE_KEY.MinWidth = 23;
            this.gridColumn_SAP_ISSURE_KEY.Name = "gridColumn_SAP_ISSURE_KEY";
            this.gridColumn_SAP_ISSURE_KEY.Width = 86;
            // 
            // gridColumn_CHECK
            // 
            this.gridColumn_CHECK.Caption = "选择";
            this.gridColumn_CHECK.ColumnEdit = this.ricChkSelect;
            this.gridColumn_CHECK.FieldName = "IsSelected";
            this.gridColumn_CHECK.MinWidth = 23;
            this.gridColumn_CHECK.Name = "gridColumn_CHECK";
            this.gridColumn_CHECK.Visible = true;
            this.gridColumn_CHECK.VisibleIndex = 0;
            this.gridColumn_CHECK.Width = 86;
            // 
            // ricChkSelect
            // 
            this.ricChkSelect.Name = "ricChkSelect";
            this.ricChkSelect.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.ricChkSelect.ValueGrayed = true;
            // 
            // gridColumn_ROWNUM
            // 
            this.gridColumn_ROWNUM.Caption = "序号";
            this.gridColumn_ROWNUM.FieldName = "ROWNUM";
            this.gridColumn_ROWNUM.MinWidth = 23;
            this.gridColumn_ROWNUM.Name = "gridColumn_ROWNUM";
            this.gridColumn_ROWNUM.OptionsColumn.AllowEdit = false;
            this.gridColumn_ROWNUM.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ROWNUM.OptionsFilter.AllowFilter = false;
            this.gridColumn_ROWNUM.Visible = true;
            this.gridColumn_ROWNUM.VisibleIndex = 1;
            this.gridColumn_ROWNUM.Width = 86;
            // 
            // gridColumn_MBLNR
            // 
            this.gridColumn_MBLNR.Caption = "来料单号";
            this.gridColumn_MBLNR.FieldName = "MBLNR";
            this.gridColumn_MBLNR.MinWidth = 23;
            this.gridColumn_MBLNR.Name = "gridColumn_MBLNR";
            this.gridColumn_MBLNR.OptionsColumn.AllowEdit = false;
            this.gridColumn_MBLNR.Visible = true;
            this.gridColumn_MBLNR.VisibleIndex = 3;
            this.gridColumn_MBLNR.Width = 86;
            // 
            // gridColumn_CHARG
            // 
            this.gridColumn_CHARG.Caption = "物料批号";
            this.gridColumn_CHARG.FieldName = "CHARG";
            this.gridColumn_CHARG.MinWidth = 23;
            this.gridColumn_CHARG.Name = "gridColumn_CHARG";
            this.gridColumn_CHARG.OptionsColumn.AllowEdit = false;
            this.gridColumn_CHARG.Visible = true;
            this.gridColumn_CHARG.VisibleIndex = 4;
            this.gridColumn_CHARG.Width = 86;
            // 
            // gridColumn_MATNR
            // 
            this.gridColumn_MATNR.Caption = "物料编码";
            this.gridColumn_MATNR.FieldName = "MATNR";
            this.gridColumn_MATNR.MinWidth = 23;
            this.gridColumn_MATNR.Name = "gridColumn_MATNR";
            this.gridColumn_MATNR.OptionsColumn.AllowEdit = false;
            this.gridColumn_MATNR.Visible = true;
            this.gridColumn_MATNR.VisibleIndex = 2;
            this.gridColumn_MATNR.Width = 86;
            // 
            // gridColumn_MATXT
            // 
            this.gridColumn_MATXT.Caption = "物料描述";
            this.gridColumn_MATXT.FieldName = "MATXT";
            this.gridColumn_MATXT.MinWidth = 23;
            this.gridColumn_MATXT.Name = "gridColumn_MATXT";
            this.gridColumn_MATXT.OptionsColumn.AllowEdit = false;
            this.gridColumn_MATXT.Visible = true;
            this.gridColumn_MATXT.VisibleIndex = 5;
            this.gridColumn_MATXT.Width = 86;
            // 
            // gridColumn_AUFNR
            // 
            this.gridColumn_AUFNR.Caption = "工单号码";
            this.gridColumn_AUFNR.FieldName = "AUFNR";
            this.gridColumn_AUFNR.MinWidth = 23;
            this.gridColumn_AUFNR.Name = "gridColumn_AUFNR";
            this.gridColumn_AUFNR.OptionsColumn.AllowEdit = false;
            this.gridColumn_AUFNR.Visible = true;
            this.gridColumn_AUFNR.VisibleIndex = 6;
            this.gridColumn_AUFNR.Width = 86;
            // 
            // gridColumn_ERFME
            // 
            this.gridColumn_ERFME.Caption = "计量单位";
            this.gridColumn_ERFME.FieldName = "ERFME";
            this.gridColumn_ERFME.MinWidth = 23;
            this.gridColumn_ERFME.Name = "gridColumn_ERFME";
            this.gridColumn_ERFME.OptionsColumn.AllowEdit = false;
            this.gridColumn_ERFME.Visible = true;
            this.gridColumn_ERFME.VisibleIndex = 7;
            this.gridColumn_ERFME.Width = 86;
            // 
            // gridColumn_ERFMG
            // 
            this.gridColumn_ERFMG.Caption = "实领数量";
            this.gridColumn_ERFMG.FieldName = "ERFMG";
            this.gridColumn_ERFMG.MinWidth = 23;
            this.gridColumn_ERFMG.Name = "gridColumn_ERFMG";
            this.gridColumn_ERFMG.OptionsColumn.AllowEdit = false;
            this.gridColumn_ERFMG.Visible = true;
            this.gridColumn_ERFMG.VisibleIndex = 8;
            this.gridColumn_ERFMG.Width = 86;
            // 
            // gridColumn_LLIEF
            // 
            this.gridColumn_LLIEF.Caption = "批次供应商";
            this.gridColumn_LLIEF.FieldName = "LLIEF";
            this.gridColumn_LLIEF.MinWidth = 23;
            this.gridColumn_LLIEF.Name = "gridColumn_LLIEF";
            this.gridColumn_LLIEF.OptionsColumn.AllowEdit = false;
            this.gridColumn_LLIEF.Visible = true;
            this.gridColumn_LLIEF.VisibleIndex = 9;
            this.gridColumn_LLIEF.Width = 86;
            // 
            // Gdc_LineStore
            // 
            this.Gdc_LineStore.Caption = "线上仓名称";
            this.Gdc_LineStore.ColumnEdit = this.Param_LineStore;
            this.Gdc_LineStore.FieldName = "OnlineWarehouse";
            this.Gdc_LineStore.MinWidth = 23;
            this.Gdc_LineStore.Name = "Gdc_LineStore";
            this.Gdc_LineStore.Visible = true;
            this.Gdc_LineStore.VisibleIndex = 10;
            this.Gdc_LineStore.Width = 86;
            // 
            // Param_LineStore
            // 
            this.Param_LineStore.AutoHeight = false;
            this.Param_LineStore.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Param_LineStore.Name = "Param_LineStore";
            this.Param_LineStore.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // Gdc_Operation
            // 
            this.Gdc_Operation.Caption = "工序名称";
            this.Gdc_Operation.FieldName = "Operation";
            this.Gdc_Operation.MinWidth = 23;
            this.Gdc_Operation.Name = "Gdc_Operation";
            this.Gdc_Operation.OptionsColumn.AllowEdit = false;
            this.Gdc_Operation.Visible = true;
            this.Gdc_Operation.VisibleIndex = 11;
            this.Gdc_Operation.Width = 86;
            // 
            // Param_Operation
            // 
            this.Param_Operation.AutoHeight = false;
            this.Param_Operation.Name = "Param_Operation";
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            // 
            // txtOperatorNumber
            // 
            this.txtOperatorNumber.Location = new System.Drawing.Point(579, 5);
            this.txtOperatorNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOperatorNumber.Name = "txtOperatorNumber";
            this.txtOperatorNumber.Size = new System.Drawing.Size(416, 24);
            this.txtOperatorNumber.StyleController = this.lcTop;
            this.txtOperatorNumber.TabIndex = 19;
            // 
            // lcgTopMain
            // 
            this.lcgTopMain.CustomizationFormText = " ";
            this.lcgTopMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgTopMain.GroupBordersVisible = false;
            this.lcgTopMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lbOperatorNumber,
            this.lpShiftName,
            this.lciMaterialDetail});
            this.lcgTopMain.Name = "Root";
            this.lcgTopMain.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgTopMain.Size = new System.Drawing.Size(999, 641);
            this.lcgTopMain.TextVisible = false;
            // 
            // lbOperatorNumber
            // 
            this.lbOperatorNumber.Control = this.txtOperatorNumber;
            this.lbOperatorNumber.CustomizationFormText = "员工号";
            this.lbOperatorNumber.Location = new System.Drawing.Point(527, 0);
            this.lbOperatorNumber.Name = "lbOperatorNumber";
            this.lbOperatorNumber.Size = new System.Drawing.Size(468, 28);
            this.lbOperatorNumber.Text = "员工号";
            this.lbOperatorNumber.TextSize = new System.Drawing.Size(45, 18);
            // 
            // lpShiftName
            // 
            this.lpShiftName.Control = this.lueShiftName;
            this.lpShiftName.CustomizationFormText = "班别";
            this.lpShiftName.Location = new System.Drawing.Point(0, 0);
            this.lpShiftName.Name = "lpShiftName";
            this.lpShiftName.Size = new System.Drawing.Size(527, 28);
            this.lpShiftName.Text = "班别";
            this.lpShiftName.TextSize = new System.Drawing.Size(45, 18);
            // 
            // lciMaterialDetail
            // 
            this.lciMaterialDetail.Control = this.gcMaterialList;
            this.lciMaterialDetail.CustomizationFormText = "lciMaterialDetail";
            this.lciMaterialDetail.Location = new System.Drawing.Point(0, 28);
            this.lciMaterialDetail.Name = "lciMaterialDetail";
            this.lciMaterialDetail.Size = new System.Drawing.Size(995, 607);
            this.lciMaterialDetail.TextSize = new System.Drawing.Size(0, 0);
            this.lciMaterialDetail.TextVisible = false;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 31);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(6);
            this.PanelTitle.Size = new System.Drawing.Size(1003, 51);
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
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.CustomizationFormText = "关联设备：";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 146);
            this.layoutControlItem4.Name = "lblDevice";
            this.layoutControlItem4.Size = new System.Drawing.Size(621, 25);
            this.layoutControlItem4.Text = "关联设备：";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(60, 14);
            // 
            // ReceiveMaterialCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ReceiveMaterialCtrl";
            this.Size = new System.Drawing.Size(1010, 765);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcTop)).EndInit();
            this.lcTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueShiftName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMaterialList)).EndInit();
            this.gcMaterialList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvMaterialDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ricChkSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_LineStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Param_Operation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperatorNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbOperatorNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lpShiftName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciMaterialDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDevice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private System.Windows.Forms.ToolStripButton tsbRefresMaterial;
        private System.Windows.Forms.ToolStripButton tsbReceiveMaterial;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraEditors.GroupControl Content;
        private DevExpress.XtraLayout.LayoutControl lcTop;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTopMain;
        private DevExpress.XtraGrid.GridControl gdcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvMaterialDefault;
        private DevExpress.XtraEditors.TextEdit txtOperatorNumber;
        private DevExpress.XtraLayout.LayoutControlItem lbOperatorNumber;
        private System.Windows.Forms.ToolStripButton tsbReceiveList;
        private DevExpress.XtraLayout.LayoutControlItem lblDevice;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.LookUpEdit lueShiftName;
        private DevExpress.XtraLayout.LayoutControlItem lpShiftName;
        private DevExpress.XtraGrid.Columns.GridColumn Gdc_Operation;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox Param_Operation;
        private DevExpress.XtraGrid.Columns.GridColumn Gdc_LineStore;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox Param_LineStore;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LLIEF;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MATXT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_AUFNR;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ERFME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ERFMG;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CHECK;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ROWNUM;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MBLNR;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CHARG;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ricChkSelect;
        private DevExpress.XtraEditors.GroupControl gcMaterialList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SAP_ISSURE_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MATNR;
        private DevExpress.XtraLayout.LayoutControlItem lciMaterialDetail;
    }
}

namespace SolarViewer.Hemera.Addins.WARK
{
    partial class HaveArkedInfQueryCtrl
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetatilList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcArkNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPattleNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcWorkNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcSapNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPic = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPalletStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreater = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreateDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcZmblnr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcWorkorder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcFac = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStyle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDept = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tscSelect = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.pgnQueryResult = new SolarViewer.Hemera.Utils.Controls.PaginationControl();
            this.teEntryNo = new DevExpress.XtraEditors.TextEdit();
            this.txtArkCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lcgTopMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgInpupt = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciArkNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPagination = new DevExpress.XtraLayout.LayoutControlItem();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblDevice = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetatilList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teEntryNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArkCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciArkNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPagination)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDevice)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetatilList
            // 
            this.gvDetatilList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcNum,
            this.gcArkNo,
            this.gcPattleNo,
            this.gcWorkNum,
            this.gcSapNo,
            this.gcPic,
            this.gcPalletStatus,
            this.gcCreater,
            this.gcCreateDate});
            this.gvDetatilList.GridControl = this.gcList;
            this.gvDetatilList.Name = "gvDetatilList";
            this.gvDetatilList.OptionsView.ShowGroupPanel = false;
            // 
            // gcNum
            // 
            this.gcNum.Caption = "序号";
            this.gcNum.FieldName = "INDEX2";
            this.gcNum.Name = "gcNum";
            this.gcNum.Visible = true;
            this.gcNum.VisibleIndex = 0;
            // 
            // gcArkNo
            // 
            this.gcArkNo.Caption = "柜号";
            this.gcArkNo.FieldName = "CONTAINER_CODE";
            this.gcArkNo.Name = "gcArkNo";
            this.gcArkNo.Visible = true;
            this.gcArkNo.VisibleIndex = 1;
            // 
            // gcPattleNo
            // 
            this.gcPattleNo.Caption = "托号";
            this.gcPattleNo.FieldName = "PALLET_NO";
            this.gcPattleNo.Name = "gcPattleNo";
            this.gcPattleNo.Visible = true;
            this.gcPattleNo.VisibleIndex = 2;
            // 
            // gcWorkNum
            // 
            this.gcWorkNum.Caption = "工单号";
            this.gcWorkNum.FieldName = "WORKNUMBER";
            this.gcWorkNum.Name = "gcWorkNum";
            this.gcWorkNum.Visible = true;
            this.gcWorkNum.VisibleIndex = 3;
            // 
            // gcSapNo
            // 
            this.gcSapNo.Caption = "料号";
            this.gcSapNo.FieldName = "SAP_NO";
            this.gcSapNo.Name = "gcSapNo";
            this.gcSapNo.Visible = true;
            this.gcSapNo.VisibleIndex = 4;
            // 
            // gcPic
            // 
            this.gcPic.Caption = "数量";
            this.gcPic.FieldName = "LOT_NUMBER_QTY";
            this.gcPic.Name = "gcPic";
            this.gcPic.Visible = true;
            this.gcPic.VisibleIndex = 5;
            // 
            // gcPalletStatus
            // 
            this.gcPalletStatus.Caption = "状态";
            this.gcPalletStatus.FieldName = "ARK_FLAG";
            this.gcPalletStatus.Name = "gcPalletStatus";
            this.gcPalletStatus.Visible = true;
            this.gcPalletStatus.VisibleIndex = 6;
            // 
            // gcCreater
            // 
            this.gcCreater.Caption = "创建人";
            this.gcCreater.FieldName = "CREATOR";
            this.gcCreater.Name = "gcCreater";
            this.gcCreater.Visible = true;
            this.gcCreater.VisibleIndex = 7;
            // 
            // gcCreateDate
            // 
            this.gcCreateDate.Caption = "创建时间";
            this.gcCreateDate.FieldName = "CDATE";
            this.gcCreateDate.Name = "gcCreateDate";
            this.gcCreateDate.Visible = true;
            this.gcCreateDate.VisibleIndex = 8;
            // 
            // gcList
            // 
            gridLevelNode1.LevelTemplate = this.gvDetatilList;
            gridLevelNode1.RelationName = "DetatilList";
            this.gcList.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcList.Location = new System.Drawing.Point(6, 52);
            this.gcList.MainView = this.gvList;
            this.gcList.Name = "gcList";
            this.gcList.Size = new System.Drawing.Size(722, 260);
            this.gcList.TabIndex = 66;
            this.gcList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvList,
            this.gvDetatilList});
            // 
            // gvList
            // 
            this.gvList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcNumber,
            this.gcZmblnr,
            this.gcWorkorder,
            this.gcFac,
            this.gcOem,
            this.gcStyle,
            this.gcDept,
            this.gcStatus,
            this.gcCreator,
            this.gcCreateTime});
            this.gvList.GridControl = this.gcList;
            this.gvList.Name = "gvList";
            this.gvList.OptionsDetail.ShowDetailTabs = false;
            this.gvList.OptionsView.ShowGroupPanel = false;
            this.gvList.OptionsView.ShowIndicator = false;
            this.gvList.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvList_CustomDrawCell);
            this.gvList.MasterRowGetChildList += new DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventHandler(this.gvList_MasterRowGetChildList);
            this.gvList.MasterRowEmpty += new DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventHandler(this.gvList_MasterRowEmpty);
            this.gvList.MasterRowGetRelationCount += new DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventHandler(this.gvList_MasterRowGetRelationCount);
            // 
            // gcNumber
            // 
            this.gcNumber.Caption = "序号";
            this.gcNumber.FieldName = "INDEX1";
            this.gcNumber.Name = "gcNumber";
            this.gcNumber.Visible = true;
            this.gcNumber.VisibleIndex = 0;
            // 
            // gcZmblnr
            // 
            this.gcZmblnr.Caption = "入库单号";
            this.gcZmblnr.FieldName = "ZMBLNR";
            this.gcZmblnr.Name = "gcZmblnr";
            this.gcZmblnr.Visible = true;
            this.gcZmblnr.VisibleIndex = 1;
            // 
            // gcWorkorder
            // 
            this.gcWorkorder.Caption = "工单号";
            this.gcWorkorder.FieldName = "AUFNR";
            this.gcWorkorder.Name = "gcWorkorder";
            this.gcWorkorder.Visible = true;
            this.gcWorkorder.VisibleIndex = 2;
            // 
            // gcFac
            // 
            this.gcFac.Caption = "工厂";
            this.gcFac.FieldName = "WERKS";
            this.gcFac.Name = "gcFac";
            this.gcFac.Visible = true;
            this.gcFac.VisibleIndex = 3;
            // 
            // gcOem
            // 
            this.gcOem.Caption = "OEM发货单";
            this.gcOem.FieldName = "VBELN_OEM";
            this.gcOem.Name = "gcOem";
            this.gcOem.Visible = true;
            this.gcOem.VisibleIndex = 4;
            // 
            // gcStyle
            // 
            this.gcStyle.Caption = "订单类型";
            this.gcStyle.FieldName = "ZMMTYP";
            this.gcStyle.Name = "gcStyle";
            this.gcStyle.Visible = true;
            this.gcStyle.VisibleIndex = 5;
            // 
            // gcDept
            // 
            this.gcDept.Caption = "部门";
            this.gcDept.FieldName = "DEPT";
            this.gcDept.Name = "gcDept";
            this.gcDept.Visible = true;
            this.gcDept.VisibleIndex = 6;
            // 
            // gcStatus
            // 
            this.gcStatus.Caption = "状态";
            this.gcStatus.FieldName = "STATUS";
            this.gcStatus.Name = "gcStatus";
            this.gcStatus.Visible = true;
            this.gcStatus.VisibleIndex = 7;
            // 
            // gcCreator
            // 
            this.gcCreator.Caption = "创建人";
            this.gcCreator.FieldName = "CREATOR";
            this.gcCreator.Name = "gcCreator";
            this.gcCreator.Visible = true;
            this.gcCreator.VisibleIndex = 8;
            // 
            // gcCreateTime
            // 
            this.gcCreateTime.Caption = "创建时间";
            this.gcCreateTime.FieldName = "CDATE";
            this.gcCreateTime.Name = "gcCreateTime";
            this.gcCreateTime.Visible = true;
            this.gcCreateTime.VisibleIndex = 9;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApplicationTitle.Appearance.Options.UseFont = true;
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationTitle.Location = new System.Drawing.Point(8, 8);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(126, 21);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "组柜信息查询";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscSelect,
            this.tsbClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(740, 25);
            this.toolStripMain.TabIndex = 0;
            // 
            // tscSelect
            // 
            this.tscSelect.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tscSelect.Image = global::SolarViewer.Hemera.Addins.WARK.Properties.Resources.system_search;
            this.tscSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tscSelect.Name = "tscSelect";
            this.tscSelect.Size = new System.Drawing.Size(60, 22);
            this.tscSelect.Text = "查询";
            this.tscSelect.Click += new System.EventHandler(this.tscSelect_Click);
            // 
            // tsbClose
            // 
            this.tsbClose.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsbClose.Image = global::SolarViewer.Hemera.Addins.WARK.Properties.Resources.cancel;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(60, 22);
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
            this.Content.Controls.Add(this.gcList);
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.teEntryNo);
            this.Content.Controls.Add(this.txtArkCode);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 74);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgTopMain;
            this.Content.Size = new System.Drawing.Size(734, 353);
            this.Content.TabIndex = 0;
            this.Content.Text = "layoutControl1";
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
            // 
            // teEntryNo
            // 
            this.teEntryNo.Location = new System.Drawing.Point(74, 27);
            this.teEntryNo.Name = "teEntryNo";
            this.teEntryNo.Size = new System.Drawing.Size(291, 21);
            this.teEntryNo.StyleController = this.Content;
            this.teEntryNo.TabIndex = 4;
            // 
            // txtArkCode
            // 
            this.txtArkCode.Location = new System.Drawing.Point(437, 27);
            this.txtArkCode.Name = "txtArkCode";
            this.txtArkCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtArkCode.Properties.Items.AddRange(new object[] {
            "",
            "已组柜",
            "未组柜"});
            this.txtArkCode.Size = new System.Drawing.Size(291, 21);
            this.txtArkCode.StyleController = this.Content;
            this.txtArkCode.TabIndex = 65;
            this.txtArkCode.TabStop = false;
            // 
            // lcgTopMain
            // 
            this.lcgTopMain.CustomizationFormText = " ";
            this.lcgTopMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgTopMain.GroupBordersVisible = false;
            this.lcgTopMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
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
            // lcgInpupt
            // 
            this.lcgInpupt.CustomizationFormText = " ";
            this.lcgInpupt.ExpandButtonVisible = true;
            this.lcgInpupt.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciArkNo,
            this.lciStatus,
            this.layoutControlItem1});
            this.lcgInpupt.Location = new System.Drawing.Point(0, 0);
            this.lcgInpupt.Name = "lcgInpupt";
            this.lcgInpupt.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgInpupt.Size = new System.Drawing.Size(734, 318);
            this.lcgInpupt.Text = " ";
            // 
            // lciArkNo
            // 
            this.lciArkNo.AppearanceItemCaption.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lciArkNo.AppearanceItemCaption.Options.UseFont = true;
            this.lciArkNo.Control = this.teEntryNo;
            this.lciArkNo.CustomizationFormText = "入库单号";
            this.lciArkNo.Location = new System.Drawing.Point(0, 0);
            this.lciArkNo.Name = "lciArkNo";
            this.lciArkNo.Size = new System.Drawing.Size(363, 25);
            this.lciArkNo.Text = "入库单号";
            this.lciArkNo.TextSize = new System.Drawing.Size(64, 16);
            // 
            // lciStatus
            // 
            this.lciStatus.AppearanceItemCaption.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lciStatus.AppearanceItemCaption.Options.UseFont = true;
            this.lciStatus.Control = this.txtArkCode;
            this.lciStatus.CustomizationFormText = "状 态";
            this.lciStatus.Location = new System.Drawing.Point(363, 0);
            this.lciStatus.Name = "lciStatus";
            this.lciStatus.Size = new System.Drawing.Size(363, 25);
            this.lciStatus.Text = "状 态";
            this.lciStatus.TextSize = new System.Drawing.Size(64, 16);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcList;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(726, 264);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
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
            // HaveArkedInfQueryCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "HaveArkedInfQueryCtrl";
            this.Size = new System.Drawing.Size(740, 430);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetatilList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teEntryNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArkCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciArkNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit teEntryNo;
        private DevExpress.XtraLayout.LayoutControlItem lciArkNo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgInpupt;
        private SolarViewer.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPagination;
        private System.Windows.Forms.ToolStripButton tscSelect;
        private DevExpress.XtraLayout.LayoutControlItem lciStatus;
        private DevExpress.XtraEditors.ComboBoxEdit txtArkCode;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetatilList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvList;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gcNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gcFac;
        private DevExpress.XtraGrid.Columns.GridColumn gcWorkorder;
        private DevExpress.XtraGrid.Columns.GridColumn gcOem;
        private DevExpress.XtraGrid.Columns.GridColumn gcStyle;
        private DevExpress.XtraGrid.Columns.GridColumn gcStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gcDept;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreator;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreateTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcZmblnr;
        private DevExpress.XtraGrid.Columns.GridColumn gcNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcArkNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcPattleNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcWorkNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcSapNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcPalletStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreater;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreateDate;
        private DevExpress.XtraGrid.Columns.GridColumn gcPic;
    }
}

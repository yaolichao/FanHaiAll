namespace SolarViewer.Hemera.Addins.WARK
{
    partial class ArkInfQueryCtrl
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
            this.tscSelect = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.label1 = new System.Windows.Forms.Label();
            this.cbeStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.pgnQueryResult = new SolarViewer.Hemera.Utils.Controls.PaginationControl();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcArkCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPalletNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPrtNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOrderNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreater = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreatetime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcArkKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.teShipmentNo = new DevExpress.XtraEditors.TextEdit();
            this.tePalletNo = new DevExpress.XtraEditors.MemoEdit();
            this.lcgTopMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciList = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgInpupt = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciPalletNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciArkNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPagination = new DevExpress.XtraLayout.LayoutControlItem();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblDevice = new DevExpress.XtraLayout.LayoutControlItem();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teShipmentNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciArkNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.lblApplicationTitle.Size = new System.Drawing.Size(168, 21);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "有无组柜信息查询";
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
            this.Content.Controls.Add(this.label1);
            this.Content.Controls.Add(this.cbeStatus);
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.gcList);
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(662, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 56);
            this.label1.TabIndex = 66;
            this.label1.Text = "注: 多个托号输入时请在各托号之间用逗号隔开或按回车依次换行输入托号";
            // 
            // cbeStatus
            // 
            this.cbeStatus.Location = new System.Drawing.Point(421, 27);
            this.cbeStatus.Name = "cbeStatus";
            this.cbeStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeStatus.Properties.Items.AddRange(new object[] {
            "",
            "已组柜",
            "未组柜"});
            this.cbeStatus.Size = new System.Drawing.Size(307, 21);
            this.cbeStatus.StyleController = this.Content;
            this.cbeStatus.TabIndex = 65;
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
            // gcList
            // 
            this.gcList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gcNumber,
            this.gcArkCode,
            this.gcPalletNo,
            this.gcPrtNum,
            this.gcOrderNum,
            this.gcStatus,
            this.gcCreater,
            this.gcCreatetime,
            this.gcArkKey});
            this.gvList.GridControl = this.gcList;
            this.gvList.Name = "gvList";
            this.gvList.OptionsBehavior.ReadOnly = true;
            this.gvList.OptionsCustomization.AllowColumnMoving = false;
            this.gvList.OptionsCustomization.AllowFilter = false;
            this.gvList.OptionsCustomization.AllowGroup = false;
            this.gvList.OptionsView.ShowGroupPanel = false;
            this.gvList.OptionsView.ShowIndicator = false;
            // 
            // gcNumber
            // 
            this.gcNumber.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcNumber.AppearanceHeader.Options.UseFont = true;
            this.gcNumber.Caption = "序号";
            this.gcNumber.FieldName = "ROWNUMBER";
            this.gcNumber.Name = "gcNumber";
            this.gcNumber.Visible = true;
            this.gcNumber.VisibleIndex = 0;
            // 
            // gcArkCode
            // 
            this.gcArkCode.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcArkCode.AppearanceHeader.Options.UseFont = true;
            this.gcArkCode.Caption = "柜号";
            this.gcArkCode.FieldName = "CONTAINER_CODE";
            this.gcArkCode.Name = "gcArkCode";
            this.gcArkCode.Visible = true;
            this.gcArkCode.VisibleIndex = 1;
            // 
            // gcPalletNo
            // 
            this.gcPalletNo.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPalletNo.AppearanceHeader.Options.UseFont = true;
            this.gcPalletNo.Caption = "托号";
            this.gcPalletNo.FieldName = "PALLET_NO";
            this.gcPalletNo.Name = "gcPalletNo";
            this.gcPalletNo.Visible = true;
            this.gcPalletNo.VisibleIndex = 2;
            // 
            // gcPrtNum
            // 
            this.gcPrtNum.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPrtNum.AppearanceHeader.Options.UseFont = true;
            this.gcPrtNum.Caption = "料号";
            this.gcPrtNum.FieldName = "SAP_NO";
            this.gcPrtNum.Name = "gcPrtNum";
            this.gcPrtNum.Visible = true;
            this.gcPrtNum.VisibleIndex = 3;
            // 
            // gcOrderNum
            // 
            this.gcOrderNum.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcOrderNum.AppearanceHeader.Options.UseFont = true;
            this.gcOrderNum.Caption = "工单号";
            this.gcOrderNum.FieldName = "WORKNUMBER";
            this.gcOrderNum.Name = "gcOrderNum";
            this.gcOrderNum.Visible = true;
            this.gcOrderNum.VisibleIndex = 4;
            // 
            // gcStatus
            // 
            this.gcStatus.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcStatus.AppearanceHeader.Options.UseFont = true;
            this.gcStatus.Caption = "状态";
            this.gcStatus.FieldName = "ARK_FLAG";
            this.gcStatus.Name = "gcStatus";
            this.gcStatus.Visible = true;
            this.gcStatus.VisibleIndex = 5;
            // 
            // gcCreater
            // 
            this.gcCreater.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcCreater.AppearanceHeader.Options.UseFont = true;
            this.gcCreater.Caption = "柜创建人";
            this.gcCreater.FieldName = "CREATOR";
            this.gcCreater.Name = "gcCreater";
            this.gcCreater.Visible = true;
            this.gcCreater.VisibleIndex = 6;
            // 
            // gcCreatetime
            // 
            this.gcCreatetime.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcCreatetime.AppearanceHeader.Options.UseFont = true;
            this.gcCreatetime.Caption = "柜创建时间";
            this.gcCreatetime.FieldName = "CDATE";
            this.gcCreatetime.Name = "gcCreatetime";
            this.gcCreatetime.Visible = true;
            this.gcCreatetime.VisibleIndex = 7;
            // 
            // gcArkKey
            // 
            this.gcArkKey.Caption = "柜主键";
            this.gcArkKey.FieldName = "CONTAINER_KEY";
            this.gcArkKey.Name = "gcArkKey";
            // 
            // teShipmentNo
            // 
            this.teShipmentNo.Location = new System.Drawing.Point(58, 27);
            this.teShipmentNo.Name = "teShipmentNo";
            this.teShipmentNo.Size = new System.Drawing.Size(307, 21);
            this.teShipmentNo.StyleController = this.Content;
            this.teShipmentNo.TabIndex = 4;
            // 
            // tePalletNo
            // 
            this.tePalletNo.Location = new System.Drawing.Point(58, 52);
            this.tePalletNo.Name = "tePalletNo";
            this.tePalletNo.Size = new System.Drawing.Size(600, 56);
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
            this.lciArkNo,
            this.lciStatus,
            this.layoutControlItem1});
            this.lcgInpupt.Location = new System.Drawing.Point(0, 0);
            this.lcgInpupt.Name = "lcgInpupt";
            this.lcgInpupt.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgInpupt.Size = new System.Drawing.Size(734, 114);
            this.lcgInpupt.Text = " ";
            // 
            // lciPalletNo
            // 
            this.lciPalletNo.AppearanceItemCaption.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lciPalletNo.AppearanceItemCaption.Options.UseFont = true;
            this.lciPalletNo.Control = this.tePalletNo;
            this.lciPalletNo.CustomizationFormText = "托盘号";
            this.lciPalletNo.Location = new System.Drawing.Point(0, 25);
            this.lciPalletNo.MinSize = new System.Drawing.Size(66, 60);
            this.lciPalletNo.Name = "lciPalletNo";
            this.lciPalletNo.Size = new System.Drawing.Size(656, 60);
            this.lciPalletNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPalletNo.Text = "托盘号";
            this.lciPalletNo.TextSize = new System.Drawing.Size(48, 16);
            // 
            // lciArkNo
            // 
            this.lciArkNo.AppearanceItemCaption.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lciArkNo.AppearanceItemCaption.Options.UseFont = true;
            this.lciArkNo.Control = this.teShipmentNo;
            this.lciArkNo.CustomizationFormText = "出货单号";
            this.lciArkNo.Location = new System.Drawing.Point(0, 0);
            this.lciArkNo.Name = "lciArkNo";
            this.lciArkNo.Size = new System.Drawing.Size(363, 25);
            this.lciArkNo.Text = "柜 号";
            this.lciArkNo.TextSize = new System.Drawing.Size(48, 16);
            // 
            // lciStatus
            // 
            this.lciStatus.AppearanceItemCaption.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lciStatus.AppearanceItemCaption.Options.UseFont = true;
            this.lciStatus.Control = this.cbeStatus;
            this.lciStatus.CustomizationFormText = "状 态";
            this.lciStatus.Location = new System.Drawing.Point(363, 0);
            this.lciStatus.Name = "lciStatus";
            this.lciStatus.Size = new System.Drawing.Size(363, 25);
            this.lciStatus.Text = "状 态";
            this.lciStatus.TextSize = new System.Drawing.Size(48, 16);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.label1;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(656, 25);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(70, 60);
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
            // ArkInfQueryCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "ArkInfQueryCtrl";
            this.Size = new System.Drawing.Size(740, 430);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbeStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teShipmentNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePalletNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInpupt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPalletNo)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit teShipmentNo;
        private DevExpress.XtraLayout.LayoutControlItem lciArkNo;
        private DevExpress.XtraLayout.LayoutControlItem lciPalletNo;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvList;
        private DevExpress.XtraLayout.LayoutControlItem lciList;
        private DevExpress.XtraLayout.LayoutControlGroup lcgInpupt;
        private SolarViewer.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPagination;
        private DevExpress.XtraEditors.MemoEdit tePalletNo;
        private System.Windows.Forms.ToolStripButton tscSelect;
        private DevExpress.XtraEditors.ComboBoxEdit cbeStatus;
        private DevExpress.XtraLayout.LayoutControlItem lciStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gcArkCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcPalletNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcStatus;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreater;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreatetime;
        private DevExpress.XtraGrid.Columns.GridColumn gcPrtNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcOrderNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcArkKey;
        private DevExpress.XtraGrid.Columns.GridColumn gcNumber;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}

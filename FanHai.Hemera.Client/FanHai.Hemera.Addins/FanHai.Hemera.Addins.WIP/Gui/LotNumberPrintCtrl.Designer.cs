namespace FanHai.Hemera.Addins.WIP
{
    partial class LotNumberPrintCtrl
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.txtLotNumber = new DevExpress.XtraEditors.TextEdit();
            this.deEndTime = new DevExpress.XtraEditors.DateEdit();
            this.deStartTime = new DevExpress.XtraEditors.DateEdit();
            this.lueLotType = new DevExpress.XtraEditors.LookUpEdit();
            this.gcLotInfo = new DevExpress.XtraGrid.GridControl();
            this.gvLotInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.批次key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LOT_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.printQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLotQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOrderNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lueType = new DevExpress.XtraEditors.LookUpEdit();
            this.teWorkOrder = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroupSearch = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciQueryResults = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgQueryInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblWorkOrder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblLotType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStartTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEndTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblLotNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btSearch = new System.Windows.Forms.ToolStripButton();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLotType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLotInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teWorkOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQueryInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWorkOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotNumber)).BeginInit();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1027, 609);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // Content
            // 
            this.Content.AllowCustomizationMenu = false;
            this.Content.Controls.Add(this.txtLotNumber);
            this.Content.Controls.Add(this.deEndTime);
            this.Content.Controls.Add(this.deStartTime);
            this.Content.Controls.Add(this.lueLotType);
            this.Content.Controls.Add(this.gcLotInfo);
            this.Content.Controls.Add(this.lueType);
            this.Content.Controls.Add(this.teWorkOrder);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 73);
            this.Content.Name = "Content";
            this.Content.Root = this.layoutControlGroupSearch;
            this.Content.Size = new System.Drawing.Size(1021, 533);
            this.Content.TabIndex = 11;
            this.Content.Text = "layoutControl1";
            // 
            // txtLotNumber
            // 
            this.txtLotNumber.Location = new System.Drawing.Point(221, 36);
            this.txtLotNumber.Name = "txtLotNumber";
            this.txtLotNumber.Size = new System.Drawing.Size(118, 21);
            this.txtLotNumber.StyleController = this.Content;
            this.txtLotNumber.TabIndex = 13;
            // 
            // deEndTime
            // 
            this.deEndTime.EditValue = null;
            this.deEndTime.Location = new System.Drawing.Point(591, 36);
            this.deEndTime.Name = "deEndTime";
            this.deEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEndTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deEndTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndTime.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deEndTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndTime.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deEndTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEndTime.Size = new System.Drawing.Size(122, 21);
            this.deEndTime.StyleController = this.Content;
            this.deEndTime.TabIndex = 12;
            // 
            // deStartTime
            // 
            this.deStartTime.EditValue = null;
            this.deStartTime.Location = new System.Drawing.Point(411, 36);
            this.deStartTime.Name = "deStartTime";
            this.deStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.deStartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartTime.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.deStartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartTime.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.deStartTime.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStartTime.Size = new System.Drawing.Size(108, 21);
            this.deStartTime.StyleController = this.Content;
            this.deStartTime.TabIndex = 11;
            // 
            // lueLotType
            // 
            this.lueLotType.Location = new System.Drawing.Point(785, 36);
            this.lueLotType.Name = "lueLotType";
            this.lueLotType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueLotType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", " "),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.lueLotType.Properties.NullText = "";
            this.lueLotType.Size = new System.Drawing.Size(65, 21);
            this.lueLotType.StyleController = this.Content;
            this.lueLotType.TabIndex = 10;
            // 
            // gcLotInfo
            // 
            this.gcLotInfo.Location = new System.Drawing.Point(2, 74);
            this.gcLotInfo.LookAndFeel.SkinName = "Coffee";
            this.gcLotInfo.MainView = this.gvLotInfo;
            this.gcLotInfo.Name = "gcLotInfo";
            this.gcLotInfo.Size = new System.Drawing.Size(1017, 457);
            this.gcLotInfo.TabIndex = 1;
            this.gcLotInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLotInfo});
            // 
            // gvLotInfo
            // 
            this.gvLotInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.批次key,
            this.LOT_NUMBER,
            this.printQty,
            this.gcLotQuantity,
            this.gcOrderNumber});
            this.gvLotInfo.GridControl = this.gcLotInfo;
            this.gvLotInfo.Name = "gvLotInfo";
            this.gvLotInfo.OptionsSelection.MultiSelect = true;
            this.gvLotInfo.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvLotInfo_RowClick);
            // 
            // 批次key
            // 
            this.批次key.Caption = "LOT_KEY";
            this.批次key.FieldName = "LOT_KEY";
            this.批次key.Name = "批次key";
            // 
            // LOT_NUMBER
            // 
            this.LOT_NUMBER.AppearanceHeader.Options.UseTextOptions = true;
            this.LOT_NUMBER.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LOT_NUMBER.Caption = "批次号";
            this.LOT_NUMBER.FieldName = "LOT_NUMBER";
            this.LOT_NUMBER.Name = "LOT_NUMBER";
            this.LOT_NUMBER.OptionsColumn.AllowEdit = false;
            this.LOT_NUMBER.Visible = true;
            this.LOT_NUMBER.VisibleIndex = 0;
            // 
            // printQty
            // 
            this.printQty.Caption = "打印次数";
            this.printQty.FieldName = "IS_PRINT";
            this.printQty.Name = "printQty";
            this.printQty.Visible = true;
            this.printQty.VisibleIndex = 1;
            // 
            // gcLotQuantity
            // 
            this.gcLotQuantity.Caption = "批次数量";
            this.gcLotQuantity.FieldName = "QUANTITY";
            this.gcLotQuantity.Name = "gcLotQuantity";
            this.gcLotQuantity.Visible = true;
            this.gcLotQuantity.VisibleIndex = 2;
            // 
            // gcOrderNumber
            // 
            this.gcOrderNumber.Caption = "工单号";
            this.gcOrderNumber.FieldName = "ORDER_NUMBER";
            this.gcOrderNumber.Name = "gcOrderNumber";
            this.gcOrderNumber.Visible = true;
            this.gcOrderNumber.VisibleIndex = 3;
            // 
            // lueType
            // 
            this.lueType.Location = new System.Drawing.Point(922, 36);
            this.lueType.Name = "lueType";
            this.lueType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueType.Properties.NullText = "";
            this.lueType.Size = new System.Drawing.Size(84, 21);
            this.lueType.StyleController = this.Content;
            this.lueType.TabIndex = 7;
            // 
            // teWorkOrder
            // 
            this.teWorkOrder.Location = new System.Drawing.Point(56, 36);
            this.teWorkOrder.Name = "teWorkOrder";
            this.teWorkOrder.Size = new System.Drawing.Size(93, 21);
            this.teWorkOrder.StyleController = this.Content;
            this.teWorkOrder.TabIndex = 1;
            // 
            // layoutControlGroupSearch
            // 
            this.layoutControlGroupSearch.CustomizationFormText = " ";
            this.layoutControlGroupSearch.GroupBordersVisible = false;
            this.layoutControlGroupSearch.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciQueryResults,
            this.lcgQueryInfo});
            this.layoutControlGroupSearch.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupSearch.Name = "layoutControlGroupSearch";
            this.layoutControlGroupSearch.Size = new System.Drawing.Size(1021, 533);
            this.layoutControlGroupSearch.Text = " ";
            // 
            // lciQueryResults
            // 
            this.lciQueryResults.Control = this.gcLotInfo;
            this.lciQueryResults.CustomizationFormText = "查询结果";
            this.lciQueryResults.Location = new System.Drawing.Point(0, 72);
            this.lciQueryResults.Name = "lciQueryResults";
            this.lciQueryResults.Size = new System.Drawing.Size(1021, 461);
            this.lciQueryResults.Text = "查询结果";
            this.lciQueryResults.TextSize = new System.Drawing.Size(0, 0);
            this.lciQueryResults.TextToControlDistance = 0;
            this.lciQueryResults.TextVisible = false;
            // 
            // lcgQueryInfo
            // 
            this.lcgQueryInfo.CustomizationFormText = " ";
            this.lcgQueryInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblWorkOrder,
            this.lblLotType,
            this.lblType,
            this.lciStartTime,
            this.lciEndTime,
            this.lblLotNumber});
            this.lcgQueryInfo.Location = new System.Drawing.Point(0, 0);
            this.lcgQueryInfo.Name = "lcgQueryInfo";
            this.lcgQueryInfo.Size = new System.Drawing.Size(1021, 72);
            this.lcgQueryInfo.Text = " ";
            // 
            // lblWorkOrder
            // 
            this.lblWorkOrder.Control = this.teWorkOrder;
            this.lblWorkOrder.CustomizationFormText = "工单号";
            this.lblWorkOrder.Location = new System.Drawing.Point(0, 0);
            this.lblWorkOrder.Name = "lblWorkOrder";
            this.lblWorkOrder.Size = new System.Drawing.Size(138, 25);
            this.lblWorkOrder.Text = "工单号";
            this.lblWorkOrder.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lblWorkOrder.TextSize = new System.Drawing.Size(36, 14);
            this.lblWorkOrder.TextToControlDistance = 5;
            // 
            // lblLotType
            // 
            this.lblLotType.Control = this.lueLotType;
            this.lblLotType.CustomizationFormText = "批次类型";
            this.lblLotType.Location = new System.Drawing.Point(702, 0);
            this.lblLotType.Name = "lblLotType";
            this.lblLotType.Size = new System.Drawing.Size(137, 25);
            this.lblLotType.Text = "批次类型";
            this.lblLotType.TextSize = new System.Drawing.Size(64, 14);
            // 
            // lblType
            // 
            this.lblType.Control = this.lueType;
            this.lblType.CustomizationFormText = "打印类型";
            this.lblType.Location = new System.Drawing.Point(839, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(156, 25);
            this.lblType.Text = "打印类型";
            this.lblType.TextSize = new System.Drawing.Size(64, 14);
            // 
            // lciStartTime
            // 
            this.lciStartTime.Control = this.deStartTime;
            this.lciStartTime.CustomizationFormText = "创建时间-起";
            this.lciStartTime.Location = new System.Drawing.Point(328, 0);
            this.lciStartTime.Name = "lciStartTime";
            this.lciStartTime.Size = new System.Drawing.Size(180, 25);
            this.lciStartTime.Text = "创建时间-起";
            this.lciStartTime.TextSize = new System.Drawing.Size(64, 14);
            // 
            // lciEndTime
            // 
            this.lciEndTime.Control = this.deEndTime;
            this.lciEndTime.CustomizationFormText = "创建时间-止";
            this.lciEndTime.Location = new System.Drawing.Point(508, 0);
            this.lciEndTime.Name = "lciEndTime";
            this.lciEndTime.Size = new System.Drawing.Size(194, 25);
            this.lciEndTime.Text = "创建时间-止";
            this.lciEndTime.TextSize = new System.Drawing.Size(64, 14);
            // 
            // lblLotNumber
            // 
            this.lblLotNumber.Control = this.txtLotNumber;
            this.lblLotNumber.CustomizationFormText = "批次号";
            this.lblLotNumber.Location = new System.Drawing.Point(138, 0);
            this.lblLotNumber.Name = "lblLotNumber";
            this.lblLotNumber.Size = new System.Drawing.Size(190, 25);
            this.lblLotNumber.Text = "批次号";
            this.lblLotNumber.TextSize = new System.Drawing.Size(64, 14);
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackgroundImage = global::FanHai.Hemera.Addins.WIP.Properties.Resources.toolstrip_bk;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSearch});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1027, 25);
            this.toolStripMain.TabIndex = 2;
            // 
            // btSearch
            // 
            this.btSearch.Image = global::FanHai.Hemera.Addins.WIP.Properties.Resources.system_search;
            this.btSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(52, 22);
            this.btSearch.Text = "查询";
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(5);
            this.PanelTitle.Size = new System.Drawing.Size(1021, 39);
            this.PanelTitle.TabIndex = 66;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(110, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "批次号打印";
            // 
            // LotNumberPrintCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "LotNumberPrintCtrl";
            this.Size = new System.Drawing.Size(1027, 609);
            this.Load += new System.EventHandler(this.LotNumberPrintCtrl_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLotType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLotInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLotInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teWorkOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQueryInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblWorkOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotNumber)).EndInit();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraGrid.GridControl gcLotInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLotInfo;
        private DevExpress.XtraEditors.LookUpEdit lueType;
        private DevExpress.XtraEditors.TextEdit teWorkOrder;
        private DevExpress.XtraGrid.Columns.GridColumn 批次key;
        private DevExpress.XtraGrid.Columns.GridColumn LOT_NUMBER;
        private DevExpress.XtraEditors.LookUpEdit lueLotType;
        private DevExpress.XtraGrid.Columns.GridColumn printQty;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btSearch;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupSearch;
        private DevExpress.XtraLayout.LayoutControlItem lblWorkOrder;
        private DevExpress.XtraLayout.LayoutControlItem lblLotType;
        private DevExpress.XtraLayout.LayoutControlItem lblType;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraLayout.LayoutControlItem lciQueryResults;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQueryInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gcLotQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn gcOrderNumber;
        private DevExpress.XtraEditors.DateEdit deEndTime;
        private DevExpress.XtraEditors.DateEdit deStartTime;
        private DevExpress.XtraLayout.LayoutControlItem lciStartTime;
        private DevExpress.XtraLayout.LayoutControlItem lciEndTime;
        private DevExpress.XtraEditors.TextEdit txtLotNumber;
        private DevExpress.XtraLayout.LayoutControlItem lblLotNumber;
    }
}

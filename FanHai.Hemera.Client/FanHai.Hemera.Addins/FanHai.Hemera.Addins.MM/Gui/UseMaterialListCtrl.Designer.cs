namespace FanHai.Hemera.Addins.MM
{
    partial class UseMaterialListCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UseMaterialListCtrl));
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbSerch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.Content = new System.Windows.Forms.TableLayoutPanel();
            this.grpBottom = new DevExpress.XtraEditors.GroupControl();
            this.gcUsedMaterialList = new DevExpress.XtraGrid.GridControl();
            this.gvUsedMaterialListMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grcNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcLotNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcCaiLiaoNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcCaiLiao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcShuLiang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcDanWei = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcGongYingShang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcLineCang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcGongXu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcFactoryRoom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcEquipment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcBanCi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcHaoYongTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.grcJobNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcJiaoBanTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grcYinShuaNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBottom)).BeginInit();
            this.grpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcUsedMaterialList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsedMaterialListMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            this.SuspendLayout();
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
            this.lblApplicationTitle.Text = "材料耗用清单";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSerch,
            this.toolStripSeparator5,
            this.tsbClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(884, 25);
            this.toolStripMain.TabIndex = 0;
            // 
            // tsbSerch
            // 
            this.tsbSerch.Image = ((System.Drawing.Image)(resources.GetObject("tsbSerch.Image")));
            this.tsbSerch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSerch.Name = "tsbSerch";
            this.tsbSerch.Size = new System.Drawing.Size(49, 22);
            this.tsbSerch.Text = "查询";
            this.tsbSerch.Click += new System.EventHandler(this.tsbSerch_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(49, 22);
            this.tsbClose.Text = "关闭";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(884, 595);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 28);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(5);
            this.PanelTitle.Size = new System.Drawing.Size(878, 40);
            this.PanelTitle.TabIndex = 0;
            // 
            // Content
            // 
            this.Content.ColumnCount = 1;
            this.Content.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Content.Controls.Add(this.grpBottom, 0, 1);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 74);
            this.Content.Name = "Content";
            this.Content.RowCount = 2;
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.544402F));
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.4556F));
            this.Content.Size = new System.Drawing.Size(878, 518);
            this.Content.TabIndex = 1;
            // 
            // grpBottom
            // 
            this.grpBottom.Controls.Add(this.gcUsedMaterialList);
            this.grpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBottom.Location = new System.Drawing.Point(3, 11);
            this.grpBottom.Name = "grpBottom";
            this.grpBottom.Size = new System.Drawing.Size(872, 504);
            this.grpBottom.TabIndex = 1;
            // 
            // gcUsedMaterialList
            // 
            this.gcUsedMaterialList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcUsedMaterialList.Location = new System.Drawing.Point(2, 23);
            this.gcUsedMaterialList.MainView = this.gvUsedMaterialListMain;
            this.gcUsedMaterialList.Name = "gcUsedMaterialList";
            this.gcUsedMaterialList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            this.gcUsedMaterialList.Size = new System.Drawing.Size(868, 479);
            this.gcUsedMaterialList.TabIndex = 0;
            this.gcUsedMaterialList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUsedMaterialListMain});
            // 
            // gvUsedMaterialListMain
            // 
            this.gvUsedMaterialListMain.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.grcNumber,
            this.grcLotNumber,
            this.grcCaiLiaoNumber,
            this.grcCaiLiao,
            this.grcShuLiang,
            this.grcDanWei,
            this.grcGongYingShang,
            this.grcLineCang,
            this.grcGongXu,
            this.grcFactoryRoom,
            this.grcEquipment,
            this.grcBanCi,
            this.grcHaoYongTime,
            this.grcJobNumber,
            this.grcJiaoBanTime,
            this.grcYinShuaNumber});
            this.gvUsedMaterialListMain.GridControl = this.gcUsedMaterialList;
            this.gvUsedMaterialListMain.Name = "gvUsedMaterialListMain";
            this.gvUsedMaterialListMain.OptionsView.ShowGroupPanel = false;
            // 
            // grcNumber
            // 
            this.grcNumber.Caption = "序号";
            this.grcNumber.FieldName = "ROWNUMBER";
            this.grcNumber.Name = "grcNumber";
            this.grcNumber.OptionsColumn.AllowEdit = false;
            this.grcNumber.Visible = true;
            this.grcNumber.VisibleIndex = 0;
            this.grcNumber.Width = 52;
            // 
            // grcLotNumber
            // 
            this.grcLotNumber.Caption = "批次号";
            this.grcLotNumber.FieldName = "MATERIAL_LOT";
            this.grcLotNumber.Name = "grcLotNumber";
            this.grcLotNumber.OptionsColumn.AllowEdit = false;
            this.grcLotNumber.Visible = true;
            this.grcLotNumber.VisibleIndex = 1;
            this.grcLotNumber.Width = 52;
            // 
            // grcCaiLiaoNumber
            // 
            this.grcCaiLiaoNumber.Caption = "材料编码";
            this.grcCaiLiaoNumber.FieldName = "MATNR";
            this.grcCaiLiaoNumber.Name = "grcCaiLiaoNumber";
            this.grcCaiLiaoNumber.OptionsColumn.AllowEdit = false;
            this.grcCaiLiaoNumber.Visible = true;
            this.grcCaiLiaoNumber.VisibleIndex = 2;
            this.grcCaiLiaoNumber.Width = 61;
            // 
            // grcCaiLiao
            // 
            this.grcCaiLiao.Caption = "材料描述";
            this.grcCaiLiao.FieldName = "MATXT";
            this.grcCaiLiao.Name = "grcCaiLiao";
            this.grcCaiLiao.OptionsColumn.AllowEdit = false;
            this.grcCaiLiao.Visible = true;
            this.grcCaiLiao.VisibleIndex = 3;
            this.grcCaiLiao.Width = 58;
            // 
            // grcShuLiang
            // 
            this.grcShuLiang.Caption = "数量";
            this.grcShuLiang.FieldName = "USED_QTY";
            this.grcShuLiang.Name = "grcShuLiang";
            this.grcShuLiang.OptionsColumn.AllowEdit = false;
            this.grcShuLiang.Visible = true;
            this.grcShuLiang.VisibleIndex = 4;
            this.grcShuLiang.Width = 48;
            // 
            // grcDanWei
            // 
            this.grcDanWei.Caption = "单位";
            this.grcDanWei.FieldName = "ERFME";
            this.grcDanWei.Name = "grcDanWei";
            this.grcDanWei.OptionsColumn.AllowEdit = false;
            this.grcDanWei.Visible = true;
            this.grcDanWei.VisibleIndex = 5;
            this.grcDanWei.Width = 48;
            // 
            // grcGongYingShang
            // 
            this.grcGongYingShang.Caption = "供应商";
            this.grcGongYingShang.FieldName = "LLIEF";
            this.grcGongYingShang.Name = "grcGongYingShang";
            this.grcGongYingShang.OptionsColumn.AllowEdit = false;
            this.grcGongYingShang.Visible = true;
            this.grcGongYingShang.VisibleIndex = 6;
            this.grcGongYingShang.Width = 48;
            // 
            // grcLineCang
            // 
            this.grcLineCang.Caption = "线上仓";
            this.grcLineCang.FieldName = "STORE_NAME";
            this.grcLineCang.Name = "grcLineCang";
            this.grcLineCang.OptionsColumn.AllowEdit = false;
            this.grcLineCang.Visible = true;
            this.grcLineCang.VisibleIndex = 7;
            this.grcLineCang.Width = 48;
            // 
            // grcGongXu
            // 
            this.grcGongXu.Caption = "工序";
            this.grcGongXu.FieldName = "ROUTE_OPERATION_NAME";
            this.grcGongXu.Name = "grcGongXu";
            this.grcGongXu.OptionsColumn.AllowEdit = false;
            this.grcGongXu.Visible = true;
            this.grcGongXu.VisibleIndex = 8;
            this.grcGongXu.Width = 48;
            // 
            // grcFactoryRoom
            // 
            this.grcFactoryRoom.Caption = "工厂车间";
            this.grcFactoryRoom.FieldName = "LOCATION_NAME";
            this.grcFactoryRoom.Name = "grcFactoryRoom";
            this.grcFactoryRoom.OptionsColumn.AllowEdit = false;
            this.grcFactoryRoom.Visible = true;
            this.grcFactoryRoom.VisibleIndex = 9;
            // 
            // grcEquipment
            // 
            this.grcEquipment.Caption = "设备";
            this.grcEquipment.FieldName = "EQUIPMENT_NAME";
            this.grcEquipment.Name = "grcEquipment";
            this.grcEquipment.OptionsColumn.AllowEdit = false;
            this.grcEquipment.Visible = true;
            this.grcEquipment.VisibleIndex = 10;
            this.grcEquipment.Width = 43;
            // 
            // grcBanCi
            // 
            this.grcBanCi.Caption = "班次";
            this.grcBanCi.FieldName = "SHIFT_NAME";
            this.grcBanCi.Name = "grcBanCi";
            this.grcBanCi.OptionsColumn.AllowEdit = false;
            this.grcBanCi.Visible = true;
            this.grcBanCi.VisibleIndex = 11;
            this.grcBanCi.Width = 43;
            // 
            // grcHaoYongTime
            // 
            this.grcHaoYongTime.Caption = "耗用时间";
            this.grcHaoYongTime.ColumnEdit = this.repositoryItemDateEdit1;
            this.grcHaoYongTime.FieldName = "USED_TIME";
            this.grcHaoYongTime.Name = "grcHaoYongTime";
            this.grcHaoYongTime.OptionsColumn.AllowEdit = false;
            this.grcHaoYongTime.Visible = true;
            this.grcHaoYongTime.VisibleIndex = 12;
            this.grcHaoYongTime.Width = 38;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Mask.EditMask = "g";
            this.repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = true;
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // grcJobNumber
            // 
            this.grcJobNumber.Caption = "员工号";
            this.grcJobNumber.FieldName = "OPERATOR";
            this.grcJobNumber.Name = "grcJobNumber";
            this.grcJobNumber.OptionsColumn.AllowEdit = false;
            this.grcJobNumber.Visible = true;
            this.grcJobNumber.VisibleIndex = 13;
            this.grcJobNumber.Width = 58;
            // 
            // grcJiaoBanTime
            // 
            this.grcJiaoBanTime.Caption = "搅拌时间";
            this.grcJiaoBanTime.ColumnEdit = this.repositoryItemDateEdit1;
            this.grcJiaoBanTime.FieldName = "STIR_TIME";
            this.grcJiaoBanTime.Name = "grcJiaoBanTime";
            this.grcJiaoBanTime.OptionsColumn.AllowEdit = false;
            this.grcJiaoBanTime.Width = 61;
            // 
            // grcYinShuaNumber
            // 
            this.grcYinShuaNumber.Caption = "印刷数量";
            this.grcYinShuaNumber.FieldName = "PRINT_QTY";
            this.grcYinShuaNumber.Name = "grcYinShuaNumber";
            this.grcYinShuaNumber.OptionsColumn.AllowEdit = false;
            this.grcYinShuaNumber.Width = 66;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Mask.UseMaskAsDisplayFormat = true;
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            this.repositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // UseMaterialListCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "UseMaterialListCtrl";
            this.Size = new System.Drawing.Size(884, 595);
            this.Load += new System.EventHandler(this.UseMaterialListCtrl_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBottom)).EndInit();
            this.grpBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcUsedMaterialList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsedMaterialListMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private System.Windows.Forms.ToolStripButton tsbSerch;
        private System.Windows.Forms.TableLayoutPanel Content;
        private DevExpress.XtraEditors.GroupControl grpBottom;
        private DevExpress.XtraGrid.GridControl gcUsedMaterialList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvUsedMaterialListMain;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraGrid.Columns.GridColumn grcNumber;
        private DevExpress.XtraGrid.Columns.GridColumn grcLotNumber;
        private DevExpress.XtraGrid.Columns.GridColumn grcCaiLiaoNumber;
        private DevExpress.XtraGrid.Columns.GridColumn grcCaiLiao;
        private DevExpress.XtraGrid.Columns.GridColumn grcShuLiang;
        private DevExpress.XtraGrid.Columns.GridColumn grcDanWei;
        private DevExpress.XtraGrid.Columns.GridColumn grcGongYingShang;
        private DevExpress.XtraGrid.Columns.GridColumn grcLineCang;
        private DevExpress.XtraGrid.Columns.GridColumn grcGongXu;
        private DevExpress.XtraGrid.Columns.GridColumn grcFactoryRoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private DevExpress.XtraGrid.Columns.GridColumn grcEquipment;
        private DevExpress.XtraGrid.Columns.GridColumn grcBanCi;
        private DevExpress.XtraGrid.Columns.GridColumn grcHaoYongTime;
        private DevExpress.XtraGrid.Columns.GridColumn grcJobNumber;
        private DevExpress.XtraGrid.Columns.GridColumn grcJiaoBanTime;
        private DevExpress.XtraGrid.Columns.GridColumn grcYinShuaNumber;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
    }
}

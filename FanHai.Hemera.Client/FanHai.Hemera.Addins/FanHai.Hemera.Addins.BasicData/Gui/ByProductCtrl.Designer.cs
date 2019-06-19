namespace FanHai.Hemera.Addins.BasicData
{
    partial class ByProductCtrl
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
            this.Content = new System.Windows.Forms.TableLayoutPanel();
            this.grcContainerIfo = new DevExpress.XtraEditors.GroupControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gvList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcMatnr_M = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatnrMDes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatnr_B2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatnrB2Des = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatnr_B3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatnrB3Des = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPtype = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcWerks = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreater = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreatetime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcEditer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GCkEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pgnQueryResult = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.grpCrtlCode = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtMoudleType = new DevExpress.XtraEditors.LookUpEdit();
            this.txtMtnrB3 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtMtnrB2 = new DevExpress.XtraEditors.ButtonEdit();
            this.txtMtnrm = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblMtnrm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblMtnrB3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblMtnrB2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblMoudleType = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.tsbSeach = new DevExpress.XtraEditors.SimpleButton();
            this.tsbNew = new DevExpress.XtraEditors.SimpleButton();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.tsbDel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grcContainerIfo)).BeginInit();
            this.grcContainerIfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCrtlCode)).BeginInit();
            this.grpCrtlCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMoudleType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrB3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrB2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrB3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrB2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMoudleType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(909, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(689, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 10:01:01";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 60);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(909, 495);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // Content
            // 
            this.Content.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Content.ColumnCount = 1;
            this.Content.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Content.Controls.Add(this.grcContainerIfo, 0, 2);
            this.Content.Controls.Add(this.grpCrtlCode, 0, 0);
            this.Content.Controls.Add(this.panelControl3, 0, 1);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Margin = new System.Windows.Forms.Padding(0);
            this.Content.Name = "Content";
            this.Content.RowCount = 3;
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.Content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.Content.Size = new System.Drawing.Size(909, 495);
            this.Content.TabIndex = 5;
            // 
            // grcContainerIfo
            // 
            this.grcContainerIfo.Controls.Add(this.panelControl2);
            this.grcContainerIfo.Controls.Add(this.panelControl1);
            this.grcContainerIfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcContainerIfo.Location = new System.Drawing.Point(3, 165);
            this.grcContainerIfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grcContainerIfo.Name = "grcContainerIfo";
            this.grcContainerIfo.ShowCaption = false;
            this.grcContainerIfo.Size = new System.Drawing.Size(903, 326);
            this.grcContainerIfo.TabIndex = 13;
            this.grcContainerIfo.Text = "信息5";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gcList);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 2);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(899, 268);
            this.panelControl2.TabIndex = 15;
            // 
            // gcList
            // 
            this.gcList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcList.Location = new System.Drawing.Point(2, 2);
            this.gcList.MainView = this.gvList;
            this.gcList.Margin = new System.Windows.Forms.Padding(6);
            this.gcList.Name = "gcList";
            this.gcList.Size = new System.Drawing.Size(895, 264);
            this.gcList.TabIndex = 13;
            this.gcList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvList});
            // 
            // gvList
            // 
            this.gvList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcMatnr_M,
            this.gcMatnrMDes,
            this.gcMatnr_B2,
            this.gcMatnrB2Des,
            this.gcMatnr_B3,
            this.gcMatnrB3Des,
            this.gcPtype,
            this.gcWerks,
            this.gcCreater,
            this.gcCreatetime,
            this.gcEditer,
            this.gcEditTime,
            this.GCkEY});
            this.gvList.DetailHeight = 450;
            this.gvList.FixedLineWidth = 3;
            this.gvList.GridControl = this.gcList;
            this.gvList.Name = "gvList";
            this.gvList.OptionsBehavior.Editable = false;
            this.gvList.OptionsCustomization.AllowColumnMoving = false;
            this.gvList.OptionsCustomization.AllowFilter = false;
            this.gvList.OptionsCustomization.AllowGroup = false;
            this.gvList.OptionsView.ColumnAutoWidth = false;
            this.gvList.OptionsView.ShowGroupPanel = false;
            this.gvList.OptionsView.ShowIndicator = false;
            this.gvList.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvList_RowClick);
            // 
            // gcMatnr_M
            // 
            this.gcMatnr_M.Caption = "主物料号";
            this.gcMatnr_M.FieldName = "MATNR_M";
            this.gcMatnr_M.MinWidth = 57;
            this.gcMatnr_M.Name = "gcMatnr_M";
            this.gcMatnr_M.Visible = true;
            this.gcMatnr_M.VisibleIndex = 0;
            this.gcMatnr_M.Width = 137;
            // 
            // gcMatnrMDes
            // 
            this.gcMatnrMDes.Caption = "主物料描述";
            this.gcMatnrMDes.FieldName = "PART_DESC";
            this.gcMatnrMDes.MinWidth = 229;
            this.gcMatnrMDes.Name = "gcMatnrMDes";
            this.gcMatnrMDes.Visible = true;
            this.gcMatnrMDes.VisibleIndex = 1;
            this.gcMatnrMDes.Width = 263;
            // 
            // gcMatnr_B2
            // 
            this.gcMatnr_B2.Caption = "低效物料号";
            this.gcMatnr_B2.FieldName = "MATNR_B2";
            this.gcMatnr_B2.MinWidth = 57;
            this.gcMatnr_B2.Name = "gcMatnr_B2";
            this.gcMatnr_B2.Visible = true;
            this.gcMatnr_B2.VisibleIndex = 2;
            this.gcMatnr_B2.Width = 137;
            // 
            // gcMatnrB2Des
            // 
            this.gcMatnrB2Des.Caption = "低效物料描述";
            this.gcMatnrB2Des.FieldName = "B2DESC";
            this.gcMatnrB2Des.MinWidth = 229;
            this.gcMatnrB2Des.Name = "gcMatnrB2Des";
            this.gcMatnrB2Des.Visible = true;
            this.gcMatnrB2Des.VisibleIndex = 3;
            this.gcMatnrB2Des.Width = 263;
            // 
            // gcMatnr_B3
            // 
            this.gcMatnr_B3.Caption = "二三级品物料号";
            this.gcMatnr_B3.FieldName = "MATNR_B3";
            this.gcMatnr_B3.MinWidth = 57;
            this.gcMatnr_B3.Name = "gcMatnr_B3";
            this.gcMatnr_B3.Visible = true;
            this.gcMatnr_B3.VisibleIndex = 4;
            this.gcMatnr_B3.Width = 137;
            // 
            // gcMatnrB3Des
            // 
            this.gcMatnrB3Des.Caption = "二三级品物料描述";
            this.gcMatnrB3Des.FieldName = "B3DESC";
            this.gcMatnrB3Des.MinWidth = 229;
            this.gcMatnrB3Des.Name = "gcMatnrB3Des";
            this.gcMatnrB3Des.Visible = true;
            this.gcMatnrB3Des.VisibleIndex = 5;
            this.gcMatnrB3Des.Width = 263;
            // 
            // gcPtype
            // 
            this.gcPtype.Caption = "组件类型";
            this.gcPtype.FieldName = "PTYP3";
            this.gcPtype.MinWidth = 46;
            this.gcPtype.Name = "gcPtype";
            this.gcPtype.Visible = true;
            this.gcPtype.VisibleIndex = 6;
            this.gcPtype.Width = 91;
            // 
            // gcWerks
            // 
            this.gcWerks.Caption = "工厂";
            this.gcWerks.FieldName = "WERKS";
            this.gcWerks.MinWidth = 34;
            this.gcWerks.Name = "gcWerks";
            this.gcWerks.Visible = true;
            this.gcWerks.VisibleIndex = 7;
            this.gcWerks.Width = 80;
            // 
            // gcCreater
            // 
            this.gcCreater.Caption = "创建人";
            this.gcCreater.FieldName = "CUSER";
            this.gcCreater.MinWidth = 34;
            this.gcCreater.Name = "gcCreater";
            this.gcCreater.Visible = true;
            this.gcCreater.VisibleIndex = 8;
            this.gcCreater.Width = 114;
            // 
            // gcCreatetime
            // 
            this.gcCreatetime.Caption = "创建时间";
            this.gcCreatetime.FieldName = "CDATE";
            this.gcCreatetime.MinWidth = 80;
            this.gcCreatetime.Name = "gcCreatetime";
            this.gcCreatetime.Visible = true;
            this.gcCreatetime.VisibleIndex = 9;
            this.gcCreatetime.Width = 137;
            // 
            // gcEditer
            // 
            this.gcEditer.Caption = "修改人";
            this.gcEditer.FieldName = "MUSER";
            this.gcEditer.MinWidth = 34;
            this.gcEditer.Name = "gcEditer";
            this.gcEditer.Visible = true;
            this.gcEditer.VisibleIndex = 10;
            this.gcEditer.Width = 114;
            // 
            // gcEditTime
            // 
            this.gcEditTime.Caption = "修改时间";
            this.gcEditTime.FieldName = "MDATE";
            this.gcEditTime.MinWidth = 80;
            this.gcEditTime.Name = "gcEditTime";
            this.gcEditTime.Visible = true;
            this.gcEditTime.VisibleIndex = 11;
            this.gcEditTime.Width = 137;
            // 
            // GCkEY
            // 
            this.GCkEY.Caption = "主键";
            this.GCkEY.FieldName = "BYP_KEY";
            this.GCkEY.MinWidth = 23;
            this.GCkEY.Name = "GCkEY";
            this.GCkEY.Width = 86;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.pgnQueryResult);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 270);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(899, 54);
            this.panelControl1.TabIndex = 14;
            // 
            // pgnQueryResult
            // 
            this.pgnQueryResult.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.pgnQueryResult.Appearance.Options.UseBackColor = true;
            this.pgnQueryResult.AutoSize = true;
            this.pgnQueryResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgnQueryResult.Location = new System.Drawing.Point(2, 2);
            this.pgnQueryResult.Margin = new System.Windows.Forms.Padding(0);
            this.pgnQueryResult.Name = "pgnQueryResult";
            this.pgnQueryResult.PageNo = 1;
            this.pgnQueryResult.Pages = 0;
            this.pgnQueryResult.PageSize = 200;
            this.pgnQueryResult.Records = 0;
            this.pgnQueryResult.Size = new System.Drawing.Size(895, 50);
            this.pgnQueryResult.TabIndex = 65;
            // 
            // grpCrtlCode
            // 
            this.grpCrtlCode.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.grpCrtlCode.Controls.Add(this.layoutControl1);
            this.grpCrtlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCrtlCode.Location = new System.Drawing.Point(3, 4);
            this.grpCrtlCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpCrtlCode.Name = "grpCrtlCode";
            this.grpCrtlCode.ShowCaption = false;
            this.grpCrtlCode.Size = new System.Drawing.Size(903, 103);
            this.grpCrtlCode.TabIndex = 2;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtMoudleType);
            this.layoutControl1.Controls.Add(this.txtMtnrB3);
            this.layoutControl1.Controls.Add(this.txtMtnrB2);
            this.layoutControl1.Controls.Add(this.txtMtnrm);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(903, 103);
            this.layoutControl1.TabIndex = 10;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtMoudleType
            // 
            this.txtMoudleType.Location = new System.Drawing.Point(561, 36);
            this.txtMoudleType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMoudleType.Name = "txtMoudleType";
            this.txtMoudleType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtMoudleType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "CODE"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "NAME")});
            this.txtMoudleType.Properties.NullText = "";
            this.txtMoudleType.Size = new System.Drawing.Size(334, 24);
            this.txtMoudleType.StyleController = this.layoutControl1;
            this.txtMoudleType.TabIndex = 1;
            this.txtMoudleType.TabStop = false;
            // 
            // txtMtnrB3
            // 
            this.txtMtnrB3.Location = new System.Drawing.Point(116, 36);
            this.txtMtnrB3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMtnrB3.Name = "txtMtnrB3";
            this.txtMtnrB3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMtnrB3.Size = new System.Drawing.Size(333, 24);
            this.txtMtnrB3.StyleController = this.layoutControl1;
            this.txtMtnrB3.TabIndex = 5;
            this.txtMtnrB3.TabStop = false;
            this.txtMtnrB3.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtMtnrB3_ButtonClick);
            // 
            // txtMtnrB2
            // 
            this.txtMtnrB2.Location = new System.Drawing.Point(561, 8);
            this.txtMtnrB2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMtnrB2.Name = "txtMtnrB2";
            this.txtMtnrB2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMtnrB2.Size = new System.Drawing.Size(334, 24);
            this.txtMtnrB2.StyleController = this.layoutControl1;
            this.txtMtnrB2.TabIndex = 4;
            this.txtMtnrB2.TabStop = false;
            this.txtMtnrB2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtMtnrB2_ButtonClick);
            // 
            // txtMtnrm
            // 
            this.txtMtnrm.Location = new System.Drawing.Point(116, 8);
            this.txtMtnrm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMtnrm.Name = "txtMtnrm";
            this.txtMtnrm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMtnrm.Size = new System.Drawing.Size(333, 24);
            this.txtMtnrm.StyleController = this.layoutControl1;
            this.txtMtnrm.TabIndex = 0;
            this.txtMtnrm.TabStop = false;
            this.txtMtnrm.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtMtnrm_ButtonClick);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblMtnrm,
            this.lblMtnrB3,
            this.lblMtnrB2,
            this.lblMoudleType});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);
            this.layoutControlGroup1.Size = new System.Drawing.Size(903, 103);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lblMtnrm
            // 
            this.lblMtnrm.Control = this.txtMtnrm;
            this.lblMtnrm.CustomizationFormText = "layoutControlItem1";
            this.lblMtnrm.Location = new System.Drawing.Point(0, 0);
            this.lblMtnrm.Name = "lblMtnrm";
            this.lblMtnrm.Size = new System.Drawing.Size(445, 28);
            this.lblMtnrm.Text = "主料料号";
            this.lblMtnrm.TextSize = new System.Drawing.Size(105, 18);
            // 
            // lblMtnrB3
            // 
            this.lblMtnrB3.Control = this.txtMtnrB3;
            this.lblMtnrB3.CustomizationFormText = "二三级品物料号";
            this.lblMtnrB3.Location = new System.Drawing.Point(0, 28);
            this.lblMtnrB3.Name = "lblMtnrB3";
            this.lblMtnrB3.Size = new System.Drawing.Size(445, 63);
            this.lblMtnrB3.Text = "二三级品物料号";
            this.lblMtnrB3.TextSize = new System.Drawing.Size(105, 18);
            // 
            // lblMtnrB2
            // 
            this.lblMtnrB2.Control = this.txtMtnrB2;
            this.lblMtnrB2.CustomizationFormText = "低效物料号";
            this.lblMtnrB2.Location = new System.Drawing.Point(445, 0);
            this.lblMtnrB2.Name = "lblMtnrB2";
            this.lblMtnrB2.Size = new System.Drawing.Size(446, 28);
            this.lblMtnrB2.Text = "低效物料号";
            this.lblMtnrB2.TextSize = new System.Drawing.Size(105, 18);
            // 
            // lblMoudleType
            // 
            this.lblMoudleType.Control = this.txtMoudleType;
            this.lblMoudleType.CustomizationFormText = "描述：";
            this.lblMoudleType.Location = new System.Drawing.Point(445, 28);
            this.lblMoudleType.Name = "lblMoudleType";
            this.lblMoudleType.Size = new System.Drawing.Size(446, 63);
            this.lblMoudleType.Text = "组件类型";
            this.lblMoudleType.TextSize = new System.Drawing.Size(105, 18);
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.tsbDel);
            this.panelControl3.Controls.Add(this.tsbSave);
            this.panelControl3.Controls.Add(this.tsbNew);
            this.panelControl3.Controls.Add(this.tsbSeach);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(3, 114);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(903, 44);
            this.panelControl3.TabIndex = 14;
            // 
            // tsbSeach
            // 
            this.tsbSeach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tsbSeach.Location = new System.Drawing.Point(489, 7);
            this.tsbSeach.Name = "tsbSeach";
            this.tsbSeach.Size = new System.Drawing.Size(94, 30);
            this.tsbSeach.TabIndex = 11;
            this.tsbSeach.Text = "查询";
            // 
            // tsbNew
            // 
            this.tsbNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tsbNew.Location = new System.Drawing.Point(592, 7);
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(94, 30);
            this.tsbNew.TabIndex = 12;
            this.tsbNew.Text = "新增";
            // 
            // tsbSave
            // 
            this.tsbSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tsbSave.Location = new System.Drawing.Point(695, 7);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(94, 30);
            this.tsbSave.TabIndex = 13;
            this.tsbSave.Text = "保存";
            // 
            // tsbDel
            // 
            this.tsbDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tsbDel.Location = new System.Drawing.Point(798, 7);
            this.tsbDel.Name = "tsbDel";
            this.tsbDel.Size = new System.Drawing.Size(94, 30);
            this.tsbDel.TabIndex = 14;
            this.tsbDel.Text = "删除";
            // 
            // ByProductCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ByProductCtrl";
            this.Size = new System.Drawing.Size(911, 555);
            this.Load += new System.EventHandler(this.ByProductCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grcContainerIfo)).EndInit();
            this.grcContainerIfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCrtlCode)).EndInit();
            this.grpCrtlCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMoudleType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrB3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrB2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMtnrm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrB3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMtnrB2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMoudleType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.GroupControl grpCrtlCode;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lblMtnrm;
        private DevExpress.XtraLayout.LayoutControlItem lblMoudleType;
        private System.Windows.Forms.TableLayoutPanel Content;
        private DevExpress.XtraLayout.LayoutControlItem lblMtnrB2;
        private DevExpress.XtraEditors.GroupControl grcContainerIfo;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gvList;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private FanHai.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraEditors.LookUpEdit txtMoudleType;
        private DevExpress.XtraEditors.ButtonEdit txtMtnrB3;
        private DevExpress.XtraEditors.ButtonEdit txtMtnrB2;
        private DevExpress.XtraEditors.ButtonEdit txtMtnrm;
        private DevExpress.XtraLayout.LayoutControlItem lblMtnrB3;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnr_M;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnr_B2;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnr_B3;
        private DevExpress.XtraGrid.Columns.GridColumn gcPtype;
        private DevExpress.XtraGrid.Columns.GridColumn gcWerks;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreater;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreatetime;
        private DevExpress.XtraGrid.Columns.GridColumn gcEditer;
        private DevExpress.XtraGrid.Columns.GridColumn gcEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnrMDes;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnrB2Des;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatnrB3Des;
        private DevExpress.XtraGrid.Columns.GridColumn GCkEY;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton tsbSeach;
        private DevExpress.XtraEditors.SimpleButton tsbDel;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraEditors.SimpleButton tsbNew;
    }
}

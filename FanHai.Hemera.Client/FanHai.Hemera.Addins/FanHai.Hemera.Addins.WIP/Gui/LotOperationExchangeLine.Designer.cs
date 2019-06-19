namespace FanHai.Hemera.Addins.WIP
{
    partial class LotOperationExchangeLine
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
            this.gcExchangeLine = new DevExpress.XtraGrid.GridControl();
            this.gvExchangeLine = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolRowNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolLotNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolLotLineCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolWorkorderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolProID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEnterpriseName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolRouteName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolStepName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.lueFactoryRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.lueLotLine = new DevExpress.XtraEditors.LookUpEdit();
            this.teLotNumber = new DevExpress.XtraEditors.TextEdit();
            this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.teRemark = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExchangeLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExchangeLine)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLotLine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(811, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(626, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-01-16 15:04:56";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // gcExchangeLine
            // 
            this.gcExchangeLine.Location = new System.Drawing.Point(4, 4);
            this.gcExchangeLine.LookAndFeel.SkinName = "Coffee";
            this.gcExchangeLine.MainView = this.gvExchangeLine;
            this.gcExchangeLine.Name = "gcExchangeLine";
            this.gcExchangeLine.Size = new System.Drawing.Size(469, 565);
            this.gcExchangeLine.TabIndex = 1;
            this.gcExchangeLine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExchangeLine});
            // 
            // gvExchangeLine
            // 
            this.gvExchangeLine.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gvExchangeLine.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvExchangeLine.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gvExchangeLine.Appearance.Row.Options.UseFont = true;
            this.gvExchangeLine.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolRowNum,
            this.gcolLotNumber,
            this.gcolLotLineCode,
            this.gcolWorkorderNo,
            this.gcolProID,
            this.gcolEnterpriseName,
            this.gcolRouteName,
            this.gcolStepName,
            this.gcolQuantity,
            this.gcolEditor,
            this.gcolEditTime});
            this.gvExchangeLine.GridControl = this.gcExchangeLine;
            this.gvExchangeLine.Name = "gvExchangeLine";
            this.gvExchangeLine.OptionsBehavior.ReadOnly = true;
            this.gvExchangeLine.OptionsCustomization.AllowSort = false;
            this.gvExchangeLine.OptionsDetail.EnableDetailToolTip = true;
            this.gvExchangeLine.OptionsDetail.ShowDetailTabs = false;
            this.gvExchangeLine.OptionsView.ShowChildrenInGroupPanel = true;
            this.gvExchangeLine.OptionsView.ShowGroupPanel = false;
            this.gvExchangeLine.OptionsView.ShowIndicator = false;
            this.gvExchangeLine.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvExchangeLine_CustomDrawRowIndicator);
            // 
            // gcolRowNum
            // 
            this.gcolRowNum.Caption = "序号";
            this.gcolRowNum.Name = "gcolRowNum";
            this.gcolRowNum.OptionsColumn.AllowEdit = false;
            this.gcolRowNum.OptionsColumn.FixedWidth = true;
            this.gcolRowNum.Visible = true;
            this.gcolRowNum.VisibleIndex = 0;
            this.gcolRowNum.Width = 70;
            // 
            // gcolLotNumber
            // 
            this.gcolLotNumber.Caption = "批次号";
            this.gcolLotNumber.FieldName = "LOT_NUMBER";
            this.gcolLotNumber.Name = "gcolLotNumber";
            this.gcolLotNumber.Visible = true;
            this.gcolLotNumber.VisibleIndex = 1;
            this.gcolLotNumber.Width = 90;
            // 
            // gcolLotLineCode
            // 
            this.gcolLotLineCode.Caption = "主线名称";
            this.gcolLotLineCode.FieldName = "LOT_LINE_CODE";
            this.gcolLotLineCode.Name = "gcolLotLineCode";
            this.gcolLotLineCode.Visible = true;
            this.gcolLotLineCode.VisibleIndex = 2;
            this.gcolLotLineCode.Width = 76;
            // 
            // gcolWorkorderNo
            // 
            this.gcolWorkorderNo.Caption = "工单号";
            this.gcolWorkorderNo.FieldName = "WORK_ORDER_NO";
            this.gcolWorkorderNo.Name = "gcolWorkorderNo";
            this.gcolWorkorderNo.Visible = true;
            this.gcolWorkorderNo.VisibleIndex = 3;
            this.gcolWorkorderNo.Width = 56;
            // 
            // gcolProID
            // 
            this.gcolProID.Caption = "产品ID";
            this.gcolProID.FieldName = "PRO_ID";
            this.gcolProID.Name = "gcolProID";
            this.gcolProID.Visible = true;
            this.gcolProID.VisibleIndex = 4;
            this.gcolProID.Width = 56;
            // 
            // gcolEnterpriseName
            // 
            this.gcolEnterpriseName.Caption = "工艺流程组";
            this.gcolEnterpriseName.FieldName = "ENTERPRISE_NAME";
            this.gcolEnterpriseName.Name = "gcolEnterpriseName";
            this.gcolEnterpriseName.Visible = true;
            this.gcolEnterpriseName.VisibleIndex = 5;
            this.gcolEnterpriseName.Width = 81;
            // 
            // gcolRouteName
            // 
            this.gcolRouteName.Caption = "工艺流程";
            this.gcolRouteName.FieldName = "ROUTE_NAME";
            this.gcolRouteName.Name = "gcolRouteName";
            this.gcolRouteName.Visible = true;
            this.gcolRouteName.VisibleIndex = 6;
            this.gcolRouteName.Width = 36;
            // 
            // gcolStepName
            // 
            this.gcolStepName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcolStepName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcolStepName.Caption = "当前工序";
            this.gcolStepName.FieldName = "ROUTE_STEP_NAME";
            this.gcolStepName.Name = "gcolStepName";
            this.gcolStepName.OptionsColumn.AllowEdit = false;
            this.gcolStepName.Visible = true;
            this.gcolStepName.VisibleIndex = 7;
            this.gcolStepName.Width = 72;
            // 
            // gcolQuantity
            // 
            this.gcolQuantity.Caption = "电池片数量";
            this.gcolQuantity.FieldName = "QUANTITY";
            this.gcolQuantity.Name = "gcolQuantity";
            this.gcolQuantity.Visible = true;
            this.gcolQuantity.VisibleIndex = 8;
            this.gcolQuantity.Width = 51;
            // 
            // gcolEditor
            // 
            this.gcolEditor.AppearanceHeader.Options.UseTextOptions = true;
            this.gcolEditor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcolEditor.Caption = "操作者";
            this.gcolEditor.FieldName = "EDITOR";
            this.gcolEditor.Name = "gcolEditor";
            this.gcolEditor.OptionsColumn.AllowEdit = false;
            this.gcolEditor.Visible = true;
            this.gcolEditor.VisibleIndex = 9;
            this.gcolEditor.Width = 72;
            // 
            // gcolEditTime
            // 
            this.gcolEditTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gcolEditTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcolEditTime.Caption = "操作时间";
            this.gcolEditTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolEditTime.FieldName = "EDIT_TIME";
            this.gcolEditTime.Name = "gcolEditTime";
            this.gcolEditTime.OptionsColumn.AllowEdit = false;
            this.gcolEditTime.Visible = true;
            this.gcolEditTime.VisibleIndex = 10;
            this.gcolEditTime.Width = 73;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerControl1, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(811, 603);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 3);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(805, 597);
            this.splitContainerControl1.SplitterPosition = 322;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.btnSave);
            this.layoutControl2.Controls.Add(this.lueFactoryRoom);
            this.layoutControl2.Controls.Add(this.lueLotLine);
            this.layoutControl2.Controls.Add(this.teLotNumber);
            this.layoutControl2.Controls.Add(this.btnRemove);
            this.layoutControl2.Controls.Add(this.btnAdd);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(0, 0);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(861, 191, 650, 400);
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(322, 597);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(4, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 26);
            this.btnSave.StyleController = this.layoutControl2;
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lueFactoryRoom
            // 
            this.lueFactoryRoom.Location = new System.Drawing.Point(60, 30);
            this.lueFactoryRoom.Name = "lueFactoryRoom";
            this.lueFactoryRoom.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.Appearance.Options.UseFont = true;
            this.lueFactoryRoom.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.AppearanceDisabled.Options.UseFont = true;
            this.lueFactoryRoom.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lueFactoryRoom.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lueFactoryRoom.Properties.AppearanceFocused.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.AppearanceFocused.Options.UseFont = true;
            this.lueFactoryRoom.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueFactoryRoom.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.lueFactoryRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactoryRoom.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", " ")});
            this.lueFactoryRoom.Properties.NullText = "";
            this.lueFactoryRoom.Size = new System.Drawing.Size(253, 26);
            this.lueFactoryRoom.StyleController = this.layoutControl2;
            this.lueFactoryRoom.TabIndex = 17;
            // 
            // lueLotLine
            // 
            this.lueLotLine.Location = new System.Drawing.Point(60, 90);
            this.lueLotLine.Name = "lueLotLine";
            this.lueLotLine.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueLotLine.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PRODUCTION_LINE_KEY", "线别主键", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LINE_CODE", "线别代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LINE_NAME", "线别名称")});
            this.lueLotLine.Properties.NullText = "";
            this.lueLotLine.Size = new System.Drawing.Size(253, 20);
            this.lueLotLine.StyleController = this.layoutControl2;
            this.lueLotLine.TabIndex = 21;
            // 
            // teLotNumber
            // 
            this.teLotNumber.Location = new System.Drawing.Point(60, 60);
            this.teLotNumber.Name = "teLotNumber";
            this.teLotNumber.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.teLotNumber.Size = new System.Drawing.Size(99, 20);
            this.teLotNumber.StyleController = this.layoutControl2;
            this.teLotNumber.TabIndex = 3;
            this.teLotNumber.TabStop = false;
            this.teLotNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.teLotNumber_KeyPress);
            // 
            // btnRemove
            // 
            this.btnRemove.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnRemove.Appearance.Options.UseFont = true;
            this.btnRemove.Location = new System.Drawing.Point(240, 60);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(73, 26);
            this.btnRemove.StyleController = this.layoutControl2;
            this.btnRemove.TabIndex = 19;
            this.btnRemove.Text = "移除";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnAdd.Appearance.Options.UseFont = true;
            this.btnAdd.Location = new System.Drawing.Point(163, 60);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(73, 26);
            this.btnAdd.StyleController = this.layoutControl2;
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlGroup3});
            this.layoutControlGroup2.Name = "Root";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(322, 597);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnSave;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 121);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(81, 30);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(81, 30);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(318, 472);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "保存";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup3.Size = new System.Drawing.Size(318, 121);
            this.layoutControlGroup3.Text = "批次信息";
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lueFactoryRoom;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(0, 30);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(106, 30);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(308, 30);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Text = "车间名称";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.teLotNumber;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(154, 30);
            this.layoutControlItem5.Text = "批次号";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnAdd;
            this.layoutControlItem6.Location = new System.Drawing.Point(154, 30);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(77, 30);
            this.layoutControlItem6.Text = "新增";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnRemove;
            this.layoutControlItem7.Location = new System.Drawing.Point(231, 30);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(77, 30);
            this.layoutControlItem7.Text = "移除";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lueLotLine;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(0, 30);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(106, 30);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(308, 30);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "批次线别";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gcExchangeLine);
            this.layoutControl1.Controls.Add(this.teRemark);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(908, 202, 650, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(477, 597);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // teRemark
            // 
            this.teRemark.Location = new System.Drawing.Point(31, 573);
            this.teRemark.Name = "teRemark";
            this.teRemark.Properties.MaxLength = 200;
            this.teRemark.Size = new System.Drawing.Size(442, 20);
            this.teRemark.StyleController = this.layoutControl1;
            this.teRemark.TabIndex = 20;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(477, 597);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup1.Text = "批次明细";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gcExchangeLine;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(473, 569);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.teRemark;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 569);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(473, 24);
            this.layoutControlItem3.Text = "备注";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(24, 14);
            // 
            // LotOperationExchangeLine
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "LotOperationExchangeLine";
            this.Size = new System.Drawing.Size(813, 650);
            this.Load += new System.EventHandler(this.LotOperationExchangeLine_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExchangeLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExchangeLine)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLotLine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraGrid.GridControl gcExchangeLine;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExchangeLine;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditor;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolStepName;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryRoom;
        private DevExpress.XtraGrid.Columns.GridColumn gcolLotNumber;
        private DevExpress.XtraEditors.SimpleButton btnRemove;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.TextEdit teLotNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gcolRowNum;
        private DevExpress.XtraEditors.TextEdit teRemark;
        private DevExpress.XtraEditors.LookUpEdit lueLotLine;
        private DevExpress.XtraGrid.Columns.GridColumn gcolLotLineCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcolWorkorderNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcolProID;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEnterpriseName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolRouteName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolQuantity;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}

namespace FanHai.Hemera.Addins.WIP
{
    partial class LotUndoCtrl
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
            this.gcUndo = new DevExpress.XtraGrid.GridControl();
            this.gvUndo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolRowNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolLotNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolStepName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolActivity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.teLotNumber = new DevExpress.XtraEditors.TextEdit();
            this.lueFactoryRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgTop = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciLotNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFactoryRoom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAdd = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciRemove = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.ContentMain = new DevExpress.XtraLayout.LayoutControl();
            this.teRemark = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgResult = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResults = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciRemark = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcUndo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUndo)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContentMain)).BeginInit();
            this.ContentMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRemark)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(812, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(627, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 17:12:05";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // gcUndo
            // 
            this.gcUndo.Location = new System.Drawing.Point(7, 7);
            this.gcUndo.LookAndFeel.SkinName = "Coffee";
            this.gcUndo.MainView = this.gvUndo;
            this.gcUndo.Name = "gcUndo";
            this.gcUndo.Size = new System.Drawing.Size(412, 519);
            this.gcUndo.TabIndex = 1;
            this.gcUndo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUndo});
            // 
            // gvUndo
            // 
            this.gvUndo.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gvUndo.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvUndo.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gvUndo.Appearance.Row.Options.UseFont = true;
            this.gvUndo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolRowNum,
            this.gcolLotNumber,
            this.gcolStepName,
            this.gcolActivity,
            this.gcolEditor,
            this.gcolEditTime});
            this.gvUndo.GridControl = this.gcUndo;
            this.gvUndo.Name = "gvUndo";
            this.gvUndo.OptionsBehavior.ReadOnly = true;
            this.gvUndo.OptionsCustomization.AllowSort = false;
            this.gvUndo.OptionsDetail.EnableDetailToolTip = true;
            this.gvUndo.OptionsDetail.ShowDetailTabs = false;
            this.gvUndo.OptionsView.ShowChildrenInGroupPanel = true;
            this.gvUndo.OptionsView.ShowGroupPanel = false;
            this.gvUndo.OptionsView.ShowIndicator = false;
            this.gvUndo.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvUndo_CustomDrawRowIndicator);
            this.gvUndo.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.UndoGridView_CustomDrawCell);
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
            this.gcolLotNumber.Width = 146;
            // 
            // gcolStepName
            // 
            this.gcolStepName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcolStepName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcolStepName.Caption = "工序";
            this.gcolStepName.FieldName = "STEP_NAME";
            this.gcolStepName.Name = "gcolStepName";
            this.gcolStepName.OptionsColumn.AllowEdit = false;
            this.gcolStepName.Visible = true;
            this.gcolStepName.VisibleIndex = 2;
            this.gcolStepName.Width = 146;
            // 
            // gcolActivity
            // 
            this.gcolActivity.AppearanceHeader.Options.UseTextOptions = true;
            this.gcolActivity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcolActivity.Caption = "操作";
            this.gcolActivity.FieldName = "ACTIVITY";
            this.gcolActivity.Name = "gcolActivity";
            this.gcolActivity.OptionsColumn.AllowEdit = false;
            this.gcolActivity.Visible = true;
            this.gcolActivity.VisibleIndex = 3;
            this.gcolActivity.Width = 146;
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
            this.gcolEditor.VisibleIndex = 4;
            this.gcolEditor.Width = 146;
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
            this.gcolEditTime.VisibleIndex = 5;
            this.gcolEditTime.Width = 151;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.AutoScroll = true;
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerControl1, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 47);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 1;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 569F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(812, 569);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 3);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.ContentMain);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(806, 563);
            this.splitContainerControl1.SplitterPosition = 374;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.teLotNumber);
            this.layoutControl1.Controls.Add(this.lueFactoryRoom);
            this.layoutControl1.Controls.Add(this.btnAdd);
            this.layoutControl1.Controls.Add(this.btnSave);
            this.layoutControl1.Controls.Add(this.btnRemove);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(653, 339, 650, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(374, 563);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // teLotNumber
            // 
            this.teLotNumber.Location = new System.Drawing.Point(77, 61);
            this.teLotNumber.Name = "teLotNumber";
            this.teLotNumber.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.teLotNumber.Size = new System.Drawing.Size(287, 20);
            this.teLotNumber.StyleController = this.layoutControl1;
            this.teLotNumber.TabIndex = 3;
            this.teLotNumber.TabStop = false;
            this.teLotNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.teLotNumber_KeyPress);
            // 
            // lueFactoryRoom
            // 
            this.lueFactoryRoom.Location = new System.Drawing.Point(77, 31);
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
            this.lueFactoryRoom.Size = new System.Drawing.Size(287, 26);
            this.lueFactoryRoom.StyleController = this.layoutControl1;
            this.lueFactoryRoom.TabIndex = 17;
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnAdd.Appearance.Options.UseFont = true;
            this.btnAdd.Location = new System.Drawing.Point(94, 85);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 26);
            this.btnAdd.StyleController = this.layoutControl1;
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(254, 85);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 26);
            this.btnSave.StyleController = this.layoutControl1;
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "确定撤销";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnRemove.Appearance.Options.UseFont = true;
            this.btnRemove.Location = new System.Drawing.Point(174, 85);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(76, 26);
            this.btnRemove.StyleController = this.layoutControl1;
            this.btnRemove.TabIndex = 19;
            this.btnRemove.Text = "移除";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgTop});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(374, 563);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lcgTop
            // 
            this.lcgTop.CustomizationFormText = " ";
            this.lcgTop.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciLotNumber,
            this.lciFactoryRoom,
            this.lciAdd,
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.lciRemove,
            this.emptySpaceItem2});
            this.lcgTop.Location = new System.Drawing.Point(0, 0);
            this.lcgTop.Name = "lcgTop";
            this.lcgTop.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgTop.Size = new System.Drawing.Size(364, 553);
            this.lcgTop.Text = " ";
            // 
            // lciLotNumber
            // 
            this.lciLotNumber.Control = this.teLotNumber;
            this.lciLotNumber.CustomizationFormText = "批次号";
            this.lciLotNumber.Location = new System.Drawing.Point(0, 30);
            this.lciLotNumber.Name = "lciLotNumber";
            this.lciLotNumber.Size = new System.Drawing.Size(358, 24);
            this.lciLotNumber.Text = "批次号";
            this.lciLotNumber.TextSize = new System.Drawing.Size(64, 19);
            // 
            // lciFactoryRoom
            // 
            this.lciFactoryRoom.Control = this.lueFactoryRoom;
            this.lciFactoryRoom.CustomizationFormText = "车间名称";
            this.lciFactoryRoom.Location = new System.Drawing.Point(0, 0);
            this.lciFactoryRoom.Name = "lciFactoryRoom";
            this.lciFactoryRoom.Size = new System.Drawing.Size(358, 30);
            this.lciFactoryRoom.Text = "车间名称";
            this.lciFactoryRoom.TextSize = new System.Drawing.Size(64, 19);
            // 
            // lciAdd
            // 
            this.lciAdd.Control = this.btnAdd;
            this.lciAdd.CustomizationFormText = "新增";
            this.lciAdd.Location = new System.Drawing.Point(84, 54);
            this.lciAdd.MaxSize = new System.Drawing.Size(80, 30);
            this.lciAdd.MinSize = new System.Drawing.Size(80, 30);
            this.lciAdd.Name = "lciAdd";
            this.lciAdd.Size = new System.Drawing.Size(80, 30);
            this.lciAdd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciAdd.Text = "新增";
            this.lciAdd.TextSize = new System.Drawing.Size(0, 0);
            this.lciAdd.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnSave;
            this.layoutControlItem1.CustomizationFormText = "保存";
            this.layoutControlItem1.Location = new System.Drawing.Point(244, 54);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(114, 30);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(114, 30);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(114, 30);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "保存";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 54);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(84, 30);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciRemove
            // 
            this.lciRemove.Control = this.btnRemove;
            this.lciRemove.CustomizationFormText = "移除";
            this.lciRemove.Location = new System.Drawing.Point(164, 54);
            this.lciRemove.MaxSize = new System.Drawing.Size(80, 30);
            this.lciRemove.MinSize = new System.Drawing.Size(80, 30);
            this.lciRemove.Name = "lciRemove";
            this.lciRemove.Size = new System.Drawing.Size(80, 30);
            this.lciRemove.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciRemove.Text = "移除";
            this.lciRemove.TextSize = new System.Drawing.Size(0, 0);
            this.lciRemove.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 84);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(358, 442);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ContentMain
            // 
            this.ContentMain.Appearance.Control.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ContentMain.Appearance.Control.Options.UseFont = true;
            this.ContentMain.Controls.Add(this.teRemark);
            this.ContentMain.Controls.Add(this.gcUndo);
            this.ContentMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentMain.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ContentMain.Location = new System.Drawing.Point(0, 0);
            this.ContentMain.Margin = new System.Windows.Forms.Padding(0);
            this.ContentMain.Name = "ContentMain";
            this.ContentMain.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1055, 342, 650, 400);
            this.ContentMain.Root = this.lcgRoot;
            this.ContentMain.Size = new System.Drawing.Size(426, 563);
            this.ContentMain.TabIndex = 4;
            this.ContentMain.Text = "layoutControl1";
            // 
            // teRemark
            // 
            this.teRemark.Location = new System.Drawing.Point(42, 530);
            this.teRemark.Name = "teRemark";
            this.teRemark.Properties.MaxLength = 200;
            this.teRemark.Size = new System.Drawing.Size(377, 26);
            this.teRemark.StyleController = this.ContentMain;
            this.teRemark.TabIndex = 20;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = " ";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgResult});
            this.lcgRoot.Name = "Root";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.lcgRoot.Size = new System.Drawing.Size(426, 563);
            this.lcgRoot.Text = " ";
            // 
            // lcgResult
            // 
            this.lcgResult.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lcgResult.AppearanceItemCaption.Options.UseFont = true;
            this.lcgResult.CustomizationFormText = "列表";
            this.lcgResult.GroupBordersVisible = false;
            this.lcgResult.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciResults,
            this.lciRemark});
            this.lcgResult.Location = new System.Drawing.Point(0, 0);
            this.lcgResult.Name = "lcgResult";
            this.lcgResult.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgResult.Size = new System.Drawing.Size(416, 553);
            this.lcgResult.Text = "列表";
            // 
            // lciResults
            // 
            this.lciResults.Control = this.gcUndo;
            this.lciResults.CustomizationFormText = " ";
            this.lciResults.Location = new System.Drawing.Point(0, 0);
            this.lciResults.Name = "lciResults";
            this.lciResults.Size = new System.Drawing.Size(416, 523);
            this.lciResults.Text = " ";
            this.lciResults.TextSize = new System.Drawing.Size(0, 0);
            this.lciResults.TextVisible = false;
            // 
            // lciRemark
            // 
            this.lciRemark.Control = this.teRemark;
            this.lciRemark.CustomizationFormText = "备注";
            this.lciRemark.Location = new System.Drawing.Point(0, 523);
            this.lciRemark.Name = "lciRemark";
            this.lciRemark.Size = new System.Drawing.Size(416, 30);
            this.lciRemark.Text = "备注";
            this.lciRemark.TextSize = new System.Drawing.Size(32, 19);
            // 
            // LotUndoCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "LotUndoCtrl";
            this.Size = new System.Drawing.Size(814, 616);
            this.Load += new System.EventHandler(this.LotUndoCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcUndo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUndo)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContentMain)).EndInit();
            this.ContentMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciRemark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraGrid.GridControl gcUndo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvUndo;
        private DevExpress.XtraGrid.Columns.GridColumn gcolActivity;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditor;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolStepName;
        private DevExpress.XtraLayout.LayoutControl ContentMain;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciResults;
        private DevExpress.XtraLayout.LayoutControlGroup lcgResult;
        private DevExpress.XtraGrid.Columns.GridColumn gcolLotNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gcolRowNum;
        private DevExpress.XtraEditors.TextEdit teRemark;
        private DevExpress.XtraLayout.LayoutControlItem lciRemark;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit teLotNumber;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryRoom;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnRemove;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTop;
        private DevExpress.XtraLayout.LayoutControlItem lciLotNumber;
        private DevExpress.XtraLayout.LayoutControlItem lciFactoryRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem lciRemove;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}

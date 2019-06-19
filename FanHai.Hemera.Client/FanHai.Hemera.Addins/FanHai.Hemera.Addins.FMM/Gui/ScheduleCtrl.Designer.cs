namespace FanHai.Hemera.Addins.FMM
{
    partial class ScheduleCtrl
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.tsbNew = new DevExpress.XtraEditors.SimpleButton();
            this.tsbCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.tsbDelete = new DevExpress.XtraEditors.SimpleButton();
            this.tsbOpen = new DevExpress.XtraEditors.SimpleButton();
            this.txtSchedule = new DevExpress.XtraEditors.TextEdit();
            this.txtOverTime = new DevExpress.XtraEditors.TextEdit();
            this.txtDesc = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.labelControl1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblScheduleName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ShiftControl = new DevExpress.XtraGrid.GridControl();
            this.ShiftView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.shift_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.shift_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.start_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.end_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.over_day = new DevExpress.XtraGrid.Columns.GridColumn();
            this.checkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.planNameGC = new DevExpress.XtraGrid.GridControl();
            this.planNameGrid = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.schedule_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.schedule_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.descriptions = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MAXOVERLAPTIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.editor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EDIT_TIMEZONE = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSchedule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblScheduleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.planNameGC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.planNameGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(838, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(653, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 14:27:37";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(256, 23);
            this.lblMenu.Text = "平台管理>车间配置>生产排班";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(1, 47);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(838, 536);
            this.panelControl2.TabIndex = 9;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.ShiftControl);
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Controls.Add(this.planNameGC);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(834, 532);
            this.splitContainerControl1.SplitterPosition = 328;
            this.splitContainerControl1.TabIndex = 12;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.tsbNew);
            this.layoutControl1.Controls.Add(this.tsbCancel);
            this.layoutControl1.Controls.Add(this.tsbSave);
            this.layoutControl1.Controls.Add(this.tsbDelete);
            this.layoutControl1.Controls.Add(this.tsbOpen);
            this.layoutControl1.Controls.Add(this.txtSchedule);
            this.layoutControl1.Controls.Add(this.txtOverTime);
            this.layoutControl1.Controls.Add(this.txtDesc);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3047, 6, 812, 500);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(328, 532);
            this.layoutControl1.TabIndex = 12;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // tsbNew
            // 
            this.tsbNew.Location = new System.Drawing.Point(166, 441);
            this.tsbNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(144, 22);
            this.tsbNew.StyleController = this.layoutControl1;
            this.tsbNew.TabIndex = 17;
            this.tsbNew.Text = "新增";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Location = new System.Drawing.Point(7, 503);
            this.tsbCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(314, 22);
            this.tsbCancel.StyleController = this.layoutControl1;
            this.tsbCancel.TabIndex = 16;
            this.tsbCancel.Text = "取消";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Location = new System.Drawing.Point(18, 467);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(144, 22);
            this.tsbSave.StyleController = this.layoutControl1;
            this.tsbSave.TabIndex = 15;
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Location = new System.Drawing.Point(166, 467);
            this.tsbDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(144, 22);
            this.tsbDelete.StyleController = this.layoutControl1;
            this.tsbDelete.TabIndex = 14;
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbOpen
            // 
            this.tsbOpen.Location = new System.Drawing.Point(18, 441);
            this.tsbOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(144, 22);
            this.tsbOpen.StyleController = this.layoutControl1;
            this.tsbOpen.TabIndex = 13;
            this.tsbOpen.Text = "查询";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // txtSchedule
            // 
            this.txtSchedule.EditValue = "";
            this.txtSchedule.EnterMoveNextControl = true;
            this.txtSchedule.Location = new System.Drawing.Point(103, 62);
            this.txtSchedule.Name = "txtSchedule";
            this.txtSchedule.Properties.MaxLength = 50;
            this.txtSchedule.Size = new System.Drawing.Size(207, 20);
            this.txtSchedule.StyleController = this.layoutControl1;
            this.txtSchedule.TabIndex = 7;
            // 
            // txtOverTime
            // 
            this.txtOverTime.EditValue = "";
            this.txtOverTime.EnterMoveNextControl = true;
            this.txtOverTime.Location = new System.Drawing.Point(103, 38);
            this.txtOverTime.Name = "txtOverTime";
            this.txtOverTime.Properties.MaxLength = 50;
            this.txtOverTime.Size = new System.Drawing.Size(207, 20);
            this.txtOverTime.StyleController = this.layoutControl1;
            this.txtOverTime.TabIndex = 11;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(103, 86);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(207, 292);
            this.txtDesc.StyleController = this.layoutControl1;
            this.txtDesc.TabIndex = 9;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlItem5});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(328, 532);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.labelControl1,
            this.lblScheduleName,
            this.lblDescription,
            this.emptySpaceItem1,
            this.layoutControlItem4,
            this.layoutControlItem6,
            this.layoutControlItem3,
            this.layoutControlItem2});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(318, 496);
            this.layoutControlGroup2.Text = "基本资料";
            // 
            // labelControl1
            // 
            this.labelControl1.Control = this.txtOverTime;
            this.labelControl1.CustomizationFormText = "layoutControlItem3";
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.OptionsTableLayoutItem.ColumnIndex = 1;
            this.labelControl1.Size = new System.Drawing.Size(296, 24);
            this.labelControl1.Text = "最大跨度(分钟)";
            this.labelControl1.TextSize = new System.Drawing.Size(82, 14);
            // 
            // lblScheduleName
            // 
            this.lblScheduleName.Control = this.txtSchedule;
            this.lblScheduleName.CustomizationFormText = "layoutControlItem1";
            this.lblScheduleName.Location = new System.Drawing.Point(0, 24);
            this.lblScheduleName.Name = "lblScheduleName";
            this.lblScheduleName.Size = new System.Drawing.Size(296, 24);
            this.lblScheduleName.Text = "计划名称";
            this.lblScheduleName.TextSize = new System.Drawing.Size(82, 14);
            // 
            // lblDescription
            // 
            this.lblDescription.Control = this.txtDesc;
            this.lblDescription.CustomizationFormText = "layoutControlItem2";
            this.lblDescription.Location = new System.Drawing.Point(0, 48);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.OptionsTableLayoutItem.RowIndex = 1;
            this.lblDescription.Size = new System.Drawing.Size(296, 296);
            this.lblDescription.Text = "描述";
            this.lblDescription.TextSize = new System.Drawing.Size(82, 14);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 344);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(296, 59);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.tsbSave;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 429);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(148, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.tsbNew;
            this.layoutControlItem6.Location = new System.Drawing.Point(148, 403);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(148, 26);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tsbDelete;
            this.layoutControlItem3.Location = new System.Drawing.Point(148, 429);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(148, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tsbOpen;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 403);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(148, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.tsbCancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 496);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(318, 26);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // ShiftControl
            // 
            this.ShiftControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShiftControl.Location = new System.Drawing.Point(0, 0);
            this.ShiftControl.LookAndFeel.SkinName = "Coffee";
            this.ShiftControl.MainView = this.ShiftView;
            this.ShiftControl.Name = "ShiftControl";
            this.ShiftControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.checkEdit});
            this.ShiftControl.Size = new System.Drawing.Size(500, 275);
            this.ShiftControl.TabIndex = 7;
            this.ShiftControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ShiftView,
            this.gridView1});
            // 
            // ShiftView
            // 
            this.ShiftView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.shift_key,
            this.shift_name,
            this.start_time,
            this.end_time,
            this.over_day});
            this.ShiftView.GridControl = this.ShiftControl;
            this.ShiftView.Name = "ShiftView";
            this.ShiftView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.ShiftView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.ShiftView.OptionsBehavior.Editable = false;
            this.ShiftView.OptionsView.ShowGroupPanel = false;
            this.ShiftView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.ShiftView_CustomDrawRowIndicator);
            // 
            // shift_key
            // 
            this.shift_key.Caption = "gridColumn2";
            this.shift_key.FieldName = "SHIFT_KEY";
            this.shift_key.Name = "shift_key";
            // 
            // shift_name
            // 
            this.shift_name.AppearanceHeader.Options.UseTextOptions = true;
            this.shift_name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.shift_name.Caption = "名称";
            this.shift_name.FieldName = "SHIFT_NAME";
            this.shift_name.Name = "shift_name";
            this.shift_name.Visible = true;
            this.shift_name.VisibleIndex = 0;
            // 
            // start_time
            // 
            this.start_time.AppearanceHeader.Options.UseTextOptions = true;
            this.start_time.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.start_time.Caption = "开始时间";
            this.start_time.FieldName = "START_TIME";
            this.start_time.Name = "start_time";
            this.start_time.Visible = true;
            this.start_time.VisibleIndex = 1;
            // 
            // end_time
            // 
            this.end_time.AppearanceHeader.Options.UseTextOptions = true;
            this.end_time.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.end_time.Caption = "结束时间";
            this.end_time.FieldName = "END_TIME";
            this.end_time.Name = "end_time";
            this.end_time.Visible = true;
            this.end_time.VisibleIndex = 2;
            // 
            // over_day
            // 
            this.over_day.AppearanceHeader.Options.UseTextOptions = true;
            this.over_day.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.over_day.Caption = "是否跨天";
            this.over_day.ColumnEdit = this.checkEdit;
            this.over_day.FieldName = "OVER_DAY";
            this.over_day.Name = "over_day";
            this.over_day.OptionsColumn.AllowEdit = false;
            this.over_day.Visible = true;
            this.over_day.VisibleIndex = 3;
            // 
            // checkEdit
            // 
            this.checkEdit.AutoHeight = false;
            this.checkEdit.Name = "checkEdit";
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 272;
            this.gridView1.GridControl = this.ShiftControl;
            this.gridView1.Name = "gridView1";
            // 
            // groupControl1
            // 
            this.groupControl1.AutoSize = true;
            this.groupControl1.Controls.Add(this.btnAdd);
            this.groupControl1.Controls.Add(this.btnEdit);
            this.groupControl1.Controls.Add(this.btnDel);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 275);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(500, 35);
            this.groupControl1.TabIndex = 10;
            this.groupControl1.Text = "groupControl1";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(208, 5);
            this.btnAdd.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(302, 5);
            this.btnEdit.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(87, 23);
            this.btnEdit.TabIndex = 13;
            this.btnEdit.Text = "编辑";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Location = new System.Drawing.Point(397, 5);
            this.btnDel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(87, 23);
            this.btnDel.TabIndex = 12;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // planNameGC
            // 
            this.planNameGC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.planNameGC.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.planNameGC.Location = new System.Drawing.Point(0, 310);
            this.planNameGC.MainView = this.planNameGrid;
            this.planNameGC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.planNameGC.Name = "planNameGC";
            this.planNameGC.Size = new System.Drawing.Size(500, 222);
            this.planNameGC.TabIndex = 11;
            this.planNameGC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.planNameGrid});
            // 
            // planNameGrid
            // 
            this.planNameGrid.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.schedule_key,
            this.schedule_name,
            this.descriptions,
            this.MAXOVERLAPTIME,
            this.editor,
            this.EDIT_TIMEZONE});
            this.planNameGrid.DetailHeight = 272;
            this.planNameGrid.GridControl = this.planNameGC;
            this.planNameGrid.Name = "planNameGrid";
            this.planNameGrid.DoubleClick += new System.EventHandler(this.planNameGrid_DoubleClick);
            // 
            // schedule_key
            // 
            this.schedule_key.Caption = "主键";
            this.schedule_key.FieldName = "SCHEDULE_KEY";
            this.schedule_key.MinWidth = 17;
            this.schedule_key.Name = "schedule_key";
            this.schedule_key.Width = 66;
            // 
            // schedule_name
            // 
            this.schedule_name.Caption = "计划名称";
            this.schedule_name.FieldName = "SCHEDULE_NAME";
            this.schedule_name.MinWidth = 17;
            this.schedule_name.Name = "schedule_name";
            this.schedule_name.OptionsColumn.AllowEdit = false;
            this.schedule_name.Visible = true;
            this.schedule_name.VisibleIndex = 0;
            this.schedule_name.Width = 66;
            // 
            // descriptions
            // 
            this.descriptions.Caption = "描述";
            this.descriptions.FieldName = "DESCRIPTIONS";
            this.descriptions.MinWidth = 17;
            this.descriptions.Name = "descriptions";
            this.descriptions.Visible = true;
            this.descriptions.VisibleIndex = 1;
            this.descriptions.Width = 66;
            // 
            // MAXOVERLAPTIME
            // 
            this.MAXOVERLAPTIME.Caption = "最大延迟(分)";
            this.MAXOVERLAPTIME.FieldName = "MAXOVERLAPTIME";
            this.MAXOVERLAPTIME.MinWidth = 17;
            this.MAXOVERLAPTIME.Name = "MAXOVERLAPTIME";
            this.MAXOVERLAPTIME.Width = 66;
            // 
            // editor
            // 
            this.editor.Caption = "修改者";
            this.editor.FieldName = "EDITOR";
            this.editor.MinWidth = 17;
            this.editor.Name = "editor";
            this.editor.Width = 66;
            // 
            // EDIT_TIMEZONE
            // 
            this.EDIT_TIMEZONE.Caption = "时区";
            this.EDIT_TIMEZONE.FieldName = "EDIT_TIMEZONE";
            this.EDIT_TIMEZONE.MinWidth = 17;
            this.EDIT_TIMEZONE.Name = "EDIT_TIMEZONE";
            this.EDIT_TIMEZONE.Width = 66;
            // 
            // ScheduleCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScheduleCtrl";
            this.Size = new System.Drawing.Size(840, 583);
            this.Load += new System.EventHandler(this.ScheduleCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSchedule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOverTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblScheduleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShiftView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.planNameGC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.planNameGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl ShiftControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ShiftView;
        private DevExpress.XtraGrid.Columns.GridColumn shift_key;
        private DevExpress.XtraGrid.Columns.GridColumn shift_name;
        private DevExpress.XtraGrid.Columns.GridColumn start_time;
        private DevExpress.XtraGrid.Columns.GridColumn end_time;
        private DevExpress.XtraGrid.Columns.GridColumn over_day;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit checkEdit;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtSchedule;
        private DevExpress.XtraEditors.TextEdit txtOverTime;
        private DevExpress.XtraEditors.MemoEdit txtDesc;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lblScheduleName;
        private DevExpress.XtraLayout.LayoutControlItem labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem lblDescription;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraGrid.GridControl planNameGC;
        private DevExpress.XtraGrid.Views.Grid.GridView planNameGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn schedule_key;
        private DevExpress.XtraGrid.Columns.GridColumn schedule_name;
        private DevExpress.XtraGrid.Columns.GridColumn descriptions;
        private DevExpress.XtraGrid.Columns.GridColumn MAXOVERLAPTIME;
        private DevExpress.XtraGrid.Columns.GridColumn editor;
        private DevExpress.XtraGrid.Columns.GridColumn EDIT_TIMEZONE;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.SimpleButton tsbNew;
        private DevExpress.XtraEditors.SimpleButton tsbCancel;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraEditors.SimpleButton tsbDelete;
        private DevExpress.XtraEditors.SimpleButton tsbOpen;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
    }
}

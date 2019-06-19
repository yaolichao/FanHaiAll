namespace FanHai.Hemera.Addins.EMS
{
    partial class EquipmentCheckItemsCtrl
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
            this.lcgForm = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgCheckItemInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciCheckItemName = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtCheckItemName = new DevExpress.XtraEditors.TextEdit();
            this.lcForm = new DevExpress.XtraLayout.LayoutControl();
            this.paginationCheckItems = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtQueryValue = new DevExpress.XtraEditors.TextEdit();
            this.grdCheckItemList = new DevExpress.XtraGrid.GridControl();
            this.grvCheckItemList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.cmbCheckItemType = new DevExpress.XtraEditors.LookUpEdit();
            this.lciCheckItemType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgCheckItemList = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciCheckItemList = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPaginationCheckItems = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgCheckItemQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciQueryLabel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCheckItemName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcForm)).BeginInit();
            this.lcForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCheckItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvCheckItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCheckItemType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPaginationCheckItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            this.tsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(856, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(636, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-03-18 14:56:10";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // lcgForm
            // 
            this.lcgForm.CustomizationFormText = " ";
            this.lcgForm.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgCheckItemInfo,
            this.lcgCheckItemList,
            this.lcgCheckItemQuery});
            this.lcgForm.Name = "Root";
            this.lcgForm.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgForm.Size = new System.Drawing.Size(850, 607);
            this.lcgForm.Spacing = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.lcgForm.Text = " ";
            // 
            // lcgCheckItemInfo
            // 
            this.lcgCheckItemInfo.CustomizationFormText = "设备检查项信息";
            this.lcgCheckItemInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciCheckItemName,
            this.lciCheckItemType,
            this.lciDescription});
            this.lcgCheckItemInfo.Location = new System.Drawing.Point(0, 73);
            this.lcgCheckItemInfo.Name = "lcgCheckItemInfo";
            this.lcgCheckItemInfo.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignModeGroup.AlignLocal;
            this.lcgCheckItemInfo.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgCheckItemInfo.Size = new System.Drawing.Size(844, 100);
            this.lcgCheckItemInfo.Text = "设备检查项信息";
            // 
            // lciCheckItemName
            // 
            this.lciCheckItemName.Control = this.txtCheckItemName;
            this.lciCheckItemName.CustomizationFormText = "检查项名称";
            this.lciCheckItemName.Location = new System.Drawing.Point(0, 0);
            this.lciCheckItemName.Name = "lciCheckItemName";
            this.lciCheckItemName.Size = new System.Drawing.Size(419, 30);
            this.lciCheckItemName.Text = "检查项名称";
            this.lciCheckItemName.TextSize = new System.Drawing.Size(75, 18);
            // 
            // txtCheckItemName
            // 
            this.txtCheckItemName.EditValue = "";
            this.txtCheckItemName.EnterMoveNextControl = true;
            this.txtCheckItemName.Location = new System.Drawing.Point(88, 138);
            this.txtCheckItemName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCheckItemName.Name = "txtCheckItemName";
            this.txtCheckItemName.Properties.MaxLength = 50;
            this.txtCheckItemName.Size = new System.Drawing.Size(337, 24);
            this.txtCheckItemName.StyleController = this.lcForm;
            this.txtCheckItemName.TabIndex = 3;
            // 
            // lcForm
            // 
            this.lcForm.Controls.Add(this.paginationCheckItems);
            this.lcForm.Controls.Add(this.btnQuery);
            this.lcForm.Controls.Add(this.txtQueryValue);
            this.lcForm.Controls.Add(this.grdCheckItemList);
            this.lcForm.Controls.Add(this.txtDescription);
            this.lcForm.Controls.Add(this.txtCheckItemName);
            this.lcForm.Controls.Add(this.cmbCheckItemType);
            this.lcForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcForm.Location = new System.Drawing.Point(3, 95);
            this.lcForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcForm.Name = "lcForm";
            this.lcForm.Root = this.lcgForm;
            this.lcForm.Size = new System.Drawing.Size(850, 607);
            this.lcForm.TabIndex = 0;
            // 
            // paginationCheckItems
            // 
            this.paginationCheckItems.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.paginationCheckItems.Appearance.Options.UseBackColor = true;
            this.paginationCheckItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paginationCheckItems.Location = new System.Drawing.Point(10, 549);
            this.paginationCheckItems.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.paginationCheckItems.LookAndFeel.UseDefaultLookAndFeel = false;
            this.paginationCheckItems.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.paginationCheckItems.Name = "paginationCheckItems";
            this.paginationCheckItems.PageNo = 0;
            this.paginationCheckItems.Pages = 0;
            this.paginationCheckItems.PageSize = 20;
            this.paginationCheckItems.Records = 0;
            this.paginationCheckItems.Size = new System.Drawing.Size(830, 45);
            this.paginationCheckItems.TabIndex = 7;
            this.paginationCheckItems.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.paginationCheckItems_DataPaging);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(730, 65);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(110, 27);
            this.btnQuery.StyleController = this.lcForm;
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtQueryValue
            // 
            this.txtQueryValue.EditValue = "";
            this.txtQueryValue.EnterMoveNextControl = true;
            this.txtQueryValue.Location = new System.Drawing.Point(88, 65);
            this.txtQueryValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtQueryValue.Name = "txtQueryValue";
            this.txtQueryValue.Properties.MaxLength = 50;
            this.txtQueryValue.Size = new System.Drawing.Size(332, 24);
            this.txtQueryValue.StyleController = this.lcForm;
            this.txtQueryValue.TabIndex = 1;
            // 
            // grdCheckItemList
            // 
            this.grdCheckItemList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grdCheckItemList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdCheckItemList.Location = new System.Drawing.Point(10, 238);
            this.grdCheckItemList.MainView = this.grvCheckItemList;
            this.grdCheckItemList.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.grdCheckItemList.Name = "grdCheckItemList";
            this.grdCheckItemList.Size = new System.Drawing.Size(830, 305);
            this.grdCheckItemList.TabIndex = 6;
            this.grdCheckItemList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvCheckItemList});
            this.grdCheckItemList.DoubleClick += new System.EventHandler(this.grdCheckItemList_DoubleClick);
            // 
            // grvCheckItemList
            // 
            this.grvCheckItemList.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Red;
            this.grvCheckItemList.Appearance.FooterPanel.Options.UseForeColor = true;
            this.grvCheckItemList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.grvCheckItemList.DetailHeight = 450;
            this.grvCheckItemList.FixedLineWidth = 3;
            this.grvCheckItemList.GridControl = this.grdCheckItemList;
            this.grvCheckItemList.Name = "grvCheckItemList";
            this.grvCheckItemList.OptionsBehavior.Editable = false;
            this.grvCheckItemList.OptionsBehavior.ReadOnly = true;
            this.grvCheckItemList.OptionsCustomization.AllowFilter = false;
            this.grvCheckItemList.OptionsCustomization.AllowGroup = false;
            this.grvCheckItemList.OptionsMenu.EnableFooterMenu = false;
            this.grvCheckItemList.OptionsView.ColumnAutoWidth = false;
            this.grvCheckItemList.OptionsView.ShowGroupPanel = false;
            this.grvCheckItemList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.grvCheckItemList_FocusedRowChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "检查项名称";
            this.gridColumn1.MinWidth = 23;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 86;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "检查项类型";
            this.gridColumn2.MinWidth = 23;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 86;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "描述";
            this.gridColumn3.MinWidth = 23;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 86;
            // 
            // txtDescription
            // 
            this.txtDescription.EnterMoveNextControl = true;
            this.txtDescription.Location = new System.Drawing.Point(88, 168);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Properties.MaxLength = 150;
            this.txtDescription.Size = new System.Drawing.Size(752, 24);
            this.txtDescription.StyleController = this.lcForm;
            this.txtDescription.TabIndex = 5;
            // 
            // cmbCheckItemType
            // 
            this.cmbCheckItemType.Location = new System.Drawing.Point(507, 138);
            this.cmbCheckItemType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCheckItemType.Name = "cmbCheckItemType";
            this.cmbCheckItemType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCheckItemType.Properties.MaxLength = 50;
            this.cmbCheckItemType.Properties.NullText = "";
            this.cmbCheckItemType.Size = new System.Drawing.Size(333, 24);
            this.cmbCheckItemType.StyleController = this.lcForm;
            this.cmbCheckItemType.TabIndex = 4;
            this.cmbCheckItemType.TabStop = false;
            // 
            // lciCheckItemType
            // 
            this.lciCheckItemType.Control = this.cmbCheckItemType;
            this.lciCheckItemType.CustomizationFormText = "检查项类型";
            this.lciCheckItemType.Location = new System.Drawing.Point(419, 0);
            this.lciCheckItemType.Name = "lciCheckItemType";
            this.lciCheckItemType.Size = new System.Drawing.Size(415, 30);
            this.lciCheckItemType.Text = "检查项类型";
            this.lciCheckItemType.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lciDescription
            // 
            this.lciDescription.Control = this.txtDescription;
            this.lciDescription.CustomizationFormText = "检查项描述";
            this.lciDescription.Location = new System.Drawing.Point(0, 30);
            this.lciDescription.Name = "lciDescription";
            this.lciDescription.Size = new System.Drawing.Size(834, 30);
            this.lciDescription.Text = "检查项描述";
            this.lciDescription.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lcgCheckItemList
            // 
            this.lcgCheckItemList.CustomizationFormText = "设备检查项列表";
            this.lcgCheckItemList.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciCheckItemList,
            this.lciPaginationCheckItems});
            this.lcgCheckItemList.Location = new System.Drawing.Point(0, 173);
            this.lcgCheckItemList.Name = "lcgCheckItemList";
            this.lcgCheckItemList.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgCheckItemList.Size = new System.Drawing.Size(844, 402);
            this.lcgCheckItemList.Text = "设备检查项列表";
            // 
            // lciCheckItemList
            // 
            this.lciCheckItemList.Control = this.grdCheckItemList;
            this.lciCheckItemList.CustomizationFormText = "检查项列表";
            this.lciCheckItemList.Location = new System.Drawing.Point(0, 0);
            this.lciCheckItemList.Name = "lciCheckItemList";
            this.lciCheckItemList.Size = new System.Drawing.Size(834, 311);
            this.lciCheckItemList.Text = "检查项列表";
            this.lciCheckItemList.TextSize = new System.Drawing.Size(0, 0);
            this.lciCheckItemList.TextVisible = false;
            // 
            // lciPaginationCheckItems
            // 
            this.lciPaginationCheckItems.Control = this.paginationCheckItems;
            this.lciPaginationCheckItems.CustomizationFormText = "检查项分页";
            this.lciPaginationCheckItems.Location = new System.Drawing.Point(0, 311);
            this.lciPaginationCheckItems.MaxSize = new System.Drawing.Size(0, 51);
            this.lciPaginationCheckItems.MinSize = new System.Drawing.Size(119, 51);
            this.lciPaginationCheckItems.Name = "lciPaginationCheckItems";
            this.lciPaginationCheckItems.Size = new System.Drawing.Size(834, 51);
            this.lciPaginationCheckItems.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPaginationCheckItems.Text = "检查项分页";
            this.lciPaginationCheckItems.TextSize = new System.Drawing.Size(0, 0);
            this.lciPaginationCheckItems.TextVisible = false;
            // 
            // lcgCheckItemQuery
            // 
            this.lcgCheckItemQuery.CustomizationFormText = "设备检查项查询";
            this.lcgCheckItemQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciQueryLabel,
            this.lciQuery,
            this.emptySpaceItem1});
            this.lcgCheckItemQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgCheckItemQuery.Name = "lcgCheckItemQuery";
            this.lcgCheckItemQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgCheckItemQuery.Size = new System.Drawing.Size(844, 73);
            this.lcgCheckItemQuery.Text = "设备检查项查询";
            // 
            // lciQueryLabel
            // 
            this.lciQueryLabel.Control = this.txtQueryValue;
            this.lciQueryLabel.CustomizationFormText = "检查项名称";
            this.lciQueryLabel.Location = new System.Drawing.Point(0, 0);
            this.lciQueryLabel.Name = "lciQueryLabel";
            this.lciQueryLabel.Size = new System.Drawing.Size(414, 33);
            this.lciQueryLabel.Text = "检查项名称";
            this.lciQueryLabel.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(720, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(114, 33);
            this.lciQuery.MinSize = new System.Drawing.Size(114, 33);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(114, 33);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(414, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(306, 33);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // tsMenu
            // 
            this.tsMenu.AutoSize = false;
            this.tsMenu.BackColor = System.Drawing.Color.Transparent;
            this.tsMenu.BackgroundImage = global::FanHai.Hemera.Addins.EMS.Properties.Resources.toolstrip_bk;
            this.tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.tsbNew,
            this.tsbSave,
            this.tsbCancel,
            this.tsbDelete});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(856, 33);
            this.tsMenu.TabIndex = 0;
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.arrow_refresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(88, 30);
            this.tsbRefresh.Text = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbNew
            // 
            this.tsbNew.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.document_add;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(66, 30);
            this.tsbNew.Text = "New";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.save_accept;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(67, 30);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbCancel
            // 
            this.tsbCancel.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.cancel;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(81, 30);
            this.tsbCancel.Text = "Cancel";
            this.tsbCancel.Click += new System.EventHandler(this.tsbCancel_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = global::FanHai.Hemera.Addins.EMS.Properties.Resources.document_delete;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(81, 30);
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(290, 32);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Equipment Check Items";
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(3, 37);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlTitle.Size = new System.Drawing.Size(850, 50);
            this.pnlTitle.TabIndex = 2;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tsMenu, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pnlTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.lcForm, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(856, 706);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // EquipmentCheckItemsCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "EquipmentCheckItemsCtrl";
            this.Size = new System.Drawing.Size(858, 706);
            this.Load += new System.EventHandler(this.EquipmentCheckItemsCtrl_Load);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCheckItemName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcForm)).EndInit();
            this.lcForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCheckItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvCheckItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCheckItemType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCheckItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPaginationCheckItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgCheckItemQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQueryLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControlGroup lcgForm;
        private DevExpress.XtraLayout.LayoutControl lcForm;
        private DevExpress.XtraLayout.LayoutControlGroup lcgCheckItemInfo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgCheckItemList;
        private DevExpress.XtraEditors.TextEdit txtCheckItemName;
        private DevExpress.XtraLayout.LayoutControlItem lciCheckItemName;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private DevExpress.XtraLayout.LayoutControlItem lciCheckItemType;
        private DevExpress.XtraLayout.LayoutControlItem lciDescription;
        private DevExpress.XtraGrid.GridControl grdCheckItemList;
        private DevExpress.XtraGrid.Views.Grid.GridView grvCheckItemList;
        private DevExpress.XtraLayout.LayoutControlItem lciCheckItemList;
        private DevExpress.XtraEditors.LookUpEdit cmbCheckItemType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbCancel;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlGroup lcgCheckItemQuery;
        private DevExpress.XtraEditors.TextEdit txtQueryValue;
        private DevExpress.XtraLayout.LayoutControlItem lciQueryLabel;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private FanHai.Hemera.Utils.Controls.PaginationControl paginationCheckItems;
        private DevExpress.XtraLayout.LayoutControlItem lciPaginationCheckItems;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;


    }
}

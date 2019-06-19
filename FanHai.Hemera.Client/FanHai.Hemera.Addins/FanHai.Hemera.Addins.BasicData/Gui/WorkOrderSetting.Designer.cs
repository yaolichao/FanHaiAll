namespace FanHai.Hemera.Addins.BasicData.Gui
{
    partial class WorkOrderSetting
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PARAMENTID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STEP_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PRODUCTCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WERKS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.memoWorkOrder = new DevExpress.XtraEditors.MemoEdit();
            this.lueWorkOrder = new DevExpress.XtraEditors.LookUpEdit();
            this.lueProductId = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd_wo_attr = new DevExpress.XtraEditors.SimpleButton();
            this.btnDel_wo_attr = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd_wo_material = new DevExpress.XtraEditors.SimpleButton();
            this.gcWoAttr = new DevExpress.XtraGrid.GridControl();
            this.gvWoAttr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ATTRIBUTE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATTRIBUTE_VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATTRIBUTE_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WORK_ORDER_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATTRIBUTE_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WORK_ORDER_ATTR_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.CODEDESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.CONTROLTYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoWorkOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueWorkOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueProductId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcWoAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWoAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(965, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(745, 0);
            this.lblInfos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-01-23 17:09:10";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // PARAMENTID
            // 
            this.PARAMENTID.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PARAMENTID.AppearanceHeader.Options.UseFont = true;
            this.PARAMENTID.Caption = "参数";
            this.PARAMENTID.FieldName = "PARAMENTID";
            this.PARAMENTID.MinWidth = 80;
            this.PARAMENTID.Name = "PARAMENTID";
            this.PARAMENTID.Visible = true;
            this.PARAMENTID.VisibleIndex = 4;
            this.PARAMENTID.Width = 80;
            // 
            // STEP_KEY
            // 
            this.STEP_KEY.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.STEP_KEY.AppearanceHeader.Options.UseFont = true;
            this.STEP_KEY.Caption = "工序";
            this.STEP_KEY.FieldName = "STEP_KEY";
            this.STEP_KEY.MinWidth = 80;
            this.STEP_KEY.Name = "STEP_KEY";
            this.STEP_KEY.Visible = true;
            this.STEP_KEY.VisibleIndex = 3;
            this.STEP_KEY.Width = 80;
            // 
            // PRODUCTCODE
            // 
            this.PRODUCTCODE.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PRODUCTCODE.AppearanceHeader.Options.UseFont = true;
            this.PRODUCTCODE.Caption = "成品类型";
            this.PRODUCTCODE.FieldName = "PRODUCTCODE";
            this.PRODUCTCODE.MinWidth = 100;
            this.PRODUCTCODE.Name = "PRODUCTCODE";
            this.PRODUCTCODE.Visible = true;
            this.PRODUCTCODE.VisibleIndex = 5;
            this.PRODUCTCODE.Width = 100;
            // 
            // WERKS
            // 
            this.WERKS.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WERKS.AppearanceHeader.Options.UseFont = true;
            this.WERKS.Caption = "车间";
            this.WERKS.FieldName = "WERKS";
            this.WERKS.MinWidth = 100;
            this.WERKS.Name = "WERKS";
            this.WERKS.Visible = true;
            this.WERKS.VisibleIndex = 2;
            this.WERKS.Width = 100;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.memoWorkOrder);
            this.layoutControl1.Controls.Add(this.lueWorkOrder);
            this.layoutControl1.Controls.Add(this.lueProductId);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(403, 626);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // memoWorkOrder
            // 
            this.memoWorkOrder.Location = new System.Drawing.Point(108, 170);
            this.memoWorkOrder.Margin = new System.Windows.Forms.Padding(4);
            this.memoWorkOrder.Name = "memoWorkOrder";
            this.memoWorkOrder.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoWorkOrder.Properties.Appearance.Options.UseFont = true;
            this.memoWorkOrder.Size = new System.Drawing.Size(271, 432);
            this.memoWorkOrder.StyleController = this.layoutControl1;
            this.memoWorkOrder.TabIndex = 8;
            // 
            // lueWorkOrder
            // 
            this.lueWorkOrder.Location = new System.Drawing.Point(119, 63);
            this.lueWorkOrder.Margin = new System.Windows.Forms.Padding(4);
            this.lueWorkOrder.Name = "lueWorkOrder";
            this.lueWorkOrder.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueWorkOrder.Properties.Appearance.Options.UseFont = true;
            this.lueWorkOrder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueWorkOrder.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ORDER_NUMBER", "工单号码")});
            this.lueWorkOrder.Properties.NullText = "";
            this.lueWorkOrder.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lueWorkOrder.Size = new System.Drawing.Size(249, 30);
            this.lueWorkOrder.StyleController = this.layoutControl1;
            this.lueWorkOrder.TabIndex = 6;
            // 
            // lueProductId
            // 
            this.lueProductId.Location = new System.Drawing.Point(119, 123);
            this.lueProductId.Margin = new System.Windows.Forms.Padding(4);
            this.lueProductId.Name = "lueProductId";
            this.lueProductId.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueProductId.Properties.Appearance.Options.UseFont = true;
            this.lueProductId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueProductId.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PRODUCT_CODE", "产品号")});
            this.lueProductId.Properties.NullText = "";
            this.lueProductId.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lueProductId.Size = new System.Drawing.Size(249, 30);
            this.lueProductId.StyleController = this.layoutControl1;
            this.lueProductId.TabIndex = 5;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(403, 626);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem4});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(383, 606);
            this.layoutControlGroup3.Text = "工单设置";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.lueWorkOrder;
            this.layoutControlItem3.CustomizationFormText = "工单号";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(13, 13, 15, 15);
            this.layoutControlItem3.Size = new System.Drawing.Size(359, 60);
            this.layoutControlItem3.Text = "工单号";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(81, 24);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.lueProductId;
            this.layoutControlItem2.CustomizationFormText = "产品ID号";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(13, 13, 15, 15);
            this.layoutControlItem2.Size = new System.Drawing.Size(359, 60);
            this.layoutControlItem2.Text = "产品ID号";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(81, 24);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.memoWorkOrder;
            this.layoutControlItem4.CustomizationFormText = "备注";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(359, 436);
            this.layoutControlItem4.Text = "备注";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(81, 24);
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.tableLayoutPanel1);
            this.layoutControl2.Controls.Add(this.gcWoAttr);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(0, 0);
            this.layoutControl2.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(555, 626);
            this.layoutControl2.TabIndex = 1;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdd_wo_attr, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDel_wo_attr, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdd_wo_material, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(21, 549);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(513, 56);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // btnAdd_wo_attr
            // 
            this.btnAdd_wo_attr.Location = new System.Drawing.Point(73, 4);
            this.btnAdd_wo_attr.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd_wo_attr.MaximumSize = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_attr.MinimumSize = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_attr.Name = "btnAdd_wo_attr";
            this.btnAdd_wo_attr.Size = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_attr.TabIndex = 1;
            this.btnAdd_wo_attr.Text = "添加工单属性";
            this.btnAdd_wo_attr.Click += new System.EventHandler(this.btnAdd_wo_attr_Click);
            // 
            // btnDel_wo_attr
            // 
            this.btnDel_wo_attr.Location = new System.Drawing.Point(366, 4);
            this.btnDel_wo_attr.Margin = new System.Windows.Forms.Padding(4);
            this.btnDel_wo_attr.MaximumSize = new System.Drawing.Size(133, 45);
            this.btnDel_wo_attr.MinimumSize = new System.Drawing.Size(133, 45);
            this.btnDel_wo_attr.Name = "btnDel_wo_attr";
            this.btnDel_wo_attr.Size = new System.Drawing.Size(133, 45);
            this.btnDel_wo_attr.TabIndex = 0;
            this.btnDel_wo_attr.Text = "删除属性";
            this.btnDel_wo_attr.Click += new System.EventHandler(this.btnDel_wo_attr_Click);
            // 
            // btnAdd_wo_material
            // 
            this.btnAdd_wo_material.Location = new System.Drawing.Point(221, 4);
            this.btnAdd_wo_material.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd_wo_material.MaximumSize = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_material.MinimumSize = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_material.Name = "btnAdd_wo_material";
            this.btnAdd_wo_material.Size = new System.Drawing.Size(133, 45);
            this.btnAdd_wo_material.TabIndex = 2;
            this.btnAdd_wo_material.Text = "添加物料属性";
            this.btnAdd_wo_material.Click += new System.EventHandler(this.btnAdd_wo_material_Click_1);
            // 
            // gcWoAttr
            // 
            this.gcWoAttr.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcWoAttr.Location = new System.Drawing.Point(21, 47);
            this.gcWoAttr.MainView = this.gvWoAttr;
            this.gcWoAttr.Margin = new System.Windows.Forms.Padding(4);
            this.gcWoAttr.Name = "gcWoAttr";
            this.gcWoAttr.Size = new System.Drawing.Size(513, 498);
            this.gcWoAttr.TabIndex = 4;
            this.gcWoAttr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWoAttr});
            // 
            // gvWoAttr
            // 
            this.gvWoAttr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ATTRIBUTE_NAME,
            this.ATTRIBUTE_VALUE,
            this.ATTRIBUTE_KEY,
            this.WORK_ORDER_KEY,
            this.ATTRIBUTE_TYPE,
            this.WORK_ORDER_ATTR_KEY});
            this.gvWoAttr.DetailHeight = 525;
            this.gvWoAttr.FixedLineWidth = 3;
            this.gvWoAttr.GridControl = this.gcWoAttr;
            this.gvWoAttr.Name = "gvWoAttr";
            this.gvWoAttr.OptionsView.ShowGroupPanel = false;
            this.gvWoAttr.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvWoAttr_FocusedRowChanged);
            this.gvWoAttr.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvWoAttr_CellValueChanging);
            // 
            // ATTRIBUTE_NAME
            // 
            this.ATTRIBUTE_NAME.Caption = "属性名称";
            this.ATTRIBUTE_NAME.FieldName = "ATTRIBUTE_NAME";
            this.ATTRIBUTE_NAME.MinWidth = 27;
            this.ATTRIBUTE_NAME.Name = "ATTRIBUTE_NAME";
            this.ATTRIBUTE_NAME.OptionsColumn.AllowEdit = false;
            this.ATTRIBUTE_NAME.OptionsColumn.ReadOnly = true;
            this.ATTRIBUTE_NAME.Visible = true;
            this.ATTRIBUTE_NAME.VisibleIndex = 0;
            this.ATTRIBUTE_NAME.Width = 100;
            // 
            // ATTRIBUTE_VALUE
            // 
            this.ATTRIBUTE_VALUE.Caption = "属性值/SapNo";
            this.ATTRIBUTE_VALUE.FieldName = "ATTRIBUTE_VALUE";
            this.ATTRIBUTE_VALUE.MinWidth = 27;
            this.ATTRIBUTE_VALUE.Name = "ATTRIBUTE_VALUE";
            this.ATTRIBUTE_VALUE.Visible = true;
            this.ATTRIBUTE_VALUE.VisibleIndex = 1;
            this.ATTRIBUTE_VALUE.Width = 100;
            // 
            // ATTRIBUTE_KEY
            // 
            this.ATTRIBUTE_KEY.Caption = "属性ID";
            this.ATTRIBUTE_KEY.FieldName = "ATTRIBUTE_KEY";
            this.ATTRIBUTE_KEY.MinWidth = 27;
            this.ATTRIBUTE_KEY.Name = "ATTRIBUTE_KEY";
            this.ATTRIBUTE_KEY.Width = 100;
            // 
            // WORK_ORDER_KEY
            // 
            this.WORK_ORDER_KEY.Caption = "工单ID";
            this.WORK_ORDER_KEY.FieldName = "WORK_ORDER_KEY";
            this.WORK_ORDER_KEY.MinWidth = 27;
            this.WORK_ORDER_KEY.Name = "WORK_ORDER_KEY";
            this.WORK_ORDER_KEY.Width = 100;
            // 
            // ATTRIBUTE_TYPE
            // 
            this.ATTRIBUTE_TYPE.Caption = "属性类别(0:工单属性;1:物料属性)";
            this.ATTRIBUTE_TYPE.FieldName = "ATTRIBUTE_TYPE";
            this.ATTRIBUTE_TYPE.MinWidth = 27;
            this.ATTRIBUTE_TYPE.Name = "ATTRIBUTE_TYPE";
            this.ATTRIBUTE_TYPE.OptionsColumn.ReadOnly = true;
            this.ATTRIBUTE_TYPE.Visible = true;
            this.ATTRIBUTE_TYPE.VisibleIndex = 2;
            this.ATTRIBUTE_TYPE.Width = 100;
            // 
            // WORK_ORDER_ATTR_KEY
            // 
            this.WORK_ORDER_ATTR_KEY.Caption = "属性值主键";
            this.WORK_ORDER_ATTR_KEY.FieldName = "WORK_ORDER_ATTR_KEY";
            this.WORK_ORDER_ATTR_KEY.MinWidth = 27;
            this.WORK_ORDER_ATTR_KEY.Name = "WORK_ORDER_ATTR_KEY";
            this.WORK_ORDER_ATTR_KEY.Width = 100;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup4});
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(7, 7, 7, 7);
            this.layoutControlGroup2.Size = new System.Drawing.Size(555, 626);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem5});
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Size = new System.Drawing.Size(541, 612);
            this.layoutControlGroup4.Text = "工单属性设置";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gcWoAttr;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(517, 502);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.tableLayoutPanel1;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 502);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(517, 60);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // CODEDESC
            // 
            this.CODEDESC.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CODEDESC.AppearanceHeader.Options.UseFont = true;
            this.CODEDESC.Caption = "计划描述";
            this.CODEDESC.FieldName = "CODEDESC";
            this.CODEDESC.MinWidth = 100;
            this.CODEDESC.Name = "CODEDESC";
            this.CODEDESC.Visible = true;
            this.CODEDESC.VisibleIndex = 1;
            this.CODEDESC.Width = 100;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlTitle.Controls.Add(this.btnDel);
            this.pnlTitle.Controls.Add(this.btnAdd);
            this.pnlTitle.Controls.Add(this.btnQuery);
            this.pnlTitle.Controls.Add(this.btnCancel);
            this.pnlTitle.Controls.Add(this.btnModify);
            this.pnlTitle.Controls.Add(this.btnSave);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTitle.Location = new System.Drawing.Point(1, 686);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.pnlTitle.Size = new System.Drawing.Size(965, 50);
            this.pnlTitle.TabIndex = 3;
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Location = new System.Drawing.Point(504, 11);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(94, 30);
            this.btnDel.TabIndex = 21;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(104, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 30);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(4, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(94, 30);
            this.btnQuery.TabIndex = 19;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(304, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 30);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Location = new System.Drawing.Point(204, 11);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(94, 30);
            this.btnModify.TabIndex = 17;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(404, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 30);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CONTROLTYPE
            // 
            this.CONTROLTYPE.AppearanceHeader.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CONTROLTYPE.AppearanceHeader.Options.UseFont = true;
            this.CONTROLTYPE.Caption = "控制图类型";
            this.CONTROLTYPE.FieldName = "CONTROLTYPE";
            this.CONTROLTYPE.MinWidth = 120;
            this.CONTROLTYPE.Name = "CONTROLTYPE";
            this.CONTROLTYPE.Visible = true;
            this.CONTROLTYPE.VisibleIndex = 6;
            this.CONTROLTYPE.Width = 120;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(1, 60);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.layoutControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(965, 626);
            this.splitContainerControl1.SplitterPosition = 403;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // WorkOrderSetting
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.pnlTitle);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "WorkOrderSetting";
            this.Size = new System.Drawing.Size(967, 736);
            this.Load += new System.EventHandler(this.WorkOrderSetting_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.pnlTitle, 0);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoWorkOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueWorkOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueProductId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcWoAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWoAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn PARAMENTID;
        private DevExpress.XtraGrid.Columns.GridColumn STEP_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn PRODUCTCODE;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS;
        private DevExpress.XtraGrid.Columns.GridColumn CODEDESC;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraGrid.Columns.GridColumn CONTROLTYPE;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.LookUpEdit lueWorkOrder;
        private DevExpress.XtraEditors.LookUpEdit lueProductId;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.MemoEdit memoWorkOrder;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraGrid.GridControl gcWoAttr;
        private DevExpress.XtraGrid.Views.Grid.GridView gvWoAttr;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnDel_wo_attr;
        private DevExpress.XtraEditors.SimpleButton btnAdd_wo_attr;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraGrid.Columns.GridColumn ATTRIBUTE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn ATTRIBUTE_VALUE;
        private DevExpress.XtraGrid.Columns.GridColumn ATTRIBUTE_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn WORK_ORDER_KEY;
        private DevExpress.XtraEditors.SimpleButton btnAdd_wo_material;
        private DevExpress.XtraGrid.Columns.GridColumn ATTRIBUTE_TYPE;
        private DevExpress.XtraGrid.Columns.GridColumn WORK_ORDER_ATTR_KEY;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
    }
}

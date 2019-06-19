namespace FanHai.Hemera.Addins.EAP
{
    partial class EDCQueryCtrl
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
            this.cbOperation = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.pgnQueryResult = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.gcResults = new DevExpress.XtraGrid.GridControl();
            this.gvResults = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEDCName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEDCStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolStepName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEquipmentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolLotNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolPartType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolCreator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.deEndTime = new DevExpress.XtraEditors.DateEdit();
            this.deStartTime = new DevExpress.XtraEditors.DateEdit();
            this.lueFactoryRoom = new DevExpress.XtraEditors.LookUpEdit();
            this.lueEDCItem = new DevExpress.XtraEditors.LookUpEdit();
            this.lueEquipment = new DevExpress.XtraEditors.LookUpEdit();
            this.txtLotNo = new DevExpress.XtraEditors.TextEdit();
            this.luePartType = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroupMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgQuery = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciFactoryRoom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblLotNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblOperation = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEDCItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEquipment = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPartType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEndTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCommand = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStartTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPagenation = new DevExpress.XtraLayout.LayoutControlItem();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbOperation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEDCItem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEquipment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.luePartType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEDCItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEquipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPagenation)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.topPanel.Size = new System.Drawing.Size(885, 60);
            this.topPanel.Visible = false;
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(665, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-03-10 16:04:55";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // cbOperation
            // 
            this.cbOperation.Location = new System.Drawing.Point(73, 63);
            this.cbOperation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbOperation.Name = "cbOperation";
            this.cbOperation.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOperation.Properties.Appearance.Options.UseFont = true;
            this.cbOperation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbOperation.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbOperation.Size = new System.Drawing.Size(804, 24);
            this.cbOperation.StyleController = this.Content;
            this.cbOperation.TabIndex = 31;
            this.cbOperation.SelectedIndexChanged += new System.EventHandler(this.cbOperation_SelectedIndexChanged);
            // 
            // Content
            // 
            this.Content.AllowCustomization = false;
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.gcResults);
            this.Content.Controls.Add(this.btnQuery);
            this.Content.Controls.Add(this.deEndTime);
            this.Content.Controls.Add(this.deStartTime);
            this.Content.Controls.Add(this.lueFactoryRoom);
            this.Content.Controls.Add(this.lueEDCItem);
            this.Content.Controls.Add(this.lueEquipment);
            this.Content.Controls.Add(this.cbOperation);
            this.Content.Controls.Add(this.txtLotNo);
            this.Content.Controls.Add(this.luePartType);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Margin = new System.Windows.Forms.Padding(0);
            this.Content.Name = "Content";
            this.Content.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(981, 213, 812, 500);
            this.Content.Root = this.layoutControlGroupMain;
            this.Content.Size = new System.Drawing.Size(885, 677);
            this.Content.TabIndex = 94;
            this.Content.Text = "layoutControl1";
            // 
            // pgnQueryResult
            // 
            this.pgnQueryResult.Location = new System.Drawing.Point(2, 642);
            this.pgnQueryResult.Margin = new System.Windows.Forms.Padding(0);
            this.pgnQueryResult.Name = "pgnQueryResult";
            this.pgnQueryResult.PageNo = 1;
            this.pgnQueryResult.Pages = 0;
            this.pgnQueryResult.PageSize = 200;
            this.pgnQueryResult.Records = 0;
            this.pgnQueryResult.Size = new System.Drawing.Size(881, 33);
            this.pgnQueryResult.TabIndex = 94;
            this.pgnQueryResult.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.pgnQueryResult_DataPaging);
            // 
            // gcResults
            // 
            this.gcResults.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcResults.Location = new System.Drawing.Point(2, 271);
            this.gcResults.MainView = this.gvResults;
            this.gcResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcResults.Name = "gcResults";
            this.gcResults.Size = new System.Drawing.Size(881, 367);
            this.gcResults.TabIndex = 98;
            this.gcResults.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResults,
            this.gridView2});
            // 
            // gvResults
            // 
            this.gvResults.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolIndex,
            this.gcolEDCName,
            this.gcolEDCStartTime,
            this.gcolStepName,
            this.gcolEquipmentName,
            this.gcolLotNo,
            this.gcolPartType,
            this.gcolCreator,
            this.gcolEditor,
            this.gcolEditTime,
            this.gcolRemark});
            this.gvResults.DetailHeight = 450;
            this.gvResults.FixedLineWidth = 3;
            this.gvResults.GridControl = this.gcResults;
            this.gvResults.Name = "gvResults";
            this.gvResults.OptionsView.ShowGroupPanel = false;
            this.gvResults.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvResults_CustomDrawCell);
            this.gvResults.DoubleClick += new System.EventHandler(this.gvResults_DoubleClick);
            // 
            // gcolIndex
            // 
            this.gcolIndex.Caption = "序号";
            this.gcolIndex.FieldName = "INDEX";
            this.gcolIndex.MinWidth = 23;
            this.gcolIndex.Name = "gcolIndex";
            this.gcolIndex.OptionsColumn.AllowEdit = false;
            this.gcolIndex.OptionsColumn.FixedWidth = true;
            this.gcolIndex.OptionsColumn.ReadOnly = true;
            this.gcolIndex.Visible = true;
            this.gcolIndex.VisibleIndex = 0;
            this.gcolIndex.Width = 86;
            // 
            // gcolEDCName
            // 
            this.gcolEDCName.Caption = "项目名称";
            this.gcolEDCName.FieldName = "EDC_NAME";
            this.gcolEDCName.MinWidth = 23;
            this.gcolEDCName.Name = "gcolEDCName";
            this.gcolEDCName.OptionsColumn.AllowEdit = false;
            this.gcolEDCName.OptionsColumn.ReadOnly = true;
            this.gcolEDCName.Visible = true;
            this.gcolEDCName.VisibleIndex = 1;
            this.gcolEDCName.Width = 86;
            // 
            // gcolEDCStartTime
            // 
            this.gcolEDCStartTime.Caption = "采集时间";
            this.gcolEDCStartTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolEDCStartTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolEDCStartTime.FieldName = "COL_START_TIME";
            this.gcolEDCStartTime.GroupFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolEDCStartTime.GroupFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolEDCStartTime.MinWidth = 23;
            this.gcolEDCStartTime.Name = "gcolEDCStartTime";
            this.gcolEDCStartTime.OptionsColumn.AllowEdit = false;
            this.gcolEDCStartTime.OptionsColumn.ReadOnly = true;
            this.gcolEDCStartTime.Visible = true;
            this.gcolEDCStartTime.VisibleIndex = 2;
            this.gcolEDCStartTime.Width = 86;
            // 
            // gcolStepName
            // 
            this.gcolStepName.Caption = "工序名称";
            this.gcolStepName.FieldName = "STEP_NAME";
            this.gcolStepName.MinWidth = 23;
            this.gcolStepName.Name = "gcolStepName";
            this.gcolStepName.OptionsColumn.AllowEdit = false;
            this.gcolStepName.OptionsColumn.ReadOnly = true;
            this.gcolStepName.Visible = true;
            this.gcolStepName.VisibleIndex = 3;
            this.gcolStepName.Width = 86;
            // 
            // gcolEquipmentName
            // 
            this.gcolEquipmentName.Caption = "设备名称";
            this.gcolEquipmentName.FieldName = "EQUIPMENT_NAME_CODE";
            this.gcolEquipmentName.MinWidth = 23;
            this.gcolEquipmentName.Name = "gcolEquipmentName";
            this.gcolEquipmentName.OptionsColumn.AllowEdit = false;
            this.gcolEquipmentName.OptionsColumn.ReadOnly = true;
            this.gcolEquipmentName.Visible = true;
            this.gcolEquipmentName.VisibleIndex = 4;
            this.gcolEquipmentName.Width = 86;
            // 
            // gcolLotNo
            // 
            this.gcolLotNo.Caption = "批次号";
            this.gcolLotNo.FieldName = "LOT_NUMBER";
            this.gcolLotNo.MinWidth = 23;
            this.gcolLotNo.Name = "gcolLotNo";
            this.gcolLotNo.OptionsColumn.ReadOnly = true;
            this.gcolLotNo.Visible = true;
            this.gcolLotNo.VisibleIndex = 5;
            this.gcolLotNo.Width = 86;
            // 
            // gcolPartType
            // 
            this.gcolPartType.Caption = "产品类型";
            this.gcolPartType.FieldName = "PART_TYPE";
            this.gcolPartType.MinWidth = 23;
            this.gcolPartType.Name = "gcolPartType";
            this.gcolPartType.OptionsColumn.AllowEdit = false;
            this.gcolPartType.OptionsColumn.ReadOnly = true;
            this.gcolPartType.Visible = true;
            this.gcolPartType.VisibleIndex = 6;
            this.gcolPartType.Width = 86;
            // 
            // gcolCreator
            // 
            this.gcolCreator.Caption = "采集人";
            this.gcolCreator.FieldName = "CREATOR";
            this.gcolCreator.MinWidth = 23;
            this.gcolCreator.Name = "gcolCreator";
            this.gcolCreator.OptionsColumn.AllowEdit = false;
            this.gcolCreator.OptionsColumn.ReadOnly = true;
            this.gcolCreator.Visible = true;
            this.gcolCreator.VisibleIndex = 7;
            this.gcolCreator.Width = 86;
            // 
            // gcolEditor
            // 
            this.gcolEditor.Caption = "最后编辑人";
            this.gcolEditor.FieldName = "EDITOR";
            this.gcolEditor.MinWidth = 23;
            this.gcolEditor.Name = "gcolEditor";
            this.gcolEditor.OptionsColumn.AllowEdit = false;
            this.gcolEditor.OptionsColumn.ReadOnly = true;
            this.gcolEditor.Visible = true;
            this.gcolEditor.VisibleIndex = 8;
            this.gcolEditor.Width = 86;
            // 
            // gcolEditTime
            // 
            this.gcolEditTime.Caption = "最后编辑时间";
            this.gcolEditTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolEditTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolEditTime.FieldName = "EDIT_TIME";
            this.gcolEditTime.GroupFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gcolEditTime.GroupFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcolEditTime.MinWidth = 23;
            this.gcolEditTime.Name = "gcolEditTime";
            this.gcolEditTime.OptionsColumn.AllowEdit = false;
            this.gcolEditTime.OptionsColumn.ReadOnly = true;
            this.gcolEditTime.Visible = true;
            this.gcolEditTime.VisibleIndex = 9;
            this.gcolEditTime.Width = 86;
            // 
            // gcolRemark
            // 
            this.gcolRemark.Caption = "备注";
            this.gcolRemark.MinWidth = 23;
            this.gcolRemark.Name = "gcolRemark";
            this.gcolRemark.OptionsColumn.ReadOnly = true;
            this.gcolRemark.Visible = true;
            this.gcolRemark.VisibleIndex = 10;
            this.gcolRemark.Width = 86;
            // 
            // gridView2
            // 
            this.gridView2.DetailHeight = 450;
            this.gridView2.FixedLineWidth = 3;
            this.gridView2.GridControl = this.gcResults;
            this.gridView2.Name = "gridView2";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(8, 231);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(99, 29);
            this.btnQuery.StyleController = this.Content;
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // deEndTime
            // 
            this.deEndTime.EditValue = null;
            this.deEndTime.Location = new System.Drawing.Point(71, 203);
            this.deEndTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deEndTime.Name = "deEndTime";
            this.deEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEndTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEndTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deEndTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deEndTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEndTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.deEndTime.Size = new System.Drawing.Size(369, 24);
            this.deEndTime.StyleController = this.Content;
            this.deEndTime.TabIndex = 97;
            // 
            // deStartTime
            // 
            this.deStartTime.EditValue = null;
            this.deStartTime.Location = new System.Drawing.Point(507, 203);
            this.deStartTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deStartTime.Name = "deStartTime";
            this.deStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStartTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStartTime.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deStartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartTime.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.deStartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStartTime.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            this.deStartTime.Size = new System.Drawing.Size(370, 24);
            this.deStartTime.StyleController = this.Content;
            this.deStartTime.TabIndex = 96;
            // 
            // lueFactoryRoom
            // 
            this.lueFactoryRoom.Location = new System.Drawing.Point(73, 35);
            this.lueFactoryRoom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueFactoryRoom.Name = "lueFactoryRoom";
            this.lueFactoryRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueFactoryRoom.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME", " ")});
            this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
            this.lueFactoryRoom.Properties.NullText = "";
            this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
            this.lueFactoryRoom.Size = new System.Drawing.Size(804, 24);
            this.lueFactoryRoom.StyleController = this.Content;
            this.lueFactoryRoom.TabIndex = 36;
            this.lueFactoryRoom.EditValueChanged += new System.EventHandler(this.lueFactoryRoom_EditValueChanged);
            // 
            // lueEDCItem
            // 
            this.lueEDCItem.Location = new System.Drawing.Point(73, 147);
            this.lueEDCItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueEDCItem.Name = "lueEDCItem";
            this.lueEDCItem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueEDCItem.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EQUIPMENT_NAME", "设备名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EDC_NAME", "项目名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("OPERATION_NAME", "工序名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EQUIPMENT_CODE", "设备代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("TOPRODUCT", "成品料号", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PART_TYPE", "成品类型"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ACTION_NAME", "采集类型"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SP_NAME", "抽检规则"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SP_DESCRIPTIONS", "规则描述"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EDC_KEY", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EQUIPMENT_KEY", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("SP_KEY", "", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.lueEDCItem.Properties.NullText = "";
            this.lueEDCItem.Size = new System.Drawing.Size(804, 24);
            this.lueEDCItem.StyleController = this.Content;
            this.lueEDCItem.TabIndex = 35;
            this.lueEDCItem.EditValueChanged += new System.EventHandler(this.lueEDCItem_EditValueChanged);
            // 
            // lueEquipment
            // 
            this.lueEquipment.Location = new System.Drawing.Point(73, 91);
            this.lueEquipment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueEquipment.Name = "lueEquipment";
            this.lueEquipment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueEquipment.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EQUIPMENT_NAME", " "),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LINE_KEY", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LINE_NAME", " ", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.lueEquipment.Properties.DisplayMember = "EQUIPMENT_NAME";
            this.lueEquipment.Properties.NullText = "";
            this.lueEquipment.Properties.ValueMember = "EQUIPMENT_KEY";
            this.lueEquipment.Size = new System.Drawing.Size(804, 24);
            this.lueEquipment.StyleController = this.Content;
            this.lueEquipment.TabIndex = 34;
            this.lueEquipment.EditValueChanged += new System.EventHandler(this.lueEquipment_EditValueChanged);
            // 
            // txtLotNo
            // 
            this.txtLotNo.Location = new System.Drawing.Point(73, 175);
            this.txtLotNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F);
            this.txtLotNo.Properties.Appearance.Options.UseFont = true;
            this.txtLotNo.Properties.MaxLength = 50;
            this.txtLotNo.Size = new System.Drawing.Size(804, 24);
            this.txtLotNo.StyleController = this.Content;
            this.txtLotNo.TabIndex = 3;
            // 
            // luePartType
            // 
            this.luePartType.Location = new System.Drawing.Point(73, 119);
            this.luePartType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.luePartType.Name = "luePartType";
            this.luePartType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luePartType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "名称")});
            this.luePartType.Properties.NullText = "";
            this.luePartType.Size = new System.Drawing.Size(804, 24);
            this.luePartType.StyleController = this.Content;
            this.luePartType.TabIndex = 95;
            this.luePartType.EditValueChanged += new System.EventHandler(this.txtPartType_EditValueChanged);
            // 
            // layoutControlGroupMain
            // 
            this.layoutControlGroupMain.CustomizationFormText = " ";
            this.layoutControlGroupMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupMain.GroupBordersVisible = false;
            this.layoutControlGroupMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgQuery,
            this.lciResult,
            this.lciPagenation});
            this.layoutControlGroupMain.Name = "Root";
            this.layoutControlGroupMain.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupMain.Size = new System.Drawing.Size(885, 677);
            this.layoutControlGroupMain.Text = "查询";
            this.layoutControlGroupMain.TextVisible = false;
            // 
            // lcgQuery
            // 
            this.lcgQuery.CustomizationFormText = "查询条件";
            this.lcgQuery.ExpandButtonVisible = true;
            this.lcgQuery.ExpandOnDoubleClick = true;
            this.lcgQuery.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciFactoryRoom,
            this.lblLotNo,
            this.lblOperation,
            this.lciEDCItem,
            this.lciEquipment,
            this.lciPartType,
            this.lciEndTime,
            this.lciCommand,
            this.lciStartTime});
            this.lcgQuery.Location = new System.Drawing.Point(0, 0);
            this.lcgQuery.Name = "lcgQuery";
            this.lcgQuery.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 4, 4);
            this.lcgQuery.Size = new System.Drawing.Size(885, 269);
            this.lcgQuery.Text = " ";
            // 
            // lciFactoryRoom
            // 
            this.lciFactoryRoom.Control = this.lueFactoryRoom;
            this.lciFactoryRoom.CustomizationFormText = "工厂车间";
            this.lciFactoryRoom.Location = new System.Drawing.Point(0, 0);
            this.lciFactoryRoom.Name = "lciFactoryRoom";
            this.lciFactoryRoom.Size = new System.Drawing.Size(873, 28);
            this.lciFactoryRoom.Text = "工厂车间";
            this.lciFactoryRoom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciFactoryRoom.TextSize = new System.Drawing.Size(60, 18);
            this.lciFactoryRoom.TextToControlDistance = 5;
            // 
            // lblLotNo
            // 
            this.lblLotNo.Control = this.txtLotNo;
            this.lblLotNo.CustomizationFormText = "批      号";
            this.lblLotNo.Location = new System.Drawing.Point(0, 140);
            this.lblLotNo.Name = "lblLotNo";
            this.lblLotNo.Size = new System.Drawing.Size(873, 28);
            this.lblLotNo.Text = "批      号";
            this.lblLotNo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lblLotNo.TextSize = new System.Drawing.Size(60, 18);
            this.lblLotNo.TextToControlDistance = 5;
            // 
            // lblOperation
            // 
            this.lblOperation.Control = this.cbOperation;
            this.lblOperation.CustomizationFormText = "工      序";
            this.lblOperation.Location = new System.Drawing.Point(0, 28);
            this.lblOperation.Name = "lblOperation";
            this.lblOperation.Size = new System.Drawing.Size(873, 28);
            this.lblOperation.Text = "工      序";
            this.lblOperation.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lblOperation.TextSize = new System.Drawing.Size(60, 18);
            this.lblOperation.TextToControlDistance = 5;
            // 
            // lciEDCItem
            // 
            this.lciEDCItem.Control = this.lueEDCItem;
            this.lciEDCItem.CustomizationFormText = "采集项目";
            this.lciEDCItem.Location = new System.Drawing.Point(0, 112);
            this.lciEDCItem.Name = "lciEDCItem";
            this.lciEDCItem.Size = new System.Drawing.Size(873, 28);
            this.lciEDCItem.Text = "采集项目";
            this.lciEDCItem.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciEDCItem.TextSize = new System.Drawing.Size(60, 18);
            this.lciEDCItem.TextToControlDistance = 5;
            // 
            // lciEquipment
            // 
            this.lciEquipment.Control = this.lueEquipment;
            this.lciEquipment.CustomizationFormText = "设      备";
            this.lciEquipment.Location = new System.Drawing.Point(0, 56);
            this.lciEquipment.Name = "lciEquipment";
            this.lciEquipment.Size = new System.Drawing.Size(873, 28);
            this.lciEquipment.Text = "设      备";
            this.lciEquipment.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciEquipment.TextSize = new System.Drawing.Size(60, 18);
            this.lciEquipment.TextToControlDistance = 5;
            // 
            // lciPartType
            // 
            this.lciPartType.Control = this.luePartType;
            this.lciPartType.CustomizationFormText = "成品类型";
            this.lciPartType.Location = new System.Drawing.Point(0, 84);
            this.lciPartType.Name = "lciPartType";
            this.lciPartType.Size = new System.Drawing.Size(873, 28);
            this.lciPartType.Text = "成品类型";
            this.lciPartType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lciPartType.TextSize = new System.Drawing.Size(60, 18);
            this.lciPartType.TextToControlDistance = 5;
            // 
            // lciEndTime
            // 
            this.lciEndTime.Control = this.deEndTime;
            this.lciEndTime.CustomizationFormText = "结束时间";
            this.lciEndTime.Location = new System.Drawing.Point(0, 168);
            this.lciEndTime.Name = "lciEndTime";
            this.lciEndTime.Size = new System.Drawing.Size(436, 28);
            this.lciEndTime.Text = "结束时间";
            this.lciEndTime.TextSize = new System.Drawing.Size(60, 18);
            // 
            // lciCommand
            // 
            this.lciCommand.Control = this.btnQuery;
            this.lciCommand.CustomizationFormText = "查询";
            this.lciCommand.Location = new System.Drawing.Point(0, 196);
            this.lciCommand.MaxSize = new System.Drawing.Size(103, 33);
            this.lciCommand.MinSize = new System.Drawing.Size(103, 33);
            this.lciCommand.Name = "lciCommand";
            this.lciCommand.Size = new System.Drawing.Size(873, 33);
            this.lciCommand.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciCommand.Text = "查询";
            this.lciCommand.TextSize = new System.Drawing.Size(0, 0);
            this.lciCommand.TextVisible = false;
            // 
            // lciStartTime
            // 
            this.lciStartTime.Control = this.deStartTime;
            this.lciStartTime.CustomizationFormText = "起始时间";
            this.lciStartTime.Location = new System.Drawing.Point(436, 168);
            this.lciStartTime.Name = "lciStartTime";
            this.lciStartTime.Size = new System.Drawing.Size(437, 28);
            this.lciStartTime.Text = "起始时间";
            this.lciStartTime.TextSize = new System.Drawing.Size(60, 18);
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcResults;
            this.lciResult.CustomizationFormText = "查询结果";
            this.lciResult.Location = new System.Drawing.Point(0, 269);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(885, 371);
            this.lciResult.Text = "查询结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lciPagenation
            // 
            this.lciPagenation.Control = this.pgnQueryResult;
            this.lciPagenation.CustomizationFormText = "分页导航";
            this.lciPagenation.Location = new System.Drawing.Point(0, 640);
            this.lciPagenation.MaxSize = new System.Drawing.Size(0, 37);
            this.lciPagenation.MinSize = new System.Drawing.Size(243, 37);
            this.lciPagenation.Name = "lciPagenation";
            this.lciPagenation.Size = new System.Drawing.Size(885, 37);
            this.lciPagenation.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPagenation.Text = "分页导航";
            this.lciPagenation.TextSize = new System.Drawing.Size(0, 0);
            this.lciPagenation.TextVisible = false;
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
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(885, 677);
            this.tableLayoutPanelMain.TabIndex = 93;
            // 
            // EDCQueryCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "EDCQueryCtrl";
            this.Size = new System.Drawing.Size(887, 737);
            this.Load += new System.EventHandler(this.EDCMainCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbOperation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueFactoryRoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEDCItem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEquipment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLotNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.luePartType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFactoryRoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblLotNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEDCItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEquipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPagenation)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit txtLotNo;
        private DevExpress.XtraEditors.ComboBoxEdit cbOperation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupMain;
        private DevExpress.XtraLayout.LayoutControlItem lblLotNo;
        private DevExpress.XtraLayout.LayoutControlItem lblOperation;
        private DevExpress.XtraEditors.LookUpEdit lueEquipment;
        private DevExpress.XtraLayout.LayoutControlItem lciEquipment;
        private DevExpress.XtraEditors.LookUpEdit lueEDCItem;
        private DevExpress.XtraLayout.LayoutControlItem lciEDCItem;
        private DevExpress.XtraEditors.LookUpEdit lueFactoryRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciFactoryRoom;
        private DevExpress.XtraLayout.LayoutControlItem lciPartType;
        private DevExpress.XtraEditors.LookUpEdit luePartType;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.DateEdit deEndTime;
        private DevExpress.XtraEditors.DateEdit deStartTime;
        private DevExpress.XtraLayout.LayoutControlItem lciStartTime;
        private DevExpress.XtraLayout.LayoutControlItem lciEndTime;
        private DevExpress.XtraLayout.LayoutControlItem lciCommand;
        private DevExpress.XtraLayout.LayoutControlGroup lcgQuery;
        private DevExpress.XtraGrid.GridControl gcResults;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResults;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private DevExpress.XtraGrid.Columns.GridColumn gcolIndex;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEDCName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEDCStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolStepName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEquipmentName;
        private DevExpress.XtraGrid.Columns.GridColumn gcolPartType;
        private DevExpress.XtraGrid.Columns.GridColumn gcolCreator;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditor;
        private DevExpress.XtraGrid.Columns.GridColumn gcolEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcolRemark;
        private FanHai.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPagenation;
        private DevExpress.XtraGrid.Columns.GridColumn gcolLotNo;

    }
}

namespace FanHai.Hemera.Addins.WIP
{
    partial class LotQueryHelpDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.pgnQueryResult = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.gcResult = new DevExpress.XtraGrid.GridControl();
            this.gvResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gclRowNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclLotNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclLotType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclWorkorderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclProId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclRoute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclEfficiency = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclStateFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclLineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclEquipment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclLotKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.teLotNumber = new DevExpress.XtraEditors.TextEdit();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciLotNumber = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciQuery = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciResult = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPageNavigation = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPageNavigation)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.AllowCustomization = false;
            this.Content.Controls.Add(this.pgnQueryResult);
            this.Content.Controls.Add(this.gcResult);
            this.Content.Controls.Add(this.btnQuery);
            this.Content.Controls.Add(this.teLotNumber);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgRoot;
            this.Content.Size = new System.Drawing.Size(650, 301);
            this.Content.TabIndex = 0;
            // 
            // pgnQueryResult
            // 
            this.pgnQueryResult.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.pgnQueryResult.Appearance.Options.UseBackColor = true;
            this.pgnQueryResult.Location = new System.Drawing.Point(4, 267);
            this.pgnQueryResult.Margin = new System.Windows.Forms.Padding(0);
            this.pgnQueryResult.Name = "pgnQueryResult";
            this.pgnQueryResult.PageNo = 1;
            this.pgnQueryResult.Pages = 0;
            this.pgnQueryResult.PageSize = 200;
            this.pgnQueryResult.Records = 0;
            this.pgnQueryResult.Size = new System.Drawing.Size(642, 30);
            this.pgnQueryResult.TabIndex = 64;
            this.pgnQueryResult.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.pgnQueryResult_DataPaging);
            // 
            // gcResult
            // 
            this.gcResult.Location = new System.Drawing.Point(4, 30);
            this.gcResult.MainView = this.gvResult;
            this.gcResult.Name = "gcResult";
            this.gcResult.Size = new System.Drawing.Size(642, 233);
            this.gcResult.TabIndex = 6;
            this.gcResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResult});
            // 
            // gvResult
            // 
            this.gvResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gclRowNum,
            this.gclLotNumber,
            this.gclLotType,
            this.gclWorkorderNo,
            this.gclProId,
            this.gclQty,
            this.gclRoute,
            this.gclOperation,
            this.gclEfficiency,
            this.gclStateFlag,
            this.gclLineName,
            this.gclEquipment,
            this.gclLotKey});
            this.gvResult.GridControl = this.gcResult;
            this.gvResult.Name = "gvResult";
            this.gvResult.OptionsBehavior.Editable = false;
            this.gvResult.OptionsBehavior.ReadOnly = true;
            this.gvResult.OptionsView.ShowGroupPanel = false;
            this.gvResult.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvResult_CustomDrawCell);
            this.gvResult.DoubleClick += new System.EventHandler(this.gvResult_DoubleClick);
            // 
            // gclRowNum
            // 
            this.gclRowNum.Caption = "序号";
            this.gclRowNum.FieldName = "ROW_NUM";
            this.gclRowNum.Name = "gclRowNum";
            this.gclRowNum.OptionsColumn.AllowEdit = false;
            this.gclRowNum.OptionsColumn.FixedWidth = true;
            this.gclRowNum.OptionsColumn.ReadOnly = true;
            this.gclRowNum.Visible = true;
            this.gclRowNum.VisibleIndex = 0;
            this.gclRowNum.Width = 50;
            // 
            // gclLotNumber
            // 
            this.gclLotNumber.Caption = "序列号";
            this.gclLotNumber.FieldName = "LOT_NUMBER";
            this.gclLotNumber.Name = "gclLotNumber";
            this.gclLotNumber.Visible = true;
            this.gclLotNumber.VisibleIndex = 1;
            this.gclLotNumber.Width = 70;
            // 
            // gclLotType
            // 
            this.gclLotType.Caption = "批次类型";
            this.gclLotType.FieldName = "LOT_TYPE";
            this.gclLotType.Name = "gclLotType";
            this.gclLotType.Visible = true;
            this.gclLotType.VisibleIndex = 2;
            this.gclLotType.Width = 67;
            // 
            // gclWorkorderNo
            // 
            this.gclWorkorderNo.Caption = "工单号";
            this.gclWorkorderNo.FieldName = "WORK_ORDER_NO";
            this.gclWorkorderNo.Name = "gclWorkorderNo";
            this.gclWorkorderNo.Visible = true;
            this.gclWorkorderNo.VisibleIndex = 3;
            this.gclWorkorderNo.Width = 70;
            // 
            // gclProId
            // 
            this.gclProId.Caption = "产品ID号";
            this.gclProId.FieldName = "PRO_ID";
            this.gclProId.Name = "gclProId";
            this.gclProId.Visible = true;
            this.gclProId.VisibleIndex = 4;
            this.gclProId.Width = 70;
            // 
            // gclQty
            // 
            this.gclQty.Caption = "电池片数量";
            this.gclQty.FieldName = "QUANTITY";
            this.gclQty.Name = "gclQty";
            this.gclQty.OptionsColumn.FixedWidth = true;
            this.gclQty.Visible = true;
            this.gclQty.VisibleIndex = 5;
            this.gclQty.Width = 85;
            // 
            // gclRoute
            // 
            this.gclRoute.Caption = "工艺流程";
            this.gclRoute.FieldName = "ROUTE_NAME";
            this.gclRoute.Name = "gclRoute";
            this.gclRoute.Visible = true;
            this.gclRoute.VisibleIndex = 6;
            this.gclRoute.Width = 66;
            // 
            // gclOperation
            // 
            this.gclOperation.Caption = "当前工序";
            this.gclOperation.FieldName = "ROUTE_STEP_NAME";
            this.gclOperation.Name = "gclOperation";
            this.gclOperation.Visible = true;
            this.gclOperation.VisibleIndex = 7;
            this.gclOperation.Width = 66;
            // 
            // gclEfficiency
            // 
            this.gclEfficiency.Caption = "转换效率";
            this.gclEfficiency.FieldName = "EFFICIENCY";
            this.gclEfficiency.Name = "gclEfficiency";
            this.gclEfficiency.Visible = true;
            this.gclEfficiency.VisibleIndex = 8;
            this.gclEfficiency.Width = 77;
            // 
            // gclStateFlag
            // 
            this.gclStateFlag.Caption = "批次状态";
            this.gclStateFlag.FieldName = "STATE_FLAG";
            this.gclStateFlag.MinWidth = 50;
            this.gclStateFlag.Name = "gclStateFlag";
            this.gclStateFlag.Visible = true;
            this.gclStateFlag.VisibleIndex = 9;
            // 
            // gclLineName
            // 
            this.gclLineName.Caption = "当前线别";
            this.gclLineName.FieldName = "OPR_LINE";
            this.gclLineName.MinWidth = 50;
            this.gclLineName.Name = "gclLineName";
            this.gclLineName.Visible = true;
            this.gclLineName.VisibleIndex = 10;
            // 
            // gclEquipment
            // 
            this.gclEquipment.Caption = "当前设备";
            this.gclEquipment.FieldName = "EQUIPMENT_NAME";
            this.gclEquipment.MinWidth = 50;
            this.gclEquipment.Name = "gclEquipment";
            this.gclEquipment.Visible = true;
            this.gclEquipment.VisibleIndex = 11;
            // 
            // gclLotKey
            // 
            this.gclLotKey.Caption = "批次主键";
            this.gclLotKey.FieldName = "LOT_KEY";
            this.gclLotKey.Name = "gclLotKey";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(569, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(77, 22);
            this.btnQuery.StyleController = this.Content;
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // teLotNumber
            // 
            this.teLotNumber.Location = new System.Drawing.Point(43, 4);
            this.teLotNumber.Name = "teLotNumber";
            this.teLotNumber.Size = new System.Drawing.Size(522, 20);
            this.teLotNumber.StyleController = this.Content;
            this.teLotNumber.TabIndex = 4;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciLotNumber,
            this.lciQuery,
            this.lciResult,
            this.lciPageNavigation});
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.lcgRoot.Size = new System.Drawing.Size(650, 301);
            this.lcgRoot.TextVisible = false;
            // 
            // lciLotNumber
            // 
            this.lciLotNumber.Control = this.teLotNumber;
            this.lciLotNumber.CustomizationFormText = "序列号";
            this.lciLotNumber.Location = new System.Drawing.Point(0, 0);
            this.lciLotNumber.Name = "lciLotNumber";
            this.lciLotNumber.Size = new System.Drawing.Size(565, 26);
            this.lciLotNumber.Text = "序列号";
            this.lciLotNumber.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lciQuery
            // 
            this.lciQuery.Control = this.btnQuery;
            this.lciQuery.CustomizationFormText = "查询";
            this.lciQuery.Location = new System.Drawing.Point(565, 0);
            this.lciQuery.MaxSize = new System.Drawing.Size(81, 26);
            this.lciQuery.MinSize = new System.Drawing.Size(81, 26);
            this.lciQuery.Name = "lciQuery";
            this.lciQuery.Size = new System.Drawing.Size(81, 26);
            this.lciQuery.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciQuery.Text = "查询";
            this.lciQuery.TextSize = new System.Drawing.Size(0, 0);
            this.lciQuery.TextVisible = false;
            // 
            // lciResult
            // 
            this.lciResult.Control = this.gcResult;
            this.lciResult.CustomizationFormText = "查询结果";
            this.lciResult.Location = new System.Drawing.Point(0, 26);
            this.lciResult.Name = "lciResult";
            this.lciResult.Size = new System.Drawing.Size(646, 237);
            this.lciResult.Text = "查询结果";
            this.lciResult.TextSize = new System.Drawing.Size(0, 0);
            this.lciResult.TextVisible = false;
            // 
            // lciPageNavigation
            // 
            this.lciPageNavigation.Control = this.pgnQueryResult;
            this.lciPageNavigation.CustomizationFormText = "分页";
            this.lciPageNavigation.Location = new System.Drawing.Point(0, 263);
            this.lciPageNavigation.MaxSize = new System.Drawing.Size(0, 34);
            this.lciPageNavigation.MinSize = new System.Drawing.Size(104, 34);
            this.lciPageNavigation.Name = "lciPageNavigation";
            this.lciPageNavigation.Size = new System.Drawing.Size(646, 34);
            this.lciPageNavigation.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPageNavigation.Text = "分页";
            this.lciPageNavigation.TextSize = new System.Drawing.Size(0, 0);
            this.lciPageNavigation.TextVisible = false;
            // 
            // LotQueryHelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(650, 301);
            this.ControlBox = false;
            this.Controls.Add(this.Content);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LotQueryHelpDialog";
            this.Deactivate += new System.EventHandler(this.OperationHelpDialog_Deactivate);
            this.Load += new System.EventHandler(this.LotQueryHelpDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLotNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPageNavigation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit teLotNumber;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciLotNumber;
        private DevExpress.XtraLayout.LayoutControlItem lciQuery;
        private DevExpress.XtraGrid.GridControl gcResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResult;
        private DevExpress.XtraLayout.LayoutControlItem lciResult;
        private FanHai.Hemera.Utils.Controls.PaginationControl pgnQueryResult;
        private DevExpress.XtraLayout.LayoutControlItem lciPageNavigation;
        private DevExpress.XtraGrid.Columns.GridColumn gclRowNum;
        private DevExpress.XtraGrid.Columns.GridColumn gclLotNumber;
        private DevExpress.XtraGrid.Columns.GridColumn gclWorkorderNo;
        private DevExpress.XtraGrid.Columns.GridColumn gclProId;
        private DevExpress.XtraGrid.Columns.GridColumn gclQty;
        private DevExpress.XtraGrid.Columns.GridColumn gclOperation;
        private DevExpress.XtraGrid.Columns.GridColumn gclEfficiency;
        private DevExpress.XtraGrid.Columns.GridColumn gclRoute;
        private DevExpress.XtraGrid.Columns.GridColumn gclLotKey;
        private DevExpress.XtraGrid.Columns.GridColumn gclLotType;
        private DevExpress.XtraGrid.Columns.GridColumn gclLineName;
        private DevExpress.XtraGrid.Columns.GridColumn gclStateFlag;
        private DevExpress.XtraGrid.Columns.GridColumn gclEquipment;

    }
}
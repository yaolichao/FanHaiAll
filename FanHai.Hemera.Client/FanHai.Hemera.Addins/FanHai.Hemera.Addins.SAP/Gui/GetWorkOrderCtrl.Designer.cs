namespace FanHai.Hemera.Addins.SAP
{
    partial class GetWorkOrderCtrl
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
            this.gdcData = new DevExpress.XtraGrid.GridControl();
            this.gdvWorkOrderDefault = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcSEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOrderNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPartNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDescriptions = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOrderType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcQuantityOrdered = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOrderState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStartTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcFinishTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStockLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcFactoryName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Content = new DevExpress.XtraLayout.LayoutControl();
            this.lcgTopMain = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciResults = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.tsbGetWorkOrderInfo = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.tsbGetWorkOrderList = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvWorkOrderDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.Content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.topPanel.Size = new System.Drawing.Size(726, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(506, 0);
            this.lblInfos.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 14:19:10";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // gdcData
            // 
            this.gdcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gdcData.Location = new System.Drawing.Point(5, 33);
            this.gdcData.MainView = this.gdvWorkOrderDefault;
            this.gdcData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdcData.Name = "gdcData";
            this.gdcData.Size = new System.Drawing.Size(487, 497);
            this.gdcData.TabIndex = 16;
            this.gdcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gdvWorkOrderDefault});
            // 
            // gdvWorkOrderDefault
            // 
            this.gdvWorkOrderDefault.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcSEQ,
            this.gcOrderNum,
            this.gcPartNum,
            this.gcDescriptions,
            this.gcOrderType,
            this.gcQuantityOrdered,
            this.gcOrderState,
            this.gcStartTime,
            this.gcFinishTime,
            this.gcStockLocation,
            this.gcFactoryName});
            this.gdvWorkOrderDefault.DetailHeight = 450;
            this.gdvWorkOrderDefault.FixedLineWidth = 3;
            this.gdvWorkOrderDefault.GridControl = this.gdcData;
            this.gdvWorkOrderDefault.Name = "gdvWorkOrderDefault";
            this.gdvWorkOrderDefault.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gdvWorkOrderDefault.OptionsBehavior.Editable = false;
            this.gdvWorkOrderDefault.OptionsView.ShowGroupPanel = false;
            this.gdvWorkOrderDefault.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gdvWorkOrderDefault_CustomDrawRowIndicator);
            this.gdvWorkOrderDefault.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gdvWorkOrderDefault_CustomDrawCell);
            // 
            // gcSEQ
            // 
            this.gcSEQ.Caption = "序号";
            this.gcSEQ.FieldName = "ROWNUM";
            this.gcSEQ.MaxWidth = 57;
            this.gcSEQ.MinWidth = 57;
            this.gcSEQ.Name = "gcSEQ";
            this.gcSEQ.OptionsColumn.AllowEdit = false;
            this.gcSEQ.OptionsColumn.FixedWidth = true;
            this.gcSEQ.Visible = true;
            this.gcSEQ.VisibleIndex = 0;
            this.gcSEQ.Width = 57;
            // 
            // gcOrderNum
            // 
            this.gcOrderNum.Caption = "工单号";
            this.gcOrderNum.FieldName = "ORDER_NUMBER";
            this.gcOrderNum.MinWidth = 23;
            this.gcOrderNum.Name = "gcOrderNum";
            this.gcOrderNum.OptionsColumn.AllowEdit = false;
            this.gcOrderNum.Visible = true;
            this.gcOrderNum.VisibleIndex = 1;
            this.gcOrderNum.Width = 95;
            // 
            // gcPartNum
            // 
            this.gcPartNum.Caption = "成品编码";
            this.gcPartNum.FieldName = "PART_NUMBER";
            this.gcPartNum.MinWidth = 23;
            this.gcPartNum.Name = "gcPartNum";
            this.gcPartNum.OptionsColumn.AllowEdit = false;
            this.gcPartNum.Visible = true;
            this.gcPartNum.VisibleIndex = 2;
            this.gcPartNum.Width = 95;
            // 
            // gcDescriptions
            // 
            this.gcDescriptions.Caption = "成品描述";
            this.gcDescriptions.FieldName = "DESCRIPTIONS";
            this.gcDescriptions.MinWidth = 23;
            this.gcDescriptions.Name = "gcDescriptions";
            this.gcDescriptions.OptionsColumn.AllowEdit = false;
            this.gcDescriptions.Visible = true;
            this.gcDescriptions.VisibleIndex = 3;
            this.gcDescriptions.Width = 95;
            // 
            // gcOrderType
            // 
            this.gcOrderType.Caption = "订单类型";
            this.gcOrderType.FieldName = "ORDER_TYPE";
            this.gcOrderType.MinWidth = 23;
            this.gcOrderType.Name = "gcOrderType";
            this.gcOrderType.OptionsColumn.AllowEdit = false;
            this.gcOrderType.Visible = true;
            this.gcOrderType.VisibleIndex = 4;
            this.gcOrderType.Width = 95;
            // 
            // gcQuantityOrdered
            // 
            this.gcQuantityOrdered.Caption = "订单数量";
            this.gcQuantityOrdered.FieldName = "QUANTITY_ORDERED";
            this.gcQuantityOrdered.MinWidth = 23;
            this.gcQuantityOrdered.Name = "gcQuantityOrdered";
            this.gcQuantityOrdered.OptionsColumn.AllowEdit = false;
            this.gcQuantityOrdered.Visible = true;
            this.gcQuantityOrdered.VisibleIndex = 7;
            this.gcQuantityOrdered.Width = 95;
            // 
            // gcOrderState
            // 
            this.gcOrderState.Caption = "状态";
            this.gcOrderState.FieldName = "ORDER_STATE";
            this.gcOrderState.MinWidth = 23;
            this.gcOrderState.Name = "gcOrderState";
            this.gcOrderState.OptionsColumn.AllowEdit = false;
            this.gcOrderState.Visible = true;
            this.gcOrderState.VisibleIndex = 8;
            this.gcOrderState.Width = 95;
            // 
            // gcStartTime
            // 
            this.gcStartTime.Caption = "开始日期";
            this.gcStartTime.FieldName = "START_TIME";
            this.gcStartTime.MinWidth = 23;
            this.gcStartTime.Name = "gcStartTime";
            this.gcStartTime.OptionsColumn.AllowEdit = false;
            this.gcStartTime.Visible = true;
            this.gcStartTime.VisibleIndex = 5;
            this.gcStartTime.Width = 95;
            // 
            // gcFinishTime
            // 
            this.gcFinishTime.Caption = "完成日期";
            this.gcFinishTime.FieldName = "FINISH_TIME";
            this.gcFinishTime.MinWidth = 23;
            this.gcFinishTime.Name = "gcFinishTime";
            this.gcFinishTime.OptionsColumn.AllowEdit = false;
            this.gcFinishTime.Visible = true;
            this.gcFinishTime.VisibleIndex = 6;
            this.gcFinishTime.Width = 95;
            // 
            // gcStockLocation
            // 
            this.gcStockLocation.Caption = "入库库位";
            this.gcStockLocation.FieldName = "STOCK_LOCATION";
            this.gcStockLocation.MinWidth = 23;
            this.gcStockLocation.Name = "gcStockLocation";
            this.gcStockLocation.OptionsColumn.AllowEdit = false;
            this.gcStockLocation.Visible = true;
            this.gcStockLocation.VisibleIndex = 9;
            this.gcStockLocation.Width = 95;
            // 
            // gcFactoryName
            // 
            this.gcFactoryName.Caption = "工厂";
            this.gcFactoryName.FieldName = "FACTORY_NAME";
            this.gcFactoryName.MinWidth = 23;
            this.gcFactoryName.Name = "gcFactoryName";
            this.gcFactoryName.OptionsColumn.AllowEdit = false;
            this.gcFactoryName.Visible = true;
            this.gcFactoryName.VisibleIndex = 10;
            this.gcFactoryName.Width = 115;
            // 
            // Content
            // 
            this.Content.AllowCustomization = false;
            this.Content.Controls.Add(this.gdcData);
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Content.Name = "Content";
            this.Content.Root = this.lcgTopMain;
            this.Content.Size = new System.Drawing.Size(497, 537);
            this.Content.TabIndex = 0;
            this.Content.Text = "layoutControl1";
            // 
            // lcgTopMain
            // 
            this.lcgTopMain.CustomizationFormText = " ";
            this.lcgTopMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgTopMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciResults});
            this.lcgTopMain.Name = "Root";
            this.lcgTopMain.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgTopMain.Size = new System.Drawing.Size(497, 537);
            this.lcgTopMain.Text = " ";
            // 
            // lciResults
            // 
            this.lciResults.Control = this.gdcData;
            this.lciResults.CustomizationFormText = "结果";
            this.lciResults.Location = new System.Drawing.Point(0, 0);
            this.lciResults.Name = "lciResults";
            this.lciResults.Size = new System.Drawing.Size(491, 503);
            this.lciResults.Text = "结果";
            this.lciResults.TextSize = new System.Drawing.Size(0, 0);
            this.lciResults.TextVisible = false;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(1, 60);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.Content);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(726, 537);
            this.splitContainerControl1.SplitterPosition = 222;
            this.splitContainerControl1.TabIndex = 3;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.AllowCustomization = false;
            this.layoutControl1.Controls.Add(this.tsbGetWorkOrderInfo);
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.tsbGetWorkOrderList);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(222, 537);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // tsbGetWorkOrderInfo
            // 
            this.tsbGetWorkOrderInfo.Location = new System.Drawing.Point(112, 503);
            this.tsbGetWorkOrderInfo.Name = "tsbGetWorkOrderInfo";
            this.tsbGetWorkOrderInfo.Size = new System.Drawing.Size(105, 27);
            this.tsbGetWorkOrderInfo.StyleController = this.layoutControl1;
            this.tsbGetWorkOrderInfo.TabIndex = 21;
            this.tsbGetWorkOrderInfo.Text = "获取工单信息";
            this.tsbGetWorkOrderInfo.Click += new System.EventHandler(this.tsbGetWorkOrderInfo_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(53, 33);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(164, 24);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 19;
            this.textEdit1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWorkOrder_KeyPress);
            // 
            // tsbGetWorkOrderList
            // 
            this.tsbGetWorkOrderList.Location = new System.Drawing.Point(5, 503);
            this.tsbGetWorkOrderList.Name = "tsbGetWorkOrderList";
            this.tsbGetWorkOrderList.Size = new System.Drawing.Size(103, 27);
            this.tsbGetWorkOrderList.StyleController = this.layoutControl1;
            this.tsbGetWorkOrderList.TabIndex = 20;
            this.tsbGetWorkOrderList.Text = "工单信息清单";
            this.tsbGetWorkOrderList.Click += new System.EventHandler(this.tsbGetWorkOrderList_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = " ";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.layoutControlGroup1.Size = new System.Drawing.Size(222, 537);
            this.layoutControlGroup1.Text = " ";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.textEdit1;
            this.layoutControlItem1.CustomizationFormText = "工单号";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "lbWorkOrder";
            this.layoutControlItem1.Size = new System.Drawing.Size(216, 30);
            this.layoutControlItem1.Text = "工单号";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(45, 18);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 30);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(105, 440);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(105, 30);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(111, 440);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tsbGetWorkOrderList;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 470);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(107, 33);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tsbGetWorkOrderInfo;
            this.layoutControlItem3.Location = new System.Drawing.Point(107, 470);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(109, 33);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // GetWorkOrderCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "GetWorkOrderCtrl";
            this.Size = new System.Drawing.Size(728, 597);
            this.Load += new System.EventHandler(this.GetWorkOrderCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvWorkOrderDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.Content.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcgTopMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraLayout.LayoutControl Content;
        private DevExpress.XtraLayout.LayoutControlGroup lcgTopMain;
        private DevExpress.XtraGrid.GridControl gdcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gdvWorkOrderDefault;
        private DevExpress.XtraGrid.Columns.GridColumn gcSEQ;
        private DevExpress.XtraGrid.Columns.GridColumn gcOrderNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartNum;
        private DevExpress.XtraGrid.Columns.GridColumn gcDescriptions;
        private DevExpress.XtraGrid.Columns.GridColumn gcOrderType;
        private DevExpress.XtraGrid.Columns.GridColumn gcQuantityOrdered;
        private DevExpress.XtraGrid.Columns.GridColumn gcOrderState;
        private DevExpress.XtraGrid.Columns.GridColumn gcStartTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcFinishTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcStockLocation;
        private DevExpress.XtraGrid.Columns.GridColumn gcFactoryName;
        private DevExpress.XtraLayout.LayoutControlItem lciResults;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton tsbGetWorkOrderInfo;
        private DevExpress.XtraEditors.SimpleButton tsbGetWorkOrderList;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}

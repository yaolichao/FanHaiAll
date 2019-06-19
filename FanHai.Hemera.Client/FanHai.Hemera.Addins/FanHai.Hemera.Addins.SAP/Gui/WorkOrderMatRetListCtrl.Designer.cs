namespace FanHai.Hemera.Addins.SAP
{
    partial class WorkOrderMatRetListCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderMatRetListCtrl));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.Content = new DevExpress.XtraGrid.GridControl();
            this.gvWoRetMatList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcReturnNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatLot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMatDes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcReturnQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcWorkOrderNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcSupplier = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcFactory = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcReturnDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcShift = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcOperator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWoRetMatList)).BeginInit();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(948, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(728, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-02-14 10:42:08";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.PanelTitle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(1, 60);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(948, 629);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.labelControl1);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 31);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(942, 55);
            this.PanelTitle.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl1.Location = new System.Drawing.Point(2, 2);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(168, 32);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "工单退料清单";
            // 
            // Content
            // 
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Content.Location = new System.Drawing.Point(3, 94);
            this.Content.MainView = this.gvWoRetMatList;
            this.Content.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(942, 586);
            this.Content.TabIndex = 1;
            this.Content.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWoRetMatList});
            this.Content.DoubleClick += new System.EventHandler(this.Content_DoubleClick);
            // 
            // gvWoRetMatList
            // 
            this.gvWoRetMatList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcIndex,
            this.gcReturnNo,
            this.gcMatLot,
            this.gcMatCode,
            this.gcMatDes,
            this.gcUint,
            this.gcReturnQty,
            this.gcWorkOrderNo,
            this.gcSupplier,
            this.gcOperation,
            this.gcStore,
            this.gcFactory,
            this.gcReturnDate,
            this.gcShift,
            this.gcOperator});
            this.gvWoRetMatList.DetailHeight = 450;
            this.gvWoRetMatList.FixedLineWidth = 3;
            this.gvWoRetMatList.GridControl = this.Content;
            this.gvWoRetMatList.Name = "gvWoRetMatList";
            // 
            // gcIndex
            // 
            this.gcIndex.Caption = "序号";
            this.gcIndex.FieldName = "INDEX1";
            this.gcIndex.MinWidth = 23;
            this.gcIndex.Name = "gcIndex";
            this.gcIndex.OptionsColumn.AllowEdit = false;
            this.gcIndex.Visible = true;
            this.gcIndex.VisibleIndex = 0;
            this.gcIndex.Width = 86;
            // 
            // gcReturnNo
            // 
            this.gcReturnNo.Caption = "退料单号";
            this.gcReturnNo.FieldName = "RETURNNO";
            this.gcReturnNo.MinWidth = 23;
            this.gcReturnNo.Name = "gcReturnNo";
            this.gcReturnNo.OptionsColumn.ReadOnly = true;
            this.gcReturnNo.Visible = true;
            this.gcReturnNo.VisibleIndex = 1;
            this.gcReturnNo.Width = 86;
            // 
            // gcMatLot
            // 
            this.gcMatLot.Caption = "物料批号";
            this.gcMatLot.FieldName = "MATLOT";
            this.gcMatLot.MinWidth = 23;
            this.gcMatLot.Name = "gcMatLot";
            this.gcMatLot.OptionsColumn.ReadOnly = true;
            this.gcMatLot.Visible = true;
            this.gcMatLot.VisibleIndex = 2;
            this.gcMatLot.Width = 86;
            // 
            // gcMatCode
            // 
            this.gcMatCode.Caption = "物料编码";
            this.gcMatCode.FieldName = "MATCODE";
            this.gcMatCode.MinWidth = 23;
            this.gcMatCode.Name = "gcMatCode";
            this.gcMatCode.OptionsColumn.ReadOnly = true;
            this.gcMatCode.Visible = true;
            this.gcMatCode.VisibleIndex = 3;
            this.gcMatCode.Width = 86;
            // 
            // gcMatDes
            // 
            this.gcMatDes.Caption = "物料描述";
            this.gcMatDes.FieldName = "MATDES";
            this.gcMatDes.MinWidth = 23;
            this.gcMatDes.Name = "gcMatDes";
            this.gcMatDes.OptionsColumn.ReadOnly = true;
            this.gcMatDes.Visible = true;
            this.gcMatDes.VisibleIndex = 4;
            this.gcMatDes.Width = 86;
            // 
            // gcUint
            // 
            this.gcUint.Caption = "计量单位";
            this.gcUint.FieldName = "UNIT";
            this.gcUint.MinWidth = 23;
            this.gcUint.Name = "gcUint";
            this.gcUint.OptionsColumn.ReadOnly = true;
            this.gcUint.Visible = true;
            this.gcUint.VisibleIndex = 5;
            this.gcUint.Width = 86;
            // 
            // gcReturnQty
            // 
            this.gcReturnQty.Caption = "退料数量";
            this.gcReturnQty.FieldName = "RETURNQTY";
            this.gcReturnQty.MinWidth = 23;
            this.gcReturnQty.Name = "gcReturnQty";
            this.gcReturnQty.OptionsColumn.ReadOnly = true;
            this.gcReturnQty.Visible = true;
            this.gcReturnQty.VisibleIndex = 6;
            this.gcReturnQty.Width = 86;
            // 
            // gcWorkOrderNo
            // 
            this.gcWorkOrderNo.Caption = "工单号码";
            this.gcWorkOrderNo.FieldName = "WORKORDERNO";
            this.gcWorkOrderNo.MinWidth = 23;
            this.gcWorkOrderNo.Name = "gcWorkOrderNo";
            this.gcWorkOrderNo.OptionsColumn.ReadOnly = true;
            this.gcWorkOrderNo.Visible = true;
            this.gcWorkOrderNo.VisibleIndex = 7;
            this.gcWorkOrderNo.Width = 86;
            // 
            // gcSupplier
            // 
            this.gcSupplier.Caption = "批次供应商";
            this.gcSupplier.FieldName = "SUPPLIER";
            this.gcSupplier.MinWidth = 23;
            this.gcSupplier.Name = "gcSupplier";
            this.gcSupplier.OptionsColumn.ReadOnly = true;
            this.gcSupplier.Visible = true;
            this.gcSupplier.VisibleIndex = 8;
            this.gcSupplier.Width = 86;
            // 
            // gcOperation
            // 
            this.gcOperation.Caption = "工序名称";
            this.gcOperation.FieldName = "OPERATION";
            this.gcOperation.MinWidth = 23;
            this.gcOperation.Name = "gcOperation";
            this.gcOperation.OptionsColumn.ReadOnly = true;
            this.gcOperation.Visible = true;
            this.gcOperation.VisibleIndex = 9;
            this.gcOperation.Width = 86;
            // 
            // gcStore
            // 
            this.gcStore.Caption = "线上仓";
            this.gcStore.FieldName = "STORE";
            this.gcStore.MinWidth = 23;
            this.gcStore.Name = "gcStore";
            this.gcStore.OptionsColumn.ReadOnly = true;
            this.gcStore.Visible = true;
            this.gcStore.VisibleIndex = 10;
            this.gcStore.Width = 86;
            // 
            // gcFactory
            // 
            this.gcFactory.Caption = "工厂车间";
            this.gcFactory.FieldName = "FACROOM";
            this.gcFactory.MinWidth = 23;
            this.gcFactory.Name = "gcFactory";
            this.gcFactory.OptionsColumn.ReadOnly = true;
            this.gcFactory.Visible = true;
            this.gcFactory.VisibleIndex = 11;
            this.gcFactory.Width = 86;
            // 
            // gcReturnDate
            // 
            this.gcReturnDate.Caption = "退料时间";
            this.gcReturnDate.FieldName = "RETURNDATE";
            this.gcReturnDate.MinWidth = 23;
            this.gcReturnDate.Name = "gcReturnDate";
            this.gcReturnDate.OptionsColumn.ReadOnly = true;
            this.gcReturnDate.Visible = true;
            this.gcReturnDate.VisibleIndex = 12;
            this.gcReturnDate.Width = 86;
            // 
            // gcShift
            // 
            this.gcShift.Caption = "班次";
            this.gcShift.FieldName = "SHIFT";
            this.gcShift.MinWidth = 23;
            this.gcShift.Name = "gcShift";
            this.gcShift.OptionsColumn.ReadOnly = true;
            this.gcShift.Visible = true;
            this.gcShift.VisibleIndex = 13;
            this.gcShift.Width = 86;
            // 
            // gcOperator
            // 
            this.gcOperator.Caption = "员工号";
            this.gcOperator.FieldName = "OPERATOR";
            this.gcOperator.MinWidth = 23;
            this.gcOperator.Name = "gcOperator";
            this.gcOperator.OptionsColumn.ReadOnly = true;
            this.gcOperator.Visible = true;
            this.gcOperator.VisibleIndex = 14;
            this.gcOperator.Width = 86;
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbQuery,
            this.toolStripSeparator1,
            this.tsbClose});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(948, 27);
            this.toolStripMain.TabIndex = 2;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tsbQuery
            // 
            this.tsbQuery.Image = ((System.Drawing.Image)(resources.GetObject("tsbQuery.Image")));
            this.tsbQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuery.Name = "tsbQuery";
            this.tsbQuery.Size = new System.Drawing.Size(63, 24);
            this.tsbQuery.Text = "查询";
            this.tsbQuery.Click += new System.EventHandler(this.tsbQuery_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::FanHai.Hemera.Addins.SAP.Properties.Resources.cancel;
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(63, 24);
            this.tsbClose.Text = "关闭";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // WorkOrderMatRetListCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "WorkOrderMatRetListCtrl";
            this.Size = new System.Drawing.Size(950, 689);
            this.Load += new System.EventHandler(this.WorkOrderMatRetListCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWoRetMatList)).EndInit();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraGrid.GridControl Content;
        private DevExpress.XtraGrid.Views.Grid.GridView gvWoRetMatList;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tsbQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private DevExpress.XtraGrid.Columns.GridColumn gcIndex;
        private DevExpress.XtraGrid.Columns.GridColumn gcReturnNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatLot;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcMatDes;
        private DevExpress.XtraGrid.Columns.GridColumn gcUint;
        private DevExpress.XtraGrid.Columns.GridColumn gcReturnQty;
        private DevExpress.XtraGrid.Columns.GridColumn gcWorkOrderNo;
        private DevExpress.XtraGrid.Columns.GridColumn gcSupplier;
        private DevExpress.XtraGrid.Columns.GridColumn gcOperation;
        private DevExpress.XtraGrid.Columns.GridColumn gcStore;
        private DevExpress.XtraGrid.Columns.GridColumn gcFactory;
        private DevExpress.XtraGrid.Columns.GridColumn gcReturnDate;
        private DevExpress.XtraGrid.Columns.GridColumn gcShift;
        private DevExpress.XtraGrid.Columns.GridColumn gcOperator;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}

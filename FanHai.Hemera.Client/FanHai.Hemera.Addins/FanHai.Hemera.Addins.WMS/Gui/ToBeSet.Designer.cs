namespace FanHai.Hemera.Addins.WMS
{
    partial class ToBeSet
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnInWarehouse = new DevExpress.XtraEditors.SimpleButton();
            this.txtPallet_NO = new DevExpress.XtraEditors.TextEdit();
            this.gcList = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcWorkOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcProductCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcNewWorkOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPallet_NO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.topPanel.Size = new System.Drawing.Size(662, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(442, 0);
            this.lblInfos.Size = new System.Drawing.Size(220, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-03-29 18:33:08";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnInWarehouse);
            this.layoutControl1.Controls.Add(this.txtPallet_NO);
            this.layoutControl1.Controls.Add(this.gcList);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(1, 60);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1185, 446, 650, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(662, 431);
            this.layoutControl1.TabIndex = 3;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnInWarehouse
            // 
            this.btnInWarehouse.Location = new System.Drawing.Point(547, 40);
            this.btnInWarehouse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInWarehouse.Name = "btnInWarehouse";
            this.btnInWarehouse.Size = new System.Drawing.Size(103, 35);
            this.btnInWarehouse.StyleController = this.layoutControl1;
            this.btnInWarehouse.TabIndex = 8;
            this.btnInWarehouse.Text = "入库";
            this.btnInWarehouse.Click += new System.EventHandler(this.btnInWarehouse_Click);
            // 
            // txtPallet_NO
            // 
            this.txtPallet_NO.Location = new System.Drawing.Point(60, 12);
            this.txtPallet_NO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPallet_NO.Name = "txtPallet_NO";
            this.txtPallet_NO.Size = new System.Drawing.Size(590, 24);
            this.txtPallet_NO.StyleController = this.layoutControl1;
            this.txtPallet_NO.TabIndex = 7;
            this.txtPallet_NO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPallet_NO_KeyPress);
            // 
            // gcList
            // 
            this.gcList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.gcList.Location = new System.Drawing.Point(12, 79);
            this.gcList.MainView = this.gridView1;
            this.gcList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcList.Name = "gcList";
            this.gcList.Size = new System.Drawing.Size(638, 340);
            this.gcList.TabIndex = 5;
            this.gcList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcWorkOrder,
            this.gcProductCode,
            this.gcQty,
            this.gcNewWorkOrder});
            this.gridView1.DetailHeight = 450;
            this.gridView1.FixedLineWidth = 3;
            this.gridView1.GridControl = this.gcList;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gcWorkOrder
            // 
            this.gcWorkOrder.Caption = "工单";
            this.gcWorkOrder.FieldName = "gcWorkOrder";
            this.gcWorkOrder.MinWidth = 23;
            this.gcWorkOrder.Name = "gcWorkOrder";
            this.gcWorkOrder.OptionsColumn.ReadOnly = true;
            this.gcWorkOrder.Visible = true;
            this.gcWorkOrder.VisibleIndex = 0;
            this.gcWorkOrder.Width = 86;
            // 
            // gcProductCode
            // 
            this.gcProductCode.Caption = "料号";
            this.gcProductCode.FieldName = "gcProductCode";
            this.gcProductCode.MinWidth = 23;
            this.gcProductCode.Name = "gcProductCode";
            this.gcProductCode.OptionsColumn.ReadOnly = true;
            this.gcProductCode.Visible = true;
            this.gcProductCode.VisibleIndex = 1;
            this.gcProductCode.Width = 86;
            // 
            // gcQty
            // 
            this.gcQty.Caption = "数量";
            this.gcQty.FieldName = "gcQty";
            this.gcQty.MinWidth = 23;
            this.gcQty.Name = "gcQty";
            this.gcQty.OptionsColumn.ReadOnly = true;
            this.gcQty.Visible = true;
            this.gcQty.VisibleIndex = 2;
            this.gcQty.Width = 86;
            // 
            // gcNewWorkOrder
            // 
            this.gcNewWorkOrder.Caption = "新工单";
            this.gcNewWorkOrder.FieldName = "gcNewWorkOrder";
            this.gcNewWorkOrder.MinWidth = 23;
            this.gcNewWorkOrder.Name = "gcNewWorkOrder";
            this.gcNewWorkOrder.Visible = true;
            this.gcNewWorkOrder.VisibleIndex = 3;
            this.gcNewWorkOrder.Width = 86;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem2,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(662, 431);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 28);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(535, 39);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gcList;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 67);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(642, 344);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtPallet_NO;
            this.layoutControlItem4.CustomizationFormText = "托盘号";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(642, 28);
            this.layoutControlItem4.Text = "托盘号";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(45, 18);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnInWarehouse;
            this.layoutControlItem5.Location = new System.Drawing.Point(535, 28);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(107, 39);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(107, 39);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(107, 39);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // ToBeSet
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ToBeSet";
            this.Size = new System.Drawing.Size(664, 491);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPallet_NO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraGrid.GridControl gcList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn gcWorkOrder;
        private DevExpress.XtraGrid.Columns.GridColumn gcProductCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcQty;
        private DevExpress.XtraEditors.SimpleButton btnInWarehouse;
        private DevExpress.XtraEditors.TextEdit txtPallet_NO;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraGrid.Columns.GridColumn gcNewWorkOrder;
    }
}

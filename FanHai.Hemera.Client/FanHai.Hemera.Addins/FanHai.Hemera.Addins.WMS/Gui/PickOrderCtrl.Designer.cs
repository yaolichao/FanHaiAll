using FanHai.Hemera.Utils.Controls;
namespace FanHai.Hemera.Addins.WMS.Gui
{
    partial class PickOrderCtrl : BaseUserCtrl
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
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btQuery = new System.Windows.Forms.ToolStripButton();
            this.btSave = new System.Windows.Forms.ToolStripButton();
            this.btConfirm = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ZREQNOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ZREQPSR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATINNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DEC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATWTB = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Content = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.posnr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MATNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.vbeln = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ARKTX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.kwmeng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OutBQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WERKS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VRKME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LGORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VSTEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CHARG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONTAINER_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PALLET = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ATINN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ATWRT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ZREQNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ZREQPS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.edOutBandNO = new System.Windows.Forms.ToolStripTextBox();
            this.TBVbeln = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.edCI = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.cbShipType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.edShipNO = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.edCabinetNO = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.edPalletNo = new System.Windows.Forms.ToolStripTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMain.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btQuery,
            this.btSave,
            this.btConfirm,
            this.toolStripButton2});
            this.toolStripMain.Location = new System.Drawing.Point(0, 23);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1000, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // btQuery
            // 
            this.btQuery.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_069;
            this.btQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(52, 22);
            this.btQuery.Text = "查询";
            this.btQuery.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btSave
            // 
            this.btSave.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_063;
            this.btSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(52, 22);
            this.btSave.Text = "保存";
            this.btSave.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // btConfirm
            // 
            this.btConfirm.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_055;
            this.btConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(52, 22);
            this.btConfirm.Text = "确认";
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_060;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton2.Text = "删除此项";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(994, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "外向交货单拣配";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.gridControl1, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.Content, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.toolStrip1, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.splitter1, 0, 4);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1000, 633);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 517);
            this.gridControl1.MainView = this.gridView4;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(994, 113);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ZREQNOR,
            this.ZREQPSR,
            this.ATINNR,
            this.DEC,
            this.ATWTB});
            this.gridView4.GridControl = this.gridControl1;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // ZREQNOR
            // 
            this.ZREQNOR.Caption = "需求单号";
            this.ZREQNOR.FieldName = "DMANDNO";
            this.ZREQNOR.Name = "ZREQNOR";
            this.ZREQNOR.Visible = true;
            this.ZREQNOR.VisibleIndex = 0;
            this.ZREQNOR.Width = 110;
            // 
            // ZREQPSR
            // 
            this.ZREQPSR.Caption = "行号";
            this.ZREQPSR.FieldName = "DMANDPS";
            this.ZREQPSR.Name = "ZREQPSR";
            this.ZREQPSR.Visible = true;
            this.ZREQPSR.VisibleIndex = 1;
            this.ZREQPSR.Width = 87;
            // 
            // ATINNR
            // 
            this.ATINNR.Caption = "内部特性";
            this.ATINNR.FieldName = "ATINN";
            this.ATINNR.Name = "ATINNR";
            this.ATINNR.Visible = true;
            this.ATINNR.VisibleIndex = 4;
            this.ATINNR.Width = 137;
            // 
            // DEC
            // 
            this.DEC.Caption = "特性描述";
            this.DEC.FieldName = "DEC";
            this.DEC.Name = "DEC";
            this.DEC.Visible = true;
            this.DEC.VisibleIndex = 2;
            this.DEC.Width = 192;
            // 
            // ATWTB
            // 
            this.ATWTB.Caption = "特性值";
            this.ATWTB.FieldName = "ATWTB";
            this.ATWTB.Name = "ATWTB";
            this.ATWTB.Visible = true;
            this.ATWTB.VisibleIndex = 3;
            this.ATWTB.Width = 584;
            // 
            // Content
            // 
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 76);
            this.Content.MainView = this.gridView1;
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(994, 426);
            this.Content.TabIndex = 3;
            this.Content.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.gridView2,
            this.gridView3});
            this.Content.Click += new System.EventHandler(this.Content_Click);
            // 
            // gridView1
            // 
            this.gridView1.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gridView1.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Gainsboro;
            this.gridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gridView1.Appearance.Empty.BackColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.Empty.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gridView1.Appearance.Empty.Options.UseBackColor = true;
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.Gray;
            this.gridView1.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.Gray;
            this.gridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gridView1.Appearance.FilterPanel.BackColor = System.Drawing.Color.Gray;
            this.gridView1.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridView1.Appearance.FooterPanel.BackColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.FooterPanel.BorderColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.GroupButton.BackColor = System.Drawing.Color.Silver;
            this.gridView1.Appearance.GroupButton.BorderColor = System.Drawing.Color.Silver;
            this.gridView1.Appearance.GroupButton.Options.UseBackColor = true;
            this.gridView1.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gridView1.Appearance.GroupFooter.BackColor = System.Drawing.Color.Silver;
            this.gridView1.Appearance.GroupFooter.BorderColor = System.Drawing.Color.Silver;
            this.gridView1.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gridView1.Appearance.GroupPanel.BackColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.GroupPanel.ForeColor = System.Drawing.Color.White;
            this.gridView1.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.GroupRow.BackColor = System.Drawing.Color.Silver;
            this.gridView1.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.GroupRow.Options.UseBackColor = true;
            this.gridView1.Appearance.GroupRow.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.DarkGray;
            this.gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.LightSlateGray;
            this.gridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridView1.Appearance.HorzLine.BackColor = System.Drawing.Color.LightGray;
            this.gridView1.Appearance.HorzLine.Options.UseBackColor = true;
            this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.gridView1.Appearance.Preview.BackColor = System.Drawing.Color.Gainsboro;
            this.gridView1.Appearance.Preview.ForeColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.Preview.Options.UseBackColor = true;
            this.gridView1.Appearance.Preview.Options.UseForeColor = true;
            this.gridView1.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.Row.Options.UseBackColor = true;
            this.gridView1.Appearance.RowSeparator.BackColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.VertLine.BackColor = System.Drawing.Color.LightGray;
            this.gridView1.Appearance.VertLine.Options.UseBackColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.posnr,
            this.MATNR,
            this.vbeln,
            this.ARKTX,
            this.kwmeng,
            this.OutBQty,
            this.WERKS,
            this.VRKME,
            this.LGORT,
            this.VSTEL,
            this.CHARG,
            this.CONTAINER_CODE,
            this.PALLET});
            this.gridView1.GridControl = this.Content;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.EnableAppearanceOddRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // posnr
            // 
            this.posnr.Caption = "行号";
            this.posnr.FieldName = "POSNR";
            this.posnr.Name = "posnr";
            this.posnr.OptionsColumn.ReadOnly = true;
            this.posnr.Visible = true;
            this.posnr.VisibleIndex = 1;
            this.posnr.Width = 54;
            // 
            // MATNR
            // 
            this.MATNR.Caption = "物料号";
            this.MATNR.FieldName = "MATNR";
            this.MATNR.Name = "MATNR";
            this.MATNR.Visible = true;
            this.MATNR.VisibleIndex = 2;
            this.MATNR.Width = 94;
            // 
            // vbeln
            // 
            this.vbeln.Caption = "销售订单号";
            this.vbeln.FieldName = "VBELN";
            this.vbeln.Name = "vbeln";
            this.vbeln.OptionsColumn.ReadOnly = true;
            this.vbeln.Visible = true;
            this.vbeln.VisibleIndex = 0;
            this.vbeln.Width = 80;
            // 
            // ARKTX
            // 
            this.ARKTX.Caption = "物料描述";
            this.ARKTX.FieldName = "ARKTX";
            this.ARKTX.Name = "ARKTX";
            this.ARKTX.Visible = true;
            this.ARKTX.VisibleIndex = 3;
            this.ARKTX.Width = 88;
            // 
            // kwmeng
            // 
            this.kwmeng.Caption = "数量";
            this.kwmeng.FieldName = "KWMENG";
            this.kwmeng.Name = "kwmeng";
            this.kwmeng.Visible = true;
            this.kwmeng.VisibleIndex = 4;
            this.kwmeng.Width = 87;
            // 
            // OutBQty
            // 
            this.OutBQty.Caption = "出库数量";
            this.OutBQty.FieldName = "OutBQty";
            this.OutBQty.Name = "OutBQty";
            this.OutBQty.Visible = true;
            this.OutBQty.VisibleIndex = 5;
            this.OutBQty.Width = 85;
            // 
            // WERKS
            // 
            this.WERKS.Caption = "工厂";
            this.WERKS.FieldName = "WERKS";
            this.WERKS.Name = "WERKS";
            this.WERKS.OptionsColumn.ReadOnly = true;
            this.WERKS.Visible = true;
            this.WERKS.VisibleIndex = 12;
            this.WERKS.Width = 87;
            // 
            // VRKME
            // 
            this.VRKME.Caption = "单位";
            this.VRKME.FieldName = "VRKME";
            this.VRKME.Name = "VRKME";
            this.VRKME.OptionsColumn.ReadOnly = true;
            this.VRKME.Visible = true;
            this.VRKME.VisibleIndex = 6;
            this.VRKME.Width = 59;
            // 
            // LGORT
            // 
            this.LGORT.Caption = "库存地点";
            this.LGORT.FieldName = "LGORT";
            this.LGORT.Name = "LGORT";
            this.LGORT.OptionsColumn.ReadOnly = true;
            this.LGORT.Visible = true;
            this.LGORT.VisibleIndex = 10;
            this.LGORT.Width = 78;
            // 
            // VSTEL
            // 
            this.VSTEL.Caption = "装运点";
            this.VSTEL.FieldName = "VSTEL";
            this.VSTEL.Name = "VSTEL";
            this.VSTEL.OptionsColumn.ReadOnly = true;
            this.VSTEL.Visible = true;
            this.VSTEL.VisibleIndex = 11;
            this.VSTEL.Width = 49;
            // 
            // CHARG
            // 
            this.CHARG.Caption = "批号";
            this.CHARG.FieldName = "CHARG";
            this.CHARG.Name = "CHARG";
            this.CHARG.Visible = true;
            this.CHARG.VisibleIndex = 8;
            // 
            // CONTAINER_CODE
            // 
            this.CONTAINER_CODE.Caption = "柜号";
            this.CONTAINER_CODE.FieldName = "CONTAINER_CODE";
            this.CONTAINER_CODE.Name = "CONTAINER_CODE";
            this.CONTAINER_CODE.Visible = true;
            this.CONTAINER_CODE.VisibleIndex = 7;
            // 
            // PALLET
            // 
            this.PALLET.Caption = "托盘号";
            this.PALLET.FieldName = "PALLET";
            this.PALLET.Name = "PALLET";
            this.PALLET.Visible = true;
            this.PALLET.VisibleIndex = 9;
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gridView2.GridControl = this.Content;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupedColumns = true;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn2";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "gridColumn3";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "gridColumn4";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "gridColumn5";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ATINN,
            this.ATWRT,
            this.ZREQNO,
            this.ZREQPS});
            this.gridView3.GridControl = this.Content;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // ATINN
            // 
            this.ATINN.Caption = "内部特性";
            this.ATINN.FieldName = "ATINN";
            this.ATINN.Name = "ATINN";
            this.ATINN.Visible = true;
            this.ATINN.VisibleIndex = 0;
            // 
            // ATWRT
            // 
            this.ATWRT.Caption = "特殊值";
            this.ATWRT.FieldName = "ATWRT";
            this.ATWRT.Name = "ATWRT";
            this.ATWRT.Visible = true;
            this.ATWRT.VisibleIndex = 1;
            // 
            // ZREQNO
            // 
            this.ZREQNO.Caption = "需求单号";
            this.ZREQNO.FieldName = "ZREQNO";
            this.ZREQNO.Name = "ZREQNO";
            this.ZREQNO.Visible = true;
            this.ZREQNO.VisibleIndex = 2;
            // 
            // ZREQPS
            // 
            this.ZREQPS.Caption = "行号";
            this.ZREQPS.FieldName = "ZREQPS";
            this.ZREQPS.Name = "ZREQPS";
            this.ZREQPS.Visible = true;
            this.ZREQPS.VisibleIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.edOutBandNO,
            this.TBVbeln,
            this.toolStripLabel4,
            this.toolStripLabel2,
            this.edCI,
            this.toolStripLabel5,
            this.cbShipType,
            this.toolStripLabel6,
            this.edShipNO,
            this.toolStripLabel1,
            this.edCabinetNO,
            this.toolStripLabel7,
            this.edPalletNo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 48);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1000, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel3.Text = "销售订单号";
            // 
            // edOutBandNO
            // 
            this.edOutBandNO.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.edOutBandNO.Name = "edOutBandNO";
            this.edOutBandNO.Size = new System.Drawing.Size(135, 25);
            // 
            // TBVbeln
            // 
            this.TBVbeln.Name = "TBVbeln";
            this.TBVbeln.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel4.Text = "出库单号";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel2.Text = "CI号";
            // 
            // edCI
            // 
            this.edCI.Name = "edCI";
            this.edCI.Size = new System.Drawing.Size(70, 25);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel5.Text = "运输类型";
            // 
            // cbShipType
            // 
            this.cbShipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShipType.Items.AddRange(new object[] {
            "陆运",
            "海运",
            "空运"});
            this.cbShipType.Name = "cbShipType";
            this.cbShipType.Size = new System.Drawing.Size(75, 25);
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel6.Text = "牌照号";
            // 
            // edShipNO
            // 
            this.edShipNO.Name = "edShipNO";
            this.edShipNO.Size = new System.Drawing.Size(70, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel1.Text = "柜号";
            // 
            // edCabinetNO
            // 
            this.edCabinetNO.Name = "edCabinetNO";
            this.edCabinetNO.Size = new System.Drawing.Size(45, 25);
            this.edCabinetNO.Leave += new System.EventHandler(this.edCabinetNO_Leave);
            this.edCabinetNO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edCabinetNO_KeyPress);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel7.Text = "托号";
            // 
            // edPalletNo
            // 
            this.edPalletNo.Name = "edPalletNo";
            this.edPalletNo.Size = new System.Drawing.Size(80, 25);
            this.edPalletNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edPalletNo_KeyPress);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(3, 508);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(782, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 6);
            // 
            // PickOrderCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Name = "PickOrderCtrl";
            this.Size = new System.Drawing.Size(1000, 633);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btSave;
        private System.Windows.Forms.ToolStripButton btConfirm;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox edOutBandNO;
        private System.Windows.Forms.ToolStripTextBox TBVbeln;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox edCabinetNO;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox edCI;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox edShipNO;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripComboBox cbShipType;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn ZREQNOR;
        private DevExpress.XtraGrid.Columns.GridColumn ZREQPSR;
        private DevExpress.XtraGrid.Columns.GridColumn ATINNR;
        private DevExpress.XtraGrid.Columns.GridColumn DEC;
        private DevExpress.XtraGrid.Columns.GridColumn ATWTB;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private DevExpress.XtraGrid.GridControl Content;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn posnr;
        private DevExpress.XtraGrid.Columns.GridColumn MATNR;
        private DevExpress.XtraGrid.Columns.GridColumn vbeln;
        private DevExpress.XtraGrid.Columns.GridColumn ARKTX;
        private DevExpress.XtraGrid.Columns.GridColumn kwmeng;
        private DevExpress.XtraGrid.Columns.GridColumn OutBQty;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS;
        private DevExpress.XtraGrid.Columns.GridColumn VRKME;
        private DevExpress.XtraGrid.Columns.GridColumn LGORT;
        private DevExpress.XtraGrid.Columns.GridColumn VSTEL;
        private DevExpress.XtraGrid.Columns.GridColumn CHARG;
        private DevExpress.XtraGrid.Columns.GridColumn CONTAINER_CODE;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn ATINN;
        private DevExpress.XtraGrid.Columns.GridColumn ATWRT;
        private DevExpress.XtraGrid.Columns.GridColumn ZREQNO;
        private DevExpress.XtraGrid.Columns.GridColumn ZREQPS;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox edPalletNo;
        private DevExpress.XtraGrid.Columns.GridColumn PALLET;


    }
}

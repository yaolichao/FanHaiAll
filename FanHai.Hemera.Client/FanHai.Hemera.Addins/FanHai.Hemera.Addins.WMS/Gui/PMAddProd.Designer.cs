namespace FanHai.Hemera.Addins.WMS.Gui
{
    partial class PMAddProd
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Content = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.posnr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MATNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.vbeln = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ARKTX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.kwmeng = new DevExpress.XtraGrid.Columns.GridColumn();
            this.WERKS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VRKME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LGORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VSTEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CHARG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CONTAINER_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Content, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(888, 404);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Content
            // 
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(3, 28);
            this.Content.MainView = this.gridView1;
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(882, 404);
            this.Content.TabIndex = 5;
            this.Content.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.gridView2,
            this.gridView3});
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
            this.WERKS,
            this.VRKME,
            this.LGORT,
            this.VSTEL,
            this.CHARG,
            this.CONTAINER_CODE});
            this.gridView1.GridControl = this.Content;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.MultiSelect = true;
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
            // WERKS
            // 
            this.WERKS.Caption = "工厂";
            this.WERKS.FieldName = "WERKS";
            this.WERKS.Name = "WERKS";
            this.WERKS.OptionsColumn.ReadOnly = true;
            this.WERKS.Visible = true;
            this.WERKS.VisibleIndex = 10;
            this.WERKS.Width = 87;
            // 
            // VRKME
            // 
            this.VRKME.Caption = "单位";
            this.VRKME.FieldName = "VRKME";
            this.VRKME.Name = "VRKME";
            this.VRKME.OptionsColumn.ReadOnly = true;
            this.VRKME.Visible = true;
            this.VRKME.VisibleIndex = 5;
            this.VRKME.Width = 59;
            // 
            // LGORT
            // 
            this.LGORT.Caption = "库存地点";
            this.LGORT.FieldName = "LGORT";
            this.LGORT.Name = "LGORT";
            this.LGORT.OptionsColumn.ReadOnly = true;
            this.LGORT.Visible = true;
            this.LGORT.VisibleIndex = 8;
            this.LGORT.Width = 78;
            // 
            // VSTEL
            // 
            this.VSTEL.Caption = "装运点";
            this.VSTEL.FieldName = "VSTEL";
            this.VSTEL.Name = "VSTEL";
            this.VSTEL.OptionsColumn.ReadOnly = true;
            this.VSTEL.Visible = true;
            this.VSTEL.VisibleIndex = 9;
            this.VSTEL.Width = 49;
            // 
            // CHARG
            // 
            this.CHARG.Caption = "批号";
            this.CHARG.FieldName = "CHARG";
            this.CHARG.Name = "CHARG";
            this.CHARG.Visible = true;
            this.CHARG.VisibleIndex = 7;
            // 
            // CONTAINER_CODE
            // 
            this.CONTAINER_CODE.Caption = "柜号";
            this.CONTAINER_CODE.FieldName = "CONTAINER_CODE";
            this.CONTAINER_CODE.Name = "CONTAINER_CODE";
            this.CONTAINER_CODE.Visible = true;
            this.CONTAINER_CODE.VisibleIndex = 6;
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
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(888, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_055;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(76, 22);
            this.toolStripButton1.Text = "确认选择";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::FanHai.Hemera.Addins.WMS.Properties.Resources.blue24_070;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton2.Text = "返回";
            this.toolStripButton2.ToolTipText = "取消选择并返回";
            // 
            // PMAddProd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 404);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PMAddProd";
            this.Text = "添加产品";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl Content;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn posnr;
        private DevExpress.XtraGrid.Columns.GridColumn MATNR;
        private DevExpress.XtraGrid.Columns.GridColumn vbeln;
        private DevExpress.XtraGrid.Columns.GridColumn ARKTX;
        private DevExpress.XtraGrid.Columns.GridColumn kwmeng;
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;


    }
}
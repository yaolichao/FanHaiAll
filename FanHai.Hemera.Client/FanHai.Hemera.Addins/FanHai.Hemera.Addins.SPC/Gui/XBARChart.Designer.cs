namespace FanHai.Hemera.Addins.SPC
{
    partial class XBARChart
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Strip strip1 = new DevExpress.XtraCharts.Strip();
            DevExpress.XtraCharts.XYDiagramPane xyDiagramPane1 = new DevExpress.XtraCharts.XYDiagramPane();
            DevExpress.XtraCharts.SecondaryAxisX secondaryAxisX1 = new DevExpress.XtraCharts.SecondaryAxisX();
            DevExpress.XtraCharts.Strip strip2 = new DevExpress.XtraCharts.Strip();
            DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gcCount = new DevExpress.XtraGrid.GridControl();
            this.gvCount = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.VALUE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(strip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(strip2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(779, 386);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // chartControl1
            // 
            this.chartControl1.AppearanceName = "Northern Lights";
            xyDiagram1.AxisX.GridLines.Visible = true;
            xyDiagram1.AxisX.Label.Angle = -45;
            xyDiagram1.AxisX.Label.Antialiasing = true;
            xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
            strip1.MaxLimit.Enabled = false;
            strip1.MinLimit.Enabled = false;
            strip1.Name = "Strip 1";
            strip1.ShowInLegend = false;
            xyDiagram1.AxisX.Strips.AddRange(new DevExpress.XtraCharts.Strip[] {
            strip1});
            xyDiagram1.AxisX.Tickmarks.Visible = false;
            xyDiagram1.AxisX.Title.Font = new System.Drawing.Font("Tahoma", 8F);
            xyDiagram1.AxisX.Title.Text = "X控制图";
            xyDiagram1.AxisX.Visible = false;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "0";
            xyDiagram1.AxisY.GridLines.Visible = false;
            xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisY.Visible = false;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "0";
            xyDiagram1.DefaultPane.Visible = false;
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.EnableAxisYScrolling = true;
            xyDiagram1.EnableAxisXZooming = true;
            xyDiagram1.EnableAxisYZooming = true;
            xyDiagramPane1.Name = "xPane";
            xyDiagramPane1.PaneID = 0;
            xyDiagramPane1.Weight = 1.0053191489361701;
            xyDiagram1.Panes.AddRange(new DevExpress.XtraCharts.XYDiagramPane[] {
            xyDiagramPane1});
            secondaryAxisX1.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisX1.AxisID = 0;
            secondaryAxisX1.Label.Angle = -45;
            secondaryAxisX1.Label.Antialiasing = true;
            secondaryAxisX1.Name = "xX";
            secondaryAxisX1.Range.ScrollingRange.SideMarginsEnabled = true;
            secondaryAxisX1.Range.SideMarginsEnabled = true;
            strip2.MaxLimit.Enabled = false;
            strip2.MinLimit.Enabled = false;
            strip2.Name = "Strip 1";
            strip2.ShowInLegend = false;
            secondaryAxisX1.Strips.AddRange(new DevExpress.XtraCharts.Strip[] {
            strip2});
            secondaryAxisX1.Tickmarks.Visible = false;
            secondaryAxisX1.VisibleInPanesSerializable = "0";
            xyDiagram1.SecondaryAxesX.AddRange(new DevExpress.XtraCharts.SecondaryAxisX[] {
            secondaryAxisX1});
            secondaryAxisY1.Alignment = DevExpress.XtraCharts.AxisAlignment.Near;
            secondaryAxisY1.AxisID = 0;
            secondaryAxisY1.Name = "xY";
            secondaryAxisY1.Range.ScrollingRange.SideMarginsEnabled = true;
            secondaryAxisY1.Range.SideMarginsEnabled = true;
            secondaryAxisY1.VisibleInPanesSerializable = "0";
            xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.BackColor = System.Drawing.Color.Thistle;
            this.chartControl1.Legend.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chartControl1.Legend.EquallySpacedItems = false;
            this.chartControl1.Legend.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
            this.chartControl1.Legend.MarkerSize = new System.Drawing.Size(15, 15);
            this.chartControl1.Legend.SpacingVertical = 5;
            this.chartControl1.Location = new System.Drawing.Point(160, 0);
            this.chartControl1.Margin = new System.Windows.Forms.Padding(0);
            this.chartControl1.Name = "chartControl1";
            pointSeriesLabel1.LineVisible = true;
            pointSeriesLabel1.Visible = false;
            series1.Label = pointSeriesLabel1;
            series1.Name = "xSeries";
            lineSeriesView1.AxisXName = "xX";
            lineSeriesView1.AxisYName = "xY";
            lineSeriesView1.LineMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
            lineSeriesView1.LineMarkerOptions.Size = 8;
            lineSeriesView1.LineStyle.Thickness = 1;
            lineSeriesView1.PaneName = "xPane";
            series1.View = lineSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            pointSeriesLabel2.LineVisible = true;
            this.chartControl1.SeriesTemplate.Label = pointSeriesLabel2;
            this.chartControl1.SeriesTemplate.LegendPointOptions = pointOptions1;
            this.chartControl1.SeriesTemplate.SynchronizePointOptions = false;
            this.chartControl1.SeriesTemplate.View = lineSeriesView2;
            this.chartControl1.Size = new System.Drawing.Size(570, 403);
            this.chartControl1.TabIndex = 1;
            chartTitle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartTitle2.Alignment = System.Drawing.StringAlignment.Near;
            chartTitle2.Dock = DevExpress.XtraCharts.ChartTitleDockStyle.Bottom;
            chartTitle2.Font = new System.Drawing.Font("Tahoma", 8F);
            chartTitle2.Visible = false;
            this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1,
            chartTitle2});
            this.chartControl1.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartControl1_CustomDrawSeriesPoint);
            this.chartControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartControl1_MouseDown);
            this.chartControl1.ObjectHotTracked += new DevExpress.XtraCharts.HotTrackEventHandler(this.chartControl1_ObjectHotTracked);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.gcCount, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chartControl1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(730, 403);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // gcCount
            // 
            this.gcCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCount.Location = new System.Drawing.Point(0, 3);
            this.gcCount.MainView = this.gvCount;
            this.gcCount.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.gcCount.Name = "gcCount";
            this.gcCount.Size = new System.Drawing.Size(157, 397);
            this.gcCount.TabIndex = 4;
            this.gcCount.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCount});
            // 
            // gvCount
            // 
            this.gvCount.ColumnPanelRowHeight = 28;
            this.gvCount.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.KEY,
            this.VALUE});
            this.gvCount.GridControl = this.gcCount;
            this.gvCount.Name = "gvCount";
            this.gvCount.OptionsBehavior.ReadOnly = true;
            this.gvCount.OptionsView.EnableAppearanceEvenRow = true;
            this.gvCount.OptionsView.ShowGroupPanel = false;
            this.gvCount.RowHeight = 26;
            // 
            // KEY
            // 
            this.KEY.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.KEY.AppearanceHeader.Options.UseFont = true;
            this.KEY.Caption = "统计量";
            this.KEY.FieldName = "KEY";
            this.KEY.Name = "KEY";
            this.KEY.Visible = true;
            this.KEY.VisibleIndex = 0;
            this.KEY.Width = 58;
            // 
            // VALUE
            // 
            this.VALUE.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.VALUE.AppearanceHeader.Options.UseFont = true;
            this.VALUE.Caption = "值";
            this.VALUE.FieldName = "VALUE";
            this.VALUE.Name = "VALUE";
            this.VALUE.Visible = true;
            this.VALUE.VisibleIndex = 1;
            this.VALUE.Width = 78;
            // 
            // XBARChart
            // 
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "XBARChart";
            this.Size = new System.Drawing.Size(730, 403);
            ((System.ComponentModel.ISupportInitialize)(strip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagramPane1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(strip2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraGrid.GridControl gcCount;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCount;
        private DevExpress.XtraGrid.Columns.GridColumn KEY;
        private DevExpress.XtraGrid.Columns.GridColumn VALUE;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;      
    }
}

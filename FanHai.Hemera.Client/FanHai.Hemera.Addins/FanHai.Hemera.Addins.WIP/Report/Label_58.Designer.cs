namespace FanHai.Hemera.Addins.WIP.Report
{
    partial class Label_58
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTableP = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrP = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrlotno = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.modulePrintDataSetNew1 = new FanHai.Hemera.Addins.WIP.Report.ModulePrintDataSetNew();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modulePrintDataSetNew1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.BorderWidth = 0;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableP,
            this.xrTable3,
            this.xrBarCode});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 98F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.StylePriority.UseBorderWidth = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableP
            // 
            this.xrTableP.Dpi = 254F;
            this.xrTableP.Font = new System.Drawing.Font("Arial", 20F);
            this.xrTableP.LocationFloat = new DevExpress.Utils.PointFloat(464.9999F, 0F);
            this.xrTableP.Name = "xrTableP";
            this.xrTableP.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTableP.SizeF = new System.Drawing.SizeF(48.2251F, 65.1387F);
            this.xrTableP.StylePriority.UseFont = false;
            this.xrTableP.StylePriority.UseTextAlignment = false;
            this.xrTableP.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrP});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1;
            // 
            // xrP
            // 
            this.xrP.Dpi = 254F;
            this.xrP.Name = "xrP";
            this.xrP.Text = "P";
            this.xrP.Weight = 3;
            // 
            // xrTable3
            // 
            this.xrTable3.BorderWidth = 0;
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Arial", 8.5F);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(95.00001F, 58.19353F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable3.SizeF = new System.Drawing.SizeF(370F, 24F);
            this.xrTable3.StylePriority.UseBorderWidth = false;
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrlotno});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1;
            // 
            // xrlotno
            // 
            this.xrlotno.Dpi = 254F;
            this.xrlotno.Font = new System.Drawing.Font("Arial", 8.7F);
            this.xrlotno.Name = "xrlotno";
            this.xrlotno.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 254F);
            this.xrlotno.StylePriority.UseFont = false;
            this.xrlotno.StylePriority.UsePadding = false;
            this.xrlotno.Weight = 3;
            // 
            // xrBarCode
            // 
            this.xrBarCode.AutoModule = true;
            this.xrBarCode.Dpi = 254F;
            this.xrBarCode.LocationFloat = new DevExpress.Utils.PointFloat(95.00001F, 4.803537F);
            this.xrBarCode.Name = "xrBarCode";
            this.xrBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 20, 254F);
            this.xrBarCode.ShowText = false;
            this.xrBarCode.SizeF = new System.Drawing.SizeF(370F, 77.39F);
            this.xrBarCode.StylePriority.UsePadding = false;
            code128Generator1.CharacterSet = DevExpress.XtraPrinting.BarCode.Code128Charset.CharsetAuto;
            this.xrBarCode.Symbology = code128Generator1;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.TopMargin.Visible = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.BottomMargin.Visible = false;
            // 
            // modulePrintDataSetNew1
            // 
            this.modulePrintDataSetNew1.DataSetName = "ModulePrintDataSetNew";
            this.modulePrintDataSetNew1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Label_58
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataMember = "ModulePrintTable";
            this.DataSource = this.modulePrintDataSetNew1;
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 100;
            this.PageWidth = 600;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.ShowPrintMarginsWarning = false;
            this.SnapGridSize = 31.75F;
            this.Version = "9.3";
            ((System.ComponentModel.ISupportInitialize)(this.xrTableP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modulePrintDataSetNew1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrlotno;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode;
        private ModulePrintDataSetNew modulePrintDataSetNew1;
        private DevExpress.XtraReports.UI.XRTable xrTableP;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrP;
    }
}

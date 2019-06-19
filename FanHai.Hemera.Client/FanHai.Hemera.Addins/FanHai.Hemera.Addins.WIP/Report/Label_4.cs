using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class Label_4 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_4(PrintLabelParameterData data)
        {
            InitializeComponent();
            this.xrBarCode.Text = data.LotNo.ToString().Trim();
            this.xrlotno.Text = data.LotNo.ToString().Trim();
            this.xrTable1.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            this.xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
        }
        public Label_4(string lotNumber,int x,int y)
        {
            InitializeComponent();
            this.xrBarCode.Text = lotNumber.Trim();
            this.xrlotno.Text = lotNumber.Trim();
            this.xrTable1.LocationF = new PointF(xrTable1.LocationF.X + x, xrTable1.LocationF.Y + y);
            this.xrTable3.LocationF = new PointF(xrTable3.LocationF.X + x, xrTable3.LocationF.Y + y);
        }
    }
}

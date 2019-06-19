using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class Label_58 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_58(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrTableP.LocationF = new PointF(xrTableP.LocationF.X + data.X, xrTableP.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
            xrBarCode.LocationF = new PointF(xrBarCode.LocationF.X + data.X, xrBarCode.LocationF.Y + data.Y);
            this.xrBarCode.Text = data.LotNo.ToString();
            this.xrlotno.Text = data.LotNo.ToString();            
        }

    }
}

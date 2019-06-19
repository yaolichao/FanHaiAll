using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class Label_60 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_60(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrBarCode.LocationF = new PointF(xrBarCode.LocationF.X + data.X, xrBarCode.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
            string s_printcode, s_date, s_year, s_month, s_day;
            DateTime d_date;
            d_date = DateTime.Parse(data.TestTime.ToString("yyyy-MM-dd"));
            s_year = d_date.ToString("yy");
            s_month = d_date.ToString("MM");
            s_day = d_date.ToString("dd");
            s_date = s_year + s_month + s_day;
            s_printcode = "21" + data.LotNo.ToString().Trim() + "<FNC1>" + "11" + s_date;
            this.xrBarCode.Text = s_printcode ;
            this.xrlotno.Text = "S/N:" + data.LotNo.ToString().Trim();
        }

    }
}

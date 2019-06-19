using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class Label_324 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_324(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
            string s_pktypename = data.PowersetSubPowerLevel ?? string.Empty;
            string label324 = string.Empty;
            if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
            {
                string[] strSplit = s_pktypename.Split('-');

                label324 = data.LotNo + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);
            }
            else
            {
                label324 = data.LotNo + "-H";
            }
            this.xrBarCode.Text = data.LotNo.ToString();
            this.xrlotno.Text = label324;
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_516 : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_516(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrLine1.LocationF = new PointF(xrLine1.LocationF.X + data.X, xrLine1.LocationF.Y + data.Y);

            xrLabel1.LocationF = new PointF(xrLabel1.LocationF.X + data.X, xrLabel1.LocationF.Y + data.Y);

            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + data.X, xrPictureBox1.LocationF.Y + data.Y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + data.X, xrPictureBox2.LocationF.Y + data.Y);
            xrPictureBox3.LocationF = new PointF(xrPictureBox3.LocationF.X + data.X, xrPictureBox3.LocationF.Y + data.Y);
            xrPictureBox4.LocationF = new PointF(xrPictureBox4.LocationF.X + data.X, xrPictureBox4.LocationF.Y + data.Y);
            xrPictureBox5.LocationF = new PointF(xrPictureBox5.LocationF.X + data.X, xrPictureBox5.LocationF.Y + data.Y);
            xrPictureBox6.LocationF = new PointF(xrPictureBox6.LocationF.X + data.X, xrPictureBox6.LocationF.Y + data.Y);
            xrPictureBox7.LocationF = new PointF(xrPictureBox7.LocationF.X + data.X, xrPictureBox7.LocationF.Y + data.Y);

            t1.LocationF = new PointF(t1.LocationF.X + data.X, t1.LocationF.Y + data.Y);
            t2.LocationF = new PointF(t2.LocationF.X + data.X, t2.LocationF.Y + data.Y);
            t3.LocationF = new PointF(t3.LocationF.X + data.X, t3.LocationF.Y + data.Y);
            t4.LocationF = new PointF(t4.LocationF.X + data.X, t4.LocationF.Y + data.Y);
            t5.LocationF = new PointF(t5.LocationF.X + data.X, t5.LocationF.Y + data.Y);
            t6.LocationF = new PointF(t6.LocationF.X + data.X, t6.LocationF.Y + data.Y);
            t7.LocationF = new PointF(t7.LocationF.X + data.X, t7.LocationF.Y + data.Y);

            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + data.X, xrTable2.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
            xrTable4.LocationF = new PointF(xrTable4.LocationF.X + data.X, xrTable4.LocationF.Y + data.Y);
            xrTable5.LocationF = new PointF(xrTable5.LocationF.X + data.X, xrTable5.LocationF.Y + data.Y);
            xrTable6.LocationF = new PointF(xrTable6.LocationF.X + data.X, xrTable6.LocationF.Y + data.Y);
            xrTable7.LocationF = new PointF(xrTable7.LocationF.X + data.X, xrTable7.LocationF.Y + data.Y);
            xrTable8.LocationF = new PointF(xrTable8.LocationF.X + data.X, xrTable8.LocationF.Y + data.Y);
            xrTable9.LocationF = new PointF(xrTable9.LocationF.X + data.X, xrTable9.LocationF.Y + data.Y);
            xrTable10.LocationF = new PointF(xrTable10.LocationF.X + data.X, xrTable10.LocationF.Y + data.Y);
            xrTable11.LocationF = new PointF(xrTable11.LocationF.X + data.X, xrTable11.LocationF.Y + data.Y);
            xrTable19.LocationF = new PointF(xrTable19.LocationF.X + data.X, xrTable19.LocationF.Y + data.Y);
            xrTable20.LocationF = new PointF(xrTable20.LocationF.X + data.X, xrTable20.LocationF.Y + data.Y);

            #region 516 lotNoNamePlate 获取与定义
            DateTime d_date = DateTime.Parse(data.TestTime.ToString("yyyy-MM-dd").Trim());
            string s_year = d_date.ToString("yy");
            string s_month = d_date.ToString("MM");
            string s_day = d_date.ToString("dd");
            string date = s_year + s_month + s_day;
            string LotNoNamePlate516 = string.Empty;
            LotNoNamePlate516 = "21" + data.LotNo.ToString().Trim() + "<FNC1>" + "11" + date + "94" + data.PowersetModuleCode.ToString().Trim(); ;
            #endregion
            this.xrConergyPh.Text = data.PowersetModuleName.ToString().Trim();
            this.xrpn.Text = data.PowersetModuleCode.ToString().Trim();
            this.xrsn.Text = data.LotNo.ToString().Trim();
            this.xrpm.Text = data.PowersetStandardPM + "Wp";
            this.xrPowerSorting.Text = data.PowersetPowerDifferent.ToString().Trim();
            this.xrImp.Text =data.PowersetStandardIPM.ToString().Trim() + "A";
            this.xrVmp.Text = data.PowersetStandardVPM.ToString().Trim() + "V";
            this.xrVoc.Text = data.PowersetStandardVOC.ToString().Trim() + "V";
            this.xrIsc.Text = data.PowersetStandardISC.ToString().Trim() + "A";
            this.xrBarCode.Text = LotNoNamePlate516;
        }

    }
}

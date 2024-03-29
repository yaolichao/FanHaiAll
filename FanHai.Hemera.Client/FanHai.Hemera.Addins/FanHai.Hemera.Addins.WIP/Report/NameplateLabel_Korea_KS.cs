using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    //韩国KS认证铭牌 add by yibin.fei 2017.12.20
    public partial class NameplateLabel_Korea_KS : DevExpress.XtraReports.UI.XtraReport
    {

       
       public NameplateLabel_Korea_KS(PrintLabelParameterData data)
       {
           InitializeComponent();
      }
       public NameplateLabel_Korea_KS(string pscode, string p, string p_2, string p_3, string p_4, string p_5, string p_6, string p_7,
          string p_8, string p_9, string p_10, string p_14, int x, int y, string code)
        {
            InitializeComponent();
            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + x, xrTable1.LocationF.Y + y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + x, xrTable2.LocationF.Y + y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + x, xrTable3.LocationF.Y + y);
            xrTable4.LocationF = new PointF(xrTable4.LocationF.X + x, xrTable4.LocationF.Y + y);
            xrTable5.LocationF = new PointF(xrTable5.LocationF.X + x, xrTable5.LocationF.Y + y);
            xrTable6.LocationF = new PointF(xrTable6.LocationF.X + x, xrTable6.LocationF.Y + y);
            xrTable7.LocationF = new PointF(xrTable7.LocationF.X + x, xrTable7.LocationF.Y + y);
            xrTable8.LocationF = new PointF(xrTable8.LocationF.X + x, xrTable8.LocationF.Y + y);
            xrTable9.LocationF = new PointF(xrTable9.LocationF.X + x, xrTable9.LocationF.Y + y);
            xrTable10.LocationF = new PointF(xrTable10.LocationF.X + x, xrTable10.LocationF.Y + y);
            xrTable11.LocationF = new PointF(xrTable11.LocationF.X + x, xrTable11.LocationF.Y + y);
            xrTable12.LocationF = new PointF(xrTable12.LocationF.X + x, xrTable12.LocationF.Y + y);
            xrTable13.LocationF = new PointF(xrTable13.LocationF.X + x, xrTable13.LocationF.Y + y);
            xrTable14.LocationF = new PointF(xrTable14.LocationF.X + x, xrTable14.LocationF.Y + y);
            xrTable15.LocationF = new PointF(xrTable15.LocationF.X + x, xrTable15.LocationF.Y + y);
            xrTable16.LocationF = new PointF(xrTable16.LocationF.X + x, xrTable16.LocationF.Y + y);
            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + x, xrPictureBox1.LocationF.Y+y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + x, xrPictureBox2.LocationF.Y + y);
            xrLabel1.LocationF = new PointF(xrLabel1.LocationF.X + x, xrLabel1.LocationF.Y + y);


            #region  赋值
            xrModuleName.Text = code;
            xrModuleName2.Text = code;
            xrPower.Text = Convert.ToDecimal(p).ToString("#.0") + "Wp";
            xrVoc.Text = p_2 + "V";
            xrIsc.Text = p_3 + "A";
            xrVmp.Text = p_4 + "V";
            xrImp.Text = p_5 + "A";
       
                xrCerNo.Text = p_9;
               xrCerDate.Text= p_10;
               xrProductionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //xrFuse.Text = Convert.ToDecimal(p_14).ToString("#.#") + "A";


               xrTableCell19.Text = "개방전압(V)       \r\n      (Voc)";

               xrTableCell23.Text = "정격전압(V)       \r\n      (Vmp)";
               xrTableCell22.Text = "단락전류(A)       \r\n      (Isc)";
               xrTableCell26.Text = "정격전류(A)       \r\n      (Imp)";
           


            #endregion



        }
        
    }
}

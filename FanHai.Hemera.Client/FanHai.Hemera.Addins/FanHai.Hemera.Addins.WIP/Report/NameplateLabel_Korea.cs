using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_Korea : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_Korea(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_Korea(string pscode,string p, string p_2, string p_3, string p_4, string p_5, string p_6, string p_7, 
            string p_8, string p_9, string p_10,string p_14, int x, int y,string code)
        {
            InitializeComponent();
            BarCode1.LocationF = new PointF(BarCode1.LocationF.X + x, BarCode1.LocationF.Y + y);

            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + x, xrPictureBox1.LocationF.Y + y);
            xrPbox01.LocationF = new PointF(xrPbox01.LocationF.X + x, xrPbox01.LocationF.Y + y);
            xrPBox05.LocationF = new PointF(xrPBox05.LocationF.X + x, xrPBox05.LocationF.Y + y);
            xrPBox06.LocationF = new PointF(xrPBox06.LocationF.X + x, xrPBox06.LocationF.Y + y);

            Line1.LocationF = new PointF(Line1.LocationF.X + x, Line1.LocationF.Y + y);
            line2.LocationF = new PointF(line2.LocationF.X + x, line2.LocationF.Y + y);
            line3.LocationF = new PointF(line3.LocationF.X + x, line3.LocationF.Y + y);
            line4.LocationF = new PointF(line4.LocationF.X + x, line4.LocationF.Y + y);

            table1.LocationF = new PointF(table1.LocationF.X + x, table1.LocationF.Y + y);
            table2Module.LocationF = new PointF(table2Module.LocationF.X + x, table2Module.LocationF.Y + y);
            table3Maximum.LocationF = new PointF(table3Maximum.LocationF.X + x, table3Maximum.LocationF.Y + y);
            table4Voc.LocationF = new PointF(table4Voc.LocationF.X + x, table4Voc.LocationF.Y + y);
            table5ISC.LocationF = new PointF(table5ISC.LocationF.X + x, table5ISC.LocationF.Y + y);
            table6Vmp.LocationF = new PointF(table6Vmp.LocationF.X + x, table6Vmp.LocationF.Y + y);
            table7Imp.LocationF = new PointF(table7Imp.LocationF.X + x, table7Imp.LocationF.Y + y);
            tabel8Fuse.LocationF = new PointF(tabel8Fuse.LocationF.X + x, tabel8Fuse.LocationF.Y + y);
            table9Voltage.LocationF = new PointF(table9Voltage.LocationF.X + x, table9Voltage.LocationF.Y + y);
            table10Power.LocationF = new PointF(table10Power.LocationF.X + x, table10Power.LocationF.Y + y);
            table12Cell.LocationF = new PointF(table12Cell.LocationF.X + x, table12Cell.LocationF.Y + y);
            table13Class.LocationF = new PointF(table13Class.LocationF.X + x, table13Class.LocationF.Y + y);
            
            table14All.LocationF = new PointF(table14All.LocationF.X + x, table14All.LocationF.Y + y);
            table15Warning.LocationF = new PointF(table15Warning.LocationF.X + x, table15Warning.LocationF.Y + y);
            table16.LocationF = new PointF(table16.LocationF.X + x, table16.LocationF.Y + y);
            table17.LocationF = new PointF(table17.LocationF.X + x, table17.LocationF.Y + y);
            table18.LocationF = new PointF(table18.LocationF.X + x, table18.LocationF.Y + y);
            table19.LocationF = new PointF(table19.LocationF.X + x, table19.LocationF.Y + y);

            table20.LocationF = new PointF(table20.LocationF.X + x, table20.LocationF.Y + y);
            table21.LocationF = new PointF(table21.LocationF.X + x, table21.LocationF.Y + y);
            table22.LocationF = new PointF(table22.LocationF.X + x, table22.LocationF.Y + y);
            table23.LocationF = new PointF(table23.LocationF.X + x, table23.LocationF.Y + y);
            table24.LocationF = new PointF(table24.LocationF.X + x, table24.LocationF.Y + y);

            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + x, xrTable1.LocationF.Y + y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + x, xrTable2.LocationF.Y + y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + x, xrTable3.LocationF.Y + y);
            xrTable4.LocationF = new PointF(xrTable4.LocationF.X + x, xrTable4.LocationF.Y + y);
            xrTable5.LocationF = new PointF(xrTable5.LocationF.X + x, xrTable5.LocationF.Y + y);
            xrTable6.LocationF = new PointF(xrTable6.LocationF.X + x, xrTable6.LocationF.Y + y);
            #region  ¸³Öµ
            xrModuleName.Text = code;
            BarCode1.Text = pscode;
            xrPower.Text = Convert.ToDecimal(p).ToString("#.0") + "Wp";
            xrVoc.Text = p_2 + "V";
            xrIsc.Text = p_3 + "A";
            xrVmp.Text = p_4 + "V";
            xrImp.Text = p_5 + "A";
            xrFuse.Text = Convert.ToDecimal(p_14).ToString("#.#") + "A";
            xrVoltage.Text = "DC" + p_6 + "V";
            xrTolerance.Text = p_7;
            xrCellTechnology.Text = p_8;
            xrTableCell27.Text = p_9;
            xrTableCell25.Text = p_10;
            xrTableCell23.Text = DateTime.Now.ToString("yyyy-MM-dd");
            #endregion
        }

    }
}

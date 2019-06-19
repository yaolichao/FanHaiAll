using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_TUV : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_TUV(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_TUV(string code, string _voc, 
            string _isc,  string _vmp, 
            string _imp,  string _fuse, 
            string _max,  string _noct, 
            string _cellType,string pscode, 
            string _power,string dif,
            int x,int y)
        {
            InitializeComponent();
            BarCode1.LocationF = new PointF(BarCode1.LocationF.X + x, BarCode1.LocationF.Y + y);

            xrPbox01.LocationF = new PointF(xrPbox01.LocationF.X + x, xrPbox01.LocationF.Y + y);
            xrPbox02.LocationF = new PointF(xrPbox02.LocationF.X + x, xrPbox02.LocationF.Y + y);
            xrPbox03.LocationF = new PointF(xrPbox03.LocationF.X + x, xrPbox03.LocationF.Y + y);
            xrPbox04.LocationF = new PointF(xrPbox04.LocationF.X + x, xrPbox04.LocationF.Y + y);
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
            table11Temp.LocationF = new PointF(table11Temp.LocationF.X + x, table11Temp.LocationF.Y + y);
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

            #region  ∏≥÷µ
            xrModuleName.Text = pscode + "-" + _power;
            BarCode1.Text = code;
            xrPower.Text = Convert.ToDecimal(_power).ToString("#.0") + "Wp";
            xrVoc.Text = _voc + "V";
            xrIsc.Text = _isc + "A";
            xrVmp.Text = _vmp + "V";
            xrImp.Text = _imp + "A";
            xrFuse.Text = Convert.ToDecimal(_fuse).ToString("#.#") + "A";
            xrVoltage.Text = "DC" + _max + "V";
            xrTolerance.Text = dif;
            xrTemp.Text = _noct + "°„C";
            xrCellTechnology.Text = _cellType;
            #endregion
        }

    }
}

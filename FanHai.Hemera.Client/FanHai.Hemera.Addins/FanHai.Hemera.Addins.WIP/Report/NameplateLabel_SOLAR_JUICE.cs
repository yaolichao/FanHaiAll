using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_SOLAR_JUICE : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_SOLAR_JUICE(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_SOLAR_JUICE(string code, string _voc, 
            string _isc,  string _vmp, 
            string _imp,  string _fuse, 
            string _max,  string _noct, 
            string _cellType,string pscode, 
            string _power,string dif,
            int x,int y)
        {
            InitializeComponent();
          

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
            line2.LocationF = new PointF(line2.LocationF.X + x, line2.LocationF.Y + y);
           
            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + x, xrTable1.LocationF.Y + y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + x, xrTable2.LocationF.Y + y);
          
            #region  ¸³Öµ
            string strMoudleName = string.Empty;
            if (pscode.Contains("/HV"))
            {
                strMoudleName = "OSA"+_power+pscode.Substring(pscode.Length-4,1);
            }
            else
            {
               
                strMoudleName = "QSA" + _power+pscode.Substring(pscode.Length-1,1);
            }
            if (pscode.Contains("6612"))
            {
                strMoudleName = strMoudleName + "-72-S";
                xrcellWeight.Text = "21.8kg";
                xrcellDimension.Text = "1954 x 990 x 40mm";
            }
            else if(pscode.Contains("6610"))
            {
                strMoudleName = strMoudleName + "-60-S";
                xrcellWeight.Text = "18.3kg";
                xrcellDimension.Text = "1648 x 990 x 35mm";
            }
            xrcellType.Text = strMoudleName;
            xrcellPower.Text = _power + "W";
            xrcellSorting.Text = dif; //"¡À3%";
            xrcellVoltage.Text = _vmp + "V";
            xrcellCurrent.Text = _imp + "A";
            xrcellOpenVoltage.Text = _voc + "V" + "¡À3%";
            xrcellShortCurrent.Text = _isc + "A" + "¡À5%";
            xrcellSystemVoltage.Text = _max + "VDC";
            xrRating.Text = Convert.ToDecimal(_fuse).ToString("#.#") + "A";
            xrcellTemperture.Text = "-40¡«+85¡ãC";



            //xrcellPower.Text = pscode + "-" + _power;
            ////BarCode1.Text = code;
            //xrcellSorting.Text = Convert.ToDecimal(_power).ToString("#.0") + "Wp";
            //xrcellVoltage.Text = _voc + "V";
            //xrcellCurrent.Text = _isc + "A";
            //xrcellOpenVoltage.Text = _vmp + "V";
            //xrcellShortCurrent.Text = _imp + "A";
            //xrcellSystemVoltage.Text = Convert.ToDecimal(_fuse).ToString("#.#") + "A";
            //xrRating.Text = "DC" + _max + "V";
            //xrcellTemperture.Text = dif;
            //xrcellClass.Text = _noct + "¡ãC";
            //xrCellFrieClass.Text = _cellType;
            #endregion
        }

    }
}

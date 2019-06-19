using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_QTXX : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_QTXX(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_QTXX(string _voc, 
            string _isc,  string _vmp, 
            string _imp,  string _fuse, 
            string _max,  string _noct, 
            string _cellType,string pscode, 
            string _power,string not,string weight,string cc,string dif,
            int x,int y)
        {
            InitializeComponent();

            xrPbox01.LocationF = new PointF(xrPbox01.LocationF.X + x, xrPbox01.LocationF.Y + y);
            xrPbox04.LocationF = new PointF(xrPbox04.LocationF.X + x, xrPbox04.LocationF.Y + y);
            xrPBox06.LocationF = new PointF(xrPBox06.LocationF.X + x, xrPBox06.LocationF.Y + y);
            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + x, xrPictureBox1.LocationF.Y + y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + x, xrPictureBox2.LocationF.Y + y);
            xrPictureBox3.LocationF = new PointF(xrPictureBox3.LocationF.X + x, xrPictureBox3.LocationF.Y + y);
            xrPictureBox4.LocationF = new PointF(xrPictureBox4.LocationF.X + x, xrPictureBox4.LocationF.Y + y);
            xrPictureBox5.LocationF = new PointF(xrPictureBox5.LocationF.X + x, xrPictureBox5.LocationF.Y + y);

            Line1.LocationF = new PointF(Line1.LocationF.X + x, Line1.LocationF.Y + y);
            line2.LocationF = new PointF(line2.LocationF.X + x, line2.LocationF.Y + y);
            line3.LocationF = new PointF(line3.LocationF.X + x, line3.LocationF.Y + y);
            line4.LocationF = new PointF(line4.LocationF.X + x, line4.LocationF.Y + y);

            table98.LocationF = new PointF(table98.LocationF.X + x, table98.LocationF.Y + y);
            table110.LocationF = new PointF(table110.LocationF.X + x, table110.LocationF.Y + y);
            table100.LocationF = new PointF(table100.LocationF.X + x, table100.LocationF.Y + y);
            table102.LocationF = new PointF(table102.LocationF.X + x, table102.LocationF.Y + y);
            table103.LocationF = new PointF(table103.LocationF.X + x, table103.LocationF.Y + y);
            table104.LocationF = new PointF(table104.LocationF.X + x, table104.LocationF.Y + y);
            table105.LocationF = new PointF(table105.LocationF.X + x, table105.LocationF.Y + y);
            tabel106.LocationF = new PointF(tabel106.LocationF.X + x, tabel106.LocationF.Y + y);
            table107.LocationF = new PointF(table107.LocationF.X + x, table107.LocationF.Y + y);
            table101.LocationF = new PointF(table101.LocationF.X + x, table101.LocationF.Y + y);
            table108.LocationF = new PointF(table108.LocationF.X + x, table108.LocationF.Y + y);
            table109.LocationF = new PointF(table109.LocationF.X + x, table109.LocationF.Y + y);
            Table111.LocationF = new PointF(Table111.LocationF.X + x, Table111.LocationF.Y + y);
            table112.LocationF = new PointF(table112.LocationF.X + x, table112.LocationF.Y + y);
            table113.LocationF = new PointF(table113.LocationF.X + x, table113.LocationF.Y + y);
            table114.LocationF = new PointF(table114.LocationF.X + x, table114.LocationF.Y + y);
            table115.LocationF = new PointF(table115.LocationF.X + x, table115.LocationF.Y + y);
            table18.LocationF = new PointF(table18.LocationF.X + x, table18.LocationF.Y + y);
            table20.LocationF = new PointF(table20.LocationF.X + x, table20.LocationF.Y + y);
            table21.LocationF = new PointF(table21.LocationF.X + x, table21.LocationF.Y + y);
            table22.LocationF = new PointF(table22.LocationF.X + x, table22.LocationF.Y + y);

            #region  ¸³Öµ
            xrTableCell12.Text = "ÐÍºÅ:" + pscode;


            xrPower.Text = Convert.ToDecimal(_power).ToString("#.0") + "W";
            xrVoc.Text = _voc + "V";
            xrIsc.Text = _isc + "A";
            xrVmp.Text = _vmp + "V";
            xrImp.Text = _imp + "A";
            xrFuse.Text = Convert.ToDecimal(_fuse).ToString("#.#") + "A";
            xrVoltage.Text =  _max + "VDC";
            xrTolerance.Text = dif;
            xrTemp.Text = not + "¡æ";
            xrCellTechnology.Text = weight +"Kg";
            xrcc.Text = cc + "mm";
            
            #endregion
        }

    }
}

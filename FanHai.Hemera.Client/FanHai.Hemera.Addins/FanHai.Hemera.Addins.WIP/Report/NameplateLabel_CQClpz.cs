using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_CQClpz : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_CQClpz(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_CQClpz(string pscode,string _powerCQClpz,string _effCQClpz,string _mianjiCQClpz, 
            string _qtyCQClpz,int x, int y)
        {
            InitializeComponent();
            xrPbox01.LocationF = new PointF(xrPbox01.LocationF.X + x, xrPbox01.LocationF.Y + y);

            table1.LocationF = new PointF(table1.LocationF.X + x, table1.LocationF.Y + y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + x, xrTable2.LocationF.Y + y);
            xrLine2.LocationF = new PointF(xrLine2.LocationF.X + x, xrLine2.LocationF.Y + y);

            table2Module.LocationF = new PointF(table2Module.LocationF.X + x, table2Module.LocationF.Y + y);
            table3Maximum.LocationF = new PointF(table3Maximum.LocationF.X + x, table3Maximum.LocationF.Y + y);
            table4Type.LocationF = new PointF(table4Type.LocationF.X + x, table4Type.LocationF.Y + y);
            table5ZT.LocationF = new PointF(table5ZT.LocationF.X + x, table5ZT.LocationF.Y + y);

            xrLine1.LocationF = new PointF(xrLine1.LocationF.X + x, xrLine1.LocationF.Y + y);
            table6Vmp.LocationF = new PointF(table6Vmp.LocationF.X + x, table6Vmp.LocationF.Y + y);
            xrPbox04.LocationF = new PointF(xrPbox04.LocationF.X + x, xrPbox04.LocationF.Y + y);
            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + x, xrPictureBox1.LocationF.Y + y);
            table12Cell.LocationF = new PointF(table12Cell.LocationF.X + x, table12Cell.LocationF.Y + y);
            table15Warning.LocationF = new PointF(table15Warning.LocationF.X + x, table15Warning.LocationF.Y + y);
            table23.LocationF = new PointF(table23.LocationF.X + x, table23.LocationF.Y + y);
            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + x, xrTable1.LocationF.Y + y);
            table24.LocationF = new PointF(table24.LocationF.X + x, table24.LocationF.Y + y);
            

            #region  ¸³Öµ
            xrXinhao.Text = pscode;
            xrEff.Text = _effCQClpz + "%";
            xrMianji.Text = _mianjiCQClpz + "©O";
            xrBcgongl.Text = _powerCQClpz + "W";
            xrLotNumber.Text = _qtyCQClpz + "Æ¬";
            #endregion
        }

    }
}

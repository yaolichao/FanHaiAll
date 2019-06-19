using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;
using FanHai.Hemera.Utils.Common;
using System.Data;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_NE : DevExpress.XtraReports.UI.XtraReport
    {
        public string moduleType, moduleWeight = string.Empty, moduleDimension = string.Empty;
        public NameplateLabel_NE(string pscode, string _maxNe, string _impNe, string _vmpNe, string _iscNe, string _vocNe, string _noctNe, string p, string p_9, string p_10, string _cellTypeNe, int x, int y)
        {
            InitializeComponent();

            foreach (var tb in Detail.Controls)
            {
                if (tb is XRTable)
                {
                    var xtb = ((XRTable)tb);
                    xtb.LocationF = new PointF(xtb.LocationF.X + x, xtb.LocationF.Y + y);
                    foreach (var row in xtb.Controls)
                    {
                        if (row is XRTableRow)
                        {
                            var xtr = ((XRTableRow)row);
                            xtr.LocationF = new PointF(xtr.LocationF.X + x, xtr.LocationF.Y + y);
                            foreach (var cell in xtr.Controls)
                            {
                                if (cell is XRTableCell)
                                {
                                    var xtc = ((XRTableCell)cell);
                                    xtc.LocationF = new PointF(xtc.LocationF.X + x, xtc.LocationF.Y + y);
                                }
                            }
                        }
                    }
                }
            }
            #region
            /*
             * 
             * 
           xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + x, xrPictureBox1.LocationF.Y + y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + x, xrPictureBox2.LocationF.Y + y);
            xrPictureBox3.LocationF = new PointF(xrPictureBox3.LocationF.X + x, xrPictureBox3.LocationF.Y + y);
            xrPictureBox4.LocationF = new PointF(xrPictureBox4.LocationF.X + x, xrPictureBox4.LocationF.Y + y);
            xrPictureBox5.LocationF = new PointF(xrPictureBox5.LocationF.X + x, xrPictureBox5.LocationF.Y + y);
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
            xrTable17.LocationF = new PointF(xrTable17.LocationF.X + x, xrTable17.LocationF.Y + y);
            xrTable18.LocationF = new PointF(xrTable18.LocationF.X + x, xrTable18.LocationF.Y + y);
            xrTable19.LocationF = new PointF(xrTable19.LocationF.X + x, xrTable19.LocationF.Y + y);
            xrTable20.LocationF = new PointF(xrTable20.LocationF.X + x, xrTable20.LocationF.Y + y);
            xrTable21.LocationF = new PointF(xrTable21.LocationF.X + x, xrTable21.LocationF.Y + y);
            xrTable22.LocationF = new PointF(xrTable22.LocationF.X + x, xrTable22.LocationF.Y + y);
            xrTable23.LocationF = new PointF(xrTable23.LocationF.X + x, xrTable23.LocationF.Y + y);

            xrTableCell1.LocationF = new PointF(xrTableCell1.LocationF.X + x, xrTableCell1.LocationF.Y + y);
            xrTableCell2.LocationF = new PointF(xrTableCell2.LocationF.X + x, xrTableCell2.LocationF.Y + y);
            xrTableCell3.LocationF = new PointF(xrTableCell3.LocationF.X + x, xrTableCell3.LocationF.Y + y);
            xrTableCell4.LocationF = new PointF(xrTableCell4.LocationF.X + x, xrTableCell4.LocationF.Y + y);
            xrTableCell5.LocationF = new PointF(xrTableCell5.LocationF.X + x, xrTableCell5.LocationF.Y + y);
            xrTableCell6.LocationF = new PointF(xrTableCell6.LocationF.X + x, xrTableCell6.LocationF.Y + y);
            xrTableCell7.LocationF = new PointF(xrTableCell7.LocationF.X + x, xrTableCell7.LocationF.Y + y);
            xrTableCell8.LocationF = new PointF(xrTableCell8.LocationF.X + x, xrTableCell8.LocationF.Y + y);
            xrTableCell9.LocationF = new PointF(xrTableCell9.LocationF.X + x, xrTableCell9.LocationF.Y + y);
            xrTableCell10.LocationF = new PointF(xrTableCell10.LocationF.X + x, xrTableCell10.LocationF.Y + y);
            xrTableCell11.LocationF = new PointF(xrTableCell11.LocationF.X + x, xrTableCell11.LocationF.Y + y);
            xrTableCell12.LocationF = new PointF(xrTableCell12.LocationF.X + x, xrTableCell12.LocationF.Y + y);
            xrTableCell13.LocationF = new PointF(xrTableCell13.LocationF.X + x, xrTableCell13.LocationF.Y + y);
            xrTableCell14.LocationF = new PointF(xrTableCell14.LocationF.X + x, xrTableCell14.LocationF.Y + y);
            xrTableCell15.LocationF = new PointF(xrTableCell15.LocationF.X + x, xrTableCell15.LocationF.Y + y);
            xrTableCell16.LocationF = new PointF(xrTableCell16.LocationF.X + x, xrTableCell16.LocationF.Y + y);
            xrTableCell17.LocationF = new PointF(xrTableCell17.LocationF.X + x, xrTableCell17.LocationF.Y + y);
            xrTableCell18.LocationF = new PointF(xrTableCell18.LocationF.X + x, xrTableCell18.LocationF.Y + y);
            xrTableCell19.LocationF = new PointF(xrTableCell19.LocationF.X + x, xrTableCell19.LocationF.Y + y);
            xrTableCell20.LocationF = new PointF(xrTableCell20.LocationF.X + x, xrTableCell20.LocationF.Y + y);
            xrTableCell21.LocationF = new PointF(xrTableCell21.LocationF.X + x, xrTableCell21.LocationF.Y + y);
            xrTableCell22.LocationF = new PointF(xrTableCell22.LocationF.X + x, xrTableCell22.LocationF.Y + y);
            xrTableCell23.LocationF = new PointF(xrTableCell23.LocationF.X + x, xrTableCell23.LocationF.Y + y);
            xrTableCell24.LocationF = new PointF(xrTableCell24.LocationF.X + x, xrTableCell24.LocationF.Y + y);
            xrTableCell25.LocationF = new PointF(xrTableCell25.LocationF.X + x, xrTableCell25.LocationF.Y + y);
            xrTableCell26.LocationF = new PointF(xrTableCell26.LocationF.X + x, xrTableCell26.LocationF.Y + y);
            xrTableCell27.LocationF = new PointF(xrTableCell27.LocationF.X + x, xrTableCell27.LocationF.Y + y);
            xrTableCell28.LocationF = new PointF(xrTableCell28.LocationF.X + x, xrTableCell28.LocationF.Y + y);
            xrTableCell29.LocationF = new PointF(xrTableCell29.LocationF.X + x, xrTableCell29.LocationF.Y + y);
            xrTableCell30.LocationF = new PointF(xrTableCell30.LocationF.X + x, xrTableCell30.LocationF.Y + y);
            xrTableCell31.LocationF = new PointF(xrTableCell31.LocationF.X + x, xrTableCell31.LocationF.Y + y);

            xrTableCell32.LocationF = new PointF(xrTableCell32.LocationF.X + x, xrTableCell32.LocationF.Y + y);
            xrTableCell33.LocationF = new PointF(xrTableCell33.LocationF.X + x, xrTableCell33.LocationF.Y + y);
            xrTableCell34.LocationF = new PointF(xrTableCell34.LocationF.X + x, xrTableCell34.LocationF.Y + y);
            xrTableCell35.LocationF = new PointF(xrTableCell35.LocationF.X + x, xrTableCell35.LocationF.Y + y);
            xrTableCell36.LocationF = new PointF(xrTableCell36.LocationF.X + x, xrTableCell36.LocationF.Y + y);
            xrTableCell37.LocationF = new PointF(xrTableCell37.LocationF.X + x, xrTableCell37.LocationF.Y + y);
            xrTableCell38.LocationF = new PointF(xrTableCell38.LocationF.X + x, xrTableCell38.LocationF.Y + y);
            xrTableCell39.LocationF = new PointF(xrTableCell39.LocationF.X + x, xrTableCell39.LocationF.Y + y);
            xrTableCell40.LocationF = new PointF(xrTableCell40.LocationF.X + x, xrTableCell40.LocationF.Y + y);
            xrTableCell41.LocationF = new PointF(xrTableCell41.LocationF.X + x, xrTableCell41.LocationF.Y + y);
            xrTableCell42.LocationF = new PointF(xrTableCell42.LocationF.X + x, xrTableCell42.LocationF.Y + y);
            xrTableCell43.LocationF = new PointF(xrTableCell43.LocationF.X + x, xrTableCell43.LocationF.Y + y);
            xrTableCell44.LocationF = new PointF(xrTableCell44.LocationF.X + x, xrTableCell44.LocationF.Y + y);
            xrTableCell45.LocationF = new PointF(xrTableCell45.LocationF.X + x, xrTableCell45.LocationF.Y + y);
            xrTableCell46.LocationF = new PointF(xrTableCell46.LocationF.X + x, xrTableCell46.LocationF.Y + y);
            xrTableCell47.LocationF = new PointF(xrTableCell47.LocationF.X + x, xrTableCell47.LocationF.Y + y);
            xrTableCell48.LocationF = new PointF(xrTableCell48.LocationF.X + x, xrTableCell48.LocationF.Y + y);
            xrTableCell49.LocationF = new PointF(xrTableCell49.LocationF.X + x, xrTableCell49.LocationF.Y + y);
            xrTableCell50.LocationF = new PointF(xrTableCell50.LocationF.X + x, xrTableCell50.LocationF.Y + y);
            xrTableCell51.LocationF = new PointF(xrTableCell51.LocationF.X + x, xrTableCell51.LocationF.Y + y);
            xrTableCell52.LocationF = new PointF(xrTableCell52.LocationF.X + x, xrTableCell52.LocationF.Y + y);
            xrTableCell53.LocationF = new PointF(xrTableCell53.LocationF.X + x, xrTableCell53.LocationF.Y + y);
            xrTableCell54.LocationF = new PointF(xrTableCell54.LocationF.X + x, xrTableCell54.LocationF.Y + y);
            xrTableCell55.LocationF = new PointF(xrTableCell55.LocationF.X + x, xrTableCell55.LocationF.Y + y);
            xrTableCell56.LocationF = new PointF(xrTableCell56.LocationF.X + x, xrTableCell56.LocationF.Y + y);
            xrTableCell57.LocationF = new PointF(xrTableCell57.LocationF.X + x, xrTableCell57.LocationF.Y + y);
            xrTableCell58.LocationF = new PointF(xrTableCell58.LocationF.X + x, xrTableCell58.LocationF.Y + y);
            xrLabel1.LocationF = new PointF(xrLabel1.LocationF.X + x, xrLabel1.LocationF.Y + y);
            */
            #endregion



            try
            {
                xrTableCell7.Text = "NERP-CS" + pscode;
                xrTableCell29.Text = _maxNe + "W";
                xrTableCell30.Text = _impNe + "A";
                xrTableCell31.Text = _vmpNe + "V";
                xrTableCell32.Text = _iscNe + "A";
                xrTableCell33.Text = _vocNe + "V";
                xrTableCell34.Text = p;
                xrTableCell56.Text = _noctNe + "¡æ";
                xrTableCell37.Text = p_9;
                xrTableCell41.Text = p_10;
                xrTableCell53.Text = _cellTypeNe;
            }
            catch (Exception ex)
            { }

        }


    }
}

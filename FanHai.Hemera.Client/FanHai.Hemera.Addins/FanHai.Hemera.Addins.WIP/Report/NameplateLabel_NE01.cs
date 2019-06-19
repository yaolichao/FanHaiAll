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
    public partial class NameplateLabel_NE01 : DevExpress.XtraReports.UI.XtraReport
    {
        public string moduleType, moduleWeight = string.Empty, moduleDimension = string.Empty;  
        public NameplateLabel_NE01(string pscode, string _maxNe, string _impNe, string _vmpNe, string _iscNe, string _vocNe, string _fengdnag, string _ckg, string _dianya, string _cellTypeNe, int x, int y)
        {
            InitializeComponent();

            foreach (var tb in Detail.Controls)
            {
               if(tb is XRLine)
               {
                   var xtb = ((XRLine)tb);
                   xtb.LocationF = new PointF(xtb.LocationF.X + x, xtb.LocationF.Y + y);


               }
                if(tb is XRPictureBox)
                {
                    var xtb = ((XRPictureBox)tb);
                    xtb.LocationF = new PointF(xtb.LocationF.X + x, xtb.LocationF.Y + y);
                }
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
           
            #endregion

            try
            {
                xrTableCell7.Text = pscode;
                xrTableCell29.Text = _maxNe + "W";
                xrTableCell30.Text = _impNe + "A";
                xrTableCell31.Text = _vmpNe + "V";
                xrTableCell32.Text = _iscNe + "A";
                xrTableCell33.Text = _vocNe + "V";
                xrTableCell34.Text = _fengdnag;
                xrTableCell41.Text = _ckg;
                xrTableCell53.Text = _cellTypeNe;
            }
            catch (Exception ex)
            { }

        }


    }
}

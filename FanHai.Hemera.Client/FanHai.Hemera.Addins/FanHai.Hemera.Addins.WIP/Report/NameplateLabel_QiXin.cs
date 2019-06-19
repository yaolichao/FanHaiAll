using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_QiXin : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_QiXin(PrintLabelParameterData data)
        {
            InitializeComponent();
        }
        public NameplateLabel_QiXin(string code, string _voc, 
            string _isc,  string _vmp, 
            string _imp,  string _fuse, 
            string _max,  string _noct, 
            string _cellType,string pscode, 
            string _power,string dif,
            int x,int y)
        {
            InitializeComponent();
            xrTable12.LocationF = new PointF(xrTable12.LocationF.X + x, xrTable12.LocationF.Y + y);

            

            #region  ∏≥÷µ
            xrModuleName.Text = pscode;
            BarCode1.Text = code;
            xrPower.Text = Convert.ToDecimal(_power).ToString("#.0") + "Wp";
            xrVoc.Text = _voc + "V";
            xrIsc.Text = _isc + "A";
            xrVmp.Text = _vmp + "V";
            xrImp.Text = _imp + "A";
            xrFuse.Text = Convert.ToDecimal(_fuse).ToString("#.#") + "A";
            xrVoltage.Text =  _max + "VDC";
            xrTolerance.Text = dif;
            xrTemp.Text = _noct + "°„C";
            #endregion
        }

    }
}

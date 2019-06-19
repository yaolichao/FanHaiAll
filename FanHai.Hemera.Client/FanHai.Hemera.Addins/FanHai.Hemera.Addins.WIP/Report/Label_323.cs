using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;
using FanHai.Hemera.Utils.Entities;
using System.Data;
using System.Linq;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class Label_323 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_323(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrBarCode.LocationF = new PointF(xrBarCode.LocationF.X + data.X, xrBarCode.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);

            string s_value = string.Empty;
            string s_pktypename = Convert.ToString(data.PowersetSubPowerLevel ?? string.Empty);
            decimal dc_pm = decimal.Parse(data.PowersetStandardPM.ToString().Trim());
            IVTestDataEntity _testDataEntity = new IVTestDataEntity();
            DataSet ds = _testDataEntity.GetIVTestData(data.LotNo);
           

            if (!string.IsNullOrEmpty(_testDataEntity.ErrorMsg) || ds == null || ds.Tables[0].Rows.Count <= 0)
            {
                return ;
            }
            DataRow drLotInfo = ds.Tables[0].Rows[0];
            decimal coff = decimal.Parse(drLotInfo["COEF_PMAX"].ToString());

            //标签/包装清单打印体现功率 fyb
            try
            {
                string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", coff));
                if (drPowerShow.Count() > 0 && coff.ToString() == drPowerShow[0]["BEFORE_POWER"].ToString())
                {

                    coff = decimal.Parse(drPowerShow[0]["AFTER_POWER"].ToString());



                }
                ds_powershow = null;

                drPowerShow = null;
            }
            catch
            { }
            //

            if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
            {
                string[] strSplit = s_pktypename.Split('-');

                s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);
            }
            else
            {
                s_value = "Pm=" + coff.ToString("#,##0.00") + "Wp";
            }
            this.xrpm.Text = s_value;
            this.xrBarCode.Text = data.LotNo.ToString();
        }

    }
}

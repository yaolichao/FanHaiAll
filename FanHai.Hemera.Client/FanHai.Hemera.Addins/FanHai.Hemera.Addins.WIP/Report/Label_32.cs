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
    public partial class Label_32 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_32(PrintLabelParameterData data)
        {
            InitializeComponent();
            string s_value = string.Empty;
            string s_pktypename = Convert.ToString(data.PowersetSubPowerLevel ?? string.Empty);

            //标签/包装清单打印体现功率 fyb
            try
            {
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(data.LotNo);
                string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", data.CoefPM.ToString()));
                if (drPowerShow.Count() > 0 && data.PowersetStandardPM == drPowerShow[0]["BEFORE_POWER"].ToString())
                {

                    data.CoefPM = decimal.Parse(drPowerShow[0]["AFTER_POWER"].ToString());



                }
                ds_powershow = null;
                ds = null;
                drPowerShow = null;
            }
            catch
            { }
            //

            if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
            {
                s_value = "Pm=" + data.CoefPM.ToString("#,##0.00") + "Wp" + s_pktypename.Substring(s_pktypename.LastIndexOf('-'), 2);
            }
            else
            {
                s_value = "Pm=" + data.CoefPM.ToString("#,##0.00") + "Wp";
            }
            this.xrpm.Text = s_value;
            this.xrlotno.Text = data.LotNo.ToString();
            this.xrisc.Text = data.CoefISC.ToString("#,##0.00").Trim() + "A";
            this.xrimp.Text = data.CoefIPM.ToString("#,##0.00").Trim() + "A";
            this.xrvoc.Text = data.CoefVOC.ToString("#,##0.00").Trim() + "V";
            this.xrvmp.Text = data.CoefVPM.ToString("#,##0.00").Trim() + "V";
            this.xrBarCode.Text = data.LotNo.ToString();
            //this.xrTable3.LocationF = new PointF(this.xrTable3.LocationF.X + 20, this.xrTable3.LocationF.Y);
        }

    }
}

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
    public partial class Label_3221 : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_3221(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrlotno.LocationF = new PointF(xrlotno.LocationF.X + data.X, xrlotno.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);

            //标签/包装清单打印体现功率 fyb
            try
            {
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(data.LotNo);
                string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", data.PowersetStandardPM));
                if (drPowerShow.Count() > 0 && data.PowersetStandardPM == drPowerShow[0]["BEFORE_POWER"].ToString())
                {

                    data.PowersetStandardPM = drPowerShow[0]["AFTER_POWER"].ToString();
                }
                ds_powershow = null;

                drPowerShow = null;
            }
            catch
            { }
            //

            string s_value = string.Empty;
            string s_pktypename = Convert.ToString(data.PowersetSubPowerLevel ?? string.Empty);
            if (!string.IsNullOrEmpty(s_pktypename) && s_pktypename.Length >= 2 && s_pktypename.LastIndexOf('-') >= 0)
            {
                string[] strSplit = s_pktypename.Split('-');

                s_value = "Nominal Power=" + decimal.Parse(data.PowersetStandardPM).ToString("#,##0.00") + "Wp" + "-" + Convert.ToString(strSplit[strSplit.Length - 1]);
            }
            else
            {
                s_value = "Nominal Power=" + decimal.Parse(data.PowersetStandardPM).ToString("#,##0.00") + "Wp";
            }

            this.xrpm.Text = s_value;
            this.xrlotno.Text = data.LotNo.ToString();
        }

    }
}

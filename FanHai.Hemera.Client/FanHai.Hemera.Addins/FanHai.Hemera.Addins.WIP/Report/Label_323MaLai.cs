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
    public partial class Label_323MaLai : DevExpress.XtraReports.UI.XtraReport
    {
        public Label_323MaLai(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrBarCode.LocationF = new PointF(xrBarCode.LocationF.X + data.X, xrBarCode.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);

            string s_value = string.Empty;

            //标签/包装清单打印体现功率 fyb
            try
            {
                IVTestDataEntity _testDataEntity = new IVTestDataEntity();
                DataSet ds = _testDataEntity.GetIVTestData(data.LotNo);
                string strWorkNumber = ds.Tables[0].Rows[0]["WORK_ORDER_NO"].ToString();
                string strSAP_NO = ds.Tables[0].Rows[0]["PART_NUMBER"].ToString();
                DataSet ds_powershow = _testDataEntity.GetPowerShowData(strWorkNumber, strSAP_NO);
                DataRow[] drPowerShow = ds_powershow.Tables[0].Select(string.Format("BEFORE_POWER={0}", data.PartNumber));
                if (drPowerShow.Count() > 0 && data.PartNumber == drPowerShow[0]["BEFORE_POWER"].ToString())
                {

                    data.PartNumber = drPowerShow[0]["AFTER_POWER"].ToString();



                }
                ds_powershow = null;
                ds = null;
                drPowerShow = null;
            }
            catch
            { }
            //



            s_value = "Nominal Power=" + data.PartNumber.ToString();


            this.xrpm.Text = s_value;
            this.xrBarCode.Text = data.LotNo.ToString();
        }

    }
}

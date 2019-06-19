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
    public partial class NameplateLabel_543 : DevExpress.XtraReports.UI.XtraReport
    {
        public string moduleType, moduleWeight = string.Empty, moduleDimension = string.Empty;
        public NameplateLabel_543(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + data.X, xrPictureBox1.LocationF.Y + data.Y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + data.X, xrPictureBox2.LocationF.Y + data.Y);

            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + data.X, xrTable2.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);

            xrSilvantis.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            xrTable5.LocationF = new PointF(xrTable5.LocationF.X + data.X, xrTable5.LocationF.Y + data.Y);
            xrTable6.LocationF = new PointF(xrTable6.LocationF.X + data.X, xrTable6.LocationF.Y + data.Y);
            xrTable7.LocationF = new PointF(xrTable7.LocationF.X + data.X, xrTable7.LocationF.Y + data.Y);

            xrLine1.LocationF = new PointF(xrLine1.LocationF.X + data.X, xrLine1.LocationF.Y + data.Y);
            xrTable8.LocationF = new PointF(xrTable8.LocationF.X + data.X, xrTable8.LocationF.Y + data.Y);
            xrTable21.LocationF = new PointF(xrTable21.LocationF.X + data.X, xrTable21.LocationF.Y + data.Y);
            xrTable23.LocationF = new PointF(xrTable23.LocationF.X + data.X, xrTable23.LocationF.Y + data.Y);

            xrBarCode.LocationF = new PointF(xrBarCode.LocationF.X + data.X, xrBarCode.LocationF.Y + data.Y);

            xrTable9.LocationF = new PointF(xrTable9.LocationF.X + data.X, xrTable9.LocationF.Y + data.Y);
            xrTable10.LocationF = new PointF(xrTable10.LocationF.X + data.X, xrTable10.LocationF.Y + data.Y);
            xrTable11.LocationF = new PointF(xrTable11.LocationF.X + data.X, xrTable11.LocationF.Y + data.Y);
            xrTable12.LocationF = new PointF(xrTable12.LocationF.X + data.X, xrTable12.LocationF.Y + data.Y);
            xrTable13.LocationF = new PointF(xrTable13.LocationF.X + data.X, xrTable13.LocationF.Y + data.Y);
            xrTable14.LocationF = new PointF(xrTable14.LocationF.X + data.X, xrTable14.LocationF.Y + data.Y);
            xrTable15.LocationF = new PointF(xrTable15.LocationF.X + data.X, xrTable15.LocationF.Y + data.Y);
            xrTable4.LocationF = new PointF(xrTable4.LocationF.X + data.X, xrTable4.LocationF.Y + data.Y);
            xrBarCode2.LocationF = new PointF(xrBarCode2.LocationF.X + data.X, xrBarCode2.LocationF.Y + data.Y);
            xrLine2.LocationF = new PointF(xrLine2.LocationF.X + data.X, xrLine2.LocationF.Y + data.Y);
            xrLine4.LocationF = new PointF(xrLine4.LocationF.X + data.X, xrLine4.LocationF.Y + data.Y);
            xrPictureBox3.LocationF = new PointF(xrPictureBox3.LocationF.X + data.X, xrPictureBox3.LocationF.Y + data.Y);
            xrBarCode3.LocationF = new PointF(xrBarCode3.LocationF.X + data.X, xrBarCode3.LocationF.Y + data.Y);

            xrTable16.LocationF = new PointF(xrTable16.LocationF.X + data.X, xrTable16.LocationF.Y + data.Y);
            xrTable17.LocationF = new PointF(xrTable17.LocationF.X + data.X, xrTable17.LocationF.Y + data.Y);
            xrTable18.LocationF = new PointF(xrTable18.LocationF.X + data.X, xrTable18.LocationF.Y + data.Y);
            xrTable19.LocationF = new PointF(xrTable19.LocationF.X + data.X, xrTable19.LocationF.Y + data.Y);
            xrTable20.LocationF = new PointF(xrTable20.LocationF.X + data.X, xrTable20.LocationF.Y + data.Y);
            string s_value, productModule, partNumber;

            try
            {

                #region 获取托盘对应工单的OEM信息

                IVTestDataEntity IVTestDateObject = new IVTestDataEntity();


                DataSet dsOEMInfo = IVTestDateObject.GetWorkOrderOEMByOrderNumberOrLotNumber(string.Empty, data.LotNo);

                if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
                {
                    data.ErrorMessage = IVTestDateObject.ErrorMsg;
                    return ;
                }
                if (dsOEMInfo.Tables[0].Rows.Count == 0)
                {
                    data.ErrorMessage = string.Format(@"工单【{0}】未设置对用的OEM信息，
                                                        请联系工艺进行设定！", dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["ORDER_NUMBER"].ToString());
                    return ;
                }

                string cellType = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_TYPE"].ToString();

                productModule = dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CUSROMER"].ToString()
                                + "-"
                                + cellType
                                + data.PowersetStandardPM
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["STRUCTURE_PARAM"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["GLASS_TYPE"].ToString()
                                + "-"
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_AUTHENTICATION_CODE"].ToString()
                                + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["JUNCTION_BOX"].ToString();
                partNumber = partNumber = "M"
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_SUPPLIER"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["CELL_MODEL"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["SE_MODULE_TYPE"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["PLACE_ORIGIN"].ToString().ToUpper()
                        + dsOEMInfo.Tables["POR_WO_OEM"].Rows[0]["BOM_DESIGN"].ToString().ToUpper();

                #endregion

                this.xrSecelltype.Text = string.Format("{0}-Series PID Free", cellType);
                this.xrProductModule.Text = productModule;
                this.xrSn.Text = "SN: " + data.LotNo;
                s_value = data.LotNo.Substring(3, 6)
                          + " "
                          + data.CoefPM.ToString().Substring(0, 6)
                          + "W "
                          + data.CoefIPM.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVPM.ToString().Substring(0, 4)
                          + "V "
                          + data.CoefISC.ToString().Substring(0, 4)
                          + "A "
                          + data.CoefVOC.ToString().Substring(0, 4)
                          + "V";
                this.xrBarCode.Text = data.LotNo;
                this.xrBarCode2.Text = s_value;
                this.xrBarCodeText.Text = s_value;
                this.xrBarCode3.Text = partNumber;
                this.xrPmax.Text = data.PowersetStandardPM + "W";
                this.xrImpp.Text = data.PowersetStandardIPM.ToString("#,##0.00") + "A";
                this.xrVmpp.Text = data.PowersetStandardVPM.ToString("#,##00.0") + "V";
                this.xrIsc.Text = data.PowersetStandardISC.ToString("#,##0.00") + "A";
                this.xrVoc.Text = data.PowersetStandardVOC.ToString("#,##00.0") + "V";
                this.xrcode.Text = partNumber;
            }
            catch(Exception ex)
            {}
           
        }
        

    }
}

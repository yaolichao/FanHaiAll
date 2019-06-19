using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;
using FanHai.Hemera.Utils.Common;
using System.Data;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_56 : DevExpress.XtraReports.UI.XtraReport
    {
        public string moduleType, moduleWeight = string.Empty, moduleDimension = string.Empty;
        public NameplateLabel_56(PrintLabelParameterData data)
        {
            InitializeComponent();
            xrTable1.LocationF = new PointF(xrTable1.LocationF.X + data.X, xrTable1.LocationF.Y + data.Y);
            xrTable2.LocationF = new PointF(xrTable2.LocationF.X + data.X, xrTable2.LocationF.Y + data.Y);
            xrTable3.LocationF = new PointF(xrTable3.LocationF.X + data.X, xrTable3.LocationF.Y + data.Y);
            xrTable4.LocationF = new PointF(xrTable4.LocationF.X + data.X, xrTable4.LocationF.Y + data.Y);
            xrTable5.LocationF = new PointF(xrTable5.LocationF.X + data.X, xrTable5.LocationF.Y + data.Y);
            xrTable6.LocationF = new PointF(xrTable6.LocationF.X + data.X, xrTable6.LocationF.Y + data.Y);
            xrTable7.LocationF = new PointF(xrTable7.LocationF.X + data.X, xrTable7.LocationF.Y + data.Y);
            xrTable8.LocationF = new PointF(xrTable8.LocationF.X + data.X, xrTable8.LocationF.Y + data.Y);
            xrTable9.LocationF = new PointF(xrTable9.LocationF.X + data.X, xrTable9.LocationF.Y + data.Y);
            xrTable10.LocationF = new PointF(xrTable10.LocationF.X + data.X, xrTable10.LocationF.Y + data.Y);
            xrTable11.LocationF = new PointF(xrTable11.LocationF.X + data.X, xrTable11.LocationF.Y + data.Y);
            xrTable12.LocationF = new PointF(xrTable12.LocationF.X + data.X, xrTable12.LocationF.Y + data.Y);
            xrTable13.LocationF = new PointF(xrTable13.LocationF.X + data.X, xrTable13.LocationF.Y + data.Y);
            xrTable14.LocationF = new PointF(xrTable14.LocationF.X + data.X, xrTable14.LocationF.Y + data.Y);
            xrTable15.LocationF = new PointF(xrTable15.LocationF.X + data.X, xrTable15.LocationF.Y + data.Y);
            xrTable16.LocationF = new PointF(xrTable16.LocationF.X + data.X, xrTable16.LocationF.Y + data.Y);
            xrTable17.LocationF = new PointF(xrTable17.LocationF.X + data.X, xrTable17.LocationF.Y + data.Y);
            xrTable18.LocationF = new PointF(xrTable18.LocationF.X + data.X, xrTable18.LocationF.Y + data.Y);
            xrTable19.LocationF = new PointF(xrTable19.LocationF.X + data.X, xrTable19.LocationF.Y + data.Y);
            xrTable20.LocationF = new PointF(xrTable20.LocationF.X + data.X, xrTable20.LocationF.Y + data.Y);
            xrTable21.LocationF = new PointF(xrTable21.LocationF.X + data.X, xrTable21.LocationF.Y + data.Y);
            xrTable22.LocationF = new PointF(xrTable22.LocationF.X + data.X, xrTable22.LocationF.Y + data.Y);

            xrPictureBox1.LocationF = new PointF(xrPictureBox1.LocationF.X + data.X, xrPictureBox1.LocationF.Y + data.Y);
            xrPictureBox3.LocationF = new PointF(xrPictureBox3.LocationF.X + data.X, xrPictureBox3.LocationF.Y + data.Y);
            xrPictureBox4.LocationF = new PointF(xrPictureBox4.LocationF.X + data.X, xrPictureBox4.LocationF.Y + data.Y);
            xrPictureBox5.LocationF = new PointF(xrPictureBox5.LocationF.X + data.X, xrPictureBox5.LocationF.Y + data.Y);
            xrPictureBox2.LocationF = new PointF(xrPictureBox2.LocationF.X + data.X, xrPictureBox2.LocationF.Y + data.Y);
            xrBarLotNo.LocationF = new PointF(xrBarLotNo.LocationF.X + data.X, xrBarLotNo.LocationF.Y + data.Y);

            #region 产品和边框、重量的对应关系
            string[] columns = new string[] { "ProductModule", "FullPalletQty", "ModuleWeight", "ModuleDimension" };
            DataTable dtModuleDismensionWeight = BaseData.Get(columns, "Basic_ModuleDimensionWeight");

            DataRow[] drs = dtModuleDismensionWeight.Select(string.Format("  ProductModule ='{0}' AND   FullPalletQty = '{1}'", data.ProductModel.Substring(0, 4), data.FullPalletQty));

            if (drs.Length == 1)
            {
                foreach (DataRow dr in drs)
                {
                    moduleWeight = Convert.ToString(dr["ModuleWeight"]);
                    moduleDimension = Convert.ToString(dr["ModuleDimension"]);
                }
            }
            else
            {
                data.ErrorMessage = data.ErrorMessage + string.Format("请工艺确认工单【{0}】中产品【{1}】的满托数是否正确！", data.WorkOrderNumber, data.ProductCode);
                return ;
            }
            #endregion
            #region ID56 ModuleType 获取与定义
            moduleType = "JS"
                       + "-"
                       + data.PowersetStandardPM;
            if (data.ProductModel.Substring(4, 1) == "P")
            {
                moduleType = moduleType + "U";
            }
            else
            {
                moduleType = moduleType + "M";
            }

            moduleType = moduleType
                       + "-"
                       + "OI"
                       + data.LotCellQty;
            #endregion
            this.xrModuleType.Text = moduleType.ToString().Trim();
            this.xrpm.Text = data.PowersetStandardPM + " W";
            this.xrVoc.Text = data.PowersetStandardVOC + " V";
            this.xrIsc.Text = data.PowersetStandardISC + " A";
            this.xrImp.Text = data.PowersetStandardIPM + " A";
            this.xrVmp.Text = data.PowersetStandardVPM + " V";
            this.xrLotNo.Text = data.LotNo.ToString().Trim();
            this.xrBarLotNo.Text = data.LotNo.ToString().Trim();
            this.xrModuleWeight.Text = moduleWeight.ToString().Trim() + " Kg";
        }

    }
}

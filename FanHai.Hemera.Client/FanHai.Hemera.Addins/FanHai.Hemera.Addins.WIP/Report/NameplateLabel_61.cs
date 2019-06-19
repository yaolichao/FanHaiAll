using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FanHai.Hemera.Utils.Barcode;
using System.Data;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.WIP.Report
{
    public partial class NameplateLabel_61 : DevExpress.XtraReports.UI.XtraReport
    {
        public NameplateLabel_61()
        {
            InitializeComponent();
        }

        public NameplateLabel_61(PrintLabelParameterData data)
        {
            InitializeComponent();

            //调整页面布局
            MovePoint(data.X, data.Y);


            //获取组件批次号
            xbCodeLotNumber.Text = data.LotNo;
            //获取组件效率档
            string strPowerSet = string.Format("{0}W", data.PowersetStandardPM);
            //获取组件电池片数目
            string strCellQty = data.LotCellQty.ToString();

            //设定产品型号
            xrtCellContentValue01.Text = string.Format("SF156×156-{0}-P {1}", strCellQty, strPowerSet);

            //获取赞皇对应的标称关系
            string[] powerSetColumns = new string[] { "PowerSetName", "StandardPmax", "StandardImp", "StandardVmp", "StandardIsc", "StandardVoc" };
            DataTable dtPowerSet = BaseData.Get(powerSetColumns, "Basic_ZanHuang_PowerSetMapping");

            //按照效率档获取对应的赞皇的标称
            DataRow[] drsPowerSet = dtPowerSet.Select(string.Format("  PowerSetName ='{0}'", strPowerSet));

            //判断是否存在有效行
            if (drsPowerSet.Length > 0)
            {
                //设定标称功率信息
                xrtCellContentValue02.Text = string.Format("{0} W（0/+3%）", Convert.ToString(drsPowerSet[0]["StandardPmax"]));
                //设定标称电流信息
                xrtCellContentValue03.Text = Convert.ToString(drsPowerSet[0]["StandardImp"]);
                //设定标称电压信息
                xrtCellContentValue04.Text = Convert.ToString(drsPowerSet[0]["StandardVmp"]);
                //设定标称短路电流信息
                xrtCellContentValue05.Text = Convert.ToString(drsPowerSet[0]["StandardIsc"]);
                //设定标称开路电压信息
                xrtCellContentValue06.Text = Convert.ToString(drsPowerSet[0]["StandardVoc"]);
            }
            else
            {
                //设定标称功率信息
                xrtCellContentValue02.Text = string.Format("{0} W（0/+3%）", "Null");
                //设定标称电流信息
                xrtCellContentValue03.Text = "Null";
                //设定标称电压信息
                xrtCellContentValue04.Text = "Null";
                //设定标称短路电流信息
                xrtCellContentValue05.Text = "Null";
                //设定标称开路电压信息
                xrtCellContentValue06.Text = "Null";
            }

            //获取子分档信息
            string strSubPower = string.Empty;
            string strSubPowerLevel = data.PowersetSubPowerLevel;

            string[] strArry = strSubPowerLevel.Split('-');
            //判断成员数目
            if (strArry.Length > 0)
            {
                strSubPower = string.Format("{0}-{1}", strPowerSet, strArry[strArry.Length - 1]);
            }

            //设定组件对应电流分档
            string[] IscSetColumns = new string[] { "IscSet", "TrueValue", "StandardValue" };
            DataTable dtIscSet = BaseData.Get(IscSetColumns, "Basic_ZanHuang_IscMapping");

            //按照子分档获取子分档信息
            DataRow[] drsIscSet = dtIscSet.Select(string.Format("  IscSet ='{0}'", strSubPower));

            //判断是否存在匹配信息
            if (drsIscSet.Length > 0)
            {
                //设定子分档区间
                xrtCellContentValue10.Text = Convert.ToString(drsIscSet[0]["StandardValue"]);
            }
            else
            {
                //设定子分档区间
                xrtCellContentValue10.Text = "Null";
            }
        }

        /// <summary>
        /// 控件位置调整
        /// </summary>
        /// <param name="x">横向偏移</param>
        /// <param name="y">纵向偏移</param>
        private void MovePoint(int x, int y)
        {
            //移动台头Logo的位置
            xrPicHead01.LocationF = new DevExpress.Utils.PointFloat(xrPicHead01.LocationFloat.X + x, xrPicHead01.LocationFloat.Y + y);
            //移动台头条码的位置
            xbCodeLotNumber.LocationF = new DevExpress.Utils.PointFloat(xbCodeLotNumber.LocationFloat.X + x, xbCodeLotNumber.LocationFloat.Y + y);
            //移动Table内容
            xrTableContentMain.LocationF = new DevExpress.Utils.PointFloat(xrTableContentMain.LocationFloat.X + x, xrTableContentMain.LocationFloat.Y + y);
            //移动中间横线
            xrLineMiddle01.LocationF = new DevExpress.Utils.PointFloat(xrLineMiddle01.LocationFloat.X + x, xrLineMiddle01.LocationFloat.Y + y);
            //移动图标位置
            xrPicMiddle01.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle01.LocationFloat.X + x, xrPicMiddle01.LocationFloat.Y + y);
            xrPicMiddle02.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle02.LocationFloat.X + x, xrPicMiddle02.LocationFloat.Y + y);
            xrPicMiddle03.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle03.LocationFloat.X + x, xrPicMiddle03.LocationFloat.Y + y);
            //移动底部文字的位置
            xrTableBottom01.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom01.LocationFloat.X + x, xrTableBottom01.LocationFloat.Y + y);
            xrTableBottom02.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom02.LocationFloat.X + x, xrTableBottom02.LocationFloat.Y + y);
            xrTableBottom03.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom03.LocationFloat.X + x, xrTableBottom03.LocationFloat.Y + y);
            //移动底部图标位置
            xrPicBottom01.LocationF = new DevExpress.Utils.PointFloat(xrPicBottom01.LocationFloat.X + x, xrPicBottom01.LocationFloat.Y + y);
            xrPicBottom02.LocationF = new DevExpress.Utils.PointFloat(xrPicBottom02.LocationFloat.X + x, xrPicBottom02.LocationFloat.Y + y);
        }
    }
}

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

            //����ҳ�沼��
            MovePoint(data.X, data.Y);


            //��ȡ������κ�
            xbCodeLotNumber.Text = data.LotNo;
            //��ȡ���Ч�ʵ�
            string strPowerSet = string.Format("{0}W", data.PowersetStandardPM);
            //��ȡ������Ƭ��Ŀ
            string strCellQty = data.LotCellQty.ToString();

            //�趨��Ʒ�ͺ�
            xrtCellContentValue01.Text = string.Format("SF156��156-{0}-P {1}", strCellQty, strPowerSet);

            //��ȡ�޻ʶ�Ӧ�ı�ƹ�ϵ
            string[] powerSetColumns = new string[] { "PowerSetName", "StandardPmax", "StandardImp", "StandardVmp", "StandardIsc", "StandardVoc" };
            DataTable dtPowerSet = BaseData.Get(powerSetColumns, "Basic_ZanHuang_PowerSetMapping");

            //����Ч�ʵ���ȡ��Ӧ���޻ʵı��
            DataRow[] drsPowerSet = dtPowerSet.Select(string.Format("  PowerSetName ='{0}'", strPowerSet));

            //�ж��Ƿ������Ч��
            if (drsPowerSet.Length > 0)
            {
                //�趨��ƹ�����Ϣ
                xrtCellContentValue02.Text = string.Format("{0} W��0/+3%��", Convert.ToString(drsPowerSet[0]["StandardPmax"]));
                //�趨��Ƶ�����Ϣ
                xrtCellContentValue03.Text = Convert.ToString(drsPowerSet[0]["StandardImp"]);
                //�趨��Ƶ�ѹ��Ϣ
                xrtCellContentValue04.Text = Convert.ToString(drsPowerSet[0]["StandardVmp"]);
                //�趨��ƶ�·������Ϣ
                xrtCellContentValue05.Text = Convert.ToString(drsPowerSet[0]["StandardIsc"]);
                //�趨��ƿ�·��ѹ��Ϣ
                xrtCellContentValue06.Text = Convert.ToString(drsPowerSet[0]["StandardVoc"]);
            }
            else
            {
                //�趨��ƹ�����Ϣ
                xrtCellContentValue02.Text = string.Format("{0} W��0/+3%��", "Null");
                //�趨��Ƶ�����Ϣ
                xrtCellContentValue03.Text = "Null";
                //�趨��Ƶ�ѹ��Ϣ
                xrtCellContentValue04.Text = "Null";
                //�趨��ƶ�·������Ϣ
                xrtCellContentValue05.Text = "Null";
                //�趨��ƿ�·��ѹ��Ϣ
                xrtCellContentValue06.Text = "Null";
            }

            //��ȡ�ӷֵ���Ϣ
            string strSubPower = string.Empty;
            string strSubPowerLevel = data.PowersetSubPowerLevel;

            string[] strArry = strSubPowerLevel.Split('-');
            //�жϳ�Ա��Ŀ
            if (strArry.Length > 0)
            {
                strSubPower = string.Format("{0}-{1}", strPowerSet, strArry[strArry.Length - 1]);
            }

            //�趨�����Ӧ�����ֵ�
            string[] IscSetColumns = new string[] { "IscSet", "TrueValue", "StandardValue" };
            DataTable dtIscSet = BaseData.Get(IscSetColumns, "Basic_ZanHuang_IscMapping");

            //�����ӷֵ���ȡ�ӷֵ���Ϣ
            DataRow[] drsIscSet = dtIscSet.Select(string.Format("  IscSet ='{0}'", strSubPower));

            //�ж��Ƿ����ƥ����Ϣ
            if (drsIscSet.Length > 0)
            {
                //�趨�ӷֵ�����
                xrtCellContentValue10.Text = Convert.ToString(drsIscSet[0]["StandardValue"]);
            }
            else
            {
                //�趨�ӷֵ�����
                xrtCellContentValue10.Text = "Null";
            }
        }

        /// <summary>
        /// �ؼ�λ�õ���
        /// </summary>
        /// <param name="x">����ƫ��</param>
        /// <param name="y">����ƫ��</param>
        private void MovePoint(int x, int y)
        {
            //�ƶ�̨ͷLogo��λ��
            xrPicHead01.LocationF = new DevExpress.Utils.PointFloat(xrPicHead01.LocationFloat.X + x, xrPicHead01.LocationFloat.Y + y);
            //�ƶ�̨ͷ�����λ��
            xbCodeLotNumber.LocationF = new DevExpress.Utils.PointFloat(xbCodeLotNumber.LocationFloat.X + x, xbCodeLotNumber.LocationFloat.Y + y);
            //�ƶ�Table����
            xrTableContentMain.LocationF = new DevExpress.Utils.PointFloat(xrTableContentMain.LocationFloat.X + x, xrTableContentMain.LocationFloat.Y + y);
            //�ƶ��м����
            xrLineMiddle01.LocationF = new DevExpress.Utils.PointFloat(xrLineMiddle01.LocationFloat.X + x, xrLineMiddle01.LocationFloat.Y + y);
            //�ƶ�ͼ��λ��
            xrPicMiddle01.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle01.LocationFloat.X + x, xrPicMiddle01.LocationFloat.Y + y);
            xrPicMiddle02.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle02.LocationFloat.X + x, xrPicMiddle02.LocationFloat.Y + y);
            xrPicMiddle03.LocationF = new DevExpress.Utils.PointFloat(xrPicMiddle03.LocationFloat.X + x, xrPicMiddle03.LocationFloat.Y + y);
            //�ƶ��ײ����ֵ�λ��
            xrTableBottom01.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom01.LocationFloat.X + x, xrTableBottom01.LocationFloat.Y + y);
            xrTableBottom02.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom02.LocationFloat.X + x, xrTableBottom02.LocationFloat.Y + y);
            xrTableBottom03.LocationF = new DevExpress.Utils.PointFloat(xrTableBottom03.LocationFloat.X + x, xrTableBottom03.LocationFloat.Y + y);
            //�ƶ��ײ�ͼ��λ��
            xrPicBottom01.LocationF = new DevExpress.Utils.PointFloat(xrPicBottom01.LocationFloat.X + x, xrPicBottom01.LocationFloat.Y + y);
            xrPicBottom02.LocationF = new DevExpress.Utils.PointFloat(xrPicBottom02.LocationFloat.X + x, xrPicBottom02.LocationFloat.Y + y);
        }
    }
}

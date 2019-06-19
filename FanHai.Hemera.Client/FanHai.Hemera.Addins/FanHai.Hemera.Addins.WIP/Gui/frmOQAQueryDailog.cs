using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.WIP.Gui
{
    public partial class frmOQAQueryDailog : BaseDialog
    {
        public frmOQAQueryDailog()
        {
            InitializeComponent();
        }

        private void frmOQAQueryDailog_Load(object sender, EventArgs e)
        {
            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            DataSet dsFactory = IVTestDateObject.GetFactoryInfo();
            lueFactory.Properties.DataSource = null;
            lueFactory.Properties.DataSource = dsFactory.Tables[0];
            lueFactory.EditValue = "ALL";
            deStartDate.DateTime = DateTime.Now;
            deEndDate.DateTime = DateTime.Now;
            try
            {
                string sStartDate, sEndDate;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("PrintConfig.xml");
                sStartDate = xmlDoc.SelectSingleNode("//UI/OQA_START_DATE").InnerText;
                sEndDate = xmlDoc.SelectSingleNode("//UI/OQA_END_DATE").InnerText;
                if (!string.IsNullOrEmpty(sStartDate))
                {
                    deStartDate.DateTime = Convert.ToDateTime(sStartDate);
                }
                if (!string.IsNullOrEmpty(sEndDate))
                {
                    deEndDate.DateTime = Convert.ToDateTime(sEndDate);
                }
            }
            catch
            { 
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OQAQueryCondition.FactoryKey = Convert.ToString(lueFactory.EditValue);
            if (radioGroup1.EditValue.ToString().Trim() == "C")
            {
                OQAQueryCondition.SNType = "C";
            }
            else
            {
                OQAQueryCondition.SNType = "F";
            }
            if (chDefult.Checked == true)
            {
               OQAQueryCondition.Default = "T";
            }
            else
            {
                OQAQueryCondition.Default = "F";
            }
            if (chData.Checked == true)
            {
                OQAQueryCondition.DateFalg = "T";
            }
            else
            {
                OQAQueryCondition.DateFalg = "F";
            }
            OQAQueryCondition.StartSN = txtStartSN.Text.Trim();
            OQAQueryCondition.EndSN = txtEndSN.Text.Trim();
            OQAQueryCondition.WO = txtWO.Text.Trim();
            OQAQueryCondition.PROID = txtProID.Text.Trim();
            if (chData.Checked == true)
            {
                if (deStartDate.DateTime > deEndDate.DateTime)
                {
                    MessageService.ShowMessage("开始时间不能大于结束时间，请确认！","提示");
                    return;
                }
                else
                {
                    OQAQueryCondition.StartDate = deStartDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    OQAQueryCondition.EndDate = deEndDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("PrintConfig.xml");
                xmlDoc.SelectSingleNode("//UI/OQA_START_DATE").InnerText = deStartDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                xmlDoc.SelectSingleNode("//UI/OQA_END_DATE").InnerText = deEndDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                xmlDoc.Save("PrintConfig.xml");
            }
            catch//(Exception ex)
            {
                File.Delete("PrintConfig.xml");
                StreamWriter sw = new StreamWriter("PrintConfig.xml");
                string sParam;
                sParam = "<UI>";
                sParam += "<AUTO_PRINT>F</AUTO_PRINT>";
                sParam += "<L_AUTO_PRINT>F</L_AUTO_PRINT>";
                sParam += "<P_AUTO_PRINT>F</P_AUTO_PRINT>";
                sParam += "<L_LABEL_ID></L_LABEL_ID>";
                sParam += "<P_LABEL_ID></P_LABEL_ID>";
                sParam += "<L_X>0</L_X>";
                sParam += "<L_Y>0</L_Y>";
                sParam += "<P_X>0</P_X>";
                sParam += "<P_Y>0</P_Y>";
                sParam += "<L_DARKNESS>12</L_DARKNESS>";
                sParam += "<P_DARKNESS>12</P_DARKNESS>";
                sParam += "<L_300DPI>F</L_300DPI>";
                sParam += "<P_300DPI>F</P_300DPI>";
                sParam += "<OQA_START_DATE></OQA_START_DATE>";
                sParam += "<OQA_END_DATE></OQA_END_DATE>";
                sParam += "</UI>";
                sw.Write(sParam);
                sw.Close();
            }
            XmlDocument xmlNew = new XmlDocument();
            xmlNew.Load("PrintConfig.xml");
            xmlNew.SelectSingleNode("//UI/OQA_START_DATE").InnerText = deStartDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            xmlNew.SelectSingleNode("//UI/OQA_END_DATE").InnerText = deEndDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            xmlNew.Save("PrintConfig.xml");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

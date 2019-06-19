using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Addins.WIP.Gui;
using Microsoft.Office.Interop.Excel;
using FanHai.Hemera.Utils.Controls.Common;
using System.Xml;
using System.IO;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotOutQuantityAssurance : BaseUserCtrl
    {
        private DataSet dsOQA = new DataSet();
        private DataSet dsQueryOQA = new DataSet();

        public LotOutQuantityAssurance()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gvOQAData);
        }


        private void InitializeLanguage()
        {
            this.tsbQuery.Text = StringParser.Parse("${res:Global.Query}");//查询
            this.LOCATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.LOCATION_NAME}");// "厂别";
            this.CC_FCODE1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.CC_FCODE1}");// "组件序号";
            this.CUSTOMCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.CUSTOMCODE}");// "客户序号";
            this.CHECK_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.CHECK_TIME}");// "检验日期";
            this.WORKNUMBER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.WORKNUMBER}");// "工单号";
            this.PRO_ID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.PRO_ID}");// "产品ID";
            this.LOT_COLOR.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.LOT_COLOR}");// "花色";
            this.COLUMN_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.COLUMN_NAME}");// "等级";
            this.SHIFT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.SHIFT_NAME}");// "作业站";
            this.REASON_CODE_CATEGORY_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.REASON_CODE_CATEGORY_NAME}");// "不良类别";
            this.REASON_CODE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.REASON_CODE_NAME}");// "不良代码";
            this.CREATER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.CREATER}");// "抽检人员";
            this.REMARK.Caption = StringParser.Parse("${res:Global.Remark}");//"备注";
            this.chCustomCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.chCustomCode}");// "检验客户编码";
            this.btnQuery.Text = StringParser.Parse("${res:Global.Save}");//保存
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layoutControlItem8}");// "组件序列号";
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layoutControlItem2}");// "不良代码";
            this.layout_factory.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layout_factory}");// "工厂车间";
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layoutControlItem3}");// "不良代码类别";
            this.layoutControlItem6.Text = StringParser.Parse("${res:Global.Remark}");//"备注";
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layoutControlItem9}");// "检验客户编码";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.layoutControlItem5}");// "组件等级";
            this.lblMenu.Text = "质量管理>质量作业>质量抽检";

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





        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sSN, sRCodeName, sRCodeKey, sRCategoryCode, sRCategoryKey, sRoomKey, sLevelCode, sLevelName, sRemark;
            string sFactoryV, sFccode2V, sCustomCodeV, sCheckTimeV, sWorkNumV, sProIDV, sLotColorV, sGradCodeV, sGradV, sWorkStationV, sShiftKey;
            sSN = "";
            sRCodeName = "";
            sRCodeKey = "";
            sRCategoryCode = "";
            sRCategoryKey = "";
            sRoomKey = "";
            sLevelCode = "";
            sLevelName = "";
            sRemark = "";
            sFactoryV = "";
            sFccode2V = "";
            sCustomCodeV = "";
            sCheckTimeV = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            sWorkNumV = "";
            sProIDV = "";
            sLotColorV = "";
            sGradV = "";
            sWorkStationV = "";
            sGradCodeV = "";
            sShiftKey = "";
            sRoomKey = lueFactoryRoom.EditValue.ToString();
            sSN = txtLotNumber.Text.Trim();
            sRCategoryCode = lueRCategory.Text.Trim();
            if (lueRCategory.EditValue != null)
            {
                sRCategoryKey = lueRCategory.EditValue.ToString();
            }
            sRCodeName = lue_NG_Code.Text.Trim();
            if (lue_NG_Code.EditValue != null)
            {
                sRCodeKey = lue_NG_Code.EditValue.ToString();
            }
            if (lueLevel.EditValue != null)
            {
                sLevelCode = lueLevel.EditValue.ToString();
            }
            sLevelName = lueLevel.Text.Trim();
            sRemark = cobRemark.Text.Trim();

            if (string.IsNullOrEmpty(sRoomKey))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg001}"));//厂别未选择，请确认！
                //MessageService.ShowMessage("厂别未选择，请确认！");
                return;
            }
            if (string.IsNullOrEmpty(sSN))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg002}"));//组件序号为空，请确认！
                //MessageService.ShowMessage("组件序号为空，请确认！");
                return;
            }

            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            DataSet dsCustCheck = new DataSet();
            if (chCustomCode.Checked == true)
            {
                dsCustCheck = IVTestDateObject.GetCustCheckData("", sSN, sRoomKey);
            }
            else
            {
                dsCustCheck = IVTestDateObject.GetCustCheckData(sSN, "", sRoomKey);
            }
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsCustCheck.Tables[0].Rows.Count < 1)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg003}"));//组件未过终检，不能抽检，请确认！
                    //MessageService.ShowMessage("组件未过终检，不能抽检，请确认！");
                    return;
                }
                else
                {
                    sFactoryV = lueFactoryRoom.Text.Trim();
                    sFccode2V = dsCustCheck.Tables[0].Rows[0]["CC_FCODE2"].ToString();
                    sCustomCodeV = dsCustCheck.Tables[0].Rows[0]["CUSTOMCODE"].ToString();
                    sWorkNumV = dsCustCheck.Tables[0].Rows[0]["WORKNUMBER"].ToString();
                    sProIDV = dsCustCheck.Tables[0].Rows[0]["PRO_ID"].ToString();
                    sLotColorV = dsCustCheck.Tables[0].Rows[0]["LOT_COLOR"].ToString();
                    sGradCodeV = dsCustCheck.Tables[0].Rows[0]["PRO_LEVEL"].ToString();
                    sShiftKey = dsCustCheck.Tables[0].Rows[0]["SHIFT_KEY"].ToString();
                    sWorkStationV = dsCustCheck.Tables[0].Rows[0]["SHIFT_NAME"].ToString();
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }
            DataSet dsGraed = IVTestDateObject.GetBasicData("ProductGrade", sGradCodeV, "", "");
            sGradV = dsGraed.Tables[0].Rows[1]["COLUMN_NAME"].ToString();

            DataSet dsConsigmentData = new DataSet();
            if (chCustomCode.Checked == true)
            {
                dsConsigmentData = IVTestDateObject.GetConsigmentDataBySN("", "", sSN, sRoomKey);
            }
            else
            {
                dsConsigmentData = IVTestDateObject.GetConsigmentDataBySN(sSN, "", "", sRoomKey);
            }
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsConsigmentData.Tables[0].Rows.Count > 0)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg004}"));//组件已包装，不能抽检，请确认！
                    //MessageService.ShowMessage("组件已包装，不能抽检，请确认！");
                    return;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }

            string sql = string.Empty;
            sql = "UPDATE WIP_CUSTCHECK SET ISFLAG='0'";
            sql += " WHERE ISFLAG='1' AND CC_DATA_GROUP='O'";
            sql += " AND ROOM_KEY='" + sRoomKey + "'";
            if (chCustomCode.Checked == true)
            {
                sql += " AND CUSTOMCODE='" + sSN + "'";
            }
            else
            {
                sql += " AND CC_FCODE1='" + sSN + "'";
            }
            DataSet dsUOQA = IVTestDateObject.UpdateData(sql, "UpdateOQA");
            if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }

            DataSet dsSEQ = new DataSet();
            if (chCustomCode.Checked == true)
            {
                dsSEQ = IVTestDateObject.GetCustCheckSEQ("", sSN, sRoomKey);
            }
            else
            {
                dsSEQ = IVTestDateObject.GetCustCheckSEQ(sSN, "", sRoomKey);
            }
            if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }
            int nSEQ = int.Parse(dsSEQ.Tables[0].Rows[0]["SEQ"].ToString());

            string sUID = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string key = FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
            sql = "INSERT INTO WIP_CUSTCHECK(CUSTCHECK_KEY,CC_DATA_GROUP,CUSTOMCODE,l_ID,CHECK_TIME,CC_FCODE1,WORKNUMBER,PRO_ID,LOT_COLOR,PRO_LEVEL,CREATER,CREATE_TIME,ISFLAG,ROOM_KEY,SHIFT_KEY,SHIFT_NAME,REMARK,REASON_CODE_NAME,REASON_CODE_KEY,REASON_CODE_CATEGORY_NAME,REASON_CODE_CATEGORY_KEY)";
            sql += " VALUES('" + key + "','O','" + sCustomCodeV + "','" + nSEQ.ToString() + "',GETDATE(),'" + sFccode2V + "','" + sWorkNumV + "'";
            sql += ",'" + sProIDV + "','" + sLotColorV + "','" + sGradCodeV + "','" + sUID + "',GETDATE(),'1','" + sRoomKey + "','" + sShiftKey + "'";
            sql += ",'" + sWorkStationV + "','" + sRemark + "','" + sRCodeName + "','" + sRCategoryKey + "','" + sRCategoryCode + "','" + sRCategoryKey + "')";
            DataSet dsAddOQA = IVTestDateObject.AddData(sql, "AddOQA");
            if (!string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                return;
            }
            int nAddOQA = int.Parse(dsAddOQA.ExtendedProperties["rows"].ToString());
            if (nAddOQA < 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg005}"));//添加OQA数据失败！
                //MessageService.ShowMessage("添加OQA数据失败！", "提示");
                return;
            }

            txtLotNumber.SelectAll();
            txtLotNumber.Focus();

            dsOQA.Tables[0].Rows.Add(sFactoryV, sFccode2V, sCustomCodeV, sCheckTimeV, sWorkNumV, sProIDV, sLotColorV, sGradV, sWorkStationV, sRCategoryCode, sRCodeName, sUID, sRemark);
            gcOQAData.DataSource = null;
            gcOQAData.DataSource = dsOQA.Tables[0];
            lblTot.Text = "TOT:" + dsOQA.Tables[0].Rows.Count.ToString();

            //清除查询数据集
            dsQueryOQA.Clear();
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            System.Data.DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME"));
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }

        private void txtLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    string sSN = string.Empty, sRoomKey = string.Empty;
            //    sSN = txtLotNumber.Text.Trim();
            //    sRoomKey = lueFactoryRoom.EditValue.ToString();
            //    if (string.IsNullOrEmpty(sSN))
            //    {
            //        MessageService.ShowMessage("组件序号为空，请确认！");
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(sRoomKey))
            //    {
            //        MessageService.ShowMessage("厂别未选，请确认！");
            //        return;
            //    }
            //    if (chCustomCode.Checked == true)
            //    {
            //        BindOQAData("", sSN, sRoomKey);
            //    }
            //    else
            //    {
            //        BindOQAData(sSN, "", sRoomKey);
            //    }
            //    btnQuery.Focus();
            //}
            if (e.KeyChar == 13)
            {
                btnQuery.Focus();
            }
        }

        private void LotOutQuantityAssurance_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindReasonCategory();
            BindLevel();

            System.Data.DataTable dtOQAData = new System.Data.DataTable();
            dtOQAData.Columns.Add("LOCATION_NAME");
            dtOQAData.Columns.Add("CC_FCODE1");
            dtOQAData.Columns.Add("CUSTOMCODE");
            dtOQAData.Columns.Add("CHECK_TIME");
            dtOQAData.Columns.Add("WORKNUMBER");
            dtOQAData.Columns.Add("PRO_ID");
            dtOQAData.Columns.Add("LOT_COLOR");
            dtOQAData.Columns.Add("COLUMN_NAME");
            dtOQAData.Columns.Add("SHIFT_NAME");
            dtOQAData.Columns.Add("REASON_CODE_CATEGORY_NAME");
            dtOQAData.Columns.Add("REASON_CODE_NAME");
            dtOQAData.Columns.Add("CREATER");
            dtOQAData.Columns.Add("REMARK");
            dsOQA.Tables.Add(dtOQAData);
            gcOQAData.DataSource = null;
            gcOQAData.MainView = gvOQAData;
            gcOQAData.DataSource = dsOQA.Tables[0];
            lblTot.Text = "TOT:0";
        }

        private void BindReasonCategory()
        {
            this.lueRCategory.EditValue = string.Empty;
            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            DataSet dsRCategory = IVTestDateObject.GetReasonCategoryData("D", "不良_OQA");
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsRCategory != null)
                {
                    this.lueRCategory.Properties.DataSource = null;
                    dsRCategory.Tables[0].Rows.Add("", "");
                    this.lueRCategory.Properties.DataSource = dsRCategory.Tables[0];
                    this.lueRCategory.Properties.DisplayMember = "REASON_CODE_CATEGORY_NAME";
                    this.lueRCategory.Properties.ValueMember = "REASON_CODE_CATEGORY_KEY";
                    this.lueRCategory.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("REASON_CODE_CATEGORY_NAME"));
                }
                else
                {
                    this.lueRCategory.Properties.DataSource = null;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                this.lueRCategory.Properties.DataSource = null;
                return;
            }
        }

        private void BindReasonCode(string sCategoryKey)
        {
            this.lue_NG_Code.EditValue = string.Empty;
            this.lue_NG_Code.Properties.DataSource = null;
            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            DataSet dsReasonCode = IVTestDateObject.GetReasonData(sCategoryKey);
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsReasonCode != null)
                {
                    this.lue_NG_Code.Properties.DataSource = dsReasonCode.Tables[0];
                    //this.lue_NG_Code.Properties.DisplayMember = "REASON_CODE_NAME";
                    //this.lue_NG_Code.Properties.ValueMember = "REASON_CODE_KEY";
                    //this.lue_NG_Code.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("REASON_CODE_NAME"));
                }
                else
                {
                    this.lue_NG_Code.Properties.DataSource = null;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                this.lue_NG_Code.Properties.DataSource = null;
                return;
            }
        }

        private void lueRCategory_EditValueChanged(object sender, EventArgs e)
        {
            string sCategoryKey = string.Empty;
            sCategoryKey = lueRCategory.EditValue.ToString();
            if (!string.IsNullOrEmpty(sCategoryKey))
            {
                BindReasonCode(sCategoryKey);
            }
            else
            {
                this.lue_NG_Code.Properties.DataSource = null;
            }
        }

        private void BindOQAData()
        {
            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            dsQueryOQA.Clear();
            dsQueryOQA = IVTestDateObject.GetOQADataReport(OQAQueryCondition.FactoryKey, OQAQueryCondition.SNType, OQAQueryCondition.Default, OQAQueryCondition.DateFalg, OQAQueryCondition.StartSN, OQAQueryCondition.EndSN, OQAQueryCondition.WO, OQAQueryCondition.PROID, OQAQueryCondition.StartDate, OQAQueryCondition.EndDate);
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                gcOQAData.DataSource = null;
                gcOQAData.MainView = gvOQAData;
                gcOQAData.DataSource = dsQueryOQA.Tables[0];
                gvOQAData.BestFitColumns();//自动调整列宽度
                gvOQAData.IndicatorWidth = 50;//自动调整行容器宽度
                lblTot.Text = "TOT:" + dsQueryOQA.Tables[0].Rows.Count.ToString();
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                lblTot.Text = "TOT:0";
                return;
            }
        }

        private void BindLevel()
        {
            this.lueLevel.EditValue = string.Empty;
            IVTestDataEntity IVTestDateObject = new IVTestDataEntity();
            DataSet dsLeve = IVTestDateObject.GetBasicData("ProductGrade", "", "", "");
            if (string.IsNullOrEmpty(IVTestDateObject.ErrorMsg))
            {
                if (dsLeve != null)
                {
                    this.lueLevel.Properties.DataSource = dsLeve.Tables[0];
                    this.lueLevel.Properties.DisplayMember = "COLUMN_NAME";
                    this.lueLevel.Properties.ValueMember = "COLUMN_CODE";
                    this.lueLevel.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("COLUMN_NAME"));
                }
                else
                {
                    this.lueLevel.Properties.DataSource = null;
                }
            }
            else
            {
                MessageService.ShowError(IVTestDateObject.ErrorMsg);
                this.lueLevel.Properties.DataSource = null;
                return;
            }
        }

        private void tsbQuery_Click(object sender, EventArgs e)
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
                    MessageService.ShowMessage("开始时间不能大于结束时间，请确认！", "提示");
                    return;
                }
                else
                {
                    OQAQueryCondition.StartDate = deStartDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    OQAQueryCondition.EndDate = deEndDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

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

            BindOQAData();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            int nColumn, nRow;

            if (gvOQAData.RowCount > 0 && dsQueryOQA.Tables[0].Rows.Count > 0)
            {
                try
                {
                    nColumn = gvOQAData.Columns.Count;
                    nRow = gvOQAData.RowCount;

                    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                    oExcel.Visible = false;
                    Microsoft.Office.Interop.Excel.Workbook oWorkbook = oExcel.Workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet oWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)oWorkbook.Worksheets[1];
                    //oWorksheet.Name = txtStockNo.Text.Trim();
                    for (int c = 0; c < nColumn; c++)
                    {
                        oWorksheet.Cells[1, c + 1] = gvOQAData.Columns[c].Caption.ToString().Trim();
                    }
                    for (int r = 0; r < nRow; r++)
                    {
                        for (int c = 0; c < nColumn; c++)
                        {
                            //oWorksheet.Cells[r + 2, c + 1] = dgConergy.Rows[r].Cells[c].Value;
                            if (c == 1 || c == 2)
                            {
                                oWorksheet.Cells[r + 2, c + 1] = "'" + dsQueryOQA.Tables[0].Rows[r][c].ToString();
                            }
                            else
                            {
                                oWorksheet.Cells[r + 2, c + 1] = dsQueryOQA.Tables[0].Rows[r][c].ToString();
                            }
                        }
                    }
                    nRow++;
                    oWorksheet.get_Range("A1", "M1").Interior.ColorIndex = 48;
                    oWorksheet.get_Range("A1", "M" + nRow.ToString()).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    oWorksheet.Cells.get_Range("A1", "M" + nRow.ToString()).Borders.LineStyle = 1;
                    oWorksheet.get_Range("D1", "D" + nRow.ToString()).EntireColumn.NumberFormat = "yyyy-MM-dd";
                    oWorksheet.get_Range("B1", "C" + nRow.ToString()).EntireColumn.NumberFormat = "@";
                    //oWorksheet.get_Range("G1", "L" + nRow.ToString()).EntireColumn.NumberFormat = "##0.00";
                    //oWorksheet.Cells.Font.Name = "Verdana";
                    //oWorksheet.Cells.Font.Size = 10;
                    //oWorksheet.Cells.AutoFit();
                    oExcel.Visible = true;
                    oExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                    System.GC.Collect();
                }
                catch //(Exception ex)
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotOutQuantityAssurance.Msg006}"), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);//创建Excel失败，请确认是否有安装Excel应用程序
                    return;
                }
            }
        }

        private void gvOQAData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;//行号居中
            if (e.Info.IsRowIndicator)
            {
                if (e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();//添加行号
                }
                else if (e.RowHandle < 0 && e.RowHandle > -1000)
                {
                    e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
                    e.Info.DisplayText = "G" + e.RowHandle.ToString();
                }
            }
        }
    }

    public class OQAQueryCondition
    {
        public static string FactoryKey;
        public static string SNType;
        public static string Default;
        public static string DateFalg;
        public static string StartSN;
        public static string EndSN;
        public static string WO;
        public static string PROID;
        public static string StartDate;
        public static string EndDate;
    }
}
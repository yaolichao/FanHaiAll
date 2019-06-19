using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class BasicRptOptForm : BaseDialog
    {
        public DataRow drCommon = null;
        public bool isEdit = false;

        RptCommonEntity optEntity = new RptCommonEntity();

        public BasicRptOptForm()
        {
            InitializeComponent();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0001}");//车间工序排班作业设置
            this.groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0002}");//车间工序排班
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0003}");//车间名称

            layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0004}");//工序
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0005}");//班别
            chkOverDay.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0006}");//是否跨天
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0007}");//开始时间
            layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0008}");//结束时间
            layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0009}");//备注
            btnOk.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0010}");//确定
            btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.lbl.0011}");//取消
            
        }

        private void BasicRptOptForm_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            BindShift();

            InitData();
        }

        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);

            if (dt != null && dt.Rows.Count > 0)
            {
                this.lueFactoryRoom.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
                this.lueFactoryRoom.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }

        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            DataSet dsOpt = optEntity.GetOperation();
            cbOperation.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
            cbOperation.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
            cbOperation.Properties.DataSource = dsOpt.Tables[0];
        }
        /// <summary>
        /// 绑定班别
        /// </summary>
        private void BindShift()
        {
            DataSet dsShift = optEntity.GetShiftName();
            if (!string.IsNullOrEmpty(optEntity.ErrorMsg))
            {
                MessageService.ShowError(optEntity.ErrorMsg);
                return;
            }

            lueShift.Properties.DisplayMember = CAL_SHIFT.FIELD_SHIFT_NAME;
            lueShift.Properties.ValueMember = CAL_SHIFT.FIELD_SHIFT_KEY;

            lueShift.Properties.DataSource = dsShift.Tables[0];
        }

        private void InitData()
        {
            string startime = Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_START_TIME]);
            string endtime=Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_END_TIME]);
            this.lueFactoryRoom.EditValue = Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_LOCATION_KEY]);
            this.cbOperation.EditValue = Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_OPERATION_KEY]);
            this.lueShift.EditValue = Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_SHIFT_KEY]);
            this.spStartTime.Text = startime.Trim().Length == 5 ? startime.Substring(0, 2) + "00" : "";
            this.spEndTime.Text = endtime.Trim().Length == 5 ? endtime.Substring(0, 2) + "00" : "";
            this.meoRemark.Text = Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_REMARK]);

            if (bool.Parse(Convert.ToString(drCommon[BASE_OPT_SETTING.FIELDS_OVER_DAY])))
                this.chkOverDay.Checked = true;
            else
                this.chkOverDay.Checked = false;

        }
        private bool isValidData()
        {            
            if (Convert.ToString(lueFactoryRoom.EditValue).Equals(string.Empty))
            {
                //MessageService.ShowMessage("车间不能为空!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                lueFactoryRoom.Select();
                return false;                
            }
            if (Convert.ToString(cbOperation.EditValue).Equals(string.Empty))
            {
                //MessageService.ShowMessage("工序不能为空!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                cbOperation.Select();
                return false;
            }
            if (Convert.ToString(lueShift.EditValue).Equals(string.Empty))
            {
                //MessageService.ShowMessage("班别不能为空!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                lueShift.Select();
                return false;
            }
            if (spStartTime.Text.Trim().Equals(string.Empty))
            {
                //MessageService.ShowMessage("开始时间不能为空!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                spStartTime.Focus();
                return false;
            }
            if (spEndTime.Text.Trim().Equals(string.Empty))
            {
                //MessageService.ShowMessage("结束时间不能为空!", "提示");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                spEndTime.Focus();
                return false;
            }
            
            return true;

        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!isValidData()) return;

            string seq = string.Empty;
            string optkey=Convert.ToString(cbOperation.EditValue);
            DataTable dtOperation = this.cbOperation.Properties.DataSource as DataTable;
            DataRow[] drs=dtOperation.Select(string.Format(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY+"='{0}'",optkey));
            if (drs != null && drs.Length > 0)
                seq = Convert.ToString(drs[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SORT_SEQ]);

            drCommon[BASE_OPT_SETTING.FIELDS_OPERATION_KEY] = optkey;
            drCommon[BASE_OPT_SETTING.FIELDS_OPERATION_NAME] = cbOperation.Text;

            drCommon[BASE_OPT_SETTING.FIELDS_LOCATION_KEY] = Convert.ToString(lueFactoryRoom.EditValue);
            drCommon[BASE_OPT_SETTING.FIELDS_LOCATION_NAME] = lueFactoryRoom.Text;
            drCommon[BASE_OPT_SETTING.FIELDS_SHIFT_KEY] = Convert.ToString(lueShift.EditValue);
            drCommon[BASE_OPT_SETTING.FIELDS_SHIFT_NAME] = lueShift.Text;
            drCommon[BASE_OPT_SETTING.FIELDS_START_TIME] = spStartTime.Text;
            drCommon[BASE_OPT_SETTING.FIELDS_END_TIME] = spEndTime.Text;
            drCommon[BASE_OPT_SETTING.FIELDS_OVER_DAY] = chkOverDay.CheckState;
            drCommon[BASE_OPT_SETTING.FIELDS_SORT_SEQ] = seq;
            drCommon[BASE_OPT_SETTING.FIELDS_REMARK] = this.meoRemark.Text.Trim();

            DataSet dsSave = new DataSet();
            DataTable dtSave = drCommon.Table.Clone();
            dtSave.Rows.Add(drCommon.ItemArray);
            if (this.isEdit)
                dtSave.TableName = BASE_OPT_SETTING.DATABASE_TABLE_FORUPDATE;
            else
                dtSave.TableName = BASE_OPT_SETTING.DATABASE_TABLE_FORINSERT;

            dsSave.Merge(dtSave, true, MissingSchemaAction.Add);

            bool bck = optEntity.SaveOptSettingData(dsSave);
            if (!string.IsNullOrEmpty(optEntity.ErrorMsg))
            {
                MessageService.ShowMessage(optEntity.ErrorMsg);
                return;
            }
            else
            {
                //MessageService.ShowMessage("保存成功!");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BasicRptOptForm.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
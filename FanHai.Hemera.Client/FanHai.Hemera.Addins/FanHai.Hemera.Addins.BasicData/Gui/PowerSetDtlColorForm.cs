using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class PowerSetDtlColorForm : BaseDialog
    {
        public DataRow drCommon = null;
        public bool isEdit = false;
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();

        public PowerSetDtlColorForm()
        {
            InitializeComponent();
        }

        private void PowerSetDtlColorForm_Load(object sender, EventArgs e)
        {
            BindProLevel();
            InitionData();
        }
        private void BindProLevel()
        {
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtCommon = BaseData.Get(l_s, category);

            DataTable dtLevel = dtCommon.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtCommon.Select(string.Format("Column_type='{0}'", BASE_POWERSET.MODELCOLOR));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";

            lueColor.Properties.DisplayMember = "Column_Name";
            lueColor.Properties.ValueMember = "Column_code";
            lueColor.Properties.DataSource = dview.Table;
        }

        private void InitionData()
        {
            if (!isEdit)
            {
                //新增
                txtArticleNo.Text = string.Empty;
                meDescription.Text = string.Empty;
            }
            else
            {
                //编辑
                lueColor.EditValue = drCommon[BASE_POWERSET_COLORATCNO.FIELDS_COLOR_CODE].ToString();
                txtArticleNo.Text = drCommon[BASE_POWERSET_COLORATCNO.FIELDS_ARTICNO].ToString();
                meDescription.Text = drCommon[BASE_POWERSET_COLORATCNO.FIELDS_DESCRIPTION].ToString();
                lueColor.Properties.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsPowerSetDtlColor = new DataSet();
            drCommon[BASE_POWERSET_COLORATCNO.FIELDS_COLOR_CODE] = Convert.ToString(lueColor.EditValue);
            drCommon[BASE_POWERSET_COLORATCNO.FIELDS_ARTICNO] = txtArticleNo.Text.Trim();
            drCommon[BASE_POWERSET_COLORATCNO.FIELDS_DESCRIPTION] = meDescription.Text.Trim();
            drCommon[BASE_POWERSET_COLORATCNO.FIELDS_COLOR_NAME] = lueColor.Text.Trim();

            if (isEdit)
                drCommon[BASE_POWERSET_COLORATCNO.FIELDS_EDITER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            else
                drCommon[BASE_POWERSET_COLORATCNO.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            DataTable dtSave = drCommon.Table.Clone();
            dtSave.Rows.Add(drCommon.ItemArray);
            if (isEdit)
                dtSave.TableName = BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORUPDATE;
            else
            {
                dtSave.TableName = BASE_POWERSET_COLORATCNO.DATABASE_TABLE_FORINSERT;

                bool bl = _basePowerSetEntity.IsExistPowerDtlColorData(dtSave);
                if (!bl && !string.IsNullOrEmpty(_basePowerSetEntity.ErrorMsg))
                {
                    MessageService.ShowError(_basePowerSetEntity.ErrorMsg);
                    this.txtArticleNo.Focus();
                    this.txtArticleNo.SelectAll();
                    return;
                }

            }

            dsPowerSetDtlColor.Merge(dtSave, true, MissingSchemaAction.Add);
            // 判断数据是否重复
            bool bl_bak = _basePowerSetEntity.SavePowerSetData(dsPowerSetDtlColor);
            if (!bl_bak)
            {
                MessageService.ShowMessage(_basePowerSetEntity.ErrorMsg);
            }
            else
            {
                MessageService.ShowMessage("保存成功!");
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
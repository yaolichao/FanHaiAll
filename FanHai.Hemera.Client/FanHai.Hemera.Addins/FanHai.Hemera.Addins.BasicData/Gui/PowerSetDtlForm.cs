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

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class PowerSetDtlForm : BaseDialog
    {
        public DataRow drCommon = null;
        public bool isEdit = false;
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();
        public PowerSetDtlForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PowerSetDtlForm_Load(object sender, EventArgs e)
        {
            InitionData();
        }

        private void InitionData()
        {
            if (!isEdit)
            {
                //新增
                txtDtlSubcode.Text = string.Empty;
                txtPowerLevel.Text = string.Empty;
                txtPdtlmin.Text = string.Empty;
                txtPdtlmax.Text = string.Empty;
            }
            else
            {
                //编辑
                txtDtlSubcode.Text = drCommon[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE].ToString();
                txtPowerLevel.Text = drCommon[BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL].ToString();
                txtPdtlmin.Text = drCommon[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MIN].ToString();
                txtPdtlmax.Text = drCommon[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MAX].ToString();
                txtDtlSubcode.Properties.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataSet dsPowerLevelDtl = new DataSet();
            drCommon[BASE_POWERSET_DETAIL.FIELDS_PS_DTL_SUBCODE] = txtDtlSubcode.Text.Trim();
            drCommon[BASE_POWERSET_DETAIL.FIELDS_POWERLEVEL] = txtPowerLevel.Text.Trim();
            drCommon[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MIN] = txtPdtlmin.Text.Trim();
            drCommon[BASE_POWERSET_DETAIL.FIELDS_P_DTL_MAX] = txtPdtlmax.Text.Trim();

            if (isEdit)
                drCommon[BASE_POWERSET_DETAIL.FIELDS_EDITER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            else
                drCommon[BASE_POWERSET_DETAIL.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
  
            DataTable dtSave = drCommon.Table.Clone();
            dtSave.Rows.Add(drCommon.ItemArray);
            if (isEdit)
                dtSave.TableName = BASE_POWERSET_DETAIL.DATABASE_TABLE_FORUPDATE;
            else
            {
                dtSave.TableName = BASE_POWERSET_DETAIL.DATABASE_TABLE_FORINSERT;
                bool bl = _basePowerSetEntity.IsExistPowerDtlData(dtSave);
                if (!bl && !string.IsNullOrEmpty(_basePowerSetEntity.ErrorMsg))
                {
                    MessageService.ShowError(_basePowerSetEntity.ErrorMsg);
                    this.txtDtlSubcode.Focus();
                    this.txtDtlSubcode.SelectAll();
                    return;
                }

            }
          
            dsPowerLevelDtl.Merge(dtSave, true, MissingSchemaAction.Add);
            bool bl_bak = _basePowerSetEntity.SavePowerSetData(dsPowerLevelDtl);
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
    }
}
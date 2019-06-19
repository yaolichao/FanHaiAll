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
    public partial class PowerSetForm : BaseDialog
    {
        DataTable dtProductLevel = null;
        public DataRow drCommon = null;
        public bool isEdit = false;
        ProductModelEntity _productModelEntity = new ProductModelEntity();
        BasePowerSetEntity _basePowerSetEntity = new BasePowerSetEntity();
        public PowerSetForm()
        {
            InitializeComponent();
        }

        private void PowerSetForm_Load(object sender, EventArgs e)
        {
            InitionCombox();
            InitionData();
        }
        private void InitionCombox()
        {
            DataSet dsCombox = _productModelEntity.GetProductModelAndCP();
            DataTable dtProductModel = dsCombox.Tables[BASE_PRODUCTMODEL.DATABASE_TABLE_NAME];
             dtProductLevel = dsCombox.Tables[BASE_PRODUCTMODEL_POWER.DATABASE_TABLE_NAME];

            this.lueProductModel.Properties.ValueMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_KEY;
            this.lueProductModel.Properties.DisplayMember = BASE_PRODUCTMODEL.FIELDS_PROMODEL_NAME;
            this.lueProductModel.Properties.DataSource = dtProductModel;
            this.lueProductModel.ItemIndex = -1;

            this.lukModelName.Properties.ValueMember = BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY;
            this.lukModelName.Properties.DisplayMember = BASE_PRODUCTMODEL_POWER.FIELDS_PM;
            this.lukModelName.Properties.DataSource = dtProductLevel;
            this.lukModelName.ItemIndex = -1;
        }
        private void InitionData()
        {
            if (!isEdit)
            {
                //新增
                this.txtPs_code.Text = string.Empty;
                this.txtPs_rule.Text = string.Empty;
                this.txtPs_subcode.Text = string.Empty;
                this.txtPs_subcode_desc.Text = string.Empty;
                this.txtSeq.Text = string.Empty;
                this.txtPmax.Text = string.Empty;
                this.txtImpp.Text = string.Empty;
                this.txtIsc.Text = string.Empty;
                this.txtVmpp.Text = string.Empty;
                this.txtVoc.Text = string.Empty;
                this.txtFuse.Text = string.Empty;
                this.txtP_min.Text = string.Empty;
                this.txtP_max.Text = string.Empty;
                this.txtPowerdifference.Text = string.Empty;
            }
            else
            {
                //编辑
                this.txtPs_code.Text = drCommon[BASE_POWERSET.FIELDS_PS_CODE].ToString();
                this.txtPs_rule.Text = drCommon[BASE_POWERSET.FIELDS_PS_RULE].ToString();
                this.txtPs_subcode.Text = drCommon[BASE_POWERSET.FIELDS_PS_SUBCODE].ToString();
                this.txtPs_subcode_desc.Text = drCommon[BASE_POWERSET.FIELDS_PS_SUBCODE_DESC].ToString();
                this.txtSeq.Text = drCommon[BASE_POWERSET.FIELDS_PS_SEQ].ToString();
                this.txtPmax.Text = drCommon[BASE_POWERSET.FIELDS_PMAXSTAB].ToString();
                this.txtImpp.Text = drCommon[BASE_POWERSET.FIELDS_IMPPSTAB].ToString();
                this.txtIsc.Text = drCommon[BASE_POWERSET.FIELDS_ISCSTAB].ToString();
                this.txtVmpp.Text = drCommon[BASE_POWERSET.FIELDS_VMPPSTAB].ToString();
                this.txtVoc.Text = drCommon[BASE_POWERSET.FIELDS_VOCSTAB].ToString();
                this.txtFuse.Text = drCommon[BASE_POWERSET.FIELDS_FUSE].ToString();
                this.txtP_min.Text = drCommon[BASE_POWERSET.FIELDS_P_MIN].ToString();
                this.txtP_max.Text = drCommon[BASE_POWERSET.FIELDS_P_MAX].ToString();
                this.txtPowerdifference.Text = drCommon[BASE_POWERSET.FIELDS_POWER_DIFFERENCE].ToString();

                this.lueProductModel.EditValue = drCommon[BASE_POWERSET.FIELDS_PROMODEL_KEY].ToString();
                this.lukModelName.Text = drCommon[BASE_POWERSET.FIELDS_MODULE_NAME].ToString();
                this.cmbCodeSubWay.EditValue = drCommon[BASE_POWERSET.FIELDS_SUB_PS_WAY].ToString();
                this.txtPs_code.Properties.ReadOnly = true;
            }
        }

        private void lueProductModel_EditValueChanged(object sender, EventArgs e)
        {
            string pmkey = lueProductModel.EditValue.ToString();
            DataTable dtLevel = dtProductLevel.Clone();
            DataRow[] drLevels = dtProductLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_KEY + "='{0}'", pmkey));
            foreach (DataRow dr in drLevels)
                dtLevel.ImportRow(dr);
            this.lukModelName.Properties.DataSource = dtLevel;
        }

        private void lukModelName_EditValueChanged(object sender, EventArgs e)
        {
            string pmlevelkey = lukModelName.EditValue.ToString();
            DataRow drlevel = dtProductLevel.Select(string.Format(BASE_PRODUCTMODEL_POWER.FIELDS_PROMODEL_POWER_KEY + "='{0}'", pmlevelkey))[0];

            //this.txtPmax.Text = drlevel[BASE_PRODUCTMODEL_POWER.FIELDS_PM].ToString();
            this.txtImpp.Text = drlevel[BASE_PRODUCTMODEL_POWER.FIELDS_IMP].ToString();
            this.txtIsc.Text = drlevel[BASE_PRODUCTMODEL_POWER.FIELDS_ISC].ToString();
            this.txtVmpp.Text = drlevel[BASE_PRODUCTMODEL_POWER.FIELDS_VMP].ToString();
            this.txtVoc.Text = drlevel[BASE_PRODUCTMODEL_POWER.FIELDS_VOC].ToString();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveForPowerSetData();
        }
        private void SaveForPowerSetData()
        {
            #region 输入条件判断
            if (string.IsNullOrEmpty(txtPs_code.Text.Trim()))
            {
                MessageService.ShowMessage("规则代码不能为空!", "提示");
                this.txtPs_code.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPs_rule.Text.Trim()))
            {
                MessageService.ShowMessage("名称不能为空!", "提示");
                this.txtPs_rule.Focus();
                return;
            }
            if (lukModelName.EditValue == null)
            {
                MessageService.ShowMessage("档位名称不能为空!", "提示");
                this.lukModelName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtSeq.Text.Trim()))
            {
                MessageService.ShowMessage("序号不能为空!", "提示");
                this.txtSeq.Focus();
                return;
            }
            if (lueProductModel.EditValue == null || string.IsNullOrEmpty(lueProductModel.EditValue.ToString()))
            {
                MessageService.ShowMessage("产品型号不能为空!", "提示");
                this.lueProductModel.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPmax.Text.Trim()))
            {
                MessageService.ShowMessage("Pmax不能为空!", "提示");
                this.txtPmax.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtImpp.Text.Trim()))
            {
                MessageService.ShowMessage("Impp不能为空!", "提示");
                this.txtImpp.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtIsc.Text.Trim()))
            {
                MessageService.ShowMessage("Isc不能为空!", "提示");
                this.txtIsc.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtVmpp.Text.Trim()))
            {
                MessageService.ShowMessage("Vmpp不能为空!", "提示");
                this.txtVmpp.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtVoc.Text.Trim()))
            {
                MessageService.ShowMessage("Voc不能为空!", "提示");
                this.txtVoc.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFuse.Text.Trim()))
            {
                MessageService.ShowMessage("Fuse不能为空!", "提示");
                this.txtFuse.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtP_min.Text.Trim()))
            {
                MessageService.ShowMessage("功率下线不能为空!", "提示");
                this.txtP_min.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtP_max.Text.Trim()))
            {
                MessageService.ShowMessage("功率上线不能为空!", "提示");
                this.txtP_max.Focus();
                return;
            }
            #endregion

            DataSet dsPowerSet = new DataSet();
            drCommon[BASE_POWERSET.FIELDS_PS_CODE] = txtPs_code.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PS_RULE] = txtPs_rule.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PS_SUBCODE] = txtPs_subcode.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PS_SUBCODE_DESC] = txtPs_subcode_desc.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PROMODEL_KEY] = lueProductModel.EditValue.ToString();
            drCommon[BASE_POWERSET.FIELDS_MODULE_NAME] = lukModelName.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PS_SEQ] = txtSeq.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_PMAXSTAB] = txtPmax.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_IMPPSTAB] = txtImpp.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_ISCSTAB] = txtIsc.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_VMPPSTAB] = txtVmpp.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_VOCSTAB] = txtVoc.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_FUSE] = txtFuse.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_P_MIN] = txtP_min.Text.Trim();
            drCommon[BASE_POWERSET.FIELDS_P_MAX] = txtP_max.Text.Trim();

            if(isEdit)
                drCommon[BASE_POWERSET.FIELDS_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            else
                drCommon[BASE_POWERSET.FIELDS_CREATER] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

            if (cmbCodeSubWay.EditValue != null)
                drCommon[BASE_POWERSET.FIELDS_SUB_PS_WAY] = cmbCodeSubWay.EditValue.ToString();
            drCommon[BASE_POWERSET.FIELDS_POWER_DIFFERENCE] = txtPowerdifference.Text.Trim();
            DataTable dtSave = drCommon.Table.Clone();
            dtSave.Rows.Add(drCommon.ItemArray);
            if (isEdit)
                dtSave.TableName = BASE_POWERSET.DATABASE_TABLE_FORUPDATE;
            else
            {
                dtSave.TableName = BASE_POWERSET.DATABASE_TABLE_FORINSERT;
                bool bl2 = _basePowerSetEntity.IsExistPowerSetData(dtSave);
                if (!bl2 && !string.IsNullOrEmpty(_basePowerSetEntity.ErrorMsg))
                {
                    MessageService.ShowMessage(_basePowerSetEntity.ErrorMsg);
                    return;
                }
            }
            
            dsPowerSet.Merge(dtSave, true, MissingSchemaAction.Add);

            bool bl_Bak = _basePowerSetEntity.SavePowerSetData(dsPowerSet);
            if (!bl_Bak)
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
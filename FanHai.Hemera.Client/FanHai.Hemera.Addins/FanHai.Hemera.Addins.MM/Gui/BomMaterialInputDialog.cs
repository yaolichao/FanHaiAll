using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 工单BOM物料输入对话框。
    /// </summary>
    public partial class BomMaterialInputDialog : BaseDialog
    {
        /// <summary>
        /// 工单号。
        /// </summary>
        string _orderNumber = string.Empty;
        DataTable _dtMaterial = null;
        /// <summary>
        /// 物料编码。
        /// </summary>
        public string MaterialCode
        {
            get
            {
                return this.cmbMaterialCode.Text;
            }
        }
        /// <summary>
        /// 工单BOM物料输入对话框。
        /// </summary>
        public BomMaterialInputDialog(string orderNumber)
        {
            InitializeComponent();
            this._orderNumber = orderNumber;
        }
        /// <summary>
        /// 确定按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            
            string materialCode = this.cmbMaterialCode.Text;
            string materialDescription = this.txtMaterialDescription.Text;
            //不是数据库中已经存在原材料编码。
            if (!this.txtMaterialDescription.Properties.ReadOnly)
            {
                if (materialCode.Length != 10)
                {
                    MessageService.ShowMessage("原材料编码必须为10码。", "提示");
                    this.cmbMaterialCode.Select();
                    return;
                }
                if (materialCode.CompareTo("200")<0)
                {
                    MessageService.ShowMessage("原材料编码必须是大于200的字符串。", "提示");
                    this.cmbMaterialCode.Select();
                    return;
                }
                if (string.IsNullOrEmpty(materialDescription))
                {
                    MessageService.ShowMessage("原材料描述不能为空。","提示");
                    this.txtMaterialDescription.Select();
                    return;
                }
            }
            //向数据库中添加工单BOM的自备料
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            entity.CreateWOBomOwnMaterial(this._orderNumber, materialCode, materialDescription);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageService.ShowError(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BomMaterialInputDialog_Load(object sender, EventArgs e)
        {
            this.teOrderNumber.Text = this._orderNumber;
            this.teOrderNumber.Properties.ReadOnly = true;
            BindMaterialCode();
            this.cmbMaterialCode.Select();
        }

        /// <summary>
        /// 绑定物料代码。
        /// </summary>
        private void BindMaterialCode()
        {
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetMaterials();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this._dtMaterial = ds.Tables[0];
                this.cmbMaterialCode.Properties.Items.Clear();
                foreach (DataRow dr in this._dtMaterial.Rows)
                {
                    string materialCode = Convert.ToString(dr["MATERIAL_CODE"]);
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        this.cmbMaterialCode.Properties.Items.Add(materialCode);
                    }
                }
            }
            else
            {
                MessageService.ShowError(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 物料编码值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMaterialCode_EditValueChanged(object sender, EventArgs e)
        {
            string materialCode = this.cmbMaterialCode.Text.Trim();
            DataRow [] drs=this._dtMaterial.Select("MATERIAL_CODE='"+materialCode+"'");
            if (drs.Length > 0)
            {
                string materialDescription = Convert.ToString(drs[0]["MATERIAL_NAME"]);
                this.txtMaterialDescription.Text = materialDescription;
                this.txtMaterialDescription.Properties.ReadOnly = true;
            }
            else
            {
                this.txtMaterialDescription.Text = string.Empty;
                this.txtMaterialDescription.Properties.ReadOnly = false;
            }
        }



    }
}

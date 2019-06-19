/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.SPC
{
    public partial class DeletePointDialog : BaseDialog
    {
        private string _pointkey = null;
        private string colkey = string.Empty;
        public bool isMr = false;
        public DeletePointDialog():base("删除该笔数据")
        {
            InitializeComponent();
        }

        public DeletePointDialog(string pointkey)
        {
            InitializeComponent();
            _pointkey = pointkey;
            //colkey = _pointList[0].Value.ToString().Trim();
            //colkey = pointList[0].Key.ToString();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            //DataTable paramTable = new DataTable(SPC_PARAM_DATA_FIELDS.DATABASE_TABLE_NAME);
            //paramTable.Columns.Add(SPC_PARAM_DATA_FIELDS.COL_KEY);

            if (meReason.Text.Trim().Length == 0)
            {
                MessageService.ShowError("请填写原因");
                return;
            }
            //剔除选择的原始数据
            DataSet dataSet = new DataSet();
            dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.POINT_KEY, _pointkey);
            dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDIT_REASON, meReason.Text);
            dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            dataSet.ExtendedProperties.Add("isMr", isMr);
            //dataSet.Tables.Add(paramTable);
            SpcEntity spcEntity = new SpcEntity();
            if (spcEntity.DeletePoints(dataSet))
            {
                MessageService.ShowMessage("剔除成功！");
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageService.ShowError("剔除异常点出错：" + spcEntity.ErrorMsg);
            }
        }
    }
}

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
    public partial class EditPointDialog : BaseDialog
    {
        SpcEntity spcEntity = new SpcEntity();
        private string _pointkey = string.Empty;
        private int _editFlag = 0;
        string colKey = string.Empty;
        public bool isMr = false;
        public EditPointDialog():base("修正数据")
        {
            InitializeComponent();
        }
        public int EditFlag
        {
            get { return _editFlag; }
        }

        public EditPointDialog(string pointkey,int editFlag)
        {
            _editFlag = editFlag;
            InitializeComponent();
            _pointkey = pointkey;
            //colKey = pointList[0].Key.ToString();
        }

        private void EditPointDialog_Load(object sender, EventArgs e)
        {
            DataSet ds = spcEntity.GetEditInformation(_pointkey);

            if (ds.Tables.Contains(SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS))
            {
                DataTable dtPoint = ds.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];
                gcPoint.MainView = gvPoint;
                gcPoint.DataSource = dtPoint;
            }
            if (ds.Tables.Contains(SPC_GROUP_POINT_EDIT_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtGroupEdit = ds.Tables[SPC_GROUP_POINT_EDIT_FIELDS.DATABASE_TABLE_NAME];
                gcPointEdit.MainView = gvPointEdit;
                gcPointEdit.DataSource = dtGroupEdit;
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {            

            if (meReason.Text.Trim().Length == 0)
            {
                MessageService.ShowError("请填写原因");
                return;
            }
            if (MessageService.AskQuestion("确定要修正数据吗？"))
            {
                //剔除选择的原始数据
                DataSet dataSet = new DataSet();
                dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDIT_REASON, meReason.Text);
                dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                //dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.COL_KEY, colKey);
                dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.EDIT_FLAG, _editFlag.ToString());
                dataSet.ExtendedProperties.Add(SPC_PARAM_DATA_FIELDS.POINT_KEY, _pointkey);

                SpcEntity spcEntity = new SpcEntity();
                //if (spcEntity.ModifyPoints(dataSet,isMr))
                if (spcEntity.ModifyPoints(dataSet))
                {
                    MessageService.ShowMessage("修正成功！");
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageService.ShowError("修正异常点出错：" + spcEntity.ErrorMsg);
                }

            }
        }     
    }
}

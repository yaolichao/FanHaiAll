using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Addins.SPC
{
    public partial class EditInformation : BaseDialog
    {
         private string _pointkey = null;
        SpcEntity spcEntity = new SpcEntity();
        string _edcInsKey = string.Empty;
        public EditInformation():base("查看编辑信息")
        {
            InitializeComponent();
        }

        public EditInformation(string pointkey)
            : base("查看编辑信息")
        {
            if (pointkey.Length < 1)
            {
                MessageService.ShowMessage("该采集点数据有错，请与系统管理员联系！");
                return;
            }

            _pointkey = pointkey;
           
            InitializeComponent();           
        }

        private void EditInformation_Load(object sender, EventArgs e)
        {
            if (_pointkey.Length < 1)
                return;

            DataSet dsInformation = spcEntity.GetEditInformation(_pointkey);

            if (dsInformation.Tables.Contains(SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS))
            {
                DataTable dtPoint = dsInformation.Tables[SPC_PARAM_DATA_FIELDS.DB_FOR_POINTS];
                gcPoint.MainView = gvPoint;
                gcPoint.DataSource = dtPoint;
            }
            if (dsInformation.Tables.Contains(SPC_GROUP_POINT_EDIT_FIELDS.DATABASE_TABLE_NAME))
            {
                DataTable dtGroupEdit = dsInformation.Tables[SPC_GROUP_POINT_EDIT_FIELDS.DATABASE_TABLE_NAME];
                gcPointEdit.MainView = gvPointEdit;
                gcPointEdit.DataSource = dtGroupEdit;
            }      
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

      
    }
}

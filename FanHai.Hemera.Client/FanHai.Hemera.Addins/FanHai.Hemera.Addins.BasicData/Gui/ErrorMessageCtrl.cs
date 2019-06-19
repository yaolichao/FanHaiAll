using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Framework.Gui;
using FanHai.Gui.Framework;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ErrorMessageCtrl : BaseUserCtrl
    {
        ErrorMessage _em = null;
        public ErrorMessageCtrl()
        {
            InitializeComponent();
            this.gcMessage.MainView = this.gvMessage;
            //this.tMessage.Enabled = true;
            GetData();
        }

        private void gvMessage_ShowingEditor(object sender, CancelEventArgs e)
        {
            //e.Cancel = true;
        }

        private void GetData()
        {           
            //get error message information 
            string strUser = string.Empty;
            _em = new ErrorMessage();
            strUser = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            DataSet dsErrMsg = new DataSet();
            dsErrMsg = _em.GetErrorMessageInfor(strUser);

            if (!string.IsNullOrEmpty(_em.ErrorMsg))
            {
                MessageService.ShowMessage(_em.ErrorMsg, "系统信息");
            }
            else
            {
                if (dsErrMsg != null && dsErrMsg.Tables.Count > 0)
                {
                    DataTable wipMsgDataTable = dsErrMsg.Tables[0];

                    if (wipMsgDataTable != null && wipMsgDataTable.Rows.Count > 0)
                    {
                        //ErrorMessageCommand errMsgCommand = new ErrorMessageCommand();
                        //errMsgCommand.Run();
                    }
                    this.gcMessage.DataSource = wipMsgDataTable;
                }
            }
        }

        private void gvMessage_DoubleClick(object sender, EventArgs e)
        {
            //transact message
            if (this.gvMessage.FocusedRowHandle >= 0 && this.gvMessage.GetRowCellValue(this.gvMessage.FocusedRowHandle, "ROW_KEY").ToString() != "")
            {
                MessageService.ShowMessage("错误消息处理!", "系统信息");
            }
        }

        private void btFresh_Click(object sender, EventArgs e)
        {
            GetData();

        }
        
    }
}

/*
<FileInfo>
  <Author>Rayna.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
#endregion

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class UserSearchDialog : BaseDialog
    {
        #region User Entity
        public User userEntity = new User();
        #endregion

        #region Constructor
        public UserSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserSearchDialog.Title}"))
        {
            InitializeComponent();
        }
        #endregion

        #region Control events
        private void userView_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            User _userEntity = new User();            
            DataSet ds = new DataSet();
            _userEntity.UserName = this.txtUserName.Text;
            ds = _userEntity.SearchUserInfo();
            if (_userEntity.ErrorMsg == "")
            {
                if (ds.Tables.Count > 0)
                {
                    this.userControl.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {                        
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserSearchDialog.Msg.SearchResult}", "${res:Global.SystemInfo}");
                    }
                }               
            }
            else
            {              
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}");
            }
        }
        #endregion

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = userView.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                userEntity.UserKey = userView.GetRowCellValue(rowHandle, this.USER_KEY).ToString();
                userEntity.UserName = userView.GetRowCellValue(rowHandle, this.USERNAME).ToString();
                return true;
            }
            return false;
        }

        #region UI event
        private void UserSearchDialog_Load(object sender, EventArgs e)
        {
            InitUIByCulture();
            GridViewHelper.SetGridView(userView);
        }
        public void InitUIByCulture()
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.groupControlQueryCondition.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserSearchDialog.QueryCondition}");
            this.lblUserName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblUserName}");
            this.UserInfoGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserSearchDialog.UserInfoGroup}");
            this.USERNAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblUserName}");
            this.BADGE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblBadge}");
            this.EMAIL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblEmail}");
            this.OFFICE_PHONE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblOfficePhone}");
            this.MOBILE_PHONE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblMobilePhone}");
            this.REMARK.Caption = StringParser.Parse("${res:Global.Remark}");
        }
        #endregion

        private void userView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

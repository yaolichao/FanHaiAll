using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections;
using System.Data;
using System.Web.Security;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 显示修改密码界面的对话框。
    /// </summary>
    public partial class ChangePasswordDialog : BaseDialog
    {
        public ChangePasswordDialog():
            base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.Title}"))
        {
            InitializeComponent();
            this.teUserName.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);            
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {

            string strAskQuestion = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.AskQuestion}");
            string strMessage=string.Empty;
            //弹出对话框确认是否继续操作                     
            if (MessageService.AskQuestion(strAskQuestion))
            {
                //判断旧密码栏位是否为空 为空弹出对话框提示    
                if (tePswOld.Text.Length < 1)
                {
                    strMessage = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.OldPasswordMustNotNull}");
                    MessageService.ShowMessage(strMessage);
                    this.tePswOld.Focus();
                    return;
                }
                //新密码栏位是否为空 为空弹出对话框提示        
                if (tePswNew.Text.Length < 1)
                {
                    strMessage = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.NewPasswordMustNotNull}");
                    MessageService.ShowMessage(strMessage);
                    this.tePswNew.Focus();
                    return;
                }
                //新密码再次确认栏位是否为空 为空弹出对话框提示 
                if (tePswAgain.Text.Length < 1)
                {
                    strMessage = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.AgainPasswordMustNotNull}");
                    MessageService.ShowMessage(strMessage);
                    this.tePswAgain.Focus();
                    return;
                }
                //判断新密码和新密码确认栏是否一致不一致弹出对话框提示  
                if (tePswNew.Text != tePswAgain.Text)
                {
                    strMessage = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.PasswordNotMatch}");
                    MessageService.ShowMessage(strMessage);
                    this.tePswNew.Text = string.Empty;
                    this.tePswAgain.Text = string.Empty;
                    this.tePswNew.Focus();
                    return;
                }

                string passwordOld = FormsAuthentication.HashPasswordForStoringInConfigFile(this.tePswOld.Text, "SHA1");
                string passwordNew = FormsAuthentication.HashPasswordForStoringInConfigFile(this.tePswNew.Text, "SHA1");
                Hashtable hashTable = new Hashtable();
                hashTable.Add(RBAC_USER_FIELDS.FIELD_BADGE, teUserName.Text);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_PASSWORD, passwordOld);
                hashTable.Add(RBAC_USER_FIELDS.FIELD_PASSWORD_NEW, passwordNew);
                DataTable tableUser = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableUser.TableName = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
                User userEntity = new User();
                if (userEntity.ChangePassword(tableUser))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.ModifySuccess}"));
                    this.btCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.Quit}");
                }                
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 对话框载入事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePasswordDialog_Load(object sender, EventArgs e)
        {
            //初始化界面资源
            this.layoutCtlGrpChangePwd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.Title}");
            this.lblUserName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.UserName}");
            this.lblPswNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.NewPassword}");
            this.lblPswOld.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.OldPassword}");
            this.lblPswAgain.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.AgainPassword}");
            this.btConfirm.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.OK}");
            this.btCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ChangePasswordDialog.Cancle}");

        }
    }
}

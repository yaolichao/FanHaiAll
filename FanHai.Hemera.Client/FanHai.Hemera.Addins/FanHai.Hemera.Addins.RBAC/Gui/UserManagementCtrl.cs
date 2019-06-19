#region using
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Windows.Forms;
#endregion

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 显示用户管理界面的用户控件类。
    /// </summary>
    public partial class UserManagementCtrl : BaseUserCtrl
    {
        #region entity and other variable define
        private User _userEntity = null;
        public new delegate void AfterStateChanged(ControlState controlState);
        public new AfterStateChanged afterStateChanged = null;
        private ControlState _ctrlState;
        #endregion

        #region Constructor
        public UserManagementCtrl()
        {
            InitializeComponent();
        }
        public UserManagementCtrl(User userEntity)
        {
            InitializeComponent();
            GridViewHelper.SetGridView(this.userView);
            _userEntity = userEntity;
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            DataSet dataSet = new DataSet();
            if (_userEntity.UserKey != "")
            {
                CtrlState = ControlState.Edit;
                dataSet = _userEntity.GetUserInfo();
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    _userEntity.Badge = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_BADGE].ToString();
                    _userEntity.Password = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_PASSWORD].ToString();
                    _userEntity.Email = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_EMAIL].ToString();
                    _userEntity.OfficePhone = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_OFFICE_PHONE].ToString();
                    _userEntity.MobilePhone = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_MOBILE_PHONE].ToString();
                    _userEntity.Remark = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_REMARK].ToString();
                    _userEntity.Editor = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_EDITOR].ToString();
                    _userEntity.EditTimeZone = dataSet.Tables[0].Rows[0][RBAC_USER_FIELDS.FIELD_EDIT_TIMEZONE].ToString();

                    this.txtUserName.Text = _userEntity.UserName;
                    this.txtBadge.Text = _userEntity.Badge;
                    this.txtPsw.Text = _userEntity.Password;
                    this.txtPswAffirm.Text = this.txtPsw.Text;
                    this.txtEmail.Text = _userEntity.Email;
                    this.txtOfficePhone.Text = _userEntity.OfficePhone;
                    this.txtMobilePhone.Text = _userEntity.MobilePhone;
                    this.txtRemark.Text = _userEntity.Remark;
                    _userEntity.IsInitializeFinished = true;


                }
            }
            else
            {
                CtrlState = ControlState.New;
            }
        }
        #endregion

        #region UI Events
        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            tsbAddUser.Enabled = true;
            tsbSearch.Enabled = true;
            switch (state)
            {
                #region case state of editer
                case ControlState.Edit:
                    tsbSave.Enabled = true;
                    tsbDeleteUser.Enabled = true;
                    tsbDistributeRole.Enabled = true;
                    tsbPrivilege.Enabled = true;
                    txtBadge.Properties.ReadOnly = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    tsbDeleteUser.Enabled = false;
                    tsbSave.Enabled = true;
                    tsbDistributeRole.Enabled = false;
                    tsbPrivilege.Enabled = false;

                    this.txtUserName.Text = string.Empty;
                    this.txtBadge.Text = string.Empty;
                    this.txtEmail.Text = string.Empty;
                    this.txtMobilePhone.Text = string.Empty;
                    this.txtOfficePhone.Text = string.Empty;
                    this.txtPsw.Text = string.Empty;
                    this.txtPswAffirm.Text = string.Empty;
                    this.txtRemark.Text = string.Empty;
                    txtBadge.Properties.ReadOnly = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    tsbDeleteUser.Enabled = false;
                    tsbSave.Enabled = false;
                    tsbDistributeRole.Enabled = false;
                    tsbPrivilege.Enabled = false;
                    txtBadge.Properties.ReadOnly = true;
                    break;
                    #endregion
            }
        }

        private void UserManagementCtrl_Load(object sender, EventArgs e)
        {
            InitUIByCulture();
        }

        public void InitUIByCulture()
        {
            #region InitUIResourcesByCulture
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}");
            this.lblUserName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblUserName}");
            this.lblBadge.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblBadge}");
            this.lblPsw.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblPassword}");
            this.lblPswAffirm.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblPasswordAffirm}");
            this.lblEmail.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblEmail}");
            this.lblMobilePhone.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblMobilePhone}");
            this.lblOfficePhone.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblOfficePhone}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            //this.layoutControlGroupRemark.Text = StringParser.Parse("${res:Global.Remark}");

            this.tsbAddUser.Text = StringParser.Parse("${res:Global.New}");
            this.tsbDeleteUser.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbDistributeRole.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.tsbDistributeRole}");
            this.tsbPrivilege.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.tsbDistributePrivilege}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbSearch.Text = StringParser.Parse("${res:Global.Query}");
            //this.layoutControlGroupBase.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.UserGroupName}");
            #endregion
        }
        #endregion

        #region Control Events
        /// <summary>
        /// 点击查询按钮打开查询页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSearch_Click(object sender, EventArgs e)
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

            //User userEntity = new User();
            //UserSearchDialog userSearch = new UserSearchDialog();
            //if (DialogResult.OK == userSearch.ShowDialog())
            //{
            //    userEntity = userSearch.userEntity;
            //    if (userEntity.UserKey != "")
            //    {
            //        string title = userEntity.UserName;
            //        //遍历查询已打开标签是否存在查询的页面存在就选中并Return
            //        foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //        {
            //            if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}") + "-" + title)
            //            {
            //                viewContent.WorkbenchWindow.SelectWindow();
            //                return;
            //            }
            //        }
            //        UserManageViewContent userViewContent = new UserManageViewContent(title, userEntity); //新建一个标签
            //        WorkbenchSingleton.Workbench.ShowView(userViewContent);                               //在工作台上显示
            //    }
            //}
        }

        /// <summary>
        /// 点击新增按钮重新遍历页面标签并重新加载一个用户管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddUser_Click(object sender, EventArgs e)
        {
            //对界面标签进行遍历查找查找到需要打开标前存在则激活该标签否则重新添加一个标签
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {

                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    UserManagementCtrl ctrl = (UserManagementCtrl)viewContent.Control.Controls.Find("UserManagementCtrl", true)[0];
                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }
            UserManageViewContent userManageViewContent = new UserManageViewContent("", new User());//创建一个新的视图页面
            WorkbenchSingleton.Workbench.ShowView(userManageViewContent);                           //视图页面
        }

        /// <summary>
        /// Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            bool isNew = (CtrlState == ControlState.New);
            if (CheckData())
            {
                _userEntity.OfficePhone = this.txtOfficePhone.Text.Trim();
                _userEntity.MobilePhone = this.txtMobilePhone.Text.Trim();
                _userEntity.Remark = this.txtRemark.Text;
                _userEntity.Email = this.txtEmail.Text;
                _userEntity.UserName = this.txtUserName.Text;
                _userEntity.Badge = this.txtBadge.Text.Trim();
                if (_userEntity.UserKey == "")
                {
                    _userEntity.UserKey = CommonUtils.GenerateNewKey(0);
                    string passwordGUID = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPsw.Text.Trim(), "SHA1");
                    _userEntity.Password = passwordGUID;
                    _userEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _userEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    isNew = true;
                }
                else
                {
                    _userEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _userEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    if (txtPsw.Text == _userEntity.Password)
                    {
                        _userEntity.Password = _userEntity.Password;
                    }
                    else
                    {
                        string passwordGUID = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPsw.Text.Trim(), "SHA1");
                        _userEntity.Password = passwordGUID;
                    }
                }
                if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    _userEntity.SaveUser(isNew);
                    if (_userEntity.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        if (isNew)
                        {
                            WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                             = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}") + "-" + _userEntity.UserName;
                            CtrlState = ControlState.Edit;
                        }
                        _userEntity.ResetDirtyList();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _userEntity.ErrorMsg);
                    }
                }
                tsbSearch_Click(null, null);
                txtUserName.Text = "";
                txtBadge.Text = "";
                txtPsw.Text = "";
                txtPswAffirm.Text = "";
                txtMobilePhone.Text = "";
                txtOfficePhone.Text = "";
                txtEmail.Text = "";
                txtRemark.Text = "";
            }

        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteUser_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                _userEntity.DeleteUser();
                if (_userEntity.ErrorMsg != "")
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}");
                }
                else
                {
                    //CtrlState = ControlState.New;                   
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}", "${res:Global.SystemInfo}");
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementViewContent.TitleName}");
                    CtrlState = ControlState.New;
                }
            }
        }

        /// <summary>
        /// Distribute role of user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDistributeRole_Click(object sender, EventArgs e)
        {
            DistributeRoleDialog distributeRole = new DistributeRoleDialog(_userEntity);
            distributeRole.ShowDialog();
        }

        /// <summary>
        /// Distribute privilege of user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrivilege_Click(object sender, EventArgs e)
        {
            string type = RBAC_USER_FIELDS.DATABASE_TABLE_NAME;
            DistributePrivilege distributePrivilege = new DistributePrivilege(type, _userEntity.UserName, _userEntity.UserKey);
            distributePrivilege.ShowDialog();
        }


        /// <summary>
        /// Get user's detail information 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBadge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string badge = this.txtBadge.Text.Trim();
                try
                {
                    IDictionary userInfo = ADValidate.GetDirectoryEntryByAccount(badge);
                    if (userInfo != null)
                    {
                        //this.txtBadge.Text = userInfo["samaccountname"].ToString();
                        this.txtUserName.Text = userInfo["name"].ToString();
                        this.txtEmail.Text = userInfo["mail"].ToString();
                        this.txtOfficePhone.Text = userInfo["telephonenumber"].ToString();
                        if (userInfo["mobile"] != null)
                        {
                            this.txtMobilePhone.Text = userInfo["mobile"].ToString();
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.UserNotExist}", "${res:Global.SystemInfo}");
                        CleanData();
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex);
                }
            }
        }
        #endregion

        #region Private Methods
        private bool CheckData()
        {
            Regex reMail = new Regex("^[0-9a-z][a-z0-9._-]{1,}@[a-z0-9-]{1,}[a-z0-9].[a-z.]{1,}[a-z]$");
            Regex reMobilePhone = new Regex("^0{0,1}(13[0-9]|15[0-9]|18[0-9])[0-9]{8}$");
            if (this.txtUserName.Text == string.Empty)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.UserNameIsNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (this.txtPsw.Text == string.Empty)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.PasswordIsNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (this.txtBadge.Text == string.Empty)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.BadgeIsNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (this.txtPswAffirm.Text == string.Empty)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.PasswordAffirmIsNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (this.txtPsw.Text.Trim() != this.txtPswAffirm.Text.Trim())
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.Msg.PasswordError}", "${res:Global.SystemInfo}");
                return false;
            }
            if (txtEmail.Text.Length > 0 && !reMail.IsMatch(this.txtEmail.Text))
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.Msg.EmailError}", "${res:Global.SystemInfo}");
                return false;
            }
            if (txtMobilePhone.Text != "")
            {
                if (this.txtMobilePhone.Text.Length > 11 || !reMobilePhone.IsMatch(this.txtMobilePhone.Text))
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.MobilePhoneError}", "${res:Global.SystemInfo}");
                    return false;
                }
            }
            return true;
        }

        private void CleanData()
        {
            this.txtUserName.Text = "";
            this.txtRemark.Text = "";
            this.txtPswAffirm.Text = "";
            this.txtPsw.Text = "";
            this.txtOfficePhone.Text = "";
            this.txtMobilePhone.Text = "";
            this.txtEmail.Text = "";
        }
        #endregion
        private void userView_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = userView.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                if (_userEntity == null)
                {
                    _userEntity = new User();
                }
                _userEntity.UserKey = userView.GetRowCellValue(rowHandle, this.USER_KEY).ToString();
                _userEntity.UserName = userView.GetRowCellValue(rowHandle, this.USERNAME).ToString();


                _userEntity.Badge = userView.GetRowCellValue(rowHandle, this.BADGE).ToString();
                _userEntity.Password = userView.GetRowCellValue(rowHandle, this.PASSWORD).ToString();
                _userEntity.Email = userView.GetRowCellValue(rowHandle, this.EMAIL).ToString();
                _userEntity.OfficePhone = userView.GetRowCellValue(rowHandle, this.OFFICE_PHONE).ToString();
                _userEntity.MobilePhone = userView.GetRowCellValue(rowHandle, this.MOBILE_PHONE).ToString();
                _userEntity.Remark = userView.GetRowCellValue(rowHandle, this.REMARK).ToString();

                this.txtUserName.Text = _userEntity.UserName;
                this.txtBadge.Text = _userEntity.Badge;
                this.txtPsw.Text = _userEntity.Password;
                this.txtPswAffirm.Text = this.txtPsw.Text;
                this.txtEmail.Text = _userEntity.Email;
                this.txtOfficePhone.Text = _userEntity.OfficePhone;
                this.txtMobilePhone.Text = _userEntity.MobilePhone;
                this.txtRemark.Text = _userEntity.Remark;
                _userEntity.IsInitializeFinished = true;
                CtrlState = ControlState.Edit;
            }
        }

    }
}

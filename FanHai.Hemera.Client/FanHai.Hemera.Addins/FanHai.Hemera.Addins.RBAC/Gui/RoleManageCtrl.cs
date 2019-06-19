using FanHai.Gui.Core;
using FanHai.Hemera.Addins.RBAC.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 显示角色管理界面的用户控件类。
    /// </summary>
    public partial class RoleManageCtrl : BaseUserCtrl
    {
        private Role _roleEntity = null;
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
        //Define and initialize control state
        private ControlState _ctrlState;

        public RoleManageCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            CtrlState = ControlState.Empty;
        }

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
            tsbAddRole.Enabled = true;
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtRoleName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtRemark.Text = string.Empty;

                    txtRoleName.Properties.ReadOnly = true;
                    txtDescription.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;

                    tsbSaveRole.Enabled = false;
                    tsbDeleteRole.Enabled = false;
                    tsbPrivilege.Enabled = false;
                    tsbContentPrivilege.Enabled = false;
                    tsbOperationPrivilege.Enabled = false;
                    tsbStorePrivilege.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtRoleName.Properties.ReadOnly = false;
                    txtDescription.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;

                    tsbSaveRole.Enabled = true;
                    tsbDeleteRole.Enabled = true;
                    tsbPrivilege.Enabled = true;
                    tsbContentPrivilege.Enabled = true;
                    tsbOperationPrivilege.Enabled = true;
                    tsbStorePrivilege.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtRoleName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtRemark.Text = string.Empty;

                    txtRoleName.Properties.ReadOnly = false;
                    txtDescription.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;

                    tsbSaveRole.Enabled = true;
                    tsbDeleteRole.Enabled = false;
                    tsbPrivilege.Enabled = false;
                    tsbContentPrivilege.Enabled = false;
                    tsbOperationPrivilege.Enabled = false;
                    tsbStorePrivilege.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    txtRoleName.Properties.ReadOnly = true;
                    txtDescription.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;

                    tsbSaveRole.Enabled = false;
                    tsbDeleteRole.Enabled = false;
                    tsbPrivilege.Enabled = false;
                    tsbContentPrivilege.Enabled = false;
                    tsbOperationPrivilege.Enabled = false;
                    tsbStorePrivilege.Enabled = false;
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 角色新增
        /// </summary>
        private void tsbAddRole_Click(object sender, EventArgs e)
        {
            _roleEntity = new Role(CommonUtils.GenerateNewKey(0));
            CtrlState = ControlState.New;
        }
        /// <summary>
        /// 删除选中的角色
        /// </summary>
        private void tsbDeleteRole_Click(object sender, EventArgs e)
        {
            bool delete = false;
            DataSet ds = new DataSet();
            ds = _roleEntity.GetUsersOfRole();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${FanHai.Hemera.Addins.RBAC.RoleManageCtrl.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    delete = true;
                }
            }
            else
            {
                if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    delete = true;
                }
            }
            if (delete)
            {
                _roleEntity.DeleteRole();
                if (_roleEntity.ErrorMsg == "")
                {
                    GridDataBind();
                    CtrlState = ControlState.Empty;
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}");
                }
            }
        }
        /// <summary>
        /// 保存新建或修改角色
        /// </summary>
        private void tsbSaveRole_Click(object sender, EventArgs e)
        {
            //判断是否有用户信息                                      modify  yongbing.yang
            if (null == _roleEntity)
            {
                throw (new Exception("Error Role Manage Set"));
            }
            // TODO: Data validation   
            //用角色名称是否为空为空弹出对话框提示不能为空            modify  yongbing.yang
            if (this.txtRoleName.Text == "")
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.Msg.RoleIsNull}", "${res:Global.SystemInfo}");
                return;
            }
            else
            {
                //弹出对话框提示是否确认保存，确认向下执行            modify  yongbing.yang
                if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"),
                    StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    _roleEntity.RoleName = this.txtRoleName.Text;
                    _roleEntity.Description = this.txtDescription.Text;
                    _roleEntity.Remark = this.txtRemark.Text;
                    _roleEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _roleEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    _roleEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _roleEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    bool bNew = (CtrlState == ControlState.New);
                    _roleEntity.SaveRole(bNew);
                    //判断操作是否成功，true成功                      modify  yongbing.yang
                    if (_roleEntity.ErrorMsg == "")
                    {
                        GridDataBind();  //绑定GridView数据信息       modify  yongbing.yang
                        CtrlState = ControlState.ReadOnly;
                        _roleEntity.ResetDirtyList();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _roleEntity.ErrorMsg);
                    }
                }
            }
        }

        private void RoleManageCtrl_Load(object sender, EventArgs e)
        {
            InitUIResourcesByCulture();
            GridDataBind();
            GridViewHelper.SetGridView(roleView);
        }

        protected override void InitUIResourcesByCulture()
        {
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageViewContent.TitleName}");  
            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbAddRole.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSaveRole.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbDeleteRole.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbPrivilege.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.tsbDistributePrivilege}");
            this.tsbContentPrivilege.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.tsbLine}");
            this.tsbOperationPrivilege.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.tsbOperation}");
            this.lblRoleName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.lblRoleName}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");

            this.RoleGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageViewContent.TitleName}");
            this.ROLE_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.lblRoleName}");
            this.DESCRIPTIONS.Caption = StringParser.Parse("${res:Global.Description}");
            this.REMARK.Caption = StringParser.Parse("${res:Global.Remark}");

            tsbUser.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.tsbUser}");
            tsbStorePrivilege.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.tsbStorePrivilege}");
            tsbEmsStatus.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.tsbEmsStatus}");
        }

        /// <summary>
        /// 把数据绑定到GridView数据视图
        /// </summary>
        public void GridDataBind()
        {
            Role roleEntity = new Role();
            DataSet ds = roleEntity.GetRoleInfo();
            this.roleControl.DataSource = ds.Tables[0];
        }

        /// <summary>
        /// 点击工作台的GridView的行信息获取该角色信息
        /// 返回到页面标签栏
        /// </summary>
        private void roleView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                CtrlState = ControlState.Edit;
                _roleEntity = new Role();
                _roleEntity.RoleKey = this.roleView.GetRowCellValue(e.RowHandle, this.ROLE_KEY).ToString();
                this.txtRoleName.Text = this.roleView.GetRowCellValue(e.RowHandle, this.ROLE_NAME).ToString();
                this.txtDescription.Text = this.roleView.GetRowCellValue(e.RowHandle, this.DESCRIPTIONS).ToString();
                this.txtRemark.Text = this.roleView.GetRowCellValue(e.RowHandle, this.REMARK).ToString();

                _roleEntity.RoleName = this.txtRoleName.Text;
                _roleEntity.Description = this.txtDescription.Text;
                _roleEntity.Remark = this.txtRemark.Text;
                _roleEntity.Editor = this.roleView.GetRowCellValue(e.RowHandle, editor).ToString();
                _roleEntity.EditTimeZone = this.roleView.GetRowCellValue(e.RowHandle, edit_timezone).ToString();
                _roleEntity.IsInitializeFinished = true;
            }
        }

        /// <summary>
        /// 对选中角色进行权限分配
        /// </summary>
        private void tsbPrivilege_Click(object sender, EventArgs e)
        {
            string type = RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME;
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();
            DistributePrivilege distributePrivilege = new DistributePrivilege(type, roleName, roleKey);
            distributePrivilege.ShowDialog();
        }

        /// <summary>
        /// 对选中角色进行线别分配
        /// </summary>
        private void tsbContentPrivilege_Click(object sender, EventArgs e)
        {
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();
            LinePrivilege linePrivilege = new LinePrivilege(roleKey, roleName);
            linePrivilege.ShowDialog();
        }

        /// <summary>
        /// 对选中角色进行分配工序
        /// </summary>
        private void tsbOperationPrivilege_Click(object sender, EventArgs e)
        {
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();

            OperationPrivilege operationPrivilege = new OperationPrivilege(roleKey, roleName);
            operationPrivilege.ShowDialog();
        }

        /// <summary>
        /// 刷新GridView的数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            GridDataBind();
        }

        /// <summary>
        /// 对选中角色分配线边舱
        /// </summary>
        private void tsbStorePrivilege_Click(object sender, EventArgs e)
        {
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();
            StorePrivilegeDialog storePrivilege = new StorePrivilegeDialog(roleKey, roleName);
            storePrivilege.ShowDialog();
        }

        /// <summary>
        /// 对角色进行人员的分配
        /// </summary>
        private void tsbUser_Click(object sender, EventArgs e)
        {
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();
            RoleUserDialog roleUserDialog = new RoleUserDialog(roleKey, roleName);
            roleUserDialog.ShowDialog();
        }

        /// <summary>
        /// 对选中的角色分配机台状态权限
        /// </summary>
        private void tsbEmsStatus_Click(object sender, EventArgs e) // add by qym 20120606 18:43 Q.001
        {
            //roleView.FocusedRowHandle代表当前焦点在的行，ROLE_NAME是列名
            //GetRowCellValue得到当前行某列的值
            string roleName = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_NAME).ToString();
            string roleKey = roleView.GetRowCellValue(roleView.FocusedRowHandle, ROLE_KEY).ToString();
            StatusPrivilegeDialog StatusPrivilege = new StatusPrivilegeDialog(roleKey, roleName);
            StatusPrivilege.ShowDialog();
        }
        private void roleView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

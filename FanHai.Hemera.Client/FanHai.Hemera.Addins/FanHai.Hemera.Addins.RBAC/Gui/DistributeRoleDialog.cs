using System;
using System.Collections;
using System.Collections.Generic;
/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class DistributeRoleDialog : BaseDialog
    {
        private User _userEntity = null;
        private DataTable userOwnRoleTable = new DataTable();
        private DataTable systemRoleTable = new DataTable();
        private Role _roleEntity = new Role();
        public DistributeRoleDialog()
        {
            InitializeComponent();
        }
        public DistributeRoleDialog(User userEntity)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributeRoleDialog.Title}"))
        {
            InitializeComponent();
            _userEntity = userEntity;
            this.txtUserName.Text = _userEntity.UserName;
            this.txtUserName.Properties.ReadOnly = true;           
        }

        private void DistributeRoleDialog_Load(object sender, EventArgs e)
        {
            InitUI();
            ListSystemRoleBindData();
            ListOwnRoleBindData();  
        }
        private void InitUI()
        {
            this.lblUserName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.UserManagementCtrl.lblUserName}");           
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CloseButtonText}");
            this.selectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributeRoleDialog.lblSelectRole}");
            this.UnselectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributeRoleDialog.lblUnSelectRole}");
        }
        private void ListSystemRoleBindData()
        {
            if (clbSystemRole.Items.Count > 0)
            {
                clbSystemRole.Items.Clear();
            }
            DataSet dsSystemRole = new DataSet();
            dsSystemRole = _userEntity.GetRoleNotBelongToUser();
            if (_userEntity.ErrorMsg == "")
            {
                systemRoleTable = dsSystemRole.Tables[0];
                List<Role> roleList = new List<Role>();                
                for (int i = 0; i < dsSystemRole.Tables[0].Rows.Count; i++)
                {
                    Role roleEntity = new Role();
                    roleEntity.RoleKey = dsSystemRole.Tables[0].Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_KEY].ToString();
                    roleEntity.RoleName = dsSystemRole.Tables[0].Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_NAME].ToString();                   
                    roleList.Add(roleEntity);                  
                }
                clbSystemRole.Items.AddRange(roleList.ToArray());               
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }
        private void ListOwnRoleBindData()
        {
            if (clbOwnRole.Items.Count > 0)
            {
                clbOwnRole.Items.Clear();
            }
            User userEntity = new User();
            DataSet dsUserInRole = new DataSet();
            userEntity.UserKey = _userEntity.UserKey;
            dsUserInRole = userEntity.GetRolesOfUser();
            if (userEntity.ErrorMsg == "")
            {
                userOwnRoleTable = dsUserInRole.Tables[0];
                List<Role> roleList = new List<Role>();       
                for (int i = 0; i < dsUserInRole.Tables[0].Rows.Count; i++)
                {
                    Role roleEntity = new Role();
                    roleEntity.RoleKey = dsUserInRole.Tables[0].Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_KEY].ToString();
                    roleEntity.RoleName = dsUserInRole.Tables[0].Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_NAME].ToString();
                    roleList.Add(roleEntity);
                }
                clbOwnRole.Items.AddRange(roleList.ToArray());
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {  
            if (this.clbSystemRole.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {                
                foreach ( object o in this.clbSystemRole.CheckedItems)                
                {
                    clbOwnRole.Items.Add(o);
                }
                for (int i = clbSystemRole.Items.Count - 1; i >= 0; i--)
                {
                    if (clbSystemRole.CheckedItems.Contains(clbSystemRole.Items[i]))
                    {
                        clbSystemRole.Items.Remove(clbSystemRole.Items[i]); 
                    }
                }
            } 
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            if (clbOwnRole.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in clbOwnRole.CheckedItems)
                {
                    clbSystemRole.Items.Add(o);
                }
                for (int i = clbOwnRole.Items.Count - 1; i >= 0; i--)
                {
                    if (clbOwnRole.CheckedItems.Contains(clbOwnRole.Items[i]))
                    {
                        clbOwnRole.Items.Remove(clbOwnRole.Items[i]);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                string msg = string.Empty;
                DataSet dsSave = new DataSet();
                DataTable dataTable = CreateDataTable();

                User userEntity = new User();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                for (int i = 0; i < clbOwnRole.Items.Count; i++)
                {
                    int j;
                    for (j = 0; j < userOwnRoleTable.Rows.Count; j++)
                    {
                        if (((Role)clbOwnRole.Items[i]).RoleKey.ToString() == userOwnRoleTable.Rows[j][RBAC_ROLE_FIELDS.FIELD_ROLE_KEY].ToString())
                            break;
                    }
                    if (j == userOwnRoleTable.Rows.Count)
                    {

                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,((Role)clbOwnRole.Items[i]).RoleKey.ToString()},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,_userEntity.UserKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }

                for (int i = 0; i < userOwnRoleTable.Rows.Count; i++)
                {
                    int j;
                    for (j = 0; j < clbOwnRole.Items.Count; j++)
                    {
                        if (userOwnRoleTable.Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_KEY].ToString() == ((Role)clbOwnRole.Items[j]).RoleKey.ToString())
                        {
                            break;
                        }
                    }
                    if (j == clbOwnRole.Items.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,userOwnRoleTable.Rows[i][RBAC_ROLE_FIELDS.FIELD_ROLE_KEY].ToString()},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,_userEntity.UserKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                    userEntity.SaveUserRole(dsSave);
                    if (userEntity.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        ListSystemRoleBindData();
                        ListOwnRoleBindData();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.DistributeRoleDialog.NoDataUpdate}");
                }
            }

        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_USER_IN_ROLE_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

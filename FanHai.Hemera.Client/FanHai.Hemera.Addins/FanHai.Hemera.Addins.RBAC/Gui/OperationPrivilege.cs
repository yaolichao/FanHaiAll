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

using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class OperationPrivilege : BaseDialog
    {
        private Privilege privilegeEntity = null;
        private DataTable selectOperationTable = new DataTable();
        public OperationPrivilege()
        {
            InitializeComponent();
        }

        public OperationPrivilege(string roleKey, string roleName)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationPrivilege.Title}"))
        {
            InitializeComponent();
            privilegeEntity = new Privilege();
            privilegeEntity.RoleKey = roleKey;
            this.txtRoleName.Text = roleName;
            this.txtRoleName.Properties.ReadOnly = true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (clbSelectOperation.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in clbSelectOperation.CheckedItems)
                {
                    clbUnSelectOperation.Items.Add(o);  
                }
                for (int i = clbSelectOperation.Items.Count - 1; i >= 0; i--)
                {
                    if (clbSelectOperation.CheckedItems.Contains(clbSelectOperation.Items[i]))
                    {
                        clbSelectOperation.Items.Remove(clbSelectOperation.Items[i]);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.clbUnSelectOperation.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in this.clbUnSelectOperation.CheckedItems)
                {
                    clbSelectOperation.Items.Add(o);
                }
                for (int i = clbUnSelectOperation.Items.Count - 1; i >= 0; i--)
                {
                    if (clbUnSelectOperation.CheckedItems.Contains(clbUnSelectOperation.Items[i]))
                    {
                        clbUnSelectOperation.Items.Remove(clbUnSelectOperation.Items[i]);
                    }
                }
            } 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}","${res:Global.SystemInfo}"))
            {
                DataSet dsSave = new DataSet();
                DataTable dataTable = CreateDataTable();
                Privilege privilege = new Privilege();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                for (int i = 0; i < clbSelectOperation.Items.Count; i++)
                {
                    int j;
                    for (j = 0; j < selectOperationTable.Rows.Count; j++)
                    {
                        if (clbSelectOperation.Items[i].ToString() == selectOperationTable.Rows[j][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString())
                            break;
                    }
                    if (j == selectOperationTable.Rows.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME,clbSelectOperation.Items[i].ToString()},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                for (int i = 0; i < selectOperationTable.Rows.Count; i++)
                {
                    int k;
                    for (k = 0; k < clbSelectOperation.Items.Count; k++)
                    {
                        if (selectOperationTable.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString() == clbSelectOperation.Items[k].ToString())
                            break;
                    }
                    if (k == clbSelectOperation.Items.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME,selectOperationTable.Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString()},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                    privilege.SaveOperationsOfRole(dsSave);
                    if (privilege.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        BindDataToUnSelectOperation();
                        BindDataToSelectOperation();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.OperationPrivilege.NoDataUpdate}");
                }
            }
        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME,
                                                        RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDITOR,
                                                        RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
                                                       };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_ROLE_OWN_OPERATION_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OperationPrivilege_Load(object sender, EventArgs e)
        {
            InitUIResourcesByCulture();
            BindDataToUnSelectOperation();
            BindDataToSelectOperation();
        }
        private void InitUIResourcesByCulture()
        {
            this.lblRoleName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.lblRoleName}");           
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
            this.btnClose.Text = StringParser.Parse("${res:Global.CloseButtonText}");
            this.selectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationPrivilege.lblSelectOperation}");
            this.UnSelectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationPrivilege.lblUnSelectOperation}");
        }
        private void BindDataToUnSelectOperation()
        {
            if (clbUnSelectOperation.Items.Count > 0)
            {
                clbUnSelectOperation.Items.Clear();
            }
            DataSet dsUnSelectOperation = new DataSet();
            dsUnSelectOperation = privilegeEntity.GetOperationRoleNotOwn();
            if (privilegeEntity.ErrorMsg == "")
            {
                if (dsUnSelectOperation.Tables.Count > 0)
                {
                    for (int i = 0; i < dsUnSelectOperation.Tables[0].Rows.Count; i++)
                    {
                        string operationName = dsUnSelectOperation.Tables[0].Rows[i]["ROUTE_OPERATION_NAME"].ToString();
                        clbUnSelectOperation.Items.Add(operationName);
                    }
                }
            }
            else
            {                
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }
        private void BindDataToSelectOperation()
        {
            if (clbSelectOperation.Items.Count > 0)
            {
                clbSelectOperation.Items.Clear();
            }
            DataSet dsSelectOperation = new DataSet();
            dsSelectOperation = privilegeEntity.GetOperationOwnedByRole();
            if (privilegeEntity.ErrorMsg == "")            {

                if (dsSelectOperation.Tables.Count > 0)
                {
                    selectOperationTable = dsSelectOperation.Tables[0];
                    for (int i = 0; i < dsSelectOperation.Tables[0].Rows.Count; i++)
                    {
                        string operationName=dsSelectOperation.Tables[0].Rows[i][RBAC_ROLE_OWN_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString();
                        clbSelectOperation.Items.Add(operationName);
                    }
                }
            }
        }
    }
}

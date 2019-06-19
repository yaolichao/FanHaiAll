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
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class LinePrivilege : BaseDialog
    {
        private Privilege privilegeEntity = null;
        private DataTable selectLineTable = new DataTable();
        public LinePrivilege()
        {
            InitializeComponent();
        }
        public LinePrivilege(string roleKey, string roleName)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.LinePrivilege.Title}"))
        {
            InitializeComponent();
            privilegeEntity = new Privilege();
            privilegeEntity.RoleKey = roleKey;
            this.txtRoleName.Text = roleName;
            this.txtRoleName.Properties.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                DataSet dsSave = new DataSet();
                DataTable dataTable = CreateDataTable();
                Privilege privilege = new Privilege();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                for (int i = 0; i < clbSelectLine.Items.Count; i++)
                {
                    int j;
                    for (j = 0; j < selectLineTable.Rows.Count; j++)
                    {
                        if (clbSelectLine.Items[i].ToString() == selectLineTable.Rows[j][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString())
                            break;
                    }
                    if (j == selectLineTable.Rows.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME,clbSelectLine.Items[i].ToString()},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                for (int i = 0; i < selectLineTable.Rows.Count; i++)
                {
                    int k;
                    for (k = 0; k < clbSelectLine.Items.Count; k++)
                    {
                        if (selectLineTable.Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString() == clbSelectLine.Items[k].ToString())
                            break;
                    }
                    if (k == clbSelectLine.Items.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME,selectLineTable.Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString()},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                    privilege.SaveLines(dsSave);
                    if (privilege.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        BindDataToClbUnSelectLine();
                        BindDataToClbSelectLine();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.LinePrivilege.NoDataUpdate}");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_ROLE_OWN_LINES_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_ROLE_OWN_LINES_FIELDS.FIELD_LINE_NAME,
                                                        RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDITOR,
                                                        RBAC_ROLE_OWN_LINES_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_ROLE_OWN_LINES_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            if (clbSelectLine.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in clbSelectLine.CheckedItems)
                {
                    clbUnSelectLine.Items.Add(o);
                }
                for (int i = clbSelectLine.Items.Count - 1; i >= 0; i--)
                {
                    if (clbSelectLine.CheckedItems.Contains(clbSelectLine.Items[i]))
                    {
                        clbSelectLine.Items.Remove(clbSelectLine.Items[i]);
                    }
                }
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            if (this.clbUnSelectLine.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in this.clbUnSelectLine.CheckedItems)
                {
                     clbSelectLine.Items.Add(o);
                }
                for (int i = clbUnSelectLine.Items.Count - 1; i >= 0; i--)
                {
                    if (clbUnSelectLine.CheckedItems.Contains(clbUnSelectLine.Items[i]))
                    {
                        clbUnSelectLine.Items.Remove(clbUnSelectLine.Items[i]);
                    }
                }
            } 
        }

        private void ContentPrivilege_Load(object sender, EventArgs e)
        {
            InitUI();
            BindDataToClbUnSelectLine();
            BindDataToClbSelectLine();
        }
        private void InitUI()
        {
            this.lblRoleName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.RoleManageCtrl.lblRoleName}");           
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
            this.btnClose.Text = StringParser.Parse("${res:Global.CloseButtonText}");
            this.selectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.LinePrivilege.lblSelectLine}");
            this.UnSelectGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.LinePrivilege.lblUnSelectLine}");
        }
        private void BindDataToClbUnSelectLine()
        {
            if (clbUnSelectLine.Items.Count > 0)
            {
                clbUnSelectLine.Items.Clear();
            }
            DataSet dsUnSelectLine = new DataSet();
            dsUnSelectLine = privilegeEntity.GetLinesRoleNotOwn();
            if (privilegeEntity.ErrorMsg == "")
            {
                if (dsUnSelectLine.Tables.Count > 0)
                {
                    for (int i = 0; i < dsUnSelectLine.Tables[0].Rows.Count; i++)
                    {
                        string lineName = dsUnSelectLine.Tables[0].Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();                       
                        clbUnSelectLine.Items.Add(lineName);
                    }
                }               
            }
            else
            {                
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }

        private void BindDataToClbSelectLine()
        {
            if (clbSelectLine.Items.Count > 0)
            {
                clbSelectLine.Items.Clear();
            }
            DataSet dsSelectLine = new DataSet();
            dsSelectLine = privilegeEntity.GetLinesOwnedByRole();
            if (privilegeEntity.ErrorMsg == "")
            {
                if (dsSelectLine.Tables.Count > 0)
                {
                    selectLineTable = dsSelectLine.Tables[0];
                    for (int i = 0; i < dsSelectLine.Tables[0].Rows.Count; i++)
                    {
                        string lineName=dsSelectLine.Tables[0].Rows[i][FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME].ToString();
                        clbSelectLine.Items.Add(lineName);
                    }
                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }
    }
}

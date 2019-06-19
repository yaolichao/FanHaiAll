using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class StorePrivilegeDialog : BaseDialog
    {
        private Privilege privilegeEntity = null;
        private DataTable selectStoreTable = new DataTable();
        public StorePrivilegeDialog()
        {
            InitializeComponent();
        }

        public StorePrivilegeDialog(string roleKey, string roleName)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.StorePrivilegeDialog.Title}"))
        {
            InitializeComponent();
            privilegeEntity = new Privilege();
            privilegeEntity.RoleKey = roleKey;
            this.txtRoleName.Text = roleName;
            this.txtRoleName.Properties.ReadOnly = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (clbSelectStore.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in clbSelectStore.CheckedItems)
                {
                    clbUnSelectStore.Items.Add(o);
                }
                for (int i = clbSelectStore.Items.Count - 1; i >= 0; i--)
                {
                    if (clbSelectStore.CheckedItems.Contains(clbSelectStore.Items[i]))
                    {
                        clbSelectStore.Items.Remove(clbSelectStore.Items[i]);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.clbUnSelectStore.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in this.clbUnSelectStore.CheckedItems)
                {
                    clbSelectStore.Items.Add(o);
                }
                for (int i = clbUnSelectStore.Items.Count - 1; i >= 0; i--)
                {
                    if (clbUnSelectStore.CheckedItems.Contains(clbUnSelectStore.Items[i]))
                    {
                        clbUnSelectStore.Items.Remove(clbUnSelectStore.Items[i]);
                    }
                }
            }
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

                for (int i = 0; i < clbSelectStore.Items.Count; i++)
                {
                    int j;
                    for (j = 0; j < selectStoreTable.Rows.Count; j++)
                    {
                        if (clbSelectStore.Items[i].ToString() == selectStoreTable.Rows[j][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString())
                            break;
                    }
                    if (j == selectStoreTable.Rows.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME,clbSelectStore.Items[i].ToString()},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                for (int i = 0; i < selectStoreTable.Rows.Count; i++)
                {
                    int k;
                    for (k = 0; k < clbSelectStore.Items.Count; k++)
                    {
                        if (selectStoreTable.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString() == clbSelectStore.Items[k].ToString())
                            break;
                    }
                    if (k == clbSelectStore.Items.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME,selectStoreTable.Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString()},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                    privilege.SaveStoreOfRole(dsSave);
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
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.StorePrivilegeDialog.NoDataUpdate}"));
                }
            }
        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() {  
                                                        RBAC_ROLE_OWN_STORE_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME,
                                                        RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDITOR,
                                                        RBAC_ROLE_OWN_STORE_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
                                                       };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_ROLE_OWN_STORE_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BindDataToUnSelectOperation()
        {
            if (clbUnSelectStore.Items.Count > 0)
            {
                clbUnSelectStore.Items.Clear();
            }
            DataSet dsUnSelectStore = new DataSet();
            dsUnSelectStore = privilegeEntity.GetStoreOfRole("UNSELECT");
            if (privilegeEntity.ErrorMsg == "")
            {
                if (dsUnSelectStore.Tables.Count > 0)
                {
                    for (int i = 0; i < dsUnSelectStore.Tables[0].Rows.Count; i++)
                    {
                        string operationName = dsUnSelectStore.Tables[0].Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                        clbUnSelectStore.Items.Add(operationName);
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
            if (clbSelectStore.Items.Count > 0)
            {
                clbSelectStore.Items.Clear();
            }
            DataSet dsSelectStore = new DataSet();
            dsSelectStore = privilegeEntity.GetStoreOfRole("SELECT");
            if (privilegeEntity.ErrorMsg == "")
            {

                if (dsSelectStore.Tables.Count > 0)
                {
                    selectStoreTable = dsSelectStore.Tables[0];
                    for (int i = 0; i < dsSelectStore.Tables[0].Rows.Count; i++)
                    {
                        string operationName = dsSelectStore.Tables[0].Rows[i][RBAC_ROLE_OWN_STORE_FIELDS.FIELD_STORE_NAME].ToString();
                        clbSelectStore.Items.Add(operationName);
                    }
                }
            }
        }

        private void StorePrivilegeDialog_Load(object sender, EventArgs e)
        {
            BindDataToUnSelectOperation();
            BindDataToSelectOperation();
        }
    }
}

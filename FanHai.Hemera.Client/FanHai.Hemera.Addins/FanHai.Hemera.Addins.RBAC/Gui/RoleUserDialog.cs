using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class RoleUserDialog : BaseDialog
    {
        private Privilege privilegeEntity = null;
        private DataTable oldSelectTable = new DataTable();
        public RoleUserDialog()
        {
            InitializeComponent();
        }
        //获取roleKey、roleName并对页面进行初始化
        public RoleUserDialog(string roleKey,string roleName)
            : base("分配用户")
        {
            InitializeComponent();
            privilegeEntity = new Privilege();
            privilegeEntity.RoleKey = roleKey;            
            this.txtRoleName.Text = roleName;
            this.txtRoleName.Properties.ReadOnly = true;
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DataTable deleteTable = gcSelect.DataSource as DataTable;
            DataTable addTable = gcUnSelect.DataSource as DataTable;
            DataRow[] dataRows= deleteTable.Select("COLUMN_CHECK='True'");
            if (dataRows.Length > 0)
            {
                foreach (DataRow row in dataRows)
                {
                    row["COLUMN_CHECK"] = "False";
                    addTable.Rows.Add(row.ItemArray);
                    row.Delete();
                }
                deleteTable.AcceptChanges();
                addTable.AcceptChanges();
            }          
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            DataTable deleteTable = gcUnSelect.DataSource as DataTable;
            DataTable addTable = gcSelect.DataSource as DataTable;
            DataRow[] dataRows = deleteTable.Select("COLUMN_CHECK='True'");
            if (dataRows.Length > 0)
            {
                foreach (DataRow row in dataRows)
                {
                    row["COLUMN_CHECK"] = "False";
                    addTable.Rows.Add(row.ItemArray);
                    row.Delete();
                }
                deleteTable.AcceptChanges();
                addTable.AcceptChanges();
            }            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                DataTable newSelectTable = gcSelect.DataSource as DataTable;
                DataTable saveTable = CreateDataTable();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                for (int i = 0; i < newSelectTable.Rows.Count; i++)
                {
                    int j;
                    string userKey = string.Empty;
                    userKey = newSelectTable.Rows[i][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                    //new的用户组和old的用户组进行对比判断是否用户为新增的
                    for (j = 0; j < oldSelectTable.Rows.Count; j++)
                    {
                        //userKey = newSelectTable.Rows[i][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                        if (oldSelectTable.Rows[j][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString() == userKey)
                            break;
                    }
                    if (j == oldSelectTable.Rows.Count)
                    {
                        //add new 
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,userKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref saveTable, rowData);
                    }
                }

                //判断修改前用户信息和现在用信息进行对比判断是否有删除
                for (int i = 0; i < oldSelectTable.Rows.Count; i++)
                {
                    int j;
                    string userKey = string.Empty;
                    userKey = oldSelectTable.Rows[i][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                    //new的用户组和old的用户组进行对比判断是否存在用户已删除newSelectTable.Rows.Count
                    for (j = 0; j < newSelectTable.Rows.Count; j++)
                    {
                        //userKey = oldSelectTable.Rows[i][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                        if (newSelectTable.Rows[j][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString() == userKey)
                            break;
                    }
                    //
                    if (j == newSelectTable.Rows.Count)
                    {
                        //add delete
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,userKey},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref saveTable, rowData);
                    }
                }
                if (saveTable.Rows.Count > 0)
                {
                    DataSet dsSave = new DataSet();
                    dsSave.Tables.Add(saveTable);
                    User userEntity = new User();
                    userEntity.SaveUserRole(dsSave);
                    if (userEntity.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        BindDataOfSelectedUser();
                        BindDataOfUnSelectUser();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("没有可更新的数据");
                }
            }

        }

        /// <summary>
        /// 创建一个自定义的表结构
        /// </summary>
        /// <returns>DataTable</returns>
        public  DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_USER_KEY,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_EDITOR,
                                                        RBAC_USER_IN_ROLE_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_USER_IN_ROLE_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        /// <summary>
        /// 点击关闭按钮关闭当前窗口
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //页面加载的时候对以选角色和未选角色进行数据绑定
        private void RoleUserDialog_Load(object sender, EventArgs e)
        {            
            BindDataOfSelectedUser();
            BindDataOfUnSelectUser();
        }

        //查询绑定角色已选用户名称信息
        private void BindDataOfSelectedUser()
        {
            DataSet dsSelect = privilegeEntity.GetUserOfRole("SELECT");
            //判断数据库查询操作是否成功
            if (privilegeEntity.ErrorMsg == "")
            {
                //判断查询到以选用户名称的数据信息是否存在
                if (dsSelect != null && dsSelect.Tables.Count > 0)
                {
                    oldSelectTable = dsSelect.Tables[0].Copy();
                    this.gcSelect.MainView = gvSelect;
                    this.gcSelect.DataSource = dsSelect.Tables[0];
                    this.gvSelect.BestFitColumns();
                }
            }
        }

        //查询绑定角色未选择用户名称信息
        private void BindDataOfUnSelectUser()
        {
            DataSet dsUnSelect = privilegeEntity.GetUserOfRole("UNSELECT");
            //判断数据库查询操作是否成功
            if (privilegeEntity.ErrorMsg == "")
            {
                //判断查询到未选用户名称的数据信息是否存在
                if (dsUnSelect != null && dsUnSelect.Tables.Count > 0)
                {                    
                    this.gcUnSelect.MainView = gvUnSelect;
                    this.gcUnSelect.DataSource = dsUnSelect.Tables[0];
                    this.gvUnSelect.BestFitColumns();
                }
            }
        }       
    }
}

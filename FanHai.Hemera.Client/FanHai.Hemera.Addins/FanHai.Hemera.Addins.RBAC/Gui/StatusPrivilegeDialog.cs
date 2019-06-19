
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace FanHai.Hemera.Addins.RBAC.Gui
{
    public partial class StatusPrivilegeDialog : BaseDialog
    {
        /// <summary>
        /// 定义私有变量
        /// </summary>
        private Privilege privilegeEntity = null;
        private DataTable selectStatusTable = new DataTable();

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public StatusPrivilegeDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="roleKey"></param>
        /// <param name="roleName"></param>
        public StatusPrivilegeDialog(string roleKey, string roleName)
            : base(StringParser.Parse("分配机台状态"))//因为集成了类，基类构造函数的参数，对话框的标题
        {
            InitializeComponent();
            privilegeEntity = new Privilege();
            privilegeEntity.RoleKey = roleKey;//类的属性赋值
            this.txtRoleName.Text = roleName;//角色名
            this.txtRoleName.Properties.ReadOnly = true;
        }

        /// <summary>
        /// show的时候load画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusPrivilegeDialog_Load(object sender, EventArgs e)
        {
            BindDataToUnSelectStatus();//角色没有权限的机台状态显示在右边
            BindDataToSelectStatus();//角色有权限的机台状态显示左边
        }

        /// <summary>
        /// 角色没有权限的机台状态显示在右边
        /// </summary>
        private void BindDataToUnSelectStatus()
        {
            if (clbUnSelectStatus.Items.Count > 0)//如果已经有了就先清空
            {
                clbUnSelectStatus.Items.Clear();
            }
            DataSet dsUnSelectStatus= new DataSet();
            dsUnSelectStatus = privilegeEntity.GetStatusOfRole("UNSELECT");
            if (privilegeEntity.ErrorMsg == "")
            {
                if (dsUnSelectStatus.Tables.Count > 0)
                {
                    for (int i = 0; i < dsUnSelectStatus.Tables[0].Rows.Count; i++)
                    {
                        string statusName = dsUnSelectStatus.Tables[0].Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                        clbUnSelectStatus.Items.Add(statusName);
                    }
                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}");
            }
        }

        /// <summary>
        /// 角色有权限的机台状态显示左边
        /// </summary>
        private void BindDataToSelectStatus()
        {
            if (clbSelectStatus.Items.Count > 0)
            {
                clbSelectStatus.Items.Clear();
            }
            DataSet dsSelectStatus = new DataSet();
            dsSelectStatus = privilegeEntity.GetStatusOfRole("SELECT");
            if (privilegeEntity.ErrorMsg == "")
            {

                if (dsSelectStatus.Tables.Count > 0)
                {
                    selectStatusTable = dsSelectStatus.Tables[0];
                    for (int i = 0; i < dsSelectStatus.Tables[0].Rows.Count; i++)
                    {
                        string statusName = dsSelectStatus.Tables[0].Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString();
                        clbSelectStatus.Items.Add(statusName);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 已选择的移到未选择里边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smbDelete_Click(object sender, EventArgs e)
        {
            if (clbSelectStatus.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in clbSelectStatus.CheckedItems)//已有的状态并且选中的添加到未有的那边
                {
                    clbUnSelectStatus.Items.Add(o);
                }
                for (int i = clbSelectStatus.Items.Count - 1; i >= 0; i--)//已选择的包含在已有的里边的删除
                {
                    if (clbSelectStatus.CheckedItems.Contains(clbSelectStatus.Items[i]))
                    {
                        clbSelectStatus.Items.Remove(clbSelectStatus.Items[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 把未选择的状态移到选择那边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smbAdd_Click(object sender, EventArgs e)
        {
            if (this.clbUnSelectStatus.CheckedItems.Count <= 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.SelectRemind}", "${res:Global.SystemInfo}");
            }
            else
            {
                foreach (object o in this.clbUnSelectStatus.CheckedItems)
                {
                    clbSelectStatus.Items.Add(o);
                }
                for (int i = clbUnSelectStatus.Items.Count - 1; i >= 0; i--)
                {
                    if (clbUnSelectStatus.CheckedItems.Contains(clbUnSelectStatus.Items[i]))
                    {
                        clbUnSelectStatus.Items.Remove(clbUnSelectStatus.Items[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                DataSet dsSave = new DataSet();
                DataTable dataTable = CreateDataTable();
                Privilege privilege = new Privilege();
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                //现有的与本来的比较，找不到的就是新增的
                for (int i = 0; i < clbSelectStatus.Items.Count; i++)//新增以后界面上的所有的与已经在DB里边的比对
                {
                    int j;
                    //
                    for (j = 0; j < selectStatusTable.Rows.Count; j++)//已经选择了
                    {
                        if (clbSelectStatus.Items[i].ToString() == selectStatusTable.Rows[j][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString())
                            break;
                    }
                    if (j == selectStatusTable.Rows.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME,clbSelectStatus.Items[i].ToString()},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                //本来的与现在的比较，找不到的就是删除的
                for (int i = 0; i < selectStatusTable.Rows.Count; i++)
                {
                    int k;
                    for (k = 0; k < clbSelectStatus.Items.Count; k++)
                    {
                        if (selectStatusTable.Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString() == clbSelectStatus.Items[k].ToString())
                            break;
                    }
                    if (k == clbSelectStatus.Items.Count)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_ROLE_KEY,privilegeEntity.RoleKey},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME,selectStatusTable.Rows[i][RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME].ToString()},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };

                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                    }
                }
                //增加到ds中调用保存函数
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                    privilege.SaveStatusOfRole(dsSave);
                    if (privilege.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        BindDataToUnSelectStatus();
                        BindDataToSelectStatus();
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

        /// <summary>
        /// 建立一个table的方法
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateDataTable()
        {
            //定义一个集合
            List<string> fields = new List<string>() {  
                                                        RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_ROLE_KEY,
                                                        RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EQUIPMENT_STATE_NAME,
                                                        RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDITOR,
                                                        RBAC_ROLE_OWN_STATUS_FIELDS.FIELD_EDIT_TIMEZONE,
                                                        COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
                                                       };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_ROLE_OWN_STATUS_FIELDS.DATABASE_TABLE_NAME, fields);
        }
    }
}


#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Addins.RBAC
{
    public partial class DistributePrivilege : BaseDialog
    {
        #region private variable 
        TreeNode tn = new TreeNode();
        DataSet dataRow = new DataSet();
        DataSet dataColumn = new DataSet();
        Privilege privilegeEntity = null;       
        private string privilegeType = "";//role or user
        private string typeName = "";//userName or roleName
        private string typeKey = "";//If assign the user rights then typeKey is userKey, otherwise it is roleKey

        private string resourceGroupKey = "";
        private string operationGroupKey = "";
        #endregion

        #region Constructor
        public DistributePrivilege(string type, string name, string key)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.Title}"))
        {
            InitializeComponent();
            privilegeType = type;
            typeName = name;
            typeKey = key;
        }
        #endregion

        #region Button Event
        /// <summary>
        /// Save information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}","${res:Global.SystemInfo}"))
            {
                #region create tables
                DataSet dsSave = new DataSet();
                DataTable rolePrivilegeTable = CreateRolePrivilegeTable();
                DataTable userPrivilegeTable = CreateUserPrivilegeTable();
                DataTable dataTable = CreateDataTable();
                #endregion
                
                #region add data to table
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                //foreach row                
                for (int i = 0; i < privilegeGridView.RowCount; i++)
                {
                    //foreach column of operation name
                    for (int j = 2; j < privilegeGridView.ColumnCount; j = j + 3)
                    {
                        int keyNumber = j + 1;
                        int privilegeNumber = j + 2;
                        //if privilege key is not null and the checkBox of operation name is false then the operation action is delete
                        if (privilegeGridView.Rows[i].Cells[privilegeNumber].Value != null)
                        {
                            if (privilegeGridView.Rows[i].Cells[j].Value.ToString() == "False")
                            {
                                //add to delete Table
                                if (privilegeType == RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME)
                                {
                                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY,privilegeGridView.Rows[i].Cells[privilegeNumber].Value.ToString()},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_ROLE_KEY,typeKey},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };
                                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref rolePrivilegeTable, rowData);
                                }
                                else if (privilegeType == RBAC_USER_FIELDS.DATABASE_TABLE_NAME)
                                {
                                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_PRIVILEGE_KEY,privilegeGridView.Rows[i].Cells[privilegeNumber].Value.ToString()},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_USER_KEY,typeKey},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.Delete)}
                                                            };
                                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref userPrivilegeTable, rowData);
                                }
                            }
                        }
                        //else if privilege key is null and the checkBox of operation name is true then the operation action is new
                        else
                        {
                            if (privilegeGridView.Rows[i].Cells[j].Value != null)
                            {
                                //add to Save Table
                                #region MapDataToDataTable
                                if (privilegeGridView.Rows[i].Cells[j].Value.ToString() == "True")
                                {
                                    privilegeEntity = new Privilege();
                                    privilegeEntity.PrivilegeKey =  CommonUtils.GenerateNewKey(0);
                                    privilegeEntity.OperationKey = privilegeGridView.Rows[i].Cells[keyNumber].Value.ToString();
                                    privilegeEntity.ResourceKey = privilegeGridView.Rows[i].Cells[0].Value.ToString();
                                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY,privilegeEntity.PrivilegeKey},
                                                                {RBAC_PRIVILEGE_FIELDS.FIELD_OPERATION_KEY,privilegeEntity.OperationKey},
                                                                {RBAC_PRIVILEGE_FIELDS.FIELD_RESOURCE_KEY,privilegeEntity.ResourceKey},
                                                                {RBAC_PRIVILEGE_FIELDS.FIELD_CREATOR,editor},
                                                                {RBAC_PRIVILEGE_FIELDS.FIELD_CREATE_TIMEZONE,editTimeZone}                                                                
                                                            };
                                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                                    if (privilegeType == RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME)
                                    {
                                        Dictionary<string, string> rowDataOfRolePrivilege = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY,privilegeEntity.PrivilegeKey},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_ROLE_KEY,typeKey},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone },
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };
                                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref rolePrivilegeTable, rowDataOfRolePrivilege);
                                    }
                                    else if (privilegeType == RBAC_USER_FIELDS.DATABASE_TABLE_NAME)
                                    {
                                        Dictionary<string, string> rowDataOfUserPrivilege = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_PRIVILEGE_KEY,privilegeEntity.PrivilegeKey},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_USER_KEY,typeKey},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDITOR,editor},
                                                                {RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDIT_TIMEZONE,editTimeZone},
                                                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)OperationAction.New)}
                                                            };
                                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref userPrivilegeTable, rowDataOfUserPrivilege);
                                    }
                                }
                                #endregion
                            }
                        }

                    }
                }
                #endregion

                #region add table to dataSet
                if (dataTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(dataTable);
                }
                if (rolePrivilegeTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(rolePrivilegeTable);
                }
                if (userPrivilegeTable.Rows.Count > 0)
                {
                    dsSave.Tables.Add(userPrivilegeTable);
                }
                #endregion

                #region execute save
                if (dsSave.Tables.Count > 0)
                {
                    privilegeEntity = new Privilege();
                    privilegeEntity.SavePrivilege(dsSave);
                    if (privilegeEntity.ErrorMsg == "")
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        BindDataToGridView();
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}");
                    }
                }
                else
                {
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.MsgNoDataForSave}", "${res:Global.SystemInfo}");
                }
                #endregion
            }
        }

        /// <summary>
        /// Cancel operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Create DataTabele
        /// <summary>
        /// RBAC_PRIVILEGE table
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>()
            { 
                 RBAC_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY,
                 RBAC_PRIVILEGE_FIELDS.FIELD_RESOURCE_KEY,
                 RBAC_PRIVILEGE_FIELDS.FIELD_OPERATION_KEY,
                 RBAC_PRIVILEGE_FIELDS.FIELD_CREATOR,
                 RBAC_PRIVILEGE_FIELDS.FIELD_CREATE_TIMEZONE                                                        
            };

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        /// <summary>
        /// RBAC_ROLE_OWN_PRIVILEGE table
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateRolePrivilegeTable()
        {
            List<string> fields = new List<string>()
            {
                RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY,
                RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_ROLE_KEY,
                RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDITOR,
                RBAC_ROLE_OWN_PRIVILEGE_FIELDS.FIELD_EDIT_TIMEZONE,
                COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_ROLE_OWN_PRIVILEGE_FIELDS.DATABASE_TABLE_NAME,fields);
        } 
     
        /// <summary>
        /// RBAC_USER_OWN_PRIVILEGE table
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateUserPrivilegeTable()
        {
            List<string> fields = new List<string>()
            {
               RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_PRIVILEGE_KEY,
               RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_USER_KEY,
               RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDITOR,
               RBAC_USER_OWN_PRIVILEGES_FIELDS.FIELD_EDIT_TIMEZONE,
               COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_USER_OWN_PRIVILEGES_FIELDS.DATABASE_TABLE_NAME,fields);
        }   
    
        /// <summary>
        ///  table for search
        /// </summary>
        /// <returns></returns>
        public DataTable CreateTableForSearchPrivilege()
        {
            List<string> fields = new List<string>()
            {
               "ROW_KEY",
               RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY,
               RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY,
               "TABLE_NAME"
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(TRANS_TABLES.TABLE_MAIN_DATA, fields);
        }
        #endregion

        #region Dialog Load
        /// <summary>
        /// Dialog load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistributePrivilege_Load(object sender, EventArgs e)
        {
            this.btnSave.Enabled = false;
            InitUI();
            BindTreeView();
            BindLookUpEdit();
            if (privilegeType == RBAC_ROLE_FIELDS.DATABASE_TABLE_NAME)
            {
                this.lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.lblType.Role}");
            }
            else if(privilegeType==RBAC_USER_FIELDS.DATABASE_TABLE_NAME)
            {
                this.lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.lblType.User}");
            }
            this.lblTypeName.Text = typeName;
        }

        /// <summary>
        /// InitUI
        /// </summary>
        private void InitUI()
        {
            this.ResourceGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceGroup}");
            this.OperationGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationGroup}");
            lblResourceName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.lblResourceName}")+":";
            lblOperationGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.lblOperationName}");
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
            this.btnClose.Text = StringParser.Parse("${res:Global.CloseButtonText}");

            this.lueOperationGroup.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("GROUP_NAME",StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationGroup}"))});
        }
        #endregion

        #region User Control Event
        /// <summary>
        /// bind operations to lookUpEdit
        /// 下拉列表数据的绑定
        /// </summary>
        private void BindLookUpEdit()
        {
            DataSet groupData = new DataSet();
            RBACOperationGroup operationGroup = new RBACOperationGroup();
            groupData=operationGroup.GetOperationGroup();
            if (operationGroup.ErrorMsg == "")
            {
                if (groupData != null)
                {
                    lueOperationGroup.Properties.DataSource = groupData.Tables[0];
                    lueOperationGroup.Properties.DisplayMember = RBAC_OPERATION_GROUP_FIELDS.FIELD_GROUP_NAME;
                    lueOperationGroup.Properties.ValueMember = RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY;
                    lueOperationGroup.ItemIndex = 0;
                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.Msg.GetOperationError}");
            }
        }

        /// <summary>
        /// Bind resources to treeView
        /// </summary>
        private void BindTreeView()
        {
            DataSet dataSet = new DataSet();
            ResourceGroup resourceGroup = new ResourceGroup();
            dataSet = resourceGroup.GetResourceGroup();
            if (resourceGroup.ErrorMsg == "")
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string resourceName = dataSet.Tables[0].Rows[i][RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME].ToString();
                        resourceTree.Nodes.Add(resourceName);
                        resourceTree.Nodes[i].ImageIndex = 0;
                        resourceTree.Nodes[i].SelectedImageIndex = 1;
                        resourceTree.Nodes[i].Tag = dataSet.Tables[0].Rows[i][RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY].ToString();
                    }
                }
            }
            else
            {               
                MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.Msg.GetResourceError}");
            }
            if (resourceTree.Nodes.Count > 0)
            {
                resourceTree.SelectedNode = resourceTree.Nodes[0];
                NodeMouseClick(resourceTree.Nodes[0]);
            }
        }  
      
        /// <summary>
        /// Greate and initialize girdView
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="dataColumn"></param>
        private void CreateGridControl(DataSet dataRow,DataSet dataColumn)
        {
            #region create Control
            if (privilegeGridView.RowCount > 0)
            {
                privilegeGridView.Rows.Clear();
            }
            if (privilegeGridView.ColumnCount > 0)
            {
                privilegeGridView.Columns.Clear();
            }

            #region Add Columns
            //编译GridView的列信息形成一个表结构
            if (dataColumn != null && dataColumn.Tables.Count>0)
            {
                if (dataColumn.Tables[0].Rows.Count > 0)
                {    
                    //add column of resource key
                    privilegeGridView.Columns.Add("ResourceKey","资源ID");
                    privilegeGridView.Columns["ResourceKey"].Visible = false;
                    //add column of resource name
                    privilegeGridView.Columns.Add("ResourceName", StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.Column.ResourceName}"));
                    int column = dataColumn.Tables[0].Rows.Count;
                    for (int j = 0; j < column; j++)
                    {  
                        //add column of operation name
                        string operationKey=dataColumn.Tables[0].Rows[j][RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY].ToString();  
                        DataGridViewCheckBoxColumn newColumn=new DataGridViewCheckBoxColumn();
                        newColumn.Name=dataColumn.Tables[0].Rows[j][RBAC_OPERATION_FIELDS.FIELD_OPERATION_NAME].ToString();
                        newColumn.HeaderText=dataColumn.Tables[0].Rows[j][RBAC_OPERATION_FIELDS.FIELD_DISPLAY_NAME].ToString();                       
                        privilegeGridView.Columns.Add(newColumn);
                        //add column of operation key
                        privilegeGridView.Columns.Add(operationKey, "operationKey" + j.ToString());
                        privilegeGridView.Columns[operationKey].Visible = false;
                        //add column of privilege key 
                        privilegeGridView.Columns.Add("PrivilegeKey" + j.ToString(), "PrivilegeKey" + j.ToString());
                        privilegeGridView.Columns["PrivilegeKey" + j.ToString()].Visible = false;
                        //privilegeGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(216, 229, 248);
                        //privilegeGridView.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(216, 229, 248);
                    }                    
                }
            }
            #endregion

            #region Add Rows
            //把获取到的数据添加到GridView的对应绑定列中
            if (dataRow != null && dataRow.Tables.Count > 0 && dataColumn.Tables[0].Rows.Count>0)
            {
                if (dataRow.Tables[0].Rows.Count > 0)
                {
                    int rowCount = dataRow.Tables[0].Rows.Count;                  
                    for (int i = 0; i < rowCount; i++)
                    {
                        int k = 3;
                        privilegeGridView.Rows.Add();
                        //set resource key to each row
                        privilegeGridView.Rows[i].Cells[0].Value=dataRow.Tables[0].Rows[i][RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_KEY].ToString();
                        //set resource name to each row
                        privilegeGridView.Rows[i].Cells[1].Value=dataRow.Tables[0].Rows[i][RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_NAME].ToString();
                        privilegeGridView.Rows[i].Cells[1].ReadOnly=true;

                        //if exist columns of gridView
                        if (dataColumn != null && dataColumn.Tables.Count > 0)
                        {
                            if (dataColumn.Tables[0].Rows.Count > 0)
                            {  
                                //set operation key to each cell
                                for (int j = 0; j < dataColumn.Tables[0].Rows.Count; j++)
                                {                                    
                                     privilegeGridView.Rows[i].Cells[k].Value = dataColumn.Tables[0].Rows[j][RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY].ToString();
                                     k = k + 3;
                                 }
                            }
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region Bind Data To Control
            BindDataToGridView();          
            #endregion

            #region set save button enable
            if (privilegeGridView.RowCount > 0 && privilegeGridView.ColumnCount > 0)
            {
                this.btnSave.Enabled = true;
            }
            #endregion
        }

        /// <summary>
        /// Bind data to GridView
        /// </summary>
        private void BindDataToGridView()
        {
            #region clean privilege key of each cell
            //if exist rows of GridView
            if (dataRow != null && dataRow.Tables.Count > 0)
            {
                //Clean privilege key of each cell
                for (int i = 0; i < privilegeGridView.RowCount; i++)
                {
                    for (int j = 4; j < privilegeGridView.ColumnCount; j = j + 3)
                    {
                        privilegeGridView.Rows[i].Cells[j].Value =null;
                    }
                }
            }
            #endregion

            #region search data
            if (operationGroupKey != "" && resourceGroupKey != "")
            {
                #region create table for search
                DataSet dsGetPrivilege = new DataSet();
                DataSet dataSet = new DataSet();
                DataTable dataTable = CreateTableForSearchPrivilege();
                //table for search
                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {"ROW_KEY",typeKey},
                                                                {RBAC_RESOURCE_GROUP_FIELDS.FIELD_RESOURCE_GROUP_KEY,resourceGroupKey},
                                                                {RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY,operationGroupKey},
                                                                {"TABLE_NAME",privilegeType}
                                                            };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);

                if (dataTable.Rows.Count > 0)
                {
                    dataSet.Tables.Add(dataTable);
                }
                #endregion

                #region bind date to GridView
                privilegeEntity = new Privilege();
                if (dataSet.Tables.Count > 0)
                {
                    //get rights already assigned
                    dsGetPrivilege = privilegeEntity.GetPrivileges(dataSet);
                    if (privilegeEntity.ErrorMsg == "")
                    {
                        if (dsGetPrivilege.Tables[0].Rows.Count > 0)
                        {
                            for (int n = 0; n < dsGetPrivilege.Tables[0].Rows.Count; n++)
                            {
                                string resourceKey = dsGetPrivilege.Tables[0].Rows[n][RBAC_PRIVILEGE_FIELDS.FIELD_RESOURCE_KEY].ToString();
                                string operationKey = dsGetPrivilege.Tables[0].Rows[n][RBAC_PRIVILEGE_FIELDS.FIELD_OPERATION_KEY].ToString();
                                if (dataRow != null && dataRow.Tables.Count > 0)
                                {
                                    //foreach row
                                    for (int i = 0; i < privilegeGridView.RowCount; i++)
                                    {
                                        //column 0 is resource key
                                        //column j is operation key
                                        //foreach column of operation key
                                        for (int j = 3; j < privilegeGridView.ColumnCount; j = j + 3)
                                        {
                                            int k = j - 1;//column of operation name
                                            int m = j + 1;//column of privilege key
                                            if (privilegeGridView.Rows[i].Cells[0].Value.ToString() == resourceKey && privilegeGridView.Rows[i].Cells[j].Value.ToString() == operationKey)
                                            {
                                                privilegeGridView.Rows[i].Cells[k].Value = "True";
                                                privilegeGridView.Rows[i].Cells[m].Value = dsGetPrivilege.Tables[0].Rows[n][RBAC_PRIVILEGE_FIELDS.FIELD_PRIVILEGE_KEY].ToString();
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// The node of resource tree clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resourceTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeMouseClick(e.Node);
        }

        private void NodeMouseClick(TreeNode node)
        {
            Resource resourceEntity = new Resource();
            resourceEntity.ResourceGroupKey = node.Tag.ToString();
            resourceGroupKey = node.Tag.ToString();
            this.lblResourceGroup.Text = node.Text;
            dataRow = resourceEntity.GetResource();
            if (resourceEntity.ErrorMsg == "")
            {
                if (privilegeGridView.ColumnCount > 0)
                {
                    CreateGridControl(dataRow, dataColumn);
                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.Msg.GetResourceError}");
            }
        }
        /// <summary>
        /// operation selected changed
        /// 下拉列表类型选择进行数据更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueOperationGroup_EditValueChanged(object sender, EventArgs e)
        {
            RBACOperation operationEntity = new RBACOperation();            
            operationEntity.GroupKey = lueOperationGroup.EditValue.ToString();
            operationGroupKey = lueOperationGroup.EditValue.ToString();
            dataColumn=operationEntity.GetOperation();
            if (operationEntity.ErrorMsg == "")
            {                
                CreateGridControl(dataRow,dataColumn);
            }
            else
            {               
                MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.DistributePrivilege.Msg.GetOperationError}");
            }
        }
        #endregion


        //private void privilegeGridView_Paint(object sender, PaintEventArgs e)
        //{
        //    e.Graphics.DrawRectangle(Pens.Blue, new Rectangle(0, 0, this.privilegeGridView.Width - 1, this.privilegeGridView.Height - 1)); 
            
        //}
    }
}

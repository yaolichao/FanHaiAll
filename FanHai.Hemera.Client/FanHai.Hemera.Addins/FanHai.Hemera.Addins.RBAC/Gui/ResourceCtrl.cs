using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.RBAC
{
    /// <summary>
    /// 显示资源管理界面的用户控件类。
    /// </summary>
    public partial class ResourceCtrl : BaseUserCtrl
    {
        #region variable define
        //define treenode
        TreeNode tn = new TreeNode();
        ContextMenu contextMenu = null;
        ResourceGroup groupEntity = null;
        Resource resourceEntity = null;
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
        //Define and initialize control state
        private ControlState _ctrlState;
        private string groupKey = "";

        #endregion
        public ResourceCtrl()
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
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtResourceName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtRemark.Text = string.Empty;
                    txtCode.Text = string.Empty;

                    txtResourceName.Properties.ReadOnly = true;
                    txtDescription.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;
                    txtCode.Properties.ReadOnly = true;

                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtResourceName.Properties.ReadOnly = false;
                    txtDescription.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;
                    txtCode.Properties.ReadOnly = false;

                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = true;

                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtResourceName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtRemark.Text = string.Empty;
                    txtCode.Text = string.Empty;

                    txtResourceName.Properties.ReadOnly = false;
                    txtDescription.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;
                    txtCode.Properties.ReadOnly = false;

                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    txtResourceName.Properties.ReadOnly = true;
                    txtDescription.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;
                    txtCode.Properties.ReadOnly = true;

                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    break;
                    #endregion
            }
        }

        private void resourceTree_MouseUp(object sender, MouseEventArgs e)
        {
            #region show basic datatable's viewcontent
            if (resourceTree.Nodes.Count == 0)
            {
                //define context menu
                contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.MenuItem.AddResourceGroup}"), new EventHandler(AddResourceGroup));
                contextMenu.MenuItems[0].Enabled = true;
                //set the position of context menu shows 
                Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                p = this.PointToClient(p);
                //show context menu
                contextMenu.Show(this, p);
            }
            else
            {
                //if left button down and treeview has nodes 
                if (resourceTree.GetNodeAt(e.X, e.Y) != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        //get right click node
                        tn = resourceTree.GetNodeAt(e.X, e.Y);
                        //set selected node
                        resourceTree.SelectedNode = tn;
                        //define context menu
                        contextMenu = new ContextMenu();
                        //add menu items to context menu 
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.MenuItem.AddResourceGroup}"), new EventHandler(AddResourceGroup));
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.MenuItem.UpdateResourceGroup}"), new EventHandler(UpdateResourceGroup));
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.MenuItem.DeleteResourceGroup}"), new EventHandler(DeleteResourceGroup));
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.MenuItem.AddResource}"), new EventHandler(AddResource));
                        CheckPrivilegeOfMenu();
                        //set the position of context menu shows 
                        Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                        p = this.PointToClient(p);
                        //show context menu
                        contextMenu.Show(this, p);
                    }
                }
            }
            #endregion
        }
        private void CheckPrivilegeOfMenu()
        {
            contextMenu.MenuItems[0].Enabled = true;
            contextMenu.MenuItems[1].Enabled = true;
            contextMenu.MenuItems[2].Enabled = true;
            contextMenu.MenuItems[3].Enabled = true;
        }
        private void AddResourceGroup(object sender, EventArgs e)
        {
            AddResourceGroup addResourceGroup = new AddResourceGroup();
            if (DialogResult.OK == addResourceGroup.ShowDialog())
            {
                groupEntity = addResourceGroup.resourceGroupEntity;
                this.resourceTree.Nodes.Add(groupEntity.GroupName);
                this.resourceTree.Nodes[resourceTree.Nodes.Count - 1].Tag = groupEntity.GroupKey;
            }
        }

        private void UpdateResourceGroup(object sender, EventArgs e)
        {
            groupEntity = new ResourceGroup();
            groupEntity.GroupKey = this.resourceTree.SelectedNode.Tag.ToString();
            AddResourceGroup resourceGroupDialog = new AddResourceGroup(groupEntity);
            if (DialogResult.OK == resourceGroupDialog.ShowDialog())
            {
                ResourceGroup resourceGroup = new ResourceGroup();
                resourceGroup = resourceGroupDialog.resourceGroupEntity;
                if (resourceTree.SelectedNode.Text != resourceGroup.GroupName)
                {
                    resourceTree.SelectedNode.Text = resourceGroup.GroupName;
                }
            }
        }

        private void AddResource(object sender, EventArgs e)
        {
            CtrlState = ControlState.New;
            resourceEntity = new Resource();
            resourceEntity.ResourceGroupKey = this.resourceTree.SelectedNode.Tag.ToString();
            resourceEntity.ResourceKey = CommonUtils.GenerateNewKey(0);
        }

        private void DeleteResourceGroup(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                ResourceGroup resourceGroup = new ResourceGroup();
                resourceGroup.GroupKey = this.resourceTree.SelectedNode.Tag.ToString();
                resourceGroup.DeleteResourceGroup();
                if (resourceGroup.ErrorMsg == "")
                {
                    this.resourceTree.Nodes.Remove(resourceTree.SelectedNode);
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.Msg.DeleteMenuItem}");
                }
            }
        }
        private void ResourceCtrl_Load(object sender, EventArgs e)
        {
            InitUIResourcesByCulture();
            BindTreeView();
            GridViewHelper.SetGridView(resourceView);
        }
        public new void InitUIResourcesByCulture()
        {
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceViewContent.TitleName}");   
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.lblResourceName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.lblResourceName}");
            this.lblResourceCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Code}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.ResourceGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceGroup}");
            this.Resource_Name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.lblResourceName}");
            this.Resource_Code.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Code}");
            this.description.Caption = StringParser.Parse("${res:Global.Description}");
            this.Remark.Caption = StringParser.Parse("${res:Global.Remark}");
        }
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

                        resourceTree.Nodes.Add(dataSet.Tables[0].Rows[i][RBAC_RESOURCE_GROUP_FIELDS.FIELD_GROUP_NAME].ToString());
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
        }

        private void resourceTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            groupKey = e.Node.Tag.ToString();
            BindDataToGridView();
            CtrlState = ControlState.Empty;
        }

        private void BindDataToGridView()
        {
            Resource entity = new Resource();
            DataSet dataSet = new DataSet();
            entity.ResourceGroupKey = groupKey;
            dataSet = entity.GetResource();
            this.resourceControl.DataSource = dataSet.Tables[0];
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.txtResourceName.Text.Trim() == "")
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.ResourceCtrl.Msg.NameIsNull}", "${res:Global.SystemInfo}");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtCode.Text, @"^\d{2}$"))
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.AddResourceGroup.CodeError}", "${res:Global.SystemInfo}");
                return;
            }
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}", "${res:Global.SystemInfo}"))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = CreateDataTable();
                bool bNew = (CtrlState == ControlState.New);
                resourceEntity.ResourceName = this.txtResourceName.Text.Trim();
                resourceEntity.Descriptions = this.txtDescription.Text;
                resourceEntity.ResourceCode = this.txtCode.Text;
                resourceEntity.Remark = this.txtRemark.Text;
                resourceEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                resourceEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                resourceEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                resourceEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                resourceEntity.ResourceGroupKey = groupKey;
                Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                            {
                                                                {RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_KEY,resourceEntity.ResourceKey},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY,resourceEntity.ResourceGroupKey},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_NAME,resourceEntity.ResourceName},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE,resourceEntity.ResourceCode},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_DESCRIPTIONS, resourceEntity.Descriptions},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_REMARK,resourceEntity.Remark},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_CREATOR,resourceEntity.Creator},
                                                                {RBAC_RESOURCE_FIELDS.FIELD_CREATE_TIMEZONE,resourceEntity.CreateTimeZone}
                                                            };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                dataSet.Tables.Add(dataTable);
                resourceEntity.SaveResource(bNew, dataSet);
                if (resourceEntity.ErrorMsg == "")
                {
                    BindDataToGridView();
                    CtrlState = ControlState.ReadOnly;
                    resourceEntity.ResetDirtyList();
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + resourceEntity.ErrorMsg);
                }
            }

        }

        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() {
                                                        RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_KEY,
                                                        RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_GROUP_KEY,
                                                        RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_NAME,
                                                        RBAC_RESOURCE_FIELDS.FIELD_RESOURCE_CODE,
                                                        RBAC_RESOURCE_FIELDS.FIELD_DESCRIPTIONS,
                                                        RBAC_RESOURCE_FIELDS.FIELD_REMARK,
                                                        RBAC_RESOURCE_FIELDS.FIELD_CREATOR,
                                                        RBAC_RESOURCE_FIELDS.FIELD_CREATE_TIMEZONE};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_RESOURCE_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}", "${res:Global.SystemInfo}"))
            {
                resourceEntity.DeleteResource();
                if (resourceEntity.ErrorMsg == "")
                {
                    BindDataToGridView();
                    CtrlState = ControlState.Empty;

                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}");
                }
            }
        }

        private void resourceView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                CtrlState = ControlState.Edit;
                resourceEntity = new Resource();
                resourceEntity.ResourceKey = this.resourceView.GetRowCellValue(e.RowHandle, Resource_key).ToString();
                resourceEntity.ResourceName = this.resourceView.GetRowCellValue(e.RowHandle, Resource_Name).ToString();
                resourceEntity.Descriptions = this.resourceView.GetRowCellValue(e.RowHandle, description).ToString();
                resourceEntity.Remark = this.resourceView.GetRowCellValue(e.RowHandle, Remark).ToString();
                resourceEntity.Editor = this.resourceView.GetRowCellValue(e.RowHandle, editor).ToString();
                resourceEntity.EditTimeZone = this.resourceView.GetRowCellValue(e.RowHandle, edit_timeZone).ToString();
                resourceEntity.ResourceCode = this.resourceView.GetRowCellValue(e.RowHandle, Resource_Code).ToString();
                resourceEntity.ResourceGroupKey = this.resourceView.GetRowCellValue(e.RowHandle, resourceGroupKey).ToString();
                resourceEntity.IsInitializeFinished = true;

                this.txtResourceName.Text = resourceEntity.ResourceName;
                this.txtDescription.Text = resourceEntity.Descriptions;
                this.txtRemark.Text = resourceEntity.Remark;
                this.txtCode.Text = resourceEntity.ResourceCode;
            }
        }

        private void resourceView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void tableLayoutPanelRight_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

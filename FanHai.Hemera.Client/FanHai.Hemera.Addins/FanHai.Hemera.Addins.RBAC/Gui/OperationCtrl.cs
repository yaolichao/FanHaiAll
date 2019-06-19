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
    /// 显示操作管理界面的用户控件类。
    /// </summary>
    public partial class OperationCtrl : BaseUserCtrl
    {
        #region variable define
        //define treenode
        TreeNode tn = new TreeNode();       
        RBACOperationGroup _GroupEntity = null;
        RBACOperation _operationEntity = null;
        ContextMenu contextMenu = null;
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
        //Define and initialize control state
        private ControlState _ctrlState;
        private string groupKey = "";

        #endregion
        public OperationCtrl()
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
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtOperationName.Text= string.Empty;
                    txtDisplayName.Text = string.Empty;                    
                    txtRemark.Text = string.Empty;
                    txtCode.Text = string.Empty;

                    txtOperationName.Properties.ReadOnly = true;
                    txtDisplayName.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;
                    txtCode.Properties.ReadOnly = true;

                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtOperationName.Properties.ReadOnly = false;
                    txtDisplayName.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;
                    txtCode.Properties.ReadOnly = false;

                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = true;

                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtOperationName.Text = string.Empty;
                    txtDisplayName.Text = string.Empty;
                    txtRemark.Text = string.Empty;
                    txtCode.Text = string.Empty;

                    txtOperationName.Properties.ReadOnly = false;
                    txtDisplayName.Properties.ReadOnly = false;
                    txtRemark.Properties.ReadOnly = false;
                    txtCode.Properties.ReadOnly = false;

                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    txtOperationName.Properties.ReadOnly = true;
                    txtDisplayName.Properties.ReadOnly = true;
                    txtRemark.Properties.ReadOnly = true;
                    txtCode.Properties.ReadOnly = true;

                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    break;
                #endregion
            }
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.txtOperationName.Text== "")
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.Msg.NameIsNull}", "${res:Global.SystemInfo}");
                this.txtOperationName.Focus();
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtCode.Text, @"^\d{3}$"))
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.CodeError}", "${res:Global.SystemInfo}");
                return;
            }
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.SaveRemind}","${res:Global.SystemInfo}"))
            {
                DataSet dataSet = new DataSet();
                DataTable dataTable = CreateDataTable();
                bool bNew = (CtrlState == ControlState.New);
                _operationEntity.OperationName = this.txtOperationName.Text.Trim();
                _operationEntity.DisplayName = this.txtDisplayName.Text;
                _operationEntity.Remark = this.txtRemark.Text;
                _operationEntity.OperationCode = this.txtCode.Text;
                _operationEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _operationEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                _operationEntity.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _operationEntity.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

                Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY,_operationEntity.OperationKey},
                                                                {RBAC_OPERATION_FIELDS.FIELD_OPERATION_GROUP_KEY,_operationEntity.GroupKey},
                                                                {RBAC_OPERATION_FIELDS.FIELD_OPERATION_NAME,_operationEntity.OperationName},
                                                                {RBAC_OPERATION_FIELDS.FIELD_OPERATION_CODE, _operationEntity.OperationCode},
                                                                {RBAC_OPERATION_FIELDS.FIELD_DISPLAY_NAME,_operationEntity.DisplayName},
                                                                {RBAC_OPERATION_FIELDS.FIELD_REMARK,_operationEntity.Remark},
                                                                {RBAC_OPERATION_FIELDS.FIELD_CREATOR,_operationEntity.Creator},
                                                                {RBAC_OPERATION_FIELDS.FIELD_CREATE_TIMEZONE,_operationEntity.CreateTimeZone}
                                                            };
                FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);
                dataSet.Tables.Add(dataTable);
                _operationEntity.SaveOperation(bNew, dataSet);
                if (_operationEntity.ErrorMsg == "")
                {
                    BindDataToGridView();
                    CtrlState = ControlState.ReadOnly;
                    _operationEntity.ResetDirtyList();
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _operationEntity.ErrorMsg);
                }
            }
        }
        public static DataTable CreateDataTable()
        {
            List<string> fields = new List<string>() { 
                                                        RBAC_OPERATION_FIELDS.FIELD_OPERATION_KEY,
                                                        RBAC_OPERATION_FIELDS.FIELD_OPERATION_GROUP_KEY,
                                                        RBAC_OPERATION_FIELDS.FIELD_OPERATION_NAME,
                                                        RBAC_OPERATION_FIELDS.FIELD_OPERATION_CODE,
                                                        RBAC_OPERATION_FIELDS.FIELD_DISPLAY_NAME,
                                                        RBAC_OPERATION_FIELDS.FIELD_REMARK,
                                                        RBAC_OPERATION_FIELDS.FIELD_CREATOR,
                                                        RBAC_OPERATION_FIELDS.FIELD_CREATE_TIMEZONE};

            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_OPERATION_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        private void BindDataToGridView()
        {
            RBACOperation entity = new RBACOperation();
            DataSet dataSet = new DataSet();
            entity.GroupKey = groupKey;
            dataSet = entity.GetOperation();
            this.operationControl.DataSource = dataSet.Tables[0];          
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}","${res:Global.SystemInfo}"))
            {
                _operationEntity.DeleteOperation();
                if (_operationEntity.ErrorMsg == "")
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

        private void OperationCtrl_Load(object sender, EventArgs e)
        {
            InitUIResourcesByCulture();
            BindTreeView();
            GridViewHelper.SetGridView(operationView);
        }
        
        protected override void InitUIResourcesByCulture()
        {
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationViewContent.TitleName}"); 
            this.lblOperationName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.lblOperationName}");
            this.lblDisplayName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.lblDispalyName}");
            this.lblCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Code}");
            this.lblRemark.Text = StringParser.Parse("${res:Global.Remark}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.OperationGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationGroup}");
            this.Operation_Name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.lblOperationName}");
            this.Operation_Code.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.Code}");
            this.Display_Name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.lblDispalyName}");
            this.Remark.Caption = StringParser.Parse("${res:Global.Remark}");
        }
        private void BindTreeView()
        {

            DataSet dataSet = new DataSet();
            RBACOperationGroup operationGroup = new RBACOperationGroup();
            dataSet = operationGroup.GetOperationGroup();
            if (operationGroup.ErrorMsg == "")
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        operationTree.Nodes.Add(dataSet.Tables[0].Rows[i][RBAC_OPERATION_GROUP_FIELDS.FIELD_GROUP_NAME].ToString());
                        operationTree.Nodes[i].ImageIndex = 0;
                        operationTree.Nodes[i].SelectedImageIndex = 1;
                        operationTree.Nodes[i].Tag = dataSet.Tables[0].Rows[i][RBAC_OPERATION_GROUP_FIELDS.FIELD_OPERATION_GROUP_KEY].ToString();

                    }

                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.Msg.GetOperationError}");
            }
        }      

        private void operationView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                CtrlState = ControlState.Edit;
                _operationEntity = new RBACOperation();
                _operationEntity.OperationKey = this.operationView.GetRowCellValue(e.RowHandle,Operation_Key).ToString();
                _operationEntity.OperationName = this.operationView.GetRowCellValue(e.RowHandle, Operation_Name).ToString();
                _operationEntity.DisplayName = this.operationView.GetRowCellValue(e.RowHandle, Display_Name).ToString();
                _operationEntity.OperationCode = this.operationView.GetRowCellValue(e.RowHandle, Operation_Code).ToString();
                _operationEntity.Remark = this.operationView.GetRowCellValue(e.RowHandle, Remark).ToString();
                _operationEntity.Editor = this.operationView.GetRowCellValue(e.RowHandle, editor).ToString();
                _operationEntity.EditTimeZone = this.operationView.GetRowCellValue(e.RowHandle, edit_timeZone).ToString();
                _operationEntity.IsInitializeFinished = true;

                this.txtOperationName.Text = _operationEntity.OperationName;
                this.txtDisplayName.Text = _operationEntity.DisplayName;
                this.txtCode.Text = _operationEntity.OperationCode;
                this.txtRemark.Text = _operationEntity.Remark;
            }
        }

        private void operationTree_MouseUp(object sender, MouseEventArgs e)
        {
            #region show basic datatable's viewcontent
            if (operationTree.Nodes.Count == 0)
            {
                //define context menu
                contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.MenuItem.AddOperationGroup}"), new EventHandler(AddOperationGroup));
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
                if (operationTree.GetNodeAt(e.X, e.Y) != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        //get right click node
                        tn = operationTree.GetNodeAt(e.X, e.Y);
                        //set selected node
                        operationTree.SelectedNode = tn;
                        //define context menu
                        contextMenu = new ContextMenu();
                        //add menu items to context menu                       
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.MenuItem.AddOperationGroup}"), new EventHandler(AddOperationGroup));
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.MenuItem.DeleteOperationGroup}"), new EventHandler(DeleteOperationGroup));
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.MenuItem.AddOperation}"), new EventHandler(AddOperation));
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
        }
        private void AddOperationGroup(object sender, EventArgs e)
        {             
            AddOperationGroup AddOperationGroup = new AddOperationGroup();
            if (DialogResult.OK == AddOperationGroup.ShowDialog())
            {
                _GroupEntity = AddOperationGroup.operationGroupEntity;
                this.operationTree.Nodes.Add(_GroupEntity.GroupName);
                this.operationTree.Nodes[operationTree.Nodes.Count - 1].Tag = _GroupEntity.OperationGroupKey;
            }
        }
        private void DeleteOperationGroup(object sender,EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                _GroupEntity = new RBACOperationGroup();
                _GroupEntity.OperationGroupKey = this.operationTree.SelectedNode.Tag.ToString();
                _GroupEntity.DeleteOperationGroup();
                if (_GroupEntity.ErrorMsg == "")
                {
                    this.operationTree.Nodes.Remove(operationTree.SelectedNode);
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.RBAC.OperationCtrl.Msg.DeleteMenuItem}");
                }
            }
        }
        private void AddOperation(object sender,EventArgs e)
        {
            CtrlState = ControlState.New;
            _operationEntity = new RBACOperation();
            _operationEntity.GroupKey = this.operationTree.SelectedNode.Tag.ToString();
            _operationEntity.OperationKey =  CommonUtils.GenerateNewKey(0);            
        }
        private void operationTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            groupKey = e.Node.Tag.ToString();
            BindDataToGridView();
            CtrlState = ControlState.Empty;
        }

        private void operationView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示途程组管理界面的用户自定义控件类。
    /// </summary>
    public partial class EnterpriseCtrl : BaseUserCtrl
    {
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;

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

        public EnterpriseCtrl(EnterpriseEntity enterprise)
        {
            InitializeComponent();

            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);

            _enterprise = enterprise;

            if (null == _enterprise || _enterprise.EnterpriseKey.Length < 1)
            {
                return;
            }
            else if (_enterprise.EnterpriseKey.Length > 1 && _enterprise.EnterpriseName == string.Empty)
            {
                CtrlState = ControlState.New;
            }
            else
            {
                MapEnterpriseToControls();

                if (_enterprise.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else
                    CtrlState = ControlState.Read;
            }
            _enterprise.GetMaxVerRoute(ref routeTable, null);
            grdCtrlRoute.MainView = gridViewRoute;
            grdCtrlRoute.DataSource = routeTable;

            GridViewHelper.SetGridView(gridViewEnterprise);
            GridViewHelper.SetGridView(gridViewRoute);

            lblMenu.Text = "基础数据 > 流程管理 > 工艺组";
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of editer
                case ControlState.Edit:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;

                    txtEnterpriseName.Properties.ReadOnly = true;
                    //_enterprise.DirtyList.Clear();
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;

                    txtEnterpriseName.Properties.ReadOnly = false;

                    _enterprise = new EnterpriseEntity();
                    MapEnterpriseToControls();
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;

                    txtEnterpriseName.Properties.ReadOnly = true;
                    break;
                #endregion

                #region case state of Read
                case ControlState.Read:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = true;

                    txtEnterpriseName.Properties.ReadOnly = true;
                    mmDescription.Properties.ReadOnly = true;
                    break;
                #endregion
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }

        #region Validation & Set Controls Data To Enterprise
        /// <summary>
        /// Validation & Collection Data for Enterprise
        /// </summary>
        private void MapControlsToEnterprise()
        {
            if (null == _enterprise)
            {
                throw (new Exception("Error Enterprise Set"));
            }

            // TODO: Data validation
            _enterprise.EnterpriseName = txtEnterpriseName.Text;
            _enterprise.EnterpriseDescription = mmDescription.Text;
            _enterprise.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            _enterprise.EditTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _enterprise.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            TreeNode parentNode = tvEnterprise.Nodes[0];
            bool blFlag = false;
            foreach (TreeNode node in parentNode.Nodes)
            {
                blFlag = false;
                Enterprises enterprises = (Enterprises)node.Tag;
                if(_enterprise.EnterpriseList.Count>0)
                {
                    foreach (Enterprises item in _enterprise.EnterpriseList)
                    {
                        if (enterprises.RouteSqenceKey == item.RouteSqenceKey)
                        {
                            blFlag = true;
                            if (node.Index.ToString() != item.RouteSeqence)
                            {
                                item.RouteSeqence = node.Index.ToString();
                                item.OperationAction = OperationAction.Modified;
                            }
                            break;
                        }
                    }
                    
                }
                //if not found the key in the list ,add this step to the route 
                if (blFlag == false)
                {
                    enterprises.RouteSeqence = node.Index.ToString();
                    _enterprise.EnterpriseList.Add(enterprises);
                }
            }

            foreach (Enterprises item in _enterprise.EnterpriseList)
            {
                blFlag = false;
                foreach (TreeNode node in parentNode.Nodes)
                {
                    Enterprises enterprises = (Enterprises)node.Tag;
                    if (item.RouteSqenceKey == enterprises.RouteSqenceKey)
                    {
                        blFlag = true;
                        break;
                    }
                }
                if (blFlag == false)
                {
                    item.OperationAction = OperationAction.Delete;
                }
            }
        }
        #endregion

        #region Validation & Set Enterprise Data To Controls
        /// <summary>
        /// Set Enterprise Item data to Controls
        /// </summary>
        private void MapEnterpriseToControls()
        {
            txtEnterpriseName.Text = _enterprise.EnterpriseName;
            mmDescription.Text = _enterprise.EnterpriseDescription;

            grdCtrlEnterprise.MainView = gridViewEnterprise;
            grdCtrlEnterprise.DataSource = _enterprise.EnterpriseList;

            tvEnterprise.Nodes[0].Nodes.Clear();
            foreach (Enterprises enterprises in _enterprise.EnterpriseList)
            {
                TreeNode childNode = new TreeNode(enterprises.RouteName);
                childNode.Tag = enterprises;
                tvEnterprise.Nodes[0].Nodes.Add(childNode);
            }
        }
        #endregion

        #region Tool Bar New Click
        /// <summary>
        /// Tool Bar New Click
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    EnterpriseCtrl ctrl = (EnterpriseCtrl)viewContent.Control.Controls.Find("EnterpriseCtrl", true)[0];
                    if (ctrl.txtEnterpriseName.Text.Trim() != "")
                    {
                        if (MessageBox.Show(StringParser.Parse("${res:Global.ClearNoteMessage}"),
                            StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }

            EnterpriseViewContent enterpriseContent = new EnterpriseViewContent(new EnterpriseEntity());
            WorkbenchSingleton.Workbench.ShowView(enterpriseContent);
        }
        #endregion

        #region Tool Bar Query Click
        /// <summary>
        /// Tool Bar Query Click
        /// </summary>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            EnterpriseSearchDialog enterprise = new EnterpriseSearchDialog();
            if (DialogResult.OK == enterprise.ShowDialog())
            {
                if (string.Empty == enterprise.EnterpriseKey || enterprise.EnterpriseKey.Length < 1)
                    return;
                if (string.Empty == enterprise.EnterpriseName || enterprise.EnterpriseName.Length < 1)
                    return;
                if (string.Empty == enterprise.EnterpriseVersion || enterprise.EnterpriseVersion.Length < 1)
                    return;

                string title = 
                    StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}") + "_" + 
                    enterprise.EnterpriseName + "." + enterprise.EnterpriseVersion;

                foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (viewContent.TitleName == title)
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }

                EnterpriseViewContent enterpriseContent = new EnterpriseViewContent(new EnterpriseEntity(enterprise.EnterpriseKey));
                WorkbenchSingleton.Workbench.ShowView(enterpriseContent);
            }
        }
        #endregion

        #region Tool Bar Save Click
        /// <summary>
        /// Tool Bar Save Click
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                MapControlsToEnterprise();

                if (_enterprise.EnterpriseName == string.Empty)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (CtrlState == ControlState.New || CtrlState == ControlState.Read)
                {
                    if (CtrlState == ControlState.Read)
                    {
                        _enterprise.EnterpriseKey =  CommonUtils.GenerateNewKey(0);
                        _enterprise.Status = EntityStatus.InActive;
                    }
                    if (_enterprise.Insert())
                    {
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                            = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}") + "_"
                            + _enterprise.EnterpriseName + "."
                            + _enterprise.EnterpriseVersion;

                        CtrlState = ControlState.Edit;
                    }
                }
                else
                {
                    if (_enterprise.Update())
                    {
                        CtrlState = ControlState.Edit;
                    }
                }

                _enterprise = new EnterpriseEntity(_enterprise.EnterpriseKey);
                MapEnterpriseToControls();
            }
        }
        #endregion

        #region Tool Bar Delete Click
        /// <summary>
        /// Tool Bar Delete Click
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            {
                if (_enterprise.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == 
                            StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }

                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName = 
                        StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseViewContent.TitleName}");
                    CtrlState = ControlState.New;
                }
            }
        }
        #endregion

        #region set active status
        /// <summary>
        /// set active status
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _enterprise.Status;
            //show dialog
            StatusDialog status = new StatusDialog(_enterprise);
            status.ShowDialog();

            if (_enterprise.Status != oldStatus)
            {
                //set page control status according to status
                if (_enterprise.Status == EntityStatus.Active)
                {
                    CtrlState = ControlState.Read;
                }
                if (_enterprise.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        #endregion

        #region Listview and gridCtrl move


        /// <summary>
        /// Item drag
        /// </summary>
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

             // Copy the dragged node when the right mouse button is used.
            else if (e.Button == MouseButtons.Right)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }


        /// <summary>
        /// Drag enter
        /// </summary>
        // Set the target drop effect to the effect 
        // specified in the ItemDrag event handler.
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }


    
        /// <summary>
        /// Drag over
        /// </summary>
        // Select the node under the mouse pointer to indicate the 
        // expected drop location.
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = tvEnterprise.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            tvEnterprise.SelectedNode = tvEnterprise.GetNodeAt(targetPoint);
        }



        /// <summary>
        /// Drag drop
        /// </summary>
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = tvEnterprise.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = tvEnterprise.GetNodeAt(targetPoint);
            TreeNode draggedNode = new TreeNode();

            if (_enterprise.Status == EntityStatus.Active || _enterprise.Status == EntityStatus.Archive
                                                                      || targetNode == null)
                return;

            draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (draggedNode != null)
            {
                Enterprises updateEnterprise = (Enterprises)draggedNode.Tag;
                if (updateEnterprise.OperationAction != OperationAction.New)
                {
                    updateEnterprise.OperationAction = OperationAction.Modified;
                }
            }

            DataRow dataRow = (DataRow)e.Data.GetData(typeof(DataRow));
            if (dataRow != null)
            {
                Enterprises enterprises = new Enterprises(dataRow);
                ListViewItem lstItem = new ListViewItem(enterprises.RouteName, 0);
                draggedNode = new TreeNode(lstItem.SubItems[0].Text);
                enterprises.OperationAction = OperationAction.New;
                draggedNode.Tag = enterprises;
            }

            // Confirm that the node at the drop location is not 
            // the dragged node or a descendant of the dragged node.
            if (!draggedNode.Equals(targetNode))
            {
                // If it is a move operation, remove the node from its current 
                // location and add it to the node at the drop location.
                if (e.Effect == DragDropEffects.Move)
                {
                    draggedNode.Remove();
                    if (targetNode.Parent == null)
                        targetNode.Nodes.Add(draggedNode);
                    else
                        targetNode.Parent.Nodes.Insert(targetNode.Index + 1, draggedNode);
                }
                // If it is a copy operation, clone the dragged node 
                // and add it to the node at the drop location.
                else if (e.Effect == DragDropEffects.Copy)
                {
                    if (targetNode.Parent == null)
                        targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
                    else
                        targetNode.Parent.Nodes.Insert(targetNode.Index + 1, (TreeNode)draggedNode.Clone());
                }

                // Expand the node at the location 
                // to show the dropped node.
                targetNode.Expand();
            }
        }


        #region Grid mouse down
        /// <summary>
        /// Grid mouse down
        /// </summary>
        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            downHitInfo = null;
            GridHitInfo hitInfo = gridViewRoute.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;
        }
        #endregion

        #region Grid mouse move
        /// <summary>
        /// Grid mouse move
        /// </summary>
        private void gridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && downHitInfo != null && _enterprise.Status != EntityStatus.Active
                                                                     && _enterprise.Status != EntityStatus.Archive)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    object row = gridViewRoute.GetDataRow(downHitInfo.RowHandle);
                    gridViewRoute.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }

        }
        #endregion
        #endregion

        #region Enterprise name change Event
        /// <summary>
        /// Enterprise name change Event
        /// </summary> 
        private void txtEnterpriseName_TextChanged(object sender, EventArgs e)
        {
            if (tvEnterprise.Nodes.Count == 0)
            {
                this.tvEnterprise.Nodes.Add(this.txtEnterpriseName.Text);
            }
            else
            {
                this.tvEnterprise.Nodes.Clear();
                this.tvEnterprise.Nodes.Add(this.txtEnterpriseName.Text);
            }
        }

        #endregion

        #region Tree view right click delete event
        /// <summary>
        /// Tree view right click delete event
        /// </summary>      
        private void tvEnterprise_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                TreeNode targetNode = tvEnterprise.GetNodeAt(e.X, e.Y);
                tvEnterprise.SelectedNode = targetNode;
                if (targetNode != null)
                {
                    if (targetNode.Parent != null && _enterprise.Status != EntityStatus.Active
                                                  && _enterprise.Status != EntityStatus.Archive)
                    {
                        ContextMenu contextMenu = new ContextMenu();
                        contextMenu.MenuItems.Clear();
                        contextMenu.MenuItems.Add(StringParser.Parse("${res:Global.Delete}"), new EventHandler(Delete_Clicked));

                        Point p = new Point(Control.MousePosition.X, Control.MousePosition.Y);
                        p = this.PointToClient(p);
                        contextMenu.Show(this, p);
                    }
                }
            }


        }
        #endregion

        #region Tree view right click delete function
        /// <summary>
        /// Tree view right click delete function
        /// </summary>        
        void Delete_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < tvEnterprise.Nodes[0].Nodes.Count; i++)
            {
                if (tvEnterprise.Nodes[0].Nodes[i].IsSelected == true)
                {
                    tvEnterprise.Nodes[0].Nodes[i].Remove();
                    break;
                }
            }
        }
        #endregion

        #region Get max version via route name
        /// <summary>
        /// Get max version via route name
        /// </summary>    
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Hashtable hashTable = new Hashtable();
            DataSet dataSet = new DataSet();

            string strName = this.txtName.Text.Trim();

            if (strName.Length > 0)
            {
                hashTable.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME, strName);

                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                dataTable.TableName = POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(dataTable);
            }
            _enterprise.GetMaxVerRoute(ref routeTable, dataSet);
            grdCtrlRoute.MainView = gridViewRoute;
            grdCtrlRoute.DataSource = null;
            grdCtrlRoute.DataSource = routeTable;
        }
        #endregion

        #region Load Resource file data change UI languages
        /// <summary>
        /// Load Resource file data change UI languages
        /// </summary>
        private void EnterpriseCtrl_Load(object sender, EventArgs e)
        {
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");

            this.EnterpriseBaseTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.EnterpriseBaseTabPage}");
            this.EnterpriseEditTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.EnterpriseEditTabPage}");
            this.lblEnterpriseName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.lblEnterpriseName}");
            this.lblDescription.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.lblDescription}");
            this.gridColumn_RouteName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_RouteName}");
            this.gridColumn_RouteDescription.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_RouteDescription}");
            this.gridColumn_RouteSeqence.Caption = 
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_RouteSeqence}");
            this.grpCtrlMaintainLeft.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.grpCtrl1}");
            this.grpCtrlMaintainRight.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.grpCtrl2}");
            this.lblName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.lblName}");
            this.btnSearch.Text=
                 StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.btnSearch}");
            this.gridColumn_Name.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_Name}");
            this.gridColumn_Description.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_Description}");
            this.gridColumn_Version.Caption =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.EnterpriseCtrl.gridColumn_Version}");
        }
        #endregion

        #region Private variable definition

        private GridHitInfo downHitInfo = null;
        private ControlState _ctrlState = ControlState.Empty;

        private EnterpriseEntity _enterprise = null;
        private DataTable routeTable = new DataTable();
        #endregion

        private void gridViewEnterprise_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridViewRoute_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

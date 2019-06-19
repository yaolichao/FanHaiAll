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

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示工艺流程管理界面的控件类。
    /// </summary>
    public partial class RouteCtrl : BaseUserCtrl
    {
        private GridHitInfo downHitInfo = null;
        private ControlState _ctrlState = ControlState.Empty;
        private RouteEntity _route = null;
        private List<OperationEntity> operationList = new List<OperationEntity>();
        /// <summary>
        /// 状态改变委托。
        /// </summary>
        /// <param name="controlState"></param>
        private new delegate void AfterStateChanged(ControlState controlState);
        /// <summary>
        /// 状态改变事件。
        /// </summary>
        private new AfterStateChanged afterStateChanged = null;
        /// <summary>
        /// 状态。
        /// </summary>
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
        /// 状态改变事件方法。
        /// </summary>
        /// <param name="state">控件状态。</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                case ControlState.Edit:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;
                    txtRouteName.Properties.ReadOnly = true;
                    RouteTabCtrl.TabPages.Remove(RouteLineTabPage);
                    break;
                case ControlState.New:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    txtRouteName.Properties.ReadOnly = false;
                    RouteTabCtrl.TabPages.Remove(RouteLineTabPage);
                    _route = new RouteEntity();
                    MapRouteToControls();
                    break;
                case ControlState.ReadOnly:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    txtRouteName.Properties.ReadOnly = true;
                    break;
                case ControlState.Read:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = true;

                    txtRouteName.Properties.ReadOnly = true;
                    dtBeginTime.Properties.ReadOnly = true;
                    dtEndTime.Properties.ReadOnly = true;
                    mmDescription.Properties.ReadOnly = true;

                    RouteTabCtrl.TabPages.Add(RouteLineTabPage);
                    gcRouteLine.MainView = gvRouteLine;
                    gcRouteLine.DataSource = _route.GetRouteLineRelation();
                    break;
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public RouteCtrl(RouteEntity route)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            _route = route;
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void RouteCtrl_Load(object sender, EventArgs e)
        {
            if (null == _route || _route.RouteVerKey.Length < 1)
            {
                return;
            }
            //新增。
            else if (_route.RouteVerKey.Length > 1 && _route.RouteName == string.Empty)
            {
                CtrlState = ControlState.New;
            }
            else
            {
                //修改或只读。
                MapRouteToControls();
                if (_route.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else
                    CtrlState = ControlState.Read;
            }
            _route.GetMaxVerOperation(ref operationList, null);
            gcOperation.MainView = gvOperation;
            gcOperation.DataSource = operationList;
        }
        /// <summary>
        /// 映射工艺流程数据到对应的控件中。
        /// </summary>
        private void MapRouteToControls()
        {
            txtRouteName.Text = _route.RouteName;
            dtBeginTime.Text = _route.RouteEffectivityStart;
            dtEndTime.Text = _route.RouteEffectivityEnd;
            mmDescription.Text = _route.RouteDescription;

            grdCtrlStep.MainView = gridViewStep;
            grdCtrlStep.DataSource = _route.StepList;

            tvRoute.Nodes[0].Nodes.Clear();
            foreach (StepEntity step in _route.StepList)
            {
                TreeNode childNode = new TreeNode(step.StepName);
                childNode.Tag = step;
                tvRoute.Nodes[0].Nodes.Add(childNode);
            }
        }
        /// <summary>
        /// 验证并收集工艺流程数据。
        /// </summary>
        private void MapControlsToRoute()
        {
            if (null == _route)
            {
                throw (new Exception("Error Route Set"));
            }
            _route.RouteName = txtRouteName.Text;
            _route.RouteEffectivityStart = dtBeginTime.Text;
            _route.RouteEffectivityEnd = dtEndTime.Text;
            _route.RouteDescription = mmDescription.Text;

            _route.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            _route.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            TreeNode parentNode = tvRoute.Nodes[0];
            bool blFlag = false;
            foreach (TreeNode node in parentNode.Nodes)
            {
                blFlag = false;
                StepEntity step = (StepEntity)node.Tag;
                if(_route.StepList.Count>0)
                {
                    foreach (StepEntity item in _route.StepList)
                    {
                        if (step.StepKey == item.StepKey)
                        {
                            blFlag = true;
                            if (node.Index.ToString() != item.StepSeqence)
                            {
                                item.StepSeqence = node.Index.ToString();
                                item.OperationAction = OperationAction.Modified;
                            }
                            break;
                        }
                    }
                    
                }
                //if not found the key in the list ,add this step to the route 
                if (blFlag == false)
                {
                    step.StepSeqence = node.Index.ToString();
                    _route.StepList.Add(step);
                }
            }

            foreach (StepEntity item in _route.StepList)
            {
                blFlag = false;
                foreach (TreeNode node in parentNode.Nodes)
                {
                    StepEntity step = (StepEntity)node.Tag;
                    if (item.StepKey == step.StepKey)
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
        /// <summary>
        /// 查询按钮。
        /// </summary>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            RouteSearchDialog route = new RouteSearchDialog();
            if (DialogResult.OK == route.ShowDialog())
            {
                if (string.Empty == route.RouteKey || route.RouteKey.Length < 1)
                    return;
                if (string.Empty == route.RouteName || route.RouteName.Length < 1)
                    return;
                if (string.Empty == route.RouteVersion || route.RouteVersion.Length < 1)
                    return;

                string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}")
                             + "_" + route.RouteName + "." + route.RouteVersion;

                foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (viewContent.TitleName == title)
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }

                RouteViewContent routeContent = new RouteViewContent(new RouteEntity(route.RouteKey));
                WorkbenchSingleton.Workbench.ShowView(routeContent);
            }
        }
        /// <summary>
        /// 新增按钮。
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    RouteCtrl ctrl = (RouteCtrl)viewContent.Control.Controls.Find("RouteCtrl", true)[0];
                    if (ctrl.txtRouteName.Text.Trim() != "")
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

            RouteViewContent routeContent = new RouteViewContent(new RouteEntity());
            WorkbenchSingleton.Workbench.ShowView(routeContent);
        }
        /// <summary>
        /// 保存按钮。
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {

                if (string.IsNullOrEmpty(txtRouteName.Text))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                //如果开始时间和结束时间有输入。
                if (!string.IsNullOrEmpty(dtBeginTime.Text) && !string.IsNullOrEmpty(dtEndTime.Text))
                {
                    DateTime dtStart = DateTime.Parse(dtBeginTime.Text);
                    DateTime dtEnd = DateTime.Parse(dtEndTime.Text);
                    if (dtEnd < dtStart)//结束时间必须大于开始时间。
                    {
                        MessageBox.Show("结束时间必须大于开始时间。", "提示");     //请填写完整再保存
                        dtEndTime.Focus();
                        return;
                    }
                }
                MapControlsToRoute();

                if (CtrlState == ControlState.New || CtrlState == ControlState.Read)
                {
                    if (CtrlState == ControlState.Read)
                    {
                        _route.RouteVerKey =  CommonUtils.GenerateNewKey(0);
                        _route.Status = EntityStatus.InActive;
                    }
                    if (_route.Insert())
                    {
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                            = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}") + "_"
                            + _route.RouteName + "."
                            + _route.RouteVersion;
                        CtrlState = ControlState.Edit;
                    }
                }
                else
                {
                    if (_route.Update())
                    {
                        CtrlState = ControlState.Edit;
                    }
                }
                _route = new RouteEntity(_route.RouteVerKey);
                MapRouteToControls();
            }
        }
        /// <summary>
        /// 删除按钮。
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            {
                if (_route.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName = 
                        StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteViewContent.TitleName}");
                    CtrlState = ControlState.New;
                }
            }
        }
        /// <summary>
        /// 状态按钮。
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _route.Status;
            //show dialog
            StatusDialog status = new StatusDialog(_route);
            status.ShowDialog();

            if (_route.Status != oldStatus)
            {
                //set page control status according to status
                if (_route.Status == EntityStatus.Active)
                {
                    CtrlState = ControlState.Read;
                }
                if (_route.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        /// <summary>
        /// 添加工艺流程和线别的关联。
        /// </summary>       
        private void btnAdd_Click(object sender, EventArgs e)
        {
            RouteLineSearchDialog routeLine = new RouteLineSearchDialog(_route.RouteVerKey);
            routeLine.ShowDialog();
            gcRouteLine.MainView = gvRouteLine;
            gcRouteLine.DataSource = _route.GetRouteLineRelation();
        }
        /// <summary>
        /// 删除工艺流程和线别的管理。
        /// </summary>
        private void btnDel_Click(object sender, EventArgs e)
        {
            int rowHandle = gvRouteLine.FocusedRowHandle;
            string strLineKey = Convert.ToString(this.gvRouteLine.GetRowCellValue(rowHandle, "PRODUCTION_LINE_KEY"));
            string strRouteKey = Convert.ToString(this.gvRouteLine.GetRowCellValue(rowHandle, "ROUTE_ROUTE_VER_KEY"));
            string strLineCode = Convert.ToString(this.gvRouteLine.GetRowCellValue(rowHandle, "LINE_CODE"));
            string strLineName = Convert.ToString(this.gvRouteLine.GetRowCellValue(rowHandle, "LINE_NAME"));

            if (MessageBox.Show(StringParser.Parse("${res:Global.DeleteNoteMessage}"),
               StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_route.DeleteRouteLineRelation(strRouteKey, strLineKey))
                    this.gvRouteLine.DeleteRow(rowHandle);
            }
        }
        /// <summary>
        /// 工艺流程中工步双击事件。
        /// </summary>
        private void tvRoute_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvRoute.Nodes[0].Nodes.Count > 0 && tvRoute.SelectedNode!=null && tvRoute.SelectedNode.Tag != null)
            {
                StepEntity step = (StepEntity)tvRoute.SelectedNode.Tag;
                step.Status = _route.Status;
                StepDialog stepDialog = new StepDialog(step);
                stepDialog.ShowDialog();
            }
        }
        /// <summary>
        /// 工艺流程列表拖拽事件。
        /// </summary>
        private void tvRoute_ItemDrag(object sender, ItemDragEventArgs e)
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
        /// 拖拽进入工艺流程列表上。
        /// </summary>
        private void tvRoute_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        /// <summary>
        /// 拖拽停放在工艺流程列表上。
        /// </summary>
        private void tvRoute_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = tvRoute.PointToClient(new Point(e.X, e.Y));
            // Select the node at the mouse position.
            tvRoute.SelectedNode = tvRoute.GetNodeAt(targetPoint);
        }
        /// <summary>
        /// 拖拽在工艺流程列表上放下。
        /// </summary>
        private void tvRoute_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = tvRoute.PointToClient(new Point(e.X, e.Y));
            // Retrieve the node at the drop location.
            TreeNode targetNode = tvRoute.GetNodeAt(targetPoint);
            TreeNode draggedNode = new TreeNode();

            if (_route.Status == EntityStatus.Active || _route.Status == EntityStatus.Archive || targetNode == null)
                return;

            draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (draggedNode != null)
            {
                StepEntity updateStep = (StepEntity)draggedNode.Tag;
                if (updateStep.OperationAction != OperationAction.New)
                {
                    updateStep.OperationAction = OperationAction.Modified;
                }
            }

            OperationEntity operation = (OperationEntity)e.Data.GetData(typeof(OperationEntity));
            if (operation != null)
            {
                StepEntity newStep = new StepEntity(operation);
                ListViewItem lstItem = new ListViewItem(newStep.StepName, 0);
                draggedNode = new TreeNode(lstItem.SubItems[0].Text);
                newStep.OperationAction = OperationAction.New;
                draggedNode.Tag = newStep;
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
        /// <summary>
        /// 工序列表鼠标按下事件。
        /// </summary>
        private void gcOperation_MouseDown(object sender, MouseEventArgs e)
        {
            downHitInfo = null;
            GridHitInfo hitInfo = gvOperation.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;
        }
        /// <summary>
        /// 工序列表鼠标移动事件。
        /// </summary>
        private void gcOperation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && downHitInfo != null 
                && _route.Status != EntityStatus.Active
                && _route.Status != EntityStatus.Archive)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    object row = gvOperation.GetRow(downHitInfo.RowHandle);
                    gvOperation.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }
        /// <summary>
        /// 工艺流程组名称改变事件。
        /// </summary> 
        private void txtRouteName_TextChanged(object sender, EventArgs e)
        {
            if (tvRoute.Nodes.Count == 0)
            {
                this.tvRoute.Nodes.Add(this.txtRouteName.Text);
            }
            else
            {
                this.tvRoute.Nodes.Clear();
                this.tvRoute.Nodes.Add(this.txtRouteName.Text);
            }
        }
        /// <summary>
        /// 工艺流程列表鼠标按下事件。
        /// </summary>      
        private void tvRoute_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //显示右键菜单。
                TreeNode targetNode = tvRoute.GetNodeAt(e.X, e.Y);
                tvRoute.SelectedNode = targetNode;
                if (targetNode != null && targetNode.Parent != null 
                    && _route.Status != EntityStatus.Active 
                    && _route.Status != EntityStatus.Archive)
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
        /// <summary>
        /// 工艺流程列表删除方法。
        /// </summary>        
        void Delete_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; i < tvRoute.Nodes[0].Nodes.Count; i++)
            {
                if (tvRoute.Nodes[0].Nodes[i].IsSelected == true)
                {
                    tvRoute.Nodes[0].Nodes[i].Remove();
                    break;
                }
            }
        }
        /// <summary>
        /// 查询最大版本号的工序。
        /// </summary>    
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Hashtable htQueryParams = new Hashtable();
            DataSet dsQueryParams = new DataSet();
            string strName = this.txtName.Text.Trim();
            if (strName.Length > 0)
            {
                htQueryParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, strName);
                DataTable dtQueryParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htQueryParams);
                dtQueryParams.TableName = POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME;
                dsQueryParams.Tables.Add(dtQueryParams);
            }
            _route.GetMaxVerOperation(ref operationList, dsQueryParams);
            gcOperation.MainView = gvOperation;
            gcOperation.DataSource = null;
            gcOperation.DataSource = operationList;
        }
        /// <summary>
        /// 根据区域特性初始化界面资源。
        /// </summary>
        protected override void InitUIResourcesByCulture()
        {
            GridViewHelper.SetGridView(gridViewStep);
            GridViewHelper.SetGridView(gvOperation);
            GridViewHelper.SetGridView(gvRouteLine);
            
            lblMenu.Text = "基础数据>流程管理>工序流程";

            base.InitUIResourcesByCulture();
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");

            this.RouteBaseTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.RouteBaseTabPage}");
            this.RouteEditTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.RouteEditTabPage}");
            this.RouteLineTabPage.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.RouteLineTabPage}");
            this.lblRouteName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.lblRouteName}");
            this.lblBeginTime.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.lblBeginTime}");
            this.lblEndTime.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.lblEndTime}");
            this.lblDescription.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.lblDescription}");
            this.gridColumn_stepName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_stepName}");
            this.gridColumn_stepDescription.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_stepDescription}");
            this.gridColumn_stepIsReWork.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_stepIsReWork}");
            this.gridColumn_stepEditor.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_stepEditor}");
            this.gridColumn_stepEditorTime.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_stepEditorTime}");
            this.grpCtrlStep.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.grpCtrlStep}");
            this.grpCrtlOperation.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.grpCrtlOperation}");
            this.lblName.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.lblName}");
            this.btnSearch.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.btnSearch}");
            this.gridColumn_Name.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_Name}");
            this.gridColumn_Description.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_Description}");
            this.gridColumn_Version.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_Version}");
            this.btnAdd.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.btnAdd}");
            this.btnDel.Text =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.btnDel}");
            this.gridColumn_RouteName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_RouteName}");
            this.gridColumn_LineCode.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_LineCode}");
            this.gridColumn_LineName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_LineName}");
            this.gridColumn_Descriptions.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.RouteCtrl.gridColumn_Descriptions}");
        }

        private void gridViewStep_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvOperation_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvRouteLine_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

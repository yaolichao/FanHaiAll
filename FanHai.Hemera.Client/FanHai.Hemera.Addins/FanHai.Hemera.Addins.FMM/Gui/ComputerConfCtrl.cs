using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class ComputerConfCtrl : BaseUserCtrl
    {
        #region Private variable definitions       
        //private WorkOrders _workOrders = null;
        private ComputerEntity _computerEntity = null;
        private UdaCommonControl _udaCommonControl = null;
        private ControlState _controlState = ControlState.Empty;
        private new delegate void AfterStateChanged(ControlState controlState);
        private new AfterStateChanged afterStateChanged = null;       
      
        #endregion

        #region constructor
        public ComputerConfCtrl()
        {
            InitializeComponent();
        }
        #endregion
        private string _computerKey = "";
        private string _computerName = "";
        private string _computerDescription = "";
        public string ComputerKey
        {
            get
            {
                return _computerKey;
            }
        }
        public string ComputerName
        {
            get
            {
                return _computerName;
            }
        }
        public string ComputerDescription
        {
            get
            {
                return _computerDescription;
            }
        }
        #region toolbar event
        public ComputerConfCtrl(ComputerEntity computerEntity)
        {
            InitializeComponent();
            _computerEntity = computerEntity;

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);
            if (_computerEntity.ComputerName.Length < 1)
            {
                _udaCommonControl = new UdaCommonControl(EntityType.Computer, "");
                _udaCommonControl.UserDefinedAttrs = _computerEntity.UserDefinedAttrs;
                State = ControlState.New;
            }
            else
            {
                _udaCommonControl = new UdaCommonControl(EntityType.Computer, _computerEntity.CodeKey);
                MapComputerToControl();

                if (_computerEntity.Status == EntityStatus.InActive)
                    State = ControlState.Edit;
                else
                    State = ControlState.ReadOnly;
            }
            _udaCommonControl.Dock = DockStyle.Fill;
            UdaPanel.Controls.Add(_udaCommonControl);
        }
        #endregion

        #region State Change
        private new ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;
                _udaCommonControl.CtrlState = _controlState;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Deal with state change event
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {
                #region case state of empty
                case ControlState.Empty:
                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    //tsbStatus.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly/Read
                case ControlState.ReadOnly:
                case ControlState.Read:                                     
                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    //tsbStatus.Enabled = true;
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = true;
                    //tsbStatus.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:                  
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = false;
                    //tsbStatus.Enabled = false;
                    _udaCommonControl.LinkedToItemKey = _computerEntity.CodeKey;
                    break;
                #endregion
            }
        }
        
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            //载入计算机维护-查询视图
            ComputerSearchDialog orderSearch = new ComputerSearchDialog();
            //计算机维护-查询视图返回值为OK执行下面操作 
            if (DialogResult.OK == orderSearch.ShowDialog())
            {
                //返回值的主键为空执行下面操作  
                if (null == orderSearch.ComputerKey || orderSearch.ComputerKey.Length < 1)
                    return;
                //返回值的计算机名称为空就是没有值执行下面操作 
                if (null == orderSearch.ComputerName || orderSearch.ComputerName.Length < 1)
                    return;

                //title=计算机维护_计算机名称 
                string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerConfCtrl.Title}") + "_" + orderSearch.ComputerName;
                //string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}") + "_" + orderSearch.ComputerName;
                foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (viewContent.TitleName == title)
                    {
                        viewContent.WorkbenchWindow.SelectWindow();
                        return;
                    }
                }
                //新建视图窗体 
                ComputerViewContext OrderContent = new ComputerViewContext(new ComputerEntity(orderSearch.ComputerName));
                WorkbenchSingleton.Workbench.ShowView(OrderContent);
            }
        }
        /// <summary>
        /// Search button click 查询按钮的单击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            Hashtable mainDataHashTable = new Hashtable();                                                      //定义hashtable对象 
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(COMPUTER_FIELDS.FIELDS_COMPUTER_NAME, txtComputerName.Text.Trim().ToUpper()); //将txtComputerName的值去空格和全部大写话到hashtable对象COMPUTER_NAME列中 
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = COMPUTER_FIELDS.DATABASE_TABLE_NAME;                                      //表明为COMPUTER_CONFIG 
            dataSet.Tables.Add(mainDataTable);
            //Call Remoting Service
            try
            {
                //远程调用技术 
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIComputerEngine().SearchComputers(dataSet);                    //调用SearchComputers方法传入表集dataset获取查询数据 

                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    //返回值为-1 则提示错误 
                    if (null == returnMessage || returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    //返回值不为-1 
                    else
                    {
                        BindDataSourceToGrid(retDS.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// bind data to GridView
        /// </summary>
        /// <param name="dataSet"></param>
        private void BindDataSourceToGrid(DataTable dt)
        {
            gridData.MainView = gridDataView;
            gridData.DataSource = dt;
        }
        private void tsbNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}"))
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerConfCtrl.Title}"))     //视图名为计算机维护执行下面操作 midi by chao.pang
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            //新建视图窗体 
            WorkbenchSingleton.Workbench.ShowView(new ComputerViewContext(new ComputerEntity()));
        }
        /// <summary>
        /// grid Control Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridData_DoubleClick(object sender, EventArgs e)
        {
            MapSelectedItemToProperties();

            //返回值的主键为空执行下面操作  
            if (null == ComputerKey ||ComputerKey.Length < 1)
                return;
            //返回值的计算机名称为空就是没有值执行下面操作 
            if (null == ComputerName || ComputerName.Length < 1)
                return;
            ComputerEntity computerEntity = new ComputerEntity(ComputerName);
            computerEntity.CodeKey = ComputerKey;
            computerEntity.ComputerName = ComputerName;
            computerEntity.ComputerDesc = ComputerDescription;
            _computerEntity = computerEntity;
            MapComputerToControl();
            State = ControlState.Edit;
            OnAfterStateChanged(State);
            //title=计算机维护_计算机名称 
            //string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerConfCtrl.Title}") + "_" + ComputerName;
            ////string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}") + "_" + orderSearch.ComputerName;
            //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //{
            //    if (viewContent.TitleName == title)
            //    {
            //        viewContent.WorkbenchWindow.SelectWindow();
            //        return;
            //    }
            //}
            ////新建视图窗体 
            //ComputerViewContext OrderContent = new ComputerViewContext(new ComputerEntity(ComputerName));
            //WorkbenchSingleton.Workbench.ShowView(OrderContent);
            //if (MapSelectedItemToProperties())
            //{
            //    //DialogResult = DialogResult.OK;
            //}
        }
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridDataView.GetDataRowHandleByGroupRowHandle(gridDataView.FocusedRowHandle);
            if (rowHandle >= 0)
            {
               // _workOrderKey = gridWorkOrdersView.GetRowCellValue(rowHandle,POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY).ToString();
                _computerKey = gridDataView.GetRowCellValue(rowHandle, COMPUTER_FIELDS.FIELDS_CODE_KEY).ToString();
                _computerName = gridDataView.GetRowCellValue(rowHandle, COMPUTER_FIELDS.FIELDS_COMPUTER_NAME).ToString();
                _computerDescription = gridDataView.GetRowCellValue(rowHandle, COMPUTER_FIELDS.FIELDS_DESCRIPTION).ToString();
                // _workOrderNumber =gridWorkOrdersView.GetRowCellValue(rowHandle, "ORDER_NUMBER").ToString();
                return true;
            }
            return false;
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //系统提示确定要删除吗？ 返回值为true执行以下操作 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                //计算机名称为空 
                if (txtCompName.Text  == string.Empty )
                {
                    //提示 请填写完整再保存 
                    MessageService.ShowWarning("${res:FanHai.Hemera.Addins.WIP.LotEDCDialog.CheckNULL}");
                    return;
                }

                MapControlToComputer();
                //为新增数据 
                if (ControlState.New == State)//新增
                {
                    //插入数据返回值为true 插入成功 
                    if (_computerEntity.Insert())
                    {
                        //系统提示保存成功 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        //计算机维护_计算机名称 
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                         = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerConfCtrl.Title}") + "_"
                         + _computerEntity.ComputerName;
                        //状态改为修改edit 
                        State = ControlState.Edit;
                    }
                }
                //状态为edit修改时 
                else if (ControlState.Edit == State) //编辑
                {
                    //修改数据返回值为true 修改成功 
                    if (_computerEntity.Update())
                    {
                        //系统提示更新成功！ 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                        //状态改为修改状态 
                        State = ControlState.Edit;
                    }
                }
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //系统提示确定要删除吗？返回值为true 执行下面操作 
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}", "${res:Global.SystemInfo}"))
            {
                //状态state为edit修改状态 执行下面操作 
                if (ControlState.Edit == State)
                {
                    //调用delete方法删除数据返回值为true说明执行成功 
                    if (_computerEntity.Delete())
                    {
                        //提示删除成功！ 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}");
                        WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);

                        foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                        {
                            //如果视图名称为计算机维护 执行下面的操作 modi by chao.pnag
                            if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ComputerConfCtrl.Title}"))
                            {
                                viewContent.WorkbenchWindow.SelectWindow();
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void tsbStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _computerEntity.Status;
            StatusDialog status = new StatusDialog(_computerEntity);
            status.ShowDialog();
            if (oldStatus != _computerEntity.Status)
            {
                if (_computerEntity.Status == EntityStatus.Active)
                {
                    State = ControlState.Read;
                }
                if (_computerEntity.Status == EntityStatus.Archive)
                {
                    State = ControlState.ReadOnly;
                }
            }
        }
        #endregion

        #region private function
        private void MapComputerToControl()
        {
            txtCompName.Text = _computerEntity.ComputerName;
            txtCompDesc.Text = _computerEntity.ComputerDesc;
            //lblKey.Text      = _computerEntity.CodeKey;   //Delete by feng 20111028   //不显示codekey，需求见redmine[Hemera_-_功能_#23]
            _udaCommonControl.UserDefinedAttrs = _computerEntity.UserDefinedAttrs;
        }

        private void MapControlToComputer()
        {
            _computerEntity.ComputerName = txtCompName.Text;
            _computerEntity.ComputerDesc = txtCompDesc.Text;
            //_computerEntity.UserDefinedAttrs = _udaCommonControl.UserDefinedAttrs;
            //set uda control's status add by rayna 2011-5-18
            ColumnView view =
                ((DevExpress.XtraGrid.GridControl)(this._udaCommonControl.Controls.Find("gridUDAs", true)[0])).FocusedView as ColumnView;
            view.CloseEditor();
            view.UpdateCurrentRow();
            //end
        }
        private void InitUI()
        {
            lblMenu.Text = "基础数据 > 配置管理 > 终端配置";

            //toolbar
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            //this.tsbSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            //this.tsbStatus.Text = StringParser.Parse("${res:Global.Status}");

            //tabPage

            this.BaseTabPage.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.BasePage}");
            this.UdaTabPage.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.UdaPage}");

            lblComputeName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ComputerConfCtrl.lbl.0002}");//计算机名称:
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ComputerConfCtrl.lbl.0003}");//计算机描述:
            //label           
            //this.lblOrderVersion.Text = StringParser.Parse("${res:Dialog.About.label1Text}");
            //this.lblWorkOrder.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblWorkOrderNumber}");
            //this.lblPart.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblParNumber}");
            //this.lblWorkOrderQuantity.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblWorkOrderQuantity}");
            //this.lblRemark.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.Gui.WorkOrderCtrl.lblRemark}");
            //this.lblRevenueType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblRevenueType}");
            //this.lblSuppliers.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderCtrl.lblSupplier}");         
        }
        #endregion


        private void ComputerConf_Load(object sender, EventArgs e)
        {
            InitUI();

            //this.txtCompName.Text = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            //this.lblKey.Text = System.Environment.MachineName.ToString();
        }

     
    }
}


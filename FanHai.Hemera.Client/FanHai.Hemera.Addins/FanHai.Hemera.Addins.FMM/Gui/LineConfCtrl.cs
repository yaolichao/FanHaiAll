using DevExpress.XtraGrid.Views.Base;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.UDA;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;



namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示线别维护界面的用户自定义控件类。
    /// </summary>
    public partial class LineConfCtrl : BaseUserCtrl
    {
        #region Private variable definitions       
        private UdaEntity _objectEntity = null;

        private UdaCommonControlEx _udaCommonControl = null;
        private ControlState _controlState = ControlState.Empty;
        private new delegate void AfterStateChanged(ControlState controlState);
        private new AfterStateChanged afterStateChanged = null;
        private bool isMainUpdate = false;
        string _objCategory = null;
        #endregion

        #region constructor
        public LineConfCtrl()
        {
            InitializeComponent();
        }
        public LineConfCtrl(UdaEntity udaEntity, EntityType objectType)
        {
            InitializeComponent();
            //this.gridDataView.DoubleClick += new System.EventHandler(this.gridDataView_DoubleClick);
            _objectEntity = udaEntity;
            _objCategory = objectType.ToString();

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);
            if (_objectEntity.ObjectName.Length < 1)
            {
                _udaCommonControl = new UdaCommonControlEx(objectType, "", "");
                _udaCommonControl.UserDefinedAttrs = _objectEntity.UserDefinedAttrsEx;
                State = ControlState.New;
            }
            else
            {
                _udaCommonControl = new UdaCommonControlEx(objectType, _objectEntity.ObjectKey, _objectEntity.LinkedToTable);
                MapAttributeToControl();

                if (_objectEntity.Status == EntityStatus.InActive)
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
                    tsbStatus.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly/Read
                case ControlState.ReadOnly:
                case ControlState.Read:
                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbStatus.Enabled = true;
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbStatus.Enabled = true;
                    _udaCommonControl.LinkedToItemKey = _objectEntity.ObjectKey;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = false;
                    tsbStatus.Enabled = false;
                    _udaCommonControl.LinkedToItemKey = _objectEntity.ObjectKey;
                    break;
                    #endregion
            }
        }
        #endregion
        private void QueryLine(string lineName)
        {
            Hashtable mainDataHashTable = new Hashtable();
            DataSet dataSet = new DataSet();
            mainDataHashTable.Add(FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME, lineName);
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName = FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(mainDataTable);
            //Call Remoting Service
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIUdaCommonControlEx().SearchLineAttribute(dataSet);

                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    if (null == returnMessage || returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        gridDataView.GridControl.DataSource = retDS.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME];
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
        #region toolbar event
        private void tsbSearch_Click(object sender, EventArgs e)
        {
            QueryLine(cboObjectName.Text);       
        }

        private void gridDataView_DoubleClick(object sender, EventArgs e)
        {
            //int rowHandle = gridDataView.GetDataRowHandleByGroupRowHandle(gridDataView.FocusedRowHandle);
            int rowHandle = gridDataView.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                string objectname = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_NAME).ToString();
                string key = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_PRODUCTION_LINE_KEY).ToString();
                if (null == objectname || objectname.Length < 1)
                    return;
                if (null == key || key.Length < 1)
                    return;
                UdaEntity tempEntity = new UdaEntity(objectname);
                tempEntity.ObjectKey = key;
                tempEntity.LineCode = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_LINE_CODE).ToString();
                tempEntity.Description = gridDataView.GetRowCellValue(rowHandle, FMM_PRODUCTION_LINE_FIELDS.FIELD_DESCRIPTIONS).ToString();
                //LineConfViewContent OrderContent = new LineConfViewContent(new UdaEntity(tempEntity.ObjectName));
                //WorkbenchSingleton.Workbench.ShowView(OrderContent);
                _objectEntity = tempEntity;
                //_objCategory = EntityType.Line.ToString();
                //_udaCommonControl = new UdaCommonControlEx(EntityType.Line, _objectEntity.ObjectKey, _objectEntity.LinkedToTable);
                MapAttributeToControl();
                State = ControlState.Edit;
                OnAfterStateChanged(State);
            }
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.WorkOrderManagement.Name}"))
                if (viewContent.TitleName == "Default Title")
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new LineConfViewContent(new UdaEntity()));
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            QueryLine("");
            if (gridDataView.DataRowCount >= 4)
            {
                return;
            }

            //系统提示提示确定要保存吗？ 如果确定继续下面的操作 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                //判断线别名称是否为空 
                if (cboObjectName.Text == string.Empty)
                {
                    //提示请填写完整信息
                    MessageService.ShowWarning("${res:FanHai.Hemera.Addins.WIP.LotEDCDialog.CheckNULL}");
                    return;
                }

                MapControlToAttribute();
                //判断状态为新增
                if (ControlState.New == State)
                {
                    if (_objectEntity.Insert())
                    {
                        //系统提示保存成功 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                         = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.LineConfCtrl.title}") + "_" //线别维护
                         + _objectEntity.ObjectName;
                        //修改该条记录的状态 
                        State = ControlState.Edit;
                    }
                }
                //判断状态为修改操作 
                else if (ControlState.Edit == State)
                {
                    if (isMainUpdate)
                    {
                        UpdateMainTable();
                    }

                    if (_objectEntity.Update())
                    {
                        //系统提示更新成功 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                        State = ControlState.Edit;
                    }
                }
                tsbSearch_Click(sender, e);
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //提示确定要删除吗？ 
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}", "${res:Global.SystemInfo}"))
            {
                //判断状态是不是修改状态
                if (ControlState.Edit == State)
                {
                    if (_objectEntity.Delete())
                    {
                        //删除成功！
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}");
                        WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);

                        foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                        {
                            if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.LineConfCtrl.title}"))//线别维护
                            {
                                viewContent.WorkbenchWindow.SelectWindow();
                                return;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region private function
        private void MapAttributeToControl()
        {
            cboObjectName.Text = _objectEntity.ObjectName;
            lblObjectKey.Text = _objectEntity.ObjectKey;
            this.txtDescription.Text = _objectEntity.Description;
            this.txtLineCode.Text = _objectEntity.LineCode;

            _udaCommonControl.UserDefinedAttrs = _objectEntity.UserDefinedAttrsEx;
        }

        private void MapControlToAttribute()
        {
            _objectEntity.ObjectName = cboObjectName.Text;
            _objectEntity.CategoryKey = this.lblObjectKey.Text.ToString();
            _objectEntity.LineCode = this.txtLineCode.Text.ToString();
            _objectEntity.Description = this.txtDescription.Text.ToString();
            _objectEntity.UserDefinedAttrsEx = _udaCommonControl.UserDefinedAttrs;

            var test = ((DevExpress.XtraGrid.GridControl)(this._udaCommonControl.Controls.Find("gridUDAs", true)[0])).FocusedView as ColumnView;

            ColumnView view =
                ((DevExpress.XtraGrid.GridControl)(this._udaCommonControl.Controls.Find("gridUDAs", true)[0])).FocusedView as ColumnView;
            view.CloseEditor();
            view.UpdateCurrentRow();
            //end
        }

        private void InitUI()
        {
            //toolbar
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbStatus.Text = StringParser.Parse("${res:Global.Status}");

            //tabPage
            //this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.LineManagement.Name}"); 
            this.BaseTabPage.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.BasePage}");
            this.UdaTabPage.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.UdaPage}");

            lblCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LineConfCtrl.lbl.0001}");//编号
            lblObjectName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LineConfCtrl.lbl.0002}");//线别名称
            lblDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LineConfCtrl.lbl.0003}");//描述

            this.cboObjectType.ReadOnly = true;

        }
        #endregion


        private void LineConfCtrl_Load(object sender, EventArgs e)
        {
            InitUI();
            //BindObjectTypeList();
            //this.cboObjectName.Text = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            //this.lblKey.Text = System.Environment.MachineName.ToString();
        }

        private void EditValueChanged(object sender, EventArgs e)
        {
            isMainUpdate = true;
        }



        private void UpdateMainTable()
        {
            string lineName = this.cboObjectName.Text.ToString();
            string lineCode = this.txtLineCode.Text.ToString();
            string lineDesc = this.txtDescription.Text.ToString();
            string lineKey = this.lblObjectKey.Text.ToString();
            string edtor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            string editTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            DataSet dsObjectType = new DataSet();
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            int exeResult = serverFactory.CreateIUdaCommonControlEx().UpdateLineInfo(lineKey, lineName, lineCode, lineDesc, edtor, editTimeZone);

        }

        private void tsbStatus_Click(object sender, EventArgs e)
        {

        }

    }
}


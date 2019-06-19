using DevExpress.XtraGrid.Views.Base;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.UDA;
using System;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示产品料号维护自定义控件类。
    /// </summary>
    public partial class PartEditor : BaseUserCtrl
    {
        private Part _part = null;
        private UdaCommonControl _udaCommonControl = null;
        private ControlState _controlState = ControlState.Empty;
        private new delegate void AfterStateChanged(ControlState controlState);
        private new AfterStateChanged afterStateChanged = null;

        public PartEditor(Part part)
        {
            InitializeComponent();
            InitUIByResourceFile();
            _part = part;

            BindDataForPartType();

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            if (_part.PartName.Length < 1 )
            {
                _udaCommonControl = new UdaCommonControl(EntityType.Part,"");
                _udaCommonControl.UserDefinedAttrs = _part.UserDefinedAttrs;
                State = ControlState.New;
            }
            else
            {
                _udaCommonControl = new UdaCommonControl(EntityType.Part,_part.PartKey);
                MapPartToControls();
                if (_part.Status == EntityStatus.InActive)
                    State = ControlState.Edit;
                else
                    State = ControlState.ReadOnly;
               
            }
            _udaCommonControl.Dock = DockStyle.Fill;
            panelUDACtrl.Controls.Add(_udaCommonControl);
        }

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
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtPartNumber.Properties.ReadOnly = true;
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly / Read
                case ControlState.ReadOnly:
                case ControlState.Read:
                    txtPartNumber.Properties.ReadOnly = true;
                    txtDescriptions.Properties.ReadOnly = true;
                    //toolbarEdit.Enabled = true;
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = true;
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    txtPartNumber.Properties.ReadOnly = false;
                    txtDescriptions.Properties.ReadOnly = false;
                   // toolbarEdit.Enabled = false;
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;
                    _udaCommonControl.LinkedToItemKey = _part.PartKey;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtPartNumber.Properties.ReadOnly = false;
                    txtDescriptions.Properties.ReadOnly = false;
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    _udaCommonControl.LinkedToItemKey =_part.PartKey;

                    //cbeMoudle.Text = "-填完描述后回车,自动带出描述中出现第一个\" - \"符号前面部分的信息作为产品型号,请慎重填写描述-";
                    cbeMoudle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.lbl.0001}"); 
                    BindDataForPartType();   
                    break;
                #endregion
            }
        }
        #endregion

        //定义控件名称
        private void InitUIByResourceFile()
        {
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}");             //成品管理
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");                                            //删除
            //this.toolbarEdit.Text = StringParser.Parse("${res:Global.Edit}");                                              
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");                                                  //新增
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");                                                //保存
            this.toolbarSearch.Text = StringParser.Parse("${res:Global.Query}");                                             //查询
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");                                            //状态
            lblDes.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.Description}");                 //描述
            xtraTabPage1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.BasePage}");         //基本属性
            xtraTabPage2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotTemplateCtrl.UdaPage}");          //自定义属性

            lblMatNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PartEditor.lbl.0002}");//产品料号
            lcPartType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PartEditor.lbl.0003}");//产品类型
            lcPartClass.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PartEditor.lbl.0004}");//产品分类
            lcPartModule.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.PartEditor.lbl.0005}");//产品型号
        }


       
        private void BindDataForPartType()
        {
            DataSet dsPartType = _part.GetPartType();
            luePartYtpe.Properties.DataSource = dsPartType.Tables[0];
            this.luePartYtpe.Properties.DisplayMember = "PART_TYPE";
            this.luePartYtpe.Properties.ValueMember = "PART_TYPE";
            this.luePartYtpe.ItemIndex = 0;
        }

        #region Toolbar Actions
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}", "${res:Global.SystemInfo}"))
            {//系统提示确定要删除吗？
                if (ControlState.Edit == State)
                {//状态为EDIT Empty=0，Readonly=1，Read，Edit，New，Delete
                    if (_part.Delete())
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}");          //删除成功
                        WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                        //foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection) 
                        //{//遍历当前视图
                        //    if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}")) 
                        //    {//视图名称为成品管理
                                
                        //        viewContent.WorkbenchWindow.SelectWindow();
                        //        return;
                        //    }
                        //}
                        //WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        //  StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}"); //成品管理
                        //State = ControlState.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {//系统提示确定要保存吗

                //判断控件是否有空值 
                if (string.IsNullOrEmpty(this.txtPartNumber.Text))
                {
                    //MessageService.ShowMessage("请填写产品料号。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.txtPartNumber.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.txtDescriptions.Text))
                {
                    //MessageService.ShowMessage("请填写产品描述。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.txtDescriptions.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.cbeMoudle.Text))
                {
                    //MessageService.ShowMessage("请填写产品型号。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.cbeMoudle.Focus();
                    this.cbeMoudle.SelectAll();
                    return;
                }
                if (this.cbeMoudle.Text == StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.lbl.0001}"))//"-填完描述后回车,自动带出描述中出现第一个\" - \"符号前面部分的信息作为产品型号,请慎重填写描述-")
                {
                    //MessageService.ShowMessage("请合理填写描述,然后回车带出型号。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.txtDescriptions.Focus();
                    this.txtDescriptions.SelectAll();
                    return;
                }
                if (string.IsNullOrEmpty(this.luePartYtpe.Text))
                {
                    //MessageService.ShowMessage("请选择产品类型。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.luePartYtpe.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtPartClass.Text))
                {
                    //MessageService.ShowMessage("请选择产品分类。", "系统错误提示");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.PartEditor.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    this.txtPartClass.Focus();
                    return;
                }
                //判断控件是否有空值 
               
                MapControlsToPart();                      //变量赋值

                if (ControlState.New == State)
                {//判断状态为new
                    _part.PartVersion = "1";
                    if (_part.Insert())
                    {//插入操作成功
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                          = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}") + "_"
                          +_part.PartName;
                      
                        State = ControlState.Edit;
                    }
                }
                else if (ControlState.Edit == State)
                {
                    if (_part.Update())
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                        State = ControlState.Edit;
                    }
                }

                toolbarSearch_Click(sender, e);
            }
        }

        private void toolbarNew_Click(object sender, EventArgs e)
        {
            this._part = new Part();
            MapPartToControls();
            State = ControlState.New;
        }

        private void toolbarSearch_Click(object sender, EventArgs e)
        {
            string partName = this.txtPartNumber.Text.Trim();
            //绑定数据表数据参数值_partName为物料号 
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (serverFactory != null)
                {
                    DataSet ds = new DataSet();
                    //查询成品数据通过物料号
                    ds = serverFactory.CreateIPartEngine().SearchPart(partName);
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(ds);
                    if (msg != "")
                    {//查询出错！
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}");
                    }
                    else
                    {
                        if (ds.Tables.Count > 0)
                        {
                            gridControl1.MainView = gridView1;
                            gridControl1.DataSource = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            //PartSearch pSearch = new PartSearch();
            //if (DialogResult.OK == pSearch.ShowDialog())
            //{
            //    if (null == pSearch.PartKey || pSearch.PartKey.Length < 1)
            //        return;
            //    if (null == pSearch.PartName || pSearch.PartName.Length < 1)
            //        return;

            //    string title = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartManagement.Name}") + "_" + pSearch.PartName;


            //    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //    {
            //        if (viewContent.TitleName == title)
            //        {
            //            viewContent.WorkbenchWindow.SelectWindow();
            //            return;
            //        }
            //    }
            //    this._part = new Part(pSearch.PartKey);
            //    MapPartToControls();
            //    State = ControlState.Edit;
            //}
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                string partkey  = this.gridView1.GetRowCellValue(rowHandle, POR_PART_FIELDS.FIELD_PART_KEY).ToString();
                Part tempPart = new Part(partkey);
                tempPart.PartID= this.gridView1.GetRowCellValue(rowHandle, POR_PART_FIELDS.FIELD_PART_ID).ToString();
                tempPart.PartName = this.gridView1.GetRowCellValue(rowHandle, POR_PART_FIELDS.FIELD_PART_NAME).ToString();
                //tempPar = this.gridView1.GetRowCellValue(rowHandle, "PART_TYPE").ToString();
                //_partModule = this.gridView1.GetRowCellValue(rowHandle, "PART_MODULE").ToString();
                //_partClass = this.gridView1.GetRowCellValue(rowHandle, "PART_CLASS").ToString();
                _part = tempPart;
                //_udaCommonControl = new UdaCommonControl(EntityType.Part, partkey);
                MapPartToControls();
                State = ControlState.Edit;
                OnAfterStateChanged(State);
            }
        }
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _part.Status;
            StatusDialog status = new StatusDialog(_part);
            status.ShowDialog();
            if (oldStatus != _part.Status)
            {
                if (_part.Status == EntityStatus.Active)
                {
                    State = ControlState.Edit;
                }
                if (_part.Status == EntityStatus.Archive)
                {
                    State = ControlState.ReadOnly;
                }                
            }
        }

        private void MapPartToControls()
        {
            txtPartNumber.Text = _part.PartName;
            //cmbSOP.Text = _part.InstructionsName;
            txtDescriptions.Text = _part.Descriptions;
            txtPartClass.Text = _part.PartClass;
            luePartYtpe.EditValue = _part.Type;
            cbeMoudle.Text = _part.Module;
            _udaCommonControl.UserDefinedAttrs = _part.UserDefinedAttrs;
        }
        private void MapControlsToPart()
        {
            _part.PartName = txtPartNumber.Text.Trim();
            _part.PartID = txtPartNumber.Text.Trim();
            _part.Descriptions = txtDescriptions.Text.Trim(); 
            _part.Module = cbeMoudle.Text.ToString().Trim();
            _part.Type = luePartYtpe.Text.ToString().Trim();
            _part.PartClass = txtPartClass.Text.ToString().Trim();
            //_part.Editor=
            _part.UserDefinedAttrs = _udaCommonControl.UserDefinedAttrs;
            _part.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            //set uda control's status
            ColumnView view =
                ((DevExpress.XtraGrid.GridControl)(this._udaCommonControl.Controls.Find("gridUDAs", true)[0])).FocusedView as ColumnView;
            view.CloseEditor();
            view.UpdateCurrentRow();
            //end
        }
        #endregion      

        private void PartEditor_Load(object sender, EventArgs e)
        {

        }

        private void txtDescriptions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string[] arrTemp = txtDescriptions.Text.Split('-');
                cbeMoudle.Text = arrTemp[0];
            }
        }

    }
}

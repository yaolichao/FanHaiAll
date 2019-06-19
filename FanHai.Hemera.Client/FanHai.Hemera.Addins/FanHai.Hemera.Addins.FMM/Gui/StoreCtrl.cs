using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.StaticFuncs;

using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class StoreCtrl : BaseUserCtrl
    {
        public new delegate void AfterStateChanged(ControlState controlState);

        public new AfterStateChanged afterStateChanged = null;
        
        private ControlState cState;    //窗体状态
        private Store _store = null;    //线边仓对象        

        public StoreCtrl()
        {
            InitializeComponent();
            //根据窗体状态，设定界面上的按钮是否可用
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            //默认窗体状态为新增状态
            CtrlState = ControlState.New;
            GridViewHelper.SetGridView(storeView);
        }

        public ControlState CtrlState
        {
            get
            {
                return cState;
            }
            set
            {
                cState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }
        /// <summary>
        /// 根据窗体状态，设定界面上的按钮是否可用
        /// </summary>
        /// <param name="state"></param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {  
                #region case state of edit
                case ControlState.Edit:
                    this.teStoreName.Enabled = true;
                    this.meDescription.Enabled = true;
                    this.lueType.Enabled = true;
                    this.lueLocation.Enabled = true;
                    this.ceEnable.Enabled = true;
                    this.ceRequest.Enabled = true;
                    this.cmbOperation.Enabled = true;
                    //根据用户是否有权限来设置保存与删除按钮是否可用
                    this.BtnSave.Enabled = true;
                    this.BtnDelete.Enabled = true;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    this.teStoreName.Enabled = false;
                    this.meDescription.Enabled = false;
                    this.lueType.Enabled = false;
                    this.lueLocation.Enabled = false;
                    this.ceEnable.Enabled = false;
                    this.ceRequest.Enabled = false;
                    this.cmbOperation.Enabled = false;
                   
                    this.BtnSave.Enabled = false;
                    this.BtnDelete.Enabled = false;
                    break;
                #endregion

                #region case state of new
                case ControlState.New:                    
                    this.teStoreName.Text = "";
                    this.meDescription.Text = "";
                    this.lueLocation.EditValue = null;
                    this.lueType.EditValue = null;
                    this.ceEnable.Checked = false;
                    this.ceRequest.Checked = false;
                    this.cmbOperation.Text = "";

                    this.teStoreName.Enabled = true;
                    this.meDescription.Enabled = true;
                    this.lueType.Enabled = true;
                    this.lueLocation.Enabled = true;
                    this.ceEnable.Enabled = true;
                    this.ceRequest.Enabled = true;
                    this.cmbOperation.Enabled = true;
                    

                    this.BtnSave.Enabled = true;
                    this.BtnDelete.Enabled = false;
                    break;
                #endregion
            }
            BtnAdd.Enabled = true;
        }
        /// <summary>
        /// 初期界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreCtrl_Load(object sender, EventArgs e)
        {
            //从资源文件中获取字符串，为界面赋值
            InitUIByResourceFile();

            checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
            IceRequest.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
            //调用远程对象，查出区域信息，赋值给界面上的下拉框
            BindDataToLoopUpEdit();
            //获取线边仓信息，并绑定到grid控件上
            BindDataToGridView();
        }
        /// <summary>
        /// 从资源文件中获取字符串，为界面赋值
        /// </summary>
        private void InitUIByResourceFile()
        {
            this.lblDescription.Text = StringParser.Parse("${res:Global.Remark}");
            //this.lblLocation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblLocation}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblName}");
            this.lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblType}");
            this.ceEnable.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.ceEnable}");
            this.store_name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblName}");
            //this.location_name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblLocation}");
            this.description.Caption = StringParser.Parse("${res:Global.Remark}");           
            this.object_status.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatchListCtl.State}");           
            this.type.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblType}");
            
            this.lueType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.lblType}"))});
            this.lueLocation.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LOCATION_NAME",string.Empty)});

            lblLocation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0002}");//车间
            lblOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0003}");//工序
            ceRequest.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0004}");//申请过账
            location_name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0002}");//车间
            OperationName.Caption= StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0003}");//工序
            RequestFlag.Caption= StringParser.Parse("${res:FanHai.Hemera.Addins.StoreCtrl.lbl.0004}");//申请过账

        }
        /// <summary>
        /// 将调用远程对象查出的结果赋值给下拉框
        /// </summary>
        private void BindDataToLoopUpEdit()
        {
            DataSet dsLocation = new DataSet();
            LocationEntity location = new LocationEntity();  
            //根据区域名称和区域类型查询区域信息
            location.LocalLevel = "5";//只查询车间数据
            dsLocation = location.SearchLocation();
            if (location.ErrorMsg != "")
            {
                //提示“获取区域出错！”
                MessageService.ShowError("${res:FanHai.Hemera.Addins.FMM.StoreCtrl.Msg.GetLocationError}");
            }
            else
            {
                //若结果集中包含“FMM_LOCATION”表
                if (dsLocation.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                {
                    //为界面上的“区域”下拉框赋值
                    this.lueLocation.Properties.DataSource = dsLocation.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
                    this.lueLocation.Properties.DisplayMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME;
                    this.lueLocation.Properties.ValueMember = FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY;
                }
            }

            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Store_Type");
            //得到线边仓类型的基础数据
            DataTable dataTable = BaseData.Get(columns, category);
            //为界面上的“类型”下拉框赋值
            this.lueType.Properties.DataSource = dataTable;
            this.lueType.Properties.DisplayMember = "NAME";
            this.lueType.Properties.ValueMember = "CODE";
            //？？？？？
            this.lueStoreType.DataSource = dataTable;
            this.lueStoreType.DisplayMember = "NAME";
            this.lueStoreType.ValueMember = "CODE";

            try
            {                
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                //调用远程对象的方法，得到最新版本的工序
                DataSet dsReturn = factor.CreateIOperationEngine().GetMaxVerOperation(null);
                string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (msg == string.Empty)
                {
                    if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                    {
                        DataTable operationTable = dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                        foreach (DataRow dataRow in operationTable.Rows)
                        {
                            //设置界面上的“工序”内容为POR_ROUTE_OPERATION_VER表的工序名称
                            cmbOperation.Properties.Items.Add(dataRow[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString());                            
                        }                       
                    }
                }
                else
                {
                    throw new Exception(msg);
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
        }
        /// <summary>
        /// 新增按钮click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //设置窗体状态为新增
            CtrlState = ControlState.New;
            _store = new Store();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            //弹出"确定要保存吗？"对话框，点击“是”时，继续
            if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                #region MapControlToEntity  //将控件上的值赋给对象
                //如果名称为空
                if (string.IsNullOrEmpty(this.teStoreName.Text))
                {
                    //提示“名称不能为空”错误信息
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.FMM.Msg.NameIsNull}", "${res:Global.SystemInfo}");
                    this.teStoreName.Focus();
                    return;
                }
                //如果工序为空
                if (string.IsNullOrEmpty(this.cmbOperation.Text))
                {
                    MessageService.ShowMessage("请选择工序。");
                    this.cmbOperation.Focus();
                    return;
                }

                //如果工序为空
                if (lueType.EditValue==null || string.IsNullOrEmpty(lueType.EditValue.ToString()))
                {
                    MessageService.ShowMessage("请选择类型。");
                    this.lueType.Focus();
                    return;
                }
                _store.StoreName = this.teStoreName.Text;
                //若类型不为空
                if (null !=lueType.EditValue && lueType.EditValue.ToString()!="")
                {
                    _store.StoreType = this.lueType.EditValue.ToString();
                    _store.TypeName = this.lueType.Text;
                }
                //若区域不为空
                if (null !=lueLocation.EditValue && lueLocation.EditValue.ToString()!="")
                {
                    _store.LocationKey = this.lueLocation.EditValue.ToString();
                }                
                _store.Description = this.meDescription.Text;
                _store.OperationName = this.cmbOperation.Text;
                //若“申请过账”复选框选中
                if (ceRequest.Checked == true)
                {
                    _store.RequestFlag = "1";
                }
                else
                {
                    _store.RequestFlag = "0";
                }
                //若“可用”复选框选中
                if (ceEnable.Checked == true)
                {
                    _store.ObjectStatus = "1";
                }
                else
                {
                    _store.ObjectStatus = "0";
                }
                #endregion

                //若窗体状态为新增
                if (CtrlState == ControlState.New)
                {
                    //获取新的GUID作为主键
                    _store.StoreKey =  CommonUtils.GenerateNewKey(0);
                    _store.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _store.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    //调用远程对象的插入方法，将新增的线边仓数据插入数据库
                    if (_store.Insert())
                    {
                        //提示“保存成功”
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        //设置窗体状态为只读
                        CtrlState = ControlState.ReadOnly;
                        //add new row   //将数据显示在gridview控件上
                        storeView.AddNewRow();
                        EditRecord();
                        storeView.UpdateCurrentRow();
                        storeView.ShowEditor();
                    }
                }
                //若窗体状态为编辑
                else if (CtrlState == ControlState.Edit)
                {
                    //默认的编辑时间为9999-12-31？？？？？？
                    _store.EditTime = "9999-12-31 23:59:59";
                    _store.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _store.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);                   
                    if (_store.Update())
                    {
                        //提示“更新成功！”
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                        //获取线边仓数据，并绑定到界面的grid控件上
                        BindDataToGridView();
                        //？？？？？
                        _store.ResetDirtyList();
                        //设置窗体状态为只读
                        CtrlState = ControlState.ReadOnly;
                    }
                }
            }
        }

        /// <summary>
        /// Add New Record
        /// </summary>
        private bool EditRecord()
        {
            //根据行句柄，获取选中行
            DataRow row = storeView.GetDataRow(storeView.FocusedRowHandle);          
            row[WST_STORE_FIELDS.FIELD_STORE_KEY] = _store.StoreKey;
            row[WST_STORE_FIELDS.FIELD_STORE_NAME] =_store.StoreName;
            row[WST_STORE_FIELDS.FIELD_STORE_TYPE] = _store.StoreType;           
            row[WST_STORE_FIELDS.FIELD_OBJECT_STATUS] = _store.ObjectStatus;           
            row[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME] = this.lueLocation.Text;
            row[FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY] =_store.LocationKey;
            row[WST_STORE_FIELDS.FIELD_DESCRIPTION] =this.meDescription.Text;
            row[WST_STORE_FIELDS.FIELD_EDITOR] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            row[WST_STORE_FIELDS.FIELD_EDIT_TIMEZONE] = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            row[WST_STORE_FIELDS.FIELD_OPERATION_NAME] = _store.OperationName;
            row[WST_STORE_FIELDS.FIELD_REQUEST_FLAG] = _store.RequestFlag;
            //结束数据的行编辑事件
            row.EndEdit();
            return true;
        }
        /// <summary>
        /// 获取线边仓信息，并绑定到grid控件上
        /// </summary>
        private void BindDataToGridView()
        {
            DataSet dsStores = new DataSet();
            _store = new Store ();
            //获取线边仓信息
            dsStores = _store.GetStore();
            //若查询成功
            if (_store.ErrorMsg == "")
            {
                //若结果集中包含“WST_STORE”数据表
                if (dsStores.Tables.Contains(WST_STORE_FIELDS.DATABASE_TABLE_NAME))
                {
                    //将“WST_STORE”数据表中的内容赋给grid控件
                    gcStoreControl.DataSource = dsStores.Tables[WST_STORE_FIELDS.DATABASE_TABLE_NAME];
                    //调整gridView的列宽，让列更适合他们的内容
                    storeView.BestFitColumns();
                }
            }
            else
            {
                //提出“查询出错！”
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}");
            }           
        }
        /// <summary>
        /// 删除按钮click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //弹出“确定要删除吗？”对话框，选“是”时，继续
            if (DialogResult.Yes == MessageBox.Show(this, StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                //调用远程对象的删除方法
                if (_store.Delete())
                {
                    //提示“删除成功！”信息
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}", "${res:Global.SystemInfo}");
                    //更新界面，
                    storeView.DeleteSelectedRows();
                    //设置窗体状态为新增
                    CtrlState = ControlState.New;
                }
            }
        }
        /// <summary>
        /// 线变仓明细表格的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storeControl_DoubleClick(object sender, EventArgs e)
        {  
            //若选中表格的某行双击
            if (this.storeView.FocusedRowHandle > -1)
            {
                //将该行数据赋给线边仓对象的对应属性
                CtrlState = ControlState.Edit;
                _store = new Store();
                _store.StoreName = storeView.GetRowCellValue(storeView.FocusedRowHandle,store_name).ToString();
                _store.StoreKey = storeView.GetRowCellValue(storeView.FocusedRowHandle,store_key).ToString();
                _store.StoreType = storeView.GetRowCellValue(storeView.FocusedRowHandle,store_type).ToString();
                _store.LocationKey = storeView.GetRowCellValue(storeView.FocusedRowHandle,location_key).ToString();
                _store.ObjectStatus = storeView.GetRowCellValue(storeView.FocusedRowHandle,object_status).ToString();
                _store.Description = storeView.GetRowCellValue(storeView.FocusedRowHandle,description).ToString();
                _store.EditTime = storeView.GetRowCellValue(storeView.FocusedRowHandle, edit_time).ToString();
                _store.OperationName = storeView.GetRowCellValue(storeView.FocusedRowHandle, WST_STORE_FIELDS.FIELD_OPERATION_NAME).ToString();
                _store.RequestFlag = storeView.GetRowCellValue(storeView.FocusedRowHandle, WST_STORE_FIELDS.FIELD_REQUEST_FLAG).ToString();
                _store.IsInitializeFinished = true;
                //再把线边仓对象的属性赋给界面的上半部分
                this.teStoreName.Text = _store.StoreName;
                this.lueLocation.EditValue = _store.LocationKey;
                this.lueType.EditValue = _store.StoreType;
                this.meDescription.Text = _store.Description;
                this.cmbOperation.Text = _store.OperationName;
                //若线变仓的状态为1
                if (_store.ObjectStatus == "1")
                {
                    //“可用”复选框选中
                    this.ceEnable.Checked = true;
                }
                else
                {
                    //否则“可用”复选框不选中
                    this.ceEnable.Checked = false;
                }
                //若是否需要申请过账状态为1
                if (_store.RequestFlag == "1")
                {
                    //“申请过账”复选框选中
                    this.ceRequest.Checked = true;
                }
                else
                {
                    //否则“申请过账”复选框不选中
                    this.ceRequest.Checked = false;
                }
            }
        }
        /// <summary>
        /// 刷新按钮click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            //获取线边仓信息，并帮定到界面的表格上
            BindDataToGridView();
        }

        private void storeControl_Click(object sender, EventArgs e)
        {

        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}

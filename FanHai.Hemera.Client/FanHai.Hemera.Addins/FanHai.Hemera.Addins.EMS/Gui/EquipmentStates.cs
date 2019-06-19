using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Controls;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment States GUI
    /// </summary>
    public partial class EquipmentStates : BaseUserCtrl
    {
        #region EquipmentStateEntity Object

        private FanHai.Hemera.Utils.Entities.EquipmentManagement.EquipmentStateEntity equipmentStateEntity = null;

        #endregion

        #region Constructor

        public EquipmentStates()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void EquipmentStates_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(EquipmentStates_afterStateChanged);
            //设备状态类型的数据绑定 
            LoadEquipmentStateTypeData();
            //绑定数据表的数据为查询的设备状态数据表的数据 
            LoadEquipmentStatesData();
            //state=Read
            State = ControlState.Read;
        }

        /// <summary>
        /// state状态的改变Read，ReadOnly，New，Edit
        /// </summary>
        private void EquipmentStates_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtStateName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbStateType.Enabled = false;
                    this.txtStateCategory.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtStateName.Enabled = false;
                    this.txtDescription.Enabled = false;
                    this.cmbStateType.Enabled = false;
                    this.txtStateCategory.Enabled = false;
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtStateName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.cmbStateType.Enabled = true;
                    this.txtStateCategory.Enabled = true;

                    this.txtStateName.Text = equipmentStateEntity.EquipmentStateName;
                    this.txtDescription.Text = equipmentStateEntity.Description;
                    this.cmbStateType.EditValue = equipmentStateEntity.EquipmentStateType;
                    this.txtStateCategory.Text = equipmentStateEntity.EquipmentStateCategory;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtStateName.Enabled = true;
                    this.txtDescription.Enabled = true;
                    this.cmbStateType.Enabled = true;
                    this.txtStateCategory.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// 初始化页面控件
        /// </summary>
        protected override void InitUIControls()
        {
            this.cmbStateType.Properties.Columns.Clear();

            LookUpColumnInfo columnInfo = new LookUpColumnInfo(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE);
            this.cmbStateType.Properties.Columns.Add(columnInfo);
            this.cmbStateType.Properties.DisplayMember = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE;
            this.cmbStateType.Properties.ValueMember = EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE;

            this.grdViewStates.Columns.Clear();

            EMS_EQUIPMENT_STATES_FIELDS equipmentStatesFields = new EMS_EQUIPMENT_STATES_FIELDS();

            int index = 0;

            foreach (KeyValuePair<string, FieldProperties> field in equipmentStatesFields.FIELDS)
            {
                GridColumn column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewStates.Columns.Add(column);
            }
        }

        /// <summary>
        /// 初始化页面资源信息
        /// </summary>
        protected override void InitUIResourcesByCulture()
        {
            GridViewHelper.SetGridView(grdViewStates);
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            //注释 by Peter.Zhang 图标资源使用项目内的图标。
            //tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");

            //this.grpState.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.Name}");
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.Name}");
            this.lblStateName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateName}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblStateType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateType}");
            this.lblStateCategory.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateCategory}");

            this.cmbStateType.Properties.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateType}");

            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateName}");
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateType}");
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.StateCategory}");

            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATOR].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grdViewStates.Columns[EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME].Visible = false;
        }

        /// <summary>
        /// 初始化页面操作权限
        /// </summary>
        protected override void InitUIAuthoritiesByUser()
        {

        }

        /// <summary>
        /// 行焦点数据获取赋值给equipmentStateEntity对象的变量
        /// </summary>
        protected override void MapControlsToEntity()
        {
            if (equipmentStateEntity != null)
            {
                equipmentStateEntity.ClearData();

                switch (State)
                {
                    case ControlState.New:
                        equipmentStateEntity.EquipmentStateKey =  CommonUtils.GenerateNewKey(0);

                        equipmentStateEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        equipmentStateEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                        equipmentStateEntity.CreateTime = string.Empty;
                        break;
                    case ControlState.Edit:
                    case ControlState.Delete:
                        equipmentStateEntity.EquipmentStateKey = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY).ToString();
                        equipmentStateEntity.EquipmentStateName = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME).ToString();
                        equipmentStateEntity.Description = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION).ToString();
                        equipmentStateEntity.EquipmentStateType = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE).ToString();
                        equipmentStateEntity.EquipmentStateCategory = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY).ToString();
                        equipmentStateEntity.Editor = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDITOR).ToString();
                        equipmentStateEntity.EditTimeZone = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                        equipmentStateEntity.EditTime = this.grdViewStates.GetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EDIT_TIME).ToString();
                        break;
                }

                equipmentStateEntity.IsInitializeFinished = true;

                equipmentStateEntity.EquipmentStateName = this.txtStateName.Text.Trim();
                equipmentStateEntity.Description = this.txtDescription.Text.Trim();
                equipmentStateEntity.EquipmentStateType = this.cmbStateType.Text.Trim();
                equipmentStateEntity.EquipmentStateCategory = this.txtStateCategory.Text.Trim();
            }
        }
        /// <summary>
        ///判定state状态对数据操作
        /// </summary>
        protected override void MapEntityToControls()
        {
            if (equipmentStateEntity != null)
            {
                switch (State)
                {
                    case ControlState.New:
                        this.grdViewStates.AddNewRow();

                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY, equipmentStateEntity.EquipmentStateKey);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME, equipmentStateEntity.EquipmentStateName);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION, equipmentStateEntity.Description);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE, equipmentStateEntity.EquipmentStateType);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY, equipmentStateEntity.EquipmentStateCategory);

                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATOR, equipmentStateEntity.Creator);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIMEZONE_KEY, equipmentStateEntity.CreateTimeZone);
                        this.grdViewStates.SetFocusedRowCellValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_CREATE_TIME, equipmentStateEntity.CreateTime);

                        this.grdViewStates.UpdateCurrentRow();
                        break;
                    case ControlState.Edit:
                        foreach (KeyValuePair<string, DirtyItem> keyValue in equipmentStateEntity.DirtyList)
                        {
                            this.grdViewStates.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                        }
                        
                        this.grdViewStates.UpdateCurrentRow();
                        break;
                    case ControlState.Delete:
                        this.grdViewStates.DeleteRow(this.grdViewStates.FocusedRowHandle);

                        this.grdViewStates.UpdateCurrentRow();
                        break;
                }

                grdViewStates_FocusedRowChanged(this.grdViewStates, new FocusedRowChangedEventArgs(-1, this.grdViewStates.FocusedRowHandle));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Equipment State Type Data 绑定设备状态类型的值 
        /// </summary>
        private void LoadEquipmentStateTypeData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Build Input Parameters
            //GetTwoColumnsDataTable返回值为数据表包含2列  Key 和 Value
            DataTable dataTable = AddinCommonStaticFunction.GetTwoColumnsDataTable();   

            dataTable.TableName = TRANS_TABLES.TABLE_PARAM;   //表名PARAM

            dataTable.Rows.Add(new object[] { "CODE", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE });          //EQUIPMENT_STATE_TYPE状态类型
            dataTable.Rows.Add(new object[] { "CATEGORY", EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY });  //EQUIPMENT_STATE_CATEGORY设备状态分类
            //对datatable进行更改
            dataTable.AcceptChanges();
            //将数据表添加到数据集reqDS中
            reqDS.Tables.Add(dataTable);

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    //获取基础数据的设备状态类型 
                    resDS = serverFactory.CreateICrmAttributeEngine().GetSpecifyAttributeData(reqDS);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {
                //绑定设备状态类型下拉列表的数据 
                BindDataToStateTypeList(resDS.Tables[TRANS_TABLES.TABLE_UDAS]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Load Equipment States Data  绑定数据表的数据为查询的设备状态的数据 
        /// </summary>
        private void LoadEquipmentStatesData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();       //远程调用

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentStates().GetEquipmentStates(reqDS);  //获取设备状态数据
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);

                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #endregion

            #region Process Output Parameters

            string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);

            if (string.IsNullOrEmpty(returnMsg))
            {//returnMsg没有错误的结果返回值为true 
                BindDataToStatesGrid(resDS.Tables[EMS_EQUIPMENT_STATES_FIELDS.DATABASE_TABLE_NAME]);        //绑定数据表的数据为查询的设备状态的数据
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to States Grid
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToStatesGrid(DataTable dataTable)
        {
            this.grdStates.MainView = this.grdViewStates;
            this.grdStates.DataSource = null;
            this.grdStates.DataSource = dataTable;
        }

        /// <summary>
        /// Bind Data to State Type List 设备状态类型数据绑定 
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToStateTypeList(DataTable dataTable)
        {
            this.cmbStateType.Properties.DataSource = null;
            this.cmbStateType.Properties.DataSource = dataTable;
        }

        #endregion

        #region Controls Events

        private void cmbStateType_EditValueChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.cmbStateType.Properties.GetDataSourceRowIndex(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE, this.cmbStateType.EditValue);

            object selectedStateCategory = this.cmbStateType.Properties.GetDataSourceValue(EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_CATEGORY, selectedIndex);

            if (selectedStateCategory != null)
            {
                this.txtStateCategory.Text = selectedStateCategory.ToString();
            }
            else
            {
                this.txtStateCategory.Text = string.Empty;
            }
        }

        /// <summary>
        /// 行焦点发生改变触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewStates_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {//存在行焦点给界面输入控件赋值
                this.txtStateName.Text = this.grdViewStates.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_NAME).ToString();
                this.txtDescription.Text = this.grdViewStates.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_DESCRIPTION).ToString();
                this.cmbStateType.EditValue = this.grdViewStates.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_TYPE).ToString();
            }
            else
            {//无行焦点则输入控件值设为空
                this.txtStateName.Text = string.Empty;
                this.txtDescription.Text = string.Empty;
                this.cmbStateType.EditValue = string.Empty;
            }
        }

        private void grdStates_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grdViewStates.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                equipmentStateEntity = new FanHai.Hemera.Utils.Entities.EquipmentManagement.EquipmentStateEntity();

                State = ControlState.Edit;

                grdViewStates_FocusedRowChanged(this.grdViewStates, new FocusedRowChangedEventArgs(-1, this.grdViewStates.FocusedRowHandle));
            }
        }
        /// <summary>
        /// 刷新按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            //设备状态类型的数据绑定 
            LoadEquipmentStateTypeData();
            //绑定数据表的数据为查询的设备状态数据表的数据 
            LoadEquipmentStatesData();

            equipmentStateEntity = null;
            //状态为Read
            State = ControlState.Read;
            //行焦点的改变,这里是获取首行数据然后绑定数据到状态名称，状态类型，描述，状态分类的输入控件中 
            grdViewStates_FocusedRowChanged(this.grdViewStates, new FocusedRowChangedEventArgs(-1, this.grdViewStates.FocusedRowHandle));
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //初始化equipmentStateEntity对象中的值
            equipmentStateEntity = new FanHai.Hemera.Utils.Entities.EquipmentManagement.EquipmentStateEntity();
            //状态设置为new
            State = ControlState.New;
        }
        /// <summary>
        /// 保存按钮 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtStateName.Text))
            {
                //系统提示状态名称不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0007}", "${res:Global.InformationText}");
                //获取焦点
                this.txtStateName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.cmbStateType.Text))
            {
                //状态类型不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0008}", "${res:Global.InformationText}");
                this.cmbStateType.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtStateCategory.Text))
            {
                //状态分类不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0009}", "${res:Global.InformationText}");
                this.txtStateCategory.Focus();
                return;
            }

            if (equipmentStateEntity != null)
            {
                MapControlsToEntity();

                if (State == ControlState.New)
                {//判断状态
                    if (equipmentStateEntity.Insert())
                    {//新增成功返回值为true
                        //判定state状态对数据操作
                        MapEntityToControls();
                        //增加数据成功!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0001}", "${res:Global.InformationText}");
                        //修改状态为Read
                        State = ControlState.Read;
                    }
                }
                else if (State == ControlState.Edit)
                {//判断状态
                    if (equipmentStateEntity.IsDirty)
                    {
                        if (equipmentStateEntity.Update())
                        {//修改成功返回值为True
                            //判定state状态对数据操作
                            MapEntityToControls();
                            //更新数据成功!
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0003}", "${res:Global.InformationText}");
                            //状态修改Read
                            State = ControlState.Read;
                        }
                    }
                    else
                    {
                        //当前设备状态没有数据修改!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0011}", "${res:Global.InformationText}");
                    }
                }
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            equipmentStateEntity = null;
            //状态修改为Read
            State = ControlState.Read;
            //行焦点的改变,这里是获取当前行绑定数据到状态名称，状态类型，描述，状态分类的输入控件中
            grdViewStates_FocusedRowChanged(this.grdViewStates, new FocusedRowChangedEventArgs(-1, this.grdViewStates.FocusedRowHandle));
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.grdViewStates.FocusedRowHandle < 0)
            {//没有获取到行数据
                //系统提示请选择需要删除的数据!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0012}", "${res:Global.InformationText}");

                return;
            }

            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0010}", "${res:Global.QuestionText}"))
            {//系统提示确定删除吗?返回值为true
                equipmentStateEntity = new FanHai.Hemera.Utils.Entities.EquipmentManagement.EquipmentStateEntity();
                //状态修改为Delete
                State = ControlState.Delete;
                //行焦点数据获取赋值给equipmentStateEntity对象的变量
                MapControlsToEntity();

                if (equipmentStateEntity.Delete())
                {//删除成功返回值为true
                    //判定state状态对数据操作
                    MapEntityToControls();
                    //系统提示删除数据成功!
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentStates.M0005}", "${res:Global.InformationText}");
                    //状态修改为Read
                    State = ControlState.Read;
                }
            }
        }
        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion
    }
}

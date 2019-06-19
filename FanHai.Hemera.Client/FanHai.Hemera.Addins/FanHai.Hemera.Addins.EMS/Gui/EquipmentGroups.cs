using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities.EquipmentManagement;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EMS
{
    /// <summary>
    /// Equipment Groups GUI 
    /// </summary>
    public partial class EquipmentGroups : BaseUserCtrl
    {
        #region EquipmentGroupEntity Object

        private EquipmentGroupEntity equipmentGroupEntity = null;

        #endregion

        #region Constructor

        public EquipmentGroups()
        {
            InitializeComponent();
        }

        #endregion

        #region UI Events
        /// <summary>
        /// 页面Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentGroups_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(EquipmentGroups_afterStateChanged);
            //载入设备组信息到数据表 
            LoadEquipmentGroupsData();
            //状态state改为Read
            State = ControlState.Read;
        }

        private void EquipmentGroups_afterStateChanged()
        {
            switch (State)
            {
                case ControlState.Read:
                    this.tsbNew.Enabled = true;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = true;

                    this.txtGroupName.Enabled = false;
                    this.txtSPEC.Enabled = false;
                    this.txtDescription.Enabled = false;
                    break;
                case ControlState.ReadOnly:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = false;
                    this.tsbCancel.Enabled = false;
                    this.tsbDelete.Enabled = false;

                    this.txtGroupName.Enabled = false;
                    this.txtSPEC.Enabled = false;
                    this.txtDescription.Enabled = false;
                    break;
                case ControlState.New:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtGroupName.Enabled = true;
                    this.txtSPEC.Enabled = true;
                    this.txtDescription.Enabled = true;

                    this.txtGroupName.Text = equipmentGroupEntity.EquipmentGroupName;
                    this.txtSPEC.Text = equipmentGroupEntity.Spec;
                    this.txtDescription.Text = equipmentGroupEntity.Description;
                    break;
                case ControlState.Edit:
                    this.tsbNew.Enabled = false;
                    this.tsbSave.Enabled = true;
                    this.tsbCancel.Enabled = true;
                    this.tsbDelete.Enabled = false;

                    this.txtGroupName.Enabled = true;
                    this.txtSPEC.Enabled = true;
                    this.txtDescription.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load Equipment Groups Data
        /// </summary>
        private void LoadEquipmentGroupsData()
        {
            #region Variables

            DataSet reqDS = new DataSet();
            DataSet resDS = new DataSet();

            #endregion

            #region Call Remoting Interface

            try
            {
                //远程调用 
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentGroups().GetEquipmentGroups(reqDS);
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
                BindDataToStatesGrid(resDS.Tables[EMS_EQUIPMENT_GROUPS_FIELDS.DATABASE_TABLE_NAME]);
            }
            else
            {
                MessageService.ShowError(returnMsg);
            }

            #endregion
        }

        /// <summary>
        /// Bind Data to Groups Grid
        /// </summary>
        /// <param name="dataTable"></param>
        private void BindDataToStatesGrid(DataTable dataTable)
        {
            this.grdGroups.MainView = this.grdViewGroups;
            this.grdGroups.DataSource = null;
            this.grdGroups.DataSource = dataTable;
        }

        #endregion

        #region Override Methods

        protected override void InitUIControls()
        {
            this.grdViewGroups.Columns.Clear();

            GridViewHelper.SetGridView(grdViewGroups);

            EMS_EQUIPMENT_GROUPS_FIELDS equipmentGroupsFields = new EMS_EQUIPMENT_GROUPS_FIELDS();

            int index = 0;

            foreach (KeyValuePair<string, FieldProperties> field in equipmentGroupsFields.FIELDS)
            {
                GridColumn column = new GridColumn();

                column.Name = field.Key;
                column.FieldName = field.Key;
                column.Visible = true;
                column.VisibleIndex = index++;

                this.grdViewGroups.Columns.Add(column);
            }
        }

        protected override void InitUIResourcesByCulture()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");

            //tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            //tsbNew.Image = (Image)ResourceService.GetImageResource("Icons.32x32.EmptyFileIcon");
            //tsbSave.Image = (Image)ResourceService.GetImageResource("Icons.16x16.SaveIcon");
            //tsbCancel.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Cancel");
            //tsbDelete.Image = (Image)ResourceService.GetImageResource("Icons.16x16.DeleteIcon");
            //this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.Name}");
            //this.grpGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.Name}");

            this.lblGroupName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.GroupName}");
            this.lblSPEC.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.SPEC}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");

            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.GroupName}");
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC].Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.SPEC}");
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION].Caption = StringParser.Parse("${res:Global.Description}");

            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATOR].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIMEZONE_KEY].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIMEZONE_KEY].Visible = false;
            this.grdViewGroups.Columns[EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME].Visible = false;
        }

        protected override void InitUIAuthoritiesByUser()
        {

        }
        /// <summary>
        /// 重写基方法
        /// </summary>
        protected override void MapControlsToEntity()
        {
            if (equipmentGroupEntity != null)
            {
                //清理equipmentGroupEntity数据
                equipmentGroupEntity.ClearData();

                switch (State)
                {//判定状态
                    case ControlState.New:
                        equipmentGroupEntity.EquipmentGroupKey =  CommonUtils.GenerateNewKey(0);

                        equipmentGroupEntity.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                        equipmentGroupEntity.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                        equipmentGroupEntity.CreateTime = string.Empty;
                        break;
                    case ControlState.Edit:
                    case ControlState.Delete:
                        equipmentGroupEntity.EquipmentGroupKey = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY).ToString();
                        equipmentGroupEntity.EquipmentGroupName = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME).ToString();
                        equipmentGroupEntity.Spec = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC).ToString();
                        equipmentGroupEntity.Description = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION).ToString();
                        equipmentGroupEntity.Editor = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDITOR).ToString();
                        equipmentGroupEntity.EditTimeZone = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIMEZONE_KEY).ToString();
                        equipmentGroupEntity.EditTime = this.grdViewGroups.GetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EDIT_TIME).ToString();
                        break;
                }

                equipmentGroupEntity.IsInitializeFinished = true;

                equipmentGroupEntity.EquipmentGroupName = this.txtGroupName.Text.Trim();
                equipmentGroupEntity.Spec = this.txtSPEC.Text.Trim();
                equipmentGroupEntity.Description = this.txtDescription.Text.Trim();
            }
        }

        protected override void MapEntityToControls()
        {
            if (equipmentGroupEntity != null)
            {
                switch (State)
                {
                    case ControlState.New:
                        this.grdViewGroups.AddNewRow();

                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, equipmentGroupEntity.EquipmentGroupKey);
                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME, equipmentGroupEntity.EquipmentGroupName);
                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC, equipmentGroupEntity.Spec);
                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION, equipmentGroupEntity.Description);

                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATOR, equipmentGroupEntity.Creator);
                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIMEZONE_KEY, equipmentGroupEntity.CreateTimeZone);
                        this.grdViewGroups.SetFocusedRowCellValue(EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_CREATE_TIME, equipmentGroupEntity.CreateTime);

                        this.grdViewGroups.UpdateCurrentRow();
                        break;
                    case ControlState.Edit:
                        foreach (KeyValuePair<string, DirtyItem> keyValue in equipmentGroupEntity.DirtyList)
                        {
                            this.grdViewGroups.SetFocusedRowCellValue(keyValue.Key, keyValue.Value.FieldNewValue);
                        }

                        this.grdViewGroups.UpdateCurrentRow();
                        break;
                    case ControlState.Delete:
                        this.grdViewGroups.DeleteRow(this.grdViewGroups.FocusedRowHandle);

                        this.grdViewGroups.UpdateCurrentRow();
                        break;
                }

                grdViewGroups_FocusedRowChanged(this.grdViewGroups, new FocusedRowChangedEventArgs(-1, this.grdViewGroups.FocusedRowHandle));
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

        #region Controls Events
        /// <summary>
        /// 刷新按钮Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            //页面重新载入
            LoadEquipmentGroupsData();

            equipmentGroupEntity = null;
            //状态修改为Read
            State = ControlState.Read;
            //获取数据表dategridview行焦点，获取行数据
            grdViewGroups_FocusedRowChanged(this.grdViewGroups, new FocusedRowChangedEventArgs(-1, this.grdViewGroups.FocusedRowHandle));
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            //重新创建对象
            equipmentGroupEntity = new EquipmentGroupEntity();
            //状态修改为new
            State = ControlState.New;
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtGroupName.Text))
            {
                //组名称不允许为空!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0001}", "${res:Global.InformationText}");
                this.txtGroupName.Focus();                   //获取焦点到txtGroupName
                return;
            }

            if (equipmentGroupEntity != null)
            {
                //调用该方法给equipmentGroupEntity对象里面的值赋值
                MapControlsToEntity();

                if (State == ControlState.New)
                {//状态为new
                    if (equipmentGroupEntity.Insert())
                    {//新增成功返回值为true
                        MapEntityToControls();
                        //增加数据成功!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0002}", "${res:Global.InformationText}");
                        //状态修改Read
                        State = ControlState.Read;
                    }
                }
                else if (State == ControlState.Edit)
                {//状态为Edit
                    if (equipmentGroupEntity.IsDirty)
                    {
                        if (equipmentGroupEntity.Update())
                        {
                            MapEntityToControls();
                            //更新数据成功!
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0004}", "${res:Global.InformationText}");

                            State = ControlState.Read;
                        }
                    }
                    else
                    {
                        //当前设备组没有数据修改!
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0003}", "${res:Global.InformationText}");
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
            equipmentGroupEntity = null;
            //状态为Read
            State = ControlState.Read;
            //获取数据表dategridview行焦点，获取行数据
            grdViewGroups_FocusedRowChanged(this.grdViewGroups, new FocusedRowChangedEventArgs(-1, this.grdViewGroups.FocusedRowHandle));
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (this.grdViewGroups.FocusedRowHandle < 0)
            {//不存在行焦点
                //请选择需要删除的数据!
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0007}", "${res:Global.InformationText}");
                return;
            }

            if (MessageService.AskQuestion("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0005}", "${res:Global.QuestionText}"))
            {//系统提示确定删除吗? 返回值为true执行以下操作
                equipmentGroupEntity = new EquipmentGroupEntity();
                //状态改为delete
                State = ControlState.Delete;
                //赋值对象equipmentGroupEntity的值
                MapControlsToEntity();

                if (equipmentGroupEntity.Delete())
                {//清除成功返回值为true
                    MapEntityToControls();          //赋值对象equipmentGroupEntity的值
                    //系统提示删除数据成功!
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.EquipmentGroups.M0006}", "${res:Global.InformationText}");
                    //修改状态为Read
                    State = ControlState.Read;
                }
            }
        }

        private void grdGroups_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo gridHitInfo = this.grdViewGroups.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition));

            if (gridHitInfo.RowHandle >= 0)
            {
                equipmentGroupEntity = new EquipmentGroupEntity();
                //状态修改
                State = ControlState.Edit;

                grdViewGroups_FocusedRowChanged(this.grdViewGroups, new FocusedRowChangedEventArgs(-1, this.grdViewGroups.FocusedRowHandle));
            }
        }

        /// <summary>
        /// 焦点行改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewGroups_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {//存在行焦点获取行数据到控件
                this.txtGroupName.Text = this.grdViewGroups.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_EQUIPMENT_GROUP_NAME).ToString();
                this.txtSPEC.Text = this.grdViewGroups.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_SPEC).ToString();
                this.txtDescription.Text = this.grdViewGroups.GetRowCellValue(e.FocusedRowHandle, EMS_EQUIPMENT_GROUPS_FIELDS.FIELD_DESCRIPTION).ToString();
            }
            else
            {//不存在行焦点则清空控件数据
                this.txtGroupName.Text = string.Empty;
                this.txtSPEC.Text = string.Empty;
                this.txtDescription.Text = string.Empty;
            }
        }

        #endregion
    }
}

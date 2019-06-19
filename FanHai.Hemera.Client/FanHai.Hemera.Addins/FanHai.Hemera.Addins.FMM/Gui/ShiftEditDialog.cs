//==================================================================================
// 修改人               修改时间              说明
//----------------------------------------------------------------------------------
// Peter.Zhang          2012-01-24            添加注释 
//==================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class ShiftEditDialog : BaseDialog
    {
        private Shift _shift = null;          
        public ShiftEditDialog()
        {
            InitializeComponent();
        }
        public ShiftEditDialog(string scheduleKey, Shift shift)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.Title}"))    //班次编辑
        {
            InitializeComponent();            
            _shift = shift;
            _shift.ScheduleKey = scheduleKey; 
            //如果存在班次信息主键说明已经安排了时间 
            if (_shift.ShiftKey != string.Empty)
            {
                //执行获取数据到视图控件 
                MapDataToControl();
            }
        }
        private void MapDataToControl()
        {           
            DataSet dsShift = new DataSet();
            //获取排班信息 
            dsShift = _shift.GetShift();
            if (_shift.ErrorMsg == string.Empty)
            {
                //存在表
                if (dsShift.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
                {
                    _shift.ShiftName=dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_SHIFT_NAME].ToString();
                    _shift.Descriptions = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_DESCRIPTIONS].ToString();
                    _shift.StartTime = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_START_TIME].ToString();
                    _shift.EndTime = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_END_TIME].ToString();
                    _shift.OverDay = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_OVER_DAY].ToString();
                    _shift.Editor = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_EDITOR].ToString();
                    _shift.EditTimeZone = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_EDIT_TIMEZONE].ToString();
                    _shift.EditTime = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME].Rows[0][CAL_SHIFT.FIELD_EDIT_TIME].ToString();
                    _shift.IsInitializeFinished = true;

                    this.txtName.Text = _shift.ShiftName;
                    this.txtDes.Text = _shift.Descriptions;
                    this.lueStartTime.EditValue = _shift.StartTime;
                    this.lueEndTime.EditValue = _shift.EndTime;
                    //跨天为0
                    if (_shift.OverDay == "0")
                    {
                        this.ceOverDay.Checked = false;
                    }
                    else
                    {
                        this.ceOverDay.Checked = true;
                    }
                }
            }
            else
            {
                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}" + _shift.ErrorMsg);
            }
           
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShiftEditDialog_Load(object sender, EventArgs e)
        {
            InitUIByResourceFile();
            BindDataToLookUpEdit();
        }

        #region InitUIByResourceFile
        /// <summary>
        /// InitUIByResourceFile
        /// </summary>
        private void InitUIByResourceFile()
        {
            this.lblDes.Text = StringParser.Parse("${res:Global.Description}");
            this.lblEndTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.EndTime}");
            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.ShiftName}");
            this.lblStartTime.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.StartTime}");
            this.ceOverDay.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.IsOverDay}");
            this.btnClose.Text = StringParser.Parse("${res:Global.CloseButtonText}");
            this.btnSave.Text = StringParser.Parse("${res:Global.Save}");
        }
        #endregion

        #region BindDataToLookUpEdit
        /// <summary>
        /// Bind data to LookUpEdit
        /// </summary>
        private void BindDataToLookUpEdit()
        {           
            DataSet dataSetBackAll = new DataSet(); //all data to receive           
            //UnregisterChannel
            CallRemotingService.UnregisterChannel();
            //get server object factory
            IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();         
            //get result of excute sql
            dataSetBackAll = iServerObjFactory.CreateIShift().GetLookUpEditData("Shift_Time_Type");
            lueStartTime.Properties.DisplayMember = "CODE";
            lueStartTime.Properties.ValueMember = "CODE";
            lueStartTime.Properties.DataSource = dataSetBackAll.Tables[0];
            //lueStartTime.ItemIndex = 0;

            lueEndTime.Properties.DisplayMember = "CODE";
            lueEndTime.Properties.ValueMember = "CODE";
            lueEndTime.Properties.DataSource = dataSetBackAll.Tables[0];
            //lueEndTime.ItemIndex = 0;
        }
        #endregion

        #region btnSave_Click
        /// <summary>
        /// Save Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //系统提示确定要保存吗 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                bool isNew = true;
                if (!CheckData())
                {//初始化判定  
                    return;
                }
                if (_shift.ShiftKey != string.Empty)
                {//主键 
                    isNew = false;
                }
                MapDataToEntity();
                if (isNew)
                {
                    //添加
                    if (_shift.Insert())
                    {
                        //系统提示保存成功
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        //保存失败
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _shift.ErrorMsg);
                    }
                }
                else
                {
                    //修改
                    if (_shift.Update())
                    {
                        if (_shift.ErrorMsg != "")
                        {
                            //EDITTIMEEXP 
                            if (_shift.ErrorMsg == COMMON_FIELDS.FIELD_COMMON_EDITTIME_EXP)
                            {
                                //数据已经被修改，请刷新后再操作！
                                MessageService.ShowWarning("${res:Global.RecordExpired}");
                            }
                            else
                            {
                                MessageService.ShowMessage(_shift.ErrorMsg, "${res:Global.SystemInfo}");
                            }
                        }
                        else
                        {
                            //系统提示更新成功！
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.UpdateFailed}" + _shift.ErrorMsg);
                    }
                }
            }
        }
        #endregion

        private bool CheckData()
        {
            if (txtName.Text == string.Empty)
            {//名称不能为空
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.NameISNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (lueStartTime.Text == string.Empty)
            {//开始时间不能为空
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.StartTimeISNull}", "${res:Global.SystemInfo}");
                return false;
            }
            if (lueEndTime.Text == string.Empty)
            {//结束时间不能为空
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.EndTimeISNull}", "${res:Global.SystemInfo}");
                return false;
            }           
            return true;
        }
        #region MapDataToEntity
        /// <summary>
        /// MapDataToEntity
        /// </summary>
        private void MapDataToEntity()
        {           
            if (_shift.ShiftKey == string.Empty)
            {
                _shift.ShiftKey =  CommonUtils.GenerateNewKey(0);
                _shift.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _shift.CreateTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
            else
            {
                _shift.EditTime = "9999-12-31 23:59:59";
                _shift.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _shift.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
            _shift.ShiftName = this.txtName.Text;
            _shift.Descriptions = this.txtDes.Text;
            _shift.StartTime = lueStartTime.Text;
            _shift.EndTime = lueEndTime.Text;            
            if (ceOverDay.Checked)
            {
                _shift.OverDay = "1";
            }
            else
            {
                _shift.OverDay = "0";
            } 
        }
        #endregion

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

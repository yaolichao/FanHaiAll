using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示班次管理界面的用户自定义控件类。
    /// </summary>
    public partial class ScheduleCtrl : BaseUserCtrl
    {
        #region define private variable
        private new delegate void AfterStateChanged(ControlState controlState);
        private new AfterStateChanged afterStateChanged = null;

        private ControlState cState;
        private Shift _shift = new Shift();
        private Schedule _schedule = null;
        #endregion

        #region Constructor
        public ScheduleCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            CtrlState = ControlState.ReadOnly;
            InitEmptyShiftDataSet();
        }
        public ScheduleCtrl(Schedule schedule)
        {
            InitializeComponent();
            _schedule = schedule;
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            if (_schedule.ScheduleKey == string.Empty)
            {
                CtrlState = ControlState.ReadOnly;
                InitEmptyShiftDataSet();
            }
            else
            {
                CtrlState = ControlState.Edit;
                BindDataToControl();
                _schedule.IsInitializeFinished = true;
            }
        }
        #endregion

        #region State Changed
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

        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of New
                case ControlState.New:
                    tsbCancel.Enabled = true;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = true;

                    btnAdd.Enabled = false;
                    btnDel.Enabled = false;
                    btnEdit.Enabled = false;

                    txtSchedule.Text = string.Empty;
                    txtDesc.Text = string.Empty;
                    txtOverTime.Text = "0";
                    txtDesc.Properties.ReadOnly = false;
                    txtOverTime.Properties.ReadOnly = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    tsbCancel.Enabled = false;
                    tsbDelete.Enabled = true;
                    tsbSave.Enabled = true;

                    btnEdit.Enabled = true;
                    btnDel.Enabled = true;
                    btnAdd.Enabled = true;
                    txtDesc.Properties.ReadOnly = false;
                    txtOverTime.Properties.ReadOnly = false;
                    txtSchedule.Properties.ReadOnly = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    tsbCancel.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = false;

                    btnAdd.Enabled = false;
                    btnDel.Enabled = false;
                    btnEdit.Enabled = false;
                    txtOverTime.Properties.ReadOnly = true;
                    txtDesc.Properties.ReadOnly = true;
                    txtSchedule.Properties.ReadOnly = false;
                    break;
                    #endregion
            }
        }
        #endregion

        #region Page_load
        private void ScheduleCtrl_Load(object sender, EventArgs e)
        {
            InitUIResourcesByCulture();
            checkEdit.QueryCheckStateByValue += new QueryCheckStateByValueEventHandler(ControlUtils.QueryCheckStateByValue);
            GridViewHelper.SetGridView(ShiftView);
        }
        #endregion

        #region InitUIResourcesByCulture
        protected override void InitUIResourcesByCulture()
        {
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbOpen.Text = StringParser.Parse("${res:Global.Query}");
            this.tsbCancel.Text = StringParser.Parse("${res:Global.Cancel}");
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.btnAdd.Text = StringParser.Parse("${res:Global.AddButtonText}");
            this.btnDel.Text = StringParser.Parse("${res:Global.Delete}");
            this.btnEdit.Text = StringParser.Parse("${res:Global.EditButtonText}");
            //this.basicInfoGroup.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.BasicInfo}");
            this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            this.lblScheduleName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.ScheduleName}");
            this.shift_name.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.ShiftName}");
            this.start_time.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.StartTime}");
            this.end_time.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.EndTime}");
            this.over_day.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftEditDialog.IsOverDay}");

            labelControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.lbl.0001}");//最大跨度(分钟)

            //this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}");//班次管理
        }
        #endregion

        #region BindDataToControl
        private void BindDataToControl()
        {
            DataSet dsShift = new DataSet();
            this.txtSchedule.Text = _schedule.ScheduleName;
            this.txtDesc.Text = _schedule.Description;
            this.txtOverTime.Text = _schedule.MaxOverLapTime;

            if (_schedule.ScheduleKey != string.Empty)
            {
                dsShift = _schedule.GetShift();
                if (_schedule.ErrorMsg == "")
                {
                    if (dsShift.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
                    {
                        ShiftControl.MainView = ShiftView;
                        ShiftControl.DataSource = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME];
                    }
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}" + _schedule.ErrorMsg);
                }
            }

        }
        #endregion

        #region InitEmptyShiftDataSet
        private void InitEmptyShiftDataSet()
        {
            List<string> fields = new List<string>()
                                                    {
                                                        CAL_SHIFT.FIELD_SHIFT_KEY,
                                                        CAL_SHIFT.FIELD_SHIFT_NAME,
                                                        CAL_SHIFT.FIELD_START_TIME,
                                                        CAL_SHIFT.FIELD_END_TIME,
                                                        CAL_SHIFT.FIELD_OVER_DAY
                                                    };
            DataTable dataTable = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(CAL_SHIFT.DATABASE_TABLE_NAME, fields);
            ShiftControl.MainView = ShiftView;
            ShiftControl.DataSource = dataTable;
        }
        #endregion

        #region btnAdd_Click
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Shift shift = new Shift();
            ShiftEditDialog shiftDialog = new ShiftEditDialog(_schedule.ScheduleKey, shift);
            if (DialogResult.OK == shiftDialog.ShowDialog())
            {
                ShiftView.AddNewRow();
                DataRow newRow = ShiftView.GetDataRow(ShiftView.FocusedRowHandle);
                newRow[CAL_SHIFT.FIELD_SHIFT_KEY] = shift.ShiftKey;
                newRow[CAL_SHIFT.FIELD_SHIFT_NAME] = shift.ShiftName;
                newRow[CAL_SHIFT.FIELD_START_TIME] = shift.StartTime;
                newRow[CAL_SHIFT.FIELD_END_TIME] = shift.EndTime;
                newRow[CAL_SHIFT.FIELD_OVER_DAY] = shift.OverDay;
                newRow.EndEdit();
                ShiftView.UpdateCurrentRow();
                ShiftView.ShowEditor();
            }
        }
        #endregion

        #region btnEdit_Click
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int rowHandle = 0;
            if (ShiftView.FocusedRowHandle < 0)
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Addin.SelectRemaind}", "${res:Global.SystemInfo}");
            }
            else
            {
                rowHandle = ShiftView.FocusedRowHandle;
                Shift shift = new Shift();
                shift.ShiftKey = this.ShiftView.GetRowCellValue(rowHandle, shift_key).ToString();
                ShiftEditDialog shiftDialog = new ShiftEditDialog(_schedule.ScheduleKey, shift);
                if (DialogResult.OK == shiftDialog.ShowDialog())
                {
                    ShiftView.SetRowCellValue(rowHandle, shift_name, shift.ShiftName);
                    ShiftView.SetRowCellValue(rowHandle, start_time, shift.StartTime);
                    ShiftView.SetRowCellValue(rowHandle, end_time, shift.EndTime);
                    ShiftView.SetRowCellValue(rowHandle, over_day, shift.OverDay);
                }
            }
        }
        #endregion

        #region btnDel_Click
        private void btnDel_Click(object sender, EventArgs e)
        {
            //系统提示确定要删除吗？
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"),
                StringParser.Parse("${res:Global.SystemInfo}")))
            {
                int rowHandle = 0;
                //焦点没有获取到
                if (ShiftView.FocusedRowHandle < 0)
                {
                    //系统提示没有选中行
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addin.SelectRemaind}", "${res:Global.SystemInfo}");
                    return;
                }
                else
                {
                    rowHandle = ShiftView.FocusedRowHandle;
                    Shift shift = new Shift();
                    shift.ScheduleKey = _schedule.ScheduleKey;
                    shift.ShiftKey = this.ShiftView.GetRowCellValue(rowHandle, shift_key).ToString();
                    if (shift.ShiftKey != string.Empty && shift.ScheduleKey != string.Empty)
                    {
                        shift.DeleteShift();
                        if (shift.ErrorMsg == string.Empty)
                        {
                            //delete row from gridView 删除成功                                            
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}", "${res:Global.SystemInfo}");
                            ShiftView.DeleteSelectedRows();
                        }
                        else
                        {
                            MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}" + shift.ErrorMsg);
                        }
                    }
                }
            }
        }
        #endregion

        #region tsbQuery_Click
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            //界面整合
            Schedule schedule = new Schedule();
            schedule.ScheduleName = txtSchedule.Text;
            DataSet dsSchedule = new DataSet();
            dsSchedule = schedule.SearchSchedule();
            planNameGC.MainView = planNameGrid;
            planNameGC.DataSource = dsSchedule.Tables[0];

            //Schedule scheduleEntity = new Schedule();
            ////显示排班计划查询界面 
            //SearchScheduleDialog searchSchedule = new SearchScheduleDialog();
            ////排班计划查询界面返回结果为OK，执行下面操作
            //if (DialogResult.OK == searchSchedule.ShowDialog())
            //{
            //    scheduleEntity = searchSchedule._shcedule;
            //    //返回值主键不为空 
            //    if (scheduleEntity.ScheduleKey != string.Empty)
            //    {
            //        foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            //        {
            //            //判定标题名为班次管理_班次名称 
            //            if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}") + "_" + scheduleEntity.ScheduleName)
            //            {
            //                viewContent.WorkbenchWindow.SelectWindow();
            //                return;
            //            }
            //        }
            //        ScheduleViewContent scheduleViewContent = new ScheduleViewContent(scheduleEntity.ScheduleName, scheduleEntity);
            //        WorkbenchSingleton.Workbench.ShowView(scheduleViewContent);
            //    }
            //}
        }
        #endregion

        #region tsbNew_Click
        private void tsbNew_Click(object sender, EventArgs e)
        {
            txtSchedule.ReadOnly = false;
            _schedule = new Schedule(CommonUtils.GenerateNewKey(0));
            //State = ControlState.New;   
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                //判定标题名为班次管理
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    ScheduleCtrl ctrl = (ScheduleCtrl)viewContent.Control.Controls.Find("ScheduleCtrl", true)[0];
                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }
            ScheduleViewContent scheduleViewContent = new ScheduleViewContent("", new Schedule());
            WorkbenchSingleton.Workbench.ShowView(scheduleViewContent);
        }
        #endregion

        #region tsbSave_Click
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //系统提示是否要保存吗？
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                bool isNew = (CtrlState == ControlState.New);

                if (this.txtSchedule.Text == string.Empty)
                {
                    //系统提示名称不能为空
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.NameISNull}", "${res:Global.SystemInfo}");
                    return;
                }

                try
                {
                    int nOverTime = int.Parse(this.txtOverTime.Text);
                }
                catch
                {
                    //MessageService.ShowMessage("班别最大延迟时间必须是整数");
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.lbl.0002}"));
                    return;
                }

                MapControlToEntity();
                //状态new为true
                if (isNew)
                {
                    if (_schedule.Insert())
                    {
                        //系统提示保存成功 
                        MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                        WorkbenchSingleton.Workbench.ActiveViewContent.TitleName
                                 = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}") + "_" + _schedule.ScheduleName;
                        //以保存因此状态改为edit
                        CtrlState = ControlState.Edit;
                    }
                    else
                    {
                        //保存失败
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _schedule.ErrorMsg);
                    }
                }
                else
                {
                    if (_schedule.Update())
                    {
                        if (_schedule.ErrorMsg != "")
                        {
                            MessageService.ShowMessage(_schedule.ErrorMsg, "${res:Global.SystemInfo}");
                        }
                        else
                        {
                            //系统提示更新成功！
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                        }
                    }
                    else
                    {
                        //更新失败！原因
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.UpdateFailed}" + _schedule.ErrorMsg);
                    }
                }
                tsbOpen_Click(null, null);
            }
        }
        #endregion

        #region MapControlToEntity
        private void MapControlToEntity()
        {
            _schedule.ScheduleName = this.txtSchedule.Text;
            _schedule.Description = this.txtDesc.Text;
            _schedule.MaxOverLapTime = this.txtOverTime.Text;

            //通过有没有主键判定给变量辅助修改还是新增
            if (_schedule.ScheduleKey == string.Empty)
            {
                _schedule.Creator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _schedule.CreateTime = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
            else
            {
                _schedule.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                _schedule.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            }
        }
        #endregion

        #region tsbCancel_Click
        private void tsbCancel_Click(object sender, EventArgs e)
        {
            this.txtDesc.Text = string.Empty;
            this.txtSchedule.Text = string.Empty;
        }
        #endregion

        #region tsbDelete_Click
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //系统提示确定要删除吗？
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                DataSet dataSetSchedule = new DataSet();
                dataSetSchedule.Tables.Add();
                dataSetSchedule.Tables[0].TableName = CAL_SCHEDULE.DATABASE_TABLE_NAME;
                dataSetSchedule.Tables[0].Columns.Add(CAL_SCHEDULE.FIELD_SCHEDULE_KEY);
                dataSetSchedule.Tables[0].Rows.Add();
                dataSetSchedule.Tables[0].Rows[0][0] = _schedule.ScheduleKey;

                dataSetSchedule.Tables.Add();
                dataSetSchedule.Tables[1].TableName = CAL_SHIFT.DATABASE_TABLE_NAME;
                dataSetSchedule.Tables[1].Columns.Add(CAL_SHIFT.FIELD_SHIFT_KEY);
                for (int i = 0; i < ShiftView.RowCount; i++)
                {
                    dataSetSchedule.Tables[1].Rows.Add();
                    dataSetSchedule.Tables[1].Rows[i][0] = ShiftView.GetRowCellValue(i, shift_key).ToString();
                }
                //删除信息
                _schedule.DeleteSchedule(dataSetSchedule);
                if (_schedule.ErrorMsg == "")
                {
                    //系统提示删除成功！
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}", "${res:Global.SystemInfo}");
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        //标题等于班次管理
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}"))
                        {
                            //关闭窗体
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }
                    //班次管理
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                       StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleViewContent.ViewContentScheduleTitle}");
                    //状态设置为new
                    CtrlState = ControlState.New;

                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}" + _schedule.ErrorMsg);
                }
            }
        }
        #endregion

        private void ShiftView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void Content_Paint(object sender, PaintEventArgs e)
        {

        }

        private void planNameGrid_DoubleClick(object sender, EventArgs e)
        {
            int index = planNameGrid.FocusedRowHandle;
            if (index >= 0)
            {
                _schedule.ScheduleKey = planNameGrid.GetRowCellValue(index, schedule_key).ToString();
                _schedule.ScheduleName = planNameGrid.GetRowCellValue(index, schedule_name).ToString();
                _schedule.Description = planNameGrid.GetRowCellValue(index, descriptions).ToString();
                _schedule.MaxOverLapTime = planNameGrid.GetRowCellValue(index, MAXOVERLAPTIME).ToString();
                tsbSave.Enabled = true;
                BindDataToControl();
                this.txtSchedule.Properties.ReadOnly = false;
                this.txtDesc.ReadOnly = false;
                this.txtOverTime.ReadOnly = false;
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDel.Enabled = true;
                CtrlState = ControlState.Edit;
                _schedule.IsInitializeFinished = true;
            }
        }
    }
}

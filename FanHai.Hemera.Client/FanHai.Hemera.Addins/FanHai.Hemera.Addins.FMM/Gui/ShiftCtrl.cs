using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace FanHai.Hemera.Addins.FMM
{    /// <summary>
     /// 排班管理窗体类。
     /// </summary>
    public partial class ShiftCtrl : BaseUserCtrl
    {
        #region 定义变量
        private new delegate void AfterStateChanged(ControlState controlState);         //状态变更的事件委托类。
        private new AfterStateChanged afterStateChanged = null;                     //状态变更事件。
        private ControlState cState;                                                //窗体状态。
        private Schedule _schedule = null;
        private RepositoryItemComboBox cbShift = new RepositoryItemComboBox();
        private Dictionary<string, Shift> _shiftDictionary = new Dictionary<string, Shift>();
        private DataTable originalDataTable = new DataTable();
        private string _monthKey = "";
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShiftCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            CtrlState = ControlState.New;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shift">表示班次的对象。</param>
        public ShiftCtrl(Shift shift)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            CtrlState = ControlState.New;
        }
        #endregion

        #region Page Load
        #region ShiftCtrl_Load
        private void ShiftCtrl_Load(object sender, EventArgs e)
        {
            InitUIResourceByFile();
            //绑定数据年份和月份控件的数据  
            BindDataToControl();
            GridViewHelper.SetGridView(shiftView);
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
                    lueSchedule.Properties.ReadOnly = false;
                    tsbSave.Enabled = false;
                    tsbDelete.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    tsbSave.Enabled = true;
                    tsbDelete.Enabled = true;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.Read:
                    lueSchedule.Properties.ReadOnly = true;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = false;
                    this.lueSchedule.EditValue = null;
                    this.lueYear.EditValue = null;
                    this.cbeMonth.Text = "";
                    break;
                    #endregion
            }
        }
        #endregion

        #region InitUIResourceByFile
        private void InitUIResourceByFile()
        {
            this.tsbDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.groupInfo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.BasicInfo}");
            this.groupShift.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.ScheduleGroup}");
            this.lblYear.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.lblYear}");
            this.lblMonth.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.lblMonth}");
            this.lblSchedule.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ScheduleCtrl.ScheduleName}");
            this.btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.CreateShiftTable}");
            //this.lblApplicationTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.ShiftCtrl.title}");//排班管理
        }
        #endregion


        #region BindDataToControl
        /// <summary>
        /// 绑定数据到年份和月份控件 
        /// </summary>
        private void BindDataToControl()
        {
            //获取年份 
            #region get years
            DataSet dataSetBackAll = new DataSet(); //all data to receive           
            //UnregisterChannel
            CallRemotingService.UnregisterChannel();
            //get server object factory 远程调用技术 
            IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
            //get result of excute sql 获取年份的数据参数值为basic_year 
            dataSetBackAll = iServerObjFactory.CreateIShift().GetLookUpEditData("Basic_Year");
            lueYear.Properties.DisplayMember = "CODE";
            lueYear.Properties.ValueMember = "CODE";
            //返回的第一张数据表绑定 
            lueYear.Properties.DataSource = dataSetBackAll.Tables[0];
            #endregion

            //获取月份 
            #region get basic shift
            //远程调用技术传入参数basic_shift 
            dataSetBackAll = iServerObjFactory.CreateIShift().GetLookUpEditData("Basic_Shift");
            //返回的表的行数为大于0表示存在数据
            if (dataSetBackAll.Tables.Count > 0)
            {
                for (int i = 0; i < dataSetBackAll.Tables[0].Rows.Count; i++)
                {
                    //数据绑定到下拉列表中 
                    cbShift.Items.Add(dataSetBackAll.Tables[0].Rows[i]["CODE"].ToString());
                }
            }
            this.cbShift.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            #endregion

            //获取排班计划 
            #region get Schedules
            DataSet dsSchedule = new DataSet();
            _schedule = new Schedule();
            //调用SearchSchedule方法获取排班计划 
            dsSchedule = _schedule.SearchSchedule();
            if (dsSchedule.Tables.Count > 0)
            {
                //数据绑定 到下拉列表中 
                lueSchedule.Properties.DataSource = dsSchedule.Tables[0];
                lueSchedule.Properties.DisplayMember = CAL_SCHEDULE.FIELD_SCHEDULE_NAME;
                lueSchedule.Properties.ValueMember = CAL_SCHEDULE.FIELD_SCHEDULE_KEY;
            }
            #endregion


        }
        #endregion

        #endregion       

        #region New Button Click
        private void tsbNew_Click(object sender, EventArgs e)
        {
            CtrlState = ControlState.New;
            ClearDataOfDataGrid();
        }
        #endregion

        /// <summary>
        /// 保存排班计划 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Save Button Click
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.shiftView.State == GridState.Editing && this.shiftView.IsEditorFocused
                                                    && this.shiftView.EditingValueModified)
            {
                this.shiftView.SetFocusedRowCellValue(this.shiftView.FocusedColumn, this.shiftView.EditingValue);
            }
            this.shiftView.UpdateCurrentRow();
            //系统提示确定要保存吗？ 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.SaveRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                if (shiftView.RowCount > 0)
                {
                    #region Map Data To Entity
                    for (int i = 1; i < shiftView.Columns.Count; i = i + 3)
                    {
                        for (int j = 0; j < shiftView.RowCount; j++)
                        {
                            if (shiftView.GetRowCellValue(j, shiftView.Columns[i + 2].Name).ToString() == "")
                            {
                                if (shiftView.GetRowCellValue(j, shiftView.Columns[i].Name).ToString() != "")
                                {
                                    string shiftValue = shiftView.GetRowCellValue(j, shiftView.Columns[i].Name).ToString();
                                    string day = shiftView.GetRowCellValue(j, shiftView.Columns[0].Name).ToString();
                                    string seqNo = (j + 1).ToString();
                                    string shiftKey = shiftView.GetRowCellValue(j, shiftView.Columns[i + 1].Name).ToString();
                                    string startTime = _shiftDictionary[shiftKey].StartTime;
                                    string endTime = _shiftDictionary[shiftKey].EndTime;
                                    string endDate = "";
                                    if (Convert.ToInt32(startTime.Substring(0, 2)) > Convert.ToInt32(endTime.Substring(0, 2)))
                                    {
                                        endDate = Convert.ToDateTime(day).AddDays(1).ToShortDateString() + " " + endTime;
                                    }
                                    else
                                    {
                                        endDate = day + " " + endTime;
                                    }
                                    string startDate = day + " " + startTime;
                                    ScheduleDay scheduleDay = new ScheduleDay();
                                    scheduleDay.OperationAction = OperationAction.New;
                                    scheduleDay.DKey = CommonUtils.GenerateNewKey(0);
                                    scheduleDay.StartTime = startDate;
                                    scheduleDay.EndTime = endDate;
                                    scheduleDay.ShiftValue = shiftValue;
                                    scheduleDay.SeqNo = seqNo;
                                    scheduleDay.ShiftKey = shiftKey;
                                    scheduleDay.Day = day;
                                    _schedule.ScheduleDayList.Add(scheduleDay);
                                    shiftView.SetRowCellValue(j, shiftView.Columns[i + 2].Name, scheduleDay.DKey);
                                }
                            }
                            else
                            {
                                string rowKey = shiftView.GetRowCellValue(j, shiftView.Columns[i + 2].Name).ToString();
                                foreach (ScheduleDay schedule in _schedule.ScheduleDayList)
                                {
                                    if (rowKey == schedule.DKey)
                                    {
                                        if (shiftView.GetRowCellValue(j, shiftView.Columns[i].Name).ToString() != "")
                                        {
                                            schedule.ShiftValue = shiftView.GetRowCellValue(j, shiftView.Columns[i].Name).ToString();
                                        }
                                        else
                                        {
                                            schedule.OperationAction = OperationAction.Delete;
                                            shiftView.SetRowCellValue(j, shiftView.Columns[i + 2].Name, string.Empty);
                                        }
                                    }
                                    if (schedule.IsDirty)
                                    {
                                        schedule.OperationAction = OperationAction.Modified;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (CtrlState == ControlState.New)
                    {
                        if (_schedule.ScheduleDayList.Count > 0)
                        {
                            #region Add data to shift_month table
                            DataTable shiftMonthTable = CAL_SCHEDULE_MONTH.CreateDataTable();
                            string monthKey = CommonUtils.GenerateNewKey(0);
                            Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                {
                                                    {CAL_SCHEDULE_MONTH.FIELD_MKEY,monthKey},
                                                    {CAL_SCHEDULE_MONTH.FIELD_CUR_YEAR,this.lueYear.Text},
                                                    {CAL_SCHEDULE_MONTH.FIELD_CUR_MONTH,this.cbeMonth.Text},
                                                    {CAL_SCHEDULE_MONTH.FIELD_SCHEDULE_KEY,this.lueSchedule.EditValue.ToString()}
                                                };
                            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref shiftMonthTable, rowData);
                            #endregion

                            _schedule.SaveShiftOfSchedule(shiftMonthTable);
                            if (_schedule.ErrorMsg == "")
                            {
                                _monthKey = monthKey;
                                //系统提示保存成功
                                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.SaveSucceed}", "${res:Global.SystemInfo}");
                                CtrlState = ControlState.Edit;
                            }
                            else
                            {
                                //系统提示保存失败
                                MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SaveFailed}" + _schedule.ErrorMsg);
                            }
                        }
                        else
                        {
                            //当前没有保存项！
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.MsgNoDataForSave}", "${res:Global.SystemInfo}");
                        }
                    }
                    else
                    {
                        _schedule.MKey = _monthKey;
                        if (_schedule.UpdateShiftOfSchedule())
                        {
                            //系统提示更新成功
                            MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.UpdateSucceed}", "${res:Global.SystemInfo}");
                            if (CtrlState == ControlState.New)
                                CtrlState = ControlState.Edit;
                        }
                        else
                        {
                            MessageService.ShowError(_schedule.ErrorMsg);
                        }
                    }
                }
            }
        }
        #endregion

        #region Delete Button Click
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            //系统提示确定要删除吗 
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.Msg.DeleteRemind}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                _schedule.MKey = _monthKey;
                _schedule.DeleteShiftOfSchedule();
                if (_schedule.ErrorMsg == "")
                {
                    //系统提示删除成功
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.Msg.DeleteSucceed}", "${res:Global.SystemInfo}");
                    ClearDataOfDataGrid();
                    this.lueYear.EditValue = null;
                    this.cbeMonth.Text = "";
                    this.lueSchedule.EditValue = null;
                }
                else
                {
                    //系统提示删除失败
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.DeleteFailed}" + _schedule.ErrorMsg);
                }
            }
        }
        #endregion       

        /// <summary>
        /// 生成排班计划单击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Query Button Click
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dsShift = new DataSet();
            _schedule = new Schedule();
            string sYear = lueYear.Text;
            string sMonth = cbeMonth.Text;
            object schedule = lueSchedule.EditValue;

            #region CheckEmpty
            if (schedule != null)
            {
                _schedule.ScheduleKey = schedule.ToString();
            }

            if (string.IsNullOrEmpty(sYear))
            {
                //系统提示 请选择年份 
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.Msg.SelectYear}", "${res:Global.SystemInfo}");
                return;
            }
            if (string.IsNullOrEmpty(sMonth))
            {
                //系统提示 请选择月份 
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.Msg.SelectMonth}", "${res:Global.SystemInfo}");
                return;
            }
            if (_schedule.ScheduleKey == string.Empty)
            {
                //系统提示 请选择计划 
                MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.Msg.SelectSchedule}", "${res:Global.SystemInfo}");
                return;
            }
            #endregion

            int year = Convert.ToInt32(sYear);
            int month = Convert.ToInt32(sMonth);

            #region Create Grid   
            //清空数据表内容 
            ClearDataOfDataGrid();
            _shiftDictionary.Clear();
            int days = GetDays(Convert.ToInt32(year), Convert.ToInt32(month));
            //根据排班主键获取排班计划 
            dsShift = _schedule.GetShift();
            int index = 0;
            if (dsShift.Tables.Contains(CAL_SHIFT.DATABASE_TABLE_NAME))
            {
                DataTable dataTable = dsShift.Tables[CAL_SHIFT.DATABASE_TABLE_NAME];
                if (dataTable.Rows.Count > 0)
                {
                    tsbSave.Enabled = true;
                    #region Create Column
                    GridColumn column = new GridColumn();
                    column.Name = "Date";
                    column.FieldName = "Date";
                    column.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.ColumnDateName}");    //日期
                    column.Visible = true;
                    column.OptionsColumn.AllowEdit = false;
                    column.VisibleIndex = index++;
                    shiftView.Columns.Add(column);


                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        #region Bind data to list
                        Shift shift = new Shift();
                        shift.ShiftKey = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_KEY].ToString();
                        shift.ShiftName = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_NAME].ToString();
                        shift.StartTime = dataTable.Rows[i][CAL_SHIFT.FIELD_START_TIME].ToString();
                        shift.EndTime = dataTable.Rows[i][CAL_SHIFT.FIELD_END_TIME].ToString();
                        _shiftDictionary.Add(shift.ShiftKey, shift);
                        #endregion

                        column = new GridColumn();
                        column.Name = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_NAME].ToString();
                        column.FieldName = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_NAME].ToString();
                        column.Caption = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_NAME].ToString();
                        column.ColumnEdit = cbShift;
                        column.Visible = true;
                        column.VisibleIndex = index++;
                        shiftView.Columns.Add(column);

                        GridColumn columnKey = new GridColumn();
                        columnKey.Name = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_KEY].ToString();
                        columnKey.Caption = "Key" + i.ToString();
                        columnKey.FieldName = dataTable.Rows[i][CAL_SHIFT.FIELD_SHIFT_KEY].ToString();
                        columnKey.Visible = false;
                        //columnKey.VisibleIndex = index++;
                        shiftView.Columns.Add(columnKey);

                        GridColumn rowKey = new GridColumn();
                        rowKey.Name = "rowKey" + i.ToString();
                        rowKey.Caption = "rowKey" + i.ToString();
                        rowKey.FieldName = "rowKey" + i.ToString();
                        rowKey.Visible = false;
                        //rowKey.VisibleIndex = index++;
                        shiftView.Columns.Add(rowKey);
                    }
                    #endregion

                    #region Bind Empty DataTable To Grid
                    List<string> fields = new List<string>();
                    for (int j = 0; j < shiftView.Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            fields.Add("Date");
                        }
                        else
                        {
                            fields.Add(shiftView.Columns[j].Name);
                        }
                    }
                    DataTable shiftTable = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(CAL_SHIFT.DATABASE_TABLE_NAME, fields);
                    shiftControl.MainView = shiftView;
                    shiftControl.DataSource = shiftTable;

                    #region Add Rows                   
                    int columnCount = 0;
                    columnCount = shiftTable.Columns.Count;
                    for (int k = 1; k < days + 1; k++)
                    {
                        shiftView.AddNewRow();
                        DataRow newRow = shiftView.GetDataRow(shiftView.FocusedRowHandle);

                        newRow["Date"] = year.ToString("0000") + "-" + month.ToString("00") + "-" + k.ToString("00");
                        for (int i = columnCount; i > 2; i = i - 3)
                        {
                            newRow[i - 2] = shiftTable.Columns[i - 2].ColumnName;
                        }
                        newRow.EndEdit();
                        shiftView.UpdateCurrentRow();
                        shiftView.ShowEditor();

                    }

                    #endregion
                    #endregion

                    this.shiftView.FocusedRowHandle = 0;
                }
                else
                {
                    //系统提示请先添加班次信息
                    MessageService.ShowMessage("${res:FanHai.Hemera.Addins.EMS.ShiftCtrl.Msg.AddShiftFirst}", "${res:Global.SystemInfo}");
                    return;
                }
            }

            #endregion

            #region Bind Data To Grid
            if (CtrlState == ControlState.Edit)
            {
                DataSet dataSetShifts = new DataSet();
                IServerObjFactory serverObjFactory = CallRemotingService.GetRemoteObject();
                dataSetShifts = serverObjFactory.CreateISchedule().GetShiftOfSchedule(lueYear.Text, cbeMonth.Text, lueSchedule.EditValue.ToString());
                string errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dataSetShifts);
                if (errorMsg == "")
                {
                    if (dataSetShifts.Tables.Contains(CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME))
                    {
                        originalDataTable = dataSetShifts.Tables[CAL_SCHEDULE_DAY.DATABASE_TABLE_NAME];
                        //_schedule.MKey=originalDataTable.Rows[0][CAL_SCHEDULE_DAY.FIELD_MKEY].ToString();
                        _monthKey = originalDataTable.Rows[0][CAL_SCHEDULE_DAY.FIELD_MKEY].ToString();
                        //bind data to Grid
                        for (int i = 0; i < originalDataTable.Rows.Count; i++)
                        {
                            string day = Convert.ToDateTime(originalDataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_DAY]).ToString("yyyy-MM-dd");
                            string shiftValue = originalDataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_SHIFT_VALUE].ToString();
                            string shiftKey = originalDataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_SHIFT_KEY].ToString();
                            string dKey = originalDataTable.Rows[i][CAL_SCHEDULE_DAY.FIELD_DKEY].ToString();
                            for (int rowNo = 0; rowNo < shiftView.RowCount; rowNo++)
                            {
                                string columnDay = shiftView.GetRowCellValue(rowNo, shiftView.Columns[0].Name).ToString();
                                for (int columnNo = 2; columnNo < shiftView.Columns.Count; columnNo = columnNo + 3)
                                {
                                    string columnKey = shiftView.GetRowCellValue(rowNo, shiftView.Columns[columnNo].Name).ToString();
                                    if (day == columnDay && shiftKey == columnKey)
                                    {
                                        shiftView.SetRowCellValue(rowNo, shiftView.Columns[columnNo - 1].Name, shiftValue);
                                        shiftView.SetRowCellValue(rowNo, shiftView.Columns[columnNo + 1].Name, dKey);
                                        //Map Data To Entity
                                        ScheduleDay scheduleDay = new ScheduleDay();
                                        scheduleDay.ShiftValue = shiftValue;
                                        scheduleDay.DKey = dKey;
                                        scheduleDay.OperationAction = OperationAction.Update;
                                        scheduleDay.IsInitializeFinished = true;
                                        _schedule.ScheduleDayList.Add(scheduleDay);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.ReadFailed}！" + errorMsg);
                }
            }
            #endregion
        }
        #endregion

        #region Get days of month
        /// <summary>
        /// Get days of month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>days</returns>
        public int GetDays(int year, int month)
        {
            int days;
            if (month == 1 | month == 3 | month == 5 | month == 7 | month == 8
                | month == 10 | month == 12)
            {
                days = 31;
            }
            else
            {
                if (month == 2)
                {
                    if (year % 4 == 0 & year % 100 != 0 | year % 400 == 0)
                    {
                        days = 29;
                    }
                    else
                    {
                        days = 28;
                    }
                }
                else
                {
                    days = 30;
                }
            }
            return days;
        }
        #endregion

        /// <summary>
        /// 清除数据表中的数据 
        /// </summary>
        #region Clear Data of DataGrid
        private void ClearDataOfDataGrid()
        {
            for (int i = shiftView.RowCount - 1; i >= 0; i--)
            {//循环行次数
                shiftView.DeleteRow(i);                //删除行记录
            }
            shiftView.Columns.Clear();
        }
        #endregion

        /// <summary>
        /// 月份控件改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region cbeMonth_SelectedIndexChanged
        private void cbeMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lueYear.Text != "")
            {
                GetValueOfSchedule(lueYear.Text, cbeMonth.Text);
            }
            ClearDataOfDataGrid();
        }
        #endregion

        /// <summary>
        /// 年份控件改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueYear_EditValueChanged(object sender, EventArgs e)
        {
            if (cbeMonth.Text != "")
            {
                GetValueOfSchedule(lueYear.Text, cbeMonth.Text);
            }
            ClearDataOfDataGrid();
        }

        /// <summary>
        /// 通过年份和月份查询主键 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void GetValueOfSchedule(string year, string month)
        {
            string scheduleKey = "";
            //远程调用 
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            //通过年份和月份获取主键
            scheduleKey = serverFactory.CreateISchedule().GetScheduleKey(year, month);
            if (scheduleKey != "")
            {//如果主键不为空
                this.lueSchedule.Properties.ReadOnly = true;     //控件可用
                this.lueSchedule.EditValue = scheduleKey;
                //状态ctrlstate修改为edit状态 
                CtrlState = ControlState.Edit;
            }
            else
            {//如果主键为空
                this.lueSchedule.Properties.ReadOnly = false;    //控件不可用 
                this.lueSchedule.EditValue = null;
                //状态修改为new 
                CtrlState = ControlState.New;
            }
        }

        private void shiftView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// E-Mail:  peter.zhang@foxmail.com
//----------------------------------------------------------------------------------
//==================================================================================
// 修改人               修改时间              说明
//----------------------------------------------------------------------------------
// Peter.Zhang          2012-01-24            添加注释 
// chao.pang            2012-02-24            添加注释
//==================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 班别操作的实体类。
    /// </summary>
    public class Shift : EntityObject
    {
        #region define attribute
        private string _shiftKey="";
        private string _shiftName = "";
        private string _starTime = "";
        private string _endTime = "";
        private string _descriptions = "";
        private string _overDay = "0";
        private string _errorMsg = "";
        private string _scheduleKey = "";
        #endregion      

        #region Properties 
        public string ShiftKey
        {
            get
            {
                return this._shiftKey;
            }
            set
            {
                this._shiftKey = value;
            }
        }
        public string ScheduleKey
        {
            get
            {
                return this._scheduleKey;
            }
            set
            {
                this._scheduleKey = value;
            }
        }
        public string ShiftName
        {
            get
            {
                return this._shiftName;
            }
            set
            {
                this._shiftName = value;
                ValidateDirtyList(CAL_SHIFT.FIELD_SHIFT_NAME, value);
            }
        }

        public string StartTime
        {
            get
            {
                return this._starTime;
            }
            set
            {
                this._starTime = value;
                ValidateDirtyList(CAL_SHIFT.FIELD_START_TIME, value);
            }
        }

        public string EndTime
        {
            get
            {
                return this._endTime;
            }
            set
            {
                this._endTime = value;
                ValidateDirtyList(CAL_SHIFT.FIELD_END_TIME, value);
            }
        }

        public string OverDay
        {
            get
            {
                return this._overDay;
            }
            set
            {
                this._overDay = value;
                ValidateDirtyList(CAL_SHIFT.FIELD_OVER_DAY, value);
            }
        }


        public string Descriptions
        {
            get
            {
                return this._descriptions;
            }
            set
            {
                this._descriptions = value;
                ValidateDirtyList(CAL_SHIFT.FIELD_DESCRIPTIONS, value);
            }
        }

        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }

        #endregion
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Shift(){ }

        /// <summary>
        /// One param construct function
        /// </summary>
        public Shift(string shiftKey)
        {
            this._shiftKey = shiftKey;
        }
        #region Action

        #region Insert
        public override bool Insert()
        {
             
            DataSet dataSet = new DataSet();
            DataTable shiftTable = CAL_SHIFT.CreateDataTable();
            //准备插入到数据库中的数据。 modi by chao.pang
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                     {CAL_SHIFT.FIELD_SHIFT_KEY,_shiftKey},
                                                     {CAL_SHIFT.FIELD_SHIFT_NAME,_shiftName},
                                                     {CAL_SHIFT.FIELD_DESCRIPTIONS,_descriptions},
                                                     {CAL_SHIFT.FIELD_START_TIME,_starTime},
                                                     {CAL_SHIFT.FIELD_END_TIME,_endTime},
                                                     {CAL_SHIFT.FIELD_OVER_DAY,_overDay},
                                                     {CAL_SHIFT.FIELD_CREATOR,Creator},
                                                     {CAL_SHIFT.FIELD_CREATE_TIMEZONE,CreateTimeZone},
                                                     {CAL_SHIFT.FIELD_EDITOR,Creator},                                                     
                                                     {CAL_SHIFT.FIELD_EDIT_TIMEZONE,CreateTimeZone},
                                                     {CAL_SCHEDULE.FIELD_SCHEDULE_KEY,_scheduleKey}
                                                     
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref shiftTable, dataRow);
            //为远程调用函数准备参数。 modi by chao.Pang
            dataSet.Tables.Add(shiftTable);
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //远程调用进行添加操作 modi by chao.pang
                    dsReturn = serverFactory.CreateIShift().AddShift(dataSet);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg != "")
                    {                       
                        return false;
                    }
                    else
                    {
                        this.ResetDirtyList();
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }
        #endregion


        #region Update
        public override bool Update()
        {
            //if (IsDirty)
            //{
                DataSet dataSet = new DataSet();
                if (this.DirtyList.Count > 1)
                {
                    DataTable shiftTable = DataTableHelper.CreateDataTableForUpdateBasicData(CAL_SHIFT.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _shiftKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref shiftTable, rowData);
                    }
                    if (shiftTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(shiftTable);
                    }

                    try
                    {
                        DataSet dsReturn = null;
                        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                        if (null != serverFactory)
                        {
                            dsReturn = serverFactory.CreateIShift().UpdateShift(dataSet);
                            _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                            if (_errorMsg != "")
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorMsg = ex.Message;
                    }
                    finally
                    {
                        CallRemotingService.UnregisterChannel();
                    }
                }
                else
                {
                    _errorMsg = "${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}";               
                }
            //}
            //else
            //{
            //    _errorMsg = "${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}";               
            //}
            return true;
        }
        #endregion  
        /// <summary>
        /// 获取排班基本信息 modi by chao.Pang
        /// </summary>
        /// <returns></returns>
        public DataSet GetShift()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIShift().GetShift(_shiftKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        public void DeleteShift()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIShift().DeleteShift(_scheduleKey, _shiftKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }            
        }
        #endregion

        /// <summary>
        /// 获取当前班别的名称。
        /// </summary>
        /// <returns>班别的名称</returns>
        public String GetCurrShiftName()
        {
            string strShiftName = string.Empty;
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strShiftName = serverFactory.Get<IShift>().GetShiftNameBySysdate();
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return strShiftName;
        }
        ///// <summary>
        ///// 检查班别是否存在。当前时间对应的排班班别是否是指定的班别值。必须先进行排班。
        ///// </summary>
        ///// <param name="shiftValue">班别值</param>
        ///// <returns>返回班别主键的标识字符串。空字符串代表班别不存在</returns>
        //public string IsShiftValueExists(string shiftValue)
        //{
        //    string seleShiftKey = string.Empty;
        //    try
        //    {
        //        this._errorMsg = string.Empty;
        //        //创建远程调用的工厂对象。
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        //判别班别是否存在。返回班别对应的主键标识符。
        //        seleShiftKey = serverFactory.Get<IShift>().IsExistsShift(shiftValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }
        //    //返回班别对应的主键标识符。如果是空字符串代表班别不存在。
        //    return seleShiftKey;
        //}
    }
    /// <summary>
    /// 对排班计划进行相关操作的实体类。
    /// </summary>
    public class Schedule : EntityObject
    {
        #region define attribute
        private string _scheduleKey = "";                                          //排班计划主键
        private string _scheduleName = "";                                         //排班计划名称
        private string _overTime = "0";                                            //是否跨天
        private string _descriptions = "";                                         //描述
        private string _errorMsg = "";                                             //错误消息
        private List<ScheduleDay> _scheduleDayList = new List<ScheduleDay>();      //每天排班计划的集合。
        #endregion

        #region Properties of schedule
        /// <summary>
        /// 排班计划主键。
        /// </summary>
        public string ScheduleKey
        {
            get
            {
                return this._scheduleKey;
            }
            set
            {
                this._scheduleKey = value;
            }
        }
        /// <summary>
        /// 排班计划名称。
        /// </summary>
        public string ScheduleName
        {
            get
            {
                return this._scheduleName;
            }
            set
            {
                this._scheduleName = value;
                ValidateDirtyList(CAL_SCHEDULE.FIELD_SCHEDULE_NAME, value);
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return this._descriptions;
            }
            set
            {
                this._descriptions = value;
                ValidateDirtyList(CAL_SCHEDULE.FIELD_DESCRIPTIONS,value);
            }
        }
        /// <summary>
        /// 最大跨度（不用）
        /// </summary>
        public string MaxOverLapTime
        {
            get
            {
                return this._overTime;
            }
            set
            {
                this._overTime = value;
                ValidateDirtyList(CAL_SCHEDULE.FIELD_MAXOVERLAPTIME, value);
            }
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        /// <summary>
        /// 存储每月排班计划的主键
        /// </summary>
        public string MKey
        {
            get;
            set;
        }
        /// <summary>
        /// 每天排班计划的集合。
        /// </summary>
        public List<ScheduleDay> ScheduleDayList
        {
            get
            {
                return this._scheduleDayList;
            }
            set
            {
                this._scheduleDayList = value;
            }
        }
        /// <summary>
        /// 数据是否有不一致。
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || ScheduleDayList.Count > 0);
            }
        }       
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Schedule()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public Schedule(string scheduleKey)
        {
            this._scheduleKey = scheduleKey;
        }
        #endregion

        #region Action
        /// <summary>
        /// 插入排班计划记录到数据库表中。
        /// </summary>
        /// <returns>是否插入成功，true：成功。false：失败。</returns>
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //准备插入到数据库中的数据。
            DataTable scheduleTable = CAL_SCHEDULE.CreateDataTable();
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                     {CAL_SCHEDULE.FIELD_SCHEDULE_KEY,_scheduleKey},
                                                     {CAL_SCHEDULE.FIELD_SCHEDULE_NAME,_scheduleName},
                                                     {CAL_SCHEDULE.FIELD_DESCRIPTIONS,_descriptions},
                                                     {CAL_SCHEDULE.FIELD_MAXOVERLAPTIME,_overTime.ToString()},
                                                     {CAL_SCHEDULE.FIELD_CREATOR,Creator},
                                                     {CAL_SCHEDULE.FIELD_CREATE_TIMEZONE,CreateTimeZone},
                                                     {CAL_SCHEDULE.FIELD_EDITOR,Editor},
                                                     {CAL_SCHEDULE.FIELD_EDIT_TIMEZONE,EditTimeZone}
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref scheduleTable, dataRow);
            //为远程调用函数准备参数。
            dataSet.Tables.Add(scheduleTable);
            try
            {
                DataSet dsReturn = null;
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //调用远程函数，新增一行排班计划记录。
                    dsReturn = serverFactory.CreateISchedule().AddSchedule(dataSet);
                    //获取远程函数的执行结果。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg != "")
                    {//执行不成功。
                        return false;
                    }
                    else
                    {
                        //重置脏数据集合。
                        this.ResetDirtyList();
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //注销远程调用信道。
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }
        /// <summary>
        /// 更新数据库表中的排班计划数据。
        /// </summary>
        /// <returns>是否更新成功。true：更新成功，false：更新失败。</returns>
        public override bool Update()
        {
            //集合中是否有待更新的数据，通过判断是否有不一致的数据来确定。
            if (IsDirty)
            {
                DataSet dataSet = new DataSet();
                if (this.DirtyList.Count > 0)
                {
                    //为更新创建数据表对象。
                    DataTable scheduleTable = DataTableHelper.CreateDataTableForUpdateBasicData(CAL_SCHEDULE.DATABASE_TABLE_NAME);
                    //遍历数据不一致的集合。
                    foreach (string Key in this.DirtyList.Keys)
                    {
                        //增加一行待更新的数据。
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _scheduleKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                    };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref scheduleTable, rowData);
                    }
                    //如果数据表中有待更新的数据，添加到数据集中。
                    if (scheduleTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(scheduleTable);
                    }
                }
                try
                {
                    DataSet dsReturn = null;
                    //创建远程调用的工厂对象。
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    if (null != serverFactory)
                    {
                        //调用远程函数，更新排班计划记录。
                        dsReturn = serverFactory.CreateISchedule().UpdateSchedule(dataSet);
                        //获取远程函数的执行结果。
                        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (_errorMsg != "")
                        {//执行不成功。
                            return false;
                        }
                        else
                        {
                            //重置脏数据集合。
                            this.ResetDirtyList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _errorMsg = ex.Message;
                }
                finally
                {
                    //注销远程调用信道。
                    CallRemotingService.UnregisterChannel();
                }
            }
            else
            {
                _errorMsg = "${res:FanHai.Hemera.Addins.FMM.Msg.UpdateError}";
            }
            return true;
        }
        /// <summary>
        /// 查询排班计划。根据排班计划名称查询。
        /// </summary>
        /// <returns>查询得到的排班计划数据集。</returns>
        public DataSet SearchSchedule()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //工厂对象创建成功。
                if (null != serverFactory)
                {
                    //调用远程函数，通过排班计划名称进行搜索查询。
                    dsReturn = serverFactory.CreateISchedule().SearchSchedule(_scheduleName);
                    //远程函数执行结果消息。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);                    
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //关闭远程调用信道。
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取班次信息计划，根据排班计划主键获取。
        /// </summary>
        /// <returns>班次信息集合。</returns>
        public DataSet GetShift()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //工厂对象创建成功。
                if (null != serverFactory)
                {
                    //调用远程函数，根据排班计划主键获取。
                    dsReturn = serverFactory.CreateISchedule().GetShift(_scheduleKey);
                    //远程函数执行结果消息。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //关闭远程调用的信道。
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 删除排班计划。
        /// </summary>
        /// <param name="dataSet">数据集对象。</param>
        public void DeleteSchedule(DataSet dataSet)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //工厂对象创建成功。
                if (null != serverFactory)
                {
                    //调用远程函数
                    dsReturn = serverFactory.CreateISchedule().DeleteSchedule(dataSet);
                    //远程函数执行结果消息。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //关闭远程调用的信道。
                CallRemotingService.UnregisterChannel();
            }           
        }
        /// <summary>
        /// 保存每天排班计划记录。
        /// </summary>
        /// <param name="dataTable">数据表对象。</param>
        public void SaveShiftOfSchedule(DataTable dataTable)
        {
            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dataTable);
            //每天排班计划集合不为空。
            if (ScheduleDayList.Count > 0)
            {
                DataTable shiftDayTable = CAL_SCHEDULE_DAY.CreateDataTable();
                //遍历每天排班计划集合中的记录。
                foreach (ScheduleDay scheduleDay in ScheduleDayList)
                {
                    Dictionary<string, string> rowData = new Dictionary<string, string>()
                                                {
                                                    {CAL_SCHEDULE_DAY.FIELD_DKEY,scheduleDay.DKey},
                                                    {CAL_SCHEDULE_DAY.FIELD_STARTTIME,scheduleDay.StartTime},
                                                    {CAL_SCHEDULE_DAY.FIELD_ENDTIME,scheduleDay.EndTime},
                                                    {CAL_SCHEDULE_DAY.FIELD_SHIFT_VALUE,scheduleDay.ShiftValue},
                                                    {CAL_SCHEDULE_DAY.FIELD_SEQNO,scheduleDay.SeqNo},
                                                    {CAL_SCHEDULE_DAY.FIELD_SHIFT_KEY,scheduleDay.ShiftKey},
                                                    {CAL_SCHEDULE_DAY.FIELD_DAY,scheduleDay.Day}
                                                };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref shiftDayTable, rowData);
                }
                dsReturn.Tables.Add(shiftDayTable);
            }
            try
            {
                //远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //工厂对象创建成功。
                if (null != serverFactory)
                {
                    //调用远程函数
                    dsReturn = serverFactory.CreateISchedule().SaveShiftOfSchedule(dsReturn);
                    //远程函数执行结果消息。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg == "")//保存每天排班计划数据成功。
                    {
                        this.ResetDirtyList();
                        //遍历日排班计划集合。
                        foreach (ScheduleDay scheduleDay in ScheduleDayList)
                        {
                            scheduleDay.OperationAction = OperationAction.Update;
                            scheduleDay.IsInitializeFinished = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //关闭远程调用信道。
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 更新每天排班计划数据。
        /// </summary>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public bool UpdateShiftOfSchedule()
        {
            DataSet dsReturn = new DataSet();
            if (IsDirty) //数据有不一致。
            {
                //每天排班计划的数据集合不为空。
                if (ScheduleDayList.Count > 0)
                {
                    DataTable scheduleDayTable = CAL_SCHEDULE_DAY.CreateDataTable();
                    DataColumn mKeyDataRow = new DataColumn(CAL_SCHEDULE_DAY.FIELD_MKEY);
                    DataColumn actionDataRow = new DataColumn(COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION);
                    scheduleDayTable.Columns.Add(mKeyDataRow);              //每月排班计划主键列。
                    scheduleDayTable.Columns.Add(actionDataRow);            //操作动作列。
                    foreach (ScheduleDay scheduleDay in ScheduleDayList)
                    {
                        //记录操作动作为None或或者Update，则不需要进行更新。
                        if (scheduleDay.OperationAction == OperationAction.None
                            || scheduleDay.OperationAction == OperationAction.Update)
                            continue;
                        else
                        {
                            //待更新的数据。添加到数据表中。
                            Dictionary<string, string> rowData = new Dictionary<string, string>()
                           {
                                {CAL_SCHEDULE_DAY.FIELD_DKEY,scheduleDay.DKey},
                                {CAL_SCHEDULE_DAY.FIELD_STARTTIME,scheduleDay.StartTime},
                                {CAL_SCHEDULE_DAY.FIELD_ENDTIME,scheduleDay.EndTime},
                                {CAL_SCHEDULE_DAY.FIELD_SHIFT_VALUE,scheduleDay.ShiftValue},
                                {CAL_SCHEDULE_DAY.FIELD_SEQNO,scheduleDay.SeqNo},
                                {CAL_SCHEDULE_DAY.FIELD_SHIFT_KEY,scheduleDay.ShiftKey},
                                {CAL_SCHEDULE_DAY.FIELD_DAY,scheduleDay.Day},
                                {CAL_SCHEDULE_DAY.FIELD_MKEY,MKey},
                                {COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION,Convert.ToString((int)scheduleDay.OperationAction)}
                           };
                           FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref scheduleDayTable, rowData);
                        }
                    }
                    //数据表中有待更新的记录。
                    if (scheduleDayTable.Rows.Count > 0)
                    {
                        dsReturn.Tables.Add(scheduleDayTable);
                    }
                }
                try
                {
                    //远程调用的工厂对象。
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    //工厂对象创建成功。
                    if (null != serverFactory)
                    {
                        if (dsReturn.Tables.Count > 0)
                        {
                            //更新每天排班计划数据。
                            dsReturn = serverFactory.CreateISchedule().UpdateShiftOfSchedule(dsReturn);
                            //远程函数执行结果消息。
                            _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        }
                        else
                        {                           
                            _errorMsg = "${res:Global.UpdateItemDataMessage}";
                        }
                        if (_errorMsg == "")//成功
                        {
                            this.ResetDirtyList();
                            //遍历日排班计划集合。
                            foreach (ScheduleDay scheduleDay in ScheduleDayList)
                            {
                                scheduleDay.OperationAction = OperationAction.Update;
                                scheduleDay.IsInitializeFinished = true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _errorMsg = ex.Message;
                }
                finally
                {
                    //关闭远程调用信道。
                    CallRemotingService.UnregisterChannel();
                }
            }           
            return true;
        }
        /// <summary>
        /// 删除每天的排班计划。根据月排班计划主键。调用该函数之前需要先设置<see cref="MKey"/>属性的值。
        /// </summary>
        public void DeleteShiftOfSchedule()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //工厂对象创建成功。
                if (null != serverFactory)
                {
                    //调用远程函数，根据月排班计划主键。
                    dsReturn = serverFactory.CreateISchedule().DeleteShiftOfSchedule(MKey);
                    //远程函数执行结果消息。
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                //关闭远程调用信道。
                CallRemotingService.UnregisterChannel();
            }
        }
        #endregion
    }
}

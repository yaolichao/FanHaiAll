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
//==================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 对每天的排班计划进行相关操作的实体类。
    /// </summary>
    public class ScheduleDay:EntityObject
    {
        #region define attribute
        private string _dKey = "";                                                                      //主键
        //private string _mKey = "";                                                                    
        private string _startTime = "";                                                                 //开始时间
        private string _endTime = "";                                                                   //结束时间
        private string _shiftValue = "";                                                                //班别值
        private string _seqNo = "";                                                                     //序号
        private string _shiftKey = "";                                                                  //班别主键
        private string _day = "";                                                                       //日期
        private string _errorMsg= "";                                                                   //错误消息
        private OperationAction _operationAction = OperationAction.None;        //操作动作
        #endregion

        #region Properties
        /// <summary>
        /// 主键。
        /// </summary>
        public string DKey
        {
            get { return _dKey; }
            set { _dKey = value; }
        }       
        /// <summary>
        /// 开始时间。
        /// </summary>
        public string StartTime
        {
            get { return _startTime; }
            set 
            {
                _startTime = value;
                //ValidateDirtyList(CAL_SCHEDULE_DAY.FIELD_STARTTIME,_startTime);
            }
        }
        /// <summary>
        /// 结束时间。
        /// </summary>
        public string EndTime
        {
            get { return _endTime; }
            set
            { 
                _endTime = value;
                //ValidateDirtyList(CAL_SCHEDULE_DAY.FIELD_ENDTIME,_endTime);
            }
        }
        /// <summary>
        /// 班别值。
        /// </summary>
        public string ShiftValue
        {
            get { return _shiftValue; }
            set 
            { 
                _shiftValue = value;
                ValidateDirtyList(CAL_SCHEDULE_DAY.FIELD_SHIFT_VALUE,_shiftValue);
            }
        }
        /// <summary>
        /// 序号。
        /// </summary>
        public string SeqNo
        {
            get { return _seqNo; }
            set { _seqNo = value; }
        }
        /// <summary>
        /// 班别主键。
        /// </summary>
        public string ShiftKey
        {
            get { return _shiftKey; }
            set { _shiftKey = value; }
        }
        /// <summary>
        /// 日期。
        /// </summary>
        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }
        /// <summary>
        /// 错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        /// <summary>
        /// 操作动作。
        /// </summary>
        public OperationAction OperationAction
        {
            get
            {
                return _operationAction;
            }
            set
            {
                _operationAction = value;
            }
        }
        #endregion

    }
}

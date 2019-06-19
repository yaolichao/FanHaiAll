using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 排班计划班次数据管理接口。
    /// </summary>
    public interface IShift
    {
        /// <summary>
        /// 根据自定义属性类别名获取自定义属性值数据。
        /// </summary>
        /// <param name="tableName">自定义属性类别名。</param>
        /// <returns>包含自定义属性值数据的数据集对象。</returns>
        DataSet GetLookUpEditData(string tableName);
        /// <summary>
        /// 新增排班计划班次数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划班次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddShift(DataSet dsParams);
        /// <summary>
        /// 更新排班计划班次数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划班次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateShift(DataSet dsParams);
        /// <summary>
        /// 根据班次主键获取班次数据。
        /// </summary>
        /// <param name="shiftKey">班次主键。</param>
        /// <returns>包含班次数据的数据集对象。</returns>
        DataSet GetShift(string shiftKey);
        /// <summary>
        /// 删除排班计划班次数据。
        /// </summary>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <param name="shiftKey">班次主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteShift(string scheduleKey,string shiftKey);
        /// <summary>
        /// 判别班别是否存在。返回班别对应的主键标识符。当前时间对应的排班班别是否是指定的班别值。必须先进行排班。
        /// </summary>
        /// <param name="shiftValue">班别值。</param>
        /// <returns>返回班别主键的标识字符串。空字符串代表班别不存在</returns>
        string IsExistsShift(string shiftValue);
        /// <summary>
        /// 根据当前日期获取班别名称。
        /// </summary>
        /// <returns>班别名称。</returns>
        string GetShiftNameBySysdate();
    }
    /// <summary>
    /// 排班计划数据的管理接口。
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// 添加排班计划记录。
        /// </summary>
        /// <param name="dataSet">包含排班计划数据表的数据集对象。
        /// 排班计划数据表的表名必须为<see cref="CAL_SCHEDULE.DATABASE_TABLE_NAME"/>。
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddSchedule(DataSet dataSet);
        /// <summary>
        /// 查询排班计划信息。
        /// </summary>
        /// <param name="scheduleName">排班计划名称。</param>
        /// <returns>包含排班计划信息的数据集对象。</returns>
        DataSet SearchSchedule(string scheduleName);
        /// <summary>
        /// 更新排班计划数据。
        /// </summary>
        /// <param name="dsParams">包含排班计划数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateSchedule(DataSet dataSet);
        /// <summary>
        /// 根据排班计划主键获取班次数据。
        /// </summary>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <returns>包含班次数据的数据集对象。</returns>
        DataSet GetShift(string scheduleKey);
        /// <summary>
        /// 删除排班计划数据。
        /// </summary>
        /// <param name="dsParams">包含排班主键的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteSchedule(DataSet dataSet);
        /// <summary>
        /// 保存具体月份和天的排班数据。
        /// </summary>
        /// <param name="dsParams">包含具体月份和天的排班数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveShiftOfSchedule(DataSet dataSet);
        /// <summary>
        /// 获取排班计划主键。
        /// </summary>
        /// <param name="year">年份。</param>
        /// <param name="month">月份。</param>
        /// <returns>排班计划主键。</returns>
        string GetScheduleKey(string year,string month);
        /// <summary>
        /// 根据年份月份和排班计划主键获取排班数据。
        /// </summary>
        /// <param name="year">年份。</param>
        /// <param name="month">月份。</param>
        /// <param name="scheduleKey">排班计划主键。</param>
        /// <returns>包含排班数据的数据集对象。</returns>
        DataSet GetShiftOfSchedule(string year,string month,string scheduleKey);
        /// <summary>
        /// 更新具体月份和天的排班数据。
        /// </summary>
        /// <param name="dsParams">包含具体月份和天的排班数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateShiftOfSchedule(DataSet dataSet);
        /// <summary>
        /// 删除指定月份的排班数据。
        /// </summary>
        /// <param name="monthKey">排班月份主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteShiftOfSchedule(string monthKey);
        
    }
}

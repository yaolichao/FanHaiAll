using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IRptCommonEngine
    {
        /// <summary>
        /// 获得计划输入量
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetRptPlanAimData(Hashtable hstable);
        /// <summary>
        /// 保存计划数据
        /// </summary>
        /// <param name="dsPowerSet"></param>
        /// <returns></returns>
        DataSet SaveRptPlanAimData(DataSet dsPowerSet);
        /// <summary>
        /// 获得计划数据查询条件
        /// </summary>
        /// <returns></returns>
        DataSet GetProWoModuleType();
        /// <summary>
        /// 获得工序排班数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetOptSettingData(Hashtable hstable);
        /// <summary>
        /// 保存工序排班数据
        /// </summary>
        /// <param name="dsOptSetting"></param>
        /// <returns></returns>
        DataSet SaveOptSettingData(DataSet dsOptSetting);
        /// <summary>
        /// 获得班别数据
        /// </summary>
        /// <returns></returns>
        DataSet GetShiftName();
        /// <summary>
        /// 获得所有工序数据
        /// </summary>
        /// <returns></returns>
        DataSet GetOperation();
        /// <summary>
        /// 获得厂别数据
        /// </summary>
        /// <returns></returns>
        DataSet GetFactoryDate();
        /// <summary>
        /// 获取生产排班数据
        /// </summary>
        /// <returns></returns>
        DataSet GetFactoryShiftData(string sFactoryShiftSetKey, string sFactoryKey, string sDate, string sShiftValue, string sFactoryShiftName);
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        DataSet UpdateData(string sql, string sUpFuntionName);
        /// <summary>
        /// 获取线别
        /// </summary>
        /// <returns></returns>
        DataSet GetLines();
    }
}

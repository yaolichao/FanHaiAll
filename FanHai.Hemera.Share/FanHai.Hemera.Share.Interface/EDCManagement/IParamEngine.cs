#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
#endregion

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 采集参数的数据管理接口。
    /// </summary>
    public interface IParamEngine
    {
        /// <summary>
        /// 新增采集参数。
        /// </summary>
        /// <param name="dataSet">包含采集参数的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet ParamInsert(DataSet dataSet);
        /// <summary>
        /// 删除采集参数。
        /// </summary>
        /// <param name="paramKey">采集参数主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteParam(string paramKey);
        /// <summary>
        /// 查询采集参数。
        /// </summary>
        /// <param name="dataset">
        /// 包含查询条件的数据集对象。
        /// ----------------------------------
        /// {PARAM_NAME}
        /// ----------------------------------
        /// </param>
        /// <returns>包含采集参数的数据集对象。</returns>
        DataSet SearchParam(DataSet dataset);
        /// <summary>
        /// 根据采集参数主键获取采集参数及其对应的计算数据
        /// </summary>
        /// <param name="paramKey">采集参数主键。</param>
        /// <returns>包含采集参数及其对应的计算数据的数据集对象。</returns>
        DataSet GetParamByKey(string paramKey);
        /// <summary>
        /// 根据采集参数主键获取用于计算该参数的采集参数。
        /// </summary>
        /// <param name="paramKey">被计算的采集参数主键。 </param>
        /// <returns>
        /// 包含参与计算的采集参数的数据集对象。。
        /// 【ROW_KEY,DERIVATION_KEY,PARAM_KEY,PARAM_NAME】
        /// </returns>
        DataSet GetParamDerivationByKey(string paramKey);
        /// <summary>
        /// 获取不重复的采集参数名称。
        /// </summary>
        /// <returns>包含采集参数名称的数据集对象。</returns>
        DataSet GetDistinctParamName();
        /// <summary>
        /// 更新采集参数及其对应的计算数据。
        /// </summary>
        /// <param name="dsParams">包含采集参数及其对应的计算数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet ParamUpdate(DataSet dataSet);
        /// <summary>
        /// 获得基础参数设定的过站采集信息
        /// </summary>
        /// <returns></returns>
        DataSet GetBaseParamsByCategory();
    }
}

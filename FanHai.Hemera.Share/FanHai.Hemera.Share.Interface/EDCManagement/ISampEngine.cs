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
    /// 抽检规则和异常规则的数据管理接口。
    /// </summary>
    public interface ISampEngine
    {
        /// <summary>
        /// 新增抽检规则。
        /// </summary>
        /// <param name="dsParams">包含抽检规则的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddSamp(DataSet dsParams);
        /// <summary>
        /// 删除抽检规则。
        /// </summary>
        /// <param name="spKey">抽检见规则主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteSamp(string spKey);
        /// <summary>
        /// 查询抽检规则。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------
        /// {SP_NAME}
        /// -------------------------------
        /// </param>
        /// <returns>包含抽检规则的数据集对象。</returns>
        DataSet SearchSamp(DataSet dsParams);
        /// <summary>
        /// 根据抽检规则主键获取抽检规则。
        /// </summary>
        /// <param name="spKey">抽检规则主键。</param>
        /// <returns>包含抽检规则的数据集对象。</returns>
        DataSet GetSampByKey(string spKey);
        /// <summary>
        /// 获取不重复的抽检规则名称。
        /// </summary>
        /// <returns>包含抽检规则名称的数据集对象。</returns>
        DataSet GetDistinctSampName();

        /// <summary>
        /// 更新抽检规则数据。
        /// </summary>
        /// <param name="dsParams">包含抽检规则的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateSamp(DataSet dsParams);
        //-------------------------------------------------------------------
        // 异常规则
        //-------------------------------------------------------------------
        /// <summary>
        /// 获取异常规则数据。
        /// </summary>
        /// <returns>包含异常规则数据的数据集对象。</returns>
        DataSet GetAbnormalRule();
        /// <summary>
        /// 新增/更新异常规则。
        /// </summary>
        /// <param name="dsAbnormalRule">包含异常规则数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveAbnormalRule(DataSet dsAbnormalRule);
        /// <summary>
        /// 判断异常规则代码是否存在。
        /// </summary>
        /// <param name="strCode">
        /// 异常规则代码数组。
        /// {ARULECODE(必须),INSERT/UPDATE(必须),ABNORMALID(UPDATE时必须)}
        /// -------------------------------
        /// ARULECODE       ：异常规则代码。
        /// INSERT/UPDATE   ：判断类型。INSERT：是否存在指定的异常规则代码。,UPDATE：除指定规则主键外是否还存在指定的异常规则代码。
        /// ABNORMALID      ：异常规则主键。
        /// -------------------------------
        /// </param>
        /// <returns>
        /// 包含执行结果的数据集对象。
        /// 扩展属性Code_Counts表示存在的异常规则代码个数。
        /// </returns>
        DataSet IsExistAbnormalCode(string[] strCode);
        /// <summary>
        /// 根据异常规则主键获得异常规则明细数据。包括异常规则的颜色值
        /// </summary>
        /// <param name="abnormalIds">异常规则主键,格式如下：'id1','id2'...</param>
        /// <returns>包含异常规则明细数据的数据集对象。</returns>
        DataSet GetAbnormalDetailRule(string abnormalIds);
    }
}

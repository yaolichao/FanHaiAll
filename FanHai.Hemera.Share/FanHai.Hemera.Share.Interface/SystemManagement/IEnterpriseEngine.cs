using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 工艺流程组数据管理接口。
    /// </summary>
    public interface IEnterpriseEngine
    {
        /// <summary>
        /// 新增工艺流程组。
        /// </summary>
        /// <param name="dsParams">包含工艺流程组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。/returns>
        DataSet EnterpriseInsert(DataSet dataSet);
        /// <summary>
        /// 删除工艺流程组。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet EnterpriseDelete(string enterpriseKey);
        /// <summary>
        /// 查询工艺流程组。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet EnterpriseSearch(DataSet dataset);
        /// <summary>
        /// 根据工艺流程组主键获取工艺流程组数据。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <returns>包含工艺流程组数据的数据集对象。</returns>
        DataSet GetEnterpriseByKey(string enterpriseKey);
        /// <summary>
        /// 更新工艺流程组数据。
        /// </summary>
        /// <param name="dsParams">包含工艺流程组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet EnterpriseUpdate(DataSet dataSet);
        /// <summary>
        /// 获取不重复的工艺流程组名称。
        /// </summary>
        /// <returns>包含工艺流程组名称的数据集对象。</returns>
        DataSet GetDistinctEnterpriseName();
        /// <summary>
        /// 获取工艺流程组信息。
        /// </summary>
        /// <returns>包含最高版本工艺流程组数据的数据集对象。</returns>
        DataSet GetHelpInfoForEnterprise();
        /// <summary>
        /// 根据车间名称获取工艺流程组信息。
        /// </summary>
        /// <param name="roomName">车间名称。</param>
        /// <returns>包含最高版本工艺流程数据的数据集对象。</returns>
        DataSet GetHelpInfoForEnterprise(string roomName);
        /// <summary>
        /// 获取指定工艺流程组内的工艺流程信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含工艺流程组主键的数据集对象。
        /// ------------------------------
        /// {EnterpriseKey}
        /// ------------------------------
        /// </param>
        /// <returns>包含最高版本工艺流程数据的数据集对象。</returns>
        DataSet GetHelpInfoForEnterpriserRout(DataSet dsParams);
        /// <summary>
        /// 获取指定工艺流程内的工步信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含工艺流程主键的数据集对象。
        /// ------------------------------
        /// {RouteKey}
        /// ------------------------------
        /// </param>
        /// <returns>包含最高版本工艺流程内工步数据的数据集对象。</returns>
        DataSet GetHelpInfoForEnterpriserRoutStep(DataSet dsParams);
         /// <summary>
        /// 获取当前工艺流程工步的下一工步。
        /// </summary>
        /// <remarks>
        /// 如果当前工步在当前工艺流程是最后一个工步但所属工艺流程组有下一工艺流程，则下一工步为下一工艺流程的第一个工步。
        /// </remarks>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <param name="routeKey">ROUTE_ROUTE_VER_KEY（工艺流程主键）</param>
        /// <param name="stepKey">ROUTE_STEP_KEY（工步主键）。</param>
        /// <returns>包含下一工步数据的数据集对象。</returns>
        DataSet GetEnterpriseNextRouteAndStep(string enterpriseKey,string routeKey,string stepKey);
                /// <summary>
        /// 获取当前工艺流程工步的前一工步。
        /// </summary>
        /// <param name="enterpriseKey">工艺流程组主键。</param>
        /// <param name="routeKey">ROUTE_ROUTE_VER_KEY（工艺流程主键）</param>
        /// <param name="stepKey">ROUTE_STEP_KEY（工步主键）。</param>
        /// <returns>包含前一工步数据的数据集对象。</returns>
        DataSet GetEnterprisePreviousRouteAndStep(string enterpriseKey, string routeKey, string stepKey);
        /// <summary>
        /// 获取工艺流程信息。
        /// </summary>
        /// <returns>包含工艺流程信息的数据集。</returns>
        DataSet GetProcessPlan();
        /// <summary>
        /// 根据工厂车间名称和产品类型获取工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程信息的数据集。</returns>
        DataSet GetProcessPlan(string factoryName, string productType,bool isRework);
        /// <summary>
        /// 根据工厂车间名称和产品类型获取首工序的工艺流程信息。
        /// </summary>
        /// <param name="factoryName">车间名称。</param>
        /// <param name="productType">成品类型。</param>
        /// <param name="isRework">重工标记.true:重工。false:正常。</param>
        /// <returns>包含工艺流程首工序信息的数据集。</returns>
        DataSet GetProcessPlanFirstOperation(string factoryName, string productType, bool isRework);
    }
}

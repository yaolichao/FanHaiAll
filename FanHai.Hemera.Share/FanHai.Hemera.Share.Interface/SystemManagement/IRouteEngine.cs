using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 工艺流程数据管理接口。
    /// </summary>
    public interface IRouteEngine
    {
        /// <summary>
        /// 新增工艺流程。
        /// </summary>
        /// <param name="dsParams">包含工艺流程数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet RouteInsert(DataSet dataSet);
        /// <summary>
        /// 删除工艺流程。
        /// </summary>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet RouteDelete(string routeKey);
        /// <summary>
        /// 查询工艺流程。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工艺流程数据的数据集对象。</returns>
        DataSet RouteSearch(DataSet dataset);
        /// <summary>
        /// 获取不重复的工艺流程名。
        /// </summary>
        /// <returns>包含工艺流程名的数据集对象。</returns>
        DataSet GetRouteName();
        /// <summary>
        /// 根据工艺流程主键获取工艺流程。
        /// </summary>
        /// <param name="routeKey">工艺流程主键。</param>
        /// <returns>包含工艺流程的数据集对象。</returns>
        DataSet GetRouteByKey(string routeKey);

        /// <summary>
        /// 获取版本号最大的工艺流程。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------
        /// {route name}
        /// -----------------------
        /// </param>
        /// <returns>包含工艺流程的数据集对象。</returns>
        DataSet GetMaxVerRoute(DataSet dataset);
        /// <summary>
        /// 更新工艺流程。
        /// </summary>
        /// <param name="dsParams">包含工艺流程的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet RouteUpdate(DataSet dataSet);
        /// <summary>
        /// 添加工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">包含工艺流程和生产线关联的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddRouteLine(DataSet dataset);
        /// <summary>
        /// 删除工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------
        /// {ROUTE_ROUTE_VER_KEY}
        /// {PRODUCTION_LINE_KEY}
        /// ------------------------------
        /// </param>
        /// <returns>DataSet</returns>
        DataSet DeleteRouteLine(DataSet dataset);
        /// <summary>
        ///  查询工艺流程和生产线的关联关系。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象，可以为null。
        /// ------------------------------
        /// {ROUTE_ROUTE_VER_KEY}
        /// {PRODUCTION_LINE_KEY}
        /// ------------------------------
        /// </param>
        /// <returns>包含工艺流程和生产线的关联关系的数据集对象。</returns>
        DataSet SearchRouteLine(DataSet dataset);
        /// <summary>
        /// 获取已激活的工艺流程数据。
        /// </summary>
        /// <returns>
        /// 包含已激活的工艺流程数据的数据集对象。
        /// [ROUTE_ROUTE_VER_KEY,ROUTE_NAME,DESCRIPTION]
        /// </returns>
        DataSet GetActivedRouteData();
        /// <summary>
        /// 根据工艺流程主键获取工步数据。
        /// </summary>
        /// <param name="routeKey">工艺流程主键</param>
        /// <returns>
        /// 包含工步数据的数据集对象。
        /// [ROUTE_STEP_KEY,ROUTE_STEP_NAME,ROUTE_STEP_SEQ,ROUTE_OPERATION_VER_KEY]
        /// </returns>
        DataSet GetRouteStepDataByRouteKey(string routeKey);
        /// <summary>
        /// 根据工步主键获取工步数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步数据的数据集对象。</returns>
        DataSet GetStepDataByKey(string stepKey);
        /// <summary>
        /// 根据工步主键获取工步数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dctype">数据采集类型(0：进站时采集 1：出站时采集)。</param>
        /// <returns>包含工步数据的数据集对象。</returns>
        DataSet GetStepBaseDataAndParamDataByKey(string stepKey, int dctype);
        /// <summary>
        /// 根据工步主键获取工步自定义数据。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含工步自定义数据的数据集对象。</returns>
        DataSet GetStepUda(string stepKey);
        /// <summary>
        /// 获取工步指定的自定义属性值。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="attributeName">自定义属性名。</param>
        /// <returns>自定义属性名对应的属性值。</returns>
        string GetStepUdaValue(string stepKey, string attributeName);
    }
}

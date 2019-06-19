using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 工序数据管理接口。
    /// </summary>
    public interface IOperationEngine
    {
        /// <summary>
        /// 新增工序。
        /// </summary>
        /// <param name="dsParams">包含工序数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet OperationInsert(DataSet dataSet);
        /// <summary>
        /// 删除工序数据。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet OperationDelete(string operationKey);
        /// <summary>
        /// 更新工序数据。
        /// </summary>
        /// <param name="dsParams">包含工序数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet OperationUpdate(DataSet dsParams);
        /// <summary>
        /// 查询工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        DataSet OperationSearch(DataSet dataset);
        /// <summary>
        /// 查询工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        DataSet GetOperations(DataSet reqDS, string OperationName, int pageNo, int pageSize, out int pages, out int records);
        /// <summary>
        /// 根据工序主键获取工序数据。
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        DataSet GetOperationByKey(string operationKey);
        /// <summary>
        /// 获取版本号最大的工序数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含工序数据的数据集对象。</returns>
        DataSet GetMaxVerOperation(DataSet dsParams);
        /// <summary>
        /// 获取版本号最大的工序基本数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序基本数据的数据集对象。</returns>
        DataSet GetMaxVersionOperationBaseInfo(string operationName);
        /// <summary>
        /// 获取版本号最大的工序自定义属性数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <param name="attrName">自定义属性名称,如果为空则查询所有自定义属性数据。</param>
        /// <returns>包含工序自定义数据的数据集对象。</returns>
        DataSet GetMaxVersionOperationAttrInfo(string operationName, string attrName);
        /// <summary>
        /// 获取版本号最大的工序基础数据和参数数据。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>包含工序参数数据的数据集对象。</returns>
        DataSet GetOperationBaseAndParamInfo(string operationName);
        /// <summary>
        /// 获取已激活且不重复的工序名称。
        /// </summary>
        /// <returns>
        /// 包含工序名称的数据集对象。
        /// [ROUTE_OPERATION_NAME]
        /// </returns>
        DataSet GetDistinctOperationNameList();

    }
}

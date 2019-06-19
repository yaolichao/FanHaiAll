using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 系统操作组数据管理接口。
    /// </summary>
    public interface IOperationGroupEngine
    {
        /// <summary>
        /// 添加系统操作组。
        /// </summary>
        /// <param name="dsParams">包含系统操作组的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddOperationGroup(DataSet dataset);

        /// <summary>
        /// 删除系统操作组。
        /// </summary>
        /// <param name="resourceGroupKey">系统操作组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteOperationGroup(string operationGroupKey);

        /// <summary>
        /// 获取系统操作组。
        /// </summary>
        /// <returns>包含系统操作组的数据集对象。</returns>
        DataSet GetOperationGroup();
    }
}

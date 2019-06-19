using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 系统操作数据管理接口。
    /// </summary>
    public interface IRBACOperationEngine
    {
        /// <summary>
        /// 添加系统操作。
        /// </summary>
        /// <param name="dsParams">包含系统操作数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddOperation(DataSet dataset);

        /// <summary>
        /// 更新系统操作。
        /// </summary>
        /// <param name="dsParams">包含系统操作数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateOperation(DataSet dataSet);

        /// <summary>
        /// 删除系统操作。
        /// </summary>
        /// <param name="operationKey">系统操作主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteOperation(string operationKey);

        /// <summary>
        /// 获取系统操作数据。
        /// </summary>
        /// <param name="operationKey">系统操作主键。</param>
        /// <returns>包含系统操作数据的数据集对象。</returns>
        DataSet GetOperation(string operationGroupKey);
    }
}

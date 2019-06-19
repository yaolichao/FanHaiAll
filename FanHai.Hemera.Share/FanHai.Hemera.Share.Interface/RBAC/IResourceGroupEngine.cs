using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 资源组数据管理接口。
    /// </summary>
    public interface IResourceGroupEngine
    {
        /// <summary>
        /// 添加资源组。
        /// </summary>
        /// <param name="dsParams">包含资源组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddResourceGroup(DataSet dsParams);
        /// <summary>
        /// 更新资源组。
        /// </summary>
        /// <param name="dsParams">包含资源组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateResourceGroup(DataSet dsParams);
        /// <summary>
        /// 删除资源组数据。
        /// </summary>
        /// <param name="resourceGroupKey">资源组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteResourceGroup(string resourceGroupKey);
        /// <summary>
        /// 获取资源组数据。
        /// </summary>
        /// <param name="resourceGroupKey">资源组主键。</param>
        /// <returns>包含资源组数据的数据集对象。</returns>
        DataSet GetResourceGroup(string resourceGroupKey);
    }
}

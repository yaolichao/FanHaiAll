using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 资源数据管理接口。
    /// </summary>
    public interface IResourceEngine
    {
        /// <summary>
        /// 添加系统资源。
        /// </summary>
        /// <param name="dsParams">包含系统资源的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddResource(DataSet dsParams);
        /// <summary>
        /// 更新资源数据。
        /// </summary>
        /// <param name="resourceKey">包含资源数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateResource(DataSet dsParams);
        /// <summary>
        /// 删除资源数据。
        /// </summary>
        /// <param name="resourceKey">资源主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteResource(string resourceKey);
        /// <summary>
        /// 获取资源数据。
        /// </summary>
        /// <param name="resourceKey">资源主键。</param>
        /// <returns>包含资源主键的数据集对象。</returns>
        DataSet GetResource(string resourceGroupKey);
    }
}

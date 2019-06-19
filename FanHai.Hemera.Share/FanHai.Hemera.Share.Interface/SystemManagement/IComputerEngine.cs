using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 客户端及其配置数据管理接口。
    /// </summary>
    public interface IComputerEngine
    {
        /// <summary>
        /// 新增客户端名称和相应的配置属性。
        /// </summary>
        /// <param name="dsParams">包含客户端名称和相应配置属性数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddComputer(DataSet dsParams);
        /// <summary>
        /// 根据客户端名称获取客户端及其配置数据。
        /// </summary>
        /// <param name="computerName">客户端名称。</param>
        /// <returns>包含客户端及其配置数据的数据集对象。</returns>
        DataSet GetComputerByName(string computerName);
        /// <summary>
        /// 查询客户端数据。
        /// </summary>
        /// <param name="dsParams">包含查询参数的数据集对象。</param>
        /// <returns>包含客户端数据的数据集对象。</returns>
        DataSet SearchComputers(DataSet dsParams);
        /// <summary>
        /// 更新客户端及其配置数据。
        /// </summary>
        /// <param name="dsParams">包含客户端及其配置数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateComputer(DataSet dsParams);
        /// <summary>
        /// 删除客户端及其配置数据。
        /// </summary>
        /// <param name="computerKey">客户端主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteComputer(string computerKey);
    }
}

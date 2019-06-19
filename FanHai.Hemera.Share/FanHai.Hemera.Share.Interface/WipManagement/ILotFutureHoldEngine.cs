using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 批次预设暂停数据接口。
    /// </summary>
    public interface ILotFutureHoldEngine
    {
        /// <summary>
        /// 根据主键获取预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        DataSet Get(string key);
        /// <summary>
        /// 批量新增预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待预设暂停批次的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet Insert(DataSet dsParams);
        /// <summary>
        /// 更新预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含待更新的预设暂停批次的数据集。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet Update(DataSet dsParams);
        /// <summary>
        /// 根据主键删除预设暂停数据。
        /// </summary>
        /// <param name="key">预设暂停主键。</param>
        /// <param name="deletor">删除人。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet Delete(string key,string deletor);
        /// <summary>
        /// 分页查询预设暂停数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="config">数据分页配置对象。</param>
        /// <returns>包含预设暂停数据的数据集对象。</returns>
        DataSet Query(DataSet dsParams, ref PagingQueryConfig config);
    }
}

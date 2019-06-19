using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 托盘信息查询操作接口。
    /// </summary>
    public interface IPalletQueryEngine
    {
        /// <summary>
        /// 查询包含托盘信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        DataSet SearchPalletList(DataSet dsParams);
        /// <summary>
        /// 查询包含托盘信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="config">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        DataSet SearchPalletList(DataSet dsParams, ref PagingQueryConfig config);
        /// <summary>
        /// 查询包含托盘信息及其组件序列号的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息及其组件序列号的数据集。。</returns>
        DataSet SearchPalletDetailList(DataSet dsSearch, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 查询包含托盘信息及其组件序列号的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含托盘信息及其组件序列号的数据集。</returns>
        DataSet SearchPalletDetailList(DataSet dsSearch);
    }
}

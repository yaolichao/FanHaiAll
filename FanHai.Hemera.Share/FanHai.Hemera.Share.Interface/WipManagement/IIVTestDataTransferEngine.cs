using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// IV测试数据转置接口。
    /// </summary>
    public interface IIVTestDataTransferEngine
    {
        /// <summary>
        /// 根据设备代码获取最大的IV测试时间。
        /// </summary>
        /// <param name="deviceNo">设备代码。</param>
        /// <returns>指定设备最大的IV测试时间。</returns>
        DateTime GetMaxIVTestTime(string deviceNo);
        /// <summary>
        /// 新增IV测试数据。
        /// </summary>
        /// <param name="dsParams">包含IV测试数据的数据集对象。</param>
        /// <returns>新增IV测试数据的个数。</returns>
        int InsertIVTestData(DataSet dsParams);
    }
}

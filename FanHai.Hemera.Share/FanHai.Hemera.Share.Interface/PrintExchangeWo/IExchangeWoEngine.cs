using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 转/返工单操作接口。
    /// </summary>
    public interface IExchangeWoEngine
    {
        /// <summary>
        /// 获取转工单主数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetExchangeWoData(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hstable);
        /// <summary>
        /// 根据查询条件获得校验转工单数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetExchangeByFilter(Hashtable hstable);
        /// <summary>
        /// 保存转工单作业
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        DataSet SaveExchangeWo(DataSet dsSave);
    }
}

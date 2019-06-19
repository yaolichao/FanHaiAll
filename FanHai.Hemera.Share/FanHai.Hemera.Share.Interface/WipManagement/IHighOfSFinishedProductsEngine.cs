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
    public interface IHighOfSFinishedProductsEngine
    {
        /// <summary>
        /// 查询包含高位检的批次数据信息。
        /// </summary>
        /// </param>
        /// <returns>包含高位检的信息的数据集。</returns>
        DataSet GetHighInfByLotNum(string strLotNum);

        /// <summary>
        /// 插入高位检数据信息
        /// </summary>
        /// </param>
        /// <returns>包含高位检的信息的数据集。</returns>
        DataSet InsertIntoGWJ(DataTable dtData,string lotNumber);
    }
}

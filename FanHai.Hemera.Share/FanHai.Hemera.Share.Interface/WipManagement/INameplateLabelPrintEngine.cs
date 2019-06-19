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
    public interface INameplateLabelPrintEngine
    {
        /// <summary>
        /// 查询包含托盘号的字符串。
        /// </summary>
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        DataSet GetMatrialByWorkOrderNumber(string orderNumber);

        DataSet GetPowerByWOnumberAndPartID(string _orderNumner, string _partName);

        DataSet GetInfByWOnumberAndPartIDAndPower(string _orderNumner, string _partName, string _power);

        DataSet GetInfByWOnumberAndPartID(string _orderNumner, string _partName);
        /// <summary>
        /// 根据工单号获取电池类型如156M-N,156M-P,通过是否包含-N和-P区别是否为背钝化
        /// </summary>
        /// <param name="workOrder">工单号</param>
        /// <returns></returns>
        DataSet GetCellTypeByWorkOrderNumber(string workOrder);
    }
}

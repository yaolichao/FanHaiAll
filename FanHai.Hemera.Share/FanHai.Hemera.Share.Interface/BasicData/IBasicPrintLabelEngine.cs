using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 标签或者铭牌数据操作接口
    /// </summary>
    public interface IBasicPrintLabelEngine
    {
        /// <summary>
        /// 获取标签或铭牌数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据的数据集对象。</returns>
        DataSet GetPrintLabelData();
        /// <summary>
        /// 保存标签或铭牌数据。
        /// </summary>
        /// <param name="dsParams">包含标签或铭牌数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SavePrintLabelData(DataSet dsParams);
        /// <summary>
        /// 是否允许设置标签或数据为无效数据。
        /// </summary>
        /// <param name="labelId">标签或铭牌ID</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet IsAllowInvalid(string labelId);
        /// <summary>
        /// 获取标签或铭牌明细数据。
        /// </summary>
        /// <returns>包含标签或铭牌明细数据的数据集对象。</returns>
        DataSet GetPrintLabelDetailData(string labelId);
        /// <summary>
        /// 获取产品型号数据。
        /// </summary>
        /// <returns>包含产品型号数据的数据集对象。</returns>
        DataSet GetProductModelData();
        /// <summary>
        /// 获取认证类型数据。
        /// </summary>
        /// <returns>包含认证类型数据的数据集对象。</returns>
        DataSet GetCertificateTypeData();
        /// <summary>
        /// 获取分档方式数据。
        /// </summary>
        /// <returns>包含分档方式数据的数据集对象。</returns>
        DataSet GetPowersetTypeData();
        /// <summary>
        /// 获取标签或铭牌数据类型数据。
        /// </summary>
        /// <returns>包含标签或铭牌数据类型数据的数据集对象。</returns>
        DataSet GetLabelDataTypeData();
        /// <summary>
        /// 获取打印机类型数据。
        /// </summary>
        /// <returns>包含打印机类型数据的数据集对象。</returns>
        DataSet GetPrinterTypeData();
        /// <summary>
        /// 获取铭牌检验类型数据。
        /// </summary>
        /// <returns>包含获取铭牌检验类型数据集对象。</returns>
        DataSet GetCustCheckTypeData();
        
    }
}

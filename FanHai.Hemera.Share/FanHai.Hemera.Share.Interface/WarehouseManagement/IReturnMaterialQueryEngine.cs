using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.WarehouseManagement
{
    /// <summary>
    /// 批次退料数据查询接口。
    /// </summary>
    public interface IReturnMaterialQueryEngine
    {
        /// <summary>
        /// 获取批次退料信息。
        /// </summary>
        /// <returns>包含批次退料信息的数据集对象。</returns>
        DataSet GetReturnMaterial();
        /// <summary>
        /// 获取批次退料信息。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集对象。
        /// ---------------------------
        /// {START_TIME}
        /// {END_TIME}
        /// ---------------------------
        /// </param>
        /// <returns>包含批次退料信息的数据集对象。</returns>
        DataSet GetReturnMaterial(DataSet dsSearch);
    }
}

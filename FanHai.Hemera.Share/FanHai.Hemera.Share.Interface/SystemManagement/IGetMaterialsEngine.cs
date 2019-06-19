using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 物料数据相关数据操作接口。
    /// </summary>
    /// <remarks>
    /// 包含添加、查询、更新、删除物料，检查工单是否存在等方法。
    /// </remarks>
    public interface IGetMaterialsEngine
    {
        /// <summary>
        /// 向工单表中插入工单信息，同时向配料单表中插入配料信息
        /// </summary>
        /// <param name="dsParams">POR_WORK_ORDER和POR_WORK_ORDER_REC数据表和数据</param>
        /// <returns>返回含有操作信息的结果集</returns>
        DataSet AddMaterialInformation(DataSet dsParams);
        /// <summary>
        /// 查询物料信息。
        /// </summary>
        /// <param name="workOrderNumber">工单号。</param>
        /// <param name="recNumber">配料单号。</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        DataSet QueryMaterialInformation(string workOrderNumber, string recNumber);
        /// <summary>
        /// 更新物料信息。
        /// </summary>
        /// <param name="dsParams">包含物料信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateMaterialInformation(DataSet dsParams);
        /// <summary>
        /// 删除物料信息。
        /// </summary>
        /// <param name="dsParams">包含物料信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteMaterialInformation(DataSet dsParams);
        /// <summary>
        /// 通过工单号检查工单是否存在。
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <returns>true或者false,ture表示存在，false表示不存在。</returns>
        bool CheckWorkOrderExistedViaOrderNum(string workOrderNo);
        /// <summary>
        /// 根据工单号和配料单号检查工单是否存在。
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="recNumber">配料单号</param>
        /// <returns>true或者false,ture表示存在，false表示不存在。</returns>
        bool CheckWorkOrderExisted(string workOrderNo, string recNumber);
    }
}

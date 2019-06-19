using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备状态转变原因的数据管理接口。
    /// </summary>
    public interface IEquipmentChangeReasons
    {
        /// <summary>
        /// 获取设备状态转变原因。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备状态转变原因的数据集对象。</returns>
        DataSet GetEquipmentChangeReasons(DataSet reqDS);
        /// <summary>
        /// 更新设备状态转变原因。
        /// </summary>
        /// <param name="dsParams">包含设备状态转变原因的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEquipmentChangeReasons(DataSet reqDS);
    }
}

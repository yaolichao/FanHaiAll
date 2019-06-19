using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备转变状态的数据管理接口。
    /// </summary>
    public interface IEquipmentChangeStates
    {
        /// <summary>
        /// 获取设备转变状态数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备转变状态数据的数据集对象。</returns>
        DataSet GetEquipmentChangeStates(DataSet dsParams);
        /// <summary>
        /// 获取指定设备状态（作为FROM状态）的可转变状态数据。
        /// </summary>
        /// <param name="equipmentChangeStateKey">设备转变状态主键。通过它查找对应的转变到的设备状态（TO状态）作为指定设备状态（FROM状态）。</param>
        /// <returns>包含设备转变状态数据的数据集对象。</returns>
        DataSet GetEquipmentChangeStates(string equipmentChangeStateKey);
        /// <summary>
        /// 更新设备转变状态数据。
        /// </summary>
        /// <param name="dsParams">包含设备转变状态数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEquipmentChangeStates(DataSet dsParams);
    }
}

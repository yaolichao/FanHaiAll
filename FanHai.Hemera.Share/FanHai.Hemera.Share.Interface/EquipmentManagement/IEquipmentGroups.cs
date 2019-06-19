using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备组数据管理接口。
    /// </summary>
    public interface IEquipmentGroups
    {
        /// <summary>
        /// 获取设备组信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含设备组信息的数据集对象。</returns>
        DataSet GetEquipmentGroups(DataSet reqDS);
        /// <summary>
        /// 新增设备组。
        /// </summary>
        /// <param name="dsParams">包含设备组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertEquipmentGroups(DataSet reqDS);
        /// <summary>
        /// 更新设备组数据。
        /// </summary>
        /// <param name="dsParams">包含设备组数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEquipmentGroups(DataSet reqDS);
        /// <summary>
        /// 删除设备组数据。
        /// </summary>
        /// <param name="dsParams">包含删除条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteEquipmentGroups(DataSet reqDS);
    }
}

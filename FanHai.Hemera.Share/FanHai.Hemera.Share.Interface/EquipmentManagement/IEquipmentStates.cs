using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备状态数据管理接口。
    /// </summary>
    public interface IEquipmentStates
    {
        /// <summary>
        /// 查询设备状态。可以传入指定的设备状态进行查询(<see cref="EMS_EQUIPMENT_STATES_FIELDS.FIELD_EQUIPMENT_STATE_KEY"/>对应的值)。
        /// </summary>
        /// <param name="dsParams">
        /// 数据集对象。包含名称为<see cref="PARAMETERS_INPUT.DATABASE_TABLE_NAME"/>的数据表。
        /// 数据表中包含列见<see cref="PARAMETERS_INPUT.FILEDS"/>。
        /// 数据表中<see cref="PARAMETERS_INPUT.FIELD_KEY"/>列用来设置查询条件的值。
        /// </param>
        /// <returns>包含设备状态信息的数据集对象。</returns>
        DataSet GetEquipmentStates(DataSet reqDS);
        /// <summary>
        /// 新增设备状态。
        /// </summary>
        /// <param name="dsParams">包含设备状态的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertEquipmentStates(DataSet reqDS);
        /// <summary>
        /// 更新设备状态数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含设备状态数据的数据集对象。
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEquipmentStates(DataSet reqDS);
        /// <summary>
        /// 删除设备状态。
        /// </summary>
        /// <param name="dsParams">包含</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteEquipmentStates(DataSet reqDS);
    }
}

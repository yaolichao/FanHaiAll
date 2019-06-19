using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 工序设备数据管理接口。
    /// </summary>
    public interface IOperationEquipments
    {
        /// <summary>
        /// 根据工序设备主键获取工序和设备的关联关系。
        /// </summary>
        /// <param name="operationKey">工序设备主键。</param>
        /// <returns>包含工序和设备的关联关系的数据集对象。</returns>
        DataSet GetOperationEquipments(string operationKey);
        /// <summary>
        /// 更新工序设备数据。
        /// </summary>
        /// <param name="dsParams">包含工序设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateOperationEquipments(DataSet reqDS);
        /// <summary>
        /// 获取工序下指定线别的设备信息
        /// </summary>
        /// <param name="operationKey">工序主键。</param>
        /// <param name="operateLineName">线别名称。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        DataSet GetOperationEquipment(string operationKey, string operateLineName);
        /// <summary>
        /// 根据批次主键和工步主键获取批次设备信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>
        /// 包含批次加工设备信息的数据集对象。
        /// [EQUIPMENT_KEY,EQUIPMENT_NAME,EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME]
        /// </returns>
        DataSet GetLotEquipment(string lotKey, string stepKey);
        
    }
}

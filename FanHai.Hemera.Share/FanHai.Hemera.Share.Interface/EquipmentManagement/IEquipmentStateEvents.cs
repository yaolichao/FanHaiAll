using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备状态事件登记数据的管理接口。
    /// </summary>
    public interface IEquipmentStateEvents
    {
        /// <summary>
        /// 根据设备主键获取指定设备及其状态事件登记数据。。
        /// </summary>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>包含设备及其状态事件登记数据的数据集对象。</returns>
        DataSet GetCurrentEquipment(string strEquipmentKey);
        /// <summary>
        /// 根据设备主键和用户工号获取指定设备及其状态事件登记数据。
        /// </summary>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="userId">用户工号。</param>
        /// <returns>包含设备及其状态事件登记数据的数据集对象。</returns>
        DataSet GetCurrentEquipment2(string strEquipmentKey,string strUserid);
        /// <summary>
        /// 新增设备状态事件登记记录。
        /// </summary>
        /// <param name="dsParams">包含设备状态事件登记数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertEquipmentStateEvents(DataSet dsStateEvent);
        /// <summary>
        /// 获取当前面板中设备的当前状态
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------------
        /// {EQUIPMENT_KEY}
        /// -----------------------------
        /// </param>
        /// <returns>
        /// 包含设备状态数据的数据集对象。
        /// EQUIPMENT_KEY,EQUIPMENT_NAME,EQUIPMENT_CODE,EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME,EQUIPMENT_STATE_TYPE
        /// </returns>
        DataSet GetLayoutEquipmentCurrStates(DataSet reqDs);
    }
}

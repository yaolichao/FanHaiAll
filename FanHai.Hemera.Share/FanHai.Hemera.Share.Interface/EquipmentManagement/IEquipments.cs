using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.EquipmentManagement
{
    /// <summary>
    /// 设备数据管理接口。
    /// </summary>
    public interface IEquipments
    {
        /// <summary>
        /// 根据工厂车间和工序名称查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        DataSet GetEquipments(string factoryRoomKey, string operationName);
        /// <summary>
        /// 根据工厂车间、工序名称和线别主键查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        DataSet GetEquipments(string factoryRoomKey, string operationName, string lineKey);
        /// <summary>
        /// 根据工厂车间、工序名称和线别主键查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineNames">包含线别名称的数组。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME,EQUIPMENT_CODE,LINE_NAME,LINE_KEY,EQUIPMENT_STATE_NAME]
        /// </returns>
        DataSet GetEquipments(string factoryRoomKey, string operationName, string []lineNames);
        /// <summary>
        /// 根据设备编码查询设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码。</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// </returns>
        DataSet GetEquipments(string equipmentCode);
        /// <summary>
        /// 包含设备信息的数据集
        /// </summary>
        /// <returns></returns>
        DataSet GetEquipments();

     
        /// <summary>
        /// 获取设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码，左匹配模糊查询。</param>
        /// <param name="equipmentName">设备名称，左匹配模糊查询。</param>
        /// <param name="equipmentType">设备类型。如果为空，则查询所有类型的设备。</param>
        /// <param name="pconfig">数据分页查询的配置对象。</param>
        /// <returns>包含设备数据信息的数据集对象。</returns>
        DataSet GetEquipments(string equipmentCode, string equipmentName, string equipmentType, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 获取设备信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="equipmentName">设备名称。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">输出参数，总页数。</param>
        /// <param name="records">输出参数，总记录数。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        DataSet GetEquipments(DataSet reqDS, string equipmentName, int pageNo, int pageSize, 
                                             out int pages, out int records);
        /// <summary>
        /// 获取所有设备数据。
        /// </summary>
        /// <param name="equipmentName">设备名称，左右模糊匹配。</param>
        /// <param name="pageNo">页号。</param>
        /// <param name="pageSize">每页记录数。</param>
        /// <param name="pages">输出参数，总页数。</param>
        /// <param name="records">输出参数，总记录数。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        DataSet GetAllChildEquipments(string equipmentName, int pageNo, int pageSize, out int pages, out int records);
        /// <summary>
        /// 获取可以包含子设备的设备数据。
        /// </summary>
        /// <returns>包含父设备数据的数据集对象。</returns>
        DataSet GetAllParentEquipments();
        /// <summary>
        /// 根据设备主键获取设备的状态信息。
        /// </summary>
        /// <param name="equipmentKey">设备主键</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// [EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME,EQUIPMENT_STATE_TYPE,Equipment_STATE_CATEGORY,DESCRIPTION]
        /// </returns>
        DataSet GetEquipmentState(string equipmentKey);
        /// <summary>
        /// 新增设备。
        /// </summary>
        /// <param name="dsParams">包含设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertEquipments(DataSet dsParams);
        /// <summary>
        /// 更新设备数据。
        /// </summary>
        /// <param name="dsParams">包含设备数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEquipments(DataSet dsParams);
        /// <summary>
        /// 删除设备数据。
        /// </summary>
        /// <param name="dsParams">包含删除条件的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteEquipments(DataSet dsParams);
        /// <summary>
        /// 获取初始设备改变状态。
        /// </summary>
        /// <returns>包含设备改变状态的数据集对象。</returns>
        DataSet GetInitEquipmentChangeState();
    }
}

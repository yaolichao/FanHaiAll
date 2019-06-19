using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 抽检点数据管理接口。
    /// </summary>
    public interface IEDCPiont
    {
        /// <summary>
        /// 查询抽检点设置数据。
        /// </summary>
        /// <param name="dataTable">
        /// 包含查询条件的数据表对象。包含键值对的数据表（TOPRODUCT 可选,OPERATION_NAME 可选），可以为NULL。
        /// </param>
        /// <returns>
        /// 包含抽检点设置数据的数据集。
        /// [ROW_KEY（分组中最小的ROW_KEY），TOPRODUCT,PART_TYPE,OPERATION_NAME,POINT_STATUS,
        /// POINT_STATE_DESCRIPTION,EQUIPMENT_NAME(用逗号分隔开),EQUIPMENT_KEY(用逗号分隔开),
        /// ACTION_NAME,EDC_NAME,SP_NAME,GROUP_KEY(标识分组的键)]
        /// </returns>
        DataSet SearchEdcPoint(DataTable dataTable);
        /// <summary>
        /// 创建抽检点设置数据。
        /// </summary>
        /// <param name="dataSet">
        /// 包含抽检点设置数据的数据集对象。包含一个键值对的数据表。
        /// </param>
        /// <returns>包含EDC名称对应参数数据和执行结果数据的数据集。</returns>
        DataSet CreateEdcPoint(DataSet dataset);
        /// <summary>
        /// 根据抽检点设置的主键获取抽检点参数数据集合。
        /// </summary>
        /// <param name="pointRowKey">抽检点设置主键。</param>
        /// <returns>包含抽检点参数数据的数据集。</returns>
        DataSet GetEdcPiontParams(string pointRowKey);
        /// <summary>
        /// 抽检点修改历史查询
        /// </summary>
        /// <param name="pointRowKey"></param>
        /// <param name="pconfig"></param>
        /// <returns></returns>
        /// Q.001
        DataSet GetEdcPiontParamsTrans(string pointRowKey, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 更新抽检点参数数据。
        /// </summary>
        /// <param name="dsEdcPointParam">包含抽检点参数数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet UpdateEDCPointParams(DataSet dsEdcPointParam);
        /// <summary>
        /// 删除抽检点数据。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <returns>true：删除成功。false：删除失败。</returns>
        DataSet DeleteEDCPoint(string groupKey);
        /// <summary>
        /// 更新抽检点数据的状态。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <param name="pointStatus">新的抽检点设置状态。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet UpdateEDCPointStatus(string groupKey, string pointStatus);
        /// <summary>
        /// 查找正在使用的相同类型的抽检点数据是否存在。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <returns>true：存在。false：不存在。</returns>
        bool FindExistUsedEDCPoint(string pointRowKey);
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="productNo">产品号。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="equipmentKey">设备主键。如果为空，则查询设备主键为NULL的数据。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        DataSet GetEDCPoint(string productNo,string operationName,string equipmentKey);
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        DataSet GetEDCPoint(string factoryRoomKey, string operationName);
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="factoryRoomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="partType">成品类型。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        DataSet GetEDCPoint(string factoryRoomKey, string operationName, string partType, string equipmentKey);
    }
}

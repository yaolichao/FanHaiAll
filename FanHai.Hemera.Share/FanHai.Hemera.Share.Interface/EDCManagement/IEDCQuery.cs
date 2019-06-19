using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据采集查询接口，用于导出采集得到的数据值。
    /// </summary>
    public interface IEDCQuery
    {
        /// <summary>
        /// 查询抽检点数据。
        /// </summary>
        /// <returns>包含抽检点数据的数据集对象。</returns>
        DataSet SearchEdcPoint();
        /// <summary>
        /// 根据车间名称查询设备。
        /// </summary>
        /// <param name="strFactoryRoom">车间名称。</param>
        /// <returns>包含设备名称的数据集对象。</returns>
        DataSet SearchEMS(string strFactoryRoom);
        /// <summary>
        /// 根据条件查询设备。
        /// </summary>
        /// <param name="strFactoryRoom">车间名称。</param>
        /// <param name="groupKey">抽检点分组。</param>
        /// <param name="equipmentKey">设备主键，使用逗号分隔：设备1,设备2...。</param>
        /// <returns>包含设备名称的数据集对象。</returns>
        DataSet SearchEMS(string strFactoryRoom, string groupKey, string equipmentKey);
        /// <summary>
        ///  查询采集参数。
        /// </summary>
        /// <returns>包含采集参数的数据集对象。</returns>
        DataSet SearchParam( );
        /// <summary>
        /// 根据采集分组主键获取基础参数查询采集参数数据,
        /// </summary>
        /// <param name="edcKey">采集分组主键。</param>
        /// <returns>包含采集参数数据的数据集对象。</returns>
        DataSet SearchParam( string edcKey);
        /// <summary>
        /// 根据条件查询采集得到的数据。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------
        /// {ROW_KEY}
        /// {START_DATE}
        /// {END_DATE}
        /// {EQUIPMENT_NAME}
        /// {PARAM_NAME}
        /// ------------------------------
        /// </param>
        /// <returns>包含采集得到的数据的数据集对象。</returns>
        DataSet EDCValueQuery(DataTable dtParams);
        
    }
}

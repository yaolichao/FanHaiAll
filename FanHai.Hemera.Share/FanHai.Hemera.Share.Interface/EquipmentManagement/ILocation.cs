using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 区域数据管理接口。
    /// </summary>
    public interface ILocation
    {
        /// <summary>
        /// 保存区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveNewLocation(DataSet dsParams);
        /// <summary>
        /// 根据区域层级值获取可以设置子区域的区域数据。
        /// </summary>
        /// <param name="level">0：空数据集，1：工厂,2：楼层，其他值：车间。</param>
        /// <returns>包含区域数据的数据集对象。</returns>
        DataSet GetWorkshopBindData(int level);
        /// <summary>
        /// 更新区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateLocation(DataSet dsParams);
        /// <summary>
        /// 删除区域数据。
        /// </summary>
        /// <param name="dsParams">包含区域数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteLocation(DataSet dsParams);

        /// <summary>
        /// 获取区域中的线别信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含区域中的线别信息的数据集对象。</returns>
        DataSet GetLinesAndLocationLine(DataSet dsParams);

        /// <summary>
        /// 保存区域和线别的关联关系。
        /// </summary>
        /// <param name="dsParams">包含区域和线别关联关系的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveLocationLineRelation(DataSet dsParams);
        /// <summary>
        /// 查询区域信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------------------
        /// {LOCATION_NAME}
        /// {LOCATION_LEVEL}
        /// -------------------------------------------
        /// </param>
        /// <returns>包含区域信息的数据集对象。</returns>
        DataSet SearchLocation(DataSet dsParams);

        /// <summary>
        /// 获取区域信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -------------------------------------------
        /// {LOCATION_KEY}
        /// -------------------------------------------
        /// </param>
        /// <returns>包含区域信息的数据集对象。</returns>
        DataSet GetLocation(DataSet dsParams);

        /// <summary>
        /// 获取所有的区域信息。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含区域信息的数据集对象。</returns>
        DataSet GetAllLoactions(DataSet dsParams);
        /// <summary>
        /// 根据区域层级值获取可以设置子区域的区域数据。
        /// </summary>
        /// <param name="level">0：空数据集，1：工厂,2：楼层，其他值：车间。</param>
        /// <returns>包含区域数据的数据集对象。</returns>
        DataSet GetParentLocations(int parentLevel);
        /// <summary>
        /// 根据线别数据获取线别所属的车间。
        /// </summary>
        /// <param name="lines">使用逗号分开的所有线别值。</param>
        /// <returns>包含工厂车间信息的数据集对象
        /// </returns>
        DataSet GetFactoryRoomByLines(string lines);
        /// <summary>
        /// 根据线上仓数据获取所属的车间。
        /// </summary>
        /// <param name="lines">使用逗号分开的所有线上仓名称。</param>
        /// <returns>包含工厂车间信息的数据集对象
        /// </returns>
        DataSet GetFactoryRoomByStores(string stores);
        /// <summary>
        /// 根据名称获取车间信息。
        /// </summary>
        /// <param name="name">车间名称。</param>
        /// <returns>包含车间信息的数据集。</returns>
        DataSet GetFactoryRoom(string name);
        /// <summary>
        /// 根据车间名称获取其所在的工厂信息。
        /// </summary>
        /// <param name="name">车间名称。</param>
        /// <returns>包含工厂信息的数据集。</returns>
        DataSet GetFactory(string roomName);
    }
}

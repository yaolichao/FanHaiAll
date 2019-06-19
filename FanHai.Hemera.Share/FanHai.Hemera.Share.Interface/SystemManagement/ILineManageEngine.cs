using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 生产线别数据管理接口。
    /// </summary>
    public interface ILineManageEngine
    {
        /// <summary>
        /// 新增线别。
        /// </summary>
        /// <param name="dsParams">包含线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddLine(DataSet dataset);
        /// <summary>
        /// 根据线别主键获取线别数据。
        /// </summary>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>包含线别数据的数据集对象。</returns>
        DataSet GetLine(string lineKey);
        /// <summary>
        /// 删除线别。
        /// </summary>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteLine(string lineKey);
        /// <summary>
        /// 更新线别。
        /// </summary>
        /// <param name="dsParams">包含线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateLine(DataSet dataset);
        /// <summary>
        /// 根据线别名称获取线别数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// 数据表结构。
        /// ----------------------
        ///| LINE_NAME  |   ROWNUM|
        /// ----------------------
        /// </param>
        /// <returns>包含线别数据的数据集对象。</returns>
        DataSet GetHelpInfoForLineHelpForm(DataSet dataset);
        /// <summary>
        /// 根据线别名称获取线别信息。
        /// </summary>
        /// <param name="lines">使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>包含线别信息的数据集对象。</returns>
        DataSet GetLinesInfo(string lines);
        /// <summary>
        /// 根据线别名称和工厂车间主键获取线别信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间主键。</param>
        /// <param name="lines">使用“逗号(,)”分开的线别名称字符串。“X01,X02,C01...”</param>
        /// <returns>
        /// 包含线别信息的数据集对象。
        /// [LINE_NAME,PRODUCTION_LINE_KEY,LINE_CODE,ROOM_NAME,ROOM_KEY]
        /// </returns>
        DataSet GetLinesInfo(string factoryRoomKey, string lines);
        /// <summary>
        /// 根据用户主键获取用户拥有的线别数据（始终包含一条线别名称为空的记录。）。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户拥有的线别数据的数据集对象。</returns>
        DataSet GetLinesInfoContainEmpty(string userKey);
        /// <summary>
        ///根据工厂车间获取线别名称
        /// </summary>
        /// <param name="factoryroom">工厂车间名称。</param>
        /// <returns>包含线别数据的数据集对象。</returns>
        DataSet GetLinesByFactoryRoom(string factoryroom);

    }
}

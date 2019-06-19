
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface ILineSettingEngine
    {
        /// <summary>
        /// 通过用户获取对应有权限的线别信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">线别名称</param>
        /// <returns>用户拥有权限的线别表集</returns>
        DataSet GetLineByUserNameAndLineName(string userName, string lineName);

        /// <summary>
        /// 通过线别主键获取对应的子线信息
        /// </summary>
        /// <param name="lineKey">线别主键</param>
        /// <returns>线别主键对应的子线的数据集合</returns>
        DataSet GetSubLineByLineKey(string mainLineKey);

        /// <summary>
        /// 通过用户获取对应有权限的工序信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">工序名称</param>
        /// <returns>用户拥有权限的工序表集</returns>
        DataSet GetOperationByUserNameAndOperationName(string userName, string operationName);

        /// <summary>
        /// 对线别对应的修改进行进行保存
        /// </summary>
        /// <param name="dsLine">线别操作相关的表集</param>
        /// <returns>操作返回的表集信息</returns>
        DataSet SaveLineInfo(DataSet dsLine);

        /// <summary>
        /// 检查选定的线别是否为组件允许过站的线别
        /// </summary>
        /// <param name="lotLineKey">绑定的线别主键</param>
        /// <param name="curLineKey">当前的线别主键</param>
        /// <param name="curOperationName">当前工序名称</param>
        /// <returns>操作结果</returns>
        DataSet CheckLotLine(string lotLineKey, string curLineKey, string curOperationName);
    }
}

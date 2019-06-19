using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 用户数据管理接口。
    /// </summary>
    /// </summary>
    public interface IUserEngine
    {
        /// <summary>
        /// 获取用户信息。根据用户主键查询对应的用户信息。
        /// </summary>
        /// <param name="dataset">
        /// 数据集对象。数据集对象中必须包含名称为"<see cref="RBAC_USER_FIELDS.DATABASE_TABLE_NAME"/>"的数据表对象。
        /// 数据表中必须包含两个列"name"和"value"。列name存放的查询条件，列value存放查询条件对应的值。
        /// 目前可以使用的查询条件名称（即，列name存放的值）：
        /// <see cref="RBAC_USER_FIELDS.FIELD_USER_KEY"/>
        /// </param>
        /// <returns>包含用户信息和执行结果的数据集对象。</returns>
        DataSet GetUserInfo(DataSet dsParams);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dsParams">包含用户信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddUser(DataSet dsParams);
        /// <summary>
        /// 更新用户。
        /// </summary>
        /// <param name="dsParams">包含用户信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateUser(DataSet dsParams);
        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteUser(string userKey);
        /// <summary>
        /// 根据用户名或者工号查询用户数据。
        /// </summary>
        /// <param name="userName">用户名或者工号。</param>
        /// <returns>包含用户信息和执行结果的数据集对象。</returns>
        DataSet SearchUser(string userName);
        /// <summary>
        /// 获取用户信息。根据用户工号获取用户信息。
        /// </summary>
        /// <param name="ds">
        /// 数据集对象。数据集对象中必须包含表名为<see cref="RBAC_USER_FIELDS.DATABASE_TABLE_NAME"/>的数据表对象。
        /// 数据表中包含名称为<see cref="RBAC_USER_FIELDS.FIELD_USERNAME"/>的列，用于存放员工号。为查询提供查询条件。
        /// </param>
        /// <returns>
        /// 包含查询得到的用户信息的数据集对象。
        /// </returns>
        DataSet CheckUser(DataSet ds);
        /// <summary>
        /// 根据用户主键查询用户权限。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户权限和执行结果的数据集对象。</returns>
        DataSet GetPrivilegeOfUser(string userKey);
        /// <summary>
        /// 获取用户拥有权限的工序名称。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 使用,分割的工序名称字符串。
        /// </returns>
        DataSet GetOperationOfUser(string userKey);
        /// <summary>
        /// 获取用户拥有权限的线别信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 包含用户拥有权限的线别信息。
        /// </returns>
        DataSet GetLineOfUser(string userKey, string roomName);
        /// <summary>
        /// 获取用户拥有权限的线别信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 包含用户拥有权限的线别信息。
        /// </returns>
        DataSet GetLineOfUser(string userKey);
        /// <summary>
        /// 获取用户拥有权限的线上仓信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <param name="roomName">车间名称。</param>
        /// <returns>
        /// 包含用户拥有权限的线上仓信息。
        /// </returns>
        DataSet GetStoreOfUser(string userKey, string roomName);
        /// <summary>
        /// 获取用户拥有权限的线上仓信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>
        /// 包含用户拥有权限的线上仓信息。
        /// </returns>
        DataSet GetStoreOfUser(string userKey);
        /// <summary>
        /// 记录用户登入信息。
        /// </summary>
        /// <param name="reqDS">包含用户登入信息的数据集。</param>
        /// <returns>true：记录成功。false:记录失败。</returns>
        bool LogUserLoginInfo(DataSet reqDS);
        /// <summary>
        /// 记录用户登出信息。
        /// </summary>
        /// <param name="loginLogKey">用户登录日志主键。</param>
        /// <returns>true：记录成功。false:记录失败。</returns>
        bool LogUserLogoutInfo(string loginLogKey);
        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="userTable">包含用户信息的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet ChangePassword(DataTable userTable);
        /// <summary>
        /// 添加操作员。
        /// </summary>
        /// <param name="dtParams">包含操作员信息的数据表对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertOperator(DataTable dataTable);
        /// <summary>
        /// 删除操作员。
        /// </summary>
        /// <param name="dtParams">包含操作员信息的数据表对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteOperator(DataTable dataTable);
        /// <summary>
        /// 查询操作人员。
        /// </summary>
        /// <param name="badge">工号或用户名。模糊匹配查询。</param>
        /// <returns>包含操作人员信息的数据集对象。
        /// 该数据集对象附加扩展属性,扩展属性键值为<see cref="PARAMETERS.OUTPUT_MESSAGE"/>,可以通过扩展属性获取额外的输出信息。
        /// <code>
        /// dsReturn.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE]
        /// </code>
        /// </returns>
        DataSet SearchOperator(string badge);
        /// <summary>
        /// 验证操作员是否存在
        /// </summary>
        /// <param name="badge">操作员工号。</param>
        /// <returns>包含操作员信息的数据集对象。
        /// 该数据集对象附加扩展属性,扩展属性键值为<see cref="PARAMETERS.OUTPUT_MESSAGE"/>,可以通过扩展属性获取额外的输出信息。
        /// <code>
        /// dsReturn.ExtendedProperties[PARAMETERS.OUTPUT_MESSAGE]
        /// </code>
        /// </returns>
        DataSet CheckOperator(string badge);
        /// <summary>
        /// 验证操作员是否具有尾单处理权限
        /// </summary>
        /// <param name="badge"></param>
        /// <returns></returns>
        DataSet CheckLastPackageOperator(string badge);
    }
}

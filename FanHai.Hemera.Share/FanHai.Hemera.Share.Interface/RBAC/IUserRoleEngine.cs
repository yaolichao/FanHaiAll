using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 用户角色数据管理接口。
    /// </summary>
    public interface IUserRoleEngine
    {
        /// <summary>
        /// 根据用户主键获取用户所属角色信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户所属角色信息的数据集对象。</returns>
        DataSet GetRolesOfUser(string userKey);
        /// <summary>
        /// 根据角色主键获取角色中的用户信息。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含用户信息的数据集对象。</returns>
        DataSet GetUsersOfRole(string roleKey);
        /// <summary>
        /// 根据用户主键获取用户不属于的角色信息。
        /// </summary>
        /// <param name="userKey">用户主键。</param>
        /// <returns>包含用户不属于的角色信息的数据集对象。</returns>
        DataSet GetRolesNotBelongToUser(string userKey);
        /// <summary>
        /// 保存用户角色信息。
        /// </summary>
        /// <param name="dsParams">包含用户角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveUserRole(DataSet dsParams);
    }
}

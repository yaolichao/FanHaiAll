using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 角色数据管理接口。
    /// </summary>
    public interface IRoleEngine
    {
        /// <summary>
        /// 根据角色主键获取角色数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含角色数据的数据集对象。</returns>
        DataSet GetRoleInfo(string roleKey);
        /// <summary>
        /// 新增角色。
        /// </summary>
        /// <param name="dataset">包含角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>            
        DataSet AddRole(DataSet dataset);
        /// <summary>
        /// 更新角色。
        /// </summary>
        /// <param name="dsParams">包含角色数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateRole(DataSet dataset);
        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteRole(string roleKey);
    }
}

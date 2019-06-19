using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 系统权限数据管理接口。
    /// </summary>
    public interface IPrivilegeEngine
    {
        /// <summary>
        /// 保存系统权限。
        /// </summary>
        /// <param name="dsParams">包含系统权限数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SavePrivilege(DataSet dataset);
        /// <summary>
        /// 获取系统权限数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <returns>包含系统权限数据的数据集对象。</returns>
        DataSet GetPrivilege(DataSet dataset);
        /// <summary>
        /// 根据角色获取拥有的线别名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含线别名称的数据集对象。</returns>
        DataSet GetLinesOwnedByRole(string roleKey);
        /// <summary>
        /// 获取不属于角色的线别名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含线别名称的数据集对象。</returns>
        DataSet GetLinesRoleNotOwn(string roleKey);
        /// <summary>
        /// 保存角色拥有的线别。
        /// </summary>
        /// <param name="dsParams">包含角色拥有线别数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveLinesOfRole(DataSet dataset);
        /// <summary>
        /// 根据角色获取拥有的工序名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含工序名称数据的数据集对象。</returns>
        DataSet GetOperationOwnedByRole(string roleKey);
        /// <summary>
        /// 获取不属于角色的工序名称。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <returns>包含工序名称数据的数据集对象。</returns>
        DataSet GetOperationRoleNotOwn(string roleKey);
        /// <summary>
        /// 保存角色拥有的工序数据。
        /// </summary>
        /// <param name="dsParams">包含角色拥有工序数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveOperationsOfRole(DataSet dataset);
        /// <summary>
        /// 保存角色拥有的线上仓数据。
        /// </summary>
        /// <param name="dsParams">包含角色拥有线上仓数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveStoreOfRole(DataSet dataSet);
        /// <summary>
        /// 获取角色的线上仓名称数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含线上仓名称数据的数据集对象。</returns>
        DataSet GetStoreOfRole(string roleKey, string type);
        /// <summary>
        /// 获取角色的用户数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含用户数据的数据集对象。</returns>
        DataSet GetUserOfRole(string roleKey, string type);
        /// <summary>
        /// 获取角色拥有的机台状态数据。
        /// </summary>
        /// <param name="roleKey">角色主键。</param>
        /// <param name="type">类型，是否拥有（SELECT：角色拥有，UNSELECT：角色不拥有。）</param>
        /// <returns>包含机台状态数据的数据集对象。</returns>
        DataSet GetStatusOfRole(string roleKey, string type);           //add by qym 20120606  Q.001
        /// <summary>
        /// 保存角色的机台状态权限
        /// </summary>
        /// <param name="dsParams">包含角色拥有机台状态数据的数据集对象</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SaveStatusOfRole(DataSet dataSet);                      //add by qym 20120606 13:38 Q.001
    }
}

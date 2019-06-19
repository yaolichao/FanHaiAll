using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 自动过站的数据管理接口。
    /// </summary>
    public interface IWipJobAutoTrack
    {
        /// <summary>
        /// 获取需要自动过站的最新一笔数据主键。
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含最新一笔数据主键的数据集对象。</returns>
        DataSet GetTrackNow(string stepKey);
        /// <summary>
        /// 获取等待自动出站的不重复的工步主键。
        /// </summary>
        /// <returns>包含工步主键的数据集对象。</returns>
        DataSet GetMonStepKey();
        /// <summary>
        /// 获取等待出站的生产批次。
        /// </summary>
        /// <returns>包含等待出站的生产批次的数据集对象。</returns>
        DataSet GetWaitingForTrackOutJobs();
        /// <summary>
        /// 新增在制品操作信息。
        /// </summary>
        /// <param name="dtParams">包含在制品操作信息的数据集对象。</param>
        void InsertWipMessage(DataTable dtParams);
        /// <summary>
        /// 获取发送给指定人员的信息。
        /// </summary>
        /// <param name="user">指定人员登录名。</param>
        /// <returns>包含信息的数据集对象。</returns>
        DataSet GetErrorMessageInfor(string user);
    }
}
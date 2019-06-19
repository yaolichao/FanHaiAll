using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 和SAP进行数据交互的管理接口。
    /// </summary>
    public interface ISAPEngine
    {
        /// <summary>
        /// 获取工单信息方法。
        /// </summary>
        /// <param name="workOrder">工单号</param>
        /// <returns>工单信息</returns>
        DataSet GetWorkOrderInfo(string workOrder,string user);
        /// <summary>
        /// 工单信息获取
        /// </summary>
        /// <returns>包含工单信息的数据集。</returns>
        DataSet GetWorkOrderInfo(string user);
        /// <summary>
        /// 刷新来料列表，同步SAP和MES物料表。
        /// </summary>
        /// <param name="dtParams">
        /// 包含更新信息的数据集对象。
        /// -------------------------------------------
        /// {CREATOR}
        /// {CREATE_TIMEZONE}
        /// {EDITOR}
        /// {EDIT_TIMEZONE}
        /// -------------------------------------------
        /// </param>
        /// <returns>包含操作结果的数据集对象。</returns>
        DataSet RefreshReceiveMaterial(DataTable dataTable);


        void RefreshSAPWorkOrderInfo();
    }
}

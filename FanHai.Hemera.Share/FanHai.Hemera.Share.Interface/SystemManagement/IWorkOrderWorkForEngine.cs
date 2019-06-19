using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.SystemManagement
{
    /// <summary>
    /// 工单报工数据管理接口。
    /// </summary>
    public interface IWorkOrderWorkForEngine
    {
        /// <summary>
        /// 获取工单报工数据。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集对象。</param>
        /// <param name="pageNo">第几页。</param>
        /// <param name="pageSize">每页行数。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="records">总记录数。</param>
        /// <returns>包含工单数据的数据集对象。</returns>
        DataSet GetWorkOrderWorkFor(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records);
        /// <summary>
        /// 工单报工。
        /// </summary>
        /// <param name="tableParam">
        /// 传入参数的值：AUFNR 工单号 ROOM_KEY 工厂主键 CREATOR 创建者 CREATE_TIMEZONE 创建时间 REPORTOR 报工人 SHIFT_NAME 班次
        /// </param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet GongDanBaoGong(DataTable tableParam);
    }
}

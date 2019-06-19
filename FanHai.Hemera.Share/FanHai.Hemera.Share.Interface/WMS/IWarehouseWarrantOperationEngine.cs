using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 入库单管理相关操作
    /// </summary>
    public interface IWarehouseWarrantOperationEngine
    {
        /// <summary>
        /// 根据订单类型获取特征名称
        /// </summary>
        /// <param name="ZMMTYP"></param>
        /// <returns></returns>
        DataSet GetATNAM(String ZMMTYP);

        /// <summary>
        /// 根据工单号获取工单明细
        /// </summary>
        /// <param name="WorkNO"></param>
        /// <returns></returns>
        DataSet GetWorkItems(String PALNO);

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        int SaveWarehouseWarrant(DataTable dtHead, DataTable dtItem, out string returnInfo);

        /// <summary>
        /// 查询入库单信息
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        DataSet QueryWarehouseWarrant(DataSet dsParams, ref PagingQueryConfig pconfig);

        DataSet QueryWarehouseWarrantHead(string ZMBLNR, string ISSYN, bool isQueryForSyn);

        DataSet QueryWarehouseWarrantItems(string ZMBLNR);
        /// <summary>
        /// 入库单加删除标记
        /// </summary>
        /// <param name="ZMBLNR"></param>
        /// <returns></returns>
        bool AddLvorm(string ZMBLNR);

        /// <summary>
        /// 同步过账
        /// </summary>
        bool SynSAP(string ZMBLNR, string SYNMAN, out string returnStr);

    }
}

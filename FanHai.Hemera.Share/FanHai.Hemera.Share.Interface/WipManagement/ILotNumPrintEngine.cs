using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 托盘信息查询操作接口。
    /// </summary>
    public interface ILotNumPrintEngine
    {
        /// <summary>
        /// 获取未打印的组件序列号
        /// </summary>
        /// <param name="facKey">车间主键</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns>未打印的组件序列号数据集</returns>
        DataSet GetNotPrintLotNumInf(string facKey, string equipmentKey, string lineKey);

        /// <summary>
        /// 获取打印信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        DataSet GetPrintInf(string lotNumber, string dateStart, string dateEnd, string printerNo);
        /// <summary>
        /// 确认批次是否存在且更新信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <param name="printer">打印人员</param>
        /// <returns>是否更新成功</returns>
        DataSet CheckAndUpdateLotInf(string lotNumber,string printer,string facKey,string equipmentKey,string lineKey);

        /// <summary>
        /// 更新打印信息
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <param name="printer"></param>
        /// <param name="mac"></param>
        /// <returns></returns>
        bool UpdateLotInf(string lotNumber, string printer);
        
        /// <summary>
        /// 保存打印信息
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <param name="printer"></param>
        /// <param name="mac"></param>
        /// <param name="is_RePrint"></param>
        /// <returns></returns>
        bool save_Print(string lotNumber, string printer, string mac, char is_RePrint);
        /// <summary>
        /// 根据工单号获取打印代码
        /// </summary>
        /// <param name="orderNum">工单号</param>
        /// <returns></returns>
        DataSet GetIdByOrderNumber(string orderNum);
        /// <summary>
        /// 根据组件序列号获取打印代码
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        DataSet GetPrintIdByLotNumber(string lotNumber);
        /// <summary>
        /// //根据批次号获取工序设备线别信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <returns></returns>
        DataSet GetEquipmentByLotNumber(string lotNumber);
        /// <summary>
        /// 更新数据信息
        /// </summary>
        /// <param name="lotNumber">批次号</param>
        /// <param name="operations">工序名称</param>
        /// <param name="equipmentkey">设备主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns></returns>
        DataSet UpdatePorLot(string lotNumber, string operations, string equipmentkey, string lineKey);
    }
}

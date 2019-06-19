using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 线上仓的数据管理接口。
    /// </summary>
    public interface IStoreEngine
    {
        /// <summary>
        /// 添加线上仓。
        /// </summary>
        /// <param name="dsParams">包含线上仓数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddStore(DataSet dsParams);
        /// <summary>
        /// 根据线上仓主键获取线边仓信息。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含线上仓信息的数据集对象。</returns>
        DataSet GetStore(string storeKey);
        /// <summary>
        /// 根据线上仓类型获取线上仓信息。
        /// </summary>
        /// <param name="storeType">线上仓类型。</param>
        /// <returns>包含线上仓信息的数据集对象。</returns>
        DataSet GetStores(string storeType);
        /// <summary>
        /// 根据线上仓主键删除线上仓记录。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteStore(string storeKey);
        /// <summary>
        /// 更新线上仓数据。
        /// </summary>
        /// <param name="dsParams">包含线上仓数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateStore(DataSet dataset);
        /// <summary>
        /// 获取线上仓数据集合。
        /// </summary>
        /// <returns>
        /// 包含线上仓数据的数据集对象。
        /// [STORE_KEY, STORE_NAME,STORE_TYPE]
        /// </returns>
        DataSet GetStoreName();
        /// <summary>
        /// 根据线上仓类型获取可用且需要申请过账的线上仓信息。
        /// </summary>
        /// <param name="stroeType">线上仓类型,使用逗号(,)分隔。(可以为空字符串，表示查询所有线上仓）。</param>
        /// <returns>
        /// 查询得到的包含线上仓信息的数据集对象。
        /// </returns>
        DataSet GetReworkStroeInfor(string stroeType);
        /// <summary>
        /// 根据线上仓名称查询可用线上仓信息。
        /// </summary>
        /// <param name="storeName">线上仓名称，使用逗号(,)分隔。</param>
        /// <returns>包含重工线上仓信息的数据集对象。</returns>
        DataSet GetReworkStore(string storeName);

        //WST_STORE_MAT
        /// <summary>
        /// 根据线上仓主键获取打到线上仓的不良生产批次信息。
        /// </summary>
        /// <param name="storeKey">线上仓主键。</param>
        /// <returns>包含线上仓中不良生产批次信息的数据集对象。</returns>
        DataSet GetLotsInStore(string storeKey);
        /// <summary>
        /// 从线上仓中转出生产批次。
        /// </summary>
        /// <param name="dsParams">包含转出生产批次数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet TransferFromStore(DataSet dataset);
        /// <summary>
        /// 获取不良生产批次信息。
        /// </summary>
        /// <param name="paramTable">包含查询条件的数据集对象。</param>
        /// <returns>包含不良生产批次信息的数据集对象。</returns>
        DataSet GetStoreMatInfo(DataTable paramTable);
        /// <summary>
        /// 根据工单号、线边仓主键、工序名称、线别主键、班别名称、状态获取批次返工或退库的数据。
        /// </summary>
        /// <param name="workerOrder">工单号码。</param>
        /// <param name="storeKey">线边仓主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <param name="shiftName">班别名称。</param>
        /// <param name="status">状态。（0：初始状态 1：申请过账 2：过账通过3：批次作废）</param>
        /// <returns>
        /// 查询得到的包含批次返工或退库的数据的数据集对象。
        /// </returns>
        DataSet GetReworkLotInformation(string workerOrder, string storeKey,string operationName,string lineKey,string shiftName,string status);
        /// <summary>
        /// 根据工序名（用","分开的工序名字符串），线别名（用","分开的工序名字符串），线边仓类型，班别名称查询等待返工或退库操作的批次信息。
        /// </summary>
        /// <param name="operations">工序名（用","分开的工序名字符串）</param>
        /// <param name="lines">线别名（用","分开的工序名字符串</param>
        /// <param name="storeTypes">线别仓类型。</param>
        /// <param name="shiftName">班别名称。</param>
        /// <returns>
        /// 查询得到的包含等待返工或退库操作批次信息的数据集对象。
        /// </returns>
        DataSet SearchLotWaitingForTransact(string operations, string lines, string storeTypes, string shiftName);
        /// <summary>
        /// 根据线上仓主键和状态获取线上仓中工单信息。
        /// </summary>
        /// <param name="storeKey">线边仓主键。</param>
        /// <param name="strStatus">状态（0：初始状态 1：申请过账 2：过账通过3：批次作废）。</param>
        /// <returns>
        /// 查询得到的包含线上仓中工单信息的数据集对象。
        /// </returns>
        DataSet GetWorkOrderInforByStroe(string storeKey,string strStatus);
        /// <summary>
        /// 根据线上仓类型获取线上仓中工单信息。
        /// </summary>
        /// <param name="storeType">线上仓类型。</param>
        /// <returns>
        /// 查询得到的包含线上仓中工单信息的数据集对象。
        /// </returns>
        DataSet GetWorkOrderInforByStroeType(string storeType);
        /// <summary>
        /// 根据返工或退库记录主键获取返工或退库批次的信息。
        /// </summary>
        /// <param name="strRowKey">返工或退库记录主键。</param>
        /// <returns>
        /// 查询得到的包含返工或退库批次信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        DataSet GetReworkLotInformationByRowKey(string strRowKey);
        /// <summary>
        /// 根据工序主键获取不良缺陷代码信息。
        /// </summary>
        /// <param name="strStepKey">工序主键。</param>
        /// <returns>
        /// 查询得到的包含不良缺陷代码信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        DataSet GetDefectInforByStepKey(string strStepKey);
        /// <summary>
        /// 根据工序主键获取退库缺陷代码信息。
        /// </summary>
        /// <param name="strStepKey">工序主键。</param>
        /// <returns>
        /// 查询得到的包含退库缺陷代码信息的数据集对象。
        /// </returns>
        /// comment by peter 2012-2-23
        DataSet GetReasonCodeForReturn(string strStepKey);
        /// <summary>
        /// 根据用户获取错误消息。
        /// </summary>
        /// <param name="User">用户工号。</param>
        /// <returns>包含错误消息的数据集对象。</returns>
        DataSet GetErrorMessageInfor(string User);

    }
}

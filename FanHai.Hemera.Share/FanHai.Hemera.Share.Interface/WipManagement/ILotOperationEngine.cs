using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 包含批次操作方法的接口。
    /// </summary>
    public interface ILotOperationEngine
    {
        /// <summary>
        /// 获取可选问题工序。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        DataSet GetTroubleStepInfo(string lotKey, string operations);
        /// <summary>
        /// 获取电池片回收的可选问题工序。
        /// </summary>
        /// <param name="lotKey">回收电池片的批次主键。。</param>
        /// <param name="operations">拥有权限的工序名称，使用逗号(,)分隔。</param>
        /// <returns>包含问题工序的数据集对象。</returns>
        DataSet GetRecoveredTroubleStepInfo(string lotKey, string operations);
        /// <summary>
        /// 获取暂停原因代码类别。
        /// </summary>
        /// <returns>包含暂停原因代码类别的数据集对象。</returns>
        DataSet GetHoldReasonCodeCategory();
        /// <summary>
        /// 根据原因分组主键获取原因分类。
        /// </summary>
        /// <param name="categoryKey">原因分组主键。</param>
        /// <returns>包含原因分类的数据集对象。</returns>
        DataSet GetReasonCodeClass(string categoryKey);
        /// <summary>
        /// 根据原因代码类别获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        DataSet GetReasonCode(string categoryKey);
        /// <summary>
        /// 根据原因代码类别主键和原因代码分类获取原因代码。
        /// </summary>
        /// <param name="categoryKey">原因代码分类主键。</param>
        /// <param name="codeClass">原因代码分类。</param>
        /// <returns>包含原因代码数据的数据集对象。</returns>
        DataSet GetReasonCode(string categoryKey,string codeClass);
        /// <summary>
        /// 获取退料原因信息代码
        /// </summary>
        /// <param name="stepKey">工步主键。</param>
        /// <returns>包含退料原因代码的数据集对象。</returns>
        DataSet GetReturnMaterialReasonCode(string stepKey);
        /// <summary>
        /// 获取待补片的批次信息。
        /// </summary>
        /// <param name="workorderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="efficiency">转换效率。</param>
        /// <returns>包含待补片批次信息的数据集对象。</returns>
        DataSet GetPatchedLotNumber(string workorderNo, string proId, string efficiency);
        /// <summary>
        /// 获取待回收的批次信息（包含电池片报废和被电池片补片的批次数据）。
        /// </summary>
        /// <param name="recoverdLotKey">回收批次主键。</param>
        /// <param name="operationKey">问题工序主键。</param>
        /// <returns>包含待回收批次信息的数据集对象。</returns>
        DataSet GetBeRecoverdLotNumber(string recoverdLotKey, string operationKey);
        /// <summary>
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        DataSet GetProdId();
        /// <summary>
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。
        /// [EFFICIENCY_NAME,LEFFICIENCY,UEFFICIENCY]
        /// </returns>
        DataSet GetEfficiency();
        /// <summary>
        /// 获取批次暂停信息。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次暂停信息的数据集对象。</returns>
        DataSet GetLotHoldInfo(string lotNumber);
        /// <summary>
        /// 根据批次主键获取批次进站是否超时。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// true表示超时，false表示没有超时。
        /// </returns>
        bool GetLotTrackInIsDelay(string lotKey);
        /// <summary>
        /// 根据批次主键获取批次出站的时间控制数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次时间控制数据的数据集对象。数据集中包含两个数据表对象。
        /// 有一个名称为TrackOutStatus的数据表对象，数据表的列名为
        /// TimeStatusFlag（时间状态，0：加工时间没有满足最小加工时间。
        /// 1:加工时间满足最小时间，没有超过报警时间。
        /// 2:加工时间超过报警时间，没有超过最大加工时间。
        /// 3:加工时间超过最大加工时间。） 
        /// TimeControlBaseSubMin（基础加工时间-最小加工时间）。
        /// </returns>
        DataSet GetTrackOutTimeControlStatus(string lotKey);
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        bool CheckFixCycle(string lineKey, string proId, DateTime waitTrackInTime);
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        bool CheckCalibrationCycle(string proId, string deviceCode);
        /// <summary>
        /// 检查IV测试数据是否存在。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="trackInTime">进站时间。</param>
        /// <returns>false:不存在。true：存在。</returns>
        bool CheckIVTestData(string lotNumber, DateTime trackInTime);
        /// <summary>
        /// 工单在过站时检查花色是否必须输入。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <returns>true：需要检查花色。false：不用检查花色。</returns>
        bool IsCheckColorByWorkOrder(string workOrderNo);
        /// <summary>
        /// 获取组件有效的IV测试时间。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>组件有效的IV测试时间。</returns>
        DateTime GetLotValidIVTestTime(string lotNumber);
        /// <summary>
        /// （电池片/组件）报废操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP "/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP"/>。
        /// </remarks>
        /// <param name="dsParams">包含报废信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotScrap(DataSet dsParams);
        /// <summary>
        /// （电池片/组件）不良操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLDEFECT "/>和<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT"/>。
        /// </remarks>
        /// <param name="dsParams">包含不良信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotDefect(DataSet dsParams);
        /// <summary>
        /// 电池片补片操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH"/>。
        /// </remarks>
        /// <param name="dsParams">包含补片信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotPatch(DataSet dsParams);
        /// <summary>
        /// 电池片回收操作。用于撤销电池片报废和电池片补片。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RECOVERED"/>。
        /// </remarks>
        /// <param name="dsParams">包含回收信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotRecovered(DataSet dsParams);
        /// <summary>
        /// 批次调整操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次调整信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotAdjust(DataSet dsParams);
        /// <summary>
        /// 批次暂停操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次暂停信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotHold(DataSet dsParams);
        /// <summary>
        /// 批次暂停操作。仅用于服务器端调用。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD"/>。
        /// </remarks>
        /// <param name="dbTrans">数据库操作事务对象。</param>
        /// <param name="dsParams">包含批次暂停信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        void LotHold(DbTransaction dbTrans, DataSet dsParams);
        /// <summary>
        /// 批次释放操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次释放信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotRelease(DataSet dsParams);
        /// <summary>
        /// 批次线别调整。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_LINE"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次线别调整信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotExchangeLine(DataSet dsParams);
        /// <summary>
        /// 批次线别调整操作。仅用于服务器端调用。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_LINE"/>。
        /// </remarks>
        /// <param name="dbTrans">数据库操作事务对象。</param>
        /// <param name="dsParams">包含批次线别调整信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        void LotExchangeLine(DbTransaction dbTrans, DataSet dsParams);
        /// <summary>
        /// 批次(返工/返修)操作。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次（返工/返修）信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotRework(DataSet dsParams);
        /// <summary>
        /// 退料操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL"/>。
        /// </remarks>
        /// <param name="dsParams">包含退料信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotReturnMaterial(DataSet dsParams);
        /// <summary>
        /// 进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含进站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotTrackIn(DataSet dsParams);
        /// <summary>
        /// 出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// </remarks>
        /// <param name="dsParams">包含出站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotTrackOut(DataSet dsParams);
        /// <summary>
        /// 批次结束操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次结束信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet LotTerminal(DataSet dsParams);
        /// <summary>
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        DataSet GetPalletInfo(string palletNo);
        /// <summary>
        /// 根据托盘号获取托盘上的批次信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含批次信息的数据集对象。</returns>
        DataSet GetPalletLotInfo(string palletNo);
        /// <summary>
        /// 托盘进站。
        /// </summary>
        /// <param name="dsParams">包含托盘进站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet PalletTrackIn(DataSet dsParams);
        /// <summary>
        /// 托盘出站。
        /// </summary>
        /// <param name="dsParams">包含托盘出站信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet PalletTrackOut(DataSet dsParams);
        /// <summary>
        /// 托盘入库作业。
        /// </summary>
        /// <param name="dsParams">包含托盘入库信息的数据集对象。</param>
        /// <returns>包含结果数据的数据集对象。</returns>
        DataSet PalletToWarehouse(DataSet dsParams);
        /// <summary>
        /// 获取批次最后一笔操作记录。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次最后一笔操作记录的数据集对象。</returns>
        DataSet GetLotLastestActivity(string lotNumber);
        /// <summary>
        /// 批次撤销操作。
        /// </summary>
        /// <param name="dsParams">包含撤销操作信息的数据集对象。</param>
        /// <returns>
        /// 包含结果数据的数据集对象。
        /// </returns>
        DataSet LotUndo(DataSet dsParams);
        /// <summary>
        /// 批次批量进站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次进站信息的数据集对象。</param>
        /// <returns>0:进站成功。1:记录过期。2:需要抽检。-1:进站失败。</returns>
        DataSet LotBatchTrackIn(IList<DataSet> lstTrackInData);
        /// <summary>
        /// 批次批量出站操作，根据操作名称(<see cref="WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY"/>)区分。
        /// </summary>
        /// <remarks>
        /// 操作名称：<see cref="ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT"/>。
        /// </remarks>
        /// <param name="dsParams">包含批次出站信息的数据集对象。</param>
        DataSet LotBatchTrackOut(IList<DataSet> lstTrackInData,DataTable dtStepParam,int flag);
        /// <summary>
        /// 批次包装作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet LotPackage(DataSet dsParams);
        /// <summary>
        /// 根据托盘号获取包装数据。
        /// </summary>
        /// <returns>包含包装及其明细数据的数据集对象。</returns>
        DataSet GetPackageData(string packageNo);
        /// <summary>
        /// 根据批次号获取包装数据。
        /// </summary>
        /// <returns>包含包装及其明细数据的数据集对象。</returns>
        DataSet GetPackageDataByLotNo(string val);
        /// <summary>
        /// 获取包装批次数据。
        /// </summary>
        /// <param name="htParams">
        /// 允许参数使用的键值。
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_CUSTOMERCODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_SIDECODE"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_LOT_NUMBER"/>
        /// <see cref="POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY"/>
        /// </param>
        /// <returns>包含包装批次数据的数据集对象。</returns>
        DataSet GetPackageLotInfo(Hashtable htParams);
        /// <summary>
        /// 根据批次号获取满包数量。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>满包数量。</returns>
        int GetPackageFullQty(string lotNumber);
        /// <summary>
        /// 获取包装批次对应产品的平均功率控制数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含包装批次对应产品的平均功率控制数据的数据集对象。</returns>
        DataSet GetPackageAvgPowerRangeData(string lotNumber);
        /// <summary>
        /// 获取批次功率分档和子分档数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次功率分档和子分档数据的数据集对象。</returns>
        DataSet GetLotPowersetData(string lotNumber);
        /// <summary>
        /// 根据批次号获取包装混包规则数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含混包规则数据的数据集对象。</returns>
        DataSet GetPackageMixRule(string lotNumber);
        /// <summary>
        /// 根据等级代码获取等级名称。
        /// </summary>
        /// <param name="gradeCode">等级代码。</param>
        /// <returns>等级名称。</returns>
        string GetGradeName(string proLevel);
        /// <summary>
        /// 根据分档主键获取分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="psKey">分档主键。</param>
        /// <returns>包含分档数据的数据集对象。</returns>
        DataSet GetWOProductPowersetData(string orderNumber, string partNumber, string psKey);
        /// <summary>
        /// 批次拆包作业。
        /// </summary>
        /// <param name="dsParams">包含包装数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet LotUnpack(DataSet dsParams);
        /// <summary>
        /// 批次入库检验作业。
        /// </summary>
        /// <param name="dsParams">包含包装检验数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet LotToWarehouseCheck(DataSet dsParam);
        /// <summary>
        /// 检查是否超过固化时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过固化周期。false:没有超过固化周期。</returns>
        bool CheckFixCycle(string lotNumber, string lineKey, string proId, DateTime waitTrackInTime);
        /// <summary>
        /// 检查是否超过恒温时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="lineKey">生产线主键。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="waitTrackInTime">等待进站时间。</param>
        /// <returns>true:超过恒温周期。false:没有超过恒温周期。</returns>
        bool CheckConstantTemperatureCycle(string lotNumber, string lineKey, string proId, DateTime waitTrackInTime);
        /// <summary>
        /// 检查是否超过校准板时间周期。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <param name="deviceCode">设备代码。</param>
        /// <returns>true:超过校准板时间周期。false:没有超过校准版周期。</returns>
        bool CheckCalibrationCycle(string lotNumber, string proId, string deviceCode);
        /// <summary>
        /// 检查流程卡。
        /// </summary>
        /// <returns>true:成功;false:失败。</returns>
        bool CheckProcessCard(string lotNumber);
        /// <summary>
        /// 获取工单产品数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单及其产品数据的数据集对象。</returns>
        DataSet GetWoProductData(string orderNo);
        /// <summary>
        /// 返工单作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet LotExchange(DataSet dsParams);
        /// <summary>
        /// 根据托盘号和批次号获取返工数据。
        /// </summary>
        /// <remarks>
        /// 只输入托盘号，根据托盘号获取返工明细数据。
        /// 只输入批次号，根据批次号获取返工明细数据。
        /// 同时输入托盘号和批次号，获取返工明细数据。
        /// </remarks>
        /// <returns>包含返工明细数据的数据集对象。</returns>
        DataSet GetExchangeData(string packageNo, string lotNo);
        /// <summary>
        /// 获取组件对应的工艺流程中组件所在工序的属性信息
        /// </summary>
        /// <param name="lotNo">组件序列号</param>
        /// <returns>工序属性信息</returns>
        DataSet GetLotRouteAttrData(string lotNo);
        /// <summary>
        /// 比对等级是否符合工单和产品的要求。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>true：符合;false:不符合。</returns>
        bool CompareExchangeGrade(string newOrderNumber, string newPartNumber, string grade);
        /// <summary>
        /// 入库检验返回到上一工序作业。
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet LotToWarehouseCheckReject(DataSet dsParam);
        /// <summary>
        /// 是否检查组件的IV测试数据。
        /// </summary>
        /// <param name="lotNumber">组件序列号。</param>
        /// <returns>true：检查。false：不检查。</returns>
        bool IsCheckIVTestData(string lotNumber);
        /// <summary>
        /// 获取工单检验允许的等级代码。符合的等级在终检将不需要输入不良和备注。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>工单检验允许的等级代码。多个等级使用逗号（,）分隔。</returns>
        string GetWOCheckAllowGrade(string lotNumber, out string gradeName);
        /// <summary>
        /// 根据批次号生成新的托号
        /// </summary>
        /// <param name="_lotNumber">批次号。</param>
        /// <returns>包含新托盘号的数据集对象。</returns>
        DataSet NewPalletNo(string _lotNumber);
        /// <summary>
        /// 根据批次号生成新的托号
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>托号</returns>
        string GetNewPalletNum(string lotNum);
        /// <summary>
        /// 获取托状态 
        /// </summary>
        /// <param name="palletNo">托号</param>
        /// <returns>包含执行结果的数据集对象。托状态</returns>
        DataSet GetPalletStatus(string palletNo);


        /// <summary>
        /// 判断托盘号是否已存在。yibin.fei 2018.03.13
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        bool SurePalletNo(string PalletNo);
        /// <summary>
        /// 判断是否是se客户
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>true or false</returns>
        bool GetOutLotForSe(string lotNum);
        /// <summary>
        /// SE客户自动生成托号
        /// </summary>
        /// <param name="lotNum"></param>
        /// <returns></returns>
        DataSet NewSEPalletNo(string lotNum);
 
        /// <summary>
        /// 根据工单主键，物料代码
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料代码</param>
        /// <returns>数据集</returns>
        DataSet GetWorkOrderBomByMat(string workOrderKey,string mat);

        /// <summary>
        /// 根据工步主键，工厂主键，设备主键，工序名称，工单号，采集类型获取参数信息以及对应设备虚拟仓数量
        /// </summary>
        /// <param name="stepKey">工步主键</param>
        /// <param name="roomKey">工厂主键</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <param name="operationName">工序名称</param>
        /// <param name="workorder">工单号</param>
        /// <param name="dcType">采集类型</param>
        /// <returns>结果集</returns>
        DataSet GetStepParams(string stepKey, string roomKey, string equipmentKey, string operationName, string workorder, int dcType);
        /// <summary>
        /// 根据参数获取参数组
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <returns></returns>
        DataSet GetParamerTeamName(string paramName);
        /// <summary>
        /// 根据工单主键和mat物料类型获取替代料信息和工单bom信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料类型</param>
        /// <returns></returns>
        DataSet GetWorkOrderBomByWorkKeyAndMat(string workOrderKey, string mat);
        /// <summary>
        /// 根据工单主键和物料类型获取特殊物料数据
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="mat">物料类型</param>
        /// <returns></returns>
        DataSet GetSpecailBomByWorkKeyAndMat(string workOrderKey, string mat);

        /// <summary>
        /// 更新POR_LOT表新工单号字段
        /// </summary>
        /// <param name="newWorkOrder"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        bool UpdatePor_Lot(string newWorkOrder, string Pallet_No);

        /// <summary>
        /// 更新Wip_consignment表新入库单号字段
        /// </summary>
        /// <param name="INWHORDER"></param>
        /// <param name="Pallet_No"></param>
        /// <returns></returns>
        bool UpdateWip_consignment(string INWHORDER, string Pallet_No);

    }
}

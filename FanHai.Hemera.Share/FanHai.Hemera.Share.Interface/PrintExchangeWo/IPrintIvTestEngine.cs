using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IPrintIvTestEngine
    {
        /// <summary>
        /// 根据查询条件获得标签打印信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetIvTestData(Hashtable hstable);

        /// <summary>
        /// 获取指定类型标签信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetLabelInfo(string strFilter);
        /// <summary>
        /// 获取指定类型标签信息
        /// </summary>
        /// <param name="type">L:标签；P：铭牌</param>
        /// <returns>包含标签铭牌数据的数据集对象。</returns>
        DataSet GetLabelData(string type);
        /// <summary>
        /// 获取指定组件信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPorLotInfo(string sSN);
        /// 获取批次测试记录。
        /// </summary>
        /// <param name="sSN">组件序列号。</param>
        /// <param name="sDefault">是否只获取当前有效记录， 空：否， 1：是。 </param>
        /// <returns>包含批次测试记录的数据集对象。</returns>
        DataSet GetIVTestDateInfo(string sSN,string sDefault);

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPorProductData(string sProductCode);
        
        /// <summary>
        /// 获取产品设定的打印标签信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPrintLabelSetInfo(string sProductCode);

        /// <summary>
        /// 获取批次标签打印记录。
        /// </summary>
        /// <param name="sSN">批次号。</param>
        /// <param name="sLabelNo">标签号。</param>
        /// <returns>包含标签打印记录的数据集对象。</returns>
        DataSet GetPrintLabelLogData(string sSN, string sLabelNo);

        /// <summary>
        /// 获取校准版测试时间。
        /// </summary>
        /// <param name="sSN">校准版序列号。</param>
        /// <param name="sDeviceNo">设备号。</param>
        /// <returns>包含校准版测试时间的数据集对象。</returns>
        DataSet GetCalibrationMaxTTime(string sSN, string sDeviceNo);

        /// <summary>
        /// 获取产品对应的测试规则信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetTestRuleData(string sProductCode);

        /// <summary>
        /// 获取产品对应的衰减系数信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetDecoeffiData(string sProductCode, string sCoeffCode, string sPM);

        /// <summary>
        /// 获取产品型号信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetProductModelData(string sProductCode);

        /// <summary>
        /// 获取测试规则对应的控制参数信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetTestRuleCtlParaData(string sProductCode);

        /// <summary>
        /// 获取功率分档信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        //DataSet GetPowerSetData(string sProductCode, string sPM, string sPSSeq);

        /// <summary>
        /// 获取子功率分档信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPowerSetDetailData(string sProductCode, string sPM, string sPSSeq, string sPSDSeq);

        /// <summary>
        /// 获取功率分档控制信息
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetTestRulePowerCtlData(string sProductCode, string sPM, string sPSSeq, string sSeq);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet UpdateData(string sql, string sUpFuntionName);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet AddData(string sql, string sUpFuntionName);

        /// <summary>
        /// 查询测试数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetIVTestData2(string sWorkNum, string sStartSN, string sEndSN, string sStartDevice, string sEndDevice, string sStartDate, string sEndDate, string sDefault, string sVC_CONTROL);

        /// <summary>
        /// 包装清单Conergy数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListConergyData(string sPalltNo);

        /// <summary>
        /// 包装清单Schueco数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListSchuecoData(string sPalltNo);

        /// <summary>
        /// 包装清单Japan数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListJapanData(string sPalltNo, string sCINumber);

        /// <summary>
        /// 包装清单Common数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListCommonData(string sPalltNo);

        /// <summary>
        /// 包装清单Common For TST数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListCommonDataSTS(string sPalltNo);

         /// <summary>
        /// 包装清单Common For 青岛昌盛数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListCommonDataQDCS(string sPalltNo);

        /// <summary>
        /// 包装清单Common For 特变埃及数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPackingListCommonDataAiji(string sPalltNo);
        /// <summary>
        /// PPS清单主数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPPSMasterData(string sPalltNo);
        DataSet GetPortMark(string sPalltNo);

        /// <summary>
        /// 为体现标签/包装清单功率判断查询 yibin.fei 2017.10.10
        /// </summary>
        /// <param name="sWorkNo"></param>
        /// <param name="sSapNo"></param>
        /// <returns></returns>
        DataSet GetPowerShowData(string sWorkNo, string sSapNo);
       
        /// <summary>
        /// PPS清单汇总数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPPSCollectData(string sPalltNo);

        /// <summary>
        /// Flash Data 出货单数据查询
        /// </summary>
        /// <param name="sPalltNo"></param>
        /// <returns></returns>
        DataSet GetFlashDataDelivery(string sPalltNo);

        /// <summary>
        /// 获得查询数据-针对终检数据的查询
        /// </summary>
        /// <param name="hsParams">参数集合</param>
        /// <returns>返回数据集合</returns>
        DataSet GetIvTestDataForCustCheckQuery(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hsParams);

        /// <summary>
        /// 获取要导出终检数据信息
        /// </summary>
        /// <param name="hsParams">选择的参数信息集合</param>
        /// <returns></returns>
        DataSet GetIvTestToCustCheckQueryForImport(Hashtable hsParams);

        /// <summary>
        /// 终检数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetCustCheckData(string sSN, string sCustCode, string sRoomKey);

        /// <summary>
        /// 组件包装数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetConsigmentDataBySN(string sSN, string sSideCode, string sCustomerCode, string sRoomKey);

        /// <summary>
        /// 基础数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetBasicData(string sColumnType, string sColumnCode, string sColumnName, string sColumnNameDesc);

        /// <summary>
        /// 不良类型数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetReasonCategoryData(string sCategoryType, string sCategoryName);

        /// <summary>
        /// 不良代码数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetReasonData(string sCategoryKey);

        /// <summary>
        /// OQA数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetOQAData(string sSN, string sCustomCode, string sRoomKey);

        /// <summary>
        /// 终检记录序号查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetCustCheckSEQ(string sSN, string sCustomCode, string sRoomKey);

        /// <summary>
        /// 厂区信息查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetFactoryInfo();

        /// <summary>
        /// OQAReport数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetOQADataReport(string sFactoryKey, string sSNType, string sDefault, string sDateFalg, string sStartSN, string sEndSN, string sWO, string sPROID, string sStartDate, string sEndDate);
        /// <summary>
        /// 根据批次或产品ID号，功率档等信息获得分档数据
        /// </summary>
        /// <param name="lotNum"></param>
        /// <param name="sProductCode"></param>
        /// <param name="sPM"></param>
        /// <param name="sPSSeq"></param>
        /// <returns></returns>
        DataSet GetPowerSetData(string lotNum, string sProductCode, string sPM, string sPSSeq);
        /// <summary>
        /// 根据批次、工单属性类别获取工单设定属性
        /// </summary>
        /// <param name="sLotNum"></param>
        /// <param name="sType"></param>
        /// <returns></returns>
        DataSet GetWOAttributeValueByLotNum(string sLotNum, string sType);
        /// <summary>
        /// 按产品ID号获取电流子分档
        /// </summary>
        /// <param></param>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPowerSetDetailDataByIMP(string sProductCode, string sPM, string sIMP, string sPSSeq, string sPSDSeq);
        /// <summary>
        /// 按托号获取电流子分档
        /// </summary>
        /// <param></param>
        /// <param></param>
        /// <returns></returns>
        DataSet dsGetConergyPackgeData2(string sPalletNo);
        /// <summary>
        /// 获取图片的存储路径
        /// </summary>
        /// <param></param>
        /// <param></param>
        /// <returns></returns>
        DataSet dsGetPicPath(string sFactoryCode, string sPicType);
        /// <summary>
        /// 获取CodeSoft标签设定信息
        /// </summary>
        /// <param></param>
        /// <param></param>
        /// <returns></returns>
        DataSet GetCodeSoftLabelSet(string sLabelID);
        /// <summary>
        /// PPSIMP清单主数据查询
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        DataSet GetPPSMasterImpData(string sPalltNo);
        /// <summary>
        /// 是否重新计算衰减数据。
        /// </summary>
        /// <remarks>已经过入库检验，则不允许进行衰减数据的重新计算。</remarks>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：是。false：否</returns>
        bool IsRecalcDecayData(string lotNumber);
        /// <summary>
        /// 根据工单号获取工单设置的产品基本规则数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单产品基本规则数据的数据集对象。</returns>
        DataSet GetWoProductData(string orderNumber);
        /// <summary>
        /// 根据工单号、产品料号和功率获取对应的产品衰减系数。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="pm">功率。</param>
        /// <returns>包含工单产品衰减系数数据的数据集对象。</returns>
        DataSet GetDecayCoefficient(string orderNumber, string productId, decimal pm);
        /// <summary>
        /// 根据工单号、产品料号和衰减后功率获取对应的分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="lotNo">组件批次号。</param>
        /// <param name="coefPM">衰减后功率。</param>
        /// <returns>包含对应分档数据的数据集对象。</returns>
        DataSet GetWOPowerSetData(string orderNumber, string partNumber, string lotNo, decimal coefPM);
        /// <summary>
        /// 根据工单主键、产品料号、分档主键和衰减后数据获取对应的子分档数据。
        /// </summary>
        /// <param name="workOrderKy">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <param name="val">根据子分档规则设置不同的值，如果是功率子分档设置为衰减后功率，如果是电流子分档设置为衰减后电流。</param>
        /// <returns>包含对应子分档数据的数据集对象。</returns>
        DataSet GetWOPowerSetDetailData(string workOrderKy, string partNumber, string powersetKey, decimal val);

        /// <summary>
        /// 根据工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间
        /// </summary>
        /// <param name="workOrderKy">工单主键</param>
        /// <param name="partNumber">产品料号</param>
        /// <param name="powersetKey">分档主键</param>
        /// <returns>工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间数据</returns>
        DataSet GetWOPowerSetDetailDataRang(string workOrderKy, string partNumber, string powersetKey);

        /// <summary>
        /// 根据批次号获取有效的IV测试数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <returns>包含批次数据及其IV测试数据的数据集对象。</returns>
        DataSet GetIVTestData(string lotNo);
        /// <summary>
        /// 根据工单主键、产品料号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        DataSet GetWOPrintLabelData(string workOrderKey, string partNumber);
        /// <summary>
        /// 获取工单已生产的产品数量。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <returns>产品数量。</returns>
        decimal GetWOProductPowersetQty(string workOrderKey, string partNumber, string powersetKey);
        /// <summary>
        /// 保存打印数据。
        /// </summary>
        /// <param name="dsParams">包含打印数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SavePrintData(DataSet dsParams);
        /// <summary>
        /// 根据工单号、产品料号、产品ID号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="productId">产品ID号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        DataSet GetWOPrintLabelDataByNo(string workOrderNo,string partNumber, string productId);
        /// <summary>
        /// 是否允许打印标签。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <param name="msg">提示信息。如果允许则返回空字符串。否则返回对应的提示信息。</param>
        /// <returns>true：允许；false：不允许。</returns>
        bool IsAllowPrintLabel(string lotNumber, out string msg);

        /// <summary>
        /// 通过信息获取对应的SunEdison的信息
        /// </summary>
        /// <param name="hsSunEdison">查询条件信息</param>        
        /// <returns>通过信息获取对应的SunEdison的信息</returns>
        DataSet GetSunEdisonList(Hashtable hsSunEdison);
        /// <summary>
        /// 通过批次号获取功率范围
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>功率信息</returns>
        DataSet GetPowerRangeDate(string lotNum);
        /// <summary>
        /// 通过工单主键，效率下限，效率上限，Ctm值获取对应工单设定的符合的ctm信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="lowCelleff">低效率</param>
        /// <param name="highCelleff">高效率</param>
        /// <param name="ctm">实际ctm值</param>
        /// <returns>符合要求的ctm数据集</returns>
        DataSet GetCtmInfByWorderEffCtm(string workOrderKey, decimal lowCelleff, decimal highCelleff, decimal ctm);
        /// <summary>
        ///  根据工单获取效率
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns></returns>
        DataSet GetInefficientPARAM(string lotNumber); 

        /// <summary>
        /// 通过产品类型获取效率
        /// </summary>
        /// <param name="procode"></param>
        /// <returns></returns>
        DataSet GetProductCp(string procode, string conopj);

        /// <summary>
        /// 组件线下打印pps清单
        /// </summary>
        /// <param name="sPalltNo"></param>
        /// <returns></returns>
        DataSet GetPPSMasterDataForMalai(string sPalltNo);

        DataSet GetPPSMalai(string sPalltNo);

        DataSet GetLotNumMalai(string lotNo);

        DataSet GetLotNumsMalai(string palletNo);
        /// <summary>
        ///根据托号获取distinct的Color 判定花色是否混，主要用于金刚线和非金刚线的深和浅的问题
        ///金刚线组件：浅花、深花或者混花色（一个单托既有浅花又有深花，则为混花色）。
        ///非金刚线组件：浅蓝、深蓝或者混色（一个单托既有浅蓝又有深蓝，则为混色）。
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>distinct Color 的数据集</returns>
        DataSet GetColorData(string palletNo);
        /// <summary>
        /// 根据托号获取金刚线
        /// </summary>
        /// <param name="PalletNo">托号</param>
        /// <returns></returns>
        DataSet GetKingLineByPallet(string PalletNo);
    }
}

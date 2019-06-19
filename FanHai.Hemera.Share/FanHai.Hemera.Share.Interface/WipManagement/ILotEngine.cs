using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 批次数据的操作接口。
    /// </summary>
    public interface ILotEngine
    {
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        DataSet SearchLotList(DataSet dsParams);
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="config">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        DataSet SearchLotList(DataSet dsParams, ref PagingQueryConfig config);
        /// <summary>
        /// 查询包含批次信息的数据集。
        /// </summary>
        /// <param name="dsSearch">
        /// 包含查询条件的数据集。
        /// </param>
        /// <param name="config">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含批次信息的数据集。</returns>
        DataSet SearchLotList(DataSet dsParams, ref PagingConfig config);
        /// <summary>
        /// 根据批次主键获取批次的历史操作记录
        /// </summary>
        /// <param name="lotKEY">批次主键。</param>
        /// <returns>
        /// 查询得到的包含批次操作信息的数据集对象。
        /// </returns>
        DataSet GetInfoForLotHistory(object lotKey);
        /// <summary>
        /// 为批次号打印获取批次信息。
        /// </summary>
        /// <param name="dataTable">包含查询条件的数据表。键值对数据表。
        /// 键值<see cref="POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER"/>,
        /// <see cref="POR_LOT_FIELDS.FIELD_IS_PRINT"/>,<see cref="POR_LOT_FIELDS.FIELD_IS_REWORKED"/>
        /// </param>
        /// <returns>包含批次信息的数据集。
        /// [LOT_KEY,LOT_NUMBER,IS_PRINT,REWORK_FLAG,QUANTITY,ORDER_NUMBER]
        /// </returns>
        DataSet GetLotNumberForPrint(DataTable dataTable);
        /// <summary>
        /// 更新批次号打印标记，使打印数量+1
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        void UpdatePrintFlag(string lotNumber);
        /// <summary>
        /// 批量更新批次号码打印数量。
        /// </summary>
        /// <param name="lotNumber">批次号集合。</param>
        void UpdatePrintFlag(IList<string> lotNumbers);
        /// <summary>
        /// 根据批次号获取批次信息。
        /// </summary>
        /// <param name="lotNo">批次号或批次主键。</param>
        /// <returns>包含批次信息的数据集。</returns>
        DataSet GetLotInfo(string lotNo);
        /// <summary>
        /// 根据批次号获取批次报废不良信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>包含批次报废不良信息的数据集。
        /// [LOT_NUMBER,ROUTE_OPERATION_NAME,SCRAP_QUANTITY,DEFECT_QUANTITY]
        /// </returns>
        DataSet GetScrapAndDefectQty(string lotKey);
        /// <summary>
        /// 根据批次主键获取批次工序参数信息。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次工序参数信息的数据集。
        /// </returns>
        DataSet GetParamInfo(string lotKey);
        /// <summary>
        /// 保存终检过账信息
        /// </summary>
        /// <param name="dsParam">终检过账信息表</param>
        /// <returns>数据保存成功之后，才能进入下一步数据采集的作业画面，否则提示信息，仍停留在当前画面。</returns>
        DataSet SaveLot2CustCheckData(DataSet dsParam);
        /// <summary>
        /// 根据批次号或者批次ID获得工单号，工单ID和产品ID号信息
        /// </summary>
        /// <param name="s_lot"></param>
        /// <returns>Data Columns:
        /// ORDER_NUMBER,WORK_ORDER_KEY,PRODUCT_CODE,PRODUCT_KEY,PRODUCT_NAME,
        /// LOT_KEY,LOT_NUMBER,PALLET_NO,PRO_LEVEL,PALLET_TIME,FACTORYROOM_KEY,LINE_NAME</returns>
        DataSet GetWOProductByLotNum(string s_lot, string roomkey);
        /// <summary>
        /// 根据工单号获取工单对应的属性信息
        /// </summary>
        /// <param name="orderNumber">工单号</param>
        /// <returns>工单对应的属性信息</returns>
        DataSet GetOrderAttrByOrderNumber(string orderNumber);
        /// <summary>
        /// 获取批次档位名称及其子分档名称。
        /// </summary>
        /// <param name="Lot_Number"></param>
        /// <returns></returns>
        DataSet GetModulePowerInfo(string Lot_Number);
        /// <summary>
        /// 获得服务端时间，防止PC时间被修改，而导致客户端时间判断有误
        /// </summary>
        /// <returns>数据库当前时间</returns>
        DateTime GetSysdate();
        /// <summary>
        /// 判断终检数据是否已经存在，如果存在则获得终检的数据
        /// </summary>
        /// <param name="s_lot"></param>
        /// <returns></returns>
        DataSet GetCustCheckDataGroupByZero(string s_lot, string roomkey);
        /// <summary>
        /// 获得包装数据
        /// </summary>
        /// <param name="s_lot"></param>
        /// <param name="roomkey"></param>
        /// <returns>1，返回工单产品信息表；
        /// 2，返回分档数据表；
        /// 3，返回分档明细表</returns>
        DataSet GetLotPorWOForPallet(Hashtable hstable);
        /// <summary>
        /// 检查并获得托盘号信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetPalletOrLotData(Hashtable hstable);
        /// <summary>
        /// 保存包装托盘信息
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet SavePalletLotData(DataSet dsParams);
        /// <summary>
        /// 出托作业
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet UpdateLotOutPallet(Hashtable hstable);
        /// <summary>
        /// 判断批次和托号是否存在
        /// </summary>
        /// <param name="hsParams"></param>
        /// <returns></returns>
        DataSet IsExistPalletLotNum(Hashtable hsParams);
        /// <summary>
        /// 获得入库检数据
        /// </summary>
        /// <param name="hsParams"></param>
        /// <returns></returns>
        DataSet GetToWarehouseCheckData(Hashtable hsParams);
        /// <summary>
        /// 保存入库检数据
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns> 
        DataSet SaveToWarehouseCheckData(DataSet dsSave);
        /// <summary>
        /// 查询包装，终检，批次信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetPalletCustLotData(Hashtable hstable);
        /// <summary>
        /// 测试规则中-产品等级判定
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetTestRulePackageLevel(Hashtable hstable);
        /// <summary>
        /// 从入库检返回到包装
        /// </summary>
        /// <param name="dsPamas"></param>
        /// <returns></returns>
        DataSet SavePallet2Package(DataSet dsPamas);
        /// <summary>
        /// 判定组件图片是否存在（入库检）
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet IsExistModulePic(Hashtable hstable);
        /// <summary>
        /// 托号变更
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        DataSet SaveExchgPalletNumber(Hashtable hstable);
        /// <summary>
        /// 获得批次工单子属性，产品ID属性
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetLotWoProAttribute(Hashtable hstable);
        /// <summary>
        /// 根据厂别获取组件录入卡控信息
        /// </summary>
        /// <param name="sFactoryName"></param>
        /// <returns></returns>
        DataSet GetCheckbarcodeInputType(string sFactoryName);
        /// <summary>
        /// 根据批次号及指定等级。获取批次对应工单中符合指定等级条件的推荐产品数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>包含产品数据的数据集对象。</returns>
        DataSet GetLotProductData(string lotNo, string grade);
        /// <summary>
        /// 根据组件序列号获取组件对应的铭牌检查类型
        /// </summary>
        /// <param name="LotNo">组件序列号</param>
        /// <returns>组件对应的检查类型的数据集</returns>
        DataSet GetLotCustCheckType(string lotNo);
        
        /// <summary>
        /// 根据托盘号获取批次信息。
        /// </summary>
        /// <param name="pallet_no">托盘号</param>
        /// <returns>包含批次信息的数据集</returns>
        DataSet GetLotInfoByPallet_No(string pallet_no);
    }
}

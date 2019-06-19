using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 创建批次的接口。
    /// </summary>
    public interface ILotCreateEngine
    {
        /// <summary>
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        DataSet GetProdId();
        /// <summary>
        /// 获取产品ID号对应的电池片数量。
        /// </summary>
        /// <param name="proId">产品ID号。</param>
        /// <returns>电池片数量。</returns>
        double GetCellNumber(string proId);
        /// <summary>
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="orderNo">工单号。</param>
        /// <param name="proId">产品ID号。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        DataSet GetReceiveMaterialInfo(string roomKey, string operationName, string orderNo, string proId);
        /// <summary>
        /// 获取领料项目信息。
        /// </summary>
        /// <param name="val">原材料线上仓存储明细主键。</param>
        /// <returns>包含领料项目信息的数据集对象。</returns>
        DataSet GetReceiveMaterialInfo(string val);
        /// <summary>
        /// 根据车间主键获取工单号。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。
        /// 【工单主键,ORDER_NUMBER（工单号）,产品ID号，成品编码，成品主键】
        /// </returns>
        DataSet GetWorkOrderByFactoryRoomKey(string roomKey);
        /// <summary>
        /// 创建批次。
        /// </summary>
        /// <param name="dsParams">包含创建批次所需信息的数据集对象。</param>
        /// <returns>包含创建批次结果的数据集对象。</returns>
        DataSet CeateLot(DataSet dsParams);

        /// <summary>
        /// 根据产品ID获取产品类型。
        /// </summary>
        /// <param name="sProductCode">产品ID。</param>
        /// <returns>
        /// 产品类型的数据集合。
        /// </returns>
        DataSet GetProductModeByPID(string sProductCode);

        DataSet GetOrderNoType(string OrderNo);


        /// <summary>
        /// 根据小包条码获取组件对应电池片的对应信息    add by  yongbing.yang 20130806
        /// </summary>
        /// <param name="Small_Pack_Number">小包条码</param>
        /// <returns>包含电池片的详细信息</returns>
        DataSet GetCellInformation(string Small_Pack_Number);

        /// <summary>
        /// 获取工单BOM信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        DataSet GetWorkOrderBom(string orderNo);
        /// <summary>
        /// 获取外购电池片供应商信息
        /// </summary>
        /// <param name="smallPackNumber">电池片条码</param>
        /// <returns>数据集</returns>
        DataSet GetOutCellSupplier(string smallPackNumber);
        /// <summary>
        /// 传入工单主键获取工单中的规则信息创建除流水号之外的组件序列号
        /// </summary>
        /// <param name="orderKey">工单主键</param>
        /// <returns>主键序列号的数据集</returns>
        DataSet CreateLotNumByRule(string orderKey,int count);
        /// <summary>
        /// 批次创建。自动生成组件序列号，设定单串焊线别设备
        /// </summary>
        /// <param name="dsParams">创批的数据集信息</param>
        /// <returns>包含创建批次结果的数据集对象</returns>
        DataSet CeateNewLot(DataSet dsParams);

        DataSet GetAnNeng(string ORDER_NUMBER);
    }
}

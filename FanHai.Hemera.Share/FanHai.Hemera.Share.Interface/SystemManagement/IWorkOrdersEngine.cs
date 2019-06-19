using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 工单数据操作接口。
    /// </summary>
    public interface IWorkOrdersEngine
    {
        /// <summary>
        /// 根据工单主键获取工单信息。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <returns>包含工单信息的数据集。</returns>
        DataSet GetWorkOrderByKey(string workOrderKey);
        /// <summary>
        /// 根据工单号获取工单主键。
        /// </summary>
        /// <param name="pWorkOrder">工单号。</param>
        /// <returns>包含工单主键信息的数据集。</returns>
        DataSet GetWorkOrderKeyByOrder(string pWorkOrder);
        /// <summary>
        /// 根据工厂车间名称获取工单号
        /// </summary>
        /// <param name="factoryroom">车间名称</param>
        /// <returns>包含工单号信息的数据集。</returns>
        DataSet GetWorkOrderByFactoryRoom(string roomName);
        /// <summary>
        /// 根据车间主键获取工单号。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。
        /// 【ORDER_NUMBER（工单号）】
        /// </returns>
        DataSet GetWorkOrderByFactoryRoomKey(string roomKey);
        /// <summary>
        /// 根据工单号获取成品料号信息。
        /// </summary>
        /// <param name="orderNum">工单号。</param>
        /// <returns>包含成品数据的数据集[PART_NAME, PART_DESC,PART_TYPE]</returns>
        DataSet GetPartBytWorkOrder(string orderNum);
        /// <summary>
        /// 工单清单。
        /// </summary>
        /// <param name="pconfig">分页设置对象。</param>
        /// <returns>包含工单数据的数据集。</returns>
        DataSet GetWorkOrderInfoList(ref PagingQueryConfig config);
        /// <summary>
        /// 根据条件获取工单数据。
        /// </summary>
        /// <param name="strFactory">工厂名称。</param>
        /// <param name="strWorkOrderNo">工单号。左匹配。</param>
        /// <param name="strPart">成品料号。左匹配。</param>
        /// <param name="strType">成品类型。</param>
        /// <param name="strStore">入库库位。</param>
        /// <param name="strStatus">状态。</param>
        /// <param name="pconfig">分页设置对象。</param>
        /// <returns>包含工单数据的数据集。</returns>
        DataSet GetWorkOrderByCondition(string strFactory, string strWorkOrderNo, string strPart,
                                        string strType, string strStore, string strStatus, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 获取指定工步采集参数对应的工单参数设定数据。用于比对在采集时输入的数据是否符合工单参数设定。
        /// </summary>
        /// <param name="workorderKey">工单主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dcType">数据采集时刻。0:进站时采集 1：出站时采集</param>
        /// <returns>包含工单参数设定数据的数据集对象。</returns>
        DataSet GetWorkOrderParam(string workorderKey, string stepKey, int dcType);
        /// <summary>
        /// 根据工单号或者pro_id号查询工单信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetWorkOrderByNoOrProid(Hashtable hstable);
        /// <summary>
        /// 根据工单ID获得工单属性设置的信息
        /// </summary>
        /// <param name="workorderkey"></param>
        /// <returns></returns>
        DataSet GetWorkOrderAndAttrParamByKey(string workorderkey);
        /// <summary>
        /// 根据工单号获得工单属性设置的信息
        /// </summary>
        /// <param name="workorderkey"></param>
        /// <returns></returns>
        DataSet GetWorkOrderAttrParamByOrderNumber(string workorderNumber);
        /// <summary>
        /// 获取所有工单信息
        /// </summary>
        /// <returns></returns>
        DataSet GetAllWorkOrderData();
        /// <summary>
        /// 获取产品ID号数据。
        /// </summary>
        /// <returns>包含产品ID号数据的数据集对象。</returns>
        DataSet GetProdId();
        /// <summary>
        /// 获取产品料号数据。
        /// </summary>
        /// <returns>包含产品料号数据的数据集对象。</returns>
        DataSet GetPartNumber();
        /// <summary>
        /// 获取物料编码数据。
        /// </summary>
        /// <returns>包含物料编码数据的数据集对象。</returns>
        DataSet GetMaterialCode();
        /// <summary>
        /// 根据工单号获取工单信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单信息的数据集对象。</returns>
        DataSet GetWorkorderInfo(string orderNumber);
        /// <summary>
        /// 保存工单数据。
        /// </summary>
        /// <param name="dsParam">包含工单信息的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet Save(DataSet dsParam);
        /// <summary>
        /// 保存工单设置属性信息
        /// </summary>
        /// <param name="dsWorkAttrParam"></param>
        /// <returns></returns>
        DataSet SaveWorkOrderAttrParam(DataSet dsWorkAttrParam);
        /// <summary>
        /// 物理删除工单属性
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet DelWorkAttrDataBy2Key(Hashtable hstable);
        /// <summary>
        /// 根据工单号和工单属性值，获取工单属性的设定值
        /// </summary>
        /// <param name="workorder"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        DataSet GetWorkOrderAttributeValue(Hashtable hsParams);
        /// <summary>
        /// 根据工单号获得客户类别
        /// </summary>
        /// <param name="hsparams"></param>
        /// <returns></returns>
        DataSet GetViewForWorkOrder(Hashtable hsparams);
        /// <summary>
        /// 通过工单主键获取工单产品信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>工单对应的工单产品的表集信息</returns>
        DataSet GetWorkOrderProByOrderKey(string workOrderKey);
        /// <summary>
        /// 功过主产品料号获取对应的料号信息
        /// </summary>
        /// <param name="partNumber">主产品料号</param>
        /// <returns>主产品料号对应的产品料号的表集</returns>
        DataSet GetPartNumberByMainPartNumber(string mainPartNumber);
        /// <summary>
        /// 对工单对应的产品信息进行保存
        /// </summary>
        /// <param name="workOrderProInfo">工单对应的产品信息的集合</param>
        /// <returns>操作结果</returns>
        DataSet SaveWorkOrderProInfo(DataSet workOrderProInfo);

        /// <summary>
        /// 通过分档代码获取对应的分档规则
        /// </summary>
        /// <param name="powerSetCode">分档代码</param>
        /// <returns>分档代码对应的功率分档的集合</returns>
        DataSet GetPowerSetByPowerSetCode(string powerSetCode,string partMinPower,string partMaxPower);

        /// <summary>
        /// 通过工单号或者序列号获取对应工单的OEM信息
        /// </summary>
        /// <param name="orderNumber">托盘对应的工单号</param>
        /// <param name="lotNumber">组件序列号</param>
        /// <returns>工单对应的OEM信息</returns>
        DataSet GetWorkOrderOEMByOrderNumberOrLotNumber(string orderNumber, string lotNumber);

        /// <summary>
        /// 通过工单号获取接线盒信息的集合
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>接线盒信息的集合</returns>
        DataSet GetWOJunctionBox(string workorderNumber);

        /// <summary>
        /// 通过工单号获取工单对应的工艺流程信息
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>工单对应的工艺流程信息</returns>
        DataSet GetWorkOrderRouteInfo(string workorderNumber);

        /// <summary>
        /// 通过工单获取OEM客户信息
        /// </summary>
        /// <param name="workorderNumber">工单信息</param>
        /// <returns>工单对应的OEM信息</returns>
        DataSet GetWorkOrderOEMCustomer(string workorderNumber);

        /// <summary>
        /// 获取工厂线别信息
        /// </summary>
        /// <returns>工厂线别的数据集合</returns>
        DataSet GetFatoryLine();

        /// <summary>
        /// 获取绑定的工厂线别是否正确
        /// </summary>
        /// <returns>True:绑定线别正确、或未绑定线别。False：绑定线别但是所选线别不在绑定范围</returns>
        bool CheckWorkOrderLineBind(string LotNumber, string lineKey);
        /// <summary>
        /// 更新工单产品设置清单打印设置 2018.4.16
        /// </summary>
        /// <param name="WorkOrderNumber"></param>
        /// <returns></returns>
        bool isUpDataPrint(string WorkOrderNumber);

        /// <summary>
        /// 通过LableID获取对应的打印类型
        /// </summary>
        /// <param name="lableID">LableID</param>
        /// <returns>该LableID对应的打印类型</returns>
        string GetLablePrinterType(string lableID);

        /// <summary>
        /// 通过工单号获取工单对应的上下限功率管控值
        /// </summary>
        /// <param name="workOrderNumber"></param>
        /// <returns></returns>
        DataSet GetUpLowRule(string workOrderNumber);
        /// <summary>
        /// 通过产品id获取ctm值
        /// </summary>
        /// <param name="proKey">产品id主键</param>
        /// <returns>ctm上下限管控信息</returns>
        DataSet GetCtmInf(string proKey, string workOrder);
        /// <summary>
        /// 通过工单号获取打印规则内容
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>数据集</returns>
        DataSet GetPrintRuleData(string workOrderKey);
        /// <summary>
        /// 获取产品类型打印编码
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>数据集</returns>
        DataSet GetProductModel();
        /// <summary>
        /// 通过工单号获取EL规则内容
        /// </summary>
        /// <param name="workOrderKey"></param>
        /// <returns></returns>
        DataSet GetElPicRuleData(string workOrderKey,string ruleType);
        /// <summary>
        /// 获取包装清单打印设置
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>数据集</returns>
        DataSet GetFlashAutoPrintData(string workOrderNumber);
        /// <summary>
        /// 保存打印信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        DataSet GetWoPrint(string workOrderNumber);
    }
}

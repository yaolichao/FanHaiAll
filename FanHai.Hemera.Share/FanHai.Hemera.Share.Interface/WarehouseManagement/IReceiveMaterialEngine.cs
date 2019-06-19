using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 来料接收数据的操作接口。
    /// </summary>
    public interface IReceiveMaterialEngine
    {
        /// <summary>
        /// 获取物料明细信息。
        /// </summary>
        /// <param name="sapOperation">SAP工作中心名称，多个工作中心使用逗号分割。C1,C2...</param>
        /// <param name="storeNameList">线上仓名称，多个线上仓使用逗号分割。store1,store2..</param>
        /// <returns>
        /// 包含物料明细信息的数据集对象。
        /// 【W.SAP_ISSURE_KEY,ROWNUM,W.MBLNR, W.MATNR,W.CHARG, W.MATXT, W.AUFNR,W.ERFME,W.ERFMG, W.LLIEF,B.WORK_CENTER】
        /// </returns>
        DataSet GetMaterialDetail(string sapOperation, string storeNameList);
        
        /// <summary>
        /// 通过线上仓名称获取工厂名称。
        /// </summary>
        /// <param name="store">线上仓名称。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        DataSet GetFactoryByStore(string store);
        /// <summary>
        /// 通过工单号获取工单下发的工厂名称。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        DataSet GetFactoryByOrderNumber(string orderNumber);

        /// <summary>
        /// 根据线上仓名称获取工序名称。
        /// </summary>
        /// <param name="lineStore">线上仓名称。</param>
        /// <returns>包含工序名称的数据集。[OPERATION_NAME,STORE_NAME]</returns>
        DataSet GetOperationByLineStore(string lineStore);
        /// <summary>
        /// 通过工序和车间主键获取线上仓。
        /// </summary>
        /// <param name="operation">工序名称,</param>
        /// <param name="roomKey">车间主键,</param>
        /// <param name="stores">拥有权限的线上仓名称，使用逗号分隔store1,store2...。</param>
        /// <returns>包含线上仓的数据集对象。</returns>
        DataSet GetStores(string operation, string roomKey, string stores);
        /// <summary>
        /// 通过工序获取线上仓。
        /// </summary>
        /// <param name="operation">工序名称,</param>
        /// <param name="stores">拥有权限的线上仓名称。使用逗号分隔,store1,store2...。</param>
        /// <returns>包含线上仓的数据集对象。</returns>
        DataSet GetStoreByOperation(string operation, string stores);
        /// <summary>
        /// 通过工单号和物料料号获取线上仓。
        /// </summary>
        /// <param name="workOrder">工单号,</param>
        /// <param name="materialCode">物料编码,</param>
        /// <param name="stores">拥有权限的线上仓名称,使用逗号分隔,store1,store2...。</param>
        ///  <returns>包含线上仓的数据集对象。</returns>
        DataSet GetStoreByMaterialCode(string workOrder, string materialCode, string stores);
        /// <summary>
        /// 获取硅片物料信息。
        /// </summary>
        /// <returns>
        /// 包含硅片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        DataSet GetMaterials();
        /// <summary>
        /// 获取硅片物料信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>
        /// 包含硅片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        DataSet GetMaterials(string orderNumber);
        /// <summary>
        /// 获取供应商信息。
        /// </summary>
        /// <returns>
        /// 包含供应商信息的数据集。
        /// 【CODE,NAME,NICKNAME】
        /// </returns>
        DataSet GetSuppliers();
        /// <summary>
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。
        /// [PRODUCT_CODE,PRODUCT_NAME]
        /// </returns>
        DataSet GetProdId();
        /// <summary>
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。
        /// [EFFICIENCY_NAME,LEFFICIENCY,UEFFICIENCY]
        /// </returns>
        DataSet GetEfficiency();
        /// <summary>
        /// 获取领料项目（批次）号对应的信息。
        /// </summary>
        /// <param name="val">领料项目（批次）号。</param>
        /// <returns>包含领料项目（批次）号信息的数据集对象。</returns>
        DataSet GetReceiveMaterialLotInfo(string val);
        /// <summary>
        /// 手工输入并保存接收的物料信息。
        /// </summary>
        /// <param name="htParams">包含输入数据的哈希表对象。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet ManualSaveReceiveMaterial(Hashtable htParams);
        /// <summary>
        /// 将选择的物料信息接收到线上仓。
        /// </summary>
        /// <param name="dtParams">包含物料信息的数据集对象。物料信息，员工号，时区</param>
        /// <returns>包含操作结果的数据集对象。</returns>
        DataSet ReceiveLineMaterial(DataTable dataTable);
        /// <summary>
        /// 获取物料领用记录。
        /// </summary>
        /// <param name="dtParams">
        /// 包含查询条件的数据表对象。
        /// --------------------------------------
        /// {DO}{STORE_NAME}{OPERATION_NAME}{OPERATOR}{CHARG}{LLIEF}{MATNR}{MBLNR}{RECEIVE_TIME_START}{RECEIVE_TIME_END}
        /// --------------------------------------
        /// DO=Query
        /// ---------------------------------------
        /// DO=其他, 线上仓名称使用逗号分隔,store1,store1...
        /// --------------------------------------
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含物料领用详细信息的数据集对象。</returns>
        DataSet GetReceiveMaterialHistory(DataTable dtParams, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 为工单BOM添加自备料。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="materialCode">物料编码。</param>
        /// <param name="materialDescription">物料描述。</param>
        /// <returns>包含操作结果的数据集对象。</returns>
        DataSet CreateWOBomOwnMaterial(string orderNumber, string materialCode, string materialDescription);

    }
}

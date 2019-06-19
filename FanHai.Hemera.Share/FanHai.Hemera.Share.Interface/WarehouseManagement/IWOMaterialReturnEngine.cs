using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.WarehouseManagement
{
    /// <summary>
    /// 工单退料数据的操作接口。
    /// </summary>
    public interface IWOMaterialReturnEngine
    {
        /// <summary>
        /// 获取当前时间的班别。
        /// </summary>
        /// <returns>包含当前时间班别数据的数据集对象。</returns>
        DataSet   GetCurrentShift();
        /// <summary>
        /// 根据物料批号获取物料信息。
        /// </summary>
        /// <param name="MatLot">物料批号。</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        DataSet GetMatLotInfo(string MatLot);
        /// <summary>
        /// 根据车间名称获取工厂名称。
        /// </summary>
        /// <param name="strFacRoom">车间名称。</param>
        /// <returns>包含工厂名称的数据集对象。</returns>
        DataSet GetFacRoomtoFac(string strFacRoom);
        /// <summary>
        /// 根据前置码获取退料单最大流水号。
        /// </summary>
        /// <param name="strPrex">退料单前置码。</param>
        /// <returns>包含最大流水号的数据集对象。</returns>
        DataSet GetRetMatNo(string strprex);
        /// <summary>
        /// 保存退料单数据。
        /// </summary>
        /// <param name="strRetMatNo">退料单号。</param>
        /// <param name="strRetMatDate">退料单日期。</param>
        /// <param name="strShift">班别名称。</param>
        /// <param name="strOperator">操作人。</param>
        /// <param name="strRetMatReason">退料原因。</param>
        /// <param name="dtMatLotList">包含退料物料数据的数据表对象。</param>
        /// <returns>true:保存成功。false：保存失败。</returns>
        bool Save(string strRetMatNo, string strRetMatDate, string strShift, string strOperator,
                  string strRetMatReason, DataTable dtMatLotList);
        /// <summary>
        /// 根据线上仓名称和工序名称获取所有退料信息。
        /// </summary>
        /// <param name="strStore">线上仓名称，使用逗号分隔:store1,store2....</param>
        /// <param name="strOperation">工序名称，使用逗号分隔:op1,op2.....</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        DataSet GetRetMatInfo(string strStore,string strOperation);
        /// <summary>
        /// 根据查询条件获取工单退料信息。
        /// </summary>
        /// <param name="strMatLot">物料批号。</param>
        /// <param name="strMatCode">物料料号。</param>
        /// <param name="strMatDes">物料描述。</param>
        /// <param name="strOperation">工序名称。</param>
        /// <param name="strStore">线上仓名称。</param>
        /// <param name="strFacRoom">车间名称。</param>
        /// <param name="strSupplier">供应商名称。</param>
        /// <param name="strShift">班别名。</param>
        /// <param name="strOperator">操作人。</param>
        /// <param name="strFromRetDate">退料日期（开始）</param>
        /// <param name="strToRetDate">退料日期（结束）</param>
        /// <param name="strRetMatNo">退料单号。</param>
        /// <param name="strOperationALL">工序名称，使用逗号分隔:op1,op2.....</param>
        /// <param name="strStoreALL">线上仓名称，使用逗号分隔:store1,store2....</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        DataSet GetWoRetMatInof(string strMatLot, string strMatCode, string strMatDes, string strOperation, 
            string strStore, string strFacRoom, string strSupplier, string strShift, 
            string strOperator, string strFromRetDate, string strToRetDate, string strRetMatNo, 
            string strOperationALL, string strStoreALL);
        /// <summary>
        /// 根据退料单得到退料信息。
        /// </summary>
        /// <param name="strRetMatList">退料单号。</param>
        /// <returns>包含退料信息的数据集对象。</returns>
        DataSet GetRetMatInfo1(string strRetMatList);
        /// <summary>
        /// 获取SAP中的退料单是否删除。
        /// </summary>
        /// <param name="dsIMPORT">包含退料单信息的数据集对象。</param>
        /// <returns>true:退料单在SAP中已删除。false:退料单未删除。</returns>
        bool DeleteMat(DataSet dsIMPORT);
        /// <summary>
        /// 删除退料单。
        /// </summary>
        /// <param name="strReturnNo">退料单号。</param>
        /// <returns>true：删除成功。false：删除失败。</returns>
        bool DeleteMat2(string strReturnNo);
    }
}

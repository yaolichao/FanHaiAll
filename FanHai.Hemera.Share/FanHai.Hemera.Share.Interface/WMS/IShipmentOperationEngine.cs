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
    /// 包含出货操作方法的接口。
    /// </summary>
    public interface IShipmentOperationEngine
    {
        /// <summary>
        /// 获取出货单中的指定柜号当前已入柜数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <param name="containerNo">货柜号。</param>
        /// <returns>指定柜号当前已入柜数量。</returns>
        double GetContainerQuantity(string shipmentNo, string containerNo);
        /// <summary>
        /// 获取出货单当前已出货数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <returns>已出货数量。</returns>
        double GetShipmentQuantity(string shipmentNo);
        /// <summary>
        /// 获取CI当前已出货数量。
        /// </summary>
        /// <param name="ciNumber">CI单号。</param>
        /// <returns>指定CI号当前已出货数量。</returns>
        double GetCIQuantity(string ciNumber);
        /// <summary>
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        DataSet GetPalletInfo(string palletNo);
        /// <summary>
        /// 托盘出货作业。
        /// </summary>
        /// <param name="dsParams">包含托盘操作信息的数据集对象。</param>
        /// <returns>包含托盘出货作业结果的数据集对象。</returns>
        DataSet PalletShipment(DataSet dsParams);
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        DataSet Query(DataSet dsParams, ref PagingQueryConfig config);
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        DataSet Query(DataSet dsParams);
        /// <summary>
        /// 从BCP数据库中根据指定托盘号导入托盘数据。
        /// </summary>
        /// <param name="palletNo">托盘号。可以使用逗号分隔的多个托盘号。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet ImportPalletDataFromBCP(string palletNo);

        DataSet UpdateConteinerNo(DataSet dssetin);
        /// <summary>
        /// 查询是否存在该出货单号的记录
        /// </summary>
        /// <param name="ShipmentNum">出货单号</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet SelectShipmentInf(string ShipmentNum);
        /// <summary>
        /// 根据出货单号查询出货信息
        /// </summary>
        /// <param name="sNum">出货单号</param>
        /// <returns></returns>
        DataSet GetShipmentInf(string sNum);
        /// <summary>
        /// 修改出货信息状态
        /// </summary>
        /// <param name="ShipmentNum">出货单号</param>
        /// <returns></returns>
        bool DeleteShipMentInf(string ShipmentNum,string name);
        /// <summary>
        /// 新增出货单数据
        /// </summary>
        /// <param name="dsParams">数据信息集</param>
        /// <returns></returns>
        DataSet PalletShipmentAdd(DataSet dsParams);
        /// <summary>
        /// 修改状态过账
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet PassShipMentInf(DataSet dsParams, string name);
        /// <summary>
        /// 修改出货单
        /// </summary>
        /// <param name="dsParams"></param>
        /// <returns></returns>
        DataSet PalletShipmentAUpdate(DataSet dsParams);
        /// <summary>
        /// 通过托盘号查询出货清单中是否存在可用记录
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>数据集</returns>
        DataSet GetShipMentNumByPallet(string palletNo);
        /// <summary>
        /// 根据出货单号进行冲销
        /// </summary>
        /// <param name="shipmentNo">出货单号</param>
        /// <returns></returns>
        DataSet SterilisationShipment(string shipmentNo);

        DataSet GetPalletShipInf(DataSet dsPalletShipInf);

        DataSet ShipmentQuery(DataSet dsParams, ref PagingQueryConfig config);

        //通过CI号获取出货地
        string GetShipMentBasicGOTO(string ciNumber);
    }
}

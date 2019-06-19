using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 耗用物料的数据操作接口。
    /// </summary>
    public interface IUseMaterialEngine
    {
        /// <summary>
        /// 根据线边仓名称获取车间名称和主键。
        /// </summary>
        /// <param name="STORES">用户拥有权限的线边仓名称,使用逗号分隔:store1,store2...</param>
        /// <returns>包含车间名称和车间主键的数据集对象。</returns>
        DataSet GetWorkShopInfo(string STORES);
        /// <summary>
        /// 根据工序名称和车间名称获取设备信息。
        /// </summary>
        /// <param name="operationname">工序名称。</param>
        /// <param name="cmbfactoryroom">车间名称。</param>
        /// <returns>包含设备信息的数据集对象。</returns>
        DataSet GetEquipmentInfo(string operationname, string cmbfactoryroom);
        /// <summary>
        /// 获取物料耗用信息。
        /// </summary>
        /// <param name="operations">工序名称,使用逗号分隔:op1,op2...</param>
        /// <param name="stores">线上仓名称，使用逗号分隔:store1,store2...</param>
        /// <returns>包含物料耗用信息的数据集对象。</returns>
        DataSet GetMaterialUsed(string operations,string stores);
        /// <summary>
        /// 获取材料耗用详细信息。
        /// </summary>
        /// <param name="_materialLot">物料批号。</param>
        /// <param name="_gongXuName">工序名称。</param>
        /// <param name="_wuLiaoNumber">物料料号。</param>
        /// <param name="_factoryRoomName">车间名称。</param>
        /// <param name="_wuLiaoMiaoShu">物料描述。</param>
        /// <param name="_equipmentName">设备名称。</param>
        /// <param name="_gongYingShang">供应商名称。</param>
        /// <param name="_banCi">班次。</param>
        /// <param name="_lineCang">线上仓名称。</param>
        /// <param name="_jobNumber">工号。</param>
        /// <param name="_startTime">耗用时间。</param>
        /// <param name="_endTime">耗用时间。</param>
        /// <param name="_stores">线上仓名称。使用逗号分隔：store1,store2...</param>
        /// <param name="_operations">工序名称。使用逗号分隔：op1,op2...</param>
        /// <returns></returns>
        DataSet GetStoreMaterialDetail(string _materialLot, string _gongXuName, string _wuLiaoNumber, string _factoryRoomName, string _wuLiaoMiaoShu, string _equipmentName, string _gongYingShang, string _banCi, string _lineCang, string _jobNumber, DateTime _startTime, DateTime _endTime,string _stores,string _operations);
        /// <summary>
        /// 通过工序车间和物料批号获取物料信息。
        /// </summary>
        /// <param name="materialLot">物料批号</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="cmbFactoryRoom">工厂车间</param>
        /// <returns>包含物料信息的数据集对象。</returns>
        DataSet GetMaterialByLotOpFa(string materialLot, string operationName, string cmbFactoryRoom);
        /// <summary>
        /// 根据耗用主键获取材料耗用信息
        /// </summary>
        /// <param name="materialUsedDetalKey">耗用主键</param>
        /// <returns>dataset</returns>
        DataSet GetMaterialDetailByKey(string materialUsedDetalKey);
        /// <summary>
        /// 通过批次物料批号和物料耗用主键获取未结账耗用明细。
        /// </summary>
        /// <param name="materialLot">物料批次号。</param>
        /// <param name="materitalUseKey">耗用主键。</param>
        /// <returns>包含物料耗用明细的数据集对象。</returns>
        DataSet GetMaterialDetailByMaterialLot(string materialLot, string materitalKey);
        /// <summary>
        /// 更新物料耗用及其明细记录。
        /// </summary>
        /// <param name="materialLot">物料批号。</param>
        /// <param name="materialUsedKey">物料耗用主键。</param>
        /// <param name="materialUsedDetailKey">物料耗用明细主键</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="equipmentKey">设备名称。</param>
        /// <param name="usedTime">耗用时间。2012-09-12 13:00:00</param>
        /// <param name="stirTime">搅拌时间。2012-09-12 13:00:00</param>
        /// <param name="printQty">印刷数量。</param>
        /// <param name="dtParams">
        /// 包含更新信息的数据表对象。
        /// --------------------------------------------
        /// {EDITOR}
        /// {EDIT_TIMEZONE}
        /// --------------------------------------------
        /// </param>
        /// <returns>true：更新成功。false：更新失败。</returns>
        bool UpDateMaterialUsedAndDetail(string materialLot, string materialUsedKey, string materialUsedKey1, string operationName, string equipmentKey,
            string usedTime, string stirTime, string printQty, DataTable hashTable);
        /// <summary>
        /// 删除物料耗用。
        /// </summary>
        /// <param name="materialLot">物料批号。</param>
        /// <param name="materialUsedKey">物料耗用主键。</param>
        /// <param name="materialUsedDetailKey">物料耗用明细主键。</param>
        /// <returns>true：成功。false：失败。</returns>
        bool DeleteMaterital(string materialLot, string materialUsedKey, string materialUsedKey1);
        /// <summary>
        /// 新增物料耗用。
        /// </summary>
        /// <param name="dsParams">包含物料耗用记录的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet InsertMaterial(DataSet dssetin);
        /// <summary>
        /// 获取物料耗用的所有信息。
        /// </summary>
        /// <returns>包含物料耗用信息的数据集对象。</returns>
        DataSet GetAllMaterialUsed();
    }
}

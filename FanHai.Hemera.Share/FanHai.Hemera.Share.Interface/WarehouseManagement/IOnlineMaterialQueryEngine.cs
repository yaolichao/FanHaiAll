using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface.WarehouseManagement
{
    /// <summary>
    /// 线上物料查询条件类。
    /// </summary>
    [Serializable]
    public class OnlineMaterialQueryModel
    {
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey { get; set; }
        /// <summary>
        /// 车间名称。
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 物料编码。
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 物料批号。
        /// </summary>
        public string MaterialLot { get; set; }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 供应商。
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 线上仓名称。
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OnlineMaterialQueryModel()
        {
            this.OperationName = string.Empty;
            this.RoomKey = string.Empty;
            this.RoomName = string.Empty;
            this.MaterialCode = string.Empty;
            this.MaterialLot = string.Empty;
            this.SupplierName = string.Empty;
            this.StoreName = string.Empty;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public OnlineMaterialQueryModel(OnlineMaterialQueryModel model)
        {
            this.OperationName = model.OperationName;
            this.RoomKey = model.RoomKey;
            this.RoomName = model.RoomName;
            this.MaterialCode = model.MaterialCode;
            this.MaterialLot = model.MaterialLot;
            this.SupplierName = model.SupplierName;
            this.StoreName = model.StoreName;
        }
    }

    /// <summary>
    /// 线上仓物料数据查询接口。
    /// </summary>
    public interface IOnlineMaterialQueryEngine
    {
        /// <summary>
        /// 查询在线物料信息。
        /// </summary>
        /// <param name="model">
        /// 包含查询条件对象。
        /// </param>
        /// <param name="pconfig">
        /// 分页查询的配置对象。
        /// </param>
        /// <returns>包含在线物料信息的数据集。</returns>
        DataSet Query(OnlineMaterialQueryModel model, ref PagingQueryConfig pconfig);
        /// <summary>
        /// 查询在线物料明细信息。
        /// </summary>
        /// <param name="model">
        /// 包含查询条件对象。
        /// </param>
        /// <param name="storeMaterialKey">
        /// 在线物料主键
        /// </param>
        /// <returns>包含在线物料信息的数据集。</returns>
        DataSet QueryDetail(OnlineMaterialQueryModel model, string storeMaterialKey);
    }
}

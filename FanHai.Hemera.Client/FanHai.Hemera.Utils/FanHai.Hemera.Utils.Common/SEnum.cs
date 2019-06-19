using System;
using System.Collections.Generic;
using System.Text;

namespace FanHai.Hemera.Utils.Common
{
    public enum EntityType 
    { 
        None = 0, 
        SaleOrder = 1, 
        SalesOrderItem = 2, 
        WorkOrder = 3, 
        Operator = 4, 
        Step = 5, 
        Product = 6, 
        Lot = 7, 
        Equipment = 8,
        LotTemplate = 9,
        Part = 10, 
        Computer=11,
        Line=12 
    }
    /// <summary>
    /// 控件状态枚举类型。
    /// </summary>
    public enum ControlState { 
        /// <summary>
        /// 空
        /// </summary>
        Empty = 0, 
        /// <summary>
        /// 只读
        /// </summary>
        ReadOnly = 1, 
        /// <summary>
        /// 读
        /// </summary>
        Read,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit,
        /// <summary>
        /// 新建
        /// </summary>
        New, 
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    }
    /// <summary>
    /// 批次状态标记
    /// </summary> 
    public enum LotStateFlag {
        /// <summary>
        /// 等待进站
        /// </summary>
        [System.ComponentModel.Description("等待进站")]
        WaitintForTrackIn = 0,
        /// <summary>
        /// 等待出站数据收集 4
        /// </summary>
        [System.ComponentModel.Description("等待出站数据收集")]
        WaitingForOutEDC = 4, 
        /// <summary>
        /// 出站数据收集 5
        /// </summary>
        [System.ComponentModel.Description("出站数据收集")]
        OutEDC = 5,
        /// <summary>
        /// 等待出站 9
        /// </summary>
        [System.ComponentModel.Description("等待出站")]
        WaitingForTrackout = 9, 
        /// <summary>
        /// 完成 10
        /// </summary>
        [System.ComponentModel.Description("已完成")]
        Finished = 10,
        /// <summary>
        /// 已入库 11
        /// </summary>
        [System.ComponentModel.Description("已入库")]
        ToStore = 11 
    }
    public enum EntityState 
    { 
        None = 0, 
        Added, 
        Modified, 
        Deleted 
    };
    //public enum CommonValue { False = 0, True = 1 };
    /// <summary>
    /// 端口类型。（打印机等）
    /// </summary>
    public enum PortType {
        /// <summary>
        /// 网络
        /// </summary>
        Network = 0, 
        /// <summary>
        /// 并口
        /// </summary>
        Parallel, 
        /// <summary>
        /// 串口
        /// </summary>
        Serial,
        /// <summary>
        /// 本地
        /// </summary>
        Local 
    };
    //public enum StoreType { Rework=0,BClass=1,CClass=2,withDraw=3,needCut=4,Fragment=5};
    /// <summary>
    /// 存放指定字段原始值和新值的数据项类。
    /// </summary>
    public class DirtyItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DirtyItem()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DirtyItem(string fieldName, string originalValue, string newValue)
        {
            _fieldName = fieldName;
            _fieldOrgValue = originalValue;
            _fieldNewValue = newValue;
        }

        private string _fieldName = "";             //字段名
        private string _fieldOrgValue = "";         //原始值
        private string _fieldNewValue = "";         //新值
        /// <summary>
        /// 字段名。
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
            }
        }
        /// <summary>
        /// 字段原始值。
        /// </summary>
        public string FieldOriginalValue
        {
            get { return _fieldOrgValue; }
            set
            {
                _fieldOrgValue = value;
            }
        }
        /// <summary>
        /// 字段新值。
        /// </summary>
        public string FieldNewValue
        {
            get { return _fieldNewValue; }
            set
            {
                _fieldNewValue = value;
            }
        }
    }
}

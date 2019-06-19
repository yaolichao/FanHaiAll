using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 操作功能枚举。
    /// </summary>
    public enum OperationAction { 
        /// <summary>
        /// 无
        /// </summary>
        None = 0, 
        /// <summary>
        /// 新增
        /// </summary>
        New, 
        /// <summary>
        /// 更新
        /// </summary>
        Update, 
        /// <summary>
        /// 修改
        /// </summary>
        Modified, 
        /// <summary>
        /// 删除
        /// </summary>
        Delete
    };
    /// <summary>
    /// 自定义数据类型。
    /// </summary>
    public enum AttributeDataType 
    {
        /// <summary>
        /// 整型。
        /// </summary>
        [DescriptionAttribute("整型")]
        INTEGER = 1,
        /// <summary>
        /// 日期。
        /// </summary>
        [DescriptionAttribute("日期")]
        DATE = 2, 
        /// <summary>
        /// 日期时间。
        /// </summary>
        [DescriptionAttribute("日期时间")]
        DATETIME = 3, 
        /// <summary>
        /// 布尔。
        /// </summary>
        [DescriptionAttribute("布尔")]
        BOOLEAN = 4, 
        /// <summary>
        /// 字符串。
        /// </summary>
        [DescriptionAttribute("字符串")]
        STRING = 5, 
        /// <summary>
        /// 浮点型。
        /// </summary>
        [DescriptionAttribute("浮点型")]
        FLOAT = 6, 
        /// <summary>
        /// 设置。
        /// </summary>
        [DescriptionAttribute("设置")]
        SETTING = 7, 
        /// <summary>
        /// 关联。
        /// </summary>
        [DescriptionAttribute("关联")]
        LINKED = 8 
    };
    /// <summary>
    /// 实体状态。
    /// </summary>
    public enum EntityStatus 
    { 
        /// <summary>
        /// 未激活。
        /// </summary>
        [DescriptionAttribute("未激活")]
        InActive = 0,
        /// <summary>
        /// 激活。
        /// </summary>
        [DescriptionAttribute("激活")]
        Active = 1, 
        /// <summary>
        /// 存档。
        /// </summary>
        [DescriptionAttribute("存档")]
        Archive = 2 
    };
    /// <summary>
    /// 逻辑操作符定义。
    /// </summary>
    public enum DatabaseLogicOperator 
    { 
        /// <summary>
        /// 非。
        /// </summary>
        Not = 0, 
        /// <summary>
        /// 和。
        /// </summary>
        And = 1,
        /// <summary>
        /// 或。
        /// </summary>
        Or = 2 
    };
    /// <summary>
    /// 逻辑比较操作符。
    /// </summary>
    public enum DatabaseCompareOperator
    {
        Equal = 0, 
        NotEqual, 
        LessThan, 
        GreaterThan, 
        LessThanEqual,
        GreaterThanEqual,
        In, 
        NotIn,
        Between, 
        Like, 
        Null, 
        NotNull
    }
    /// <summary>
    /// 托盘状态。
    /// </summary>
    public enum PalletState
    {
        /// <summary>
        /// 包装中。
        /// </summary>
        [DescriptionAttribute("包装中")]
        Packaging = 0,
        /// <summary>
        /// 待入库检。
        /// </summary>
        [DescriptionAttribute("待入库检")]
        Packaged=1,
        /// <summary>
        /// 待入库。
        /// </summary>
        [DescriptionAttribute("待入库")]
        Checked=2,
        /// <summary>
        /// 已入库
        /// </summary>
        [DescriptionAttribute("已入库")]
        Warehouse=3,
        /// <summary>
        /// 已出货
        /// </summary>
        [DescriptionAttribute("已出货")]
        Shipment = 4
    }
    /// <summary>
    /// 工序参数的数据来源
    /// </summary>
    public enum OperationParamDataFrom
    {
        /// <summary>
        /// 手工输入
        /// </summary>
        [DescriptionAttribute("手工输入")]
        ManualInput=0,
        /// <summary>
        /// 设备接口。
        /// </summary>
        [DescriptionAttribute("设备接口")]
        EquipmentInterface,
        /// <summary>
        /// 上料功能
        /// </summary>
        [DescriptionAttribute("上料功能")]
        UseMaterial
    }
    /// <summary>
    /// 工序参数的采集时刻。
    /// </summary>
    public enum OperationParamDCType
    {
        /// <summary>
        /// 进站时采集
        /// </summary>
        [DescriptionAttribute("进站时采集")]
        TrackIn = 0,
        /// <summary>
        /// 出站时采集。
        /// </summary>
        [DescriptionAttribute("出站时采集")]
        TrackOut
    }
    /// <summary>
    /// 工序参数的验证规则。
    /// </summary>
    public enum OperationParamValidateRule
    {
        /// <summary>
        /// 不验证
        /// </summary>
        [DescriptionAttribute("不验证")]
        None = 0,
        /// <summary>
        /// 前缀匹配。
        /// </summary>
        [DescriptionAttribute("前缀匹配")]
        MatchPrefix,
        /// <summary>
        /// 后缀匹配。
        /// </summary>
        [DescriptionAttribute("后缀匹配")]
        MatchPostfix,
        /// <summary>
        /// 内容包含。
        /// </summary>
        [DescriptionAttribute("内容包含")]
        InContent,
        /// <summary>
        /// 完全匹配。
        /// </summary>
        [DescriptionAttribute("完全匹配")]
        MatchContent,
        /// <summary>
        /// 工单BOM物料完全匹配。
        /// </summary>
        [DescriptionAttribute("工单BOM物料完全匹配")]
        MatchWorkOrderMat,
        /// <summary>
        /// 工单BOM物料前缀匹配。
        /// </summary>
        [DescriptionAttribute("工单BOM物料前缀匹配")]
        MatchWorkOrdeMatPrefix,
        /// <summary>
        /// 工单BOM物料后缀匹配。
        /// </summary>
        [DescriptionAttribute("工单BOM物料后缀匹配")]
        MatchWorkOrdeMatPostfix,
        /// <summary>
        /// 工单BOM物料包含匹配。
        /// </summary>
        [DescriptionAttribute("工单BOM物料包含匹配")]
        MatchWorkOrdeMatInContent
    }
    /// <summary>
    /// 工序参数的验证失败规则。
    /// </summary>
    public enum OperationParamValidateFailedRule
    {
        /// <summary>
        /// 提示验证失败
        /// </summary>
        [DescriptionAttribute("提示验证失败")]
        PromptAndNoContinue = 0
    }
    /// <summary>
    /// 工序参数的排列顺序。
    /// </summary>
    public enum OperationParamOrderType
    {
        /// <summary>
        /// 先行后列。
        /// </summary>
        FirstRow=0,
        /// <summary>
        /// 先列后行。
        /// </summary>
        FirstColumn
    }
    /// <summary>
    /// 计算规则。
    /// </summary>
    public enum CalculateRule
    {
        /// <summary>
        /// 无计算规则
        /// </summary>
        [DescriptionAttribute("None")]
        None = 0,
        /// <summary>
        /// 大于
        /// </summary>
        [DescriptionAttribute(">")]
        GreaterThan,
        /// <summary>
        /// 等于
        /// </summary>
        [DescriptionAttribute("=")]
        Equal,
        /// <summary>
        /// 小于
        /// </summary>
        [DescriptionAttribute("<")]
        LessThan,
        /// <summary>
        /// 大于等于
        /// </summary>
        [DescriptionAttribute(">=")]
        GreaterThanOrEqual,
        /// <summary>
        /// 小于等于
        /// </summary>
        [DescriptionAttribute("<=")]
        LessThanOrEqual
    }
}

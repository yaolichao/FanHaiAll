using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class TRANS_TABLES
    {
        public const string TABLE_PARAM = "PARAM";
        public const string TABLE_RETURN_PARAM = "PARAM_TABLE";
        public const string TABLE_MAIN_DATA = "MAIN_DATA";
        public const string TABLE_SUB_ITEMS = "SUB_ITEMS";
        public const string TABLE_UDAS = "UDAS";
    }
    /// <summary>
    /// 表示属性名的实体类。
    /// </summary>
    public class PROPERTY_FIELDS
    {
        public const string PRIVILEGE = "PRIVILEGE";
        public const string LINES = "LINES";
        /// <summary>
        /// 工序名。
        /// </summary>
        public const string OPERATIONS = "OPERATIONS";
        public const string STORES = "STORES";
        public const string uiLanguageProperty = "CoreProperties.UILanguage";
        public const string USER_KEY = "USER_KEY";
        public const string USER_NAME = "USER_NAME";
        public const string USER_NAME_MZ = "USER_NAME_MZ"; //add by qym 20120530
        /// <summary>
        /// 计算机名。
        /// </summary>
        public const string COMPUTER_NAME = "COMPUTER_NAME";
        public const string COMPUTER_MAC = "COMPUTER_MAC";
        public const string TIMEZONE = "TIMEZONE";
        /// <summary>
        /// 站点
        /// </summary>
        public const string SITE = "SITE";
        public const string LOGIN_LOG_KEY = "LOGIN_LOG_KEY";
        public const string CHARG = "CHARG";
        public const string LGORT = "LGORT";
        /// <summary>
        /// 是否进行时间控制。
        /// </summary>
        public const string TimeControl = "IsTimeControl";
        public const string FACTORY_CODE = "FACTORY_CODE";
        public const string HISTORYINFO_CLEAR = "HISTORYINFO_CLEAR";
        /// <summary>
        /// 上次刷入组件的工单号。
        /// </summary>
        public const string PRE_WORK_ORDER_NO = "PRE_WORK_ORDER_NO";
        /// <summary>
        /// IV测试数据的路径。
        /// </summary>
        public const string IVTEST_DATA_PATH = "IVTEST_DATA_PATH";
        /// <summary>
        /// IV测试数据的类型。
        /// </summary>
        public const string IVTEST_DATA_TYPE = "IVTEST_DATA_TYPE";
        /// <summary>
        /// IV测试数据的设备号。
        /// </summary>
        public const string IVTEST_DATA_DEVICE = "IVTEST_DATA_DEVICE";
        /// <summary>
        /// IV测试设备所在车间主键。
        /// </summary>
        public const string IVTEST_DATA_ROOM_KEY = "IVTEST_DATA_ROOM_KEY";
        /// <summary>
        /// IV测试设备所在工序名称。
        /// </summary>
        public const string IVTEST_DATA_OPERATION_NAME = "IVTEST_DATA_OPERATION_NAME";

        /// <summary>
        /// 自动铭牌打印机名称
        /// </summary>
        public const string NAMEPLATE_PRINTER = "NAMEPLATE_PRINTER";
        /// <summary>
        /// 铭牌打印机X轴
        /// </summary>
        public const string NAMEPLATE_PRINTER_X = "NAMEPLATE_PRINTER_X";
        /// <summary>
        /// 铭牌打印机Y轴
        /// </summary>
        public const string NAMEPLATE_PRINTER_Y = "NAMEPLATE_PRINTER_Y";
        /// <summary>
        /// 自动标签打印机名称
        /// </summary>
        public const string LABEL_PRINTER = "LABEL_PRINTER";
        /// <summary>
        /// IV测试数据库信息变量定义
        /// </summary>
        public const string IVTEST_DATA_DB_ADDRESS = "IVTEST_DATA_DB_ADDRESS";
        public const string IVTEST_DATA_DB_NAME = "IVTEST_DATA_DB_NAME";
        public const string IVTEST_DATA_DB_PORT = "IVTEST_DATA_DB_PORT";
        public const string IVTEST_DATA_DB_LGNAME = "IVTEST_DATA_DB_LGNAME";
        public const string IVTEST_DATA_DB_LGPW = "IVTEST_DATA_DB_LGPW";
        /// <summary>
        /// EL图片上传配置数据。 add by yongbing.yang 2016年1月27日 10:44:21
        /// </summary>
        public const string EL_PICTURE_SOURCE_ROOT_PATH = "EL_PICTURE_SOURCE_ROOT_PATH";
        public const string EL_PICTURE_SOURCE_PATH_FORMAT = "EL_PICTURE_SOURCE_PATH_FORMAT";
        public const string EL_PICTURE_DEST_ROOT_PATH = "EL_PICTURE_DEST_ROOT_PATH";
        public const string EL_PICTURE_DEST_PATH_FORMAT = "EL_PICTURE_DEST_PATH_FORMAT";
        public const string EL_PICTURE_EXISTENSION = "EL_PICTURE_EXISTENSION";
        public const string EL_PICTURE_USER_NAME = "EL_PICTURE_USER_NAME";
        public const string EL_PICTURE_USER_PASSWORD = "EL_PICTURE_USER_PASSWORD";
        public const string EL_PICTURE_IS_DELETE_LOCAL_FILE = "EL_PICTURE_IS_DELETE_LOCAL_FILE";
        //add new const for product type modify by yongbing.yang 2015年5月27日
        public const string EL_PICTURE_IS_SEMIMANUFACTURES = "EL_PICTURE_IS_SEMIMANUFACTURES";
        /// <summary>
        /// IV图片上传配置数据。
        /// </summary>
        public const string IV_PICTURE_SOURCE_ROOT_PATH = "IV_PICTURE_SOURCE_ROOT_PATH";
        public const string IV_PICTURE_SOURCE_PATH_FORMAT = "IV_PICTURE_SOURCE_PATH_FORMAT";
        public const string IV_PICTURE_DEST_ROOT_PATH = "IV_PICTURE_DEST_ROOT_PATH";
        public const string IV_PICTURE_DEST_PATH_FORMAT = "IV_PICTURE_DEST_PATH_FORMAT";
        public const string IV_PICTURE_EXISTENSION = "IV_PICTURE_EXISTENSION";
        public const string IV_PICTURE_USER_NAME = "IV_PICTURE_USER_NAME";
        public const string IV_PICTURE_USER_PASSWORD = "IV_PICTURE_USER_PASSWORD";
        public const string IV_PICTURE_IS_DELETE_LOCAL_FILE = "IV_PICTURE_IS_DELETE_LOCAL_FILE";
        /// <summary>
        /// ELNG图片上传配置数据。
        /// </summary>
        public const string ELNG_PICTURE_SOURCE_ROOT_PATH = "ELNG_PICTURE_SOURCE_ROOT_PATH";
        public const string ELNG_PICTURE_SOURCE_PATH_FORMAT = "ELNG_PICTURE_SOURCE_PATH_FORMAT";
        public const string ELNG_PICTURE_DEST_ROOT_PATH = "ELNG_PICTURE_DEST_ROOT_PATH";
        public const string ELNG_PICTURE_DEST_PATH_FORMAT = "ELNG_PICTURE_DEST_PATH_FORMAT";
        public const string ELNG_PICTURE_EXISTENSION = "ELNG_PICTURE_EXISTENSION";
        public const string ELNG_PICTURE_USER_NAME = "ELNG_PICTURE_USER_NAME";
        public const string ELNG_PICTURE_USER_PASSWORD = "ELNG_PICTURE_USER_PASSWORD";
        public const string ELNG_PICTURE_IS_DELETE_LOCAL_FILE = "ELNG_PICTURE_IS_DELETE_LOCAL_FILE";
    }
    /// <summary>
    /// 表示基础数据分类的名称。
    /// </summary>
    public class BASEDATA_CATEGORY_NAME
    {
        /// <summary>
        /// 批次类型。
        /// </summary>
        public const string Lot_Type = "Lot_Type";
        /// <summary>
        /// 批次创建类型。
        /// </summary>
        public const string Lot_CreateType = "Lot_CreateType";
        /// <summary>
        /// 批次优先级。
        /// </summary>
        public const string Lot_Priority = "Lot_Priority";
        /// <summary>
        /// 成品类型。
        /// </summary>
        public const string PART_TYPE = "PART_TYPE";
        /// <summary>
        /// 原因代码类型。
        /// </summary>
        public const string Basic_TypeOfRCode = "Basic_TypeOfRCode";
        /// <summary>
        /// 原因代码分类。
        /// </summary>
        public const string Basic_ClassOfRCode = "Basic_ClassOfRCode";
        /// <summary>
        /// 原因代码分类值-电池片类型的分类 格式如下："'17','19','21'"。
        /// </summary>
        public const string Basic_ClassOfRCodeValue_Cell = "'17','19','21'";
        /// <summary>
        /// MES和ERP数据的对应关系。
        /// </summary>
        public const string MEScontrastERP = "MEScontrastERP";
        /// <summary>
        /// 出货类型。
        /// </summary>
        public const string Basic_ShipmentType = "Basic_ShipmentType";
        /// <summary>
        /// 存放图片地址的基础数据。
        /// </summary>
        public const string Uda_Pic_Address = "Uda_Pic_Address";
        /// <summary>
        /// 存放测试规则相关的数据。
        /// </summary>
        public const string Basic_TestRule_PowerSet = "Basic_TestRule_PowerSet";
        /// <summary>
        /// 工单类型（P:量产工单，R:返工工单）
        /// </summary>
        public const string Uda_work_type = "Uda_work_type";
        /// <summary>
        /// 根据产品ID混包
        /// </summary>
        public const string Basic_MixPackage_ByFactory = "Basic_MixPackage_ByFactory";
        /// <summary>
        /// 入库检工序检验流程卡图片
        /// </summary>
        public const string Basic_CheckPic_ByBeforeWareHouse = "Basic_CheckPic_ByBeforeWareHouse";
        /// <summary>
        /// 班别代码。
        /// </summary>
        public const string Basic_Shift = "Basic_Shift";
        /// <summary>
        /// E工单判定条件设置
        /// </summary>
        public const string Uda_Setting_ExperimentWo = "Uda_Setting_ExperimentWo";
        /// <summary>
        /// 返工单\转工单配置。
        /// </summary>
        public const string ExchangeWoConfig = "ExchangeWoConfig";
        /// <summary>
        /// 标签打印配置。
        /// </summary>
        public const string PrintLabelConfig = "PrintLabelConfig";

        /// <summary>
        /// 工单读取规则
        /// </summary>
        public const string WorkOrderRuleConfig = "WorkOrderRuleConfig";

        /// <summary>
        /// 工厂
        /// </summary>
        public const string Basic_Werks = "Basic_Werks";

        public const string Basic_CheckIscImp = "Basic_CheckIscImp";
    }
    /// <summary>
    /// 用于过站时间控制的字段名称。
    /// </summary>
    public class TIME_CONTROL_FILEDS
    {
        /// <summary>
        /// 数据表名称。
        /// </summary>
        public const string DATATABLE_NAME = "TIME_CONTROL";
        /// <summary>
        /// 加工的基本时间。
        /// </summary>
        public const string TIMECONTROLBASE = "TIMECONTROLBASE";
        /// <summary>
        /// 加工的最小时间。
        /// </summary>
        public const string TIMECONTROLMIN = "TIMECONTROLMIN";
        /// <summary>
        /// 加工的警告时间。
        /// </summary>
        public const string TIMECONTROLWARNING = "TIMECONTROLWARNING";
        /// <summary>
        /// 加工的最大时间。
        /// </summary>
        public const string TIMECONTROLMAX = "TIMECONTROLMAX";
        /// <summary>
        /// 最大等待进站时间。
        /// </summary>
        public const string MAXTRACKINTIME = "MAXTRACKINTIME";
        /// <summary>
        /// 时间状态：
        /// 0：加工时间没有满足最小加工时间。
        /// 1:加工时间满足最小时间，没有超过报警时间。
        /// 2:加工时间超过报警时间，没有超过最大加工时间。
        /// 3:加工时间超过最大加工时间。
        /// </summary>
        public const string TIMESTATUSFLAG = "TIMESTATUSFLAG";
        /// <summary>
        /// 基本加工时间和最小加工时间之间的差。
        /// </summary>
        public const string TIMECONTROLBASESUBMIN = "TIMECONTROLBASESUBMIN";
    }
    /// <summary>
    /// 表示操作动作名称的字段值。
    /// </summary>
    public class ACTIVITY_FIELD_VALUES
    {
        /// <summary>
        /// 创建批次。
        /// </summary>
        public const string FIELD_ACTIVITY_CREATELOT = "CREATELOT";
        /// <summary>
        /// 进站数据采集。
        /// </summary>
        public const string FIELD_ACTIVITY_INEDC = "INEDC";
        /// <summary>
        /// 出站数据采集。
        /// </summary>
        public const string FIELD_ACTIVITY_OUTEDC = "OUTEDC";
        /// <summary>
        /// 进站操作
        /// </summary>
        public const string FIELD_ACTIVITY_TRACKIN = "TRACKIN";
        /// <summary>
        /// 出站操作
        /// </summary>
        public const string FIELD_ACTIVITY_TRACKOUT = "TRACKOUT";
        /// <summary>
        /// 出托操作
        /// </summary>
        public const string FIELD_ACTIVITY_PALLETOUT = "PALLETOUT";
        /// <summary>
        /// 无任何操作
        /// </summary>
        public const string FIELD_ACTIVITY_NONE = "NONE";
        /// <summary>
        /// 组件报废操作
        /// </summary>
        public const string FIELD_ACTIVITY_SCRAP = "SCRAP";
        /// <summary>
        /// 电池片报废操作
        /// </summary>
        public const string FIELD_ACTIVITY_CELLSCRAP = "CELLSCRAP";
        /// <summary>
        /// 组件不良操作
        /// </summary>
        public const string FIELD_ACTIVITY_DEFECT = "DEFECT";
        /// <summary>
        /// 电池片不良操作
        /// </summary>
        public const string FIELD_ACTIVITY_CELLDEFECT = "CELLDEFECT";
        /// <summary>
        /// 电池片补片操作。
        /// </summary>
        public const string FIELD_ACTIVITY_PATCH = "PATCH";
        /// <summary>
        /// 电池片被补片操作。
        /// </summary>
        public const string FIELD_ACTIVITY_PATCHED = "PATCHED";
        /// <summary>
        /// 电池片回收(撤销报废/撤销补片)操作。
        /// </summary>
        public const string FIELD_ACTIVITY_RECOVERED = "RECOVERED";
        /// <summary>
        /// 电池片被回收(被撤销补片)操作。
        /// </summary>
        public const string FIELD_ACTIVITY_BE_RECOVERED = "BE_RECOVERED";
        /// <summary>
        /// 释放批次
        /// </summary>
        public const string FIELD_ACTIVITY_RELEASE = "RELEASE";
        /// <summary>
        /// 暂停批次
        /// </summary>
        public const string FIELD_ACTIVITY_HOLD = "HOLD";
        /// <summary>
        /// 终止批次
        /// </summary>
        public const string FIELD_ACTIVITY_TERMINALLOT = "TERMINALLOT";
        /// <summary>
        /// 批次退料。
        /// </summary>
        public const string FIELD_ACTIVITY_RETURN_MATERIAL = "RETURN_MATERIAL";
        /// <summary>
        /// 批次调整。
        /// </summary>
        public const string FIELD_ACTIVITY_ADJUST = "ADJUST";
        /// <summary>
        /// 重工操作。
        /// </summary>
        public const string FIELD_ACTIVITY_REWORK = "REWORK";
        /// <summary>
        /// 转工单操作。
        /// </summary>
        public const string FIELD_ACTIVITY_CHANGE_WO = "CHANGE_WO";
        /// <summary>
        /// 转产品ID号操作（返工单）。
        /// </summary>
        public const string FIELD_ACTIVITY_CHANGE_PROID = "CHANGE_PROID";
        /// <summary>
        /// 包装。
        /// </summary>
        public const string FIELD_ACTIVITY_PACKAGE = "PACKAGE";
        /// <summary>
        /// 拆包。
        /// </summary>
        public const string FIELD_ACTIVITY_UNPACK = "UNPACK";
        /// <summary>
        /// 入库。
        /// </summary>
        public const string FIELD_ACTIVITY_TO_WAREHOUSE = "TO_WAREHOUSE";
        /// <summary>
        /// 撤销操作。
        /// </summary>
        public const string FIELD_ACTIVITY_UNDO = "UNDO";
        /// <summary>
        /// 线别调整。
        /// </summary>
        public const string FIELD_ACTIVITY_CHANGE_LINE = "CHANGE_LINE";
    }
    /// <summary>
    /// 工步自定义属性名称。
    /// </summary>
    public class ROUTE_STEP_ATTRIBUTE
    {
        /// <summary>
        /// 预设暂停，用于设置预设暂停的工序名称。当检验超出规格线时，在指定工序进站时暂停。
        /// </summary>
        public const string FutureHold = "FutureHold";
        /// <summary>
        /// 暂停默认密码。用于当检验超出规格线时等导致系统自动暂停批次时使用。
        /// </summary>
        public const string HoldPassword = "HoldPassword";
    }
    /// <summary>
    /// 工序自定义属性名称。
    /// </summary>
    public class ROUTE_OPERATION_ATTRIBUTE
    {
        /// <summary>
        /// 是否显示设备选择对话框。
        /// </summary>
        public const string IsShowPickEquipmentDialog = "IsShowPickEquipmentDialog";
        /// <summary>
        /// 是否检查校准板时间周期。
        /// </summary>
        public const string IsCheckCalibrationCycle = "IsCheckCalibrationCycle";
        /// <summary>
        /// 是否检查固化时间周期。
        /// </summary>
        public const string IsCheckFixCycle = "IsCheckFixCycle";
        /// <summary>
        /// 是否检查恒温时间周期。
        /// </summary>
        public const string IsCheckConstantTemperatureCycle = "IsCheckConstantTemperatureCycle";
        /// <summary>
        /// 进站时检查数量是否和创批时数量一样，不满足条件不允许进站。
        /// </summary>
        public const string IsCheckQuantityAtTrackIn = "IsCheckQuantityAtTrackIn";
        /// <summary>
        /// 出站时检查数量是否和创批时数量一样，不满足条件不允许出站。
        /// </summary>
        public const string IsCheckQuantityAtTrackOut = "IsCheckQuantityAtTrackOut";
        /// <summary>
        /// 是否是入库工序。
        /// </summary>
        public const string IsToWarehouseOperation = "IsToWarehouseOperation";
        /// <summary>
        /// 是否是入库检验工序
        /// </summary>
        public const string IsToWarehouseCheckOperation = "IsToWarehouseCheckOperation";
        /// <summary>
        /// 是否是包装工序
        /// </summary>
        public const string IsToPackageOperation = "IsToPackageOperation";
        /// <summary>
        /// 是否检查电池片信息。如果电池片信息没有输入，则在出站时必须输入电池片信息。
        /// </summary>
        public const string IsCheckSILot = "IsCheckSILot";
        /// <summary>
        /// 是否检查电池片转换效率是否和领料转换效率一致。
        /// </summary>
        public const string IsCheckEfficiency = "IsCheckEfficiency";
        /// <summary>
        /// 是否根据工单检查颜色。如果颜色信息没有输入且工单设置了必须输入颜色，则颜色必须输入。
        /// </summary>
        public const string IsCheckColorByWorkOrder = "IsCheckColorByWorkOrder";
        /// <summary>
        /// 出站时是否检查IV测试数据，如果不满足不允许出站。
        /// </summary>
        public const string IsCheckIVTestData = "IsCheckIVTestData";
        /// <summary>
        /// 出站时是否检查EL图片，如果没有EL图片不允许出站。
        /// </summary>
        public const string IsCheckELPicture = "IsCheckELPicture";
        /// <summary>
        /// 出站时是否可以设置新的工艺流程。
        /// </summary>
        public const string IsShowSetNewRoute = "IsShowSetNewRoute";
        /// <summary>
        /// 出站时是否显示打印标签的对话框。
        /// </summary>
        public const string IsShowPrintLabelDialog = "IsShowPrintLabelDialog";
        /// <summary>
        /// 批量过站数量。
        /// </summary>
        public const string BatchTrackCount = "BatchTrackCount";
        /// <summary>
        /// 创建批次时间允许的最大时间范围，如果不设置表示不进行检查。
        /// </summary>
        public const string CreateLotMaxCycle = "CreateLotMaxCycle";
        /// <summary>
        /// 是否检查组件工单和前一次刷入工单的异同，如果不相同给出提示，如果相同不做任何提示。
        /// </summary>
        public const string IsCheckWorkOrderNoDifferent = "IsCheckWorkOrderNoDifferent";
        /// <summary>
        /// 过站作业时是否显示设备选择下拉框。
        /// </summary>
        public const string IsShowEquipmentDropdownList = "IsShowEquipmentDropdownList";
        
    }
    /// <summary>
    /// 生产线自定义属性名称。
    /// </summary>
    public class LINE_ATTRIBUTE
    {
        /// <summary>
        /// 是否是代工线。
        /// </summary>
        public const string IsOemLine = "IsOemLine";
        /// <summary>
        /// 固化时间周期。
        /// </summary>
        public const string FixCycle = "FixCycle";
    }
    /// <summary>
    /// 批次线别卡控。
    /// </summary>
    public class LOT_LINE
    {
        /// <summary>
        /// 是否进行批次线别卡控。
        /// </summary>
        public const string IsCheckLotLine = "IsCheckLotLine";
    }

    /// <summary>
    /// 表示通用字段名称的实体类。
    /// </summary>
    public class COMMON_FIELDS
    {
        public const string FIELD_COMMON_LINKED_TABLE_NAME = "LINKED_TABLE_NAME";
        public const string FIELD_COMMON_LINKED_ITEM_KEY = "LINKED_TO_ITEM_KEY";
        public const string FIELD_COMMON_LINKED_ITEM_SEQ = "LINKED_ITEM_SEQ";

        public const string FIELD_COMMON_OPERATION_ACTION = "OPERATION_ACTION";
        public const string FIELD_ATTRIBUTE_NAME = "ATTRIBUTE_NAME";
        public const string FIELD_COMMON_ATTRIBUTE_KEY = "ATTRIBUTE_KEY";
        public const string FIELD_COMMON_ATTRIBUTE_VALUE = "ATTRIBUTE_VALUE";
        /// <summary>
        /// 更新主键。
        /// </summary>
        public const string FIELD_COMMON_UPDATE_KEY = "UPDATE_KEY";
        /// <summary>
        /// 更新名称。
        /// </summary>
        public const string FIELD_COMMON_UPDATE_NAME = "UPDATE_NAME";
        public const string FIELD_COMMON_UPDATE_VALUE = "UPDATE_VALUE";
        /// <summary>
        /// 更新原始值。
        /// </summary>
        public const string FIELD_COMMON_UPDATE_OLD_VALUE = "UPDATE_OLD_VALUE";
        /// <summary>
        /// 更新用新值。
        /// </summary>
        public const string FIELD_COMMON_UPDATE_NEW_VALUE = "UPDATE_NEW_VALUE";
        public const string FIELD_COMMON_EDITOR = "EDITOR";
        public const string FIELD_COMMON_EDIT_TIME = "EDIT_TIME";
        public const string FIELD_COMMON_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string FIELD_COMMON_CREATOR = "CREATOR";
        public const string FIELD_COMMON_CREATE_TIME = "CREATE_TIME";
        public const string FIELD_COMMON_CREATE_TIMEZONE = "CREATE_TIMEZONE";
        public const string FIELD_COMMON_CREATE_TIMEZONE_KEY = "CREATE_TIMEZONE_KEY";

        public const string FIELD_COMMON_MESSAGE_CODE = "MESSAGE_CODE";
        public const string FIELD_COMMON_MESSAGE_CONTENT = "MESSAGE_CONTENT";
        public const string FIELD_COMMON_EDITTIME_EXP = "EDITTIMEEXP";

        public const string FIELD_COMMON_ROWNUM = "RN";
        public const string FIELD_COMMON_CHECKED = "CHECKED";

        
        public const string FIELD_WORK_ORDER_STEP = "WORK_ORDER_STEP";
        /// <summary>
        /// 行号。
        /// </summary>
        public const string FIELD_ROW_COUNT = "ROW_COUNT";
        /// <summary>
        /// 主键。
        /// </summary>
        public const string FIELD_COMMON_KEY = "KEY";
        /// <summary>
        /// 值。
        /// </summary>
        public const string FIELD_COMMON_VALUE = "VALUE";
        /// <summary>
        /// 名称。
        /// </summary>
        public const string FIELD_COMMON_NAME = "NAME";
        /// <summary>
        /// 描述。
        /// </summary>
        public const string FIELD_COMMON_DESCRIPTION = "DESCRIPTION";
    }

    public class COMMON_NAMES
    {
        public const string LINKED_ITEM_EDC = "EDC";
    }

    public class COMMON_VALUES
    {
        public const string VALUE_COMMON_ZERO = "0";
        public const string VALUE_COMMON_ONE = "1";
        public const string VALUE_COMMON_YES = "Yes";
        public const string VALUE_COMMON_NO = "No";
        public const string VALUE_COMMON_TRUE = "True";
        public const string VALUE_COMMON_FALSE = "False";
    }

    public class COMMON_SYMBOL
    {
        public const string CALC_PLUS = "+";
        public const string CALC_MINUS = "-";
        public const string CALC_TIMES = "*";
        public const string CALC_DIVIDE = "/";

    }

    public class BASIC_CONST
    {
        public const string PARAM_TABLENAME_COLUMNS = "COLUMNS";
        public const string PARAM_TABLENAME_CATEGORY = "CATEGORY";
        public const string PARAM_TABLENAME_CONDITIONS = "CONDITIONS";

        public const string PARAM_COL_COLUMN_NAME = "COLUMN_NAME";
        public const string PARAM_COL_COLUMN_VALUE = "COLUMN_VALUE";
    }

    public class MEI_CONST
    {
        public const string TABLENAME_DEVICE_CONFIG = "DEVICE_CONFIG";
        public const string COL_COMPUTER_NAME = "COMPUTER_NAME";
        public const string COL_DEVICE_TYPE = "DEVICE_TYPE";

        public const string PARAM_TABLENAME_COLUMNS = "COLUMNS";
        public const string PARAM_TABLENAME_CATEGORY = "CATEGORY";
        public const string PARAM_TABLENAME_CONDITIONS = "CONDITIONS";

        public const string PARAM_COL_COLUMN_NAME = "COLUMN_NAME";
        public const string PARAM_COL_COLUMN_VALUE = "COLUMN_VALUE";
    }

    public class FACTORY_CONFIG
    {
        public const string FACTORY_TABLE_NAME = "Factory_Config";
        public const string FACTORY_NAME = "FACTORY_NAME";
        public const string WORK_ORDER = "WORK_ORDER";
        public const string LINE = "LINE";
        public const string BATTERY_TYPE = "BATTERY_TYPE";
        public const string SERIAL_NUMBER = "SERIAL_NUMBER";
    }

    public class COMMON_FORMAT
    {
        public const string YEAR_FORMAT = "yyyy";
        public const string MONTH_FORMAT = "MM";
        public const string DAY_FORMAT = "dd";
        public const string YEAR_MONTH_FORMAT = "yyyy-MM";
        public const string MONTH_DAY_FORMAT = "MM-dd";
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const string SHORT_DATE_FORMAT = "yyMMdd";
        public const string FAST_DATE_FORMAT = "yyyyMMdd";
        public const string TIME_FORMAT = "HH:mm:ss";
        public const string FAST_TIME_FORMAT = "HHmmss";
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string FAST_DATETIME_FORMAT = "yyyyMMddHHmmss";
    }

    public class PARAMETERS
    {
        public const string INPUT_KEY = "KEY";
        public const string INPUT_PAGENO = "PAGENO";
        public const string INPUT_PAGESIZE = "PAGESIZE";
        public const string INPUT_EDITOR = "EDITOR";
        public const string INPUT_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string INPUT_EDIT_TIME = "EDIT_TIME";

        public const string OUTPUT_MESSAGE = "MESSAGE";
        public const string OUTPUT_PAGES = "PAGES";
        public const string OUTPUT_RECORDS = "RECORDS";
        public const string OUTPUT_EDITOR = "EDITOR";
        public const string OUTPUT_EDIT_TIMEZONE = "EDIT_TIMEZONE";
        public const string OUTPUT_EDIT_TIME = "EDIT_TIME";
    }

    public class CHECKTYPE
    {
        /// <summary>
        /// 终检
        /// </summary>
        public const string DATA_GROUP_ENDCHECK = "ENDCHECK";
        /// <summary>
        /// 客检
        /// </summary>
        public const string DATA_GROUP_CUSTOMERCHECK = "CUSTOMERCHECK";
        /// <summary>
        /// 包装
        /// </summary>
        public const string DATA_GROUP_CONSIGNMENT = "1";
        /// <summary>
        /// 入库检
        /// </summary>
        public const string DATA_GROUP_TOSTORE = "2";
        /// <summary>
        /// 检验类别
        /// </summary>
        public const string DATA_TYPE = "DATA_TYPE";
    }

    public class DATETIME_CLASS
    {
        public const string DATETIME_YEAR = "年";
        public const string DATETIME_MONTH = "月";
        public const string DATETIME_HOUR = "时";

    }
    /// <summary>
    /// 工单属性设置(根据工单配置)
    /// </summary>
    public class WORKORDER_SETTING_ATTRIBUTE
    {
        /// <summary>
        /// 清洁工序是否必须输入花色
        /// </summary>
        public const string IsMustInputModuleColorByCleanOpt = "isMustInputModuleColorByCleanOpt";
        /// <summary>
        /// 是否可以按工单混托
        /// </summary>
        public const string IsReceiveMixWosByPackage = "isReceiveMixWosByPackage";
        /// <summary>
        /// 是否为实验工单（true:是；false:否）
        /// </summary>
        public const string IsExperimentWo = "IsExperimentWo";
        /// <summary>
        /// 工单客户
        /// </summary>
        public const string Customer = "Customer";
        /// <summary>
        /// Conergy类别
        /// </summary>
        public const string Conergy = "Conergy";
        /// <summary>
        /// Schueco类别
        /// </summary>
        public const string Schueco = "Schueco";
        /// <summary>
        /// 常规-客户类别
        /// </summary>
        public const string CommondCustomerType = "常规";
        /// <summary>
        /// 客级
        /// </summary>
        public const string Grade_KJ = "Grade_KJ";
        /// <summary>
        /// A级
        /// </summary>
        public const string Grade_AJ = "Grade_AJ";
    }
}

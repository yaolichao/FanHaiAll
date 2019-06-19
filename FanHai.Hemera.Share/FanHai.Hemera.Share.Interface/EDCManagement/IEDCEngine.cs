using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据采集操作接口。
    /// </summary>
    /// <remarks>
    /// 参数分组数据管理，抽检点数据管理，保存采集数据，获取采集相关数据等。
    /// </remarks>
    public interface IEDCEngine
    {
        /// <summary>
        /// 根据抽样规则主键获取抽样规则信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含抽样规则主键的数据集。
        /// -------------------------
        /// {SP_KEY}
        /// -------------------------
        /// </param>
        /// <returns>包含抽样规则信息的数据集。 </returns>
        DataSet GetSamplingInfo(DataSet dataset);
        /// <summary>
        /// 根据管控数据组主键获取需要管控项的信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含管控数据组主键的数据集。
        /// ----------------------------
        /// {EDC_KEY}
        /// ----------------------------
        /// </param>
        /// <returns>
        /// 包含管控项数据的数据集。
        /// 【A.PARAM_KEY,A.PARAM_NAME,A.MANDATORY,A.ISDERIVED,A.DATA_TYPE,A.DEVICE_TYPE】
        /// </returns>
        DataSet GetParams(DataSet dataset);
        /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <param name="dsParams">
        /// 包含数据采集实例数据的数据集。
        /// </param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet SaveEDCMainIN(DataSet dataset);
        /// <summary>
        /// 删除参数分组及其参数数据。
        /// </summary>
        /// <param name="paramKey">参数分组主键。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteEdcAndParam(string edcKey);
        /// <summary>
        /// 新增参数分组及其参数数据。
        /// </summary>
        /// <param name="dsParams">包含参数分组及其参数数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet AddEdcAndParam(DataSet dataset);
        /// <summary>
        /// 查询参数分组。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// -----------------------------------------
        /// {EDC_NAME}
        /// -----------------------------------------
        /// </param>
        /// <returns>包含参数分组的数据集对象。</returns>
        DataSet SearchEdcMain(DataSet dataset);
        /// <summary>
        /// 获取不重复的参数分组名称。
        /// </summary>
        /// <returns>包含参数分组名称的数据集对象。</returns>
        DataSet GetDistinctEdcName();
        /// <summary>
        /// 根据参数分组主键获取参数分组信息。
        /// </summary>
        /// <param name="edcKey">参数分组主键。</param>
        /// <returns>包含参数分组信息的数据集对象。</returns>
        DataSet GetEdcByKey(string edcKey);
        /// <summary>
        /// 参数分组名称和参数分组主键相互转换。
        /// </summary>
        /// <param name="inputParam">查询条件（EDC_NAME或EDC_KEY）。</param>
        /// <param name="symBol">I：查询条件为EDC_NAME。 O(字母)：查询条件为EDC_KEY。</param>
        /// <returns>
        /// I：返回EDC_KEY。 O(字母)：返回EDC_NAME。
        /// </returns>
        string ConvertEdcKeyOrName(string inputParam, string symBol);
        /// <summary>
        /// 更新参数分组及其对应的参数数据。
        /// </summary>
        /// <param name="dsParams">包含参数分组及其对应的参数的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet UpdateEdcAndParam(DataSet dataset);
        /// <summary>
        /// 保存采集到的明细数据。
        /// </summary>
        /// <param name="dsParams">包含到采集到的明细数据的数据集。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet SaveEDCCollectionData(DataSet dataset);
        /// <summary>
        /// 根据批次主键和批次数据采集主键获取批次已采集的数据集合。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">批次数据采集主键。</param>
        /// <returns>
        /// 包含批次已采集的数据集合的数据集对象。
        /// [SP_UNIT_SEQ,PARAM_KEY,EDC_INS_KEY,PARAM_VALUE,COL_KEY,VALID_FLAG,
        /// FAILED_RULES,SP_SAMP_SEQ,EDITOR,EDIT_TIME,EDIT_TIMEZONE,DELETED_FLAG]
        /// </returns>
        DataSet GetEDCCollectionData(string lotKey,string edcInsKey);
        /// <summary>
        /// 根据批次主键获取批次采集的数据。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <returns>
        /// 包含批次采集数据的数据集。
        /// 【EDC_KEY,SP_COUNT,SP_UNITS,EDC_INS_KEY】
        /// </returns>
        DataSet GetEDCMainIN(string lotKey);
        /// <summary>
        /// 保存在线采集的管控项数据值。
        /// </summary>
        /// <param name="dsParams">包含管控项数据值的数据集。</param>
        /// <param name="paramCount">参数采集数的个数。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcInsKey">数据采集实例主键。</param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet SaveOnlineEDCData(DataSet dataset, int paramCount,string lotKey,string edcInsKey);
        /// <summary>
        /// 保存离线采集到的管控项数据值。
        /// </summary>
        /// <param name="dataset">
        /// 包含采集到的管控项数据值的数据集。
        /// <see cref="EDC_MAIN_INS_FIELDS.DATABASE_TABLE_NAME"/>、 <see cref="EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME"/>
        /// </param>
        /// <returns>包含执行结果的数据集。</returns>
        DataSet SaveOfflineEDCData(DataSet dataset);
        /// <summary>
        /// 获取最大版本号的参数分组信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ----------------------------------
        /// {EDC_NAME}
        /// ----------------------------------
        /// </param>
        /// <returns>
        /// 包含参数分组信息的数据集对象。
        /// </returns>
        DataSet GetEDCScrapCategory(DataSet dataset);
        /// <summary>
        /// 获取版本号最大的抽检规则信息。
        /// </summary>
        /// <param name="dsParams">
        /// 包含查询条件的数据集对象。
        /// ------------------------------------
        /// {SP_NAME}
        /// ------------------------------------
        /// </param>
        /// <returns>
        /// 包含抽检规则信息的数据集对象。
        /// </returns>
        DataSet GetSPScrapCategory(DataSet dataset);
        /// <summary>
        /// 检查抽检规则的设置从而判断是否需要进行抽检。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <param name="sampKey">抽样规则主键。</param>
        /// <param name="type">数据采集类型。
        /// <see cref="COMMON_FIELDS.FIELD_ACTIVITY_INEDC"/>
        /// 或者<see cref="COMMON_FIELDS.FIELD_ACTIVITY_OUTEDC"/>
        /// </param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>true:需要抽检，false:不需要抽检。</returns>
        bool CheckSampling(string lotKey, string stepKey, string lineKey, string sampKey, string type,string equipmentKey);
        /// <summary>
        /// 新增数据采集实例及其采集的明细数据。
        /// </summary>
        /// <param name="dsParams">包含数据采集实例及其采集的明细数据的数据集对象。</param>
        /// <returns>true：新增成功，false:新增失败。</returns>
        bool InsertEdcMainAndDetails(DataSet dataSet);
        /// <summary>
        /// 根据批次号获取批次采集的数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>包含批次采集数据的数据集对象。</returns>
        DataSet GetEdcExistData(string lotNumber);
        /// <summary>
        /// 获取已激活的成品数据。
        /// </summary>
        /// <returns>
        /// 包含成品数据的数据集。
        /// 【PART_KEY,PART_NAME,PART_TYPE】
        /// </returns>
        DataSet GetPOR_PART();
        /// <summary>
        /// 获得已激活的工序数据。
        /// </summary>
        /// <returns>
        /// 包含工序数据的数据集。
        /// 【ROUTE_OPERATION_VER_KEY,ROUTE_OPERATION_NAME】
        /// </returns>
        DataSet GetPOR_ROUTE_OPERATION_VER();
        /// <summary>
        /// 获取已激活的抽样规则数据。
        /// </summary>
        /// <returns>
        /// 包含抽样规则的数据集。
        /// [SP_KEY,SP_NAME,SAMPLING_MODE,STRATEGY_SIZE,DESCRIPTIONS]
        /// </returns>
        DataSet GetEDC_SP();
        /// <summary>
        /// 获取已激活的管控项目组数据。
        /// </summary>
        /// <returns>
        /// 包含管控项目组数据的数据集。
        /// </returns>
        DataSet GetEDC_MAIN();
        /// <summary>
        /// 根据参数分组主键获取参数数据。
        /// </summary>
        /// <param name="key">参数分组主键。</param>
        /// <returns>包含参数数据的数据集对象。</returns>
        DataSet GetBASE_PARAMETERByEDC_KEY(string key);
        /// <summary>
        /// 根据抽检点主键获取抽检点对应的参数数据。
        /// </summary>
        /// <param name="ds">包含查询条件的数据集对象。
        /// --------------------------------------
        /// {FIELD_TOPRODUCT}
        /// {FIELD_EDC_KEY}
        /// {FIELD_SP_KEY}
        /// {FIELD_OPERATION_NAME}
        /// --------------------------------------
        /// </param>
        /// <param name="ifHas">输出参数，抽检点是否存在对应的参数数据。</param>
        /// <param name="EdcPointKey">输出参数，抽检点主键。</param>
        /// <returns>包含抽检点对应的参数数据的数据集对象。</returns>
        DataSet GetPARAMETERByEDCPOINT(DataSet ds, out bool ifHas, out string EdcPointKey);
        /// <summary>
        /// 检查指定EDCPoint的是否有满足条件的抽检参数，从而判断是否需要进行抽检。
        /// （1）基础参数处于激活状态。
        /// （2）抽检点设置的需要抽检数量>0
        /// </summary>
        /// <returns>true:需要抽检，false:不需要抽检。</returns>
        DataSet CheckEdc(DataTable dataTable);
        /// <summary>
        /// 新增抽检点数据及其对应的参数数据。
        /// </summary>
        /// <param name="EdcPoint">包含抽检点数据的数据集对象。</param>
        /// <param name="EdcPointP">包含抽检点对应参数数据的数据集对象。</param>
        /// <param name="ishasCreate">
        /// true:之前已创建抽检点数据，需要删除抽检点对应参数数据。
        /// false:之前未创建抽检点数据。
        /// </param>
        /// <returns>true：新增成功。false：新增失败。</returns>
        bool InsertEDCPointAll(DataSet EdcPoint, DataSet EdcPointP, bool ishasCreate);
        /// <summary>
        /// 根据批次数据采集主键和数据采集参数类型（R:根据设备类型正排序，否则根据设备类型倒排序。）获取采样参数点集合。
        /// </summary>
        /// <param name="edcInsKey">批次数据采集主键。</param>
        /// <param name="paramType">参数类型（R:根据设备类型正排序，否则根据设备类型倒排序。）</param>
        /// <returns>
        /// 包含批次采样参数点集合的数据集对象。
        /// [ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,
        /// UPPER_SPEC,UPPER_CONTROL,TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,
        /// PARAM_COUNT,PARAM_INDEX,PARAM_KEY,PARAM_FORMULA,PARAM_TYPE,Device_Type,DATA_TYPE,ISDERIVED,CALCULATE_TYPE]
        /// </returns>
        DataSet GetPointParamsByEDCInsKey(string edcInsKey,string paramType);
        /// <summary>
        /// 根据抽检点设置主键获取采样参数点集合。
        /// </summary>
        /// <param name="pointKey">抽检点设置主键。</param>
        /// <returns>
        /// 包含批次采样参数点集合的数据集对象。
        /// [ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,
        /// UPPER_SPEC,UPPER_CONTROL,TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,
        /// PARAM_COUNT,PARAM_INDEX,PARAM_KEY,PARAM_FORMULA,Device_Type,DATA_TYPE,ISDERIVED,CALCULATE_TYPE]
        /// </returns>
        DataSet GetPointParamsByPointKey(string pointKey);
        /// <summary>
        /// 根据批次号查询批次采集的数据。
        /// </summary>
        /// <param name="lotNumber">生产批次号。</param>
        /// <returns>
        /// 包含批次数据采集信息的数据集对象。
        /// </returns>
        DataSet GetLotParamsCollection(string lotNumber);
        /// <summary>
        /// 根据方块电阻设备的数据采集主键删除采集的方块电阻数据。
        /// </summary>
        /// <param name="edcInskey">方块电阻设备的数据采集主键</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet DeleteRData(string edcInskey);
         /// <summary>
        /// 查询离线数据采集数据。
        /// </summary>
        /// <param name="dtParams">数据采集实例。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含数据采集数据的数据集对象。</returns>
        DataSet QueryEDCData(DataTable dtParams, ref PagingQueryConfig config);
        /// <summary>
        /// 检查指定EDCPoint的是否有满足条件的抽检参数，从而判断是否需要进行抽检。
        /// （1）基础参数处于激活状态。
        /// （2）抽检点设置的需要抽检数量>0
        /// </summary>
        /// <returns>true:需要抽检，false:不需要抽检。</returns>
        bool CheckEDCPointParams(string edcPointKey);
        /// <summary>
        /// 指定批次在指定工序上是否进行了抽检。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <returns>true：进行抽检了。false：没有进行抽检。</returns>
        bool CheckDependSampStep(string lotKey, string operationName);
        /// <summary>
        /// 保存数据采集实例数据。
        /// </summary>
        /// <param name="dbTrans">事务操作对象。</param>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="edcPointKey">抽检点设置主键。</param>
        /// <param name="editor">编辑人。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="oprLine">操作线别名称。</param>
        /// <param name="shiftKey">班别主键。</param>
        void SaveEdcMainInfo(DbTransaction dbTrans,string lotKey, string edcPointKey,string editor, 
                             string equipmentKey,string oprLine, string shiftKey);
    }
}

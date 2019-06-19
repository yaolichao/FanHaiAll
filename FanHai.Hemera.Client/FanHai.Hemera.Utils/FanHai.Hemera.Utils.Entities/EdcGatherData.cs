
#region using
using System;
using System.Text;
using System.Data;
using System.Linq;

using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using System.Collections;
#endregion

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 表示设备数据采集的数据实体类。
    /// </summary>
    public class EdcGatherData
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EdcGatherData(){
            this.EDCMainInsKey = string.Empty;
        }
       
        /// <summary>
        /// 根据批次数据采集主键获取采样参数点集合。
        /// </summary>
        /// <param name="edcInsKey">批次数据采集主键。</param>
        /// <returns>
        /// 包含批次采样参数点集合的数据集对象。
        /// [ROW_KEY,EDC_POINT_ROWKEY,EDC_NAME,EDC_VERSION,PARAM_NAME,UPPER_BOUNDARY,
        /// UPPER_SPEC,UPPER_CONTROL,TARGET,LOWER_CONTROL,LOWER_SPEC,LOWER_BOUNDARY,
        /// PARAM_COUNT,PARAM_INDEX,PARAM_KEY,PARAM_FORMULA,PARAM_TYPE,Device_Type,DATA_TYPE,ISDERIVED,CALCULATE_TYPE]
        /// </returns>
        public DataSet GetPointParamsByEDCInsKey(string edcInsKey)
        {
            DataSet dsParams = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsParams = factor.CreateIEDCEngine().GetPointParamsByEDCInsKey(edcInsKey,string.Empty);
                ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsParams);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsParams;
        }
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
        /// comment by peter 2012-2-28
        public DataSet GetPointParamsByPointKey(string pointKey)
        {
            DataSet dsParams = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsParams = factor.CreateIEDCEngine().GetPointParamsByPointKey(pointKey);
                ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsParams);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsParams;
        }

        /// <summary>
        /// 根据参数主键获取用于计算该参数值的参数。
        /// </summary>
        /// <param name="paramKey">参数主键。</param>
        /// <returns>包含计算指定参数主键参数值的参数。
        /// 【ROW_KEY,DERIVATION_KEY,PARAM_KEY,PARAM_NAME】
        /// </returns>
        public DataSet GetParamDerivationByKey(string paramKey)
        {
            DataSet dsParams = new DataSet();
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsParams = factor.CreateIParamEngine().GetParamDerivationByKey(paramKey);
                ErrorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsParams);
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsParams;
        }
        /// <summary>
        /// 创建一个用来保存数据的数据表。
        /// </summary>
        /// <param name="tableName">数据表名称。</param>
        /// <returns>用来保存数据的<see cref="DataTable"/>对象。</returns>
        public DataTable BuildTable(string tableName)
        {
            DataTable edcTable = new DataTable();
            edcTable.TableName = tableName;
            DataColumn dc1 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ);
            DataColumn dc2 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ);
            DataColumn dc3 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY);
            DataColumn dc4 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE);
            DataColumn dc5 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_EDC_INS_KEY);
            DataColumn dc6 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_COL_KEY);
            DataColumn dc7 = new DataColumn(COMMON_FIELDS.FIELD_COMMON_OPERATION_ACTION);
            DataColumn dc8 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG);
            DataColumn dc9 = new DataColumn(EDC_COLLECTION_DATA_FIELDS.FIELD_EDITOR);
            edcTable.Columns.Add(dc1);
            edcTable.Columns.Add(dc2);
            edcTable.Columns.Add(dc3);
            edcTable.Columns.Add(dc4);
            edcTable.Columns.Add(dc5);
            edcTable.Columns.Add(dc6);
            edcTable.Columns.Add(dc7);
            edcTable.Columns.Add(dc8);
            edcTable.Columns.Add(dc9);
            return edcTable;
        }
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get;
            private set;
        }
        /// <summary>
        /// 工单号码。
        /// </summary>
        public string OrderNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 线别名称。
        /// </summary>
        public string LineName
        {
            get;
            set;
        }
        /// <summary>
        /// 批次号。
        /// </summary>
        public string LotNumber
        {
            get;
            set;
        }
        
        /// <summary>
        /// 班别名称。
        /// </summary>
        public string ShiftName
        {
            get;
            set;
        }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName
        {
            get;
            set;
        }
        /// <summary>
        /// 设备主键。
        /// </summary>
        public string EquipmentKey
        {
            get;
            set;
        }
        /// <summary>
        /// 设备名称。
        /// </summary>
        public string EquipmentName
        {
            get;
            set;
        }
        /// <summary>
        /// 物料批号。
        /// </summary>
        public string MaterialLot
        {
            get;
            set;
        }
        /// <summary>
        /// 成品料号。
        /// </summary>
        public string PartNumber
        {
            get;
            set;
        }
         /// <summary>
        /// 成品料号。
        /// </summary>
        public string PartKey
        {
            get;
            set;
        }
        /// <summary>
        /// 成品类型。
        /// </summary>
        public string PartType
        {
            get;
            set;
        }
        /// <summary>
        /// 用户工号。
        /// </summary>
        public string Operator
        {
            get;
            set;
        }
        /// <summary>
        /// EDC组主键。
        /// </summary>
        public string EDCKey
        {
            get;
            set;
        }
        /// <summary>
        /// EDC组名称。
        /// </summary>
        public string EDCName
        {
            get;
            set;
        }
        /// <summary>
        /// 抽检点设置主键。
        /// </summary>
        public string EDCPointKey
        {
            get;
            set;
        }
        /// <summary>
        /// EDC抽检主键。
        /// </summary>
        public string EDCSPKey
        {
            get;
            set;
        }
        /// <summary>
        /// EDC抽检名称。
        /// </summary>
        public string EDCSPName
        {
            get;
            set;
        }
        /// <summary>
        /// 工厂车间名称。
        /// </summary>
        public string FactoryRoomName
        {
            get;
            set;
        }
        /// <summary>
        /// 工厂车间主键。
        /// </summary>
        public string FactoryRoomKey
        {
            get;
            set;
        }
        /// <summary>
        /// EDC动作名称。NONE or TRACKOUT
        /// </summary>
        public string EDCActionName
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商名称。
        /// </summary>
        public string SupplierName
        {
            get;
            set;
        }
        /// <summary>
        /// 当前数据采集实例主键。
        /// </summary>
        public string EDCMainInsKey
        {
            get;
            set;
        }
    }
}

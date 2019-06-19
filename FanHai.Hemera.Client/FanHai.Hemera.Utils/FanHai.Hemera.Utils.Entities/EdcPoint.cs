/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
 * 20120709    YONGMING.QIAO     modifgy     增加属性EDIT_DESC                    Q.001
 * 20120709    YONGMING.QIAO     modifgy     增加查询修改历史的方法               Q.002
 * 20120716    YONGMING.QIAO     modifgy     分页显示查询结果                     Q.003
***************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;


namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 抽检点设置数据的实体类。
    /// </summary>
    public class EdcPoint : EntityObject
    {
        private string _partName = string.Empty;
        private string _operationName = string.Empty;
        private string _errorMsg = string.Empty;
        private string _pointRowKey = string.Empty;
        private string _equipmentName = string.Empty;
        private string _equipmentKey = string.Empty;
        private string _edcName = string.Empty;       
        private string _edcKey = string.Empty;       
        private string _spName = string.Empty;       
        private string _spKey = string.Empty;        
        private string _actionName = string.Empty;
        private string _pointState = string.Empty;  //add by zxa 20110822
        private string _edit_desc = string.Empty; //Q.001
        /// <summary>
        /// 抽检点修改新增原因
        /// </summary>
        ///Q.001
        public string EDIT_DESC
        {
            get { return _edit_desc; }
            set { _edit_desc = value; }
        }

        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        /// <summary>
        /// 抽检点设置记录的状态。
        /// </summary>
        public string PointState
        {
            get { return _pointState; }
            set { _pointState = value;}
        }
        /// <summary>
        /// 成品编码。
        /// </summary>
        public string PartName
        {
            get { return _partName; }
            set { _partName = value; }
        }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
            set { _operationName = value; }
        }
        /// <summary>
        /// 工序主键。
        /// </summary>
        public string OperationKey
        {
            get;
            set;
        }
        /// <summary>
        /// 抽检点设置主键。
        /// </summary>
        public string PointRowKey
        {
            get { return _pointRowKey; }
            set { _pointRowKey = value; }
        }
        /// <summary>
        /// 设备名称。
        /// </summary>
        public string EquipmentName
        {
            get { return _equipmentName; }
            set { _equipmentName = value; }
        }
        /// <summary>
        /// 设备主键。
        /// </summary>
        public string EquipmentKey
        {
            get { return _equipmentKey; }
            set { _equipmentKey = value; }
        }
        /// <summary>
        /// EDC名称。
        /// </summary>
        public string EdcName
        {
            get { return _edcName; }
            set { _edcName = value; }
        }
        /// <summary>
        /// EDC主键。
        /// </summary>
        public string EdcKey
        {
            get { return _edcKey; }
            set { _edcKey = value; }
        }
        /// <summary>
        /// 抽样规则名称。
        /// </summary>
        public string SpName
        {
            get { return _spName; }
            set { _spName = value; }
        }
        /// <summary>
        /// 抽样规则主键。
        /// </summary>
        public string SpKey
        {
            get { return _spKey; }
            set { _spKey = value; }
        }
        /// <summary>
        /// 动作名称 TRACKOUT,NONE
        /// </summary>
        public string ActionName
        {
            get { return _actionName; }
            set { _actionName = value; }
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
        /// 表示抽检点设置分组的键。
        /// </summary>
        public string GroupKey
        {
            get;
            set;
        }
        /// <summary>
        /// 表示抽检点设置分组名称。
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }
        /// <summary>
        /// 表示工艺流程主键。
        /// </summary>
        public string RouteKey
        {
            get;
            set;
        }
        /// <summary>
        /// 表示工艺流程名称。
        /// </summary>
        public string RouteName
        {
            get;
            set;
        }
        /// <summary>
        /// 表示工艺流程中的工步主键。
        /// </summary>
        public string StepKey
        {
            get;
            set;
        }
        /// <summary>
        /// 表示抽检点设置汇总必须输入的栏位值。
        /// </summary>
        public EDCPointMustInputField MustInputField
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EdcPoint()
        {
            this.MustInputField = EDCPointMustInputField.None;
        }

        /// <summary>
        /// 查询抽检点设置数据。
        /// 查询条件：
        /// 产品料号<see cref="PartName"/>（可选，如果为空，不作为查询条件）；
        /// 工序名称<see cref="OperationName"/>（可选，如果为空，不作为查询条件）。
        /// </summary>
        /// <returns>
        /// 包含抽检点设置数据的数据集。
        /// [ROW_KEY（分组中最小的ROW_KEY），TOPRODUCT,PART_TYPE,OPERATION_NAME,POINT_STATUS,
        /// POINT_STATE_DESCRIPTION,EQUIPMENT_NAME(用逗号分隔开),EQUIPMENT_KEY(用逗号分隔开),
        /// ACTION_NAME,EDC_NAME,SP_NAME,GROUP_KEY(标识分组的键)]
        /// </returns>
        public DataSet SearchEdcPoint()
        {
            DataSet dsReturn = new DataSet();
            DataTable dataTable =null;
            Hashtable hashTable = new Hashtable();
            if (_partName != string.Empty)
            {
                hashTable.Add(EDC_POINT_FIELDS.FIELD_TOPRODUCT, _partName);
            }
            if (_operationName != string.Empty)
            {
                hashTable.Add(EDC_POINT_FIELDS.FIELD_OPERATION_NAME, _operationName);
            }
            if (hashTable.Count > 0)
            {
                dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
            }
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn=iServerObjFactory.CreateIEDCPiont().SearchEdcPoint(dataTable);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据抽检点设置的主键获取抽检点参数数据集合。调用该方法前需要先设置<see cref="PointRowKey"/>的属性值。
        /// </summary>
        /// <returns>包含抽检点参数数据的数据集。</returns>
        public DataSet GetEdcPointParams()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn= obj.CreateIEDCPiont().GetEdcPiontParams(_pointRowKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.GetRemoteObject();
            }
            return dsReturn;
        }
        /// <summary>
        /// 捞取修改抽检点的his
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        /// Q.003
        public DataSet GetEdcPointParamsTrans(ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCPiont().GetEdcPiontParamsTrans(_pointRowKey,ref config);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.GetRemoteObject();
            }
            return dsReturn;
        }
        /// <summary>
        /// 创建抽检点设置数据。
        /// </summary>
        /// <param name="dataSet">
        /// 包含抽检点设置数据的数据集对象。包含一个键值对的数据表。
        /// </param>
        /// <returns>-1 失败。非-1值成功。</returns>
        public int CreateEdcPoint(DataSet dataSet)
        {
            int code = -1;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCPiont().CreateEdcPoint(dataSet);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return code;
        }
        /// <summary>
        /// 更新抽检点参数数据。
        /// </summary>
        /// <param name="dsEdcPointParam">包含抽检点参数数据的数据集。</param>
        /// <returns>true：插入成功。false：插入失败。</returns>
        public bool UpdateEDCPointParams(DataSet dataSet)
        {
            bool bReturn = false;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = iServerObjFactory.CreateIEDCPiont().UpdateEDCPointParams(dataSet);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg == string.Empty)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }
        /// <summary>
        /// 删除抽检点数据。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <returns>true：删除成功。false：删除失败。</returns>
        public bool DeleteEdcPoint(string groupKey)
        {
            bool bReturn = false;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = iServerObjFactory.CreateIEDCPiont().DeleteEDCPoint(groupKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg == string.Empty)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }
        /// <summary>
        /// 更新抽检点数据的状态。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <param name="pointStatus">新的抽检点设置状态。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public bool UpdateEDCPointStatus(string groupKey, string pointStatus)
        {
            bool bReturn = false;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = iServerObjFactory.CreateIEDCPiont().UpdateEDCPointStatus(groupKey, pointStatus);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg == string.Empty)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }
        /// <summary>
        /// 查找正在使用的相同类型的抽检点数据是否存在。
        /// </summary>
        /// <param name="groupKey">表示抽检点设置分组的键。</param>
        /// <returns>true：存在。false：不存在。</returns>
        public bool FindExistUsedEDCPoint(string pointRowKey)
        {
            bool bReturn = false;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                bReturn = iServerObjFactory.CreateIEDCPiont().FindExistUsedEDCPoint(pointRowKey);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bReturn;
        }
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="productNo">产品号。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="equipmentKey">设备主键。如果为空，则查询设备主键为NULL的数据。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        public DataSet GetEDCPoint(string productNo,string operationName,string equipmentKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.CreateIEDCPiont().GetEDCPoint(productNo, operationName, equipmentKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME】
        /// </returns>
        public DataSet GetEDCPoint(string factoryRoomKey, string operationName)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.CreateIEDCPiont().GetEDCPoint(factoryRoomKey, operationName);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取包含抽检点设置数据的集合。
        /// </summary>
        /// <param name="factoryRoomKey">车间主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="partType">成品类型。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <returns>
        /// 包含抽检点设置数据的集合。
        /// 【ROW_KEY,TOPRODUCT,OPERATION_NAME,EQUIPMENT_KEY,ACTION_NAME,SP_KEY,EDC_KEY,EDC_NAME,STATUS,PART_TYPE,EQUIPMENT_NAME,SP_NAME,SP_DESCRIPTIONS】
        /// </returns>
        public DataSet GetEDCPoint(string factoryRoomKey, string operationName,string partType,string equipmentKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                dsReturn = iServerObjFactory.CreateIEDCPiont().GetEDCPoint(factoryRoomKey, operationName, partType, equipmentKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
    }
}

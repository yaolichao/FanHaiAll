// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-03-29             新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 包含设备相关操作的实体类。
    /// </summary>
    public class EquipmentEntity:EntityObject
    {
        private string _errorMsg=string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get{ return  _errorMsg; }
        }
        /// <summary>
        /// 根据工厂车间、工序名称和线别主键查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineNames">包含线别名称的数组。</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// [EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE，LINE_NAME，LINE_KEY,EQUIPMENT_STATE_NAME]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName, string[] lineNames)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments(factoryRoomKey, operationName, lineNames);
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
        /// 根据工厂车间、工序名称和线别主键查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <param name="lineKey">线别主键。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName,string lineKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments(factoryRoomKey, operationName, lineKey);
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
        /// 根据工厂车间和工序名称查询设备信息。
        /// </summary>
        /// <param name="factoryRoomKey">工厂车间的主键。</param>
        /// <param name="operationName">工序名称。</param>
        /// <returns>
        /// 包含设备信息的数据集。[EQUIPMENT_KEY,EQUIPMENT_NAME，EQUIPMENT_CODE]
        /// </returns>
        public DataSet GetEquipments(string factoryRoomKey, string operationName)
        {
            DataSet dsReturn=null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments(factoryRoomKey, operationName);
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
        /// 根据设备编码查询设备信息。
        /// </summary>
        /// <param name="equipmentCode">设备编码。</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// </returns>
        public DataSet GetEquipments(string equipmentCode)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments(equipmentCode);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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

        public DataSet GetEquipments()
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipments();
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 根据设备主键获取设备的状态信息。
        /// </summary>
        /// <param name="equipmentKey">设备主键</param>
        /// <returns>
        /// 包含设备信息的数据集。
        /// [EQUIPMENT_STATE_KEY,EQUIPMENT_STATE_NAME,EQUIPMENT_STATE_TYPE,Equipment_STATE_CATEGORY,DESCRIPTION]
        /// </returns>
        public DataSet GetEquipmentState(string equipmentKey)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIEquipments().GetEquipmentState(equipmentKey);
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

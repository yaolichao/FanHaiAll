using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.StaticFuncs;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    /// <summary>
    /// Equipment State Entity Object
    /// </summary>
    /// Owner:Genchille.yang 2012-03-28 12:55:45
    public class EquipmentStateEventsEntity : SEntity
    {
        #region Constructor
        string _errorMsg = string.Empty;
        public EquipmentStateEventsEntity()
        {

        }
        #endregion


        /// <summary>
        /// 设备状态事件记录
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        public bool UpdateEquipmentStateEvents(DataSet dsSave)
        {
            bool blReturn = true;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    DataSet dsReturn = serverFactory.CreateIEquipmentStateEventsEngine().InsertEquipmentStateEvents(dsSave);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (_errorMsg.Length > 0)
                    {
                        blReturn = false;
                        MessageService.ShowError(_errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                blReturn = false;
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            #region Process Output Parameters

            return blReturn;

            #endregion
        }

        public DataSet GetCurrentEquipment(string strEquipmentKey)
        {
            DataSet resDS = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentStateEventsEngine().GetCurrentEquipment(strEquipmentKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);
                    if (_errorMsg.Length > 0)
                    {
                        MessageService.ShowError(_errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #region Process Output Parameters

            return resDS;

            #endregion
        }

        //add by qym 20120606 17:51
        public DataSet GetCurrentEquipment2(string strEquipmentKey,string strUserid)
        {
            DataSet resDS = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    resDS = serverFactory.CreateIEquipmentStateEventsEngine().GetCurrentEquipment2(strEquipmentKey, strUserid);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(resDS);
                    if (_errorMsg.Length > 0)
                    {
                        MessageService.ShowError(_errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            #region Process Output Parameters

            return resDS;

            #endregion
        }





    }
}

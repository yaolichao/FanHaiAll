using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    public class RptCommonEntity
    {
        #region Private variable definitions
        private string _errorMsg = string.Empty;
        #endregion

        #region Properties
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        #endregion

        public DataSet GetRptPlanAimData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetRptPlanAimData(hstable);
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

        public bool SaveRptPlanAimData(DataSet dsSave)
        {
            bool blBack = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
              DataSet  dsReturn = serverFactory.CreateIRptCommonEngine().SaveRptPlanAimData(dsSave);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (string.IsNullOrEmpty(_errorMsg))
                    blBack = true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;               
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return blBack;
        }

        public DataSet GetProWoModuleType()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetProWoModuleType();
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
        /// 获得工序排班数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetOptSettingData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetOptSettingData(hstable);
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
        /// 保存工序排班数据
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
        public bool SaveOptSettingData(DataSet dsSave)
        {
            bool blBack = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIRptCommonEngine().SaveOptSettingData(dsSave);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (string.IsNullOrEmpty(_errorMsg))
                    blBack = true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return blBack;
        }
        /// <summary>
        /// 获得班别数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetShiftName()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetShiftName();
               
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
        /// 获得所有工序数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetOperation()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetOperation();

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
        /// 获取厂别数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetFactoryDate()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetFactoryDate();

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
        /// 获取生产排班数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetFactoryShiftData(string sFactoryShiftSetKey, string sFactoryKey, string sDate, string sShiftValue, string sFactoryShiftName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetFactoryShiftData(sFactoryShiftSetKey, sFactoryKey, sDate, sShiftValue, sFactoryShiftName);

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
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        public DataSet UpdateData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().UpdateData(sql, sUpFuntionName);

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
        /// 获取线别
        /// </summary>
        /// <returns></returns>
        public DataSet GetLines()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIRptCommonEngine().GetLines();

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

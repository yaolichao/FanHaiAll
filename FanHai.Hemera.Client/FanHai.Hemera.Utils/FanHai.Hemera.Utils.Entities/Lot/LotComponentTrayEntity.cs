using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Interface;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class LotComponentTrayEntity : ILotComponentTrayEngine
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        #region ILotComponentTrayEngine 成员

        public bool InsertComponentTray(LotCustomerModel model)
        {
            bool isBool = false;

            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                isBool = serverFactory.CreateILotComponentTrayEngine().InsertComponentTray(model);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return isBool;
        }

        public DataSet SelectComponentTray(string trayValue)
        {
            DataSet dsReturn = null;

            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotComponentTrayEngine().SelectComponentTray(trayValue);
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
        /// 线别托盘数据查询
        /// </summary>
        /// <param name="trayValue"></param>
        /// <param name="linekey"></param>
        /// <returns></returns>
        public DataSet SelectComponentTrayLine(string trayValue,string linekey)
        {
            DataSet dsReturn = null;

            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotComponentTrayEngine().SelectComponentTrayLine(trayValue,linekey);
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


        public int CreateTrayNouber()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 自动生产编号
        /// </summary>
        /// <param name="strTxt">编号类型</param>
        /// <param name="isAdd">是否增长</param>
        /// <returns></returns>
        public string GetShgCod(string strTxt, bool isAdd)
        {
            String strReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                strReturn = serverFactory.CreateILotComponentTrayEngine().GetShgCod(strTxt, isAdd);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return strReturn;
        }


        #endregion

        #region ILotComponentTrayEngine 成员


        public bool UpdateOpcValue(string strAddress, string strValue,string strDateTime)
        {
            bool isBool = false;

            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                isBool = serverFactory.CreateILotComponentTrayEngine().UpdateOpcValue(strAddress,strValue,strDateTime);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return isBool;
        }

        #endregion
    }
}

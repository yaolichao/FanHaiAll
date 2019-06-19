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
    public class LineSettingEntity
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

        /// <summary>
        /// 通过用户获取对应有权限的线别信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">线别名称</param>
        /// <returns>用户拥有权限的线别表集</returns>
        public DataSet GetLineByUserNameAndLineName(string userName, string lineName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILineSettingEngine().GetLineByUserNameAndLineName(userName, lineName);
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
        /// 通过线别主键获取对应的子线信息
        /// </summary>
        /// <param name="lineKey">线别主键</param>
        /// <returns>线别主键对应的子线的数据集合</returns>
        public DataSet GetSubLineByLineKey(string mainLineKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILineSettingEngine().GetSubLineByLineKey(mainLineKey);
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
        /// 通过用户获取对应有权限的工序信息
        /// </summary>
        /// <param name="userName">用户帐号</param>
        /// <param name="LineName">工序名称</param>
        /// <returns>用户拥有权限的工序表集</returns>
        public DataSet GetOperationByUserNameAndOperationName(string userName, string operationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILineSettingEngine().GetOperationByUserNameAndOperationName(userName, operationName);
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
        /// 对线别对应的修改进行进行保存
        /// </summary>
        /// <param name="dsLine">线别操作相关的表集</param>
        /// <returns>操作返回的表集信息</returns>
        public DataSet SaveLineInfo(DataSet dsLine)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILineSettingEngine().SaveLineInfo(dsLine);
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
        /// 检查选定的线别是否为组件允许过站的线别
        /// </summary>
        /// <param name="lotLineKey">绑定的线别主键</param>
        /// <param name="curLineKey">当前的线别主键</param>
        /// <param name="curOperationName">当前工序名称</param>
        /// <returns>操作结果</returns>
        public DataSet CheckLotLine(string lotLineKey, string curLineKey, string curOperationName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILineSettingEngine().CheckLotLine(lotLineKey, curLineKey, curOperationName);
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

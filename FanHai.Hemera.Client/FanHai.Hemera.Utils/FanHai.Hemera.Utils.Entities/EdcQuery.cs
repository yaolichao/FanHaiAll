/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
 * 20120809    YONGMING.QIAO     Create     增加采集查询并导出数据                   Q.001
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class EdcQuery : EntityObject
    {
        private string _errorMsg = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public  EdcQuery()
        {
        }

        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        /// <summary>
        /// 查询抽检点
        /// </summary>
        /// <returns></returns>
        public DataSet SearchEdcPoint()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().SearchEdcPoint();
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
        /// 根据车间捞取设备
        /// </summary>
        /// <param name="?"></param>
        public DataSet SearchEMS(string strFactoryRoom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().SearchEMS(strFactoryRoom);
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
        /// 根据车间捞取设备,其他参数
        /// </summary>
        /// <param name="?"></param>
        public DataSet SearchEMS(string strFactoryRoom,string groupKey, string equipmentKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().SearchEMS(strFactoryRoom, groupKey, equipmentKey);
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
        /// 查询抽检参数
        /// </summary>
        /// <returns></returns>
        public DataSet SearchParam( )
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().SearchParam();
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
        /// 查询抽检参数,根据
        /// </summary>
        /// <returns></returns>
        public DataSet SearchParam( string edcKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().SearchParam(edcKey);
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
        /// 根据条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public DataSet EDCValueQuery(DataTable dtParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                dsReturn = obj.CreateIEDCQuery().EDCValueQuery(dtParams);
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

        

        

    }
}

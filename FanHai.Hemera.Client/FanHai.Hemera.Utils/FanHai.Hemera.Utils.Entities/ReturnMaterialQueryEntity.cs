/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
   20120627    YONGMING.QIAO     Create     增加批次退料查询的实体类.         
 * 20120627    YONGMING.QIAO     Create     增加重载函数.                         Q.001
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
   public class ReturnMaterialQueryEntity:EntityObject
    {
       private string _errorMsg = "";
       public string ErrorMsg
       {
            get
            {
                return this._errorMsg;
            }
        }

       /// <summary>
       /// 捞取批次退料的记录
       /// </summary>
       /// <returns></returns>
        public DataSet GetReturnMaterial()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIReturnMaterialQueryEngine().GetReturnMaterial();
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return dsReturn;
        }

       /// <summary>
        /// 捞取批次退料的记录,加了条件
       /// </summary>
       /// <param name="dsSearch"></param>
       /// <returns></returns>
        /// Q.001
        public DataSet GetReturnMaterial(DataSet dsSearch)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIReturnMaterialQueryEngine().GetReturnMaterial(dsSearch);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return dsReturn;
        }


    }
}

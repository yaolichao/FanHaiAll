/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
   2014.12.4    chao.pang       Create     增加批次退料查询的实体类.         
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
    public class SendingMaterialEntity : EntityObject
    {
       private string _errorMsg = "";
       public string ErrorMsg
       {
            get
            {
                return this._errorMsg;
            }
        }

       /// <summary>捞取维护的参数对应扣料信息
       /// 捞取维护的参数对应扣料信息
       /// </summary>
       /// <returns></returns>
       public DataSet GetParameters()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISendingMaterialEngine().GetParameters();
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

       public bool InsertNewInf(DataTable dtInf)
       {
           DataSet dsReturn = new DataSet();
           try
           {
               IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
               if (null != serverFactory)
               {
                   dsReturn = serverFactory.CreateISendingMaterialEngine().InsertNewInf(dtInf);
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
           if (string.IsNullOrEmpty(_errorMsg))
           {
               MessageService.ShowMessage("已成功发料到设备虚拟仓！", "${res:Global.SystemInfo}");  //系统提示发料成功
               return true;
           }
           else
           {
               MessageService.ShowError(_errorMsg);
               return false;
           }
       }

       public bool UpdateParameterInf(DataTable dtInf)
       {
           DataSet dsReturn = new DataSet();
           try
           {
               IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
               if (null != serverFactory)
               {
                   dsReturn = serverFactory.CreateISendingMaterialEngine().UpdateParameterInf(dtInf);
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
           if (string.IsNullOrEmpty(_errorMsg))
           {
               MessageService.ShowMessage("已成功从设备虚拟仓退料！", "${res:Global.SystemInfo}");  //系统提示退料成功
               return true;
           }
           else
           {
               MessageService.ShowError(_errorMsg);
               return false;
           }
       }

       public DataSet GetMatEquipmentStore(string facKey, string equipmentKey, string operationName, string lineKey, string parameterKey, string matCode, string orderNumber,string _type)
       {
           DataSet dsReturn = new DataSet();
           try
           {
               //创建远程调用的工厂对象。
               IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
               if (null != serverFactory)
               {
                   dsReturn = serverFactory.CreateISendingMaterialEngine().GetMatEquipmentStore(facKey, equipmentKey, operationName, lineKey, parameterKey, matCode, orderNumber,_type);
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

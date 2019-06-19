/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using System.Data.Common;

namespace FanHai.Hemera.Utils.Entities.BasicData
{
    public class MaterialBuckleControlEntity : EntityObject
    {
        private string _errorMsg = "";
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
        }
        /// <summary>删除信息
        /// 删除信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool DeleteInf(string parameter)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialBuckleControlEngine().DeleteInf(parameter);
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
                MessageService.ShowMessage("删除信息成功！", "${res:Global.SystemInfo}");  //系统提示删除成功
                return true;
            }
            else
            {
                MessageService.ShowError(_errorMsg);
                return false;
            }
        }
        /// <summary>通过参数名获取材料耗用设定基础表参数信息
        /// 通过参数名获取参数信息
        /// </summary>
        /// <param name="parameter">参数名</param>
        /// <returns></returns>
        public DataSet GetInfByParameter(string parameter)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialBuckleControlEngine().GetInfByParameter(parameter);
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
        /// <summary>新增信息到材料耗用设定基础表
        /// 新增信息到材料耗用设定基础表
        /// </summary>
        /// <param name="parameter">参数值</param>
        /// <param name="useqty">扣料数量</param>
        /// <param name="useunit">扣料单位</param>
        /// <param name="conrtastQty">扣料对应数量</param>
        /// <param name="conrtastUnt">扣料对应数量单位</param>
        /// <param name="name">创建人</param>
        /// <returns></returns>
        public bool InsertNewInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialBuckleControlEngine().InsertNewInf(parameter, useqty, useunit, conrtastQty, conrtastUnt, name);
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
                MessageService.ShowMessage("新增信息成功！", "${res:Global.SystemInfo}");  //系统提示删除成功
                return true;
            }
            else
            {
                MessageService.ShowError(_errorMsg);
                return false;
            }

        }
        /// <summary>新增信息到材料耗用设定基础表
        /// 新增信息到材料耗用设定基础表
        /// </summary>
        /// <param name="parameter">参数值</param>
        /// <param name="useqty">扣料数量</param>
        /// <param name="useunit">扣料单位</param>
        /// <param name="conrtastQty">扣料对应数量</param>
        /// <param name="conrtastUnt">扣料对应数量单位</param>
        /// <param name="name">创建人</param>
        /// <returns></returns>
        public bool UpdateParameterInf(string parameter, string useqty, string useunit, string conrtastQty, string conrtastUnt, string name)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialBuckleControlEngine().UpdateParameterInf(parameter, useqty, useunit, conrtastQty, conrtastUnt, name);
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
                MessageService.ShowMessage("修改信息成功！", "${res:Global.SystemInfo}");  //系统提示删除成功
                return true;
            }
            else
            {
                MessageService.ShowError(_errorMsg);
                return false;
            }
        }
        /// <summary>获取参数值
        /// 获取参数值
        /// </summary>
        /// <returns></returns>
        public DataSet GetParameter()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialBuckleControlEngine().GetParameter();
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities.BasicData
{

    public class ByProductEntity
    {
        private string _errorMsg = "";
        private string _key = string.Empty;
        private string _matnrmcode = string.Empty;
        private string _matnrbcode = string.Empty;
        private string _matnrbbmcode = string.Empty;

        public DataSet GetLotPartInf(string strCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //dsReturn = serverFactory.CreateIByProductEngine().GetLotPartInf(strCode);
                    dsReturn = serverFactory.Get<IByProductEngine>().GetLotPartInf(strCode);
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
        public DataSet GetByProductInf(DataSet dsreturn)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    //dsReturn = serverFactory.CreateIByProductEngine().GetByProductInf(dsreturn);
                    dsReturn = serverFactory.Get<IByProductEngine>().GetByProductInf(dsreturn);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete(string key)
        {
            if (key != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        //DataSet dsReturn = factor.CreateIByProductEngine().DeleteByProductCode(key);
                        DataSet dsReturn = factor.Get<IByProductEngine>().DeleteByProductCode(key);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");  //系统提示删除成功
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }

            }
            return true;
        }                  

        public bool InsertPro(DataSet dsSetIn)
        {
            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    //dsReturn = factor.CreateIByProductEngine().ProIsert(dsSetIn);
                    dsReturn = factor.Get<IByProductEngine>().ProIsert(dsSetIn);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    MessageService.ShowMessage("保存成功", "保存");    //操作成功!
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

            return true;
        }


        public bool UpdatePro(DataSet dsSetIn)
        {
            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    //dsReturn = factor.CreateIByProductEngine().ProUpdate(dsSetIn);
                    dsReturn = factor.Get<IByProductEngine>().ProUpdate(dsSetIn);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    MessageService.ShowMessage("修改保存成功", "保存");    //操作成功!
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

            return true;
        }
    }
}

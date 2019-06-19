using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
      
    public class NameplateLabelAutoPrintEntity
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }


        public DataSet GetInfoForNamepalteLabelAutoPrint(string lotNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateINameplateLabelAutoPrintEngine().getInfoForNamepalteLabelAutoPrint(lotNum);
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
        /// 根据序列号和功率档位查询对应档位的铭牌打印信息（体现功率档位） yibin.fei 2017.10.30
        /// </summary>
        /// <param name="lotNum"></param>
        /// <param name="AfterPower"></param>
        /// <returns></returns>
        public DataSet getInfoForNamepalteLabelAutoPrintForPowerShow(string lotNum, string AfterPower)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateINameplateLabelAutoPrintEngine().getInfoForNamepalteLabelAutoPrintForPowerShow(lotNum, AfterPower);
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


        public DataSet getTemplateByProdId(string prodId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateINameplateLabelAutoPrintEngine().getTemplateByProdId(prodId);
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

        public DataSet GetSizeForQTX(string prodId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateINameplateLabelAutoPrintEngine().getSizeForQTX(prodId);
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

        public DataSet GetInfoForTemplate()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateINameplateLabelAutoPrintEngine().getInfoForTemplate();
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


        public bool addTemplate(string prodId, string template, string editor, string editTime)
        {
            bool flag = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    flag = serverFactory.CreateINameplateLabelAutoPrintEngine().addTemplate( prodId,  template,  editor,  editTime);

                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;

                flag = false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return flag;
        }

        public bool updateTemplate(string prodId, string template, string editor, string editTime)
        {
            bool flag = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    flag = serverFactory.CreateINameplateLabelAutoPrintEngine().updateTemplate(prodId, template, editor, editTime);

                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;

                flag = false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return flag;
        }
    }
}

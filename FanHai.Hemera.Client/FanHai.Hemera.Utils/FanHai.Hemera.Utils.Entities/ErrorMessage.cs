using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
namespace FanHai.Hemera.Utils.Entities
{
    public class ErrorMessage : EntityObject
    {
        #region define attribute
        private string _errorMsg = string.Empty;

        
        #endregion

        #region Properties

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        #endregion

        #region Construct function

        public ErrorMessage()
        {
 
        }        

        #endregion

        #region Action

        public DataSet GetErrorMessageInfor(string strUser)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIStoreEngine().GetErrorMessageInfor(strUser); 
                    //dsReturn = serverFactory.CreateIWipJobAutoTrack().GetErrorMessageInfor(strUser); 
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class SplitArkEntity
    {
        private string _errorMsg = "";


        public bool SplitArk(DataSet ds)
        {
            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISplitArkEngine().SplitArk(ds);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }

            return true;
        }
    }
}

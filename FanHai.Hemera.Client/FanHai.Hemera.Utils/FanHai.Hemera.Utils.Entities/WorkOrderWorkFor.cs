// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Chao.Pang           2012-04-11             新建
// =================================================================================
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
    public class WorkOrderWorkFor : EntityObject
    {
        private string _errorMsg = "";


        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }
        public bool GongDanBaoGong(DataTable tableParam)
        {
            DataSet dsRerurn;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsRerurn = serverFactory.CreateIWorkOrderWorkForEngine().GongDanBaoGong(tableParam);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                    //执行失败。
                    if (_errorMsg != string.Empty) return false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return false;
        }
    }
}

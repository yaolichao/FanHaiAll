//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-04-13            新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 与生产批次操作相关的实体类。
    /// </summary>
    public class LotEntity
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
         /// <summary>
        /// 更新批次号打印状态。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public bool UpdatePrintFlag(string lotNumber)
        {
            return UpdatePrintFlag(new List<string>(){lotNumber});
        }
        /// <summary>
        /// 更新批次号打印状态。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：更新成功。false：更新失败。</returns>
        public bool UpdatePrintFlag(IList<string> lotNumbers)
        {
            bool bResult = false;
            try
            {
                IServerObjFactory obj = CallRemotingService.GetRemoteObject();
                obj.CreateILotEngine().UpdatePrintFlag(lotNumbers);
                bResult = true;
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        } 
      
    }
}

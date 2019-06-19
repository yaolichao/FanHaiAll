using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    public class PorProductEntity
    {
        #region Private variable definitions
        private string _errorMsg = string.Empty;
        #endregion

        #region Properties
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        #endregion

        public DataSet GetPorProductData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPorProductEngine().GetPorProductData(hstable);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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


        public bool SavePorProductData(DataSet dsPorProduct)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIPorProductEngine().SavePorProductData(dsPorProduct);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (!string.IsNullOrEmpty(_errorMsg))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }
        /// <summary>
        /// 获得产品ID号对应的主等级信息
        /// </summary>
        /// <param name="proid"></param>
        /// <returns></returns>
        public DataSet GetProductDtlGrade(string proid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPorProductEngine().GetProductDtlGrade(proid);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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

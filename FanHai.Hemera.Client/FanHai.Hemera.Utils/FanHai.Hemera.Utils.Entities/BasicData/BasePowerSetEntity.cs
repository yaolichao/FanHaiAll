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
    public class BasePowerSetEntity
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

        public DataSet GetPowerSetData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIBasicPowerSetEngine().GetPowerSetData(hstable);
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

        public DataSet GetBasicPowerSetEngine_CommonData(string strFilter)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIBasicPowerSetEngine().GetBasicPowerSetEngine_CommonData(strFilter);
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
        public bool IsExistPowerSetData(DataTable dtInsertPowerSetData)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIBasicPowerSetEngine().IsExistPowerSetData(dtInsertPowerSetData);
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
        /// 判断子分档规则是否存在
        /// </summary>
        /// <param name="dtInsertPowerDtlData"></param>
        /// <returns></returns>
        public bool IsExistPowerDtlData(DataTable dtInsertPowerDtlData)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIBasicPowerSetEngine().IsExistPowerDtlData(dtInsertPowerDtlData);
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
        /// 判断子分档花色规则是否存在
        /// </summary>
        /// <param name="dtInsertPowerDtlData"></param>
        /// <returns></returns>
        public bool IsExistPowerDtlColorData(DataTable dtInsertPowerDtlData)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIBasicPowerSetEngine().IsExistPowerDtlColorData(dtInsertPowerDtlData);
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

        public bool SavePowerSetData(DataSet dsPowerSet)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIBasicPowerSetEngine().SavePowerSetData(dsPowerSet);
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
        public DataSet GetPowerSetDtl(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIBasicPowerSetEngine().GetPowerSetDtl(hstable);
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

        public DataSet GetPowerLevelByLotNum(string slot)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                Hashtable hsParams = new Hashtable();
                hsParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, slot);
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIBasicPowerSetEngine().GetPowerLevelByLotNum(hsParams);
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

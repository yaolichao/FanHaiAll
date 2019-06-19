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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    public class IVTestDataEntity
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

        public DataSet GetIvTestData(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetIvTestData(hstable);
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

        public DataSet GetLabelInfo(string strFilter)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetLabelData(strFilter);
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

        public DataSet GetPorLotInfo(string sSN)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPorLotInfo(sSN);
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
        /// <summary>
        /// 获取批次测试记录。
        /// </summary>
        /// <param name="sSN">组件序列号。</param>
        /// <param name="sDefault">是否只获取当前有效记录， 空：否， 1：是。 </param>
        /// <returns>包含批次测试记录的数据集对象。</returns>
        public DataSet GetIVTestDateInfo(string sSN, string sDefault)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetIVTestDateInfo(sSN, sDefault);
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
        /// <summary>
        /// 是否重新计算衰减数据。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：是。false：否</returns>
        public bool IsRecalcDecayData(string lotNumber)
        {
            try
            {
                this._errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.CreateIPrintIvTestEngine().IsRecalcDecayData(lotNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }

        public DataSet GetPorProductData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPorProductData(sProductCode);
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

        public DataSet GetPrintLabelSetInfo(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPrintLabelSetInfo(sProductCode);
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
        /// <summary>
        /// 获取批次标签打印记录。
        /// </summary>
        /// <param name="sSN">批次号。</param>
        /// <param name="sLabelNo">标签号。</param>
        /// <returns>包含标签打印记录的数据集对象。</returns>
        public DataSet GetPrintLabelLogData(string sSN, string sLabelNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPrintLabelLogData(sSN, sLabelNo);
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
        /// <summary>
        /// 获取校准版测试时间。
        /// </summary>
        /// <param name="sSN">校准版序列号。</param>
        /// <param name="sDeviceNo">设备号。</param>
        /// <returns>包含校准版测试记录的数据集对象。</returns>
        public DataSet GetCalibrationMaxTTime(string sSN, string sDeviceNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetCalibrationMaxTTime(sSN, sDeviceNo);
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

        public DataSet GetTestRuleData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetTestRuleData(sProductCode);
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

        public DataSet GetDecoeffiData(string sProductCode, string sCoeffCode, string sPM)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetDecoeffiData(sProductCode, sCoeffCode, sPM);
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

        public DataSet GetProductModelData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetProductModelData(sProductCode);
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

        public DataSet GetTestRuleCtlParaData(string sProductCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetTestRuleCtlParaData(sProductCode);
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

        //public DataSet GetPowerSetData(string sProductCode, string sPM, string sPSSeq)
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        _errorMsg = string.Empty;
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerSetData(sProductCode,sPM,sPSSeq);
        //        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        CallRemotingService.UnregisterChannel();
        //    }
        //    return dsReturn;
        //}
        public DataSet GetPowerSetData(string lotNum, string sProductCode, string sPM, string sPSSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerSetData(lotNum, sProductCode, sPM, sPSSeq);
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
        public DataSet GetPowerSetDetailData(string sProductCode, string sPM, string sPSSeq, string sPSDSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerSetDetailData(sProductCode, sPM, sPSSeq, sPSDSeq);
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

        public DataSet GetTestRulePowerCtlData(string sProductCode, string sPM, string sPSSeq, string sSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetTestRulePowerCtlData(sProductCode, sPM, sPSSeq, sSeq);
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

        public DataSet UpdateData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().UpdateData(sql, sUpFuntionName);
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

        public DataSet AddData(string sql, string sUpFuntionName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().UpdateData(sql, sUpFuntionName);
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
        /// <summary>
        /// 获得查询数据-针对终检数据的查询
        /// </summary>
        /// <param name="hsParams">参数集合</param>
        /// <returns>返回数据集合</returns>
        public DataSet GetIvTestToCustCheckQuery(DataSet reqDS, int pageNo, int pageSize, out int pages, out int records, Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();
            pages = 0;
            records = 0;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetIvTestDataForCustCheckQuery(reqDS, pageNo, pageSize, out  pages, out  records, hsParams);
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

        public DataSet GetIvTestToCustCheckQueryForImport(Hashtable hsParams)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetIvTestToCustCheckQueryForImport(hsParams);
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


        public DataSet GetPackingListConergyData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListConergyData(sPalltNo);
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

        public DataSet GetPackingListSchuecoData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListSchuecoData(sPalltNo);
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

        public DataSet GetPackingListJapanData(string sPalltNo, string sCINumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListJapanData(sPalltNo, sCINumber);
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



        public DataSet GetPackingListCommonData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListCommonData(sPalltNo);
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

        public DataSet GetPackingListCommonDataSTS(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListCommonDataSTS(sPalltNo);
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

        public DataSet GetPackingListCommonDataQDCS(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListCommonDataQDCS(sPalltNo);
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
        public DataSet GetPackingListCommonDataAiji(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPackingListCommonDataAiji(sPalltNo);
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

        public DataSet GetFlashDataDelivery(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetFlashDataDelivery(sPalltNo);
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

        public DataSet GetPPSMasterData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPPSMasterData(sPalltNo);
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
        public DataSet GetPortMark(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPortMark(sPalltNo);
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

        //为体现标签/包装清单功率判断获取数据库数据 yibin.fei 2017.10.10
        public DataSet GetPowerShowData(string sWorkNo, string sSapNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerShowData(sWorkNo, sSapNo);
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

      

        public DataSet GetPPSCollectData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPPSCollectData(sPalltNo);
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

        public DataSet GetIVTestData2(string sWorkNum,string sStartSN, string sEndSN, string sStartDevice, string sEndDevice, string sStartDate, string sEndDate, string sDefault, string sVC_CONTROL)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetIVTestData2(sWorkNum,sStartSN, sEndSN, sStartDevice, sEndDevice, sStartDate, sEndDate, sDefault, sVC_CONTROL);
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

        public DataSet GetCustCheckData(string sSN, string sCustCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetCustCheckData(sSN, sCustCode, sRoomKey);
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

        public DataSet GetConsigmentDataBySN(string sSN, string sSideCode, string sCustomerCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetConsigmentDataBySN(sSN, sSideCode, sCustomerCode, sRoomKey);
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

        public DataSet GetBasicData(string sColumnType, string sColumnCode, string sColumnName, string sColumnNameDesc)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetBasicData(sColumnType, sColumnCode, sColumnName, sColumnNameDesc);
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

        public DataSet GetReasonCategoryData(string sCategoryType, string sCategoryName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetReasonCategoryData(sCategoryType, sCategoryName);
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

        public DataSet GetReasonData(string sCategoryKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetReasonData(sCategoryKey);
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

        public DataSet GetOQAData(string sSN, string sCustomCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetOQAData(sSN, sCustomCode, sRoomKey);
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

        public DataSet GetCustCheckSEQ(string sSN, string sCustomCode, string sRoomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetCustCheckSEQ(sSN, sCustomCode, sRoomKey);
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

        public DataSet GetFactoryInfo()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetFactoryInfo();
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

        public DataSet GetOQADataReport(string sFactoryKey, string sSNType, string sDefault, string sDateFalg, string sStartSN, string sEndSN, string sWO, string sPROID, string sStartDate, string sEndDate)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetOQADataReport(sFactoryKey, sSNType, sDefault, sDateFalg, sStartSN, sEndSN, sWO, sPROID, sStartDate, sEndDate);
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

        public DataSet GetWOAttributeValueByLotNum(string sLotNum, string sType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetWOAttributeValueByLotNum(sLotNum, sType);
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

        public DataSet GetPowerSetDetailDataByIMP(string sProductCode, string sPM, string sIMP, string sPSSeq, string sPSDSeq)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerSetDetailDataByIMP(sProductCode, sPM, sIMP, sPSSeq, sPSDSeq);
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

        public DataSet dsGetConergyPackgeData2(string sPalletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().dsGetConergyPackgeData2(sPalletNo);
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

        public DataSet dsGetPicPath(string sFactoryCode, string sPicType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().dsGetPicPath(sFactoryCode, sPicType);
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

        public DataSet GetCodeSoftLabelSet(string sLabelID)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetCodeSoftLabelSet(sLabelID);
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

        public DataSet GetPPSMasterImpData(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPPSMasterImpData(sPalltNo);
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
        /// <summary>
        /// 根据工单号获取工单设置的产品基本规则数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>包含工单产品基本规则数据的数据集对象。</returns>
        public DataSet GetWoProductData(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWoProductData(orderNumber);
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
        /// <summary>
        /// 根据工单号、产品料号和功率获取对应的产品衰减系数。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="pm">功率。</param>
        /// <returns>包含工单产品衰减系数数据的数据集对象。</returns>
        public DataSet GetDecayCoefficient(string orderNumber, string partNumber, decimal pm)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetDecayCoefficient(orderNumber, partNumber, pm);
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
        /// <summary>
        /// 根据工单号、产品料号和衰减后功率获取对应的分档数据。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="lotNo">组件批次号。</param>
        /// <param name="coefPM">衰减后功率。</param>
        /// <returns>包含对应分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetData(string orderNumber, string partNumber, string lotNo, decimal coefPM)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWOPowerSetData(orderNumber, partNumber, lotNo, coefPM);
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
        /// <summary>
        /// 根据工单主键、产品料号、分档主键和衰减后数据获取对应的子分档数据。
        /// </summary>
        /// <param name="workOrderKy">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <param name="val">根据子分档规则设置不同的值，如果是功率子分档设置为衰减后功率，如果是电流子分档设置为衰减后电流。</param>
        /// <returns>包含对应子分档数据的数据集对象。</returns>
        public DataSet GetWOPowerSetDetailData(string workOrderKy, string partNumber, string powersetKey, decimal val)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWOPowerSetDetailData(workOrderKy, partNumber, powersetKey, val);
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
        /// <summary>
        /// 根据工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间
        /// </summary>
        /// <param name="workOrderKy">工单主键</param>
        /// <param name="partNumber">产品料号</param>
        /// <param name="powersetKey">分档主键</param>
        /// <returns>工单主键、产品料号、分档主键获取对应子分档最大及最小功率区间数据</returns>
        public DataSet GetWOPowerSetDetailDataRang(string workOrderKy, string partNumber, string powersetKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWOPowerSetDetailDataRang(workOrderKy, partNumber, powersetKey);
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
        /// <summary>
        /// 根据批次号获取有效的IV测试数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <returns>包含批次数据及其IV测试数据的数据集对象。</returns>
        public DataSet GetIVTestData(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetIVTestData(lotNo);
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
        /// <summary>
        /// 根据工单号、产品料号、产品ID号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderNo">工单号。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="productId">产品ID号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        public DataSet GetWOPrintLabelDataByNo(string workOrderNo, string partNumber, string productId)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWOPrintLabelDataByNo(workOrderNo, partNumber, productId);
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
        /// <summary>
        /// 根据工单主键、产品料号获取对应的打印标签数据。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <returns>包含打印标签数据的数据集对象。</returns>
        public DataSet GetWOPrintLabelData(string workOrderKey, string partNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().GetWOPrintLabelData(workOrderKey, partNumber);
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
        /// <summary>
        /// 获取工单已生产的产品数量。
        /// </summary>
        /// <param name="workOrderKey">工单主键。</param>
        /// <param name="partNumber">产品料号。</param>
        /// <param name="powersetKey">分档主键。</param>
        /// <returns>产品数量。</returns>
        public decimal GetWOProductPowersetQty(string workOrderKey, string partNumber, string powersetKey)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<IPrintIvTestEngine>().GetWOProductPowersetQty(workOrderKey, partNumber, powersetKey);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return 0;
        }
        /// <summary>
        /// 保存打印数据。
        /// </summary>
        /// <param name="dsParams">包含打印数据的数据集对象。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        public DataSet SavePrintData(DataSet dsParams)
        {

            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPrintIvTestEngine>().SavePrintData(dsParams);
                _errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
        /// 是否允许打印标签。
        /// </summary>
        /// <param name="lotNumber">批次号。</param>
        /// <returns>true：允许；false：不允许。</returns>
        public bool IsAllowPrintLabel(string lotNumber)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                return serverFactory.Get<IPrintIvTestEngine>().IsAllowPrintLabel(lotNumber, out _errorMsg);
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
        }

        /// <summary>
        /// 通过工单号或者序列号获取对应工单的OEM信息
        /// </summary>
        /// <param name="orderNumber">托盘对应的工单号</param>
        /// <param name="lotNumber">组件序列号</param>
        /// <returns>工单对应的OEM信息</returns>
        public DataSet GetWorkOrderOEMByOrderNumberOrLotNumber(string orderNumber, string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIWorkOrders().GetWorkOrderOEMByOrderNumberOrLotNumber(orderNumber, lotNumber);
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


        /// <summary>
        /// 通过信息获取对应的SunEdison的信息
        /// </summary>
        /// <param name="hsSunEdison">查询条件信息</param>        
        /// <returns>通过信息获取对应的SunEdison的信息</returns>
        public DataSet GetSunEdisonList(Hashtable hsSunEdison)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetSunEdisonList(hsSunEdison);
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
        /// <summary>
        /// 通过批次号获取功率范围
        /// </summary>
        /// <param name="lotNum">批次号</param>
        /// <returns>功率信息</returns>
        public DataSet GetPowerRangeDate(string lotNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPowerRangeDate(lotNum);
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
        /// <summary>
        /// 通过工单主键，效率下限，效率上限，Ctm值获取对应工单设定的符合的ctm信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="lowCelleff">低效率</param>
        /// <param name="highCelleff">高效率</param>
        /// <param name="ctm">实际ctm值</param>
        /// <returns>符合要求的ctm数据集</returns>
        public DataSet GetCtmInfByWorderEffCtm(string workOrderKey, decimal lowCelleff, decimal highCelleff, decimal ctm)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetCtmInfByWorderEffCtm(workOrderKey, lowCelleff, highCelleff,ctm);
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

        public DataSet GetInefficientPARAM(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetInefficientPARAM(lotNumber);
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
        /// <summary>
        ///  通过产品类型获取效率
        /// </summary>
        /// <returns></returns>
        public DataSet GetProductCp(string procode, string conopj)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetProductCp(procode, conopj);
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


        public DataSet GetPPSMasterDataForMalai(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPPSMasterDataForMalai(sPalltNo);
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

        public DataSet GetPPSMalai(string sPalltNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetPPSMalai(sPalltNo);
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


        public DataSet GetLotNumMalai(string lotNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetLotNumMalai(lotNo);
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

        public DataSet GetLotNumsMalai(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetLotNumsMalai(palletNo);
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
        /// <summary>
        ///根据托号获取distinct的Color 判定花色是否混，主要用于金刚线和非金刚线的深和浅的问题
        ///金刚线组件：浅花、深花或者混花色（一个单托既有浅花又有深花，则为混花色）。
        ///非金刚线组件：浅蓝、深蓝或者混色（一个单托既有浅蓝又有深蓝，则为混色）。
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>distinct Color 的数据集</returns>
        public DataSet GetColorData(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetColorData(palletNo);
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

        public DataSet GetKingLineByPallet(string palletNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPrintIvTestEngine().GetKingLineByPallet(palletNo);
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

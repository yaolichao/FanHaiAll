using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 终检及以后工序的操作实体类
    /// </summary>
    public class LotAfterIvTestEntity
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }

        public DataSet SaveLot2CustCheckData(DataSet dsSave)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SaveLot2CustCheckData(dsSave);
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
        /// 根据工单号获取工单对应的属性信息
        /// </summary>
        /// <param name="orderNumber">工单号</param>
        /// <returns>工单对应的属性信息</returns>
        public DataSet GetOrderAttrByOrderNumber(string orderNumber)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetOrderAttrByOrderNumber(orderNumber);
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
        /// 根据批次号获取工单产品数据。
        /// </summary>
        /// <param name="s_lot">批次号。</param>
        /// <param name="roomkey">车间主键。</param>
        /// <returns>包含工单产品数据的数据集对象。</returns>
        public DataSet GetWOProductByLotNum(string s_lot, string roomkey)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetWOProductByLotNum(s_lot, roomkey);
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
        public DataSet GetModulePowerInfo(string Lot_Number)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetModulePowerInfo(Lot_Number);
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
        public DataSet GetCustCheckDataGroupByZero(string s_lot, string roomkey)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetCustCheckDataGroupByZero(s_lot, roomkey);
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

        public DataSet GetQueryPalletData(Hashtable hstable)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetLotPorWOForPallet(hstable);
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
        public DataSet GetPalletOrLotData(Hashtable hstable)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetPalletOrLotData(hstable);
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
        public DataSet SavePalletLotData(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SavePalletLotData(dsParams);
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
        /// 从入库检返回到包装
        /// </summary>
        /// <param name="dsPamas"></param>
        /// <returns></returns>
        public DataSet SavePallet2Package(DataSet dsPamas)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SavePallet2Package(dsPamas);
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

        public DataSet UpdateLotOutPallet(Hashtable hs)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().UpdateLotOutPallet(hs);
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
        public DataSet GetExistPalletLotNum(Hashtable hs)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().IsExistPalletLotNum(hs);
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
        /// 获得入库检信息数据
        /// </summary>
        /// <param name="hs">lot_num,palletno,roomkey</param>
        /// <returns></returns>
        public DataSet GetToWarehouseCheckData(Hashtable hs)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetToWarehouseCheckData(hs);
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
        /// 判断入库检验图片是否存在
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public DataSet IsExistModulePic(Hashtable hs)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().IsExistModulePic(hs);
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

        public DataSet GetPalletCustLotData(Hashtable hstable)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetPalletCustLotData(hstable);
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

        public DataSet SaveToWarehouseCheckData(DataSet dsSave)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SaveToWarehouseCheckData(dsSave);
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
        /// 获得服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSysdate()
        {
            DateTime dtime = new DateTime();
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            dtime = serverFactory.CreateILotEngine().GetSysdate();
            CallRemotingService.UnregisterChannel();
            return dtime;
        }

        public DataSet GetTestRulePackageLevel(Hashtable hstable)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetTestRulePackageLevel(hstable);
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
        /// 保存托号变更数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet SaveExchgPalletNumber(Hashtable hstable)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().SaveExchgPalletNumber(hstable);
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
        /// 根据获得的数据，判定批次是否为E工单(实验批次)
        /// </summary>
        /// <param name="hstable"></param>
        /// <param name="InputMarkForCustCheck">出站不良及备注，是否为必输项</param>
        /// <returns></returns>
        public bool IsExperimentLot(Hashtable hstable, out bool InputMarkForCustCheck)
        {
            bool blBack = false;
            InputMarkForCustCheck = true;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateILotEngine().GetLotWoProAttribute(hstable);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                DataTable dtAttribute = dsReturn.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                DataRow[] drsAttribute = null;
                  string[] l_s = new string[] {"Check_Name", "Is_Checked"};
                string category = BASEDATA_CATEGORY_NAME.Uda_Setting_ExperimentWo;
                DataTable dtCheckForExperiment = BaseData.Get(l_s, category);
                //混合检验
                DataRow[] drs01 = dtCheckForExperiment.Select("Check_Name='MinCheck_WoId' and Is_Checked='true'");
                //只检工单
                DataRow[] drs02 = dtCheckForExperiment.Select("Check_Name='OnlyCheck_Wo' and Is_Checked='true'");
                //只检产品ID
                DataRow[] drs03 = dtCheckForExperiment.Select("Check_Name='OnlyCheck_Id' and Is_Checked='true'");
                //检验E工单出站作业是否必输备注原因
                DataRow[] drs04 = dtCheckForExperiment.Select("Check_Name='InputMarkForCustCheck'");

                //工单，产品ID都检验
                if (drs01 != null && drs01.Length > 0)
                {
                    drsAttribute = dtAttribute.Select(string.Format(@"ISEXPERIMENT=1 and ATTRIBUTE_NAME='{0}'", WORKORDER_SETTING_ATTRIBUTE.IsExperimentWo));
                }
                else if (drs02 != null && drs02.Length > 0)
                {
                    drsAttribute = dtAttribute.Select(string.Format(@" ATTRIBUTE_NAME='{0}'", WORKORDER_SETTING_ATTRIBUTE.IsExperimentWo));
                }
                else if (drs03 != null && drs03.Length > 0)
                {
                    drsAttribute = dtAttribute.Select(string.Format(@"ISEXPERIMENT=1 "));
                }


                if (drsAttribute != null && drsAttribute.Length > 0)
                {
                    if (drs04 != null && drs04.Length > 0)
                        InputMarkForCustCheck = bool.Parse(Convert.ToString(drs04[0]["Is_Checked"]));

                    blBack = true;
                }
                else
                    blBack = false;               
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return blBack;
        }
        /// <summary>
        /// 检验批次是否为E工单批次
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public bool IsExperimentLot(Hashtable hstable)
        {
            bool InputMarkForCustCheck = false;
            return IsExperimentLot(hstable, out InputMarkForCustCheck);
        }
        /// <summary>
        /// 根据批次号及指定等级。获取批次对应工单中符合指定等级条件的推荐产品数据。
        /// </summary>
        /// <param name="lotNo">批次号。</param>
        /// <param name="grade">等级。</param>
        /// <returns>包含产品数据的数据集对象。</returns>
        public DataSet GetLotProductData(string lotNo,string grade)
        {
            DataSet dsReturn = null;
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateILotEngine().GetLotProductData(lotNo,grade);
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
        /// 通过序列号获取金刚线的值
        /// </summary>
        /// <param name="lotNumber"></param>
        /// <returns>金刚线的</returns>
        public DataSet GetKingLineInf(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIPorProductEngine().GetKingLineInf(lotNumber);
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

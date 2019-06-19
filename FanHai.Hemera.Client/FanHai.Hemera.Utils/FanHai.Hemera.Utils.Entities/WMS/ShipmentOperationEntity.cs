//----------------------------------------------------------------------------------
// Copyright (c) ASTRONERGY
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-26            新建
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
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 出货操作实体类。
    /// </summary>
    public class ShipmentOperationEntity
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
        /// 获取出货单当前已出货数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <returns>已出货数量。</returns>
        public double GetShipmentQuantity(string shipmentNo)
        {
            double quantity = 0;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                quantity = serverFactory.Get<IShipmentOperationEngine>().GetShipmentQuantity(shipmentNo);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return quantity;
        }
        /// <summary>
        /// 获取出货单中的指定柜号当前已入柜数量。
        /// </summary>
        /// <param name="shipmentNo">出货单号。</param>
        /// <param name="containerNo">货柜号。</param>
        /// <returns>指定柜号当前已入柜数量。</returns>
        public double GetContainerQuantity(string shipmentNo, string containerNo)
        {
            double quantity = 0;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                quantity = serverFactory.Get<IShipmentOperationEngine>().GetContainerQuantity(shipmentNo, containerNo);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return quantity;
        }
        /// <summary>
        /// 获取CI当前已出货数量。
        /// </summary>
        /// <param name="ciNumber">CI单号。</param>
        /// <returns>指定CI号当前已出货数量。</returns>
        public double GetCIQuantity(string ciNumber)
        {
            double quantity = 0;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                quantity = serverFactory.Get<IShipmentOperationEngine>().GetCIQuantity(ciNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return quantity;
        }
        /// <summary>
        /// 根据托盘号获取托盘信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        /// <returns>包含托盘信息的数据集对象。</returns>
        public DataSet GetPalletInfo(string palletNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().GetPalletInfo(palletNo);
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
        /// 托盘出货作业。
        /// </summary>
        /// <param name="dsParams">包含托盘操作信息的数据集对象。</param>
        public void PalletShipment(DataSet dsParams)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IShipmentOperationEngine>().PalletShipment(dsParams);
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
        }
        /// <summary>
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().Query(dsParams,ref config);
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
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <param name="config">分页配置对象。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet ShipmentQuery(DataSet dsParams, ref PagingQueryConfig config)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().ShipmentQuery(dsParams, ref config);
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
        /// 查询出货记录。
        /// </summary>
        /// <param name="dsParams">包含查询条件的数据集。</param>
        /// <returns>包含出货信息的数据集对象。</returns>
        public DataSet Query(DataSet dsParams)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().Query(dsParams);
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
        /// 从BCP数据库中根据指定托盘号导入托盘数据。
        /// </summary>
        /// <param name="palletNo">托盘号。可以使用逗号分隔的多个托盘号。</param>
        public void ImportPalletDataFromBCP(string palletNo)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IShipmentOperationEngine>().ImportPalletDataFromBCP(palletNo);
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
        }

        public bool UpdateConteinerNo(DataSet dssetin)
        {
            DataSet dsRerurn;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsRerurn = serverFactory.Get<IShipmentOperationEngine>().UpdateConteinerNo(dssetin);
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

        public bool SelectShipmentInf(string ShipmentNum)
        {
            DataSet dsRerurn;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsRerurn = serverFactory.Get<IShipmentOperationEngine>().SelectShipmentInf(ShipmentNum);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                    //执行失败。
                    if (_errorMsg != string.Empty) return false;
                    if (dsRerurn != null && dsRerurn.Tables.Count > 0 && dsRerurn.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
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
        /// <summary>
        /// 根据出货单号查询出货信息
        /// </summary>
        /// <param name="sNum">出货单号</param>
        /// <returns></returns>
        public DataSet GetShipmentInf(string sNum)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().GetShipmentInf(sNum);
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

        public bool DeleteShipMentInf(string ShipmentNum,string name)
        {
            bool dsRerurn = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsRerurn = serverFactory.Get<IShipmentOperationEngine>().DeleteShipMentInf(ShipmentNum,name);
                    return dsRerurn;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                return dsRerurn;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsRerurn;
        }
        /// <summary>
        /// 新增出货清单行数据
        /// </summary>
        /// <param name="dsParams"></param>
        public void PalletShipmentAdd(DataSet dsParams)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IShipmentOperationEngine>().PalletShipmentAdd(dsParams);
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
        }

        public void PassShipMentInf(DataSet dsParams,string name)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IShipmentOperationEngine>().PassShipMentInf(dsParams, name);
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
        }

        public void PalletShipmentAUpdate(DataSet dsParams)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.Get<IShipmentOperationEngine>().PalletShipmentAUpdate(dsParams);
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
        }
        /// <summary>
        /// 通过托盘号查询出货清单中是否存在可用记录
        /// </summary>
        /// <param name="palletNo">托盘号</param>
        /// <returns>数据集</returns>
        public DataSet GetShipMentNumByPallet(string palletNo)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().GetShipMentNumByPallet(palletNo);
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
        /// 根据出货单号进行冲销
        /// </summary>
        /// <param name="shipmentNo">出货单号</param>
        /// <returns></returns>
        public bool SterilisationShipment(string shipmentNo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.Get<IShipmentOperationEngine>().SterilisationShipment(shipmentNo);
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
            if (string.IsNullOrEmpty(_errorMsg))
            {
                MessageService.ShowMessage("已成功冲销，可回到出货界面进行出货！", "${res:Global.SystemInfo}");  //系统提示冲销成功
                return true;
            }
            else
            {
                MessageService.ShowError(_errorMsg);
                return false;
            }
        }

        public DataSet GetPalletShipInf(DataSet dsPalletShipInf)
        {
            DataSet dsReturn = null;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IShipmentOperationEngine>().GetPalletShipInf(dsPalletShipInf);
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
        public string GetShipMentBasicGOTO(string ciNumber)
        {
            string shipgoto = string.Empty;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                shipgoto = serverFactory.Get<IShipmentOperationEngine>().GetShipMentBasicGOTO(ciNumber);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return shipgoto;
        }
    }
}

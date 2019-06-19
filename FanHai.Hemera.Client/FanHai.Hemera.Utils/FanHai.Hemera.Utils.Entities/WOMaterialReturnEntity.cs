using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class WOMaterialReturnEntity : EntityObject
    {
        //私有成员
        private string _errorMsg = "";
      
        //属性
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

        //---------------------------------------------第一个界面的用到的方法
        //取得当前班别
        public DataSet  GetCurrentShift()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetCurrentShift();
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

        //根据物料批号获取物料信息
        public DataSet GetMatLotInfo(string MatLot)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetMatLotInfo(MatLot);
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

        //根据工厂车间获取工厂别      
        public DataSet GetFacRoomtoFac(string strFacRoom)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetFacRoomtoFac(strFacRoom);
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

        //根据前置码获得最大流水号
        public DataSet GetRetMatNo(string strPrex)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetRetMatNo(strPrex);
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

        //保存更新数据
        public bool Save(string strRetMatNo, string strRetMatDate, string strShift, string strOperator, 
                         string strRetMatReason, DataTable dtMatLotList)
        {
            bool blReturn = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    blReturn = serverFactory.CreateIWOMaterialReturnEngine().Save( strRetMatNo,  strRetMatDate,  strShift,  strOperator,  strRetMatReason,  dtMatLotList);
                   // _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return blReturn;
        }


        //loading时根据拥有权限获得所有退料信息
        public DataSet GetRetMatInfo(string strStore, string strOperation)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetRetMatInfo(strStore, strOperation);
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


        //按照条件得到退料信息，查询的结果
        public DataSet GetWoRetMatInof(string strMatLot, string strMatCode, string strMatDes, string strOperation, 
                                       string strStore, string strFacRoom, string strSupplier, string strShift, 
                                       string strOperator,  string strFromRetDate, string strToRetDate,string strRetMatNo ,
                                       string strOperationALL,string  strStoreALL)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetWoRetMatInof(strMatLot, strMatCode, strMatDes, strOperation, strStore, strFacRoom,
                        strSupplier, strShift, strOperator, strFromRetDate, strToRetDate, strRetMatNo, strOperationALL, strStoreALL);
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

        //根据退料单得到退料信息
        public DataSet GetRetMatInfo1(string strRetMatList)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsReturn = serverFactory.CreateIWOMaterialReturnEngine().GetRetMatInfo1(strRetMatList);
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

        //删除
        public bool DeleteMat(DataSet dsIMPORT)
        {
            bool blReturn = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    blReturn = serverFactory.CreateIWOMaterialReturnEngine().DeleteMat(dsIMPORT);
                    // _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return blReturn;
        }

        //删除2
        public bool DeleteMat2(string strReturnNo)
        {
            bool blReturn = false;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    blReturn = serverFactory.CreateIWOMaterialReturnEngine().DeleteMat2(strReturnNo);
                    // _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
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
            return blReturn;
        }
    }
}

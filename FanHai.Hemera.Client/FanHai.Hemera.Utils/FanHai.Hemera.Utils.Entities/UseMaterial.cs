// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Chao.Pang           2012-03-15             新建
// Chao.Pang           2012-03-16             修改,新增
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
    public class UseMaterial:EntityObject
    {
        #region define attribute
        private string _errorMsg = "";
        #endregion

        #region Properties

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

        #endregion


        #region 
        /// <summary>
        /// 根据线边仓名称获取工厂车间信息。
        /// </summary>
        public DataSet GetWorkShopInfo(string STORES)
        {
            DataSet dsMaterial = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterial = serverFactory.CreateIUseMaterialEngine().GetWorkShopInfo(STORES);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterial);
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
            return dsMaterial;

        }

        #endregion

        #region
        /// <summary>
        /// 根据工序名称和车间名称获取设备信息。
        /// </summary>
        public DataSet GetEquipmentInfo(string operationname, string cmbfactoryroom)
        {
            DataSet dsMaterial = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterial = serverFactory.CreateIUseMaterialEngine().GetEquipmentInfo(operationname, cmbfactoryroom);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterial);
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
            return dsMaterial;

        }

        #endregion


        #region
        /// <summary>
        /// 获取耗用物料信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetMaterialUsed(string operations,string stores)
        {
            DataSet dsMaterialUsed = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterialUsed = serverFactory.CreateIUseMaterialEngine().GetMaterialUsed(operations, stores);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterialUsed);
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
            return dsMaterialUsed;

        }
        #endregion
        #region
        /// <summary>
        /// 获取耗用物料详细信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetStoreMaterialDetail(string _materialLot, string _gongXuName, string _wuLiaoNumber, 
            string _factoryRoomName, string _wuLiaoMiaoShu, string _equipmentName, string _gongYingShang, 
            string _banCi, string _lineCang, string _jobNumber, DateTime _startTime, DateTime _endTime,string  _stores,string _operations)
        {
            DataSet dsMaterialDetail = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterialDetail = serverFactory.CreateIUseMaterialEngine().GetStoreMaterialDetail(_materialLot, _gongXuName, 
                        _wuLiaoNumber, _factoryRoomName, _wuLiaoMiaoShu, _equipmentName, _gongYingShang, _banCi, _lineCang, _jobNumber, 
                        _startTime, _endTime, _stores, _operations);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterialDetail);
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
            return dsMaterialDetail;
        }
        #endregion

        #region
        /// <summary>
        /// 获取线上仓材料数据
        /// </summary>
        /// <param name="materialLot">物料批次</param>
        /// <param name="operationName">工序名称</param>
        /// <param name="cmbFactoryRoom">工厂车间</param>
        /// <returns>dataset</returns>
        public DataSet GetMaterialByLotOpFa(string materialLot, string operationName, string cmbFactoryRoom)
        {
            DataSet dsMaterialDetail = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterialDetail = serverFactory.CreateIUseMaterialEngine().GetMaterialByLotOpFa(materialLot,operationName,cmbFactoryRoom);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterialDetail);
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
            return dsMaterialDetail;
        }
        #endregion
        #region
        /// <summary>
        /// 根据耗用主键获取材料耗用信息
        /// </summary>
        /// <param name="materialUsedDetalKey">耗用主键</param>
        /// <returns>dataset</returns>
        public DataSet GetMaterialDetailByKey(string materialUsedDetalKey)
        {

            DataSet dsMaterial = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterial = serverFactory.CreateIUseMaterialEngine().GetMaterialDetailByKey(materialUsedDetalKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterial);
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
            return dsMaterial;
        }
        #endregion

        #region
        /// <summary>
        /// 根据物料批次号和物料主键获取状态为1的物料信息
        /// </summary>
        /// <param name="materialLot"></param>
        /// <returns></returns>
        public DataSet GetMaterialDetailByMaterialLot(string materialLot, string materitalKey)
        {
            DataSet dsMaterialDetail = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsMaterialDetail = serverFactory.CreateIUseMaterialEngine().GetMaterialDetailByMaterialLot(materialLot, materitalKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsMaterialDetail);
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
            return dsMaterialDetail;
        }
        #endregion


        /// <summary>
        /// 修改耗用历史记录
        /// </summary>
        /// <param name="materialLot">物料批次号</param>
        /// <param name="operationName">工序名称</param>
        /// <param name="equipmentKey">设备主键</param>
        /// <param name="usedTime">耗用时间</param>
        /// <param name="stirTime">搅拌时间</param>
        /// <param name="printQty">印刷数量</param>
        /// <returns></returns>
        public bool UpDateMaterialUsedAndDetail(string materialLot, string materialUsedKey,string materialUsedKey1,
            string operationName, string equipmentKey, string usedTime, string stirTime, string printQty,DataTable hashTable)
        {
            bool boMaterialDetail = false;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    boMaterialDetail = serverFactory.CreateIUseMaterialEngine().UpDateMaterialUsedAndDetail(materialLot, materialUsedKey, materialUsedKey1, operationName, equipmentKey, usedTime, stirTime, printQty, hashTable);
                    if (boMaterialDetail == true)
                    {
                        MessageService.ShowMessage("材料历史耗用记录修改成功!", "${res:Global.SystemInfo}");
                        return boMaterialDetail;
                    }
                    else
                    {
                        MessageService.ShowMessage("材料历史耗用记录修改失败!", "${res:Global.SystemInfo}");
                        return boMaterialDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage("Update Error:"+ex.Message,"${res:Global.SystemInfo}");
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return boMaterialDetail;
        }

        /// <summary>
        /// 删除物料历史信息
        /// </summary>
        /// <param name="materialLot">物料批号</param>
        /// <param name="materialUsedKey">物料表主键</param>
        /// <returns></returns>
        public bool DeleteMaterital(string materialLot, string materialUsedKey, string materialUsedKey1)
        {
            bool boMaterialDetail = false;
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    boMaterialDetail = serverFactory.CreateIUseMaterialEngine().DeleteMaterital(materialLot, materialUsedKey, materialUsedKey1);
                    if (boMaterialDetail == true)
                    {
                        MessageService.ShowMessage("删除成功!", "${res:Global.SystemInfo}");
                        return boMaterialDetail;
                    }
                    else
                    {
                        MessageService.ShowMessage("删除失败!", "${res:Global.SystemInfo}");
                        return boMaterialDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage("Update Error:" + ex.Message, "${res:Global.SystemInfo}");
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return boMaterialDetail;
        }
        /// <summary>
        /// 新增物料耗用记录
        /// </summary>
        /// <param name="dssetin">dataset</param>
        /// <returns></returns>
        public bool InsertMaterial(DataSet dssetin)
        {
            DataSet dsRerurn ;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsRerurn = serverFactory.CreateIUseMaterialEngine().InsertMaterial(dssetin);
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
        /// <summary>
        /// 获取所有的耗用物料的信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllMaterialUsed()
        {
            DataSet dsAllMaterialUsed = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsAllMaterialUsed = serverFactory.CreateIUseMaterialEngine().GetAllMaterialUsed();
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsAllMaterialUsed);
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
            return dsAllMaterialUsed;
        }
    }
}

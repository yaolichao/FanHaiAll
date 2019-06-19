// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Chao.Pang           2012-03-26             新建
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
    public class OperationHandover
    {
        #region define attribute
        private string _errorMsg = "";
        #endregion

        #region
        /// <summary>
        /// 通过用户拥有权限的工序和线边仓的工厂车间获取工序交接班记录
        /// </summary>
        /// <param name="operations">工序</param>
        /// <param name="stores">线边仓</param>
        /// <returns></returns>
        public DataSet GetOperationHandoverBySAndF(string operations, DataTable dt)
        {
            DataSet dsOperationHandover = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsOperationHandover = serverFactory.CreateIOperationHandoverEngine().GetOperationHandoverBySAndF(operations, dt);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsOperationHandover);
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
            return dsOperationHandover;
        }
        #endregion


        #region
        /// <summary>
        /// 查询界面返回参数
        /// </summary>
        /// <param name="_lupFactoryRoom">工厂车间</param>
        /// <param name="_cmbGongXuName">工序名称</param>
        /// <param name="_lupJiaoBanShife">交接班次</param>
        /// <param name="_lupJieBanShife">接班班次</param>
        /// <param name="_timJiaoBanStart">交接时间起</param>
        /// <param name="_timJiaoBanEnd">交接时间末</param>
        /// <param name="_lupZhuangTai">状态</param>
        /// <param name="operations">用户拥有权限的工序</param>
        /// <param name="dt">工厂车间</param>
        /// <returns></returns>
        public DataSet GetOperationHandoverByReturn(string _lupFactoryRoom,string _cmbGongXuName,string _lupJiaoBanShife,string _lupJieBanShife,
            string _timJiaoBanStart,string _timJiaoBanEnd,string _lupZhuangTai, string operations, DataTable dt)
        {
            DataSet dsOperationHandover = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsOperationHandover = serverFactory.CreateIOperationHandoverEngine().GetOperationHandoverByReturn(_lupFactoryRoom, _cmbGongXuName, _lupJiaoBanShife, _lupJieBanShife,_timJiaoBanStart,_timJiaoBanEnd,_lupZhuangTai,operations, dt);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsOperationHandover);
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
            return dsOperationHandover;
        }
        #endregion



        /// <summary>
        /// 根据当前班次和当前日期获取上一班次和上一班的交班日期。根据上一班次、工厂车间、工序名称、上一班的交班日期获取上一班的交班记录
        /// </summary>
        /// <param name="_lupShift">当前班次</param>
        /// <param name="_lupGongXu">工序名称</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        /// <returns></returns>
        public DataSet GetShangBanShift(string _lupShift, string _lupGongXu, string _lupFacRoomKey)
        {
            DataSet dsShangBanShift = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsShangBanShift = serverFactory.CreateIOperationHandoverEngine().GetShangBanShift(_lupShift, _lupGongXu, _lupFacRoomKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsShangBanShift);
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
            return dsShangBanShift;
        }
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="_lupGongXu">工序</param>
        /// <param name="_lupFacRoomKey">工厂车间主键</param>
        public DataSet GetShangBan(string _lupGongXu, string _lupFacRoomKey)
        {

            DataSet dsShangBanShift = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsShangBanShift = serverFactory.CreateIOperationHandoverEngine().GetShangBan( _lupGongXu, _lupFacRoomKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsShangBanShift);
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
            return dsShangBanShift;
        }
        /// <summary>
        /// 通过工序交接主键获取物料信息和在制品信息
        /// </summary>
        /// <param name="key">工序交接班主键</param>
        /// <returns>物料信息表在制品信息表</returns>
        public DataSet GetWipAndMatByKey(string key)
        {
            DataSet dsGetWipAndMatByKey = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsGetWipAndMatByKey = serverFactory.CreateIOperationHandoverEngine().GetWipAndMatByKey(key);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsGetWipAndMatByKey);
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
            return dsGetWipAndMatByKey;
        }
        /// <summary>
        /// 获取当前班别的工序交接班信息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataSet GetDangQianShiftHandover(string shift,string operation,string factRoom)
        {
            DataSet dsGetDangQianShiftHandover = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsGetDangQianShiftHandover = serverFactory.CreateIOperationHandoverEngine().GetDangQianShiftHandover(shift,operation,factRoom);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsGetDangQianShiftHandover);
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
            return dsGetDangQianShiftHandover;
        }
        /// <summary>
        /// 没记录就在工序交接班表中插入一条记录
        /// </summary>
        /// <param name="dsSetIn"></param>
        public void InsertHandOver(DataSet dsSetIn)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().InsertHandOver(dsSetIn);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        /// <summary>
        ///通过根据工序和工厂车间获取所有线上仓中的物料信息（WST_STORE,WST_STORE_MATERIAL)插入到WST_OPERATION_HANDOVER_MAT中
        ///（数量全部设置为0）。根据工序和工厂车间获取所有工单的在制品信息（POR_LOT,POR_WORK_ORDER,WIP_TRANSACTION)插入到
        ///WST_OPERATION_HANDOVER_WIP中（数量全部设置为0）
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        /// <param name="actRoom">工厂车间</param>
        /// <param name="operation">工序名称</param>
        public void InsertHandOverMatAndWip(string handOverKey, string actRoom, string operation)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().InsertHandOverMatAndWip(handOverKey,actRoom,operation);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 通过工序交接班的主键然后获取WIP和物料的数量更新到表中
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        public void UpdateHandOverMatAndWip(string handOverKey)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().UpdateHandOverMatAndWip(handOverKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 通过工序交接班的主键更新期末数据
        /// </summary>
        /// <param name="handOverKey">工序交接班主键</param>
        public void UpdateHandOverMatAndWipQiMoShuLiang(string handOverKey)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().UpdateHandOverMatAndWipQiMoShuLiang(handOverKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 获取上一班的交接记录
        /// </summary>
        /// <param name="shift">当前班次</param>
        /// <param name="operation">工序名称</param>
        /// <param name="factRoom">工厂主键</param>
        /// <returns></returns>
        public DataSet GetShangYiBanHandOver(string shift, string operation, string factRoom)
        {
            DataSet dsShangBanShift = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsShangBanShift = serverFactory.CreateIOperationHandoverEngine().GetShangYiBanHandOver(shift, operation, factRoom);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsShangBanShift);
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
            return dsShangBanShift;
        }
        /// <summary>
        /// 交班后更新工序交接班内容和MAT内容
        /// </summary>
        /// <param name="dsSetIn">Hash表和界面MAT数据信息</param>
        public bool UpdateWipMatHandOverBySaveJiaoban(DataSet dsSetIn)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().UpdateWipMatHandOverBySaveJiaoban(dsSetIn);
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
        /// 交班后更新工序交接班内容
        /// </summary>
        /// <param name="dsSetIn1"></param>
        public bool UpdateHandOver(DataSet dsSetIn1)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().UpdateHandOver(dsSetIn1);
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
        /// 根据上一工序交接班主键获取上一工序交接班的期末数量插入到新生成的数据中的期初数量
        /// </summary>
        /// <param name="handDangqianOverKey">当前工序交接班主键</param>
        /// <param name="handOverKey">上一工序交接班主键</param>
        public void InsertMatWipQiChu(string handDangqianOverKey, string handOverKey)
        {
            DataSet dsRerurn;
            try
            {//创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {//调用远程方法，并处理远程方法的执行结果。
                    dsRerurn = serverFactory.CreateIOperationHandoverEngine().InsertMatWipQiChu(handDangqianOverKey,handOverKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsRerurn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        public DataSet GetFacKeyByFacName(string facName)
        {
            DataSet dsFacKey = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)//创建远程调用的工厂对象成功。
                {
                    //调用远程方法，并处理远程方法的执行结果。
                    dsFacKey = serverFactory.CreateIOperationHandoverEngine().GetFacKeyByFacName(facName);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsFacKey);
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
            return dsFacKey;
        }
    }
}

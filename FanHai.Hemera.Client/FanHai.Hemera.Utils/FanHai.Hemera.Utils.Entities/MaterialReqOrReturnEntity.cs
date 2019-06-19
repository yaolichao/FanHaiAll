/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
   2014.11.18    chao.pang       Create     增加批次退料查询的实体类.         
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Entities
{
    public class MaterialReqOrReturnEntity : EntityObject
    {
       private string _errorMsg = "";
       public string ErrorMsg
       {
            get
            {
                return this._errorMsg;
            }
        }

       /// <summary>捞取工单bom中工单对应的原材料 物料类型
       /// 捞取工单bom中工单对应的原材料 物料类型 < 200
       /// </summary>
       /// <returns></returns>
       public DataSet GetMaterials(string orderNum)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterials(orderNum);
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

       /// <summary>根据领料单号查询领料信息
        /// 根据领料单号查询领料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>领料单信息抬头表和明细表的信息</returns>
        public DataSet GetMatRequisitionInfByNum(string _numForSelect)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMatRequisitionInfByNum(_numForSelect);
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
        /// <summary>创建领料单
        /// 创建领料单
        /// </summary>
        /// <param name="dsIn">领料单抬头和明细信息</param>
        /// <returns></returns>
        public DataSet CreateRequistionKoPo(DataSet dsIn)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().CreateRequistionKoPo(dsIn);
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
        /// <summary>通过领料单号判定领料单号是否已经存在
        /// 通过领料单号判定领料单号是否已经存在
        /// </summary>
        /// <param name="number">领料单号</param>
        /// <returns>数量的数据集</returns>
        public DataSet GetCountByNumToCheck(string number,int flag)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetCountByNumToCheck(number,flag);
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

        /// <summary>修改领料单
        /// 修改领料单
        /// </summary>
        /// <param name="dsSave">明细数据的新增，删除数据集</param>
        /// <param name="_editer">修改人</param>
        /// <param name="_mblnr">领料单号</param>
        /// <returns></returns>
        public DataSet UpdateRequistionKoPo(DataSet dsSave, string _editer, string _mblnr)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().UpdateRequistionKoPo(dsSave, _editer, _mblnr);
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
        /// <summary>根据领料单号修改领料状态
        /// 根据领料单号修改领料状态
        /// </summary>
        /// <param name="mblnr">领料单号</param>
        public DataSet UpdateStatus(string mblnr,string editor)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().UpdateStatus(mblnr,editor);
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






        /// <summary>根据退料单号查询领料信息
        /// 根据退料单号查询领料信息
        /// </summary>
        /// <param name="_numForSelect"></param>
        /// <returns>退料单信息抬头表和明细表的信息</returns>
        public DataSet GetMatRequisitionInfByNumTui(string _numForSelect)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMatRequisitionInfByNumTui(_numForSelect);
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
        /// <summary>创建退料单
        /// 创建退料单
        /// </summary>
        /// <param name="dsIn">退料单抬头和明细信息</param>
        /// <returns></returns>
        public DataSet CreateRequistionKoPoTui(DataSet dsIn)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().CreateRequistionKoPoTui(dsIn);
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

        /// <summary>修改退料单
        /// 修改退料单
        /// </summary>
        /// <param name="dsSave">明细数据的新增，删除数据集</param>
        /// <param name="_editer">修改人</param>
        /// <param name="_mblnr">退料单号</param>
        /// <returns></returns>
        public DataSet UpdateRequistionKoPoTui(DataSet dsSave, string _editer, string _mblnr)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().UpdateRequistionKoPoTui(dsSave, _editer, _mblnr);
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
        /// <summary>根据退料单号修改领料状态
        /// 根据退料单号修改领料状态
        /// </summary>
        /// <param name="mblnr">退料单号</param>
        public DataSet UpdateStatusTui(string mblnr, string editor)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().UpdateStatusTui(mblnr, editor);
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
        
        /// <summary>
        /// 根据工单,物料代码,批次号 获取已经领料的信息
        /// </summary>
        /// <param name="workorder">工单号</param>
        /// <param name="mat">物料号</param>
        /// <param name="charg">批次号</param>
        /// <returns></returns>
        public DataSet GetMaterialstTui(string workorder,string mat,string num)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterialstTui(workorder, mat, num);
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

        /// <summary>
        /// 根据工单获取已经领料的物料信息
        /// </summary>
        /// <param name="workorder">工单号</param>
        /// <returns></returns>
        public DataSet GetMaterialstTui(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterialstTui(orderNumber);
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
        /// <summary>删除领料单
        /// 删除领料单
        /// </summary>
        /// <param name="_num">领料单号</param>
        /// <param name="name">修改人</param>
        /// <returns></returns>
        public DataSet DeleteNum(string _num, string name)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().DeleteNum(_num, name);
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

        public DataSet GetMaterialInf(DataTable paramTable, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterialInf(paramTable, ref config);
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

        public DataSet GetMaterialRequisitionList(DataTable paramTable, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterialRequisitionList(paramTable, ref config);
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

        public DataSet GetEquMaterialInf(DataTable paramTable, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetEquMaterialInf(paramTable, ref config);
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

        public DataSet GetMaterialSendingList(DataTable paramTable, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIMaterialReqOrReturnEngine().GetMaterialSendingList(paramTable, ref config);
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

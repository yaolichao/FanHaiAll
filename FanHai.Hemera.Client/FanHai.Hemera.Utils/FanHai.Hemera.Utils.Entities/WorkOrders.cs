//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间                   说明
// ---------------------------------------------------------------------------------
// 冯旭                 2012-02-07            新增“工单下达”所需的方法
// 冯旭                 2012-02-08            添加注释
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using System.Data;
using FanHai.Hemera.Share.Common;
using System.Collections;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 工单数据操作的实体类。
    /// </summary>
    public class WorkOrders
    {
        private string _errorMsg = "";
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        /// <summary>
        /// 根据工单号查询主键
        /// </summary>
        /// <param name="pWorkOrder"></param>
        /// <returns></returns>
        public string GetWorkOrderKeyByOrder(string pWorkOrder) 
        {
            string workOrderKey = string.Empty;
            DataSet dtData = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //用远程对象的方法，根据工单号查询主键
                dtData = serverFactory.CreateIWorkOrders().GetWorkOrderKeyByOrder(pWorkOrder);
                //DataSet中有表，且第一个表中有记录时
                if (dtData.Tables.Count > 0 && dtData.Tables[0].Rows.Count > 0) 
                {
                    //遍历第一个表中的每一条记录，取出每条记录的第一列赋给主键
                    //（？？疑问）：有多条记录时，workOrderKey值会被覆盖。
                    foreach (DataRow dr in dtData.Tables[0].Rows) 
                    {
                        workOrderKey = dr[0].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }

            return workOrderKey;
        }
        /// <summary>
        /// 从SAP接口数据库获取工单信息
        /// </summary>
        /// <param name="workOrder">工单号</param>
        /// <returns>工单信息</returns>
        public DataTable GetWorkOrder(String workOrder,String user)
        {
            //定义返回的工单信息表
            DataTable dtWorkOrder = CreateWorkOrderInfoTable();
            //调用远程对象为工单信息表赋值
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    //调用接口函数创建工单对象，并调用工单对象的获取工单信息方法，并在中间数据库写入读取标志。
                    DataSet retDS = factor.Get<ISAPEngine>().GetWorkOrderInfo(workOrder,user);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return retDS.Tables[0];
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dtWorkOrder;
        }
        /// <summary>
        /// 定义返回的工单信息表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateWorkOrderInfoTable()
        {
            //将需要的列加入到自定义表中
            List<string> fields = new List<string>() 
                                                    { 
                                                        POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY,
                                                        POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER,
                                                        POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER,
                                                        POR_WORK_ORDER_FIELDS.FIELD_DESCRIPTIONS,

                                                        POR_WORK_ORDER_FIELDS.FIELD_PART_REVISION,
                                                        POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_ORDERED,
                                                        POR_WORK_ORDER_FIELDS.FIELD_QUANTITY_LEFT,
                                                        POR_WORK_ORDER_FIELDS.FIELD_ORDER_PRIORITY,
                                                        POR_WORK_ORDER_FIELDS.FIELD_ORDER_STATE,
                                                        
                                                        POR_WORK_ORDER_FIELDS.FIELD_ENTERED_TIME,
                                                        POR_WORK_ORDER_FIELDS.FIELD_PLANNED_START_TIME,
                                                        POR_WORK_ORDER_FIELDS.FIELD_PLANNED_FINISH_TIME,
                                                        COMMON_FIELDS.FIELD_COMMON_CREATOR,
                                                        COMMON_FIELDS.FIELD_COMMON_EDITOR
                                                    };
            //使用指定字段名创建数据表对象。
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME, fields);
        }
        /// <summary>
        /// 从SAP接口数据库获取工单信息
        /// </summary>
        /// <returns>工单信息</returns>
        public DataTable GetWorkOrder(string user)
        {
            //定义返回的工单信息表
            DataTable dtWorkOrder = CreateWorkOrderInfoTable();
            //调用远程对象为工单信息表赋值
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    //调用接口函数创建工单对象，并调用工单对象的获取工单信息方法，并在中间数据库写入读取标志。
                    DataSet retDS = factor.Get<ISAPEngine>().GetWorkOrderInfo(user);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return retDS.Tables[0];
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dtWorkOrder;
        }
        
        /// <summary>
        /// 根据工厂车间名称获取工单号
        /// </summary>
        /// <param name="roomName">车间名称</param>
        /// <returns>包含工单号信息的数据集。</returns>
        public DataSet GetWorkOrderByFactoryRoom(string roomName)
        {
            DataSet retDS = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                retDS = factor.CreateIWorkOrders().GetWorkOrderByFactoryRoom(roomName);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return retDS;
        }
        /// <summary>
        /// 根据车间主键获取工单号。
        /// </summary>
        /// <param name="roomKey">车间主键。</param>
        /// <returns>
        /// 包含工单号信息的数据集合。【ORDER_NUMBER（工单号）】
        /// </returns>
        public DataSet GetWorkOrderByFactoryRoomKey(string roomKey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWorkOrderByFactoryRoomKey(roomKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号获取成品料号信息。
        /// </summary>
        /// <param name="orderNum">工单号。</param>
        /// <returns>
        /// 包含成品数据的数据集
        /// [PART_NAME, PART_DESC,PART_TYPE]
        /// </returns>
        public DataSet GetPartBytWorkOrder(String ordernum)
        {
            DataSet retDS = new DataSet();

            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    retDS = factor.CreateIWorkOrders().GetPartBytWorkOrder(ordernum);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return retDS;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return retDS;
        }
        /// <summary>
        /// 工单清单。
        /// </summary>
        /// <param name="pconfig">分页设置对象。</param>
        /// <returns>包含工单数据的数据集。</returns>
        public DataSet GetWorkOrderInfoList(ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderInfoList( ref config);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据条件获取工单数据。
        /// </summary>
        /// <param name="strFactory">工厂名称。</param>
        /// <param name="strWorkOrderNo">工单号。左匹配。</param>
        /// <param name="strPart">成品料号。左匹配。</param>
        /// <param name="strType">成品类型。</param>
        /// <param name="strStore">入库库位。</param>
        /// <param name="strStatus">状态。</param>
        /// <returns>包含工单数据的数据集。</returns>
        public DataSet  GetWorkOrderByCondition(string strFactory,string strWorkOrderNo,string strPart,string strType,
                                                string strStore, string strStatus, ref PagingQueryConfig config)
        {
            DataSet dsReturn=new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderByCondition(strFactory, strWorkOrderNo, strPart, 
                                                                                strType, strStore, strStatus,ref config);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 获取指定工步采集参数对应的工单参数设定数据。用于比对在采集时输入的数据是否符合工单参数设定。
        /// </summary>
        /// <param name="workorderKey">工单主键。</param>
        /// <param name="stepKey">工步主键。</param>
        /// <param name="dcType">数据采集时刻。0:进站时采集 1：出站时采集</param>
        /// <returns>包含工单参数设定数据的数据集对象。</returns>
        public DataSet GetWorkOrderParam(string workorderKey, string stepKey, OperationParamDCType dcType)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWorkOrderParam(workorderKey,stepKey,(int)dcType);
                this._errorMsg = ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                this._errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        public DataSet GetWorkOrderByNoOrProid(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderByNoOrProid(hstable);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }


        public DataSet GetWorkOrderAttrParamByOrderNumber(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderAttrParamByOrderNumber(workorderNumber);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }


        public DataSet GetWorkOrderAndAttrParamByKey(string workorderkey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderAndAttrParamByKey(workorderkey);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        public DataSet GetAllWorkOrderData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetAllWorkOrderData();
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    //若返回错误消息，则显示
                    if (returnMessage.Length > 0)
                    {
                        MessageService.ShowError(returnMessage);
                    }
                    else
                    {
                        return dsReturn;
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e);
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        public DataSet SaveWorkOrderAttrParam(DataSet dsWorkAttrParam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                _errorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().SaveWorkOrderAttrParam(dsWorkAttrParam);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);                   
                }
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        public DataSet DelWorkAttrDataBy2Key(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                _errorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().DelWorkAttrDataBy2Key(hstable);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号或者工单ID号工单属性名称，获得工单属性数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetWorkOrderAttributeValue(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                _errorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetWorkOrderAttributeValue(hstable);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 根据工单号获得工单所对应的客户类别
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        public DataSet GetViewForWorkOrder(Hashtable hstable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                _errorMsg = string.Empty;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIWorkOrders().GetViewForWorkOrder(hstable);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取工单对应的工单产品信息
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <returns>工单对应的工单产品信息的表集</returns>
        public DataSet GetWorkOrderProByOrderKey(string workOrderKey)
        {
            DataSet dsWOPro = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsWOPro = factor.CreateIWorkOrders().GetWorkOrderProByOrderKey(workOrderKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsWOPro);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsWOPro;
        }

        /// <summary>
        /// 通过主产品料号获取工单对应的料号信息
        /// </summary>
        /// <param name="mainPartNumber">主产品料号</param>
        /// <returns>工单对应的料号信息</returns>
        public DataSet GetPartNumberByMainPartNumber(string mainPartNumber)
        {
            DataSet dsPartNumber = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsPartNumber = factor.CreateIWorkOrders().GetPartNumberByMainPartNumber(mainPartNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsPartNumber);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsPartNumber;
        }

        /// <summary>
        /// 对工单对应的产品信息进行保存
        /// </summary>
        /// <param name="workOrderProInfo">工单对应的产品信息的集合</param>
        /// <returns>操作结果</returns>
        public DataSet SaveWorkOrderProInfo(DataSet workOrderProInfo)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().SaveWorkOrderProInfo(workOrderProInfo);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 通过分档代码获取对应的分档规则
        /// </summary>
        /// <param name="powerSetCode">分档代码</param>
        /// <returns>分档代码对应的功率分档的集合</returns>
        public DataSet GetPowerSetByPowerSetCode(string powerSetCode,string partMinPower,string partMaxPower)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetPowerSetByPowerSetCode(powerSetCode, partMinPower, partMaxPower);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 通过工单号获取接线盒信息的集合
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>接线盒信息的集合</returns>
        public DataSet GetWOJunctionBox(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWOJunctionBox(workorderNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }


        /// <summary>
        /// 通过工单号获取工单对应的工艺流程信息
        /// </summary>
        /// <param name="workorderNumber">工单号</param>
        /// <returns>工单对应的工艺流程信息</returns>
        public DataSet GetWorkOrderRouteInfo(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWorkOrderRouteInfo(workorderNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 通过工单获取OEM客户信息
        /// </summary>
        /// <param name="workorderNumber">工单信息</param>
        /// <returns>工单对应的OEM信息</returns>
        public DataSet GetWorkOrderOEMCustomer(string workorderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWorkOrderOEMCustomer(workorderNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        /// <summary>
        /// 获取工厂线别
        /// </summary>
        /// <returns>获取工厂线别的数据集合</returns>
        public DataSet GetFatoryLine()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetFatoryLine();
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
                
        /// <summary>
        /// 获取绑定的工厂线别是否正确
        /// </summary>
        /// <param name="workOrderKey">工单主键</param>
        /// <param name="lineKey">线别主键</param>
        /// <returns>True:绑定线别正确、或未绑定线别。False：绑定线别但是所选线别不在绑定范围</returns>
        public bool CheckWorkOrderLineBind(string LotNumber, string lineKey)
        {
            bool isBindTrueLine = true;
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                isBindTrueLine = factor.CreateIWorkOrders().CheckWorkOrderLineBind(LotNumber, lineKey);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return isBindTrueLine;
        }

        public bool isUpDataPrint(string WorkOrderNumber)
        {
            bool isUpData = true;
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                isUpData = factor.CreateIWorkOrders().isUpDataPrint(WorkOrderNumber);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return isUpData;
        }


        /// <summary>
        /// 通过LableID获取对应的打印类型
        /// </summary>
        /// <param name="lableID">LableID</param>
        /// <returns>该LableID对应的打印类型</returns>
        public string GetLablePrinterType(string lableID)
        {
            string printerType = string.Empty;
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                printerType = factor.CreateIWorkOrders().GetLablePrinterType(lableID);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return printerType;
        }


        public DataTable GetUpLowRule(string workOrderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetUpLowRule(workOrderNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn.Tables[0];
        }
        //工单打印信息
        public DataTable GetWoPrint(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //调用远程对象
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                dsReturn = factor.CreateIWorkOrders().GetWoPrint(orderNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception e)
            {
                _errorMsg = e.Message;
            }
            finally
            {
                //注销通道
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn.Tables[0];
        }
    }
}

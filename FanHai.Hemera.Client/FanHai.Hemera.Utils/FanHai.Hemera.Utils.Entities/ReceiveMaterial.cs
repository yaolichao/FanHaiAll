//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  yongbing.yang
//----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 来料接收的实体类。
    /// </summary>
    public class ReceiveMaterialEntity 
    {
        private string _errorMsg = string.Empty;
        /// <summary>
        /// 该类实例最近一次执行方法发生的错误消息。
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
        }
        /// <summary>
        /// 刷新来料数据
        /// </summary>
        /// <param name="dataTable">登录用户工号，所在时区时区</param>
        public void RefreshReceiveMaterial(DataTable dataTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.Get<ISAPEngine>().RefreshReceiveMaterial(dataTable);
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
        }
        /// <summary>
        /// 获取工厂通过线上仓
        /// </summary>
        /// <param name="dataTable">线上仓</param>
        public string GetFactoryByStore(string store)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetFactoryByStore(store);
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
            if (dsReturn.Tables[0].Rows.Count > 0)
            {
                return dsReturn.Tables[0].Rows[0][0].ToString();
            }
            else 
            {
                return "StoreFactoryNull";
            }
        }
        /// <summary>
        /// 获取工厂通过工单号
        /// </summary>
        /// <param name="dataTable">工单号</param>
        public string GetFactoryByOrderNumber(string orderNmber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetFactoryByOrderNumber(orderNmber);
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
            if (dsReturn.Tables[0].Rows.Count > 0)
            {
                return dsReturn.Tables[0].Rows[0][0].ToString();
            }
            else 
            {
                return "orderNmberFactoryNull";
            }
        }
        /// <summary>
        /// 来料接收
        /// </summary>
        /// <param name="dataTable">来料接收，选择工单信息</param>
        public void ReceiveLineMaterial(DataTable dataTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().ReceiveLineMaterial(dataTable);
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
        }
        /// <summary>
        /// 通过线上仓获取工序
        /// </summary>
        /// <param name="lineStore">线上仓名称</param>
        /// <returns>工序名称</returns>
        public string GetOperationByLineStore(string lineStore)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetOperationByLineStore(lineStore);
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
            return dsReturn.Tables[0].Rows[0]["OPERATION_NAME"].ToString().Trim();
        }
        /// <summary>
        /// 获取物料清单列表
        /// </summary>
        /// <param name="paramTable">包含查询条件的数据表。</param>
        /// <returns>获取物料清单列表</returns>
        public DataSet GetReceiveMaterialHistory(DataTable paramTable, ref PagingQueryConfig config)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetReceiveMaterialHistory(paramTable,ref config);
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
        /// 通过工序名称和车间主键获取线上仓
        /// </summary>
        /// <param name="operation">工序名称,</param>
        /// <param name="roomKey">车间主键,</param>
        /// <param name="stores">拥有权限的线上仓名称。使用逗号分隔。</param>
        /// <returns>线上仓集合</returns>
        public DataSet GetStores(string operation, string roomKey, string stores)
        {
            DataSet dsReturn = new DataSet();
            try{
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetStores(operation, roomKey,stores);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex){
                _errorMsg = ex.Message;
            }
            finally{
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 通过工序获取线上仓
        /// </summary>
        /// <param name="operation">工序名称。</param>
        /// <param name="stores">线上仓名称。使用逗号分隔。</param>
        /// <returns>包含线上仓名称的数据集。</returns>
        public DataTable GetStoreByOperation(string operation ,string stores)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetStoreByOperation(operation, stores);
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
            return dsReturn.Tables[0];
        }
        /// <summary>
        /// 通过物料编码获取线上仓
        /// </summary>
        /// <param name="workOrder">工单号。</param>
        /// <param name="materialCode">物料编码。</param>
        /// <param name="stores">线上仓名称。</param>
        /// <returns>包含线上仓名称的数据集。</returns>
        public DataTable GetStoreByMaterialCode(string workOrder,string materialCode, string stores)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetStoreByMaterialCode(workOrder,materialCode, stores);
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
            return dsReturn.Tables[0];
        }
        /// <summary>
        /// 获取硅片物料信息。
        /// </summary>
        /// <returns>
        /// 包含硅片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        public DataSet GetMaterials()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetMaterials();
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
        /// 获取产品号数据。
        /// </summary>
        /// <returns>包含产品号数据的数据集对象。</returns>
        public DataSet GetProdId()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IReceiveMaterialEngine>().GetProdId();
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
        /// 获取转换效率数据。
        /// </summary>
        /// <returns>包含转换效率数据的数据集对象。</returns>
        public DataSet GetEfficiency()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IReceiveMaterialEngine>().GetEfficiency();
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
        /// 获取领料项目（批次）号对应的信息。
        /// </summary>
        /// <param name="val">领料项目（批次）号。</param>
        /// <returns>包含领料项目（批次）号信息的数据集对象。</returns>
        public DataSet GetReceiveMaterialLotInfo(string val)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IReceiveMaterialEngine>().GetReceiveMaterialLotInfo(val);
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
        /// 获取硅片物料信息。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <returns>
        /// 包含硅片物料信息的数据集。
        /// 【MATERIAL_KEY,MATERIAL_NAME,MATERIAL_CODE,UNIT】
        /// </returns>
        public DataSet GetMaterials(string orderNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetMaterials(orderNumber);
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
        /// 获取供应商信息。
        /// </summary>
        /// <returns>
        /// 包含供应商信息的数据集。
        ///【CODE,NAME】
        /// </returns>
        public DataSet GetSuppliers()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().GetSuppliers();
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
        /// 根据车间名称获取领料仓库信息。
        /// </summary>
        /// <param name="roomName">车间名称。</param>
        /// <returns>包含领料仓库信息的数据表。</returns>
        public DataTable GetIssueStores(string roomName)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateILocationEngine().GetFactory(roomName);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);

                if (string.IsNullOrEmpty(_errorMsg)
                    &&dsReturn.Tables[0].Rows.Count>0)
                {
                    string factoryName = Convert.ToString(dsReturn.Tables[0].Rows[0]["FACTORY_NAME"]);
                    string[] columns = new string[] { "ERPRETURNSTORE" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.MEScontrastERP);
                    List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
                    lst.Add(new KeyValuePair<string, string>("MESFACTORY", factoryName));
                    return BaseData.GetBasicDataByCondition(columns, category, lst);
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
            return null;
        }
        /// <summary>
        /// 手工输入并保存接收的物料信息。
        /// </summary>
        /// <param name="ht">包含输入数据的哈希表对象。</param>
        /// <returns>包含执行结果的数据集。</returns>
        public DataSet ManualSaveReceiveMaterial(Hashtable ht)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreatIReceiveMaterialEngine().ManualSaveReceiveMaterial(ht);
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
        
        public DataTable CreatParamTable()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("CHARG");
            dtParams.Columns.Add("AUFNR");
            dtParams.Columns.Add("PRO_ID");
            dtParams.Columns.Add("EFFICIENCY");
            dtParams.Columns.Add("GRADE");
            dtParams.Columns.Add("LLIEF");
            dtParams.Columns.Add("STORE_NAME");
            dtParams.Columns.Add("RECEIVE_TIME_START");
            dtParams.Columns.Add("RECEIVE_TIME_END");
            dtParams.Columns.Add("DO");
            return dtParams;
        }
        /// <summary>
        /// 为工单BOM添加自备料。
        /// </summary>
        /// <param name="orderNumber">工单号。</param>
        /// <param name="materialCode">物料编码。</param>
        /// <param name="materialDescription">物料描述。</param>
        public void CreateWOBomOwnMaterial(string orderNumber, string materialCode, string materialDescription)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreatIReceiveMaterialEngine().CreateWOBomOwnMaterial(orderNumber, materialCode, materialDescription);
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
    }
}

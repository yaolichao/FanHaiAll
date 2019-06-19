/*
<FileInfo>
  <Author>Rayna Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
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
    public class SpcEntity
    {
        #region Private variable definitions
        private string _paramKey = string.Empty;
        private string _paramName = string.Empty;
        private string _errorMsg = string.Empty;
        private string _paramValue = string.Empty;
        //参数规格值
        private string uSL = string.Empty;
        private string sL = string.Empty;
        private string lSL = string.Empty;

        private string ucl = string.Empty;
        private string lcl = string.Empty;
        #endregion

        #region Properties
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg= value; }
        }
        public string ParamKey
        {
            get { return _paramKey; }
            set { _paramKey = value; }
        }
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
        public string ParamValue
        {
            get { return _paramValue; }
            set { _paramValue = value; }
        }

        //参数规格值
        /// <summary>
        /// 规格上线
        /// </summary>
        public string USL
        {
            get { return uSL; }
            set { uSL = value; }
        }
        /// <summary>
        /// 均值
        /// </summary>
        public string SL
        {
            get { return sL; }
            set { sL = value; }
        }
        /// <summary>
        /// 规格下限
        /// </summary>
        public string LSL
        {
            get { return lSL; }
            set { lSL = value; }
        }
        /// <summary>
        /// 控制上线
        /// </summary>
        public string UCL
        {
            get { return ucl; }
            set { ucl = value; }
        }
        /// <summary>
        /// 控制下限
        /// </summary>
        public string LCL
        {
            get { return lcl; }
            set { lcl = value; }
        }
        #endregion
      
        #region Spc params
        //public DataSet GetSpcParams()
        //{
        //    DataSet dsParams = null;
        //    try
        //    {
        //        IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
        //        dsParams=serverObj.CreateISpcEngine().GetParams();
        //        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsParams);               
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorMsg = ex.Message;
        //    }
        //    return dsParams;
        //}

        //public DataSet SearchParams()
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        DataSet dataSet = new DataSet();
        //        Hashtable paramHashTable = new Hashtable();
        //        if (_paramName.Length > 0)
        //        {
        //            paramHashTable.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, _paramName);
        //        }

        //        DataTable paramDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(paramHashTable);
        //        paramDataTable.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
        //        dataSet.Tables.Add(paramDataTable);

               
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

        //        if (serverFactory != null)
        //        {
        //            dsReturn = serverFactory.CreateISpcEngine().SearchParam(dataSet);
        //            _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);                   
        //        }
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

        //public bool SaveParams(DataSet dataSet)
        //{
        //    bool bReturn = true;
        //    try
        //    {
        //        DataSet dsReturn = new DataSet();
        //        IServerObjFactory serverObj=CallRemotingService.GetRemoteObject();
        //        dsReturn = serverObj.CreateISpcEngine().SaveParams(dataSet);
        //        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //        if (_errorMsg.Length > 0)
        //        {
        //            bReturn = false;
        //            MessageService.ShowError(_errorMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bReturn = false;
        //        MessageService.ShowError(ex.Message);
        //    }
        //    return bReturn;
        //}
        #endregion

        /// <summary>
        /// 保存抽检界面数据保存时，spc相应操作。
        /// </summary>
        /// <param name="dataset">包含SPC数据的数据集对象。</param>
        /// <returns>true：保存成功。false:保存失败。</returns>
        public bool SaveParamData(DataSet dataset)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn= serverFactory.CreateISpcEngine().SaveParamData(dataset);
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
        /// 数据采集界面方块电阻数据清空时，spc相应删除操作
        /// </summary>
        /// <returns></returns>
        public bool DeleteRParamData(string edcInskey)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateISpcEngine().DeleteRParamData(edcInskey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    throw new Exception(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }

        /// <summary>
        /// 撤销操作（UndoOUTEDC）时，Spc相应操作
        /// </summary>
        /// <param name="transactionKey"></param>
        /// <returns></returns>
        public bool DeleteParamData(string transactionKey)
        {

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateISpcEngine().DeleteParamData(transactionKey);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    throw new Exception(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
                return false;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return true;
        }

        #region Other Function
        public DataTable GetEquipments()
        {
            DataSet reqDS = new DataSet();
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENTS_FIELDS.FIELD_LOCATION_KEY, string.Empty);
            reqDS.ExtendedProperties.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_GROUP_KEY, string.Empty);

            string msg;
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.Equipments, FanHai.Hemera.Modules.EMS", "GetEquipments", reqDS, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                MessageService.ShowError("获取设备出错："+msg);
                return null;
            }
        }
        public DataSet GetSpcStandardSigma(Hashtable hashTable,string avg, out string err)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                err = string.Empty;
                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                if (serverObj != null)
                {
                    dsReturn = serverObj.CreateISpcEngine().GetStandardAndPSigma(dataTable,avg);
                    //if (chartType.Equals("XBAR-MR"))
                    //    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForMr(dataTable);
                    //else
                    //    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForXbar(dataTable);

                    err = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }
        /// <summary>
        /// 标准差，极差，均值图数据
        /// </summary>
        /// <param name="hashTable"></param>
        /// <returns></returns>
        public DataSet SearchParamValue(Hashtable hashTable, string chartType, out string err)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                err = string.Empty;
                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                if (serverObj != null)
                {
                    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForXbar(dataTable);
                    //if (chartType.Equals("XBAR-MR"))
                    //    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForMr(dataTable);
                    //else
                    //    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForXbar(dataTable);

                    err = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn;
        }

        //Q.003
        public DataSet GetTableData(string strControlCode, string pointkeys)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                if (serverObj != null)
                {
                    dsReturn = serverObj.CreateISpcEngine().GetTableData(strControlCode, pointkeys);
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
        //Q.0031
        public DataSet GetTableAvData(string strControlCode, string pointkeys)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                if (serverObj != null)
                {
                    dsReturn = serverObj.CreateISpcEngine().GetTableAvData(strControlCode, pointkeys);
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
        /// 移动极差数据数据
        /// </summary>
        /// <param name="hashTable"></param>
        /// <returns></returns>
        public DataSet SearchParamValueForMr(Hashtable hashTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                DataTable dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                if (serverObj != null)
                {
                    dsReturn = serverObj.CreateISpcEngine().SearchParamValueForMr(dataTable);
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
        /// 修改数据中的采集点
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-03-22 14:18:19
        public bool ModifyPoints(DataSet dataSet)
        {
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = new DataSet();
                dsReturn = serverObj.CreateISpcEngine().ModifyPoints(dataSet);

                //if (!isMr)
                //    dsReturn = serverObj.CreateISpcEngine().ModifyPoints(dataSet);
                //else
                //    dsReturn = serverObj.CreateISpcEngine().ModifyPointsForMr(dataSet);

                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length == 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return false;
        }
        /// <summary>
        /// 剔除图中异常的点
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        /// Owner genchille.yang 2012-03-21 10:37:33
        public bool DeletePoints(DataSet dataSet)
        {
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverObj.CreateISpcEngine().DeletePoints(dataSet);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length == 0)
                {
                    return true;
                }               
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return false;
        }
        /// <summary>
        /// 获得采集点明细信息
        /// </summary>
        /// <param name="edcInskey"></param>
        /// <returns></returns>
        public DataSet GetPointDetailInformation(string pointKey)
        {
            DataSet dsReturn = new DataSet();

            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().GetPointDetailInformation(pointKey);

            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得采集点信息
        /// </summary>
        /// <param name="edcInskey"></param>
        /// <returns></returns>
        public DataTable GetPointInformation(string edcInskey)
        {
            DataSet dsReturn = new DataSet();
            DataTable dt = new DataTable("PointInformation");
            DataColumn dc =null;

            dc = new DataColumn();
            dc.ColumnName = COMMON_FIELDS.FIELD_COMMON_KEY;
            dc.Caption = "要点";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = COMMON_FIELDS.FIELD_COMMON_VALUE;
            dc.Caption = "值";
            dt.Columns.Add(dc);

            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                 dsReturn = serverObj.CreateISpcEngine().GetPointInformation(edcInskey);

                 Dictionary<string, string> dicStr = new Dictionary<string, string>();
                 DataRow dr = dsReturn.Tables[0].Rows[0];
                 foreach (DataColumn dc01 in dsReturn.Tables[0].Columns)
                 {
                     dt.Rows.Add(dc01.ColumnName, dr[dc01.ColumnName].ToString());
                 }
                 dt.AcceptChanges();              
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 获得编辑信息
        /// </summary>
        /// <param name="edcInskey"></param>
        /// <returns></returns>
        public DataSet GetEditInformation(string edccolkey)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().GetEditInformation(edccolkey);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }
        #endregion
        /// <summary>
        /// 获得SPC选择的异常规则
        /// </summary>
        /// <param name="abnormalIds"></param>
        /// <returns></returns>
        /// Owner by genchille.yang 2012-03-26 16:12:39
        public DataSet GetAbnormalDetailRule(string abnormalIds)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISampEngine().GetAbnormalDetailRule(abnormalIds);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    MessageService.ShowError(_errorMsg);
                }
                else if (!dsReturn.Tables.Contains(EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME))
                {
                    MessageService.ShowError("异常规则表信息出错，请与管理员联系");
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return dsReturn;
        }

        #region 获取异常规则数据 
        /// <summary>
        /// 获得异常规则
        /// </summary>
        /// <returns></returns>
        /// Owner by genchille.yang 2012-03-12 17:10:22
        public DataSet GetAbnormalRule()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISampEngine().GetAbnormalRule();
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    MessageService.ShowError(_errorMsg);
                }
                else
                {
                    DataTable dtMain = null, dtDtl = null;
                    if (dsReturn.Tables.Contains(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME))
                        dtMain = dsReturn.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                    if (dsReturn.Tables.Contains(EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME))
                        dtDtl = dsReturn.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];
                    if (null == dtMain || null == dtDtl)
                    {
                        MessageService.ShowError("异常规则表信息出错，请与管理员联系");
                    }
                    else
                    {
                        dsReturn.Tables.Clear();
                        dsReturn.Tables.Add(dtMain);
                        dsReturn.Tables.Add(dtDtl);

                        dsReturn.Relations.Add("子规则明细", dtMain.Columns[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID], dtDtl.Columns[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID]);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return dsReturn;
        }
        ///// <summary>
        ///// 新增异常点规则
        ///// </summary>
        ///// <param name="dsAbnormalRule"></param>
        ///// <returns></returns>
        //public bool InsertAbnormalRule(DataSet dsAbnormalRule)
        //{
        //    bool blResult = true;
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
        //        dsReturn = serverObj.CreateISampEngine().InsertAbnormalRule(dsAbnormalRule);
        //        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //        if (_errorMsg.Length > 0)
        //        {
        //            blResult = false;
        //            MessageService.ShowError(_errorMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        blResult = false;
        //        MessageService.ShowError(ex.Message);
        //    }
        //    return blResult;
        //}

        ///// <summary>
        ///// 更新/删除 异常点规则
        ///// </summary>
        ///// <param name="dsAbnormalRule"></param>
        ///// <returns></returns>
        //public bool UpdateAbnormalRule(DataSet dsAbnormalRule)
        //{
        //    bool blResult = true;
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
        //        dsReturn = serverObj.CreateISampEngine().UpdateAbnormalRule(dsAbnormalRule);
        //        _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
        //        if (_errorMsg.Length > 0)
        //        {
        //            blResult = false;
        //            MessageService.ShowError(_errorMsg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        blResult = false;
        //        MessageService.ShowError(ex.Message);
        //    }
        //    return blResult;
        //}
        /// <summary>
        /// 保存异常规则---包括新增和更新
        /// </summary>
        /// <param name="dsAbnormalRule"></param>
        /// <returns></returns>
        public bool SaveAbnormalRule(DataSet dsAbnormalRule)
        {
            bool blResult = true;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISampEngine().SaveAbnormalRule(dsAbnormalRule);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    blResult = false;
                    MessageService.ShowError(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                blResult = false;
                MessageService.ShowError(ex.Message);
            }
            return blResult;
        }
        #endregion
        /// <summary>
        /// 判断异常规则代码是否存在
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public bool IsExistAbnormalCode(string[] strCode)
        {
            bool blResult = true;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISampEngine().IsExistAbnormalCode(strCode);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    blResult = false;
                    MessageService.ShowError(_errorMsg);
                }
                else if (Convert.ToInt32(dsReturn.ExtendedProperties["Code_Counts"].ToString())>0)
                {
                    blResult = false; 
                }
            }
            catch (Exception ex)
            {
                blResult = false;
                MessageService.ShowError(ex.Message);
            }
            return blResult;
        }
        #region 绑定管控计划基本查询条件
        public DataSet GetSpcControlPlan()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().GetSPControlData();
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    MessageService.ShowError(_errorMsg);
                }
                else
                {
                    DataTable dt01 = null, dt02 = null, dt03 = null, dt04 = null;
                    if (dsReturn.Tables.Contains(FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME))
                        dt01 = dsReturn.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
                    if (dsReturn.Tables.Contains(EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME))
                        dt02 = dsReturn.Tables[EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME];

                    if (dsReturn.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                        dt03 = dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                    if (dsReturn.Tables.Contains(BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME))
                        dt04 = dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    if (null == dt01 || null == dt02 || null == dt03 || null == dt04)
                    {
                        MessageService.ShowError("异常规则表信息出错，请与管理员联系");
                    }                   
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return dsReturn;
        }
        /// <summary>
        /// 获得SPC表格主数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetSPControlGridData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().GetSPControlGridData();
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    MessageService.ShowError(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return dsReturn;
        }
        #endregion

        #region 新增/修改SPC管控计划资料
        public bool SaveSpcControlPlan(DataSet dsSpcControl,out string controlCode)
        {
            controlCode = string.Empty;
            bool blResult = true;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().SaveSpcControlPlan(dsSpcControl);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    blResult = false;
                    MessageService.ShowError(_errorMsg);
                }
                if (dsReturn.ExtendedProperties.ContainsKey("CONTROLCODE"))
                    controlCode = dsReturn.ExtendedProperties["CONTROLCODE"].ToString();               
            }
            catch (Exception ex)
            {
                blResult = false;
                MessageService.ShowError(ex.Message);
            }
            return blResult;
        }
        #endregion
        /// <summary>
        /// 删除管控计划
        /// </summary>
        /// <param name="arrLst"></param>
        /// <returns></returns>
        public bool UpdateControlPlan(List<string> arrLst)
        {
            bool blResult = true;
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverObj = CallRemotingService.GetRemoteObject();
                dsReturn = serverObj.CreateISpcEngine().UpdateControlPlan(arrLst);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg.Length > 0)
                {
                    blResult = false;
                    MessageService.ShowError(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return blResult;
        }
        /// <summary>
        ///  获得SPC统计量表格
        /// </summary>
        /// <param name="dicStr"></param>
        /// <returns></returns>
        public DataTable AddRowToTable(Dictionary<string, string> dicStr)
        {
            DataTable dt = new DataTable(SPC_COUNT_FIELD.DATABASE_TABLE_NAME);
            dt.Columns.Add(COMMON_FIELDS.FIELD_COMMON_KEY);
            dt.Columns.Add(COMMON_FIELDS.FIELD_COMMON_VALUE);
           
            foreach (var dic in dicStr)
            {
                dt.Rows.Add(dic.Key, dic.Value);
            }
            return dt;
        }
    }
}

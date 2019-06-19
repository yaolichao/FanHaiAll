/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using System.Data;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Utils.Entities
{
    public class EdcManage : EntityObject
    {
        public EdcManage()
        {
            _edcKey = CommonUtils.GenerateNewKey(0);
        }

        public EdcManage(string edcKey)
        {
            _edcKey = edcKey;

            if (edcKey.Length > 0)
            {
                GetEdcByKey(edcKey);
            }
        }


        public bool Save(bool bNew)
        {
            //get all informations of Edc and put them into hashtable                    
            Hashtable edcHashTable = new Hashtable();
            DataSet dataSet = new DataSet();

            // Add "Edc Data" 
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDC_KEY, _edcKey);
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDC_NAME, _edcName);
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_STATUS, Convert.ToInt32(_edcStatus).ToString());
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_DESCRIPTIONS, _edcDescription);
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));

            DataTable edcDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(edcHashTable);
            edcDataTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(edcDataTable);

            if (paramTable.Rows.Count > 0)
            {
                dataSet.Merge(paramTable, false, MissingSchemaAction.Add);
            }


            //Call Remoting Service
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    if (bNew)
                    {
                        dsReturn = factor.CreateIEDCEngine().AddEdcAndParam(dataSet);
                    }
                    else
                    {
                        dsReturn = factor.CreateIEDCEngine().UpdateEdcAndParam(dataSet);
                    }
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

            return true;
        }

        public override bool Delete()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIEDCEngine().DeleteEdcAndParam(_edcKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");
                    }
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

            return true;
        }

        public override bool UpdateStatus()
        {
            //get all informations of Edc and put them into hashtable                    
            Hashtable edcHashTable = new Hashtable();
            DataSet dataSet = new DataSet();

            // Add "Edc Data" 
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_EDC_KEY, _edcKey);
            edcHashTable.Add(EDC_MAIN_FIELDS.FIELD_STATUS, Convert.ToInt32(_edcStatus).ToString());

            DataTable edcDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(edcHashTable);
            edcDataTable.TableName = EDC_MAIN_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(edcDataTable);

            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIEDCEngine().UpdateEdcAndParam(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:Global.UpdateStatusMessage}", "${res:Global.SystemInfo}");
                    }
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

            return true;
        }

        public void GetEdcByKey(string edcKey)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIEDCEngine().GetEdcByKey(edcKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        SetEdcProperties(dsReturn.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME]);
                        SetParamProperties(dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME]);
                    }
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

        private void SetEdcProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0 && 1 == dt.Rows.Count)
                {
                    _edcName = dt.Rows[0][EDC_MAIN_FIELDS.FIELD_EDC_NAME].ToString();
                    _edcDescription = dt.Rows[0][EDC_MAIN_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    _edcVersion = dt.Rows[0][EDC_MAIN_FIELDS.FIELD_EDC_VERSION].ToString();
                    _edcStatus = (EntityStatus)Convert.ToInt32(dt.Rows[0][EDC_MAIN_FIELDS.FIELD_STATUS]);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        private void SetParamProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    paramTable = dt;
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }

        public bool EdcNameValidate()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateIEDCEngine().GetDistinctEdcName();
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        foreach (DataRow dataRow in dsReturn.Tables[EDC_MAIN_FIELDS.DATABASE_TABLE_NAME].Rows)
                        {
                            if (_edcName == dataRow[0].ToString())
                                return false;
                        }
                    }
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

            return true;
        }
        /// <summary>
        /// 根据批次号获取批次数据采集信息。
        /// </summary>
        /// <param name="lotNumber">批次号码。</param>
        /// <returns>包含批次数据采集信息的数据集对象。该数据集对象包含两个数据表对象。
        /// 一个是包含批次数据采集信息的数据表对象。
        /// 一个名称为paraTable的数据表对象。paraTable的数据表对象存放查询的执行结果，
        /// paraTable数据表对象中包含两列“ATTRIBUTE_KEY”和“ATTRIBUTE_VALUE”。
        /// 最多包含两行：“ATTRIBUTE_KEY”列等于“CODE”的行和“ATTRIBUTE_KEY”列等于“MESSAGE”的行。
        /// </returns>
        /// comment by peter 2012-2-17
        public DataSet GetLotParamsCollection(string lotNumber)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                //创建远程调用的工厂对象。
                IServerObjFactory factory = CallRemotingService.GetRemoteObject();
                dsReturn = factory.CreateIEDCEngine().GetLotParamsCollection(lotNumber);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }

        public bool SaveEdcMainInfo(string LotKey, string edcPointKey)
        {
            bool bResult = false;
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                //DataSet dsReturn = factor.CreateIEDCEngine().SaveEdcMainInfo(null,LotKey, edcPointKey, PropertyService.Get(PROPERTY_FIELDS.USER_NAME),"","","");
                //_errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                //if (_errorMsg == string.Empty)
                //{
                //    bResult = true;
                //}
            }
            catch (Exception ex)
            {
                MessageService.ShowError("SaveEdcMainInfo Error" + ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        #region Properties

        public string EdcKey
        {
            get { return _edcKey; }
            set { _edcKey = value; }
        }

        public string EdcName
        {
            get { return _edcName; }
            set { _edcName = value; }
        }

        public string EdcDescription
        {
            get { return _edcDescription; }
            set { _edcDescription = value; }
        }

        public string EdcCollectionType
        {
            get { return _edcCollectionType; }
            set { _edcCollectionType = value; }
        }

        public string EdcTableName
        {
            get { return _edcTableName; }
            set { _edcTableName = value; }
        }

        public string EdcCreateTime
        {
            get { return _edcCreateTime; }
            set { _edcCreateTime = value; }
        }

        public string EdcEditor
        {
            get { return _edcEditor; }
            set { _edcEditor = value; }
        }


        public string EdcEditTime
        {
            get { return _edcEditTime; }
            set { _edcEditTime = value; }
        }

        public string EdcEditTimeZone
        {
            get { return _edcEditTimeZone; }
            set { _edcEditTimeZone = value; }
        }


        public string EdcSiteNumber
        {
            get { return _edcSiteNumber; }
            set { _edcSiteNumber = value; }
        }

        public string EdcVersion
        {
            get { return _edcVersion; }
            set { _edcVersion = value; }
        }

        public override EntityStatus Status
        {
            get { return _edcStatus; }
            set { _edcStatus = value; }
        }

        public DataTable ParamTable
        {
            get { return paramTable; }
            set { paramTable = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        #endregion
        #region Private variable definition
        //Private variable definition
        private string _edcKey = string.Empty;
        private string _edcName = string.Empty;
        private string _edcDescription = string.Empty;
        private string _edcCollectionType = string.Empty;
        private string _edcTableName = string.Empty;
        private string _edcCreateTime = string.Empty;
        private string _edcEditor = string.Empty;
        private string _edcEditTime = string.Empty;
        private string _edcEditTimeZone = string.Empty;
        private string _edcSiteNumber = string.Empty;
        private string _edcVersion = string.Empty;
        private string _errorMsg = string.Empty;
        private EntityStatus _edcStatus = EntityStatus.InActive;

        private DataTable paramTable = null;
        #endregion
    }
}

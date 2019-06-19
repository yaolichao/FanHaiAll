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
    public class SampManage:EntityObject
    {
        public SampManage()
        {
            _spKey = CommonUtils.GenerateNewKey(0);
        }

        public SampManage(string spKey)
        {
            _spKey = spKey;

            if (spKey.Length > 0)
            {
                GetSampByKey(spKey);
            }
        }

        public bool Save(bool bNew)
        {
            //get all informations of samp and put them into hashtable                    
            Hashtable spHashTable = new Hashtable();
            DataSet dataSet = new DataSet();

            // Add "Edc Data" 
            spHashTable.Add(EDC_SP_FIELDS.FIELD_SP_KEY, _spKey);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_SP_NAME, _spName);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_STATUS, Convert.ToInt32(_spStatus).ToString());
            spHashTable.Add(EDC_SP_FIELDS.FIELD_DESCRIPTIONS, _spDescription);

            spHashTable.Add(EDC_SP_FIELDS.FIELD_SAMPLING_MODE, _spMode);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_SAMPLING_SIZE, _spSize);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_PERCENT_FLAG, _spPercentFlag);

            spHashTable.Add(EDC_SP_FIELDS.FIELD_MAX_SAMPLING_SIZE, _spMaxSize);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_MODE, _spUnitMode);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_SIZE, _spUnitSize);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_STRATEGY_SIZE, _spStrategySize);

            DataTable spDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(spHashTable);
            spDataTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(spDataTable);

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
                        dsReturn = factor.CreateISampEngine().AddSamp(dataSet);
                    }
                    else
                    {
                        dsReturn = factor.CreateISampEngine().UpdateSamp(dataSet);
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

        public void GetSampByKey(string spKey)
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISampEngine().GetSampByKey(spKey);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        SetSampProperties(dsReturn.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME]);
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
        }

        private void SetSampProperties(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count>0 && 1 == dt.Rows.Count)
                {
                    _spName = dt.Rows[0][EDC_SP_FIELDS.FIELD_SP_NAME].ToString();
                    _spDescription = dt.Rows[0][EDC_SP_FIELDS.FIELD_DESCRIPTIONS].ToString();
                    _spVersion = dt.Rows[0][EDC_SP_FIELDS.FIELD_SP_VERSION].ToString();
                    _spStatus = (EntityStatus)Convert.ToInt32(dt.Rows[0][EDC_SP_FIELDS.FIELD_STATUS]);

                    _spMode = dt.Rows[0][EDC_SP_FIELDS.FIELD_SAMPLING_MODE].ToString();
                    _spSize = dt.Rows[0][EDC_SP_FIELDS.FIELD_SAMPLING_SIZE].ToString();
                    _spPercentFlag = dt.Rows[0][EDC_SP_FIELDS.FIELD_PERCENT_FLAG].ToString();

                    _spMaxSize = dt.Rows[0][EDC_SP_FIELDS.FIELD_MAX_SAMPLING_SIZE].ToString();
                    _spUnitMode = dt.Rows[0][EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_MODE].ToString();
                    _spUnitSize = dt.Rows[0][EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_SIZE].ToString();
                    _spStrategySize = dt.Rows[0][EDC_SP_FIELDS.FIELD_STRATEGY_SIZE].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
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
                    dsReturn = factor.CreateISampEngine().DeleteSamp(_spKey);
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
            //get status of samp and put them into hashtable                    
            Hashtable spHashTable = new Hashtable();
            DataSet dataSet = new DataSet();

            // Add "Edc Data" 
            spHashTable.Add(EDC_SP_FIELDS.FIELD_SP_KEY, _spKey);
            spHashTable.Add(EDC_SP_FIELDS.FIELD_STATUS, Convert.ToInt32(_spStatus).ToString());

            DataTable spDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(spHashTable);
            spDataTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
            dataSet.Tables.Add(spDataTable);

            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISampEngine().UpdateSamp(dataSet);
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

        public bool SampNameValidate()
        {
            try
            {
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISampEngine().GetDistinctSampName();
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        foreach (DataRow dataRow in dsReturn.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME].Rows)
                        {
                            if (_spName == dataRow[0].ToString())
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

        #region Properties

        public string SpKey
        {
            get { return _spKey; }
            set { _spKey = value; }
        }

        public string SpName
        {
            get { return _spName; }
            set { _spName = value; }
        }

        public string SpDescription
        {
            get { return _spDescription; }
            set { _spDescription = value; }
        }

        public string SpType
        {
            get { return _spType; }
            set { _spType = value; }
        }

        public string SpMode
        {
            get { return _spMode; }
            set { _spMode = value; }
        }

        public string SpSize
        {
            get { return _spSize; }
            set { _spSize = value; }
        }

        public string SpUnitMode
        {
            get { return _spUnitMode; }
            set { _spUnitMode = value; }
        }


        public string SpUnitSize
        {
            get { return _spUnitSize; }
            set { _spUnitSize = value; }
        }

        public string SpMaxSize
        {
            get { return _spMaxSize; }
            set { _spMaxSize = value; }
        }


        public string SpPercentFlag
        {
            get { return _spPercentFlag; }
            set { _spPercentFlag = value; }
        }

        public string SpVersion
        {
            get { return _spVersion; }
            set { _spVersion = value; }
        }

        public override EntityStatus Status
        {
            get { return _spStatus; }
            set { _spStatus = value; }
        }


        public string SpStrategyMode
        {
            get { return _spStrategyMode; }
            set { _spStrategyMode = value; }
        }

        public string SpStrategySize
        {
            get { return _spStrategySize; }
            set { _spStrategySize = value; }
        }
        #endregion
        #region Private variable definition
        //Private variable definition
        private string _spKey = string.Empty;
        private string _spName = string.Empty;
        private string _spDescription = string.Empty;
        private string _spType = string.Empty;
        private string _spMode = string.Empty;
        private string _spSize = string.Empty;
        private string _spUnitMode = string.Empty;
        private string _spUnitSize = string.Empty;
        private string _spMaxSize = string.Empty;
        private string _spPercentFlag = string.Empty;
        private string _spVersion = string.Empty;
        private string _spStrategyMode = string.Empty;
        private string _spStrategySize = string.Empty;

        private EntityStatus _spStatus = EntityStatus.InActive;
        #endregion
    }
}

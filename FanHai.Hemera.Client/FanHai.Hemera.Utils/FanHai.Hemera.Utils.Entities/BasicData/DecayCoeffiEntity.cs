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
    public class DecayCoeffiEntity
    {
        #region Private variable definitions

        private string _decoeffi_key = string.Empty;
        private string _d_name = string.Empty;
        private string _d_code = string.Empty;
        private string _d_code_desc = string.Empty;
        private string _coefficient = string.Empty;
        private string _coefficient_desc = string.Empty;
        private string _dit = string.Empty;
        private string _decoeffi_type = string.Empty;
        private string _creater = string.Empty;
        private string _create_time = string.Empty;
        private string _editor = string.Empty;
        private string _edit_time = string.Empty;
        private string _isflag = string.Empty;
        private string _errorMsg = string.Empty;

        #endregion

        #region Properties
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        public string DECOEFFI_KEY
        {
            get { return _decoeffi_key; }
            set { _decoeffi_key = value; }
        }
        public string D_NAME
        {
            get { return _d_name; }
            set { _d_name = value; }
        }
        public string D_CODE
        {
            get { return _d_code; }
            set { _d_code = value; }
        }
        public string D_CODE_DESC
        {
            get { return _d_code_desc; }
            set { _d_code_desc = value; }
        }
        public string COEFFICIENT
        {
            get { return _coefficient; }
            set { _coefficient = value; }
        }
        public string COEFFICIENT_DESC
        {
            get { return _coefficient_desc; }
            set { _coefficient_desc = value; }
        }
        public string DIT
        {
            get { return _dit; }
            set { _dit = value; }
        }
        public string DECOEFFI_TYPE
        {
            get { return _decoeffi_type; }
            set { _decoeffi_type = value; }
        }
        public string CREATER
        {
            get { return _creater; }
            set { _creater = value; }
        }
        public string CREATE_TIME
        {
            get { return _create_time; }
            set { _create_time = value; }
        }
        public string EDITOR
        {
            get { return _editor; }
            set { _editor = value; }
        }
        public string EDIT_TIME
        {
            get { return _edit_time; }
            set { _edit_time = value; }
        }
        public string ISFLAG
        {
            get { return _isflag; }
            set { _isflag = value; }
        }
        #endregion

        public bool IsExistDecayCoeffiData(DataTable dtInsertDecayCoeffi)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIDecayCoeffiEngine().IsExistDecayCoeffiData(dtInsertDecayCoeffi);
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


        public bool SaveDecayCoeffiData(DataSet dsDecayCoeffi)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIDecayCoeffiEngine().SaveDecayCoeffiData(dsDecayCoeffi);
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

        public DataSet GetDecayCoeffiData()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIDecayCoeffiEngine().GetDecayCoeffiData();
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

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
    public class ProductModelEntity
    {
        #region Private variable definitions

        private string _promodel_key = string.Empty;
        private string _promodel_name = string.Empty;
        private string _memo = string.Empty;
        private string _cell_area = string.Empty;
        private string _cell_num = string.Empty;
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

        public string PROMODEL_KEY
        {
            get { return _promodel_key; }
            set { _promodel_key = value; }
        }
        public string PROMODEL_NAME
        {
            get { return _promodel_name; }
            set { _promodel_name = value; }
        }
        public string MEMO
        {
            get { return _memo; }
            set { _memo = value; }
        }
        public string CELL_AREA
        {
            get { return _cell_area; }
            set { _cell_area = value; }
        }
        public string CELL_NUM
        {
            get { return _cell_num; }
            set { _cell_num = value; }
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

        public bool DelProductModel(DataSet dsProModel)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIProductModelEngine().DelProductModel(dsProModel);
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
        public bool IsExistProductModel(DataTable dtInsertProductModel)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIProductModelEngine().IsExistProductModel(dtInsertProductModel);
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
        public bool SaveProductModel(DataSet dsProModel)
        {
            try
            {
                _errorMsg = string.Empty;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                DataSet dsReturn = serverFactory.CreateIProductModelEngine().SaveProductModel(dsProModel);
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

        public DataSet GetProductModelAndCP()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIProductModelEngine().GetProductModelAndCP();
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
        public DataSet GetCertification()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIProductModelEngine().GetCertification();
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

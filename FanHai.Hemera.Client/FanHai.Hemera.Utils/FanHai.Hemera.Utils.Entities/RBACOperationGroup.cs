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
namespace FanHai.Hemera.Utils.Entities
{
    public class RBACOperationGroup
    {
        #region define attribute
        private string _operationGroupKey = "";
        private string _groupName = "";
        private string _descriptions = "";
        private string _creator = "";
        private string _createTimeZone = "";
        private string _editor = "";
        private string _editTimeZone = "";
        private string _remark = "";
        private string _errorMsg = "";
        #endregion

        #region Properties
        public string OperationGroupKey
        {
            get { return _operationGroupKey; }
            set { _operationGroupKey = value; }
        }
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        public string Descriptions
        {
            get { return _descriptions; }
            set { _descriptions = value; }
        }
        public string CreateTimeZone
        {
            get { return _createTimeZone; }
            set { _createTimeZone = value; }
        }
        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }
        public string EditTimeZone
        {
            get { return _editTimeZone; }
            set { _editTimeZone = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        #endregion

        #region Action
        public void AddOperationGroup(DataSet dataset)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIOperationGroupEngine().AddOperationGroup(dataset);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
        }

        public void DeleteOperationGroup()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIOperationGroupEngine().DeleteOperationGroup(_operationGroupKey);
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
        }

        public DataSet GetOperationGroup()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateIOperationGroupEngine().GetOperationGroup();
                    _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return dsReturn;
        }
        #endregion
    }
}

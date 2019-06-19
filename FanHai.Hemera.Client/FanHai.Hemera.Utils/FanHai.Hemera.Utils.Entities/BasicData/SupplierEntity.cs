/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
 * 20130730    CHAO.PANG         create      供应商管理                              Q.001
***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using System.Data.Common;

namespace FanHai.Hemera.Utils.Entities.BasicData
{
    public class SupplierEntity : EntityObject
    {
        private string _errorMsg = "";
        private string _supplierCode = string.Empty;
        private string _supplierName = string.Empty;
        private string _supplierNickName = string.Empty;
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0);
            }
        }

        public SupplierEntity() { }

        public SupplierEntity(string supplierCode)
        {
            _supplierCode = supplierCode;
        }

        public string SupplierCode
        {
            get { return _supplierCode; }
            set
            {
                _supplierCode = value;
                ValidateDirtyList("CODE", value);
            }
        }

        public string SupplierName
        {
            get { return _supplierName; }
            set
            {
                _supplierName = value;
                ValidateDirtyList("NAME", value);
            }
        }

        public string SupplierNickName
        {
            get { return _supplierNickName; }
            set
            {
                _supplierNickName = value;
                ValidateDirtyList("NICKNAME", value);
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetSupplierCode()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISupplierEngine().GetSupplierCode();
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            if (_supplierCode != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        DataSet dsReturn = factor.CreateISupplierEngine().DeleteSupplierCode(_supplierCode);
                        msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                        if (msg != string.Empty)
                        {
                            MessageService.ShowError(msg);
                            return false;
                        }
                        else
                        {
                            MessageService.ShowMessage("${res:Global.SuccessMessage}", "${res:Global.SystemInfo}");  //系统提示删除成功
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
            return true;
        }
        /// <summary>
        /// 查询获取数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetSupplierCode(string strSupplierName, string strSupplierCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISupplierEngine().GetSupplierCode(strSupplierName,strSupplierCode);
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
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeTable = DataTableHelper.CreateDataTableForInsertSupplierCode();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {"CODE",_supplierCode},
                                                        {"NAME",_supplierName},
                                                        {"NICKNAME",_supplierNickName},
                                                        {"CREATOR",PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}
                                                     
                                                    };

            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref codeTable, dataRow);
            dataSet.Tables.Add(codeTable);

            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISupplierEngine().SupplierCodeInsert(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }

                    this.ResetDirtyList();
                    MessageService.ShowMessage("新增保存成功", "保存");    //操作成功!
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
        public override bool Update(string lblCode)
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeTable = DataTableHelper.CreateDataTableForUpdateSupplierCode();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {"CODE",_supplierCode},
                                                        {"NAME",_supplierName},
                                                        {"NICKNAME",_supplierNickName},
                                                        {"CREATOR",PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}                                                     
                                                    };

            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref codeTable, dataRow);
            dataSet.Tables.Add(codeTable);

            try
            {
                int code = 0;
                string msg = string.Empty;
                DataSet dsReturn = null;
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    dsReturn = factor.CreateISupplierEngine().SupplierCodeUpdate(dataSet, lblCode);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }

                    this.ResetDirtyList();
                    MessageService.ShowMessage("修改保存成功", "保存");    //操作成功!
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
    }
}


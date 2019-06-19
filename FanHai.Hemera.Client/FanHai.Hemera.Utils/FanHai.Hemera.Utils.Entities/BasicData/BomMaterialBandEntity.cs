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
    public class BomMaterialBandEntity : EntityObject
    {
        private string _errorMsg = "";
        private string _code = string.Empty;
        private string _name = string.Empty;
        private string _barCode = string.Empty;
        private string _desc = string.Empty;
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0);
            }
        }

        public BomMaterialBandEntity() { }

        public BomMaterialBandEntity(string code)
        {
            _code = code;
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ValidateDirtyList("MATERIAL_CODE", value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ValidateDirtyList("MATERIAL_NAME", value);
            }
        }

        public string BarCode
        {
            get { return _barCode; }
            set
            {
                _barCode = value;
                ValidateDirtyList("BARCODE", value);
            }
        }

        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                ValidateDirtyList("MATERIAL_SPEC", value);
            }
        }


        public DataSet GetMaterialDateByCodeAndBarcode(string code, string barCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.Get<IBomMaterialBandEngine>().GetMaterialDateByCodeAndBarcode(code, barCode);
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

        public DataSet GetBomMaterial()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.Get<IBomMaterialBandEngine>().GetBomMaterial();
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
            if (_code != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        DataSet dsReturn = factor.Get<IBomMaterialBandEngine>().DeleteMaterialCode(_code);
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
        public override bool Insert()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeTable = DataTableHelper.CreateDataTableForInsertBomMarital();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {"MATERIAL_CODE",_code},
                                                        {"MATERIAL_NAME",_name},
                                                        {"BARCODE",_barCode},
                                                        {"MATERIAL_SPEC",_desc},
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
                    dsReturn = factor.CreateIBomMaterialBandEngine().MaterialDateInsert(dataSet);
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
        public override bool Update()
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeTable = DataTableHelper.CreateDataTableForUpdateBomMarital();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {"MATERIAL_CODE",_code},
                                                        {"MATERIAL_NAME",_name},
                                                        {"BARCODE",_barCode},
                                                        {"MATERIAL_SPEC",_desc},
                                                        {"EDITOR",PropertyService.Get(PROPERTY_FIELDS.USER_NAME)}                                                   
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
                    dsReturn = factor.CreateIBomMaterialBandEngine().MaterialDateUpdate(dataSet);
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


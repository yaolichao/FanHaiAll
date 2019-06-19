/***************************************************************************************************
     日期        Author           类型       Description                           标识
   ----------  --------------    ---------  ------------------------------------  ---------------
 * 20150128    CHAO.PANG         create      特殊物料管控                              Q.001
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
    public class SpecialMatTeamEntity : EntityObject
    {
        private string _errorMsg = "";
        private string _workOrderNum = string.Empty;
        private string _mat = string.Empty;
        private string _paramerTeam = string.Empty;
        private string _matDesc = string.Empty;
        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0);
            }
        }

        public SpecialMatTeamEntity() { }

        public SpecialMatTeamEntity(string workOrderNum,string mat,string paramerTeam)
        {
            _workOrderNum = workOrderNum;
            _mat = mat;
            _paramerTeam = paramerTeam;
        }

        public string WorkOrderNum
        {
            get { return _workOrderNum; }
            set
            {
                _workOrderNum = value;
                ValidateDirtyList("ORDER_NUMBER", value);
            }
        }

        public string Mat
        {
            get { return _mat; }
            set
            {
                _mat = value;
                ValidateDirtyList("MATERIAL_CODE", value);
            }
        }

        public string ParamerTeam
        {
            get { return _paramerTeam; }
            set
            {
                _paramerTeam = value;
                ValidateDirtyList("MATKL", value);
            }
        }
        public string MatDesc
        {
            get { return _matDesc; }
            set
            {
                _matDesc = value;
                ValidateDirtyList("DESCRIPTION", value);
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        //public DataSet GetSupplierCode()
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            dsReturn = serverFactory.CreateISupplierEngine().GetSupplierCode();
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            if (_workOrderNum != string.Empty && _mat != string.Empty && _paramerTeam != string.Empty)
            {
                //Call Remoting Service
                try
                {
                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                    if (null != factor)
                    {
                        string msg = string.Empty;
                        DataSet dsReturn = factor.CreateISpecialMatTeamEngine().DeleteMatSpecialInf(_workOrderNum,_mat,_paramerTeam);
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
        public override bool Update(string keyNum)
        {
            DataSet dataSet = new DataSet();
            //Add basic data
            DataTable codeTable = DataTableHelper.CreateDataTableForUpdateSpecialMatTeam();
            Dictionary<string, string> dataRow = new Dictionary<string, string>() 
                                                    {
                                                        {"ORDER_NUMBER",_workOrderNum},
                                                        {"MATERIAL_CODE",_mat},
                                                        {"DESCRIPTION",_matDesc},
                                                        {"MATKL",_paramerTeam}                                                     
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
                    dsReturn = factor.CreateISpecialMatTeamEngine().UpdateSpecialMatTeam(dataSet, keyNum);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);
                    if (code == -1)
                    {
                        MessageService.ShowError(msg);
                        return false;
                    }

                    this.ResetDirtyList();
                    MessageService.ShowMessage("保存成功", "保存");    //操作成功!
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

        public DataSet GetMatSpecialInf(string _workOrder, string _material, string _paramTeam)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISpecialMatTeamEngine().GetMatSpecialInf(_workOrder, _material, _paramTeam);
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

        public DataSet GetWorkNumber()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISpecialMatTeamEngine().GetWorkNumber();
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

        public DataSet GetMaterialByWorkOrder(string workOrder)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISpecialMatTeamEngine().GetMaterialByWorkOrder(workOrder);
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


        public DataSet GetMatSpecialInf()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISpecialMatTeamEngine().GetMatSpecialInf();
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

        public DataSet GetParamerTeam()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateISpecialMatTeamEngine().GetParamerTeam();
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
    }
}


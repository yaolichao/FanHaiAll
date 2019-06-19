using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Utils.Entities
{
    #region 注释掉了
    /// <summary>
    /// 测试分选客户端计算机的配置类。
    /// </summary>
    //public class ComputerConfig
    //{
    //    #region Private variable definition
    //    private string _codeKey = "";
    //    private string _codeId = "";
    //    private string _codeName = "";
    //    private string _codeState = "";
    //    private string _computerName = "";
    //    private string _areaNo = "";
    //    private string _lineId = "";
    //    private string _eqptId = "";
    //    private string _codeTime = "";
    //    private string _ext1 = "";
    //    private string _ext2 = "";
    //    private string _ext3 = "";
    //    private string _ext4 = "";
    //    private string _ext5 = "";
    //    private string _ext6 = "";
    //    private string _ext7 = "";
    //    private string _ext8 = "";
    //    private string _ext9 = "";
    //    private string _ext10 = "";
    //    private string _ext11 = "";
    //    private string _ext12 = "";
    //    private string _ext13 = "";
    //    private string _ext14 = "";
    //    private string _printerType = "";
    //    private string _errorMsg = "";
    //    #endregion

    //    #region property
    //    public string CodeKey
    //    {
    //        get { return _codeKey; }
    //        set { _codeKey = value; }
    //    }

    //    public string CodeId
    //    {
    //        get { return _codeId; }
    //        set { _codeId = value; }
    //    }
    //    public string CodeName
    //    {
    //        get { return _codeName; }
    //        set { _codeName = value; }
    //    }

    //    public string CodeState
    //    {
    //        get { return _codeState; }
    //        set { _codeState = value; }
    //    }

    //    public string ComputerName
    //    {
    //        get { return _computerName; }
    //        set { _computerName = value; }
    //    }
    //    public string AreaNo
    //    {
    //        get { return _areaNo; }
    //        set { _areaNo = value; }
    //    }

    //    public string LineId
    //    {
    //        get { return _lineId; }
    //        set { _lineId = value; }
    //    }

    //    public string EqptId
    //    {
    //        get { return _eqptId; }
    //        set { _eqptId = value; }
    //    }

    //    public string CodeTime
    //    {
    //        get { return _codeTime; }
    //        set { _codeTime = value; }
    //    }

    //    public string Ext1
    //    {
    //        get { return _ext1; }
    //        set { _ext1 = value; }
    //    }

    //    public string Ext2
    //    {
    //        get { return _ext2; }
    //        set { _ext2 = value; }
    //    }

    //    public string Ext3
    //    {
    //        get { return _ext3; }
    //        set { _ext3 = value; }
    //    }

    //    public string Ext4
    //    {
    //        get { return _ext4; }
    //        set { _ext4 = value; }
    //    }

    //    public string Ext5
    //    {
    //        get { return _ext5; }
    //        set { _ext5 = value; }
    //    }

    //    public string Ext6
    //    {
    //        get { return _ext6; }
    //        set { _ext6 = value; }
    //    }

    //    public string Ext7
    //    {
    //        get { return _ext7; }
    //        set { _ext7 = value; }
    //    }

    //    public string Ext8
    //    {
    //        get { return _ext8; }
    //        set { _ext8 = value; }
    //    }
    //    public string Ext9
    //    {
    //        get { return _ext9; }
    //        set { _ext9 = value; }
    //    }
    //    public string Ext10
    //    {
    //        get { return _ext10; }
    //        set { _ext10 = value; }
    //    }
    //    public string Ext11
    //    {
    //        get { return _ext11; }
    //        set { _ext11 = value; }
    //    }
    //    public string Ext12
    //    {
    //        get { return _ext12; }
    //        set { _ext12 = value; }
    //    }
    //    public string Ext13
    //    {
    //        get { return _ext13; }
    //        set { _ext13 = value; }
    //    }
    //    public string Ext14
    //    {
    //        get { return _ext14; }
    //        set { _ext14 = value; }
    //    }      
    //    public string PrinterType
    //    {
    //        get { return _printerType; }
    //        set { _printerType = value; }
    //    }
    //    public string ErrorMsg
    //    {
    //        get { return _errorMsg; }
    //        set { _errorMsg = value; }
    //    }
    //    #endregion

    //    /// <summary>
    //    /// 保存测试分选客户端计算机的配置数据。
    //    /// </summary>
    //    public void SaveCategoryTestConfigData()
    //    {
    //        try
    //        {
    //            DataSet dsReturn = new DataSet();
    //            Hashtable hsTable = new Hashtable();
    //            DataTable dataTable = new DataTable();
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_KEY, this._codeKey);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_ID, this._codeId);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_NAME, this._codeName);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_STATE, this._codeState);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_COMPUTER_NAME, this._computerName);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EQPT_ID, this._eqptId);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_AREA_NO, this._areaNo);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_LINE_ID, this._lineId);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_TIME, this._codeTime);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_1, this._ext1);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_2, this._ext2);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_3, this._ext3);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_4, this._ext4);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_5, this._ext5);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_6, this._ext6);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_7, this._ext7);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_8, this._ext8);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_9, this._ext9);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_10, _ext10);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_11, _ext11);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_12, _ext12);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_13, _ext13);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_14, _ext14);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_EXT_15, _printerType);
    //            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hsTable);
    //            //dataTable.TableName = BATTERY_ABNORMITY_MEMORY_FIELDS.DATABASE_TABLE_NAME;

    //            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
    //            if (null != serverFactory)
    //            {
    //                dsReturn = serverFactory.CreateIQMSEngine().SaveCategoryTestConfig(dataTable);
    //                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _errorMsg = ex.Message;
    //        }
    //        finally
    //        {

    //            CallRemotingService.UnregisterChannel();
    //        }
    //    }
    //    /// <summary>
    //    /// 根据计算机名称和CODEID查询客户端配置信息(<see cref="QMS.BASE_CODE"/>)。
    //    /// <see cref="ComputerName"/>和<see cref="CodeId"/>
    //    /// </summary>
    //    /// <returns>包含客户端配置信息的数据集。</returns>
    //    public DataSet GetConfigInformationData()
    //    {
    //        DataSet dsReturn = new DataSet();
    //        try
    //        {
    //            Hashtable hsTable = new Hashtable();
    //            DataTable dataTable = new DataTable();
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_COMPUTER_NAME, _computerName);
    //            hsTable.Add(BASE_CODE_FIELDS.FIELDS_CODE_ID,_codeId);
    //            dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hsTable);
    //            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
    //            if (null != serverFactory)
    //            {
    //                DataSet ds = new DataSet();
    //                dsReturn = serverFactory.CreateIQMSEngine().GetConfigInformation(dataTable);
    //                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _errorMsg = ex.Message;
    //        }
    //        finally
    //        {
    //            CallRemotingService.UnregisterChannel();
    //            if (_errorMsg != "")
    //            {
    //                //throw new Exception("获取配置信息失败！");
    //                throw new Exception("${res:FanHai.Hemera.Addins.WIP.LotCategoryTest.GetConfigurationFailed}");
    //            }
    //        }
    //        return dsReturn;
    //    }
    #endregion 注释结束

    //}
    /// <summary>
    /// 表示客户端计算机信息的实体类
    /// </summary>
    public class ComputerEntity : EntityObject
    {
        private string _codeKey = "";                           //
        private string _computerName = "";                      //
        private string _computerDesc = "";                      // Q.002
        private string _errorMsg = "";                            //
        private string _printerName = "";                       //
        private string _printerIP = "";                         //
        private string _printerPort = "";                       //
        private string _printerType = "";                       //
        private string _barcodeModule = "";                     //
        private UserDefinedAttrs _UDAs = new UserDefinedAttrs(COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY);

        public string CodeKey
        {
            get { return _codeKey; }
            set { _codeKey = value; }
        }
        public string ComputerName
        {
            get { return _computerName; }
            set
            {
                ValidateDirtyList(COMPUTER_FIELDS.FIELDS_COMPUTER_NAME, value);
                _computerName = value;
            }
        }
        public string ComputerDesc //Q.002
        {
            get { return _computerDesc; }
            set
            {
                ValidateDirtyList(COMPUTER_FIELDS.FIELDS_DESCRIPTION, value);
                _computerDesc = value;
            }
        }
        /// <summary>
        /// 最后一次执行方法发生的错误消息
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName
        {
            get { return _printerName; }
            set { _printerName = value; }
        }
        /// <summary>
        /// 打印机IP地址
        /// </summary>
        public string PrinterIP
        {
            get { return _printerIP; }
            set { _printerIP = value; }
        }
        /// <summary>
        /// 打印机端口
        /// </summary>
        public string PrinterPort
        {
            get { return _printerPort; }
            set { _printerPort = value; }
        }
        /// <summary>
        /// 打印机类型
        /// </summary>
        public string PrinterType
        {
            get { return _printerType; }
            set { _printerType = value; }
        }
        /// <summary>
        /// 标签模板
        /// </summary>
        public string BarcodeModule
        {
            get { return _barcodeModule; }
            set { _barcodeModule = value; }
        }

        public UserDefinedAttrs UserDefinedAttrs
        {
            get
            {
                return _UDAs;
            }
            set
            {
                _UDAs = value;
            }
        }
        public ComputerEntity()
        {
            _codeKey = CommonUtils.GenerateNewKey(0);
        }

        public ComputerEntity(string computerName)
        {
            if (computerName.Length > 0)
            {
                GetComputerConfByName(computerName);
                IsInitializeFinished = true;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return (DirtyList.Count > 0 || _UDAs.IsDirty);
            }
        }

        public override bool Insert()
        {
            bool bResult = false;
            DataSet dataSet = new DataSet();

            DataTable dtComputerConf = CreateDataTableForInsert();
            Dictionary<string, string> dataRow = new Dictionary<string, string>()
                                                {
                                                   {COMPUTER_FIELDS.FIELDS_CODE_KEY,_codeKey},
                                                   {COMPUTER_FIELDS.FIELDS_COMPUTER_NAME,_computerName.ToUpper()},
                                                   {COMPUTER_FIELDS.FIELDS_EDITOR,PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                   {COMPUTER_FIELDS.FIELDS_DESCRIPTION,_computerDesc.ToUpper()}
                                                };
            FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dtComputerConf, dataRow);
            if (dtComputerConf.Rows.Count > 0)
            {
                dataSet.Tables.Add(dtComputerConf);
            }

            // WorkOrder UDAs
            if (_UDAs.UserDefinedAttrList.Count > 0)
            {
                DataTable dtUDAs = DataTableHelper.CreateDataTableForUDA(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME, COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY);
                _UDAs.ParseInsertDataToDataTable(ref dtUDAs);

                if (dtUDAs.Rows.Count > 0)
                {
                    dataSet.Tables.Add(dtUDAs);
                }
            }
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    if (dataSet.Tables.Count > 0)
                    {
                        DataSet retDS = factor.CreateIComputerEngine().AddComputer(dataSet);
                        string strMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                        if (strMsg.Length < 1)
                        {
                            foreach (UserDefinedAttr uda in _UDAs.UserDefinedAttrList)
                            {
                                uda.OperationAction = OperationAction.Update;
                            }
                            this.ResetDirtyList();
                            bResult = true;
                        }
                        else
                        {
                            MessageService.ShowError(strMsg);
                        }
                    }
                    else
                    {
                        MessageService.ShowWarning("No dataTable in input parameter");
                    }
                }
            }
            catch (Exception e)
            {
                MessageService.ShowError(e.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }

        public override bool Update()
        {
            if (ComputerUpdate())
                return true;
            else
                return false;
        }
        public override bool Delete()
        {
            bool bResult = false;
            try
            {
                IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                if (null != factor)
                {
                    DataSet retDS = factor.CreateIComputerEngine().DeleteComputer(_codeKey);
                    string returnMessage = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(retDS);
                    if (returnMessage.Length < 1)
                    {
                        bResult = true;
                    }
                    else
                    {
                        MessageService.ShowError(returnMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        public override bool UpdateStatus()
        {
            //if (WorkOrderUpdate())
            //    return true;
            //else
            return false;
        }
        private bool ComputerUpdate()
        {
            bool bReturn = false;
            DataSet dataSet = new DataSet();
            if (IsDirty)
            {
                if (DirtyList.Count > 0)//
                {

                    DataTable entityTable = DataTableHelper.CreateDataTableForUpdateBasicData(COMPUTER_FIELDS.DATABASE_TABLE_NAME);

                    foreach (string Key in this.DirtyList.Keys)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                    {
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_KEY, _codeKey},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NAME, Key},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_OLD_VALUE, this.DirtyList[Key].FieldOriginalValue},
                                                        {COMMON_FIELDS.FIELD_COMMON_UPDATE_NEW_VALUE, this.DirtyList[Key].FieldNewValue}
                                                     };
                        FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref entityTable, rowData);
                    }
                    if (entityTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(entityTable);
                    }
                }
                if (_UDAs.IsDirty)
                {
                    DataTable dtUDAs = DataTableHelper.CreateDataTableForUDA(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME, COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY);
                    _UDAs.ParseUpdateDataToDataTable(ref dtUDAs);
                    dataSet.Tables.Add(dtUDAs);
                }
                try
                {
                    DataSet dsReturn = null;
                    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                    dsReturn = serverFactory.CreateIComputerEngine().UpdateComputer(dataSet);
                    string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (returnMsg.Length < 1)
                    {
                        foreach (UserDefinedAttr uda in _UDAs.UserDefinedAttrList)
                        {
                            uda.OperationAction = OperationAction.Update;
                        }
                        this.ResetDirtyList();
                        bReturn = true;
                    }
                    else
                    {
                        MessageService.ShowError(returnMsg);
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowError(ex.Message);
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }
            else
            {
                MessageService.ShowMessage
                 ("${res:Global.UpdateItemDataMessage}", "${res:Global.SystemInfo}");
            }
            return bReturn;
        }

        ///// <summary>
        ///// Get Config Information Data
        ///// </summary>
        ///// <param name="strComputerName"></param>
        ///// <returns></returns>
        //public DataSet GetConfigInformationData()
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {
        //        Hashtable hsTable = new Hashtable();
        //        DataTable dataTable = new DataTable();
        //        hsTable.Add(COMPUTER_FIELDS.FIELDS_COMPUTER_NAME, _computerName);
        //        hsTable.Add(COMPUTER_FIELDS.FIELDS_CODE_KEY, _codeKey);
        //        dataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hsTable);
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)
        //        {
        //            DataSet ds = new DataSet();
        //            dsReturn = serverFactory.CreateIQMSEngine().GetConfigInformation(dataTable);
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
        //        if (_errorMsg != "")
        //        {
        //            //throw new Exception("获取配置信息失败！");
        //            throw new Exception("${res:FanHai.Hemera.Addins.WIP.LotCategoryTest.GetConfigurationFailed}");
        //        }
        //    }
        //    return dsReturn;
        //}
        /// <summary>
        /// 获取客户端计算机连接的打印机信息。
        /// </summary>
        /// <param name="computerName">客户端计算机名称。</param>
        /// <returns>true：获取打印机信息成功。false：获取打印机信息失败。</returns>
        public bool GetComputerPrinterInfo(string computerName)
        {
            bool bResult = false;
            try
            {
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIComputerEngine().GetComputerByName(computerName);
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (_errorMsg == string.Empty)
                {
                    if (dsReturn == null || dsReturn.Tables.Count > 0 && dsReturn.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        if (dsReturn.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                        {
                            DataTable table = dsReturn.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                            foreach (DataRow dataRow in table.Rows)
                            {
                                switch (Convert.ToString(dataRow[COMMON_FIELDS.FIELD_ATTRIBUTE_NAME]))
                                {
                                    case "PRINTER_NAME":
                                        _printerName = Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                                        break;
                                    case "PRINTER_IP":
                                        _printerIP = Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                                        break;
                                    case "PRINTER_PORT":
                                        _printerPort = Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                                        break;
                                    case "PRINTER_TYPE":
                                        _printerType = Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                                        break;
                                    case "PRINTER_LABEL":
                                        _barcodeModule = Convert.ToString(dataRow[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE]);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            bResult = true;
                        }
                    }
                }
                else
                {
                    MessageService.ShowError(_errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return bResult;
        }
        /// <summary>
        /// 获取指定机器的自定义属性信息。
        /// </summary>
        /// <param name="computerName">机器名称。</param>
        /// <returns>包含自定义属性信息的数据集。</returns>
        public DataSet GetComputerUda(string computerName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIComputerEngine().GetComputerByName(computerName);
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
        /// 获取本机的的自定义属性值。
        /// </summary>
        /// <returns>包含计算机自定义属性值的数据表。</returns>
        public DataTable GetComputerInfo()
        {
            DataTable tableComputerUda = new DataTable();
            try
            {
                DataSet dsComputerUda = GetComputerUda(PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME));
                _errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsComputerUda);
                if (_errorMsg.Length > 0)
                {
                    MessageService.ShowError(_errorMsg);
                }
                else
                {
                    if (dsComputerUda.Tables.Count > 0 && dsComputerUda.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                    {
                        tableComputerUda = dsComputerUda.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            return tableComputerUda;
        }
        /// <summary>
        /// 通过属性名获取属性值。
        /// </summary>
        /// <param name="tableComputerUda">包含计算机自定义属性值的数据表。</param>
        /// <param name="attributeName">属性名称。</param>
        /// <returns>属性值。</returns>
        public string GetComputerUDAAttributeValue(DataTable tableComputerUda, string attributeName)
        {
            string strAttributeValue = "";
            try
            {
                if (tableComputerUda != null)
                {
                    DataRow[] row = tableComputerUda.Select("ATTRIBUTE_NAME='" + attributeName + "'");
                    if (row.Length > 0)
                    {
                        strAttributeValue = row[0][COMPUTER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE].ToString();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);

            }
            return strAttributeValue;
        }
        /// <summary>
        /// 获取EDC值是否允许手工输入。默认值为true。
        /// </summary>
        /// <returns>true：允许手工输入。false：不允许手工输入。</returns>
        public bool GetIsEDCAllowManualInputValue()
        {
            DataTable dtComputerUDA = GetComputerInfo();
            string result = GetComputerUDAAttributeValue(dtComputerUDA, "IsEDCAllowManualInput");
            bool bIsEDCAllowManualInput = true;
            if (bool.TryParse(result, out bIsEDCAllowManualInput))
            {
                return bIsEDCAllowManualInput;
            }
            return true;
        }


        public void GetComputerConfByName(string computerName)
        {
            try
            {
                DataSet dsReturn = null;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.CreateIComputerEngine().GetComputerByName(computerName);
                string returnMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                if (returnMsg != string.Empty)
                {
                    MessageService.ShowError(returnMsg);
                }
                else
                {
                    SetComputerDataToProperty(dsReturn);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        private void SetComputerDataToProperty(DataSet dataSet)
        {
            try
            {
                if (dataSet.Tables.Contains(COMPUTER_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable objTable = dataSet.Tables[COMPUTER_FIELDS.DATABASE_TABLE_NAME];
                    if (objTable.Rows.Count > 0)
                    {
                        CodeKey = objTable.Rows[0][COMPUTER_FIELDS.FIELDS_CODE_KEY].ToString();
                        ComputerName = objTable.Rows[0][COMPUTER_FIELDS.FIELDS_COMPUTER_NAME].ToString();
                        ComputerDesc = objTable.Rows[0][COMPUTER_FIELDS.FIELDS_DESCRIPTION].ToString();
                    }
                }
                if (dataSet.Tables.Contains(COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                {
                    DataTable objUdaTable = dataSet.Tables[COMPUTER_ATTR_FIELDS.DATABASE_TABLE_NAME];
                    if (objUdaTable.Rows.Count > 0)
                    {
                        SetObjUda(objUdaTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }

        private void SetObjUda(DataTable objUdaTable)
        {
            foreach (DataRow dataRow in objUdaTable.Rows)
            {
                string linkedObjKey = dataRow[objUdaTable.Columns[COMPUTER_ATTR_FIELDS.FIELDS_COMPUTER_KEY]].ToString();
                string attributeKey = dataRow[objUdaTable.Columns[COMPUTER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY]].ToString();
                string attributeName = dataRow[objUdaTable.Columns[COMPUTER_ATTR_FIELDS.FIELDS_ATTRIBUTE_NAME]].ToString();
                string attributeValue = dataRow[objUdaTable.Columns[COMPUTER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]].ToString();
                UserDefinedAttr uda = new UserDefinedAttr(linkedObjKey, attributeKey, attributeName, attributeValue, "");
                uda.DataType = dataRow[objUdaTable.Columns["DATA_TYPE"]].ToString();
                uda.OperationAction = OperationAction.Update;
                _UDAs.UserDefinedAttrAdd(uda);
            }
        }

        private DataTable CreateDataTableForInsert()
        {
            List<string> fields = new List<string>() 
                                                    { 
                                                        COMPUTER_FIELDS.FIELDS_CODE_KEY,
                                                        COMPUTER_FIELDS.FIELDS_COMPUTER_NAME,                                                       
                                                        COMPUTER_FIELDS.FIELDS_EDITOR,
                                                        COMPUTER_FIELDS.FIELDS_DESCRIPTION
                                                        //COMPUTER_FIELDS.FIELDS_EDIT_TIME,
                                                        //COMPUTER_FIELDS.FIELDS_EDIT_TIMEZONE
                                                    };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(COMPUTER_FIELDS.DATABASE_TABLE_NAME, fields);
        }
    }
}

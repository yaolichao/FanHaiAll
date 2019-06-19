using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

using DevExpress.XtraEditors.Controls; 
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Utils.UDA
{
    public partial class UdaCommonControl : UserControl
    {
        #region constructor
        public UdaCommonControl(EntityType entityType)
        {
            InitializeComponent();
            SetLanguageInfoToControl();

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            InitEmptyUserDefinedAttrDataSet();
            _entityType = entityType;         //get entity type

        }
        public UdaCommonControl(EntityType entityType, string linkedItemKey)
        {
            InitializeComponent();
            SetLanguageInfoToControl();

            _entityType     = entityType;         //get entity type
            _linkedItemKey  = linkedItemKey;      //get linked item key(parent key)

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            InitEmptyUserDefinedAttrDataSet();
        }
        //控件命名 modi by chao.pang
        private void SetLanguageInfoToControl()
        {
            this.btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.btnAdd}");                                 //添加属性 modi by chao.pang
            this.btnDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.btnDelete}");                           //删除属性 modi by chao.pang

            this.clnAttributeKey.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeId}");      //属性ID modi by chao.pang
            this.clnAttributeName.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeName}");   //属性名 modi by chao.pang
            this.clnAttributeValue.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeValue}"); //属性值 modi by chao.pang
            this.clnAttributeDataType.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnDataType}");    //数据类型 modi by chao.pang
            this.clnEditTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnEditTime}");             //最后修改时间 modi by chao.pang
        }
        #endregion

        #region State Changes
        public ControlState CtrlState
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {
                case ControlState.Empty:
                    break;
                case ControlState.ReadOnly:
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    gridUDAsView.OptionsBehavior.Editable = false;
                    break;
                case ControlState.Read:
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    gridUDAsView.OptionsBehavior.Editable = false;
                    break;
                case ControlState.Edit:
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    gridUDAsView.OptionsBehavior.Editable = true;
                    break;
                case ControlState.New:
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    gridUDAsView.OptionsBehavior.Editable = true;
                    InitEmptyUserDefinedAttrDataSet();
                    break;
            }
        }
        #endregion State Changes

        /// <summary>
        /// add attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            UdaColumnsSelect udaColumnSelect = new UdaColumnsSelect(_entityType, SelectedBaseUserAttrs);
            //判断添加自定义属性界面返回的值为DialogResult.OK 执行以下操作 
            if (DialogResult.OK == udaColumnSelect.ShowDialog())
            {//获取返回值为OK
                BaseUserDefinedAttr uda = udaColumnSelect.CurrentSelectedUserDefinedAttr;
                if (null != uda && _selectedUDAs.UserDefinedAttrAdd(new UserDefinedAttr(_linkedItemKey, uda)))
                {
                    gridUDAsView.AddNewRow();
                    DataRow dataRow = gridUDAsView.GetDataRow(gridUDAsView.FocusedRowHandle);
                    dataRow[COLUMN_ATTRIBUTE_KEY] = uda.Key;
                    dataRow[COLUMN_ATTRIBUTE_NAME] = uda.Name;
                    dataRow[COLUMN_ATTRIBUTE_DATA_TYPE] = uda.DataType;
                    dataRow.EndEdit();
                    gridUDAsView.UpdateCurrentRow();
                    gridUDAsView.ShowEditor();
                }
            }
        }
        /// <summary>
        /// delete attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get row handle
            int rowHandle = gridUDAsView.GetDataRowHandleByGroupRowHandle(gridUDAsView.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                string attributeKey = (string)gridUDAsView.GetRowCellValue(rowHandle, COLUMN_ATTRIBUTE_KEY);
                if (null != attributeKey && attributeKey.Length > 0)
                {
                    //调用UserDefinedAttrDelete方法删除相应的数据返回值为true执行下面操作 
                    if (_selectedUDAs.UserDefinedAttrDelete(attributeKey))
                    {
                        gridUDAsView.DeleteRow(rowHandle);
                        gridUDAsView.UpdateCurrentRow();
                        //MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgDeleteSuccessfully}");
                        return;
                    }
                    else
                    {
                        //提示删除失败 modi by chao.Pang
                        MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgDeleteFailure}");
                    }
                }
            }
            else
            {
                //提示未选中行 modi by chao.pang
                MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgUnselectRow}");
            }
        }
        /// <summary>
        /// gvUdaList_CustomRowCellEdit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvUdaList_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string attributeDataType = gridUDAsView.GetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_DATA_TYPE).ToString();
                string attributeName = gridUDAsView.GetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_NAME).ToString();
                if (attributeDataType == string.Empty)
                    return;
                AttributeDataType dateType = (AttributeDataType)Convert.ToInt32(attributeDataType);
                switch (dateType)
                {
                    case AttributeDataType.DATE:
                        RepositoryItemDateEdit dateEdit = new RepositoryItemDateEdit();
                        dateEdit.DisplayFormat.FormatString = "yyyy-MM-dd";
                        dateEdit.EditMask = "yyyy-MM-dd";
                        e.RepositoryItem = dateEdit;
                        break;
                    case AttributeDataType.DATETIME:
                        RepositoryItemDateEdit dateEditTime = new RepositoryItemDateEdit();
                        dateEditTime.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
                        dateEditTime.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

                        dateEditTime.VistaTimeProperties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        dateEditTime.VistaTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        dateEditTime.VistaTimeProperties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        dateEditTime.VistaTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        e.RepositoryItem = dateEditTime;
                        break;
                    case AttributeDataType.BOOLEAN:
                        RepositoryItemComboBox cmbBoolean = new RepositoryItemComboBox();
                        cmbBoolean.Items.Add("true");
                        cmbBoolean.Items.Add("false");
                        cmbBoolean.TextEditStyle = TextEditStyles.DisableTextEditor;
                        e.RepositoryItem = cmbBoolean;
                        break;

                    case AttributeDataType.LINKED: 
                            if (attributeName == "OnLineStore")
                            {
                                RepositoryItemCheckedComboBoxEdit cmbLinked = new RepositoryItemCheckedComboBoxEdit();
                                DataTable storeTable = _crmAttr.GetStoreName();
                                storeTable.DefaultView.RowFilter = "STORE_TYPE=0";//只保留返工类型的线上仓。
                                foreach (DataRowView dataRow in storeTable.DefaultView)
                                {
                                    cmbLinked.Items.Add(dataRow[WST_STORE_FIELDS.FIELD_STORE_NAME]);  
                                }

                                cmbLinked.TextEditStyle = TextEditStyles.DisableTextEditor;
                                e.RepositoryItem = cmbLinked;
                            }
                            else if (attributeName == COMMON_NAMES.LINKED_ITEM_EDC)
                            {
                                RepositoryItemComboBox comboBox = new RepositoryItemComboBox();
                                DataTable edcTable = _crmAttr.GetEDC();
                                foreach (DataRow dataRow in edcTable.Rows)
                                {
                                    comboBox.Items.Add(dataRow[EDC_MAIN_FIELDS.FIELD_EDC_NAME]);
                                }

                                comboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
                                e.RepositoryItem = comboBox;
                            }
                            else if (attributeName == "FutureHold" || attributeName=="DependSampStep")
                            {
                                try
                                {
                                    RepositoryItemComboBox cbFutureHold = new RepositoryItemComboBox();
                                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                                    DataSet dsReturn = factor.CreateIOperationEngine().GetMaxVerOperation(null);
                                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                                    if (msg == string.Empty)
                                    {
                                        if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME))
                                        {
                                            DataTable operationTable = dsReturn.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
                                            foreach (DataRow dataRow in operationTable.Rows)
                                            {
                                                cbFutureHold.Items.Add(dataRow[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME].ToString());
                                            }

                                            cbFutureHold.TextEditStyle = TextEditStyles.DisableTextEditor;
                                            e.RepositoryItem = cbFutureHold;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(msg);
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
                            else if (attributeName == "ReturnCode")
                            {
                                try
                                {
                                    RepositoryItemComboBox cbResonCode = new RepositoryItemComboBox();
                                    IServerObjFactory factor = CallRemotingService.GetRemoteObject();
                                    DataSet _dataDsFrom=new DataSet ();
                                    Hashtable _hsTable=new Hashtable ();
                                    _hsTable.Add("REASON_CODE_CATEGORY_TYPE", "TK");
                                    DataTable _dtTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(_hsTable);
                                    _dtTable.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                                    _dataDsFrom.Tables.Add(_dtTable);
                                    //get return dataset
                                    DataSet dsReturn =factor.CreateIReasonCodeEngine().GetReasonCodeByNameAndType(_dataDsFrom);
                                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                                    if (msg == string.Empty)
                                    {
                                        if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables.Contains(FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME))
                                        {
                                            DataTable operationTable = dsReturn.Tables[FMM_REASON_CODE_CATEGORY_FIELDS.DATABASE_TABLE_NAME];
                                            foreach (DataRow dataRow in operationTable.Rows)
                                            {
                                                cbResonCode.Items.Add(dataRow[FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME].ToString());
                                            }

                                            cbResonCode.TextEditStyle = TextEditStyles.DisableTextEditor;
                                            e.RepositoryItem = cbResonCode;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(msg);
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
                            break;

                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// gvUdaList_CellValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvUdaList_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                string attributeValue = gridUDAsView.GetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_VALUE).ToString();
                if (attributeValue.Length > 0)
                {
                    string attributeKey = gridUDAsView.GetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_KEY).ToString();
                    string attributeDataType = gridUDAsView.GetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_DATA_TYPE).ToString();
                    if (FanHai.Hemera.Utils.Common.Utils.ValidateDataByType(attributeValue, attributeDataType))
                    {
                        _selectedUDAs.UserDefinedAttrUpdate(attributeKey, attributeValue);
                        _dataTypeCheckResult = true;
                    }
                    else
                    {
                        string attributeOldValue = _selectedUDAs.GetUserDefinedAttrValueByAttrKey(attributeKey);
                        gridUDAsView.SetRowCellValue(e.RowHandle, COLUMN_ATTRIBUTE_VALUE, attributeOldValue);
                        _dataTypeCheckResult = false;
                        MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgDataTypeIsNotRight}");
                    }
                }
            }
        }
        public string LinkedToItemKey
        {
            get
            {
                return _linkedItemKey;
            }
            set
            {
                _linkedItemKey = value;
                foreach (UserDefinedAttr uda in _selectedUDAs.UserDefinedAttrList)
                {
                    uda.LinkToItemKey = _linkedItemKey;
                }
            }
        }

        public bool DataTypeCheckResult
        {
            get
            {
                return _dataTypeCheckResult;
            }
        }

        public UserDefinedAttrs UserDefinedAttrs
        {
            get
            {
                if (this.gridUDAsView.IsEditing && this.gridUDAsView.IsEditorFocused && this.gridUDAsView.EditingValueModified)
                {
                    this.gridUDAsView.SetFocusedRowCellValue(this.gridUDAsView.FocusedColumn, this.gridUDAsView.EditingValue);
                }
                this.gridUDAsView.UpdateCurrentRow();
                return _selectedUDAs;
            }
            set
            {
                _selectedUDAs = value;
                MapUserDefinedAttrsToControls();
            }
        }
        private void MapUserDefinedAttrsToControls()
        {
            gridUDAs.DataSource = null;
            InitEmptyUserDefinedAttrDataSet();
            if (null != _selectedUDAs)
            {
                foreach (UserDefinedAttr uda in _selectedUDAs.UserDefinedAttrList)
                {
                    gridUDAsView.AddNewRow();
                    DataRow dataRow = gridUDAsView.GetDataRow(gridUDAsView.FocusedRowHandle);
                    if (null == dataRow)
                    {
                        MessageService.ShowError("UdaCommonControl::MapUserDefinedAttrsToControls::AddNewRow");
                        return;
                    }
                    dataRow[COLUMN_ATTRIBUTE_KEY] = uda.Key;
                    dataRow[COLUMN_ATTRIBUTE_NAME] = uda.Name;
                    dataRow[COLUMN_ATTRIBUTE_VALUE] = uda.Value;
                    dataRow[COLUMN_ATTRIBUTE_DATA_TYPE] = uda.DataType;
                    dataRow.EndEdit();
                    gridUDAsView.UpdateCurrentRow();
                }
                gridUDAsView.ShowEditor();
            }
        }

        private List<string> SelectedBaseUserAttrs
        {
            get
            {
                if (null == _selectedUDAs) return null;
                List<string> selectedBaseUDAs = new List<string>();
                foreach (UserDefinedAttr uda in _selectedUDAs.UserDefinedAttrList)
                {
                    if (OperationAction.None == uda.OperationAction ||
                           OperationAction.Delete == uda.OperationAction)
                    {
                        continue;
                    }
                    if (!selectedBaseUDAs.Contains(uda.Key))
                    {
                        selectedBaseUDAs.Add(uda.Key);
                    }
                }
                return selectedBaseUDAs;
            }
        }
        private void InitEmptyUserDefinedAttrDataSet()
        {
            List<string> fields = new List<string>()
                                                    {
                                                        COLUMN_ATTRIBUTE_KEY,
                                                        COLUMN_ATTRIBUTE_NAME,
                                                        COLUMN_ATTRIBUTE_VALUE,
                                                        COLUMN_LAST_UPDATE_TIME,
                                                        COLUMN_ATTRIBUTE_DATA_TYPE
                                                    };

            DataTable dt = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("DONTUSEIT", fields);
            gridUDAs.MainView = gridUDAsView;
            gridUDAs.DataSource = dt;
        }
        private string _linkedItemKey = "";
        private EntityType _entityType = EntityType.None;
        private UserDefinedAttrs _selectedUDAs = new UserDefinedAttrs();
        private ControlState _controlState = ControlState.Empty;
        private delegate void AfterStateChanged(ControlState controlState);
        private AfterStateChanged afterStateChanged = null;
        private CrmAttribute _crmAttr = new CrmAttribute();

        private const string COLUMN_ATTRIBUTE_KEY       = "clnAttributeKey";
        private const string COLUMN_ATTRIBUTE_NAME      = "clnAttributeName";
        private const string COLUMN_ATTRIBUTE_VALUE     = "clnAttributeValue";
        private const string COLUMN_LAST_UPDATE_TIME    = "clnEditTime";
        private const string COLUMN_ATTRIBUTE_DATA_TYPE = "clnAttributeDataType";

        private bool _dataTypeCheckResult = true;

        private void UdaCommonControl_Load(object sender, EventArgs e)
        {

        }
    }
}

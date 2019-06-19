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
    public partial class UdaCommonControlEx : UserControl
    {
        #region constructor
        public UdaCommonControlEx(EntityType entityType)
        {
            InitializeComponent();
            SetLanguageInfoToControl();

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            InitEmptyUserDefinedAttrDataSet();
            _entityType = entityType;         //get entity type

        }
        public UdaCommonControlEx(EntityType entityType, string linkedItemKey)
        {
            InitializeComponent();
            SetLanguageInfoToControl();

            _entityType = entityType;         //get entity type
            _linkedItemKey = linkedItemKey;      //get linked item key(parent key)

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            InitEmptyUserDefinedAttrDataSet();
        }

        public UdaCommonControlEx(EntityType entityType, string linkedItemKey,string linkedTable)
        {
            InitializeComponent();
            SetLanguageInfoToControl();

            _entityType = entityType;         //get entity type
            _linkedItemKey = linkedItemKey;      //get linked item key(parent key)
            _linkedTable = linkedTable;

            afterStateChanged += new AfterStateChanged(OnAfterStateChanged);

            InitEmptyUserDefinedAttrDataSet();
        }
        //控件命名 modi by chao.pang
        private void SetLanguageInfoToControl()
        {
            this.btnAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.btnAdd}");                                       //添加属性  modi by chao.pang
            this.btnDelete.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.btnDelete}");                                 //删除属性 modi by chao.pang

            this.clnAttributeKey.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeId}");            //属性ID modi by chao.pang
            this.clnAttributeName.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeName}");         //属性名 modi by chao.pang
            this.clnAttributeValue.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeValue}");       //属性值 modi by chao.pang
            this.clnAttributeDataType.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnDataType}");          //数据类型 modi by chao.pang
            this.clnEditTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnEditTime}");                   //最后修改时间 modi by chao.pang
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

        #region Actions
        #region add attribute
        /// <summary>
        /// add attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            UdaColumnsSelect udaColumnSelect = new UdaColumnsSelect(_entityType, SelectedBaseUserAttrs);
            if (DialogResult.OK == udaColumnSelect.ShowDialog())
            {
                BaseUserDefinedAttr uda = udaColumnSelect.CurrentSelectedUserDefinedAttr;
                if (null != uda && _selectedUDAs.UserDefinedAttrAdd(new UserDefinedAttrEx(_linkedItemKey, uda)))
                {
                    gridUDAsView.AddNewRow();
                    DataRow dataRow = gridUDAsView.GetDataRow(gridUDAsView.FocusedRowHandle);
                    dataRow[COLUMN_ATTRIBUTE_KEY] = uda.Key;
                    dataRow[COLUMN_OBJECT_TYPE] = _linkedTable;
                    dataRow[COLUMN_ATTRIBUTE_NAME] = uda.Name;
                    dataRow[COLUMN_ATTRIBUTE_DATA_TYPE] = uda.DataType;
                    dataRow.EndEdit();
                    gridUDAsView.UpdateCurrentRow();
                    gridUDAsView.ShowEditor();
                }
            }
        }
        #endregion

        #region delete attribute
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
                    if (_selectedUDAs.UserDefinedAttrDelete(attributeKey))
                    {
                        gridUDAsView.DeleteRow(rowHandle);
                        gridUDAsView.UpdateCurrentRow();
                        //MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgDeleteSuccessfully}");
                        return;
                    }
                    else
                    {
                        MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgDeleteFailure}");
                    }
                }
            }
            else
            {
                MessageService.ShowMessage("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.MsgUnselectRow}");
            }
        }
        #endregion
        #endregion Actions

        #region Control Events
        #region gvUdaList_CustomRowCellEdit
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

                    default:
                        break;
                }
            }
        }

        #endregion

        #region gvUdaList_CellValueChanged
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
        #endregion
        #endregion Control Events

        #region Properties
        public string LinkedToItemKey
        {
            get
            {
                return _linkedItemKey;
            }
            set
            {
                _linkedItemKey = value;
                foreach (UserDefinedAttrEx uda in _selectedUDAs.UserDefinedAttrList)
                {
                    uda.LinkToItemKey = _linkedItemKey;
                }
            }
        }


        public string LinkedToTable
        {
            get
            {
                return _linkedTable;
            }
            set
            {
                _linkedTable = value;
                foreach (UserDefinedAttrEx uda in _selectedUDAs.UserDefinedAttrList)
                {
                    uda.LinkToTable = _linkedTable;
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

        public UserDefinedAttrsEx UserDefinedAttrs
        {
            get
            {
                return _selectedUDAs;
            }
            set
            {
                _selectedUDAs = value;
                MapUserDefinedAttrsToControls();
            }
        }
        #endregion Properties

        #region Private Functions
        private void MapUserDefinedAttrsToControls()
        {
            gridUDAs.DataSource = null;
            InitEmptyUserDefinedAttrDataSet();
            if (null != _selectedUDAs)
            {
                foreach (UserDefinedAttrEx uda in _selectedUDAs.UserDefinedAttrList)
                {
                    gridUDAsView.AddNewRow();
                    DataRow dataRow = gridUDAsView.GetDataRow(gridUDAsView.FocusedRowHandle);
                    if (null == dataRow)
                    {
                        MessageService.ShowError("UdaCommonControl::MapUserDefinedAttrsToControls::AddNewRow");
                        return;
                    }
                    dataRow[COLUMN_ATTRIBUTE_KEY] = uda.Key;
                    dataRow[COLUMN_OBJECT_TYPE] = uda.LinkToTable;
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
                foreach (UserDefinedAttrEx uda in _selectedUDAs.UserDefinedAttrList)
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
                                                    {   COLUMN_OBJECT_KEY,
                                                        COLUMN_OBJECT_TYPE,
                                                        COLUMN_ATTRIBUTE_KEY,
                                                        COLUMN_ATTRIBUTE_NAME,
                                                        COLUMN_ATTRIBUTE_VALUE,
                                                        COLUMN_EDITOR,
                                                        COLUMN_EDIT_TIME,
                                                        COLUMN_EDIT_TIMEZONE,
                                                        COLUMN_ATTRIBUTE_DATA_TYPE
                                                    };

            DataTable dt = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(BASE_ATTRIBUTE_VALUE_FIELDS.DATABASE_TABLE_NAME, fields);
            gridUDAs.MainView = gridUDAsView;
            gridUDAs.DataSource = dt;
        }
        #endregion Private Functions

        #region Private variable definition
        private string _linkedItemKey = "";
        private string _linkedTable = "";
        private EntityType _entityType = EntityType.None;
        private UserDefinedAttrsEx _selectedUDAs = new UserDefinedAttrsEx();
        private ControlState _controlState = ControlState.Empty;
        private delegate void AfterStateChanged(ControlState controlState);
        private AfterStateChanged afterStateChanged = null;
        private CrmAttribute _crmAttr = new CrmAttribute();


        private const string COLUMN_OBJECT_KEY = "clnOBJECT_KEY";
        private const string COLUMN_OBJECT_TYPE = "clnOBJECT_TYPE";
        private const string COLUMN_ATTRIBUTE_KEY = "clnAttributeKey";
        private const string COLUMN_ATTRIBUTE_NAME = "clnAttributeName";
        private const string COLUMN_ATTRIBUTE_VALUE = "clnAttributeValue";
        private const string COLUMN_EDITOR = "clnEditor";
        private const string COLUMN_EDIT_TIME = "clnEditTime";
        private const string COLUMN_EDIT_TIMEZONE = "clnEditTimeZone";
        private const string COLUMN_ATTRIBUTE_DATA_TYPE = "clnAttributeDataType";

        private bool _dataTypeCheckResult = true;   //true--pass check;false----not pass

        #endregion
    }
}

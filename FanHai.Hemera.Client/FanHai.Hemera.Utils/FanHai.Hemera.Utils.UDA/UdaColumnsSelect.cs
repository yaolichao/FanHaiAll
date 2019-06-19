using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FanHai.Hemera.Utils.UDA
{
    public partial class UdaColumnsSelect : BaseDialog
    {
        #region Construtor
        public UdaColumnsSelect(EntityType entityType, List<string> selectedUDAs)
            : base(StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaColumnsSelect.Title}"))
        {//添加自定义属性
            InitializeComponent();
            //定义控件名称
            SetLanguageInfoToControl();
            //创建空表
            InitEmptyUserDefinedAttrDataSet();
            //get entityType
            _entityType = entityType;

            _selectedUDAs = selectedUDAs;
            //check type and show data

            CheckTypeAndShowData();
        }

        private void SetLanguageInfoToControl()
        {
            this.btnOK.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaColumnsSelect.btnOK}");                //确定
            this.btnCancel.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaColumnsSelect.btnCancel}");        //取消

            this.clnAttributeName.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnAttributeName}");     //属性名
            this.clnAttributeDataType.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnDataType}");      //数据类型
            this.clnAttributeDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnDescription}");       //描述
            this.clnAttributeDataTypeStr.Caption = StringParser.Parse("${res:FanHai.Hemera.Utils.UDA.UdaCommonControl.gridColumnDataTypeStr}");//数据类型
        }
        #endregion

        #region Properties
        public BaseUserDefinedAttr CurrentSelectedUserDefinedAttr
        {
            get
            {
                return _currentSelectedUDA;
            }
        }
        #endregion
        /// <summary>
        /// 比对类型获取数据
        /// </summary>
        #region CheckTypeAndShowData
        private void CheckTypeAndShowData()
        {
            if (EntityType.None == _entityType)
            {
                return;
            }

            #region variable define
            DataSet dataDsBack = new DataSet(); //dataset to get data
            #endregion

            #region code detail
            try
            {
                //set value to crmAttributeEntity
                crmAttributeEntity.MyCategory = _entityType.ToString();
                //excute method of entity
                dataDsBack = crmAttributeEntity.GetAttributsColumnsForSomeCategory();
                //check result
                if (crmAttributeEntity.ErrorMsg != "")
                {
                    MessageService.ShowError(new Exception(crmAttributeEntity.ErrorMsg));
                }
                else
                {
                    _remainUDAs = new BaseUserDefinedAttrs(dataDsBack.Tables[0]);
                    _remainUDAs.UserDefinedAttrDelete(_selectedUDAs);
                    MapRemainUserDefinedAttrsToControls();
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                //UnregisterChannel
                CallRemotingService.UnregisterChannel();
            }
            #endregion



        }
        #endregion

        #region Set Data to XtraGrid Control
        private void MapRemainUserDefinedAttrsToControls()
        {
            foreach (BaseUserDefinedAttr uda in _remainUDAs.BaseUserDefinedAttrList)
            {
                gridAttributeSelectView.AddNewRow();
                DataRow dataRow = gridAttributeSelectView.GetDataRow(gridAttributeSelectView.FocusedRowHandle);
                if (null == dataRow)
                {
                    MessageService.ShowError(new Exception("UdaColumnsSelect::MapUserDefinedAttrsToControls"));
                    return;
                }
                dataRow[COLUMN_ATTRIBUTE_KEY] = uda.Key;
                dataRow[COLUMN_ATTRIBUTE_NAME] = uda.Name;
                dataRow[COLUMN_ATTRIBUTE_DATA_TYPE] = uda.DataType;
                dataRow[COLUMN_ATTRIBUTE_DESCRIPTION] = uda.Description;
                dataRow[COLUMN_ATTRIBUTE_DATA_TYPE_STR] = uda.DataTypeString;
                dataRow.EndEdit();
                gridAttributeSelectView.UpdateCurrentRow();
            }
            gridAttributeSelectView.ShowEditor();
        }
        #endregion
        /// <summary>
        /// 获取选中行数据 modi by chao.pang
        /// </summary>
        /// <returns></returns>
        private bool GetSelectDataRowToUDA()
        {
            int rowHandle = gridAttributeSelectView.GetDataRowHandleByGroupRowHandle(gridAttributeSelectView.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                string udaKey = gridAttributeSelectView.GetFocusedRowCellValue(COLUMN_ATTRIBUTE_KEY).ToString();
                string udaName = gridAttributeSelectView.GetFocusedRowCellValue(COLUMN_ATTRIBUTE_NAME).ToString();
                string udaDataType = gridAttributeSelectView.GetFocusedRowCellValue(COLUMN_ATTRIBUTE_DATA_TYPE).ToString();
                string udaDescription = gridAttributeSelectView.GetFocusedRowCellValue(COLUMN_ATTRIBUTE_DESCRIPTION).ToString();
                _currentSelectedUDA = new BaseUserDefinedAttr(udaKey, udaName, udaDataType, udaDescription);
                return true;
            }
            return false;
        }

        #region get double clicking row value
        /// <summary>
        /// get double clicking row value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridAttributeSelect_DoubleClick(object sender, EventArgs e)
        {
            if (GetSelectDataRowToUDA())
            {//获取选中数据返回值为true
                DialogResult = DialogResult.OK;             //返回结果OK
                Close();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        #endregion

        #region Actions
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _currentSelectedUDA = null;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (GetSelectDataRowToUDA())
            {//获取选中数据返回值为true
                DialogResult = DialogResult.OK;             //返回结果OK
                Close();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        #endregion

        #region Init XtraGrid Control's DataSource Settings
        private void InitEmptyUserDefinedAttrDataSet()
        {
            List<string> fields = new List<string>()
                                                    {
                                                        COLUMN_ATTRIBUTE_KEY,
                                                        COLUMN_ATTRIBUTE_NAME,
                                                        COLUMN_ATTRIBUTE_DATA_TYPE,
                                                        COLUMN_ATTRIBUTE_DATA_TYPE_STR,
                                                        COLUMN_ATTRIBUTE_DESCRIPTION
                                                    };

            DataTable dt = FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns("DONTUSEIT", fields);
            gridAttributeSelect.MainView = gridAttributeSelectView;
            gridAttributeSelect.DataSource = dt;
        }
        #endregion

        #region Client DragAndDrop
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Private const definition
        // Drag client area
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x01;
        private const int HTCAPTION = 0x02;
        #endregion

        #region Private variables definition
        private EntityType _entityType = EntityType.None;
        private BaseUserDefinedAttr _currentSelectedUDA = null;
        private BaseUserDefinedAttrs _remainUDAs = new BaseUserDefinedAttrs();
        private List<string> _selectedUDAs = new List<string>();

        // Private Consts
        private const string COLUMN_ATTRIBUTE_KEY = "clnAttributeKey";
        private const string COLUMN_ATTRIBUTE_NAME = "clnAttributeName";
        private const string COLUMN_ATTRIBUTE_DATA_TYPE = "clnAttributeDataType";
        private const string COLUMN_ATTRIBUTE_DATA_TYPE_STR = "clnAttributeDataTypeStr";
        private const string COLUMN_ATTRIBUTE_DESCRIPTION = "clnAttributeDesc";
        private CrmAttribute crmAttributeEntity = new CrmAttribute();

        #endregion

        private void gridAttributeSelectView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void UdaColumnsSelect_Load(object sender, EventArgs e)
        {
            GridViewHelper.SetGridView(gridAttributeSelectView);
        }
    }
}

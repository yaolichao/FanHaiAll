using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using BarCodePrint;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using FanHai.Hemera.Share.Common;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Mask;
using DevExpress.Utils;
using FanHai.Hemera.Utils.Dialogs;
using System.Threading;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示工作站作业（进站/出站）明细的控件类。
    /// </summary>
    public partial class LotToWarehouseDetail : BaseUserCtrl
    {
        LotOperationEntity _entity = new LotOperationEntity();          //批次操作类。
        LotDispatchDetailModel _model = null;                           //参数数据。
        IViewContent _view = null;                                      //当前视图。
        DataTable _dtOperationBaseData = null;                          //暂存待采集的工序数据。
        DataTable _dtOperationParamData = null;                         //暂存待采集的工序参数数据。
        DataTable _dtProductGrade = null;                               //暂存产品等级。
        string _activity = string.Empty;                                //操作动作名称
        string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
        string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
        /// <summary>
        /// 用于代码同步的标识位。
        /// </summary>
        private readonly object objlock = new object();
        /// <summary>
        /// 工序参数排序方式。
        /// </summary>
        OperationParamOrderType _paramOrderType = OperationParamOrderType.FirstRow;
        /// <summary>
        /// 每行参数个数。
        /// </summary>
        int _paramCountPerRow = 2;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model"></param>
        public LotToWarehouseDetail(LotDispatchDetailModel model, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotToWarehouseDetail_Load(object sender, EventArgs e)
        {
            lblMenu.Text = "生产管理>过站管理>过站作业-入库";
            //绑定线别，站别和设备
            lblLine.Text = this._model.LineName;
            lblWork.Text = this._model.OperationName;
            lblEquipment.Text = this._model.EquipmentName;
            BindParams();
            ResetControlValue();
            BindProductGrade();
            if (this.gvParams.Columns.Count > 0)
            {
                this.gvParams.Columns[0].Visible = false;
            }
        }
        /// <summary>
        /// 绑定产品等级。
        /// </summary>
        private void BindProductGrade()
        {
            string[] columns = new string[] { "Column_code", "Column_Name" };
            List<KeyValuePair<string, string>> where = new List<KeyValuePair<string, string>>();
            where.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
            this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, BASEDATA_CATEGORY_NAME.Basic_TestRule_PowerSet, where);
        }
        /// <summary>
        /// 绑定工序参数数据。
        /// </summary>
        private void BindParams()
        {
            RouteQueryEntity queryEntity = new RouteQueryEntity();
            DataSet dsOperationData = queryEntity.GetOperationBaseAndParamInfo(this._model.OperationName);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                return;
            }
            //是否获取到工序参数数据。
            if (null == dsOperationData
                || !dsOperationData.Tables.Contains(POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME)
                || !dsOperationData.Tables.Contains(POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME)
                )
            {
                return;
            }
            this._dtOperationParamData = dsOperationData.Tables[POR_ROUTE_OPERATION_PARAM_FIELDS.DATABASE_TABLE_NAME];
            this._dtOperationBaseData = dsOperationData.Tables[POR_ROUTE_OPERATION_VER_FIELDS.DATABASE_TABLE_NAME];
            //是否获取到工序参数数据。
            if (this._dtOperationBaseData.Rows.Count < 1 || this._dtOperationParamData.Rows.Count < 1)
            {
                return;
            }
            //根据工步主键获取绑定参数。
            int nOrderType = Convert.ToInt32(this._dtOperationBaseData.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_ORDER_TYPE]);
            this._paramOrderType = (OperationParamOrderType)nOrderType;
            this._paramCountPerRow = Convert.ToInt32(this._dtOperationBaseData.Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_COUNT_PER_ROW]);
            //初始化采集参数控件。
            InitParamsControl(this._dtOperationParamData, this._paramOrderType, this._paramCountPerRow);
        }
        /// <summary>
        /// 初始化参数控件。
        /// </summary>
        /// <param name="paramCountPerRow">每行参数个数。</param>
        private void InitParamsControl(DataTable dtStepParamData, OperationParamOrderType orderType, int paramCountPerRow)
        {
            //初始化结构。
            DataTable dtParams = new DataTable();
            for (int i = 0; i < paramCountPerRow; i++)
            {
                GridColumn gcolRowNum = new GridColumn();
                gcolRowNum.Caption = "序号";
                gcolRowNum.FieldName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, i);
                gcolRowNum.OptionsColumn.ReadOnly = true;
                gcolRowNum.OptionsColumn.AllowEdit = false;
                gcolRowNum.AppearanceCell.BackColor = System.Drawing.Color.LightGray;
                gcolRowNum.Name = string.Format("gcolRowNum{0}", i);
                gcolRowNum.OptionsColumn.FixedWidth = true;
                gcolRowNum.Width = 50;
                gcolRowNum.Visible = true;
                gcolRowNum.VisibleIndex = i * paramCountPerRow + 1;
                gcolRowNum.Tag = i;
                this.gvParams.Columns.Add(gcolRowNum);

                GridColumn gcolName = new GridColumn();
                gcolName.Caption = "参数名称";
                gcolName.FieldName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_NAME, i);
                gcolName.OptionsColumn.ReadOnly = true;
                gcolName.OptionsColumn.AllowEdit = false;
                gcolName.AppearanceCell.BackColor = System.Drawing.Color.LightGray;
                gcolName.Name = string.Format("gcolName{0}", i);
                gcolName.Visible = true;
                gcolName.VisibleIndex = i * paramCountPerRow + 2;
                gcolName.Tag = i;
                this.gvParams.Columns.Add(gcolName);

                GridColumn gcolValue = new GridColumn();
                gcolValue.Caption = "参数值";
                gcolValue.FieldName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_VALUE, i);
                gcolValue.Name = string.Format("gcolValue{0}", i);
                gcolValue.Visible = true;
                gcolValue.Tag = i;
                gcolValue.VisibleIndex = i * paramCountPerRow + 3;
                this.gvParams.Columns.Add(gcolValue);

                dtParams.Columns.Add(gcolRowNum.FieldName);
                dtParams.Columns.Add(string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, i));
                dtParams.Columns.Add(gcolName.FieldName);
                dtParams.Columns.Add(gcolValue.FieldName);
            }
            //初始化数据。
            //计算行数。
            int nRowCount = dtStepParamData.Rows.Count / paramCountPerRow;
            if (dtStepParamData.Rows.Count % paramCountPerRow != 0)
            {
                nRowCount = nRowCount + 1;
            }
            for (int i = 0; i < nRowCount; i++)
            {
                DataRow drParam = dtParams.NewRow();
                dtParams.Rows.Add(drParam);
                int nCol = 0;
                for (int j = 0; j < paramCountPerRow; j++)
                {
                    int nRow = i * paramCountPerRow + j;  //先行后列时，抓取工步参数数据所在的行。
                    if (orderType == OperationParamOrderType.FirstColumn)
                    {
                        nRow = i + nRowCount * j;
                    }
                    if (nRow >= dtStepParamData.Rows.Count)
                    {
                        break;
                    }
                    drParam[nCol] = dtStepParamData.Rows[nRow][POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_INDEX];
                    drParam[nCol + 1] = dtStepParamData.Rows[nRow][POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_KEY];
                    drParam[nCol + 2] = dtStepParamData.Rows[nRow][POR_ROUTE_STEP_PARAM_FIELDS.FIELD_PARAM_NAME];
                    nCol = nCol + 4;//跳过参数值。
                }
            }
            this.lciParams.SizeConstraintsType = SizeConstraintsType.Custom;
            int height = this.gvParams.RowHeight * (dtParams.Rows.Count + 1) + this.gvParams.ColumnPanelRowHeight + 10;
            this.lciParams.MinSize = new Size(this.lciParams.MinSize.Width, height);
            this.lciParams.MaxSize = new Size(this.lciParams.MaxSize.Width, height);
            this.lciParams.Size = new Size(this.lciParams.Size.Width, height);
            this.gcParams.DataSource = dtParams;
            this.gcParams.MainView = this.gvParams;
            this.gvParams.OptionsView.ColumnAutoWidth = true;
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            if (this._dtOperationParamData == null
              || this._dtOperationParamData.Rows.Count < 1)
            {
                this.gcParams.Visible = false;
                this.lciParams.Visibility = LayoutVisibility.Never;
                this.lcgParams.Visibility = LayoutVisibility.Never;
            }
            //初始化参数列表。
            DataTable dtParam = this.gcParams.DataSource as DataTable;
            if (dtParam != null)
            {
                foreach (DataColumn col in dtParam.Columns)
                {
                    if (col.ColumnName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
                    {
                        for (int i = 0; i < dtParam.Rows.Count; i++)
                        {
                            dtParam.Rows[i][col] = string.Empty;
                        }
                    }
                }
            }
            //初始化拖号列表。
            DataTable dtList = this.gcPalletInfo.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }
            ReCalcPalletListSize();
            this.bePalletNo.Text = string.Empty;
            this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TO_WAREHOUSE;
            this.teRemark.Text = string.Empty;
            this.bePalletNo.Select();

        }
        /// <summary>
        /// 移除托盘信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemovePallet_Click(object sender, EventArgs e)
        {
            int index = this.gvPalletInfo.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要移除的托盘信息。", "提示");
                return;
            }
            DataTable dtList = this.gcPalletInfo.DataSource as DataTable;
            dtList.Rows.RemoveAt(index);
            ReCalcPalletListSize();
        }
        /// <summary>
        /// 托盘号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bePalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAddPallet_Click(sender, e);
            }
        }
        /// <summary>
        /// 添加托盘信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPallet_Click(object sender, EventArgs e)
        {
            try
            {
                AddPallet();
            }
            finally
            {
                this.bePalletNo.Select();
                this.bePalletNo.SelectAll();
            }
        }
        /// <summary>
        /// 添加托盘信息。
        /// </summary>
        private void AddPallet()
        {
            DataTable dtList = this.gcPalletInfo.DataSource as DataTable;
            string palletNo = this.bePalletNo.Text;
            //托盘号不能为空。
            if (string.IsNullOrEmpty(palletNo))
            {
                MessageService.ShowMessage("请输入托盘号。", "提示");
                return;
            }
            if (dtList != null)
            {
                //一次入库的托盘记录不能超过
                if (dtList.Rows.Count >= 20)
                {
                    MessageService.ShowMessage("托盘信息记录不能超过20条。", "提示");
                    return;
                }
                //托盘号不能在托盘信息列表中存在。
                DataRow drPallet = dtList.AsEnumerable()
                              .Where(dr => Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]) == palletNo)
                              .SingleOrDefault();
                if (drPallet != null)
                {
                    MessageService.ShowMessage("托盘信息在列表中已经存在。", "提示");
                    this.gvPalletInfo.FocusedRowHandle = dtList.Rows.IndexOf(drPallet);
                    return;
                }
            }
            //获取托盘信息记录
            DataSet dsPalletReturn = this._entity.GetPalletInfo(palletNo);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                return;
            }
            if (null == dsPalletReturn
                || dsPalletReturn.Tables.Count < 1
                || dsPalletReturn.Tables[0].Rows.Count < 1
                )
            {
                MessageService.ShowMessage("托盘号不存在。", "提示");
                return;
            }
            if (dsPalletReturn.Tables[0].Rows.Count > 1)
            {
                MessageService.ShowMessage("托盘记录异常，请检查。", "提示");
                return;
            }
            DataRow drPalletReturn = dsPalletReturn.Tables[0].Rows[0];
            string roomKey = Convert.ToString(drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_ROOM_KEY]);
            if (roomKey != this._model.RoomKey)
            {
                MessageService.ShowMessage("托盘不属于当前车间，请检查。", "提示");
                return;
            }
            //托盘记录必须是经过抽检的
            int checkFlag = Convert.ToInt32(drPalletReturn[WIP_CONSIGNMENT_FIELDS.FIELDS_CS_DATA_GROUP]);
            if (checkFlag != 2)
            {
                if (checkFlag == 10)
                {
                    MessageService.ShowMessage("托盘被入库检返到包装，请包装人员整托出托后再次包装检验。", "提示");
                }
                else if (checkFlag >= 3)
                {
                    MessageService.ShowMessage("托盘已经入库，请确认。", "提示");
                }
                else
                {
                    MessageService.ShowMessage("托盘未做入库检验，不能入库。", "提示");
                }
                return;
            }
            if (dtList == null)
            {
                dtList = dsPalletReturn.Tables[0];
                this.gcPalletInfo.DataSource = dtList;
                this.gcPalletInfo.MainView = this.gvPalletInfo;
                this.lciPalletList.SizeConstraintsType = SizeConstraintsType.Custom;
            }
            else
            {
                dtList.Merge(dsPalletReturn.Tables[0]);
            }
            ReCalcPalletListSize();
        }
        /// <summary>
        /// 重新计算托盘列表大小。
        /// </summary>
        private void ReCalcPalletListSize()
        {
            int height = this.gvPalletInfo.RowHeight * (this.gvPalletInfo.RowCount + 1) + this.gvPalletInfo.ColumnPanelRowHeight + 10;
            this.lciPalletList.MinSize = new Size(this.lciPalletList.MinSize.Width, height);
            this.lciPalletList.MaxSize = new Size(this.lciPalletList.MaxSize.Width, height);
            this.lciPalletList.Size = new Size(this.lciPalletList.Size.Width, height);
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //重新打开工作站作业视图。
            LotDispathViewContent view = new LotDispathViewContent(this._model);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 重置按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            ResetControlValue();
        }
        /// <summary>
        /// 自定义显示参数列表中的参数值编辑器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvParams_ShowingEditor(object sender, CancelEventArgs e)
        {
            //绑定原因代码。
            if (this.gvParams.FocusedColumn.FieldName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE) 
                && this.gvParams.FocusedRowHandle >= 0)
            {
                this.gvParams.FocusedColumn.ColumnEdit = null;
                DataRow drCurrentStepParams = this.gvParams.GetFocusedDataRow();
                //没有设置数据，不显示编辑器。
                if (drCurrentStepParams == null)
                {
                    e.Cancel = true;
                }
                int index = Convert.ToInt32(this.gvParams.FocusedColumn.Tag);
                int row = this.gvParams.FocusedRowHandle;
                string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                //当前单元格没有对应任何参数，则不能显示编辑器。
                string paramIndex = Convert.ToString(drCurrentStepParams[paramIndexColumnName]);
                string paramKey = Convert.ToString(drCurrentStepParams[paramKeyColumnName]);
                if (string.IsNullOrEmpty(paramKey) || string.IsNullOrEmpty(paramIndex))
                {
                    e.Cancel = true;
                }
                //获取参数设置。
                DataRow drStepParamData = this._dtOperationParamData
                                             .AsEnumerable()
                                             .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                       && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                             .SingleOrDefault();
                //如果有获取到对应行
                if (drStepParamData != null)
                {
                    bool isReadOnly = Convert.ToBoolean(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_READONLY]);
                    AttributeDataType dataType = (AttributeDataType)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_TYPE]);
                    OperationParamDataFrom dataFrom = (OperationParamDataFrom)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_FROM]);
                    //只读，不显示编辑器。
                    if (isReadOnly)
                    {
                        e.Cancel = true;
                    }
                    //如果数据来源是上料，显示对应的编辑器，并绑定数据。
                    if (dataFrom == OperationParamDataFrom.UseMaterial)
                    {
                        RepositoryItemComboBox edit = new RepositoryItemComboBox();
                        edit.NullText = string.Empty;
                        //获取数据
                        this.gvParams.FocusedColumn.ColumnEdit = edit;
                    }
                    //如果数据来源是设备接口，显示对应的编辑器，并绑定数据。
                    else if (dataFrom == OperationParamDataFrom.EquipmentInterface)
                    {

                    }
                    //如果数据来源是手工输入，根据数据类型显示编辑器。
                    else
                    {
                        if (dataType == AttributeDataType.BOOLEAN)
                        {
                            RepositoryItemComboBox edit = new RepositoryItemComboBox();
                            edit.Items.Add("是");
                            edit.Items.Add("否");
                            edit.NullText = string.Empty;
                            edit.Mask.UseMaskAsDisplayFormat = false;
                            edit.TextEditStyle = TextEditStyles.DisableTextEditor;
                            this.gvParams.FocusedColumn.ColumnEdit = edit;
                        }
                        else if (dataType == AttributeDataType.DATE || dataType == AttributeDataType.DATETIME)
                        {
                            RepositoryItemDateEdit edit = new RepositoryItemDateEdit();
                            edit.NullText=string.Empty;
                            string mask = "yyyy-MM-dd";
                            if (dataType == AttributeDataType.DATETIME)
                            {
                                mask = "yyyy-MM-dd HH:mm:ss";
                                edit.VistaDisplayMode = DefaultBoolean.True;
                                edit.VistaEditTime = DefaultBoolean.True;
                            }
                            edit.EditMask = mask;
                            edit.Mask.MaskType = MaskType.DateTime;
                            edit.DisplayFormat.FormatType = FormatType.Custom;
                            edit.DisplayFormat.FormatString = mask;
                            edit.EditFormat.FormatType = FormatType.Custom;
                            edit.EditFormat.FormatString = mask;
                            edit.Mask.EditMask = mask;
                            edit.Mask.ShowPlaceHolders = false;
                            this.gvParams.FocusedColumn.ColumnEdit = edit;
                        }
                        else if (dataType == AttributeDataType.FLOAT)
                        {
                            RepositoryItemTextEdit edit = new RepositoryItemTextEdit();
                            edit.NullText = string.Empty;
                            edit.Mask.MaskType = MaskType.RegEx;
                            edit.Mask.EditMask = @"([+-]?\d{0,10})(\.\d{0,8})?";
                            edit.Mask.ShowPlaceHolders = false;
                            this.gvParams.FocusedColumn.ColumnEdit = edit;
                        }
                        else if (dataType == AttributeDataType.INTEGER)
                        {
                            RepositoryItemTextEdit edit = new RepositoryItemTextEdit();
                            edit.NullText = string.Empty;
                            edit.Mask.MaskType = MaskType.RegEx;
                            edit.Mask.EditMask = @"[+-]?\d{0,10}";
                            edit.Mask.ShowPlaceHolders = false;
                            this.gvParams.FocusedColumn.ColumnEdit = edit;
                        }
                        else//默认
                        {
                            RepositoryItemTextEdit edit = new RepositoryItemTextEdit();
                            edit.NullText = string.Empty;
                            edit.MaxLength = 100;
                            this.gvParams.FocusedColumn.ColumnEdit = edit;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 自定义绘制参数单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvParams_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            //绑定原因代码。
            if (e.Column.FieldName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
            {
                DataRow drCurrentStepParams = this.gvParams.GetDataRow(e.RowHandle);
                //没有设置数据。
                if (drCurrentStepParams == null)
                {
                    return;
                }
                int index = Convert.ToInt32(e.Column.Tag);
                int row = e.RowHandle;
                string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                //当前单元格没有对应任何参数，则不能显示编辑器。
                string paramIndex = Convert.ToString(drCurrentStepParams[paramIndexColumnName]);
                string paramKey = Convert.ToString(drCurrentStepParams[paramKeyColumnName]);
                if (string.IsNullOrEmpty(paramKey) || string.IsNullOrEmpty(paramIndex))
                {
                    return;
                }
                //获取参数设置。
                DataRow drStepParamData = this._dtOperationParamData
                                             .AsEnumerable()
                                             .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                       && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                             .SingleOrDefault();
                //如果有获取到对应行
                if (drStepParamData != null)
                {
                    AttributeDataType dataType = (AttributeDataType)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_DATA_TYPE]);
                    //自定义显示日期或日期时间值。
                    if (dataType == AttributeDataType.DATE || dataType == AttributeDataType.DATETIME)
                    {
                        string mask = "yyyy-MM-dd";
                        if(dataType == AttributeDataType.DATETIME)
                        {
                            mask = "yyyy-MM-dd HH:mm:ss";
                        }
                        string val = Convert.ToString(e.CellValue);
                        if (!string.IsNullOrEmpty(val))
                        {
                            e.DisplayText = Convert.ToDateTime(val).ToString(mask);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 自定义绘制托盘信息列表单元格显示文本。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPalletInfo_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolPalletRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gcolPalletGrade && this._dtProductGrade!=null)
            {
                DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'",e.CellValue));
                if (drs.Length > 0)
                {
                    e.DisplayText = Convert.ToString(drs[0]["Column_Name"]);
                }
            }
        }
        /// <summary>
        /// 自定义绘制批次信息列表单元格显示文本。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gcolProLevel && this._dtProductGrade != null)
            {
                DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'", e.CellValue));
                if (drs.Length > 0)
                {
                    e.DisplayText = Convert.ToString(drs[0]["Column_Name"]);
                }
            }
        }
        /// <summary>
        /// 托盘信息双击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPalletInfo_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = this.gvPalletInfo.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataRow drPallet = this.gvPalletInfo.GetFocusedDataRow();
                string palletNo = Convert.ToString(drPallet[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                BindLotInfo(palletNo);
            }
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        /// <param name="palletNo">托盘号。</param>
        private void BindLotInfo(string palletNo)
        {
            DataSet dsLotInfo=this._entity.GetPalletLotInfo(palletNo);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                this.lcgLotInfo.Visibility = LayoutVisibility.Never;
                MessageService.ShowError(this._entity.ErrorMsg);
                return;
            }
            DataTable dtLotInfo = null;
            if (dsLotInfo != null && dsLotInfo.Tables.Count > 0)
            {
                dtLotInfo = dsLotInfo.Tables[0];
            }
            this.gcLotInfo.DataSource = dtLotInfo;
            this.gcLotInfo.MainView = this.gvLotInfo;

            if (dtLotInfo == null || dtLotInfo.Rows.Count == 0)
            {
                this.lcgLotInfo.Visibility = LayoutVisibility.Never;
            }
            else
            {
                this.lcgLotInfo.Visibility = LayoutVisibility.Always;
            }
            int height = this.gvLotInfo.RowHeight * (dtLotInfo.Rows.Count + 1) + this.gvLotInfo.ColumnPanelRowHeight + 10;
            this.lciLotInfo.MinSize = new Size(this.lciLotInfo.MinSize.Width, height);
            this.lciLotInfo.MaxSize = new Size(this.lciLotInfo.MaxSize.Width, height);
            this.lciLotInfo.Size = new Size(this.lciLotInfo.Size.Width, height);
        }
        /// <summary>
        /// 确认按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            //判断获取工步信息是否失败。
            if (this._dtOperationParamData == null)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请关闭该界面重试。", this._model.OperationName), "提示");
                return;
            }
            string remark = this.teRemark.Text;
            //托盘信息。
            DataTable dtPalletInfo = this.gcPalletInfo.DataSource as DataTable;
            if (dtPalletInfo == null || dtPalletInfo.Rows.Count <= 0)
            {
                MessageService.ShowMessage("请输入要入库的托盘号。", "提示");
                this.bePalletNo.Select();
                this.bePalletNo.SelectAll();
                return;
            }
            dtPalletInfo.TableName = WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME;
            //工步参数信息。
            DataTable dtCurrentOperationParams = this.gcParams.DataSource as DataTable;
            DataTable dtStepParams = null;
            //验证采集参数列表。
            if (this.gcParams.Visible && dtCurrentOperationParams != null && dtCurrentOperationParams.Rows.Count > 0)
            {
                if (this.gvParams.State == GridState.Editing && this.gvParams.IsEditorFocused && this.gvParams.EditingValueModified)
                {
                    this.gvParams.SetFocusedRowCellValue(this.gvParams.FocusedColumn, this.gvParams.EditingValue);
                }
                this.gvParams.UpdateCurrentRow();
                if (dtCurrentOperationParams != null && dtCurrentOperationParams.Rows.Count >= 1)
                {
                    //组织工步参数数据。
                    dtStepParams = CommonUtils.CreateDataTable(new WIP_PARAM_FIELDS());
                    //遍历所有列。
                    foreach (DataColumn col in dtCurrentOperationParams.Columns)
                    {
                        //如果是输入值的列名。
                        if (col.ColumnName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
                        {
                            GridColumn gcol = this.gvParams.Columns[col.ColumnName];
                            int index = Convert.ToInt32(gcol.Tag); //对应每行工步第几个参数。

                            #region 遍历该列所有行
                            for (int row = 0; row < dtCurrentOperationParams.Rows.Count; row++)
                            {
                                string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                                string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                                string paramNameColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_NAME, index);
                                string paramIndex = Convert.ToString(dtCurrentOperationParams.Rows[row][paramIndexColumnName]);
                                string paramKey = Convert.ToString(dtCurrentOperationParams.Rows[row][paramKeyColumnName]);
                                string paramName = Convert.ToString(dtCurrentOperationParams.Rows[row][paramNameColumnName]);
                                string paramValue = Convert.ToString(dtCurrentOperationParams.Rows[row][col]);
                                DataRow drStepParamData = this._dtOperationParamData
                                                              .AsEnumerable()
                                                              .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                                        && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                                              .SingleOrDefault();
                                //如果有获取到对应行
                                if (drStepParamData != null)
                                {
                                    //是否必须输入。
                                    bool isMustInput = Convert.ToBoolean(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_MUSTINPUT]);
                                    //如果必须输入 且参数值为空。
                                    if (isMustInput && string.IsNullOrEmpty(paramValue))
                                    {
                                        MessageService.ShowMessage(string.Format("{0}不能为空，请确认。", paramName), "提示");
                                        this.gvParams.FocusedColumn = gcol;
                                        this.gvParams.FocusedRowHandle = row;
                                        this.gvParams.ShowEditor();
                                        return;
                                    }
                                    #region 如果参数值不为空,将记录添加到待提交的工步参数表中
                                    if (!string.IsNullOrEmpty(paramValue))
                                    {
                                        //添加一笔工步参数值。
                                        DataRow drStepParams = dtStepParams.NewRow();
                                        dtStepParams.Rows.Add(drStepParams);
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX] = paramIndex;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_KEY] = paramKey;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_NAME] = paramName;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_VALUE] = paramValue;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_EDITOR] = this._model.UserName;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                                        drStepParams[WIP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            //组织其他附加参数数据
            Hashtable htMaindata = new Hashtable();
            string operationKey = Convert.ToString(this._dtOperationBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
            string duration = Convert.ToString(this._dtOperationBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);
            htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
            htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, duration);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, this._model.ShiftKey);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, this._model.ShiftName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
            htMaindata.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
            DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
            dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
            DataSet dsParams = new DataSet();
            dsParams.Merge(dtParams);
            if (dtStepParams != null)
            {
                dsParams.Merge(dtStepParams);
            }
            dsParams.Merge(dtPalletInfo);
            //执行入库作业。
            ParameterizedThreadStart start = new ParameterizedThreadStart(PalletToWarehouse);
            Thread t = new Thread(start);
            t.Start(dsParams);
        }
        /// <summary>
        /// 执行入库作业。
        /// </summary>
        /// <param name="obj">包含入库信息的数据集对象。</param>
        public void PalletToWarehouse(object obj)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                //this.lblMsg1.Visible = true;
                this.lblMsg.Visibility = LayoutVisibility.Always;
                this.lblMsg.Text = string.Format("正在执行入库操作，请勿关闭界面，等待...");
                this.tableLayoutPanelMain.Enabled = false;
            }));
            try
            {
                DataSet dsParam = obj as DataSet;
                DataTable dtParam = dsParam.Tables[TRANS_TABLES.TABLE_PARAM];
                DataTable dtStepParams = null;
                if (dsParam.Tables.Contains(WIP_PARAM_FIELDS.DATABASE_TABLE_NAME))
                {
                    dtStepParams = dsParam.Tables[WIP_PARAM_FIELDS.DATABASE_TABLE_NAME];
                }
                DataTable dtPalletInfo = dsParam.Tables[WIP_CONSIGNMENT_FIELDS.DATABASE_TABLE_NAME];
                StringBuilder sbMsg = new StringBuilder();
                //循环提交托盘记录过站。
                foreach (DataRow dr in dtPalletInfo.Rows)
                {
                    DataSet dsPalletParams = new DataSet();
                    dsPalletParams.Merge(dtParam);
                    if (dtStepParams != null)
                    {
                        dsPalletParams.Merge(dtStepParams);
                    }
                    string consignmentKey = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY]);
                    string palletNo = Convert.ToString(dr[WIP_CONSIGNMENT_FIELDS.FIELDS_VIRTUAL_PALLET_NO]);
                    string rowFilter = string.Format("{0}='{1}'", WIP_CONSIGNMENT_FIELDS.FIELDS_CONSIGNMENT_KEY, consignmentKey);
                    DataTable dtTempPalletInfo = dtPalletInfo.Select(rowFilter).CopyToDataTable();
                    dtTempPalletInfo.TableName = dtPalletInfo.TableName;
                    dsPalletParams.Merge(dtTempPalletInfo);
                    //显示进站进度
                    this.Invoke(new MethodInvoker(() =>
                    {
                        //this.lblMsg1.Visible = true;
                        this.lblMsg.Visibility = LayoutVisibility.Always;
                        int totalCount = dtPalletInfo.Rows.Count;
                        int count = dtPalletInfo.Rows.Count - this.gvPalletInfo.RowCount;
                        this.lblMsg.Text = string.Format("正在执行托盘[{0}]进站操作，请勿关闭界面，等待...({1}/{2})", palletNo, count + 1, totalCount);
                    }));
                    this._entity.PalletTrackIn(dsPalletParams);
                    //显示出站进度
                    this.Invoke(new MethodInvoker(() =>
                    {
                        //this.lblMsg1.Visible = true;
                        this.lblMsg.Visibility = LayoutVisibility.Always;
                        int totalCount = dtPalletInfo.Rows.Count;
                        int count = dtPalletInfo.Rows.Count - this.gvPalletInfo.RowCount;
                        this.lblMsg.Text = string.Format("正在执行托盘[{0}]出站操作，请勿关闭界面，等待...({1}/{2})", palletNo, count + 1, totalCount);
                    }));
                    this._entity.PalletTrackOut(dsPalletParams);
                    //显示入库进度。
                    this.Invoke(new MethodInvoker(() =>
                    {
                        int totalCount = dtPalletInfo.Rows.Count;
                        int count = dtPalletInfo.Rows.Count - this.gvPalletInfo.RowCount;
                        this.lblMsg.Text = string.Format("正在执行托盘[{0}]入库操作，请勿关闭界面，等待...({1}/{2})", palletNo, count + 1, totalCount);
                    }));
                    this._entity.PalletToWarehouse(dsPalletParams);
                    //入库操作失败。
                    if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                    {
                        sbMsg.AppendLine(this._entity.ErrorMsg);
                    }
                    else
                    {
                        ////已提交成功，移除托盘记录。
                        this.Invoke(new MethodInvoker(() =>
                        {
                            //已提交成功，移除托盘记录。
                            DataTable dtPallet = this.gcPalletInfo.DataSource as DataTable;
                            DataRow[] drs = dtPallet.Select(rowFilter);
                            foreach (DataRow drPallet in drs)
                            {
                                dtPallet.Rows.Remove(drPallet);
                            }
                            ReCalcPalletListSize();
                        }));
                    }
                    dsPalletParams = null;
                }
                //部分托盘入库失败。
                if (sbMsg.Length>0)
                {
                    MessageService.ShowError(sbMsg.ToString());
                }
                else
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        ResetControlValue();
                    }));
                }
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    //this.lblMsg1.Visible = false;
                    this.lblMsg.Visibility = LayoutVisibility.Never;
                    this.tableLayoutPanelMain.Enabled = true;
                }));
            }
        }
        /// <summary>
        /// 处理采集参数回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcParams_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (gvParams.IsGroupRow(gvParams.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter)
            {
                int rowHandle = this.gvParams.FocusedRowHandle;
                int i = Convert.ToInt32(gvParams.FocusedColumn.Tag);
                string paramValueName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_VALUE, i);
                //如果当前选择的列不是可输入值的列。
                if (this.gvParams.FocusedColumn.FieldName != paramValueName)
                {
                    gvParams.FocusedColumn = gvParams.Columns[paramValueName];
                }
                else
                {
                    //先行。
                    if (this._paramOrderType == OperationParamOrderType.FirstRow)
                    {
                        //跳到下一个参数。
                        i = i + 1;
                        //跳回第一个参数所在的单元格。
                        if (i >= this._paramCountPerRow)
                        {
                            i = 0;
                        }
                    }
                    else
                    {
                        //跳到下一行。
                        rowHandle += 1;
                        if (rowHandle >= this.gvParams.RowCount)
                        {
                            //跳回第一行。
                            rowHandle = 0;
                            //跳到下一个参数。
                            i = i + 1;
                            //跳回第一个参数所在的单元格。
                            if (i >= this._paramCountPerRow)
                            {
                                i = 0;
                            }
                        }
                        this.gvParams.FocusedRowHandle = rowHandle;
                    }
                    paramValueName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_VALUE, i);
                    gvParams.FocusedColumn = gvParams.Columns[paramValueName];
                    gvParams.ShowEditor();
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// 控件响应Ctrl+Enter事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs args = new KeyEventArgs(keyData);
            if (args.Control && args.KeyCode == Keys.Enter)
            {
                tsbOK_Click(null, null);
                args.Handled = true;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// 选择托盘。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bePalletNo_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PalletQueryHelpModel model = new PalletQueryHelpModel();
            model.RoomKey = Convert.ToString(this._model.RoomKey);
            model.PalletState = PalletState.Checked;
            PalletQueryHelpDialog dlg = new PalletQueryHelpDialog(model);
            dlg.OnValueSelected += new PalletQueryValueSelectedEventHandler(PalletQueryHelpDialog_OnValueSelected);
            Point i = bePalletNo.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            dlg.Width = bePalletNo.Width;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + bePalletNo.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + bePalletNo.Width - dlg.Width, i.Y + bePalletNo.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + bePalletNo.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 选中托盘值后的事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PalletQueryHelpDialog_OnValueSelected(object sender, PalletQueryValueSelectedEventArgs args)
        {
            this.bePalletNo.Text = args.PalletNo;
        }
    }
}

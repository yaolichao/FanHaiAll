using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Dialogs;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;

using BarCodePrint;

using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Mask;
using DevExpress.Utils;


namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 表示工作站作业（进站/出站）明细的控件类。
    /// </summary>
    public partial class LotDispatchDetail : BaseUserCtrl
    {
        private const string MESSAGEBOX_CAPTION = "提示";
        private const string REASON_CODE_TYPE_DEFECT = "D";             //不良原因代码所属类型标识
        private const string REASON_CODE_TYPE_SCRAP = "S";              //报废原因代码所属类型标识

        LotComponentTrayEntity _lotComponentTrayEntity = new LotComponentTrayEntity(); //分档操作类
        LotOperationEntity _entity = new LotOperationEntity();          //批次操作类。
        LotDispatchDetailModel _model = null;                           //参数数据。
        IViewContent _view = null;                                      //当前视图。
        DataTable _dtLotInfo = null;                                    //暂存批次信息。
        DataTable _dtStepBaseData = null;                               //暂存待采集的工步数据。
        DataTable _dtStepParamData = null;                              //暂存待采集的工步参数数据。
        DataTable _dtOrderParamSetting = null;                          //工单对应参数的设置数据，用于卡控过站。
        DataTable _dtColor = null;                                      //包含颜色信息的数据表。
        DataTable dtStepParams = null;                                  //工步参数
        int _flag = 0;                                                  //标记是否自动上料  1：是   0：否
        string _activity = string.Empty;                                //操作动作名称
        string ErrorMessage = string.Empty;
        //bool _isTrackInDelay = false;                                 //批次进站是否超时。
        bool _bLotEnterKeyNextControl = false;                          //批次列表。用于控制按回车选择下一个控件而不是显示编辑器。
        bool _bParamEnterKeyNextControl = false;                        //参数列表。用于控制按回车选择下一个控件而不是显示编辑器。
        bool _bListEnterKeyNextControl = false;                         //不良/报废列表。用于控制按回车选择下一个控件而不是显示编辑器。
        /// <summary>
        /// 暂存原因分类数据表。
        /// </summary>
        DataTable _dtReasonCodeClass = null;
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
        public LotDispatchDetail(LotDispatchDetailModel model, DataTable dtLotInfo, IViewContent view)
        {
            InitializeComponent();
            this._view = view;
            this._model = model;
            this._dtLotInfo = dtLotInfo;
        }
        /// <summary>
        /// 窗体载入事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotDispatchDetail_Load(object sender, EventArgs e)
        {
            BindLotInfo();          /// 绑定批次信息。
            BindParams();
            InitReasonCodeList();
            BindReasonCodeClassToDataTable();
            BindReasonCodeType();
            InitialDelayReasonBox();
            //重置控件值。
            ResetControlValue();
            BindParamsMatrial();    ///绑定上料的物料信息。
            //判断是否为终检作业，发生误判情况，动态添加“返回上一步作业”，进行重新判定。
            IsCustCheckOperation();
            if (this.gvParams.Columns.Count > 0)
            {
                this.gvParams.Columns[0].Visible = false;
            }
        }
        private bool CheckFacTrueOrFalse()
        {
            //1.判定基础数据工厂是否设定自动上料：是 继续  否 return
            string[] columns = new string[] { "FAC_NAME", "FAC_CODE","STATUS" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Wip_LotDispatch_CheckFac");
            DataTable dtFac = BaseData.Get(columns, category);
            DataRow[] drs = dtFac.Select(string.Format("FAC_CODE='{0}'", _model.RoomName));
            DataTable dt = dtFac.Clone();
            foreach (DataRow dr in drs)
                dt.ImportRow(dr);
            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool CheckFacSpecialTrueOrFalse()
        {
            //1.判定是否使用通过同步sap传入参数组来卡控物料信息：是 继续  否 return
            string[] columns = new string[] { "FAC_NAME", "FAC_CODE", "STATUS" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Wip_LotDispatch_CheckFacForSpecial");
            DataTable dtFac = BaseData.Get(columns, category);
            DataRow[] drs = dtFac.Select(string.Format("FAC_CODE='{0}'", _model.RoomName));
            DataTable dt = dtFac.Clone();
            foreach (DataRow dr in drs)
                dt.ImportRow(dr);
            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        ///绑定上料的物料信息。
        /// </summary>
        private void BindParamsMatrial()
        {
            if (!CheckFacTrueOrFalse()) return;
            //lcgEquMatInf.Visibility = LayoutVisibility.Always;
            string workorder = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
            //DataSet dsEquipmentInf = _entity.GetEquipmentMatInf(_model.RoomKey, _model.EquipmentKey, _model.OperationName, workorder);
            //2.抓取工艺流程中各工艺自动上料参数，条件该 车间，设备，工序，工单时间节点最早的物料信息
            string stepKey = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            DataSet dsStepParams = new DataSet();
            if (this._model.OperationType == LotOperationType.TrackIn)
            {
                dsStepParams = _entity.GetStepParams(stepKey, _model.RoomKey, _model.EquipmentKey, _model.OperationName, workorder, OperationParamDCType.TrackIn);    //进战时数据采集
            }
            else if (this._model.OperationType == LotOperationType.TrackOut)
            {
                dsStepParams = _entity.GetStepParams(stepKey, _model.RoomKey, _model.EquipmentKey, _model.OperationName, workorder, OperationParamDCType.TrackOut);   //出战时数据采集
            }
            if (dsStepParams != null || dsStepParams.Tables.Count > 0)
                dtStepParams = dsStepParams.Tables[0];
            else
                return;
            gcDate.DataSource = dtStepParams;
            //4.自动上料带出对应的物料批次号，但是没有料的情况提示"对应参数没有物料信息，请发料"
            //初始化参数列表。
            DataTable dtParam = this.gcParams.DataSource as DataTable;
            if (dtParam != null)
            {
                foreach (GridColumn col in this.gvParams.Columns)
                {
                    if (col.FieldName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
                    {
                        int index = Convert.ToInt32(col.Tag);
                        string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                        string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                        string paramName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_NAME, index);
                        for (int i = 0; i < dtParam.Rows.Count; i++)
                        {
                            string paramKey = Convert.ToString(dtParam.Rows[i][paramKeyColumnName]);
                            if (string.IsNullOrEmpty(paramKey))
                            {
                                continue;
                            }
                            string paramIndex = Convert.ToString(dtParam.Rows[i][paramIndexColumnName]);
                            DataRow drStepParamData = this._dtStepParamData
                                                          .AsEnumerable()
                                                          .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                                    && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                                          .SingleOrDefault();
                            string propertyValue = string.Empty;
                            if (drStepParamData != null)
                            {
                                //是否自动填充前一次输入的值。
                                //bool isCompletePreValue = Convert.ToBoolean(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE]);
                                //if (!isCompletePreValue)
                                //{
                                foreach(DataRow dr in dtStepParams.Rows)
                                {
                                    if (dr["PARAM_KEY"].ToString().Equals(paramKey))
                                    {
                                        propertyValue = dr["SUPPLIER_CODE"].ToString();
                                        dtParam.Rows[i][col.FieldName] = propertyValue;
                                        if(string.IsNullOrEmpty(propertyValue))
                                            ErrorMessage += "参数[" + dr["PARAM_NAME"].ToString() + "]没有对应物料!";
                                    }
                                }
                                //}
                            }
                            
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ErrorMessage))
                    ErrorMessage += "请发料后重新进站！";
            }
        }
        /// <summary>
        /// 判断是否为终检作业，发生误判情况，动态添加“返回上一步作业”，进行重新判定。
        /// </summary>
        private void IsCustCheckOperation()
        {
            if (_model.OperationName == "终检")
            {
                lblMenu.Text = "生产管理>过站管理>过站作业—终检";
                btnBack.Visible = true;
                //System.ComponentModel.ComponentResourceManager resources01 = new System.ComponentModel.ComponentResourceManager(typeof(LotDispatchDetail));
                //ToolStripButton itm = new ToolStripButton();
                //itm.Image = ((System.Drawing.Image)(resources01.GetObject("undo.Image")));
                //itm.ImageTransparentColor = System.Drawing.Color.Magenta;
                //itm.Name = "tsbBack";
                //itm.Size = new System.Drawing.Size(52, 22);
                //itm.Text = "返回上一步";
                //itm.Click += new System.EventHandler(tsbBack_Click);
                //toolStripMain.Items.Add(itm);
            }
            else
            {
                lblMenu.Text = "生产管理>过站管理>过站作业—" + this._view.TitleName;
            }
        }
        /// <summary>
        /// 返回终检作业视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbBack_Click(object sender, EventArgs e)
        {
            this._view.WorkbenchWindow.CloseWindow(false);
            //重新打开终检作业视图。
            LotDispatchForCustCheckViewContent view = new LotDispatchForCustCheckViewContent(this._model, CHECKTYPE.DATA_GROUP_ENDCHECK);
            WorkbenchSingleton.Workbench.ShowView(view);
        }
        /// <summary>
        /// 绑定原因分类。
        /// </summary>
        private void BindReasonCodeClassToDataTable()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            this._dtReasonCodeClass = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);
        }
        /// <summary>
        /// 绑定颜色信息。
        /// </summary>
        private string GetColorDisplayText(string val)
        {
            string displayText = null;
            try
            {
                if (this._dtColor == null)
                {
                    string[] columns = new string[] { "Column_Name", "Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ModelColor"));
                    this._dtColor = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtColor)
                {
                    DataRow[] drs = this._dtColor.Select(string.Format("Column_code='{0}'", val));
                    if (drs.Length > 0)
                    {
                        displayText = Convert.ToString(drs[0]["Column_Name"]);
                    }
                }
            }
            catch
            {
                displayText = null;
            }
            return displayText;
        }
        /// <summary>
        /// 绑定批次信息。
        /// </summary>
        private void BindLotInfo()
        {
            if (this._dtLotInfo == null)
            {
                LotQueryEntity queryEntity = new LotQueryEntity();
                DataSet dsLotInfo = queryEntity.GetLotInfo(this._model.LotNumber);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    dsLotInfo = null;
                    return;
                }
                if (dsLotInfo.Tables[0].Rows.Count < 1)
                {
                    MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请重试。", this._model.LotNumber));
                    dsLotInfo = null;
                    return;
                }
                this._dtLotInfo = dsLotInfo.Tables[0];
            }
            this.gcLotInfo.DataSource = this._dtLotInfo;

            lblLine.Text = this._model.LineName;
            lblWork.Text = this._model.OperationName;
            lblEquipment.Text = this._model.EquipmentName;
            this.txtLotNumber.Text = this._dtLotInfo.Rows[0]["LOT_NUMBER"].ToString();
            this.txtWorkorderNo.Text = this._dtLotInfo.Rows[0]["WORK_ORDER_NO"].ToString();
            this.txtProductID.Text = this._dtLotInfo.Rows[0]["PRO_ID"].ToString();
            this.txtRouteName.Text = this._dtLotInfo.Rows[0]["ROUTE_NAME"].ToString();
            this.txtQty.Text = this._dtLotInfo.Rows[0]["QUANTITY"].ToString();
            this.txtEffiency.Text= this._dtLotInfo.Rows[0]["EFFICIENCY"].ToString();
            this.txtSILot.Text = this._dtLotInfo.Rows[0]["SI_LOT"].ToString();
            this.txtColor.Text = this._dtLotInfo.Rows[0]["COLOR"].ToString();
            if (this._model.IsCheckSILot)
            {
                this.txtSILot.ReadOnly = false;
            }
            else
            {
                this.txtSILot.ReadOnly = true;
            }
            if (this._model.IsCheckColorByWorkOrder)
            {
                //this.lciColor.Visibility = LayoutVisibility.Always;
                txtColor.Visible = true;
            }
            else
            {
                //this.lciColor.Visibility = LayoutVisibility.Never;
                txtColor.Visible = false;
            }

            //this.lciLotInfo.SizeConstraintsType = SizeConstraintsType.Custom;
            //int height = this.gvLotInfo.RowHeight * (this._dtLotInfo.Rows.Count + 1) + this.gvLotInfo.ColumnPanelRowHeight + 10;
            //this.lciLotInfo.MinSize = new Size(this.lciLotInfo.MinSize.Width, height);
            //this.lciLotInfo.MaxSize = new Size(this.lciLotInfo.MaxSize.Width, height);
            //this.lciLotInfo.Size = new Size(this.lciLotInfo.Size.Width, height);
        }
        /// <summary>
        /// 绑定采集参数。
        /// </summary>
        private void BindParams()
        {
            if (this._model.OperationType == LotOperationType.TrackIn)
            {
                //绑定进站参数。
                BindParams(OperationParamDCType.TrackIn);
                BindWorkOrderParams(OperationParamDCType.TrackIn);
                lblMenu.Text = "过站作业_进站";
            }
            else if (this._model.OperationType == LotOperationType.TrackOut)
            {
                //绑定出站参数。
                BindParams(OperationParamDCType.TrackOut);
                BindWorkOrderParams(OperationParamDCType.TrackOut);
                lblMenu.Text = "过站作业_出站";
            }
        }
        /// <summary>
        /// 绑定工单参数设定数据。
        /// </summary>
        private void BindWorkOrderParams(OperationParamDCType dcType)
        {
            if (this._dtLotInfo == null)
            {
                return;
            }
            WorkOrders woEntity = new WorkOrders();
            string workorderKey = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
            string stepKey = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            DataSet dsWorkOrderParam = woEntity.GetWorkOrderParam(workorderKey, stepKey, dcType);
            if (!string.IsNullOrEmpty(woEntity.ErrorMsg))
            {
                MessageService.ShowError(woEntity.ErrorMsg);
                return;
            }
            //是否获取到工单参数设定数据。
            if (null == dsWorkOrderParam
                || dsWorkOrderParam.Tables.Count == 1)
            {
                return;
            }
            this._dtOrderParamSetting = dsWorkOrderParam.Tables[0];
        }
        /// <summary>
        /// 进站时绑定进站参数数据。
        /// </summary>
        private void BindParams(OperationParamDCType dcType)
        {
            if (this._dtLotInfo == null)
            {
                return;
            }
            //根据工步主键获取排列顺序和每行参数个数
            string stepKey = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
            RouteQueryEntity queryEntity = new RouteQueryEntity();
            DataSet dsStepData = queryEntity.GetStepBaseDataAndParamDataByKey(stepKey, dcType);
            if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
            {
                MessageService.ShowError(queryEntity.ErrorMsg);
                return;
            }

            //是否获取到工步基本数据及其工步参数数据。
            if (null == dsStepData
                || !dsStepData.Tables.Contains(POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME)
                || !dsStepData.Tables.Contains(POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME)
                )
            {
                return;
            }
            this._dtStepBaseData = dsStepData.Tables[POR_ROUTE_STEP_FIELDS.DATABASE_TABLE_NAME];
            this._dtStepParamData = dsStepData.Tables[POR_ROUTE_STEP_PARAM_FIELDS.DATABASE_TABLE_NAME];

            //是否获取到工步基本数据及其工步参数数据。
            if (this._dtStepBaseData.Rows.Count < 1
                || this._dtStepParamData.Rows.Count < 1
                )
            {
                return;
            }
            //根据工步主键获取绑定参数。
            int nOrderType = Convert.ToInt32(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_PARAM_ORDER_TYPE]);
            this._paramOrderType = (OperationParamOrderType)nOrderType;
            this._paramCountPerRow = Convert.ToInt32(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_PARAM_COUNT_PER_ROW]);
            //初始化采集参数控件。
            InitParamsControl(this._dtStepParamData, this._paramOrderType, this._paramCountPerRow);
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
                gcolRowNum.Tag = i;
                gcolRowNum.VisibleIndex = i * paramCountPerRow + 1;
                this.gvParams.Columns.Add(gcolRowNum);

                GridColumn gcolName = new GridColumn();
                gcolName.Caption = "参数名称";
                gcolName.FieldName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_NAME, i);
                gcolName.OptionsColumn.ReadOnly = true;
                gcolName.OptionsColumn.AllowEdit = false;
                gcolName.AppearanceCell.BackColor = System.Drawing.Color.LightGray;
                gcolName.Name = string.Format("gcolName{0}", i);
                gcolName.Visible = true;
                gcolName.Tag = i;
                gcolName.VisibleIndex = i * paramCountPerRow + 2;
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
                    //跳过参数值。
                    //drParam[nCol + 3] = dtStepParamData.Rows[nRow][POR_ROUTE_STEP_PARAM_FIELDS.FIELD_EDITOR];
                    nCol = nCol + 4;
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
        /// 绑定原因类型。
        /// </summary>
        private void BindReasonCodeType()
        {
            if (this._dtStepBaseData != null && this._dtStepBaseData.Rows.Count > 0)
            {
                DataTable dtType = new DataTable();
                dtType.Columns.Add("NAME", typeof(string));
                dtType.Columns.Add("CODE", typeof(string));
                string scrapCategoryKey = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY]);
                string defectCategoryKey = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY]);
                if (!string.IsNullOrEmpty(scrapCategoryKey))
                {
                    dtType.Rows.Add("报废", REASON_CODE_TYPE_SCRAP);
                }
                if (!string.IsNullOrEmpty(defectCategoryKey))
                {
                    dtType.Rows.Add("不良", REASON_CODE_TYPE_DEFECT);
                }
                this.rilueReasonCodeType.Columns.Clear();
                this.rilueReasonCodeType.Columns.Add(new LookUpColumnInfo("NAME", "名称"));
                this.rilueReasonCodeType.DataSource = dtType;
                this.rilueReasonCodeType.DisplayMember = "NAME";
                this.rilueReasonCodeType.ValueMember = "CODE";
            }
        }
        /// <summary>
        /// 绑定原因代码分类信息。
        /// </summary>
        private void BindReasonCodeClass(string categoryKey)
        {
            DataSet dsReturn = this._entity.GetReasonCodeClass(categoryKey);
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                return;
            }
            DataTable dtReturn = dsReturn.Tables[0];
            string rowFilterValue = "'',";
            foreach (DataRow dr in dtReturn.Rows)
            {
                string code = Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS]);
                rowFilterValue += string.Format("'{0}',", code);
            }
            rowFilterValue = string.Format(" CODE IN ({0})", rowFilterValue.TrimEnd(','));
            if (this._dtReasonCodeClass != null)
            {
                this._dtReasonCodeClass.DefaultView.RowFilter = rowFilterValue;
                this._dtReasonCodeClass.DefaultView.Sort = "CODE ASC";
                this.rilueReasonCodeClass.Columns.Clear();
                this.rilueReasonCodeClass.Columns.Add(new LookUpColumnInfo("NAME", string.Empty));
                this.rilueReasonCodeClass.DataSource = this._dtReasonCodeClass;
                this.rilueReasonCodeClass.ValueMember = "CODE";
                this.rilueReasonCodeClass.DisplayMember = "NAME";
            }
        }
        /// <summary>
        /// 绑定原因代码信息。
        /// </summary>
        private void BindReasonCode(string categoryKey, string codeClass)
        {
            DataSet dsReasonCode = this._entity.GetReasonCode(categoryKey, codeClass);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }
            this.rilueReasonCode.Columns.Clear();
            this.rilueReasonCode.Columns.Add(new LookUpColumnInfo(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME, "名称"));
            this.rilueReasonCode.DataSource = dsReasonCode.Tables[0];
            this.rilueReasonCode.DisplayMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME;
            this.rilueReasonCode.ValueMember = FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_KEY;
        }
        /// <summary>
        /// 初始化原因列表。
        /// </summary>
        private void InitReasonCodeList()
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_TRANSACTION_KEY);
            dtList.Columns.Add(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY, typeof(double));
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_KEY);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_NAME);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_ROUTE_KEY);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_ROUTE_NAME);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_STEP_KEY);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_STEP_NAME);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_DESCRIPTION);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_RESPONSIBLE_PERSON);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_EDITOR);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_EDIT_TIME);
            dtList.Columns.Add(WIP_SCRAP_FIELDS.FIELD_EDIT_TIMEZONE_KEY);
            this.gcList.MainView = this.gvList;
            this.gcList.DataSource = dtList;
        }
        /// <summary>
        /// 初始化显示备注信息。将超时原因代码绑定到窗体控件中供用户选择。
        /// </summary>
        private void InitialDelayReasonBox()
        {
            DataSet dsReturn = this._entity.GetDelayReasonCode();
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
                return;
            }
            //获取超时原因代码数据成功。
            if (dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
            {
                //绑定到备注控件上供用户选择录入备注信息。
                foreach (DataRow dr in dsReturn.Tables[0].Rows)
                {
                    this.teRemark.Properties.Items.Add(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME]);
                }
            }
        }
        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            if (this._dtStepBaseData == null
               || this._dtStepBaseData.Rows.Count < 1
               ||( string.IsNullOrEmpty(Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY]))
               && string.IsNullOrEmpty(Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY]))))
            {
                this.gcList.Visible = false;
                this.lciList.Visibility = LayoutVisibility.Never;
                this.lcgList.Visibility = LayoutVisibility.Never;
            }

            if (this._dtStepParamData == null
              || this._dtStepParamData.Rows.Count < 1)
            {
                this.gcParams.Visible = false;
                this.lciParams.Visibility = LayoutVisibility.Never;
                this.lcgParams.Visibility = LayoutVisibility.Never;
            }

               
            //初始化参数列表。
            DataTable dtParam = this.gcParams.DataSource as DataTable;
            if (dtParam != null)
            {
                foreach (GridColumn col in this.gvParams.Columns)
                {
                    if (col.FieldName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
                    {
                        int index = Convert.ToInt32(col.Tag);
                        string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                        string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                        for (int i = 0; i < dtParam.Rows.Count; i++)
                        {
                            string paramKey = Convert.ToString(dtParam.Rows[i][paramKeyColumnName]);
                            string workorder = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                            if (string.IsNullOrEmpty(paramKey))
                            {
                                continue;
                            }
                            string paramIndex = Convert.ToString(dtParam.Rows[i][paramIndexColumnName]);
                            DataRow drStepParamData = this._dtStepParamData
                                                          .AsEnumerable()
                                                          .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                                    && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                                          .SingleOrDefault();
                            string propertyValue = string.Empty;
                            if (drStepParamData != null)
                            {
                                //是否自动填充前一次输入的值。
                                bool isCompletePreValue = Convert.ToBoolean(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_COMPLETE_PREVALUE]);
                                if (isCompletePreValue)
                                {
                                    string propertyName = string.Format("{0}_{1}_{2}", this._model.OperationName, paramKey, workorder);
                                    propertyValue = PropertyService.Get(propertyName);

                                } 
                              
                            }
                            dtParam.Rows[i][col.FieldName] = propertyValue;
                        }
                    }
                }
                DataRowCollection rows = dtParam.Rows;
                DataColumnCollection cols = dtParam.Columns;
                Random r = new Random();
                //清洁工序自动赋值 
                if("清洁".Equals(this._model.OperationName)){
                    for (int i = 0; i < rows.Count; i++)
                    {
                        for (int j = 0; j < cols.Count; j++)
                        {
                            if (rows[i][j].ToString().Contains("绝缘"))
                            {
                                dtParam.Rows[i][j + 1] = "2000";
                            }
                            if (rows[i][j].ToString().Contains("接地"))
                            {
                                
                                int a = r.Next(20);
                                dtParam.Rows[i][j + 1] = a;
                            }
                            if (rows[i][j].ToString().Contains("耐压"))
                            {
                                dtParam.Rows[i][j + 1] = "0";
                            }
                        }
                    }
                }
            }

            //初始化原因列表。
            DataTable dtList = this.gcList.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
            }

            if (this._model.OperationType == LotOperationType.TrackIn)
            {
                this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN;
            }
            else
            {
                this._activity = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT;
            }
            this.teRemark.Text = string.Empty;
            if (this._model.IsCheckColorByWorkOrder)
            {
                this.gcolColor.Visible = true;
                this.gcLotInfo.Select();
                this.gvLotInfo.FocusedRowHandle = 0;
                this.gvLotInfo.FocusedColumn = this.gcolColor;
                this.gvLotInfo.ShowEditor();
            }
            //焦点跳转到参数列表的第一列第一行。
            else if (this.gvParams.DataRowCount >= 1)
            {
                this.gcParams.Select();
                this.gvParams.FocusedRowHandle = 0;
                this.gvParams.FocusedColumn = this.gvParams.Columns[2];
                this.gvParams.ShowEditor();
            }
            //焦点跳转到报废不良列表的第一列第一行。
            else if (this.gvList.DataRowCount >= 1)
            {
                this.gcList.Select();
                this.gvList.FocusedRowHandle = 0;
                this.gvList.FocusedColumn = this.gvList.Columns[1];
                this.gvList.ShowEditor();
            }
            //焦点在保存按钮上
            else
            {
                //this.toolStripMain.Select();
                //this.tsbOK.Select();
                this.btnSave.Select();
            }
            //出站时，设置显示新工艺流程的栏位，则出站时可以手工指定新的工艺流程。
            if (this._model.IsShowSetNewRoute && this._model.OperationType == LotOperationType.TrackOut)
            {
                this.lcgNewRoute.Visibility = LayoutVisibility.Always;
            }
            else
            {
                this.lcgNewRoute.Visibility = LayoutVisibility.Never;
            }
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
        /// 添加原因信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
            DataRow dr = dtList.NewRow();
            dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE] = REASON_CODE_TYPE_SCRAP;
            dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = 0;
            dtList.Rows.Add(dr);
            this.gvList.FocusedRowHandle = dtList.Rows.Count - 1;
            this.gvList.FocusedColumn = this.gclReasonCode;
            this.gvList.ShowEditor();
        }
        /// <summary>
        /// 移除原因信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = this.gvList.FocusedRowHandle;
            if (index < 0)
            {
                MessageService.ShowMessage("请选择要移除的原因信息。", MESSAGEBOX_CAPTION);
                return;
            }
            DataTable dtList = this.gcList.DataSource as DataTable;
            dtList.Rows.RemoveAt(index);
        }
        /// <summary>
        /// 自定义显示参数列表中的参数值编辑器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvParams_ShowingEditor(object sender, CancelEventArgs e)
        {
            //按回车不显示编辑器。
            if (this._bParamEnterKeyNextControl)
            {
                e.Cancel = true;
            }
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
                DataRow drStepParamData = this._dtStepParamData
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
                            edit.NullText = string.Empty;
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
                DataRow drStepParamData = this._dtStepParamData
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
                        if (dataType == AttributeDataType.DATETIME)
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
        /// 原因代码中的单元格值改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            DataTable dtList = this.gcList.DataSource as DataTable;
            string rcType = Convert.ToString(dtList.Rows[e.RowHandle][FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]);
            string reasonCodeClass = Convert.ToString(dtList.Rows[e.RowHandle][WIP_DEFECT_FIELDS.FIELD_REASON_CODE_CLASS]);
            //原因类型
            if (e.Column == this.gclReasonCodeType || e.Column == this.gclReasonCodeClass)
            {
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = 0;
            }
            else if (e.Column == this.gclReasonCode)
            {
                string rcName = Convert.ToString(this.rilueReasonCodeType.GetDisplayValueByKeyValue(rcType));

                string codeKey = Convert.ToString(dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY]);
                string codeName = Convert.ToString(this.rilueReasonCode.GetDisplayValueByKeyValue(codeKey));
                //判断原因代码在列表中是否存在。
                if (!string.IsNullOrEmpty(codeKey) && !string.IsNullOrEmpty(rcType))
                {
                    int count = dtList.AsEnumerable().Count(dr => Convert.ToString(dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY]) == codeKey
                                                                  && Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]) == rcType);
                    //原因代码在列表中存在。
                    if (count > 1)
                    {
                        MessageService.ShowMessage(string.Format("原因类型【{0}】+原因代码【{1}】已在列表中存在，请重新选择。", rcName, codeName));
                        dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = string.Empty;
                        dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = string.Empty;
                        this.gvList.FocusedColumn = e.Column;
                        this.gvList.ShowEditor();
                        return;
                    }
                }
                dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = codeName;
            }
        }
        /// <summary>
        /// 自定义显示编辑器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_ShowingEditor(object sender, CancelEventArgs e)
        {
            //按回车不显示编辑器。
            if (this._bListEnterKeyNextControl)
            {
                e.Cancel = true;
            }
            DataTable dtList = this.gcList.DataSource as DataTable;
            string rcType = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]);
            string categoryKey = string.Empty;
            if (rcType == REASON_CODE_TYPE_DEFECT) //不良
            {
                categoryKey = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY]);
            }
            else if (rcType == REASON_CODE_TYPE_SCRAP)
            {
                categoryKey = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY]);
            }
            //绑定原因分类
            if (this.gvList.FocusedColumn == this.gclReasonCodeClass && this.gvList.FocusedRowHandle >= 0)
            {
                if (!string.IsNullOrEmpty(categoryKey))
                {
                    BindReasonCodeClass(categoryKey);
                }
                else
                {
                    this.rilueReasonCodeClass.DataSource = null;
                }
            }
            //绑定原因代码。
            else if (this.gvList.FocusedColumn == this.gclReasonCode && this.gvList.FocusedRowHandle >= 0)
            {
                string codeClass = Convert.ToString(dtList.Rows[this.gvList.FocusedRowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS]);
                if (!string.IsNullOrEmpty(categoryKey) && !string.IsNullOrEmpty(codeClass))
                {
                    BindReasonCode(categoryKey, codeClass);
                }
                else
                {
                    this.rilueReasonCode.DataSource = null;
                }
            }
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            //设置序号。
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclReasonCodeType)
            {
                DataTable dtReasonCodeType = this.rilueReasonCodeType.DataSource as DataTable;
                DataRow drReasonCodeType = dtReasonCodeType.AsEnumerable()
                                                           .Where(dr => Convert.ToString(dr["CODE"]) == Convert.ToString(e.CellValue))
                                                           .SingleOrDefault();
                if (drReasonCodeType != null)
                {
                    e.DisplayText = Convert.ToString(drReasonCodeType["NAME"]);
                }
            }
            else if (e.Column == this.gclReasonCode)
            {
                DataTable dtList = this.gcList.DataSource as DataTable;
                e.DisplayText = Convert.ToString(dtList.Rows[e.RowHandle][WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME]);
            }
            else if (e.Column == this.gclReasonCodeClass && this._dtReasonCodeClass != null)
            {
                e.DisplayText = this._dtReasonCodeClass.AsEnumerable()
                                    .Where(dr => Convert.ToString(dr["CODE"]) == Convert.ToString(e.CellValue))
                                    .Select(dr => Convert.ToString(dr["NAME"]))
                                    .SingleOrDefault();
            }
        }
        /// <summary>
        /// 确认按钮事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOK_Click(object sender, EventArgs e)
        {
            if (this._dtLotInfo == null)
            {
                return;
            }
            string silot = txtSILot.Text;
            string color = txtColor.Text;
            this.gvLotInfo.FocusedRowHandle = 0;
            //this.gvLotInfo.FocusedColumn = this.gcolColor;
            //this.gvLotInfo.SetFocusedRowCellValue(this.gvLotInfo.FocusedColumn, color);
            this.gvLotInfo.FocusedColumn = this.gcolSILot;
            this.gvLotInfo.SetFocusedRowCellValue(this.gvLotInfo.FocusedColumn, silot);
            //更新批次信息。
            if (this.gvLotInfo.State == GridState.Editing && this.gvLotInfo.IsEditorFocused && this.gvLotInfo.EditingValueModified)
            {
                this.gvLotInfo.SetFocusedRowCellValue(this.gvLotInfo.FocusedColumn, this.gvLotInfo.EditingValue);
            }
            this.gvLotInfo.UpdateCurrentRow();
            //判断获取工步信息是否失败。
            if (this._dtStepBaseData == null || this._dtStepParamData == null)
            {
                MessageService.ShowMessage(string.Format("获取【{0}】信息失败，请关闭该界面重试。", this._model.OperationName), MESSAGEBOX_CAPTION);
                return;
            }
            IList<DataSet> lstTrackData = null;
            if (!CollectTrackData(out lstTrackData))
            {
                return;
            }
            //判定工厂设定是否指定需要进行上料卡控
            if (CheckFacTrueOrFalse())
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageService.ShowMessage(string.Format("错误提示：{0}", this.ErrorMessage), MESSAGEBOX_CAPTION);
                    return;
                }
                this._flag = 1;
            }
               
            //执行工作站作业。
            if (this._model.OperationType == LotOperationType.TrackIn)
            {
                int code = this._entity.LotBatchTrackIn(lstTrackData);
                if (code == 2)
                {
                    MessageService.ShowMessage("批次需要抽检。", "系统提示");
                    this.tsbClose_Click(sender, e);
                    return;
                }
                //进站成功。获取是否返回主界面的设置。
                string stepKey = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                bool bReturnMainview = this._entity.GetTrackInIsReturnMainView(stepKey);
                if (!bReturnMainview)
                {
                    this._view.WorkbenchWindow.CloseWindow(false);
                    //显示工作站作业出站明细明细界面。
                    LotQueryEntity queryEntity = new LotQueryEntity();
                    LotDispatchDetailModel model = new LotDispatchDetailModel(this._model);
                    model.OperationType = LotOperationType.TrackOut;
                    DataTable dtTrackOutLotInfo = null;
                    //重新获取批次信息。
                    foreach (DataRow dr in this._dtLotInfo.Rows)
                    {
                        string lotNo = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                        DataSet dsTrackOutLotInfo = queryEntity.GetLotInfo(lotNo);
                        if (dtTrackOutLotInfo == null)
                        {
                            dtTrackOutLotInfo = dsTrackOutLotInfo.Tables[0];
                        }
                        else
                        {
                            dtTrackOutLotInfo.Merge(dsTrackOutLotInfo.Tables[0]);
                        }
                    }
                    LotDispatchDetailViewContent view = new LotDispatchDetailViewContent(model, dtTrackOutLotInfo);
                    WorkbenchSingleton.Workbench.ShowView(view);
                    return;
                }
            }
            else if (this._model.OperationType == LotOperationType.TrackOut)
            {
                if (!CheckModuleGradeByOperation()) return;
                
                this._entity.LotBatchTrackOut(lstTrackData,dtStepParams,_flag);
            }
            lstTrackData.Clear();
            lstTrackData = null;

            //-------------Add Function For Opc--------
            //-------------Begin-----------------------
            if (_model.OperationName == "终检")
            {
                try
                {
                    bool isBool = false;
                    isBool = _lotComponentTrayEntity.InsertComponentTray(CopyModel());  //是否翻转
                    
                    if (!isBool)
                    {
                        MessageBox.Show("OPC通信失败！","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ce)
                {

                    throw ce;
                }
            }
            //-------------End-------------------------

            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowError(this._entity.ErrorMsg);
            }
            else
            {
                //是否显示打印标签的对话框。
                if (this._model.IsShowPrintLabelDialog)
                {
                    foreach (DataRow dr in this._dtLotInfo.Rows)
                    {
                        string lotNumber = Convert.ToString(dr[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                        using (LotIVTestPrintDialog frmPrintDialog = new LotIVTestPrintDialog(lotNumber))
                        {
                            frmPrintDialog.ShowDialog();
                        }
                    }
                }
                this.tsbClose_Click(sender, e);
            }


        }

        /// <summary>
        /// 临时拷贝实体类,因原有项目存在两个实体类项目集.混淆了.后续替换
        /// </summary>
        private LotCustomerModel CopyModel()
        {
            LotCustomerModel constantsModel = new LotCustomerModel();

            constantsModel.TrayText = _model.TrayText;
            constantsModel.TrayValue = _model.TrayValue;
            constantsModel.LotNumber = _model.LotNumber;
            constantsModel.LineKey = _model.LineKey;
            constantsModel.LineName = _model.LineName;
            constantsModel.PackageNumber = _model.PackageNumber;
            constantsModel.Number = _model.Number;
            constantsModel.Color = _model.Color;
            constantsModel.PsKey = _model.PsKey;
            constantsModel.SubPowerlevel = _model.SubPowerlevel;
            constantsModel.WorkOrderNo = _model.WorkOrderNo;
            constantsModel.PatrNumber = _model.PatrNumber;
            constantsModel.GradeName = _model.GradeName;
            constantsModel.VirtualCustomerNumber = _model.VirtualCustomerNumber;
            constantsModel.IsFlip = _model.IsFlip;
            constantsModel.IsPack = _model.IsPack;
            return constantsModel;


        }


        /// <summary>
        /// 校验不良原因及备注信息 true检查通过 false检查不通过。
        /// </summary>
        /// <returns></returns>
        private bool CheckModuleGradeByOperation()
        {
            bool isMustTypeinRemark = false;
            if (_model.OperationName == "终检")
            {
                string gradeName = string.Empty;
                //获取工单在终检时允许的等级。允许的等级不需要输入备注和不良。
                string checkAllowGrade = this._entity.GetWOCheckAllowGrade(this._model.LotNumber, out gradeName);
                //如果没有设置或者获取错误，则默认使用原来的方式进行判断
                //常规工单，质量等级不是A级，必须输入不良和备注。
                //非常规工单，质量等级不是客级，必须输入不良和备注。
                if (string.IsNullOrEmpty(checkAllowGrade))
                {
                    //常规客户输入的不是A级
                    if (_model.CustomerType.Equals(WORKORDER_SETTING_ATTRIBUTE.CommondCustomerType)
                        && _model.ModuleGrade != WORKORDER_SETTING_ATTRIBUTE.Grade_AJ)
                    {
                        isMustTypeinRemark = true;
                        gradeName = "A级";
                    }
                    //非常规客户输入的不是客级
                    else if (!_model.CustomerType.Equals(WORKORDER_SETTING_ATTRIBUTE.CommondCustomerType)
                        && _model.ModuleGrade != WORKORDER_SETTING_ATTRIBUTE.Grade_KJ)
                    {
                        isMustTypeinRemark = true;
                        gradeName = "客级";
                    }
                }
                else
                {
                    //工单在终检时允许的等级不包含当前组件等级，则必须输入不良和备注。
                    if (!checkAllowGrade.Contains(_model.ModuleGrade))
                    {
                        isMustTypeinRemark = true;
                    }
                }
                //不满足限制条件。必须输入报废和不良原因
                if (isMustTypeinRemark)
                {
                    if (this.gvList.RowCount < 1)
                    {
                        MessageService.ShowError(string.Format(@"批次【{0}】等级非【{1}】，请输入【报废/不良 原因】!", _model.LotNumber, gradeName));
                        return false;
                    }
                    else if (string.IsNullOrEmpty(teRemark.Text.Trim()))
                    {
                        MessageService.ShowError(string.Format(@"批次【{0}】等级非【{1}】，请输入【备注原因】!", _model.LotNumber, gradeName));
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 收集过站数据。
        /// </summary>
        /// <param name="lstTrackData">包含过站数据的数据集集合。</param>
        /// <returns>true：成功；false：失败。</returns>
        private bool CollectTrackData(out IList<DataSet> lstTrackData)
        {
            lstTrackData = new List<DataSet>();
            string shiftName = this._model.ShiftName;
            string shiftKey = string.Empty;
            //Shift shiftEntity = new Shift();
            //string shiftKey = shiftEntity.IsShiftValueExists(shiftName);//班次主键。
            ////获取班次主键失败。
            //if (!string.IsNullOrEmpty(shiftEntity.ErrorMsg))
            //{
            //    MessageService.ShowError(shiftEntity.ErrorMsg);
            //    return false;
            //}
            ////没有排班。
            //if (string.IsNullOrEmpty(shiftKey))
            //{
            //    MessageService.ShowMessage("请先在系统中进行排班。", MESSAGEBOX_CAPTION);
            //    return false;
            //}
            string oprComputer = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_NAME);
            string timezone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);

            foreach (DataRow drLotInfo in this._dtLotInfo.Rows)
            {
                string lotKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LOT_KEY]);
                double qty = Convert.ToDouble(drLotInfo[POR_LOT_FIELDS.FIELD_QUANTITY]);
                double leftQty = qty;
                string lineKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY]);
                string lineName = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_LINE_NAME]);
                string workOrderKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY]);
                string enterpriseKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                string enterpriseName = Convert.ToString(drLotInfo[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                string routeKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY]);
                string routeName = Convert.ToString(drLotInfo[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                string stepKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY]);
                int stateFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_STATE_FLAG]);
                int reworkFlag = Convert.ToInt32(drLotInfo[POR_LOT_FIELDS.FIELD_IS_REWORKED]);
                string equipmentKey = Convert.ToString(drLotInfo[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                string edcInsKey = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_EDC_INS_KEY]);
                string stepName = Convert.ToString(drLotInfo[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                string remark = this.teRemark.Text;

                DataSet dsParams = new DataSet();

                //检查进站超时
                #region 检查进站超时
                if (this._model.OperationType == LotOperationType.TrackIn
                    && this._entity.GetLotTrackInIsDelay(lotKey) == true
                    && string.IsNullOrEmpty(remark))
                {
                    MessageService.ShowMessage("进站失败，未输入超时原因。", MESSAGEBOX_CAPTION);
                    this.teRemark.Select();
                    return false;
                }
                #endregion

                //出站作业
                #region 出站作业信息
                string toEnterpriseKey = enterpriseKey;
                string toRouteKey = routeKey;
                string toStepKey = stepKey;
                string toEnterpriseName = enterpriseName;
                string toRouteName = routeName;
                string toStepName = stepName;
                bool setNewRoute = false;
                string siLot = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_SI_LOT]);
                string color = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_COLOR]);
                if (this._model.OperationType == LotOperationType.TrackOut)
                {
                    //检查电池信息，但没有输入电池片信息，则给出提示。
                    if (this._model.IsCheckSILot && string.IsNullOrEmpty(siLot))
                    {
                        MessageService.ShowMessage("请输入电池片信息。", MESSAGEBOX_CAPTION);
                        this.gcList.Select();
                        this.gvLotInfo.FocusedRowHandle = this._dtLotInfo.Rows.IndexOf(drLotInfo);
                        this.gvLotInfo.FocusedColumn = this.gcolSILot;
                        this.gvLotInfo.ShowEditor();
                        return false;
                    }
                    //如果需要根据工单检查颜色信息，且工单设置了必须检查颜色则必须输入颜色信息，则给出提示。
                    if (this._model.IsCheckColorByWorkOrder
                        && string.IsNullOrEmpty(color))
                    {
                        this._bLotEnterKeyNextControl = false;
                        MessageService.ShowMessage("请输入花色。", MESSAGEBOX_CAPTION);
                        this.gcList.Select();
                        this.gvLotInfo.FocusedRowHandle = this._dtLotInfo.Rows.IndexOf(drLotInfo);
                        this.gvLotInfo.FocusedColumn = this.gcolColor;
                        this.gvLotInfo.ShowEditor();
                        return false;
                    }
                    //获取时间控制。
                    string isTimeControl = PropertyService.Get(PROPERTY_FIELDS.TimeControl);
                    int timeControlStatus = 1;
                    double timeControlBaseSubMin = 0;
                    DataSet dsTimeControlData = this._entity.GetTrackOutTimeControlStatus(lotKey);
                    if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                    {
                        MessageService.ShowError(this._entity.ErrorMsg);
                        return false;
                    }
                    //成功获取批次时间控制状态数据。
                    if (dsTimeControlData.Tables.Count > 0 && dsTimeControlData.Tables[0].Rows.Count > 0)
                    {
                        DataRow drTimeControlData = dsTimeControlData.Tables[0].Rows[0];
                        timeControlStatus = Convert.ToInt32(drTimeControlData[TIME_CONTROL_FILEDS.TIMESTATUSFLAG]);
                        timeControlBaseSubMin = Convert.ToDouble(drTimeControlData[TIME_CONTROL_FILEDS.TIMECONTROLBASESUBMIN]);
                    }
                    //批次时间控制状态=3（超过最大加工时间） 且 备注未输入，提示需要输入超时原因。
                    if (timeControlStatus == 3 && string.IsNullOrEmpty(remark))
                    {
                        MessageService.ShowMessage("出站失败。未输入超时原因。", MESSAGEBOX_CAPTION);
                        this.teRemark.Select();
                        return false;
                    }
                    //如果时间控制状态=0（未满足最小加工时间） 且需要进行时间控制，给出提示。
                    if (timeControlStatus == 0 && isTimeControl == "1")
                    {
                        string strMessage = "";
                        strMessage = string.Format("出站失败。至少需要等待{0}分钟才能出站!", timeControlBaseSubMin);
                        MessageService.ShowMessage(strMessage, MESSAGEBOX_CAPTION);
                        return false;
                    }

                    string newEnterpriseName = this.teNewEnterpriseName.Text;
                    string newEnterpriseKey = Convert.ToString(this.teNewEnterpriseName.Tag);
                    string newRouteName = this.teNewRouteName.Text;
                    string newRouteKey = Convert.ToString(this.teNewRouteName.Tag);
                    string newStepName = this.beNewStepName.Text;
                    string newStepKey = Convert.ToString(this.beNewStepName.Tag);
                    //使用新的工艺流程作为下一个工艺流程。
                    if (this._model.IsShowSetNewRoute
                        && !string.IsNullOrEmpty(newEnterpriseKey)
                        && !string.IsNullOrEmpty(newRouteKey)
                        && !string.IsNullOrEmpty(newStepKey))
                    {
                        toEnterpriseKey = newEnterpriseKey;
                        toRouteKey = newRouteKey;
                        toStepKey = newStepKey;
                        toEnterpriseName = newEnterpriseName;
                        toRouteName = newRouteName;
                        toStepName = newStepName;
                        setNewRoute = true;
                    }
                    else
                    {
                        //获取下一个工步数据。
                        RouteQueryEntity queryEntity = new RouteQueryEntity();
                        DataSet dsRouteNextStep = queryEntity.GetEnterpriseNextRouteAndStep(enterpriseKey, routeKey, stepKey);
                        if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                        {
                            MessageService.ShowMessage(queryEntity.ErrorMsg, MESSAGEBOX_CAPTION);
                            return false;
                        }
                        if (null != dsRouteNextStep
                            && dsRouteNextStep.Tables.Count > 0
                            && dsRouteNextStep.Tables[0].Rows.Count > 0)
                        {
                            DataRow drRouteNextStep = dsRouteNextStep.Tables[0].Rows[0];
                            toEnterpriseKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY]);
                            toRouteKey = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_ROUTE_VER_KEY]);
                            toStepKey = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY]);
                            toEnterpriseName = Convert.ToString(drRouteNextStep[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME]);
                            toRouteName = Convert.ToString(drRouteNextStep[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME]);
                            toStepName = Convert.ToString(drRouteNextStep[POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME]);
                        }
                    }

                    Hashtable htStepTransaction = new Hashtable();
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE, timezone);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_KEY, toEnterpriseKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ENTERPRISE_NAME, toEnterpriseName);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_KEY, toRouteKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_ROUTE_NAME, toRouteName);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                    htStepTransaction.Add(WIP_STEP_TRANSACTION_FIELDS.FIELD_TO_STEP_NAME, toStepName);
                    DataTable dtStepTransaction = CommonUtils.ParseToDataTable(htStepTransaction);
                    dtStepTransaction.TableName = WIP_STEP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                    dsParams.Tables.Add(dtStepTransaction);
                }
                #endregion

                //工步参数信息。
                #region 工步参数信息
                DataTable dtCurrentStepParams = this.gcParams.DataSource as DataTable;
                //验证采集参数列表。
                if (this.gcParams.Visible && dtCurrentStepParams != null && dtCurrentStepParams.Rows.Count > 0)
                {
                    if (this.gvParams.State == GridState.Editing && this.gvParams.IsEditorFocused && this.gvParams.EditingValueModified)
                    {
                        this.gvParams.SetFocusedRowCellValue(this.gvParams.FocusedColumn, this.gvParams.EditingValue);
                    }
                    this.gvParams.UpdateCurrentRow();

                    if (dtCurrentStepParams != null && dtCurrentStepParams.Rows.Count >= 1)
                    {
                        //组织工步参数数据。
                        DataTable dtStepParams = CommonUtils.CreateDataTable(new WIP_PARAM_FIELDS());
                        //遍历所有列。
                        foreach (DataColumn col in dtCurrentStepParams.Columns)
                        {
                            //如果是输入值的列名。
                            if (col.ColumnName.StartsWith(WIP_PARAM_FIELDS.FIELD_PARAM_VALUE))
                            {
                                GridColumn gcol = this.gvParams.Columns[col.ColumnName];
                                int index = Convert.ToInt32(gcol.Tag); //对应每行工步第几个参数。

                                #region 遍历该列所有行
                                for (int row = 0; row < dtCurrentStepParams.Rows.Count; row++)
                                {
                                    string paramIndexColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_INDEX, index);
                                    string paramKeyColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_KEY, index);
                                    string paramNameColumnName = string.Format("{0}_{1}", WIP_PARAM_FIELDS.FIELD_PARAM_NAME, index);
                                    string paramIndex = Convert.ToString(dtCurrentStepParams.Rows[row][paramIndexColumnName]);
                                    string paramKey = Convert.ToString(dtCurrentStepParams.Rows[row][paramKeyColumnName]);
                                    string paramName = Convert.ToString(dtCurrentStepParams.Rows[row][paramNameColumnName]);
                                    string paramValue = Convert.ToString(dtCurrentStepParams.Rows[row][col]);
                                    DataRow drStepParamData = this._dtStepParamData
                                                                  .AsEnumerable()
                                                                  .Where(dr => Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX]) == paramIndex
                                                                            && Convert.ToString(dr[WIP_PARAM_FIELDS.FIELD_PARAM_KEY]) == paramKey)
                                                                  .SingleOrDefault();
                                    //如果有获取到对应行
                                    if (drStepParamData != null)
                                    {
                                        //是否必须输入。
                                        bool isMustInput = Convert.ToBoolean(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_IS_MUSTINPUT]);
                                        OperationParamValidateRule validateRule = (OperationParamValidateRule)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_RULE]);
                                        OperationParamValidateFailedRule validateFailedRule = (OperationParamValidateFailedRule)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_FAILED_RULE]);
                                        CalculateRule calculateRule = (CalculateRule)Convert.ToInt32(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_CALCULATE_RULE]);
                                        string length = Convert.ToString(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_FIELD_LENGTH]);
                                        string validateFailedMsg = Convert.ToString(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_VALIDATE_FAILED_MSG]);
                                        string mat = Convert.ToString(drStepParamData[POR_ROUTE_STEP_PARAM_FIELDS.FIELD_MAT_RULE]);
                                        //如果必须输入 且参数值为空。
                                        if (isMustInput && string.IsNullOrEmpty(paramValue))
                                        {
                                            MessageService.ShowMessage(string.Format("{0}不能为空，请确认。", paramName), MESSAGEBOX_CAPTION);
                                            this.gvParams.FocusedColumn = gcol;
                                            this.gvParams.FocusedRowHandle = row;
                                            this.gvParams.ShowEditor();
                                            return false;
                                        }
                                        //计算规则，如果设定了工单物料信息则不会卡控长度
                                        bool calculateRuleCheck = CalculateRuleCheck(calculateRule, length, paramName, paramValue);
                                        if (calculateRuleCheck == false)
                                            return false;
                                        //工单参数设置验证。
                                        DataRow drOrderParamSetting = this._dtOrderParamSetting
                                                                          .AsEnumerable()
                                                                          .Where(dr => Convert.ToString(dr[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_KEY]) == paramKey)
                                                                          .SingleOrDefault();
                                        bool check = false;
                                        if (validateRule == OperationParamValidateRule.MatchWorkOrderMat ||
                                                validateRule == OperationParamValidateRule.MatchWorkOrdeMatInContent ||
                                                    validateRule == OperationParamValidateRule.MatchWorkOrdeMatPostfix ||
                                                        validateRule == OperationParamValidateRule.MatchWorkOrdeMatPrefix)
                                            check = true;
                                        if (drOrderParamSetting != null || check == true)
                                        {
                                            string paramSettingValue = string.Empty;
                                            if (drOrderParamSetting != null)
                                            {
                                                paramSettingValue = Convert.ToString(drOrderParamSetting[POR_WORK_ORDER_ATTR_FIELDS.FIELDS_ATTRIBUTE_VALUE]);
                                            }
                                            DataSet dsParamerTeamName = this._entity.GetParamerTeamName(paramName);  //获取基础配置的参数组
                                            string paramerTeam = string.Empty;
                                            DataTable dtParamerTeamName = dsParamerTeamName.Tables[0];
                                            bool bValidateSuccess = true;
                                            DataRow[] rowBom, rowMaterial, rowSpecial;
                                            DataTable dtPorMaterial = new DataTable();
                                            DataTable dtWorkOrderBom = new DataTable();
                                            DataTable dtBarcode = new DataTable();
                                            DataTable dtWorkOrderBomParamer = new DataTable();
                                            DataTable dtSpecial = new DataTable();
                                            DataSet dsReturn = this._entity.GetWorkOrderBomByMat(workOrderKey, mat);
                                            if (dsReturn != null)
                                            {
                                                dtPorMaterial = dsReturn.Tables["PorMaterial"];   //物料代码绑定关系中对应工单的所有物料信息
                                                dtWorkOrderBom = dsReturn.Tables["WorkOrderBom"]; //工单Bom中对应工单的所有物料信息
                                            }
                                            
                                            DataSet dsWorkOrderBom = this._entity.GetWorkOrderBomByWorkKeyAndMat(workOrderKey, mat);
                                            if (dsWorkOrderBom != null)
                                            {
                                                dtBarcode = dsWorkOrderBom.Tables["PorMaterial"];   //物料代码绑定关系中对应工单的所有物料信息
                                                dtWorkOrderBomParamer = dsWorkOrderBom.Tables["WorkOrderBom"]; //工单Bom中对应工单的所有物料信息
                                            }

                                            
                                            DataSet dsSpecial = this._entity.GetSpecailBomByWorkKeyAndMat(workOrderKey, mat);
                                            if (dsSpecial != null)
                                            {
                                                dtSpecial = dsSpecial.Tables[0];   //物料代码绑定关系中对应工单的所有物料信息
                                            }
                                            int count = 0;   //计数

                                            //根据验证规则进行验证。
                                            switch (validateRule)
                                            {
                                                case OperationParamValidateRule.InContent:
                                                    bValidateSuccess = paramValue.IndexOf(paramSettingValue) > -1;
                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                    {
                                                        validateFailedMsg = string.Format("参数[{0}]的值必须包含对应工单的设定值（{1}）。", paramName, paramSettingValue);
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchContent:
                                                    bValidateSuccess = paramValue == paramSettingValue;
                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                    {
                                                        validateFailedMsg = string.Format("参数[{0}]的值必须等于对应工单的设定值（{1}）。", paramName, paramSettingValue);
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchPostfix:
                                                    bValidateSuccess = paramValue.EndsWith(paramSettingValue);
                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                    {
                                                        validateFailedMsg = string.Format("参数[{0}]的值的后缀必须和对应工单的设定值（{1}）一致。", paramName, paramSettingValue);
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchPrefix:
                                                    bValidateSuccess = paramValue.StartsWith(paramSettingValue);
                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                    {
                                                        validateFailedMsg = string.Format("参数[{0}]的值的前缀必须和对应工单的设定值（{1}）一致。", paramName, paramSettingValue);
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchWorkOrderMat:
                                                    if (drOrderParamSetting != null)
                                                    {
                                                        bValidateSuccess = paramValue == paramSettingValue;
                                                        //验证失败，并且提示消息为空，则设置默认提示消息。
                                                        if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                        {
                                                            validateFailedMsg = string.Format("参数[{0}]的值必须等于对应工单的设定值（{1}）。", paramName, paramSettingValue);
                                                        }
                                                    }
                                                    else
                                                    {

                                                        if (dtWorkOrderBom.Rows.Count == 0)
                                                        {
                                                            if (!string.IsNullOrEmpty(mat))
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到以[" + mat + "]开头的料号，请工艺查看工艺流程管理设置的参数。");
                                                            }
                                                            else
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到该组件所属工单的料号。");
                                                            }
                                                            bValidateSuccess = false;
                                                        }
                                                        else
                                                        {
                                                            if (!CheckFacSpecialTrueOrFalse())
                                                            {
                                                                rowBom = dtWorkOrderBom.Select("MATERIAL_CODE = '" + paramValue + "'");
                                                                rowMaterial = dtPorMaterial.Select("BARCODE = '" + paramValue + "'");

                                                                bValidateSuccess = (rowBom.Length + rowMaterial.Length) > 0;

                                                                //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                {
                                                                    validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}]的完全匹配！
1、请确认该物料是否存在替代料号。
2、请确认物料号或替代料号是否存在在录入的参数中！", paramName, paramValue, mat);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsParamerTeamName != null && dsParamerTeamName.Tables.Count > 0 && dsParamerTeamName.Tables[0].Rows.Count > 0)
                                                                {
                                                                    paramerTeam = dsParamerTeamName.Tables[0].Rows[0]["EDC_NAME"].ToString();
                                                                    rowBom = dtWorkOrderBomParamer.Select("MATERIAL_CODE = '" + paramValue + "' AND MATKL = '" + paramerTeam + "'");
                                                                    rowMaterial = dtBarcode.Select("BARCODE = '" + paramValue + "' AND MATKL = '" + paramerTeam + "'");
                                                                    rowSpecial = dtSpecial.Select("MATERIAL_CODE = '" + paramValue + "' AND MATKL = '" + paramerTeam + "'");

                                                                    bValidateSuccess = (rowBom.Length + rowMaterial.Length + rowSpecial.Length) > 0;
                                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                    {
                                                                        validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}],且参数组为[{2}]的完全匹配！
1、请确认该物料是否存在指定参数的替代料号。
2、请确认Bom或特殊物料管控以及替代料维护中是否存在信息！", paramName, paramValue, paramerTeam);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    bValidateSuccess = false;
                                                                    validateFailedMsg = string.Format(@"参数{0}没有设定到参数组中，请联系工艺设定！", paramName);
                                                                }

                                                            }

                                                        }
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchWorkOrdeMatPrefix:
                                                    if (drOrderParamSetting != null)
                                                    {
                                                        bValidateSuccess = paramValue.StartsWith(paramSettingValue);
                                                        //验证失败，并且提示消息为空，则设置默认提示消息。
                                                        if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                        {
                                                            validateFailedMsg = string.Format("参数[{0}]的值的前缀必须和对应工单的设定值（{1}）一致。", paramName, paramSettingValue);
                                                        }
                                                    }
                                                    else
                                                    {

                                                        if (dtWorkOrderBom.Rows.Count == 0)
                                                        {
                                                            if (!string.IsNullOrEmpty(mat))
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到以[" + mat + "]开头的料号，请工艺查看工艺流程管理设置的参数。");
                                                            }
                                                            else
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到该组件所属工单的料号。");
                                                            }
                                                            bValidateSuccess = false;
                                                        }
                                                        else //进行料号检查
                                                        {
                                                            if (!CheckFacSpecialTrueOrFalse())
                                                            {
                                                                //遍历查看工单BOM中是否有 物料号包含在 录入的参数中
                                                                foreach (DataRow dr in dtWorkOrderBom.Rows)
                                                                {
                                                                    if (paramValue.StartsWith(dr["MATERIAL_CODE"].ToString()))
                                                                    {
                                                                        count++;
                                                                    }
                                                                }
                                                                //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                foreach (DataRow dr in dtPorMaterial.Rows)
                                                                {
                                                                    if (paramValue.StartsWith(dr["BARCODE"].ToString()))
                                                                    {
                                                                        count++;
                                                                    }
                                                                }

                                                                bValidateSuccess = count > 0;
                                                                //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                {
                                                                    validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}]的前缀匹配！
1、请确认该物料是否存在替代料号。
2、请确认物料号或替代料号是否包含在录入的参数中！", paramName, paramValue, mat);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsParamerTeamName != null && dsParamerTeamName.Tables.Count > 0 && dsParamerTeamName.Tables[0].Rows.Count > 0)
                                                                {
                                                                    paramerTeam = dsParamerTeamName.Tables[0].Rows[0]["EDC_NAME"].ToString();
                                                                    foreach (DataRow dr in dtWorkOrderBomParamer.Rows)
                                                                    {
                                                                        if (paramValue.StartsWith(dr["MATERIAL_CODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtBarcode.Rows )
                                                                    {
                                                                        if (paramValue.StartsWith(dr["BARCODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtSpecial.Rows)
                                                                    {
                                                                        if (paramValue.StartsWith(dr["MATERIAL_CODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }

                                                                    bValidateSuccess = count > 0;
                                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                    {
                                                                        validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}],且参数组为[{2}]的前缀匹配！
1、请确认该物料是否存在指定参数的替代料号。
2、请确认Bom或特殊物料管控以及替代料维护中是否存在信息！", paramName, paramValue, paramerTeam);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    bValidateSuccess = false;
                                                                    validateFailedMsg = string.Format(@"参数{0}没有设定到参数组中，请联系工艺设定！", paramName);
                                                                }
                                                            }

                                                        }
                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchWorkOrdeMatPostfix:
                                                    if (drOrderParamSetting != null)
                                                    {
                                                        bValidateSuccess = paramValue.EndsWith(paramSettingValue);
                                                        //验证失败，并且提示消息为空，则设置默认提示消息。
                                                        if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                        {
                                                            validateFailedMsg = string.Format("参数[{0}]的值的后缀必须和对应工单的设定值（{1}）一致。", paramName, paramSettingValue);
                                                        }
                                                    }
                                                    else
                                                    {

                                                        if (dtWorkOrderBom.Rows.Count == 0)
                                                        {
                                                            if (!string.IsNullOrEmpty(mat))
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到以[" + mat + "]开头的料号，请工艺查看工艺流程管理设置的参数。");
                                                            }
                                                            else
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到该组件所属工单的料号。");
                                                            }
                                                            bValidateSuccess = false;
                                                        }
                                                        else
                                                        {
                                                            if (!CheckFacSpecialTrueOrFalse())
                                                            {
                                                                //遍历查看工单BOM中是否有 物料号包含在 录入的参数中
                                                                foreach (DataRow dr in dtWorkOrderBom.Rows)
                                                                {
                                                                    if (paramValue.EndsWith(dr["MATERIAL_CODE"].ToString()))
                                                                    {
                                                                        count++;
                                                                    }
                                                                }
                                                                //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                foreach (DataRow dr in dtPorMaterial.Rows)
                                                                {
                                                                    if (paramValue.EndsWith(dr["BARCODE"].ToString()))
                                                                    {
                                                                        count++;
                                                                    }
                                                                }

                                                                bValidateSuccess = count > 0;
                                                                //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                {
                                                                    validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}]的后缀匹配！
1、请确认该物料是否存在替代料号。
2、请确认物料号或替代料号是否包含在录入的参数中！", paramName, paramValue, mat);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsParamerTeamName != null && dsParamerTeamName.Tables.Count > 0 && dsParamerTeamName.Tables[0].Rows.Count > 0)
                                                                {
                                                                    paramerTeam = dsParamerTeamName.Tables[0].Rows[0]["EDC_NAME"].ToString();
                                                                    foreach (DataRow dr in dtWorkOrderBomParamer.Rows)
                                                                    {
                                                                        if (paramValue.EndsWith(dr["MATERIAL_CODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtBarcode.Rows)
                                                                    {
                                                                        if (paramValue.EndsWith(dr["BARCODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtSpecial.Rows)
                                                                    {
                                                                        if (paramValue.EndsWith(dr["MATERIAL_CODE"].ToString()) && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }

                                                                    bValidateSuccess = count > 0;
                                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                    {
                                                                        validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}],且参数组为[{2}]的后缀匹配！
1、请确认该物料是否存在指定参数的替代料号。
2、请确认Bom或特殊物料管控以及替代料维护中是否存在信息！", paramName, paramValue, paramerTeam);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    bValidateSuccess = false;
                                                                    validateFailedMsg = string.Format(@"参数{0}没有设定到参数组中，请联系工艺设定！", paramName);
                                                                }
                                                            }
                                                        }

                                                    }
                                                    break;
                                                case OperationParamValidateRule.MatchWorkOrdeMatInContent:
                                                    if (drOrderParamSetting != null)
                                                    {
                                                        bValidateSuccess = paramValue.IndexOf(paramSettingValue) > -1;
                                                        //验证失败，并且提示消息为空，则设置默认提示消息。
                                                        if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                        {
                                                            validateFailedMsg = string.Format("参数[{0}]的值必须包含对应工单的设定值（{1}）。", paramName, paramSettingValue);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dtWorkOrderBom.Rows.Count == 0)
                                                        {
                                                            if (!string.IsNullOrEmpty(mat))
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到以[" + mat + "]开头的料号，请工艺查看工艺流程管理设置的参数。");
                                                            }
                                                            else
                                                            {
                                                                validateFailedMsg = string.Format("工单Bom中没有找到该组件所属工单的料号。");
                                                            }
                                                            bValidateSuccess = false;
                                                        }
                                                        else
                                                        {
                                                            if (!CheckFacSpecialTrueOrFalse())
                                                            {
                                                                //遍历查看工单BOM中是否有 物料号包含在 录入的参数中
                                                                foreach (DataRow dr in dtWorkOrderBom.Rows)
                                                                {
                                                                    if (paramValue.IndexOf(dr["MATERIAL_CODE"].ToString()) > -1)
                                                                    {
                                                                        count++;
                                                                    }
                                                                }
                                                                //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                foreach (DataRow dr in dtPorMaterial.Rows)
                                                                {
                                                                    if (paramValue.IndexOf(dr["BARCODE"].ToString()) > -1)
                                                                    {
                                                                        count++;
                                                                    }
                                                                }

                                                                bValidateSuccess = count > 0;
                                                                //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                {
                                                                    validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}]的包含匹配！
1、请确认该物料是否存在替代料号。
2、请确认物料号或替代料号是否包含在录入的参数中！", paramName, paramValue, mat);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsParamerTeamName != null && dsParamerTeamName.Tables.Count > 0 && dsParamerTeamName.Tables[0].Rows.Count > 0)
                                                                {
                                                                    paramerTeam = dsParamerTeamName.Tables[0].Rows[0]["EDC_NAME"].ToString();
                                                                    foreach (DataRow dr in dtWorkOrderBomParamer.Rows)
                                                                    {
                                                                        if (paramValue.IndexOf(dr["MATERIAL_CODE"].ToString())>-1 && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtBarcode.Rows)
                                                                    {
                                                                        if (paramValue.IndexOf(dr["BARCODE"].ToString()) > -1 && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }
                                                                    //遍历查看 工单BOM对应的替代料是否包含在录入的参数中
                                                                    foreach (DataRow dr in dtSpecial.Rows)
                                                                    {
                                                                        if (paramValue.IndexOf(dr["MATERIAL_CODE"].ToString()) > -1 && dr["MATKL"].Equals(paramerTeam))
                                                                        {
                                                                            count++;
                                                                        }
                                                                    }

                                                                    bValidateSuccess = count > 0;
                                                                    //验证失败，并且提示消息为空，则设置默认提示消息。
                                                                    if (!bValidateSuccess && string.IsNullOrEmpty(validateFailedMsg))
                                                                    {
                                                                        validateFailedMsg = string.Format(@"[{0}]没有找到对应物料代码为[{1}],且参数组为[{2}]的包含匹配！
1、请确认该物料是否存在指定参数的替代料号。
2、请确认Bom或特殊物料管控以及替代料维护中是否存在信息！", paramName, paramValue, paramerTeam);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    bValidateSuccess = false;
                                                                    validateFailedMsg = string.Format(@"参数{0}没有设定到参数组中，请联系工艺设定！", paramName);
                                                                }
                                                            }

                                                        }
                                                    }
                                                    break;
                                                case OperationParamValidateRule.None:
                                                default:
                                                    bValidateSuccess = true;
                                                    break;
                                            }
                                            //验证失败，根据验证失败规则进行相应操作。
                                            if (!bValidateSuccess)
                                            {
                                                //判断验证规则，进行相应操作。
                                                switch (validateFailedRule)
                                                {
                                                    case OperationParamValidateFailedRule.PromptAndNoContinue:
                                                    default:
                                                        MessageService.ShowMessage(validateFailedMsg);
                                                        break;
                                                }
                                                return false;
                                            }
                                        }
                                        #region 如果参数值不为空,将记录添加到待提交的工步参数表中
                                        if (!string.IsNullOrEmpty(paramValue))
                                        {
                                            //添加一笔工步参数值。
                                            DataRow drStepParams = dtStepParams.NewRow();
                                            string workorder = Convert.ToString(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                                            dtStepParams.Rows.Add(drStepParams);
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_LOT_KEY] = lotKey;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_INDEX] = paramIndex;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_KEY] = paramKey;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_NAME] = paramName;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_PARAM_VALUE] = paramValue;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_ROUTE_NAME] = routeName;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_STEP_KEY] = stepKey;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_STEP_NAME] = stepName;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_EDITOR] = this._model.UserName;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                                            drStepParams[WIP_PARAM_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;

                                            string propertyName = string.Format("{0}_{1}_{2}", this._model.OperationName, paramKey, workorder);
                                            PropertyService.Set<string>(propertyName, paramValue);
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                        }
                        PropertyService.Save();
                        dsParams.Tables.Add(dtStepParams);
                    }
                }
                #endregion

                //原因信息。
                #region 原因信息
                DataTable dtList = this.gcList.DataSource as DataTable;
                //验证原因信息列表。
                if (this.gcList.Visible && dtList != null && dtList.Rows.Count > 0)
                {
                    if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
                    {
                        this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
                    }
                    this.gvList.UpdateCurrentRow();
                    if (dtList != null && dtList.Rows.Count >= 1)
                    {
                        //原因类型必须全部输入
                        List<DataRow> lst = (from item in dtList.AsEnumerable()
                                             where string.IsNullOrEmpty(Convert.ToString(item[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]))
                                             select item).ToList<DataRow>();
                        if (lst.Count() > 0)
                        {
                            MessageService.ShowMessage("报废/不良原因列表中的【原因类型】必须输入。", MESSAGEBOX_CAPTION);
                            this.gvList.FocusedColumn = this.gclReasonCodeType;
                            this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                            this.gvList.ShowEditor();
                            return false;
                        }
                        //报废/不良原因必须全部输入
                        lst = (from item in dtList.AsEnumerable()
                               where string.IsNullOrEmpty(Convert.ToString(item[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY]))
                               select item).ToList<DataRow>();
                        if (lst.Count() > 0)
                        {
                            MessageService.ShowMessage("报废/不良原因列表中的【原因名称】必须输入。", MESSAGEBOX_CAPTION);
                            this.gvList.FocusedColumn = this.gclReasonCode;
                            this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                            this.gvList.ShowEditor();
                            return false;
                        }
                        //数量必须输入值且大于0
                        lst = (from item in dtList.AsEnumerable()
                               where (string.IsNullOrEmpty(Convert.ToString(item[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]).Trim())
                                   || Convert.ToInt32(item[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]) <= 0)
                               select item).ToList<DataRow>();
                        if (lst.Count() > 0)
                        {
                            MessageService.ShowMessage("数量必须输入且大于0。", MESSAGEBOX_CAPTION);
                            this.gvList.FocusedColumn = this.gclQty;
                            this.gvList.FocusedRowHandle = dtList.Rows.IndexOf(lst[0]);
                            this.gvList.ShowEditor();
                            return false;
                        }
                    }
                    //报废电池片数量总和不能超过当前电池片数量
                    double scrapQty = dtList.AsEnumerable()
                                            .Where(dr => Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]) == REASON_CODE_TYPE_SCRAP
                                                   && BASEDATA_CATEGORY_NAME.Basic_ClassOfRCodeValue_Cell.IndexOf(string.Format("'{0}'", Convert.ToString(dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS]))) >= 0)
                                            .Sum(dr => Convert.ToDouble(dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY]));
                    if ((qty - scrapQty) < 0)
                    {
                        MessageService.ShowMessage("电池片报废数量不能超过当前电池片数量。", MESSAGEBOX_CAPTION);
                        return false;
                    }
                    //组织报废原因数据
                    DataTable dtScrap = CommonUtils.CreateDataTable(new WIP_SCRAP_FIELDS());
                    foreach (DataRow dr in dtList.AsEnumerable()
                                                 .Where(dr => Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]) == REASON_CODE_TYPE_SCRAP))
                    {
                        DataRow drScrap = dtScrap.NewRow();
                        dtScrap.Rows.Add(drScrap);
                        drScrap[WIP_SCRAP_FIELDS.FIELD_DESCRIPTION] = dr[WIP_SCRAP_FIELDS.FIELD_DESCRIPTION];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_RESPONSIBLE_PERSON] = dr[WIP_SCRAP_FIELDS.FIELD_RESPONSIBLE_PERSON];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_ROUTE_NAME] = routeName;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY] = dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY];
                        drScrap[WIP_SCRAP_FIELDS.FIELD_STEP_KEY] = stepKey;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_STEP_NAME] = stepName;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_EDITOR] = this._model.UserName;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                        drScrap[WIP_SCRAP_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timezone;
                    }
                    dsParams.Tables.Add(dtScrap);
                    //组织不良原因数据
                    DataTable dtDefect = CommonUtils.CreateDataTable(new WIP_DEFECT_FIELDS());
                    foreach (DataRow dr in dtList.AsEnumerable()
                                                 .Where(dr => Convert.ToString(dr[FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE]) == REASON_CODE_TYPE_DEFECT))
                    {
                        DataRow drDefect = dtDefect.NewRow();
                        dtDefect.Rows.Add(drDefect);
                        drDefect[WIP_DEFECT_FIELDS.FIELD_DESCRIPTION] = dr[WIP_SCRAP_FIELDS.FIELD_DESCRIPTION];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_ENTERPRISE_KEY] = enterpriseKey;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_ENTERPRISE_NAME] = enterpriseName;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_REASON_CODE_KEY] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_KEY];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_REASON_CODE_NAME] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_NAME];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_REASON_CODE_CLASS] = dr[WIP_SCRAP_FIELDS.FIELD_REASON_CODE_CLASS];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_RESPONSIBLE_PERSON] = dr[WIP_SCRAP_FIELDS.FIELD_RESPONSIBLE_PERSON];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_ROUTE_KEY] = routeKey;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_ROUTE_NAME] = routeName;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_DEFECT_QUANTITY] = dr[WIP_SCRAP_FIELDS.FIELD_SCRAP_QUANTITY];
                        drDefect[WIP_DEFECT_FIELDS.FIELD_STEP_KEY] = stepKey;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_STEP_NAME] = stepName;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_EDITOR] = this._model.UserName;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_EDIT_TIME] = DBNull.Value;
                        drDefect[WIP_DEFECT_FIELDS.FIELD_EDIT_TIMEZONE] = timezone;
                    }
                    dsParams.Tables.Add(dtDefect);
                }
                #endregion

                //组织操作数据。
                #region 操作数据
                Hashtable htTransaction = new Hashtable();
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, lotKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, this._activity);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, qty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_OUT, leftQty);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_KEY, enterpriseKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ENTERPRISE_NAME, enterpriseName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_KEY, routeKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ROUTE_NAME, routeName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, stepKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME, stepName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, workOrderKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, stateFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, reworkFlag);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, this._model.UserName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, oprComputer);
                //进站作业。
                if (this._model.OperationType == LotOperationType.TrackIn)
                {
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, this._model.LineKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, this._model.LineName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, this._model.EquipmentKey);
                }
                else
                {
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, lineName);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                    htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TO_STEP_KEY, toStepKey);
                }
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, lineName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDC_INS_KEY, edcInsKey);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, remark);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, this._model.UserName);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, null);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, timezone);
                htTransaction.Add(WIP_TRANSACTION_FIELDS.FIELD_TIME_STAMP, null);
                DataTable dtTransaction = CommonUtils.ParseToDataTable(htTransaction);
                dtTransaction.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtTransaction);
                #endregion

                //组织其他附加参数数据
                #region 其他附加参数数据
                string operationKey = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY]);
                string duration = Convert.ToString(this._dtStepBaseData.Rows[0][POR_ROUTE_STEP_FIELDS.FIELD_DURATION]);
                string partNumber = Convert.ToString(drLotInfo[POR_LOT_FIELDS.FIELD_PART_NUMBER]);

                Hashtable htMaindata = new Hashtable();
                htMaindata.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
                htMaindata.Add(POR_ROUTE_STEP_FIELDS.FIELD_DURATION, duration);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, drLotInfo[POR_LOT_FIELDS.FIELD_LOT_NUMBER]);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, drLotInfo[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO]);
                htMaindata.Add(POR_LOT_FIELDS.FIELD_PART_NUMBER, partNumber);
                htMaindata.Add(ROUTE_OPERATION_ATTRIBUTE.IsShowSetNewRoute, setNewRoute);
                //将电池片信息加入到出站参数中。
                if (this._model.OperationType == LotOperationType.TrackOut && this._model.IsCheckSILot)
                {
                    htMaindata.Add(POR_LOT_FIELDS.FIELD_SI_LOT, siLot);
                }
                //将花色信息加入到出站参数中。
                if (this._model.OperationType == LotOperationType.TrackOut && this._model.IsCheckColorByWorkOrder)
                {
                    htMaindata.Add(POR_LOT_FIELDS.FIELD_COLOR, color);
                }

                DataTable dtParams = CommonUtils.ParseToDataTable(htMaindata);
                //加载数据时的编辑时间。
                dtParams.ExtendedProperties.Add(COMMON_FIELDS.FIELD_COMMON_EDIT_TIME, drLotInfo[POR_LOT_FIELDS.FIELD_EDIT_TIME]);
                dtParams.TableName = TRANS_TABLES.TABLE_PARAM;
                dsParams.Tables.Add(dtParams);
                #endregion

                lstTrackData.Add(dsParams);
            }
            return true;
        }

        private bool CalculateRuleCheck(CalculateRule calculateRule, string length, string paramName, string paramValue)
        {
            int valueLength = paramValue.Length;
            //根据计算规则进行验证。
            if (!string.IsNullOrEmpty(length))
            {
                switch (calculateRule)
                {
                    case CalculateRule.None:
                        break;
                    case CalculateRule.LessThan:
                        if (valueLength >= Convert.ToInt32(length))
                        {
                            MessageBox.Show("参数[" + paramName + "]输入值长度为[" + valueLength + "]不符合工艺设定值即应小于[" + length + "]位，请重新输入");
                            return false;
                        }
                        break;
                    case CalculateRule.GreaterThan:
                        if (valueLength <= Convert.ToInt32(length))
                        {
                            MessageBox.Show("参数[" + paramName + "]输入值长度为[" + valueLength + "]不符合工艺设定值即应大于[" + length + "]位，请重新输入");
                            return false;
                        }
                        break;
                    case CalculateRule.Equal:
                        if (valueLength != Convert.ToInt32(length))
                        {
                            MessageBox.Show("参数[" + paramName + "]输入值长度为[" + valueLength + "]不符合工艺设定值即应等于[" + length + "]位，请重新输入");
                            return false;
                        }
                        break;
                    case CalculateRule.LessThanOrEqual:
                        if (valueLength > Convert.ToInt32(length))
                        {
                            MessageBox.Show("参数[" + paramName + "]输入值长度为[" + valueLength + "]不符合工艺设定值即应小于等于[" + length + "]位，请重新输入");
                            return false;
                        }
                        break;
                    case CalculateRule.GreaterThanOrEqual:
                        if (valueLength < Convert.ToInt32(length))
                        {
                            MessageBox.Show("参数[" + paramName + "]输入值长度为[" + valueLength + "]不符合工艺设定值即应大于等于[" + length + "]位，请重新输入");
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        /// <summary>
        /// 处理采集参数回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcParams_ProcessGridKey(object sender, KeyEventArgs e)
        {
            this._bParamEnterKeyNextControl = false;
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
                                this._bParamEnterKeyNextControl = true;
                                //跳转到报废不良列表的第一列第一行。
                                if (this.gvList.DataRowCount >= 1)
                                {
                                    this.gvList.FocusedRowHandle = 0;
                                    this.gvList.FocusedColumn = this.gvList.Columns[1];
                                    this.gvList.ShowEditor();
                                }
                                else
                                {
                                    //更新参数信息
                                    if (this.gvParams.State == GridState.Editing && this.gvParams.IsEditorFocused && this.gvParams.EditingValueModified)
                                    {
                                        this.gvParams.SetFocusedRowCellValue(this.gvParams.FocusedColumn, this.gvParams.EditingValue);
                                    }
                                    //this.toolStripMain.Select();
                                    //this.tsbOK.Select();
                                    this.btnSave.Select();
                                }
                                e.Handled = true;
                                return;
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
        /// 报废不良列表响应回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcList_ProcessGridKey(object sender, KeyEventArgs e)
        {
            this._bListEnterKeyNextControl = false;
            if (gvList.IsGroupRow(gvList.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter)
            {
                int rowHandle = this.gvList.FocusedRowHandle;
                int colIndex = this.gvList.FocusedColumn.AbsoluteIndex;
                if (rowHandle >= this.gvList.DataRowCount - 1 && colIndex >= this.gvList.Columns.Count - 1)
                {
                    this._bListEnterKeyNextControl = true;
                    //this.toolStripMain.Select();
                    //this.tsbOK.Select();
                    this.btnSave.Select();
                }
                else
                {
                    if (colIndex >= this.gvList.Columns.Count - 1)
                    {
                        colIndex = 0;
                        rowHandle++;
                    }
                    colIndex++;
                    this.gvList.FocusedRowHandle = rowHandle;
                    this.gvList.FocusedColumn = this.gvList.Columns[colIndex];
                    this.gvList.ShowEditor();
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// 备注信息回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teRemark_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //this.toolStripMain.Select();
                //this.tsbOK.Select();
                this.btnSave.Select();
                e.Handled = true;
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
        /// 选择工艺流程。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beNewStepName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            dlg.FactoryRoom = this._model.RoomName;
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = teNewEnterpriseName;
            dlg.RouteName = teNewRouteName;
            dlg.StepName = beNewStepName;
            int rework = Convert.ToInt32(this._dtLotInfo.Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED]);
            dlg.IsRework = rework > 0; //返修次数>0,则显示返修的工艺流程。

            Point i = beNewStepName.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + beNewStepName.Height);

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
                    dlg.Location = new Point(i.X + beNewStepName.Width - dlg.Width, i.Y + beNewStepName.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + beNewStepName.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolSeqNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gcolStateFlag)
            {
                LotStateFlag lotStateFlag = (LotStateFlag)(Convert.ToInt32(e.CellValue));
                e.DisplayText = CommonUtils.GetEnumValueDescription(lotStateFlag);
            }
            else if (e.Column == this.gcolColor)
            {
                string val = Convert.ToString(e.CellValue);
                e.DisplayText = GetColorDisplayText(val);
            }
        }
        /// <summary>
        /// 显示单元格值的编辑器。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_ShowingEditor(object sender, CancelEventArgs e)
        {
            //按回车不显示编辑器。
            if (this._bLotEnterKeyNextControl)
            {
                e.Cancel = true;
            }
            int rowHandle = this.gvLotInfo.FocusedRowHandle;
            if (rowHandle >= 0 && this.gvLotInfo.FocusedColumn == this.gcolSILot)
            {
                //不检查电池片信息,不显示编辑器。
                if (!this._model.IsCheckSILot)
                {
                    e.Cancel = true;
                    return;
                }
                DataRow dr = this.gvLotInfo.GetDataRow(rowHandle);
                string val = Convert.ToString(dr[this.gvLotInfo.FocusedColumn.FieldName]);
                //电池片信息不为空，并且行数据没有被修改过。
                //行数据被修改过说明电池片信息之前为空。
                if (!string.IsNullOrEmpty(val) && dr.RowState != DataRowState.Modified)
                {
                    e.Cancel = true;
                }
            }
            else if (rowHandle >= 0 && this.gvLotInfo.FocusedColumn == this.gcolColor)
            {
                //不检查颜色信息,不显示编辑器。
                if (!this._model.IsCheckColorByWorkOrder)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        /// <summary>
        /// 批次信息的单元格值改变事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLotInfo_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column == this.gcolColor)
            {
                string val = Convert.ToString(e.Value);
                if (string.IsNullOrEmpty(val)) return;
                string displayText = GetColorDisplayText(val);
                if (displayText == null)
                {
                    this._bLotEnterKeyNextControl = false;
                    MessageService.ShowMessage("输入的花色不存在，请确认。", MESSAGEBOX_CAPTION);
                    this.gvLotInfo.SetFocusedRowCellValue(this.gcolColor, DBNull.Value);
                    this.gvLotInfo.FocusedRowHandle = e.RowHandle;
                    this.gvLotInfo.FocusedColumn = e.Column;
                    this.gvLotInfo.ShowEditor();
                }
            }
        }
        /// <summary>
        /// 处理批次列表电池信息回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcLotInfo_ProcessGridKey(object sender, KeyEventArgs e)
        {
            this._bLotEnterKeyNextControl = false;
            if (gvLotInfo.IsGroupRow(gvLotInfo.FocusedRowHandle)) return;
            if (e.KeyCode == Keys.Enter)
            {
                int rowHandle = this.gvLotInfo.FocusedRowHandle;
                if (rowHandle >= this.gvLotInfo.DataRowCount - 1)
                {
                    string color = Convert.ToString(this.gvLotInfo.GetFocusedRowCellValue(this.gcolColor));
                    if (string.IsNullOrEmpty(color))
                    {
                        return;
                    }
                    this._bLotEnterKeyNextControl = true;
                    //跳转到参数列表的第一列第一行。
                    if (this.gvParams.DataRowCount >= 1)
                    {
                        this.gcParams.Select();
                        this.gvParams.FocusedRowHandle = 0;
                        this.gvParams.FocusedColumn = this.gvParams.Columns[2];
                        this.gvParams.ShowEditor();
                    }
                    //跳转到报废不良列表的第一列第一行。
                    else if (this.gvList.DataRowCount >= 1)
                    {
                        this.gcList.Select();
                        this.gvList.FocusedRowHandle = 0;
                        this.gvList.FocusedColumn = this.gvList.Columns[1];
                        this.gvList.ShowEditor();
                    }
                    else
                    {
                        //this.toolStripMain.Select();
                        //this.tsbOK.Select();
                        this.btnSave.Select();
                    }
                    e.Handled = true;
                    return;
                }
                else
                {
                    this.gvLotInfo.FocusedRowHandle = rowHandle + 1;
                    this.gvLotInfo.ShowEditor();
                }
                e.Handled = true;
            }
        }

        private void gvDate_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUM": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
    }
}

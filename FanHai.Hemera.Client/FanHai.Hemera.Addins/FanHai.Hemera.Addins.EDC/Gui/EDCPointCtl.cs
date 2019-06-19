using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;


using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DevExpress.XtraEditors.Controls;
using System.Linq;
using System.Linq.Expressions;
using FanHai.Hemera.Share.Common;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 显示抽检点设置界面的用户控件类。
    /// </summary>
    public partial class EDCPointCtl : BaseUserCtrl
    {

        private EdcPoint edcPoint = null;
        public new delegate void AfterStateChanged(ControlState controlState);
        public new AfterStateChanged afterStateChanged = null;
        private ControlState _ctrlState;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EDCPointCtl()
        {
            InitializeComponent();
            //this.toolbarStatus.Enabled = false;  //add by zxa 20110822
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            CtrlState = ControlState.Empty;

            lblMenu.Text = "基础数据 > 抽检管理 > 抽检点";

            GridViewHelper.SetGridView(gvEDCPoint);
            GridViewHelper.SetGridView(gvHistoryResults);
            GridViewHelper.SetGridView(gvHistoryResultRelation);
        }
        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }
        /// <summary>
        /// 控件状态改变时方法。
        /// </summary>
        /// <param name="state">控件状态。</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                case ControlState.Empty:
                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    toolbarStatus.Enabled = false;
                    this.teActionName.Text = string.Empty;
                    this.teEDCName.Text = string.Empty;
                    this.teEquipmentKey.Text = string.Empty;
                    this.teEquipmentKey.EditValue = string.Empty;
                    this.teSPName.Text = string.Empty;
                    this.tePartName.Text = string.Empty;
                    this.teOperationName.Text = string.Empty;
                    this.txtPartType.Text = string.Empty;
                    this.meDesc.Text = string.Empty;
                    this.txtRoute.Text = string.Empty;
                    this.lciIsMustInputLotNo.Visibility = LayoutVisibility.Never;
                    this.chkIsMustInputLotNo.Checked = false;
                    this.teEquipmentKey.Properties.ReadOnly = true;
                    grdEDCPoint.DataSource = null;
                    BindEquipmentData();
                    break;
                case ControlState.Edit:
                case ControlState.New:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;
                    this.teEquipmentKey.Properties.ReadOnly = false;
                    this.lciIsMustInputLotNo.Visibility = LayoutVisibility.Never;
                    this.chkIsMustInputLotNo.Checked = false;
                    BindEquipmentData();
                    break;
            }
        }
        /// <summary>
        /// 根据工序主键绑定设备数据。
        /// </summary>
        private void BindEquipmentData()
        {
            this.teEquipmentKey.Properties.Items.Clear();
            if (edcPoint == null || string.IsNullOrEmpty(edcPoint.OperationKey))
                return;
            IServerObjFactory sof = CallRemotingService.GetRemoteObject();
            DataSet dsLines = sof.CreateIOperationEquipments().GetOperationEquipments(edcPoint.OperationKey);
            if (dsLines != null && dsLines.Tables.Count > 0)
            {
                foreach (DataRow dr in dsLines.Tables[0].Rows)
                {
                    string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                    string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                    string equipmentKey = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                    string description = string.Format("{0}({1})", equipmentName, equipmentCode);
                    this.teEquipmentKey.Properties.Items.Add(equipmentKey, description);
                }
            }
            CallRemotingService.UnregisterChannel();
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (this.gvEDCPoint.State == GridState.Editing && this.gvEDCPoint.IsEditorFocused
                                                   && this.gvEDCPoint.EditingValueModified)
            {
                this.gvEDCPoint.SetFocusedRowCellValue(this.gvEDCPoint.FocusedColumn, this.gvEDCPoint.EditingValue);
            }
            this.gvEDCPoint.UpdateCurrentRow();

            if (string.IsNullOrEmpty(edcPoint.PointRowKey))
            {
                return;
            }
            //Q.001
            if (string.IsNullOrEmpty(meDesc.Text))
            {
                MessageService.ShowMessage("原因不能为空", "提示");
                return;
            }
            //Q.001
            if (edcPoint.EDIT_DESC.Equals(meDesc.Text))
            {
                MessageService.ShowMessage("没有修改原因栏位，还是原来的原因", "提示");
                return;
            }
            if (MessageService.AskQuestion("确定要保存？", "提示"))
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_EDC_POINT_ROWKEY);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_TARGET);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_FORMULA);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MIN_VALUE);
                dt.Columns.Add(EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MAX_VALUE);

                for (int i = 0; i < gvEDCPoint.RowCount; i++)
                {
                    //gridView1.GetRowCellValue(0, "");
                    string upperBoundary = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY).ToString().Trim();
                    string lowerBoundary = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY).ToString().Trim();
                    string paramCount = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT).ToString().Trim();
                    string paramType = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE).ToString().Trim();
                    string paramIndex = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX).ToString().Trim();
                    string allowMinValue = Convert.ToString(gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MIN_VALUE));
                    string allowMaxValue = Convert.ToString(gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MAX_VALUE));
                    if (upperBoundary.Length == 0)
                    {
                        MessageService.ShowMessage("参数上线值不能为空", "提示");
                        this.gvEDCPoint.FocusedRowHandle = i;
                        this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY];
                        return;
                    }
                    if (lowerBoundary.Length == 0)
                    {
                        MessageService.ShowMessage("参数下线值不能为空", "提示");
                        this.gvEDCPoint.FocusedRowHandle = i;
                        this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY];
                        return;
                    }
                    if (paramCount.Length == 0)
                    {
                        MessageService.ShowMessage("抽检片数不能为空", "提示");
                        this.gvEDCPoint.FocusedRowHandle = i;
                        this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT];
                        return;
                    }
                    if (paramType.Length == 0)
                    {
                        MessageService.ShowMessage("参数类型不能为空", "提示");
                        this.gvEDCPoint.FocusedRowHandle = i;
                        this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE];
                        return;
                    }
                    if (paramIndex.Length == 0)
                    {
                        MessageService.ShowMessage("序号不能为空", "提示");
                        this.gvEDCPoint.FocusedRowHandle = i;
                        this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX];
                        return;
                    }
                    if (!string.IsNullOrEmpty(allowMinValue) && !string.IsNullOrEmpty(allowMaxValue))
                    {
                        double nAllowMinValue = double.Parse(allowMinValue);
                        double nAllowMaxValue = double.Parse(allowMaxValue);
                        //最大值<最小值，则给出提示。
                        if (nAllowMaxValue < nAllowMinValue)
                        {
                            MessageService.ShowMessage("允许输入的最大值不能小于允许输入的最大值。", "提示");
                            this.gvEDCPoint.FocusedRowHandle = i;
                            this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_ALLOW_MAX_VALUE];
                            return;
                        }
                    }
                    dt.Rows.Add(edcPoint.PointRowKey,
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_NAME),
                        upperBoundary,
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_SPEC),
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_CONTROL),
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_TARGET),
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_CONTROL),
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_SPEC),
                        lowerBoundary,
                        paramCount,
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_KEY),
                        paramIndex,
                        paramType,
                        gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_FORMULA),
                        allowMinValue,
                        allowMaxValue
                        );
                }
                DataSet ds = new DataSet();
                dt.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
                ds.Tables.Add(dt);
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_GROUP_KEY, edcPoint.GroupKey);
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY, edcPoint.EquipmentKey);
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_EDIT_DESC, meDesc.Text);
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_GROUP_NAME, txtGroupName.Text);
                string newEquipmentKey = Convert.ToString(this.teEquipmentKey.EditValue).Replace(" ", "");
                ds.ExtendedProperties.Add("NEW_EQUIPMENT_KEY", newEquipmentKey);
                EDCPointMustInputField field = EDCPointMustInputField.None;
                if (this.chkIsMustInputLotNo.Checked)
                {
                    field = EDCPointMustInputField.LotNo;
                }
                ds.ExtendedProperties.Add(EDC_POINT_FIELDS.FIELD_MUST_INPUT_FIELD, (int)field);

                if (edcPoint.UpdateEDCPointParams(ds))
                {
                    edcPoint.EquipmentKey = newEquipmentKey;
                    MessageService.ShowMessage("保存成功", "提示");
                }
                else
                {
                    MessageService.ShowMessage("保存失败：" + edcPoint.ErrorMsg, "提示");
                }
            }

        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void EDCPointCtl_Load(object sender, EventArgs e)
        {
            DataTable dt = BaseData.Get(new string[] { "CODE", "NAME", "DESCRIPTION" },
                                      new KeyValuePair<string, string>("CATEGORY_NAME", "EDC_PARAM_TYPE"));
            lueParamType.DataSource = dt;
            lueParamType.TextEditStyle = TextEditStyles.DisableTextEditor;
            lueParamType.ValueMember = "CODE";
            lueParamType.DisplayMember = "NAME";
        }
        /// <summary>
        /// 查询按钮Click事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            //EDCPointSearch eps = new EDCPointSearch();
            //eps.Width = this.Width;
            //eps.Height = this.Height;
            //if (DialogResult.OK == eps.ShowDialog())
            //{
            //    edcPoint = eps.edcPoint;
            //    CtrlState = ControlState.Edit;
            //    EntityStatus status = (EntityStatus)Enum.Parse(typeof(EntityStatus), eps.edcPoint.PointState);
            //    if (status != EntityStatus.InActive)
            //    {
            //        this.toolbarDelete.Enabled = false;
            //    }
            //    BindData(edcPoint);
            //}
            edcPoint = new EdcPoint();
            DataSet dsPoint = new DataSet();
            edcPoint.PartName = this.tePartName.Text.Trim();
            edcPoint.OperationName = this.teOpName.Text.Trim();
            dsPoint = edcPoint.SearchEdcPoint();
            //判断返回的结果若成功则绑定在视图上，若失败则弹出对话框进行提示
            if (edcPoint.ErrorMsg == string.Empty)
            {
                if (dsPoint.Tables.Count > 0)
                {
                    EDCPionts.MainView = gridViewEdc;
                    EDCPionts.DataSource = dsPoint.Tables[0];
                    gridViewEdc.BestFitColumns();
                }
            }
            else
            {
                MessageService.ShowError(edcPoint.ErrorMsg);
            }
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            NewEdcPoint newPoint = new NewEdcPoint();
            if (DialogResult.OK == newPoint.ShowDialog())
            {
                edcPoint = newPoint.edcPoint;
                CtrlState = ControlState.New;
                BindData(edcPoint);
            }
        }
        /// <summary>
        /// 绑定抽检点参数数据。
        /// </summary>
        /// <param name="point">抽检点对象。</param>
        private void BindData(EdcPoint point)
        {
            this.meDesc.Text = point.EDIT_DESC;
            this.teActionName.Text = point.ActionName;
            this.teEDCName.Text = point.EdcName;
            this.txtGroupName.Text = point.GroupName;
            //this.teEquipmentKey.EditValue = edcPoint.EquipmentKey;
            //this.teEquipmentKey.Text = point.EquipmentName;
            string[] equipmnetKeys = edcPoint.EquipmentKey.Split(',');
            foreach (CheckedListBoxItem item in this.teEquipmentKey.Properties.Items)
            {
                foreach (string equipmentKey in equipmnetKeys)
                {
                    if (equipmentKey.Trim().Equals(item.Value.ToString().Trim()))
                    {
                        item.CheckState = CheckState.Checked;
                        break;
                    }
                }
            }


            this.teSPName.Text = point.SpName;
            this.tePartName.Text = point.PartName;
            this.teOperationName.Text = point.OperationName;
            this.txtPartType.Text = point.PartType;
            this.txtRoute.Text = point.RouteName;

            if (point.ActionName == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE)
            {
                this.lciIsMustInputLotNo.Visibility = LayoutVisibility.Always;
                bool bChecked = ((edcPoint.MustInputField & EDCPointMustInputField.LotNo) == EDCPointMustInputField.LotNo);
                this.chkIsMustInputLotNo.Checked = bChecked;
            }

            DataSet dsParam = point.GetEdcPointParams();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = paginationControl1.PageNo,
                PageSize = paginationControl1.PageSize
            }; //Q.002
            DataSet dsHis = point.GetEdcPointParamsTrans(ref config);//Q.002
            if (point.ErrorMsg == string.Empty)
            {
                if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
                {
                    grdEDCPoint.DataSource = dsParam.Tables[0];
                    grdEDCPoint.MainView = gvEDCPoint;
                    //Q.001  //Q.002
                    if (dsHis.Relations.Count > 0)
                    {
                        paginationControl1.Pages = config.Pages;
                        paginationControl1.Records = config.Records;
                        this.gcHistoryResults.DataSource = dsHis.Relations[0].ParentTable;
                    }//Q.002
                }
            }
            else
            {
                MessageService.ShowError(point.ErrorMsg);
            }
        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(edcPoint.GroupKey))
            {
                if (MessageService.AskQuestion("确定要删除吗？", "提示"))
                {
                    if (edcPoint.DeleteEdcPoint(edcPoint.GroupKey))
                    {
                        CtrlState = ControlState.Empty;
                        MessageService.ShowMessage("删除成功", "提示");
                    }
                    else
                    {
                        MessageService.ShowError("删除出错" + edcPoint.ErrorMsg);
                    }
                }
            }
            else
            {
                MessageService.ShowMessage("不能删除，删除失败。", "提示");
            }
        }
        /// <summary>
        /// 状态按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            if (this.gvEDCPoint.State == GridState.Editing && this.gvEDCPoint.IsEditorFocused
                                                  && this.gvEDCPoint.EditingValueModified)
            {
                this.gvEDCPoint.SetFocusedRowCellValue(this.gvEDCPoint.FocusedColumn, this.gvEDCPoint.EditingValue);
            }
            if (string.IsNullOrEmpty(edcPoint.PointRowKey))
            {
                return;
            }
            for (int i = 0; i < gvEDCPoint.RowCount; i++)
            {
                string upperBoundary = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY).ToString().Trim();
                string lowerBoundary = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY).ToString().Trim();
                string paramCount = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT).ToString().Trim();
                string paramType = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE).ToString().Trim();
                string paramIndex = gvEDCPoint.GetRowCellValue(i, EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX).ToString().Trim();
                if (upperBoundary.Length == 0)
                {
                    MessageService.ShowMessage("参数上线值不能为空", "提示");
                    this.gvEDCPoint.FocusedRowHandle = i;
                    this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_UPPER_BOUNDARY];
                    return;
                }
                if (lowerBoundary.Length == 0)
                {
                    MessageService.ShowMessage("参数下线值不能为空", "提示");
                    this.gvEDCPoint.FocusedRowHandle = i;
                    this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_LOWER_BOUNDARY];
                    return;
                }
                if (paramCount.Length == 0)
                {
                    MessageService.ShowMessage("抽检片数不能为空", "提示");
                    this.gvEDCPoint.FocusedRowHandle = i;
                    this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_COUNT];
                    return;
                }
                if (paramType.Length == 0)
                {
                    MessageService.ShowMessage("参数类型不能为空", "提示");
                    this.gvEDCPoint.FocusedRowHandle = i;
                    this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_TYPE];
                    return;
                }
                if (paramIndex.Length == 0)
                {
                    MessageService.ShowMessage("序号不能为空", "提示");
                    this.gvEDCPoint.FocusedRowHandle = i;
                    this.gvEDCPoint.FocusedColumn = this.gvEDCPoint.Columns[EDC_POINT_PARAMS_FIELDS.FIELD_PARAM_INDEX];
                    return;
                }
            }

            EntityObject pointObject = new EntityObject();

            if (CtrlState == ControlState.New)
            {
                pointObject.Status = EntityStatus.InActive;
            }
            if (CtrlState == ControlState.Edit)
            {
                string pointState = edcPoint.PointState;
                switch (pointState)
                {
                    case "0":
                        pointObject.Status = EntityStatus.InActive;
                        break;
                    case "1":
                        pointObject.Status = EntityStatus.Active;
                        break;
                    case "2":
                        pointObject.Status = EntityStatus.Archive;
                        break;
                    default:
                        pointObject.Status = EntityStatus.InActive;
                        break;
                }
            }
            EntityStatus oldStatus = pointObject.Status;
            StatusDialog status = new StatusDialog(pointObject);
            status.ShowDialog();


            if (pointObject.Status != oldStatus)
            {
                string newPointStatus = string.Empty;
                newPointStatus = (Convert.ToInt32(pointObject.Status)).ToString();

                if (newPointStatus != "1")
                {
                    if (edcPoint.UpdateEDCPointStatus(edcPoint.GroupKey, newPointStatus))
                    {
                        edcPoint.PointState = newPointStatus;
                        MessageService.ShowMessage("状态修改成功", "提示");

                    }
                    else
                    {
                        MessageService.ShowError("状态修改失败");
                    }
                }
                else
                {
                    DataSet dsReturn = new DataSet();
                    bool bExist = edcPoint.FindExistUsedEDCPoint(edcPoint.GroupKey);
                    if (bExist)
                    {
                        MessageService.ShowError("存在可用的抽检设置，状态修改失败。");
                        return;
                    }
                    else
                    {
                        if (edcPoint.UpdateEDCPointStatus(edcPoint.GroupKey, newPointStatus))
                        {
                            edcPoint.PointState = newPointStatus;
                            MessageService.ShowMessage("状态修改成功", "提示");
                        }
                        else
                        {
                            MessageService.ShowError("状态修改失败");
                        }

                    }
                }
                if (edcPoint.PointState == "1")
                {
                    this.toolbarDelete.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 接口方法的实现
        /// </summary>
        /// Q.001
        private void paginationControl1_DataPaging()
        {
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = paginationControl1.PageNo,
                PageSize = paginationControl1.PageSize
            };
            DataSet dsHis = edcPoint.GetEdcPointParamsTrans(ref config);
            if (dsHis.Relations.Count > 0)
            {
                paginationControl1.Pages = config.Pages;
                paginationControl1.Records = config.Records;
                this.gcHistoryResults.DataSource = dsHis.Relations[0].ParentTable;
            }
        }

        private void gvHistoryResultRelation_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvEDCPoint_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvHistoryResults_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridViewEdc_DoubleClick(object sender, EventArgs e)
        {

            if (MapSelectedItemToProperties())
            {
                CtrlState = ControlState.Edit;
                EntityStatus status = (EntityStatus)Enum.Parse(typeof(EntityStatus), edcPoint.PointState);
                if (status != EntityStatus.InActive)
                {
                    this.toolbarDelete.Enabled = false;
                }
                BindData(edcPoint);
            }
        }


        /// <summary>
        /// 获取选择行的数据信息
        /// </summary>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridViewEdc.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                edcPoint.PointRowKey = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_ROW_KEY).ToString();
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_TOPRODUCT) != null)
                {
                    edcPoint.PartName = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_TOPRODUCT).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_OPERATION_NAME) != null)
                {
                    edcPoint.OperationName = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_OPERATION_NAME).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, "OPERATION_KEY") != null)
                {
                    edcPoint.OperationKey = gridViewEdc.GetRowCellValue(rowHandle, "OPERATION_KEY").ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME) != null)
                {
                    edcPoint.EquipmentName = gridViewEdc.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY) != null)
                {
                    edcPoint.EquipmentKey = gridViewEdc.GetRowCellValue(rowHandle, EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY).ToString().Replace(" ", "");
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_NAME) != null)
                {
                    edcPoint.EdcName = gridViewEdc.GetRowCellValue(rowHandle, EDC_MAIN_FIELDS.FIELD_EDC_NAME).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_NAME) != null)
                {
                    edcPoint.SpName = gridViewEdc.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_NAME).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_ACTION_NAME) != null)
                {
                    edcPoint.ActionName = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_ACTION_NAME).ToString();
                }
                //add by zxa
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_POINT_STATUS) != null)
                {
                    edcPoint.PointState = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_POINT_STATUS).ToString();
                }
                //add by zxa
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_PART_TYPE) != null)
                {
                    edcPoint.PartType = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_PART_TYPE).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_GROUP_KEY) != null)
                {
                    edcPoint.GroupKey = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_GROUP_KEY).ToString();
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_GROUP_NAME) != null)
                {
                    edcPoint.GroupName = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_GROUP_NAME).ToString();
                }
                edcPoint.RouteName = Convert.ToString(gridViewEdc.GetRowCellValue(rowHandle, POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME));
                //  Q.001
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_EDIT_DESC) != null)
                {
                    edcPoint.EDIT_DESC = gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_EDIT_DESC).ToString();
                }
                else
                {
                    edcPoint.EDIT_DESC = "";
                }
                if (gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_MUST_INPUT_FIELD) != null)
                {
                    string field = Convert.ToString(gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_MUST_INPUT_FIELD));
                    field = string.IsNullOrEmpty(field) ? "0" : field;
                    edcPoint.MustInputField = (EDCPointMustInputField)Enum.Parse(typeof(EDCPointMustInputField), field);
                }

                return true;
            }
            return false;
        }
    }

}

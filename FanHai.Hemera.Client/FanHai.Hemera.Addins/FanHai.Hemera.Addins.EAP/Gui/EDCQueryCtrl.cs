#region using
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraLayout.Utils;
using System.Collections;
using FanHai.Hemera.Share.Interface;
#endregion

namespace FanHai.Hemera.Addins.EAP
{
    /// <summary>
    /// 表示设备数据采集的主控件类。用于输入设备数据采集的批次号、工单号、线别以及操作的员工号等信息。
    /// </summary>
    public partial class EDCQueryCtrl : BaseUserCtrl
    {
        bool bIsOnLoading = true;                                               //是否是正在加载中。
        EdcGatherData _edcData = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// comment by peter 2012-2-23
        public EDCQueryCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入事件方法。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        ///  comment by peter 2012-2-23
        private void EDCMainCtrl_Load(object sender, EventArgs e)
        {
            bIsOnLoading = true;
            BindFactoryRoom();
            BindOperations();
            BindEquipment();
            BindPartType();
            BindEDCItem();
            bIsOnLoading = false;
            HideControl();
            this.deStartTime.DateTime = DateTime.Now.AddDays(-1);
            this.deEndTime.DateTime = DateTime.Now;
        }
        /// <summary>
        /// 隐藏控件。
        /// </summary>
        private void HideControl()
        {
            if (this.lueFactoryRoom.Properties.DataSource != null)
            {
                DataTable dt = this.lueFactoryRoom.Properties.DataSource as DataTable;
                if (dt != null && dt.Rows.Count == 1)
                {
                    this.lciFactoryRoom.Visibility = LayoutVisibility.Never;
                }
            }
            if (this.cbOperation.Properties.Items.Count <= 1)
            {
                this.cbOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 绑定成品类型数据。
        /// </summary>
        private void BindPartType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "PART_TYPE");
            DataTable lotTypeTable = BaseData.Get(columns, category);
            lotTypeTable.Rows.InsertAt(lotTypeTable.NewRow(), 0);
            this.luePartType.Properties.DataSource = lotTypeTable;
            this.luePartType.Properties.DisplayMember = "NAME";
            this.luePartType.Properties.ValueMember = "CODE";
            this.luePartType.ItemIndex = 0;
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
        }
        /// <summary>
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cbOperation.Properties.Items.Add(strOperations[i]);
                }
                this.cbOperation.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 绑定设备。
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text;
            string strFactoryRoomName = this.lueFactoryRoom.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            //如果工厂车间或者工序或者线别主键为空。
            if (string.IsNullOrEmpty(strFactoryRoomName)
                || string.IsNullOrEmpty(strOperation)
                || string.IsNullOrEmpty(strLines))
            {
                return;
            }
            this.lueEquipment.EditValue = string.Empty;
            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, string.Empty);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(), 0);
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = "EQUIPMENT_NAME";
                this.lueEquipment.Properties.ValueMember = "EQUIPMENT_KEY";
                this.lueEquipment.ItemIndex = 0;
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 绑定采集项目。
        /// </summary>
        private void BindEDCItem()
        {
            string strOperation = this.cbOperation.Text;
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue == null ? string.Empty : this.lueFactoryRoom.EditValue.ToString();
            string equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            string partType = Convert.ToString(this.luePartType.EditValue);

            this.lueEDCItem.EditValue = string.Empty;
            string strParam = string.Empty;
            EdcPoint entity = new EdcPoint();
            DataSet ds = entity.GetEDCPoint(strFactoryRoomKey, strOperation, partType, equipmentKey);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(), 0);
                this.lueEDCItem.Properties.DataSource = ds.Tables[0];
                this.lueEDCItem.Properties.DisplayMember = "EDC_NAME";
                this.lueEDCItem.Properties.ValueMember = "EDC_KEY";//EDCPoint主键。
            }
        }
        /// <summary>
        /// EDC项目改变后修改数据。
        /// </summary>
        private void SetEDCChangedData()
        {
            string partNo = Convert.ToString(this.lueEDCItem.GetColumnValue("TOPRODUCT"));
            string equipmentKey = Convert.ToString(this.lueEDCItem.GetColumnValue("EQUIPMENT_KEY"));
            string partType = Convert.ToString(this.lueEDCItem.GetColumnValue("PART_TYPE"));
            if (string.IsNullOrEmpty(Convert.ToString(this.lueEquipment.EditValue)))
            {
                this.lueEquipment.EditValue = equipmentKey;
            }
            if (partType.CompareTo((Convert.ToString(this.luePartType.EditValue))) != 0)
            {
                this.luePartType.EditValue = partType;
            }
        }
        /// <summary>
        /// 工厂车间改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            //重新绑定设备控件
            BindEquipment();
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 设备改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 工序改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            //重新绑定设备控件
            BindEquipment();
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 关闭当前视图界面。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 采集项目改变。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueEDCItem_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            SetEDCChangedData();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 成品类型改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPartType_EditValueChanged(object sender, EventArgs e)
        {
            if (bIsOnLoading) return;
            bIsOnLoading = true;
            BindEDCItem();
            bIsOnLoading = false;
        }
        /// <summary>
        /// 查询按钮Click事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string factoryRoomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
                if (string.IsNullOrEmpty(factoryRoomKey))
                {
                    MessageService.ShowMessage("请选择车间。", "提示");
                    this.lueFactoryRoom.Select();
                    return;
                }
                string stepName = this.cbOperation.Text.Trim();
                string equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
                string partType = Convert.ToString(this.luePartType.EditValue);
                string edcKey = Convert.ToString(this.lueEDCItem.EditValue);
                string lotNo = this.txtLotNo.Text.Trim();
                string startTime = Convert.ToString(this.deStartTime.EditValue);
                string endTime = Convert.ToString(this.deEndTime.EditValue);

                if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
                {
                    if (this.deEndTime.DateTime < this.deStartTime.DateTime)
                    {
                        MessageService.ShowMessage("结束时间不能小于开始时间。", "提示");
                        this.deEndTime.Select();
                        return;
                    }
                }
                LotEDCEntity entity = new LotEDCEntity();
                Hashtable htParmas = new Hashtable();
                //工厂车间
                if (!string.IsNullOrEmpty(factoryRoomKey))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY, factoryRoomKey);
                }
                //工序名称
                if (!string.IsNullOrEmpty(stepName))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME, stepName);
                }
                //设备主键
                if (!string.IsNullOrEmpty(equipmentKey))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                }
                //产品类型
                if (!string.IsNullOrEmpty(partType))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE, partType);
                }
                //采集项目主键。
                if (!string.IsNullOrEmpty(edcKey))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY, edcKey);
                }
                //批次号
                if (!string.IsNullOrEmpty(lotNo))
                {
                    htParmas.Add(EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER, lotNo);
                }
                string startTime0 = EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME + "_START";
                string startTime1 = EDC_MAIN_INS_FIELDS.FIELD_COL_START_TIME + "_END";
                //开始时间
                if (!string.IsNullOrEmpty(startTime))
                {
                    htParmas.Add(startTime0, this.deStartTime.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                //结束时间
                if (!string.IsNullOrEmpty(endTime))
                {
                    htParmas.Add(startTime1, this.deEndTime.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                DataTable dtParams = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(htParmas);
                PagingQueryConfig config = new PagingQueryConfig()
                {
                    PageNo = pgnQueryResult.PageNo,
                    PageSize = pgnQueryResult.PageSize
                };
                DataSet dsReturn = entity.QueryEDCData(dtParams, ref config);
                pgnQueryResult.Pages = config.Pages;
                pgnQueryResult.Records = config.Records;

                if (!string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    MessageService.ShowMessage(entity.ErrorMsg);
                    return;
                }
                gcResults.DataSource = dsReturn.Tables[0];
                gcResults.MainView = gvResults;
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message, "错误");
            }
        }
        /// <summary>
        /// 自定义绘制单元格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResults_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if ("INDEX" == e.Column.FieldName)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            btnQuery_Click(null, null);
        }
        /// <summary>
        /// 双击采集的数据行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResults_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = this.gvResults.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            DataRow dr = this.gvResults.GetFocusedDataRow();
            //获取当前班别名称。
            Shift _shift = new Shift();
            string defaultShift = _shift.GetCurrShiftName();
            _edcData = new EdcGatherData();
            _edcData.Operator = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            _edcData.LotNumber = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_LOT_NUMBER]);
            _edcData.EDCKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EDC_KEY]);
            _edcData.LineName = string.Empty;
            _edcData.OperationName = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME]);
            _edcData.EquipmentKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EQUIPMENT_KEY]);
            _edcData.MaterialLot = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_MATERIAL_LOT]);
            _edcData.OrderNumber = Convert.ToString(dr[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER]);
            _edcData.PartNumber = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_PART_NO]);
            _edcData.SupplierName = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_SUPPLIER]);
            _edcData.ShiftName = defaultShift;
            _edcData.EquipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
            _edcData.EDCName = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EDC_NAME]);
            _edcData.FactoryRoomName = Convert.ToString(dr[FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME]);
            _edcData.FactoryRoomKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_LOCATION_KEY]);
            _edcData.EDCActionName = Convert.ToString(dr[EDC_POINT_FIELDS.FIELD_ACTION_NAME]);
            _edcData.EDCPointKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EDC_POINT_KEY]);
            _edcData.EDCSPKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EDC_SP_KEY]);
            _edcData.PartType = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_PART_TYPE]);
            _edcData.EDCMainInsKey = Convert.ToString(dr[EDC_MAIN_INS_FIELDS.FIELD_EDC_INS_KEY]);
            //遍历工作台中的视图对象。
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == "数据采集明细")
                {
                    viewContent.WorkbenchWindow.CloseWindow(true);
                    break;
                }
            }
            //创建新的视图对象并显示。
            EDCData04WViewContent view = new EDCData04WViewContent(_edcData);
            WorkbenchSingleton.Workbench.ShowView(view);
        }


    }
}


#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Share.Common;
#endregion

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 新增抽检点的对话框
    /// </summary>
    public partial class NewEdcPoint : BaseDialog
    {
        public EdcPoint edcPoint = null;
        public NewEdcPoint()
            : base("新增抽检点")
        {
            InitializeComponent();            
        }

        private void NewEdcPoint_Load(object sender, EventArgs e)
        {
            //UnregisterChannel
            CallRemotingService.UnregisterChannel();
            //get server object factory
            IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
            //物料编号信息进行数据绑定
            BindPartNameToControl(iServerObjFactory);
            //抽检规则信息进行数据绑定
            BindSPNameToControl(iServerObjFactory);
            //EDC名称信息进行数据绑定
            BindEDCNameToControl(iServerObjFactory);
            //绑定操作名称到控件
            BindActionNameToControl();
            CallRemotingService.UnregisterChannel();
            BindPartType();
            BindRouteData();
            BindOperationName(string.Empty);
        }
        /// <summary>
        /// 绑定操作名称到控件
        /// </summary>
        private void BindActionNameToControl()
        {
            this.cmbActionName.Properties.Items.Add(ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT);
            this.cmbActionName.Properties.Items.Add(ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_NONE);
            this.cmbActionName.Text = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT;
        }
        /// <summary>
        /// 绑定成品类型。
        /// </summary>
        private void BindPartType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "PART_TYPE");
            //返回3列ITEM_ORDER,CODE,NAME
            DataTable lotTypeTable = BaseData.Get(columns, category);
            lotTypeTable.Rows.InsertAt(lotTypeTable.NewRow(),0);
            this.luePartType.Properties.DataSource = lotTypeTable;
            this.luePartType.Properties.DisplayMember = "NAME";
            this.luePartType.Properties.ValueMember = "CODE";
            this.luePartType.ItemIndex = 0;
        }
        /// <summary>
        /// 物料编号信息进行数据绑定
        /// </summary>
        private void BindPartNameToControl(IServerObjFactory iServerObjFactory)
        {
            DataSet dsLines = iServerObjFactory.CreateIEDCEngine().GetPOR_PART();
            if (dsLines != null && dsLines.Tables.Count > 0)
            {
                dsLines.Tables[0].Rows.InsertAt(dsLines.Tables[0].NewRow(),0);
                luePartName.Properties.DataSource = dsLines.Tables[0];
                luePartName.Properties.DisplayMember = POR_PART_FIELDS.FIELD_PART_NAME;
                luePartName.Properties.ValueMember = POR_PART_FIELDS.FIELD_PART_KEY;
               
                if (dsLines.Tables[0].Rows.Count > 0)
                {
                    luePartName.ItemIndex = 0;
                }
            }
        }
        /// <summary>
        /// 绑定工序名称。
        /// </summary>
        private void BindOperationName(string routeKey)
        {
            if (string.IsNullOrEmpty(routeKey))
            {
                IServerObjFactory iServerObjFactory = CallRemotingService.GetRemoteObject();
                DataSet ds = iServerObjFactory.CreateIEDCEngine().GetPOR_ROUTE_OPERATION_VER();
                if (ds != null && ds.Tables.Count > 0)
                {
                    lueOperationName.Properties.DataSource = ds.Tables[0];
                    lueOperationName.Properties.DisplayMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
                    lueOperationName.Properties.ValueMember = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
                    this.lueOperationName.Properties.Columns[0].FieldName = POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lueOperationName.ItemIndex = 0;
                    }
                }
                else
                {
                    this.lueOperationName.Properties.DataSource = null;
                }
                CallRemotingService.UnregisterChannel();
            }
            else
            {
                RouteQueryEntity entity = new RouteQueryEntity();
                DataSet ds = entity.GetRouteStepDataByRouteKey(routeKey);
                if (string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    this.lueOperationName.Properties.DataSource = ds.Tables[0];
                    this.lueOperationName.Properties.ValueMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY;
                    this.lueOperationName.Properties.DisplayMember = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME;
                    this.lueOperationName.Properties.Columns[0].FieldName = POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_NAME;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.lueOperationName.ItemIndex = 0;
                    }
                }
                else
                {
                    this.lueOperationName.Properties.DataSource = null;
                }
            }
        }
        /// <summary>
        /// 绑定工艺流程数据。
        /// </summary>
        private void BindRouteData()
        {
            RouteQueryEntity entity = new RouteQueryEntity();
            DataSet ds=entity.GetActivedRouteData();
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                ds.Tables[0].Rows.InsertAt(ds.Tables[0].NewRow(),0);
                this.lueRoute.Properties.DataSource = ds.Tables[0];
                this.lueRoute.Properties.ValueMember = "ROUTE_ROUTE_VER_KEY";
                this.lueRoute.Properties.DisplayMember = "ROUTE_NAME";
            }
        }
        /// <summary>
        /// 抽检规则信息进行数据绑定
        /// </summary>
        private void BindSPNameToControl(IServerObjFactory iServerObjFactory)
        {
            DataSet dsLines = iServerObjFactory.CreateIEDCEngine().GetEDC_SP();
            if (dsLines != null &&　dsLines.Tables.Count > 0)
            {
                lueSPName.Properties.DataSource = dsLines.Tables[0];
                lueSPName.Properties.DisplayMember = EDC_SP_FIELDS.FIELD_SP_NAME;
                lueSPName.Properties.ValueMember = EDC_SP_FIELDS.FIELD_SP_KEY;
                if (dsLines.Tables[0].Rows.Count > 0)
                {
                    lueSPName.ItemIndex = 0;
                }
            }

        }
        /// <summary>
        /// EDC名称信息进行数据绑定
        /// </summary>
        private void BindEDCNameToControl(IServerObjFactory iServerObjFactory)
        {

            DataSet dsLines = iServerObjFactory.CreateIEDCEngine().GetEDC_MAIN();
            if (dsLines != null &&dsLines.Tables.Count > 0)
            {
                lueEDCName.Properties.DataSource = dsLines.Tables[0];
                lueEDCName.Properties.DisplayMember = EDC_MAIN_FIELDS.FIELD_EDC_NAME;
                lueEDCName.Properties.ValueMember = EDC_MAIN_FIELDS.FIELD_EDC_KEY;
                if (dsLines.Tables[0].Rows.Count > 0)
                {
                    lueEDCName.ItemIndex = 0;
                }
            }

        }
        /// <summary>
        /// 取消按钮Click事件按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 确定按钮Click事件按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string actionName = this.cmbActionName.Text;
            //判断相应栏位是否都不为空
            if (actionName.Equals(ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT) && string.IsNullOrEmpty(this.luePartType.Text))
            {
                MessageService.ShowMessage("动作设置为TRACKOUT，必须选择成品类型。", "提示");
                this.luePartName.Select();
                return;
            }

            if (string.IsNullOrEmpty(this.lueEDCName.Text))
            {
                MessageService.ShowMessage("请选择参数组名称。","提示");
                this.lueEDCName.Select();
                return;
            }

            if (string.IsNullOrEmpty(this.lueOperationName.Text))
            {
                MessageService.ShowMessage("请选择工序名称。", "提示");
                this.lueOperationName.Select();
                return;
            }
            //this.cmbActionName.Text = COMMON_FIELDS.FIELD_ACTIVITY_TRACKOUT;
            DataSet dsPoint = new DataSet();
            Hashtable mainDataHashTable = new Hashtable();
            if (!string.IsNullOrEmpty(this.cmbActionName.Text))
            {
                mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_ACTION_NAME, this.cmbActionName.Text);
            }
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_EDC_KEY, this.lueEDCName.EditValue.ToString());
            if (!string.IsNullOrEmpty(this.lueEquipmentKey.Text))
            {
                mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_EQUIPMENT_KEY, this.lueEquipmentKey.EditValue.ToString());
                mainDataHashTable.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME, this.lueEquipmentKey.Text.ToString());
            }           
            if (this.lueSPName.EditValue!=null)
            {
                mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_SP_KEY, this.lueSPName.EditValue.ToString());
            }

            string routeKey = Convert.ToString(this.lueRoute.EditValue);
            string routeName = this.lueRoute.Text;
            string stepKey = string.Empty;
            if (!string.IsNullOrEmpty(routeKey))
            {
                stepKey = Convert.ToString(this.lueOperationName.GetColumnValue(POR_ROUTE_STEP_FIELDS.FIELD_ROUTE_STEP_KEY));
            }
            string groupKey =  CommonUtils.GenerateNewKey(0);//表示抽检点设置分组的标识键。
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_TOPRODUCT, this.luePartName.Text);
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_OPERATION_NAME, this.lueOperationName.Text);
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_PART_TYPE, Convert.ToString(this.luePartType.EditValue));
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_GROUP_KEY, groupKey);
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_ROUTE_VER_KEY, routeKey);
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_STEP_KEY, stepKey);
            mainDataHashTable.Add(EDC_POINT_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            edcPoint = new EdcPoint();
            DataTable mainDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
            mainDataTable.TableName =EDC_POINT_FIELDS.DATABASE_TABLE_NAME;
            dsPoint.Tables.Add(mainDataTable);
            int code = edcPoint.CreateEdcPoint(dsPoint);
            if (code == 1)
            {
                //新增成功
                edcPoint.PointRowKey = edcPoint.ErrorMsg;
                edcPoint.PartName = this.luePartName.Text;
                edcPoint.OperationName = this.lueOperationName.Text;
                edcPoint.OperationKey = this.lueOperationName.EditValue.ToString();
                edcPoint.EquipmentName = this.lueEquipmentKey.Text;
                edcPoint.EquipmentKey = Convert.ToString(this.lueEquipmentKey.EditValue).Replace(" ", "");
                edcPoint.ActionName = this.cmbActionName.Text;
                edcPoint.SpName = this.lueSPName.Text;
                edcPoint.EdcName = this.lueEDCName.Text;
                edcPoint.RouteKey = routeKey;
                edcPoint.RouteName = routeName;
                edcPoint.StepKey = stepKey;
                edcPoint.PartType = Convert.ToString(this.luePartType.EditValue);
                edcPoint.GroupKey = groupKey;
                edcPoint.SpKey =  Convert.ToString(this.lueSPName.EditValue);
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageService.ShowError(edcPoint.ErrorMsg);
            } 
        }
        /// <summary>
        /// 工序名称改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationName_EditValueChanged(object sender, EventArgs e)
        {
            if (this.lueOperationName.Text != "")
            {
                IServerObjFactory sof = CallRemotingService.GetRemoteObject();
                DataSet dsLines = sof.CreateIOperationEquipments().GetOperationEquipments(this.lueOperationName.EditValue.ToString());
                if (dsLines != null　&& dsLines.Tables.Count > 0)
                {
                    this.lueEquipmentKey.Properties.Items.Clear();
                    foreach (DataRow dr in dsLines.Tables[0].Rows)
                    {
                        string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                        string equipmentCode =  Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                        string equipmentKey =  Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                        string description=string.Format("{0}({1})",equipmentName,equipmentCode);
                        this.lueEquipmentKey.Properties.Items.Add(equipmentKey.Trim(),description);
                    }
                }
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 成品号改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void luePartName_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.luePartName.Text))
            {
                string partType = Convert.ToString(this.luePartName.GetColumnValue("PART_TYPE"));
                this.luePartType.EditValue = partType;
                this.luePartType.Enabled = false;
            }
            else
            {
                this.luePartType.EditValue = string.Empty;
                this.luePartType.Enabled = true;
            }
        }
        /// <summary>
        /// 工艺流程改变时触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueRoute_EditValueChanged(object sender, EventArgs e)
        {
            string routeKey = Convert.ToString(this.lueRoute.EditValue);
            BindOperationName(routeKey);
        }
    }
}

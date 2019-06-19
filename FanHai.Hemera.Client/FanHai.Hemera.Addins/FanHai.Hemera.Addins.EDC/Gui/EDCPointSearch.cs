
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
#endregion

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 抽检点查询对话框。
    /// </summary>
    public partial class EDCPointSearch : BaseDialog
    {
        public EdcPoint edcPoint = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public EDCPointSearch()
            : base("查询抽检点")
        {
            InitializeComponent();
            edcPoint = new EdcPoint();
        }
        /// <summary>
        /// 点击查询按钮获取符合条件的信息
        /// </summary>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dsPoint= new DataSet();
            edcPoint.PartName = this.tePartName.Text.Trim();
            edcPoint.OperationName = this.teOperationName.Text.Trim();
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
        /// 双击页面的数据行
        /// </summary>
        private void gridViewEdc_DoubleClick(object sender, EventArgs e)
        {           
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                this.Close();
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
               edcPoint.PointRowKey=gridViewEdc.GetRowCellValue(rowHandle, EDC_POINT_FIELDS.FIELD_ROW_KEY).ToString();
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
        /// <summary>
        /// 取消按钮单击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
    }
}

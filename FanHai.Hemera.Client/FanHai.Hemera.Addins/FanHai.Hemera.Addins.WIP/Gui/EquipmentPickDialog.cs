using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Core;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DXUtils=DevExpress.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 设备查询的对话框。
    /// </summary>
    public partial class EquipmentPickDialog : BaseDialog
    {
        private DataSet _ds = null;
        /// <summary>
        /// 选择的设备数据。
        /// </summary>
        public object[] SelectedEquipmentData = null;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public EquipmentPickDialog(DataSet ds)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.EquipmentQuery.Name}"))
        {
            InitializeComponent();
            _ds = ds;
        }
        /// <summary>
        /// 界面载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentQuery_Load(object sender, EventArgs e)
        {
            if (_ds != null && _ds.Tables.Count > 0)
            {
                BindDataToEquipmentsGrid(_ds.Tables[0]);
            }
        }
        /// <summary>
        /// 绑定设备数据到UI界面。
        /// </summary>
        /// <param name="dataTable">包含设备数据的数据表对象。</param>
        private void BindDataToEquipmentsGrid(DataTable dataTable)
        {
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.DataSource = dataTable;
        }
        /// <summary>
        /// 映射选择的数据返回。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = grdViewEquipments.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                DataRow selectedEquipmentDataRow = this.grdViewEquipments.GetDataRow(rowHandle);

                this.SelectedEquipmentData = new object[selectedEquipmentDataRow.ItemArray.Length];
                selectedEquipmentDataRow.ItemArray.CopyTo(this.SelectedEquipmentData, 0);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设备数据行双击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdEquipments_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 确定按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 取消按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 自定义绘制单元格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewEquipments_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "EQUIPMENT_STATE_NAME")
            {
                string stateName=Convert.ToString(e.CellValue);
                System.Drawing.Color backColor= FanHai.Hemera.Utils.Common.Utils.GetEquipmentStateColor(stateName);
                e.Appearance.BackColor = backColor;
            }
        }
    }
}

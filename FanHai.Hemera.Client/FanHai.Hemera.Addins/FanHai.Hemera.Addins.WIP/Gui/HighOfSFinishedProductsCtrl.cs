using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Addins.WIP;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class HighOfSFinishedProductsCtrl : BaseUserCtrl
    {

        //LotDispatchDetailModel _model = null;
        public int version = 1;
        public HighOfSFinishedProductsCtrl()
        {
            InitializeComponent();
           
            this.txtUser.Text = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
        }

        ///// <summary>
        ///// 绑定车间。
        ///// </summary>
        //private void BindFactoryRoom()
        //{
        //    string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
        //    DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
        //    if (dt != null)
        //    {
        //        this.lueFac.Properties.DataSource = dt;
        //        this.lueFac.Properties.DisplayMember = "LOCATION_NAME";
        //        this.lueFac.Properties.ValueMember = "LOCATION_KEY";
        //        if (dt.Rows.Count > 0)
        //        {
        //            this.lueFac.ItemIndex = 0;
        //        }
        //    }
        //    else
        //    {
        //        this.lueFac.Properties.DataSource = null;
        //        this.lueFac.EditValue = string.Empty;
        //    }
        //}

        ///// <summary>
        ///// 绑定线别。
        ///// </summary>
        //private void BindLine()
        //{
        //    string strFactoryRoomKey = this.lueFac.EditValue.ToString();
        //    string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
        //    this.lueLine.EditValue = string.Empty;
        //    Line entity = new Line();
        //    DataSet ds = entity.GetLinesInfo(strFactoryRoomKey, strLines);
        //    if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
        //    {
        //        this.lueLine.Properties.DataSource = ds.Tables[0];
        //        this.lueLine.Properties.DisplayMember = "LINE_NAME";
        //        this.lueLine.Properties.ValueMember = "PRODUCTION_LINE_KEY";
        //    }
        //    else
        //    {
        //        MessageService.ShowMessage(entity.ErrorMsg);
        //    }
        //}
        ///// <summary>
        ///// 绑定工序。
        ///// </summary>
        //private void BindOperations()
        //{
        //    //获取登录用户拥有权限的工序名称。
        //    string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
        //    if (operations.Length > 0)//如果有拥有权限的工序名称
        //    {
        //        string[] strOperations = operations.Split(',');
        //        //遍历工序，并将其添加到窗体控件中。
        //        for (int i = 0; i < strOperations.Length; i++)
        //        {
        //            lueOprition.Properties.Items.Add(strOperations[i]);
        //        }
        //        this.lueOprition.SelectedIndex = 0;
        //    }
        //}
        ///// <summary>
        ///// 初始化窗体组件上的文本。
        ///// </summary>
        //private void InitializeComponentText()
        //{
        //    if (this._model != null)
        //    {
        //        this.lueFac.EditValue = this._model.RoomKey;
        //        this.lueOprition.EditValue = this._model.OperationName;
        //        this.txtLotNumber.Select();
        //    }
        //    else
        //    {
        //        this._model = new LotDispatchDetailModel();
        //        if (this.lueOprition.Properties.Items.Count > 1)
        //        {
        //            this.lueOprition.Select();
        //        }
        //        else
        //        {
        //            this.txtLotNumber.Select();

        //        }
        //    }
        //}


        ///// <summary>
        ///// 绑定设备。
        ///// </summary>
        //private void BindEquipment()
        //{
        //    string strOperation = this.lueOprition.Text.Trim();
        //    string strFactoryRoomName = this.lueFac.Text;
        //    string strFactoryRoomKey = this.lueFac.EditValue == null ? string.Empty : this.lueFac.EditValue.ToString();
        //    string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
        //    //如果工厂车间或者工序或者线别主键为空。
        //    if (string.IsNullOrEmpty(strFactoryRoomName)
        //        || string.IsNullOrEmpty(strOperation)
        //        || string.IsNullOrEmpty(strLines))
        //    {
        //        return;
        //    }
        //    this.lueEuipment.EditValue = string.Empty;
        //    this.lueEuipment.Properties.ReadOnly = false;

        //    EquipmentEntity entity = new EquipmentEntity();
        //    DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
        //    if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
        //    {
        //        this.lueEuipment.Properties.DataSource = ds.Tables[0];
        //        this.lueEuipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE;
        //        this.lueEuipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;

        //    }
        //    else
        //    {
        //        MessageService.ShowMessage(entity.ErrorMsg);
        //    }
         
        //    SetLineValue();
        //}

        ///// <summary>
        ///// 设置线别的值。
        ///// </summary>
        //private void SetLineValue()
        //{
        //    string lineKey = Convert.ToString(this.lueEuipment.GetColumnValue("LINE_KEY"));
        //    this.lueLine.EditValue = lineKey;
        //}

        //private void lueFac_EditValueChanged(object sender, EventArgs e)
        //{
        //    BindLine();
        //    //重新绑定设备控件
        //    BindEquipment();
            
        //}

        //private void lueEuipment_EditValueChanged(object sender, EventArgs e)
        //{
        //    SetLineValue();
        //}

        //private void lueOprition_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //重新绑定设备控件
        //    BindEquipment();
        //}

        private void HighOfSFinishedProductsCtrl_Load(object sender, EventArgs e)
        {
            //BindFactoryRoom();
            //BindOperations();
            //BindLine();
            //BindEquipment();
            //InitializeComponentText();
            this.txtLotNumber.Focus();

        }

        private void txtLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string lotNumber = this.txtLotNumber.Text.ToString().Trim();
                if (string.IsNullOrEmpty(lotNumber))
                {
                    MessageBox.Show("请输入组件序列号！","系统提示");
                    return;
                }
                GetInfByLotNum(lotNumber);
            }
        }

        public void GetInfByLotNum(string lotNumber)
        {
            HighOfSFinishedProductsEngine highOfFinished = new HighOfSFinishedProductsEngine();
            DataSet dtGetHighInfByLotNum = highOfFinished.GetHighInfByLotNum(lotNumber);

            if (dtGetHighInfByLotNum.Tables[0].Rows.Count > 0)
            {
                version =Convert.ToInt32(dtGetHighInfByLotNum.Tables[0].Rows[0]["GWJ_VERSION"].ToString());
            }
            this.gcItems.DataSource = dtGetHighInfByLotNum.Tables[0];

            this.txtLotNumber.Properties.ReadOnly = true;
        }

        private void btnAddPal_Click(object sender, EventArgs e)
        {
            string lotNumber = this.txtLotNumber.Text.ToString().Trim();
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageBox.Show("请输入组件序列号！", "系统提示");
                return;
            }
            if (this.txtLotNumber.Properties.ReadOnly == false)
            {
                GetInfByLotNum(lotNumber);
            }
            DataTable dt = ((DataView)gridView1.DataSource).Table;

            DataRow dr = dt.NewRow();
            dr["LOT_NUMBER"] = lotNumber;
            dr["GWJ_POSITION"] = "";
            dr["GWJ_DETAIL"] = "";
            dr["CREATOR"] = this.txtUser.Text.ToString().Trim();
            dr["EDITOR"] = this.txtUser.Text.ToString().Trim();
            dr["GWJ_VERSION"] = version ;

            dt.Rows.Add(dr);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {

            DialogResult dts = MessageBox.Show("取消会清空所有信息不做保存，确认要取消吗？", "系统提示！", MessageBoxButtons.OKCancel);
            if (dts == DialogResult.OK)
            {
                this.txtLotNumber.Properties.ReadOnly = false;
                this.txtLotNumber.SelectAll();
                this.gcItems.DataSource = null;
            }

        }

        private void btnRemovePal_Click(object sender, EventArgs e)
        {
            if (this.gridView1.GetFocusedRow() != null)
            {
                if (this.gridView1.State == GridState.Editing && this.gridView1.IsEditorFocused && this.gridView1.EditingValueModified)
                {
                    this.gridView1.SetFocusedRowCellValue(this.gridView1.FocusedColumn, this.gridView1.EditingValue);
                }
                //this.gvList.UpdateCurrentRow();
                int rowHandle = this.gridView1.FocusedRowHandle;
                this.gridView1.DeleteRow(rowHandle);

                if (rowHandle == 0)
                {
                    this.txtLotNumber.Properties.ReadOnly = false;
                    this.txtLotNumber.SelectAll();
                    return;
                }
                ((DataView)gridView1.DataSource).Table.AcceptChanges();
                DataTable dt = ((DataView)gridView1.DataSource).Table; ;
                if (dt != null)
                {
                    gcItems.Refresh();
                    gcItems.DataSource = dt;
                }
                else
                {
                    gcItems.DataSource = null;

                }

            }
            else
            {
                MessageService.ShowMessage("必须选择至少选择一条记录", "${res:Global.SystemInfo}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (gridView1.DataSource == null)
            {
                MessageService.ShowMessage("没有要保存的数据!", "${res:Global.SystemInfo}");
                return;
            }
            string lotNumber = this.txtLotNumber.Text.ToString().Trim();
            try
            {
                if (MessageBox.Show(StringParser.Parse("是否保存当前高位检的信息？"),
                           StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (this.gridView1.State == GridState.Editing && this.gridView1.IsEditorFocused && this.gridView1.EditingValueModified)
                    {
                        this.gridView1.SetFocusedRowCellValue(this.gridView1.FocusedColumn, this.gridView1.EditingValue);
                    }
                    this.gridView1.UpdateCurrentRow();
                    string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                    HighOfSFinishedProductsEngine highOfFinished = new HighOfSFinishedProductsEngine();
                    DataTable dtGvlist = ((DataView)gridView1.DataSource).Table;

                    bool dsreturn = highOfFinished.InsertIntoGWJ(dtGvlist, lotNumber);

                    if (dsreturn == true)
                    {
                        this.txtLotNumber.Properties.ReadOnly = false;
                        this.txtLotNumber.Focus();
                        this.txtLotNumber.SelectAll();
                        this.gcItems.DataSource = null;

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message, "${res:Global.SystemInfo}");
                return;
            }
        }

        private void btnOutPut_Click(object sender, EventArgs e)
        {
            if (this.txtLotNumber.Properties.ReadOnly == false)
            {
                MessageBox.Show("当前为编辑状态不能导出！", "${res:Global.SystemInfo}");
                return;
            }
            if (gridView1.DataSource == null)
            {
                MessageService.ShowMessage("清单列表为空，无可导出数据!", "${res:Global.SystemInfo}");
                return;
            }
            DataTable dtGvlist = ((DataView)gridView1.DataSource).Table;

            int nColumn, nRow, nNowRow;
            nNowRow = 1;
            if (dtGvlist.Rows.Count > 0)
            {
                try
                {
                    nColumn = dtGvlist.Columns.Count;
                    nRow = dtGvlist.Rows.Count;

                    Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
                    oExcel.Visible = false;
                    Microsoft.Office.Interop.Excel.Workbook oWorkbook = oExcel.Workbooks.Add(true);
                    Microsoft.Office.Interop.Excel.Worksheet oWorksheet =oWorkbook.Worksheets[1];
       
                    oWorksheet.Cells[nNowRow, 1] = "组件序列号";
                    oWorksheet.Cells[nNowRow, 2] = "不良位置";
                    oWorksheet.Cells[nNowRow, 3] = "不良描述";
                    oWorksheet.Cells[nNowRow, 4] = "修改人";
                    oWorksheet.Cells[nNowRow, 5] = "创建人";
                    oWorksheet.Cells[nNowRow, 6] = "版本";
                    
                    oWorksheet.get_Range("A1", "F1").Interior.ColorIndex = 48;


                    for (int r = 0; r < nRow; r++)
                    {
                        nNowRow++;
                        for (int c = 0; c < nColumn; c++)
                        {
                            if (c == 0)
                                oWorksheet.Cells[nNowRow, c + 1] = "'"+dtGvlist.Rows[r][c].ToString();
                            else
                                oWorksheet.Cells[nNowRow, c + 1] = dtGvlist.Rows[r][c].ToString();
                        }
                    }

                    oExcel.Visible = true;
                    oExcel.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel);
                    System.GC.Collect();
                }
                catch //(Exception ex)
                {
                    MessageService.ShowMessage("创建Excel失败，请确认是否有安装Excel应用程序！", "提示");
                    return;
                }
            }
        
        }
    }
}

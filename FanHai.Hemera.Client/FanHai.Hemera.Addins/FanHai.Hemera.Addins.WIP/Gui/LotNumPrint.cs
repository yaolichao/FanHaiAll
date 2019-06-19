using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraLayout.Utils;
using FanHai.Hemera.Addins.WIP.Report;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotNumPrint : BaseUserCtrl
    {        
        /// <summary>
        /// 是否显示选择设备的对话框。
        /// </summary>
        bool _isShowPickEquipmetDialog = false;
        string facKey = string.Empty;
        string opritionName = string.Empty;
        string equipmentKey = string.Empty;
        string lineKey = string.Empty;
        string mac = string.Empty;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        public LotNumPrint()
        {
            InitializeComponent();
            InitializeLanguage();
        }



        private void InitializeLanguage()
        {
            this.gridColumn1.Caption = StringParser.Parse("${res:Global.XuLieHao}");//"序列号";
            this.gridColumn2.Caption = StringParser.Parse("${res:Global.WorkNumber}");// "工单号";
            this.btn_Search.Text = StringParser.Parse("${res:Global.Query}");// "查询";
            this.layoutControlItem3.Text = StringParser.Parse("${res:Global.Step}");//"工序";
            this.layoutControlItem2.Text = StringParser.Parse("${res:Global.Line}");//"线别";

            this.gridColumn6.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gridColumn6}");// "打印时间";
            this.gridColumn3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gridColumn3}");// "打印人";
            this.gridColumn4.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gridColumn4}");// "电脑MAC";
            this.gridColumn5.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gridColumn5}");// "是否补打";
            this.smbJZ.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.smbJZ}");// "校准";
            this.smbNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.smbNew}");// "刷新";
            this.btnPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.btnPrint}");// "打印";
            this.gcLotNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcLotNum}");// "组件序列号";
            this.gcWorkOrder.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcWorkOrder}");// "工单号";
            this.gcCreateTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcCreateTime}");// "创建日期";
            this.gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcCreator}");// "创建人";
            this.gcLotNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcLotNumber}");// "组件序列号";
            this.gcWordOrderNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcWordOrderNum}");// "工单号";
            this.gcCreate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcCreate}");// "创建人";
            this.gcCreateDate.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcCreateDate}");// "创建日期";
            this.gcPrinter.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcPrinter}");// "打印人员";
            this.gcPrintTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.gcPrintTime}");// "打印时间";
            this.lblEqu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.lblEqu}");// "设备号";
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem1}");// "状态";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.lciFactoryRoom}");// "工厂车间";
            this.layoutControlGroup1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlGroup1}");// "未打印序列号";
            this.layoutControlGroup3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlGroup3}");// "标签定位";
            this.layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem11}");// "横向X轴";
            this.layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem12}");// "纵向Y轴";
            this.layoutControlGroup2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlGroup2}");// "已打印未过单串焊站序列号";
            this.layoutControlItem4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem4}");// "已打印为过站序列号";
            this.layoutControlGroup4.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlGroup4}");// "打印查询条件";
            this.layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem13}");// "序列号";
            this.layoutControlItem14.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem14}");// "起始日期";
            this.layoutControlItem15.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem15}");// "结束日期";
            this.layoutControlItem16.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlItem16}");// "打印人";
            this.layoutControlGroup5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.layoutControlGroup5}");// "查询结果";
        }





        LotNumPrintEngine lotNumPrint = new LotNumPrintEngine();
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
        /// 绑定线别。
        /// </summary>
        private void BindLine()
        {
            string strFactoryRoomKey = this.lueFactoryRoom.EditValue.ToString();
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            this.lueLine.EditValue = string.Empty;
            Line entity = new Line();
            DataSet ds = entity.GetLinesInfo(strFactoryRoomKey, strLines);
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueLine.Properties.DataSource = ds.Tables[0];
                this.lueLine.Properties.DisplayMember = "LINE_NAME";
                this.lueLine.Properties.ValueMember = "PRODUCTION_LINE_KEY";
            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
        }
        /// <summary>
        /// 绑定设备。
        /// </summary>
        private void BindEquipment()
        {
            string strOperation = this.cbOperation.Text.Trim();
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
            this.lueEquipment.Properties.ReadOnly = false;

            EquipmentEntity entity = new EquipmentEntity();
            DataSet ds = entity.GetEquipments(strFactoryRoomKey, strOperation, strLines.Split(','));
            if (string.IsNullOrEmpty(entity.ErrorMsg))//执行成功。
            {
                this.lueEquipment.Properties.DataSource = ds.Tables[0];
                this.lueEquipment.Properties.DisplayMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE;
                this.lueEquipment.Properties.ValueMember = EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY;

            }
            else
            {
                MessageService.ShowMessage(entity.ErrorMsg);
            }
            if (this._isShowPickEquipmetDialog)
            {
                ShowEquipmentPickDialog(ds);
            }
            SetEquipmentState();
            SetLineValue();
        }
        /// <summary>
        /// 显示设备选择对话框。
        /// </summary>
        private void ShowEquipmentPickDialog(DataSet ds)
        {
            this.lueEquipment.Properties.ReadOnly = true;
            //显示选择设备的对话框
            EquipmentPickDialog dlg = new EquipmentPickDialog(ds);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.SelectedEquipmentData != null && dlg.SelectedEquipmentData.Length > 0)
                {
                    string equipmentKey = Convert.ToString(dlg.SelectedEquipmentData[0]); //设备主键
                    this.lueEquipment.EditValue = equipmentKey;
                }
            }
        }

        /// <summary>
        /// 设置设备状态。
        /// </summary>
        private void SetEquipmentState()
        {
            string equipmentKey = Convert.ToString(this.lueEquipment.GetColumnValue("EQUIPMENT_REAL_KEY"));
            if (string.IsNullOrEmpty(equipmentKey))
            {
                equipmentKey = Convert.ToString(this.lueEquipment.EditValue);
            }
            if (!string.IsNullOrEmpty(equipmentKey))
            {
                EquipmentEntity entity = new EquipmentEntity();
                DataSet ds = entity.GetEquipmentState(equipmentKey);
                if (string.IsNullOrEmpty(entity.ErrorMsg))
                {
                    string description = Convert.ToString(ds.Tables[0].Rows[0]["DESCRIPTION"]);
                    string stateName = Convert.ToString(ds.Tables[0].Rows[0]["EQUIPMENT_STATE_NAME"]);
                    this.txtEquipmentState.Text = ds.Tables[0].Rows.Count > 0
                            ? string.Format("{0}({1})", stateName, description)
                            : string.Empty;
                    System.Drawing.Color backColor = FanHai.Hemera.Utils.Common.Utils.GetEquipmentStateColor(stateName);
                    this.txtEquipmentState.BackColor = backColor;
                }
                else
                {
                    this.txtEquipmentState.Text = string.Empty;
                    this.txtEquipmentState.BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                this.txtEquipmentState.Text = string.Empty;
            }
        }

        /// <summary>
        /// 设置线别的值。
        /// </summary>
        private void SetLineValue()
        {
            string lineKey = Convert.ToString(this.lueEquipment.GetColumnValue("LINE_KEY"));
            this.lueLine.EditValue = lineKey;
        }
        /// <summary>
        /// 绑定车间。
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
                    this.lueFactoryRoom.ItemIndex = 0;
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
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
        private void LotNumPrint_Load(object sender, EventArgs e)
        {
            repositoryItemCheckEdit1.CheckedChanged += repositoryItemCheckEdit1_CheckedChanged;
            mac = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_MAC);
            gcPrintMessage.DataSource = lotNumPrint.GetPrintInf(null,null,null,null).Tables[0];

            BindFactoryRoom();
            BindOperations();
            BindLine();
            BindEquipment();
            btnPrint.Select();
            HideControl();
            repositoryItemCheckEdit1.CheckedChanged += repositoryItemCheckEdit1_CheckedChanged;
        }

        private void cbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //重新绑定设备控件
            BindEquipment();
        }

        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            //设置设备状态。
            SetEquipmentState();
            SetLineValue();
            //绑定数据
            if (!string.IsNullOrEmpty(lueFactoryRoom.EditValue.ToString())
                && !string.IsNullOrEmpty(lueEquipment.EditValue.ToString())
                )
            {
                BindGcNotPrint();
            }
        }
        private void BindGcNotPrint()
        {
            facKey = lueFactoryRoom.EditValue.ToString();
            equipmentKey = lueEquipment.EditValue.ToString();
            lineKey = lueEquipment.GetColumnValue("LINE_KEY").ToString();

            DataSet dsNotPrint = lotNumPrint.GetNotPrintLotNumInf(facKey, equipmentKey, lineKey);
            DataTable dt = dsNotPrint.Tables["NOT_PRINT"];
            dt.Columns.Add("IS_CHECK", System.Type.GetType("System.Boolean"));
            gcNotPrint.DataSource = dt;
            gcPrint.DataSource = dsNotPrint.Tables["PRINT"]; 
        }
        private void lueEquipment_Click(object sender, EventArgs e)
        {
            this.lueEquipment.SelectAll();
            //工序自定义属性 IS_SHOW_PICK_EQUIPMENT_DIALOG 确定是否显示设备选择对话框。
            if (this._isShowPickEquipmetDialog)
            {
                DataTable dt = this.lueEquipment.Properties.DataSource as DataTable;
                DataSet ds = new DataSet();
                ds.Merge(dt);
                ShowEquipmentPickDialog(ds);
                SetEquipmentState();
                SetLineValue();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.gvNotPrint.GetFocusedRow() != null)
            {
                DataSet dsId = new DataSet();
                DataRow drlotNumber = this.gvNotPrint.GetFocusedDataRow();
                string lotNumber = Convert.ToString(drlotNumber["LOT_NUMBER"]);
                string orderNum = Convert.ToString(drlotNumber["WORK_ORDER_NO"]);
                string printer = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);

                bool flag = lotNumPrint.CheckAndUpdateLotInf(lotNumber, printer, facKey, equipmentKey, lineKey);
                if (flag == true && string.IsNullOrEmpty(lotNumPrint.ErrorMsg))
                {
                    dsId = lotNumPrint.GetIdByOrderNumber(orderNum);
                    if (dsId.Tables[0].Rows.Count <= 0)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.Msg002}"), MESSAGEBOX_CAPTION);//打印标签失败，打印规则有改动，请工艺确认！
                        //MessageService.ShowMessage("打印标签失败，打印规则有改动，请工艺确认！", "提示");
                        BindGcNotPrint();
                        return;
                    }
                    int x = string.IsNullOrEmpty(this.txtX.Text.Trim()) ? 0 : Convert.ToInt32(this.txtX.Text.Trim());
                    int y = string.IsNullOrEmpty(this.txtY.Text.Trim()) ? 0 : Convert.ToInt32(this.txtY.Text.Trim());
                    int printCount = 1;
                    if (!string.IsNullOrEmpty(txtPrintCount.Text))
                    {
                        printCount = Convert.ToInt32(txtPrintCount.Text);
                    }
                    int c = gvNotPrint.RowCount;
                    for (int i = 0; i < c; i++)
                    {
                        DataRowView dr = (DataRowView)gvNotPrint.GetRow(i);
                        string lot = dr.Row["LOT_NUMBER"].ToString();
                        string order = dr.Row["WORK_ORDER_NO"].ToString();
                        bool ischecked = (bool)dr.Row["IS_CHECK"];
                        if (!ModulePrint.PrintLabel(lot, dsId.Tables[0].Rows[0]["PRINT_CODE"].ToString(),
                              x, y, printCount))
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.Msg001}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                                                                                                                                                     //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                            BindGcNotPrint();
                            return;
                        }
                        lotNumPrint.UpdateLotInf(lot, printer);

                        lotNumPrint.save_Print(lot, printer, mac, 'N');

                    }

                    BindGcNotPrint();

                }
                if (!string.IsNullOrEmpty(lotNumPrint.ErrorMsg))
                {
                    MessageBox.Show(lotNumPrint.ErrorMsg);
                    BindGcNotPrint();
                    return;
                }
            }
            else
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumPrint.Msg003}"), MESSAGEBOX_CAPTION);//必须选择至少选择一条记录！
                //MessageService.ShowMessage("必须选择至少选择一条记录", "${res:Global.SystemInfo}");
            }
        }


        private void smbJZ_Click(object sender, EventArgs e)
        {
            DataSet dsId = new DataSet();

            //DataRow drlotNumber = this.gvNotPrint.GetFocusedDataRow();
            //string orderNum = Convert.ToString(drlotNumber["WORK_ORDER_NO"]);
            //dsId = lotNumPrint.GetIdByOrderNumber(orderNum);
            int x = string.IsNullOrEmpty(this.txtX.Text.Trim()) ? 0 : Convert.ToInt32(this.txtX.Text.Trim());
            int y = string.IsNullOrEmpty(this.txtY.Text.Trim()) ? 0 : Convert.ToInt32(this.txtY.Text.Trim());
            ModulePrint.PrintLabel("0000000000000000", "4", x, y);
        }

        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            BindLine();
            //重新绑定设备控件
            BindEquipment();
        }

        private void cbOperation_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定设备控件
            BindEquipment();
        }

        private void smbNew_Click(object sender, EventArgs e)
        {
            BindGcNotPrint();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string lotNumber = txt_lotNumber.Text.Trim();
            string dateStart = date_Start.Text.Trim();
            string dateEnd = date_End.Text.Trim();
            string printerNo = txt_PrinterNo.Text.Trim();

            DataSet dsPrintMessage = lotNumPrint.GetPrintInf(lotNumber, dateStart, dateEnd, printerNo);

            gcPrintMessage.DataSource = dsPrintMessage.Tables[0];
        }

        private void txtPrintCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && !Char.IsPunctuation(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;//消除不合适字符  
            }
            else if (Char.IsPunctuation(e.KeyChar))
            {
                if (e.KeyChar != '.' || this.Text.Length == 0||e.KeyChar<'1')//小数点  
                {
                    e.Handled = true;
                }
                if (this.Text.LastIndexOf('.') != -1)
                {
                    e.Handled = true;
                }
            }
        }

        //全局变量 0:表格中的数据没有全部选中 1：表格中的数据全部选中
        public int iCheckAll = 0;
        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit chkCheck = (sender as CheckEdit);
            DataRow dr = gvNotPrint.GetFocusedDataRow();
            if (chkCheck.CheckState == CheckState.Checked)
            {
                dr["IS_CHECK"] = true;
            }
            else
            {
                dr["IS_CHECK"] = false;

            }

            //增加全部选择时，全选按钮应该勾选上
            DataTable dt = gcNotPrint.DataSource as DataTable;

            //判断如果GridView中按钮都全选了，把全选按钮也设置为选中状态 
            DataRow[] drTemp = dt.Select("IS_CHECK=0 OR IS_CHECK IS NULL");
            if (drTemp.Length > 0)
            {
                //没有全部选中
                iCheckAll = 0;
                cb_checkall.CheckState = CheckState.Unchecked;
            }
            else
            {
                iCheckAll = 1;
                cb_checkall.CheckState = CheckState.Checked;
            }



        }

        private void cb_checkall_CheckedChanged(object sender, EventArgs e)
        {

            DataTable dt = gcNotPrint.DataSource as DataTable;

            //优化 增加判断dt为null的条件
            if (dt != null)
            {
                if (cb_checkall.Checked == true)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item["IS_CHECK"] = true;
                    }
                    iCheckAll = 1;
                }
                else
                {
                    if (iCheckAll == 0)
                    {
                        //表格中的数据没有全部选中时  设置全选框的的状态为FALSE  （觉得这个条件可以不要，可以试下哦O(∩_∩)O哈哈~）
                        DataRow[] drMM = dt.Select("IS_CHECK=0 OR IS_CHECK IS NULL");
                        if (drMM.Length > 0)
                        {
                            cb_checkall.Checked = false;
                        }
                    }
                    else if (iCheckAll == 1)
                    {

                        //表格中的数据是全选中状态时，取消全选时，设置表格中的标识为不选中的状态
                        foreach (DataRow item in dt.Rows)
                        {
                            item["IS_CHECK"] = false;
                        }
                    }


                }

            }
            else
            {
                //判断条件
                MessageBox.Show("没有可供选择的数据", "提示！");
                cb_checkall.Checked = false;
            }

        }
    }
}

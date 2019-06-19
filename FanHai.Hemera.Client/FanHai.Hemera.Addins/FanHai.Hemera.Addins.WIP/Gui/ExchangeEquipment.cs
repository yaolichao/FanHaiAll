using System;
using System.Collections.Generic;
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
using DevExpress.XtraLayout.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ExchangeEquipment : BaseUserCtrl
    {
        
        public ExchangeEquipment()
        {
            InitializeComponent();
            InitializeLanguage();
        }



        private void InitializeLanguage()
        {
            this.btnSave.Text = StringParser.Parse("${res:Global.OKButtonText}");// "确定"
            this.lciLine.Text = StringParser.Parse("${res:Global.Line}");//"线别";
            this.lciBtnSave.Text = StringParser.Parse("${res:Global.Save}");// "保存";
            this.lciLineNew.Text = StringParser.Parse("${res:Global.Line}");//"线别";
            this.layoutControlItem1.Text = StringParser.Parse("${res:Global.LotSeqNumber}");//组件序列号
            this.lcgTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lcgTop}");//"表头";
            this.lcgContent.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lcgContent}");//"现所在设备";
            this.lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lciOperation}");//"工序名称";
            this.lciEquipment.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lciEquipment}");//"设备名称";
            this.lcgHidden.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lcgHidden}");//"隐藏";
            this.lcgCommands.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lcgCommands}");//"命令";
            this.layoutControlGroup1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.layoutControlGroup1}");//"需转设备";
            this.lciProId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lciProId}");//"工序名称";
            this.lciEquipmentNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lciEquipmentNew}");//"设备名称";
            this.layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.layoutControlItem2}");//"状态";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.lciFactoryRoom}");//"工厂车间";
        }





        LotNumPrintEngine lotNumPrint = new LotNumPrintEngine();

        private void txtLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string lotNumber = this.txtLotNumber.Text.ToString().Trim();

                if (string.IsNullOrEmpty(lotNumber))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg001}"));//组件序列号不能为空
                    //MessageBox.Show("组件序列号不能为空！请输入");
                    return;
                }

                DataSet dsreturn = lotNumPrint.GetEquipmentByLotNumber(lotNumber);   //根据批次号获取工序设备线别信息
                if (!string.IsNullOrEmpty(lotNumPrint.ErrorMsg))
                {
                    MessageBox.Show(lotNumPrint.ErrorMsg);
                    return;
                }
                if (dsreturn.Tables.Count <= 0 || dsreturn.Tables[0].Rows.Count <= 0)
                {
                    MessageBox.Show("Batch exception GetEquipmentByLotNumber(lotNumber)");
                    return;
                }
                this.txtOperation.Text = dsreturn.Tables[0].Rows[0]["OPERATION_NAME"].ToString();
                this.txtEquipment.Text = dsreturn.Tables[0].Rows[0]["EQUIPMENT_CODE"].ToString();
                this.txtLine.Text = dsreturn.Tables[0].Rows[0]["LINE_NAME"].ToString();
                
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
            //if (this._isShowPickEquipmetDialog)
            //{
            //    ShowEquipmentPickDialog(ds);
            //}
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

        private void ExchangeEquipment_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            BindLine();
            BindEquipment();
            HideControl();
            this.txtLotNumber.Select();
            this.lblMenu.Text = "生产管理>组件管理>焊接设备转产";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string lotNumber = this.txtLotNumber.Text.ToString().Trim();

            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg001}"));//组件序列号不能为空
                return;
            }
            if (string.IsNullOrEmpty(this.txtEquipment.Text.ToString().Trim()))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.Msg001}"));//原设备号不能为空！请输入
                //MessageBox.Show("原设备号不能为空！请输入");
                return;
            }
            if (string.IsNullOrEmpty(this.lueEquipment.Text.ToString().Trim()))
            {
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.Msg002}"));//转换设备号不能为空！请输入
                //MessageBox.Show("转换设备号不能为空！请输入");
                return;
            }
            string lineKey = Convert.ToString(this.lueEquipment.GetColumnValue("LINE_KEY"));
            string equipmentkey = Convert.ToString(this.lueEquipment.GetColumnValue("EQUIPMENT_REAL_KEY"));
            string operations = this.cbOperation.Text.ToString();
            
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.Msg004}"), "Save")) //你确定要保存当前界面的数据吗？
            {

                DataSet dsreturn = lotNumPrint.UpdatePorLot(lotNumber, operations, equipmentkey, lineKey);   //更新批次信息
                if (!string.IsNullOrEmpty(lotNumPrint.ErrorMsg))
                {
                    MessageBox.Show(lotNumPrint.ErrorMsg);
                    return;
                }
                else
                {
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.ExchangeEquipment.Msg003}"));//保存成功！
                    //MessageBox.Show("保存成功！", "系统提示");
                    this.txtLotNumber.SelectAll();
                }
            }
        }

        private void lueEquipment_EditValueChanged(object sender, EventArgs e)
        {
            //设置设备状态。
            SetEquipmentState();
            SetLineValue();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }
        
    }
}

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
    public partial class LotNumRepeatPrint : BaseUserCtrl
    {
        string Mac = string.Empty;
        string printer = string.Empty;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:Global.SystemInfo}"); //提示

        public LotNumRepeatPrint()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.lblTitle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.lblTitle}");//"序列号补打";
            this.smbJZ.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.smbJZ}");//"校准";
            this.btnPrint.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.btnPrint}");//"补打";
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.layoutControlItem1}");//"组件序列号";
            this.layoutControlGroup3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.layoutControlGroup3}");//"标签定位";
            this.layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.layoutControlItem11}");//"横向X轴";
            this.layoutControlItem12.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.layoutControlItem12}");//"纵向Y轴";
        }



        LotNumPrintEngine lotNumPrint = new LotNumPrintEngine();

        private void smbJZ_Click(object sender, EventArgs e)
        {
            int x = string.IsNullOrEmpty(this.txtX.Text.Trim()) ? 0 : Convert.ToInt32(this.txtX.Text.Trim());
            int y = string.IsNullOrEmpty(this.txtY.Text.Trim()) ? 0 : Convert.ToInt32(this.txtY.Text.Trim());
            ModulePrint.PrintLabel("0000000000000000", "4", x, y);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string lotNumber = this.txtLotNumber.Text.ToString().Trim();
            int counts = 0;
            
            if (string.IsNullOrEmpty(lotNumber))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotIVTestPrintDialog.Msg001}"), MESSAGEBOX_CAPTION);//组件序列号不能为空，请确认！
                //MessageBox.Show("组件序列号不能为空！请输入");
                return;
            }
            
            //补打进行提示
            DataSet ds = lotNumPrint.GetPrintInf(lotNumber,null,null,null);
            if (ds.Tables.Count > 0)
            {
                counts = ds.Tables[0].Rows.Count;
                MessageBox.Show(string.Format("【{0}】序列号本次为第{1}次打印，请注意控制！",lotNumber,counts+1));
            }

            DataSet dsId = lotNumPrint.GetPrintIdByLotNumber(lotNumber);   //根据批次号获取打印代码
            if (!string.IsNullOrEmpty(lotNumPrint.ErrorMsg))
            {
                MessageBox.Show(lotNumPrint.ErrorMsg);
                return;
            }
            if (dsId.Tables.Count <= 0 || dsId.Tables[0].Rows.Count <= 0)
            {

                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.Msg001}"), MESSAGEBOX_CAPTION);//请工艺检查该批次工单是否已经维护了对应的打印规则！（工单产品属性设置->打印规则设定）
                //MessageBox.Show("请工艺检查该批次工单是否已经维护了对应的打印规则！（工单产品属性设置->打印规则设定）");
                return;
            }
            int x = string.IsNullOrEmpty(this.txtX.Text.Trim()) ? 0 : Convert.ToInt32(this.txtX.Text.Trim());
            int y = string.IsNullOrEmpty(this.txtY.Text.Trim()) ? 0 : Convert.ToInt32(this.txtY.Text.Trim());
            var aa = dsId.Tables[0].Rows[0]["PRINT_CODE"].ToString();
            if (!ModulePrint.PrintLabel(lotNumber, dsId.Tables[0].Rows[0]["PRINT_CODE"].ToString(),
                        x, y))
            //if (!ModulePrint.PrintLabel("0115002HH265P061500001", "5",
            //x, y))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotNumRepeatPrint.Msg002}"), MESSAGEBOX_CAPTION);//打印标签失败，请重试！
                //MessageService.ShowMessage("打印标签失败，请重试！", "提示");
                return;
            }
            SavePrintMessage(lotNumber, printer, Mac, 'Y');
        }

        private void LotNumRepeatPrint_Load(object sender, EventArgs e)
        {
            Mac = PropertyService.Get(PROPERTY_FIELDS.COMPUTER_MAC);
            printer = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
        }
        /// <summary>
        /// 每次补打记录补打信息,以便追踪 by ruhu.yu 2017/06/20
        /// </summary>
        public void SavePrintMessage(string lotNum, string printer, string Mac, char is_RePrint)
        {
            lotNumPrint.save_Print(lotNum, printer, Mac, is_RePrint);

        }

    }
}

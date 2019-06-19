using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Entities;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 转工单\返工单作业界面。
    /// </summary>
    public partial class LotExchangeWo : BaseUserCtrl
    {
        private ExchangeWoFlag exchangeType;
        private ExchangeWoEntity exchangewoEntity = new ExchangeWoEntity();
        private string queryType = string.Empty;
        /// <summary>
        /// 重新绘制窗体。
        /// </summary>
        private bool isRePaint = false;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="isrepair"></param>
        public LotExchangeWo(ExchangeWoFlag flag)
        {
            InitializeComponent();
            exchangeType = flag;
            InitializeLanguage();
            GridViewHelper.SetGridView(gvExchangeWo);
        }


        private void InitializeLanguage()
        {
            this.RN.Caption = StringParser.Parse("${res:Global.RowNumber}");//"序号";
            this.LOT_NUMBER.Caption = StringParser.Parse("${res:Global.LotSeqNumber}");//组件序列号
            this.WORK_ORDER_NO.Caption = StringParser.Parse("${res:Global.WorkNumber}");//工单号
            this.gcolPartNumber.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.gcolPartNumber}");// "产品料号";
            this.PRO_ID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.PRO_ID}");// "产品ID号";
            this.DESCRIPTIONS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.DESCRIPTIONS}");// "转/返工单原因";
            this.WORK_ORDER_NO2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.WORK_ORDER_NO2}");// "原工单号";
            this.gcolPartNumber2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.gcolPartNumber2}");// "原产品料号";
            this.PRO_ID2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.PRO_ID2}");// "原产品ID号";
            this.EDITOR2.Caption = StringParser.Parse("${res:Global.Operator}");//"操作者";
            this.EDIT_TIME2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.EDIT_TIME2}");// "转换时间";
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");//查询
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.layoutControlItem3}");// "原工单号";
            this.layoutControlItem4.Text = StringParser.Parse("${res:Global.WorkNumber}");//工单号
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.layoutControlItem4}");// "组件序列号";
            this.layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.layoutControlItem1}");// "转换日期";
            this.btnLotExchange.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.btnLotExchange}");// "转/返工单";
            this.btnPalletExchange.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.btnPalletExchange}");// "成托转工单";
            this.btnMultiExchange.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.btnMultiExchange}");// "批量转工单";
            //this.groupControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.groupControl1}");// "转工单信息";
            this.WERKS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.WERKS}");// "车间";
            this.STEP_KEY.Caption = StringParser.Parse("${res:Global.Step}");//"工序";
            this.PARAMENTID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.PARAMENTID}");// "参数";
            this.CODEDESC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.CODEDESC}");// "计划描述";
            this.PRODUCTCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.PRODUCTCODE}");// "成品类型";
            this.CONTROLTYPE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.CONTROLTYPE}");// "控制图类型";
        }


        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotExchangeWo_Load(object sender, EventArgs e)
        {
            InitTextAndTitle();
            InitGridData();
        }
        /// <summary>
        /// 初始化文本和标题。
        /// </summary>
        private void InitTextAndTitle()
        {
            //返工单
            if (exchangeType == ExchangeWoFlag.Repair)
            {
                this.lblMenu.Text = "质量管理>质量作业>组件返工";// "返工";
                //this.lblTitle.Text = "返工";
                //this.lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.btnLotExchange1}");// "返工单";
                this.btnLotExchange.Text = "返工单";
                this.queryType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_PROID;
            }
            //转工单。
            else if (exchangeType == ExchangeWoFlag.Exchange || exchangeType == ExchangeWoFlag.MultiExchange)
            {
                this.lblMenu.Text = "质量管理> 质量作业>工单转换";// "返工单作业";
                //this.lblTitle.Text = "转工单作业";
                //this.lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotExchangeWo.btnLotExchange2}");// "返工单";
                this.btnLotExchange.Text = "转工单";
                this.queryType = ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO;
            }
        }
        /// <summary>
        /// 窗体绘制事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotExchangeWo_Paint(object sender, PaintEventArgs e)
        {
            if (!isRePaint)
            {
                isRePaint = true;
                //单批转工单
                if (exchangeType == ExchangeWoFlag.Exchange)
                {
                   btnLotExchange_Click(null, null);
                }
                //批量转工单
                else if (exchangeType == ExchangeWoFlag.MultiExchange)
                {
                   btnMultiExchange_Click(null, null);
                }
                //返工单
                else if (exchangeType == ExchangeWoFlag.Repair)
                {
                   btnPalletExchange_Click(null, null);
                }
            }
        }
        /// <summary>
        /// 查询按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitGridData();
        }
        /// <summary>
        /// 初始化网格数据。
        /// </summary>
        private void InitGridData()
        {
            Hashtable hstable = new Hashtable();
            if (!string.IsNullOrEmpty(txtLotNumber.Text.Trim()))
                hstable[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtLotNumber.Text.Trim();
            if (!string.IsNullOrEmpty(txtOrderNumber.Text.Trim()))
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtOrderNumber.Text.Trim();
            if (!string.IsNullOrEmpty(txtOrderNumber2.Text.Trim()))
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER + "2"] = txtOrderNumber.Text.Trim();
            if (!string.IsNullOrEmpty(dateStart.Text.Trim()))
                hstable[POR_LOT_FIELDS.FIELD_CREATE_TIME] = dateStart.Text.Trim();
            if (!string.IsNullOrEmpty(dateEnd.Text.Trim()))
                hstable[POR_LOT_FIELDS.FIELD_EDIT_TIME] = dateEnd.Text.Trim();
            hstable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, this.queryType);
            
            #region 添加分页查询
            DataSet reqDS = new DataSet();
            DataSet dsRetun = new DataSet();

            int pages, records, pageNo, pageSize;
            this.paginationExchangeWo.GetPaginationProperties(out pageNo, out pageSize);

            if (pageNo <= 0)
            {
                pageNo = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = PaginationControl.DEFAULT_PAGESIZE;                          //每页行数DEFAULT_PAGESIZE=20
            }

            dsRetun = exchangewoEntity.GetExchangeWoData(reqDS, pageNo, pageSize, out  pages, out  records, hstable);

            if (pages > 0 && records > 0)
            {
                this.paginationExchangeWo.PageNo = pageNo > pages ? pages : pageNo;
                this.paginationExchangeWo.PageSize = pageSize;
                this.paginationExchangeWo.Pages = pages;
                this.paginationExchangeWo.Records = records;
            }
            else
            {
                this.paginationExchangeWo.PageNo = 0;
                this.paginationExchangeWo.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                this.paginationExchangeWo.Pages = 0;
                this.paginationExchangeWo.Records = 0;
            }

            if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
            {
                MessageService.ShowError(exchangewoEntity.ErrorMsg);
                return;
            }

            #endregion

            this.gcExchangeWo.MainView = gvExchangeWo;
            this.gcExchangeWo.DataSource = dsRetun.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            this.gvExchangeWo.BestFitColumns();
        }

        /// <summary>
        /// 分页查询事件。
        /// </summary>
        private void paginationExchangeWo_DataPaging()
        {
            InitGridData();
        }

        /// <summary>
        /// 批次转工单作业事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLotExchange_Click(object sender, EventArgs e)
        {
            LotExchgWoMultiForm lewsf = new LotExchgWoMultiForm(exchangeType);
            if (DialogResult.OK == lewsf.ShowDialog())
            {
                InitGridData();
            }
        }
        /// <summary>
        /// 成托转工单（返工单）作业事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPalletExchange_Click(object sender, EventArgs e)
        {
            LotExchgWoMultiForm lotExchgWoMuuti = new LotExchgWoMultiForm(exchangeType);

            if (DialogResult.OK == lotExchgWoMuuti.ShowDialog())
            { 
            
            }
        }
        /// <summary>
        /// 批量转工单作业。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMultiExchange_Click(object sender, EventArgs e)
        {
            LotExchgWoNoSingleForm lotExchgWoMuuti = new LotExchgWoNoSingleForm(exchangeType);

            if (DialogResult.OK == lotExchgWoMuuti.ShowDialog())
            {

            }
        }

        private void gvExchangeWo_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
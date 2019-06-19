using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using FanHai.Gui.Core;

namespace FanHai.Hemera.Utils.Controls
{
    public delegate void Paging();

    /// <summary>
    /// Commonable Pagination Control
    /// </summary>
    public partial class PaginationControl : DevExpress.XtraEditors.XtraUserControl
    {

        #region Public Constant Variables

        public const int DEFAULT_PAGENO = 1;
        public const int DEFAULT_PAGESIZE = 20;

        #endregion

        #region Constructor

        public PaginationControl()
        {
            InitializeComponent();
            this.PageNo = DEFAULT_PAGENO;
            this.PageSize = DEFAULT_PAGESIZE;

            InitUi();
        }

        private void InitUi()
        {
            //btnRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            //lciPageNo.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.Controls.PaginationControl.lbl.0001}");
            //lciPageCount.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.Controls.PaginationControl.lbl.0002}");
            //lciPageSize.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.Controls.PaginationControl.lbl.0003}");
            //lciRecords.Text = StringParser.Parse("${res:FanHai.Hemera.Utils.Controls.PaginationControl.lbl.0004}");
        }
        #endregion

        #region Control Events

        private void PaginationControl_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Public Events

        public event Paging DataPaging = null;

        #endregion

        #region Public Properties
        /// <summary>
        /// 页号。
        /// </summary>
        public int PageNo
        {
            set
            {
                if (this.currentPage.Properties.MaxValue < value)
                {
                    this.currentPage.Value = this.currentPage.Properties.MaxValue;
                }
                else if (value < this.currentPage.Properties.MinValue)
                {
                    this.currentPage.Value = this.currentPage.Properties.MinValue;
                }
                else
                {
                    this.currentPage.Value = value;
                }
            }
            get
            {
                return (int)this.currentPage.Value;
            }
        }
        /// <summary>
        /// 总页数。
        /// </summary>
        public int Pages
        {
            set
            {
                this.titlePage.Value = value;
                if (value == 0)
                {
                    this.currentPage.Properties.MinValue = 0;
                    this.currentPage.Properties.MaxValue = 1;
                }
                else
                {
                    this.currentPage.Properties.MinValue = 1;
                    this.currentPage.Properties.MaxValue = value;
                }
            }
            get
            {
                return (int)this.titlePage.Value;
            }
        }
        /// <summary>
        /// 每页大小。
        /// </summary>
        public int PageSize
        {
            set
            {
                this.everyPageQty.Value = value;
            }
            get
            {
                return (int)this.everyPageQty.Value;
            }
        }
        /// <summary>
        /// 总记录数。
        /// </summary>
        public int Records
        {
            set
            {
                this.txtRowQty.Text = value+"";

            }
            get
            {
                int records;
                int.TryParse(this.txtRowQty.Text, out records);
                return records;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Data Paging CallBack
        /// </summary>
        private void DataPagingCallBack()
        {
            if (DataPaging != null)
            {
                DataPaging();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Pagination Properties
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        public void GetPaginationProperties(out int pageNo, out int pageSize)
        {
            pageNo = this.PageNo;
            pageSize = this.PageSize;

            if (pageNo <= 0)
            {
                pageNo = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = DEFAULT_PAGESIZE;
            }
        }

        /// <summary>
        /// Set Pagination Properties
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="pages"></param>
        /// <param name="records"></param>
        public void SetPaginationProperties(int pageNo, int pageSize, int pages, int records)
        {
            if (pages > 0 && records > 0)
            {
                this.PageNo = pageNo > pages ? pages : pageNo;
                this.PageSize = pageSize;
                this.Pages = pages;
                this.Records = records;
            }
            else
            {
                this.Pages = 0;
                this.PageNo = 0;
                this.PageSize = DEFAULT_PAGESIZE;
                this.Records = 0;
            }
        }

        #endregion

        #region Component Events

        private void necPageNo_ValueChanged(object sender, EventArgs e)
        {
            DataPagingCallBack();
        }

        private void necPageSize_ValueChanged(object sender, EventArgs e)
        {
            DataPagingCallBack();
        }

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataPagingCallBack();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (currentPage.Value > 1)
            {
                currentPage.Value -= 1;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (currentPage.Value >= titlePage.Value)
            {
                return;
            }
            currentPage.Value += 1;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            currentPage.Value = titlePage.Value;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            currentPage.Value = 1;
        }
    }
}

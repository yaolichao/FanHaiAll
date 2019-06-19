
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.BasicData.Gui
{
    public partial class LineSettingForm : BaseDialog
    {
        //string workOrder = "";
        public DataSet searchDataSet = new DataSet();
        public DataRow drLine = null;

        LineSettingEntity lineSettingEntity = new LineSettingEntity();


        #region Constructor
        public LineSettingForm()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.lbl.0005}"))//"线别查询"
        {
            InitializeComponent();
            //InitEmptyWorkOrdersDataSet();
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            this.lblAttributeName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.lbl.0001}");//线别名称
            this.btSearch.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.lbl.0002}");//查询
            this.btnOK.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.lbl.0003}");//确定
            this.btnCancle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.lbl.0004}");//取消
            gridColumn1.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.GridControl.0001}");//序号
            gridColumn2.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.GridControl.0002}");//名称
            gridColumn3.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.LineSettingForm.GridControl.0003}");//描述
        }
        #endregion

        #region Properties
        public string ObjectKey
        {
            get
            {
                return _objectKey;
            }
        }
        public string ObjectName
        {
            get
            {
                return _objectName;
            }
        }
        #endregion

        /// <summary>
        /// bind data to GridView
        /// </summary>
        /// <param name="dataSet"></param>
        private void BindDataSourceToGrid(DataTable dt)
        {
            gridData.MainView = gridDataView;
            gridData.DataSource = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridDataView_ShowingEditor(object sender, CancelEventArgs e)
        {
            //gridView can't edit
            e.Cancel = true;
        }

        /// <summary>
        /// grid Control Double Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridData_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                // Close();
            }
        }
        #region Actions

        /// <summary>
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            string lineName = string.Empty;
            string userName = string.Empty;

            DataSet dsLine = null;

            try
            {
                userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                lineName = teLineName.Text;

                dsLine = lineSettingEntity.GetLineByUserNameAndLineName(userName, lineName);

                if (string.IsNullOrEmpty(lineSettingEntity.ErrorMsg))
                {
                    MapLineToGridView(dsLine.Tables[FMM_PRODUCTION_LINE_FIELDS.DATABASE_TABLE_NAME]);
                }

            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
        }
        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #endregion Actions

        #region Private Functions

        private bool MapSelectedItemToProperties()
        {
            if (gridDataView.FocusedRowHandle >= 0)
            {
                drLine = gridDataView.GetFocusedDataRow();
                return true;
            }
            return false;
        }

        private void MapLineToGridView(DataTable dt)
        {
            gridData.DataSource = dt;
        }


        #endregion

        #region Private Variables Definition
        private string _objectKey = "";
        private string _objectName = "";

        private void LineSearchDialog_Load(object sender, EventArgs e)
        {
            #region InitUI

            this.btSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancle.Text = StringParser.Parse("${res:Global.CancelButtonText}");


            #endregion
        }
        #endregion Private Variables Definition

    }
}

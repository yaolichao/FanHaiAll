using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class LocationSearchDialog : BaseDialog
    {
        #region public variable
        public string pubLocationKey = ""; //location key to return
        public string pubLocationName = "";
        
        #endregion
        #region constructor with no parameter
        public LocationSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.Title}"))  //窗体名“区域查询” 
        {
            InitializeComponent();
            SetLanguageInfoToControl();
            this.gcList.MainView = gridView1;
        }

        private void SetLanguageInfoToControl()
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");                     //btnQuery按钮名称“查询”                        
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");            //btnConfirm按钮名称“确定” 
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");         //btnConfirm按钮名称“取消” 

            this.lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.lblName}");                                 //lblName名称“名称” 
            //this.grpTop.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GroupCtrlgcGridView}");                      //groupControl1名称“检索到的数据”
            this.gridView1.GridControl.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GridgcList}");                //gridView1.GridControl名称“检索到的数据” 
            this.LOCATION_KEY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GridColumnLocationKey}");           //LOCATION_KEY值“区域主键” 
            this.LOCATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GridColumnLocationName}");         //LOCATION_NAME值“名称”
            this.DESCRIPTIONS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GridColumnLocationDescription}");   //DESCRIPTIONS值“描述” 
            this.LOCATION_LEVEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.GridColumnLocationLevel}");       //LOCATION_LEVEL值“车间/区域”
        }
        #endregion


        #region search data
        /// <summary>
        /// search data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            #region variable define
            string locationName = "";   //location name
            string locationLevel = "";
            LocationEntity locationEntity = new LocationEntity(); //Location entity
            DataSet _returnDs = new DataSet(); //dataset to receive data of searching
            #endregion

            #region search deal
            //get locationName 将当前txtName的值赋值给locationName
            locationName = this.txtName.Text.Trim().ToString();
            //cmbLocationLevel下拉列表值不为空 locationName 
            if (this.cmbLocationLevel.Text != "")
            {
                //将cmbLocationLevel中选择的值取第一个字符赋值给变量locationLevel 
                locationLevel = this.cmbLocationLevel.Text.Substring(0, 1);
            }

            //set value to entity of LotNumber 判断locationName不为空  
            if (locationName != "")
            {
                locationEntity.LocationName = locationName;
            }

                locationEntity.LocalLevel = locationLevel;

            //call search action and get dataset
            _returnDs = locationEntity.SearchLocation();

            if (locationEntity.ErrorMsg != "")
            {
                //show message “查询数据出错！” 
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.EMS.LocationSearchDialog.MsgQueryDataFailure}"));
            }
            else
            {
                //gridView1中的数据用返回数据集的FMM_LOCATION表填充
                this.gridView1.GridControl.DataSource = _returnDs.Tables[FMM_LOCATION_FIELDS.DATABASE_TABLE_NAME];
            }
            
            #endregion
        }
        #endregion

        #region double click
        /// <summary>
        /// double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        #endregion


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool MapSelectedItemToProperties()
        {
            //获取行焦点 
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                //获取行单元格LOCATION_KEY数据 
                pubLocationKey = gridView1.GetRowCellValue(rowHandle, FMM_LOCATION_FIELDS.FIELD_LOCATION_KEY).ToString();
                //获取行单元格LOCATION_NAME数据 
                pubLocationName = gridView1.GetRowCellValue(rowHandle, FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME).ToString();
                return true;
            }
            return false;
        }

        private void LocationSearchDialog_Load(object sender, EventArgs e)
        {
            GridViewHelper.SetGridView(gridView1);
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.CommonControls.Dialogs;

namespace FanHai.Hemera.Addins.FMM
{
    public partial class PartSearch : BaseDialog
    {
        #region define variables
        private string _partKey =string.Empty;
        private string _partName = string.Empty;

        private string _partType = string.Empty;
        private string _partModule = string.Empty;
        private string _partClass = string.Empty;

        #endregion

        #region Properties
        public string PartKey
        {
            get
            {
                return _partKey;
            }
            set
            {
                _partKey = value;
            }
        }
        public string PartName
        {
            get
            {
                return _partName;
            }
            set
            {
                _partName = value;
            }
        }
        public string PartType
        {
            get
            {
                return _partType;
            }
            set
            {
                _partType = value;
            }
        }
        public string PartModule
        {
            get
            {
                return _partModule;
            }
            set
            {
                _partModule = value;
            }
        }
        public string PartClass
        {
            get
            {
                return _partClass;
            }
            set
            {
                _partClass = value;
            }
        }

        #endregion
        public PartSearch()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartSearch.Title}"))
        {//成品查询
            InitializeComponent();
            //控件命名
            InitUI();
        }

        /// <summary>
        /// 控件命名
        /// </summary>
        private void InitUI()
        {
            this.lblMatNumber.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.MatNumber}");
            this.part_number.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.MatNumber}");
            this.part_revision.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.PartEditor.MatVersion}");
            this.description.Caption = StringParser.Parse("${res:Global.Description}");
            this.btnSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
        }

        /// <summary>
        /// bind data to gridview
        /// </summary>
        private void BindData(string partName)
        {
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (serverFactory != null)
                {
                    DataSet ds = new DataSet();
                    //查询成品数据通过物料号
                    ds = serverFactory.CreateIPartEngine().SearchPart(partName);
                    string msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(ds);
                    if (msg != "")
                    {//查询出错！
                        MessageService.ShowError("${res:FanHai.Hemera.Addins.Msg.SearchFailed}");
                    }
                    else
                    {
                        if (ds.Tables.Count>0)
                        {
                            gridControl1.MainView = gridView1;
                            gridControl1.DataSource = ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        /// <summary>
        /// Search button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            if (this.txtPart.Text != string.Empty)
            {//物料号不为空 
                _partName = this.txtPart.Text;
            }
            //绑定数据表数据参数值_partName为物料号 
        }        

        /// <summary>
        /// Showing Editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// gridControl Double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {//返回值为true
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {//返回值为true
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取行数据PART_KEY和PART_NAME
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle >= 0)
            {
                _partKey = this.gridView1.GetRowCellValue(rowHandle, POR_PART_FIELDS.FIELD_PART_KEY).ToString();
                _partName = this.gridView1.GetRowCellValue(rowHandle, POR_PART_FIELDS.FIELD_PART_NAME).ToString();
                _partType = this.gridView1.GetRowCellValue(rowHandle, "PART_TYPE").ToString();
                _partModule = this.gridView1.GetRowCellValue(rowHandle, "PART_MODULE").ToString();
                _partClass = this.gridView1.GetRowCellValue(rowHandle, "PART_CLASS").ToString();
                return true;
            }
            return false;
        }
    }
}


using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 参数查询对话框。
    /// </summary>
    public partial class ParamSearchDialog : BaseDialog
    {
        /// <summary>
        /// 参数主键。
        /// </summary>
        private string _paramKey = string.Empty;
        /// <summary>
        /// 参数名称。
        /// </summary>
        private string _paramName = string.Empty;
        /// <summary>
        /// 参数类别。
        /// </summary>
        private string _category = string.Empty;
        /// <summary>
        /// 参数名称。
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }
        /// <summary>
        /// 参数主键。
        /// </summary>
        public string ParamKey
        {
            get { return _paramKey; }
            set { _paramKey = value; }
        }
        /// <summary>
        /// 参数数据类型。
        /// </summary>
        public int ParamDataType
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ParamSearchDialog(string category)
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamSearchDialog.Title}"))
        {
            InitializeComponent();
            this._category = category;
            this._paramKey = string.Empty;
            this._paramName = string.Empty;
        }

        public ParamSearchDialog()
            : base(StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.ParamSearchDialog.Title}"))
        {
            InitializeComponent();
            this._category = string.Empty;
            this._paramKey = string.Empty;
            this._paramName = string.Empty;
        }
        /// <summary>
        /// 查询参数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable htParams = new Hashtable();
                DataSet dsParams = new DataSet();

                string paramName = this.txtParamName.Text.Trim();

                if (paramName.Length > 0)
                {
                    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME, paramName);
                }
                if (!string.IsNullOrEmpty(this._category))
                {
                    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_CATEGORY, this._category);
                    htParams.Add(BASE_PARAMETER_FIELDS.FIELD_STATUS, "1");
                }
                DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
                dtParams.TableName = BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME;
                dsParams.Tables.Add(dtParams);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateIParamEngine().SearchParam(dsParams);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        gcParamList.MainView = gvParamList;
                        gcParamList.DataSource = dsReturn.Tables[BASE_PARAMETER_FIELDS.DATABASE_TABLE_NAME];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }
        /// <summary>
        /// 双击选择行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvParamList_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 确定按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// 映射选择项。
        /// </summary>
        /// <returns></returns>
        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gvParamList.GetDataRowHandleByGroupRowHandle(gvParamList.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                DataRow drParamter=gvParamList.GetDataRow(rowHandle);
                _paramKey = Convert.ToString(drParamter[BASE_PARAMETER_FIELDS.FIELD_PARAM_KEY]);
                _paramName = Convert.ToString(drParamter[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME]);
                this.ParamDataType = Convert.ToInt32(drParamter[BASE_PARAMETER_FIELDS.FIELD_DATA_TYPE]);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取消按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗台载入事件。
        /// </summary>
        private void ParamSearchDialog_Load(object sender, EventArgs e)
        {
            this.btnQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.btnConfirm.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.CancelButtonText}");
            this.gridColumn_paramName.Caption = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_paramDescription.Caption = StringParser.Parse("${res:Global.Description}");
        }

    }
}

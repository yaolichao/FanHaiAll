using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.StaticFuncs;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Entities;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 原因代码组查询帮助对话框。
    /// </summary>
    public partial class ReasonCodeCategoryQueryHelpDialog : XtraForm
    {
        /// <summary>
        /// 原因代码组查询操作所需参数。
        /// </summary>
        private ReasonCodeCategoryQueryHelpModel _model;
        /// <summary>
        /// 值被选中事件。
        /// </summary>
        public event ReasonCodeCategoryQueryValueSelectedEventHandler OnValueSelected;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="transactType"></param>
        public ReasonCodeCategoryQueryHelpDialog(ReasonCodeCategoryQueryHelpModel model)
        {
            InitializeComponent();
            this._model = model;
        }
        /// <summary>
        /// 查询按钮。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSearch_Click(object sender, EventArgs e)
        {
            BindQueryResult();
        }
        /// <summary>
        /// 绑定查询结果。
        /// </summary>
        private void BindQueryResult()
        {
            ReasonCodeCategoryEntity entity = new ReasonCodeCategoryEntity();
            string name = this.txName.Text;
            //S:是报废代码类别 D：是不良代码类别
            string type = this._model.QueryType == ReasonCodeCategoryQueryType.Scrap ? "S" : "D";
            DataTable dtParam = entity.GetReasonCodeCategoryParamTable(name,type);
            DataTable dtReturn = entity.GetReasonCodeCategory(dtParam);
            if (!string.IsNullOrEmpty(entity.ErrorMsg))
            {
                MessageService.ShowError(entity.ErrorMsg);
                return;
            }
            this.gcResult.DataSource = dtReturn;
            this.gcResult.MainView = this.gvResult;
        }
        /// <summary>
        /// 双击选择批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            if (gvResult.FocusedRowHandle >= 0)
            {
                if (this.OnValueSelected != null)
                {
                    ReasonCodeCategoryQueryValueSelectedEventArgs args = new ReasonCodeCategoryQueryValueSelectedEventArgs();
                    args.ReasonCodeCategoryKey = Convert.ToString(gvResult.GetFocusedRowCellValue(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_KEY));
                    args.ReasonCodeCategoryName = Convert.ToString(gvResult.GetFocusedRowCellValue(FMM_REASON_CODE_CATEGORY_FIELDS.FIELD_REASON_CODE_CATEGORY_NAME));
                    this.OnValueSelected(sender, args);
                    if (args.Cancel == true)
                    {
                        return;
                    }
                }
            }
            this.Visible = false;
            this.Close();
        }
        /// <summary>
        /// 触发非激活事件，隐藏并关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReasonCodeQueryHelpDialog_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void ReasonCodeQueryHelpDialog_Load(object sender, EventArgs e)
        {
            this.btSearch.Text = StringParser.Parse("${res:Global.Query}");
            this.gclName.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Name}");
            this.gclDesc.Caption =
                StringParser.Parse("${res:FanHai.Hemera.Addins.FMM.SearchDialog.gridColumn_Description}");
        }
        /// <summary>
        /// 显示编辑器事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }

    /// <summary>
    /// 原因代码组查询枚举类型。
    /// </summary>
    public enum ReasonCodeCategoryQueryType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 不良代码组查询。
        /// </summary>
        Defect,
        /// <summary>
        /// 报废代码组查询。
        /// </summary>
        Scrap,
    }

    /// <summary>
    /// 查询原因代码组的参数数据
    /// </summary>
    public class ReasonCodeCategoryQueryHelpModel
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReasonCodeCategoryQueryHelpModel()
        {
            this.QueryType = ReasonCodeCategoryQueryType.None;
        }
        /// <summary>
        /// 原因代码组查询类型。
        /// </summary>
        public ReasonCodeCategoryQueryType QueryType { get; set; }
    }
    /// <summary>
    /// 原因代码组查询被选中时的参数类。
    /// </summary>
    public class ReasonCodeCategoryQueryValueSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 原因代码组名称。
        /// </summary>
        public string ReasonCodeCategoryName { get; set; }
        /// <summary>
        /// 原因代码组主键。
        /// </summary>
        public string ReasonCodeCategoryKey { get; set; }
        /// <summary>
        /// 是否取消选中值。
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReasonCodeCategoryQueryValueSelectedEventArgs()
        {
            this.Cancel = false;
        }
    }
    /// <summary>
    /// 原因代码组查询被选中的事件委托类。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReasonCodeCategoryQueryValueSelectedEventHandler(object sender, ReasonCodeCategoryQueryValueSelectedEventArgs e);
}

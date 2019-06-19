
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
using FanHai.Hemera.Utils.UDA;
using System.Collections;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 管理原因代码的控件类。
    /// </summary>
    public partial class ReasonCodeCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 原因代码管理对象。
        /// </summary>
        private ReasonCode _reasonCode = new ReasonCode();
        /// <summary>
        /// 状态改变事件委托。
        /// </summary>
        /// <param name="controlState"></param>
        public new delegate void AfterStateChanged(ControlState controlState);
        /// <summary>
        /// 状态改变事件。
        /// </summary>
        public new AfterStateChanged afterStateChanged = null;
        /// <summary>
        /// 控件状态。
        /// </summary>
        private ControlState _ctrlState = ControlState.Empty;
        /// <summary>
        /// 控件状态。
        /// </summary>
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// 当控件状态改变时触发的方法。
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            tsbNew.Enabled = true;
            switch (state)
            {
                case ControlState.Empty:
                    txtEditCodeName.Text = string.Empty;
                    txtEditCodeDescription.Text = string.Empty;

                    txtEditCodeName.Enabled = false;
                    txtEditCodeDescription.Enabled = false;

                    tsbSave.Enabled = false;
                    tsbDel.Enabled = false;
                    break;
                case ControlState.Edit:
                    txtEditCodeName.Enabled = true;
                    txtEditCodeDescription.Enabled = true;

                    tsbSave.Enabled = true;
                    tsbDel.Enabled = true;
                    break;
                case ControlState.New:
                    txtEditCodeName.Text = string.Empty;
                    txtEditCodeDescription.Text = string.Empty;
                    lueType.EditValue = string.Empty;

                    txtEditCodeName.Enabled = true;
                    txtEditCodeDescription.Enabled = true;

                    tsbSave.Enabled = true;
                    tsbDel.Enabled = false;
                    break;
                case ControlState.ReadOnly:

                    txtEditCodeName.Enabled = false;
                    txtEditCodeDescription.Enabled = false;

                    tsbSave.Enabled = false;
                    tsbDel.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ReasonCodeCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);

            InitializeLanguage();
            GridViewHelper.SetGridView(gridViewCode);
        }
        public void InitializeLanguage()
        {
            this.tsbRefresh.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0001}");//刷新
            this.tsbNew.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0002}");//新增
            this.tsbSave.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0003}");//保存
            this.tsbDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0004}");//删除

            lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0005}");//代码管理
            lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0006}");//名称
            lblType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0007}");//类型
            lciQueryClass.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0008}");//分类
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0009}");//查询
            //layoutControl1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0006}");//名称
            //lblCategoryType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0007}");//类型
            //lciClass.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0008}");//分类
            //lblCodeDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0010}");//描述
            gcolCodeName.Caption= StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0006}");//名称
            gcolCodeType.Caption= StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0007}");//类型
            gcolClass.Caption= StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0008}");//分类
            gcolDescription.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.lbl.0010}");//描述
            this.lblMenu.Text = "质量管理>代码管理>代码管理";

        }
        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        private void ReasonCodeCtrl_Load(object sender, EventArgs e)
        {
            BindReasonCodeType();
            BindReasonCodeClass();
            CodeGridDataBind();
        }
        /// <summary>
        /// 绑定类型数据到下拉列表数据
        /// </summary>
        public void BindReasonCodeType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            DataTable dtType=BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_TypeOfRCode);
            if (dtType != null)
            {

                this.lueType.Properties.DataSource = dtType;
                this.lueType.Properties.DisplayMember = "NAME";
                this.lueType.Properties.ValueMember = "CODE";

                DataTable dt = dtType.Copy();
                dt.Rows.InsertAt(dt.NewRow(), 0);
                this.lueQueryType.Properties.DataSource = dt;
                this.lueQueryType.Properties.DisplayMember = "NAME";
                this.lueQueryType.Properties.ValueMember = "CODE";
            }
        }
        /// <summary>
        /// 绑定原因代码分类数据到下拉列表数据
        /// </summary>
        public void BindReasonCodeClass()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            DataTable dtReturn = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_ClassOfRCode);
            if (dtReturn != null)
            {
                dtReturn.DefaultView.Sort = "CODE ASC";
                this.lueClass.Properties.DataSource = dtReturn;
                this.lueClass.Properties.ValueMember = "CODE";
                this.lueClass.Properties.DisplayMember = "NAME";
                DataTable dt = dtReturn.Copy();
                dt.Rows.InsertAt(dt.NewRow(), 0);
                this.lueQueryClass.Properties.DataSource = dt;
                this.lueQueryClass.Properties.ValueMember = "CODE";
                this.lueQueryClass.Properties.DisplayMember = "NAME";
            }
        }
        /// <summary>
        /// 绑定原因代码。
        /// </summary>
        public void CodeGridDataBind()
        {
            grdCrtlCode.MainView = gridViewCode;
            grdCrtlCode.DataSource = _reasonCode.GetReasonCode(null);
        }

        /// <summary>
        /// 查询按钮事件。
        /// </summary>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Hashtable htParams = new Hashtable();
            htParams.Add(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_NAME, txtName.EditValue);
            string codeType = Convert.ToString(this.lueQueryType.EditValue);
            string codeClass = Convert.ToString(this.lueQueryClass.EditValue);
            if (!string.IsNullOrEmpty(codeType))
            {
                htParams.Add(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_TYPE, codeType);
            }
            if (!string.IsNullOrEmpty(codeClass))
            {
                htParams.Add(FMM_REASON_CODE_FIELDS.FIELD_REASON_CODE_CLASS, codeClass);
            }
            DataTable paramTable = CommonUtils.ParseToDataTable(htParams);
            if (null != paramTable)
            {
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = _reasonCode.GetReasonCode(paramTable);
            }
        }
        /// <summary>
        /// 刷新按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            CodeGridDataBind();
        }
        /// <summary>
        /// 新增按钮事件。
        /// </summary>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            _reasonCode = new ReasonCode(CommonUtils.GenerateNewKey(0));
            CtrlState = ControlState.New;
        }
        /// <summary>
        /// 保存按钮事件。
        /// </summary>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            //你确定要保存当前界面的数据吗？
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                MapControlsToReasonCode();
                bool IsTrue = false;
                //名称不为空
                if (_reasonCode.CodeName == string.Empty)
                {
                    //MessageService.ShowMessage("原因代码名称不能为空。", StringParser.Parse("${res:Global.SystemInfo}"));    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));  
                    return;
                }
                //名称+类型已经存在
                if (CtrlState == ControlState.New && !_reasonCode.ReasonCodeNameValidate())
                {
                    //MessageService.ShowMessage("原因代码类型+原因代码分类+原因代码名称已经存在，请确认。", "${res:Global.SystemInfo}");      //当前名称已存在!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    return;
                }
                //状态为new
                if (CtrlState == ControlState.New)
                {
                    _reasonCode.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                    _reasonCode.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                    //新增成功
                    if (_reasonCode.Insert())
                    {
                        IsTrue = true;
                    }
                }
                else//状态不为new
                {
                    //修改成功
                    if (_reasonCode.Update())
                    {
                        IsTrue = true;
                    }
                }
                //值为true
                if (IsTrue)
                {
                    CodeGridDataBind();                                //数据表数据重新绑定
                    CtrlState = ControlState.ReadOnly;                 //状态为readonly
                }
            }
        }
        /// <summary>
        /// 验证并收集原因代码数据。
        /// </summary>
        private void MapControlsToReasonCode()
        {
            if (null == _reasonCode)
            {
                throw (new Exception("Error Reason Code Set"));
            }
            // TODO: Data validation
            _reasonCode.CodeName = txtEditCodeName.Text;
            _reasonCode.CodeDescriptions = txtEditCodeDescription.Text;
            _reasonCode.CodeType =  lueType.EditValue.ToString();
            _reasonCode.CodeClass = Convert.ToString(this.lueClass.EditValue);
        }
        /// <summary>
        /// 删除按钮事件。
        /// </summary>
        private void tsbDel_Click(object sender, EventArgs e)
        {
            //if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {//系统提示你确定要删除吗？
                if (_reasonCode.Delete())
                {
                    //数据表数据重新绑定 modi my chao.pang
                    CodeGridDataBind();
                    CtrlState = ControlState.Empty;
                }
            }
        }
        /// <summary>
        /// 原因代码记录单击事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCode_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            CtrlState = ControlState.Edit;
            _reasonCode = new ReasonCode(this.gridViewCode.GetRowCellValue(e.RowHandle, this.gcolCodeKey).ToString());
            this.txtEditCodeName.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, this.gcolCodeName).ToString();
            this.txtEditCodeDescription.Text = Convert.ToString(this.gridViewCode.GetRowCellValue(e.RowHandle, this.gcolDescription));
            this.lueType.EditValue = Convert.ToString(this.gridViewCode.GetRowCellValue(e.RowHandle, this.gcolCodeType));
            this.lueClass.EditValue = Convert.ToString(this.gridViewCode.GetRowCellValue(e.RowHandle, this.gcolClass));
            _reasonCode.CodeName = this.txtEditCodeName.Text;
            _reasonCode.CodeType = this.lueType.EditValue.ToString();
            _reasonCode.CodeDescriptions = this.txtEditCodeDescription.Text;
            _reasonCode.CodeClass = Convert.ToString(this.lueClass.EditValue);
            _reasonCode.Editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            _reasonCode.EditTimeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
            _reasonCode.ResetDirtyList();
        }
        /// <summary>
        /// 自定义绘制单元格文本。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCode_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolCodeType)
            {
                DataTable dtCodeType = this.lueType.Properties.DataSource as DataTable;
                if (dtCodeType != null)
                {
                    DataRow[] drs = dtCodeType.Select(string.Format("CODE='{0}'", e.CellValue));
                    if (drs != null && drs.Length > 0)
                    {
                        e.DisplayText = Convert.ToString(drs[0]["NAME"]);
                    }
                }
            }
            else if (e.Column == this.gcolClass)
            {
                DataTable dtClass= this.lueClass.Properties.DataSource as DataTable;
                if (dtClass != null)
                {
                    DataRow[] drs= dtClass.Select(string.Format("CODE='{0}'", e.CellValue));
                    if (drs != null && drs.Length > 0)
                    {
                        e.DisplayText = Convert.ToString(drs[0]["NAME"]);
                    }
                }
            }
        }
    }
}

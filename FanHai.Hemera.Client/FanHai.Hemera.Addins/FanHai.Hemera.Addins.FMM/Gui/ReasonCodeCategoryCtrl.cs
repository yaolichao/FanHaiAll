
#region using
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
#endregion

namespace FanHai.Hemera.Addins.FMM
{
    /// <summary>
    /// 显示代码组管理界面的用户控件类。
    /// </summary>
    public partial class ReasonCodeCategoryCtrl : BaseUserCtrl
    {
#pragma warning disable 
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
#pragma warning restore
        //Define and initialize control state
        private ControlState _ctrlState = ControlState.Empty;

        //Control state property
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
        /// Construct function
        /// </summary>
        public ReasonCodeCategoryCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            GridViewHelper.SetGridView(gridViewCode);
            GridViewHelper.SetGridView(gridViewCodeCategory);
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            tsbNew.Enabled = true;
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtCategoryName.Text = string.Empty;
                    txtDescription.Text = string.Empty;

                    txtCategoryName.Enabled = false;
                    txtDescription.Enabled = false;                   
                    
                    tsbSave.Enabled = false;
                    tsbDel.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtCategoryName.Enabled = true;
                    txtDescription.Enabled = true;
                    
                    tsbSave.Enabled = true; ;
                    tsbDel.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtCategoryName.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    lpCategoryType.EditValue = string.Empty;

                    txtCategoryName.Enabled = true;
                    txtDescription.Enabled = true;

                    tsbSave.Enabled = true;
                    tsbDel.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    txtCategoryName.Enabled = false;
                    txtDescription.Enabled = false;
                    
                    tsbSave.Enabled = false;
                    tsbDel.Enabled = true;
                    break;
                #endregion
            }
        }

        /// <summary>
        /// The ReasonCodeCategory Form load data
        /// </summary>
        private void ReasonCodeCategoryCtrl_Load(object sender, EventArgs e)
        {
            CodeGridDataBind();
            BindCategoryType();
            LoadResourceFileToUI();
        }

        /// <summary>
        /// Bind CategoryType Data
        /// </summary>
        public void BindCategoryType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            DataTable dtType = BaseData.Get(columns, BASEDATA_CATEGORY_NAME.Basic_TypeOfRCode);
            if (dtType != null)
            {
                this.lpCategoryType.Properties.DataSource = dtType;
                this.lpCategoryType.Properties.DisplayMember = "NAME";
                this.lpCategoryType.Properties.ValueMember = "CODE";

                DataTable dt = dtType.Copy();
                dt.Rows.InsertAt(dt.NewRow(), 0);
                this.lpType.Properties.DataSource = dt;
                this.lpType.Properties.DisplayMember = "NAME";
                this.lpType.Properties.ValueMember = "CODE";

                this.lpItemCategory.DataSource = dtType;
                this.lpItemCategory.DisplayMember = "NAME";
                this.lpItemCategory.ValueMember = "CODE";
            }
        }

        #region Reason code bind data
        /// <summary>
        /// Reason code bind data
        /// </summary>
        public void CodeGridDataBind()
        {
            gridControlCodeCategory.MainView = gridViewCodeCategory;
            gridControlCodeCategory.DataSource = _reasonCodeCategory.GetReasonCodeCategory(null);
        }
        #endregion

        #region Query button click event
        /// <summary>
        /// Query button click event
        /// </summary>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string type=this.lpType.EditValue==null?string.Empty:this.lpType.EditValue.ToString();
            //根据名称和类型 
            DataTable paramTable = _reasonCodeCategory.GetReasonCodeCategoryParamTable(this.txtName.Text, type);
            gridControlCodeCategory.DataSource = _reasonCodeCategory.GetReasonCodeCategory(paramTable);
        }
        #endregion


        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            CodeGridDataBind();
        }

        /// <summary>
        /// Tool Bar New Click 工具栏新增按钮单击事件 
        /// </summary>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            _reasonCodeCategory = new ReasonCodeCategoryEntity(CommonUtils.GenerateNewKey(0));
            //状态保存为new 
            CtrlState = ControlState.New;
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {//系统提示你确定要保存当前界面的数据吗？
                MapControlsToReasonCode();

                bool IsTrue = false;

                if (_reasonCodeCategory.CodeCategoryName == string.Empty)
                {//判定名称为空
                    //系统提示名称不能为空
                    MessageService.ShowMessage("${res:Global.NameNotNullMessage}", "${res:Global.SystemInfo}");
                    return;
                }

                if (CtrlState == ControlState.New && !_reasonCodeCategory.CodeCategoryNameValidate())
                {//判定状态为new,且名称存在
                    //系统提示没有可更新项
                    MessageService.ShowMessage("${res:Global.NameIsExistMessage}", "${res:Global.SystemInfo}");
                    return;
                }

                //判断状态为new执行 
                if (CtrlState == ControlState.New)
                {
                    if (_reasonCodeCategory.Insert())    //新增成功返回true
                    {
                        IsTrue = true;
                    }
                }
                //状态不为new执行 
                else
                {
                    //更新数据
                    if (_reasonCodeCategory.Update())    //修改成功返回为true
                    {
                        IsTrue = true;                   
                    }
                }

                if (IsTrue)
                {
                    //数据表数据重新绑定 
                    CodeGridDataBind();
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (_reasonCodeCategory.Delete())
            {
                CodeGridDataBind();
                //清空状态 
                CtrlState = ControlState.Empty;
            }
            return;
        }

        #region Validation & Set Controls Data To Reason Code
        /// <summary>
        /// Validation & Collection Data for Sales Order
        /// </summary>
        private void MapControlsToReasonCode()
        {
            if (null == _reasonCodeCategory)
            {
                throw (new Exception("Error Reason Code Set"));
            }
            // TODO: Data validation
            _reasonCodeCategory.CodeCategoryName = txtCategoryName.Text;
            _reasonCodeCategory.CodeCategoryDescriptions = txtDescription.Text;
            _reasonCodeCategory.CodeCategoryType = lpCategoryType.EditValue.ToString();
        }
        #endregion
        /// <summary>
        /// 行单击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCodeCategory_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            //修改状态edit  
            CtrlState = ControlState.Edit;
            _reasonCodeCategory = new ReasonCodeCategoryEntity(this.gridViewCodeCategory.GetRowCellValue(e.RowHandle, this.gridColumn_CategoryKey).ToString());
            this.txtCategoryName.Text = this.gridViewCodeCategory.GetRowCellValue(e.RowHandle, this.gridColumn_CategoryName).ToString();
            this.txtDescription.Text = this.gridViewCodeCategory.GetRowCellValue(e.RowHandle, this.gridColumn_Descriptions).ToString();
            this.lpCategoryType.EditValue = this.gridViewCodeCategory.GetRowCellValue(e.RowHandle, this.gridColumn_CategoryType).ToString();

            _reasonCodeCategory.CodeCategoryName = this.txtCategoryName.Text;
            _reasonCodeCategory.CodeCategoryType = this.lpCategoryType.EditValue.ToString();
            _reasonCodeCategory.CodeCategoryDescriptions = this.txtDescription.Text;
            _reasonCodeCategory.ResetDirtyList();
        }

        private void gridViewCodeCategory_DoubleClick(object sender, EventArgs e)
        {
            ReasonCodeDialog codeDialog = new ReasonCodeDialog(_reasonCodeCategory);
            codeDialog.StartPosition = FormStartPosition.CenterParent;
            codeDialog.ShowDialog();
        }

        #region Load resource file to UI
        /// <summary>
        /// Load resource file to UI
        /// </summary>
        private void LoadResourceFileToUI()
        {
            //this.tsbRefresh.Image = (Image)ResourceService.GetImageResource("Icons.16x16.Refresh");
            this.tsbRefresh.Text = StringParser.Parse("${res:Global.Refresh}");
            this.tsbNew.Text = StringParser.Parse("${res:Global.New}");
            this.tsbSave.Text = StringParser.Parse("${res:Global.Save}");
            this.tsbDel.Text = StringParser.Parse("${res:Global.Delete}");
            //this.lblCategoryName.Text = StringParser.Parse("${res:Global.NameText}");
            //this.lblDescription.Text = StringParser.Parse("${res:Global.Description}");
            //this.lblCategoryType.Text = StringParser.Parse("${res:Global.Category}");
            this.lblName.Text = StringParser.Parse("${res:Global.NameText}");
            this.lblType.Text = StringParser.Parse("${res:Global.Category}");
            this.gridColumn_CategoryName.Caption = StringParser.Parse("${res:Global.NameText}");
            this.gridColumn_Descriptions.Caption = StringParser.Parse("${res:Global.Description}");
            this.gridColumn_CategoryType.Caption = StringParser.Parse("${res:Global.Category}");
            //lblMenu.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReasonCodeCategoryCtrl.lbl.0001}");//代码组管理
            lblMenu.Text = "质量管理>代码管理>代码组";
        }
        #endregion

        #region Private variable definition
        private ReasonCodeCategoryEntity _reasonCodeCategory = new ReasonCodeCategoryEntity();
        #endregion

        private void gridViewCodeCategory_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridViewCode_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        
    }
}

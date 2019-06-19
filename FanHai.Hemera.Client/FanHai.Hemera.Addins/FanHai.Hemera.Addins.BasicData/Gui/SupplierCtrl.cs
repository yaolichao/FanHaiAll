using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities.BasicData;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{
    /// <summary>
    /// 管理供应商的控件类。
    /// </summary>
    public partial class SupplierCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 供应商管理对象。
        /// </summary>
        private SupplierEntity _supplierEntity = new SupplierEntity();
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
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            BtnAdd.Enabled = true;
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtEditCode.Text = string.Empty;
                    txtEditName.Text = string.Empty;
                    txtEditShortName.Text = string.Empty;

                    txtEditCode.Enabled = false;
                    txtEditName.Enabled = false;
                    txtEditShortName.Enabled = false;

                    BtnSave.Enabled = false;
                    BtnDelete.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:

                    txtEditCode.Enabled = true;
                    txtEditName.Enabled = true;
                    txtEditShortName.Enabled = true;

                    BtnSave.Enabled = true;
                    BtnDelete.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtEditCode.Text = string.Empty;
                    txtEditName.Text = string.Empty;
                    txtEditShortName.Text = string.Empty;
                    lblCode.Text = string.Empty;

                    txtEditCode.Enabled = true;
                    txtEditName.Enabled = true;
                    txtEditShortName.Enabled = true;

                    BtnSave.Enabled = true;
                    BtnDelete.Enabled = false;
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    txtEditCode.Enabled = false;
                    txtEditName.Enabled = false;
                    txtEditShortName.Enabled = false;

                    BtnSave.Enabled = false;
                    BtnDelete.Enabled = true;
                    break;
                #endregion
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public SupplierCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            GridViewHelper.SetGridView(gridViewCode);
        }
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        /// <summary>
        /// 绑定供应商代码
        /// </summary>
        public void BindSupplierCode()
        {
            DataSet dataSet = new DataSet();
            SupplierEntity supplierEntity = new SupplierEntity();
            dataSet = supplierEntity.GetSupplierCode();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0].Copy();
                dt.Rows.InsertAt(dt.NewRow(), 0);
                this.lueCode.Properties.DataSource = dt;
                this.lueCode.Properties.DisplayMember = "CODE";
                this.lueCode.Properties.ValueMember = "CODE";
                lueCode.ItemIndex = 0;

                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
        }
        private void MapControlsToSupplier()
        {
            if (null == _supplierEntity)
            {
                throw (new Exception("Error Reason Code Set"));
            }
            _supplierEntity.SupplierCode = txtEditCode.Text.Trim();
            _supplierEntity.SupplierName = txtEditName.Text.Trim();
            _supplierEntity.SupplierNickName = txtEditShortName.Text.Trim();
        }

        //刷新按钮
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            BindSupplierCode();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            _supplierEntity = new SupplierEntity();
            CtrlState = ControlState.New;
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("确定删除吗?", "系统提示"))
            {//系统提示你确定要删除吗？
                if (_supplierEntity.Delete())
                {
                    //数据表数据重新绑定 modi my chao.pang
                    BindSupplierCode();
                    CtrlState = ControlState.Empty;
                }
            }
        }
        /// <summary>
        /// 行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCode_RowClick(object sender, RowClickEventArgs e)
        {
            CtrlState = ControlState.Edit;

            _supplierEntity = new SupplierEntity(this.gridViewCode.GetRowCellValue(e.RowHandle, "CODE").ToString());
            this.txtEditName.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "NAME").ToString();
            this.txtEditCode.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "CODE").ToString();
            this.txtEditShortName.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "NICKNAME").ToString();
            this.lblCode.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "CODE").ToString();

            _supplierEntity.SupplierName = this.txtEditName.Text;
            _supplierEntity.SupplierCode = this.txtEditCode.Text;
            _supplierEntity.SupplierNickName = this.txtEditShortName.Text;
            _supplierEntity.ResetDirtyList();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            SupplierEntity supplierEntity = new SupplierEntity();
            dataSet = supplierEntity.GetSupplierCode(txtName.Text.ToString().Trim(), lueCode.EditValue.ToString());
            if (dataSet != null)
            {
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            {
                MapControlsToSupplier();
                bool IsTrue = false;

                if (_supplierEntity.SupplierCode == string.Empty)
                {
                    MessageService.ShowMessage("供应商代码不为空", "保存");    //名称不能为空!
                    return;
                }

                if (_supplierEntity.SupplierName == string.Empty)
                {
                    MessageService.ShowMessage("供应商名称不为空", "保存");    //名称不能为空!
                    return;
                }
                if (_supplierEntity.SupplierNickName == string.Empty)
                {
                    MessageService.ShowMessage("简称不为空", "保存");    //名称不能为空!
                    return;
                }

                if (CtrlState == ControlState.New && _supplierEntity.GetSupplierCode(_supplierEntity.SupplierName, "").Tables[0].Rows.Count > 0)
                {
                    MessageService.ShowMessage("当前供应商代码已经存在,请重新设置", "保存");      //当前名称已存在!
                    return;
                }

                if (CtrlState == ControlState.New)
                {//状态为new
                    if (_supplierEntity.Insert())
                    {//新增成功
                        IsTrue = true;
                    }
                }
                else
                {//状态不为new
                    if (lblCode.Text.ToString() != "")
                    {
                        if (_supplierEntity.Update(lblCode.Text.ToString()))
                        {//修改成功
                            IsTrue = true;
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage("请选择要修改的供应商信息", "保存");      //当前名称已存在!
                    }
                
                }

                if (IsTrue)
                {//值为true
                    BindSupplierCode();                                //数据表数据重新绑定
                    CtrlState = ControlState.ReadOnly;                 //状态为readonly
                }
            }
        }

        private void SupplierCtrl_Load(object sender, EventArgs e)
        {
            BindSupplierCode();
        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}
 
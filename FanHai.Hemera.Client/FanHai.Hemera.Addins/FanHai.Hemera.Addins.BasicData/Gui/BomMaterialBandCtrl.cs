
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
    /// 管理物料维护的控件类。
    /// </summary>
    public partial class BomMaterialBandCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 物料对象。
        /// </summary>
        private BomMaterialBandEntity _bomMaterialBandEntity = new BomMaterialBandEntity();
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
                    txtName.Text = string.Empty;
                    txtBarcode.Text = string.Empty;
                    txtDesc.Text = string.Empty;

                    txtName.Enabled = false;
                    txtCode.Enabled = false;
                    txtBarcode.Enabled = false;
                    txtDesc.Enabled = false;

                    BtnSave.Enabled = false;
                    BtnDelete.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtCode.Enabled = false;
                    txtName.Enabled = true;
                    txtBarcode.Enabled = true;
                    txtDesc.Enabled = true;
                    txtName.Focus();
                    BtnSave.Enabled = true;
                    BtnDelete.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtName.Text = string.Empty;
                    txtBarcode.Text = string.Empty;
                    txtDesc.Text = string.Empty;

                    txtName.Enabled = true;
                    txtCode.Enabled = true;
                    txtBarcode.Enabled = true;
                    txtDesc.Enabled = true;

                    BtnSave.Enabled = true;
                    BtnDelete.Enabled = false;
                    txtCode.Focus();
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    txtName.Enabled = false;
                    txtCode.Enabled = false;
                    txtBarcode.Enabled = false;
                    txtDesc.Enabled = false;

                    BtnSave.Enabled = false;
                    BtnDelete.Enabled = true;
                    break;
                #endregion
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public BomMaterialBandCtrl()
        {
            InitializeComponent();

            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            InitializeLanguage();
            GridViewHelper.SetGridView(gridViewCode);
        }
        public void InitializeLanguage()
        {
            lblName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0005}"); //物料料号
            lciCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0006}");//物料代码
            btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0007}");//查询
            lblCodeName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0008}");//物料料号
            layoutControlItem1.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0009}");//物料名称
            lblCodeDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0010}");//物料代码
            layoutControlItem2.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0011}");//物料描述
            gcCode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0008}");//物料料号
            gcName.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0009}");//物料名称
            gcBarcode.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0010}");//物料代码
            gcDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.lbl.0011}");//物料描述
            gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.GridControl.0001}");//创建人
            gcCreatTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.GridControl.0002}");//创建时间
            gcEditer.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.GridControl.0003}");//最后修改人
            gcEditTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.GridControl.0004}");//最后修改时间
        }
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            _bomMaterialBandEntity = new BomMaterialBandEntity();
            CtrlState = ControlState.New;
        }

        private void tsbDel_Click(object sender, EventArgs e)
        {
            //if (MessageService.AskQuestion("确定删除吗?", "系统提示"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {//系统提示你确定要删除吗？
                if (_bomMaterialBandEntity.Delete())
                {
                    //数据表数据重新绑定 modi my chao.pang
                    BandCode();
                    BandView();
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

            _bomMaterialBandEntity = new BomMaterialBandEntity(this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_CODE").ToString());
            this.txtCode.EditValue = this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_CODE").ToString();
            this.txtName.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_NAME").ToString();
            this.txtBarcode.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "BARCODE").ToString();
            this.txtDesc.Text = this.gridViewCode.GetRowCellValue(e.RowHandle, "MATERIAL_SPEC").ToString();

            _bomMaterialBandEntity.Code = this.txtCode.Text;
            _bomMaterialBandEntity.Name = this.txtName.Text;
            _bomMaterialBandEntity.BarCode = this.txtBarcode.Text;
            _bomMaterialBandEntity.Desc = this.txtBarcode.Text;
            _bomMaterialBandEntity.ResetDirtyList();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            string code = txtSelCode.Text.Trim();
            string barCode = txtSelBarcode.Text.Trim();
            dataSet = _bomMaterialBandEntity.GetMaterialDateByCodeAndBarcode(code, barCode);
            if (dataSet != null)
            {
                grdCrtlCode.MainView = gridViewCode;
                grdCrtlCode.DataSource = dataSet.Tables[0];
                gridViewCode.BestFitColumns();
            }
        }
        private void MapControlsToBomMateil()
        {
            if (null == _bomMaterialBandEntity)
            {
                throw (new Exception("Error Reason Code Set"));
            }
            _bomMaterialBandEntity.Code = txtCode.Text.Trim();
            _bomMaterialBandEntity.Name = txtName.Text.Trim();
            _bomMaterialBandEntity.BarCode = txtBarcode.Text.Trim();
            _bomMaterialBandEntity.Desc = txtDesc.Text.Trim();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            //if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {
                bool IsTrue = false;
                MapControlsToBomMateil();
                if (_bomMaterialBandEntity.Code == string.Empty)
                {
                    //MessageService.ShowMessage("物料不为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (_bomMaterialBandEntity.Name == string.Empty)
                {
                    //MessageService.ShowMessage("物料名称不为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    return;
                }
                if (_bomMaterialBandEntity.BarCode == string.Empty)
                {
                    //MessageService.ShowMessage("物料代码不能为空", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));  
                    return;
                }
                if (_bomMaterialBandEntity.Desc == string.Empty)
                {
                    //MessageService.ShowMessage("物料描述", "保存");    //名称不能为空!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    return;
                }

                if (CtrlState == ControlState.New && _bomMaterialBandEntity.GetMaterialDateByCodeAndBarcode(_bomMaterialBandEntity.Code, "").Tables[0].Rows.Count > 0)
                {
                    //MessageService.ShowMessage("当前物料料号已经存在,请重新设置", "保存");      //当前名称已存在!
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (CtrlState == ControlState.New)
                {//状态为new
                    if (_bomMaterialBandEntity.Insert())
                    {//新增成功
                        IsTrue = true;
                    }
                }
                else
                {//状态不为new

                    if (_bomMaterialBandEntity.Update())
                    {//修改成功
                        IsTrue = true;
                    }
                    else
                    {
                        //MessageService.ShowMessage("请选择要修改的信息", "保存");      //当前名称已存在!
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.BomMaterialBandCtrl.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}")); 
                    }
                
                }

                if (IsTrue)
                {//值为true
                    BandView();
                    CtrlState = ControlState.ReadOnly;                 //状态为readonly
                }
            }
        }

        private void SupplierCtrl_Load(object sender, EventArgs e)
        {
            BandCode();
            BandView();
        }

        private void BandCode()
        {
            DataSet ds =  _bomMaterialBandEntity.GetBomMaterial();
            txtCode.Properties.DataSource = ds.Tables[0];
            txtCode.Properties.DisplayMember = "MATERIAL_CODE";
            txtCode.Properties.ValueMember = "MATERIAL_CODE";
            txtCode.ItemIndex = 0;
            
        }
        private void BandView()
        {
            DataSet dataSet = _bomMaterialBandEntity.GetMaterialDateByCodeAndBarcode("", "");
            grdCrtlCode.MainView = gridViewCode;
            grdCrtlCode.DataSource = dataSet.Tables[0];
            gridViewCode.BestFitColumns();
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
 
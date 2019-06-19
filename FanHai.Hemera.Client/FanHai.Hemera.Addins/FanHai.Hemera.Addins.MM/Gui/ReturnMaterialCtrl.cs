using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示退料数据的窗体类。
    /// </summary>
    public partial class ReturnMaterialCtrl : BaseUserCtrl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReturnMaterialCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
            GridViewHelper.SetGridView(gvInf);
        }
        public void InitializeLanguage()
        {
            layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0005}");//退料单号
            smbSelect.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0006}");//查询
            lciMaterialLot.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0007}");//退料单号
            layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0008}");//状态
            lciFactoryRoom.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0009}");//车间
            lciOperation.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0010}");//工序名称
            lciStoreName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0011}");//线上仓名称
            lciWorkorderNo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0012}");//工单号
            layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0013}");//创建人
            layoutControlItem13.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0014}");//创建时间
            lciMaterialCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0015}");//原材料编码
            lciMaterialDescription.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0016}");//原材料描述
            lblMblnr.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0017}");//领料单号
            lciSupplierName.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0018}");//原材料供应商
            lciIssueQty.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0019}");//退料数量
            layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0020}");//单位
            lciMemo.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0021}");//备注

            sbtRowAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0022}");//新增
            sbtRowDel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0023}");//移除
            layoutControlItem11.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.lbl.0024}");//线上仓总量


            gcNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0001}");//序号
            gcBackMblnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.GridControl.0001}");//领料单号
            gcAufnr.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0002}");//工单号
            gcMat.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0003}");//物料
            gcMatxt.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0004}");//物料描述
            gcQty.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0005}");//数量
            gcUnit.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0006}");//单位
            gcLlief.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0007}");//供应商名称
            gcMemo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0008}");//备注
            gcCreator.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0009}");//创建人
            gcCreatetime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0010}");//创建时间
            gcEditor.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0011}");//修改人
            gcEditTime.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.MaterialRequisitionCtrl.GridControl.0012}");//修改时间
        }
        public string _status = "Empty";                 //状态表明是新增的还是修改还是查询删除
        private void Status(string _status)
        {
            if (_status.Equals("New"))  //新增过账成功
            {
                lueFactoryRoom.Properties.ReadOnly = false;
                lueOperation.Properties.ReadOnly = false;
                lueStoreName.Properties.ReadOnly = false;
                lueWorkOrderNo.Properties.ReadOnly = false;

                txtNum.Properties.ReadOnly = false;
                BindFactoryRoom();
                BindOperations();
                Bind_Rk_People();
                tetCreateTime.EditValue = DateTime.Now;
                gcInf.DataSource = null;

                txtIssueQty.Text = "";
                txtMemo.Text = "";
                txtUnit.Text = "";
                txtStatus.Text = "";
                lueSupplierName.Text = "";
                txtNum.Text = "";
                sbtRowAdd.Enabled = true;
                sbtRowDel.Enabled = true;

                BtnNew.Enabled = true;
                BtnSave.Enabled = true;
            }
            if (_status.Equals("Edit"))   //修改
            {
                sbtRowAdd.Enabled = true;
                sbtRowDel.Enabled = true;

                BtnNew.Enabled = false;
                BtnSave.Enabled = true;
            }
            if (_status.Equals("Select")) //查询
            {
                sbtRowAdd.Enabled = false;
                sbtRowDel.Enabled = false;

                lueFactoryRoom.Properties.ReadOnly = true;
                lueOperation.Properties.ReadOnly = true;
                lueStoreName.Properties.ReadOnly = true;
                lueWorkOrderNo.Properties.ReadOnly = true;
                txtNum.Properties.ReadOnly = true;
                BtnNew.Enabled = true;
                BtnSave.Enabled = false;
            }
            if (_status.Equals("Save"))   //保存
            {
                sbtRowAdd.Enabled = false;
                sbtRowDel.Enabled = false;

                BtnNew.Enabled = true;
                BtnSave.Enabled = false;
            }

        }
        /// <summary>窗体载入事件。
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManualReceiveMaterialCtrl_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            BindOperations();
            _status = "New";
            Status(_status);
        }
        /// <summary>
        /// 绑定入库人名字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bind_Rk_People()
        {
            try
            {
                string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                txtCreator.Text = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }
        /// <summary> 绑定工厂车间。
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.lueFactoryRoom.Properties.ReadOnly = false;
            DataTable dt = FactoryUtils.GetFactoryRoomByStores(stores);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用退料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>绑定工序。
        /// 绑定工序。
        /// </summary>
        private void BindOperations()
        {
            //获取登录用户拥有权限的工序名称。
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            this.lueStoreName.Properties.ReadOnly = false;
            if (operations.Length > 0)//如果有拥有权限的工序名称
            {
                string[] strOperations = operations.Split(',');
                //遍历工序，并将其添加到窗体控件中。
                for (int i = 0; i < strOperations.Length; i++)
                {
                    lueOperation.Properties.Items.Add(strOperations[i]);
                }
                this.lueOperation.SelectedIndex = 0;
            }
            //禁用工序
            if (string.IsNullOrEmpty(operations)
                || this.lueOperation.Properties.Items.Count <= 1)
            {
                this.lueOperation.Properties.ReadOnly = true;
            }
        }
        /// <summary>绑定线上仓。
        /// 绑定线上仓。
        /// </summary>
        private void BindStores()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            string operation = Convert.ToString(this.lueOperation.Text);
            string stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
            this.lueStoreName.Properties.ReadOnly = false;
            ReceiveMaterialEntity entity = new ReceiveMaterialEntity();
            DataSet ds = entity.GetStores(operation, roomKey, stores);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueStoreName.Properties.DataSource = ds.Tables[0];
                this.lueStoreName.Properties.DisplayMember = "STORE_NAME";
                this.lueStoreName.Properties.ValueMember = "STORE_KEY";
                this.lueStoreName.ItemIndex = 0;
            }
            else
            {
                this.lueStoreName.Properties.DataSource = null;
                this.lueStoreName.EditValue = string.Empty;
            }
            //禁用线上仓
            if (this.lueStoreName.Properties.DataSource == null
                || ds.Tables[0].Rows.Count <= 1)
            {
                this.lueStoreName.Properties.ReadOnly = true;
            }
        }
        /// <summary>绑定工单。
        /// 绑定工单。
        /// </summary>
        private void BindWorkOrderNo()
        {
            string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
            WorkOrders wo = new WorkOrders();
            DataSet ds = wo.GetWorkOrderByFactoryRoomKey(roomKey);
            if (string.IsNullOrEmpty(wo.ErrorMsg))
            {
                //绑定工单号数据到窗体控件。
                this.lueWorkOrderNo.Properties.DataSource = ds.Tables[0];
                this.lueWorkOrderNo.Properties.DisplayMember = "ORDER_NUMBER";
                this.lueWorkOrderNo.Properties.ValueMember = "ORDER_NUMBER";
                //this.lueWorkOrderNo.ItemIndex = 0;
            }
            else
            {
                this.lueWorkOrderNo.Properties.DataSource = null;
                this.lueWorkOrderNo.EditValue = string.Empty;
                this.lueWorkOrderNo.Text = string.Empty;
            }
        }
        /// <summary>绑定物料编码。
        /// 绑定物料编码。
        /// </summary>
        private void BindMaterialCode()
        {
            string orderNumber = this.lueWorkOrderNo.Text;
            this.lueMaterialCode.EditValue = string.Empty;
            this.txtMaterialDescription.Text = string.Empty;
            MaterialReqOrReturnEntity entity = new MaterialReqOrReturnEntity();
            DataSet ds = entity.GetMaterialstTui(orderNumber);
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                this.lueMaterialCode.Properties.DataSource = ds.Tables[0];
                this.lueMaterialCode.Properties.DisplayMember = "MATERIAL_CODE";
                this.lueMaterialCode.Properties.ValueMember = "MATERIAL_CODE";
                this.lueMaterialCode.ItemIndex = 0;
                this.lueMaterialCode.Properties.PopupFormSize = new Size(220, 140); 
            }
            else
            {
                this.lueMaterialCode.Properties.DataSource = null;
                this.lueMaterialCode.EditValue = string.Empty;
                this.txtMaterialDescription.Text = string.Empty;
            }
        }
        /// <summary>工厂信息改变事件
        /// 工厂信息改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定线上仓
            BindStores();
            //重新绑定工单
            BindWorkOrderNo();
        }
        /// <summary>工序变更后绑定线上仓
        /// 工序变更后绑定线上仓
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueOperation_EditValueChanged(object sender, EventArgs e)
        {
            //重新绑定线上仓
            BindStores();
        }

        /// <summary>物料号变更带出物料描述和单位
        /// 物料号变更带出物料描述和单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueMaterialCode_EditValueChanged(object sender, EventArgs e)
        {
            string materialName = Convert.ToString(this.lueMaterialCode.GetColumnValue("MATERIAL_NAME"));   
            this.txtMaterialDescription.Text = materialName;

            MaterialReqOrReturnEntity entity = new MaterialReqOrReturnEntity();
            DataSet ds = entity.GetMaterialstTui(lueWorkOrderNo.Text.Trim(), lueMaterialCode.Text.Trim(), "");
            if (string.IsNullOrEmpty(entity.ErrorMsg))
            {
                lueMblnr.Properties.DataSource = ds.Tables[0];
                this.lueMblnr.Properties.DisplayMember = "MBLNR";
                this.lueMblnr.Properties.ValueMember = "MBLNR";
                this.lueMblnr.ItemIndex = 0;
                this.lueMblnr.Properties.PopupFormSize = new Size(200, 120); 
            }
            string llief = Convert.ToString(this.lueMblnr.GetColumnValue("LLIEF"));
            string sumqty = Convert.ToString(this.lueMblnr.GetColumnValue("SUMQTY"));
            string unit = Convert.ToString(this.lueMblnr.GetColumnValue("UNIT"));
            lueSupplierName.Text = llief;
            txtSum.Text = sumqty;
            txtUnit.Text = unit;
        }
        //关闭     
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }

        /// <summary>新增行记录
        /// 新增行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtRowAdd_Click(object sender, EventArgs e)
        {
            //1.检查栏位数据信息
            //2.检查是否重复：同工单，同料号，同批次号
            //3.插入数据到界面数据清单表中
            bool _bool = CheckParameter();//1.检查栏位数据信息
            if (_bool == false) return;
            //判定是否存在领料单号
            MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
            DataSet dsCheckNum = materialReqOrReturnEntity.GetCountByNumToCheck(txtNum.Text.Trim(),0);
            if (_status == "New")
            {
                if (Convert.ToInt32(dsCheckNum.Tables[0].Rows[0]["COUNT"].ToString()) > 0)
                {
                    //MessageBox.Show("已经存在该领料单号请重新输入！", "系统错误提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
            }
            if (Convert.ToDecimal(txtSum.Text.Trim()) < Convert.ToDecimal(txtIssueQty.Text.Trim()))
            {
                //MessageBox.Show("退料数量不能大于线上仓总量！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            if (_bool == false) return;
            //2.判断新增数据是否在列表中有重复？
            DataTable dtSource = this.gcInf.DataSource as DataTable;
            if (dtSource != null)
            {
                DataRow[] drs = dtSource.Select("AUFNR='" + lueWorkOrderNo.Text.Trim() + "'AND MATNR= '" + lueMaterialCode.Text.Trim() + "'AND MBLNR= '" + lueMblnr.Text.Trim() + "'");
                if (drs.Length >= 1)
                {
                    //MessageBox.Show("数据表中存在重复数据！", "系统提示");
                    MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }
            }
            else
            {
                lueFactoryRoom.Properties.ReadOnly = true;
                lueOperation.Properties.ReadOnly = true;
                lueStoreName.Properties.ReadOnly = true;
                lueWorkOrderNo.Properties.ReadOnly = true;
                txtNum.Properties.ReadOnly = true;
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
            //3.插入数据到界面数据清单表中
            GetInfToGvlist();
        }
        /// <summary> 插入数据到界面数据清单表中
        /// 插入数据到界面数据清单表中
        /// </summary>
        private void GetInfToGvlist()
        {
            MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
            string _workOrder = lueWorkOrderNo.Text.Trim();
            string _mat = lueMaterialCode.Text.Trim();
            string _matDesc = txtMaterialDescription.Text.Trim();
            string _qty = txtIssueQty.Text.Trim();
            string _unit = txtUnit.Text.Trim();
            string _supplier = lueSupplierName.Text.Trim();
            string _creator = txtCreator.Text.Trim();
            string _backMblnr = lueMblnr.EditValue.ToString().Trim();
            DataTable dtSource = gcInf.DataSource as DataTable;
            string _memo = txtMemo.Text.Trim();
            DataTable dt = new DataTable();
            if (dtSource == null)
            {
                DataSet dsMaterialRequisitionInf = materialReqOrReturnEntity.GetMatRequisitionInfByNumTui("");
                if (dsMaterialRequisitionInf != null || dsMaterialRequisitionInf.Tables.Count > 0)
                {
                    dt = dsMaterialRequisitionInf.Tables["WST_STORE_MATERIAL_REQUISITION_DETAIL"].Clone();
                }
            }
            else
            {
                dt = dtSource.Clone();
            }

            DataRow dr = dt.NewRow();
            dr["AUFNR"] = _workOrder;
            dr["MATNR"] = _mat;
            dr["MATXT"] = _matDesc;
            dr["QTY"] = _qty;
            dr["ERFME"] = _unit;
            dr["LLIEF"] = _supplier;
            dr["MEMO"] = _memo;
            dr["CREATOR"] = _creator;
            dr["BACK_MBLNR"] = _backMblnr;
            dt.Rows.Add(dr);
            
            if (dtSource == null)
            {
                dtSource = dt;
            }
            else
            {
                dtSource.Merge(dt, true);
            }
            int j = 1;
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                if (dtSource.Rows[i].RowState != DataRowState.Deleted && dtSource.Rows[i].RowState != DataRowState.Detached)
                {
                    dtSource.Rows[i]["ROWNUMBER"] = j.ToString();
                    j++;
                }
            }
            gcInf.DataSource = dtSource;
            lueMaterialCode.Select();
            lueMaterialCode.SelectAll();
            lueFactoryRoom.Properties.ReadOnly = true;
            lueOperation.Properties.ReadOnly = true;
            lueStoreName.Properties.ReadOnly = true;
            lueWorkOrderNo.Properties.ReadOnly = true;
        }
        /// <summary>检查界面数据输入信息是否符合要求
        /// 检查界面数据输入信息是否符合要求
        /// </summary>
        private bool CheckParameter()   
        {
            if (string.IsNullOrEmpty(txtNum.Text.Trim()))
            {
                //MessageBox.Show("退料单号不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueFactoryRoom.Text.Trim()))
            {
                //MessageBox.Show("车间不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueOperation.Text.Trim()))
            {
                //MessageBox.Show("工序不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0006}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueStoreName.Text.Trim()))
            {
                //MessageBox.Show("线上仓不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueWorkOrderNo.Text.Trim()))
            {
                //MessageBox.Show("工单不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueMaterialCode.Text.Trim()))
            {
                //MessageBox.Show("物料不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(lueSupplierName.Text.Trim()))
            {
                //MessageBox.Show("供应商不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            if (string.IsNullOrEmpty(txtIssueQty.Text.Trim()))
            {
                //MessageBox.Show("数量不能为空！", "系统提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0011}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return false;
            }
            return true;
        }
        /// <summary>新建退料单
        /// 新建退料单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspNew_Click(object sender, EventArgs e)
        {
            _status = "New";
            Status(_status);
        }
        /// <summary>移除行项目
        /// 移除行项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtRowDel_Click(object sender, EventArgs e)
        {
            if (this.gvInf.GetFocusedRow() != null)
            {
                DataTable dtCheck = ((DataView)gvInf.DataSource).Table;
                if (dtCheck != null)
                {
                    if (dtCheck.Rows.Count == 1)
                    {
                        //MessageBox.Show("不能完全清空列表数据，请确认！", "系统错误提示");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0012}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        return;
                    }
                }
                this.gvInf.DeleteRow(this.gvInf.FocusedRowHandle);
                //((DataView)gvInf.DataSource).Table.AcceptChanges();
                DataTable dt = ((DataView)gvInf.DataSource).Table;

                if (dt != null)
                {
                    if (!dt.Columns.Contains("ROWNUMBER"))
                        dt.Columns.Add("ROWNUMBER");
                    int j = 1;
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {

                        if (dt.Rows[i - 1].RowState != DataRowState.Deleted && dt.Rows[i - 1].RowState != DataRowState.Detached)
                        {
                            dt.Rows[i - 1]["ROWNUMBER"] = j.ToString();
                            j++;
                        }
                    }
                    gcInf.Refresh();
                    gcInf.DataSource = dt;
                }
                else
                {
                    gcInf.DataSource = null;     //清空datasource  
                }
            }
            else
            {
                //MessageService.ShowMessage("必须选择至少选择一条记录", "${res:Global.SystemInfo}");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0013}"), StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }
        /// <summary>修改操作
        /// 修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspEdit_Click(object sender, EventArgs e)
        {
            ((DataView)gvInf.DataSource).Table.AcceptChanges();
            if (txtStatus.Text == "已创建")
            {
                _status = "Edit";
                Status(_status);
            }
        }
        /// <summary>根据退料单号查询领料单信息
        /// 根据退料单号查询领料单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smbSelect_Click(object sender, EventArgs e)
        {            
            MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
            string _numForSelect = txtNumForSelect.Text.Trim();
            DataTable dtRequistion = new DataTable();
            DataTable dtRequistionDetail = new DataTable();
            if (string.IsNullOrEmpty(_numForSelect))
            {
                //MessageBox.Show("退料单号不能为空！","系统错误提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0014}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            //判定是否存在退料单号
            DataSet dsCheckNum = materialReqOrReturnEntity.GetCountByNumToCheck(_numForSelect,0);
            if (Convert.ToInt32(dsCheckNum.Tables[0].Rows[0]["COUNT"].ToString()) < 1)
            {
                //MessageBox.Show("不存在该退料单号请重新输入！", "系统错误提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0015}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }

            DataSet dsMaterialRequisitionInf = materialReqOrReturnEntity.GetMatRequisitionInfByNumTui(_numForSelect);
            if (dsMaterialRequisitionInf != null || dsMaterialRequisitionInf.Tables.Count > 0)
            {
                dtRequistion = dsMaterialRequisitionInf.Tables["WST_STORE_MATERIAL_REQUISITION"];
                dtRequistionDetail = dsMaterialRequisitionInf.Tables["WST_STORE_MATERIAL_REQUISITION_DETAIL"];
                if (dtRequistion.Rows.Count > 0 && dtRequistionDetail.Rows.Count > 0)
                {
                    txtNum.Text = dtRequistion.Rows[0]["MBLNR"].ToString();
                    lueFactoryRoom.EditValue = dtRequistion.Rows[0]["FACTORYKEY"].ToString();
                    lueFactoryRoom.Text = dtRequistion.Rows[0]["FACTORYNAME"].ToString();
                    lueOperation.Text = dtRequistion.Rows[0]["PROCESS"].ToString();
                    lueStoreName.EditValue = dtRequistion.Rows[0]["STORE_KEY"].ToString();
                    lueStoreName.Text = dtRequistion.Rows[0]["STORE_NAME"].ToString();
                    lueWorkOrderNo.Text = dtRequistion.Rows[0]["AUFNR"].ToString();
                    txtCreator.Text = dtRequistion.Rows[0]["CREATOR"].ToString();
                    tetCreateTime.EditValue = dtRequistion.Rows[0]["CREATE_TIME"].ToString();
                    txtStatus.Text = GetStatus(dtRequistion.Rows[0]["STATUS"].ToString());
                    for (int i = 0; i < dtRequistionDetail.Rows.Count; i++)
                    {
                        dtRequistionDetail.Rows[i]["ROWNUMBER"] = i + 1;
                    }
                    gcInf.DataSource = dtRequistionDetail;
                    _status = "Select";
                    Status(_status);
                }
            }
        }
        /// <summary>获取状态
        /// 获取状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GetStatus(string status)
        {
            string retStatus = string.Empty;
            if(string.IsNullOrEmpty(status))
            {
                retStatus = "已创建";
            }
            else if(status == "W")
            {
                retStatus = "末审批";
            }
            else if(status == "A")
            {
                retStatus = "审批通过";
            }
            else if(status == "R")
            {
                retStatus = "拒绝";           
            }
            else if(status == "T")
            {
                retStatus = "已过帐";                   
            }
            else if(status == "D")
            {
                retStatus = "已删除";                      
            }
            return retStatus;
        }
        /// <summary>保存退料单
        /// 保退领料单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspSave_Click(object sender, EventArgs e)
        {
            MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
            if (gcInf.DataSource as DataTable == null)
            {
                //MessageBox.Show("没有需要保存的信息！", "系统错误提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0016}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            try
            {
                //if (MessageBox.Show(StringParser.Parse("是否保存退料单信息？"),
                //   StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)

                if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0017}"),
                  StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    #region 新增退料单信息，抬头表，从表，数据记录表
                    if (_status == "New")
                    {
                        //数据绑定
                        DataTable dtInKo = new DataTable();
                        dtInKo.Columns.Add("MBLNR");
                        dtInKo.Columns.Add("FACTORYKEY");
                        dtInKo.Columns.Add("FACTORYNAME");
                        dtInKo.Columns.Add("PROCESS");
                        dtInKo.Columns.Add("STORE_KEY");
                        dtInKo.Columns.Add("STORE_NAME");
                        dtInKo.Columns.Add("AUFNR");
                        dtInKo.Columns.Add("CREATOR");
                        DataRow dr = dtInKo.NewRow();
                        dr["MBLNR"] = txtNum.Text.Trim();
                        dr["FACTORYKEY"] = lueFactoryRoom.EditValue.ToString();
                        dr["FACTORYNAME"] = lueFactoryRoom.Text.Trim();
                        dr["PROCESS"] = lueOperation.Text.Trim();
                        dr["STORE_KEY"] = lueStoreName.EditValue.ToString();
                        dr["STORE_NAME"] = lueStoreName.Text.Trim();
                        dr["AUFNR"] = lueWorkOrderNo.Text.Trim();
                        dr["CREATOR"] = txtCreator.Text.Trim();
                        dtInKo.Rows.Add(dr);
                        dtInKo.TableName = "WST_STORE_MATERIAL_REQUISITION";

                        DataTable dtInPo = new DataTable();
                        DataView dv = gvInf.DataSource as DataView;
                        if (dv != null) dtInPo = dv.Table;
                        dtInPo.TableName = "WST_STORE_MATERIAL_REQUISITION_DETAIL";

                        DataSet dsIn = new DataSet();
                        dsIn.Merge(dtInKo);
                        dsIn.Merge(dtInPo);
                        DataSet dsReturn = materialReqOrReturnEntity.CreateRequistionKoPoTui(dsIn);
                        if (string.IsNullOrEmpty(materialReqOrReturnEntity.ErrorMsg))
                        {
                            //MessageBox.Show("保存成功！", "系统提示");
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0018}"), StringParser.Parse("${res:Global.SystemInfo}"));
                            _status = "Save";
                            txtStatus.Text = "已过账";
                            Status(_status);
                        }
                        else
                        {
                            MessageBox.Show(materialReqOrReturnEntity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
                        }
                        return;
                    }
                    #endregion
                    #region 修改退料单记录，抬头表不变，从表新增新记录修改删除记录状态，修改退料信息数据
                    else if (_status == "Edit")
                    {
                        if (!txtStatus.Text.Equals("已创建"))
                        {
                            //MessageBox.Show("此退料单已过帐或删除，不能修改！", "系统提示");
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0019}"), StringParser.Parse("${res:Global.SystemInfo}"));
                            return;
                        }
                        if (this.gvInf.State == GridState.Editing && this.gvInf.IsEditorFocused && this.gvInf.EditingValueModified)
                        {
                            this.gvInf.SetFocusedRowCellValue(this.gvInf.FocusedColumn, this.gvInf.EditingValue);
                        }
                        this.gvInf.UpdateCurrentRow();
                        DataView dv = gvInf.DataSource as DataView;                        
                        DataTable dt = new DataTable();
                        if (dv != null) dt = dv.Table;
                        DataSet dsSave = new DataSet();
                        DataTable _dt_Insert = dt.GetChanges(DataRowState.Added);
                        DataTable _dt_Delete = dt.GetChanges(DataRowState.Deleted);
                        
                        string _editer = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                        string _mblnr = txtNum.Text.Trim();
                        if (_dt_Insert != null && _dt_Insert.Rows.Count > 0)
                        {
                            _dt_Insert.TableName = "INSERT";
                            dsSave.Merge(_dt_Insert, true, MissingSchemaAction.Add);
                        }
                        if (_dt_Delete != null && _dt_Delete.Rows.Count > 0)
                        {
                            _dt_Delete.RejectChanges();
                            _dt_Delete.TableName = "DELETE";
                            dsSave.Merge(_dt_Delete, true, MissingSchemaAction.Add);
                        }
                        DataSet dsReturn = materialReqOrReturnEntity.UpdateRequistionKoPoTui(dsSave,_editer,_mblnr);
                        if (string.IsNullOrEmpty(materialReqOrReturnEntity.ErrorMsg))
                        {
                            //MessageBox.Show("修改成功！", "系统提示");
                            MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0020}"), StringParser.Parse("${res:Global.SystemInfo}"));
                            _status = "Save";
                            txtStatus.Text = "已创建";
                            Status(_status);
                        }
                        else
                        {
                            MessageBox.Show(materialReqOrReturnEntity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
                            return;
                        }
                    }
                    #endregion


                    ((DataView)gvInf.DataSource).Table.AcceptChanges();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }
        /// <summary>
        /// 过账操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspPass_Click(object sender, EventArgs e)
        {
            MaterialReqOrReturnEntity materialReqOrReturnEntity = new MaterialReqOrReturnEntity();
            //if (MessageBox.Show(StringParser.Parse("是否对当前退料单进行过账？"),
            //    StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            if (MessageBox.Show(StringParser.Parse(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0021}")),
               StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (txtStatus.Text == "已创建")
                {
                    DataSet dsReturn = materialReqOrReturnEntity.UpdateStatusTui(txtNum.Text.Trim(), PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ));
                    if (string.IsNullOrEmpty(materialReqOrReturnEntity.ErrorMsg))
                    {
                        //MessageBox.Show("退料单" + txtNum.Text.Trim() + "过账成功！", "系统提示");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0022}") 
                            + txtNum.Text.Trim()
                            + StringParser.Parse("${res:FanHai.Hemera.Addins.ReturnMaterialCtrl.msg.0023}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        _status = "New";
                        txtStatus.Text = "已过账";
                        Status(_status);
                    }
                    else
                    {
                        MessageBox.Show(materialReqOrReturnEntity.ErrorMsg, StringParser.Parse("${res:Global.SystemInfo}"));
                    }
                }
            }
        }
        /// <summary>工单改变事件
        /// 工单改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueWorkOrderNo_EditValueChanged(object sender, EventArgs e)
        {
            BindMaterialCode();
        }

        private void lueMblnr_EditValueChanged(object sender, EventArgs e)
        {
            string llief = Convert.ToString(this.lueMblnr.GetColumnValue("LLIEF"));
            string sumqty = Convert.ToString(this.lueMblnr.GetColumnValue("SUMQTY"));
            string unit = Convert.ToString(this.lueMblnr.GetColumnValue("UNIT"));
            lueSupplierName.Text = llief;
            txtSum.Text = sumqty;
            txtUnit.Text = unit;
        }

        private void lueWorkOrderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BindMaterialCode();
            }
        }

        private void gvDecayCoeffi_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void PanelTitle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanelMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

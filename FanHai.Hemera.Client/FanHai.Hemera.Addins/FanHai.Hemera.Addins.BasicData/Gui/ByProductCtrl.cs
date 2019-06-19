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

    public partial class ByProductCtrl : BaseUserCtrl
    {
        ByProductEntity _byProductEntity = new ByProductEntity();
        string por_part = string.Empty;
        public string key = string.Empty;
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
            tsbNew.Enabled = true;
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:
                    txtMtnrm.Text = string.Empty;
                    txtMtnrB2.Text = string.Empty;
                    txtMtnrB3.Text = string.Empty;

                    txtMtnrm.Enabled = false;
                    txtMtnrB2.Enabled = false;
                    txtMtnrB3.Enabled = false;
                    tsbSave.Enabled = true;
                    tsbDel.Enabled = false;
                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:
                    txtMtnrm.Enabled = true;
                    txtMtnrB2.Enabled = true;
                    txtMtnrB3.Enabled = true;

                    tsbDel.Enabled = true;
                    tsbSave.Enabled = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtMtnrm.Text = string.Empty;
                    txtMtnrB2.Text = string.Empty;
                    txtMtnrB3.Text = string.Empty;

                    txtMtnrm.Enabled = true;
                    txtMtnrB2.Enabled = true;
                    txtMtnrB3.Enabled = true;
                    tsbSave.Enabled = true;
                    tsbDel.Enabled = false;
                    key = "";
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    txtMtnrm.Text = string.Empty;
                    txtMtnrB2.Text = string.Empty;
                    txtMtnrB3.Text = string.Empty;

                    txtMtnrm.Enabled = false;
                    txtMtnrB2.Enabled = false;
                    txtMtnrB3.Enabled = false;

                    tsbDel.Enabled = false;
                    tsbSave.Enabled = false;
                    break;
                #endregion

                #region case state of del
                case ControlState.Delete:

                    break;
                #endregion
            }
        }


        /// <summary>
        /// 构造函数。
        /// </summary>
        public ByProductCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
        }

        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        //  新建
        private void tsbNew_Click(object sender, EventArgs e)
        {
            CtrlState = ControlState.New;
        }

        /// <summary>
        /// 绑定组件类型
        /// </summary>
        public void BindSupplierCode()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.PART_TYPE);
            this.txtMoudleType.Properties.DataSource = BaseData.Get(columns, category);
            this.txtMoudleType.Properties.DisplayMember = "NAME";
            this.txtMoudleType.Properties.ValueMember = "CODE";
            this.txtMoudleType.ItemIndex = 0;
        }

        //数据表绑定数据
        public void BindDataGridSource()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ByProduct");
            ByProductEntity boe = new ByProductEntity();
            DataColumn dc = null;
            dc = dt.Columns.Add("PARTM", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTB2", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTB3", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTTYPE", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTCREATER", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTCS", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTCE", Type.GetType("System.String"));
            dc = dt.Columns.Add("PARTEDITER", Type.GetType("System.String"));
            //dc = dt.Columns.Add("PARTES", Type.GetType("System.String"));
            //dc = dt.Columns.Add("PARTEE", Type.GetType("System.String"));

            DataRow newRow = dt.NewRow();
            newRow["PARTM"] = string.Empty;
            newRow["PARTB2"] = string.Empty;
            newRow["PARTB3"] = string.Empty;
            newRow["PARTTYPE"] = string.Empty;
            newRow["PARTCREATER"] = string.Empty;
            newRow["PARTCS"] = string.Empty;
            newRow["PARTCE"] = string.Empty;
            newRow["PARTEDITER"] = string.Empty;
            //newRow["PARTES"] =string.Empty;
            //newRow["PARTEE"] = string.Empty;
            dt.Rows.Add(newRow);

            ds.Tables.Add(dt);
            DataSet dsreturn = boe.GetByProductInf(ds);
            DataTable dtByProduct = dsreturn.Tables[0];
            gcList.DataSource = dtByProduct;
            
        }

        //窗体载入
        private void ByProductCtrl_Load(object sender, EventArgs e)
        {
            txtMtnrm.Text = "";
            txtMtnrB2.Text = "";
            txtMtnrB3.Text = "";
            BindSupplierCode();
            BindDataGridSource();

            lblMenu.Text = "基础数据 > 工艺参数设置 > 主副产品管理";
            GridViewHelper.SetGridView(gvList);
        }

        public string SelectPorPartName()
        {
            string ret = string.Empty;
            ByProductForm bpf = new ByProductForm();
            if (DialogResult.OK == bpf.ShowDialog())
            {
                ret = bpf.por_part.ToString();
            }
            return ret;
        }
        //事件
        #region
        private void txtMtnrm_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            txtMtnrm.Text = SelectPorPartName().ToString();
        }

        private void txtMtnrB3_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            txtMtnrB3.Text = SelectPorPartName().ToString();
        }

        private void txtMtnrB2_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            txtMtnrB2.Text = SelectPorPartName().ToString();
        }

        private void tsbSeach_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ByProduct");
            ByPortductSelectForm bpsf = new ByPortductSelectForm();
            ByProductEntity boe = new ByProductEntity();
            if (DialogResult.OK == bpsf.ShowDialog())
            {
                DataColumn dc = null;
                dc = dt.Columns.Add("PARTM", Type.GetType("System.String"));
                dc = dt.Columns.Add("PARTB2", Type.GetType("System.String"));
                dc = dt.Columns.Add("PARTB3", Type.GetType("System.String"));
                dc = dt.Columns.Add("PARTTYPE", Type.GetType("System.String"));
                dc = dt.Columns.Add("PARTCREATER", Type.GetType("System.String"));
                dc = dt.Columns.Add("PARTCS", Type.GetType("System.DateTime"));
                dc = dt.Columns.Add("PARTCE", Type.GetType("System.DateTime"));
                dc = dt.Columns.Add("PARTEDITER", Type.GetType("System.String"));
                //dc = dt.Columns.Add("PARTES", Type.GetType("System.DateTime"));
                //dc = dt.Columns.Add("PARTEE", Type.GetType("System.DateTime"));

                DataRow newRow = dt.NewRow();
                newRow["PARTM"] = bpsf.strM;
                newRow["PARTB2"] = bpsf.strB2;
                newRow["PARTB3"] = bpsf.strB3;
                newRow["PARTTYPE"] = bpsf.strType;
                newRow["PARTCREATER"] = bpsf.strCreater;
                newRow["PARTCS"] = bpsf.strCtstart;
                newRow["PARTCE"] = bpsf.strCtsend;
                newRow["PARTEDITER"] = bpsf.strEditer;
                //newRow["PARTES"] = bpsf.strEtstart;
                //newRow["PARTEE"] = bpsf.strEtsend;
                dt.Rows.Add(newRow);

                ds.Tables.Add(dt);
                DataSet dsreturn = boe.GetByProductInf(ds);
                DataTable dtByProduct = dsreturn.Tables[0];
                gcList.DataSource = dtByProduct;
            }
        }
        #endregion
        //删除行数据
        private void tsbDel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (MessageService.AskQuestion("确定删除吗?", "系统提示"))
                {//系统提示你确定要删除吗？
                    if (_byProductEntity.Delete(key))
                    {
                        //数据表数据重新绑定 
                        BindDataGridSource();
                        CtrlState = ControlState.Empty;
                    }
                }
            }
            else
            {
                MessageService.ShowMessage("请选择记录再删除！", "系统提示！");
            }
        }
        //行单击事件
        private void gvList_RowClick(object sender, RowClickEventArgs e)
        {
            CtrlState = ControlState.Edit;

            _byProductEntity = new ByProductEntity();
            this.txtMtnrm.EditValue = this.gvList.GetRowCellValue(e.RowHandle, "MATNR_M").ToString();
            this.txtMtnrB2.EditValue = this.gvList.GetRowCellValue(e.RowHandle, "MATNR_B2").ToString();
            this.txtMtnrB3.EditValue = this.gvList.GetRowCellValue(e.RowHandle, "MATNR_B3").ToString();
            this.txtMoudleType.EditValue = this.gvList.GetRowCellValue(e.RowHandle, "PTYP3").ToString();
            key = this.gvList.GetRowCellValue(e.RowHandle, "BYP_KEY").ToString();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("你确定要保存当前界面的数据吗？", "保存"))
            {
                bool IsTrue = false;
                ByProductEntity byProductEntity = new ByProductEntity();
                if (CtrlState == ControlState.New )
                {
                    if (!string.IsNullOrEmpty(txtMtnrm.Text.Trim()))
                    {
                        DataSet dsMtnrm = byProductEntity.GetLotPartInf(txtMtnrm.Text.Trim());
                        if (dsMtnrm.Tables[0].Rows.Count < 1)
                        {
                            MessageService.ShowMessage("主料料号不存在请在成品管理模块维护料号！", "系统提示！");
                            return;
                        }
                        DataSet dsMtnrB2 = byProductEntity.GetLotPartInf(txtMtnrB2.Text.Trim());
                        if (!string.IsNullOrEmpty(txtMtnrB2.Text.Trim()))
                        {
                            if (dsMtnrB2.Tables[0].Rows.Count < 1)
                            {
                                MessageService.ShowMessage("低效物料号不存在请在成品管理模块维护料号！", "系统提示！");
                                return;
                            }
                        }
                        DataSet dsMtnrB3 = byProductEntity.GetLotPartInf(txtMtnrB3.Text.Trim());
                        if (!string.IsNullOrEmpty(txtMtnrB3.Text.Trim()))
                        {
                            if (dsMtnrB3.Tables[0].Rows.Count < 1)
                            {
                                MessageService.ShowMessage("二三级品物料号不存在请在成品管理模块维护料号！", "系统提示！");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage("主料料号不能为空！", "系统提示！");
                        return;
                    }
                    
                }
                Hashtable hashTable = new Hashtable();
                hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableParam.TableName = "HASH";
                if (CtrlState == ControlState.New)
                {//状态为new
                    DataTable dtPro = new DataTable();
                    dtPro.Columns.Add("MATNR_M");
                    dtPro.Columns.Add("MATNR_B2");
                    dtPro.Columns.Add("MATNR_B3");
                    dtPro.Columns.Add("PTYP3");
                    //dtPro.Columns.Add("WERKS");
                    DataRow dr = dtPro.NewRow();
                    dr["MATNR_M"] = txtMtnrm.EditValue.ToString();
                    dr["MATNR_B2"] = txtMtnrB2.EditValue.ToString();
                    dr["MATNR_B3"] = txtMtnrB3.EditValue.ToString();
                    dr["PTYP3"] = txtMoudleType.EditValue.ToString();
                    dtPro.Rows.Add(dr);

                    DataSet dsSetIn = new DataSet();
                    dtPro.TableName = "PP_ZMMDBYP";
                    dsSetIn.Merge(dtPro);                    
                    dsSetIn.Merge(tableParam);
                    if (byProductEntity.InsertPro(dsSetIn))
                    {//新增成功
                        IsTrue = true;
                    }
                }
                else
                {//状态不为new
                    if (key != "")
                    {
                        DataTable dtPro = new DataTable();
                        dtPro.Columns.Add("MATNR_M");
                        dtPro.Columns.Add("MATNR_B2");
                        dtPro.Columns.Add("MATNR_B3");
                        dtPro.Columns.Add("PTYP3");
                        dtPro.Columns.Add("BYP_KEY");
                        DataRow dr = dtPro.NewRow();
                        dr["MATNR_M"] = txtMtnrm.EditValue.ToString();
                        dr["MATNR_B2"] = txtMtnrB2.EditValue.ToString();
                        dr["MATNR_B3"] = txtMtnrB3.EditValue.ToString();
                        dr["PTYP3"] = txtMoudleType.EditValue.ToString();
                        dr["BYP_KEY"] = key;
                        dtPro.Rows.Add(dr);

                        DataSet dsSetIn = new DataSet();
                        dtPro.TableName = "PP_ZMMDBYP";
                        dsSetIn.Merge(dtPro);
                        dsSetIn.Merge(tableParam);
                        if (byProductEntity.UpdatePro(dsSetIn))
                        {//修改成功
                            IsTrue = true;
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage("请选择要修改的行信息", "保存");      //当前名称已存在!
                    }

                }

                if (IsTrue)
                {//值为true
                    BindDataGridSource(); ;                                //数据表数据重新绑定
                    CtrlState = ControlState.ReadOnly;                 //状态为readonly
                }
            }
        }
    }
}
 
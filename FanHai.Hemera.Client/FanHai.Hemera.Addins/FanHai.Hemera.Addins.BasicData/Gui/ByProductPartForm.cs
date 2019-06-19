using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Constants;
using System.Collections;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities.BasicData;
using DevExpress.XtraGrid.Views.Grid;
using System.Linq;

namespace FanHai.Hemera.Addins.BasicData
{
    public partial class ByProductPartForm : BaseDialog
    {
        string _partNum = string.Empty;
        public ByProductPartForm()
        {
            InitializeComponent();
            //lcPeople.Text = "当前操作用户：" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ); 
            lcPeople.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0010}") +PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ); 
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            //this.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0001}");//主副产品管理
            //this.lblPartId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0002}");//产品料号
            //this.lblPartMoudle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0003}");//产品描述
            //this.sbtAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0004}");//添加
            //this.sbtAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0005}");//移除

            //gcItemNo.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0001}");//序号
            //gcPartNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0002}");//对应料号
            //gcPartDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0003}");//描述
            //gcMinPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0004}");//最小标称功率
            //gcMaxPower.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0005}");//最大标称功率
            //gcGrades.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0006}");//等级
            //gcStorageLocation.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.GridControl.0007}");//库位

            //this.lblPartId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0006}");//新增
            //this.lblPartMoudle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0007}");//删除
            //this.sbtAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0008}");//保存
            //this.sbtAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0009}");//关闭

        }

        public ByProductPartForm(ControlState control)
        {
            InitializeComponent();
            OnAfterStateChanged(control);
            //lcPeople.Text = "当前操作用户：" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            lcPeople.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0010}") + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            InitializeLanguage();
        }

        public ByProductPartForm(ControlState control,string partNum)
        {
            InitializeComponent();
            _partNum = partNum;
            OnAfterStateChanged(control);
            InitializeLanguage();
            //lcPeople.Text = "当前操作用户：" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            lcPeople.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.lbl.0010}") + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
            if (!string.IsNullOrEmpty(_partNum))
            {
                ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
                DataSet dsGetByPartId = byProductPartEntity.GetByPartId(_partNum);
                DataTable dtGetByPartId = new DataTable();
                DataTable dsGetByPartId2 = new DataTable();
                dtGetByPartId = dsGetByPartId.Tables["POR_PART_BYPRODUCT"];
                dsGetByPartId2 = dsGetByPartId.Tables["POR_PART"];
                //if (!dtGetByPartId.Columns.Contains("ROWNUMBER"))
                //    dtGetByPartId.Columns.Add("ROWNUMBER");
                if (dtGetByPartId.Rows.Count > 0)
                {
                    //for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                    //{
                    //    dtGetByPartId.Rows[i]["ROWNUMBER"] = i + 1;
                    //}
                    //如果存在且已经有对应料号则带出所有的对应料号信息

                    //遍历查询结果
                    for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                    {
                        string gradesOut = string.Empty;
                        string gradesOutUsed = string.Empty;
                        string grades = dtGetByPartId.Rows[i]["GRADES"].ToString().Trim();    ///获取等级列信息
                        string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                        for (int j = 0; j < arrTemp.Length; j++)
                        {
                            gradesOut += GetProductGradeDisplayText(arrTemp[j]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                        }
                        gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                        dtGetByPartId.Rows[i]["GRADES"] = gradesOutUsed;
                    }///列重新赋值

                    gcList.DataSource = dtGetByPartId;
                    txtDesc.Text = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();
                    txtPartId.Text = dsGetByPartId2.Rows[0]["PART_ID"].ToString().Trim();
                    OnAfterStateChanged(ControlState.Edit);
                }
                else
                {
                    //如果存在但未进行绑定联副产品则带出首条对应料号即为主料号
                    DataRow dr = dtGetByPartId.NewRow();
                    dr["ITEM_NO"] = "1";
                    dr["PART_NUMBER"] = _partNum;

                    DataTable dtPro = new DataTable();
                    dtPro = GetRules();
                    for (int j = 0; j < dtPro.Rows.Count; j++)
                    {
                        DataRow drPro = dtPro.Rows[j];
                        if (drPro["ITEMNO"].ToString().Trim() == "1" && drPro["MODULE"].ToString().Trim() == dsGetByPartId2.Rows[0]["PART_MODULE"].ToString().Trim())
                        {
                            string gradesOut = string.Empty;
                            string gradesOutUsed = string.Empty;
                            string grades = dtPro.Rows[j]["GRADES"].ToString().Trim();    ///获取等级列信息
                            string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                gradesOut += GetProductGradeDisplayText(arrTemp[i]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                            }
                            gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号

                            dr["GRADES"] = gradesOutUsed;                      ///列重新赋值
                                                                               ///
                            if (!string.IsNullOrEmpty(dtPro.Rows[j]["MINPOWER"].ToString().Trim()))
                            dr["MIN_POWER"] = dtPro.Rows[j]["MINPOWER"].ToString().Trim();

                            if (!string.IsNullOrEmpty(dtPro.Rows[j]["MAXPOWER"].ToString().Trim()))
                                dr["MAX_POWER"] = dtPro.Rows[j]["MAXPOWER"].ToString().Trim();
                            dr["STORAGE_LOCATION"] = dtPro.Rows[j]["STORAGE"].ToString().Trim();
                        }
                    }
                    dr["PART_DESC"] = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();

                    dtGetByPartId.Rows.Add(dr);
                    gcList.DataSource = dtGetByPartId;
                    txtDesc.Text = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();
                    txtPartId.Text = dsGetByPartId2.Rows[0]["PART_ID"].ToString().Trim();
                    OnAfterStateChanged(ControlState.Edit);
                }
            }
        }
        private new delegate void AfterStateChanged(ControlState controlState);
        public ControlState _controlState = ControlState.Empty;
        private new AfterStateChanged afterStateChanged = null;
       
        DataTable _dtProductGrade = null;

        private new ControlState State
        {
            get
            {
                return _controlState;
            }
            set
            {
                _controlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        ///方法
        #region
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="controlState"></param>
        private void OnAfterStateChanged(ControlState controlState)
        {
            switch (controlState)
            {
                #region case state of empty
                case ControlState.Empty:

                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

                    break;
                #endregion

                #region case state of Read
                case ControlState.Read:
                   
                    break;
                #endregion

                #region case state of edit
                case ControlState.Edit:
                    txtPartId.Enabled = false;
                    txtDesc.Enabled = false;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtPartId.Enabled = true;
                    txtDesc.Enabled = true;
                    gcList.DataSource = null;
                    txtPartId.SelectAll();
                    txtDesc.Text = "";
                    break;
                #endregion
            }
        }
        private DataTable GetRules()
        {
            string[] columns = new string[] { "MINPOWER", "GRADES", "ITEMNO", "STORAGE", "MAXPOWER", "MODULE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_ProductInbound_Rules");
            DataTable dtPro = BaseData.Get(columns, category);
            return dtPro;
        }

        private void ByProductPartForm_Load(object sender, EventArgs e)
        {
            ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
            txtPartId.Focus();
            txtPartId.SelectAll();
            gridPartNum.DisplayMember = "PART_NUMBER";
            gridPartNum.ValueMember = "PART_NUMBER";
            gridPartNum.DataSource = byProductPartEntity.GetByPartId().Tables[0];
            gvPartView.BestFitColumns();
            
        }

        /// <summary>
        /// 获取产品等级的显示值。
        /// </summary>
        /// <returns>产品等级的显示值</returns>
        private string GetProductGradeDisplayText(string value)
        {
            string displayText = value;
            try
            {
                if (this._dtProductGrade == null)
                {
                    string[] columns = new string[] { "Column_Name", "Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                    this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtProductGrade)
                {
                    DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_code='{0}'", value));
                    if (drs.Length > 0)
                    {
                        displayText = Convert.ToString(drs[0]["Column_Name"]);
                    }
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }

        /// <summary>
        /// 产品等级的隐值。
        /// </summary>
        /// <returns>产品等级的隐值</returns>
        private string GetProductGrade(string value)
        {
            string displayText = value;
            try
            {
                if (this._dtProductGrade == null)
                {
                    string[] columns = new string[] { "Column_Name", "Column_code" };
                    KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_TestRule_PowerSet");
                    List<KeyValuePair<string, string>> whereCondition = new List<KeyValuePair<string, string>>();
                    whereCondition.Add(new KeyValuePair<string, string>("Column_type", "ProductGrade"));
                    this._dtProductGrade = BaseData.GetBasicDataByCondition(columns, category, whereCondition);
                }
                if (null != this._dtProductGrade)
                {
                    var lnq = from item in this._dtProductGrade.AsEnumerable()
                              where Convert.ToString(item["Column_Name"]).Trim() == value.Trim()
                              select item;
                    //DataRow[] drs = this._dtProductGrade.Select(string.Format("Column_Name='{0}'", value));
                    if (lnq.Count()>0)
                    {
                        DataRow dr = lnq.First();
                        displayText = Convert.ToString(dr["Column_code"]);
                    }
                }
            }
            catch
            {
                displayText = value;
            }
            return displayText;
        }
        #endregion

        #region
        private void txtPartId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    string partId = string.Empty;
                    DataSet dsCheckPart = new DataSet();
                    //chack料号是否存在且可用
                    ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
                    partId = txtPartId.Text.Trim();
                    dsCheckPart = byProductPartEntity.CheckPart(partId);
                    if (dsCheckPart.Tables[0].Rows.Count > 0)
                    {
                        DataSet dsGetByPartId = byProductPartEntity.GetByPartId(partId);
                        DataTable dtGetByPartId = new DataTable();
                        DataTable dsGetByPartId2 = new DataTable();
                        dtGetByPartId = dsGetByPartId.Tables["POR_PART_BYPRODUCT"];
                        dsGetByPartId2 = dsGetByPartId.Tables["POR_PART"];
                        //if (!dtGetByPartId.Columns.Contains("ROWNUMBER"))
                        //    dtGetByPartId.Columns.Add("ROWNUMBER");
                        if (dtGetByPartId.Rows.Count > 0)
                        {
                            //for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                            //{
                            //    dtGetByPartId.Rows[i]["ROWNUMBER"] = i + 1;
                            //}
                            //如果存在且已经有对应料号则带出所有的对应料号信息
                           
                            //遍历查询结果
                            for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                            {
                                string gradesOut = string.Empty;
                                string gradesOutUsed = string.Empty;
                                string grades = dtGetByPartId.Rows[i]["GRADES"].ToString().Trim();    ///获取等级列信息
                                string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                                for (int j = 0; j < arrTemp.Length; j++)
                                {
                                    gradesOut += GetProductGradeDisplayText(arrTemp[j]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                                }
                                gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                                dtGetByPartId.Rows[i]["GRADES"] = gradesOutUsed;
                            }///列重新赋值

                            gcList.DataSource = dtGetByPartId;
                            txtDesc.Text = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();
                            OnAfterStateChanged(ControlState.Edit);
                        }
                        else
                        {
                            //如果存在但未进行绑定联副产品则带出首条对应料号即为主料号
                            DataRow dr = dtGetByPartId.NewRow();
                            dr["ITEM_NO"] = "1";
                            dr["PART_NUMBER"] = partId;
                            
                            DataTable dtPro = new DataTable();
                            dtPro = GetRules();
                            for (int j = 0; j < dtPro.Rows.Count; j++)
                            {
                                DataRow drPro = dtPro.Rows[j];
                                if (drPro["ITEMNO"].ToString().Trim() == "1" && drPro["MODULE"].ToString().Trim() == dsGetByPartId2.Rows[0]["PART_MODULE"].ToString().Trim())
                                {
                                    string gradesOut = string.Empty;
                                    string gradesOutUsed = string.Empty;
                                    string grades = dtPro.Rows[j]["GRADES"].ToString().Trim();    ///获取等级列信息
                                    string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                                    for (int i = 0; i < arrTemp.Length; i++)
                                    {
                                        gradesOut += GetProductGradeDisplayText(arrTemp[i]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                                    }
                                    gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                                    
                                    dr["GRADES"] = gradesOutUsed;                      ///列重新赋值
                                                                                       ///if (string.IsNullOrEmpty(dtPro.Rows[j]["MINPOWER"].ToString().Trim()))
                                    if (!string.IsNullOrEmpty(dtPro.Rows[j]["MINPOWER"].ToString().Trim()))
                                        dr["MIN_POWER"] = dtPro.Rows[j]["MINPOWER"].ToString().Trim();

                                    if (!string.IsNullOrEmpty(dtPro.Rows[j]["MAXPOWER"].ToString().Trim()))
                                        dr["MAX_POWER"] = dtPro.Rows[j]["MAXPOWER"].ToString().Trim();
                                    
                                    dr["STORAGE_LOCATION"] = dtPro.Rows[j]["STORAGE"].ToString().Trim();
                                }
                            }
                            dr["PART_DESC"] = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();

                            dtGetByPartId.Rows.Add(dr);
                            gcList.DataSource = dtGetByPartId;
                            txtDesc.Text = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();
                            OnAfterStateChanged(ControlState.Edit);
                        }
                    }
                    else
                    {
                        //如果不存在料号则提示料号不存在，请先进行料号维护
                        //MessageBox.Show("料号不存在,请先进行料号维护,然后执行此操作！", "系统错误提示");
                        MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0001}"), StringParser.Parse("${res:Global.SystemInfo}"));
                        
                        txtPartId.Focus();
                        txtPartId.SelectAll();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));

                }
            }
        }
        
        private void sbtAdd_Click(object sender, EventArgs e)
        { 
            if (gvList.DataSource == null)
            {
                //MessageBox.Show("列表中无主料号信息,请先填写主料料号,然后回车！", "系统错误提示");
                MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0002}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            DataTable dt = ((DataView)gvList.DataSource).Table;
            DataRow dr = dt.NewRow();
            dr["ITEM_NO"] = dt.Rows.Count + 1;
            DataTable dtPro = new DataTable();
            dtPro = GetRules();
            for (int j = 0; j < dtPro.Rows.Count; j++)
            {
                DataRow drPro = dtPro.Rows[j];
                //string rownumber = string.Empty;
                //if (Convert.ToInt32(dr["ROWNUMBER"].ToString().Trim()) >= 6)
                //{
                //    rownumber = "6";
                //}
                //else
                //{
                //    rownumber = ;
                //}

                if (drPro["ITEMNO"].ToString().Trim() == dr["ITEM_NO"].ToString().Trim() && drPro["MODULE"].ToString().Trim() == "")
                {
                    string gradesOut = string.Empty;
                    string gradesOutUsed = string.Empty;
                    string grades = dtPro.Rows[j]["GRADES"].ToString().Trim();    ///获取等级列信息
                    string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        gradesOut += GetProductGradeDisplayText(arrTemp[i]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                    }
                    gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号

                    dr["GRADES"] = gradesOutUsed;                      ///列重新赋值
                    dr["STORAGE_LOCATION"] = dtPro.Rows[j]["STORAGE"].ToString().Trim();
                }
                
            }
            dt.Rows.Add(dr);
        }

        private void sbtDel_Click(object sender, EventArgs e)
        {
            if (this.gvList.GetFocusedRow() != null)
            {
                if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
                {
                    this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
                }
                //this.gvList.UpdateCurrentRow();
                int rowHandle = this.gvList.FocusedRowHandle;
                this.gvList.DeleteRow(rowHandle);

                if (rowHandle == 0)
                {
                    OnAfterStateChanged(ControlState.New);
                    return;
                }
                ((DataView)gvList.DataSource).Table.AcceptChanges();
                DataTable dt = ((DataView)gvList.DataSource).Table; ;//.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    //if (!dt.Columns.Contains("ITEM_NO"))
                    //    dt.Columns.Add("ITEM_NO");
                    //for (int i = 1; i < dt.Rows.Count + 1; i++)
                    //    dt.Rows[i - 1]["ITEM_NO"] = i.ToString();
                    gcList.Refresh();
                    gcList.DataSource = dt;
                }
                else
                {
                    gcList.DataSource = null;     //清空datasource  
                }

            }
            else
            {
                //MessageService.ShowMessage("必须选择至少选择一条记录", "${res:Global.SystemInfo}");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0003}"), StringParser.Parse("${res:Global.SystemInfo}"));
            }
        }        

        private void sbtNew_Click(object sender, EventArgs e)
        {
            OnAfterStateChanged(ControlState.New);
        }

        private void sbtClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        #endregion

        private void gvList_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.ColumnEditName == "gridPartNum")
            {
                string  partItemNum = (e.RowHandle +1).ToString();
                string partNum = e.Value.ToString();
                ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
                DataTable dtParM = byProductPartEntity.CheckPart(partNum).Tables[0];
                //if (Convert.ToInt32(partItemNum) < 4)
                //{             
                GetRules();
                DataTable dtPro = new DataTable();
                dtPro = GetRules();
                for (int j = 0; j < dtPro.Rows.Count; j++)
                {
                    DataRow drPro = dtPro.Rows[j];
                    if (drPro["ITEMNO"].ToString().Trim() == partItemNum && drPro["MODULE"].ToString().Trim() == dtParM.Rows[0]["PART_MODULE"].ToString().Trim())
                    {
                        if (string.IsNullOrEmpty(dtPro.Rows[j]["MINPOWER"].ToString().Trim()))
                            gvList.SetRowCellValue(e.RowHandle, "MIN_POWER", null);
                        else
                            gvList.SetRowCellValue(e.RowHandle, "MIN_POWER", dtPro.Rows[j]["MINPOWER"].ToString().Trim());

                        if (string.IsNullOrEmpty(dtPro.Rows[j]["MAXPOWER"].ToString().Trim()))
                            gvList.SetRowCellValue(e.RowHandle, "MAX_POWER", null);
                        else
                            gvList.SetRowCellValue(e.RowHandle, "MAX_POWER", dtPro.Rows[j]["MAXPOWER"].ToString().Trim());
                        gvList.SetRowCellValue(e.RowHandle, "PART_DESC", dtParM.Rows[0]["PART_DESC"].ToString().Trim());
                        break;
                    }
                    else
                    {
                        gvList.SetRowCellValue(e.RowHandle, "PART_DESC", dtParM.Rows[0]["PART_DESC"].ToString().Trim());
                        gvList.SetRowCellValue(e.RowHandle, "MIN_POWER", null);
                        gvList.SetRowCellValue(e.RowHandle, "MAX_POWER", null);
                    }
                }
            }

            
             
        }

        private void sbtSave_Click(object sender, EventArgs e)
        {
            if (gvList.DataSource == null)
            {
                //MessageService.ShowMessage("没有要保存的数据!", "${res:Global.SystemInfo}");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0004}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            if (string.IsNullOrEmpty(txtPartId.Text.Trim()))
            {
                //MessageService.ShowMessage("请输入产品料号,然后回车!", "${res:Global.SystemInfo}");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0005}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            try
            {
                //if (MessageBox.Show(StringParser.Parse("是否保存当前联副产品的信息？"),
                if (MessageBox.Show(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0006}"),
                           StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (this.gvList.State == GridState.Editing && this.gvList.IsEditorFocused && this.gvList.EditingValueModified)
                    {
                        this.gvList.SetFocusedRowCellValue(this.gvList.FocusedColumn, this.gvList.EditingValue);
                    }
                    this.gvList.UpdateCurrentRow();
                    string name = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                    ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
                    DataTable dtGvlist = ((DataView)gvList.DataSource).Table;
                    for (int i = 0; i < dtGvlist.Rows.Count; i++)
                    {
                        string grades = dtGvlist.Rows[i]["GRADES"].ToString().Trim();///获取等级列信息
                        string gradesOut = string.Empty;
                        string[] arrTemp = grades.Split(',');                         ///数组取值（等级组所有等级）
                        for (int j = 0; j < arrTemp.Length; j++)
                        {
                            gradesOut += GetProductGrade(arrTemp[j]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                        }
                        grades = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                        dtGvlist.Rows[i]["GRADES"] = grades;

                    }
                    string partNum = txtPartId.Text.Trim();
                    DataSet dsReturn = byProductPartEntity.GetInfFromPorPartAndProductPart(name,partNum,dtGvlist);
                    if (dsReturn.Tables.Count > 0)
                    {
                        if (dsReturn.Tables["OUTPUT_PARAM_TABLE"].Rows[0]["CODE"].ToString().Trim() == "0")
                        {
                            MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0007}"), StringParser.Parse("${res:Global.SystemInfo}"));//保存成功
                            ByProductPartEntity proentity = new ByProductPartEntity();                             
                            DataSet dsGetByPartId = proentity.GetByPartId(partNum);
                            DataTable dtGetByPartId = new DataTable();
                            DataTable dsGetByPartId2 = new DataTable();
                            dtGetByPartId = dsGetByPartId.Tables["POR_PART_BYPRODUCT"];
                            dsGetByPartId2 = dsGetByPartId.Tables["POR_PART"];
                            //if (!dtGetByPartId.Columns.Contains("ITEM_NO"))
                            //    dtGetByPartId.Columns.Add("ITEM_NO");
                            //for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                            //{
                            //    dtGetByPartId.Rows[i]["ROWNUMBER"] = i + 1;
                            //}
                            //如果存在且已经有对应料号则带出所有的对应料号信息
                           
                            //遍历查询结果
                            for (int i = 0; i < dtGetByPartId.Rows.Count; i++)
                            {
                                string gradesOut = string.Empty;
                                string gradesOutUsed = string.Empty;
                                string grades = dtGetByPartId.Rows[i]["GRADES"].ToString().Trim();    ///获取等级列信息
                                string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                                for (int j = 0; j < arrTemp.Length; j++)
                                {
                                    gradesOut += GetProductGradeDisplayText(arrTemp[j]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                                }
                                gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                                dtGetByPartId.Rows[i]["GRADES"] = gradesOutUsed;
                            }///列重新赋值

                            gcList.DataSource = dtGetByPartId;
                            txtDesc.Text = dsGetByPartId2.Rows[0]["PART_DESC"].ToString().Trim();
                            OnAfterStateChanged(ControlState.Edit);
                            
                            return;
                        }
                        else
                        {
                            MessageService.ShowMessage(dsReturn.Tables["OUTPUT_PARAM_TABLE"].Rows[0]["MESSAGE"].ToString().Trim(), StringParser.Parse("${res:Global.SystemInfo}"));
                            return;
                        }
                    }
                    else
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0008}"), StringParser.Parse("${res:Global.SystemInfo}"));//保存输入时服务端异常,联系管理员!
                        return;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message, StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
        }

        private void gvList_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvList.FocusedRowHandle == 0 && gvList.FocusedColumn == this.gcPartNum)
            {
                e.Cancel = true;
            }
            else if (gvList.FocusedRowHandle == 0 && gvList.FocusedColumn == this.gcItemNo)
            {
                e.Cancel = true;
            }
        }

        private void spbDel_Click(object sender, EventArgs e)
        {
            if (txtPartId.Enabled == true)
            {
                //MessageService.ShowMessage("当前没有信息可以删除,请确认!", "${res:Global.SystemInfo}");
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0009}"), StringParser.Parse("${res:Global.SystemInfo}"));
                return;
            }
            ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
            //if (MessageService.AskQuestion("确定删除吗?", "系统提示"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartForm.msg.0010}"), StringParser.Parse("${res:Global.SystemInfo}")))
            {//系统提示你确定要删除吗？
                if (byProductPartEntity.Delete(txtPartId.Text.ToString().Trim()))
                {
                    OnAfterStateChanged(ControlState.New);
                }
            }
        }

        private void gvList_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "MIN_POWER")
            {
                if (string.IsNullOrEmpty(e.Value.ToString().Trim()))
                {
                    gvList.SetRowCellValue(e.RowHandle, "MIN_POWER", null);
                }
            }
            if (e.Column.FieldName == "MAX_POWER")
            {
                if (string.IsNullOrEmpty(e.Value.ToString().Trim()))
                {
                    gvList.SetRowCellValue(e.RowHandle, "MAX_POWER", null);
                }
            }
        }
        
    }
}
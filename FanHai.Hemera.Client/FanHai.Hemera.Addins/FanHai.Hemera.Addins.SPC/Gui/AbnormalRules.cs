using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Framework.Gui;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;


namespace FanHai.Hemera.Addins.SPC
{
    /// <summary>
    /// SPC异常规则设定
    /// </summary>
    public partial class AbnormalRules : BaseUserCtrl
    {
        string abnormalKey = "", abnormalKey_Dtl = "";
        //string strFlagMain = "";
        string strFlagDtl = "";
        SpcEntity spcEntity;
        private bool isAddColorField = false;
        private static bool isContinue = false;
        ControlState _ctrlState = new ControlState();

        public new delegate void AfterStateChanged(ControlState controlState);
        public new AfterStateChanged afterStateChanged = null;

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

        public AbnormalRules()
        {
            InitializeComponent();
            spcEntity = new SpcEntity();
        }
      
        public void onChangeControlState(ControlState cState)
        {
            switch (cState)
            {
                case  ControlState.Edit:
                    this.gridView1.OptionsBehavior.Editable = true;
                    this.gridView2.OptionsBehavior.Editable = true;
                    this.btnAdd.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnEdit.Enabled = true;
                    this.btnCancel.Enabled = true;
                    break;                
                case ControlState.New:
                    this.gridView1.OptionsBehavior.Editable = true;
                    this.gridView2.OptionsBehavior.Editable = true;
                    this.btnEdit.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.btSave.Enabled = true;
                    this.btnAdd.Enabled = true;
                    this.btnCancel.Enabled = true;
                    break;
                case ControlState.ReadOnly:
                    this.gridView1.OptionsBehavior.Editable = false;
                    this.gridView2.OptionsBehavior.Editable = false;
                    this.btnAdd.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btnEdit.Enabled = true;
                    this.btSave.Enabled = false;
                    this.btnCancel.Enabled = false;
                    break;
                case ControlState.Delete:
                    this.gridView1.OptionsBehavior.Editable = false;
                    this.gridView2.OptionsBehavior.Editable = false;
                    this.btnAdd.Enabled = true;
                    this.btnDelete.Enabled = true;
                    this.btnEdit.Enabled = true;
                    this.btSave.Enabled = true;
                    this.btnCancel.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            DataSet dsForAbnormalRule = new DataSet();
            DataSet dsOld = ((DataView)gridView1.DataSource).Table.DataSet;

            DataTable dtMain = ((DataView)gridView1.DataSource).Table;
            DataTable dtDtl = dtMain.DataSet.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];

            if (_ctrlState == ControlState.Edit || _ctrlState == ControlState.Delete)
            {
                dsOld = ((DataView)gridView1.DataSource).Table.DataSet.GetChanges(DataRowState.Modified);
                if (dsOld == null)
                {
                    MessageService.ShowMessage("未作任何修改,无需保存!");
                    this.CtrlState = ControlState.ReadOnly;
                    return;
                }
                else
                {
                    dtMain = dsOld.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                    dtDtl = dsOld.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];
                }
            }
            else if (_ctrlState == ControlState.New)
            {
                int length_Main = dtMain.Select("FLAGMAIN='NEW'").Length;
                int length_Dtl = dtDtl.Select("FLAGDTL='NEW'").Length;

                if (length_Main < 1 && length_Dtl < 1)
                {
                    MessageService.ShowMessage("没有新增数据,无需保存!");
                    this.CtrlState = ControlState.ReadOnly;
                    return;
                }
            }
            else
            {
                MessageService.ShowMessage("数据为只读状态，无法保存!");
                return;
            }

            if (dtMain.Rows.Count <0)
            {
                MessageService.ShowMessage("数据异常,请与管理员联系!");
                return;
            }

            DataTable saveTableMain = Utils.Common.Utils.CreateDataTableWithColumns(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME, new EDC_ABNORMAL_FIELDS().FIELDS.Keys.ToList<string>());
            DataTable saveTableMainForUpdate = saveTableMain.Clone();
            saveTableMainForUpdate.TableName = EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE;
            string[] astr = new string[3];
            //新增 主表
            foreach (DataRow drMain in dtMain.Select("FLAGMAIN='NEW'"))
            {
                astr[0] = drMain[EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString().ToUpper();
                astr[1] = "INSERT";
                if (!spcEntity.IsExistAbnormalCode(astr))
                {
                    MessageService.ShowMessage("规则代码重复,无法新增!");
                    return;
                }
                
                SetRowDataToTable(drMain, saveTableMain, true, true);
                if (!isContinue) return;
            }//修改 主表
            foreach(DataRow dr in dtMain.Select("FLAGMAIN<>'NEW'"))
            {
                astr[0] = dr[EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString().ToUpper();
                astr[1] = "UPDATE";
                astr[2] = dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString();
                if (!spcEntity.IsExistAbnormalCode(astr))
                {
                    MessageService.ShowMessage("规则代码重复,无法修改!");
                    return;
                }
                
                SetRowDataToTable(dr, saveTableMainForUpdate, false, true);
                if (!isContinue) return;
            }
            if (saveTableMain.Rows.Count > 0)
                dsForAbnormalRule.Merge(saveTableMain, false, MissingSchemaAction.Add);

            if (saveTableMainForUpdate.Rows.Count > 0)
                dsForAbnormalRule.Merge(saveTableMainForUpdate, false, MissingSchemaAction.Add);

            DataTable saveTableDtl = Utils.Common.Utils.CreateDataTableWithColumns(EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME, new EDC_ABNORMAL_DTL_FIELDS().FIELDS.Keys.ToList<string>());
            DataTable saveTableDtlForUpdate = saveTableDtl.Clone();
            saveTableDtlForUpdate.TableName = EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME_FORUPDATE;
            //新增 子表
            foreach (DataRow drDtl in dtDtl.Select("FLAGDTL='NEW'"))
            {               
                SetRowDataToTable(drDtl, saveTableDtl, true, false);
                if (!isContinue) return;
            }
            //修改 子表
            foreach(DataRow dr in dtDtl.Select("FLAGDTL<>'NEW'"))
            {
                SetRowDataToTable(dr, saveTableDtlForUpdate, false, false);
                if (!isContinue) return;
            }

            if (saveTableDtl.Rows.Count > 0)
                dsForAbnormalRule.Merge(saveTableDtl, false, MissingSchemaAction.Add);
            if (saveTableDtlForUpdate.Rows.Count > 0)
                dsForAbnormalRule.Merge(saveTableDtlForUpdate, false, MissingSchemaAction.Add);

            SaveData(dsForAbnormalRule);
        }
        /// <summary>
        /// 添加数据到表中
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="saveTable">添加数据的表明</param>
        /// <param name="isNew">是否新增到数据表</param>
        /// <param name="isMast">是否为主要的数据表</param>
        private static void SetRowDataToTable(DataRow dr, DataTable saveTable, bool isNew, bool isMast)
        {
            isContinue = true;
            if (isMast)//主表
            {                
                string _abnormalKey = dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString();
                string _arulecode = dr[EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString();
                string _abnormaldesc = dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALDESC].ToString();
                string _abnormalcolor = dr[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR].ToString();
                string _lvorm = dr[EDC_ABNORMAL_FIELDS.FIELD_LVORM].ToString() == "" ? "0" : dr[EDC_ABNORMAL_FIELDS.FIELD_LVORM].ToString();
                if (string.IsNullOrEmpty(_abnormalKey) || string.IsNullOrEmpty(_arulecode))
                {
                    MessageService.ShowMessage("规则代码不能为空!");
                    isContinue = false;
                    return;
                }
               
                if (isNew && isMast)//新增 主表
                {
                    Dictionary<string, string> rowDataMain = new Dictionary<string, string>()
                                                                            { 
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID,_abnormalKey},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALDESC,_abnormaldesc},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ARULECODE,_arulecode},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR,_abnormalcolor},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_CREATOR,  PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_CREATE_TIME, DateTime.Now.ToShortDateString()},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_LVORM,_lvorm}
                                                                            };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTable, rowDataMain);
                }

                if (!isNew && isMast)//修改 主表
                {

                    Dictionary<string, string> rowDataMain = new Dictionary<string, string>()
                                                                            { 
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID,_abnormalKey},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALDESC,_abnormaldesc},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ARULECODE,_arulecode},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR,_abnormalcolor},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDITOR,  PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToShortDateString()},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_LVORM,_lvorm}
                                                                            };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTable, rowDataMain);
                }
               
            }
            if (!isMast)//子表
            {
                string _flagDtl = dr["FLAGDTL"].ToString();
                string _abnormalKey = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID].ToString();
                string _abnormalKey_Dtl = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL].ToString();

                string _watchPoints = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS].ToString();
                string _overRulePoints = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS].ToString();
                string _ruleValue = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE].ToString();

                string _compareRule = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE].ToString();
                string _compareSign = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN].ToString();
                string _lvorm = dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_LVORM].ToString() == "" ? "0" : dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_LVORM].ToString();

                if (string.IsNullOrEmpty(_watchPoints))
                {
                    MessageService.ShowMessage("监控点数不能为空!");                    
                    isContinue = false;
                    return;
                }
                if (string.IsNullOrEmpty(_overRulePoints))
                {
                    MessageService.ShowMessage("超规点数不能为空!");
                    isContinue = false;
                    return;
                }
                if (Convert.ToInt32(_overRulePoints) > Convert.ToInt32(_watchPoints))
                {
                    MessageService.ShowMessage("超规点数不能大于监控点数!");
                    isContinue = false;
                    return;
                }
                if (string.IsNullOrEmpty(_compareSign))
                {
                    MessageService.ShowMessage("比较符不能为空!");
                    isContinue = false;
                    return;
                }
                if (_compareSign.ToUpper() == SPC_ABNORMAL_COMPARE.SIGN_DECREASING || _compareSign.ToUpper() == SPC_ABNORMAL_COMPARE.SIGN_INCREASING
                    || _compareSign.ToUpper() == SPC_ABNORMAL_COMPARE.SIGN_STRICTLYDECREASING || _compareSign.ToUpper() == SPC_ABNORMAL_COMPARE.SIGN_STRICTLYINCREASING
                    || _compareSign.ToUpper() == SPC_ABNORMAL_COMPARE.SIGN_ALTERNATING)
                {
                    _compareRule = string.Empty;
                    _ruleValue = string.Empty;
                }
                else
                {
                    if (string.IsNullOrEmpty(_compareRule))
                    {
                        MessageService.ShowMessage("比较规格不能为空!");
                        isContinue = false;
                        return;
                    }
                    if (string.IsNullOrEmpty(_ruleValue))
                    {
                        MessageService.ShowMessage("规格值不能为空!");
                        isContinue = false;
                        return;
                    }
                }
             
                if (isNew && !isMast)//新增 子表
                {
                    Dictionary<string, string> rowDataDtl = new Dictionary<string, string>()
                                                                            { 
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID,_abnormalKey},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL,_abnormalKey_Dtl},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE,_compareRule},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN,_compareSign},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS,_overRulePoints},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS,_watchPoints},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE,_ruleValue},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_CREATOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_CREATE_TIME,DateTime.Now.ToShortDateString()},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_LVORM,_lvorm}
                                                                            };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTable, rowDataDtl);

                }
                if (!isNew && !isMast)//修改 子表
                {
                    Dictionary<string, string> rowDataDtl = new Dictionary<string, string>()
                                                                            { 
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID,_abnormalKey},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL,_abnormalKey_Dtl},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARERULE,_compareRule},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_COMPARESIGN,_compareSign},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_OVERRULEPOINTS,_overRulePoints},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_WATCHPOINTS,_watchPoints},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_RULEVALUE,_ruleValue},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_EDITOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME)},
                                                                                {EDC_ABNORMAL_DTL_FIELDS.FIELD_EDIT_TIME,DateTime.Now.ToShortDateString()},
                                                                                {EDC_ABNORMAL_FIELDS.FIELD_LVORM,_lvorm}
                                                                            };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToTable(saveTable, rowDataDtl);

                }
            }
        }

        public void SaveData(DataSet dsSave)
        {
            if (spcEntity.SaveAbnormalRule(dsSave))
            {
                MessageService.ShowMessage("保存成功!");
                BindGv();
            }

        }

        private void tbnEdit_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.Edit;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataSet ds = ((DataTable)gcMainAbnormal.DataSource).DataSet;
            //新增子表
            if (!string.IsNullOrEmpty(abnormalKey))
            {
                DataTable dtDtl = ds.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];
                //如果异常子规则超过两笔，则此规则不允许新增
                DataRow[] drSubRules = dtDtl.Select(string.Format("ABNORMALID='{0}'", abnormalKey));
                if (drSubRules.Length > 1)
                {
                    string mainRule = ds.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME].Select(string.Format("ABNORMALID='{0}'", abnormalKey))[0][EDC_ABNORMAL_FIELDS.FIELD_ARULECODE].ToString();
                    MessageService.ShowError(string.Format("异常规则{0},子规则不能大于2笔!", mainRule));
                    abnormalKey = string.Empty;
                }
                else
                {
                    abnormalKey_Dtl =  FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);

                    DataRow drDtl = dtDtl.NewRow();
                    drDtl[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID] = abnormalKey;
                    drDtl[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL] = abnormalKey_Dtl;
                    drDtl["FLAGDTL"] = "NEW";
                    dtDtl.Rows.Add(drDtl);
                    dtDtl.AcceptChanges();
                }

            }
            //主表子表一起新增
            if (string.IsNullOrEmpty(abnormalKey))
            {
                abnormalKey =  FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                abnormalKey_Dtl =  FanHai.Hemera.Share.Common.CommonUtils.GenerateNewKey(0);
                DataTable dtMain = ds.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];
                DataTable dtDtl = ds.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];
                //dtDtl.Rows.Clear();
                //dtMain.Rows.Clear();
            

                DataRow drMain = dtMain.NewRow();
                drMain[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID] = abnormalKey;
                drMain[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR] = "Red";
                drMain["FLAGMAIN"] = "NEW";
                dtMain.Rows.Add(drMain);

                DataRow drDtl = dtDtl.NewRow();
                drDtl[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID] = abnormalKey;
                drDtl[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL] = abnormalKey_Dtl;
                drDtl["FLAGDTL"] = "NEW";

                dtDtl.Rows.Add(drDtl);
                dtMain.AcceptChanges();
                dtDtl.AcceptChanges();
            }
          
           
            this.gcMainAbnormal.DataSource = null;
            this.gcMainAbnormal.DataSource = ds.Relations[0].ParentTable;

            this.CtrlState = ControlState.New;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow dr = (gcMainAbnormal.FocusedView as GridView).GetFocusedDataRow();
            if (dr == null) return;

            if (MessageService.AskQuestion("确定要删除该信息么?", "提示"))
            {             
                dr[EDC_ABNORMAL_DTL_FIELDS.FIELD_LVORM] = 1;//规则主表字表都具有该栏位
                this.CtrlState = ControlState.Delete;
            }
        }

        private void AbnormalRules_Load(object sender, EventArgs e)
        {
            afterStateChanged += new AfterStateChanged(onChangeControlState);

            BindGv();

            gridView1.SetMasterRowExpandedEx(0, 0, true);
            gridView1.SetMasterRowExpanded(1, true);
            //gridView1.SetMasterRowExpanded(2, true);
        }

        private void GridNullBind()
        {
            DataSet dsNull = new DataSet();

        }

        private void BindGv()
        {
            DataSet dataSet = spcEntity.GetAbnormalRule();
            InitialData(dataSet);
        }

        private void InitialData(DataSet dataSet)
        {
            try
            {
                this.CtrlState = ControlState.ReadOnly;
                
                if (spcEntity.ErrorMsg.Length > 0)
                {
                    MessageService.ShowError("get spc AbnormalRule error:" + spcEntity.ErrorMsg);
                }
                else
                {
                    gridView1.Columns[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR].Visible = false;

                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables.Contains(EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME))
                    {
                        this.gcMainAbnormal.DataSource = null;
                        this.gcMainAbnormal.DataSource = dataSet.Relations[0].ParentTable;

                        if (!isAddColorField)
                        {
                            GridColumn unboundColumn = gridView1.Columns.AddField("Color");
                            unboundColumn.VisibleIndex = gridView1.Columns.Count;
                            unboundColumn.UnboundType = DevExpress.Data.UnboundColumnType.Object;
                            RepositoryItemColorEdit ce = new RepositoryItemColorEdit();
                            ce.ShowCustomColors = false;
                            unboundColumn.ColumnEdit = ce;
                            isAddColorField = true;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message);
            }
        }
        /// <summary>
        /// 新增子规则，如果子规则>2笔，就会新增主规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            abnormalKey = gridView1.GetRowCellValue(e.RowHandle, EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID).ToString();
        }

        private void gridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            GridView view = sender as GridView;
            DataView dv = view.DataSource as DataView;
            if (e.IsGetData)
                e.Value = GetColorFromString(dv[e.ListSourceRowIndex][EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR].ToString());
            else
                dv[e.ListSourceRowIndex][EDC_ABNORMAL_FIELDS.FIELD_ABNORMALCOLOR] = ((Color)e.Value).Name;
        }
        Color GetColorFromString(string colorString)
        {
            Color color = Color.Empty;
            ColorConverter converter = new ColorConverter();
            try
            {
                if (string.IsNullOrEmpty(colorString)) colorString = "red";
                color = (Color)converter.ConvertFromString(colorString);
            }
            catch
            { }
            return color;
        }
        /// <summary>
        /// 新增子规则，如果子规则>2笔，就会新增主规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_RowClick(object sender, RowClickEventArgs e) 
        {
            abnormalKey = ((DataRowView)(((GridView)sender).GetRow(e.RowHandle))).Row[EDC_ABNORMAL_FIELDS.FIELD_ABNORMALID].ToString();
            abnormalKey_Dtl = ((DataRowView)(((GridView)sender).GetRow(e.RowHandle))).Row[EDC_ABNORMAL_DTL_FIELDS.FIELD_ABNORMALID_DTL].ToString();            
        }
        /// <summary>
        /// 取消按钮，还原正在编辑和正在新增的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CtrlState = ControlState.ReadOnly;
            abnormalKey = "";
            DataSet dsCancel = ((DataView)gridView1.DataSource).Table.DataSet;
            dsCancel.RejectChanges();
            DataRow[] drMain = dsCancel.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME].Select("FLAGMAIN='NEW'");
            DataRow[] drDtl = dsCancel.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME].Select("FLAGDTL='NEW'");
            foreach (DataRow dr in drDtl)
                dsCancel.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME].Rows.Remove(dr);
            foreach (DataRow dr in drMain)
                dsCancel.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME].Rows.Remove(dr);

            InitialData(dsCancel);
        }

        string tmpValueDtl = string.Empty, tmpValueMain = string.Empty;
        private void gridView2_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (!e.Value.ToString().Trim().Equals(tmpValueDtl))
            {
                if (!string.IsNullOrEmpty(abnormalKey_Dtl.Trim()))
                {
                    DataSet dsData = ((DataView)gridView1.DataSource).Table.DataSet;
                    DataTable dtDtl = dsData.Tables[EDC_ABNORMAL_DTL_FIELDS.DATABASE_TABLE_NAME];

                    DataRow[] drs = dtDtl.Select(string.Format("ABNORMALID_DTL='{0}'", abnormalKey_Dtl.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
            }
        }

        private void gridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (!e.Value.ToString().Trim().Equals(tmpValueMain))
            {
                if (!string.IsNullOrEmpty(abnormalKey.Trim()))
                {
                    DataSet dsData = ((DataView)gridView1.DataSource).Table.DataSet;
                    DataTable dtMain = dsData.Tables[EDC_ABNORMAL_FIELDS.DATABASE_TABLE_NAME];

                    DataRow[] drs = dtMain.Select(string.Format("ABNORMALID='{0}'", abnormalKey.Trim()));
                    drs[0][e.Column.FieldName] = e.Value;
                }
            }
        }

        private void gridView2_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            tmpValueDtl = e.CellValue.ToString().Trim();
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            tmpValueMain = e.CellValue.ToString().Trim();
        }
    }
}

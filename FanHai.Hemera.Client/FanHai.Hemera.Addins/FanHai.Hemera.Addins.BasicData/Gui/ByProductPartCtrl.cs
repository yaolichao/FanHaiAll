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
using FanHai.Hemera.Share.Interface;
using System.Threading;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.BasicData
{

    public partial class ByProductPartCtrl : BaseUserCtrl
    {
        ByProductEntity _byProductEntity = new ByProductEntity();
        string por_part = string.Empty;
        public string key = string.Empty;
        DataTable _dtProductGrade = null;
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
            switch (state)
            {
                #region case state of empty
                case ControlState.Empty:

                    break;
                #endregion

                #region case state of editer
                case ControlState.Edit:

                    break;
                #endregion

                #region case state of New
                case ControlState.New:

                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:

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
        public ByProductPartCtrl()
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);
            InitializeLanguage();
        }
        public void InitializeLanguage()
        {
            lblMenu.Text = "基础数据 > 工艺参数设置 > 连副产品";
            GridViewHelper.SetGridView(gvList);
            GridViewHelper.SetGridView(gdView);

            //this.tsbSeach.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0001}");//查询
            //this.tsbAdd.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0003}");//新增
            //this.tsbEdit.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0004}");//修改
            //tsbExportExcel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0006}");//导出Excel
            //grpCrtlCode.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0008}");//查询条件
            //lblPartId.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0009}");//产品料号
            //lblPartType.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0010}");//类型
            //lblPartMoudle.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0011}");//型号
            //lblClass.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.lbl.0012}");//分类
            //gcRowNum.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0001}");//序号
            //gcPartId.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0002}");//产品料号
            //gcPartDesc.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0003}");//物料描述
            //gcPartType.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0004}");//类型
            //gcPartMoudle.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0005}");//型号
            //gcPartClass.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0006}");//分类
            //gcTotol.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.GridControl.0007}");//对应产品数量
        }

        ///方法
        private void BindData()
        {
            string strPartId = txtPartId.Text.Trim();
            string strPartType = txtPartType.Text.Trim();
            string strPartModule = txtMoudle.Text.Trim();
            string strPartClass = txtClass.Text.Trim();
            ByProductPartEntity ByProductPartEntity = new ByProductPartEntity();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = ByProductPartEntity.GetByFourParameters(strPartId, strPartType, strPartModule, strPartClass, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(ByProductPartEntity.ErrorMsg))
            {
                MessageService.ShowMessage(ByProductPartEntity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcList.DataSource = dsReturn.Tables[0].DefaultView;
                gcList.MainView = gvList;
                gvList.BestFitColumns();
            }
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
        //事件
        //  关闭窗体
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        //add by chao.pang  20131204
        private void BindDataForPartType()
        {
            Part _part = new Part();
            DataSet dsPartType = _part.GetPartType();
            DataTable dt = dsPartType.Tables[0];
            DataRow drNew = dt.NewRow();
            drNew["PART_TYPE"] = "";
            dt.Rows.Add(drNew);
            dt.AcceptChanges();
            txtPartType.Properties.DataSource = dt.DefaultView;
            this.txtPartType.Properties.DisplayMember = "PART_TYPE";
            this.txtPartType.Properties.ValueMember = "PART_TYPE";
        }

        //窗体载入
        private void ByProductCtrl_Load(object sender, EventArgs e)
        {
            BindDataForPartType();
        }

        private void tsbSeach_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void gvList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "INDEX1": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }

        private void pgnQueryResult_DataPaging()
        {
            BindData();
        }

        private void gvList_MasterRowGetRelationCount(object sender, MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gvList_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            DataRow dr = this.gvList.GetDataRow(e.RowHandle);
            string partid = Convert.ToString(dr["PART_ID"]);
            ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
            DataSet dsReturn = byProductPartEntity.GetByPartId(partid);
            if (!string.IsNullOrEmpty(byProductPartEntity.ErrorMsg))
            {
                MessageService.ShowMessage(byProductPartEntity.ErrorMsg);
                return;
            }
            DataTable dtChild = new DataTable();
            //判定获取数据是否为空
            if (dsReturn != null || dsReturn.Tables.Count > 0)
            {
                dtChild = dsReturn.Tables[0];
                //遍历查询结果
                for (int i = 0; i < dtChild.Rows.Count; i++)
                {
                    string gradesOut = string.Empty;
                    string gradesOutUsed = string.Empty;
                    string grades = dtChild.Rows[i]["GRADES"].ToString().Trim();    ///获取等级列信息
                    string[] arrTemp = grades.Split(',');                           ///数组取值（等级组所有等级）
                    for (int j = 0; j < arrTemp.Length; j++)
                    {
                        gradesOut += GetProductGradeDisplayText(arrTemp[j]) + ",";  ///调用等级转换将转换数据重新组合，并用逗号隔开
                    }
                    gradesOutUsed = gradesOut.Remove(gradesOut.LastIndexOf(","), 1);///去掉最后的，号
                    dtChild.Rows[i]["GRADES"] = gradesOutUsed;                      ///列重新赋值
                }
                e.ChildList = dtChild.DefaultView;
            }
            else
            {
                e.ChildList = dsReturn.Tables[0].DefaultView;
            }

        }

        private void gvList_MasterRowEmpty(object sender, MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = false;
        }

        private void gvList_MasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "PartDetail";
        }

        private void gvList_MasterRowGetRelationDisplayCaption(object sender, MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "明细数据";
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            ControlState controlState = ControlState.New;
            ByProductPartForm form = new ByProductPartForm(controlState);
            if (DialogResult.OK == form.ShowDialog())
            {
                BindData();
            }
        }

        private void gvList_DoubleClick(object sender, EventArgs e)
        {
            if (gvList.FocusedRowHandle > -1)
            {
                ControlState controlState = ControlState.Edit;
                DataRow dr = gvList.GetFocusedDataRow();
                string partNum = dr["PART_ID"].ToString().Trim();
                ByProductPartForm form = new ByProductPartForm(controlState, partNum);
                if (DialogResult.OK == form.ShowDialog())
                {
                    BindData();
                }
            }
        }
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExportExcel_Click(object sender, EventArgs e)
        {

            DataView dv = this.gvList.DataSource as DataView;
            dv.RowFilter = this.gvList.RowFilter;
            if (dv.Count > 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel文件(*.xls)|*.xls";
                dlg.FilterIndex = 1;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    object[] objs = new object[2]
                    {
                        dv,
                        dlg.FileName
                    };
                    ParameterizedThreadStart s = new ParameterizedThreadStart(ExportExcel);
                    Thread t = new Thread(s);
                    t.Start(objs);
                }
            }

        }

        void ExportExcel(object obj)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.lblMsg.Visible = true;
                //this.lblMsg.Text = string.Format("正在执行导出EXCEL，请勿关闭界面，等待...");
                this.lblMsg.Text = string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.msg.0001}"));
                //this.tableLayoutPanelMain.Enabled = false;
                //原来的tablelayout控件变更为splitcontrol
                this.splitContainerControl1.Enabled = false;
            }));

            try
            {
                object[] objs = obj as object[];
                DataView dv = objs[0] as DataView;
                string fileName = objs[1] as string;

                DataTable dtExportExcel = new DataTable();
                dtExportExcel.TableName = "Sheet1";
                Dictionary<string, string> dicColumns = new Dictionary<string, string>()
                {
                    {"MODULE","组件类型"},
                    {"PARTNUMBER_01","主料号"},
                    {"PARTDESC_01","物料描述"},
                    {"REMARK_01","备注"},
                    {"PARTNUMBER_02","第一联副产品"},
                    {"PARTDESC_02","物料描述"},
                    {"REMARK_02","备注"},
                    {"PARTNUMBER_03","第二联副产品"},
                    {"PARTDESC_03","物料描述"},
                    {"REMARK_03","备注"},
                    {"PARTNUMBER_04","第三联副产品"},
                    {"PARTDESC_04","物料描述"},
                    {"REMARK_04","备注"},
                    {"CREATE_TIME","创建时间"}
                };

                foreach (string colName in dicColumns.Keys)
                {
                    DataColumn dc = dtExportExcel.Columns.Add(colName);
                    dc.Caption = dicColumns[colName];
                }

                foreach (DataRowView drv in dv)
                {
                    string partid = Convert.ToString(drv["PART_ID"]);
                    DataRow dr = dtExportExcel.NewRow();
                    dr["MODULE"] = drv["PART_MODULE"];
                    dr["PARTNUMBER_01"] = partid;
                    dr["PARTDESC_01"] = drv["PART_DESC"];

                    ByProductPartEntity byProductPartEntity = new ByProductPartEntity();
                    DataSet dsReturn = byProductPartEntity.GetByPartId(partid);
                    if (string.IsNullOrEmpty(byProductPartEntity.ErrorMsg)
                        && dsReturn.Tables.Count > 0)
                    {
                        DataTable dtChild = dsReturn.Tables[0];
                        //遍历查询结果
                        for (int i = 0; i < dtChild.Rows.Count; i++)
                        {
                            DataRow drChild = dtChild.Rows[i];
                            string partNumber = Convert.ToString(drChild["PART_NUMBER"]);
                            int itemNo = Convert.ToInt32(drChild["ITEM_NO"]);
                            string partNumberColName = string.Format("PARTNUMBER_{0}", itemNo.ToString("00"));
                            string partDescColName = string.Format("PARTDESC_{0}", itemNo.ToString("00"));
                            string remarkColName = string.Format("REMARK_{0}", itemNo.ToString("00"));

                            string remark = string.Empty;
                            string gradesOut = string.Empty;
                            string grades = drChild["GRADES"].ToString().Trim();    //获取等级列信息
                            string[] arrTemp = grades.Split(',');                   //数组取值（等级组所有等级）
                            for (int j = 0; j < arrTemp.Length; j++)
                            {
                                gradesOut += GetProductGradeDisplayText(arrTemp[j]) + ",";  //调用等级转换将转换数据重新组合，并用逗号隔开
                            }
                            gradesOut = gradesOut.TrimEnd(',');
                            if (string.IsNullOrEmpty(gradesOut))
                            {
                                gradesOut = "无要求";
                            }

                            string minPower = Convert.ToString(drChild["MIN_POWER"]);
                            string maxPower = Convert.ToString(drChild["MAX_POWER"]);
                            string power = string.Empty;
                            if (!string.IsNullOrEmpty(minPower))
                            {
                                power += string.Format("≥{0}", minPower);
                            }
                            if (!string.IsNullOrEmpty(maxPower))
                            {
                                power += string.Format("＜{0}", maxPower);
                            }
                            if (string.IsNullOrEmpty(power))
                            {
                                power = "无要求";
                            }

                            string storeLocation = Convert.ToString(drChild["STORAGE_LOCATION"]);
                            //remark = string.Format("等级要求：{0};功率要求：{1};建议库位：{2};",
                            remark = string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.msg.0003}"),
                                                 gradesOut,
                                                 power,
                                                 storeLocation);

                            dr[partNumberColName] = partNumber;
                            dr[partDescColName] = drChild["PART_DESC"];
                            dr[remarkColName] = remark;
                            if (partNumber == partid)
                            {
                                dr["CREATE_TIME"] = drChild["CREATE_TIME"];
                            }
                        }
                    }
                    dtExportExcel.Rows.Add(dr);
                }
                Export.ExportToExcel(fileName, dtExportExcel);
            }
            catch (Exception ex)
            {
                //MessageService.ShowError(string.Format("导出EXCEL失败:{0}", ex.Message));
                MessageService.ShowError(string.Format(StringParser.Parse("${res:FanHai.Hemera.Addins.ByProductPartCtrl.msg.0002}"), ex.Message));
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.lblMsg.Visible = false;
                    this.lblMsg.Text = string.Empty;
                    //this.tableLayoutPanelMain.Enabled = true;
                    //原来的tablelayout控件变更为splitcontrol
                    this.splitContainerControl1.Enabled = true;
                }));

            }
        }

        private void gvList_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gdView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

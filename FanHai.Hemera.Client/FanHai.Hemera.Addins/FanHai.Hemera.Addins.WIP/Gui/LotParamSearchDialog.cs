using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using DevExpress.Utils;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 显示批次数据采集信息的对话框。
    /// </summary>
    /// comment by peter 2012-2-17
    public partial class LotParamSearchDialog : BaseDialog
    {
        private string _lotNumber = string.Empty;                   //批次号码     
        private EdcManage edcManage = new EdcManage();              //数据采集管理类，包含和数据采集相关的操作
        private DataSet dsParam = new DataSet();                   //包含批次数据采集信息的对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// comment by peter 2012-2-17
        public LotParamSearchDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dsCollection">包含批次数据采集信息的对象。</param>
        /// <param name="lotNumber">批号号。</param>
        /// comment by peter 2012-2-17
        public LotParamSearchDialog(DataSet dsCollection, string lotNumber)
            : base("批次工艺参数")
        {
            InitializeComponent();
            this.lbLotNumber.Text = lotNumber;
            _lotNumber = lotNumber;
            dsParam = dsCollection;
            //计算最大列数。
            int column = Convert.ToInt32(dsParam.Tables[0].Compute("max(SP_UNIT_SEQ)", ""));
            DataTable table = dsParam.Tables[0].DefaultView.ToTable(true, "STEP_KEY", "STEP_NAME", "PARAM_NAME");
            CreateGridView(column, table);
        }
        /// <summary>
        /// 生成网格控件
        /// </summary>
        /// <param name="column">网格控件的列数。</param>
        /// <param name="paramTable"></param>
        /// comment by peter 2012-2-17
        private void CreateGridView(int column, DataTable paramTable)
        {
            try
            {

                DataTable table = new DataTable();
                #region Column of GridView  根据列数和参数表生成GridView
                int index = 0;
                GridColumn gcStepName = new GridColumn();
                gcStepName.Caption = "工序名称";
                gcStepName.Name = "step_name";
                gcStepName.FieldName = EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME;
                gcStepName.Visible = true;
                gcStepName.VisibleIndex = index;
                gcStepName.OptionsColumn.AllowMerge = DefaultBoolean.True;
                //gridColumn.ReadOnly = true;
                this.gvParamInfo.Columns.Add(gcStepName); //添加一个网格列。
                table.Columns.Add(EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME);

                GridColumn gridColumn = new GridColumn();
                gridColumn.Caption = "参数名";
                gridColumn.Name = "param_name";
                gridColumn.FieldName = BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME;
                gridColumn.Visible = true;
                gridColumn.VisibleIndex = ++index;
                gridColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;
                //gridColumn.ReadOnly = true;
                this.gvParamInfo.Columns.Add(gridColumn); //添加一个网格列。
                table.Columns.Add(BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME);
                for (int i = 0; i < column; i++)//循环列数
                {
                    GridColumn newColumn = new GridColumn();
                    newColumn.Caption = "数据" + (i + 1).ToString();
                    newColumn.Name = "paramValue" + (i + 1).ToString();
                    newColumn.FieldName = "paramValue" + (i + 1).ToString();
                    newColumn.Visible = true;
                    newColumn.VisibleIndex = ++index;
                    //newColumn.ReadOnly = true;
                    newColumn.OptionsColumn.AllowMerge = DefaultBoolean.False;
                    this.gvParamInfo.Columns.Add(newColumn);//添加一个新列
                    table.Columns.Add("paramValue" + (i + 1).ToString());
                }
                //显示网格框架。
                gcParamInfo.MainView = gvParamInfo;
                gcParamInfo.DataSource = table;
                gvParamInfo.BestFitColumns();
                for (int j = 0; j < paramTable.Rows.Count; j++)
                {
                    gvParamInfo.AddNewRow();
                    DataRow newRow = gvParamInfo.GetDataRow(gvParamInfo.FocusedRowHandle);
                    newRow[EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME] = paramTable.Rows[j][EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME].ToString();
                    newRow[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME] = paramTable.Rows[j][BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString();
                    newRow.EndEdit();
                    gvParamInfo.UpdateCurrentRow();
                    gvParamInfo.ShowEditor();
                }

                #endregion

                #region BindDataToGrid
                BindDataToGrid();
                #endregion
            }
            catch (Exception ex)
            {
                MessageService.ShowWarning(ex.Message);
            }
        }
        /// <summary>
        /// 绑定数据到Grid控件中。
        /// </summary>
        /// comment by peter 2012-2-17
        private void BindDataToGrid()
        {
            try
            {
                DataTable collectionTable = dsParam.Tables[0];
                //遍历批次采集数据的行数
                foreach (DataRow row in collectionTable.Rows)
                {
                    //遍历网格控件的行数。
                    for (int i = 0; i < gvParamInfo.RowCount; i++)
                    {
                        //如果参数名相同
                        if (row[BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME].ToString()
                            == gvParamInfo.GetRowCellValue(i, BASE_PARAMETER_FIELDS.FIELD_PARAM_NAME).ToString()
                            && row[EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME].ToString()
                            == gvParamInfo.GetRowCellValue(i, EDC_MAIN_INS_FIELDS.FIELD_STEP_NAME).ToString())
                        {
                            gvParamInfo.SetRowCellValue(i,
                                "paramValue" + row[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString(),
                                row[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 退出 按钮的Click事件方法，用于关闭对话框。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// comment by peter 2012-2-17
        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();//关闭对话框。
        }

        /// <summary>
        /// 自定义绘制单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// comment by jing.xie 2012-6-14
        private void gvParamInfo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            DataTable dt = dsParam.Tables[0];

            //遍历批次采集数据的行数
            foreach (DataRow row in dt.Rows)
            {
                //无效值
                if (row["VALID_FLAG"].ToString() == "1")
                {
                    if (e.Column.FieldName == "paramValue" + row["SP_UNIT_SEQ"].ToString())
                    {
                        string value = Convert.ToString(e.CellValue);

                        if (row["PARAM_VALUE"].ToString() == value && 
                            row["STEP_NAME"].ToString() == gvParamInfo.GetRowCellValue(e.RowHandle,"STEP_NAME").ToString()&&
                            row["PARAM_NAME"].ToString() == gvParamInfo.GetRowCellValue(e.RowHandle, "PARAM_NAME").ToString())
                        {
                            e.Appearance.BackColor = System.Drawing.Color.Red;
                            break;
                        }
                    }
                }
            }

        }
    }
}

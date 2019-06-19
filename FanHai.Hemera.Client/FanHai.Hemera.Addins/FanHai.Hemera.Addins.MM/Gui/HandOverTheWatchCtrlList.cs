using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

using FanHai.Hemera.Utils.Controls;

using DevExpress.XtraEditors.Controls;
using FanHai.Gui.Framework.Gui;
using System.Collections;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraGrid.Views.Grid;

namespace FanHai.Hemera.Addins.MM
{
    public partial class HandOverTheWatchCtrlList : BaseUserCtrl
    {
        string key = string.Empty;
        string dangQianBanbie = string.Empty;
        public static int flagList = -1;


        public HandOverTheWatchCtrlList()
        {
            InitializeComponent();
            gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["OUT_QTY_2"].ColumnEdit = repositoryItemTextEdit1;
        }
        /// <summary>
        /// 关闭当前视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 绑定数据表
        /// </summary>
        /// <param name="key"></param>
        public void BindShuJuBiao(string key)
        {
            gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = true;
            OperationHandover _operationHandover = new OperationHandover();
            gvWuLiaoJiaoJieList.MainView = gridView1;
            DataSet dtGetWipAndMat = _operationHandover.GetWipAndMatByKey(key);//绑定数据表通过工序交接班主键获取物料信息和WIP信息
            gvWuLiaoJiaoJieList.DataSource = dtGetWipAndMat.Tables["MatByKey"];

            gvWipJiaoJieList.MainView = gridView2;
            gvWipJiaoJieList.DataSource = dtGetWipAndMat.Tables["WipByKey"];
        }

        /// <summary>
        /// HandOverTheWatchCtrl双击行事件绑定窗体控件值
        /// </summary>
        /// <param name="handOverTheWatchCtrl"></param>
        public void BindContral(HandOverTheWatchCtrl handOverTheWatchCtrl)
        {
            key = handOverTheWatchCtrl.OperationHandoverKey;

            dangQianBanbie = handOverTheWatchCtrl.DangQianBanBie;

            this.txtFacRoom.Text = handOverTheWatchCtrl.FactoryName;
            this.txtGongXu.Text = handOverTheWatchCtrl.GongXu;
            this.txtJiaoBanShift.Text = handOverTheWatchCtrl.JiaoBanShifeName;
            this.txtJieBanShift.Text = handOverTheWatchCtrl.JieBanShifeName;
            this.txtJiaoBaoTime.Text = Convert.ToDateTime(handOverTheWatchCtrl.JiaoJieDate).ToString("yyyy-MM-dd");
            this.txtZhuangTai.Text = handOverTheWatchCtrl.ZhuangTai;
            this.txtJiaoBanJobNumber.Text = handOverTheWatchCtrl.JiaoBanPeople;
            this.txtJieBanJobNumber.Text = handOverTheWatchCtrl.JieBanPeople;

            BindShuJuBiao(key);   //绑定数据表通过工序交接班主键获取物料信息和WIP信息

            //获取当前班次的day
            //获取载入班次的day
            //获取载入班次的下一班的day
            
            //已接班为接班操作，保存和重新生成不可用
            if (handOverTheWatchCtrl.ZhuangTai == "已接班")
            {
                tsbSave.Enabled = false;
                tsbShengCheng.Enabled = false;
                gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = false;
            }
            //未交班，取出当前班次和交班班次的DAY是否相同,不相同的话不能交班,保存和重新生成都需要不可用
            if (handOverTheWatchCtrl.ZhuangTai == "未交班" && dangQianBanbie != this.txtJiaoBanShift.Text)
            {
                tsbSave.Enabled = false;
                tsbShengCheng.Enabled = false;
                gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = false;
            }
            //未交班，取出当前班次和交班班次的DAY是否相同,相同的话可交班,呈现接班操作,确定和重新生成都可用
            if (handOverTheWatchCtrl.ZhuangTai == "未交班" && dangQianBanbie == this.txtJiaoBanShift.Text)
            {
                flagList = 1;
                this.labelControl1.Text = "工序交接班清单--交班";
            }
            //已交班，当前班次和交班班次相同,为交班界面。如果当前班次和接班班次相同，为接班班次
            if (handOverTheWatchCtrl.ZhuangTai == "已交班" && dangQianBanbie == this.txtJiaoBanShift.Text)
            {
                flagList = 1;
                this.labelControl1.Text = "工序交接班清单--交班";
            }
            if (handOverTheWatchCtrl.ZhuangTai == "已交班" && dangQianBanbie == this.txtJieBanShift.Text)
            {
                flagList = 2;   
                this.labelControl1.Text = "工序交接班清单--接班";
                gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = false;
            }

        }
        /// <summary>
        ///  HandOverTheWatchCtrl上交接按钮Click事件绑定窗体控件值
        /// </summary>
        /// <param name="handOverTheWatchCtrl"></param>
        public void BindContralByJiaoJieClick(HandOverTheWatchCtrl handOverTheWatchCtrl)
        {
            DataSet dsGetDangQianShiftHandover = new DataSet();
            OperationHandover _operationHandover = new OperationHandover();
            dsGetDangQianShiftHandover = _operationHandover.GetDangQianShiftHandover(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);   //获取当前班次的交班日期
            if (dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows.Count > 0)
            {
                string handOverKey = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][0].ToString();
                _operationHandover.UpdateHandOverMatAndWip(handOverKey);//通过工序交接班的主键然后获取WIP和物料的数量更新到表中
                _operationHandover.UpdateHandOverMatAndWipQiMoShuLiang(handOverKey);//通过工序交接班的主键更新期末数据
                this.txtFacRoom.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][1].ToString();
                this.txtGongXu.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][2].ToString();
                this.txtJiaoBanShift.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][3].ToString();
                this.txtJieBanShift.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][4].ToString();
                this.txtJiaoBaoTime.Text = Convert.ToDateTime(dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][5].ToString()).ToString("yyyy-MM-dd");
                this.txtZhuangTai.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][6].ToString();
                this.txtJiaoBanJobNumber.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][7].ToString();
                this.txtJieBanJobNumber.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][8].ToString();
                BindShuJuBiao(handOverKey);   //绑定数据表通过工序交接班主键获取物料信息和WIP信息
            }
            else
            {
                //没有查询出来的交接班记录 为交接班表插入一条记录  定义hash准备插入操作
                Hashtable hashTable = new Hashtable();
                hashTable.Add("LOCATIOMKEY", handOverTheWatchCtrl.FactRoom);
                hashTable.Add("OPERATIONNAME", handOverTheWatchCtrl.Operation);
                hashTable.Add("SENDSHIFTVALUE", handOverTheWatchCtrl.Shift);
                hashTable.Add("DAY", dsGetDangQianShiftHandover.Tables["DAY"].Rows[0][0].ToString());
                hashTable.Add("STATUS", "0");
                hashTable.Add("SENDOPERATOR", handOverTheWatchCtrl.Sendoperator);
                hashTable.Add("CREATE_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableParam.TableName = "HASH";

                DataSet dsSetIn = new DataSet();
                dsSetIn.Merge(tableParam);
                _operationHandover.InsertHandOver(dsSetIn);    //插入数据到工序交接班表
                //获取插入后的工序交接班的详细信息
                dsGetDangQianShiftHandover = _operationHandover.GetDangQianShiftHandover(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);   //先获取当前班次的交班日期然后获取工序交接班的信息记录
                //获取插入记录的主键
                string handOverKey = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][0].ToString();
                //通过根据工序和工厂车间获取所有线上仓中的物料信息（WST_STORE,WST_STORE_MATERIAL)插入到WST_OPERATION_HANDOVER_MAT中
                //（数量全部设置为0）。根据工序和工厂车间获取所有工单的在制品信息（POR_LOT,POR_WORK_ORDER,WIP_TRANSACTION)插入到
                //WST_OPERATION_HANDOVER_WIP中（数量全部设置为0）
                _operationHandover.InsertHandOverMatAndWip(handOverKey, handOverTheWatchCtrl.FactRoom, handOverTheWatchCtrl.Operation);
                _operationHandover.UpdateHandOverMatAndWip(handOverKey);//通过工序交接班的主键然后获取WIP和物料的数量更新到表中
                _operationHandover.UpdateHandOverMatAndWipQiMoShuLiang(handOverKey);//通过工序交接班的主键更新期末数据

                this.txtFacRoom.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][1].ToString();
                this.txtGongXu.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][2].ToString();
                this.txtJiaoBanShift.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][3].ToString();
                this.txtJieBanShift.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][4].ToString();
                this.txtJiaoBaoTime.Text = Convert.ToDateTime(dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][5].ToString()).ToString("yyyy-MM-dd");
                this.txtZhuangTai.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][6].ToString();
                this.txtJiaoBanJobNumber.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][7].ToString();
                this.txtJieBanJobNumber.Text = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][8].ToString();

                BindShuJuBiao(handOverKey);
            }

        }
        /// <summary>
        ///  HandOverTheWatchCtrl上接班按钮Click事件绑定窗体控件值
        /// </summary>
        /// <param name="handOverTheWatchCtrl"></param>
        public void BindContralByJieBanClick(HandOverTheWatchCtrl handOverTheWatchCtrl)
        {
            DataSet dsGetShangYiBanHandOver = new DataSet();
            OperationHandover _operationHandover = new OperationHandover();
            //获取上一班的交接记录
            dsGetShangYiBanHandOver = _operationHandover.GetShangYiBanHandOver(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);
            string handOverKey = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][0].ToString();
            this.txtFacRoom.Text = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][1].ToString();
            this.txtGongXu.Text = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][2].ToString();
            this.txtJiaoBanShift.Text = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][3].ToString();
            this.txtJieBanShift.Text = handOverTheWatchCtrl.Shift;//dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][4].ToString();
            this.txtJiaoBaoTime.Text = Convert.ToDateTime(dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][5].ToString()).ToString("yyyy-MM-dd");
            this.txtZhuangTai.Text = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][6].ToString();
            this.txtJiaoBanJobNumber.Text = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][7].ToString();
            this.txtJieBanJobNumber.Text = handOverTheWatchCtrl.Receiveoperator;//dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][8].ToString();
            BindShuJuBiao(handOverKey);   //绑定数据表通过工序交接班主键获取物料信息和WIP信息
            gridView1.Columns["OUT_QTY_2"].OptionsColumn.AllowEdit = false;

        }

        /// <summary>
        /// 视图载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandOverTheWatchCtrlList_Load(object sender, EventArgs e)
        {
            HandOverTheWatchCtrl handOverTheWatchCtrl = new HandOverTheWatchCtrl();
            flagList = handOverTheWatchCtrl.Flag2;
            if (flagList == 0)
            {//双击工序交接班界面表行进入
                //绑定窗体控件值
                BindContral(handOverTheWatchCtrl);
            }
            else if (flagList == 1)
            {//单击交班进入
                this.labelControl1.Text = "工序交接班清单--交班";
                BindContralByJiaoJieClick(handOverTheWatchCtrl);
            }
            else if (flagList == 2)
            {//单击接班进入
                this.tsbShengCheng.Enabled = false;
                this.labelControl1.Text = "工序交接班清单--接班";
                BindContralByJieBanClick(handOverTheWatchCtrl);
            }
        }

        /// <summary>
        /// 序号子增长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUM": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 序号子增长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "ROWNUMBER": //设置行号
                    e.DisplayText = Convert.ToString(e.RowHandle + 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///单元格修改触发事件                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //if (e.Column.FieldName == "OUT_QTY_2")
            //{
            //    double used = Convert.ToDouble(e.CellValue);                                             //耗用数量
            //    double lailiao = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "IN_QTY_1"));   //来料数量
            //    double tuiliao = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "OUT_QTY_1")); //退料数量
            //    double first = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "INIT_QTY"));     //期初数量
            //    gridView1.SetRowCellValue(e.RowHandle, "END_QTY", lailiao + first - used - tuiliao);     //计算期末数量在单元格内数据发生变更时
            //}
        }

        /// <summary>
        /// 交班时更新wip和mat数据和交接班记录表的数据
        /// </summary>
        public void UpdateWipMatHandOverBySaveJiaoban(HandOverTheWatchCtrl handOverTheWatchCtrl)
        {
            DataSet dsGetDangQianShiftHandover = new DataSet();
            OperationHandover _operationHandover = new OperationHandover();
            dsGetDangQianShiftHandover = _operationHandover.GetDangQianShiftHandover(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);   //获取当前班次的交班日期
            
            DataTable dtHandOverTable = new DataTable();
            DataView dv = gridView1.DataSource as DataView;
            if (dv != null) dtHandOverTable = dv.Table;

            string handOverKey = dsGetDangQianShiftHandover.Tables["ShiftHandover"].Rows[0][0].ToString();
            Hashtable hashTable = new Hashtable();
            hashTable.Add("OPERATION_HANDOVER_KEY", handOverKey);
            hashTable.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            hashTable.Add("EDIT_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
            DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);//02322 136669
            tableParam.TableName = "HASH";

            DataSet dsSetIn = new DataSet();
            dtHandOverTable.TableName = "WST_OPERATION_HANDOVER_MAT";
            dsSetIn.Merge(dtHandOverTable);
            dsSetIn.Merge(tableParam);

            if(dtHandOverTable  == null)
            {
                MessageService.ShowMessage("没有可保存的物料信息!","${res:Global.SystemInfo}");
            }

            if (MessageBox.Show(StringParser.Parse("确定要交班吗？"),
                    StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_operationHandover.UpdateWipMatHandOverBySaveJiaoban(dsSetIn) != true)//保存数据到MAT和工序交接班表中
                {
                    MessageService.ShowMessage("交接信息保存失败!", "${res:Global.SystemInfo}");
                }
                else
                {
                    MessageService.ShowMessage("交接信息保存成功!", "${res:Global.SystemInfo}");
                }
            }
        }
        /// <summary>
        /// 接班时更新状态并且插入一条接班记录期同时插入mat和wip的记录初数量为接班时数量的期末数量
        /// </summary>
        public void UpdateAndInsertWipMatHandOverBySaveJieBan(HandOverTheWatchCtrl handOverTheWatchCtrl)
        {
            if (MessageBox.Show(StringParser.Parse("确定要接班吗？"),
                  StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {

                DataSet dsGetShangYiBanHandOver = new DataSet();
                OperationHandover _operationHandover = new OperationHandover();
                //获取上一班的交接记录
                dsGetShangYiBanHandOver = _operationHandover.GetShangYiBanHandOver(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);
                string handOverKey = dsGetShangYiBanHandOver.Tables["ShiftHandover"].Rows[0][0].ToString();

                Hashtable hashTable1 = new Hashtable();
                hashTable1.Add("OPERATION_HANDOVER_KEY", handOverKey);
                hashTable1.Add("Receiveoperator", handOverTheWatchCtrl.Receiveoperator);
                hashTable1.Add("SHIFT", handOverTheWatchCtrl.Shift);
                hashTable1.Add("EDITOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                hashTable1.Add("EDIT_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                DataTable tableParam1 = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable1);
                tableParam1.TableName = "HASH1";

                DataSet dsSetIn1 = new DataSet();
                dsSetIn1.Merge(tableParam1);
                _operationHandover.UpdateHandOver(dsSetIn1);    //插入数据到工序交接班表

                DataSet dsGetDangQianHandOver = _operationHandover.GetDangQianShiftHandover(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);

                Hashtable hashTable = new Hashtable();
                hashTable.Add("LOCATIOMKEY", handOverTheWatchCtrl.FactRoom);
                hashTable.Add("OPERATIONNAME", handOverTheWatchCtrl.Operation);
                hashTable.Add("SENDSHIFTVALUE", handOverTheWatchCtrl.Shift);
                hashTable.Add("DAY", dsGetDangQianHandOver.Tables["DAY"].Rows[0][0].ToString().Trim());
                hashTable.Add("STATUS", "0");
                hashTable.Add("SENDOPERATOR", handOverTheWatchCtrl.Sendoperator);
                hashTable.Add("CREATE_TIMEZONE", PropertyService.Get(PROPERTY_FIELDS.TIMEZONE));
                hashTable.Add("CREATOR", PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
                DataTable tableParam = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(hashTable);
                tableParam.TableName = "HASH";

                DataSet dsSetIn = new DataSet();
                dsSetIn.Merge(tableParam);
                try
                {
                    _operationHandover.InsertHandOver(dsSetIn);    //插入数据到工序交接班表

                    DataSet dsGetDangQianHandOver1 = _operationHandover.GetDangQianShiftHandover(handOverTheWatchCtrl.Shift, handOverTheWatchCtrl.Operation, handOverTheWatchCtrl.FactRoom);
                    string handDangqianOverKey = dsGetDangQianHandOver1.Tables["ShiftHandover"].Rows[0][0].ToString();
                    _operationHandover.InsertMatWipQiChu(handDangqianOverKey, handOverKey);//根据上一工序交接班主键获取上一工序交接班的期末数量插入到新生成的数据中的期初数量
                    MessageService.ShowMessage("保存成功","系统提示");
                }
                catch (Exception ex)
                {
                    MessageService.ShowMessage(ex.Message, "系统提示");
                }


            }

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (this.gridView1.State == GridState.Editing && this.gridView1.IsEditorFocused
                           && this.gridView1.EditingValueModified)
            {
                this.gridView1.SetFocusedRowCellValue(this.gridView1.FocusedColumn, this.gridView1.EditingValue);
            }
            this.gridView1.UpdateCurrentRow();

            HandOverTheWatchCtrl handOverTheWatchCtrl = new HandOverTheWatchCtrl();
            //flagList = handOverTheWatchCtrl.Flag2;
            if (flagList == 0)
            {//双击工序交接班界面表行进入
                //绑定窗体控件值
            }
            else if (flagList == 1)
            {//单击交班进入
                UpdateWipMatHandOverBySaveJiaoban(handOverTheWatchCtrl);
            }
            else if (flagList == 2)
            {//单击接班进入
                UpdateAndInsertWipMatHandOverBySaveJieBan(handOverTheWatchCtrl);
            }
        }

        private void tsbShengCheng_Click(object sender, EventArgs e)
        {
            HandOverTheWatchCtrlList_Load(sender, e);
        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {      
            if (e.Column.FieldName == "OUT_QTY_2")
            {
                double used = Convert.ToDouble(e.Value);                                             //耗用数量
                double lailiao = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "IN_QTY_1"));   //来料数量
                double tuiliao = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "OUT_QTY_1")); //退料数量
                double first = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "INIT_QTY"));     //期初数量
                gridView1.SetRowCellValue(e.RowHandle, "END_QTY", lailiao + first - used - tuiliao);     //计算期末数量在单元格内数据发生变更时
                
            } 

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 撤销批次的窗体类。
    /// </summary>
    public partial class LotUndoCtrl : BaseUserCtrl
    {
        LotOperationEntity _entity = new LotOperationEntity();
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        /// <summary>
        /// 允许撤销的操作列表。
        /// </summary>
        private Dictionary<string, string> AllowUndoActivityList = new Dictionary<string,string>()
        {
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT,"创建操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN,"进站操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT,"出站操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_DEFECT,"组件不良操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_SCRAP,"组件报废操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_ADJUST,"调整批次操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_HOLD,"暂停批次操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RELEASE,"释放批次操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_REWORK,"返修操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO,"转工单作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_PROID,"返工单作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH,"电池片补片作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCHED,"电池片被补片作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP,"电池片报废作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLDEFECT,"电池片不良作业"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TERMINALLOT,"终止批次操作"},
            {ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_RETURN_MATERIAL,"退料操作"}
        };
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotUndoCtrl()
        {
            InitializeComponent();
            InitializeLanguage();
        }


        private void InitializeLanguage()
        {
            this.gcolRowNum.Caption = StringParser.Parse("${res:Global.RowNumber}");//"序号";
            this.gcolLotNumber.Caption = StringParser.Parse("${res:Global.LotNumber}");//"批次号";
            this.gcolStepName.Caption = StringParser.Parse("${res:Global.Step}");//"工序";
            this.gcolActivity.Caption = StringParser.Parse("${res:Global.Operation}");//"操作";
            this.gcolEditor.Caption = StringParser.Parse("${res:Global.Operator}");//"操作者";
            this.gcolEditTime.Caption = StringParser.Parse("${res:Global.Operation.Time}");//"操作时间";
            this.btnSave.Text = StringParser.Parse("${res:Global.OKCancer}");//"确定撤销";
            
            this.btnRemove.Text = StringParser.Parse("${res:Global.Remove}");//移除
            this.btnAdd.Text = StringParser.Parse("${res:Global.New}");//"新增";
            this.lcgResult.Text = StringParser.Parse("${res:Global.List}");//"列表";
            this.lciRemark.Text = StringParser.Parse("${res:Global.Remark}");//"备注";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:Global.FactoryRoom}");//"车间名称";

            this.lciLotNumber.Text = StringParser.Parse("${res:Global.LotNumber}");//"批次号";
            this.lciAdd.Text = StringParser.Parse("${res:Global.New}");//"新增";
            this.lciRemove.Text = StringParser.Parse("${res:Global.Remove}");//"移除";
            this.lciFactoryRoom.Text = StringParser.Parse("${res:Global.FactoryRoom}");//"车间名称";
        }



        /// <summary>
        /// 窗体载入事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotUndoCtrl_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            ResetControlValue();
            this.lblMenu.Text = "生产管理>电池片管理>回退作业";
            GridViewHelper.SetGridView(gvUndo);
        }



        /// <summary>
        /// 重置控件值。
        /// </summary>
        private void ResetControlValue()
        {
            DataTable dtList = this.gcUndo.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Clear();
                dtList.AcceptChanges();
            }
            this.teLotNumber.Text = string.Empty;
            this.teLotNumber.Select();
        }   
        /// <summary>
        /// 绑定车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                if (dt.Rows.Count > 0)
                {
                    this.lueFactoryRoom.EditValue = dt.Rows[0]["LOCATION_KEY"].ToString();
                }
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            //禁用领料车间
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 自定义绘制单元格显示值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoGridView_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gcolRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gcolActivity)
            {
                string activity=Convert.ToString(e.CellValue);
                string description = GetActivityText(activity);
                e.DisplayText = description + string.Format("({0})", activity);
            }
        }
        /// <summary>
        /// 获取操作的描述文本。
        /// </summary>
        /// <param name="activity">操作。</param>
        /// <returns>操作的描述。</returns>
        private string GetActivityText(string activity)
        {
            if (AllowUndoActivityList.ContainsKey(activity))
            {
                return AllowUndoActivityList[activity];
            }
            return string.Empty;
        }
        /// <summary>
        /// 批次号回车事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void teLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAdd_Click(sender, e);
            }
        }
        /// <summary>
        /// 新增批次操作记录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string lotNumber = this.teLotNumber.Text;
                string roomKey = Convert.ToString(this.lueFactoryRoom.EditValue);
                //车间没有选择，给出提示。
                if (string.IsNullOrEmpty(roomKey))
                {

                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.Msg004}"), MESSAGEBOX_CAPTION);//车间名称不能为空
                    //MessageService.ShowMessage("车间名称不能为空", "提示");
                    this.lueFactoryRoom.Select();
                    return;
                }
                //批号没有输入，给出提示。
                if (string.IsNullOrEmpty(lotNumber))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg001}"), MESSAGEBOX_CAPTION);//请输入序列号
                    //MessageService.ShowMessage("请输入序列号。", "提示");
                    return;
                }
                DataSet dsReturn = this._entity.GetLotLastestActivity(lotNumber);
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                    return;
                }
                if (dsReturn == null || dsReturn.Tables.Count <= 0 || dsReturn.Tables[0].Rows.Count<=0)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg002}"), MESSAGEBOX_CAPTION);//输入的序列号没有操作可被撤销
                    //MessageService.ShowMessage("输入的序列号没有操作可被撤销。", "提示");
                    return;
                }
                DataTable dtReturn = dsReturn.Tables[0];
                DataRow drReturn = dtReturn.Rows[0];
                //判断批次号在指定车间中是否存在。
                string curRoomKey = Convert.ToString(drReturn[POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY]);
                if (roomKey != curRoomKey)
                {
                    MessageService.ShowMessage(string.Format("【{0}】在当前车间中不存在，请确认。", lotNumber), "提示");
                    return;
                }
                string curOperationName = Convert.ToString(drReturn[WIP_TRANSACTION_FIELDS.FIELD_STEP_NAME]);
                string createOperationName = Convert.ToString(drReturn[POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME]);
                string palletNo = Convert.ToString(drReturn[POR_LOT_FIELDS.FIELD_PALLET_NO]);
                if (!string.IsNullOrEmpty(palletNo)
                    && MessageService.AskQuestion(string.Format("组件已包装，若撤销会影响托盘({0})数据及其中的所有组件数据，是否确认？",palletNo), "询问")==false)
                {
                    return;
                }
                string activity = Convert.ToString(drReturn[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY]);
                //操作是否允许撤销。
                //if (!AllowUndoActivityList.ContainsKey(activity))
                //{
                //    MessageService.ShowMessage(string.Format("对不起，操作({0})不能被撤销。",activity), "提示");
                //    return;
                //}
                string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);//获取有权限的所有工序
                //除批次进站 批次出站或创建批次外，不卡控工序权限。
                if (activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN
                    || activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT
                    || activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT
                    || activity==ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP
                    || activity==ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCHED
                    || activity== ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH)
                {
                    //非创建批次操作，检查登录用户对批次操作时的工序是否拥有权限。 
                    if ((activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN
                    || activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKOUT) 
                    && (operations + ",").IndexOf(curOperationName + ",") == -1)
                    {
                        MessageService.ShowMessage(string.Format("您没有权限撤销工序[{0}]的[{1}]操作。", curOperationName, activity), "提示");
                        return;
                    }
                    //创建批次操作，判断用户对创建批次工序是否拥有权限。
                    if ((activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CREATELOT
                        || activity==ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CELLSCRAP
                        || activity==ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCHED
                        || activity== ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_PATCH)
                        &&(operations + ",").IndexOf(createOperationName + ",") == -1)
                    {
                        MessageService.ShowMessage(string.Format("您没有权限撤销工序[{0}]的[{1}]操作。", createOperationName,activity), "提示");
                        return;
                    }

                }
                //只有包装人员才允许撤销 返工单作业
                if ((activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_PROID)
                    && (operations + ",").IndexOf("包装,") == -1)
                {

                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg003}"), MESSAGEBOX_CAPTION);//只有包装人员才允许撤销返工单作业
                    //MessageService.ShowMessage(string.Format("只有包装人员才允许撤销返工单作业。"), "提示");
                    return;
                }
                //只有入库人员才允许撤销 入库作业
                if ((activity == ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TO_WAREHOUSE)
                   && (operations + ",").IndexOf("入库,") == -1)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg004}"), MESSAGEBOX_CAPTION);//只有入库人员才允许撤销入库作业
                    //MessageService.ShowMessage(string.Format("只有入库人员才允许撤销入库作业。"), "提示");
                    return;
                }

                DataTable dtList = this.gcUndo.DataSource as DataTable;
                if (dtList == null)
                {
                    this.gcUndo.DataSource = dtReturn;
                }
                else
                {
                    if (dtList.Rows.Count >= 100)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg005}"), MESSAGEBOX_CAPTION);//一次撤销记录数不能超过100条
                        //MessageService.ShowMessage("一次撤销记录数不能超过100条。", "提示");
                        return;
                    }
                    //判断记录是否在列表中存在。
                    string transactionKey = Convert.ToString(drReturn[WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY]);
                    DataRow[] drs = dtList.Select(string.Format("{0}='{1}'",WIP_TRANSACTION_FIELDS.FIELD_TRANSACTION_KEY,transactionKey));
                    if (drs.Length > 0)
                    {
                        MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg006}"), MESSAGEBOX_CAPTION);//记录已存在，请确认
                        //MessageService.ShowMessage("记录已存在，请确认。", "提示");
                        this.gvUndo.FocusedRowHandle = dtList.Rows.IndexOf(drs[0]);
                        return;
                    }
                    dtList.Merge(dtReturn);
                }
            }
            finally
            {
                this.teLotNumber.Select();
                this.teLotNumber.SelectAll();
            }
        }
        /// <summary>
        /// 移除批次操作记录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int rowHandler = this.gvUndo.FocusedRowHandle;
            if (rowHandler < 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg007}"), MESSAGEBOX_CAPTION);//请选择要删除的记录
                //MessageService.ShowMessage("请选择要删除的记录。", "提示");
                return;
            }
            DataRow dr = this.gvUndo.GetDataRow(rowHandler);
            DataTable dtList = this.gcUndo.DataSource as DataTable;
            if (dtList != null)
            {
                dtList.Rows.Remove(dr);
            }
        }
        /// <summary>
        /// 确定撤销操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtList = this.gcUndo.DataSource as DataTable;
            if (dtList == null || dtList.Rows.Count <= 0)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg008}"), MESSAGEBOX_CAPTION);//列表中没有记录，撤销完成
                //MessageService.ShowMessage("列表中没有记录，撤销完成。", "提示");
                return;
            }

            //if (MessageService.AskQuestion("确定撤销？", "撤销"))
            if (MessageService.AskQuestion(StringParser.Parse("${res:Global.OKCancer}")+"?", StringParser.Parse("${res:Global.Undo}")))
            {
                string remark = this.teRemark.Text.Trim();
                if (string.IsNullOrEmpty(remark))
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotUndoCtrl.Msg009}"), MESSAGEBOX_CAPTION);//必须输入备注信息
                    //MessageService.ShowMessage("必须输入备注信息。", "提示");
                    this.teRemark.Select();
                    return;
                }

                //组织批次撤销数据
                DataSet dsParams = new DataSet();
                DataTable dtUndo = dtList.Copy();
                dtUndo.TableName = WIP_TRANSACTION_FIELDS.DATABASE_TABLE_NAME;

                string timeZone = PropertyService.Get(PROPERTY_FIELDS.TIMEZONE);
                string editor = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
                foreach (DataRow dr in dtUndo.Rows)
                {
                    dr[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME] = dr["LOT_EDIT_TIME"];  //暂存批次最后编辑时间，用于比对批次数据是否被修改过。
                    dr[WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY] = timeZone;
                    dr[WIP_TRANSACTION_FIELDS.FIELD_EDITOR] = editor;
                    dr[WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT] = remark;
                }
                //b.LOT_NUMBER,b.CREATE_OPERTION_NAME,b.FACTORYROOM_KEY,b.PALLET_NO,b.EDIT_TIME LOT_EDIT_TIME
                dtUndo.Columns.Remove("LOT_EDIT_TIME");
                dtUndo.Columns.Remove(POR_LOT_FIELDS.FIELD_LOT_NUMBER);
                dtUndo.Columns.Remove(POR_LOT_FIELDS.FIELD_CREATE_OPERTION_NAME);
                dtUndo.Columns.Remove(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY);
                dtUndo.Columns.Remove(POR_LOT_FIELDS.FIELD_PALLET_NO);
                dsParams.Tables.Add(dtUndo);
                this._entity.LotUndo(dsParams);
                //撤销批次操作失败，给出对应提示。
                if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
                {
                    MessageService.ShowError(this._entity.ErrorMsg);
                    return;
                }
                else
                {
                    //撤销成功给出对应提示。
                    ResetControlValue();
                }
            }
        }
        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
        }

        private void gvUndo_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}

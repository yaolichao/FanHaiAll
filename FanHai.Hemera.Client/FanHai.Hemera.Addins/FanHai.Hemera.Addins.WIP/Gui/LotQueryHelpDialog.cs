using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Common;

namespace FanHai.Hemera.Addins.WIP
{
    /// <summary>
    /// 查询批次的对话框。
    /// </summary>
    public partial class LotQueryHelpDialog : Form
    {
        /// <summary>
        /// 批次查询操作所需参数。
        /// </summary>
        private LotQueryHelpModel _model;
        /// <summary>
        /// 批次查询实体类。
        /// </summary>
        private LotQueryEntity _entity = new LotQueryEntity();
        /// <summary>
        /// 存放批次类型。
        /// </summary>
        private DataTable dtLotType = null;
        /// <summary>
        /// 值被选中事件。
        /// </summary>
        public event LotQueryValueSelectedEventHandler OnValueSelected;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="model">批次查询操作所需参数。</param>
        public LotQueryHelpDialog(LotQueryHelpModel model)
        {
            InitializeComponent();
            this._model = model;
        }

        /// <summary>
        /// 获取批次类型。
        /// </summary>
        private void BindLotType()
        {
            if (dtLotType == null)
            {
                string[] columns = new string[] { "CODE", "NAME" };
                KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Lot_Type);
                dtLotType = BaseData.Get(columns, category);
            }
        }
        /// <summary>
        /// 触发非激活事件，隐藏并关闭对话框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationHelpDialog_Deactivate(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();
        }
        /// <summary>
        /// 窗体载入事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LotQueryHelpDialog_Load(object sender, EventArgs e)
        {
            //BindQueryResult();
        }
        /// <summary>
        /// 绑定查询结果。
        /// </summary>
        private void BindQueryResult()
        {
            string lotNumber=this.teLotNumber.Text.Trim();
            //查询批次
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            //组织查询参数。
            DataSet dsParams = new DataSet();
            Hashtable htParams = new Hashtable();
            htParams.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER,lotNumber);
            htParams.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, this._model.RoomKey);
            switch(_model.OperationType)
            {
                case LotOperationType.CellScrap:
                case LotOperationType.CellDefect:
                case LotOperationType.ReturnMaterial:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
                    break;
                case LotOperationType.Scrap:
                case LotOperationType.Defect:
                case LotOperationType.Terminal:
                case LotOperationType.Merge:
                case LotOperationType.Split:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "N");               //N：生产批次 L：组件补片批次
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
                    break;
                case LotOperationType.CellPatch:
                case LotOperationType.CellRecovered:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "L");               //N：生产批次 L：组件补片批次
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
                    break;
                case LotOperationType.Adjust:
                case LotOperationType.BatchAdjust:
                case LotOperationType.Hold:
                case LotOperationType.BatchHold:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);            //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);    //0:未结束未删除 1：已结束 2：已删除
                    break;
                case LotOperationType.Release:
                case LotOperationType.BatchRelease:
                case LotOperationType.Rework:
                case LotOperationType.BatchRework:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 1);            //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);    //0:未结束未删除 1：已结束 2：已删除
                    break;
                case LotOperationType.Dispatch:
                    htParams.Add(POR_LOT_FIELDS.FIELD_HOLD_FLAG, 0);                //0:未暂停的批次；1：暂停的批次。
                    htParams.Add(POR_LOT_FIELDS.FIELD_LOT_TYPE, "N");               //N：生产批次 L：组件补片批次
                    htParams.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG, 0);        //0:未结束未删除 1：已结束 2：已删除
                    htParams.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, this._model.OperationName);//工序名称
                    break;
                default:
                    break;
            }
            DataTable dtParams=CommonUtils.ParseToDataTable(htParams);
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            //进行查询
            DataSet dsReturn = _entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowMessage(_entity.ErrorMsg);
                return;
            }
            else
            {
                gcResult.DataSource = dsReturn.Tables[0];
                gcResult.MainView = gvResult;
            }
            
        }
        /// <summary>
        /// 根据批次查询。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindQueryResult();
        }
        /// <summary>
        /// 双击选择批次信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_DoubleClick(object sender, EventArgs e)
        {
            int rowIndex = gvResult.FocusedRowHandle;
            if (rowIndex >= 0)
            {
                if (this.OnValueSelected!=null)
                {
                    LotQueryValueSelectedEventArgs args=new LotQueryValueSelectedEventArgs();
                    args.LotNumber=Convert.ToString(gvResult.GetFocusedRowCellValue(POR_LOT_FIELDS.FIELD_LOT_NUMBER));
                    args.LotKey = Convert.ToString(gvResult.GetFocusedRowCellValue(POR_LOT_FIELDS.FIELD_LOT_KEY));
                    this.OnValueSelected(sender, args);
                    if (args.Cancel == true)
                    {
                        return;
                    }
                }
                this.Visible = false;
                this.Close();
            }
        }
        /// <summary>
        /// 自定义绘制单元格值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gclRowNum)
            {
                e.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
            else if (e.Column == this.gclLotType)
            {
                string val=Convert.ToString(e.CellValue);
                BindLotType();
                foreach(object name in dtLotType.AsEnumerable()
                                        .Where(dr=>Convert.ToString(dr["CODE"])==val)
                                        .Select(dr=>dr["NAME"]))
                {
                    e.DisplayText = Convert.ToString(name);
                    break;
                }
            }
            else if (e.Column == this.gclStateFlag)
            {
                LotStateFlag stateFlag = (LotStateFlag)Convert.ToInt32(e.CellValue);
                e.DisplayText = CommonUtils.GetEnumValueDescription(stateFlag);
            }
        }
        /// <summary>
        /// 分页查询。
        /// </summary>
        private void pgnQueryResult_DataPaging()
        {
            BindQueryResult();
        }
    }
    /// <summary>
    /// 查询批次的参数数据
    /// </summary>
    public class LotQueryHelpModel
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotQueryHelpModel()
        {
            this.OperationType = LotOperationType.None;
        }
        /// <summary>
        /// 批次操作类型。
        /// </summary>
        public LotOperationType OperationType { get; set; }
        /// <summary>
        /// 车间主键。
        /// </summary>
        public string RoomKey { get; set; }
        /// <summary>
        /// 工序名称。
        /// </summary>
        public string OperationName { get; set; }
    }
    /// <summary>
    /// 批次查询被选中的参数类。
    /// </summary>
    public class LotQueryValueSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 批次号。
        /// </summary>
        public string LotNumber { get; set; }
        /// <summary>
        /// 批次主键。
        /// </summary>
        public string LotKey { get; set; }
        /// <summary>
        /// 是否取消选中值。
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotQueryValueSelectedEventArgs()
        {
            this.Cancel = false;
        }
    }
    /// <summary>
    /// 批次查询被选中的事件委托类。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LotQueryValueSelectedEventHandler(object sender, LotQueryValueSelectedEventArgs e);
}

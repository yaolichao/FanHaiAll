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
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Dialogs;
using System.Net;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotExchgWoSingleForm : BaseDialog
    {
        private ExchangeWoEntity exchangewoEntity = new ExchangeWoEntity();
        private string _factoryname = string.Empty, _factorykey = string.Empty;
        private string _lotnum = string.Empty, _workorder = string.Empty, _workorder2 = string.Empty;
        private string _proid = string.Empty, _proid2 = string.Empty;
        private bool _isContinue = false;
        private ExchangeWoFlag sRepair;
        public LotExchgWoSingleForm()
        {
            InitializeComponent();
        }
        public LotExchgWoSingleForm(ExchangeWoFlag repair)
        {
            InitializeComponent();
            this.sRepair = repair;
        }

        private void LotExchgWoSingleForm_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();

            if (sRepair == ExchangeWoFlag.Repair)
            {
                this.txtPro_id.Properties.ReadOnly = true;
                this.Text = "返工工单作业";
            }
            else
            {
                this.txtPro_id.Properties.ReadOnly = false;
                this.Text = "批次转工单作业";
            }            
            this.txtLotNumber.Focus();
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
        }
        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            if (lueFactoryRoom.EditValue != null)
            {
                _factorykey = this.lueFactoryRoom.EditValue.ToString();
                _factoryname = this.lueFactoryRoom.Text.Trim();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region isValid data
            if (string.IsNullOrEmpty(txtWoNumber.Text.Trim()) )
            {
                MessageService.ShowError("工单号不能都为空!");
                txtWoNumber.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPro_id.Text.Trim()))
            {
                MessageService.ShowError("产品ID号不能都为空!");
                txtPro_id.Focus();                
                return;
            }
            if (string.IsNullOrEmpty(txtWoNumber2.Text.Trim()))
            {
                MessageService.ShowError("原工单号不能都为空!");
                txtWoNumber2.Focus();                
                return;
            }
            if (string.IsNullOrEmpty(txtPro_id2.Text.Trim()))
            {
                MessageService.ShowError("原产品ID号不能都为空!");
                txtPro_id2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(btnEditRoute.Text.Trim()))
            {
                MessageService.ShowError("请指定工艺流程组!");
                btnEditRoute.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtOperation.Text.Trim()))
            {
                MessageService.ShowError("请指定工艺路线!");
                txtOperation.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtStep.Text.Trim()))
            {
                MessageService.ShowError("请指定工序!");
                txtStep.Focus();
                return;
            }
            if (string.IsNullOrEmpty(emDescriptions.Text.Trim()))
            {
                MessageService.ShowError("请输入备注原因!");
                return;
            }
            if (txtWoNumber.Text.Trim().Equals(txtWoNumber2.Text.Trim())
                && txtPro_id.Text.Trim().Equals(txtPro_id2.Text.Trim()))
            {
                MessageService.ShowError("工单和产品ID号不符合转工单作业!");
                txtWoNumber.SelectAll();
                txtWoNumber.Focus();
                return;
            }
            Shift shift = new Shift();
            string shiftName = shift.GetCurrShiftName();
            string shiftKey = string.Empty;// shift.IsShiftValueExists(shiftName);
            //if (string.IsNullOrEmpty(shiftKey))
            //{
            //    MessageService.ShowError("没有获取到班别!");
            //    return;
            //}
            //---------------------------------------------------------------------------------------------
            int v = 13; char k = (char)v;
            txtLotNumber_KeyPress(null, new KeyPressEventArgs(k));
            if (!this._isContinue) return;
            //txtWoNumber_KeyPress(null, new KeyPressEventArgs(k));
            //if (!this._isContinue) return;
            txtPro_id_KeyPress(null, new KeyPressEventArgs(k));
            if (!this._isContinue) return;

            //转工单：工单类型条件判断
            #region
            Hashtable hsOrderType=new Hashtable();
            hsOrderType.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, txtWoNumber.Text.Trim());
            hsOrderType.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER+"2", txtWoNumber2.Text.Trim());
            string msg = exchangewoEntity.CompareWorkOrderType(hsOrderType);
            if (!string.IsNullOrEmpty(msg))
            {
                MessageService.ShowError(msg);
                txtWoNumber.SelectAll();
                txtWoNumber.Focus();
                return;
            }
            #endregion

            Hashtable hstable = new Hashtable();
            hstable["flag"] = "w";
            hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWoNumber.Text.Trim();

            DataSet dsRetun = exchangewoEntity.GetExchangeByFilter(hstable);
            if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
            {
                MessageService.ShowError(exchangewoEntity.ErrorMsg);
                txtWoNumber.Focus();
                txtWoNumber.SelectAll();
                return;
            }

            DataTable dtCommon = dsRetun.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            if (dtCommon.Rows.Count < 1)
            {
                MessageService.ShowMessage(string.Format("未找到工单号:{0}，请确认!", txtWoNumber.Text.Trim()));
                txtWoNumber.Focus();
                txtWoNumber.SelectAll();
                return;
            }
            DataRow drCommon = dtCommon.Rows[0];
            string s_pro = drCommon[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();
            if (string.IsNullOrEmpty(s_pro))
            {
                MessageService.ShowError(string.Format("工单号【{0}】没有对应的产品ID号，请确认!", txtWoNumber.Text.Trim()));
                txtWoNumber.SelectAll();
                txtWoNumber.Focus();
                return;
            }
            txtWoNumber.Tag = drCommon[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY].ToString();

            //---------------------------------------------------------------------------------------------
            string workorder = txtWoNumber.Text.Trim();
            string proid = txtPro_id.Text.Trim();
             hstable = new Hashtable();
            hstable.Add("flag", "save");
            hstable.Add(POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER, workorder);
            hstable.Add(POR_WORK_ORDER_FIELDS.FIELD_PRO_ID, proid);
            DataSet dsback = exchangewoEntity.GetExchangeByFilter(hstable);
            if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
            {
                MessageService.ShowError(exchangewoEntity.ErrorMsg);                
                txtLotNumber.Focus();
                txtLotNumber.SelectAll();
                return;
            }

            DataTable dtWork = new DataTable(), dtWorkAttr = new DataTable();
            if (dsback.Tables.Contains(POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME))
                dtWork = dsback.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
            if (dsback.Tables.Contains(POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME))
                dtWorkAttr = dsback.Tables[POR_WORK_ORDER_ATTR_FIELDS.DATABASE_TABLE_NAME];
            if (dtWork.Rows.Count < 1 && dtWorkAttr.Rows.Count < 1)
            {
                MessageService.ShowError(string.Format("工单【{0}】中未设置产品ID号【{1}】，请确认!", workorder, proid));
                txtWoNumber.SelectAll();
                txtWoNumber.Focus();
                return;
            }          
            #endregion
        
            DataSet dsSave=new DataSet();

            dsSave.ExtendedProperties.Add(ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_CHANGE_WO);
            
            DataTable dtPor_Lot = new DataTable(POR_LOT_FIELDS.DATABASE_TABLE_NAME_FORUPDATE);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_PRO_ID);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_ROUTE_ENTERPRISE_VER_KEY);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_CUR_ROUTE_VER_KEY);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_STATE_FLAG);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_PALLET_NO);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_EDITOR);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_EDIT_TIME);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_SHIFT_NAME);
            dtPor_Lot.Columns.Add(POR_LOT_FIELDS.FIELD_DESCRIPTIONS);
           

            DataRow drPor_lot = dtPor_Lot.NewRow();
            drPor_lot[POR_LOT_FIELDS.FIELD_LOT_NUMBER] = txtLotNumber.Text.Trim();
            drPor_lot[POR_LOT_FIELDS.FIELD_DESCRIPTIONS] = emDescriptions.Text.Trim();
            dtPor_Lot.Rows.Add(drPor_lot);

            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, Dns.GetHostName());
            dsSave.ExtendedProperties.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_NO, txtWoNumber.Text.Trim());
            dsSave.ExtendedProperties.Add(POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY, txtWoNumber.Tag.ToString());
            dsSave.ExtendedProperties.Add(POR_LOT_FIELDS.FIELD_PRO_ID, txtPro_id.Text.Trim());
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_KEY, shiftKey);
            dsSave.ExtendedProperties.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);

            dsSave.ExtendedProperties.Add(CHECKTYPE.DATA_TYPE, Convert.ToString(ExchangeWoFlag.Exchange));

            dsSave.ExtendedProperties.Add(POR_LOT_FIELDS.FIELD_CREATOR, PropertyService.Get(PROPERTY_FIELDS.USER_NAME));
            dsSave.Merge(dtPor_Lot, true, MissingSchemaAction.Add);

            DataTable dtRoute = new DataTable(POR_ROUTE_ROUTE_VER_FIELDS.DATABASE_TABLE_NAME);
            dtRoute.Columns.Add(POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME);
            dtRoute.Columns.Add(POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME);
            dtRoute.Columns.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME);

            DataRow drNew01 = dtRoute.NewRow();
            drNew01[POR_ROUTE_ENTERPRISE_VER_FIELDS.FIELD_ENTERPRISE_NAME] = btnEditRoute.Text.Trim();
            drNew01[POR_ROUTE_ROUTE_VER_FIELDS.FIELD_ROUTE_NAME] = txtOperation.Text.Trim();
            drNew01[POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME] = txtStep.Text.Trim();
            dtRoute.Rows.Add(drNew01);
            dtRoute.AcceptChanges();
            dsSave.Merge(dtRoute, true, MissingSchemaAction.Add);

            DataSet dsReturn = exchangewoEntity.SaveExchangeWo(dsSave);
            if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
            {
                MessageService.ShowError(exchangewoEntity.ErrorMsg);
                return;
            }
            else
            {
                MessageService.ShowMessage(string.Format("批次【{0}】转工单成功!", txtLotNumber.Text.Trim()), "提示");
                txtLotNumber.Text = string.Empty;
                txtPro_id.Text = string.Empty;
                txtPro_id2.Text = string.Empty;
                txtWoNumber.Text = string.Empty;
                txtWoNumber2.Text = string.Empty;
                emDescriptions.Text = string.Empty;
                txtOperation.Text = string.Empty;
                btnEditRoute.Text = string.Empty;
                txtStep.Text = string.Empty;               
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtLotNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (lueFactoryRoom.EditValue == null)
                {
                    MessageService.ShowMessage(string.Format("请选择工厂车间!", "提示"));
                    lueFactoryRoom.Focus();
                    _isContinue = false;
                    return;
                }

                if (string.IsNullOrEmpty(txtLotNumber.Text.Trim())) return;

                Hashtable hstable = new Hashtable();
                
                hstable.Add("flag", "l");
                hstable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, txtLotNumber.Text.Trim());
                hstable.Add(POR_LOT_FIELDS.FIELD_FACTORYROOM_KEY, _factorykey);
                if (sRepair == ExchangeWoFlag.Repair)
                {
                    //批次是否已入库
                    hstable[LotStateFlag.ToStore.ToString()] = LotStateFlag.ToStore.ToString();
                }

                DataSet dsRetun = exchangewoEntity.GetExchangeByFilter(hstable);
                if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
                {
                    MessageService.ShowError(exchangewoEntity.ErrorMsg);
                    txtLotNumber.Focus();
                    txtLotNumber.SelectAll();
                    _isContinue = false;
                    return;
                }

                DataTable dtCommon = dsRetun.Tables[POR_LOT_FIELDS.DATABASE_TABLE_NAME];
                if (dtCommon.Rows.Count < 1)
                {
                    MessageService.ShowMessage(string.Format("未找到批次:{0}，请确认!", txtLotNumber.Text.Trim()));
                    txtLotNumber.Focus();
                    txtLotNumber.SelectAll();
                    _isContinue = false;
                    return;
                }                
                DataRow drCommon = dtCommon.Rows[0];
       
                if (Convert.ToInt16(drCommon[POR_LOT_FIELDS.FIELD_DELETED_TERM_FLAG]) == 1)
                {
                    MessageService.ShowMessage(string.Format("批次【{0}】已经结束，不能转工单作业!", txtLotNumber.Text.Trim()), "提示");
                    txtLotNumber.Focus();
                    txtLotNumber.SelectAll();
                    _isContinue = false;
                    return;
                }

                if (!string.IsNullOrEmpty(drCommon[POR_LOT_FIELDS.FIELD_PALLET_NO].ToString()))
                {
                    MessageService.ShowMessage(string.Format("批次【{0}】已在包装托【{1}】中，请出托后在转工单!",
                        txtLotNumber.Text.Trim(), drCommon[POR_LOT_FIELDS.FIELD_PALLET_NO].ToString()));
                    txtLotNumber.Focus();
                    txtLotNumber.SelectAll();
                    _isContinue = false;
                    return;
                }

                if (Convert.ToInt16(drCommon[POR_LOT_FIELDS.FIELD_HOLD_FLAG]) == 0)
                {
                    MessageService.ShowMessage(string.Format("批次【{0}】请先暂停，在转工单作业!", txtLotNumber.Text.Trim()), "提示");
                    txtLotNumber.Focus();
                    txtLotNumber.SelectAll();
                    _isContinue = false;
                    return;
                }

                _isContinue = true;
                txtWoNumber2.Text = drCommon[POR_LOT_FIELDS.FIELD_WORK_ORDER_NO].ToString();
                txtPro_id2.Text = drCommon[POR_LOT_FIELDS.FIELD_PRO_ID].ToString();
                SetFocusCompontent();
            }
        }

        private void SetFocusCompontent()
        {
            if (txtLotNumber.Text.Trim().Equals(string.Empty))
            {
                txtLotNumber.Focus();
                return;
            }
            if (txtWoNumber.Text.Trim().Equals(string.Empty))
            {
                txtWoNumber.Focus();
                return;
            }

            if (txtPro_id.Text.Trim().Equals(string.Empty))
            {
                txtPro_id.Focus();
                return;
            }

            if (btnEditRoute.Text.Trim().Equals(string.Empty)
                || txtOperation.Text.Trim().Equals(string.Empty)
                || txtStep.Text.Trim().Equals(string.Empty))
            {
                btnEditRoute.Focus();
                return;
            }
            if (!btnSave.Focused)
                btnSave.Focus();
        }


        private void txtWoNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtLotNumber.Text.Trim())) return;
                if (string.IsNullOrEmpty(txtWoNumber.Text.Trim())) return;

                Hashtable hstable = new Hashtable();
                hstable["flag"] = "w";
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWoNumber.Text.Trim();

                DataSet dsRetun = exchangewoEntity.GetExchangeByFilter(hstable);
                if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
                {
                    MessageService.ShowError(exchangewoEntity.ErrorMsg);
                    txtWoNumber.Focus();
                    txtWoNumber.SelectAll();
                    _isContinue = false;
                    return;
                }

                DataTable dtCommon = dsRetun.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME];
                if (dtCommon.Rows.Count < 1)
                {
                    MessageService.ShowMessage(string.Format("未找到工单号:{0}，请确认!", txtWoNumber.Text.Trim()));
                    txtWoNumber.Focus();
                    txtWoNumber.SelectAll();
                    _isContinue = false;   
                    return;
                }
                DataRow drCommon = dtCommon.Rows[0];
                string s_pro = drCommon[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();
                if (string.IsNullOrEmpty(s_pro))
                {
                    MessageService.ShowError(string.Format("工单号【{0}】没有对应的产品ID号，请确认!", txtWoNumber.Text.Trim()));
                    txtWoNumber.SelectAll();
                    txtWoNumber.Focus();
                    _isContinue = false;
                    return;
                }
                _isContinue = true;
                txtWoNumber.Tag = drCommon[POR_WORK_ORDER_FIELDS.FIELD_WORK_ORDER_KEY].ToString();
                txtPro_id.Text = drCommon[POR_PRODUCT.FIELDS_PRODUCT_CODE].ToString();

                SetFocusCompontent();
            }       
        }

        private void txtPro_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtWoNumber.Text.Trim()))
                {
                    MessageService.ShowError("工单号不能为空!");
                    txtWoNumber.Focus();
                    return;
                }

                Hashtable hstable = new Hashtable();
                hstable["flag"] = "p";
                hstable[POR_PRODUCT.FIELDS_PRODUCT_CODE] = txtPro_id.Text.Trim();
                hstable[POR_WORK_ORDER_FIELDS.FIELD_ORDER_NUMBER] = txtWoNumber.Text.Trim();

                DataSet dsRetun = exchangewoEntity.GetExchangeByFilter(hstable);
                if (!string.IsNullOrEmpty(exchangewoEntity.ErrorMsg))
                {
                    MessageService.ShowError(exchangewoEntity.ErrorMsg);
                    txtPro_id.Focus();
                    txtPro_id.SelectAll();
                    _isContinue = false;
                    return;
                }

                DataTable dtCommon = dsRetun.Tables[POR_PRODUCT.DATABASE_TABLE_NAME];
                if (dtCommon.Rows.Count < 1)
                {
                    MessageService.ShowMessage(string.Format("未找到产品ID号【{0}】，请确认!", txtPro_id.Text.Trim()));
                    txtPro_id.Focus();
                    txtPro_id.SelectAll();
                    _isContinue = false;
                    return;
                }
                _isContinue = true;
                SetFocusCompontent();
            }
        }

        private void LotExchgWoSingleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnEditRoute_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OperationHelpDialog dlg = new OperationHelpDialog();
            //车间名称
            dlg.FactoryRoom = _factoryname;
            //dlg.FactoryRoom = "F5M7";
            dlg.ProductType = string.Empty;
            dlg.EnterpriseName = btnEditRoute;
            dlg.RouteName = txtOperation;
            dlg.StepName = txtStep;
            dlg.IsRework = false;
            Point i = btnEditRoute.PointToScreen(new Point(0, 0));
            dlg.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Rectangle screenArea = System.Windows.Forms.Screen.GetWorkingArea(this);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            if ((screenWidth - i.X) > dlg.Width)
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X, i.Y + btnEditRoute.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X, i.Y - dlg.Height);
                }
            }
            else
            {
                if ((screenHeight - i.Y) > dlg.Height)
                {
                    dlg.Location = new Point(i.X + btnEditRoute.Width - dlg.Width, i.Y + btnEditRoute.Height);

                }
                else
                {
                    dlg.Location = new Point(i.X + btnEditRoute.Width - dlg.Width, i.Y - dlg.Height);
                }
            }
            dlg.Visible = true;
            dlg.Show();
        }
 
    }
}
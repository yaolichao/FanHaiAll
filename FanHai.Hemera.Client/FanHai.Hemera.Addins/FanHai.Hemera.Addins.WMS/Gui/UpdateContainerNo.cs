using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using System.Collections;
using FanHai.Hemera.Utils.Dialogs;
using System.Linq;
using FanHai.Hemera.Share.Common;
using System.Threading;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class UpdateContainerNo : BaseUserCtrl
    {
        IViewContent _view = null;                                      //当前视图。
        /// <summary>
        /// 出货操作对象。
        /// </summary>
        ShipmentOperationEntity _entity = new ShipmentOperationEntity();
        public UpdateContainerNo()
        {
            InitializeComponent();
        }

        private void UpdateContainerNo_Load(object sender, EventArgs e)
        {
            BindShipmentType();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }

        /// <summary>
        /// 绑定出货类型。
        /// </summary>
        private void BindShipmentType()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", BASEDATA_CATEGORY_NAME.Basic_ShipmentType);
            this.lueShipmentType1.Properties.DataSource = BaseData.Get(columns, category);
            this.lueShipmentType1.Properties.DisplayMember = "NAME";
            this.lueShipmentType1.Properties.ValueMember = "CODE";
        }

        /// <summary>
        /// 查询按钮Click事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataSet dsParams = this.GetQueryCondition();
            PagingQueryConfig config = new PagingQueryConfig()
            {
                PageNo = pgnQueryResult.PageNo,
                PageSize = pgnQueryResult.PageSize
            };
            DataSet dsReturn = this._entity.Query(dsParams, ref config);
            pgnQueryResult.Pages = config.Pages;
            pgnQueryResult.Records = config.Records;
            if (!string.IsNullOrEmpty(this._entity.ErrorMsg))
            {
                MessageService.ShowMessage(this._entity.ErrorMsg);
                return;
            }
            if (dsReturn.Tables.Count > 0)
            {
                gcList.DataSource = dsReturn.Tables[0];
                gcList.MainView = gvList;
                gvList.BestFitColumns();
            }
        }

        /// <summary>
        /// 获取查询条件。
        /// </summary>
        /// <returns>包含查询条件的数据集对象。</returns>
        private DataSet GetQueryCondition()
        {
            string shipmentNo = this.teShipmentNo.Text.Trim();
            string containerNo = this.teContainerNo.Text.Trim();
            string ciNo = this.teCINumber.Text.Trim();
            string shipmentType = Convert.ToString(this.lueShipmentType1.EditValue);
            string palletNo = "";
            Hashtable htParams = new Hashtable();
            if (!string.IsNullOrEmpty(shipmentNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_NO, shipmentNo);
            }
            if (!string.IsNullOrEmpty(containerNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_CONTAINER_NO, containerNo);
            }
            if (!string.IsNullOrEmpty(shipmentType))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_SHIPMENT_TYPE, shipmentType);
            }
            if (!string.IsNullOrEmpty(palletNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_PALLET_NO, palletNo);
            }
            if (!string.IsNullOrEmpty(ciNo))
            {
                htParams.Add(WMS_SHIPMENT_FIELDS.FIELDS_CI_NO, ciNo);
            }
            DataTable dtParams = CommonUtils.ParseToDataTable(htParams);
            DataSet dsParams = new DataSet();
            dtParams.TableName = TRANS_TABLES.TABLE_MAIN_DATA;
            dsParams.Tables.Add(dtParams);
            return dsParams;
        }


    }
}

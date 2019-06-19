using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using System.Xml;

namespace FanHai.Hemera.Addins.WMS
{
    public partial class ToBeSet : BaseUserCtrl
    {
        IViewContent _view = null;
        XmlDocument doc = new XmlDocument();
        DataTable dtgc = new DataTable();
        FanHai.Gui.Core.Properties webApiAddress = PropertyService.Get("FanHai.Gui.Framework.Gui.WebApi", new FanHai.Gui.Core.Properties());

        /// <summary>
        /// 增加生产收货单请求数据
        /// </summary>
        public class AddInventoryGenEntryDataObject
        {
            public string DocDate { get; set; }

            public string Reference2 { get; set; }

            public string Comments { get; set; }

            public int BaseEntry { get; set; }

            public int Quantity { get; set; }

            public int AllOverReceive { get; set; }

        }

        /// <summary>
        /// 第一次调用响应数据
        /// </summary>
        public class ReceiveFirstDataObject
        {
            public string Code { get; set; }

            public string Message { get; set; }

            public SubItem Data { get; set; }
            public class SubItem
            {
                public string DocEntry { get; set; }
            }
        }


        /// <summary>
        /// 第二次调用响应数据
        /// </summary>
        public class ReceiveSecondDataObject
        {
            public string Code { get; set; }

            public string Message { get; set; }

            public SubItem Data { get; set; }
            public class SubItem
            {
                public int DocEntry { get; set; }
            }
        }

        /// <summary>
        /// 增加生产订单请求数据
        /// </summary>
        public class AddProductionOrdersDataObject
        {
            public string ItemNo { get; set; }

            public int PlannedQuantity { get; set; }

            public string Warehouse { get; set; }

            public string PostingDate { get; set; }

            public string StartDate { get; set; }

            public string DueDate { get; set; }

            public string U_MAIN_MO_ID { get; set; }

            public string DistributionRule { get; set; }

            public string U_ProductionLine { get; set; }
        }

        public ToBeSet()
        {
            InitializeComponent();
            this.lblMenu.Text = "";
            dtgc.Columns.Add(new DataColumn("gcWorkOrder", typeof(string)));
            dtgc.Columns.Add(new DataColumn("gcProductCode", typeof(string)));
            dtgc.Columns.Add(new DataColumn("gcQty", typeof(string)));
            dtgc.Columns.Add(new DataColumn("gcNewWorkOrder", typeof(string)));
            gcList.DataSource = dtgc;

            //XmlDocument doc = new XmlDocument();

            //doc.Load(Application.StartupPath+ "\\Settings\\AMESProperties.xml");

            //XmlNodeList list = doc.SelectNodes("/WebApi");
        }


        // 提交接口
        public static string HttpPost(string url, string body)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ProtocolVersion = new Version(1, 0);   //Http/1.0版本
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        // 读取时间戳
        public static string GetTimeStamp()
        {
            DateTime time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位  
            return t.ToString();
        }

        public ReceiveFirstDataObject InvokeFirstAPI(string itemNo, int plannedQuantity, string warehouse, DateTime postingDate, DateTime startDate,
            DateTime dueDate, string U_MAIN_MO_ID, string distributionRule, string U_ProductionLine)
        {
            // 发送数据
            string ApiUrl = "";
            string postResult = "";
            try
            {
                string customerId = "OWNVIEW";
                string appkey = "c4ca4238a0b923820dcc509a6f75849b";
                string secret = "OwnviewSAP";
                string method = "ProductionOrders.Add";
                string signmethod = "md5";
                string timestamp = GetTimeStamp();

                //将请求数据转为Json，需要修改
                AddProductionOrdersDataObject addProductionOrdersDataObject = new AddProductionOrdersDataObject();
                addProductionOrdersDataObject.ItemNo = itemNo;
                addProductionOrdersDataObject.PlannedQuantity = plannedQuantity;
                addProductionOrdersDataObject.Warehouse = warehouse;
                addProductionOrdersDataObject.PostingDate = postingDate.ToString("yyyy-MM-dd");
                addProductionOrdersDataObject.StartDate = startDate.ToString("yyyy-MM-dd");
                addProductionOrdersDataObject.DueDate = dueDate.ToString("yyyy-MM-dd");
                addProductionOrdersDataObject.U_MAIN_MO_ID = U_MAIN_MO_ID;
                addProductionOrdersDataObject.DistributionRule = distributionRule;
                addProductionOrdersDataObject.U_ProductionLine = U_ProductionLine;

                string postData = JsonConvert.SerializeObject(addProductionOrdersDataObject);

                string sign = secret + "appkey" + appkey + "customerid" + customerId + "method" + method + "timestamp" + timestamp + postData + secret;
                sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToLower();
                //接口待定
                ApiUrl = "http://192.168.1.49:9003/SAPB1WebApi"
                                + "?method=" + System.Web.HttpUtility.UrlEncode(method, Encoding.UTF8)
                                + "&timestamp=" + System.Web.HttpUtility.UrlEncode(timestamp, Encoding.UTF8)
                                + "&appkey=" + System.Web.HttpUtility.UrlEncode(appkey, Encoding.UTF8)
                                + "&customerid=" + System.Web.HttpUtility.UrlEncode(customerId, Encoding.UTF8)
                                + "&sign=" + System.Web.HttpUtility.UrlEncode(sign, Encoding.UTF8)
                                + "&signmethod=" + System.Web.HttpUtility.UrlEncode(signmethod, Encoding.UTF8);
                postResult = "";

                MessageService.ShowMessage(ApiUrl);
                postResult = HttpPost(ApiUrl, postData);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message + " " + ApiUrl);
                return new ReceiveFirstDataObject();
            }
            //将Json转为响应数据
            ReceiveFirstDataObject data = JsonConvert.DeserializeObject<ReceiveFirstDataObject>(postResult);
            MessageService.ShowMessage(postResult);
            return data;
        }

        public ReceiveSecondDataObject InvokeSecondAPI(DateTime docDate, string reference2, string comments, int baseEntry, int quantity,
            int allOverReceive)
        {
            // 发送数据
            string ApiUrl = "";
            string postResult = "";
            try
            {
                string customerId = "OWNVIEW";
                string appkey = "c4ca4238a0b923820dcc509a6f75849b";
                string secret = "OwnviewSAP";
                string method = "InventoryGenEntry.Add";
                string signmethod = "md5";
                string timestamp = GetTimeStamp();

                //将请求数据转为Json，需要修改
                AddInventoryGenEntryDataObject addInventoryGenEntryDataObject = new AddInventoryGenEntryDataObject();
                addInventoryGenEntryDataObject.DocDate = docDate.ToString("yyyy-MM-dd");
                addInventoryGenEntryDataObject.Reference2 = reference2;
                addInventoryGenEntryDataObject.Comments = comments;
                addInventoryGenEntryDataObject.BaseEntry = baseEntry;
                addInventoryGenEntryDataObject.Quantity = quantity;
                addInventoryGenEntryDataObject.AllOverReceive = allOverReceive;

                string postData = JsonConvert.SerializeObject(addInventoryGenEntryDataObject);

                string sign = secret + "appkey" + appkey + "customerid" + customerId + "method" + method + "timestamp" + timestamp + postData + secret;
                sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToLower();
                //接口待定
                ApiUrl = "http://192.168.1.49:9003/SAPB1WebApi"
                                + "?method=" + System.Web.HttpUtility.UrlEncode(method, Encoding.UTF8)
                                + "&timestamp=" + System.Web.HttpUtility.UrlEncode(timestamp, Encoding.UTF8)
                                + "&appkey=" + System.Web.HttpUtility.UrlEncode(appkey, Encoding.UTF8)
                                + "&customerid=" + System.Web.HttpUtility.UrlEncode(customerId, Encoding.UTF8)
                                + "&sign=" + System.Web.HttpUtility.UrlEncode(sign, Encoding.UTF8)
                                + "&signmethod=" + System.Web.HttpUtility.UrlEncode(signmethod, Encoding.UTF8);

                MessageService.ShowMessage(ApiUrl);

                postResult = HttpPost(ApiUrl, postData);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message + " " + ApiUrl);
                return new ReceiveSecondDataObject();
            }
            //将Json转为响应数据
            ReceiveSecondDataObject data = JsonConvert.DeserializeObject<ReceiveSecondDataObject>(postResult);
            MessageService.ShowMessage(postResult);
            return data;
        }


        private void txtPallet_NO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LotQueryEntity queryEntity = new LotQueryEntity();
                LotOperationEntity workOrderEntity = new LotOperationEntity();
                DataSet dsLotInfo = new DataSet();
                dsLotInfo = queryEntity.GetLotInfoByPallet_No(txtPallet_NO.Text);
                if (!string.IsNullOrEmpty(queryEntity.ErrorMsg))
                {
                    MessageService.ShowError(queryEntity.ErrorMsg);
                    return;
                }
                //该托盘号有数据
                if (dsLotInfo.Tables[0].Rows.Count > 0)
                {
                    //获得该托盘号对应的料号
                    string partNumber = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_PART_NUMBER].ToString();
                    //获得该托盘号对应的工单号
                    string workOrder = dsLotInfo.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_NO].ToString();

                    string ocrCode = "";

                    //获取该工单号的数据
                    DataSet dsProductData = workOrderEntity.GetWoProductData(workOrder);
                    if (string.IsNullOrEmpty(workOrderEntity.ErrorMsg))
                    {
                        //查到工单对应的数据
                        if (dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                        {
                            //工单号对应的料号与托盘号对应的料号一致，执行第二次请求，需回填新工单号字段
                            if (dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_PART_NUMBER].ToString() == partNumber)
                            {
                                //执行第二次请求
                                ReceiveSecondDataObject receiveSecondDataObject = new ReceiveSecondDataObject();
                                receiveSecondDataObject = InvokeSecondAPI(DateTime.Now, "", "",
                                    Convert.ToInt32(workOrder), dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count, 0);
                                
                                //receiveSecondDataObject = InvokeSecondAPI(DateTime.Now, "", "",
                                //    41, 1, 0);

                                //调用成功
                                if (receiveSecondDataObject != null && receiveSecondDataObject.Code == "0")
                                {
                                    int newInWarehouseOrder = 0;
                                    newInWarehouseOrder = receiveSecondDataObject.Data.DocEntry;

                                    //回填por_lot表新工单号
                                    bool isUpdatePor_lot = workOrderEntity.UpdatePor_Lot(workOrder.ToString(), txtPallet_NO.Text);
                                    if (!isUpdatePor_lot)
                                    {
                                        MessageService.ShowError("新工单号写入失败");
                                        return;
                                    }
                                    MessageService.ShowMessage("新工单号填入成功");
                                    //回填Wip_consignment表入库单号
                                    bool isUpdateWip_consignment = workOrderEntity.UpdateWip_consignment(newInWarehouseOrder.ToString(), txtPallet_NO.Text);
                                    if (!isUpdateWip_consignment)
                                    {
                                        MessageService.ShowError("入库单号写入失败");
                                        return;
                                    }
                                    MessageService.ShowMessage("入库单号写入成功");
                                    dtgc.Clear();
                                    DataRow dr = dtgc.NewRow();
                                    dr["gcWorkOrder"] = workOrder;
                                    dr["gcProductCode"] = partNumber;
                                    dr["gcQty"] = dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count;
                                    dr["gcNewWorkOrder"] = workOrder;
                                }
                                else
                                {
                                    MessageService.ShowError("调用第二次接口失败");
                                }
                            }
                            //不一致，执行第一次请求，再执行第二次请求，需回填por_lot新工单号字段
                            else
                            {
                                ReceiveFirstDataObject receiveFirstDataObject = new ReceiveFirstDataObject();
                                //receiveFirstDataObject = InvokeFirstAPI("2010005103", 1,
                                //      "301", DateTime.Now, DateTime.Now, DateTime.Now, "41",
                                //     dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_OCRCODE].ToString(), "");
                                receiveFirstDataObject = InvokeFirstAPI(partNumber, dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count,
                                     dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_STOCK_LOCATION].ToString(), DateTime.Now, DateTime.Now, DateTime.Now, workOrder,
                                    dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows[0][POR_WORK_ORDER_FIELDS.FIELD_OCRCODE].ToString(), "");
                                //调用成功
                                if (receiveFirstDataObject != null && receiveFirstDataObject.Code == "0")
                                {
                                    int newWorkOrder = 0;
                                    try
                                    {
                                        newWorkOrder = Convert.ToInt32(receiveFirstDataObject.Data.DocEntry);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageService.ShowError("新工单号格式错误");
                                        return;
                                    }

                                    //回填por_lot表新工单号
                                    bool isUpdatePor_lot = workOrderEntity.UpdatePor_Lot(newWorkOrder.ToString(), txtPallet_NO.Text);
                                    if (!isUpdatePor_lot)
                                    {
                                        MessageService.ShowError("新工单号写入失败");
                                        return;
                                    }
                                    else
                                    {
                                        MessageService.ShowMessage("新工单号写入成功");
                                    }

                                    //执行第二次请求
                                    ReceiveSecondDataObject receiveSecondDataObject = new ReceiveSecondDataObject();
                                    receiveSecondDataObject = InvokeSecondAPI(DateTime.Now, "", "",
                                        Convert.ToInt32(receiveFirstDataObject.Data.DocEntry), dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count, 0);

                                    //receiveSecondDataObject = InvokeSecondAPI(DateTime.Now, "", "",
                                    //    41, 1, 0);
                                    //调用成功
                                    if (receiveSecondDataObject.Code == "0")
                                    {
                                        int newInWarehouseOrder = 0;
                                        newInWarehouseOrder = receiveSecondDataObject.Data.DocEntry;
                                        //回填Wip_consignment表入库单号
                                        bool isUpdateWip_consignment = workOrderEntity.UpdateWip_consignment(newInWarehouseOrder.ToString(), txtPallet_NO.Text);
                                        if (!isUpdateWip_consignment)
                                        {
                                            MessageService.ShowError("入库单号写入失败");
                                            return;
                                        }
                                        else
                                        {
                                            MessageService.ShowMessage("入库单写入成功");
                                            dtgc.Clear();
                                            DataRow dr = dtgc.NewRow();
                                            dr["gcWorkOrder"] = workOrder;
                                            dr["gcProductCode"] = partNumber;
                                            dr["gcQty"] = dsProductData.Tables[POR_WORK_ORDER_FIELDS.DATABASE_TABLE_NAME].Rows.Count;
                                            dr["gcNewWorkOrder"] = newWorkOrder;
                                            dtgc.Rows.Add(dr);
                                        }
                                    }
                                    else
                                    {
                                        MessageService.ShowError("调用第二次接口失败");
                                    }
                                }
                                else
                                {
                                    MessageService.ShowError("调用第一次接口失败");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageService.ShowError("托盘号对应的工单没有明细");
                            return;
                        }
                    }
                    else
                    {
                        MessageService.ShowError(workOrderEntity.ErrorMsg);
                        return;
                    }
                }
                else
                {
                    MessageService.ShowError("托盘号不存在！");
                    return;
                }
            }
        }

        private void btnInWarehouse_Click(object sender, EventArgs e)
        {

        }
    }
}

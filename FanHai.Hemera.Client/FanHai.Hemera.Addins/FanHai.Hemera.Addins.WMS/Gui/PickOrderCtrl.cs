using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using SAPLogonCtrl;
//using SAPFunctionsOCX;
//using SAPTableFactoryCtrl;

using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Interface.RFC;




namespace FanHai.Hemera.Addins.WMS.Gui
{

    public partial class PickOrderCtrl:BaseUserCtrl
    {
        DataSet ds;
        DataSet dsCabinet;
        
        public PickOrderCtrl()
        {
            InitializeComponent();
            dsCabinet = new DataSet();            
        }

        private void ClearDataset()
        {
            try
            {
                ds.Clear();
                dsCabinet.Clear();
            }
            catch { };
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TBVbeln.Text))
            {
                MessageBox.Show(this, "请输入销售订单号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ClearDataset();
            string rfcFuntionName = "ZAST_GET_SO";
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            if (null != serverFactory)
            {
                IRFCEngine rfcCallObj = serverFactory.Get<IRFCEngine>();
                DataSet dsParams = new DataSet();
                dsParams.ExtendedProperties.Add("WK_VBELN",  this.TBVbeln.Text);
                ds = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
            }
            if (ds.Tables["RT_VBAP"].Rows.Count <= 0)
            {
                MessageBox.Show(this, "没有查询到你所要的数据，请核对查询条件！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            addCol(ds.Tables["RT_VBAP"], "Sapqty", typeof(float));
            addCol(ds.Tables["RT_VBAP"], "OutBQty", typeof(float));
            addCol(ds.Tables["RT_VBAP"], "CI", typeof(string));
            addCol(ds.Tables["RT_VBAP"], "ShipmentType", typeof(string));
            addCol(ds.Tables["RT_VBAP"], "ShipmentNo", typeof(string));

            addCol(ds.Tables["RT_CUSTOMER_RQ"], "DEC", typeof(string));
            addCol(ds.Tables["RT_CUSTOMER_RQ"], "FATNAM", typeof(string));

            addCol(ds.Tables["RT_AUSP"], "DEC", typeof(string));
            addCol(ds.Tables["RT_AUSP"], "FATNAM", typeof(string));
            
            addCol(ds.Tables["RT_VBAP"], "CONTAINER_CODE", typeof(string));
            addCol(ds.Tables["RT_VBAP"], "CONTAINER_KEY", typeof(string));
            addCol(ds.Tables["RT_VBAP"], "PALLET", typeof(string));

            foreach (DataRow DR in ds.Tables["RT_VBAP"].Rows)
            {
                DR["Sapqty"] = float.Parse(DR["KWMENG"].ToString());
                DR["OutBQty"] = 0;
                DR["PALLET"] = "";
            }
            
            try
            {
                DataSet dsATINN = new DataSet();    
                serverFactory = CallRemotingService.GetRemoteObject();
                dsATINN = serverFactory.Get<IPickOperationEngine>().GetCRRelation();
                //foreach (DataRow dr in dsATINN.Tables["AWMS_ATINN"].Rows)
                //{

                //    DataRow[] drmatrix = ds.Tables["RT_CUSTOMER_RQ"].Select(" ATINN = '" + dr["ATINN"].ToString() + "'");
                //    foreach ( DataRow dm in drmatrix )
                //    {
                //        dm["ATINN"] = dr["ATNAM"].ToString();
                //        dm["FATNAM"] = dr["FATNAM"].ToString();
                //        dm["DEC"] = dr["DEC"].ToString();
                //    }

                //}
                //foreach  (DataRow dr in ds.Tables["RT_CUSTOMER_RQ"].Rows)
                //{
                //    if (string.IsNullOrEmpty(dr["FATNAM"].ToString()) == true)
                //    {
                //        dr.Delete();
                //    }
                //}
                //ds.Tables["RT_CUSTOMER_RQ"].AcceptChanges();  
                foreach (DataRow dr in dsATINN.Tables["AWMS_ATINN"].Rows)
                {

                    DataRow[] drmatrix = ds.Tables["RT_AUSP"].Select(" ATINN = '" + dr["ATINN"].ToString() + "'");
                    foreach (DataRow dm in drmatrix)
                    {
                        dm["ATINN"] = dr["ATNAM"].ToString();
                        dm["FATNAM"] = dr["FATNAM"].ToString();
                        dm["DEC"] = dr["DEC"].ToString();
                    }

                }
                foreach (DataRow dr in ds.Tables["RT_AUSP"].Rows) //去除无关特性
                {                    
                    if (string.IsNullOrEmpty(dr["FATNAM"].ToString()) == true)
                    {
                        dr.Delete();
                    }
                }
                ds.Tables["RT_AUSP"].AcceptChanges(); 

                //foreach (DataRow dr in ds.Tables["RT_AUSP"].Rows)  //
                //{
                //    if (string.IsNullOrEmpty(dr["ATWRT"].ToString()) == true)
                //    {
                //        dr["ATWRT"] = float.Parse(dr["ATFLV"].ToString()).ToString();                        
                //    }
                //}
                //ds.Tables["RT_AUSP"].AcceptChanges();  
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            ds.Tables["RT_VBAP"].AcceptChanges();
            this.Content.DataSource = ds.Tables["RT_VBAP"];
            this.gridControl1.DataSource = ds.Tables["RT_AUSP"].DefaultView;
            Content_Click(this, EventArgs.Empty);
        }

        private void Content_Click(object sender, EventArgs e)
        {
            
            try
            {
                string s1 = (string)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "VGPOS");
                string s2 = (string)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "WKTPS");
                string s3 = (string)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "AUFNR");
                //ds.Tables["RT_CUSTOMER_RQ"].DefaultView.RowFilter = " ZREQPS='" + s1 + "' AND ATZHL ='"+s2+"'";
                //gridControl1.DataSource = ds.Tables["RT_CUSTOMER_RQ"].DefaultView;
                ds.Tables["RT_AUSP"].DefaultView.RowFilter = " DMANDP='" + s1 + "' AND DMANDC ='" + s2 + "' AND DMANDNO ='"+s3+"'";
                gridControl1.DataSource = ds.Tables["RT_AUSP"].DefaultView;
            }
            catch 
            {
                return;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
                       
            try
            {
                foreach (DataRow dr in ds.Tables["RT_VBAP"].Rows)  //删除出库数量为0的行
                {
                    dr["ERNAM"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                    if (float.Parse(dr["OutBQty"].ToString()) <= 0)
                    {
                        dr.Delete();
                    }
                }
                ds.Tables["RT_VBAP"].AcceptChanges(); 
                if (ds.Tables["RT_VBAP"].Rows.Count <= 0)
                {
                    return;
                }
                if (string.IsNullOrEmpty(edCI.Text) == true)
                {
                    MessageBox.Show(this, "请输入CI！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edCI.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(edShipNO.Text) == true)
                {
                    MessageBox.Show(this, "请输入牌照号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edShipNO.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(cbShipType.Text) == true)
                {
                    MessageBox.Show(this, "请选择运输类型！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbShipType.Focus();
                    return;
                }

                if (ds.Tables["RT_VBAK"].Columns.Contains("CI") == false) //判断是否包含指定列
                {
                    ds.Tables["RT_VBAK"].Columns.Add("CI", typeof(string));
                }

                if (ds.Tables["RT_VBAK"].Columns.Contains("ShipmentType") == false) //判断是否包含指定列
                {
                    ds.Tables["RT_VBAK"].Columns.Add("ShipmentType", typeof(string));
                }
                if (ds.Tables["RT_VBAK"].Columns.Contains("ShipmentNO") == false) //判断是否包含指定列
                {
                    ds.Tables["RT_VBAK"].Columns.Add("ShipmentNO", typeof(string));
                }
                DataRow[] drCheck = dsCabinet.Tables["CabinetData"].Select("IsUsed <> 'Y'");
                if (drCheck.Length > 0)
                {
                    MessageBox.Show(this, "此柜中仍有托盘未刷！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            catch 
            {
                return;
            }

            try
            {
                ds.Tables["RT_VBAK"].Rows[0]["CI"] = edCI.Text;
                ds.Tables["RT_VBAK"].Rows[0]["ShipmentNO"] = edShipNO.Text;
                ds.Tables["RT_VBAK"].Rows[0]["ShipmentType"] = cbShipType.Items.IndexOf(cbShipType.Text);
                ds.Tables["RT_VBAK"].Rows[0]["BNAME"] =  PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);  
                ds.Tables["RT_VBAK"].AcceptChanges();
                edOutBandNO.Text = "" ;
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                edOutBandNO.Text = serverFactory.Get<IPickOperationEngine>().SavePickData(
                    ds,
                    "CK"+PropertyService.Get(PROPERTY_FIELDS.USER_NAME).Substring(0,3).ToUpper()
                    );
                MessageBox.Show("成功保存出库单，单号为："+edOutBandNO.Text);

            }
            catch (Exception ex)
            {                
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            ClearDataset();
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            

            if (string.IsNullOrEmpty(edOutBandNO.Text) == true)
            {
                MessageBox.Show(this, "请输入出库单号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                edOutBandNO.Focus();
                return;
            }
            string SAPVBELN = "";
            string vbeln ="";
            DataTable REQUEST ;

            //-----------------------------------------从数据库中读取已经保存的信息--------------------------------------------
            try   
            {

                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsReturn = serverFactory.Get<IPickOperationEngine>().ImportPickdata(edOutBandNO.Text);
                REQUEST = dsReturn.Tables["REQUEST"].Copy();
                REQUEST.Columns.Add("DOCUMENT_TYPE", typeof(string));
                REQUEST.Columns.Add("SHIP_TO", typeof(string));
                REQUEST.Columns.Add("SOLD_TO", typeof(string));
                if (dsReturn.Tables["CK_CONTAINER"].Rows.Count > 0)
                {
                    MessageBox.Show(this, "不存在此单号或者此订单已经确认，请核对单号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (REQUEST.Rows.Count <= 0)
                {
                    MessageBox.Show(this, "不存在此单号或者此订单已经确认，请核对单号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                for (int i = 0; i < REQUEST.Rows.Count; i++)
                {
                    REQUEST.Rows[i]["DOCUMENT_TYPE"] = "A";
                    REQUEST.Rows[i]["SHIP_TO"] = dsReturn.Tables["HEADER"].Rows[0]["SHIPTO"].ToString();
                    REQUEST.Rows[i]["SOLD_TO"] = dsReturn.Tables["HEADER"].Rows[0]["SALESTO"].ToString();
                }
                vbeln = REQUEST.Rows[0]["DOCUMENT_NUMB"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            //--------------------------------------------------------------------------------------------------------------


            //-----------------------------------------同步SAP生成外向交货单------------------------------------------------
            try
            {
                string rfcFuntionName = "ZSOLAR_OUTBAND_DELIVERY";//BAPI_DELIVERYPROCESSING_EXEC
                IServerObjFactory sapserverFactory = CallRemotingService.GetRemoteObject();          
            
                
                if (null != sapserverFactory)
                {
                    IRFCEngine rfcCallObj = sapserverFactory.Get<IRFCEngine>();
                    DataSet dsParams = new DataSet();
                    dsParams.Tables.Add(REQUEST);
                    dsParams.Tables[0].TableName = "REQUEST";
                    //dsParams.ExtendedProperties.Add("WK_VBELN", this.TBVbeln.Text);
                    ds = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
                    SAPVBELN = ds.Tables["CREATEDITEMS"].Rows[0]["DOCUMENT_NUMB"].ToString();

                }
                if (string.IsNullOrEmpty(SAPVBELN))
                {
                    MessageBox.Show(this, "同步SAP失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            //--------------------------------------------------------------------------------------------------------------

            //-------------------------------------------保存状态-----------------------------------------------------------

            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                serverFactory.Get<IPickOperationEngine>().UpdateSapVbeln(SAPVBELN, edOutBandNO.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            //--------------------------------------------------------------------------------------------------------------
            MessageBox.Show("成功生成外向交货单:" + SAPVBELN);            

            //---------------------------------------更新拣配数量-------------------------------------------
            //System.Threading.Thread.Sleep(1000); 
            //DataTable VBKOK_WA = new DataTable();
            //DataTable VBPOK_TAB = new DataTable();
            //VBKOK_WA.Columns.Add("VBELN_VL",typeof(string));
            //VBKOK_WA.Columns.Add("VBELN", typeof(string));
            //DataRow dr = VBKOK_WA.NewRow();
            //dr["VBELN_VL"] = vbeln ;
            //dr["VBELN"] = SAPVBELN;
            //VBKOK_WA.Rows.Add(dr);

            //VBPOK_TAB.Columns.Add("VBELN_VL", typeof(string));
            //VBPOK_TAB.Columns.Add("VBELN", typeof(string));
            //VBPOK_TAB.Columns.Add("POSNR_VL", typeof(string));
            //VBPOK_TAB.Columns.Add("POSNN", typeof(string));
            //VBPOK_TAB.Columns.Add("VBTYP_N", typeof(string));
            //VBPOK_TAB.Columns.Add("PIKMG", typeof(string));
            //VBPOK_TAB.Columns.Add("MEINS", typeof(string));
            //VBPOK_TAB.Columns.Add("NDIFM", typeof(string));
            //VBPOK_TAB.Columns.Add("TAQUI", typeof(string));
            //VBPOK_TAB.Columns.Add("CHARG", typeof(string));
            //VBPOK_TAB.Columns.Add("ORPOS", typeof(string));
            //for (int i = 0; i < REQUEST.Rows.Count; i++)
            //{
            //    DataRow drV = VBPOK_TAB.NewRow();
            //    drV["VBELN_VL"] = REQUEST.Rows[i]["DOCUMENT_NUMB"];
            //    drV["POSNR_VL"] = REQUEST.Rows[i]["DOCUMENT_ITEM"];
            //    drV["VBELN"] = SAPVBELN;
            //    drV["POSNN"] = REQUEST.Rows[i]["POSNR"];
            //    drV["VBTYP_N"] = "Q";
            //    drV["PIKMG"] = REQUEST.Rows[i]["QUANTITY_SALES_UOM"];
            //    drV["MEINS"] = REQUEST.Rows[i]["SALES_UNIT"];                
            //    drV["NDIFM"] = "0";
            //    drV["TAQUI"] = "";
            //    //dr["CHARG"] = REQUEST.Rows[i]["BATCH"];
            //    drV["ORPOS"] = "0";
            //    VBPOK_TAB.Rows.Add(drV);
            //}
            //rfcFuntionName = "WS_DELIVERY_UPDATE";
            //sapserverFactory = CallRemotingService.GetRemoteObject();           
            //if (null != sapserverFactory)
            //{
            //    IRFCEngine rfcCallObj = sapserverFactory.Get<IRFCEngine>();
            //    DataSet dsParams = new DataSet();
            //    dsParams.Tables.Add(VBKOK_WA);
            //    dsParams.Tables.Add(VBPOK_TAB);
            //    dsParams.Tables[0].TableName = "VBKOK_WA";
            //    dsParams.Tables[0].TableName = "VBPOK_TAB";
            //    dsParams.ExtendedProperties.Add("UPDATE_PICKING", "X");
            //    dsParams.ExtendedProperties.Add("DELIVERY", SAPVBELN); 
            //    ds = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);  
            //}
        }

        private bool CheckInput(out string msg)
        {
            if (string.IsNullOrEmpty(edCabinetNO.Text) == true)
            {
                //MessageBox.Show(this, "请输入柜号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                msg = "请输入柜号!";
                return false;
            }

            if (string.IsNullOrEmpty(edPalletNo.Text) == true)
            {
                //MessageBox.Show(this, "请输入托盘号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                msg = "请输入托盘号!";
                return false;
            }
            if (ds == null)
            {
                msg = "请先查询数据!";
                return false;
            }

            if (ds.Tables.Count <= 0)
            {
                msg = "请先查询数据!";
                return false;
            }
            if (ds.Tables.Contains("RT_VBAP")==false)
            {
                msg = "请先查询数据!";
                return false;
            }

            if (ds.Tables["RT_VBAP"].Rows.Count <= 0)
            {
                //MessageBox.Show(this, "请先查询数据！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                msg = "请先查询数据!";
                return false;
            }

            if (ds.Tables.Contains("RT_AUSP") == false)
            {
                msg = "没有特性信息，请重新查询后重试！";
                return false;
            }


            if (ds.Tables["RT_CUSTOMER_RQ"].Rows.Count <= 0)
            {
                //MessageBox.Show(this, "没有特性信息，请重新查询后重试！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                msg = "没有特性信息，请重新查询后重试！";
                return false;
            }
            msg = "";
            return true;
        }

        private bool addCol(DataTable tb, string FieldName, Type type)
        {
            try
            {
                if (tb.Columns.Contains(FieldName) == false) tb.Columns.Add(FieldName, type);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void edCabinetNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar != 13)  return;            
            //if ( CheckInput() ==false )  return ;

            //addCol(ds.Tables["RT_VBAP"], "CONTAINER_CODE", typeof(string));
            //addCol(ds.Tables["RT_VBAP"], "CONTAINER_KEY", typeof(string));
            //addCol(ds.Tables["RT_VBAP"], "CONTAINER_KEY", typeof(string));
            //DataSet dsReturn = new DataSet();
            //try
            //{
            //    IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            //    dsReturn = serverFactory.Get<IPickOperationEngine>().GetCabinetData(edCabinetNO.Text);
            //    if (dsReturn.Tables["CabinetData"].Rows.Count <= 0)
            //    {
            //        MessageBox.Show(this, "柜号有误或者此柜已经出货，请核对！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //finally
            //{
            //    CallRemotingService.UnregisterChannel();
            //}
            //addCol(dsReturn.Tables["CabinetData"], "IsUsed", typeof(string));
                     
            //DataTable TMP = ds.Tables["RT_VBAP"];
            //DataRow[] nPick = ds.Tables["RT_VBAP"].Select(" OutBQty <= 0 ");//and '
            //foreach (DataRow pick in nPick)
            //{
            //    string s1 =  pick["VGPOS"].ToString();
            //    string s2 = pick["GRKOR"].ToString();
            //    DataRow[] nReq = ds.Tables["RT_CUSTOMER_RQ"].Select(" ZREQPS='" + s1 + "' AND ATZHL ='"+s2+"'");  //获取所有可以出库的行
            //    DataRow CompareRow =  dsReturn.Tables["CabinetData"].NewRow();// 生成入库单特性值新行
               
            //    foreach (DataRow req in nReq) 
            //    {
            //        CompareRow[req["FATNAM"].ToString()] = req["ATWRT"]; //对新生成的行中，填入销售单明细项所对应的特性值 
            //    }
            //    foreach (DataRow nCabinet in dsReturn.Tables["CabinetData"].Rows)
            //    {
            //        if (nCabinet["IsUsed"].ToString() == "Y") 
            //        {
            //            continue;
            //        }
            //        CompareRow["CONTAINER_CODE"] = nCabinet["CONTAINER_CODE"];
            //        CompareRow["CONTAINER_KEY"] = nCabinet["CONTAINER_KEY"];
            //        CompareRow["MATNR"] = nCabinet["MATNR"];
            //        CompareRow["MENGE"] = nCabinet["MENGE"];
            //        CompareRow["CHARG"] = nCabinet["CHARG"];
            //        bool isEqual = true;
            //        for (int i = 0; i < dsReturn.Tables["CabinetData"].Columns.Count; i++)
            //        {
            //            if( CompareRow[i].ToString()  !=  nCabinet[i].ToString()) 
            //            {
            //                isEqual = false;
            //            }
            //        }

            //        if (isEqual == true) 
            //        {
            //            float sQty = float.Parse(pick["KWMENG"].ToString());
            //            float oQty = float.Parse(pick["OutBQty"].ToString());
            //            float mQty = float.Parse(nCabinet["MENGE"].ToString());
            //            if ((sQty - oQty) > mQty)  //若匹配成功   
            //            {
            //                nCabinet["IsUsed"] = "Y";
            //                pick["CONTAINER_CODE"] = nCabinet["CONTAINER_CODE"].ToString();
            //                pick["CONTAINER_KEY"] = nCabinet["CONTAINER_KEY"].ToString();
            //                pick["OutBQty"] = float.Parse(nCabinet["MENGE"].ToString()); //出库数量增加
            //                pick["CHARG"] = nCabinet["CHARG"].ToString();
            //                break;
            //            }
            //        }

            //    }
            //}
            //dsReturn.Tables["CabinetData"].AcceptChanges();

            //DataRow[] isu = dsReturn.Tables["CabinetData"].Select("IsUsed <>'Y'");

            //if (isu.Length>0)  //若有柜中的托没有刷进去
            //{
            //    ds.Tables["RT_VBAP"].RejectChanges();
            //    MessageBox.Show(this,"柜中含有非本订单的数据，请核对柜号或者重组柜后再试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //ds.Tables["RT_VBAP"].AcceptChanges(); //保存数据


        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (ds.Tables["RT_VBAP"].Rows.Count <= 0)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            try
            {
                ds.Tables["RT_VBAP"].Rows[gridView1.FocusedRowHandle].Delete();
                ds.Tables["RT_VBAP"].AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool PalletOperation(DataSet ds, DataSet dSCabinet,string CabinetNO, string PalletNO,out string msg)
        {

            DataRow[] drselect = dsCabinet.Tables["CabinetData"].Select("XP004='" + PalletNO + "' AND  IsUsed ='N'"); //获取柜中所有此托号的信息
            foreach (DataRow drCabinet in drselect)
            {
                DataRow[] dsSelect = ds.Tables["RT_VBAP"].Select(" MATNR = '" + drCabinet["MATNR"].ToString() + "'"   //获取所有未刷托 物料号相同的行项目
                                                           + " AND PALLET =''"
                                                           + " AND Sapqty >= " + drCabinet["MENGE"].ToString());
                if (dsSelect.Length <= 0)
                {
                    continue;
                } 
                foreach (DataRow drSelect in dsSelect)  // dsSelect 所有未刷托 物料号相同的行项目
                {
                    string s1 = drSelect["VGPOS"].ToString();
                    string s2 = drSelect["WKTPS"].ToString();
                    string s3 = drSelect["AUFNR"].ToString();
                    DataRow[] dsAusp = ds.Tables["RT_AUSP"].Select(" DMANDPS='" + s1 + "' AND DMANDCT ='" + s2 + "' AND DMANDNO ='" + s3 + "'");
                    if (dsAusp.Length <= 0)
                    {
                        continue;
                    }
                    bool succ = true;
                    foreach (DataRow drAusp in dsAusp)  //匹配特性信息
                    {
                        if (drCabinet[drAusp["FATNAM"].ToString()].ToString() != drAusp["ATWRT"].ToString())
                        {
                            succ = false;
                            break;
                        }
                    }
                    if (succ == true)
                    {
                        drSelect["OutBQty"] = drCabinet["MENGE"].ToString();
                        drSelect["CONTAINER_CODE"] = drCabinet["CONTAINER_CODE"].ToString();
                        drSelect["CONTAINER_KEY"] = drCabinet["CONTAINER_KEY"].ToString();
                        drSelect["PALLET"] = PalletNO;
                        drCabinet["IsUsed"] = "Y";
                        ds.Tables["RT_VBAP"].AcceptChanges();
                        dsCabinet.AcceptChanges();
                        msg = "";
                        return true;
                    }
                }

            }

            msg = "此托盘号无法匹配出库项目，请检查！";
            return false;
            //foreach (DataRow drCabinet in drselect)
            //{
            //    DataRow[] dsSelect = ds.Tables["RT_VBAP"].Select("CHARG = '" + drCabinet["CHARG"].ToString() + "'"
            //                                               + " AND MATNR = '" + drCabinet["MATNR"].ToString() + "'"
            //                                               + " AND PALLET =''"
            //                                               + " AND Sapqty >= " + drCabinet["MENGE"].ToString()
            //                                              );
            //    if (dsSelect.Length <= 0)
            //    {
            //        continue;
            //    }
            //    dsSelect[0]["OutBQty"] = drCabinet["MENGE"].ToString();
            //    dsSelect[0]["CONTAINER_CODE"] = drCabinet["CONTAINER_CODE"].ToString();
            //    dsSelect[0]["CONTAINER_KEY"] = drCabinet["CONTAINER_KEY"].ToString();
            //    dsSelect[0]["PALLET"] = PalletNO;
            //    drCabinet["IsUsed"] = "Y";
            //    ds.Tables["RT_VBAP"].AcceptChanges();
            //    dsCabinet.AcceptChanges();
            //    msg = "";
            //    return true;
            //}
            //msg = "此托盘号无法匹配出库项目，请检查！";
            //return false;
        }

        private bool ClearCabinetInfo()
        {
            try
            {
                dsCabinet.Clear();
                foreach (DataRow dr in ds.Tables["RT_VBAP"].Rows)
                {
                    dr["OutBQty"] = 0;
                    dr["CONTAINER_CODE"] = "";
                    dr["CONTAINER_KEY"] = "";
                }
                ds.Tables["RT_VBAP"].AcceptChanges();
            }
            catch
            {
                return false;
            };
            return true;
            
        }
        private void edPalletNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string msg = string.Empty;
            if (e.KeyChar != 13) return;
            if (CheckInput(out msg) == false)
            {
                MessageBox.Show(this, msg, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                return;
            }
            if (string.IsNullOrEmpty(edCabinetNO.Text) == true)
            {
                MessageBox.Show("请输入柜号！");
                edCabinetNO.Focus();
                return;
            }

            if (dsCabinet==null)
            {
                MessageBox.Show(this, "不存在柜信息，请重新输入柜信息！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dsCabinet.Tables.Count<=0)
            {
                MessageBox.Show(this, "不存在柜信息，请重新输入柜信息！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (dsCabinet.Tables.Contains("CabinetData") == false)
            {
                MessageBox.Show(this, "不存在柜信息，请重新输入柜信息！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dsCabinet.Tables["CabinetData"].Rows.Count <= 0)
            {
                MessageBox.Show(this, "不存在柜信息，请重新输入柜信息！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow[] CheckCabinet = dsCabinet.Tables["CabinetData"].Select("CONTAINER_CODE <> '" + edCabinetNO.Text + "'");
            if (CheckCabinet.Length > 0)
            {
                if (MessageBox.Show(this, "柜号与您上次所输入的柜号不符，是否重新刷柜？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes)
                {
                    if (MessageBox.Show(this, "重新刷柜将清除所有刷柜信息，请再次确认！", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                       == DialogResult.OK)
                    {
                        ClearCabinetInfo();
                        edCabinetNO_Leave(this, EventArgs.Empty); //读入柜信息
                    }
                }

            }

            
            if (PalletOperation(ds, dsCabinet,edCabinetNO.Text,edPalletNo.Text, out msg) == false)
            {
                MessageBox.Show(this, msg, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            };
            
        }

        private void edCabinetNO_Leave(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(edCabinetNO.Text) == true) return;
            string msg = string.Empty;
            try
            {
                if (dsCabinet != null)
                {
                    if (dsCabinet.Tables.Count > 0)
                    {
                        if (dsCabinet.Tables.Contains("CabinetData") == true)
                        {
                            DataRow[] CheckCabinet = dsCabinet.Tables["CabinetData"].Select("CONTAINER_CODE <> '" + edCabinetNO.Text + "'");
                            if (CheckCabinet.Length > 0)
                            {
                                if (MessageBox.Show(this, "柜号与您上次所输入的柜号不符，是否重新刷柜？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == DialogResult.Yes)
                                {
                                    if (MessageBox.Show(this, "重新刷柜将清除所有刷柜信息，请再次确认！", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                                       == DialogResult.OK)
                                    {
                                        if (ClearCabinetInfo() == false) return;

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                dsCabinet = serverFactory.Get<IPickOperationEngine>().GetCabinetData(edCabinetNO.Text,out msg);
                if (msg != "")
                {
                    MessageBox.Show(this, msg, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ;
                }
            }
            catch (Exception ex)
            {               
                MessageBox.Show(this, ex.Message, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

        }

    }
}


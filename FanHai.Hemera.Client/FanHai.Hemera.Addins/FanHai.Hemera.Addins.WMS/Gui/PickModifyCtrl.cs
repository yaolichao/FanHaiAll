using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;
using FanHai.Hemera.Share.Interface.RFC;


namespace FanHai.Hemera.Addins.WMS.Gui
{
    public partial class PickModifyCtrl : BaseUserCtrl
    {
        public PickModifyCtrl()
        {
            InitializeComponent();
        }

        DataSet ds;
        DataSet dsReturn;
        DataSet dsAdd;
        private void btQuery_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(edOutBandNO.Text) == true) && ( string.IsNullOrEmpty(edSapvbeln.Text)))
            {
                MessageBox.Show("请输入查询单号！");
                edOutBandNO.Focus();
                return;
            }
            try
            {
                dsReturn.Clear();
                ds.Clear();
                dsAdd.Clear();
                dsReturn.AcceptChanges();
                ds.AcceptChanges();               
                dsAdd.AcceptChanges();
            }
            catch
            {  };

            string SalesNO = "";
            
            
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            dsReturn = serverFactory.Get<IPickOperationEngine>().GetPickedInfo(edOutBandNO.Text, edSapvbeln.Text);
            if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
            {
                MessageBox.Show("没有你要查询的单据，请核对后重试！");
                return;
            }
            SalesNO = dsReturn.Tables["OUTB_TAB"].Rows[0]["REF_NO"].ToString();

            //string rfcFuntionName = "ZAST_GET_SO";
            //serverFactory = CallRemotingService.GetRemoteObject();
            //if (null != serverFactory)
            //{
            //    IRFCEngine rfcCallObj = serverFactory.Get<IRFCEngine>();
            //    DataSet dsParams = new DataSet();
            //    dsParams.ExtendedProperties.Add("WK_VBELN", SalesNO);
            //    ds = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
            //    ds.Tables["RT_CUSTOMER_RQ"].Columns.Add("DEC", typeof(string));
            //    ds.Tables["RT_CUSTOMER_RQ"].Columns.Add("FATNAM", typeof(string));
            //}

            //// --------------------------------去除无关特性-------------------------------------------------------------
            //try
            //{
            //    DataSet dsATINN = new DataSet();
            //    serverFactory = CallRemotingService.GetRemoteObject();
            //    dsATINN = serverFactory.Get<IPickOperationEngine>().GetCRRelation();
            //    foreach (DataRow dr in dsATINN.Tables["AWMS_ATINN"].Rows)
            //    {

            //        DataRow[] drmatrix = ds.Tables["RT_CUSTOMER_RQ"].Select(" ATINN = '" + dr["ATINN"].ToString() + "'");
            //        foreach (DataRow dm in drmatrix)
            //        {
            //            dm["ATINN"] = dr["ATNAM"].ToString();
            //            dm["FATNAM"] = dr["FATNAM"].ToString();
            //            dm["DEC"] = dr["DEC"].ToString();
            //        }

            //    }
            //    foreach (DataRow dr in ds.Tables["RT_CUSTOMER_RQ"].Rows)
            //    {
            //        if (string.IsNullOrEmpty(dr["FATNAM"].ToString()) == true)
            //        {
            //            dr.Delete();
            //        }
            //    }
            //    ds.Tables["RT_CUSTOMER_RQ"].AcceptChanges();
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
            Content.DataSource = dsReturn.Tables["OUTB_TAB"];
            //gridControl1.DataSource = ds.Tables["RT_CUSTOMER_RQ"].DefaultView;
            Content_Click(this, EventArgs.Empty);

        }

        private void Content_Click(object sender, EventArgs e)
        {
             
            //try
            //{
            //    string s1 = (string)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DemardPOS");
            //    string s2 = (string)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DemardSch");
            //    //s1 = int.Parse(s1).ToString();
            //    //s2 = int.Parse(s2).ToString();
            //    DataTable tmp = ds.Tables["RT_CUSTOMER_RQ"].Copy();
            //    ds.Tables["RT_CUSTOMER_RQ"].DefaultView.RowFilter = " ZREQPS='" + s1 + "' AND ATZHL ='" + s2 + "'";
            //    gridControl1.DataSource = ds.Tables["RT_CUSTOMER_RQ"].DefaultView;
            //}
            //catch
            //{
            //    return;
            //}
                
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            string t1 = "";
            try
            {
                if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
                {
                    return;
                }
             }
             catch
            {
                 return;
            }
            string Sapvbeln = dsReturn.Tables["OUTB_TAB"].Rows[0]["VBELN"].ToString();

            DataSet dsBAPI = new DataSet();
            string rfcFuntionName = "ZDZSW_XSDD";
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
            if (null != serverFactory)
            {
                IRFCEngine rfcCallObj = serverFactory.Get<IRFCEngine>();
                DataSet dsParams = new DataSet();
                dsParams.ExtendedProperties.Add("BZ", "WD");
                dsParams.ExtendedProperties.Add("WK_VBELN", Sapvbeln);
                dsBAPI = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
                bool isSuccess = true;
                string errstr = "";
                for (int i = 0; i < dsBAPI.Tables["LI_RETURN"].Rows.Count; i++)
                {
                    if (dsBAPI.Tables["LI_RETURN"].Rows[i]["TYPE"].ToString() == "E")
                    {
                        errstr += dsBAPI.Tables["LI_RETURN"].Rows[i]["MESSAGE"].ToString() + 13;
                        isSuccess = false;
                    }
                }
                if (isSuccess == false)
                {
                    MessageBox.Show(this, errstr, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }  
            try
            {
                serverFactory = CallRemotingService.GetRemoteObject();
                serverFactory.Get<IPickOperationEngine>().DeleteOutBand(Sapvbeln, 1);
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
            try
            {
                dsReturn.Clear();
                ds.Clear();
                dsReturn.AcceptChanges();
                ds.AcceptChanges();
            }
            catch
            { };

            MessageBox.Show(this, "删除成功！" ,"操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btCConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
                {
                    return;
                }
             }
             catch
            {
                 return;
            }
            string OutNO = dsReturn.Tables["OUTB_TAB"].Rows[0]["OUTBANDNO"].ToString();
            if (string.IsNullOrEmpty(OutNO) == true)
            {
                MessageBox.Show(this, "无法获取相关出库单信息", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataSet carinfo = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                carinfo = serverFactory.Get<IPickOperationEngine>().GetCarInfo(OutNO);
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


            PickCarInfoWinFM Pfm = new PickCarInfoWinFM();

            Pfm.edSapvbeln.Text = dsReturn.Tables["OUTB_TAB"].Rows[0]["VBELN"].ToString();
            Pfm.edOutBandNO.Text = OutNO;
            Pfm.edCI.Text = carinfo.Tables["CarInfo"].Rows[0]["CI"].ToString();            
            //Pfm.cbShipType.Text = dsReturn.Tables["OUTB_TAB"].Rows[0]["ShipmentType"].ToString();
            Pfm.cbShipType.SelectedIndex = int.Parse(carinfo.Tables["CarInfo"].Rows[0]["ShipmentType"].ToString());
            Pfm.edShipNO.Text = carinfo.Tables["CarInfo"].Rows[0]["ShipmentNO"].ToString();
            Pfm.ShowDialog();

        }

        private void btADD_Click(object sender, EventArgs e)
        {

            try
            {
                if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            string Sapvbeln = dsReturn.Tables["OUTB_TAB"].Rows[0]["REF_NO"].ToString();
            if (string.IsNullOrEmpty(Sapvbeln) == true)
            {
                MessageBox.Show(this, "无法获取相关销售订单信息", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string rfcFuntionName = "ZAST_GET_SO";
            IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

            
            if (null != serverFactory)
            {
                IRFCEngine rfcCallObj = serverFactory.Get<IRFCEngine>();
                DataSet dsParams = new DataSet();
                dsParams.ExtendedProperties.Add("WK_VBELN", Sapvbeln);
                dsAdd = rfcCallObj.ExecuteRFC(rfcFuntionName, dsParams);
            }
            if (dsAdd.Tables["RT_VBAP"].Rows.Count <= 0)
            {
                MessageBox.Show(this, "没有可添加的数据！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dsAdd.Tables["RT_VBAP"].Columns.Add("Sapqty", typeof(string));
            dsAdd.Tables["RT_VBAP"].Columns.Add("OutBQty", typeof(float));
            dsAdd.Tables["RT_VBAP"].Columns.Add("CI", typeof(string));
            dsAdd.Tables["RT_VBAP"].Columns.Add("ShipmentType", typeof(string));
            dsAdd.Tables["RT_VBAP"].Columns.Add("ShipmentNo", typeof(string));
            dsAdd.Tables["RT_CUSTOMER_RQ"].Columns.Add("DEC", typeof(string));
            dsAdd.Tables["RT_CUSTOMER_RQ"].Columns.Add("FATNAM", typeof(string));
            if (dsAdd.Tables["RT_VBAP"].Columns.Contains("CONTAINER_CODE") == false) //判断是否包含指定列
            {
                dsAdd.Tables["RT_VBAP"].Columns.Add("CONTAINER_CODE", typeof(string));
            }

            if (dsAdd.Tables["RT_VBAP"].Columns.Contains("CONTAINER_KEY") == false) //判断是否包含指定列
            {
                dsAdd.Tables["RT_VBAP"].Columns.Add("CONTAINER_KEY", typeof(string));
            }
            for (int i = 0; i < ds.Tables["RT_VBAP"].Rows.Count; i++)
            {
                dsAdd.Tables["RT_VBAP"].Rows[i]["Sapqty"] = dsAdd.Tables["RT_VBAP"].Rows[i]["KWMENG"];
                dsAdd.Tables["RT_VBAP"].Rows[i]["OutBQty"] = 0;
            } 
            PMAddProd pmadd = new PMAddProd(dsAdd);
            pmadd.ShowDialog();
            if (dsAdd.Tables.IndexOf("SelectedRow") < 0) //若没有选中或者不存在数据，返回
            {
                return;
            }

            if (dsAdd.Tables["SelectedRow"].Rows.Count <= 0)
            {
                MessageBox.Show(this, "未选中任何行", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < dsAdd.Tables["SelectedRow"].Rows.Count; i++)
            {
                DataRow[] drM = dsReturn.Tables["OUTB_TAB"].Select(" REF_NO = '" + dsAdd.Tables["SelectedRow"].Rows[i]["VBELN"]
                                                + "' and REF_ITEM = '" + dsAdd.Tables["SelectedRow"].Rows[i]["POSNR"] + "'");

                if (drM.Length<=0)  //若已存在此行，则不添加,否则添加
                {
                    DataRow Newrow = dsReturn.Tables["OUTB_TAB"].NewRow();
                    
                    Newrow["PLANT"] = dsAdd.Tables["SelectedRow"].Rows[i]["WERKS"].ToString();
                    Newrow["STORAGELOCATION"] = dsAdd.Tables["SelectedRow"].Rows[i]["LGORT"].ToString();
                    Newrow["CPBH"] = dsAdd.Tables["SelectedRow"].Rows[i]["matnr"].ToString();
                    Newrow["XHGG"] = dsAdd.Tables["SelectedRow"].Rows[i]["ARKTX"].ToString();
                    Newrow["OBDQTY"] = 0;
                    Newrow["UNIT"] = dsAdd.Tables["SelectedRow"].Rows[i]["VRKME"].ToString();
                    Newrow["VSTEL"] = dsAdd.Tables["SelectedRow"].Rows[i]["VSTEL"].ToString();
                    Newrow["REF_NO"] = dsAdd.Tables["SelectedRow"].Rows[i]["VBELN"].ToString();
                    Newrow["REF_ITEM"] = dsAdd.Tables["SelectedRow"].Rows[i]["POSNR"].ToString();
                    Newrow["LAST_CNG_BY"] = PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ);
                    Newrow["DemandNO"] = dsAdd.Tables["SelectedRow"].Rows[i]["AUFNR"].ToString();
                    Newrow["DemardPOS"] = dsAdd.Tables["SelectedRow"].Rows[i]["VGPOS"].ToString();
                    Newrow["DemardSch"] = dsAdd.Tables["SelectedRow"].Rows[i]["GRKOR"].ToString();
                    dsReturn.Tables["OUTB_TAB"].Rows.Add(Newrow);
                }
                dsReturn.Tables["OUTB_TAB"].AcceptChanges();
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
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
                dsReturn.Tables["OUTB_TAB"].Rows[gridView1.FocusedRowHandle].Delete();
                dsReturn.Tables["OUTB_TAB"].AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(edCabinetNO.Text) == true)
            {
                MessageBox.Show(this, "请输入柜号！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (dsReturn.Tables["OUTB_TAB"].Rows.Count <= 0)
            {
                MessageBox.Show(this, "请先查询数据！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            try
            {
                if (dsAdd.Tables["RT_CUSTOMER_RQ"].Rows.Count <= 0)
                {
                    MessageBox.Show(this, "没有特性信息，请重新查询后重试！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show(this, "没有特性信息，请重新查询后重试！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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
            if (e.KeyChar != 13) return;
            if (CheckInput() == false) return;

            addCol(dsReturn.Tables["OUTB_TAB"], "CONTAINER_CODE", typeof(string));
            addCol(dsReturn.Tables["OUTB_TAB"], "CONTAINER_KEY", typeof(string));
            DataTable TMP = ds.Tables["RT_VBAP"];
            DataRow[] nPick = ds.Tables["RT_VBAP"].Select(" OutBQty <= 0 ");//and '
            DataSet dsCabinet = new DataSet();
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                //dsCabinet = serverFactory.Get<IPickOperationEngine>().GetCabinetData(edCabinetNO.Text);
                if (dsCabinet.Tables["CabinetData"].Rows.Count <= 0)
                {
                    MessageBox.Show(this, "柜号有误或者此柜已经出货，请核对！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            addCol(dsCabinet.Tables["CabinetData"], "IsUsed", typeof(string));

            foreach (DataRow pick in nPick)
            {
                string s1 = pick["VGPOS"].ToString();
                string s2 = pick["GRKOR"].ToString();
                DataRow[] nReq = dsAdd.Tables["RT_CUSTOMER_RQ"].Select(" ZREQPS='" + s1 + "' AND ATZHL ='" + s2 + "'");  //获取所有可以出库的行
                DataRow CompareRow = dsCabinet.Tables["CabinetData"].NewRow();// 生成入库单特性值新行

                foreach (DataRow req in nReq)
                {
                    CompareRow[req["FATNAM"].ToString()] = req["ATWRT"]; //对新生成的行中，填入销售单明细项所对应的特性值 
                }
                foreach (DataRow nCabinet in dsCabinet.Tables["CabinetData"].Rows)
                {
                    if (nCabinet["IsUsed"].ToString() == "Y")
                    {
                        continue;
                    }
                    CompareRow["CONTAINER_CODE"] = nCabinet["CONTAINER_CODE"];
                    CompareRow["CONTAINER_KEY"] = nCabinet["CONTAINER_KEY"];
                    CompareRow["MATNR"] = nCabinet["MATNR"];
                    CompareRow["MENGE"] = nCabinet["MENGE"];
                    CompareRow["CHARG"] = nCabinet["CHARG"];
                    bool isEqual = true;
                    for (int i = 0; i < dsReturn.Tables["CabinetData"].Columns.Count; i++)
                    {
                        if (CompareRow[i].ToString() != nCabinet[i].ToString())
                        {
                            isEqual = false;
                        }
                    }

                    if (isEqual == true)
                    {
                        //float sQty = float.Parse(pick["KWMENG"].ToString());
                        //float oQty = float.Parse(pick["OutBQty"].ToString());
                        //float mQty = float.Parse(nCabinet["MENGE"].ToString());
                        //if ((sQty - oQty) > mQty)  //若匹配成功   
                        //{
                            nCabinet["IsUsed"] = "Y";
                            pick["CONTAINER_CODE"] = nCabinet["CONTAINER_CODE"].ToString();
                            pick["CONTAINER_KEY"] = nCabinet["CONTAINER_KEY"].ToString();
                            pick["OutBQty"] = float.Parse(nCabinet["MENGE"].ToString()); //出库数量增加
                            pick["CHARG"] = nCabinet["CHARG"].ToString();
                            break;
                       // }
                    }

                }
            }

            DataRow[] isu = dsCabinet.Tables["CabinetData"].Select("IsUsed <>'Y'");

            if (isu.Length > 0)  //若有柜中的托没有刷进去
            {
                dsReturn.Tables["OUTB_TAB"].RejectChanges();
                MessageBox.Show(this, "柜中含有非本订单的数据，请核对柜号或者重组柜后再试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dsReturn.Tables["OUTB_TAB"].AcceptChanges(); //保存数据










        }

        private void btSave_Click(object sender, EventArgs e)
        {

        }

        
    }
}

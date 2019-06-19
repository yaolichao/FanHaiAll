using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SAPLogonCtrl;
using SAPFunctionsOCX;
using SAPTableFactoryCtrl;
using SolarViewer.Hemera.Share.Interface;
using SolarViewer.Hemera.Share.Constants;
using SolarViewer.Hemera.Share.Common;
using SolarViewer.Gui.Core;




namespace SolarViewer.Hemera.Addins.WMS.Gui
{
    public partial class SapConfig
    {
        public string ApplicationServer;
        public string Client;
        public string Language ;
        public string User ;
        public string Password ;
        public int SystemNumber; 
        public SapConfig()
        {
            ApplicationServer = "10.20.30.107";
            Client = "400";
            Language = "ZH";
            User = "HUGANG";
            Password = "90137218";
            SystemNumber = 0;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
        
    }
    public partial class PickOrderCtrl 
    {
        DataSet ds;
        public DataTable ReqTab ;
        public DataTable table ;
        private SapConfig sapconfig;
        
        public PickOrderCtrl()
        {
            InitializeComponent();
            sapconfig = new SapConfig();
            
        }
        

       

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            SAPLogonControlClass login = new SAPLogonControlClass();
            login.ApplicationServer = sapconfig.ApplicationServer;
            login.Client = sapconfig.Client;
            login.Language = sapconfig.Language;
            login.User = sapconfig.User;
            login.Password = sapconfig.Password;
            login.SystemNumber = sapconfig.SystemNumber;
            Connection conn = (Connection)login.NewConnection();
            ds = new DataSet();
            table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("VBELN", typeof(string));
            table.Columns.Add("POSNR", typeof(string));
            table.Columns.Add("KWMENG", typeof(string));
            table.Columns.Add("Sapqty", typeof(string));
            table.Columns.Add("WERKS", typeof(string));
            table.Columns.Add("VRKME", typeof(string));
            table.Columns.Add("LGORT", typeof(string));
            table.Columns.Add("VSTEL", typeof(string));
            table.Columns.Add("AUFNR", typeof(string));
            table.Columns.Add("VGPOS", typeof(string));
            table.Columns.Add("MATNR", typeof(string));
            table.Columns.Add("ARKTX", typeof(string));
            table.Columns.Add("OutBQty", typeof(string));
            table.Columns.Add("CHARG", typeof(string)); 
            ReqTab = new DataTable();
            ReqTab.Columns.Add("ID", typeof(string));
            ReqTab.Columns.Add("ZRQWK", typeof(string));
            ReqTab.Columns.Add("ZREQNO", typeof(string));
            ReqTab.Columns.Add("ZREQPS", typeof(string));
            ReqTab.Columns.Add("ATINN", typeof(string));
            ReqTab.Columns.Add("ATZHL", typeof(string));
            ReqTab.Columns.Add("ATWRT", typeof(string));
            if (conn.Logon(0, true))
            {
                SAPFunctionsClass func = new SAPFunctionsClass();
                func.Connection = conn;
                IFunction ifunc = (IFunction)func.Add("ZAST_GET_SO");
                IParameter gclient = (IParameter)ifunc.get_Exports("WK_VBELN");
                gclient.Value = TBVbeln.Text;
                ifunc.Call();
                Tables tables = (Tables)ifunc.Tables;
                Table vbap = (Table)tables.get_Item("RT_VBAP");
                Table rels = (Table)tables.get_Item("RT_CUSTOMER_RQ");
                Table RET = (Table)tables.get_Item("RETURN");                
                for (int i = 1; i <= RET.RowCount; i++)
                {
                    if (((string)RET.get_Cell(i,"TYPE") == "E") || ((string)RET.get_Cell(i,"TYPE") == "A"))
                    {
                        MessageBox.Show((string)RET.get_Cell(i,"MESSAGE"));
                        return;
                    }
                }
                for (int i = 1; i <= vbap.RowCount; i++)
                {
                    DataRow dr = table.NewRow();
                    dr["ID"] = i.ToString();
                    dr["VBELN"] = vbap.get_Cell(i, "VBELN");
                    dr["POSNR"] = vbap.get_Cell(i, "POSNR");
                    dr["KWMENG"] = vbap.get_Cell(i, "KWMENG");
                    dr["Sapqty"] = vbap.get_Cell(i, "KWMENG");
                    dr["WERKS"] = vbap.get_Cell(i, "WERKS");
                    dr["VRKME"] = vbap.get_Cell(i, "VRKME");
                    dr["LGORT"] = vbap.get_Cell(i, "LGORT");
                    dr["VSTEL"] = vbap.get_Cell(i, "VSTEL");
                    dr["AUFNR"] = vbap.get_Cell(i, "AUFNR");
                    dr["VGPOS"] = vbap.get_Cell(i, "VGPOS");
                    dr["MATNR"] = vbap.get_Cell(i, "MATNR");
                    dr["ARKTX"] = vbap.get_Cell(i, "ARKTX");
                    dr["CHARG"] = vbap.get_Cell(i, "CHARG"); 
                    dr["OutBQty"] =vbap.get_Cell(i, "KWMENG");

                    table.Rows.Add(dr);
                }

                for (int i = 1; i <= rels.RowCount; i++)
                {
                    DataRow dr = ReqTab.NewRow();                   
                    dr["ID"] = i.ToString();
                    dr["ZRQWK"] = rels.get_Cell(i, "ZRQWK");                    
                    dr["ZREQNO"] = rels.get_Cell(i, "ZREQNO");
                    dr["ZREQPS"] = rels.get_Cell(i, "ZREQPS");
                    dr["ATINN"] = rels.get_Cell(i, "ATINN");
                    dr["ATZHL"] = rels.get_Cell(i, "ATZHL");
                    dr["ATWRT"] = rels.get_Cell(i, "ATWRT");                    
                    
                    ReqTab.Rows.Add(dr);
                }
                
                ds.Tables.Add(table.Copy());
                ds.Tables.Add(ReqTab.Copy());
                conn.Logoff();
                this.Content.DataSource = ds.Tables[0];
                gridControl1.DataSource = ds.Tables[1].DefaultView;
                
                                          
            }
        }

        private void Content_Click(object sender, EventArgs e)
        {            
            string s1;
            s1 = (string) gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "VGPOS");            
            ds.Tables[1].DefaultView.RowFilter = " ZREQPS='" + s1 + "'";
            gridControl1.DataSource = ds.Tables[1].DefaultView;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();                
                serverFactory.Get<IShipmentOperationEngine>().SavePickData(ds,"");
                //dsReturn = serverFactory.Get<IShipmentOperationEngine>().Query(dsParams,ref config);
                //_errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
            }
            catch (Exception ex)
            {                
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }

        }

    }
}

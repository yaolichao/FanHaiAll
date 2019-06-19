using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.WMS.Gui
{
    public partial class PickCarInfoWinFM : Form
    {
        public PickCarInfoWinFM()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (( string.IsNullOrEmpty(edCI.Text) == true) 
                || ( string.IsNullOrEmpty(cbShipType.Text) == true) 
                || ( string.IsNullOrEmpty(edShipNO.Text) == true) )
            {
                MessageBox.Show("请输入货运信息！");
                return;
            }
            string str = "";
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                serverFactory.Get<IPickOperationEngine>().UpateCarInfo(edCI.Text,
                                                                cbShipType.Items.IndexOf(cbShipType.Text).ToString(),
                                                                edShipNO.Text,
                                                                edOutBandNO.Text,
                                                                edSapvbeln.Text);                                                         
               
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



        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZHDSpace;

namespace QCCheck
{
    public partial class frmShipNo : Form
    {
        public frmShipNo()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtShipNo.Text == "")
            {
                MessageBox.Show("The Ship No Can Not Be Empty, Please Scan Your Number!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            ZHDSpace.DBUtility dbAct = new DBUtility();
            string strSQL = "Select Count(*) From ShipNoList Where ship_no = '" + txtShipNo.Text.Trim().ToString() + "'";
            int rtnValue =Convert.ToInt32( dbAct.GetSingle(strSQL));
            if (rtnValue > 0)
            {
                MessageBox.Show("This Ship No is Already Exist, Please Reset Again.");
            }
            else
            {
                strSQL = "Insert Into ShipNoList(ship_no,ship_date) Values('" + txtShipNo.Text.Trim() + "','" + dtpShipDate.Value.ToString() + "')";
                rtnValue = dbAct.ExecuteSql(strSQL);
                if (rtnValue > 0)
                {
                    this.Close();                    
                }
                else
                    MessageBox.Show("NG");
            }
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

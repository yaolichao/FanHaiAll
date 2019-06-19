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
    public partial class LogIn : Form
    {
        DBUtility db = new DBUtility();

        public LogIn()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sLoginno, sPassword,sql;

            sLoginno = txtUID.Text.Trim();
            sPassword = txtPWD.Text.Trim();
            if (sLoginno == "")
            {
                MessageBox.Show("用户名不能为空","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtUID.SelectAll();
                txtUID.Focus();
                return;
            }
            if (sPassword == "")
            {
                MessageBox.Show("用户名不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPWD.SelectAll();
                txtPWD.Focus();
                return;
            }

            sql = "select * from bcp_sfis_userinfo  where  user_id='" + sLoginno + "' and pass_word='" + sPassword + "'";
            DataSet dsUserInfo = db.Query(sql);
            if (dsUserInfo.Tables[0].Rows.Count > 0)
            {
                DBUtility.sUserId = sLoginno;
                DBUtility.sUserName = dsUserInfo.Tables[0].Rows[0]["user_name"].ToString().Trim();
                DBUtility.sUserRight = dsUserInfo.Tables[0].Rows[0]["user_right"].ToString().Trim();
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtUID.SelectAll();
                txtUID.Focus();
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPWD.SelectAll();
                txtPWD.Focus();
            }
        }

        private void txtPWD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnLogin_Click(sender,e);
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            //txtUID.Focus();
        }

        private void LogIn_Shown(object sender, EventArgs e)
        {
            txtUID.Focus();
        }
    }
}

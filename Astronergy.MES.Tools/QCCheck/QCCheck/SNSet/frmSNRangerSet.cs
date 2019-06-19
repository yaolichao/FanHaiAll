using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZHDSpace;

namespace QCCheck.SNSet
{
    public partial class frmSNRangerSet : Form
    {
        public static string sFlay;
        DBUtility db = new DBUtility();

        public frmSNRangerSet()
        {
            InitializeComponent();
        }

        private void frmSNRangerSet_Load(object sender, EventArgs e)
        {
            DataSet dsSNRanger = GetSNRangerSet();
            gvSNRange.DataSource = null;
            gvSNRange.DataSource = dsSNRanger.Tables[0];
            gvSNRange_Bind();
            sFlay = "";
            UdateToolButtonStatus();
        }

        public void UdateToolButtonStatus()
        { 
            switch (sFlay)
            {
                case "A":
                    tsbAdd.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = true;
                    tsbCancel.Enabled = true;
                    txtProductID.Text = "";
                    txtProductID.Enabled = true;
                    txtProductType.Text = "";
                    txtProductType.Enabled = true;
                    txtStartSN.Text = "";
                    txtStartSN.Enabled = true;
                    txtEndSN.Text = "";
                    txtEndSN.Enabled = true;
                    break;
                case "U":
                    tsbAdd.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = true;
                    tsbCancel.Enabled = true;
                    //txtProductID.Text = "";
                    txtProductID.Enabled = false;
                    //txtProductType.Text = "";
                    txtProductType.Enabled = false;
                    //txtStartSN.Text = "";
                    txtStartSN.Enabled = true;
                    //txtEndSN.Text = "";
                    txtEndSN.Enabled = true;
                    break;
                case "D":
                    break;
                case "S":
                    break;
                case "C":
                    break;
                default:
                    tsbAdd.Enabled = true;
                    tsbUpdate.Enabled = true;
                    tsbDelete.Enabled = true;
                    tsbSave.Enabled = false;
                    tsbCancel.Enabled = false;
                    txtProductID.Text = "";
                    txtProductID.Enabled = false;
                    txtProductType.Text = "";
                    txtProductType.Enabled = false;
                    txtStartSN.Text = "";
                    txtStartSN.Enabled = false;
                    txtEndSN.Text = "";
                    txtEndSN.Enabled = false;
                    break;
            }
        }

        public DataSet GetSNRangerSet()
        {
            DataSet ds = new DataSet();
            string sProductID = "", sProductType = "", sql="";
            sProductID = txtProductID.Text.Trim();
            sProductType = txtProductID.Text.Trim();
            sql = "select product_id,product_type,start_num,end_num from sn_print_set where 1=1";
            if (!string.IsNullOrEmpty(sProductID))
            {
                sql += " and product_id='" + sProductID + "'";
            }
            if (!string.IsNullOrEmpty(sProductType))
            {
                sql += " and product_type='" + sProductType + "'";
            }
            sql += " order by product_id,product_type,start_num,end_num asc";
            ds = db.Query(sql);
            return ds;
        }

        private void gvSNRange_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                gvSNRange.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gvSNRange.RowHeadersDefaultCellStyle.Font,
                rectangle,
                gvSNRange.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        public void gvSNRange_Bind()
        {
            gvSNRange.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gvSNRange.Columns["product_id"].HeaderText = "产品号";
            gvSNRange.Columns["product_type"].HeaderText = "产品类型";
            gvSNRange.Columns["start_num"].HeaderText = "起始序号";
            gvSNRange.Columns["end_num"].HeaderText = "终止序号";
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            sFlay = "A";
            UdateToolButtonStatus();
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (gvSNRange.CurrentRow.Index < 0)
            {
                MessageBox.Show("没有选中任何要修改的行，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int nIndex = gvSNRange.CurrentRow.Index;
            txtProductID.Text = gvSNRange.Rows[nIndex].Cells["product_id"].Value.ToString().Trim();
            txtProductType.Text = gvSNRange.Rows[nIndex].Cells["product_type"].Value.ToString().Trim();
            txtStartSN.Text = gvSNRange.Rows[nIndex].Cells["start_num"].Value.ToString().Trim();
            txtEndSN.Text = gvSNRange.Rows[nIndex].Cells["end_num"].Value.ToString().Trim(); ;
            sFlay = "U";
            UdateToolButtonStatus();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (gvSNRange.CurrentRow.Index < 0)
            {
                MessageBox.Show("没有选中任何要删除的行，请确认！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("请确认是否要删除选中的数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                string sProductID = "", sProductType = "", sql = "";
                int nIndex, nResult;
                nIndex = gvSNRange.CurrentRow.Index;
                sProductID = gvSNRange.Rows[nIndex].Cells["product_id"].Value.ToString().Trim();
                sProductType = gvSNRange.Rows[nIndex].Cells["product_type"].Value.ToString().Trim();
                if (string.IsNullOrEmpty(sProductID) || string.IsNullOrEmpty(sProductType))
                {
                    MessageBox.Show("选中行中的数据异常，请刷新后重试！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sql = "delete from sn_print_set where product_id='" + sProductID + "' and product_type='" + sProductType + "'";
                nResult = db.ExecuteSql(sql);
                if (nResult > 0)
                {
                    MessageBox.Show("删除数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("删除数据失败,请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                sFlay = "";
                UdateToolButtonStatus();
                DataSet dsSNRanger = GetSNRangerSet();
                gvSNRange.DataSource = null;
                gvSNRange.DataSource = dsSNRanger.Tables[0];
                gvSNRange_Bind();
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            string sProductID = "", sProductType = "", sql = "", sStartSN, sEndSn;
            int nStartSN, nEndSn,nResult;
            nResult = 0;
            sProductID = txtProductID.Text.Trim();
            sProductType = txtProductType.Text.Trim();
            sStartSN = txtStartSN.Text.Trim();
            sEndSn = txtEndSN.Text.Trim();
            if (string.IsNullOrEmpty(sProductID))
            {
                MessageBox.Show("产品号不能为空，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductID.SelectAll();
                txtProductID.Focus();
                return;
            }
            if (string.IsNullOrEmpty(sProductType))
            {
                MessageBox.Show("产品类型不能为空，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductType.SelectAll();
                txtProductType.Focus();
                return;
            }
            try
            {
                nStartSN = int.Parse(sStartSN);
            }
            catch
            {
                MessageBox.Show("起始序号应为整数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStartSN.SelectAll();
                txtStartSN.Focus();
                return;
            }
            try
            {
                nEndSn = int.Parse(sEndSn);
            }
            catch
            {
                MessageBox.Show("终止序号应为整数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEndSN.SelectAll();
                txtEndSN.Focus();
                return;
            }
            if (nStartSN >= nEndSn)
            {
                MessageBox.Show("起始序号不能大于或等于终止序号，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (sFlay == "A")
            {
                sql = "select * from sn_print_set where product_id='" + sProductID + "' and product_type='" + sProductType + "'";
                DataSet ds = db.Query(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("产品号[" + sProductID +"]产品类型[" + sProductType + "]对应的流水号范围已存在,不可新增！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sql = "insert into sn_print_set(product_id,product_type,start_num,end_num)";
                sql += " values('" + sProductID + "','" + sProductType + "'," + nStartSN + "," + nEndSn + ")";
                nResult = db.ExecuteSql(sql);
                if (nResult > 0)
                {
                    MessageBox.Show("新增数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("新增数据失败,请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            if (sFlay == "U")
            {
                sql = "update sn_print_set set start_num=" + nStartSN + ",end_num=" + nEndSn + ", where product_id='" + sProductID + "' and product_type='" + sProductType + "'";
                nResult = db.ExecuteSql(sql);
                if (nResult > 0)
                {
                    MessageBox.Show("修改数据成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改数据失败,请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            sFlay = "";
            UdateToolButtonStatus();
            DataSet dsSNRanger = GetSNRangerSet();
            gvSNRange.DataSource = null;
            gvSNRange.DataSource = dsSNRanger.Tables[0];
            gvSNRange_Bind();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            sFlay = "";
            UdateToolButtonStatus();
        }
    }
}

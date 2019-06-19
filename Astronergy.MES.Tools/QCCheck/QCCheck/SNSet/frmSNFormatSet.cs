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
    public partial class frmSNFormatSet : Form
    {
        public static string sFlay;
        DBUtility db = new DBUtility();

        public frmSNFormatSet()
        {
            InitializeComponent();
        }

        private void frmSNFormatSet_Load(object sender, EventArgs e)
        {
            DataSet dsSNFormat = GetSNFormatSetByCustomer();
            gvSNFormat.DataSource = null;
            gvSNFormat.DataSource = dsSNFormat.Tables[0];
            gvSNFormat_Bind();
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
                    txtCustomer.Text = "";
                    txtCustomer.Enabled = true;
                    cboSequence.SelectedIndex = -1;
                    cboSequence.Enabled = true;
                    txtParameter.Text = "";
                    txtParameter.Enabled = true;
                    cboParameterType.SelectedIndex = -1;
                    cboParameterType.Enabled = true;
                    txtParameterValue.Text = "";
                    txtParameterValue.Enabled = true;
                    txtAdjustValue.Text = "";
                    txtAdjustValue.Enabled = true;
                    cboAdjustType.SelectedIndex = -1;
                    cboAdjustType.Enabled = true;
                    txtStartIndex.Text = "";
                    txtStartIndex.Enabled = true;
                    txtLenth.Text = "";
                    txtLenth.Enabled = true;
                    txtFormat.Text = "";
                    txtFormat.Enabled = true;
                    break;
                case "U":
                    tsbAdd.Enabled = false;
                    tsbUpdate.Enabled = false;
                    tsbDelete.Enabled = false;
                    tsbSave.Enabled = true;
                    tsbCancel.Enabled = true;
                    //txtCustomer.Text = "";
                    txtCustomer.Enabled = false;
                    //cboSequence.SelectedIndex = -1;
                    cboSequence.Enabled = false;
                    //txtParameter.Text = "";
                    txtParameter.Enabled = true;
                    //cboParameterType.SelectedIndex = -1;
                    cboParameterType.Enabled = true;
                    //txtParameterValue.Text = "";
                    txtParameterValue.Enabled = true;
                    //txtAdjustValue.Text = "";
                    txtAdjustValue.Enabled = true;
                    //cboAdjustType.SelectedIndex = -1;
                    cboAdjustType.Enabled = true;
                    //txtStartIndex.Text = "";
                    txtStartIndex.Enabled = true;
                    //txtLenth.Text = "";
                    txtLenth.Enabled = true;
                    //txtFormat.Text = "";
                    txtFormat.Enabled = true;
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
                    txtCustomer.Text = "";
                    txtCustomer.Enabled = false;
                    cboSequence.SelectedIndex = -1;
                    cboSequence.Enabled = false;
                    txtParameter.Text = "";
                    txtParameter.Enabled = false;
                    cboParameterType.SelectedIndex = -1;
                    cboParameterType.Enabled = false;
                    txtParameterValue.Text = "";
                    txtParameterValue.Enabled = false;
                    txtAdjustValue.Text = "";
                    txtAdjustValue.Enabled = false;
                    cboAdjustType.SelectedIndex = -1;
                    cboAdjustType.Enabled = false;
                    txtStartIndex.Text = "";
                    txtStartIndex.Enabled = false;
                    txtLenth.Text = "";
                    txtLenth.Enabled = false;
                    txtFormat.Text = "";
                    txtFormat.Enabled = false;
                    break;
            }
        }

        public DataSet GetSNFormatSetByCustomer()
        {
            DataSet ds = new DataSet();
            string sCustomer, sql;
            sCustomer = txtQCustomer.Text.Trim();
            sql = "select * from sn_format_set where customer like '%" + sCustomer + "%'";
            sql += " order by customer,sequence";
            ds = db.Query(sql);
            return ds;
        }

        private void gvSNFormat_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                gvSNFormat.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                gvSNFormat.RowHeadersDefaultCellStyle.Font,
                rectangle,
                gvSNFormat.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        public void gvSNFormat_Bind()
        {
            gvSNFormat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gvSNFormat.Columns["customer"].HeaderText = "模版名称";
            gvSNFormat.Columns["sequence"].HeaderText = "参数序号";
            gvSNFormat.Columns["parameter"].HeaderText = "参数名称";
            gvSNFormat.Columns["parameter_type"].HeaderText = "参数类型";
            gvSNFormat.Columns["parameter_value"].HeaderText = "参数值";
            gvSNFormat.Columns["adjust_type"].HeaderText = "调整类型";
            gvSNFormat.Columns["adjust_value"].HeaderText = "调整值";
            gvSNFormat.Columns["start_index"].HeaderText = "起始位置";
            gvSNFormat.Columns["length"].HeaderText = "字符长度";
            gvSNFormat.Columns["format"].HeaderText = "格式";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet dsSNFormat = GetSNFormatSetByCustomer();
            gvSNFormat.DataSource = null;
            gvSNFormat.DataSource = dsSNFormat.Tables[0];
            gvSNFormat_Bind();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            sFlay = "A";
            UdateToolButtonStatus();
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            if (gvSNFormat.CurrentRow.Index < 0)
            {
                MessageBox.Show("没有选中任何要修改的行，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int nIndex = gvSNFormat.CurrentRow.Index;
            txtCustomer.Text = gvSNFormat.Rows[nIndex].Cells["customer"].Value.ToString().Trim();
            //cboSequence.SelectedIndex = cboSequence.Items.IndexOf(gvSNFormat.Rows[nIndex].Cells["sequence"].Value);
            cboSequence.SelectedIndex = cboSequence.Items.IndexOf(gvSNFormat.Rows[nIndex].Cells["sequence"].Value.ToString().Trim());
            txtParameter.Text = gvSNFormat.Rows[nIndex].Cells["parameter"].Value.ToString().Trim();
            cboParameterType.SelectedIndex = cboParameterType.Items.IndexOf(gvSNFormat.Rows[nIndex].Cells["parameter_type"].Value);
            txtParameterValue.Text = gvSNFormat.Rows[nIndex].Cells["parameter_value"].Value.ToString().Trim();
            txtAdjustValue.Text = gvSNFormat.Rows[nIndex].Cells["adjust_value"].Value.ToString().Trim();
            cboAdjustType.SelectedIndex = cboAdjustType.Items.IndexOf(gvSNFormat.Rows[nIndex].Cells["adjust_type"].Value);
            txtStartIndex.Text = gvSNFormat.Rows[nIndex].Cells["start_index"].Value.ToString().Trim();
            txtLenth.Text = gvSNFormat.Rows[nIndex].Cells["length"].Value.ToString().Trim();
            txtFormat.Text = gvSNFormat.Rows[nIndex].Cells["format"].Value.ToString().Trim();
            sFlay = "U";
            UdateToolButtonStatus();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (gvSNFormat.CurrentRow.Index < 0)
            {
                MessageBox.Show("没有选中任何要删除的行，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("请确认是否要删除选中的数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                string sql, sCustomer, sSequence;
                int nIndex, nResult;
                nResult = 0;
                nIndex = gvSNFormat.CurrentRow.Index;
                sCustomer = gvSNFormat.Rows[nIndex].Cells["customer"].Value.ToString().Trim();
                sSequence = gvSNFormat.Rows[nIndex].Cells["sequence"].Value.ToString().Trim();
                sql = "delete from sn_format_set where customer='" + sCustomer + "' and sequence='" + sSequence + "'";
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
                DataSet dsSNFormat = GetSNFormatSetByCustomer();
                gvSNFormat.DataSource = null;
                gvSNFormat.DataSource = dsSNFormat.Tables[0];
                gvSNFormat_Bind();
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            string sCustomer, sParameter, sparameter_type, sParameter_value, sAdjust_type, sFormat, sql;
            string sSequence, sAdjust_value, sStart_index, slength;
            int nResult;
            nResult = 0;
            sCustomer = txtCustomer.Text.Trim();
            sParameter = txtParameter.Text.Trim();
            sparameter_type = cboParameterType.Text.Trim();
            sParameter_value = txtParameterValue.Text.Trim();
            sAdjust_type = cboAdjustType.Text.Trim();
            sFormat = txtFormat.Text.Trim();
            if (cboSequence.SelectedIndex < 0)
            {
                MessageBox.Show("请选择参数序号！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sSequence = cboSequence.Text.Trim();
            sAdjust_value = txtAdjustValue.Text.Trim();
            sStart_index = txtStartIndex.Text.Trim();
            slength = txtLenth.Text.Trim();
            if (sCustomer == "")
            {
                MessageBox.Show("模版名称不能为空，请确认！","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtCustomer.SelectAll();
                txtCustomer.Focus();
                return;
            }
            if (sSequence == "")
            {
                MessageBox.Show("模版序号不能为空，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSequence.Focus();
                return;
            }
            if (sAdjust_value != "")
            {
                try
                {
                    int.Parse(sAdjust_value);
                }
                catch
                {
                    MessageBox.Show("调整值只能是整数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAdjustValue.SelectAll();
                    txtAdjustValue.Focus();
                    return;
                }
            }
            if (sStart_index != "")
            {
                try
                {
                    int.Parse(sStart_index);
                }
                catch
                {
                    MessageBox.Show("起始位置只能是整数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStartIndex.SelectAll();
                    txtStartIndex.Focus();
                    return;
                }
            }
            if (slength != "")
            {
                try
                {
                    int.Parse(slength);
                }
                catch
                {
                    MessageBox.Show("字符长度只能是整数，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLenth.SelectAll();
                    txtLenth.Focus();
                    return;
                }
            }
            if (sFlay == "A")
            {
                sql = "select * from sn_format_set where customer='" + sCustomer + "' and sequence='" + sSequence + "'";
                DataSet ds = db.Query(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("模版名称[" + sCustomer + "]参数序号["+ sSequence + "]的数据已存在，请确认！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSequence.Focus();
                    return;
                }
                sql = "insert into sn_format_set(customer,sequence,parameter,parameter_type,parameter_value,adjust_type,adjust_value,start_index,length,format)";
                sql += " values('" + sCustomer + "','" + sSequence + "','" + sParameter + "','" + sparameter_type + "','" + sParameter_value + "'";
                sql += ",'" + sAdjust_type + "','" + sAdjust_value + "','" + sStart_index + "','" + slength + "','" + sFormat + "')";
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
                sql = "update sn_format_set set parameter='" + sParameter + "',parameter_type='" + sparameter_type + "'";
                sql += ",parameter_value='" + sParameter_value + "',adjust_type='" + sAdjust_type + "',adjust_value='" + sAdjust_value + "'";
                sql += ",start_index='" + sStart_index + "',length='" + slength + "',format='" + sFormat + "'";
                sql += " where customer='" + sCustomer + "' and sequence='" + sSequence + "'";
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
            DataSet dsSNFormat = GetSNFormatSetByCustomer();
            gvSNFormat.DataSource = null;
            gvSNFormat.DataSource = dsSNFormat.Tables[0];
            gvSNFormat_Bind();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            sFlay = "";
            UdateToolButtonStatus();
        }
    }
}

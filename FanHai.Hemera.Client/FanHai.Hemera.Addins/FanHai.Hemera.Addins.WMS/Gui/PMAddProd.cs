using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FanHai.Hemera.Addins.WMS.Gui
{
    public partial class PMAddProd : Form
    {
        DataSet DSource;
        public PMAddProd()
        {
            InitializeComponent();
        }
        public PMAddProd(DataSet _ds)
        {            
            InitializeComponent();
            DSource = _ds;
            Content.DataSource = DSource.Tables["RT_VBAP"];
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int[] selectedRow = gridView1.GetSelectedRows();
            DataTable dtSelected = new DataTable();
            dtSelected = DSource.Tables["RT_VBAP"].Clone();
            if (selectedRow.Length <= 0)
            {
                MessageBox.Show(this, "未选中任何行！" ,"操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ;
            }
            for (int i = 0; i < selectedRow.Length; i++)
            {
                 int index = selectedRow[i];
                 dtSelected.ImportRow(DSource.Tables["RT_VBAP"].Rows[index]);
            }
            dtSelected.TableName = "SelectedRow";
            dtSelected.AcceptChanges();
            DSource.Tables.Add(dtSelected);
            this.Close();
        }
    }
}

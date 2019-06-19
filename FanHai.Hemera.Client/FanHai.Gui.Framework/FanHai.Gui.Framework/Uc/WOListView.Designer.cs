using System.Windows.Forms;
using System.Data.OracleClient;
using System;

namespace FanHai.MES.Framework
{
    partial class WOListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView1
            // 
            //this.listView1.Location = new System.Drawing.Point(0, 0);
            //this.listView1.Name = "listView1";
            //this.listView1.Size = new System.Drawing.Size(121, 97);
            //this.listView1.TabIndex = 0;
            //this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(600, 390);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;

            this.listView1.Height = this.ClientRectangle.Height ;   
            this.listView1.GridLines = true ;//显示各个记录的分隔线   
            this.listView1.FullRowSelect = true ; //要选择就是一行   
            this.listView1.View = View.Details ; //定义列表显示的方式   
            this.listView1.Scrollable = true ; //需要时候显示滚动条   
            this.listView1.MultiSelect = false ; // 不可以多行选择   
            this.listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable ;   
            // 针对数据库的字段名称，建立与之适应显示表头   
            this.listView1.Columns.Add("NUMBER", 60, HorizontalAlignment.Right);
            this.listView1.Columns.Add("WORK_ORDER_ID", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_NUMBER", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_STATE", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_PRIORITY", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ENTERED_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("PROMISED_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("FINISHED_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("SHIPPED_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CLOSED_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("SOP_ID", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("DESCRIPTION", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("COMMENTS", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_STATUS", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_FLAG", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ORDER_COLSE_TYPE", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CREATOR", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CREATE_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("EDITOR", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("EDIT_TIME", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("CREATE_TIMEZONE_KEY", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("EDIT_TIMEZONE_KEY", 100, HorizontalAlignment.Left); 
            this.listView1.Visible = true ;
            InitializeComponent(this.listView1);

            // 
            // WOListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Name = "WOListView";
            this.ResumeLayout(false);

        }
        public void InitializeComponent(ListView lv) 
        {
            //-------------------------连接DB--------------------------------------------
            string ConnectionString = "Data Source=SUNVIEW;User ID=sunmesdev;Password=sunmesdev";   //写连接串   
            OracleConnection conn = new OracleConnection(ConnectionString);   //创建一个新连接   
            try
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select   *   from   base_work_order ";   //在这儿写sql语句   
                OracleDataReader odr = cmd.ExecuteReader();//创建一个OracleDateReader对象   

                int cut = 0;
                while (odr.Read())//读取数据，如果odr.Read()返回为false的话，就说明到记录集的尾部了                                   
                {
                    ListViewItem li = new ListViewItem ( ) ;   
                    cut++;
                    li.SubItems.Clear ( ) ;
                    li.SubItems[0].Text = cut.ToString();
                    li.SubItems.Add(odr["WORK_ORDER_ID"].ToString());
                    li.SubItems.Add(odr["ORDER_NUMBER"].ToString());
                    li.SubItems.Add(odr["ORDER_STATE"].ToString());
                    li.SubItems.Add(odr["ORDER_PRIORITY"].ToString());
                    li.SubItems.Add(odr["ENTERED_TIME"].ToString());
                    li.SubItems.Add(odr["PROMISED_TIME"].ToString());
                    li.SubItems.Add(odr["FINISHED_TIME"].ToString());
                    li.SubItems.Add(odr["SHIPPED_TIME"].ToString());
                    li.SubItems.Add(odr["CLOSED_TIME"].ToString());
                    li.SubItems.Add(odr["SOP_ID"].ToString());
                    li.SubItems.Add(odr["DESCRIPTION"].ToString());
                    li.SubItems.Add(odr["COMMENTS"].ToString());
                    li.SubItems.Add(odr["ORDER_STATUS"].ToString());
                    li.SubItems.Add(odr["ORDER_FLAG"].ToString());
                    li.SubItems.Add(odr["ORDER_COLSE_TYPE"].ToString());
                    li.SubItems.Add(odr["CREATOR"].ToString());
                    li.SubItems.Add(odr["CREATE_TIME"].ToString());
                    li.SubItems.Add(odr["EDITOR"].ToString());
                    li.SubItems.Add(odr["EDIT_TIME"].ToString());
                    li.SubItems.Add(odr["CREATE_TIMEZONE_KEY"].ToString());
                    li.SubItems.Add(odr["EDIT_TIMEZONE_KEY"].ToString()); 
                    lv.Items.Add ( li ) ; 

                }
                odr.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);//如果有错误，输出错误信息   
            }
            finally
            {
                conn.Close();   //关闭连接   
            }
        
        }

        #endregion

        private System.Windows.Forms.ListView listView1;
    }
}

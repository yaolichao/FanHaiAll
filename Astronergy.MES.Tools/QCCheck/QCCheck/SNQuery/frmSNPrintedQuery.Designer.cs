namespace QCCheck.SNQuery
{
    partial class frmSNPrintedQuery
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSNStart = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.cboLabelType = new System.Windows.Forms.ComboBox();
            this.txtSNStart = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWO = new System.Windows.Forms.Label();
            this.lblProductType = new System.Windows.Forms.Label();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblLabelType = new System.Windows.Forms.Label();
            this.lblWeek = new System.Windows.Forms.Label();
            this.lblPrintDate = new System.Windows.Forms.Label();
            this.lblRePrintUser = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.btnExcel = new System.Windows.Forms.Button();
            this.txtSNEnd = new System.Windows.Forms.TextBox();
            this.txtWO = new System.Windows.Forms.TextBox();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.dtpPrintDate = new System.Windows.Forms.DateTimePicker();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.txtRePrintUser = new System.Windows.Forms.TextBox();
            this.txtWeek = new System.Windows.Forms.TextBox();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.dgvPrintSN = new System.Windows.Forms.DataGridView();
            this.cbxPrintDate = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintSN)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.cbxPrintDate);
            this.panel1.Controls.Add(this.txtPower);
            this.panel1.Controls.Add(this.txtWeek);
            this.panel1.Controls.Add(this.txtRePrintUser);
            this.panel1.Controls.Add(this.txtMonth);
            this.panel1.Controls.Add(this.lblPower);
            this.panel1.Controls.Add(this.lblMonth);
            this.panel1.Controls.Add(this.dtpPrintDate);
            this.panel1.Controls.Add(this.txtYear);
            this.panel1.Controls.Add(this.txtProductCode);
            this.panel1.Controls.Add(this.txtProductType);
            this.panel1.Controls.Add(this.txtWO);
            this.panel1.Controls.Add(this.txtSNEnd);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.lblYear);
            this.panel1.Controls.Add(this.lblRePrintUser);
            this.panel1.Controls.Add(this.lblPrintDate);
            this.panel1.Controls.Add(this.lblWeek);
            this.panel1.Controls.Add(this.lblLabelType);
            this.panel1.Controls.Add(this.lblProductCode);
            this.panel1.Controls.Add(this.lblProductType);
            this.panel1.Controls.Add(this.lblWO);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSNStart);
            this.panel1.Controls.Add(this.cboLabelType);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.lblSNStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 175);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.dgvPrintSN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 175);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 301);
            this.panel2.TabIndex = 1;
            // 
            // lblSNStart
            // 
            this.lblSNStart.AutoSize = true;
            this.lblSNStart.Location = new System.Drawing.Point(10, 9);
            this.lblSNStart.Name = "lblSNStart";
            this.lblSNStart.Size = new System.Drawing.Size(65, 12);
            this.lblSNStart.TabIndex = 0;
            this.lblSNStart.Text = "起始序号：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(515, 143);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查  询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // cboLabelType
            // 
            this.cboLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelType.FormattingEnabled = true;
            this.cboLabelType.Location = new System.Drawing.Point(76, 119);
            this.cboLabelType.Name = "cboLabelType";
            this.cboLabelType.Size = new System.Drawing.Size(176, 20);
            this.cboLabelType.TabIndex = 2;
            // 
            // txtSNStart
            // 
            this.txtSNStart.Location = new System.Drawing.Point(76, 6);
            this.txtSNStart.Name = "txtSNStart";
            this.txtSNStart.Size = new System.Drawing.Size(176, 21);
            this.txtSNStart.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(258, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "~";
            // 
            // lblWO
            // 
            this.lblWO.AutoSize = true;
            this.lblWO.Location = new System.Drawing.Point(10, 36);
            this.lblWO.Name = "lblWO";
            this.lblWO.Size = new System.Drawing.Size(65, 12);
            this.lblWO.TabIndex = 5;
            this.lblWO.Text = "工 单 号：";
            // 
            // lblProductType
            // 
            this.lblProductType.AutoSize = true;
            this.lblProductType.Location = new System.Drawing.Point(10, 64);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(65, 12);
            this.lblProductType.TabIndex = 6;
            this.lblProductType.Text = "产品型号：";
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Location = new System.Drawing.Point(10, 93);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(65, 12);
            this.lblProductCode.TabIndex = 7;
            this.lblProductCode.Text = "产品代码：";
            // 
            // lblLabelType
            // 
            this.lblLabelType.AutoSize = true;
            this.lblLabelType.Location = new System.Drawing.Point(10, 122);
            this.lblLabelType.Name = "lblLabelType";
            this.lblLabelType.Size = new System.Drawing.Size(65, 12);
            this.lblLabelType.TabIndex = 8;
            this.lblLabelType.Text = "标签类型：";
            // 
            // lblWeek
            // 
            this.lblWeek.AutoSize = true;
            this.lblWeek.Location = new System.Drawing.Point(282, 122);
            this.lblWeek.Name = "lblWeek";
            this.lblWeek.Size = new System.Drawing.Size(65, 12);
            this.lblWeek.TabIndex = 9;
            this.lblWeek.Text = "周    别：";
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.AutoSize = true;
            this.lblPrintDate.Location = new System.Drawing.Point(282, 36);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Size = new System.Drawing.Size(65, 12);
            this.lblPrintDate.TabIndex = 10;
            this.lblPrintDate.Text = "打印日期：";
            // 
            // lblRePrintUser
            // 
            this.lblRePrintUser.AutoSize = true;
            this.lblRePrintUser.Location = new System.Drawing.Point(10, 148);
            this.lblRePrintUser.Name = "lblRePrintUser";
            this.lblRePrintUser.Size = new System.Drawing.Size(65, 12);
            this.lblRePrintUser.TabIndex = 11;
            this.lblRePrintUser.Text = "补印人员：";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(282, 64);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(65, 12);
            this.lblYear.TabIndex = 12;
            this.lblYear.Text = "年    份：";
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(609, 143);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 13;
            this.btnExcel.Text = "导  出";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // txtSNEnd
            // 
            this.txtSNEnd.Location = new System.Drawing.Point(284, 6);
            this.txtSNEnd.Name = "txtSNEnd";
            this.txtSNEnd.Size = new System.Drawing.Size(176, 21);
            this.txtSNEnd.TabIndex = 14;
            // 
            // txtWO
            // 
            this.txtWO.Location = new System.Drawing.Point(76, 33);
            this.txtWO.Name = "txtWO";
            this.txtWO.Size = new System.Drawing.Size(176, 21);
            this.txtWO.TabIndex = 15;
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(76, 61);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(176, 21);
            this.txtProductType.TabIndex = 16;
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(76, 90);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(176, 21);
            this.txtProductCode.TabIndex = 17;
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(350, 61);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(110, 21);
            this.txtYear.TabIndex = 18;
            // 
            // dtpPrintDate
            // 
            this.dtpPrintDate.CustomFormat = "yyyy-MM-dd";
            this.dtpPrintDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPrintDate.Location = new System.Drawing.Point(350, 32);
            this.dtpPrintDate.Name = "dtpPrintDate";
            this.dtpPrintDate.Size = new System.Drawing.Size(110, 21);
            this.dtpPrintDate.TabIndex = 19;
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Location = new System.Drawing.Point(282, 93);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(65, 12);
            this.lblMonth.TabIndex = 20;
            this.lblMonth.Text = "月    份：";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(282, 148);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(65, 12);
            this.lblPower.TabIndex = 21;
            this.lblPower.Text = "功率档位：";
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(350, 90);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(110, 21);
            this.txtMonth.TabIndex = 22;
            // 
            // txtRePrintUser
            // 
            this.txtRePrintUser.Location = new System.Drawing.Point(76, 145);
            this.txtRePrintUser.Name = "txtRePrintUser";
            this.txtRePrintUser.Size = new System.Drawing.Size(176, 21);
            this.txtRePrintUser.TabIndex = 23;
            // 
            // txtWeek
            // 
            this.txtWeek.Location = new System.Drawing.Point(350, 119);
            this.txtWeek.Name = "txtWeek";
            this.txtWeek.Size = new System.Drawing.Size(110, 21);
            this.txtWeek.TabIndex = 24;
            // 
            // txtPower
            // 
            this.txtPower.Location = new System.Drawing.Point(350, 145);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(110, 21);
            this.txtPower.TabIndex = 25;
            // 
            // dgvPrintSN
            // 
            this.dgvPrintSN.AllowUserToAddRows = false;
            this.dgvPrintSN.AllowUserToDeleteRows = false;
            this.dgvPrintSN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrintSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrintSN.Location = new System.Drawing.Point(0, 0);
            this.dgvPrintSN.Name = "dgvPrintSN";
            this.dgvPrintSN.ReadOnly = true;
            this.dgvPrintSN.RowTemplate.Height = 23;
            this.dgvPrintSN.Size = new System.Drawing.Size(704, 297);
            this.dgvPrintSN.TabIndex = 0;
            this.dgvPrintSN.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvPrintSN_RowPostPaint);
            // 
            // cbxPrintDate
            // 
            this.cbxPrintDate.AutoSize = true;
            this.cbxPrintDate.Location = new System.Drawing.Point(467, 36);
            this.cbxPrintDate.Name = "cbxPrintDate";
            this.cbxPrintDate.Size = new System.Drawing.Size(15, 14);
            this.cbxPrintDate.TabIndex = 26;
            this.cbxPrintDate.UseVisualStyleBackColor = true;
            // 
            // frmSNPrintedQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 476);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmSNPrintedQuery";
            this.Text = "打印组件序号报表";
            this.Load += new System.EventHandler(this.frmSNPrintedQuery_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrintSN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSNEnd;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblRePrintUser;
        private System.Windows.Forms.Label lblPrintDate;
        private System.Windows.Forms.Label lblWeek;
        private System.Windows.Forms.Label lblLabelType;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.Label lblWO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSNStart;
        private System.Windows.Forms.ComboBox cboLabelType;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label lblSNStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtWO;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.DateTimePicker dtpPrintDate;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.TextBox txtRePrintUser;
        private System.Windows.Forms.TextBox txtPower;
        private System.Windows.Forms.TextBox txtWeek;
        private System.Windows.Forms.DataGridView dgvPrintSN;
        private System.Windows.Forms.CheckBox cbxPrintDate;
    }
}
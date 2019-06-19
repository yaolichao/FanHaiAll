namespace FanHai.Hemera.Addins.FMM
{
    partial class ShiftEditDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ceOverDay = new DevExpress.XtraEditors.CheckEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.lueEndTime = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEndTime = new DevExpress.XtraEditors.LabelControl();
            this.lueStartTime = new DevExpress.XtraEditors.LookUpEdit();
            this.lblStartTime = new DevExpress.XtraEditors.LabelControl();
            this.txtDes = new DevExpress.XtraEditors.MemoEdit();
            this.lblDes = new DevExpress.XtraEditors.LabelControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceOverDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(507, 334);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 4);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(501, 326);
            this.groupControl1.TabIndex = 11;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.Controls.Add(this.ceOverDay);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.lueEndTime);
            this.panelControl1.Controls.Add(this.lblEndTime);
            this.panelControl1.Controls.Add(this.lueStartTime);
            this.panelControl1.Controls.Add(this.lblStartTime);
            this.panelControl1.Controls.Add(this.txtDes);
            this.panelControl1.Controls.Add(this.lblDes);
            this.panelControl1.Controls.Add(this.txtName);
            this.panelControl1.Controls.Add(this.lblName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(497, 296);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // ceOverDay
            // 
            this.ceOverDay.Location = new System.Drawing.Point(163, 202);
            this.ceOverDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ceOverDay.Name = "ceOverDay";
            this.ceOverDay.Properties.Caption = "是否跨天";
            this.ceOverDay.Size = new System.Drawing.Size(99, 22);
            this.ceOverDay.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(341, 249);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 32);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(210, 249);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 32);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lueEndTime
            // 
            this.lueEndTime.Location = new System.Drawing.Point(163, 163);
            this.lueEndTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueEndTime.Name = "lueEndTime";
            this.lueEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueEndTime.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", " ")});
            this.lueEndTime.Properties.NullText = "";
            this.lueEndTime.Size = new System.Drawing.Size(286, 24);
            this.lueEndTime.TabIndex = 7;
            // 
            // lblEndTime
            // 
            this.lblEndTime.Location = new System.Drawing.Point(33, 167);
            this.lblEndTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(60, 18);
            this.lblEndTime.TabIndex = 6;
            this.lblEndTime.Text = "结束时间";
            // 
            // lueStartTime
            // 
            this.lueStartTime.Location = new System.Drawing.Point(163, 125);
            this.lueStartTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueStartTime.Name = "lueStartTime";
            this.lueStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueStartTime.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", " ")});
            this.lueStartTime.Properties.NullText = "";
            this.lueStartTime.Size = new System.Drawing.Size(286, 24);
            this.lueStartTime.TabIndex = 5;
            // 
            // lblStartTime
            // 
            this.lblStartTime.Location = new System.Drawing.Point(33, 129);
            this.lblStartTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(60, 18);
            this.lblStartTime.TabIndex = 4;
            this.lblStartTime.Text = "开始时间";
            // 
            // txtDes
            // 
            this.txtDes.Location = new System.Drawing.Point(163, 58);
            this.txtDes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDes.Name = "txtDes";
            this.txtDes.Size = new System.Drawing.Size(286, 54);
            this.txtDes.TabIndex = 3;
            // 
            // lblDes
            // 
            this.lblDes.Location = new System.Drawing.Point(33, 62);
            this.lblDes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblDes.Name = "lblDes";
            this.lblDes.Size = new System.Drawing.Size(60, 18);
            this.lblDes.TabIndex = 2;
            this.lblDes.Text = "描      述";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(163, 19);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 24);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(33, 23);
            this.lblName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 18);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名      称";
            // 
            // ShiftEditDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 334);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ShiftEditDialog";
            this.Load += new System.EventHandler(this.ShiftEditDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceOverDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit ceOverDay;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LookUpEdit lueEndTime;
        private DevExpress.XtraEditors.LabelControl lblEndTime;
        private DevExpress.XtraEditors.LookUpEdit lueStartTime;
        private DevExpress.XtraEditors.LabelControl lblStartTime;
        private DevExpress.XtraEditors.MemoEdit txtDes;
        private DevExpress.XtraEditors.LabelControl lblDes;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl lblName;


    }
}
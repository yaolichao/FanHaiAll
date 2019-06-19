namespace FanHai.Hemera.Addins.BasicData
{
    partial class ColumnAddOrEdit
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
            this.tbColumnKey = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.baseInforGroup = new DevExpress.XtraEditors.GroupControl();
            this.tbDesc = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.cbbDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.tbName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.baseInforGroup)).BeginInit();
            this.baseInforGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDataType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tbColumnKey
            // 
            this.tbColumnKey.Location = new System.Drawing.Point(107, 177);
            this.tbColumnKey.Name = "tbColumnKey";
            this.tbColumnKey.Size = new System.Drawing.Size(79, 22);
            this.tbColumnKey.TabIndex = 8;
            this.tbColumnKey.Visible = false;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(34, 77);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(67, 14);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "数据类型：";
            // 
            // tbType
            // 
            this.tbType.Location = new System.Drawing.Point(204, 177);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(223, 22);
            this.tbType.TabIndex = 3;
            this.tbType.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "列 组 别：";
            this.label4.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(34, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(63, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "列 名 称：";
            // 
            // baseInforGroup
            // 
            this.baseInforGroup.Controls.Add(this.tbDesc);
            this.baseInforGroup.Controls.Add(this.label1);
            this.baseInforGroup.Controls.Add(this.tbColumnKey);
            this.baseInforGroup.Controls.Add(this.btnCancel);
            this.baseInforGroup.Controls.Add(this.tbType);
            this.baseInforGroup.Controls.Add(this.btnOk);
            this.baseInforGroup.Controls.Add(this.cbbDataType);
            this.baseInforGroup.Controls.Add(this.tbName);
            this.baseInforGroup.Controls.Add(this.label4);
            this.baseInforGroup.Controls.Add(this.lblName);
            this.baseInforGroup.Controls.Add(this.lblType);
            this.baseInforGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseInforGroup.Location = new System.Drawing.Point(0, 0);
            this.baseInforGroup.Name = "baseInforGroup";
            this.baseInforGroup.Size = new System.Drawing.Size(451, 178);
            this.baseInforGroup.TabIndex = 1;
            this.baseInforGroup.Text = "普通属性";
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(129, 105);
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(298, 21);
            this.tbDesc.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "描述：";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(251, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(129, 141);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbbDataType
            // 
            this.cbbDataType.Location = new System.Drawing.Point(129, 74);
            this.cbbDataType.Name = "cbbDataType";
            this.cbbDataType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbbDataType.Properties.Items.AddRange(new object[] {
            "1-INTEGER",
            "2-DATA",
            "3-DATATIME",
            "4-BOOLEAN",
            "5-STRING",
            "6-FLOAT",
            "7-SETTING",
            "8-LINKED"});
            this.cbbDataType.Size = new System.Drawing.Size(298, 21);
            this.cbbDataType.TabIndex = 6;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(129, 43);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(298, 21);
            this.tbName.TabIndex = 5;
            // 
            // ColumnAddOrEdit
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 178);
            this.Controls.Add(this.baseInforGroup);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "ColumnAddOrEdit";
            this.Load += new System.EventHandler(this.ColumnAddOrEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.baseInforGroup)).EndInit();
            this.baseInforGroup.ResumeLayout(false);
            this.baseInforGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbbDataType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox tbColumnKey;
        private DevExpress.XtraEditors.GroupControl baseInforGroup;
        private DevExpress.XtraEditors.ComboBoxEdit cbbDataType;
        private DevExpress.XtraEditors.TextEdit tbName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit tbDesc;
    }
}
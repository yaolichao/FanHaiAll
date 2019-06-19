namespace FanHai.Hemera.Addins.EMS
{
    partial class ChoiceLayoutPicDialog
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
            this.lblSelectPic = new DevExpress.XtraEditors.LabelControl();
            this.tePicPath = new DevExpress.XtraEditors.TextEdit();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tePicPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSelectPic
            // 
            this.lblSelectPic.Location = new System.Drawing.Point(12, 32);
            this.lblSelectPic.Name = "lblSelectPic";
            this.lblSelectPic.Size = new System.Drawing.Size(60, 14);
            this.lblSelectPic.TabIndex = 0;
            this.lblSelectPic.Text = "选择布局图";
            // 
            // tePicPath
            // 
            this.tePicPath.Location = new System.Drawing.Point(88, 29);
            this.tePicPath.Name = "tePicPath";
            this.tePicPath.Size = new System.Drawing.Size(321, 21);
            this.tePicPath.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(415, 27);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(69, 27);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "...";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(294, 72);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 27);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(397, 72);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ChoiceLayoutPicDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 111);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.tePicPath);
            this.Controls.Add(this.lblSelectPic);
            this.Name = "ChoiceLayoutPicDialog";
            this.Text = "导入布局图";
            ((System.ComponentModel.ISupportInitialize)(this.tePicPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblSelectPic;
        private DevExpress.XtraEditors.TextEdit tePicPath;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
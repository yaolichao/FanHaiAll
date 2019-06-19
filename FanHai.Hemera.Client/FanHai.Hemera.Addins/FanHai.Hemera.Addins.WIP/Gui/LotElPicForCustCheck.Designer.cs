namespace FanHai.Hemera.Addins.WIP
{
    partial class LotElPicForCustCheck
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
            this.picel = new System.Windows.Forms.PictureBox();
            this.lblPicAddressAndTime = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.picel)).BeginInit();
            this.SuspendLayout();
            // 
            // picel
            // 
            this.picel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picel.Location = new System.Drawing.Point(0, 0);
            this.picel.Name = "picel";
            this.picel.Size = new System.Drawing.Size(499, 345);
            this.picel.TabIndex = 0;
            this.picel.TabStop = false;
            // 
            // lblPicAddressAndTime
            // 
            this.lblPicAddressAndTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPicAddressAndTime.Location = new System.Drawing.Point(0, 0);
            this.lblPicAddressAndTime.Name = "lblPicAddressAndTime";
            this.lblPicAddressAndTime.Size = new System.Drawing.Size(118, 14);
            this.lblPicAddressAndTime.TabIndex = 1;
            this.lblPicAddressAndTime.Text = "lblPicAddressAndTime";
            // 
            // LotElPicForCustCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(499, 345);
            this.ControlBox = false;
            this.Controls.Add(this.lblPicAddressAndTime);
            this.Controls.Add(this.picel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.Name = "LotElPicForCustCheck";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LotElPicForCustCheck";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LotElPicForCustCheck_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LotElPicForCustCheck_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.picel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picel;
        private DevExpress.XtraEditors.LabelControl lblPicAddressAndTime;
    }
}
namespace FanHai.MES.IVTest
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnDataTransfer = new System.Windows.Forms.Button();
            this.grpServiceControl = new System.Windows.Forms.GroupBox();
            this.btnResetService = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.btnUninstallService = new System.Windows.Forms.Button();
            this.btnInstallService = new System.Windows.Forms.Button();
            this.grpDataTransfer = new System.Windows.Forms.GroupBox();
            this.lblLotNumber = new System.Windows.Forms.Label();
            this.txtLotNumber = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.grpServiceControl.SuspendLayout();
            this.grpDataTransfer.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFullName
            // 
            this.txtFullName.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFullName.Location = new System.Drawing.Point(17, 29);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.ReadOnly = true;
            this.txtFullName.Size = new System.Drawing.Size(544, 35);
            this.txtFullName.TabIndex = 1;
            // 
            // btnFile
            // 
            this.btnFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFile.Location = new System.Drawing.Point(566, 27);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 38);
            this.btnFile.TabIndex = 2;
            this.btnFile.Text = "打开文件";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnDataTransfer
            // 
            this.btnDataTransfer.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDataTransfer.Location = new System.Drawing.Point(566, 80);
            this.btnDataTransfer.Name = "btnDataTransfer";
            this.btnDataTransfer.Size = new System.Drawing.Size(75, 30);
            this.btnDataTransfer.TabIndex = 7;
            this.btnDataTransfer.Text = "转数据";
            this.btnDataTransfer.UseVisualStyleBackColor = true;
            this.btnDataTransfer.Click += new System.EventHandler(this.btnDataTransfer_Click);
            // 
            // grpServiceControl
            // 
            this.grpServiceControl.Controls.Add(this.btnResetService);
            this.grpServiceControl.Controls.Add(this.btnStopService);
            this.grpServiceControl.Controls.Add(this.btnUninstallService);
            this.grpServiceControl.Controls.Add(this.btnInstallService);
            this.grpServiceControl.Location = new System.Drawing.Point(12, 188);
            this.grpServiceControl.Name = "grpServiceControl";
            this.grpServiceControl.Size = new System.Drawing.Size(659, 109);
            this.grpServiceControl.TabIndex = 8;
            this.grpServiceControl.TabStop = false;
            this.grpServiceControl.Text = "服务控制";
            // 
            // btnResetService
            // 
            this.btnResetService.Location = new System.Drawing.Point(530, 38);
            this.btnResetService.Name = "btnResetService";
            this.btnResetService.Size = new System.Drawing.Size(106, 44);
            this.btnResetService.TabIndex = 4;
            this.btnResetService.Text = "重启服务";
            this.btnResetService.UseVisualStyleBackColor = true;
            this.btnResetService.Click += new System.EventHandler(this.btnResetService_Click);
            // 
            // btnStopService
            // 
            this.btnStopService.Location = new System.Drawing.Point(400, 38);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(106, 44);
            this.btnStopService.TabIndex = 2;
            this.btnStopService.Text = "停止服务";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // btnUninstallService
            // 
            this.btnUninstallService.Location = new System.Drawing.Point(153, 39);
            this.btnUninstallService.Name = "btnUninstallService";
            this.btnUninstallService.Size = new System.Drawing.Size(106, 44);
            this.btnUninstallService.TabIndex = 1;
            this.btnUninstallService.Text = "卸载服务";
            this.btnUninstallService.UseVisualStyleBackColor = true;
            this.btnUninstallService.Click += new System.EventHandler(this.btnUninstallService_Click);
            // 
            // btnInstallService
            // 
            this.btnInstallService.Location = new System.Drawing.Point(24, 39);
            this.btnInstallService.Name = "btnInstallService";
            this.btnInstallService.Size = new System.Drawing.Size(106, 44);
            this.btnInstallService.TabIndex = 0;
            this.btnInstallService.Text = "安装服务";
            this.btnInstallService.UseVisualStyleBackColor = true;
            this.btnInstallService.Click += new System.EventHandler(this.btnInstallService_Click);
            // 
            // grpDataTransfer
            // 
            this.grpDataTransfer.Controls.Add(this.lblLotNumber);
            this.grpDataTransfer.Controls.Add(this.txtLotNumber);
            this.grpDataTransfer.Controls.Add(this.lblDevice);
            this.grpDataTransfer.Controls.Add(this.cmbDevice);
            this.grpDataTransfer.Controls.Add(this.lblMsg);
            this.grpDataTransfer.Controls.Add(this.txtFullName);
            this.grpDataTransfer.Controls.Add(this.btnFile);
            this.grpDataTransfer.Controls.Add(this.btnDataTransfer);
            this.grpDataTransfer.Location = new System.Drawing.Point(12, 12);
            this.grpDataTransfer.Name = "grpDataTransfer";
            this.grpDataTransfer.Size = new System.Drawing.Size(659, 170);
            this.grpDataTransfer.TabIndex = 9;
            this.grpDataTransfer.TabStop = false;
            this.grpDataTransfer.Text = "数据手工转置";
            // 
            // lblLotNumber
            // 
            this.lblLotNumber.AutoSize = true;
            this.lblLotNumber.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLotNumber.Location = new System.Drawing.Point(207, 84);
            this.lblLotNumber.Name = "lblLotNumber";
            this.lblLotNumber.Size = new System.Drawing.Size(76, 22);
            this.lblLotNumber.TabIndex = 12;
            this.lblLotNumber.Text = "序列号";
            // 
            // txtLotNumber
            // 
            this.txtLotNumber.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLotNumber.Location = new System.Drawing.Point(290, 81);
            this.txtLotNumber.Name = "txtLotNumber";
            this.txtLotNumber.Size = new System.Drawing.Size(270, 32);
            this.txtLotNumber.TabIndex = 11;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDevice.Location = new System.Drawing.Point(16, 84);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(54, 22);
            this.lblDevice.TabIndex = 10;
            this.lblDevice.Text = "设备";
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(76, 81);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(121, 29);
            this.cmbDevice.TabIndex = 9;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(17, 55);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 12);
            this.lblMsg.TabIndex = 8;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Manz Sort Data Transfer";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 326);
            this.Controls.Add(this.grpDataTransfer);
            this.Controls.Add(this.grpServiceControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manz Sort Data Transfer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpServiceControl.ResumeLayout(false);
            this.grpDataTransfer.ResumeLayout(false);
            this.grpDataTransfer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnDataTransfer;
        private System.Windows.Forms.GroupBox grpServiceControl;
        private System.Windows.Forms.GroupBox grpDataTransfer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnResetService;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Button btnUninstallService;
        private System.Windows.Forms.Button btnInstallService;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.Label lblLotNumber;
        private System.Windows.Forms.TextBox txtLotNumber;
    }
}


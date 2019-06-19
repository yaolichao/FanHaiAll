namespace FanHai.MES.IVTest.Service
{
    partial class ProjectInstaller
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.spiDataTransfer = new System.ServiceProcess.ServiceProcessInstaller();
            this.siDataTransfer = new System.ServiceProcess.ServiceInstaller();
            // 
            // spiDataTransfer
            // 
            this.spiDataTransfer.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.spiDataTransfer.Password = null;
            this.spiDataTransfer.Username = null;
            // 
            // siDataTransfer
            // 
            this.siDataTransfer.Description = "将IV测试数据转移到SQL Server数据库中。";
            this.siDataTransfer.DisplayName = "FanHai.MES.IVTestDataTransfer";
            this.siDataTransfer.ServiceName = "IVTestDataTransfer";
            this.siDataTransfer.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.spiDataTransfer,
            this.siDataTransfer});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller spiDataTransfer;
        private System.ServiceProcess.ServiceInstaller siDataTransfer;
    }
}
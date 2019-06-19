namespace FanHai.Hemera.Addins.SAP
{
    partial class WorkOrderSendQuery
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblApplicationTitle = new DevExpress.XtraEditors.LabelControl();
            this.lcMain = new DevExpress.XtraLayout.LayoutControl();
            this.lcgMain = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(822, 60);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(605, 0);
            this.lblInfos.Size = new System.Drawing.Size(217, 54);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t操作时间：2019-05-15 11:05:54";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.PanelTitle, 0, 0);
            this.tlpMain.Controls.Add(this.lcMain, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(1, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpMain.Size = new System.Drawing.Size(822, 818);
            this.tlpMain.TabIndex = 0;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblApplicationTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelTitle.Location = new System.Drawing.Point(3, 4);
            this.PanelTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.PanelTitle.Size = new System.Drawing.Size(816, 49);
            this.PanelTitle.TabIndex = 1;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApplicationTitle.Appearance.Options.UseFont = true;
            this.lblApplicationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblApplicationTitle.Location = new System.Drawing.Point(8, 8);
            this.lblApplicationTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(162, 27);
            this.lblApplicationTitle.TabIndex = 41;
            this.lblApplicationTitle.Text = "工单下达查询";
            // 
            // lcMain
            // 
            this.lcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcMain.Location = new System.Drawing.Point(3, 61);
            this.lcMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcMain.Name = "lcMain";
            this.lcMain.Root = this.lcgMain;
            this.lcMain.Size = new System.Drawing.Size(816, 753);
            this.lcMain.TabIndex = 2;
            this.lcMain.Text = "layoutControl1";
            // 
            // lcgMain
            // 
            this.lcgMain.CustomizationFormText = "lcgMain";
            this.lcgMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgMain.GroupBordersVisible = false;
            this.lcgMain.Name = "lcgMain";
            this.lcgMain.Size = new System.Drawing.Size(816, 753);
            this.lcgMain.TextVisible = false;
            // 
            // WorkOrderSendQuery
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "WorkOrderSendQuery";
            this.Size = new System.Drawing.Size(824, 818);
            this.Controls.SetChildIndex(this.tlpMain, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraEditors.LabelControl lblApplicationTitle;
        private DevExpress.XtraLayout.LayoutControl lcMain;
        private DevExpress.XtraLayout.LayoutControlGroup lcgMain;

    }
}

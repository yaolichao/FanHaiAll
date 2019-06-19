namespace FanHai.Hemera.Addins.WIP
{
    partial class LotParamSearchDialog
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btExit = new DevExpress.XtraEditors.SimpleButton();
            this.lbLotNumber = new DevExpress.XtraEditors.LabelControl();
            this.lblLotNumber = new DevExpress.XtraEditors.LabelControl();
            this.gcParamInfo = new DevExpress.XtraGrid.GridControl();
            this.gvParamInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParamInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.gcParamInfo, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(859, 481);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btExit);
            this.panelControl1.Controls.Add(this.lbLotNumber);
            this.panelControl1.Controls.Add(this.lblLotNumber);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(853, 34);
            this.panelControl1.TabIndex = 0;
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(769, 5);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "退出";
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // lbLotNumber
            // 
            this.lbLotNumber.Location = new System.Drawing.Point(101, 10);
            this.lbLotNumber.Name = "lbLotNumber";
            this.lbLotNumber.Size = new System.Drawing.Size(0, 14);
            this.lbLotNumber.TabIndex = 1;
            // 
            // lblLotNumber
            // 
            this.lblLotNumber.Location = new System.Drawing.Point(27, 10);
            this.lblLotNumber.Name = "lblLotNumber";
            this.lblLotNumber.Size = new System.Drawing.Size(36, 14);
            this.lblLotNumber.TabIndex = 0;
            this.lblLotNumber.Text = "批次号";
            // 
            // gcParamInfo
            // 
            this.gcParamInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcParamInfo.Location = new System.Drawing.Point(3, 43);
            this.gcParamInfo.LookAndFeel.SkinName = "Coffee";
            this.gcParamInfo.MainView = this.gvParamInfo;
            this.gcParamInfo.Name = "gcParamInfo";
            this.gcParamInfo.Size = new System.Drawing.Size(853, 435);
            this.gcParamInfo.TabIndex = 1;
            this.gcParamInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvParamInfo});
            // 
            // gvParamInfo
            // 
            this.gvParamInfo.GridControl = this.gcParamInfo;
            this.gvParamInfo.Name = "gvParamInfo";
            this.gvParamInfo.OptionsBehavior.ReadOnly = true;
            this.gvParamInfo.OptionsView.AllowCellMerge = true;
            this.gvParamInfo.OptionsView.ColumnAutoWidth = false;
            this.gvParamInfo.OptionsView.ShowGroupPanel = false;
            this.gvParamInfo.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvParamInfo_CustomDrawCell);
            // 
            // LotParamSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 481);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "LotParamSearchDialog";
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParamInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParamInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl lbLotNumber;
        private DevExpress.XtraEditors.LabelControl lblLotNumber;
        private DevExpress.XtraGrid.GridControl gcParamInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvParamInfo;
        private DevExpress.XtraEditors.SimpleButton btExit;
    }
}
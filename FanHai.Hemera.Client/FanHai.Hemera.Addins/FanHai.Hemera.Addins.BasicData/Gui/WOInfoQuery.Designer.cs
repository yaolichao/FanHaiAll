namespace SolarViewer.Hemera.Addins.BasicData.Gui
{
    partial class WOInfoQuery
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtProID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtWO = new DevExpress.XtraEditors.TextEdit();
            this.lblWO = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gcWOInfo = new DevExpress.XtraGrid.GridControl();
            this.gvWOInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.i_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.s_wo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.s_proid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcWOInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWOInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(734, 43);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(8, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(88, 24);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "工单查询";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtProID);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.btnQuery);
            this.panelControl2.Controls.Add(this.txtWO);
            this.panelControl2.Controls.Add(this.lblWO);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 43);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(734, 50);
            this.panelControl2.TabIndex = 1;
            // 
            // txtProID
            // 
            this.txtProID.Location = new System.Drawing.Point(383, 12);
            this.txtProID.Name = "txtProID";
            this.txtProID.Properties.Appearance.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProID.Properties.Appearance.Options.UseFont = true;
            this.txtProID.Size = new System.Drawing.Size(250, 23);
            this.txtProID.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(315, 15);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(69, 16);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "产品ID号：";
            // 
            // btnQuery
            // 
            this.btnQuery.Appearance.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuery.Appearance.Options.UseFont = true;
            this.btnQuery.Image = global::SolarViewer.Hemera.Addins.BasicData.Properties.Resources.select;
            this.btnQuery.Location = new System.Drawing.Point(650, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            // 
            // txtWO
            // 
            this.txtWO.Location = new System.Drawing.Point(59, 12);
            this.txtWO.Name = "txtWO";
            this.txtWO.Properties.Appearance.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWO.Properties.Appearance.Options.UseFont = true;
            this.txtWO.Size = new System.Drawing.Size(250, 23);
            this.txtWO.TabIndex = 1;
            // 
            // lblWO
            // 
            this.lblWO.Appearance.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWO.Appearance.Options.UseFont = true;
            this.lblWO.Location = new System.Drawing.Point(8, 15);
            this.lblWO.Name = "lblWO";
            this.lblWO.Size = new System.Drawing.Size(56, 16);
            this.lblWO.TabIndex = 0;
            this.lblWO.Text = "工单号：";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gcWOInfo);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 93);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(734, 367);
            this.panelControl3.TabIndex = 2;
            // 
            // gcWOInfo
            // 
            this.gcWOInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcWOInfo.Location = new System.Drawing.Point(2, 2);
            this.gcWOInfo.MainView = this.gvWOInfo;
            this.gcWOInfo.Name = "gcWOInfo";
            this.gcWOInfo.Size = new System.Drawing.Size(730, 363);
            this.gcWOInfo.TabIndex = 0;
            this.gcWOInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvWOInfo,
            this.bandedGridView1});
            // 
            // gvWOInfo
            // 
            this.gvWOInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.i_no,
            this.s_wo,
            this.s_proid});
            this.gvWOInfo.GridControl = this.gcWOInfo;
            this.gvWOInfo.Name = "gvWOInfo";
            // 
            // i_no
            // 
            this.i_no.Caption = "序号";
            this.i_no.Name = "i_no";
            this.i_no.Visible = true;
            this.i_no.VisibleIndex = 0;
            // 
            // s_wo
            // 
            this.s_wo.Caption = "工单号";
            this.s_wo.Name = "s_wo";
            this.s_wo.Visible = true;
            this.s_wo.VisibleIndex = 1;
            // 
            // s_proid
            // 
            this.s_proid.Caption = "产品ID号";
            this.s_proid.Name = "s_proid";
            this.s_proid.Visible = true;
            this.s_proid.VisibleIndex = 2;
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.bandedGridView1.GridControl = this.gcWOInfo;
            this.bandedGridView1.Name = "bandedGridView1";
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Name = "gridBand1";
            // 
            // WOInfoQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "WOInfoQuery";
            this.Size = new System.Drawing.Size(734, 460);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcWOInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvWOInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl lblWO;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtWO;
        private DevExpress.XtraEditors.TextEdit txtProID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.GridControl gcWOInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvWOInfo;
        private DevExpress.XtraGrid.Columns.GridColumn i_no;
        private DevExpress.XtraGrid.Columns.GridColumn s_wo;
        private DevExpress.XtraGrid.Columns.GridColumn s_proid;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
    }
}

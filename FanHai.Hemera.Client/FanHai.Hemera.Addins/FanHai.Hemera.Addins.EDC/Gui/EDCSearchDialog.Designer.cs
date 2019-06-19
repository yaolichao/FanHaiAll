namespace FanHai.Hemera.Addins.EDC
{
    partial class EDCSearchDialog
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtEdcName = new DevExpress.XtraEditors.TextEdit();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.grdCtrlEdc = new DevExpress.XtraGrid.GridControl();
            this.gridViewEdc = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_EdcKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_EdcName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_EdcDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdcName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlEdc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEdc)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnQuery);
            this.groupControl1.Controls.Add(this.txtEdcName);
            this.groupControl1.Controls.Add(this.lblName);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(695, 65);
            this.groupControl1.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(571, 30);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtEdcName
            // 
            this.txtEdcName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEdcName.Location = new System.Drawing.Point(88, 32);
            this.txtEdcName.Name = "txtEdcName";
            this.txtEdcName.Size = new System.Drawing.Size(464, 21);
            this.txtEdcName.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.Location = new System.Drawing.Point(36, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(24, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名称";
            // 
            // grdCtrlEdc
            // 
            this.grdCtrlEdc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCtrlEdc.Location = new System.Drawing.Point(3, 74);
            this.grdCtrlEdc.MainView = this.gridViewEdc;
            this.grdCtrlEdc.Name = "grdCtrlEdc";
            this.grdCtrlEdc.Size = new System.Drawing.Size(695, 324);
            this.grdCtrlEdc.TabIndex = 3;
            this.grdCtrlEdc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEdc});
            // 
            // gridViewEdc
            // 
            this.gridViewEdc.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_EdcKey,
            this.gridColumn_EdcName,
            this.gridColumn_EdcDescription});
            this.gridViewEdc.GridControl = this.grdCtrlEdc;
            this.gridViewEdc.Name = "gridViewEdc";
            this.gridViewEdc.OptionsBehavior.Editable = false;
            this.gridViewEdc.DoubleClick += new System.EventHandler(this.gridViewEdc_DoubleClick);
            // 
            // gridColumn_EdcKey
            // 
            this.gridColumn_EdcKey.Caption = "EDC_KEY";
            this.gridColumn_EdcKey.FieldName = "EDC_KEY";
            this.gridColumn_EdcKey.Name = "gridColumn_EdcKey";
            // 
            // gridColumn_EdcName
            // 
            this.gridColumn_EdcName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EdcName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EdcName.Caption = "名称";
            this.gridColumn_EdcName.FieldName = "EDC_NAME";
            this.gridColumn_EdcName.Name = "gridColumn_EdcName";
            this.gridColumn_EdcName.Visible = true;
            this.gridColumn_EdcName.VisibleIndex = 0;
            // 
            // gridColumn_EdcDescription
            // 
            this.gridColumn_EdcDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_EdcDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_EdcDescription.Caption = "描述";
            this.gridColumn_EdcDescription.FieldName = "DESCRIPTIONS";
            this.gridColumn_EdcDescription.Name = "gridColumn_EdcDescription";
            this.gridColumn_EdcDescription.Visible = true;
            this.gridColumn_EdcDescription.VisibleIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdCtrlEdc, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(701, 449);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Location = new System.Drawing.Point(3, 404);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(695, 42);
            this.panelControl1.TabIndex = 4;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(500, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(596, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // EDCSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 449);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "EDCSearchDialog";
            this.Load += new System.EventHandler(this.EDCSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdcName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlEdc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEdc)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtEdcName;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraGrid.GridControl grdCtrlEdc;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewEdc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EdcKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EdcName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_EdcDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
namespace FanHai.Hemera.Addins.EDC
{
    partial class SampSearchDialog
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
            this.txtSpName = new DevExpress.XtraEditors.TextEdit();
            this.lblSpName = new DevExpress.XtraEditors.LabelControl();
            this.grdCtrlSamp = new DevExpress.XtraGrid.GridControl();
            this.gridViewSamp = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_SpKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SpName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SpDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlSamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSamp)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnQuery);
            this.groupControl1.Controls.Add(this.txtSpName);
            this.groupControl1.Controls.Add(this.lblSpName);
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(695, 69);
            this.groupControl1.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(587, 29);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtSpName
            // 
            this.txtSpName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpName.Location = new System.Drawing.Point(77, 31);
            this.txtSpName.Name = "txtSpName";
            this.txtSpName.Size = new System.Drawing.Size(499, 21);
            this.txtSpName.TabIndex = 0;
            // 
            // lblSpName
            // 
            this.lblSpName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpName.Location = new System.Drawing.Point(12, 32);
            this.lblSpName.Name = "lblSpName";
            this.lblSpName.Size = new System.Drawing.Size(24, 14);
            this.lblSpName.TabIndex = 0;
            this.lblSpName.Text = "名称";
            // 
            // grdCtrlSamp
            // 
            this.grdCtrlSamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCtrlSamp.Location = new System.Drawing.Point(3, 78);
            this.grdCtrlSamp.MainView = this.gridViewSamp;
            this.grdCtrlSamp.Name = "grdCtrlSamp";
            this.grdCtrlSamp.Size = new System.Drawing.Size(695, 312);
            this.grdCtrlSamp.TabIndex = 3;
            this.grdCtrlSamp.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSamp});
            // 
            // gridViewSamp
            // 
            this.gridViewSamp.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_SpKey,
            this.gridColumn_SpName,
            this.gridColumn_SpDescription});
            this.gridViewSamp.GridControl = this.grdCtrlSamp;
            this.gridViewSamp.Name = "gridViewSamp";
            this.gridViewSamp.OptionsBehavior.Editable = false;
            this.gridViewSamp.DoubleClick += new System.EventHandler(this.gridViewEdc_DoubleClick);
            // 
            // gridColumn_SpKey
            // 
            this.gridColumn_SpKey.Caption = "SP_KEY";
            this.gridColumn_SpKey.FieldName = "SP_KEY";
            this.gridColumn_SpKey.Name = "gridColumn_SpKey";
            // 
            // gridColumn_SpName
            // 
            this.gridColumn_SpName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SpName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SpName.Caption = "名称";
            this.gridColumn_SpName.FieldName = "SP_NAME";
            this.gridColumn_SpName.Name = "gridColumn_SpName";
            this.gridColumn_SpName.Visible = true;
            this.gridColumn_SpName.VisibleIndex = 0;
            // 
            // gridColumn_SpDescription
            // 
            this.gridColumn_SpDescription.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SpDescription.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SpDescription.Caption = "描述";
            this.gridColumn_SpDescription.FieldName = "DESCRIPTIONS";
            this.gridColumn_SpDescription.Name = "gridColumn_SpDescription";
            this.gridColumn_SpDescription.Visible = true;
            this.gridColumn_SpDescription.VisibleIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdCtrlSamp, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(701, 449);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 396);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(695, 50);
            this.panelControl1.TabIndex = 10;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(484, 16);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(584, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // SampSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 449);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "SampSearchDialog";
            this.Load += new System.EventHandler(this.SampSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlSamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSamp)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtSpName;
        private DevExpress.XtraEditors.LabelControl lblSpName;
        private DevExpress.XtraGrid.GridControl grdCtrlSamp;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewSamp;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SpKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SpName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SpDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}
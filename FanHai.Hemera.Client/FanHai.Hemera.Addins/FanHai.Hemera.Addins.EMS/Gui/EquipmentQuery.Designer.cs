namespace FanHai.Hemera.Addins.EMS
{
    partial class EquipmentQuery
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
            this.grpEquipmentQuery = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.lblQueryName = new DevExpress.XtraEditors.LabelControl();
            this.txtQueryValue = new DevExpress.XtraEditors.TextEdit();
            this.grpEquipmentList = new DevExpress.XtraEditors.GroupControl();
            this.grdEquipments = new DevExpress.XtraGrid.GridControl();
            this.grdViewEquipments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.paginationEquipments = new FanHai.Hemera.Utils.Controls.PaginationControl();
            this.pnlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentQuery)).BeginInit();
            this.grpEquipmentQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentList)).BeginInit();
            this.grpEquipmentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEquipments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEquipments)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpEquipmentQuery
            // 
            this.grpEquipmentQuery.Controls.Add(this.btnQuery);
            this.grpEquipmentQuery.Controls.Add(this.lblQueryName);
            this.grpEquipmentQuery.Controls.Add(this.txtQueryValue);
            this.grpEquipmentQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEquipmentQuery.Location = new System.Drawing.Point(3, 3);
            this.grpEquipmentQuery.Name = "grpEquipmentQuery";
            this.grpEquipmentQuery.Size = new System.Drawing.Size(692, 74);
            this.grpEquipmentQuery.TabIndex = 58;
            this.grpEquipmentQuery.Text = "Equipment Query";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(580, 36);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 80;
            this.btnQuery.Text = "Query";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblQueryName
            // 
            this.lblQueryName.Appearance.Options.UseTextOptions = true;
            this.lblQueryName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblQueryName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblQueryName.Location = new System.Drawing.Point(6, 39);
            this.lblQueryName.Name = "lblQueryName";
            this.lblQueryName.Size = new System.Drawing.Size(94, 14);
            this.lblQueryName.TabIndex = 78;
            this.lblQueryName.Text = "Equipment Name";
            // 
            // txtQueryValue
            // 
            this.txtQueryValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryValue.EditValue = "";
            this.txtQueryValue.EnterMoveNextControl = true;
            this.txtQueryValue.Location = new System.Drawing.Point(154, 39);
            this.txtQueryValue.Name = "txtQueryValue";
            this.txtQueryValue.Properties.MaxLength = 50;
            this.txtQueryValue.Size = new System.Drawing.Size(416, 21);
            this.txtQueryValue.TabIndex = 79;
            // 
            // grpEquipmentList
            // 
            this.grpEquipmentList.Controls.Add(this.grdEquipments);
            this.grpEquipmentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEquipmentList.Location = new System.Drawing.Point(3, 83);
            this.grpEquipmentList.Name = "grpEquipmentList";
            this.grpEquipmentList.Size = new System.Drawing.Size(692, 261);
            this.grpEquipmentList.TabIndex = 59;
            this.grpEquipmentList.Text = "Equipment List";
            // 
            // grdEquipments
            // 
            this.grdEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEquipments.Location = new System.Drawing.Point(2, 23);
            this.grdEquipments.LookAndFeel.SkinName = "Coffee";
            this.grdEquipments.MainView = this.grdViewEquipments;
            this.grdEquipments.Name = "grdEquipments";
            this.grdEquipments.Size = new System.Drawing.Size(688, 236);
            this.grdEquipments.TabIndex = 4;
            this.grdEquipments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewEquipments});
            this.grdEquipments.DoubleClick += new System.EventHandler(this.grdEquipments_DoubleClick);
            // 
            // grdViewEquipments
            // 
            this.grdViewEquipments.GridControl = this.grdEquipments;
            this.grdViewEquipments.Name = "grdViewEquipments";
            this.grdViewEquipments.OptionsBehavior.Editable = false;
            this.grdViewEquipments.OptionsBehavior.ReadOnly = true;
            this.grdViewEquipments.OptionsView.ColumnAutoWidth = false;
            this.grdViewEquipments.OptionsView.RowAutoHeight = true;
            this.grdViewEquipments.OptionsView.ShowGroupPanel = false;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.paginationEquipments, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.grpEquipmentList, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.grpEquipmentQuery, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pnlBottom, 0, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(698, 448);
            this.tableLayoutPanelMain.TabIndex = 61;
            // 
            // paginationEquipments
            // 
            this.paginationEquipments.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.paginationEquipments.Appearance.Options.UseBackColor = true;
            this.paginationEquipments.AutoSize = true;
            this.paginationEquipments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.paginationEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paginationEquipments.Location = new System.Drawing.Point(3, 350);
            this.paginationEquipments.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.paginationEquipments.Name = "paginationEquipments";
            this.paginationEquipments.PageNo = 1;
            this.paginationEquipments.Pages = 0;
            this.paginationEquipments.PageSize = 20;
            this.paginationEquipments.Records = 0;
            this.paginationEquipments.Size = new System.Drawing.Size(692, 42);
            this.paginationEquipments.TabIndex = 81;
            this.paginationEquipments.DataPaging += new FanHai.Hemera.Utils.Controls.Paging(this.paginationEquipments_DataPaging);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnConfirm);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 398);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(692, 47);
            this.pnlBottom.TabIndex = 82;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(580, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(480, 13);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // EquipmentQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 448);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "EquipmentQuery";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.EquipmentQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentQuery)).EndInit();
            this.grpEquipmentQuery.ResumeLayout(false);
            this.grpEquipmentQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpEquipmentList)).EndInit();
            this.grpEquipmentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEquipments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEquipments)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpEquipmentQuery;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.LabelControl lblQueryName;
        private DevExpress.XtraEditors.TextEdit txtQueryValue;
        private DevExpress.XtraEditors.GroupControl grpEquipmentList;
        private DevExpress.XtraGrid.GridControl grdEquipments;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewEquipments;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private FanHai.Hemera.Utils.Controls.PaginationControl paginationEquipments;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.PanelControl pnlBottom;
    }
}
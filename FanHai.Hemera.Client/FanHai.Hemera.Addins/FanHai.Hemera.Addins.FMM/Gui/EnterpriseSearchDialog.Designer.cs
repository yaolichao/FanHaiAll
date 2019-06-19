namespace FanHai.Hemera.Addins.FMM
{
    partial class EnterpriseSearchDialog
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
            this.PanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupControlTop = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.grdCtrlCommon = new DevExpress.XtraGrid.GridControl();
            this.grdViewCommon = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Version = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Creator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.PanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).BeginInit();
            this.groupControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewCommon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelMain
            // 
            this.PanelMain.ColumnCount = 1;
            this.PanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelMain.Controls.Add(this.groupControlTop, 0, 0);
            this.PanelMain.Controls.Add(this.grdCtrlCommon, 0, 1);
            this.PanelMain.Controls.Add(this.panelControlBottom, 0, 2);
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 0);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.RowCount = 3;
            this.PanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.PanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.PanelMain.Size = new System.Drawing.Size(692, 390);
            this.PanelMain.TabIndex = 1;
            // 
            // groupControlTop
            // 
            this.groupControlTop.Controls.Add(this.btnQuery);
            this.groupControlTop.Controls.Add(this.txtName);
            this.groupControlTop.Controls.Add(this.lblName);
            this.groupControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlTop.Location = new System.Drawing.Point(3, 3);
            this.groupControlTop.Name = "groupControlTop";
            this.groupControlTop.Size = new System.Drawing.Size(686, 70);
            this.groupControlTop.TabIndex = 10;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(587, 35);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(77, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(499, 20);
            this.txtName.TabIndex = 0;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.Location = new System.Drawing.Point(12, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(24, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名称";
            // 
            // grdCtrlCommon
            // 
            this.grdCtrlCommon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCtrlCommon.Location = new System.Drawing.Point(3, 79);
            this.grdCtrlCommon.MainView = this.grdViewCommon;
            this.grdCtrlCommon.Name = "grdCtrlCommon";
            this.grdCtrlCommon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.grdCtrlCommon.Size = new System.Drawing.Size(686, 255);
            this.grdCtrlCommon.TabIndex = 11;
            this.grdCtrlCommon.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewCommon});
            // 
            // grdViewCommon
            // 
            this.grdViewCommon.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_Key,
            this.gridColumn_Name,
            this.gridColumn_Description,
            this.gridColumn_Version,
            this.gridColumn_Creator,
            this.gridColumn_CreateTime});
            this.grdViewCommon.GridControl = this.grdCtrlCommon;
            this.grdViewCommon.Name = "grdViewCommon";
            this.grdViewCommon.OptionsBehavior.Editable = false;
            this.grdViewCommon.DoubleClick += new System.EventHandler(this.gridViewCommon_DoubleClick);
            // 
            // gridColumn_Key
            // 
            this.gridColumn_Key.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Key.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Key.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Key.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Key.Caption = "KEY";
            this.gridColumn_Key.FieldName = "ROUTE_ENTERPRISE_VER_KEY";
            this.gridColumn_Key.Name = "gridColumn_Key";
            // 
            // gridColumn_Name
            // 
            this.gridColumn_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Name.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_Name.Caption = "名称";
            this.gridColumn_Name.FieldName = "ENTERPRISE_NAME";
            this.gridColumn_Name.Name = "gridColumn_Name";
            this.gridColumn_Name.Visible = true;
            this.gridColumn_Name.VisibleIndex = 0;
            // 
            // gridColumn_Description
            // 
            this.gridColumn_Description.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Description.Caption = "描述";
            this.gridColumn_Description.FieldName = "DESCRIPTION";
            this.gridColumn_Description.Name = "gridColumn_Description";
            this.gridColumn_Description.Visible = true;
            this.gridColumn_Description.VisibleIndex = 1;
            // 
            // gridColumn_Version
            // 
            this.gridColumn_Version.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Version.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Version.Caption = "版本";
            this.gridColumn_Version.FieldName = "ENTERPRISE_VERSION";
            this.gridColumn_Version.Name = "gridColumn_Version";
            this.gridColumn_Version.Visible = true;
            this.gridColumn_Version.VisibleIndex = 2;
            // 
            // gridColumn_Creator
            // 
            this.gridColumn_Creator.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Creator.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Creator.Caption = "操作人";
            this.gridColumn_Creator.FieldName = "CREATOR";
            this.gridColumn_Creator.Name = "gridColumn_Creator";
            this.gridColumn_Creator.Visible = true;
            this.gridColumn_Creator.VisibleIndex = 3;
            // 
            // gridColumn_CreateTime
            // 
            this.gridColumn_CreateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CreateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CreateTime.Caption = "操作时间";
            this.gridColumn_CreateTime.FieldName = "CREATE_TIME";
            this.gridColumn_CreateTime.Name = "gridColumn_CreateTime";
            this.gridColumn_CreateTime.Visible = true;
            this.gridColumn_CreateTime.VisibleIndex = 4;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.DisplayValueChecked = "1";
            this.repositoryItemCheckEdit1.DisplayValueGrayed = "1";
            this.repositoryItemCheckEdit1.DisplayValueUnchecked = "0";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnConfirm);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBottom.Location = new System.Drawing.Point(3, 340);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(686, 47);
            this.panelControlBottom.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(587, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(485, 11);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 25);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // EnterpriseSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 390);
            this.Controls.Add(this.PanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EnterpriseSearchDialog";
            this.Load += new System.EventHandler(this.EnterpriseSearchDialog_Load);
            this.PanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).EndInit();
            this.groupControlTop.ResumeLayout(false);
            this.groupControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCtrlCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewCommon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PanelMain;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraGrid.GridControl grdCtrlCommon;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewCommon;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Key;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Description;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Version;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Creator;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CreateTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.GroupControl groupControlTop;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;
    }
}
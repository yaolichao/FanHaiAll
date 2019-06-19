namespace FanHai.Hemera.Addins.RBAC
{
    partial class UserSearchDialog
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlQueryCondition = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.lblUserName = new DevExpress.XtraEditors.LabelControl();
            this.UserInfoGroup = new DevExpress.XtraEditors.GroupControl();
            this.userControl = new DevExpress.XtraGrid.GridControl();
            this.userView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.USER_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.USERNAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BADGE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EMAIL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OFFICE_PHONE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MOBILE_PHONE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.REMARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PASSWORD = new DevExpress.XtraGrid.Columns.GridColumn();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlQueryCondition)).BeginInit();
            this.groupControlQueryCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserInfoGroup)).BeginInit();
            this.UserInfoGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(this.panelControlBottom, 0, 2);
            tableLayoutPanelMain.Controls.Add(this.groupControlQueryCondition, 0, 0);
            tableLayoutPanelMain.Controls.Add(this.UserInfoGroup, 0, 1);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(791, 491);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnConfirm);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBottom.Location = new System.Drawing.Point(3, 435);
            this.panelControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(785, 52);
            this.panelControlBottom.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(675, 9);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 32);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(563, 9);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(103, 32);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // groupControlQueryCondition
            // 
            this.groupControlQueryCondition.Controls.Add(this.btnQuery);
            this.groupControlQueryCondition.Controls.Add(this.txtUserName);
            this.groupControlQueryCondition.Controls.Add(this.lblUserName);
            this.groupControlQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlQueryCondition.Location = new System.Drawing.Point(3, 4);
            this.groupControlQueryCondition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlQueryCondition.Name = "groupControlQueryCondition";
            this.groupControlQueryCondition.Size = new System.Drawing.Size(785, 95);
            this.groupControlQueryCondition.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(675, 41);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(103, 32);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(85, 45);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(582, 24);
            this.txtUserName.TabIndex = 1;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(16, 49);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 18);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "用户名：";
            // 
            // UserInfoGroup
            // 
            this.UserInfoGroup.Controls.Add(this.userControl);
            this.UserInfoGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserInfoGroup.Location = new System.Drawing.Point(3, 107);
            this.UserInfoGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UserInfoGroup.Name = "UserInfoGroup";
            this.UserInfoGroup.Size = new System.Drawing.Size(785, 320);
            this.UserInfoGroup.TabIndex = 1;
            this.UserInfoGroup.Text = "检索到的数据";
            // 
            // userControl
            // 
            this.userControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userControl.Location = new System.Drawing.Point(2, 28);
            this.userControl.MainView = this.userView;
            this.userControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userControl.Name = "userControl";
            this.userControl.Size = new System.Drawing.Size(781, 290);
            this.userControl.TabIndex = 1;
            this.userControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.userView});
            // 
            // userView
            // 
            this.userView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.USER_KEY,
            this.USERNAME,
            this.BADGE,
            this.EMAIL,
            this.OFFICE_PHONE,
            this.MOBILE_PHONE,
            this.REMARK,
            this.PASSWORD});
            this.userView.DetailHeight = 450;
            this.userView.FixedLineWidth = 3;
            this.userView.GridControl = this.userControl;
            this.userView.Name = "userView";
            this.userView.OptionsView.ShowGroupPanel = false;
            this.userView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.userView_CustomDrawRowIndicator);
            this.userView.DoubleClick += new System.EventHandler(this.userView_DoubleClick);
            // 
            // USER_KEY
            // 
            this.USER_KEY.AppearanceHeader.Options.UseTextOptions = true;
            this.USER_KEY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.USER_KEY.Caption = "用户ID";
            this.USER_KEY.FieldName = "USER_KEY";
            this.USER_KEY.MinWidth = 23;
            this.USER_KEY.Name = "USER_KEY";
            this.USER_KEY.OptionsColumn.AllowEdit = false;
            this.USER_KEY.Width = 86;
            // 
            // USERNAME
            // 
            this.USERNAME.AppearanceHeader.Options.UseTextOptions = true;
            this.USERNAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.USERNAME.Caption = "用户名";
            this.USERNAME.FieldName = "USERNAME";
            this.USERNAME.MinWidth = 23;
            this.USERNAME.Name = "USERNAME";
            this.USERNAME.OptionsColumn.AllowEdit = false;
            this.USERNAME.Visible = true;
            this.USERNAME.VisibleIndex = 0;
            this.USERNAME.Width = 86;
            // 
            // BADGE
            // 
            this.BADGE.AppearanceHeader.Options.UseTextOptions = true;
            this.BADGE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.BADGE.Caption = "工号";
            this.BADGE.FieldName = "BADGE";
            this.BADGE.MinWidth = 23;
            this.BADGE.Name = "BADGE";
            this.BADGE.OptionsColumn.AllowEdit = false;
            this.BADGE.Visible = true;
            this.BADGE.VisibleIndex = 1;
            this.BADGE.Width = 86;
            // 
            // EMAIL
            // 
            this.EMAIL.AppearanceHeader.Options.UseTextOptions = true;
            this.EMAIL.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.EMAIL.Caption = "邮箱";
            this.EMAIL.FieldName = "EMAIL";
            this.EMAIL.MinWidth = 23;
            this.EMAIL.Name = "EMAIL";
            this.EMAIL.OptionsColumn.AllowEdit = false;
            this.EMAIL.Visible = true;
            this.EMAIL.VisibleIndex = 2;
            this.EMAIL.Width = 86;
            // 
            // OFFICE_PHONE
            // 
            this.OFFICE_PHONE.AppearanceHeader.Options.UseTextOptions = true;
            this.OFFICE_PHONE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OFFICE_PHONE.Caption = "办公电话";
            this.OFFICE_PHONE.FieldName = "OFFICE_PHONE";
            this.OFFICE_PHONE.MinWidth = 23;
            this.OFFICE_PHONE.Name = "OFFICE_PHONE";
            this.OFFICE_PHONE.OptionsColumn.AllowEdit = false;
            this.OFFICE_PHONE.Visible = true;
            this.OFFICE_PHONE.VisibleIndex = 3;
            this.OFFICE_PHONE.Width = 86;
            // 
            // MOBILE_PHONE
            // 
            this.MOBILE_PHONE.AppearanceHeader.Options.UseTextOptions = true;
            this.MOBILE_PHONE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MOBILE_PHONE.Caption = "移动电话";
            this.MOBILE_PHONE.FieldName = "MOBILE_PHONE";
            this.MOBILE_PHONE.MinWidth = 23;
            this.MOBILE_PHONE.Name = "MOBILE_PHONE";
            this.MOBILE_PHONE.OptionsColumn.AllowEdit = false;
            this.MOBILE_PHONE.Visible = true;
            this.MOBILE_PHONE.VisibleIndex = 4;
            this.MOBILE_PHONE.Width = 86;
            // 
            // REMARK
            // 
            this.REMARK.AppearanceHeader.Options.UseTextOptions = true;
            this.REMARK.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.REMARK.Caption = "备注";
            this.REMARK.FieldName = "REMARK";
            this.REMARK.MinWidth = 23;
            this.REMARK.Name = "REMARK";
            this.REMARK.OptionsColumn.AllowEdit = false;
            this.REMARK.Visible = true;
            this.REMARK.VisibleIndex = 5;
            this.REMARK.Width = 86;
            // 
            // PASSWORD
            // 
            this.PASSWORD.AppearanceHeader.Options.UseTextOptions = true;
            this.PASSWORD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.PASSWORD.Caption = "密码";
            this.PASSWORD.FieldName = "PASSWORD";
            this.PASSWORD.MinWidth = 23;
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.Width = 86;
            // 
            // UserSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 491);
            this.Controls.Add(tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "UserSearchDialog";
            this.Load += new System.EventHandler(this.UserSearchDialog_Load);
            tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlQueryCondition)).EndInit();
            this.groupControlQueryCondition.ResumeLayout(false);
            this.groupControlQueryCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserInfoGroup)).EndInit();
            this.UserInfoGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlQueryCondition;
        private DevExpress.XtraEditors.LabelControl lblUserName;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DevExpress.XtraEditors.GroupControl UserInfoGroup;
        private DevExpress.XtraGrid.GridControl userControl;
        private DevExpress.XtraGrid.Views.Grid.GridView userView;
        private DevExpress.XtraGrid.Columns.GridColumn USER_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn USERNAME;
        private DevExpress.XtraGrid.Columns.GridColumn BADGE;
        private DevExpress.XtraGrid.Columns.GridColumn EMAIL;
        private DevExpress.XtraGrid.Columns.GridColumn OFFICE_PHONE;
        private DevExpress.XtraGrid.Columns.GridColumn MOBILE_PHONE;
        private DevExpress.XtraGrid.Columns.GridColumn REMARK;
        private DevExpress.XtraGrid.Columns.GridColumn PASSWORD;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
    }
}
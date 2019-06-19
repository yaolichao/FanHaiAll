namespace FanHai.Hemera.Addins.RBAC
{
    partial class RoleUserDialog
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
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.lblRoleName = new DevExpress.XtraEditors.LabelControl();
            this.txtRoleName = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.groupControlMiddle = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanelMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeleteUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddUser = new DevExpress.XtraEditors.SimpleButton();
            this.UnSelectGroup = new DevExpress.XtraEditors.GroupControl();
            this.gcUnSelect = new DevExpress.XtraGrid.GridControl();
            this.gvUnSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.UnSelectCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.unSelectCE = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.UserKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.UserBadge = new DevExpress.XtraGrid.Columns.GridColumn();
            this.selectGroup = new DevExpress.XtraEditors.GroupControl();
            this.gcSelect = new DevExpress.XtraGrid.GridControl();
            this.gvSelect = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SELECT_CHECK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.selectCE = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.USER_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.USER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.USER_BADGE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).BeginInit();
            this.tableLayoutPanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMiddle)).BeginInit();
            this.groupControlMiddle.SuspendLayout();
            this.tableLayoutPanelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).BeginInit();
            this.UnSelectGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcUnSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUnSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unSelectCE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).BeginInit();
            this.selectGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectCE)).BeginInit();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelControlTop, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelLeft, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelBottom, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(829, 752);
            this.tableLayoutPanelMain.TabIndex = 10;
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.lblRoleName);
            this.panelControlTop.Controls.Add(this.txtRoleName);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlTop.Location = new System.Drawing.Point(3, 4);
            this.panelControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(823, 79);
            this.panelControlTop.TabIndex = 2;
            // 
            // lblRoleName
            // 
            this.lblRoleName.Location = new System.Drawing.Point(10, 31);
            this.lblRoleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(60, 18);
            this.lblRoleName.TabIndex = 0;
            this.lblRoleName.Text = "角色名：";
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(72, 27);
            this.txtRoleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(744, 24);
            this.txtRoleName.TabIndex = 1;
            // 
            // tableLayoutPanelLeft
            // 
            this.tableLayoutPanelLeft.ColumnCount = 3;
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.Controls.Add(this.groupControlMiddle, 1, 0);
            this.tableLayoutPanelLeft.Controls.Add(this.UnSelectGroup, 2, 0);
            this.tableLayoutPanelLeft.Controls.Add(this.selectGroup, 0, 0);
            this.tableLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLeft.Location = new System.Drawing.Point(3, 91);
            this.tableLayoutPanelLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelLeft.Name = "tableLayoutPanelLeft";
            this.tableLayoutPanelLeft.RowCount = 1;
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 595F));
            this.tableLayoutPanelLeft.Size = new System.Drawing.Size(823, 595);
            this.tableLayoutPanelLeft.TabIndex = 1;
            // 
            // groupControlMiddle
            // 
            this.groupControlMiddle.Controls.Add(this.tableLayoutPanelMiddle);
            this.groupControlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlMiddle.Location = new System.Drawing.Point(359, 4);
            this.groupControlMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlMiddle.Name = "groupControlMiddle";
            this.groupControlMiddle.ShowCaption = false;
            this.groupControlMiddle.Size = new System.Drawing.Size(104, 587);
            this.groupControlMiddle.TabIndex = 13;
            this.groupControlMiddle.Text = "groupControl1";
            // 
            // tableLayoutPanelMiddle
            // 
            this.tableLayoutPanelMiddle.ColumnCount = 1;
            this.tableLayoutPanelMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMiddle.Controls.Add(this.btnDeleteUser, 0, 1);
            this.tableLayoutPanelMiddle.Controls.Add(this.btnAddUser, 0, 2);
            this.tableLayoutPanelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMiddle.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMiddle.Name = "tableLayoutPanelMiddle";
            this.tableLayoutPanelMiddle.RowCount = 4;
            this.tableLayoutPanelMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanelMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.tableLayoutPanelMiddle.Size = new System.Drawing.Size(100, 583);
            this.tableLayoutPanelMiddle.TabIndex = 9;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Location = new System.Drawing.Point(3, 260);
            this.btnDeleteUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(78, 40);
            this.btnDeleteUser.TabIndex = 4;
            this.btnDeleteUser.Text = ">>";
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(3, 324);
            this.btnAddUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(79, 40);
            this.btnAddUser.TabIndex = 5;
            this.btnAddUser.Text = "<<";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // UnSelectGroup
            // 
            this.UnSelectGroup.Controls.Add(this.gcUnSelect);
            this.UnSelectGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnSelectGroup.Location = new System.Drawing.Point(469, 4);
            this.UnSelectGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UnSelectGroup.Name = "UnSelectGroup";
            this.UnSelectGroup.Size = new System.Drawing.Size(351, 587);
            this.UnSelectGroup.TabIndex = 14;
            this.UnSelectGroup.Text = "未选用户";
            // 
            // gcUnSelect
            // 
            this.gcUnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcUnSelect.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcUnSelect.Location = new System.Drawing.Point(2, 28);
            this.gcUnSelect.LookAndFeel.SkinName = "Coffee";
            this.gcUnSelect.MainView = this.gvUnSelect;
            this.gcUnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcUnSelect.Name = "gcUnSelect";
            this.gcUnSelect.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.unSelectCE});
            this.gcUnSelect.Size = new System.Drawing.Size(347, 557);
            this.gcUnSelect.TabIndex = 2;
            this.gcUnSelect.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUnSelect});
            // 
            // gvUnSelect
            // 
            this.gvUnSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.UnSelectCheck,
            this.UserKey,
            this.UserName,
            this.UserBadge});
            this.gvUnSelect.DetailHeight = 450;
            this.gvUnSelect.FixedLineWidth = 3;
            this.gvUnSelect.GridControl = this.gcUnSelect;
            this.gvUnSelect.Name = "gvUnSelect";
            this.gvUnSelect.OptionsView.ShowGroupPanel = false;
            // 
            // UnSelectCheck
            // 
            this.UnSelectCheck.Caption = " ";
            this.UnSelectCheck.ColumnEdit = this.unSelectCE;
            this.UnSelectCheck.FieldName = "COLUMN_CHECK";
            this.UnSelectCheck.MinWidth = 23;
            this.UnSelectCheck.Name = "UnSelectCheck";
            this.UnSelectCheck.Visible = true;
            this.UnSelectCheck.VisibleIndex = 0;
            this.UnSelectCheck.Width = 86;
            // 
            // unSelectCE
            // 
            this.unSelectCE.AutoHeight = false;
            this.unSelectCE.Name = "unSelectCE";
            this.unSelectCE.ValueChecked = "True";
            this.unSelectCE.ValueGrayed = "<Null>";
            this.unSelectCE.ValueUnchecked = "False";
            // 
            // UserKey
            // 
            this.UserKey.Caption = "主键";
            this.UserKey.FieldName = "USER_KEY";
            this.UserKey.MinWidth = 23;
            this.UserKey.Name = "UserKey";
            this.UserKey.Width = 86;
            // 
            // UserName
            // 
            this.UserName.Caption = "用户名";
            this.UserName.FieldName = "USERNAME";
            this.UserName.MinWidth = 23;
            this.UserName.Name = "UserName";
            this.UserName.OptionsColumn.ReadOnly = true;
            this.UserName.Visible = true;
            this.UserName.VisibleIndex = 1;
            this.UserName.Width = 86;
            // 
            // UserBadge
            // 
            this.UserBadge.Caption = "工号";
            this.UserBadge.FieldName = "BADGE";
            this.UserBadge.MinWidth = 23;
            this.UserBadge.Name = "UserBadge";
            this.UserBadge.OptionsColumn.ReadOnly = true;
            this.UserBadge.Visible = true;
            this.UserBadge.VisibleIndex = 2;
            this.UserBadge.Width = 86;
            // 
            // selectGroup
            // 
            this.selectGroup.Controls.Add(this.gcSelect);
            this.selectGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectGroup.Location = new System.Drawing.Point(3, 4);
            this.selectGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Size = new System.Drawing.Size(350, 587);
            this.selectGroup.TabIndex = 2;
            this.selectGroup.Text = "已有用户";
            // 
            // gcSelect
            // 
            this.gcSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSelect.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcSelect.Location = new System.Drawing.Point(2, 28);
            this.gcSelect.LookAndFeel.SkinName = "Coffee";
            this.gcSelect.MainView = this.gvSelect;
            this.gcSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcSelect.Name = "gcSelect";
            this.gcSelect.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.selectCE});
            this.gcSelect.Size = new System.Drawing.Size(346, 557);
            this.gcSelect.TabIndex = 0;
            this.gcSelect.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSelect});
            // 
            // gvSelect
            // 
            this.gvSelect.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SELECT_CHECK,
            this.USER_KEY,
            this.USER_NAME,
            this.USER_BADGE});
            this.gvSelect.DetailHeight = 450;
            this.gvSelect.FixedLineWidth = 3;
            this.gvSelect.GridControl = this.gcSelect;
            this.gvSelect.Name = "gvSelect";
            this.gvSelect.OptionsView.ShowGroupPanel = false;
            // 
            // SELECT_CHECK
            // 
            this.SELECT_CHECK.Caption = " ";
            this.SELECT_CHECK.ColumnEdit = this.selectCE;
            this.SELECT_CHECK.FieldName = "COLUMN_CHECK";
            this.SELECT_CHECK.MinWidth = 23;
            this.SELECT_CHECK.Name = "SELECT_CHECK";
            this.SELECT_CHECK.Visible = true;
            this.SELECT_CHECK.VisibleIndex = 0;
            this.SELECT_CHECK.Width = 86;
            // 
            // selectCE
            // 
            this.selectCE.AutoHeight = false;
            this.selectCE.Name = "selectCE";
            this.selectCE.ValueChecked = "True";
            this.selectCE.ValueGrayed = "<Null>";
            this.selectCE.ValueUnchecked = "False";
            // 
            // USER_KEY
            // 
            this.USER_KEY.Caption = "主键";
            this.USER_KEY.FieldName = "USER_KEY";
            this.USER_KEY.MinWidth = 23;
            this.USER_KEY.Name = "USER_KEY";
            this.USER_KEY.Width = 86;
            // 
            // USER_NAME
            // 
            this.USER_NAME.Caption = "用户名";
            this.USER_NAME.FieldName = "USERNAME";
            this.USER_NAME.MinWidth = 23;
            this.USER_NAME.Name = "USER_NAME";
            this.USER_NAME.OptionsColumn.ReadOnly = true;
            this.USER_NAME.Visible = true;
            this.USER_NAME.VisibleIndex = 1;
            this.USER_NAME.Width = 86;
            // 
            // USER_BADGE
            // 
            this.USER_BADGE.Caption = "工号";
            this.USER_BADGE.FieldName = "BADGE";
            this.USER_BADGE.MinWidth = 23;
            this.USER_BADGE.Name = "USER_BADGE";
            this.USER_BADGE.OptionsColumn.ReadOnly = true;
            this.USER_BADGE.Visible = true;
            this.USER_BADGE.VisibleIndex = 2;
            this.USER_BADGE.Width = 86;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.ColumnCount = 4;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.69551F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.30449F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanelBottom.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(3, 694);
            this.tableLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(823, 54);
            this.tableLayoutPanelBottom.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(572, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 37);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(700, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 37);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // RoleUserDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 752);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "RoleUserDialog";
            this.Load += new System.EventHandler(this.RoleUserDialog_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.panelControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).EndInit();
            this.tableLayoutPanelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMiddle)).EndInit();
            this.groupControlMiddle.ResumeLayout(false);
            this.tableLayoutPanelMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).EndInit();
            this.UnSelectGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcUnSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUnSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unSelectCE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).EndInit();
            this.selectGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectCE)).EndInit();
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.LabelControl lblRoleName;
        private DevExpress.XtraEditors.TextEdit txtRoleName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeft;
        private DevExpress.XtraEditors.GroupControl groupControlMiddle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMiddle;
        private DevExpress.XtraEditors.SimpleButton btnDeleteUser;
        private DevExpress.XtraEditors.SimpleButton btnAddUser;
        private DevExpress.XtraEditors.GroupControl UnSelectGroup;
        private DevExpress.XtraEditors.GroupControl selectGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraGrid.GridControl gcSelect;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSelect;
        private DevExpress.XtraGrid.Columns.GridColumn SELECT_CHECK;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit selectCE;
        private DevExpress.XtraGrid.Columns.GridColumn USER_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn USER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn USER_BADGE;
        private DevExpress.XtraGrid.GridControl gcUnSelect;
        private DevExpress.XtraGrid.Views.Grid.GridView gvUnSelect;
        private DevExpress.XtraGrid.Columns.GridColumn UnSelectCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit unSelectCE;
        private DevExpress.XtraGrid.Columns.GridColumn UserKey;
        private DevExpress.XtraGrid.Columns.GridColumn UserName;
        private DevExpress.XtraGrid.Columns.GridColumn UserBadge;
    }
}
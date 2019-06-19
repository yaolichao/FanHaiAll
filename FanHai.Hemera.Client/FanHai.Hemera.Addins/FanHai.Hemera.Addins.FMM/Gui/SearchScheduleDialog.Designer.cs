namespace FanHai.Hemera.Addins.FMM
{
    partial class SearchScheduleDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.ScheduleControl = new DevExpress.XtraGrid.GridControl();
            this.ScheduleView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.schedule_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.schedule_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MAXOVERLAPTIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.editor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.edit_timeZone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ScheduleControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(791, 504);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 441);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 59);
            this.panelControl1.TabIndex = 15;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(672, 14);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(545, 14);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(103, 32);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // ScheduleControl
            // 
            this.ScheduleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScheduleControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ScheduleControl.Location = new System.Drawing.Point(3, 97);
            this.ScheduleControl.LookAndFeel.SkinName = "Coffee";
            this.ScheduleControl.MainView = this.ScheduleView;
            this.ScheduleControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ScheduleControl.Name = "ScheduleControl";
            this.ScheduleControl.Size = new System.Drawing.Size(785, 336);
            this.ScheduleControl.TabIndex = 17;
            this.ScheduleControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ScheduleView});
            // 
            // ScheduleView
            // 
            this.ScheduleView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.schedule_key,
            this.schedule_name,
            this.description,
            this.MAXOVERLAPTIME,
            this.editor,
            this.edit_timeZone});
            this.ScheduleView.DetailHeight = 450;
            this.ScheduleView.FixedLineWidth = 3;
            this.ScheduleView.GridControl = this.ScheduleControl;
            this.ScheduleView.Name = "ScheduleView";
            this.ScheduleView.DoubleClick += new System.EventHandler(this.ScheduleView_DoubleClick);
            // 
            // schedule_key
            // 
            this.schedule_key.Caption = "主键";
            this.schedule_key.FieldName = "SCHEDULE_KEY";
            this.schedule_key.MinWidth = 23;
            this.schedule_key.Name = "schedule_key";
            this.schedule_key.Width = 86;
            // 
            // schedule_name
            // 
            this.schedule_name.AppearanceHeader.Options.UseTextOptions = true;
            this.schedule_name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.schedule_name.Caption = "计划名称";
            this.schedule_name.FieldName = "SCHEDULE_NAME";
            this.schedule_name.MinWidth = 23;
            this.schedule_name.Name = "schedule_name";
            this.schedule_name.OptionsColumn.AllowEdit = false;
            this.schedule_name.Visible = true;
            this.schedule_name.VisibleIndex = 0;
            this.schedule_name.Width = 86;
            // 
            // description
            // 
            this.description.AppearanceHeader.Options.UseTextOptions = true;
            this.description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.description.Caption = "描述";
            this.description.FieldName = "DESCRIPTIONS";
            this.description.MinWidth = 23;
            this.description.Name = "description";
            this.description.OptionsColumn.AllowEdit = false;
            this.description.Visible = true;
            this.description.VisibleIndex = 1;
            this.description.Width = 86;
            // 
            // MAXOVERLAPTIME
            // 
            this.MAXOVERLAPTIME.Caption = "最大延迟(分钟)";
            this.MAXOVERLAPTIME.FieldName = "MAXOVERLAPTIME";
            this.MAXOVERLAPTIME.MinWidth = 23;
            this.MAXOVERLAPTIME.Name = "MAXOVERLAPTIME";
            this.MAXOVERLAPTIME.Width = 86;
            // 
            // editor
            // 
            this.editor.AppearanceHeader.Options.UseTextOptions = true;
            this.editor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.editor.Caption = "修改者";
            this.editor.FieldName = "EDITOR";
            this.editor.MinWidth = 23;
            this.editor.Name = "editor";
            this.editor.Width = 86;
            // 
            // edit_timeZone
            // 
            this.edit_timeZone.AppearanceHeader.Options.UseTextOptions = true;
            this.edit_timeZone.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.edit_timeZone.Caption = "时区";
            this.edit_timeZone.FieldName = "EDIT_TIMEZONE";
            this.edit_timeZone.MinWidth = 23;
            this.edit_timeZone.Name = "edit_timeZone";
            this.edit_timeZone.Width = 86;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Controls.Add(this.btnQuery);
            this.groupControl1.Controls.Add(this.txtName);
            this.groupControl1.Controls.Add(this.lblName);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 4);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(785, 85);
            this.groupControl1.TabIndex = 16;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(672, 40);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(103, 32);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(115, 42);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(532, 24);
            this.txtName.TabIndex = 13;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(29, 46);
            this.lblName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 18);
            this.lblName.TabIndex = 12;
            this.lblName.Text = "计划名称";
            // 
            // SearchScheduleDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 504);
            this.Controls.Add(this.tableLayoutPanel1);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "SearchScheduleDialog";
            this.Load += new System.EventHandler(this.SearchScheduleDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraGrid.GridControl ScheduleControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ScheduleView;
        private DevExpress.XtraGrid.Columns.GridColumn schedule_key;
        private DevExpress.XtraGrid.Columns.GridColumn schedule_name;
        private DevExpress.XtraGrid.Columns.GridColumn description;
        private DevExpress.XtraGrid.Columns.GridColumn editor;
        private DevExpress.XtraGrid.Columns.GridColumn edit_timeZone;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraGrid.Columns.GridColumn MAXOVERLAPTIME;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
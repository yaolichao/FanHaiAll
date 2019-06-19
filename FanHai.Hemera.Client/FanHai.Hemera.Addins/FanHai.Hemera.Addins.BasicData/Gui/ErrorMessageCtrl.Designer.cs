namespace FanHai.Hemera.Addins.BasicData
{
    partial class ErrorMessageCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gcMessage = new DevExpress.XtraGrid.GridControl();
            this.gvMessage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRowKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcContext = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcObjectKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcObjectType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCriticalLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcToUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btFresh = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMessage)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcMessage
            // 
            this.gcMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMessage.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcMessage.Location = new System.Drawing.Point(3, 49);
            this.gcMessage.MainView = this.gvMessage;
            this.gcMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcMessage.Name = "gcMessage";
            this.gcMessage.Size = new System.Drawing.Size(884, 216);
            this.gcMessage.TabIndex = 0;
            this.gcMessage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMessage});
            // 
            // gvMessage
            // 
            this.gvMessage.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcRowKey,
            this.gcTitle,
            this.gcContext,
            this.gcObjectKey,
            this.gcObjectType,
            this.gcCriticalLevel,
            this.gcToUser,
            this.gcStatus,
            this.gcCreateTime});
            this.gvMessage.DetailHeight = 450;
            this.gvMessage.FixedLineWidth = 3;
            this.gvMessage.GridControl = this.gcMessage;
            this.gvMessage.Name = "gvMessage";
            this.gvMessage.OptionsBehavior.ReadOnly = true;
            this.gvMessage.OptionsView.ShowGroupPanel = false;
            this.gvMessage.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvMessage_ShowingEditor);
            this.gvMessage.DoubleClick += new System.EventHandler(this.gvMessage_DoubleClick);
            // 
            // gcRowKey
            // 
            this.gcRowKey.Caption = "ROW_KEY";
            this.gcRowKey.FieldName = "ROW_KEY";
            this.gcRowKey.MinWidth = 23;
            this.gcRowKey.Name = "gcRowKey";
            this.gcRowKey.OptionsColumn.AllowMove = false;
            this.gcRowKey.Width = 86;
            // 
            // gcTitle
            // 
            this.gcTitle.AppearanceHeader.Options.UseTextOptions = true;
            this.gcTitle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcTitle.Caption = "消息标题";
            this.gcTitle.FieldName = "TITLE";
            this.gcTitle.MinWidth = 23;
            this.gcTitle.Name = "gcTitle";
            this.gcTitle.OptionsColumn.AllowMove = false;
            this.gcTitle.Visible = true;
            this.gcTitle.VisibleIndex = 0;
            this.gcTitle.Width = 114;
            // 
            // gcContext
            // 
            this.gcContext.AppearanceHeader.Options.UseTextOptions = true;
            this.gcContext.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcContext.Caption = "消息内容";
            this.gcContext.FieldName = "CONTEXT";
            this.gcContext.MinWidth = 23;
            this.gcContext.Name = "gcContext";
            this.gcContext.OptionsColumn.AllowMove = false;
            this.gcContext.Visible = true;
            this.gcContext.VisibleIndex = 1;
            this.gcContext.Width = 222;
            // 
            // gcObjectKey
            // 
            this.gcObjectKey.Caption = "对象ID";
            this.gcObjectKey.FieldName = "OBJECTKEY";
            this.gcObjectKey.MinWidth = 23;
            this.gcObjectKey.Name = "gcObjectKey";
            this.gcObjectKey.OptionsColumn.AllowMove = false;
            this.gcObjectKey.Width = 86;
            // 
            // gcObjectType
            // 
            this.gcObjectType.AppearanceHeader.Options.UseTextOptions = true;
            this.gcObjectType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcObjectType.Caption = "对象类型";
            this.gcObjectType.FieldName = "OBJECTTYPE";
            this.gcObjectType.MinWidth = 23;
            this.gcObjectType.Name = "gcObjectType";
            this.gcObjectType.OptionsColumn.AllowMove = false;
            this.gcObjectType.Visible = true;
            this.gcObjectType.VisibleIndex = 2;
            this.gcObjectType.Width = 79;
            // 
            // gcCriticalLevel
            // 
            this.gcCriticalLevel.AppearanceHeader.Options.UseTextOptions = true;
            this.gcCriticalLevel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcCriticalLevel.Caption = "严重程度";
            this.gcCriticalLevel.FieldName = "CRITICAL_LEVEL ";
            this.gcCriticalLevel.MinWidth = 23;
            this.gcCriticalLevel.Name = "gcCriticalLevel";
            this.gcCriticalLevel.OptionsColumn.AllowMove = false;
            this.gcCriticalLevel.Visible = true;
            this.gcCriticalLevel.VisibleIndex = 3;
            this.gcCriticalLevel.Width = 69;
            // 
            // gcToUser
            // 
            this.gcToUser.AppearanceHeader.Options.UseTextOptions = true;
            this.gcToUser.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcToUser.Caption = "接收者";
            this.gcToUser.FieldName = "TO_USER";
            this.gcToUser.MinWidth = 23;
            this.gcToUser.Name = "gcToUser";
            this.gcToUser.OptionsColumn.AllowMove = false;
            this.gcToUser.Visible = true;
            this.gcToUser.VisibleIndex = 5;
            this.gcToUser.Width = 88;
            // 
            // gcStatus
            // 
            this.gcStatus.AppearanceHeader.Options.UseTextOptions = true;
            this.gcStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcStatus.Caption = "消息状态";
            this.gcStatus.FieldName = "STATUS";
            this.gcStatus.MinWidth = 23;
            this.gcStatus.Name = "gcStatus";
            this.gcStatus.OptionsColumn.AllowMove = false;
            this.gcStatus.Visible = true;
            this.gcStatus.VisibleIndex = 4;
            this.gcStatus.Width = 69;
            // 
            // gcCreateTime
            // 
            this.gcCreateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gcCreateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcCreateTime.Caption = "消息时间";
            this.gcCreateTime.FieldName = "CREATE_TIME ";
            this.gcCreateTime.MinWidth = 23;
            this.gcCreateTime.Name = "gcCreateTime";
            this.gcCreateTime.OptionsColumn.AllowMove = false;
            this.gcCreateTime.Visible = true;
            this.gcCreateTime.VisibleIndex = 6;
            this.gcCreateTime.Width = 226;
            // 
            // btFresh
            // 
            this.btFresh.Location = new System.Drawing.Point(5, 4);
            this.btFresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btFresh.Name = "btFresh";
            this.btFresh.Size = new System.Drawing.Size(86, 30);
            this.btFresh.TabIndex = 1;
            this.btFresh.Text = "刷新";
            this.btFresh.Click += new System.EventHandler(this.btFresh_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.gcMessage, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(890, 269);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btFresh);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 4);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(884, 37);
            this.panelControl1.TabIndex = 0;
            // 
            // ErrorMessageCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Office 2007 Pink";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ErrorMessageCtrl";
            this.Size = new System.Drawing.Size(890, 269);
            this.Controls.SetChildIndex(this.tableLayoutPanelMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gcMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMessage)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMessage;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMessage;
        private DevExpress.XtraGrid.Columns.GridColumn gcRowKey;
        private DevExpress.XtraGrid.Columns.GridColumn gcTitle;
        private DevExpress.XtraGrid.Columns.GridColumn gcContext;
        private DevExpress.XtraGrid.Columns.GridColumn gcObjectKey;
        private DevExpress.XtraGrid.Columns.GridColumn gcObjectType;
        private DevExpress.XtraGrid.Columns.GridColumn gcCriticalLevel;
        private DevExpress.XtraGrid.Columns.GridColumn gcToUser;
        private DevExpress.XtraGrid.Columns.GridColumn gcCreateTime;
        private DevExpress.XtraGrid.Columns.GridColumn gcStatus;
        private DevExpress.XtraEditors.SimpleButton btFresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.PanelControl panelControl1;

    }
}

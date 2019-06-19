namespace SolarViewer.Hemera.Addins.SPC
{
    partial class SpcParameterCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpcParameterCtrl));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.SpcTools = new System.Windows.Forms.ToolStrip();
            this.btSave = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.gcMainParam = new DevExpress.XtraGrid.GridControl();
            this.gvMainParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.paramName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paramKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pnlTitle = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SpcTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.SpcTools, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.gcMainParam, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.pnlTitle, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(869, 576);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // SpcTools
            // 
            this.SpcTools.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SpcTools.BackgroundImage")));
            this.SpcTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpcTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SpcTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btSave,
            this.btnAdd,
            this.btnDelete});
            this.SpcTools.Location = new System.Drawing.Point(0, 0);
            this.SpcTools.Name = "SpcTools";
            this.SpcTools.Size = new System.Drawing.Size(869, 25);
            this.SpcTools.TabIndex = 0;
            this.SpcTools.Text = "toolStrip1";
            // 
            // btSave
            // 
            this.btSave.Image = global::SolarViewer.Hemera.Addins.SPC.Properties.Resources.save_accept;
            this.btSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(52, 22);
            this.btSave.Text = "保存";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::SolarViewer.Hemera.Addins.SPC.Properties.Resources.document_add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(52, 22);
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::SolarViewer.Hemera.Addins.SPC.Properties.Resources.document_delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(52, 22);
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gcMainParam
            // 
            this.gcMainParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMainParam.Location = new System.Drawing.Point(3, 73);
            this.gcMainParam.LookAndFeel.SkinName = "Blue";
            this.gcMainParam.MainView = this.gvMainParam;
            this.gcMainParam.Name = "gcMainParam";
            this.gcMainParam.Size = new System.Drawing.Size(863, 500);
            this.gcMainParam.TabIndex = 2;
            this.gcMainParam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMainParam});
            // 
            // gvMainParam
            // 
            this.gvMainParam.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.paramName,
            this.paramKey});
            this.gvMainParam.GridControl = this.gcMainParam;
            this.gvMainParam.Name = "gvMainParam";
            this.gvMainParam.OptionsView.ShowGroupPanel = false;
            // 
            // paramName
            // 
            this.paramName.AppearanceHeader.Options.UseTextOptions = true;
            this.paramName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.paramName.Caption = "参数名";
            this.paramName.FieldName = "PARAM_NAME";
            this.paramName.Name = "paramName";
            this.paramName.OptionsColumn.AllowEdit = false;
            this.paramName.Visible = true;
            this.paramName.VisibleIndex = 0;
            // 
            // paramKey
            // 
            this.paramKey.Caption = "主键";
            this.paramKey.FieldName = "PARAM_KEY";
            this.paramKey.Name = "paramKey";
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(3, 28);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Padding = new System.Windows.Forms.Padding(6);
            this.pnlTitle.Size = new System.Drawing.Size(863, 39);
            this.pnlTitle.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Arial", 16F);
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(131, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "SPC参数设置";
            // 
            // SpcParameterCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Money Twins";
            this.Name = "SpcParameterCtrl";
            this.Size = new System.Drawing.Size(869, 576);
            this.Load += new System.EventHandler(this.SpcParameterCtrl_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.SpcTools.ResumeLayout(false);
            this.SpcTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTitle)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip SpcTools;
        private System.Windows.Forms.ToolStripButton btSave;
        private DevExpress.XtraGrid.GridControl gcMainParam;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMainParam;
        private DevExpress.XtraGrid.Columns.GridColumn paramName;
        private DevExpress.XtraGrid.Columns.GridColumn paramKey;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private DevExpress.XtraEditors.PanelControl pnlTitle;
        private DevExpress.XtraEditors.LabelControl lblTitle;
    }
}

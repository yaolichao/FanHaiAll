namespace FanHai.Hemera.Addins.FMM
{
    partial class LineSearchDialog
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btSearch = new DevExpress.XtraEditors.SimpleButton();
            this.txtAttributeName = new DevExpress.XtraEditors.TextEdit();
            this.lblAttributeName = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.gridData = new DevExpress.XtraGrid.GridControl();
            this.gridDataView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clnKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.line_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.line_code = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttributeName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Controls.Add(this.btSearch);
            this.groupControl1.Controls.Add(this.txtAttributeName);
            this.groupControl1.Controls.Add(this.lblAttributeName);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(843, 61);
            this.groupControl1.TabIndex = 0;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(741, 26);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(91, 29);
            this.btSearch.TabIndex = 2;
            this.btSearch.Text = "查 询";
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // txtAttributeName
            // 
            this.txtAttributeName.Location = new System.Drawing.Point(118, 31);
            this.txtAttributeName.Name = "txtAttributeName";
            this.txtAttributeName.Size = new System.Drawing.Size(498, 20);
            this.txtAttributeName.TabIndex = 1;
            // 
            // lblAttributeName
            // 
            this.lblAttributeName.Location = new System.Drawing.Point(9, 32);
            this.lblAttributeName.Name = "lblAttributeName";
            this.lblAttributeName.Size = new System.Drawing.Size(56, 14);
            this.lblAttributeName.TabIndex = 0;
            this.lblAttributeName.Text = "线别名称 :";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(582, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 29);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.panelControl1);
            this.groupControl3.Controls.Add(this.gridData);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 61);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(843, 422);
            this.groupControl3.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnOK);
            this.panelControl1.Controls.Add(this.btnCancle);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 380);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(839, 40);
            this.panelControl1.TabIndex = 3;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(702, 5);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(105, 29);
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // gridData
            // 
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gridData.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridData.Location = new System.Drawing.Point(2, 23);
            this.gridData.LookAndFeel.SkinName = "Coffee";
            this.gridData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridData.MainView = this.gridDataView;
            this.gridData.Name = "gridData";
            this.gridData.Size = new System.Drawing.Size(839, 397);
            this.gridData.TabIndex = 2;
            this.gridData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridDataView});
            this.gridData.DoubleClick += new System.EventHandler(this.gridData_DoubleClick);
            // 
            // gridDataView
            // 
            this.gridDataView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clnKey,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridDataView.CustomizationFormBounds = new System.Drawing.Rectangle(279, 328, 300, 0);
            this.gridDataView.GridControl = this.gridData;
            this.gridDataView.Name = "gridDataView";
            this.gridDataView.OptionsBehavior.ReadOnly = true;
            this.gridDataView.OptionsView.ShowGroupPanel = false;
            this.gridDataView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridDataView_CustomDrawRowIndicator);
            this.gridDataView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridDataView_ShowingEditor);
            // 
            // clnKey
            // 
            this.clnKey.AppearanceHeader.Options.UseTextOptions = true;
            this.clnKey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnKey.Caption = "Key";
            this.clnKey.FieldName = "ROW_KEY";
            this.clnKey.Name = "clnKey";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "序号";
            this.gridColumn1.FieldName = "LINE_CODE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "名称";
            this.gridColumn2.FieldName = "LINE_NAME";
            this.gridColumn2.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "描述";
            this.gridColumn3.FieldName = "DESCRIPTIONS";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // clnName
            // 
            this.clnName.AppearanceHeader.Options.UseTextOptions = true;
            this.clnName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnName.Caption = "属性名称";
            this.clnName.FieldName = "OBJECT_NAME";
            this.clnName.Name = "clnName";
            this.clnName.Visible = true;
            this.clnName.VisibleIndex = 0;
            // 
            // line_name
            // 
            this.line_name.AppearanceHeader.Options.UseTextOptions = true;
            this.line_name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.line_name.Caption = "名称";
            this.line_name.FieldName = "LINE_NAME";
            this.line_name.Name = "line_name";
            this.line_name.Visible = true;
            this.line_name.VisibleIndex = 1;
            // 
            // line_code
            // 
            this.line_code.AppearanceHeader.Options.UseTextOptions = true;
            this.line_code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.line_code.Caption = "编号";
            this.line_code.FieldName = "LINE_CODE";
            this.line_code.Name = "line_code";
            this.line_code.Visible = true;
            this.line_code.VisibleIndex = 0;
            // 
            // LineSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 483);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LineSearchDialog";
            this.Load += new System.EventHandler(this.LineSearchDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttributeName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btSearch;
        private DevExpress.XtraEditors.TextEdit txtAttributeName;
        private DevExpress.XtraEditors.LabelControl lblAttributeName;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private DevExpress.XtraGrid.Columns.GridColumn clnName;
        private DevExpress.XtraGrid.Columns.GridColumn line_name;
        private DevExpress.XtraGrid.Columns.GridColumn line_code;
        private DevExpress.XtraGrid.GridControl gridData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridDataView;
        private DevExpress.XtraGrid.Columns.GridColumn clnKey;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
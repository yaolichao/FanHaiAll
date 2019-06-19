namespace FanHai.Hemera.Addins.WIP
{
    partial class PorLineHelpDialog
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
            this.txtpartnumber = new DevExpress.XtraEditors.TextEdit();
            this.btnsearch = new DevExpress.XtraEditors.SimpleButton();
            this.lblName = new DevExpress.XtraEditors.LabelControl();
            this.lblShow = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gvpnhelp = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PRODUCTION_LINE_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LINE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LINE_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtshownumber = new DevExpress.XtraEditors.TextEdit();
            this.lblLine = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.txtpartnumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvpnhelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtshownumber.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtpartnumber
            // 
            this.txtpartnumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtpartnumber.Location = new System.Drawing.Point(112, 5);
            this.txtpartnumber.Name = "txtpartnumber";
            this.txtpartnumber.Properties.LookAndFeel.SkinName = "Coffee";
            this.txtpartnumber.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtpartnumber.Size = new System.Drawing.Size(105, 21);
            this.txtpartnumber.TabIndex = 7;
            // 
            // btnsearch
            // 
            this.btnsearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnsearch.Location = new System.Drawing.Point(383, 3);
            this.btnsearch.LookAndFeel.SkinName = "Coffee";
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(75, 25);
            this.btnsearch.TabIndex = 8;
            this.btnsearch.Text = "开始检索";
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblName.Location = new System.Drawing.Point(3, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 14);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "线别名称";
            // 
            // lblShow
            // 
            this.lblShow.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblShow.Location = new System.Drawing.Point(223, 8);
            this.lblShow.Name = "lblShow";
            this.lblShow.Size = new System.Drawing.Size(24, 14);
            this.lblShow.TabIndex = 10;
            this.lblShow.Text = "显示";
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(3, 3);
            this.gridControl1.LookAndFeel.SkinName = "Coffee";
            this.gridControl1.MainView = this.gvpnhelp;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(464, 157);
            this.gridControl1.TabIndex = 6;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvpnhelp,
            this.gridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gvpnhelp
            // 
            this.gvpnhelp.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.PRODUCTION_LINE_KEY,
            this.LINE_NAME,
            this.LINE_CODE});
            this.gvpnhelp.GridControl = this.gridControl1;
            this.gvpnhelp.Name = "gvpnhelp";
            this.gvpnhelp.OptionsView.ShowGroupPanel = false;
            // 
            // PRODUCTION_LINE_KEY
            // 
            this.PRODUCTION_LINE_KEY.Caption = "LineKey";
            this.PRODUCTION_LINE_KEY.FieldName = "PRODUCTION_LINE_KEY";
            this.PRODUCTION_LINE_KEY.Name = "PRODUCTION_LINE_KEY";
            this.PRODUCTION_LINE_KEY.OptionsColumn.AllowEdit = false;
            // 
            // LINE_NAME
            // 
            this.LINE_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.LINE_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LINE_NAME.Caption = "线别名称";
            this.LINE_NAME.FieldName = "LINE_NAME";
            this.LINE_NAME.Name = "LINE_NAME";
            this.LINE_NAME.OptionsColumn.AllowEdit = false;
            this.LINE_NAME.Visible = true;
            this.LINE_NAME.VisibleIndex = 0;
            // 
            // LINE_CODE
            // 
            this.LINE_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.LINE_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LINE_CODE.Caption = "线别编码";
            this.LINE_CODE.FieldName = "LINE_CODE";
            this.LINE_CODE.Name = "LINE_CODE";
            this.LINE_CODE.OptionsColumn.AllowEdit = false;
            this.LINE_CODE.Visible = true;
            this.LINE_CODE.VisibleIndex = 1;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // txtshownumber
            // 
            this.txtshownumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtshownumber.EditValue = "200";
            this.txtshownumber.Location = new System.Drawing.Point(281, 5);
            this.txtshownumber.Name = "txtshownumber";
            this.txtshownumber.Properties.LookAndFeel.SkinName = "Coffee";
            this.txtshownumber.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtshownumber.Properties.Mask.EditMask = "n0";
            this.txtshownumber.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtshownumber.Size = new System.Drawing.Size(52, 21);
            this.txtshownumber.TabIndex = 11;
            // 
            // lblLine
            // 
            this.lblLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLine.Location = new System.Drawing.Point(339, 8);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(12, 14);
            this.lblLine.TabIndex = 10;
            this.lblLine.Text = "行";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(470, 200);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.54128F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.45872F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel2.Controls.Add(this.txtshownumber, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnsearch, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblLine, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtpartnumber, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblShow, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 166);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(464, 31);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // PorLineHelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(470, 200);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PorLineHelpDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Deactivate += new System.EventHandler(this.PorLineHelpDialog_Deactivate);
            ((System.ComponentModel.ISupportInitialize)(this.txtpartnumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvpnhelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtshownumber.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtpartnumber;
        private DevExpress.XtraEditors.SimpleButton btnsearch;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraEditors.LabelControl lblShow;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvpnhelp;
        private DevExpress.XtraGrid.Columns.GridColumn PRODUCTION_LINE_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn LINE_NAME;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.TextEdit txtshownumber;
        private DevExpress.XtraEditors.LabelControl lblLine;
        private DevExpress.XtraGrid.Columns.GridColumn LINE_CODE;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

    }
}
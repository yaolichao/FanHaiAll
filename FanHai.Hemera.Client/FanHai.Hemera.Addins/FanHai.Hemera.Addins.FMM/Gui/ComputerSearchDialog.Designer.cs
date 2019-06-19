namespace FanHai.Hemera.Addins.FMM
{
    partial class ComputerSearchDialog
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
            this.groupControlTop = new DevExpress.XtraEditors.GroupControl();
            this.btSearch = new DevExpress.XtraEditors.SimpleButton();
            this.txtComputerName = new DevExpress.XtraEditors.TextEdit();
            this.lblWorkOrderNumber = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.groupControlContent = new DevExpress.XtraEditors.GroupControl();
            this.gridData = new DevExpress.XtraGrid.GridControl();
            this.gridDataView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clnKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancle = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).BeginInit();
            this.groupControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtComputerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlContent)).BeginInit();
            this.groupControlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControlTop
            // 
            this.groupControlTop.Controls.Add(this.btSearch);
            this.groupControlTop.Controls.Add(this.txtComputerName);
            this.groupControlTop.Controls.Add(this.lblWorkOrderNumber);
            this.groupControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlTop.Location = new System.Drawing.Point(3, 4);
            this.groupControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlTop.Name = "groupControlTop";
            this.groupControlTop.Size = new System.Drawing.Size(957, 96);
            this.groupControlTop.TabIndex = 0;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(842, 44);
            this.btSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(104, 37);
            this.btSearch.TabIndex = 2;
            this.btSearch.Text = "查 询";
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // txtComputerName
            // 
            this.txtComputerName.Location = new System.Drawing.Point(114, 49);
            this.txtComputerName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtComputerName.Name = "txtComputerName";
            this.txtComputerName.Size = new System.Drawing.Size(712, 24);
            this.txtComputerName.TabIndex = 1;
            // 
            // lblWorkOrderNumber
            // 
            this.lblWorkOrderNumber.Location = new System.Drawing.Point(22, 51);
            this.lblWorkOrderNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblWorkOrderNumber.Name = "lblWorkOrderNumber";
            this.lblWorkOrderNumber.Size = new System.Drawing.Size(75, 18);
            this.lblWorkOrderNumber.TabIndex = 0;
            this.lblWorkOrderNumber.Text = "计算机名称";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(712, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(111, 37);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupControlContent
            // 
            this.groupControlContent.Controls.Add(this.gridData);
            this.groupControlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlContent.Location = new System.Drawing.Point(3, 108);
            this.groupControlContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlContent.Name = "groupControlContent";
            this.groupControlContent.Size = new System.Drawing.Size(957, 436);
            this.groupControlContent.TabIndex = 2;
            // 
            // gridData
            // 
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridData.Location = new System.Drawing.Point(2, 28);
            this.gridData.LookAndFeel.SkinName = "Coffee";
            this.gridData.MainView = this.gridDataView;
            this.gridData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridData.Name = "gridData";
            this.gridData.Size = new System.Drawing.Size(953, 406);
            this.gridData.TabIndex = 1;
            this.gridData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridDataView});
            this.gridData.DoubleClick += new System.EventHandler(this.gridData_DoubleClick);
            // 
            // gridDataView
            // 
            this.gridDataView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clnKey,
            this.clnName,
            this.clnDescription});
            this.gridDataView.DetailHeight = 450;
            this.gridDataView.FixedLineWidth = 3;
            this.gridDataView.GridControl = this.gridData;
            this.gridDataView.Name = "gridDataView";
            this.gridDataView.OptionsBehavior.ReadOnly = true;
            this.gridDataView.OptionsView.ShowGroupPanel = false;
            this.gridDataView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridDataView_ShowingEditor);
            // 
            // clnKey
            // 
            this.clnKey.AppearanceHeader.Options.UseTextOptions = true;
            this.clnKey.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnKey.Caption = "Key";
            this.clnKey.FieldName = "ROW_KEY";
            this.clnKey.MinWidth = 23;
            this.clnKey.Name = "clnKey";
            this.clnKey.Width = 86;
            // 
            // clnName
            // 
            this.clnName.AppearanceHeader.Options.UseTextOptions = true;
            this.clnName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnName.Caption = "计算机名称";
            this.clnName.FieldName = "COMPUTER_NAME";
            this.clnName.MinWidth = 23;
            this.clnName.Name = "clnName";
            this.clnName.Visible = true;
            this.clnName.VisibleIndex = 0;
            this.clnName.Width = 86;
            // 
            // clnDescription
            // 
            this.clnDescription.Caption = "计算机描述";
            this.clnDescription.FieldName = "DESCRIPTION";
            this.clnDescription.MinWidth = 23;
            this.clnDescription.Name = "clnDescription";
            this.clnDescription.Visible = true;
            this.clnDescription.VisibleIndex = 1;
            this.clnDescription.Width = 86;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel2.Controls.Add(this.btnOK, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancle, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 565);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(957, 52);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(841, 4);
            this.btnCancle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(102, 37);
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.AllowDrop = true;
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.groupControlTop, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupControlContent, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(963, 621);
            this.tableLayoutPanelMain.TabIndex = 5;
            // 
            // ComputerSearchDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 621);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ComputerSearchDialog";
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).EndInit();
            this.groupControlTop.ResumeLayout(false);
            this.groupControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtComputerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlContent)).EndInit();
            this.groupControlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlTop;
        private DevExpress.XtraEditors.SimpleButton btSearch;
        private DevExpress.XtraEditors.TextEdit txtComputerName;
        private DevExpress.XtraEditors.LabelControl lblWorkOrderNumber;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraGrid.GridControl gridData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridDataView;
        private DevExpress.XtraGrid.Columns.GridColumn clnKey;
        private DevExpress.XtraGrid.Columns.GridColumn clnName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnCancle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.GroupControl groupControlContent;
        private DevExpress.XtraGrid.Columns.GridColumn clnDescription;
    }
}
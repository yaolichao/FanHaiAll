namespace FanHai.Hemera.Utils.UDA
{
    partial class UdaCommonControl
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
            this.gridUDAs = new DevExpress.XtraGrid.GridControl();
            this.gridUDAsView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clnAttributeKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnAttributeDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clnEditTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemPopupContainerEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupControlMain = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridUDAs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUDAsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).BeginInit();
            this.groupControlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridUDAs
            // 
            this.gridUDAs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUDAs.Location = new System.Drawing.Point(3, 39);
            this.gridUDAs.LookAndFeel.SkinName = "Coffee";
            this.gridUDAs.MainView = this.gridUDAsView;
            this.gridUDAs.Name = "gridUDAs";
            this.gridUDAs.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemPopupContainerEdit1,
            this.repositoryItemComboBox1});
            this.gridUDAs.Size = new System.Drawing.Size(515, 242);
            this.gridUDAs.TabIndex = 0;
            this.gridUDAs.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridUDAsView});
            // 
            // gridUDAsView
            // 
            this.gridUDAsView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clnAttributeKey,
            this.clnAttributeName,
            this.clnAttributeValue,
            this.clnAttributeDataType,
            this.clnEditTime});
            this.gridUDAsView.GridControl = this.gridUDAs;
            this.gridUDAsView.Name = "gridUDAsView";
            this.gridUDAsView.OptionsView.ShowGroupPanel = false;
            this.gridUDAsView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvUdaList_CellValueChanged);
            this.gridUDAsView.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvUdaList_CustomRowCellEdit);
            // 
            // clnAttributeKey
            // 
            this.clnAttributeKey.Caption = "属性ID";
            this.clnAttributeKey.FieldName = "clnAttributeKey";
            this.clnAttributeKey.Name = "clnAttributeKey";
            this.clnAttributeKey.OptionsColumn.AllowEdit = false;
            // 
            // clnAttributeName
            // 
            this.clnAttributeName.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeName.Caption = "属性名";
            this.clnAttributeName.FieldName = "clnAttributeName";
            this.clnAttributeName.Name = "clnAttributeName";
            this.clnAttributeName.OptionsColumn.AllowEdit = false;
            this.clnAttributeName.Visible = true;
            this.clnAttributeName.VisibleIndex = 0;
            // 
            // clnAttributeValue
            // 
            this.clnAttributeValue.AppearanceHeader.Options.UseTextOptions = true;
            this.clnAttributeValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.clnAttributeValue.Caption = "属性值";
            this.clnAttributeValue.FieldName = "clnAttributeValue";
            this.clnAttributeValue.Name = "clnAttributeValue";
            this.clnAttributeValue.Visible = true;
            this.clnAttributeValue.VisibleIndex = 1;
            // 
            // clnAttributeDataType
            // 
            this.clnAttributeDataType.Caption = "数据类型";
            this.clnAttributeDataType.FieldName = "clnAttributeDataType";
            this.clnAttributeDataType.Name = "clnAttributeDataType";
            this.clnAttributeDataType.OptionsColumn.AllowEdit = false;
            // 
            // clnEditTime
            // 
            this.clnEditTime.Caption = "最后修改时间";
            this.clnEditTime.FieldName = "clnEditTime";
            this.clnEditTime.Name = "clnEditTime";
            this.clnEditTime.OptionsColumn.AllowEdit = false;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // repositoryItemPopupContainerEdit1
            // 
            this.repositoryItemPopupContainerEdit1.AutoHeight = false;
            this.repositoryItemPopupContainerEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupContainerEdit1.Name = "repositoryItemPopupContainerEdit1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(306, 3);
            this.btnAdd.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加属性";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(416, 3);
            this.btnDelete.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(96, 24);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除属性";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gridUDAs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(521, 284);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAdd, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(515, 30);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupControlMain
            // 
            this.groupControlMain.Controls.Add(this.tableLayoutPanel1);
            this.groupControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlMain.Location = new System.Drawing.Point(0, 0);
            this.groupControlMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.groupControlMain.Margin = new System.Windows.Forms.Padding(6);
            this.groupControlMain.Name = "groupControlMain";
            this.groupControlMain.Size = new System.Drawing.Size(525, 309);
            this.groupControlMain.TabIndex = 4;
            // 
            // UdaCommonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Controls.Add(this.groupControlMain);
            this.Name = "UdaCommonControl";
            this.Size = new System.Drawing.Size(525, 309);
            ((System.ComponentModel.ISupportInitialize)(this.gridUDAs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUDAsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).EndInit();
            this.groupControlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridUDAs;
        private DevExpress.XtraGrid.Views.Grid.GridView gridUDAsView;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeKey;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeName;
        private DevExpress.XtraGrid.Columns.GridColumn clnEditTime;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeDataType;
        private DevExpress.XtraGrid.Columns.GridColumn clnAttributeValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.GroupControl groupControlMain;       
    }
}

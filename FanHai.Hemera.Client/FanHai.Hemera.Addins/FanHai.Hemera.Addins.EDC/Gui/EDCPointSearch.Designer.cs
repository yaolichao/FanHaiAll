namespace FanHai.Hemera.Addins.EDC
{
    partial class EDCPointSearch
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
            this.EDCPionts = new DevExpress.XtraGrid.GridControl();
            this.gridViewEdc = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ROW_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EDC_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TOPRODUCT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPartType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRoute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OPERATION_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EQUIPMENT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ACTION_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SP_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.POINT_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GROUP_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EQUIPMENT_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OPERATION_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MUST_INPUT_FIELD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.teOperationName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.tePartName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EDCPionts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEdc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePartName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.EDCPionts, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // EDCPionts
            // 
            this.EDCPionts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EDCPionts.Location = new System.Drawing.Point(3, 74);
            this.EDCPionts.LookAndFeel.SkinName = "Coffee";
            this.EDCPionts.MainView = this.gridViewEdc;
            this.EDCPionts.Name = "EDCPionts";
            this.EDCPionts.Size = new System.Drawing.Size(778, 373);
            this.EDCPionts.TabIndex = 4;
            this.EDCPionts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEdc});
            // 
            // gridViewEdc
            // 
            this.gridViewEdc.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ROW_KEY,
            this.gcolGroupName,
            this.EDC_NAME,
            this.TOPRODUCT,
            this.gcPartType,
            this.gcRoute,
            this.OPERATION_NAME,
            this.EQUIPMENT_NAME,
            this.ACTION_NAME,
            this.SP_NAME,
            this.POINT_STATUS,
            this.GROUP_KEY,
            this.EQUIPMENT_KEY,
            this.OPERATION_KEY,
            this.MUST_INPUT_FIELD});
            this.gridViewEdc.GridControl = this.EDCPionts;
            this.gridViewEdc.Name = "gridViewEdc";
            this.gridViewEdc.OptionsBehavior.Editable = false;
            this.gridViewEdc.OptionsView.ColumnAutoWidth = false;
            this.gridViewEdc.OptionsView.ShowGroupPanel = false;
            this.gridViewEdc.DoubleClick += new System.EventHandler(this.gridViewEdc_DoubleClick);
            // 
            // ROW_KEY
            // 
            this.ROW_KEY.Caption = "ROW_KEY";
            this.ROW_KEY.FieldName = "ROW_KEY";
            this.ROW_KEY.Name = "ROW_KEY";
            // 
            // gcolGroupName
            // 
            this.gcolGroupName.Caption = "抽检点名称";
            this.gcolGroupName.FieldName = "GROUP_NAME";
            this.gcolGroupName.Name = "gcolGroupName";
            this.gcolGroupName.Visible = true;
            this.gcolGroupName.VisibleIndex = 0;
            // 
            // EDC_NAME
            // 
            this.EDC_NAME.Caption = "参数组名称";
            this.EDC_NAME.FieldName = "EDC_NAME";
            this.EDC_NAME.Name = "EDC_NAME";
            this.EDC_NAME.OptionsColumn.AllowEdit = false;
            this.EDC_NAME.OptionsColumn.ReadOnly = true;
            this.EDC_NAME.Visible = true;
            this.EDC_NAME.VisibleIndex = 1;
            // 
            // TOPRODUCT
            // 
            this.TOPRODUCT.AppearanceHeader.Options.UseTextOptions = true;
            this.TOPRODUCT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TOPRODUCT.Caption = "成品编号";
            this.TOPRODUCT.FieldName = "TOPRODUCT";
            this.TOPRODUCT.Name = "TOPRODUCT";
            this.TOPRODUCT.OptionsColumn.AllowEdit = false;
            this.TOPRODUCT.OptionsColumn.ReadOnly = true;
            // 
            // gcPartType
            // 
            this.gcPartType.Caption = "成品类型";
            this.gcPartType.FieldName = "PART_TYPE";
            this.gcPartType.Name = "gcPartType";
            this.gcPartType.Visible = true;
            this.gcPartType.VisibleIndex = 2;
            // 
            // gcRoute
            // 
            this.gcRoute.Caption = "工艺流程";
            this.gcRoute.FieldName = "ROUTE_NAME";
            this.gcRoute.Name = "gcRoute";
            this.gcRoute.Visible = true;
            this.gcRoute.VisibleIndex = 3;
            // 
            // OPERATION_NAME
            // 
            this.OPERATION_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.OPERATION_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OPERATION_NAME.Caption = "工序名称";
            this.OPERATION_NAME.FieldName = "OPERATION_NAME";
            this.OPERATION_NAME.Name = "OPERATION_NAME";
            this.OPERATION_NAME.OptionsColumn.AllowEdit = false;
            this.OPERATION_NAME.OptionsColumn.ReadOnly = true;
            this.OPERATION_NAME.Visible = true;
            this.OPERATION_NAME.VisibleIndex = 4;
            // 
            // EQUIPMENT_NAME
            // 
            this.EQUIPMENT_NAME.Caption = "设备";
            this.EQUIPMENT_NAME.FieldName = "EQUIPMENT_NAME";
            this.EQUIPMENT_NAME.Name = "EQUIPMENT_NAME";
            this.EQUIPMENT_NAME.OptionsColumn.AllowEdit = false;
            this.EQUIPMENT_NAME.OptionsColumn.ReadOnly = true;
            this.EQUIPMENT_NAME.Visible = true;
            this.EQUIPMENT_NAME.VisibleIndex = 5;
            this.EQUIPMENT_NAME.Width = 200;
            // 
            // ACTION_NAME
            // 
            this.ACTION_NAME.Caption = "动作";
            this.ACTION_NAME.FieldName = "ACTION_NAME";
            this.ACTION_NAME.Name = "ACTION_NAME";
            this.ACTION_NAME.OptionsColumn.AllowEdit = false;
            this.ACTION_NAME.OptionsColumn.ReadOnly = true;
            this.ACTION_NAME.Visible = true;
            this.ACTION_NAME.VisibleIndex = 6;
            // 
            // SP_NAME
            // 
            this.SP_NAME.Caption = "规则名称";
            this.SP_NAME.FieldName = "SP_NAME";
            this.SP_NAME.Name = "SP_NAME";
            this.SP_NAME.OptionsColumn.AllowEdit = false;
            this.SP_NAME.OptionsColumn.ReadOnly = true;
            this.SP_NAME.Visible = true;
            this.SP_NAME.VisibleIndex = 7;
            // 
            // POINT_STATUS
            // 
            this.POINT_STATUS.Caption = "状态";
            this.POINT_STATUS.FieldName = "POINT_STATE_DESCRIPTION";
            this.POINT_STATUS.Name = "POINT_STATUS";
            this.POINT_STATUS.Visible = true;
            this.POINT_STATUS.VisibleIndex = 8;
            // 
            // GROUP_KEY
            // 
            this.GROUP_KEY.FieldName = "GROUP_KEY";
            this.GROUP_KEY.Name = "GROUP_KEY";
            // 
            // EQUIPMENT_KEY
            // 
            this.EQUIPMENT_KEY.FieldName = "EQUIPMENT_KEY";
            this.EQUIPMENT_KEY.Name = "EQUIPMENT_KEY";
            // 
            // OPERATION_KEY
            // 
            this.OPERATION_KEY.FieldName = "OPERATION_KEY";
            this.OPERATION_KEY.Name = "OPERATION_KEY";
            // 
            // MUST_INPUT_FIELD
            // 
            this.MUST_INPUT_FIELD.FieldName = "MUST_INPUT_FIELD";
            this.MUST_INPUT_FIELD.Name = "MUST_INPUT_FIELD";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.teOperationName);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.tePartName);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.btnCancel);
            this.groupControl1.Controls.Add(this.btnQuery);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(778, 65);
            this.groupControl1.TabIndex = 3;
            // 
            // teOperationName
            // 
            this.teOperationName.Location = new System.Drawing.Point(83, 33);
            this.teOperationName.Name = "teOperationName";
            this.teOperationName.Size = new System.Drawing.Size(456, 21);
            this.teOperationName.TabIndex = 90;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 35);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 89;
            this.labelControl2.Text = "工序名称";
            // 
            // tePartName
            // 
            this.tePartName.Location = new System.Drawing.Point(83, 2);
            this.tePartName.Name = "tePartName";
            this.tePartName.Size = new System.Drawing.Size(45, 21);
            this.tePartName.TabIndex = 88;
            this.tePartName.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 87;
            this.labelControl1.Text = "成品料号";
            this.labelControl1.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(671, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 86;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(568, 31);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 25);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // EDCPointSearch
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "EDCPointSearch";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EDCPionts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEdc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePartName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraGrid.GridControl EDCPionts;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewEdc;
        private DevExpress.XtraGrid.Columns.GridColumn ROW_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn TOPRODUCT;
        private DevExpress.XtraGrid.Columns.GridColumn OPERATION_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn EQUIPMENT_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn ACTION_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn EDC_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn SP_NAME;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit tePartName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit teOperationName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn POINT_STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn gcPartType;
        private DevExpress.XtraGrid.Columns.GridColumn GROUP_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn EQUIPMENT_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn OPERATION_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn gcRoute;
        private DevExpress.XtraGrid.Columns.GridColumn MUST_INPUT_FIELD;
        private DevExpress.XtraGrid.Columns.GridColumn gcolGroupName;


    }
}
namespace FanHai.Hemera.Addins.RBAC
{
    partial class AddOperationGroup
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
            this.tableLayoutPanelOperation = new System.Windows.Forms.TableLayoutPanel();
            this.layoutControlOperation = new DevExpress.XtraLayout.LayoutControl();
            this.txtRemark = new DevExpress.XtraEditors.MemoEdit();
            this.txtGroupName = new DevExpress.XtraEditors.TextEdit();
            this.txtGroupDescription = new DevExpress.XtraEditors.TextEdit();
            this.layoutCtlGrpOperation = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblGroupName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRemark = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblGroupDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlOperation)).BeginInit();
            this.layoutControlOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGrpOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGroupDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelOperation
            // 
            this.tableLayoutPanelOperation.ColumnCount = 1;
            this.tableLayoutPanelOperation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOperation.Controls.Add(this.layoutControlOperation, 0, 0);
            this.tableLayoutPanelOperation.Controls.Add(this.panelControlBottom, 0, 1);
            this.tableLayoutPanelOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOperation.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelOperation.Name = "tableLayoutPanelOperation";
            this.tableLayoutPanelOperation.RowCount = 2;
            this.tableLayoutPanelOperation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOperation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanelOperation.Size = new System.Drawing.Size(533, 215);
            this.tableLayoutPanelOperation.TabIndex = 1;
            // 
            // layoutControlOperation
            // 
            this.layoutControlOperation.Controls.Add(this.txtRemark);
            this.layoutControlOperation.Controls.Add(this.txtGroupName);
            this.layoutControlOperation.Controls.Add(this.txtGroupDescription);
            this.layoutControlOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlOperation.Location = new System.Drawing.Point(3, 3);
            this.layoutControlOperation.Name = "layoutControlOperation";
            this.layoutControlOperation.Root = this.layoutCtlGrpOperation;
            this.layoutControlOperation.Size = new System.Drawing.Size(527, 168);
            this.layoutControlOperation.TabIndex = 2;
            this.layoutControlOperation.Text = "layoutControl1";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(52, 58);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(463, 98);
            this.txtRemark.StyleController = this.layoutControlOperation;
            this.txtRemark.TabIndex = 5;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(52, 33);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(209, 21);
            this.txtGroupName.StyleController = this.layoutControlOperation;
            this.txtGroupName.TabIndex = 1;
            // 
            // txtGroupDescription
            // 
            this.txtGroupDescription.Location = new System.Drawing.Point(305, 33);
            this.txtGroupDescription.Name = "txtGroupDescription";
            this.txtGroupDescription.Size = new System.Drawing.Size(210, 21);
            this.txtGroupDescription.StyleController = this.layoutControlOperation;
            this.txtGroupDescription.TabIndex = 3;
            // 
            // layoutCtlGrpOperation
            // 
            this.layoutCtlGrpOperation.CustomizationFormText = "操作组";
            this.layoutCtlGrpOperation.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutCtlGrpOperation.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblGroupName,
            this.lblRemark,
            this.lblGroupDescription});
            this.layoutCtlGrpOperation.Location = new System.Drawing.Point(0, 0);
            this.layoutCtlGrpOperation.Name = "layoutCtlGrpOperation";
            this.layoutCtlGrpOperation.Size = new System.Drawing.Size(527, 168);
            this.layoutCtlGrpOperation.Text = "操作组";
            // 
            // lblGroupName
            // 
            this.lblGroupName.Control = this.txtGroupName;
            this.lblGroupName.CustomizationFormText = "名称：";
            this.lblGroupName.Location = new System.Drawing.Point(0, 0);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(253, 25);
            this.lblGroupName.Text = "名称：";
            this.lblGroupName.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lblRemark
            // 
            this.lblRemark.Control = this.txtRemark;
            this.lblRemark.CustomizationFormText = "备注：";
            this.lblRemark.Location = new System.Drawing.Point(0, 25);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(507, 102);
            this.lblRemark.Text = "备注：";
            this.lblRemark.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lblGroupDescription
            // 
            this.lblGroupDescription.Control = this.txtGroupDescription;
            this.lblGroupDescription.CustomizationFormText = "描述：";
            this.lblGroupDescription.Location = new System.Drawing.Point(253, 0);
            this.lblGroupDescription.Name = "lblGroupDescription";
            this.lblGroupDescription.Size = new System.Drawing.Size(254, 25);
            this.lblGroupDescription.Text = "描述：";
            this.lblGroupDescription.TextSize = new System.Drawing.Size(36, 14);
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnOk);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBottom.Location = new System.Drawing.Point(3, 177);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(527, 35);
            this.panelControlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(425, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(328, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AddOperationGroup
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 215);
            this.Controls.Add(this.tableLayoutPanelOperation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LookAndFeel.SkinName = "iMaginary";
            this.Name = "AddOperationGroup";
            this.Load += new System.EventHandler(this.AddOperationGroup_Load);
            this.tableLayoutPanelOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlOperation)).EndInit();
            this.layoutControlOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGrpOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblGroupDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOperation;
        private DevExpress.XtraEditors.MemoEdit txtRemark;
        private DevExpress.XtraEditors.TextEdit txtGroupDescription;
        private DevExpress.XtraEditors.TextEdit txtGroupName;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControl layoutControlOperation;
        private DevExpress.XtraLayout.LayoutControlGroup layoutCtlGrpOperation;
        private DevExpress.XtraLayout.LayoutControlItem lblGroupName;
        private DevExpress.XtraLayout.LayoutControlItem lblGroupDescription;
        private DevExpress.XtraLayout.LayoutControlItem lblRemark;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;
    }
}
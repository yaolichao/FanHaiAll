namespace FanHai.Hemera.Addins.RBAC
{
    partial class AddResourceGroup
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
            this.tableLayoutPanelResource = new System.Windows.Forms.TableLayoutPanel();
            this.layoutControlResource = new DevExpress.XtraLayout.LayoutControl();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.txtRemark = new DevExpress.XtraEditors.MemoEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.layoutCtlGroupResource = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRemark = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControlResource = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlResource)).BeginInit();
            this.layoutControlResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGroupResource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlResource)).BeginInit();
            this.panelControlResource.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelResource
            // 
            this.tableLayoutPanelResource.ColumnCount = 1;
            this.tableLayoutPanelResource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelResource.Controls.Add(this.layoutControlResource, 0, 0);
            this.tableLayoutPanelResource.Controls.Add(this.panelControlResource, 0, 1);
            this.tableLayoutPanelResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelResource.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelResource.Name = "tableLayoutPanelResource";
            this.tableLayoutPanelResource.RowCount = 2;
            this.tableLayoutPanelResource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelResource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanelResource.Size = new System.Drawing.Size(544, 269);
            this.tableLayoutPanelResource.TabIndex = 0;
            // 
            // layoutControlResource
            // 
            this.layoutControlResource.Controls.Add(this.txtCode);
            this.layoutControlResource.Controls.Add(this.txtRemark);
            this.layoutControlResource.Controls.Add(this.txtName);
            this.layoutControlResource.Controls.Add(this.txtDescription);
            this.layoutControlResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlResource.Location = new System.Drawing.Point(3, 3);
            this.layoutControlResource.Name = "layoutControlResource";
            this.layoutControlResource.Root = this.layoutCtlGroupResource;
            this.layoutControlResource.Size = new System.Drawing.Size(538, 222);
            this.layoutControlResource.TabIndex = 2;
            this.layoutControlResource.Text = "layoutControl1";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(311, 33);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(215, 21);
            this.txtCode.StyleController = this.layoutControlResource;
            this.txtCode.TabIndex = 7;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(52, 83);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(474, 127);
            this.txtRemark.StyleController = this.layoutControlResource;
            this.txtRemark.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(52, 33);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(215, 21);
            this.txtName.StyleController = this.layoutControlResource;
            this.txtName.TabIndex = 5;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(52, 58);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(474, 21);
            this.txtDescription.StyleController = this.layoutControlResource;
            this.txtDescription.TabIndex = 3;
            // 
            // layoutCtlGroupResource
            // 
            this.layoutCtlGroupResource.CustomizationFormText = "资源组";
            this.layoutCtlGroupResource.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutCtlGroupResource.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblName,
            this.lblDescription,
            this.lblRemark,
            this.lblCode});
            this.layoutCtlGroupResource.Location = new System.Drawing.Point(0, 0);
            this.layoutCtlGroupResource.Name = "layoutCtlGroupResource";
            this.layoutCtlGroupResource.Size = new System.Drawing.Size(538, 222);
            this.layoutCtlGroupResource.Text = "资源组";
            // 
            // lblName
            // 
            this.lblName.Control = this.txtName;
            this.lblName.CustomizationFormText = "名称：";
            this.lblName.Location = new System.Drawing.Point(0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(259, 25);
            this.lblName.Text = "名称：";
            this.lblName.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lblDescription
            // 
            this.lblDescription.Control = this.txtDescription;
            this.lblDescription.CustomizationFormText = "描述：";
            this.lblDescription.Location = new System.Drawing.Point(0, 25);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(518, 25);
            this.lblDescription.Text = "描述：";
            this.lblDescription.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lblRemark
            // 
            this.lblRemark.Control = this.txtRemark;
            this.lblRemark.CustomizationFormText = "备注：";
            this.lblRemark.Location = new System.Drawing.Point(0, 50);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(518, 131);
            this.lblRemark.Text = "备注：";
            this.lblRemark.TextSize = new System.Drawing.Size(36, 14);
            // 
            // lblCode
            // 
            this.lblCode.Control = this.txtCode;
            this.lblCode.CustomizationFormText = "编码：";
            this.lblCode.Location = new System.Drawing.Point(259, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(259, 25);
            this.lblCode.Text = "编码：";
            this.lblCode.TextSize = new System.Drawing.Size(36, 14);
            // 
            // panelControlResource
            // 
            this.panelControlResource.Controls.Add(this.btnCancel);
            this.panelControlResource.Controls.Add(this.btnOk);
            this.panelControlResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlResource.Location = new System.Drawing.Point(3, 231);
            this.panelControlResource.Name = "panelControlResource";
            this.panelControlResource.Size = new System.Drawing.Size(538, 35);
            this.panelControlResource.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(436, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(340, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AddResourceGroup
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 269);
            this.Controls.Add(this.tableLayoutPanelResource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "AddResourceGroup";
            this.Load += new System.EventHandler(this.AddResourceGroup_Load);
            this.tableLayoutPanelResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlResource)).EndInit();
            this.layoutControlResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGroupResource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlResource)).EndInit();
            this.panelControlResource.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResource;
        private DevExpress.XtraEditors.MemoEdit txtRemark;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private DevExpress.XtraLayout.LayoutControl layoutControlResource;
        private DevExpress.XtraLayout.LayoutControlGroup layoutCtlGroupResource;
        private DevExpress.XtraLayout.LayoutControlItem lblName;
        private DevExpress.XtraLayout.LayoutControlItem lblCode;
        private DevExpress.XtraLayout.LayoutControlItem lblDescription;
        private DevExpress.XtraLayout.LayoutControlItem lblRemark;
        private DevExpress.XtraEditors.PanelControl panelControlResource;
    }
}
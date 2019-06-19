namespace FanHai.Hemera.Addins.RBAC
{
    partial class DistributePrivilege
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributePrivilege));
            this.groupControlMain = new DevExpress.XtraEditors.GroupControl();
            this.lblResourceGroup = new DevExpress.XtraEditors.LabelControl();
            this.lblResourceName = new DevExpress.XtraEditors.LabelControl();
            this.lblTypeName = new DevExpress.XtraEditors.LabelControl();
            this.lblType = new DevExpress.XtraEditors.LabelControl();
            this.OperationGroup = new DevExpress.XtraEditors.GroupControl();
            this.lueOperationGroup = new DevExpress.XtraEditors.LookUpEdit();
            this.lblOperationGroup = new DevExpress.XtraEditors.LabelControl();
            this.operationControl = new DevExpress.XtraEditors.GroupControl();
            this.privilegeGridView = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            this.ResourceGroup = new DevExpress.XtraEditors.GroupControl();
            this.resourceTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).BeginInit();
            this.groupControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroup)).BeginInit();
            this.OperationGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationControl)).BeginInit();
            this.operationControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.privilegeGridView)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
            this.splitContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGroup)).BeginInit();
            this.ResourceGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelRight
            // 
            tableLayoutPanelRight.ColumnCount = 1;
            tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelRight.Controls.Add(this.groupControlMain, 0, 0);
            tableLayoutPanelRight.Controls.Add(this.OperationGroup, 0, 1);
            tableLayoutPanelRight.Controls.Add(this.operationControl, 0, 2);
            tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            tableLayoutPanelRight.RowCount = 3;
            tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelRight.Size = new System.Drawing.Size(911, 530);
            tableLayoutPanelRight.TabIndex = 0;
            // 
            // groupControlMain
            // 
            this.groupControlMain.Controls.Add(this.lblResourceGroup);
            this.groupControlMain.Controls.Add(this.lblResourceName);
            this.groupControlMain.Controls.Add(this.lblTypeName);
            this.groupControlMain.Controls.Add(this.lblType);
            this.groupControlMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControlMain.Location = new System.Drawing.Point(3, 5);
            this.groupControlMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlMain.Name = "groupControlMain";
            this.groupControlMain.Size = new System.Drawing.Size(905, 85);
            this.groupControlMain.TabIndex = 0;
            // 
            // lblResourceGroup
            // 
            this.lblResourceGroup.Location = new System.Drawing.Point(144, 41);
            this.lblResourceGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblResourceGroup.Name = "lblResourceGroup";
            this.lblResourceGroup.Size = new System.Drawing.Size(111, 18);
            this.lblResourceGroup.TabIndex = 3;
            this.lblResourceGroup.Text = "lblResourceGroup";
            // 
            // lblResourceName
            // 
            this.lblResourceName.Location = new System.Drawing.Point(18, 40);
            this.lblResourceName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblResourceName.Name = "lblResourceName";
            this.lblResourceName.Size = new System.Drawing.Size(90, 18);
            this.lblResourceName.TabIndex = 2;
            this.lblResourceName.Text = "当前资源组：";
            // 
            // lblTypeName
            // 
            this.lblTypeName.Location = new System.Drawing.Point(421, 42);
            this.lblTypeName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblTypeName.Name = "lblTypeName";
            this.lblTypeName.Size = new System.Drawing.Size(85, 18);
            this.lblTypeName.TabIndex = 1;
            this.lblTypeName.Text = "lblTypeName";
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(325, 42);
            this.lblType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(46, 18);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "lblType";
            // 
            // OperationGroup
            // 
            this.OperationGroup.Controls.Add(this.lueOperationGroup);
            this.OperationGroup.Controls.Add(this.lblOperationGroup);
            this.OperationGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OperationGroup.Location = new System.Drawing.Point(3, 98);
            this.OperationGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OperationGroup.Name = "OperationGroup";
            this.OperationGroup.Size = new System.Drawing.Size(905, 94);
            this.OperationGroup.TabIndex = 1;
            this.OperationGroup.Text = "操作组";
            // 
            // lueOperationGroup
            // 
            this.lueOperationGroup.Location = new System.Drawing.Point(158, 48);
            this.lueOperationGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueOperationGroup.Name = "lueOperationGroup";
            this.lueOperationGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueOperationGroup.Properties.NullText = "";
            this.lueOperationGroup.Size = new System.Drawing.Size(222, 24);
            this.lueOperationGroup.TabIndex = 1;
            this.lueOperationGroup.EditValueChanged += new System.EventHandler(this.lueOperationGroup_EditValueChanged);
            // 
            // lblOperationGroup
            // 
            this.lblOperationGroup.Location = new System.Drawing.Point(18, 50);
            this.lblOperationGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblOperationGroup.Name = "lblOperationGroup";
            this.lblOperationGroup.Size = new System.Drawing.Size(90, 18);
            this.lblOperationGroup.TabIndex = 0;
            this.lblOperationGroup.Text = "请选择操作组";
            // 
            // operationControl
            // 
            this.operationControl.Controls.Add(this.privilegeGridView);
            this.operationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationControl.Location = new System.Drawing.Point(3, 200);
            this.operationControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.operationControl.Name = "operationControl";
            this.operationControl.Size = new System.Drawing.Size(905, 326);
            this.operationControl.TabIndex = 2;
            // 
            // privilegeGridView
            // 
            this.privilegeGridView.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.privilegeGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.privilegeGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.privilegeGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.privilegeGridView.EnableHeadersVisualStyles = false;
            this.privilegeGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.privilegeGridView.Location = new System.Drawing.Point(2, 28);
            this.privilegeGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.privilegeGridView.Name = "privilegeGridView";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.privilegeGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.privilegeGridView.RowTemplate.Height = 23;
            this.privilegeGridView.Size = new System.Drawing.Size(901, 296);
            this.privilegeGridView.TabIndex = 0;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.panelControl1, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerControl, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1131, 591);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 542);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1125, 45);
            this.panelControl1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1018, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(890, 8);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // splitContainerControl
            // 
            this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl.Location = new System.Drawing.Point(3, 4);
            this.splitContainerControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl.Name = "splitContainerControl";
            this.splitContainerControl.Panel1.Controls.Add(this.ResourceGroup);
            this.splitContainerControl.Panel1.Text = "Panel1";
            this.splitContainerControl.Panel2.Controls.Add(tableLayoutPanelRight);
            this.splitContainerControl.Panel2.Text = "Panel2";
            this.splitContainerControl.Size = new System.Drawing.Size(1125, 530);
            this.splitContainerControl.SplitterPosition = 207;
            this.splitContainerControl.TabIndex = 1;
            this.splitContainerControl.Text = "splitContainerControl1";
            // 
            // ResourceGroup
            // 
            this.ResourceGroup.Controls.Add(this.resourceTree);
            this.ResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResourceGroup.Location = new System.Drawing.Point(0, 0);
            this.ResourceGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ResourceGroup.Name = "ResourceGroup";
            this.ResourceGroup.Size = new System.Drawing.Size(207, 530);
            this.ResourceGroup.TabIndex = 0;
            this.ResourceGroup.Text = "资源组";
            // 
            // resourceTree
            // 
            this.resourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceTree.ImageIndex = 0;
            this.resourceTree.ImageList = this.imageList1;
            this.resourceTree.Location = new System.Drawing.Point(2, 28);
            this.resourceTree.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resourceTree.Name = "resourceTree";
            this.resourceTree.SelectedImageIndex = 0;
            this.resourceTree.Size = new System.Drawing.Size(203, 500);
            this.resourceTree.TabIndex = 1;
            this.resourceTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.resourceTree_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "00032.ico");
            this.imageList1.Images.SetKeyName(1, "00023.ico");
            // 
            // DistributePrivilege
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 591);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "DistributePrivilege";
            this.Load += new System.EventHandler(this.DistributePrivilege_Load);
            tableLayoutPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMain)).EndInit();
            this.groupControlMain.ResumeLayout(false);
            this.groupControlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroup)).EndInit();
            this.OperationGroup.ResumeLayout(false);
            this.OperationGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueOperationGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationControl)).EndInit();
            this.operationControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.privilegeGridView)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
            this.splitContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGroup)).EndInit();
            this.ResourceGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
        private DevExpress.XtraEditors.GroupControl ResourceGroup;
        private System.Windows.Forms.TreeView resourceTree;
        private DevExpress.XtraEditors.GroupControl groupControlMain;
        private DevExpress.XtraEditors.LabelControl lblResourceGroup;
        private DevExpress.XtraEditors.LabelControl lblResourceName;
        private DevExpress.XtraEditors.LabelControl lblTypeName;
        private DevExpress.XtraEditors.LabelControl lblType;
        private DevExpress.XtraEditors.GroupControl OperationGroup;
        private DevExpress.XtraEditors.LookUpEdit lueOperationGroup;
        private DevExpress.XtraEditors.LabelControl lblOperationGroup;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.GroupControl operationControl;
        private System.Windows.Forms.DataGridView privilegeGridView;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
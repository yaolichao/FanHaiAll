namespace FanHai.Hemera.Addins.RBAC
{
    partial class OperationPrivilege
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupControlTop = new DevExpress.XtraEditors.GroupControl();
            this.lblRoleName = new DevExpress.XtraEditors.LabelControl();
            this.txtRoleName = new DevExpress.XtraEditors.TextEdit();
            this.tableLayoutPanelMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.groupControlMiddle = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanelCenter = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.UnSelectGroup = new DevExpress.XtraEditors.GroupControl();
            this.clbUnSelectOperation = new System.Windows.Forms.CheckedListBox();
            this.selectGroup = new DevExpress.XtraEditors.GroupControl();
            this.clbSelectOperation = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).BeginInit();
            this.groupControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).BeginInit();
            this.tableLayoutPanelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMiddle)).BeginInit();
            this.groupControlMiddle.SuspendLayout();
            this.tableLayoutPanelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).BeginInit();
            this.UnSelectGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).BeginInit();
            this.selectGroup.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.groupControlTop, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelMiddle, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelBottom, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(749, 590);
            this.tableLayoutPanelMain.TabIndex = 10;
            // 
            // groupControlTop
            // 
            this.groupControlTop.Controls.Add(this.lblRoleName);
            this.groupControlTop.Controls.Add(this.txtRoleName);
            this.groupControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlTop.Location = new System.Drawing.Point(3, 4);
            this.groupControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlTop.Name = "groupControlTop";
            this.groupControlTop.ShowCaption = false;
            this.groupControlTop.Size = new System.Drawing.Size(743, 67);
            this.groupControlTop.TabIndex = 14;
            this.groupControlTop.Text = "groupControl2";
            // 
            // lblRoleName
            // 
            this.lblRoleName.Location = new System.Drawing.Point(10, 26);
            this.lblRoleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(60, 18);
            this.lblRoleName.TabIndex = 2;
            this.lblRoleName.Text = "角色名：";
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(69, 22);
            this.txtRoleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(397, 24);
            this.txtRoleName.TabIndex = 3;
            // 
            // tableLayoutPanelMiddle
            // 
            this.tableLayoutPanelMiddle.ColumnCount = 3;
            this.tableLayoutPanelMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanelMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMiddle.Controls.Add(this.groupControlMiddle, 1, 0);
            this.tableLayoutPanelMiddle.Controls.Add(this.UnSelectGroup, 2, 0);
            this.tableLayoutPanelMiddle.Controls.Add(this.selectGroup, 0, 0);
            this.tableLayoutPanelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMiddle.Location = new System.Drawing.Point(3, 79);
            this.tableLayoutPanelMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelMiddle.Name = "tableLayoutPanelMiddle";
            this.tableLayoutPanelMiddle.RowCount = 1;
            this.tableLayoutPanelMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMiddle.Size = new System.Drawing.Size(743, 454);
            this.tableLayoutPanelMiddle.TabIndex = 1;
            // 
            // groupControlMiddle
            // 
            this.groupControlMiddle.Controls.Add(this.tableLayoutPanelCenter);
            this.groupControlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlMiddle.Location = new System.Drawing.Point(327, 4);
            this.groupControlMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControlMiddle.Name = "groupControlMiddle";
            this.groupControlMiddle.ShowCaption = false;
            this.groupControlMiddle.Size = new System.Drawing.Size(88, 446);
            this.groupControlMiddle.TabIndex = 2;
            this.groupControlMiddle.Text = "groupControl1";
            // 
            // tableLayoutPanelCenter
            // 
            this.tableLayoutPanelCenter.ColumnCount = 1;
            this.tableLayoutPanelCenter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCenter.Controls.Add(this.btnDelete, 0, 1);
            this.tableLayoutPanelCenter.Controls.Add(this.btnAdd, 0, 2);
            this.tableLayoutPanelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCenter.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelCenter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelCenter.Name = "tableLayoutPanelCenter";
            this.tableLayoutPanelCenter.RowCount = 4;
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanelCenter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            this.tableLayoutPanelCenter.Size = new System.Drawing.Size(84, 442);
            this.tableLayoutPanelCenter.TabIndex = 9;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(3, 164);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(66, 35);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = ">>";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 219);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(67, 35);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "<<";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // UnSelectGroup
            // 
            this.UnSelectGroup.Controls.Add(this.clbUnSelectOperation);
            this.UnSelectGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnSelectGroup.Location = new System.Drawing.Point(421, 4);
            this.UnSelectGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UnSelectGroup.Name = "UnSelectGroup";
            this.UnSelectGroup.Size = new System.Drawing.Size(319, 446);
            this.UnSelectGroup.TabIndex = 2;
            this.UnSelectGroup.Text = "未选工序";
            // 
            // clbUnSelectOperation
            // 
            this.clbUnSelectOperation.CheckOnClick = true;
            this.clbUnSelectOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbUnSelectOperation.FormattingEnabled = true;
            this.clbUnSelectOperation.Location = new System.Drawing.Point(2, 28);
            this.clbUnSelectOperation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clbUnSelectOperation.Name = "clbUnSelectOperation";
            this.clbUnSelectOperation.Size = new System.Drawing.Size(315, 416);
            this.clbUnSelectOperation.TabIndex = 12;
            // 
            // selectGroup
            // 
            this.selectGroup.Controls.Add(this.clbSelectOperation);
            this.selectGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectGroup.Location = new System.Drawing.Point(3, 4);
            this.selectGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Size = new System.Drawing.Size(318, 446);
            this.selectGroup.TabIndex = 2;
            this.selectGroup.Text = "已有工序";
            // 
            // clbSelectOperation
            // 
            this.clbSelectOperation.CheckOnClick = true;
            this.clbSelectOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbSelectOperation.FormattingEnabled = true;
            this.clbSelectOperation.Location = new System.Drawing.Point(2, 28);
            this.clbSelectOperation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clbSelectOperation.Name = "clbSelectOperation";
            this.clbSelectOperation.Size = new System.Drawing.Size(314, 416);
            this.clbSelectOperation.TabIndex = 13;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.ColumnCount = 4;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanelBottom.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(3, 541);
            this.tableLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(743, 45);
            this.tableLayoutPanelBottom.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(419, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(637, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // OperationPrivilege
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 590);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "OperationPrivilege";
            this.Load += new System.EventHandler(this.OperationPrivilege_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).EndInit();
            this.groupControlTop.ResumeLayout(false);
            this.groupControlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).EndInit();
            this.tableLayoutPanelMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMiddle)).EndInit();
            this.groupControlMiddle.ResumeLayout(false);
            this.tableLayoutPanelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnSelectGroup)).EndInit();
            this.UnSelectGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.selectGroup)).EndInit();
            this.selectGroup.ResumeLayout(false);
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMiddle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.GroupControl selectGroup;
        private System.Windows.Forms.CheckedListBox clbSelectOperation;
        private DevExpress.XtraEditors.GroupControl UnSelectGroup;
        private System.Windows.Forms.CheckedListBox clbUnSelectOperation;
        private DevExpress.XtraEditors.GroupControl groupControlTop;
        private DevExpress.XtraEditors.LabelControl lblRoleName;
        private DevExpress.XtraEditors.TextEdit txtRoleName;
        private DevExpress.XtraEditors.GroupControl groupControlMiddle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCenter;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
    }
}
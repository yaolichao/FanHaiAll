namespace FanHai.Hemera.Addins.RBAC
{
    partial class ChangePasswordDialog
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
            this.tableLayoutPanelModifyPwd = new System.Windows.Forms.TableLayoutPanel();
            this.layoutControlMdiPsw = new DevExpress.XtraLayout.LayoutControl();
            this.tePswAgain = new DevExpress.XtraEditors.TextEdit();
            this.teUserName = new DevExpress.XtraEditors.TextEdit();
            this.tePswOld = new DevExpress.XtraEditors.TextEdit();
            this.tePswNew = new DevExpress.XtraEditors.TextEdit();
            this.layoutCtlGrpChangePwd = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblPswOld = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblUserName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblPswNew = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblPswAgain = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanelModifyPwd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMdiPsw)).BeginInit();
            this.layoutControlMdiPsw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tePswAgain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePswOld.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePswNew.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGrpChangePwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswOld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswAgain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModifyPwd
            // 
            this.tableLayoutPanelModifyPwd.ColumnCount = 1;
            this.tableLayoutPanelModifyPwd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 390F));
            this.tableLayoutPanelModifyPwd.Controls.Add(this.layoutControlMdiPsw, 0, 0);
            this.tableLayoutPanelModifyPwd.Controls.Add(this.panelControlBottom, 0, 1);
            this.tableLayoutPanelModifyPwd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelModifyPwd.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelModifyPwd.Name = "tableLayoutPanelModifyPwd";
            this.tableLayoutPanelModifyPwd.RowCount = 2;
            this.tableLayoutPanelModifyPwd.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.43662F));
            this.tableLayoutPanelModifyPwd.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelModifyPwd.Size = new System.Drawing.Size(390, 212);
            this.tableLayoutPanelModifyPwd.TabIndex = 0;
            // 
            // layoutControlMdiPsw
            // 
            this.layoutControlMdiPsw.Controls.Add(this.tePswAgain);
            this.layoutControlMdiPsw.Controls.Add(this.teUserName);
            this.layoutControlMdiPsw.Controls.Add(this.tePswOld);
            this.layoutControlMdiPsw.Controls.Add(this.tePswNew);
            this.layoutControlMdiPsw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlMdiPsw.Location = new System.Drawing.Point(3, 3);
            this.layoutControlMdiPsw.Name = "layoutControlMdiPsw";
            this.layoutControlMdiPsw.Root = this.layoutCtlGrpChangePwd;
            this.layoutControlMdiPsw.Size = new System.Drawing.Size(384, 154);
            this.layoutControlMdiPsw.TabIndex = 10;
            this.layoutControlMdiPsw.Text = "layoutControlMdiPsw";
            // 
            // tePswAgain
            // 
            this.tePswAgain.Location = new System.Drawing.Point(77, 88);
            this.tePswAgain.Name = "tePswAgain";
            this.tePswAgain.Properties.PasswordChar = '*';
            this.tePswAgain.Size = new System.Drawing.Size(294, 21);
            this.tePswAgain.StyleController = this.layoutControlMdiPsw;
            this.tePswAgain.TabIndex = 7;
            // 
            // teUserName
            // 
            this.teUserName.Location = new System.Drawing.Point(77, 13);
            this.teUserName.Name = "teUserName";
            this.teUserName.Properties.ReadOnly = true;
            this.teUserName.Size = new System.Drawing.Size(294, 21);
            this.teUserName.StyleController = this.layoutControlMdiPsw;
            this.teUserName.TabIndex = 1;
            // 
            // tePswOld
            // 
            this.tePswOld.Location = new System.Drawing.Point(77, 38);
            this.tePswOld.Name = "tePswOld";
            this.tePswOld.Properties.PasswordChar = '*';
            this.tePswOld.Size = new System.Drawing.Size(294, 21);
            this.tePswOld.StyleController = this.layoutControlMdiPsw;
            this.tePswOld.TabIndex = 3;
            // 
            // tePswNew
            // 
            this.tePswNew.Location = new System.Drawing.Point(77, 63);
            this.tePswNew.Name = "tePswNew";
            this.tePswNew.Properties.PasswordChar = '*';
            this.tePswNew.Size = new System.Drawing.Size(294, 21);
            this.tePswNew.StyleController = this.layoutControlMdiPsw;
            this.tePswNew.TabIndex = 5;
            // 
            // layoutCtlGrpChangePwd
            // 
            this.layoutCtlGrpChangePwd.CustomizationFormText = "layoutCtlGrpChangePwd";
            this.layoutCtlGrpChangePwd.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutCtlGrpChangePwd.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblPswOld,
            this.lblUserName,
            this.lblPswNew,
            this.lblPswAgain});
            this.layoutCtlGrpChangePwd.Location = new System.Drawing.Point(0, 0);
            this.layoutCtlGrpChangePwd.Name = "layoutCtlGrpChangePwd";
            this.layoutCtlGrpChangePwd.Size = new System.Drawing.Size(384, 154);
            this.layoutCtlGrpChangePwd.Text = "layoutCtlGrpChangePwd";
            this.layoutCtlGrpChangePwd.TextVisible = false;
            // 
            // lblPswOld
            // 
            this.lblPswOld.Control = this.tePswOld;
            this.lblPswOld.CustomizationFormText = "原密码：";
            this.lblPswOld.Location = new System.Drawing.Point(0, 25);
            this.lblPswOld.Name = "lblPswOld";
            this.lblPswOld.Size = new System.Drawing.Size(362, 25);
            this.lblPswOld.Text = "原密码：";
            this.lblPswOld.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lblUserName
            // 
            this.lblUserName.Control = this.teUserName;
            this.lblUserName.CustomizationFormText = "用户名：";
            this.lblUserName.Location = new System.Drawing.Point(0, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(362, 25);
            this.lblUserName.Text = "用户名：";
            this.lblUserName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lblPswNew
            // 
            this.lblPswNew.Control = this.tePswNew;
            this.lblPswNew.CustomizationFormText = "新密码：";
            this.lblPswNew.Location = new System.Drawing.Point(0, 50);
            this.lblPswNew.Name = "lblPswNew";
            this.lblPswNew.Size = new System.Drawing.Size(362, 25);
            this.lblPswNew.Text = "新密码：";
            this.lblPswNew.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lblPswAgain
            // 
            this.lblPswAgain.Control = this.tePswAgain;
            this.lblPswAgain.CustomizationFormText = "确认密码：";
            this.lblPswAgain.Location = new System.Drawing.Point(0, 75);
            this.lblPswAgain.Name = "lblPswAgain";
            this.lblPswAgain.Size = new System.Drawing.Size(362, 57);
            this.lblPswAgain.Text = "确认密码：";
            this.lblPswAgain.TextSize = new System.Drawing.Size(60, 14);
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btCancel);
            this.panelControlBottom.Controls.Add(this.btConfirm);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlBottom.Location = new System.Drawing.Point(3, 163);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(384, 46);
            this.panelControlBottom.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(229, 12);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 9;
            this.btCancel.Text = "取消";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btConfirm
            // 
            this.btConfirm.Location = new System.Drawing.Point(90, 12);
            this.btConfirm.Name = "btConfirm";
            this.btConfirm.Size = new System.Drawing.Size(75, 23);
            this.btConfirm.TabIndex = 8;
            this.btConfirm.Text = "确定";
            this.btConfirm.Click += new System.EventHandler(this.btConfirm_Click);
            // 
            // ChangePasswordDialog
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 212);
            this.Controls.Add(this.tableLayoutPanelModifyPwd);
            this.LookAndFeel.SkinName = "iMaginary";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Name = "ChangePasswordDialog";
            this.Load += new System.EventHandler(this.ChangePasswordDialog_Load);
            this.tableLayoutPanelModifyPwd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMdiPsw)).EndInit();
            this.layoutControlMdiPsw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tePswAgain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePswOld.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePswNew.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCtlGrpChangePwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswOld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblPswAgain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModifyPwd;
        private DevExpress.XtraEditors.TextEdit tePswOld;
        private DevExpress.XtraEditors.TextEdit teUserName;
        private DevExpress.XtraEditors.SimpleButton btCancel;
        private DevExpress.XtraEditors.SimpleButton btConfirm;
        private DevExpress.XtraEditors.TextEdit tePswAgain;
        private DevExpress.XtraEditors.TextEdit tePswNew;
        private DevExpress.XtraLayout.LayoutControl layoutControlMdiPsw;
        private DevExpress.XtraLayout.LayoutControlGroup layoutCtlGrpChangePwd;
        private DevExpress.XtraLayout.LayoutControlItem lblPswAgain;
        private DevExpress.XtraLayout.LayoutControlItem lblPswNew;
        private DevExpress.XtraLayout.LayoutControlItem lblUserName;
        private DevExpress.XtraLayout.LayoutControlItem lblPswOld;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;

    }
}
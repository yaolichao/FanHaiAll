namespace FanHai.Hemera.Addins.RBAC
{
    partial class ResourceCtrl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceCtrl));
            this.ResourceGroup = new DevExpress.XtraEditors.GroupControl();
            this.resourceTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.resourceControl = new DevExpress.XtraGrid.GridControl();
            this.resourceView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Resource_key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Resource_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.description = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Resource_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.resourceGroupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.editor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.edit_timeZone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlResource = new DevExpress.XtraLayout.LayoutControl();
            this.txtResourceName = new DevExpress.XtraEditors.TextEdit();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.txtRemark = new DevExpress.XtraEditors.MemoEdit();
            this.tsbDelete = new DevExpress.XtraEditors.SimpleButton();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroupResource = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblResourceName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblResourceCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRemark = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGroup)).BeginInit();
            this.ResourceGroup.SuspendLayout();
            this.tableLayoutPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resourceControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlResource)).BeginInit();
            this.layoutControlResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtResourceName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupResource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResourceName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResourceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(872, 47);
            this.topPanel.TabIndex = 0;
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(687, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-15 10:07:45";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(256, 23);
            this.lblMenu.Text = "平台管理>系统管理>功能分类";
            // 
            // ResourceGroup
            // 
            this.ResourceGroup.Controls.Add(this.resourceTree);
            this.ResourceGroup.Location = new System.Drawing.Point(11, 31);
            this.ResourceGroup.Name = "ResourceGroup";
            this.ResourceGroup.Size = new System.Drawing.Size(322, 256);
            this.ResourceGroup.TabIndex = 0;
            this.ResourceGroup.Text = "资源组";
            // 
            // resourceTree
            // 
            this.resourceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceTree.ImageIndex = 0;
            this.resourceTree.ImageList = this.imageList;
            this.resourceTree.Location = new System.Drawing.Point(2, 23);
            this.resourceTree.Name = "resourceTree";
            this.resourceTree.SelectedImageIndex = 0;
            this.resourceTree.Size = new System.Drawing.Size(318, 231);
            this.resourceTree.TabIndex = 0;
            this.resourceTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.resourceTree_NodeMouseClick);
            this.resourceTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.resourceTree_MouseUp);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "00032.ico");
            this.imageList.Images.SetKeyName(1, "00023.ico");
            // 
            // tableLayoutPanelRight
            // 
            this.tableLayoutPanelRight.ColumnCount = 1;
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRight.Controls.Add(this.resourceControl, 0, 0);
            this.tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            this.tableLayoutPanelRight.RowCount = 1;
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 509F));
            this.tableLayoutPanelRight.Size = new System.Drawing.Size(522, 509);
            this.tableLayoutPanelRight.TabIndex = 0;
            this.tableLayoutPanelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanelRight_Paint);
            // 
            // resourceControl
            // 
            this.resourceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceControl.Location = new System.Drawing.Point(3, 3);
            this.resourceControl.MainView = this.resourceView;
            this.resourceControl.Name = "resourceControl";
            this.resourceControl.Size = new System.Drawing.Size(516, 503);
            this.resourceControl.TabIndex = 0;
            this.resourceControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.resourceView});
            // 
            // resourceView
            // 
            this.resourceView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Resource_key,
            this.Resource_Name,
            this.description,
            this.Resource_Code,
            this.Remark,
            this.resourceGroupKey,
            this.editor,
            this.edit_timeZone});
            this.resourceView.GridControl = this.resourceControl;
            this.resourceView.Name = "resourceView";
            this.resourceView.OptionsView.ShowGroupPanel = false;
            this.resourceView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.resourceView_RowClick);
            this.resourceView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.resourceView_CustomDrawRowIndicator);
            // 
            // Resource_key
            // 
            this.Resource_key.Caption = "资源id";
            this.Resource_key.FieldName = "RESOURCE_KEY";
            this.Resource_key.Name = "Resource_key";
            // 
            // Resource_Name
            // 
            this.Resource_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.Resource_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Resource_Name.Caption = "资源名称";
            this.Resource_Name.FieldName = "RESOURCE_NAME";
            this.Resource_Name.Name = "Resource_Name";
            this.Resource_Name.OptionsColumn.AllowEdit = false;
            this.Resource_Name.Visible = true;
            this.Resource_Name.VisibleIndex = 0;
            // 
            // description
            // 
            this.description.AppearanceHeader.Options.UseTextOptions = true;
            this.description.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.description.Caption = "描述";
            this.description.FieldName = "DESCRIPTIONS";
            this.description.Name = "description";
            this.description.OptionsColumn.AllowEdit = false;
            this.description.Visible = true;
            this.description.VisibleIndex = 2;
            // 
            // Resource_Code
            // 
            this.Resource_Code.AppearanceHeader.Options.UseTextOptions = true;
            this.Resource_Code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Resource_Code.Caption = "编码";
            this.Resource_Code.FieldName = "RESOURCE_CODE";
            this.Resource_Code.Name = "Resource_Code";
            this.Resource_Code.OptionsColumn.AllowEdit = false;
            this.Resource_Code.Visible = true;
            this.Resource_Code.VisibleIndex = 1;
            // 
            // Remark
            // 
            this.Remark.AppearanceHeader.Options.UseTextOptions = true;
            this.Remark.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Remark.Caption = "备注";
            this.Remark.FieldName = "REMARK";
            this.Remark.Name = "Remark";
            this.Remark.OptionsColumn.AllowEdit = false;
            this.Remark.Visible = true;
            this.Remark.VisibleIndex = 3;
            // 
            // resourceGroupKey
            // 
            this.resourceGroupKey.Caption = "资源组id";
            this.resourceGroupKey.FieldName = "RESOURCE_GROUP_KEY";
            this.resourceGroupKey.Name = "resourceGroupKey";
            // 
            // editor
            // 
            this.editor.Caption = "修改者";
            this.editor.FieldName = "EDITOR";
            this.editor.Name = "editor";
            // 
            // edit_timeZone
            // 
            this.edit_timeZone.Caption = "时区";
            this.edit_timeZone.FieldName = "EDIT_TIMEZONE";
            this.edit_timeZone.Name = "edit_timeZone";
            // 
            // layoutControlResource
            // 
            this.layoutControlResource.Controls.Add(this.ResourceGroup);
            this.layoutControlResource.Controls.Add(this.txtResourceName);
            this.layoutControlResource.Controls.Add(this.txtDescription);
            this.layoutControlResource.Controls.Add(this.txtCode);
            this.layoutControlResource.Controls.Add(this.txtRemark);
            this.layoutControlResource.Controls.Add(this.tsbDelete);
            this.layoutControlResource.Controls.Add(this.tsbSave);
            this.layoutControlResource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlResource.Location = new System.Drawing.Point(0, 0);
            this.layoutControlResource.Name = "layoutControlResource";
            this.layoutControlResource.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3549, 338, 812, 500);
            this.layoutControlResource.Root = this.layoutControlGroupResource;
            this.layoutControlResource.Size = new System.Drawing.Size(344, 509);
            this.layoutControlResource.TabIndex = 2;
            this.layoutControlResource.Text = "layoutControl1";
            // 
            // txtResourceName
            // 
            this.txtResourceName.Location = new System.Drawing.Point(62, 376);
            this.txtResourceName.Name = "txtResourceName";
            this.txtResourceName.Size = new System.Drawing.Size(271, 20);
            this.txtResourceName.StyleController = this.layoutControlResource;
            this.txtResourceName.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(62, 352);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(271, 20);
            this.txtDescription.StyleController = this.layoutControlResource;
            this.txtDescription.TabIndex = 3;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(62, 328);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(271, 20);
            this.txtCode.StyleController = this.layoutControlResource;
            this.txtCode.TabIndex = 7;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(62, 400);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(271, 73);
            this.txtRemark.StyleController = this.layoutControlResource;
            this.txtRemark.TabIndex = 5;
            // 
            // tsbDelete
            // 
            this.tsbDelete.Location = new System.Drawing.Point(174, 477);
            this.tsbDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(159, 22);
            this.tsbDelete.StyleController = this.layoutControlResource;
            this.tsbDelete.TabIndex = 8;
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Location = new System.Drawing.Point(11, 477);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(159, 22);
            this.tsbSave.StyleController = this.layoutControlResource;
            this.tsbSave.TabIndex = 9;
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // layoutControlGroupResource
            // 
            this.layoutControlGroupResource.CustomizationFormText = " ";
            this.layoutControlGroupResource.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupResource.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.lblResourceName,
            this.lblResourceCode,
            this.lblDescription,
            this.lblRemark,
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.emptySpaceItem1});
            this.layoutControlGroupResource.Name = "Root";
            this.layoutControlGroupResource.Size = new System.Drawing.Size(344, 509);
            this.layoutControlGroupResource.Text = " ";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.ResourceGroup;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(326, 260);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // lblResourceName
            // 
            this.lblResourceName.Control = this.txtResourceName;
            this.lblResourceName.CustomizationFormText = "资源名：";
            this.lblResourceName.Location = new System.Drawing.Point(0, 345);
            this.lblResourceName.Name = "lblResourceName";
            this.lblResourceName.Size = new System.Drawing.Size(326, 24);
            this.lblResourceName.Text = "资源名：";
            this.lblResourceName.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lblResourceCode
            // 
            this.lblResourceCode.Control = this.txtCode;
            this.lblResourceCode.CustomizationFormText = "编码：";
            this.lblResourceCode.Location = new System.Drawing.Point(0, 297);
            this.lblResourceCode.Name = "lblResourceCode";
            this.lblResourceCode.Size = new System.Drawing.Size(326, 24);
            this.lblResourceCode.Text = "编码：";
            this.lblResourceCode.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lblDescription
            // 
            this.lblDescription.Control = this.txtDescription;
            this.lblDescription.CustomizationFormText = "描述：";
            this.lblDescription.Location = new System.Drawing.Point(0, 321);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(326, 24);
            this.lblDescription.Text = "描述：";
            this.lblDescription.TextSize = new System.Drawing.Size(48, 14);
            // 
            // lblRemark
            // 
            this.lblRemark.Control = this.txtRemark;
            this.lblRemark.CustomizationFormText = "备注：";
            this.lblRemark.Location = new System.Drawing.Point(0, 369);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(326, 77);
            this.lblRemark.Text = "备注：";
            this.lblRemark.TextSize = new System.Drawing.Size(48, 14);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tsbSave;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 446);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(163, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.tsbDelete;
            this.layoutControlItem1.Location = new System.Drawing.Point(163, 446);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(163, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 260);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(326, 37);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(1, 47);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControlResource);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.tableLayoutPanelRight);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(872, 509);
            this.splitContainerControl1.SplitterPosition = 344;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // ResourceCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ResourceCtrl";
            this.Size = new System.Drawing.Size(874, 556);
            this.Load += new System.EventHandler(this.ResourceCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceGroup)).EndInit();
            this.ResourceGroup.ResumeLayout(false);
            this.tableLayoutPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resourceControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlResource)).EndInit();
            this.layoutControlResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtResourceName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupResource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResourceName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblResourceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
        private DevExpress.XtraGrid.GridControl resourceControl;
        private DevExpress.XtraGrid.Views.Grid.GridView resourceView;
        private DevExpress.XtraEditors.GroupControl ResourceGroup;
        private System.Windows.Forms.TreeView resourceTree;
        private DevExpress.XtraEditors.TextEdit txtResourceName;
        private DevExpress.XtraEditors.MemoEdit txtRemark;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private DevExpress.XtraGrid.Columns.GridColumn Resource_key;
        private DevExpress.XtraGrid.Columns.GridColumn Resource_Name;
        private DevExpress.XtraGrid.Columns.GridColumn description;
        private DevExpress.XtraGrid.Columns.GridColumn Remark;
        private DevExpress.XtraGrid.Columns.GridColumn resourceGroupKey;
        private System.Windows.Forms.ImageList imageList;
        private DevExpress.XtraGrid.Columns.GridColumn editor;
        private DevExpress.XtraGrid.Columns.GridColumn edit_timeZone;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private DevExpress.XtraGrid.Columns.GridColumn Resource_Code;
        private DevExpress.XtraLayout.LayoutControl layoutControlResource;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupResource;
        private DevExpress.XtraLayout.LayoutControlItem lblResourceName;
        private DevExpress.XtraLayout.LayoutControlItem lblDescription;
        private DevExpress.XtraLayout.LayoutControlItem lblResourceCode;
        private DevExpress.XtraLayout.LayoutControlItem lblRemark;
        private DevExpress.XtraEditors.SimpleButton tsbDelete;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}

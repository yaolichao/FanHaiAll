namespace FanHai.Hemera.Addins.RBAC
{
    partial class OperationCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperationCtrl));
            this.OperationGroup = new DevExpress.XtraEditors.GroupControl();
            this.operationTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.layoutControlGrpOperation = new DevExpress.XtraLayout.LayoutControlGroup();
            this.operationControl = new DevExpress.XtraGrid.GridControl();
            this.operationView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Operation_Key = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Operation_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Display_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Remark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Operation_Code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.operationGroupKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.editor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.edit_timeZone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlOperation = new DevExpress.XtraLayout.LayoutControl();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.txtOperationName = new DevExpress.XtraEditors.TextEdit();
            this.txtDisplayName = new DevExpress.XtraEditors.TextEdit();
            this.txtRemark = new DevExpress.XtraEditors.MemoEdit();
            this.tsbSave = new DevExpress.XtraEditors.SimpleButton();
            this.tsbDelete = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroupOperation = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblRemark = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lblOperationName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDisplayName = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tableLayoutPanelRight = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).BeginInit();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroup)).BeginInit();
            this.OperationGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGrpOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlOperation)).BeginInit();
            this.layoutControlOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOperationName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.tableLayoutPanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.topPanel.Appearance.Options.UseBackColor = true;
            this.topPanel.Size = new System.Drawing.Size(914, 47);
            // 
            // lblInfos
            // 
            this.lblInfos.Location = new System.Drawing.Point(729, 0);
            this.lblInfos.Size = new System.Drawing.Size(185, 42);
            this.lblInfos.Text = "\t姓名：\n\t工号：\n\t登录时间：2019-01-14 17:31:01";
            // 
            // lblMenu
            // 
            this.lblMenu.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenu.Appearance.Options.UseFont = true;
            this.lblMenu.Size = new System.Drawing.Size(256, 23);
            this.lblMenu.Text = "平台管理>系统管理>功能权限";
            // 
            // OperationGroup
            // 
            this.OperationGroup.Controls.Add(this.operationTree);
            this.OperationGroup.Location = new System.Drawing.Point(11, 31);
            this.OperationGroup.Name = "OperationGroup";
            this.OperationGroup.Size = new System.Drawing.Size(275, 241);
            this.OperationGroup.TabIndex = 0;
            this.OperationGroup.Text = "操作组";
            // 
            // operationTree
            // 
            this.operationTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationTree.ImageIndex = 0;
            this.operationTree.ImageList = this.imageList;
            this.operationTree.Location = new System.Drawing.Point(2, 23);
            this.operationTree.Name = "operationTree";
            this.operationTree.SelectedImageIndex = 0;
            this.operationTree.Size = new System.Drawing.Size(271, 216);
            this.operationTree.TabIndex = 1;
            this.operationTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.operationTree_NodeMouseClick);
            this.operationTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.operationTree_MouseUp);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "00032.ico");
            this.imageList.Images.SetKeyName(1, "00023.ico");
            // 
            // layoutControlGrpOperation
            // 
            this.layoutControlGrpOperation.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGrpOperation.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGrpOperation.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGrpOperation.Name = "layoutControlGroup1";
            this.layoutControlGrpOperation.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGrpOperation.Size = new System.Drawing.Size(717, 157);
            // 
            // operationControl
            // 
            this.operationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operationControl.Location = new System.Drawing.Point(3, 3);
            this.operationControl.MainView = this.operationView;
            this.operationControl.Name = "operationControl";
            this.operationControl.Size = new System.Drawing.Size(601, 506);
            this.operationControl.TabIndex = 0;
            this.operationControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.operationView,
            this.gridView1});
            // 
            // operationView
            // 
            this.operationView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Operation_Key,
            this.Operation_Name,
            this.Display_Name,
            this.Remark,
            this.Operation_Code,
            this.operationGroupKey,
            this.editor,
            this.edit_timeZone});
            this.operationView.GridControl = this.operationControl;
            this.operationView.Name = "operationView";
            this.operationView.OptionsView.ShowGroupPanel = false;
            this.operationView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.operationView_RowClick);
            this.operationView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.operationView_CustomDrawRowIndicator);
            // 
            // Operation_Key
            // 
            this.Operation_Key.Caption = "操作id";
            this.Operation_Key.FieldName = "OPERATION_KEY";
            this.Operation_Key.Name = "Operation_Key";
            // 
            // Operation_Name
            // 
            this.Operation_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.Operation_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Operation_Name.Caption = "操作名称";
            this.Operation_Name.FieldName = "OPERATION_NAME";
            this.Operation_Name.Name = "Operation_Name";
            this.Operation_Name.OptionsColumn.AllowEdit = false;
            this.Operation_Name.Visible = true;
            this.Operation_Name.VisibleIndex = 0;
            // 
            // Display_Name
            // 
            this.Display_Name.AppearanceHeader.Options.UseTextOptions = true;
            this.Display_Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Display_Name.Caption = "显示名称";
            this.Display_Name.FieldName = "DISPLAY_NAME";
            this.Display_Name.Name = "Display_Name";
            this.Display_Name.OptionsColumn.AllowEdit = false;
            this.Display_Name.Visible = true;
            this.Display_Name.VisibleIndex = 1;
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
            // Operation_Code
            // 
            this.Operation_Code.AppearanceHeader.Options.UseTextOptions = true;
            this.Operation_Code.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Operation_Code.Caption = "编码";
            this.Operation_Code.FieldName = "OPERATION_CODE";
            this.Operation_Code.Name = "Operation_Code";
            this.Operation_Code.OptionsColumn.AllowEdit = false;
            this.Operation_Code.Visible = true;
            this.Operation_Code.VisibleIndex = 2;
            // 
            // operationGroupKey
            // 
            this.operationGroupKey.Caption = "操作组id";
            this.operationGroupKey.FieldName = "OPERATION_GROUP_KEY";
            this.operationGroupKey.Name = "operationGroupKey";
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
            // gridView1
            // 
            this.gridView1.DetailHeight = 272;
            this.gridView1.GridControl = this.operationControl;
            this.gridView1.Name = "gridView1";
            // 
            // layoutControlOperation
            // 
            this.layoutControlOperation.Controls.Add(this.OperationGroup);
            this.layoutControlOperation.Controls.Add(this.txtCode);
            this.layoutControlOperation.Controls.Add(this.txtOperationName);
            this.layoutControlOperation.Controls.Add(this.txtDisplayName);
            this.layoutControlOperation.Controls.Add(this.txtRemark);
            this.layoutControlOperation.Controls.Add(this.tsbSave);
            this.layoutControlOperation.Controls.Add(this.tsbDelete);
            this.layoutControlOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlOperation.Location = new System.Drawing.Point(0, 0);
            this.layoutControlOperation.Name = "layoutControlOperation";
            this.layoutControlOperation.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(3830, 23, 812, 500);
            this.layoutControlOperation.Root = this.layoutControlGroupOperation;
            this.layoutControlOperation.Size = new System.Drawing.Size(297, 512);
            this.layoutControlOperation.TabIndex = 2;
            this.layoutControlOperation.Text = "layoutControl1";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(75, 347);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(211, 20);
            this.txtCode.StyleController = this.layoutControlOperation;
            this.txtCode.TabIndex = 7;
            // 
            // txtOperationName
            // 
            this.txtOperationName.Location = new System.Drawing.Point(75, 323);
            this.txtOperationName.Name = "txtOperationName";
            this.txtOperationName.Size = new System.Drawing.Size(71, 20);
            this.txtOperationName.StyleController = this.layoutControlOperation;
            this.txtOperationName.TabIndex = 1;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(214, 323);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(72, 20);
            this.txtDisplayName.StyleController = this.layoutControlOperation;
            this.txtDisplayName.TabIndex = 3;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(75, 371);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(211, 105);
            this.txtRemark.StyleController = this.layoutControlOperation;
            this.txtRemark.TabIndex = 5;
            // 
            // tsbSave
            // 
            this.tsbSave.Location = new System.Drawing.Point(11, 480);
            this.tsbSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(135, 22);
            this.tsbSave.StyleController = this.layoutControlOperation;
            this.tsbSave.TabIndex = 9;
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Location = new System.Drawing.Point(150, 480);
            this.tsbDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(136, 22);
            this.tsbDelete.StyleController = this.layoutControlOperation;
            this.tsbDelete.TabIndex = 11;
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // layoutControlGroupOperation
            // 
            this.layoutControlGroupOperation.CustomizationFormText = " ";
            this.layoutControlGroupOperation.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupOperation.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblRemark,
            this.lblCode,
            this.emptySpaceItem2,
            this.emptySpaceItem1,
            this.lblOperationName,
            this.lblDisplayName,
            this.emptySpaceItem4,
            this.emptySpaceItem3,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroupOperation.Name = "Root";
            this.layoutControlGroupOperation.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroupOperation.Size = new System.Drawing.Size(297, 512);
            this.layoutControlGroupOperation.Text = " ";
            // 
            // lblRemark
            // 
            this.lblRemark.Control = this.txtRemark;
            this.lblRemark.CustomizationFormText = "备注：";
            this.lblRemark.Location = new System.Drawing.Point(0, 340);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.OptionsTableLayoutItem.RowIndex = 2;
            this.lblRemark.Size = new System.Drawing.Size(279, 109);
            this.lblRemark.Text = "备注：";
            this.lblRemark.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lblCode
            // 
            this.lblCode.Control = this.txtCode;
            this.lblCode.CustomizationFormText = "编码：";
            this.lblCode.Location = new System.Drawing.Point(0, 316);
            this.lblCode.Name = "lblCode";
            this.lblCode.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lblCode.OptionsTableLayoutItem.RowIndex = 1;
            this.lblCode.Size = new System.Drawing.Size(279, 24);
            this.lblCode.Text = "编码：";
            this.lblCode.TextSize = new System.Drawing.Size(60, 14);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 245);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(70, 47);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(70, 245);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(70, 47);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lblOperationName
            // 
            this.lblOperationName.Control = this.txtOperationName;
            this.lblOperationName.CustomizationFormText = "操作名称：";
            this.lblOperationName.Location = new System.Drawing.Point(0, 292);
            this.lblOperationName.Name = "lblOperationName";
            this.lblOperationName.Size = new System.Drawing.Size(139, 24);
            this.lblOperationName.Text = "操作名称：";
            this.lblOperationName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.Control = this.txtDisplayName;
            this.lblDisplayName.CustomizationFormText = "显示名称：";
            this.lblDisplayName.Location = new System.Drawing.Point(139, 292);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.OptionsTableLayoutItem.RowIndex = 1;
            this.lblDisplayName.Size = new System.Drawing.Size(140, 24);
            this.lblDisplayName.Text = "显示名称：";
            this.lblDisplayName.TextSize = new System.Drawing.Size(60, 14);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(140, 245);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(69, 47);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(209, 245);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(70, 47);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.OperationGroup;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(279, 245);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tsbSave;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 449);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(139, 26);
            this.layoutControlItem2.Text = "保存";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tsbDelete;
            this.layoutControlItem3.Location = new System.Drawing.Point(139, 449);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(140, 26);
            this.layoutControlItem3.Text = "删除";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // tableLayoutPanelRight
            // 
            this.tableLayoutPanelRight.ColumnCount = 1;
            this.tableLayoutPanelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRight.Controls.Add(this.operationControl, 0, 0);
            this.tableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRight.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRight.Name = "tableLayoutPanelRight";
            this.tableLayoutPanelRight.RowCount = 1;
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 513F));
            this.tableLayoutPanelRight.Size = new System.Drawing.Size(607, 512);
            this.tableLayoutPanelRight.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(1, 47);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(914, 516);
            this.panelControl2.TabIndex = 3;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControlOperation);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.tableLayoutPanelRight);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(910, 512);
            this.splitContainerControl1.SplitterPosition = 297;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // OperationCtrl
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.LookAndFeel.SkinName = "Coffee";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OperationCtrl";
            this.Size = new System.Drawing.Size(916, 563);
            this.Load += new System.EventHandler(this.OperationCtrl_Load);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.panelControl2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.topPanel)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGroup)).EndInit();
            this.OperationGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGrpOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operationView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlOperation)).EndInit();
            this.layoutControlOperation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDisplayName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblOperationName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDisplayName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.tableLayoutPanelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl OperationGroup;
        private System.Windows.Forms.TreeView operationTree;
        private System.Windows.Forms.ImageList imageList;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGrpOperation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRight;
        private DevExpress.XtraLayout.LayoutControl layoutControlOperation;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private DevExpress.XtraEditors.TextEdit txtOperationName;
        private DevExpress.XtraEditors.TextEdit txtDisplayName;
        private DevExpress.XtraEditors.MemoEdit txtRemark;
        private DevExpress.XtraEditors.SimpleButton tsbSave;
        private DevExpress.XtraEditors.SimpleButton tsbDelete;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupOperation;
        private DevExpress.XtraLayout.LayoutControlItem lblRemark;
        private DevExpress.XtraLayout.LayoutControlItem lblCode;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem lblOperationName;
        private DevExpress.XtraLayout.LayoutControlItem lblDisplayName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraGrid.GridControl operationControl;
        private DevExpress.XtraGrid.Views.Grid.GridView operationView;
        private DevExpress.XtraGrid.Columns.GridColumn Operation_Key;
        private DevExpress.XtraGrid.Columns.GridColumn Operation_Name;
        private DevExpress.XtraGrid.Columns.GridColumn Display_Name;
        private DevExpress.XtraGrid.Columns.GridColumn Remark;
        private DevExpress.XtraGrid.Columns.GridColumn Operation_Code;
        private DevExpress.XtraGrid.Columns.GridColumn operationGroupKey;
        private DevExpress.XtraGrid.Columns.GridColumn editor;
        private DevExpress.XtraGrid.Columns.GridColumn edit_timeZone;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}

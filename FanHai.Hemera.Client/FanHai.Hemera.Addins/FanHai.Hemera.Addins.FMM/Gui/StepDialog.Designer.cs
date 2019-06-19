namespace FanHai.Hemera.Addins.FMM
{
    partial class StepDialog
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lcContent = new DevExpress.XtraLayout.LayoutControl();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.tcStep = new DevExpress.XtraTab.XtraTabControl();
            this.xptBaseInfo = new DevExpress.XtraTab.XtraTabPage();
            this.lcBaseInfo = new DevExpress.XtraLayout.LayoutControl();
            this.beDefectReasonCode = new DevExpress.XtraEditors.ButtonEdit();
            this.beScrapReasonCode = new DevExpress.XtraEditors.ButtonEdit();
            this.txtDuration = new DevExpress.XtraEditors.SpinEdit();
            this.txtStepName = new DevExpress.XtraEditors.TextEdit();
            this.txtOperationVersion = new DevExpress.XtraEditors.TextEdit();
            this.mmDescription = new DevExpress.XtraEditors.MemoEdit();
            this.lcgBaseInfoRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblStepName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDuration = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciScrapReasonCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDefectReasonCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblVersion = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiDuration = new DevExpress.XtraLayout.EmptySpaceItem();
            this.xtpParam = new DevExpress.XtraTab.XtraTabPage();
            this.lcOperationParam = new DevExpress.XtraLayout.LayoutControl();
            this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
            this.btnMoveDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnMoveUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.gcParams = new DevExpress.XtraGrid.GridControl();
            this.gvParams = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gclRowNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclParamName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueDataType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gclIsMustInput = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riChkIsMustInput = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gclDataFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueDataFrom = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gclDCType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueDCType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gclReadOnly = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riChkIsReadOnly = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gclIsCompletePreValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riChkIsCompletePreValue = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gcFeeding = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gclValidateRule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueValidateRule = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gclValidateFailedRule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilueValidateFailedRule = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gclValidateFailedMessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gclValidateBomRule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tedValidateBomValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gcCalculateRule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riRadioRules = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gcLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ritextLength = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.rilueValidateBomValue = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.riRadioRule = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.teParamCountPerCount = new DevExpress.XtraEditors.TextEdit();
            this.rgOrderType = new DevExpress.XtraEditors.RadioGroup();
            this.lcgOpeartionParamRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgOpeartionParamBase = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciOrderType = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiAfter = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciParamCountPerRow = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgOperationParamCommands = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciBtnAdd = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnRemove = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnMoveUp = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnMoveDown = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiCommand = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lcgOperationParamParams = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciParams = new DevExpress.XtraLayout.LayoutControlItem();
            this.xtpAttribute = new DevExpress.XtraTab.XtraTabPage();
            this.gcStepAtrribute = new DevExpress.XtraEditors.GroupControl();
            this.panelUda = new System.Windows.Forms.Panel();
            this.lcgRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcgInfo = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciInfo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcgButtons = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciConfirm = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.esiPrefixButtons = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).BeginInit();
            this.lcContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcStep)).BeginInit();
            this.tcStep.SuspendLayout();
            this.xptBaseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcBaseInfo)).BeginInit();
            this.lcBaseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beDefectReasonCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beScrapReasonCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDuration.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStepName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgBaseInfoRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStepName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciScrapReasonCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDefectReasonCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiDuration)).BeginInit();
            this.xtpParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcOperationParam)).BeginInit();
            this.lcOperationParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsMustInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDCType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsReadOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsCompletePreValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateFailedRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedValidateBomValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRadioRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ritextLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateBomValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRadioRule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teParamCountPerCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgOrderType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOpeartionParamRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOpeartionParamBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciParamCountPerRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOperationParamCommands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnMoveUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnMoveDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOperationParamParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciParams)).BeginInit();
            this.xtpAttribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcStepAtrribute)).BeginInit();
            this.gcStepAtrribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(791, 563);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 35);
            this.btnCancel.StyleController = this.lcContent;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lcContent
            // 
            this.lcContent.AllowCustomization = false;
            this.lcContent.Controls.Add(this.btnCancel);
            this.lcContent.Controls.Add(this.btnConfirm);
            this.lcContent.Controls.Add(this.tcStep);
            this.lcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcContent.Location = new System.Drawing.Point(0, 0);
            this.lcContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcContent.Name = "lcContent";
            this.lcContent.Root = this.lcgRoot;
            this.lcContent.Size = new System.Drawing.Size(885, 606);
            this.lcContent.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(700, 563);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(87, 35);
            this.btnConfirm.StyleController = this.lcContent;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // tcStep
            // 
            this.tcStep.Location = new System.Drawing.Point(7, 8);
            this.tcStep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tcStep.Name = "tcStep";
            this.tcStep.SelectedTabPage = this.xptBaseInfo;
            this.tcStep.Size = new System.Drawing.Size(871, 539);
            this.tcStep.TabIndex = 8;
            this.tcStep.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xptBaseInfo,
            this.xtpParam,
            this.xtpAttribute});
            // 
            // xptBaseInfo
            // 
            this.xptBaseInfo.Controls.Add(this.lcBaseInfo);
            this.xptBaseInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xptBaseInfo.Name = "xptBaseInfo";
            this.xptBaseInfo.Size = new System.Drawing.Size(867, 509);
            this.xptBaseInfo.Text = "基本信息";
            // 
            // lcBaseInfo
            // 
            this.lcBaseInfo.Controls.Add(this.beDefectReasonCode);
            this.lcBaseInfo.Controls.Add(this.beScrapReasonCode);
            this.lcBaseInfo.Controls.Add(this.txtDuration);
            this.lcBaseInfo.Controls.Add(this.txtStepName);
            this.lcBaseInfo.Controls.Add(this.txtOperationVersion);
            this.lcBaseInfo.Controls.Add(this.mmDescription);
            this.lcBaseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcBaseInfo.Location = new System.Drawing.Point(0, 0);
            this.lcBaseInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcBaseInfo.Name = "lcBaseInfo";
            this.lcBaseInfo.Root = this.lcgBaseInfoRoot;
            this.lcBaseInfo.Size = new System.Drawing.Size(867, 509);
            this.lcBaseInfo.TabIndex = 90;
            this.lcBaseInfo.Text = "layoutControl1";
            // 
            // beDefectReasonCode
            // 
            this.beDefectReasonCode.Location = new System.Drawing.Point(513, 89);
            this.beDefectReasonCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.beDefectReasonCode.Name = "beDefectReasonCode";
            this.beDefectReasonCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDefectReasonCode.Properties.ReadOnly = true;
            this.beDefectReasonCode.Size = new System.Drawing.Size(348, 24);
            this.beDefectReasonCode.StyleController = this.lcBaseInfo;
            this.beDefectReasonCode.TabIndex = 91;
            this.beDefectReasonCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beReasonCode_ButtonClick);
            // 
            // beScrapReasonCode
            // 
            this.beScrapReasonCode.Location = new System.Drawing.Point(84, 89);
            this.beScrapReasonCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.beScrapReasonCode.Name = "beScrapReasonCode";
            this.beScrapReasonCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beScrapReasonCode.Properties.ReadOnly = true;
            this.beScrapReasonCode.Size = new System.Drawing.Size(347, 24);
            this.beScrapReasonCode.StyleController = this.lcBaseInfo;
            this.beScrapReasonCode.TabIndex = 91;
            this.beScrapReasonCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beReasonCode_ButtonClick);
            // 
            // txtDuration
            // 
            this.txtDuration.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDuration.Location = new System.Drawing.Point(84, 61);
            this.txtDuration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDuration.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.txtDuration.Size = new System.Drawing.Size(347, 24);
            this.txtDuration.StyleController = this.lcBaseInfo;
            this.txtDuration.TabIndex = 2;
            // 
            // txtStepName
            // 
            this.txtStepName.Location = new System.Drawing.Point(84, 33);
            this.txtStepName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStepName.Name = "txtStepName";
            this.txtStepName.Properties.ReadOnly = true;
            this.txtStepName.Size = new System.Drawing.Size(347, 24);
            this.txtStepName.StyleController = this.lcBaseInfo;
            this.txtStepName.TabIndex = 0;
            // 
            // txtOperationVersion
            // 
            this.txtOperationVersion.Location = new System.Drawing.Point(513, 33);
            this.txtOperationVersion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOperationVersion.Name = "txtOperationVersion";
            this.txtOperationVersion.Properties.ReadOnly = true;
            this.txtOperationVersion.Size = new System.Drawing.Size(348, 24);
            this.txtOperationVersion.StyleController = this.lcBaseInfo;
            this.txtOperationVersion.TabIndex = 1;
            // 
            // mmDescription
            // 
            this.mmDescription.Location = new System.Drawing.Point(84, 117);
            this.mmDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mmDescription.Name = "mmDescription";
            this.mmDescription.Size = new System.Drawing.Size(777, 99);
            this.mmDescription.StyleController = this.lcBaseInfo;
            this.mmDescription.TabIndex = 4;
            // 
            // lcgBaseInfoRoot
            // 
            this.lcgBaseInfoRoot.CustomizationFormText = " ";
            this.lcgBaseInfoRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgBaseInfoRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblStepName,
            this.lblDescription,
            this.lblDuration,
            this.lciScrapReasonCode,
            this.lciDefectReasonCode,
            this.lblVersion,
            this.esiDuration});
            this.lcgBaseInfoRoot.Name = "lcgBaseInfoRoot";
            this.lcgBaseInfoRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 4, 4);
            this.lcgBaseInfoRoot.Size = new System.Drawing.Size(867, 509);
            this.lcgBaseInfoRoot.Text = " ";
            // 
            // lblStepName
            // 
            this.lblStepName.Control = this.txtStepName;
            this.lblStepName.CustomizationFormText = "名称";
            this.lblStepName.Location = new System.Drawing.Point(0, 0);
            this.lblStepName.Name = "lblStepName";
            this.lblStepName.Size = new System.Drawing.Size(429, 28);
            this.lblStepName.Text = "名称";
            this.lblStepName.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lblDescription
            // 
            this.lblDescription.Control = this.mmDescription;
            this.lblDescription.CustomizationFormText = "描述";
            this.lblDescription.Location = new System.Drawing.Point(0, 84);
            this.lblDescription.MaxSize = new System.Drawing.Size(0, 103);
            this.lblDescription.MinSize = new System.Drawing.Size(89, 103);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(859, 389);
            this.lblDescription.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblDescription.Text = "描述";
            this.lblDescription.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lblDuration
            // 
            this.lblDuration.Control = this.txtDuration;
            this.lblDuration.CustomizationFormText = "时长";
            this.lblDuration.Location = new System.Drawing.Point(0, 28);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(429, 28);
            this.lblDuration.Text = "时长";
            this.lblDuration.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lciScrapReasonCode
            // 
            this.lciScrapReasonCode.Control = this.beScrapReasonCode;
            this.lciScrapReasonCode.CustomizationFormText = "报废代码组";
            this.lciScrapReasonCode.Location = new System.Drawing.Point(0, 56);
            this.lciScrapReasonCode.Name = "lciScrapReasonCode";
            this.lciScrapReasonCode.Size = new System.Drawing.Size(429, 28);
            this.lciScrapReasonCode.Text = "报废代码组";
            this.lciScrapReasonCode.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lciDefectReasonCode
            // 
            this.lciDefectReasonCode.Control = this.beDefectReasonCode;
            this.lciDefectReasonCode.CustomizationFormText = "不良代码组";
            this.lciDefectReasonCode.Location = new System.Drawing.Point(429, 56);
            this.lciDefectReasonCode.Name = "lciDefectReasonCode";
            this.lciDefectReasonCode.Size = new System.Drawing.Size(430, 28);
            this.lciDefectReasonCode.Text = "不良代码组";
            this.lciDefectReasonCode.TextSize = new System.Drawing.Size(75, 18);
            // 
            // lblVersion
            // 
            this.lblVersion.Control = this.txtOperationVersion;
            this.lblVersion.CustomizationFormText = "版本";
            this.lblVersion.Location = new System.Drawing.Point(429, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(430, 28);
            this.lblVersion.Text = "版本";
            this.lblVersion.TextSize = new System.Drawing.Size(75, 18);
            // 
            // esiDuration
            // 
            this.esiDuration.AllowHotTrack = false;
            this.esiDuration.CustomizationFormText = "esiDuration";
            this.esiDuration.Location = new System.Drawing.Point(429, 28);
            this.esiDuration.Name = "esiDuration";
            this.esiDuration.Size = new System.Drawing.Size(430, 28);
            this.esiDuration.TextSize = new System.Drawing.Size(0, 0);
            // 
            // xtpParam
            // 
            this.xtpParam.Controls.Add(this.lcOperationParam);
            this.xtpParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtpParam.Name = "xtpParam";
            this.xtpParam.Size = new System.Drawing.Size(867, 509);
            this.xtpParam.Text = "工步参数";
            // 
            // lcOperationParam
            // 
            this.lcOperationParam.AllowCustomization = false;
            this.lcOperationParam.Controls.Add(this.btnRemove);
            this.lcOperationParam.Controls.Add(this.btnMoveDown);
            this.lcOperationParam.Controls.Add(this.btnMoveUp);
            this.lcOperationParam.Controls.Add(this.btnAdd);
            this.lcOperationParam.Controls.Add(this.gcParams);
            this.lcOperationParam.Controls.Add(this.teParamCountPerCount);
            this.lcOperationParam.Controls.Add(this.rgOrderType);
            this.lcOperationParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcOperationParam.Location = new System.Drawing.Point(0, 0);
            this.lcOperationParam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lcOperationParam.Name = "lcOperationParam";
            this.lcOperationParam.Root = this.lcgOpeartionParamRoot;
            this.lcOperationParam.Size = new System.Drawing.Size(867, 509);
            this.lcOperationParam.TabIndex = 1;
            this.lcOperationParam.Text = "layoutControl1";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(87, 51);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(76, 35);
            this.btnRemove.StyleController = this.lcOperationParam;
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "移除";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(247, 51);
            this.btnMoveDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(76, 35);
            this.btnMoveDown.StyleController = this.lcOperationParam;
            this.btnMoveDown.TabIndex = 8;
            this.btnMoveDown.Text = "下移";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(167, 51);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(76, 35);
            this.btnMoveUp.StyleController = this.lcOperationParam;
            this.btnMoveUp.TabIndex = 7;
            this.btnMoveUp.Text = "上移";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(7, 51);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(76, 35);
            this.btnAdd.StyleController = this.lcOperationParam;
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gcParams
            // 
            this.gcParams.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcParams.Location = new System.Drawing.Point(7, 96);
            this.gcParams.MainView = this.gvParams;
            this.gcParams.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcParams.Name = "gcParams";
            this.gcParams.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rilueDataType,
            this.riChkIsMustInput,
            this.rilueDataFrom,
            this.rilueDCType,
            this.riChkIsReadOnly,
            this.rilueValidateRule,
            this.rilueValidateFailedRule,
            this.riChkIsCompletePreValue,
            this.rilueValidateBomValue,
            this.tedValidateBomValue,
            this.riRadioRule,
            this.riRadioRules,
            this.ritextLength,
            this.repositoryItemCheckEdit1});
            this.gcParams.Size = new System.Drawing.Size(853, 405);
            this.gcParams.TabIndex = 10;
            this.gcParams.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvParams});
            // 
            // gvParams
            // 
            this.gvParams.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gclRowNum,
            this.gclParamName,
            this.gclDataType,
            this.gclIsMustInput,
            this.gclDataFrom,
            this.gclDCType,
            this.gclReadOnly,
            this.gclIsCompletePreValue,
            this.gcFeeding,
            this.gclValidateRule,
            this.gclValidateFailedRule,
            this.gclValidateFailedMessage,
            this.gclValidateBomRule,
            this.gcCalculateRule,
            this.gcLength});
            this.gvParams.DetailHeight = 450;
            this.gvParams.FixedLineWidth = 3;
            this.gvParams.GridControl = this.gcParams;
            this.gvParams.Name = "gvParams";
            this.gvParams.OptionsCustomization.AllowFilter = false;
            this.gvParams.OptionsCustomization.AllowGroup = false;
            this.gvParams.OptionsCustomization.AllowSort = false;
            this.gvParams.OptionsMenu.EnableColumnMenu = false;
            this.gvParams.OptionsMenu.EnableFooterMenu = false;
            this.gvParams.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvParams.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvParams.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvParams.OptionsView.ColumnAutoWidth = false;
            this.gvParams.OptionsView.ShowGroupPanel = false;
            // 
            // gclRowNum
            // 
            this.gclRowNum.Caption = "序号";
            this.gclRowNum.FieldName = "PARAM_INDEX";
            this.gclRowNum.MinWidth = 57;
            this.gclRowNum.Name = "gclRowNum";
            this.gclRowNum.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gclRowNum.OptionsColumn.FixedWidth = true;
            this.gclRowNum.OptionsColumn.ReadOnly = true;
            this.gclRowNum.Visible = true;
            this.gclRowNum.VisibleIndex = 0;
            this.gclRowNum.Width = 57;
            // 
            // gclParamName
            // 
            this.gclParamName.Caption = "参数名称";
            this.gclParamName.FieldName = "PARAM_NAME";
            this.gclParamName.MinWidth = 80;
            this.gclParamName.Name = "gclParamName";
            this.gclParamName.OptionsColumn.ReadOnly = true;
            this.gclParamName.Visible = true;
            this.gclParamName.VisibleIndex = 1;
            this.gclParamName.Width = 80;
            // 
            // gclDataType
            // 
            this.gclDataType.Caption = "数据类型";
            this.gclDataType.ColumnEdit = this.rilueDataType;
            this.gclDataType.FieldName = "DATA_TYPE";
            this.gclDataType.MinWidth = 103;
            this.gclDataType.Name = "gclDataType";
            this.gclDataType.OptionsColumn.FixedWidth = true;
            this.gclDataType.OptionsColumn.ReadOnly = true;
            this.gclDataType.Visible = true;
            this.gclDataType.VisibleIndex = 2;
            this.gclDataType.Width = 103;
            // 
            // rilueDataType
            // 
            this.rilueDataType.AutoHeight = false;
            this.rilueDataType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueDataType.Name = "rilueDataType";
            this.rilueDataType.NullText = "";
            // 
            // gclIsMustInput
            // 
            this.gclIsMustInput.Caption = "必填";
            this.gclIsMustInput.ColumnEdit = this.riChkIsMustInput;
            this.gclIsMustInput.FieldName = "IS_MUSTINPUT";
            this.gclIsMustInput.MinWidth = 57;
            this.gclIsMustInput.Name = "gclIsMustInput";
            this.gclIsMustInput.OptionsColumn.FixedWidth = true;
            this.gclIsMustInput.Visible = true;
            this.gclIsMustInput.VisibleIndex = 3;
            this.gclIsMustInput.Width = 57;
            // 
            // riChkIsMustInput
            // 
            this.riChkIsMustInput.AutoHeight = false;
            this.riChkIsMustInput.Name = "riChkIsMustInput";
            this.riChkIsMustInput.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.riChkIsMustInput.ValueChecked = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.riChkIsMustInput.ValueUnchecked = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // gclDataFrom
            // 
            this.gclDataFrom.Caption = "数据来源";
            this.gclDataFrom.ColumnEdit = this.rilueDataFrom;
            this.gclDataFrom.FieldName = "DATA_FROM";
            this.gclDataFrom.MinWidth = 103;
            this.gclDataFrom.Name = "gclDataFrom";
            this.gclDataFrom.OptionsColumn.FixedWidth = true;
            this.gclDataFrom.Visible = true;
            this.gclDataFrom.VisibleIndex = 4;
            this.gclDataFrom.Width = 103;
            // 
            // rilueDataFrom
            // 
            this.rilueDataFrom.AutoHeight = false;
            this.rilueDataFrom.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueDataFrom.Name = "rilueDataFrom";
            this.rilueDataFrom.NullText = "";
            // 
            // gclDCType
            // 
            this.gclDCType.Caption = "采集类型";
            this.gclDCType.ColumnEdit = this.rilueDCType;
            this.gclDCType.FieldName = "DC_TYPE";
            this.gclDCType.MinWidth = 114;
            this.gclDCType.Name = "gclDCType";
            this.gclDCType.OptionsColumn.FixedWidth = true;
            this.gclDCType.Visible = true;
            this.gclDCType.VisibleIndex = 5;
            this.gclDCType.Width = 114;
            // 
            // rilueDCType
            // 
            this.rilueDCType.AutoHeight = false;
            this.rilueDCType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueDCType.Name = "rilueDCType";
            this.rilueDCType.NullText = "";
            // 
            // gclReadOnly
            // 
            this.gclReadOnly.Caption = "只读";
            this.gclReadOnly.ColumnEdit = this.riChkIsReadOnly;
            this.gclReadOnly.FieldName = "IS_READONLY";
            this.gclReadOnly.MinWidth = 57;
            this.gclReadOnly.Name = "gclReadOnly";
            this.gclReadOnly.OptionsColumn.FixedWidth = true;
            this.gclReadOnly.Visible = true;
            this.gclReadOnly.VisibleIndex = 6;
            this.gclReadOnly.Width = 57;
            // 
            // riChkIsReadOnly
            // 
            this.riChkIsReadOnly.AutoHeight = false;
            this.riChkIsReadOnly.Name = "riChkIsReadOnly";
            this.riChkIsReadOnly.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.riChkIsReadOnly.ValueChecked = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.riChkIsReadOnly.ValueUnchecked = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // gclIsCompletePreValue
            // 
            this.gclIsCompletePreValue.Caption = "自动填值";
            this.gclIsCompletePreValue.ColumnEdit = this.riChkIsCompletePreValue;
            this.gclIsCompletePreValue.FieldName = "IS_COMPLETE_PREVALUE";
            this.gclIsCompletePreValue.MinWidth = 23;
            this.gclIsCompletePreValue.Name = "gclIsCompletePreValue";
            this.gclIsCompletePreValue.OptionsColumn.FixedWidth = true;
            this.gclIsCompletePreValue.Visible = true;
            this.gclIsCompletePreValue.VisibleIndex = 7;
            this.gclIsCompletePreValue.Width = 80;
            // 
            // riChkIsCompletePreValue
            // 
            this.riChkIsCompletePreValue.AutoHeight = false;
            this.riChkIsCompletePreValue.Name = "riChkIsCompletePreValue";
            this.riChkIsCompletePreValue.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.riChkIsCompletePreValue.ValueChecked = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.riChkIsCompletePreValue.ValueUnchecked = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // gcFeeding
            // 
            this.gcFeeding.Caption = "自动上料";
            this.gcFeeding.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcFeeding.FieldName = "IS_FEEDING";
            this.gcFeeding.MinWidth = 23;
            this.gcFeeding.Name = "gcFeeding";
            this.gcFeeding.Visible = true;
            this.gcFeeding.VisibleIndex = 8;
            this.gcFeeding.Width = 86;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.ValueChecked = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemCheckEdit1.ValueUnchecked = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // gclValidateRule
            // 
            this.gclValidateRule.Caption = "验证工单属性物料控制规则";
            this.gclValidateRule.ColumnEdit = this.rilueValidateRule;
            this.gclValidateRule.FieldName = "VALIDATE_RULE";
            this.gclValidateRule.MinWidth = 229;
            this.gclValidateRule.Name = "gclValidateRule";
            this.gclValidateRule.OptionsColumn.FixedWidth = true;
            this.gclValidateRule.Visible = true;
            this.gclValidateRule.VisibleIndex = 9;
            this.gclValidateRule.Width = 229;
            // 
            // rilueValidateRule
            // 
            this.rilueValidateRule.AutoHeight = false;
            this.rilueValidateRule.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueValidateRule.Name = "rilueValidateRule";
            this.rilueValidateRule.NullText = "";
            // 
            // gclValidateFailedRule
            // 
            this.gclValidateFailedRule.Caption = "验证失败规则";
            this.gclValidateFailedRule.ColumnEdit = this.rilueValidateFailedRule;
            this.gclValidateFailedRule.FieldName = "VALIDATE_FAILED_RULE";
            this.gclValidateFailedRule.MinWidth = 103;
            this.gclValidateFailedRule.Name = "gclValidateFailedRule";
            this.gclValidateFailedRule.Visible = true;
            this.gclValidateFailedRule.VisibleIndex = 10;
            this.gclValidateFailedRule.Width = 103;
            // 
            // rilueValidateFailedRule
            // 
            this.rilueValidateFailedRule.AutoHeight = false;
            this.rilueValidateFailedRule.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueValidateFailedRule.Name = "rilueValidateFailedRule";
            this.rilueValidateFailedRule.NullText = "";
            // 
            // gclValidateFailedMessage
            // 
            this.gclValidateFailedMessage.Caption = "验证失败提示信息";
            this.gclValidateFailedMessage.FieldName = "VALIDATE_FAILED_MSG";
            this.gclValidateFailedMessage.MinWidth = 171;
            this.gclValidateFailedMessage.Name = "gclValidateFailedMessage";
            this.gclValidateFailedMessage.Visible = true;
            this.gclValidateFailedMessage.VisibleIndex = 11;
            this.gclValidateFailedMessage.Width = 171;
            // 
            // gclValidateBomRule
            // 
            this.gclValidateBomRule.Caption = "Bom物料类型";
            this.gclValidateBomRule.ColumnEdit = this.tedValidateBomValue;
            this.gclValidateBomRule.FieldName = "VALIDATE_MAT_RULE";
            this.gclValidateBomRule.MinWidth = 23;
            this.gclValidateBomRule.Name = "gclValidateBomRule";
            this.gclValidateBomRule.Visible = true;
            this.gclValidateBomRule.VisibleIndex = 12;
            this.gclValidateBomRule.Width = 86;
            // 
            // tedValidateBomValue
            // 
            this.tedValidateBomValue.AutoHeight = false;
            this.tedValidateBomValue.Name = "tedValidateBomValue";
            // 
            // gcCalculateRule
            // 
            this.gcCalculateRule.Caption = "计算规则";
            this.gcCalculateRule.ColumnEdit = this.riRadioRules;
            this.gcCalculateRule.FieldName = "CALCULATERULE";
            this.gcCalculateRule.MinWidth = 23;
            this.gcCalculateRule.Name = "gcCalculateRule";
            this.gcCalculateRule.OptionsColumn.FixedWidth = true;
            this.gcCalculateRule.Visible = true;
            this.gcCalculateRule.VisibleIndex = 13;
            this.gcCalculateRule.Width = 86;
            // 
            // riRadioRules
            // 
            this.riRadioRules.AutoHeight = false;
            this.riRadioRules.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riRadioRules.Name = "riRadioRules";
            this.riRadioRules.NullText = "";
            // 
            // gcLength
            // 
            this.gcLength.Caption = "管控长度";
            this.gcLength.ColumnEdit = this.ritextLength;
            this.gcLength.FieldName = "FIELDLENGTH";
            this.gcLength.MinWidth = 23;
            this.gcLength.Name = "gcLength";
            this.gcLength.Visible = true;
            this.gcLength.VisibleIndex = 14;
            this.gcLength.Width = 86;
            // 
            // ritextLength
            // 
            this.ritextLength.AutoHeight = false;
            this.ritextLength.Mask.EditMask = "###0";
            this.ritextLength.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.ritextLength.Mask.UseMaskAsDisplayFormat = true;
            this.ritextLength.Name = "ritextLength";
            // 
            // rilueValidateBomValue
            // 
            this.rilueValidateBomValue.AutoHeight = false;
            this.rilueValidateBomValue.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilueValidateBomValue.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DESCRIPTION", "描述")});
            this.rilueValidateBomValue.Name = "rilueValidateBomValue";
            this.rilueValidateBomValue.NullText = "";
            // 
            // riRadioRule
            // 
            this.riRadioRule.Name = "riRadioRule";
            // 
            // teParamCountPerCount
            // 
            this.teParamCountPerCount.EditValue = "2";
            this.teParamCountPerCount.Location = new System.Drawing.Point(542, 8);
            this.teParamCountPerCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.teParamCountPerCount.Name = "teParamCountPerCount";
            this.teParamCountPerCount.Properties.Mask.EditMask = "\\d{0,1}";
            this.teParamCountPerCount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
            this.teParamCountPerCount.Properties.Mask.SaveLiteral = false;
            this.teParamCountPerCount.Properties.Mask.ShowPlaceHolders = false;
            this.teParamCountPerCount.Size = new System.Drawing.Size(119, 24);
            this.teParamCountPerCount.StyleController = this.lcOperationParam;
            this.teParamCountPerCount.TabIndex = 5;
            // 
            // rgOrderType
            // 
            this.rgOrderType.EditValue = "1";
            this.rgOrderType.Location = new System.Drawing.Point(7, 8);
            this.rgOrderType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rgOrderType.Name = "rgOrderType";
            this.rgOrderType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rgOrderType.Properties.Appearance.Options.UseBackColor = true;
            this.rgOrderType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rgOrderType.Properties.Columns = 2;
            this.rgOrderType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "先行后列顺序排列参数"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "先列后行顺序排列参数")});
            this.rgOrderType.Size = new System.Drawing.Size(438, 33);
            this.rgOrderType.StyleController = this.lcOperationParam;
            this.rgOrderType.TabIndex = 4;
            // 
            // lcgOpeartionParamRoot
            // 
            this.lcgOpeartionParamRoot.CustomizationFormText = "Root";
            this.lcgOpeartionParamRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgOpeartionParamRoot.GroupBordersVisible = false;
            this.lcgOpeartionParamRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgOpeartionParamBase,
            this.lcgOperationParamCommands,
            this.lcgOperationParamParams});
            this.lcgOpeartionParamRoot.Name = "Root";
            this.lcgOpeartionParamRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgOpeartionParamRoot.Size = new System.Drawing.Size(867, 509);
            this.lcgOpeartionParamRoot.TextVisible = false;
            // 
            // lcgOpeartionParamBase
            // 
            this.lcgOpeartionParamBase.CustomizationFormText = "基础";
            this.lcgOpeartionParamBase.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciOrderType,
            this.esiAfter,
            this.lciParamCountPerRow});
            this.lcgOpeartionParamBase.Location = new System.Drawing.Point(0, 0);
            this.lcgOpeartionParamBase.Name = "lcgOpeartionParamBase";
            this.lcgOpeartionParamBase.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgOpeartionParamBase.Size = new System.Drawing.Size(863, 43);
            this.lcgOpeartionParamBase.Text = "基础";
            this.lcgOpeartionParamBase.TextVisible = false;
            // 
            // lciOrderType
            // 
            this.lciOrderType.Control = this.rgOrderType;
            this.lciOrderType.CustomizationFormText = "排列方式";
            this.lciOrderType.Location = new System.Drawing.Point(0, 0);
            this.lciOrderType.MaxSize = new System.Drawing.Size(442, 37);
            this.lciOrderType.MinSize = new System.Drawing.Size(442, 37);
            this.lciOrderType.Name = "lciOrderType";
            this.lciOrderType.Size = new System.Drawing.Size(442, 37);
            this.lciOrderType.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciOrderType.Text = "排列方式";
            this.lciOrderType.TextSize = new System.Drawing.Size(0, 0);
            this.lciOrderType.TextVisible = false;
            // 
            // esiAfter
            // 
            this.esiAfter.AllowHotTrack = false;
            this.esiAfter.CustomizationFormText = "After";
            this.esiAfter.Location = new System.Drawing.Point(658, 0);
            this.esiAfter.Name = "esiAfter";
            this.esiAfter.Size = new System.Drawing.Size(199, 37);
            this.esiAfter.Text = "After";
            this.esiAfter.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lciParamCountPerRow
            // 
            this.lciParamCountPerRow.Control = this.teParamCountPerCount;
            this.lciParamCountPerRow.CustomizationFormText = "每行参数数量";
            this.lciParamCountPerRow.Location = new System.Drawing.Point(442, 0);
            this.lciParamCountPerRow.MaxSize = new System.Drawing.Size(216, 37);
            this.lciParamCountPerRow.MinSize = new System.Drawing.Size(216, 37);
            this.lciParamCountPerRow.Name = "lciParamCountPerRow";
            this.lciParamCountPerRow.Size = new System.Drawing.Size(216, 37);
            this.lciParamCountPerRow.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciParamCountPerRow.Text = "每行参数数量";
            this.lciParamCountPerRow.TextSize = new System.Drawing.Size(90, 18);
            // 
            // lcgOperationParamCommands
            // 
            this.lcgOperationParamCommands.CustomizationFormText = "按钮";
            this.lcgOperationParamCommands.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciBtnAdd,
            this.lciBtnRemove,
            this.lciBtnMoveUp,
            this.lciBtnMoveDown,
            this.esiCommand});
            this.lcgOperationParamCommands.Location = new System.Drawing.Point(0, 43);
            this.lcgOperationParamCommands.Name = "lcgOperationParamCommands";
            this.lcgOperationParamCommands.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgOperationParamCommands.Size = new System.Drawing.Size(863, 45);
            this.lcgOperationParamCommands.Text = "按钮";
            this.lcgOperationParamCommands.TextVisible = false;
            // 
            // lciBtnAdd
            // 
            this.lciBtnAdd.Control = this.btnAdd;
            this.lciBtnAdd.CustomizationFormText = "添加";
            this.lciBtnAdd.Location = new System.Drawing.Point(0, 0);
            this.lciBtnAdd.MaxSize = new System.Drawing.Size(80, 39);
            this.lciBtnAdd.MinSize = new System.Drawing.Size(80, 39);
            this.lciBtnAdd.Name = "lciBtnAdd";
            this.lciBtnAdd.Size = new System.Drawing.Size(80, 39);
            this.lciBtnAdd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnAdd.Text = "添加";
            this.lciBtnAdd.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnAdd.TextVisible = false;
            // 
            // lciBtnRemove
            // 
            this.lciBtnRemove.Control = this.btnRemove;
            this.lciBtnRemove.CustomizationFormText = "移除";
            this.lciBtnRemove.Location = new System.Drawing.Point(80, 0);
            this.lciBtnRemove.MaxSize = new System.Drawing.Size(80, 39);
            this.lciBtnRemove.MinSize = new System.Drawing.Size(80, 39);
            this.lciBtnRemove.Name = "lciBtnRemove";
            this.lciBtnRemove.Size = new System.Drawing.Size(80, 39);
            this.lciBtnRemove.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnRemove.Text = "移除";
            this.lciBtnRemove.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnRemove.TextVisible = false;
            // 
            // lciBtnMoveUp
            // 
            this.lciBtnMoveUp.Control = this.btnMoveUp;
            this.lciBtnMoveUp.CustomizationFormText = "上移";
            this.lciBtnMoveUp.Location = new System.Drawing.Point(160, 0);
            this.lciBtnMoveUp.MaxSize = new System.Drawing.Size(80, 39);
            this.lciBtnMoveUp.MinSize = new System.Drawing.Size(80, 39);
            this.lciBtnMoveUp.Name = "lciBtnMoveUp";
            this.lciBtnMoveUp.Size = new System.Drawing.Size(80, 39);
            this.lciBtnMoveUp.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnMoveUp.Text = "上移";
            this.lciBtnMoveUp.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnMoveUp.TextVisible = false;
            // 
            // lciBtnMoveDown
            // 
            this.lciBtnMoveDown.Control = this.btnMoveDown;
            this.lciBtnMoveDown.CustomizationFormText = "下移";
            this.lciBtnMoveDown.Location = new System.Drawing.Point(240, 0);
            this.lciBtnMoveDown.MaxSize = new System.Drawing.Size(80, 39);
            this.lciBtnMoveDown.MinSize = new System.Drawing.Size(80, 39);
            this.lciBtnMoveDown.Name = "lciBtnMoveDown";
            this.lciBtnMoveDown.Size = new System.Drawing.Size(80, 39);
            this.lciBtnMoveDown.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciBtnMoveDown.Text = "下移";
            this.lciBtnMoveDown.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnMoveDown.TextVisible = false;
            // 
            // esiCommand
            // 
            this.esiCommand.AllowHotTrack = false;
            this.esiCommand.CustomizationFormText = "esiCommand";
            this.esiCommand.Location = new System.Drawing.Point(320, 0);
            this.esiCommand.MaxSize = new System.Drawing.Size(0, 39);
            this.esiCommand.MinSize = new System.Drawing.Size(11, 39);
            this.esiCommand.Name = "esiCommand";
            this.esiCommand.Size = new System.Drawing.Size(537, 39);
            this.esiCommand.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.esiCommand.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lcgOperationParamParams
            // 
            this.lcgOperationParamParams.CustomizationFormText = "参数";
            this.lcgOperationParamParams.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciParams});
            this.lcgOperationParamParams.Location = new System.Drawing.Point(0, 88);
            this.lcgOperationParamParams.Name = "lcgOperationParamParams";
            this.lcgOperationParamParams.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgOperationParamParams.Size = new System.Drawing.Size(863, 415);
            this.lcgOperationParamParams.Text = "参数";
            this.lcgOperationParamParams.TextVisible = false;
            // 
            // lciParams
            // 
            this.lciParams.Control = this.gcParams;
            this.lciParams.CustomizationFormText = "参数";
            this.lciParams.Location = new System.Drawing.Point(0, 0);
            this.lciParams.Name = "lciParams";
            this.lciParams.Size = new System.Drawing.Size(857, 409);
            this.lciParams.Text = "参数";
            this.lciParams.TextSize = new System.Drawing.Size(0, 0);
            this.lciParams.TextVisible = false;
            // 
            // xtpAttribute
            // 
            this.xtpAttribute.Controls.Add(this.gcStepAtrribute);
            this.xtpAttribute.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtpAttribute.Name = "xtpAttribute";
            this.xtpAttribute.Size = new System.Drawing.Size(867, 509);
            this.xtpAttribute.Text = "自定义属性";
            // 
            // gcStepAtrribute
            // 
            this.gcStepAtrribute.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gcStepAtrribute.Controls.Add(this.panelUda);
            this.gcStepAtrribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcStepAtrribute.Location = new System.Drawing.Point(0, 0);
            this.gcStepAtrribute.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcStepAtrribute.Name = "gcStepAtrribute";
            this.gcStepAtrribute.ShowCaption = false;
            this.gcStepAtrribute.Size = new System.Drawing.Size(867, 509);
            this.gcStepAtrribute.TabIndex = 1;
            // 
            // panelUda
            // 
            this.panelUda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUda.Location = new System.Drawing.Point(0, 0);
            this.panelUda.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelUda.Name = "panelUda";
            this.panelUda.Size = new System.Drawing.Size(867, 509);
            this.panelUda.TabIndex = 1;
            // 
            // lcgRoot
            // 
            this.lcgRoot.CustomizationFormText = "lcgRoot";
            this.lcgRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgRoot.GroupBordersVisible = false;
            this.lcgRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcgInfo,
            this.lcgButtons});
            this.lcgRoot.Name = "lcgRoot";
            this.lcgRoot.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgRoot.Size = new System.Drawing.Size(885, 606);
            this.lcgRoot.TextVisible = false;
            // 
            // lcgInfo
            // 
            this.lcgInfo.CustomizationFormText = "信息";
            this.lcgInfo.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciInfo});
            this.lcgInfo.Location = new System.Drawing.Point(0, 0);
            this.lcgInfo.Name = "lcgInfo";
            this.lcgInfo.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgInfo.Size = new System.Drawing.Size(885, 555);
            this.lcgInfo.Text = "信息";
            this.lcgInfo.TextVisible = false;
            // 
            // lciInfo
            // 
            this.lciInfo.Control = this.tcStep;
            this.lciInfo.CustomizationFormText = "信息";
            this.lciInfo.Location = new System.Drawing.Point(0, 0);
            this.lciInfo.Name = "lciInfo";
            this.lciInfo.Size = new System.Drawing.Size(875, 543);
            this.lciInfo.Text = "信息";
            this.lciInfo.TextSize = new System.Drawing.Size(0, 0);
            this.lciInfo.TextVisible = false;
            // 
            // lcgButtons
            // 
            this.lcgButtons.CustomizationFormText = "按钮";
            this.lcgButtons.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciConfirm,
            this.lciClose,
            this.esiPrefixButtons});
            this.lcgButtons.Location = new System.Drawing.Point(0, 555);
            this.lcgButtons.Name = "lcgButtons";
            this.lcgButtons.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 3);
            this.lcgButtons.Size = new System.Drawing.Size(885, 51);
            this.lcgButtons.Text = "按钮";
            this.lcgButtons.TextVisible = false;
            // 
            // lciConfirm
            // 
            this.lciConfirm.Control = this.btnConfirm;
            this.lciConfirm.CustomizationFormText = "确定";
            this.lciConfirm.Location = new System.Drawing.Point(693, 0);
            this.lciConfirm.MaxSize = new System.Drawing.Size(91, 39);
            this.lciConfirm.MinSize = new System.Drawing.Size(91, 39);
            this.lciConfirm.Name = "lciConfirm";
            this.lciConfirm.Size = new System.Drawing.Size(91, 39);
            this.lciConfirm.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciConfirm.Text = "确定";
            this.lciConfirm.TextSize = new System.Drawing.Size(0, 0);
            this.lciConfirm.TextVisible = false;
            // 
            // lciClose
            // 
            this.lciClose.Control = this.btnCancel;
            this.lciClose.CustomizationFormText = "关闭";
            this.lciClose.Location = new System.Drawing.Point(784, 0);
            this.lciClose.MaxSize = new System.Drawing.Size(91, 39);
            this.lciClose.MinSize = new System.Drawing.Size(91, 39);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(91, 39);
            this.lciClose.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciClose.Text = "关闭";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextVisible = false;
            // 
            // esiPrefixButtons
            // 
            this.esiPrefixButtons.AllowHotTrack = false;
            this.esiPrefixButtons.CustomizationFormText = "esiPrefixButtons";
            this.esiPrefixButtons.Location = new System.Drawing.Point(0, 0);
            this.esiPrefixButtons.Name = "esiPrefixButtons";
            this.esiPrefixButtons.Size = new System.Drawing.Size(693, 39);
            this.esiPrefixButtons.TextSize = new System.Drawing.Size(0, 0);
            // 
            // StepDialog
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 606);
            this.Controls.Add(this.lcContent);
            this.LookAndFeel.SkinName = "Coffee";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "StepDialog";
            this.Load += new System.EventHandler(this.StepDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcContent)).EndInit();
            this.lcContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcStep)).EndInit();
            this.tcStep.ResumeLayout(false);
            this.xptBaseInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcBaseInfo)).EndInit();
            this.lcBaseInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.beDefectReasonCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beScrapReasonCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDuration.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStepName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOperationVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgBaseInfoRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStepName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciScrapReasonCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDefectReasonCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiDuration)).EndInit();
            this.xtpParam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcOperationParam)).EndInit();
            this.lcOperationParam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsMustInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDataFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueDCType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsReadOnly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riChkIsCompletePreValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateFailedRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedValidateBomValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRadioRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ritextLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilueValidateBomValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRadioRule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teParamCountPerCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgOrderType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOpeartionParamRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOpeartionParamBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciParamCountPerRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOperationParamCommands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnMoveUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciBtnMoveDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOperationParamParams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciParams)).EndInit();
            this.xtpAttribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcStepAtrribute)).EndInit();
            this.gcStepAtrribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lcgRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.esiPrefixButtons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraTab.XtraTabControl tcStep;
        private DevExpress.XtraTab.XtraTabPage xptBaseInfo;
        private DevExpress.XtraTab.XtraTabPage xtpAttribute;
        private DevExpress.XtraEditors.GroupControl gcStepAtrribute;
        private System.Windows.Forms.Panel panelUda;
        private DevExpress.XtraEditors.SpinEdit txtDuration;
        private DevExpress.XtraEditors.MemoEdit mmDescription;
        private DevExpress.XtraEditors.TextEdit txtOperationVersion;
        private DevExpress.XtraEditors.TextEdit txtStepName;
        private DevExpress.XtraLayout.LayoutControl lcBaseInfo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgBaseInfoRoot;
        private DevExpress.XtraLayout.LayoutControlItem lblStepName;
        private DevExpress.XtraLayout.LayoutControlItem lblVersion;
        private DevExpress.XtraLayout.LayoutControlItem lblDuration;
        private DevExpress.XtraLayout.LayoutControlItem lblDescription;
        private DevExpress.XtraEditors.ButtonEdit beScrapReasonCode;
        private DevExpress.XtraLayout.LayoutControlItem lciScrapReasonCode;
        private DevExpress.XtraEditors.ButtonEdit beDefectReasonCode;
        private DevExpress.XtraLayout.LayoutControlItem lciDefectReasonCode;
        private DevExpress.XtraLayout.EmptySpaceItem esiDuration;
        private DevExpress.XtraTab.XtraTabPage xtpParam;
        private DevExpress.XtraLayout.LayoutControl lcOperationParam;
        private DevExpress.XtraGrid.GridControl gcParams;
        private DevExpress.XtraGrid.Views.Grid.GridView gvParams;
        private DevExpress.XtraGrid.Columns.GridColumn gclRowNum;
        private DevExpress.XtraGrid.Columns.GridColumn gclParamName;
        private DevExpress.XtraGrid.Columns.GridColumn gclDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueDataType;
        private DevExpress.XtraGrid.Columns.GridColumn gclIsMustInput;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riChkIsMustInput;
        private DevExpress.XtraGrid.Columns.GridColumn gclDataFrom;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueDataFrom;
        private DevExpress.XtraGrid.Columns.GridColumn gclDCType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueDCType;
        private DevExpress.XtraGrid.Columns.GridColumn gclReadOnly;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riChkIsReadOnly;
        private DevExpress.XtraGrid.Columns.GridColumn gclValidateRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueValidateRule;
        private DevExpress.XtraGrid.Columns.GridColumn gclValidateFailedRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueValidateFailedRule;
        private DevExpress.XtraGrid.Columns.GridColumn gclValidateFailedMessage;
        private DevExpress.XtraEditors.SimpleButton btnRemove;
        private DevExpress.XtraEditors.SimpleButton btnMoveDown;
        private DevExpress.XtraEditors.SimpleButton btnMoveUp;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.TextEdit teParamCountPerCount;
        private DevExpress.XtraEditors.RadioGroup rgOrderType;
        private DevExpress.XtraLayout.LayoutControlGroup lcgOpeartionParamRoot;
        private DevExpress.XtraLayout.LayoutControlGroup lcgOpeartionParamBase;
        private DevExpress.XtraLayout.LayoutControlItem lciOrderType;
        private DevExpress.XtraLayout.EmptySpaceItem esiAfter;
        private DevExpress.XtraLayout.LayoutControlItem lciParamCountPerRow;
        private DevExpress.XtraLayout.LayoutControlGroup lcgOperationParamCommands;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnAdd;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnRemove;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnMoveUp;
        private DevExpress.XtraLayout.LayoutControlItem lciBtnMoveDown;
        private DevExpress.XtraLayout.EmptySpaceItem esiCommand;
        private DevExpress.XtraLayout.LayoutControlGroup lcgOperationParamParams;
        private DevExpress.XtraLayout.LayoutControlItem lciParams;
        private DevExpress.XtraLayout.LayoutControl lcContent;
        private DevExpress.XtraLayout.LayoutControlGroup lcgRoot;
        private DevExpress.XtraLayout.LayoutControlItem lciInfo;
        private DevExpress.XtraLayout.LayoutControlItem lciConfirm;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraLayout.LayoutControlGroup lcgInfo;
        private DevExpress.XtraLayout.LayoutControlGroup lcgButtons;
        private DevExpress.XtraLayout.EmptySpaceItem esiPrefixButtons;
        private DevExpress.XtraGrid.Columns.GridColumn gclIsCompletePreValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riChkIsCompletePreValue;
        private DevExpress.XtraGrid.Columns.GridColumn gclValidateBomRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rilueValidateBomValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit tedValidateBomValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcCalculateRule;
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup riRadioRule;
        private DevExpress.XtraGrid.Columns.GridColumn gcLength;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit riRadioRules;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ritextLength;
        private DevExpress.XtraGrid.Columns.GridColumn gcFeeding;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}
#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Utils.UDA;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Hemera.Utils.Controls;
using System.Collections;
#endregion

namespace FanHai.Hemera.Addins.EDC
{
    /// <summary>
    /// 显示采样管理界面的用户控件类。
    /// </summary>
    public partial class SampManageCtl : BaseUserCtrl
    {
        //Define delegate manager control state
        public new delegate void AfterStateChanged(ControlState controlState);
        //Define event change control state
        public new AfterStateChanged afterStateChanged = null;
        //Define and initialize control state
        private ControlState _ctrlState = ControlState.Empty;

        //Control state property
        public ControlState CtrlState
        {
            get
            {
                return _ctrlState;
            }
            set
            {
                _ctrlState = value;
                if (afterStateChanged != null)
                {
                    afterStateChanged(value);
                }
            }
        }

        /// <summary>
        /// Construct function
        /// </summary>
        public SampManageCtl(SampManage sp)
        {
            InitializeComponent();
            afterStateChanged += new AfterStateChanged(this.OnAfterStateChanged);

            _sp = sp;

            if (null == _sp || _sp.SpName.Length < 1)
            {
                CtrlState = ControlState.New;
            }
            else
            {
                if (_sp.Status == EntityStatus.InActive)
                    CtrlState = ControlState.Edit;
                else
                    CtrlState = ControlState.ReadOnly;

                MapSampToControls();
            }
        }

        /// <summary>
        /// Control state change method
        /// </summary>
        /// <param name="state">Control state</param>
        private void OnAfterStateChanged(ControlState state)
        {
            switch (state)
            {
                #region case state of editer
                case ControlState.Edit:
                    toolbarSave.Enabled = true;
                    toolbarDelete.Enabled = true;
                    toolbarStatus.Enabled = true;

                    txtSpName.Properties.ReadOnly = true;
                    break;
                #endregion

                #region case state of New
                case ControlState.New:
                    txtSpName.Properties.ReadOnly = false;
                    //txtSpSize.Properties.ReadOnly = false;
                    txtSpVersion.Properties.ReadOnly = false;
                    //txtSpMaxSize.Properties.ReadOnly = false;
                    txtSpUnitSize.Properties.ReadOnly = false;

                    lpSampMode.Properties.ReadOnly = false;
                    lpUnitMode.Properties.ReadOnly = false;
                    //cbxSpPercentFlag.Enabled = true;

                    mmSpDescription.Properties.ReadOnly = false;

                    toolbarSave.Enabled = true;
                    toolbarStatus.Enabled = false;
                    toolbarDelete.Enabled = false;

                    _sp = new SampManage();
                    MapSampToControls();
                    break;
                #endregion

                #region case state of ReadOnly
                case ControlState.ReadOnly:
                    txtSpName.Properties.ReadOnly = true;
                    //txtSpSize.Properties.ReadOnly = true;
                    txtSpVersion.Properties.ReadOnly = true;
                    //txtSpMaxSize.Properties.ReadOnly = true;
                    txtSpUnitSize.Properties.ReadOnly = true;

                    lpSampMode.Properties.ReadOnly = true;
                    lpUnitMode.Properties.ReadOnly = true;
                    //cbxSpPercentFlag.Enabled = false;

                    mmSpDescription.Properties.ReadOnly = true;

                    toolbarSave.Enabled = false;
                    toolbarDelete.Enabled = false;
                    break;
                #endregion
            }
            toolbarNew.Enabled = true;
            toolbarQuery.Enabled = true;
        }

        #region Validation & Set Controls Data To Samp
        /// <summary>
        /// Validation & Collection Data for samp
        /// </summary>
        private void MapControlsToSamp()
        {
            if (null == _sp)
            {
                throw (new Exception("Error Samp Set"));
            }
            // TODO: Data validation
            _sp.SpName = txtSpName.Text;                //抽样名称
            _sp.SpVersion = txtSpVersion.Text;          //版本号
            _sp.SpDescription = mmSpDescription.Text;   //描述
            _sp.SpUnitSize = txtSpUnitSize.Text;        //抽样点数
            //_sp.SpSize = txtSpSize.Text;                //抽样数
            //_sp.SpMaxSize = txtSpMaxSize.Text;          //最大抽样数
            //if (cbxSpPercentFlag.Checked)               //百分比
            //    _sp.SpPercentFlag = "1";
            //else
            //    _sp.SpPercentFlag = "0";
            _sp.SpMode = lpSampMode.EditValue.ToString();           //模式：LastEdcTime	按时间抽检 Lot	按批次抽检 LotAccount	按批次数量抽检
            _sp.SpUnitMode = lpUnitMode.EditValue.ToString();       //
            _sp.SpStrategySize = txtStrategySize.Text;              //策略值

        }
        #endregion

        #region Validation & Set Samp Data To Controls
        /// <summary>
        /// Set samp data to Controls
        /// </summary>
        private void MapSampToControls()
        {
            txtSpName.Text = _sp.SpName;
            txtSpVersion.Text = _sp.SpVersion;
            mmSpDescription.Text = _sp.SpDescription;
            txtSpUnitSize.Text = _sp.SpUnitSize;
            //txtSpSize.Text = _sp.SpSize;
            //txtSpMaxSize.Text = _sp.SpMaxSize;
            //if (_sp.SpPercentFlag == "1")
            //    cbxSpPercentFlag.Checked = true;
            //else
            //    cbxSpPercentFlag.Checked = false;

            lpSampMode.EditValue = _sp.SpMode;
            lpUnitMode.EditValue = _sp.SpUnitMode;
            txtStrategySize.Text = _sp.SpStrategySize;
        }
        #endregion

        /// <summary>
        /// Tool Bar New Click
        /// </summary>
        private void toolbarNew_Click(object sender, EventArgs e)
        {
            foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}"))
                {
                    viewContent.WorkbenchWindow.SelectWindow();
                    SampManageCtl ctrl = (SampManageCtl)viewContent.Control.Controls.Find("SampManageCtl", true)[0];
                    if (ctrl.txtSpName.Text.Trim() != "")
                    {
                        if (MessageBox.Show(StringParser.Parse("${res:Global.ClearNoteMessage}"),
                            StringParser.Parse("${res:Global.SystemInfo}"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }

                    ctrl.CtrlState = ControlState.New;
                    return;
                }
            }

            SampViewContent sampContent = new SampViewContent(new SampManage());
            WorkbenchSingleton.Workbench.ShowView(sampContent);
        }

        /// <summary>
        /// Tool Bar Save Click
        /// </summary>
        private void toolbarSave_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.EnsureSaveCurrentData}", "${res:Global.SystemInfo}"))
            {
                MapControlsToSamp();

                if (_sp.SpName == string.Empty)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameNotNullMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }


                bool bNew = (CtrlState == ControlState.New);

                if (bNew && !_sp.SampNameValidate())
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:Global.NameIsExistMessage}"), StringParser.Parse("${res:Global.SystemInfo}"));
                    return;
                }

                if (_sp.Save(bNew))
                {
                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName =
                        StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}") + "_" + _sp.SpName;
                    CtrlState = ControlState.Edit;
                }
            }
        }

        /// <summary>
        /// Tool Bar Delete Click
        /// </summary>
        private void toolbarDelete_Click(object sender, EventArgs e)
        {
            if (MessageService.AskQuestion("${res:Global.DeleteNoteMessage}", "${res:Global.SystemInfo}"))
            {
                if (_sp.Delete())
                {
                    foreach (IViewContent viewContent in WorkbenchSingleton.Workbench.ViewContentCollection)
                    {
                        if (viewContent.TitleName == StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}"))
                        {
                            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
                            viewContent.WorkbenchWindow.SelectWindow();
                            return;
                        }
                    }

                    WorkbenchSingleton.Workbench.ActiveViewContent.TitleName = 
                        StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampViewContent}");
                    CtrlState = ControlState.New;
                }
            }
        }

        /// <summary>
        /// Tool Bar Query Click
        /// </summary>
        private void toolbarQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable spHashTable = new Hashtable();
                DataSet dataSet = new DataSet();

                string spName = this.tbSpName.Text.Trim();

                if (spName.Length > 0)
                {
                    spHashTable.Add(EDC_SP_FIELDS.FIELD_SP_NAME, spName);
                }

                DataTable spDataTable = FanHai.Hemera.Share.Common.CommonUtils.ParseToDataTable(spHashTable);
                spDataTable.TableName = EDC_SP_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(spDataTable);

                string msg = string.Empty;
                DataSet dsReturn = new DataSet();
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();

                if (serverFactory != null)
                {
                    dsReturn = serverFactory.CreateISampEngine().SearchSamp(dataSet);
                    msg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (msg != string.Empty)
                    {
                        MessageService.ShowError(msg);
                    }
                    else
                    {
                        grdCtrlSamp.MainView = gridViewSamp;
                        grdCtrlSamp.DataSource = dsReturn.Tables[EDC_SP_FIELDS.DATABASE_TABLE_NAME];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
        }

        #region set active status
        /// <summary>
        /// set active status
        /// </summary>
        private void toolbarStatus_Click(object sender, EventArgs e)
        {
            EntityStatus oldStatus = _sp.Status;
            //show dialog
            StatusDialog status = new StatusDialog(_sp);
            status.ShowDialog();

            if (_sp.Status != oldStatus)
            {
                //set page control status according to status
                if (_sp.Status == EntityStatus.Active || _sp.Status == EntityStatus.Archive)
                {
                    CtrlState = ControlState.ReadOnly;
                }
            }
        }
        #endregion

        #region Samp Mode Text Changed Event
        /// <summary>
        /// Samp Mode Text Changed Event
        /// 模式下拉选项changed事件
        /// </summary>
        private void lpSampMode_TextChanged(object sender, EventArgs e)
        {
            if (lpSampMode.EditValue.ToString() == "Lot")
            {
                txtStrategySize.Properties.ReadOnly = true;
            }
            else
            {
                txtStrategySize.Properties.ReadOnly = false;
            }
        }
        #endregion

        #region Bind Edc Mode
        /// <summary>
        /// 绑定模式在下拉列表中  modify yongbing.yang
        /// </summary>
        public void BindEdcMode()
        {
            string[] columns = new string[] { "CODE", "NAME" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Edc_Rule");

            this.lpSampMode.Properties.DataSource = BaseData.Get(columns, category);
            this.lpSampMode.Properties.DisplayMember = "NAME";
            this.lpSampMode.Properties.ValueMember = "CODE";
            this.lpSampMode.ItemIndex = 0;
        }
        #endregion

        #region Page load Initialize
        /// <summary>
        /// Page load Initialize
        /// </summary>
        private void SampManageCtl_Load(object sender, EventArgs e)
        {
            BindEdcMode();
            LoadResourceFileToUI();
        }
        #endregion

        #region Load resource file to UI
        /// <summary>
        /// Load resource file to UI
        /// </summary>
        private void LoadResourceFileToUI()
        {
            this.toolbarNew.Text = StringParser.Parse("${res:Global.New}");
            this.toolbarSave.Text = StringParser.Parse("${res:Global.Save}");
            this.toolbarQuery.Text = StringParser.Parse("${res:Global.Query}");
            this.toolbarDelete.Text = StringParser.Parse("${res:Global.Delete}");
            this.toolbarStatus.Text = StringParser.Parse("${res:Global.Status}");

            //this.lcgOne.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.grpCtrlBase}");
            this.lblSpName.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpName}");
            this.lblSpVersion.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpVersion}");
            this.lblSpDescription.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpDescription}");
            //this.lcgTwo.Text =
            //     StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.grbLot}");
            this.lblSpMode.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpMode}");
            //this.lblSpSize.Text =
            //     StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpSize}");
            //this.lblSpMaxSize.Text =
            //     StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpMaxSize}");
            this.lblSpUnitMode.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpMode}");
            this.lblSpUnitSize.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblSpUnitSize}");
            this.lblStrategySize.Text =
                 StringParser.Parse("${res:FanHai.Hemera.Addins.EDC.SampManageCtl.lblStrategySize}");
            this.lblMenu.Text = "基础数据 > 抽检管理 > 抽检项";

        }
        #endregion

        #region Private variable definition
        private SampManage _sp = null;
        #endregion

        private bool MapSelectedItemToProperties()
        {
            int rowHandle = gridViewSamp.GetDataRowHandleByGroupRowHandle(gridViewSamp.FocusedRowHandle);
            if (rowHandle >= 0)
            {
                _sp.SpKey = gridViewSamp.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_KEY).ToString();
                _sp.SpName = gridViewSamp.GetRowCellValue(rowHandle, EDC_SP_FIELDS.FIELD_SP_NAME).ToString();
                _sp.GetSampByKey(_sp.SpKey);
                return true;
            }
            return false;
        }

        private void gridViewSamp_DoubleClick(object sender, EventArgs e)
        {
            if (MapSelectedItemToProperties())
            {
                MapSampToControls();
            }
        }
    }
}

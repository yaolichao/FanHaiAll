
#region using
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FanHai.Hemera.Utils.Common;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using FanHai.Gui.Core;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;
using FanHai.Hemera.Share.Common;
using FanHai.Hemera.Share.Constants;
#endregion

namespace FanHai.Hemera.Utils.Controls
{

    public partial class BaseUserCtrl : DevExpress.XtraEditors.XtraUserControl
    {
        protected const string MENU_FONT_NAME = FontConst.MENU_FONT_NAME;
        protected const string TITLE_FONT_NAME = FontConst.TITLE_FONT_NAME;
        protected const string CONTENT_FONT_NAME = FontConst.CONTENT_FONT_NAME;
        protected const string CONTENT_GRID_FONT_NAME = FontConst.CONTENT_GRID_FONT_NAME;
        protected const int MENU_FONT_SIZE = FontConst.MENU_FONT_SIZE;
        protected const int TITLE_FONT_SIZE = FontConst.TITLE_FONT_SIZE;
        protected const int CONTENT_FONT_SIZE =FontConst.CONTENT_FONT_SIZE;
        protected const int CONTENT_GRID_FONT_SIZE = FontConst.CONTENT_GRID_FONT_SIZE;
        
        #region Protected Delegate Methods

        protected delegate void AfterStateChanged();

        protected event AfterStateChanged afterStateChanged = null;

        #endregion

        #region Private Properties

        private ControlState state = ControlState.Empty;

        #endregion

        #region Constructor

        public BaseUserCtrl()
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            //DevExpress.Utils.AppearanceObject.DefaultFont = new Font(MENU_FONT_NAME, MENU_FONT_SIZE);
            InitializeComponent();
        }

        #endregion

        #region Protected Properties

        protected ControlState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;

                if (afterStateChanged != null)
                {
                    afterStateChanged();
                }

                InitUIAuthoritiesByUser();
            }
        }

        #endregion

        #region Define Commonable UI Virtual Methods
        /// <summary>
        /// 初始化用户控件。
        /// </summary>
        protected virtual void InitUIControls()
        {

        }
        /// <summary>
        /// 根据区域特性初始化界面资源。
        /// </summary>
        protected virtual void InitUIResourcesByCulture()
        {

        }
        /// <summary>
        /// 根据用户初始化界面权限。
        /// </summary>
        protected virtual void InitUIAuthoritiesByUser()
        {

        }
        /// <summary>
        /// 映射控件到对应实体。
        /// </summary>
        protected virtual void MapControlsToEntity()
        {

        }
        /// <summary>
        /// 映射实体到对应控件。
        /// </summary>
        protected virtual void MapEntityToControls()
        {

        }

        #endregion

        #region Commonable UI Events

        private void BaseUserCtrl_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            //foreach (DevExpress.Skins.SkinContainer skin in DevExpress.Skins.SkinManager.Default.Skins)
            //{
            //    comboBox1.Items.Add(skin.SkinName);
            //}

            //if (MySkin.SelectedSkin != null)
            //{
            //    comboBox1.SelectedText = MySkin.SelectedSkin;
            //    this.defaultLookAndFeel.LookAndFeel.SkinName = MySkin.SelectedSkin;
            //}
            SetUI();
            InitUIControls();
            InitUIResourcesByCulture();
            InitUIAuthoritiesByUser();
            this.ResumeLayout(true);


        }
        /// <summary>
        /// 在窗体载入时调用，用来统一用户界面
        /// </summary>
        private void SetUI()
        {
            //控件背景色
            //this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(229)))), ((int)(((byte)(248)))));
            //this.Appearance.Options.UseBackColor = true;

            foreach (Control c in this.Controls)
            {
                Control[] lblInfoList = c.Controls.Find("lblInfos", true);
                if (lblInfoList != null && lblInfoList.Length > 0)
                {
                    lblInfoList[0].Text = "\t姓名：" + PropertyService.Get(PROPERTY_FIELDS.USER_NAME_MZ)
                       + "\n\t工号：" + PropertyService.Get("USER_NAME")
                       + "\n\t操作时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                //if (c.Name == "tableLayoutPanelMain")
                //{
                //    AdjustToolMainMenu(c);
                //    AdjustTitle(c);
                //    AdjustContent(c);
                //}
            }
        }

            
        private void AdjustToolMainMenu(Control control)
        {
            Control[] toolStrips=control.Controls.Find("toolStripMain", true);
            if (toolStrips.Length >= 1 && toolStrips[0] is ToolStrip)
            {
                ToolStrip toolstrip = toolStrips[0] as ToolStrip;
                toolstrip.AutoSize = true;
                toolstrip.BackgroundImage = null;
                toolstrip.BackColor = System.Drawing.Color.FromArgb(251, 248, 240);
                toolstrip.Font = new Font(MENU_FONT_NAME, MENU_FONT_SIZE);
                
            }
        }

        private void AdjustTitle(Control control)
        {
            Control[] pnlTitles = control.Controls.Find("PanelTitle", true);
            if (pnlTitles.Length >= 1 && pnlTitles[0] is PanelControl)
            {
                ((PanelControl)pnlTitles[0]).AutoSize = true;
                PanelControl ctrl = pnlTitles[0] as PanelControl;
                ctrl.Font = new Font(TITLE_FONT_NAME, TITLE_FONT_SIZE);
                foreach (Control c in ctrl.Controls)
                {
                    c.Font = new Font(TITLE_FONT_NAME, TITLE_FONT_SIZE);
                }
            }
        }

        private void AdjustContent(Control control)
        {
            Control[] contents = control.Controls.Find("Content", true);
            if (contents.Length >= 1)
            {
                contents[0].Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                AdjustContentFont(contents[0]);
            }
        }

        private void AdjustContentFont(Control control)
        {
           control.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
            if (control is LayoutControl)  //如果是LayoutControl
            {
                LayoutControl lControl = control as LayoutControl;
                foreach (BaseLayoutItem item in lControl.Items)
                {
                    if (item is LayoutControlItem)
                    {
                        item.AppearanceItemCaption.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                        if (((LayoutControlItem)item).Control != null)
                        {
                            ((LayoutControlItem)item).Control.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                        }
                    }
                    else if (item is LayoutGroup)
                    {
                        item.AppearanceItemCaption.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                        ((LayoutGroup)item).AppearanceGroup.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                    }
                }
                lControl.ResumeLayout(true);
            }
            else if (control is ComboBoxEdit)
            {
                ComboBoxEdit cmbEdit = control as ComboBoxEdit;
                cmbEdit.Properties.AppearanceDropDown.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
            }
            else if (control is LookUpEdit)
            {
                LookUpEdit luEdit = control as LookUpEdit;
                luEdit.Properties.AppearanceDropDown.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
            }
            else if (control is GroupControl)
            {
                ((GroupControl)control).Appearance.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                ((GroupControl)control).AppearanceCaption.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                ((GroupControl)control).AutoSize = true;
            }
            else if (control is XtraTabControl)
            {
                ((XtraTabControl)control).Appearance.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
                ((XtraTabControl)control).AppearancePage.Header.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
            }
            else if (control is GridControl)
            {
                ((GridControl)control).Font = new Font(CONTENT_GRID_FONT_NAME, CONTENT_GRID_FONT_SIZE);
                GridView gv = ((GridControl)control).MainView as GridView;
                if (gv != null)
                {
                    gv.Appearance.HeaderPanel.Font = new Font(CONTENT_GRID_FONT_NAME, CONTENT_GRID_FONT_SIZE);
                    gv.Appearance.Row.Font = new Font(CONTENT_GRID_FONT_NAME, CONTENT_GRID_FONT_SIZE);
                    gv.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(199, 237, 204);
                    gv.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(199, 237, 204);

                    gv.IndicatorWidth = 60;
                    gv.OptionsView.ShowIndicator = true;
                    gv.OptionsView.ShowAutoFilterRow = true;
                    gv.OptionsView.EnableAppearanceEvenRow = true;
                    gv.OptionsView.EnableAppearanceOddRow = true;
                    gv.Appearance.EvenRow.BackColor = Color.FromArgb(224, 224, 224);
                    gv.Appearance.OddRow.BackColor = Color.White;
                }
                return;
            }
           //else if (control is PaginationControl)
           //{
           //    ((PaginationControl)control).Font = new Font(CONTENT_GRID_FONT_NAME, CONTENT_GRID_FONT_SIZE);
           //    return;
           //}
           foreach (Control c in control.Controls)
           {
               if (c is Panel) ((Panel)c).AutoSize = true;
               if (c.Controls.Count > 0)
               {
                   AdjustContentFont(c);
               }
               else
               {
                   c.Font = new Font(CONTENT_FONT_NAME, CONTENT_FONT_SIZE);
               }
           }
        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.defaultLookAndFeel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = false;
            this.defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            //string SkinName = this.comboBox1.SelectedItem.ToString();
            //MySkin.SelectedSkin = SkinName;
            //this.defaultLookAndFeel.LookAndFeel.SkinName = SkinName;
        }
    }
}

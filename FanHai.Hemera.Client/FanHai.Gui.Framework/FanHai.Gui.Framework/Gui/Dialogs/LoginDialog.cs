using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Utils.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Management;
using System.Web.Security;
using System.Windows.Forms;

namespace FanHai.Gui.Framework.Gui
{
    public partial class LoginDialog : Form
    {
        public static bool flag = false; //if flag is true then exit the application 
        private string site = "";
        private string language = "";
        private string userKey = "";
        private string userNameMz = "";
        private string privilege = "";
        private string lines = "";
        private string operations = "";
        private string stores = "";
        private IDictionary languageDictionary = null;

        public LoginDialog()
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            InitializeComponent();
            #if DEBUG
            this.txtUserName.Text = "admin";
            this.txtPassword.Text = "123456";
            this.ShowInTaskbar = true;
            #else
            string userName = PropertyService.Get(PROPERTY_FIELDS.USER_NAME);
            this.txtUserName.Text = userName;
            this.txtPassword.Text = string.Empty;
            this.ShowInTaskbar = true;
            #endif
        }

        /// <summary>
        /// 更新语言信息。
        /// </summary>
        private void UpdateLanguageInfo()
        {
            string[] columns = new string[] { "LANGUAGE_NAME", "LANGUAGE_SIGN" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Language");
            DataTable languageTable = BaseData.Get(columns, category);
            if (languageTable.Rows.Count > 0)
            {
                Dictionary<string, string> dicLanguage = new Dictionary<string, string>();
                //把语言信息更新到XML文件
                for (int i = 0; i < languageTable.Rows.Count; i++)
                {
                    dicLanguage.Add(languageTable.Rows[i]["language_name"].ToString(), languageTable.Rows[i]["language_sign"].ToString());

                }
                CallRemotingService.UpdateConfigureLanguageXml(dicLanguage);
            }
        }

        /// <summary>
        /// 更新服务器列表。
        /// </summary>
        private void UpdateServerSitList()
        {
            //连接服务器列表进行更新
            string[] columns = new string[] { "SITE_NAME", "SITE_VALUE", "TIME_CONTROL", "FACTORY_CODE", "CHECK_OPERATOR" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Site");
            DataTable siteTable = BaseData.Get(columns, category);

            if (siteTable.Rows.Count > 0)
            {
                DataRow[] dataRow = siteTable.Select("SITE_NAME='" + site + "'");//查询自定义表siteTable的数据“select * from siteTable where site_name="+site+"”
                if (dataRow.Length > 0)
                {
                    string bIsTimeControl = "0";
                    if (!string.IsNullOrEmpty(dataRow[0]["TIME_CONTROL"].ToString()))
                    {
                        bIsTimeControl = dataRow[0]["TIME_CONTROL"].ToString();
                    }
                    PropertyService.Set(PROPERTY_FIELDS.TimeControl, bIsTimeControl);

                    string factoryCode = string.Empty;
                    if (!string.IsNullOrEmpty(dataRow[0]["FACTORY_CODE"].ToString()))
                    {
                        factoryCode = dataRow[0]["FACTORY_CODE"].ToString();
                    }
                    PropertyService.Set(PROPERTY_FIELDS.FACTORY_CODE, factoryCode.Trim());
                }

                Dictionary<string, string[]> dicSite = new Dictionary<string, string[]>();
                for (int i = 0; i < siteTable.Rows.Count; i++)
                {
                    string siteName = siteTable.Rows[i]["site_name"].ToString();
                    string siteValue = siteTable.Rows[i]["site_value"].ToString();
                    string factoryCode = siteTable.Rows[i]["FACTORY_CODE"].ToString();
                    string[] value = { siteValue, factoryCode };
                    dicSite.Add(siteName, value);

                }
//#if DEBUG
//                dicSite.Add("LocalHost", new string[] { "LocalHost", string.Empty });
//#endif
                CallRemotingService.UpdateConfigureSiteXml(dicSite);
            }

        }

        /// <summary>
        /// 更新登录用户信息。
        /// </summary>
        private void UpdateLoginUserInfo()
        {
            User user = new User();
            string factoryCode = PropertyService.Get(PROPERTY_FIELDS.FACTORY_CODE);
            user.UserKey = userKey;
            privilege = user.GetPrivilegeOfUser();
            if (user.ErrorMsg != "")
            {
                MessageService.ShowError("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.Msg.GetPrivilegeError}");
            }
            PropertyService.Set("UserPrivilege", privilege);
            lines = user.GetLineOfUser(userKey, factoryCode);
            //PropertyService.Set("Lines", lines);
            PropertyService.Set(PROPERTY_FIELDS.LINES, lines);
            operations = user.GetOperationOfUser(userKey);
            PropertyService.Set(PROPERTY_FIELDS.OPERATIONS, operations);
            stores = user.GetStoreOfUser(userKey, factoryCode);
            PropertyService.Set(PROPERTY_FIELDS.STORES, stores);

            PropertyService.Set(PROPERTY_FIELDS.USER_NAME, txtUserName.Text.Trim());
            PropertyService.Set(PROPERTY_FIELDS.USER_KEY, userKey);
            PropertyService.Set(PROPERTY_FIELDS.USER_NAME_MZ, userNameMz);//add by qym 20120530
            PropertyService.Set(PROPERTY_FIELDS.TIMEZONE, "CN-ZH");
            PropertyService.Set(PROPERTY_FIELDS.COMPUTER_NAME, System.Environment.MachineName);
            PropertyService.Set(PROPERTY_FIELDS.COMPUTER_MAC, getComputerMAC());
            PropertyService.Set(PROPERTY_FIELDS.SITE, site);
            //add rayna liu 2011-07-15 清空charg和lgort的历史记录
            PropertyService.Set(PROPERTY_FIELDS.CHARG, new List<string>());
            PropertyService.Set(PROPERTY_FIELDS.LGORT, new List<string>());
            PropertyService.Set("CURVERSION", RevisionClass.FullVersion);

            //end
            //IDictionary Dictionary = CallRemotingService.QueryLanguageOption();
            if (null != languageDictionary)
            {
                foreach (string s in languageDictionary.Keys)
                {
                    if (s == this.cmbLanguage.SelectedItem.ToString())
                    {
                        PropertyService.Set(PROPERTY_FIELDS.uiLanguageProperty, languageDictionary[s].ToString());
                    }
                }
            }

            user.LogUserLoginInfo(userKey, site, language, RevisionClass.FullVersion);
        }
        //定期删除,间隔时间一个月，写入xml日期
        private void Xmlproper()
        {
            DateTime de;
            if (PropertyService.Get("Bmonthdata") == "")
            {
                de = DateTime.Now;
            }
            else
            {
                de = Convert.ToDateTime(PropertyService.Get("Bmonthdata"));
            }
            //DateTime de = PropertyService.Get("Bmonthdata") == "" ? PropertyService.Set<string>("Bmonthdata", DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd")) : Convert.ToDateTime(PropertyService.Get("Bmonthdata"));
            TimeSpan ts = de - DateTime.Now;
            string[] prop = PropertyService.Pproperties();
            if (ts.Days <= 0)
            {
                for (int j = 0; j < prop.Length; j++)
                {
                    char[] Uchar = prop[j].Substring(0, 1).ToCharArray();
                    if (Uchar[0] >= 0x4e00 && Uchar[0] <= 0x9fbb)
                    {
                        PropertyService.Remove(prop[j]);
                    }
                }
                //PropertyService.Save();
                PropertyService.Set<string>("Bmonthdata", DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd"));
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            //对登陆界面的帐号密码进行判断并对XML 进行重新配置
            if (CheckData(this.txtUserName.Text.Trim(), this.txtPassword.Text.Trim()))
            {
                try
                {
                    CallRemotingService.SelectServerSite(site);
                    DataSet ds = new DataSet();
                    DataTable dataTable = CreateUserTable();//创建一张自定义结构的表
                    Dictionary<string, string> rowData = new Dictionary<string, string>() 
                                                            {
                                                                {RBAC_USER_FIELDS.FIELD_USERNAME,txtUserName.Text.Trim()}                                                               
                                                            };
                    FanHai.Hemera.Utils.Common.Utils.AddRowDataToDataTable(ref dataTable, rowData);//把rowData中的数据添加在表dataTable中
                    ds.Tables.Add(dataTable);//把表dataTable添加到表集ds中
                    DataSet dataUser = new DataSet();

                    IServerObjFactory isof = CallRemotingService.GetRemoteObject();//创建工厂接口对象
                    IUserEngine userEngine = isof.CreateIUserEngine();

                    dataUser = userEngine.CheckUser(ds); //通过传入的帐号查询返回该帐号对应的信息

                    if (CheckUser(dataUser))//数据库信息和用户信息进行对比返回bool类型
                    {
                        UpdateLanguageInfo();
                        UpdateServerSitList();
                        //对登录用户的信息进行登记
                        UpdateLoginUserInfo();

                        DialogResult = DialogResult.OK;
                        this.Close();
                        StatusBarService.SetCaretPosition(30, 70, 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageService.ShowMessage(ex.Message);
                }
                finally
                {
                    CallRemotingService.UnregisterChannel();
                }
            }
            Xmlproper();
        }

        /// <summary>
        /// 通过参数传递手动创建一张表并通过方法对表进行列的增加
        /// </summary>
        /// <returns></returns>
        private DataTable CreateUserTable()
        {
            List<string> fields = new List<string>()
            { 
                 RBAC_USER_FIELDS.FIELD_USERNAME                                                           
            };
            return FanHai.Hemera.Utils.Common.Utils.CreateDataTableWithColumns(RBAC_USER_FIELDS.DATABASE_TABLE_NAME, fields);
        }

        /// <summary>
        /// dataUser中账户的信息和登录帐号、密码进行对比来确认该账户的正确性
        /// </summary>
        /// <param name="dataUser"></param>
        /// <returns></returns>
        private bool CheckUser(DataSet dataUser)
        {
            //对登陆界面密码进行加密处理
            string passwordGUID = FormsAuthentication.HashPasswordForStoringInConfigFile(this.txtPassword.Text.Trim(), "SHA1");

            if (dataUser == null || !dataUser.Tables.Contains(RBAC_USER_FIELDS.DATABASE_TABLE_NAME) ||
                dataUser.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME].Rows.Count == 0)
            {
                MessageService.ShowError("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.Msg.LoginError}");
                return false;
            }
            else
            {
                DataTable dtUser = dataUser.Tables[RBAC_USER_FIELDS.DATABASE_TABLE_NAME];
                string userName = dtUser.Rows[0][RBAC_USER_FIELDS.FIELD_BADGE].ToString();
                string password = dtUser.Rows[0][RBAC_USER_FIELDS.FIELD_PASSWORD].ToString();

                //判断数据库用户信息和登录信息是否一致
                if (userName.ToLower() == this.txtUserName.Text.ToLower().Trim() && password == passwordGUID)
                {
                    //获取数据库中的userKey
                    userKey = dtUser.Rows[0][RBAC_USER_FIELDS.FIELD_USER_KEY].ToString();
                    userNameMz = dtUser.Rows[0][RBAC_USER_FIELDS.FIELD_USERNAME].ToString(); //add by qym 20120530
                    return true;
                }
                else
                {
                    MessageService.ShowError("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.Msg.PasswordError}");
                    return false;
                }
            }
        }

        /// <summary>
        /// check whether the userName and password is legit
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>True if input is legit,otherwise return false</returns>
        private bool CheckData(string userName, string password)
        {
            if (userName == "" || password == "")//判断帐号密码是否为空
            {
                MessageService.ShowError("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.Msg.LoginEmpty}");
                return false;
            }
            else if (userName.IndexOf("'") > 0 || password.IndexOf("'") > 0)//防止注入
            {
                MessageService.ShowError("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.Msg.ErrorChar}");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (WorkbenchSingleton.Workbench != null)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                LoginDialog.flag = true;
                this.Close();
            }
        }

        private void LoginDialog_Load(object sender, EventArgs e)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "logo.ico";

            InitUIResourcesByCulture();
            //this.Icon = IconService.GetIcon("Icons.16x16.Logo");
            this.Icon = IconService.GetIcon(file);
            try
            {
                Dictionary<string, string> siteDictionary = CallRemotingService.QueryServerSite();

                foreach (KeyValuePair<string, string> s in siteDictionary)
                {
                    this.cmbSite.Properties.Items.Add(s.Key);
                }
                string defaultSite = PropertyService.Get(PROPERTY_FIELDS.SITE);
                if (string.IsNullOrEmpty(defaultSite))
                {
                    this.cmbSite.SelectedIndex = 0;
                }
                else
                {
                    cmbSite.SelectedText = defaultSite;
                }
                site = cmbSite.SelectedItem.ToString();
                cmbSite.Visible = false;
                languageDictionary = CallRemotingService.QueryLanguageOption();

                foreach (string s in languageDictionary.Keys)
                {
                    this.cmbLanguage.Properties.Items.Add(s);
                }
                this.cmbLanguage.SelectedIndex = 0;
                //隐藏语言选择栏位。
                if (languageDictionary.Keys.Count > 0)
                {
                    this.lblLanguage.Visible = false;
                    this.cmbLanguage.Visible = false;
                }
                language = cmbLanguage.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 根据区域特性初始化UI资源
        /// </summary>
        private void InitUIResourcesByCulture()
        {
            this.lblUserName.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblUserName}");
            this.lblPassword.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblPassword}");
            this.lblSite.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblSite}");
            this.lblLanguage.Text = StringParser.Parse("${res:FanHai.Gui.Framework.Gui.Dialogs.Login.lblLanguage}");
            this.btnOK.Text = StringParser.Parse("${res:Global.OKButtonText}");
            this.btnCancel.Text = StringParser.Parse("${res:Global.Cancel}");
        }

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            site = this.cmbSite.SelectedItem.ToString();
        }
        
        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            language = this.cmbLanguage.SelectedItem.ToString();
            if (null != languageDictionary)
            {
                foreach (string s in languageDictionary.Keys)
                {
                    if (s == this.cmbLanguage.SelectedItem.ToString())
                    {
                        PropertyService.Set(PROPERTY_FIELDS.uiLanguageProperty, languageDictionary[s].ToString());
                    }
                }
            }
            InitUIResourcesByCulture();
        }


        private void LoginDialog_Paint(object sender, PaintEventArgs e)
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LoginDialog));
            Image myImage = (Image)(resources.GetObject("login.bg"));
            Graphics myGraphics = e.Graphics;

            if (myImage != null)
            {
                myGraphics.DrawImage(myImage, 0, 0, 468, 251);
            }
        }

        #region Client DragAndDrop
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region Private const definition
        // Drag client area
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x01;
        private const int HTCAPTION = 0x02;
        #endregion

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.Focus();
            }
        }

        //获取计算机MAC地址
        private string getComputerMAC()
        {
            string mac = string.Empty;

            ManagementClass mc;

            mc = new ManagementClass("Win32_NetworkAdapterConfiguration"); 
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {

                if (mo["IPEnabled"].ToString() == "True")

                    mac = mo["MacAddress"].ToString();

            }

            return mac;
        }
    }
}

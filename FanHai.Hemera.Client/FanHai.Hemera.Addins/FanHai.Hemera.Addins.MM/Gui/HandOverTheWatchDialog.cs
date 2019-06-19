using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Share.CommonControls.Dialogs;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Entities;
using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Interface;

namespace FanHai.Hemera.Addins.MM
{
    public partial class HandOverTheWatchDialog : BaseDialog
    {
        #region define variables
        private string _lupFactoryRoom = string.Empty;
        private string _cmbGongXuName = string.Empty;
        private string _lupJiaoBanShife = string.Empty;
        private string _lupJieBanShife = string.Empty;
        private DateTime _timJiaoBanStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
        private DateTime _timJiaoBanEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        private string _lupZhuangTai = string.Empty;        
        #endregion

        #region Properties
        public string FactoryRoom
        {
            get
            {
                return _lupFactoryRoom;
            }
            set
            {
                _lupFactoryRoom = value;
            }
        }
        public string GongXuName
        {
            get
            {
                return _cmbGongXuName;
            }
            set
            {
                _cmbGongXuName = value;
            }
        }
        public string JiaoBanShife
        {
            get
            {
                return _lupJiaoBanShife;
            }
            set
            {
                _lupJiaoBanShife = value;
            }
        }
        public string JieBanShife
        {
            get
            {
                return _lupJieBanShife;
            }
            set
            {
                _lupJieBanShife = value;
            }
        }
        public DateTime JiaoBanStart
        {
            get
            {
                return _timJiaoBanStart;
            }
            set
            {
                _timJiaoBanStart = value;
            }
        }
        public DateTime JiaoBanEnd
        {
            get
            {
                return _timJiaoBanEnd;
            }
            set
            {
                _timJiaoBanEnd = value;
            }
        }
        public string ZhuangTai
        {
            get
            {
                return _lupZhuangTai;
            }
            set
            {
                _lupZhuangTai = value;
            }
        }
        
        #endregion

        public HandOverTheWatchDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(timJiaoBanStart.EditValue) == "")
            {
                timJiaoBanStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            }
            if (Convert.ToString(timJiaoBanEnd.EditValue) == "")
            {
                timJiaoBanEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (DateTime.Compare(Convert.ToDateTime(timJiaoBanStart.Text), Convert.ToDateTime(timJiaoBanEnd.Text)) > 0)
            {
                MessageService.ShowMessage("截止时间不能小于起始时间！", "系统提示！");
                timJiaoBanStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
                timJiaoBanEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                SelectedItemToList();
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        /// <summary>
        /// 获取控件数据赋值给页面参数
        /// </summary>
        /// <returns></returns>
        private void SelectedItemToList()
        {
            _lupFactoryRoom = lupFactoryRoom.Text.Trim(); 
            _cmbGongXuName = cmbGongXuName.Text.Trim();
            _lupJiaoBanShife = lupJiaoBanShife.Text.Trim();
            _lupJieBanShife = lupJieBanShife.Text.Trim();
            _timJiaoBanStart = DateTime.Parse(timJiaoBanStart.EditValue.ToString());
            _timJiaoBanEnd = DateTime.Parse(timJiaoBanEnd.EditValue.ToString());
            _lupZhuangTai = lupZhuangTai.Text;
        }

        /// <summary>
        /// 通过用户拥有的线边仓获取工厂名称到控件中
        /// </summary>
        public void GetFacRoomByStores()
        {
            #region
            //绑定工厂车间名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            dt2.Rows.InsertAt(dt2.NewRow(), 0);
            //绑定工厂车间数据到窗体控件。
            lupFactoryRoom.Properties.DataSource = dt2;
            lupFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
            lupFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
            //线别表中有数据，设置窗体控件的默认索引为0。
            if (dt2.Rows.Count > 0)
            {
                lupFactoryRoom.ItemIndex = -1;
            }
            #endregion
        }

        /// <summary>
        /// 获取登陆用户拥有权限的工序名称
        /// </summary>
        public void GetGongXu()
        {
            //绑定工序
            #region Bind Operation
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)
            {
                string[] strOperations = operations.Split(',');
                cmbGongXuName.Properties.Items.Add("");             //添加空行
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cmbGongXuName.Properties.Items.Add(strOperations[i]);
                }
                this.cmbGongXuName.SelectedIndex = -1;
            }
            #endregion
        }

        public void GetShift()
        {
            #region
            /// <summary>
            /// 绑定班别信息
            /// </summary>
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            DataTable dt1 = BaseData.Get(columns, category);
            dt1.Rows.InsertAt(dt1.NewRow(),0);
            this.lupJiaoBanShife.Properties.DataSource = dt1;
            this.lupJiaoBanShife.Properties.DisplayMember = "CODE";
            this.lupJiaoBanShife.Properties.ValueMember = "CODE";
            this.lupJiaoBanShife.ItemIndex = 0;

            this.lupJieBanShife.Properties.DataSource = dt1;
            this.lupJieBanShife.Properties.DisplayMember = "CODE";
            this.lupJieBanShife.Properties.ValueMember = "CODE";
            this.lupJieBanShife.ItemIndex = 0;
            #endregion
        }
     
        /// <summary>
        /// 关闭当前窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 视图载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandOverTheWatchDialog_Load(object sender, EventArgs e)
        {
            GetFacRoomByStores();
            GetGongXu();
            GetShift();
            timJiaoBanStart.EditValue = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            timJiaoBanEnd.EditValue = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void timJiaoBanEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (timJiaoBanEnd.Text == "")
            {
                timJiaoBanEnd.EditValue = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void timJiaoBanStart_EditValueChanged(object sender, EventArgs e)
        {
            if (timJiaoBanStart.Text == "")
            {
                timJiaoBanStart.EditValue = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
            }
        }

    }
}

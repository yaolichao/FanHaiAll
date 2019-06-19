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
    public partial class UseMaterialDialog : BaseDialog
    {
        #region define variables
        private string _materialLot = string.Empty;
        private string _gongXuName = string.Empty;
        private string _wuLiaoNumber = string.Empty;
        private string _factoryRoomName = string.Empty;
        private string _wuLiaoMiaoShu = string.Empty;
        private string _equipmentName = string.Empty;
        private string _gongYingShang = string.Empty;
        private string _banCi = string.Empty;
        private string _lineCang = string.Empty;
        private string _jobNumber = string.Empty;
        private DateTime _startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
        private DateTime _endTime =DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        #endregion

        #region Properties
        public string MaterialLot
        {
            get
            {
                return _materialLot;
            }
            set
            {
                _materialLot = value;
            }
        }
        public string GongXuName
        {
            get
            {
                return _gongXuName;
            }
            set
            {
                _gongXuName = value;
            }
        }
        public string WuLiaoNumber
        {
            get
            {
                return _wuLiaoNumber;
            }
            set
            {
                _wuLiaoNumber = value;
            }
        }
        public string FactoryRoomName
        {
            get
            {
                return _factoryRoomName;
            }
            set
            {
                _factoryRoomName = value;
            }
        }
        public string WuLiaoMiaoShu
        {
            get
            {
                return _wuLiaoMiaoShu;
            }
            set
            {
                _wuLiaoMiaoShu = value;
            }
        }
        public string EquipmentName
        {
            get
            {
                return _equipmentName;
            }
            set
            {
                _equipmentName = value;
            }
        }
        public string GongYingShang
        {
            get
            {
                return _gongYingShang;
            }
            set
            {
                _gongYingShang = value;
            }
        }
        public string BanCi
        {
            get
            {
                return _banCi;
            }
            set
            {
                _banCi = value;
            }
        }
        public string LineCang
        {
            get
            {
                return _lineCang;
            }
            set
            {
                _lineCang = value;
            }
        }
        public string JobNumber
        {
            get
            {
                return _jobNumber;
            }
            set
            {
                _jobNumber = value;
            }
        }
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
            }
        }
        #endregion

        public UseMaterialDialog()
        {
            InitializeComponent();
            timStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            timEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(Convert.ToDateTime(timStart.Text), Convert.ToDateTime(timEnd.Text)) > 0)
            {
                MessageService.ShowMessage("截止时间不能小于起始时间！", "系统提示！");
                timStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
                timEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                MapSelectedItemToList();
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        /// <summary>
        /// 获取控件数据赋值给页面参数
        /// </summary>
        /// <returns></returns>
        private void MapSelectedItemToList()
        {
            _materialLot = cmbMaterialLot.Text;
            _gongXuName = cmbGongXuName.Text;
            _wuLiaoNumber = cmbWuLiaoNumber.Text;
            _factoryRoomName = cmbFactoryRoomName.Text;
            _wuLiaoMiaoShu = txtWuLiaoMiaoShu.Text;
            _equipmentName = cmbEquipmentName.Text;
            _gongYingShang = txtGongYingShang.Text;
            _banCi = cmbBanCi.Text;
            _lineCang = cmbLineCang.Text;
            _jobNumber = cmbJobNumber.Text;
            _startTime = DateTime.Parse(timStart.EditValue.ToString());
            _endTime = DateTime.Parse(timEnd.EditValue.ToString());
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

        private void UseMaterialDialog_Load(object sender, EventArgs e)
        {
            //绑定工序
            #region Bind Operation
            string operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
            if (operations.Length > 0)
            {
                string[] strOperations = operations.Split(',');
                cmbGongXuName.Properties.Items.Add("");
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cmbGongXuName.Properties.Items.Add(strOperations[i]);
                }
                this.cmbGongXuName.SelectedIndex = -1;
            }
            #endregion

            //绑定线边仓
            #region Bind Operation
            string STORE = PropertyService.Get(PROPERTY_FIELDS.STORES);
            if (STORE.Length > 0)
            {
                string[] strOperations = STORE.Split(',');
                cmbLineCang.Properties.Items.Add("");
                for (int i = 0; i < strOperations.Length; i++)
                {
                    cmbLineCang.Properties.Items.Add(strOperations[i]);
                }
                this.cmbLineCang.SelectedIndex = -1;
            }
            #endregion

            #region
            //绑定工厂车间名称
            DataTable dt2 = FactoryUtils.GetFactoryRoomByStores(PropertyService.Get(PROPERTY_FIELDS.STORES));
            dt2.Rows.InsertAt(dt2.NewRow(), 0);
            //绑定工厂车间数据到窗体控件。
            cmbFactoryRoomName.Properties.DataSource = dt2;
            cmbFactoryRoomName.Properties.DisplayMember = "LOCATION_NAME";
            cmbFactoryRoomName.Properties.ValueMember = "LOCATION_KEY";
            //线别表中有数据，设置窗体控件的默认索引为0。
            if (dt2.Rows.Count > 0)
            {
                 cmbFactoryRoomName.ItemIndex = -1;
            }
            #endregion

            #region
            /// <summary>
            /// 绑定班别信息
            /// </summary>
            string[] columns = new string[] { "CODE" };
            KeyValuePair<string, string> category = new KeyValuePair<string, string>("CATEGORY_NAME", "Basic_Shift");
            DataTable dt1=BaseData.Get(columns, category);
            dt1.Rows.InsertAt(dt1.NewRow(), 0);
            this.cmbBanCi.Properties.DataSource = dt1;
            this.cmbBanCi.Properties.DisplayMember = "CODE";
            this.cmbBanCi.Properties.ValueMember = "CODE";
            this.cmbBanCi.ItemIndex = -1;
            //GetNowShift();
            #endregion
        }

        ///// <summary>
        ///// 但前班次信息
        ///// </summary>
        //public void GetNowShift()
        //{
        //    DataTable dataTable = new DataTable();
        //    try
        //    {
        //        //创建远程调用的工厂对象。
        //        IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
        //        if (null != serverFactory)//工厂对象不为null
        //        {
        //            //调用远程方法，根据分类名和列名从数据库中查询基础数据。
        //            dataTable = serverFactory.CreatIReceiveMaterialEngine().GetShiftName();

        //            if (dataTable.Rows.Count > 0)
        //            {
        //                cmbBanCi.EditValue = dataTable.Rows[0][0].ToString().Trim();
        //            }
        //        }
        //    }
        //    finally
        //    {

        //    }
        //}

        /// <summary>
        /// 设备名称绑定
        /// </summary>
        public void GetEquipment()
        {
            //绑定设备名称
            #region
            string operationname = cmbGongXuName.Text;
            string cmbfactoryroom = cmbFactoryRoomName.Text;
            UseMaterial _material = new UseMaterial();
            ////根据根据工序车间获取设备信息。
            DataSet dsMaterial = _material.GetEquipmentInfo(operationname, cmbfactoryroom);
            if (_material.ErrorMsg.Length < 1)//如果执行查询失败。
            {
                if (dsMaterial.Tables.Count > 0)//查询结果数据集中有表。
                {
                    //绑定工厂车间数据到窗体控件。
                    //dsMaterial.Tables[0].Rows.Add();
                    dsMaterial.Tables[0].Rows.InsertAt(dsMaterial.Tables[0].NewRow(), 0);
                    cmbEquipmentName.Properties.DataSource = dsMaterial.Tables[0];
                    cmbEquipmentName.Properties.DisplayMember = "EQUIPMENT_NAME";
                    cmbEquipmentName.Properties.ValueMember = "EQUIPMENT_KEY";
                    //线别表中有数据，设置窗体控件的默认索引为0。
                    if (dsMaterial.Tables[0].Rows.Count > 0)
                    {
                        cmbEquipmentName.ItemIndex = -1;
                    }
                }
            }
            else
            {
                MessageService.ShowError(_material.ErrorMsg);
            }
            #endregion
        }

        private void cmbGongXuName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEquipment();
        }

        private void cmbFactoryRoomName_EditValueChanged(object sender, EventArgs e)
        {
            GetEquipment();
        }

        private void timEnd_EditValueChanged(object sender, EventArgs e)
        {
            //if (DateTime.Compare(Convert.ToDateTime(timStart.Text), Convert.ToDateTime(timEnd.Text)) > 0)
            //{
            //    MessageService.ShowMessage("截止时间不能小于起始时间！");
            //    timEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            //}
            if (timEnd.Text == "")
            {
                timEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void timStart_EditValueChanged(object sender, EventArgs e)
        {
            if (timStart.Text == "")
            {
                timStart.EditValue = DateTime.Now.ToString("yyyy-MM-01");
            }
        }

    }
}

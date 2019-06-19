using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FanHai.Hemera.Utils.Controls;
using FanHai.Gui.Core;
using System.Collections;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
using System.IO;
using FanHai.Hemera.Utils.Entities;
using DevExpress.XtraLayout.Utils;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using FanHai.Hemera.Utils.Controls.Common;

namespace FanHai.Hemera.Addins.WIP
{
    public partial class LotQueryCustCheck : BaseUserCtrl
    {
        IVTestDataEntity _entity = new IVTestDataEntity();
        LotAfterIvTestEntity _lotAfterIvTestEntity = new LotAfterIvTestEntity();
        int _width = 0, _height = 0;
        string _factoryname = string.Empty, lot_num = string.Empty, _d_date = string.Empty;
        private string MESSAGEBOX_CAPTION = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotDispatch.INFORMATION}"); //提示
        string picAddress = "";//图片地址


        public LotQueryCustCheck()
        {
            InitializeComponent();
            InitializeLanguage();
        }



        private void InitializeLanguage()
        {
            this.chkDefault.Properties.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.chkDefault}");//"有效数据";
            this.btnExportExcel.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.btnExportExcel}");//"导出数据";
            this.btnQuery.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.btnQuery}");//"查询";
            this.layoutControlItem8.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem8}");//"组件序列号";
            this.layoutControlItem9.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem9}");//"终检日期-开始";
            this.layoutControlItem5.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem5}");//"托号";
            this.layoutControlItem3.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem3}");//"产品ID号";
            this.layoutControlItem10.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem10}");//"终检日期-结束";
            this.layoutControlItem19.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem19}");//"终检设备号";
            this.layoutControlItem18.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem18}");//"工厂车间";
            this.layoutControlItem21.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem21}");//"测试日期-开始";
            this.layoutControlItem22.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem22}");//"测试日期-结束";
            this.layoutControlItem23.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem23}");//"测试机台号";
            this.layoutControlItem25.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem25}");//"工单号";
            this.lblMenu.Text = "质量管理>质量作业>功率信息";//"终检信息查询";
            this.tab_custcheck.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.tab_custcheck}");//"终检信息";
            this.RN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.RN}");//"序号";
            this.LOCATION_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.LOCATION_NAME}");//"车间";
            this.LOT_NUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.LOT_NUM}");//"组件序列号";
            this.PRO_ID.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.PRO_ID}");//"产品ID号";
            this.WORKNUMBER.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.WORKNUMBER}");//"工单号";
            this.EQUIPMENT_NAME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.EQUIPMENT_NAME}");//"终检台";
            this.CREATE_TIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.CREATE_TIME}");//"检验时间";
            this.PRO_LEVEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.PRO_LEVEL}");//"等级";
            this.gcNamePlatena.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.gcNamePlatena}");//"铭牌编码";
            this.LOT_COLOR.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.LOT_COLOR}");//"花色";
            this.LOT_CUSTOMERCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.LOT_CUSTOMERCODE}");//"客户序号";
            this.PALLET_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.PALLET_NO}");//"托号";
            this.CS_DATA_GROUP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.CS_DATA_GROUP}");//"托状态";
            this.AMBIENTTEMP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.AMBIENTTEMP}");//"测试温度";
            this.INTENSITY.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.INTENSITY}");//"光强";
            this.FF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.FF}");//"填充因子(%)";
            this.EFF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.EFF}");//"组件转换效率";
            this.TTIME.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.TTIME}");//"测试时间";
            this.DEVICENUM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.DEVICENUM}");//"设备编码";
            this.VC_PSIGN.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.VC_PSIGN}");//"打印标示";
            this.DT_PRINTDT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.DT_PRINTDT}");//"打印时间";
            this.VC_DEFAULT.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.VC_DEFAULT}");//"有效数据";

            this.SENSORTEMP.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.SENSORTEMP}");//"环境温度(C)";
            this.VC_CUSTCODE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.VC_CUSTCODE}");//"客户编码";
            this.OPERATERS.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.OPERATERS}");//"操作员";
            this.COEF_PMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_PMAX}");//"衰减最大功率";
            this.COEF_ISC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_ISC}");//"衰减短路电流";
            this.COEF_VOC.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_VOC}");//"衰减开路电压";
            this.COEF_IMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_IMAX}");//"衰减最大工作电流";
            this.COEF_VMAX.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_VMAX}");//"衰减最大工作电压";
            this.COEF_FF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.COEF_FF}");//"衰减填充因子";
            this.VC_CELLEFF.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.VC_CELLEFF}");//"电池片效率";
            this.DEC_CTM.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.DEC_CTM}");//"CTM转换";
            this.CALIBRATION_NO.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.CALIBRATION_NO}");//"标准版序号";
            this.PARAM_VALUE.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.PARAM_VALUE}");//"接线盒料号";
            this.POWERLEVEL.Caption = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.POWERLEVEL}");//"电流分档";
            this.tab_elpic.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.tab_elpic}");//"EL图片";
            this.sbtnMagnify.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.sbtnMagnify}");//"放大";
            this.sbtnReduce.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.sbtnReduce}");//"缩小";
            this.layoutControlItem6.Text = StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.layoutControlItem6}");//"图片类型";
            GridViewHelper.SetGridView(gvIvTestData);
        }



        private void LotQueryCustCheck_Load(object sender, EventArgs e)
        {
            BindFactoryRoom();
            LoadCustEquipments();
            LoadIvTestEquipments();
            LoadProductLevel();
            this.panel1.Dock = DockStyle.Fill;
            this.layout_progressbar.Visibility = LayoutVisibility.Never;
        }

        /// <summary>
        /// 绑定工厂车间。
        /// </summary>
        private void BindFactoryRoom()
        {
            this.lueFactoryRoom.EditValue = string.Empty;
            string strLines = PropertyService.Get(PROPERTY_FIELDS.LINES);
            DataTable dt = FactoryUtils.GetFactoryRoomByLines(strLines);
            //绑定工厂车间数据到窗体控件。
            if (dt != null)
            {
                this.lueFactoryRoom.Properties.DataSource = dt;
                this.lueFactoryRoom.Properties.DisplayMember = "LOCATION_NAME";
                this.lueFactoryRoom.Properties.ValueMember = "LOCATION_KEY";
                this.lueFactoryRoom.ItemIndex = 0;
            }
            else
            {
                this.lueFactoryRoom.Properties.DataSource = null;
                this.lueFactoryRoom.EditValue = string.Empty;
            }
            if (dt == null || dt.Rows.Count <= 1)
            {
                this.lueFactoryRoom.Properties.ReadOnly = true;
            }
        }
        /// <summary>
        /// 加载终检设备
        /// </summary>
        private void LoadCustEquipments()
        {
            if (this.lueFactoryRoom.EditValue == null || string.IsNullOrEmpty(this.lueFactoryRoom.EditValue.ToString()))
                return;

            #region 加载设备
            DataTable dtEquipment = new EquipmentEntity().GetEquipments(lueFactoryRoom.EditValue.ToString(), "终检").Tables[0];

            if (dtEquipment != null && dtEquipment.Rows.Count > 0)
            {
                this.betEquipment.Properties.Items.Clear();
                foreach (DataRow dr in dtEquipment.Rows)
                {
                    string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                    //string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);
                   string equipmentKey = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY]);
                   
                    this.betEquipment.Properties.Items.Add(equipmentKey.Trim(), equipmentName);
                }
            }
            else
            {
                this.betEquipment.Properties.Items.Clear();
            }
            #endregion
        }
        /// <summary>
        /// 加载测试设备
        /// </summary>
        private void LoadIvTestEquipments()
        {
            if (this.lueFactoryRoom.EditValue == null || string.IsNullOrEmpty(this.lueFactoryRoom.EditValue.ToString()))
                return;

            #region 加载设备
            DataTable dtEquipment = new EquipmentEntity().GetEquipments(lueFactoryRoom.EditValue.ToString(), "组件测试").Tables[0];

            if (dtEquipment != null && dtEquipment.Rows.Count > 0)
            {
                this.betTestEquipment.Properties.Items.Clear();
                foreach (DataRow dr in dtEquipment.Rows)
                {
                    string equipmentName = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_NAME]);
                    string equipmentCode = Convert.ToString(dr[EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_CODE]);

                    this.betTestEquipment.Properties.Items.Add(equipmentCode.Trim(), equipmentName);
                }
            }
            else
            {
                this.betTestEquipment.Properties.Items.Clear();
            }
            #endregion
        }

        private void LoadProductLevel()
        {
            //绑定产品等级
            string[] l_s = new string[] { "Column_Name", "Column_Index", "Column_type", "Column_code" };
            string category = "Basic_TestRule_PowerSet";
            DataTable dtProLevel = BaseData.Get(l_s, category);
            DataTable dtLevel = dtProLevel.Clone();
            dtLevel.TableName = "Level";
            DataRow[] drs = dtProLevel.Select(string.Format("Column_type='{0}'", BASE_POWERSET.PRODUCT_GRADE));
            foreach (DataRow dr in drs)
                dtLevel.ImportRow(dr);
            DataView dview = dtLevel.DefaultView;
            dview.Sort = "Column_Index asc";
            repositoryItemLookUpEdit_PRO_LEVEL.DisplayMember = "Column_Name";
            repositoryItemLookUpEdit_PRO_LEVEL.ValueMember = "Column_code";
            repositoryItemLookUpEdit_PRO_LEVEL.DataSource = dview.Table;
        }

        private void LoadPicAddress()
        {
            //绑定图片地址作业
            string[] l_s01 = new string[] { "PIC_ADDRESS","PIC_DATE_FORMAT", "PIC_ORDER_INDEX", "PIC_FACTORY_CODE", "PIC_TYPE", "PIC_ADDRESS_NAME" };
            string category01 = "Uda_Pic_Address";
            DataTable dtPicAddress = BaseData.Get(l_s01, category01);
            DataTable dtEl = dtPicAddress.Clone();
            DataTable dtIv = dtPicAddress.Clone();
            dtEl.TableName = "EL";
            DataRow[] drs01 = null;
            if (!string.IsNullOrEmpty(_factoryname))
                drs01 = dtPicAddress.Select(string.Format(" PIC_TYPE='{0}' AND PIC_FACTORY_CODE='{1}'", "EL", _factoryname));
            else
                drs01 = dtPicAddress.Select(string.Format(" PIC_TYPE='{0}'", "EL"));

            foreach (DataRow dr in drs01)
                dtEl.ImportRow(dr);
            DataView dv = dtEl.DefaultView;
            dv.Sort = "PIC_ORDER_INDEX ASC";
            DataTable dtElPic = dv.ToTable();
            this.lue_elpic.Properties.DataSource = dtElPic;
            this.lue_elpic.EditValue = string.Empty;
            if (dtEl.Rows.Count > 0)
                this.lue_elpic.EditValue = Convert.ToString(dtElPic.Rows[0]["PIC_ADDRESS"]);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            InitGvDataBind();
        }

        private void paginationCustCheck_DataPaging()
        {
            InitGvDataBind();
        }

        private void InitGvDataBind()
        {
            if (string.IsNullOrEmpty(betTestEquipment.Text.Trim())
                     && string.IsNullOrEmpty(txtLotNumber.Text.Trim())
                     && string.IsNullOrEmpty(txtPalletNo.Text.Trim())
                     && string.IsNullOrEmpty(txtPro_Id.Text.Trim())
                     && string.IsNullOrEmpty(dateStart.Text.Trim())
                     && string.IsNullOrEmpty(dateEnd.Text.Trim())
                     && string.IsNullOrEmpty(betEquipment.Text.Trim())
                     && string.IsNullOrEmpty(txtWo.Text.Trim())
                && string.IsNullOrEmpty(dateStartTest.Text.Trim())
                && string.IsNullOrEmpty(dateEndTest.Text.Trim()))
            {
                MessageService.ShowMessage(StringParser.Parse("${res:Global.ServerMessage.M0006}"), MESSAGEBOX_CAPTION);//请输入【查询条件】
                //MessageService.ShowMessage("请输入【查询条件】", "提示");
                return;
            }

            Hashtable hsParams = new Hashtable();
            if (!string.IsNullOrEmpty(txtLotNumber.Text.Trim()))
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM, txtLotNumber.Text.Trim());
            if (!string.IsNullOrEmpty(txtPro_Id.Text.Trim()))
                hsParams.Add(POR_LOT_FIELDS.FIELD_PRO_ID, txtPro_Id.Text.Trim());
            if (!string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
                hsParams.Add(POR_LOT_FIELDS.FIELD_PALLET_NO, txtPalletNo.Text.Trim());          

            if (!string.IsNullOrEmpty(dateStart.Text.Trim()))
                hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1", dateStart.Text.Trim());
            if (!string.IsNullOrEmpty(dateEnd.Text.Trim()))
                hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2", dateEnd.Text.Trim());

            if (!string.IsNullOrEmpty(dateStartTest. Text.Trim()))
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1", dateStartTest.Text.Trim());
            if (!string.IsNullOrEmpty(dateEndTest.Text.Trim()))
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2", dateEndTest.Text.Trim());

            if (!string.IsNullOrEmpty(lueFactoryRoom.EditValue.ToString()))
                hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY, lueFactoryRoom.EditValue.ToString());

            if (!string.IsNullOrEmpty(this.txtWo.Text.Trim()))
                hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER, this.txtWo.Text.Trim());
    
            if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
            {               
                hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM, betEquipment.EditValue.ToString());
            }
            if (!string.IsNullOrEmpty(betTestEquipment.Text.Trim()))
            {
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1", betTestEquipment.EditValue.ToString());
            }

            if (this.chkDefault.Checked)
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT, "1");
            else
                hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT, "0");

            //DataSet dsReturn = _entity.GetIvTestToCustCheckQuery(hsParams);
            this.gvIvTestData.FocusedRowHandle = -1;

            #region 添加分页查询
            DataSet reqDS = new DataSet();
            DataSet dsReturn = new DataSet();

            int pages, records, pageNo, pageSize;
            this.paginationCustCheck.GetPaginationProperties(out pageNo, out pageSize);

            if (pageNo <= 0)
            {
                pageNo = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = PaginationControl.DEFAULT_PAGESIZE;                          //每页行数DEFAULT_PAGESIZE=20
            }

            dsReturn = _entity.GetIvTestToCustCheckQuery(reqDS, pageNo, pageSize, out  pages, out  records, hsParams);

            try
            {
                if (pages > 0 && records > 0)
                {
                    this.paginationCustCheck.PageNo = pageNo > pages ? pages : pageNo;
                    this.paginationCustCheck.PageSize = pageSize;
                    this.paginationCustCheck.Pages = pages;
                    this.paginationCustCheck.Records = records;
                }
                else
                {
                    this.paginationCustCheck.PageNo = 0;
                    this.paginationCustCheck.PageSize = PaginationControl.DEFAULT_PAGESIZE;
                    this.paginationCustCheck.Pages = 0;
                    this.paginationCustCheck.Records = 0;
                }
            }
            catch //(Exception ex)
            {

            }

            //dsRetun = exchangewoEntity.GetExchangeWoData(hstable);
            if (!string.IsNullOrEmpty(_entity.ErrorMsg))
            {
                MessageService.ShowError(_entity.ErrorMsg);
                return;
            }

            #endregion
            
            gcIvTestData.DataSource = null;
            gcIvTestData.MainView = gvIvTestData;           
            gcIvTestData.DataSource = dsReturn.Tables[WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME];
            gvIvTestData.BestFitColumns();
            GetFocusedRowHandelData();
        }


        private void lue_elpic_EditValueChanged(object sender, EventArgs e)
        {
            string val = Convert.ToString(lue_elpic.EditValue);
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            DataTable dtEL = this.lue_elpic.Properties.DataSource as DataTable;
            DataRow[] drs = dtEL.Select(string.Format("PIC_ADDRESS='{0}'", val));
            string dateFormat = string.Empty;
            if (drs.Length > 0)
            {
                dateFormat = Convert.ToString(drs[0]["PIC_DATE_FORMAT"]);
            }
            if (string.IsNullOrEmpty(_factoryname)) return;
            bool isExistFieldFold = true;
            picAddress = CombAddress(val, dateFormat, out isExistFieldFold);
            if (!isExistFieldFold) return;


            if (!File.Exists(picAddress))
            {
                // MessageService.ShowError(string.Format("【图片名称{0}】不存在，请确认!", lot_num));
                return;
            }
            //this.lblMenu.Text = picAddress;

            if (this.xtraTabControl1.SelectedTabPage == this.tab_elpic)
            {
                LoadLotPic(picAddress);
            }
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (gvIvTestData.SelectedRowsCount>0)
            {
                LoadLotPic(picAddress);
            }
        }

        /// <summary>
        /// 显示组件序列号图片
        /// </summary>
        private void LoadLotPic(string picAddress)
        {
            try
            {
                Image img = Image.FromFile(picAddress);

                //测试数据
                //Image img = Image.FromFile(@"E:\3108530257304632.jpg");
                layout_picaddress.Visibility = LayoutVisibility.Always;
                _width = img.Width;
                _height = img.Height;

                pic_el.Image = img;
                pic_el.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageService.ShowMessage(ex.Message, MESSAGEBOX_CAPTION);
                return;
            }

        }
        /// <summary>
        /// 返回图片地址
        /// </summary>
        /// <param name="address">配置的图片路径</param>
        /// <param name="factoryname">配置的图片路径—图片所在工厂</param>
        /// year—图片路径中的年
        /// month—图片路径中的月
        /// sdate—图片路径中的年月日
        /// lot_num—图片目录中的文件名称
        /// <returns>返回图片路径地址</returns>
        private string CombAddress(string address,string dateFormat, out bool isExistFieldFold)
        {
            isExistFieldFold = true;
            string address_Return = string.Empty;

            //DateTime dtime = _lotAfterIvTestEntity.GetSysdate();
            DateTime dtime = Convert.ToDateTime(_d_date);
            string sdate = dtime.ToString("yyyy-M-d");
            if (!string.IsNullOrEmpty(dateFormat))
            {
                sdate = dtime.ToString(dateFormat);
            }
            string month = dtime.Month.ToString() + DATETIME_CLASS.DATETIME_MONTH;
            string year = dtime.Year.ToString() + DATETIME_CLASS.DATETIME_YEAR;
            address_Return = address + "\\" + year + "\\" + month + "\\" + sdate;

            if (!Directory.Exists(address_Return))
            {
                MessageService.ShowError(string.Format("【图片路径】-{0}", address_Return));
                isExistFieldFold = false;
                return address_Return;
            }

            address_Return = address + "\\" + year + "\\" + month + "\\" + sdate + "\\" + lot_num + ".jpg";
            if (!File.Exists(address_Return))
            {
                MessageService.ShowError(string.Format("【图片路径】-{0}", address_Return));
                isExistFieldFold = false;
                return address_Return;
            }
         
            return address_Return;
        }

        private void rdioGroup_EditValueChanged(object sender, EventArgs e)
        {
            //适合尺寸
            if (rdioGroup.EditValue.ToString() == "0")
            {
                pic_el.Width = panel1.Width;
                pic_el.Height = panel1.Height;
            }
            //原始尺寸
            if (rdioGroup.EditValue.ToString() == "1")
            {
                pic_el.Width = _width;
                pic_el.Height = _height;
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvIvTestData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvIvTestData.FocusedRowHandle > -1)
            {
                GetFocusedRowHandelData();
            }
        }

        private void GetFocusedRowHandelData()
        {
            if (gvIvTestData.FocusedRowHandle > -1)
            {
                layout_picaddress.Visibility = LayoutVisibility.Never;
                _factoryname = gvIvTestData.GetFocusedRowCellValue(FMM_LOCATION_FIELDS.FIELD_LOCATION_NAME).ToString();
                lot_num = gvIvTestData.GetFocusedRowCellValue(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM).ToString();
                DataRow dr = gvIvTestData.GetFocusedDataRow();
                _d_date = Convert.ToString(dr[WIP_IV_TEST_FIELDS.FIELDS_T_DATE]);

                //加载图片地址
                LoadPicAddress();
            }
        }

        private void sbtnMagnify_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(speSize.Text.Trim()) < 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.Msg001}"), MESSAGEBOX_CAPTION);//请设定图片【放大范围】
                //MessageService.ShowMessage("请设定图片【放大范围】", "提示");
                return;
            }
            int width = 0, height = 0;
            if (pic_el.Image != null)
            {
                width = pic_el.Width + Convert.ToInt16(speSize.Text.Trim());
                height = pic_el.Height + Convert.ToInt16(speSize.Text.Trim());
                if (width > _width)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.Msg002}"), MESSAGEBOX_CAPTION);//EL图片尺寸【超出范围】
                    //MessageService.ShowMessage("EL图片尺寸【超出范围】", "提示");
                    return;
                }
                pic_el.Width = width;
                pic_el.Height = height;
            }          
        }

        private void sbtnReduce_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(speSize.Text.Trim()) < 1)
            {
                MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.Msg003}"), MESSAGEBOX_CAPTION);//请设定图片【缩小范围】
                //MessageService.ShowMessage("请设定图片【缩小范围】", "提示");
                return;
            }
            int width = 0, height = 0;
            if (pic_el.Image != null)
            {
                width = pic_el.Width - Convert.ToInt16(speSize.Text.Trim());
                height = pic_el.Height - Convert.ToInt16(speSize.Text.Trim());
                if (width > _width)
                {
                    MessageService.ShowMessage(StringParser.Parse("${res:FanHai.Hemera.Addins.WIP.LotQueryCustCheck.Msg004}"), MESSAGEBOX_CAPTION);//EL图片尺寸【最小范围】
                    //MessageService.ShowMessage("EL图片尺寸【最小范围】", "提示");
                    return;
                }
                pic_el.Width = width;
                pic_el.Height = height;
            }          
        }

        private void gvIvTestData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void lueFactoryRoom_EditValueChanged(object sender, EventArgs e)
        {
            LoadCustEquipments();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (this.gvIvTestData.RowCount > 0)
                print();
        }
        /// <summary>
        /// 导出表格数据
        /// </summary>
        /// DataGridView dataGridView1
        public void print()
        {
            try
            {
                #region //获取查询条件到hashtable
                Hashtable hsParams = new Hashtable();
                if (!string.IsNullOrEmpty(txtLotNumber.Text.Trim()))
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM, txtLotNumber.Text.Trim());
                if (!string.IsNullOrEmpty(txtPro_Id.Text.Trim()))
                    hsParams.Add(POR_LOT_FIELDS.FIELD_PRO_ID, txtPro_Id.Text.Trim());
                if (!string.IsNullOrEmpty(txtPalletNo.Text.Trim()))
                    hsParams.Add(POR_LOT_FIELDS.FIELD_PALLET_NO, txtPalletNo.Text.Trim());

                if (!string.IsNullOrEmpty(dateStart.Text.Trim()))
                    hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "1", dateStart.Text.Trim());
                if (!string.IsNullOrEmpty(dateEnd.Text.Trim()))
                    hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_CREATE_TIME + "2", dateEnd.Text.Trim());

                if (!string.IsNullOrEmpty(dateStartTest.Text.Trim()))
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "1", dateStartTest.Text.Trim());
                if (!string.IsNullOrEmpty(dateEndTest.Text.Trim()))
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_TTIME + "2", dateEndTest.Text.Trim());

                if (!string.IsNullOrEmpty(lueFactoryRoom.EditValue.ToString()))
                    hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_ROOM_KEY, lueFactoryRoom.EditValue.ToString());

                if (!string.IsNullOrEmpty(this.txtWo.Text.Trim()))
                    hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER, this.txtWo.Text.Trim());

                if (!string.IsNullOrEmpty(betEquipment.EditValue.ToString()))
                {
                    hsParams.Add(WIP_CUSTCHECK_FIELDS.FIELDS_DEVICENUM, betEquipment.EditValue.ToString());
                }
                if (!string.IsNullOrEmpty(betTestEquipment.Text.Trim()))
                {
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_DEVICENUM + "1", betTestEquipment.EditValue.ToString());
                }

                if (this.chkDefault.Checked)
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT, "1");
                else
                    hsParams.Add(WIP_IV_TEST_FIELDS.FIELDS_VC_DEFAULT, "0");
                #endregion  

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel文件(*.xls)|*.xls";
                dlg.FilterIndex = 1;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DataTable dtReturn = _entity.GetIvTestToCustCheckQueryForImport(hsParams).Tables[WIP_IV_TEST_FIELDS.DATABASE_TABLE_NAME];

                    dtReturn.Columns.Add("ROW_NUM", typeof(Decimal));
                    StringBuilder sbFieldNames = new StringBuilder();
                    foreach (GridColumn gc in this.gvIvTestData.Columns)
                    {
                        if (dtReturn.Columns.Contains(gc.FieldName))
                        {
                            dtReturn.Columns[gc.FieldName].Caption = gc.Caption;
                            sbFieldNames.AppendFormat("{0},", gc.FieldName);
                        }
                    }

                    for (int i = 0; i < dtReturn.Rows.Count; i++)
                    {
                        DataRow dr = dtReturn.Rows[i];
                        dr["ROW_NUM"] = i + 1;                        
                    }
                    string fileNames = sbFieldNames.ToString().TrimEnd(',');
                    Export.ExportToExcel(dlg.FileName, dtReturn.DefaultView.ToTable(false, fileNames.Split(',')));
                }
                MessageBox.Show("导出成功", "提示！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误提示");
            }


            ////导出到execl  
            //try
            //{
            //    DevExpress.XtraGrid.Views.Grid.GridView gvExport = this.gvIvTestData;

            //    string fieldpath = string.Empty, fileNameExt = string.Empty;
            //    saveExcelDialog.Filter = "excel文件(*.xls)|*.xls";
            //    saveExcelDialog.DefaultExt = "xls";
            //    saveExcelDialog.InitialDirectory = Directory.GetCurrentDirectory();

            //    saveExcelDialog.RestoreDirectory = true;

            //    if (DialogResult.OK == saveExcelDialog.ShowDialog())
            //    {
            //        int rowscount = gvExport.RowCount;
            //        int colscount = gvExport.Columns.Count;

            //        if (rowscount > 65536)
            //        {
            //            MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }

            //        //列数不可以大于255
            //        if (colscount > 255)
            //        {
            //            MessageBox.Show("数据记录行数太多，不能保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }

            //        fieldpath = saveExcelDialog.FileName;
            //        fileNameExt = fieldpath.Substring(fieldpath.LastIndexOf("\\") + 1);

            //        //验证以fileNameString命名的文件是否存在，如果存在删除它
            //        FileInfo file = new FileInfo(fieldpath);
            //        if (file.Exists)
            //        {
            //            try
            //            {
            //                file.Delete();
            //            }
            //            catch (Exception error)
            //            {
            //                MessageBox.Show(error.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                return;
            //            }
            //        }
            //        Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
            //        Microsoft.Office.Interop.Excel.Workbook objWorkbook = objExcel.Workbooks.Add(Missing.Value);
            //        Microsoft.Office.Interop.Excel.Worksheet objsheet = (Microsoft.Office.Interop.Excel.Worksheet)objWorkbook.ActiveSheet;
            //        try
            //        {
            //            //设置EXCEL不可见
            //            objExcel.Visible = false;                
            //            //向Excel中写入表格的表头
            //            int displayColumnsCount = 1;
            //            for (int i = 0; i < gvExport.Columns.Count; i++)
            //            {
            //                if (gvExport.Columns[i].Visible == true)
            //                {
            //                    string tmp = gvExport.Columns[i].Caption;
            //                    //objExcel.Cells[1, displayColumnsCount] = tmp;
            //                    objsheet.Cells[1, displayColumnsCount] = tmp;                              
            //                    displayColumnsCount++;
            //                }
            //            }                       

            //             //tempProgressBar = new System.Windows.Forms.ProgressBar();
            //            //设置进度条
                        
            //            this.layout_progressbar.Visibility = LayoutVisibility.Always;
            //            tempProgressBar.Maximum = 1;
            //            tempProgressBar.Refresh();                        
            //            tempProgressBar.Minimum = 1;
            //            tempProgressBar.Maximum = gvExport.RowCount;
            //            tempProgressBar.Step = 1;

            //            //向Excel中逐行逐列写入表格中的数据
            //            for (int row = 0; row < gvExport.RowCount; row++)
            //            {
            //                tempProgressBar.PerformStep();
            //                string sfront = "'";
            //                displayColumnsCount = 1;
            //                for (int col = 0; col < colscount; col++)
            //                {
            //                    if (gvExport.Columns[col].Visible == true)
            //                    {
            //                        try
            //                        {
            //                            if (gvExport.Columns[col].FieldName.Equals(WIP_IV_TEST_FIELDS.FIELDS_LOT_NUM)
            //                                || gvExport.Columns[col].FieldName.Equals(WIP_CUSTCHECK_FIELDS.FIELDS_WORKNUMBER))
            //                                objsheet.Cells[row + 2, displayColumnsCount] = sfront + gvExport.GetRowCellValue(row, gvExport.Columns[col].FieldName).ToString().Trim();
            //                            else
            //                                objsheet.Cells[row + 2, displayColumnsCount] = gvExport.GetRowCellValue(row, gvExport.Columns[col].FieldName).ToString().Trim();
            //                        }
            //                        catch(Exception ex)
            //                        {}
            //                        displayColumnsCount++;
            //                    }
            //                }
            //            }
            //            //隐藏进度条
            //            this.layout_progressbar.Visibility = LayoutVisibility.Never;

            //            //保存文件
            //            objWorkbook.SaveAs(fieldpath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            //                Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
            //                Missing.Value, Missing.Value);


            //        }
            //        catch (Exception error)
            //        {
            //            MessageBox.Show(error.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //        finally
            //        {
            //            //关闭Excel应用
            //            if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
            //            if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
            //            if (objExcel != null) objExcel.Quit();

            //            objsheet = null;
            //            objWorkbook = null;
            //            objExcel = null;
            //        }

            //        MessageBox.Show(fieldpath + "\n\n导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "错误提示");
            //}
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FanHai.Hemera.Utils.Common;
using FanHai.Hemera.Share.Constants;
using FanHai.Gui.Core;
using FanHai.Hemera.Utils.Entities;
using FanHai.Gui.Framework.Gui;
using FanHai.Hemera.Share.Interface;

using DevExpress.XtraGrid.Views.Base;
using FanHai.Hemera.Utils.Controls;

namespace FanHai.Hemera.Addins.MM
{
    /// <summary>
    /// 表示材料耗用的窗体类。
    /// </summary>
    public partial class UseMaterialListCtrl : BaseUserCtrl
    {

        #region define variables
        string _materialLot = string.Empty;
        string _gongXuName = string.Empty;
        string _wuLiaoNumber = string.Empty;
        string _factoryRoomName = string.Empty;
        string _wuLiaoMiaoShu = string.Empty;
        string _equipmentName = string.Empty;
        string _gongYingShang = string.Empty;
        string _banCi = string.Empty;
        string _lineCang = string.Empty;
        string _jobNumber = string.Empty;
        DateTime _startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01"));
        DateTime _endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        string _stores = PropertyService.Get(PROPERTY_FIELDS.STORES);
        string _operations = PropertyService.Get(PROPERTY_FIELDS.OPERATIONS);
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public UseMaterialListCtrl()
        {
            InitializeComponent();
        }
         
        private void tsbSerch_Click(object sender, EventArgs e)
        {
            UseMaterialDialog ud = new UseMaterialDialog();
            if (ud.ShowDialog() == DialogResult.OK)
            {
                SetRetrunToDefine(ud);
                BindDateGrid();
            }
        }
        /// <summary>
        /// 对查询界面返回值进行取值赋值到变量中
        /// </summary>
        /// <param name="ud">窗体对象</param>
        public void SetRetrunToDefine(UseMaterialDialog ud)
        {
            this._materialLot=ud.MaterialLot;
            this._gongXuName = ud.GongXuName;
            this._wuLiaoNumber = ud.WuLiaoNumber;
            this._factoryRoomName = ud.FactoryRoomName;
            this._wuLiaoMiaoShu = ud.WuLiaoMiaoShu;
            this._equipmentName = ud.EquipmentName;
            this._gongYingShang = ud.GongYingShang;
            this._banCi = ud.BanCi;
            this._lineCang = ud.LineCang;
            this._jobNumber = ud.JobNumber;
            this._startTime = ud.StartTime;
            this._endTime = ud.EndTime;
        }

        /// <summary>
        /// 关闭当前视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClose_Click(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(true);
        }
        /// <summary>
        /// 绑定数据到数据表
        /// </summary>
        public void BindDateGrid()
        {
            #region
            UseMaterial _material = new UseMaterial();
            if (_material.ErrorMsg.Length < 1)//如果执行查询失败。
            {               
                gcUsedMaterialList.MainView = gvUsedMaterialListMain;
                gcUsedMaterialList.DataSource = _material.GetStoreMaterialDetail(_materialLot, _gongXuName, _wuLiaoNumber,
                    _factoryRoomName, _wuLiaoMiaoShu, _equipmentName, _gongYingShang, _banCi, _lineCang, _jobNumber, _startTime, 
                    _endTime, _stores, _operations).Tables[0];
                gvUsedMaterialListMain.BestFitColumns();
            }
            else
            {
                MessageService.ShowError(_material.ErrorMsg);
            }
            #endregion

        }

        /// <summary>
        /// 界面载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseMaterialListCtrl_Load(object sender, EventArgs e)
        {
            #region
            UseMaterial _material = new UseMaterial();
            if (_material.ErrorMsg.Length < 1)//如果执行查询失败。
            {
                gcUsedMaterialList.MainView = gvUsedMaterialListMain;
                gcUsedMaterialList.DataSource = _material.GetAllMaterialUsed().Tables[0];
                gvUsedMaterialListMain.BestFitColumns();
            }
            else
            {
                MessageService.ShowError(_material.ErrorMsg);
            }
            #endregion
        }
    }
}

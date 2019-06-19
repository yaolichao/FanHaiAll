/*
<FileInfo>
  <Author>rayna liu FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Utils.Common;
using FanHai.Gui.Core;
using FanHai.Hemera.Share.Interface;
using System.Collections;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.Entities.EquipmentManagement
{
    public class EquipmentLayoutEntity
    {
        #region Constructor
        public EquipmentLayoutEntity()
        {

        }
        #endregion

        #region Properties
        private string _LayoutName =string.Empty;
        private string _LayoutDesc = string.Empty;
        private string _layoutKey = string.Empty;

        public string LayoutName
        {
            set { this._LayoutName = value; }
            get { return _LayoutName; }
        }
        public string LayoutKey
        {
            set { this._layoutKey = value; }
            get { return _layoutKey; }
        }
        public string LayoutDesc
        {
            set { this._LayoutDesc = value; }
            get { return _LayoutDesc; }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 保存设备布局图
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public bool InsertEquipmentLayout(DataSet dataSet)
        {            
            string msg;
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "InsertEquipmentLayout", dataSet, out msg);
            
            
            if (string.IsNullOrEmpty(msg))
            {
                _layoutKey = resDS.ExtendedProperties.ContainsKey(PARAMETERS.INPUT_KEY) ? resDS.ExtendedProperties[PARAMETERS.INPUT_KEY].ToString() : string.Empty;
                return true;
            }
            else
            {
                MessageService.ShowError(msg);
                return false;
            }
        }

        /// <summary>
        /// 更新设备布局图
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public bool UpdateEquipmentLayout(DataSet dataSet)
        {
            string msg;
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "UpdateEquipmentLayout", dataSet, out msg);
            if (string.IsNullOrEmpty(msg))
                return true;
            else
            {
                MessageService.ShowError("更新设备布局图失败："+msg);
                return false;
            }           
        }

        /// <summary>
        /// 查询设备布局图
        /// </summary>
        /// <param name="layoutName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable SearchEquipmentLayout(string layoutName,out string msg)
        {
            DataSet reqDS = new DataSet();
            reqDS.ExtendedProperties.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_NAME,layoutName);
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "SearchEquipmentLayout", reqDS, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return resDS.Tables[EMS_LAYOUT_MAIN_FIELDS.DATABASE_TABLE_NAME];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询设备布局图明细数据
        /// </summary>
        /// <param name="layoutKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataSet GetEquipmentLayoutDetail(string layoutKey,out string msg)
        {
            DataSet reqDS = new DataSet();
            reqDS.ExtendedProperties.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY,layoutKey);
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "GetEquipmentLayout", reqDS, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return resDS;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除设备布局图
        /// </summary>
        /// <param name="layoutKey"></param>
        /// <returns></returns>
        public bool DeleteEquipmentLayout(string layoutKey)
        {
            string msg;
            DataSet reqDS = new DataSet();
            reqDS.ExtendedProperties.Add(EMS_LAYOUT_MAIN_FIELDS.LAYOUT_KEY, layoutKey);
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "DeleteEquipmentLayout", reqDS, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return true;
            }
            else
            {
                MessageService.ShowError("删除布局图失败："+msg);
                return false;
            }
        }
        /// <summary>
        /// 查询设备布局图明细数据
        /// </summary>
        /// <param name="layoutKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataSet GetLayoutEquipmentCurrStates(DataTable dt, out string msg)
        {
            DataSet reqDs=new DataSet();
            reqDs.Tables.Add(dt.Copy());
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentStateEventsEngine, FanHai.Hemera.Modules.EMS", "GetLayoutEquipmentCurrStates", reqDs, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return resDS;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得单个设备的记录
        /// </summary>
        /// <param name="layoutKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataSet GetSingleLayoutEventDoWorkHistory(Dictionary<string,string> _dictionary, out string msg)
        {
            DataSet reqDs=new DataSet();
            foreach (KeyValuePair<string, string> kvl in _dictionary)
            {
                reqDs.ExtendedProperties.Add(kvl.Key, kvl.Value);
            }        
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "GetSingleLayoutEventDoWorkHistory", reqDs, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return resDS;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetCurrentEquWorkList(Dictionary<string, string> _dictionary, out string msg)
        {
            DataSet reqDs = new DataSet();
            msg = string.Empty;
            foreach (KeyValuePair<string, string> kvl in _dictionary)
            {
                reqDs.ExtendedProperties.Add(kvl.Key, kvl.Value);
            }
            DataSet resDS = FanHai.Hemera.Utils.Common.Utils.ExecuteEngineMethod("FanHai.Hemera.Modules.EMS.EquipmentLayoutEngine, FanHai.Hemera.Modules.EMS", "GetCurrentEquWorkList", reqDs, out msg);
            if (string.IsNullOrEmpty(msg))
            {
                return resDS;
            }
            else
            {
                return null;
            }
        }
        #endregion     
    }

    public class EquipmentLayoutDetailEntity
    {
        #region Properties
        private string _layoutKey = string.Empty;//主表主键
        private string _equipmentKey = string.Empty;//设备主键
        private string _equipmentName = string.Empty;//设备名称
        private string _picLeft= string.Empty;  //图片左上角X坐标
        private string _picTop = string.Empty;  //图片左上角Y坐标
        private string _picWidth = string.Empty; //图片宽度
        private string _picHeight = string.Empty;//图片高度
        private string _picType = string.Empty;//图片类型 
        private string _detailColKey = string.Empty;//明细表主键
        private string _parentEquKey = string.Empty;//父设备主键

        private string _chamberTotal = string.Empty;//腔体个数
        private string _chamberIndex = string.Empty;//腔体编号
        private string _isMultiChamber = string.Empty;//是否多腔体设备
        private string _picName = string.Empty;//设备对于的图片的名称 
        private string _color = string.Empty;//设备当前颜色

        private string _flag = string.Empty;
        private string _editor = string.Empty;
        private string _editTime = string.Empty;
        /// <summary>
        /// 主表主键
        /// </summary>
        public string LayoutKey
        {
            get { return _layoutKey; }
            set { _layoutKey = value; }
        }
        /// <summary>
        /// 设备主键
        /// </summary>
        public string EquipmentKey
        {
            get { return _equipmentKey; } 
            set { _equipmentKey = value; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return _equipmentName; }
            set { _equipmentName = value; }
        }
        /// <summary>
        /// 图片左上角X坐标
        /// </summary>
        public string PicLeft
        {
            get { return _picLeft; }
            set { _picLeft = value; }
        }
        /// <summary>
        /// 图片左上角Y坐标
        /// </summary>
        public string PicTop
        {
            get { return _picTop; }
            set { _picTop = value; }
        }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public string PicWidth
        {
            get { return _picWidth; }
            set { _picWidth = value; }
        }
        /// <summary>
        /// 图片高度
        /// </summary>
        public string PicHeight
        {
            get { return _picHeight; }
            set { _picHeight = value; }
        }
        /// <summary>
        /// 图片类型
        /// </summary>
        public string PicType
        {
            get { return _picType; }
            set { _picType = value; }
        }
        /// <summary>
        /// 明细表主键
        /// </summary>
        public string DetailColKey
        {
            get { return _detailColKey; }
            set { _detailColKey = value; }
        }
        /// <summary>
        /// 父设备主键
        /// </summary>
        public string ParentEquKey
        {
            get { return _parentEquKey; }
            set { _parentEquKey = value; }
        }
        /// <summary>
        /// 腔体个数
        /// </summary>
        public string ChamberTotal
        {
            get { return _chamberTotal; }
            set { _chamberTotal = value; }
        }
        /// <summary>
        /// 腔体编号
        /// </summary>
        public string ChamberIndex
        {
            get { return _chamberIndex; }
            set { _chamberIndex = value; }
        }
        /// <summary>
        /// 是否多腔体设备
        /// </summary>
        public string IsMultiChamber
        {
            get { return _isMultiChamber; }
            set { _isMultiChamber = value; }
        }
        /// <summary>
        /// 设备对于的图片的名称
        /// </summary>
        public string PicName
        {
            get { return _picName; }
            set { _picName = value; }
        }
        /// <summary>
        /// 当前设备状态
        /// </summary>
        public string ColorName
        {
            get { return _color; }
            set { _color = value; }
        }
        /// <summary>
        /// 编辑标示，表示是否可用
        /// </summary>
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        /// <summary>
        /// 编辑人员
        /// </summary>
        public string Editor
        {
            get { return _editor; }
            set { _editor = value; }
        }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public string EditTime
        {
            get { return _editTime; }
            set { _editTime = value; }
        }
   
        #endregion
    }
}

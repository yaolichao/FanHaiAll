//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-11-5             修改
// =================================================================================
using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using FanHai.Gui.Core;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Entities
{
    /// <summary>
    /// 工序实体类和工步实体类的基础类。
    /// </summary>
    public abstract class Osbase : EntityObject
    {
        /// <summary>
        /// 主键。
        /// </summary>
        protected string _operationVerKey = string.Empty;
        /// <summary>
        /// 不良原因代码组主键。
        /// </summary>
        protected string _defectCodesKey = string.Empty;
        /// <summary>
        /// 报废原因代码组主键。
        /// </summary>
        protected string _scrapCodesKey = string.Empty;
        /// <summary>
        /// 工序名称。
        /// </summary>
        protected string _operationName = string.Empty;
        /// <summary>
        /// 报废原因代码组名称。
        /// </summary>
        protected string _scrapCodesName = string.Empty;
        /// <summary>
        /// 不良原因代码组名称。
        /// </summary>
        protected string _defectCodesName = string.Empty;
        /// <summary>
        /// 工序版本号。
        /// </summary>
        protected string _operationVersion = string.Empty;
        /// <summary>
        /// 描述。
        /// </summary>
        protected string _osDescription = string.Empty;
        /// <summary>
        /// 时长。
        /// </summary>
        protected string _osDuration = string.Empty;
        /// <summary>
        /// 工序参数的排列顺序。
        /// </summary>
        protected OperationParamOrderType _paramOrderType = OperationParamOrderType.FirstRow;
        /// <summary>
        /// 工序参数的每行个数。
        /// </summary>
        protected int _paramCountPerRow = 2;
        /// <summary>
        /// 存放参数的数据表对象。
        /// </summary>
        protected DataTable _params = null;
        /// <summary>
        /// 操作类型。
        /// </summary>
        protected OperationAction _operationAction = OperationAction.None;
        /// <summary>
        /// 报废原因代码组主键。
        /// </summary>
        public string ScrapCodesKey
        {
            get 
            { 
                return _scrapCodesKey; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_SCRAP_REASON_CODE_CATEGORY_KEY, value);
                _scrapCodesKey = value; 
            }
        }
        /// <summary>
        /// 不良原因代码组主键。
        /// </summary>
        public string DefectCodesKey
        {
            get { return _defectCodesKey; }
            set 
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DEFECT_REASON_CODE_CATEGORY_KEY, value);
                _defectCodesKey = value; 
            }
        }
        /// <summary>
        /// 主键。
        /// </summary>
        public string OperationVerKey
        {
            get 
            { 
                return _operationVerKey; 
            }
            set 
            {
                _operationVerKey = value; 
            }
        }
        /// <summary>
        /// 描述。
        /// </summary>
        public string OsDescription
        {
            get 
            { 
                return _osDescription; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DESCRIPTIONS, value);
                _osDescription = value; 
            }
        }
        /// <summary>
        /// 时长。
        /// </summary>
        public string OsDuration
        {
            get 
            { 
                return _osDuration; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_DURATION, value);
                _osDuration = value; 
            }
        }
        /// <summary>
        /// 名称。
        /// </summary>
        public string OperationName
        {
            get 
            { 
                return _operationName; 
            }
            set 
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_NAME, value);
                _operationName = value; 
            }
        }
        /// <summary>
        /// 参数排列顺序。
        /// </summary>
        public OperationParamOrderType ParamOrderType
        {
            get
            {
                return _paramOrderType;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_ORDER_TYPE, Convert.ToInt32(value).ToString());
                _paramOrderType = value;
            }
        }
        /// <summary>
        /// 每行参数个数。
        /// </summary>
        public int ParamCountPerRow
        {
            get
            {
                return _paramCountPerRow;
            }
            set
            {
                ValidateDirtyList(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_PARAM_COUNT_PER_ROW, value.ToString());
                _paramCountPerRow = value;
            }
        }
        /// <summary>
        /// 参数。
        /// </summary>
        public DataTable Params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }
        /// <summary>
        /// 报废原因代码组名称。
        /// </summary>
        public string ScrapCodesName
        {
            get { return _scrapCodesName; }
            set { _scrapCodesName = value; }
        }
        /// <summary>
        /// 不良原因代码组名称。
        /// </summary>
        public string DefectCodesName
        {
            get { return _defectCodesName; }
            set { _defectCodesName = value; }
        }
        /// <summary>
        /// 操作类型。
        /// </summary>
        public OperationAction OperationAction
        {
            get
            {
                return _operationAction;
            }
            set
            {
                _operationAction = value;
            }
        }

        /// <summary>
        /// 处理自定义属性表，将参数名称转换参数分组主键。
        /// </summary>
        /// <param name="dtUDA">自定义属性表。</param>
        /// <param name="symBol">I：查询条件为EDC_NAME。 O(字母)：查询条件为EDC_KEY。</param>
        protected void DealUdaTable(DataTable dtUDA, string symBol)
        {
            foreach (DataRow drUDA in dtUDA.Rows)
            {
                if (drUDA[BASE_ATTRIBUTE_FIELDS.FIELDS_ATTRIBUTE_NAME].ToString() == COMMON_NAMES.LINKED_ITEM_EDC)
                {
                    drUDA[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE] = ConvertEdcKeyOrName(drUDA[COMMON_FIELDS.FIELD_COMMON_ATTRIBUTE_VALUE], symBol);
                }
            }
            dtUDA.AcceptChanges();
        }
        /// <summary>
        /// 参数分组名称和参数分组主键相互转换。
        /// </summary>
        /// <param name="inputParam">查询条件（EDC_NAME或EDC_KEY）。</param>
        /// <param name="symBol">I：查询条件为EDC_NAME。 O(字母)：查询条件为EDC_KEY。</param>
        /// <returns>
        /// I：返回EDC_KEY。 O(字母)：返回EDC_NAME。
        /// </returns>
        protected string ConvertEdcKeyOrName(object inputParam, string symBol)
        {
            try
            {

                IServerObjFactory factory = CallRemotingService.GetRemoteObject();
                if (null != factory)
                {
                    string strResult = factory.CreateIEDCEngine().ConvertEdcKeyOrName(inputParam.ToString(), symBol);

                    if (strResult != string.Empty)
                    {
                        return strResult;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex);
            }

            return string.Empty;
        }

    }
}

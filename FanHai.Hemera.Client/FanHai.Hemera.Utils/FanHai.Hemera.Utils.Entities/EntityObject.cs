//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// 冯旭                 2012-02-10            添加注释 
// =================================================================================
using System;
using System.Collections.Generic;
using System.Text;

using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;

namespace FanHai.Hemera.Utils.Entities
{
    public class EntityObject
    {
        #region Constructor
        public EntityObject()
        {
        
        }

        public EntityObject(EntityStatus status)
        {
            _entityStatus = status;
        }
        #endregion

        #region Public Functions
        #region 1. Actions
        public virtual bool UpdateStatus()
        {
            return false;
        }
        public virtual bool Insert()
        {
            return false;
        }
        public virtual bool Update()
        {
            return false;
        }
        public virtual bool Delete()
        {
            return false;
        }
        public virtual bool Update(string lblCode)
        {
            return false;
        }
        #endregion //Actions

        #region 2. Properties
        public virtual EntityStatus Status
        {
            get
            {
                return _entityStatus;
            }
            set
            {
                _entityStatus = value;
            }
        }
        public string StatusStr
        {
            get
            {
                return Convert.ToInt32(_entityStatus).ToString();
            }
        }
        public virtual bool IsDirty
        {
            get
            {
                return (_dirtyList.Count > 0);
            }
        }
        public Dictionary<string, DirtyItem> DirtyList
        {
            get
            {
                return _dirtyList;
            }
            set
            {
                _dirtyList = value;
            }
        }
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        public string CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public string CreateTimeZone
        {
            get { return _createTimeZone; }
            set { _createTimeZone = value; }
        }
        public string Editor
        {
            get { return _editor; }
            set
            {
                ValidateDirtyList(EDITOR, value);
                _editor = value;
            }
        }
        public string EditTime
        {
            get { return _editTime; }
            set
            {
                ValidateDirtyList(EDIT_TIME, value);
                _editTime = value;
            }
        }
        public string EditTimeZone
        {
            get { return _editTimeZone; }
            set
            {
                ValidateDirtyList(EDIT_TIMEZONE, value);
                _editTimeZone = value;
            }
        }

        public bool IsInitializeFinished
        {
            get { return _isInitializeFinished; }
            set
            {
                _isInitializeFinished = value;
            }
        }

        #endregion //Properties
        #endregion // Public Functions

        #region Protected Functions
        /// <summary>
        /// 验证数据是否一致，添加数据到数据集合中，不一致的数据同时添加到数据不一致的数据集合中。
        /// </summary>
        /// <param name="key">数据表字段</param>
        /// <param name="newValue">字段新值</param>
        protected virtual void ValidateDirtyList(string key, string newValue)
        {
            bool IsExist = false;
            DirtyItem dItem = new DirtyItem();
            //数据集合中包含该字段的记录。
            if (_dataList.ContainsKey(key))
            {
                IsExist = true;
                dItem = _dataList[key];
            }

            dItem.FieldName = key;
            //没有初始化完成。
            if (_isInitializeFinished == false)
            {
                dItem.FieldOriginalValue = newValue;
            }
            dItem.FieldNewValue = newValue;
            //如果不存在，添加到数据集合中。
            if (!IsExist)
            {
                _dataList.Add(key, dItem);
            }
            //初始化完成。
            if (_isInitializeFinished == true)
            {
                //字段新值和字段旧值不一致。
                if (dItem.FieldOriginalValue != dItem.FieldNewValue)
                {
                    //将数据添加到不一致的数据集合中。
                    if (_dirtyList.ContainsKey(key))
                    {
                        _dirtyList[key] = dItem;
                    }
                    else
                    {
                        _dirtyList.Add(key, dItem);
                    }
                }
                else//字段新值和字段旧值一致。
                {
                    //如果数据不一致的集合中包含该字段，则移除该字段。
                    if (_dirtyList.ContainsKey(key))
                    {
                        _dirtyList.Remove(key);
                    }
                }
            }
        }


        /// <summary>
        /// 重置数据集合，使用新数据替换原始数据值。
        /// </summary>
        public void ResetDirtyList()
        {
            foreach (string key in _dataList.Keys)
            {
                _dataList[key].FieldOriginalValue = _dataList[key].FieldNewValue;
            }
            _dirtyList.Clear();
            _isInitializeFinished = true;
        }

        public void ClearData()
        {
            _dataList.Clear();
            _dirtyList.Clear();
            _isInitializeFinished = false;
        }

        #endregion

        #region Private Functions
        #endregion //Private Functions

        #region 1 .Public Variables / Consts Definitions
        public const string EDITOR          = "EDITOR";
        public const string EDIT_TIME       = "EDIT_TIME";
        public const string EDIT_TIMEZONE   = "EDIT_TIMEZONE";
        #endregion //Public Variables / Consts Definitions

        #region 2. Protected Variables / Consts Definitions
        #endregion //Protected Variables / Consts Definitions

        #region 3. Private Variables / Consts Definitions
        private EntityStatus _entityStatus = EntityStatus.InActive;
        private string _creator         = string.Empty;
        private string _createTime      = string.Empty;
        private string _createTimeZone  = string.Empty;
        private string _editor          = string.Empty;
        private string _editTime      = string.Empty;
        private string _editTimeZone  = string.Empty;
        private bool _isInitializeFinished = false;

        private Dictionary<string, DirtyItem> _dirtyList = new Dictionary<string, DirtyItem>();
        private Dictionary<string, DirtyItem> _dataList = new Dictionary<string, DirtyItem>();
        #endregion //Private Variables / Consts Definitions
    }
}

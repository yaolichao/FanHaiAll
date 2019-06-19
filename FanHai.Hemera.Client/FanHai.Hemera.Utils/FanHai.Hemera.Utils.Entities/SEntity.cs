/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils.Common;
#endregion

namespace FanHai.Hemera.Utils.Entities
{
    //public delegate void UIDataUpdated();
    //public delegate void UIDataCollected();

    public class SEntity
    {
        public const string EDITOR = "EDITOR";
        public const string EDIT_TIME = "EDIT_TIME";
        public const string EDIT_TIMEZONE = "EDIT_TIMEZONE";

        /*
        private event UIDataUpdated _OnUIDataUpdated;
        private event UIDataUpdated _OnUIDataCollected;

        public UIDataUpdated OnUIDataUpdated
        {
            get { return _OnUIDataUpdated; }
            set { _OnUIDataUpdated = value; }
        }

        public UIDataCollected OnUIDataCollected
        {
            get { return OnUIDataCollected; }
            set { OnUIDataCollected = value; }
        }*/

        public virtual string Status
        {
            get;
            set;
        }

        #region Virtual Methods
        public virtual bool UpdateStatus()
        {
            return false;
        }

        public virtual bool Update()
        {
            return false;
        }

        public virtual bool Insert()
        {
            return false;
        }
        /// <summary>
        /// 并发新增
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public virtual bool Insert(DataSet ds)
        {
            return false;
        }

        public virtual bool Load()
        {
            return false;
        }

        public virtual bool Delete()
        {
            return false;
        }
        #endregion

        #region Private function/method for validate dirty data
        /// <summary>
        /// Validate dirty data
        /// </summary>
        /// <param name="key">Table fields</param>
        /// <param name="newValue">New value</param>
        protected  virtual void ValidateDirtyList(string key, string newValue)
        {
            bool IsExist = false;
            DirtyItem dItem = new DirtyItem();

            if (_dataList.ContainsKey(key))
            {
                IsExist = true;
                dItem = _dataList[key];
            }

            dItem.FieldName = key;
            if (_isInitializeFinished == false)
            {
                dItem.FieldOriginalValue = newValue;
            }
            dItem.FieldNewValue = newValue;

            if (!IsExist)
            {
                _dataList.Add(key, dItem);
            }

            if (_isInitializeFinished == true)
            {
                if (dItem.FieldOriginalValue != dItem.FieldNewValue)
                {
                    if (_dirtyList.ContainsKey(key))
                    {
                        _dirtyList[key] = dItem;
                    }
                    else
                    {
                        _dirtyList.Add(key, dItem);
                    }
                }
                else
                {
                    if (_dirtyList.ContainsKey(key))
                    {
                        _dirtyList.Remove(key);
                    }
                }
            }
        }
        #endregion

        #region basic Property

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

        //public Dictionary<string, DirtyItem> DataList
        //{
        //    get
        //    {
        //        return _dataList;
        //    }
        //    set
        //    {
        //        _dataList = value;
        //    }
        //}


        public virtual bool IsDirty
        {
            get
            {
                return (_dirtyList.Count > 0);
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
        #endregion

        private string _creator = string.Empty;
        private string _createTime = string.Empty;
        private string _createTimeZone = string.Empty;
        private string _editor = string.Empty;
        private string _editTime = string.Empty;
        private string _editTimeZone = string.Empty;
        private bool _isInitializeFinished = false;

        private Dictionary<string, DirtyItem> _dirtyList = new Dictionary<string, DirtyItem>();
        private Dictionary<string, DirtyItem> _dataList = new Dictionary<string, DirtyItem>();
        //private Dictionary<string, string> _originalList = new Dictionary<string, string>();

    }

  
    

   

    
}

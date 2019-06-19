//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 包含控制工作台启动的数据设置类，工作台如何启动属性包含在该类中。
//----------------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// 该类包含了用来控制工作台启动时的属性。
    /// </summary>
    [Serializable]
    public sealed class WorkbenchSettings
    {
        bool runOnNewThread = true;
        Collection<string> fileList = new Collection<string>();

        /// <summary>
        /// 获取或设置是否创建一个新的线程运行工作台。
        /// 默认设置为true。
        /// </summary>
        public bool RunOnNewThread
        {
            get
            {
                return runOnNewThread;
            }
            set
            {
                runOnNewThread = value;
            }
        }

        /// <summary>
        /// 存放工作台启动时需要打开文件的列表集合。
        /// </summary>
        public Collection<string> InitialFileList
        {
            get
            {
                return fileList;
            }
        }
    }
}

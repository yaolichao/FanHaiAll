//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件包含服务管理类，该类管理日志服务对象和消息服务对象。
//----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FanHai.Gui.Core.Services
{
    /// <summary>
    /// 核心服务管理类。包含对日志服务类对象的获取和设置，对消息服务类对象的获取和设置。
    /// </summary>
    public static class ServiceManager
    {
        static ILoggingService loggingService = new TextWriterLoggingService(new DebugTextWriter());
        /// <summary>
        /// 获取或设置日志服务类对象
        /// </summary>
        public static ILoggingService LoggingService
        {
            get { return loggingService; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                loggingService = value;
            }
        }

        static IMessageService messageService = new TextWriterMessageService(Console.Out);
        /// <summary>
        /// 获取或设置消息服务类对象。
        /// </summary>
        public static IMessageService MessageService
        {
            get { return messageService; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                messageService = value;
            }
        }
    }
}

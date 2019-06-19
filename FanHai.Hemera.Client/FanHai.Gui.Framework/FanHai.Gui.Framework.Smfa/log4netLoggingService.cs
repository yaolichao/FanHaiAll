//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 包含使用log4net记录日志的日志服务类。
//----------------------------------------------------------------------------------
using System;
using System.IO;
using FanHai.Gui.Core.Services;
using log4net;
using log4net.Config;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// 使用log4net记录日志的日志服务类，实现<see cref="ILoggingService"/>接口。
    /// </summary>
    sealed class log4netLoggingService : ILoggingService
    {
        ILog log;

        /// <summary>
        /// 构造函数
        /// </summary>
        public log4netLoggingService()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            log = LogManager.GetLogger(typeof(log4netLoggingService));
        }

        /// <summary>
        /// 记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        public void Debug(object message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 使用指定格式记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        /// <summary>
        /// 记录INFO级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 使用指定格式记录INFO级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Warn(object message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        /// <summary>
        /// 使用指定格式记录WARN级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Error(object message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        /// <summary>
        /// 使用指定格式记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        /// <summary>
        /// 使用指定格式记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        /// <summary>
        /// 获取是否启用了DEBUG级别的日志记录
        /// </summary>
        public bool IsDebugEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        /// <summary>
        /// 获取是否启用了INFO级别的日志记录
        /// </summary>
        public bool IsInfoEnabled
        {
            get
            {
                return log.IsInfoEnabled;
            }
        }

        /// <summary>
        /// 获取是否启用了WARN级别的日志记录
        /// </summary>
        public bool IsWarnEnabled
        {
            get
            {
                return log.IsWarnEnabled;
            }
        }

        /// <summary>
        /// 获取是否启用了ERROR级别的日志记录
        /// </summary>
        public bool IsErrorEnabled
        {
            get
            {
                return log.IsErrorEnabled;
            }
        }

        /// <summary>
        /// 获取是否启用了FATAL级别的日志记录
        /// </summary>
        public bool IsFatalEnabled
        {
            get
            {
                return log.IsFatalEnabled;
            }
        }

        /// <summary>
        /// 关闭当前日志记录，清理当前日志记录对象。
        /// </summary>
        public void CloseCmd()
        {

        }
    }
}

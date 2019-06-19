//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 包含方便日志记录的服务类，该服务类通过调用实现了ILoggingService接口的具
//          体类对象来记录日志。
//----------------------------------------------------------------------------------
using System;
using FanHai.Gui.Core.Services;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 记录日志消息的服务类。
    /// </summary>
    public static class LoggingService
    {

        /// <summary>
        /// 记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        public static void Debug(object message)
        {
            ServiceManager.LoggingService.Debug(message);
        }

        /// <summary>
        /// 使用指定格式记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public static void DebugFormatted(string format, params object[] args)
        {
            ServiceManager.LoggingService.DebugFormatted(format, args);
        }

        /// <summary>
        /// 记录INFO级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public static void Info(object message)
        {
            ServiceManager.LoggingService.Info(message);
        }
        /// <summary>
        /// 使用指定格式记录INFO级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public static void InfoFormatted(string format, params object[] args)
        {
            ServiceManager.LoggingService.InfoFormatted(format, args);
        }
        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public static void Warn(object message)
        {
            ServiceManager.LoggingService.Warn(message);
        }

        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public static void Warn(object message, Exception exception)
        {
            ServiceManager.LoggingService.Warn(message, exception);
        }
        /// <summary>
        /// 使用指定格式记录WARN级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public static void WarnFormatted(string format, params object[] args)
        {
            ServiceManager.LoggingService.WarnFormatted(format, args);
        }
        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public static void Error(object message)
        {
            ServiceManager.LoggingService.Error(message);
        }
        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public static void Error(object message, Exception exception)
        {
            ServiceManager.LoggingService.Error(message, exception);
        }
        /// <summary>
        /// 使用指定格式记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public static void ErrorFormatted(string format, params object[] args)
        {
            ServiceManager.LoggingService.ErrorFormatted(format, args);
        }
        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public static void Fatal(object message)
        {
            ServiceManager.LoggingService.Fatal(message);
        }
        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public static void Fatal(object message, Exception exception)
        {
            ServiceManager.LoggingService.Fatal(message, exception);
        }
        /// <summary>
        /// 使用指定格式记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public static void FatalFormatted(string format, params object[] args)
        {
            ServiceManager.LoggingService.FatalFormatted(format, args);
        }
        /// <summary>
        /// 获取是否启用了DEBUG级别的日志记录
        /// </summary>
        public static bool IsDebugEnabled
        {
            get
            {
                return ServiceManager.LoggingService.IsDebugEnabled;
            }
        }
        /// <summary>
        /// 获取是否启用了INFO级别的日志记录
        /// </summary>
        public static bool IsInfoEnabled
        {
            get
            {
                return ServiceManager.LoggingService.IsInfoEnabled;
            }
        }
        /// <summary>
        /// 获取是否启用了WARN级别的日志记录
        /// </summary>
        public static bool IsWarnEnabled
        {
            get
            {
                return ServiceManager.LoggingService.IsWarnEnabled;
            }
        }
        /// <summary>
        /// 获取是否启用了ERROR级别的日志记录
        /// </summary>
        public static bool IsErrorEnabled
        {
            get
            {
                return ServiceManager.LoggingService.IsErrorEnabled;
            }
        }
        /// <summary>
        /// 获取是否启用了FATAL级别的日志记录
        /// </summary>
        public static bool IsFatalEnabled
        {
            get
            {
                return ServiceManager.LoggingService.IsFatalEnabled;
            }
        }
        /// <summary>
        /// 关闭当前日志记录，清理当前日志记录对象。
        /// </summary>
        public static void CloseCmd()
        {

            ServiceManager.LoggingService.CloseCmd();

        }
    }

}

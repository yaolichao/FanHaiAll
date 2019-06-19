//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件包含日志记录服务接口，该接口抽象出日志记录所需要的方法。
//----------------------------------------------------------------------------------
using System;

namespace FanHai.Gui.Core.Services
{
    /// <summary>
    /// 日志记录服务接口。
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// 记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        void Debug(object message);
        /// <summary>
        /// 使用指定格式记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        void DebugFormatted(string format, params object[] args);
        /// <summary>
        /// 记录INFO级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        void Info(object message);
        /// <summary>
        /// 使用指定格式记录INFO级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        void InfoFormatted(string format, params object[] args);
        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        void Warn(object message);
        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        void Warn(object message, Exception exception);
        /// <summary>
        /// 使用指定格式记录WARN级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        void WarnFormatted(string format, params object[] args);
        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        void Error(object message);
        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        void Error(object message, Exception exception);
        /// <summary>
        /// 使用指定格式记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        void ErrorFormatted(string format, params object[] args);
        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        void Fatal(object message);
        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        void Fatal(object message, Exception exception);
        /// <summary>
        /// 使用指定格式记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        void FatalFormatted(string format, params object[] args);
        /// <summary>
        /// 获取是否启用了DEBUG级别的日志记录
        /// </summary>
        bool IsDebugEnabled { get; }
        /// <summary>
        /// 获取是否启用了INFO级别的日志记录
        /// </summary>
        bool IsInfoEnabled { get; }
        /// <summary>
        /// 获取是否启用了WARN级别的日志记录
        /// </summary>
        bool IsWarnEnabled { get; }
        /// <summary>
        /// 获取是否启用了ERROR级别的日志记录
        /// </summary>
        bool IsErrorEnabled { get; }
        /// <summary>
        /// 获取是否启用了FATAL级别的日志记录
        /// </summary>
        bool IsFatalEnabled { get; }
        /// <summary>
        /// 关闭当前日志记录，清理当前日志记录对象。
        /// </summary>
        void CloseCmd();
    }
}

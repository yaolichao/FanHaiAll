//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件包含了实现ILoggingService接口的日志服务类，该类使用文本流的方式输出日志信息。
//----------------------------------------------------------------------------------
using System;
using System.IO;

namespace FanHai.Gui.Core.Services
{
    /// <summary>
    /// 使用文本流的方式输出日志信息的日志服务类，该类型实现ILoggingService接口。
    /// </summary>
    public class TextWriterLoggingService : ILoggingService
    {
        readonly TextWriter writer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/>对象。</param>
        public TextWriterLoggingService(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            this.writer = writer;
            this.IsFatalEnabled = true;
            this.IsErrorEnabled = true;
            this.IsWarnEnabled = true;
            this.IsInfoEnabled = true;
            this.IsDebugEnabled = true;
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Write(object message, Exception exception)
        {
            if (message != null)
            {
                writer.WriteLine(message.ToString());
            }
            if (exception != null)
            {
                writer.WriteLine(exception.ToString());
            }
        }

        /// <summary>
        /// 获取是否启用了DEBUG级别的日志记录
        /// </summary>
        public bool IsDebugEnabled { get; set; }
        /// <summary>
        /// 获取是否启用了INFO级别的日志记录
        /// </summary>
        public bool IsInfoEnabled { get; set; }
        /// <summary>
        /// 获取是否启用了WARN级别的日志记录
        /// </summary>
        public bool IsWarnEnabled { get; set; }
        /// <summary>
        /// 获取是否启用了ERROR级别的日志记录
        /// </summary>
        public bool IsErrorEnabled { get; set; }
        /// <summary>
        /// 获取是否启用了FATAL级别的日志记录
        /// </summary>
        public bool IsFatalEnabled { get; set; }

        /// <summary>
        /// 记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        public void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                Write(message, null);
            }
        }

        /// <summary>
        /// 使用指定格式记录DEBUG级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void DebugFormatted(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        /// <summary>
        /// 记录INFO级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Info(object message)
        {
            if (IsInfoEnabled)
            {
                Write(message, null);
            }
        }

        /// <summary>
        /// 使用指定格式记录INFO级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void InfoFormatted(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Warn(object message)
        {
            Warn(message, null);
        }

        /// <summary>
        /// 记录WARN级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                Write(message, exception);
            }
        }

        /// <summary>
        /// 使用指定格式记录WARN级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void WarnFormatted(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Error(object message)
        {
            Error(message, null);
        }

        /// <summary>
        /// 记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(message, exception);
            }
        }

        /// <summary>
        /// 使用指定格式记录ERROR级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void ErrorFormatted(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象.</param>
        public void Fatal(object message)
        {
            Fatal(message, null);
        }

        /// <summary>
        /// 记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="exception">异常对象。</param>
        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(message, exception);
            }
        }

        /// <summary>
        /// 使用指定格式记录FATAL级别的日志消息。
        /// </summary>
        /// <param name="format">格式化字符串。例如："{0}类中的{1}属性必须是字符串类型。"。</param>
        /// <param name="args">消息参数。</param>
        public void FatalFormatted(string format, params object[] args)
        {
            Fatal(string.Format(format, args));
        }

        /// <summary>
        /// 关闭当前日志，清理当前日志记录对象。
        /// </summary>
        public void CloseCmd()
        {
            writer.Close();
            writer.Dispose();
        }
    }
}

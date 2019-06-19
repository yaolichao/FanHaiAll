//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件包含文本流消息服务类，实现IMessageService接口。用于将消息使用文本流的方式输出。
//----------------------------------------------------------------------------------
using System;
using System.IO;

namespace FanHai.Gui.Core.Services
{
    /// <summary>
    /// 文本流消息服务类，实现<see cref="IMessageService"/>接口。
    /// </summary>
    public class TextWriterMessageService : IMessageService
    {
        readonly TextWriter writer;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/>对象。</param>
        public TextWriterMessageService(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            this.writer = writer;
        }

        /// <summary>
        /// 显示错误消息对话框。
        /// 如果<paramref name="ex"/> 为空, 使用消息框显示<paramref name="message"/>字符串。
        /// 否则，显示异常错误。
        /// </summary>
        /// <param name="ex">异常对象。</param>
        /// <param name="message">消息字符串。</param>
        public void ShowError(Exception ex, string message)
        {
            if (message != null)
            {
                writer.WriteLine(message);
            }
            if (ex != null)
            {
                writer.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 显示警告消息对话框。
        /// </summary>
        /// <param name="message"></param>
        public void ShowWarning(string message)
        {
            writer.WriteLine(message);
        }

        /// <summary>
        /// 询问用户“是/否”的问题对话框。默认按钮为“是”。
        /// </summary>
        /// <param name="question">问题字符串。</param>
        /// <param name="caption">标题字符串。</param>
        /// <returns>
        /// 如果“是”被点击返回<c>true</c>，否则返回<c>false</c>。
        /// </returns>
        public bool AskQuestion(string question, string caption)
        {
            writer.WriteLine(caption + ": " + question);
            return false;
        }

        /// <summary>
        /// 询问用户“是/否”的问题对话框。默认按钮为“否”。
        /// </summary>
        /// <param name="question">问题字符串。</param>
        /// <param name="caption">标题字符串。</param>
        /// <returns>
        /// 如果“是”被点击返回<c>true</c>，否则返回<c>false</c>。
        /// </returns>
        public bool AskQuestionSpecifyNoButton(string question, string caption)
        {
            writer.WriteLine(caption + ": " + question);
            return false;
        }

        /// <summary>
        /// 显示客户对话框。
        /// </summary>
        /// <param name="caption">对话框标题</param>
        /// <param name="dialogText">对话框文本描述.</param>
        /// <param name="acceptButtonIndex">默认“同意”按钮的索引值，如果你不想有一个“取消”按钮，则使用-1</param>
        /// <param name="cancelButtonIndex">默认“取消”按钮的索引值，如果你不想有一个“取消”按钮，则使用-1</param>
        /// <param name="buttontexts">按钮的文本值.</param>
        /// <returns>
        /// 返回点击文本的索引值, 如果没使用点击按钮关闭，返回-1。
        /// </returns>
        public int ShowCustomDialog(string caption, string dialogText, int acceptButtonIndex, int cancelButtonIndex, params string[] buttontexts)
        {
            writer.WriteLine(caption + ": " + dialogText);
            return cancelButtonIndex;
        }

        /// <summary>
        /// 显示输入对话框。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="dialogText">对话框文本</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>
        /// 输入的值。
        /// </returns>
        public string ShowInputBox(string caption, string dialogText, string defaultValue)
        {
            writer.WriteLine(caption + ": " + dialogText);
            return defaultValue;
        }

        /// <summary>
        /// 显示一个消息对话框。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="caption">标题。</param>
        public void ShowMessage(string message, string caption)
        {
            writer.WriteLine(caption + ": " + message);
        }

        /// <summary>
        /// 显示保存错误消息的对话框。
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="message">消息</param>
        /// <param name="dialogName">对话框名</param>
        /// <param name="exceptionGot">异常对象</param>
        public void InformSaveError(string fileName, string message, string dialogName, Exception exceptionGot)
        {
            writer.WriteLine(dialogName + ": " + message + " (" + fileName + ")");
            if (exceptionGot != null)
                writer.WriteLine(exceptionGot.ToString());
        }

        /// <summary>
        /// 显示选择保存错误消息的对话框。允许用户使用替换的名称重试/保存。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="message">消息。</param>
        /// <param name="dialogName">对话框名。</param>
        /// <param name="exceptionGot">异常对象。</param>
        /// <param name="chooseLocationEnabled">启用选择其他位置。</param>
        /// <returns>
        /// 返回<see cref="ChooseSaveErrorResult"/>对象。
        /// </returns>
        public ChooseSaveErrorResult ChooseSaveError(string fileName, string message, string dialogName, Exception exceptionGot, bool chooseLocationEnabled)
        {
            writer.WriteLine(dialogName + ": " + message + " (" + fileName + ")");
            if (exceptionGot != null)
                writer.WriteLine(exceptionGot.ToString());
            return ChooseSaveErrorResult.Ignore;
        }
    }
}

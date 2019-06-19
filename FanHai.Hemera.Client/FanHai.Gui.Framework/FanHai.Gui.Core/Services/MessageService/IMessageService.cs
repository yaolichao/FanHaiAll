//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件包含消息服务接口，该服务接口提供用来显示各类消息的方法。
//----------------------------------------------------------------------------------
using System;

namespace FanHai.Gui.Core.Services
{
    /// <summary>
    /// 消息服务接口。
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 显示错误消息对话框。
        /// 如果<paramref name="ex"/> 为空, 使用消息框显示<paramref name="message"/>字符串。
        /// 否则，显示异常错误。
        /// </summary>
        /// <param name="ex">异常对象。</param>
        /// <param name="message">消息字符串。</param>
        void ShowError(Exception ex, string message);

        /// <summary>
        /// 显示警告消息对话框。
        /// </summary>
        void ShowWarning(string message);

        /// <summary>
        /// 询问用户“是/否”的问题对话框。默认按钮为“是”。
        /// </summary>
        /// <param name="question">问题字符串。</param>
        /// <param name="caption">标题字符串。</param>
        /// <returns>如果“是”被点击返回<c>true</c>，否则返回<c>false</c>。</returns>
        bool AskQuestion(string question, string caption);

        /// <summary>
        /// 询问用户“是/否”的问题对话框。默认按钮为“否”。
        /// </summary>
        /// <param name="question">问题字符串。</param>
        /// <param name="caption">标题字符串。</param>
        /// <returns>如果“是”被点击返回<c>true</c>，否则返回<c>false</c>。</returns>
        bool AskQuestionSpecifyNoButton(string question, string caption);

        /// <summary>
        /// 显示客户对话框。
        /// </summary>
        /// <param name="caption">对话框标题</param>
        /// <param name="dialogText">对话框文本描述.</param>
        /// <param name="acceptButtonIndex">
        /// 默认“同意”按钮的索引值，如果你不想有一个“取消”按钮，则使用-1
        /// </param>
        /// <param name="cancelButtonIndex">
        /// 默认“取消”按钮的索引值，如果你不想有一个“取消”按钮，则使用-1
        /// </param>
        /// <param name="buttontexts">按钮的文本值.</param>
        /// <returns>返回点击文本的索引值, 如果没使用点击按钮关闭，返回-1。</returns>
        int ShowCustomDialog(string caption, string dialogText, int acceptButtonIndex, int cancelButtonIndex, params string[] buttontexts);
        /// <summary>
        /// 显示输入对话框。
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="dialogText">对话框文本</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>输入的值。</returns>
        string ShowInputBox(string caption, string dialogText, string defaultValue);
        /// <summary>
        /// 显示一个消息对话框。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="caption">标题。</param>
        void ShowMessage(string message, string caption);

        /// <summary>
        /// 显示保存错误消息的对话框。
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="message">消息</param>
        /// <param name="dialogName">对话框名</param>
        /// <param name="exceptionGot">异常对象</param>
        void InformSaveError(string fileName, string message, string dialogName, Exception exceptionGot);

        /// <summary>
        /// 显示选择保存错误消息的对话框。允许用户使用替换的名称重试/保存。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="message">消息。</param>
        /// <param name="dialogName">对话框名。</param>
        /// <param name="exceptionGot">异常对象。</param>
        /// <param name="chooseLocationEnabled">启用选择其他位置。</param>
        /// <returns>返回<see cref="ChooseSaveErrorResult"/>对象。</returns>
        ChooseSaveErrorResult ChooseSaveError(string fileName, string message, string dialogName, Exception exceptionGot, bool chooseLocationEnabled);
    }
    /// <summary>
    /// 选择保存错误结果的类。
    /// </summary>
    public sealed class ChooseSaveErrorResult
    {
        /// <summary>
        ///  获取一个值指示是否重试。
        /// </summary>
        public bool IsRetry { get; private set; }
        /// <summary>
        /// 获取一个值指示是否忽略。
        /// </summary>
        public bool IsIgnore { get; private set; }
        /// <summary>
        /// 获取一个值指示是否另存为。
        /// </summary>
        public bool IsSaveAlternative { get { return AlternativeFileName != null; } }
        /// <summary>
        /// 获取另存为的文件名称。
        /// </summary>
        public string AlternativeFileName { get; private set; }
        /// <summary>
        /// ctor
        /// </summary>
        private ChooseSaveErrorResult() { }
        /// <summary>
        /// 返回“重试”选择保存错误结果的对象。
        /// </summary>
        public readonly static ChooseSaveErrorResult Retry = new ChooseSaveErrorResult { IsRetry = true };
        /// <summary>
        /// 返回“忽略”选择保存错误结果的对象。
        /// </summary>
        public readonly static ChooseSaveErrorResult Ignore = new ChooseSaveErrorResult { IsIgnore = true };
        /// <summary>
        /// 返回“另存为”选择保存错误结果的对象。
        /// </summary>
        /// <param name="alternativeFileName">另存为的文件名。</param>
        /// <returns>选择保存错误结果的对象。</returns>
        public static ChooseSaveErrorResult SaveAlternative(string alternativeFileName)
        {
            return new ChooseSaveErrorResult { AlternativeFileName = alternativeFileName };
        }
    }
}

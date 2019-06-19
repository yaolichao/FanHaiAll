//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// Author:  Peter.Zhang
// Content: 文件将字符写入到System.Diagnostics.Debug.Listeners集合中的文本写入类。
//----------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// 将字符写入到System.Diagnostics.Debug.Listeners集合中的文本写入类。
    /// </summary>
    public class DebugTextWriter : TextWriter
    {
        /// <summary>
        /// 当在派生类中重写时，返回用来写输出的 <see cref="T:System.Text.Encoding"></see>。
        /// </summary>
        /// <returns>用来写入输出的 Encoding。</returns>
        public override Encoding Encoding
        {
            get
            {
                return Encoding.Unicode;
            }
        }

        /// <summary>
        /// 将字符写入文本流。
        /// </summary>
        /// <param name="value">要写入文本流中的字符。</param>
        /// <exception cref="T:System.IO.IOException">发生 I/O 错误。 </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException"><see cref="T:System.IO.TextWriter"></see> 是关闭的。 </exception>
        public override void Write(char value)
        {
            Debug.Write(value.ToString());
        }

        /// <summary>
        /// 将字符的子数组写入文本流。
        /// </summary>
        /// <param name="buffer">要从中写出数据的字符数组。</param>
        /// <param name="index">在缓冲区中开始索引。</param>
        /// <param name="count">要写入的字符数。</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index 或 count 为负。 </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">发生 I/O 错误。 </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">缓冲区长度减去 index 小于 count。 </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">buffer 参数为 null。 </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException"><see cref="T:System.IO.TextWriter"></see> 是关闭的。 </exception>
        public override void Write(char[] buffer, int index, int count)
        {
            Debug.Write(new string(buffer, index, count));
        }

        /// <summary>
        /// 将字符串写入文本流。
        /// </summary>
        /// <param name="value">要写入的字符串。</param>
        /// <exception cref="T:System.IO.IOException">发生 I/O 错误。 </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException"><see cref="T:System.IO.TextWriter"></see> 是关闭的。 </exception>
        public override void Write(string value)
        {
            Debug.Write(value);
        }

        /// <summary>
        /// 将行结束符写入文本流。
        /// </summary>
        /// <exception cref="T:System.IO.IOException">发生 I/O 错误。 </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException"><see cref="T:System.IO.TextWriter"></see> 是关闭的。 </exception>
        public override void WriteLine()
        {
            Debug.WriteLine(string.Empty);
        }

        /// <summary>
        /// 将后跟行结束符的字符串写入文本流。
        /// </summary>
        /// <param name="value">要写入的字符串。如果 value 为 null，则仅写入行结束字符。</param>
        /// <exception cref="T:System.IO.IOException">发生 I/O 错误。 </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException"><see cref="T:System.IO.TextWriter"></see> 是关闭的。 </exception>
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }
    }
}

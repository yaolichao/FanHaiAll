using System;
using System.Runtime.Serialization;

namespace FanHai.Gui.Framework.Smfa
{
    /// <summary>
    /// Exception used when there's an unhandled exception running the workbench.
    /// </summary>
    [Serializable()]
    public class RunWorkbenchException : Exception
    {
        /// <summary>
        /// Create a new RunWorkbenchException instance.
        /// </summary>
        public RunWorkbenchException()
            : base()
        {
        }

        /// <summary>
        /// Create a new RunWorkbenchException instance.
        /// </summary>
        public RunWorkbenchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a new RunWorkbenchException instance.
        /// </summary>
        public RunWorkbenchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Create a new RunWorkbenchException instance.
        /// </summary>
        protected RunWorkbenchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// Is thrown when the GlobalResource manager can't find a requested
    /// resource.
    /// </summary>
    [Serializable()]
    public class ResourceNotFoundException : CoreException
    {
        public ResourceNotFoundException(string resource)
            : base("Resource not found : " + resource)
        {
        }

        public ResourceNotFoundException()
            : base()
        {
        }

        public ResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

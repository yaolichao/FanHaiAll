using System;
using System.Threading;

namespace FanHai.Gui.Core
{
    /// <summary>
    /// Invokes a callback when this class is disposed.
    /// </summary>
    sealed class CallbackOnDispose : IDisposable
    {
        // TODO: in 4.0, use System.Action and make this class public
        System.Threading.ThreadStart callback;

        public CallbackOnDispose(System.Threading.ThreadStart callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");
            this.callback = callback;
        }

        public void Dispose()
        {
            System.Threading.ThreadStart action = Interlocked.Exchange(ref callback, null);
            if (action != null)
                action();
        }
    }
}

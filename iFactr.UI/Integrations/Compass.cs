using System;
using System.Diagnostics;
using MonoCross.Navigation;

namespace iFactr.Integrations
{
    /// <summary>
    /// Represents a utility for accessing a device's compass.
    /// </summary>
    public sealed class Compass : ICompass
    {
        /// <summary>
        /// Occurs when the compass heading changes.
        /// </summary>
        public event EventHandler<HeadingEventArgs> HeadingUpdated
        {
            add { nativeCompass.HeadingUpdated += value; }
            remove { nativeCompass.HeadingUpdated -= value; }
        }

        /// <summary>
        /// Gets a value indicating whether the device's compass is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return nativeCompass.IsActive; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly ICompass nativeCompass;

        /// <summary>
        /// Initializes a new instance of the <see cref="Compass"/> class.
        /// </summary>
        public Compass()
        {
            nativeCompass = MXContainer.Resolve<ICompass>();
        }

        /// <summary>
        /// Begins tracking the compass heading of the device.
        /// </summary>
        public void Start()
        {
            nativeCompass.Start();
        }

        /// <summary>
        /// Stops tracking the compass heading of the device.
        /// </summary>
        public void Stop()
        {
            nativeCompass.Stop();
        }
    }
}

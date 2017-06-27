using System;
using System.Diagnostics;
using MonoCross.Navigation;

namespace iFactr.Integrations
{
    /// <summary>
    /// Represents a utility for providing real-time acceleration data.
    /// </summary>
    public sealed class Accelerometer : IAccelerometer
    {
        /// <summary>
        /// Occurs when the X, Y, or Z value of the accelerometer changes.
        /// </summary>
        public event EventHandler<AccelerometerEventArgs> ValuesUpdated
        {
            add { nativeAccel.ValuesUpdated += value; }
            remove { nativeAccel.ValuesUpdated -= value; }
        }

        /// <summary>
        /// Gets a value indicating whether the device's accelerometer is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return nativeAccel.IsActive; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly IAccelerometer nativeAccel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Compass"/> class.
        /// </summary>
        public Accelerometer()
        {
            nativeAccel = MXContainer.Resolve<IAccelerometer>();
        }

        /// <summary>
        /// Begins listening for acceleration changes.
        /// </summary>
        public void Start()
        {
            nativeAccel.Start();
        }

        /// <summary>
        /// Stops listening for acceleration changes.
        /// </summary>
        public void Stop()
        {
            nativeAccel.Stop();
        }
    }
}

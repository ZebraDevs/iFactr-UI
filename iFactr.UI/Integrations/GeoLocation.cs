using System;
using System.Diagnostics;
using MonoCross.Navigation;

namespace iFactr.Integrations
{
    /// <summary>
    /// Represents a utility for retrieving the location of a device via longitude and latitude coordinates.
    /// </summary>
    public sealed class GeoLocation : IGeoLocation
    {
        /// <summary>
        /// Occurs when the location of the device changes.
        /// </summary>
        public event EventHandler<GeoLocationEventArgs> LocationUpdated
        {
            add { nativeGeo.LocationUpdated += value; }
            remove { nativeGeo.LocationUpdated -= value; }
        }

        /// <summary>
        /// Gets a value indicating whether the device's GPS is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return nativeGeo.IsActive; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly IGeoLocation nativeGeo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocation"/> class.
        /// </summary>
        public GeoLocation()
        {
            nativeGeo = MXContainer.Resolve<IGeoLocation>();
        }

        /// <summary>
        /// Begins tracking the GPS coordinates of the device.
        /// </summary>
        public void Start()
        {
            nativeGeo.Start();
        }

        /// <summary>
        /// Stops tracking the GPS coordinates of the device.
        /// </summary>
        public void Stop()
        {
            nativeGeo.Stop();
        }
    }
}
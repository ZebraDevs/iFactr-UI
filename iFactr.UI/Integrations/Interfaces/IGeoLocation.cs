using System;

namespace iFactr.Integrations
{
    /// <summary>
    /// Defines a utility for retrieving the location of a device via longitude and latitude coordinates.
    /// </summary>
    public interface IGeoLocation
    {
        /// <summary>
        /// Gets a value indicating whether the device's GPS is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Occurs when the location of the device changes.
        /// </summary>
        event EventHandler<GeoLocationEventArgs> LocationUpdated;

        /// <summary>
        /// Begins tracking the GPS coordinates of the device.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops tracking the GPS coordinates of the device.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Represents a location based on longitude and latitude coordinates.
    /// </summary>
    public struct GeoLocationData
    {
        /// <summary>
        /// The latitude coordinate.  This field is read-only.
        /// </summary>
        public readonly double Latitude;

        /// <summary>
        /// The longitude coordinate.  This field is read-only.
        /// </summary>
        public readonly double Longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationData"/> structure.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        public GeoLocationData(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    /// <summary>
    /// Provides data for the geolocator's <see cref="E:LocationUpdated"/> event.
    /// </summary>
    public class GeoLocationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocationEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data for the event.</param>
        public GeoLocationEventArgs(GeoLocationData data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the location data for the event.
        /// </summary>
        public GeoLocationData Data { get; private set; }
    }
}

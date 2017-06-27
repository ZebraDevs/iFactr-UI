using System;

namespace iFactr.Integrations
{
    /// <summary>
    /// Represents a utility for accessing a device's compass.
    /// </summary>
    public interface ICompass
    {
        /// <summary>
        /// Gets a value indicating whether the device's compass is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Occurs when the compass heading changes.
        /// </summary>
        event EventHandler<HeadingEventArgs> HeadingUpdated;

        /// <summary>
        /// Begins tracking the compass heading of the device.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops tracking the compass heading of the device.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Represents a compass heading in degrees.
    /// </summary>
    public struct HeadingData
    {
        /// <summary>
        /// The magnetic heading in degrees.  This field is read-only.
        /// </summary>
        public readonly double MagneticHeading;

        /// <summary>
        /// The true heading in degrees.  This field is read-only.
        /// </summary>
        public readonly double TrueHeading;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadingData"/> structure.
        /// </summary>
        /// <param name="trueHeading">The true heading in degrees.</param>
        /// <param name="magneticHeading">The magnetic heading in degrees.</param>
        public HeadingData(double trueHeading, double magneticHeading)
        {
            TrueHeading = trueHeading;
            MagneticHeading = magneticHeading;
        }
    }

    /// <summary>
    /// Provides data for the compass's <see cref="E:HeadingUpdated"/> event.
    /// </summary>
    public class HeadingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeadingEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data for the event.</param>
        public HeadingEventArgs(HeadingData data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the heading data for the event.
        /// </summary>
        public HeadingData Data { get; private set; }
    }
}
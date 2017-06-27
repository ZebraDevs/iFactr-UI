using System;

namespace iFactr.Integrations
{
    /// <summary>
    /// Defines a utility for providing real-time acceleration data.
    /// </summary>
    public interface IAccelerometer
    {
        /// <summary>
        /// Gets a value indicating whether the device's accelerometer is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Occurs when the X, Y, or Z value of the accelerometer changes.
        /// </summary>
        event EventHandler<AccelerometerEventArgs> ValuesUpdated;

        /// <summary>
        /// Begins listening for acceleration changes.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops listening for acceleration changes.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Represents the X, Y, and Z values of an accelerometer.
    /// </summary>
    public struct AccelerometerData
    {
        /// <summary>
        /// The value of the X axis.  This field is read-only.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The value of the Y axis.  This field is read-only.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// The value of the Z axis.  This field is read-only.
        /// </summary>
        public readonly double Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerData"/> structure.
        /// </summary>
        /// <param name="x">The value of the X axis.</param>
        /// <param name="y">The value of the Y axis.</param>
        /// <param name="z">The value of the Z axis.</param>
        public AccelerometerData(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

    }

    /// <summary>
    /// Provides data for the accelerometer's <see cref="E:ValuesUpdated"/> event.
    /// </summary>
    public class AccelerometerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelerometerEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data for the event.</param>
        public AccelerometerEventArgs(AccelerometerData data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the acceleration data for the event.
        /// </summary>
        public AccelerometerData Data { get; private set; }
    }
}

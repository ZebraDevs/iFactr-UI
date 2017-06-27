using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a timer able to run across multiple platforms.
    /// </summary>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the timer is running.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, before the <see cref="Elapsed"/> event is fired.
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        /// Occurs when the amount of time specified by the <see cref="Interval"/> property has passed.
        /// </summary>
        event EventHandler Elapsed;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        void Stop();
    }
}

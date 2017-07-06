using System;

namespace iFactr.UI
{
    /// <summary>
    /// Provides data for events that can be used to intercept system behavior.
    /// </summary>
    public class EventHandledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether the event should be considered handled.  A value of <c>true</c> means that the event
        /// has been handled and that the system should not proceed with any default behavior associated with the event.
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandledEventArgs"/> class.
        /// </summary>
        public EventHandledEventArgs()
        {
        }
    }
}

using System;

namespace iFactr.UI
{
    /// <summary>
    /// Indicates that the handlers for the event are stored in a location other than the default.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
	public sealed class EventDelegateAttribute : Attribute
	{
        /// <summary>
        /// Gets the name of the delegate that contains the invocation list for the event.
        /// </summary>
		public string DelegateName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.EventDelegateAttribute"/> class.
        /// </summary>
        /// <param name="delegateName">The name of the delegate that contains the invocation list for the event.</param>
		public EventDelegateAttribute(string delegateName)
		{
			DelegateName = delegateName;
		}
	}
}


using System;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will handle events that are fired by a change in a control value.
    /// </summary>
    /// <typeparam name="T">The type of the value that was changed.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> args);

    /// <summary>
    /// Provides data for events that are fired by a change in a control value.
    /// </summary>
    /// <typeparam name="T">The type of the value that was changed.</typeparam>
    public class ValueChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the value of the control after the change was made.
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the value of the control before the change was made.
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ValueChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="oldValue">The value of the control before the change was made.</param>
        /// <param name="newValue">The value of the control after the change was made.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}

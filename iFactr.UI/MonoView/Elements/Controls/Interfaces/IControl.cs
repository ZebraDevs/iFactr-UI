using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that holds a value and can be interacted with by the user.
    /// </summary>
    public interface IControl : IElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the user can interact with this control.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the string representation of the control's current value.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        string StringValue { get; }

        /// <summary>
        /// Gets or sets the key to use when submitting control values.
        /// If a key is not set, the control will not be submitted.
        /// </summary>
        string SubmitKey { get; set; }

        /// <summary>
        /// Occurs when the control is being validated.
        /// </summary>
        event ValidationEventHandler Validating;

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        void NullifyEvents();

        /// <summary>
        /// Fires the <see cref="Validating"/> event and returns a value indicating whether or not validation has passed.
        /// </summary>
        /// <param name="errors">When the method returns, an array of validation errors that have occurred.</param>
        /// <returns><c>true</c> if validation has passed; otherwise, <c>false</c>.</returns>
        bool Validate(out string[] errors);
    }
}


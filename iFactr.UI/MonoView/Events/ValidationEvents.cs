using System;
using System.Collections.Generic;

namespace iFactr.UI
{
	/// <summary>
	/// Represents the method that will handle the <see cref="UI.Controls.IControl.Validating"/> event.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="args">The event data.</param>
	public delegate void ValidationEventHandler(object sender, ValidationEventArgs args);

	/// <summary>
    /// Provides data for the <see cref="UI.Controls.IControl.Validating"/> event.
	/// </summary>
	public class ValidationEventArgs : EventArgs
	{
		/// <summary>
		/// Gets a collection of reasons for why validation has failed.
        /// Validation will pass only if this collection is empty.
		/// </summary>
        public IList<string> Errors { get; private set; }

		/// <summary>
		/// Gets the <see cref="System.String"/> that represents the control's current value.
		/// </summary>
		public string StringValue { get; private set; }

        /// <summary>
        /// Gets the submission key of the control that is being validated.
        /// </summary>
        public string SubmitKey { get; private set; }

        /// <summary>
        /// Gets the current value of the control that is being validated.
        /// </summary>
        public object Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="iFactr.UI.ValidationEventArgs"/> class.
		/// </summary>
        /// <param name="submitKey">The submission key of the control that is being validated.</param>
        /// <param name="value">The current value of the control that is being validated.</param>
		/// <param name="stringValue">The <see cref="System.String"/> that represents the control's current value.</param>
		public ValidationEventArgs(string submitKey, object value, string stringValue)
		{
            Errors = new List<string>();
            SubmitKey = submitKey;
			StringValue = stringValue;
            Value = value;
		}
	}
}


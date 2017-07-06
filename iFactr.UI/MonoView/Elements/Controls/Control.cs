using System;
using System.Diagnostics;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that holds a value and can be interacted with by the user.  This class is abstract.
    /// </summary>
    public abstract class Control : Element, IControl
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:IsEnabled"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string IsEnabledProperty = "IsEnabled";

        /// <summary>
        /// The name of the <see cref="P:StringValue"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StringValueProperty = "StringValue";

        /// <summary>
        /// The name of the <see cref="P:SubmitKey"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SubmitKeyProperty = "SubmitKey";
        #endregion

        /// <summary>
        /// Occurs when the control is being validated.
        /// </summary>
        public event ValidationEventHandler Validating
        {
            add { NativeControl.Validating += value; }
            remove { NativeControl.Validating -= value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the user can interact with this control.
        /// </summary>
        public bool IsEnabled
        {
            get { return NativeControl.IsEnabled; }
            set { NativeControl.IsEnabled = value; }
        }

        /// <summary>
        /// Gets the string representation of the control's current value.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public string StringValue
        {
            get { return NativeControl.StringValue; }
        }

        /// <summary>
        /// Gets or sets the key to use when submitting control values.
        /// If a key is not set, the control will not be submitted.
        /// </summary>
        public string SubmitKey
        {
            get { return NativeControl.SubmitKey; }
            set { NativeControl.SubmitKey = value; }
        }


#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IControl NativeControl
        {
            get { return (IControl)Pair; }
        }

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        public void NullifyEvents()
        {
            NativeControl.NullifyEvents();
        }

        /// <summary>
        /// Fires the <see cref="Validating"/> event and returns a value indicating whether or not validation has passed.
        /// </summary>
        /// <param name="errors">When the method returns, an array of validation errors that have occurred.</param>
        /// <returns><c>true</c> if validation has passed; otherwise, <c>false</c>.</returns>
        public bool Validate(out string[] errors)
        {
            return NativeControl.Validate(out errors);
        }
    }
}


using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that lets the user select from a range of time values.
    /// </summary>
	public class TimePicker : Control, ITimePicker
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:Font"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FontProperty = "Font";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:Time"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TimeProperty = "Time";

        /// <summary>
        /// The name of the <see cref="P:TimeFormat"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TimeFormatProperty = "TimeFormat";
        #endregion

        /// <summary>
        /// Occurs when the picker's selected time has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event ValueChangedEventHandler<DateTime?> TimeChanged
        {
            add { NativeControl.TimeChanged += value; }
            remove { NativeControl.TimeChanged -= value; }
        }

        /// <summary>
        /// Gets or sets the background color of picker.
        /// </summary>
        public Color BackgroundColor
        {
            get { return NativeControl.BackgroundColor; }
            set { NativeControl.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the font to be used when rendering the picker's value.
        /// </summary>
        public Font Font
        {
            get { return NativeControl.Font; }
            set { NativeControl.Font = value; }
        }

        /// <summary>
        /// Gets or sets the color of the picker's foreground content.
        /// </summary>
        public Color ForegroundColor
        {
            get { return NativeControl.ForegroundColor; }
            set { NativeControl.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the selected time for the picker.
        /// </summary>
        public DateTime? Time
        {
            get { return NativeControl.Time; }
            set { NativeControl.Time = value; }
        }

        /// <summary>
        /// Gets or sets a string that describes the format in which the picker's value should be presented.
        /// </summary>
        public string TimeFormat
        {
            get { return NativeControl.TimeFormat; }
            set { NativeControl.TimeFormat = value; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private ITimePicker NativeControl
        {
            get { return (ITimePicker)Pair; }
        }
		
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.TimePicker"/> class.
        /// </summary>
		public TimePicker()
		{
			Pair = MXContainer.Resolve<ITimePicker>();

            NativeControl.BackgroundColor = new Color();
            NativeControl.Font = Font.PreferredDateTimePickerFont;
            NativeControl.ForegroundColor = new Color();
            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.TimePicker"/> class.
        /// </summary>
        /// <param name="value">The initial selected value.</param>
		public TimePicker(DateTime? value)
			: this()
		{
			Time = value;
		}

        /// <summary>
        /// Programmatically presents the picker to the user.
        /// </summary>
        public void ShowPicker()
        {
            NativeControl.ShowPicker();
        }
	}
}


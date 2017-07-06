using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that lets the user select from a range of date values.
    /// </summary>
	public class DatePicker : Control, IDatePicker
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
        /// The name of the <see cref="P:Date"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string DateProperty = "Date";

        /// <summary>
        /// The name of the <see cref="P:DateFormat"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string DateFormatProperty = "DateFormat";
        #endregion

        /// <summary>
        /// Occurs when the picker's selected date has changed.
        /// </summary>
		[NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
		public event ValueChangedEventHandler<DateTime?> DateChanged
		{
			add { NativeControl.DateChanged += value; }
			remove { NativeControl.DateChanged -= value; }
		}
		
        /// <summary>
        /// Gets or sets the background color of the picker.
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
        /// Gets or sets the selected date for the picker.
        /// </summary>
		public DateTime? Date
		{
			get { return NativeControl.Date; }
			set { NativeControl.Date = value; }
		}
		
        /// <summary>
        /// Gets or sets a string that describes the format in which the picker's value should be presented.
        /// </summary>
		public string DateFormat
		{
			get { return NativeControl.DateFormat; }
			set { NativeControl.DateFormat = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private IDatePicker NativeControl
        {
            get { return (IDatePicker)Pair; }
        }
		
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.DatePicker"/> class.
        /// </summary>
		public DatePicker()
		{
			Pair = MXContainer.Resolve<IDatePicker>();

            NativeControl.BackgroundColor = new Color();
            NativeControl.Font = Font.PreferredDateTimePickerFont;
            NativeControl.ForegroundColor = new Color();
            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.DatePicker"/> class.
        /// </summary>
        /// <param name="value">The initial selected value.</param>
		public DatePicker(DateTime? value)
			: this()
		{
			Date = value;
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


using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that lets the user select from a range of numerical values by sliding a knob along a track.
    /// </summary>
	public class Slider : Control, ISlider
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:MaximumTrackColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MaximumTrackColorProperty = "MaximumTrackColor";

        /// <summary>
        /// The name of the <see cref="P:MinimumTrackColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MinimumTrackColorProperty = "MinimumTrackColor";

        /// <summary>
        /// The name of the <see cref="P:MaxValue"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MaxValueProperty = "MaxValue";

        /// <summary>
        /// The name of the <see cref="P:MinValue"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MinValueProperty = "MinValue";

        /// <summary>
        /// The name of the <see cref="P:Value"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ValueProperty = "Value";
        #endregion

        /// <summary>
        /// Occurs when the slider's selected value has changed.
        /// </summary>
		[NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
		public event ValueChangedEventHandler<double> ValueChanged
		{
			add { NativeControl.ValueChanged += value; }
			remove { NativeControl.ValueChanged -= value; }
		}

        /// <summary>
        /// Gets or sets the color of the track between the knob and the maximum value.
        /// </summary>
		public Color MaximumTrackColor
		{
			get { return NativeControl.MaximumTrackColor; }
			set { NativeControl.MaximumTrackColor = value; }
		}

        /// <summary>
        /// Gets or sets the color of the track between the knob and the minimum value.
        /// </summary>
		public Color MinimumTrackColor
		{
			get { return NativeControl.MinimumTrackColor; }
			set { NativeControl.MinimumTrackColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the maximum numerical value that the user can select with the knob.
        /// </summary>
		public double MaxValue
		{
			get { return NativeControl.MaxValue; }
			set { NativeControl.MaxValue = value; }
		}
		
        /// <summary>
        /// Gets or sets the minimum numerical value that the user can select with the knob.
        /// </summary>
		public double MinValue
		{
			get { return NativeControl.MinValue; }
			set { NativeControl.MinValue = value; }
		}

        /// <summary>
        /// Gets or sets the increment value. User can only select values that are multiples of this value.
        /// </summary>
        public double StepSize { get; set; }
		
        /// <summary>
        /// Gets or sets the selected value of the slider.
        /// </summary>
		public double Value
		{
			get { return NativeControl.Value; }
			set { NativeControl.Value = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private ISlider NativeControl
        {
            get { return (ISlider)Pair; }
        }
		
        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Slider"/> class.
        /// </summary>
		public Slider()
		{
			Pair = MXContainer.Resolve<ISlider>();

            NativeControl.MinValue = 0;
            NativeControl.MaxValue = 100;
            NativeControl.MaximumTrackColor = new Color();
            NativeControl.MinimumTrackColor = new Color();
            NativeControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Slider"/> class.
        /// </summary>
        /// <param name="minValue">The minimum numerical value that the user can select with the knob.</param>
        /// <param name="maxValue">The maximum numerical value that the user can select with the knob.</param>
		public Slider(double minValue, double maxValue)
			: this()
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Slider"/> class.
        /// </summary>
        /// <param name="minValue">The minimum numerical value that the user can select with the knob.</param>
        /// <param name="maxValue">The maximum numerical value that the user can select with the knob.</param>
        /// <param name="value">The initial selected value.</param>
		public Slider(double minValue, double maxValue, double value)
			: this(minValue, maxValue)
		{
			Value = value;
		}
	}
}


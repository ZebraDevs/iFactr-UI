namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that lets the user select from a range of numerical values by sliding a knob along a track.
    /// </summary>
    public interface ISlider : IControl
    {
        /// <summary>
        /// Gets or sets the color of the track between the knob and the maximum value.
        /// </summary>
        Color MaximumTrackColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the track between the knob and the minimum value.
        /// </summary>
        Color MinimumTrackColor { get; set; }

        /// <summary>
        /// Gets or sets the maximum numerical value that the user can select with the knob.
        /// </summary>
        double MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum numerical value that the user can select with the knob.
        /// </summary>
        double MinValue { get; set; }

        /// <summary>
        /// Gets or sets the selected value of the slider.
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Occurs when the slider's selected value has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<double> ValueChanged;
    }
}

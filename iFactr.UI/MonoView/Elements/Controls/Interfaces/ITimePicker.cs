using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that lets the user select from a range of time values.
    /// </summary>
    public interface ITimePicker : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the picker.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the picker's value.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the picker's foreground content.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the selected time for the picker.
        /// </summary>
        DateTime? Time { get; set; }

        /// <summary>
        /// Gets or sets a string that describes the format in which the picker's value should be presented.
        /// </summary>
        string TimeFormat { get; set; }

        /// <summary>
        /// Occurs when the picker's selected time has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<DateTime?> TimeChanged;

        /// <summary>
        /// Programmatically presents the picker to the user.
        /// </summary>
        void ShowPicker();
    }
}

using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that lets the user select from a range of date values.
    /// </summary>
    public interface IDatePicker : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the picker.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the selected date for the picker.
        /// </summary>
        DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets a string that describes the format in which the picker's value should be presented.
        /// </summary>
        string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the picker's value.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the picker's foreground content.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Occurs when the picker's selected date has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<DateTime?> DateChanged;

        /// <summary>
        /// Programmatically presents the picker to the user.
        /// </summary>
        void ShowPicker();
    }
}

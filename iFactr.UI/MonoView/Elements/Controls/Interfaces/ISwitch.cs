namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that holds a boolean value and can be toggled by the user.
    /// </summary>
    public interface ISwitch : IControl
    {
        /// <summary>
        /// Gets or sets the color of the switch when its value is <c>false</c>.
        /// </summary>
        Color FalseColor { get; set; }

        /// <summary>
        /// Gets or sets the color of any additional visual information that is part of the switch.
        /// For example, Metro and Windows Phone include text that represents the value of the switch.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the switch when its value is <c>true</c>.
        /// </summary>
        Color TrueColor { get; set; }

        /// <summary>
        /// Gets or sets the boolean value of the switch.
        /// </summary>
        bool Value { get; set; }

        /// <summary>
        /// Occurs when the switch's boolean value has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<bool> ValueChanged;
    }
}

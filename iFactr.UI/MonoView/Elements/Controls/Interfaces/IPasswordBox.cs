using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that can accept a single line of text input from the user.
    /// The input is masked so that it cannot be read, allowing for secured entry of sensitive information.
    /// </summary>
    public interface IPasswordBox : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the password box.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a regular expression string for restricting the text that the user can input.
        /// A value of <c>null</c> means that there is no restriction.
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering masked input characters.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the masked input characters.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this instance has input focus.
        /// </summary>
        bool IsFocused { get; }

        /// <summary>
        /// Gets or sets the type of soft keyboard to use when the password box has focus.
        /// </summary>
        KeyboardType KeyboardType { get; set; }

        /// <summary>
        /// Gets or sets how the 'Return' key of a soft keyboard should be presented when the password box has focus.
        /// </summary>
        KeyboardReturnType KeyboardReturnType { get; set; }

        /// <summary>
        /// Gets or sets the password value inside of the password box.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the text to display when the password box does not have a value.
        /// </summary>
        string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        Color PlaceholderColor { get; set; }

        /// <summary>
        /// Occurs when the password box receives focus.
        /// </summary>
        event EventHandler GotFocus;

        /// <summary>
        /// Occurs when the password box loses focus.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        /// Occurs when the password box's password value has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<string> PasswordChanged;

        /// <summary>
        /// Occurs when the 'Return' key on the keyboard is pressed.
        /// </summary>
        event EventHandler<EventHandledEventArgs> ReturnKeyPressed;

        /// <summary>
        /// Programmatically sets input focus to the password box.
        /// </summary>
        void Focus();
    }
}

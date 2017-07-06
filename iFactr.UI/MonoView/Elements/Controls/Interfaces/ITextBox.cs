using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that can accept a single line of text input from the user.
    /// </summary>
    public interface ITextBox : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the text box.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a regular expression string for restricting the text that the user can input.
        /// A value of <c>null</c> means that there is no restriction.
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the text.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets a value indicating whether or not this instance has input focus.
        /// </summary>
        bool IsFocused { get; }

        /// <summary>
        /// Gets or sets the type of soft keyboard to use when the text box has focus.
        /// </summary>
        KeyboardType KeyboardType { get; set; }

        /// <summary>
        /// Gets or sets how the 'Return' key of a soft keyboard should be presented when the text box has focus.
        /// </summary>
        KeyboardReturnType KeyboardReturnType { get; set; }

        /// <summary>
        /// Gets or sets the text to display when the text box does not have a value.
        /// </summary>
        string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        Color PlaceholderColor { get; set; }

        /// <summary>
        /// Gets or sets the text value inside of the text box.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets how the text is aligned within the text box.
        /// </summary>
        TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// Gets or sets completion behavior for text that is entered into the text box.
        /// Not all platforms support all behaviors.
        /// </summary>
        TextCompletion TextCompletion { get; set; }

        /// <summary>
        /// Occurs when the text box receives focus.
        /// </summary>
        event EventHandler GotFocus;

        /// <summary>
        /// Occurs when the text box loses focus.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        /// Occurs when the 'Return' key on the keyboard is pressed.
        /// </summary>
        event EventHandler<EventHandledEventArgs> ReturnKeyPressed;

        /// <summary>
        /// Occurs when the text box's text value has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<string> TextChanged;

        /// <summary>
        /// Programmatically sets input focus to the text box.
        /// </summary>
        void Focus();
    }
}


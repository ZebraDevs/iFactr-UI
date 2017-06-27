using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that can perform application-defined behavior when the user clicks or taps on it.
    /// </summary>
    public interface IButton : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the button.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the button's title.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the button's foreground content.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets an image to display with the button.
        /// </summary>
        IImage Image { get; set; }

        /// <summary>
        /// Gets or sets the link to navigate to when the button is selected.
        /// The navigation only occurs if there is no handler for the <see cref="Clicked"/> event.
        /// </summary>
        Link NavigationLink { get; set; }

        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Occurs when the user clicks or taps on the button.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Clicked;
    }
}


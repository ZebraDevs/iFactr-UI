using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a clickable button in an <see cref="iFactr.UI.IToolbar"/> object.
    /// </summary>
    public interface IToolbarButton : IToolbarItem, IEquatable<IToolbarButton>
    {
        /// <summary>
        /// Gets or sets the file path to an image to display as part of the button.
        /// </summary>
        string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the link to navigate to when the user clicks or taps the button.
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

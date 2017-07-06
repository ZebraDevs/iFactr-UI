using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an pressable button within an <see cref="iFactr.UI.IMenu"/> object.
    /// </summary>
    public interface IMenuButton : IPairable, IEquatable<IMenuButton>
    {
        /// <summary>
        /// Gets the title of the button.
        /// </summary>
        string Title { get; }
        
        /// <summary>
        /// Gets or sets the file path of the image to use with the button.
        /// </summary>
        string ImagePath { get; set; }
        
        /// <summary>
        /// Gets or sets the link to navigate to when the button is selected.
        /// The navigation only occurs if there is no handler for the <see cref="Clicked"/> event.
        /// </summary>
        Link NavigationLink { get; set; }
        
        /// <summary>
        /// Occurs when the user clicks or taps on the button.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Clicked;
    }
}


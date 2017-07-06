using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a selectable item within an <see cref="iFactr.UI.ITabView"/>.
    /// </summary>
    public interface ITabItem : IPairable, IEquatable<ITabItem>
    {
        /// <summary>
        /// Gets or sets any auxiliary information to display alongside the item.
        /// </summary>
        string BadgeValue { get; set; }
        
        /// <summary>
        /// Gets or sets the file path of the image to use for the item.
        /// </summary>
        string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the link to navigate to when the item is selected and the view stack associated with the item is empty.
        /// The navigation only occurs if there is no handler for the <see cref="Selected"/> event.
        /// </summary>
        Link NavigationLink { get; set; }
        
        /// <summary>
        /// Gets or sets the title for the item.
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the color with which to display the title.
        /// </summary>
        Color TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering the title.
        /// </summary>
        Font TitleFont { get; set; }
        
        /// <summary>
        /// Occurs when the item is selected by the user.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Selected;
    }
}


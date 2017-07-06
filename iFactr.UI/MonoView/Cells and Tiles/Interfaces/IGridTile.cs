using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a tile that acts as a grid for laying out various UI elements.
    /// </summary>
    public interface IGridTile : ITile, IGrid
    {
        /// <summary>
        /// Gets or sets the link to navigate to when the tile has been selected by the user.
        /// The navigation only occurs if there is no handler for the <see cref="Selected"/> event.
        /// </summary>
        Link NavigationLink { get; set; }

        /// <summary>
        /// Occurs when the user selects the tile.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Selected;

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        void NullifyEvents();
    }
}

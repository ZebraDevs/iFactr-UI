using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a cell that acts as a grid for laying out various UI elements.
    /// This is the most common type of cell.
    /// </summary>
    public interface IGridCell : ICell, IGridBase
    {
        /// <summary>
        /// Gets or sets the link to navigate to when the cell's accessory has been selected by the user.
        /// The navigation only occurs if there is no handler for the <see cref="AccessorySelected"/> event.
        /// </summary>
        Link AccessoryLink { get; set; }

        /// <summary>
        /// Gets or sets the link to navigate to when the cell has been selected by the user.
        /// The navigation only occurs if there is no handler for the <see cref="Selected"/> event.
        /// </summary>
        Link NavigationLink { get; set; }

        /// <summary>
        /// Gets or sets the color with which to highlight the cell when it is selected.
        /// This may not appear on all platforms.
        /// </summary>
        Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets which visual elements to use to indicate that the cell is selectable or has been selected.
        /// </summary>
        SelectionStyle SelectionStyle { get; set; }

        /// <summary>
        /// Occurs when the user selects the cell.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Selected;

        /// <summary>
        /// Occurs when the user selects the accessory for the cell.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler AccessorySelected;

        /// <summary>
        /// Resets the invocation list of all events within the class.
        /// </summary>
        void NullifyEvents();

        /// <summary>
        /// Programmatically selects the cell.
        /// </summary>
        void Select();
    }
}

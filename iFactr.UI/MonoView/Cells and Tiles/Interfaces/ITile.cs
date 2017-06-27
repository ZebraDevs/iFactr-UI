using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an entry in an <see cref="iFactr.UI.IListView"/> instance.  This is the base interface for
    /// the <see cref="iFactr.UI.IGridTile"/> interface and is only supported on the Windows 8 Metro platform.
    /// </summary>
    public interface ITile : IPairable, IEquatable<ITile>
    {
        /// <summary>
        /// Gets or sets the background color of the tile.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the maximum size that the tile is allowed to be.
        /// </summary>
        Size MaxSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum size that the tile is allowed to be.
        /// </summary>
        Size MinSize { get; set; }

        /// <summary>
        /// Gets or sets the color with which to highlight the tile when it is selected.
        /// </summary>
        Color SelectionColor { get; set; }
    }
}

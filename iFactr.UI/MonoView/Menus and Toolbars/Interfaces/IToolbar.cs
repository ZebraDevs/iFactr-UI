using System;
using System.Collections.Generic;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a toolbar containing items that provide support functions for an <see cref="iFactr.UI.ICanvasView"/>.
    /// </summary>
    public interface IToolbar : IPairable, IEquatable<IToolbar>
    {
        /// <summary>
        /// Gets or sets the background color of the toolbar on platforms that support it.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the toolbar on platforms that support it.
        /// Any toolbar items that do not specify their own foreground color will use this value.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets a collection of toolbar items that are considered primary commands.
        /// These are commonly aligned to the right of the toolbar, but platform behavior may vary.
        /// </summary>
        IEnumerable<IToolbarItem> PrimaryItems { get; set; }

        /// <summary>
        /// Gets or sets a collection of toolbar items that are considered secondary commands.
        /// These are commonly aligned to the left of the toolbar, but platform behavior may vary.
        /// </summary>
        IEnumerable<IToolbarItem> SecondaryItems { get; set; }
    }
}

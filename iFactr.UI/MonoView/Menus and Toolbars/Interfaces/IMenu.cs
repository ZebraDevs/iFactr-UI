using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a set of items that provide support functions for an
    /// <see cref="iFactr.UI.IListView"/> or <see cref="iFactr.UI.IBrowserView"/>.
    /// </summary>
    public interface IMenu : IPairable, IEquatable<IMenu>
    {
        /// <summary>
        /// Gets or sets the background color of the menu on supported platforms.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the menu on supported platforms.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the file path of the image to use for the button that activates the menu.
        /// Not all platforms have this button available.
        /// </summary>
        string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the color to highlight the menu when it is selected.
        /// </summary>
        Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets the title of the button that activates the menu.
        /// Not all platforms have this button available.
        /// </summary>
        string Title { get; set; }

		/// <summary>
		/// Gets the number of menu buttons that are currently on the menu.
		/// </summary>
		int ButtonCount { get; }

        /// <summary>
        /// Adds the specified <see cref="iFactr.UI.IMenuButton"/> to the menu.
        /// </summary>
        /// <param name="menuButton">The button to add.</param>
        void Add(IMenuButton menuButton);

		/// <summary>
		/// Returns the <see cref="iFactr.UI.IMenuButton"/> at the specified index.
		/// </summary>
		/// <param name="index">The index of the button to return.</param>
		/// <returns>The <see cref="iFactr.UI.IMenuButton"/> at the specified index.</returns>
		/// <exception cref="IndexOutOfRangeException">Thrown when the index equals or exceeds the
		/// number of buttons in the menu -or- when the index is less than 0.</exception>
		IMenuButton GetButton(int index);
    }
}

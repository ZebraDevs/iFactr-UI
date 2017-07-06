using iFactr.UI.Controls;

namespace iFactr.UI
{
    /// <summary>
    /// Defines the visible state of an <see cref="IElement"/> instance.
    /// </summary>
    public enum Visibility : byte
    {
        /// <summary>
        /// The element is visible.
        /// </summary>
        Visible = 0,
        /// <summary>
        /// The element is not visible, but it will still reserve space during layout.
        /// </summary>
        Hidden = 1,
        /// <summary>
        /// The element is not visible, and it does not reserve space during layout.
        /// </summary>
        Collapsed = 2,
    }
}
using System.Collections;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element that lets the user make a single selection from a list of items.
    /// </summary>
    public interface ISelectList : IControl
    {
        /// <summary>
        /// Gets or sets the background color of the select list.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font to be used when rendering each item in the list.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of each item in the list.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets a collection of the items that are to be presented in the select list.
        /// The select list will render the string that is returned by each item's ToString method.
        /// </summary>
        IEnumerable Items { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// Occurs when the selected item has changed.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event ValueChangedEventHandler<object> SelectionChanged;

        /// <summary>
        /// Programmatically presents the list to the user.
        /// </summary>
        void ShowList();
    }
}

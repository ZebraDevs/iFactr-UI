using System.Collections.Generic;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a native view that contains selectable tab items for separating an application into categories.
    /// </summary>
    public interface ITabView : IView
    {
        /// <summary>
        /// Gets or sets the index of the currently selected item.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the color with which to overlay the selected item.
        /// </summary>
        Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="ITabItem"/>s to populate the view with when it is displayed in a normal context.
        /// </summary>
        IEnumerable<ITabItem> TabItems { get; set; }
    }

    /// <summary>
    /// Defines a native view that contains selectable tab items for separating an application into categories.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface ITabView<T> : ITabView, IMXView<T> { }
}
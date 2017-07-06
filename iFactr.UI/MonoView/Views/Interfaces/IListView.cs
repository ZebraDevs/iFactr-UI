#pragma warning disable 0419

using System;
using System.Collections.Generic;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will retrieve an <see cref="iFactr.UI.ICell"/> instance when it is ready to be rendered on screen.
    /// </summary>
    /// <param name="section">The section that will contain the cell.</param>
    /// <param name="index">The index at which the cell will be placed in the section.</param>
    /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or <c>null</c> if no cell has been recycled.</param>
    /// <returns>The cell that will be rendered on screen.</returns>
    public delegate ICell CellDelegate(int section, int index, ICell recycledCell);

    /// <summary>
    /// Represents the method that will retrieve an identifier for determining which, if any, <see cref="iFactr.UI.ICell"/>
    /// instances can be reused for the item at the given position.
    /// </summary>
    /// <param name="section">The section that will contain the item.</param>
    /// <param name="index">The index at which the item will be placed in the section.</param>
    /// <returns>An identifier for determining which cells can be reused for the item at the given position.</returns>
    public delegate int ItemIdDelegate(int section, int index);

    /// <summary>
    /// Defines a native view that lays its contents out in a list of customizable cells.
    /// This is the most common type of view.
    /// </summary>
    public interface IListView : IView, IHistoryEntry
    {
        /// <summary>
        /// Gets or sets the number of columns in which to lay out the contents of the view.
        /// Certain platforms may only support a single column and will ignore any other setting.
        /// </summary>
        ColumnMode ColumnMode { get; set; }

        /// <summary>
        /// Gets or sets the color of the separator between cells.  This may not appear on all platforms.
        /// </summary>
        Color SeparatorColor { get; set; }

        /// <summary>
        /// Gets the style in which the view is to be rendered.
        /// </summary>
        ListViewStyle Style { get; }

        /// <summary>
        /// Gets or sets a menu of selectable items that provide support functions for the view.
        /// </summary>
        IMenu Menu { get; set; }

        /// <summary>
        /// Gets or sets a search box to include with the view.
        /// </summary>
        ISearchBox SearchBox { get; set; }

        /// <summary>
        /// Gets a collection of the <see cref="Section"/>s that are currently part of the view.
        /// </summary>
        SectionCollection Sections { get; }

        /// <summary>
        /// Gets a collection of errors that have occurred during control validation.
        /// </summary>
        ValidationErrorCollection ValidationErrors { get; }

        /// <summary>
        /// Invoked when a cell is ready to be rendered on the screen.  Return the <see cref="iFactr.UI.ICell"/> instance
        /// that should be placed at the given index within the given section.
        /// </summary>
        CellDelegate CellRequested { get; set; }

        /// <summary>
        /// Invoked when a reuse identifier is needed for a cell.  Return the identifier that should be used
        /// to determine which cells may be reused for the item at the given index within the given section.
        /// This is only invoked on platforms that support recycling.
        /// </summary>
        ItemIdDelegate ItemIdRequested { get; set; }

        /// <summary>
        /// Occurs when the view is being submitted and the values of its controls are being gathered.
        /// Use this event to manipulate a submitted value prior to navigation or to cancel the submission.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event SubmissionEventHandler Submitting;

        /// <summary>
        /// Returns the control values that would be submitted as parameters if the <see cref="Submit(string)"/> or <see cref="Submit(Link)"/> methods are called.
        /// </summary>
        /// <returns>The control values as an <see cref="T:IDictionary&lt;string, string&gt;"/>.</returns>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        IDictionary<string, string> GetSubmissionValues();

        /// <summary>
        /// Returns the cells that are currently attached to the visual tree.
        /// </summary>
        /// <returns>The visible cells as an <see cref="T:IEnumerable&lt;ICell&gt;"/>.</returns>
        IEnumerable<ICell> GetVisibleCells();

        /// <summary>
        /// Clears out and reloads the sections in the view while keeping the rest of the view intact.
        /// This is commonly called in an <see cref="ISearchBox.SearchPerformed"/> handler in order to
        /// display the filtered results.
        /// </summary>
        void ReloadSections();

        /// <summary>
        /// Programmatically scrolls to the cell located at the specified <paramref name="section"/> and <paramref name="index"/>.
        /// </summary>
        /// <param name="section">The section of the cell to scroll to.</param>
        /// <param name="index">The index of the cell to scroll to.</param>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        void ScrollToCell(int section, int index, bool animated);

        /// <summary>
        /// Programmatically scrolls to the end of the list.
        /// This is typically the bottom for vertical lists and the far right for horizontal lists.
        /// </summary>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        void ScrollToEnd(bool animated);

        /// <summary>
        /// Programmatically scrolls to the beginning of the list.
        /// This is typically the top for vertical lists and the far left for horizontal lists.
        /// </summary>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        void ScrollToHome(bool animated);

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating
        /// to the specified URL with the gathered control values as parameters.
        /// </summary>
        /// <param name="url">The URL to navigate to after all control values have been gathered.</param>
        void Submit(string url);

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating to the specified
        /// <see cref="Link"/> with the gathered control values as parameters.
        /// </summary>
        /// <param name="link">The link to navigate to after all control values have been gathered.</param>
        void Submit(Link link);
    }

    /// <summary>
    /// Defines a native view that lays its contents out in a list of customizable cells.
    /// This is the most common type of view.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface IListView<T> : IListView, IMXView<T> { }
}
#pragma warning disable 0419

using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native view that lays its contents out in a list of customizable cells.
    /// This is the most common type of view.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public class ListView<T> : View, IListView<T>
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackLinkProperty = "BackLink";

        /// <summary>
        /// The name of the <see cref="P:ColumnMode"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ColumnModeProperty = "ColumnMode";

        /// <summary>
        /// The name of the <see cref="P:Menu"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MenuProperty = "Menu";

        /// <summary>
        /// The name of the <see cref="P:OutputPane"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string OutputPaneProperty = "OutputPane";

        /// <summary>
        /// The name of the <see cref="P:PopoverPresentationStyle"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PopoverPresentationStyleProperty = "PopoverPresentationStyle";

        /// <summary>
        /// The name of the <see cref="P:SearchBox"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SearchBoxProperty = "SearchBox";

        /// <summary>
        /// The name of the <see cref="P:SeparatorColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SeparatorColorProperty = "SeparatorColor";

        /// <summary>
        /// The name of the <see cref="P:StackID"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StackIDProperty = "StackID";
        #endregion

        /// <summary>
        /// Occurs when the view is pushed or popped onto the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Activated
        {
            add { NativeView.Activated += value; }
            remove { NativeView.Activated -= value; }
        }

        /// <summary>
        /// Occurs when the view is pushed under or popped off of the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Deactivated
        {
            add { NativeView.Deactivated += value; }
            remove { NativeView.Deactivated -= value; }
        }

        /// <summary>
        /// Occurs when the view is being submitted and the values of its controls are being gathered.
        /// Use this event to manipulate a submitted value prior to navigation or to cancel the submission.
        /// </summary>
        public event SubmissionEventHandler Submitting
        {
            add { NativeView.Submitting += value; }
            remove { NativeView.Submitting -= value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Link"/> that describes the behavior
        /// and appearance of the back button associated with the view.
        /// </summary>
        public Link BackLink
        {
            get { return NativeView.BackLink; }
            set { NativeView.BackLink = value; }
        }

        /// <summary>
        /// Gets or sets the number of columns in which to lay out the contents of the view.
        /// Certain platforms may only support a single column and will ignore any other setting.
        /// </summary>
        public ColumnMode ColumnMode
        {
            get { return NativeView.ColumnMode; }
            set { NativeView.ColumnMode = value; }
        }

        /// <summary>
        /// Gets or sets the color of the separator between cells.  This may not appear on all platforms.
        /// </summary>
        public Color SeparatorColor
        {
            get { return NativeView.SeparatorColor; }
            set { NativeView.SeparatorColor = value; }
        }

        /// <summary>
        /// Gets the style in which the view is to be rendered.
        /// </summary>
        public ListViewStyle Style
        {
            get { return NativeView.Style; }
        }

        /// <summary>
        /// Gets or sets a menu of selectable items that provide support functions for the view.
        /// </summary>
        public IMenu Menu
        {
            get { return NativeView.Menu; }
            set { NativeView.Menu = value; }
        }

        /// <summary>
        /// Gets or sets a search box to include with the view.
        /// </summary>
        public ISearchBox SearchBox
        {
            get { return NativeView.SearchBox; }
            set { NativeView.SearchBox = value; }
        }

        /// <summary>
        /// Gets or sets the stack identifier for the view.
        /// Views with the same identifier will take the same place in the view stack.
        /// </summary>
        public string StackID
        {
            get { return NativeView.StackID; }
            set { NativeView.StackID = value; }
        }

        /// <summary>
        /// Gets or sets the pane on which the view will be rendered.
        /// </summary>
        public Pane OutputPane
        {
            get { return NativeView.OutputPane; }
            set { NativeView.OutputPane = value; }
        }

        /// <summary>
        /// Gets or sets the style in which the view should be presented when displayed in a popover pane.
        /// </summary>
        public PopoverPresentationStyle PopoverPresentationStyle
        {
            get { return NativeView.PopoverPresentationStyle; }
            set { NativeView.PopoverPresentationStyle = value; }
        }

        /// <summary>
        /// Gets the view stack that the view is currently on.
        /// </summary>
        public IHistoryStack Stack
        {
            get { return NativeView.Stack; }
        }

        /// <summary>
        /// Gets a collection of the <see cref="Section"/>s that are currently part of the view.
        /// </summary>
        public SectionCollection Sections
        {
            get { return NativeView.Sections; }
        }

        /// <summary>
        /// Gets a collection of errors that have occurred during control validation.
        /// </summary>
        public ValidationErrorCollection ValidationErrors
        {
            get { return NativeView.ValidationErrors; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        CellDelegate IListView.CellRequested
        {
            get { return NativeView.CellRequested; }
            set { NativeView.CellRequested = value ?? OnCellRequested; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        ItemIdDelegate IListView.ItemIdRequested
        {
            get { return NativeView.ItemIdRequested; }
            set { NativeView.ItemIdRequested = value ?? OnItemIdRequested; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        ShouldNavigateDelegate IHistoryEntry.ShouldNavigate
        {
            get { return NativeView.ShouldNavigate; }
            set { NativeView.ShouldNavigate = value ?? ShouldNavigateFrom; }
        }

        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        public T Model
        {
            get
            {
                try
                {
                    return (T)Pair.GetModel();
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("Could not cast to " + typeof(T).Name + ".  There is a type mismatch between the view and its native counterpart.", e);
                }
            }
            set { Pair.SetModel(value); }
        }

        /// <summary>
        /// Gets the type of the model displayed by the view.
        /// </summary>
        public override sealed Type ModelType
        {
            get { return typeof(T); }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IListView NativeView
        {
            get { return (IListView)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ListView&lt;T&gt;"/> class.
        /// </summary>
        public ListView()
            : this(default(T), ListViewStyle.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ListView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model containing the information that is displayed by the view.</param>
        public ListView(T model)
            : this(model, ListViewStyle.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ListView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="style">The style in which the view should be rendered.</param>
        public ListView(ListViewStyle style)
            : this(default(T), style)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ListView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model containing the information that is displayed by the view.</param>
        /// <param name="style">The style in which the view should be rendered.</param>
        public ListView(T model, ListViewStyle style)
        {
            Pair = MXContainer.Resolve<IListView>(style);

            NativeView.CellRequested = OnCellRequested;
            NativeView.ItemIdRequested = OnItemIdRequested;
            NativeView.ShouldNavigate = ShouldNavigateFrom;
            NativeView.SeparatorColor = iApp.Instance.Style.SeparatorColor;
            NativeView.PopoverPresentationStyle = PopoverPresentationStyle.Normal;

            Model = model;
        }

        /// <summary>
        /// Returns the control values that would be submitted as parameters if the <see cref="Submit(string)"/> or <see cref="Submit(Link)"/> methods are called.
        /// </summary>
        /// <returns>The control values as an <see cref="T:IDictionary&lt;string, string&gt;"/>.</returns>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public IDictionary<string, string> GetSubmissionValues()
        {
            return NativeView.GetSubmissionValues();
        }

        /// <summary>
        /// Returns the cells that are currently attached to the visual tree.
        /// </summary>
        /// <returns>The visible cells as an <see cref="T:IEnumerable&lt;ICell&gt;"/>.</returns>
        public IEnumerable<ICell> GetVisibleCells()
        {
            return NativeView.GetVisibleCells();
        }

        /// <summary>
        /// Clears out and reloads the sections in the view while keeping the rest of the view intact.
        /// This is commonly called in an <see cref="ISearchBox.SearchPerformed"/> handler in order to
        /// display the filtered results.
        /// </summary>
        public void ReloadSections()
        {
            NativeView.ReloadSections();
        }

        /// <summary>
        /// Programmatically scrolls to the cell located at the specified <paramref name="section"/> and <paramref name="index"/>.
        /// </summary>
        /// <param name="section">The section of the cell to scroll to.</param>
        /// <param name="index">The index of the cell to scroll to.</param>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        public void ScrollToCell(int section, int index, bool animated)
        {
            NativeView.ScrollToCell(section, index, animated);
        }

        /// <summary>
        /// Programmatically scrolls to the end of the list.
        /// This is typically the bottom for vertical lists and the far right for horizontal lists.
        /// </summary>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        public void ScrollToEnd(bool animated)
        {
            NativeView.ScrollToEnd(animated);
        }

        /// <summary>
        /// Programmatically scrolls to the beginning of the list.
        /// This is typically the top for vertical lists and the far left for horizontal lists.
        /// </summary>
        /// <param name="animated">Whether or not the scrolling should be animated.</param>
        public void ScrollToHome(bool animated)
        {
            NativeView.ScrollToHome(animated);
        }

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating
        /// to the specified URL with the gathered control values as parameters.
        /// </summary>
        /// <param name="url">The URL to navigate to after all control values have been gathered.</param>
        public void Submit(string url)
        {
            NativeView.Submit(url);
        }

        /// <summary>
        /// Submits the view, gathering the values of its controls and navigating
        /// to the specified link with the gathered control values as parameters.
        /// </summary>
        /// <param name="link">The link to navigate to after all control values have been gathered.</param>
        public void Submit(Link link)
        {
            NativeView.Submit(link);
        }

        /// <summary>
        /// Gets the model for the view.
        /// </summary>
        public override sealed object GetModel()
        {
            return Pair.GetModel();
        }

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The object to set the model to.</param>
        /// <exception cref="InvalidCastException">Thrown when the <paramref name="model"/> is of an incorrect type.</exception>
        public override sealed void SetModel(object model)
        {
            Pair.SetModel(model);
        }

        /// <summary>
        /// Called when a cell is ready to be rendered on the screen.  Return the <see cref="iFactr.UI.ICell"/> instance
        /// that should be placed at the given index within the given section.
        /// </summary>
        /// <param name="section">The section that will contain the cell.</param>
        /// <param name="index">The index at which the cell will be placed in the section.</param>
        /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or <c>null</c> if no cell has been recycled.</param>
        protected virtual ICell OnCellRequested(int section, int index, ICell recycledCell)
        {
            return null;
        }

        /// <summary>
        /// Called when a reuse identifier is needed for a cell.  Return the identifier that should be used
        /// to determine which cells may be reused for the item at the given index within the given section.
        /// This is only called on platforms that support recycling.  See Remarks for details.
        /// </summary>
        /// <param name="section">The section that will contain the item.</param>
        /// <param name="index">The index at which the item will be placed in the section.</param>
        /// <remarks>
        /// Cell recycling is an important technique for increasing performance on certain platforms.
        /// It is recommended to utilize this whenever possible to keep your application running at optimal efficiency.
        /// When a platform recycles a cell, instead of instantiating a new instance, it uses a cell that already
        /// resides in memory but is no longer on the screen.  Any cell with the same identifier can be chosen for
        /// recycling regardless of what the cell contains.  It is considered best practice to only use the same identifier
        /// for cells that have the same layout structure.  For instance, two cells with the same columns, rows, and element
        /// types would be ideal candidates for reuse, but two cells with significantly different layouts would be better
        /// off with different identifiers.  A recycled cell still has all of its contents from when it was last rendered,
        /// and it is the responsibility of the developer to remove what is no longer needed, add what is now necessary,
        /// and alter what needs to be changed.  Be aware that any event handlers are still attached.  If you want to replace
        /// the handlers with new ones, use the NullifyEvents method to detached any handlers that are currently attached.
        /// Using the same identifier for <see cref="GridCell"/>s and <see cref="RichContentCell"/>s is not supported and should be avoided.
        /// </remarks>
        protected virtual int OnItemIdRequested(int section, int index)
        {
            return 0;
        }

        /// <summary>
        /// Called when the view is being pushed under or popped off of the top of the view stack.
        /// Returning a value of <c>false</c> will cancel the navigation.
        /// </summary>
        /// <param name="link">A <see cref="Link"/> containing the destination and any other relevant information regarding the navigation taking place.</param>
        /// <param name="type">The type of navigation that was initiated.</param>
        /// <returns><c>true</c> to proceed with the navigation; otherwise, <c>false</c>.</returns>
        protected virtual bool ShouldNavigateFrom(Link link, NavigationType type)
        {
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using iFactr.UI.Controls;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native view that acts as a grid for laying out various UI elements.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public class GridView<T> : View, IGridView<T>
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackLinkProperty = "BackLink";

        /// <summary>
        /// The name of the <see cref="P:HorizontalScrollingEnabled"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string HorizontalScrollingEnabledProperty = "HorizontalScrollingEnabled";

        /// <summary>
        /// The name of the <see cref="P:VerticalScrollingEnabled"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string VerticalScrollingEnabledProperty = "VerticalScrollingEnabled";

        /// <summary>
        /// The name of the <see cref="P:Menu"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string MenuProperty = "Menu";

        /// <summary>
        /// The name of the <see cref="P:OutputPane"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string OutputPaneProperty = "OutputPane";

        /// <summary>
        /// The name of the <see cref="P:Padding"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PaddingProperty = "Padding";

        /// <summary>
        /// The name of the <see cref="P:PopoverPresentationStyle"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PopoverPresentationStyleProperty = "PopoverPresentationStyle";

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
        /// Gets or sets a value indicating whether horizontal scrolling is enabled.
        /// If <c>false</c>, the content will be locked to the width of the view.
        /// </summary>
        public bool HorizontalScrollingEnabled
        {
            get { return NativeView.HorizontalScrollingEnabled; }
            set { NativeView.HorizontalScrollingEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical scrolling is enabled.
        /// If <c>false</c>, the content will be locked to the height of the view.
        /// </summary>
        public bool VerticalScrollingEnabled
        {
            get { return NativeView.VerticalScrollingEnabled; }
            set { NativeView.VerticalScrollingEnabled = value; }
        }

        /// <summary>
        /// Gets a collection of the columns that currently make up the view.
        /// </summary>
        public ColumnCollection Columns
        {
            get { return NativeView.Columns; }
        }

        /// <summary>
        /// Gets a collection of the UI elements that currently reside within the view.
        /// </summary>
        public IEnumerable<IElement> Children
        {
            get { return NativeView.Children; }
        }

        /// <summary>
        /// Gets or sets the amount of spacing between the edges of the view and the children within it.
        /// </summary>
        public Thickness Padding
        {
            get { return NativeView.Padding; }
            set { NativeView.Padding = value; }
        }

        /// <summary>
        /// Gets a collection of the rows that currently make up the view.
        /// </summary>
        public RowCollection Rows
        {
            get { return NativeView.Rows; }
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
        /// Gets or sets the <see cref="Link"/> that describes the behavior
        /// and appearance of the back button associated with the view.
        /// </summary>
        public Link BackLink
        {
            get { return NativeView.BackLink; }
            set { NativeView.BackLink = value; }
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
        /// Invoked when the view is being pushed under or popped off of the top of the view stack.
        /// Returning a value of <c>false</c> will cancel the navigation.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        ShouldNavigateDelegate IHistoryEntry.ShouldNavigate
        {
            get { return NativeView.ShouldNavigate; }
            set
            {
                if (value == null)
                {
                    NativeView.ShouldNavigate = ShouldNavigateFrom;
                }
                else
                {
                    NativeView.ShouldNavigate = value;
                }
            }
        }

        /// <summary>
        /// Gets a collection of errors that have occurred during control validation.
        /// </summary>
        public ValidationErrorCollection ValidationErrors
        {
            get { return NativeView.ValidationErrors; }
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
        private IGridView NativeView
        {
            get { return (IGridView)Pair; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.GridView&lt;T&gt;"/> class.
        /// </summary>
        public GridView()
        {
            Pair = MXContainer.Resolve<IGridView>();

            NativeView.HorizontalScrollingEnabled = false;
            NativeView.VerticalScrollingEnabled = false;
            NativeView.Padding = new Thickness(Thickness.LeftMargin, Thickness.TopMargin, Thickness.RightMargin, Thickness.BottomMargin);
            NativeView.PopoverPresentationStyle = PopoverPresentationStyle.Normal;
            NativeView.ShouldNavigate = ShouldNavigateFrom;
        }

        /// <summary>
        /// Adds the specified <see cref="IElement"/> instance to the view.
        /// </summary>
        /// <param name="element">The element to be added to the view.</param>
        public void AddChild(IElement element)
        {
            NativeView.AddChild(element);
        }

        /// <summary>
        /// Removes the specified <see cref="IElement"/> instance from the view.
        /// </summary>
        /// <param name="element">The element to be removed from the view.</param>
        public void RemoveChild(IElement element)
        {
            NativeView.RemoveChild(element);
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

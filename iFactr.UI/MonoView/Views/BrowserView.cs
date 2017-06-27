using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native view that supports HTML rendering and web page browsing.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public class BrowserView<T> : View, IBrowserView<T>
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackLinkProperty = "BackLink";

        /// <summary>
        /// The name of the <see cref="P:CanGoBack"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string CanGoBackProperty = "CanGoBack";

        /// <summary>
        /// The name of the <see cref="P:CanGoForward"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string CanGoForwardProperty = "CanGoForward";

        /// <summary>
        /// The name of the <see cref="P:EnableDefaultControls"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string EnableDefaultControlsProperty = "EnableDefaultControls";

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
        /// Occurs when the page has been loaded.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler<LoadFinishedEventArgs> LoadFinished
        {
            add { NativeView.LoadFinished += value; }
            remove { NativeView.LoadFinished -= value; }
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
        /// Gets a value indicating whether the browser can navigate back a page in the browsing history.
        /// </summary>
        public bool CanGoBack
        {
            get { return NativeView.CanGoBack; }
        }

        /// <summary>
        /// Gets a value indicating whether the browser can navigate forward a page in the browsing history.
        /// </summary>
        public bool CanGoForward
        {
            get { return NativeView.CanGoForward; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view should automatically create 'Back' and 'Forward' buttons.
        /// </summary>
        public bool EnableDefaultControls
        {
            get { return NativeView.EnableDefaultControls; }
            set { NativeView.EnableDefaultControls = value; }
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
        private IBrowserView NativeView
        {
            get { return (IBrowserView)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.BrowserView&lt;T&gt;"/> class.
        /// </summary>
        public BrowserView()
        {
            Pair = MXContainer.Resolve<IBrowserView>();

            NativeView.ShouldNavigate = ShouldNavigateFrom;
            NativeView.PopoverPresentationStyle = PopoverPresentationStyle.Normal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.BrowserView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model containing the information that is displayed by the view.</param>
        public BrowserView(T model)
            : this()
        {
            Model = model;
        }

        /// <summary>
        /// If able, navigates to the previous page in the browser's history.
        /// </summary>
        public void GoBack()
        {
            NativeView.GoBack();
        }

        /// <summary>
        /// If able, navigates to the next page in the browser's history.
        /// </summary>
        public void GoForward()
        {
            NativeView.GoForward();
        }

        /// <summary>
        /// Launches an external browser application and navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public void LaunchExternal(string url)
        {
            NativeView.LaunchExternal(url);
        }

        /// <summary>
        /// Loads the specified URL in the browser.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        public void Load(string url)
        {
            NativeView.Load(url);
        }

        /// <summary>
        /// Reads the specified string as HTML and loads the result in the browser.
        /// </summary>
        /// <param name="html">The HTML to load.</param>
        public void LoadFromString(string html)
        {
            NativeView.LoadFromString(html);
        }

        /// <summary>
        /// Refreshes the contents of the browser.
        /// </summary>
        public void Refresh()
        {
            NativeView.Refresh();
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


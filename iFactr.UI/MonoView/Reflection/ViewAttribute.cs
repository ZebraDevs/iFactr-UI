using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents a model type from which the framework should generate a view by reflecting on its members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class ViewAttribute : Attribute
    {
        internal ViewType Type { get; private set; }

        /// <summary>
        /// Gets or sets the address to navigate to when the user presses the back button.
        /// </summary>
        public string BackAddress { get; set; }

        /// <summary>
        /// Gets or sets the background color of the view.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the header bar, if there is one.
        /// </summary>
        public string HeaderColor { get; set; }

        /// <summary>
        /// Gets or sets the pane in which the view should be displayed.
        /// </summary>
        public Pane OutputPane { get; set; }

        /// <summary>
        /// Gets or sets the orientation preference for the view.
        /// </summary>
        public PreferredOrientation PreferredOrientations { get; set; }

        /// <summary>
        /// Gets or sets the title for the view.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the color with which to display the title.
        /// </summary>
        public string TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the style in which to render a list view.
        /// </summary>
        public ListViewStyle ListStyle { get; set; }

        /// <summary>
        /// Gets or sets the address to navigate to in a browser view.
        /// </summary>
        public string BrowserUrl { get; set; }

        /// <summary>
        /// Gets or sets whether or not to display the back and forward buttons in a browser view.
        /// </summary>
        public bool EnableBrowserDefaultControls { get; set; }

        /// <summary>
        /// Gets or sets the color of the strokes when drawing in a canvas view.
        /// </summary>
        public string CanvasStrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the strokes when drawing in a canvas view.
        /// </summary>
        public string CanvasStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the color of the separator lines in a list view.
        /// </summary>
        public string ListSeparatorColor { get; set; }

        /// <summary>
        /// Gets or sets the color with which to overlay the selected tab in a tab view.
        /// </summary>
        public string TabSelectionColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAttribute"/> class using a view type of List.
        /// </summary>
        public ViewAttribute()
            : this(ViewType.List)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of view to generate.</param>
        public ViewAttribute(ViewType type)
        {
            Type = type;
            PreferredOrientations = PreferredOrientation.PortraitOrLandscape;
            EnableBrowserDefaultControls = true;
        }
    }

    /// <summary>
    /// Describes the available types of views that are supported by reflective UI.
    /// </summary>
    public enum ViewType : byte
    {
        /// <summary>
        /// A view that supports HTML rendering and web page browsing.
        /// </summary>
        Browser,
        /// <summary>
        /// A view that acts as an ink canvas for drawing on.
        /// </summary>
        Canvas,
        /// <summary>
        /// A view that lays its contents out in a list of cells.
        /// </summary>
        List,
        /// <summary>
        /// A view that contains selectable tab items for separating an application into categories.
        /// </summary>
        Tabs
    }
}

using System;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a unified interface for native views across multiple platforms.
    /// This is the base interface for the <see cref="IListView"/>, <see cref="IGridView"/>,
    /// <see cref="ITabView"/>, <see cref="IBrowserView"/>, and <see cref="ICanvasView"/> interfaces.
    /// </summary>
    public interface IView : IMXView, IPairable, IEquatable<IView>
    {
        /// <summary>
        /// Gets or sets the color of the header bar, if there is one.
        /// </summary>
        Color HeaderColor { get; set; }

        /// <summary>
        /// Gets the current height value of the view in native coordinates.
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        MetadataCollection Metadata { get; }

        /// <summary>
        /// Gets or sets the orientation preference for the view.
        /// </summary>
        PreferredOrientation PreferredOrientations { get; set; }

        /// <summary>
        /// Gets or sets the title for the view.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the color with which to display the title.
        /// </summary>
        Color TitleColor { get; set; }

        /// <summary>
        /// Gets the current width value of the view in native coordinates.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Occurs when the view is being rendered.
        /// </summary>
        event EventHandler Rendering;

        /// <summary>
        /// Sets the background of the view to the specified <see cref="iFactr.UI.Color"/>.
        /// </summary>
        /// <param name="color">The color to set the background to.</param>
        void SetBackground(Color color);

        /// <summary>
        /// Sets the background of the view to the image at the specified file path.
        /// </summary>
        /// <param name="imagePath">The file path of the image to set the background to.</param>
        /// <param name="stretch">The way the image is stretched to fill the available space.</param>
        void SetBackground(string imagePath, ContentStretch stretch);
    }

    /// <summary>
    /// Defines a generic unified interface for native views across multiple platforms.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface IView<T> : IView, IMXView<T> { }
}
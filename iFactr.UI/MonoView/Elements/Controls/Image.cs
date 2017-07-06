using MonoCross;
using MonoCross.Navigation;
using System;
using System.Diagnostics;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element for displaying an image from a file.
    /// </summary>
    public class Image : Control, IImage
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:FilePath"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FilePathProperty = "FilePath";

        /// <summary>
        /// The name of the <see cref="P:Dimensions"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string DimensionsProperty = "Dimensions";

        /// <summary>
        /// The name of the <see cref="P:Stretch"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StretchProperty = "Stretch";
        #endregion

        /// <summary>
        /// Occurs when the user clicks or taps on the image.
        /// </summary>
        public event EventHandler Clicked
        {
            add { NativeControl.Clicked += value; }
            remove { NativeControl.Clicked -= value; }
        }

        /// <summary>
        /// Occurs when the data for the image has been loaded into memory.
        /// </summary>
        public event EventHandler Loaded
        {
            add { NativeControl.Loaded += value; }
            remove { NativeControl.Loaded -= value; }
        }

        /// <summary>
        /// Gets or sets the full path of the file for the image that is being rendered.
        /// </summary>
        public string FilePath
        {
            get { return NativeControl.FilePath; }
            set { NativeControl.FilePath = value; }
        }

        /// <summary>
        /// Gets the width and height of the image.
        /// </summary>
        public Size Dimensions
        {
            get { return NativeControl.Dimensions; }
        }

        /// <summary>
        /// Gets or sets how the image should be stretched to fill the space that is available to it.
        /// </summary>
        public ContentStretch Stretch
        {
            get { return NativeControl.Stretch; }
            set { NativeControl.Stretch = value; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IImage NativeControl
        {
            get { return (IImage)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Image"/> class.
        /// </summary>
        public Image()
            : this(null, ImageCreationOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Image"/> class.
        /// </summary>
        /// <param name="filePath">The full path of the file for the image.</param>
        public Image(string filePath)
            : this(filePath, ImageCreationOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Image"/> class.
        /// </summary>
        /// <param name="filePath">The full path of the file for the image.</param>
        /// <param name="options">Additional options to apply to the image during creation.</param>
        public Image(string filePath, ImageCreationOptions options)
        {
            Pair = MXContainer.Resolve<IImage>(null, options);

            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
            NativeControl.FilePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Image"/> class.
        /// </summary>
        /// <param name="imageData">The in-memory data of the image.  This data will not be cached automatically
        /// and must be added to the cache manually if caching is desired.</param>
        public Image(IImageData imageData)
        {
            Pair = MXContainer.Resolve<IImage>(null, imageData);

            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// Returns the data of the image that is being displayed by this control.
        /// </summary>
        public IImageData GetImageData()
        {
            return NativeControl.GetImageData();
        }
    }
}


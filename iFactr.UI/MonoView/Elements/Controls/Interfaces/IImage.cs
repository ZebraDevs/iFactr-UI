using System;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Defines a UI element for displaying an image from a file.
    /// </summary>
    public interface IImage : IControl
    {
        /// <summary>
        /// Gets or sets the full path of the file for the image that is being rendered.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Gets the width and height of the image.
        /// </summary>
        Size Dimensions { get; }

        /// <summary>
        /// Gets or sets how the image should be stretched to fill the space that is available to it.
        /// </summary>
        ContentStretch Stretch { get; set; }

        /// <summary>
        /// Occurs when the user clicks or taps on the image.
        /// </summary>
        event EventHandler Clicked;

        /// <summary>
        /// Occurs when the data for the image has been loaded into memory.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Returns the data of the image that is being displayed by this control.
        /// </summary>
        MonoCross.IImageData GetImageData();
    }
}

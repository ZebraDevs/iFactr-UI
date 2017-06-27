using System;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field for selecting and displaying images.
    /// </summary>
    public class ImagePickerField : Field
    {
        /// <summary>
        /// Gets or sets the URL of the image to display.
        /// </summary>
        /// <value>The image URL as a <see cref="String"/> value.</value>
        public string ImageUrl
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePickerField"/> class with no submission ID.
        /// </summary>
        public ImagePickerField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePickerField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public ImagePickerField(string id)
        {
            ID = id;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new ImagePickerField Clone()
        {
            return (ImagePickerField)CloneOverride();
        }
    }
}
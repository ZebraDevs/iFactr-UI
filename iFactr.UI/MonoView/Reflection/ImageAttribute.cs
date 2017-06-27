using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents an image that should accompany the user control that is representing the member on screen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ImageAttribute : OptInAttribute
    {
        internal string FilePath { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageAttribute"/> class.
        /// </summary>
        /// <param name="filePath">The full path to the image file that should be used.</param>
        public ImageAttribute(string filePath)
        {
            FilePath = filePath;
        }
    }
}


using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a layer for displaying local or web-based content.
    /// </summary>
    /// <remarks>
    /// Browser layers are used to navigate to and display external content from within the application.  
    /// </remarks>
    public class Browser : iLayer
    {
        /// <summary>
        /// Gets or sets the URL to navigate to.
        /// </summary>
        /// <value>The URL as a <see cref="String"/> value.</value>
        public string Url { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class using the navigation URL provided.
        /// </summary>
        public Browser() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class using the navigation URL provided.
        /// </summary>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        public Browser(string url)
        {
            Url = url;
        }
    }
}
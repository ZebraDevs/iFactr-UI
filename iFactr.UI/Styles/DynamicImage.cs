namespace iFactr.Core.Styles
{
    /// <summary>
    /// Represents a 9-patch image that is able to scale or tile the edges and center.
    /// </summary>
    public struct DynamicImage
    {
        /// <summary>
        /// The file name of the image. File name only. Does not include any portion of the path.
        /// </summary>
        public string FileName;

        /// <summary>
        /// The resize method.
        /// </summary>
        public DynamicResizeMethod ResizeMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicImage"/> struct.
        /// </summary>
        /// <param name="fileName">The file name of the image excluding any part of the path.</param>
        /// <param name="resizeMethod">The resize method.</param>
        public DynamicImage(string fileName, DynamicResizeMethod resizeMethod)
        {
            FileName = fileName;
            ResizeMethod = resizeMethod;
        }
    }

    /// <summary>
    /// The method in which a <see cref="DynamicImage"/> fills areas larger than the image.
    /// </summary>
    public enum DynamicResizeMethod
    {
        /// <summary>
        /// The edges and center are scaled to fill the available area.
        /// </summary>
        Stretch,
        /// <summary>
        /// The edges and center are tiled continuously until the available area is filled.
        /// </summary>
        Tile,
    }
}
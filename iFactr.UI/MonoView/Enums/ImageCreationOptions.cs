using System;

namespace iFactr.UI
{
    /// <summary>
    /// Describes the available options that can be applied during image creation.
    /// </summary>
    [Flags]
    public enum ImageCreationOptions : byte
    {
        /// <summary>
        /// No options are specified.  This is the default value.
        /// </summary>
        None = 0,
        /// <summary>
        /// The image should not be cached.
        /// </summary>
        IgnoreCache = 1
    }
}

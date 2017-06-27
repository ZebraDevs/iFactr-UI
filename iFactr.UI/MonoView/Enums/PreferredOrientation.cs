using System;

namespace iFactr.UI
{
    /// <summary>
    /// Describes the orientation preference for a view.  Platforms that support this
    /// will lock the view in the specified orientation, preventing it from rotating.
    /// </summary>
	[Flags]
    public enum PreferredOrientation : byte
    {
        /// <summary>
        /// The view should be kept in portrait orientation.
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// The view should be kept in landscape orientation.
        /// </summary>
        Landscape = 2,
        /// <summary>
        /// The view can rotate to either portrait or landscape orientation.
        /// </summary>
        PortraitOrLandscape = 3
    }
}

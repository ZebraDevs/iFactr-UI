using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Defines a layer that is displayed in a modal popover window on a large form-factor device.
    /// </summary>
    [Obsolete("Use the PreferredPane attribute instead.")]
    public interface IPopoverLayer
    {
        /// <summary>
        /// Gets or sets whether to display the layer in a fullscreen modal window.
        /// </summary>
        bool IsFullscreen { get; set; }
    }
}
namespace iFactr.UI
{
    /// <summary>
    /// Describes how a view should be presented when displayed in a popover pane.
    /// </summary>
    public enum PopoverPresentationStyle : byte
    {
        /// <summary>
        /// The view is presented in either a full-screen pane or a smaller modal window, depending on the platform.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The view is presented in a full-screen pane.
        /// </summary>
        FullScreen = 1
    }
}

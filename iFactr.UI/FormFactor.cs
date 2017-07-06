namespace iFactr.Core
{
    /// <summary>
    /// The available application layouts for large form-factor devices.
    /// </summary>
    public enum FormFactor
    {
        /// <summary>
        /// The screen is split between the master pane and a detail pane.
        /// </summary>
        SplitView,
        /// <summary>
        /// No detail pane.  Layers assigned to the detail pane will appear in the master pane instead.
        /// </summary>
        Fullscreen,
        /// <summary>
        /// A layout consisting of sliding panels that are stacked on top of each other.
        /// </summary>
        Accordion,
    }
}
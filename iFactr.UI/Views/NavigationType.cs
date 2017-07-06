namespace iFactr.UI
{
    /// <summary>
    /// The types of navigation.
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// The navigation occurred because the back button has been pressed.
        /// </summary>
        Back,
        /// <summary>
        /// The navigation occurred because an iApp navigation has been initiated.
        /// </summary>
        Forward,
        /// <summary>
        /// The navigation occurred because a tab has been selected.
        /// </summary>
        Tab,
    }
}
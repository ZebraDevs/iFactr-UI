namespace iFactr.UI
{
    /// <summary>
    /// Describes the available button combinations for an alert dialog.
    /// </summary>
    public enum AlertButtons : byte
    {
        /// <summary>
        /// The alert contains a single 'OK' button.
        /// </summary>
        OK = 0,
        /// <summary>
        /// The alert contains an 'OK' button and a 'Cancel' button.
        /// </summary>
        OKCancel = 1,
        /// <summary>
        /// The alert contains a 'Yes' button and a 'No' button.
        /// </summary>
        YesNo = 2,
    }
}
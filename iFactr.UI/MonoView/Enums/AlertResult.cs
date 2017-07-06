namespace iFactr.UI
{
    /// <summary>
    /// Describes the possible results of an alert dialog.
    /// </summary>
    public enum AlertResult : byte
    {
        /// <summary>
        /// The primary or 'OK' button was pressed.
        /// </summary>
        OK = 0,
        /// <summary>
        /// The secondary or 'Cancel' button was pressed.
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// The primary or 'Yes' button was pressed.
        /// </summary>
        Yes = 2,
        /// <summary>
        /// The secondary or 'No' button was pressed.
        /// </summary>
        No = 3,
    }
}
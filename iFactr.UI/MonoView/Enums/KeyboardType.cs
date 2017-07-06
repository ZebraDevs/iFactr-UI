namespace iFactr.UI
{
    /// <summary>
    /// Describes the type of soft keyboard to display when a text entry control has focus.
    /// </summary>
    public enum KeyboardType : byte
    {
        /// <summary>
        /// The default keyboard containing letters and numbers.
        /// </summary>
        AlphaNumeric,
        /// <summary>
        /// A PIN-entry keyboard.
        /// </summary>
        PIN,
        /// <summary>
        /// A keyboard with numbers and symbols.
        /// </summary>
        Symbolic,
        /// <summary>
        /// A keyboard for entering URLs and email addresses.
        /// </summary>
        Email,
    }
}
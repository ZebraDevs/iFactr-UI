using System;

namespace iFactr.UI
{
    /// <summary>
    /// Describes automatic completion behavior for text that is entered into a text entry control.
    /// </summary>
    [Flags]
    public enum TextCompletion : byte
    {
        /// <summary>
        /// The text should not be handled in any way.
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// Word suggestions should be offered while entering text.  Not all platforms support this.
        /// </summary>
        OfferSuggestions = 1,
        /// <summary>
        /// Letters should be automatically capitalized when appropriate.  Not all platforms support this.
        /// </summary>
        AutoCapitalize = 2,
    }
}
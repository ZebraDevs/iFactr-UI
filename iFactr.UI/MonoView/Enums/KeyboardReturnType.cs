using System;

namespace iFactr.UI
{
    /// <summary>
    /// Describes how to present the 'Return' key of a soft keyboard when a text entry control has focus.
    /// </summary>
    public enum KeyboardReturnType : byte
    {
        /// <summary>
        /// The Return key should be displayed as its default value, typically a 'Return' or 'Enter' button.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The Return key should be displayed as a 'Next' button.
        /// If this option is not available on the target platform, <see cref="KeyboardReturnType.Default"/> will be used instead.
        /// </summary>
        Next = 1,
        /// <summary>
        /// The Return key should be displayed as a 'Done' button.
        /// If this option is not available on the target platform, <see cref="KeyboardReturnType.Default"/> will be used instead.
        /// </summary>
        Done = 2,
        /// <summary>
        /// The Return key should be displayed as a 'Go' button.
        /// If this option is not available on the target platform, <see cref="KeyboardReturnType.Default"/> will be used instead.
        /// </summary>
        Go = 3,
        /// <summary>
        /// The Return key should be displayed as a 'Search' button.
        /// If this option is not available on the target platform, <see cref="KeyboardReturnType.Default"/> will be used instead.
        /// </summary>
        Search = 4
    }
}

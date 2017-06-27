using System;

namespace iFactr.Core
{
    /// <summary>
    /// The available mobile targets available in the iFactr framework.
    /// </summary>
    [Flags]
    public enum MobileTarget
    {
        /// <summary>
        /// A Microsoft Azure-based HTML/CSS/JS rich Web target.
        /// </summary>
        Cloud = 1,
        /// <summary>
        /// A Microsoft Windows Mobile and .NET Compact Framework target.
        /// </summary>
        Compact = 2,
        /// <summary>
        /// A Microsoft Windows Console target.
        /// </summary>
        Console = 4,
        /// <summary>
        /// An Google Android OS target.
        /// </summary>
        Android = 8,
        /// <summary>
        /// An Apple iOS target.
        /// </summary>
        Touch = 16,
        /// <summary>
        /// A HTML Large-format Web target.
        /// </summary>
        Web = 32,
        /// <summary>
        /// A Mobile XHTML version for basic devices.
        /// </summary>
        Wap = 64,
        /// <summary>
        /// A HTML/CSS/Javascript rich Mobile Web target.
        /// </summary>
        WebKit = 128,
        /// <summary>
        /// A Microsoft Windows OS Laptop/Desktop target.
        /// </summary>
        Windows = 256,
        /// <summary>
        /// A Microsoft Windows 8 'Metro' target.
        /// </summary>
        Metro = 512,
        /// <summary>
        /// A Microsoft Windows Phone target.
        /// </summary>
        WinPhone = 1024,
        /// <summary>
        /// The target is unknown.
        /// </summary>
        Unknown = 0
    }
}
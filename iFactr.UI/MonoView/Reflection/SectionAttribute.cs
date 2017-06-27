using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents the beginning of a new group of cells.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SectionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the color of the footer for the section.
        /// </summary>
        public string FooterColor { get; set; }

        /// <summary>
        /// Gets or sets the text to display underneath the section.
        /// </summary>
        public string FooterText { get; set; }

        /// <summary>
        /// Gets or sets the color of the footer text for the section.
        /// </summary>
        public string FooterTextColor { get; set; }
        
        /// <summary>
        /// Gets or sets the color of the header for the section.
        /// </summary>
        public string HeaderColor { get; set; }
        
        /// <summary>
        /// Gets or sets the text to display above the section.
        /// </summary>
        public string HeaderText { get; set; }
        
        /// <summary>
        /// Gets or sets the color of the header text for the section.
        /// </summary>
        public string HeaderTextColor { get; set; }
    }
}

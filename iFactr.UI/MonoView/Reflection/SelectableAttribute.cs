using System;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents a cell or button that should perform a navigation when selected by the user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SelectableAttribute : OptInAttribute
    {
        /// <summary>
        /// Gets or sets the color with which to highlight the cell when it is selected.
        /// </summary>
        public string SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets which visual elements to use to indicate that the cell is selectable or has been selected.
        /// </summary>
        public SelectionStyle SelectionStyle { get; set; }

        internal string NavigationAddress { get; private set; }
        
        internal string AccessoryAddress { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableAttribute"/> class.
        /// </summary>
        /// <param name="navigationAddress">The address to navigate to when the element has been selected.</param>
        public SelectableAttribute(string navigationAddress)
            : this(navigationAddress, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableAttribute"/> class.
        /// </summary>
        /// <param name="navigationAddress">The address to navigate to when the element has been selected.</param>
        /// <param name="accessoryAddress">The address to navigate to when selecting the cell's accessory.</param>
        public SelectableAttribute(string navigationAddress, string accessoryAddress)
        {
            NavigationAddress = navigationAddress;
            AccessoryAddress = accessoryAddress;
            SelectionStyle = SelectionStyle.Default;
        }
    }
}


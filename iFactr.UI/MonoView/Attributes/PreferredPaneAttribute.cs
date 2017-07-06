using System;

namespace iFactr.UI
{
    /// <summary>
    /// Indicates that a view has a preference for which pane it will be displayed in.
    /// If the preferred pane is of an equal or higher ordinal value to the current top pane,
    /// the view will be displayed in the preferred pane; otherwise, it will be displayed in the top pane.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class PreferredPaneAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="iFactr.UI.Pane"/> that the view prefers to be displayed in.
        /// </summary>
        public Pane Pane { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferredPaneAttribute"/> class.
        /// </summary>
        /// <param name="pane">The <see cref="iFactr.UI.Pane"/> that the view prefers to be displayed in.</param>
        public PreferredPaneAttribute(Pane pane)
        {
            Pane = pane;
        }
    }
}

using System.Xml.Serialization;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a button control with an action type of cancel.
    /// </summary>
    public class CancelButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelButton"/> class.
        /// </summary>
        public CancelButton()
        {
            base.Action = ActionType.Cancel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelButton"/> class.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public CancelButton(string text)
            : base(text)
        {
            base.Action = ActionType.Cancel;
        }

        /// <summary>
        /// Gets the action type for this instance.
        /// </summary>
        /// <value>The action type as an <see cref="ActionType"/> value.</value>
        /// <remarks>Because this is a CancelButton, this will always return ActionType.Cancel.</remarks>
        [XmlIgnore]
        public sealed override ActionType Action { get { return ActionType.Cancel; } }
    }
}
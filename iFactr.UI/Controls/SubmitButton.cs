using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a button control with an action type of submit.
    /// </summary>
    [Obsolete("Buttons submit by default.")]
    public class SubmitButton : Button
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitButton"/> class.
        /// </summary>
        public SubmitButton()
        {
            Action = ActionType.Submit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitButton"/> class.
        /// </summary>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        public SubmitButton(string address)
            : base(null, address)
        {
            Action = ActionType.Submit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitButton"/> class using the URL address provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        public SubmitButton(string text, string address)
            : base(text, address)
        {
            Action = ActionType.Submit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitButton"/> class using the URL address provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        /// <param name="parameters">An optional set of parameters to pass through.</param>
        public SubmitButton(string text, string address, Dictionary<string, string> parameters)
            : base(text, address, parameters)
        {
            Action = ActionType.Submit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitButton"/> class using the URL address and async setting provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="string"/> representing the URL address value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public SubmitButton(string text, string address, bool async)
            : base(text, address, async)
        {
            Action = ActionType.Submit;
        }

        #endregion

        /// <summary>
        /// Gets the action type for this instance.
        /// </summary>
        /// <value>The action type as an <see cref="ActionType"/> value.</value>
        /// <remarks>Because this is a SubmitButton, this will always return ActionType.Submit.</remarks>
        [XmlIgnore]
        public sealed override ActionType Action { get { return ActionType.Submit; } }

        /// <summary>
        /// Gets the position of this instance within the layer.
        /// </summary>
        /// <value>The button position as a <see cref="Button.Position"/> value.</value>
        [Obsolete("This property is no longer honored")]
        public sealed override Position ButtonPosition { get { return Position.TopRight; } }
    }
}
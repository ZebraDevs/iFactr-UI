using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a control that performs a navigation when pressed.
    /// </summary>
    /// <remarks>
    /// The Button Control is a navigation control rendered as a button on the target
    /// platform.  Generally all buttons are rendered in the layer header, but may be
    /// added to other parts of a layer depending upon the desired behavior.  All
    /// buttons are in essence navigation links that load a layer from the Navigation
    /// Map in the same manner as a list or menu item does.  
    /// <para></para>
    /// <para><b>Position Property</b></para>
    /// <para>The iFactr Framework automatically determines the position of buttons on a
    /// layer based upon the button type and usage.  The Position property give the
    /// developer the opportunity to override this behavior and force a specific
    /// position.  The default value for Position is NotSpecified.</para>
    /// <para></para>
    /// <para><b>ActionType Property</b></para>
    /// <para>The ActionType property provides the framework with additional information
    /// regarding the intended action of the button, the default value is
    /// Undefined.</para>
    /// </remarks>
    public class Button : Core.Controls.Link
    {
        #region Enumerations
        /// <summary>
        /// The available button control positions within a layer.
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// Positions the button in the top-left corner of the layer.
            /// </summary>
            NotSpecified,
            /// <summary>
            /// Positions the button in the top-left corner of the layer.
            /// </summary>
            TopLeft,
            /// <summary>
            /// Positions the button in the top-right corner of the layer.
            /// </summary>
            TopRight,
            /// <summary>
            /// Positions the button in-line at the relative position defined in the layer code.
            /// </summary>
            InLine,
        }
        /// <summary>
        /// The available button actions.
        /// </summary>
        [XmlType("ButtonActionType", TypeName = "ButtonActionType", Namespace = "Button")]
        public enum ActionType
        {
            /// <summary>
            /// Specifies the button action is undefined.
            /// </summary>
            Undefined,
            /// <summary>
            /// Specifies the button is used for add or create operations.
            /// </summary>
            Add,
            /// <summary>
            /// Specifies the button is used to cancel an operation.
            /// </summary>
            Cancel,
            /// <summary>
            /// Specifies the button is used for an edit operation.
            /// </summary>
            Edit,
            /// <summary>
            /// Specifies the button is used for a delete operation.
            /// </summary>
            Delete,
            /// <summary>
            /// Specifies the button is used to display additional information within the layer context.
            /// </summary>
            More,
            /// <summary>
            /// Specifies the button is used to submit a form.
            /// </summary>
            Submit,
            /// <summary>
            /// Specifies that no button should be displayed.
            /// </summary>
            None,
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the action type for this instance.
        /// </summary>
        /// <value>The action type as an <see cref="ActionType"/> value.</value>
        [XmlIgnore]
        public new virtual ActionType Action
        {
            get { return (ActionType)base.Action; }
            set { base.Action = (Controls.ActionType)value; }
        }
        /// <summary>
        /// Gets or sets the position of this instance within the layer.
        /// </summary>
        /// <value>The button position as a <see cref="Position"/> value.</value>
        [Obsolete("This property is no longer honored")]
        public virtual Position ButtonPosition { get; set; }
        /// <summary>
        /// Gets or sets the ID for this instance.
        /// </summary>
        /// <value>The ID as a <see cref="String"/> value.</value>
        public string ID { get; set; }
        /// <summary>
        /// Gets or sets the text to be displayed for this instance.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public override string Text
        {
            get
            {
                if (base.Text == null)
                {
                    switch (Action)
                    {
                        case ActionType.Add:
                            base.Text = iApp.Factory.GetResourceString("Add") ?? "Add";
                            break;
                        case ActionType.Cancel:
                            base.Text = iApp.Factory.GetResourceString("Cancel") ?? "Cancel";
                            break;
                        case ActionType.Edit:
                            base.Text = iApp.Factory.GetResourceString("Edit") ?? "Edit";
                            break;
                        case ActionType.Delete:
                            base.Text = iApp.Factory.GetResourceString("Delete") ?? "Delete";
                            break;
                        case ActionType.More:
                            base.Text = iApp.Factory.GetResourceString("More") ?? "More";
                            break;
                        case ActionType.Submit:
                            base.Text = iApp.Factory.GetResourceString("Submit") ?? "Submit";
                            break;
                        case ActionType.Undefined:
                        case ActionType.None:
                            break;
                    }
                }
                return base.Text;
            }
            set { base.Text = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
            Action = ActionType.Submit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        public Button(string text) 
        {
            Action = ActionType.Submit;
            Text = text;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class using the URL address provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        public Button(string text, string address)
            : this(text)
        {
            Address = address;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class using the URL address provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        /// <param name="parameters">An optional set of parameters to pass through.</param>
        public Button(string text, string address, Dictionary<string, string> parameters)
            : this(text)
        {
            Address = address;
            Parameters = parameters;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class using the URL address and async setting provided.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address value.</param>
        /// <param name="async">If <c>true</c>, sets the link rev value to Async.</param>
        public Button(string text, string address, bool async)
            : this(text, address)
        {
            if (!async) RequestType = UI.RequestType.ClearPaneHistory;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new Button Clone()
        {
            return (Button)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        protected override object CloneOverride()
        {
            var newButton = (Button)base.CloneOverride();

            if (Image != null)
            {
                newButton.Image = Image.Clone();
            }
            if (Parameters != null)
            {
                newButton.Parameters = new Dictionary<string, string>(Parameters);
            }

            return newButton;
        }
        #endregion
    }
}
using System;
using iFactr.Core.Controls;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field that initiates a navigation when selected.
    /// </summary>
    public class NavigationField : Field
    {
        #region Properties
        /// <summary>
        /// Gets or sets the text to be displayed in a confirmation dialog before navigating.
        /// </summary>
        /// <value>The confirmation text as a <see cref="String"/> value.</value>
        public string ConfirmationText { get { return Link.ConfirmationText; } set { Link.ConfirmationText = value; } }
        /// <summary>
        /// Gets or sets the label text to display.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public override string Text { get { return Link.Text; } set { Link.Text = value; } }
        /// <summary>
        /// Gets or sets the label text to display.
        /// </summary>
        /// <value>The label as a <see cref="String"/> value.</value>
        public override string Label { get { return Link.Text; } set { Link.Text = value; } }
        /// <summary>
        /// Gets the link that this instance will navigate to when selected.
        /// </summary>
        /// <value>The link as a <see cref="Button"/>.</value>
        public Link Link
        {
            get { return _link; }
            set { _link = value ?? new Button(); }
        }
        private Link _link;
        /// <summary>
        /// Gets or sets the rev setting for this instance.
        /// </summary>
        /// <value>The rev setting as a <see cref="Controls.Link.Rev"/> value.</value>
        [Obsolete("Use RequestType instead.")]
        public Link.Rev RevSetting { get { return Link.RevSetting; } set { Link.RevSetting = value; } }
        
        /// <summary>
        /// Gets or sets the request type for this instance.
        /// </summary>
        public UI.RequestType RequestType { get { return Link.RequestType; } set { Link.RequestType = value; } }

        /// <summary>
        /// Gets or sets the action type for this instance.
        /// </summary>
        /// <value>The action as a <see cref="Button.ActionType"/> value.</value>
        public Button.ActionType Action { get { return (Button.ActionType)Link.Action; } set { Link.Action = (ActionType)value; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationField"/> class.
        /// </summary>
        public NavigationField()
        {
            Link = new Button();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public NavigationField(string id)
        {
            ID = id;
            Link = new Button();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationField"/> class using the ID and address provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name="address">A <see cref="String"/> representing the URL address to navigate to.</param>
        public NavigationField(string id, string address)
        {
            ID = id;
            Link = new Button(null, address);
        }

        #endregion

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public new NavigationField Clone()
        {
            return (NavigationField)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected override object CloneOverride()
        {
            var nf = (NavigationField)base.CloneOverride();
            nf.Link = Link == null ? null : Link.Clone();
            return nf;
        }
    }
}
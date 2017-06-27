using iFactr.UI;
using MonoCross.Navigation;
using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/*
This namespace contains all of the classes of various abstract user interface
controls that are not limited to a specific use. For example, a Button, Icon, or
Link can be utilized in many ways like added to an iItem, an iForm, or even
an iLayer, while the iFactr.Forms.TextField can only be placed in a Fieldset
within an iForm. 
*/
namespace iFactr.Core.Controls
{
    /// <summary>
    /// Represents a control that points to a URL address and can be navigated to.
    /// </summary>
    [Obsolete("Use iFactr.UI.Link instead.")]
    [XmlType("CoreLink", TypeName = "CoreLink", Namespace = "Core")]
    [XmlInclude(typeof(Button))]
    [XmlInclude(typeof(SubmitButton))]
    [XmlInclude(typeof(CancelButton))]
    public class Link : PanelItem
    {
        #region Enumerations
        /// <summary>
        /// The possible rev values of a link.
        /// </summary>
        public enum Rev
        {
            /// <summary>
            /// The link delivers asynchronous content.
            /// </summary>
            Async,
            /// <summary>
            /// The link delivers media content, (audio or video).
            /// </summary>
            Media,
            /// <summary>
            /// No rev value specified.
            /// </summary>
            None,
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the action type of this instance.
        /// </summary>
        public virtual ActionType Action { get; set; }
        /// <summary>
        /// Gets or sets the URL address that will be navigated to.
        /// </summary>
        /// <value>The URL address as a <see cref="String"/> value.</value>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the text to be displayed in a confirmation dialog before navigating.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public string ConfirmationText { get; set; }
        /// <summary>
        /// Gets or sets the image to be displayed in conjunction with the link.
        /// </summary>
        /// <value>The image as an <see cref="Icon"/>.</value>
        public Icon Image { get; set; }
        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, before the load indicator is displayed when navigating.
        /// Any value below 0 means the load indicator will not display.
        /// </summary>
        public double LoadIndicatorDelay { get; set; }
        /// <summary>
        /// Gets or sets the title to display for the load indicator when navigating.
        /// </summary>
        public string LoadIndicatorTitle { get; set; }
        /// <summary>
        /// Gets or sets whether this instance should open a new browser window when targeting the Web/Webkit bindings.
        /// </summary>
        /// <value><c>true</c> to launch the end link outside of the framework; otherwise <c>false</c>.</value>
        [Obsolete("Use RequestType instead.")]
        public bool NewWindow
        {
            get { return requestType == UI.RequestType.NewWindow; }
            set { requestType = value ? UI.RequestType.NewWindow : UI.RequestType.ClearPaneHistory; }
        }

        /// <summary>
        /// Gets or sets an optional collection of parameters to pass through when navigating.
        /// </summary>
        public SerializableDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the rev setting for this instance.
        /// </summary>
        /// <value>The rev setting as a <see cref="Rev"/> value.</value>
        [Obsolete("Use RequestType instead.")]
        public Rev RevSetting
        {
            get { return requestType == UI.RequestType.NewWindow ? Rev.None : (Rev)requestType; }
            set { requestType = (RequestType)value; }
        }

        /// <summary>
        /// Gets or sets the request type for this instance.
        /// </summary>
        public RequestType RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }
        private RequestType requestType;

        /// <summary>
        /// Gets or sets the text to be displayed for this instance.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public virtual string Text { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        public Link()
        {
            Parameters = new Dictionary<string, string>();
            LoadIndicatorDelay = iApp.Factory != null ? iApp.Factory.DefaultLoadIndicatorDelay : 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class with the specified URL address.
        /// </summary>
        /// <param name="address">The URL address to navigate to.</param>
        public Link(string address)
            : this()
        {
            Address = address;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class with the specified URL address and parameters.
        /// </summary>
        /// <param name="address">The URL address to navigate to.</param>
        /// <param name="parameters">The parameters to pass through when navigating.</param>
        public Link(string address, Dictionary<string, string> parameters)
            : this()
        {
            Address = address;
            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class with the specified URL address.
        /// </summary>
        /// <param name="address">The URL address to navigate to.</param>
        /// <param name="newWindow"><c>true</c> to open the link in a new window; otherwise <c>false</c>.</param>
        public Link(string address, bool newWindow)
            : this()
        {
            Address = address;
            NewWindow = newWindow;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new Link Clone()
        {
            return (Link)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        protected override object CloneOverride()
        {
            var l = (Link)base.CloneOverride();

            if (Image != null)
            {
                l.Image = Image.Clone();
            }
            if (Parameters != null)
            {
                l.Parameters = new Dictionary<string, string>(Parameters);
            }

            return l;
        }

        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representing this instance in HTML.</returns>
        public override string GetHtml()
        {
            string text = Text.CleanEntities();
            if (string.IsNullOrEmpty(text)) text = Address.CleanEntities();
            if (Image != null && !string.IsNullOrEmpty(Image.Location))
            {
                Image.Name = text;
                text = Image.GetHtml();
            }

            //The confirmation text will probably only work in web-based targets. It should be bad practice to include a link in an IHtml item; opt for an action button instead.
            return string.Format("<a href=\"{0}\"{1}{2}>{3}</a>", Address,
                string.IsNullOrEmpty(ConfirmationText) ? string.Empty :
                    " onclick=\"if(!confirm('" + ConfirmationText + "'))return false;)\"",
                    NewWindow ? " target=\"_blank\"" : null, text);
        }
        #endregion
    }
}
using iFactr.Core;
using iFactr.Core.Controls;
using MonoCross.Navigation;
using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a control that points to a URL address and can be navigated to.
    /// </summary>
    [XmlType("Link", TypeName = "Link", Namespace = "UI")]
    public sealed class Link : PanelItem
    {
        #region Properties
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
        /// Gets or sets the path to the image to be displayed.
        /// </summary>
        /// <value>The image as a <see cref="String"/>.</value>
        public string ImagePath { get; set; }
        /// <summary>
        /// Gets or sets the request type for this instance.
        /// </summary>
        /// <value>The request type as a <see cref="RequestType"/> value.</value>
        public RequestType RequestType { get; set; }
        /// <summary>
        /// Gets or sets the action type of this instance.
        /// </summary>
        public ActionType Action { get; set; }
        /// <summary>
        /// Gets or sets the text to be displayed for this instance.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public string Text
        {
            get
            {
                if (_text == null)
                {
                    switch (Action)
                    {
                        case ActionType.Add:
                            _text = iApp.Factory.GetResourceString("Add") ?? "Add";
                            break;
                        case ActionType.Cancel:
                            _text = iApp.Factory.GetResourceString("Cancel") ?? "Cancel";
                            break;
                        case ActionType.Edit:
                            _text = iApp.Factory.GetResourceString("Edit") ?? "Edit";
                            break;
                        case ActionType.Delete:
                            _text = iApp.Factory.GetResourceString("Delete") ?? "Delete";
                            break;
                        case ActionType.More:
                            _text = iApp.Factory.GetResourceString("More") ?? "More";
                            break;
                        case ActionType.Submit:
                            _text = iApp.Factory.GetResourceString("Submit") ?? "Submit";
                            break;
                        case ActionType.Undefined:
                        case ActionType.None:
                            break;
                    }
                }
                return _text;
            }
            set { _text = value; }
        }
        private string _text;

        /// <summary>
        /// Gets or sets an optional collection of parameters to pass through when navigating.
        /// </summary>
        public SerializableDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, before the load indicator is displayed when navigating.
        /// Any value below 0 means the load indicator will not display.
        /// </summary>
        public double LoadIndicatorDelay { get; set; }

        /// <summary>
        /// Gets or sets the title to display for the load indicator when navigating.
        /// </summary>
        public string LoadIndicatorTitle { get; set; }
        #endregion

        #region Ctors
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
        /// <param name="requestType">The link's request type.</param>
        public Link(string address, RequestType requestType)
            : this()
        {
            Address = address;
            RequestType = requestType;
        }
        #endregion

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
            if (!string.IsNullOrEmpty(ImagePath))
            {
                text = new Icon(ImagePath) { Name = text }.GetHtml();
            }

            //The confirmation text will probably only work in web-based targets. It should be bad practice to include a link in an IHtml item; opt for an action button instead.
            return string.Format("<a href=\"{0}\"{1}{2}>{3}</a>", Address,
                string.IsNullOrEmpty(ConfirmationText) ? string.Empty :
                    " onclick=\"if(!confirm('" + ConfirmationText + "'))return false;)\"",
                    RequestType == RequestType.NewWindow ? " target=\"_blank\"" : null, text);
        }

        /// <summary>
        /// Converts a <see cref="Link"/> to the older <see cref="iFactr.Core.Controls.Link"/>
        /// </summary>
        /// <param name="link">The link to convert.</param>
        /// <returns>The converted link.</returns>
        public static implicit operator Core.Controls.Link(Link link)
        {
            return link == null ? null : new Core.Controls.Link()
            {
                Action = (Core.Controls.ActionType)link.Action,
                Address = link.Address,
                ConfirmationText = link.ConfirmationText,
                Image = link.ImagePath == null ? null : new Icon(link.ImagePath),
                LoadIndicatorDelay = link.LoadIndicatorDelay,
                LoadIndicatorTitle = link.LoadIndicatorTitle,
                NewWindow = link.RequestType == RequestType.NewWindow,
                Parameters = link.Parameters == null ? null : new Dictionary<string, string>(link.Parameters),
                RequestType = link.RequestType,
                Text = link.Text
            };
        }

        /// <summary>
        /// Converts a <see cref="Core.Controls.Link"/> to the updated <see cref="Link"/>
        /// </summary>
        /// <param name="link">The link to convert.</param>
        /// <returns>The converted link.</returns>
        public static implicit operator Link(Core.Controls.Link link)
        {
            return link == null ? null : new Link()
            {
                Action = (ActionType)link.Action,
                Address = link.Address,
                ConfirmationText = link.ConfirmationText,
                ImagePath = link.Image == null ? null : link.Image.Location,
                LoadIndicatorDelay = link.LoadIndicatorDelay,
                LoadIndicatorTitle = link.LoadIndicatorTitle,
                Parameters = link.Parameters == null ? null : new Dictionary<string, string>(link.Parameters),
                RequestType = link.RequestType,
                Text = link.Text
            };
        }
    }

    /// <summary>
    /// The available types of actions to perform.
    /// </summary>
    [XmlType("ActionType", TypeName = "ActionType", Namespace = "UI")]
    public enum ActionType
    {
        /// <summary>
        /// An undefined or default action.
        /// </summary>
        Undefined,
        /// <summary>
        /// The action performs an addition or creation operation.
        /// </summary>
        Add,
        /// <summary>
        /// The action performs a cancellation operation.
        /// </summary>
        Cancel,
        /// <summary>
        /// The action performs an edit operation.
        /// </summary>
        Edit,
        /// <summary>
        /// The action performs a deletion operation.
        /// </summary>
        Delete,
        /// <summary>
        /// The action performs an operation for displaying additional information.
        /// </summary>
        More,
        /// <summary>
        /// The action performs a form submission operation.
        /// </summary>
        Submit,
        /// <summary>
        /// No action is performed.
        /// </summary>
        None,
    }

    /// <summary>
    /// The possible request types for a <see cref="Link"/>.
    /// </summary>
    public enum RequestType
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
        /// The history of the output pane is cleared before displaying the navigated view.
        /// </summary>
        ClearPaneHistory,
        /// <summary>
        /// The link delivers content in a new window.
        /// </summary>
        NewWindow,
    }
}
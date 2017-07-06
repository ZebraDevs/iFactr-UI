using System;
using System.Collections.Generic;
using iFactr.Core.Controls;
using iFactr.Core.Forms;
using iFactr.UI;
using Link = iFactr.UI.Link;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a layer that accepts credentials and performs authentication logic.
    /// </summary>
    [PreferredPane(Pane.Popover)]
    public abstract class LoginLayer : FormLayer
    {
        private static readonly string DefaultErrorText = iApp.Factory.GetResourceString("DefaultError") ?? "Authentication Failed";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginLayer"/> class.
        /// </summary>
        public LoginLayer()
        {
            PopoverPresentationStyle = PopoverPresentationStyle.FullScreen;
        }

        /// <summary>
        /// The base implementation will create the UI with username and password fields
        /// and disable the back button.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey, TValue&gt;"/> representing any parameters.</param>
        /// <remarks>
        /// It is not advised to override this method to build custom UI.  If this method
        /// is overridden to provide some additional logic processing, it is necessary
        /// to call base.Load() in the override.
        /// </remarks>
        public override void Load(Dictionary<string, string> parameters)
        {
            //if (BackButton == null)
            //    BackButton = new Button(string.Empty) { Action = Button.ActionType.None, };
            if (parameters.ContainsKey(UserKey))
                DefaultUsername = parameters[UserKey];

            if (DefaultUsername != null && parameters.ContainsKey(PasswordKey))
            {
                if (LogIn(DefaultUsername, parameters[PasswordKey]))
                {
                    if (LoginLink != null)
                    {
                        CancelLoadAndNavigate(LoginLink);
                    }
                    return;
                }

                //set text indicating login failure if none supply by business logic
                if (ErrorText == null)
                    ErrorText = DefaultErrorText;

                // add fieldset with error message as default implementation
                Items.Add(new Fieldset
                {
                    new LabelField { Label = ErrorText, },
                });
            }

            Title = ErrorTitle ?? iApp.Factory.GetResourceString("LoginTitle") ?? "Login";

            Items.Add(new Fieldset
            {
                new TextField(UserKey) { Label = UsernameLabel, Text = DefaultUsername ?? string.Empty, TextCompletion = TextCompletion.Disabled, },
                new TextField(PasswordKey) { IsPassword = true, Label = PasswordLabel, },
            });
            ActionButtons.Add(new SubmitButton(iApp.Factory.GetResourceString("LoginAction") ?? "Login", NavContext.NavigatedUrl)
            {
                RequestType = RequestType.NewWindow,
            });
            BackButton = new Button { Action = Button.ActionType.None, };
        }

        /// <summary>
        /// Performs authentication logic and returns whether logging in was successful.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password paired with the username.</param>
        /// <returns><c>true</c> if login succeeded; otherwise <c>false</c>.</returns>
        /// <remarks>If you return false in this method, you may find it useful to set ErrorTitle and/or ErrorText.</remarks>
        public abstract bool LogIn(string username, string password);

        /// <summary>
        /// Gets or sets the link to navigate to on a successful login.
        /// </summary>
        public virtual Link LoginLink
        {
            get
            {
                return iApp.Session.ContainsKey(LoginLinkKey) ? (Link)iApp.Session[LoginLinkKey] : null;
            }
            set
            {
                iApp.Session[LoginLinkKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the link to navigate to on a successful login.
        /// </summary>
        [Obsolete("Use LoginLink instead.")]
        public virtual Controls.Link NavigateOnLoginLink
        {
            get
            {
                return LoginLink;
            }
            set
            {
                LoginLink = value;
            }
        }

        /// <summary>
        /// Gets or sets the URI to navigate to on a successful login.
        /// </summary>
        /// <remarks>Store the implementation of this on iApp.Session, or a similar globally-accessible data store.</remarks>
        [Obsolete("Use LoginLink instead.")]
        public virtual string NavigateOnLogin
        {
            get
            {
                var loginLink = LoginLink;
                return loginLink == null ? null : loginLink.Address;
            }
            set
            {
                if (LoginLink == null)
                {
                    LoginLink = new Link(value);
                }
                else
                {
                    LoginLink.Address = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets any parameters to include on a successful login.
        /// </summary>
        /// <remarks>Store the implementation of this on iApp.Session, or a similar globally-accessible data store.</remarks>
        [Obsolete("Use LoginLink instead.")]
        public virtual Dictionary<string, string> NavigateOnLoginParams
        {
            get
            {
                var loginLink = LoginLink;
                return loginLink == null ? null : loginLink.Parameters;
            }
            set
            {
                if (LoginLink == null)
                {
                    LoginLink = new Link(null);
                }
                LoginLink.Parameters = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to display if authentication failed.
        /// </summary>
        public virtual string ErrorText
        {
            get { return iApp.Session.ContainsKey(ErrorTextKey) ? iApp.Session[ErrorTextKey].ToString() : null; }
            set { iApp.Session[ErrorTextKey] = value; }
        }


        /// <summary>
        /// Gets or sets the title of the layer if there is an authentication error.
        /// </summary>
        /// <value>
        /// The layer title in the event of a failed login.
        /// </value>
        public virtual string ErrorTitle
        {
            get { return iApp.Session.ContainsKey(ErrorTitleKey) ? iApp.Session[ErrorTitleKey].ToString() : null; }
            set { iApp.Session[ErrorTitleKey] = value; }
        }


        /// <summary>
        /// Gets or sets the text that pre-populates the username field.
        /// </summary>
        /// <value>The default username.</value>
        public virtual string DefaultUsername
        {
            get { return iApp.Session.ContainsKey(UserKey) ? (string)iApp.Session[UserKey] : null; }
            set { iApp.Session[UserKey] = value; }
        }

        /// <summary>
        /// Gets or sets the label of the username field.
        /// </summary>
        /// <value>
        /// The label of the username field.
        /// </value>
        public virtual string UsernameLabel
        {
            get { return iApp.Session.ContainsKey(UserLabelKey) ? iApp.Session[UserLabelKey].ToString() : iApp.Factory.GetResourceString("Username") ?? "Username:"; }
            set { iApp.Session[UserLabelKey] = value; }
        }

        /// <summary>
        /// Gets or sets the label of the password field.
        /// </summary>
        /// <value>
        /// The label of the password field.
        /// </value>
        public virtual string PasswordLabel
        {
            get { return iApp.Session.ContainsKey(PassLabelKey) ? iApp.Session[PassLabelKey].ToString() : iApp.Factory.GetResourceString("Password") ?? "Password:"; }
            set { iApp.Session[PassLabelKey] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Icon"/> to display above the login controls.
        /// </summary>
        /// <value>
        /// The <see cref="Icon"/> to display.
        /// </value>
        public virtual Icon BrandImage
        {
            get { return iApp.Session.ContainsKey(BrandImageKey) ? (Icon)iApp.Session[BrandImageKey] : null; }
            set { iApp.Session[BrandImageKey] = value; }
        }

        private const string UserKey = "username";
        private const string UserLabelKey = "usernameLabel";
        private const string PasswordKey = "password";
        private const string PassLabelKey = "passwordLabel";
        private const string BrandImageKey = "brandImage";
        private const string ErrorTitleKey = "errorTitle";
        private const string ErrorTextKey = "errorText";
        private const string LoginLinkKey = "loginLink";
    }
}
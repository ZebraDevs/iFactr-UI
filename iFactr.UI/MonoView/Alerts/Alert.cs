using System;
using System.Diagnostics;

using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native dialog box for modally presenting a small amount of textual information.
    /// </summary>
    public sealed class Alert : IAlert
    {
        /// <summary>
        /// Occurs when the dialog box is dismissed by the user.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event AlertResultEventHandler Dismissed
        {
            add { nativeAlert.Dismissed += value; }
            remove { nativeAlert.Dismissed -= value; }
        }
        
        /// <summary>
        /// Gets or sets the link to navigate to when the result of the alert is <see cref="AlertResult.Cancel"/>.
        /// The navigation only occurs if there is no handler for the <see cref="Dismissed"/> event.
        /// </summary>
        public Link CancelLink
        {
            get { return nativeAlert.CancelLink; }
            set { nativeAlert.CancelLink = value; }
        }
        
        /// <summary>
        /// Gets or sets the link to navigate to when the result of the alert is <see cref="AlertResult.OK"/>.
        /// The navigation only occurs if there is no handler for the <see cref="Dismissed"/> event.
        /// </summary>
        public Link OKLink
        {
            get { return nativeAlert.OKLink; }
            set { nativeAlert.OKLink = value; }
        }
        
        /// <summary>
        /// Gets the message being displayed by the dialog box.
        /// </summary>
        public string Message
        {
            get { return nativeAlert.Message; }
        }
        
        /// <summary>
        /// Gets the title of the dialog box.
        /// </summary>
        public string Title
        {
            get { return nativeAlert.Title; }
        }
        
        /// <summary>
        /// Gets the button combination for the dialog box.
        /// </summary>
        public AlertButtons Buttons
        {
            get { return nativeAlert.Buttons; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IAlert nativeAlert;

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Alert"/> class.
        /// </summary>
        /// <param name="message">The message to display with the alert.</param>
        /// <param name="title">The title of the alert.</param>
        /// <param name="buttons">The button combination for the alert.</param>
        public Alert(string message, string title, AlertButtons buttons)
        {
            nativeAlert = MXContainer.Resolve<IAlert>(null, message, title, buttons);
            if (nativeAlert == null)
            {
                throw new InvalidOperationException("No native object was found for the current instance.");
            }
        }

        /// <summary>
        /// Modally presents the dialog box.
        /// </summary>
        public void Show()
        {
            nativeAlert.Show();
        }
    }
}


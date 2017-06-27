namespace iFactr.UI
{
	/// <summary>
	/// Defines a native dialog box for modally presenting a small amount of textual information.
	/// </summary>
    public interface IAlert
    {
        /// <summary>
        /// Gets or sets the link to navigate to when the result of the alert is <see cref="AlertResult.Cancel"/>.
        /// The navigation only occurs if there is no handler for the <see cref="Dismissed"/> event.
        /// </summary>
        Link CancelLink { get; set; }

        /// <summary>
        /// Gets or sets the link to navigate to when the result of the alert is <see cref="AlertResult.OK"/>.
        /// The navigation only occurs if there is no handler for the <see cref="Dismissed"/> event.
        /// </summary>
        Link OKLink { get; set; }

		/// <summary>
		/// Gets the message being displayed by the dialog box.
		/// </summary>
        string Message { get; }

		/// <summary>
		/// Gets the title of the dialog box.
		/// </summary>
        string Title { get; }

		/// <summary>
		/// Gets the button combination for the dialog box.
		/// </summary>
        AlertButtons Buttons { get; }

        /// <summary>
        /// Occurs when the dialog box is dismissed by the user.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event AlertResultEventHandler Dismissed;

		/// <summary>
		/// Modally presents the dialog box.
		/// </summary>
        void Show();
    }
}

using iFactr.Core.Controls;
using iFactr.Core.Layers;

namespace iFactr.Core
{
    /// <summary>
    /// Defines an alert to the user that is displayed through the device's notification system.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Gets or sets the primary text of the notification.
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the secondary text of the notification.
        /// </summary>
        string Subtext { get; set; }
        /// <summary>
        /// Gets or sets an icon to display with the notificiation.
        /// </summary>
        Icon Icon { get; set; }
        /// <summary>
        /// Gets or sets a URL to an image to use as the background of the notification.
        /// </summary>
        string BackgroundImageUrl { get; set; }
        /// <summary>
        /// Gets or sets a link to navigate to when the notification has been selected.
        /// </summary>
        Link Link { get; set; }
    }

    /// <summary>
    /// Represents an alert to the user that is displayed through the device's notification system.
    /// </summary>
    /// <typeparam name="T">The type of the context that will be used to set the notification.</typeparam>
    public abstract class Notification<T> : iItem, INotification
    {
        /// <summary>
        /// Gets or sets a URL to an image to use as the background of the notification.
        /// </summary>
        public string BackgroundImageUrl { get; set; }
        /// <summary>
        /// Sets the notification using the specified context.
        /// </summary>
        /// <param name="context">The context with which to set the notification.</param>
        public abstract void SetNotificationContext(T context);
    }
}

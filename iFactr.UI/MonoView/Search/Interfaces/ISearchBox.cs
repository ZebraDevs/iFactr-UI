namespace iFactr.UI
{
    /// <summary>
    /// Defines a text box that can perform search queries.
    /// </summary>
    public interface ISearchBox : IPairable
    {
        /// <summary>
        /// Gets or sets the background color of the search box.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the search box.
        /// </summary>
        Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text within the search box.
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the text to display when there is no value in the search box.
        /// </summary>
        string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets completion behavior for text that is entered into the search box.
        /// Not all platforms support all behaviors.
        /// </summary>
        TextCompletion TextCompletion { get; set; }

        /// <summary>
        /// Gets or sets the text within the search box.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Occurs when the user executes a search through the search box.
        /// Use this event to filter any model data and reload the view's content.
        /// </summary>
        event SearchEventHandler SearchPerformed;

        /// <summary>
        /// Programmatically sets input focus to the search box.
        /// </summary>
        void Focus();
    }
}

using System;

using iFactr.Core;

namespace iFactr.UI
{
    /// <summary>
    /// Represents the method that will determine whether or not a navigation should proceed.
    /// </summary>
    /// <param name="link">A <see cref="Link"/> containing the destination and any other relevant information regarding the navigation taking place.</param>
    /// <param name="type">The type of navigation that was initiated.</param>
    /// <returns><c>true</c> to proceed with the navigation; otherwise, <c>false</c>.</returns>
    public delegate bool ShouldNavigateDelegate(Link link, NavigationType type);

    /// <summary>
    /// Defines a view that is displayed in a view stack.
    /// </summary>
    public interface IHistoryEntry
    {
        /// <summary>
        /// Gets or sets the <see cref="Link"/> that describes the behavior
        /// and appearance of the back button associated with the view.
        /// </summary>
        Link BackLink { get; set; }

        /// <summary>
        /// Gets or sets the stack identifier for the view.
        /// Views with the same identifier will take the same place in the view stack.
        /// </summary>
        string StackID { get; set; }

        /// <summary>
        /// Gets or sets the pane on which the view will be rendered.
        /// </summary>
        Pane OutputPane { get; set; }

        /// <summary>
        /// Gets or sets the style in which the view should be presented when displayed in a popover pane.
        /// </summary>
        PopoverPresentationStyle PopoverPresentationStyle { get; set; }

        /// <summary>
        /// Invoked when the view is being pushed under or popped off of the top of the view stack.
        /// Returning a value of <c>false</c> will cancel the navigation.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        ShouldNavigateDelegate ShouldNavigate { get; set; }

        /// <summary>
        /// Gets the view stack that the view is currently on.
        /// </summary>
        IHistoryStack Stack { get; }

        /// <summary>
        /// Occurs when the view is pushed or popped onto the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Activated;

        /// <summary>
        /// Occurs when the view is pushed under or popped off of the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event EventHandler Deactivated;
    }
}
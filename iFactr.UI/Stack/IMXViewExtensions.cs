using System.Linq;
using iFactr.UI;
using MonoCross.Navigation;

namespace iFactr.Core
{
    /// <summary>
    /// Contains extension methods for IMXView.
    /// </summary>
    public static class IMXViewExtensions
    {
        /// <summary>
        /// Determines whether the given view is being displayed.
        /// </summary>
        /// <param name="view">The view to check for displaying.</param>
        /// <returns><c>true</c> if the view is currently presented; otherwise <c>false</c>.</returns>
        public static bool IsDisplaying(this IMXView view)
        {
            var stackFound = false;
            for (var pane = Pane.Popover; pane > Pane.Tabs; pane--)
            {
                var stack = PaneManager.Instance.FromNavContext(pane);
                if (stack != null)
                {
                    stackFound = stack.Views.Any();
                    if (Equals(stack.CurrentView, view))
                    {
                        return true;
                    }
                }
                else if (stackFound)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using MonoCross.Utilities;
using iFactr.UI;
using MonoCross.Navigation;

namespace iFactr.Core
{
    /// <summary>
    /// Static class containing history stack management methods.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="view"/> is a member of the stack.
        /// </summary>
        /// <param name="historyStack">The stack to check.</param>
        /// <param name="view">The <see cref="IMXView"/> to check for.</param>
        /// <returns><c>true</c> if the view exists in the history stack, or is being displayed; otherwise <c>false</c>.</returns>
        public static bool Contains(this IHistoryStack historyStack, IMXView view)
        {
            var entry = view as IHistoryEntry;
            return historyStack != null && historyStack.Views.Any(v =>
            {
                var e = v as IHistoryEntry;
                if (e != null && entry != null)
                {
                    return e.StackID == entry.StackID;
                }
                return Equals(v, view);
            });
        }

        /// <summary>
        /// Finds the <see cref="Pane"/> in which the stack is displayed.
        /// </summary>
        /// <param name="stack">The stack displayed in a pane.</param>
        /// <returns>The <see cref="Pane"/> in which the stack is displayed.</returns>
        public static Pane FindPane(this IHistoryStack stack)
        {
            return PaneManager.Instance.NavContextFromKey(PaneManager.Instance.HistoryStacks.First(s => s.Value == stack).Key).ActivePane;
        }

        /// <summary>
        /// Whether this stack can navigate back
        /// </summary>
        public static bool CanGoBack(this IHistoryStack stack)
        {
            var entry = stack.CurrentView as IHistoryEntry;
            return stack.DisplayBackButton(entry == null ? null : entry.BackLink);
        }

        /// <summary>
        /// Whether this stack can navigate back, given a back button
        /// </summary>
        public static bool DisplayBackButton(this IHistoryStack stack, Link backButton)
        {
            if (stack == null || ((backButton == null || backButton.Address == null) && stack.Views.Count() < 2))
            {
                return false;
            }

            if (backButton != null)
            {
                return backButton.Action != ActionType.None;
            }

            var previousView = stack.Views.ElementAtOrDefault(stack.Views.Count() - 2);
            if (previousView == null)
            {
                return false;
            }

            var viewAtt = Device.Reflector.GetCustomAttribute<StackBehaviorAttribute>(previousView.GetType(), true);
            return viewAtt == null || (viewAtt.Options & StackBehaviorOptions.HistoryShy) == 0;
        }

        /// <summary>
        /// Executes the appropriate logic for the specified <see cref="Link"/> object, treating it as a back button.
        /// </summary>
        /// <param name="stack">The stack that contains the view the link object is a part of.</param>
        /// <param name="backLink">The link object to be handled.</param>
        /// <param name="pane">The pane in which the view containing the back link resides.</param>
        public static void HandleBackLink(this IHistoryStack stack, Link backLink, Pane pane)
        {
            if (backLink == null || backLink.Address == null)
            {
                var views = stack.Views.ToList();
                var view = views.ElementAtOrDefault(views.Count() - 2);
                if (view != null && !PaneManager.Instance.ShouldNavigate(new Link(PaneManager.Instance.GetNavigatedURI(view),
                        new Dictionary<string, string>()), pane, NavigationType.Back)) { return; }

                if (backLink != null)
                {
                    if (backLink.Action == ActionType.None) return;
                    if (string.IsNullOrEmpty(backLink.ConfirmationText))
                    {
                        if (view == null) stack.PopView();
                        else PaneManager.Instance.DisplayView(view);
                    }
                    else
                    {
                        var newLink = backLink.Clone();
                        newLink.ConfirmationText = null;

                        var alert = new Alert(backLink.ConfirmationText, iApp.Factory.GetResourceString("ConfirmTitle"), AlertButtons.OKCancel);
                        alert.Dismissed += (o, e) =>
                        {
                            if (e.Result == AlertResult.OK)
                            {
                                if (view == null) stack.PopView();
                                else PaneManager.Instance.DisplayView(view);
                            }
                        };
                        alert.Show();
                    }
                }
                else
                {
                    if (view == null) stack.PopView();
                    else PaneManager.Instance.DisplayView(view);
                }
            }
            else
            {
                iApp.Navigate(backLink, stack.CurrentView);
            }
        }

        /// <summary>
        /// Displays the specified view in the stack.
        /// </summary>
        /// <param name="stack">The stack to display the view.</param>
        /// <param name="view">The view to be displayed.</param>
        public static void DisplayView(this IHistoryStack stack, IMXView view)
        {
            stack.DisplayView(view, false);
        }

        /// <summary>
        /// Displays the specified view in the stack.
        /// </summary>
        /// <param name="stack">The stack to display the view.</param>
        /// <param name="view">The view to be displayed.</param>
        /// <param name="forceRoot"><c>true</c> to empty the history stack; otherwise <c>false</c></param>
        public static void DisplayView(this IHistoryStack stack, IMXView view, bool forceRoot)
        {
            var entry = view as IHistoryEntry;
            var behaviorOptions = Device.Reflector.GetCustomAttributes(view.GetType(), true).OfType<StackBehaviorAttribute>().Select(stackAtt => stackAtt.Options).FirstOrDefault();
            var viewForcesRoot = (behaviorOptions & StackBehaviorOptions.ForceRoot) != 0;

            if (viewForcesRoot || forceRoot)
            {
                var root = stack.Views.FirstOrDefault();
                if (root == null)
                {
                    stack.PushView(view);
                }
                else
                {
                    var rootAtt = Device.Reflector.GetCustomAttribute<StackBehaviorAttribute>(root.GetType(), true);
                    if (!viewForcesRoot && rootAtt != null && (rootAtt.Options & StackBehaviorOptions.ForceRoot) != 0)
                    {
                        // If current root is already ForceRoot, let it be
                        stack.PopToRoot();
                        stack.PushView(view);
                    }
                    else
                    {
                        // Otherwise replace it
                        stack.ReplaceView(root, view);
                        stack.PopToView(view);
                    }
                }
            }
            else
            {
                var current = entry == null ? null : stack.Views.OfType<IHistoryEntry>().FirstOrDefault(e => e.StackID == entry.StackID);
                if (current != null)
                {
                    if (current != entry)
                    {
                        stack.ReplaceView((IMXView)current, view);
                    }
                    stack.PopToView(view);
                }
                else
                {
                    stack.PushView(view);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified stack has views in its history.
        /// </summary>
        /// <param name="stack">The stack to check.</param>
        /// <returns><c>true</c> if the <see cref="IHistoryStack.Views"/> collection contains more than one view; otherwise <c>false</c>.</returns>
        public static bool HasHistory(this IHistoryStack stack)
        {
            return stack.Views.Where(v => v != stack.CurrentView)
                .Select(view => Device.Reflector.GetCustomAttribute<StackBehaviorAttribute>(view.GetType(), true))
                .Any(behavior => behavior == null || (behavior.Options & StackBehaviorOptions.HistoryShy) == 0);
        }
    }
}
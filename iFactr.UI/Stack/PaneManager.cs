using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using iFactr.Core.Layers;
using MonoCross.Utilities;
using iFactr.UI;
using MonoCross.Navigation;

namespace iFactr.Core
{
    /// <summary>
    /// Manages a collection of <see cref="IHistoryStack"/>s.
    /// </summary>
    public sealed class PaneManager : IEnumerable<IHistoryStack>
    {
        /// <summary>
        /// Gets the collection of history stacks.
        /// </summary>
        /// <value>
        /// A <see cref="Dictionary&lt;TKey,TValue&gt;"/> that maps a <see cref="iApp.AppNavigationContext"/> represented as an int to a <see cref="IHistoryStack"/>.
        /// </value>
        /// <remarks>To convert a key back to an <see cref="iApp.AppNavigationContext"/>, use <see cref="NavContextFromKey"/>.</remarks>
        internal readonly Dictionary<int, IHistoryStack> HistoryStacks;
        internal readonly WeakKeyDictionary<IMXView, string> NavigatedURIs;

        /// <summary>
        /// Gets or sets the state of the active tab displayed by the stack manager.
        /// </summary>
        public int CurrentTab
        {
            get
            {
                return Tabs == null || Tabs.SelectedIndex < 0 ? 0 : Tabs.SelectedIndex;
            }
            set
            {
                if (Tabs == null || value < 0 || value >= Tabs.TabItems.Count() || CurrentTab == value)
                {
                    return;
                }

                iApp.CurrentNavContext.ActiveTab = Tabs.SelectedIndex = value;
                if (IsSplitView) FromNavContext(Pane.Detail, CurrentTab).PopToRoot();
            }
        }

        internal ITabView Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                if (_tabs != null)
                {
                    iApp.CurrentNavContext.ActiveTab = _tabs.SelectedIndex;
                }
            }
        }
        private ITabView _tabs;

        /// <summary>
        /// Displays the layer in the appropriate history stack.
        /// </summary>
        /// <param name="layer">The layer to display.</param>
        /// <param name="view">The view that is displaying the <paramref name="layer"/>.</param>
        [Obsolete]
        public void DisplayLayer(iLayer layer, IMXView view)
        {
            DisplayView(view);
        }

        /// <summary>
        /// Inserts the specified <see cref="IMXView"/> object into an appropriate stack and renders it.
        /// </summary>
        /// <param name="view">The view to be inserted into a stack.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="view"/> is <c>null</c>.</exception>
        public void DisplayView(IMXView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            var entry = view as IHistoryEntry;
            if (entry != null && entry.StackID == null)
            {
                entry.StackID = view.GetType().FullName;
            }

            var navContext = new iLayer.NavigationContext
            {
                OutputOnPane = GetPreferredPaneForView(view),
                NavigatedActiveTab = iApp.CurrentNavContext.ActiveTab,
                NavigatedActivePane = iApp.CurrentNavContext.ActivePane,
            };

            var stack = FindStack(view);
            var layer = view.GetModel() as iLayer;
            if (layer != null)
            {
                if (stack.Contains(view))
                {
                    layer.NavContext.NavigatedActivePane = navContext.NavigatedActivePane;
                    layer.NavContext.ClearPaneHistoryOnOutput = false;
                }
                layer.NavContext.OutputOnPane = navContext.OutputOnPane;
                layer.NavContext.NavigatedActiveTab = navContext.NavigatedActiveTab;
                navContext = layer.NavContext;
            }

            var originalPane = navContext.OutputOnPane;
            if (stack.Contains(view))
            {
                originalPane = stack.FindPane();
                iApp.CurrentNavContext.ActivePane = originalPane;
                navContext.NavigatedActivePane = originalPane;
                if (entry != null)
                {
                    entry.OutputPane = originalPane;
                }
            }

            for (navContext.OutputOnPane = Pane.Popover; navContext.OutputOnPane > originalPane; navContext.OutputOnPane--)
            {
                var clearStack = FromNavContext(navContext);
                if (clearStack != null && clearStack.FindPane() > originalPane)
                {
                    clearStack.PopToRoot();
                }
            }

            stack.DisplayView(view, navContext.NavigatedActivePane < navContext.OutputOnPane || navContext.ClearPaneHistoryOnOutput);
        }

        /// <summary>
        /// Finds the stack to which the view belongs.
        /// </summary>
        /// <param name="view">The view used for querying the history stacks.</param>
        /// <returns>The history stack that this view exists in or prefers.</returns>
        public IHistoryStack FindStack(IMXView view)
        {
            return HistoryStacks.FirstOrDefault(s =>
             {
                 var nav = NavContextFromKey(s.Key);
                 return s.Value.Contains(view) && (nav.ActivePane > Pane.Master || nav.ActiveTab == CurrentTab)
                     || nav.ActivePane == Pane.Master && !s.Value.Views.Any() && nav.ActiveTab == CurrentTab;
             }).Value ?? //search stacks for existing view
                  FromNavContext(GetPreferredPaneForView(view), CurrentTab) ?? //then try to get by the preferred pane
                  FromNavContext(Pane.Master, CurrentTab); //and if all else fails
        }

        /// <summary>
        /// Adds the stack to this pane manager.
        /// </summary>
        /// <param name="stack">The history stack to add to the manager.</param>
        /// <param name="context">The navigation context that describes the stack position.</param>
        public void AddStack(IHistoryStack stack, iApp.AppNavigationContext context)
        {
            if (HistoryStacks.ContainsKey(context.GetHashCode()))
                HistoryStacks[context.GetHashCode()] = stack;
            else
                HistoryStacks.Add(context.GetHashCode(), stack);

            if (stack != null && stack.FindPane() == Pane.Detail)
            {
                var root = stack.Views.FirstOrDefault();
                var view = (root as IView) ?? MXContainer.Resolve<IView>();
                if (view != null)
                {
                    if (iApp.Instance != null)
                    {
                        view.HeaderColor = iApp.Instance.Style.HeaderColor;
                    }
                    view.SetBackground(iApp.VanityImagePath, ContentStretch.None);
                    if (root == null)
                    {
                        stack.PushView(view);
                    }
                    else
                    {
                        stack.ReplaceView(root, view);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all of the history stacks from all of the panes.
        /// </summary>
        public void Clear()
        {
            HistoryStacks.Clear();
        }

        /// <summary>
        /// Removes all of the history stacks from the specified <see cref="Pane"/>.
        /// </summary>
        /// <param name="pane">The pane from which to remove the history stacks.</param>
        public void Clear(Pane pane)
        {
            var stacks = HistoryStacks.Where(hs => (Pane)(hs.Key % byte.MaxValue) == pane).ToArray();
            foreach (var stack in stacks)
            {
                HistoryStacks.Remove(stack.Key);
            }
        }

        /// <summary>
        /// Checks relevant stacks to see if any on-screen layers would prevent navigation.
        /// </summary>
        /// <param name="link">The link that is being navigated to.</param>
        /// <param name="pane">The pane of the layer that is being navigated away from.</param>
        /// <param name="type">The navigation event.</param>
        /// <returns><c>true</c> to continue navigation; otherwise <c>false</c>.</returns>
        public bool ShouldNavigate(Link link, Pane pane, NavigationType type)
        {
            IHistoryStack stack = null;
            while (stack == null && pane > Pane.Tabs)
            {
                stack = HistoryStacks.FirstOrDefault(s =>
                {
                    var nav = NavContextFromKey(s.Key);
                    return nav.ActivePane == pane && (nav.ActivePane > Pane.Master || nav.ActiveTab == CurrentTab);
                }).Value;

                if (stack == null) pane--;
            }

            foreach (var view in HistoryStacks
                .Where(s =>
                {
                    var nav = NavContextFromKey(s.Key);
                    return s.Value.CurrentView != null && (pane >= Pane.Detail && nav.ActivePane >= pane || pane <= Pane.Master && nav.ActiveTab == CurrentTab);
                }).Select(pair => pair.Value.CurrentView))
            {
                var layer = view.GetModel() as iLayer;
                if (layer != null)
                {
                    // if the declaring type is not iLayer, it means that the method has been overridden and we should use that instead of the factory's delegate
                    if (Device.Reflector.GetMethod(layer.GetType(), "ShouldNavigateFrom", typeof(Link), typeof(NavigationType)).DeclaringType != typeof(iLayer))
                    {
                        if (!layer.ShouldNavigateFrom(link, type))
                            return false;
                    }
                    else if (iApp.Factory.ShouldNavigateFromLayer != null && !iApp.Factory.ShouldNavigateFromLayer(layer.View as IHistoryEntry, link, type))
                    {
                        return false;
                    }
                }
                else
                {
                    var entry = view as IHistoryEntry;
                    if (entry != null && entry.ShouldNavigate != null && !entry.ShouldNavigate(link, type))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets whether the specified stack manager is for a split view.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if specified stack manager is for a split view; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSplitView
        {
            get { return iApp.Factory.LargeFormFactor && iApp.Instance.FormFactor == FormFactor.SplitView; }
        }

        /// <summary>
        /// Gets a history stack instance by nav context.
        /// </summary>
        /// <param name="navContext">The nav context of the desired history stack.</param>
        /// <returns>A history stack matching the nav context.</returns>
        [Obsolete("Use the overload FromNavContext(Pane pane, int tab)")]
        public IHistoryStack FromNavContext(iLayer.NavigationContext navContext)
        {
            return FromNavContext(navContext.OutputOnPane, navContext.NavigatedActiveTab);
        }

        /// <summary>
        /// Gets a history stack instance by pane.
        /// </summary>
        /// <param name="pane">The <see cref="Pane"/> to search for.</param>
        /// <returns>A history stack matching the nav context.</returns>
        public IHistoryStack FromNavContext(Pane pane)
        {
            return FromNavContext(pane, CurrentTab);
        }

        /// <summary>
        /// Gets a history stack instance by pane and tab.
        /// </summary>
        /// <param name="pane">The <see cref="Pane"/> to search for.</param>
        /// <param name="tab">The tab of a <see cref="Pane.Master"/> stack.</param>
        /// <returns>A history stack matching the nav context.</returns>
        public IHistoryStack FromNavContext(Pane pane, int tab)
        {
            if (HistoryStacks.Count == 1) return HistoryStacks.First().Value;
            if (pane < Pane.Master) pane = Pane.Master;
            return HistoryStacks.FirstOrDefault(stack =>
            {
                var nav = NavContextFromKey(stack.Key);
                return nav.ActivePane == pane && (nav.ActivePane > Pane.Master || nav.ActiveTab == tab);
            }).Value;
        }

        /// <summary>
        /// Gets the URI of the controller that outputted the specified <see cref="IMXView"/>.
        /// </summary>
        /// <param name="view">The <see cref="IMXView"/> for which to retrieve the navigated URI.</param>
        public string GetNavigatedURI(IMXView view)
        {
            // used primarily for ShouldNavigate
            string uri = null;
            NavigatedURIs.TryGetValue(view, out uri);
            return uri;
        }

        internal iApp.AppNavigationContext NavContextFromKey(int key)
        {
            return new iApp.AppNavigationContext
            {
                ActivePane = (Pane)(key % byte.MaxValue),
                ActiveTab = key / byte.MaxValue,
                ActiveLayer = iApp.CurrentNavContext.ActiveLayer
            };
        }

        internal Pane GetPreferredPaneForView(IMXView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            // Order of importance: iLayer > IHistoryEntry > PreferredPane
            var layer = view.GetModel() as iLayer;
            if (layer != null)
            {
                return layer.NavContext.OutputOnPane;
            }

            var entry = view as IHistoryEntry;
            if (entry != null && entry.OutputPane != Pane.Tabs)
            {
                return entry.OutputPane;
            }

            var topPane = TopmostPane.OutputOnPane;
            var paneAttribute = Device.Reflector.GetCustomAttribute<PreferredPaneAttribute>(view.GetType(), true);

            if (topPane < Pane.Detail && paneAttribute == null)
            {
                if (topPane == Pane.Tabs)
                {
                    return Pane.Master;
                }

                var detail = FromNavContext(Pane.Detail, 0);
                if (detail != null && detail.FindPane() == Pane.Detail)
                {
                    return Pane.Detail;
                }
            }

            return paneAttribute != null && paneAttribute.Pane > topPane ? paneAttribute.Pane : topPane;
        }

        /// <summary>
        /// Gets the navigation context of the highest-ordinal Pane
        /// </summary>
        /// <returns>An <see cref="iLayer.NavigationContext"/> with its OutputOnPane set to the topmost Pane with an active layer.</returns>
        public iLayer.NavigationContext TopmostPane
        {
            get
            {
                var navContext = new iLayer.NavigationContext
                {
                    OutputOnPane = Pane.Popover,
                    NavigatedActiveTab = iApp.CurrentNavContext.ActiveTab,
                    NavigatedActivePane = iApp.CurrentNavContext.ActivePane,
                };

                for (; navContext.OutputOnPane > Pane.Tabs; navContext.OutputOnPane--)
                {
                    var context = FromNavContext(navContext);
                    if (context == null || context.FindPane() != navContext.OutputOnPane || !context.Views.Any(v =>
                        {
                            var stackAtt = Device.Reflector.GetCustomAttribute<StackBehaviorAttribute>(v.GetType(), true);
                            return stackAtt == null || (stackAtt.Options & StackBehaviorOptions.HistoryShy) == 0;
                        }))
                    {
                        continue;
                    }

                    return navContext;
                }
                return navContext;
            }
        }

        private PaneManager()
        {
            HistoryStacks = new Dictionary<int, IHistoryStack>();
            NavigatedURIs = new WeakKeyDictionary<IMXView, string>();
        }

        #region Singleton members
        private static volatile PaneManager _instance;
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Gets the singleton manager instance.
        /// </summary>
        public static PaneManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new PaneManager();
                }

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the history stacks.
        /// </summary>
        public IEnumerator<IHistoryStack> GetEnumerator()
        {
            return HistoryStacks.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return HistoryStacks.Values.GetEnumerator();
        }
    }
}
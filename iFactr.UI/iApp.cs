using iFactr.Core.Layers;
using iFactr.Core.Styles;
using iFactr.Core.Targets;
using iFactr.Core.Targets.Config;
using iFactr.Core.Targets.Settings;
using iFactr.UI;
using MonoCross;
using MonoCross.Navigation;
using MonoCross.Utilities;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.ImageComposition;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ActionType = iFactr.UI.ActionType;
using Link = iFactr.UI.Link;

namespace iFactr.Core
{
    /// <summary>
    /// Defines a cross-platform iFactr application.
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// Gets the navigation map that associates the application's layers and controllers to their respective URIs.
        /// </summary>
        NavigationList NavigationMap { get; }

        /// <summary>
        /// Gets or sets the application's title.
        /// </summary>
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI to navigate to once the application has loaded.
        /// </summary>
        string NavigateOnLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether the application has a <see cref="NavigationTabs"/> layer.
        /// </summary>
        bool HasNavigationTabs { get; }

        /// <summary>
        /// Gets or sets the application's style.  If no layer style is set, this style is used.
        /// </summary>
        Style Style
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the application's form factor as a <see cref="FormFactor"/> value.
        /// </summary>
        FormFactor FormFactor { get; }

        /// <summary>
        /// Gets the platform that the application is running on.
        /// </summary>
        MobilePlatform Platform
        {
            get;
        }

        /// <summary>
        /// Gets the target that the application is running on.
        /// </summary>
        MobileTarget Target
        {
            get;
        }
    }

    /// <summary>
    /// Represents a cross-platform iFactr application.  This class is abstract.
    /// </summary>
    /// <remarks>
    /// <b>iApp </b>is the base container for your business logic when using the iFactr
    /// framework. Within your own implementation of <b>iApp</b> you will define a
    /// navigation map that establishes the heirarchy of <b>iLayer</b> objects. These
    /// layers will be consumed by the target bindings for your mobile platform, and
    /// translated to an optimized user-experience on your target, or targets of choice.
    /// <para> </para>
    /// <para>When implementing your own<b> iApp</b> you will override the <c>OnAppLoad()</c>method to perform the following tasks: </para>
    /// <para> </para>
    /// <para><b>Setting the application Title</b></para>
    /// <para> </para>
    /// <para>You can give you application a title by simply setting the Title property
    /// on your <b>iApp</b> instance:</para>
    /// <para></para>
    /// <code lang="C#">// Set the application title
    /// Title = &quot;Best Sellers&quot;;</code>
    /// <para> </para>
    /// <para><b>Establishing a Navigation Map</b></para>
    /// <para> </para>
    /// <para>The Navigation Map defines your application structure by associating a specific URI template with a specific layer instance. Establish a navigation map by adding these associations in your <c>OnAppLoad()</c> override:</para>
    /// <code lang="C#">
    /// 
    /// // Add navigation mappings
    /// NavigationMap.Add(&quot;&quot;, new CategoryList());
    /// NavigationMap.Add(&quot;{Category}&quot;, new BookList());
    /// NavigationMap.Add(&quot;{Category}/{Book}&quot;, new BookDetails());
    /// </code>
    /// <para></para>
    /// <para><b>Advanced Endpoint Definitions</b> </para>
    /// <para>You can parameterize any data and/or metadata needed for your layer
    /// processing, as long as you follow a consistent URI template scheme. But there
    /// are a few rules that need to be followed to ensure proper processing by the
    /// framework navigation processing.</para>
    /// <para> </para>
    /// <para>Let's say you want to use a single layer to process both edit and delete
    /// operations for an order. Following the design paradigm in our previous examples,
    /// you might place two separate endpoints in your navigation map:</para>
    /// <para> </para>
    /// <para><code lang="C#"> NavigationMap.Add(&quot;Orders/Edit/{Id}&quot;,
    /// orderDetails);    </code></para>
    /// <para><code lang="C#"> NavigationMap.Add(&quot;Orders/Delete/{Id}&quot;, orderDetails);<c></c></code>  </para>
    /// <para> </para>
    /// <para>This will establish two endpoints to your OrderDetails layer, one for
    /// edit, and one for delete, but the resulting parameters passed will be the same,
    /// (i.e. a single Order ID parameter). In order to provide the necessary metadata,
    /// in this case the layer action, you can parameterize your URI to include the
    /// necessary value. Simply replace the two URI's above with the following: </para>
    /// <para><c>      </c></para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Id}&quot;,
    /// orderDetails; </code></para>
    /// <para> </para>
    /// <para>Now when you make a request to <c>&quot;Orders/Edit/1234&quot;</c> your Layer will load with two parameters in the dictionary: <c>Action=Edit;Id=1234</c>. The action parameter, indicates the desired transaction, (in this case, edit), and the ID parameter represents the order you wish to edit. </para>
    /// <para> </para>
    /// <para>One additional rule to follow when establishing your navigation map
    /// ensures the navigation framework will produce the desired result. Let's take our
    /// example to the next logical step to illustrate this point. Now I'd like to
    /// provide an endpoint for adding an order based on an existing customer, so I
    /// might assume I can add the following to support that function: </para>
    /// <para> </para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Customer}&quot;, orderDetails;<c></c></code>  </para>
    /// <para> </para>
    /// <para>Now when I receive an Action of &quot;Add&quot;, and a parameter called Customer on
    /// my Layer parameter dictionary, I can process the transaction as a new order. But
    /// there is a problem; in this instance you'll never receive a Customer parameter
    /// on your Layer parameters. Here's why. Your navigation map entries now look like
    /// this: </para>
    /// <para><c></c></para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Id}&quot;,
    /// orderDetails;</code></para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Customer}&quot;, orderDetails;<c></c></code>   </para>
    /// <para> </para>
    /// <para>When the iFactr navigation framework processes your request, it does a
    /// Regex match between the URI requested and the URI key in the navigation map.
    /// Because we use some segments of the navigation map URI for parameters, they must
    /// all be treated equally. Because the possible parameter substitutions are
    /// infinitely variable, we cannot include them as part of our Regex, because they
    /// will never produce a match. </para>
    /// <para> </para>
    /// <para>So the navigation framework treats each squiggly bracket variable as a generic placeholder. And the precedence of the layers in the navigation map are set by the order in which they are added to the map, so when the navigation map is processed for a match on the URI <c>&quot;Orders/Add/9876&quot;</c>, it will match the entry for <c>&quot;Orders/{Action}/{Id}&quot;</c>, and return the layer with the associated parameters. The second entry, (<c>URI=&quot;Orders/{Action}/{Customer}&quot;</c>), will never be found, and the Customer parameter will never be generated.</para>
    /// <para> </para>
    /// <para>We can try to remedy this situation by ensuring our navigation map URI
    /// patterns are unique. So we can replace one of the parameters with the hard-coded
    /// action we are performing: </para>
    /// <para> </para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Id}&quot;,
    /// orderDetails;</code></para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/Add/{Customer}&quot;, orderDetails; <c></c></code>  </para>
    /// <para> </para>
    /// <para>But there is still a problem; remember the precedence of the pattern
    /// matches? Placing the hard-coded item in the second position in this case does
    /// not help, because the Regex will still find and match the first entry. Remember,
    /// all parameters are treated as generic placeholders, and will match on any value
    /// passed to the map. We need to make sure any hard-coded values appear before any
    /// parameterized URI's in the same navigation tree. So the final fix is to move it
    /// up in the order:</para>
    /// <para> </para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/Add/{Customer}&quot;,
    /// orderDetails;</code></para>
    /// <para><code lang="C#">NavigationMap.Add(&quot;Orders/{Action}/{Id}&quot;, orderDetails;<c></c></code>   </para>
    /// <para> </para>
    /// <para>Now the URI<c> &quot;Orders/Add/9876&quot; </c>will return the correct layer with the correct Customer parameter.</para>
    /// <para> </para>
    /// <para><b>Setting the Default Navigation URI</b></para>
    /// <para> </para>
    /// <para>The default navigation URI represents the starting layer for your application. Use the URI value from your Navigation Map that represents the starting point for your app, and set the <c>NavigateOnLoad</c> property:<code lang="C#"></code></para>
    /// <code lang="C#">
    /// // Set default navigation URI
    /// NavigateOnLoad = &quot;&quot;;</code>
    /// <para> </para>
    /// <para><b>Setting the Application Style</b></para>
    /// <para> </para>
    /// <para>Finally, you can apply application-level styles using the <c>Style</c> class. For example, to set the header color to black, set the <c>HeaderColor</c> property on your <b>iApp</b> instance's <c>Style</c> to a new <b>Color</b> object containing appropriate information:</para>
    /// <code lang="C#">
    /// // Set the application style
    /// Style.HeaderColor = new Style.Color(0,0,0);</code>
    /// <para> </para>
    /// </remarks>
    /// <example>
    /// <code lang="C#">
    /// using System;
    /// using System.Collections.Generic;
    /// using System.Linq;
    /// using System.Text;
    /// 
    /// using iFactr.Core;
    /// using iFactr.Core.Styles;
    /// 
    /// namespace BestSellers
    /// {
    ///     public class App : iApp
    /// 
    ///         public override void OnAppLoad()
    ///         {
    ///             // Set the application title
    ///             Title = &quot;Best Sellers&quot;;
    /// 
    ///             // Add navigation mappings
    ///             NavigationMap.Add(&quot;&quot;, new CategoryList());
    ///             NavigationMap.Add(&quot;{Category}&quot;, new BookList());
    ///             NavigationMap.Add(&quot;{Category}/{Book}&quot;, new BookDetails());
    /// 
    ///             // Set default navigation URI
    ///             NavigateOnLoad = &quot;&quot;;
    /// 
    ///             // Set the application style
    ///             Style.HeaderColor = new Style.Color(0,0,0);
    ///         }
    ///     }
    /// }</code>
    /// </example>
    public abstract class iApp : MXApplication, IApp
    {
        #region static members

        /// <summary>
        /// Occurs when the layer load has been completed and is ready to be displayed. This event is to be consumed by the Target Factory instance.
        /// </summary>
        public static event iLayer.LayerEventHandler OnLayerLoadComplete;

        /// <summary>
        /// Occurs when the orientation of the application is changed.
        /// </summary>
        public static event OrientationEvent OrientationChanged;

        /// <summary>
        /// Sends the specified notification to the device's notification system.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public static void SetNotification(INotification notification)
        {
            Factory.SetNotification(notification);
        }

        #region navigation

        /// <summary>
        /// Matches the specified URL to an entry in the navigation map.
        /// </summary>
        /// <param name="url">The URL to match.</param>
        public static MXNavigation MatchUrl(string url)
        {
            return ((MXApplication)Instance).NavigationMap.MatchUrl(url);
        }


        /// <summary>
        /// Gets or sets the current navigation context containing information about the last navigation.
        /// </summary>
        /// <value>The current navigation context.</value>
        public static AppNavigationContext CurrentNavContext
        {
            get
            {
                if (!Session.ContainsKey(NavContextKey))
                    Session.Add(NavContextKey, new AppNavigationContext());
                else if (Session[NavContextKey] == null)
                    Session[NavContextKey] = new AppNavigationContext();

                return (AppNavigationContext)Session[NavContextKey];
            }
            set
            {
                if (!Session.ContainsKey(NavContextKey))
                    Session.Add(NavContextKey, value);
                else
                    Session[NavContextKey] = value;
            }
        }
        private const string NavContextKey = "CurrentNavContext";

        /// <summary>
        ///  Initiates a navigation to the specified URL.
        /// </summary>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        public static void Navigate(string url)
        {
            Navigate(new Link(url));
        }

        /// <summary>
        ///  Initiates a navigation to the specified URL with the specified parameters.
        /// </summary>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey, TValue&gt;"/> representing the parameters to pass through.</param>
        public static void Navigate(string url, Dictionary<string, string> parameters)
        {
            Navigate(new Link(url, parameters));
        }

        /// <summary>
        /// Initiates a navigation using the specified Link.
        /// </summary>
        /// <param name='link'>A <see cref="Link"/> containing the URL to navigate to with the parameters to pass through.</param>
        public static void Navigate(Link link)
        {
            Navigate(link, null);
        }

        /// <summary>
        /// Initiates a navigation using the specified Link.
        /// </summary>
        /// <param name='link'>A <see cref="Link"/> containing the URL to navigate to with the parameters to pass through.</param>
        /// <param name="fromView">The <see cref="IMXView"/> instance from which the navigation was initiated.</param>
        public static void Navigate(Link link, IMXView fromView)
        {
            if (link == null)
            {
                Log.Warn("Navigation to null link cancelled.");
                return;
            }

            // Add layer's ActionParameters to Link
            var layer = fromView == null ? null : fromView.GetModel() as iLayer;
            if (layer != null && layer.ActionParameters != null)
            {
                if (link.Parameters == null)
                {
                    link.Parameters = new Dictionary<string, string>();
                }

                link.Parameters.AddRange(layer.ActionParameters);
            }

            if (fromView is ITabView)
            {
                CurrentNavContext.ActivePane = Pane.Tabs;
                CurrentNavContext.ActiveTab = (fromView as ITabView).SelectedIndex;
            }
            else
            {
                var entry = fromView as IHistoryEntry;
                if (entry != null)
                {
                    CurrentNavContext.ActivePane = entry.OutputPane == Pane.Tabs ? Pane.Master : entry.OutputPane;
                }
                else if (layer != null)
                {
                    CurrentNavContext.ActivePane = layer.NavContext.OutputOnPane;
                }
            }

            #region Link actions

            if (fromView != null)
            {
                link.Address = Resolve(PaneManager.Instance.GetNavigatedURI(fromView), link.Address);
            }

            if (link.Action == ActionType.Submit)
            {
                var listview = fromView as IListView;
                if (listview != null)
                {
                    var newLink = link.Clone();
                    newLink.Action = ActionType.Undefined;
                    listview.Submit(newLink);
                    return;
                }

                var gridview = fromView as IGridView;
                if (gridview != null)
                {
                    var newLink = link.Clone();
                    newLink.Action = ActionType.Undefined;
                    gridview.Submit(newLink);
                    return;
                }
            }

            if (link.Action == ActionType.None)
            {
                return;
            }

            #endregion

            if (!string.IsNullOrEmpty(link.ConfirmationText))
            {
                var newLink = link.Clone();
                newLink.ConfirmationText = null;

                var alert = new Alert(link.ConfirmationText, Factory.GetResourceString("ConfirmTitle"), AlertButtons.OKCancel);
                alert.OKLink = newLink;
                alert.Show();
                return;
            }

            if (link.Address == null)
            {
                Log.Warn("Navigation to null URI cancelled.");
                return;
            }

            var parameters = link.Parameters == null ? new Dictionary<string, string>() : new Dictionary<string, string>(link.Parameters);

            Factory.LastActivityDate = DateTime.Now;
            Log.Info("Navigating to: " + link.Address);

            // Determine which mapping has a matching pattern to the URL to navigate to
            var navMap = Instance.NavigationMap.MatchUrl(link.Address);
            iLayer navLayer;

            // If there is no result, assume the URL is external and create a new Browser Layer
            if (navMap == null)
            {
                if (link.RequestType == RequestType.NewWindow)
                {
                    new BrowserView<string>().LaunchExternal(link.Address);
                    Factory.StopBlockingUserInput();
                    return;
                }

                var browserLayer = new Browser(link.Address);
                browserLayer.OnLoadComplete += TargetFactory.TheApp.LayerLoadCompleted;
                navLayer = browserLayer;
            }
            else
            {
                navLayer = navMap.Controller as iLayer;
            }

            // UI elements could be accessed while figuring out nav context, so ensure that it happens on the UI thread
            Thread.ExecuteOnMainThread(() =>
            {
                Pane pane = Pane.Master;
                var topPane = PaneManager.Instance.TopmostPane.OutputOnPane;
                var stack = PaneManager.Instance.FromNavContext(topPane);
                if (fromView == null && stack != null && stack.CurrentView != null)
                {
                    var history = stack.CurrentView as IHistoryEntry;
                    CurrentNavContext.ActivePane = history == null ? topPane : history.OutputPane;
                }

                if (navLayer != null)
                {
                    // Set up the Layer's navigation context
                    var navContext = navLayer.NavContext;
                    navContext.NavigatedUrl = link.Address == string.Empty && navMap != null ? navMap.Pattern : link.Address;
                    var navUriEntry = PaneManager.Instance.NavigatedURIs.FirstOrDefault(p => p.Value == navContext.NavigatedUrl);
                    var existingView = navUriEntry.Key as IHistoryEntry;

                    var activePane = CurrentNavContext.ActivePane;
                    var targetPane = existingView == null ? navLayer.FindTarget() : existingView.OutputPane;
                    stack = PaneManager.Instance.FromNavContext(targetPane, CurrentNavContext.ActiveTab);
                    if (existingView != null && !stack.Contains(existingView as IMXView))
                    {
                        targetPane = navLayer.FindTarget();
                    }

                    if (targetPane > activePane && (targetPane != Pane.Detail ||
                        Factory.LargeFormFactor && Instance.FormFactor == FormFactor.SplitView))
                    {
                        navContext.ClearPaneHistoryOnOutput = true;
                    }
                    else
                    {
                        if (CurrentNavContext.ActivePane != targetPane && (stack == null || stack.Views.OfType<IHistoryEntry>().All(v => v.StackID != navLayer.Name)))
                            targetPane = PaneManager.Instance.TopmostPane.OutputOnPane;
                        navContext.OutputOnPane = activePane = CurrentNavContext.ActivePane = targetPane;
                        navContext.ClearPaneHistoryOnOutput = false;
                    }

                    // Always render to Master from Tabs, otherwise target the targetPane
                    navContext.OutputOnPane = (activePane == Pane.Tabs) ? Pane.Master : targetPane;
                    navContext.NavigatedActivePane = activePane;
                    navContext.NavigatedActiveTab = CurrentNavContext.ActiveTab;
                    navLayer.LayerStyle = Instance.Style.Clone();
                    pane = navContext.OutputOnPane;
                }

                Link navLink = link.Clone();
                navLink.Parameters = parameters;
                if (PaneManager.Instance.ShouldNavigate(navLink, pane, NavigationType.Forward))
                {
                    Factory.ActivateLoadTimer(link.LoadIndicatorTitle, link.LoadIndicatorDelay);

                    // Initiate the layer loading for the associated layer passing all parameters 
                    CurrentNavContext.ActiveLayer = navLayer;
                    if (navMap == null)
                    {
                        Factory.LoadLayer(fromView, navLayer, navLink.Address, navLink.Parameters);
                    }
                    else
                    {
                        MXContainer.Navigate(fromView, navLink.Address, navLink.Parameters);
                    }
                }
                else
                {
                    Factory.StopBlockingUserInput();
                }
            });
        }

        private static string Resolve(string baseuri, string uri)
        {
            const string callbackParam = "callback=";

            if (uri != null && uri.Contains(callbackParam))
            {
                var callBase = uri.Remove(uri.IndexOf(callbackParam) + callbackParam.Length);
                var callback = uri.Substring(callBase.Length);
                return callBase + Resolve(baseuri, callback);
            }

            if (uri == null || !uri.Contains(".") || uri.Contains(":"))
            {
                return uri;
            }

            var uriParts = new List<string>();
            foreach (var part in uri.Split('/'))
            {
                if (part == "..")
                {
                    if (uriParts.Count == 0)
                    {
                        uriParts.InsertRange(0, baseuri.Split('/'));
                        baseuri = string.Empty;
                    }

                    if (uriParts.Count > 0)
                    {
                        uriParts.RemoveAt(uriParts.Count - 1);
                    }
                }
                else
                {
                    uriParts.Add(part == "." ? string.Empty : part);
                }
            }

            string retval = string.Empty;
            foreach (var part in uriParts)
            {
                if (retval.EndsWith("/") && string.IsNullOrEmpty(part)) continue;
                retval += part + "/";
            }

            if (retval.StartsWith("/"))
            {
                retval = baseuri + retval;
            }

            if (retval.EndsWith("/"))
            {
                retval = retval.Remove(retval.Length - 1);
            }

            return retval;
        }

        /// <summary>
        /// Sets the application's navigation context using the specified layer navigation context.
        /// </summary>
        /// <param name="navContext">The layer navigation context to set the application's navigation context with.</param>
        public static void SetNavigationContext(iLayer.NavigationContext navContext)
        {
            CurrentNavContext.ActiveTab = navContext.NavigatedActiveTab;
            CurrentNavContext.ActivePane = navContext.OutputOnPane;
        }

        /// <summary>
        /// Posts the specified network response.
        /// </summary>
        /// <param name="networkResponse">The network response to post.</param>
        public static void PostNetworkResponse(NetworkResponse networkResponse)
        {
            if (networkResponse == null)
                return;
            TargetFactory.HandleNetworkResponse(networkResponse);
        }

        /// <summary>
        /// Called when the device changes orientation.
        /// </summary>
        /// <param name="orientation">The new device orientation.</param>
        internal static void OnOrientationChanged(Orientation orientation)
        {
            var handler = OrientationChanged;
            if (handler != null) handler(orientation);
        }

        #endregion

        #region singleton accessors


        /// <summary>
        /// Gets or sets the request injection headers within the current session.
        /// </summary>
        /// <value>A <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> representing the request injection headers.</value>
        public static SerializableDictionary<string, string> RequestInjectionHeaders
        {
            get
            {
                return Device.RequestInjectionHeaders;
            }
            set
            {
                Device.RequestInjectionHeaders = value;
            }
        }

        /// <summary>
        /// Gets or sets the application instance.
        /// </summary>
        /// <value>The application instance.</value>
        public static IApp Instance
        {
            get
            {
                return MXContainer.Instance == null ? null : MXContainer.Instance.App as IApp;
            }
            set
            {
                TargetFactory.TheApp = (iApp)value;
            }
        }
        /// <summary>
        /// Gets the target-specific singleton factory instance.
        /// </summary>
        /// <value>The factory as a <see cref="TargetFactory"/> instance.</value>
        public static TargetFactory Factory
        {
            get { return MXContainer.Instance as TargetFactory; }
        }

        /// <summary>
        /// Gets or sets the file path to the image that should be used in the vanity panel for split view applications.
        /// </summary>
        public static string VanityImagePath
        {
            get { return vanityImagePath; }
            set
            {
                if (value != vanityImagePath)
                {
                    vanityImagePath = value;
                    var detailPane = PaneManager.Instance.FromNavContext(Pane.Detail, 0);
                    if (detailPane != null && detailPane.FindPane() == Pane.Detail)
                    {
                        Thread.ExecuteOnMainThread(() =>
                        {
                            var vanity = detailPane.Views.FirstOrDefault() as IView;
                            if (vanity != null)
                            {
                                vanity.SetBackground(vanityImagePath, ContentStretch.None);
                            }
                        });
                    }
                }
            }
        }
        private static string vanityImagePath;

        #endregion

        #region utilities

        /// <summary>
        /// Gets the application's configuration settings.
        /// </summary>
        /// <remarks>
        /// Provides durable storage of string values across multiple application instances and uses.
        /// This property is set by the Target Bindings at runtime.
        /// </remarks>
        public static IConfig Config
        {
            get
            {
                return Factory.Config;
            }
        }

        /// <summary>
        /// Gets the application's file system interface.
        /// </summary>
        /// <value>File system access for the application as an <see cref="IFile"/> instance.</value>
        public static IFile File
        {
            get
            {
                return Factory.File;
            }
        }

        /// <summary>
        /// Gets the application's image compositor.
        /// </summary>
        /// <value>The image compositor for the application as an <see cref="ICompositor"/> instance.</value>
        public static ICompositor Compositor
        {
            get
            {
                return Factory.Compositor;
            }
        }

        /// <summary>
        /// Gets the application's logging utility.
        /// </summary>
        /// <value>The logger as an <see cref="ILog"/> instance.</value>
        public static ILog Log
        {
            get
            {
                return Factory.Logger;
            }
            set
            {
                Factory.Logger = value;
            }
        }

        /// <summary>
        /// Gets the application's threading utility.
        /// </summary>
        /// <value>The threading utility as an <see cref="IThread"/> instance.</value>
        public static IThread Thread
        {
            get
            {
                return Factory.Thread;
            }
        }

        /// <summary>
        /// Gets the application's networking utility.
        /// </summary>
        /// <value>The networking utility as an <see cref="INetwork"/> instance.</value>
        public static INetwork Network
        {
            get
            {
                return Factory.Network;
            }
        }

        /// <summary>
        /// Gets the application's data encryptor.
        /// </summary>
        /// <value>The encryptor as an <see cref="IEncryption"/> instance.</value>
        public static IEncryption Encryption
        {
            get
            {
                return Factory.Encryption;
            }
        }

        /// <summary>
        /// Gets the application's session settings.
        /// </summary>
        /// <remarks>
        /// Provides non-durable values for session-scoped storage of objects to be used during this instance of the application.
        /// This property is set by the Target Bindings at runtime.
        /// </remarks>
        public static ISession Session
        {
            get
            {
                return Factory.Session;
            }
        }

        /// <summary>
        /// Gets the application's settings.
        /// </summary>
        /// <remarks>
        /// Provides durable storage of string values across multiple application instances and uses.
        /// This property is set by the Target Bindings at runtime.
        /// </remarks>
        public static ISettings Settings
        {
            get
            {
                return Factory.Settings;
            }
        }
        ///// <summary>
        ///// Gets the image path, which is usually the root of the application.
        ///// </summary>
        //public static string ImagePath
        //{
        //    get { return Factory.ImagePath; }
        //}

        /// <summary>
        /// Gets a unique identifier for the device running the application.
        /// </summary>
        public static string DeviceId
        {
            get
            {
                return Factory.DeviceId;
            }
        }

        #endregion

        #endregion

        #region IApp members

        /// <summary>
        /// Gets or sets the application's style.  If no layer style is set, this style is used.
        /// </summary>
        public Style Style
        {
            get { return _style ?? Factory.Style; }
            set { _style = value; }
        }
        private Style _style;

        /// <summary>
        /// Gets the platform that the application is running on.
        /// </summary>
        /// <value>The platform as a <see cref="MobilePlatform"/> value.</value>
        public MobilePlatform Platform
        {
            get { return Factory.Platform; }
        }

        /// <summary>
        /// Gets the target that the application is running on.
        /// </summary>
        /// <value>
        /// The target as a <see cref="MobileTarget" /> value.
        /// </value>
        public MobileTarget Target
        {
            get { return Factory.Target; }
        }

        /// <summary>
        /// Gets the application's form factor as a <see cref="FormFactor"/> value.
        /// </summary>
        public FormFactor FormFactor { get; protected set; }

        /// <summary>
        /// Gets whether the application has a <see cref="NavigationTabs"/> layer.
        /// </summary>
        /// <value><c>true</c> if this instance has navigation tabs; otherwise <c>false</c>.</value>
        public bool HasNavigationTabs
        {
            get { return NavigationMap.Any(item => item.Controller is NavigationTabs); }
        }

        #endregion

        #region instance members

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="iApp"/> class.
        /// </summary>
        public iApp()
        {
            Session.SafeKeys.Add(NavContextKey);
        }

        #endregion

        #region virtual event methods

        /// <summary>
        /// Called when an application layer begins loading.  This method is meant to be overridden in consuming applications 
        /// for notification when a layer is loaded as an alternative to using events.
        /// </summary>
        /// <param name="layer">An <see cref="iLayer"/> representing the layer being loaded.</param>
        public virtual void OnLayerLoading(iLayer layer)
        {
        }

        internal void LayerLoadCompleted(iLayer layer)
        {
            bool output = true;
            if (Device.Reflector.GetMethod(GetType(), "OnLayerLoading", typeof(iLayer)).DeclaringType != typeof(iApp))
            {
                output = false;
                OnLayerLoading(layer);
            }

            var handler = OnLayerLoadComplete;
            if (handler != null)
            {
                output = false;
                handler(layer);
            }

            if (output)
            {
                Factory.OutputLayer(layer);
            }
        }

        /// <summary>
        /// Called when a controller's view output has been canceled by setting <see cref="MXContainer.CancelLoad"/> or by returning a <c>null</c> perspective.
        /// </summary>
        /// <param name="fromView">The view that initiated the navigation to the controller.</param>
        /// <param name="controller">The canceled controller.</param>
        /// <param name="navigatedUri">The uri used to navigate to the controller.</param>
        public virtual void OnControllerLoadCanceled(IMXView fromView, IMXController controller, string navigatedUri)
        {
            Device.Log.Debug("{0} canceled load to {1}. This can be handled by overriding OnControllerLoadCanceled in the app.", controller, navigatedUri);
        }

        #endregion

        #endregion

        #region iApp classes

        /// <summary>
        /// Represents an application's navigation context.
        /// </summary>
        public class AppNavigationContext
        {
            /// <summary>
            /// Gets or sets the tab that the last navigation was initiated from.
            /// </summary>
            public int ActiveTab
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the pane that the last navigation was initiated from.
            /// </summary>
            public Pane ActivePane
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the layer that was last navigated to.
            /// </summary>
            public iLayer ActiveLayer
            {
                get;
                set;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AppNavigationContext"/> class.
            /// </summary>
            public AppNavigationContext()
            {
                ActivePane = Pane.Master;
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return ActivePane == Pane.Master ? ActiveTab.ToString(CultureInfo.InvariantCulture) : ActivePane.ToString();
            }

            /// <summary>
            /// Determines if the specified object is equal to this instance.
            /// </summary>
            /// <param name="obj">The object to test for equality.</param>
            /// <returns><c>true</c> if the object is equal to this instance; otherwise <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                var context = obj as AppNavigationContext;
                return context != null && context.ActivePane == ActivePane && context.ActiveTab == ActiveTab;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode()
            {
                return (int)ActivePane + byte.MaxValue * ActiveTab;
            }
        }

        #endregion

        /// <summary>
        /// The available device orientations.
        /// </summary>
        public enum Orientation
        {
            /// <summary>
            /// Portrait orientation.
            /// </summary>
            Portrait,
            /// <summary>
            /// Portrait orientation, inverted.
            /// </summary>
            PortraitUpsideDown,
            /// <summary>
            /// Landscape, left oriented.
            /// </summary>
            LandscapeLeft,
            /// <summary>
            /// Landscape, right oriented.
            /// </summary>
            LandscapeRight,
        }

        /// <summary>
        /// Represents the method that will handle the OrientationChanged event.
        /// </summary>
        /// <param name="orientation">The new device orientation.</param>
        public delegate void OrientationEvent(Orientation orientation);
    }
}
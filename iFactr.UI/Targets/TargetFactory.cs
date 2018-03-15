using iFactr.Core.Layers;
using iFactr.Core.Styles;
using iFactr.Core.Targets.Config;
using iFactr.Core.Targets.Settings;
using iFactr.Integrations;
using iFactr.UI;
using iFactr.UI.Instructions;
using iFactr.UI.Reflection;
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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Link = iFactr.Core.Controls.Link;

/*
This namespace contains the classes representing the abstract TargetFactory
that is implemented on each target with bindings that translate the abstract
representation of an iApp and iLayers into the platform and target-specific user
experience.
*/
namespace iFactr.Core.Targets
{
    /// <summary>
    /// Represents the method that will handle navigation events.
    /// </summary>
    /// <param name="source">The history entry that triggered the navigation event.</param>
    /// <param name="link">The link containing information about the navigation taking place.</param>
    /// <param name="navigationType">The type of navigation that is occurring.</param>
    /// <returns><c>true</c> if navigation should continue; otherwise <c>false</c></returns>
    public delegate bool NavigationDelegate(IHistoryEntry source, Link link, NavigationType navigationType);

    /// <summary>
    /// Represents the base class of a bindings factory.  This class is abstract.
    /// </summary>
    //// <typeparam name="T">The concrete factory type to be initialized.</typeparam>
    // public abstract class TargetFactory<T> where T : TargetFactory<T>, ITargetFactory
    public abstract class TargetFactory : MXContainer, ITargetFactory
    {
        /// <summary>
        /// Occurs when an <see cref="IMXView"/> instance is about to be outputted.
        /// </summary>
        public event EventHandler<ViewOutputCancelEventArgs> ViewOutputting;

        /// <summary>
        /// Occurs after an <see cref="IMXView"/> instance has been outputted.
        /// </summary>
        public event EventHandler<ViewOutputEventArgs> ViewOutputted;

        /// <summary>
        /// Gets or sets the <see cref="Converter"/> instance that is responsible for
        /// generating cells from <see cref="iFactr.Core.Layers.iItem"/> and <see cref="iFactr.Core.Forms.Field"/> objects.
        /// </summary>
        public Converter Converter
        {
            get { return _converter ?? (_converter = new Converter()); }
            set { _converter = value; }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private Converter _converter;

        /// <summary>
        /// Gets or sets the <see cref="Instructor"/> instance that is responsible for executing
        /// specialized logic for certain UI elements depending on the target platform.
        /// </summary>
        public virtual Instructor Instructor
        {
            get { return instructor ?? (instructor = new UniversalInstructor()); }
            set { instructor = value; }
        }
        private Instructor instructor;

        /// <summary>
        /// Gets or sets an instance of the timer that is used for determining when to display the load indicator.
        /// </summary>
        protected ITimer LoadTimer { get; set; }

        /// <summary>
        /// Gets or sets the default delay, in milliseconds, before the load indicator is displayed.
        /// </summary>
        protected internal double DefaultLoadIndicatorDelay { get; set; }

        internal IPlatformDefaults PlatformDefaults
        {
            get
            {
                if (platformDefaults == null)
                {
                    platformDefaults = Resolve<IPlatformDefaults>();
                }

                return platformDefaults;
            }
        }
        private IPlatformDefaults platformDefaults;

        /// <summary>
        /// Signals the application to begin ignoring any new input from the user.
        /// </summary>
        public void BeginBlockingUserInput()
        {
            iApp.Thread.ExecuteOnMainThread(() =>
            {
                OnBeginBlockingUserInput();
            });
        }

        /// <summary>
        /// Signals the application to begin accepting user input again.
        /// </summary>
        public void StopBlockingUserInput()
        {
            iApp.Thread.ExecuteOnMainThread(() =>
            {
                if (LoadTimer != null && LoadTimer.IsEnabled)
                {
                    LoadTimer.Stop();
                }
                LoadTimer = null;

                OnHideLoadIndicator();
                OnStopBlockingUserInput();
            });
        }

        /// <summary>
        /// Executes the rendering logic for the view of the specified <see cref="IMXController"/> instance before painting it on the screen.
        /// </summary>
        /// <param name="controller">The controller to output.</param>
        /// <param name="perspective">The perspective that is mapped to the view that will be rendered.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="controller"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when there is no view mapped to the <paramref name="perspective"/>.</exception>
        public void OutputController(IMXController controller, string perspective, string navigatedUri)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            var ilayer = controller as iLayer;
            IMXView view = Views.GetView(controller.ModelType, perspective);
            if (view == null)
            {
                if (ilayer != null)
                {
                    view = ilayer.GetView();
                    ilayer.View = view;
                }
                else
                {
                    var viewType = Views.GetViewType(controller.ModelType, perspective);
                    if (viewType == null)
                    {
                        object model = controller.GetModel();
                        if (model != null && Device.Reflector.HasAttribute(model.GetType(), typeof(ViewAttribute), false))
                        {
                            view = ReflectiveUIBuilder.GenerateView(model);
                        }
                        else
                        {
                            throw new InvalidOperationException("There is no view or view type mapped to the given perspective.");
                        }
                    }
                    else
                    {
                        view = (IMXView)Activator.CreateInstance(viewType);
                    }
                }
            }

            view.SetModel(controller.GetModel());

            var entry = view as IHistoryEntry;
            if (entry != null)
            {
                entry.OutputPane = PaneManager.Instance.GetPreferredPaneForView(view);
            }

            view.Render();

            {
                var handler = ViewOutputting;
                if (handler != null)
                {
                    var args = new ViewOutputCancelEventArgs(controller, view);
                    handler(this, args);

                    if (args.Cancel)
                        return;
                }
            }

            var tabs = view as ITabView;
            if (tabs != null)
            {
                PaneManager.Instance.Tabs = tabs;
            }

            PaneManager.Instance.NavigatedURIs[view] = navigatedUri;
            OnOutputView(view);

            { // Handler scope
                var handler = ViewOutputted;
                if (handler != null)
                {
                    handler(this, new ViewOutputEventArgs(controller, view));
                }
            }

            StopBlockingUserInput();
        }

        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/> using the specified parameters.
        /// The type must have been registered using <see cref="MXContainer.Resolve"/> for it to be created.
        /// </summary>
        /// <typeparam name="T">The type of the object to be created.</typeparam>
        /// <param name="parameters">Any parameters to pass along to the constructor of the created object.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentException">Thrown when type <typeparamref name="T"/> is not an interface type.</exception>
        /// <exception cref="MissingMemberException">Thrown when an appropriate constructor is not found for type <typeparamref name="T"/>.</exception>
        [Obsolete("Use the Resolve methods for object initialization.")]
        public T CreateObject<T>(params object[] parameters)
        {
            return (T)CreateObject(typeof(T), parameters);
        }

        /// <summary>
        /// Creates an instance of the specified interface type using the specified parameters.
        /// The type must have been registered using <see cref="MXContainer.Resolve"/> for it to be created.
        /// </summary>
        /// <param name="interfaceType">The interface type of the object to be created.</param>
        /// <param name="parameters">Any parameters to pass along to the constructor of the created object.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="interfaceType"/> is not an interface type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="interfaceType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when an appropriate constructor is not found for the object of the specified <paramref name="interfaceType"/>.</exception>
        [Obsolete("Use the Resolve methods for object initialization.")]
        public object CreateObject(Type interfaceType, params object[] parameters)
        {
            return Resolve(interfaceType, null, parameters);
        }

        /// <summary>
        /// Called when an <see cref="ICustomItem"/> instance is ready to be rendered and a native object is needed for insertion into the view.
        /// </summary>
        /// <param name="item">The item that is ready to be rendered.</param>
        /// <param name="layer">The layer instance associated with the item, or <c>null</c> if the item is not associated with a layer.</param>
        /// <param name="view">The view that will contain the item.</param>
        /// <param name="recycledCell">An already instantiated cell that is ready for reuse, or <c>null</c> if no cell has been recycled.</param>
        /// <returns>The object that will be inserted into the view.</returns>
        protected internal virtual object OnGetCustomItem(ICustomItem item, iLayer layer, IListView view, object recycledCell)
        {
            return Converter.ConvertToCell(item, layer.LayerStyle, view, recycledCell as ICell);
        }

        /// <summary>
        /// Returns the platform-specific object that the specified object represents.
        /// </summary>
        /// <param name="obj">The object for which to return the platform-specific counterpart.</param>
        /// <param name="objectName">The name of the object being passed in.</param>
        /// <param name="nativeTypes">The types that an object must be for it to be considered a valid native counterpart.</param>
        protected internal static object GetNativeObject(object obj, string objectName, params Type[] nativeTypes)
        {
            if (nativeTypes == null)
                throw new ArgumentNullException("nativeTypes");

            var pairable = obj as IPairable;
            if (pairable == null)
            {
                Parameter.CheckParameterType(obj, objectName, false, nativeTypes);
                return obj;
            }

            foreach (var nativeType in nativeTypes)
            {
                if (Device.Reflector.IsAssignableFrom(nativeType, pairable.GetType()))
                    return pairable;

                if (pairable.Pair != null && Device.Reflector.IsAssignableFrom(nativeType, pairable.Pair.GetType()))
                    return pairable.Pair;
            }

            if (pairable.Pair == null)
            {
                var type = Device.Reflector.GetInterfaces(pairable.GetType()).Select(i => Resolve(i, null)).FirstOrDefault(i => i != null);
                pairable.Pair = type as IPairable;
            }

            Parameter.CheckParameterType(pairable.Pair, objectName, false, nativeTypes);
            return pairable.Pair;
        }

        /// <summary>
        /// Gets the abstracted object from an <see cref="IPairable"/>.
        /// </summary>
        /// <param name="pair">The object for which to return the abstract counterpart.</param>
        protected internal static object GetPair(object pair)
        {
            var pairable = pair as IPairable;
            if (pairable == null)
            {
                return pair;
            }

            return (TheApp != null && Device.Reflector.GetAssembly(pair.GetType()) == Device.Reflector.GetAssembly(TheApp.GetType())) ? pair : pairable.Pair ?? pair;
        }

        /// <summary>
        /// Called when an <see cref="iLayer"/> instance is ready to be outputted.  Override this method in a subclass
        /// in order to handle layer types that cannot be handled by the available abstract objects.
        /// </summary>
        /// <param name="layer">The layer to be outputted.</param>
        /// <returns><c>true</c> if layer output was handled and the factory should not attempt to output it as a controller; otherwise <c>false</c>.</returns>
        protected virtual bool OnOutputLayer(iLayer layer)
        {
            var browser = layer as Browser;
            if (browser != null && browser.Url != null)
            {
                int index = browser.Url.IndexOf(':');
                if (index < 0)
                {
                    return false;
                }

                switch (browser.Url.Substring(0, index))
                {
                    case "accel":
                        var parameters = HttpUtility.ParseQueryString(browser.Url.Substring(browser.Url.IndexOf('?')));
                        string callback = parameters == null ? null : parameters.GetValueOrDefault("callback");
                        if (callback == null)
                        {
                            throw new ArgumentException("Accelerometer requires a callback URI.");
                        }

                        var accel = new Accelerometer();
                        accel.ValuesUpdated += (o, e) =>
                        {
                            lock (accel)
                            {
                                if (accel.IsActive)
                                {
                                    accel.Stop();
                                    iApp.Navigate(callback, new Dictionary<string, string>()
                                    {
                                        { "X", e.Data.X.ToString() },
                                        { "Y", e.Data.Y.ToString() },
                                        { "Z", e.Data.Z.ToString() },
                                    });
                                }
                            }
                        };

                        accel.Start();
                        return true;
                    case "compass":
                        parameters = HttpUtility.ParseQueryString(browser.Url.Substring(browser.Url.IndexOf('?')));
                        callback = parameters == null ? null : parameters.GetValueOrDefault("callback");
                        if (callback == null)
                        {
                            throw new ArgumentException("Compass requires a callback URI.");
                        }

                        var compass = new Compass();
                        compass.HeadingUpdated += (o, e) =>
                        {
                            lock (compass)
                            {
                                if (compass.IsActive)
                                {
                                    compass.Stop();
                                    iApp.Navigate(callback, new Dictionary<string, string>()
                                    {
                                        { "Bearing", e.Data.TrueHeading.ToString() },
                                    });
                                }
                            }
                        };

                        compass.Start();
                        return true;
                    case "geoloc":
                        parameters = HttpUtility.ParseQueryString(browser.Url.Substring(browser.Url.IndexOf('?')));
                        callback = parameters == null ? null : parameters.GetValueOrDefault("callback");
                        if (callback == null)
                        {
                            throw new ArgumentException("Geolocation requires a callback URI.");
                        }

                        var geoloc = new GeoLocation();
                        geoloc.LocationUpdated += (o, e) =>
                        {
                            lock (geoloc)
                            {
                                if (geoloc.IsActive)
                                {
                                    geoloc.Stop();
                                    iApp.Navigate(callback, new Dictionary<string, string>()
                                    {
                                        { "Lat", e.Data.Latitude.ToString() },
                                        { "Lon", e.Data.Longitude.ToString() }
                                    });
                                }
                            }
                        };

                        geoloc.Start();
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called when an <see cref="IMXController"/> instance is about to be loaded.
        /// </summary>
        /// <param name="controller">The controller that is about to be loaded.</param>
        /// <param name="fromView">The view that initiated that navigation to the controller.</param>
        protected override void OnControllerLoadBegin(IMXController controller, IMXView fromView)
        {
            var browser = controller as Browser;
            if (browser == null || browser.Url == null || !browser.Url.StartsWith("audio:"))
            {
                BeginBlockingUserInput();
            }

            var layer = controller as iLayer;
            if (layer != null)
            {
                layer.Clear();
            }
        }

        /// <summary>
        /// Called when an <see cref="IMXController"/> instance has finished loading.
        /// </summary>
        /// <param name="fromView">The view that initiated the navigation to the controller.</param>
        /// <param name="controller">The controller that has finished loading.</param>
        /// <param name="perspective">The view perspective that was returned from the controller.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        protected override void OnControllerLoadComplete(IMXView fromView, IMXController controller, string perspective, string navigatedUri)
        {
            if (perspective == null)
            {
                TheApp.OnControllerLoadCanceled(fromView, controller, navigatedUri);
                return;
            }

            var layer = controller as iLayer;
            var parent = layer;

            while (layer != null)
            {
                var tabs = layer as NavigationTabs;
                if (tabs != null && tabs.TabItems != null && tabs.TabItems.Count == 0)
                    throw new NotSupportedException("NavigationTabs must have at least 1 Tab.");
                if (layer is FormLayer)
                    layer.Items.AddRange(((FormLayer)layer).Fieldsets.Cast<iLayerItem>()); //Cast required for NETCF
                if (layer.CompositeParent != null)
                    layer.CompositeParent.Items.AddRange(layer.Items);

                // for backwards compatibility
                if (layer.Items.FirstOrDefault() is iList || layer.Items.FirstOrDefault() is iPanel)
                {
                    layer.Layout = LayerLayout.EdgetoEdge;
                }
                else if (layer.Items.FirstOrDefault() is iMenu || layer.Items.FirstOrDefault() is iBlock)
                {
                    layer.Layout = LayerLayout.Rounded;
                }

                iLayer navLayer;
                var nextMapping = layer.CompositeLayerLink == null
                    ? null : App.NavigationMap.MatchUrl(layer.CompositeLayerLink.Address);

                if (nextMapping == null || (navLayer = nextMapping.Controller as iLayer) == null || !LargeFormFactor)
                    break;

                navLayer.CompositeParent = layer.CompositeParent ?? layer;
                navLayer.Clear();

                var parameters = new Dictionary<string, string>();
                nextMapping.ExtractParameters(layer.CompositeLayerLink.Address, parameters);
                navLayer.Load(navigatedUri, parameters);
                if (navLayer.ActionButtons.Any())
                    navLayer.CompositeParent.ActionButtons = navLayer.ActionButtons;
                layer = navLayer;
            }

            Thread.ExecuteOnMainThread(() =>
            {
                if (parent == null)
                {
                    OutputController(controller, perspective, navigatedUri);
                }
                else
                {
                    parent.LoadComplete();
                }
            });
        }

        /// <summary>
        /// Called when an error occurs during the loading of an <see cref="IMXController"/> instance.
        /// </summary>
        /// <param name="controller">The controller that failed to load.</param>
        /// <param name="ex">The exception that caused loading to fail.</param>
        protected override void OnControllerLoadFailed(IMXController controller, Exception ex)
        {
            Logger.Error("Controller load failed: ", ex);
            StopBlockingUserInput();
            throw new Exception(ex.Message, ex);
        }

        /// <summary>
        /// Called when the application should begin ignoring any new input from the user.
        /// This method should be overridden in the platform factory.
        /// </summary>
        protected virtual void OnBeginBlockingUserInput() { }

        /// <summary>
        /// Called when the application should begin accepting user input again.
        /// This method should be overridden in the platform factory.
        /// </summary>
        protected virtual void OnStopBlockingUserInput() { }

        /// <summary>
        /// Called when an <see cref="IMXView"/> is ready to be outputted.
        /// </summary>
        /// <param name="view">The view to be outputted.</param>
        protected virtual void OnOutputView(IMXView view)
        {
            PaneManager.Instance.DisplayView(view);
        }

        /// <summary>
        /// Called when the loading indicator should be displayed.
        /// </summary>
        /// <param name="title">The text to displayed with the loading indicator.</param>
        protected abstract void OnShowLoadIndicator(string title);

        /// <summary>
        /// Called when the loading sequence has begun.
        /// </summary>
        protected virtual void OnShowImmediateLoadIndicator() { }

        /// <summary>
        /// Called when the loading indicator should be hidden.
        /// </summary>
        protected abstract void OnHideLoadIndicator();

        /// <summary>
        /// Logs an unhandled exception with a fatal message using the factory's logger.
        /// </summary>
        /// <param name="e">The exception that was unhandled.</param>
        protected void LogUnhandledException(Exception e)
        {
            if (Logger != null)
            {
                Logger.Fatal("THE APPLICATION CAUGHT AN UNHANDLED EXCEPTION", e);
            }
        }

        /// <summary>
        /// Activates the timer for the load indicator.  The load indicator will be displayed with a default title after the default timeout.
        /// </summary>
        public void ActivateLoadTimer()
        {
            ActivateLoadTimer(null, DefaultLoadIndicatorDelay);
        }

        /// <summary>
        /// Activates the timer for the load indicator.  The load indicator will be displayed after the default timeout.
        /// </summary>
        /// <param name="title">The title to display on the load indicator.</param>
        public void ActivateLoadTimer(string title)
        {
            ActivateLoadTimer(title, DefaultLoadIndicatorDelay);
        }

        /// <summary>
        /// Activates the timer for the load indicator.  Once the timer has elapsed, the load indicator will be displayed.
        /// </summary>
        /// <param name="title">The title to display on the load indicator.</param>
        /// <param name="delay">The amount of time, in milliseconds, to wait before showing the load indicator.</param>
        public void ActivateLoadTimer(string title, double delay)
        {
            iApp.Thread.ExecuteOnMainThread(() =>
            {
                if (LoadTimer != null)
                {
                    if (LoadTimer.IsEnabled)
                    {
                        LoadTimer.Stop();
                    }
                    LoadTimer = null;
                }

                OnShowImmediateLoadIndicator();

                if (delay == 0)
                {
                    OnShowLoadIndicator(title ?? GetResourceString("Loading"));
                }
                else if (delay > 0)
                {
                    LoadTimer = Resolve<ITimer>();
                    if (LoadTimer == null)
                    {
                        OnShowLoadIndicator(title ?? GetResourceString("Loading"));
                        return;
                    }

                    LoadTimer.Interval = delay;
                    LoadTimer.Elapsed += (sender, e) =>
                    {
                        iApp.Thread.ExecuteOnMainThread(() =>
                        {
                            if (LoadTimer != null)
                            {
                                OnShowLoadIndicator(title ?? GetResourceString("Loading"));
                            }
                        });
                    };
                    LoadTimer.Start();
                }
            });
        }

        /// <summary>
        /// Invokes the <see cref="Device.OnNetworkResponse"/> event using the specified network response.
        /// </summary>
        /// <param name="networkResponse">The network response from a previous request.</param>
        public static void HandleNetworkResponse(NetworkResponse networkResponse)
        {
            Device.PostNetworkResponse(networkResponse);
        }

        /// <summary>
        /// Gets the date and time of the last action that occurred.
        /// </summary>
        /// <value>The last action date.</value>
        public DateTime LastActivityDate
        {
            get { return Device.LastActivityDate; }
            set { Device.LastActivityDate = value; }
        }

        #region ctor/abstract singleton initializers

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetFactory"/> class.
        /// </summary>
        protected TargetFactory()
            : base(null)
        {
            DefaultLoadIndicatorDelay = 250;
        }

        /// <summary>
        /// Initializes the singleton factory instance.
        /// </summary>
        /// <param name="newInstance">The singleton factory instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newInstance"/> is <c>null</c>.</exception>
        // public static void Initialize( T newInstance )
        public static void Initialize(TargetFactory newInstance)
        {
            if (newInstance == null)
                throw new ArgumentNullException();
            Instance = newInstance;
        }
        /// <summary>
        /// Initializes the singleton factory instance with the specified <see cref="iApp"/> instance.
        /// </summary>
        /// <param name="newInstance">The singleton factory instance.</param>
        /// <param name="app">The <see cref="iApp"/> instance to initialize the factory with.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="newInstance"/> is <c>null</c>.</exception>
        //public static void Initialize(T newInstance, iApp app)
        public static void Initialize(TargetFactory newInstance, iApp app)
        {
            Initialize(newInstance);
            TheApp = app;
        }

        #endregion

        #region static properties

        /// <summary>
        /// Gets whether the singleton factory instance has been initialized.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return Instance != null;
            }
        }

        /// <summary>
        /// Gets or sets the factory style.  This style differs between platforms
        /// and is used when an application style has not been set.
        /// </summary>
        public virtual Style Style
        {
            get { return _style ?? (_style = new Style()); }
            set { _style = value; }
        }
        private Style _style;

        #region singleton accessors

        /// <summary>
        /// Gets or sets the iFactr application on the factory.
        /// </summary>
        /// <value>The application as an <see cref="iApp"/> instance.</value>
        public static iApp TheApp
        {
            get
            {
                return Instance == null ? null : Instance.App as iApp;
            }
            set
            {
                var instance = ((TargetFactory)TargetFactory.Instance);
                instance.SessionDataRoot = instance.DataPath.AppendPath("session");
                if (!instance.File.Exists(instance.SessionDataPath))
                {
                    instance.File.CreateDirectory(instance.SessionDataPath);
                }
                // clear out the tmp directory
                if (!instance.File.Exists(instance.TempPath))
                {
                    instance.File.CreateDirectory(instance.TempPath);
                }
                else
                {
                    foreach (var file in instance.File.GetFileNames(instance.TempPath))
                    {
                        instance.File.Delete(file);
                    }
                    foreach (var directory in instance.File.GetDirectoryNames(instance.TempPath))
                    {
                        instance.File.DeleteDirectory(directory, true);
                    }
                }

                instance.Session["CurrentNavContext"] = new iApp.AppNavigationContext() { ActivePane = Pane.Tabs };

                SetApp(value);

                LoadLanguageResource();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets or sets the delegate to be executed when a layer is being navigated away from.
        /// </summary>
        [Obsolete("Use ShouldNavigateFrom override on iLayer")]
        public NavigationDelegate ShouldNavigateFromLayer { get; set; }

        #region abstract methods

        /// <summary>
        /// Renders the specified <see cref="iLayer"/> instance on the screen.
        /// </summary>
        /// <param name="layer">A <see cref="iLayer"/> representing the Layer value.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="layer"/> is <c>null</c>.</exception>
        public void OutputLayer(iLayer layer) { OutputLayer(layer, LastNavigationUrl); }

        /// <summary>
        /// Renders the specified <see cref="iLayer"/> instance on the screen.
        /// </summary>
        /// <param name="layer">A <see cref="iLayer"/> representing the Layer value.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the layer.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="layer"/> is <c>null</c>.</exception>
        public void OutputLayer(iLayer layer, string navigatedUri)
        {
            if (layer == null)
            {
                throw new ArgumentNullException("layer");
            }

            Thread.ExecuteOnMainThread(() =>
            {
                if (!OnOutputLayer(layer))
                {
                    OutputController(layer, layer.Name, navigatedUri);
                }
                else
                {
                    StopBlockingUserInput();
                }
            });
        }

        /// <summary>
        /// Outputs a layer from the specified network response.
        /// </summary>
        /// <param name="response">The network response from which to output a layer.</param>
        public virtual void OutputLayerResponse(NetworkResponse response)
        {
            iApp.Navigate(response.OutputFile);
        }

        /// <summary>
        /// Loads the specified layer with the specified parameters.
        /// </summary>
        /// <param name="navLayer">The <see cref="iLayer"/> instance to load.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey, TValue&gt;"/> representing the parameters to pass to the layer's Load method.</param>
        [Obsolete]
        public void LoadLayer(iLayer navLayer, Dictionary<String, String> parameters)
        {
            LoadLayer(null, navLayer, null, parameters);
        }

        /// <summary>
        /// Loads the specified layer with the specified parameters.
        /// </summary>
        /// <param name="fromView">The <see cref="IMXView"/> instance from which the navigation was initiated.</param>
        /// <param name="navLayer">The <see cref="iLayer"/> instance to load.</param>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the layer.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey, TValue&gt;"/> representing the parameters to pass to the layer's Load method.</param>
        internal virtual void LoadLayer(IMXView fromView, iLayer navLayer, string navigatedUri, Dictionary<String, String> parameters)
        {
            TryLoadController(this, fromView, navLayer, navigatedUri, parameters);
        }

        /// <summary>
        /// Called when the device changes orientation.
        /// </summary>
        /// <param name="orientation">The new device orientation.</param>
        protected virtual void OnOrientationChanged(iApp.Orientation orientation)
        {
            iApp.OnOrientationChanged(orientation);
        }

        /// <summary>
        /// Gets a value for scaling calculations on absolute pixels.
        /// </summary>
        /// <returns>A value for scaling a pixel value into a display-independent pixel.</returns>
        public virtual double GetDisplayScale()
        {
            return 1;
        }

        /// <summary>
        /// Cancels loading of the current controller and navigates to the specified url.
        /// </summary>
        /// <param name="url">The url of the controller to navigate to.</param>
        public override void Redirect(string url)
        {
            CancelLoad = true;
            if (iApp.CurrentNavContext.ActiveLayer != null)
            {
                iApp.CurrentNavContext.ActiveLayer.CancelLoadAndNavigate(url);
            }
            else
            {
                iApp.Navigate(url);
            }
        }

        /// <summary>
        /// Returns the path of the file that is associated with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the file to get the path of.</param>
        public virtual string RetrieveImage(string id)
        {
            Device.Log.Warn(string.Format("Image retrieval is not supported on the {0} platform", iApp.Factory.Platform));
            return null;
        }

        /// <summary>
        /// Sends the specified notification to the device's notification system.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public virtual void SetNotification(INotification notification)
        {
            Device.Log.Warn(string.Format("Notifications are not supported on the {0} platform", iApp.Factory.Platform));
        }

        /// <summary>
        /// Called when a layer is being navigated away from.
        /// </summary>
        /// <param name="link">The link that is being navigated to.</param>
        /// <param name="pane">The pane of the layer that is being navigated away from.</param>
        /// <param name="handler">The action to invoke if navigation should continue.</param>
        [Obsolete]
        protected internal virtual void ShouldNavigate(Link link, Pane pane, Action handler)
        {
            handler.Invoke();
        }

        #endregion

        #region ITargetFactory Members
        ///// <summary>
        ///// Gets the image path.
        ///// </summary>
        ///// <value>The image path as a <see cref="String"/> instance.</value>
        //public virtual string ImagePath
        //{
        //    get { return string.Empty; }
        //}
        /// <summary>
        /// Gets the path for application assets.
        /// </summary>
        /// <value>The application path as a <see cref="String"/> instance.</value>
        public virtual string ApplicationPath
        {
            get
            {
                return Device.ApplicationPath;
            }
        }

        /// <summary>
        /// Gets the path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string DataPath
        {
            get
            {
                return Device.DataPath;
            }
        }

        /// <summary>
        /// Gets a unique identifier for the device running the application.
        /// </summary>
        public virtual string DeviceId
        {
            get
            {
                //TODO: Figure out a better default... generate GUID into iApp.Settings?
                return null;
            }
        }

        /// <summary>
        /// Gets whether the factory is running on a large form-factor device.
        /// </summary>
        /// <value><c>true</c> if the device is large form factor; otherwise <c>false</c>.</value>
        public virtual bool LargeFormFactor
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Gets or sets the restrictions to impose on network GET methods.
        /// </summary>
        public virtual NetworkGetMethod NetworkGetMethod
        {
            get
            {
                return Device.NetworkGetMethod;
            }
            set
            {
                Device.NetworkGetMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the restrictions to impose on network POST methods.
        /// </summary>
        public virtual NetworkPostMethod NetworkPostMethod
        {
            get
            {
                return Device.NetworkPostMethod;
            }
            set
            {
                Device.NetworkPostMethod = value;
            }
        }

        /// <summary>
        /// Gets the session-scoped path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string SessionDataPath
        {
            get
            {
                string _sessionDataPath = SessionDataRoot.AppendPath(SessionDataAppend);
                File.EnsureDirectoryExists(_sessionDataPath);
                return _sessionDataPath;
            }
        }

        /// <summary>
        /// Gets the session-scoped root path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string SessionDataRoot
        {
            get
            {
                return Device.Instance.SessionDataRoot;
            }
            set
            {
                Device.Instance.SessionDataRoot = value;
            }
        }

        /// <summary>
        /// Gets or sets the session-scoped appended path for application data.
        /// </summary>
        /// <value>The data path as a <see cref="String"/> instance.</value>
        public virtual string SessionDataAppend
        {
            get
            {
                return Device.Instance.SessionDataAppend;
            }

            set
            {
                Device.Instance.SessionDataAppend = value;
            }
        }

        /// <summary>
        /// Gets the path for temporary application data.  The contents of this path are removed on application startup.
        /// </summary>
        /// <value>The temp path as a <see cref="String"/> instance.</value>
        public virtual string TempPath
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the application's logging utility.
        /// </summary>
        /// <remarks>The default ILog implementation is the NullLogger</remarks>
        public virtual ILog Logger
        {
            get { return Device.Log; }
            set { Device.Log = value; }
        }

        /// <summary>
        /// Gets the application's file system interface.
        /// </summary>
        public virtual IFile File
        {
            get { return Device.File; }
        }

        /// <summary>
        /// Gets the application's data encryptor.
        /// </summary>
        public virtual IEncryption Encryption
        {
            get { return Device.Encryption; }
        }


        /// <summary>
        /// Gets the application's image compositor.
        /// </summary>
        public abstract ICompositor Compositor
        {
            get;
        }

        /// <summary>
        /// Gets the application's threading utility.
        /// </summary>
        public virtual IThread Thread
        {
            get { return Device.Thread; }
        }

        /// <summary>
        /// Gets the application's networking utility.
        /// </summary>
        public virtual INetwork Network
        {
            get
            {
                return Device.Network;
            }
        }

        /// <summary>
        /// Gets the application's session settings.
        /// </summary>
        public new virtual ISession Session { get { return MXContainer.Session; } }

        /// <summary>
        /// Gets the application's settings.
        /// </summary>
        public abstract ISettings Settings { get; }

        /// <summary>
        /// Gets the application's configuration settings.
        /// </summary>
        public abstract IConfig Config { get; }

        #endregion

        /// <summary>
        /// Gets the platform that the factory is running on.
        /// </summary>
        public MobilePlatform Platform
        {
            get { return Device.Platform; }
        }

        /// <summary>
        /// Gets the target that the factory is running on.
        /// </summary>
        public virtual MobileTarget Target
        {
            get { return MobileTarget.Unknown; }
        }

        /// <summary>
        /// Gets or sets the language code to use for localization.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <c>null</c> or an empty string.</exception>
        public string LanguageCode
        {
            get { return (string)(iApp.Session.GetValueOrDefault("LanguageCode") ?? (iApp.Session["LanguageCode"] = CultureInfo.CurrentUICulture.Name)); }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Language code cannot be null or empty.", "value");
                iApp.Session["LanguageCode"] = value;
            }
        }

        private static void LoadLanguageResource()
        {
            if (TheApp == null)
                throw new NullReferenceException("An instance of iApp is required to load language resources.");

            Device.Resources.RemoveAllResources();
            Device.Resources.AddResources("iFactr.UI.Resources.Strings", Device.Reflector.GetAssembly(typeof(TargetFactory)));
            Device.Resources.AddResources(Device.Reflector.GetAssembly(TheApp.GetType()));
        }

        /// <summary>
        /// Returns a localized string with the specified name from a string resource file.
        /// If no appropriate string is found, null is returned.
        /// </summary>
        /// <param name="name">The name of the localized string to return.</param>
        /// <seealso href="http://support.ifactr.com/kb/utilities/localization"/>
        public virtual string GetResourceString(string name)
        {
            if (Device.Resources.Count == 0)
                LoadLanguageResource();

            try
            {
                return Device.Resources.GetString(name, new CultureInfo(LanguageCode));
            }
            catch
            {
                try
                {
                    return Device.Resources.GetString(name, new CultureInfo(LanguageCode.Substring(0, 2)));
                }
                catch
                {
                    return Device.Resources.GetString(name, CultureInfo.CurrentUICulture);
                }
            }
        }

        /// <summary>
        /// Measures the amount of space that a single line of text will take up when it is rendered with the specified font.
        /// </summary>
        /// <param name="font">The <see cref="iFactr.UI.Font"/> with which to measure a line of text.</param>
        /// <returns>The render height of the string.</returns>
        protected internal abstract double GetLineHeight(Font font);
    }
}
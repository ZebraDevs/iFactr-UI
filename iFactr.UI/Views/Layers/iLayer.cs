using iFactr.Core.Forms;
using iFactr.Core.Styles;
using iFactr.Core.Targets;
using iFactr.UI;
using MonoCross.Navigation;
using MonoCross.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Button = iFactr.Core.Controls.Button;
using Link = iFactr.Core.Controls.Link;

/*
This namespace contains all of the classes that represent the abstract
implementation of a layer as well the objects that can be directly placed on a
layer.

The abstract idea of a layer is generally defined as what would be necessary as
a screen on a base platform, although richer targets and platforms may support
situations where multiple layers are displayed to the user simultaneously in
order to provide the optimal user experience, for instance the NavigationTabs
concept.
*/
namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents the base implementation for all iFactr layers.  This class is abstract.
    /// <para> </para>
    /// <para><img src="Diagrams\iLayer.cd"/></para>
    /// </summary>
    /// <remarks>
    /// <para><b>Layer Types </b> </para>
    /// <para>A Layer is the foundational construct for all iFactr applications. Each
    /// Layer represents a self-contained piece of application functionality. There are
    /// two layer types in an iFactr application. </para>
    /// <para> </para>
    /// <para>A <b>Layer </b>is comprised of navigation items, (lists and menus), and
    /// items used for display of information, (blocks and panels). A Navigation Layer's
    /// primary purpose is to provide information and workflow options within your
    /// application. </para>
    /// <para> </para>
    /// <para>A <b>FormLayer</b> is comprised of form items, or fields, that are used to
    /// capture user-provided information. Form Layers provide the basic input views for
    /// an iFactr application, and are accessed through the navigation map as any other
    /// layer, but a form layer has an action property that specifies the endpoint for
    /// submission of form values. Form layers must provide for adequate processing,
    /// (save and/or cancel), and may provide validation of fields before processing to
    /// an external endpoint. Form Layers are discussed in more detail below. </para>
    /// </remarks>
    public abstract class iLayer : IMXController
    {
        #region Properties

        /// <summary>
        /// Gets a GUID for this instance when it is cloned.
        /// </summary>
        [XmlIgnore]
        public string ID { get; protected set; }

        /// <summary>
        /// Gets or sets the name of this instance.  Layer equality is determined by this property.
        /// Layers with the same name will occupy the same place in the history stack.
        /// </summary>
        /// <value>The layer name as a <see cref="String"/> value.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title for this instance.
        /// </summary>
        /// <value>The layer title as a <see cref="String"/> value.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URI that points to this instance within the NavigationMap.
        /// </summary>
        /// <value>The URI as a <see cref="String"/> value.</value>
        [XmlIgnore]
        public string MapUri { get; set; }

        /// <summary>
        /// Gets or sets any parameters to pass along when this instance is submitted.
        /// </summary>
        /// <value>The parameters to pass through.</value>
        /// <remarks>Parameters in this collection are equivalent to hidden form fields.</remarks>
        [XmlIgnore]
        public SerializableDictionary<string, string> ActionParameters
        {
            get { return Parameters; }
            set { Parameters = value; }
        }

        /// <summary>
        /// Gets or sets the action buttons for this instance.  These generally appear in the form of a popup menu, depending on the platform.
        /// </summary>
        /// <value>An <see cref="ItemsCollection&lt;T&gt;"/> of <see cref="Button"/> objects.</value>
        public virtual ItemsCollection<Button> ActionButtons { get; set; }

        /// <summary>
        /// Gets or sets the UI elements to render within this instance.
        /// </summary>
        /// <value>An <see cref="ItemsCollection&lt;T&gt;"/> of <see cref="iLayerItem"/> objects.</value>
        public ItemsCollection<iLayerItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the item to scroll to when the layer is presented.
        /// </summary>
        public object FocusedItem { get; set; }

        /// <summary>
        /// Gets or sets the URI that was used to navigate to this instance.
        /// </summary>
        /// <value>The URI as a <see cref="String"/> value.</value>
        [Obsolete("Use NavContext.NavigatedUrl instead.")]
        [XmlIgnore]
        public string NavigationUri
        {
            get { return NavContext.NavigatedUrl; }
            set { NavContext.NavigatedUrl = value; }
        }

        /// <summary>
        /// Gets or sets the layout of this instance.
        /// </summary>
        public LayerLayout Layout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can scroll up and down.
        /// </summary>
        public bool IsScrollable { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Button"/> that will be used as the back button.  A value of null will use the default back button.
        /// </summary>
        /// <value>The back button as a <see cref="Button"/> instance.</value>
        /// <remarks>To hide the back button, set its action type to ActionType.None.</remarks>
        public Button BackButton { get; set; }

        /// <summary>
        /// Gets or sets the keyboard shortcuts for this instance.
        /// </summary>
        /// <value>The <see cref="Gesture"/>-indexed collection as a <see cref="Dictionary&lt;TKey,TValue&gt;"/> instance. </value>
        public Shortcuts ShortcutGestures { get; set; }

        /// <summary>
        /// Gets or sets the action button to use when this instance is part of a composite layer set.
        /// The button is only used on large form-factor targets and defines the action button for the composite layer set.
        /// </summary>
        /// <value>The composite action button as a <see cref="Button"/>.</value>
        public Button CompositeActionButton { get; set; }

        /// <summary>
        /// Gets or sets the link to the next layer for composite layer layout on large form-factor targets.
        /// </summary>
        /// <value>The composite layer link as a <see cref="UI.Link"/>.</value>
        public UI.Link CompositeLayerLink { get; set; }

        /// <summary>
        /// Gets or sets the layout of the composite layer on large form-factor targets.
        /// </summary>
        /// <value>The composite layer layout as a <see cref="CompositeLayerLayout"/> value.</value>
        public CompositeLayout CompositeLayerLayout { get; set; }

        /// <summary>
        /// Gets or sets the root composite layer on large form-factor targets.
        /// </summary>
        public iLayer CompositeParent { get; set; }

        /// <summary>
        /// Gets or sets the navigation context containing information about this instance's last navigation.
        /// </summary>
        [Obsolete("This concept has been deprecated; the navigated URI is passed to the Load method")]
        [XmlIgnore]
        public NavigationContext NavContext { get; set; }

        /// <summary>
        /// Gets or sets the detail pane link for this instance.  When set and this instance is loaded in the master pane,
        /// this link will automatically be navigated to and the resultant layer will be displayed in the detail pane.
        /// </summary>
        /// <value>The detail pane link as a <see cref="UI.Link"/>.</value>
        public UI.Link DetailLink { get; set; }

        /// <summary>
        /// Gets or sets the style of this instance.  If this is not set, the application style will be used.
        /// </summary>
        /// <value>The layer style as a <see cref="Style"/> instance.</value>
        public Style LayerStyle
        {
            get { return _layerStyle ?? (_layerStyle = (Style)iApp.Factory.Style.Clone()); }
            set { _layerStyle = value; }
        }
        private Style _layerStyle;

        /// <summary>
        /// Gets or sets the style in which the layer should be presented when rendered in a popover.
        /// </summary>
        /// <value>The popover mode as a <see cref="PopoverPresentationStyle"/> instance.</value>
        public PopoverPresentationStyle PopoverPresentationStyle { get; set; }

        #endregion

        #region Layer initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="iLayer"/> class.
        /// </summary>
        public iLayer()
        {
            ID = string.Format("frm{0}", GetType().ToString().Replace(".", "_"));
            Name = GetType().ToString();
            Parameters = new Dictionary<string, string>();
            ActionButtons = new List<Button>();
            Items = new List<iLayerItem>();
            Title = string.Empty;
            NavContext = new NavigationContext();
            IsScrollable = true;
            ShortcutGestures = new Shortcuts();
            ValidationErrors = new Dictionary<string, ValidationInfo>();
        }

        /// <summary>
        ///  Loads this instance with the specified parameters.  This method is meant to be overridden and should be
        ///  where the layer's UI elements are added.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted field values.</param>
        public virtual void Load(Dictionary<string, string> parameters) { }

        /// <summary>
        ///  Loads this instance with the specified parameters.  This method is meant to be overridden and should be
        ///  where the layer's UI elements are added.
        /// </summary>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted field values.</param>
        public virtual void Load(string navigatedUri, Dictionary<string, string> parameters) { Load(parameters); }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public iLayer Clone()
        {
            return (iLayer)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected virtual object CloneOverride()
        {
            var newLayer = (iLayer)MemberwiseClone();
            newLayer.ID = Guid.NewGuid().ToString();

            newLayer.ActionButtons = new List<Button>(ActionButtons.Select(a => a.Clone()));
            newLayer.Items = new List<iLayerItem>(Items.Select(i =>
            {
                var clone = i.Clone();
                if (i == FocusedItem)
                {
                    newLayer.FocusedItem = clone;
                }
                else
                {
                    var list = i as IList;
                    int index = -1;
                    if (list != null && (index = list.IndexOf(FocusedItem)) >= 0)
                    {
                        newLayer.FocusedItem = ((IList)clone)[index];
                    }
                }
                return clone;
            }));

            if (BackButton != null)
                newLayer.BackButton = BackButton.Clone();
            if (CompositeLayerLink != null)
                newLayer.CompositeLayerLink = CompositeLayerLink.Clone();
            if (CompositeActionButton != null)
                newLayer.CompositeActionButton = CompositeActionButton.Clone();
            if (DetailLink != null)
                newLayer.DetailLink = DetailLink.Clone();
            if (FieldValuesRequested != null)
                newLayer.FieldValuesRequested = FieldValuesRequested;
            newLayer.LayerStyle = LayerStyle.Clone();

            return newLayer;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Title + "/" + Name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Programmatically selects an <see cref="iItem"/>.
        /// </summary>
        /// <param name="collection">The collection to check for a SelectEventHandler.</param>
        /// <param name="item">The <see cref="iItem"/> that is to be selected.</param>
        public void Select(iCollection<iItem> collection, iItem item)
        {
            if (collection.HasSelectEventHandler)
                collection.ItemSelected(item);
            else if (item != null && item.Link != null && item.Link.Address != null)
                iApp.Navigate(item.Link, View);
        }

        /// <summary>
        /// Sets the value of each <see cref="iFactr.Core.Forms.Field"/> to the corresponding value within the specified collection.
        /// See remarks for more information.
        /// </summary>
        /// <param name="parameters">The values for some or all of the fields.</param>
        /// <remarks> If a key in the dictionary matches the Legend + '.' + Field.ID, then that
        /// KeyValuePair's value is converted to the appropriate type.  Format Exceptions
        /// will bubble up.  This does not need to be a 1-to-1 correlation of KeyValuePairs
        /// to Fields.  This method can be used to partially populate the fieldsets.</remarks>
        public void SetFieldValues(Dictionary<string, string> parameters)
        {
            foreach (var fieldset in Items.OfType<Fieldset>())
            {
                foreach (var field in fieldset)
                {
                    var name = field.ID.Replace(fieldset.Header + ".", string.Empty);
                    if (!parameters.ContainsKey(name)) continue;
                    if (field is SelectListField)
                        (field as SelectListField).SelectedKey = parameters[name + ".Key"];
                    else if (field is DateField)
                        (field as DateField).Value = string.IsNullOrEmpty(parameters[name]) ? default(DateTime?) : Convert.ToDateTime(parameters[name]);
                    else if (field is SliderField)
                    {
                        float value = 0;

                        try
                        {
                            value = float.Parse(parameters[name]);
                        }
                        catch { }

                        (field as SliderField).Value = value;
                    }
                    else if (field is LabelField)
                    {
                        if (field.Text.IsNullOrEmptyOrWhiteSpace())
                            field.Label = parameters[name];
                        else
                            field.Text = parameters[name];
                    }
                    else field.Text = parameters[name];
                }
            }
        }

        #endregion

        #region IMXController members

        /// <summary>
        /// The type of the model used by this controller.
        /// </summary>
        Type IMXController.ModelType
        {
            get { return GetType(); }
        }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        /// <returns></returns>
        object IMXController.GetModel()
        {
            return this;
        }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">A <see cref="T:System.Collections.Generic.Dictionary`2" /> representing any parameters such as submitted values.</param>
        /// <returns></returns>
        string IMXController.Load(Dictionary<string, string> parameters)
        {
            return ((IMXController)this).Load(null, parameters);
        }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="navigatedUri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        string IMXController.Load(string navigatedUri, Dictionary<string, string> parameters)
        {
            int startTick = Environment.TickCount;
            iApp.Log.Debug("Loading Layer: " + this);

            Load(navigatedUri, parameters);
            if (CancelLoad)
            {
                iApp.Log.Info("Loading canceled for: " + this);
                return null;
            }

            iApp.Log.Metric(string.Format("Loading Layer cost {0}ms for: {1}", Environment.TickCount - startTick, this));

            return Name;
        }

        /// <summary>
        /// Gets or sets the parameters added to the controller.
        /// </summary>
        [XmlIgnore]
        public virtual SerializableDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets the <see cref="T:MonoCross.Navigation.IMXView" /> that is loaded into the controller from a <see cref="T:MonoCross.Navigation.ViewPerspective" />.
        /// </summary>
        [Obsolete]
        [XmlIgnore]
        public IMXView View { get; internal set; }

        #endregion

        #region Load complete

        /// <summary>
        /// Represents the method that will handle layer events.
        /// </summary>
        public delegate void LayerEventHandler(iLayer layer);

        /// <summary>
        /// Occurs when the layer finishes loading.
        /// </summary>
        public event Action<iLayer> OnLoadComplete;

        /// <summary>
        /// Gets a value indicating whether this instance has an OnLoadComplete delegate defined.
        /// </summary>
        /// <value><c>true</c> if this instance has an OnLoadComplete delegate defined; otherwise <c>false</c>.</value>
        public bool HasOnLoadCompleteDelegate
        {
            get { return OnLoadComplete != null; }
        }

        /// <summary>
        /// Called when this instance has finished loading.
        /// </summary>
        public void LoadComplete()
        {
            var loadComplete = OnLoadComplete;
            if (loadComplete != null) loadComplete(this);
            else TargetFactory.TheApp.LayerLoadCompleted(this);
        }

        #endregion

        #region Cancel Load

        /// <summary>
        /// Gets or sets whether to pass this instance to the bindings for rendering.
        /// </summary>
        [XmlIgnore]
        public bool CancelLoad { get; set; }

        /// <summary>
        /// Cancels loading of the current layer and navigates to the specified uri.
        /// </summary>
        /// <param name="uri">The uri of the layer to navigate to.</param>
        public void CancelLoadAndNavigate(string uri)
        {
            CancelLoadAndNavigate(uri, new Dictionary<string, string>());
        }

        /// <summary>
        /// Cancels loading of the current layer and navigates to the specified uri with the given parameters.
        /// </summary>
        /// <param name="uri">The uri of the layer to navigate to.</param>
        /// <param name="parameters">The parameters to pass along to the next layer.</param>
        public void CancelLoadAndNavigate(string uri, Dictionary<string, string> parameters)
        {
            CancelLoadAndNavigate(new Link { Address = uri, Parameters = parameters, });
        }

        /// <summary>
        /// Cancels loading of the current layer and performs a navigation using the specified Link.
        /// </summary>
        /// <param name="link">The <see cref="Link"/> to navigate to.</param>
        public void CancelLoadAndNavigate(Link link)
        {
            CancelLoad = true;
            if (link == null || link.Address == null)
            {
                throw new ArgumentNullException("link", "Null URI supplied to CancelLoadAndNavigate.");
            }
            link.Parameters.AddRange(ActionParameters);

            iApp.Thread.ExecuteOnMainThread(() =>
            {
                var fromView = PaneManager.Instance.FromNavContext(NavContext.OutputOnPane).CurrentView;
                iApp.Navigate(link, fromView);
            });
        }

        #endregion

        #region Layer refresh

        /// <summary>
        /// Occurs when a refresh of the layer is requested.
        /// </summary>
        public event EventHandler iLayerRefreshRequested;

        /// <summary>
        /// Requests a refresh of this instance. 
        /// </summary>
        public void RequestRefresh()
        {
            if (LayerRefreshAvailable)
                iLayerRefreshRequested(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets whether this instance can accept refresh requests. 
        /// </summary>
        public bool LayerRefreshAvailable
        {
            get { return iLayerRefreshRequested != null; }
        }

        #endregion

        #region Get field values

        /// <summary>
        /// Occurs when the layer gets its field values.
        /// </summary>
        public event Func<object, Dictionary<string, string>> FieldValuesRequested;

        /// <summary>
        /// Requests the current values of each <see cref="iFactr.Core.Forms.Field"/> on the layer.
        /// This does not validate the layer.  Call Validate to validate the returned parameters.
        /// This method will throw a NotImplementedException on web targets.
        /// </summary>
        public virtual Dictionary<string, string> GetFieldValues()
        {
            if (FieldValuesRequested != null)
                return FieldValuesRequested(this);

            throw new NotImplementedException("GetFieldValues is not available on web-based applications.");
        }

        #endregion

        #region Validation

        /// <summary>
        /// Gets whether each <see cref="iFactr.Core.Forms.Field"/> on the layer passes validation.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return ValidationErrors == null || ValidationErrors.Count == 0 ||
                       ValidationErrors.Values.All(error => error.BrokenRulesPerField.Count == 0);
            }
        }

        /// <summary>
        /// Gets a <see cref="Dictionary&lt;TKey,TValue&gt;"/> of validation errors for all fields on the layer.
        /// Each field can have a list of broken validation rules.
        /// </summary>
        [XmlIgnore]
        public SerializableDictionary<string, ValidationInfo> ValidationErrors { get; private set; }

        /// <summary>
        /// Validates a set of parameters based on the validation rules of each <see cref="iFactr.Core.Forms.Field"/> on the layer.
        /// </summary>
        /// <param name="parameters">The parameters to validate.</param>
        /// <remarks>The parameter values are matched to <see cref="Field"/>s by the Key and Field.Name</remarks>
        public void Validate(Dictionary<string, string> parameters)
        {
            ValidationErrors.Clear();
            //a new list is created here so that error fieldsets can be added on-the-fly
            foreach (var field in Items.OfType<Fieldset>()
                .SelectMany(fieldset => fieldset.Where(field => field != null && field.Validate != null
                && parameters.Keys.Contains(field.ID))))
            {
                // Validation is scheduled for the field
                // Use the posted value to perform validation.
                string postedValue = parameters[field.ID];

                // Invoke any validation rules...
                field.ClearBrokenRules();
                field.Validate(postedValue, string.Empty);

                // Store all findings...
                var validationInfo = ValidationErrors[field.ID] = new ValidationInfo(postedValue, field.BrokenRules);
                if (validationInfo.BrokenRulesPerField.Count == 0)
                {
                    if (field is LabelField && field.Text.IsNullOrEmptyOrWhiteSpace())
                        field.Label = validationInfo.OriginalFieldValue;
                    else
                        field.Text = validationInfo.OriginalFieldValue;
                }
            }
        }

        #endregion

        #region Layer teardown

        /// <summary>
        /// Called when this instance is about to be removed from the history stack or placed underneath another layer.
        /// </summary>
        /// <returns><c>true</c> if the navigation should be allowed to proceed; otherwise <c>false</c>.</returns>
        /// <param name="link">The link containing information about the navigation taking place.</param>
        /// <param name='navigationType'>The type of navigation that caused the method to be called.</param>
        public virtual bool ShouldNavigateFrom(UI.Link link, NavigationType navigationType)
        {
            return true;
        }

        /// <summary>
        /// Called when this instance is unloaded from memory.  On native targets, this is called when the layer is popped from the history stack.
        /// On web targets,  this is called immediately after the layer is outputted.
        /// </summary>
        [Obsolete("Use ShouldNavigateFrom instead.")]
        public virtual void Unload()
        {
        }

        /// <summary>
        /// Removes all UI elements and validation errors from the layer.
        /// </summary>
        public virtual void Clear()
        {
            CancelLoad = false;
            BackButton = null;
            DetailLink = null;
            CompositeLayerLink = null;
            CompositeActionButton = null;
            Parameters.Clear();
            ActionButtons.Clear();
            Items.Clear();
            ValidationErrors.Clear();
            ShortcutGestures.Clear();
        }

        #endregion

        #region Equality comparer overrides

        /// <summary>
        /// Tests for equality between two <see cref="iLayer"/> instances.
        /// </summary>
        /// <param name="layer1">The first layer to test.</param>
        /// <param name="layer2">The second layer to test.</param>
        /// <returns><c>true</c> if the layers are equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(iLayer layer1, iLayer layer2)
        {
            if ((object)layer1 == null)
                return (object)layer2 == null;
            return layer1.Equals(layer2);
        }

        /// <summary>
        /// Tests for inequality between two <see cref="iLayer"/> instances.
        /// </summary>
        /// <param name="layer1">The first layer to test.</param>
        /// <param name="layer2">The second layer to test.</param>
        /// <returns><c>true</c> if the layers are not equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(iLayer layer1, iLayer layer2)
        {
            if ((object)layer1 == null)
                return (object)layer2 != null;
            return !layer1.Equals(layer2);
        }

        /// <summary>
        /// Determines if the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to test for equality.</param>
        /// <returns><c>true</c> if the object is equal to this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var comparingLayer = obj as iLayer;
            return comparingLayer != null && Name == comparingLayer.Name;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            int hCode = 0;
            if (Name != null) hCode += Name.GetHashCode();
            if (Title != null) hCode += Title.GetHashCode();
            if (NavContext != null)
            {
                hCode += NavContext.NavigatedActivePane.GetHashCode();
                hCode += NavContext.NavigatedActiveTab.GetHashCode();
            }

            return hCode;
        }

        #endregion

        /// <summary>
        /// Represents a layer navigation context containing information of the last navigation.
        /// </summary>
        [Obsolete("This concept has been deprecated and will not be available in future versions.")]
        public class NavigationContext
        {
            /// <summary>
            /// Gets or sets the URL that was used to navigate to the layer.
            /// </summary>
            /// <value>The URL as a <see cref="String"/> value.</value>
            public string NavigatedUrl { get; set; }

            /// <summary>
            /// Gets or sets the tab that was selected when the layer was navigated to.
            /// </summary>
            /// <value>The active tab as a <see cref="Int32"/> value.</value>
            public int NavigatedActiveTab { get; set; }

            /// <summary>
            /// Gets or sets the pane from which the navigation to the layer was initiated.
            /// </summary>
            /// <value>The active pane as a <see cref="iFactr.UI.Pane"/> value.</value>
            public Pane NavigatedActivePane { get; set; }

            /// <summary>
            /// Gets or sets the pane that the layer was rendered on.
            /// </summary>
            /// <value>
            /// The pane as a <see cref="iFactr.UI.Pane"/> value.
            /// </value>
            public Pane OutputOnPane { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether to clear the large form-factor panel history when the layer is rendered.
            /// </summary>
            /// <value><c>true</c> if the large form-factor panel history is to be cleared; otherwise <c>false</c>.</value>
            public bool ClearPaneHistoryOnOutput { get; set; }
        }
    }
}
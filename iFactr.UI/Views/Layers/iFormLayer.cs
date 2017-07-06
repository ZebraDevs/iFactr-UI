using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using iFactr.Core.Controls;
using iFactr.Core.Forms;
using iFactr.UI;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a layer that contains form fields for accepting user input.  This class is abstract.
    /// </summary>
    /// <remarks>
    /// the <c>FormLayer </c>class is comprised of form items, or fields, that are used to capture user-provided information. Form Layers provide the basic input views for an iFactr application, and are accessed through the navigation map as any other layer, but a form layer has an action property that specifies the endpoint for submission of form values. Form layers must provide for adequate processing, (save and/or cancel), and may provide validation of fields before processing to an external endpoint. 
    /// <para> </para>
    /// <para><b>Using Forms</b></para>
    /// <para>The iFactr Framework supports user-provided input through the use of Form
    /// Layers. Form Layers differ from Navigation Layers in a few key areas: </para>
    /// <para> </para>
    /// <list type="bullet">
    /// <item>
    /// <description>Forms are composed of sets of input (fieldsets), rather than layer
    /// items</description></item>
    /// <item>
    /// <description>Forms cannot contain navigation-layer items, (lists, menus, panels,
    /// and blocks). </description></item>
    /// <item>
    /// <description>A form layer includes an Action property, that defines the
    /// navigation endpoint, (i.e. Layer), on which the form is processed.
    /// </description></item>
    /// <item>
    /// <description>The form layer fields are included in the parameter dictionary of
    /// the layer defined on the Action property. </description></item>
    /// <item>
    /// <description>Form layer fields can be carried forward in a muti-layer form
    /// workflow by setting the ActionParameters property on the form.
    /// </description></item></list>
    /// <para> </para>
    /// <para> </para>
    /// <para><b>Parts of a Form</b> </para>
    /// <para>iFactr Form Layers are composed of three parts, which form a logical
    /// structure and enable rich layouts accepting multiple input data types. The three
    /// parts of a form are: </para>
    /// <para> </para>
    /// <list type="bullet">
    /// <item>
    /// <description><i>The Form Layer</i> - (represented by the <c>FormLayer </c>class); this container represents the layer itself, and contains the ActionButton property, and Action Parameters dictionary. All form fields are rolled-up from this container and submitted to the Action Layer as a whole. </description></item>
    /// <item>
    /// <description><i>One or more Fieldsets</i> - (represented by the Fieldset class);
    /// a fieldset is a logical grouping of form fields, and has a legend property that
    /// will display text as a header to the group. A form can contain multiple
    /// fieldsets allowing you to organize your form however you desire.
    /// </description></item>
    /// <item>
    /// <description><i>One or more Form Fields</i> - (represented by the Field abstract
    /// class + concrete field types); Fields are the information-gathering controls on
    /// your form. iFactr currently supports the following Form Fields:
    /// </description></item></list>
    /// <para> o <c>BoolField</c> - a dual-state switch field, (i.e. True/False or On/Off) </para>
    /// <para> o <c>TextField</c> - a single-line of text </para>
    /// <para> o <c>NumericField</c> - a single line numeric value </para>
    /// <para> o <c>SelectListField</c> - a select list of pre-defined values </para>
    /// <para> o <c>MultiLineTextField</c> - multiple lines of text </para>
    /// <para> o <c>DateField</c> - a date/time value </para>
    /// <para> o <c>SliderField</c> - a variable slider field, (e.g. volume control)</para>
    /// <para> o <c>BarcodeField</c> - a barcode scanning field (translates to numeric) </para>
    /// <para> o <c>ImagePickerField</c> - a cover-flow image picker control </para>
    /// <para> </para>
    /// <para><b>Adding Forms to an Application</b> </para>
    /// <para>Let's suppose we want to add a simple survey form to our application. We
    /// will need to create a new Survey layer and add an iForm instance in the OnLoad()
    /// method. Currently the iForm and Fieldset classes use the .NET/C# extended
    /// constructor syntax to provide for maximum flexibility when constructing your
    /// Form Layers. Fieldsets and fields can be built following the form-flow desired
    /// by setting the various iForm properties within the extended constructor
    /// initializers. </para>
    /// <para> </para>
    /// <para>The base iForm constructor takes the name of your form. This name needs to
    /// be unique across all forms in your application, so name it carefully. </para>
    /// <para> </para>
    /// <para></para>
    /// <code lang="C#">      new iForm(&quot;SystemInfo&quot;){ . .
    /// .</code>
    /// <para> </para>
    /// <para> </para>
    /// <para>Next, you'll add one or more Fieldset instances to your form. You may
    /// optionally give the fieldset a name by setting the Legend property.</para>
    /// <para> </para>
    /// <code lang="C#">      Fieldsets = { new Fieldset()
    ///       {
    ///           Legend = &quot;Project Info&quot;,
    ///            . . .  </code>
    /// <para> </para>
    /// <para>Now you're ready to add some fields. Simply set the Fields property on the
    /// form with new instances of the field types you want to add to the form. </para>
    /// <para> </para>
    /// <code lang="C#">      Fields = {
    ///                 new SelectListField() {
    ///                     Name = &quot;Technology.Language&quot;,
    ///                     Items = new List&lt;SelectListFieldItem&gt;()
    /// 
    ///                     {
    ///                         new SelectListFieldItem(&quot;C# .NET),
    /// 
    ///                         new SelectListFieldItem(&quot;COBOL),
    ///                         new SelectListFieldItem(&quot;Java J2EE),
    /// 
    ///                         new SelectListFieldItem(&quot;Ruby on Rails),
    /// 
    ///                         new SelectListFieldItem(&quot;Other),
    ///                     },
    ///                     Label = &quot;Language:&quot;,
    ///                },
    ///                . . .   </code>
    /// <para> </para>
    /// <para>The ActionButtons collection is where you will define your save/submit button,
    /// as well as an optional cancel button. Every form must have a save/submit button
    /// to enable saving of all form field values to the application. The cancel button
    /// is optional; if you choose not to define a cancel button, the usual back button
    /// will be displayed and serve the same function. Unlike some iPhone applications,
    /// the back button in an iFactr application does not save the form values to enable
    /// cross-platform support. An explicit cancel button provides</para>
    /// <para>clarity for the user, and is the recommended approach.</para>
    /// <para> </para>
    /// <code lang="C#">      ActionButtons = new List&lt;Button&gt; {
    ///                                   new Button(&quot;Submit&quot;) {
    ///                                       Action = ActionType.Submit; },
    ///                                   new Button(&quot;Cancel&quot;) {
    ///                                       Action = ActionType.Cancel; }
    /// 
    ///                                  },
    ///                                  . . .  </code>
    /// <para> </para>
    /// <para>Next, you need to set the Action property of your form. The Action
    /// property must be set to the navigation map endpoint of the Layer on which you
    /// want to process the form. Action = &quot;Surveys&quot;, Finally you can
    /// optionally set the ActionParameters property of your form to the parameters
    /// dictionary passed-in to your layer. This will ensure the parameters collection
    /// is carried forward to the Layer defined on the Action property, and are
    /// subsequently available as you progress through the workflow. This technique is
    /// used to enable multiple-form workflows with a single form-post to a backend
    /// service when the entire workflow is complete. ActionParameters = parameters
    /// </para>
    /// </remarks>
    public abstract class FormLayer : iLayer
    {
        /// <summary>
        /// Gets or sets the address to the submission endpoint of a composite layer set.
        /// This is only used on large form-factor targets.
        /// </summary>
        /// <value>The composite form action as a <see cref="String"/> value.</value>
        public string CompositeFormAction
        {
            get { return CompositeActionButton == null ? null : CompositeActionButton.Address; }
            set
            {
                if (CompositeActionButton == null)
                    CompositeActionButton = new SubmitButton("Submit");

                CompositeActionButton.Address = value;
            }
        }

        /// <summary>
        /// Gets or sets the layout of the layer.
        /// </summary>
        /// <value>
        /// The layout as a <see cref="FormLayout"/> value.
        /// </value>
        [XmlIgnore]
        public new FormLayout Layout
        {
            get { return (FormLayout)base.Layout; }
            set { base.Layout = (LayerLayout)value; }
        }

        /// <summary>
        /// Gets or sets the action button that will submit the form.
        /// </summary>
        /// <value>
        /// The action button as a <see cref="SubmitButton"/> instance.
        /// </value>
        [XmlIgnore]
        public SubmitButton ActionButton
        {
            get { return (SubmitButton)ActionButtons.FirstOrDefault(b => b is SubmitButton); }
            set { ActionButtons.Insert(0, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="Fieldset"/>s that contain the fields to render on the layer.
        /// </summary>
        /// <value>
        /// The collection of <see cref="Fieldset"/>s as an <see cref="ItemsCollection&lt;T&gt;"/> instance.
        /// </value>
        public ItemsCollection<Fieldset> Fieldsets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormLayer"/> class.
        /// </summary>
        public FormLayer()
        {
            Fieldsets = new List<Fieldset>();
        }

        /// <summary>
        /// Removes all UI elements and validation errors from the layer.
        /// </summary>
        public override void Clear()
        {
            Fieldsets.Clear();
            base.Clear();
        }
    }
}

namespace iFactr.Core.Forms
{
    /// <summary>
    /// The available form layout values.
    /// </summary>
    public enum FormLayout
    {
        /// <summary>
        /// Simple in-line rendering based on relative position in the layer code.
        /// </summary>
        Simple,
        /// <summary>
        /// Rendered within a panel or equivalent.
        /// </summary>
        Panel,
    }
}
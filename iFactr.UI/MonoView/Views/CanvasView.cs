using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native view that acts as an ink canvas for drawing on.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public class CanvasView<T> : View, ICanvasView<T>
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackLink"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackLinkProperty = "BackLink";

        /// <summary>
        /// The name of the <see cref="P:OutputPane"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string OutputPaneProperty = "OutputPane";

        /// <summary>
        /// The name of the <see cref="P:PopoverPresentationStyle"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PopoverPresentationStyleProperty = "PopoverPresentationStyle";

        /// <summary>
        /// The name of the <see cref="P:StackID"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StackIDProperty = "StackID";

        /// <summary>
        /// The name of the <see cref="P:StrokeColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StrokeColorProperty = "StrokeColor";

        /// <summary>
        /// The name of the <see cref="P:StrokeThickness"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string StrokeThicknessProperty = "StrokeThickness";

        /// <summary>
        /// The name of the <see cref="P:Toolbar"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ToolbarProperty = "Toolbar";
        #endregion

        /// <summary>
        /// Occurs when the view is pushed or popped onto the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Activated
        {
            add { NativeView.Activated += value; }
            remove { NativeView.Activated -= value; }
        }

        /// <summary>
        /// Occurs when the view is pushed under or popped off of the top of a view stack.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event EventHandler Deactivated
        {
            add { NativeView.Deactivated += value; }
            remove { NativeView.Deactivated -= value; }
        }

        /// <summary>
        /// Occurs when the current drawing has been saved to disk.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        public event SaveEventHandler DrawingSaved
        {
            add { NativeView.DrawingSaved += value; }
            remove { NativeView.DrawingSaved -= value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Link"/> that describes the behavior
        /// and appearance of the back button associated with the view.
        /// </summary>
        public Link BackLink
        {
            get { return NativeView.BackLink; }
            set { NativeView.BackLink = value; }
        }

        /// <summary>
        /// Gets or sets the stack identifier for the view.
        /// Views with the same identifier will take the same place in the view stack.
        /// </summary>
        public string StackID
        {
            get { return NativeView.StackID; }
            set { NativeView.StackID = value; }
        }

        /// <summary>
        /// Gets or sets the pane on which the view will be rendered.
        /// </summary>
        public Pane OutputPane
        {
            get { return NativeView.OutputPane; }
            set { NativeView.OutputPane = value; }
        }

        /// <summary>
        /// Gets or sets the style in which the view should be presented when displayed in a popover pane.
        /// </summary>
        public PopoverPresentationStyle PopoverPresentationStyle
        {
            get { return NativeView.PopoverPresentationStyle; }
            set { NativeView.PopoverPresentationStyle = value; }
        }

        /// <summary>
        /// Gets the view stack that the view is currently on.
        /// </summary>
        public IHistoryStack Stack
        {
            get { return NativeView.Stack; }
        }

        /// <summary>
        /// Invoked when the view is being pushed under or popped off of the top of the view stack.
        /// Returning a value of <c>false</c> will cancel the navigation.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        ShouldNavigateDelegate IHistoryEntry.ShouldNavigate
        {
            get { return NativeView.ShouldNavigate; }
            set
            {
                if (value == null)
                {
                    NativeView.ShouldNavigate = ShouldNavigateFrom;
                }
                else
                {
                    NativeView.ShouldNavigate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the strokes when drawing.
        /// </summary>
        public Color StrokeColor
        {
            get { return NativeView.StrokeColor; }
            set { NativeView.StrokeColor = value; }
        }

        /// <summary>
        /// Gets or sets the thickness of the strokes when drawing.
        /// </summary>
        public double StrokeThickness
        {
            get { return NativeView.StrokeThickness; }
            set { NativeView.StrokeThickness = value; }
        }

        /// <summary>
        /// Gets or sets a toolbar of selectable items that provide support functions for the view.
        /// </summary>
        public IToolbar Toolbar
        {
            get { return NativeView.Toolbar; }
            set { NativeView.Toolbar = value; }
        }

        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        public T Model
        {
            get
            {
                try
                {
                    return (T)Pair.GetModel();
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("Could not cast to " + typeof(T).Name + ".  There is a type mismatch between the view and its native counterpart.", e);
                }
            }
            set { Pair.SetModel(value); }
        }

        /// <summary>
        /// Gets the type of the model displayed by the view.
        /// </summary>
        public override sealed Type ModelType
        {
            get { return typeof(T); }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private ICanvasView NativeView
        {
            get { return (ICanvasView)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.CanvasView&lt;T&gt;"/> class.
        /// </summary>
        public CanvasView()
        {
            Pair = MXContainer.Resolve<ICanvasView>();

            NativeView.ShouldNavigate = ShouldNavigateFrom;
            NativeView.StrokeColor = new Color();
            NativeView.PopoverPresentationStyle = PopoverPresentationStyle.Normal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.CanvasView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model containing the information that is displayed by the view.</param>
        public CanvasView(T model)
            : this()
        {
            Model = model;
        }

        /// <summary>
        /// Clears the canvas of all foreground content.
        /// </summary>
        public void Clear()
        {
            NativeView.Clear();
        }

        /// <summary>
        /// Loads the specified file into the canvas.
        /// </summary>
        /// <param name="fileName">The full path of the file to load.</param>
        public void Load(string fileName)
        {
            NativeView.Load(fileName);
        }

        /// <summary>
        /// Saves the current drawing to the temp directory with a randomly generated file name.
        /// </summary>
        /// <param name="compositeBackground">Whether to include the background as part of the saved image.</param>
        public void Save(bool compositeBackground)
        {
            NativeView.Save(compositeBackground);
        }

        /// <summary>
        /// Saves the current drawing to the specified file.
        /// </summary>
        /// <param name="fileName">The full path of the file in which to save the image.</param>
        public void Save(string fileName)
        {
            NativeView.Save(fileName);
        }

        /// <summary>
        /// Saves the current drawing to the specified file.
        /// </summary>
        /// <param name="fileName">The full path of the file in which to save the image.</param>
        /// <param name="compositeBackground">Whether to include the background as part of the saved image.</param>
        public void Save(string fileName, bool compositeBackground)
        {
            NativeView.Save(fileName, compositeBackground);
        }

        /// <summary>
        /// Gets the model for the view.
        /// </summary>
        public override sealed object GetModel()
        {
            return Pair.GetModel();
        }

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The object to set the model to.</param>
        /// <exception cref="InvalidCastException">Thrown when the <paramref name="model"/> is of an incorrect type.</exception>
        public override sealed void SetModel(object model)
        {
            Pair.SetModel(model);
        }

        /// <summary>
        /// Called when the view is being pushed under or popped off of the top of the view stack.
        /// Returning a value of <c>false</c> will cancel the navigation.
        /// </summary>
        /// <param name="link">A <see cref="Link"/> containing the destination and any other relevant information regarding the navigation taking place.</param>
        /// <param name="type">The type of navigation that was initiated.</param>
        /// <returns><c>true</c> to proceed with the navigation; otherwise, <c>false</c>.</returns>
        protected virtual bool ShouldNavigateFrom(Link link, NavigationType type)
        {
            return true;
        }
    }

    // helper class for ilayer drawing fields
    internal class CanvasController : IMXController
    {
        /// <summary>
        /// Gets the type of the model used by this controller.
        /// </summary>
        public Type ModelType
        {
            get { return model == null ? null : model.GetType(); }
        }

        private object model;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasController"/> class.
        /// </summary>
        /// <param name="canvas">The canvas view that the controller displays.</param>
        /// <param name="perspective">The perspective of the canvas view.</param>
        public CanvasController(ICanvasView canvas, string perspective)
        {
            model = canvas.GetModel();
            MXContainer.Instance.Views.Register(canvas.ModelType, canvas, perspective);
        }

        /// <summary>
        /// Gets the model for the controller.
        /// </summary>
        public object GetModel()
        {
            return model;
        }

        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        /// <returns>The view perspective from loading this instance.</returns>
        public string Load(Dictionary<string, string> parameters)
        {
            return Load(null, parameters);
        }


        /// <summary>
        /// Loads this instance with the specified parameters.
        /// </summary>
        /// <param name="uri">A <see cref="String"/> that represents the uri used to navigate to the controller.</param>
        /// <param name="parameters">A <see cref="Dictionary&lt;TKey,TValue&gt;"/> representing any parameters such as submitted values.</param>
        public string Load(string uri, Dictionary<string, string> parameters)
        {
            return ViewPerspective.Default;
        }
    }
}
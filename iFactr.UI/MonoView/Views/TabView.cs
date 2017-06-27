using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a native view that contains selectable tab items for separating an application into categories.
    /// </summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    public class TabView<T> : View, ITabView<T>
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:SelectedIndex"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectedIndexProperty = "SelectedIndex";

        /// <summary>
        /// The name of the <see cref="P:SelectionColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string SelectionColorProperty = "SelectionColor";
        #endregion

        /// <summary>
        /// Gets or sets the index of the currently selected item.
        /// </summary>
        public int SelectedIndex
        {
            get { return NativeView.SelectedIndex; }
            set { NativeView.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets or sets the color with which to overlay the selected item.
        /// </summary>
        public Color SelectionColor
        {
            get { return NativeView.SelectionColor; }
            set { NativeView.SelectionColor = value; }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="ITabItem"/>s to populate the view with.
        /// </summary>
        public IEnumerable<ITabItem> TabItems
        {
            get { return NativeView.TabItems; }
            set { NativeView.TabItems = value; }
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
        private ITabView NativeView
        {
            get { return (ITabView)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.TabView&lt;T&gt;"/> class.
        /// </summary>
        public TabView()
        {
            Pair = MXContainer.Resolve<ITabView>();
            NativeView.SelectionColor = new Color();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.TabView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model containing the information that is displayed by the view.</param>
        public TabView(T model)
            : this()
        {
            Model = model;
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
    }
}


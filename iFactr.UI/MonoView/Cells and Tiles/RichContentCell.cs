using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using iFactr.Core.Controls;
using iFactr.Core.Layers;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a cell that parses HTML or XML into rich content.  Since not all platforms support HTML rendering
    /// or XML rendering, it is advised to only use the various Append methods to create the content.
    /// </summary>
	public class RichContentCell : Cell, IRichContentCell
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:Text"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextProperty = "Text";
        #endregion

        List<PanelItem> IHtmlText.Items
        {
            get { return NativeCell.Items; }
            set { NativeCell.Items = value; }
        }

        /// <summary>
        /// Gets or sets the foreground color of the cell.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeCell.ForegroundColor; }
			set { NativeCell.ForegroundColor = value; }
		}

        /// <summary>
        /// Gets or sets a string that represents the content within the cell in the form of HTML or XML.
        /// This string will be parsed when the <see cref="Load"/> method is called to generate rich content.
        /// </summary>
		public string Text
		{
			get { return NativeCell.Text; }
			set { NativeCell.Text = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private IRichContentCell NativeCell
		{
			get { return (IRichContentCell)base.Pair; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.RichContentCell"/> class.
        /// </summary>
		public RichContentCell()
		{
			base.Pair = MXContainer.Resolve<IRichContentCell>();

            NativeCell.ForegroundColor = iApp.Instance.Style.TextColor;
		}

        /// <summary>
        /// Begins parsing the content in the <see cref="Text"/> property.
        /// This method is called once automatically when the cell is added to the view.
        /// </summary>
		public void Load()
		{
			NativeCell.Load();
		}
	}
}


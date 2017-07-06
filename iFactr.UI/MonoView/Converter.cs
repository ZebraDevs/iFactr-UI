using iFactr.Core;
using iFactr.Core.Forms;
using iFactr.Core.Layers;
using iFactr.Core.Styles;
using iFactr.UI.Controls;
using MonoCross.Navigation;
using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace iFactr.UI
{
    /// <summary>
    /// Provides methods for creating <see cref="ICell"/> instances from various <see cref="iItem"/>s and <see cref="Field"/>s.
    /// These methods can be overridden in a derived class for customized behavior.
    /// </summary>
    public class Converter
    {
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly IEnumerable<MethodInfo> _cellConversionMethods;

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Converter"/> class.
        /// </summary>
        public Converter()
        {
            _cellConversionMethods = Device.Reflector.GetMethods(GetType()).Where(m =>
            {
                if (m.Name != "ConvertToCell") return false;
                var p = m.GetParameters();
                return p.Length == 4 && p[0].ParameterType != typeof(object) && p[1].ParameterType == typeof(Style) && p[2].ParameterType == typeof(IListView) && p[3].ParameterType == typeof(ICell);
            });
        }

        /// <summary>
        /// Generates an <see cref="iFactr.UI.ICell"/> instance from the specified object.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style.  Can be <c>null</c>.</param>
        /// <param name="view">The <see cref="iFactr.UI.IListView"/> instance that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use instead of creating a new one.  Can be <c>null</c>.</param>
        /// <returns>The generated cell -or- <c>null</c> if no suitable conversion method can be found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="item"/> or <paramref name="view"/> is <c>null</c>.</exception>
        public ICell ConvertToCell(object item, Style layerStyle, IListView view, ICell cell)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            MethodInfo method, secondary = null;
            Type type = item.GetType();

            do
            {
                //Find a converter method where the first parameter matches the item type exactly
                method = _cellConversionMethods.FirstOrDefault(m => m.GetParameters()[0].ParameterType == type);

                //if there is no exact match, squirrel away the first conversion method that can be used with the item
                if (method == null && secondary == null)
                {
                    secondary = _cellConversionMethods.FirstOrDefault(m => Device.Reflector.IsAssignableFrom(m.GetParameters()[0].ParameterType, type));
                }
            }
            while (method == null && (type = Device.Reflector.GetBaseType(type)) != null);

            method = method ?? secondary;
            if (method != null && method.GetParameters()[0].ParameterType != typeof(object))
            {
                return method.Invoke(this, new[] { item, layerStyle, view, cell }) as ICell;
            }
            return null;
        }

        #region IHtmlText Cell Conversion
        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="iBlock"/>.
        /// </summary>
        /// <param name="block">The block from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the block style,
        /// or <c>null</c> if the block will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified block.</returns>
        protected virtual ICell ConvertToCell(iBlock block, Style layerStyle, IListView view, ICell cell)
        {
            IRichContentCell richCell = (cell as IRichContentCell) ?? new RichContentCell();
            richCell.Items = block.Items;
            richCell.Text = block.Text;
            richCell.MinHeight = block.FullSize ? view.Height : 0;
            richCell.MaxHeight = block.FullSize ? view.Height : double.PositiveInfinity;

            if (layerStyle != null)
            {
                richCell.BackgroundColor = layerStyle.LayerBackgroundColor;
                richCell.ForegroundColor = layerStyle.TextColor;
            }

            return richCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="iPanel"/>.
        /// </summary>
        /// <param name="panel">The block from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the panel style,
        /// or <c>null</c> if the panel will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified panel.</returns>
        protected virtual ICell ConvertToCell(iPanel panel, Style layerStyle, IListView view, ICell cell)
        {
            IRichContentCell richCell = (cell as IRichContentCell) ?? new RichContentCell();
            richCell.Items = panel.Items;
            richCell.Text = panel.Text;
            richCell.MinHeight = panel.FullSize ? view.Height : 0;
            richCell.MaxHeight = panel.FullSize ? view.Height : double.PositiveInfinity;

            if (layerStyle != null)
            {
                richCell.BackgroundColor = layerStyle.LayerBackgroundColor;
                richCell.ForegroundColor = layerStyle.TextColor;
            }

            return richCell;
        }
        #endregion

        #region iItem Cell Conversion
        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="iItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(iItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.TextLabel.Text = item.Text;

            if (item.Subtext != null && item.Subtext.Length > 7)
            {
                gridCell.SubtextLabel.Text = item.Subtext;
                gridCell.ValueLabel.Text = null;
            }
            else
            {
                gridCell.SubtextLabel.Text = null;
                gridCell.ValueLabel.Text = item.Subtext;
            }

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
                gridCell.ValueLabel.ForegroundColor = layerStyle.SecondarySubTextColor;
            }

            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="ContentItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(ContentItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.MaxHeight = gridCell.MinHeight = Cell.StandardCellHeight * 2 + Thickness.TopMargin + Thickness.BottomMargin;

            var font = Font.PreferredLabelFont;
            font.Size -= 2;
            font.Formatting = FontFormatting.Normal;

            gridCell.TextLabel.Font = font;
            gridCell.TextLabel.Lines = 3;
            gridCell.TextLabel.Text = item.Text;
            gridCell.TextLabel.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SubtextLabel.Font = Font.PreferredSmallFont - 2;
            gridCell.SubtextLabel.Lines = 5;
            gridCell.SubtextLabel.Text = item.Subtext;
            gridCell.SubtextLabel.VerticalAlignment = VerticalAlignment.Top;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;
            gridCell.Image.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="MessageItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(MessageItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell);
            if (gridCell == null)
            {
                gridCell = new ContentCell();
                gridCell.Rows.Add(Row.OneStar);
            }

            gridCell.MaxHeight = gridCell.MinHeight = Cell.StandardCellHeight * 2 + Thickness.TopMargin + Thickness.BottomMargin;

            gridCell.TextLabel.Font = Font.PreferredMessageTitleFont;
            gridCell.TextLabel.Text = item.Text;
            gridCell.TextLabel.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SubtextLabel.Font = Font.PreferredMessageBodyFont;
            gridCell.SubtextLabel.Text = item.Subtext;
            gridCell.SubtextLabel.VerticalAlignment = VerticalAlignment.Top;
            gridCell.SubtextLabel.ColumnSpan = 2;

            var messageLabel = gridCell.GetChild<Label>("1");
            if (messageLabel == null)
            {
                messageLabel = new Label();
                messageLabel.ID = "1";
                gridCell.AddChild(messageLabel);
            }

            messageLabel.Font = Font.PreferredMessageBodyFont;
            messageLabel.Text = item.MessageText;
            messageLabel.ColumnIndex = 1;
            messageLabel.ColumnSpan = 2;
            messageLabel.RowIndex = 2;

            gridCell.ValueLabel.Font = Font.PreferredMessageBodyFont;
            gridCell.ValueLabel.Text = item.DateText;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;
            gridCell.Image.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.ValueLabel.ForegroundColor = layerStyle.SecondarySubTextColor;

                messageLabel.ForegroundColor = layerStyle.SubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="MultiLineSubtextItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(MultiLineSubtextItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.MaxHeight = double.PositiveInfinity;

            gridCell.TextLabel.Text = item.Text;

            gridCell.SubtextLabel.Lines = 0;
            gridCell.SubtextLabel.Text = item.Subtext;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;
            gridCell.Image.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="RightSubtextItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(RightSubtextItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.TextLabel.Text = item.Text;
            gridCell.ValueLabel.Text = item.Subtext;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.ValueLabel.ForegroundColor = layerStyle.SecondarySubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="ShopItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(ShopItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell);
            if (gridCell == null)
            {
                gridCell = new ContentCell();
                gridCell.Rows[1] = Row.OneStar;
                gridCell.Rows.Add(Row.OneStar);
            }

            gridCell.MaxHeight = gridCell.MinHeight = Cell.StandardCellHeight * 2;

            gridCell.TextLabel.Font = Font.PreferredSmallFont;
            gridCell.TextLabel.Text = item.Toptext;

            gridCell.SubtextLabel.Font = Font.PreferredLabelFont;
            gridCell.SubtextLabel.Text = item.Text;

            var sublabel = gridCell.GetChild<Label>("1");
            if (sublabel == null)
            {
                sublabel = new Label();
                sublabel.ID = "1";
                gridCell.AddChild(sublabel);
            }

            sublabel.Font = Font.PreferredSmallFont;
            sublabel.Lines = 1;
            sublabel.Text = item.Subtext;
            sublabel.ColumnIndex = 1;
            sublabel.RowIndex = 2;

            //IImage starImage = gridCell.GetChild<IImage>("stars");
            //if (item.StarRating >= 0)
            //{
            //    if (starImage == null)
            //    {
            //        starImage = MXContainer.Resolve<IImage>();
            //        gridCell.AddChild(starImage);
            //    }
            //    starImage.ID = "stars";
            //    starImage.Stretch = ContentStretch.None;
            //    starImage.VerticalAlignment = VerticalAlignment.Center;
            //    starImage.ColumnIndex = 2;
            //    starImage.RowIndex = 2;
            //}
            //else if (starImage != null)
            //{
            //    gridCell.RemoveChild(starImage);
            //}

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.SubTextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.TextColor;
                sublabel.ForegroundColor = layerStyle.SubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="SubtextBelowAndBesideItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(SubtextBelowAndBesideItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.TextLabel.Text = item.Text;
            gridCell.SubtextLabel.Text = item.Subtext;
            gridCell.ValueLabel.Text = item.BesideText;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
                gridCell.ValueLabel.ForegroundColor = layerStyle.SecondarySubTextColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="SubtextItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(SubtextItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            gridCell.TextLabel.Text = item.Text;
            gridCell.SubtextLabel.Text = item.Subtext;
            gridCell.SubtextLabel.Lines = 1;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
            }

            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="VariableContentItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        protected virtual ICell ConvertToCell(VariableContentItem item, Style layerStyle, IListView view, ICell cell)
        {
            var gridCell = (cell as ContentCell ?? (cell == null ? null : cell.Pair as ContentCell)) ?? new ContentCell();

            var padding = gridCell.Padding;
            padding.Top = padding.Bottom = Thickness.LargeVerticalSpacing;
            gridCell.Padding = padding;

            gridCell.MaxHeight = double.PositiveInfinity;

            var font = Font.PreferredLabelFont;
            font.Formatting = FontFormatting.Normal;

            gridCell.TextLabel.Font = font;
            gridCell.TextLabel.Lines = 0;
            gridCell.TextLabel.Text = item.Text;
            gridCell.TextLabel.VerticalAlignment = VerticalAlignment.Top;

            gridCell.SubtextLabel.Lines = 0;
            gridCell.SubtextLabel.Text = item.Subtext;

            gridCell.Image.FilePath = item.Icon == null ? null : item.Icon.Location;
            gridCell.Image.Stretch = ContentStretch.None;
            gridCell.Image.VerticalAlignment = VerticalAlignment.Top;
            gridCell.MinHeight = Math.Max(gridCell.Image.Dimensions.Height, Cell.StandardCellHeight);

            gridCell.SelectionStyle = item.Link == null || item.Link.Address == null ? SelectionStyle.None : SelectionStyle.Default;
            gridCell.NavigationLink = item.Link;
            gridCell.AccessoryLink = item.Button;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                gridCell.SelectionColor = layerStyle.SelectionColor;
                gridCell.TextLabel.ForegroundColor = layerStyle.TextColor;
                gridCell.SubtextLabel.ForegroundColor = layerStyle.SubTextColor;
            }
            return gridCell;
        }
        #endregion

        #region Field Cell Conversion
        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="BoolField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(BoolField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new Switch());
                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var control = c.GetChild<Switch>();
                        if (control != null)
                        {
                            control.Value = !control.Value;
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var boolSwitch = headeredCell.GetChild<Switch>();
            boolSwitch.SubmitKey = field.ID;
            boolSwitch.ClearBinding(Switch.ValueProperty);
            boolSwitch.SetBinding(new Binding(Switch.ValueProperty, "Value") { Source = field, Mode = BindingMode.TwoWay });

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                boolSwitch.ForegroundColor = layerStyle.TextColor;
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="ButtonField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(ButtonField field, Style layerStyle, IListView view, ICell cell)
        {
            IGridCell gridCell = cell as IGridCell;
            if (gridCell == null)
            {
                gridCell = new GridCell();
            }

            var button = gridCell.GetChild<Button>();
            if (button == null)
            {
                button = new Button();
                gridCell.AddChild(button);
            }

            gridCell.MaxHeight = Cell.StandardCellHeight;
            gridCell.SetValue(Cell.MaxHeightProperty, _tallItemHeight, MobileTarget.Android | MobileTarget.Compact);
            gridCell.SelectionStyle = SelectionStyle.None;

            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Title = field.Label;
            button.NavigationLink = field.Link;
            button.SubmitKey = field.ID;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                button.ForegroundColor = layerStyle.TextColor;
            }
            return gridCell;
        }
        private readonly double _tallItemHeight = 40 + Thickness.TopMargin + Thickness.BottomMargin;

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="DateField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(DateField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label,
                    field.Type == DateField.DateType.Time ? null : new DatePicker(),
                    field.Type == DateField.DateType.Date ? null : new TimePicker());

                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c == null) return;
                    var date = c.GetChild<DatePicker>();
                    if (date != null)
                    {
                        date.ShowPicker();
                    }
                    else
                    {
                        var time = c.GetChild<TimePicker>();
                        if (time != null)
                        {
                            time.ShowPicker();
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var datePicker = headeredCell.GetChild<DatePicker>();
            if (field.Type != DateField.DateType.Time)
            {
                if (datePicker == null)
                {
                    datePicker = new DatePicker();
                    headeredCell.AddControl(datePicker);
                }

                datePicker.NullifyEvents();
                datePicker.SubmitKey = field.ID;
                datePicker.Date = field.Value;

                datePicker.DateChanged += (o, e) =>
                {
                    if (e.NewValue.HasValue)
                    {
                        field.Value = new DateTime(e.NewValue.Value.Date.Ticks + (field.Value.HasValue ? field.Value.Value.TimeOfDay.Ticks : 0));
                    }
                    else
                    {
                        field.Value = null;
                    }
                };
            }
            else if (datePicker != null)
            {
                datePicker.NullifyEvents();
                headeredCell.RemoveControl(datePicker);
            }

            var timePicker = headeredCell.GetChild<TimePicker>();
            if (field.Type != DateField.DateType.Date)
            {
                if (timePicker == null)
                {
                    timePicker = new TimePicker();
                    headeredCell.AddControl(timePicker);
                }

                timePicker.NullifyEvents();
                timePicker.SubmitKey = field.Type == DateField.DateType.DateTime ? field.ID + ".time" : field.ID;
                timePicker.Time = field.Value;

                timePicker.TimeChanged += (o, e) =>
                {
                    if (e.NewValue.HasValue)
                    {
                        field.Value = new DateTime((field.Value.HasValue ? field.Value.Value.Date.Ticks : 0) + e.NewValue.Value.TimeOfDay.Ticks);
                    }
                    else
                    {
                        field.Value = null;
                    }
                };
            }
            else if (timePicker != null)
            {
                timePicker.NullifyEvents();
                headeredCell.RemoveControl(timePicker);
            }

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;

                if (datePicker != null)
                {
                    datePicker.SetValue(DatePicker.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
                }
                if (timePicker != null)
                {
                    timePicker.SetValue(TimePicker.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
                }
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="DrawingField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(DrawingField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new Image());
                headeredCell.Selected += (sender, e) =>
                {
                    var canvas = new CanvasView<DrawingField>(field);
                    canvas.OutputPane = Pane.Popover;
                    canvas.StrokeColor = field.StrokeColor;
                    canvas.Title = field.Label;

                    if (!string.IsNullOrEmpty(field.BackgroundImage) && iApp.File.Exists(field.BackgroundImage))
                    {
                        canvas.SetBackground(field.BackgroundImage, ContentStretch.None);
                    }
                    else
                    {
                        canvas.SetBackground(layerStyle.LayerBackgroundColor);
                    }

                    if (!string.IsNullOrEmpty(field.DrawnImageId))
                    {
                        canvas.Load(field.DrawnImageId);
                    }

                    var weak = new WeakReference(canvas);
                    var toolbar = new Toolbar();

                    var save = new ToolbarButton();
                    save.Title = iApp.Factory.GetResourceString("Done");
                    save.Clicked += (o, args) =>
                    {
                        field = (DrawingField)canvas.GetModel();
                        var cv = weak.Target as CanvasView<DrawingField>;
                        if (cv != null)
                        {
                            cv.Save(field.CompositeResult);
                        }
                    };

                    var cancel = new ToolbarButton();
                    cancel.Title = iApp.Factory.GetResourceString("Cancel");
                    cancel.Clicked += (o, args) =>
                    {
                        var cv = weak.Target as CanvasView<DrawingField>;
                        if (cv != null)
                        {
                            cv.Stack.PopView();
                            cv.Clear();
                        }
                    };

                    var clear = new ToolbarButton();
                    clear.Title = iApp.Factory.GetResourceString("Clear");
                    clear.Clicked += (o, args) =>
                    {
                        var alert = new Alert(iApp.Factory.GetResourceString("ClearConfirm"), iApp.Factory.GetResourceString("ClearConfirmTitle"), AlertButtons.OKCancel);
                        alert.Dismissed += (o2, e2) =>
                        {
                            if (e2.Result == AlertResult.OK)
                            {
                                var cv = weak.Target as CanvasView<DrawingField>;
                                if (cv != null)
                                {
                                    cv.Clear();
                                }
                            }
                        };
                        alert.Show();
                    };

                    toolbar.PrimaryItems = new[] { save };
                    toolbar.SecondaryItems = new[] { cancel, clear };

                    canvas.Toolbar = toolbar;

                    canvas.DrawingSaved += (o, args) =>
                    {
                        var di = headeredCell.GetChild<Image>();
                        if (di != null)
                        {
                            di.FilePath = args.FilePath;
                            if (di.Dimensions.IsEmpty)
                            {
                                di.FilePath = null;
                            }
                        }

                        var cv = weak.Target as CanvasView<DrawingField>;
                        if (cv != null)
                        {
                            cv.Stack.PopView();
                            cv.Clear();
                        }
                    };

                    iApp.Factory.OutputController(new CanvasController(canvas, ViewPerspective.Default), ViewPerspective.Default, MXContainer.Instance.LastNavigationUrl);
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.MaxHeight = headeredCell.MinHeight = Cell.StandardCellHeight * 2;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var image = headeredCell.GetChild<Image>();
            image.Stretch = ContentStretch.Uniform;
            image.SubmitKey = field.ID;
            image.ClearBinding(Image.FilePathProperty);
            image.SetBinding(new Binding(Image.FilePathProperty, "DrawnImageId") { Source = field, Mode = BindingMode.TwoWay });

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="EmailField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(EmailField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new TextBox());
                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var control = c.GetChild<TextBox>();
                        if (control != null)
                        {
                            control.Focus();
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.Metadata["Field"] = field;
            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var textBox = headeredCell.GetChild<TextBox>();
            textBox.Expression = field.Expression;
            textBox.KeyboardType = (KeyboardType)field.KeyboardType;
            textBox.Placeholder = field.Placeholder;
            textBox.SubmitKey = field.ID;
            textBox.ClearBinding(TextBox.TextProperty);
            textBox.SetBinding(new Binding(TextBox.TextProperty, "Text") { Source = field, Mode = BindingMode.TwoWay });
            textBox.NullifyEvents();

            var layer = view.GetModel() as iLayer;
            var weak = new WeakReference(view);
            Field nextField = null;
            textBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
            if (textBox.KeyboardReturnType == KeyboardReturnType.Next)
            {
                textBox.ReturnKeyPressed += (o, e) =>
                {
                    e.IsHandled = SetNextFocus(weak.Target as IListView, nextField);
                };
            }
            else if (textBox.KeyboardReturnType == KeyboardReturnType.Go)
            {
                textBox.ReturnKeyPressed += (o, e) =>
                {
                    e.IsHandled = true;
                    iApp.Navigate(layer.ActionButtons.FirstOrDefault(), weak.Target as IListView);
                };
            }

            if (view.Metadata["NextField"] == field)
            {
                textBox.Focus();
                view.Metadata.Remove("NextField");
            }

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                textBox.SetValue(TextBox.ForegroundColorProperty, layerStyle.TextColor, MobileTarget.Android | MobileTarget.Compact);
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="LabelField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(LabelField field, Style layerStyle, IListView view, ICell cell)
        {
            IGridCell gridCell = (cell as IGridCell) ?? new GridCell();
            if (gridCell.Columns.Count == 0)
            {
                gridCell.Columns.Add(Column.OneStar);
                gridCell.Columns.Add(Column.AutoSized);
            }

            gridCell.MinHeight = Cell.StandardCellHeight;
            gridCell.SelectionStyle = SelectionStyle.None;

            var label = gridCell.GetChild<Label>("1");
            if (label == null)
            {
                label = new Label();
                label.ID = "1";
                gridCell.AddChild(label);
            }

            label.Font = Font.PreferredLabelFont;
            label.Lines = 0;
            label.Text = field.Label;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.ColumnIndex = 0;
            label.RowIndex = 0;

            var sublabel = gridCell.GetChild<Label>("2");
            if (!string.IsNullOrEmpty(field.Text))
            {
                if (sublabel == null)
                {
                    sublabel = new Label();
                    sublabel.ID = "2";
                    gridCell.AddChild(sublabel);
                }

                sublabel.SubmitKey = field.ID;
                sublabel.Font = Font.PreferredSmallFont;
                sublabel.Lines = 1;
                sublabel.Text = field.Text;
                sublabel.VerticalAlignment = VerticalAlignment.Center;
                sublabel.Margin = new Thickness(Thickness.SmallHorizontalSpacing, 0, 0, 0);
                sublabel.ColumnIndex = 1;
                sublabel.RowIndex = 0;

                label.SubmitKey = null;
            }
            else
            {
                label.SubmitKey = field.ID;

                if (sublabel != null)
                {
                    gridCell.RemoveChild(sublabel);
                }
            }

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                label.ForegroundColor = layerStyle.TextColor;

                if (sublabel != null)
                {
                    sublabel.ForegroundColor = layerStyle.SubTextColor;
                }
            }

            if (!field.IsValid)
            {
                var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                label.ForegroundColor = errorColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="MultiLineTextField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(MultiLineTextField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new TextArea());
                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var control = c.GetChild<TextArea>();
                        if (control != null)
                        {
                            control.Focus();
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.Metadata["Field"] = field;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var textArea = headeredCell.GetChild<ITextArea>();
            textArea.Expression = field.Expression;
            textArea.KeyboardType = (KeyboardType)field.KeyboardType;
            textArea.TextCompletion = field.TextCompletion;
            textArea.Placeholder = field.Placeholder;
            textArea.SubmitKey = field.ID;
            textArea.ClearBinding(TextArea.TextProperty);
            textArea.SetBinding(new Binding(TextArea.TextProperty, "Text") { Source = field, Mode = BindingMode.TwoWay });

            if (iApp.Factory.Target == MobileTarget.Android)
            {
                textArea.ClearBinding("MinLines");
                textArea.SetBinding(new Binding("MinLines", "Rows") { Source = field, Mode = BindingMode.TwoWay });
            }
            else
            {
                headeredCell.MinHeight = headeredCell.MaxHeight = Thickness.SmallVerticalSpacing + Thickness.TopMargin + Thickness.BottomMargin +
                    Font.PreferredLabelFont.GetLineHeight() + Font.PreferredTextBoxFont.GetLineHeight() * field.Rows;
            }

            if (view.Metadata["NextField"] == field)
            {
                textArea.Focus();
                view.Metadata.Remove("NextField");
            }

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                textArea.ForegroundColor = layerStyle.TextColor;
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
                headeredCell.MaxHeight += errorLabel.Measure(new Size(view.Width, double.PositiveInfinity)).Height + Thickness.SmallVerticalSpacing;
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="NavigationField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(NavigationField field, Style layerStyle, IListView view, ICell cell)
        {
            IGridCell gridCell = (cell as IGridCell) ?? new GridCell();
            gridCell.MaxHeight = gridCell.MinHeight = Cell.StandardCellHeight;

            ILabel label = gridCell.GetChild<Label>();
            if (label == null)
            {
                label = new Label();
                gridCell.AddChild(label);
            }

            label.SubmitKey = field.ID;
            label.Font = Font.PreferredLabelFont;
            label.Lines = 1;
            label.Text = field.Label;
            label.VerticalAlignment = VerticalAlignment.Center;

            gridCell.NavigationLink = field.Link;

            if (layerStyle != null)
            {
                gridCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                label.ForegroundColor = layerStyle.TextColor;
            }

            if (!field.IsValid)
            {
                var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                label.ForegroundColor = errorColor;
            }
            return gridCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="NumericField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(NumericField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new TextBox());
                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var control = c.GetChild<TextBox>();
                        if (control != null)
                        {
                            control.Focus();
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.Metadata["Field"] = field;
            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var textBox = headeredCell.GetChild<ITextBox>();
            textBox.Expression = field.Expression;
            textBox.KeyboardType = (KeyboardType)field.KeyboardType;
            textBox.TextCompletion = field.TextCompletion;
            textBox.Placeholder = field.Placeholder;
            textBox.SubmitKey = field.ID;
            textBox.ClearBinding(TextBox.TextProperty);
            textBox.SetBinding(new Binding(TextBox.TextProperty, "Text") { Source = field, Mode = BindingMode.TwoWay });
            textBox.NullifyEvents();

            var layer = view.GetModel() as iLayer;
            var weak = new WeakReference(view);
            Field nextField = null;
            textBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
            if (textBox.KeyboardReturnType == KeyboardReturnType.Next)
            {
                textBox.ReturnKeyPressed += (o, e) =>
                {
                    e.IsHandled = SetNextFocus(weak.Target as IListView, nextField);
                };
            }
            else if (textBox.KeyboardReturnType == KeyboardReturnType.Go)
            {
                textBox.ReturnKeyPressed += (o, e) =>
                {
                    e.IsHandled = true;
                    iApp.Navigate(layer.ActionButtons.FirstOrDefault(), weak.Target as IListView);
                };
            }

            if (view.Metadata["NextField"] == field)
            {
                textBox.Focus();
                view.Metadata.Remove("NextField");
            }

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                textBox.SetValue(TextBox.ForegroundColorProperty, layerStyle.TextColor, MobileTarget.Android | MobileTarget.Compact);
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="SelectListField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(SelectListField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new SelectList());
                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var control = c.GetChild<SelectList>();
                        if (control != null)
                        {
                            control.ShowList();
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;
            headeredCell.SetValue(HeaderedControlCell.SelectionStyleProperty, SelectionStyle.IndicatorOnly, MobileTarget.Touch);

            var selectList = headeredCell.GetChild<SelectList>();
            selectList.SubmitKey = field.ID;

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                selectList.SetValue(SelectList.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }

            selectList.ClearBinding(SelectList.SelectedItemProperty);

            selectList.Items = field.Items;
            selectList.SetBinding(new Binding(SelectList.SelectedItemProperty, "SelectedItem") { Source = field, Mode = BindingMode.TwoWay });

            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="SliderField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(SliderField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, new Slider());
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var slider = headeredCell.GetChild<Slider>();
            slider.MaxValue = field.Max;
            slider.MinValue = field.Min;
            slider.SubmitKey = field.ID;
            slider.ClearBinding(Slider.ValueProperty);
            slider.SetBinding(new Binding(Slider.ValueProperty, "Value") { Source = field, Mode = BindingMode.TwoWay });

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="TextField"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        protected virtual ICell ConvertToCell(TextField field, Style layerStyle, IListView view, ICell cell)
        {
            var headeredCell = cell as HeaderedControlCell;
            if (headeredCell == null)
            {
                headeredCell = new HeaderedControlCell(field.Label, field.IsPassword ? (IControl)new PasswordBox() : new TextBox());

                headeredCell.Selected += (o, e) =>
                {
                    var c = o as HeaderedControlCell;
                    if (c != null)
                    {
                        var t = c.GetChild<TextBox>();
                        if (t != null)
                        {
                            t.Focus();
                        }
                        else
                        {
                            var p = c.GetChild<PasswordBox>();
                            if (p != null)
                            {
                                p.Focus();
                            }
                        }
                    }
                };
            }
            else
            {
                headeredCell.Header.Text = field.Label;
            }

            headeredCell.Metadata["Field"] = field;
            headeredCell.MinHeight = Cell.StandardCellHeight;
            headeredCell.SelectionStyle = SelectionStyle.None;

            var textBox = headeredCell.GetChild<TextBox>();
            if (textBox != null)
            {
                textBox.Expression = field.Expression;
                textBox.KeyboardType = (KeyboardType)field.KeyboardType;
                textBox.TextCompletion = field.TextCompletion;
                textBox.Placeholder = field.Placeholder;
                textBox.SubmitKey = field.ID;
                textBox.ClearBinding(TextBox.TextProperty);
                textBox.SetBinding(new Binding(TextBox.TextProperty, "Text") { Source = field, Mode = BindingMode.TwoWay });
                textBox.NullifyEvents();

                var layer = view.GetModel() as iLayer;
                var weak = new WeakReference(view);
                Field nextField = null;
                textBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
                if (textBox.KeyboardReturnType == KeyboardReturnType.Next)
                {
                    textBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = SetNextFocus(weak.Target as IListView, nextField);
                    };
                }
                else if (textBox.KeyboardReturnType == KeyboardReturnType.Go)
                {
                    textBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = true;
                        iApp.Navigate(layer.ActionButtons.FirstOrDefault(b => b.Action == Core.Controls.Button.ActionType.Submit), weak.Target as IListView);
                    };
                }

                if (view.Metadata["NextField"] == field)
                {
                    textBox.Focus();
                    view.Metadata.Remove("NextField");
                }
            }
            else
            {
                var passwordBox = headeredCell.GetChild<PasswordBox>();
                if (passwordBox != null)
                {
                    passwordBox.Expression = field.Expression;
                    passwordBox.KeyboardType = (KeyboardType)field.KeyboardType;
                    passwordBox.Placeholder = field.Placeholder;
                    passwordBox.SubmitKey = field.ID;
                    passwordBox.ClearBinding(PasswordBox.PasswordProperty);
                    passwordBox.SetBinding(new Binding(PasswordBox.PasswordProperty, "Text") { Source = field, Mode = BindingMode.TwoWay });
                    passwordBox.NullifyEvents();

                    var layer = view.GetModel() as iLayer;
                    var weak = new WeakReference(view);
                    Field nextField = null;
                    passwordBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
                    if (passwordBox.KeyboardReturnType == KeyboardReturnType.Next)
                    {
                        passwordBox.ReturnKeyPressed += (o, e) =>
                        {
                            e.IsHandled = SetNextFocus(weak.Target as IListView, nextField);
                        };
                    }
                    else if (passwordBox.KeyboardReturnType == KeyboardReturnType.Go)
                    {
                        passwordBox.ReturnKeyPressed += (o, e) =>
                        {
                            e.IsHandled = true;
                            iApp.Navigate(layer.ActionButtons.FirstOrDefault(), weak.Target as IListView);
                        };
                    }

                    if (view.Metadata["NextField"] == field)
                    {
                        passwordBox.Focus();
                        view.Metadata.Remove("NextField");
                    }
                }
            }

            if (layerStyle != null)
            {
                headeredCell.BackgroundColor = layerStyle.LayerItemBackgroundColor;
                headeredCell.Header.ForegroundColor = layerStyle.TextColor;
                if (textBox != null)
                {
                    textBox.ForegroundColor = layerStyle.TextColor;
                }

                var passwordBox = headeredCell.GetChild<PasswordBox>();
                if (passwordBox != null)
                {
                    passwordBox.ForegroundColor = layerStyle.TextColor;
                }
            }

            var errorLabel = headeredCell.GetChild<Label>("error");
            if (field.IsValid && errorLabel != null)
            {
                headeredCell.RemoveControl(errorLabel);
            }
            else if (!field.IsValid)
            {
                if (errorLabel == null)
                {
                    var errorColor = layerStyle == null || layerStyle.ErrorTextColor.IsDefaultColor ? Color.Red : layerStyle.ErrorTextColor;
                    errorLabel = new Label();
                    errorLabel.ForegroundColor = errorColor;
                    errorLabel.Font = Font.PreferredSmallFont;
                    errorLabel.ID = "error";
                    headeredCell.AddControl(errorLabel);
                }

                errorLabel.Text = string.Join("\n", field.BrokenRules.ToArray());
            }
            return headeredCell;
        }

        /// <summary>
        /// Called when an <see cref="iFactr.UI.ICell"/> instance is needed for the specified <see cref="AggregateFieldset"/>s.
        /// </summary>
        /// <param name="fieldsets">The fieldsets from which to generate the cell.</param>
        /// <param name="layerStyle">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the fieldsets' style,
        /// or <c>null</c> if the fieldsets will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="cell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified fieldsets.</returns>
        protected virtual ICell ConvertToCell(IEnumerable<AggregateFieldset> fieldsets, Style layerStyle, IListView view, ICell cell)
        {
            var array = fieldsets.ToArray();
            int rowCount = array.Max(a => a.Count);
            int columnCount = array.Length;

            var tableCell = (cell as TableCell) ?? new TableCell(rowCount, columnCount);
            tableCell.TableHeader.Text = null;

            if (tableCell.RowCount > rowCount)
            {
                tableCell.RemoveRows(tableCell.RowCount - rowCount);
            }
            else if (tableCell.RowCount < rowCount)
            {
                tableCell.AddRows(rowCount - tableCell.RowCount);
            }

            if (tableCell.ColumnCount > columnCount)
            {
                tableCell.RemoveColumns(tableCell.ColumnCount - columnCount);
            }
            else if (tableCell.ColumnCount < columnCount)
            {
                tableCell.AddColumns(columnCount - tableCell.ColumnCount);
            }

            var typeSwitch = new TypeSwitch();
            for (int i = 0; i < array.Length; i++)
            {
                var fieldset = array[i];
                if (i == 0)
                {
                    for (int j = 0; j < tableCell.RowHeaders.Count; j++)
                    {
                        tableCell.RowHeaders[j].Text = fieldset.Count > j ? fieldset[j].Label : null;
                    }
                }

                tableCell.ColumnHeaders[i].Text = fieldset.Header;

                var controls = tableCell.GetControlsForColumn(i);
                if (controls.Length < fieldset.Count)
                {
                    Array.Resize(ref controls, fieldset.Count);
                }

                for (int j = 0; j < fieldset.Count; j++)
                {
                    controls[j] = ConvertToControl(fieldset[j], view, typeSwitch, controls[j]);
                }
                tableCell.SetControlsForColumn(i, controls);
            }

            return tableCell;
        }

        /// <summary>
        /// Converts a Field to a Control.
        /// </summary>
        /// <param name="field">The <see cref="Field"/> to convert.</param>
        /// <param name="view"></param>
        /// <param name="typeSwitch">A <see cref="TypeSwitch"/> to be used with multiple calls to this method; otherwise <c>null</c> to automatically initialize a switch.</param>
        /// <param name="recycledControl">An <see cref="IControl"/> to recycle or initialize the returned object; otherwise <c>null</c> to automatically initializze the control.</param>
        /// <returns>An <see cref="IControl"/> generated from the given field.</returns>
        public IControl ConvertToControl(Field field, IMXView view, TypeSwitch typeSwitch, IControl recycledControl)
        {
            if (typeSwitch == null) typeSwitch = new TypeSwitch();
            iLayer layer = null;
            Style layerStyle = null;
            if (view != null)
            {
                layer = view.GetModel() as iLayer;
                if (layer != null) layerStyle = layer.LayerStyle;
            }

            var weak = new WeakReference(view);
            typeSwitch.Object = field;
            typeSwitch.Case<TextField>(f => f.IsPassword, f =>
            {
                var passwordBox = recycledControl as PasswordBox ?? new PasswordBox();
                passwordBox.Expression = f.Expression;
                passwordBox.KeyboardType = (KeyboardType)f.KeyboardType;
                passwordBox.SubmitKey = f.ID;
                passwordBox.ClearBinding(PasswordBox.PasswordProperty);
                passwordBox.SetBinding(new Binding(PasswordBox.PasswordProperty, "Text") { Source = f, Mode = BindingMode.TwoWay });
                Field nextField;
                passwordBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
                if (passwordBox.KeyboardReturnType == KeyboardReturnType.Next)
                {
                    passwordBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = SetNextFocus(o, weak.Target as IMXView, nextField);
                    };
                }
                else if (passwordBox.KeyboardReturnType == KeyboardReturnType.Go && layer != null)
                {
                    passwordBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = true;
                        iApp.Navigate(layer.ActionButtons.FirstOrDefault(), weak.Target as IMXView);
                    };
                }
                recycledControl = passwordBox;
            })
            .Case<TextField>(f =>
            {
                var textBox = recycledControl as TextBox ?? new TextBox();
                textBox.Expression = f.Expression;
                textBox.KeyboardType = (KeyboardType)f.KeyboardType;
                textBox.Placeholder = f.Placeholder;
                textBox.SubmitKey = f.ID;
                textBox.ClearBinding(TextBox.TextProperty);
                textBox.SetBinding(new Binding(TextBox.TextProperty, "Text") { Source = f, Mode = BindingMode.TwoWay });
                Field nextField;
                textBox.KeyboardReturnType = GetKeyboardReturnType(field, layer, out nextField);
                if (textBox.KeyboardReturnType == KeyboardReturnType.Next)
                {
                    textBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = SetNextFocus(o, weak.Target as IMXView, nextField);
                    };
                }
                else if (textBox.KeyboardReturnType == KeyboardReturnType.Go && layer != null)
                {
                    textBox.ReturnKeyPressed += (o, e) =>
                    {
                        e.IsHandled = true;
                        iApp.Navigate(layer.ActionButtons.FirstOrDefault(), weak.Target as IMXView);
                    };
                }
                recycledControl = textBox;
            })
            .Case<SelectListField>(f =>
            {
                var selectList = recycledControl as SelectList ?? new SelectList();
                selectList.HorizontalAlignment = HorizontalAlignment.Stretch;
                selectList.Items = f.Items;
                selectList.SubmitKey = f.ID;
                selectList.ClearBinding(SelectList.SelectedItemProperty);
                selectList.SetBinding(new Binding(SelectList.SelectedItemProperty, "SelectedItem") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = selectList;

                if (layerStyle != null)
                {
                    selectList.SetValue(SelectList.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
                }
            })
            .Case<BoolField>(f =>
            {
                var boolSwitch = recycledControl as Switch ?? new Switch();
                boolSwitch.HorizontalAlignment = HorizontalAlignment.Left;
                boolSwitch.VerticalAlignment = VerticalAlignment.Center;
                boolSwitch.SubmitKey = f.ID;
                boolSwitch.ClearBinding(Switch.ValueProperty);
                boolSwitch.SetBinding(new Binding(Switch.ValueProperty, "Value") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = boolSwitch;

                if (layerStyle != null)
                {
                    boolSwitch.ForegroundColor = layerStyle.TextColor;
                }
            })
            .Case<DateField>(f => f.Type == DateField.DateType.Time, f =>
            {
                var timePicker = recycledControl as TimePicker ?? new TimePicker();
                timePicker.HorizontalAlignment = HorizontalAlignment.Stretch;
                timePicker.SubmitKey = f.ID;
                timePicker.ClearBinding(TimePicker.TimeProperty);
                timePicker.SetBinding(new Binding(TimePicker.TimeProperty, "Value") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = timePicker;

                if (layerStyle != null)
                {
                    timePicker.SetValue(TimePicker.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
                }
            })
            .Case<DateField>(f =>
            {
                var datePicker = recycledControl as DatePicker ?? new DatePicker();
                datePicker.HorizontalAlignment = HorizontalAlignment.Stretch;
                datePicker.SubmitKey = f.ID;
                datePicker.ClearBinding(DatePicker.DateProperty);
                datePicker.SetBinding(new Binding(DatePicker.DateProperty, "Value") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = datePicker;

                if (layerStyle != null)
                {
                    datePicker.SetValue(DatePicker.ForegroundColorProperty, layerStyle.SubTextColor, MobileTarget.Touch | MobileTarget.Compact | MobileTarget.Android);
                }
            })
            .Case<MultiLineTextField>(f =>
            {
                var textArea = recycledControl as TextArea ?? new TextArea();
                textArea.Expression = f.Expression;
                textArea.KeyboardType = (KeyboardType)f.KeyboardType;
                textArea.SubmitKey = f.ID;
                textArea.ClearBinding(TextArea.TextProperty);
                textArea.SetBinding(new Binding(TextArea.TextProperty, "Text") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = textArea;
            })
            .Case<ButtonField>(f =>
            {
                var button = recycledControl as Button ?? new Button();
                button.Font = Font.PreferredLabelFont;
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.Title = f.Label;
                button.NavigationLink = f.Link;
                button.SubmitKey = f.ID;
                recycledControl = button;

                if (layerStyle != null)
                {
                    button.ForegroundColor = layerStyle.TextColor;
                }
            })
            .Case<LabelField>(f =>
            {
                var label = recycledControl as Label ?? new Label();
                label.Font = Font.PreferredLabelFont;
                label.Lines = 0;
                label.Text = f.Text;
                recycledControl = label;

                if (layerStyle != null)
                {
                    label.ForegroundColor = layerStyle.TextColor;
                }
            })
            .Case<SliderField>(f =>
            {
                var slider = recycledControl as Slider ?? new Slider();
                slider.MaxValue = 100;
                slider.MinValue = 0;
                slider.SubmitKey = f.ID;
                slider.ClearBinding(Slider.ValueProperty);
                slider.SetBinding(new Binding(Slider.ValueProperty, "Value") { Source = f, Mode = BindingMode.TwoWay });
                recycledControl = slider;
            });
            return recycledControl;
        }

        private KeyboardReturnType GetKeyboardReturnType(Field field, iLayer layer, out Field nextField)
        {
            nextField = null;
            if (layer == null)
            {
                return KeyboardReturnType.Default;
            }
            var fieldsets = layer.Items.OfType<Fieldset>().ToArray();
            var fieldset = fieldsets.LastOrDefault(fs => fs.Contains(field));
            if (fieldset == null)
            {
                return KeyboardReturnType.Default;
            }

            if (layer.Items.SkipWhile(i => i != fieldset).All(fs => fs is Fieldset) &&
                fieldsets.SelectMany(fs => fs).LastOrDefault(f => !(f is ButtonField) && !(f is NavigationField)) == field &&
                layer.ActionButtons != null && layer.ActionButtons.Any(b => b.Action == Core.Controls.Button.ActionType.Submit))
            {
                return KeyboardReturnType.Go;
            }

            int fieldIndex = fieldset.IndexOf(field) + 1;
            nextField = fieldset.Count > fieldIndex ? fieldset[fieldIndex] : null;
            if (nextField is TextField)
            {
                return KeyboardReturnType.Next;
            }

            return KeyboardReturnType.Default;
        }

        private bool SetNextFocus(object sender, IMXView view, Field nextField)
        {
            var list = view as IListView;
            if (list != null) return SetNextFocus(list, nextField);

            var grid = view as IGridView;
            return grid != null && SetNextFocus(sender, grid, nextField);
        }

        private bool SetNextFocus(IListView view, Field nextField)
        {
            if (nextField == null)
            {
                return false;
            }

            var nextCell = view.GetVisibleCells().FirstOrDefault(c => c.Metadata != null && c.Metadata["Field"] == nextField) as IElementHost;
            if (nextCell != null)
            {
                var tb = nextCell.GetChild<TextBox>();
                if (tb != null)
                {
                    tb.Focus();
                    return true;
                }

                var pb = nextCell.GetChild<PasswordBox>();
                if (pb != null)
                {
                    pb.Focus();
                    return true;
                }

                var ta = nextCell.GetChild<TextArea>();
                if (ta != null)
                {
                    ta.Focus();
                    return true;
                }
            }

            var layer = view.GetModel() as iLayer;
            if (layer == null)
            {
                return false;
            }

            view.Metadata["NextField"] = nextField;
            var fieldset = layer.Items.OfType<Fieldset>().LastOrDefault(fs => fs.Contains(nextField));
            view.ScrollToCell(layer.Items.IndexOf(fieldset), fieldset.IndexOf(nextField), true);

            return false;
        }

        private bool SetNextFocus(object sender, IGridView view, Field nextField)
        {
            if (nextField == null)
            {
                return false;
            }

            int index = view.Children.IndexOf(element => element.Equals(sender));
            var nextControl = index > -1 ? view.Children.Skip(index + 1).FirstOrDefault(c => c is TextBox || c is TextArea || c is PasswordBox) : null;
            if (nextControl != null)
            {
                var tb = nextControl as TextBox;
                if (tb != null)
                {
                    tb.Focus();
                    return true;
                }

                var pb = nextControl as PasswordBox;
                if (pb != null)
                {
                    pb.Focus();
                    return true;
                }

                var ta = nextControl as TextArea;
                if (ta != null)
                {
                    ta.Focus();
                    return true;
                }
            }

            var layer = view.GetModel() as iLayer;
            if (layer == null)
            {
                return false;
            }

            view.Metadata["NextField"] = nextField;
            //var fieldset = layer.Items.OfType<Fieldset>().LastOrDefault(fs => fs.Contains(nextField));
            //view.ScrollToControl(layer.Items.IndexOf(fieldset), fieldset.IndexOf(nextField), true);

            return false;
        }
        #endregion
    }
}
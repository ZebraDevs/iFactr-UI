using iFactr.Core;
using iFactr.Core.Forms;
using iFactr.Core.Layers;
using iFactr.Core.Styles;

namespace iFactr.UI
{
    /// <summary>
    /// Provides methods for converting <see cref="iItem"/>, <see cref="Field"/>, and <see cref="IHtmlText"/> objects to <see cref="ICell"/>.
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Generates an <see cref="iFactr.UI.ICell"/> instance from the specified <see cref="iItem"/>.
        /// </summary>
        /// <param name="item">The item from which to generate the cell.</param>
        /// <param name="style">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the item style,
        /// or <c>null</c> if the item will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="recycledCell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified item.</returns>
        public static ICell Convert(this iItem item, Style style, IListView view, ICell recycledCell)
        {
            return iApp.Factory.Converter.ConvertToCell(item, style, view, recycledCell);
        }

        /// <summary>
        /// Generates an <see cref="iFactr.UI.ICell"/> instance from the specified <see cref="Field"/>.
        /// </summary>
        /// <param name="field">The field from which to generate the cell.</param>
        /// <param name="style">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the field style,
        /// or <c>null</c> if the field will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="recycledCell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified field.</returns>
        public static ICell Convert(this Field field, Style style, IListView view, ICell recycledCell)
        {
            return iApp.Factory.Converter.ConvertToCell(field, style, view, recycledCell);
        }

        /// <summary>
        /// Generates an <see cref="iFactr.UI.ICell"/> instance from the specified <see cref="iPanel"/>.
        /// </summary>
        /// <param name="panel">The block from which to generate the cell.</param>
        /// <param name="style">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the panel style,
        /// or <c>null</c> if the panel will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="recycledCell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified panel.</returns>
        public static ICell Convert(this iPanel panel, Style style, IListView view, ICell recycledCell)
        {
            return iApp.Factory.Converter.ConvertToCell(panel, style, view, recycledCell);
        }

        /// <summary>
        /// Generates an <see cref="iFactr.UI.ICell"/> instance from the specified <see cref="iBlock"/>.
        /// </summary>
        /// <param name="block">The block from which to generate the cell.</param>
        /// <param name="style">The <see cref="iFactr.Core.Styles.Style"/> instance that describes the block style,
        /// or <c>null</c> if the block will use the default style.</param>
        /// <param name="view">The view that the cell will reside in.</param>
        /// <param name="recycledCell">An <see cref="iFactr.UI.ICell"/> instance to use in place of a new one, or <c>null</c> if a new one is needed.</param>
        /// <returns>The generated cell for the specified block.</returns>
        public static ICell Convert(this iBlock block, Style style, IListView view, ICell recycledCell)
        {
            return iApp.Factory.Converter.ConvertToCell(block, style, view, recycledCell);
        }
    }
}
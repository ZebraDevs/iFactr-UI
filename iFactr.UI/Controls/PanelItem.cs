using iFactr.Core.Layers;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// An item that can be added to an <see cref="IHtmlText"/> element.
    /// </summary>
    public abstract class PanelItem : IBlockPanelItem
    {
        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representing this instance in HTML.</returns>
        public abstract string GetHtml();

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public PanelItem Clone()
        {
            return (PanelItem)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected virtual object CloneOverride()
        {
            return MemberwiseClone();
        }
    }
}
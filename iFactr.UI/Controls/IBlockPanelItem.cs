using iFactr.Core.Layers;

namespace iFactr.Core.Controls
{
    /// <summary>
    /// Defines an object that can be inserted into <see cref="iBlock"/>s and <see cref="iPanel"/>s.
    /// </summary>
    public interface IBlockPanelItem
    {
        /// <summary>
        /// Returns an HTML representation of this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representing this instance in HTML.</returns>
        string GetHtml();
    }
}
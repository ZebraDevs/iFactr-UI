using iFactr.Core.Layers;
using MonoCross.Utilities;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// The available large form-factor layout panes.
    /// </summary>
    public enum Pane
    {
        /// <summary>
        /// A tabbed pane, for displaying navigation tabs.
        /// </summary>
        Tabs,
        /// <summary>
        /// The master pane on the left side of the screen.
        /// </summary>
        Master,
        /// <summary>
        /// The detail pane on the right side of the screen.
        /// </summary>
        Detail,
        /// <summary>
        /// A popover pane that is superimposed over the other panes.
        /// </summary>
        Popover,
    }

    /// <summary>
    /// Provides helpers for the <see cref="Pane"/> enum.
    /// </summary>
    public static class PaneExtensions
    {
        /// <summary>
        /// Finds the target pane for the layer according to its interface.
        /// </summary>
        /// <param name="layer">The controller implementing a pane management interface.</param>
        /// <returns>The <see cref="Pane"/> to which the controller's view renders.</returns>
        public static Pane FindTarget(this IMXController layer)
        {
            var paneAttribute = Device.Reflector.GetCustomAttribute<PreferredPaneAttribute>(layer.GetType(), true);
            if (paneAttribute != null)
            {
                return paneAttribute.Pane;
            }

            if (layer is NavigationTabs)
            {
                return Pane.Tabs;
            }

#pragma warning disable 618

            if (layer is IMasterLayer)
            {
                return Pane.Master;
            }
            if (layer is IPopoverLayer)
            {
                return Pane.Popover;
            }

#pragma warning restore 618

            return Pane.Detail;
        }
    }
}
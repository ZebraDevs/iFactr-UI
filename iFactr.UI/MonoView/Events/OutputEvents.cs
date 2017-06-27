using System;
using iFactr.Core.Targets;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Provides data for the <see cref="TargetFactory.ViewOutputting"/> event.
    /// </summary>
    public class ViewOutputCancelEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the output should be canceled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the <see cref="IMXController"/> instance that the view being outputted belongs to.
        /// </summary>
        public IMXController Controller { get; private set; }

        /// <summary>
        /// Gets the <see cref="IMXView"/> instance that is being outputted.
        /// </summary>
        public IMXView View { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ViewOutputCancelEventArgs"/> class.
        /// </summary>
        /// <param name="controller">The <see cref="IMXController"/> instance that the view being outputted belongs to.</param>
        /// <param name="view">The <see cref="IMXView"/> instance that is being outputted.</param>
        public ViewOutputCancelEventArgs(IMXController controller, IMXView view)
        {
            Controller = controller;
            View = view;
        }
    }

    /// <summary>
    /// Provides data for the <see cref="TargetFactory.ViewOutputted"/> event.
    /// </summary>
    public class ViewOutputEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="IMXController"/> instance that the outputted view belongs to.
        /// </summary>
        public IMXController Controller { get; private set; }

        /// <summary>
        /// Gets the <see cref="IMXView"/> instance that was outputted.
        /// </summary>
        public IMXView View { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ViewOutputEventArgs"/> class.
        /// </summary>
        /// <param name="controller">The <see cref="IMXController"/> instance that the outputted view belongs to.</param>
        /// <param name="view">The <see cref="IMXView"/> instance that was outputted.</param>
        public ViewOutputEventArgs(IMXController controller, IMXView view)
        {
            Controller = controller;
            View = view;
        }
    }
}
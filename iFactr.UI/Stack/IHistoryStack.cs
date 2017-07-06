using System;
using System.Collections.Generic;
using iFactr.Core.Layers;
using MonoCross.Navigation;

namespace iFactr.Core
{
    /// <summary>
    /// Defines a view manager that can push and pop <see cref="IMXView"/> instances from a view stack.
    /// </summary>
    public interface IHistoryStack
    {
        /// <summary>
        /// Gets the viewport identifier of this stack.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Gets the <see cref="iLayer"/> that acts as the view-model for the <see cref="CurrentView"/>.
        /// </summary>
        iLayer CurrentLayer { get; }

        #region Obsolete members

        /// <summary>
        /// A stack of layers that used to be in the pane.
        /// </summary>
        [Obsolete("Use Views instead.")]
        IEnumerable<iLayer> History { get; }

        /// <summary>
        /// Clears the history stack through the given layer.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use PopToView instead.")]
        void PopToLayer(iLayer layer);

        /// <summary>
        /// Gets the last layer pushed onto the history stack.
        /// </summary>
        /// <returns>The <see cref="IMXView"/> on the top of the history stack.</returns>
        /// <remarks>This can be used to get information about the previous Layer.</remarks>
        [Obsolete("Use Views instead.")]
        iLayer Peek();

        /// <summary>
        /// Pushes the <see cref="CurrentView"/> onto the History to make way for another layer.
        /// </summary>
        /// <remarks>If the CurrentPerspective is associated with a LoginLayer, it will not be pushed to the stack history.</remarks>
        [Obsolete]
        void PushCurrent();

        /// <summary>
        /// Clears the history and current display.
        /// </summary>
        /// <remarks>If this is a popover stack, the popover is closed. If this is a detail stack, it will show the vanity panel.</remarks>
        [Obsolete("Use PopToRoot instead.")]
        void Clear(iLayer layer);

        #endregion

        #region MonoView stuff

        /// <summary>
        /// Gets the view that is currently being displayed on the stack.
        /// </summary>
        IMXView CurrentView { get; }

        /// <summary>
        /// Gets a collection of the views that are shown in this stack.
        /// </summary>
        /// <remarks>
        /// This collection represents the history of views shown on this stack plus the view currently shown.
        /// </remarks>
        IEnumerable<IMXView> Views { get; }

        /// <summary>
        /// Inserts the specified <see cref="IMXView"/> instance into the stack at the specified index.
        /// </summary>
        /// <param name="index">The index of the stack in which to insert the view.</param>
        /// <param name="view">The view to be inserted.</param>
        /// <exception cref="ArgumentException">Thrown when the view is of an unexpected type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the view is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index exceeds the upper or lower bound of the <see cref="Views"/> collection.</exception>
        void InsertView(int index, IMXView view);

        /// <summary>
        /// Removes all views from the stack except for the root.
        /// If this is a stack for a popover pane, it will also close the popover.
        /// </summary>
        /// <returns>The views that have been removed from the stack.</returns>
        IMXView[] PopToRoot();

        /// <summary>
        /// Removes from the stack all of the views that are on top of the specified <see cref="IMXView"/> instance.
        /// </summary>
        /// <param name="view">The view that should be on the top of the stack.</param>
        /// <exception cref="ArgumentException">Thrown when the view does not exist within the stack -or- when the view is of an unexpected type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the view is <c>null</c>.</exception>
        /// <returns>The views that have been removed from the stack.</returns>
        IMXView[] PopToView(IMXView view);

        /// <summary>
        /// Removes the top view from the stack.  If this is a stack for a popover pane
        /// and the top view is the root view, it will also close the popover.
        /// </summary>
        /// <returns>The view that was removed from the stack.</returns>
        IMXView PopView();

        /// <summary>
        /// Pushes the specified <see cref="IMXView"/> instance onto the stack.
        /// </summary>
        /// <param name="view">The view to be pushed.</param>
        /// <exception cref="ArgumentException">Thrown when the view is of an unexpected type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the view is <c>null</c>.</exception>
        void PushView(IMXView view);

        /// <summary>
        /// Removes the specified <see cref="IMXView"/> instance from the stack and inserts another view into its place.
        /// </summary>
        /// <param name="currentView">The view to be removed.</param>
        /// <param name="newView">The view to be inserted.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="currentView"/> does not exist within the stack -or- when the <paramref name="newView"/> is of an unexpected type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="currentView"/> is <c>null</c> -or- when the <paramref name="newView"/> is <c>null</c>.</exception>
        void ReplaceView(IMXView currentView, IMXView newView);

        #endregion
    }
}
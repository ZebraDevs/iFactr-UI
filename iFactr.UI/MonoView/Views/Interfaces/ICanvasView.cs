using System;
﻿using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a native view that acts as an ink canvas for drawing on.
    /// </summary>
    public interface ICanvasView : IView, IHistoryEntry
    {
        /// <summary>
        /// Gets or sets the color of the strokes when drawing.
        /// </summary>
        Color StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the strokes when drawing.
        /// </summary>
        double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets a toolbar of selectable items that provide support functions for the view.
        /// </summary>
        IToolbar Toolbar { get; set; }

        /// <summary>
        /// Clears the canvas of all foreground content.
        /// </summary>
        void Clear();

        /// <summary>
        /// Loads the specified file into the canvas.
        /// </summary>
        /// <param name="fileName">The full path of the file to load.</param>
        void Load(string fileName);

        /// <summary>
        /// Saves the current drawing to the temp directory with a randomly generated file name.
        /// </summary>
        /// <param name="compositeBackground">Whether to include the background as part of the saved image.</param>
        void Save(bool compositeBackground);

        /// <summary>
        /// Saves the current drawing to the specified file.
        /// </summary>
        /// <param name="fileName">The full path of the file in which to save the image.</param>
        void Save(string fileName);

        /// <summary>
        /// Saves the current drawing to the specified file.
        /// </summary>
        /// <param name="fileName">The full path of the file in which to save the image.</param>
        /// <param name="compositeBackground">Whether to include the background as part of the saved image.</param>
        void Save(string fileName, bool compositeBackground);

        /// <summary>
        /// Occurs when the current drawing has been saved to disk.
        /// </summary>
        [NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
        event SaveEventHandler DrawingSaved;
    }

    /// <summary>
    /// Defines a native view that acts as an ink canvas for drawing on.
    /// </summary>
    /// <typeparam name="T">The type of the Model.</typeparam>
    public interface ICanvasView<T> : ICanvasView, IMXView<T> { }
}
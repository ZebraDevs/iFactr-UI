using System;

namespace iFactr.UI
{
    /// <summary>
    /// Defines a set of property values that may differ from platform to platform.
    /// This interface is used by the framework to set default values for properties on cross-platform objects.
    /// </summary>
    public interface IPlatformDefaults
    {
        /// <summary>
        /// Gets the value that is appropriate as a margin between the left edge of a grid and the grid's contents.
        /// </summary>
        double LeftMargin { get; }

        /// <summary>
        /// Gets the value that is appropriate as a margin between the right edge of a grid and the grid's contents.
        /// </summary>
        double RightMargin { get; }

        /// <summary>
        /// Gets the value that is appropriate as a margin between the bottom edge of a grid and the grid's contents.
        /// </summary>
        double BottomMargin { get; }

        /// <summary>
        /// Gets the value that is appropriate as a margin between the top edge of a grid and the grid's contents.
        /// </summary>
        double TopMargin { get; }

        /// <summary>
        /// Gets the value that is appropriate for a large amount of left or right spacing between UI elements.
        /// </summary>
        double LargeHorizontalSpacing { get; }

        /// <summary>
        /// Gets the value that is appropriate for a small amount of left or right spacing between UI elements.
        /// </summary>
        double SmallHorizontalSpacing { get; }

        /// <summary>
        /// Gets the value that is appropriate for a large amount of top or bottom spacing between UI elements.
        /// </summary>
        double LargeVerticalSpacing { get; }

        /// <summary>
        /// Gets the value that is appropriate for a small amount of top or bottom spacing between UI elements.
        /// </summary>
        double SmallVerticalSpacing { get; }

        /// <summary>
        /// Gets a value for the height of a typical cell in an <see cref="IListView"/>.
        /// </summary>
        double CellHeight { get; }

        /// <summary>
        /// Gets the font for button controls.
        /// </summary>
        Font ButtonFont { get; }

        /// <summary>
        /// Gets the font for date and time picker controls.
        /// </summary>
        Font DateTimePickerFont { get; }

        /// <summary>
        /// Gets the font for label controls that act as headers.
        /// </summary>
        Font HeaderFont { get; }

        /// <summary>
        /// Gets the font for standard label controls.
        /// </summary>
        Font LabelFont { get; }

        /// <summary>
        /// Gets the font for label controls that act as the body of messages.
        /// </summary>
        Font MessageBodyFont { get; }

        /// <summary>
        /// Gets the font for label controls that act as the title of messages.
        /// </summary>
        Font MessageTitleFont { get; }

        /// <summary>
        /// Gets the font for section header text.
        /// </summary>
        Font SectionHeaderFont { get; }

        /// <summary>
        /// Gets the font for section footer text.
        /// </summary>
        Font SectionFooterFont { get; }

        /// <summary>
        /// Gets the font for select list controls.
        /// </summary>
        Font SelectListFont { get; }

        /// <summary>
        /// Gets the font for label controls with small text.
        /// </summary>
        Font SmallFont { get; }

        /// <summary>
        /// Gets the font for standard tab items.
        /// </summary>
        Font TabFont { get; }

        /// <summary>
        /// Gets the font for text box controls.
        /// </summary>
        Font TextBoxFont { get; }

        /// <summary>
        /// Gets the font for label controls showing data values.
        /// </summary>
        Font ValueFont { get; }
    }
}


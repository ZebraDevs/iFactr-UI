using System.Linq;
using iFactr.Core;
using iFactr.UI.Controls;
using iFactr.UI.Instructions;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a <see cref="GridCell"/> with predefined labels and a predefined image.
    /// </summary>
    public sealed class ContentCell : GridCell, ILayoutInstruction
    {
        /// <summary>
        /// Gets a predefined image control for the cell.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets a predefined label control for displaying the primary textual data in the cell.
        /// </summary>
        public Label TextLabel { get; private set; }

        /// <summary>
        /// Gets a predefined label control for displaying any secondary textual data in the cell.
        /// This label typically appears underneath the <see cref="P:TextLabel"/>.
        /// </summary>
        public Label SubtextLabel { get; private set; }

        /// <summary>
        /// Gets a predefined label control for displaying data values.
        /// This label typically appears to the right of the <see cref="P:TextLabel"/>.
        /// </summary>
        public Label ValueLabel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ContentCell"/> class.
        /// </summary>
        public ContentCell()
        {
            MinHeight = MaxHeight = Cell.StandardCellHeight;

            Rows.Add(Row.AutoSized);
            Rows.Add(Row.AutoSized);
            Columns.Add(Column.AutoSized);
            Columns.Add(Column.OneStar);
            Columns.Add(Column.AutoSized);

            Image = new Image();
            Image.RowIndex = 0;
            Image.RowSpan = 2;
            Image.ColumnIndex = 0;
            Image.Margin = new Thickness(-Thickness.LeftMargin, -Thickness.TopMargin, Thickness.LargeHorizontalSpacing, -Thickness.BottomMargin);
            Image.HorizontalAlignment = HorizontalAlignment.Left;
            Image.VerticalAlignment = VerticalAlignment.Center;
            AddChild(Image);

            TextLabel = new Label();
            TextLabel.RowIndex = 0;
            TextLabel.ColumnIndex = 1;
            TextLabel.HorizontalAlignment = HorizontalAlignment.Left;
            TextLabel.VerticalAlignment = VerticalAlignment.Center;
            TextLabel.Lines = 1;
            AddChild(TextLabel);

            SubtextLabel = new Label();
            SubtextLabel.Font = Font.PreferredSmallFont;
            SubtextLabel.RowIndex = 1;
            SubtextLabel.ColumnIndex = 1;
            SubtextLabel.ColumnSpan = 2;
            SubtextLabel.HorizontalAlignment = HorizontalAlignment.Left;
            SubtextLabel.VerticalAlignment = VerticalAlignment.Center;
            SubtextLabel.ForegroundColor = iApp.Instance.Style.SubTextColor;
            SubtextLabel.Lines = 1;
            AddChild(SubtextLabel);

            ValueLabel = new Label();
            ValueLabel.Font = Font.PreferredValueFont;
            ValueLabel.Margin = new Thickness(Thickness.SmallHorizontalSpacing, 0, 0, 0);
            ValueLabel.RowIndex = 0;
            ValueLabel.ColumnIndex = 2;
            ValueLabel.HorizontalAlignment = HorizontalAlignment.Right;
            ValueLabel.VerticalAlignment = VerticalAlignment.Center;
            ValueLabel.ForegroundColor = iApp.Instance.Style.SecondarySubTextColor;
            ValueLabel.Lines = 1;
            AddChild(ValueLabel);
        }

        void ILayoutInstruction.Layout()
        {
            if (string.IsNullOrEmpty(Image.FilePath))
            {
                RemoveChild(Image);
            }
            else
            {
                if (!Children.Contains(Image))
                {
                    AddChild(Image);
                }

                if (Image.VerticalAlignment != VerticalAlignment.Center && Image.VerticalAlignment != VerticalAlignment.Stretch)
                {
                    Image.Margin = new Thickness(Image.Margin.Left, 0, Image.Margin.Right, Image.Margin.Bottom);
                }
                else
                {
                    Image.Margin = new Thickness(Image.Margin.Left, -Thickness.TopMargin, Image.Margin.Right, Image.Margin.Bottom);
                }

                Image.RowIndex = 0;
                Image.RowSpan = Rows.Count;
                Image.ColumnIndex = 0;
            }

            TextLabel.RowIndex = 0;
            TextLabel.ColumnIndex = 1;

            SubtextLabel.ColumnIndex = 1;
            ValueLabel.RowIndex = 0;

            SubtextLabel.RowIndex = 1;
            ValueLabel.ColumnIndex = 2;
        }
    }
}

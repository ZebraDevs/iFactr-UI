using iFactr.Core.Controls;
using iFactr.Core.Styles;
using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using Link = iFactr.UI.Link;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Defines a control that accepts HTML strings.
    /// </summary>
    public interface IHtmlText
    {
        /// <summary>
        /// Gets or sets a string of HTML.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets a collection of HTML-building items.
        /// </summary>
        List<PanelItem> Items { get; set; }
    }

    /// <summary>
    /// Represents methods for appending HTML strings to an HTML control.
    /// </summary>
    public static class HtmlTextExtensions
    {
        /// <summary>
        /// Appends the specified text.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void Append(this IHtmlText htmlText, string text)
        {
            var label = new Label() { Text = text.CleanEntities(), };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        public static void AppendLine(this IHtmlText htmlText)
        {
            var line = new Break();
            htmlText.Items.Add(line);
        }

        /// <summary>
        /// Appends the specified text, followed by a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendLine(this IHtmlText htmlText, string text)
        {
            htmlText.Append(text);
            htmlText.AppendLine();
        }

        /// <summary>
        /// Appends the specified text with bold formatting.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendBold(this IHtmlText htmlText, string text)
        {
            var label = new Label()
            {
                Text = text,
                Style = { TextFormat = LabelStyle.Format.Bold, },
            };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends the specified text with bold formatting, followed by a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendBoldLine(this IHtmlText htmlText, string text)
        {
            htmlText.AppendBold(text);
            htmlText.AppendLine();
        }

        /// <summary>
        /// Appends the specified text with italic formatting.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendItalic(this IHtmlText htmlText, string text)
        {
            var label = new Label()
            {
                Text = text,
                Style = { TextFormat = LabelStyle.Format.Italic, },
            };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends the specified text with italic formatting, followed by a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendItalicLine(this IHtmlText htmlText, string text)
        {
            htmlText.AppendItalic(text);
            htmlText.AppendLine();
        }

        /// <summary>
        /// Appends the specified text with bold italic formatting.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendBoldItalic(this IHtmlText htmlText, string text)
        {
            var label = new Label()
            {
                Text = text,
                Style = { TextFormat = LabelStyle.Format.BoldItalic, },
            };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends the specified text with bold italic formatting, followed by a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendBoldItalicLine(this IHtmlText htmlText, string text)
        {
            htmlText.AppendBoldItalic(text);
            htmlText.AppendLine();
        }

        /// <summary>
        /// Appends a horizontal rule.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        public static void AppendHorizontalRule(this IHtmlText htmlText)
        {
            var line = new Break() { IsHorizontalRule = true, };
            htmlText.Items.Add(line);
        }

        /// <summary>
        /// Appends a heading with the specified text.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendHeading(this IHtmlText htmlText, string text)
        {
            var label = new HeaderLabel() { Text = text, };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends a sub heading with the specified text.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendSubHeading(this IHtmlText htmlText, string text)
        {
            var label = new HeaderLabel() { Text = text, Style = { HeaderLevel = 3, }, };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Appends a secondary sub heading with the specified text.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append.</param>
        public static void AppendSecondarySubHeading(this IHtmlText htmlText, string text)
        {
            var label = new HeaderLabel() { Text = text, Style = { HeaderLevel = 4, }, };
            htmlText.Items.Add(label);
        }

        /// <summary>
        /// Inserts an inlined image.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="maxHeight">The maximum height of the image.</param>
        /// <param name="maxWidth">The maximum width of the image.</param>
        public static void InsertImage(this IHtmlText htmlText, string imageSource, string maxHeight, string maxWidth)
        {
            htmlText.InsertImage(imageSource, null, maxHeight, maxWidth);
        }

        /// <summary>
        /// Inserts an inlined image.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        public static void InsertImage(this IHtmlText htmlText, string imageSource)
        {
            htmlText.InsertImage(imageSource, null, null, null);
        }

        /// <summary>
        /// Inserts an inlined image.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="alignment">A <see cref="String"/> representing the image's alignment.</param>
        /// <param name="maxHeight">The maximum height of the image.</param>
        /// <param name="maxWidth">The maximum width of the image.</param>
        private static void InsertImage(this IHtmlText htmlText, string imageSource, string alignment, string maxHeight, string maxWidth)
        {
            var icon = new Icon { Location = imageSource, Align = alignment, Height = maxHeight, Width = maxWidth, };
            htmlText.Items.Add(icon);
        }

        /// <summary>
        /// Inserts an inlined image.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="alignment">A <see cref="String"/> representing the image's alignment.</param>
        private static void InsertImage(this IHtmlText htmlText, string imageSource, string alignment)
        {
            htmlText.InsertImage(imageSource, alignment, null, null);
        }

        /// <summary>
        /// Inserts an inlined image with a link attached to it.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="url">A <see cref="String"/> representing the URL to navigate to when the image is selected.</param>
        public static void InsertImageLink(this IHtmlText htmlText, string imageSource, string url)
        {
            htmlText.AppendLink(new Link(url) { ImagePath = imageSource });
        }

        /// <summary>
        /// Inserts an image floating to the left.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="maxHeight">The maximum height of the image.</param>
        /// <param name="maxWidth">The maximum width of the image.</param>
        public static void InsertImageFloatLeft(this IHtmlText htmlText, string imageSource, string maxHeight, string maxWidth)
        {
            htmlText.InsertImage(imageSource, "left", maxHeight, maxWidth);
        }

        /// <summary>
        /// Inserts an image floating to the left.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        public static void InsertImageFloatLeft(this IHtmlText htmlText, string imageSource)
        {
            htmlText.InsertImage(imageSource, "left");
        }

        /// <summary>
        /// Inserts an image floating to the right.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        /// <param name="maxHeight">The maximum height of the image.</param>
        /// <param name="maxWidth">The maximum width of the image.</param>
        public static void InsertImageFloatRight(this IHtmlText htmlText, string imageSource, string maxHeight, string maxWidth)
        {
            htmlText.InsertImage(imageSource, "right", maxHeight, maxWidth);
        }

        /// <summary>
        /// Inserts an image floating to the right.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="imageSource">A <see cref="String"/> representing the image source value.</param>
        public static void InsertImageFloatRight(this IHtmlText htmlText, string imageSource)
        {
            htmlText.InsertImage(imageSource, "right");
        }

        /// <summary>
        /// Appends an inlined link.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="link">A <see cref="Link"/> representing the link to append.</param>
        public static void AppendLink(this IHtmlText htmlText, Link link)
        {
            htmlText.Items.Add(link);
        }

        /// <summary>
        /// Appends an inlined email (mailto:) link.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="email">A <see cref="String"/> representing the email address to create the link for.</param>
        public static void AppendEmail(this IHtmlText htmlText, string email)
        {
            htmlText.AppendLink(new Link("mailto:" + email) { Text = email });
        }

        /// <summary>
        /// Appends an inlined telephone (tel:) link.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="telephone">A <see cref="String"/> representing the telephone number to create the link for.</param>
        public static void AppendTelephone(this IHtmlText htmlText, string telephone)
        {
            htmlText.AppendLink(new Link("tel:" + telephone.Replace(" ", string.Empty)) { Text = telephone });
        }

        /// <summary>
        /// Appends the specified label with bold formatting and a colon, followed by the text with normal formatting, followed by a line break.
        /// </summary>
        /// <param name="htmlText">An <see cref="IHtmlText"/> representing the HTML control.</param>
        /// <param name="label">A <see cref="String"/> representing the label to append with bold formatting.</param>
        /// <param name="text">A <see cref="String"/> representing the text to append with normal formatting.</param>
        public static void AppendLabeledTextLine(this IHtmlText htmlText, string label, string text)
        {
            htmlText.AppendBold(label + ":");
            htmlText.AppendLine(text);
        }

        /// <summary>
        /// Converts the specified path into a virtual directory path and returns the result.
        /// </summary>
        /// <param name="path">The path to convert to a virtual directory path.</param>
        /// <returns>The virtual directory path.</returns>
        public static string VirtualPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith("data:"))
                return path;
            string cleanPath = path;
            int endPath = cleanPath.IndexOf('\'');
            if (endPath > -1)
#if !NETCF
                cleanPath = cleanPath.Remove(endPath);
#else 
                cleanPath = cleanPath.Remove(endPath, cleanPath.Length - endPath);
#endif

            //            if (!new Uri(cleanPath, UriKind.RelativeOrAbsolute).IsAbsoluteUri && !cleanPath.StartsWith(iApp.Factory.WebAppVirtualPath))
            //                path = (iApp.Factory.WebAppVirtualPath + path).Replace("//", "/");
            return path;
        }
    }
}
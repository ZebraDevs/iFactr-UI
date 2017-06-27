using System.Collections.Generic;

namespace iFactr.Core.Integrations
{
    /// <summary>
    /// Represents a "mailto:" scheme parser for common handling across platforms.
    /// </summary>
    /// <remarks>The attachment parsing may not work on all platforms.</remarks>
    public class MailTo
    {
        /// <summary>
        /// Gets or sets a collection of email addresses to include in the recipient list.
        /// </summary>
        public List<string> EmailTo { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the message body of the email.
        /// </summary>
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets a collection of attachments to include in the email.
        /// </summary>
        public List<Attachment> EmailAttachments { get; set; }

        /// <summary>
        /// Represents an email attachment.
        /// </summary>
        public class Attachment
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Attachment"/> class.
            /// </summary>
            public Attachment()
            {
                Filename = string.Empty;
                MimeType = string.Empty;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Attachment"/> class.
            /// </summary>
            /// <param name="filename">The path of the file to attach.</param>
            /// <param name="mimeType">The type of the file to attach.</param>
            public Attachment(string filename, string mimeType)
            {
                Filename = filename;
                MimeType = mimeType;
            }

            /// <summary>
            /// Gets or sets the path of the file to attach.
            /// </summary>
            public string Filename { get; set; }

            /// <summary>
            /// Gets or sets the type of the file to attach.
            /// </summary>
            public string MimeType { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailTo"/> class.
        /// </summary>
        public MailTo()
        {
            EmailTo = new List<string>();
            EmailAttachments = new List<Attachment>();
            EmailSubject = string.Empty;
            EmailBody = string.Empty;
        }

        /// <summary>
        /// Parses the specified "mailto:" URL into a <see cref="MailTo"/> instance.
        /// </summary>
        /// <param name="mailToUrl">The "mailto:" URL to parse into a <see cref="MailTo"/> instance.</param>
        /// <returns>The <see cref="MailTo"/> instance that was generated from the URL.</returns>
        public static MailTo ParseUrl(string mailToUrl)
        {
            MailTo mailTo = new MailTo();
            mailToUrl = mailToUrl.Replace("mailto:", "").Replace("%20", " ");

            int lastIndex = 0;
            for (int i = 0; i < mailToUrl.Length; i++)
            {
                if (mailToUrl[i] == '?')
                {
                    if (!string.IsNullOrEmpty(mailToUrl.Substring(lastIndex, i - lastIndex)))
                        mailTo.EmailTo.Add(mailToUrl.Substring(lastIndex, i - lastIndex));
                    break;
                }
                if (i == mailToUrl.Length - 1)
                {
                    if (!string.IsNullOrEmpty(mailToUrl.Substring(lastIndex, mailToUrl.Length - lastIndex)))
                        mailTo.EmailTo.Add(mailToUrl.Substring(lastIndex, mailToUrl.Length - lastIndex));
                    break;
                }
                if (mailToUrl[i] == ',')
                {
                    mailTo.EmailTo.Add(mailToUrl.Substring(lastIndex, i - lastIndex));
                    lastIndex = i + 1;
                }
            }

            if (mailToUrl.Contains("subject="))
            {
                if (mailToUrl.Substring(mailToUrl.IndexOf("subject=")).Contains("&"))
                {
                    mailTo.EmailSubject = mailToUrl.Substring(mailToUrl.IndexOf("subject=") + 8,
                    mailToUrl.Substring(mailToUrl.IndexOf("subject=") + 8).IndexOf("&"));
                }
                else
                {
                    mailTo.EmailSubject = mailToUrl.Substring(mailToUrl.IndexOf("subject=") + 8,
                            mailToUrl.Substring(mailToUrl.IndexOf("subject=") + 8).Length);
                }
            }

            if (mailToUrl.Contains("body="))
            {
                if (mailToUrl.Substring(mailToUrl.IndexOf("body=")).Contains("&"))
                {
                    mailTo.EmailBody = mailToUrl.Substring(mailToUrl.IndexOf("body=") + 5,
                        mailToUrl.Substring(mailToUrl.IndexOf("body=") + 5).IndexOf("&"));
                }
                else
                {
                    mailTo.EmailBody = mailToUrl.Substring(mailToUrl.IndexOf("body=") + 5,
                        mailToUrl.Substring(mailToUrl.IndexOf("body=") + 5).Length);
                }
            }

            if (mailToUrl.Contains("attach="))
            {
                string attachUrl = mailToUrl.Substring(mailToUrl.IndexOf("attach=") + 7);

                if (mailToUrl.Substring(mailToUrl.IndexOf("attach=")).Contains("&"))
                {
                    attachUrl = attachUrl.Substring(0, attachUrl.IndexOf("&"));
                }
                lastIndex = 0;

                for (int i = 0; i < attachUrl.Length; i++)
                {
                    string filename = null;
                    if (attachUrl[i] == ',')
                    {
                        filename = attachUrl.Substring(lastIndex, i - lastIndex);
                        lastIndex = i + 1;
                    }
                    else if (i == attachUrl.Length - 1)
                    {
                        if (!string.IsNullOrEmpty(attachUrl.Substring(lastIndex, i + 1 - lastIndex)))
                            filename = attachUrl.Substring(lastIndex, i + 1 - lastIndex);
                    }
                    else
                        continue;

                    int index = filename.LastIndexOf(',');
                    if (index > -1)
                        filename = filename.Substring(index);

                    mailTo.EmailAttachments.Add(new Attachment(filename, filename.Substring(filename.LastIndexOf('.') + 1)));
                }
            }

            return mailTo;
        }
    }
}
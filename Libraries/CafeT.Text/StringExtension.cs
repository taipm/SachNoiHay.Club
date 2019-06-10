using CafeT.Enumerable;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CafeT.Text
{
    public static partial class StringExtension
    {
        public static string ConvertToString(this List<string> list, string token)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string cat in list) // Loop through all strings
            {
                builder.Append(cat).Append(token); // Append string to StringBuilder
            }
            string result = builder.ToString(); // Get string from StringBuilder
            return result;
        }
    }

    public static partial class StringExtension
    {
        private static string _paraBreak = "\r\n\r\n";
        private static string _link = "<a href=\"{0}\">{1}</a>";
        private static string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

        #region Delete
        public static string DeleteLastest(this string input, string token)
        {
            if (input.Contains(token))
            {
                int lastIndex = input.LastIndexOf(token);
                return input.Remove(lastIndex, token.Length);
            }
            return input;
        }
        public static string DeleteFirst(this string input, string token)
        {
            if (input.Contains(token))
            {
                int firstIndex = input.IndexOf(token);
                return input.Remove(firstIndex, token.Length);
            }
            return input;
        }
        #endregion
        #region Get
        public static string GetLastestFrom(this string input, string token, bool isInclude)
        {
            string s = input;
            if (input.Contains(token))
            {
                int lastIndex = input.LastIndexOf(token);
                if(isInclude)
                {
                    return s.Substring(lastIndex, s.Length - lastIndex);
                }
                else
                {
                    return s.Substring(lastIndex+1, s.Length - lastIndex);
                }
            }
            return s;
        }
        #endregion
        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// </summary>
        public static string ToHtml(this string s)
        {
            return ToHtml(s, false);
        }

        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// </summary>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        public static string ToHtml(this string s, bool nofollow)
        {
            StringBuilder sb = new StringBuilder();

            int pos = 0;
            while (pos < s.Length)
            {
                // Extract next paragraph
                int start = pos;
                pos = s.IndexOf(_paraBreak, start);
                if (pos < 0)
                    pos = s.Length;
                string para = s.Substring(start, pos - start).Trim();

                // Encode non-empty paragraph
                if (para.Length > 0)
                    EncodeParagraph(para, sb, nofollow);

                // Skip over paragraph break
                pos += _paraBreak.Length;
            }
            // Return result
            return sb.ToString();
        }

        /// <summary>
        /// Encodes a single paragraph to HTML.
        /// </summary>
        /// <param name="s">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow)
        {
            // Start new paragraph
            sb.AppendLine("<p>");

            // HTML encode text
            s = HttpUtility.HtmlEncode(s);

            // Convert single newlines to <br>
            s = s.Replace(Environment.NewLine, "<br />\r\n");

            // Encode any hyperlinks
            EncodeLinks(s, sb, nofollow);

            // Close paragraph
            sb.AppendLine("\r\n</p>");
        }

        /// <summary>
        /// Encodes [[URL]] and [[Text][URL]] links to HTML.
        /// </summary>
        /// <param name="text">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeLinks(string s, StringBuilder sb, bool nofollow)
        {
            // Parse and encode any hyperlinks
            int pos = 0;
            while (pos < s.Length)
            {
                // Look for next link
                int start = pos;
                pos = s.IndexOf("[[", pos);
                if (pos < 0)
                    pos = s.Length;
                // Copy text before link
                sb.Append(s.Substring(start, pos - start));
                if (pos < s.Length)
                {
                    string label, link;

                    start = pos + 2;
                    pos = s.IndexOf("]]", start);
                    if (pos < 0)
                        pos = s.Length;
                    label = s.Substring(start, pos - start);
                    int i = label.IndexOf("][");
                    if (i >= 0)
                    {
                        link = label.Substring(i + 2);
                        label = label.Substring(0, i);
                    }
                    else
                    {
                        link = label;
                    }
                    sb.Append(String.Format(nofollow ? _linkNoFollow : _link, link, label));
                    pos += 2;
                }
            }
        }
    }

    
}

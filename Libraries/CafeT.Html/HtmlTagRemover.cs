using System;
using System.IO;

using System.Text;

using System.Text.RegularExpressions;

namespace CafeT.Html
{
    public static class HtmlTagRemover

    {



        /// <summary>

        /// Replaces every tag with new line

        /// </summary>

        public static string RemoveAllTags(string str)

        {

            string strWithoutTags =

                Regex.Replace(str, "<[^>]*>", "\n");

            return strWithoutTags;

        }



        /// <summary>

        /// Replaces sequence of new lines with only one new line

        /// </summary>

        public static string RemoveDoubleNewLines(string str)

        {

            string pattern = "[\n]+";

            return Regex.Replace(str, pattern, "\n");

        }



        /// <summary>

        /// Removes new lines from start and end of string

        /// </summary>

        public static string TrimNewLines(string str)

        {

            int start = 0;

            while (start < str.Length && str[start] == '\n')

            {

                start++;

            }



            int end = str.Length - 1;

            while (end >= 0 && str[end] == '\n')

            {

                end--;

            }



            if (start > end)

            {

                return string.Empty;

            }



            string trimmed = str.Substring(start, end - start + 1);

            return trimmed;

        }

    }
}


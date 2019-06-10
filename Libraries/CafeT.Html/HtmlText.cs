using CafeT.Enumerable;
using CafeT.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CafeT.Html
{
    
    public static class HtmlText
    {
        public static string RemoveImages(this string htmlString)
        {
            string[] _images = htmlString.GetImages();
            if (_images.Count() >= 1)
                foreach (string _image in _images)
                {
                    htmlString = htmlString.Replace(_image, "");
                }
            return htmlString;
        }

        public static string[] GetImages(this string htmlString)
        {
            if (htmlString.IsNullOrEmptyOrWhiteSpace()) return null;
            List<string> images = new List<string>();
            string pattern = @"<(img)\b[^>]*>";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                images.Add(matches[i].Value);
            }

            return images.ToArray();
        }
        
        /// <summary>
        /// width:%
        /// height:auto or %
        /// </summary>
        /// <param name="html"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string ResizeImages(this string html, string width, string height)
        {
            var oldImages = html.GetImages().ToList();
            string copy = html;
            foreach (string img in oldImages)
            {
                string _imageHref = img.GetUrls().FirstOrDefault();
                string newImg = "<img src=" + _imageHref;
                string _width = " width= \"" + width + "\"";
                string _heigh = " height= \"" + height + "\"";
                //if (!img.Contains("width"))
                //{
                //    newImg = newImg + _width;
                //}
                //if (!img.Contains("height"))
                //{
                //    newImg = newImg + _heigh;
                //}
                newImg = newImg + _width;
                newImg = newImg + _heigh;
                newImg = newImg + ">";
                copy = copy.Replace(img, newImg);
            }
            return copy;
        }

        /// <summary>
        /// Return the first n words in the html
        /// </summary>
        /// <param name="html"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string TakeMaxWords(this string html, int n)
        {
            string words = html, n_words;
            words = html.StripHtml();
            n_words = GetNWords(words, n);
            return n_words;
        }


        /// <summary>
        /// Returns the first n words in text
        /// Assumes text is not a html string
        /// http://stackoverflow.com/questions/13368345/get-first-250-words-of-a-string
        /// http://stackoverflow.com/questions/1279859/how-to-replace-multiple-white-spaces-with-one-white-space
        /// </summary>
        /// <param name="text"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetNWords(this string text, int n)
        {
            StringBuilder builder = new StringBuilder();
            string cleanedString = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
            IEnumerable<string> words = cleanedString.Split().TakeMax(n + 1);
            foreach (string word in words)
                builder.Append(" " + word);

            return builder.ToString();
        }
    }
}

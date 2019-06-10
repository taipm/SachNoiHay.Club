using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CafeT.Text;

namespace Web.ModelViews
{
    public class ArticleView : BaseView
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string ContentUrl { set; get; }
        public string Content { set; get; }
        public string ImageUrl { set; get; }
        public string[] Tags { set; get; }
        public bool IsPublished { set; get; }
        public string CountViews { set; get; }
        public string EditLink { set; get; }
        public string DocumentId { set; get; }
        //SEO ...
        public string MetaDescription { set; get; }
        public string MetaKeywords { set; get; }

        // Slug generation taken from http://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
        public string GenerateSlug()
        {
            string titleToSlug = Title.RemoveUnicode();
            string phrase = string.Format("{0}-{1}", Id, titleToSlug);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

    }
}
using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Models
{
    public class Articles
    {
        public IList<ArticleModel> articles;
    }

    public class ArticleModel : BaseModel
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string ContentUrl { set; get; }
        public string Content { set; get; }
        public string ImageUrl { set; get; }
        public string Tags { set; get; }
        public bool IsPublished { set; get; }
        public string CountViews { set; get; }
        public string DocumentId { set; get; }

        //SEO ...
        public string MetaDescription { set; get; }
        public string MetaKeywords { set; get; }


        public bool IsGoogleDoc()
        {
            if (!ContentUrl.IsNullOrEmptyOrWhiteSpace())
            {
                if (ContentUrl.Contains("https://docs.google.com/document")) return true;
            }
            return false;
        }

        public bool Published()
        {
            if (IsPublished) return true;
            if (IsGoogleDoc() && ContentUrl.Contains("edit")) return false;
            return false;
        }
    }
}
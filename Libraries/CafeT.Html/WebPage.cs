using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeT.Html
{
    public class WebPage
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument html;
        public string Domain { set; get; }
        public string PathConfig { set; get; }
        public string Url { set; get; }
        public string Title { set; get; }
        public string Meta { set; get; }
        public string HtmlContent { set; get; }
        public string TextContent { set; get; }
        public List<string> CssDivs { set; get; } = new List<string>();
        public List<string> CssClasses { set; get; } = new List<string>();
        public List<string> Images { set; get; } = new List<string>();
        public List<HtmlTable> HtmlTables { set; get; } = new List<HtmlTable>();
        public List<string> Links { set; get; } = new List<string>();
        public List<string> InternalLinks { set; get; } = new List<string>();
        public List<string> ExternalLinks { set; get; } = new List<string>();
        public IEnumerable<HtmlNode> Nodes { set; get; }
        public List<string> Keywords { set; get; } = new List<string>(); //Keyword start with #
        HtmlParseError Errors { set; get; }

        public string[] NodesCss { set; get; }

        public WebPage() 
        {
            Title = string.Empty;
        }

        public WebPage(string url)
        {
            Url = url;
        }

        public bool IsGoogleDoc()
        {
            if (Url.Contains("https://docs.google.com/document")) return true;
            return false;
        }
        public bool IsGoogleSheet()
        {
            if (Url.Contains("https://docs.google.com/sheet")) return true;
            return false;
        }
        //public void BuildKeywords()
        //{
        //    #region Url
        //    if (Url.ToLower().Contains("truyen"))
        //    {
        //        Keywords.Add("#image"); //Demo
        //    }
        //    else if(Url.IsImageUrl())
        //    {
        //        Keywords.Add("#image"); //Demo
        //    }
        //    #endregion
        //    #region TextContent
        //    if (HtmlContent.ToLower().Contains("Ngân hàng".ToLower())
        //        || HtmlContent.ToLower().Contains("bank".ToLower())
        //        )
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("tỷ giá"))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("lãi suất".ToLower()))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("vietlott".ToLower()))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("xổ số".ToLower()))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("bảng giá".ToLower()))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    else if (HtmlContent.ToLower().Contains("hàng hóa".ToLower()))
        //    {
        //        Keywords.Add("#table"); //Demo
        //    }
        //    #endregion

        //    Keywords = Keywords.Distinct().ToList();
        //}

        public void GetHtmlTables()
        {
            var tables = html.DocumentNode.SelectNodes("//table");
            if (tables != null)
            {
                foreach (string table in tables.Select(t=>t.OuterHtml))
                {
                    HtmlTable _table = new HtmlTable(table);
                    HtmlTables.Add(_table);
                }
                HtmlTables = HtmlTables.Distinct().ToList();
            }
        }

        public string GetDomain()
        {
            Uri myUri = new Uri(Url);
            string host = myUri.Host;
            return host;
        }

        public void LoadTitle()
        {
            var nodes = Nodes
                .Where(t => (t.InnerText.ToStandard().GetCountWords() >= 5)
                && (t.InnerText.ToStandard().GetCountWords() <= 35));

            foreach (HtmlNode node in nodes)
            {
                if (node.InnerHtml.Contains("title"))
                {
                    Title = node.InnerText.HtmlToText().ToStandard().GetFirstSentence();
                    return;
                }
                else
                {
                    Title = nodes.FirstOrDefault().InnerText.HtmlToText().ToStandard();
                    return;
                }
            }
        }

        /// <summary>
        /// Load and assign HtmlContent and TextContent for WebPage.
        /// </summary>
        public void LoadContent()
        {
            web = new HtmlWeb();
            html = web.Load(Url);
            HtmlContent = html.DocumentNode.OuterHtml;
            TextContent = HtmlContent.HtmlToText();
            GetHtmlTables();
        }

        public List<HtmlNode> GetNodesByClass(string className)
        {
            return html.DocumentNode.GetNodesByClasses(className);
        }
        public HtmlNode GetNodeById(string idName)
        {
            return html.GetElementbyId(idName);
        }
        public List<HtmlNode> GetNodesByXPath(string xpath)
        {
            return html.DocumentNode.SelectNodes(xpath).ToList();
        }
        public void GetImages(string ext)
        {
            var imageLinks = Links.Where(t => t.Contains(ext)).ToList();
            try
            {
                var images = HtmlContent.GetImages().Distinct().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Images = imageLinks;
        }
       
        public bool IsLoaded = false;
        public List<string> CssIds { set; get; } = new List<string>();

        public void Load()
        {
            if (IsLoaded) return;
            if (Url.IsUrl())
            {
                Domain = GetDomain();
                web = new HtmlWeb();
                html = web.Load(Url);
                HtmlContent = html.DocumentNode.OuterHtml;
                TextContent = HtmlContent.HtmlToText();
                var _allLinks = HtmlContent.GetUrls();
                GetHtmlTables();
                //BuildKeywords();
                CssIds = HtmlContent.GetAllIds().ToList();
                CssClasses = HtmlContent.GetAllClasses()
                    .ToList();
                Nodes = html.GetNodes().Where(t => t.IsCssClass());
                    //.Where(t=>t.OuterHtml.Contains("class"));
                foreach (string _link in _allLinks)
                {
                    if (_link.StartsWith("/"))
                    {
                        string _url = Url.GetBefore(Domain) + Domain + _link;
                        Links.Add(_url);
                    }
                    else
                    {
                        Links.Add(_link);
                    }
                }
                if (Links != null)
                {
                    InternalLinks = Links.Where(t => t.StartsWith(Url.GetBefore(Domain) + Domain)).ToList();
                }
                GetImages("jpg");
                LoadTitle();
                IsLoaded = true;
            }
        }
    }
}

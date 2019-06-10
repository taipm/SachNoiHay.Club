using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Web.Models
{
    public class ApplicationDbContext
    {
        private string ConnectionString = "https://spreadsheets.google.com/feeds/list/1QMLAbUgfLUtgZN4FCFY4aWc5eZFznXLTUEpKEhTPzOs/od6/public/values?alt=json";
        public ArticleModel[] GetNews()
        {
            List<ArticleModel> books = new List<ArticleModel>();

            using (WebClient httpClient = new WebClient())
            {
                httpClient.Encoding = Encoding.UTF8;

                List<string> data = new List<string>();
                var jsonData = httpClient.DownloadString(ConnectionString);

                dynamic result = JsonValue.Parse(jsonData);
                if (result.ContainsKey("feed"))
                {
                    dynamic feed = result["feed"];
                    if (feed.ContainsKey("entry"))
                    {
                        dynamic entry = feed["entry"];

                        for (int i = 0; i < entry.Count; i++)
                        {
                            ArticleModel book = Mappers.Mapper.EntryToArticle(entry[i]);

                            books.Add(book);
                        }
                        return books.OrderByDescending(t => t.Id)
                            .ToArray();
                    }
                }
                return null;
            }
        }

        public ArticleModel[] GetArticles()
        {
            List<ArticleModel> books = new List<ArticleModel>();

            using (WebClient httpClient = new WebClient())
            {
                httpClient.Encoding = Encoding.UTF8;

                List<string> data = new List<string>();
                var jsonData = httpClient.DownloadString(ConnectionString);

                dynamic result = JsonValue.Parse(jsonData);
                if (result.ContainsKey("feed"))
                {
                    dynamic feed = result["feed"];
                    if (feed.ContainsKey("entry"))
                    {
                        dynamic entry = feed["entry"];

                        for (int i = 0; i < entry.Count; i++)
                        {
                            ArticleModel book = Mappers.Mapper.EntryToArticle(entry[i]);

                            books.Add(book);
                        }
                        return books.OrderByDescending(t => t.Id)
                            .ToArray();
                    }
                }
                return null;
            }
        }

        public ArticleModel GetArticleByContentUrl(string url)
        {
            var article = GetArticles().Where(t => t.ContentUrl == url).FirstOrDefault();
            return article;
        }

        public ArticleModel GetArticleById(int id)
        {
            var article = GetArticles().Where(t => t.Id == id).FirstOrDefault();
            return article;
        }
    }
}
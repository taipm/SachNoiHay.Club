using CafeT.Html;
using CafeT.Text;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Web.ModelViews;

namespace Web.Controllers
{
    public class ArticlesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public List<ArticleModel> PublishedArticles = new List<ArticleModel>();

        public ArticlesController()
        {
            PublishedArticles = db.GetArticles()
                .Where(t => t.Published())
                .ToList();
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var modelviews = Mappers.Mapper.ArticlesToViews(PublishedArticles);
            PagedList<ArticleView> views = new PagedList<ArticleView>(modelviews, page, pageSize);

            ViewBag.Count = PublishedArticles.Count();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Articles", PublishedArticles);
            }
            return View(views);
        }

        public ActionResult Search(string keyWords, int page = 1, int pageSize = 5)
        {
            List<ArticleModel> books = new List<ArticleModel>();

            books = PublishedArticles
                .Where(t => t.Title.Contains(keyWords))
                .GroupBy(t => t.Title)
                .Select(group => group.First()).ToList();

            PagedList<ArticleModel> views = new PagedList<ArticleModel>(books, page, pageSize);

            ViewBag.Count = books.Count();
            ViewBag.KeyWords = keyWords;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SearchResults", books);
            }
            return View("_SearchResults", views);
        }

        public ActionResult Details(int id)
        {
            var model = db.GetArticleById(id);
            if (!model.ContentUrl.IsNullOrEmptyOrWhiteSpace() && model.ContentUrl.IsUrl())
            {
                var link = model.ContentUrl;
                WebPage page = new WebPage(link);
                page.Load();
                if (!page.Title.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!page.IsGoogleDoc())
                    {
                        model.Title = page.Title;
                    }
                }
                if (!page.HtmlContent.IsNullOrEmptyOrWhiteSpace())
                {
                    //model.Content = page.GetNodesByClass("contents")
                    //    .FirstOrDefault()
                    //    .InnerHtml;
                    model.Content = page.GetNodeById("contents")
                        .InnerHtml;
                    //model.Content = page.HtmlContent;
                }
            }

            ViewBag.Title = model.Title;
            ArticleView view = Mappers.Mapper.ArticleToView(model);

            return View(view);
        }
        //public ActionResult Details(string contentUrl)
        //{
            
        //    var model = db.GetArticleByContentUrl(contentUrl);
        //    if (!model.ContentUrl.IsNullOrEmptyOrWhiteSpace() && model.ContentUrl.IsUrl())
        //    {
        //        var link = model.ContentUrl;
        //        WebPage page = new WebPage(link);
        //        page.Load();
        //        if (!page.Title.IsNullOrEmptyOrWhiteSpace())
        //        {
        //            if (!page.IsGoogleDoc())
        //            {
        //                model.Title = page.Title;
        //            }
        //        }
        //        if (!page.HtmlContent.IsNullOrEmptyOrWhiteSpace())
        //        {
        //            //model.Content = page.GetNodesByClass("contents")
        //            //    .FirstOrDefault()
        //            //    .InnerHtml;
        //            model.Content = page.GetNodeById("contents")
        //                .InnerHtml;
        //            //model.Content = page.HtmlContent;
        //        }
        //    }

        //    ViewBag.Title = model.Title;
        //    ArticleView view = Mappers.Mapper.ArticleToView(model);

        //    return View(view);
        //}
    }
}
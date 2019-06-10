using CafeT.Html;
using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using Web.ModelViews;

namespace Web.Mappers
{
    public static class Mapper
    {
        public static ArticleModel EntryToArticle(dynamic entryItem)
        {
            ArticleModel model = new ArticleModel();
            model.Id = int.Parse(GetFieldValue(entryItem, "Id"));
            model.Title = GetFieldValue(entryItem, "Title");
            model.Description = GetFieldValue(entryItem, "Description");
            model.ContentUrl = GetFieldValue(entryItem, "ContentUrl");
            model.Tags = GetFieldValue(entryItem, "Tags");
            model.CreatedBy = GetFieldValue(entryItem, "CreatedBy");
            model.CreatedDate = GetFieldValue(entryItem, "CreatedDate");
            model.IsPublished = bool.Parse(GetFieldValue(entryItem, "IsPublished"));
            model.MetaDescription = GetFieldValue(entryItem, "MetaDescription");
            model.MetaKeywords = GetFieldValue(entryItem, "MetaKeywords");
            model.DocumentId = GetFieldValue(entryItem, "DocumentId");
            model.ImageUrl = GetFieldValue(entryItem, "ImageUrl");

            return model;
        }
        #region View
        public static ArticleView ArticleToView(ArticleModel model)
        {
            ArticleView view = new ArticleView();
            view.Id = model.Id;
            view.CreatedDate = model.CreatedDate;
            view.Title = GetRawText(model.Title);
            view.Description = GetRawText(model.Description);


            view.Content = DecorHtml(model.Content);
            if (!model.Tags.IsNullOrEmptyOrWhiteSpace())
            view.Tags = model.Tags.Split(new char[] { ',', ';' }, StringSplitOptions.None);
            view.IsPublished = model.IsPublished;
            view.CreatedBy = model.CreatedBy;
            view.MetaKeywords = model.Tags + ";" + model.MetaKeywords;
            view.MetaDescription = model.MetaDescription;
            view.ImageUrl = model.ImageUrl;
            view.DocumentId = model.DocumentId;

            view.EditLink = "https://docs.google.com/document/d/"+ model.DocumentId +"/edit";
            return view;
        }

        public static List<ArticleView> ArticlesToViews(List<ArticleModel> models)
        {
            List<ArticleView> views = new List<ArticleView>();
            foreach (var model in models)
            {
                //var view = ArticleToView(model)
                views.Add(ArticleToView(model));
            }
            return views;
        }
        #endregion
        public static string GetFieldValue(dynamic item, string fieldName)
        {
            try
            {
                string value = (string)item["gsx$" + fieldName.ToLower()]["$t"].ToString();
                return value.DeleteFirst("\"").DeleteLastest("\"");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Because from Google Sheets, text has two char " in begin and end, must to remove
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetRawText(string text)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return text;
            string output = text; //string output = text.DeleteFirst("\"").DeleteLastest("\"");
            output = output.Replace("\n", "<br />");
            output = output.Replace("\\", "\"");
            return output;
        }

        public static string DecorHtml(string htmlText)
        {
            if (htmlText.IsNullOrEmptyOrWhiteSpace()) return htmlText;
            string html = string.Empty;
            html = htmlText.Replace("\n", "<br />");
            html = html.Replace("\\", "\"");
            html = htmlText.ResizeImages("100%", "auto");

            return html;
        }
    }
}
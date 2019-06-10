using CafeT.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeT.Html
{
    public class HtmlTable
    {
        public string Name { set; get; }
        public string HtmlContent { set; get; }
        public List<string> Rows { set; get; } = new List<string>();
        public List<string> Columns { set; get; } = new List<string>();
        public List<List<string>> Lines = new List<List<string>>();
        public HtmlTable(string htmlContent)
        {
            HtmlContent = htmlContent;
            GetRows();
        }

        public void GetRows()
        {
            HtmlDocument _doc = new HtmlDocument();
            _doc.LoadHtml(HtmlContent);
            var _rows = _doc.DocumentNode.SelectNodes("//table//tr");
            if (_rows != null && _rows.Count > 0)
            {
                
                Name = _rows[0].InnerText;
                foreach (var _row in _rows)
                {
                    Lines.Add(_row.ChildNodes.Select(t=>t.InnerText).ToList());
                    //foreach (var item in _row.ChildNodes)
                    //{
                    //    Lines.Add(item.InnerText);
                    //}
                    //Lines.Add(_row);
                    Rows.Add(_row.InnerText);
                }
            }
        }

        //public bool MaybeRate()
        //{
        //    if (Rows.Where(t => t.IsContainsNumber()).Count() > 3) return true;
        //    return false;
        //}
    }
}

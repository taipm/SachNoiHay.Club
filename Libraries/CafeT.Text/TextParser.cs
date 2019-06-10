using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public class TextParser
    {
        public string Text { set; get; } = string.Empty;
        public string[] Words { set; get; }
        public string[] Sentences { set; get; }
        public string[] Urls { set; get; }
        public bool IsParsered = false;

        public TextParser(string text)
        {
            Text = text;
            Parser();
        }

        public void Parser()
        {
            Words = Text.ToWords();
            Sentences = Text.ExtractSentences();
            Urls = Text.GetUrls();
            IsParsered = true;
        }
    }
}

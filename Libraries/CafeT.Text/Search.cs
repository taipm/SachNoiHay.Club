using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Text
{
    public static partial class Search
    {
        public static bool MatchSearch(this string text, string keyword)
        {
            bool findOrNot = false;
            if (text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                findOrNot = true;
            }
            else
            {
                findOrNot = false;
            }
            return findOrNot;
        }
    }
}

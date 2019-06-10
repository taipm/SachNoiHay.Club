namespace CafeT.Text
{
    public static class VietnameseTextHelper
    {
        public static string[] tokens = new string[] 
        {
            "â","ă",
            "đ",
            "ê",
            "ô","ơ",
            "ư"
        };

        public static bool IsVietnameseWord(this string text)
        {
            if (!text.IsWord()) return false;
            if (text.ContainsAny(tokens)) return true;
            return false;
        }

        public static bool IsDaily(this string text)
        {
            string[] DailyPatterns = new string[] { "daily", "hàng ngày" };
            foreach (string pattern in DailyPatterns)
            {
                if (text.ToLower().Contains(pattern))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

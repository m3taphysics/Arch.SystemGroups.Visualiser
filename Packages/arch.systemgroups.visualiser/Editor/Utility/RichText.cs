using System.Globalization;

namespace SystemGroups.Visualiser.Editor.Utility
{
    public static class RichText
    { 
        public static string Escape(string s) =>
        s?.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") ?? string.Empty;

        public static string HighlightMatch(string text, string query)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (string.IsNullOrEmpty(query)) return Escape(text);

            // Case-insensitive search using culture rules
            var idx = CultureInfo.CurrentCulture.CompareInfo.IndexOf(
                text, query, CompareOptions.IgnoreCase);

            if (idx < 0) return Escape(text);

            var before = Escape(text.Substring(0, idx));
            var match  = Escape(text.Substring(idx, query.Length));
            var after  = Escape(text.Substring(idx + query.Length));

            return $"{before}<b><u>{match}</u></b>{after}";
        }

    }
}
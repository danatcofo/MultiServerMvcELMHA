using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
    public static class ViewExtensionMethods
    {
        public static string GetRouteValue(this ViewContext view, string id)
        {
            return view.RouteData.GetRequiredString(id);
        }

        public static string BreakLongString(this string SubjectString, int CharsToBreakAfter)
        {
            string Pattern = "\\S{" + CharsToBreakAfter + ",}";
            int Counter = 0;
            bool IsMatching = Regex.IsMatch(SubjectString, Pattern);
            while (IsMatching)
            {
                Counter++;
                string MatchedString = Regex.Match(SubjectString, Pattern).Value;
                SubjectString = SubjectString.Replace(MatchedString.Substring(0, (CharsToBreakAfter - 1)), MatchedString.Substring(0, (CharsToBreakAfter - 1)) + " ");

                // Prevent endless loops
                if (Counter > 20) break;

                // Check if we still have long strings
                IsMatching = Regex.IsMatch(SubjectString, Pattern);
            }

            return SubjectString;
        }
    }
}
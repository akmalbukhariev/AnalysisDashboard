namespace AnalysisDashboard
{
    public static class StringHelper
    {
        public static string RemoveWhitespace(this string input)
        {
            if (input == null)
                input = "";
            else
                input = input.Trim();

            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static bool Checknull(this string input)
        {
            return input == null;
        }

        public static bool IsOk(this string input)
        {
            if (input == null) return false;
            if (input == "") return false;

            return true;
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }
}

namespace AnalysisDashboard.Helper
{ 
    public class Constants
    {
        public const string NotFound = "NotFound";
        public const string Exist = "User exist";
        public const string SaveUserImagePath = "/Upload/images/user/";
    }
}

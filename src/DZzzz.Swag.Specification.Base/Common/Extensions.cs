using System.Text.RegularExpressions;

namespace DZzzz.Swag.Specification.Base.Common
{
    public static class Extensions
    {
        public static string NormalizeName(this string value)
        {
            Regex regex = new Regex(@"/(?<middle>\w)");

            return regex.Replace(value, Evaluator);
        }

        private static string Evaluator(Match match)
        {
            if (match.Success)
            {
                return match.Groups["middle"].Value.ToUpper();
            }

            return match.Groups["middle"].Value;
        }
    }
}

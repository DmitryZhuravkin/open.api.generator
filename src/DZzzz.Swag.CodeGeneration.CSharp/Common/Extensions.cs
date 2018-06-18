using System.Text.RegularExpressions;

namespace DZzzz.Swag.CodeGeneration.CSharp.Common
{
    public static class Extensions
    {
        public static string ToCamelCase(this string value)
        {
            return $"{value[0].ToString().ToUpper()}{value.Substring(1, value.Length - 1)}";
        }

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
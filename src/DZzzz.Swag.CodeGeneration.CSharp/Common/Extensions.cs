using System;
using System.IO;

namespace DZzzz.Swag.CodeGeneration.CSharp.Common
{
    public static class Extensions
    {
        public static string ToCamelCase(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return $"{value[0].ToString().ToUpper()}{value.Substring(1, value.Length - 1)}";
        }

        public static void CreateDirectoryIfNotExists(this string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
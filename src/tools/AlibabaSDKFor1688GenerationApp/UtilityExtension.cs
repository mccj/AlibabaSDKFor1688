using NJsonSchema;
using System;
using System.Linq;

namespace ConsoleApp2
{
    public static class UtilityExtension
    {
        public static string 过滤特殊字符(this string str)
        {
            return str?.Replace("\b", "").Replace("\u001b", "");
        }
        ///// <summary>
        ///// CamelCase
        ///// PascalCase
        ///// </summary>
        ///// <param name="the_string"></param>
        ///// <returns></returns>
        //public static string 转换驼峰命名方式(this string the_string)
        //{
        //    var charlist = the_string.ToArray();
        //    for (int i = 0; i < charlist.Length; i++)
        //    {
        //        if (i == 0) { charlist[i] = char.ToUpper(charlist[i]); }
        //        else if (new[] { '.', '_' }.Contains(charlist[i]) && i <= charlist.Length - 1)
        //        {
        //            charlist[i + 1] = char.ToUpper(charlist[i + 1]);
        //        }
        //    }
        //    return new string(charlist).Replace(".", "").Replace("_", "");
        //}
        /// <summary>
        /// 转换大驼峰命名方式
        /// </summary>
        /// <param name="the_string"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(new[] { '.', '_'/*, '-', ' '*/ }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = new System.Text.StringBuilder();
            foreach (string word in words)
            {
                //if (word.All(f => char.IsUpper(f)))
                //    result.Append("_" + word);
                //else
                    result.Append(word.Substring(0, 1).ToUpper() + word.Substring(1));
            }

            if (char.IsNumber(result[0])) result.Insert(0, '_');
            return result.ToString();
        }
        /// <summary>
        /// 转换小驼峰命名方式
        /// </summary>
        /// <param name="the_string"></param>
        /// <returns></returns>
        // Convert the string to camel case.
        public static string ToCamelCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(new[] { '.', '_', '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = new System.Text.StringBuilder(words[0].ToLower());
            foreach (string word in words)
            {
                //if (word.All(f => char.IsUpper(f)))
                //    result.Append("_" + word);
                //else
                    result.Append(word.Substring(0, 1).ToUpper() + word.Substring(1));
            }

            if (char.IsNumber(result[0])) result.Insert(0, '_');
            return result.ToString();
        }

        // Capitalize the first character and add a space before
        // each capitalized letter (except the first character).
        public static string ToProperCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Start with the first character.
            var result = new System.Text.StringBuilder(the_string.Substring(0, 1).ToUpper());

            // Add the remaining characters.
            for (int i = 1; i < the_string.Length; i++)
            {
                if (char.IsUpper(the_string[i])) result.Append(" ");
                result.Append(the_string[i]);
            }

            return result.ToString();
        }
    }
}
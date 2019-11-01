using NJsonSchema;
using System.Linq;

namespace ConsoleApp2
{
    public static class UtilityExtension
    {
        public static string 过滤特殊字符(this string str)
        {
            return str?.Replace("\b", "").Replace("\u001b", "");
        }
        public static string 转换驼峰命名方式(this string name)
        {
            var charlist = name.ToArray();
            for (int i = 0; i < charlist.Length; i++)
            {
                if (i == 0) { charlist[i] = char.ToUpper(charlist[i]); }
                else if (charlist[i] == '.' && i <= charlist.Length - 1)
                {
                    charlist[i + 1] = char.ToUpper(charlist[i + 1]);
                }
            }
            return new string(charlist).Replace(".", "");
        }
    }
}
using System;

namespace WM
{
    class StringUtil
    {
        public static String Get3Digit(int value)
        {
            var result = value.ToString();

            while (result.Length < 3)
                result = "0" + result;

            return result;
        }
    }
}
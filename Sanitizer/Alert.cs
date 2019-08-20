using System;

namespace Sanitizer
{
    public class Alert
    {
        /// <summary>
        /// Alerts if malitious XSS Code is implemented
        /// script, Javascript, Jquery, onclick and malitious href
        /// </summary>
        /// <param name="item">HTML Code to proof</param>
        /// <returns>true if malitious Code</returns>
        public static bool ForScript(string item)
        {
            var scriptTest = item.IndexOf("<script", StringComparison.Ordinal);
            if (scriptTest != -1)
            {
                return true;
            }

            var functionTest = item.IndexOf("function(", StringComparison.Ordinal);
            var functionTest2 = item.IndexOf("function (", StringComparison.Ordinal);
            if (functionTest != -1 || functionTest2 != -1)
            {
                return true;
            }

            var jqueryTest = item.IndexOf("$(", StringComparison.Ordinal);
            if (jqueryTest != -1)
            {
                return true;
            }

            var functionIndex3 = item.IndexOf("function", StringComparison.Ordinal);
            if (functionIndex3 != -1)
            {
                var brakets = item.IndexOf("(", functionIndex3, StringComparison.Ordinal);
                var res = brakets - functionIndex3;
                if (res < 50 && brakets != -1)
                {
                    return true;
                }
            }

            var hrefTest1 = item.IndexOf("href=\"/", StringComparison.Ordinal);
            var hrefTest2 = item.IndexOf("href=\"~", StringComparison.Ordinal);
            var hrefTest3 = item.IndexOf("href=\"\\", StringComparison.Ordinal);
            if (hrefTest1 != -1 || hrefTest2 != -1 || hrefTest3 != -1)
            {
                return true;
            }

            var onclickTest = item.IndexOf("onclick=\"", StringComparison.Ordinal);
            if (onclickTest != -1)
            {
                return true;
            }

            return false;
        }
    }
}

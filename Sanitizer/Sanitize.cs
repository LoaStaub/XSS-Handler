using System;

namespace Sanitizer
{
    public class Sanitize
    {
        /// <summary>
        /// filters for Javascript function,
        /// strange href,
        /// onclick,
        /// for Jquery Eventhandlers
        /// and scriptsection
        /// </summary>
        /// <param name="item">html code to proof</param>
        /// <returns>Filtered String</returns>
        public static string ForScript(string item)
        {
            var scriptTest = item.IndexOf("<script", StringComparison.Ordinal);
            while (scriptTest != -1)
            {
                var scriptEnd = item.IndexOf("</script>", scriptTest, StringComparison.Ordinal);

                item = item.Remove(scriptTest, scriptEnd - scriptTest + 9);

                scriptTest = item.IndexOf("<script", StringComparison.Ordinal);
            }

            var js = 0;
            while (js == 0)
            {
                var functionIndex = item.IndexOf("function(", StringComparison.Ordinal);
                if (functionIndex != -1)
                {
                    item = ClearJavascriptFunction(item, functionIndex);
                }
                var functionIndex2 = item.IndexOf("function (", StringComparison.Ordinal);
                if (functionIndex2 != -1)
                {
                    item = ClearJavascriptFunction(item, functionIndex2);
                }


                if (functionIndex == -1 && functionIndex2 == -1)
                {
                    js++;
                }
            }


            var jqueryIndex = item.IndexOf("$(", StringComparison.Ordinal);
            while (jqueryIndex != -1)
            {
                item = ClearJqueryFunction(item, jqueryIndex);
                jqueryIndex = item.IndexOf("$(", StringComparison.Ordinal);
            }

            var functionIndex3 = item.IndexOf("function", StringComparison.Ordinal);
            while (functionIndex3 != -1)
            {
                var brakets = item.IndexOf("(", functionIndex3, StringComparison.Ordinal);
                var res = brakets - functionIndex3;
                if (res < 50 && brakets != -1)
                {
                    item = ClearJavascriptFunction(item, functionIndex3);
                    functionIndex3 = item.IndexOf("function", StringComparison.Ordinal);
                }
                else
                {
                    functionIndex3 = -1;
                }
            }

            var hrefIndex = item.IndexOf("href=\"/", StringComparison.Ordinal);
            while (hrefIndex != -1)
            {
                var hrefEnd = item.IndexOf("\"", StringComparison.Ordinal);
                item = item.Remove(hrefIndex, hrefEnd - hrefIndex);
                hrefIndex = item.IndexOf("href=\"/", StringComparison.Ordinal);
            }

            hrefIndex = item.IndexOf("href=\"~", StringComparison.Ordinal);
            while (hrefIndex != -1)
            {
                var hrefEnd = item.IndexOf("\"", StringComparison.Ordinal);
                item = item.Remove(hrefIndex, hrefEnd - hrefIndex);
                hrefIndex = item.IndexOf("href=\"~", StringComparison.Ordinal);
            }

            hrefIndex = item.IndexOf("href=\"\\", StringComparison.Ordinal);
            while (hrefIndex != -1)
            {
                var hrefEnd = item.IndexOf("\"", StringComparison.Ordinal);
                item = item.Remove(hrefIndex, hrefEnd - hrefIndex);
                hrefIndex = item.IndexOf("href=\"\\", StringComparison.Ordinal);
            }

            var onclickIndex = item.IndexOf("onclick=\"", StringComparison.Ordinal);
            while (onclickIndex != -1)
            {
                var onclickEnd = item.IndexOf("\"", onclickIndex + 10, StringComparison.Ordinal);
                item = item.Remove(onclickIndex, onclickEnd - onclickIndex + 1);
                onclickIndex = item.IndexOf("onclick=\"", StringComparison.Ordinal);
            }

            return item;
        }

        private static string ClearJavascriptFunction(string item, int position)
        {
            var index = 0;
            var locateFunctionStart = item.IndexOf("{", position, StringComparison.Ordinal);
            var possibleFunctionEnd = item.IndexOf("}", locateFunctionStart, StringComparison.Ordinal);
            var possibleBracketOpenInFunction = item.IndexOf("{", locateFunctionStart + 1, StringComparison.Ordinal);
            if (possibleBracketOpenInFunction < possibleFunctionEnd && possibleBracketOpenInFunction != -1)
            {
                index++;
                while (index != 0)
                {
                    possibleFunctionEnd = item.IndexOf("}", possibleFunctionEnd + 1, StringComparison.Ordinal);
                    possibleBracketOpenInFunction = item.IndexOf("{", possibleBracketOpenInFunction + 1, StringComparison.Ordinal);
                    if (possibleBracketOpenInFunction > possibleFunctionEnd || possibleBracketOpenInFunction == -1)
                    {
                        item = item.Remove(position, possibleFunctionEnd - position + 1);
                        index = 0;
                    }
                }
            }
            else
            {
                item = item.Remove(position, possibleFunctionEnd - position + 1);
            }

            return item;
        }

        private static string ClearJqueryFunction(string item, int position)
        {
            var index = item.IndexOf(");", position, StringComparison.Ordinal);
            item = item.Remove(position, index - position);
            return item;
        }
    }
}

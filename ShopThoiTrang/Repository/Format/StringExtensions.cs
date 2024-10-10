using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

public static class StringExtensions
{
    public static string TruncateHtml(this string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Loại bỏ các thẻ HTML
        string text = Regex.Replace(input, "<.*?>", string.Empty);

        // Giải mã các ký tự HTML (như &nbsp;)
        text = HttpUtility.HtmlDecode(text);

        // Cắt ngắn văn bản
        if (text.Length <= length)
            return text;

        return text.Substring(0, length) + "...";
    }
    
}
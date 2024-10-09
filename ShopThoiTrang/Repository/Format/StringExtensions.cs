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
    public static string FormatCurrency(int price)
    {
        // Tạo CultureInfo cho tiếng Việt
        CultureInfo cultureInfo = new CultureInfo("vi-VN");

        // Định dạng số theo kiểu tiền tệ Việt Nam
        string formattedAmount = price.ToString("N0", cultureInfo);

        // Thêm "VNĐ" vào cuối chuỗi
        return $"{formattedAmount} VNĐ";
    }
}
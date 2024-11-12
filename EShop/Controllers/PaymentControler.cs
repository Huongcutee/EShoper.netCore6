using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class PaymentControler : Controller
    {
        public IActionResult VnpayPayment()
        {
            // Logic để tạo request VNPAY và chuyển hướng đến trang thanh toán VNPAY
            string vnpayUrl = CreateVnpayPaymentUrl(); // Tạo URL thanh toán VNPAY
            return Redirect(vnpayUrl);
        }

        private string CreateVnpayPaymentUrl()
        {
            // Ở đây bạn cần cấu hình request đến VNPAY API để lấy URL thanh toán
            // Có thể sử dụng các thông tin như totalPrice, orderInfo,...
            string vnpayUrl = "https://pay.vnpay.vn/vpcpay.html"; // Ví dụ URL API VNPAY
                                                                  // Thêm các tham số cần thiết vào URL
            return vnpayUrl;
        }
    }
}

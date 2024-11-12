using EShop.Models;

namespace EShop.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponeModel PaymentExecute(IQueryCollection collections);
                
        
    }
}

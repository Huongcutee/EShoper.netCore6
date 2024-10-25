using EShop.Models;

namespace WEBAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<ProductModel> GetProducts();
        ProductModel GetProductByID(int id); // Thay đổi kiểu trả về thành ProductModel
        ProductModel CreateProduct(ProductModel product);
        ProductModel UpdateProduct(int id, ProductModel product);
        bool DeleteProduct(int id);
    }
}

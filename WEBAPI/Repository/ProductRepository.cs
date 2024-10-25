using EShop.Data;
using EShop.Models;
using WEBAPI.Interfaces;

namespace WEBAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ICollection<ProductModel> GetProducts()
        {
            return _dataContext.Products.OrderByDescending(p => p.Id).ToList();
        }
        public ProductModel GetProductByID(int id)
        {
            return _dataContext.Products.Find(id);
        }

        public ProductModel CreateProduct(ProductModel product)
        {
            _dataContext.Products.Add(product);
            _dataContext.SaveChanges();
            return product; 
        }

        public ProductModel UpdateProduct(int id, ProductModel product)
        {
            var existingProduct = _dataContext.Products.Find(id);
            if (existingProduct == null) return null; 

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Quantity = product.Quantity;
            existingProduct.BrandId = product.BrandId;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Images = product.Images;
            _dataContext.SaveChanges();
            return existingProduct;
        }

        public bool DeleteProduct(int id)
        {
            var product = _dataContext.Products.Find(id);
            if (product == null) return false; 

            _dataContext.Products.Remove(product);
            _dataContext.SaveChanges();
            return true; 
        }

    }
}

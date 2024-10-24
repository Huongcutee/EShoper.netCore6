namespace EShop.Models
{
    public class CartItemModel
    {
        public  int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Quantity {  get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal { 
            get {  return Price* Quantity; } 
        }

        public CartItemModel()
        {

        }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            Image = product.Images;
            ProductName = product.Name;
            Price = product.Price;
            Quantity = 1;

        }
        
    }
}

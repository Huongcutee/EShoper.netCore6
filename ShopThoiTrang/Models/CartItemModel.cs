namespace ShopThoiTrang.Models
{
    public class CartItemModel
    {
        public  long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Quanity {  get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal { 
            get {  return Price*Quanity; } 
        }

        public CartItemModel()
        {

        }
        public CartItemModel(ProductModel product)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Price = product.Price;
            Quanity = 1;

        }
        
    }
}

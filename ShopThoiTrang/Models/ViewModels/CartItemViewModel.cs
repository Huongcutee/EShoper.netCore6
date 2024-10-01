namespace ShopThoiTrang.Models.ViewModels
{
	public class CartItemViewModel
	{
		public List<CartItemModel> cartItems { get; set; }
		public decimal GrandTotal { get; set; }
	}
}

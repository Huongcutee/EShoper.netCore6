﻿namespace EShop.Models.ViewModels
{
	public class CartItemViewModel
	{
		public List<CartItemModel> cartItems { get; set; }
		public decimal GrandTotal { get; set; }
		public decimal ShippingPrice { get;set; }
		public decimal TotalPrice{ get; set; }
	}
}

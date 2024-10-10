using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
	public class OrderDetailModel
	{
		[Key]
		public int Id { get; set; }

		public string UserName { get; set; }

		public int OrderCode { get; set; }

		public int ProductId { get; set; }
		public decimal Price { get; set; }

		public int Quantity { get; set; }
		public OrderDetailModel()
		{
		}
		public OrderDetailModel(CartItemModel cartItem)
		{

			ProductId = cartItem.ProductId;
			Price = cartItem.Price;
			Quantity = cartItem.Quantity;
		}
	} 
}

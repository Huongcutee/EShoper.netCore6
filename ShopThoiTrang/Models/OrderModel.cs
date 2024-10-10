using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
	public class OrderModel
	{
		[Key]
		public int Id { get; set; }
		public string OrderCode { get; set; }

		public string UserName { get; set; }
		public decimal TotalPrice { get; set; }
		

		public DateTime CreateDate { get; set; }

		public int Status { get; set; }


    }
}

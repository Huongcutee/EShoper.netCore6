using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
	public class OrderModel
	{
		[Key]
		public int Id { get; set; }
		public string OrderCode { get; set; }

		public string UserName { get; set; }
		public decimal TotalPrice { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }


        public DateTime CreateDate { get; set; }

		public int Status { get; set; }

        [Required]
        public string phoneNumber { get; set; }
        // Liên kết với OrderDetailModel
        public ICollection<OrderDetailModel> OrderDetailModel { get; set; }

    }
}

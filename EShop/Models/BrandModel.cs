using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Vui lòng nhập tên thương hiệu")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required, MinLength(20, ErrorMessage = "Vui lòng nhập mô tả thương hiệu")]
		public string Description { get; set; }
		public int Status { get; set; }

	}
}

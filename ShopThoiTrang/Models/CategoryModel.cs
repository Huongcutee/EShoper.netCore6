using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Vui lòng nhập tên danh mục")]
		public string Name { get; set; }
		[Required]
		public string Slug { get; set; }
		[Required, MinLength(20, ErrorMessage = "Vui lòng nhập mô tả danh mục")]
		public string Description { get; set; }
		public int Status { get; set; }
	}
}

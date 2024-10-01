using System.ComponentModel.DataAnnotations;

namespace ShopThoiTrang.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required,MinLength(4, ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Vui lòng nhập thông tin danh mục sản phẩm")]
		public string Slug { get; set; }
		[Required, MinLength(20, ErrorMessage = "Vui lòng nhập thông tin mô tả sản phẩm")]

		public string Description { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập số lượng sản phẩm")]
		[Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn 0")]
		public int Quanity { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
		[Range(1, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
		public int Price { get; set; }
		public string Images { get; set; }

		[Required(ErrorMessage = "Vui lòng chọn thương hiệu sản phẩm")]
		public int BrandId { get; set; }

		[Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
		public int CategoryId { get; set; }
		public  CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }


	}
}

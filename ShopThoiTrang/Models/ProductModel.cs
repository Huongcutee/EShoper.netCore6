using ShopThoiTrang.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EShoper.netCore6.Repository.Validation;

public class ProductModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
    [MinLength(4, ErrorMessage = "Tên sản phẩm phải có ít nhất 4 ký tự")]
    public string Name { get; set; }

    public string Slug { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập thông tin mô tả sản phẩm")]
    [MinLength(20, ErrorMessage = "Mô tả sản phẩm phải có ít nhất 20 ký tự")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng sản phẩm")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
    [Range(1, int.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
    public int Price { get; set; }
   
    public string Images { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn thương hiệu sản phẩm")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
    public int CategoryId { get; set; }

    public CategoryModel Category { get; set; }
    public BrandModel Brand { get; set; }

    [NotMapped]
    [FileExtension(new string[] { ".jpg", ".png" })]
    public IFormFile? ImageUpload { get; set; }
}
using Microsoft.AspNetCore.Mvc;
using EShop.Models;
using WEBAPI.Interfaces;
using System.Collections.Generic;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductController : ControllerBase 
    {
        private readonly IProductRepository _productRepository;

        public ApiProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductModel>))]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(products);
        }

        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductModel))]
        [ProducesResponseType(404)]
        public IActionResult GetProductByID(int id)
        {
            var product = _productRepository.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductModel))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = _productRepository.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductByID), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProduct(int id, [FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = _productRepository.UpdateProduct(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        // Xóa sản phẩm
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int id)
        {
            var isDeleted = _productRepository.DeleteProduct(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

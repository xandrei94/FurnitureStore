using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FurnitureStore.DataAccess;
using FurnitureStore.DataAccess.Data;
using FurnitureStore.Models;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using FurnitureStore.Utility;

namespace FurnitureStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;
        public ProductAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetProducts")]
        public ActionResult<Product> GetProducts()
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            //foreach (var product in products) { product.Stock += 1000; }
            return Ok(products);
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product can not be created!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Request.Headers.TryGetValue("PseudoToken", out var pseudoTOken) || pseudoTOken != "552882c7-685f-4ecf-8c75-e81eefbc282f") return Unauthorized("Invalid token");
            if (product.Price == 0 || product.Price != product.ListPrice) product.Price = product.ListPrice;

            try
            {
                _unitOfWork.ProductRepository.Add(product);
                _unitOfWork.Save();

                return CreatedAtRoute("GetProducts", new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest("Product doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productToBeDeleted = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            if (productToBeDeleted == null) return StatusCode(404, $"Product not found");

            try
            {
                _unitOfWork.ProductRepository.Remove(productToBeDeleted);
                _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

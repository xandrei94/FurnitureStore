using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using FurnitureStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FurnitureStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public readonly IUnitOfWork _unitOfWork;
        string productAPI = APIConfig.ProductAPI;

        public ProductController(IHttpClientFactory clientFactory, IUnitOfWork unitOfWork)
        {
            _clientFactory = clientFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            //List<Product> objProductList = _db.GetAll().ToList();
            return View();

            //var httpClient = _clientFactory.CreateClient();
            //var response = await httpClient.GetAsync(productAPI);

            //if (response.IsSuccessStatusCode)
            //{
            //    var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            //    return View(products);
            //}
            //else
            //{
            //    return View("Error");
            //}
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        obj.ImageUrl = "";
        //        _db.Add(obj);
        //        _db.Save();
        //        TempData["success"] = "Product created successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                product.Price = product.ListPrice; //price no longer relevant
                product.ImageUrl = ""; //to implement later

                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("PseudoToken", "552882c7-685f-4ecf-8c75-e81eefbc282f");
                //httpClient.DefaultRequestHeaders.Add("Role", User.FindFirst(ClaimTypes.Role).Value);

                var jsonContent = JsonSerializer.Serialize(product);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{productAPI}/AddProduct/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Unauthorized: You are not authorized to perform this action.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Product productFromDb = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            if (productFromDb == null) return NotFound();
            return View(productFromDb);
        }

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        obj.ImageUrl = ""; //temporary 
        //        _db.Update(obj);
        //        _db.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                product.Price = product.ListPrice; //price no longer relevant
                //product.ImageUrl = "";

                var httpClient = _clientFactory.CreateClient();
                var jsonContent = JsonSerializer.Serialize(product);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{productAPI}/UpdateProduct/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var url = $"{productAPI}/DeleteProduct/{id}";

                var response = await httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Product deleted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            //if (id == null || id == 0) return NotFound();
            //Product productFromDb = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            //if (productFromDb == null) return NotFound();
            //return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var url = $"{productAPI}/DeleteProduct/{id}";

                var response = await httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Product deleted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            //Product obj = _db.Get(x => x.Id == id);
            //if (obj == null) return NotFound();

            //if (ModelState.IsValid)
            //{
            //    _unitOfWork.ProductRepository.Remove(obj);
            //    _unitOfWork.Save();
            //    TempData["success"] = "Product deleted successfully";
            //    return RedirectToAction("Index", "Product");
            //}
            //return View();
        }

        #region INTERNAL API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            return Json(new { data = objProductList });
        }
        #endregion
    }
}

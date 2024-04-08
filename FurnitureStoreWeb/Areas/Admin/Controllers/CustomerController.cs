using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FurnitureStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public readonly IUnitOfWork _unitOfWork;
        string routeCustomerAPI = APIConfig.CustomerAPI;
        string routeDiscountAPI = APIConfig.DiscountAPI;

        public readonly ApplicationDbContext _entity;

        public CustomerController(IHttpClientFactory clientFactory, IUnitOfWork unitOfWork, ApplicationDbContext entity)
        {
            _clientFactory = clientFactory;
            _unitOfWork = unitOfWork;
            _entity = entity;
        }
        public async Task<IActionResult> Index()
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("PseudoToken", "552882c7-685f-4ecf-8c75-e81eefbc282f");
            var requestUrl = $"{routeCustomerAPI}/GetCustomers";
            var response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var customerUsers = JsonConvert.DeserializeObject<List<CustomerUser>>(responseBody);
                return View(customerUsers);
            }
            else
            {
                TempData["error"] = "Could not load the data!";
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            //List<CustomerUser> objCUstomerLists = _unitOfWork.CustomerRepository.GetAll(includeProperties:"Discount").ToList();
            //return View(objCUstomerLists);
        }

        public IActionResult Create()
        {
            return View(); //can not create customers from anywhere else but Register now
            IEnumerable<SelectListItem> DiscountList = _unitOfWork.DiscountRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.DiscountList = DiscountList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerUser customer)
        {
            return View(); //can not create customers from anywhere else but Register 

            try
            {
                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("PseudoToken", "552882c7-685f-4ecf-8c75-e81eefbc282f");
                //httpClient.DefaultRequestHeaders.Add("Role", User.FindFirst(ClaimTypes.Role).Value);

                var jsonContent = JsonSerializer.Serialize(customer);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{routeCustomerAPI}/AddCustomer/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Customer created successfully";
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

        public IActionResult Edit(string id)
        {
            if (id == null) return NotFound();
            CustomerUser customerFromDb = _unitOfWork.CustomerRepository.Get(filter: x => x.Id == id, includeProperties: "Discount");
            if (customerFromDb == null) return NotFound();
            IEnumerable<SelectListItem> DiscountList = _unitOfWork.DiscountRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.DiscountList = DiscountList;
            return View(customerFromDb);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(CustomerUser customer)
        {
            var httpClient = _clientFactory.CreateClient();
            var jsonContent = JsonSerializer.Serialize(customer);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{routeCustomerAPI}/UpdateCustomer/", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Operation failed, product could not be updated!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var httpClient = _clientFactory.CreateClient();
            var url = $"{routeCustomerAPI}/DeleteCustomer/{id}/{userId}";
            var response = await httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Customer deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    TempData["error"] = "You can not delete the user currently logged!";
                }
                else
                {
                    TempData["error"] = "The customer could not be deleted";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(string id)
        {
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var url = $"{routeCustomerAPI}/DeleteCustomer/{id}";

                var response = await httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Customer deleted successfully";
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
    }
}

using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore.Models.ViewModels;
using FurnitureStore.Utility;
using FurnitureStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FurnitureStoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HomeController> _logger;
        public readonly IUnitOfWork _unitOfWork;
        public readonly ApplicationDbContext _db;
        string cartAPI = APIConfig.CartAPI;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext db, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _clientFactory = httpClientFactory;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var discount = _unitOfWork.CustomerRepository.Get(filter: x => x.Id == user, includeProperties: "Discount").Discount.Percentage;
                ViewBag.CustomerDiscount = discount;
            }

            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("PseudoToken", "552882c7-685f-4ecf-8c75-e81eefbc282f");
            var requestUrl = $"{cartAPI}/GetProducts";
            var response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var productsList = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                return View(productsList);
            }
            else
            {
                TempData["error"] = "The API could not fetch the data!";
                var objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
                return View(objProductList);
                //return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            //List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            //return View(objProductList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (quantity == 0)
            {
                TempData["error"] = "Quantitiy added was 0! Plase try again.";
                return RedirectToAction("Index");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("PseudoToken", "552882c7-685f-4ecf-8c75-e81eefbc282f");
            var requestUrl = $"{cartAPI}/AddToCart";

            var requestData = new CartInfoDTO { ProductId = id, Quantity = quantity, UserId = userId };
            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var cartItemsList = JsonConvert.DeserializeObject<List<CartItem>>(responseBody);
                return View(cartItemsList);
            }
            else
            {
                TempData["error"] = "Error! Product could not be added in the cart.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> ViewCart()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;

            var httpClient = _clientFactory.CreateClient();
            var requestUrl = $"{cartAPI}/ViewCart";

            var requestData = new CartInfoDTO { ProductId = 1, Quantity = 1, UserId = userId };
            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var cartItemsList = JsonConvert.DeserializeObject<List<CartItem>>(responseBody);
                return View("AddToCart", cartItemsList);
            }
            else
            {
                TempData["error"] = "Error! Product could not be added in the cart.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient();
            var requestUrl = $"{cartAPI}/RemoveFromCart";

            var requestData = new CartInfoDTO { ProductId = id, Quantity = 1, UserId = userId };
            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Product removed from the cart!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Error encountered when trying to remove a product!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CheckOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient();
            var requestUrl = $"{cartAPI}/Checkout";

            var requestData = new CartInfoDTO { ProductId = 1, Quantity = 1, UserId = userId };
            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Thank you for your purchese!";
                return RedirectToAction("Index");
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["error"] = "Your cart was empty. Add some products before trying to checkout!";
                }
                else
                {
                    TempData["error"] = "Something went wrong! Checkout incomplete.";
                }
                return RedirectToAction("Index");
            }
        }
    }
}

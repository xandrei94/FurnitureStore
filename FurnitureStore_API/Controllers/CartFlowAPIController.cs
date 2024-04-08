using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FurnitureStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartFlowAPIController : Controller
    {
        public readonly ApplicationDbContext _db;
        public readonly IUnitOfWork _unitOfWork;
        public CartFlowAPIController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
            return Ok(objProductList);
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(CartInfoDTO cartDTO)
        {
            var product = _unitOfWork.ProductRepository.Get(x => x.Id == cartDTO.ProductId);
            var discountId = _unitOfWork.CustomerRepository.Get(x => x.Id == cartDTO.UserId).DiscountId;
            var discountPercentage = _unitOfWork.DiscountRepository.Get(x => x.Id == discountId).Percentage;
            var discountedPrice = product.ListPrice - (product.ListPrice * ((double)discountPercentage / 100));

            var cartOfTheUser = _db.ShoppingCarts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CustomerId == cartDTO.UserId);
            if (cartOfTheUser == null)
            {
                var shoppingCart = new ShoppingCart
                {
                    CustomerId = cartDTO.UserId,
                    CreatedAt = DateAndTime.Now,
                    UpdatedAt = DateAndTime.Now,
                    CartItems = new List<CartItem>
                        {
                            new CartItem
                            {
                                ProductId = product.Id,
                                Quantity = cartDTO.Quantity,
                                Price = discountedPrice,
                                TotalPrice = cartDTO.Quantity * discountedPrice,
                                Status = CartItemStatus.Added.ToString(),
                                UpdatedAt = DateTime.Now
                            }
                        }

                };
                _db.Add(shoppingCart);
                _db.SaveChanges();
                cartOfTheUser = shoppingCart;
            }

            else
            {
                var productExists = cartOfTheUser.CartItems.FirstOrDefault(x => x.ProductId == cartDTO.ProductId && x.Status == CartItemStatus.Added.ToString());
                if (productExists == null)
                {
                    CartItem newCartItem = new CartItem
                    {
                        ProductId = product.Id,
                        Quantity = cartDTO.Quantity,
                        Price = discountedPrice,
                        TotalPrice = cartDTO.Quantity * discountedPrice,
                        Status = CartItemStatus.Added.ToString(),
                        UpdatedAt = DateTime.Now
                    };
                    cartOfTheUser.CartItems.Add(newCartItem);
                    _db.Update(cartOfTheUser);
                    _db.SaveChanges();
                }
                else
                {
                    //if new discount values applies compared to existing cart added
                    if (productExists.Price != discountedPrice)
                    {
                        CartItem newCartItem = new CartItem
                        {
                            ProductId = product.Id,
                            Quantity = cartDTO.Quantity,
                            Price = discountedPrice,
                            TotalPrice = cartDTO.Quantity * discountedPrice,
                            Status = CartItemStatus.Added.ToString(),
                            UpdatedAt = DateTime.Now
                        };
                        cartOfTheUser.CartItems.Add(newCartItem);
                        _db.Update(cartOfTheUser);
                        _db.SaveChanges();
                    }
                    else
                    {
                        productExists.Quantity += cartDTO.Quantity;
                        productExists.TotalPrice = productExists.Quantity * productExists.Price;
                        productExists.UpdatedAt = DateTime.Now;
                        _db.Update(cartOfTheUser);
                        _db.SaveChanges();
                    }
                }
            }

            var listOfCartItems = _db.CartItems.Where(x => x.ShoppingCartId == cartOfTheUser.Id && x.Status == CartItemStatus.Added.ToString()).ToList();
            var listCartItemDTO = listOfCartItems
                .Select(cartItem => new CartTransferDTO
                {
                    Id = cartItem.Id,
                    Product = new ProductDTO
                    {
                        Id = cartItem.Product.Id,
                        Name = cartItem.Product.Name,
                        Description = cartItem.Product.Description,
                        Category = cartItem.Product.Category,
                        ProductCode = cartItem.Product.ProductCode,
                        Supplier = cartItem.Product.Supplier,
                        Manufacturer = cartItem.Product.Manufacturer,
                        Price = cartItem.Product.Price,
                        IsAvailableForPurchase = cartItem.Product.IsAvailableForPurchase
                    },
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    TotalPrice = cartItem.TotalPrice,
                    ShoppingCartId = cartItem.ShoppingCartId,
                    Status = cartItem.Status,
                    UpdatedAt = cartItem.UpdatedAt
                }).ToList();
            return Ok(listCartItemDTO);
        }

        [HttpPost("ViewCart")]
        public IActionResult ViewCart(CartInfoDTO cartDTO)
        {
            var cartOfTheUser = _db.ShoppingCarts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CustomerId == cartDTO.UserId);
            if (cartOfTheUser == null)
            {
                return NotFound("Cart does not exist!");
            }
            else
            {
                var cartItems = cartOfTheUser.CartItems.Where(x => x.ShoppingCartId == cartOfTheUser.Id && x.Status == CartItemStatus.Added.ToString()).ToList();
                var listCartItemDTO = cartItems
                .Select(cartItem => new CartTransferDTO
                {
                    Id = cartItem.Id,
                    Product = new ProductDTO
                    {
                        Id = cartItem.Product.Id,
                        Name = cartItem.Product.Name,
                        Description = cartItem.Product.Description,
                        Category = cartItem.Product.Category,
                        ProductCode = cartItem.Product.ProductCode,
                        Supplier = cartItem.Product.Supplier,
                        Manufacturer = cartItem.Product.Manufacturer,
                        Price = cartItem.Product.Price,
                        IsAvailableForPurchase = cartItem.Product.IsAvailableForPurchase
                    },
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    TotalPrice = cartItem.TotalPrice,
                    ShoppingCartId = cartItem.ShoppingCartId,
                    Status = cartItem.Status,
                    UpdatedAt = cartItem.UpdatedAt
                }).ToList();
                return Ok(listCartItemDTO);
            }
        }

        [HttpPatch("RemoveFromCart")]
        public IActionResult RemoveFromCart(CartInfoDTO cartDTO)
        {
            var cartOfTheUser = _db.ShoppingCarts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefault(x => x.CustomerId == cartDTO.UserId);
            if (cartOfTheUser != null)
            {
                CartItem? itemToRemove = cartOfTheUser.CartItems.FirstOrDefault(x => x.Status == CartItemStatus.Added.ToString() && x.Id == cartDTO.ProductId);
                if (itemToRemove != null)
                {
                    itemToRemove.Status = CartItemStatus.Removed.ToString();
                    itemToRemove.UpdatedAt = DateTime.Now;
                    _db.Update(itemToRemove);
                    _db.SaveChanges();
                    return Ok();
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPatch("Checkout")]
        public IActionResult Checkout(CartInfoDTO cartDTO)
        {
            var cartOfTheUser = _db.ShoppingCarts.Include(x => x.CartItems.Where(y => y.Status == CartItemStatus.Added.ToString())).ThenInclude(x => x.Product).FirstOrDefault(x => x.CustomerId == cartDTO.UserId);
            if (cartOfTheUser != null)
            {
                if (cartOfTheUser.CartItems.Any())
                {
                    foreach (var cartItem in cartOfTheUser.CartItems)
                    {
                        cartItem.Status = CartItemStatus.Purchased.ToString();
                    }
                    _db.SaveChanges();
                    return Ok();
                }
                else return NotFound();
            }
            else return BadRequest();
        }
    }
}

using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountAPIController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public DiscountAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetDiscounts")]
        public ActionResult<Discount> GetCustomers()
        {
            List<Discount> discountList = _unitOfWork.DiscountRepository.GetAll().ToList();
            return Ok(discountList);
        }
    }
}

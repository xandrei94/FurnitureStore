using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FurnitureStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAPIController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public CustomerAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetCustomers")]
        public ActionResult<CustomerUser> GetCustomers()
        {
            List<CustomerUser> customerUsers = _unitOfWork.CustomerRepository.GetAll(includeProperties: "Discount").ToList();
            List<CustomerUserDTO> customerUsersDTO = customerUsers.Select(x => new CustomerUserDTO(x)).ToList();
            return Ok(customerUsersDTO);
        }

        [HttpGet("GetCustomersOnly5")]
        public ActionResult<CustomerUser> Get5Customers()
        {
            List<CustomerUser> objCUstomerLists = _unitOfWork.CustomerRepository.GetAll(includeProperties: "Discount").Take(5).ToList();
            return Ok(objCUstomerLists);
        }

        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer([FromBody] CustomerUser customer)
        {
            return BadRequest("Not supported at the moment");

            if (customer == null)
            {
                return BadRequest("Customer can not be created!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Request.Headers.TryGetValue("PseudoToken", out var pseudoTOken) || pseudoTOken != "552882c7-685f-4ecf-8c75-e81eefbc282f") return Unauthorized("Invalid token");

            try
            {
                _unitOfWork.CustomerRepository.Add(customer);
                _unitOfWork.Save();

                return CreatedAtRoute("GetProduct", new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] CustomerUser customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jsonContent = JsonSerializer.Serialize(customer);

            try
            {
                _unitOfWork.CustomerRepository.Update(customer);
                _unitOfWork.Save();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeleteCustomer/{id}/{userId}")]
        public IActionResult DeleteCustomer(string id, string userId)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Customer doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerToBeDeleted = _unitOfWork.CustomerRepository.Get(x => x.Id == id);
            if (customerToBeDeleted == null) return StatusCode(404, $"Product not found");

            //try
            //{
                if (customerToBeDeleted.Id == userId)
                {
                    return StatusCode(403, "You can not delete the user currently logged!");
                }
                else
                {
                    _unitOfWork.CustomerRepository.Remove(customerToBeDeleted);
                    _unitOfWork.Save();
                    return Ok("Customer deleted succesfully!");
                }
            //}
            //catch (Exception ex)
            //{
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }
    }
}

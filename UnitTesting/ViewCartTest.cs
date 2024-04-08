using System.Diagnostics.Contracts;
using System.Security.Claims;
using FurnitureStore.DataAccess.Data;
using FurnitureStore.DataAccess.Repository;
using FurnitureStore.DataAccess.Repository.IRepository;
using FurnitureStore.Models;
using FurnitureStore_API.Controllers;
using FurnitureStoreWeb.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Resource;
using FurnitureStore.Models.ViewModels;
using System.Linq.Expressions;

namespace UnitTesting
{
    public class ViewCartTest
    {
        [Fact]
        public async Task TestViewCart()
        {
            //actual code works but this does not because IHttpClientFactory...
            var userId = "f01ec7c0-2ca2-472e-8bb2-926f34c529b9";
            var claim = new Claim(ClaimTypes.NameIdentifier, userId);

            var cartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1, Quantity = 4, Price = 213, TotalPrice = 852, ShoppingCartId = 1, Status = CartItemStatus.Added.ToString(), UpdatedAt = DateTime.Now },
            new CartItem { ProductId = 2, Quantity = 3, Price = 680, TotalPrice = 2040, ShoppingCartId = 1, Status = CartItemStatus.Added.ToString(), UpdatedAt = DateTime.Now}
        };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var mockDbContext = new ApplicationDbContext(options))
            {
                mockDbContext.Database.EnsureCreated();

                var shoppingCart = new ShoppingCart { Id = 1, CustomerId = userId, CartItems = cartItems, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
                mockDbContext.ShoppingCarts.Add(shoppingCart);
                mockDbContext.SaveChanges();

                var mockUser = new Mock<ClaimsPrincipal>();
                var mockIdentity = new Mock<ClaimsIdentity>();

                mockIdentity.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, userId));
                mockUser.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(claim);
                mockUser.Setup(x => x.Identity.IsAuthenticated).Returns(true);

                var mockLogger = new Mock<ILogger<HomeController>>();
                var mockUnitOfWork = new Mock<IUnitOfWork>();

                var mockHttpClient = new Mock<HttpClient>();

                var mockHttpClientFactory = new Mock<IHttpClientFactory>();
                mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object).Verifiable();

                var controller = new HomeController(mockLogger.Object, mockUnitOfWork.Object, mockDbContext, mockHttpClientFactory.Object);
                controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = mockUser.Object } };

                mockHttpClientFactory.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once);

                var result = await controller.ViewCart();

                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.NotNull(viewResult);
                Assert.Equal("AddToCart", viewResult.ViewName);

                var model = Assert.IsAssignableFrom<List<CartItem>>(viewResult.Model);
                Assert.NotNull(model);
                Assert.Equal(cartItems.Count, model.Count);
            }
        }
    }
}
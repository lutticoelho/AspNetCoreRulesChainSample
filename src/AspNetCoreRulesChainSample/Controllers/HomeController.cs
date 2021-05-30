using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRulesChainSample.Models;
using AspNetCoreRulesChainSample.Rules.Chains;
using AspNetCoreRulesChainSample.Model;
using System;
using System.Collections.Generic;

namespace AspNetCoreRulesChainSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About([FromServices]ShoppingCartRulesChain shoppingCartRulesChain)
        {
            var shoppingCart = new ShoppingCart
            {
                CouponCode = "coupon-001",
                StartDate = DateTime.UtcNow,
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem{ Id = 1, Name = "Product 1", Price = 1.00m, Quantity = 2},
                    new ShoppingCartItem{ Id = 2, Name = "Product 2", Price = 2.00m, Quantity = 1},
                    new ShoppingCartItem{ Id = 3, Name = "Product 3", Price = 3.50m, Quantity = 1},
                }
            };

            shoppingCart = shoppingCartRulesChain.ApplyDiscountOnShoppingCart(shoppingCart);
            ViewData["Message"] = $"Your discount was applied by {shoppingCart.DiscountType} rule.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
    }
}

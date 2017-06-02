using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace eWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IIdentityParser<ApplicationUser> _identityParser;

        public CartController(IBasketService basketService, IIdentityParser<ApplicationUser> identityParser)
        {
            _basketService = basketService;
            _identityParser = identityParser;
        }

        public async Task<IActionResult> Index()
        {
            var user = _identityParser.Parse(HttpContext.User);
            var viewModel = await _basketService.GetBasket(user);


            return View(viewModel);
        }

        public IActionResult AddToCart(CatalogItem catalogItem)
        {
            if (catalogItem.Id > 0)
            {
                var user = _identityParser.Parse(HttpContext.User);
                var product = new BasketItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    UnitPrice = catalogItem.Price,
                    ProductId = catalogItem.Id
                };

                //await _basketService.AddItemToBasket(user, product);
            }

            return RedirectToAction("Index", "Catalog");
        }
    }
}
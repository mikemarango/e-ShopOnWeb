using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ApplicationCore.Interfaces;
using eWeb.Services;
using eWeb.Models.ViewModels;
using ApplicationCore.Exceptions;

namespace eWeb.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ICatalogService _catalogService;
        private readonly IImageService _imageService;
        private readonly IAppLogger<CatalogController> _appLogger;

        public CatalogController(IHostingEnvironment environment, ICatalogService catalogService, IImageService imageService, IAppLogger<CatalogController> appLogger)
        {
            _environment = environment;
            _catalogService = catalogService;
            _imageService = imageService;
            _appLogger = appLogger;
        }

        // GET: /<controller>
        public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page)
        {
            var itemsPage = 10;
            var catalog = await _catalogService.GetCatalogItemsAsync(page ?? 0, itemsPage, brandFilterApplied, typesFilterApplied);

            var vm = new CatalogIndex()
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogService.GetBrandsAsync(),
                Types = await _catalogService.GetTypesAsync(),
                BrandFilterApplied = brandFilterApplied ?? 0,
                TypesFilterApplied = typesFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = catalog.Data.Count,
                    TotalItems = catalog.Count,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)catalog.Count / itemsPage)).ToString())
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previouse = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(vm);
        }

        [HttpGet("[controller]/pic/{id}")]
        public IActionResult GetImage(int id)
        {
            byte[] imageBytes;
            try
            {
                imageBytes = _imageService.GetImageBytesById(id);
            }
            catch (CatalogImageMissingException ex)
            {
                _appLogger.LogWarning($"No image found for id: {id}", ex);
                return NotFound();
            }

            return File(imageBytes, "image/png");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
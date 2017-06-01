using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using eWeb.Data;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Entities;

namespace eWeb.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly CatalogContext _context;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings, ILoggerFactory loggerFactory)
        {
            _context = context;
            _settings = settings;
            _logger = loggerFactory.CreateLogger<CatalogService>();
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
        {
            _logger.LogInformation("GetBrands called.");
            var brands = await _context.CatalogBrands.ToListAsync();
            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogBrand brand in brands)
            {
                items.Add(new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand });
            }

            return items;
        }

        public async Task<Catalog> GetCatalogItemsAsync(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            _logger.LogInformation("GetCatalogItems called.");
            var root = (IQueryable<CatalogItem>)_context.CatalogItems;

            if (typeId.HasValue)
            {
                root = root.Where(ci => ci.CatalogTypeId == typeId);
            }

            if (brandId.HasValue)
            {
                root = root.Where(ci => ci.CatalogBrandId == brandId);
            }

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root
                .Skip(itemsPage * pageIndex)
                .Take(itemsPage).ToListAsync();

            return new Catalog() { Data = itemsOnPage, PageIndex = pageIndex, Count = (int)totalItems };
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            _logger.LogInformation("GetTypes called.");
            var types = await _context.CatalogTypes.ToListAsync();
            var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
            foreach (CatalogType type in types)
            {
                items.Add(new SelectListItem() { Value = type.Id.ToString(), Text = type.Type });
            }

            return items;
        }
    }
}

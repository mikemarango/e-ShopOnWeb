using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace eWeb.Services
{
    public class CachedCatalogService : ICatalogService
    {
        private readonly IMemoryCache _cache;
        private readonly CatalogService _catalogService;
        private static readonly string _brandsKey = "brands";
        private static readonly string _typesKey = "types";
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

        public CachedCatalogService(IMemoryCache cache, CatalogService catalogService)
        {
            _cache = cache;
            _catalogService = catalogService;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
        {
            return await _cache.GetOrCreateAsync(_brandsKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _catalogService.GetBrandsAsync();
            });
        }

        public async Task<Catalog> GetCatalogItemsAsync(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            string cacheKey = string.Format(_itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId);
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _catalogService.GetCatalogItemsAsync(pageIndex, itemsPage, brandId, typeId);
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            return await _cache.GetOrCreateAsync(_typesKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _catalogService.GetTypesAsync();
            });
        }
    }
}

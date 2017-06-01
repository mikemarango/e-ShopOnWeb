using eWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eWeb.Services
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItemsAsync(int pageIndex, int itemsPage, int? brandId, int? typeId);
        Task<IEnumerable<SelectListItem>> GetBrandsAsync();
        Task<IEnumerable<SelectListItem>> GetTypesAsync();
    }
}

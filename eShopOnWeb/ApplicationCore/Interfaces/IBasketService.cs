using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasket(ApplicationUser user);
    }

    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}

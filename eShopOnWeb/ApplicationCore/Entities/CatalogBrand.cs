using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class CatalogBrand : BaseEntity<int>
    {
        public string Brand { get; set; }
    }
}

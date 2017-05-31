using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class BasketItem : BaseEntity<string>
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
